import { Box, Typography } from "@mui/material";
import React, { useState } from "react";
interface Prop {
    user:string,
    message:string,
    color:string
}
export const Message:React.FC<Prop> = ({user, message, color='red'}) => {
    console.log('here');
  return (
    <Box sx={{
        display:"flex",
        gap:1
    }}>
      <Typography variant="subtitle1" color={color} fontWeight="bold">
        {user}:
      </Typography>
      <Typography variant="subtitle1">{message}</Typography>
    </Box>
  );
};
