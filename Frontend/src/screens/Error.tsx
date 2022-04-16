import { Paper as Box, Typography } from "@mui/material"

export const Error : React.FC = () => {

  return (
    <>
      <Box sx={{
        height: "100vh",
        display: "grid",
        placeItems: "center"
      }}>
        <Typography variant="h2">Oops! The page you requested doesn't exist.</Typography>
      </Box>
    </>
  ) 
}