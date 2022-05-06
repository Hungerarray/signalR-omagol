using Microsoft.AspNetCore.SignalR;
using Omagol.Hubs;
using Omagol.Infrastructure.Data;
namespace Omagol.Infrastructure;

public class GroupProvider : IGroupProvider {

	private readonly ILogger<GroupProvider> _logger;

	private readonly IStorageProvider _storage;
	private readonly IHubContext<OmagolRoom, IOmagol> _hubContext;

	public GroupProvider(
		IHubContext<OmagolRoom, IOmagol> hubContext,
		ILogger<GroupProvider> logger,
		IStorageProvider storage
	)
	{
		_logger = logger;
		_hubContext = hubContext;
		_storage = storage;
	}

	public string? this[User user] {
		get {
			var ( _ , groups) = _storage.Containers(user.Type);
			groups.TryGetValue(user, out Group? value);
			return value?.GroupId;
		}
	}

	public async Task Register(User user)
	{
		var (connections, groups) = _storage.Containers(user.Type);
		var otherUser = connections.FirstOrDefault();
		if (otherUser is null) {
			connections.Add(user);
			return;
		}
		connections.Remove(otherUser);

		Group group = new Group(
			GroupId: Guid.NewGuid().ToString(),
			Users: new[] { user, otherUser }
		);
		await CreateGroup(groups, group);

		await _hubContext.Clients.Group(group.GroupId).UserConnected();
	}

	private async Task CreateGroup(IDictionary<User, Group> groups, Group group)
	{
		foreach (User usr in group.Users) {
			groups.Add(usr, group);
			await _hubContext.Groups.AddToGroupAsync(usr.ConnectionId, group.GroupId);
		}
	}

	public async Task UnRegister(User user)
	{
		var (connections, groups) = _storage.Containers(user.Type);
		groups.TryGetValue(user, out Group? group);

		if (group is not null) {
			foreach (User usr in group.Users) {
				groups.Remove(usr);
				await _hubContext.Groups.RemoveFromGroupAsync(usr.ConnectionId, group.GroupId);
			}
			return;
		}

		connections.Remove(user);
	}
}