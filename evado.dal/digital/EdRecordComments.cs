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
using Evado.Dal;


namespace Evado.Dal.Digital
{
  /// <summary>
  /// This class is handles the data access layer for the form record comment data object.
  /// </summary>
  public class EdRecordComments  : EvDalBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EdRecordComments ( )
    {
      this.ClassNameSpace = "Evado.Dal.Clinical.EvFormRecordComments.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EdRecordComments ( EvClassParameters Settings )
    {
      this.ClassParameters = Settings;
      this.ClassNameSpace = "Evado.Dal.Clinical.EvFormRecordComments.";

      if ( this.ClassParameters.LoggingLevel == 0 )
      {
        this.ClassParameters.LoggingLevel = Evado.Dal.EvStaticSetting.LoggingLevel;
      }
    }

    #endregion

    #region Initialise class objects and constants

    private static string _eventLogSource = ConfigurationManager.AppSettings [ "EventLogSource" ];

    /// <summary>
    /// This constant defines the sql query string for selecting all items from form record comments table.
    /// </summary>
    private const string _sqlQuery_View = "Select * FROM EvFormRecordComments ";

    #region Define class storeprocedure.
    /// <summary>
    /// This constant defines storeprocedure for adding items to form record comment table. 
    /// </summary>
    private const string _STORED_PROCEDURE_AddItem = "usr_FormRecordComment_add";

    /// <summary>
    /// This constant defines storeprocedure for updating items on form record comment table. 
    /// </summary>
    private const string _STORED_PROCEDURE_UpdateItem = "usr_FormRecordComment_update";

    /// <summary>
    /// This constant defines storeprocedure for deleting items from form record comment table. 
    /// </summary>
    private const string _STORED_PROCEDURE_DeleteItem = "";
    #endregion
    /*
     *@RECORD_GUID uniqueidentifier,
 @RECORD_FIELD_GUID uniqueidentifier,
 @COMMENT_TYPE nvarchar(10),
 @AUTHOR_TYPE nvarchar(40),
 @CONTENT nvarchar(500),
 @USER_ID nvarchar(100),
 @USER_COMMON_NAME nvarchar(100),
 @COMMENT_DATE datetime
     */
    #region Define class parameters
    /// <summary>
    /// This constant defines parameter for Record global unique identifier of form record comments table. 
    /// </summary>
    private const string _parmRecordGuid = "@RECORD_GUID";

    /// <summary>
    /// This constant defines parameter for record field global unique identifier of form record comments table. 
    /// </summary>
    private const string _parmRecordFieldGuid = "@RECORD_FIELD_GUID";

    /// <summary>
    /// This constant defines parameter for comment type of form record comments table. 
    /// </summary>
    private const string _parmCommentType = "@COMMENT_TYPE";

    /// <summary>
    /// This constant defines parameter for author type of form record comments table. 
    /// </summary>
    private const string _parmAuthorType = "@AUTHOR_TYPE";

    /// <summary>
    /// This constant defines parameter for comment of form record comments table. 
    /// </summary>
    private const string _parmContent = "@CONTENT";

    /// <summary>
    /// This constant defines parameter for user identifier of form record comments table. 
    /// </summary>
    private const string _parmUserId = "@USER_ID";

    /// <summary>
    /// This constant defines parameter for user common name of form record comments table. 
    /// </summary>
    private const string _parmUserCommonName = "@USER_COMMON_NAME";

    /// <summary>
    /// This constant defines parameter for comment date of form record comments table. 
    /// </summary>
    private const string _parmCommentDate = "@COMMENT_DATE";
    #endregion
    #endregion

    #region SQL Paramter methods

    // =====================================================================================
    /// <summary>
    ///  This method creates an array of SqlParameter objects, to pass update values to 
    ///  database update stored procedures. 
    /// </summary>
    /// <returns>SqlParameter: an array of sql query parameters</returns>
    /// <remarks>
    /// This method consists of the following step: 
    /// 
    /// 1. Create the array of sql query parameters. 
    /// 
    /// 2. Return the array of sql query parameters. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private static SqlParameter [ ] getItemsParameters ( )
    {
      SqlParameter [ ] parms = new SqlParameter [ ] 
      {
        new SqlParameter( _parmRecordGuid, SqlDbType.UniqueIdentifier ),
        new SqlParameter( _parmRecordFieldGuid, SqlDbType.UniqueIdentifier),
        new SqlParameter( _parmCommentType, SqlDbType.NVarChar, 20),
        new SqlParameter( _parmAuthorType, SqlDbType.NVarChar, 20),
        new SqlParameter( _parmContent, SqlDbType.NVarChar, 500),
        new SqlParameter( _parmUserId, SqlDbType.NVarChar, 100),
        new SqlParameter( _parmUserCommonName, SqlDbType.NVarChar, 100),
        new SqlParameter( _parmCommentDate, SqlDbType.DateTime )
      };

      return parms;
    }

    // =====================================================================================
    /// <summary>
    ///  This method fills the parameter array with values to be passed to the update 
    ///  stored procedures.
    /// </summary>
    /// <param name="parms">SqlParameter: an array of sql query parameters</param>
    /// <param name="Comment">EvFormRecordComment: Values to bind to parameters</param>
    /// <remarks>
    /// This method consists of the following step: 
    /// 
    /// 1. Update the items from comment object to the array of sql query parameters. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private void SetParameters ( SqlParameter [ ] parms, EdFormRecordComment Comment )
    {
      parms [ 0 ].Value = Comment.RecordGuid;
      parms [ 1 ].Value = Comment.RecordFieldGuid;
      parms [ 2 ].Value = Comment.CommentType;
      parms [ 3 ].Value = Comment.AuthorType;
      parms [ 4 ].Value = Comment.Content;
      parms [ 5 ].Value = Comment.UserId;
      parms [ 6 ].Value = Comment.UserCommonName;
      parms [ 7 ].Value = Comment.CommentDate;

    }//END SetLetterParameters.

    #endregion

    #region Data Reader methods

    // =====================================================================================
    /// <summary>
    /// This method reads the content of the data row object containing a query result
    /// into an form record comment object.
    /// </summary>
    /// <param name="Row">DataRow: a data row object</param>
    /// <returns>EvFormRecordComment: a form record comment object</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Extract the compatible data row values to the comment object. 
    /// 
    /// 2. Return the form record comment object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EdFormRecordComment readDataRow ( DataRow Row )
    {
      // 
      // Initialise the comment object. 
      // 
      EdFormRecordComment comment = new EdFormRecordComment ( );

      // 
      // Extract the data object values.
      // 
      comment.RecordGuid = EvSqlMethods.getGuid ( Row, "FRC_RECORD_GUID" );
      comment.RecordFieldGuid = EvSqlMethods.getGuid ( Row, "FRC_RECORD_FIELD_GUID" );

      comment.AuthorType =
        Evado.Model.EvStatics.Enumerations.parseEnumValue<EdFormRecordComment.AuthorTypeCodes> (
        EvSqlMethods.getString ( Row, "FRC_AUTHOR_TYPE" ) );

      comment.Content = EvSqlMethods.getString( Row, "FRC_Content" );

      comment.UserId = EvSqlMethods.getString ( Row, "FRC_USER_ID" );
      comment.UserCommonName = EvSqlMethods.getString ( Row, "FRC_USER_COMMON_NAME" );
      comment.CommentDate = EvSqlMethods.getDateTime ( Row, "FRC_COMMENT_DATE" );
      comment.NewComment = false;

      // 
      // Return the object.
      // 
      return comment;

    }// End readRow method.

    #endregion

    #region Class list and queries methods
    
    // =====================================================================================
    /// <summary>
    /// This class returns a list of form record comments object retrieved by passed parameters.
    /// </summary>
    /// <param name="RecordGuid">Guid: The record guid identifier</param>
    /// <param name="RecordFieldGuid">Guid: The record field guid identifier</param>
    /// <param name="CommentType">EvFormRecordComment.CommentTypeCodes: Comment type setting</param>
    /// <param name="AuthorType"> EvFormRecordComment.AuthorTypeCodes: Author type seting</param>
    /// <returns>List of EvFormRecordComment: a list of form record comment.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Reset the record field guid to empty for all form coments.
    /// 
    /// 2. Define the sql query parameters and sql query string. 
    /// 
    /// 3. Execute the sql query string and store the results on data table. 
    /// 
    /// 4. Iterate through the data table and extract the data row to the comment object. 
    /// 
    /// 5. Add the object values to the Form Record Comment list. 
    /// 
    /// 6. Return the Form Record Comment List. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EdFormRecordComment> getCommentList (
      Guid RecordGuid,
      Guid RecordFieldGuid,
      EdFormRecordComment.CommentTypeCodes CommentType,
      EdFormRecordComment.AuthorTypeCodes AuthorType )
    {
      this.LogMethod( "getCommentList method. " );
      //this.LogDebugValue ( "RecordGuid: " + RecordGuid );
      //this.LogDebugValue ( "RecordFieldGuid: " + RecordFieldGuid );
      //this.LogDebugValue ( "CommentType: " + CommentType );
      //this.LogDebugValue ( "AuthorType: " + AuthorType );
      //
      // Initialzie the method debug log, local sql query string and the return list of comment object. 
      //

      string sqlQueryString;
      List<EdFormRecordComment> view = new List<EdFormRecordComment> ( );

      //
      // Reset the record field guid to empty for all form coments.
      //
      if ( CommentType == EdFormRecordComment.CommentTypeCodes.Form
        || CommentType == EdFormRecordComment.CommentTypeCodes.Subject )
      {
        RecordFieldGuid = Guid.Empty;
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( _parmRecordGuid, SqlDbType.UniqueIdentifier ),
        new SqlParameter( _parmRecordFieldGuid, SqlDbType.UniqueIdentifier),
        new SqlParameter( _parmCommentType, SqlDbType.NVarChar, 10),
        new SqlParameter( _parmAuthorType, SqlDbType.NVarChar, 10),
      };
      cmdParms [ 0 ].Value = RecordGuid;
      cmdParms [ 1 ].Value = RecordFieldGuid;
      cmdParms [ 2 ].Value = CommentType;
      cmdParms [ 3 ].Value = AuthorType;

      // 
      // Generate the SQL query string
      // 
      sqlQueryString = _sqlQuery_View + "WHERE FRC_RECORD_GUID = " + _parmRecordGuid;

      if ( RecordFieldGuid != Guid.Empty )
      {
        sqlQueryString += " AND FRC_RECORD_FIELD_GUID = " + _parmRecordFieldGuid;
      }

      if ( CommentType != EdFormRecordComment.CommentTypeCodes.Not_Set )
      {
        sqlQueryString += " AND FRC_COMMENT_TYPE = " + _parmCommentType;
      }

      if ( AuthorType != EdFormRecordComment.AuthorTypeCodes.Not_Set )
      {
        sqlQueryString += " AND FRC_AUTHOR_TYPE = " + _parmAuthorType;
      }

      sqlQueryString += " ORDER BY FRC_COMMENT_DATE; ";

      //this.LogDebugValue(  sqlQueryString );

      //
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
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

          EdFormRecordComment BinaryFile = this.readDataRow ( row );

          view.Add ( BinaryFile );

        } //END interation loop.

      }//END using method

      this.LogDebug( "View count: " + view.Count.ToString ( ) );

      this.LogMethodEnd ( "getCommentList." );
      // 
      // Return the list containing the User data object.
      // 
      return view;

    }//END getCommentList method.

    #endregion

    #region Class update methods

    // =====================================================================================
    /// <summary>
    /// This method add mew comments to the form record comments table. 
    /// </summary>
    /// <param name="CommentList">List of EvFormRecordComment: a list of form record comment objects</param>
    /// <returns>EvEventCodes: an event code for adding new commetns.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Iterate through the list of comments 
    /// 
    /// 2. Return 'OK' event code if, the comment's content is empty
    /// 
    /// 3. Exit if the comment's RecordGuid is empty. 
    /// 
    /// 4. Add new comment to the Form Record Comment table
    /// 
    /// 5. Return the event code for adding new commetns. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes  addNewComments ( List<EdFormRecordComment> CommentList )
    {
      //
      // Initialzie the method debug log and the return event code. 
      //
      this.LogMethod ( "addNewComments Method. " );
      this.LogDebug ( "CommentList count: " + CommentList.Count );

      EvEventCodes eventCode = EvEventCodes.Ok;

      //
      // Iterate through the list of comments to add new comments to the database
      //
      foreach ( EdFormRecordComment comment in CommentList )
      {
        this.LogDebug ( "Content: " + comment.Content +", NewComment" + comment.NewComment );

        if ( comment.NewComment == false )
        {
          this.LogDebug ( "New content = false" );
          continue;
        }

        if ( comment.Content == String.Empty )
        {
          this.LogDebug ( "Comment empty" );
          continue;
        }

        //
        // if the record guid is empty coment error
        //
        if ( comment.RecordGuid == Guid.Empty )
        {
          this.LogDebug ( "Record Guid is Empty" );
          continue;
        }

        //
        // Add comment to database 
        //
        eventCode = this.addItem ( comment );

        //
        // exit if an event error.
        //
        if ( eventCode != EvEventCodes.Ok )
        {
          return eventCode;

        }//END error exit

      }//END iteration loop

      this.LogMethodEnd ( "addNewComments." );

      return EvEventCodes.Ok;

    }//END addNewComments method 

    // =====================================================================================
    /// <summary>
    /// This class adds new items to the form record comment table. 
    /// </summary>
    /// <param name="Comment">EvFormRecordComment: a retrieved form record comment object</param>
    /// <returns>EvEventCodes: an event code for adding items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 2. Try define the SQL query parameters and execute the storeprocedure for adding items. 
    /// 
    /// 3. Return the event code for adding items. 
    /// 
    /// 4. Catch write out error message log. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes  addItem ( EdFormRecordComment Comment )
    {
      //
      // Initialzie the debug log
      //
      this.LogMethod ( "addItem." );
      //this.LogDebugValue ( "RecordGuid: " + Comment.RecordGuid );
      //this.LogDebugValue ( "RecordFieldGuid: " + Comment.RecordFieldGuid );
     // this.LogDebugValue ( "CommentType: " + Comment.CommentType );
      //this.LogDebugValue ( "AuthorType: " + Comment.AuthorType );
      //this.LogDebugValue ( "Content: " + Comment.Content );
      //this.LogDebugValue ( "UserCommonName: " + Comment.UserCommonName );
      //this.LogDebugValue ( "CommentDate: " + Comment.stCommentDate );
      try
      {
        // 
        // Define the SQL query parameters and load the query values.
        // 
        SqlParameter [ ] _cmdParms = getItemsParameters( );
        SetParameters( _cmdParms, Comment );

        //
        // Execute the update command.
        //
        if ( EvSqlMethods.StoreProcUpdate( _STORED_PROCEDURE_AddItem, _cmdParms ) == 0 )
        {
          return EvEventCodes.Database_Record_Update_Error;
        }

        this.LogMethodEnd ( "addItem." );
        return EvEventCodes.Ok;
      }
      catch ( Exception Ex )
      {
        //
        // Create the event message
        //
        string eventMessage = "DebugLog: + " + this.Log
          + "\r\n Exception: \r\n" + Evado.Model.Digital.EvcStatics.getException( Ex );

        Evado.Model.Digital.EvcStatics.WriteToEventLog( _eventLogSource, eventMessage, EventLogEntryType.Error );

        throw ( Ex );
      }

    }//END addItem class

    #endregion

  }//END EvFormRecordComments class

}//END namespace Evado.Dal.Digital
