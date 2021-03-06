/****** File: 014_R1.0_ED_MENUS_UPDATE_TBL_PROCS.sql ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

DECLARE @RC int

EXECUTE @RC = dbo.usr_DB_Add_Database_Update 
   014, 'R1.0','ED_MENUS', 
   'CREATE STORED PROCEDURES.'
GO

PRINT N'START: 014_R1.0_ED_MENUS_UPDATE_TBL_PROCS.'; 
GO


ALTER TABLE EV_MENUS
 ALTER COLUMN 
  MNU_GROUP  NVARCHAR(10)
GO

IF NOT EXISTS( SELECT 1 FROM sys.columns 
          WHERE Name = N'MNU_PARAMETERS'
          AND Object_ID = Object_ID(N'EV_MENUS' ))
BEGIN
ALTER TABLE EV_MENUS
 ADD 
  MNU_PARAMETERS NVARCHAR(100) NULL
END
GO

IF NOT EXISTS( SELECT 1 FROM EV_MENUS WHERE MNU_GROUP = N'DSGN' AND MNU_PLATFORM = 'ADMIN' )
BEGIN
Insert Into EV_MENUS 
( Mnu_Guid, MNU_PAGE_ID, MNU_TITLE, MNU_ORDER, MNU_GROUP, MNU_GROUP_HEADER, 
	MNU_PLATFORM, MNU_USER_TYPES, MNU_ROLES ) 
values
( NEWID(), 'Home_Page', 'Design',  5, 'DSGN', 1,'ADMIN','', 'Administrator' )
END
GO

IF NOT EXISTS( SELECT 1 FROM EV_MENUS WHERE MNU_GROUP = N'DSGN' AND MNU_PLATFORM = 'PROD' )
BEGIN
Insert Into EV_MENUS 
( Mnu_Guid, MNU_PAGE_ID, MNU_TITLE, MNU_ORDER, MNU_GROUP, MNU_GROUP_HEADER, 
	MNU_PLATFORM, MNU_USER_TYPES, MNU_ROLES ) 
values
( NEWID(), 'Home_Page', 'Design',  5, 'DSGN', 1,'PROD','', 'Administrator' )
END
GO

/****** Object:  StoredProcedure [dbo].[usr_Menu_add]    Script Date: 03/22/2021 15:38:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usr_Menu_add]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usr_Menu_add]
GO

/****** Object:  StoredProcedure [dbo].[usr_Menu_update]    Script Date: 03/22/2021 15:38:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usr_Menu_update]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usr_Menu_update]
GO

/****** Object:  StoredProcedure [dbo].[usr_Menu_update]    Script Date: 03/22/2021 15:38:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usr_Menu_sync]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usr_Menu_update]
GO

/****** Object:  StoredProcedure [dbo].[usr_Menu_add]    Script Date: 03/22/2021 15:38:16 ******/

CREATE    PROCEDURE [dbo].[usr_Menu_add]
 @Guid uniqueidentifier,
 @PLATFORM nvarchar(10),
 @PAGE_ID nvarchar(100),
 @TITLE nvarchar(20),
 @ORDER smallint,
 @GROUP nvarchar(10),
 @GROUP_HEADER bit,
 @USER_TYPES nvarchar(250),
 @ROLES nvarchar(250),
 @PARAMETERS nvarchar(100)
AS

Insert Into EV_MENUS 
( Mnu_Guid, MNU_PAGE_ID, MNU_TITLE, MNU_ORDER, MNU_GROUP, MNU_GROUP_HEADER, 
	MNU_PLATFORM, MNU_USER_TYPES, MNU_ROLES,MNU_PARAMETERS ) 
values
( @Guid, @PAGE_ID, @TITLE,  @ORDER, @GROUP, @GROUP_HEADER,
	@PLATFORM, @USER_TYPES, @ROLES,@PARAMETERS );

GO

/****** Object:  StoredProcedure [dbo].[usr_Menu_add]    Script Date: 03/22/2021 15:38:16 ******/

CREATE    PROCEDURE [dbo].[usr_Menu_sync]
AS

DELETE FROM EV_MENUS
WHERE MNU_PLATFORM = 'PROD';

Insert Into EV_MENUS 
( MNU_GUID, MNU_PAGE_ID, MNU_TITLE, MNU_ORDER, MNU_GROUP, MNU_GROUP_HEADER, 
	MNU_PLATFORM, MNU_USER_TYPES, MNU_ROLES,MNU_PARAMETERS ) 
SELECT
NEWID(), MNU_PAGE_ID, MNU_TITLE, MNU_ORDER, MNU_GROUP, MNU_GROUP_HEADER, 
	'PROD', MNU_USER_TYPES, MNU_ROLES,MNU_PARAMETERS 
 
FROM EV_MENUS 
WHERE (MNU_PLATFORM = 'ADMIN');
GO


/****** Object:  StoredProcedure [dbo].[usr_Menu_update]    Script Date: 03/22/2021 15:38:16 ******/
CREATE   PROCEDURE [dbo].[usr_Menu_update]
 @Guid uniqueidentifier,
 @PLATFORM nvarchar(10),
 @PAGE_ID nvarchar(100),
 @TITLE nvarchar(20),
 @ORDER smallint,
 @GROUP nvarchar(10),
 @GROUP_HEADER bit,
 @USER_TYPES nvarchar(250),
 @ROLES nvarchar(250),
 @PARAMETERS nvarchar(100)
AS

UPDATE 	EV_MENUS 
set 
  MNU_PAGE_ID =  @PAGE_ID,  
  MNU_TITLE =  @TITLE,  
	MNU_ORDER = @ORDER,  
	MNU_GROUP = @GROUP,  
	MNU_GROUP_HEADER = @GROUP_HEADER,  
	MNU_PLATFORM = @PLATFORM,  
  MNU_USER_TYPES = @USER_TYPES, 
  MNU_ROLES  = @ROLES,
  MNU_PARAMETERS = @PARAMETERS
WHERE 	Mnu_Guid = @Guid;




GO




PRINT N'FINISH: 014_R1.0_ED_MENUS_UPDATE_TBL_PROCS.'; 
GO
