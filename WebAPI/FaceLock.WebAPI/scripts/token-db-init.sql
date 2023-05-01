--USE [master]


--IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = 'TokenDB')
--BEGIN
    --CREATE DATABASE [TokenDB];
--END
--CREATE DATABASE [TokenDB];


--USE [TokenDB];


CREATE TABLE [dbo].[TokenStates] (
    [Id]                  INT            IDENTITY (1, 1) NOT NULL,
    [Token]               NVARCHAR (500) NOT NULL,
    [UserId]              NVARCHAR (128) NOT NULL,
    [RefreshTokenExpires] DATETIME       NOT NULL,
    [TimeCreated]         DATETIME       NOT NULL,
    [Country]             NVARCHAR (100) NOT NULL,
    [City]                NVARCHAR (100) NOT NULL,
    [Device]              NVARCHAR (100) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);



CREATE TABLE [dbo].[BlacklistTokens] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [Token]          NVARCHAR (500) NOT NULL,
    [ExpirationTime] DATETIME       NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);