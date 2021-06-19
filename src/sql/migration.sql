ALTER DATABASE CHARACTER SET utf8mb4;
CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET utf8mb4;

START TRANSACTION;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    ALTER DATABASE CHARACTER SET utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE TABLE `Address` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Street` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        `City` varchar(25) CHARACTER SET utf8mb4 NOT NULL,
        `State` nvarchar(2) NOT NULL,
        `Zipcode` varchar(10) CHARACTER SET utf8mb4 NOT NULL,
        CONSTRAINT `PK_Address` PRIMARY KEY (`Id`)
    ) CHARACTER SET utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE TABLE `AspNetRoles` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Name` varchar(256) CHARACTER SET utf8mb4 NULL,
        `NormalizedName` varchar(256) CHARACTER SET utf8mb4 NULL,
        `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 NULL,
        CONSTRAINT `PK_AspNetRoles` PRIMARY KEY (`Id`)
    ) CHARACTER SET utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE TABLE `AspNetUsers` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `FirstName` varchar(25) CHARACTER SET utf8mb4 NOT NULL,
        `LastName` varchar(25) CHARACTER SET utf8mb4 NOT NULL,
        `Created` datetime(6) NOT NULL,
        `IsAccountActivated` tinyint(1) NOT NULL,
        `UserName` varchar(256) CHARACTER SET utf8mb4 NULL,
        `NormalizedUserName` varchar(256) CHARACTER SET utf8mb4 NULL,
        `Email` varchar(256) CHARACTER SET utf8mb4 NULL,
        `NormalizedEmail` varchar(256) CHARACTER SET utf8mb4 NULL,
        `EmailConfirmed` tinyint(1) NOT NULL,
        `PasswordHash` longtext CHARACTER SET utf8mb4 NULL,
        `SecurityStamp` longtext CHARACTER SET utf8mb4 NULL,
        `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 NULL,
        `PhoneNumber` longtext CHARACTER SET utf8mb4 NULL,
        `PhoneNumberConfirmed` tinyint(1) NOT NULL,
        `TwoFactorEnabled` tinyint(1) NOT NULL,
        `LockoutEnd` datetime(6) NULL,
        `LockoutEnabled` tinyint(1) NOT NULL,
        `AccessFailedCount` int NOT NULL,
        CONSTRAINT `PK_AspNetUsers` PRIMARY KEY (`Id`)
    ) CHARACTER SET utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE TABLE `Authors` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `FullName` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        CONSTRAINT `PK_Authors` PRIMARY KEY (`Id`)
    ) CHARACTER SET utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE TABLE `Category` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Name` varchar(15) CHARACTER SET utf8mb4 NOT NULL,
        `Description` longtext CHARACTER SET utf8mb4 NULL,
        CONSTRAINT `PK_Category` PRIMARY KEY (`Id`)
    ) CHARACTER SET utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE TABLE `AspNetRoleClaims` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `RoleId` int NOT NULL,
        `ClaimType` longtext CHARACTER SET utf8mb4 NULL,
        `ClaimValue` longtext CHARACTER SET utf8mb4 NULL,
        CONSTRAINT `PK_AspNetRoleClaims` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE TABLE `AspNetUserClaims` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `UserId` int NOT NULL,
        `ClaimType` longtext CHARACTER SET utf8mb4 NULL,
        `ClaimValue` longtext CHARACTER SET utf8mb4 NULL,
        CONSTRAINT `PK_AspNetUserClaims` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE TABLE `AspNetUserLogins` (
        `LoginProvider` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `ProviderKey` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `ProviderDisplayName` longtext CHARACTER SET utf8mb4 NULL,
        `UserId` int NOT NULL,
        CONSTRAINT `PK_AspNetUserLogins` PRIMARY KEY (`LoginProvider`, `ProviderKey`),
        CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE TABLE `AspNetUserRoles` (
        `UserId` int NOT NULL,
        `RoleId` int NOT NULL,
        CONSTRAINT `PK_AspNetUserRoles` PRIMARY KEY (`UserId`, `RoleId`),
        CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE TABLE `AspNetUserTokens` (
        `UserId` int NOT NULL,
        `LoginProvider` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `Name` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `Value` longtext CHARACTER SET utf8mb4 NULL,
        CONSTRAINT `PK_AspNetUserTokens` PRIMARY KEY (`UserId`, `LoginProvider`, `Name`),
        CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE TABLE `LibraryCards` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `CardNumber` int NOT NULL,
        `Fees` decimal(65,30) NOT NULL,
        `Created` datetime(6) NOT NULL,
        `MemberId` int NOT NULL,
        `AddressId` int NULL,
        CONSTRAINT `PK_LibraryCards` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_LibraryCards_Address_AddressId` FOREIGN KEY (`AddressId`) REFERENCES `Address` (`Id`) ON DELETE RESTRICT,
        CONSTRAINT `FK_LibraryCards_AspNetUsers_MemberId` FOREIGN KEY (`MemberId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE TABLE `LibraryAssets` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Title` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        `Year` int NOT NULL,
        `Status` varchar(10) CHARACTER SET utf8mb4 NOT NULL,
        `Added` datetime(6) NOT NULL,
        `NumberOfCopies` int NOT NULL,
        `CopiesAvailable` int NOT NULL,
        `Description` varchar(100) CHARACTER SET utf8mb4 NULL,
        `AssetType` varchar(10) CHARACTER SET utf8mb4 NOT NULL,
        `ISBN` varchar(25) CHARACTER SET utf8mb4 NULL,
        `DeweyIndex` varchar(15) CHARACTER SET utf8mb4 NULL,
        `CategoryId` int NOT NULL,
        CONSTRAINT `PK_LibraryAssets` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_LibraryAssets_Category_CategoryId` FOREIGN KEY (`CategoryId`) REFERENCES `Category` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE TABLE `Checkouts` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `LibraryCardId` int NOT NULL,
        `CheckoutDate` datetime(6) NOT NULL,
        `DueDate` datetime(6) NOT NULL,
        `RenewalCount` tinyint unsigned NOT NULL,
        `IsReturned` tinyint(1) NOT NULL,
        `DateReturned` datetime(6) NOT NULL,
        `Status` varchar(10) CHARACTER SET utf8mb4 NOT NULL,
        CONSTRAINT `PK_Checkouts` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Checkouts_LibraryCards_LibraryCardId` FOREIGN KEY (`LibraryCardId`) REFERENCES `LibraryCards` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE TABLE `CheckoutHistory` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `LibraryAssetId` int NULL,
        `LibraryCardId` int NULL,
        `CheckedOut` datetime(6) NOT NULL,
        `CheckedIn` datetime(6) NULL,
        CONSTRAINT `PK_CheckoutHistory` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_CheckoutHistory_LibraryAssets_LibraryAssetId` FOREIGN KEY (`LibraryAssetId`) REFERENCES `LibraryAssets` (`Id`) ON DELETE RESTRICT,
        CONSTRAINT `FK_CheckoutHistory_LibraryCards_LibraryCardId` FOREIGN KEY (`LibraryCardId`) REFERENCES `LibraryCards` (`Id`) ON DELETE RESTRICT
    ) CHARACTER SET utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE TABLE `Holds` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `LibraryAssetId` int NULL,
        `LibraryCardId` int NULL,
        `HoldPlaced` datetime(6) NOT NULL,
        CONSTRAINT `PK_Holds` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Holds_LibraryAssets_LibraryAssetId` FOREIGN KEY (`LibraryAssetId`) REFERENCES `LibraryAssets` (`Id`) ON DELETE RESTRICT,
        CONSTRAINT `FK_Holds_LibraryCards_LibraryCardId` FOREIGN KEY (`LibraryCardId`) REFERENCES `LibraryCards` (`Id`) ON DELETE RESTRICT
    ) CHARACTER SET utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE TABLE `LibraryAssetAuthors` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `LibrayAssetId` int NOT NULL,
        `AuthorId` int NOT NULL,
        CONSTRAINT `PK_LibraryAssetAuthors` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_LibraryAssetAuthors_Authors_AuthorId` FOREIGN KEY (`AuthorId`) REFERENCES `Authors` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_LibraryAssetAuthors_LibraryAssets_LibrayAssetId` FOREIGN KEY (`LibrayAssetId`) REFERENCES `LibraryAssets` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE TABLE `Photos` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Url` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        `DateAdded` datetime(6) NOT NULL,
        `PublicId` longtext CHARACTER SET utf8mb4 NULL,
        `Discriminator` longtext CHARACTER SET utf8mb4 NOT NULL,
        `LibraryAssetId` int NULL,
        `UserId` int NULL,
        CONSTRAINT `PK_Photos` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Photos_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_Photos_LibraryAssets_LibraryAssetId` FOREIGN KEY (`LibraryAssetId`) REFERENCES `LibraryAssets` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE TABLE `ReserveAssets` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `LibraryAssetId` int NOT NULL,
        `LibraryCardId` int NOT NULL,
        `Reserved` datetime(6) NOT NULL,
        `Until` datetime(6) NOT NULL,
        `IsCheckedOut` tinyint(1) NOT NULL,
        `IsExpired` tinyint(1) NOT NULL,
        `DateCheckedOut` datetime(6) NULL,
        CONSTRAINT `PK_ReserveAssets` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_ReserveAssets_LibraryAssets_LibraryAssetId` FOREIGN KEY (`LibraryAssetId`) REFERENCES `LibraryAssets` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_ReserveAssets_LibraryCards_LibraryCardId` FOREIGN KEY (`LibraryCardId`) REFERENCES `LibraryCards` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE TABLE `CheckoutItems` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `LibraryAssetId` int NOT NULL,
        `Status` varchar(10) CHARACTER SET utf8mb4 NOT NULL,
        `CheckoutId` int NOT NULL,
        CONSTRAINT `PK_CheckoutItems` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_CheckoutItems_Checkouts_CheckoutId` FOREIGN KEY (`CheckoutId`) REFERENCES `Checkouts` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_CheckoutItems_LibraryAssets_LibraryAssetId` FOREIGN KEY (`LibraryAssetId`) REFERENCES `LibraryAssets` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    INSERT INTO `AspNetRoles` (`Id`, `ConcurrencyStamp`, `Name`, `NormalizedName`)
    VALUES (1, 'df038373-a791-4fc0-9328-ee6d4daf5bb1', 'Member', 'MEMBER');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    INSERT INTO `AspNetRoles` (`Id`, `ConcurrencyStamp`, `Name`, `NormalizedName`)
    VALUES (2, 'b7dd1ed5-ebb2-40fa-ac96-669d70211435', 'Admin', 'ADMIN');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    INSERT INTO `AspNetRoles` (`Id`, `ConcurrencyStamp`, `Name`, `NormalizedName`)
    VALUES (3, '6a9ba63b-a9b7-4cb5-9200-1e8441d142ce', 'Librarian', 'LIBRARIAN');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE INDEX `IX_AspNetRoleClaims_RoleId` ON `AspNetRoleClaims` (`RoleId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE UNIQUE INDEX `RoleNameIndex` ON `AspNetRoles` (`NormalizedName`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE INDEX `IX_AspNetUserClaims_UserId` ON `AspNetUserClaims` (`UserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE INDEX `IX_AspNetUserLogins_UserId` ON `AspNetUserLogins` (`UserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE INDEX `IX_AspNetUserRoles_RoleId` ON `AspNetUserRoles` (`RoleId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE INDEX `EmailIndex` ON `AspNetUsers` (`NormalizedEmail`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE UNIQUE INDEX `UserNameIndex` ON `AspNetUsers` (`NormalizedUserName`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE INDEX `IX_CheckoutHistory_LibraryAssetId` ON `CheckoutHistory` (`LibraryAssetId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE INDEX `IX_CheckoutHistory_LibraryCardId` ON `CheckoutHistory` (`LibraryCardId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE INDEX `IX_CheckoutItems_CheckoutId` ON `CheckoutItems` (`CheckoutId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE INDEX `IX_CheckoutItems_LibraryAssetId` ON `CheckoutItems` (`LibraryAssetId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE INDEX `IX_Checkouts_LibraryCardId` ON `Checkouts` (`LibraryCardId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE INDEX `IX_Holds_LibraryAssetId` ON `Holds` (`LibraryAssetId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE INDEX `IX_Holds_LibraryCardId` ON `Holds` (`LibraryCardId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE INDEX `IX_LibraryAssetAuthors_AuthorId` ON `LibraryAssetAuthors` (`AuthorId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE INDEX `IX_LibraryAssetAuthors_LibrayAssetId` ON `LibraryAssetAuthors` (`LibrayAssetId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE INDEX `IX_LibraryAssets_CategoryId` ON `LibraryAssets` (`CategoryId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE INDEX `IX_LibraryCards_AddressId` ON `LibraryCards` (`AddressId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE UNIQUE INDEX `IX_LibraryCards_MemberId` ON `LibraryCards` (`MemberId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE UNIQUE INDEX `IX_Photos_LibraryAssetId` ON `Photos` (`LibraryAssetId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE UNIQUE INDEX `IX_Photos_UserId` ON `Photos` (`UserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE INDEX `IX_ReserveAssets_LibraryAssetId` ON `ReserveAssets` (`LibraryAssetId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    CREATE INDEX `IX_ReserveAssets_LibraryCardId` ON `ReserveAssets` (`LibraryCardId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210520194825_initial') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20210520194825_initial', '5.0.5');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

START TRANSACTION;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210619002029_manytomany') THEN

    DROP PROCEDURE IF EXISTS `POMELO_BEFORE_DROP_PRIMARY_KEY`;
    DELIMITER //
    CREATE PROCEDURE `POMELO_BEFORE_DROP_PRIMARY_KEY`(IN `SCHEMA_NAME_ARGUMENT` VARCHAR(255), IN `TABLE_NAME_ARGUMENT` VARCHAR(255))
    BEGIN
    	DECLARE HAS_AUTO_INCREMENT_ID TINYINT(1);
    	DECLARE PRIMARY_KEY_COLUMN_NAME VARCHAR(255);
    	DECLARE PRIMARY_KEY_TYPE VARCHAR(255);
    	DECLARE SQL_EXP VARCHAR(1000);
    	SELECT COUNT(*)
    		INTO HAS_AUTO_INCREMENT_ID
    		FROM `information_schema`.`COLUMNS`
    		WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
    			AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
    			AND `Extra` = 'auto_increment'
    			AND `COLUMN_KEY` = 'PRI'
    			LIMIT 1;
    	IF HAS_AUTO_INCREMENT_ID THEN
    		SELECT `COLUMN_TYPE`
    			INTO PRIMARY_KEY_TYPE
    			FROM `information_schema`.`COLUMNS`
    			WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
    				AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
    				AND `COLUMN_KEY` = 'PRI'
    			LIMIT 1;
    		SELECT `COLUMN_NAME`
    			INTO PRIMARY_KEY_COLUMN_NAME
    			FROM `information_schema`.`COLUMNS`
    			WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
    				AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
    				AND `COLUMN_KEY` = 'PRI'
    			LIMIT 1;
    		SET SQL_EXP = CONCAT('ALTER TABLE `', (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA())), '`.`', TABLE_NAME_ARGUMENT, '` MODIFY COLUMN `', PRIMARY_KEY_COLUMN_NAME, '` ', PRIMARY_KEY_TYPE, ' NOT NULL;');
    		SET @SQL_EXP = SQL_EXP;
    		PREPARE SQL_EXP_EXECUTE FROM @SQL_EXP;
    		EXECUTE SQL_EXP_EXECUTE;
    		DEALLOCATE PREPARE SQL_EXP_EXECUTE;
    	END IF;
    END //
    DELIMITER ;


    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210619002029_manytomany') THEN

    DROP PROCEDURE IF EXISTS `POMELO_AFTER_ADD_PRIMARY_KEY`;
    DELIMITER //
    CREATE PROCEDURE `POMELO_AFTER_ADD_PRIMARY_KEY`(IN `SCHEMA_NAME_ARGUMENT` VARCHAR(255), IN `TABLE_NAME_ARGUMENT` VARCHAR(255), IN `COLUMN_NAME_ARGUMENT` VARCHAR(255))
    BEGIN
    	DECLARE HAS_AUTO_INCREMENT_ID INT(11);
    	DECLARE PRIMARY_KEY_COLUMN_NAME VARCHAR(255);
    	DECLARE PRIMARY_KEY_TYPE VARCHAR(255);
    	DECLARE SQL_EXP VARCHAR(1000);
    	SELECT COUNT(*)
    		INTO HAS_AUTO_INCREMENT_ID
    		FROM `information_schema`.`COLUMNS`
    		WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
    			AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
    			AND `COLUMN_NAME` = COLUMN_NAME_ARGUMENT
    			AND `COLUMN_TYPE` LIKE '%int%'
    			AND `COLUMN_KEY` = 'PRI';
    	IF HAS_AUTO_INCREMENT_ID THEN
    		SELECT `COLUMN_TYPE`
    			INTO PRIMARY_KEY_TYPE
    			FROM `information_schema`.`COLUMNS`
    			WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
    				AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
    				AND `COLUMN_NAME` = COLUMN_NAME_ARGUMENT
    				AND `COLUMN_TYPE` LIKE '%int%'
    				AND `COLUMN_KEY` = 'PRI';
    		SELECT `COLUMN_NAME`
    			INTO PRIMARY_KEY_COLUMN_NAME
    			FROM `information_schema`.`COLUMNS`
    			WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
    				AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
    				AND `COLUMN_NAME` = COLUMN_NAME_ARGUMENT
    				AND `COLUMN_TYPE` LIKE '%int%'
    				AND `COLUMN_KEY` = 'PRI';
    		SET SQL_EXP = CONCAT('ALTER TABLE `', (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA())), '`.`', TABLE_NAME_ARGUMENT, '` MODIFY COLUMN `', PRIMARY_KEY_COLUMN_NAME, '` ', PRIMARY_KEY_TYPE, ' NOT NULL AUTO_INCREMENT;');
    		SET @SQL_EXP = SQL_EXP;
    		PREPARE SQL_EXP_EXECUTE FROM @SQL_EXP;
    		EXECUTE SQL_EXP_EXECUTE;
    		DEALLOCATE PREPARE SQL_EXP_EXECUTE;
    	END IF;
    END //
    DELIMITER ;


    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210619002029_manytomany') THEN

    ALTER TABLE `LibraryAssets` DROP FOREIGN KEY `FK_LibraryAssets_Category_CategoryId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210619002029_manytomany') THEN

    ALTER TABLE `LibraryAssets` DROP INDEX `IX_LibraryAssets_CategoryId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210619002029_manytomany') THEN

    CALL POMELO_BEFORE_DROP_PRIMARY_KEY(NULL, 'LibraryAssetAuthors');
    ALTER TABLE `LibraryAssetAuthors` DROP PRIMARY KEY;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210619002029_manytomany') THEN

    ALTER TABLE `LibraryAssetAuthors` DROP INDEX `IX_LibraryAssetAuthors_LibrayAssetId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210619002029_manytomany') THEN

    ALTER TABLE `LibraryAssets` DROP COLUMN `CategoryId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210619002029_manytomany') THEN

    ALTER TABLE `LibraryAssetAuthors` DROP COLUMN `Id`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210619002029_manytomany') THEN

    ALTER TABLE `LibraryAssets` MODIFY COLUMN `Description` varchar(250) CHARACTER SET utf8mb4 NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210619002029_manytomany') THEN

    ALTER TABLE `LibraryAssetAuthors` ADD `Order` tinyint unsigned NOT NULL DEFAULT 0;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210619002029_manytomany') THEN

    ALTER TABLE `LibraryAssetAuthors` ADD CONSTRAINT `PK_LibraryAssetAuthors` PRIMARY KEY (`LibrayAssetId`, `AuthorId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210619002029_manytomany') THEN

    CREATE TABLE `CategoryLibraryAsset` (
        `AssetsId` int NOT NULL,
        `CategoriesId` int NOT NULL,
        CONSTRAINT `PK_CategoryLibraryAsset` PRIMARY KEY (`AssetsId`, `CategoriesId`),
        CONSTRAINT `FK_CategoryLibraryAsset_Category_CategoriesId` FOREIGN KEY (`CategoriesId`) REFERENCES `Category` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_CategoryLibraryAsset_LibraryAssets_AssetsId` FOREIGN KEY (`AssetsId`) REFERENCES `LibraryAssets` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210619002029_manytomany') THEN

    UPDATE `AspNetRoles` SET `ConcurrencyStamp` = 'a39fa3a9-486f-44f5-ac47-65ea2783b00a'
    WHERE `Id` = 1;
    SELECT ROW_COUNT();


    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210619002029_manytomany') THEN

    UPDATE `AspNetRoles` SET `ConcurrencyStamp` = 'f0dd774b-d29a-46d1-888a-10ac068b68b1'
    WHERE `Id` = 2;
    SELECT ROW_COUNT();


    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210619002029_manytomany') THEN

    UPDATE `AspNetRoles` SET `ConcurrencyStamp` = '97931e82-0a4b-4590-8495-fe0ff155e7f4'
    WHERE `Id` = 3;
    SELECT ROW_COUNT();


    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210619002029_manytomany') THEN

    CREATE INDEX `IX_CategoryLibraryAsset_CategoriesId` ON `CategoryLibraryAsset` (`CategoriesId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210619002029_manytomany') THEN

    DROP PROCEDURE `POMELO_BEFORE_DROP_PRIMARY_KEY`;


    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210619002029_manytomany') THEN

    DROP PROCEDURE `POMELO_AFTER_ADD_PRIMARY_KEY`;


    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210619002029_manytomany') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20210619002029_manytomany', '5.0.5');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

