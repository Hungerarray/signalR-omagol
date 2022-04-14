using Microsoft.AspNetCore.SignalR;
using Omagol.Hubs;
using Omagol.Infrastructure.Data;
namespace Omagol.Infrastructure;

public class GroupProvider : IGroupProvider {

	private ILogger<GroupProvider> _logger { get; init; }

	private Queue<User> _availableConnections { get; set; } = new Queue<User>();

	private Dictionary<User, Group> _groupMap { get; } = new Dictionary<User, Group>();
	private IHubContext<OmagolRoom, IOmagol> _hubContext { get; }

	public GroupProvider(IHubContext<OmagolRoom, IOmagol> hubContext, ILogger<GroupProvider> logger) {
		_logger = logger;
		_hubContext = hubContext;
	}

	public string? this[User user] {
		get {
			if (_groupMap.ContainsKey(user))
				return _groupMap[user].GroupId;
			return null;
		}
	}

	public void Register(User user) {
		if (_availableConnections.Count == 0) {
			_availableConnections.Enqueue(user);
			return;
		}

		User availableUser = _availableConnections.Dequeue();
		User[] users = new[] { user, availableUser };
		string newGroupId = Guid.NewGuid().ToString();

		Group group = new Group(newGroupId, users);
		_groupMap.Add(availableUser, group);
		_groupMap.Add(user, group);
		_hubContext.Clients.Group(group.GroupId).UserConnected();
	}

	public void UnRegister(User user) {
		_groupMap.TryGetValue(user, out Group? group);
		if (group is not null) {
			foreach (User usr in group.Users) {
				_groupMap.Remove(usr);
			}
			return;
		}

		var filteredQueue = _availableConnections
													.Where(usr => usr.ConnectionId != user.ConnectionId);
		_availableConnections = new Queue<User>(filteredQueue);
	}
}