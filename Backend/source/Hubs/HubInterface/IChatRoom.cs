using Omagol.Data;

namespace Omagol.Hubs;

public interface IChatRoom
{
  Task MessageReceive(ChatMessage message);
}
