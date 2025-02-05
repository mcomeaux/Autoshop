CREATE TABLE [Vehicles]
(
    [VehicleId] INTEGER CONSTRAINT 'PK_Vehicles' PRIMARY KEY AUTOINCREMENT,
    [Make] NVARCHAR(50) NOT NULL,
    [Model] NVARCHAR(50) NOT NULL,
    [Year] INTEGER NOT NULL,
    [Color] NVARCHAR(50) NOT NULL,
    [VIN] NVARCHAR(50) NOT NULL,
    [Description] NVARCHAR(5000),
    [CustomerId] INTEGER NOT NULL,
    CONSTRAINT 'FK_Vehicles_Customers' FOREIGN KEY(CustomerId) REFERENCES Customers(CustomerId)
);