import * as signalR from "@microsoft/signalr";
import { app } from "./Constants";

export const chatMoqData2 = [
  {
    "uuid": "123",
    "user": "Kiran Ghimire",
    "message": "Ma xada vae sake"
  },
  {
    "uuid": "123",
    "user": "Kiran Ghimire",
    "message": "Ah yar"
  },
  {
    "uuid": "124",
    "user": "Regmi C. Mahesh",
    "message": "last bigryo yar kiran"
  },
  {
    "uuid": "124",
    "user": "Regmi C. Mahesh",
    "message": "hait"
  }, 
]

export interface ChatMessage {
  user:string,
  message:string|null,
  uuid:string
}

export const ChatRoomConnection = new signalR.HubConnectionBuilder()
                                              .withUrl(`${app}/chatroom`)
                                              .build();