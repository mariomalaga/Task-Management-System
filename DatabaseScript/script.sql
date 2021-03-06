USE [master]
GO
/****** Object:  Database [TMSTasks]    Script Date: 29/09/2020 13:44:23 ******/
CREATE DATABASE [TMSTasks]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'TSMTasks', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERV2019\MSSQL\DATA\TSMTasks.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'TSMTasks_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERV2019\MSSQL\DATA\TSMTasks_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [TMSTasks] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [TMSTasks].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [TMSTasks] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [TMSTasks] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [TMSTasks] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [TMSTasks] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [TMSTasks] SET ARITHABORT OFF 
GO
ALTER DATABASE [TMSTasks] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [TMSTasks] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [TMSTasks] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [TMSTasks] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [TMSTasks] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [TMSTasks] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [TMSTasks] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [TMSTasks] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [TMSTasks] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [TMSTasks] SET  DISABLE_BROKER 
GO
ALTER DATABASE [TMSTasks] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [TMSTasks] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [TMSTasks] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [TMSTasks] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [TMSTasks] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [TMSTasks] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [TMSTasks] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [TMSTasks] SET RECOVERY FULL 
GO
ALTER DATABASE [TMSTasks] SET  MULTI_USER 
GO
ALTER DATABASE [TMSTasks] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [TMSTasks] SET DB_CHAINING OFF 
GO
ALTER DATABASE [TMSTasks] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [TMSTasks] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [TMSTasks] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'TMSTasks', N'ON'
GO
ALTER DATABASE [TMSTasks] SET QUERY_STORE = OFF
GO
USE [TMSTasks]
GO
/****** Object:  Table [dbo].[TMSSubTask]    Script Date: 29/09/2020 13:44:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMSSubTask](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [varchar](50) NULL,
	[Description] [varchar](50) NULL,
	[StartDate] [datetime] NULL,
	[FinishDate] [datetime] NULL,
	[State] [varchar](50) NULL,
	[IdTask] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_TMSSubTask] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMSTask]    Script Date: 29/09/2020 13:44:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMSTask](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [varchar](50) NULL,
	[Description] [varchar](50) NULL,
	[StartDate] [datetime] NULL,
	[FinishDate] [datetime] NULL,
	[State] [varchar](50) NULL,
 CONSTRAINT [PK_TSMTask] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[TMSSubTask] ([Id], [Name], [Description], [StartDate], [FinishDate], [State], [IdTask]) VALUES (N'd5a4ecd4-cbc9-47f4-a757-3f1c6d331fe5', N'Subtask 4', N'Doing subtask 4', CAST(N'2012-03-19T07:22:12.000' AS DateTime), CAST(N'2012-03-19T07:22:12.000' AS DateTime), N'Planned', N'90a6dadf-3714-4944-b94c-42f928fc37bb')
INSERT [dbo].[TMSSubTask] ([Id], [Name], [Description], [StartDate], [FinishDate], [State], [IdTask]) VALUES (N'90a6dadf-3714-4944-b94c-42f928fc37bc', N'Subtask 1', N'Doing Subtask 1', CAST(N'2012-03-19T07:22:12.000' AS DateTime), CAST(N'2012-03-19T07:23:33.000' AS DateTime), N'Completed', N'90a6dadf-3714-4944-b94c-42f928fc37bb')
INSERT [dbo].[TMSSubTask] ([Id], [Name], [Description], [StartDate], [FinishDate], [State], [IdTask]) VALUES (N'11ba9ed1-cdf6-454a-bed0-a302444033fb', N'Subtask 3', N'Doing Subtask 3', CAST(N'2012-03-19T07:22:12.000' AS DateTime), CAST(N'2012-03-19T07:23:33.000' AS DateTime), N'Completed', N'90a6dadf-3714-4944-b94c-42f928fc37bb')
GO
INSERT [dbo].[TMSTask] ([Id], [Name], [Description], [StartDate], [FinishDate], [State]) VALUES (N'960b656b-9ea1-4c32-aa04-0878802c7f7e', N'Task 6', N'Doing task 6', CAST(N'2020-09-28T11:56:37.930' AS DateTime), CAST(N'2020-10-19T07:22:12.000' AS DateTime), N'Planned')
INSERT [dbo].[TMSTask] ([Id], [Name], [Description], [StartDate], [FinishDate], [State]) VALUES (N'c604ba86-59e4-4bff-9660-3e40c96b0c8a', NULL, NULL, CAST(N'2020-09-28T19:50:40.880' AS DateTime), NULL, N'Planned')
INSERT [dbo].[TMSTask] ([Id], [Name], [Description], [StartDate], [FinishDate], [State]) VALUES (N'90a6dadf-3714-4944-b94c-42f928fc37bb', N'Task 1', N'Doing task 1', CAST(N'2012-03-19T07:22:12.000' AS DateTime), CAST(N'2012-03-19T07:23:33.000' AS DateTime), N'Completed')
INSERT [dbo].[TMSTask] ([Id], [Name], [Description], [StartDate], [FinishDate], [State]) VALUES (N'a1b99286-4de2-431b-bbce-7853f9285d9e', N'Task 9', N'Doing task 9', NULL, CAST(N'2020-09-28T20:09:31.160' AS DateTime), N'Completed')
INSERT [dbo].[TMSTask] ([Id], [Name], [Description], [StartDate], [FinishDate], [State]) VALUES (N'50272c1c-2cd2-4e41-9ab4-7aff7bef8220', N'Task 3', N'Doing task 3', CAST(N'2012-03-19T07:22:12.000' AS DateTime), CAST(N'2012-03-19T07:22:12.000' AS DateTime), N'inProgress')
INSERT [dbo].[TMSTask] ([Id], [Name], [Description], [StartDate], [FinishDate], [State]) VALUES (N'8bbdc46e-f334-4677-b93d-a6ab11bce5c4', N'Task 7', N'Doing task 7', CAST(N'2020-09-28T19:55:38.277' AS DateTime), NULL, N'Planned')
INSERT [dbo].[TMSTask] ([Id], [Name], [Description], [StartDate], [FinishDate], [State]) VALUES (N'f1f79f4e-7cc0-417c-bcea-a74236abc968', N'Task 5', N'Doing task 5', CAST(N'2012-03-19T07:23:33.000' AS DateTime), CAST(N'2012-03-19T07:23:33.000' AS DateTime), N'Planned')
INSERT [dbo].[TMSTask] ([Id], [Name], [Description], [StartDate], [FinishDate], [State]) VALUES (N'c942a4d4-786b-4483-a7a4-b84c5efc1d90', N'Task 8', N'Doing task 8', NULL, CAST(N'2020-09-28T19:59:32.070' AS DateTime), N'Completed')
INSERT [dbo].[TMSTask] ([Id], [Name], [Description], [StartDate], [FinishDate], [State]) VALUES (N'a2c91127-2a86-4f63-810e-b8b21272b93d', NULL, NULL, CAST(N'2020-09-28T19:52:14.710' AS DateTime), NULL, N'Planned')
INSERT [dbo].[TMSTask] ([Id], [Name], [Description], [StartDate], [FinishDate], [State]) VALUES (N'6d474980-3d1d-408b-b4df-c1419ed91541', NULL, NULL, CAST(N'2020-09-28T19:50:32.170' AS DateTime), NULL, N'Planned')
INSERT [dbo].[TMSTask] ([Id], [Name], [Description], [StartDate], [FinishDate], [State]) VALUES (N'bdb81673-e3a7-41bb-9dec-c66d7f260b35', N'Task 4', N'Doing task 4', CAST(N'2012-03-19T07:22:12.000' AS DateTime), CAST(N'2012-03-19T07:22:12.000' AS DateTime), N'Completed')
GO
ALTER TABLE [dbo].[TMSSubTask] ADD  CONSTRAINT [DF_TMSSubTask_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[TMSSubTask] ADD  CONSTRAINT [DF_TMSSubTask_State]  DEFAULT ('Planned') FOR [State]
GO
ALTER TABLE [dbo].[TMSTask] ADD  CONSTRAINT [DF_TSMTask_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[TMSTask] ADD  CONSTRAINT [DF_TMSTask_State]  DEFAULT ('Planned') FOR [State]
GO
ALTER TABLE [dbo].[TMSSubTask]  WITH NOCHECK ADD  CONSTRAINT [FK_TMSSubTask_TMSSubTask] FOREIGN KEY([IdTask])
REFERENCES [dbo].[TMSTask] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[TMSSubTask] NOCHECK CONSTRAINT [FK_TMSSubTask_TMSSubTask]
GO
USE [master]
GO
ALTER DATABASE [TMSTasks] SET  READ_WRITE 
GO
