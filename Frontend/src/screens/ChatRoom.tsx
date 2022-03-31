import { Box, Grid, IconButton, TextField, ThemeProvider } from "@mui/material";
import { NavBar } from "../components/navbar";
import { Pages } from "../Infrastrcture/PageEnums";
import SendIcon from "@mui/icons-material/Send";
import { PubTheme } from "../Infrastrcture/Themes";
import { ChangeEvent, useState } from "react";
import { useTextField } from "../components/textField";

export const ChatRoom = () => {
  const [username, setUsername] = useState<string>("");

  const handleUsernameChange = (event: ChangeEvent<HTMLInputElement>) => {
    const curr = event.target.value;
    if (curr.length > 10) return;
    setUsername(event.target.value);
  };

  const [message, handleMessageChange] = useTextField({length: 200});

  return (
    <>
      <ThemeProvider theme={PubTheme}>
        <NavBar pageType={Pages.ChatRoom} />
        <Grid container>
          <Grid item xs={0} md={1}></Grid>
          <Grid item container xs={12} md={10} spacing={1}>
            <Grid item xs={12}>
              <Box
                sx={{
                  backgroundColor: "#000",
                  height: "75vh",
                  flex: 1,
                  mt: 3,
                }}
              ></Box>
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
