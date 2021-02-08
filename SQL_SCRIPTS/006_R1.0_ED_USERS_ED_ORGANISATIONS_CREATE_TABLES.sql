/****** File: 006_R1.0_ED_USERS_ED_ORGANISATIONS_CREATE_TABLES.sql ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

DECLARE @RC int

EXECUTE @RC = dbo.usr_DB_Add_Database_Update 
   006, 'R1.0','ED_USER, ED_ORGANISATIONS', 
   'CREATE TABLES.'
GO

PRINT N'START: 006_R1.0_ED_USERS_ED_ORGANISATIONS_CREATE_TABLES.'; 
GO



/****** Object:  Table [dbo].[ED_USER_PROFILES]    Script Date: 02/02/2021 14:21:54 ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ED_USER_PROFILES]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ED_USER_PROFILES](
	[UP_GUID] [uniqueidentifier] NOT NULL,
	[USER_ID] [nvarchar](100) NOT NULL,
	[ORG_ID] [nvarchar](10) NULL,
	[UP_ACTIVE_DIRECTORY_NAME] [nvarchar](100) NULL,
	[UP_COMMON_NAME] [nvarchar](100) NULL,
	[UP_TITLE] [varchar](100) NULL,
	[UP_PREFIX] [varchar](10) NULL,
	[UP_GIVEN_NAME] [varchar](50) NULL,
	[UP_FAMILY_NAME] [varchar](50) NULL,
	[UP_SUFFIX] [varchar](50) NULL,
	[UP_ADDRESS_1] [nvarchar](50) NULL,
	[UP_ADDRESS_2] [nvarchar](50) NULL,
	[UP_ADDRESS_CITY] [nvarchar](50) NULL,
	[UP_ADDRESS_POST_CODE] [nvarchar](10) NULL,
	[UP_ADDRESS_STATE] [nvarchar](50) NULL,
	[UP_ADDRESS_COUNTRY] [nvarchar](50) NULL,
	[UP_MOBILE_PHONE] [varchar](20) NULL,
	[UP_TELEPHONE] [varchar](20) NULL,
	[UP_EMAIL_ADDRESS] [varchar](255) NULL,
	[UP_ROLES] [nvarchar](250) NULL,
	[UP_TYPE] [nvarchar](50) NULL,
	[UP_EXPIRY_DATE] [datetime] NULL,
	[UP_UPDATED_BY_USER_ID] [nvarchar](100) NULL,
	[UP_UPDATE_BY] [nvarchar](100) NULL,
	[UP_UPDATE_DATE] [datetime] NULL,
	[UP_DELETED] [bit] NULL,
 CONSTRAINT [PK_USER_PROFILES] PRIMARY KEY CLUSTERED 
(
	[UP_GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

END
GO

/****** Object:  Table [dbo].[ED_ORGANISATIONS]    Script Date: 02/02/2021 14:30:55 ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ED_ORGANISATIONS]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ED_ORGANISATIONS](
	[O_GUID] [uniqueidentifier] NOT NULL,
	[ORG_ID] [nvarchar](10) NOT NULL,
	[O_NAME] [nvarchar](50) NULL,
	[O_ADDRESS_1] [nvarchar](50) NULL,
	[O_ADDRESS_2] [nvarchar](50) NULL,
	[O_ADDRESS_CITY] [nvarchar](50) NULL,
	[O_ADDRESS_POST_CODE] [nvarchar](10) NULL,
	[O_ADDRESS_STATE] [nvarchar](50) NULL,
	[O_COUNTRY] [nvarchar](50) NULL,
	[O_ORG_TYPE] [nvarchar](30) NULL,
	[O_TELEPHONE] [varchar](20) NULL,
	[O_EMAIL_ADDRESS] [varchar](255) NULL,
	[O_UPDATED_BY_USER_ID] [nvarchar](100) NULL,
	[O_UPDATE_BY] [nvarchar](100) NULL,
	[O_UPDATE_DATE] [datetime] NULL,
	[O_DELETED] [bit] NULL,
	[O_AD_GROUP] [varchar](30) NULL
) 
END
GO

IF NOT EXISTS( SELECT 1 FROM ED_USER_PROFILES 
          WHERE (USER_ID = N'Ross' ) )
BEGIN
Insert Into ED_USER_PROFILES 
  ([UP_GUID]
  ,[USER_ID]
  ,[ORG_ID]
  ,[UP_ACTIVE_DIRECTORY_NAME]
  ,[UP_COMMON_NAME]
  ,[UP_EMAIL_ADDRESS]
  ,[UP_ROLES]
  ,[UP_TYPE]
  ,[UP_EXPIRY_DATE]
  ,[UP_UPDATED_BY_USER_ID]
  ,[UP_UPDATE_BY]
  ,[UP_UPDATE_DATE]
  ,[UP_DELETED] ) 
values 	
 (NEWID(),
 'Ross',
 'Evado',
 'Ross',
 'Ross Anderson',
 'ross@evado.com',
 'Administrator',
 'Evado',
 '31 dec 2099',
 'Ross',
 'Ross Anderson',
  GETDATE(), 0 );
 END
 GO
 
IF NOT EXISTS( SELECT 1 FROM ED_ORGANISATIONS 
          WHERE (ORG_ID = N'Evado' ) )
BEGIN
Insert Into ED_ORGANISATIONS 
  (O_GUID,
	 ORG_ID,
   O_NAME,
	 O_ORG_TYPE,
   O_UPDATED_BY_USER_ID,
   O_UPDATE_BY,
   O_UPDATE_DATE,
   O_DELETED ) 
values 	
 (NEWID(),
 'Evado',
 'Evado Digital',
 'Evado',
 'Ross',
 'Ross Anderson',
  GETDATE(), 0 );
 END
 GO
 
 
PRINT N'FINISH: 006_R1.0_ED_USERS_ED_ORGANISATIONS_CREATE_TABLES.'; 
GO
