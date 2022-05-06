
using Omagol.Data;

namespace Omagol.Hubs;

public interface IOmagol {
  Task MessageReceive(OmaChat message);
  Task UserConnected();
  Task UserDisconnected();
}
  