using System.Threading.Tasks;
using FluentAssertions;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Omagol.Data;
using Omagol.Hubs;
using Xunit;

namespace Omagol.Tests;
public class ChatRoomTests : ChatRoomTestsBase {

	[Theory]
	[AutoData]
	public async Task Broadcasts_Message_with_Correct_UUID(
		ChatMessage message,
		string ConnectionId
	)
	{
		// Act
		await ChatRoom.MessageSend(message);

		// Assert
		await ChatRoom.Clients.Received(1).Others.MessageReceive(
			Arg.Do<ChatMessage>(x => {
				x.uuid.Should().Be(ConnectionId);
        x.Message.Should().Be(message.Message);
        x.User.Should().Be(message.User);
			}));
	}

}

public abstract class ChatRoomTestsBase {
	protected readonly ILogger<ChatRoom> Logger;
	protected readonly ChatRoom ChatRoom;

	protected ChatRoomTestsBase()
	{
		Logger = Substitute.For<ILogger<ChatRoom>>();
		ChatRoom = new ChatRoom(Logger);
		ChatRoom.Clients = Substitute.For<IHubCallerClients<IChatRoom>>();
		ChatRoom.Context = Substitute.For<HubCallerContext>();
	}

}