using System.Collections.Concurrent;
using Omagol.Infrastructure.Data;

namespace Omagol.Infrastructure;
public interface IStorageProvider {
	(ICollection<User>, IDictionary<User, Group>) Containers(UserType type);
}