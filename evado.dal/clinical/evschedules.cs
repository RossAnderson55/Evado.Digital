/***************************************************************************************
 * <copyright file="dal\EvSchedules.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2020 EVADO HOLDING PTY. LTD.  All rights reserved.
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
using System.Xml.Serialization;
using System.IO;

//References to Evado specific libraries

using Evado.Model;
using Evado.Model.Digital;


namespace Evado.Dal.Clinical
{
  //=================================================================================
  /// <summary>
  /// A business Component used to manage Schedules roles
  /// The Evado.Model.EvSchedule is used in most methods 
  /// and is used to store serializable information about an account
  /// </summary>
  //----------------------------------------------------------------------------------
  public class EvSchedules : EvDalBase
  {
    #region class initialisation method.
    /// <summary>
    /// This method initialises the schedule DAL class.
    /// </summary>
    public EvSchedules ( )
    {
      this.ClassNameSpace = "Evado.Dal.Clinical.EvSchedules.";
    }

    /// <summary>
    /// This method initialises the schedule DAL class.
    /// </summary>
    public EvSchedules ( EvClassParameters Settings )
    {
      this.ClassParameters = Settings;
      this.ClassNameSpace = "Evado.Dal.Clinical.EvSchedules.";

      this.LogDebug ( "CustomerGuid: " + this.ClassParameters.CustomerGuid );
    }

    #endregion

    #region Class constants and variable initialisation

    private const string _sqlQuery_View = "Select * FROM EvSchedules ";

    // 
    // The SQL Store Procedure constants
    // 
    private const string _storedProcedureAddItem = "usr_Schedule_add";
    private const string _storedProcedureUpdateItem = "usr_Schedule_update";
    private const string _storedProcedureDeleteItem = "usr_Schedule_delete";

    private const string DB_GUID = "SCH_Guid";
    private const string DB_CUSTOMER_GUID = "CU_GUID";
    private const string DB_PROJECT_ID = "TrialId";
    private const string DB_SCHEDULE_ID = "SCHEDULE_ID";
    private const string DB_TITLE = "SCH_TITLE";
    private const string DB_DESCRIPTION = "SCH_Description";
    private const string DB_MILESTONE_PERIOD_INCREMENT = "SCH_MILESTONE_PERIOD_INCREMENT";
    private const string DB_VERSION = "SCH_Version";
    private const string DB_STATE = "SCH_State";
    private const string DB_TYPE = "SCH_TYPE";
    private const string DB_APPROVED_USER_ID = "SCH_ApprovedByUserId";
    private const string DB_APPROVED_BY = "SCH_ApprovedBy";
    private const string DB_APPROVAL_DATE = "SCH_ApprovedDate";
    private const string DB_UPDATED_USER_ID = "SCH_UpdatedByUserId";
    private const string DB_UPDATED_BY = "SCH_UpdatedBy";
    private const string DB_UPDATED_DATE = "SCH_UpdatedDate";
    private const string DB_XML_SIGNOFF = "SCH_XmlSignoffs";
    private const string DB_DELETED = "SCH_Deleted";

    private const string PARM_Guid = "@Guid";
    private const string PARM_CUSTOMER_GUID = "@CUSTOMER_GUID";
    private const string PARM_PROJECT_ID = "@TrialId";
    private const string PARM_SCHEDULE_ID = "@SCHEDULE_ID";
    private const string PARM_TITLE = "@TITLE";
    private const string PARM_Description = "@Description";
    private const string PARM_MILESTONE_PERIOD_INCREMENT = "@MILESTONE_PERIOD_INCREMENT";
    private const string PARM_Version = "@Version";
    private const string PARM_State = "@State";
    private const string PARM_TYPE = "@TYPE";
    private const string PARM_XmlSignoffs = "@XmlSignoffs";
    private const string PARM_ApprovedByUserId = "@ApprovedByUserId";
    private const string PARM_ApprovedBy = "@ApprovedBy";
    private const string PARM_ApprovedDate = "@ApprovedDate";
    private const string PARM_UpdatedByUserId = "@UpdatedByUserId";
    private const string PARM_UpdatedBy = "@UpdatedBy";
    private const string PARM_UpdatedDate = "@UpdatedDate";
    private const string PARM_ScheduleGuid = "@ScheduleGuid";


    #endregion

    #region SQL Parameter methods

    // =====================================================================================
    /// <summary>
    /// This method sets the array with EvSchedule's constants and returns an Array
    /// of SqlParameter type.
    /// </summary>
    /// <returns>SqlParameter: An array of sql query parameters.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Create an array of sql query parameters 
    /// 
    /// 2. Return the array of sql query parameters. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private static SqlParameter [ ] getItemsParameters ( )
    {
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( PARM_Guid, SqlDbType.UniqueIdentifier ),
        new SqlParameter( PARM_CUSTOMER_GUID, SqlDbType.UniqueIdentifier ),
        new SqlParameter( PARM_PROJECT_ID, SqlDbType.NVarChar, 10),
        new SqlParameter( PARM_SCHEDULE_ID, SqlDbType.NVarChar, 10),
        new SqlParameter( PARM_TITLE, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_Description, SqlDbType.NVarChar, 250),
        new SqlParameter( PARM_MILESTONE_PERIOD_INCREMENT, SqlDbType.NVarChar, 30),
        new SqlParameter( PARM_Version, SqlDbType.SmallInt ),
        new SqlParameter( PARM_State, SqlDbType.VarChar, 15),
        new SqlParameter( PARM_TYPE, SqlDbType.NVarChar, 30),
        new SqlParameter( PARM_XmlSignoffs, SqlDbType.NText),
        new SqlParameter( PARM_ApprovedByUserId, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_ApprovedBy, SqlDbType.NVarChar, 30),
        new SqlParameter( PARM_ApprovedDate, SqlDbType.DateTime),
        new SqlParameter( PARM_UpdatedByUserId, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_UpdatedBy, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_UpdatedDate, SqlDbType.DateTime )
      };

      return cmdParms;
    }//END getItemsParameters class

    // =====================================================================================
    /// <summary>
    /// This method assigns the Schedule object values to the array of sql parameters. 
    /// </summary>
    /// <param name="cmdParms">SqlParameter: An array of sql parameters.</param>
    /// <param name="Schedule">EvSchedule: a schedule object.</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Set Schedule's state to draft, if it is null. 
    /// 
    /// 2. Bind the Schedule object's values to the array of sql parameters. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private void SetParameters ( SqlParameter [ ] cmdParms, EvSchedule Schedule )
    {
      // 
      // If the schedule's state is null, set it to draft. 
      // 
      if ( Schedule.State == EvSchedule.ScheduleStates.Null )
      {
        Schedule.State = EvSchedule.ScheduleStates.Draft;
      }

      // 
      // Set the Sql paramter values.
      // 
      cmdParms [ 0 ].Value = Schedule.Guid;
      cmdParms [ 1 ].Value = Schedule.CustomerGuid;
      cmdParms [ 2 ].Value = Schedule.TrialId;
      cmdParms [ 3 ].Value = Schedule.ScheduleId;
      cmdParms [ 4 ].Value = Schedule.Title;
      cmdParms [ 5 ].Value = Schedule.Description;
      cmdParms [ 6 ].Value = Schedule.MilestonePeriodIncrement.ToString ( );
      cmdParms [ 7 ].Value = Schedule.Version;
      cmdParms [ 8 ].Value = Schedule.State.ToString ( );
      cmdParms [ 9 ].Value = Schedule.Type.ToString ( );
      cmdParms [ 10 ].Value = Evado.Model.EvStatics.SerialiseObject<List<EvUserSignoff>> ( Schedule.Signoffs );
      cmdParms [ 11 ].Value = Schedule.ApprovedByUserId;
      cmdParms [ 12 ].Value = Schedule.ApprovedBy;
      cmdParms [ 13 ].Value = Schedule.ApprovedDate;
      cmdParms [ 14 ].Value = Schedule.UpdatedByUserId;
      cmdParms [ 15 ].Value = Schedule.UserCommonName;
      cmdParms [ 16 ].Value = DateTime.Now;

    }//END SetParameters class.

    #endregion

    #region Data Reader methods

    // =====================================================================================
    /// <summary>
    /// This method extracts data row values to the Schedule object.
    /// </summary>
    /// <param name="Row">DataRow: a data row object</param>
    /// <returns>EvSchedule: a schedule object.</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Extract the compatible data row values to the schedule object. 
    /// 
    /// 2. Return the Schedule data object. 
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvSchedule readRow ( DataRow Row )
    {

      EvSchedule schedule = new EvSchedule ( );
      // 
      // Extract the data object values.
      // 
      schedule.Guid = EvSqlMethods.getGuid ( Row, EvSchedules.DB_GUID );
      schedule.CustomerGuid = EvSqlMethods.getGuid ( Row, EvSchedules.DB_CUSTOMER_GUID );
      schedule.TrialId = EvSqlMethods.getString ( Row, EvSchedules.DB_PROJECT_ID );
      schedule.ScheduleId = EvSqlMethods.getInteger ( Row, EvSchedules.DB_SCHEDULE_ID );
      schedule.Title = EvSqlMethods.getString ( Row, EvSchedules.DB_TITLE );
      schedule.Description = EvSqlMethods.getString ( Row, EvSchedules.DB_DESCRIPTION );

      schedule.MilestonePeriodIncrement = EvSchedule.MilestonePeriodIncrements.Days;
      string value = EvSqlMethods.getString ( Row, DB_MILESTONE_PERIOD_INCREMENT );
      if ( value != String.Empty )
      {
        schedule.MilestonePeriodIncrement = Evado.Model.EvStatics.Enumerations.parseEnumValue<EvSchedule.MilestonePeriodIncrements> ( value );
      }

      schedule.Version = EvSqlMethods.getInteger ( Row, DB_VERSION );

      schedule.State = EvSchedule.ScheduleStates.Draft;
      value = EvSqlMethods.getString ( Row, DB_STATE );
      if ( value != String.Empty )
      {
        schedule.State = Evado.Model.EvStatics.Enumerations.parseEnumValue<EvSchedule.ScheduleStates> ( value );
      }
      value = EvSqlMethods.getString ( Row, DB_TYPE );
      if ( value != String.Empty )
      {
        schedule.Type = Evado.Model.EvStatics.Enumerations.parseEnumValue<EvSchedule.ScheduleTypes> ( value );
      }

      // 
      // Get the XmlVsignoff  object and convert array Signoffs
      // 
      string xmlSignoffs = EvSqlMethods.getString ( Row, EvSchedules.DB_XML_SIGNOFF );
      if ( xmlSignoffs != String.Empty )
      {
        schedule.Signoffs =
          Evado.Model.EvStatics.DeserialiseObject<List<EvUserSignoff>> ( xmlSignoffs );
      }

      schedule.ApprovedByUserId = EvSqlMethods.getString ( Row, EvSchedules.DB_APPROVED_USER_ID );
      schedule.ApprovedBy = EvSqlMethods.getString ( Row, EvSchedules.DB_APPROVED_BY );
      schedule.ApprovedDate = EvSqlMethods.getDateTime ( Row, EvSchedules.DB_APPROVAL_DATE );
      schedule.UpdatedByUserId = EvSqlMethods.getString ( Row, EvSchedules.DB_UPDATED_USER_ID );
      schedule.UpdatedBy = EvSqlMethods.getString ( Row, EvSchedules.DB_UPDATED_BY );

      schedule.UpdatedBy += " on " + EvSqlMethods.getDateTime ( Row, EvSchedules.DB_UPDATED_DATE ).ToString ( "dd MMM yyyy HH:mm" );


      if ( schedule.ScheduleId == 0
       || schedule.ScheduleId == Evado.Model.EvStatics.CONST_INTEGER_NULL )
      {
        schedule.ScheduleId = 1;
      }

      if ( schedule.Title == String.Empty )
      {
        schedule.Title = EvLabels.Schedule_Default_Title;
      }

      this.LogValue ( "schedule.ScheduleId: " + schedule.ScheduleId );
      this.LogValue ( "schedule.Title: " + schedule.Title );
      //
      // Return an object containing an EvSchedule data object.
      //
      return schedule;

    }//END readRow method.

    #endregion

    #region Schedules Query methods

    // =====================================================================================
    /// <summary>
    /// This method returns a list of Schedule objects based on VisitId 
    /// </summary>
    /// <param name="TrialId">String: A trial identifier</param>
    /// <param name="ScheduleId">Int: A schedule identifier</param>
    /// <param name="ScheduleState">EvSchedule.ScheduleStates: A schedule state</param>
    /// <returns>List of EvSchedule: a list of Schedule object items.</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Define the SQL query parameters and sql query string
    /// 
    /// 2. Execute the sql query string and store the results on datatable
    /// 
    /// 3. Iterate through the table and extract data row to the Schedule object
    /// 
    /// 4. Add the Schedule object values to the Schedule list. 
    /// 
    /// 5. Return the Schedule list. 
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvSchedule> getScheduleList (
      String TrialId,
      int ScheduleId,
      EvSchedule.ScheduleStates ScheduleState )
    {
      this.LogMethod ( "getScheduleList" );
      this.LogDebug ( "CustomerGuid: " + this.ClassParameters.CustomerGuid );
      this.LogDebug ( "TrialId: " + TrialId );
      this.LogDebug ( "ScheduleId: " + ScheduleId );
      this.LogDebug ( "ScheduleState: " + ScheduleState );

      // 
      // Define the local variables and objects.
      // 
      System.Text.StringBuilder sqlQueryString = new System.Text.StringBuilder ( );
      List<EvSchedule> view = new List<EvSchedule> ( );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter ( EvSchedules.PARM_CUSTOMER_GUID, SqlDbType.UniqueIdentifier),
        new SqlParameter( PARM_PROJECT_ID, SqlDbType.NVarChar, 10),
        new SqlParameter( PARM_SCHEDULE_ID, SqlDbType.Int ),
      };
      cmdParms [ 0 ].Value = this.ClassParameters.CustomerGuid;
      cmdParms [ 1 ].Value = TrialId;
      cmdParms [ 2 ].Value = ScheduleId;

      // 
      // Generate the SQL query string
      // 
      sqlQueryString.AppendLine ( _sqlQuery_View );
      sqlQueryString.AppendLine ( " WHERE ( " + EvSchedules.DB_CUSTOMER_GUID + " = " + EvSchedules.PARM_CUSTOMER_GUID + " ) " );
      sqlQueryString.AppendLine ( "  AND (" + DB_PROJECT_ID + " = " + PARM_PROJECT_ID + ") " );
      sqlQueryString.AppendLine ( "  AND (" + DB_DELETED + " = 0 ) " );

      if ( ScheduleId > 0 )
      {
        sqlQueryString.AppendLine ( " AND (" + DB_SCHEDULE_ID + " = " + PARM_SCHEDULE_ID + ") " );
      }

      switch ( ScheduleState )
      {
        case EvSchedule.ScheduleStates.Draft:
          {
            sqlQueryString.AppendLine ( " AND (" + DB_STATE + " = '" + EvSchedule.ScheduleStates.Draft + "') " );
            break;
          }
        case EvSchedule.ScheduleStates.Reviewed:
          {
            sqlQueryString.AppendLine ( " AND (" + DB_STATE + " = '" + EvSchedule.ScheduleStates.Reviewed + "') " );
            break;
          }
        case EvSchedule.ScheduleStates.Issued:
          {
            sqlQueryString.AppendLine ( " AND (" + DB_STATE + " = '" + EvSchedule.ScheduleStates.Issued + "') " );
            break;
          }
        case EvSchedule.ScheduleStates.Withdrawn:
          {
            sqlQueryString.AppendLine ( " AND (" + DB_STATE + " = '" + EvSchedule.ScheduleStates.Withdrawn + "') " );
            break;
          }
        default:
          {
            sqlQueryString.AppendLine ( " AND NOT (" + DB_STATE + " = '" + EvSchedule.ScheduleStates.Withdrawn + "') " );
            break;
          }
      }//END state switch.

      sqlQueryString.AppendLine ( " ORDER BY " + DB_SCHEDULE_ID + ", " + DB_VERSION + "; " );

      this.LogDebug ( sqlQueryString.ToString ( ) );

      //
      // Execute the query against the database
      //
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString.ToString ( ), cmdParms ) )
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

          EvSchedule schedule = this.readRow ( row );

          view.Add ( schedule );
        }
      }

      this.LogValue ( "Schedule list count: " + view.Count );

      this.LogMethodEnd ( "getScheduleList" );
      // 
      // Return the ArrayList containing the EvSchedule data object.
      // 
      return view;

    }//END getView method.

    // =====================================================================================
    /// <summary>
    /// This method returns a list of Option object for schedule selection. 
    /// </summary>
    /// <param name="TrialId">string: a trial identifier</param>
    /// <param name="IsIssued">Boolean: true, if the Schedule'state is issued</param>
    /// <param name="UseGuid">Boolean: true, user GUID identifiers</param>
    /// <param name="SelectionList">Boolean: true, if the selectionlist exists</param>
    /// <returns>List of EvOption: A list of Option object.</returns>
    /// <remarks>
    /// This method consists of following steps.    
    /// 
    /// 1. Define the SQL query parameters and the sql query string
    /// 
    /// 2. Execute the sql query string and store the results on datatable.
    /// 
    /// 3. Iterate through the table and extract data row to the Option object. 
    /// 
    /// 4. Add Option object values to the option list. 
    /// 
    /// 5. If the option list empty, create new option list. 
    /// 
    /// 6. Return the option list. 
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> getOptionList (
      String TrialId,
      bool IsIssued,
      bool UseGuid,
      bool SelectionList )
    {
      this.LogMethod ( "getOptionList. " );
      this.LogValue ( "TrialId: " + TrialId );

      // 
      // Define the local variables and objects. 
      // 
      List<EvOption> list = new List<EvOption> ( );
      EvOption option = new EvOption ( Guid.Empty.ToString ( ), String.Empty );

      if ( SelectionList == true )
      {
        list.Add ( option );
      }

      var state = EvSchedule.ScheduleStates.Null;
      if ( IsIssued == true )
      {
        state = EvSchedule.ScheduleStates.Issued;
      }

      var scheduleList = this.getScheduleList ( TrialId, -1, state );
      //
      // 
      // Iterate through the table rows count information.
      // 
      foreach ( EvSchedule schedule in scheduleList )
      {
        String stScheduleTitle = schedule.ScheduleId.ToString ( "00" )
         + EvLabels.Space_Hypen
         + schedule.Title
         + EvLabels.Space_Arrow_Right
         + EvLabels.Label_Version
         + schedule.Version.ToString ( "000" );

        if ( UseGuid == true )
        {
          option = new EvOption (
            schedule.Guid.ToString ( ), stScheduleTitle );
        }
        else
        {
          option = new EvOption (
          schedule.ScheduleId, stScheduleTitle );
        }
        list.Add ( option );
      }


      this.LogValue ( "list count: " + list.Count );

      this.LogMethodEnd ( "getOptionList" );
      // 
      // Return a list containing the EvSchedule data object.
      // 
      return list;

    }//END getList method.

    #endregion

    #region Schedules Retrieval methods

    // =====================================================================================
    /// <summary>
    ///  This methods retrieves Schedule data table based on Guid. 
    /// </summary>
    /// <param name="Guid">Guid: a schedule object's global unique identifier</param>
    /// <returns>EvSchedule: a schedule data object.</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Return an empty Schedule object, if the Guid is empty. 
    /// 
    /// 2. Define the SQL query parameters and sql query string
    /// 
    /// 3. Execute the sql query string and store the values on data table. 
    /// 
    /// 4. Return an empty Schedule object, if the table has no value.
    /// 
    /// 5. Else, extract the first row values to the Schedule object. 
    /// 
    /// 6. Update Schedule's milestone value
    /// 
    /// 7. Return the Schedule data object. 
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvSchedule getSchedule ( Guid Guid )
    {
      this.LogMethod ( "getSchedule method. " );
      this.LogValue ( "Guid: " + Guid );
      // 
      // Define local variables and objects. 
      // 
      string _sqlQueryString;
      EvSchedule schedule = new EvSchedule ( );
      EvMilestones milestones = new EvMilestones ( );

      // 
      // If Guid is equal to empty, return an object containing an EvSchedule data object.
      // 
      if ( Guid == Guid.Empty )
      {
        return schedule;
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter ( EvSchedules.PARM_CUSTOMER_GUID, SqlDbType.UniqueIdentifier),
        new SqlParameter( PARM_Guid, SqlDbType.UniqueIdentifier ),
      };
      cmdParms [ 0 ].Value = this.ClassParameters.CustomerGuid;
      cmdParms [ 1 ].Value = Guid;
      // 
      // Generate the SQL query string.
      // 
      _sqlQueryString = _sqlQuery_View + " WHERE (" + DB_GUID + "= " + PARM_Guid + ");";
      this.LogValue ( _sqlQueryString );

      //
      // Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( _sqlQueryString, cmdParms ) )
      {
        // 
        // If not rows the found, Return an object containing an EvSchedule data object.
        // 
        if ( table.Rows.Count == 0 )
        {
          return schedule;
        }

        // 
        // Extract the table row.
        // 
        DataRow row = table.Rows [ 0 ];

        // 
        // Fill the schedule object.
        // 
        schedule = this.readRow ( row );


        // 
        // Get the schedules milestones.
        // 
        schedule.Milestones = milestones.getMilestoneList ( schedule.Guid, EvMilestone.MilestoneTypes.Null );

        this.LogValue ( milestones.Log );

      }//END Using


      this.LogMethodEnd ( "getSchedule" );
      // 
      // Return an object containing an EvSchedule data object.
      // 
      return schedule;

    }//END getSchedule method

    // =====================================================================================
    /// <summary>
    /// This method retrieves the Schedule data object based on VisitId.
    /// </summary>
    /// <param name="TrialId">String: A project identifier.</param>
    /// <param name="ScheduleId">int: A schedule identifier.</param>
    /// <param name="withActivities">bool: include activities.</param>
    /// <returns>EvSchedule: A Schedule data object.</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Return an empty Schedule object, if the VisitId is empty. 
    /// 
    /// 2. Define the SQL query parameters and sql query string
    /// 
    /// 3. Execute the sql query string and store the values on data table. 
    /// 
    /// 4. Return an empty Schedule object, if the table has no value.
    /// 
    /// 5. Else, extract the first row values to the Schedule object. 
    /// 
    /// 6. Update Schedule's milestone value
    /// 
    /// 7. Return the Schedule data object. 
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvSchedule getSchedule (
      string TrialId,
      int ScheduleId,
      bool withActivities )
    {
      this.LogMethod ( "getSchedule. " ); 
      this.LogValue ( "TrialId: " + TrialId );
      this.LogValue ( "ScheduleId: " + ScheduleId );
      this.LogValue ( "withActivities: " + withActivities );
      // 
      // Define local variables
      // 
      System.Text.StringBuilder sqlQueryString = new System.Text.StringBuilder ( );
      EvSchedule schedule = new EvSchedule ( );
      EvMilestones milestones = new EvMilestones ( );

      // 
      // Check that the EvApprovedBy is valid.
      // 
      if ( TrialId == String.Empty )
      {
        return schedule;
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter ( EvSchedules.PARM_CUSTOMER_GUID, SqlDbType.UniqueIdentifier),
        new SqlParameter( PARM_PROJECT_ID, SqlDbType.NVarChar, 10),
        new SqlParameter( PARM_SCHEDULE_ID, SqlDbType.Int ),
      };
      cmdParms [ 0 ].Value = this.ClassParameters.CustomerGuid;
      cmdParms [ 1 ].Value = TrialId;
      cmdParms [ 2 ].Value = ScheduleId;

      // 
      // Generate the SQL query string.
      // 
      sqlQueryString.AppendLine ( _sqlQuery_View );
      sqlQueryString.AppendLine ( " WHERE ( " + EvSchedules.DB_CUSTOMER_GUID + " = " + EvSchedules.PARM_CUSTOMER_GUID + " ) " );
      sqlQueryString.AppendLine ( "  AND (" + DB_PROJECT_ID + " = " + PARM_PROJECT_ID + ") " );
      sqlQueryString.AppendLine ( "  AND (" + DB_DELETED + " = 0 ) " );
      sqlQueryString.AppendLine ( "  AND (" + DB_SCHEDULE_ID + " = " + PARM_SCHEDULE_ID + ") " );
      sqlQueryString.AppendLine ( "  AND  (" + DB_STATE + " = '" + EvSchedule.ScheduleStates.Issued + "') ;" );

      this.LogValue ( sqlQueryString.ToString() );

      //
      // Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString.ToString ( ), cmdParms ) )
      {
        // 
        // If not rows the return found, Return an object containing an EvSchedule data object.
        // 
        if ( table.Rows.Count == 0 )
        {
          return schedule;
        }

        // 
        // Extract the table row.
        // 
        DataRow row = table.Rows [ 0 ];

        // 
        // Fill the shedule object.
        // 
        schedule = this.readRow ( row );

        // 
        // Get the schedules milestones.
        // 
        List<EvMilestone.MilestoneTypes> types = new List<EvMilestone.MilestoneTypes> ( );
        types.Add ( EvMilestone.MilestoneTypes.Null );

        schedule.Milestones = milestones.getMilestoneList ( schedule.Guid, types, withActivities );

      }//END Using 

      this.LogMethodEnd ( "getSchedule" );
      // 
      // Return an object containing an EvSchedule data object.
      // 
      return schedule;

    }//END getSchedule method

    #endregion

    #region Schedules Update methods

    // =====================================================================================
    /// <summary>
    /// This method updates the items on Schedule data table
    /// </summary>
    /// <param name="Schedule">EvSchedule: A Schedule data object.</param>
    /// <returns>EvEventCodes: an event code for updating items.</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Exit, if the old schedule object's Guid is empty. 
    /// 
    /// 2. Add items to datachange object, if they do not exist after comparing. 
    /// 
    /// 3. Define the SQL query parameters and execute the storeprocedure for updating items. 
    /// 
    /// 4. Exit, if the storeprocedure runs fail. 
    /// 
    /// 5. Publish the schedule, if its state is issued. 
    /// 
    /// 6. Return an event code for updating items. 
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes updateSchedule ( EvSchedule Schedule )
    {
      this.LogMethod ( "updateSchedule" );
      this.LogValue ( "TrialId: " + Schedule.TrialId );
      this.LogValue ( "UserCommonName: " + Schedule.UserCommonName );
      this.LogValue ( "ApprovedDate: " + Schedule.ApprovedDate );
      this.LogValue ( "SiteGuid: " + Evado.Dal.EvStaticSetting.SiteGuid );
      // 
      // Initialise the method variables and objects.
      // 
      EvEventCodes iReturn = EvEventCodes.Ok;

      //
      // Validate whether the Old Schedule object's Guid is not empty.
      //
      EvSchedule oldSchedule = this.getSchedule ( Schedule.Guid );
      if ( oldSchedule.Guid == Guid.Empty )
      {
        return EvEventCodes.Data_InvalidId_Error;
      }

      // 
      // Compare the objects.
      // 
      EvDataChanges dataChanges = new EvDataChanges ( );
      EvDataChange dataChange = new EvDataChange ( );
      dataChange.TableName = EvDataChange.DataChangeTableNames.EvSchedules;
      dataChange.RecordGuid = Schedule.Guid;
      dataChange.TrialId = Schedule.TrialId;
      dataChange.UserId = Schedule.UpdatedByUserId;
      dataChange.DateStamp = DateTime.Now;

      //
      // Add items to the datachange object if they do not exist on the old schedule object. 
      //
      if ( Schedule.TrialId != oldSchedule.TrialId )
      {
        dataChange.AddItem ( "ProjectId", oldSchedule.TrialId, Schedule.TrialId );
      }
      if ( Schedule.ScheduleId != oldSchedule.ScheduleId )
      {
        dataChange.AddItem ( "ScheduleId", oldSchedule.ScheduleId, Schedule.ScheduleId );
      }
      if ( Schedule.Version != oldSchedule.Version )
      {
        dataChange.AddItem ( "Version", oldSchedule.Version.ToString ( ), Schedule.Version.ToString ( ) );
      }
      if ( Schedule.ApprovedBy != oldSchedule.ApprovedBy )
      {
        dataChange.AddItem ( "ApprovedBy", oldSchedule.ApprovedBy, Schedule.ApprovedBy );
      }
      if ( Schedule.ApprovedByUserId != oldSchedule.ApprovedByUserId )
      {
        dataChange.AddItem ( "ApprovedByUserId", oldSchedule.ApprovedByUserId, Schedule.ApprovedByUserId );
      }
      if ( Schedule.ApprovedDate != oldSchedule.ApprovedDate )
      {
        dataChange.AddItem ( "ApprovedDate",
          oldSchedule.ApprovedDate.ToString ( "yy MMM yyyy HH:mm:ss" ),
          Schedule.ApprovedDate.ToString ( "yy MMM yyyy HH:mm:ss" ) );
      }

      string oldXmlSignoffs =
        Evado.Model.Digital.EvcStatics.SerialiseObject<List<EvUserSignoff>> ( oldSchedule.Signoffs );

      string newXmlSignoffs =
        Evado.Model.Digital.EvcStatics.SerialiseObject<List<EvUserSignoff>> ( Schedule.Signoffs );

      if ( oldXmlSignoffs != newXmlSignoffs )
      {
        dataChange.AddItem ( "XmlSignoffs", oldXmlSignoffs, newXmlSignoffs );
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      this.LogValue ( "Update EvSchedule details" );

      SqlParameter [ ] _cmdParms = getItemsParameters ( );
      SetParameters ( _cmdParms, Schedule );

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( _storedProcedureUpdateItem, _cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      // 
      // Add the change record.
      // 
      dataChanges.AddItem ( dataChange );

      this.LogValue ( dataChanges.Log );

      this.LogMethodEnd ( "updateSchedule" );
      // 
      // Return an enumerated value EventCode status.
      // 
      return iReturn;

    }//END updateSchedule method.

    // =====================================================================================
    /// <summary>
    /// This method adds items to the schedule data table. 
    /// </summary>
    /// <param name="Schedule">EvSchedule: A schedule data object.</param>
    /// <returns>EvEventCodes: an event code for adding items.</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Create new Schedule object's Guid.  
    /// 
    /// 2. Define the SQL query parameters and execute the storeprocedure for adding items
    /// 
    /// 3. Exit, if the storeprocedure runs fail
    /// 
    /// 4. Else, return an event code for adding items. 
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes addSchedule ( EvSchedule Schedule )
    {
      this.LogMethod ( "addSchedule method. " );
      this.LogValue ( "ProjectId: " + Schedule.TrialId );
      this.LogValue ( "ApprovedBy: " + Schedule.ApprovedBy );
      this.LogValue ( "ApprovedDate: " + Schedule.ApprovedDate );
      // 
      // Define the local variables and objects.
      // 
      string _sqlQueryString = String.Empty;

      Schedule.Guid = Guid.NewGuid ( );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] _cmdParms = getItemsParameters ( );
      SetParameters ( _cmdParms, Schedule );

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( _storedProcedureAddItem, _cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      this.LogMethodEnd ( "addSchedule" );

      //
      // Return an enumerated value EventCode status.
      //
      return EvEventCodes.Ok;

    } //END addSchedule method.

    // =====================================================================================
    /// <summary>
    /// This method deletes items from Schedule data table.  
    /// </summary>
    /// <param name="Schedule">EvSchedule: A schedule data object.</param>
    /// <returns>EvEventCodes: an event code for deleting items.</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Exit, if the Schedule object's Guid or User common name is empty. 
    /// 
    /// 2. Define the sql query parameters and execute the storeprocedure for deleting schedule items. 
    /// 
    /// 3. Exit, if the storeprocedure runs fail. 
    /// 
    /// 4. Delete the schedule's milestones, if they exist. 
    /// 
    /// 5. Return an event code for deleting items. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes deleteSchedule ( EvSchedule Schedule )
    {
      this.LogMethod ( "deleteSchedule method. " );
      this.LogValue ( "ProjectId: " + Schedule.TrialId );

      //
      // Define local variables and objects.
      //
      EvMilestones milestones = new EvMilestones ( );

      // 
      // Validate whether the Guid or User Common Name is not empty. 
      // 
      if ( Schedule.Guid == Guid.Empty )
      {
        return EvEventCodes.Identifier_Global_Unique_Identifier_Error;
      }

      if ( Schedule.UserCommonName == String.Empty )
      {
        return EvEventCodes.Identifier_User_Id_Error;
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(PARM_Guid, SqlDbType.UniqueIdentifier),
          new SqlParameter(PARM_UpdatedByUserId, SqlDbType.NVarChar,100),
        new SqlParameter(PARM_UpdatedBy, SqlDbType.NVarChar,30),
        new SqlParameter(PARM_UpdatedDate, SqlDbType.DateTime)
      };
      cmdParms [ 0 ].Value = Schedule.Guid;
      cmdParms [ 1 ].Value = Schedule.UpdatedByUserId;
      cmdParms [ 2 ].Value = Schedule.UserCommonName;
      cmdParms [ 3 ].Value = DateTime.Now;

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( _storedProcedureDeleteItem, cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      // 
      // Delete milestones if they exist.
      // 
      if ( Schedule.Milestones.Count > 0 )
      {
        // 
        // Iterate through the milestones deleting each milestone.
        // 
        for ( int count = 0; count < Schedule.Milestones.Count; count++ )
        {
          EvMilestone milestone = Schedule.Milestones [ count ];

          milestone.UpdatedByUserId = Schedule.UpdatedByUserId;
          milestone.UserCommonName = Schedule.UserCommonName;

          EvEventCodes iReturn = milestones.deleteMilestone ( milestone );

          if ( iReturn < EvEventCodes.Ok )
          {
            return iReturn;
          }

        }//END Interation loop.

      }//END Milestone exist.

      this.LogMethodEnd ( "deleteSchedule" );
      //
      // Return an enumerated value EventCode status.
      //
      return EvEventCodes.Ok;

    }//END deleteSchedule method

    // =====================================================================================
    /// <summary>
    /// This method adds schedule items and the related milestones to database. 
    /// </summary>
    /// <param name="Schedule">EvSchedule: A schedule object.</param>
    /// <returns>EvEventCodes: An event code for adding items.</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Create new Schedule data object. 
    /// 
    /// 2. Iterate through the Schedule's milestones and add items to the milestone table
    /// 
    /// 3. Define the sql query parameters and execute the storeprocedure for adding items to schedule table. 
    /// 
    /// 4. Exit, if the storeprocedure runs fail. 
    /// 
    /// 5. Return an event code for adding items to schedule table. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes reviseSchedule ( EvSchedule Schedule )
    {
      this.LogMethod ( "reviseSchedule method. " );
      this.LogValue ( "ProjectId: " + Schedule.TrialId );
      this.LogValue ( "ApprovedBy: " + Schedule.ApprovedBy );
      this.LogValue ( "ApprovedDate: " + Schedule.ApprovedDate );

      // 
      // Define the local variables.
      // 
      string _sqlQueryString = String.Empty;
      EvMilestones milestones = new EvMilestones ( );

      //
      // check that there is not already a draft schedule object.
      //
      List<EvSchedule> scheduleList = this.getScheduleList ( Schedule.TrialId, 0, EvSchedule.ScheduleStates.Issued );

      foreach ( EvSchedule schedule in scheduleList )
      {
        if ( schedule.State == EvSchedule.ScheduleStates.Draft )
        {
          return EvEventCodes.Data_Duplicate_Object_Error;
        }

      }//END iteration loop


      //
      // Retrieve all of the milestones in the schedule.
      //
      Schedule.Milestones = milestones.getMilestoneList ( Schedule.Guid );

      this.LogValue ( milestones.Log );
      this.LogValue ( "Schedule.Milestones.Count: " + Schedule.Milestones.Count );

      // 
      // Generate the new schedule Guid and reset values.
      // 
      Schedule.Guid = Guid.NewGuid ( );
      Schedule.State = EvSchedule.ScheduleStates.Draft;
      Schedule.ApprovedDate = Evado.Model.Digital.EvcStatics.CONST_DATE_NULL;
      Schedule.ApprovedBy = Schedule.UserCommonName;
      Schedule.ApprovedByUserId = Schedule.UpdatedByUserId;
      Schedule.Version++;

      // 
      // Iterate through the Milestone list to add the milestones.
      // 
      this.LogValue ( "adding the milestones." );
      foreach ( EvMilestone milestone in Schedule.Milestones )
      {
        milestone.ScheduleGuid = Schedule.Guid;
        milestone.UpdatedByUserId = Schedule.UpdatedByUserId;
        milestone.UserCommonName = Schedule.UserCommonName;
        this.LogValue ( "MIilestoneId: " + milestone.MilestoneId + ", Activity count: " + milestone.ActivityList.Count );

        EvEventCodes iReturn = milestones.addMilestone ( milestone );
        this.LogValue ( milestones.Log );

        if ( iReturn < EvEventCodes.Ok )
        {
          this.LogValue ( "ERROR RETURN = " + iReturn );
          return iReturn;
        }

      }//END Milestone iteration loop.

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] _cmdParms = getItemsParameters ( );
      SetParameters ( _cmdParms, Schedule );

      this.LogValue ( "adding the schedule." );

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( _storedProcedureAddItem, _cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      this.LogMethodEnd ( "reviseSchedule" );
      //
      // Return an enumerated value EventCode status.
      //
      return EvEventCodes.Ok;

    }//END reviseSchedule method.

    #endregion

  }//END EvSchedules class

}//END Evado.Dal.Clinical namespace. 
