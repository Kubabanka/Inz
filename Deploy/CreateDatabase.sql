USE [master]
GO

/****** Object:  Database [eVoteDatabase-Deployment]    Script Date: 06.09.2017 10:42:33 ******/
CREATE DATABASE [eVoteDatabase-Deployment]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'eVoteDatabase-Deployment', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\eVoteDatabase-Deployment.mdf' , SIZE = 6144KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'eVoteDatabase-Deployment_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\eVoteDatabase-Deployment_log.ldf' , SIZE = 4224KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [eVoteDatabase-Deployment] SET COMPATIBILITY_LEVEL = 120
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [eVoteDatabase-Deployment].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [eVoteDatabase-Deployment] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [eVoteDatabase-Deployment] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [eVoteDatabase-Deployment] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [eVoteDatabase-Deployment] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [eVoteDatabase-Deployment] SET ARITHABORT OFF 
GO

ALTER DATABASE [eVoteDatabase-Deployment] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [eVoteDatabase-Deployment] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [eVoteDatabase-Deployment] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [eVoteDatabase-Deployment] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [eVoteDatabase-Deployment] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [eVoteDatabase-Deployment] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [eVoteDatabase-Deployment] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [eVoteDatabase-Deployment] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [eVoteDatabase-Deployment] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [eVoteDatabase-Deployment] SET  DISABLE_BROKER 
GO

ALTER DATABASE [eVoteDatabase-Deployment] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [eVoteDatabase-Deployment] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [eVoteDatabase-Deployment] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [eVoteDatabase-Deployment] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [eVoteDatabase-Deployment] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [eVoteDatabase-Deployment] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [eVoteDatabase-Deployment] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [eVoteDatabase-Deployment] SET RECOVERY FULL 
GO

ALTER DATABASE [eVoteDatabase-Deployment] SET  MULTI_USER 
GO

ALTER DATABASE [eVoteDatabase-Deployment] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [eVoteDatabase-Deployment] SET DB_CHAINING OFF 
GO

ALTER DATABASE [eVoteDatabase-Deployment] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [eVoteDatabase-Deployment] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO

ALTER DATABASE [eVoteDatabase-Deployment] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [eVoteDatabase-Deployment] SET  READ_WRITE 
GO


USE [eVoteDatabase-Deployment]
GO

/****** Object:  Table [dbo].[Polls]    Script Date: 06.09.2017 10:44:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Polls](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[EndDate] [datetime] NOT NULL,
	[AesKey] [varbinary](max) NULL,
	[AesIV] [varbinary](max) NULL,
 CONSTRAINT [PK_dbo.Polls] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


USE [eVoteDatabase-Deployment]
GO

/****** Object:  Table [dbo].[Voters]    Script Date: 06.09.2017 10:44:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Voters](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](max) NULL,
	[LastName] [nvarchar](max) NULL,
	[Login] [nvarchar](max) NULL,
	[Password] [nvarchar](max) NULL,
	[Keys] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Voters] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

USE [eVoteDatabase-Deployment]
GO

/****** Object:  Table [dbo].[VoteOptions]    Script Date: 06.09.2017 10:45:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VoteOptions](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[VoteCount] [bigint] NOT NULL,
	[PollID] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.VoteOptions] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[VoteOptions]  WITH CHECK ADD  CONSTRAINT [FK_dbo.VoteOptions_dbo.Polls_PollID] FOREIGN KEY([PollID])
REFERENCES [dbo].[Polls] ([ID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[VoteOptions] CHECK CONSTRAINT [FK_dbo.VoteOptions_dbo.Polls_PollID]
GO

USE [eVoteDatabase-Deployment]
GO

/****** Object:  Table [dbo].[VoterPolls]    Script Date: 06.09.2017 10:45:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VoterPolls](
	[Voter_ID] [bigint] NOT NULL,
	[Poll_ID] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.VoterPolls] PRIMARY KEY CLUSTERED 
(
	[Voter_ID] ASC,
	[Poll_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[VoterPolls]  WITH CHECK ADD  CONSTRAINT [FK_dbo.VoterPolls_dbo.Polls_Poll_ID] FOREIGN KEY([Poll_ID])
REFERENCES [dbo].[Polls] ([ID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[VoterPolls] CHECK CONSTRAINT [FK_dbo.VoterPolls_dbo.Polls_Poll_ID]
GO

ALTER TABLE [dbo].[VoterPolls]  WITH CHECK ADD  CONSTRAINT [FK_dbo.VoterPolls_dbo.Voters_Voter_ID] FOREIGN KEY([Voter_ID])
REFERENCES [dbo].[Voters] ([ID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[VoterPolls] CHECK CONSTRAINT [FK_dbo.VoterPolls_dbo.Voters_Voter_ID]
GO

USE [eVoteDatabase-Deployment]
GO

/****** Object:  Table [dbo].[Votes]    Script Date: 06.09.2017 10:44:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Votes](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Choice] [varbinary](max) NULL,
	[VoteTime] [datetime] NOT NULL,
	[PollID] [bigint] NOT NULL,
	[EncryptedVoter] [varbinary](max) NULL,
 CONSTRAINT [PK_dbo.Votes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Votes]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Votes_dbo.Polls_PollID] FOREIGN KEY([PollID])
REFERENCES [dbo].[Polls] ([ID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Votes] CHECK CONSTRAINT [FK_dbo.Votes_dbo.Polls_PollID]
GO


