CREATE TABLE Roles(
	Id SERIAL PRIMARY KEY,
	Name VARCHAR(100) UNIQUE NOT NULL
);


CREATE TABLE Users(
	Id SERIAL PRIMARY KEY,
	FullName VARCHAR(100) NOT NULL,
	Emailid VARCHAR (100) UNIQUE NOT NULL,
	PasswordHash TEXT NOT NULL,
	RoleId INT REFERENCES Roles(Id),
	createdAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);


CREATE TABLE Events(
	Id SERIAL PRIMARY KEY,
	Title VARCHAR(100) NOT NULL,
	Description TEXT,
	Location VARCHAR(100),
	EventDate TIMESTAMP NOT NULL,
	TotalSeats INT NOT NULL,
	AvailableSeats INT NOT NULL,
	Price DECIMAL(10,2) NOT NULL, 
	CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE Bookings (
    Id SERIAL PRIMARY KEY,
    UserId INT REFERENCES Users(Id),
    EventId INT REFERENCES Events(Id),
    SeatsBooked INT NOT NULL,
    BookingDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    Status VARCHAR(20) DEFAULT 'Confirmed'
);


INSERT INTO Roles (Name)
VALUES ('Admin'), ('User');


INSERT INTO Users (FullName, Emailid, PasswordHash, RoleId)
VALUES
('Madhvesh', 'madhvesh@gmail.com', 'M@dhvesh14', 1),
('Rahul', 'rahul@gmail.com', 'R@hul1508', 2),
('Sharma', 'sneha@gmail.com', 'Sh@rm@1307', 2);

INSERT INTO Events
(Title, Description, Location, EventDate, TotalSeats, AvailableSeats, Price)
VALUES
(
'Music Concert',
'Live music concert',
'Rathnavarma Indoor Hall',
'2026-06-10 18:00:00',
100,
100,
499.99
),
(
'Tech Conference',
'Software developer conference',
'Krishna Main Hall',
'2026-07-15 10:00:00',
200,
200,
999.99
),
(
'Food Festival',
'Multi cuisine food festival',
'vanaranha Outdoor Hall',
'2026-08-01 12:00:00',
150,
150,
299.99
);



INSERT INTO Bookings
(UserId, EventId, SeatsBooked, Status)
VALUES
(2, 1, 2, 'Confirmed'),
(3, 2, 1, 'Confirmed'),
(2, 3, 4, 'Cancelled');

select * from roles;


select * from bookings;

select * from users;

select * from events;



