# Optsol Eventdriven Architecture.

## Playground

### Requirements

Docker 
.Net 6

### How to start

Inside the main folder there is an "docker-compose.yml" file. you have to run ```docker-compose up```. That will start a docker for mongodb and rabbitmq.

To run with visual studio, you select Properties of the solution and change to multiple startup projects. 
You will need to run:

- Sample.Bff.Api.
- Sample.Flight.Driving.Commands
- Sample.Hotel.Driving.Commands
- Sample.Saga

In your browser, go to "https://localhost:7186/swagger" and there will be the methods that you can call.

Inside the playground folder there is an project called "sample-frontend-basic". If you want to see the Signalr notification running. You just need to do an "npm install && npm start".

