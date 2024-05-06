IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'ContactAndMainAddressUdt' AND ss.name = N'dbo')
CREATE TYPE [dbo].[ContactAndMainAddressUdt] AS TABLE(
	[ContactId] [int] NOT NULL,
	[ContactName] [varchar](100) COLLATE Latin1_General_CI_AS NULL,
	[City] [varchar](100) COLLATE Latin1_General_CI_AS NULL,
	[Street] [varchar](100) COLLATE Latin1_General_CI_AS NULL,
	PRIMARY KEY CLUSTERED 
(
	[ContactId] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CombineContactName]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[CombineContactName]
(
	@FirstName VARCHAR(50),
	@LastName VARCHAR(50)
)
RETURNS VARCHAR(100)
AS
BEGIN
	RETURN @LastName + '', '' + @FirstName; 
END
' 
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Addresses]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Addresses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[City] [varchar](100) COLLATE Latin1_General_CI_AS NULL,
	[Street] [varchar](100) COLLATE Latin1_General_CI_AS NULL
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[AddressView]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[AddressView]
AS
SELECT     dbo.Addresses.*
FROM         dbo.Addresses
' 
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Contacts]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Contacts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) COLLATE Latin1_General_CI_AS NULL,
	[LastName] [varchar](50) COLLATE Latin1_General_CI_AS NULL,
	[AddressId] [int] NULL
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ContactsView]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[ContactsView]
AS
SELECT     dbo.Contacts.*
FROM         dbo.Contacts
' 
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ContactsWithCityView]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[ContactsWithCityView]
AS
SELECT     dbo.ContactsView.*, dbo.AddressView.City
FROM         dbo.AddressView CROSS JOIN
                      dbo.ContactsView
' 
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SchemaMigrations]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SchemaMigrations](
	[Version] [varchar](50) COLLATE Latin1_General_CI_AS NOT NULL
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddressInsert]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[AddressInsert] AS' 
END
GO
ALTER PROCEDURE [dbo].[AddressInsert]
	@City VARCHAR(50),
	@Street VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
	INSERT INTO Addresses(City, Street)
	VALUES (@City, @Street);
	Return SCOPE_IDENTITY();
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddressContactInsert]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[AddressContactInsert] AS' 
END
GO
ALTER PROCEDURE [dbo].[AddressContactInsert]
	@FirstName VARCHAR(50),
	@LastName VARCHAR(50),
	@City VARCHAR(50),
	@Street VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	-- Call other stored procs.
	DECLARE @AddressId INT;
	EXEC @AddressId = AddressInsert @City, @Street;
	EXEC ContactInsert @FirstName, @LastName, @AddressId;
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ContactInsert]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[ContactInsert] AS' 
END
GO
ALTER PROCEDURE [dbo].[ContactInsert]
	@FirstName VARCHAR(50),
	@LastName VARCHAR(50),
	@AddressId INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
	INSERT INTO Contacts (FirstName, LastName, AddressId)
	VALUES (@FirstName, @LastName, @AddressId)
END

GO
-- Migrations --
Insert Into SchemaMigrations Values ('20101118122220');
Insert Into SchemaMigrations Values ('20101118122225');
Insert Into SchemaMigrations Values ('20101118122227');
