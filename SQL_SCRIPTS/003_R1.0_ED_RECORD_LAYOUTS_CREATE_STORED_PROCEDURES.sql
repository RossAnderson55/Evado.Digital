/****** File: 003_R1.0_ED_RECORD_LAYOUTS_CREATE_STORED_PROCEDURES.sql ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

DECLARE @RC int

EXECUTE @RC = dbo.usr_DB_Add_Database_Update 
   003, 'R1.0','ED_RECORD_LAYOUTS', 
   'CREATE STORED PROCEDURES.'

GO

PRINT N'START: 003_R1.0_ED_RECORD_LAYOUTS_CREATE_STORED_PROCEDURES.'; 
GO

/****** Object:  StoredProcedure dbo.USR_RECORD_LAYOUT_ADD    Script Date: 01/04/2021 10:32:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.USR_RECORD_LAYOUT_ADD') AND type in (N'P', N'PC'))
DROP PROCEDURE dbo.USR_RECORD_LAYOUT_ADD
GO

/****** Object:  StoredProcedure dbo.USR_RECORD_LAYOUT_COPY    Script Date: 01/04/2021 10:32:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.USR_RECORD_LAYOUT_COPY') AND type in (N'P', N'PC'))
DROP PROCEDURE dbo.USR_RECORD_LAYOUT_COPY
GO

/****** Object:  StoredProcedure dbo.USR_RECORD_LAYOUT_DELETE    Script Date: 01/04/2021 10:32:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.USR_RECORD_LAYOUT_DELETE') AND type in (N'P', N'PC'))
DROP PROCEDURE dbo.USR_RECORD_LAYOUT_DELETE
GO

/****** Object:  StoredProcedure dbo.USR_RECORD_LAYOUT_UPDATE    Script Date: 01/04/2021 10:32:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.USR_RECORD_LAYOUT_UPDATE') AND type in (N'P', N'PC'))
DROP PROCEDURE dbo.USR_RECORD_LAYOUT_UPDATE
GO

/****** Object:  StoredProcedure dbo.USR_RECORD_LAYOUT_WITHDRAWN    Script Date: 01/04/2021 10:32:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.USR_RECORD_LAYOUT_WITHDRAWN') AND type in (N'P', N'PC'))
DROP PROCEDURE dbo.USR_RECORD_LAYOUT_WITHDRAWN
GO

/****** Object:  View dbo.ED_RECORD_LAYOUT_VIEW    Script Date: 01/05/2021 10:02:06 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'dbo.ED_RECORD_LAYOUT_VIEW'))
DROP VIEW dbo.ED_RECORD_LAYOUT_VIEW
GO

/****** Object:  View [dbo].[ED_RECORD_LAYOUT_ISSUED]    Script Date: 01/07/2021 11:24:29 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ED_RECORD_LAYOUT_ISSUED]'))
DROP VIEW [dbo].[ED_RECORD_LAYOUT_ISSUED]
GO

/****** Object:  View dbo.ED_RECORD_LAYOUT_VIEW    Script Date: 01/05/2021 10:02:06 ******/
CREATE VIEW dbo.ED_RECORD_LAYOUT_VIEW
AS
SELECT 
   EDRL_GUID
  ,EDR_LAYOUT_ID
  ,EDRL_STATE
  ,EDRL_TYPE_ID
  ,EDRL_TITLE
  ,EDRL_HTTP_REFERENCE
  ,EDRL_INSTRUCTIONS
  ,EDRL_DESCRIPTION
  ,EDRL_UPDATE_REASON
  ,EDRL_RECORD_CATEGORY
  ,EDRL_VERSION
  ,EDRL_JAVA_SCRIPT
  ,EDRL_HAS_CS_SCRIPT
  ,EDRL_LANGUAGE
  ,EDRL_RECORD_PREFIX
  ,EDRL_READ_ACCESS_ROLES
  ,EDRL_EDIT_ACCESS_ROLES
  ,EDRL_PARENT_TYPE
  ,EDRL_PARENT_ACCESS
  ,EDRL_PARENT_ENTITIES
  ,EDRL_LINK_CONTENT_SETTING
  ,EDRL_DEFAULT_PAGE_LAYOUT
  ,EDRL_DISPLAY_ENTITIES
  ,EDRL_DISPLAY_AUTHOR_DETAILS
  ,[EDRL_HEADER_FORMAT]
  ,[EDRL_FOOTER_FORMAT]
  ,[EDRL_FILTER_FIELD_0]
  ,[EDRL_FILTER_FIELD_1]
  ,[EDRL_FILTER_FIELD_2]
  ,[EDRL_FILTER_FIELD_3]
  ,[EDRL_FILTER_FIELD_4]
  ,[EDRL_FIELD_DISPLAY_FORMAT]
  ,EDRL_UPDATED_BY_USER_ID
  ,EDRL_UPDATED_BY
  ,EDRL_UPDATED_DATE
  ,EDRL_DELETED
FROM        ED_RECORD_LAYOUTS
WHERE     (EDRL_DELETED = 0)

GO
/****** Object:  View [dbo].[ED_RECORD_LAYOUT_ISSUED]    Script Date: 01/07/2021 11:24:29 ******/

CREATE VIEW [dbo].[ED_RECORD_LAYOUT_ISSUED]
AS
SELECT     
   EDRL_GUID
  ,EDR_LAYOUT_ID
  ,EDRL_STATE
  ,EDRL_TYPE_ID
  ,EDRL_TITLE
  ,EDRL_HTTP_REFERENCE
  ,EDRL_INSTRUCTIONS
  ,EDRL_DESCRIPTION
  ,EDRL_UPDATE_REASON
  ,EDRL_RECORD_CATEGORY
  ,EDRL_VERSION
  ,EDRL_JAVA_SCRIPT
  ,EDRL_HAS_CS_SCRIPT
  ,EDRL_LANGUAGE
  ,EDRL_RECORD_PREFIX
  ,EDRL_READ_ACCESS_ROLES
  ,EDRL_EDIT_ACCESS_ROLES
  ,EDRL_PARENT_TYPE
  ,EDRL_PARENT_ACCESS
  ,EDRL_PARENT_ENTITIES
  ,EDRL_LINK_CONTENT_SETTING
  ,EDRL_DEFAULT_PAGE_LAYOUT
  ,EDRL_DISPLAY_ENTITIES
  ,EDRL_DISPLAY_AUTHOR_DETAILS
  ,EDRL_HEADER_FORMAT
  ,EDRL_FOOTER_FORMAT
  ,[EDRL_FILTER_FIELD_0]
  ,[EDRL_FILTER_FIELD_1]
  ,[EDRL_FILTER_FIELD_2]
  ,[EDRL_FILTER_FIELD_3]
  ,[EDRL_FILTER_FIELD_4]
  ,[EDRL_FIELD_DISPLAY_FORMAT]
  ,EDRL_UPDATED_BY_USER_ID
  ,EDRL_UPDATED_BY
  ,EDRL_UPDATED_DATE
  ,EDRL_DELETED
FROM        ED_RECORD_LAYOUTS
WHERE     (EDRL_DELETED = 0) AND (EDRL_STATE = 'Form_Issued')

GO



/****** Object:  StoredProcedure dbo.USR_RECORD_LAYOUT_ADD    Script Date: 01/04/2021 10:32:51 ******/
CREATE PROCEDURE dbo.USR_RECORD_LAYOUT_ADD
 @GUID UniqueIdentifier,
 @LAYOUT_ID NVarChar(20),
 @STATE  NVarChar(20),
 @TITLE NVarChar(80),
 @HTTP_REFERENCE NVarChar(250),
 @INSTRUCTIONS NText,
 @DESCRIPTION NText,   
 @UPDATE_REASON  NVarChar(50),
 @RECORD_CATEGORY NVarChar(100),
 @TYPE_ID VarChar(25),
 @VERSION Float,
 @JAVA_SCRIPT NText,   
 @HAS_CS_SCRIPT Bit,
 @LANGUAGE VarChar(5),
 @RECORD_PREFIX VarChar(5),
 @READ_ACCESS_ROLES NVarChar(250),
 @EDIT_ACCESS_ROLES NVarChar(250),
 @PARENT_TYPE NVARCHAR(50),
 @DEFAULT_PAGE_LAYOUT NVARCHAR(50),
 @PARENT_ENTITIES NVARCHAR(250),
 @PARENT_ACCESS NVARCHAR(50),
 @LINK_CONTENT_SETTING NVARCHAR(50),
 @DISPLAY_ENTITIES BIT,
 @DISPLAY_AUTHOR_DETAILS BIT,
 @HEADER_FORMAT NVARCHAR(30),
 @FOOTER_FORMAT NVARCHAR(30),
 @FIELD_DISPLAY_FORMAT NVarChar(50),
 @FILTER_FIELD_0  NVARCHAR(30),
 @FILTER_FIELD_1  NVARCHAR(30),
 @FILTER_FIELD_2  NVARCHAR(30),
 @FILTER_FIELD_3  NVARCHAR(30),
 @FILTER_FIELD_4  NVARCHAR(30),
 @UPDATED_BY_USER_ID NVarChar(100),
 @UPDATED_BY NVarChar(30),
 @UPDATED_DATE DateTime
AS

Insert Into ED_RECORD_LAYOUTS 
  (EDRL_GUID
  ,EDR_LAYOUT_ID
  ,EDRL_STATE
  ,EDRL_TYPE_ID
  ,EDRL_TITLE
  ,EDRL_HTTP_REFERENCE
  ,EDRL_INSTRUCTIONS
  ,EDRL_DESCRIPTION
  ,EDRL_UPDATE_REASON
  ,EDRL_RECORD_CATEGORY
  ,EDRL_VERSION
  ,EDRL_JAVA_SCRIPT
  ,EDRL_HAS_CS_SCRIPT
  ,EDRL_LANGUAGE
  ,EDRL_RECORD_PREFIX
  ,EDRL_READ_ACCESS_ROLES
  ,EDRL_EDIT_ACCESS_ROLES
  ,EDRL_PARENT_TYPE
  ,EDRL_PARENT_ACCESS
  ,EDRL_PARENT_ENTITIES
  ,EDRL_LINK_CONTENT_SETTING
  ,EDRL_DEFAULT_PAGE_LAYOUT
  ,EDRL_DISPLAY_ENTITIES
  ,EDRL_DISPLAY_AUTHOR_DETAILS
  ,EDRL_HEADER_FORMAT
  ,EDRL_FOOTER_FORMAT
  ,EDRL_FILTER_FIELD_0
  ,EDRL_FILTER_FIELD_1
  ,EDRL_FILTER_FIELD_2
  ,EDRL_FILTER_FIELD_3
  ,EDRL_FILTER_FIELD_4
  ,EDRL_FIELD_DISPLAY_FORMAT
  ,EDRL_UPDATED_BY_USER_ID
  ,EDRL_UPDATED_BY
  ,EDRL_UPDATED_DATE
  ,EDRL_DELETED  ) 
values 
 ( @GUID
  ,@LAYOUT_ID
  ,@STATE
  ,@TYPE_ID
  ,@TITLE
  ,@HTTP_REFERENCE
  ,@INSTRUCTIONS
  ,@DESCRIPTION
  ,@UPDATE_REASON
  ,@RECORD_CATEGORY
  ,@VERSION
  ,@JAVA_SCRIPT
  ,@HAS_CS_SCRIPT
  ,@LANGUAGE
  ,@RECORD_PREFIX
  ,@READ_ACCESS_ROLES
  ,@EDIT_ACCESS_ROLES
  ,@PARENT_TYPE
  ,@PARENT_ACCESS
  ,@PARENT_ENTITIES
  ,@LINK_CONTENT_SETTING
  ,@DEFAULT_PAGE_LAYOUT
  ,@DISPLAY_ENTITIES
  ,@DISPLAY_AUTHOR_DETAILS
  ,@HEADER_FORMAT
  ,@FOOTER_FORMAT
  ,@FILTER_FIELD_0
  ,@FILTER_FIELD_1
  ,@FILTER_FIELD_2
  ,@FILTER_FIELD_3
  ,@FILTER_FIELD_4
  ,@FIELD_DISPLAY_FORMAT
  ,@UPDATED_BY_USER_ID
  ,@UPDATED_BY
  ,@UPDATED_DATE,0  );

GO

/****** Object:  StoredProcedure dbo.USR_RECORD_LAYOUT_COPY    Script Date: 01/04/2021 10:32:51 ******/

CREATE PROCEDURE dbo.USR_RECORD_LAYOUT_COPY
 @GUID UniqueIdentifier,
 @VERSION Float,
 @Copy bit,
 @UPDATED_BY_USER_ID NVarChar(100),
 @UPDATED_BY NVarChar(30),
 @UPDATED_DATE DateTime
AS

DECLARE @NewGuid uniqueidentifier
DECLARE @LAYOUT_ID varchar(20)
DECLARE @Title varchar(100)
DECLARE @CopyId varchar(4)

SET @NewGuid =  newid()

/***** Revise the form  ****/
SELECT @LAYOUT_ID = @LAYOUT_ID,  @Title = EDRL_TITLE 
FROM ED_RECORD_LAYOUTS 
WHERE (EDRL_GUID = @GUID);

IF ( @Copy = 1 )
BEGIN

SET @Version ='0.00'
SET @Title = SUBSTRING ( '(COPY) ' + @Title, 1, 100 )
SET @LAYOUT_ID = SUBSTRING ( 'CPY_' + @LAYOUT_ID , 1 , 20 ) 

END

/** COPY THE FORM CVALUES IN TO THE FORM OBJECT.**/
Insert Into ED_RECORD_LAYOUTS 
  ( EDRL_GUID
  ,EDR_LAYOUT_ID
  ,EDRL_STATE
  ,EDRL_TITLE
  ,EDRL_TYPE_ID
  ,EDRL_HTTP_REFERENCE
  ,EDRL_INSTRUCTIONS
  ,EDRL_DESCRIPTION
  ,EDRL_UPDATE_REASON
  ,EDRL_RECORD_CATEGORY
  ,EDRL_VERSION
  ,EDRL_JAVA_SCRIPT
  ,EDRL_HAS_CS_SCRIPT
  ,EDRL_LANGUAGE
  ,EDRL_RECORD_PREFIX
  ,EDRL_READ_ACCESS_ROLES
  ,EDRL_EDIT_ACCESS_ROLES
  ,EDRL_PARENT_TYPE
  ,EDRL_PARENT_ACCESS
  ,EDRL_PARENT_ENTITIES
  ,EDRL_LINK_CONTENT_SETTING
  ,EDRL_DEFAULT_PAGE_LAYOUT
  ,EDRL_DISPLAY_ENTITIES
  ,EDRL_DISPLAY_AUTHOR_DETAILS
  ,EDRL_HEADER_FORMAT
  ,EDRL_FOOTER_FORMAT
  ,EDRL_FILTER_FIELD_0
  ,EDRL_FILTER_FIELD_1
  ,EDRL_FILTER_FIELD_2
  ,EDRL_FILTER_FIELD_3
  ,EDRL_FILTER_FIELD_4
  ,EDRL_FIELD_DISPLAY_FORMAT
  ,EDRL_UPDATED_BY_USER_ID
  ,EDRL_UPDATED_BY
  ,EDRL_UPDATED_DATE
  , EDRL_DELETED  ) 
SELECT 
   @NewGuid
  ,@LAYOUT_ID 
  ,'Form_Draft'
  ,@Title 
  ,EDRL_TYPE_ID
  ,EDRL_HTTP_REFERENCE
  ,EDRL_INSTRUCTIONS
  ,EDRL_DESCRIPTION
  ,EDRL_UPDATE_REASON
  ,EDRL_RECORD_CATEGORY
  ,EDRL_VERSION
  ,EDRL_JAVA_SCRIPT
  ,EDRL_HAS_CS_SCRIPT
  ,EDRL_LANGUAGE
  ,EDRL_RECORD_PREFIX
  ,EDRL_READ_ACCESS_ROLES
  ,EDRL_EDIT_ACCESS_ROLES
  ,EDRL_PARENT_TYPE
  ,EDRL_PARENT_ACCESS
  ,EDRL_PARENT_ENTITIES
  ,EDRL_LINK_CONTENT_SETTING
  ,EDRL_DEFAULT_PAGE_LAYOUT
  ,EDRL_DISPLAY_ENTITIES
  ,EDRL_DISPLAY_AUTHOR_DETAILS
  ,EDRL_HEADER_FORMAT
  ,EDRL_FOOTER_FORMAT
  ,EDRL_FILTER_FIELD_0
  ,EDRL_FILTER_FIELD_1
  ,EDRL_FILTER_FIELD_2
  ,EDRL_FILTER_FIELD_3
  ,EDRL_FILTER_FIELD_4
  ,EDRL_FIELD_DISPLAY_FORMAT
  ,@UPDATED_BY_USER_ID
  ,@UPDATED_BY
  ,@UPDATED_DATE
  , 0
FROM ED_RECORD_LAYOUTS 
WHERE (EDRL_GUID = @GUID);


/** COPY THE FORM SECTION IN TO THE FORM OBJECT.**/
INSERT INTO ED_RECORD_SECTIONS 
 ( EDRL_GUID 
  ,EDRLS_NUMBER 
  ,EDRLS_ORDER 
  ,EDRLS_NAME 
  ,EDRLS_INSTRUCTIONS 
  ,EDRLS_FIELD_NAME 
  ,EDRLS_FIELD_VALUE 
  ,EDRLS_ON_MATCH_VISIBLE 
  ,EDRLS_VISIBLE 
  ,EDRLS_DEFAULT_DISPLAY_ROLES 
  ,EDRLS_DEFAULT_EDIT_ROLES ) 
SELECT 
   @NewGuid
  ,EDRLS_NUMBER 
  ,EDRLS_ORDER 
  ,EDRLS_NAME 
  ,EDRLS_INSTRUCTIONS 
  ,EDRLS_FIELD_NAME 
  ,EDRLS_FIELD_VALUE 
  ,EDRLS_ON_MATCH_VISIBLE 
  ,EDRLS_VISIBLE 
  ,EDRLS_DEFAULT_DISPLAY_ROLES 
  ,EDRLS_DEFAULT_EDIT_ROLES
FROM ED_RECORD_SECTIONS 
WHERE (EDRL_GUID = @GUID);

/** COPY THE FORM FIELDS INTO THE NEW FORM OBJECT.**/
INSERT INTO ED_RECORD_FIELDS 
 ( EDRL_GUID 
  , EDRLF_GUID 
  , EDR_LAYOUT_ID 
  , EDRLF_FIELD_ID 
  , EDRLF_TYPE_ID 
  , EDRLF_ORDER 
  , EDRLF_TITLE 
  , EDRLF_INSTRUCTIONS 
  , EDRLF_HTTP_REFERENCE 
  , EDRLF_SECTION_ID 
  , EDRLF_OPTIONS 
  , EDRLF_SUMMARY_FIELD 
  , EDRLF_MANDATORY 
  , EDRLF_AI_DATA_POINT 
  , EDRLF_HIDDEN 
  , EDRLF_ANALYTICS_DATA_POINT
  , EDRLF_DEFAULT_VALUE 
  , EDRLF_UNIT 
  , EDRLF_UNIT_SCALING 
  , EDRLF_VALIDATION_LOWER_LIMIT 
  , EDRLF_VALIDATION_UPPER_LIMIT 
  , EDRLF_ALERT_LOWER_LIMIT 
  , EDRLF_ALERT_UPPER_LIMIT 
  , EDRLF_NORMAL_LOWER_LIMIT 
  , EDRLF_NORMAL_UPPER_LIMIT 
  , EDRLF_FIELD_CATEGORY 
  , EDRLF_ANALOGUE_LEGEND_START 
  , EDRLF_ANALOGUE_LEGEND_FINISH 
  , EDRLF_JAVA_SCRIPT 
  , EDRLF_TABLE 
  , EDRLF_INITIAL_OPTION_LIST 
  , EDRLF_FIELD_LAYOUT 
  , EDRLF_INITIAL_VERSION 
  , EDRLF_DELETED  ) 
SELECT 
   @NewGuid
  , NEWID()
  , EDR_LAYOUT_ID
  , EDRLF_FIELD_ID 
  , EDRLF_TYPE_ID 
  , EDRLF_ORDER 
  , EDRLF_TITLE 
  , EDRLF_INSTRUCTIONS 
  , EDRLF_HTTP_REFERENCE 
  , EDRLF_SECTION_ID 
  , EDRLF_OPTIONS 
  , EDRLF_SUMMARY_FIELD 
  , EDRLF_MANDATORY 
  , EDRLF_AI_DATA_POINT
  , EDRLF_ANALYTICS_DATA_POINT
  , EDRLF_HIDDEN 
  , EDRLF_DEFAULT_VALUE 
  , EDRLF_UNIT 
  , EDRLF_UNIT_SCALING 
  , EDRLF_VALIDATION_LOWER_LIMIT 
  , EDRLF_VALIDATION_UPPER_LIMIT 
  , EDRLF_ALERT_LOWER_LIMIT 
  , EDRLF_ALERT_UPPER_LIMIT 
  , EDRLF_NORMAL_LOWER_LIMIT 
  , EDRLF_NORMAL_UPPER_LIMIT 
  , EDRLF_FIELD_CATEGORY 
  , EDRLF_ANALOGUE_LEGEND_START 
  , EDRLF_ANALOGUE_LEGEND_FINISH 
  , EDRLF_JAVA_SCRIPT 
  , EDRLF_TABLE 
  , EDRLF_INITIAL_OPTION_LIST 
  , EDRLF_FIELD_LAYOUT 
  , EDRLF_INITIAL_VERSION 
  ,0
  FROM   ED_RECORD_FIELDS 
WHERE (EDRL_GUID = @GUID) and (EDRLF_DELETED = 0);

GO

/****** Object:  StoredProcedure dbo.USR_RECORD_LAYOUT_DELETE    Script Date: 01/04/2021 10:32:51 ******/

CREATE   PROCEDURE dbo.USR_RECORD_LAYOUT_DELETE 
 @GUID UniqueIdentifier ,
 @UPDATED_BY_USER_ID NVarChar (100),
 @UPDATED_BY  NVarChar(30),
 @UPDATED_DATE DateTime
AS

UPDATE ED_RECORD_LAYOUTS 
SET 
  EDRL_UPDATED_BY_USER_ID = @UPDATED_BY_USER_ID,  
  EDRL_UPDATED_BY = @UPDATED_BY,  
  EDRL_UPDATED_DATE = @UPDATED_DATE, 
  EDRL_DELETED = -1 
WHERE 	EDRL_DELETED = 0 
	AND EDRL_GUID = @GUID ;

GO

/****** Object:  StoredProcedure dbo.USR_RECORD_LAYOUT_UPDATE    Script Date: 01/04/2021 10:32:51 ******/

CREATE PROCEDURE dbo.USR_RECORD_LAYOUT_UPDATE
 @GUID UniqueIdentifier,
 @LAYOUT_ID NVarChar(20),
 @STATE  NVarChar(20),
 @TITLE NVarChar(80),
 @HTTP_REFERENCE NVarChar(250),
 @INSTRUCTIONS NText,
 @DESCRIPTION NText,   
 @UPDATE_REASON  NVarChar(50),
 @RECORD_CATEGORY NVarChar(100),
 @RECORD_PREFIX NVarChar(5),
 @TYPE_ID VarChar(25),
 @VERSION Float,
 @JAVA_SCRIPT NText,   
 @HAS_CS_SCRIPT Bit,
 @LANGUAGE VarChar(5),
 @READ_ACCESS_ROLES NVarChar(250),
 @EDIT_ACCESS_ROLES NVarChar(250),
 @PARENT_TYPE NVARCHAR(50),
 @DEFAULT_PAGE_LAYOUT NVARCHAR(50),
 @PARENT_ENTITIES NVARCHAR(250),
 @PARENT_ACCESS NVARCHAR(50),
 @LINK_CONTENT_SETTING NVARCHAR(50),
 @DISPLAY_ENTITIES BIT,
 @DISPLAY_AUTHOR_DETAILS BIT,
 @HEADER_FORMAT NVARCHAR(30),
 @FOOTER_FORMAT NVARCHAR(30),
 @FIELD_DISPLAY_FORMAT NVarChar(50),
 @FILTER_FIELD_0  NVARCHAR(30),
 @FILTER_FIELD_1  NVARCHAR(30),
 @FILTER_FIELD_2  NVARCHAR(30),
 @FILTER_FIELD_3  NVARCHAR(30),
 @FILTER_FIELD_4  NVARCHAR(30),
 @UPDATED_BY_USER_ID NVarChar(100),
 @UPDATED_BY NVarChar(30),
 @UPDATED_DATE DateTime
AS
UPDATE ED_RECORD_LAYOUTS 
SET 
  EDR_LAYOUT_ID = @LAYOUT_ID ,
  EDRL_STATE = @STATE  ,
  EDRL_TITLE = @TITLE ,
  EDRL_HTTP_REFERENCE = @HTTP_REFERENCE ,
  EDRL_INSTRUCTIONS = @INSTRUCTIONS ,
  EDRL_DESCRIPTION = @DESCRIPTION ,
  EDRL_UPDATE_REASON = @UPDATE_REASON  ,
  EDRL_RECORD_CATEGORY = @RECORD_CATEGORY,
  EDRL_RECORD_PREFIX = @RECORD_PREFIX,
  EDRL_TYPE_ID = @TYPE_ID ,
  EDRL_VERSION = @VERSION,
  EDRL_JAVA_SCRIPT = @JAVA_SCRIPT, 
  EDRL_HAS_CS_SCRIPT = @HAS_CS_SCRIPT,
  EDRL_LANGUAGE = @LANGUAGE,
  EDRL_READ_ACCESS_ROLES = @READ_ACCESS_ROLES,
  EDRL_EDIT_ACCESS_ROLES = @EDIT_ACCESS_ROLES,
  EDRL_PARENT_TYPE = @PARENT_TYPE,
  EDRL_PARENT_ENTITIES = @PARENT_ENTITIES,
  EDRL_PARENT_ACCESS = @PARENT_ACCESS,
  EDRL_DEFAULT_PAGE_LAYOUT = @DEFAULT_PAGE_LAYOUT,
  EDRL_LINK_CONTENT_SETTING = @LINK_CONTENT_SETTING,
  EDRL_DISPLAY_ENTITIES = @DISPLAY_ENTITIES,
  EDRL_DISPLAY_AUTHOR_DETAILS = @DISPLAY_AUTHOR_DETAILS,
  EDRL_HEADER_FORMAT = @HEADER_FORMAT,
  EDRL_FOOTER_FORMAT = @FOOTER_FORMAT,
  EDRL_FILTER_FIELD_0 = @FILTER_FIELD_0,
  EDRL_FILTER_FIELD_1 = @FILTER_FIELD_1,
  EDRL_FILTER_FIELD_2 = @FILTER_FIELD_2,
  EDRL_FILTER_FIELD_3 = @FILTER_FIELD_3,
  EDRL_FILTER_FIELD_4 = @FILTER_FIELD_4,
  EDRL_FIELD_DISPLAY_FORMAT = @FIELD_DISPLAY_FORMAT,
  EDRL_UPDATED_BY_USER_ID = @UPDATED_BY_USER_ID,
  EDRL_UPDATED_BY = @UPDATED_BY,
  EDRL_UPDATED_DATE =@UPDATED_DATE
WHERE (EDRL_GUID = @GUID) ;


GO

/****** Object:  StoredProcedure dbo.USR_RECORD_LAYOUT_UPDATE    Script Date: 01/04/2021 10:32:52 ******/

CREATE PROCEDURE dbo.USR_RECORD_LAYOUT_WITHDRAWN
 @LAYOUT_ID NVarChar(20),
 @UPDATED_BY_USER_ID NVarChar (100),
 @UPDATED_BY  NVarChar(30),
 @UPDATED_DATE DateTime
AS
UPDATE ED_RECORD_LAYOUTS 
SET 
  EDRL_STATE = 'Withdrawn', 
  EDRL_UPDATED_BY_USER_ID = @UPDATED_BY_USER_ID,
  EDRL_UPDATED_BY = @UPDATED_BY,
  EDRL_UPDATED_DATE =@UPDATED_DATE
WHERE (EDR_LAYOUT_ID = @LAYOUT_ID)
  AND (EDRL_STATE = 'Form_Issued' ) ;



GO




PRINT N'FINISH: 003_R1.0_ED_RECORD_LAYOUTS_CREATE_STORED_PROCEDURES.'; 
GO
