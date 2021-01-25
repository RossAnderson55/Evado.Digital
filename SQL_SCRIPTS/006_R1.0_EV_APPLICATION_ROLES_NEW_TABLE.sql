/****** File: 006_R1.0_EV_APPLICATION_ROLES_NEW_TABLE.sql ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DECLARE @RC int

EXECUTE @RC = dbo.usr_DB_Add_Database_Update 
   001, 'R1.0','ED_APPLICATION_ROLES ', 
   'ADD NEW TABLE.'

GO

PRINT N'START: 006_R1.0_EV_APPLICATION_ROLES_NEW_TABLE.'; 
GO

/****** Object:  Table dbo.ED_APPLICATION_SETTINGS    Script Date: 12/27/2020 16:06:48 ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.ED_APPLICATION_ROLES') AND type in (N'U'))
BEGIN


CREATE TABLE dbo.ED_APPLICATION_ROLES(
	AS_Guid uniqueidentifier NOT NULL,	
	AR_ROLE_ID nvarchar(10) NOT NULL,
	AR_DESCRIPTION varchar(50) NULL
)  

END
GO



PRINT N'FINISH: 006_R1.0_EV_APPLICATION_ROLES_NEW_TABLE.'; 
go

/*************************************  END OF SQL DB UPDATE SCRIPT *****************************************/


