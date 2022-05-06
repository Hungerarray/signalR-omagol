using System.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Omagol.Data;
using Omagol.Hubs;
using Omagol.Infrastructure;
using AutoFixture.Xunit2;
using MELT;
using Omagol.Infrastructure.Data;
using NSubstitute.ReturnsExtensions;

namespace Omagol.Tests;

public class OmagolRoomTests : OmagolRoomTestsBase {

	[Fact]
	public void Ensure_Everything_Setup_Properly()
	{
		Room.Should().NotBeNull();
		Room.Context.Should().NotBeNull();
		Room.Clients.Should().NotBeNull();
		LoggerFactory.Should().NotBeNull();
		Logger.Should().NotBeNull();
		GroupProvider.Should().NotBeNull();
		UserStore.Should().NotBeNull();
	}

	[Theory]
	[AutoData]
	public async Task Log_ConnectionID_On_Connection(string connectionId)
	{
		// arrange
		Room.Context.ConnectionId.Returns(connectionId);

		// Act
		await Room.OnConnectedAsync();

		// Assert
		var log = Assert.Single(LoggerFactory.Sink.LogEntries);
		log.LogLevel.Should().Be(LogLevel.Information);
		log.Message.Should().Contain(connectionId);
	}


	[Theory]
	[AutoData]
	public async Task Logs_On_Disconnect(
		Exception excep,
		CancellationToken abort,
		string connectedId
	)
	{
		// Arrange
		Room.Context.ConnectionAborted.Returns(abort);
		Room.Context.ConnectionId.Returns(connectedId);

		// Act
		await Room.OnDisconnectedAsync(excep);

		// Assert
		var logs = LoggerFactory.Sink.LogEntries.ToArray();
		logs.Should().HaveCount(2);
		
		// - check proper exception warning
		logs[0].LogLevel.Should().Be(LogLevel.Error);
		logs[0].Exception.Should().Be(excep);
		logs[0].Message.Should().Contain(abort.ToString());

		// - check disconnect logs
		logs[1].LogLevel.Should().Be(LogLevel.Information);
		logs[1].Message.Should().Contain(connectedId);
	}

	[Theory]
	[AutoData]
	public async Task Registers_User_On_Start(
		string connectionId,
		UserInfo info
	)
	{
		// Arrange
		Room.Context.ConnectionId.Returns(connectionId);
		UserType type = info.Video ? UserType.Video : UserType.Chat;
		User usr = new User(connectionId, type);

		// Act
		await Room.Start(info);

		// Assert
		UserStore.Received(1).Add(
			Arg.Do<User>(x => UserComparer(usr, x))
		);
		await GroupProvider.Received(1).Register(
			Arg.Do<User>(x => UserComparer(usr, x))
		);
	}

	[Theory]
	[AutoData]
	public async Task Remove_user_on_Stop(
		string connectionId,
		User usr
	)
	{
		// Arrange
		Room.Context.ConnectionId.Returns(connectionId);
		UserStore[connectionId].Returns(usr);
    GroupProvider[usr].ReturnsNull();

		// Act
		await Room.Stop();

		// Assert
		UserStore.Received(1).Remove(Arg.Do<string>(x => x.Should().Be(connectionId)));
		await GroupProvider.Received(1).UnRegister(Arg.Do<User>(x => UserComparer(usr, x)));
	}

	// [Theory]
	// [AutoData]
	// public async Task Sends_UserDisconnected_Event_On_Stop(
	// 	string connectionId,
	// 	User usr,
  //   string groupId
	// )
	// {
	// 	// Arrange
	// 	Room.Context.ConnectionId.Returns(connectionId);
	// 	UserStore[connectionId].Returns(usr);
  //   GroupProvider[usr].Returns(groupId);

	// 	// Act
	// 	await Room.Stop();

	// 	// Assert
	// 	UserStore.Received(1).Remove(Arg.Do<string>(x => x.Should().Be(connectionId)));
	// 	await GroupProvider.Received(1).UnRegister(Arg.Do<User>(x => UserComparer(usr, x)));
  //   var a = Room.Clients.Received(1).ReceivedCalls();
	// }

  [Theory]
  [AutoData]
  public async Task Does_Not_BroadCast_Message_If_Not_Assigned_Group(
		OmaChat message,
		string connectionId,
		User user
  )
  {
		// Arrange
		Room.Context.ConnectionId.Returns(connectionId);
		UserStore[connectionId].Returns(user);
		GroupProvider[user].ReturnsNull();

		// Act
		await Room.MessageSend(message);

		// Arrange
		Room.Clients.DidNotReceive();
  }


}

public abstract class OmagolRoomTestsBase {
	protected readonly ILogger<OmagolRoom> Logger;
	protected readonly IGroupProvider GroupProvider;
	protected readonly IUserStore UserStore;
	protected readonly OmagolRoom Room;
	protected ITestLoggerFactory LoggerFactory;
	protected OmagolRoomTestsBase()
	{
		LoggerFactory = TestLoggerFactory.Create();
		Logger = LoggerFactory.CreateLogger<OmagolRoom>();
		GroupProvider = Substitute.For<IGroupProvider>();
		UserStore = Substitute.For<IUserStore>();
		Room = new OmagolRoom(Logger, GroupProvider, UserStore);
		Room.Context = Substitute.For<HubCallerContext>();
    Room.Clients = Substitute.For<IHubCallerClients<IOmagol>>();
	}

	protected void UserComparer(User expected, User actual)
	{
		actual.ConnectionId.Should().Be(expected.ConnectionId);
		actual.Type.Should().Be(expected.Type);
	}

}