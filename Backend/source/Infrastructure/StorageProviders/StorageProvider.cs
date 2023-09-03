using Omagol.Infrastructure.Data;

namespace Omagol.Infrastructure;

public class StorageFactory : IStorageFactory
{
	private readonly ICollection<User> ChatConnections;
	private readonly ICollection<User> VideoConnections;

	private readonly IDictionary<User, Group> ChatGroup;
	private readonly IDictionary<User, Group> VideoGroup;

	public StorageFactory(
		IDictionary<User, Group> videoGroup,
		IDictionary<User, Group> chatGroup,
		ICollection<User> chatConnections,
		ICollection<User> videoConnections
  )
	{
		VideoGroup = videoGroup;
		ChatGroup = chatGroup;
		ChatConnections = chatConnections;
		VideoConnections = videoConnections;
	}

	public (ICollection<User>, IDictionary<User, Group>) Containers(UserType type)
	{
		if (type == UserType.Chat)
			return (ChatConnections, ChatGroup);
		else if (type == UserType.Video)
			return (VideoConnections, VideoGroup);

		throw new ArgumentOutOfRangeException("Invalid type given");
	}
}