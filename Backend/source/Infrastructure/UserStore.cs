using Omagol.Infrastructure.Data;

namespace Omagol.Infrastructure;

public class UserStore : IUserStore {
	private Dictionary<string, User> _userStore { get; init; }

	public UserStore() {
		_userStore = new Dictionary<string, User>();
	}

	public User this[string connectionId] {
		get {
			return _userStore[connectionId];
		}
	}

	public void Add(string connectionId, User user) {
		_userStore.Add(connectionId, user);
	}

	public void Remove(string connectionId) {
		_userStore.Remove(connectionId);
	}

}