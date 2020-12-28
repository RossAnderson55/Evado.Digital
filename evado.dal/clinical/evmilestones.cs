/***************************************************************************************
 * <copyright file="dal\EvMilestones.cs" company="EVADO HOLDING PTY. LTD.">
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
  /// <summary>
  /// This class is handles the data access layer for the milestone data object.
  /// </summary>
  public class EvMilestones : EvDalBase
  {
    #region class initialisation method.
    /// <summary>
    /// This method initialises the schedule DAL class.
    /// </summary>
    public EvMilestones (  )
    {
      this.ClassNameSpace = "Evado.Dal.Clinical.EvMilestones.";
    }

    /// <summary>
    /// This method initialises the schedule DAL class.
    /// </summary>
    public EvMilestones ( EvClassParameters Settings )
    {
      this.ClassParameters = Settings;
      this.ClassNameSpace = "Evado.Dal.Clinical.EvMilestones.";

      if ( this.ClassParameters.LoggingLevel == 0 )
      {
        this.ClassParameters.LoggingLevel = Evado.Dal.EvStaticSetting.LoggingLevel;
      }

    }

    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion


    #region Class constants and variable initialisation

    private static string _eventLogSource = ConfigurationManager.AppSettings [ "EventLogSource" ];

    //private const string _sqlQuery_PreparationView = "Select * FROM EvMilestones ";


    private const string _sqlQuery_View = "Select * FROM EV_MILESTONE_VIEW ";

    // 
    // The SQL Store Procedure constants
    // 
    private const string _storedProcedureAddItem = "usr_Milestone_add";
    private const string _storedProcedureUpdateItem = "usr_Milestone_update";
    private const string _storedProcedureDeleteItem = "usr_Milestone_delete";

    /// <summary>
    /// This constand defines database column name for the milestone Guid 
    /// </summary>
    public const string DB_Milestone_Guid = "M_Guid";
    /// <summary>
    /// This constand defines database column name for the schedule Guid 
    /// </summary>
    public const string DB_ScheduleGuid = "SCH_Guid";
    /// <summary>
    /// This constand defines database column name for the project identifier
    /// </summary>
    public const string DB_TrialId = "TrialId";
    /// <summary>
    /// This constand defines database column name for the milestone type 
    /// </summary>
    public const string DB_Type = "M_Type";
    /// <summary>
    /// This constand defines database column name for the schedule identifier
    /// </summary>
    public const string DB_SCHEDULE_ID = "SCHEDULE_ID";
    /// <summary>
    /// This constand defines database column name for the schedule title
    /// </summary>
    public const string DB_SCH_TITLE = "SCH_TITLE";
    /// <summary>
    /// This constand defines database column name for the milestone identifier 
    /// </summary>
    public const string DB_MilestoneId = "MilestoneId";
    /// <summary>
    /// This constand defines database column name for the milestone title 
    /// </summary>
    public const string DB_Title = "M_Title";
    /// <summary>
    /// This constand defines database column name for the milestone order 
    /// </summary>
    public const string DB_Order = "M_Order";
    /// <summary>
    /// This constand defines database column name for the milestone CDASH metadata 
    /// </summary>
    public const string DB_CDASH_METADATA = "M_CDASH_METADATA";
    /// <summary>
    /// This constand defines database column name for the milestone site role identifier
    /// </summary>
    public const string DB_SiteRoleId = "M_SITE_ROLE_ID";
    /// <summary>
    /// This constand defines database column name for the milestone visit period intervals
    /// </summary>
    public const string DB_INTER_VISIT_PERIOD = "M_INTER_VISIT_PERIOD";
    /// <summary>
    /// This constand defines database column name for the milestone period 
    /// </summary>
    public const string DB_VISIT_PERIOD = "M_VISIT_PERIOD";
    /// <summary>
    /// This constand defines database column name for the number of times the milestone is repeated 
    /// </summary>
    public const string DB_REPEAT_NO_TIMES = "M_REPEAT_NO_TIMES";

    /// <summary>
    /// This constanT defines the table column for autoamtic scheduleing 
    /// </summary>
    private const string DB_ENABLE_AUTOMATIC_SCHEDULING = "M_ENABLE_AUTOMATIC_SCHEDULING";
    /// <summary>
    /// This constand defines database column name for the milestone starting after the consent date.
    /// </summary>
    public const string DB_LaterThanConsentDate = "M_LATER_THEN_CONSENT_DATE";
    /// <summary>
    /// This constand defines database column name for the milestone coordinators user id 
    /// </summary>
    public const string DB_COORDINATOR_USER_ID = "M_COORDINATOR_USER_ID";
    /// <summary>
    /// This constand defines database column name for the version of the schedule the milestone was create in. 
    /// </summary>
    public const string DB_INITIAL_SCHEDULE_VERSION = "M_INITIAL_SCHEDULE_VERSION";
    /// <summary>
    /// This constand defines database column name for the version of the schedule the milestone was create in. 
    /// </summary>
    public const string DB_OPTIONAL_SCHEDULE_VERSION = "M_OPTIONAL_SCHEDULE_VERSION";
    /// <summary>
    /// This constand defines database column name for the user's role identifier
    /// </summary>
    public const string DB_ROLE_ID = "M_SITE_ROLE_ID";
    /// <summary>
    /// This constand defines database column name for the previouse visit date
    /// </summary>
    public const string DB_PREVIOUS_VISIT_DATE = "M_PREVIOUS_VISIT_DATE";
    /// <summary>
    /// This constand defines database column name for the milestone description 
    /// </summary>
    public const string DB_Description = "M_Description";
    /// <summary>
    /// This constand defines database column name for the updated by user id 
    /// </summary>
    public const string DB_UpdatedByUserId = "M_UpdatedByUserId";
    /// <summary>
    /// This constand defines database column name for the update by user name
    /// </summary>
    public const string DB_UpdatedBy = "M_UpdatedBy";
    /// <summary>
    /// This constand defines database column name for the milestone Guid 
    /// </summary>
    public const string DB_UpdateDate = "M_UpdateDate";
    /// <summary>
    /// This constand defines database column name for the milestone data object as XML 
    /// </summary>
    public const string DB_XmlData = "M_XmlData";
    /// <summary>
    /// This constand defines database column name for the schedule title
    /// </summary>
    public const string DB_TITLE = "SCH_TITLE";
    /// <summary>
    /// This constand defines database column name for the milestone period increment 
    /// </summary>
    public const string DB_MILESTONE_PERIOD_INCREMENT = "SCH_MILESTONE_PERIOD_INCREMENT";

    private const string PARM_Guid = "@Guid";
    private const string PARM_ScheduleGuid = "@ScheduleGuid";
    private const string PARM_TrialId = "@TrialId";
    private const string PARM_Type = "@Type";
    private const string PARM_SCHEDULE_ID = "@SCHEDULE_ID";
    private const string PARM_MilestoneId = "@MilestoneId";
    private const string PARM_Title = "@Title";
    private const string PARM_Order = "@Order";
    private const string PARM_SiteRoleId = "@SiteRoleid";
    private const string PARM_InterVisitPeriod = "@InterVisitPeriod";
    private const string PARM_REPEAT_NO_TIMES = "@REPEAT_NO_TIMES";
    private const string PARM_ENABLE_AUTOMATIC_SCHEDULING = "@ENABLE_AUTOMATIC_SCHEDULING";
    private const string PARM_VisitPeriod = "@VisitPeriod";
    private const string PARM_LaterThanConsentDate = "@LaterThanConsentDate";
    private const string PARM_CDASH_METADATA = "@CDASH_METADATA";
    private const string PARM_COORDINATOR_USER_ID = "@COORDINATOR_USER_ID";
    private const string PARM_INITIAL_SCHEDULE_VERSION = "@INITIAL_SCHEDULE_VERSION";
    private const string PARM_OPTIONAL_SCHEDULE_VERSION = "@OPTIONAL_SCHEDULE_VERSION";
    private const string PARM_CURRENT_SCHEDULE_VERSION = "@CURRENT_SCHEDULE_VERSION";
    private const string PARM_PREVIOUS_VISIT_DATE = "@PREVIOUS_VISIT_DATE";
    private const string PARM_Description = "@Description";
    private const string PARM_UpdatedByUserId = "@UpdatedByUserId";
    private const string PARM_UpdatedBy = "@UpdatedBy";
    private const string PARM_UpdateDate = "@UpdateDate";
    private const string PARM_SCHEDULE_STATE = "@SCH_STATE";


    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region SQL Parameter methods

    // =====================================================================================
    /// <summary>
    /// This class sets the array of sql query parameters
    /// </summary>
    /// <returns>SqlParameter: an array of sql query parameters</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Create an array of sql query parameters. 
    /// 
    /// 2. Return an array of sql query parameters.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private static SqlParameter [ ] getItemsParameters ( )
    {
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( PARM_Guid, SqlDbType.UniqueIdentifier ),
        new SqlParameter( PARM_ScheduleGuid, SqlDbType.UniqueIdentifier ),
        new SqlParameter( PARM_TrialId, SqlDbType.NVarChar, 10),
        new SqlParameter( PARM_Type, SqlDbType.NVarChar, 50 ),
        new SqlParameter( PARM_SiteRoleId, SqlDbType.NVarChar, 100 ),

        new SqlParameter( PARM_InterVisitPeriod, SqlDbType.Int ),
        new SqlParameter( PARM_VisitPeriod, SqlDbType.VarChar, 30 ),
        new SqlParameter( PARM_LaterThanConsentDate, SqlDbType.Bit ),
        new SqlParameter( PARM_INITIAL_SCHEDULE_VERSION, SqlDbType.Int ),
        new SqlParameter( PARM_OPTIONAL_SCHEDULE_VERSION, SqlDbType.Int ),
        
        new SqlParameter( PARM_PREVIOUS_VISIT_DATE, SqlDbType.DateTime ),
        new SqlParameter( PARM_MilestoneId, SqlDbType.NVarChar, 20),
        new SqlParameter( PARM_Title, SqlDbType.NVarChar, 100),        
        new SqlParameter( PARM_CDASH_METADATA, SqlDbType.NVarChar, 250),

        new SqlParameter( PARM_Description, SqlDbType.NText),
        new SqlParameter( PARM_Order, SqlDbType.SmallInt),
        new SqlParameter( PARM_UpdatedByUserId, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_UpdatedBy, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_UpdateDate, SqlDbType.DateTime ),
        new SqlParameter( PARM_REPEAT_NO_TIMES, SqlDbType.Int ),
        new SqlParameter( PARM_ENABLE_AUTOMATIC_SCHEDULING, SqlDbType.Bit ),
      };

      return cmdParms;
    }

    // =====================================================================================
    /// <summary>
    /// This class binds values from milestone object to the parameter array.
    /// </summary>
    /// <param name="cmdParms">SqlParameter: an array of sql query parameters</param>
    /// <param name="Milestone">EvMilestone: Values to bind to parameters</param>
    /// <remarks>
    /// This method consists of the following step: 
    /// 
    /// 1. Update the values from Milestone object to the array of sql query parameters. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private void SetParameters ( SqlParameter [ ] cmdParms, EvMilestone Milestone )
    {

      cmdParms [ 0 ].Value = Milestone.Guid;
      cmdParms [ 1 ].Value = Milestone.ScheduleGuid;
      cmdParms [ 2 ].Value = Milestone.ProjectId.Trim ( );
      cmdParms [ 3 ].Value = Milestone.Type.ToString ( );
      cmdParms [ 4 ].Value = Milestone.SiteRoleId;

      cmdParms [ 5 ].Value = Milestone.InterMilestonePeriod_In_Days;
      cmdParms [ 6 ].Value = Milestone.MilestoneRange;
      cmdParms [ 7 ].Value = Milestone.MilestoneLaterThanConsentDate;
      cmdParms [ 8 ].Value = Milestone.Data.InitialScheduleVersion;
      cmdParms [ 9 ].Value = Milestone.Data.OptionalScheduleVersion;

      cmdParms [ 10 ].Value = Milestone.Data.PreviousVisitDate;
      cmdParms [ 11 ].Value = Milestone.MilestoneId.Trim ( );
      cmdParms [ 12 ].Value = Milestone.Title;
      cmdParms [ 13 ].Value = Milestone.cDashMetadata;
      cmdParms [ 14 ].Value = Milestone.Description;

      cmdParms [ 15 ].Value = Milestone.Order;
      cmdParms [ 16 ].Value = Milestone.UpdatedByUserId;
      cmdParms [ 17 ].Value = Milestone.UserCommonName;
      cmdParms [ 18 ].Value = DateTime.Now;
      cmdParms [ 19 ].Value = Milestone.RepeatNoTimes;

      cmdParms [ 20 ].Value = Milestone.EnableAutomaticScheduling;

    }//END SetParameters class.

    #endregion

    #region Data Reader methods

    // =====================================================================================
    /// <summary>
    /// This class extracts the compatible data row to the milestone object. 
    /// </summary>
    /// <param name="Row">DataRow: a data row object</param>
    /// <returns>EvMilestone: a milestone data object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Extract the compatible data row values to the milestone object values.
    /// 
    /// 2. If datarow contain xml validation rules, convert the old validation names to the current ones
    /// 
    /// 3. If the datarow contains xml data, deserialize the xmldata to the milestone object. 
    /// 
    /// 4. Return the Milestone data object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvMilestone readRow ( DataRow Row )
    {
      //
      // Initialize the milestone object. 
      //
      EvMilestone milestone = new EvMilestone ( );

      // 
      // Extract the compatible data row values to the milestone object values.
      // 
      milestone.Guid = EvSqlMethods.getGuid ( Row, DB_Milestone_Guid );
      milestone.MilestoneGuid = milestone.Guid;

      milestone.ScheduleGuid = EvSqlMethods.getGuid ( Row, DB_ScheduleGuid );
      milestone.ProjectId = EvSqlMethods.getString ( Row, DB_TrialId );
      milestone.ScheduleId = EvSqlMethods.getInteger ( Row, DB_SCHEDULE_ID );
      milestone.ScheduleTitle = EvSqlMethods.getString ( Row, DB_SCH_TITLE );
      milestone.MilestoneId = EvSqlMethods.getString ( Row, DB_MilestoneId );
      milestone.SiteRoleId = EvSqlMethods.getString ( Row, DB_SiteRoleId );

      string value = EvSqlMethods.getString ( Row, DB_MILESTONE_PERIOD_INCREMENT );
      if ( value != String.Empty )
      {
        milestone.MilestonePeriodIncrement = Evado.Model.EvStatics.Enumerations.parseEnumValue<EvSchedule.MilestonePeriodIncrements> ( value );
      }

      milestone.InterMilestonePeriod_In_Days = EvSqlMethods.getInteger ( Row, DB_INTER_VISIT_PERIOD );
      milestone.MilestoneRange = EvSqlMethods.getFloat ( Row, EvMilestones.DB_VISIT_PERIOD );

      milestone.MilestoneLaterThanConsentDate = EvSqlMethods.getBool ( Row, DB_LaterThanConsentDate );

      milestone.MilestoneId = EvSqlMethods.getString ( Row, DB_MilestoneId );
      milestone.Title = EvSqlMethods.getString ( Row, DB_Title );
      milestone.Description = EvSqlMethods.getString ( Row, DB_Description ); 
      milestone.Order = EvSqlMethods.getInteger ( Row, DB_Order );
      milestone.cDashMetadata = EvSqlMethods.getString ( Row, DB_CDASH_METADATA );
      milestone.RepeatNoTimes = EvSqlMethods.getInteger ( Row, DB_REPEAT_NO_TIMES );
      // 
      // If the datarow contains xml data, deserialize the xmldata to the milestone object. 
      // 
      string xmlData = EvSqlMethods.getString ( Row, DB_XmlData );
      if ( xmlData != String.Empty )
      {
        // 
        // Deserialise it into the data object.
        // 
        milestone.Data = Evado.Model.EvStatics.DeserialiseObject<EvMilestoneData> ( xmlData );

      }//End XML data
      else
      {
        milestone.Data.OptionalScheduleVersion = EvSqlMethods.getInteger ( Row, DB_INITIAL_SCHEDULE_VERSION );
        milestone.Data.InitialScheduleVersion = EvSqlMethods.getInteger ( Row, DB_OPTIONAL_SCHEDULE_VERSION );
        milestone.Data.PreviousVisitDate = EvSqlMethods.getDateTime ( Row, DB_PREVIOUS_VISIT_DATE );
      }

      milestone.Type = (EvMilestone.MilestoneTypes) Enum.Parse (
        typeof ( EvMilestone.MilestoneTypes ), EvSqlMethods.getString ( Row, DB_Type ) );

      milestone.UpdatedByUserId = EvSqlMethods.getString ( Row, DB_UpdatedByUserId );
      milestone.UpdatedBy = EvSqlMethods.getString ( Row, DB_UpdatedBy );
      milestone.UpdatedBy += " on " + EvSqlMethods.getDateTime ( Row, DB_UpdateDate ).ToString ( "dd MMM yyyy HH:mm" );

      //
      // Return the Milestone object.
      //
      return milestone;

    }// End readRow method.

    #endregion

    #region Milestone Query methods

    // =====================================================================================
    /// <summary>
    /// This class gets a list of EvMilestone data objects. 
    /// </summary>
    /// <param name="ScheduleGuid">Guid: (Mandatory) The schedule global unique identifier</param>
    /// <param name="Type">EvMilestone.MilestoneTypes: The Milestone type </param>
    /// <returns>List of EvMilestone: a milestone data objects.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. If Arm Index is not set, set it to the default value.
    /// 
    /// 2. Define the sql query parameters and sql query string. 
    /// 
    /// 3. Execute the sql query string and store the results on data table. 
    /// 
    /// 4. Loop through the table and extract the data row to the milestone object
    /// 
    /// 5. Add the milestone activities object items to the milestone object. 
    /// 
    /// 6. Add the milestone object to the Milestones list. 
    /// 
    /// 7. Return the Milestones list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvMilestone> getMilestoneList (
      Guid ScheduleGuid,
      EvMilestone.MilestoneTypes Type )
    {
      //
      // Initialize the method debug, a local sql query, a return milestone list and a milestone activities object. 
      //
      this.LogMethod ( "getMilestoneList. " );
      this.LogValue ( "ScheduleGuid: " + ScheduleGuid );
      this.LogValue ( "MilestoneType: " + Type );

      string sqlQueryString;
      List<EvMilestone> milestoneList = new List<EvMilestone> ( );
      EvMilestoneActivities milestoneActivites = new EvMilestoneActivities ( );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(PARM_ScheduleGuid, SqlDbType.UniqueIdentifier),
      };
      cmdParms [ 0 ].Value = ScheduleGuid;

      // 
      // Generate the SQL query string
      // 
      sqlQueryString = _sqlQuery_View + "WHERE SCH_Guid = @ScheduleGuid AND M_Deleted = 0 ";

      if ( Type != EvMilestone.MilestoneTypes.Null )
      {
        if ( Type == EvMilestone.MilestoneTypes.Non_Clinical )
        {
          sqlQueryString += " AND M_Type <> '" + EvMilestone.MilestoneTypes.Clinical + "' ";
          sqlQueryString += " AND M_Type <> '" + EvMilestone.MilestoneTypes.Questionnaire + "' ";
          sqlQueryString += " AND M_Type <> '" + EvMilestone.MilestoneTypes.UnScheduled + "' ";
          sqlQueryString += " AND M_Type <> '" + EvMilestone.MilestoneTypes.Implant_Visit + "' ";
        }
        else
        {
          sqlQueryString += " AND M_Type = '" + Type.ToString ( ) + "' ";
        }
      }

      sqlQueryString += " ORDER BY M_Order,MilestoneId; ";

      this.LogValue (   sqlQueryString );

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

          //
          // read in the milestone.
          //
          EvMilestone milestone = this.readRow ( row );

          this.LogValue ( "Milestone Guid: " + milestone.Guid
            + " MilestoneId: " + milestone.MilestoneId );

          // 
          // Read in the milestone activities.
          // 
          milestone.ActivityList = milestoneActivites.getActivityList ( milestone.Guid,
            new List<EvActivity.ActivityTypes> ( ) );

          this.LogValue ( "MilestoneActivities:" + milestoneActivites.DebugLog + "\r\n" );

          // 
          // Add the Milestone to the list
          // 
          milestoneList.Add ( milestone );

        }//END read table iteration loop.

      }//END using method

      this.LogValue ( "view count: " + milestoneList.Count.ToString ( ) );
      // 
      // Return the ArrayList containing the EvMilestone data object.
      // 
      return milestoneList;

    }//END getPreparationView method.

    // =====================================================================================
    /// <summary>
    /// This class gets an list of preparation EvMilestone data objects based onTrialId and Type. 
    /// </summary>
    /// <param name="ProjectId">string: (Mandatory) The trial identifier</param>
    /// <param name="ScheduleId">int: (Mandatory) The schedule identifier</param>
    /// <param name="Type">EvMilestone.MilestoneType: The Milestone type </param>
    /// <param name="withActivities">Boolean: true if the activities exist</param>
    /// <returns>List of EvMilestone: a list of milestone object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define the sql query parameters and sql query string. 
    /// 
    /// 2. Execute the sql query string and store the results on the datatable.
    /// 
    /// 3. Loop through the table and extract the row data to milestone object. 
    /// 
    /// 4. Add a list of activities to milestone object if they exist. 
    /// 
    /// 5. Add milestone object to the the Milestones list. 
    /// 
    /// 6. Return the Milestones list.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvMilestone> getMilestoneList (
      String ProjectId,
      int ScheduleId,
      EvMilestone.MilestoneTypes Type,
      bool withActivities )
    {
      //
      // Initilialize the debug log, a local sql query string, a return list of milestone 
      // and a milestone activities object. 
      //
      this.LogMethod ( "getMilestoneList. " );
      this.LogValue ( "ProjectId: " + ProjectId );
      this.LogValue ( "ScheduleId: " + ScheduleId );
      this.LogValue ( "MilestoneType: " + Type );
      this.LogValue ( "withActivities: " + withActivities );

      string sqlQueryString;
      List<EvMilestone> milestoneList = new List<EvMilestone> ( );
      EvMilestoneActivities milestoneActivites = new EvMilestoneActivities ( );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(PARM_TrialId, SqlDbType.NVarChar,10),
        new SqlParameter(PARM_SCHEDULE_ID, SqlDbType.Int),
      };
      cmdParms [ 0 ].Value = ProjectId;
      cmdParms [ 1 ].Value = ScheduleId;

      // 
      // Generate the SQL query string
      // 
      sqlQueryString = _sqlQuery_View + "WHERE (" + DB_TrialId + " = " + PARM_TrialId + ") "
         + " AND (" + DB_SCHEDULE_ID + " = " + PARM_SCHEDULE_ID + ") ";

      if ( Type != EvMilestone.MilestoneTypes.Null )
      {
        if ( Type == EvMilestone.MilestoneTypes.Non_Clinical )
        {
          sqlQueryString += " AND (M_Type <> '" + EvMilestone.MilestoneTypes.Clinical + "') ";
          sqlQueryString += " AND (M_Type <> '" + EvMilestone.MilestoneTypes.Questionnaire + "') ";
          sqlQueryString += " AND (M_Type <> '" + EvMilestone.MilestoneTypes.UnScheduled + "') ";
        }
        else
        {
          sqlQueryString += " AND (M_Type = '" + Type.ToString ( ) + "') ";
        }
      }
      sqlQueryString += " ORDER BY M_UpdateDate, Arm_Index, M_Order; ";

      this.LogValue (   sqlQueryString );

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

          EvMilestone milestone = this.readRow ( row );

          // 
          // If withActivities true then attach activities.
          // 
          if ( withActivities == true )
          {
            //
            // Read in the newField forms.
            // 
            milestone.ActivityList = milestoneActivites.getActivityList ( milestone.Guid, new List<EvActivity.ActivityTypes> ( ) );

            this.LogValue ( "MilestoneId: " + milestone.MilestoneId );
            this.LogValue ( "MilestoneActivities: \r\n" + milestoneActivites.DebugLog + "\r\n" );
          }

          // 
          // Add the Milestone to the list
          // 
          milestoneList.Add ( milestone );

        }//END read table iteration loop.

      }//END using method

      this.LogValue ( "view count: " + milestoneList.Count.ToString ( ) );

      // 
      // Return the ArrayList containing the EvMilestone data object.
      // 
      return milestoneList;

    }//END getPreparationView method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of milestone object items based on Schedule Guid
    /// </summary>
    /// <param name="ScheduleGuid">Guid: (Mandatory) The schedule global unique identiifer</param>
    /// <returns>List of EvMilestone: a list of milestone object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define the sql query parameters and sql query string. 
    /// 
    /// 2. Execute the sql query string and store the results on the datatable.
    /// 
    /// 3. Loop through the table and extract the row data to milestone object. 
    /// 
    /// 4. Add a list of activities to milestone object if they exist. 
    /// 
    /// 5. Add milestone object to the the Milestones list. 
    /// 
    /// 6. Return a Milestones list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvMilestone> getMilestoneList ( 
      Guid ScheduleGuid )
    {
      //
      // Initialize the debug log, sql query, a return list of milestone object 
      // and a milestone activity object. 
      //
      this.LogMethod ( "getMilestoneList. " );
      this.LogValue ( "ScheduleGuid: " + ScheduleGuid );

      string sqlQueryString;
      List<EvMilestone> milestoneList = new List<EvMilestone> ( );
      EvMilestoneActivities milestoneActivites = new EvMilestoneActivities ( );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(PARM_ScheduleGuid, SqlDbType.UniqueIdentifier),
      };
      cmdParms [ 0 ].Value = ScheduleGuid;

      // 
      // Generate the SQL query string
      // 
      sqlQueryString = _sqlQuery_View + "WHERE SCH_Guid = @ScheduleGuid "
        + " ORDER BY SCHEDULE_ID, M_Order, MilestoneId; ";

      this.LogValue (   sqlQueryString );

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

          EvMilestone milestone = this.readRow ( row );


          // Read in the newField forms.
          // 
          milestone.ActivityList = milestoneActivites.getActivityList ( milestone.Guid, new List<EvActivity.ActivityTypes> ( ) );

          // 
          // Add the Milestone to the list
          // 
          milestoneList.Add ( milestone );

        }//END read table iteration loop.

      }//END using method

      this.LogValue ( "view count: " + milestoneList.Count.ToString ( ) );
      // 
      // Return the ArrayList containing the EvMilestone data object.
      // 
      return milestoneList;

    }//END getPreparationView method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of option object items based on Schedule Guid, ScheduleId, Type 
    /// and usingMilestoneId condition
    /// </summary>
    /// <param name="ScheduleGuid">Guid: (Mandatory) The schedule's global unique identifier</param>
    /// <param name="Type">EvMilestone.MilestoneTypes: The Milestone type </param>
    /// <param name="usingMilestoneId">Boolean: True, MilestoneId is used</param>
    /// <returns>List of EvOption: an option list contains EvOption data objects.</returns>
    /// <remarks>
    /// 
    /// 1. If the Arm Index is not set, set it to the default.
    /// 
    /// 2. Get a list of milestone data object. 
    /// 
    /// 3. Iterate through the results and extract the EvMilestone information to the option object
    /// with MilestoneId condition.
    /// 
    /// 4. Add the option object values to the options list. 
    /// 
    /// 5. Return the options list. 
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> getMilestoneSelectionList (
      Guid ScheduleGuid,
      EvMilestone.MilestoneTypes Type,
      bool usingMilestoneId )
    {
      //
      // Initialize the debug log, a return option list and an option object
      //
      this.LogMethod ( "getMilestoneSelectionList. " );
      this.LogValue ( "ScheduleGuid: " + ScheduleGuid );
      this.LogValue ( "MilestoneType: " + Type.ToString ( ) );

      List<EvOption> selectionList = new List<EvOption> ( );
      EvOption option = new EvOption ( Guid.Empty.ToString ( ), String.Empty );
      selectionList.Add ( option );

      //
      // Get a list of milestone data object. 
      //
      List<EvMilestone> milestoneList = this.getMilestoneList ( ScheduleGuid,
          Type );

      // 
      // Iterate through the results extracting the EvMilestone information.
      // 
      foreach ( EvMilestone milestone in milestoneList )
      {
        if ( milestone.Guid != Guid.Empty )
        {
          if ( usingMilestoneId == true )
          {
            option = new EvOption ( milestone.MilestoneId,
                milestone.MilestoneId + " " + milestone.Title );

            selectionList.Add ( option );
          }
          else
          {
            option = new EvOption ( milestone.Guid.ToString ( ),
               milestone.MilestoneId + " " + milestone.Title );

            selectionList.Add ( option );
          }
        }

      }//END userList iteration loop.

      this.LogValue ( "list count: " + selectionList.Count.ToString ( ) );

      // 
      // Return the ArrayList containing the EvMilestone data object.
      // 
      return selectionList;

    }//END getPrepationSelectionList method.

    #endregion

    #region Milestone Query methods

    // =====================================================================================
    /// <summary>
    /// This class returns a list of Milestone data object based on VisitId, ScheduleId, Types,
    /// OrderBy and WithActivities condition
    /// </summary>
    /// <param name="ProjectId">string: (Mandatory) The milestone trial identifier</param>
    /// <param name="ScheduleId">EvTrialArm.ScheduleIdes: (Mandatory)The schedule arm index</param>
    /// <param name="Types">List of EvMilestone.MilestoneTypes: A list of milestone types</param>
    /// <param name="withActivities">Boolean: true, if activities are included in the list</param>
    /// <returns>List of EvMilestone: a list of Milestones data object.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. If the ScheduleId is null, set it to a default value. 
    /// 
    /// 2. Define the sql query parameters and sql query string. 
    /// 
    /// 3. Execute the sql query string and store the results on the datatable. 
    /// 
    /// 4. Loop through the table and extract row data to the milestone object.
    /// 
    /// 5. Add Milestone's activities to the milestone object if they exist. 
    /// 
    /// 6. Add Milestone object value to the Milestones list. 
    /// 
    /// 7. Return the Milestones list.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvMilestone> getIssuedScheduleMilestoneList (
      String ProjectId,
      int ScheduleId,
      List<EvMilestone.MilestoneTypes> Types,
      bool withActivities )
    {
      //
      // Initialize the debug log, sql query string, a return Milestone list and Milestone activities object
      //
      this.LogMethod ( "getIssuedScheduleMilestoneList. " );
      this.LogValue ( "ProjectId: " + ProjectId );
      this.LogValue ( "ScheduleId: " + ScheduleId );
      this.LogValue ( "MilestoneType count: " + Types.Count );

      string sqlQueryString;
      List<EvMilestone> view = new List<EvMilestone> ( );
      EvMilestoneActivities milestoneActivities = new EvMilestoneActivities ( );

      //
      // If the ScheduleId is null, set it to a default value. 
      //
      if ( ScheduleId < 0 )
      {
        ScheduleId = 1;
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( PARM_TrialId, SqlDbType.NVarChar, 10),
        new SqlParameter( PARM_SCHEDULE_ID, SqlDbType.SmallInt ),
      };
      cmdParms [ 0 ].Value = ProjectId;
      cmdParms [ 1 ].Value = ScheduleId;

      // 
      // Generate the SQL query string
      // 
      sqlQueryString = _sqlQuery_View + "WHERE TrialId = @TrialId "
        + "  AND SCHEDULE_ID = " + PARM_SCHEDULE_ID
        + "  AND SCH_STATE = '" + EvSchedule.ScheduleStates.Issued + "' \r\n";

      // 
      // If the type list exist process it
      // 
      if ( Types.Count > 0 )
      {
        // 
        // If not a null type process the list.
        // 
        if ( (EvMilestone.MilestoneTypes) Types [ 0 ] != EvMilestone.MilestoneTypes.Null )
        {
          string stTypes = string.Empty;  // The string collects the or selection for types.

          // 
          // Iterate through the types.
          // 
          foreach ( EvMilestone.MilestoneTypes type in Types )
          {
            if ( stTypes != String.Empty )
            {
              stTypes += " OR ";
            }
            stTypes += " M_Type = '" + type.ToString ( ) + "' \r\n";

          }//END types interation loop.

          sqlQueryString += " AND ( " + stTypes + " ) ";
        }
      }//END Types exists.

      sqlQueryString += " ORDER BY M_Order; ";

      this.LogValue (   sqlQueryString );

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

          EvMilestone milestone = this.readRow ( row );

          this.LogValue ( String.Format ("M: {0}, T: {1} ", milestone.MilestoneId, milestone.Type ) );

          view.Add ( milestone );

          // 
          // Get the milestone activities if requrested.
          // 
          if ( withActivities == true )
          {
            milestone.ActivityList = milestoneActivities.getActivityList ( milestone.Guid, EvActivity.ActivityTypes.Null, false, true );
            // this.writeDebugLog( "" + milestoneActivities.DebugLog;
            // this.writeDebugLog( " Activity count: " + milestone.ActvityList.Count;

          }//END Get activities

        }//END record iteration loop
      }//END Using method

      this.LogValue ( "Milestone object count: " + view.Count );

      // 
      // Return the ArrayList containing the EvMilestone data object.
      // 
      return view;

    }//END getView method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of Milestone data object based on VisitId, ScheduleId, Types,
    /// OrderBy and WithActivities condition
    /// </summary>
    /// <param name="ScheduleGuid">Guid: (Mandatory) The milestone trial identifier</param>
    /// <param name="Types">List of EvMilestone.MilestoneTypes: A list of milestone types</param>
    /// <param name="withActivities">Boolean: true, if activities are included in the list</param>
    /// <returns>List of EvMilestone: a list of Milestones data object.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. If the ScheduleId is null, set it to a default value. 
    /// 
    /// 2. Define the sql query parameters and sql query string. 
    /// 
    /// 3. Execute the sql query string and store the results on the datatable. 
    /// 
    /// 4. Loop through the table and extract row data to the milestone object.
    /// 
    /// 5. Add Milestone's activities to the milestone object if they exist. 
    /// 
    /// 6. Add Milestone object value to the Milestones list. 
    /// 
    /// 7. Return the Milestones list.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvMilestone> getMilestoneList (
      Guid ScheduleGuid,
      List<EvMilestone.MilestoneTypes> Types,
      bool withActivities )
    {
      //
      // Initialize the debug log, sql query string, a return Milestone list and Milestone activities object
      //
      this.LogMethod ( "getMilestoneList. " );
      this.LogValue ( "ScheduleGuid: " + ScheduleGuid );
      this.LogValue ( "MilestoneType count: " + Types.Count );
      this.LogValue ( "withActivities: " + withActivities );

      string sqlQueryString;
      List<EvMilestone> view = new List<EvMilestone> ( );
      EvMilestoneActivities milestoneActivities = new EvMilestoneActivities ( );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( PARM_ScheduleGuid, SqlDbType.UniqueIdentifier),
      };
      cmdParms [ 0 ].Value = ScheduleGuid;

      // 
      // Generate the SQL query string
      // 
      sqlQueryString = _sqlQuery_View + "WHERE SCH_GUID = " + PARM_ScheduleGuid + " ";

      // 
      // If the type list exist process it
      // 
      if ( Types.Count > 0 )
      {
        // 
        // If not a null type process the list.
        // 
        if ( (EvMilestone.MilestoneTypes) Types [ 0 ] != EvMilestone.MilestoneTypes.Null )
        {
          string stTypes = string.Empty;  // The string collects the or selection for types.

          // 
          // Iterate through the types.
          // 
          foreach ( EvMilestone.MilestoneTypes type in Types )
          {
            if ( stTypes != String.Empty )
            {
              stTypes += " OR ";
            }
            stTypes += " M_Type = '" + type.ToString ( ) + "' ";

          }//END types interation loop.

          sqlQueryString += " AND ( " + stTypes + " ) ";
        }
      }//END Types exists.

      sqlQueryString += " ORDER BY M_Order; ";

      this.LogValue (   sqlQueryString );

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

          EvMilestone milestone = this.readRow ( row );

          view.Add ( milestone );

          // 
          // Get the milestone activities if requrested.
          // 
          if ( withActivities == true )
          {
            milestone.ActivityList = milestoneActivities.getActivityList ( milestone.Guid, EvActivity.ActivityTypes.Null, false, true );
            // this.writeDebugLog( "" + milestoneActivities.DebugLog;
            // this.writeDebugLog( " Activity count: " + milestone.ActvityList.Count;

          }//END Get activities

        }//END record iteration loop
      }//END Using method

      this.LogValue ( "Milestone object count: " + view.Count );

      // 
      // Return the ArrayList containing the EvMilestone data object.
      // 
      return view;

    }//END getView method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of option data object based on VisitId, ScheduleId and Types. 
    /// </summary>
    /// <param name="ProjectId">string: (Mandatory) The trial identifier</param>
    /// <param name="ScheduleId">EvTrialArm.ScheduleIdes: The schedule arm index</param>
    /// <param name="Types">List of EvMilestone.MilestoneTypes: a list of milestone types</param>
    /// <param name="ScheduleState">EvSchedule.ScheduleStates enumeration value</param>
    /// <returns>List of EvOption: a list of milestone options.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Set the ScheduleId to default value, if it is null. 
    /// 
    /// 2. Define the sql query prameters and sql query string
    /// 
    /// 3. Execute the sql query string and store the results on datatable. 
    /// 
    /// 5. Loop through table and extract data row to the option object. 
    /// 
    /// 6. Add the option object to the Options list.
    /// 
    /// 7. Return the Option list
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> getOptionList (
      string ProjectId,
      int ScheduleId,
      List<EvMilestone.MilestoneTypes> Types,
      EvSchedule.ScheduleStates ScheduleState )
    {
      //
      // Initialize the debug log, sql query string, a return option list and an option object. 
      //
      this.LogMethod ( "getList. " );
      this.LogValue ( "ProjectId: " + ProjectId );
      this.LogValue ( "ScheduleId: " + ScheduleId );
      this.LogValue ( "MilestoneType: " + Types.Count );

      string sqlQueryString;
      List<EvOption> list = new List<EvOption> ( );
      EvOption option = new EvOption ( );
      list.Add ( option );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( PARM_TrialId, SqlDbType.NVarChar, 10),
        new SqlParameter( PARM_SCHEDULE_ID, SqlDbType.SmallInt ),
        new SqlParameter( PARM_SCHEDULE_STATE, SqlDbType.NVarChar, 50 ),
      };
      cmdParms [ 0 ].Value = ProjectId;
      cmdParms [ 1 ].Value = (int) ScheduleId;
      cmdParms [ 2 ].Value = ScheduleState;

      // 
      // Generate the SQL query string
      // 
      sqlQueryString = _sqlQuery_View + "WHERE TrialId = @TrialId  "
        + " AND SCHEDULE_ID = " + PARM_SCHEDULE_ID + " ";

      if ( ScheduleState != EvSchedule.ScheduleStates.Null )
      {
        sqlQueryString += " AND SCH_STATE = " + PARM_SCHEDULE_STATE + " ";
      }

      // 
      // If the type list exist process it
      // 
      if ( Types.Count > 0 )
      {
        // 
        // If not a null type process the list.
        // 
        if ( (EvMilestone.MilestoneTypes) Types [ 0 ] != EvMilestone.MilestoneTypes.Null )
        {
          string stTypes = string.Empty;  // The string collects the or selection for types.

          // 
          // Iterate through the types.
          // 
          foreach ( EvMilestone.MilestoneTypes type in Types )
          {
            if ( stTypes != String.Empty )
            {
              stTypes += " OR ";
            }
            stTypes += " M_Type = '" + type.ToString ( ) + "' ";

          }//END types interation loop.

          sqlQueryString += " AND ( " + stTypes + " ) ";
        }
      }//END Types exists.

      sqlQueryString += " ORDER BY M_Order; ";

      this.LogValue (   sqlQueryString );

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

          option = new EvOption ( EvSqlMethods.getString ( row, "MilestoneId" ),
            EvSqlMethods.getString ( row, "MilestoneId" ) + " - " + EvSqlMethods.getString ( row, "M_Title" ) );

          list.Add ( option );
        }
      }
      // 
      // Return the list containing the EvMilestone data object.
      // 
      return list;

    }//END getList method.

    // =====================================================================================
    /// <summary>
    /// This class returns an option list based on VisitId and Schedule Guid. 
    /// </summary>
    /// <param name="ProjectId">String: (Mandatory) The trial identifier</param>
    /// <param name="ScheduleGuid">Guid: (Mandatory) The schedule global unique identifier.</param>
    /// <returns>List of EvOption: a list of milestone options</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define the sql query prameters and sql query string
    /// 
    /// 2. Execute the sql query string and store the results on datatable. 
    /// 
    /// 3. Loop through table and extract data row to the option object. 
    /// 
    /// 4. Add the option object to the Options list. 
    /// 
    /// 5. Return the Options list.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private List<EvOption> milestoneValidationList (
      String ProjectId,
      Guid ScheduleGuid )
    {
      //
      // Initialize the debug log, a sql query string and a return option list. 
      //
      this.LogMethod ( "milestoneValidationList. " );
      this.LogValue ( "ProjectId: " + ProjectId );
      this.LogValue ( "ScheduleGuid: " + ScheduleGuid );

      string sqlQueryString;
      List<EvOption> list = new List<EvOption> ( );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( PARM_TrialId, SqlDbType.NVarChar, 10),
        new SqlParameter(PARM_ScheduleGuid, SqlDbType.UniqueIdentifier),
     };
      cmdParms [ 0 ].Value = ProjectId;
      cmdParms [ 1 ].Value = ScheduleGuid;

      // 
      // Generate the SQL query string
      // 
      sqlQueryString = _sqlQuery_View + "WHERE TrialId = @TrialId "
        + " AND SCH_GUID = @ScheduleGuid "
        + " ORDER BY MilestoneId, M_Order; ";

      this.LogValue (   sqlQueryString );

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

          EvOption option = new EvOption ( EvSqlMethods.getString ( row, "MilestoneId" ),
            EvSqlMethods.getString ( row, "MilestoneId" ) + " - " + EvSqlMethods.getString ( row, "M_Title" ) );

          this.LogValue (   option.Description );

          list.Add ( option );
        }
      }

      this.LogValue ( "List count: " + list.Count );

      // 
      // Return the list containing the EvMilestone data object.
      // 
      return list;

    }//END milestoneValidationList method.

    #endregion

    #region Milestone Retrieval methods

    // =====================================================================================
    /// <summary>
    /// This class retrieves Milestone data object based on MilestoneGuid and operational condition. 
    /// </summary>
    /// <param name="MilestoneGuid">Guid: Milestone global unqiue identifier</param>
     /// <returns>EvMilestone: a Milestone Data object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Return an empty Milestone object if the Milestone Guid is empty. 
    /// 
    /// 2. Define sql query parameters and sql query string. 
    /// 
    /// 3. Execute the sql query string and store the results on data table. 
    /// 
    /// 4. Extract the first row data to the return milestone object. 
    /// 
    /// 5. Add the milestone activities to the milestone object, if they exist. 
    /// 
    /// 6. Return the Milestone data object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvMilestone getMilestone (
      Guid MilestoneGuid )
    {
      //
      // Initialize debug log, sql query string, a return milestone object and a milestone activities object. 
      //
      this.LogMethod ( "getMilestone method." );
      this.LogValue ( "MilestoneGuid: " + MilestoneGuid );

      string sqlQueryString;
      EvMilestone milestone = new EvMilestone ( );
      EvMilestoneActivities activities = new EvMilestoneActivities ( );

      // 
      // Validate whether the MilestoneGuid is not empty.
      // 
      if ( MilestoneGuid == Guid.Empty )
      {
        return milestone;
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter cmdParms = new SqlParameter ( PARM_Guid, SqlDbType.UniqueIdentifier );
      cmdParms.Value = MilestoneGuid;

      // 
      // Generate the SQL query string
      // 
      sqlQueryString = _sqlQuery_View + " WHERE (M_Guid = @Guid);";

      this.LogValue (   sqlQueryString );

      //
      // Execute the query against the database
      //
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
      {
        // 
        // If not rows the return
        // 
        if ( table.Rows.Count == 0 )
        {
          return milestone;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        milestone = this.readRow ( row );

        // 
        // Get the Milestone activities.
        // 
        milestone.ActivityList = activities.getActivityList (
          milestone.Guid,
          new List<EvActivity.ActivityTypes> ( ) );

      }//END Using method

      this.LogValue ( "InterMilestonePeriod_In_Days: " + milestone.InterMilestonePeriod_In_Days );
      this.LogValue ( "MilestoneRange: " + milestone.MilestoneRange );

      // 
      // Return the EvMilestone data object.
      // 
      return milestone;

    }//END getMilestone method    

    // =====================================================================================
    /// <summary>
    /// This class retrieves Milestone data object based on VisitId, ScheduleId and MilestoneId
    /// </summary>
    /// <param name="ProjectId">String: The trial identifier</param>
    /// <param name="ScheduleId">EvTrialArm.ScheduleIdes: The milestone arm index</param>
    /// <param name="MilestoneId">String: The milestone identifier</param>
    /// <returns>EvMilestone: a milestone Data object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Return an empty Milestone object if the MilestoneId and VisitId are not empty. 
    /// 
    /// 2. Define sql query parameters and sql query string. 
    /// 
    /// 3. Execute the sql query string and store the results on data table. 
    /// 
    /// 4. Extract the first row data to the return milestone object. 
    /// 
    /// 5. Add the activities to the milestone object, if they exist. 
    /// 
    /// 6. Return the Milestone data object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvMilestone getMilestone (
      String ProjectId,
      int ScheduleId,
      String MilestoneId )
    {
      //
      // Initialize the debug log, a sql query string, a return milestone object, a milestone activities object. 
      //
      this.LogValue (   Evado.Model.EvStatics.CONST_METHOD_START
        + "Evado.Dal.Clinical.EvMilestones.getMilestone method. " );
      this.LogValue ( "ProjectId: " + ProjectId );
      this.LogValue ( "ScheduleId: " + ScheduleId );
      this.LogValue ( "MilestoneId: " + MilestoneId );

      string sqlQueryString;
      EvMilestone milestone = new EvMilestone ( );
      EvMilestoneActivities activities = new EvMilestoneActivities ( );

      // 
      // Validate whether the MilestoneId and VisitId are not empty. 
      // 
      if ( MilestoneId == String.Empty
        && ScheduleId < 1
        && ProjectId == String.Empty )
      {
        return milestone;
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( PARM_TrialId, SqlDbType.NVarChar, 10),
        new SqlParameter( PARM_SCHEDULE_ID, SqlDbType.Int ),
        new SqlParameter( PARM_MilestoneId, SqlDbType.Char, 20),
      };
      cmdParms [ 0 ].Value = ProjectId;
      cmdParms [ 1 ].Value = ScheduleId;
      cmdParms [ 2 ].Value = MilestoneId;

      foreach ( SqlParameter parm in cmdParms )
      {
        this.LogValue ( "PN:" + parm.ParameterName + ",  T:" + parm.DbType + ", V:" + parm.Value );
      }

      // 
      // Generate the SQL query string
      // 
        sqlQueryString = _sqlQuery_View + " WHERE (" + DB_TrialId + " = " + PARM_TrialId + ") "
          + " AND " + DB_SCHEDULE_ID + " = " + PARM_SCHEDULE_ID + " "
          + " AND (" + DB_MilestoneId + " = " + PARM_MilestoneId + ") "
          + " AND (SCH_State = 'Issued') ;";

      this.LogValue (   sqlQueryString );

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
          this.LogValue ( "Zero rows returned. " );
          return milestone;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        milestone = readRow ( row );

        this.LogValue ( "MilestoneId: " + milestone.MilestoneId );
        this.LogValue ( "Title: " + milestone.Title );

        // 
        // Get the Milestone activities.
        // 
        milestone.ActivityList = activities.getActivityList ( milestone.Guid, EvActivity.ActivityTypes.Null, false, true );

      }//END using statement

      // 
      // Return the EvMilestone data object.
      // 
      return milestone;

    }//END getMilestone method

    // =====================================================================================
    /// <summary>
    /// This class retrieves Milestone data object based on VisitId, ScheduleId and MilestoneId
    /// </summary>
    /// <param name="ScheduleGuid">Guid: The Schedule Guid identifier</param>
    /// <param name="MilestoneId">String: The milestone identifier</param>
    /// <returns>EvMilestone: a milestone Data object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Return an empty Milestone object if the MilestoneId and VisitId are not empty. 
    /// 
    /// 2. Define sql query parameters and sql query string. 
    /// 
    /// 3. Execute the sql query string and store the results on data table. 
    /// 
    /// 4. Extract the first row data to the return milestone object. 
    /// 
    /// 5. Add the activities to the milestone object, if they exist. 
    /// 
    /// 6. Return the Milestone data object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvMilestone getMilestone (
      Guid ScheduleGuid,
      String MilestoneId )
    {
      //
      // Initialize the debug log, a sql query string, a return milestone object, a milestone activities object. 
      //
      this.LogValue (   Evado.Model.EvStatics.CONST_METHOD_START
        + "Evado.Dal.Clinical.EvMilestones.getMilestone method. " );
      this.LogValue ( "ScheduleGuid: " + ScheduleGuid );
      this.LogValue ( "MilestoneId: " + MilestoneId );

      string sqlQueryString;
      EvMilestone milestone = new EvMilestone ( );
      EvMilestoneActivities activities = new EvMilestoneActivities ( );

      // 
      // Validate whether the MilestoneId and VisitId are not empty. 
      // 
      if ( MilestoneId == String.Empty
        && ScheduleGuid == Guid.Empty )
      {
        return milestone;
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( PARM_ScheduleGuid, SqlDbType.UniqueIdentifier),
        new SqlParameter( PARM_MilestoneId, SqlDbType.Char, 20),
      };
      cmdParms [ 0 ].Value = ScheduleGuid;
      cmdParms [ 1 ].Value = MilestoneId;

      foreach ( SqlParameter parm in cmdParms )
      {
        this.LogValue ( "PN:" + parm.ParameterName + ",  T:" + parm.DbType + ", V:" + parm.Value );
      }

      // 
      // Generate the SQL query string
      // 
      sqlQueryString = _sqlQuery_View + " WHERE (SCH_GUID = @ScheduleGuid) "
          + "AND (MilestoneId = @MilestoneId);";

      this.LogValue (   sqlQueryString );

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
          this.LogValue ( "Zero rows returned. " );
          return milestone;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        milestone = readRow ( row );

        this.LogValue ( "MilestoneId: " + milestone.MilestoneId );
        this.LogValue ( "Title: " + milestone.Title );

        // 
        // Get the Milestone activities.
        // 
        milestone.ActivityList = activities.getActivityList ( milestone.Guid, EvActivity.ActivityTypes.Null, false, true );

      }//END using statement

      // 
      // Return the EvMilestone data object.
      // 
      return milestone;

    }//END getMilestone method

    #endregion

    #region Milestone Update methods

    // =====================================================================================
    /// <summary>
    /// This class updates values to the milestone data table. 
    /// </summary>
    /// <param name="Milestone">EvMilestone: a milestone data object</param>
    /// <returns>EvEventCodes: an event code for updating milestone table result</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the old Milestone's Guid is empty and the New Milestone identifier is duplicated. 
    /// 
    /// 2. Set the Milestone ScheduleId to the default value, if it is null
    /// 
    /// 3. Append new items to the datachange object if they do not exist.
    /// 
    /// 5. Append new items from Activities list to datachange object if they do not exist. 
    /// 
    /// 8. Define sql query parameters and execute the storeprocedure for updating items. 
    /// 
    /// 9. Add datachange object values to the backup datachanges object
    /// 
    /// 9. Return an event code for updating items. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes updateMilestone ( EvMilestone Milestone )
    {
      //
      // Initialize debug log, a return event code, a milestone activities object and an old milestone object
      //
      this.LogMethod ( "updateMilestone. " );
      this.LogValue ( "ProjectId: " + Milestone.ProjectId );
      this.LogValue ( "MilestoneId: " + Milestone.MilestoneId );
      this.LogValue ( "Title: " + Milestone.Title );
      this.LogValue ( "Order: " + Milestone.Order );
      this.LogValue ( "Activity count: " + Milestone.ActivityList.Count );

      EvEventCodes iReturn = EvEventCodes.Ok;
      EvMilestoneActivities milestoneActivities = new EvMilestoneActivities ( );

      //
      // If the milestone is null set to the default of Clinical.
      //
      if ( Milestone.Type == EvMilestone.MilestoneTypes.Null )
      {
        Milestone.Type = EvMilestone.MilestoneTypes.Clinical;
      }

      EvMilestone oldMilestone = this.getMilestone ( Milestone.Guid );

      //
      // Validate whether the OldMilestone's Guid is not empty.
      //
      if ( oldMilestone.Guid == Guid.Empty )
      {
        return EvEventCodes.Data_InvalidId_Error;
      }

      //
      // Validate that the milestone id is unique.
      //
      this.LogValue ( "Validating milstone id" );

      List<EvOption> milestoneList = this.milestoneValidationList (
        Milestone.ProjectId,
        Milestone.ScheduleGuid );

      //
      // Loop through the milestone list. 
      //
      foreach ( EvOption milestone in milestoneList )
      {
        //
        // IF the old milestone Id does not match the update milestoneId 
        // it has been changed. So the new milestone id needs to be validated.
        //
        if ( oldMilestone.MilestoneId.ToLower ( ) != Milestone.MilestoneId.ToLower ( )
          && milestone.Value.ToLower ( ) == Milestone.MilestoneId.ToLower ( ) )
        {
          this.LogValue ( "Duplicate milestone Id." );

          return EvEventCodes.Data_Duplicate_Id_Error;
        }
      }//EMD milestone iteration loop.

      // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      // Compare the objects.
      // Initialize datachange object and a backup datachanges object. 
      // 
      EvDataChanges dataChanges = new EvDataChanges ( );
      EvDataChange dataChange = setChangeRecord ( Milestone, oldMilestone );

      // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      // 
      // Define the SQL query parameters and load the query values.
      // 
      this.LogValue ( "Update EvMilestone details" );

      SqlParameter [ ] _cmdParms = getItemsParameters ( );
      SetParameters ( _cmdParms, Milestone );

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( _storedProcedureUpdateItem, _cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      // 
      // Update the Milestone activities list.
      // 
      iReturn = milestoneActivities.updateActivities ( Milestone );

      this.LogValue (   milestoneActivities.DebugLog );

      if ( iReturn < EvEventCodes.Ok )
      {
        this.LogValue ( "Activities update failed." );

        return iReturn;
      }

      // 
      // Add the change record
      // 
      dataChanges.AddItem ( dataChange );

      // 
      // Return code
      // 
      return EvEventCodes.Ok;

    }//END updateMilestone class.
    // ==================================================================================
    /// <summary>
    /// This method sets the data change object for the update action.
    /// </summary>
    /// <param name="Milestone">EvMilestone: a current milestone object</param>
    /// <param name="OldMilestone">EvMilestone: an old milestone object</param>
    /// <returns>EvDataChange: a change record object</returns>
    /// <remarks>
    /// This method consits of the following steps:
    /// 
    /// 1. Add items from Activity object to the datachange object, 
    /// if they do not exist in the old activity object.
    /// 
    /// 2. Return the data change object.
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public EvDataChange setChangeRecord ( EvMilestone Milestone, EvMilestone OldMilestone )
    {
      // 
      // Initialise the datachange object.
      // 
      EvDataChange dataChange = new EvDataChange ( );
      dataChange.TableName = EvDataChange.DataChangeTableNames.EvMilestones;
      dataChange.RecordGuid = Milestone.Guid;
      dataChange.TrialId = Milestone.ProjectId;
      dataChange.UserId = Milestone.UpdatedByUserId;
      dataChange.DateStamp = DateTime.Now;

      //
      // Append new items to the datachange object if they do not exist. 
      //
      if ( Milestone.MilestoneId != OldMilestone.MilestoneId )
      {
        dataChange.AddItem ( "MilestoneId", OldMilestone.MilestoneId, Milestone.MilestoneId );
      }
      if ( Milestone.Title != OldMilestone.Title )
      {
        dataChange.AddItem ( "Title", OldMilestone.Title, Milestone.Title );
      }
      if ( Milestone.Order != OldMilestone.Order )
      {
        dataChange.AddItem ( "Order", OldMilestone.Order.ToString ( ), Milestone.Order.ToString ( ) );
      }
      if ( Milestone.Description != OldMilestone.Description )
      {
        dataChange.AddItem ( "Description", OldMilestone.Description, Milestone.Description );
      }

      if ( OldMilestone.MilestoneRange != Milestone.MilestoneRange )
      {
        dataChange.AddItem ( "VisitPeriod",
          OldMilestone.MilestoneRange.ToString ( ),
          Milestone.MilestoneRange.ToString ( ) );
      }

      if ( OldMilestone.InterMilestonePeriod != Milestone.InterMilestonePeriod )
      {
        dataChange.AddItem ( "InterVisitPeriod_In_Days",
          OldMilestone.InterMilestonePeriod.ToString ( ),
          Milestone.InterMilestonePeriod.ToString ( ) );
      }

      //
      // Append new xml data if it does not exist. 
      //
      string oldXmlData =
        Evado.Model.EvStatics.SerialiseObject<EvMilestoneData> ( OldMilestone.Data );

      string newXmlData =
        Evado.Model.Digital.EvcStatics.SerialiseObject<EvMilestoneData> ( Milestone.Data );

      if ( oldXmlData != newXmlData )
      {
        dataChange.AddItem ( "XmlData", oldXmlData, newXmlData );
      }

      // 
      // Update the milestone activities.
      // 
      foreach ( EvActivity newActivity in Milestone.ActivityList )
      {
        // 
        // Get the old milestone
        // 
        EvActivity oldActivity = this.getActivity ( OldMilestone.ActivityList, newActivity.ActivityId );

        // 
        // Update the object properties.
        // 
        if ( newActivity.ActivityId != oldActivity.ActivityId )
        {
          dataChange.AddItem ( "ACT_" + newActivity.ActivityId,
            oldActivity.ActivityId,
            newActivity.ActivityId );
        }

        // 
        // Update the object properties.
        // 
        if ( newActivity.Order != oldActivity.Order )
        {
          dataChange.AddItem ( "ACT_" + newActivity.ActivityId + "_Order",
            oldActivity.Order.ToString ( ),
            newActivity.Order.ToString ( ) );
        }

        // 
        // Update the object properties.
        // 
        if ( newActivity.IsMandatory != oldActivity.IsMandatory )
        {
          dataChange.AddItem ( "ACT_" + newActivity.ActivityId + "_Mandatory",
            oldActivity.IsMandatory.ToString ( ),
            newActivity.IsMandatory.ToString ( ) );
        }
      }//END Activity interation loop.


      return dataChange;
    }

    // =====================================================================================
    /// <summary>
    /// This method returns the selected Actvity from a list of activities.
    /// </summary>
    /// <param name="ActivityList">List of EvActivity: The list of activities</param>
    /// <param name="ActivityId">String: The activity to be found</param>
    /// <returns>EvActivity: an activity data object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Loop through the Activity list
    /// 
    /// 2. Return the matching activity if it is found. 
    /// 
    /// 3. Else return new Activity object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private EvActivity getActivity ( List<EvActivity> ActivityList, String ActivityId )
    {
      foreach ( EvActivity activity in ActivityList )
      {
        // 
        // Return the matching newField.
        // 
        if ( activity.ActivityId == ActivityId )
        {
          return activity;
        }
      }
      // 
      // IF none return emptyh object.
      // 
      return new EvActivity ( );
    }//END getActivity class. 

    // =====================================================================================
    /// <summary>
    /// This class adds new items to the milestone data table.
    /// </summary>
    /// <param name="Milestone">EvMilestone: a milestone data object</param>
    /// <returns>EvEventCodes: an event code for adding result.</returns>
    /// <remarks>
    /// This class consists of the following steps: 
    /// 
    /// 2. Exit, if the new Milestone Identifier is duplicated.  
    /// 
    /// 3. Creat the new Milestone Guid
    /// 
    /// 4. Define sql query parameters and execute the storeprocedure for adding items to milestone table. 
    /// 
    /// 5. Execute the storeprocedure for adding new milestone activities if they exist. 
    /// 
    /// 6. Return the event code for adding items. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes addMilestone ( EvMilestone Milestone )
    {
      //
      // Initialize the debug log, a return event code, a milestone activities object and sql query string. 
      //
      this.LogMethod ( "addMilestone. " );
      this.LogValue ( "ProjectId: " + Milestone.ProjectId );
      this.LogValue ( "MilestoneId: " + Milestone.MilestoneId );
      this.LogValue ( "Title: " + Milestone.Title );
      this.LogValue ( "Order: " + Milestone.Order );

      EvEventCodes iReturn = EvEventCodes.Ok;
      EvMilestoneActivities milestoneActivities = new EvMilestoneActivities ( );
      string sqlQueryString = String.Empty;

      // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      //
      // Validate that the milestone id is unique.
      //
      this.LogValue ( "Validating milstone id" );

      EvMilestone oldMilestone = this.getMilestone (
        Milestone.ScheduleGuid,
        Milestone.MilestoneId );

      if ( oldMilestone.Guid != Guid.Empty )
      {
        this.LogValue ( "ERROR: " + EvEventCodes.Data_Duplicate_Id_Error );
        return EvEventCodes.Data_Duplicate_Id_Error;
      }

      // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      // 
      // Creat the new Milestone Guid.
      // 
      Milestone.Guid = Guid.NewGuid ( );
      this.LogValue ( "Milestone Guid: " + Milestone.Guid );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] _cmdParms = getItemsParameters ( );
      SetParameters ( _cmdParms, Milestone );

      this.LogValue ( "Adding Milestone" );
      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( _storedProcedureAddItem, _cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }


      this.LogValue ( "Adding Activities, count: " + Milestone.ActivityList.Count );
      // 
      // Update Milestone activities.
      // 
      if ( Milestone.ActivityList.Count > 0 )
      {
        // 
        // Reset the Guids and save action.
        // 
        for ( int count = 0; count < Milestone.ActivityList.Count; count++ )
        {
          Milestone.ActivityList [ count ].Guid = Guid.Empty;
          Milestone.ActivityList [ count ].Action = EvActivity.ActivitiesActionsCodes.Save;
        }

        // 
        // Update the Milestone activities list.
        // 
        iReturn = milestoneActivities.updateActivities ( Milestone );
        this.LogValue (  milestoneActivities.DebugLog );

        if ( iReturn < EvEventCodes.Ok )
        {
          this.LogValue ( "ERROR RETURN = " + iReturn );
          return iReturn;
        }

      }//END update Activities.
      this.LogValue ( "Activities added" );

      return EvEventCodes.Ok;

    }//END addMilestone class

    // =====================================================================================
    /// <summary>
    /// This class deletes items from the milestone data table. 
    /// </summary>
    /// <param name="Milestone">EvMilestone: a milestone data object</param>
    /// <returns>EvEventCodes: an event code for adding result</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 2. Exit, if the Milestone Uid is not unique or UserCommon Name is empty.. 
    /// 
    /// 3. Define the sql query parameters and execute the storeprocedure for deleting milestone's items. 
    /// 
    /// 4. Execute the storeprocedure for deleting related milstone's activities. 
    /// 
    /// 5. Return an event code for deleting items. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes deleteMilestone ( EvMilestone Milestone )
    {
      //
      // Initialize the debug log, a milestone activities object
      //
      this.LogMethod ( "deleteMilestone. " );

      EvMilestoneActivities activities = new EvMilestoneActivities ( );

      // 
      // Validate whether the Milestone Uid and UserCommon Name are valid. 
      // 
      if ( Milestone.Guid == Guid.Empty )
      {
        return EvEventCodes.Identifier_Global_Unique_Identifier_Error;
      }

      if ( Milestone.UserCommonName == String.Empty )
      {
        return EvEventCodes.Identifier_User_Id_Error;
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( PARM_Guid, SqlDbType.UniqueIdentifier),
        new SqlParameter( PARM_UpdatedByUserId, SqlDbType.NVarChar,100),
        new SqlParameter( PARM_UpdatedBy, SqlDbType.NVarChar,100),
        new SqlParameter( PARM_UpdateDate, SqlDbType.DateTime)
      };
      cmdParms [ 0 ].Value = Milestone.Guid;
      cmdParms [ 1 ].Value = Milestone.UpdatedByUserId;
      cmdParms [ 2 ].Value = Milestone.UserCommonName;
      cmdParms [ 3 ].Value = DateTime.Now;

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( _storedProcedureDeleteItem, cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      if ( Milestone.ActivityList.Count > 0 )
      {
        String SqlQuery = "/** DELETE ALL OF MILESTONE ACTIVITIES PREPARAITON ROW FOR MILESTONE **/"
        + " DELETE FROM EvMilestoneActivities_Prep "
        + " WHERE  M_Guid = '" + Milestone.Guid + "' ; ";

        if ( EvSqlMethods.QueryUpdate ( SqlQuery, null ) == 0 )
        {
          return EvEventCodes.Database_Record_Update_Error;
        }
      }
      return EvEventCodes.Ok;

    }//END deleteMilestone method


    #endregion

    #region Static Class methods.

    // ==================================================================================
    /// <summary>
    /// This public static method converts the milestone validation rules from the 
    /// old structure into the new structure for version 4.0.1
    /// </summary>
    /// <param name="milestone">EvMilestone: a milestone object</param>
    /// <param name="validationRules">EvMilestoneValidationRules: a milestone validation rules</param>
    /// <returns>string: a debug log string</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. If the validation rules do not exist, set the milestone's inter visit period and 
    /// visit period to the default value. 
    /// 
    /// 2. Else, set to the new structure values
    /// 
    /// 3. Add the values to the debug log. 
    /// 
    /// 4. Return the debug log string. 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public static string updateMilestoneValidationRules (
      EvMilestone milestone, EvMilestoneValidationRules validationRules )
    {
      //
      // Initialize the debug log. 
      //
      string debugLog = Evado.Model.Digital.EvcStatics.CONST_METHOD_START
       + "Evado.Dal.Clinical.EvMilestones.updateMilestoneValidationRules method "
       + " \r\n Visit period not set. Update from validation rules."
       + " \r\n Milestone: " + milestone.MilestoneId
       + "\r\n MinimumDaysFromPreviousVisit: " + validationRules.MinimumDaysFromPreviousVisit
       + "\r\n MaximumDaysFromPreviousVisit: " + validationRules.MaximumDaysFromPreviousVisit;

      if ( ( validationRules.MinimumDaysFromPreviousVisit == 0
          && validationRules.MaximumDaysFromPreviousVisit == 0 )
        || ( validationRules.MinimumDaysFromPreviousVisit == 0
          && validationRules.MaximumDaysFromPreviousVisit == 365 )
        || ( validationRules.MinimumDaysFromPreviousVisit == 0
          && validationRules.MaximumDaysFromPreviousVisit == 1095 ) )
      {
        debugLog += " Set schedule to default setting.";

        //
        // Set the inter visit period to the default.
        //
        milestone.InterMilestonePeriod = 0;
        milestone.MilestoneRange = 0;

        //
        // Set validation rules to null.
        //
        //milestone.ValidationRules = null;

        return debugLog;
      }
      else
      {
        if ( validationRules.MinimumDaysFromPreviousVisit > validationRules.MaximumDaysFromPreviousVisit )
        {
          debugLog += " Min greater than max set to default value.";

          //
          // Set the inter visit period to the default.
          //
          milestone.InterMilestonePeriod = 0;
          milestone.MilestoneRange = 0;

          //
          // Set validation rules to null.
          //
          //milestone.ValidationRules = null;

          return debugLog;
        }

        debugLog += " Validation rules set so update appropriately.";

        //
        // Determine the inter visit period in days.
        //
        milestone.InterMilestonePeriod = (
          validationRules.MaximumDaysFromPreviousVisit
          + validationRules.MinimumDaysFromPreviousVisit ) / 2;

        debugLog += " InterVisitPeriod_In_Days: " + milestone.InterMilestonePeriod;

        //
        // Determine the period within with the visit must occur.
        //
        milestone.MilestoneRange = ( validationRules.MaximumDaysFromPreviousVisit
          - validationRules.MinimumDaysFromPreviousVisit ) / 2;

        debugLog += " milestone VisitPeriod: " + milestone.MilestoneRange;

        //
        // Set validation rules to null.
        //
        //milestone.ValidationRules = null;
      }

      //
      // Return the debug log. 
      //
      return debugLog;
    }

    #endregion

  }//END EvMilestones class

}//END namespace Evado.Dal
