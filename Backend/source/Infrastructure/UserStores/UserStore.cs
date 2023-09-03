using Omagol.Infrastructure.Data;

namespace Omagol.Infrastructure;

public class UserStore : IUserStore
{
	private IDictionary<string, User> _userStore { get; init; }

	public UserStore(IDictionary<string, User> userStore)
	{
		_userStore = userStore;
	}

	public User this[string connectionId]
	{
		get
		{
			return _userStore[connectionId];
		}
	}

	public void Add(User user)
	{
		_userStore.Add(user.ConnectionId, user);
	}

	public void Remove(string connectionId)
	{
		_userStore.Remove(connectionId);
	}

}