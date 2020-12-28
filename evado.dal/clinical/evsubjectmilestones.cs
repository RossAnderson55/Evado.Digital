/* <copyright file="DAL\EvSubjectMilestones.cs" company="EVADO HOLDING PTY. LTD.">
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

//Annotations to XML instrument System specific libraries

using Evado.Model;
using Evado.Model.Digital;


namespace Evado.Dal.Clinical
{
  /// <summary>
  /// A business Component used to manage Ethics roles
  /// The Evado.Evado.TrialSubjectVisit is used in most methods 
  /// and is used to store serializable information about an account
  /// </summary>
  public class EvSubjectMilestones : EvDalBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EvSubjectMilestones ( )
    {
      this.ClassNameSpace = "Evado.Dal.Clinical.EvSubjectMilestones.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EvSubjectMilestones ( EvClassParameters Settings )
    {
      this.ClassParameters = Settings;
      this.ClassNameSpace = "Evado.Dal.Clinical.EvSubjectMilestones.";

      if ( this.ClassParameters.LoggingLevel == 0 )
      {
        this.ClassParameters.LoggingLevel = Evado.Dal.EvStaticSetting.LoggingLevel;
      }
    }

    #endregion

    #region Initialise Class constants and variables

    /// <summary>
    /// Define the selectionList query string ( where filters will be added by methods as needed.
    /// 
    /// </summary>
    private const string _sqlQuery_View = "Select * FROM EvSubjectMilestone_View ";

    private const string _sqlVisitCalendar = "Select * FROM EvSubjectCalendar_View ";

    /// 
    /// Define the stored procedure names.
    /// 
    private const string _storedProcedureAddItem = "usr_SubjectMilestone_add";
    private const string _storedProcedureUpdateItem = "usr_SubjectMilestone_update";
    private const string _storedProcedureDeleteItem = "usr_SubjectMilestone_delete";
    private const string _storedProcedureLockItem = "usr_SubjectMilestone_lock";
    private const string _storedProcedureUnlockItem = "usr_SubjectMilestone_unlock";


    private const string DB_Guid = "SM_Guid";
    private const string DB_TrialId = "TrialId";
    private const string DB_OrgId = "OrgId";
    private const string DB_SubjectId = "SubjectId";
    private const string DB_SCHEDULE_ID = "SCHEDULE_ID";
    private const string DB_MILESTONE_ID = "MilestoneId";
    private const string DB_VisitId = "VisitId";
    private const string DB_ScheduleDate = "SM_ScheduleDate";
    private const string DB_StartDate = "SM_StartDate";
    private const string DB_FinishDate = "SM_FinishDate";
    private const string DB_XmlData = "SM_XmlData";
    private const string DB_PROTOCOL_VIOLATION_LIST = "SM_PROTOCOL_VIOLATION_LIST";
    private const string DB_SIGNOFF_LIST = "SM_SIGNOFF_LIST";
    private const string DB_Comments = "SM_Comments";
    private const string DB_State = "SM_State";
    private const string DB_ProtocolViolation = "SM_ProtocolViolation";
    private const string DB_UpdatedByUserId = "SM_UpdatedByUSerId";
    private const string DB_UpdatedBy = "SM_UpdatedBy";
    private const string DB_UpdateDate = "SM_UpdateDate";
    private const string DB_BookedOut = "SM_BookedOutBy";
    private const string DB_StartDate1 = "SM_StartDate1";
    private const string DB_SchedulePeriod = "SM_SchedulePeriod";
    private const string DB_SiteRoleId = "SM_SiteRoleId";

    /// 
    /// Define the query parameter constants.
    /// 
    private const string PARM_Guid = "@Guid";
    private const string PARM_MILESTONE_GUID = "@MILESTONE_GUID";
    private const string PARM_TrialId = "@TrialId";
    private const string PARM_OrgId = "@OrgId";
    private const string PARM_SubjectId = "@SubjectId";
    private const string PARM_MilestoneId = "@MilestoneId";
    private const string PARM_SCHEDULE_ID = "@ScheduleId";
    private const string PARM_VisitId = "@VisitId";
    private const string PARM_GroupId = "@GroupId";
    private const string PARM_ScheduleDate = "@ScheduleDate";
    private const string PARM_StartDate = "@StartDate";
    private const string PARM_FinishDate = "@FinishDate";
    private const string PARM_XmlData = "@XmlData";
    private const string PARM_CDASH_METADATA = "@CDASH_METADATA";
    private const string PARM_PROTOCOL_VIOLATION_LIST = "@PROTOCOL_VIOLATION_LIST";
    private const string PARM_SIGNOFF_LIST = "@SIGNOFF_LIST";
    private const string PARM_Comments = "@Comments";
    private const string PARM_State = "@State";
    private const string PARM_ProtocolViolation = "@ProtocolViolation";
    private const string PARM_UpdatedByUserId = "@UpdatedByUSerId";
    private const string PARM_UpdatedBy = "@UpdatedBy";
    private const string PARM_UpdateDate = "@UpdateDate";
    private const string PARM_BookedOut = "@BookedOutBy";
    private const string PARM_StartDate1 = "@StartDate1";
    private const string PARM_SchedulePeriod = "@SchedulePeriod";
    private const string PARM_SiteRoleId = "@SiteRoleId";

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region SQL Parameter methods

    // =====================================================================================
    /// <summary>
    /// This class returns an array of sql query parameters.
    /// </summary>
    /// <returns>SqlParameter: an array of sql query parameters.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Create an array of sql query parameters. 
    /// 
    /// 2. Return an array of sql query parameters.
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private static SqlParameter [ ] GetParameters ( )
    {
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter( PARM_Guid, SqlDbType.UniqueIdentifier),
        new SqlParameter( PARM_MILESTONE_GUID, SqlDbType.UniqueIdentifier),
        new SqlParameter( PARM_TrialId, SqlDbType.NVarChar, 10),
        new SqlParameter( PARM_OrgId, SqlDbType.NVarChar, 10),
        new SqlParameter( PARM_SubjectId, SqlDbType.NVarChar, 20),

        new SqlParameter( PARM_MilestoneId, SqlDbType.NVarChar, 20),
        new SqlParameter( PARM_VisitId, SqlDbType.NVarChar, 20),
        new SqlParameter( PARM_ScheduleDate, SqlDbType.DateTime),
        new SqlParameter( PARM_StartDate, SqlDbType.DateTime),
        new SqlParameter( PARM_FinishDate, SqlDbType.DateTime),

        new SqlParameter( PARM_Comments, SqlDbType.NText),
        new SqlParameter( PARM_State, SqlDbType.NVarChar, 20),
        new SqlParameter( PARM_ProtocolViolation, SqlDbType.Bit),
        new SqlParameter( PARM_PROTOCOL_VIOLATION_LIST, SqlDbType.NText),
        new SqlParameter( PARM_SIGNOFF_LIST, SqlDbType.NText),

        new SqlParameter( PARM_CDASH_METADATA, SqlDbType.NVarChar, 250),
        new SqlParameter( PARM_UpdatedByUserId, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_UpdatedBy, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_UpdateDate, SqlDbType.DateTime),
        new SqlParameter( PARM_BookedOut, SqlDbType.NVarChar, 100),
      };

      return cmdParms;

    }//END GetParameters method

    // =====================================================================================
    /// <summary>
    /// This class assigns Milestone object's values to the array of sql query parameters. 
    /// </summary>
    /// <param name="cmdParms">SqlParameter: An array of sql query parameters.</param>
    /// <param name="milestone">EvMilestone: A milestone data object.</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Create new DB row Guid, if it is empty.
    /// 
    /// 2. Bind the Milestone object's values to the array of sql parameters. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private void SetParameters ( SqlParameter [ ] cmdParms, EvMilestone milestone )
    {
      if ( milestone.Guid == Guid.Empty )
      {
        milestone.Guid = Guid.NewGuid ( );
      }

      cmdParms [ 0 ].Value = milestone.Guid;
      cmdParms [ 1 ].Value = milestone.MilestoneGuid;
      cmdParms [ 2 ].Value = milestone.ProjectId;
      cmdParms [ 3 ].Value = milestone.OrgId;
      cmdParms [ 4 ].Value = milestone.SubjectId;

      cmdParms [ 5 ].Value = milestone.MilestoneId;
      cmdParms [ 6 ].Value = milestone.VisitId;
      cmdParms [ 7 ].Value = milestone.ScheduleDate;
      cmdParms [ 8 ].Value = milestone.StartDate;
      cmdParms [ 9 ].Value = milestone.FinishDate;

      cmdParms [ 10 ].Value = String.Empty;
      if ( milestone.CommentList.Count > 0 )
      {
        cmdParms [ 10 ].Value = Evado.Model.EvStatics.SerialiseObject<List<EvFormRecordComment>> ( milestone.CommentList );
      }
      cmdParms [ 11 ].Value = milestone.State;
      cmdParms [ 12 ].Value = milestone.ProtocolViolation;

      cmdParms [ 13 ].Value = String.Empty;
      if ( milestone.Data.ProtocolViolations.Count > 0 )
      {
        cmdParms [ 13 ].Value = Evado.Model.EvStatics.SerialiseObject<List<EvProtocolViolation>> ( milestone.Data.ProtocolViolations );
      }

      cmdParms [ 14 ].Value = String.Empty;
      if ( milestone.Data.Signoffs.Count > 0 )
      {
        cmdParms [ 14 ].Value = Evado.Model.EvStatics.SerialiseObject<List<EvUserSignoff>> ( milestone.Data.Signoffs );
      }
      cmdParms [ 15 ].Value = milestone.cDashMetadata;
      cmdParms [ 16 ].Value = milestone.UpdatedByUserId;
      cmdParms [ 17 ].Value = milestone.UserCommonName;
      cmdParms [ 18 ].Value = DateTime.Now;
      cmdParms [ 19 ].Value = milestone.BookedOutBy;

    }//END SetParameters class.

    #endregion

    #region SQL DataReader methods

    // =====================================================================================
    /// <summary>
    /// This method extracts the SubjectMilestone object content from the data reader object.
    /// </summary>
    /// <param name="Row">Object: Represents a row of data in a System.Data.DataTable.</param>
    /// <returns>An object containing EvMilestone object.</returns>
    /// <remarks>
    /// This method conisists of following steps. 
    /// 
    /// 1. Extract the compatible data row values to the Milestone object. 
    /// 
    /// 2. Return the Milestone data object. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private EvMilestone readRow ( DataRow Row )
    {
      //this.LogMethod ("readRow method. " );
      // 
      // Initialise the method variables and objects.
      // 
      EvMilestone milestone = new EvMilestone ( );
      EvMilestones subjectMilestone = new EvMilestones ( );
      string value = String.Empty;
      // 
      // Extract the compatible data row values to the Milestone object.
      // 
      milestone.Guid = EvSqlMethods.getGuid ( Row, EvSubjectMilestones.DB_Guid );
      milestone.MilestoneGuid = EvSqlMethods.getGuid ( Row, EvMilestones.DB_Milestone_Guid );
      milestone.ProjectId = EvSqlMethods.getString ( Row, EvMilestones.DB_TrialId );

      milestone.ScheduleId = EvSqlMethods.getInteger ( Row, EvMilestones.DB_SCHEDULE_ID );
      milestone.ScheduleTitle = EvSqlMethods.getString ( Row, EvMilestones.DB_SCH_TITLE );
      milestone.OrgId = EvSqlMethods.getString ( Row, DB_OrgId );
      milestone.MilestoneId = EvSqlMethods.getString ( Row, EvMilestones.DB_MilestoneId );
      milestone.SubjectId = EvSqlMethods.getString ( Row, EvSubjectMilestones.DB_SubjectId );
      milestone.VisitId = EvSqlMethods.getString ( Row, EvSubjectMilestones.DB_VisitId );

      milestone.Data.OptionalScheduleVersion = EvSqlMethods.getInteger ( Row, EvMilestones.DB_INITIAL_SCHEDULE_VERSION );
      milestone.Data.InitialScheduleVersion = EvSqlMethods.getInteger ( Row, EvMilestones.DB_OPTIONAL_SCHEDULE_VERSION );
      milestone.Data.PreviousVisitDate = EvSqlMethods.getDateTime ( Row, EvMilestones.DB_PREVIOUS_VISIT_DATE );
      milestone.RepeatNoTimes = EvSqlMethods.getInteger ( Row, EvMilestones.DB_REPEAT_NO_TIMES );

      value = EvSqlMethods.getString ( Row, EvMilestones.DB_MILESTONE_PERIOD_INCREMENT );
      if ( value != String.Empty )
      {
        milestone.MilestonePeriodIncrement = Evado.Model.EvStatics.Enumerations.parseEnumValue<EvSchedule.MilestonePeriodIncrements> ( value );
      }
      milestone.ScheduleDate = EvSqlMethods.getDateTime ( Row, EvSubjectMilestones.DB_ScheduleDate );


      milestone.StartDate = EvSqlMethods.getDateTime ( Row, EvSubjectMilestones.DB_StartDate );
      milestone.FinishDate = EvSqlMethods.getDateTime ( Row, EvSubjectMilestones.DB_FinishDate );
      milestone.Title = EvSqlMethods.getString ( Row, EvMilestones.DB_Title );
      milestone.Description = EvSqlMethods.getString ( Row, EvMilestones.DB_Description );
      milestone.Comments = EvSqlMethods.getString ( Row, EvSubjectMilestones.DB_Comments );
      milestone.ProtocolViolation = EvSqlMethods.getBool ( Row, EvSubjectMilestones.DB_ProtocolViolation );

      milestone.InterMilestonePeriod = EvSqlMethods.getInteger ( Row, EvMilestones.DB_INTER_VISIT_PERIOD );
      milestone.MilestoneRange = EvSqlMethods.getFloat ( Row, EvMilestones.DB_VISIT_PERIOD );

      milestone.MilestoneLaterThanConsentDate = EvSqlMethods.getBool ( Row, EvMilestones.DB_LaterThanConsentDate );

      string xmlData = EvSqlMethods.getString ( Row, EvMilestones.DB_XmlData );
      if ( xmlData != String.Empty )
      {
        // 
        // Deserialise it into the data object.
        // 
        milestone.Data = Evado.Model.EvStatics.DeserialiseObject<EvMilestoneData> ( xmlData );
      }
      else
      {
        milestone.Data.OptionalScheduleVersion = EvSqlMethods.getInteger ( Row, EvMilestones.DB_INITIAL_SCHEDULE_VERSION );
        milestone.Data.InitialScheduleVersion = EvSqlMethods.getInteger ( Row, EvMilestones.DB_OPTIONAL_SCHEDULE_VERSION );
        milestone.Data.PreviousVisitDate = EvSqlMethods.getDateTime ( Row, EvMilestones.DB_PREVIOUS_VISIT_DATE );
      }

      xmlData = EvSqlMethods.getString ( Row, EvSubjectMilestones.DB_XmlData );
      if ( xmlData != String.Empty )
      {
        xmlData = xmlData.Replace ( "encoding=\"utf-16\"", "encoding=\"utf-8\"" );
        xmlData = xmlData.Replace ( "Activity_Not_Completed", "Null" );
        xmlData = xmlData.Replace ( "Early_Finish", "Early_Start" );
        xmlData = xmlData.Replace ( "Late_Start", "Late_Finish" );
        xmlData = xmlData.Replace ( "Activities_Not_Completed", "Null" );
        xmlData = xmlData.Replace ( "Mandatory_Tests_Not_Completed", "Null" );
        xmlData = xmlData.Replace ( "Other", "Null" );
        xmlData = xmlData.Replace ( "Manual", "Null" );//Activities_Not_Completed
        // 
        // Deserialise it into the data object.
        // 
        milestone.Data = Evado.Model.Digital.EvcStatics.DeserialiseObject<EvMilestoneData> ( xmlData );

        for ( int i = 0; i < milestone.Data.ProtocolViolations.Count; i++ )
        {
          EvProtocolViolation pv = milestone.Data.ProtocolViolations [ i ];

          if ( pv.Type == EvProtocolViolation.ProtocolViolationTypes.Null )
          {
            milestone.Data.ProtocolViolations.RemoveAt ( i );
            i--;
          }
        }
      }
      else
      {
        milestone.Data.OptionalScheduleVersion = EvSqlMethods.getInteger ( Row, EvMilestones.DB_INITIAL_SCHEDULE_VERSION );
        milestone.Data.InitialScheduleVersion = EvSqlMethods.getInteger ( Row, EvMilestones.DB_OPTIONAL_SCHEDULE_VERSION );
        milestone.Data.PreviousVisitDate = EvSqlMethods.getDateTime ( Row, EvMilestones.DB_PREVIOUS_VISIT_DATE );

        value = EvSqlMethods.getString ( Row, EvSubjectMilestones.DB_PROTOCOL_VIOLATION_LIST );
        if ( value != String.Empty )
        {
          milestone.Data.ProtocolViolations = Evado.Model.Digital.EvcStatics.DeserialiseObject<List<EvProtocolViolation>> ( value );

          for ( int i = 0; i < milestone.Data.ProtocolViolations.Count; i++ )
          {
            EvProtocolViolation pv = milestone.Data.ProtocolViolations [ i ];

            if ( pv.Type == EvProtocolViolation.ProtocolViolationTypes.Null )
            {
              milestone.Data.ProtocolViolations.RemoveAt ( i );
              i--;
            }
          }
        }

        value = EvSqlMethods.getString ( Row, EvSubjectMilestones.DB_SIGNOFF_LIST );
        if ( value != String.Empty )
        {
          milestone.Data.Signoffs = Evado.Model.Digital.EvcStatics.DeserialiseObject<List<EvUserSignoff>> ( value );
        }
      }

      milestone.Order = EvSqlMethods.getInteger ( Row, EvMilestones.DB_Order );

      string type = EvSqlMethods.getString ( Row, EvMilestones.DB_Type );
      milestone.Type = ( EvMilestone.MilestoneTypes ) Enum.Parse ( typeof ( EvMilestone.MilestoneTypes ), type );

      string state = EvSqlMethods.getString ( Row, EvSubjectMilestones.DB_State );

      if ( state == "Rescheduled" ) { state = EvMilestone.MilestoneStates.Scheduled.ToString ( ); }

      milestone.State = ( EvMilestone.MilestoneStates ) Enum.Parse ( typeof ( EvMilestone.MilestoneStates ), state );

      milestone.UpdatedByUserId = EvSqlMethods.getString ( Row, EvSubjectMilestones.DB_UpdatedByUserId );
      milestone.UpdatedBy = EvSqlMethods.getString ( Row, EvSubjectMilestones.DB_UpdatedBy );
      milestone.UpdatedBy += " by " + EvSqlMethods.getDateTime ( Row, EvSubjectMilestones.DB_UpdateDate ).ToString ( "dd MMM yyyy HH:mm" );

      milestone.BookedOutBy += EvSqlMethods.getString ( Row, EvSubjectMilestones.DB_BookedOut );
      /*
      this.LogValue ( "MilestoneId: " + milestone.MilestoneId );
      this.LogValue ( "Dates: Scheduled: " + milestone.stScheduleDate );
      this.LogValue ( "Start: " + milestone.stStartDate );
      this.LogValue ( "Finish: " + milestone.stFinishDate );
      this.LogValue ( "Comments: " + milestone.Comments );
      */

      //
      // Get the milestone comment
      //
      this.getCommentsList ( milestone );

      // 
      // Return an object containing an EvMilestone object.
      // 
      return milestone;

    }//END readRow method

    //===================================================================================
    /// <summary>
    /// This method gets the Comment list if exists.
    /// </summary>
    /// <param name="Milestone"></param>
    //-----------------------------------------------------------------------------------
    private void getCommentsList ( EvMilestone Milestone )
    {
      //this.LogMethod ("getCommentsList method. " );
      //
      // there are no comments then exit.
      //
      if ( Milestone.Comments == String.Empty )
      {
        return;
      }

      //
      // Deserialize the comments if they are xml. 
      //
      if ( Milestone.Comments.Contains ( "<?xml version=" ) == true )
      {
        Milestone.CommentList = Evado.Model.Digital.EvcStatics.DeserialiseObject<List<EvFormRecordComment>> ( Milestone.Comments );

        //
        // reset the comments new status is an xml source
        //
        foreach ( EvFormRecordComment comment in Milestone.CommentList )
        {
          comment.RecordGuid = Milestone.Guid;
          comment.RecordFieldGuid = Guid.Empty;
          comment.NewComment = true;
        }

        //
        // empty comments 
        //
        Milestone.Comments = String.Empty;

        return;
      }//END XML comment list exists.

      //this._DebugLog.AppendLine( "\r\nCOMMENTS EXISTS \r\n" );
      //
      // Convert the comment into the comment with a delimiter
      //
      String stComments = Milestone.Comments.Replace ( "\r\n\r\n", "^" );

      //
      // Convert the comments string into array of comments.
      //
      String [ ] arrComments = stComments.Split ( '^' );

      //
      // Iterate through the array of comments
      //
      for ( int i = ( arrComments.Length - 1 ); i >= 0; i-- )
      {
        this.ParseComments ( Milestone, arrComments [ i ] );
      }

      //
      // empty comments 
      //
      Milestone.Comments = String.Empty;

    }//END get comment list.

    // =====================================================================================
    /// <summary>
    /// This method updates the form record comment structure.
    /// </summary>
    /// <param name="MilestStone">EvForm: a form object containing the form record</param>
    /// <param name="Comment">String: the comment text</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Convert the comment into an array of lines
    /// 
    /// 2. Iterate through the line to parse the message content.
    /// 
    /// 3. Append new comment to the list if it exists. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private void ParseComments ( EvMilestone MilestStone, String Comment )
    {
      //this.LogMethod ("PartComments method. " );
      //
      // Initialise method variables and objects.
      //
      const string delimiter_NameStart1 = "By ";
      const string delimiter_NameStart2 = "by ";
      const string delimiter_NameStart3 = "By: ";
      const string delimiter_NameStart4 = "by: ";
      const string delimiter_DateStart = " on ";
      int inNameStart = 0;
      int inDateStart = 0;
      int inNameLength = 0;
      string stLine = String.Empty;
      string stName = String.Empty;
      string stDate = String.Empty;
      string stContent = String.Empty;
      string stContent1 = String.Empty;
      string stContent2 = String.Empty;
      DateTime dtValue = Evado.Model.Digital.EvcStatics.CONST_DATE_NULL;
      bool bDateStampFound = false;
      EvFormRecordComment comment = new EvFormRecordComment ( );

      //
      // Convert the comment into an array of lines
      //
      Comment = Comment.Replace ( "\r\n", "~" );

      String [ ] arrLines = Comment.Split ( '~' );

      this.LogValue ( "The comment has " + arrLines.Length + " lines. " );

      //
      // Iterate through the line to parse the message content.
      //
      for ( int inLine = 0; inLine < arrLines.Length; inLine++ )
      {
        //
        // set the date stamp to false, this will assume that the line is content text 
        // not name date stamp.
        //
        bDateStampFound = false;

        //
        // get the line as a string.
        //
        stLine = arrLines [ inLine ];

        this.LogValue ( "Line: " + inLine + " >> " + stLine );

        //
        // Check for first name type
        //
        if ( stLine.Contains ( delimiter_NameStart1 ) == true
          || stLine.Contains ( delimiter_NameStart2 ) == true
          || stLine.Contains ( delimiter_NameStart3 ) == true
          || stLine.Contains ( delimiter_NameStart4 ) == true )
        {
          //
          // Standardise the name start string.
          //
          stLine = stLine.Replace ( delimiter_NameStart2, delimiter_NameStart1 );
          stLine = stLine.Replace ( delimiter_NameStart3, delimiter_NameStart1 );
          stLine = stLine.Replace ( delimiter_NameStart4, delimiter_NameStart1 );

          //
          // find the start of the name
          //
          inNameStart = stLine.IndexOf ( delimiter_NameStart1 );
          inDateStart = stLine.IndexOf ( delimiter_DateStart );

          //
          // the name length will be difference between the date start and name start plus the delimiter.
          //
          inNameLength = inDateStart - inNameStart - delimiter_NameStart1.Length;

          this.LogValue ( "inNameStart: " + inNameStart
            + ", inDateStart: " + inDateStart
            + ", inNameLength: " + inNameLength );

          //
          // if the text length is greater than zero process the comment.
          //  Assume that the string is a comment content.
          //
          if ( inNameLength > 0 )
          {
            //
            // check to see if there is content before the name start.
            //
            if ( inNameStart > 0 )
            {
              stContent1 = stLine.Substring ( 0, inNameStart );
            }

            //
            // retrieve the name string.
            //
            stName = stLine.Substring (
              inNameStart + delimiter_NameStart1.Length,
              inNameLength );

            stDate = stLine.Substring ( inDateStart + delimiter_DateStart.Length );

            //
            // validate that a date time object has been found.
            //
            if ( stDate.Length > 0 )
            {
              if ( DateTime.TryParse ( stDate, out dtValue ) == true )
              {
                bDateStampFound = true;
              }
            }// Date string found.

            this.LogValue ( "Parsing Name line: \r\n Comment1: " + stContent1
             + "\r\n Name: " + stName
             + "\r\n Date: " + stDate );

          }//END Name text has been found.

          //
          // Process the stLine as a comment content.
          //
          if ( bDateStampFound == false )
          {
            if ( stContent != String.Empty )
            {
              stContent += "\r\n";
            }
            stContent = arrLines [ inLine ];

            //
            // reset the name and date as this is not a signoff line.
            //
            stName = String.Empty;
            stDate = String.Empty;
            dtValue = Evado.Model.Digital.EvcStatics.CONST_DATE_NULL;
          }
          else
          {
            //
            // if there is content prior to the name date stamp include in the content.
            //
            if ( stContent1 != string.Empty )
            {
              if ( stContent != String.Empty )
              {
                stContent += "\r\n";
              }
              stContent = stContent1;
            }
          }//END else - bDateStampFound

        }//END Line delimiter found.
        else
        {
          if ( stContent != String.Empty )
          {
            stContent += "\r\n";
          }
          stContent += stLine;
        }

      }//END line iteration loop

      //
      // Append new comment to the list if it exists. 
      //
      if ( stContent != String.Empty )
      {
        //this.LogValue ( "Comment: \r\n stContent: " + stContent
        //  + "\r\n Name: " + stName
        //  + "\r\n Date: " + stDate );

        comment = new EvFormRecordComment (
          MilestStone.Guid,
          EvFormRecordComment.AuthorTypeCodes.Not_Set,
          stName,
          stName,
          stContent );
        if ( stDate != String.Empty )
        {
          comment.CommentDate = dtValue;
        }
        comment.NewComment = true;

        //
        // Append the comment to the list.
        //
        MilestStone.CommentList.Add ( comment );
      }//END if content add

    }//ENd ParseComments method.

    #endregion

    #region SQL Query methods

    // =====================================================================================
    /// <summary>
    /// This method gets a list of Milestone objects based on the query parameters object, 
    /// activitySelection object and withRecords condition.
    /// </summary>
    /// <param name="QueryParameters">Object: An object that contains the query parmaters.</param>
    /// <param name="WithRecords">Bool: A boolean withRecord status.</param>
    /// <param name="ActivitySelection">Enum: An enum ActivitySelection.</param>
    /// <param name="WithActivities">Bool: A boolean WithActivities status.</param>
    /// <param name="HideCompleted">Bool: A boolean HideCompleted status.</param>
    /// <returns>A list containing an EvMilestone object.</returns>
    ///<remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Return an empty Milestones list, if the VisitId is empty. 
    /// 
    /// 2. Define the sql query parameters and sql query string. 
    /// 
    /// 3. Execute the sql query string and store the results on datatable. 
    /// 
    /// 4. Loop through the table and extract the data row values to the Milestone object. 
    /// 
    /// 5. Add the Milestone object values to the Milestones list. 
    /// 
    /// 6. Add Milestone Records to the Milestone list, if they exist. 
    /// 
    /// 7. Return the Milestones list. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public List<EvMilestone> getMilestoneList (
      EvQueryParameters QueryParameters,
      EvActivity.ActivitySelection ActivitySelection,
      bool WithActivities,
      bool WithRecords,
      bool HideCompleted )
    {
      this.LogMethod ( "getMilestoneList method. " );
      this.LogValue ( "ProjectId: " + QueryParameters.TrialId );
      this.LogValue ( "ScheduleId: " + QueryParameters.ScheduleId );
      this.LogValue ( "OrgId: " + QueryParameters.OrgId );
      this.LogValue ( "SubjectId: " + QueryParameters.SubjectId );
      this.LogValue ( "MilestoneId: " + QueryParameters.MilestoneId );
      this.LogValue ( "ScheduleDate: " + QueryParameters.stStartDate );
      this.LogValue ( "FinishDate: " + QueryParameters.stFinishDate );
      this.LogValue ( "DatePeriod: " + QueryParameters.DatePeriodMonths );
      this.LogValue ( "IsCurrent: " + QueryParameters.IsCurrent );
      this.LogValue ( "State: " + QueryParameters.State );
      this.LogValue ( "WithActivities: " + WithActivities );
      this.LogValue ( "WithRecords: " + WithRecords );
      this.LogValue ( "HideCompleted: " + HideCompleted );

      // 
      // Define the local variables and objects.
      // 
      EvSubjectMilestoneActivities subjectMilestoneActivities = new EvSubjectMilestoneActivities ( this.ClassParameters );
      List<EvMilestone> view = new List<EvMilestone> ( );
      string sqlQueryString = String.Empty;

      //
      // Return an empty Milestones list, if the VisitId is empty. 
      //
      if ( QueryParameters.TrialId == String.Empty )
      {
        return view;
      }

      // 
      // Define the query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter( PARM_TrialId, SqlDbType.NVarChar, 10),
        new SqlParameter( PARM_SCHEDULE_ID, SqlDbType.Int),
        new SqlParameter( PARM_OrgId, SqlDbType.NVarChar, 10),
        new SqlParameter( PARM_SubjectId, SqlDbType.NVarChar, 20),
        new SqlParameter( PARM_MilestoneId, SqlDbType.NVarChar, 20),
        new SqlParameter( PARM_StartDate, SqlDbType.DateTime ),
        new SqlParameter( PARM_StartDate1, SqlDbType.DateTime ),
        new SqlParameter( PARM_State, SqlDbType.VarChar, 20 ),
        new SqlParameter( PARM_SiteRoleId, SqlDbType.VarChar, 50 ),
      };
      cmdParms [ 0 ].Value = QueryParameters.TrialId;
      cmdParms [ 1 ].Value = QueryParameters.ScheduleId;
      cmdParms [ 2 ].Value = QueryParameters.OrgId;
      cmdParms [ 3 ].Value = QueryParameters.SubjectId;
      cmdParms [ 4 ].Value = QueryParameters.MilestoneId;
      cmdParms [ 5 ].Value = QueryParameters.StartDate;
      cmdParms [ 6 ].Value = QueryParameters.StartDate.AddDays ( 1 );
      cmdParms [ 7 ].Value = QueryParameters.State;
      cmdParms [ 8 ].Value = QueryParameters.RoleId;

      // 
      // If a month period is defined uses it.
      // 
      if ( QueryParameters.DatePeriodMonths > 0 )
      {
        cmdParms [ 5 ].Value = QueryParameters.StartDate.AddMonths ( QueryParameters.DatePeriodMonths );
      }

      // 
      // If a finish date is defined use it.
      // 
      if ( QueryParameters.FinishDate > Evado.Model.Digital.EvcStatics.CONST_DATE_NULL )
      {
        cmdParms [ 5 ].Value = QueryParameters.FinishDate;
      }

      //
      // Generate the SQL query string.   
      //
      sqlQueryString = _sqlQuery_View + " WHERE ( " + DB_TrialId + " = " + PARM_TrialId + " )  \r\n"
        + " AND ( " + DB_SCHEDULE_ID + " = " + PARM_SCHEDULE_ID + "  )  \r\n";
      ;

      if ( QueryParameters.OrgId != String.Empty )
      {
        sqlQueryString += " AND ( " + DB_OrgId + " = " + PARM_OrgId + "  )  \r\n";
      }

      if ( QueryParameters.SubjectId != String.Empty )
      {
        sqlQueryString += " AND ( " + DB_SubjectId + " = " + PARM_SubjectId + " )  \r\n";
      }

      if ( QueryParameters.MilestoneId != String.Empty )
      {
        sqlQueryString += " AND ( " + EvMilestones.DB_MilestoneId + " = " + PARM_MilestoneId + " )  \r\n";
      }

      if ( QueryParameters.RoleId != String.Empty )
      {
        sqlQueryString += " AND ( " + EvMilestones.DB_ROLE_ID + " = " + PARM_SiteRoleId + " OR " + EvMilestones.DB_ROLE_ID + " = '')  \r\n";
      }

      if ( QueryParameters.StartDate > Evado.Model.Digital.EvcStatics.CONST_DATE_NULL )
      {
        sqlQueryString += " AND ( " + DB_ScheduleDate + " >= @StartDate) "
          + " AND ( " + DB_ScheduleDate + " <= @StartDate1 )  \r\n";
      }

      if ( QueryParameters.ObjectType != String.Empty )
      {
        sqlQueryString += " AND ( " + EvMilestones.DB_Type + " = '" + QueryParameters.ObjectType + "' )  \r\n";
      }

      // 
      // If a Current = true then only display current appointments, orgCount.e. Not CancelledRecords. 
      // 
      if ( HideCompleted == true )
      {
        this.LogValue ( "Hiding completed milestones." );

        sqlQueryString += " AND ( " + DB_State +"  = '" + EvMilestone.MilestoneStates.Scheduled + "' "
          + " OR " + DB_State + " = '" + EvMilestone.MilestoneStates.Attended + "' ) \r\n";
      }
      else
      {
        // 
        // If Current = true then only display current appointments, orgCount.e. Not CancelledRecords. 
        // 
        if ( QueryParameters.State != String.Empty )
        {
          sqlQueryString += " AND ( " + DB_State + " = " + PARM_State+ " ) \r\n";
        }
        else
        {
          if ( QueryParameters.IsCurrent == true )
          {
            sqlQueryString += " AND not ( " + DB_State + " = '" + EvMilestone.MilestoneStates.Cancelled.ToString ( ) + "' ) \r\n";
          }
        }
      }

      if ( QueryParameters.OrderBy.Length == 0 )
      {
        sqlQueryString += " ORDER BY " + DB_ScheduleDate;
      }
      else
      {
        sqlQueryString += " ORDER BY " + QueryParameters.OrderBy;
      }

      this.LogValue ( sqlQueryString );

      //
      // Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
      {
        // 
        // Iterate through the table rows count information.
        // 
        for ( int Count = 0; Count < table.Rows.Count; Count++ )
        {
          // 
          // Extract the table row.
          // 
          DataRow row = table.Rows [ Count ];

          EvMilestone milestone = this.readRow ( row );

          if ( WithActivities == true )
          {
            // 
            // Get the Subject Milestone's activities.
            // 
            milestone.ActivityList = subjectMilestoneActivities.getView (
              milestone.MilestoneGuid,
              EvActivity.ActivityTypes.Null,
              ActivitySelection );

            this.LogValue ( subjectMilestoneActivities.Log );
          }

          // 
          // If the with methods is selected then attach the trial records associated with the milestone.
          // 
          if ( WithRecords == true )
          {
            this.getMilestoneRecords ( milestone );
          }

          view.Add ( milestone );

        }//END for loop

      }//END using

      // 
      // Return a list containing an EvMilestone object.
      // 
      return view;

    }//END getView method.

    // =====================================================================================
    /// <summary>
    /// This method updates the trial milestone's query status.
    /// </summary>
    /// <param name="MilestoneList">A list milestone objects.</param>
    /// <param name="VisitId">string  A visit id.</param>
    // -------------------------------------------------------------------------------------
    public void SetVisitRecordStatus ( String VisitId, List<EvMilestone> MilestoneList )
    {
      this.LogMethod ( "SetVisitRecordStatus method." );
      this.LogValue ( "VisitId: " + VisitId );
      this.LogValue ( "MIlestoneList.Count: " + MilestoneList.Count );
      //
      // Initialise the methods variables and objects.
      //
      String SQL_String = String.Empty;
      List<EvSubjectQueryState> queryStatusList = new List<EvSubjectQueryState> ( );

      //
      // Define the query parameters.
      //
      SqlParameter [ ] cmdParms =
      {
        new SqlParameter( PARM_VisitId, SqlDbType.NVarChar, 20),
      };
      cmdParms [ 0 ].Value = VisitId.Trim ( );

      //
      // Define the query.
      //
      SQL_String = "Select VisitId as VISIT_ID,\r\n"
         + "SUM(CASE TR_QueryState  WHEN '" + EvForm.QueryStates.Open + "' THEN 1 ELSE 0 END)  AS OPEN_QUERIES ,\r\n"
         + "SUM(CASE TR_QueryState  WHEN '" + EvForm.QueryStates.Closed + "' THEN 1 ELSE 0 END)   AS CLOSED_QUERIES \r\n"
         + " FROM EvRecords \r\n"
         + " WHERE VisitId = @VisitId \r\n"
         + " GROUP BY SubjectId;";

      this.LogValue ( "SQL: " + SQL_String );


      using ( DataTable table = EvSqlMethods.RunQuery ( SQL_String, cmdParms ) )
      {
        this.LogValue ( "Query Rows: " + table.Rows.Count );
        // 
        // Iterate through table row count list until all of the table rows read or the maximum list length is reached..
        // 
        for ( int count = 0; count < table.Rows.Count; count++ )
        {
          // 
          // Extract the table row.
          // 
          DataRow row = table.Rows [ count ];

          String subjectId = EvSqlMethods.getString ( row, "SUBJECT_ID" );
          int iOpen_Queries = EvSqlMethods.getInteger ( row, "OPEN_QUERIES" );
          int iClosed_Queries = EvSqlMethods.getInteger ( row, "CLOSED_QUERIES" );

          EvSubjectQueryState queryState = new EvSubjectQueryState ( subjectId );

          if ( iClosed_Queries > 0 )
          {
            queryState.QueryState = EvForm.QueryStates.Closed;
          }
          if ( iOpen_Queries > 0 )
          {
            queryState.QueryState = EvForm.QueryStates.Open;
          }

          this.LogValue ( "Subject: " + queryState.SubjectId
            + ", Open: " + iOpen_Queries
            + ", Closed: " + iClosed_Queries
            + ", QueryState: " + queryState.QueryState );

          queryStatusList.Add ( queryState );

        }//END result iteration loop.

      }//END using statement

      foreach ( EvMilestone milestone in MilestoneList )
      {
        foreach ( EvSubjectQueryState state in queryStatusList )
        {
          if ( state.SubjectId == milestone.SubjectId )
          {
            this.LogValue ( "state.QueryState: " + state.QueryState );

            milestone.RecordQueryState = state.QueryState;
          }
        }
      }

      this.LogValue ( "SetSubjectRecordStatus method completed" );

    }//END SetSubjectRecordStatus method 

    // =====================================================================================
    /// <summary>
    /// This method retrieves the Report table based on the selection report object. 
    /// </summary>
    /// <param name="Report">EvReport: A report object</param>
    /// <returns>EvReport: An object containing Report object.</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Create new 8 report column headers, if the column number is not equal to 8. 
    /// 
    /// 2. Define the sql query parameters and sql query string. 
    /// 
    /// 3. Execute the sql query string and store the results on datatable. 
    /// 
    /// 4. Loop through the table and extract data row to the Milestone object. 
    /// 
    /// 5. Loop through the Milestone object's protocol violations and extract values to report row object. 
    /// 
    /// 6. Add the Report row object' values to the Report object. 
    /// 
    /// 7. Return the Report data object. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvReport getProtocolViolations ( EvReport Report )
    {
      this.LogMethod ( "getProtocolViolations report method." );
      this.LogValue ( "Value: " + Report.Queries [ 0 ].Value );

      // 
      // Define the local variables and objects.
      // 
      int columns = 8;
      List<EvReportRow> reportRows = new List<EvReportRow> ( );
      string sqlQueryString = String.Empty;
      EvMilestone milestone = new EvMilestone ( );

      Report.ReportTitle = "Trial Protocol Violations";
      Report.DataSourceId = EvReport.ReportSourceCode.ProtocolViolations;
      Report.ReportDate = DateTime.Now;

      //
      // Reset the column definitions
      //
      Report.Columns = new List<EvReportColumn> ( );

      // 
      // Set the Report columns.
      // 
      EvReportColumn column = new EvReportColumn ( );
      column.HeaderText = "ProjectId";
      column.SectionLvl = 1;
      column.GroupingIndex = true;
      column.DataType = EvReport.DataTypes.Text;
      column.SourceField = "TrialId";
      column.GroupingType = EvReport.GroupingTypes.None;

      Report.Columns.Add ( column );

      // 
      // Set the Report columns
      // 
      column = new EvReportColumn ( );
      column.HeaderText = "ScheduleId";
      column.SectionLvl = 2;
      column.GroupingIndex = true;
      column.DataType = EvReport.DataTypes.Text;
      column.SourceField = "ScheduleId";
      column.GroupingType = EvReport.GroupingTypes.None;

      Report.Columns.Add ( column );
      // 
      // Set the Report columns
      // 
      column = new EvReportColumn ( );
      column.HeaderText = "VisitId";
      column.SectionLvl = 3;
      column.GroupingIndex = true;
      column.DataType = EvReport.DataTypes.Text;
      column.SourceField = "VisitId";
      column.GroupingType = EvReport.GroupingTypes.None;

      Report.Columns.Add ( column );

      // 
      // Set the Report columns
      // 
      column = new EvReportColumn ( );
      column.HeaderText = "SubjectId";
      column.SectionLvl = 3;
      column.GroupingIndex = false;
      column.DataType = EvReport.DataTypes.Text;
      column.SourceField = "SubjectId";
      column.GroupingType = EvReport.GroupingTypes.None;

      Report.Columns.Add ( column );

      // 
      // Set the Report columns
      // 
      column = new EvReportColumn ( );
      column.HeaderText = "Milestone";
      column.SectionLvl = 3;
      column.GroupingIndex = false;
      column.DataType = EvReport.DataTypes.Text;
      column.SourceField = "Milestone";
      column.GroupingType = EvReport.GroupingTypes.None;

      Report.Columns.Add ( column );

      // 
      // Set the trial columns
      // 
      column = new EvReportColumn ( );
      column.HeaderText = "Violation Type";
      column.SectionLvl = 0;
      column.GroupingIndex = false;
      column.DataType = EvReport.DataTypes.Text;
      column.SourceField = "Type";
      column.GroupingType = EvReport.GroupingTypes.None;
      column.StyleWidth = "170px";

      Report.Columns.Add ( column );

      // 
      // Set the trial columns
      // 
      column = new EvReportColumn ( );
      column.HeaderText = "Comments";
      column.SectionLvl = 0;
      column.GroupingIndex = false;
      column.DataType = EvReport.DataTypes.Text;
      column.SourceField = "Comments";
      column.GroupingType = EvReport.GroupingTypes.None;

      Report.Columns.Add ( column );

      // 
      // Set the trial columns
      // 
      column = new EvReportColumn ( );
      column.HeaderText = "Status";
      column.SectionLvl = 0;
      column.GroupingIndex = false;
      column.DataType = EvReport.DataTypes.Text;
      column.SourceField = "Status";
      column.GroupingType = EvReport.GroupingTypes.None;
      column.StyleWidth = "70px";

      Report.Columns.Add ( column );

      //
      // Set the data record column length
      //
      columns = Report.Columns.Count;

      // 
      // Define the query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter( PARM_TrialId, SqlDbType.NVarChar, 10),
        new SqlParameter( PARM_SCHEDULE_ID, SqlDbType.NVarChar, 10 ),
      };
      cmdParms [ 0 ].Value = Report.Queries [ 0 ].Value; ;
      cmdParms [ 1 ].Value = Report.Queries [ 1 ].Value;

      //
      // Generate the SQL query string.
      //
      sqlQueryString = _sqlQuery_View + " WHERE ( TrialId = @TrialId ) "
        + "  AND (SM_ProtocolViolation = 1) ";

      if ( Report.Queries [ 1 ].Value != String.Empty )
      {
        sqlQueryString += " AND ( ARM_Index = @ScheduleId ) ";
      }

      sqlQueryString += " ORDER BY SM_StartDate";

      this.LogValue ( sqlQueryString );

      //
      // Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
      {
        this.LogValue ( "Returned Records: " + table.Rows.Count );

        // 
        // Iterate through the table rows count information.
        // 
        for ( int count = 0; count < table.Rows.Count; count++ )
        {
          // 
          // Extract the table row.
          // 
          DataRow row = table.Rows [ count ];

          milestone = this.readRow ( row );

          // 
          // Extract the protocol violations for the milestone.
          // 
          foreach ( EvProtocolViolation protocolViolation in milestone.Data.ProtocolViolations )
          {
            EvReportRow reportRow = new EvReportRow ( columns );

            reportRow.ColumnValues [ 0 ] = milestone.ProjectId;
            reportRow.ColumnValues [ 1 ] = milestone.ScheduleId.ToString ( );
            reportRow.ColumnValues [ 2 ] = milestone.VisitId;
            reportRow.ColumnValues [ 3 ] = milestone.SubjectId;
            reportRow.ColumnValues [ 4 ] = milestone.Title;
            reportRow.ColumnValues [ 5 ] = Evado.Model.Digital.EvcStatics.Enumerations.enumValueToString ( protocolViolation.Type );
            reportRow.ColumnValues [ 6 ] = protocolViolation.CommentsHtml;
            reportRow.ColumnValues [ 7 ] = protocolViolation.StateDesc;

            Report.DataRecords.Add ( reportRow );

          }//END protocol violation iteration loop.

        }//END record interation loop.

      }//END using statement.

      this.LogValue ( "Returned Protocol violations: " + reportRows.Count );

      if ( Report.DataRecords.Count == 0 )
      {
        EvReportRow reportRow = new EvReportRow ( columns );

        reportRow.ColumnValues [ 0 ] = milestone.ProjectId;
        reportRow.ColumnValues [ 1 ] = milestone.ScheduleId.ToString ( );
        reportRow.ColumnValues [ 2 ] = milestone.VisitId;
        reportRow.ColumnValues [ 3 ] = milestone.SubjectId;
        reportRow.ColumnValues [ 4 ] = milestone.Title;
        reportRow.ColumnValues [ 5 ] = String.Empty;
        reportRow.ColumnValues [ 6 ] = String.Empty;
        reportRow.ColumnValues [ 7 ] = String.Empty;

        Report.DataRecords.Add ( reportRow );

      }//END Add empty row

      // 
      // Return an object containing an EvReportRow object.
      // 
      return Report;

    } //END getProtocolViolations method.

    // =====================================================================================
    /// <summary>
    /// This method gets the list of form object items associated with a milestone.
    /// </summary>
    /// <param name="Milestone">EvMilestone: A Milestone object.</param>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Retrieve the list of form object items. 
    /// 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private void getMilestoneRecords ( EvMilestone Milestone )
    {
      this.LogMethod ( "getMilestoneRecords method. " );
      //
      // Define an object.
      //
      EvFormRecords formRecords = new EvFormRecords ( );

      //
      // Get records.
      //
      Milestone.FormList = formRecords.getRecordList ( Milestone.ProjectId,
        Milestone.VisitId,
        String.Empty,
        EvFormObjectStates.Null );

    }//END getMilestoneRecords method

    // =====================================================================================
    /// <summary>
    /// This method returns a list of Milestone object items. 
    /// </summary>
    /// <param name="Subject">EvSubject: A Subject data object.</param>
    /// <param name="IncludeUnScheduledVisits">Boolean: true, if the Milestone includes unscheduled visits.</param>
    /// <param name="StartDate">DateTime: A milestone's start date.</param>
    /// <param name="SchedulePeriod">EvMilestone.MilestoneSchedulePeriod: a Schedule period</param>
    /// <param name="SiteRoleId">String: A site role identifier.</param>
    /// <param name="SelectAll">Boolean: true, if the all milestones are selected</param>
    /// <param name="SelectActivities">Boolean: true, include activities in data</param>
    /// <returns>List of EvMilestone: A list containing an Milestone object.</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Return an empty Milestones list, if the Milestone object's VisitId is empty. 
    /// 
    /// 2. Update the startdate to consent date, if the startdate and schedule period are null. 
    /// 
    /// 3. Define the sql query parameters and sql query string. 
    /// 
    /// 4. Execute the sql query string and store the results on datatable. 
    /// 
    /// 5. Loop through the table and extract the data row to the Milestone object. 
    /// 
    /// 6. Add the Milestone's activitylist to the Milestone object. 
    /// 
    /// 7. Add the Milestone object's values to the Milestones list. 
    /// 
    /// 8. Return the Milestones list. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public List<EvMilestone> getVisitSchedule (
      EvSubject Subject,
      bool IncludeUnScheduledVisits,
      DateTime StartDate,
      EvMilestone.MilestoneSchedulePeriod SchedulePeriod,
      EvRoleList SiteRoleId,
      bool SelectAll,
      bool SelectActivities )
    {
      this.LogMethod ( "getVisitSchedule method. " );
      this.LogValue ( "ProjectId: " + Subject.TrialId );
      this.LogValue ( "SubjectId: " + Subject.SubjectId );
      this.LogValue ( "IncludeUnScheduledVisits: " + IncludeUnScheduledVisits );
      this.LogValue ( "ConsentDate: " + Subject.stConsentDate );
      this.LogValue ( "ScheduleDate: " + Evado.Model.Digital.EvcStatics.getDateAsString ( StartDate ) );
      this.LogValue ( "SchedulePeriod: " + SchedulePeriod );
      this.LogValue ( "SiteRoleId: " + SiteRoleId );
      // 
      // Define the local variables and objects.
      // 
      EvMilestoneActivities milestoneActivities = new EvMilestoneActivities ( );
      List<EvMilestone> view = new List<EvMilestone> ( );
      string sqlQueryString = String.Empty;

      // 
      // If milestone trial is empty exit as there is nothing to query.
      // 
      if ( Subject.TrialId == String.Empty )
      {
        return view;
      }

      // 
      // If the start date is null and a schedule period is set then start the schedule period 
      // at the milestone's consent date.
      // 
      if ( StartDate == Evado.Model.Digital.EvcStatics.CONST_DATE_NULL
        && SchedulePeriod > EvMilestone.MilestoneSchedulePeriod.Null )
      {
        StartDate = Subject.ConsentDate;
      }

      this.LogValue ( " StartDate: " + Evado.Model.Digital.EvcStatics.getDateAsString ( StartDate )
       + ", EndDate: " + Evado.Model.Digital.EvcStatics.getDateAsString ( StartDate.AddMonths ( ( int ) SchedulePeriod ) ) );

      // 
      // Define the query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter(PARM_TrialId, SqlDbType.NVarChar, 10),
        new SqlParameter(PARM_SubjectId, SqlDbType.NVarChar, 20),
        new SqlParameter(PARM_StartDate, SqlDbType.DateTime ),
        new SqlParameter(PARM_StartDate1, SqlDbType.DateTime ),
        new SqlParameter( PARM_SiteRoleId, SqlDbType.VarChar, 50 ),
      };
      cmdParms [ 0 ].Value = Subject.TrialId;
      cmdParms [ 1 ].Value = Subject.SubjectId;
      cmdParms [ 2 ].Value = StartDate;
      cmdParms [ 3 ].Value = StartDate.AddMonths ( 3 );
      cmdParms [ 4 ].Value = SiteRoleId.ToString ( );

      // 
      // If a schedule period is defined use it.
      // 
      if ( SchedulePeriod > EvMilestone.MilestoneSchedulePeriod.Null )
      {
        DateTime endDate = StartDate.AddMonths ( ( int ) SchedulePeriod );
        endDate = StartDate.AddDays ( 5 );
        cmdParms [ 3 ].Value = endDate;
      }

      // 
      // Generate the SQL query string.
      // 
      sqlQueryString = _sqlQuery_View + " WHERE ( TrialId = @TrialId ) "
        + "AND ( SubjectId = @SubjectId ) AND NOT( M_Type = '" + EvMilestone.MilestoneTypes.UnScheduled + "' ) \r\n";

      //
      // If include unscheduled visits is true then remove the Unscheduled visit filter.
      //
      if ( IncludeUnScheduledVisits == true )
      {
        sqlQueryString = _sqlQuery_View + " WHERE ( TrialId = @TrialId ) "
          + "AND ( SubjectId = @SubjectId ) \r\n";
      }

      //
      // Part of the upgrade to v4.2.0
      //
      if ( SiteRoleId != EvRoleList.Null )
      {
        // sqlQueryString += " AND ( M_SITE_ROLE_ID = " + PARM_SiteRoleId + " OR M_SITE_ROLE_ID = '' ) ";
      }

      if ( StartDate > Evado.Model.Digital.EvcStatics.CONST_DATE_NULL
        && SchedulePeriod != EvMilestone.MilestoneSchedulePeriod.Null
        && SchedulePeriod != EvMilestone.MilestoneSchedulePeriod.Project )
      {
        sqlQueryString += " AND ( SM_ScheduleDate >= @StartDate) "
          + " AND ( SM_ScheduleDate <= @StartDate1 ) \r\n";
      }

      // 
      // If selectAll is true then select all scheduled subjectMilestones including both 
      // scheduled, attended, and completed.
      // 
      if ( SelectAll == false )
      {
        sqlQueryString += " AND ( (SM_State = '" + EvMilestone.MilestoneStates.Scheduled.ToString ( ) + "' )  )\r\n";
      }
      else
      {
        sqlQueryString += " AND NOT ( SM_State = '" + EvMilestone.MilestoneStates.Cancelled.ToString ( ) + "' )";
      }

      sqlQueryString += " AND ( SM_Deleted = '0' )";

      // 
      // Set the sorting order of the query.
      // 
      sqlQueryString += " ORDER BY SCHEDULE_ID,SM_ScheduleDate;";

      this.LogValue ( sqlQueryString );

      //
      // Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
      {
        // 
        // Iterate through the table rows count information.
        // 
        for ( int Count = 0; Count < table.Rows.Count; Count++ )
        {
          // 
          // Extract the table row.
          // 
          DataRow row = table.Rows [ Count ];

          EvMilestone milestone = this.readRow ( row );

          // 
          // Get the Milestone's activities.
          // 
          if ( SelectActivities == true )
          {
            milestone.ActivityList = milestoneActivities.getActivityList (
              milestone.MilestoneGuid,
              EvActivity.ActivityTypes.Null,
              false,
              false );

            this.LogValue ( milestoneActivities.DebugLog );
          }
          view.Add ( milestone );
        }
      }

      this.LogValue ( " DAL: View object count: " + view.Count );
      // 
      // Return a list containing an EvMilestone object.
      // 
      return view;

    }//END getVisitSchedule method

    // =====================================================================================
    /// <summary>
    /// This method returns a list of Milestone data object items.
    /// </summary>
    /// <param name="Subject">EvSubject: A Subject data object.</param>
    /// <param name="StartDate">DateTime: A milestone's start date.</param>
    /// <param name="SchedulePeriod">EvMilestone.MilestoneSchedulePeriod: a Schedule period</param>
    /// <param name="SelectAll">Boolean: true, if the all milestones are selected</param>
    /// <returns>List of EvMilestone: A list containing an Milestone object.</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Return an empty Milestones list, if the Milestone object's VisitId is empty. 
    /// 
    /// 2. Update the startdate to consent date, if the startdate and schedule period are null. 
    /// 
    /// 3. Define the sql query parameters and sql query string. 
    /// 
    /// 4. Execute the sql query string and store the results on datatable. 
    /// 
    /// 5. Loop through the table and extract the data row to the Milestone object. 
    /// 
    /// 6. Add the Milestone's activitylist to the Milestone object, if the Milestone object's Guid is not empty. 
    /// 
    /// 7. Add the Milestone object's values to the Milestones list. 
    /// 
    /// 8. Return the Milestones list. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public List<EvMilestone> getVisitCalendar ( EvSubject Subject,
      DateTime StartDate,
      EvMilestone.MilestoneSchedulePeriod SchedulePeriod,
      bool SelectAll )
    {
      this.LogMethod ( "getVisitCalendar method. " );
      this.LogValue ( "ProjectId: " + Subject.TrialId );
      this.LogValue ( "SubjectId: " + Subject.SubjectId );
      this.LogValue ( "ScheduleDate: " + Evado.Model.Digital.EvcStatics.getDateAsString ( StartDate ) );
      this.LogValue ( "SchedulePeriod: " + SchedulePeriod );
      this.LogValue ( "EndDate: " + Evado.Model.Digital.EvcStatics.getDateAsString ( StartDate.AddMonths ( 3 ) ) );

      // 
      // Define the local variables and objects.
      // 
      EvMilestoneActivities milestoneActivities = new EvMilestoneActivities ( );
      List<EvMilestone> view = new List<EvMilestone> ( );
      string sqlQueryString = String.Empty;
      Guid lastGuid = Guid.Empty;
      List<EvActivity> activityList = new List<EvActivity> ( );

      // 
      // If trial is empty exit as there is nothing to query.
      // 
      if ( Subject.TrialId == String.Empty )
      {
        return view;
      }

      // 
      // If the start date is null and a schedule period is set then start the schedule period 
      // at the milestone's consent date.
      // 
      if ( StartDate == Evado.Model.Digital.EvcStatics.CONST_DATE_NULL
        && SchedulePeriod > 0 )
      {
        StartDate = Subject.ConsentDate;
      }

      // 
      // Define the query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter(PARM_TrialId, SqlDbType.NVarChar, 10),
        new SqlParameter(PARM_SubjectId, SqlDbType.NVarChar, 20),
        new SqlParameter(PARM_StartDate, SqlDbType.DateTime ),
        new SqlParameter(PARM_StartDate1, SqlDbType.DateTime ),
      };
      cmdParms [ 0 ].Value = Subject.TrialId;
      cmdParms [ 1 ].Value = Subject.SubjectId;
      cmdParms [ 2 ].Value = StartDate;
      cmdParms [ 3 ].Value = StartDate.AddMonths ( 3 );

      // 
      // If a schedule period is defined use it.
      // 
      if ( SchedulePeriod > EvMilestone.MilestoneSchedulePeriod.Null )
      {
        cmdParms [ 3 ].Value = Evado.Model.Digital.EvcStatics.getDateAsString ( StartDate.AddMonths ( ( int ) SchedulePeriod ) );
      }

      // 
      // Generate the SQL query string.
      // 
      sqlQueryString = _sqlVisitCalendar + " WHERE ( TrialId = @TrialId ) "
        + "AND ( SubjectId = @SubjectId ) \r\n";

      if ( StartDate > Evado.Model.Digital.EvcStatics.CONST_DATE_NULL
        && SchedulePeriod != EvMilestone.MilestoneSchedulePeriod.Null
        && SchedulePeriod != EvMilestone.MilestoneSchedulePeriod.Project )
      {
        sqlQueryString += " AND ( ScheduleDate >= @StartDate) "
          + " AND ( ScheduleDate <= @StartDate1 ) \r\n";
      }

      // 
      // If selectAll is true then select all scheduled subjectMilestones including both 
      // scheduled, attended, and completed.
      // 
      if ( SelectAll == false )
      {
        sqlQueryString += " AND ( SM_State = '" + EvMilestone.MilestoneStates.Scheduled.ToString ( ) + "' )\r\n";
      }
      else
      {
        sqlQueryString += " AND NOT ( SM_State = '" + EvMilestone.MilestoneStates.Cancelled.ToString ( ) + "' )";
      }

      // 
      // Set the sorting order of the query.
      // 
      sqlQueryString += " ORDER BY ScheduleDate;";

      this.LogValue ( sqlQueryString );

      //
      // Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
      {
        // 
        // Iterate through the table rows count information.
        // 
        for ( int Count = 0; Count < table.Rows.Count; Count++ )
        {
          // 
          // Extract the table row.
          // 
          DataRow row = table.Rows [ Count ];

          EvMilestone milestone = new EvMilestone ( );

          milestone.Guid = EvSqlMethods.getGuid ( row, "SM_Guid" );
          milestone.MilestoneGuid = EvSqlMethods.getGuid ( row, "M_Guid" );
          milestone.ProjectId = EvSqlMethods.getString ( row, "TrialId" );
          milestone.SubjectId = EvSqlMethods.getString ( row, "SubjectId" );
          milestone.ScheduleId = EvSqlMethods.getInteger ( row, "SCHEDULE_ID" );
          milestone.MilestoneId = EvSqlMethods.getString ( row, "MilestoneId" );
          milestone.Title = EvSqlMethods.getString ( row, "M_Title" );

          milestone.ScheduleDate = EvSqlMethods.getDateTime ( row, "ScheduleDate" );
          milestone.StartDate = EvSqlMethods.getDateTime ( row, "SM_ScheduleDate" );

          // 
          // If the miletones guid changes it is a new Milestone so retrieve
          // the activities for tha Milestone.
          // 
          if ( lastGuid != milestone.Guid )
          {
            // 
            // Get the Milestone's activities.
            // 
            activityList = milestoneActivities.getActivityList ( milestone.MilestoneGuid, EvActivity.ActivityTypes.Null, false, false );

            //this._Status += "\r\n " + milestoneActivities.DebugLog;

            lastGuid = milestone.Guid;
          }

          // 
          // Interate through the activity list.
          // 
          foreach ( EvActivity activity in activityList )
          {
            milestone.ActivityList.Add ( activity );

          }//END interation loop

          //
          // Add milestone to the list.
          //
          view.Add ( milestone );
        }
      }

      this.LogValue ( " View object count: " + view.Count );
      // 
      // Return a list containing an EvMilestone object.
      // 
      return view;

    }//END getVisitCalendar method

    // =====================================================================================
    /// <summary>
    /// This method returns a list of option object items for the selection milestone object. 
    /// </summary>
    /// <param name="ProjectId">String: a trial identifier.</param>
    /// <param name="SubjectId">String: a milestone identifier.</param>
    /// <param name="useGuid">Boolean: true, if the Guid is used</param>
    /// <returns>List of EvOption: A list containing an Option object.</returns>
    /// <remarks>
    /// This method consists of following steps:
    /// 
    /// 1. Return an empty Option list, if the VisitId is empty. 
    /// 
    /// 2. Define the sql query parameters and sql query string. 
    /// 
    /// 3. Execute the sql query string and store the results on datatable. 
    /// 
    /// 4. Loop through the table and extract the data row to the Option object. 
    /// 
    /// 5. Add the Option object's values to the Options list. 
    /// 
    /// 6. Return the Options list.
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public List<EvOption> getOptionList (
      String ProjectId,
      String SubjectId,
      bool useGuid )
    {
      this.LogMethod ( "getOptionList method. " );
      this.LogValue ( "ProjectId: " + ProjectId );
      this.LogValue ( "SubjectId: " + SubjectId );

      // 
      // Define the local variables and objects.
      // 
      List<EvOption> list = new List<EvOption> ( );
      string sqlQueryString = String.Empty;

      EvOption option = new EvOption ( );

      // 
      // If useGuid is equal to true, add an option object to list.
      // 
      if ( useGuid == true )
      {
        option = new EvOption ( Guid.Empty.ToString ( ), String.Empty );
      }
      list.Add ( option );

      //
      // Validate whether the VisitId is not empty.
      //
      if ( ProjectId == String.Empty )
      {
        return list;
      }

      // 
      // Define the query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter(PARM_TrialId, SqlDbType.NVarChar, 10),
        new SqlParameter(PARM_SubjectId, SqlDbType.NVarChar, 20),
      };
      cmdParms [ 0 ].Value = ProjectId;
      cmdParms [ 1 ].Value = SubjectId;

      //
      // Generate the SQL query string.
      //
      sqlQueryString = _sqlQuery_View
        + " WHERE ( TrialId = @TrialId ) "
        + " AND NOT ( SM_State = '" + EvMilestone.MilestoneStates.Cancelled.ToString ( ) + "' ) ";

      if ( SubjectId != String.Empty )
      {
        sqlQueryString += " AND ( SubjectId = @SubjectId ) ";
      }
      sqlQueryString += " ORDER BY SubjectId, SM_ScheduleDate;";

      this.LogValue ( sqlQueryString );

      //
      // Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
      {
        // 
        // Iterate through the table rows count information.
        // 
        for ( int Count = 0; Count < table.Rows.Count; Count++ )
        {
          // 
          // Extract the table row.
          // 
          DataRow row = table.Rows [ Count ];

          string stOption = EvSqlMethods.getString ( row, "VisitId" )
            + " - " + EvSqlMethods.getString ( row, "M_Title" );

          if ( SubjectId == String.Empty )
          {
            stOption = "(" + EvSqlMethods.getString ( row, "SubjectId" ) + ") " + stOption;
          }

          //  
          //  Process the query result.
          // 
          if ( useGuid == true )
          {
            option = new EvOption ( EvSqlMethods.getString ( row, "SM_Guid" ), stOption );
          }
          else
          {
            option = new EvOption ( EvSqlMethods.getString ( row, "VisitId" ), stOption );
          }

          // 
          // Append the option object to the list.
          // 
          list.Add ( option );

        }//END iteration loop.

      }//END using method

      // 
      // Return a list containing an EvOption object.
      // 
      return list;

    } //END getList method.

    // =====================================================================================
    /// <summary>
    ///  This method returns a list of option object for the previous visit dates based on VisitId and SubjectId 
    /// </summary>
    /// <param name="ProjectId">String: A trial identifier</param>
    /// <param name="SubjectId">String: A Subject identifier.</param>
    /// <param name="VisitId">String: A visit identifier.</param>
    /// <returns>DateTime: pervious visit date</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Return an empty Option list, if the VisitId is empty. 
    /// 
    /// 2. Define the sql query parameters and sql query string. 
    /// 
    /// 3. Execute the sql query string and store the results on datatable. 
    /// 
    /// 4. Loop through the table and extract the data row to the Option object. 
    /// 
    /// 5. Add the Option object's values to the Options list. 
    /// 
    /// 6. Return the Options list.
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public DateTime getPreviouslyAttendedVisitDates (
      String ProjectId,
      String SubjectId,
      String VisitId )
    {
      this.LogMethod ( "getPreviouslyAttendedVisitDates." );
      this.LogValue ( "ProjectId: " + ProjectId );
      this.LogValue ( "SubjectId: " + SubjectId );
      this.LogValue ( "VisitId: " + VisitId );
      // 
      // Define the local variables and objects 
      // 
      DateTime previousAttededVisitDate = Evado.Model.Digital.EvcStatics.CONST_DATE_NULL;
      String sqlQueryString = String.Empty;
      bool visitFound = false;

      // 
      // Define the query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter(PARM_TrialId, SqlDbType.NVarChar, 10),
        new SqlParameter(PARM_SubjectId, SqlDbType.NVarChar, 20),
      };
      cmdParms [ 0 ].Value = ProjectId;
      cmdParms [ 1 ].Value = SubjectId;
      //
      // Generate the SQL query string.
      //
      sqlQueryString = _sqlQuery_View
        + " WHERE ( TrialId = @TrialId ) "
        + " AND NOT ( SM_State = '" + EvMilestone.MilestoneStates.Cancelled.ToString ( ) + "' ) "
        + " AND NOT ( SM_State = '" + EvMilestone.MilestoneStates.Scheduled.ToString ( ) + "' ) "
        + " AND ( SubjectId = @SubjectId ) "
        + " ORDER BY SM_StartDate;";

      this.LogValue ( sqlQueryString );

      //
      // Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
      {
        // 
        // Iterate through table rows count information.
        // 
        for ( int Count = 0; Count < table.Rows.Count; Count++ )
        {
          // 
          // Extract the table row.
          // 
          DataRow row = table.Rows [ Count ];

          //
          // Extract the start and finish dates for each milestone visit.
          //
          DateTime dtStart = EvSqlMethods.getDateTime ( row, "SM_StartDate" );

          String visitId = EvSqlMethods.getString ( row, "VisitId" );

          //
          // If the visit is found set visit found to true
          //
          if ( visitId == VisitId )
          {
            visitFound = true;
          }

          //
          // if the visit is not found set the visit date to previousAttededVisitDate.
          //
          if ( visitFound == false )
          {
            previousAttededVisitDate = dtStart;
          }

        }//END iteration loop.

      }//END using method

      // 
      // Return a previous visit date.
      // 
      return previousAttededVisitDate;

    }//END getPreviouslyAttendedVisitDate method.

    // =====================================================================================
    /// <summary>
    /// This method gets a ArrayList containing a  visitSchedule EvSubjectVisitSummary objects.
    /// The queryState objects contain the results of a pivot query on Adverse EvEvent status.
    /// </summary>
    /// <param name="SubjectId">String: A milestone identifier.</param>
    /// <returns>List of EvSubjectVisitSummary: A list of milestone visit queryState object.</returns>
    /// <remarks>
    /// This method consists of following steps:
    /// 
    /// 1. Define the sql query parameters and sql query string. 
    /// 
    /// 2. Execute the sql query string and store the results on data reader. 
    /// 
    /// 3. Return an empty milestone visit queryState list, if the data reader has no value. 
    /// 
    /// 4. Else, scroll through the reader and extract values to the milestone visit queryState object. 
    /// 
    /// 5. Add the visit queryState object's values to the list of visit queryState object items. 
    /// 
    /// 6. Return the list of visit queryState object. 
    /// </remarks>
    //  ------------------------------------------------------------------------------------
    public EvSubjectVisitSummary getSummary (
      String SubjectId )
    {
      this.LogMethod ( "getSummary method. " );
      this.LogValue ( "SubjectId: " + SubjectId );

      // 
      // Define the local variables and object.
      // 
      EvSubjectVisitSummary visitSummary = new EvSubjectVisitSummary ( );
      string sqlQueryString = String.Empty;
      string sGroup = "SubjectId";

      // 
      // Define the SQL query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter(PARM_SubjectId, SqlDbType.NVarChar, 20),
      };
      cmdParms [ 0 ].Value = SubjectId;

      // 
      // Define the query string.
      // 
      sqlQueryString = "Select " + sGroup + ", " +
        "SUM(CASE SM_State WHEN '" + EvMilestone.MilestoneStates.Scheduled + "' THEN 1 ELSE 0 END ) AS Scheduled, " +
        "SUM(CASE SM_State WHEN '" + EvMilestone.MilestoneStates.Attended + "' THEN 1 ELSE 0 END ) AS Attended, " +
        "SUM(CASE SM_State WHEN '" + EvMilestone.MilestoneStates.Completed + "' THEN 1 ELSE 0 END ) AS Completed, " +
        "SUM(CASE SM_State WHEN '" + EvMilestone.MilestoneStates.Monitored + "' THEN 1 ELSE 0 END ) AS Monitored, " +
        "SUM(CASE SM_State WHEN '" + EvMilestone.MilestoneStates.Issues_Resolved + "' THEN 1 ELSE 0 END ) AS Issues_Resolved, " +
        "SUM(CASE SM_State WHEN '" + EvMilestone.MilestoneStates.Cancelled + "' THEN 1 ELSE 0 END ) AS Cancelled, " +
        "COUNT(SM_State) AS Total " +
        "FROM EvSubjectMilestone_View ";
      sqlQueryString += "WHERE SubjectId = @SubjectId ";
      sqlQueryString += "GROUP BY " + sGroup + ";";

      this.LogValue ( sqlQueryString );

      // 
      // Execute the query against the database.
      // 
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
      {
        if ( table.Rows.Count == 0 )
        {
          return visitSummary;
        }
        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        visitSummary.Group = EvSqlMethods.getString ( row, sGroup );
        visitSummary.Secheduled = EvSqlMethods.getInteger ( row, "Scheduled" );
        visitSummary.Attended = EvSqlMethods.getInteger ( row, "Attended" );
        visitSummary.Completed = EvSqlMethods.getInteger ( row, "Completed" );
        visitSummary.Monitored = EvSqlMethods.getInteger ( row, "Monitored" );
        visitSummary.IssuesResolved = EvSqlMethods.getInteger ( row, "Issues_Resolved" );
        visitSummary.Cancelled = EvSqlMethods.getInteger ( row, "Cancelled" );
        visitSummary.Total = EvSqlMethods.getInteger ( row, "Total" );
      }

      // 
      // Return a list containing an EvSubjectVisitSummary object.
      // 
      return visitSummary;

    }//END getSummary method.

    // =====================================================================================
    /// <summary>
    /// This method queries the database to find the maximum number of unscheduled milestones
    /// (of each type) that have been generated across all subjects.
    /// The result is a list of item counts, the item is the milestoneId the count is the 
    /// maximum number of a particular milestones in the trial.
    /// </summary>
    /// <param name="ProjectId">String: The trial identifier</param>
    /// <returns>List of EvItemCount: A list containing an Field Count object.</returns>
    /// <remarks>
    /// This method consists of following steps: 
    /// 
    /// 1. Define the sql query parameters and sql query string. 
    /// 
    /// 2. Execute the sql query string and store the results on datatable. 
    /// 
    /// 3. Loop through the table and top up the CountOfMilestones list, if the milestoneId does not exist. 
    /// 
    /// 4. Loop through the CountOfMilestones list and count the items that match MilestoneId. 
    /// 
    /// 5. Return the CountOfMilestones list. 
    /// </remarks>
    //  ------------------------------------------------------------------------------------
    public List<EvItemCount> getUnScheduledMilestoneCount (
      String ProjectId )
    {
      this.LogMethod ( "getUnScheduledMilestoneCount method. " );
      this.LogValue ( "ProjectId: " + ProjectId );

      // 
      // Define the local variables and objects. 
      // 
      List<EvItemCount> CountOfMilestones = new List<EvItemCount> ( );
      String sqlQueryString = String.Empty;

      //
      // String of the milestones id that have been found.
      //
      String stMilestoneIds = String.Empty;

      //
      // STAGE 1 -- Query the database to get a result set of milestones each milestone has generated 
      //

      // 
      // Define the SQL query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter(PARM_TrialId, SqlDbType.NVarChar, 10),
      };
      cmdParms [ 0 ].Value = ProjectId;

      // 
      // Generate the query string.
      // 
      sqlQueryString = "SELECT dbo.EvSubjectMilestones.MilestoneId As MilestoneId, "
      + " dbo.EvSubjectMilestones.SubjectId as SubjectId, "
      + " COUNT(dbo.EvSubjectMilestones.VisitId) AS Count "
      + " FROM dbo.EvSubjectMilestones INNER JOIN"
      + " dbo.EvMilestones ON dbo.EvSubjectMilestones.MilestoneId = dbo.EvMilestones.MilestoneId"
      + " GROUP BY dbo.EvSubjectMilestones.MilestoneId, dbo.EvSubjectMilestones.SubjectId, "
      + " dbo.EvMilestones.M_Type, dbo.EvSubjectMilestones.TrialId "
      + " HAVING  (dbo.EvMilestones.M_Type = 'UnScheduled') AND (TrialId = @TrialId ) ";

      this.LogValue ( sqlQueryString );

      //
      // Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
      {
        // 
        // Iterate through the table rows count information.
        // 
        for ( int Count = 0; Count < table.Rows.Count; Count++ )
        {

          // 
          // Extract the table row.
          // 
          DataRow row = table.Rows [ Count ];

          String stMilestoneId = EvSqlMethods.getString ( row, "MilestoneId" );
          int inCount = EvSqlMethods.getInteger ( row, "Count" );

          //
          // If the milestone is not in the milestone list then add it to the list.
          //
          if ( stMilestoneIds.Contains ( stMilestoneId ) == false )
          {
            stMilestoneIds += "," + stMilestoneId;
            CountOfMilestones.Add ( new EvItemCount ( stMilestoneId, inCount ) );

            continue;
          }

          //
          // Iterate through the item counts to find the matching item
          // and update the count if it is smaller that the returned count.
          //
          foreach ( EvItemCount itemCount in CountOfMilestones )
          {
            if ( itemCount.Item == stMilestoneId
              && itemCount.Count < inCount )
            {
              itemCount.Count = inCount;
            }
          }//END foreach loop. 

        }//END table row interation loop.

      }//END using statement.

      // 
      // Return a list containing an EvItemCount object.
      // 
      return CountOfMilestones;

    } //END getUnScheduledMilestoneCount method.

    #endregion

    #region Object Retrieval methods

    // =====================================================================================
    /// <summary>
    /// This method retrieves the Milestone data table based on visit's Guid
    /// </summary>
    /// <param name="VisitGuid">Guid: a visit's global unique identifier</param>
    /// <returns>EvMilestone: A milestone data object.</returns>
    /// <remarks>
    /// This method consists of following steps: 
    /// 
    /// 1. Return an empty Milestone object, if the Visit's Guid is empty.
    /// 
    /// 2. Define the sql query parameters and sql query string. 
    /// 
    /// 3. Execute sql query string and store results on datatable. 
    /// 
    /// 4. Return an empty Milestone object, if datatable has no value. 
    /// 
    /// 5. Else, extract the fist data row to Milestone object. 
    /// 
    /// 6. Return the Milestone data object. 
    /// 
    /// </remarks>
    //  ------------------------------------------------------------------------------------
    public EvMilestone getMilestone ( Guid VisitGuid )
    {
      this.LogMethod ( "getMilestone method " );
      this.LogValue ( "VisitGuid: " + VisitGuid );
      // 
      // Define the local variables and objects.
      // 
      EvMilestone milestone = new EvMilestone ( );
      String sqlQueryString = String.Empty;

      // 
      // Check that there is a valid unique identifier.
      // 
      if ( VisitGuid == Guid.Empty )
      {
        return milestone;
      }

      // 
      // Define the query parameters.
      // 
      SqlParameter cmdParms = new SqlParameter ( PARM_Guid, SqlDbType.UniqueIdentifier );
      cmdParms.Value = VisitGuid;

      // 
      // Generate the query string.
      // 
      sqlQueryString = _sqlQuery_View + " WHERE (SM_Guid = @Guid ); ";

      this.LogValue ( "Query: " + sqlQueryString );

      // 
      // Execute the query against the database.
      // 
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
      {
        // 
        // If no rows found, return an object containing an EvMilestone object.
        // 
        if ( table.Rows.Count == 0 )
        {
          return milestone;
        }

        // 
        // Extract the table row.
        // 
        DataRow row = table.Rows [ 0 ];

        milestone = readRow ( row );

        //
        // If the earliest start date is null it has not been set.
        //
        if ( milestone.Data.PreviousVisitDate == Evado.Model.Digital.EvcStatics.CONST_DATE_NULL )
        {
          this.getPreviousVisitDate ( milestone );
        }

        // 
        // Retrieve the activities for the Milestone.
        // 
        this.getActivities ( milestone );

      }//END Using method

      this.LogValue ( "DAL Retrieved Guid: " + milestone.Guid
       + " ProjectId: " + milestone.ProjectId
       + " Activity count: " + milestone.ActivityList.Count );

      /*
      foreach ( EvActivity activity in milestone.ActivityList )
      {
        this.LogValue ( "Actvity: " + activity.ActivityId
          + " Guid: " + activity.Guid
          + " Status: " + activity.Status );
      }
      */
      // 
      // Return an object containing an EvMilestone object.
      // 
      return milestone;

    }//END getItem method.

    // =====================================================================================
    /// <summary>
    /// This method retrieves a Milestone data table based on VisitId. 
    /// </summary>
    /// <param name="VisitId">String: a Visit's identifier</param>
    /// <param name="IncludeActivities">True = Include Milestone Activities</param>
    /// <returns>EvMilestone: a Milestone data object.</returns>
    /// <remarks>
    /// This method consists of following steps: 
    /// 
    /// 1. Return an empty Milestone object, if the VisitId is empty.
    /// 
    /// 2. Define the sql query parameters and sql query string. 
    /// 
    /// 3. Execute sql query string and store results on datatable. 
    /// 
    /// 4. Return an empty Milestone object, if datatable has no value. 
    /// 
    /// 5. Else, extract the fist data row to Milestone object. 
    /// 
    /// 6. Return the Milestone data object. 
    /// 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvMilestone getItem ( String VisitId, bool IncludeActivities )
    {

      //
      // Appends the debuglog string to the debug log for the class and adds
      // a new line at the end of the text.
      //
      this.LogMethod ( "getItem method." );
      this.LogValue ( " VisitId: " + VisitId );
      this.LogDebug ( "IncludeActivities: " + IncludeActivities );
      // 
      // Define the local variables and objects.
      // 
      EvMilestone milestone = new EvMilestone ( );
      String sqlQueryString = String.Empty;

      // 
      // If VisitId does not exist, return an object containing an EvMilestone object.
      // 
      if ( VisitId == String.Empty )
      {
        return milestone;
      }

      // 
      // Define the parameters for the query
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter(PARM_VisitId, SqlDbType.NVarChar, 20),
      };
      cmdParms [ 0 ].Value = VisitId;

      sqlQueryString = _sqlQuery_View + " WHERE (VisitID = @VisitId);";

      //
      // Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
      {

        // 
        // If no rows found, return an object containing an EvMilestone object.
        // 
        if ( table.Rows.Count == 0 )
        {
          return milestone;
        }

        // 
        // Extract the table row.
        // 
        DataRow row = table.Rows [ 0 ];

        milestone = readRow ( row );

      }//END Using method

      //
      // Include activities if set to true.
      //
      if ( IncludeActivities == true )
      {
        //
        // If the earliest start date is null it has not been set.
        //
        if ( milestone.Data.PreviousVisitDate == Evado.Model.Digital.EvcStatics.CONST_DATE_NULL )
        {
          this.getPreviousVisitDate ( milestone );
        }

        // 
        // Retrieve the activities for the Milestone.
        // 
        this.getActivities ( milestone );
      }

      this.LogValue ( " milestone Guid: " + milestone.Guid
       + " ProjectId: " + milestone.ProjectId
       + " Activity count: " + milestone.ActivityList.Count );

      // 
      // Return an object containing an EvMilestone object.
      // 
      return milestone;

    }//END getItem method

    // =====================================================================================
    /// <summary>
    ///  This method retrieves a Milestone data table based on VisitId, SubjectId and MilestoneId
    /// </summary>
    /// <param name="ProjectId">String: A Project identifier</param>
    /// <param name="SubjectId">String: A Subject identifier</param>
    /// <param name="MilestoneId">String: A Milestone identifier</param>
    /// <returns>EvMilestone: A Milestone data object</returns>
    /// <remarks>
    /// This method consists of following steps: 
    /// 
    /// 1. Return an empty Milestone object, if the VisitId or SubjectId or MilestoneId is empty.
    /// 
    /// 2. Define the sql query parameters and sql query string. 
    /// 
    /// 3. Execute sql query string and store results on datatable. 
    /// 
    /// 4. Return an empty Milestone object, if datatable has no value. 
    /// 
    /// 5. Else, extract the fist data row to Milestone object. 
    /// 
    /// 6. Return the Milestone data object. 
    /// 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvMilestone getItem (
      String ProjectId,
      String SubjectId,
      String MilestoneId )
    {

      //
      // Appends the debuglog string to the debug log for the class and adds
      // a new line at the end of the text.
      //
      this.LogMethod ( "getItem method. " );
      this.LogValue ( "ProjectId: " + ProjectId );
      this.LogValue ( "SubjectId: " + SubjectId );
      this.LogValue ( "MilestoneId: " + MilestoneId );

      // 
      // Define the local variables.
      // 
      EvMilestone milestone = new EvMilestone ( );
      String sqlQueryString = String.Empty;

      // 
      // If a Project or a Subject or a Milestone does not exist, return an object containing
      // EvMilestone object.
      // 
      if ( ProjectId == String.Empty
        || SubjectId == String.Empty
        || MilestoneId == String.Empty )
      {
        return milestone;
      }

      // 
      // Define the parameters for the query.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter( PARM_TrialId, SqlDbType.NVarChar, 10),
        new SqlParameter( PARM_SubjectId, SqlDbType.NVarChar, 20),
        new SqlParameter( PARM_MilestoneId, SqlDbType.NVarChar, 20),
      };
      cmdParms [ 0 ].Value = ProjectId;
      cmdParms [ 1 ].Value = SubjectId;
      cmdParms [ 2 ].Value = MilestoneId;

      sqlQueryString = _sqlQuery_View + " WHERE (TrialId = @TrialId) "
        + "AND (SubjectId = @SubjectId) "
        + "AND  (MilestoneId = @MilestoneId);";

      //
      // Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
      {
        // 
        // If no rows found, return an object containing an EvMilestone object.
        // 
        if ( table.Rows.Count == 0 )
        {
          return milestone;
        }

        // 
        // Extract the table row.
        // 
        DataRow row = table.Rows [ 0 ];

        milestone = readRow ( row );

        //
        // If the earliest start date is null it has not been set.
        //
        if ( milestone.Data.PreviousVisitDate == Evado.Model.Digital.EvcStatics.CONST_DATE_NULL )
        {
          this.getPreviousVisitDate ( milestone );
        }

        // 
        // Retrieve the activities for the Milestone.
        // 
        this.getActivities ( milestone );

      }//END Using method

      this.LogValue ( " Retrieved Guid: " + milestone.Guid
       + " ProjectId: " + milestone.ProjectId
       + " Activity count: " + milestone.ActivityList.Count );

      // 
      // Return an object containing an EvMilestone object.
      // 
      return milestone;

    }//END getItem method.
    // =====================================================================================
    /// <summary>
    /// This method retrieves the Milestone data table based on visit's Guid
    /// </summary>
    /// <param name="VisitGuid">Guid: a visit's global unique identifier</param>
    /// <returns>EvMilestone: A milestone data object.</returns>
    /// <remarks>
    /// This method consists of following steps: 
    /// 
    /// 1. Return an empty Milestone object, if the Visit's Guid is empty.
    /// 
    /// 2. Define the sql query parameters and sql query string. 
    /// 
    /// 3. Execute sql query string and store results on datatable. 
    /// 
    /// 4. Return an empty Milestone object, if datatable has no value. 
    /// 
    /// 5. Else, extract the fist data row to Milestone object. 
    /// 
    /// 6. Return the Milestone data object. 
    /// 
    /// </remarks>
    //  ------------------------------------------------------------------------------------
    public EvMilestone getDuplicateMilestone ( Guid VisitGuid )
    {
      this.LogMethod ( "getDuplicateMilestone method " );
      this.LogValue ( "VisitGuid: " + VisitGuid );
      // 
      // Define the local variables and objects.
      // 
      EvMilestone milestone = new EvMilestone ( );
      String sqlQueryString = String.Empty;

      // 
      // Check that there is a valid unique identifier.
      // 
      if ( VisitGuid == Guid.Empty )
      {
        return milestone;
      }

      // 
      // Define the query parameters.
      // 
      SqlParameter cmdParms = new SqlParameter ( PARM_Guid, SqlDbType.UniqueIdentifier );
      cmdParms.Value = VisitGuid;

      // 
      // Generate the query string.
      // 
      sqlQueryString = _sqlQuery_View + " WHERE (SM_Guid = @Guid ); ";

      this.LogValue ( "Query: " + sqlQueryString );

      // 
      // Execute the query against the database.
      // 
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
      {
        // 
        // If no rows found, return an object containing an EvMilestone object.
        // 
        if ( table.Rows.Count == 0 )
        {
          return milestone;
        }

        // 
        // Extract the table row.
        // 
        DataRow row = table.Rows [ 0 ];

        milestone = readRow ( row );

      }//END Using method

      // 
      // Return an object containing an EvMilestone object.
      // 
      return milestone;

    }//END getItem method.

    // =====================================================================================
    /// <summary>
    /// This method gets the previous visit date.
    /// </summary>
    /// <param name="SubjectMilestone">EvMilestone:  current milestone milestone.</param>
    ///<returns>DateTime: of the previous visit date.</returns> 
    ///<remarks>
    /// This method conists of following steps.
    /// 
    /// 1. Get a list of milestone Milestones. 
    /// 
    /// 2. Iterate through the list of Milestone list. 
    /// 
    /// 3. Searching the pervious visit date. 
    /// 
    /// 4. update milestone milestone data previous visit date. 
    ///</remarks>
    //  ------------------------------------------------------------------------------------
    public void getPreviousVisitDate ( EvMilestone SubjectMilestone )
    {
      this.LogMethod ( "getPreviousVisitDate Method " );
      //
      // Initialise the methods variables and objects.
      //
      DateTime previousVisitDate = Evado.Model.Digital.EvcStatics.CONST_DATE_NULL;
      bool bVisitFound = false;
      string sqlQueryString = String.Empty;

      // 
      // Define the query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter( PARM_TrialId, SqlDbType.NVarChar, 10),
        new SqlParameter( PARM_SubjectId, SqlDbType.NVarChar, 20),
      };
      cmdParms [ 0 ].Value = SubjectMilestone.ProjectId;
      cmdParms [ 1 ].Value = SubjectMilestone.SubjectId;

      //
      // Generate the SQL query string.   
      //
      sqlQueryString = _sqlQuery_View + " WHERE ( " + DB_TrialId + " = " + PARM_TrialId + " )  \r\n"
        +" AND ( " + DB_SubjectId + " = " + PARM_SubjectId + " )  \r\n"
        + " AND ( " + DB_State +"  = '" + EvMilestone.MilestoneStates.Attended + "' "
        + " OR " + DB_State + " = '" + EvMilestone.MilestoneStates.Completed + "' "
        + " OR " + DB_State + " = '" + EvMilestone.MilestoneStates.Monitored + "' "
        + " OR " + DB_State + " = '" + EvMilestone.MilestoneStates.Issues_Resolved + "' ) "
        +" ORDER BY " + DB_ScheduleDate;

      this.LogValue ( sqlQueryString );

      //
      // Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
      {
        this.LogValue ( "table.Rows.Count: " + table.Rows.Count );
        // 
        // Iterate through the table rows count information.
        // 
        for ( int Count = 0; Count < table.Rows.Count; Count++ )
        {
          // 
          // Extract the table row.
          // 
          DataRow row = table.Rows [ Count ];

          String visitId = EvSqlMethods.getString ( row, EvSubjectMilestones.DB_VisitId );
          DateTime scheduleDate = EvSqlMethods.getDateTime ( row, EvSubjectMilestones.DB_ScheduleDate );
          DateTime startDate = EvSqlMethods.getDateTime ( row, EvSubjectMilestones.DB_StartDate );

          //
          // set the start to the schedule data if it empty.
          //
          if ( startDate == Evado.Model.Digital.EvcStatics.CONST_DATE_NULL )
          {
            startDate = scheduleDate;
          }
          this.LogValue ( "VisitId : " + visitId + " startDate: " + startDate.ToString ( "dd-MMM-yy" ) );

          //
          // If the milestone is found se the visit found to true
          // this will stop updating the visit schedule.
          //
          if ( visitId == SubjectMilestone.VisitId )
          {
            bVisitFound = true;
          }

          //
          // If the visit has not been found then update the 
          // validation rule objects.
          //
          if ( bVisitFound == false )
          {
            previousVisitDate = startDate;
          }

        }//END for loop

      }//END using

      this.LogValue ( "previousVisitDate: " + previousVisitDate.ToString ( "dd-MMM-yy" ) );

      SubjectMilestone.Data.PreviousVisitDate = previousVisitDate;

    }//END getPreviousVisitDate method.

    // =====================================================================================
    /// <summary>
    /// This method gets activities and returns an enumerated value.
    /// </summary>
    /// <param name="SubjectMilestone">Object: An object EvMilestone.</param>
    ///<returns> An enumerated value EventCode status.</returns> 
    ///<remarks>
    /// This method conists of following steps.
    /// 
    /// 1. Get a list of Milestone activities and a list of Subject Milestone activities. 
    /// 
    /// 2. Loop through the list of Milestone's activities. 
    /// 
    /// 3. Retrieve the Subject Activity Object using the list of Subject Milestone activities. 
    /// 
    /// 4. If the Subject Activity Object is not null, add its values to the Activity Object.
    /// 
    /// 5. Return an event code for getting Activity object's values. 
    ///</remarks>
    //  ------------------------------------------------------------------------------------
    public EvEventCodes getActivities ( EvMilestone SubjectMilestone )
    {
      this.LogMethod ( "getActivities Method " );
      this.LogValue ( "SubjectMilestone.Guid: " + SubjectMilestone.Guid );

      // 
      // Initialise the method variables and objects.
      // 
      EvActivity subjectActivity = new EvActivity ( );
      List<EvActivity> subjectMilestoneActivityList = new List<EvActivity> ( );
      EvMilestoneActivities milestoneActivitites = new EvMilestoneActivities ( );
      EvSubjectMilestoneActivities subjectMilestoneActivities = new EvSubjectMilestoneActivities ( );

      //
      // Retrieve the list of the milestone's activities
      //
      SubjectMilestone.ActivityList = milestoneActivitites.getActivityList ( SubjectMilestone.MilestoneGuid,
        EvActivity.ActivityTypes.Null,
        false,
        true );

      this.LogClass ( milestoneActivitites.DebugLog );

      this.LogValue ( "Milestone Activities count: "
       + SubjectMilestone.ActivityList.Count );

      //
      // Retrieve the milestone milestone's activities list.
      //
      subjectMilestoneActivityList = subjectMilestoneActivities.getView (
        SubjectMilestone.Guid,
        EvActivity.ActivitySelection.Null );

      this.LogValue ( "Subject Milestone Activities: "
       + subjectMilestoneActivities.Log );

      this.LogValue ( "Subject Milestone Activity List count: "
       + subjectMilestoneActivityList.Count );

      //
      // Iterate through the milestone milestion activities updating the 
      // list with milestone milestone activities.
      //
      for ( int count = 0; count < SubjectMilestone.ActivityList.Count; count++ )
      {
        EvActivity activity = SubjectMilestone.ActivityList [ count ];

        //
        // Reset the activity properties.
        //
        activity.Guid = Guid.Empty;
        activity.Action = EvActivity.ActivitiesActionsCodes.Save;
        activity.Status = EvActivity.ActivityStates.Created;

        //
        // Retrieve the list of milestone milestone activities.
        //
        subjectActivity = this.getSubjectActivities (
          subjectMilestoneActivityList,
          activity );

        //
        // If the milestone milestone activity is not null 
        // replace the the activity object.
        //
        if ( subjectActivity != null )
        {
          subjectActivity.FormList = activity.FormList;

          SubjectMilestone.ActivityList [ count ] = subjectActivity;
        }

        this.LogValue ( "Actvity: " + SubjectMilestone.ActivityList [ count ].ActivityId
          + " Guid: " + SubjectMilestone.ActivityList [ count ].Guid
          + " Status: " + SubjectMilestone.ActivityList [ count ].Status );

      }//END milestone milestone activity iteration loop.


      this.LogValue ( "Subject Milestone Activity List count: "
       + SubjectMilestone.ActivityList.Count );

      this.LogMethodEnd ( "getActivities" );
      // 
      // Return an enumerated value EventCode status.
      // 
      return EvEventCodes.Ok;

    }//END getActivities method

    // =====================================================================================
    /// <summary>
    /// This method retrieves an Activity data table based on Activities list and Activity object. 
    /// </summary>
    /// <param name="SubjectMilestoneList">List of EvActivity: A list of Activities.</param>
    /// <param name="Activity">EvActivity: An Activity Object</param>
    /// <returns>EvActivity: An Activity data object.</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Return the matching Activity object, if ActivityId and IsMandatory are found in both Activity object 
    /// and the list of Activities. 
    ///    
    /// 2. Else, return null. 
    ///  
    /// </remarks>
    //  ------------------------------------------------------------------------------------
    private EvActivity getSubjectActivities (
      List<EvActivity> SubjectMilestoneList,
      EvActivity Activity )
    {
      //
      // Iterate through the milestone milestone list to find matching 
      // activity and return an activity.
      //
      foreach ( EvActivity activity in SubjectMilestoneList )
      {
        if ( activity.ActivityId == Activity.ActivityId
          && activity.IsMandatory == Activity.IsMandatory )
        {
          return activity;
        }
      }//END Foreach loop.

      //
      // If none is found return null.
      //
      return null;
    }//END getSubjectActivities method

    #endregion

    #region Update  methods
    // =====================================================================================
    /// <summary>
    /// This method adds and retrieves the Milestone data object. 
    /// </summary>
    /// <param name="Milestone">EvMilestone: A selected Milestone data object.</param>
    /// <returns>EvMilestone: A Milestone data object.</returns>
    /// <remarks>
    /// This method consists of following steps: 
    /// 
    /// 1. Create new DB row Guid. 
    /// 
    /// 2. Define the sql query parameters and execute storeprocedure for adding items. 
    /// 
    /// 3. Return an empty Milestone data object, if the storeprocedure runs fail. 
    /// 
    /// 4. Else, retrieve Milestone table using the selected Milestone's Guid
    /// 
    /// 5. Return an empty Milestone object, if the retrieving result has no value. 
    /// 
    /// 6. Else, return the retrieved Milestone object. 
    /// 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvMilestone createItem (
      EvMilestone Milestone )
    {
      this.LogMethod ( "createItem method. " );
      this.LogValue ( "MilestoneId: " + Milestone.MilestoneId );
      this.LogValue ( "ProjectId: " + Milestone.ProjectId );
      this.LogValue ( "SubjectId: " + Milestone.SubjectId );
      this.LogValue ( "OrgId: " + Milestone.OrgId );
      // 
      // Initialise the methods variables and objects.
      // 
      EvMilestone newMilestone = new EvMilestone ( );

      // 
      // Create the Milestone's guid identifier.
      // 
      Milestone.Guid = Guid.NewGuid ( );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] _cmdParms = GetParameters ( );
      SetParameters ( _cmdParms, Milestone );

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( _storedProcedureAddItem, _cmdParms ) == 0 )
      {
        newMilestone.EventCode = EvEventCodes.Database_Query_Error;

        return newMilestone;
      }

      //
      // Retrieve the new milestone from the database.
      //
      newMilestone = getMilestone ( Milestone.Guid );

      //
      // Validate that the visit was retrieved.
      //
      if ( newMilestone.Guid == Guid.Empty )
      {
        this.LogValue ( " Visit Guid is empty." );

        newMilestone.EventCode = EvEventCodes.Identifier_Global_Unique_Identifier_Error;
      }

      this.LogValue ( " END Evado.Dal.Clinical.EvSubjectMilestones.createItem method comp." );

      //
      // Return an object containing an EvMilestone object.
      //
      return newMilestone;

    } //END createItem method.

    // =====================================================================================
    /// <summary>
    /// This method adds items to Milestone data table. 
    /// </summary>
    /// <param name="Milestone">EvMilestone: A selected Milestone object</param>
    /// <returns>EvEventCodes: An event code for adding items.</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Create new DB row Milestone's Guid. 
    /// 
    /// 2. Define sql query parameters and execute storeprocedure for adding items to Milestone table. 
    /// 
    /// 3. Exit, if the storeprocedure runs fail. 
    /// 
    /// 4. Update the items to the related Milestone's Activity table.
    /// 
    /// 5. Return an error code, if the updating runs fail. 
    /// 
    /// 6. Else, return an event code for adding items. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvEventCodes addItem ( EvMilestone Milestone )
    {

      //
      // Appends the debuglog string to the debug log for the class and adds
      // a new line at the end of the text.
      //
      this.LogMethod ( "addItem method " );
      this.LogValue ( "MilestoneId: " + Milestone.MilestoneId );
      this.LogValue ( "ProjectId: " + Milestone.ProjectId );
      this.LogValue ( "ubjectId: " + Milestone.SubjectId );
      this.LogValue ( "OrgId: " + Milestone.OrgId );
      this.LogValue ( "stScheduleDate: " + Milestone.stScheduleDate );
      this.LogValue ( "stStartDate: " + Milestone.stStartDate );

      // 
      // Initialise the methods variables and objects.
      // 
      EvSubjectMilestoneActivities milestoneActivitites = new EvSubjectMilestoneActivities ( );
      EvEventCodes iReturn = EvEventCodes.Ok;

      // 
      // Create the Milestone's guid identifier.
      // 
      Milestone.Guid = Guid.NewGuid ( );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] _cmdParms = GetParameters ( );
      SetParameters ( _cmdParms, Milestone );

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( _storedProcedureAddItem, _cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      // 
      // Update the milestone Milestone activities.
      // 
      iReturn = milestoneActivitites.updateActivities ( Milestone );

      if ( iReturn != EvEventCodes.Ok )
      {
        this.LogValue ( milestoneActivitites.Log );
        this.LogValue ( "Event message: " + Evado.Model.Digital.EvcStatics.Enumerations.enumValueToString ( iReturn ) );

        return iReturn;
      }
      this.LogValue ( " END Evado.Dal.Clinical.EvSubjectMilestones.addItem method comp." );

      //
      // Return an enumerated value EventCode status.
      //
      return EvEventCodes.Ok;

    } //END addItem method.

    // =====================================================================================
    /// <summary>
    /// This method updates the items to the Milestone table. 
    /// </summary>
    /// <param name="Milestone">EvMilestone: A selected Milestone object</param>
    /// <returns>EvEventCodes: An event code for updating items.</returns>
    /// <remarks>
    /// This method consists of following steps:
    /// 
    /// 1. Exit, if the Old Milestone object's Guid is empty. 
    /// 
    /// 2. Add items to datachange object, if they do not exist. 
    /// 
    /// 3. Update items to Milestone Activity table. 
    /// 
    /// 4. Exit, if the updating runs fail. 
    /// 
    /// 5. Define the sql query parameters and execute the storeprocedure for updating items to Milestone table. 
    /// 
    /// 6. Exit, if the storeprocedure runs fail. 
    /// 
    /// 7. Add datachange object's values to the backup datachanges object.  
    /// 
    /// 8. Return an event code for updating items. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvEventCodes updateItem ( EvMilestone Milestone )
    {

      //
      // Appends the debuglog string to the debug log for the class and adds
      // a new line at the end of the text.
      //
      this.LogMethod ( "updateItem method. " );
      this.LogValue ( "Guid: " + Milestone.Guid );
      this.LogValue ( "ProjectId: " + Milestone.ProjectId );
      this.LogValue ( "SubjectId: " + Milestone.SubjectId );
      this.LogValue ( "MilestoneId: " + Milestone.MilestoneId );
      this.LogValue ( "VisitId: " + Milestone.VisitId );
      this.LogValue ( "OrgId: " + Milestone.OrgId );
      this.LogValue ( "CommentList.Count: " + Milestone.CommentList.Count );
      this.LogValue ( "Activities.Count: " + Milestone.ActivityList.Count + "\r\n" );

      // 
      // Initialise the method variables and objects.
      // 
      EvSubjectMilestoneActivities milestoneActivitites = new EvSubjectMilestoneActivities ( );
      EvEventCodes iReturn = EvEventCodes.Ok;
      EvDataChanges dataChanges = new EvDataChanges ( );

      // 
      // Retrieve the existing Milestone to verify it exists.
      // 
      EvMilestone oldMilestone = this.getDuplicateMilestone ( Milestone.Guid );
      if ( oldMilestone.Guid == Guid.Empty )
      {
        this.LogValue ( " >>> oldMilestone.Guid " + oldMilestone.Guid );
        return EvEventCodes.Data_InvalidId_Error;
      }

      // 
      // Fill the data change object
      // 
      EvDataChange dataChange = SetChangeRecord ( Milestone );

      // 
      // Update the milestone Milestone activities.
      // 
      iReturn = milestoneActivitites.updateActivities ( Milestone );
      this.LogValue ( milestoneActivitites.Log );

      if ( iReturn < EvEventCodes.Ok )
      {
        this.LogValue ( "Event message: " + Evado.Model.Digital.EvcStatics.Enumerations.enumValueToString ( iReturn ) );

        return iReturn;
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] _cmdParms = GetParameters ( );
      SetParameters ( _cmdParms, Milestone );

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( _storedProcedureUpdateItem, _cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      // 
      // Add the data changes to the database.
      // 
      dataChanges.AddItem ( dataChange );

      // 
      // Return an enumerated value EventCode status.
      // 
      return EvEventCodes.Ok;

    }//END updateItem method.

    // =====================================================================================
    /// <summary>
    ///  This method sets the data change object for the update action.
    /// </summary>
    /// <param name="Milestone">EvMilestone: A selected Milestone object</param>
    /// <returns>EvDataChange: A Data Change object.</returns>
    /// <remarks>
    /// This method consists of following steps:
    /// 
    /// 1. Add items to datachange object, if they do not exist on Old Milestone object. 
    /// 
    /// 2. Return the datachange object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvDataChange SetChangeRecord ( EvMilestone Milestone )
    {
      this.LogMethod ( "SetChangeRecord method " );
      // 
      // Initialise the methods variables and objects.
      // 
      EvDataChange dataChange = new EvDataChange ( );

      // 
      // Retieve the current object.
      // 
      EvMilestone oldMilestone = this.getMilestone ( Milestone.Guid );

      // 
      // Set the data change object values.
      //   
      dataChange.TableName = EvDataChange.DataChangeTableNames.EvSubjectMilestones;
      dataChange.RecordGuid = Milestone.Guid;
      dataChange.TrialId = Milestone.ProjectId;
      dataChange.SubjectId = Milestone.SubjectId;
      dataChange.UserId = Milestone.UpdatedByUserId;
      dataChange.DateStamp = DateTime.Now;

      //
      // Add items to datachange if they exist. 
      //
      if ( Milestone.ScheduleDate != oldMilestone.ScheduleDate )
      {
        dataChange.AddItem ( "ScheduleDate", oldMilestone.ScheduleDate.ToString ( "dd MMM yy HH:mm:ss" ),
          Milestone.ScheduleDate.ToString ( "dd MMM yy HH:mm:ss" ) );
      }

      if ( Milestone.StartDate != oldMilestone.StartDate )
      {
        dataChange.AddItem ( "ScheduleDate", oldMilestone.StartDate.ToString ( "dd MMM yy HH:mm:ss" ),
          Milestone.StartDate.ToString ( "dd MMM yy HH:mm:ss" ) );
      }
      if ( Milestone.FinishDate != oldMilestone.FinishDate )
      {
        dataChange.AddItem ( "FinishDate", oldMilestone.FinishDate.ToString ( "dd MMM yy HH:mm:ss" ),
          Milestone.FinishDate.ToString ( "dd MMM yy HH:mm:ss" ) );
      }

      string oldXmlData =
        Evado.Model.Digital.EvcStatics.SerialiseObject<EvMilestoneData> ( oldMilestone.Data );

      string newXmlData =
        Evado.Model.Digital.EvcStatics.SerialiseObject<EvMilestoneData> ( Milestone.Data );

      if ( oldXmlData != newXmlData )
      {
        dataChange.AddItem ( "XmlData", oldXmlData, newXmlData );
      }
      if ( Milestone.Comments != oldMilestone.Comments )
      {
        dataChange.AddItem ( "Comments", oldMilestone.Comments, Milestone.Comments );
      }

      if ( Milestone.ProtocolViolation != oldMilestone.ProtocolViolation )
      {
        dataChange.AddItem ( "ProtocolViolation", oldMilestone.ProtocolViolation.ToString ( ), Milestone.ProtocolViolation.ToString ( ) );
      }

      if ( Milestone.State != oldMilestone.State )
      {
        dataChange.AddItem ( "State", oldMilestone.State.ToString ( ), Milestone.State.ToString ( ) );
      }

      //
      // Iterate through the comment list.
      //
      for ( int newcount = 0; newcount < Milestone.CommentList.Count; newcount++ )
      {
        EvFormRecordComment comment = Milestone.CommentList [ newcount ];

        if ( newcount < oldMilestone.CommentList.Count )
        {
          EvFormRecordComment oldComment = Milestone.CommentList [ newcount ];

          if ( oldComment.Content != comment.Content )
          {
            dataChange.AddItem ( "Comment_Content",
              oldComment.Content,
              comment.Content );
          }
          if ( oldComment.AuthorType != comment.AuthorType )
          {
            dataChange.AddItem ( "Comment_AuthorType",
              oldComment.AuthorType.ToString ( ),
              comment.AuthorType.ToString ( ) );
          }
          if ( oldComment.CommentDate != comment.CommentDate )
          {
            dataChange.AddItem ( "Comment_CommentDate",
              oldComment.CommentDate.ToString ( ),
              comment.CommentDate.ToString ( ) );
          }
          if ( oldComment.UserCommonName != comment.UserCommonName )
          {
            dataChange.AddItem ( "Comment_UserCommonName",
              oldComment.UserCommonName,
              comment.UserCommonName );
          }
          if ( oldComment.UserId != comment.UserId )
          {
            dataChange.AddItem ( "Comment_UserCommonName",
              oldComment.UserId,
              comment.UserId );
          }
        }//END old comment update.
        else
        {
          dataChange.AddItem ( "Comment_Content",
            String.Empty,
            comment.Content );
          dataChange.AddItem ( "Comment_AuthorType",
            String.Empty,
            comment.AuthorType.ToString ( ) );
          dataChange.AddItem ( "Comment_CommentDate",
            String.Empty,
            comment.CommentDate.ToString ( ) );
          dataChange.AddItem ( "Comment_UserCommonName",
            String.Empty,
            comment.UserCommonName );
          dataChange.AddItem ( "Comment_UserCommonName",
            String.Empty,
            comment.UserId );
        }//END new comment 
      }//END comment list interation.

      foreach ( EvActivity newActivity in Milestone.ActivityList )
      {
        EvActivity oldActivity = getOldActivity ( oldMilestone.ActivityList, newActivity.Guid );
        /*
        if ( newActivity.ScheduleDate != oldActivity.ScheduleDate )
        {
          dataChange.AddItem ( "Activity_ScheduleDate",
            oldActivity.ScheduleDate.ToString ( "dd MMM yyyy HH:mm" ),
            newActivity.ScheduleDate.ToString ( "dd MMM yyyy HH:mm" ) );
        }

        if ( newActivity.ActualQuantity != oldActivity.ActualQuantity )
        {
          dataChange.AddItem ( "Activity_ActualQuantity",
            oldActivity.ActualQuantity.ToString ( ),
            newActivity.ActualQuantity.ToString ( ) );
        }
        */

        if ( newActivity.CompletionDate != oldActivity.CompletionDate )
        {
          dataChange.AddItem ( "Activity_CompletionDate",
            oldActivity.CompletionDate.ToString ( "dd MMM yyyy HH:mm" ),
            newActivity.CompletionDate.ToString ( "dd MMM yyyy HH:mm" ) );
        }

        if ( newActivity.CompletedBy != oldActivity.CompletedBy )
        {
          dataChange.AddItem ( "Activity_CompletedBy",
            oldActivity.CompletedBy,
            newActivity.CompletedBy );
        }

        if ( newActivity.Comments != oldActivity.Comments )
        {
          dataChange.AddItem ( "Activity_Comments",
            oldActivity.Comments,
            newActivity.Comments );
        }

        if ( newActivity.ProtocolViolation != oldActivity.ProtocolViolation )
        {
          dataChange.AddItem ( "Activity_ProtocolViolation",
            oldActivity.ProtocolViolation.ToString ( ),
            newActivity.ProtocolViolation.ToString ( ) );
        }

        if ( newActivity.IsMandatory != oldActivity.IsMandatory )
        {
          dataChange.AddItem ( "Activity_IsMandatory",
            oldActivity.IsMandatory.ToString ( ),
            newActivity.IsMandatory.ToString ( ) );
        }

        if ( newActivity.Status != oldActivity.Status )
        {
          dataChange.AddItem ( "Activity_Status",
            oldActivity.Status.ToString ( ),
            newActivity.Status.ToString ( ) );
        }

      }

      //
      // Return an object containing an EvDataChange object.
      //
      return dataChange;
    }//END SetChangeRecord method

    // =====================================================================================
    /// <summary>
    ///  This method retrieves the old Activity from a given list and Guid.
    /// </summary>
    /// <param name="ActivityGuid">Guid: An Activity's Global Unique Identifier</param>
    /// <param name="OldActivities">List: A list of old activity object.</param>
    /// <returns>EvActivity: An old activity object</returns>
    /// <remarks>
    /// This method consists of following steps.
    /// 
    /// 1. Loop through the list of Old Activity object. 
    /// 
    /// 2. Return a matching Activity object, if the selected Guid exist. 
    /// 
    /// 3. Else, return an empty activity object. 
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private EvActivity getOldActivity ( List<EvActivity> OldActivities, Guid ActivityGuid )
    {
      //
      // Iterate throch old activity. If old activity guid is equal to activity guid, return
      // an object containing an EvActivity object.
      //
      foreach ( EvActivity activity in OldActivities )
      {
        if ( activity.Guid == ActivityGuid )
        {
          return activity;
        }
      }//END iteration loop

      //
      // Else return a blank activity. 
      //
      return new EvActivity ( );
    }//END getOldActivity method

    // =====================================================================================
    /// <summary>
    /// This method deletes items from Milestone data table.
    /// </summary>
    /// <param name="milestone">EvMilestone: A selected Milestone object.</param>
    /// <returns>EvEventCodes: An event code for deleting items.</returns>
    /// <remarks>
    /// This method consists of following steps.
    /// 
    /// 1. Exit, if the VisitId or User common name is not defined. 
    /// 
    /// 2. Define sql query parameters and execute the storeprocedure for deleting items. 
    /// 
    /// 3. Exit, if the storeprocedure runs fail. 
    /// 
    /// 4. Else, return an event code for deleting items. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private EvEventCodes deleteItem ( EvMilestone milestone )
    {

      //
      // Appends the debuglog string to the debug log for the class and adds
      // a new line at the end of the text.
      //
      this.LogMethod ( "deleteItem method. " );
      this.LogValue ( "VisitId: " + milestone.VisitId );

      // 
      // Validate whether the VisitId or UserCommonName is defined. 
      // 
      if ( milestone.ProjectId.Length == 0 )
      {
        return EvEventCodes.Identifier_Project_Id_Error;
      }

      if ( milestone.UserCommonName.Length == 0 )
      {
        return EvEventCodes.Identifier_User_Id_Error;
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] _cmdParms = new SqlParameter [ ]
      {
        new SqlParameter(PARM_Guid, SqlDbType.UniqueIdentifier),
        new SqlParameter(PARM_UpdatedByUserId, SqlDbType.NVarChar,100),
        new SqlParameter(PARM_UpdatedBy, SqlDbType.NVarChar,30),
        new SqlParameter(PARM_UpdateDate, SqlDbType.DateTime)
      };
      _cmdParms [ 0 ].Value = milestone.Guid;
      _cmdParms [ 1 ].Value = milestone.UpdatedByUserId;
      _cmdParms [ 2 ].Value = milestone.UserCommonName;
      _cmdParms [ 3 ].Value = DateTime.Now;

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( _storedProcedureDeleteItem, _cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      //
      // Return an enumerated value EventCode status.
      //
      return EvEventCodes.Ok;

    }//END deleteItem method

    #endregion

    #region Lock Records methods

    // =====================================================================================
    /// <summary>
    /// This method locks the Milestone object for single user update.
    /// </summary>
    /// <param name="SubjectMilestone">EvMilestone:A selected Milestone object.</param> 
    /// <returns>EvEventCodes: An event code for locking items.</returns>
    /// <remarks>
    /// This method consists of following steps:
    /// 
    /// 1. Exit, if the Milestone Object's Uid is not defined. 
    /// 
    /// 2. Define the sql query parameters and execute the storeprocedure for locking items. 
    /// 
    /// 3. Exit, if the storeprocedure runs fail. 
    /// 
    /// 4. Return an event code for locking items. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvEventCodes LockItem ( EvMilestone SubjectMilestone )
    {
      // 
      // Initialise the method variables
      // 
      this.LogMethod ( "lockItem method " );
      this.LogValue ( "Guid: " + SubjectMilestone.Guid );
      this.LogValue ( "UserCommonName: " + SubjectMilestone.UserCommonName );
      int RecordsUpdated = 0;

      // 
      // Check that the data object has valid identifiers to add it to the database.
      // 
      if ( SubjectMilestone.Guid == Guid.Empty )
      {
        return EvEventCodes.Identifier_Global_Unique_Identifier_Error;
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter(PARM_Guid, SqlDbType.UniqueIdentifier),
        new SqlParameter(PARM_UpdatedByUserId, SqlDbType.NVarChar, 100),
        new SqlParameter(PARM_UpdatedBy, SqlDbType.NVarChar, 100),
        new SqlParameter(PARM_UpdateDate, SqlDbType.DateTime)
      };
      cmdParms [ 0 ].Value = SubjectMilestone.Guid;
      cmdParms [ 1 ].Value = SubjectMilestone.UpdatedByUserId;
      cmdParms [ 2 ].Value = SubjectMilestone.UserCommonName;
      cmdParms [ 3 ].Value = DateTime.Now;

      //
      // Execute the update command.
      //
      if ( ( EvSqlMethods.StoreProcUpdate ( _storedProcedureLockItem, cmdParms ) ) == 0 )
      {
        this.LogValue ( "RecordsUpdated " + RecordsUpdated + " " );
        return EvEventCodes.Database_Record_Lock_Error;
      }

      this.LogValue ( "RecordsUpdated " + RecordsUpdated + " " );
      // 
      // Return an enumerated value EventCode status.
      // 
      return EvEventCodes.Ok;

    }//END LockItem method.

    // =====================================================================================
    /// <summary>
    /// This method unlocks the Milestone Object items for other users to update. 
    /// </summary>
    /// <param name="SubjectMilestone">EvMilestone: A Milestone data object.</param>
    /// <returns>EvEventCodes: An event code for unlocking items.</returns>
    /// <remarks>
    /// This method consists of following steps:
    /// 
    /// 1. Exit, if the Milestone object's Uid is not defined. 
    /// 
    /// 2. Define the sql query parameters and execute the storeprocedure for unlocking items. 
    /// 
    /// 3. Exit, if the storeprocedure runs fail. 
    /// 
    /// 4. Else, return an event code for unlocking items. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvEventCodes UnlockItem ( EvMilestone SubjectMilestone )
    {
      // 
      // Initialise the method variables
      // 
      this.LogMethod ( "unlockItem method " );
      this.LogValue ( "Guid: " + SubjectMilestone.Guid );
      this.LogValue ( "UserCommonName: " + SubjectMilestone.UserCommonName );
      EvEventCodes RecordsUpdated = EvEventCodes.Ok;

      // 
      // Check that the data object has valid identifiers to add it to the database.
      // 
      if ( SubjectMilestone.Guid == Guid.Empty )
      {
        return EvEventCodes.Identifier_Global_Unique_Identifier_Error;
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter(PARM_Guid, SqlDbType.UniqueIdentifier),
        new SqlParameter(PARM_UpdatedByUserId, SqlDbType.NVarChar, 100),
        new SqlParameter(PARM_UpdatedBy, SqlDbType.NVarChar, 100),
        new SqlParameter(PARM_UpdateDate, SqlDbType.DateTime),
      };
      cmdParms [ 0 ].Value = SubjectMilestone.Guid;
      cmdParms [ 1 ].Value = SubjectMilestone.UpdatedByUserId;
      cmdParms [ 2 ].Value = SubjectMilestone.UserCommonName;
      cmdParms [ 3 ].Value = DateTime.Now;

      //
      // Execute the update command.
      //
      if ( ( EvSqlMethods.StoreProcUpdate ( _storedProcedureUnlockItem, cmdParms ) ) == 0 )
      {
        this.LogValue ( "RecordsUpdated " + RecordsUpdated + " " );
        return EvEventCodes.Database_Record_UnLock_Error;
      }

      this.LogValue ( "RecordsUpdated " + RecordsUpdated + " " );

      // 
      // Return an enumerated value EventCode status.
      // 
      return EvEventCodes.Ok;

    }//END unlockItem method.

    #endregion

  }//END EvSubjectMilestones class

}//END namespace Evado.Dal.Clinical
