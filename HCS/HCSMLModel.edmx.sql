
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 09/03/2015 04:47:28
-- Generated from EDMX file: G:\freelancing\HCS\hcs latest on Client's Machine\hcs source  Old\hcs\HCS\HCS\HCSMLModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [HCSML];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_product_producttype]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[product] DROP CONSTRAINT [FK_product_producttype];
GO
IF OBJECT_ID(N'[dbo].[FK_purchaseproduct_sellertype]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[purchaseproduct] DROP CONSTRAINT [FK_purchaseproduct_sellertype];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Activity]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Activity];
GO
IF OBJECT_ID(N'[dbo].[bankuser]', 'U') IS NOT NULL
    DROP TABLE [dbo].[bankuser];
GO
IF OBJECT_ID(N'[dbo].[companyuser]', 'U') IS NOT NULL
    DROP TABLE [dbo].[companyuser];
GO
IF OBJECT_ID(N'[dbo].[FeedMillTotalRecievable]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FeedMillTotalRecievable];
GO
IF OBJECT_ID(N'[dbo].[feedmilluser]', 'U') IS NOT NULL
    DROP TABLE [dbo].[feedmilluser];
GO
IF OBJECT_ID(N'[dbo].[individualuser]', 'U') IS NOT NULL
    DROP TABLE [dbo].[individualuser];
GO
IF OBJECT_ID(N'[dbo].[Khata]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Khata];
GO
IF OBJECT_ID(N'[dbo].[KhataTypeCode]', 'U') IS NOT NULL
    DROP TABLE [dbo].[KhataTypeCode];
GO
IF OBJECT_ID(N'[dbo].[login]', 'U') IS NOT NULL
    DROP TABLE [dbo].[login];
GO
IF OBJECT_ID(N'[dbo].[OtherKhata]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OtherKhata];
GO
IF OBJECT_ID(N'[dbo].[product]', 'U') IS NOT NULL
    DROP TABLE [dbo].[product];
GO
IF OBJECT_ID(N'[dbo].[producttype]', 'U') IS NOT NULL
    DROP TABLE [dbo].[producttype];
GO
IF OBJECT_ID(N'[dbo].[purchaseproduct]', 'U') IS NOT NULL
    DROP TABLE [dbo].[purchaseproduct];
GO
IF OBJECT_ID(N'[dbo].[purchaseruser]', 'U') IS NOT NULL
    DROP TABLE [dbo].[purchaseruser];
GO
IF OBJECT_ID(N'[dbo].[saleproduct]', 'U') IS NOT NULL
    DROP TABLE [dbo].[saleproduct];
GO
IF OBJECT_ID(N'[dbo].[saletype]', 'U') IS NOT NULL
    DROP TABLE [dbo].[saletype];
GO
IF OBJECT_ID(N'[dbo].[SellerTotalPayable]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SellerTotalPayable];
GO
IF OBJECT_ID(N'[dbo].[sellertype]', 'U') IS NOT NULL
    DROP TABLE [dbo].[sellertype];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'bankusers'
CREATE TABLE [dbo].[bankusers] (
    [bankid] int IDENTITY(1,1) NOT NULL,
    [bankname_e] nvarchar(50)  NULL,
    [phone] nvarchar(30)  NOT NULL,
    [email] nvarchar(50)  NULL,
    [address_e] nvarchar(300)  NULL,
    [imagepath] nvarchar(max)  NULL,
    [imagetobyte] varbinary(max)  NULL,
    [date] datetime  NOT NULL,
    [bankname_u] nvarchar(50)  NULL,
    [address_u] nchar(300)  NULL,
    [bankaccountnumber] nvarchar(50)  NULL,
    [bankbranch_e] nvarchar(50)  NULL,
    [bankbranch_u] nvarchar(50)  NULL,
    [bankAccountTitle_e] nvarchar(30)  NULL,
    [bankAccountTitle_u] nvarchar(30)  NULL
);
GO

-- Creating table 'companyusers'
CREATE TABLE [dbo].[companyusers] (
    [companyid] int IDENTITY(1,1) NOT NULL,
    [companyname_e] nvarchar(50)  NULL,
    [phone] nvarchar(30)  NOT NULL,
    [email] nvarchar(50)  NULL,
    [address_e] nvarchar(300)  NULL,
    [imagepath] nvarchar(max)  NULL,
    [imagetobyte] varbinary(max)  NULL,
    [date] datetime  NOT NULL,
    [companyname_u] nvarchar(50)  NULL,
    [address_u] nchar(300)  NULL,
    [bankaccountnumber] nvarchar(50)  NULL,
    [bankbranch_e] nvarchar(50)  NULL,
    [bankbranch_u] nvarchar(50)  NULL
);
GO

-- Creating table 'FeedMillTotalRecievables'
CREATE TABLE [dbo].[FeedMillTotalRecievables] (
    [feedmillid] int  NOT NULL,
    [totalPriceRecievable] decimal(18,2)  NOT NULL,
    [totalPayable] decimal(18,2)  NOT NULL
);
GO

-- Creating table 'feedmillusers'
CREATE TABLE [dbo].[feedmillusers] (
    [feedmillid] int IDENTITY(1,1) NOT NULL,
    [feemillname_e] nvarchar(50)  NULL,
    [phone] nvarchar(30)  NOT NULL,
    [email] nvarchar(50)  NULL,
    [address_e] nvarchar(300)  NULL,
    [imagepath] nvarchar(max)  NULL,
    [imagetobyte] varbinary(max)  NULL,
    [date] datetime  NOT NULL,
    [feedmillname_u] nvarchar(50)  NULL,
    [address_u] nchar(300)  NULL,
    [bankaccountnumber] nvarchar(50)  NULL,
    [bankbranch_e] nvarchar(50)  NULL,
    [bankbranch_u] nvarchar(50)  NULL,
    [numOfDealingAuthority] int  NULL,
    [numOfPaymentAuthority] int  NULL
);
GO

-- Creating table 'individualusers'
CREATE TABLE [dbo].[individualusers] (
    [individualid] int IDENTITY(1,1) NOT NULL,
    [name_e] nvarchar(50)  NULL,
    [mobile] nvarchar(30)  NOT NULL,
    [phone] nvarchar(30)  NULL,
    [email] nvarchar(50)  NULL,
    [address_e] nvarchar(300)  NULL,
    [imagepath] nvarchar(max)  NULL,
    [imagetobyte] varbinary(max)  NULL,
    [date] datetime  NOT NULL,
    [name_u] nvarchar(50)  NULL,
    [address_u] nvarchar(300)  NULL,
    [bankaccountnumber] nvarchar(50)  NULL,
    [bankbranch_e] nvarchar(50)  NULL,
    [bankbranch_u] nvarchar(50)  NULL
);
GO

-- Creating table 'logins'
CREATE TABLE [dbo].[logins] (
    [seq_id] int IDENTITY(1,1) NOT NULL,
    [user_id] nvarchar(30)  NOT NULL,
    [password] nvarchar(30)  NOT NULL,
    [can_edit] bit  NOT NULL
);
GO

-- Creating table 'products'
CREATE TABLE [dbo].[products] (
    [productid] int IDENTITY(1,1) NOT NULL,
    [productname_e] nvarchar(30)  NOT NULL,
    [producttype] nvarchar(5)  NOT NULL,
    [productname_u] nvarchar(30)  NOT NULL
);
GO

-- Creating table 'producttypes'
CREATE TABLE [dbo].[producttypes] (
    [product_type_cde] nvarchar(5)  NOT NULL,
    [product_type_dsc] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'purchaseproducts'
CREATE TABLE [dbo].[purchaseproducts] (
    [seqid] int IDENTITY(1,1) NOT NULL,
    [productid] int  NOT NULL,
    [date] datetime  NOT NULL,
    [seller_cde] nvarchar(5)  NOT NULL,
    [sellerid] int  NOT NULL,
    [sellername] nvarchar(30)  NULL,
    [noofbags] int  NOT NULL,
    [rate] decimal(18,2)  NOT NULL,
    [bagweight] decimal(18,2)  NOT NULL,
    [extraweight] decimal(18,2)  NOT NULL,
    [commission] decimal(18,2)  NOT NULL,
    [labour] decimal(18,2)  NOT NULL,
    [purchaserid] int  NOT NULL,
    [commissionamt] decimal(18,2)  NOT NULL,
    [price] decimal(18,2)  NOT NULL,
    [payableprice] decimal(18,2)  NOT NULL,
    [productname] nvarchar(30)  NULL,
    [totalpayable] decimal(18,2)  NOT NULL,
    [amoutpaid] decimal(18,2)  NOT NULL,
    [purchasername] nvarchar(50)  NULL
);
GO

-- Creating table 'purchaserusers'
CREATE TABLE [dbo].[purchaserusers] (
    [purchaserid] int IDENTITY(1,1) NOT NULL,
    [purchasername_e] nvarchar(50)  NOT NULL,
    [mobile] nvarchar(30)  NOT NULL,
    [phone] nvarchar(30)  NULL,
    [address_e] nvarchar(300)  NULL,
    [email] nvarchar(50)  NULL,
    [imagepath] nvarchar(max)  NULL,
    [imagetobyte] varbinary(max)  NULL,
    [date] datetime  NOT NULL,
    [purchasername_u] nvarchar(50)  NULL,
    [address_u] nvarchar(300)  NULL,
    [bankaccountnumber] nvarchar(50)  NULL,
    [bankbranch_e] nvarchar(50)  NULL,
    [bankbranch_u] nvarchar(50)  NULL
);
GO

-- Creating table 'saleproducts'
CREATE TABLE [dbo].[saleproducts] (
    [seqid] int IDENTITY(1,1) NOT NULL,
    [productid] int  NOT NULL,
    [date] datetime  NOT NULL,
    [feedmillid] int  NOT NULL,
    [rate] decimal(18,2)  NOT NULL,
    [drivernumber] nvarchar(30)  NOT NULL,
    [vehicleno] nvarchar(30)  NOT NULL,
    [price] decimal(18,2)  NOT NULL,
    [recievableprice] decimal(18,2)  NOT NULL,
    [labour] decimal(18,2)  NOT NULL,
    [totalpricerecieveable] decimal(18,2)  NOT NULL,
    [amountrecieved] decimal(18,2)  NOT NULL,
    [productname] nvarchar(30)  NOT NULL,
    [noofbags] int  NOT NULL,
    [bagweight] decimal(18,2)  NOT NULL,
    [extraweight] decimal(18,2)  NOT NULL,
    [weight] decimal(18,2)  NOT NULL,
    [totalpayable] decimal(18,2)  NOT NULL,
    [fee] decimal(18,2)  NOT NULL,
    [thread] decimal(18,2)  NOT NULL,
    [accountcharges] decimal(18,2)  NOT NULL,
    [bardana] decimal(18,2)  NOT NULL,
    [commissionpercent] decimal(18,2)  NOT NULL,
    [commissionamount] decimal(18,2)  NOT NULL,
    [purchasertypecde] nvarchar(5)  NOT NULL,
    [purchasername] nvarchar(50)  NULL,
    [saletypecde] nvarchar(5)  NOT NULL,
    [vehiclerent] nvarchar(25)  NOT NULL,
    [sellertypecde] nvarchar(5)  NOT NULL,
    [sellername] nvarchar(25)  NOT NULL
);
GO

-- Creating table 'saletypes'
CREATE TABLE [dbo].[saletypes] (
    [saletypecde] nvarchar(5)  NOT NULL,
    [saletype_e] nvarchar(50)  NULL,
    [saletype_u] nvarchar(50)  NULL
);
GO

-- Creating table 'SellerTotalPayables'
CREATE TABLE [dbo].[SellerTotalPayables] (
    [seller_cde] varchar(5)  NOT NULL,
    [sellerid] int  NOT NULL,
    [totalpayable] decimal(18,2)  NOT NULL,
    [totalreceivable] decimal(18,2)  NOT NULL
);
GO

-- Creating table 'sellertypes'
CREATE TABLE [dbo].[sellertypes] (
    [seller_cde] nvarchar(5)  NOT NULL,
    [seller_des_e] nvarchar(25)  NOT NULL,
    [seller_des_u] nvarchar(25)  NULL
);
GO

-- Creating table 'Activities'
CREATE TABLE [dbo].[Activities] (
    [activitycde] nvarchar(5)  NOT NULL,
    [activityname] nvarchar(10)  NOT NULL
);
GO

-- Creating table 'Khatas'
CREATE TABLE [dbo].[Khatas] (
    [id] int  NOT NULL,
    [sellertypecode] nvarchar(5)  NOT NULL,
    [sellerid] int  NOT NULL,
    [purchasertypecode] nvarchar(5)  NOT NULL,
    [purchaserid] int  NOT NULL,
    [payable_naam] decimal(18,2)  NOT NULL,
    [receivable_jama] decimal(18,2)  NOT NULL,
    [activitycode] nvarchar(5)  NOT NULL,
    [date] datetime  NOT NULL,
    [productid] int  NOT NULL,
    [totalreceivable_jama] decimal(18,2)  NOT NULL,
    [totalpayable_naam] decimal(18,2)  NOT NULL
);
GO

-- Creating table 'KhataTypeCodes'
CREATE TABLE [dbo].[KhataTypeCodes] (
    [khatatypecde] nvarchar(5)  NOT NULL,
    [khataname] nvarchar(15)  NOT NULL
);
GO

-- Creating table 'OtherKhatas'
CREATE TABLE [dbo].[OtherKhatas] (
    [id] int IDENTITY(1,1) NOT NULL,
    [payable_naam] decimal(18,2)  NOT NULL,
    [receivable_jama] decimal(18,2)  NOT NULL,
    [productname] nvarchar(30)  NOT NULL,
    [weight] decimal(18,2)  NOT NULL,
    [date] datetime  NOT NULL,
    [khatatypecde] nvarchar(5)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [bankid] in table 'bankusers'
ALTER TABLE [dbo].[bankusers]
ADD CONSTRAINT [PK_bankusers]
    PRIMARY KEY CLUSTERED ([bankid] ASC);
GO

-- Creating primary key on [companyid] in table 'companyusers'
ALTER TABLE [dbo].[companyusers]
ADD CONSTRAINT [PK_companyusers]
    PRIMARY KEY CLUSTERED ([companyid] ASC);
GO

-- Creating primary key on [feedmillid] in table 'FeedMillTotalRecievables'
ALTER TABLE [dbo].[FeedMillTotalRecievables]
ADD CONSTRAINT [PK_FeedMillTotalRecievables]
    PRIMARY KEY CLUSTERED ([feedmillid] ASC);
GO

-- Creating primary key on [feedmillid] in table 'feedmillusers'
ALTER TABLE [dbo].[feedmillusers]
ADD CONSTRAINT [PK_feedmillusers]
    PRIMARY KEY CLUSTERED ([feedmillid] ASC);
GO

-- Creating primary key on [individualid] in table 'individualusers'
ALTER TABLE [dbo].[individualusers]
ADD CONSTRAINT [PK_individualusers]
    PRIMARY KEY CLUSTERED ([individualid] ASC);
GO

-- Creating primary key on [user_id] in table 'logins'
ALTER TABLE [dbo].[logins]
ADD CONSTRAINT [PK_logins]
    PRIMARY KEY CLUSTERED ([user_id] ASC);
GO

-- Creating primary key on [productid] in table 'products'
ALTER TABLE [dbo].[products]
ADD CONSTRAINT [PK_products]
    PRIMARY KEY CLUSTERED ([productid] ASC);
GO

-- Creating primary key on [product_type_cde] in table 'producttypes'
ALTER TABLE [dbo].[producttypes]
ADD CONSTRAINT [PK_producttypes]
    PRIMARY KEY CLUSTERED ([product_type_cde] ASC);
GO

-- Creating primary key on [seqid] in table 'purchaseproducts'
ALTER TABLE [dbo].[purchaseproducts]
ADD CONSTRAINT [PK_purchaseproducts]
    PRIMARY KEY CLUSTERED ([seqid] ASC);
GO

-- Creating primary key on [purchaserid] in table 'purchaserusers'
ALTER TABLE [dbo].[purchaserusers]
ADD CONSTRAINT [PK_purchaserusers]
    PRIMARY KEY CLUSTERED ([purchaserid] ASC);
GO

-- Creating primary key on [seqid] in table 'saleproducts'
ALTER TABLE [dbo].[saleproducts]
ADD CONSTRAINT [PK_saleproducts]
    PRIMARY KEY CLUSTERED ([seqid] ASC);
GO

-- Creating primary key on [saletypecde] in table 'saletypes'
ALTER TABLE [dbo].[saletypes]
ADD CONSTRAINT [PK_saletypes]
    PRIMARY KEY CLUSTERED ([saletypecde] ASC);
GO

-- Creating primary key on [seller_cde], [sellerid] in table 'SellerTotalPayables'
ALTER TABLE [dbo].[SellerTotalPayables]
ADD CONSTRAINT [PK_SellerTotalPayables]
    PRIMARY KEY CLUSTERED ([seller_cde], [sellerid] ASC);
GO

-- Creating primary key on [seller_cde] in table 'sellertypes'
ALTER TABLE [dbo].[sellertypes]
ADD CONSTRAINT [PK_sellertypes]
    PRIMARY KEY CLUSTERED ([seller_cde] ASC);
GO

-- Creating primary key on [activitycde] in table 'Activities'
ALTER TABLE [dbo].[Activities]
ADD CONSTRAINT [PK_Activities]
    PRIMARY KEY CLUSTERED ([activitycde] ASC);
GO

-- Creating primary key on [id] in table 'Khatas'
ALTER TABLE [dbo].[Khatas]
ADD CONSTRAINT [PK_Khatas]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [khatatypecde] in table 'KhataTypeCodes'
ALTER TABLE [dbo].[KhataTypeCodes]
ADD CONSTRAINT [PK_KhataTypeCodes]
    PRIMARY KEY CLUSTERED ([khatatypecde] ASC);
GO

-- Creating primary key on [id] in table 'OtherKhatas'
ALTER TABLE [dbo].[OtherKhatas]
ADD CONSTRAINT [PK_OtherKhatas]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [producttype] in table 'products'
ALTER TABLE [dbo].[products]
ADD CONSTRAINT [FK_product_producttype]
    FOREIGN KEY ([producttype])
    REFERENCES [dbo].[producttypes]
        ([product_type_cde])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_product_producttype'
CREATE INDEX [IX_FK_product_producttype]
ON [dbo].[products]
    ([producttype]);
GO

-- Creating foreign key on [seller_cde] in table 'purchaseproducts'
ALTER TABLE [dbo].[purchaseproducts]
ADD CONSTRAINT [FK_purchaseproduct_sellertype]
    FOREIGN KEY ([seller_cde])
    REFERENCES [dbo].[sellertypes]
        ([seller_cde])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_purchaseproduct_sellertype'
CREATE INDEX [IX_FK_purchaseproduct_sellertype]
ON [dbo].[purchaseproducts]
    ([seller_cde]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------