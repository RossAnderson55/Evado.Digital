/***************************************************************************************
 * <copyright file="dal\EvSubjectMilestoneActivities.cs" company="EVADO HOLDING PTY. LTD.">
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
  /// A business Component used to manage Milestone roles
  /// The Evado.Model.EvActivity is used in most methods 
  /// and is used to store serializable information about an account
  /// </summary>
  public class EvSubjectMilestoneActivities : EvDalBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EvSubjectMilestoneActivities ( )
    {
      this.ClassNameSpace = "Evado.Dal.Clinical.EvSubjectMilestoneActivities.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EvSubjectMilestoneActivities ( EvClassParameters Settings )
    {
      this.ClassParameters = Settings;
      this.ClassNameSpace = "Evado.Dal.Clinical.EvSubjectMilestoneActivities.";

      if ( this.ClassParameters.LoggingLevel == 0 )
      {
        this.ClassParameters.LoggingLevel = Evado.Dal.EvStaticSetting.LoggingLevel;
      }
    }

    #endregion

    #region Class constants and variable initialisation

    private const string _sqlQuery_View = "Select * FROM EvSubjectMilestoneActivity_View ";

    /// 
    /// The SQL Store Procedure constants
    /// 
    private const string _STORED_PROCEDURE_AddItem = "usr_SubjectMilestoneActivity_add";
    private const string _STORED_PROCEDURE_UpdateItem = "usr_SubjectMilestoneActivity_update";
    private const string _STORED_PROCEDURE_DeleteItem = "usr_SubjectMilestoneActivity_delete";

    private const string _parmGuid = "@Guid";
    private const string _parmMilestoneGuid = "@MilestoneGuid";
    private const string _parmTrialId = "@TrialId";
    private const string _parmActivityId = "@ActivityId";
    private const string _parmScheduleDate = "@ScheduleDate";
    private const string _parmQuantity = "@Quantity";
    private const string _parmCompletedBy = "@CompletedBy";
    private const string _parmCompletedDate = "@CompletedDate";
    private const string _parmComments = "@Comments";
    private const string _parmProtocolViolation = "@ProtocolViolation";
    private const string _parmIsMandatory = "@IsMandatory";
    private const string _parmStatus = "@Status";
    private const string _parmUpdateByUserId = "@UpdateByUserId";
    private const string _parmUpdatedBy = "@UpdatedBy";
    private const string _parmUpdated = "@UpdatedByDate";

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region SQL Parameter methods

    // =====================================================================================
    /// <summary>
    /// This class returns an array of sql query parameters. 
    /// </summary>
    /// <returns>An array of sql parameters</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Create an array of sql query parameters. 
    /// 
    /// 2. Return an array of sql query parameters. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private static SqlParameter [ ] getItemsParameters( )
    {
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( _parmGuid, SqlDbType.UniqueIdentifier ),
        new SqlParameter( _parmMilestoneGuid, SqlDbType.UniqueIdentifier),
        new SqlParameter( _parmActivityId, SqlDbType.NVarChar, 20),
        new SqlParameter( _parmTrialId, SqlDbType.NVarChar, 10),
        new SqlParameter( _parmScheduleDate, SqlDbType.DateTime),
        new SqlParameter( _parmQuantity, SqlDbType.Float),
        new SqlParameter( _parmCompletedDate, SqlDbType.DateTime),
        new SqlParameter( _parmCompletedBy, SqlDbType.NVarChar,100),
        new SqlParameter( _parmComments, SqlDbType.NText),
        new SqlParameter( _parmProtocolViolation, SqlDbType.Bit),
        new SqlParameter( _parmIsMandatory, SqlDbType.Bit),
        new SqlParameter( _parmStatus, SqlDbType.NVarChar,15),
        new SqlParameter( _parmUpdateByUserId, SqlDbType.NVarChar,100),
        new SqlParameter( _parmUpdatedBy, SqlDbType.NVarChar,100),
        new SqlParameter( _parmUpdated, SqlDbType.DateTime)
      };

      return cmdParms;
    }//END getItemsParameters method.

    // =====================================================================================
    /// <summary>
    /// This method assigns the Activity object's values to the array of sql parameters. 
    /// </summary>
    /// <param name="cmdParms">SqlParameter: An array of sql parameters</param>
    /// <param name="Activity">EvActivity: an activity object</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Bind the Activity object's values to the array of sql parameters. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private void SetParameters( SqlParameter [ ] cmdParms, EvActivity Activity )
    {

      cmdParms [ 0 ].Value = Activity.Guid;
      cmdParms [ 1 ].Value = Activity.MilestoneGuid;
      cmdParms [ 2 ].Value = Activity.ActivityId;
      cmdParms [ 3 ].Value = Activity.ProjectId;
      cmdParms [ 4 ].Value = Evado.Model.EvStatics.CONST_DATE_NULL;
      cmdParms [ 5 ].Value = 1;
      cmdParms [ 6 ].Value = Activity.CompletionDate;
      cmdParms [ 7 ].Value = Activity.CompletedBy;
      cmdParms [ 8 ].Value = Activity.Comments;
      cmdParms [ 9 ].Value = Activity.ProtocolViolation;
      cmdParms [ 10 ].Value = Activity.IsMandatory;
      cmdParms [ 11 ].Value = Activity.Status.ToString( );
      cmdParms [ 12 ].Value = Activity.UpdatedByUserId;
      cmdParms [ 13 ].Value = Activity.UserCommonName;
      cmdParms [ 14 ].Value = DateTime.Now;

    }//END SetLetterParameters.

    #endregion

    #region Data Reader methods

    // =====================================================================================
    /// <summary>
    /// This method extracts data row values to the activity object. 
    /// </summary>
    /// <param name="Row">DataRow: The data row object</param>
    /// <returns>EvActivity: an activity data object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Extract the compatible data row values to the Activity object. 
    /// 
    /// 2. Return the Activity object. 
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvActivity readDataRow( DataRow Row )
    {
      // 
      // Initialise the object.
      // 
      EvActivity activity = new EvActivity( );

      // 
      // Extract the data object values. 
      // 
      activity.Guid = EvSqlMethods.getGuid ( Row, "SMA_Guid" );
      activity.MilestoneGuid = EvSqlMethods.getGuid( Row, "SM_Guid" );
      activity.MilestoneActivityGuid = EvSqlMethods.getGuid( Row, "MA_Guid" );

      // 
      // Activity Components
      // 
      activity.ProjectId = EvSqlMethods.getString( Row, "TrialId" );
      activity.ActivityId = EvSqlMethods.getString( Row, "ActivityId" );
      activity.Title = EvSqlMethods.getString( Row, "AC_Title" );
      activity.Description = EvSqlMethods.getString( Row, "AC_Description" );

      String stType = EvSqlMethods.getString( Row, "AC_Type" );

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

      activity.Type = Evado.Model.EvStatics.Enumerations.parseEnumValue<EvActivity.ActivityTypes> ( stType );

      // 
      // Milestone Activity components.
      // 
      activity.Order = EvSqlMethods.getInteger( Row, "MA_Order" );
      activity.IsMandatory = EvSqlMethods.getBool( Row, "SMA_IsMandatory" );
      //activity.Quantity = EvSqlMethods.getInteger( Row, "MA_PlannedQuantity" );

      // 
      // Subject Milestone Activity components.
      // 
      //activity.sc = EvSqlMethods.getDateTime( Row, "SMA_ScheduleDate" );
      activity.Quantity = EvSqlMethods.getInteger( Row, "SMA_ActualQuantity" );
      activity.CompletionDate = EvSqlMethods.getDateTime( Row, "SMA_CompletedDate" );
      activity.CompletedBy = EvSqlMethods.getString( Row, "SMA_CompletedBy" );
      activity.Comments = EvSqlMethods.getString( Row, "SMA_Comments" );
      activity.ProtocolViolation = EvSqlMethods.getBool( Row, "SMA_ProtocolViolation" );
      activity.Status = Evado.Model.EvStatics.Enumerations.parseEnumValue<EvActivity.ActivityStates> ( EvSqlMethods.getString ( Row, "SMA_Status" ) );
      activity.UpdatedBy = EvSqlMethods.getString( Row, "SMA_UpdatedBy" );
      activity.UpdatedDate = EvSqlMethods.getDateTime( Row, "SMA_UpdatedByDate" );
      activity.UpdatedByUserId = EvSqlMethods.getString( Row, "SMA_UpdatedByUserId" );

      //
      // Return an object containing an EvActivity object.
      //
      return activity;

    }//END readDataRow method.

    #endregion

    #region Activity Query methods

    // =====================================================================================
    /// <summary>
    /// This method gets a list of Activity data objects. 
    /// </summary>
    /// <param name="MilestoneGuid">Guid: An identifier for a Milestone object.</param>
    /// <param name="Selection">Enum: An enum ActivitySelection.</param>
    /// <returns>An list containing an EvActivity object.</returns>
    /// <remarks>
    /// This method consists of folloiwng steps.
    /// 
    /// 1. Define the SQL query parameters and SQL query string. 
    /// 
    /// 2. Execute the sql query string and store the results on datatable. 
    /// 
    /// 3. Loop through the table and extract the data row to the Activity object. 
    /// 
    /// 4. Add the Activity object to the Activities list. 
    /// 
    /// 5. Remove the duplicated visits from the Activities list. 
    /// 
    /// 6. Return the Activities list. 
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvActivity> getView( 
      Guid MilestoneGuid,
      EvActivity.ActivitySelection Selection )
    {
      this.LogMethod ( "getView. " );
      this.LogDebug( "MilestoneGuid: " + MilestoneGuid );
      this.LogDebug( "Selection: " + Selection );

      // 
      // Define the local variables and objects.
      // 
      string sqlQueryString;
      List<EvActivity> visitActivityList = new List<EvActivity>( );

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
      sqlQueryString = _sqlQuery_View + "WHERE (SM_Guid = @MilestoneGuid) ";

      if ( Selection == EvActivity.ActivitySelection.Mandatory )
      {
        sqlQueryString += " AND (MA_IsMandatory = '1') ";
      }
      else if ( Selection == EvActivity.ActivitySelection.Optional )
      {
        sqlQueryString += " AND (MA_IsMandatory = '0') ";
      }

      sqlQueryString += " ORDER BY MA_Order, SMA_CompletedDate ; ";

     this.LogDebug ( sqlQueryString );

      //
      // Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery( sqlQueryString, cmdParms ) )
      {
        // 
        // Iterate through the table rows count information.
        // 
        for ( int Count = 0; Count < table.Rows.Count; Count++ )
        {
          // 
          // Extract the table row
          // 
          DataRow row = table.Rows [ Count ];

          EvActivity activity = this.readDataRow( row );

          // 
          // Append the value to the visit activity list.
          // 
          visitActivityList.Add( activity );

        } //END interation loop.

      }//END using method

      //
      // Remove the any duplicate visit activities.
      //
      //this.removeDuplicateVisitActivities( visitActivityList );

      this.LogDebug( "view count: " + visitActivityList.Count );

      // 
      // Return the list containing the EvActivity data object.
      // 
      return visitActivityList;

    }//END getView method.

    // -----------------------------------------------------------------------------------
    /// <summary>
    /// This method removes any duplicate items from the Activities list. 
    /// </summary>
    /// <param name="VisitActivityList">List of EvActivity: A list containing an EvActivity data object.
    /// </param>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Exit, if the Visit activity list has less than 2 items. 
    /// 
    /// 2. Loop through the Visit activity list and update the ActivityId, latest Mandatory date 
    /// and latest Optional date. 
    /// 
    /// 3. Loop through the String Array of ActivityIds and VisitActivityList to remove the duplicated items.
    /// 
    /// </remarks>
    // ===================================================================================
    private void removeDuplicateVisitActivities( List<EvActivity> VisitActivityList )
    {
      //
      // Initialise the methods variables and objects.
      //
      List<EvActivity> activityList = new List<EvActivity>( );
      String activityIds = String.Empty;
      DateTime latestManDate = Evado.Model.EvStatics.CONST_DATE_NULL;
      DateTime latestOptDate = Evado.Model.Digital.EvcStatics.CONST_DATE_NULL;

      //
      // If there is less than 2 activities exit, return void.
      //
      if ( VisitActivityList.Count < 2 )
      {
        return;
      }

      //
      // Loop through the Visit activity list and update the ActivityId, latest Mandatory datae and latest Optional date. 
      //
      foreach ( EvActivity activity in VisitActivityList )
      {

        //
        // Add activity ids to the string to provide a list of identifiers.
        //
        if ( activityIds.Contains( activity.ActivityId ) != false )
        {
          activityIds += activityIds = ";";
        }

        //
        // Find the latest mandatory activity date and 
        // set latest mandatory date to it.
        //
        if ( activity.IsMandatory == true
          && latestManDate < activity.CompletionDate )
        {
          latestManDate = activity.CompletionDate;
        }

        //
        // Find the latest mandatory activity date and 
        // set latest optional date to it.
        //
        if ( activity.IsMandatory == false
          && latestOptDate < activity.CompletionDate )
        {
          latestOptDate = activity.CompletionDate;
        }

      }//END first visit activity list iteration loop.

      String [ ] arrActivityIds = activityIds.Split( ';' );

      //
      // Loop through the String Array of ActivityIds and VisitActivityList to remove the duplicated items. 
      //
      for ( int index = 0; index < arrActivityIds.Length; index++ )
      {
        //
        // Iterate through the list of visit activities looking for duplicates.
        //
        for ( int count = 0; count < VisitActivityList.Count; count++ )
        {
          if ( VisitActivityList [ count ].Type == EvActivity.ActivityTypes.Clinical )

            //
            // If the activity ID and the activity is mandatory process it.
            //
            if ( VisitActivityList [ count ].ActivityId == arrActivityIds [ index ]
                && VisitActivityList [ count ].IsMandatory == true
                && VisitActivityList [ count ].CompletionDate < latestManDate )
            {
              VisitActivityList.RemoveAt( count );
              count--;
            }//END delete mandatory duplicate visit activities.

          //
          // If the activity ID and the activity is not mandatory process it.
          //
          if ( VisitActivityList [ count ].ActivityId == arrActivityIds [ index ]
              && VisitActivityList [ count ].IsMandatory == false
              && VisitActivityList [ count ].CompletionDate < latestOptDate )
          {
            VisitActivityList.RemoveAt( count );
            count--;
          }//END delete optional duplicate visit activities.
        }
      }//EMD visit activity list iteration loop.

    }//END removeDuplicateVisitActivities method

    // =====================================================================================
    /// <summary>
    /// This method returns a list of Activity object items based on Milestone Guid, Activity's Type and Selection
    /// </summary>
    /// <param name="MilestoneGuid">Guid: the Milestone's Global unique identifier</param>
    /// <param name="Type">EvActivity.ActivityTypes: The Activity types</param>
    /// <param name="ActivitySelection">EvActivity.ActivitySelection: The Activity selection</param>
    /// <returns>List of EvActivity: A list containing Activity data object.</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Define the SQL query parameters and sql query string. 
    /// 
    /// 2. Execute the sql query string and store the results on datatable. 
    /// 
    /// 3. Loop through the table and extract data row to the Activity object. 
    /// 
    /// 4. Add the Activity object's values to the Activities list. 
    /// 
    /// 5. Return the list of activities. 
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvActivity> getView(
      Guid MilestoneGuid,
      EvActivity.ActivityTypes Type,
      EvActivity.ActivitySelection ActivitySelection )
    {
      this.LogDebug( Evado.Model.Digital.EvcStatics.CONST_METHOD_START + "Evado.Dal.Clinical.EvSubjectMilestoneActivities.getView. " );
      this.LogDebug( "MilestoneGuid: " + MilestoneGuid );
      this.LogDebug( "Type: " + Type );
      this.LogDebug( "Selection: " + ActivitySelection );

      // 
      // Define the local variables and objects.
      // 
      string sqlQueryString;
      List<EvActivity> view = new List<EvActivity>( );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( _parmGuid, SqlDbType.UniqueIdentifier),
      };
      cmdParms [ 0 ].Value = MilestoneGuid;

      // 
      // Generate the SQL query string.
      // 
      sqlQueryString = _sqlQuery_View + "WHERE SM_Guid = @Guid ";

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
          sqlQueryString += " AND (AC_Type = '" + Type.ToString( ) + "') ";
        }
      }

      if ( ActivitySelection == EvActivity.ActivitySelection.Mandatory )
      {
        sqlQueryString += " AND MA_IsMandatory = '1' ";
      }
      if ( ActivitySelection == EvActivity.ActivitySelection.Optional )
      {
        sqlQueryString += " AND MA_IsMandatory = '0' ";
      }

      sqlQueryString += " ORDER BY MA_Order; ";

      this.LogDebug( sqlQueryString  );

      //
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery( sqlQueryString, cmdParms ) )
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

          EvActivity activity = this.readDataRow( row );

          // 
          // Append the value to the object containing an EvActivity data object.
          // 
          view.Add( activity );

        } //END interation loop.

      }//END using method

      this.LogDebug( " View count: " + view.Count );

      // 
      // Return the list containing the EvActivity data object.
      // 
      return view;

    }//END getView method.

    // =====================================================================================
    /// <summary>
    /// This method returns a list of Options based on Milestone's Guid and Activity selection. 
    /// </summary>
    /// <param name="MilestoneGuid">Guid: a milestone's global unique identifier</param>
    /// <param name="Selection">EvActivity.ActivitySelection: The activity selection.</param>
    /// <returns>List of EvOption: A list containing an Option data object.</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Define the SQL query parameters and sql query string. 
    /// 
    /// 2. Execute the sql query string and store the results on datatable. 
    /// 
    /// 3. Loop through the table and extract data row to the Option object. 
    /// 
    /// 4. Add the Option object's values to the Options list. 
    /// 
    /// 5. Return the list of options. 
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> getList( 
      Guid MilestoneGuid,
      EvActivity.ActivitySelection Selection )
    {
      this.LogMethod ( "getList. " );
      this.LogDebug( "MilestoneGuid: " + MilestoneGuid );
       this.LogDebug( "Selection: " + Selection );

      // 
      // Define the local variables
      // 
      string sqlQueryString;
      List<EvOption> list = new List<EvOption>( );
      EvOption option = new EvOption( );
      list.Add( option );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( _parmMilestoneGuid, SqlDbType.UniqueIdentifier),
      };
      cmdParms [ 0 ].Value = MilestoneGuid;

      // 
      // Generate the SQL query string.
      // 
      sqlQueryString = _sqlQuery_View + "WHERE SM_Guid = @MilestoneGuid ";

      if ( Selection == EvActivity.ActivitySelection.Mandatory )
      {
        sqlQueryString += " AND MA_IsMandatory = '1' ";
      }
      if ( Selection == EvActivity.ActivitySelection.Optional )
      {
        sqlQueryString += " AND MA_IsMandatory = '0' ";
      }

      sqlQueryString += " ORDER BY MA_Order; ";

      this.LogDebug( sqlQueryString );

      //
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery( sqlQueryString, cmdParms ) )
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

          option = new EvOption( EvSqlMethods.getString( row, "MA_Guid" ),
            EvSqlMethods.getString( row, "ActivityId" )
            + " - " + EvSqlMethods.getString( row, "M_Title" ) );

          bool CompletedDate = EvSqlMethods.getBool( row, "M_CompletedDate" );
          if ( CompletedDate == true )
          {
            option.Description += " (Mandatory) ";
          }

          list.Add( option );

        } //END interation loop.

      }//END using method

     this.LogDebug ( "list count: " + list.Count );

      // 
      // Return the list containing the EvActivity data object.
      // 
      return list;

    }//END getList method.

    // =====================================================================================
    /// <summary>
    /// This method returns a list of Options based on Milestone's Guid, Activity's Type and Activity selection. 
    /// </summary>
    /// <param name="MilestoneGuid">Guid: a milestone's global unique identifier</param>
    /// <param name="Type">EvActivity.ActivityTypes: The activity's types</param>
    /// <param name="Selection">EvActivity.ActivitySelection: The activity selection.</param>
    /// <returns>List of EvOption: A list containing an Option data object.</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Define the SQL query parameters and sql query string. 
    /// 
    /// 2. Execute the sql query string and store the results on datatable. 
    /// 
    /// 3. Loop through the table and extract data row to the Option object. 
    /// 
    /// 4. Add the Option object's values to the Options list. 
    /// 
    /// 5. Return the list of options. 
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> getList( 
      Guid MilestoneGuid,
      EvActivity.ActivityTypes Type,
      EvActivity.ActivitySelection Selection )
    {
      this.LogMethod ( "getList. " );
      this.LogDebug( "MilestoneGuid: " + MilestoneGuid );
      this.LogDebug( "Type: " + Type );
      this.LogDebug( "Selection: " + Selection );

      // 
      // Define the local variables and objects. 
      // 
      string sqlQueryString;
      List<EvOption> list = new List<EvOption>( );
      EvOption option = new EvOption( );
      list.Add( option );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( _parmMilestoneGuid, SqlDbType.UniqueIdentifier),
      };
      cmdParms [ 0 ].Value = MilestoneGuid;

      // 
      // Generate the SQL query string.
      // 
      sqlQueryString = _sqlQuery_View + "WHERE SM_Guid = @MilestoneGuid ";

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
          sqlQueryString += " AND (AC_Type = '" + Type.ToString( ) + "') ";
        }
      }

      if ( Selection == EvActivity.ActivitySelection.Mandatory )
      {
        sqlQueryString += " AND (MA_IsMandatory = '1') ";
      }
      if ( Selection == EvActivity.ActivitySelection.Optional )
      {
        sqlQueryString += " AND (MA_IsMandatory = '0') ";
      }

      sqlQueryString += " ORDER BY MA_Order; ";

      this.LogDebug( sqlQueryString );

      //
      // Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery( sqlQueryString, cmdParms ) )
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

          option = new EvOption( EvSqlMethods.getString( row, "MA_Guid" ),
            EvSqlMethods.getString( row, "ActivityId" )
            + " - " + EvSqlMethods.getString( row, "M_Title" ) );

          bool bMandatory = EvSqlMethods.getBool( row, "M_IsMandatory" );
          if ( bMandatory == true )
          {
            option.Description += " (Mandatory) ";
          }

          list.Add( option );

        } //END interation loop.

      }//END using method

     this.LogDebug ( "list count: " + list.Count );

      // 
      // Return the list containing the Option data object.
      // 
      return list;

    }//END getList method.

    #endregion

    #region Activity Retrieval methods

    // =====================================================================================
    /// <summary>
    /// This method retrieves the Activity data table based on Guid
    /// </summary>
    /// <param name="Guid">Guid: The Activity's global unique identifier </param>
    /// <returns>EvActivity: An Activity data object.</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Return an empty Activity object, if the Guid is empty. 
    /// 
    /// 2. Define the sql query parameters and sql query string. 
    /// 
    /// 3. Execute the sql query string and store the results on datatable. 
    /// 
    /// 4. Return an empty Activity Object, if the datatable has no value
    /// 
    /// 5. Else, extract the first row values to the Activity Object. 
    /// 
    /// 6. Return the Activity Object. 
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvActivity getActivity( 
      Guid Guid )
    {
      this.LogMethod ( "getActivity method. " );
      this.LogDebug ( "Guid: " + Guid );

      // 
      // Define local variables and objects. 
      // 
      string sqlQueryString;
      EvActivity activity = new EvActivity( );

      // 
      // If Guid does not exists, return an object containing an EvActivity data object.
      // 
      if ( Guid == Guid.Empty )
      {
        return activity;
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter cmdParms = new SqlParameter( _parmGuid, SqlDbType.UniqueIdentifier );
      cmdParms.Value = Guid;

      // 
      // Generate the SQL query string.
      // 
      sqlQueryString = _sqlQuery_View + " WHERE (SMA_Guid = '" + Guid.ToString( ) + "');";

      this.LogDebug( sqlQueryString );

      //
      // Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery( sqlQueryString, null ) )
      {

        // 
        // If no rows found, return an object containing an EvActivity data object.
        // 
        if ( table.Rows.Count == 0 )
        {
          return activity;
        }

        // 
        // Extract the table row.
        // 
        DataRow row = table.Rows [ 0 ];

        activity = this.readDataRow( row );

      }//END Using 

      // 
      // Return an object containing an EvActivity object.
      // 
      return activity;

    }//END getActivity method 

    #endregion

    #region Activity Update methods

    // =====================================================================================
    /// <summary>
    ///  This method updates items to the Activity data table. 
    /// </summary>
    /// <param name="SubjectMilestone">List of EvMilestonedata object.</param>
    /// <returns>EvEventCodes: an event code for updating activities.</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Loop through the ActivityList
    /// 
    /// 2. Add items to Activity table, if Activity's action is 'Save' and its Guid is empty.
    /// 
    /// 3. Update items on the Activity table, if Activity's action is 'Save' and its Guid has value.
    /// 
    /// 4. Delete items from the Activity table, if Activity's action is not 'Save'
    ///    
    /// 5. Return an event code for updating Activity Object items. 
    ///       
    /// </remarks>
    /// 
    // -------------------------------------------------------------------------------------
    public EvEventCodes updateActivities( 
      EvMilestone SubjectMilestone )
    {
      this.LogMethod ( "updateActivities." );
      this.LogDebug ( "list count: " + SubjectMilestone.ActivityList.Count );
      this.LogDebug ( "MilestoneGuid: " + SubjectMilestone.Guid );

      // 
      // Initialise the method variables and objects.
      // 
      EvEventCodes iReturn = EvEventCodes.Ok;

      // 
      // Update the activity if the activity is correct or it has just been created.
      // 
      if ( SubjectMilestone.Guid == Guid.Empty )
      {
        this.LogDebug ( "Subject milestone guid is empty." );
        return EvEventCodes.Identifier_Global_Unique_Identifier_Error;
      }

      // 
      // Iterate through each activity in the Activity list.
      // Updating those activities that have changed.
      // 
      foreach ( EvActivity activity in SubjectMilestone.ActivityList )
      {
        this.LogDebug ( "ActivityGuid: " + activity.Guid );
        this.LogDebug ( "ActivityId: " + activity.ActivityId );
        this.LogDebug ( "Type: " + activity.Type );
        this.LogDebug ( "IsMandatory: " + activity.IsMandatory );
        this.LogDebug ( "Status: " + activity.Status );
        this.LogDebug ( "Action: " + activity.Action );

        // 
        // Update the activity if the activity is correct or it has just been created.
        // 
        if ( activity.Action == EvActivity.ActivitiesActionsCodes.Skip )
        {
          this.LogDebug ( " >> SKIP ACTIVITY" );
          continue;
        }

        activity.UpdatedBy = SubjectMilestone.UpdatedBy;
        activity.UpdatedByUserId = SubjectMilestone.UpdatedByUserId;

        //
        // Add the activity to the database.
        //
        if ( activity.Action == EvActivity.ActivitiesActionsCodes.Save )
        {
          if ( activity.Guid == Guid.Empty )
          {
            this.LogDebug ( " >> Add Activity " );
            // 
            // Set the activities milestone guid value .
            // 
            activity.MilestoneGuid = SubjectMilestone.Guid;
            activity.Status = EvActivity.ActivityStates.Created;

            iReturn = this.addActivity ( activity );

            if ( iReturn < EvEventCodes.Ok )
            {
              return iReturn;
            }

          }//END Add Activity
          else
          {
            // 
            // Update the activity.
            // 
            this.LogDebug ( " >> Update Activity " );
            iReturn = this.updateActivity ( activity );

            if ( iReturn < EvEventCodes.Ok )
            {
              return iReturn;
            }
          }
        }
        else
        {
         this.LogDebug ( " >> Delete Activity " );
          iReturn = this.deleteActivity( activity );

          if ( iReturn < EvEventCodes.Ok )
          {
            return iReturn;
          }
        }//END Delete Activity

      }//END iteration loop.

      //
      // Return an enumerated value EventCode status.
      //
      return EvEventCodes.Ok;

    }//END updateActivities method

    // =====================================================================================
    /// <summary>
    /// This method updates items on Activity data table. 
    /// </summary>
    /// <param name="Activity">EvActivity: An activity object.</param>
    /// <returns>EvEventCodes: An event code for updating activity object.</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Exit, if the Milestone's Guid or ActivityId or Old Activity's Guid is empty. 
    /// 
    /// 2. Create a new DB row for Activity's Guid
    /// 
    /// 3. Update Activity's status to be completed if the user who completes the Activity is not empty.
    /// 
    /// 4. Define the sql query parameters and execute the storeprocedure for updating items. 
    /// 
    /// 5. Exit, if the storeprocedure runs fail. 
    /// 
    /// 6. Return an event code for updating items.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private EvEventCodes updateActivity( EvActivity Activity )
    {
      this.LogMethod ( "updateItem. " );
      this.LogDebug ( "Guid: " + Activity.Guid );
      this.LogDebug ( "MilestoneGuid: " + Activity.MilestoneGuid );
      this.LogDebug ( "ActivityId: " + Activity.ActivityId );
      this.LogDebug ( "Order: " + Activity.Order );
      this.LogDebug ( "Action: " + Activity.Action );

      // 
      // Validate whether the Milestone's Guid or ActivityId or Old Activity's Guid is not empty.
      //
      if ( Activity.MilestoneGuid == Guid.Empty )
      {
       this.LogDebug ( "No MilestoneGuid" );
        return EvEventCodes.Identifier_Milestone_Id_Error;
      }

      if ( Activity.ActivityId == String.Empty )
      {
        this.LogDebug( "No ActivityId" );
        return EvEventCodes.Identifier_Activity_Id_Error;
      }

      EvActivity oldActivity = this.getActivity( Activity.Guid );

      if ( oldActivity.Guid == Guid.Empty )
      {
        this.LogDebug( "No Milestone Activity in the database." );
        return EvEventCodes.Data_InvalidId_Error;
      }

      // 
      // Create a new DB row Activity's Guid
      // 
      if ( Activity.Guid == Guid.Empty )
      {
        Activity.Guid = Guid.NewGuid( );
      }
      this.LogDebug( "new Guid:" + Activity.Guid );

      //
      // Update Activity's status to be completed if the user who completes the Activity is not empty. 
      //
      if ( Activity.CompletedBy != String.Empty )
      {
        Activity.Status = EvActivity.ActivityStates.Completed;
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      this.LogDebug( "Update EvActivity details" );

      SqlParameter [ ] _cmdParms = getItemsParameters( );
      SetParameters( _cmdParms, Activity );

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate( _STORED_PROCEDURE_UpdateItem, _cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      // 
      // Return an enumerated value EventCode status.
      // 
      return EvEventCodes.Ok;

    }//END updateActivity method

    // =====================================================================================
    /// <summary>
    /// This method adds an activity into the database. 
    /// </summary>
    /// <param name="Activity">Object: An object containing EvActivity data object.</param>
    /// <returns>An enumerated value EventCode status.</returns>
    /// <remarks>
    /// This method consists of following steps.
    /// 
    /// 1. Exit, if the Milestone's Guid or ActivityId is empty. 
    /// 
    /// 2. Create a new DB row for Activity's Guid
    /// 
    /// 3. Update Activity's status to be completed if the user who completes the Activity is not empty.
    /// 
    /// 4. Define the sql query parameters and execute the storeprocedure for adding items. 
    /// 
    /// 5. Exit, if the storeprocedure runs fail. 
    /// 
    /// 6. Return an event code for updating items.
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private EvEventCodes addActivity( EvActivity Activity )
    {
      try
      {
        this.LogMethod ( "addItem. " );
        this.LogDebug ( "Guid: " + Activity.Guid );
        this.LogDebug ( "MilestoneGuid: " + Activity.MilestoneGuid );
        this.LogDebug ( "ActivityId: " + Activity.ActivityId );
        this.LogDebug ( "Order: " + Activity.Order );
        this.LogDebug ( "Action: " + Activity.Action );
        // 
        // Define the local variables.
        // 
        string sqlQueryString = String.Empty;

        // 
        // Validate whether the Milestone's Guid or ActivityId is empty.
        // 
        if ( Activity.MilestoneGuid == Guid.Empty )
        {
         this.LogDebug ( "No MilestoneGuid" );
          return EvEventCodes.Identifier_Milestone_Id_Error;
        }

        if ( Activity.ActivityId == String.Empty )
        {
          this.LogDebug( "No ActivityId" );
          return EvEventCodes.Identifier_Activity_Id_Error;
        }

        // 
        // Generate the row guid.
        // 
        Activity.Guid = Guid.NewGuid( );
        this.LogDebug( "Object Guid: " + Activity.Guid );

        //
        // Update Activity's status to be completed if the user who completes the Activity is not empty.
        //
        Activity.Status = EvActivity.ActivityStates.Created;
        if ( Activity.CompletedBy != String.Empty )
        {
          Activity.Status = EvActivity.ActivityStates.Completed;
        }

        // 
        // Define the SQL query parameters and load the query values.
        // 
        SqlParameter [ ] _cmdParms = getItemsParameters( );
        this.SetParameters( _cmdParms, Activity );

        //
        // Execute the update command.
        //
        if ( EvSqlMethods.StoreProcUpdate( _STORED_PROCEDURE_AddItem, _cmdParms ) == 0 )
        {
          this.LogDebug( "Update Error" );
          return EvEventCodes.Database_Record_Update_Error;
        }
        this.LogDebug( "Activity Added" );


      }
      catch ( Exception Ex )
      {
        string _eventLogSource = ConfigurationManager.AppSettings [ "EventLogSource" ];
        string eventMessage = this.Log
         + "\r\n" + Ex.Message
         + "\r\n" + Ex.Source
         + "\r\n" + Ex.StackTrace.ToString( )
         + "\r\n" + Ex.TargetSite.ToString( )
         + "\r\n" + Ex.InnerException;

        EventLog.WriteEntry( _eventLogSource, eventMessage,
          EventLogEntryType.Error );
        throw;

      }

      //
      // Return an enumerated value EventCode status.
      //
      return EvEventCodes.Ok;

    }//END addActivity method

    // =====================================================================================
    /// <summary>
    /// This method deletes items from the Activity data table.  
    /// </summary>
    /// <param name="Activity">EvActivity: An activity object</param>
    /// <returns>EvEventCodes: An event code for deleting items</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Define the SQL query parameters and execute the storeprocedure for deleting items
    /// 
    /// 2. Exit, if the storeprocedure runs fail
    /// 
    /// 3. Else, return an event code for deleting items.
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private EvEventCodes deleteActivity( EvActivity Activity )
    {
      this.LogMethod ( "deleteActivity. "
        + " ActivityGuid: " + Activity.Guid );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( _parmGuid, SqlDbType.UniqueIdentifier),
        new SqlParameter( _parmUpdateByUserId, SqlDbType.NVarChar,100),
        new SqlParameter( _parmUpdatedBy, SqlDbType.NText),
        new SqlParameter( _parmUpdated, SqlDbType.DateTime )
      };

      cmdParms [ 0 ].Value = Activity.Guid;
      cmdParms [ 1 ].Value = Activity.UpdatedByUserId;
      cmdParms [ 2 ].Value = Activity.UpdatedBy;
      cmdParms [ 3 ].Value = DateTime.Now;

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate( _STORED_PROCEDURE_DeleteItem, cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      //
      // Return an enumerated value EventCode status.
      //
      return EvEventCodes.Ok;

    }//END deleteActivity method

    #endregion


  }//END EvSubjectMilestoneActivities class

}//END namespace Evado.Dal.Clinical
