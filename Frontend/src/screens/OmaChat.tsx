import {
  Box,
  Button,
  Grid,
  IconButton,
  TextField,
  ThemeProvider,
} from "@mui/material";
import { NavBar } from "../components/navbar";
import { Pages } from "../Infrastrcture/PageEnums";
import { OmaTheme } from "../Infrastrcture/Themes";
import SendIcon from "@mui/icons-material/Send";
import { useTextField } from "../components/textField";
import { TEXTMESSAGE_LIMIT } from "../Infrastrcture/Constants";
import { destroyConnection, OmagolConnection, OmagolMessage, sendMessage, setupConnection, start, subscribe, stop } from "../Infrastrcture/Omagol";
import { KeyboardEventHandler, useEffect, useState } from "react";
import ChatArea from "../components/ChatArea";
import { ChatMessage } from "../Infrastrcture/ChatRoom";

enum RoomState {
  Initial,
  Waiting,
  Connected,
  Disconnected
};

export const OmaChat = () => {
  const [message, handleMessageChange, clearMessage] = useTextField({
    length: TEXTMESSAGE_LIMIT,
  });
  const [roomState, setRoomState] = useState<RoomState>(RoomState.Initial);
  const [messages, setMessages] = useState<ChatMessage[]>([]);

  const connection = OmagolConnection;

  const userConnectedEventHandler = () => {
    console.log("user connected");
    setRoomState(RoomState.Connected);
  }

  const userDisconnectedEventHandler = () => {
    console.log("User Disconnected");
    setRoomState(RoomState.Disconnected);
  }

  const handleEnterEvent: KeyboardEventHandler<HTMLDivElement> = (event) => {
    if (event.key === "Enter" && event.shiftKey === false) {
      handleSendEvent();
      event.preventDefault();
    }
  }

  const handleSendEvent = async () => {
    if (message && roomState === RoomState.Connected) {
      const msg = {
        user: "You",
        message: message,
        uuid: connection.connectionId!
      };
      await sendMessage(msg);
      setMessages(prevList => {
        return [...prevList, msg];
      });
      clearMessage();
    }
  }

  const handleReceiveEvent = (message: OmagolMessage) => {
    const msg = {
      user: "Stranger",
      message: message.message,
      uuid: connection.connectionId!
    };

    setMessages(prevList => {
      return [...prevList, msg];
    });
  }

  useEffect(() => {
    setupConnection()
      .then(() => {
        subscribe("UserConnected", userConnectedEventHandler);
        subscribe("UserDisconnected", userDisconnectedEventHandler);
        subscribe("MessageReceive", handleReceiveEvent);
        start();
        setRoomState(RoomState.Waiting);
      });

    return () => {
      destroyConnection();
    }
  }, [])

  const nextButtonHandler = () => {
    console.log("Next button Pressed");
    stop();
    start();
    setRoomState(RoomState.Waiting);
  }

  return (
    <>
      <ThemeProvider theme={OmaTheme}>
        <NavBar pageType={Pages.OmaChat} />
        <Grid container>
          <Grid item xs={0} md={1}></Grid>
          <Grid item container xs={12} md={10} spacing={1}>
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
            <Grid
              item
              xs={12}
              sm={1}
              sx={{
                display: "flex",
                justifyContent: "center",
                alignItems: "center",
              }}
            >
              <Button variant="contained" onClick={nextButtonHandler}>Next</Button>
            </Grid>
            <Grid item xs={12} sm={10}>
              <TextField
                label="Type your message here..."
                variant="outlined"
                fullWidth
                multiline
                maxRows={2}
                value={message}
                onChange={handleMessageChange}
                onKeyDownCapture={handleEnterEvent}
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
              <IconButton size="large" color="primary">
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
