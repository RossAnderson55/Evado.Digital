/****** File: 000_R1.0_CREATE_EVADO_DIGITAL_TABLES.sql ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

DECLARE @RC int

EXECUTE @RC = [dbo].[usr_DB_Add_Database_Update] 
   000, 'R1.0','ED_RECORD_LAYOUTS, ED_RECORDS, ED_ENTITIES, ED_ENTITY_LAYOUTS ', 
   'CREATE EVADO DIGITAL RECORD TABLES.'

GO

PRINT N'START: 000_R1.0_CREATE_EVADO_DIGITAL_TABLES.'; 
GO

/****** Object:  Table [dbo].[ED_RECORD_LAYOUTS]    Script Date: 12/24/2020 10:45:30 ******/

IF NOT EXISTS( SELECT 1 FROM sys.columns 
          WHERE Name = N'CU_GUID'
          AND Object_ID = Object_ID(N'ED_RECORD_LAYOUTS' ))
BEGIN

CREATE TABLE [dbo].[ED_RECORD_LAYOUTS](
	[CU_GUID] [uniqueidentifier] NOT NULL,
	[EDRL_GUID] [uniqueidentifier] NOT NULL,
	[EDR_LAYOUT_ID] [nvarchar](20) NULL,
	[EDRL_STATE] [varchar](50) NULL,
	[EDRL_TYPE_ID] [varchar](30) NOT NULL,
	[EDRL_TITLE] [nvarchar](100) NULL,
	[EDRL_HTTP_REFERENCE] [nvarchar](100)  NULL,
	[EDRL_INSTRUCTIONS] [ntext] NULL,
	[EDRL_DESCRIPTION] [ntext] NULL,
	[EDRL_UPDATE_REASON] [varchar](50) NULL,
	[EDRL_RECORD_CATEGORY] [nvarchar](100) NULL,
	[EDRL_VERSION] [float] NULL,
	[EDRL_JAVA_SCRIPT] [nvarchar](250) NULL,
	[EDRL_HAS_CS_SCRIPT] [bit] NULL,
	[EDRL_LANGUAGE] [nvarchar](10) NULL,
	[EDRL_RECORD_PREFIX] [nvarchar](10) NULL,
	[EDRL_CDASH_METADATA] [varchar](250) NULL,
	[EDRL_UPDATED_BY_USER_ID] [nvarchar](100) NULL,
	[EDRL_UPDATED_BY] [nvarchar](100) NULL,
	[EDRL_UPDATED_DATE] [datetime] NULL,
	[EDRL_DELETED] [bit] NULL,
 CONSTRAINT [PK_RECORD_LAYOUTS] PRIMARY KEY CLUSTERED 
(
	[EDRL_GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

END
GO

IF NOT EXISTS( SELECT 1 FROM sys.columns 
          WHERE Name = N'EDRL_READ_ACCESS_ROLES'
          AND Object_ID = Object_ID(N'ED_RECORD_LAYOUTS' ))
BEGIN
ALTER TABLE ED_RECORD_LAYOUTS
 ADD 
 [EDRL_READ_ACCESS_ROLES] NVARCHAR(250) NULL,
 [EDRL_EDIT_ACCESS_ROLES] NVARCHAR(250) NULL
 END
GO

IF NOT EXISTS( SELECT 1 FROM sys.columns 
          WHERE Name = N'EDRL_RELATED_ENTITIES'
          AND Object_ID = Object_ID(N'ED_RECORD_LAYOUTS' ))
BEGIN
ALTER TABLE ED_RECORD_LAYOUTS
 ADD 
 [EDRL_RELATED_ENTITIES] NVARCHAR(250) NULL,
 [EDRL_DEFAULT_PAGE_LAYOUT] NVARCHAR(50) NULL,
 [EDRL_DISPLAY_RECORD_SUMMARY] BIT NULL,
 [EDRL_DISPLAY_ENTITIES] BIT NULL,
 [EDRL_DISPLAY_AUTHOR_DETAILS] BIT NULL
 END
GO

IF NOT EXISTS( SELECT 1 FROM sys.columns 
          WHERE Name = N'EDRL_LINK_CONTENT_SETTING'
          AND Object_ID = Object_ID(N'ED_RECORD_LAYOUTS' ))
BEGIN
ALTER TABLE ED_RECORD_LAYOUTS
 ADD 
 [EDRL_LINK_CONTENT_SETTING]  NVARCHAR(50) NULL;
 
ALTER TABLE ED_RECORD_LAYOUTS
 DROP COLUMN
 [EDRL_DISPLAY_RECORD_SUMMARY];
 END
GO

IF NOT EXISTS( SELECT 1 FROM sys.columns 
          WHERE Name = N'EDRL_RECORD_PRFIX'
          AND Object_ID = Object_ID(N'ED_RECORD_LAYOUTS' ))
BEGIN
ALTER TABLE ED_RECORD_LAYOUTS
 ADD 
 [EDRL_RECORD_PRFIX]  NVARCHAR(50) NULL;
 END
GO

/****** Object:  Table [dbo].[ED_RECORD_LAYOUT_SECTIONS]    Script Date: 01/04/2021 11:20:06 ******/

CREATE TABLE [dbo].[ED_RECORD_LAYOUT_SECTIONS](
	[EDRLS_GUID] [uniqueidentifier] NOT NULL,
	[EDRL_GUID] [uniqueidentifier] NOT NULL,
	[EDRLS_NUMBER] [int] NULL,
	[EDRLS_ORDER] [int] NULL,
	[EDRLS_NAME] [nvarchar](30) NULL,
	[EDRLS_INSTRUCTIONS] [nvarchar](max) NULL,
	[EDRLS_FIELD_NAME] [nvarchar](20) NULL,
	[EDRLS_FIELD_VALUE] [nvarchar](250) NULL,
	[EDRLS_ON_MATCH_VISIBLE] [bit] NULL,
	[EDRLS_VISIBLE] [bit] NULL,
	[EDRLS_DEFAULT_DISPLAY_ROLES] [nvarchar](250) NULL,
	[EDRLS_DEFAULT_EDIT_ROLES] [nvarchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ED_RECORD_LAYOUT_FIELDS]    Script Date: 12/24/2020 10:45:30 ******/

CREATE TABLE [dbo].[ED_RECORD_LAYOUT_FIELDS](
	[EDRL_GUID] [uniqueidentifier] NOT NULL,
	[EDRLF_GUID] [uniqueidentifier] NOT NULL,
	[EDR_LAYOUT_ID] [nvarchar](20) NULL,
	[FIELD_ID] [nvarchar](20) NOT NULL,
	[EDRLF_TYPE_ID] [varchar](30) NULL,
	[EDRLF_ORDER] [smallint] NULL,
	[EDRLF_TITLE] [nvarchar](150) NULL,
	[EDRLF_INSTRUCTIONS] [ntext] NULL,
	[EDRLF_HTTP_REFERENCE] [nvarchar](150) NULL,
	[EDRLF_SECTION_ID] [smallint] NULL,
	[EDRLF_OPTIONS] [nvarchar](256) NULL,
	[EDRLF_SUMMARY_FIELD] [bit] NULL,
	[EDRLF_MANDATORY] [bit] NULL,
	[EDRLF_AI_DATA_POINT] [bit] NULL,
	[EDRLF_ANALYTICS_DATA_POINT] [bit] NULL,
	[EDRLF_HIDDEN] [bit] NULL,
  [EDRLF_EX_SELECTION_LIST_ID] nvarchar(250)NULL,
  [EDRLF_EX_SELECTION_LIST_CATEGORY] nvarchar(250) NULL,
	[EDRLF_DEFAULT_VALUE] [nvarchar](100) NULL,
	[EDRLF_UNIT] [nvarchar](15) NULL,
	[EDRLF_UNIT_SCALING] [nvarchar](15) NULL,
	[EDRLF_VALIDATION_LOWER_LIMIT] [float] NULL,
	[EDRLF_VALIDATION_UPPER_LIMIT] [float] NULL,
	[EDRLF_ALERT_LOWER_LIMIT] [float] NULL,
	[EDRLF_ALERT_UPPER_LIMIT] [float] NULL,
	[EDRLF_NORMAL_LOWER_LIMIT] [int] NULL,
	[EDRLF_NORMAL_UPPER_LIMIT] [int] NULL,
	[EDRLF_FIELD_CATEGORY] [varchar](50) NULL,
	[EDRLF_ANALOGUE_LEGEND_START] [varchar](30) NULL,
	[EDRLF_ANALOGUE_LEGEND_FINISH] [varchar](30) NULL,
	[EDRLF_JAVA_SCRIPT] [ntext] NULL,
	[EDRLF_TABLE] [ntext] NULL,
	[EDRLF_INITIAL_OPTION_LIST] [nvarchar](250) NULL,
	[EDRLF_INITIAL_VERSION] [smallint] NULL,
	[EDRLF_DELETED] [bit] NULL,
 CONSTRAINT [PK_LAYOUT_FIELD] PRIMARY KEY CLUSTERED 
(
	[EDRLF_GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


IF NOT EXISTS( SELECT 1 FROM sys.columns 
          WHERE Name = N'EDRLF_FIELD_LAYOUT'
          AND Object_ID = Object_ID(N'ED_RECORD_LAYOUT_FIELDS' ))
BEGIN
ALTER TABLE ED_RECORD_LAYOUT_FIELDS
 ADD 
 [EDRLF_FIELD_LAYOUT] NVARCHAR(50) NULL
 END
GO

/****** Object:  Table [dbo].[ED_RECORD_VALUES]    Script Date: 12/24/2020 10:45:30 ******/
CREATE TABLE [dbo].[ED_RECORD_VALUES](
	[EDR_GUID] [uniqueidentifier] NOT NULL,
	[EDRLF_GUID] [uniqueidentifier] NOT NULL,
	[EDRV_GUID] [uniqueidentifier] NOT NULL,
	[EDRV_COLUMN_ID] [nvarchar](10) NULL,
	[EDRV_ROW] [smallint] NULL,
	[EDRV_STRING] [nvarchar](100) NULL,
	[EDRV_NUMERIC] [float] NULL,
	[EDRV_DATE] [datetime] NULL,
	[EDRV_TEXT] [ntext] NULL,
 CONSTRAINT [PK_RECORD_VALUES] PRIMARY KEY CLUSTERED 
(
	[EDRV_GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ED_RECORDS]    Script Date: 01/08/2021 10:47:30 ******/
/*
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ED_RECORDS]') AND type in (N'U'))
DROP TABLE [dbo].[ED_RECORDS]
GO
*/
/****** Object:  Table [dbo].[ED_RECORDS]    Script Date: 12/24/2020 10:45:30 ******/

CREATE TABLE [dbo].[ED_RECORDS](
	[EDR_GUID] [uniqueidentifier] NOT NULL,
	[EDRL_GUID] [uniqueidentifier] NOT NULL,
	[EDR_STATE] [varchar](30) NOT NULL,
	[RECORD_ID] [nvarchar](20) NULL,
	[EDR_SOURCE_ID] [varchar](20) NULL,
	[EDR_COLLECTION_EVENT_ID] [varchar](20) NULL,
	[EDR_RECORD_DATE] [datetime] NULL,
	[EDR_COMMENTS] [ntext] NULL,
	[EDR_SIGN_OFFS] [ntext] NULL,
	[EDR_SERIAL_ID] [int] NULL,
	[EDR_BOOKED_OUT_USER_ID] nvarchar(100) NULL,
	[EDR_BOOKED_OUT] [nvarchar](100) NULL,
	[EDR_UPDATED_BY_USER_ID] [nvarchar](100) NULL,
	[EDR_UPDATED_BY] [nvarchar](100) NULL,
	[EDR_UPDATED_DATE] [datetime] NULL,
	[EDR_DELETED] [bit] NULL,
 CONSTRAINT [PK_RECORDS] PRIMARY KEY CLUSTERED 
(
	[EDR_GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

IF NOT EXISTS( SELECT 1 FROM sys.columns 
          WHERE Name = N'EDR_ORG_ID'
          AND Object_ID = Object_ID(N'ED_RECORDS' ))
BEGIN
ALTER TABLE ED_RECORDS
 ADD 
 [EDR_ORG_ID] NVARCHAR(10) NULL,
 [EDR_USER_ID] NVARCHAR(100) NULL
 END
GO

/****** Object:  Table [dbo].[ED_ENTITY_LAYOUTS]    Script Date: 12/24/2020 10:45:30 ******/

CREATE TABLE [dbo].[ED_ENTITY_LAYOUTS](
	[CU_GUID] [uniqueidentifier] NOT NULL,
	[EDEL_GUID] [uniqueidentifier] NOT NULL,
	[EDE_LAYOUT_ID] [nvarchar](20) NULL,
	[EDEL_STATE] [varchar](50) NULL,
	[EDEL_TITLE] [nvarchar](100) NULL,
	[EDEL_HTTP_REFERENCE] [nvarchar](100)  NULL,
	[EDEL_INSTRUCTIONS] [ntext] NULL,
	[EDEL_DESCRIPTION] [ntext] NULL,
	[EDEL_UPDATE_REASON] [varchar](50) NULL,
	[EDEL_RECORD_CATEGORY] [nvarchar](100) NULL,
	[EDEL_TYPE_ID] [varchar](30) NOT NULL,
	[EDEL_VERSION] [float] NULL,
	[EDEL_JAVA_SCRIPT] [nvarchar](250) NULL,
	[EDEL_HAS_CS_SCRIPT] [bit] NULL,
	[EDEL_LANGUAGE] [nvarchar](10) NULL,
	[EDEL_RECORD_PREFIX] [nvarchar](10) NULL,
	[EDEL_FILTER_FIELD_ID_0] [nvarchar](20) NULL,
	[EDEL_FILTER_FIELD_ID_1] [nvarchar](20) NULL,
	[EDEL_FILTER_FIELD_ID_2] [nvarchar](20) NULL,
	[EDEL_FILTER_FIELD_ID_3] [nvarchar](20) NULL,
	[EDEL_FILTER_FIELD_ID_4] [nvarchar](20) NULL,
	[EDEL_CDASH_METADATA] [varchar](250) NULL,
	[EDEL_UPDATED_BY_USER_ID] [nvarchar](100) NULL,
	[EDEL_UPDATED_BY] [nvarchar](100) NULL,
	[EDEL_UPDATED_DATE] [datetime] NULL,
	[EDEL_DELETED] [bit] NULL,
 CONSTRAINT [PK_ENTITY_LAYOUTS] PRIMARY KEY CLUSTERED 
(
	[EDEL_GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]

) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


IF NOT EXISTS( SELECT 1 FROM sys.columns 
          WHERE Name = N'EDEL_READ_ACCESS'
          AND Object_ID = Object_ID(N'ED_ENTITY_LAYOUTS' ))
BEGIN
ALTER TABLE ED_ENTITY_LAYOUTS
 ADD 
 [EDEL_READ_ACCESS] NVARCHAR(250) NULL,
 [EDEL_EDIT_ACCESS] NVARCHAR(250) NULL
 END
GO

IF NOT EXISTS( SELECT 1 FROM sys.columns 
          WHERE Name = N'EDEL_RELATED_ENTITIES'
          AND Object_ID = Object_ID(N'ED_ENTITY_LAYOUTS' ))
BEGIN
ALTER TABLE ED_ENTITY_LAYOUTS
 ADD 
 [EDEL_RELATED_ENTITIES] NVARCHAR(250) NULL,
 [EDEL_DEFAULT_PAGE_LAYOUT] NVARCHAR(50) NULL,
 [EDEL_DISPLAY_RECORD_SUMMARY] BIT NULL,
 [EDEL_DISPLAY_ENTITIES] BIT NULL,
 [EDEL_DISPLAY_AUTHOR_DETAILS] BIT NULL
 END
GO

/****** Object:  Table [dbo].[ED_ENTITY_LAYOUT_SECTIONS]    Script Date: 01/04/2021 11:20:06 ******/

CREATE TABLE [dbo].[ED_ENTITY_LAYOUT_SECTIONS](
	[EDELS_GUID] [uniqueidentifier] NOT NULL,
	[EDEL_GUID] [uniqueidentifier] NOT NULL,
	[EDELS_NUMBER] [int] NULL,
	[EDELS_ORDER] [int] NULL,
	[EDELS_NAME] [nvarchar](30) NULL,
	[EDELS_INSTRUCTIONS] [nvarchar](max) NULL,
	[EDELS_FIELD_NAME] [nvarchar](20) NULL,
	[EDELS_FIELD_VALUE] [nvarchar](250) NULL,
	[EDELS_ON_MATCH_VISIBLE] [bit] NULL,
	[EDELS_VISIBLE] [bit] NULL,
	[EDELS_DEFAULT_DISPLAY_ROLES] [nvarchar](250) NULL,
	[EDELS_DEFAULT_EDIT_ROLES] [nvarchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ED_ENTITY_LAYOUT_FIELDS]    Script Date: 12/24/2020 10:45:30 ******/

CREATE TABLE [dbo].[ED_ENTITY_LAYOUT_FIELDS](
	[EDEL_GUID] [uniqueidentifier] NOT NULL,
	[EDELF_GUID] [uniqueidentifier] NOT NULL,
	[EDE_LAYOUT_ID] [nvarchar](20) NULL,
	[EDELF_FIELD_ID] [nvarchar](20) NOT NULL,
	[EDELF_TYPE_ID] [varchar](30) NULL,
	[EDELF_ORDER] [smallint] NULL,
	[EDELF_TITLE] [nvarchar](150) NULL,
	[EDELF_INSTRUCTIONS] [ntext] NULL,
	[EDELF_HTTP_REFERENCE] [nvarchar](150) NULL,
	[EDELF_SECTION_ID] [smallint] NULL,
	[EDELF_OPTIONS] [nvarchar](256) NULL,
	[EDELF_SUMMARY_FIELD] [bit] NULL,
	[EDELF_MANDATORY] [bit] NULL,
	[EDELF_AI_DATA_POINT] [bit] NULL,
	[EDELF_ANALYTICS_DATA_POINT] [bit] NULL,
	[EDELF_HIDDEN] [bit] NULL,
  [EDELF_EX_SELECTION_LIST_ID] nvarchar(250)NULL,
  [EDELF_EX_SELECTION_LIST_CATEGORY] nvarchar(250) NULL,
	[EDELF_DEFAULT_VALUE] [nvarchar](100) NULL,
	[EDELF_UNIT] [nvarchar](15) NULL,
	[EDELF_UNIT_SCALING] [nvarchar](15) NULL,
	[EDELF_VALIDATION_LOWER_LIMIT] [float] NULL,
	[EDELF_VALIDATION_UPPER_LIMIT] [float] NULL,
	[EDELF_ALERT_LOWER_LIMIT] [float] NULL,
	[EDELF_ALERT_UPPER_LIMIT] [float] NULL,
	[EDELF_NORMAL_LOWER_LIMIT] [int] NULL,
	[EDELF_NORMAL_UPPER_LIMIT] [int] NULL,
	[EDELF_FIELD_CATEGORY] [varchar](50) NULL,
	[EDELF_ANALOGUE_LEGEND_START] [varchar](30) NULL,
	[EDELF_ANALOGUE_LEGEND_FINISH] [varchar](30) NULL,
	[EDELF_JAVA_SCRIPT] [ntext] NULL,
	[EDELF_TABLE] [ntext] NULL,
	[EDELF_INITIAL_OPTION_LIST] [nvarchar](250) NULL,
	[EDELF_INITIAL_VERSION] [smallint] NULL,
	[EDELF_DELETED] [bit] NULL
  ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


IF NOT EXISTS( SELECT 1 FROM sys.columns 
          WHERE Name = N'EDELF_FIELD_LAYOUT'
          AND Object_ID = Object_ID(N'ED_ENTITY_LAYOUT_FIELDS' ))
BEGIN
ALTER TABLE ED_ENTITY_LAYOUT_FIELDS
 ADD 
 [EDELF_FIELD_LAYOUT] NVARCHAR(50) NULL
 END
GO

/****** Object:  Table [dbo].[ED_ENTITIES]    Script Date: 01/08/2021 10:47:30 ******/
/* IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ED_ENTITIES]') AND type in (N'U'))
DROP TABLE [dbo].[ED_ENTITIES]
GO*/

/****** Object:  Table [dbo].[ED_ENTITIES]    Script Date: 12/24/2020 10:45:30 ******/
CREATE TABLE [dbo].[ED_ENTITIES](
	[EDE_GUID] [uniqueidentifier] NOT NULL,
	[EDEL_GUID] [uniqueidentifier] NOT NULL,
	[EDE_STATE] [varchar](30) NOT NULL,
	[ENTITY_ID] [nvarchar](20) NULL,
	[EDE_SOURCE_ID] [varchar](20) NULL,
	[EDE_COLLECTION_EVENT_ID] [varchar](20) NULL,
	[EDE_RECORD_DATE] [datetime] NULL,
	[EDE_COMMENTS] [ntext] NULL,
	[EDE_SERIAL_ID] [int] NULL,
	[EDE_SIGN_OFFS] [ntext] NULL,
	[EDE_BOOKED_OUT_USER_ID] [nvarchar](100) NULL,
	[EDE_BOOKED_OUT] [nvarchar](100) NULL,
	[EDE_UPDATED_BY_USER_ID] [nvarchar](100) NULL,
	[EDE_UPDATED_BY] [nvarchar](100) NULL,
	[EDE_UPDATED_DATE] [datetime] NULL,
	[EDE_DELETED] [bit] NULL,
 CONSTRAINT [PK_ENTITIES] PRIMARY KEY CLUSTERED 
(
	[EDE_GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


IF NOT EXISTS( SELECT 1 FROM sys.columns 
          WHERE Name = N'EDE_ORG_ID'
          AND Object_ID = Object_ID(N'ED_ENTITIES' ))
BEGIN
ALTER TABLE ED_ENTITIES
 ADD 
 [EDE_ORG_ID] NVARCHAR(10) NULL,
 [EDE_USER_ID] NVARCHAR(100) NULL
 END
GO

/****** Object:  Table [dbo].[ED_ENTITY_VALUES]    Script Date: 12/24/2020 10:45:30 ******/

CREATE TABLE [dbo].[ED_ENTITY_VALUES](
	[EDE_GUID] [uniqueidentifier] NOT NULL,
	[EDELF_GUID] [uniqueidentifier] NOT NULL,
	[EDEV_GUID] [uniqueidentifier] NOT NULL,
	[EDEV_COLUMN] [smallint] NULL,
	[EDEV_ROW] [smallint] NULL,
	[EDEV_STRING] [nvarchar](100) NULL,
	[EDEV_NUMERIC] [float] NULL,
	[EDEV_DATE] [datetime] NULL,
	[EDEV_TEXT] [ntext] NULL,
 CONSTRAINT [PK_ENTITY_VALUES] PRIMARY KEY CLUSTERED 
(
	[EDEV_GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ED_ENTITIES]    Script Date: 12/24/2020 10:45:30 ******/
CREATE TABLE [dbo].[ED_ENTITY_RECORD_JOIN](
	[EDRL_GUID] [uniqueidentifier] NOT NULL,
	[EDR_GUID] [uniqueidentifier] NOT NULL,
	[EDE_GUID] [uniqueidentifier] NULL,
	[EDE_LAYOUT_ID] [nvarchar](20) NULL,
	[EDEL_TITLE] [nvarchar](100) NULL
	)
	
GO

PRINT N'FINISH: 000_R1.0_CREATE_EVADO_DIGITAL_TABLES.'; 
GO
