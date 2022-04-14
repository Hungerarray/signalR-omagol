using Omagol.Infrastructure.Data;

namespace Omagol.Infrastructure;

public interface IGroupProvider {

  public string? this[User user] { get; }

  public void Register(User user);

  public void UnRegister(User user);

}