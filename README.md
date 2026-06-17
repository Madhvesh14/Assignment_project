Event Booking API

Project overview:

Event booking API is a RESTful backend application built using ASP.NET core and Entity Framework core. It allows users to register, login using JWT authentication, browse events, book seats, update bookings(update the number of seats)and cancel bookings. Admin can create, update and delete events.

Prerequisites:

.Net SDK.
POSTgreSQL.
Docker Desktop.
Visual Studio or any other code editor.
Postman for testing APIs.

Steps for running application:

Clone the repository.

Clone the repository from the main branch.

Rearrange the folder structure.

 Move the docker-compose.yml and eventbookingdb.sql files outside the EventBookingAPI.

For example the folder structure should look like this.

Project
|_
|  EventBookingAPI 
|_
|  docker-compose.yml
|_
   Eventbookingdb.sql

Steps to run the postgres container 

First we need to bring up the database container, to do that  make sure you are in the root folder.


For example:
PS C:\Users\Desktop\Project> 

In front of Project copy paste this line.
docker compose up -d postgres
This command brings up the postgres container in detached mode and you should see something like this as output.

[+] up 2/2
 ✔ Network project_default Created                                                                                                                                         
 ✔ Container eventbooking-db  Started

This is done in order to connect the backend and  database.
To stop the container run this command.
docker compose down

To run the Backend application 

Go inside the EventBookingAPI folder 

For example:
PS C:\Users\Desktop\Project\EventBookingAPI> 

Run this command: dotnet run

The API will start on http://localhost:5226

To run the tests 

Run this command inside EventBookingAPI

dotnet test .\EventBookingAPI.Tests\ 

API Endpoints

Authentication
POST
/api/auth/register 
POST
/api/auth/login

Events
GET
/api/events 
GET
/api/events/{id}
POST
/api/events
PUT
/api/events/{id}
DELETE
/api/events/{id}

Bookings
POST
/api/bookings 
GET
/api/bookings/mybookings
PUT
/api/bookings/{id} 
DELETE
/api/bookings/{id} 


