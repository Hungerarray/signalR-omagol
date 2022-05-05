using Omagol.Infrastructure.Data;

namespace Omagol.Infrastructure;

public interface IGroupProvider {

  public string? this[User user] { get; }

  public Task Register(User user);

  public Task UnRegister(User user);

}