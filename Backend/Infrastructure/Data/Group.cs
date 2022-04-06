namespace Omagol.Infrastructure.Data;

public record Group (string GroupId, IEnumerable<User> Users);