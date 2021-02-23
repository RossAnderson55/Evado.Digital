/***************************************************************************************
 * <copyright file="dal\EvEvFormBinaryFiles.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2020 EVADO HOLDING PTY. LTD..  All rights reserved.
 *     
 *      The use and distribution terms for this software are contained in the file
 *      named license.txt, which can be found in the root of this distribution.
 *      By using this software in any fashion, you are agreeing to be bound by the
 *      terms of this license.
 *     
 *      You must not remove this notice, or any other, from this software.
 *     
 * </copyright>
 *
 ****************************************************************************************/


using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text;

//References to Evado specific libraries

using Evado.Model;
using Evado.Model.Digital;


namespace Evado.Dal.Digital
{

  /// <summary>
  /// This class is handles the data access layer for the binary file metadata data object.
  /// </summary>
  public class EvBinaryFiles : EvDalBase
  {
    #region class initialisation
    // ==================================================================================
    /// <summary>
    /// This is the class initialisation method.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EvBinaryFiles ( )
    {
      this.ClassNameSpace = "Evado.Dal.Digital.EvBinaryFiles.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EvBinaryFiles ( EvClassParameters Settings )
    {
      this.ClassParameters = Settings;
      this.ClassNameSpace = "Evado.Dal.Digital.EvBinaryFiles.";
    }
    #endregion

    #region Initialise class objects and constants

    /// <summary>
    /// This constant defines a sql query for viewing binary files. 
    /// </summary>
    private const string SQL_VIEW_QUERY = "Select * FROM EV_BINARY_FILE_META_DATA ";

    /// <summary>
    /// This constant defines a binary glabal unique identifier of binary files.
    /// </summary>
    private const string DB_GUID = "BFMD_GUID";

    /// <summary>
    /// This constant defines a binary glabal unique identifier of binary files.
    /// </summary>
    private const string DB_FILE_GUID = "BFMD_FILE_GUID";

    /// <summary>
    /// This constant defines a trial global unique identifier of binary files.
    /// </summary>
    private const string DB_PROJECT_GUID = "BFMD_PROJECT_GUID";

    /// <summary>
    /// This constant defines a subject global unique identifier of binary files.
    /// </summary>
    private const string DB_GROUP_GUID = "BFMD_GROUP_GUID";

    /// <summary>
    /// This constant defines a filing index global unique identifier of binary files.
    /// </summary>
    private const string DB_SUB_GROUP_GUID = "BFMD_SUB_GROUP_GUID";

    /// <summary>
    /// This constant defines a language identifier of binary files.
    /// </summary>
    private const string DB_LANGUAGE = "BFMD_LANGUAGE";

    /// <summary>
    /// This constant defines a trial identifier of binary files.
    /// </summary>
    private const string DB_TRIAL_ID = "PROJECT_ID";

    /// <summary>
    /// This constant defines a filing index of binary files.
    /// </summary>
    private const string DB_GROUP_ID = "BFMD_GROUP_ID";

    /// <summary>
    /// This constant defines a filing index of binary files.
    /// </summary>
    private const string DB_SUB_GROUP_ID = "BFMD_SUB_GROUP_ID";

    /// <summary>
    /// This constant defines an object identifier of binary files.
    /// </summary>
    private const string DB_FILE_ID = "BFMD_FILE_ID";

    /// <summary>
    /// This constant defines a title of binary files.
    /// </summary>
    private const string DB_FILE_TITLE = "BFMD_TITLE";

    /// <summary>
    /// This constant defines a title of binary files.
    /// </summary>
    private const string DB_FILE_COMMENT = "BFMD_COMMENT";

    /// <summary>
    /// This constant defines a file name of binary files.
    /// </summary>
    private const string DB_FILE_NAME = "BFMD_FILE_NAME";

    /// <summary>
    /// This constant defines a mime type of binary files.
    /// </summary>
    private const string DB_MIME_TYPE = "BFMD_MIME_TYPE";

    /// <summary>
    /// This constant defines a mime type of binary files.
    /// </summary>
    private const string DB_FILE_STATUS = "BFMD_STATUS";

    /// <summary>
    /// This constant defines a uploaded date of binary files.
    /// </summary>
    private const string DB_UPLOAD_DATE = "BFMD_UPLOAD_DATE";

    /// <summary>
    /// This constant defines a current date of binary files.
    /// </summary>
    private const string DB_RELEASE_DATE = "BFMD_RELEASE_DATE";

    /// <summary>
    /// This constant defines a superseded date of binary files.
    /// </summary>
    private const string DB_SUPERSEDED_DATE = "BFMD_SUPERSEDED_DATE";

    /// <summary>
    /// This constant defines a file exists of binary files.
    /// </summary>
    private const string DB_FILE_EXISTS = "BFMD_EXISTS";

    /// <summary>
    /// This constant defines a file exists of binary files.
    /// </summary>
    private const string DB_FILE_ENCRYPTED = "BFMD_ENCRYPTED";

    /// <summary>
    /// This constant defines version of binary files.
    /// </summary>
    private const string DB_VERSION = "BFMD_VERSION";

    /// <summary>
    /// This constant defines a user identifier of those who updates binary files.
    /// </summary>
    private const string DB_UPDATED_BY_USER_ID = "BFMD_UPDATED_BY_USER_ID";

    /// <summary>
    /// This constant defines a user who updates binary files.
    /// </summary>
    private const string DB_UPDATED_BY = "BFMD_UPDATED_BY";

    /// <summary>
    /// This constant defines an updated date of binary files.
    /// </summary>
    private const string DB_UPDATED_DATE = "BFMD_UPDATED_DATE";

    #endregion

    #region The SQL Store Procedure constants

    /// <summary>
    /// This constant define a storeprocedure for adding item.
    /// </summary>
    private const string _storedProcedureAddItem = "usr_BinaryFileMetaData_add";

    /// <summary>
    /// This constant define a storeprocedure for updating item.
    /// </summary>
    private const string _storedProcedureUpdateItem = "usr_BinaryFileMetaData_update";

    /// <summary>
    /// This constant define a storeprocedure for deleting item.
    /// </summary>
    private const string _storedProcedureDeleteItem = "";
    #endregion

    #region The SQL query parameters.
    /// <summary>
    /// This constant defines a parameter for binary global unique identifier
    /// </summary>
    private const string PARM_GUID = "@GUID";
    /// <summary>
    /// This constant defines a parameter for binary global unique identifier
    /// </summary>
    private const string PARM_FILE_GUID = "@FILE_GUID";

    /// <summary>
    /// This constant defines a parameter for a trial global unique identifier
    /// </summary>
    private const string PARM_PROJECT_GUID = "@PROJECT_GUID";

    /// <summary>
    /// This constant defines a parameter for a subject global unique identifier
    /// </summary>
    private const string PARM_GROUP_GUID = "@GROUP_GUID";

    /// <summary>
    /// This constant defines a parameter for a filing index global unique identifier
    /// </summary>
    private const string PARM_SUB_GROUP_ID_GUID = "@SUB_GROUP_GUID";

    /// <summary>
    /// This constant defines a language identifier of binary files.
    /// </summary>
    private const string PARM_LANGUAGE = "@LANGUAGE";

    /// <summary>
    /// This constant defines a parameter for a trial identifier
    /// </summary>
    private const string PARM_TRIAL_ID = "@PROJECT_ID";

    /// <summary>
    /// This constant defines a parameter for a filing index
    /// </summary>
    private const string PARM_GROUP_ID = "@GROUP_ID";

    /// <summary>
    /// This constant defines a parameter for a filing index
    /// </summary>
    private const string PARM_SUB_GROUP_ID = "@SUB_GROUP_ID";

    /// <summary>6
    /// This constant defines a parameter for an object identifier
    /// </summary>
    private const string PARM_FILE_ID = "@FILE_ID";

    /// <summary>
    /// This constant defines a parameter for a title of binary file
    /// </summary>
    private const string PARM_FILE_TITLE = "@TITLE";

    /// <summary>
    /// This constant defines a parameter for a title of binary file
    /// </summary>
    private const string PARM_FILE_COMMENT = "@COMMENT";

    /// <summary>
    /// This constant defines a parameter for a file name of binary file
    /// </summary>
    private const string PARM_FILE_NAME = "@FILE_NAME";

    /// <summary>
    /// This constant defines a parameter for a mime type of binary file
    /// </summary>
    private const string PARM_MIME_TYPE = "@MIME_TYPE";
    /// <summary>
    /// This constant defines a mime type of binary files.
    /// </summary>
    private const string PARM_FILE_STATUS = "@STATUS";

    /// <summary>
    /// This constant defines a uploaded date of binary files.
    /// </summary>
    private const string PARM_UPLOAD_DATE = "@UPLOAD_DATE";

    /// <summary>
    /// This constant defines a current date of binary files.
    /// </summary>
    private const string PARM_RELEASE_DATE = "@RELEASE_DATE";

    /// <summary>
    /// This constant defines a superseded date of binary files.
    /// </summary>
    private const string PARM_SUPERSEDED_DATE = "@SUPERSEDED_DATE";

    /// <summary>
    /// This constant defines a parameter for a version of binary file
    /// </summary>
    private const string PARM_VERSION = "@VERSION";

    /// <summary>
    /// This constant defines a parameter for a version of binary file
    /// </summary>
    private const string PARM_FILE_ENCRYPTED = "@ENCRYPTED";

    /// <summary>
    /// This constant defines a parameter for a file exists of binary file
    /// </summary>
    private const string PARM_FILE_EXISTS = "@EXISTS";

    /// <summary>
    /// This constant defines a parameter for a user identifier of those who updates binary file
    /// </summary>
    private const string PARM_UPDATED_BY_USER_ID = "@UPDATED_BY_USER_ID";

    /// <summary>
    /// This constant defines a parameter for a user who update a binary file
    /// </summary>
    private const string PARM_UPDATED_BY = "@UPDATED_BY";

    /// <summary>
    /// This constant defines a parameter for an updated date of binary file
    /// </summary>
    private const string PARM_UPDATED_DATE = "@UPDATED_DATE";
    #endregion

    #region SQL Paramter methods

    // ==================================================================================
    /// <summary>
    ///  This method creates an array of SqlParameter objects, to pass update values to 
    ///  database update stored procedures. 
    /// </summary>
    /// <returns>SqlParameter: an array of sql query parameters</returns>
    /// <remarks>
    /// This method consists of the following step:
    /// 
    /// 1. Create an array of sql query parameters. 
    /// 
    /// 2. Return an array of sql query parameters.
    /// </remarks>
    // ----------------------------------------------------------------------------------
    private static SqlParameter [ ] getItemsParameters ( )
    {
      SqlParameter [ ] parms = new SqlParameter [ ] 
      {
        new SqlParameter( EvBinaryFiles.PARM_GUID, SqlDbType.UniqueIdentifier ),
        new SqlParameter( EvBinaryFiles.PARM_FILE_GUID, SqlDbType.UniqueIdentifier ),
        new SqlParameter( EvBinaryFiles.PARM_PROJECT_GUID, SqlDbType.UniqueIdentifier),
        new SqlParameter( EvBinaryFiles.PARM_GROUP_GUID, SqlDbType.UniqueIdentifier),
        new SqlParameter( EvBinaryFiles.PARM_SUB_GROUP_ID_GUID, SqlDbType.UniqueIdentifier),

        new SqlParameter( EvBinaryFiles.PARM_LANGUAGE, SqlDbType.NVarChar, 10),
        new SqlParameter( EvBinaryFiles.PARM_TRIAL_ID, SqlDbType.NVarChar, 10),
        new SqlParameter( EvBinaryFiles.PARM_GROUP_ID, SqlDbType.NVarChar, 50),
        new SqlParameter( EvBinaryFiles.PARM_SUB_GROUP_ID, SqlDbType.NVarChar, 50),
        new SqlParameter( EvBinaryFiles.PARM_FILE_ID, SqlDbType.NVarChar, 50),

        new SqlParameter( EvBinaryFiles.PARM_FILE_TITLE, SqlDbType.NVarChar, 100),
        new SqlParameter( EvBinaryFiles.PARM_FILE_COMMENT, SqlDbType.NVarChar, 250),
        new SqlParameter( EvBinaryFiles.PARM_FILE_NAME, SqlDbType.NVarChar, 100),
        new SqlParameter( EvBinaryFiles.PARM_MIME_TYPE, SqlDbType.NVarChar, 20),
        new SqlParameter( EvBinaryFiles.PARM_FILE_STATUS, SqlDbType.NVarChar, 30),

        new SqlParameter( EvBinaryFiles.PARM_UPLOAD_DATE, SqlDbType.DateTime ),
        new SqlParameter( EvBinaryFiles.PARM_RELEASE_DATE, SqlDbType.DateTime ),
        new SqlParameter( EvBinaryFiles.PARM_SUPERSEDED_DATE, SqlDbType.DateTime ),
        new SqlParameter( EvBinaryFiles.PARM_VERSION, SqlDbType.SmallInt),
        new SqlParameter( EvBinaryFiles.PARM_FILE_EXISTS, SqlDbType.Bit),

        new SqlParameter( EvBinaryFiles.PARM_FILE_ENCRYPTED, SqlDbType.Bit),
        new SqlParameter( EvBinaryFiles.PARM_UPDATED_BY_USER_ID, SqlDbType.NVarChar, 100),
        new SqlParameter( EvBinaryFiles.PARM_UPDATED_BY, SqlDbType.NVarChar, 100),
        new SqlParameter( EvBinaryFiles.PARM_UPDATED_DATE, SqlDbType.DateTime )
      };

      return parms;
    }//END getItemsParameters class

    // ==================================================================================
    /// <summary>
    ///  This method fills the parameter array with values to be passed to the update 
    ///  stored procedures.
    /// </summary>
    /// <param name="cmdParms">SqlParameter: an array of Database parameters</param>
    /// <param name="BinaryFile">EvBinaryFileMetaData: Values to bind to parameters</param>
    /// <remarks>
    /// This method consists of the following step: 
    /// 
    /// 1. Update values from a binary file object to the array of sql query parameter. 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    private void SetParameters ( SqlParameter [ ] cmdParms, EvBinaryFileMetaData BinaryFile )
    {
      cmdParms [ 0 ].Value = BinaryFile.Guid;
      cmdParms [ 1 ].Value = BinaryFile.FileGuid;
      cmdParms [ 2 ].Value = BinaryFile.TrialGuid;
      cmdParms [ 3 ].Value = BinaryFile.GroupGuid;
      cmdParms [ 4 ].Value = BinaryFile.SubGroupGuid;

      cmdParms [ 5 ].Value = BinaryFile.Language;
      cmdParms [ 6 ].Value = BinaryFile.TrialId;
      cmdParms [ 7 ].Value = BinaryFile.GroupId;
      cmdParms [ 8 ].Value = BinaryFile.SubGroupId;
      cmdParms [ 9 ].Value = BinaryFile.FileId;

      cmdParms [ 10 ].Value = BinaryFile.Title;
      cmdParms [ 11 ].Value = BinaryFile.Comments;
      cmdParms [ 12 ].Value = BinaryFile.FileName;
      cmdParms [ 13 ].Value = BinaryFile.MimeType;
      cmdParms [ 14 ].Value = BinaryFile.Status;

      cmdParms [ 15 ].Value = BinaryFile.UploadDate;
      cmdParms [ 16 ].Value = BinaryFile.ReleaseDate;
      cmdParms [ 17 ].Value = BinaryFile.SupersededDate;
      cmdParms [ 18 ].Value = BinaryFile.Version;
      cmdParms [ 19 ].Value = BinaryFile.FileExists;

      cmdParms [ 20 ].Value = BinaryFile.FileEncrypted;
      cmdParms [ 21 ].Value = BinaryFile.UpdatedByUserId;
      cmdParms [ 22 ].Value = BinaryFile.UpdatedBy;
      cmdParms [ 23 ].Value = DateTime.Now;

    }//END SetParameters class.

    #endregion

    #region Data Reader methods

    // ==================================================================================
    /// <summary>
    /// This method reads the content of the data row object containing a query result
    /// into an ActivityRecord object.    
    /// </summary>
    /// <param name="Row">DataRow: a retrieving data row object</param>
    /// <returns>EvBinaryFileMetaData: a readed data row object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Extract the compatible data row values to the Binary file Meta data object
    /// 
    /// 2. Return the Binary file Meta data object
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public EvBinaryFileMetaData readDataRow ( DataRow Row )
    {
      // 
      // Initialise the instrument objects and variables.
      // 
      EvBinaryFileMetaData binaryFile = new EvBinaryFileMetaData ( );

      // 
      // Extract the data object values.
      // 
      binaryFile.Guid = EvSqlMethods.getGuid ( Row, EvBinaryFiles.DB_GUID );
      binaryFile.FileGuid = EvSqlMethods.getGuid ( Row, EvBinaryFiles.DB_FILE_GUID );
      binaryFile.TrialGuid = EvSqlMethods.getGuid ( Row, EvBinaryFiles.DB_PROJECT_GUID );
      binaryFile.GroupGuid = EvSqlMethods.getGuid ( Row, EvBinaryFiles.DB_GROUP_GUID );
      binaryFile.SubGroupGuid = EvSqlMethods.getGuid ( Row, EvBinaryFiles.DB_SUB_GROUP_GUID );

      binaryFile.Language = EvSqlMethods.getString ( Row, EvBinaryFiles.DB_LANGUAGE );
      binaryFile.TrialId = EvSqlMethods.getString ( Row, EvBinaryFiles.DB_TRIAL_ID );
      binaryFile.GroupId = EvSqlMethods.getString ( Row, EvBinaryFiles.DB_GROUP_ID );
      binaryFile.SubGroupId = EvSqlMethods.getString ( Row, EvBinaryFiles.DB_SUB_GROUP_ID );
      binaryFile.FileId = EvSqlMethods.getString ( Row, EvBinaryFiles.DB_FILE_ID );
      binaryFile.Title = EvSqlMethods.getString ( Row, EvBinaryFiles.DB_FILE_TITLE );

      binaryFile.Comments = EvSqlMethods.getString ( Row, EvBinaryFiles.DB_FILE_COMMENT );
      binaryFile.FileName = EvSqlMethods.getString ( Row, EvBinaryFiles.DB_FILE_NAME );
      binaryFile.MimeType = EvSqlMethods.getString ( Row, EvBinaryFiles.DB_MIME_TYPE );
      binaryFile.Status = EvSqlMethods.getString<EvBinaryFileMetaData.FileStatus> (
        Row, EvBinaryFiles.DB_FILE_STATUS );
      binaryFile.UploadDate = EvSqlMethods.getDateTime ( Row, EvBinaryFiles.DB_UPLOAD_DATE );

      binaryFile.ReleaseDate = EvSqlMethods.getDateTime ( Row, EvBinaryFiles.DB_RELEASE_DATE );
      binaryFile.SupersededDate = EvSqlMethods.getDateTime ( Row, EvBinaryFiles.DB_SUPERSEDED_DATE );
      binaryFile.FileExists = EvSqlMethods.getBool ( Row, EvBinaryFiles.DB_FILE_EXISTS );
      binaryFile.FileEncrypted = EvSqlMethods.getBool ( Row, EvBinaryFiles.DB_FILE_ENCRYPTED );
      binaryFile.Version = EvSqlMethods.getInteger ( Row, EvBinaryFiles.DB_VERSION );

      binaryFile.UpdatedByUserId = EvSqlMethods.getString ( Row, EvBinaryFiles.DB_UPDATED_BY_USER_ID );
      binaryFile.UpdatedBy = EvSqlMethods.getString ( Row, EvBinaryFiles.DB_UPDATED_BY );
      binaryFile.UpdatedByDate = EvSqlMethods.getDateTime ( Row, EvBinaryFiles.DB_UPDATED_DATE );

      // 
      // Return the object.
      // 
      return binaryFile;

    }//End readDataRow method.

    #endregion

    #region Class list and queries methods

    // ==================================================================================
    /// <summary>
    /// This method retrieves a list of project and site binary file metadata objects as a single list. 
    /// </summary>
    /// <param name="ProjectId">String: Project identifier (Mandatory)</param>
    /// <param name="OrgId">Stirng: FilingIndex Identifier (Mandatory)</param>
    /// <returns>List of EvBinaryFileMetaData: a list contains binary file data objects.</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Define the sql query parameters and the sql query string
    /// 
    /// 2. Execute the sql query string with parameters and store the values on datatable. 
    /// 
    /// 3. Iterate through the table and extract the row data to a binary file object. 
    /// 
    /// 4. Add the object values to the return binary file meta data list. 
    /// 
    /// 5. Return the list of binary file meta data
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public List<EvBinaryFileMetaData> getProjectFileList (
      String OrgId )
    {
      this.LogMethod ( "getProjectFileList method. " );

      //
      // Initialize a method debug status, a internal sql query string and a return list of binary file data objects.
      //
      StringBuilder  sqlQueryString = new StringBuilder();
      List<EvBinaryFileMetaData> fileList = new List<EvBinaryFileMetaData> ( );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( EvBinaryFiles.PARM_GROUP_ID, SqlDbType.VarChar, 50),
      };
      cmdParms [ 0 ].Value = OrgId;

      // 
      // Generate the SQL query string
      // 
      sqlQueryString.AppendLine( SQL_VIEW_QUERY );
      sqlQueryString.AppendLine ( "WHERE (" + EvBinaryFiles.DB_FILE_EXISTS + " = 1 ) " );
      sqlQueryString.AppendLine ( " AND (" + EvBinaryFiles.DB_FILE_STATUS + " <> '" + EvBinaryFileMetaData.FileStatus.Superseded + "' ) " );

      if ( OrgId == String.Empty )
      {
        sqlQueryString.AppendLine ( " AND (" + EvBinaryFiles.DB_GROUP_ID + " = '' ) " );
      }
      else
      {

        sqlQueryString.AppendLine ( " AND ( (" + EvBinaryFiles.DB_GROUP_ID + " = '' ) " );
        sqlQueryString.AppendLine ( " OR (" + EvBinaryFiles.DB_TRIAL_ID + "= " + EvBinaryFiles.PARM_TRIAL_ID 
          + " AND " + EvBinaryFiles.DB_GROUP_ID + " = " + EvBinaryFiles.PARM_GROUP_ID + ") ) " );
      }


      if ( OrgId == String.Empty )
      {
        sqlQueryString.AppendLine ( "ORDER BY "
          + EvBinaryFiles.DB_FILE_ID + ", "
          + EvBinaryFiles.DB_UPDATED_DATE + " ; " );
      }
      else
      {
        sqlQueryString.AppendLine ( "ORDER BY "
          + EvBinaryFiles.DB_GROUP_ID + ", "
          + EvBinaryFiles.DB_FILE_ID + ", "
          + EvBinaryFiles.DB_UPDATED_DATE + " ; " );
      }

      this.LogDebug ( sqlQueryString.ToString() );

      //
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString.ToString(), cmdParms ) )
      {
        // 
        // Iterate through the results extracting the role information.
        // 
        for ( int Count = 0; Count < table.Rows.Count; Count++ )
        {
          // 
          // Extract the table row
          // 
          DataRow row = table.Rows [ Count ];

          EvBinaryFileMetaData BinaryFile = this.readDataRow ( row );

          fileList.Add ( BinaryFile );

        } //END interation loop.

      }//END using method

      this.LogValue ( "fileList.Count: " + fileList.Count );
      // 
      // Return the list containing the User data object.
      // 
      this.LogMethodEnd ( "getProjectFileList" );
      return fileList;

    }//END getView method.

    // ==================================================================================
    /// <summary>
    /// This method retrieves a list of binary file metadata objects.
    /// </summary>
    /// <param name="GroupId">Stirng: FilingIndex Identifier (Mandatory)</param>
    /// <param name="SubGroupId">String: A sub group identifier</param>
    /// <returns>List of EvBinaryFileMetaData: a list contains binary file data objects.</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Define the sql query parameters and the sql query string
    /// 
    /// 2. Execute the sql query string with parameters and store the values on datatable. 
    /// 
    /// 3. Iterate through the table and extract the row data to a binary file object. 
    /// 
    /// 4. Add the object values to the return binary file meta data list. 
    /// 
    /// 5. Return the list of binary file meta data
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public List<EvBinaryFileMetaData> getBinaryFileList (
      String GroupId,
      String SubGroupId )
    {
      this.LogMethod ( "getBinaryFileList method. " );

      //
      // Initialize a method debug status, a internal sql query string and a return list of binary file data objects.
      //
      StringBuilder sqlQueryString = new StringBuilder ( );
      List<EvBinaryFileMetaData> fileList = new List<EvBinaryFileMetaData> ( );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( EvBinaryFiles.PARM_GROUP_ID, SqlDbType.VarChar, 50),
        new SqlParameter( EvBinaryFiles.PARM_SUB_GROUP_ID, SqlDbType.VarChar, 50),
      };
      cmdParms [ 0 ].Value = GroupId;
      cmdParms [ 1 ].Value = SubGroupId;

      // 
      // Generate the SQL query string
      // 
      sqlQueryString.AppendLine ( SQL_VIEW_QUERY );
      sqlQueryString.AppendLine ( "WHERE (" + EvBinaryFiles.DB_FILE_EXISTS + " = 1 ) " );
      sqlQueryString.AppendLine ( " AND (" + EvBinaryFiles.DB_FILE_STATUS + " <> '" + EvBinaryFileMetaData.FileStatus.Superseded + "' ) " );

      if ( GroupId == String.Empty )
      {
        sqlQueryString.AppendLine ( " AND (" + EvBinaryFiles.DB_GROUP_ID + " = '' ) " );
      }
      else
      {
        sqlQueryString.AppendLine ( " AND (" + EvBinaryFiles.DB_GROUP_ID + " = " + EvBinaryFiles.PARM_GROUP_ID + ") " );
      }

      if ( SubGroupId == String.Empty )
      {
        sqlQueryString.AppendLine ( " AND (" + EvBinaryFiles.DB_SUB_GROUP_ID + " = '' ) " );
      }
      else
      {
        sqlQueryString.AppendLine ( " AND (" + EvBinaryFiles.DB_SUB_GROUP_ID + " = " + EvBinaryFiles.PARM_SUB_GROUP_ID + ") " );
      }


      sqlQueryString.AppendLine ( "ORDER BY "
        + EvBinaryFiles.DB_FILE_ID + ", "
        + EvBinaryFiles.DB_UPDATED_DATE + " ; " );

      this.LogDebug ( sqlQueryString.ToString() );

      //
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString.ToString ( ), cmdParms ) )
      {
        // 
        // Iterate through the results extracting the role information.
        // 
        for ( int Count = 0; Count < table.Rows.Count; Count++ )
        {
          // 
          // Extract the table row
          // 
          DataRow row = table.Rows [ Count ];

          EvBinaryFileMetaData BinaryFile = this.readDataRow ( row );

          fileList.Add ( BinaryFile );

        } //END interation loop.

      }//END using method

      this.LogValue ( "fileList.Count: " + fileList.Count );
      // 
      // Return the list containing the User data object.
      // 
      this.LogMethodEnd ( "getBinaryFileList" );
      return fileList;

    }//END getView method.

    // ==================================================================================
    /// <summary>
    /// This class gets an list of last upload of versioned binary file objects. 
    /// </summary>
    /// <param name="ProjectId">String: Project identifier (Mandatory)</param>
    /// <param name="GroupId">Stirng: FilingIndex Identifier (Mandatory)</param>
    /// <param name="SubGroupId">String: A sub group identifier</param>
    /// <param name="FileId">String: A file identifier</param>
    /// <returns>List of EvBinaryFileMetaData: a list contains binary file data objects.</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Define the sql query parameters and the sql query string
    /// 
    /// 2. Execute the sql query string with parameters and store the values on datatable. 
    /// 
    /// 3. Iterate through the table and extract the row data to a binary file object. 
    /// 
    /// 4. Find the last instance of the file (with the highest version). 
    /// 
    /// 5. Add the object values to the return binary file meta data list. 
    /// 
    /// 6. Return the list of binary file meta data
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public List<EvBinaryFileMetaData> GetVersionedFileList (
      String GroupId,
      String SubGroupId,
      String FileId )
    {
      this.LogMethod ( "GetVersionedFileList method. " );

      //
      // Initialize a method debug status, a internal sql query string and a return list of binary file data objects.
      //
      StringBuilder sqlQueryString = new StringBuilder ( );
      List<EvBinaryFileMetaData> fileList = new List<EvBinaryFileMetaData> ( );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( EvBinaryFiles.PARM_GROUP_ID, SqlDbType.VarChar, 50),
        new SqlParameter( EvBinaryFiles.PARM_SUB_GROUP_ID, SqlDbType.VarChar, 50),
        new SqlParameter( EvBinaryFiles.PARM_FILE_ID, SqlDbType.VarChar, 100),
      };
      cmdParms [ 0 ].Value = GroupId;
      cmdParms [ 1 ].Value = SubGroupId;
      cmdParms [ 2 ].Value = FileId;

      // 
      // Generate the SQL query string
      // 
      sqlQueryString.AppendLine ( SQL_VIEW_QUERY );
      sqlQueryString.AppendLine ( "WHERE (" + EvBinaryFiles.DB_FILE_ID + " = " + EvBinaryFiles.PARM_FILE_ID + ") " );
      sqlQueryString.AppendLine ( " AND (" + EvBinaryFiles.DB_FILE_EXISTS + " = 1 ) " );

      if ( GroupId == String.Empty )
      {
        sqlQueryString.AppendLine ( " AND (" + EvBinaryFiles.DB_GROUP_ID + " = '' ) " );
      }
      else
      {
        sqlQueryString.AppendLine ( " AND (" + EvBinaryFiles.DB_GROUP_ID + " = " + EvBinaryFiles.PARM_GROUP_ID + ") " );
      }

      if ( SubGroupId == String.Empty )
      {
        sqlQueryString.AppendLine ( " AND (" + EvBinaryFiles.DB_SUB_GROUP_ID + " = '' ) " );
      }
      else
      {
        sqlQueryString.AppendLine ( " AND (" + EvBinaryFiles.DB_SUB_GROUP_ID + " = " + EvBinaryFiles.PARM_SUB_GROUP_ID + ") " );
      }

      sqlQueryString.AppendLine ( "ORDER BY "
        + EvBinaryFiles.DB_VERSION + ", "
        + EvBinaryFiles.DB_UPDATED_DATE + " ; " );

      this.LogDebug ( sqlQueryString.ToString ( ) );

      //
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString.ToString ( ), cmdParms ) )
      {
        this.LogDebug ( "Rows.Count: " + table.Rows.Count );
        // 
        // Iterate through the results extracting the role information.
        // 
        for ( int Count = 0; Count < table.Rows.Count; Count++ )
        {
          // 
          // Extract the table row
          // 
          DataRow row = table.Rows [ Count ];

          EvBinaryFileMetaData binaryFile = this.readDataRow ( row );

          fileList.Add ( binaryFile );

        } //END interation loop.

      }//END using method

      this.LogValue ( "fileList.Count: " + fileList.Count );
      // 
      // Return the list containing the User data object.
      // 
      this.LogMethodEnd ( "GetVersionedFileList" );
      return fileList;

    }//END getVersionedView method.
    #endregion

    #region Class Retrieval methods

    // ==================================================================================
    /// <summary>
    /// This class gets itmes of binary file data object. 
    /// </summary>
    /// <param name="Guid">Guid: a binary global unique identifier</param>
    /// <returns>EvBinaryFileMetaData: a binary file data object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Return an empty binary file object if the Binary file's Guid is empty 
    /// 
    /// 2. Define the sql query parameter and the sql query string
    /// 
    /// 3. Execute the sql query string with parameter and store the result on the datatable
    /// 
    /// 4. Extract the first data row to the binary file object. 
    /// 
    /// 5. Return the Binary file object. 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public EvBinaryFileMetaData GetFile ( Guid Guid )
    {
      this.LogMethod ( "GetFile method. " );
      this.LogDebug ( "Guid: " + Guid );
      // 
      // Define local variables
      // 
      string sqlQueryString;
      EvBinaryFileMetaData binaryFile = new EvBinaryFileMetaData ( );

      // 
      // Check that the TrialObjectId is valid.
      // 
      if ( Guid == Guid.Empty )
      {
        return binaryFile;
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter cmdParms = new SqlParameter ( EvBinaryFiles.PARM_GUID, SqlDbType.UniqueIdentifier );
      cmdParms.Value = Guid;

      // 
      // Generate the SQL query string
      // 
      sqlQueryString = SQL_VIEW_QUERY
        + "\r\n WHERE (" + EvBinaryFiles.DB_GUID + " = " + EvBinaryFiles.PARM_GUID + ");";

      this.LogDebug ( sqlQueryString );

      //
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
      {
        // 
        // If not rows the return
        //
        if ( table.Rows.Count == 0 )
        {
          return binaryFile;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        binaryFile = this.readDataRow ( row );

      }//END Using 

      // 
      // Return the TrialVisit data object.
      // 
      this.LogMethodEnd ( "GetFile" );
      return binaryFile;

    }//END GetItem class. 

    #endregion

    #region Class update methods

    // ==================================================================================
    /// <summary>
    /// This class adds a record to the binary file table.
    /// </summary>
    /// <param name="BinaryFile">EvBinaryFileMetaData: a binary file object</param>
    /// <returns>EvEventCodes: an event code for adding items to the binary file table</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define sql query parameters and execute the storeprocedure for adding items. 
    /// 
    /// 2. Return the event code for adding new items. 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public EvEventCodes addItem ( EvBinaryFileMetaData BinaryFile )
    {
      this.LogMethod ( "addItem method. " );
      this.LogDebug ( " BinaryFile.Version: " + BinaryFile.Version );
      this.LogDebug ( " BinaryFile.FileEncrypted: " + BinaryFile.FileEncrypted );
      this.LogDebug ( " BinaryFile.FileExists: " + BinaryFile.FileExists );
      this.LogDebug ( " BinaryFile.Comments: " + BinaryFile.Comments );
      this.LogDebug ( " BinaryFile.Language: " + BinaryFile.Language );
      //
      // Initialize a method debug status. 
      //
      BinaryFile.FileExists = true;

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] _cmdParms = getItemsParameters ( );
      SetParameters ( _cmdParms, BinaryFile );

      this.LogDebug ( EvSqlMethods.ListParameters(_cmdParms) );

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( _storedProcedureAddItem, _cmdParms ) == 0 )
      {
        this.LogMethodEnd ( "addItem" );
        return EvEventCodes.Database_Record_Update_Error;
      }

      this.LogMethodEnd ( "addItem" );
      return EvEventCodes.Ok;

    }//END addItem method. 


    // ==================================================================================
    /// <summary>
    /// This class adds a record to the binary file table.
    /// </summary>
    /// <param name="BinaryFile">EvBinaryFileMetaData: a binary file object</param>
    /// <returns>EvEventCodes: an event code for adding items to the binary file table</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define sql query parameters and execute the storeprocedure for adding items. 
    /// 
    /// 2. Return the event code for adding new items. 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public EvEventCodes updateItem ( EvBinaryFileMetaData BinaryFile )
    {
      //
      // Initialize a method debug status. 
      //
      this.LogMethod ( "updateItem" );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] _cmdParms = getItemsParameters ( );
      SetParameters ( _cmdParms, BinaryFile );

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( _storedProcedureUpdateItem, _cmdParms ) == 0 )
      {
        this.LogMethodEnd ( "updateItem" );
        return EvEventCodes.Database_Record_Update_Error;
      }

      this.LogMethodEnd ( "updateItem" );
      return EvEventCodes.Ok;

    }//END addItem method. 

    #endregion

    #region Class save file to repository methods
    /*
    ///=====================================================================================
    /// <summary>
    /// This class generates the file path name.
    /// </summary>
    /// <param name="_File">File data object</param>
    /// <returns>True: Successful, False: not successful</returns>
    ///-------------------------------------------------------------------------------------
    private bool SaveFileToDisk ( EvBinaryFile BinaryFile )
    {
      try
      {
        /// 
        /// Initialise the local variables.
        /// 
        this._DebugLog.AppendLine( "\r\nSaveFileToDisk method.";
        string FilePath = BinaryFile.FilePath;
        string extension = Path.GetExtension ( BinaryFile.FileName );

        // 
        // Strip out illegal characters
        // 
        FilePath = Evado.Model.EvStatics.StripIllegalDirChars ( FilePath );

        this._DebugLog.AppendLine( "\r\n FilePath: " + FilePath;

        // 
        // Increment the version counter.
         
        BinaryFile.Version++;

        this._DebugLog.AppendLine( "\r\n New Version: " + BinaryFile.Version;

        // 
        // Generate the directory path if it does not exist.
        // 
        if ( Directory.Exists ( this._RootPath + FilePath ) == false )
        {
          this._DebugLog.AppendLine( "\r\n Directory Path: " + this._RootPath + FilePath;
          Directory.CreateDirectory ( this._RootPath + FilePath );
        }

        // 
        // Add the file name to the directory path.
        // 
        FilePath += @"\"
         + BsStatics.StripIllegalDirChars ( BinaryFile.FileName )
         + "_" + BinaryFile.Version
         + extension;

        // 
        // Remove the "'" quotation marks
        // 
        FilePath = FilePath.Replace ( "'", "" );

         
        // Update the FilePath in the data object.
        // 
        BinaryFile.FilePath = FilePath;

        // 
        // Return the Filepath
        // 
        this._DebugLog.AppendLine( "\r\nFilePath: " + FilePath;

        // 
        // Move the file from the temporary path the actual path.
        // 
        File.Move ( BinaryFile.WebPath, this._RootPath + BinaryFile.FilePath );
        //File.Copy( binaryFile.WebPath, rootPath + FilePath );


      }
      catch ( Exception Ex )
      {
        this._DebugLog.AppendLine( "\r\n\r\n" + Ex.ToString ( );
        BinaryFile.FileExists = false;

        return false;
      }

      this._DebugLog.AppendLine( "\r\nSaveFileToDisk method return True.";
      BinaryFile.FileExists = true;

      return true;

    }//END SaveFileToDisk method.

    ///=====================================================================================
    /// <summary>
    /// GetFileFromDisk Method
    /// 
    /// Description:
    ///  Save the file to disk.
    /// 
    /// </summary>
    /// <param name="binaryFile">Bianry file data object</param>
    /// <returns>True: Successful, False: not successful</returns>
    ///-------------------------------------------------------------------------------------
    private bool GetFileFromDisk ( EvBinaryFile binaryFile )
    {
      try
      {
        // 
        // Initialise the local variables.
        // 
        this._DebugLog.AppendLine( "\r\nGetFileFromDisk method.";
        string fileName = Path.GetFileName ( binaryFile.FilePath );
        binaryFile.WebPath = "./" + ConfigurationManager.AppSettings [ "TempPath" ] + "/" + fileName;
        string repositoryPath = this._RootPath + binaryFile.FilePath;
        string temporaryPath = this._TempPath + fileName;

        this._DebugLog.AppendLine( "\r\n RootPath: " + this._RootPath
          + ", binaryFile.FilePath: " + binaryFile.FilePath
          + ", binaryFile.WebPath: " + binaryFile.WebPath;

        // 
        // Move the file from the temporary path the actual path.
        // 
        File.Copy ( repositoryPath, temporaryPath );

        // 
        // Test that the file has been transfered and exists.
        // 
        if ( File.Exists ( temporaryPath ) == false )
        {
          this._DebugLog.AppendLine( "\r\n File does not exist.";
          return false;
        }

        // 
        // Reset the last update date to now.
        // 
        File.SetLastAccessTime ( temporaryPath, DateTime.Now );
        File.SetLastWriteTime ( temporaryPath, DateTime.Now );
        this._DebugLog.AppendLine( "\r\n File access date updated.";

      }
      catch ( Exception Ex )
      {
        this._DebugLog.AppendLine( "\r\n\r\n" + Ex.ToString ( );
        binaryFile.FileExists = false;
        return false;
      }

      this._DebugLog.AppendLine( "\r\nGetFileFromDisk method return True.";
      return true;

    }//END GetFileFromDisk method
    */
    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

  }//END EvBinaryFiles class

}//END namespace Evado.Dal.Digital
