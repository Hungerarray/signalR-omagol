import { Route, Routes } from "react-router-dom";
import { ChatRoom } from "./screens/ChatRoom";
import { Error } from "./screens/Error";
import { OmaChat } from "./screens/OmaChat";
import { OmaVid } from "./screens/OmaVid";

function App() {

  return (
    <Routes>
      <Route path="/chatroom" element={<ChatRoom />} />
      <Route path="/omachat" element={<OmaChat />} />
{/*       <Route path="/omavideo" element={<OmaVid />} /> */}
      <Route path="/" element={<ChatRoom />} />
      <Route path="*" element={<Error />} />
    </Routes>
  );
}

export default App;
