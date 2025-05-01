USE AuctionHouse;
GO

-- ENUM DATA

-- 1. AuctionStatus (from enum)
INSERT INTO dbo.AuctionStatus (Status) VALUES
('SCHEDULED'), ('ACTIVE'), ('ENDED_SOLD'), ('ENDED_UNSOLD'), ('CANCELLED');

-- 2. ItemCategory (from enum Category)
INSERT INTO dbo.ItemCategory (Category) VALUES
('ART_ANTIQUES'), ('JEWELRY_LUXURY_GOODS'), ('COLLECTIBLES'), ('BOOKS_MANUSCRIPTS'),
('FURNITURE_HOME_DECOR'), ('ELECTRONICS'), ('VEHICLES'), ('REAL_ESTATE'),
('TOOLS_EQUIPMENT'), ('FASHION_APPAREL'), ('MUSIC_INSTRUMENTS'), ('SPORTS_OUTDOORS'),
('TOYS_GAMES'), ('INDUSTRIAL_MATERIALS'), ('OTHER');

-- 3. TransactionType
INSERT INTO dbo.TransactionType ([Type]) VALUES
('WITHDRAWAL'), ('DEPOSIT'), ('AUCTION_PAYMENT'), ('AUCTION_PAYOUT'), ('FEE');

-- USER & WALLET

-- 4. Users
INSERT INTO dbo.[User] (CantBuy, CantSell, UserName, PasswordHash, RegistrationDate,FirstName,LastName,Email, PhoneNumber, [Address]) VALUES
(0, 0, 'AliceThePowerWoman', '$2a$12$xm4jKCsTzECjReRq.7zeU.CqKor53lTygXtZ7zftIVy7YFnTvi1pC' ,GETDATE(),'Alice', 'Johnson', 'alice@example.com', '12345678', 'AnnesVej 21'),
(0, 0, 'BobTheLegend','$2a$12$xm4jKCsTzECjReRq.7zeU.CqKor53lTygXtZ7zftIVy7YFnTvi1pC' ,GETDATE(),'Bob', 'Smith', 'bob@example.com', '22446688', 'Bodilsvej 26'),
(0, 0, 'CritRine','$2a$12$xm4jKCsTzECjReRq.7zeU.CqKor53lTygXtZ7zftIVy7YFnTvi1pC' ,GETDATE(), 'Catherine', 'Lee', 'catherine@example.com', '88552211', 'Carolinevenget 22');

-- 5. Wallets (assuming 1-to-1 relationship)
INSERT INTO dbo.Wallet (TotalBalance, ReservedBalance, UserId) VALUES
(2000.00,1225, 1), (750.00,550, 2), (1200.00,600, 3);

-- ITEM & AUCTION

-- 6. Items
INSERT INTO dbo.Item (Name, Description, Category, Image, UserId) VALUES
('Antique Vase', '19th century Chinese porcelain.', 'ART_ANTIQUES',NULL, 1),
('Diamond Ring', '14K white gold with diamonds.', 'JEWELRY_LUXURY_GOODS',NULL , 2),
('Electric Guitar', 'Limited edition electric guitar.', 'MUSIC_INSTRUMENTS',NULL , 3);

-- 7. Auctions
INSERT INTO dbo.Auction (StartTime, EndTime, StartPrice, BuyOutPrice, MinimumBidIncrement, AuctionStatus, Version, Notify, ItemId)
VALUES
(GETDATE(), DATEADD(DAY, 7, GETDATE()), 500.00, 2000.00, 50.00, 'ACTIVE', 1, 1, 1),
(GETDATE(), DATEADD(DAY, 5, GETDATE()), 300.00, NULL, 25.00, 'ACTIVE', 1, 0, 2),
(GETDATE(), DATEADD(DAY, 10, GETDATE()), 800.00, 2500.00, 100.00, 'ACTIVE', 1, 1, 3);

-- BID & TRANSACTION

-- 8. Bids
INSERT INTO dbo.Bid (Amount, UserId, AuctionId)
VALUES
(550.00, 2, 1),
(600.00, 3, 1),
(325.00, 1, 2),
(900.00, 1, 3);

-- 9. Transactions
INSERT INTO dbo.[Transaction] (Amount, [Type], WalletId)
VALUES
(500.00, 'DEPOSIT',  1),
(200.00, 'WITHDRAWAL',  2),
(300.00, 'AUCTION_PAYMENT',  3),
(750.00, 'AUCTION_PAYOUT',  1),
(50.00, 'FEE',  2);
