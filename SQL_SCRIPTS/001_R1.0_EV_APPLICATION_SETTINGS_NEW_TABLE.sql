/****** File: 001_R1.0_EV_APPLICATION_SETTINGS_NEW_TABLE.sql ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DECLARE @RC int

EXECUTE @RC = [dbo].[usr_DB_Add_Database_Update] 
   001, 'R1.0','ED_APPLICATION_SETTINGS ', 
   'ADD NEW TABLE.'

GO

PRINT N'START: 001_R1.0_EV_APPLICATION_SETTINGS_NEW_TABLE.'; 
GO

/****** Object:  Table [dbo].[ED_APPLICATION_SETTINGS]    Script Date: 12/27/2020 16:06:48 ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ED_APPLICATION_SETTINGS]') AND type in (N'U'))
BEGIN


CREATE TABLE [dbo].[ED_APPLICATION_SETTINGS](
	[AS_Guid] [uniqueidentifier] NOT NULL,	
	[CU_Guid] [uniqueidentifier] NOT NULL,
	[APPLICATION_ID] [nvarchar](10) NOT NULL,
	[AS_STATE] [varchar](20) NULL,
	[AS_TITLE] [nvarchar](50) NULL,
	[AS_HTTP_REFERENCE] [nvarchar](250) NULL,
	[AS_DESCRIPTION] [nvarchar](max) NULL,
	[AS_CONFIRMATION_EMAIL_TITLE] [nvarchar](100) NULL,
	[AS_CONFIRMATION_EMAIL_BODY] [ntext] NULL,
	[AS_SIGN_OFFS] [ntext] NULL,
	[AS_UPDATE_USER_ID] [nvarchar](100) NULL,
	[AS_UPDATE_USER] [nvarchar](100) NULL,
	[AS_UPDATE_DATE] [datetime] NULL,
	[AS_DELETED] [bit] NULL,
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

END
GO

SET ANSI_PADDING OFF
GO


/****** Object:  Table [dbo].[ED_APPLICATION_SETTINGS]    Script Date: 12/27/2020 16:06:48 ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ED_APPLICATION_SETTINGS]') AND type in (N'U'))
BEGIN
ALTER TABLE [dbo].[ED_APPLICATION_SETTINGS] ADD  CONSTRAINT [DF_APP_AS_DELETED]  DEFAULT ((0)) FOR [AS_DELETED]
END
GO




PRINT N'FINISH: 001_R1.0_EV_APPLICATION_SETTINGS_NEW_TABLE.'; 
go

/*************************************  END OF SQL DB UPDATE SCRIPT *****************************************/


