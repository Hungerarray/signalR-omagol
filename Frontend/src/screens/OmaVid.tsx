import { ThemeProvider } from "@mui/material"
import { NavBar } from "../components/navbar"
import { Pages } from "../Infrastrcture/PageEnums"
import { OmaTheme } from "../Infrastrcture/Themes"


export const OmaVid = () => {

  return (
    <>
      <ThemeProvider theme={OmaTheme}>
        <NavBar pageType={Pages.OmaVideo} />
      </ThemeProvider>
    </>
  )
}