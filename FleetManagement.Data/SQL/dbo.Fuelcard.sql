CREATE TABLE [dbo].[Fuelcard] (
    [fuelCardId]   INT           IDENTITY (1, 1) NOT NULL,
    [cardNumber]   NVARCHAR (50) NOT NULL,
    [validityDate] DATETIME      NOT NULL,
    [pin]          NVARCHAR (4)  NULL,
    [fuelType]     NVARCHAR (50) NULL,
    [driverId]     INT           NULL,
    [isEnabled]    BIT           NOT NULL,
    CONSTRAINT [PK_Fuelcard] PRIMARY KEY CLUSTERED ([fuelCardId] ASC),
	FOREIGN KEY (driverId) REFERENCES dbo.Driver(driverId),
);

