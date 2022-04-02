
namespace Omagol.Infrastructure;

public interface IGroupProvider {

  public Task<string> this[string connectionId] { get; }

  public Task Register(string connectionId);

  public Task UnRegister(string connectionId);

}