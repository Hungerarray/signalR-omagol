using System.Security;
using Microsoft.AspNetCore.SignalR;
using Omagol.Data;
using Omagol.Infrastructure;
using Omagol.Infrastructure.Data;

namespace Omagol.Hubs;

public class OmagolRoom : Hub<IOmagol> {

	private ILogger<OmagolRoom> _logger { get; init; }
	private IGroupProvider _groupProvider { get; init; }

	private IUserStore _userStore { get; init; }

	public OmagolRoom(ILogger<OmagolRoom> logger, IGroupProvider groupProvider, IUserStore userStore) {
		_logger = logger;
		_groupProvider = groupProvider;
		_userStore = userStore;
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

		User currUser = _userStore[connectionId];
		_userStore.Remove(connectionId);
		string? groupId = _groupProvider[currUser];
		if (groupId is not null) {
			await Clients.GroupExcept(groupId, connectionId).UserDisconnected();
		}
		await _groupProvider.UnRegister(currUser);

		await base.OnDisconnectedAsync(exception);
	}

	public async Task MessageSend(OmaChat message) {
		var connectionId = Context.ConnectionId;

		User currUser = _userStore[connectionId];
		string? groupId = _groupProvider[currUser];
		if (groupId is null) {
			return;
		}
		await Clients.OthersInGroup(groupId).MessageReceive(message);
	}

	public async Task Start(UserInfo info) {
		string connectionId = Context.ConnectionId;

		User currUser = new User(connectionId, info.Video ? UserType.Video : UserType.Chat);
		await _groupProvider.Register(currUser);
	}

	public async Task Stop() {
		string connectionId = Context.ConnectionId;

		User currUser = _userStore[connectionId];
		_userStore.Remove(connectionId);
		string? groupId = _groupProvider[currUser];
		if (groupId is not null) {
			await Clients.GroupExcept(groupId, connectionId).UserDisconnected();
		}
		await _groupProvider.UnRegister(currUser);
	}
}
