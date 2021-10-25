CREATE TABLE [dbo].[Driver] (
    [driverId]       INT           IDENTITY (1, 1) NOT NULL,
    [firstName]      NVARCHAR (50) NOT NULL,
    [lastName]       NVARCHAR (50) NOT NULL,
    [dateOfBirth]    DATETIME      NOT NULL,
    [addressId]      INT           NULL,
    [securityNumber] NVARCHAR (50) NOT NULL,
    [vehicleId]      INT           NULL,
    [fuelcardId]     INT           NULL,
    CONSTRAINT [PK_Driver] PRIMARY KEY CLUSTERED ([driverId] ASC),
	FOREIGN KEY (vehicleId) REFERENCES dbo.Vehicle(vehicleId),
	FOREIGN KEY (fuelcardId) REFERENCES dbo.Fuelcard(fuelCardId),
);

