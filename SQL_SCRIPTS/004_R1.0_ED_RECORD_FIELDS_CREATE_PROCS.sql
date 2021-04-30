/****** File: 004_R1.0_ED_RECORD_FIELDS_CREATE_PROCS.sql ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

DECLARE @RC int

EXECUTE @RC = dbo.usr_DB_Add_Database_Update 
   004, 'R1.0','ED_RECORD_FIELDS', 
   'CREATE STORED PROCEDURES.'

GO

PRINT N'START: 004_R1.0_ED_RECORD_FIELDS_CREATE_PROCS.'; 
GO

/****** Object:  StoredProcedure dbo.USR_RECORD_LAYOUT_FIELD_ADD    Script Date: 01/04/2021 11:39:53 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.USR_RECORD_LAYOUT_FIELD_ADD') AND type in (N'P', N'PC'))
DROP PROCEDURE dbo.USR_RECORD_LAYOUT_FIELD_ADD
GO

/****** Object:  StoredProcedure dbo.USR_RECORD_LAYOUT_FIELD_DELETE    Script Date: 01/04/2021 11:39:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.USR_RECORD_LAYOUT_FIELD_DELETE') AND type in (N'P', N'PC'))
DROP PROCEDURE dbo.USR_RECORD_LAYOUT_FIELD_DELETE
GO

/****** Object:  StoredProcedure dbo.USR_RECORD_LAYOUT_FIELD_UPDATE    Script Date: 01/04/2021 11:39:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.USR_RECORD_LAYOUT_FIELD_UPDATE') AND type in (N'P', N'PC'))
DROP PROCEDURE dbo.USR_RECORD_LAYOUT_FIELD_UPDATE
GO

/****** Object:  StoredProcedure dbo.USR_RECORD_FIELD_ADD    Script Date: 01/04/2021 11:39:53 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.USR_RECORD_FIELD_ADD') AND type in (N'P', N'PC'))
DROP PROCEDURE dbo.USR_RECORD_FIELD_ADD
GO

/****** Object:  StoredProcedure dbo.USR_RECORD_FIELD_DELETE    Script Date: 01/04/2021 11:39:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.USR_RECORD_FIELD_DELETE') AND type in (N'P', N'PC'))
DROP PROCEDURE dbo.USR_RECORD_FIELD_DELETE
GO

/****** Object:  StoredProcedure dbo.USR_RECORD_FIELD_UPDATE    Script Date: 01/04/2021 11:39:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.USR_RECORD_FIELD_UPDATE') AND type in (N'P', N'PC'))
DROP PROCEDURE dbo.USR_RECORD_FIELD_UPDATE
GO

/****** Object:  View [dbo].[ED_RECORD_FIELD_VIEW]    Script Date: 01/06/2021 16:56:23 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ED_RECORD_FIELD_VIEW]'))
DROP VIEW [dbo].[ED_RECORD_FIELD_VIEW]
GO

/****** Object:  View [dbo].[ED_RECORD_FIELD_VIEW]    Script Date: 01/05/2021 11:30:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ED_RECORD_FIELD_VIEW]'))
DROP VIEW [dbo].[ED_RECORD_FIELD_VIEW]
GO

/****** Object:  View [dbo].[ED_RECORD_FIELD_VIEW]    Script Date: 01/05/2021 11:30:49 ******/

CREATE VIEW [dbo].[ED_RECORD_FIELD_VIEW]
AS
SELECT 
  dbo.ED_RECORD_LAYOUTS.EDRL_GUID, 
  dbo.ED_RECORD_LAYOUTS.EDR_LAYOUT_ID, 
  dbo.ED_RECORD_LAYOUTS.EDRL_STATE, 
  dbo.ED_RECORD_LAYOUTS.EDRL_TITLE, 
  dbo.ED_RECORD_FIELDS.EDRLF_GUID,  
  dbo.ED_RECORD_FIELDS.EDRLF_FIELD_ID, 
  dbo.ED_RECORD_FIELDS.EDRLF_TYPE_ID, 
  dbo.ED_RECORD_FIELDS.EDRLF_ORDER, 
  dbo.ED_RECORD_FIELDS.EDRLF_TITLE, 
  dbo.ED_RECORD_FIELDS.EDRLF_INSTRUCTIONS, 
  dbo.ED_RECORD_FIELDS.EDRLF_HTTP_REFERENCE, 
  dbo.ED_RECORD_FIELDS.EDRLF_SECTION_ID, 
  dbo.ED_RECORD_FIELDS.EDRLF_OPTIONS, 
  dbo.ED_RECORD_FIELDS.EDRLF_SUMMARY_FIELD, 
  dbo.ED_RECORD_FIELDS.EDRLF_MANDATORY, 
  dbo.ED_RECORD_FIELDS.EDRLF_AI_DATA_POINT, 
  dbo.ED_RECORD_FIELDS.EDRLF_ANALYTICS_DATA_POINT, 
  dbo.ED_RECORD_FIELDS.EDRLF_HIDDEN, 
  dbo.ED_RECORD_FIELDS.EDRLF_EX_SELECTION_LIST_ID, 
  dbo.ED_RECORD_FIELDS.EDRLF_EX_SELECTION_LIST_CATEGORY, 
  dbo.ED_RECORD_FIELDS.EDRLF_DEFAULT_VALUE, 
  dbo.ED_RECORD_FIELDS.EDRLF_UNIT, 
  dbo.ED_RECORD_FIELDS.EDRLF_UNIT_SCALING, 
  dbo.ED_RECORD_FIELDS.EDRLF_VALIDATION_LOWER_LIMIT, 
  dbo.ED_RECORD_FIELDS.EDRLF_VALIDATION_UPPER_LIMIT, 
  dbo.ED_RECORD_FIELDS.EDRLF_ALERT_LOWER_LIMIT, 
  dbo.ED_RECORD_FIELDS.EDRLF_ALERT_UPPER_LIMIT, 
  dbo.ED_RECORD_FIELDS.EDRLF_NORMAL_LOWER_LIMIT, 
  dbo.ED_RECORD_FIELDS.EDRLF_NORMAL_UPPER_LIMIT, 
  dbo.ED_RECORD_FIELDS.EDRLF_FIELD_CATEGORY, 
  dbo.ED_RECORD_FIELDS.EDRLF_ANALOGUE_LEGEND_START, 
  dbo.ED_RECORD_FIELDS.EDRLF_ANALOGUE_LEGEND_FINISH, 
  dbo.ED_RECORD_FIELDS.EDRLF_JAVA_SCRIPT, 
  dbo.ED_RECORD_FIELDS.EDRLF_TABLE, 
  dbo.ED_RECORD_FIELDS.EDRLF_INITIAL_OPTION_LIST, 
  dbo.ED_RECORD_FIELDS.EDRLF_FIELD_LAYOUT,
  dbo.ED_RECORD_FIELDS.EDRLF_INITIAL_VERSION, 
  dbo.ED_RECORD_FIELDS.EDRLF_DELETED
FROM  dbo.ED_RECORD_LAYOUTS INNER JOIN
  dbo.ED_RECORD_FIELDS ON dbo.ED_RECORD_LAYOUTS.EDRL_GUID = dbo.ED_RECORD_FIELDS.EDRL_GUID

GO


/****** Object:  StoredProcedure dbo.USR_RECORD_FIELD_ADD    Script Date: 01/04/2021 11:39:54 ******/

CREATE PROCEDURE dbo.USR_RECORD_FIELD_ADD
  @LAYOUT_GUID uniqueidentifier,
	@GUID uniqueidentifier  ,
	@LAYOUT_ID nvarchar(20) ,
	@FIELD_ID nvarchar(20)  ,
	@TYPE_ID varchar(30) ,
	@ORDER smallint ,
	@TITLE nvarchar(150) ,
	@INSTRUCTIONS ntext ,
	@HTTP_REFERENCE nvarchar(150) ,
	@SECTION_ID smallint ,
	@OPTIONS nvarchar(256) ,
	@SUMMARY_FIELD bit ,
	@MANDATORY bit ,
	@AI_DATA_POINT bit ,
	@ANALYTICS_DATA_POINT bit,
	@HIDDEN bit ,
  @EX_SELECTION_LIST_ID nvarchar(250),
  @EX_SELECTION_LIST_CATEGORY nvarchar(250),
	@DEFAULT_VALUE nvarchar(100) ,
	@UNIT nvarchar(15) ,
	@UNIT_SCALING nvarchar(15) ,
	@VALIDATION_LOWER_LIMIT float ,
	@VALIDATION_UPPER_LIMIT float ,
	@ALERT_LOWER_LIMIT float ,
	@ALERT_UPPER_LIMIT float ,
	@NORMAL_LOWER_LIMIT int ,
	@NORMAL_UPPER_LIMIT int ,
	@FIELD_CATEGORY varchar(50) ,
	@ANALOGUE_LEGEND_START varchar(30) ,
	@ANALOGUE_LEGEND_FINISH varchar(30) ,
	@JAVA_SCRIPT ntext ,
	@TABLE ntext ,
	@FIELD_LAYOUT NVARCHAR(50),
	@INITIAL_OPTION_LIST nvarchar(250) ,
	@INITIAL_VERSION smallint
AS

INSERT INTO ED_RECORD_FIELDS 
 ( EDRL_GUID
  ,EDRLF_GUID
  ,EDR_LAYOUT_ID
  ,EDRLF_FIELD_ID
  ,EDRLF_TYPE_ID
  ,EDRLF_ORDER
  ,EDRLF_TITLE
  ,EDRLF_INSTRUCTIONS
  ,EDRLF_HTTP_REFERENCE
  ,EDRLF_SECTION_ID
  ,EDRLF_OPTIONS
  ,EDRLF_SUMMARY_FIELD
  ,EDRLF_MANDATORY
  ,EDRLF_AI_DATA_POINT
  ,EDRLF_ANALYTICS_DATA_POINT
  ,EDRLF_HIDDEN
  ,EDRLF_EX_SELECTION_LIST_ID
  ,EDRLF_EX_SELECTION_LIST_CATEGORY
  ,EDRLF_DEFAULT_VALUE
  ,EDRLF_UNIT
  ,EDRLF_UNIT_SCALING
  ,EDRLF_VALIDATION_LOWER_LIMIT
  ,EDRLF_VALIDATION_UPPER_LIMIT
  ,EDRLF_ALERT_LOWER_LIMIT
  ,EDRLF_ALERT_UPPER_LIMIT
  ,EDRLF_NORMAL_LOWER_LIMIT
  ,EDRLF_NORMAL_UPPER_LIMIT
  ,EDRLF_FIELD_CATEGORY
  ,EDRLF_ANALOGUE_LEGEND_START
  ,EDRLF_ANALOGUE_LEGEND_FINISH
  ,EDRLF_JAVA_SCRIPT
  ,EDRLF_TABLE
  ,EDRLF_INITIAL_OPTION_LIST
  ,EDRLF_FIELD_LAYOUT
  ,EDRLF_INITIAL_VERSION
  ,EDRLF_DELETED ) 
VALUES 
 ( @LAYOUT_GUID
  ,@GUID
  ,@LAYOUT_ID
  ,@FIELD_ID
  ,@TYPE_ID
  ,@ORDER
  ,@TITLE
  ,@INSTRUCTIONS
  ,@HTTP_REFERENCE
  ,@SECTION_ID
  ,@OPTIONS
  ,@SUMMARY_FIELD
  ,@MANDATORY
  ,@AI_DATA_POINT
  ,@ANALYTICS_DATA_POINT
  ,@HIDDEN
  ,@EX_SELECTION_LIST_ID
  ,@EX_SELECTION_LIST_CATEGORY
  ,@DEFAULT_VALUE
  ,@UNIT
  ,@UNIT_SCALING
  ,@VALIDATION_LOWER_LIMIT
  ,@VALIDATION_UPPER_LIMIT
  ,@ALERT_LOWER_LIMIT
  ,@ALERT_UPPER_LIMIT
  ,@NORMAL_LOWER_LIMIT
  ,@NORMAL_UPPER_LIMIT
  ,@FIELD_CATEGORY
  ,@ANALOGUE_LEGEND_START
  ,@ANALOGUE_LEGEND_FINISH
  ,@JAVA_SCRIPT
  ,@TABLE
  ,@INITIAL_OPTION_LIST
  ,@FIELD_LAYOUT
  ,@INITIAL_VERSION
  ,0 );

GO

/****** Object:  StoredProcedure dbo.USR_RECORD_FIELD_DELETE    Script Date: 01/04/2021 11:39:54 ******/
CREATE   PROCEDURE dbo.USR_RECORD_FIELD_DELETE 
 @Guid UniqueIdentifier
AS

UPDATE ED_RECORD_FIELDS 
SET 
  EDRLF_DELETED = -1 
WHERE 	EDRLF_DELETED = 0 
	AND EDRLF_GUID = @Guid ;
GO

/****** Object:  StoredProcedure dbo.USR_RECORD_FIELD_UPDATE    Script Date: 01/04/2021 11:39:54 ******/

CREATE PROCEDURE dbo.USR_RECORD_FIELD_UPDATE
  @LAYOUT_GUID uniqueidentifier,
	@GUID uniqueidentifier  ,
	@LAYOUT_ID nvarchar(20) ,
	@FIELD_ID nvarchar(20)  ,
	@TYPE_ID varchar(30) ,
	@ORDER smallint ,
	@TITLE nvarchar(150) ,
	@INSTRUCTIONS ntext ,
	@HTTP_REFERENCE nvarchar(150) ,
	@SECTION_ID smallint ,
	@OPTIONS nvarchar(256) ,
	@SUMMARY_FIELD bit ,
	@MANDATORY bit ,
	@AI_DATA_POINT bit ,
	@ANALYTICS_DATA_POINT bit,
	@HIDDEN bit ,
  @EX_SELECTION_LIST_ID nvarchar(250),
  @EX_SELECTION_LIST_CATEGORY nvarchar(250),
	@DEFAULT_VALUE nvarchar(100) ,
	@UNIT nvarchar(15) ,
	@UNIT_SCALING nvarchar(15) ,
	@VALIDATION_LOWER_LIMIT float ,
	@VALIDATION_UPPER_LIMIT float ,
	@ALERT_LOWER_LIMIT float ,
	@ALERT_UPPER_LIMIT float ,
	@NORMAL_LOWER_LIMIT int ,
	@NORMAL_UPPER_LIMIT int ,
	@FIELD_CATEGORY varchar(50) ,
	@ANALOGUE_LEGEND_START varchar(30) ,
	@ANALOGUE_LEGEND_FINISH varchar(30) ,
	@JAVA_SCRIPT ntext ,
	@TABLE ntext ,
	@FIELD_LAYOUT NVARCHAR(50),
	@INITIAL_OPTION_LIST nvarchar(250) ,
	@INITIAL_VERSION smallint
AS

UPDATE 	ED_RECORD_FIELDS 
SET	
   EDR_LAYOUT_ID = @LAYOUT_ID
  ,EDRLF_FIELD_ID = @FIELD_ID
  ,EDRLF_TYPE_ID = @TYPE_ID
  ,EDRLF_ORDER = @ORDER
  ,EDRLF_TITLE = @TITLE
  ,EDRLF_INSTRUCTIONS = @INSTRUCTIONS
  ,EDRLF_HTTP_REFERENCE = @HTTP_REFERENCE
  ,EDRLF_SECTION_ID = @SECTION_ID
  ,EDRLF_OPTIONS = @OPTIONS
  ,EDRLF_SUMMARY_FIELD = @SUMMARY_FIELD
  ,EDRLF_MANDATORY = @MANDATORY
  ,EDRLF_AI_DATA_POINT = @AI_DATA_POINT
  ,EDRLF_ANALYTICS_DATA_POINT = @ANALYTICS_DATA_POINT
  ,EDRLF_HIDDEN = @HIDDEN
  ,EDRLF_EX_SELECTION_LIST_ID = @EX_SELECTION_LIST_ID
  ,EDRLF_EX_SELECTION_LIST_CATEGORY = @EX_SELECTION_LIST_CATEGORY
  ,EDRLF_DEFAULT_VALUE = @DEFAULT_VALUE
  ,EDRLF_UNIT = @UNIT
  ,EDRLF_UNIT_SCALING = @UNIT_SCALING
  ,EDRLF_VALIDATION_LOWER_LIMIT = @VALIDATION_LOWER_LIMIT
  ,EDRLF_VALIDATION_UPPER_LIMIT = @VALIDATION_UPPER_LIMIT
  ,EDRLF_ALERT_LOWER_LIMIT = @ALERT_LOWER_LIMIT
  ,EDRLF_ALERT_UPPER_LIMIT = @ALERT_UPPER_LIMIT
  ,EDRLF_NORMAL_LOWER_LIMIT = @NORMAL_LOWER_LIMIT
  ,EDRLF_NORMAL_UPPER_LIMIT = @NORMAL_UPPER_LIMIT
  ,EDRLF_FIELD_CATEGORY = @FIELD_CATEGORY
  ,EDRLF_ANALOGUE_LEGEND_START = @ANALOGUE_LEGEND_START
  ,EDRLF_ANALOGUE_LEGEND_FINISH = @ANALOGUE_LEGEND_FINISH
  ,EDRLF_JAVA_SCRIPT = @JAVA_SCRIPT
  ,EDRLF_TABLE = @TABLE
  ,EDRLF_INITIAL_OPTION_LIST = @INITIAL_OPTION_LIST
  ,EDRLF_FIELD_LAYOUT = @FIELD_LAYOUT
  ,EDRLF_INITIAL_VERSION = @INITIAL_VERSION
FROM ED_RECORD_FIELDS 
WHERE  (EDRLF_GUID = @GUID );

GO




PRINT N'FINISH: 004_R1.0_ED_RECORD_FIELDS_CREATE_PROCS.'; 
GO
