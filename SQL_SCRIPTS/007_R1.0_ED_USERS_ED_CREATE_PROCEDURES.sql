/****** File: 007_R1.0_ED_USERS_ED_CREATE_PROCEDURES..sql ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

DECLARE @RC int

EXECUTE @RC = dbo.usr_DB_Add_Database_Update 
   007, 'R1.0','ED_USER7', 
   'CREATE PROCEDURES.'
GO

PRINT N'START: 007_R1.0_ED_USERS_ED_CREATE_PROCEDURES..'; 
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
 @RoleId nvarchar(100),
 @TYPE nvarchar(50),
 @EXPIRY_DATE datetime,
 @UpdatedByUserId nvarchar(100),
 @UpdatedBy nvarchar(100),
 @UpdateDate datetime
AS
Insert Into ED_USER_PROFILES 
  ([UP_GUID]
  ,[USER_ID]
  ,[ORG_ID]
  ,[UP_ACTIVE_DIRECTORY_NAME]
  ,[UP_COMMON_NAME]
  ,[UP_TITLE]
  ,[UP_PREFIX]
  ,[UP_GIVEN_NAME]
  ,[UP_FAMILY_NAME]
  ,[UP_SUFFIX]
  ,[UP_ADDRESS_1]
  ,[UP_ADDRESS_2]
  ,[UP_ADDRESS_CITY]
  ,[UP_ADDRESS_POST_CODE]
  ,[UP_ADDRESS_STATE]
  ,[UP_ADDRESS_COUNTRY]
  ,[UP_MOBILE_PHONE]
  ,[UP_TELEPHONE]
  ,[UP_EMAIL_ADDRESS]
  ,[UP_ROLES]
  ,[UP_TYPE]
  ,[UP_EXPIRY_DATE]
  ,[UP_UPDATED_BY_USER_ID]
  ,[UP_UPDATED_BY]
  ,[UP_UPDATED_DATE]
  ,[UP_DELETED] ) 
values 	
 (@Guid,
 @UserId,
 @OrgId,
 @ActiveDirectName,
 @CommonName,
 @Title,
 @PREFIX,
 @GIVEN_NAME,
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
 @RoleId,
 @TYPE,
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
 @RoleId nvarchar(100),
 @TYPE nvarchar(20),
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
  UP_ROLES = @RoleId, 
  UP_TYPE = @TYPE,
  UP_Title = @Title, 
  UP_EXPIRY_DATE = @EXPIRY_DATE,
 	UP_UPDATED_BY_USER_ID = @UpdatedByUserId,  
 	UP_UPDATED_BY = @UpdatedBy,  
	UP_UPDATED_DATE = @UpdateDate,
  UP_DELETED = 0 
WHERE UP_Guid = @Guid;



GO


PRINT N'FINISH: 007_R1.0_ED_USERS_ED_CREATE_PROCEDURES..'; 
GO
