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

//References to Evado specific libraries

using Evado.Model;
using Evado.Model.Digital;


namespace Evado.Dal.Clinical
{
  /// <summary>
  /// This class is handles the data access layer for the activity data object.
  /// </summary>
  public class EvActivities : EvDalBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EvActivities ( )
    {
      this.ClassNameSpace = "Evado.Dal.Clinical.EvActivities.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EvActivities ( EvClassParameters Settings )
    {
      this.ClassParameters = Settings;
      this.ClassNameSpace = "Evado.Dal.Clinical.EvActivities.";

      if ( this.ClassParameters.LoggingLevel == 0 )
      {
        this.ClassParameters.LoggingLevel = Evado.Dal.EvStaticSetting.LoggingLevel;
      }
    }

    #endregion

    #region Class constants

    /// <summary>
    /// This constant defines a sql query view. 
    /// </summary>
    private const string _sqlQuery_View = "Select * "
      + "FROM EvActivities ";


    #region The SQL Store Procedure constants

    /// <summary>
    /// This constant defines an add item stored prodedure
    /// </summary>
    private const string _storedProcedureAddItem = "usr_Activity_add";

    /// <summary>
    /// This constant defines an update item stored prodedure
    /// </summary>
    private const string _storedProcedureUpdateItem = "usr_Activity_update";

    /// <summary>
    /// This constant defines a delete item stored prodedure
    /// </summary>
    private const string _storedProcedureDeleteItem = "usr_Activity_delete";

    /// <summary>
    /// This constant defines a parameter global unique identifier
    /// </summary>
    private const string _parmGuid = "@Guid";

    /// <summary>
    /// This constant defines a parameter trial identifier
    /// </summary>
    private const string _parmTrialId = "@TrialId";

    /// <summary>
    /// This constant defines a parameter activity identifier
    /// </summary>
    private const string _parmActivityId = "@ActivityId";

    /// <summary>
    /// This constant defines a parameter title
    /// </summary>
    private const string _parmTitle = "@Title";

    /// <summary>
    /// This constant defines a parameter description
    /// </summary>
    private const string _parmDescription = "@Description";

    /// <summary>
    /// This constant defines a parameter initial version
    /// </summary>
    private const string _parmInitialVersion = "@InitialVersion";

    /// <summary>
    /// This constant defines a parameter type
    /// </summary>
    private const string _parmType = "@Type";

    /// <summary>
    /// This constant defines a parameter containing user identifier of those who updates the activities. 
    /// </summary>
    private const string _parmUpdatedByUserId = "@UpdatedByUserId";

    /// <summary>
    /// This constant defines a parameter containing a user who updates the activities. 
    /// </summary>
    private const string _parmUpdatedBy = "@UpdatedBy";

    /// <summary>
    /// This constant defines a parameter update date. 
    /// </summary>
    private const string _parmUpdateDate = "@UpdateDate";

    #endregion


    #endregion

    #region Class Property

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region SQL Parameter methods

    // ==================================================================================
    /// <summary>
    /// This class sets the update query properties. 
    /// </summary>
    /// <returns>SqlParameter: an array of items parameters.</returns>
    /// <remarks>
    /// This method consists of the following step:
    /// 
    /// 1. Create an array of sql parameters object. 
    /// 
    /// 2. Return an array of sql query parameters
    /// </remarks>
    // ----------------------------------------------------------------------------------
    private static SqlParameter [ ] getItemsParameters ( )
    {
      //
      // Create an array of sql parameters object. 
      //
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( _parmGuid, SqlDbType.UniqueIdentifier ),
        new SqlParameter( _parmTrialId, SqlDbType.NVarChar, 10),
        new SqlParameter( _parmActivityId, SqlDbType.NVarChar, 10),
        new SqlParameter( _parmTitle, SqlDbType.NVarChar, 100),
        new SqlParameter( _parmDescription, SqlDbType.NText),
        new SqlParameter( _parmType, SqlDbType.NVarChar, 20 ),
        new SqlParameter( _parmInitialVersion, SqlDbType.SmallInt ),
        new SqlParameter( _parmUpdatedByUserId, SqlDbType.NVarChar, 100),
        new SqlParameter( _parmUpdatedBy, SqlDbType.NVarChar, 100),
        new SqlParameter( _parmUpdateDate, SqlDbType.DateTime )
      };

      return cmdParms;

    }//END getItemsParameters class. 

    // ==================================================================================
    /// <summary>
    /// This class binds values parameters from activity object to the parameter array. 
    /// </summary>
    /// <param name="cmdParms">SqlParameter: Database parameters</param>
    /// <param name="Activity">EvActivity: Values to bind to parameters</param>
    /// <remarks>
    /// This method consists of the following step:
    /// 
    /// 1. Bind values from activity object to the parameter array. 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    private void SetParameters ( SqlParameter [ ] cmdParms, EvActivity Activity )
    {
      //
      // Bind values from activity object to the parameter array. 
      //
      cmdParms [ 0 ].Value = Activity.Guid;
      cmdParms [ 1 ].Value = Activity.ProjectId.Trim ( );
      cmdParms [ 2 ].Value = Activity.ActivityId.Trim ( );
      cmdParms [ 3 ].Value = Activity.Title;
      cmdParms [ 4 ].Value = Activity.Description;
      cmdParms [ 5 ].Value = Activity.Type.ToString ( );
      cmdParms [ 6 ].Value = Activity.InitialVersion;
      cmdParms [ 7 ].Value = Activity.UpdatedByUserId;
      cmdParms [ 8 ].Value = Activity.UserCommonName;
      cmdParms [ 9 ].Value = DateTime.Now;

    }//END SetParameters class

    #endregion

    #region Data Reader methods

    // =====================================================================================
    /// <summary>
    /// This class reads the content of the SqlDataReader into the Facility data object.
    /// </summary>
    /// <param name="Row">DataRow: a row of data table</param>
    /// <returns>EvActivity: a data row object.</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Extract the compatible data row values to the activity object 
    /// 
    /// 2. Return the activity object.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvActivity readDataRow ( DataRow Row )
    {
      // 
      // Initialise the activity object.
      // 
      EvActivity activity = new EvActivity ( );

      // 
      // Extract the data object values from the DataRow.
      // 
      activity.Guid = EvSqlMethods.getGuid ( Row, "AC_Guid" );
      activity.ProjectId = EvSqlMethods.getString ( Row, "TrialId" );
      activity.ActivityId = EvSqlMethods.getString ( Row, "ActivityId" );
      activity.Title = EvSqlMethods.getString ( Row, "AC_Title" );
      activity.Description = EvSqlMethods.getString ( Row, "AC_Description" );

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
      activity.InitialVersion = EvSqlMethods.getInteger ( Row, "AC_InitialVersion" );

      activity.UpdatedByUserId = EvSqlMethods.getString ( Row, "AC_UpdatedByUserId" );
      activity.UpdatedBy = EvSqlMethods.getString ( Row, "AC_UpdatedBy" );
      activity.UpdatedDate = EvSqlMethods.getDateTime ( Row, "AC_UpdatedByDate" );

      //
      // Return the activity object.
      //
      return activity;

    }// End readDataRow method.

    #endregion

    #region Activites Query methods

    // =====================================================================================
    /// <summary>
    /// This class returns a list of Activities based on TrialId, Types and WithForms condition 
    /// </summary>
    /// <param name="ProjectId">String: (Mandatory) The selection organistion's identifier</param>
    /// <param name="Type">List of EvActivity.ActivityTypes: a type of activity</param>
    /// <param name="WithForms">Boolean: true, if The sorting order for the ArrayList</param>
    /// <returns>List of EvActivity: a list of an activity object.</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Define the SQL query parameters and the sql query string. 
    /// 
    /// 2. Execute the query string and store the result in a data table
    /// 
    /// 3. Iterate through the results table for extracting the role information.
    /// 
    /// 4. Extract the table row and add the row data to the activity object. 
    /// 
    /// 5. Add the form list to the activity object, it the form exists and activity is clinical
    /// 
    /// 6. Return the list of activity. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvActivity> getActivityList (
      String ProjectId,
      EvActivity.ActivityTypes Type,
      bool WithForms )
    {
      //
      // Initialize a debug log. 
      //
      this.LogMethod( "getView. " );
      this.LogValue ( "ProjectId: " + ProjectId );
      this.LogValue ( "Type: " + Type );
      this.LogValue ( "WithForms: " + WithForms );

      // 
      // Initialize a sql query string, a return activity list and an activity forms object. 
      // 
      string sqlQueryString;
      List<EvActivity> view = new List<EvActivity> ( );
      EvActivityForms activityForms = new EvActivityForms ( );
      activityForms.ClassParameters.LoggingLevel = this.ClassParameters.LoggingLevel;

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( _parmTrialId, SqlDbType.NVarChar, 10),
      };
      cmdParms [ 0 ].Value = ProjectId;

      // 
      // Define the SQL query string
      // 
      sqlQueryString = _sqlQuery_View + "WHERE TrialId = @TrialId AND AC_DELETED = 0 ";

      if ( Type != EvActivity.ActivityTypes.Null )
      {
        sqlQueryString += " AND AC_Type = '" + Type + "' ";
      }
      sqlQueryString += " ORDER BY ActivityId; ";

      this.LogDebug (  sqlQueryString );

      //
      // Execute the query and store the result in a data table
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
      {
        // 
        // Iterate through the results table for extracting the role information.
        // 
        for ( int Count = 0; Count < table.Rows.Count; Count++ )
        {
          // 
          // Extract the table row and add the row data to the activity object. 
          // 
          DataRow row = table.Rows [ Count ];

          EvActivity activity = this.readDataRow ( row );

          // 
          // Add the form list to the activity object, it the form exists and activity is clinical
          // 
          if ( WithForms == true
            && activity.Type == EvActivity.ActivityTypes.Clinical )
          {
            activity.FormList = activityForms.getFormList ( activity.Guid );
            this.LogDebug (  "ActivityForms: " + activityForms.Log );

            for ( int i = 0; i < activity.FormList.Count; i++ )
            {
              activity.FormList [ i ].ActivityId = activity.ActivityId;
            }
          }

          // 
          // Append the value to the visit
          // 
          view.Add ( activity );

        } //END interation loop.

      }//END using statement

      this.LogDebug (  "view count: " + view.Count );
      // 
      // Return the ArrayList containing the EvTrialActivity data object.
      // 
      return view;

    }//END getView method.

    // =====================================================================================
    /// <summary>
    /// This class gets a list of activities using trial identifier. 
    /// </summary>
    /// <param name="ProjectId">string: (Mandatory) The selection EvTrialActivity's identifier</param>
    /// <returns>List of EvOption: an arrayList contains EvTrialActivity data objects.</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Define the SQL query parameters and Generate the SQL query string. 
    /// 
    /// 2. Execute the query and add values to the data table
    /// 
    /// 3. Iterate through the results table for extracting the role information.
    /// 
    /// 4. Extract the table row and add the row data to the option object.
    /// 
    /// 5. Append the option object to the Options list. 
    /// 
    /// 6. Return the Options list.
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> getOptionList ( string ProjectId )
    {
      this.LogMethod ( "getOptionList. " );
      this.LogValue ( "ProjectId: " + ProjectId );
      //
      // Initialize a debug log, sqlQueryString, a return option list and an option object. 
      //
      string sqlQueryString;
      List<EvOption> list = new List<EvOption> ( );
      EvOption option = new EvOption ( );
      list.Add ( option );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(_parmTrialId, SqlDbType.NVarChar, 10),
      };
      cmdParms [ 0 ].Value = ProjectId;

      // 
      // Generate the SQL query string
      // 
      sqlQueryString = _sqlQuery_View + "WHERE TrialId = @TrialId AND AC_DELETED = 0 ORDER BY ActivityId; ";
      this.LogDebug (  sqlQueryString );

      //
      // Execute the query and add values to the data table
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
      {
        // 
        // Iterate through the results table for extracting the role information.
        // 
        for ( int Count = 0; Count < table.Rows.Count; Count++ )
        {
          // 
          // Extract the table row and add the row data into the option object. 
          // 
          DataRow row = table.Rows [ Count ];
          option = new EvOption ( EvSqlMethods.getString ( row, "AC_Guid" ),
            EvSqlMethods.getString ( row, "ActivityId" ) + " - " + EvSqlMethods.getString ( row, "AC_Title" ) );

          //
          // Append the option object to the return list. 
          //
          list.Add ( option );

        }//END iteration loop.

      }//END using method

      this.LogDebug (  "list count: " + list.Count );

      // 
      // Return the ArrayList containing the EvTrialActivity data object.
      // 
      return list;

    }//END getList method.

    #endregion

    #region Activity Retrieval methods

    // =====================================================================================
    /// <summary>
    /// This class gets the information for a EvTrialActivity.
    /// </summary>
    /// <param name="Guid">Guid: a global unique identifier of an activity</param>
    /// <returns>EvActivity: an activity data object</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Return an empty Activity object if the Guid is empty 
    /// 
    /// 2. Define the sql query parameters and sql query string. 
    /// 
    /// 3. Execute the sql query command and store the result in the data table
    /// 
    /// 4. If the table has no value, return an empty activity object. 
    /// 
    /// 5. If not, extract the table row and store the data row in the activity object. 
    /// 
    /// 6. Add the forms list to the activity object. 
    /// 
    /// 7. Return the activity data object.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvActivity getActivity ( Guid Guid )
    {
      this.LogMethod ( "getActivity. " );
      this.LogValue ( "Guid: " + Guid );
      //
      // Initialize the debug log string, a sqlQueryString, an activity object 
      // and an activity forms object
      // 
      string sqlQueryString;
      EvActivity activity = new EvActivity ( );
      EvActivityForms activityForms = new EvActivityForms ( this.ClassParameters );
      activityForms.ClassParameters.LoggingLevel = this.ClassParameters.LoggingLevel;

      // 
      // Validate whether the Guid of activity is not empty
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
      sqlQueryString = _sqlQuery_View + " WHERE (AC_Guid = @Guid);";

      this.LogDebug (  sqlQueryString );

      //
      // Execute the query and store the result in the data table
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
      {
        // 
        // If not rows the return
        // 
        if ( table.Rows.Count == 0 )
        {
          return activity;
        }

        // 
        // Extract the table row and store the data row to an activity object
        // 
        DataRow row = table.Rows [ 0 ];

        activity = this.readDataRow ( row );

        // 
        // Add the forms list to the activity object. 
        // 
        activity.FormList = activityForms.getFormList ( activity.Guid );

        this.LogDebug (  "ActivityForms: " + activityForms.Log );

        for ( int i = 0; i < activity.FormList.Count; i++ )
        {
          activity.FormList [ i ].ActivityId = activity.ActivityId;

        }

      }//END Using 

      // 
      // Return the activity data object.
      // 
      return activity;

    }// Close method getActivity

    #endregion

    #region EvActivity Update methods

    // ==================================================================================
    /// <summary>
    /// This class updates the activity object. 
    /// </summary>
    /// <param name="Activity">EvActivity: an activity object</param>
    /// <returns>EvEventCodes: an activity update code.</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Exit, if the activity identifier or trial identifier or old activity's Guid is empty
    /// 
    /// 2. Set the data change object for updating activity value to the old activity
    /// 
    /// 3. Add new activity guid, if it is empty. 
    /// 
    /// 4. Define the sql query parameter and execute the store procedure for updating the items. 
    /// 
    /// 5. Add the datachange object values to the backup datachanges object. 
    /// 
    /// 6. Return an event code for updating items
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public EvEventCodes updateActivity ( EvActivity Activity )
    {
      this.LogMethod ( "updateItem. " );
      this.LogValue ( "ProjectId: " + Activity.ProjectId );
      this.LogValue ( "ActivityId: " + Activity.ActivityId );
      this.LogValue ( "Title: " + Activity.Title );
      // 
      // Initialize a debug log and a data changes object. 
      // 
      EvDataChanges dataChanges = new EvDataChanges ( );
      EvActivityForms activityForms = new EvActivityForms ( );
      activityForms.ClassParameters.LoggingLevel = this.ClassParameters.LoggingLevel;

      // 
      // Validate whether the activity identifier is not empty
      // 
      if ( Activity.ActivityId == String.Empty )
      {
        return EvEventCodes.Identifier_Activity_Id_Error;
      }

      //
      // Validate whether the trial identifier is not empty
      //
      if ( Activity.ProjectId == String.Empty )
      {
        return EvEventCodes.Identifier_Project_Id_Error;
      }

      EvActivity oldActivity = this.getActivity ( Activity.Guid );
      //
      // Validate whether the global unique identifier of the old activity is not empty
      //
      if ( oldActivity.Guid == Guid.Empty )
      {
        return EvEventCodes.Data_InvalidId_Error;
      }

      // 
      // Set the data change object values.
      // 
      EvDataChange dataChange = this.setChangeRecord ( Activity, oldActivity );

      // 
      // Generate the DB row guid, if the activity's guid is empty.
      // 
      if ( Activity.Guid == Guid.Empty )
      {
        Activity.Guid = Guid.NewGuid ( );
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      this.LogDebug (  "Update EvActivity details" );

      SqlParameter [ ] _cmdParms = getItemsParameters ( );
      SetParameters ( _cmdParms, Activity );

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( _storedProcedureUpdateItem, _cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      // 
      // Add the change record
      // 
      dataChanges.AddItem ( dataChange );

      EvEventCodes eventcode = activityForms.updateItems ( Activity );

      this.LogDebug (  activityForms.Log );

      if ( eventcode != EvEventCodes.Ok )
      {
        return eventcode;
      }

      // 
      // Return code
      // 
      return EvEventCodes.Ok;

    } // Close updateActivity method

    // ==================================================================================
    /// <summary>
    /// This class adds a record to the activity tables. 
    /// </summary>
    /// <param name="Activity">EvActivity: an activity object</param>
    /// <returns>EvEventCodes: an add activity code.</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Exit, if the activity identifier or trial identifier is empty
    /// 
    /// 2. Exit, if the activity object has no value
    /// 
    /// 3. Define the sql query parameters and execute the storeprocedure for adding new items. 
    /// 
    /// 4. Return the event code for adding items. 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public EvEventCodes addActivity ( EvActivity Activity )
    {
      this.LogMethod ( "addActivity. " );
      this.LogValue ( "ProjectId: " + Activity.ProjectId );
      this.LogValue ( "ActivityId: " + Activity.ActivityId );
      this.LogValue ( "Title: " + Activity.Title );
      try
      {
        //
        // Initialize a debug log and a sql query string. 
        //
        string _sqlQueryString = String.Empty;
        EvActivityForms activityForms = new EvActivityForms ( );

        // 
        // Define the sql query parameters and load value to the parameters
        // 
        SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(_parmTrialId, SqlDbType.NVarChar, 10),
        new SqlParameter(_parmActivityId, SqlDbType.NVarChar, 5),
      };
        cmdParms [ 0 ].Value = Activity.ProjectId;
        cmdParms [ 1 ].Value = Activity.ActivityId;

        // 
        // Generate the SQL query string for validating whether the Activity object exits
        // 
        _sqlQueryString = _sqlQuery_View + " WHERE (TrialId = @TrialId) AND (ActivityId = @ActivityId) AND AC_DELETED = 0 ;";

        this.LogDebug ( _sqlQueryString );


        //
        // Execute the query against the database
        //

        using ( DataTable table = EvSqlMethods.RunQuery ( _sqlQueryString, cmdParms ) )
        {
          // 
          // If not rows the return
          // 
          if ( table.Rows.Count > 0 )
          {
            return EvEventCodes.Data_Duplicate_Id_Error;
          }
        }

        // 
        // Generate the DB row guid
        // 
        Activity.Guid = Guid.NewGuid ( );

        // 
        // Define the SQL query parameters and load the query values.
        // 
        SqlParameter [ ] _cmdParms = getItemsParameters ( );
        this.SetParameters ( _cmdParms, Activity );

        //
        // Execute the update command for adding new activity data row.
        //
        if ( EvSqlMethods.StoreProcUpdate ( _storedProcedureAddItem, _cmdParms ) == 0 )
        {
          return EvEventCodes.Database_Record_Update_Error;
        }

        //
        // Add the activitiess forms.
        //
        EvEventCodes eventcode = activityForms.updateItems ( Activity );

        this.LogDebug ( activityForms.Log );

        if ( eventcode != EvEventCodes.Ok )
        {
          return eventcode;
        }
      }
      catch ( Exception Ex )
      {
        //
        // Write event log
        //
        string _eventLogSource = ConfigurationManager.AppSettings [ "EventLogSource" ];
        string eventMessage = this.Log
         + "\r\n" + Ex.Message
         + "\r\n" + Ex.Source
         + "\r\n" + Ex.StackTrace.ToString ( )
         + "\r\n" + Ex.TargetSite.ToString ( )
         + "\r\n" + Ex.InnerException;

        EventLog.WriteEntry ( _eventLogSource, eventMessage,
          EventLogEntryType.Error );
        throw;

      }//END try-catch 

      return EvEventCodes.Ok;

    }//END addActivity  method

    // ==================================================================================
    /// <summary>
    /// This class logically deletes the data row from the activity table. 
    /// </summary>
    /// <param name="Activity">EvActivity: an activity object</param>
    /// <returns>EvEventCodes: an event code for deleting activity</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Define the SQL query parameters and execute the storeprocedure for deleting items. 
    /// 
    /// 2. Return an event code for deleting items. 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public EvEventCodes deleteActivity ( EvActivity Activity )
    {
      //
      // Initialize a debug log string
      //
      this.LogMethod ( "deleteActivity. " );
      this.LogValue ( "Guid: " + Activity.Guid );
      this.LogValue ( "ActivityId: " + Activity.ActivityId );
      this.LogValue ( "Title: " + Activity.Title );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(_parmGuid, SqlDbType.UniqueIdentifier),
        new SqlParameter(_parmUpdatedBy, SqlDbType.NVarChar,100),
        new SqlParameter(_parmUpdateDate, SqlDbType.DateTime)
      };
      cmdParms [ 0 ].Value = Activity.Guid;
      cmdParms [ 1 ].Value = Activity.UserCommonName;
      cmdParms [ 2 ].Value = DateTime.Now;

      //
      // Execute the update command for deleting the row data on activity table.
      //
      if ( EvSqlMethods.StoreProcUpdate ( _storedProcedureDeleteItem, cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      return EvEventCodes.Ok;

    }//END deleteActivity method

    #endregion

    #region Set Difference methods

    // ==================================================================================
    /// <summary>
    /// This method sets the data change object for the update action.
    /// </summary>
    /// <param name="Activity">EvActivity: a current activity object</param>
    /// <param name="OldActivity">EvActivity: an old activity object</param>
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
    public EvDataChange setChangeRecord ( EvActivity Activity, EvActivity OldActivity )
    {
      // 
      // Initialise the datachange object.
      // 
      EvDataChange dataChange = new EvDataChange ( );
      dataChange.Guid = Guid.NewGuid ( );
      dataChange.TableName = EvDataChange.DataChangeTableNames.EvActivities;
      dataChange.TrialId = Activity.ProjectId;
      dataChange.RecordGuid = Activity.Guid;
      dataChange.UserId = Activity.UpdatedByUserId;
      dataChange.DateStamp = DateTime.Now;

      //
      // Add values from Activity items to the datachange object, if they do not exist in the old activity object.
      //
      if ( Activity.ProjectId != OldActivity.ProjectId )
      {
        dataChange.AddItem ( "ProjectId", OldActivity.ProjectId, Activity.ProjectId );
      }

      if ( Activity.ActivityId != OldActivity.ActivityId )
      {
        dataChange.AddItem ( "ActivityId", OldActivity.ActivityId, Activity.ActivityId );
      }

      if ( Activity.Title != OldActivity.Title )
      {
        dataChange.AddItem ( "Title", OldActivity.Title, Activity.Title );
      }

      if ( Activity.Type != OldActivity.Type )
      {
        dataChange.AddItem ( "Type", OldActivity.Type.ToString ( ), Activity.Type.ToString ( ) );
      }

      if ( Activity.Description != OldActivity.Description )
      {
        dataChange.AddItem ( "Description", OldActivity.Description, Activity.Description );
      }

      if ( Activity.InitialVersion != OldActivity.InitialVersion )
      {
        dataChange.AddItem ( "InitialVersion", OldActivity.InitialVersion.ToString ( ), Activity.InitialVersion.ToString ( ) );
      }

      // 
      // Iterate through the formlist records of the current activity object
      // 
      foreach ( EvActvityForm form in Activity.FormList )
      {
        EvActvityForm oldform = this.getActivityRecord ( OldActivity.FormList, form.FormId );

        //
        // Add order and mandatory item of formlist to datachange object if they do not exist in the old activity object
        //
        if ( form.Order != oldform.Order )
        {
          dataChange.AddItem ( "Form_" + form.FormId + "_Order", oldform.Order.ToString ( ), form.Order.ToString ( ) );
        }

        if ( form.Mandatory != oldform.Mandatory )
        {
          dataChange.AddItem ( "Form_" + form.FormId + "_Mandatory", oldform.Mandatory.ToString ( ), form.Mandatory.ToString ( ) );
        }

      }//END ActivityForm iteration loop.

      // 
      // Return the data change object.
      // 
      return dataChange;

    }//END setChangeRecord method

    // ==================================================================================
    /// <summary>
    /// This method returns the selected getActivityRecords
    /// </summary>
    /// <param name="RecordList">List of EvActivityForm: a list of activity record</param>
    /// <param name="ItemId">String: an item identifier</param>
    /// <returns>EvActivityForm: an activity record object</returns>
    /// <remarks>
    /// This method consists of the followings steps:
    /// 
    /// 1. Iterate through the list of activity record
    /// 
    /// 2. Return the activity object if form identifier matches with item identifier. 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    private EvActvityForm getActivityRecord ( List<EvActvityForm> RecordList, String ItemId )
    {
      //
      // Iterate through the list of activity record
      //
      foreach ( EvActvityForm activity in RecordList )
      {
        // 
        // Return the matching newField.
        // 
        if ( activity.FormId == ItemId )
        {
          return activity;
        }
      }

      // 
      // IF none return empty object.
      // 
      return new EvActvityForm ( );

    }//END getActivityRecord method

    #endregion

  }//END EvActivities class

}//END namespace Evado.Dal
