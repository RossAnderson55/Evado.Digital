/****** File: 002_R1.0_EV_ADAPTER_SETTINGS_PROCS.sql ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DECLARE @RC int

EXECUTE @RC = dbo.usr_DB_Add_Database_Update 
   002, 'R1.0','ED_ADAPTER_SETTINGS ', 
   'ADD PROCEDURES.'

GO

PRINT N'START: 002_R1.0_EV_ADAPTER_SETTINGS_PROCS.'; 
GO

/****** Object:  StoredProcedure dbo.USR_APPLICATION_SETTINGS_ADD    Script Date: 12/28/2020 10:55:53 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.USR_APPLICATION_SETTINGS_ADD') AND type in (N'P', N'PC'))
DROP PROCEDURE dbo.USR_APPLICATION_SETTINGS_ADD
GO

/****** Object:  StoredProcedure dbo.USR_APPLICATION_SETTINGS_delete    Script Date: 12/28/2020 10:55:53 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.USR_APPLICATION_SETTINGS_delete') AND type in (N'P', N'PC'))
DROP PROCEDURE dbo.USR_APPLICATION_SETTINGS_delete
GO

/****** Object:  StoredProcedure dbo.USR_APPLICATION_SETTINGS_update    Script Date: 12/28/2020 10:55:53 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.USR_APPLICATION_SETTINGS_update') AND type in (N'P', N'PC'))
DROP PROCEDURE dbo.USR_APPLICATION_SETTINGS_update
GO


/****** Object:  StoredProcedure dbo.USR_APPLICATION_SETTINGS_update    Script Date: 12/28/2020 10:55:53 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.USR_ADAPTER_SETTINGS_update') AND type in (N'P', N'PC'))
DROP PROCEDURE dbo.USR_ADAPTER_SETTINGS_UPDATE
GO


/****** Object:  MIGRATES THE APPLICATIN SETTINGS TO ADAPTER SETTINGS.  ******/
IF NOT EXISTS (SELECT * FROM ED_ADAPTER_SETTINGS WHERE AS_GUID = '16EB2352-9058-4B55-8F64-5258172A614B' )
BEGIN
INSERT INTO ED_ADAPTER_SETTINGS
  (AS_GUID
  ,AS_APPLICATION_ID
  ,AS_HOME_PAGE_HEADER
  ,AS_HELP_URL
  ,AS_MAX_SELECTION_LENGTH
  ,AS_SMTP_SERVER
  ,AS_SMTP_PORT
  ,AS_SMTP_USER_ID
  ,AS_SMTP_PASSWORD
  ,AS_ALERT_EMAIL_ADDRESS)
SELECT 
   APS_GUID
  ,APS_APPLICATION_ID
  ,APS_HOME_PAGE_HEADER
  ,APS_HELP_URL
  ,APS_MAX_SELECTION_LENGTH
  ,APS_SMTP_SERVER
  ,APS_SMTP_PORT
  ,APS_SMTP_USER_ID
  ,APS_SMTP_PASSWORD
  ,APS_ALERT_EMAIL_ADDRESS
 FROM ED_PLATFORM_SETTINGS
 WHERE APS_GUID = '16EB2352-9058-4B55-8F64-5258172A614B' 

END
GO


/****** Object:  StoredProcedure dbo.USR_APPLICATION_SETTINGS_update    Script Date: 12/28/2020 10:55:54 ******/

CREATE PROCEDURE dbo.USR_ADAPTER_SETTINGS_UPDATE
  @GUID uniqueidentifier,
	@APPLICATION_ID char(1),
	@HOME_PAGE_HEADER nvarchar(100),
	@HELP_URL nvarchar(50),
	@MAX_SELECTION_LENGTH int,
	@SMTP_SERVER varchar(100),
	@SMTP_PORT int,
	@SMTP_USER_ID nvarchar(100),
	@SMTP_PASSWORD nvarchar(50),
	@ALERT_EMAIL_ADDRESS nvarchar(50),
	@APPLICATION_URL varchar(50),
	@TITLE nvarchar(50),
	@HTTP_REFERENCE nvarchar(250),
	@DESCRIPTION nvarchar(max),
	@ROLES nvarchar(500),
	@UPDATE_USER_ID nvarchar(100),
	@UPDATE_USER nvarchar(100),
	@UPDATE_DATE datetime 
as
UPDATE	ED_ADAPTER_SETTINGS
SET 
  AS_GUID = @GUID,
  AS_APPLICATION_ID = @APPLICATION_ID,
  AS_HOME_PAGE_HEADER = @HOME_PAGE_HEADER,
  AS_HELP_URL = @HELP_URL,
  AS_MAX_SELECTION_LENGTH = @MAX_SELECTION_LENGTH,
	AS_SMTP_SERVER=@SMTP_SERVER,
	AS_SMTP_PORT=@SMTP_PORT,
	AS_SMTP_USER_ID =@SMTP_USER_ID,
	AS_SMTP_PASSWORD =@SMTP_PASSWORD,
	AS_ALERT_EMAIL_ADDRESS =@ALERT_EMAIL_ADDRESS,
	AS_APPLICATION_URL =@APPLICATION_URL,
	AS_TITLE =@TITLE,
	AS_HTTP_REFERENCE =@HTTP_REFERENCE,
	AS_DESCRIPTION =@DESCRIPTION,
	AS_ROLES =@ROLES,
  AS_UPDATE_USER_ID = @UPDATE_USER_ID,
  AS_UPDATE_USER = @UPDATE_USER,
  AS_UPDATE_DATE = @UPDATE_DATE
WHERE AS_GUID = @GUID;


GO


PRINT N'FINISH: 002_R1.0_EV_ADAPTER_SETTINGS_PROCS.'; 
go


/*************************************  END OF SQL DB UPDATE SCRIPT *****************************************/


