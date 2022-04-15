import { MessageType } from "../components/Message";

export interface ChatMessage {
  type?: MessageType,
  user: string,
  message: string,
  uuid: string
};