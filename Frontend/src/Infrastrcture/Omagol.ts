import * as signalR from "@microsoft/signalr";
import { HubConnectionState } from "@microsoft/signalr";

import { app } from "./Constants";

export interface OmagolMessage {
  message: string,
};

export const OmagolConnection = new signalR.HubConnectionBuilder()
  .withUrl(`${app}/omagol`)
  .build();

export const setupConnection = async () => {
  if (OmagolConnection.state === HubConnectionState.Disconnected) {
    await OmagolConnection.start();
  } else if (OmagolConnection.state === HubConnectionState.Disconnecting) {
    setInterval(setupConnection, 500);
  }
}

export const destroyConnection = async () => {
  if (OmagolConnection.state === HubConnectionState.Connected) {
    await OmagolConnection.stop();
  }
}

export const subscribe = (methodName: string, callback: (...arg: any[]) => void) => {
  OmagolConnection.on(methodName, callback);
}

export const start = async () => {
  console.log("Start event started");
  await OmagolConnection.invoke("Start");
}

export const sendMessage = async (message: OmagolMessage) => {
  await OmagolConnection.invoke("MessageSend", message);
}

export const stop = async () => {
  console.log("Stop Invoked");
  await OmagolConnection.invoke("Stop");
}
