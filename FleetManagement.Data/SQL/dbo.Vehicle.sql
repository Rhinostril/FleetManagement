CREATE TABLE [dbo].[Vehicle] (
    [vehicleId]     INT           IDENTITY (1, 1) NOT NULL,
    [brand]         NVARCHAR (50) NOT NULL,
    [model]         NVARCHAR (50) NOT NULL,
    [chassisNumber] NVARCHAR (50) NOT NULL,
    [licensePlate]  NVARCHAR (50) NULL,
    [fuelType]      NVARCHAR (50) NOT NULL,
    [vehicleType]   NVARCHAR (50) NOT NULL,
    [color]         NVARCHAR (50) NULL,
    [doors]         INT           NULL,
    [driverId]      INT           NULL,
    CONSTRAINT [PK_Vehicle] PRIMARY KEY CLUSTERED ([vehicleId] ASC),
	FOREIGN KEY (driverId) REFERENCES dbo.Driver(driverId),
);

