import { ThemeProvider } from "@mui/material";
import { NavBar } from "../components/navbar"
import { Pages } from "../Interface/PageEnums";
import { OmaTheme } from "../Interface/Themes";


export const OmaChat = () => {

  return (
    <>
      <ThemeProvider theme={OmaTheme}>
        <NavBar pageType={Pages.OmaChat} /> 
      </ThemeProvider>

    </>
  );
}