import { Box, CircularProgress, Paper, Typography } from "@mui/material"

export const Loading: React.FC = () => {

  return (
      <Paper elevation={4} sx={{
        height: 1,
        width: 1,
        display: "grid",
        placeItems: "center"
      }}>
        <Box sx={{
          display: "flex",
          flexDirection: "column",
          alignItems: "center"  
        }}>
          <Typography variant="h2">Waiting for connection ...</Typography>
          <CircularProgress thickness={5}/>
        </Box>
      </Paper>
  );
}