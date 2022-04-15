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
import { Loading } from "../components/Loading";
import { ChatMessage } from "../Infrastrcture/Types";
import { MessageType } from "../components/Message";

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

  const reportInfo = (message :string) => {
    const info : ChatMessage = {
      type: MessageType.Info,
      user: "Internal",
      message: message,
      uuid: ""
    };

    setMessages(prevList => {
      return [ ... prevList, info];
    })
  }

  const userConnectedEventHandler = () => {
    console.log("user connected");
    setRoomState(RoomState.Connected);

    reportInfo("User has Connected");
  }

  const userDisconnectedEventHandler = () => {
    console.log("User Disconnected");
    setRoomState(RoomState.Disconnected);
    
    reportInfo("User has Disconnected");
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
  
  const begin = () => {
    start();
    setMessages([]);
  }

  useEffect(() => {
    setupConnection()
      .then(() => {
        subscribe("UserConnected", userConnectedEventHandler);
        subscribe("UserDisconnected", userDisconnectedEventHandler);
        subscribe("MessageReceive", handleReceiveEvent);
        begin();
        setRoomState(RoomState.Waiting);
      });

    return () => {
      destroyConnection();
    }
  }, [])

  const nextButtonHandler = () => {
    console.log("Next button Pressed");
    stop();
    begin();
    setRoomState(RoomState.Waiting);
  }

  let content = <Loading />;
  switch (roomState) {
    case RoomState.Initial:
    case RoomState.Waiting:
      content = <Loading />
      break;
    case RoomState.Connected:
    case RoomState.Disconnected:
      content = <ChatArea messageList={messages} />
      break;
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
                {content}
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
