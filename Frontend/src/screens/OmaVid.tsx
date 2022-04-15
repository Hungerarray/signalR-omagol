import { Box, Button, Grid, TextField, ThemeProvider } from "@mui/material"
import { useEffect } from "react"
import { NavBar } from "../components/navbar"
import { Pages } from "../Infrastrcture/PageEnums"
import { OmaTheme } from "../Infrastrcture/Themes"


export const OmaVid = () => {
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
                  backgroundColor: "#2f2f2f",
                  height: "75vh",
                }}>
                  {/* Chat area here */}
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
              <Button variant="contained">Next</Button>
            </Grid>
            <Grid item xs={12} sm={10}>
              <TextField
                label="Type your message here..."
                variant="outlined"
                fullWidth
                multiline
                maxRows={2}
              />
            </Grid>

          </Grid>
          <Grid item xs={0} md={1} />
        </Grid>
      </ThemeProvider>
    </>
  )
}