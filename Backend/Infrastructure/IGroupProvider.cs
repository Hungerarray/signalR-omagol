using Omagol.Infrastructure.Data;

namespace Omagol.Infrastructure;

public interface IGroupProvider {

  public Guid? this[string connectionId] { get; }

  public void Register(string connectionId);

  public void UnRegister(string connectionId);
	public event EventHandler<Group>? NewConnection;

}