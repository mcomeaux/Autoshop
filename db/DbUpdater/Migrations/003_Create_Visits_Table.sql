CREATE TABLE [Visits]
(
    [VisitId] INTEGER CONSTRAINT 'PK_Visits' PRIMARY KEY AUTOINCREMENT,
    [VehicleId] INTEGER NOT NULL,
    [DateOfArrival] DATETIME NOT NULL,
    [DateOfDeparture] DATETIME NULL,
    [Description] NVARCHAR(5000),
    CONSTRAINT 'FK_Visits_Vehicles' FOREIGN KEY(VehicleId) REFERENCES Vehicles(VehicleId)
);