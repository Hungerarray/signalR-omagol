namespace Omagol.Infrastructure.Data;

public record Group (Guid GroupId, IEnumerable<string> connections);