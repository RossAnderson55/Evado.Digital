/****** File: 002_R1.0_EV_APPLICATION_SETTINGS_PROCS.sql ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DECLARE @RC int

EXECUTE @RC = [dbo].[usr_DB_Add_Database_Update] 
   002, 'R1.0','ED_APPLICATION_SETTINGS ', 
   'ADD PROCEDURES.'

GO

PRINT N'START: 002_R1.0_EV_APPLICATION_SETTINGS_PROCS.'; 
GO


/****** Object:  StoredProcedure [dbo].[usr_Trial_add]    Script Date: 12/28/2020 10:55:53 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usr_Trial_add]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usr_Trial_add]
GO

/****** Object:  StoredProcedure [dbo].[usr_Trial_delete]    Script Date: 12/28/2020 10:55:53 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usr_Trial_delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usr_Trial_delete]
GO

/****** Object:  StoredProcedure [dbo].[usr_Trial_update]    Script Date: 12/28/2020 10:55:53 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usr_Trial_update]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usr_Trial_update]
GO


/****** Object:  StoredProcedure [dbo].[USR_APPLICATION_SETTINGS_ADD]    Script Date: 12/28/2020 10:55:53 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USR_APPLICATION_SETTINGS_ADD]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USR_APPLICATION_SETTINGS_ADD]
GO

/****** Object:  StoredProcedure [dbo].[USR_APPLICATION_SETTINGS_delete]    Script Date: 12/28/2020 10:55:53 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USR_APPLICATION_SETTINGS_delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USR_APPLICATION_SETTINGS_delete]
GO

/****** Object:  StoredProcedure [dbo].[USR_APPLICATION_SETTINGS_update]    Script Date: 12/28/2020 10:55:53 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USR_APPLICATION_SETTINGS_update]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USR_APPLICATION_SETTINGS_update]
GO


/****** Object:  StoredProcedure [dbo].[USR_APPLICATION_SETTINGS_ADD]    Script Date: 12/28/2020 10:55:53 ******/
CREATE PROCEDURE [dbo].[USR_APPLICATION_SETTINGS_ADD]
  @Guid uniqueidentifier,
	@CUSTOMER_GUID uniqueidentifier,
	@STATE varchar(20),
	@TITLE nvarchar(50),
	@HTTP_REFERENCE nvarchar(250),
	@DESCRIPTION ntext,
	@CONFIRMATION_EMAIL_TITLE nvarchar(100),
	@CONFIRMATION_EMAIL_BODY ntext,
	@SIGN_OFFS ntext,
	@UPDATE_USER_ID nvarchar(100),
	@UPDATE_USER nvarchar(100),
	@UPDATE_DATE datetime
AS

DECLARE @SERIAL_NO INT
DECLARE @APP_ID VARCHAR(6)
DECLARE @CUSTOMER_NO INT
DECLARE @CUSTOMER_ID VARCHAR(3)

SELECT  @CUSTOMER_NO =  CU_CUSTOMER_NO
FROM 	EV_CUSTOMERS 
WHERE CU_GUID = @CUSTOMER_GUID;

SELECT 	@SERIAL_NO = COUNT(AS_Guid)
FROM 	ED_APPLICATION_SETTINGS 
WHERE CU_GUID = @CUSTOMER_GUID ;

SET @APP_ID =  RIGHT( '000' + CONVERT(VARCHAR(2), @CUSTOMER_NO), 2 )
  + RIGHT( '000' + CONVERT(VARCHAR(2), @SERIAL_NO), 2 )  ;


INSERT INTO ED_APPLICATION_SETTINGS 
( AS_Guid,
  CU_Guid,
  APPLICATION_ID,
  AS_STATE,
  AS_TITLE,
  AS_HTTP_REFERENCE,
  AS_DESCRIPTION,
  AS_CONFIRMATION_EMAIL_TITLE,
  AS_CONFIRMATION_EMAIL_BODY,
  AS_SIGN_OFFS,
  AS_UPDATE_USER_ID,
  AS_UPDATE_USER,
  AS_UPDATE_DATE,
  AS_DELETED ) 
VALUES
 (@Guid, 
  @CUSTOMER_GUID,
  @APP_ID,
  @STATE,
  @TITLE,
  @HTTP_REFERENCE,
  @DESCRIPTION,
  @CONFIRMATION_EMAIL_TITLE,
  @CONFIRMATION_EMAIL_BODY,
  @SIGN_OFFS,
  @UPDATE_USER_ID,
  @UPDATE_USER,
  @UPDATE_DATE,
  0 );

GO

/****** Object:  StoredProcedure [dbo].[USR_APPLICATION_SETTINGS_delete]    Script Date: 12/28/2020 10:55:54 ******/

CREATE  PROCEDURE [dbo].[USR_APPLICATION_SETTINGS_DELETE]
  @Guid uniqueidentifier,
  @UPDATE_USER_ID nvarchar(100),
	@UPDATE_USER nvarchar(100),
	@UPDATE_DATE datetime
AS

UPDATE 	ED_APPLICATION_SETTINGS 
SET	
  AS_UPDATE_USER_ID = @UPDATE_USER_ID,
  AS_UPDATE_USER = @UPDATE_USER,
  AS_UPDATE_DATE = @UPDATE_DATE,
  AS_DELETED = -1 
WHERE	AS_Guid = @Guid;

GO

/****** Object:  StoredProcedure [dbo].[USR_APPLICATION_SETTINGS_update]    Script Date: 12/28/2020 10:55:54 ******/

CREATE PROCEDURE [dbo].[USR_APPLICATION_SETTINGS_UPDATE]
  @Guid uniqueidentifier,
	@CUSTOMER_GUID uniqueidentifier,
	@STATE varchar(20),
	@TITLE nvarchar(50),
	@HTTP_REFERENCE nvarchar(250),
	@DESCRIPTION ntext,
	@CONFIRMATION_EMAIL_TITLE nvarchar(100),
	@CONFIRMATION_EMAIL_BODY ntext,
	@SIGN_OFFS ntext,
	@UPDATE_USER_ID nvarchar(100),
	@UPDATE_USER nvarchar(100),
	@UPDATE_DATE datetime
as
UPDATE	ED_APPLICATION_SETTINGS
SET 
  AS_STATE = @STATE,
  AS_TITLE = @TITLE,
  AS_HTTP_REFERENCE = @HTTP_REFERENCE,
  AS_DESCRIPTION = @DESCRIPTION,
  AS_CONFIRMATION_EMAIL_TITLE = @CONFIRMATION_EMAIL_TITLE,
  AS_CONFIRMATION_EMAIL_BODY = @CONFIRMATION_EMAIL_BODY,
  AS_SIGN_OFFS = @SIGN_OFFS,
  AS_UPDATE_USER_ID = @UPDATE_USER_ID,
  AS_UPDATE_USER = @UPDATE_USER,
  AS_UPDATE_DATE = @UPDATE_DATE
WHERE 	AS_DELETED = 0 AND  AS_Guid = @Guid;


GO




PRINT N'FINISH: 002_R1.0_EV_APPLICATION_SETTINGS_PROCS.'; 
go

/*************************************  END OF SQL DB UPDATE SCRIPT *****************************************/


