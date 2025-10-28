CREATE TABLE [training].[Roles] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [Name] NVARCHAR(100) NOT NULL,
    [Description] NVARCHAR(MAX) NOT NULL,
    [CreatedOn] DATETIME2 NOT NULL,
    [CreatedBy] NVARCHAR(MAX) NOT NULL,
    [UpdatedOn] DATETIME2 NULL,
    [UpdatedBy] NVARCHAR(MAX) NULL,
    [IsDeleted] BIT NOT NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

CREATE TABLE [training].[Permissions] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [Name] NVARCHAR(100) NOT NULL,
    [Description] NVARCHAR(MAX) NOT NULL,
    [CreatedOn] DATETIME2 NOT NULL,
    [CreatedBy] NVARCHAR(MAX) NOT NULL,
    [UpdatedOn] DATETIME2 NULL,
    [UpdatedBy] NVARCHAR(MAX) NULL,
    [IsDeleted] BIT NOT NULL,
    CONSTRAINT [PK_Permissions] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

CREATE TABLE [training].[RolePermissions] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [RoleId] INT NOT NULL,
    [PermissionId] INT NOT NULL,
    [CreatedOn] DATETIME2 NOT NULL,
    [CreatedBy] NVARCHAR(MAX) NOT NULL,
    [UpdatedOn] DATETIME2 NULL,
    [UpdatedBy] NVARCHAR(MAX) NULL,
    [IsDeleted] BIT NOT NULL,
    CONSTRAINT [PK_RolePermissions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_RolePermissions_Roles] FOREIGN KEY ([RoleId]) REFERENCES [training].[Roles]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_RolePermissions_Permissions] FOREIGN KEY ([PermissionId]) REFERENCES [training].[Permissions]([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [training].[Users] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [Email] NVARCHAR(100) NOT NULL,
    [PasswordHash] NVARCHAR(MAX) NOT NULL,
    [UserType] NVARCHAR(50) NOT NULL,
    [PhoneNumber] NVARCHAR(20) NOT NULL,
    [IsActive] BIT NOT NULL,
    [CreatedOn] DATETIME2 NOT NULL,
    [CreatedBy] NVARCHAR(MAX) NOT NULL,
    [UpdatedOn] DATETIME2 NULL,
    [UpdatedBy] NVARCHAR(MAX) NULL,
    [IsDeleted] BIT NOT NULL,
    [FirstName] NVARCHAR(100) NOT NULL,
    [MiddleName] NVARCHAR(100) NULL,
    [LastName] NVARCHAR(100) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

CREATE TABLE [training].[UserRoles] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [UserId] INT NOT NULL,
    [RoleId] INT NOT NULL,
    [CreatedOn] DATETIME2 NOT NULL,
    [CreatedBy] NVARCHAR(MAX) NOT NULL,
    [UpdatedOn] DATETIME2 NULL,
    [UpdatedBy] NVARCHAR(MAX) NULL,
    [IsDeleted] BIT NOT NULL,
    CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserRoles_Users] FOREIGN KEY ([UserId]) REFERENCES [training].[Users]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserRoles_Roles] FOREIGN KEY ([RoleId]) REFERENCES [training].[Roles]([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [training].[Banks] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [Name] NVARCHAR(150) NOT NULL,
    [Code] NVARCHAR(20) NOT NULL,
    [HeadOfficeAddress] NVARCHAR(MAX) NOT NULL,
    [ContactNumber] NVARCHAR(MAX) NOT NULL,
    [CreatedOn] DATETIME2 NOT NULL,
    [CreatedBy] NVARCHAR(MAX) NOT NULL,
    [UpdatedOn] DATETIME2 NULL,
    [UpdatedBy] NVARCHAR(MAX) NULL,
    [IsDeleted] BIT NOT NULL,
    CONSTRAINT [PK_Banks] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

CREATE TABLE [training].[Branches] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [BankId] INT NOT NULL,
    [Name] NVARCHAR(150) NOT NULL,
    [BranchCode] NVARCHAR(20) NOT NULL,
    [Address] NVARCHAR(MAX) NOT NULL,
    [IFSCCode] NVARCHAR(20) NOT NULL,
    [ContactNumber] NVARCHAR(MAX) NOT NULL,
    [CreatedOn] DATETIME2 NOT NULL,
    [CreatedBy] NVARCHAR(MAX) NOT NULL,
    [UpdatedOn] DATETIME2 NULL,
    [UpdatedBy] NVARCHAR(MAX) NULL,
    [IsDeleted] BIT NOT NULL,
    CONSTRAINT [PK_Branches] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Branches_Banks] FOREIGN KEY ([BankId]) REFERENCES [training].[Banks]([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [training].[Accounts] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [UserId] INT NOT NULL,
    [BranchId] INT NOT NULL,
    [PowerOfAttorneyUserId] INT NULL,
    [AccountNumber] NVARCHAR(30) NOT NULL,
    [AccountType] NVARCHAR(50) NOT NULL,
    [Currency] NVARCHAR(10) NOT NULL,
    [Balance] DECIMAL(18, 2) NOT NULL,
    [IsMinor] BIT NOT NULL,
    [IsActive] BIT NOT NULL,
    [CreatedOn] DATETIME2 NOT NULL,
    [CreatedBy] NVARCHAR(MAX) NOT NULL,
    [UpdatedOn] DATETIME2 NULL,
    [UpdatedBy] NVARCHAR(MAX) NULL,
    [IsDeleted] BIT NOT NULL,
    CONSTRAINT [PK_Accounts] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Accounts_Users] FOREIGN KEY ([UserId]) REFERENCES [training].[Users]([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Accounts_Branches] FOREIGN KEY ([BranchId]) REFERENCES [training].[Branches]([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Accounts_PowerOfAttorneyUser] FOREIGN KEY ([PowerOfAttorneyUserId]) REFERENCES [training].[Users]([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [training].[Transactions] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [FromAccountId] INT NOT NULL,
    [ToAccountId] INT NOT NULL,
    [Amount] DECIMAL(18, 2) NOT NULL,
    [Currency] NVARCHAR(10) NOT NULL,
    [TransactionType] NVARCHAR(50) NOT NULL,
    [Status] NVARCHAR(50) NOT NULL,
    [TransactionDate] DATETIME2 NOT NULL,
    [Remarks] NVARCHAR(MAX) NULL,
    [AccountId] INT NULL,
    [CreatedOn] DATETIME2 NOT NULL,
    [CreatedBy] NVARCHAR(MAX) NOT NULL,
    [UpdatedOn] DATETIME2 NULL,
    [UpdatedBy] NVARCHAR(MAX) NULL,
    [IsDeleted] BIT NOT NULL,
    CONSTRAINT [PK_Transactions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Transactions_FromAccount] FOREIGN KEY ([FromAccountId]) REFERENCES [training].[Accounts]([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Transactions_ToAccount] FOREIGN KEY ([ToAccountId]) REFERENCES [training].[Accounts]([Id]) ON DELETE NO ACTION
);
GO
