import {Message} from './Message';
import { Paper } from '@mui/material'
import React, { useEffect, useMemo, useState } from 'react'
import { MessageList} from '../Infrastrcture/ChatRoomData';

interface Prop {
    messageList: MessageList[]
}
const ChatArea:React.FC<Prop> = ({messageList}) => {
    console.log(messageList);
    const [usersColor, setUsersColor] = useState<Record<string, string>>({}); 
    const randomColorGenerator = () => {
        return `hsl(${randomNumber(0, 365)},98%, 50%)`;
    }
    const randomNumber = (min:number, max:number) => {
        return Math.random() * (max - min) + min;
    }
    useEffect(() => {
        let temp:Record<string, string>= {};
        messageList.forEach(message => {
            if(!(message.uuid in usersColor)) temp[message?.uuid]= randomColorGenerator();
            else temp[usersColor[message.uuid]];
        })
        setUsersColor(temp);
    }, [messageList])
  return (
    <Paper elevation={4} sx={{
        height:1,
        width:1,
        p:2,
        overflowY:'scroll'
    }} >
       {messageList.map((eachMessage) =>  <Message message={eachMessage.message!} user={eachMessage.user} color={usersColor[eachMessage.uuid]}/>
       )}
    </Paper>
  )
}

export default ChatArea