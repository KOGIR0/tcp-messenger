# Server Client app

Server listence on two ports: 8000 and 8001

## Server-Client communication process
___

Client chooses unique identifier on start

Client connects to port 8000, sends id and recives unique code from server

Client connects to port 8001, sends: message id code

If code send by client does not match its id then server sends error message to client

If code is correct then server logs client message