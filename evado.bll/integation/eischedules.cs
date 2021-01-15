/***************************************************************************************
 * <copyright file="BLL\EvDatabaseUpdates.cs" company="EVADO HOLDING PTY. LTD.">
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
 * Description: 
 *
 ****************************************************************************************/

using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;

using Evado.Model.Integration;
using Evado.Model.Digital;
using Evado.Model;
using Evado.Bll;

namespace Evado.Bll.Integration
{
  /// <summary>
  /// This class handles the Evado integration integration services.
  /// </summary>
  public class EiSchedules: EvBllBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EiSchedules ( )
    {
      this.ClassNameSpace = "Evado.Bll.Clinical.EiSchedules.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EiSchedules ( EvClassParameters Settings )
    {
      this.ClassParameter = Settings;
      this.ClassNameSpace = "Evado.Bll.Clinical.EiSchedules.";

      this._UserCommonName = this.ClassParameter.UserProfile.CommonName;
      this._UserId = this.ClassParameter.UserProfile.UserId;

      this._bllActivites = new Evado.Bll.Clinical.EvActivities ( Settings );
      this._bllEvSchedules = new Clinical.EvSchedules ( Settings );
      this._bllMilestones = new Evado.Bll.Clinical.EvMilestones ( Settings );
    }
    #endregion
    #region class properties and constants

    private const string CONST_SCHEDULE_PREFIX = "S_";
    private const string CONST_MILESTONE_PREFIX = "M_";

    private System.Text.StringBuilder _ProcessLog = new System.Text.StringBuilder ( );


    /// <summary>
    ///  This property contains the debug log entries.
    /// </summary>
    public String ProcessLog
    {
      get { return this._ProcessLog.ToString ( ); }
    }

    private String _UserCommonName = String.Empty;
    /// <summary>
    /// This property contains the users common name 
    /// </summary>
    public String UserCommonName
    {
      get { return _UserCommonName; }
      set { _UserCommonName = value; }
    }

    private String _UserId = String.Empty;
    /// <summary>
    /// This property contains the user identifier.
    /// </summary>
    public String UserId
    {
      get { return _UserId; }
      set { _UserId = value; }
    }

    private int _ImportIdentifierColumn = -1;
    private int _ImportProjectColumn = -1;
    private String _ProjectId = String.Empty;

    private Evado.Bll.Clinical.EvSchedules _bllEvSchedules = null;
    private Evado.Bll.Clinical.EvMilestones _bllMilestones = new Evado.Bll.Clinical.EvMilestones ( );

    private Evado.Bll.Clinical.EvActivities _bllActivites = new Evado.Bll.Clinical.EvActivities ( );
    private Evado.Model.Digital.EvSchedule _Schedule = new Evado.Model.Digital.EvSchedule ( );
    private List<Evado.Model.Digital.EvActivity> _ActivityList = new List<Evado.Model.Digital.EvActivity> ( );


    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Schedule Template methods
    //===================================================================================
    /// <summary>
    /// This method generates a data import template.
    /// </summary>
    /// <param name="QueryData">Evado.Model.Integration.EiData</param>
    /// <returns>Evado.Model.Integration.EiData object</returns>
    //-----------------------------------------------------------------------------------
    public Evado.Model.Integration.EiData getTemplateData (
      Evado.Model.Integration.EiData QueryData )
    {
      this.LogMethod ( "getTemplateData method." );
      this.writeProcessLog ( "Integration Service - Processing schedule template." );
      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.Integration.EiData resultData = QueryData;
      Evado.Model.Digital.EvSchedule schedule = new Evado.Model.Digital.EvSchedule ( );
      Evado.Model.Integration.EiColumnParameters column = new Evado.Model.Integration.EiColumnParameters ( );

      //
      // remove all but the customer and project id parameters if present.
      //
      for ( int i = 0; i < resultData.ParameterList.Count; i++ )
      {
        if ( resultData.ParameterList [ i ].Name != Model.Integration.EiQueryParameterNames.Customer_Id
          && resultData.ParameterList [ i ].Name != Model.Integration.EiQueryParameterNames.Project_Id )
        {
          resultData.ParameterList.RemoveAt ( i );
          i--;
        }
      }

      //
      // set the default activity columns
      //
      this.setDefaultColumns ( resultData );
      //
      // Initialise the ResultData row object
      //
      Evado.Model.Integration.EiDataRow row = resultData.AddDataRow ( );

      //
      // Add the subject's data to the output data array.
      //
      this.setScheduleColumnData ( 
        schedule, 
        resultData, 
        row );

      Evado.Model.Digital.EvMilestone milestone = new Evado.Model.Digital.EvMilestone();

      this.setMilestoneColumnData ( 
        milestone,
        resultData,
        row );

      return resultData;

    }//END SubjectQuery method

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Schedule Export methods

    //===================================================================================
    /// <summary>
    /// This method executes a subject query
    /// </summary>
    /// <param name="QueryData">Evado.Model.Integration.EiData</param>
    /// <returns>Evado.Model.Integration.EiData object</returns>
    //-----------------------------------------------------------------------------------
    public Evado.Model.Integration.EiData ExportData (
      Evado.Model.Integration.EiData QueryData )
    {
      this.LogMethod ( "ExportData method." );
      this.writeProcessLog ( "Integration Service - Processing schedule data." );
      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.Integration.EiData resultData = new EiData ( );
      int columnCount = 0;
      int scheduleId = 1;
      //
      // get the project and schedule identifiers.
      //
      String projectId = QueryData.GetQueryParameterValue ( EiQueryParameterNames.Project_Id );
      String stScheduleId = QueryData.GetQueryParameterValue ( EiQueryParameterNames.Schedule_Id );

      if ( int.TryParse ( stScheduleId, out scheduleId ) == false )
      {
        scheduleId = 1;
      }

      //
      // Initialise the result data objects.
      //
      resultData.QueryType = QueryData.QueryType;
      resultData.ParameterList = new List<EiQueryParameter> ( );

      if ( QueryData.hasQueryParameter ( EiQueryParameterNames.Project_Id ) == true )
      {
        resultData.AddQueryParameter (
          EiQueryParameterNames.Project_Id, projectId );
      }

      if ( QueryData.hasQueryParameter ( EiQueryParameterNames.Schedule_Id ) == true )
      {
        resultData.AddQueryParameter ( EiQueryParameterNames.Schedule_Id, scheduleId.ToString ( ) );
      }

      //
      // reset the data rows.
      //
      resultData.DataRows = new List<Model.Integration.EiDataRow> ( );

      //
      // Query the database.
      //
      Evado.Model.Integration.EiEventCodes result = this.getSchedule ( projectId, scheduleId );

      //
      // return error 
      if ( result != EiEventCodes.Ok )
      {
        resultData.ProcessLog = this._ProcessLog.ToString ( );
        resultData.EventCode = result;
        return resultData;
      }

      //
      // Define the columns in the output ResultData set.
      //
      if ( QueryData.Columns == null )
      {
        this.LogDebug ( "QueryData.Columns = null." );

        QueryData.Columns = new List<Model.Integration.EiColumnParameters> ( );
      }

      //
      // if null then initialise and create the default columns.
      //
      if ( QueryData.Columns.Count == 0 )
      {
        this.LogDebug ( "QueryData.Columns.Count = 0" );

        this.setDefaultColumns ( resultData );
      }
      else
      {
        this.LogDebug ( "QueryData.Columns.Count = " + QueryData.Columns.Count );

        resultData.Columns = QueryData.Columns;
      }

      columnCount = resultData.Columns.Count;

      this.LogDebug ( "columnsCount: " + columnCount );

      //
      // Iterate through the return list extracting the required ResultData values.
      //
      for ( int milestoneCount = 0; milestoneCount < this._Schedule.Milestones.Count; milestoneCount++ )
      {
        //
        // get the milestone to be exported.
        //
        Evado.Model.Digital.EvMilestone milestone = this._Schedule.Milestones [ milestoneCount ];

        this.LogDebug ( "Processing milestone: " + milestone.MilestoneId );

        //
        // Initialise the ResultData row object
        //
        Evado.Model.Integration.EiDataRow row = resultData.AddDataRow ( );

        //
        // Add the subject's data to the output data array.
        //
        //if ( milestoneCount == 0 )
        //{
          this.setScheduleColumnData ( this._Schedule, resultData, row );
        //}

        //
        // Add the subject's data to the output data array.
        //
        this.setMilestoneColumnData ( milestone, resultData, row );

      }//END data row iteration loop.

      this.LogDebug ( "data.DataRows.Count: " + resultData.DataRows.Count );

      resultData.ProcessLog = this._ProcessLog.ToString ( );

      this.LogDebug ( "********************* FINISH EXPORT DATA METHOD *************************" );

      return resultData;

    }//END exportData method

    //===================================================================================
    /// <summary>
    /// This method defines the default subject result columns.
    /// </summary>
    /// <param name="ResultData">Evado.Model.Integration.EiData object</param>
    //-----------------------------------------------------------------------------------
    private void setDefaultColumns (
      Evado.Model.Integration.EiData ResultData )
    {
      this.LogMethod ( "setDefaultSubjectColumns method." );

      ResultData.Columns = new List<Model.Integration.EiColumnParameters> ( );
      Evado.Model.Integration.EiColumnParameters column = new EiColumnParameters ( );

      //
      // Add column for project identifier
      //
      ResultData.AddColumn (
        EiDataTypes.Text,
        Evado.Model.Digital.EvSchedule.ScheduleClassFieldNames.TrialId,
        false );

      //
      // Add column for schedule identifier
      //
      ResultData.AddColumn (
        EiDataTypes.Text,
        EiSchedules.CONST_SCHEDULE_PREFIX + Evado.Model.Digital.EvSchedule.ScheduleClassFieldNames.ScheduleId,
        false );

      //
      // Add column for schedule title
      //      
      ResultData.AddColumn (
        EiDataTypes.Text,
        EiSchedules.CONST_SCHEDULE_PREFIX + Evado.Model.Digital.EvSchedule.ScheduleClassFieldNames.Title,
        false );

      //
      // Add column for schedule description
      //
      ResultData.AddColumn (
        EiDataTypes.Text,
        EiSchedules.CONST_SCHEDULE_PREFIX + Evado.Model.Digital.EvSchedule.ScheduleClassFieldNames.Description,
        false );

      //
      // Add column for Milestone identifier
      //
      ResultData.AddColumn (
        EiDataTypes.Text,
        EiSchedules.CONST_MILESTONE_PREFIX + Evado.Model.Digital.EvMilestone.MilestoneClassFieldNames.MilestoneId,
        true );

      //
      // Add column for Milestone identifier
      //
      ResultData.AddColumn (
        EiDataTypes.Text,
        EiSchedules.CONST_MILESTONE_PREFIX + Evado.Model.Digital.EvMilestone.MilestoneClassFieldNames.Title,
        false );

      //
      // Add column for Milestone description
      //
      ResultData.AddColumn (
        EiDataTypes.Text,
        EiSchedules.CONST_MILESTONE_PREFIX + Evado.Model.Digital.EvMilestone.MilestoneClassFieldNames.Description,
        false );

      //
      // Add column for Milestone type
      //
      ResultData.AddColumn (
        EiDataTypes.Text,
        EiSchedules.CONST_MILESTONE_PREFIX + Evado.Model.Digital.EvMilestone.MilestoneClassFieldNames.Order,
        false );

      //
      // Add column for Milestone type
      //
      ResultData.AddColumn (
        EiDataTypes.Text,
        EiSchedules.CONST_MILESTONE_PREFIX + Evado.Model.Digital.EvMilestone.MilestoneClassFieldNames.Type,
        false );

      //
      // Add column for Milestone inter visit period in milestone period increments
      //
      ResultData.AddColumn (
        EiDataTypes.Text,
        EiSchedules.CONST_MILESTONE_PREFIX + Evado.Model.Digital.EvMilestone.MilestoneClassFieldNames.Inter_Visit_Period,
        false );

      //
      // Add column for Milestone inter visit range in milestone period increments
      //
      ResultData.AddColumn (
        EiDataTypes.Text,
        EiSchedules.CONST_MILESTONE_PREFIX + Evado.Model.Digital.EvMilestone.MilestoneClassFieldNames.Milestone_Range,
        false );

      //
      // Add column for Milestone mandatory clinical activity
      //
      ResultData.AddColumn (
        EiDataTypes.Text,
        EiSchedules.CONST_MILESTONE_PREFIX + Evado.Model.Digital.EvMilestone.MilestoneClassFieldNames.Mandatory_Activity,
        false );

      //
      // Add column for Milestone mandatory clinical activity
      //
      ResultData.AddColumn (
        EiDataTypes.Text,
        EiSchedules.CONST_MILESTONE_PREFIX + Evado.Model.Digital.EvMilestone.MilestoneClassFieldNames.Optional_Activity,
        false );

      //
      // Add column for Milestone mandatory clinical activity
      //
      ResultData.AddColumn (
        EiDataTypes.Text,
        EiSchedules.CONST_MILESTONE_PREFIX + Evado.Model.Digital.EvMilestone.MilestoneClassFieldNames.Non_Clinical_Activity,
        false );

    }//END setDefaultSubjectColumns method.

    //===================================================================================
    /// <summary>
    /// This method defines the default subject result columns.
    /// </summary>
    /// <param name="Schedule">Evado.Model.Digital.EvSchedule object</param>
    /// <param name="ResultData">Evado.Model.Integration.EiData object</param>
    /// <param name="row">Evado.Model.Integration.EiDataRow object</param>
    //-----------------------------------------------------------------------------------
    private void setScheduleColumnData (
      Evado.Model.Digital.EvSchedule Schedule,
      Evado.Model.Integration.EiData ResultData,
       Evado.Model.Integration.EiDataRow row )
    {
      this.LogMethod ( "setScheduleColumnData method." );

      //
      // Add column ResultData based on where the column is in the column header
      //
      int column = ResultData.getColumnNo (
        Evado.Model.Digital.EvSchedule.ScheduleClassFieldNames.TrialId );
      if ( column > -1 )
      {
        this.LogDebug ( "Output ProjectId : " + Schedule.TrialId );
        row.updateValue ( column, Schedule.TrialId );
      }

      column = ResultData.getColumnNo (
        EiSchedules.CONST_SCHEDULE_PREFIX + Evado.Model.Digital.EvSchedule.ScheduleClassFieldNames.ScheduleId );
      if ( column > -1 )
      {
        this.LogDebug ( "Output ScheduleId : " + Schedule.ScheduleId );
        row.updateValue ( column, Schedule.ScheduleId );
      }

      column = ResultData.getColumnNo (
        EiSchedules.CONST_SCHEDULE_PREFIX + Evado.Model.Digital.EvSchedule.ScheduleClassFieldNames.Title );
      if ( column > -1 )
      {
        this.LogDebug ( "Output Title : " + Schedule.Title );
        row.updateValue ( column, Schedule.Title );
      }

      column = ResultData.getColumnNo (
        EiSchedules.CONST_SCHEDULE_PREFIX + Evado.Model.Digital.EvSchedule.ScheduleClassFieldNames.Description );

      if ( column > -1 )
      {
        this.LogDebug ( "Output Description : " + Schedule.Description );
        row.updateValue ( column, Schedule.Description );
      }

      column = ResultData.getColumnNo (
        EiSchedules.CONST_SCHEDULE_PREFIX + Evado.Model.Digital.EvSchedule.ScheduleClassFieldNames.Milestone_Period_Increment );

      if ( column > -1 )
      {
        this.LogDebug ( "Output MilestonePeriodIncrement : " + Schedule.MilestonePeriodIncrement );
        row.updateValue ( column, Schedule.MilestonePeriodIncrement.ToString() );
      }

    }//END setSubjectColumnData method

    //===================================================================================
    /// <summary>
    /// This method defines the default subject result columns.
    /// </summary>
    /// <param name="Milestone">Evado.Model.Digital.EvSchedule object</param>
    /// <param name="ResultData">Evado.Model.Integration.EiData object</param>
    /// <param name="DataRow">Evado.Model.Integration.EiDataRow object</param>
    //-----------------------------------------------------------------------------------
    private void setMilestoneColumnData (
      Evado.Model.Digital.EvMilestone Milestone,
      Evado.Model.Integration.EiData ResultData,
       Evado.Model.Integration.EiDataRow DataRow )

    {
      this.LogMethod ( "setMilestoneColumnData method." );
      //
      // Initialise the methods variables and objects.
      //
      int column = -1;
      String value = String.Empty;
      Evado.Model.Digital.EvActivity activity = new Evado.Model.Digital.EvActivity ( );
      this.LogDebug ( "row column count: " + DataRow.Values.Length );

      //
      // Add column ResultData based on where the column is in the column header list
      //
      column = ResultData.getColumnNo (
        Evado.Model.Digital.EvSchedule.ScheduleClassFieldNames.TrialId );
      if ( column > -1 )
      {
        DataRow.updateValue ( column, Milestone.ProjectId );

        this.LogDebug ( "row col:  " + column + ", ProjectId: " + DataRow.Values [ column ] );
      }

      column = ResultData.getColumnNo (
        EiSchedules.CONST_MILESTONE_PREFIX + Evado.Model.Digital.EvMilestone.MilestoneClassFieldNames.MilestoneId );
      if ( column > -1 )
      {
        DataRow.updateValue ( column, Milestone.MilestoneId );

        this.LogDebug ( "row col:  " + column + ", MilestoneId: " + DataRow.Values [ column ] );
      }

      column = ResultData.getColumnNo (
        EiSchedules.CONST_MILESTONE_PREFIX + Evado.Model.Digital.EvMilestone.MilestoneClassFieldNames.Title );
      if ( column > -1 )
      {
        DataRow.updateValue ( column, Milestone.Title );

        this.LogDebug ( "row col:  " + column + ", Title: " + DataRow.Values [ column ] );
      }

      column = ResultData.getColumnNo (
        EiSchedules.CONST_MILESTONE_PREFIX + Evado.Model.Digital.EvMilestone.MilestoneClassFieldNames.Description );

      if ( column > -1 )
      {
        DataRow.updateValue ( column, Milestone.Description );

        this.LogDebug ( "row col:  " + column + ", Description: " + DataRow.Values [ column ] );
      }

      column = ResultData.getColumnNo (
        EiSchedules.CONST_MILESTONE_PREFIX + Evado.Model.Digital.EvMilestone.MilestoneClassFieldNames.Order );

      if ( column > -1 )
      {
        DataRow.updateValue ( column, Milestone.Order );
        this.LogDebug ( "row col:  " + column + ", Order: " + DataRow.Values [ column ] );
      }

      column = ResultData.getColumnNo (
        EiSchedules.CONST_MILESTONE_PREFIX + Evado.Model.Digital.EvMilestone.MilestoneClassFieldNames.Type );

      if ( column > -1 )
      {
        DataRow.updateValue ( column, Milestone.Type );

        this.LogDebug ( "row col:  " + column + ", Type: " + DataRow.Values [ column ] );
      }

      column = ResultData.getColumnNo (
        EiSchedules.CONST_MILESTONE_PREFIX + Evado.Model.Digital.EvMilestone.MilestoneClassFieldNames.Inter_Visit_Period );

      if ( column > -1 )
      {
        DataRow.updateValue ( column, Milestone.InterMilestonePeriod );

        this.LogDebug ( "row col:  " + column + ", InterMilestonePeriod: " + DataRow.Values [ column ] );
      }

      column = ResultData.getColumnNo (
        EiSchedules.CONST_MILESTONE_PREFIX + Evado.Model.Digital.EvMilestone.MilestoneClassFieldNames.Milestone_Range );

      if ( column > -1 )
      {
        DataRow.updateValue ( column, Milestone.MilestoneRange );

        this.LogDebug ( "row col:  " + column + ", MilestoneRange: " + DataRow.Values [ column ] );
      }

      column = ResultData.getColumnNo (
        EiSchedules.CONST_MILESTONE_PREFIX + Evado.Model.Digital.EvMilestone.MilestoneClassFieldNames.Mandatory_Activity );
      activity = Milestone.getClinicalActivity ( true );

      if ( column > -1
        && activity != null )
      {
        value = activity.ActivityId;
        DataRow.updateValue ( column, value );
      }

      column = ResultData.getColumnNo (
        EiSchedules.CONST_MILESTONE_PREFIX + Evado.Model.Digital.EvMilestone.MilestoneClassFieldNames.Optional_Activity );
      activity = Milestone.getClinicalActivity ( false );

      if ( column > -1
        && value != String.Empty
        && activity != null )
      {
        value = activity.ActivityId;
        DataRow.updateValue ( column, value );
      }

      column = ResultData.getColumnNo (
        EiSchedules.CONST_MILESTONE_PREFIX + Evado.Model.Digital.EvMilestone.MilestoneClassFieldNames.Non_Clinical_Activity );
      List<Evado.Model.Digital.EvActivity> activityList = Milestone.getNonClinicalActivities ( );

      if ( activityList.Count > 0
        && column > -1 )
      {
        value = String.Empty;
        //
        // Iterate through the list of activities adding them to the value as ';' delimited values.
        //
        foreach ( Evado.Model.Digital.EvActivity activity1 in activityList )
        {
          if ( value != String.Empty )
          {
            value += ";";
          }
          value += activity1.ActivityId;
        }

        //
        // add the value to the column.
        //
        DataRow.updateValue ( column, value );
      }

    }//END setSubjectColumnData method

    //===================================================================================
    /// <summary>
    /// This method retrieves the current schedule
    /// </summary>
    /// <param name="ProjectId">String: project identifier</param>
    /// <param name="ScheduleId">int: schedule identifier</param>
    /// <returns>Evado.Model.Integration.EiEventCodes</returns>
    //-----------------------------------------------------------------------------------
    private Evado.Model.Integration.EiEventCodes getSchedule (
      String ProjectId,
      int ScheduleId )
    {
      this.LogMethod ( "getSchedule method." );
      //
      // Initialise the methods variables and objects.
      //
      String value = String.Empty;
      List<Evado.Model.Digital.EvSchedule> scheduleList = new List< Evado.Model.Digital.EvSchedule> ( );
      this._Schedule = new Evado.Model.Digital.EvSchedule ( );

      //
      // If not project id then exit method with error.
      //
      if ( ProjectId == String.Empty )
      {
        this.LogDebug ( "ProjectId empty." );

        return EiEventCodes.Integration_Import_Project_Id_Error;
      }

      this.LogDebug ( "ProjectId: " + ProjectId );

      this.LogDebug ( "ScheduleId: " + ScheduleId );

      //
      // retrieve a list of scheduled for the project and scheduleID.
      //
      this._Schedule = this._bllEvSchedules.getSchedule ( ProjectId, ScheduleId, true );

      this.LogDebug ( "Schedule debuglog: " + this._bllEvSchedules.Log );

      return EiEventCodes.Ok;

    }//END getSchedule method.

    //===================================================================================
    /// <summary>
    /// This method retrieves the current schedule
    /// </summary>
    /// <param name="QueryData">Evado.Model.Integration.EiData object</param>
    /// <returns>Evado.Model.Integration.EiEventCodes</returns>
    //-----------------------------------------------------------------------------------
    private Evado.Model.Integration.EiEventCodes getImportSchedule (
      Evado.Model.Integration.EiData QueryData )
    {
      //
      // Initialise the methods variables and objects.
      //
      String projectId = String.Empty;
      int scheduleId = 1;
      List<Evado.Model.Digital.EvSchedule> scheduleList = new List< Evado.Model.Digital.EvSchedule> ( );
      this._Schedule = new Evado.Model.Digital.EvSchedule ( );

      //
      // If not row then there is not import data.
      //
      if ( QueryData.DataRows.Count == 0 )
      {
        return EiEventCodes.Integration_Import_General_Error;
      }

      //
      // Add column ResultData based on where the column is in the column header
      //
      int column = QueryData.getColumnNo (
        Evado.Model.Digital.EvSchedule.ScheduleClassFieldNames.TrialId );

      if ( column == -1 )
      {
        return EiEventCodes.Integration_Import_Project_Id_Error;
      }

      projectId = QueryData.DataRows [ 0 ].Values [ column ];

      //
      // get the schedule identifier.
      //
      String value = QueryData.GetQueryParameterValue ( EiQueryParameterNames.Schedule_Id );

      if ( value != String.Empty )
      {
        if ( int.TryParse ( value, out scheduleId ) == false )
        {
          return EiEventCodes.Integration_Import_Schedule_Id_Error;
        }
      }

      //
      // retrieve a list of scheduled for the project and scheduleID.
      //
      scheduleList = this._bllEvSchedules.getScheduleList ( projectId, scheduleId, Evado.Model.Digital.EvSchedule.ScheduleStates.Draft );

      this.LogDebug ( "Schedules debug: " + this._bllEvSchedules.Log );

      //
      // If the draft schedule count is 0 revise the current issued schedule version.
      //
      if ( scheduleList.Count == 0 )
      {
        this.getSchedule ( projectId, scheduleId );

        this._Schedule.UserCommonName = this._UserCommonName;
        this._Schedule.UpdatedByUserId = this._UserId;
        this._Schedule.Action = Evado.Model.Digital.EvSchedule.ScheduleActions.Revise;

        Evado.Model.EvEventCodes result = this._bllEvSchedules.saveSchedule ( this._Schedule );

        this.LogDebug ( "Schedules debug: " + this._bllEvSchedules.Log );
        //
        // refill the list to include the revised schedule
        //
        scheduleList = this._bllEvSchedules.getScheduleList ( projectId, scheduleId, Evado.Model.Digital.EvSchedule.ScheduleStates.Draft );

        this.LogDebug ( "Schedules debug: " + this._bllEvSchedules.Log );
      }

      //
      // iterate through the schedules to return the relevant schedule version to the import export process.
      //
      foreach ( Evado.Model.Digital.EvSchedule schedule in scheduleList )
      {
        this._Schedule = schedule;

      }//END schedule iteration loop


      this.LogDebug ( "Schedule title: " + this._Schedule.Title + ", State: " + this._Schedule.State );

      return EiEventCodes.Ok;

    }//END getSchedule method.

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Schedule Import methods

    //===================================================================================
    /// <summary>
    /// This method executes a subject query
    /// </summary>
    /// <param name="ImportData">Evado.Model.Integration.EiData</param>
    /// <returns>Evado.Model.Integration.EiData object</returns>
    //-----------------------------------------------------------------------------------
    public Evado.Model.Integration.EiData ImportData (
      Evado.Model.Integration.EiData ImportData )
    {
      this.LogMethod ( "ImportData method." );
      this.writeProcessLog ( "Integration Service - Schedule import process commenced." );
      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.Integration.EiData returnData = new Model.Integration.EiData ( );
      returnData.ParameterList = ImportData.ParameterList;
      returnData.Columns = ImportData.Columns;

      this.LogDebug ( "EvStaticSetting.DebugOn: " + EvStaticSetting.DebugOn );

      this.LogDebug ( "returnData.Columns.Count: + " + returnData.Columns.Count );

      //
      // If there is no data exit import with an iinport error.
      //
      if ( ImportData.DataRows.Count == 0 )
      {
        returnData.ProcessLog = this._ProcessLog.ToString ( );
        returnData.EventCode = Evado.Model.Integration.EiEventCodes.Integration_Import_General_Error;
        return returnData;
      }

      //
      // Query the database.
      //
      this.LogDebug ( "Import the schedule data " );

      Evado.Model.Integration.EiEventCodes result = this.getImportSchedule ( ImportData );

      //
      // return error 
      //
      if ( result != EiEventCodes.Ok )
      {
        returnData.ProcessLog = this._ProcessLog.ToString ( );
        returnData.EventCode = result;
        return returnData;
      }

      //
      // Get the list of activities to they can be linked added to the milestone.
      //
      this.LogDebug ( "Generate the list of activities " );
      result = this.getActivityList ( );

      if ( result != EiEventCodes.Ok )
      {
        returnData.ProcessLog = this._ProcessLog.ToString ( );
        returnData.EventCode = result;
        return returnData;
      }

      this.LogDebug ( "DataRows.Count: " + ImportData.DataRows.Count );

      //
      // set the import index identifier.
      //
      this.setImportIdentifier ( ImportData );

      //
      // If there is no import identifier return and error and abort the data import. 
      //
      if ( this._ImportIdentifierColumn == -1 )
      {
        returnData.EventCode = Model.Integration.EiEventCodes.Integration_Import_External_Identifier_Error;
        returnData.ErrorMessage = "Import data does not have a unique identifier indexed, i.e. one column must index value set to true.";

        this.LogDebug ( "EventCode: " + returnData.EventCode );

        this.writeProcessLog ( "Integration Service - Error event: " +
          Evado.Model.EvStatics.Enumerations.enumValueToString ( returnData.EventCode ) );

        returnData.ProcessLog = this._ProcessLog.ToString ( );

        return returnData;
      }

      //
      // Iterate through the data rows processing each row.
      //
      for ( int rowCount = 0; rowCount < ImportData.DataRows.Count; rowCount++ )
      {
        this.LogDebug ( "Process row: " + rowCount );

        if ( rowCount == 0 )
        {
          //
          // execute the subject update method.
          //
          if ( this.updateScheduleData ( ImportData, returnData, rowCount ) != Model.Integration.EiEventCodes.Ok )
          {
            returnData.ProcessLog = this._ProcessLog.ToString ( );

            return returnData;
          }
        }

        //
        // Get the data row
        //
        Evado.Model.Integration.EiDataRow dataRow = ImportData.DataRows [ rowCount ];
        Evado.Model.Integration.EiDataRow resultRow = new Model.Integration.EiDataRow ( dataRow.Columns );

        this.LogDebug ( "Identifier: " + this._ImportIdentifierColumn + " Identifier value: " + dataRow.Values [ this._ImportIdentifierColumn ] );

        this.LogDebug ( "Result value size: + " + resultRow.Values.Length );

        //
        // execute the milestone update method.
        //
        if ( this.updateMilestoneData ( ImportData, returnData, rowCount ) != Model.Integration.EiEventCodes.Ok )
        {
          returnData.ProcessLog = this._ProcessLog.ToString ( );

          return returnData;
        }

      }//END row iteration loop


      this.writeProcessLog ( "Integration Service - Subject import process completed." );

      returnData.ProcessLog = this._ProcessLog.ToString ( );

      return returnData;

    }//END SubjectQuery method

    //==================================================================================
    /// <summary>
    /// This method sets the project and import key column identifiers
    /// </summary>
    /// <param name="ImportData">Evado.Model.Integration.EiData object</param>
    /// <returns>Bool:  true = found</returns>
    //-----------------------------------------------------------------------------------
    private bool setImportIdentifier (
      Evado.Model.Integration.EiData ImportData )
    {
      //
      // Set the column containing the project identifier
      //
      this._ImportProjectColumn = ImportData.getColumnNo ( Evado.Model.Digital.EvSchedule.ScheduleClassFieldNames.TrialId );

      //
      // Set the column containing the activity id which is the external identifier.
      //
      this._ImportIdentifierColumn = ImportData.getColumnNo ( Evado.Model.Digital.EvMilestone.MilestoneClassFieldNames.MilestoneId );

      //
      // if the identifiers match and the index is true this is the import indec to be used.
      //
      if ( ImportData.Columns [ this._ImportIdentifierColumn ].Index == true )
      {
        return true;
      }

      return false;
    }

    //===================================================================================
    /// <summary>
    /// This method processes a row of import data to create or update a subject object.
    /// </summary>
    /// <param name="ImportData">Evado.Model.Integration.EiData import data object</param>
    /// <param name="ExportData">Evado.Model.Integration.EiData result data object</param>
    /// <param name="RowIndex">int: index to the row to be imported</param>
    /// <returns>Evado.Model.Integration.EiEventCodes enumeration</returns>
    //-----------------------------------------------------------------------------------
    private Evado.Model.Integration.EiEventCodes updateScheduleData (
      Evado.Model.Integration.EiData ImportData,
      Evado.Model.Integration.EiData ExportData,
      int RowIndex )
    {
      this.LogMethod ( "updateScheduleData method." );

      //
      // Initialise the methods variables and objects.
      //
      String value = String.Empty;
      int scheduleId = 0;
      int columnIndex = -1;

      //
      // Set the import and result row references.
      //
      Evado.Model.Integration.EiDataRow importRow = ImportData.DataRows [ RowIndex ];
      Evado.Model.Integration.EiDataRow resultRow = ExportData.AddDataRow ( );

      //
      // If the subject guid is empty no subject was found so create a new subject.
      //
      if ( this._Schedule.Guid == Guid.Empty )
      {
        this.LogDebug ( "Creating a new schedule" );

        columnIndex = ImportData.getColumnNo ( Evado.Model.Digital.EvSchedule.ScheduleClassFieldNames.TrialId );
        value = ImportData.DataRows [ RowIndex ].Values [ columnIndex ];

        this._Schedule.TrialId = value;

        columnIndex = ImportData.getColumnNo (
          EiSchedules.CONST_SCHEDULE_PREFIX + Evado.Model.Digital.EvSchedule.ScheduleClassFieldNames.ScheduleId );
        value = ImportData.DataRows [ RowIndex ].Values [ columnIndex ];
        if ( value != String.Empty )
        {
          if ( int.TryParse ( value, out scheduleId ) == true )
          {
            this._Schedule.ScheduleId = scheduleId;
          }
        }
        this.writeProcessLog ( "Integration Service - Schedule " + this._Schedule.ScheduleId + " created." );
        this.LogDebug ( "new Schedule: " + this._Schedule.ScheduleId );

      }//END create new subject.
      else
      {
        this.writeProcessLog ( "Integration Service - Subject " + this._Schedule.ScheduleId + " updated." );
      }

      //
      // Update title value 
      //
      columnIndex = ImportData.getColumnNo ( Evado.Model.Digital.EvSchedule.ScheduleClassFieldNames.Title );
      if ( columnIndex > -1 )
      {
        this._Schedule.Title = ImportData.DataRows [ RowIndex ].Values [ columnIndex ]; ;
      }

      //
      // Update Description value 
      //
      columnIndex = ImportData.getColumnNo ( Evado.Model.Digital.EvSchedule.ScheduleClassFieldNames.Description );
      if ( columnIndex > -1 )
      {
        this._Schedule.Description = ImportData.DataRows [ RowIndex ].Values [ columnIndex ]; ;
      }

      //
      // Update Description value 
      //
      columnIndex = ImportData.getColumnNo ( Evado.Model.Digital.EvSchedule.ScheduleClassFieldNames.Milestone_Period_Increment );
      if ( columnIndex > -1 )
      {
        value = ImportData.DataRows [ RowIndex ].Values [ columnIndex ];
        this._Schedule.MilestonePeriodIncrement = Evado.Model.EvStatics.Enumerations.parseEnumValue<
          Evado.Model.Digital.EvSchedule.MilestonePeriodIncrements> ( value );
      }

      //
      // Set the save parameters
      //
      this._Schedule.UserCommonName = this._UserCommonName;
      this._Schedule.UpdatedByUserId = this._UserId;
      this._Schedule.Action = Evado.Model.Digital.EvSchedule.ScheduleActions.Save;

      //
      // Save the updated sechedule object to the database.
      //
      Evado.Model.EvEventCodes result = this._bllEvSchedules.saveSchedule (
        this._Schedule );


      if ( result != Evado.Model.EvEventCodes.Ok )
      {
        this.LogDebug ( this._bllEvSchedules.Log );

        this.writeProcessLog ( "Integration Service - Error occured saving schedule " + this._Schedule.Title + "." );
        this.LogDebug ( "Error occured saving schedule " + this._Schedule.Title + "."
          + " Eventcode: " + result );

        return Model.EvStatics.getEvadoEventCode<Model.Integration.EiEventCodes> ( result );
      }

      this.LogDebug ( "EXIT: Schedule object has been processed." );

      return Model.Integration.EiEventCodes.Ok;

    }//END updateScheduleData method

    //===================================================================================
    /// <summary>
    /// This method processes a row of import data to create or update a subject object.
    /// </summary>
    /// <param name="ReturnData">Evado.Model.Integration.EiData import data object</param>
    /// <param name="ExportData">Evado.Model.Integration.EiData result data object</param>
    /// <param name="RowIndex">int: index to the row to be imported</param>
    /// <returns>Evado.Model.Integration.EiEventCodes enumeration</returns>
    //-----------------------------------------------------------------------------------
    private Evado.Model.Integration.EiEventCodes updateMilestoneData (
      Evado.Model.Integration.EiData ReturnData,
      Evado.Model.Integration.EiData ExportData,
      int RowIndex )
    {
      this.LogMethod ( "updateMilestoneData method." );

      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.Digital.EvMilestone milestone = new Evado.Model.Digital.EvMilestone ( );
      String value = String.Empty;
      int columnIndex = -1;
      this.LogDebug ( "EvStaticSetting.DebugOn: " + EvStaticSetting.DebugOn );
      Evado.Model.Integration.EiEventCodes result = EiEventCodes.Null;

      //
      // Set the import and result row references.
      //
      Evado.Model.Integration.EiDataRow importRow = ReturnData.DataRows [ RowIndex ];
      Evado.Model.Integration.EiDataRow resultRow = ExportData.AddDataRow ( );



      String activityIdentifier = importRow.Values [ this._ImportIdentifierColumn ];
      this.LogDebug ( "External identifier: " + activityIdentifier );

      //
      // Retrieve the subject using it activity identifier.
      //
      milestone = this.getMilestone ( activityIdentifier );

      //
      // If the subject guid is empty no subject was found so create a new subject.
      //
      if ( milestone.Guid == Guid.Empty )
      {
        this.LogDebug ( "Creating a new Milestone" );

        milestone.ProjectId = this._ProjectId;

        columnIndex = ReturnData.getColumnNo ( Evado.Model.Digital.EvMilestone.MilestoneClassFieldNames.MilestoneId );
        value = ReturnData.DataRows [ RowIndex ].Values [ columnIndex ];

        milestone.MilestoneId = value;

        this.writeProcessLog ( "Integration Service - Milestone " + milestone.MilestoneId + " created." );
        this.LogDebug ( "new MilestoneId: " + milestone.MilestoneId );

      }//END create new subject.
      else
      {
        this.writeProcessLog ( "Integration Service - Milestone " + milestone.MilestoneId + " updated." );
      }

      //
      // import the milestone's data.
      //
      result = this.importMilestoneColumnData (
         milestone,
         ReturnData,
         importRow,
         resultRow );

      if ( result != EiEventCodes.Ok )
      {
        this.writeProcessLog ( "Integration Service - Error processing milestone " + milestone.MilestoneId + "." );
        return result;
      }

      //
      // Set the save parameters
      //
      milestone.UserCommonName = this._UserCommonName;
      milestone.UpdatedByUserId = this._UserId;
      milestone.Action = Evado.Model.Digital.EvMilestone.MilestoneSaveActions.Save;

      //
      // Save the updated subject object to the database.
      //
      Evado.Model.EvEventCodes eResult = this._bllMilestones.saveItem ( milestone );


      if ( eResult != Evado.Model.EvEventCodes.Ok )
      {
        this.LogDebug ( this._bllEvSchedules.Log );

        this.writeProcessLog ( "Integration Service - Error occured saving milestone " + milestone.MilestoneId + "." );
        this.LogDebug ( "Error occured saving milestone " + milestone.MilestoneId + "."
          + " Eventcode: " + result );

        return Model.EvStatics.getEvadoEventCode<Model.Integration.EiEventCodes> ( eResult );
      }

      this.LogDebug ( "EXIT: Subject has been processed." );

      return Model.Integration.EiEventCodes.Ok;

    }//END updateMilestoneData method

    //===================================================================================
    /// <summary>
    /// THis method returns a activity matching the passed activity identifier.
    /// </summary>
    /// <param name="MilestoneId">String: Activity identifier</param>
    /// <returns>Evado.Model.Digital.EvSchedule</returns>
    //-----------------------------------------------------------------------------------
    private Evado.Model.Digital.EvMilestone getMilestone ( String MilestoneId )
    {
      this.LogMethod ( "getActivity method." );
      //
      // Iterate through each of the activities in the list and return the matching activity.
      //
      foreach ( Evado.Model.Digital.EvMilestone milestone in this._Schedule.Milestones )
      {
        if ( milestone.MilestoneId.ToLower ( ) == MilestoneId.ToLower ( ) )
        {
          return milestone;
        }
      }

      //
      // Return an empty activity is none is found.
      //
      return new Evado.Model.Digital.EvMilestone ( );
    }

    //===================================================================================
    /// <summary>
    /// This method defines the default subject result columns.
    /// </summary>
    /// <param name="Milestone">Evado.Model.Digital.EvMilestone object</param>
    /// <param name="ResultData">Evado.Model.Integration.EiData object</param>
    /// <param name="ImportRow">Evado.Model.Integration.EiDataRow object</param>
    /// <param name="ResultRow">Evado.Model.Integration.EiDataRow object containing validation results</param>
    //-----------------------------------------------------------------------------------
    private Evado.Model.Integration.EiEventCodes importMilestoneColumnData (
      Evado.Model.Digital.EvMilestone Milestone,
      Evado.Model.Integration.EiData ResultData,
      Evado.Model.Integration.EiDataRow ImportRow,
      Evado.Model.Integration.EiDataRow ResultRow )
    {
      this.LogMethod ( "importMilestoneColumnData method." );
      this.writeProcessLog ( "Integration Service - Updating milestone object data." );

      //
      // Add column ResultData based on where the column is in the column header
      //
      int columnIndex = -1;
      String value = String.Empty;
      int iValue = 0;
      float fValue = 0;
      Milestone.ActivityList = new List< Evado.Model.Digital.EvActivity> ( );

      //
      // Import the milestone title.
      //
      this.LogDebug ( "Title update." );
      columnIndex = ResultData.getColumnNo (
        Evado.Model.Digital.EvSchedule.ScheduleClassFieldNames.Title );
      if ( columnIndex > -1 )
      {
        Milestone.Title = ImportRow.Values [ columnIndex ];
        ResultRow.Values [ columnIndex ] = "True";
      }

      //
      // Import the milestone type.
      //
      columnIndex = ResultData.getColumnNo (
          Evado.Model.Digital.EvMilestone.MilestoneClassFieldNames.Type );
      if ( columnIndex > -1 )
      {
        this.LogDebug ( "Type update." );
        value = ImportRow.Values [ columnIndex ];
        Evado.Model.Digital.EvMilestone.MilestoneTypes typeValue = Evado.Model.Digital.EvMilestone.MilestoneTypes.Null;

        if ( Evado.Model.EvStatics.Enumerations.tryParseEnumValue<Evado.Model.Digital.EvMilestone.MilestoneTypes> ( value, out typeValue ) == true )
        {
          Milestone.Type = typeValue;
          ResultRow.Values [ columnIndex ] = "True";
        }
        else
        {
          ResultRow.Values [ columnIndex ] = "False";
        }
      }

      //
      // Import the milestone order.
      //
      columnIndex = ResultData.getColumnNo (
          Evado.Model.Digital.EvMilestone.MilestoneClassFieldNames.Order );
      if ( columnIndex > -1 )
      {
        this.LogDebug ( "order update." );
        value = ImportRow.Values [ columnIndex ];

        if ( int.TryParse ( value, out iValue ) == true )
        {
          Milestone.Order = iValue;
          ResultRow.Values [ columnIndex ] = "True";
        }
        else
        {
          ResultRow.Values [ columnIndex ] = "False";
        }
      }

      //
      // Import the milestone description.
      //
      columnIndex = ResultData.getColumnNo (
          Evado.Model.Digital.EvMilestone.MilestoneClassFieldNames.Description );
      if ( columnIndex > -1 )
      {
        this.LogDebug ( "Description update." );
        Milestone.Description = ImportRow.Values [ columnIndex ];
        ResultRow.Values [ columnIndex ] = "True";
      }

      //
      // Import the milestone Inter_Visit_Period.
      //
      columnIndex = ResultData.getColumnNo (
          Evado.Model.Digital.EvMilestone.MilestoneClassFieldNames.Inter_Visit_Period );
      if ( columnIndex > -1 )
      {
        this.LogDebug ( "Inter_Visit_Period update." );
        value = ImportRow.Values [ columnIndex ];

        if ( float.TryParse ( value, out fValue ) == true )
        {
          Milestone.InterMilestonePeriod = fValue;
          ResultRow.Values [ columnIndex ] = "True";
        }
        else
        {
          ResultRow.Values [ columnIndex ] = "False";
        }
      }

      //
      // Import the milestone Inter_Visit_Period.
      //
      columnIndex = ResultData.getColumnNo (
          Evado.Model.Digital.EvMilestone.MilestoneClassFieldNames.Milestone_Range );
      if ( columnIndex > -1 )
      {
        this.LogDebug ( "Milestone_Range update." );
        value = ImportRow.Values [ columnIndex ];

        if ( float.TryParse ( value, out fValue ) == true )
        {
          Milestone.MilestoneRange = fValue;
          ResultRow.Values [ columnIndex ] = "True";
        }
        else
        {
          ResultRow.Values [ columnIndex ] = "False";
        }
      }

      //
      // Import the milestone mandatory activity.
      //
      columnIndex = ResultData.getColumnNo (
          Evado.Model.Digital.EvMilestone.MilestoneClassFieldNames.Mandatory_Activity );
      if ( columnIndex > -1 )
      {
        this.LogDebug ( "mandatory activity update." );
        value = ImportRow.Values [ columnIndex ];

        Evado.Model.Integration.EiEventCodes result = this.importActivity (
          Milestone,
          value,
          Evado.Model.Digital.EvActivity.ActivitySelection.Mandatory );

        if ( result == EiEventCodes.Ok )
        {
          ResultRow.Values [ columnIndex ] = "True";
        }
        else
        {
          ResultRow.Values [ columnIndex ] = "false";
          return result;
        }
      }

      //
      // Import the milestone optional activity.
      //
      columnIndex = ResultData.getColumnNo (
          Evado.Model.Digital.EvMilestone.MilestoneClassFieldNames.Optional_Activity );
      if ( columnIndex > -1 )
      {
        this.LogDebug ( "optioan activity update." );
        value = ImportRow.Values [ columnIndex ];

        Evado.Model.Integration.EiEventCodes result = this.importActivity (
          Milestone,
          value,
          Evado.Model.Digital.EvActivity.ActivitySelection.Optional );

        if ( result == EiEventCodes.Ok )
        {
          ResultRow.Values [ columnIndex ] = "True";
        }
        else
        {
          ResultRow.Values [ columnIndex ] = "false";
          return result;
        }
      }

      //
      // Import the milestone optional activity.
      //
      columnIndex = ResultData.getColumnNo (
          Evado.Model.Digital.EvMilestone.MilestoneClassFieldNames.Non_Clinical_Activity );
      if ( columnIndex > -1 )
      {
        this.LogDebug ( "Non clinical activity update." );
        value = ImportRow.Values [ columnIndex ];

        Evado.Model.Integration.EiEventCodes result = this.importActivity (
          Milestone,
          value,
          Evado.Model.Digital.EvActivity.ActivitySelection.Non_Clinical );

        if ( result == EiEventCodes.Ok )
        {
          ResultRow.Values [ columnIndex ] = "True";
        }
        else
        {
          ResultRow.Values [ columnIndex ] = "false";
          return result;
        }
      }

      return EiEventCodes.Ok;

    }//END importMilestoneColumnData method

    //===================================================================================
    /// <summary>
    /// This method defines the default subject result columns.
    /// </summary>
    /// <param name="Milestone">Evado.Model.Digital.EvMilestone object</param>
    /// <param name="ActivityId">String Activit identifier</param>
    /// <param name="Type">Evado.Model.Digital.EvActivity.ActivitySelection enumerated list object</param>
    //-----------------------------------------------------------------------------------
    private Evado.Model.Integration.EiEventCodes importActivity (
      Evado.Model.Digital.EvMilestone Milestone,
      String ActivityId,
      Evado.Model.Digital.EvActivity.ActivitySelection Type )
    {
      this.LogMethod ( "importActivity method." );
      this.writeProcessLog ( "Integration Service - Updating importActivity object data." );
      //
      // initialise the methods variables and activities.
      //
      Evado.Model.Digital.EvActivity activity = new Evado.Model.Digital.EvActivity ( );
      String value = String.Empty;

      //
      // If this method is called is should have an activity identifier(s)
      //
      if ( ActivityId == String.Empty )
      {
        return EiEventCodes.Integration_Import_Activity_Id_Error;
      }

      //
      // Process the activities based on the activity seleciton type.
      //
      switch ( Type )
      {
        case Evado.Model.Digital.EvActivity.ActivitySelection.Mandatory:
          {
            //
            // Get the activity.
            //
            activity = this.getActivity ( ActivityId );

            if ( activity.ActivityId == String.Empty )
            {
              this.LogMethod ( "Activity Id is empty." );
              return EiEventCodes.Integration_Import_Activity_Id_Error;
            }

            if ( activity.Type == Evado.Model.Digital.EvActivity.ActivityTypes.Non_Clinical )
            {
              this.LogMethod ( "Activity is incorrect type." );
              return EiEventCodes.Integration_Import_Activity_Type_Error;
            }

            activity.IsMandatory = true;

            //
            // Add activity and exit.
            //
            Milestone.ActivityList.Add ( activity );

            break;
          }
        case Evado.Model.Digital.EvActivity.ActivitySelection.Optional:
          {
            //
            // Get the activity.
            //
            activity = this.getActivity ( ActivityId );

            if ( activity.ActivityId == String.Empty )
            {
              this.LogMethod ( "Activity Id is empty." );
              return EiEventCodes.Integration_Import_Activity_Id_Error;
            }

            if ( activity.Type == Evado.Model.Digital.EvActivity.ActivityTypes.Non_Clinical )
            {
              this.LogMethod ( "Activity is incorrect type." );
              return EiEventCodes.Integration_Import_Activity_Type_Error;
            }

            activity.IsMandatory = false;
            //
            // Add activity and exit.
            //
            Milestone.ActivityList.Add ( activity );

            break;
          }
        case Evado.Model.Digital.EvActivity.ActivitySelection.Non_Clinical:
          {
            string [ ] arrActivityId = ActivityId.Split ( ';' );

            //
            // Iterate through the array of activityIds.
            //
            for ( int i = 0; i < arrActivityId.Length; i++ )
            {
              //
              // get the activity.
              //
              activity = this.getActivity ( arrActivityId [ i ] );

              if ( activity.Type == Evado.Model.Digital.EvActivity.ActivityTypes.Clinical )
              {
                this.LogMethod ( "Activity is incorrect type." );
                return EiEventCodes.Integration_Import_Activity_Type_Error;
              }
              activity.IsMandatory = false;
            }

            break;
          }
      }//END selection type switch statemenet.

      return Evado.Model.Integration.EiEventCodes.Ok;

    }//END method

    //===================================================================================
    /// <summary>
    /// This method defines the default subject result columns.
    /// </summary>
    //-----------------------------------------------------------------------------------
    private Evado.Model.Integration.EiEventCodes getActivityList ( )
    {
      //
      // If not project id then exit method with error.
      //
      if ( this._ProjectId == String.Empty )
      {
        return EiEventCodes.Integration_Import_Project_Id_Error;
      }

      //
      // exit the method if the activity list exists.
      //
      if ( this._ActivityList.Count > 0 )
      {
        return EiEventCodes.Ok;
      }

      //
      // Query the database.
      //
      this._ActivityList = this._bllActivites.getActivityList ( this._ProjectId, Evado.Model.Digital.EvActivity.ActivityTypes.Null, false );

      this.LogDebugClass ( this._bllActivites.Log );

      //
      // exit the method if the activity list exists.
      //
      if ( this._ActivityList.Count == 0 )
      {
        return EiEventCodes.Integration_Import_Actvity_List_Error;
      }

      return EiEventCodes.Ok;

    }//END getActivityList method.

    //===================================================================================
    /// <summary>
    /// THis method returns a activity matching the passed activity identifier.
    /// </summary>
    /// <param name="ActivityId">String: Activity identifier</param>
    /// <returns>Evado.Model.Digital.EvActivity</returns>
    //-----------------------------------------------------------------------------------
    private Evado.Model.Digital.EvActivity getActivity (
      String ActivityId )
    {
      this.LogMethod ( "getActivity method." );
      //
      // Iterate through each of the activities in the list and return the matching activity.
      //
      foreach ( Evado.Model.Digital.EvActivity activity in this._ActivityList )
      {
        if ( activity.ActivityId.ToLower ( ) == ActivityId.ToLower ( ) )
        {
          return activity;
        }
      }

      //
      // Return an empty activity is none is found.
      //
      return new Evado.Model.Digital.EvActivity ( );
    }
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Process log methods

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Content">String:  debug text.</param>
    //  ----------------------------------------------------------------------------------
    protected void writeProcessLog ( String Content )
    {
      this._ProcessLog.AppendLine (
           DateTime.Now.ToString ( "dd-MM-yy HH:mm:ss" )
         + ": " + Content );
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

  }//END EiActivities Class.

}//END namespace Evado.BLL 
