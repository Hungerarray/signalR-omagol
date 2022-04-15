import { Message } from "./Message";
import { Paper } from "@mui/material";
import React, { useEffect, useState } from "react";
import { ChatMessage } from "../Infrastrcture/Types";

interface Props {
  messageList: ChatMessage[];
}

const ChatArea: React.FC<Props> = ({ messageList }) => {
  let [usersColor, setUsersColor] = useState<Record<string, string>>({}) ;

  const randomColorGenerator = () => {
    return `hsl(${randomNumber(0, 365)},98%, 50%)`;
  };

  const randomNumber = (min: number, max: number) => {
    return Math.random() * (max - min) + min;
  };

  useEffect(() => {
    const updateRecord = (prevRecord : typeof usersColor) => {
      const setUniqueColors = (message : ChatMessage) => {
        if(message.uuid in prevRecord) 
          return;
        
        prevRecord[message.uuid] = randomColorGenerator();
      };

      messageList.forEach(setUniqueColors);

      return { ...prevRecord};
    };

    setUsersColor(updateRecord);
  }, [messageList]);

  return (
    <Paper
      elevation={4}
      sx={{
        height: 1,
        width: 1,
        p: 2,
        overflow: "auto",
      }}
    >
      {messageList.map((eachMessage, index) => (
        <Message
          type={eachMessage.type}
          message={eachMessage.message!}
          user={eachMessage.user}
          color={usersColor[eachMessage.uuid]}
          key={index}
        />
      ))}
    </Paper>
  );
};

export default ChatArea;
