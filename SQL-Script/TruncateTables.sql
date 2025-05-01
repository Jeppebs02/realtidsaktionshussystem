USE AuctionHouse;
GO

-- Delete from child tables first
DELETE FROM dbo.Bid;
DELETE FROM dbo.[Transaction];

-- Then delete from parent tables
DELETE FROM dbo.Auction;
DELETE FROM dbo.Item;
DELETE FROM dbo.Wallet;
DELETE FROM dbo.[User];

-- Then lookup/reference tables
DELETE FROM dbo.AuctionStatus;
DELETE FROM dbo.ItemCategory;
DELETE FROM dbo.TransactionType;
GO

-- Optionally, reset identity columns
-- If you want to reset identity seeds back to 1
DBCC CHECKIDENT ('dbo.Bid', RESEED, 0);
DBCC CHECKIDENT ('dbo.Transaction', RESEED, 0);
DBCC CHECKIDENT ('dbo.Auction', RESEED, 0);
DBCC CHECKIDENT ('dbo.Item', RESEED, 0);
DBCC CHECKIDENT ('dbo.Wallet', RESEED, 0);
DBCC CHECKIDENT ('dbo.[User]', RESEED, 0);
GO