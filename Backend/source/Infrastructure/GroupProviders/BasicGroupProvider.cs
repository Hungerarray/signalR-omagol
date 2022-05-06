using Microsoft.AspNetCore.SignalR;
using Omagol.Hubs;
using Omagol.Infrastructure.Data;
namespace Omagol.Infrastructure;

public class BasicGroupProvider : IGroupProvider {

	private readonly IStorageProvider _storage;
	private readonly IHubContext<OmagolRoom, IOmagol> _hubContext;
	private readonly IGroupIdGenerator _generator;

	public BasicGroupProvider(
		IHubContext<OmagolRoom, IOmagol> hubContext,
		IStorageProvider storage,
		IGroupIdGenerator generator
	)
	{
		_hubContext = hubContext;
		_storage = storage;
		_generator = generator;
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
			GroupId: _generator.GetId(),
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