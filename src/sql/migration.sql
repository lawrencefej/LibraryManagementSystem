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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

    CREATE TABLE `LibraryAssets` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Added` datetime(6) NOT NULL,
        `DeweyIndex` varchar(15) CHARACTER SET utf8mb4 NULL,
        `Status` varchar(10) CHARACTER SET utf8mb4 NOT NULL,
        `NumberOfCopies` int NOT NULL,
        `CopiesAvailable` int NOT NULL,
        `Description` varchar(250) CHARACTER SET utf8mb4 NULL,
        `AssetType` varchar(10) CHARACTER SET utf8mb4 NOT NULL,
        `ISBN` varchar(25) CHARACTER SET utf8mb4 NULL,
        `Title` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        `Year` int NOT NULL,
        CONSTRAINT `PK_LibraryAssets` PRIMARY KEY (`Id`)
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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

    CREATE TABLE `State` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Name` varchar(25) CHARACTER SET utf8mb4 NOT NULL,
        `Abbreviations` varchar(5) CHARACTER SET utf8mb4 NOT NULL,
        CONSTRAINT `PK_State` PRIMARY KEY (`Id`)
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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

    CREATE TABLE `LibraryAssetAuthors` (
        `LibrayAssetId` int NOT NULL,
        `AuthorId` int NOT NULL,
        `Order` tinyint unsigned NOT NULL,
        CONSTRAINT `PK_LibraryAssetAuthors` PRIMARY KEY (`LibrayAssetId`, `AuthorId`),
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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

    CREATE TABLE `LibraryAssetCategory` (
        `LibrayAssetId` int NOT NULL,
        `CategoryId` int NOT NULL,
        CONSTRAINT `PK_LibraryAssetCategory` PRIMARY KEY (`LibrayAssetId`, `CategoryId`),
        CONSTRAINT `FK_LibraryAssetCategory_Category_CategoryId` FOREIGN KEY (`CategoryId`) REFERENCES `Category` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_LibraryAssetCategory_LibraryAssets_LibrayAssetId` FOREIGN KEY (`LibrayAssetId`) REFERENCES `LibraryAssets` (`Id`) ON DELETE CASCADE
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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

    CREATE TABLE `Address` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Street` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        `City` varchar(25) CHARACTER SET utf8mb4 NOT NULL,
        `StateId` int NOT NULL,
        `Zipcode` varchar(10) CHARACTER SET utf8mb4 NOT NULL,
        CONSTRAINT `PK_Address` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Address_State_StateId` FOREIGN KEY (`StateId`) REFERENCES `State` (`Id`) ON DELETE CASCADE
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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

    CREATE TABLE `LibraryCards` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `FirstName` varchar(25) CHARACTER SET utf8mb4 NOT NULL,
        `LastName` varchar(25) CHARACTER SET utf8mb4 NOT NULL,
        `Email` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
        `PhoneNumber` varchar(15) CHARACTER SET utf8mb4 NOT NULL,
        `CardNumber` varchar(25) CHARACTER SET utf8mb4 NOT NULL,
        `Fees` decimal(65,30) NOT NULL,
        `Created` datetime(6) NOT NULL,
        `MemberId` int NOT NULL,
        `AddressId` int NULL,
        `Gender` varchar(10) CHARACTER SET utf8mb4 NOT NULL,
        `Status` varchar(10) CHARACTER SET utf8mb4 NOT NULL,
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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

    INSERT INTO `AspNetRoles` (`Id`, `ConcurrencyStamp`, `Name`, `NormalizedName`)
    VALUES (1, '4abdb7dd-0a06-4359-b4f5-8190dbe114fc', 'Member', 'MEMBER');
    INSERT INTO `AspNetRoles` (`Id`, `ConcurrencyStamp`, `Name`, `NormalizedName`)
    VALUES (2, 'd2fa2931-ac05-40cd-86a4-26c633f066a2', 'Admin', 'ADMIN');
    INSERT INTO `AspNetRoles` (`Id`, `ConcurrencyStamp`, `Name`, `NormalizedName`)
    VALUES (3, 'c9542a94-5cae-4142-bb8b-67b261bf57c8', 'Librarian', 'LIBRARIAN');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (28, 'NE', 'Nebraska');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (29, 'NH', 'New Hampshire');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (30, 'NJ', 'New Jersey');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (31, 'NM', 'New Mexico');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (32, 'NY', 'New York');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (33, 'NC', 'North Carolina');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (34, 'NV', 'Nevada');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (35, 'ND', 'North Dakota');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (36, 'OH', 'Ohio');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (37, 'OK', 'Oklahoma');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (38, 'OR', 'Oregon');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (39, 'PA', 'Pennsylvania');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (40, 'RI', 'Rhode Island');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (41, 'SC', 'South Carolina');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (42, 'SD', 'South Dakota');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (43, 'TN', 'Tennessee');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (44, 'TX', 'Texas');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (45, 'UT', 'Utah');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (46, 'VT', 'Vermont');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (47, 'VA', 'Virginia');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (48, 'WA', 'Washington');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (49, 'WV', 'West Virginia');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (27, 'MT', 'Montana');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (26, 'MO', 'Missouri');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (24, 'MN', 'Minnesota');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (50, 'WI', 'Wisconsin');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (1, 'AL', 'Alabama');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (2, 'AK', 'Alaska');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (3, 'AR', 'Arkansas');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (4, 'AZ', 'Arizona');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (5, 'CA', 'California');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (6, 'CO', 'Colorado');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (7, 'CT', 'Connecticut');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (8, 'DC', 'District of Columbia');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (9, 'DE', 'Delaware');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (10, 'FL', 'Florida');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (11, 'GA', 'Georgia');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (12, 'HI', 'Hawaii');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (13, 'ID', 'Idaho');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (14, 'IL', 'Illinois');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (15, 'IN', 'Indiana');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (16, 'IA', 'Iowa');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (17, 'KS', 'Kansas');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (18, 'KY', 'Kentucky');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (19, 'LA', 'Louisiana');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (20, 'ME', 'Maine');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (21, 'MD', 'Maryland');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (22, 'MA', 'Massachusetts');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (23, 'MI', 'Michigan');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (25, 'MS', 'Mississippi');
    INSERT INTO `State` (`Id`, `Abbreviations`, `Name`)
    VALUES (51, 'WY', 'Wyoming');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

    CREATE INDEX `IX_Address_StateId` ON `Address` (`StateId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

    CREATE INDEX `IX_LibraryAssetCategory_CategoryId` ON `LibraryAssetCategory` (`CategoryId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

    CREATE UNIQUE INDEX `IX_LibraryCards_CardNumber` ON `LibraryCards` (`CardNumber`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

    CREATE UNIQUE INDEX `IX_LibraryCards_Email` ON `LibraryCards` (`Email`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20210625030800_initial') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20210625030800_initial', '5.0.5');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

