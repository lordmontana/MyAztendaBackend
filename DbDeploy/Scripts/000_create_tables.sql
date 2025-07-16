
-- Insert dummy values into the Employee table
INSERT INTO Employee (Name, Gender) VALUES
('Alice', 'Female'),
('Bob', 'Male'),
('Charlie', 'Non-Binary'),
('Diana', 'Female'),
('Evan', 'Male');


-- Insert 3 dummy records
INSERT INTO Customer ( Name, Gender) VALUES ('John', 'Male');
INSERT INTO Customer ( Name, Gender) VALUES ('Abaid', 'Male');
INSERT INTO Customer ( Name, Gender) VALUES ('Awan', 'Male');


-- Insert dummy values into the Location table
INSERT INTO Location (Name, Region, ClientId) VALUES
('New York Office', 'North America', 1),
('London Office', 'Europe', 2),
('Tokyo Office', 'Asia', 3),
('Sydney Office', 'Australia', 4),
('Cape Town Office', 'Africa', 5);



-- Insert dummy values into the Product table
INSERT INTO Product (Name, Price, Stock) VALUES
('Laptop', 1200.50, 25),
('Smartphone', 800.00, 100),
('Tablet', 450.75, 50),
('Smartwatch', 199.99, 75),
('Wireless Earbuds', 99.99, 200);


-- Insert dummy values into the Ticket table
INSERT INTO Ticket (Title, Description, Status, CreatedDate, ClosedDate, AssignedTo) VALUES
('Login Issue', 'User cannot log in to the system.', 'Open', GETUTCDATE(), NULL, 1),
('Payment Failure', 'Payment gateway is not processing transactions.', 'In Progress', GETUTCDATE(), NULL, 2),
('UI Bug', 'Alignment issue on the dashboard page.', 'Closed', GETUTCDATE(), GETUTCDATE(), 3),
('Feature Request', 'Add dark mode to the application.', 'Open', GETUTCDATE(), NULL, 4),
('Performance Issue', 'Application is slow during peak hours.', 'In Progress', GETUTCDATE(), NULL, 5);

