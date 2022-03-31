using Microsoft.AspNetCore.SignalR;
using Omagol.Data;

namespace Omagol.Hubs;

public class ChatRoom : Hub<IChatRoom> {
	private ILogger<ChatRoom> _logger { get; init; }

	public ChatRoom(ILogger<ChatRoom> logger) {
		_logger = logger;
	}

	public override Task OnConnectedAsync() {
    _logger.LogInformation($"{Context.ConnectionId} connected.");
		return base.OnConnectedAsync();
	}

	public override Task OnDisconnectedAsync(Exception? exception) {
    if(exception is not null) {
      _logger.LogError(exception, $"{Context.ConnectionAborted} disconnected with exception.");
    }

    _logger.LogInformation($"{Context.ConnectionId} disconnected.");
		return base.OnDisconnectedAsync(exception);
	}

	public async Task MessageSend(ChatMessage message) {
		message = message with { uuid = Context.ConnectionId };
		await Clients.Others.MessageReceive(message);
	}
}