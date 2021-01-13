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

using Evado.Model.Integration;

namespace Evado.Bll.Integration
{
  /// <summary>
  /// This class handles the Evado integration services.
  /// </summary>
  public partial class EiServices : EvBllBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EiServices ( )
    {
      this.ClassNameSpace = "Evado.Bll.Clinical.EiServices.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EiServices ( Evado.Model.Digital.EvClassParameters Settings )
    {
      this.ClassParameter = Settings;
      this.ClassNameSpace = "Evado.Bll.Clinical.EiServices.";

      if ( this.ClassParameter.LoggingLevel == 0 )
      {
        this.ClassParameter.LoggingLevel = Evado.Dal.EvStaticSetting.LoggingLevel;
      }
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
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

    //This property contains the methods from EiSubjects class to call the import and export methods

    //private static Evado.Bll.Integration.EiActivities EI_Activities = new EiActivities ( );

    //private static Evado.Bll.Integration.EiSubjects EI_Subjects = new EiSubjects ( );

    //private static Evado.Bll.Integration.EiPatients EI_Patients = new EiPatients ( );

    //private static Evado.Bll.Integration.EiVisitRecords EI_ProjectRecords = new EiVisitRecords ( );

    //private static Evado.Bll.Integration.EiCommonRecords EI_CommonRecords = new EiCommonRecords ( );

    //private static Evado.Bll.Integration.EiSchedules EI_Schedules = new EiSchedules ( );

    //private static Evado.Bll.Integration.EiBudgets EI_Budgets = new EiBudgets ( );


    private String _UserCommonName = String.Empty;
    /// <summary>
    /// 
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

    private String _ProjectId = String.Empty;
    /// <summary>
    /// This field contains the reserved identifiers for the EiServices object.
    /// </summary>
    public readonly string CONST_RESERVERD_IDENTIFIERS = "SubjectId;RecordId;VisitId";

    /// <summary>
    /// This field contains the subject reserved identifiers for the EiServices object.
    /// </summary>
    public const string EI_SUBJECTS = "EiSubjects:";

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Public Query method
    //===================================================================================
    /// <summary>
    /// This method executes a query 
    /// </summary>
    /// <param name="QueryData">Evado.Model.Integration.EiData wiht query parameters.</param>
    /// <returns>Evado.Model.Integration.EiData object</returns>
    //-----------------------------------------------------------------------------------
    public Evado.Model.Integration.EiData ProcessQuery (
      Evado.Model.Integration.EiData QueryData )
    {
      this.LogMethod ( "ProcessQuery method." );
      this.LogDebug ( "Settings.LoggingLevel: " + this.ClassParameter.LoggingLevel );
      this.LogDebug ( "QueryType: " + QueryData.QueryType );
      this.writeProcessLog ( "Integration Service - Commence processing query data." );
      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.Integration.EiData resultData = new Model.Integration.EiData ( );
      Evado.Model.Digital.EdQueryParameters queryParameters =
        new Evado.Model.Digital.EdQueryParameters ( );

      //
      // Validate that the query data has parameters.
      //
      if ( QueryData.ParameterList == null )
      {
        this.writeProcessLog ( "Integration Service - Project identifier not provided." );
        this.LogDebug ( "Parameter list is null." );

        resultData.EventCode = Model.Integration.EiEventCodes.Integration_Import_Parameter_Error;
        resultData.ErrorMessage = "Integration Service - Parameter list is empty.";
        return resultData;
      }
      if ( QueryData.ParameterList.Count == 0 )
      {
        this.writeProcessLog ( "Integration Service - Parameter list is empty." );
        this.LogDebug ( "Parameter list is empty." );

        resultData.EventCode = Model.Integration.EiEventCodes.Integration_Import_Parameter_Error;
        resultData.ErrorMessage = "Integration Service - Parameter list is empty.";
        return resultData;
      }

      string projectid = QueryData.GetQueryParameterValue ( Model.Integration.EiQueryParameterNames.Project_Id );

      if ( QueryData.QueryType != EiQueryTypes.Patients_Export 
        && QueryData.QueryType != EiQueryTypes.Patients_Import 
        && projectid == String.Empty )
      {
        this.LogDebug ( "ProjectId is empty." );
        this.writeProcessLog ( "Integration Service - Project identifier not provided." );

        resultData.EventCode = Model.Integration.EiEventCodes.Integration_Import_Parameter_Error;
        resultData.ErrorMessage = "Integration Service - Parameter list is empty.";

        return resultData;
      }

      //
      // select the query QueryType
      //
      switch ( QueryData.QueryType )
      {
        case Model.Integration.EiQueryTypes.Activities_Export: //Subjects
          {
            Evado.Bll.Integration.EiActivities EI_Activities = new EiActivities ( this.ClassParameter );
            this.LogDebug ( "Activity Data Export " );
            this.writeProcessLog ( "Activity Data Export " );
            resultData = EI_Activities.exportData ( QueryData );

            this.LogClass ( EI_Activities.Log );

            break;
          }

        case Model.Integration.EiQueryTypes.Activity_Template:
          {
            Evado.Bll.Integration.EiActivities EI_Activities = new EiActivities ( this.ClassParameter );
            this.LogDebug ( "Activity tempalate " );
            this.writeProcessLog ( "Activity tempalate " );
            resultData = EI_Activities.getTemplateData ( QueryData );

            this.LogClass ( EI_Activities.Log );

            break;
          }

        case Model.Integration.EiQueryTypes.Activities_Import:
          {
            Evado.Bll.Integration.EiActivities EI_Activities = new EiActivities ( this.ClassParameter );
            this.writeProcessLog ( "Activity Data import " );
            resultData = EI_Activities.ImportData ( QueryData );

            this.LogClass ( EI_Activities.Log );

            break;
          }

        case Model.Integration.EiQueryTypes.Schedule_Export: //Subjects
          {
            Evado.Bll.Integration.EiSchedules EI_Schedules = new EiSchedules ( this.ClassParameter );
            this.LogDebug ( "Schedule Data Export " );
            this.writeProcessLog ( "Schedule Data Export " );
            resultData = EI_Schedules.ExportData ( QueryData );

            this.LogClass ( EI_Schedules.Log );

            break;
          }

        case Model.Integration.EiQueryTypes.Schedule_Template:
          {
            Evado.Bll.Integration.EiSchedules EI_Schedules = new EiSchedules ( this.ClassParameter );
            this.LogDebug ( "Schedule tempalate " );
            this.writeProcessLog ( "Schedule tempalate " );
            resultData = EI_Schedules.getTemplateData ( QueryData );

            this.LogClass ( EI_Schedules.Log );

            break;
          }

        case Model.Integration.EiQueryTypes.Schedule_Import:
          {
            Evado.Bll.Integration.EiSchedules EI_Schedules = new EiSchedules ( this.ClassParameter );
            this.LogDebug ( "Schedule Data import " );
            this.writeProcessLog ( "Schedule Data import " );
            resultData = EI_Schedules.ImportData ( QueryData );

            this.LogClass ( EI_Schedules.Log );

            break;
          }

        case Model.Integration.EiQueryTypes.Adverse_Events_Export:
        case Model.Integration.EiQueryTypes.Serious_Adverse_Events_Export:
        case Model.Integration.EiQueryTypes.Comcomitant_Medications_Export:
        case Model.Integration.EiQueryTypes.Patient_Consent_Export:
          {
            this.LogDebug ( "Export Common Records" );
            this.writeProcessLog ( "Export Common Records" );
            break;
          }

        case Model.Integration.EiQueryTypes.Common_Records_Import:
          {
            this.LogDebug ( "Import Common Records" );
            this.writeProcessLog ( "Import Common Records" );
            break;
          }
        default:
          {
            resultData.EventCode = EiEventCodes.Integration_Import_Type_Id_Error;
            resultData.ErrorMessage = Evado.Model.EvStatics.Enumerations.enumValueToString( EiEventCodes.Integration_Import_Type_Id_Error );
            break;
          }

      }

      this.LogMethodEnd ( "ProcessQuery" );
      return resultData;

    }//END Query method.
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region general form field methods.

    //===================================================================================
    /// <summary>
    /// This method appends the column objects for a list of form fields.
    /// </summary>
    /// <param name="FieldList">List of Evado.Model.Digital.EvFormField objects</param>
    /// <param name="ResultData">Evado.Model.Integration.EiData object</param>
    //-----------------------------------------------------------------------------------
    public void getFormFieldColumns (
      List<Evado.Model.Digital.EdRecordField> FieldList,
      Evado.Model.Integration.EiData ResultData )
    {
      this.LogMethod ( "getFormFieldColumns method." );
      this.LogDebug ( "FieldList.Count: " + FieldList.Count );

      //
      // Iterate through the field list creating column object for each field object.
      //
      foreach (Evado.Model.Digital.EdRecordField field in FieldList)
      {
        this.LogDebug ( "Field: " + field.FieldId + ", Type: " + field.TypeId );
        //
        // add the correct column type for the field data type.
        //
        switch (field.TypeId)
        {
          case Model.EvDataTypes.Boolean:
          {
            this.LogDebug ( "ADD: Boolean Column" );
            //
            //The default data type is text.
            //
            ResultData.AddColumn(
                 Model.Integration.EiDataTypes.Boolean,
                 field.FieldId );

            break;
          }
          case Model.EvDataTypes.Numeric:
          {
            this.LogDebug ( "ADD: Floating_Point Column" );
            //
            //The default data type is text.
            //
            ResultData.AddColumn( 
              Model.Integration.EiDataTypes.Floating_Point,
                 field.FieldId );

            break;
          }
          case Model.EvDataTypes.Date:
          {
            this.LogDebug ( "ADD: DateTime Column" );
            //
            //The default data type is text.
            //
            ResultData.AddColumn(
                 Model.Integration.EiDataTypes.Date,
                 field.FieldId );

            break;
          }
          case Model.EvDataTypes.Check_Box_List:
          {
            this.LogDebug ( "ADD: Check_Button_List columns" );

            this.getCheckBoxFieldColumn ( field, ResultData );

            break;
          }
          case Model.EvDataTypes.Table:
          case Model.EvDataTypes.Special_Matrix:
          {
            this.LogDebug ( "ADD: Table columns" );

            this.getTableFieldColumn ( field, ResultData );

            break;
          }
          default:
          {
            this.LogDebug ( "ADD: Text Column" );
            //
            //The default data type is text.
            //
            ResultData.Columns.Add ( new Model.Integration.EiColumnParameters (
                 Model.Integration.EiDataTypes.Text,
                 field.FieldId ) );
            break;
          }

        }//END data type switch

      }//END field iteration loop

    }//END getFormFieldColumns

    //===================================================================================
    /// <summary>
    /// This method appends the form field data values to the output row .
    /// </summary>
    /// <param name="FieldList">List of Evado.Model.Digital.EvFormField objects</param>
    /// <param name="ResultData">Evado.Model.Integration.EiData object</param>
    /// <param name="Row">Evado.Model.Integration.EiDataRow object</param>
    //-----------------------------------------------------------------------------------
   // private void getFormFieldColumnData (
    public void getFormFieldColumnData (
      List<Evado.Model.Digital.EdRecordField> FieldList,
      Evado.Model.Integration.EiData ResultData,
      Evado.Model.Integration.EiDataRow Row )
    {
      this.LogMethod ( "getFormFieldColumnData method." );
      //
      // Initialise the method variables and objects.
      //
      int column = 0;

      //
      // Iterate through the field list creating column object for each field object.
      //
      foreach (Evado.Model.Digital.EdRecordField field in FieldList)
      {
        //
        // add the correct column type for the field data type.
        //
        switch (field.TypeId)
        {
          case Model.EvDataTypes.Boolean:
          {
            String value = "False";

            if (field.ItemValue == "yes"
              || field.ItemValue == "yes")
            {
              value = "True";
            }

            column = ResultData.getColumnNo ( field.FieldId );

            this.writeProcessLog ( "Column: " + column + ",\t FieldId: " + field.FieldId + ", Type: " + field.TypeId + ", Value: " + field.ItemValue );

            if (column > -1)
            {
              Row.updateValue ( column, value.ToString ( ) );
            }
            break;
          }
          case Model.EvDataTypes.Free_Text:
          {
            column = ResultData.getColumnNo ( field.FieldId );

            this.writeProcessLog ( "Column: " + column + ",\t FieldId: " + field.FieldId + ", Type: " + field.TypeId + ", Value: " + field.ItemValue );

            String text = field.ItemText.Replace ( "\r", "^" );
            text = text.Replace ( "\n", "^" );
            text = text.Replace ( "^^", "^" );

            Row.updateValue ( column, text );

            break;
          }
          case Model.EvDataTypes.Check_Box_List:
          {
            this.getCheckBoxFieldColumnValues ( field, ResultData, Row );
            break;
          }
          case Model.EvDataTypes.Table:
          case Model.EvDataTypes.Special_Matrix:
          {
            this.getTableFieldColumnValues ( field, ResultData, Row );
            break;
          }
          default:
          {
            column = ResultData.getColumnNo ( field.FieldId );

            this.writeProcessLog ( "Column: " + column + ",\t FieldId: " + field.FieldId + ", Type: " + field.TypeId + ", Value: " + field.ItemValue );

            if (column > -1)
            {
              Row.updateValue ( column, field.ItemValue );
            }
            break;
          }

        }//END data type switch

      }//END field iteration loop

    }//END getFormFieldColumns

    //===================================================================================
    /// <summary>
    /// This method appends the column objects for a list of form fields.
    /// </summary>
    /// <param name="Field">Evado.Model.Digital.EvFormField object</param>
    /// <param name="ResultData">Evado.Model.Integration.EiData object</param>
    //-----------------------------------------------------------------------------------
    //private void getCheckBoxFieldColumn (
    public void getCheckBoxFieldColumn (
      Evado.Model.Digital.EdRecordField Field,
      Evado.Model.Integration.EiData ResultData )
    {
      this.LogMethod ( "getCheckBoxFieldColumn method." );
      //
      // Iterate through the list of checkbox options
      //
      for (int count = 0; count < Field.Design.OptionList.Count; count++)
      {
        //
        // get the option
        //
        Evado.Model.EvOption option = Field.Design.OptionList [ count ];

        //
        //The default data type is text.
        //
        ResultData.AddColumn(
             Model.Integration.EiDataTypes.Boolean,
             Field.FieldId + "_" + count + "_" + option.Value );

      }//END option iteration loop.

    }//END getFormFieldColumns method

    //===================================================================================
    /// <summary>
    /// This method appends the column objects for a list of form fields.
    /// </summary>
    /// <param name="Field">Evado.Model.Digital.EvFormField object</param>
    /// <param name="ResultData">Evado.Model.Integration.EiData object</param>
    /// <param name="Row">Evado.Model.Integration.EiDataRow object</param>
    //-----------------------------------------------------------------------------------
    private void getCheckBoxFieldColumnValues (
      Evado.Model.Digital.EdRecordField Field,
      Evado.Model.Integration.EiData ResultData,
      Evado.Model.Integration.EiDataRow Row )
    {
      this.LogMethod ( "getCheckBoxFieldColumnValues method." );
      //
      // Iterate through the list of checkbox options
      //
      for (int count = 0; count < Field.Design.OptionList.Count; count++)
      {
        //
        // get the option
        //
        Evado.Model.EvOption option = Field.Design.OptionList [ count ];

        string fieldId = Field.FieldId + "_" + count + "_" + option.Value;

        //
        // Set the checkbox listing output.
        //
        int column = ResultData.getColumnNo ( fieldId );

        this.writeProcessLog ( "Column: " + column + ",\t FieldId: " + fieldId + ", Type: " + Field.TypeId + ", Value: " + Field.ItemValue );

        if (column > -1)
        {
          if (Field.ItemValue.Contains ( option.Value ) == true)
          {
            Row.updateValue ( column, "True" );
          }
          else
          {
            Row.updateValue ( column, "False" );
          }
        }

      }//END option iteration loop.

    }//END getFormFieldColumns method

    //===================================================================================
    /// <summary>
    /// This method appends the column objects for a list of form fields.
    /// </summary>
    /// <param name="Field">Evado.Model.Digital.EvFormField object</param>
    /// <param name="ResultData">Evado.Model.Integration.EiData object</param>
    //-----------------------------------------------------------------------------------
    //private void getTableFieldColumn (
    public void getTableFieldColumn (
      Evado.Model.Digital.EdRecordField Field,
      Evado.Model.Integration.EiData ResultData )
    {
      this.LogMethod ( "getTableFieldColumn method." );
      //
      // Iterate through the table columns
      //
      for (int tableRow = 0; tableRow < Field.Table.Header.Length; tableRow++)
      {
        int row = tableRow + 1;
        //
        // Iterate through the table columns
        //
        for (int tableColumn = 0; tableColumn < Field.Table.Header.Length; tableColumn++)
        {
          int column = tableColumn + 1;
          //
          // get the column text header.
          //
          String title = Field.Table.Header [ tableColumn ].Text;

          //
          // Skip empty header text.
          //
          if (title == String.Empty)
          {
            continue;
          }

          //
          // Defne the parameter.
          //
          Model.Integration.EiColumnParameters parameter = new Model.Integration.EiColumnParameters (
               Model.Integration.EiDataTypes.Text,
               Field.FieldId + "_" + row + "_" + column + "_" + title );

          switch (Field.Table.Header [ tableColumn ].TypeId)
          {
            case  Model.EvDataTypes.Numeric:
            {
              parameter.DataType = Model.Integration.EiDataTypes.Floating_Point;
              break;
            }
            case  Model.EvDataTypes.Date:
            {
              parameter.DataType = Model.Integration.EiDataTypes.Date;
              break;
            }
            case  Model.EvDataTypes.Yes_No:
            case  Model.EvDataTypes.Boolean:
            {
              parameter.DataType = Model.Integration.EiDataTypes.Boolean;
              break;
            }
          }//END data type switch

          //
          //The default data type is text.
          //
          ResultData.Columns.Add ( parameter );

        }//END table column header iteration loop

      }//END table row  iteration loop

    }//END getTableFieldColumn method.

    //===================================================================================
    /// <summary>
    /// This method appends the column objects for a list of form fields.
    /// </summary>
    /// <param name="Field">Evado.Model.Digital.EvFormField object</param>
    /// <param name="ResultData">Evado.Model.Integration.EiData object</param>
    /// <param name="Row">Evado.Model.Integration.EiDataRow object</param>
    //-----------------------------------------------------------------------------------
    //private void getTableFieldColumnValues (
    public void getTableFieldColumnValues (
      Evado.Model.Digital.EdRecordField Field,
      Evado.Model.Integration.EiData ResultData,
      Evado.Model.Integration.EiDataRow Row )
    {
      this.LogMethod ( "getTableFieldColumnValues method." );
      //
      // Iterate through the table columns
      //
      for (int tableRow = 0; tableRow < Field.Table.Rows.Count; tableRow++)
      {
        int row = tableRow + 1;
        //
        // Iterate through the table columns
        //
        for (int tableColumn = 0; tableColumn < Field.Table.Header.Length; tableColumn++)
        {
          int column = tableColumn + 1;

          //
          // Skip empty header text.
          //
          if (Field.Table.Header [ tableColumn ].Text == String.Empty)
          {
            continue;
          }

          //
          // Define the output field identifier.
          //
          string fieldId = Field.FieldId + "_" + row + "_" + column + "_" + Field.Table.Header [ tableColumn ].Text;

          //
          // Set the checkbox listing output.
          //
          int headerColumn = ResultData.getColumnNo ( fieldId );
          //

          // Header column found
          //
          if (headerColumn > -1)
          {
            string value = Field.Table.Rows [ tableRow ].Column [ tableColumn ];

            this.LogDebug ( "Column: " + headerColumn + ",\t FieldId: " + fieldId + ", Type: " + Field.TypeId + ", Value: " + value );

            //
            // data type switch to handle boolean values.
            //
            switch (Field.Table.Header [ tableColumn ].TypeId)
            {
              case Model.EvDataTypes.Yes_No:
              case Model.EvDataTypes.Boolean:
              {
                if (value == "yes" || value == "1" || value == "true")
                {
                  Row.updateValue ( headerColumn, "True" );
                }
                else
                {
                  Row.updateValue ( headerColumn, "False" );
                }
                break;
              }
              default:
              {
                Row.updateValue ( headerColumn, value );
                break;
              }
            }//END data type switch

          }//END header column index found

        }//END table column header iteration loop

      }//END table row  iteration loop

    }//END getTableFieldColumn method.

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region debug methods

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

  }//END EiService Class.

}//END namespace Evado.BLL 
