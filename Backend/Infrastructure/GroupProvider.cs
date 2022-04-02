using Omagol.Infrastructure.Data;
namespace Omagol.Infrastructure;

public class GroupProvider : IGroupProvider {

	private ILogger<GroupProvider> _logger { get; init; }

	private Queue<string> _availableConnections { get; } = new Queue<string>();

	private Dictionary<string, Group> _groupMap { get; } = new Dictionary<string, Group>();

	public GroupProvider(ILogger<GroupProvider> logger) {
		_logger = logger;
	}

	public Guid? this[string connectionId] {
		get {
			if (_groupMap.ContainsKey(connectionId))
				return _groupMap[connectionId].GroupId;
			return null;
		}
	}

	public event EventHandler<Group>? NewConnection;

	public void Register(string connectionId) {
		if (_availableConnections.Count == 0) {
			_availableConnections.Enqueue(connectionId);
			return;
		}

		var connection = _availableConnections.Dequeue();
		var connections = new string[] { connectionId, connection };
		var newGroupId = Guid.NewGuid();

		Group group = new Group(newGroupId, connections);
		_groupMap.Add(connection, group);
		_groupMap.Add(connectionId, group);
		NewConnection?.Invoke(this, group);

	}

	public void UnRegister(string connectionId) {
		_groupMap.Remove(connectionId, out Group? group);

		var secondPerson = group?.connections
														.Where(connection => connection != connectionId)
														.First();
		_groupMap.Remove(secondPerson!);
	}
}