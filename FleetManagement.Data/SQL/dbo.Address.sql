CREATE TABLE [dbo].[Address] (
    [addressId]  INT           IDENTITY (1, 1) NOT NULL,
    [street]     NVARCHAR (50) NOT NULL,
    [houseNr]    NVARCHAR (50) NOT NULL,
    [postalCode] NVARCHAR (50) NOT NULL,
    [city]       NVARCHAR (50) NOT NULL,
    [country]    NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED ([addressId] ASC)
);

