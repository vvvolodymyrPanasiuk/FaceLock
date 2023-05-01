USE [master]
GO

IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = 'BlacklistDB')
BEGIN    
    CREATE DATABASE [BlacklistDB];
END
GO

USE [BlacklistDB];
GO

CREATE TABLE [dbo].[BlacklistTokens] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [Token]          NVARCHAR (500) NOT NULL,
    [ExpirationTime] DATETIME       NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO