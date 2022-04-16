namespace Omagol.Infrastructure.Data;

public enum UserType {
  Chat,
  Video
}

public record User(string ConnectionId, UserType type);
