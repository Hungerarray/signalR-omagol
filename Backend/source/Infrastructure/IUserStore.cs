using Omagol.Infrastructure.Data;

namespace Omagol.Infrastructure;

public interface IUserStore {
  public User this[string connectionId] { get; }

  public void Add(string connectionId, User user);
  public void Remove(string connectionId);
}