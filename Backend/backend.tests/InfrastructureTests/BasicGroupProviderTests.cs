using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.AspNetCore.SignalR;
using NSubstitute;
using Omagol.Hubs;
using Omagol.Infrastructure;
using Omagol.Infrastructure.Data;
using Xunit;

namespace Omagol.Tests;

public class BasicGroupProviderTests : BasicGroupProviderTestsBase {
	[Fact]
	public void Is_Initialized()
	{
		Storage.Should().NotBeNull();
		HubContext.Should().NotBeNull();
		HubContext.Clients.Should().NotBeNull();
		HubContext.Groups.Should().NotBeNull();
		GroupProvider.Should().NotBeNull();
	}

	[Theory]
	[AutoData]
	public void Returns_Null_For_User_Not_Registered(User user)
	{
		// Arrange
		var groups = Substitute.For<IDictionary<User, Group>>();
		groups.TryGetValue(user, out Arg.Any<Group?>()).Returns(x => {
			x[1] = null;
			return false;
		});
		Storage.Containers(Arg.Any<UserType>()).Returns((default, groups));

		// Act
		var returnValue = GroupProvider[user];

		// Assert
		returnValue.Should().BeNull();
	}

	[Theory]
	[AutoData]
	public void Returns_GroupId_For_Registered_User(User user, Group group)
	{
		// Arrange
		Groups.TryGetValue(user, out Arg.Any<Group?>()).Returns(x => {
			x[1] = group;
			return true;
		});
		Storage.Containers(Arg.Any<UserType>()).Returns((Connections, Groups));

		// Act
		var returnValue = GroupProvider[user];

		// Assert
		returnValue.Should().Be(group.GroupId);
    Connections.DidNotReceive();
	}

  [Theory]
  [AutoData]
  public async Task Adds_User_To_Collection_If_The_Only_Collection(User user)
  {
    // Arrange
    Storage.Containers(Arg.Any<UserType>()).Returns((Connections, Groups));

    // Act
    await GroupProvider.Register(user);

    // Assert
    Groups.DidNotReceive();
    Connections.Received(1).Add(user);
  }

  [Theory]
  [AutoData]
  public async Task Removes_First_User_And_Creates_Group(
    User user,
    ICollection<User> users,
    string groupId
  )
  {
    // Arrange

		// - returns first user
    var otherUser = users.First();
		var beforeCount = users.Count();
    Connections.GetEnumerator().Returns(users.GetEnumerator());
		Storage.Containers(Arg.Any<UserType>()).Returns((Connections, Groups));

		// - create group
		Generator.GetId().Returns<string>(groupId);
		Group group = new Group(groupId, new[] {user, otherUser});
    
		// Act
		await GroupProvider.Register(user);

		// Assert

		// - other user was removed from connections keeping others constant
		Connections.Received(1).Remove(Arg.Do<User>(x => x.Should().Be(otherUser)));
		Connections.DidNotReceive().Add(Arg.Any<User>());

		// - proper group was made
		Groups.Received(2).Add(
			Arg.Is<User>(x => UserCompare(x, user)
																					 || UserCompare(x, otherUser)), 
			Arg.Do<Group>(x => x.Should().Be(group))																				 
		);
		await HubContext.Groups.Received(2).AddToGroupAsync(
			Arg.Is<string>(x => x == user.ConnectionId || x == otherUser.ConnectionId),
			Arg.Is<string>(x => x == groupId)
		);
		await HubContext.Clients.Received(1).Group(Arg.Do<string>(x => x.Should().Be(groupId))).UserConnected();
  }

	[Theory]
	[AutoData]		
	public async Task UnRegister_Removes_From_Connections(User user)
	{
		// Arrange
		Groups.TryGetValue(user, out Arg.Any<Group?>()).Returns(x => {
			x[1] = null;
			return false;
		});
		Storage.Containers(Arg.Any<UserType>()).Returns((Connections, Groups));

		// Act
		await GroupProvider.UnRegister(user);

		// Assert
		Connections.Received(1).Remove(Arg.Is<User>(x => UserCompare(x, user)));
	}

	[Theory]
	[AutoData]
	public async Task UnRegister_Removes_From_Group(string groupId)
	{
		// Arrange
		Group group = new Group(
			groupId,
			fixture.CreateMany<User>(2)
		);
		var user = group.Users.First();
		var other = group.Users.Last();
		Groups.TryGetValue(user, out Arg.Any<Group?>()).Returns(x => {
			x[1] = group;
			return true;
		});
		Storage.Containers(Arg.Any<UserType>()).Returns((Connections, Groups));

		// Act
		await GroupProvider.UnRegister(user);

		// Assert
		Groups.Received(2).Remove(Arg.Is<User>(x => UserCompare(x, user) || UserCompare(x, other)));
		await HubContext.Groups.Received(2).RemoveFromGroupAsync(
			Arg.Is<string>(x => x == user.ConnectionId || x == other.ConnectionId),
			Arg.Is<string>(x => x == group.GroupId)
		);
	}
}

public abstract class BasicGroupProviderTestsBase {
	protected IStorageProvider Storage;
	protected IHubContext<OmagolRoom, IOmagol> HubContext;
	protected BasicGroupProvider GroupProvider;
  protected ICollection<User> Connections;
  protected IDictionary<User, Group> Groups;

	protected IGroupIdGenerator Generator;

	protected Fixture fixture;

	protected BasicGroupProviderTestsBase()
	{
		Storage = Substitute.For<IStorageProvider>();
		HubContext = Substitute.For<IHubContext<OmagolRoom, IOmagol>>();
    Connections = Substitute.For<ICollection<User>>();
    Groups = Substitute.For<IDictionary<User, Group>>();
		Generator = Substitute.For<IGroupIdGenerator>();
		GroupProvider = new BasicGroupProvider(
			HubContext,
			Storage,
			Generator
		);

		fixture = new Fixture();
	}

	protected bool UserCompare(User lhs, User rhs) 
	{
		return lhs.ConnectionId == rhs.ConnectionId && lhs.Type == rhs.Type;
	}
}