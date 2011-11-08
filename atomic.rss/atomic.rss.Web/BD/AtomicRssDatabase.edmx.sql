
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 11/08/2011 23:17:18
-- Generated from EDMX file: C:\Users\calimeraw\Documents\dev\atomic-rss\atomic.rss\atomic.rss.Web\BD\AtomicRssDatabase.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [atomic.rss];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ChannelsArticles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ArticlesSet] DROP CONSTRAINT [FK_ChannelsArticles];
GO
IF OBJECT_ID(N'[dbo].[FK_UsersChannels_Users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UsersChannels] DROP CONSTRAINT [FK_UsersChannels_Users];
GO
IF OBJECT_ID(N'[dbo].[FK_UsersChannels_Channels]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UsersChannels] DROP CONSTRAINT [FK_UsersChannels_Channels];
GO
IF OBJECT_ID(N'[dbo].[FK_UsersArticles_Users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ArticlesReaded] DROP CONSTRAINT [FK_UsersArticles_Users];
GO
IF OBJECT_ID(N'[dbo].[FK_UsersArticles_Articles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ArticlesReaded] DROP CONSTRAINT [FK_UsersArticles_Articles];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[ChannelsSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ChannelsSet];
GO
IF OBJECT_ID(N'[dbo].[UsersSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UsersSet];
GO
IF OBJECT_ID(N'[dbo].[ArticlesSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ArticlesSet];
GO
IF OBJECT_ID(N'[dbo].[UsersChannels]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UsersChannels];
GO
IF OBJECT_ID(N'[dbo].[ArticlesReaded]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ArticlesReaded];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'ChannelsSet'
CREATE TABLE [dbo].[ChannelsSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [Link] nvarchar(max)  NOT NULL,
    [Language] nvarchar(max)  NOT NULL,
    [Author] nvarchar(max)  NOT NULL,
    [UpdateFrequency] int  NOT NULL,
    [Date] datetime  NOT NULL
);
GO

-- Creating table 'UsersSet'
CREATE TABLE [dbo].[UsersSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [Passwords] nvarchar(max)  NOT NULL,
    [IsAdmin] bit  NOT NULL
);
GO

-- Creating table 'ArticlesSet'
CREATE TABLE [dbo].[ArticlesSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [Link] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [Date] datetime  NOT NULL,
    [GUID] nvarchar(max)  NOT NULL,
    [Channels_ID] int  NOT NULL
);
GO

-- Creating table 'UsersChannels'
CREATE TABLE [dbo].[UsersChannels] (
    [Users_Id] int  NOT NULL,
    [Channels_Id] int  NOT NULL
);
GO

-- Creating table 'ArticlesReaded'
CREATE TABLE [dbo].[ArticlesReaded] (
    [Users_Id] int  NOT NULL,
    [Articles_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'ChannelsSet'
ALTER TABLE [dbo].[ChannelsSet]
ADD CONSTRAINT [PK_ChannelsSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UsersSet'
ALTER TABLE [dbo].[UsersSet]
ADD CONSTRAINT [PK_UsersSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ArticlesSet'
ALTER TABLE [dbo].[ArticlesSet]
ADD CONSTRAINT [PK_ArticlesSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Users_Id], [Channels_Id] in table 'UsersChannels'
ALTER TABLE [dbo].[UsersChannels]
ADD CONSTRAINT [PK_UsersChannels]
    PRIMARY KEY NONCLUSTERED ([Users_Id], [Channels_Id] ASC);
GO

-- Creating primary key on [Users_Id], [Articles_Id] in table 'ArticlesReaded'
ALTER TABLE [dbo].[ArticlesReaded]
ADD CONSTRAINT [PK_ArticlesReaded]
    PRIMARY KEY NONCLUSTERED ([Users_Id], [Articles_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Channels_ID] in table 'ArticlesSet'
ALTER TABLE [dbo].[ArticlesSet]
ADD CONSTRAINT [FK_ChannelsArticles]
    FOREIGN KEY ([Channels_ID])
    REFERENCES [dbo].[ChannelsSet]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ChannelsArticles'
CREATE INDEX [IX_FK_ChannelsArticles]
ON [dbo].[ArticlesSet]
    ([Channels_ID]);
GO

-- Creating foreign key on [Users_Id] in table 'UsersChannels'
ALTER TABLE [dbo].[UsersChannels]
ADD CONSTRAINT [FK_UsersChannels_Users]
    FOREIGN KEY ([Users_Id])
    REFERENCES [dbo].[UsersSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Channels_Id] in table 'UsersChannels'
ALTER TABLE [dbo].[UsersChannels]
ADD CONSTRAINT [FK_UsersChannels_Channels]
    FOREIGN KEY ([Channels_Id])
    REFERENCES [dbo].[ChannelsSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UsersChannels_Channels'
CREATE INDEX [IX_FK_UsersChannels_Channels]
ON [dbo].[UsersChannels]
    ([Channels_Id]);
GO

-- Creating foreign key on [Users_Id] in table 'ArticlesReaded'
ALTER TABLE [dbo].[ArticlesReaded]
ADD CONSTRAINT [FK_UsersArticles_Users]
    FOREIGN KEY ([Users_Id])
    REFERENCES [dbo].[UsersSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Articles_Id] in table 'ArticlesReaded'
ALTER TABLE [dbo].[ArticlesReaded]
ADD CONSTRAINT [FK_UsersArticles_Articles]
    FOREIGN KEY ([Articles_Id])
    REFERENCES [dbo].[ArticlesSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UsersArticles_Articles'
CREATE INDEX [IX_FK_UsersArticles_Articles]
ON [dbo].[ArticlesReaded]
    ([Articles_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------