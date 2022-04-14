using Microsoft.AspNetCore.SignalR;
using Omagol.Data;
using Omagol.Infrastructure;
using Omagol.Infrastructure.Data;

namespace Omagol.Hubs;

public class OmagolRoom : Hub<IOmagol> {

	private ILogger<OmagolRoom> _logger { get; init; }
	private IGroupProvider _groupProvider { get; init; }

	private void ConnectionMade(object? sender, Group group) {
		Clients.Group(group.GroupId).UserConnected();
	}

	public OmagolRoom(ILogger<OmagolRoom> logger, IGroupProvider groupProvider) {
		_logger = logger;
		_groupProvider = groupProvider;
		_groupProvider.NewConnection += ConnectionMade;
	}

	public override async Task OnConnectedAsync() {
		var connectionId = Context.ConnectionId;
		_logger.LogInformation($"{connectionId} connected.");
		await base.OnConnectedAsync();
	}

	public override async Task OnDisconnectedAsync(Exception? exception) {
		if (exception is not null) {
			_logger.LogError(exception, $"{Context.ConnectionAborted} disconnected with exception.");
		}
		var connectionId = Context.ConnectionId;
		_logger.LogInformation($"{connectionId} disconnected.");

		User currUser = new User(connectionId);
		string? groupId = _groupProvider[currUser];
		if (groupId is not null) {
			await Clients.GroupExcept(groupId, connectionId).UserDisconnected();
		}
		_groupProvider.UnRegister(currUser);

		await base.OnDisconnectedAsync(exception);
	}

	public async Task MessageSend(OmaChat message) {
		var connectionId = Context.ConnectionId;

		User currUser = new User(connectionId);
		string? groupId = _groupProvider[currUser];
		if (groupId is null) {
			return;
		}
		await Clients.OthersInGroup(groupId).MessageReceive(message);
	}

	public void Start() {
		string connectionId = Context.ConnectionId;

		User currUser = new User(connectionId);
		_groupProvider.Register(currUser);
	}

	public void Stop() {
		string connectionId = Context.ConnectionId;

		User currUser = new User(connectionId);
		string? groupId = _groupProvider[currUser];
		if (groupId is not null) {
			Clients.GroupExcept(groupId, connectionId).UserDisconnected();
		}
		_groupProvider.UnRegister(currUser);
	}
}
