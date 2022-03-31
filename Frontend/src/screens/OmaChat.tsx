import { ThemeProvider } from "@mui/material";
import { NavBar } from "../components/navbar"
import { Pages } from "../Infrastrcture/PageEnums";
import { OmaTheme } from "../Infrastrcture/Themes";


export const OmaChat = () => {

  return (
    <>
      <ThemeProvider theme={OmaTheme}>
        <NavBar pageType={Pages.OmaChat} /> 
      </ThemeProvider>

    </>
  );
}