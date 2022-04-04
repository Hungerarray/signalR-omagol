# Omagol

Any connection on omagol will be assigned a partner with whom they can converse anonymously with the other person.

```mermaid
sequenceDiagram
  autonumber
  participant c1 as Client 1
  participant b as Backend
  participant c2 as Client 2
  c1 -->> b : Connected(implicit)
  c2 -->> b : Connected(implicit)
  c1 ->> b : Start 
  c2 ->> b : Start
  par Backend to Client 1
    b ->> c1 : UserConnected
  and Backend to Client 2
    b ->> c2 : UserConnected
  end
  rect rgb(49, 18, 98, 0.5)
    c1 -->> b : MessageSend
    b -->> c2 : MessageReceive 
    c2 -->> b : MessageSend
    c2 -->> b : MessageSend
    b -->> c1 : MessageReceive 
    b -->> c1 : MessageReceive 
  end
  c2 -->> b : Disconnect(implicit)
  b ->> c1 : UserDisconnected
  Note right of c1 : Restart with Start event
  c1 -->> b : Disconnect(implicit)
```
Endpoint has events for when client is connected with other person or if the other party disconnects.
- `UserConnected` [Subscribe]
- `UserDisconnected` [Invoke]

> NOTE: unless `UserConnected` event is sent any message sent will be ignored.

This endpoint has a chat event api, and video event api.
Within Chat event api, we have 
- `MessageSend` [Invoke]
- `MessageReceive` [Subscribe]


> NOTE: `MessageReceive` is not triggered when message is sent by client.

## Structure of message object

```json
{
  "message": "<message-sent>"
}
```
