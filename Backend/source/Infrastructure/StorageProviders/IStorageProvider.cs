using System.Collections.Concurrent;
using Omagol.Infrastructure.Data;

namespace Omagol.Infrastructure;
public interface IStorageFactory {
	(ICollection<User>, IDictionary<User, Group>) Containers(UserType type);
}