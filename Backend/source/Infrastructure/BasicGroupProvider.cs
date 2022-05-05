using Microsoft.AspNetCore.SignalR;
using Omagol.Hubs;
using Omagol.Infrastructure.Data;
namespace Omagol.Infrastructure;

public class GroupProvider : IGroupProvider {

	private ILogger<GroupProvider> _logger { get; init; }

	private Queue<User> _availableChatConnections { get; set; } = new Queue<User>();
	private Queue<User> _availableVideoConnections { get; set; } = new Queue<User>();

	private Dictionary<User, Group> _chatGroupMap { get; } = new Dictionary<User, Group>();
	private Dictionary<User, Group> _videoGroupMap { get; } = new Dictionary<User, Group>();

	private IHubContext<OmagolRoom, IOmagol> _hubContext { get; }

	public GroupProvider(IHubContext<OmagolRoom, IOmagol> hubContext, ILogger<GroupProvider> logger) {
		_logger = logger;
		_hubContext = hubContext;
	}

	public string? this[User user] {
		get {
			if (user.type == UserType.Chat && _chatGroupMap.ContainsKey(user))
				return _chatGroupMap[user].GroupId;
			else if (user.type == UserType.Video && _videoGroupMap.ContainsKey(user))
				return _videoGroupMap[user].GroupId;
			return null;
		}
	}

	public async Task Register(User user) {
		User availableUser;

		switch (user.type) {
			case UserType.Chat:
				if (_availableChatConnections.Count == 0) {
					_availableChatConnections.Enqueue(user);
					return;
				}
				availableUser = _availableChatConnections.Dequeue();
				break;
			case UserType.Video:
				if (_availableVideoConnections.Count == 0) {
					_availableVideoConnections.Enqueue(user);
					return;
				}
				availableUser = _availableVideoConnections.Dequeue();
				break;
			default:
				_logger.LogWarning("Possible fallthrough on additional UserType");
				availableUser = user;
				break;
		}

		User[] users = new[] { user, availableUser };
		string newGroupId = Guid.NewGuid().ToString();

		Group group = new Group(newGroupId, users);
		await CreateGroup(group);
		await _hubContext.Clients.Group(group.GroupId).UserConnected();
	}

	private async Task CreateGroup(Group group) {
		foreach (User usr in group.Users) {
			switch (usr.type) {
				case UserType.Chat:
					_chatGroupMap.Add(usr, group);
					break;
				case UserType.Video:
					_videoGroupMap.Add(usr, group);
					break;
			}
			await _hubContext.Groups.AddToGroupAsync(usr.ConnectionId, group.GroupId);
		}
	}

	public async Task UnRegister(User user) {

		Group? group = null;
		switch (user.type) {
			case UserType.Chat:
				_chatGroupMap.TryGetValue(user, out group);
				break;
			case UserType.Video:
				_videoGroupMap.TryGetValue(user, out group);
				break;
			default:
				_logger.LogWarning("Fallthrough on switch case of UserType");
				break;
		}

		if (group is not null) {
			foreach (User usr in group.Users) {
				switch (usr.type) {
					case UserType.Chat:
						_chatGroupMap.Remove(usr);
						break;
					case UserType.Video:
						_videoGroupMap.Remove(usr);
						break;
				}
				await _hubContext.Groups.RemoveFromGroupAsync(usr.ConnectionId, group.GroupId);
			}
			return;
		}

		switch (user.type) {
			case UserType.Chat:
				var filteredQueue = _availableChatConnections
														.Where(usr => usr.ConnectionId != user.ConnectionId);
				_availableChatConnections = new Queue<User>(filteredQueue);
				break;
			case UserType.Video:
				filteredQueue = _availableVideoConnections
														.Where(usr => usr.ConnectionId != user.ConnectionId);
				_availableVideoConnections = new Queue<User>(filteredQueue);
				break;
		}
	}
}