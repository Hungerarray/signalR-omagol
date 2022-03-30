import AppBar from "@mui/material/AppBar";
import Box from "@mui/material/Box";
import Toolbar from "@mui/material/Toolbar";
import IconButton from "@mui/material/IconButton";
import Typography from "@mui/material/Typography";
import Menu from "@mui/material/Menu";
import Container from "@mui/material/Container";
import Tooltip from "@mui/material/Tooltip";
import MenuItem from "@mui/material/MenuItem";
import MessageIcon from "@mui/icons-material/Message";
import ReorderIcon from '@mui/icons-material/Reorder';
import React from "react";

const pages = ["ChatRoom", "Oma Chat", "Oma Video"];

export const NavBar = () => {
  const [pageMenu, setPageMenu] = React.useState<null | HTMLElement>(
    null
  );

  const handleOpenUserMenu = (event: React.MouseEvent<HTMLElement>) => {
    setPageMenu(event.currentTarget);
  };

  const handleClosePageMenu = () => {
    setPageMenu(null);
  };

  return (
    <AppBar position="static">
      <Container maxWidth="xl">
        <Toolbar>
          <MessageIcon fontSize="large" sx={{ mr: 2 }}/>
          <Typography
            variant="h4"
            noWrap
            component="div"
            sx={{ mr: 2, display: { xs: "none", md: "flex" }, flex: 1 }}
          >
            Chat Room
          </Typography>

          <Box sx={{ flexGrow: 0 }}>
            <Tooltip title="Open rooms">
              <IconButton onClick={handleOpenUserMenu} sx={{ p: 0 }}>
                <ReorderIcon fontSize="large" sx={{ color: "#fff" }}/>
              </IconButton>
            </Tooltip>
            <Menu
              sx={{ mt: "45px" }}
              id="menu-appbar"
              anchorEl={pageMenu}
              anchorOrigin={{
                vertical: "top",
                horizontal: "right",
              }}
              keepMounted
              transformOrigin={{
                vertical: "top",
                horizontal: "right",
              }}
              open={Boolean(pageMenu)}
              onClose={handleClosePageMenu}
            >
              {pages.map((page) => (
                <MenuItem key={page} onClick={handleClosePageMenu} selected={page == "ChatRoom"}>
                  <Typography textAlign="center">{page}</Typography>
                </MenuItem>
              ))}
            </Menu>
          </Box>
        </Toolbar>
      </Container>
    </AppBar>
  );
};
