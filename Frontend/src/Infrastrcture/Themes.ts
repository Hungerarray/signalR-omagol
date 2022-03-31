import { createTheme } from "@mui/material";
import { blue, deepPurple } from "@mui/material/colors";

export const OmaTheme = createTheme({
  palette: {
    primary: {
      light: deepPurple[300],
      main: deepPurple[700],
      dark: deepPurple[900]
    }
  }
});

export const PubTheme = createTheme({
  palette: {
    primary: {
      light: blue[300],
      main: blue[700],
      dark: blue[900]
    }
  }
});