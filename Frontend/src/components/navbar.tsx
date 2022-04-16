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
import ReorderIcon from "@mui/icons-material/Reorder";
import React from "react";
import { SvgIconProps, SvgIconTypeMap } from "@mui/material";
import { OndemandVideo } from "@mui/icons-material";
import { Pages, Routes } from "../Infrastrcture/PageEnums";
import { Link } from "react-router-dom";

// const pages = [Pages.ChatRoom, Pages.OmaChat, Pages.OmaVideo];
const pages = [Pages.ChatRoom, Pages.OmaChat];

interface Props {
  pageType: Pages;
}

export const NavBar: React.FC<Props> = ({ pageType }) => {
  const [pageMenu, setPageMenu] = React.useState<null | HTMLElement>(null);

  const handleOpenUserMenu = (event: React.MouseEvent<HTMLElement>) => {
    setPageMenu(event.currentTarget);
  };

  const handleClosePageMenu = () => {
    setPageMenu(null);
  };

  let icon: React.ReactElement<SvgIconProps> | null = null;
  if (pageType === Pages.ChatRoom || pageType === Pages.OmaChat)
    icon = <MessageIcon fontSize="large" sx={{ mr: 2 }} />;
  else if (pageType === Pages.OmaVideo)
    icon = <OndemandVideo fontSize="large" sx={{ mr: 2 }} />;

  return (
    <AppBar position="static">
      <Container maxWidth="xl">
        <Toolbar>
          {icon}
          <Typography
            variant="h4"
            noWrap
            component="div"
            sx={{ mr: 2, display: { md: "flex" }, flex: 1 }}
          >
            {pageType}
          </Typography>

          <Box sx={{ flexGrow: 0 }}>
            <Tooltip title="Open rooms">
              <IconButton onClick={handleOpenUserMenu} sx={{ p: 0 }}>
                <ReorderIcon fontSize="large" sx={{ color: "#fff" }} />
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
                <MenuItem
                  key={page}
                  onClick={handleClosePageMenu}
                  selected={page == pageType}
                  component={Link}
                  to={Routes[page]}
                >
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
