import { Box, Grid, IconButton, TextField, ThemeProvider } from "@mui/material";
import { NavBar } from "../components/navbar";
import { Pages } from "../Infrastrcture/PageEnums";
import SendIcon from "@mui/icons-material/Send";
import { PubTheme } from "../Infrastrcture/Themes";
import { useTextField } from "../components/textField";
import ChatArea from "../components/ChatArea";
import {
  chatMoqData2,
  ChatRoomConnection,
  ChatMessage,
} from "../Infrastrcture/ChatRoom";

import { TEXTMESSAGE_LIMIT, USERNAME_LIMIT } from "../Infrastrcture/Constants";
import { useEffect, useState } from "react";
import { HubConnectionState } from "@microsoft/signalr";

export const ChatRoom = () => {
  const [username, handleUsernameChange] = useTextField({
    length: USERNAME_LIMIT,
  });
  const [message, handleMessageChange, clearMessage] = useTextField({
    length: TEXTMESSAGE_LIMIT,
  });
  const [messages, setMessages] = useState<ChatMessage[]>([]);

  const connection = ChatRoomConnection;

  useEffect(() => {
    const setupConnection = async () => {
      if(connection.state) 
      await connection.start();
      connection.on("MessageReceive", receiveChatMessage);
    };
    setupConnection();

    return () => {
      const destroyConnection = async () => {
        await connection.stop();
      };
      destroyConnection();
    };
  }, []);

  const sendChatMessage = async (message: ChatMessage) => {
    await connection.send("MessageSend", message);
    setMessages(prevList => {
      return [...prevList, message];
    });
    clearMessage();
  };

  const receiveChatMessage = (message: ChatMessage) => {
    setMessages(prevList => {
      return [...prevList, message];
    });
  };

  const handleSendButton = () => {
    if (username && message) {
      sendChatMessage({
        uuid: connection.connectionId!,
        user: username,
        message: message,
      });
    }
  };
  return (
    <>
      <ThemeProvider theme={PubTheme}>
        <NavBar pageType={Pages.ChatRoom} />
        <Grid container>
          <Grid item xs={0} md={1}></Grid>
          <Grid item container xs={12} md={10} spacing={2}>
            <Grid item xs={12}>
              <Box
                sx={{
                  height: "75vh",
                  flex: 1,
                  mt: 3,
                }}
              >
                <ChatArea messageList={messages} />
              </Box>
            </Grid>
            <Grid item xs={12} md={2}>
              <TextField
                label="Username"
                variant="outlined"
                fullWidth
                value={username}
                onChange={handleUsernameChange}
              />
            </Grid>
            <Grid item xs={12} sm={9}>
              <TextField
                label="Type your message here..."
                variant="outlined"
                fullWidth
                multiline
                maxRows={2}
                value={message}
                onChange={handleMessageChange}
              />
            </Grid>
            <Grid
              item
              sm={1}
              sx={{
                display: { xs: "none", sm: "flex" },
                alignItems: "center",
                justifyContent: "center",
              }}
            >
              <IconButton
                size="large"
                color="primary"
                onClick={handleSendButton}
              >
                <SendIcon fontSize="large" />
              </IconButton>
            </Grid>
          </Grid>
          <Grid item xs={0} sm={1}></Grid>
        </Grid>
      </ThemeProvider>
    </>
  );
};
