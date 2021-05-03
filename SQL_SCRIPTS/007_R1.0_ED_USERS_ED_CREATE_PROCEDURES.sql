/****** File: 007_R1.0_ED_USERS_PROFILE_CREATE_PROCEDURES.sql ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

DECLARE @RC int

EXECUTE @RC = dbo.usr_DB_Add_Database_Update 
   007, 'R1.0','ED_USER_PROFILE', 
   'CREATE PROCEDURES.'
GO

PRINT N'START: 007_R1.0_ED_USERS_PROFILE_CREATE_PROCEDURES..'; 
GO

IF NOT EXISTS( SELECT 1 FROM sys.columns 
          WHERE Name = N'UP_MIDDLE_NAME'
          AND Object_ID = Object_ID(N'ED_USER_PROFILES' ))
BEGIN
ALTER TABLE ED_USER_PROFILES
 ADD 
  [UP_MIDDLE_NAME]  NVARCHAR(50) NULL
END
GO


/****** Object:  StoredProcedure [dbo].[usr_UserProfile_add]    Script Date: 02/08/2021 12:36:05 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usr_UserProfile_add]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usr_UserProfile_add]
GO

/****** Object:  StoredProcedure [dbo].[usr_UserProfile_delete]    Script Date: 02/08/2021 12:36:05 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usr_UserProfile_delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usr_UserProfile_delete]
GO

/****** Object:  StoredProcedure [dbo].[usr_UserProfile_update]    Script Date: 02/08/2021 12:36:05 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usr_UserProfile_update]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usr_UserProfile_update]
GO

/****** Object:  StoredProcedure [dbo].[USR_USER_PROFILE_ADD]    Script Date: 02/08/2021 12:36:05 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USR_USER_PROFILE_ADD]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USR_USER_PROFILE_ADD]
GO

/****** Object:  StoredProcedure [dbo].[USR_USER_PROFILE_DELETE]    Script Date: 02/08/2021 12:36:05 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USR_USER_PROFILE_DELETE]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USR_USER_PROFILE_DELETE]
GO

/****** Object:  StoredProcedure [dbo].[USR_USER_PROFILE_UPDATE]    Script Date: 02/08/2021 12:36:05 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USR_USER_PROFILE_UPDATE]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USR_USER_PROFILE_UPDATE]
GO

/****** Object:  View [dbo].[ED_USER_PROFILE_VIEW]    Script Date: 05/03/2021 11:59:17 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ED_USER_PROFILE_VIEW]'))
DROP VIEW [dbo].[ED_USER_PROFILE_VIEW]
GO
/****** Object:  View [dbo].[ED_USER_PROFILE_VIEW]    Script Date: 05/03/2021 11:59:18 ******/
CREATE VIEW [dbo].[ED_USER_PROFILE_VIEW]
AS
SELECT 
  dbo.ED_USER_PROFILES.UP_Guid, 
  dbo.ED_USER_PROFILES.ORG_ID, 
  dbo.ED_USER_PROFILES.USER_ID, 
  dbo.ED_USER_PROFILES.UP_ACTIVE_DIRECTORY_NAME, 
  dbo.ED_USER_PROFILES.UP_PREFIX, 
  dbo.ED_USER_PROFILES.UP_GIVEN_NAME, 
  dbo.ED_USER_PROFILES.UP_MIDDLE_NAME, 
  dbo.ED_USER_PROFILES.UP_FAMILY_NAME, 
  dbo.ED_USER_PROFILES.UP_SUFFIX, 
  dbo.ED_USER_PROFILES.UP_ADDRESS_1, 
  dbo.ED_USER_PROFILES.UP_ADDRESS_2, 
  dbo.ED_USER_PROFILES.UP_ADDRESS_CITY, 
  dbo.ED_USER_PROFILES.UP_ADDRESS_POST_CODE, 
  dbo.ED_USER_PROFILES.UP_ADDRESS_STATE, 
  dbo.ED_USER_PROFILES.UP_ADDRESS_COUNTRY, 
  dbo.ED_USER_PROFILES.UP_TELEPHONE, 
  dbo.ED_USER_PROFILES.UP_MOBILE_PHONE, 
  dbo.ED_USER_PROFILES.UP_COMMON_NAME, 
  dbo.ED_USER_PROFILES.UP_EMAIL_ADDRESS, 
  dbo.ED_USER_PROFILES.UP_IMAGE_FILENAME, 
  dbo.ED_USER_PROFILES.UP_Title, 
  dbo.ED_USER_PROFILES.UP_ROLES, 
  dbo.ED_USER_PROFILES.UP_TYPE, 
  dbo.ED_USER_PROFILES.UP_CATEGORY, 
  dbo.ED_USER_PROFILES.UP_UPDATED_BY_USER_ID, 
  dbo.ED_USER_PROFILES.UP_UPDATED_BY, 
  dbo.ED_USER_PROFILES.UP_UPDATED_DATE, 
  dbo.ED_USER_PROFILES.UP_EXPIRY_DATE, 
  dbo.ED_USER_PROFILES.UP_DELETED,
  dbo.ED_ORGANISATIONS.O_Name, 
  dbo.ED_ORGANISATIONS.O_ADDRESS_1, 
  dbo.ED_ORGANISATIONS.O_ADDRESS_2, 
  dbo.ED_ORGANISATIONS.O_ADDRESS_CITY, 
  dbo.ED_ORGANISATIONS.O_ADDRESS_POST_CODE, 
  dbo.ED_ORGANISATIONS.O_ADDRESS_STATE, 
  dbo.ED_ORGANISATIONS.O_Country, 
  dbo.ED_ORGANISATIONS.O_ORG_TYPE
FROM         dbo.ED_USER_PROFILES INNER JOIN
                      dbo.ED_ORGANISATIONS ON dbo.ED_USER_PROFILES.ORG_ID = dbo.ED_ORGANISATIONS.ORG_ID
WHERE     (dbo.ED_USER_PROFILES.UP_DELETED = 0)

GO



/****** Object:  StoredProcedure [dbo].[USR_USER_PROFILE_ADD]    Script Date: 02/08/2021 12:36:05 ******/

CREATE             PROCEDURE [dbo].[USR_USER_PROFILE_ADD]
 @Guid Uniqueidentifier,
 @UserId nvarchar(100),
 @OrgId nvarchar(10),
 @ActiveDirectName nvarchar(100),
 @CommonName nvarchar(100),
 @Title nvarchar(100),
 @PREFIX nvarchar(10),
 @GIVEN_NAME nvarchar(50),
 @MIDDLE_NAME nvarchar(50),
 @FAMILY_NAME nvarchar(50),
 @SUFFIX nvarchar(50),
 @ADDRESS_1 nvarchar(50),
 @ADDRESS_2 nvarchar(50),
 @ADDRESS_CITY nvarchar(50),
 @ADDRESS_POST_CODE nvarchar(10),
 @ADDRESS_STATE nvarchar(50),
 @ADDRESS_COUNTRY nvarchar(50),
 @TELEPHONE nvarchar(20),
 @MOBILE_PHONE nvarchar(20),
 @EmailAddress nvarchar(100),
 @ROLEID nvarchar(100),
 @CATEGORY nvarchar(50),
 @TYPE nvarchar(50),
 @IMAGE_FILENAME nvarchar(100),
 @EXPIRY_DATE datetime,
 @UpdatedByUserId nvarchar(100),
 @UpdatedBy nvarchar(100),
 @UpdateDate datetime
AS
Insert Into ED_USER_PROFILES 
  ( UP_GUID 
  , USER_ID 
  , ORG_ID 
  , UP_ACTIVE_DIRECTORY_NAME 
  , UP_COMMON_NAME 
  , UP_TITLE 
  , UP_PREFIX 
  , UP_GIVEN_NAME 
  , UP_MIDDLE_NAME
  , UP_FAMILY_NAME 
  , UP_SUFFIX 
  , UP_ADDRESS_1 
  , UP_ADDRESS_2 
  , UP_ADDRESS_CITY 
  , UP_ADDRESS_POST_CODE 
  , UP_ADDRESS_STATE 
  , UP_ADDRESS_COUNTRY 
  , UP_MOBILE_PHONE 
  , UP_TELEPHONE 
  , UP_EMAIL_ADDRESS 
  , UP_ROLES 
  , UP_CATEGORY 
  , UP_TYPE 
  , UP_IMAGE_FILENAME 
  , UP_EXPIRY_DATE 
  , UP_UPDATED_BY_USER_ID 
  , UP_UPDATED_BY 
  , UP_UPDATED_DATE 
  , UP_DELETED  ) 
values 	
 (@Guid,
 @UserId,
 @OrgId,
 @ActiveDirectName,
 @CommonName,
 @Title,
 @PREFIX,
 @GIVEN_NAME,
 @MIDDLE_NAME,
 @FAMILY_NAME,
 @SUFFIX,
 @ADDRESS_1,
 @ADDRESS_2,
 @ADDRESS_CITY,
 @ADDRESS_POST_CODE,
 @ADDRESS_STATE,
 @ADDRESS_COUNTRY,
 @TELEPHONE,
 @MOBILE_PHONE,
 @EmailAddress,
 @ROLEID,
 @CATEGORY,
 @TYPE,
 @IMAGE_FILENAME,
 @EXPIRY_DATE,
 @UpdatedByUserId,
 @UpdatedBy,
 @UpdateDate, 0 );



GO

/****** Object:  StoredProcedure [dbo].[USR_USER_PROFILE_DELETE]    Script Date: 02/08/2021 12:36:05 ******/

CREATE        PROCEDURE [dbo].[USR_USER_PROFILE_DELETE]
 @Guid Uniqueidentifier,
 @UpdatedByUserId nvarchar(100),
 @UpdatedBy nvarchar(100),
 @UpdateDate datetime
AS

UPDATE 	ED_USER_PROFILES 
SET	UP_UPDATED_BY_USER_ID =  @UpdatedByUserId,  
  UP_UPDATED_BY =  @UpdatedBy,  
	UP_UPDATED_DATE = @UpdateDate , 
	UP_DELETED = -1 
WHERE	UP_DELETED = 0 AND UP_Guid = @Guid;




GO

/****** Object:  StoredProcedure [dbo].[USR_USER_PROFILE_UPDATE]    Script Date: 02/08/2021 12:36:05 ******/

CREATE      PROCEDURE [dbo].[USR_USER_PROFILE_UPDATE]
 @Guid Uniqueidentifier,
 @UserId nvarchar(100),
 @OrgId nvarchar(10),
 @ActiveDirectName nvarchar(100),
 @CommonName nvarchar(100),
 @Title nvarchar(100),
 @PREFIX nvarchar(10),
 @GIVEN_NAME nvarchar(50),
 @MIDDLE_NAME nvarchar(50),
 @FAMILY_NAME nvarchar(50),
 @SUFFIX nvarchar(50),
 @ADDRESS_1 nvarchar(50),
 @ADDRESS_2 nvarchar(50),
 @ADDRESS_CITY nvarchar(50),
 @ADDRESS_POST_CODE nvarchar(10),
 @ADDRESS_STATE nvarchar(50),
 @ADDRESS_COUNTRY nvarchar(50),
 @TELEPHONE nvarchar(20),
 @MOBILE_PHONE nvarchar(20),
 @EmailAddress nvarchar(100),
 @ROLEID nvarchar(100),
 @CATEGORY nvarchar(50),
 @TYPE nvarchar(50),
 @IMAGE_FILENAME nvarchar(100),
 @EXPIRY_DATE datetime,
 @UpdatedByUserId nvarchar(100),
 @UpdatedBy nvarchar(100),
 @UpdateDate datetime
AS

UPDATE	ED_USER_PROFILES 
SET
  Org_Id = @OrgId, 
  User_Id = @UserId, 
  UP_ACTIVE_DIRECTORY_NAME = @ActiveDirectName, 
  UP_PREFIX = @PREFIX, 
  UP_GIVEN_NAME = 	@GIVEN_NAME, 
  UP_MIDDLE_NAME = @MIDDLE_NAME,
  UP_FAMILY_NAME = 	@FAMILY_NAME, 
  UP_SUFFIX = 	@SUFFIX, 
  UP_ADDRESS_1 = @ADDRESS_1,
  UP_ADDRESS_2 = @ADDRESS_2,
  UP_ADDRESS_CITY = @ADDRESS_CITY,
  UP_ADDRESS_POST_CODE = @ADDRESS_POST_CODE,
  UP_ADDRESS_STATE = @ADDRESS_STATE,
  UP_ADDRESS_COUNTRY = @ADDRESS_COUNTRY, 
  UP_TELEPHONE = @TELEPHONE,
  UP_MOBILE_PHONE = @MOBILE_PHONE, 
  UP_COMMON_NAME = @CommonName, 
  UP_EMAIL_ADDRESS = @EmailAddress, 
  UP_ROLES = @ROLEID, 
  UP_CATEGORY = @CATEGORY,
  UP_TYPE = @TYPE,
  UP_Title = @Title, 
  UP_IMAGE_FILENAME= @IMAGE_FILENAME,
  UP_EXPIRY_DATE = @EXPIRY_DATE,
 	UP_UPDATED_BY_USER_ID = @UpdatedByUserId,  
 	UP_UPDATED_BY = @UpdatedBy,  
	UP_UPDATED_DATE = @UpdateDate,
  UP_DELETED = 0 
WHERE UP_Guid = @Guid;



GO


PRINT N'FINISH: 007_R1.0_ED_USERS_PROFILE_CREATE_PROCEDURES..'; 
GO
