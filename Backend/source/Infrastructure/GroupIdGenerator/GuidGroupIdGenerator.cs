namespace Omagol.Infrastructure;

public class GuidGroupIdGenerator : IGroupIdGenerator
{
	public string GetId()
	{
		return Guid.NewGuid().ToString();
	}
}