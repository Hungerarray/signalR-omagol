import { Box, Typography } from "@mui/material";
import React, { useState } from "react";

export enum MessageType {
  Message,
  Info
};

interface Prop {
  type?: MessageType,
  user: string,
  message: string,
  color: string
};

export const Message: React.FC<Prop> = ({ type = MessageType.Message, user, message, color = 'red' }) => {

  console.log(type, message);
  let display;
  switch (type) {
    case MessageType.Message:
      display = (
        <Box sx={{
          display: "flex",
          gap: 1
        }}>
          <Typography variant="subtitle1" color={color} fontWeight="bold">
            {user}:
          </Typography>
          <Typography variant="subtitle1">{message}</Typography>
        </Box>);
      break;
    case MessageType.Info:
      display = (
        <Box>
          <Typography variant="body2" color="#77809a" fontStyle="oblique">
            {message}
          </Typography>
        </Box>
      );
      break;
  }

  return display;
};
