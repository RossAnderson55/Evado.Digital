/***************************************************************************************
 * <copyright file="dal\EvActivities.cs" company="EVADO HOLDING PTY. LTD.">
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
  /// This class handles the data access processes for the Milestone Activity objects.
  /// </summary>
  public class EvMilestoneActivities
  {
    #region Class constants and variable initialisation

    private const string _sqlQuery_View = "Select *  FROM EV_MILESTONE_ACTIVITY_VIEW ";

    // 
    // The SQL Store Procedure constants
    // 

    private const string _parmGuid = "@Guid";
    private const string _parmMilestoneGuid = "@MilestoneGuid";
    private const string _parmActivityId = "@ActivityId";
    private const string _parmOrder = "@Order";
    private const string _parmIsMandatory = "@IsMandatory";

    private const string _parmMilestoneId = "@MilestoneId";
    private const string _parmScheduleId = "@ScheduleId";
    private const string _parmTrialId = "@TrialId";
    private const string _parmType = "@Type";
    private const string _parmUpdateByUserId = "@UpdateByUserId";
    private const string _parmUpdatedBy = "@UpdatedBy";
    private const string _parmUpdatedDate = "@UpdatedByDate";


    private System.Text.StringBuilder _DebugLog = new System.Text.StringBuilder ( );

    /// <summary>
    /// This property contains the method debug log
    /// </summary>
    public string DebugLog
    {
      get
      {
        return _DebugLog.ToString ( );
      }
    }

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region SQL Parameter methods

    // =====================================================================================
    /// <summary>
    /// This class sets the array of sql query parameters. 
    /// </summary>
    /// <returns>SqlParameter: an array of sql query parameters</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Create the array of sql query parameters. 
    /// 
    /// 2. Return the array of sql query parameters.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private static SqlParameter [ ] getItemsParameters ( )
    {
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( _parmGuid, SqlDbType.UniqueIdentifier ),
        new SqlParameter( _parmMilestoneGuid, SqlDbType.UniqueIdentifier),
        new SqlParameter( _parmActivityId, SqlDbType.NVarChar, 20),
        new SqlParameter( _parmOrder, SqlDbType.SmallInt),
        new SqlParameter( _parmIsMandatory, SqlDbType.Bit),
      };

      return cmdParms;
    }//END getItemsParameters class

    // =====================================================================================
    /// <summary>
    /// This class binds values from activity object to the parameters array. 
    /// </summary>
    /// <param name="cmdParms">SqlParameter: an array of sql query parameters</param>
    /// <param name="Activity">EvActivity: Values to bind to parameters</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Update the values from Activity object to the array of sql query parameters. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private void SetParameters ( SqlParameter [ ] cmdParms, EvActivity Activity )
    {

      cmdParms [ 0 ].Value = Activity.Guid;
      cmdParms [ 1 ].Value = Activity.MilestoneGuid;
      cmdParms [ 2 ].Value = Activity.ActivityId;
      cmdParms [ 3 ].Value = Activity.Order;
      cmdParms [ 5 ].Value = Activity.IsMandatory;

    }//END SetParameters class.

    #endregion

    #region Data Reader methods

    // =====================================================================================
    /// <summary>
    /// This class extracts data row values to the Activity object. 
    /// </summary>
    /// <param name="Row">DataRow: a data row object</param>
    /// <returns>EvActivity: an activity object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Extract the compatible data row values to the activity object. 
    /// 
    /// 2. Return the Activity data object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvActivity readDataRow ( DataRow Row )
    {
      // 
      // Initialise the return activity object. 
      // 
      EvActivity activity = new EvActivity ( );

      // 
      // Extract the data object values.
      // 
      activity.Guid = EvSqlMethods.getGuid ( Row, "MA_Guid" );
      activity.MilestoneActivityGuid = activity.Guid;
      activity.MilestoneGuid = EvSqlMethods.getGuid ( Row, "M_Guid" );
      activity.ProjectId = EvSqlMethods.getString ( Row, "TrialId" );
      activity.ActivityId = EvSqlMethods.getString ( Row, "ActivityId" );
      activity.Title = EvSqlMethods.getString ( Row, "AC_Title" );
      activity.Description = EvSqlMethods.getString ( Row, "AC_Description" );
      activity.Order = EvSqlMethods.getInteger ( Row, "MA_Order" );
      activity.Quantity = 1;
      activity.IsMandatory = EvSqlMethods.getBool ( Row, "MA_IsMandatory" );

      String stType = EvSqlMethods.getString ( Row, "AC_Type" );

      //
      // Update the type enumerated values
      //
      if ( stType == "Procedure"
        || stType == "Investigation"
        || stType == "Laboratory_Test"
        || stType == "Other" )
      {
        stType = "Clinical";
      }

      activity.Type = (EvActivity.ActivityTypes) Enum.Parse ( typeof ( EvActivity.ActivityTypes ), stType );

      //
      // Return the activity object.
      //
      return activity;

    }//End readDataRow method.

    #endregion

    #region Activity Preparation Query methods

    // =====================================================================================
    /// <summary>
    /// This method retrieves a list of activity objects for the selected milestone.
    /// </summary>
    /// <param name="MilestoneGuid">Guid: (Mandatory) The Milestone's global Unique identifier</param>
    /// <param name="Types">List of EvActivity.ActivityTypes: (Mandatory) List of the activity types</param>
    /// <returns>List of EvActivity: a list of activity object.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define the sql query parameters and sql query string. 
    /// 
    /// 2. Execute the sql query string and store the results on data table. 
    /// 
    /// 3. Loop through the table and extract the data row values to the activity object. 
    /// 
    /// 4. Add the activity object values to the Activities list. 
    /// 
    /// 5. Return the Activities list
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvActivity> getActivityList (
      Guid MilestoneGuid,
      List<EvActivity.ActivityTypes> Types )
    {
      //
      // Initialize the debug log, an sql query string, a return activity list and the number of activity order.
      //
      this._DebugLog = new System.Text.StringBuilder ( );
      this._DebugLog.AppendLine ( Evado.Model.EvStatics.CONST_METHOD_START
        + "Evado.Dal.Clinical.EvMilestoneActivitIes.getActivityList. " );
      this._DebugLog.AppendLine ( "MilestoneGuid: " + MilestoneGuid );
      this._DebugLog.AppendLine ( "Type: " + Types.Count );

      string sqlQueryString;
      List<EvActivity> view = new List<EvActivity> ( );
      int activityOrder = 3;

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( _parmMilestoneGuid, SqlDbType.UniqueIdentifier),
      };
      cmdParms [ 0 ].Value = MilestoneGuid;

      // 
      // Generate the SQL query string
      // 
      sqlQueryString = _sqlQuery_View + "WHERE M_Guid = @MilestoneGuid ";

      if ( Types.Count > 0 )
      {
        if ( Types.Count == 1 && Types [ 0 ] != EvActivity.ActivityTypes.Null )
        {
          sqlQueryString += " AND AC_Type = '" + Types [ 0 ].ToString ( ) + "' ";
        }
        else
        {
          string sTypeSelection = String.Empty;
          foreach ( EvActivity.ActivityTypes type in Types )
          {
            if ( sTypeSelection != String.Empty )
            {
              sTypeSelection += " OR ";
            }
            sTypeSelection += " (AC_Type = '" + type.ToString ( ) + "') ";
          }
          sqlQueryString += " AND ( " + sTypeSelection + ") ";
        }
      }
      sqlQueryString += " ORDER BY MA_Order; ";

      this._DebugLog.AppendLine ( sqlQueryString );

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

          EvActivity activity = this.readDataRow ( row );

          activity.Selected = true;

          activity.Order = activityOrder;

          // 
          // Append the value to the visit
          // 
          view.Add ( activity );

          //
          // Increment the order
          //
          activityOrder += 3;

        } //END interation loop.

      }//END using method

      this._DebugLog.AppendLine ( "Activity count: " + view.Count.ToString ( ) );
      // 
      // Return the ArrayList containing the EvActivity data object.
      // 
      return view;

    }//END getPreparationList method.

    #endregion

    #region SubjectMileststoneActivities Query methods

    // =====================================================================================
    /// <summary>
    /// This method retrieves a list of Milestone activity objects based on the passed parameters.
    /// </summary>
    /// <param name="MilestoneGuid">Guid: (Mandatory) The MIlestone's global unique identifier</param>
    /// <param name="Type">EvActivity.ActivityTypes: (Mandatory) The activity type to be selected.</param>
    /// <param name="NotType">Boolean: (Mandatory) True: Do not select the type.</param>
    /// <param name="WithForms">Boolean: (Mandatory) True: append the activity forms and tests</param>
    /// <returns>List of EvActivity: a list contains activity data object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define the sql query parameters and sql query string. 
    /// 
    /// 2. Execute the sql query string and store the results on data table. 
    /// 
    /// 3. Loop through the table and extract the data row to the Activity object
    /// 
    /// 4. Add the activity object to the Activities list. 
    /// 
    /// 5. Return the Activities list.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvActivity> getActivityList (
      Guid MilestoneGuid,
      EvActivity.ActivityTypes Type,
      bool NotType,
      bool WithForms )
    {
      this._DebugLog.AppendLine ( Evado.Model.EvStatics.CONST_METHOD_START
        + "Evado.Dal.Clinical.EvMilestoneActivites.getActivityList. " );
      this._DebugLog.AppendLine ( "MilestoneGuid: " + MilestoneGuid );
      this._DebugLog.AppendLine ( "Type: " + Type );
      this._DebugLog.AppendLine ( "NotType: " + NotType );
      this._DebugLog.AppendLine ( "WithForms: " + WithForms );

      //
      // Initialize the method debug log, a sql query string, a return activity list and an activity form object. 
      //
      string sqlQueryString;
      List<EvActivity> activityList = new List<EvActivity> ( );
      EvActivityForms activityForms = new EvActivityForms ( );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( _parmMilestoneGuid, SqlDbType.UniqueIdentifier),
      };
      cmdParms [ 0 ].Value = MilestoneGuid;

      // 
      // Generate the SQL query string
      // 
      sqlQueryString = _sqlQuery_View + "WHERE (M_Guid = @MilestoneGuid ) ";

      // 
      // Insert the type selection clauses
      // 
      if ( Type != EvActivity.ActivityTypes.Null )
      {
        if ( NotType == true )
        {
          sqlQueryString += " AND NOT(AC_Type = '" + Type.ToString ( ) + "') ";
        }
        else
        {
          sqlQueryString += " AND (AC_Type = '" + Type.ToString ( ) + "') ";
        }
      }

      sqlQueryString += " ORDER BY MA_Order; ";

      this._DebugLog.AppendLine ( sqlQueryString );

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

          EvActivity activity = this.readDataRow ( row );

          activity.Selected = true;

          // 
          // IF the with forms is true retrieve the form list.
          // Used by the registry module.
          // 
          if ( WithForms == true )
          {
            activity.FormList = activityForms.getForms ( 
              activity.ProjectId, 
              activity.ActivityId );

            this._DebugLog.AppendLine ( "activityForms.DebugLog: " + activityForms.Log );
          }
          this._DebugLog.AppendLine ( "activity.FormList count: " + activity.FormList.Count );

          // 
          // Append the value to the visit
          // 
          activityList.Add ( activity );

        } //END interation loop.

      }//END using method

      this._DebugLog.AppendLine ( "view object count: " + activityList.Count );

      // 
      // Return the ArrayList containing the EvActivity data object.
      // 
      return activityList;

    }//END getList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of Milestone activity object items for the selected MilestoneId
    /// </summary>
    /// <param name="ProjectId">string: (Mandatory) The Project identifier</param>
    /// <param name="MilestoneId">string: (Mandatory) The Milestone's identifier</param>
    /// <param name="Type">EvActivity.ActivityTypes: (Mandatory) The activity type to be selected.</param>
    /// <param name="ScheduleId">EvTrialArm.ScheduleIdes: an ScheduleId object</param>
    /// <param name="WithForms">Boolean: (Mandatory) True: append the activity forms and tests</param>
    /// <returns>List of EvActivity: a list of activity</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Get the list of activity records for the selected trialId
    /// 
    /// 2. Define the sql query parameters and sql query string. 
    /// 
    /// 3. Execute the sql query string and store the results on data table. 
    /// 
    /// 4. Loop through the table and extract the data row to the activity object. 
    /// 
    /// 5. Add the activity records to the activity object. 
    /// 
    /// 6. Add the activity object values to the Activities list. 
    /// 
    /// 7. Return the Activities list.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvActivity> getActivityList (
      String ProjectId,
      String MilestoneId,
      int ScheduleId,
      EvActivity.ActivityTypes Type,
      bool WithForms )
    {
      //
      // Initialize the debug log, an activityform object, a sql query string and a return list of activity. 
      //
      this._DebugLog.AppendLine ( Evado.Model.EvStatics.CONST_METHOD_START
        + "Evado.Dal.Clinical.EvMilestoneActivites.getActivityList. " );
      this._DebugLog.AppendLine ( "ProjectId: " + ProjectId );
      this._DebugLog.AppendLine ( "MilestoneId: " + MilestoneId );
      this._DebugLog.AppendLine ( "ScheduleId: " + ScheduleId );
      this._DebugLog.AppendLine ( "Type: " + Type );

      EvActivityForms activityForms = new EvActivityForms ( );
      string sqlQueryString;
      List<EvActivity> activityList = new List<EvActivity> ( );

      //
      // Get the list of activity records for the selected trialId
      //
      List<EvActvityForm> formList = activityForms.getForms ( ProjectId, String.Empty );
      this._DebugLog.AppendLine ( "ActivityForms status: " + activityForms.Log );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( _parmTrialId, SqlDbType.VarChar,10 ),
        new SqlParameter( _parmMilestoneId, SqlDbType.VarChar,20 ),
        new SqlParameter( _parmScheduleId, SqlDbType.Int),
      };
      cmdParms [ 0 ].Value = ProjectId;
      cmdParms [ 1 ].Value = MilestoneId;
      cmdParms [ 2 ].Value = (int) ScheduleId;

      // 
      // Generate the SQL query string
      // 
      sqlQueryString = _sqlQuery_View + "WHERE TrialId = @TrialId AND MilestoneId = @MilestoneId "
        + " AND ARM_Index = @ScheduleId ";

      // 
      // Insert the type selection clauses
      // 
      if ( Type != EvActivity.ActivityTypes.Null )
      {
        if ( Type == EvActivity.ActivityTypes.Clinical )
        {
          sqlQueryString += " AND ( "
            + " (AC_Type = '" + EvActivity.ActivityTypes.Clinical + "') "
            + " OR (AC_Type = 'Procedure') "
            + " OR (AC_Type = 'Investigation') "
            + " OR (AC_Type = 'Laboratory_Test') "
            + " OR (AC_Type = 'Other') "
            + ")";
        }
        else
        {
          sqlQueryString += " AND (AC_Type = '" + Type.ToString ( ) + "') ";
        }
      }

      sqlQueryString += " ORDER BY MA_Order; ";

      this._DebugLog.AppendLine ( sqlQueryString );

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
          EvActivity activity = this.readDataRow ( row );

          this._DebugLog.AppendLine ( activity.ActivityId + " - " + activity.Title );

          // 
          // Add the forms associated with this Activity
          // 
          activity.FormList = this.getForms ( formList, activity.ActivityId );

          // 
          // Append the value to the visit
          // 
          activityList.Add ( activity );

        } //END interation loop.

      }//END using method

      this._DebugLog.AppendLine ( "view object count: " + activityList.Count.ToString ( ) );
      // 
      // Return the ArrayList containing the EvActivity data object.
      // 
      return activityList;

    }//END getList method.

    // =====================================================================================
    /// <summary>
    /// This method gets the list of ActivityRecords that match the activitId from the form list.
    /// </summary>
    /// <param name="FormList">List of EvActivityForm: a list of activity record object</param>
    /// <param name="ActivityId">String: The activity id to select the forms.</param>
    /// <returns>List of EvActivityForm: a list of activity object.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Iterate through the queried list for selecting the tests that match the activity.
    /// 
    /// 2. Return the matched activity record list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private List<EvActvityForm> getForms ( List<EvActvityForm> FormList, String ActivityId )
    {
      //
      // initialist the return list of activity record object
      //
      List<EvActvityForm> list = new List<EvActvityForm> ( );

      //
      // Iteration through the list selecting the forms that match the activity.
      //
      foreach ( EvActvityForm form in FormList )
      {
        if ( form.ActivityId == ActivityId )
        {
          list.Add ( form );
        }
      }

      this._DebugLog.AppendLine ( ", form count: " + list.Count );

      // 
      // Return the list of tests
      //
      return list;

    }//END getForms method

    #endregion

    #region Activity Retrieval methods

    // =====================================================================================
    /// <summary>
    /// This method retrieves a selected Activity object based on Guid and Optionation condition.
    /// </summary>
    /// <param name="Guid">Guid: The Activity global unique identifier</param>
    /// <returns>EvActivity: an activity data object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Return an empty Activity object if the Guid is empty
    /// 
    /// 2. Define the sql query parameters and sql query string. 
    /// 
    /// 3. Execute the sql query string and store the results on data table 
    /// 
    /// 4. Extract the fist data row to the activity object. 
    /// 
    /// 5. Return the Activity Object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvActivity getActivity ( Guid Guid )
    {
      //
      // Initialize the debug log, a local sql query string and a return activity object. 
      //
      this._DebugLog.AppendLine ( Evado.Model.Digital.EvcStatics.CONST_METHOD_START
        + "Evado.Dal.Clinical.EvMilestoneActivites.getActivity. " );
      this._DebugLog.AppendLine ( "Guid: " + Guid );

      string sqlQueryString;
      EvActivity activity = new EvActivity ( );

      // 
      // Validate whether the Guid is not empty.
      // 
      if ( Guid == Guid.Empty )
      {
        return activity;
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter cmdParms = new SqlParameter ( _parmGuid, SqlDbType.UniqueIdentifier );
      cmdParms.Value = Guid;

      // 
      // Generate the SQL query string
      // 
      sqlQueryString = _sqlQuery_View + " WHERE (MA_Guid = '" + Guid.ToString ( ) + "');";

      this._DebugLog.AppendLine ( sqlQueryString );

      //
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, null ) )
      {
        // 
        // If not rows the return
        // 
        if ( table.Rows.Count == 0 )
        {
          return activity;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        activity = this.readDataRow ( row );

      }//END Using 

      // 
      // Return the EvActivity data object.
      // 
      return activity;

    }//END getActivity class

    #endregion

    #region Activity Update methods

    // =====================================================================================
    /// <summary>
    /// This method updates the Milestone's activities by iterating through the passes newField list.
    /// </summary>
    /// <param name="Milestone"> EvMilestone object.</param>
    /// <returns>EvEventCodes an event code for updating activity object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Iterate through each newField in the activity list for
    /// Updating those activities that have changed.
    /// 
    /// 2. If the Activity Guid is empty, add new items to the activity table. 
    /// 
    /// 3. If the action is save, update the activity items to the activity table. 
    /// 
    /// 4. If the action is delete, delete actvity items from teh actvity table. 
    /// 
    /// 5. Return the event code for updating the items. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes updateActivities (
      EvMilestone Milestone )
    {
      this._DebugLog.AppendLine ( Evado.Model.Digital.EvcStatics.CONST_METHOD_START
        + "Evado.Dal.Clinical.EvMilestoneActivities.updateActivities." );
      this._DebugLog.AppendLine ( "MilestoneGuid: " + Milestone.Guid );
      this._DebugLog.AppendLine ( "list count: " + Milestone.ActivityList.Count );

      //
      // Initialize the debug log and the return event code. 
      //
      System.Text.StringBuilder SqlUpdateQuery = new System.Text.StringBuilder ( );

      //
      // Delete the milestone activities for this milestone.
      //
      SqlUpdateQuery.AppendLine ( "/** DELETE ALL OF MILESTONE ACTIVITIES PREPARATION ROW FOR MILESTONE **/" );
      SqlUpdateQuery.AppendLine ( " DELETE FROM EvMilestoneActivities " );
      SqlUpdateQuery.AppendLine ( " WHERE  M_Guid = '" + Milestone.Guid + "' ; " );
      //SqlUpdateQuery.AppendLine ( "GO " );

      // 
      // Iterate through each newField in the activity list for
      // Updating those activities that have changed.
      // 
      foreach ( EvActivity activity in Milestone.ActivityList )
      {
        //
        // if the activity id is empty continue to the next value.
        //
        if ( activity.ActivityId == String.Empty )
        {
          this._DebugLog.AppendLine ( "ActivityId: EMPTY" );
          continue;
        }

        this._DebugLog.AppendLine ( "ActivityId: " + activity.ActivityId );
        this._DebugLog.AppendLine ( "IsMandatory: " + activity.IsMandatory );

        int iMandatory = 0;
        if ( activity.IsMandatory == true )
        {
          iMandatory = 1;
        }

        SqlUpdateQuery.AppendLine ( "/****  INSERT ACTIVITY " + activity.ActivityId + " ****/" );
        SqlUpdateQuery.AppendLine ( "Insert Into EvMilestoneActivities " );
        SqlUpdateQuery.AppendLine ( " (M_Guid, " );
        SqlUpdateQuery.AppendLine ( "  MA_Guid, " );
        SqlUpdateQuery.AppendLine ( "  TrialId, " );
        SqlUpdateQuery.AppendLine ( "  ActivityId, " );
        SqlUpdateQuery.AppendLine ( "  MA_Order, " );
        SqlUpdateQuery.AppendLine ( "  MA_IsMandatory, " );
        SqlUpdateQuery.AppendLine ( "  MA_BudgetQuantity )" );
        SqlUpdateQuery.AppendLine ( "values " );
        SqlUpdateQuery.AppendLine ( " ('" + Milestone.Guid + "', " );
        SqlUpdateQuery.AppendLine ( "  '" + Guid.NewGuid ( ) + "', " );
        SqlUpdateQuery.AppendLine ( "  '" + activity.ProjectId + "', " );
        SqlUpdateQuery.AppendLine ( "  '" + activity.ActivityId + "', " );
        SqlUpdateQuery.AppendLine ( "  " + activity.Order + ", " );
        SqlUpdateQuery.AppendLine ( "  " + iMandatory + ", " );
        SqlUpdateQuery.AppendLine ( " 1 ); " );

      }//END iteration loop.


      this._DebugLog.AppendLine ( "Sql Query: " + SqlUpdateQuery.ToString ( ) );

      if ( EvSqlMethods.QueryUpdate ( SqlUpdateQuery.ToString ( ), null ) == 0
        && Milestone.ActivityList.Count > 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      return EvEventCodes.Ok;

    }//END updateActivities method

    #endregion

  }//END EvMilestoneActivities class

}//END namespace Evado.Dal.Clinical
