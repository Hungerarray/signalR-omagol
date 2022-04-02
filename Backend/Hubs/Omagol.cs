using Microsoft.AspNetCore.SignalR;
using Omagol.Data;
using Omagol.Infrastructure;

namespace Omagol.Hubs;

public class Omagol : Hub<IOmagol> {

	private ILogger<Omagol> _logger { get; init; }
	private IGroupProvider _groupProvider { get; init; }


	public Omagol(ILogger<Omagol> logger, IGroupProvider groupProvider) {
		_logger = logger;
		_groupProvider = groupProvider;
	}

	public override async Task OnConnectedAsync() {
		var connectionId = Context.ConnectionId;
		_logger.LogInformation($"{connectionId} connected.");

		await _groupProvider.Register(connectionId);
		await base.OnConnectedAsync();
	}

	public override async Task OnDisconnectedAsync(Exception? exception) {
		if (exception is not null) {
			_logger.LogError(exception, $"{Context.ConnectionAborted} disconnected with exception.");
		}
		var connectionId = Context.ConnectionId;
		_logger.LogInformation($"{connectionId} disconnected.");

		await _groupProvider.UnRegister(connectionId);
		await base.OnDisconnectedAsync(exception);
	}

	public async Task MessageSend(OmaChat message) {
		var connectionId = Context.ConnectionId;
		string groupId = await _groupProvider[connectionId];
		await Clients.OthersInGroup(groupId).MessageReceive(message);
	}
}