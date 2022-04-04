# Chat Room

Any connection on `/chatroom` will be able to message all the users within the group.

This endpoint has 2 events we can use,
- `MessageSend` [Invoke]
- `MessageReceived` [Subscribe]

`MessageSend` is initated by client whenever any message has to be sent.
Subscribe to `MessageReceive` event to get notified of when any message has been sent by other chat room users.

> NOTE: `MessageReceive` event is triggered for every connection but the sender.

In both cases we send/receive a message object

## Structure of message object
```json
{
  "uuid": "<value overwritten by server>",
  "user": "<user-name>",
  "message": "<message-sent>"
}
```

---