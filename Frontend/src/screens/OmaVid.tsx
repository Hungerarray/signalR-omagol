import { Box, Button, Grid, TextField, ThemeProvider } from "@mui/material"
import { KeyboardEventHandler, useEffect, useState } from "react"
import ChatArea from "../components/ChatArea"
import { Loading } from "../components/Loading"
import { MessageType } from "../components/Message"
import { NavBar } from "../components/navbar"
import { useTextField } from "../components/textField"
import { TEXTMESSAGE_LIMIT } from "../Infrastrcture/Constants"
import { destroyConnection, OmagolConnection, OmagolMessage, sendMessage, setupConnection, start, subscribe, stop } from "../Infrastrcture/Omagol"
import { Pages } from "../Infrastrcture/PageEnums"
import { OmaTheme } from "../Infrastrcture/Themes"
import { ChatMessage } from "../Infrastrcture/Types"
import { RoomState } from "./OmaChat"


export const OmaVid = () => {
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
    };
  }, []);

  const [message, handleMessageChange, clearMessage] = useTextField({
    length: TEXTMESSAGE_LIMIT
  });
  const [roomState, setRoomState] = useState<RoomState>(RoomState.Initial);
  const [messages, setMessages] = useState<ChatMessage[]>([]);
  const connection = OmagolConnection;

  const reportInfo = (message: string) => {
    const info: ChatMessage = {
      type: MessageType.Info,
      user: "Internal",
      message: message,
      uuid: ""
    };

    setMessages(prevList => {
      return [...prevList, info];
    });
  };

  const userConnectedEventHandler = () => {
    setRoomState(RoomState.Connected);
    reportInfo("User has connected");
  }

  const userDisconnectedEventHandler = () => {
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
      const msg: ChatMessage = {
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
    const msg: ChatMessage = {
      user: "Stranger",
      message: message.message,
      uuid: connection.connectionId!
    };

    setMessages(prevList => {
      return [...prevList, msg];
    });
  };

  const begin = () => {
    start();
    setMessages([]);
  };

  const nextButtonHandler = () => {
    stop();
    begin();
    setRoomState(RoomState.Waiting);
  };

  let content = <Loading />
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
        <NavBar pageType={Pages.OmaVideo} />
        <Grid container sx={{
          mt: 3
        }}>
          <Grid item xs={0} md={1} />
          <Grid item container xs={12} md={10} spacing={1} sx={{
            margin: {
              xs: 1,
              sm: 0
            }
          }}>
            <Grid item container xs={12} spacing={2}>
              <Grid item container xs={12} sm={4} spacing={2}>
                <Grid item xs={6} sm={12}>
                  <Box sx={{
                    backgroundColor: "#5f5f5f",
                    height: {
                      sm: 1,
                      xs: "50vh"
                    },
                  }}>
                    {/* First Video feed here */}
                  </Box>
                </Grid>
                <Grid item xs={6} sm={12}>
                  <Box sx={{
                    backgroundColor: "#5f5f5f",
                    height: {
                      sm: 1,
                      xs: "50vh"
                    },
                  }}>
                    {/* Second Video feed here */}
                  </Box>
                </Grid>
              </Grid>
              <Grid item xs={12} sm={8}>
                <Box sx={{
                  height: "75vh",
                }}>
                  {/* Chat area here */}
                  {content}
                </Box>
              </Grid>
            </Grid>
            <Grid item xs={12} sm={1} sx={{
              display: "grid",
              placeItems: "center"
            }}>
              <Button variant="contained">Test</Button>
            </Grid>
            <Grid item xs={12} sm={1} sx={{
              display: "grid",
              placeItems: "center"
            }}>
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
          </Grid>
          <Grid item xs={0} md={1} />
        </Grid>
      </ThemeProvider>
    </>
  )
}