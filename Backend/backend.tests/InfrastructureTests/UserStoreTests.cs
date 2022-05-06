using System.Collections;
using Omagol.Infrastructure.Data;
using Omagol.Infrastructure;
using System.Collections.Generic;
using NSubstitute;
using Xunit;
using FluentAssertions;
using AutoFixture.Xunit2;

namespace Omagol.Tests;

public class UserStoreTests : UserStoreTestsBase
{
  [Fact]
  public void Is_Properly_Initialized()
  {
    Storage.Should().NotBeNull();
    Store.Should().NotBeNull();
  }

  [Theory]
  [AutoData]
  public void Returns_Correct_User(User user)
  {
    // Arrange
    Storage[user.ConnectionId].Returns(user);

    // Act
    var usr = Store[user.ConnectionId];

    // Assert
    usr.ConnectionId.Should().Be(user.ConnectionId);
    usr.Type.Should().Be(user.Type);
  }

  [Theory]
  [AutoData]
  public void Adds_User(User user)
  {
    // Act
    Store.Add(user);

    // Assert
    Storage.Received(1).Add(
      Arg.Do<string>(x => x.Should().Be(user.ConnectionId)),
      Arg.Do<User>(x => {
        x.ConnectionId.Should().Be(user.ConnectionId);
        x.Type.Should().Be(user.Type);
      })
    );
  }

  [Theory]
  [AutoData]
  public void Removes_proper_user(string connectionId)
  {
    // Act
    Store.Remove(connectionId);

    // Assert
    Storage.Received(1).Remove(
      Arg.Do<string>(x => x.Should().Be(connectionId))
    );
  }

}

public abstract class UserStoreTestsBase
{
  protected IDictionary<string, User> Storage;
  
  protected UserStore Store;

  protected UserStoreTestsBase()
  {
    Storage = Substitute.For<IDictionary<string, User>>();
    Store = new UserStore(Storage);
  }
}