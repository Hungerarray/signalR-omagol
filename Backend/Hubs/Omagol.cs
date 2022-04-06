using Microsoft.AspNetCore.SignalR;
using Omagol.Data;
using Omagol.Infrastructure;
using Omagol.Infrastructure.Data;

namespace Omagol.Hubs;

public class Omagol : Hub<IOmagol> {

	private ILogger<Omagol> _logger { get; init; }
	private IGroupProvider _groupProvider { get; init; }

	private void ConnectionMade(object? sender, Group group) {
		Clients.Group(group.GroupId).UserConnected();
	}

	public Omagol(ILogger<Omagol> logger, IGroupProvider groupProvider) {
		_logger = logger;
		_groupProvider = groupProvider;
		_groupProvider.NewConnection += ConnectionMade;
	}

	public override async Task OnConnectedAsync() {
		var connectionId = Context.ConnectionId;
		_logger.LogInformation($"{connectionId} connected.");

		User newUser = new User(connectionId);
		_groupProvider.Register(newUser);
		await base.OnConnectedAsync();
	}


	public override async Task OnDisconnectedAsync(Exception? exception) {
		if (exception is not null) {
			_logger.LogError(exception, $"{Context.ConnectionAborted} disconnected with exception.");
		}
		var connectionId = Context.ConnectionId;
		_logger.LogInformation($"{connectionId} disconnected.");

		User user = new User(connectionId);
		_groupProvider.UnRegister(user);
		await base.OnDisconnectedAsync(exception);
	}

	public async Task MessageSend(OmaChat message) {
		var connectionId = Context.ConnectionId;

		User currUser = new User(connectionId);
		string? groupId = _groupProvider[currUser];
		if(groupId is null) {
			return ;
		}

		await Clients.OthersInGroup(groupId).MessageReceive(message);
	}
}