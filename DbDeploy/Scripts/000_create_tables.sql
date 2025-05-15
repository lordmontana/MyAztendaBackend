-- Create the Employee table
CREATE TABLE Employee (
    Id INT PRIMARY KEY IDENTITY(1,1), -- Auto-incrementing primary key
    Name NVARCHAR(255) NOT NULL,      -- Name of the employee
    Gender NVARCHAR(50) NOT NULL      -- Gender of the employee
);

-- Insert dummy values into the Employee table
INSERT INTO Employee (Name, Gender) VALUES
('Alice', 'Female'),
('Bob', 'Male'),
('Charlie', 'Non-Binary'),
('Diana', 'Female'),
('Evan', 'Male');

-- Create the Customer table
CREATE TABLE Customer (
    Id INT PRIMARY KEY IDENTITY(1,1), -- Auto-incrementing primary key
    Name NVARCHAR(100),
    Gender NVARCHAR(10)
);
-- Insert 3 dummy records
INSERT INTO Customer ( Name, Gender) VALUES ('John', 'Male');
INSERT INTO Customer ( Name, Gender) VALUES ('Abaid', 'Male');
INSERT INTO Customer ( Name, Gender) VALUES ('Awan', 'Male');

-- Create the Location table
CREATE TABLE Location (
    Id INT PRIMARY KEY IDENTITY(1,1), -- Auto-incrementing primary key
    Name NVARCHAR(255) NOT NULL,      -- Name of the location
    Region NVARCHAR(255) NOT NULL,    -- Region of the location
    ClientId INT NOT NULL             -- Foreign key or identifier for the client
);

-- Insert dummy values into the Location table
INSERT INTO Location (Name, Region, ClientId) VALUES
('New York Office', 'North America', 1),
('London Office', 'Europe', 2),
('Tokyo Office', 'Asia', 3),
('Sydney Office', 'Australia', 4),
('Cape Town Office', 'Africa', 5);


-- Create the Product table
CREATE TABLE Product (
    Id INT PRIMARY KEY IDENTITY(1,1), -- Auto-incrementing primary key
    Name NVARCHAR(255) NOT NULL,      -- Name of the product
    Price DECIMAL(18,2) NOT NULL,     -- Price of the product with 2 decimal places
    Stock INT NOT NULL                -- Stock quantity
);

-- Insert dummy values into the Product table
INSERT INTO Product (Name, Price, Stock) VALUES
('Laptop', 1200.50, 25),
('Smartphone', 800.00, 100),
('Tablet', 450.75, 50),
('Smartwatch', 199.99, 75),
('Wireless Earbuds', 99.99, 200);


-- Create the Ticket table
CREATE TABLE Ticket (
    Id INT PRIMARY KEY IDENTITY(1,1), -- Auto-incrementing primary key
    Title NVARCHAR(255) NOT NULL,     -- Title of the ticket
    Description NVARCHAR(MAX) NOT NULL, -- Detailed description of the ticket
    Status NVARCHAR(50) NOT NULL DEFAULT 'Open', -- Status of the ticket
    CreatedDate DATETIME NOT NULL DEFAULT GETUTCDATE(), -- Date the ticket was created
    ClosedDate DATETIME NULL,         -- Date the ticket was closed (nullable)
    AssignedTo INT NOT NULL           -- ID of the user/employee assigned to the ticket
);

-- Insert dummy values into the Ticket table
INSERT INTO Ticket (Title, Description, Status, CreatedDate, ClosedDate, AssignedTo) VALUES
('Login Issue', 'User cannot log in to the system.', 'Open', GETUTCDATE(), NULL, 1),
('Payment Failure', 'Payment gateway is not processing transactions.', 'In Progress', GETUTCDATE(), NULL, 2),
('UI Bug', 'Alignment issue on the dashboard page.', 'Closed', GETUTCDATE(), GETUTCDATE(), 3),
('Feature Request', 'Add dark mode to the application.', 'Open', GETUTCDATE(), NULL, 4),
('Performance Issue', 'Application is slow during peak hours.', 'In Progress', GETUTCDATE(), NULL, 5);

