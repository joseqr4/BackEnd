IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    [Name] nvarchar(120) NULL,
    [Last_Name] nvarchar(max) NULL,
    [password_confirmation] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Commerce] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(30) NULL,
    [Address] nvarchar(max) NULL,
    [Latitude] nvarchar(max) NULL,
    [longitude] nvarchar(max) NULL,
    [Phone] int NOT NULL,
    CONSTRAINT [PK_Commerce] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Company] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [business_name] nvarchar(max) NULL,
    [Rut] int NOT NULL,
    [Address] nvarchar(max) NULL,
    CONSTRAINT [PK_Company] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Interests] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    CONSTRAINT [PK_Interests] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [InterestsUsers] (
    [UserNameID] nvarchar(450) NOT NULL,
    [InterestsId] int NOT NULL,
    CONSTRAINT [PK_InterestsUsers] PRIMARY KEY ([InterestsId], [UserNameID]),
    CONSTRAINT [FK_InterestsUsers_Interests_InterestsId] FOREIGN KEY ([InterestsId]) REFERENCES [Interests] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_InterestsUsers_AspNetUsers_UserNameID] FOREIGN KEY ([UserNameID]) REFERENCES [AspNetUsers] ([UserName]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);

GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;

GO

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);

GO

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);

GO

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);

GO

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);

GO

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;

GO

CREATE INDEX [IX_InterestsUsers_UserNameID] ON [InterestsUsers] ([UserNameID]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20191116151443_1', N'2.2.3-servicing-35854');

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20191116172534_2', N'2.2.3-servicing-35854');

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20191116204822_3', N'2.2.3-servicing-35854');

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20191120230126_en', N'2.2.3-servicing-35854');

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20191124200512_9', N'2.2.3-servicing-35854');

GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Commerce]') AND [c].[name] = N'Phone');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Commerce] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Commerce] DROP COLUMN [Phone];

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'Photo');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [AspNetUsers] DROP COLUMN [Photo];

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20191124201236_12', N'2.2.3-servicing-35854');

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20191124205151_11', N'2.2.3-servicing-35854');

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20191124212947_15', N'2.2.3-servicing-35854');

GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Commerce]') AND [c].[name] = N'Phone');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Commerce] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Commerce] ALTER COLUMN [Phone] int NOT NULL;

GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Commerce]') AND [c].[name] = N'Longitude');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Commerce] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Commerce] ALTER COLUMN [Longitude] float NOT NULL;

GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Commerce]') AND [c].[name] = N'Latitude');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Commerce] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [Commerce] ALTER COLUMN [Latitude] float NOT NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20191124214141_phone', N'2.2.3-servicing-35854');

GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'Photo');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [AspNetUsers] DROP COLUMN [Photo];

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20191124214720_a', N'2.2.3-servicing-35854');

GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Commerce]') AND [c].[name] = N'Longitude');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [Commerce] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [Commerce] ALTER COLUMN [Longitude] real NOT NULL;

GO

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Commerce]') AND [c].[name] = N'Latitude');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Commerce] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [Commerce] ALTER COLUMN [Latitude] real NOT NULL;

GO

CREATE TABLE [Category] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [icon] varbinary(max) NULL,
    CONSTRAINT [PK_Category] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Discounts] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [discount_percentage] decimal(18,2) NOT NULL,
    [Date_start] datetime2 NOT NULL,
    [Date_end] datetime2 NOT NULL,
    CONSTRAINT [PK_Discounts] PRIMARY KEY ([Id])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20191128001926_Discount', N'2.2.3-servicing-35854');

GO

ALTER TABLE [Interests] ADD [DiscountsId] int NULL;

GO

ALTER TABLE [Commerce] ADD [CompanyId] int NULL;

GO

ALTER TABLE [Commerce] ADD [DiscountsId] int NULL;

GO

CREATE INDEX [IX_Interests_DiscountsId] ON [Interests] ([DiscountsId]);

GO

CREATE INDEX [IX_Commerce_CompanyId] ON [Commerce] ([CompanyId]);

GO

CREATE INDEX [IX_Commerce_DiscountsId] ON [Commerce] ([DiscountsId]);

GO

ALTER TABLE [Commerce] ADD CONSTRAINT [FK_Commerce_Company_CompanyId] FOREIGN KEY ([CompanyId]) REFERENCES [Company] ([Id]) ON DELETE NO ACTION;

GO

ALTER TABLE [Commerce] ADD CONSTRAINT [FK_Commerce_Discounts_DiscountsId] FOREIGN KEY ([DiscountsId]) REFERENCES [Discounts] ([Id]) ON DELETE NO ACTION;

GO

ALTER TABLE [Interests] ADD CONSTRAINT [FK_Interests_Discounts_DiscountsId] FOREIGN KEY ([DiscountsId]) REFERENCES [Discounts] ([Id]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20191130174516_Prueba', N'2.2.3-servicing-35854');

GO

CREATE TABLE [CommerceDiscounts] (
    [CommerceID] int NOT NULL,
    [DiscountsID] int NOT NULL,
    CONSTRAINT [PK_CommerceDiscounts] PRIMARY KEY ([CommerceID], [DiscountsID]),
    CONSTRAINT [FK_CommerceDiscounts_Commerce_CommerceID] FOREIGN KEY ([CommerceID]) REFERENCES [Commerce] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_CommerceDiscounts_Discounts_DiscountsID] FOREIGN KEY ([DiscountsID]) REFERENCES [Discounts] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [DiscountsInterests] (
    [DiscountsID] int NOT NULL,
    [InterestsId] int NOT NULL,
    CONSTRAINT [PK_DiscountsInterests] PRIMARY KEY ([DiscountsID], [InterestsId]),
    CONSTRAINT [FK_DiscountsInterests_Discounts_DiscountsID] FOREIGN KEY ([DiscountsID]) REFERENCES [Discounts] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_DiscountsInterests_Interests_InterestsId] FOREIGN KEY ([InterestsId]) REFERENCES [Interests] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_CommerceDiscounts_DiscountsID] ON [CommerceDiscounts] ([DiscountsID]);

GO

CREATE INDEX [IX_DiscountsInterests_InterestsId] ON [DiscountsInterests] ([InterestsId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20191130190321_prueba2', N'2.2.3-servicing-35854');

GO

ALTER TABLE [Commerce] DROP CONSTRAINT [FK_Commerce_Company_CompanyId];

GO

ALTER TABLE [Commerce] DROP CONSTRAINT [FK_Commerce_Discounts_DiscountsId];

GO

ALTER TABLE [Interests] DROP CONSTRAINT [FK_Interests_Discounts_DiscountsId];

GO

DROP INDEX [IX_Interests_DiscountsId] ON [Interests];

GO

DROP INDEX [IX_Commerce_CompanyId] ON [Commerce];

GO

DROP INDEX [IX_Commerce_DiscountsId] ON [Commerce];

GO

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Interests]') AND [c].[name] = N'DiscountsId');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [Interests] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [Interests] DROP COLUMN [DiscountsId];

GO

DECLARE @var9 sysname;
SELECT @var9 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Commerce]') AND [c].[name] = N'CompanyId');
IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [Commerce] DROP CONSTRAINT [' + @var9 + '];');
ALTER TABLE [Commerce] DROP COLUMN [CompanyId];

GO

DECLARE @var10 sysname;
SELECT @var10 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Commerce]') AND [c].[name] = N'DiscountsId');
IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [Commerce] DROP CONSTRAINT [' + @var10 + '];');
ALTER TABLE [Commerce] DROP COLUMN [DiscountsId];

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20191130204438_Acomodando', N'2.2.3-servicing-35854');

GO

DECLARE @var11 sysname;
SELECT @var11 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'LockoutEnd');
IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var11 + '];');
ALTER TABLE [AspNetUsers] ALTER COLUMN [LockoutEnd] datetime2 NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200113222556_Datatime', N'2.2.3-servicing-35854');

GO

ALTER TABLE [Discounts] ADD [Imagen] varbinary(max) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200113230301_Swagger', N'2.2.3-servicing-35854');

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200113230841_PruebaVariable', N'2.2.3-servicing-35854');

GO

CREATE TABLE [CommerceInterests] (
    [CommerceID] int NOT NULL,
    [InterestsId] int NOT NULL,
    CONSTRAINT [PK_CommerceInterests] PRIMARY KEY ([CommerceID], [InterestsId]),
    CONSTRAINT [FK_CommerceInterests_Commerce_CommerceID] FOREIGN KEY ([CommerceID]) REFERENCES [Commerce] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_CommerceInterests_Interests_InterestsId] FOREIGN KEY ([InterestsId]) REFERENCES [Interests] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_CommerceInterests_InterestsId] ON [CommerceInterests] ([InterestsId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200114000047_Actu', N'2.2.3-servicing-35854');

GO

ALTER TABLE [Discounts] ADD [CommerceId] int NULL;

GO

ALTER TABLE [Discounts] ADD [Description] nvarchar(max) NULL;

GO

DECLARE @var12 sysname;
SELECT @var12 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'LockoutEnd');
IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var12 + '];');
ALTER TABLE [AspNetUsers] ALTER COLUMN [LockoutEnd] datetimeoffset NULL;

GO

CREATE INDEX [IX_Discounts_CommerceId] ON [Discounts] ([CommerceId]);

GO

ALTER TABLE [Discounts] ADD CONSTRAINT [FK_Discounts_Commerce_CommerceId] FOREIGN KEY ([CommerceId]) REFERENCES [Commerce] ([Id]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200114222412_DiscountsC', N'2.2.3-servicing-35854');

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200116225253_Change', N'2.2.3-servicing-35854');

GO

DROP TABLE [UserDiscountConsumed];

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200116231104_z', N'2.2.3-servicing-35854');

GO

CREATE TABLE [UserDiscountConsumed] (
    [Id] int NOT NULL IDENTITY,
    [UserNameID] nvarchar(450) NULL,
    [DiscountID] int NOT NULL,
    CONSTRAINT [PK_UserDiscountConsumed] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserDiscountConsumed_Discounts_DiscountID] FOREIGN KEY ([DiscountID]) REFERENCES [Discounts] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserDiscountConsumed_AspNetUsers_UserNameID] FOREIGN KEY ([UserNameID]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
);

GO

CREATE INDEX [IX_UserDiscountConsumed_DiscountID] ON [UserDiscountConsumed] ([DiscountID]);

GO

CREATE INDEX [IX_UserDiscountConsumed_UserNameID] ON [UserDiscountConsumed] ([UserNameID]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200116231155_z1', N'2.2.3-servicing-35854');

GO

DROP TABLE [QrCode];

GO

DROP TABLE [ValuedDiscount];

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200116231615_z2', N'2.2.3-servicing-35854');

GO

