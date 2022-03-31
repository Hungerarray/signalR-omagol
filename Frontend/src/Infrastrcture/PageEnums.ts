export enum Pages {
  ChatRoom = "Chat Room",
  OmaChat = "Oma Chat",
  OmaVideo = "Oma Video"
}

export const Routes : Record<Pages, string> = {
  [Pages.ChatRoom]: "/chatroom",
  [Pages.OmaChat]: "/omachat",
  [Pages.OmaVideo]: "/omavideo"
}