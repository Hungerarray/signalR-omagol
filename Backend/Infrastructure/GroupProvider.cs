
namespace Omagol.Infrastructure;

public class GroupProvider : IGroupProvider {
	public Task<string> this[string connectionId] => throw new NotImplementedException();

	public Task Register(string connectionId) {
		throw new NotImplementedException();
	}

	public Task UnRegister(string connectionId) {
		throw new NotImplementedException();
	}
}