
using Omagol.Data;

namespace Omagol.Hubs;

public interface IOmagol {
  Task UserConnected();
  Task MessageReceive(OmaChat message);
  
}
  