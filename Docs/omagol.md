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
  c1 ->> b : Stop
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

---

## Structure of objects

The following is the structure of strongly typed Omagol hub class as required by signalR. We have a group provider that is responsible for assigning group to the registered users.

```mermaid
classDiagram 
  direction RL
  class IOmagol {
    <<interface>>
    + MessageReceive(Message message)
    + UserConnected()
    + UserDisconnected()
  }

  class Omagol {
    - IGroupProvider _groupProvider
    + Start()
    + Stop()
    + MessageSend(Message message)
  }
  
  class Message {
    +string Message
  }

  class IGroupProvider {
    + Register(User user)
    + UnRegister(User user)
    + event UsersConnected(object _, Group group)
  }

  class BasicGroupProvider {
    - List~User~ _availableConnections
    - Dictionary~Group, User~ _groupMap
    + Register(User user)
    + UnRegister(User user)
    + event UsersConnected(object _, Group group)
  }

  class Group {
    + Guid guid
    + IEnumerable~User~ ConnectionIds
  }
  
  class User {
    +string ConnectionId
  }

  BasicGroupProvider .. IGroupProvider
  Omagol .. IOmagol
  Omagol o-- IGroupProvider
  

```


