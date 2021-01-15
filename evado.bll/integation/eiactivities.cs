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
 *  This class contains the EvDatabaseUpdates business object.
 *
 ****************************************************************************************/

using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;

using Evado.Model;
using Evado.Model.Digital;
using Evado.Model.Integration;
using Evado.Bll;

namespace Evado.Bll.Integration
{
  /// <summary>
  /// This class handles the Evado integration integration services.
  /// </summary>
  public class EiActivities: EvBllBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EiActivities ( )
    {
      this.ClassNameSpace = "Evado.Bll.Clinical.EiActivities.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EiActivities ( EvClassParameters Settings )
    {
      this.ClassParameter = Settings;
      this.ClassNameSpace = "Evado.Bll.Clinical.EiActivities.";

      if ( this.ClassParameter.LoggingLevel == 0 )
      {
        this.ClassParameter.LoggingLevel = Evado.Dal.EvStaticSetting.LoggingLevel;
      }

      this._bllActivites = new Evado.Bll.Clinical.EvActivities ( Settings );
    }
    #endregion

    #region class properties

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
    /// This propoerty contains the user common name.
    /// </summary>
    public String UserCommonName
    {
      get { return _UserCommonName; }
      set { _UserCommonName = value; }
    }

    private String _UserId = String.Empty;

    /// <summary>
    /// This property contains the user identifier
    /// </summary>
    public String UserId
    {
      get { return _UserId; }
      set { _UserId = value; }
    }

    private int _ImportIdentifierColumn = -1;
    private int _ImportProjectColumn = -1;
    private String _ProjectId = String.Empty;

    private Evado.Bll.Clinical.EvActivities _bllActivites = new Evado.Bll.Clinical.EvActivities ( );
    List<Evado.Model.Digital.EvActivity> _ActivityList = new List< Evado.Model.Digital.EvActivity> ( );


    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Activity Template methods
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
      this.writeProcessLog ( "Integration Service - Processing activity template." );
      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.Integration.EiData resultData = QueryData;
      Evado.Model.Digital.EvActivity activity = new Evado.Model.Digital.EvActivity ( );
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
      this.setDefaultActivityColumns ( resultData );

      //
      // Add the subject's data to the output data array.
      //
      this.setColumnData ( activity, resultData );

      return resultData;

    }//END SubjectQuery method

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Activity Export methods

    //===================================================================================
    /// <summary>
    /// This method executes a subject query
    /// </summary>
    /// <param name="QueryData">Evado.Model.Integration.EiData</param>
    /// <returns>Evado.Model.Integration.EiData object</returns>
    //-----------------------------------------------------------------------------------
    public Evado.Model.Integration.EiData exportData (
      Evado.Model.Integration.EiData QueryData )
    {
      this.LogMethod ( "exportData method." );
      this.writeProcessLog ( "Integration Service - Processing activity data." );
      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.Integration.EiData resultData = QueryData;
      int columnCount = 0;
      String projectId = String.Empty;

      //
      // reset the data rows.
      //
      resultData.DataRows = new List<Model.Integration.EiDataRow> ( );

      //
      // Query the database.
      //
      Evado.Model.Integration.EiEventCodes result = this.getActivityList ( QueryData );

      //
      // return error 
      if ( result != EiEventCodes.Ok )
      {
        resultData.ProcessLog = this._ProcessLog.ToString ( );
        resultData.EventCode = result;
        return resultData;
      }

      //
      // if no activities return empty object.
      //
      if ( _ActivityList.Count == 0 )
      {
        this.LogDebug ( "Activity list is empty." );
        resultData.ProcessLog = this._ProcessLog.ToString ( );
        resultData.EventCode = EiEventCodes.Integration_Import_General_Error;
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

        this.setDefaultActivityColumns ( resultData );
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
      foreach ( Evado.Model.Digital.EvActivity activity in _ActivityList )
      {
        this.LogDebug ( "Processing activity: " + activity.ActivityId );

        //
        // Add the subject's data to the output data array.
        //
        this.setColumnData ( activity, resultData );

      }//END subject iteration loop.


      this.LogDebug ( "data.DataRows.Count: " + resultData.DataRows.Count );

      resultData.ProcessLog = this._ProcessLog.ToString ( );

      return resultData;

    }//END exportData method

    //===================================================================================
    /// <summary>
    /// This method defines the default subject result columns.
    /// </summary>
    /// <param name="QueryData">Evado.Model.Integration.EiData object</param>
    //-----------------------------------------------------------------------------------
    private Evado.Model.Integration.EiEventCodes getActivityList (
      Evado.Model.Integration.EiData QueryData )
    {
      //
      // Initialise the methods variables and objects.
      //
      String projectId = String.Empty;

      if ( QueryData.QueryType == EiQueryTypes.Activities_Import )
      {
        if ( QueryData.DataRows.Count == 0 )
        {
          return EiEventCodes.Integration_Import_General_Error;
        }

        //
        // Add column ResultData based on where the column is in the column header
        //
        int column = QueryData.getColumnNo (
          Evado.Model.Digital.EvActivity.ActivityClassFieldNames.ProjectId );

        if ( column == -1 )
        {
          return EiEventCodes.Integration_Import_Project_Id_Error;
        }

        projectId = QueryData.DataRows [ 0 ].Values [ column ];
      }
      else
      {
        projectId = QueryData.GetQueryParameterValue ( EiQueryParameterNames.Project_Id );

      }
      //
      // If not project id then exit method with error.
      //
      if ( projectId == String.Empty )
      {
        return EiEventCodes.Integration_Import_Project_Id_Error;
      }

      //
      // Query the database.
      //
      this._ActivityList = this._bllActivites.getActivityList ( projectId, Evado.Model.Digital.EvActivity.ActivityTypes.Null, false );

      this.LogDebugClass( this._bllActivites.Log );

      return EiEventCodes.Ok;

    }//END getActivityList method.

    //===================================================================================
    /// <summary>
    /// This method defines the default subject result columns.
    /// </summary>
    /// <param name="ResultData">Evado.Model.Integration.EiData object</param>
    //-----------------------------------------------------------------------------------
    private void setDefaultActivityColumns (
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
        Evado.Model.Digital.EvActivity.ActivityClassFieldNames.ProjectId,
        false );

      //
      // Add column for activity identifier
      //
      ResultData.AddColumn (
        EiDataTypes.Text,
        Evado.Model.Digital.EvActivity.ActivityClassFieldNames.ActivityId,
        true );

      //
      // Add column for sactivity title
      //      
      ResultData.AddColumn (
        EiDataTypes.Text,
        Evado.Model.Digital.EvActivity.ActivityClassFieldNames.Title,
        false );

      //
      // Add column for activity description
      //
      ResultData.AddColumn (
        EiDataTypes.Text,
        Evado.Model.Digital.EvActivity.ActivityClassFieldNames.Description,
        false );

      //
      // Add column for form list
      //
      /*      
      column = ResultData.AddColumn (
        EiDataTypes.Text,
        Evado.Model.Digital.EvActivity.ActivityClassFieldNames.FormList,
        false );
      column.MetaData = "FormId list encoded with ';' between keywords.";
      */
    }//END setDefaultSubjectColumns method.

    //===================================================================================
    /// <summary>
    /// This method defines the default subject result columns.
    /// </summary>
    /// <param name="Activity">Evado.Model.Digital.EvActivity object</param>
    /// <param name="ResultData">Evado.Model.Integration.EiData object</param>
    //-----------------------------------------------------------------------------------
    private void setColumnData (
      Evado.Model.Digital.EvActivity Activity,
      Evado.Model.Integration.EiData ResultData )
    {
      this.LogMethod ( "setColumnData method." );
      //
      // Initialise the ResultData row object
      //
      Evado.Model.Integration.EiDataRow row = ResultData.AddDataRow ( );

      //
      // Add column ResultData based on where the column is in the column header
      //
      int column = ResultData.getColumnNo (
        Evado.Model.Digital.EvActivity.ActivityClassFieldNames.ProjectId );
      if ( column > -1 )
      {
        row.updateValue ( column, Activity.ProjectId );
      }

      column = ResultData.getColumnNo (
        Evado.Model.Digital.EvActivity.ActivityClassFieldNames.ActivityId );
      if ( column > -1 )
      {
        row.updateValue ( column, Activity.ActivityId );
      }

      column = ResultData.getColumnNo (
        Evado.Model.Digital.EvActivity.ActivityClassFieldNames.Title );
      if ( column > -1 )
      {
        row.updateValue ( column, Activity.Title );
      }

      column = ResultData.getColumnNo (
        Evado.Model.Digital.EvActivity.ActivityClassFieldNames.Description );

      if ( column > -1 )
      {
        row.updateValue ( column, Activity.Description );
      }

      column = ResultData.getColumnNo (
        Evado.Model.Digital.EvActivity.ActivityClassFieldNames.Type );

      if ( column > -1 )
      {
        row.updateValue ( column, Activity.Type );
      }

      column = ResultData.getColumnNo ( 
        Evado.Model.Digital.EvActivity.ActivityClassFieldNames.ValidationRule );
      if ( column > -1 )
      {
        row.updateValue ( column, Activity.ValidationRule.ToString() );
      }

    }//END setSubjectColumnData method

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Activity Import methods

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
      this.writeProcessLog ( "Integration Service - Subject import process commenced." );
      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.Integration.EiData returnData = new Model.Integration.EiData ( );
      returnData.ParameterList = ImportData.ParameterList;
      returnData.Columns = ImportData.Columns;

      this.LogDebug ( "EvStaticSetting.DebugOn: " + EvStaticSetting.DebugOn );

      this.LogDebug ( "returnData.Columns.Count: + " + returnData.Columns.Count );

      //
      // Query the database.
      //
      Evado.Model.Integration.EiEventCodes result = this.getActivityList ( ImportData );

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

      this.LogDebug ( "DataRows.Count: " + ImportData.DataRows.Count );

      //
      // if the import file does not have data rows return an error and abort import.
      //
      if ( ImportData.DataRows.Count == 0 )
      {
        returnData.EventCode = Model.Integration.EiEventCodes.Integration_Import_Column_Data_Error;

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

        //
        // Get the data row
        //
        Evado.Model.Integration.EiDataRow dataRow = ImportData.DataRows [ rowCount ];
        Evado.Model.Integration.EiDataRow resultRow = new Model.Integration.EiDataRow ( dataRow.Columns );

        this.LogDebug ( "Identifier: " + this._ImportIdentifierColumn + " Identifier value: " + dataRow.Values [ this._ImportIdentifierColumn ] );

        this.LogDebug ( "Result value size: + " + resultRow.Values.Length );

        //
        // execute the subject update method.
        //
        if ( this.updateActivityData ( ImportData, returnData, rowCount ) != Model.Integration.EiEventCodes.Ok )
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
    /// This method compares a data identifier with identifer enueration.
    /// </summary>
    /// <param name="ImportData">EiData object</param>
    /// <returns>Bool  true = found</returns>
    //-----------------------------------------------------------------------------------
    private bool setImportIdentifier (
      Evado.Model.Integration.EiData ImportData )
    {
      //
      // Set the column containing the project identifier
      //
      this._ImportProjectColumn = ImportData.getColumnNo ( Evado.Model.Digital.EvActivity.ActivityClassFieldNames.ProjectId );

      //
      // Set the column containing the activity id which is the external identifier.
      //
      this._ImportIdentifierColumn = ImportData.getColumnNo ( Evado.Model.Digital.EvActivity.ActivityClassFieldNames.ActivityId );

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
    private Evado.Model.Integration.EiEventCodes updateActivityData (
      Evado.Model.Integration.EiData ImportData,
      Evado.Model.Integration.EiData ExportData,
      int RowIndex )
    {
      this.LogMethod ( "updateSubjectData method." );

      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.Digital.EvActivity activity = new Evado.Model.Digital.EvActivity ( );
      String value = String.Empty;
      int columnIndex = -1;
      this.LogDebug ( "EvStaticSetting.DebugOn: " + EvStaticSetting.DebugOn );

      //
      // Set the import and result row references.
      //
      Evado.Model.Integration.EiDataRow importRow = ImportData.DataRows [ RowIndex ];
      Evado.Model.Integration.EiDataRow resultRow = ExportData.AddDataRow ( );



      String activityIdentifier = importRow.Values [ this._ImportIdentifierColumn ];
      this.LogDebug ( "External identifier: " + activityIdentifier );

      //
      // Retrieve the subject using it activity identifier.
      //
      activity = this.getActivity ( activityIdentifier );

      //
      // If the subject guid is empty no subject was found so create a new subject.
      //
      if ( activity.Guid == Guid.Empty )
      {
        this.LogDebug ( "Creating a new Activity" );

        columnIndex = ImportData.getColumnNo( Evado.Model.Digital.EvActivity.ActivityClassFieldNames.ProjectId );
        value = ImportData.DataRows [ RowIndex ].Values [ columnIndex ];

        activity.ProjectId = value;

        columnIndex = ImportData.getColumnNo ( Evado.Model.Digital.EvActivity.ActivityClassFieldNames.ActivityId );
        value = ImportData.DataRows [ RowIndex ].Values [ columnIndex ];

        activity.ActivityId = value;

        this.writeProcessLog ( "Integration Service - Activity " + activity.ActivityId + " created." );
        this.LogDebug ( "new ActivityId: " + activity.ActivityId );

      }//END create new subject.
      else
      {
        this.writeProcessLog ( "Integration Service - Subject " + activity.ActivityId + " updated." );
      }

      //
      // import the subject's data.
      //
      bool bResult = this.importColumnData (
         activity,
         ImportData,
         importRow,
         resultRow );

      //
      // Set the save parameters
      //
      activity.UserCommonName = this._UserCommonName;
      activity.UpdatedByUserId = this._UserId;
      activity.Action = Evado.Model.Digital.EvActivity.ActivitiesActionsCodes.Save;

      //
      // Save the updated subject object to the database.
      //
      Evado.Model.EvEventCodes result = this._bllActivites.saveActivity (
        activity );


      if ( result != Evado.Model.EvEventCodes.Ok )
      {
        this.LogDebugClass ( this._bllActivites.Log );

        this.writeProcessLog ( "Integration Service - Error occured saving subject " + activity.SubjectId + "." );
        this.LogDebug ( "Error occured saving subject " + activity.SubjectId + "."
          + " Eventcode: " + result );

        return Model.EvStatics.getEvadoEventCode<Model.Integration.EiEventCodes> ( result );
      }

      this.LogDebug ( "EXIT: Subject has been processed." );

      return Model.Integration.EiEventCodes.Ok;

    }

    //===================================================================================
    /// <summary>
    /// THis method returns a activity matching the passed activity identifier.
    /// </summary>
    /// <param name="ActivityId">String: Activity identifier</param>
    /// <returns>Evado.Model.Digital.EvActivity</returns>
    //-----------------------------------------------------------------------------------
    private Evado.Model.Digital.EvActivity getActivity ( String ActivityId )
    {
      this.LogMethod ( "getActivity method." );
      //
      // Iterate through each of the activities in the list and return the matching activity.
      //
      foreach ( Evado.Model.Digital.EvActivity activity in this._ActivityList )
      {
        if ( activity.ActivityId.ToLower() == ActivityId.ToLower() )
        {
          return activity;
        }
      }

      //
      // Return an empty activity is none is found.
      //
      return new Evado.Model.Digital.EvActivity ( );
    }

    //===================================================================================
    /// <summary>
    /// This method defines the default subject result columns.
    /// </summary>
    /// <param name="Activity">Evado.Model.Digital.EvActivity object</param>
    /// <param name="ImportData">Evado.Model.Integration.EiData object of import data</param>
    /// <param name="ImportRow">Evado.Model.Integration.EiDataRow object</param>
    /// <param name="ResultRow">Evado.Model.Integration.EiDataRow object containing validation results</param>
    //-----------------------------------------------------------------------------------
    private bool importColumnData (
      Evado.Model.Digital.EvActivity Activity,
      Evado.Model.Integration.EiData ImportData,
      Evado.Model.Integration.EiDataRow ImportRow,
      Evado.Model.Integration.EiDataRow ResultRow )
    {
      this.LogMethod ( "importSubjectColumnData method." );
      this.writeProcessLog ( "Integration Service - Updating subject field data." );

      //
      // Add column ResultData based on where the column is in the column header
      //
      int columnIndex = -1;
      String value = String.Empty;

      //
      // Import the activity title
      //
      this.LogDebug ( "Title update." );
      columnIndex = ImportData.getColumnNo (
        Evado.Model.Digital.EvActivity.ActivityClassFieldNames.Title );
      if ( columnIndex > -1 )
      {
        Activity.Title = ImportRow.Values [ columnIndex ];
        ResultRow.Values [ columnIndex ] = "True";
      }

      //
      // Update the activity type
      //
      columnIndex = ImportData.getColumnNo (
          Evado.Model.Digital.EvActivity.ActivityClassFieldNames.Type );
      if ( columnIndex > -1 )
      {
        this.LogDebug ( "Type update." );
        value = ImportRow.Values [ columnIndex ];
        Evado.Model.Digital.EvActivity.ActivityTypes typeValue = Evado.Model.Digital.EvActivity.ActivityTypes.Null;

        if ( Evado.Model.EvStatics.Enumerations.tryParseEnumValue<Evado.Model.Digital.EvActivity.ActivityTypes> ( value, out typeValue ) == true )
        {
          Activity.Type = typeValue;
          ResultRow.Values [ columnIndex ] = "True";
        }
        else
        {
          ResultRow.Values [ columnIndex ] = "False";
        }
      }

      //
      // Update the description
      //
      columnIndex = ImportData.getColumnNo (
          Evado.Model.Digital.EvActivity.ActivityClassFieldNames.Description );
      if ( columnIndex > -1 )
      {
        this.LogDebug ( "Description update." );
        Activity.Description = ImportRow.Values [ columnIndex ];
      }

      //
      // update the validation rule.
      //
      columnIndex = ImportData.getColumnNo (
          Evado.Model.Digital.EvActivity.ActivityClassFieldNames.ValidationRule);
      if ( columnIndex > -1 )
      {
        this.LogDebug ( "Type update." );
        value = ImportRow.Values [ columnIndex ];
        Evado.Model.Digital.EvActivity.ActivityValidation validationRule = Evado.Model.Digital.EvActivity.ActivityValidation.Null;

        if ( Evado.Model.EvStatics.Enumerations.tryParseEnumValue<Evado.Model.Digital.EvActivity.ActivityValidation> ( value, out validationRule ) == true )
        {
          Activity.ValidationRule = validationRule;
          ResultRow.Values [ columnIndex ] = "True";
        }
        else
        {
          ResultRow.Values [ columnIndex ] = "False";
        }
      }
      return true;

    }//END importColumnData method

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
