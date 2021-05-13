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

using Evado.Integration.Model;

namespace Evado.Digital.Bll
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
      this.ClassNameSpace = "Evado.Digital.Bll.Clinical.EiServices.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EiServices ( Evado.Digital.Model.EvClassParameters Settings )
    {
      this.ClassParameter = Settings;
      this.ClassNameSpace = "Evado.Digital.Bll.Clinical.EiServices.";

      if ( this.ClassParameter.LoggingLevel == 0 )
      {
        this.ClassParameter.LoggingLevel = Evado.Digital.Dal.EvStaticSetting.LoggingLevel;
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

    //private static Evado.Digital.Bll.Integration.EiActivities EI_Activities = new EiActivities ( );

    //private static Evado.Digital.Bll.Integration.EiSubjects EI_Subjects = new EiSubjects ( );

    //private static Evado.Digital.Bll.Integration.EiPatients EI_Patients = new EiPatients ( );

    //private static Evado.Digital.Bll.Integration.EiVisitRecords EI_ProjectRecords = new EiVisitRecords ( );

    //private static Evado.Digital.Bll.Integration.EiCommonRecords EI_CommonRecords = new EiCommonRecords ( );

    //private static Evado.Digital.Bll.Integration.EiSchedules EI_Schedules = new EiSchedules ( );

    //private static Evado.Digital.Bll.Integration.EiBudgets EI_Budgets = new EiBudgets ( );


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
    /// <param name="QueryData"> Evado.Integration.Model.EiData wiht query parameters.</param>
    /// <returns> Evado.Integration.Model.EiData object</returns>
    //-----------------------------------------------------------------------------------
    public  Evado.Integration.Model.EiData ProcessQuery (
       Evado.Integration.Model.EiData QueryData )
    {
      this.LogMethod ( "ProcessQuery method." );
      this.LogDebug ( "Settings.LoggingLevel: " + this.ClassParameter.LoggingLevel );
      this.LogDebug ( "QueryType: " + QueryData.QueryType );
      this.writeProcessLog ( "Integration Service - Commence processing query data." );
      //
      // Initialise the methods variables and objects.
      //
       Evado.Integration.Model.EiData resultData = new  Evado.Integration.Model.EiData ( );
      Evado.Digital.Model.EdQueryParameters queryParameters =
        new Evado.Digital.Model.EdQueryParameters ( );

      //
      // Validate that the query data has parameters.
      //
      if ( QueryData.ParameterList == null )
      {
        this.writeProcessLog ( "Integration Service - Project identifier not provided." );
        this.LogDebug ( "Parameter list is null." );

        resultData.EventCode =  Evado.Integration.Model.EiEventCodes.Integration_Import_Parameter_Error;
        resultData.ErrorMessage = "Integration Service - Parameter list is empty.";
        return resultData;
      }
      if ( QueryData.ParameterList.Count == 0 )
      {
        this.writeProcessLog ( "Integration Service - Parameter list is empty." );
        this.LogDebug ( "Parameter list is empty." );

        resultData.EventCode =  Evado.Integration.Model.EiEventCodes.Integration_Import_Parameter_Error;
        resultData.ErrorMessage = "Integration Service - Parameter list is empty.";
        return resultData;
      }

      string projectid = QueryData.GetQueryParameterValue (  Evado.Integration.Model.EiQueryParameterNames.Project_Id );


      //
      // select the query QueryType
      //
      switch ( QueryData.QueryType )
      {
        default:
          {
            resultData.EventCode = EiEventCodes.Integration_Import_Type_Id_Error;
            resultData.ErrorMessage = Evado.Model.EvStatics.enumValueToString( EiEventCodes.Integration_Import_Type_Id_Error );
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
    /// <param name="FieldList">List of Evado.Digital.Model.EvFormField objects</param>
    /// <param name="ResultData"> Evado.Integration.Model.EiData object</param>
    //-----------------------------------------------------------------------------------
    public void getFormFieldColumns (
      List<Evado.Digital.Model.EdRecordField> FieldList,
       Evado.Integration.Model.EiData ResultData )
    {
      this.LogMethod ( "getFormFieldColumns method." );
      this.LogDebug ( "FieldList.Count: " + FieldList.Count );

      //
      // Iterate through the field list creating column object for each field object.
      //
      foreach (Evado.Digital.Model.EdRecordField field in FieldList)
      {
        this.LogDebug ( "Field: " + field.FieldId + ", Type: " + field.TypeId );
        //
        // add the correct column type for the field data type.
        //
        switch (field.TypeId)
        {
          case Evado.Model.EvDataTypes.Boolean:
          {
            this.LogDebug ( "ADD: Boolean Column" );
            //
            //The default data type is text.
            //
            ResultData.AddColumn(
                  Evado.Integration.Model.EiDataTypes.Boolean,
                 field.FieldId );

            break;
          }
          case Evado.Model.EvDataTypes.Numeric:
          {
            this.LogDebug ( "ADD: Floating_Point Column" );
            //
            //The default data type is text.
            //
            ResultData.AddColumn( 
               Evado.Integration.Model.EiDataTypes.Floating_Point,
                 field.FieldId );

            break;
          }
          case Evado.Model.EvDataTypes.Date:
          {
            this.LogDebug ( "ADD: DateTime Column" );
            //
            //The default data type is text.
            //
            ResultData.AddColumn(
                  Evado.Integration.Model.EiDataTypes.Date,
                 field.FieldId );

            break;
          }
          case Evado.Model.EvDataTypes.Check_Box_List:
          {
            this.LogDebug ( "ADD: Check_Button_List columns" );

            this.getCheckBoxFieldColumn ( field, ResultData );

            break;
          }
          case Evado.Model.EvDataTypes.Table:
          case Evado.Model.EvDataTypes.Special_Matrix:
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
            ResultData.Columns.Add ( new  Evado.Integration.Model.EiColumnParameters (
                  Evado.Integration.Model.EiDataTypes.Text,
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
    /// <param name="FieldList">List of Evado.Digital.Model.EvFormField objects</param>
    /// <param name="ResultData"> Evado.Integration.Model.EiData object</param>
    /// <param name="Row"> Evado.Integration.Model.EiDataRow object</param>
    //-----------------------------------------------------------------------------------
   // private void getFormFieldColumnData (
    public void getFormFieldColumnData (
      List<Evado.Digital.Model.EdRecordField> FieldList,
       Evado.Integration.Model.EiData ResultData,
       Evado.Integration.Model.EiDataRow Row )
    {
      this.LogMethod ( "getFormFieldColumnData method." );
      //
      // Initialise the method variables and objects.
      //
      int column = 0;

      //
      // Iterate through the field list creating column object for each field object.
      //
      foreach (Evado.Digital.Model.EdRecordField field in FieldList)
      {
        //
        // add the correct column type for the field data type.
        //
        switch (field.TypeId)
        {
          case Evado.Model.EvDataTypes.Boolean:
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
          case Evado.Model.EvDataTypes.Free_Text:
          {
            column = ResultData.getColumnNo ( field.FieldId );

            this.writeProcessLog ( "Column: " + column + ",\t FieldId: " + field.FieldId + ", Type: " + field.TypeId + ", Value: " + field.ItemValue );

            String text = field.ItemText.Replace ( "\r", "^" );
            text = text.Replace ( "\n", "^" );
            text = text.Replace ( "^^", "^" );

            Row.updateValue ( column, text );

            break;
          }
          case Evado.Model.EvDataTypes.Check_Box_List:
          {
            this.getCheckBoxFieldColumnValues ( field, ResultData, Row );
            break;
          }
          case Evado.Model.EvDataTypes.Table:
          case Evado.Model.EvDataTypes.Special_Matrix:
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
    /// <param name="Field">Evado.Digital.Model.EvFormField object</param>
    /// <param name="ResultData"> Evado.Integration.Model.EiData object</param>
    //-----------------------------------------------------------------------------------
    //private void getCheckBoxFieldColumn (
    public void getCheckBoxFieldColumn (
      Evado.Digital.Model.EdRecordField Field,
       Evado.Integration.Model.EiData ResultData )
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
              Evado.Integration.Model.EiDataTypes.Boolean,
             Field.FieldId + "_" + count + "_" + option.Value );

      }//END option iteration loop.

    }//END getFormFieldColumns method

    //===================================================================================
    /// <summary>
    /// This method appends the column objects for a list of form fields.
    /// </summary>
    /// <param name="Field">Evado.Digital.Model.EvFormField object</param>
    /// <param name="ResultData"> Evado.Integration.Model.EiData object</param>
    /// <param name="Row"> Evado.Integration.Model.EiDataRow object</param>
    //-----------------------------------------------------------------------------------
    private void getCheckBoxFieldColumnValues (
      Evado.Digital.Model.EdRecordField Field,
       Evado.Integration.Model.EiData ResultData,
       Evado.Integration.Model.EiDataRow Row )
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
    /// <param name="Field">Evado.Digital.Model.EvFormField object</param>
    /// <param name="ResultData"> Evado.Integration.Model.EiData object</param>
    //-----------------------------------------------------------------------------------
    //private void getTableFieldColumn (
    public void getTableFieldColumn (
      Evado.Digital.Model.EdRecordField Field,
       Evado.Integration.Model.EiData ResultData )
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
           Evado.Integration.Model.EiColumnParameters parameter = new  Evado.Integration.Model.EiColumnParameters (
                Evado.Integration.Model.EiDataTypes.Text,
               Field.FieldId + "_" + row + "_" + column + "_" + title );

          switch (Field.Table.Header [ tableColumn ].TypeId)
          {
            case  Evado.Model.EvDataTypes.Numeric:
            {
              parameter.DataType =  Evado.Integration.Model.EiDataTypes.Floating_Point;
              break;
            }
            case  Evado.Model.EvDataTypes.Date:
            {
              parameter.DataType =  Evado.Integration.Model.EiDataTypes.Date;
              break;
            }
            case  Evado.Model.EvDataTypes.Yes_No:
            case  Evado.Model.EvDataTypes.Boolean:
            {
              parameter.DataType =  Evado.Integration.Model.EiDataTypes.Boolean;
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
    /// <param name="Field">Evado.Digital.Model.EvFormField object</param>
    /// <param name="ResultData"> Evado.Integration.Model.EiData object</param>
    /// <param name="Row"> Evado.Integration.Model.EiDataRow object</param>
    //-----------------------------------------------------------------------------------
    //private void getTableFieldColumnValues (
    public void getTableFieldColumnValues (
      Evado.Digital.Model.EdRecordField Field,
       Evado.Integration.Model.EiData ResultData,
       Evado.Integration.Model.EiDataRow Row )
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
              case Evado.Model.EvDataTypes.Yes_No:
              case Evado.Model.EvDataTypes.Boolean:
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
