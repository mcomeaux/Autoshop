CREATE TABLE [Vehicles]
(
    [VehicleId] INTEGER CONSTRAINT 'PK_Vehicles' PRIMARY KEY AUTOINCREMENT,
    [Make] NVARCHAR(50) NOT NULL,
    [Model] NVARCHAR(50) NOT NULL,
    [Year] INTEGER NOT NULL,
    [Color] NVARCHAR(50) NOT NULL,
    [Description] NVARCHAR(500),
    [CustomerId] INTEGER NOT NULL,
    FOREIGN KEY(CustomerId) REFERENCES Customers(CustomerId)
);