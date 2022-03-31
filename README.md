# signalR-omagol
using SignalR to create chat room and omagol

---
## Basic Architecture

![Basic Architecure](/basic-architecture.png)

We will use signalR technology from ASP.Net Core to allow for bi-directional communication between frontend and backend.

---
### Current Plans

We will have 3 endpoints
- `/chatroom`
- `/omachat`
- `/omavideo`

--- 

### Chat Room
Any connection on `/chatroom` will be able to message all the users within the group.

This endpoint has 2 events we can use,
- `MessageSend`
- `MessageReceived`

`MessageSend` is initated by client whenever any message has to be sent.
Subscribe to `MessageReceive` event to get notified of when any message has been sent by other chat room users.
In both cases we send/receive a message object
#### Structure of message object
```json
{
  'uuid': '<value overwritten by server>
  'user': '<user-name>',
  'message': '<message-sent>'
}
```

---
### Potential Plans

- Able to see all users on chatroom.