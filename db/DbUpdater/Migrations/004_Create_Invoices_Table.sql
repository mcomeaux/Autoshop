CREATE TABLE [Invoices]
(
    [InvoiceId] INTEGER CONSTRAINT 'PK_Invoices' PRIMARY KEY AUTOINCREMENT,
    [VisitId] INTEGER NOT NULL,
    [CustomerId] INTEGER NOT NULL,
    [CreatedDate] DATETIME NOT NULL,
    [PaidOffDate] DATETIME NULL,
    [TotalCost] REAL NOT NULL,
    [AmountPaid] REAL NOT NULL,
    [Description] NVARCHAR(5000),
    CONSTRAINT 'FK_Invoices_Visits' FOREIGN KEY(VisitId) REFERENCES Visits(VisitId),
    CONSTRAINT 'FK_Invoices_Customers' FOREIGN KEY(CustomerId) REFERENCES Customers(CustomerId)
);