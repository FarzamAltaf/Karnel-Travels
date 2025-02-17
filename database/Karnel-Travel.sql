USE [master]
GO
/****** Object:  Database [Karnel-Travel]    Script Date: 31/01/2025 3:22:38 am ******/
CREATE DATABASE [Karnel-Travel]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Karnel-Travel', FILENAME = N'E:\sql-server\MSSQL16.SQLEXPRESS\MSSQL\DATA\Karnel-Travel.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Karnel-Travel_log', FILENAME = N'E:\sql-server\MSSQL16.SQLEXPRESS\MSSQL\DATA\Karnel-Travel_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [Karnel-Travel] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Karnel-Travel].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Karnel-Travel] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Karnel-Travel] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Karnel-Travel] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Karnel-Travel] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Karnel-Travel] SET ARITHABORT OFF 
GO
ALTER DATABASE [Karnel-Travel] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Karnel-Travel] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Karnel-Travel] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Karnel-Travel] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Karnel-Travel] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Karnel-Travel] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Karnel-Travel] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Karnel-Travel] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Karnel-Travel] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Karnel-Travel] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Karnel-Travel] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Karnel-Travel] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Karnel-Travel] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Karnel-Travel] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Karnel-Travel] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Karnel-Travel] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Karnel-Travel] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Karnel-Travel] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Karnel-Travel] SET  MULTI_USER 
GO
ALTER DATABASE [Karnel-Travel] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Karnel-Travel] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Karnel-Travel] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Karnel-Travel] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Karnel-Travel] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Karnel-Travel] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [Karnel-Travel] SET QUERY_STORE = ON
GO
ALTER DATABASE [Karnel-Travel] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [Karnel-Travel]
GO
/****** Object:  Table [dbo].[BookingDates]    Script Date: 31/01/2025 3:22:38 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BookingDates](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[checkIn] [date] NOT NULL,
	[checkOut] [date] NOT NULL,
	[packageId] [int] NULL,
 CONSTRAINT [PK_BookingDates] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[contact]    Script Date: 31/01/2025 3:22:38 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[contact](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[userId] [int] NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[phone] [nvarchar](20) NULL,
	[email] [nvarchar](50) NOT NULL,
	[message] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_contact] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[faq]    Script Date: 31/01/2025 3:22:38 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[faq](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[question] [nvarchar](max) NOT NULL,
	[answer] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_faq] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[hotelBooking]    Script Date: 31/01/2025 3:22:38 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[hotelBooking](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[userId] [int] NOT NULL,
	[hotelId] [int] NOT NULL,
	[checkIn] [date] NOT NULL,
	[checkOut] [date] NOT NULL,
	[rooms] [int] NOT NULL,
	[days] [int] NOT NULL,
	[costPerRoom] [int] NOT NULL,
	[total] [int] NOT NULL,
	[status] [nvarchar](50) NOT NULL,
	[fee] [int] NOT NULL,
 CONSTRAINT [PK_hotelBookings] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[hotelImg]    Script Date: 31/01/2025 3:22:38 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[hotelImg](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[image] [nvarchar](max) NOT NULL,
	[hotelId] [int] NOT NULL,
 CONSTRAINT [PK_hotelImg] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[hotelRestrictions]    Script Date: 31/01/2025 3:22:38 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[hotelRestrictions](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[title] [nvarchar](max) NOT NULL,
	[description] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_hotelRestrictions] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[hotels]    Script Date: 31/01/2025 3:22:38 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[hotels](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](max) NOT NULL,
	[country] [nvarchar](50) NOT NULL,
	[city] [nvarchar](50) NOT NULL,
	[address] [nvarchar](max) NOT NULL,
	[maplink] [nvarchar](max) NOT NULL,
	[description] [nvarchar](max) NOT NULL,
	[status] [nvarchar](50) NULL,
	[price] [int] NULL,
	[maxRoom] [int] NULL,
 CONSTRAINT [PK_hotels] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[packageBooking]    Script Date: 31/01/2025 3:22:38 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[packageBooking](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[checkInId] [int] NOT NULL,
	[userId] [int] NOT NULL,
	[packageId] [int] NOT NULL,
	[maxPeople] [int] NOT NULL,
	[price] [int] NOT NULL,
	[status] [nvarchar](50) NOT NULL,
	[fee] [int] NOT NULL,
	[image] [varchar](255) NULL,
 CONSTRAINT [PK_packageBooking] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[packages]    Script Date: 31/01/2025 3:22:38 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[packages](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[title] [nvarchar](max) NOT NULL,
	[duration] [nvarchar](50) NOT NULL,
	[location] [nvarchar](50) NOT NULL,
	[price] [int] NOT NULL,
	[mainImage] [nvarchar](max) NULL,
	[image1] [nvarchar](max) NULL,
	[image2] [nvarchar](max) NULL,
	[image3] [nvarchar](max) NULL,
	[image4] [nvarchar](max) NULL,
	[maxPeople] [int] NOT NULL,
	[description] [nvarchar](max) NOT NULL,
	[maplink] [nvarchar](max) NULL,
 CONSTRAINT [PK_packages] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[packageServices]    Script Date: 31/01/2025 3:22:38 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[packageServices](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[packageId] [int] NOT NULL,
	[serviceId] [int] NOT NULL,
 CONSTRAINT [PK_packageServices] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[services]    Script Date: 31/01/2025 3:22:38 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[services](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[serviceTitle] [nvarchar](50) NOT NULL,
	[serviceCost] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_services] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[users]    Script Date: 31/01/2025 3:22:38 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[username] [nvarchar](50) NOT NULL,
	[email] [nvarchar](50) NOT NULL,
	[role] [nvarchar](50) NOT NULL,
	[password] [nvarchar](50) NOT NULL,
	[createdAt] [datetime] NULL,
	[updatedAt] [datetime] NULL,
 CONSTRAINT [PK_users] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[visa]    Script Date: 31/01/2025 3:22:38 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[visa](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[title] [nvarchar](max) NOT NULL,
	[country] [nvarchar](50) NOT NULL,
	[maxStay] [int] NOT NULL,
	[maxTime] [nvarchar](50) NOT NULL,
	[validity] [nvarchar](50) NOT NULL,
	[price] [int] NULL,
	[image] [nvarchar](50) NULL,
 CONSTRAINT [PK_visa] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[visaBooking]    Script Date: 31/01/2025 3:22:38 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[visaBooking](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[userId] [int] NOT NULL,
	[visaId] [int] NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[email] [nvarchar](50) NOT NULL,
	[phone] [nvarchar](50) NOT NULL,
	[visaType] [nvarchar](50) NOT NULL,
	[message] [nvarchar](max) NOT NULL,
	[status] [nvarchar](50) NOT NULL,
	[image] [nvarchar](max) NULL,
	[fee] [int] NULL,
	[checkOut] [nvarchar](max) NULL,
	[checkIn] [nvarchar](max) NULL,
 CONSTRAINT [PK_visaBooking] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[users] ADD  DEFAULT (getdate()) FOR [createdAt]
GO
ALTER TABLE [dbo].[users] ADD  DEFAULT (getdate()) FOR [updatedAt]
GO
ALTER TABLE [dbo].[BookingDates]  WITH CHECK ADD  CONSTRAINT [FK_BookingDates_packages] FOREIGN KEY([packageId])
REFERENCES [dbo].[packages] ([id])
GO
ALTER TABLE [dbo].[BookingDates] CHECK CONSTRAINT [FK_BookingDates_packages]
GO
ALTER TABLE [dbo].[contact]  WITH CHECK ADD  CONSTRAINT [FK_contact_users] FOREIGN KEY([userId])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[contact] CHECK CONSTRAINT [FK_contact_users]
GO
ALTER TABLE [dbo].[packageServices]  WITH CHECK ADD  CONSTRAINT [FK_packageServices_packages] FOREIGN KEY([packageId])
REFERENCES [dbo].[packages] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[packageServices] CHECK CONSTRAINT [FK_packageServices_packages]
GO
ALTER TABLE [dbo].[packageServices]  WITH CHECK ADD  CONSTRAINT [FK_packageServices_services] FOREIGN KEY([serviceId])
REFERENCES [dbo].[services] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[packageServices] CHECK CONSTRAINT [FK_packageServices_services]
GO
USE [master]
GO
ALTER DATABASE [Karnel-Travel] SET  READ_WRITE 
GO
