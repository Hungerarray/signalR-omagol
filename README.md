# signalR-omagol
using SignalR to create chat room and omagol

---
## Basic Architecture

![Basic Architecure](/basic-architecture.png)

We will use signalR technology from ASP.Net Core to allow for bi-directional communication between frontend and backend.

---
### Current Plans

We will have 2 endpoints
- `/chatroom`
- `/omagol`

--- 

### Chat Room
Any connection on `/chatroom` will be able to message all the users within the group.

This endpoint has 2 events we can use,
- `MessageSend` [Invoke]
- `MessageReceived` [Subscribe]

`MessageSend` is initated by client whenever any message has to be sent.
Subscribe to `MessageReceive` event to get notified of when any message has been sent by other chat room users.

> NOTE: `MessageReceive` event is triggered for every connection but the sender.

In both cases we send/receive a message object
#### Structure of message object
```json
{
  "uuid": "<value overwritten by server>",
  "user": "<user-name>",
  "message": "<message-sent>"
}
```

---

### Omagol

Any connection on omagol will be assigned a partner with whom they can converse anonymously with the other person.

Endpoint has events for when client is connected with other person or if the other party disconnects.
- `UserConnected` [Subscribe]
- `UserDisconnected` [Invoke]

> NOTE: unless `UserConnected` event is sent any message sent will be ignored.

This endpoint has a chat event api, and video event api.
Within Chat event api, we have 
- `MessageSend` [Invoke]
- `MessageReceive` [Subscribe]


> NOTE: `MessageReceive` is not triggered when message is sent by client.

#### Structure of message object

```json
{
  "message": "<message-sent>"
}
```

---

### Potential Plans

- Able to see all users on chatroom.
- User Avatar