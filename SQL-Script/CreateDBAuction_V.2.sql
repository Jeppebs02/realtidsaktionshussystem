USE [master]
GO
/****** Object:  Database [AuctionHouse]    Script Date: 06/05/2025 09.45.32 ******/
CREATE DATABASE [AuctionHouse]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'AuctionHouse', FILENAME = N'/var/opt/mssql/data/AuctionHouse.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'AuctionHouse_log', FILENAME = N'/var/opt/mssql/data/AuctionHouse_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [AuctionHouse] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [AuctionHouse].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [AuctionHouse] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [AuctionHouse] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [AuctionHouse] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [AuctionHouse] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [AuctionHouse] SET ARITHABORT OFF 
GO
ALTER DATABASE [AuctionHouse] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [AuctionHouse] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [AuctionHouse] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [AuctionHouse] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [AuctionHouse] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [AuctionHouse] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [AuctionHouse] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [AuctionHouse] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [AuctionHouse] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [AuctionHouse] SET  DISABLE_BROKER 
GO
ALTER DATABASE [AuctionHouse] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [AuctionHouse] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [AuctionHouse] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [AuctionHouse] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [AuctionHouse] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [AuctionHouse] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [AuctionHouse] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [AuctionHouse] SET RECOVERY FULL 
GO
ALTER DATABASE [AuctionHouse] SET  MULTI_USER 
GO
ALTER DATABASE [AuctionHouse] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [AuctionHouse] SET DB_CHAINING OFF 
GO
ALTER DATABASE [AuctionHouse] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [AuctionHouse] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [AuctionHouse] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [AuctionHouse] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'AuctionHouse', N'ON'
GO
ALTER DATABASE [AuctionHouse] SET QUERY_STORE = OFF
GO
USE [AuctionHouse]
GO
/****** Object:  Table [dbo].[Auction]    Script Date: 06/05/2025 09.45.33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Auction](
	[AuctionId] [int] IDENTITY(1,1) NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NOT NULL,
	[StartPrice] [decimal](18, 2) NOT NULL,
	[BuyOutPrice] [decimal](18, 2) NULL,
	[MinimumBidIncrement] [decimal](18, 2) NULL,
	[AuctionStatus] [nvarchar](50) NOT NULL,
	[Version] [timestamp] NOT NULL,
	[Notify] [bit] NULL,
	[ItemId] [int] NOT NULL,
	[AmountOfBids] [int] NOT NULL,
 CONSTRAINT [PK_Auction] PRIMARY KEY CLUSTERED 
(
	[AuctionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AuctionStatus]    Script Date: 06/05/2025 09.45.33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuctionStatus](
	[Status] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_AuctionStatus] PRIMARY KEY CLUSTERED 
(
	[Status] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Bid]    Script Date: 06/05/2025 09.45.33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bid](
	[BidId] [int] IDENTITY(1,1) NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[TimeStamp] [datetime] NOT NULL,
	[UserId] [int] NOT NULL,
	[AuctionId] [int] NOT NULL,
 CONSTRAINT [PK_Bid] PRIMARY KEY CLUSTERED 
(
	[BidId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Item]    Script Date: 06/05/2025 09.45.33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Item](
	[ItemId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](255) NULL,
	[Category] [nvarchar](50) NULL,
	[Image] [varbinary](max) NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED 
(
	[ItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ItemCategory]    Script Date: 06/05/2025 09.45.33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ItemCategory](
	[Category] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ItemCategory] PRIMARY KEY CLUSTERED 
(
	[Category] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transaction]    Script Date: 06/05/2025 09.45.33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transaction](
	[TransactionID] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[TimeStamp] [datetime] NOT NULL,
	[Description] [nvarchar](255) NULL,
	[WalletId] [int] NOT NULL,
 CONSTRAINT [PK_Transaction] PRIMARY KEY CLUSTERED 
(
	[TransactionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransactionType]    Script Date: 06/05/2025 09.45.33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransactionType](
	[Type] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_TransactionType] PRIMARY KEY CLUSTERED 
(
	[Type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 06/05/2025 09.45.33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[CantBuy] [bit] NOT NULL,
	[CantSell] [bit] NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[PasswordHash] [nvarchar](255) NOT NULL,
	[RegistrationDate] [date] NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[PhoneNumber] [nvarchar](50) NOT NULL,
	[Address] [nvarchar](50) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Wallet]    Script Date: 06/05/2025 09.45.33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Wallet](
	[WalletId] [int] IDENTITY(1,1) NOT NULL,
	[TotalBalance] [decimal](18, 2) NULL,
	[ReservedBalance] [decimal](18, 2) NULL,
	[UserId] [int] NOT NULL,
	[Version] [timestamp] NOT NULL,
 CONSTRAINT [PK_Wallet] PRIMARY KEY CLUSTERED 
(
	[WalletId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Auction] ADD  CONSTRAINT [DF_Auction_Notify]  DEFAULT ((0)) FOR [Notify]
GO
ALTER TABLE [dbo].[Auction] ADD  CONSTRAINT [DF_Auction_Amount of bid]  DEFAULT ((0)) FOR [AmountOfBids]
GO
ALTER TABLE [dbo].[Item] ADD  CONSTRAINT [DF_Item_Category]  DEFAULT (N'OTHER') FOR [Category]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_IsBuyer]  DEFAULT ((0)) FOR [CantBuy]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_IsSeller]  DEFAULT ((0)) FOR [CantSell]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Auction]  WITH CHECK ADD  CONSTRAINT [FK_Auction_AuctionStatus] FOREIGN KEY([AuctionStatus])
REFERENCES [dbo].[AuctionStatus] ([Status])
GO
ALTER TABLE [dbo].[Auction] CHECK CONSTRAINT [FK_Auction_AuctionStatus]
GO
ALTER TABLE [dbo].[Auction]  WITH CHECK ADD  CONSTRAINT [FK_Auction_Item] FOREIGN KEY([ItemId])
REFERENCES [dbo].[Item] ([ItemId])
GO
ALTER TABLE [dbo].[Auction] CHECK CONSTRAINT [FK_Auction_Item]
GO
ALTER TABLE [dbo].[Bid]  WITH CHECK ADD  CONSTRAINT [FK_Bid_Auction] FOREIGN KEY([AuctionId])
REFERENCES [dbo].[Auction] ([AuctionId])
GO
ALTER TABLE [dbo].[Bid] CHECK CONSTRAINT [FK_Bid_Auction]
GO
ALTER TABLE [dbo].[Bid]  WITH CHECK ADD  CONSTRAINT [FK_Bid_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Bid] CHECK CONSTRAINT [FK_Bid_User]
GO
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK_Item_ItemCategory] FOREIGN KEY([Category])
REFERENCES [dbo].[ItemCategory] ([Category])
GO
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK_Item_ItemCategory]
GO
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK_Item_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK_Item_User]
GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK_Transaction_TransactionType] FOREIGN KEY([Type])
REFERENCES [dbo].[TransactionType] ([Type])
GO
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK_Transaction_TransactionType]
GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK_Transaction_Wallet] FOREIGN KEY([WalletId])
REFERENCES [dbo].[Wallet] ([WalletId])
GO
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK_Transaction_Wallet]
GO
ALTER TABLE [dbo].[Wallet]  WITH CHECK ADD  CONSTRAINT [FK_Wallet_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Wallet] CHECK CONSTRAINT [FK_Wallet_User]
GO
USE [master]
GO
ALTER DATABASE [AuctionHouse] SET  READ_WRITE 
GO
