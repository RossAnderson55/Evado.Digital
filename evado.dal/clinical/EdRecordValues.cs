/***************************************************************************************
 * <copyright file="dal\EvRecordFields.cs" company="EVADO HOLDING PTY. LTD.">
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
using System.Xml.Serialization;
using System.Text;
using Newtonsoft.Json;

//Application specific class references.
using Evado.Model;
using Evado.Model.Digital;


namespace Evado.Dal.Clinical
{
  /// <summary>
  /// This class is handles the data access layer for the form record field data object.
  /// </summary>
  public class EdRecordValues : EvDalBase
  {
    #region class initialisation method.
    /// <summary>
    /// This method initialises the schedule DAL class.
    /// </summary>
    public EdRecordValues ( )
    {
      this.ClassNameSpace = "Evado.Dal.Clinical.EdRecordFields.";
    }

    /// <summary>
    /// This method initialises the schedule DAL class.
    /// </summary>
    public EdRecordValues ( EvClassParameters Settings )
    {
      this.ClassParameters = Settings;
      this.ClassNameSpace = "Evado.Dal.Clinical.EdRecordFields.";

      if ( this.ClassParameters.LoggingLevel == 0 )
      {
        this.ClassParameters.LoggingLevel = Evado.Dal.EvStaticSetting.LoggingLevel;
      }
    }

    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Object Initialisation and constants
    /* *********************************************************************************
     * 
     * Defines the classes constansts and global variables
     * 
     * *********************************************************************************/
    // The log file source. 
    private static string _eventLogSource = ConfigurationManager.AppSettings [ "EventLogSource" ];
    // 
    // selectionList query string.
    // 
    private const string SQL_QUERY = "Select * \r\nFROM EvRecordField_View \r\n";

    // 
    // selectionList query string.
    // 
    private const string SQL_DATA_POINT_QUERY = "Select * \r\nFROM EvRecordField_DataPoints \r\n";

    /// <summary>
    /// This constant defines the data point index order for creating the index and 
    /// retrieving the fields.
    /// </summary>
    public const string SQL_UNSCHEDULED_DATA_POINT_INDEX = "SCHEDULE_ID, VisitId, M_Order, MilestoneId, MA_Order, "
      + " MA_IsMandatory, ActivityId, ACF_Order, ACF_Mandatory, FormId, TCI_Order, FieldId";

    // 
    // selectionList query string.
    // 
    private const string SQL_MONITORING_REPORT_QUERY = "Select * FROM EvRpt_FIELD_MONITOR_QUERY ";

    // 
    // Data Value Selection visitSchedule query string
    //  
    private const string SQL_QUERY_VALUE_LIST = "Select TRI_TextValue "
      + " FROM EvRecordField_ValueSelection ";

    private const string SQL_RECORD_FIELD_VALUE_LIST = "Select * FROM EV_RECORD_FIELD_VALUES ";

    #region Define the stored procedure names.
    /// <summary>
    /// This coanstant defines a storeprocedure for adding items to record field table.
    /// </summary>
    public const string _storedProcedureAddItem = "usr_RecordField_add";

    /// <summary>
    /// This coanstant defines a storeprocedure for updating items on record field table.
    /// </summary>
    private const string _storedProcedureUpdateItem = "usr_RecordField_update";

    /// <summary>
    /// This coanstant defines a storeprocedure for deleting items from record field table.
    /// </summary>
    private const string _storedProcedureDeleteItem = "usr_RecordField_delete";
    #endregion

    #region Define the query parameter constants.

    // SQL database column names
    private const string DB_RECORD_GUID = "EDR_GUID";
    private const string DB_VALUES_GUID = "EDRV_GUID";
    private const string DB_FIELD_GUID = "EDRLF_GUID";

    private const string DB_VALUES_COLUMN_ID = "EDRV_COLUMN_ID";
    private const string DB_VALUES_ROW = "EDRV_ROW";
    private const string DB_VALUES_STRING = "EDRV_STRING";
    private const string DB_VALUES_NUMERIC = "EDRV_NUMERIC";
    private const string DB_VALUES_DATE = "EDRV_DATE";
    private const string DB_VALUES_TEXT = "EDRV_TEXT";

    // SQL parameter strings.
    private const string PARM_RECORD_GUID = "@RECORD_GUID";
    private const string PARM_VALUE_GUID = "@GUID";
    private const string PARM_FIELD_GUID = "@FIELD_GUID";
    private const string PARM_VALUE_COLUMN_ID = "@COLUMN_ID";
    private const string PARM_VALUE_ROW = "@ROW";
    private const string PARM_VALUE_STRING = "@STRING_VALUE";
    private const string PARM_VALUE_DATE = "@DATE_VALUE";
    private const string PARM_VALUE_NUMERIC = "@NUMERIC_VALUE";
    private const string PARM_VALUE_TEXT = "@TEXT_VALUE";

    #endregion

    //
    //  Define the SQL query string variable.
    //
    private string _Sql_QueryString = String.Empty;

    private List<EvFormRecordComment> _FieldCommandList = new List<EvFormRecordComment> ( );

    //
    // This variable is used to skip retrieving comments when updating the form record.
    //
    private bool _SkipRetrievingComments = false;

    private String _AnnotationText = String.Empty;

    #endregion

    #region RecordField Reader

    // =====================================================================================
    /// <summary>
    /// This class reads the content of the data reader object into ChecklistItem business object.
    /// </summary>
    /// <param name="Row">DataRow: an Sql DataReader object</param>
    /// <returns>EvFormField: a formfield object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Extract the compatible row data to the formfield object. 
    /// 
    /// 2. Deserialize the xmlData to the formfield design object if it exists.
    /// 
    /// 3. Deserialize the xmlvalidationRules to the formfield validationRules object if they exist. 
    /// 
    /// 4. Reset the horizontal radion button list enumertion
    /// 
    /// 5. Ensure that the formfield state is not null.
    /// 
    /// 6. if skip retrieving comments is selected, fill the comment list and format it
    /// 
    /// 7. Get the table object and the external selection list object.
    /// 
    /// 8. Process the NA values in selectionlists.
    /// 
    /// 9. Update the current formfield state and type object. 
    /// 
    /// 10. Return the formfield object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private EdRecordField getRowData (
      DataRow Row )
    {
      //this.LogMethod ( "getRowData method" );
      // 
      // Initialise method template table string, a return formfield object and an annotation string. 
      // 
      string stTemplateTable = String.Empty;
      EdRecordField recordField = new EdRecordField ( );

      // 
      // Fill the evForm object.l
      //
      recordField.Guid = EvSqlMethods.getGuid ( Row, EdRecordValues.DB_VALUES_GUID );
      recordField.RecordGuid = EvSqlMethods.getGuid ( Row, EdRecords.DB_RECORD_GUID );
      recordField.LayoutGuid = EvSqlMethods.getGuid ( Row, EdRecords.DB_LAYOUT_GUID );
      recordField.FormFieldGuid = EvSqlMethods.getGuid ( Row, EdRecordValues.DB_FIELD_GUID );


      recordField.FieldId = EvSqlMethods.getString ( Row, EdRecordFields.DB_FIELD_ID );
      String value = EvSqlMethods.getString ( Row, EdRecordFields.DB_TYPE_ID );
      recordField.TypeId = Evado.Model.EvStatics.Enumerations.parseEnumValue<Evado.Model.EvDataTypes> ( value );

      recordField.Design.Title = EvSqlMethods.getString ( Row, EdRecordFields.DB_TITLE );
      recordField.Design.Instructions = EvSqlMethods.getString ( Row, EdRecordFields.DB_INSTRUCTIONS );
      recordField.Design.HttpReference = EvSqlMethods.getString ( Row, EdRecordFields.DB_HTTP_REFERENCE );
      recordField.Design.SectionNo = EvSqlMethods.getInteger ( Row, EdRecordFields.DB_SECTION_ID );
      recordField.Design.Options = EvSqlMethods.getString ( Row, EdRecordFields.DB_OPTIONS );
      recordField.Design.SummaryField = EvSqlMethods.getBool ( Row, EdRecordFields.DB_SUMMARY_FIELD );
      recordField.Design.Mandatory = EvSqlMethods.getBool ( Row, EdRecordFields.DB_MANDATORY );
      recordField.Design.AiDataPoint = EvSqlMethods.getBool ( Row, EdRecordFields.DB_AI_DATA_POINT );
      recordField.Design.HideField = EvSqlMethods.getBool ( Row, EdRecordFields.DB_HIDDEN );
      recordField.Design.ExSelectionListId = EvSqlMethods.getString ( Row, EdRecordFields.DB_EX_SELECTION_LIST_ID );
      recordField.Design.ExSelectionListCategory = EvSqlMethods.getString ( Row, EdRecordFields.DB_EX_SELECTION_LIST_CATEGORY );
      recordField.Design.DefaultValue = EvSqlMethods.getString ( Row, EdRecordFields.DB_DEFAULT_VALUE );
      recordField.Design.Unit = EvSqlMethods.getString ( Row, EdRecordFields.DB_UNIT );
      recordField.Design.UnitScaling = EvSqlMethods.getString ( Row, EdRecordFields.DB_UNIT_SCALING );

      recordField.Design.ValidationLowerLimit = EvSqlMethods.getFloat ( Row, EdRecordFields.DB_VALIDATION_LOWER_LIMIT );
      recordField.Design.ValidationUpperLimit = EvSqlMethods.getFloat ( Row, EdRecordFields.DB_VALIDATION_UPPER_LIMIT );
      recordField.Design.AlertLowerLimit = EvSqlMethods.getFloat ( Row, EdRecordFields.DB_ALERT_LOWER_LIMIT );
      recordField.Design.AlertUpperLimit = EvSqlMethods.getFloat ( Row, EdRecordFields.DB_ALERT_UPPER_LIMIT );
      recordField.Design.NormalRangeLowerLimit = EvSqlMethods.getFloat ( Row, EdRecordFields.DB_NORMAL_LOWER_LIMITD );
      recordField.Design.NormalRangeUpperLimit = EvSqlMethods.getFloat ( Row, EdRecordFields.DB_NORMAL_UPPER_LIMIT );

      recordField.Design.FieldCategory = EvSqlMethods.getString ( Row, EdRecordFields.DB_FIELD_CATEGORY );
      recordField.Design.AnalogueLegendStart = EvSqlMethods.getString ( Row, EdRecordFields.DB_ANALOGUE_LEGEND_START );
      recordField.Design.AnalogueLegendFinish = EvSqlMethods.getString ( Row, EdRecordFields.DB_ANALOGUE_LEGEND_FINISH );
      recordField.Design.JavaScript = EvSqlMethods.getString ( Row, EdRecordFields.DB_JAVA_SCRIPT );
      recordField.Design.InitialOptionList = EvSqlMethods.getString ( Row, EdRecordFields.DB_INITIAL_OPTION_LIST );
      recordField.Design.InitialVersion = EvSqlMethods.getInteger ( Row, EdRecordFields.DB_INITIAL_VERSION );
      //
      // if the field is a signature then decrypt the field.
      //
      if ( recordField.TypeId == EvDataTypes.Signature )
      {
        this.LogDebug ( "Encrypted Signature string" );
        EvEncrypt encrypt = new EvEncrypt ( this.ClassParameters.ApplicationGuid, recordField.Guid );
        encrypt.ClassParameters = this.ClassParameters;

        value = encrypt.decryptString ( recordField.ItemText );
        this.LogDebug ( "clear string: " + value );
        recordField.ItemText = value;

        this.LogClass ( encrypt.Log );
      }

      //
      // Get the table or matric object.
      //
      this.processTableRowObject ( Row, recordField );

      //
      // Get the external selection list values.
      //
      this.getExternalSelectionList ( Row, recordField );

      //
      // Process the NA values in selectionlists.
      //
      this.processNotAvailableValues ( Row, recordField );

      //
      // Return the formfield object. 
      //
      return recordField;

    }//END getRowData method.

    // =====================================================================================
    /// <summary>
    /// This method attached external selectionlist options to the formfield list.
    /// </summary>
    /// <param name="Row">DataRow: an sql data row object</param>
    /// <param name="Field">EvFormField: a form field object</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Validate whether the formfield type is external selectionlist. 
    /// 
    /// 2. Initialize the external codeing list object.
    /// 
    /// 3. Attach the external coding lists to the formfield option list.
    /// 
    /// 4. Reset the formfield type to be selectionlist
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private void getExternalSelectionList (
      DataRow Row,
      EdRecordField Field )
    {
      // 
      // Validate whether the formfield type is external selectionlist. 
      // 
      if ( Field.TypeId != Evado.Model.EvDataTypes.External_Selection_List )
      {
        return;
      }
      this.LogMethod ( "getExternalSelectionList method" );

      //
      // Initialize the external codeing list object.
      //
      EdRecordFieldSelectionLists externalCodingLists = new EdRecordFieldSelectionLists ( );

      this.LogDebug ( "Ext ListId: " + Field.Design.ExSelectionListId
        + " Category: " + Field.Design.ExSelectionListCategory );

      //
      // Attach the external coding lists to the formfield option list.
      //
      Field.Design.Options = externalCodingLists.getItemCodingList (
        Field.Design.ExSelectionListId,
        Field.Design.ExSelectionListCategory );

      //
      // Reset the formfield type to be selectionlist.
      //
      Field.TypeId = Evado.Model.EvDataTypes.Selection_List;

    }//END getExternalSelectionList method

    // =====================================================================================
    /// <summary>
    /// This method processes form field table or matrix object.
    /// </summary>
    /// <param name="Row">DataRow: a sql data row object</param>
    /// <param name="Field">EvFormField: a formfield object</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Validate whether the formfield type is matrix and table. 
    /// 
    /// 2. if the Itemtext value is empty then initialise it with the EvFormField table value.
    /// 
    /// 3. Deserialize the formfield item text to the formfield table. 
    /// 
    /// 4. Empty the formfield itemtext so it will not cause problems when XML styling the record object.
    /// 
    /// 5. Iterate through the table for setting the validation objects.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private void processTableRowObject (
      DataRow Row,
      EdRecordField Field )
    {
      //this.LogMethod ( "processTableRowObject method " );
      //this.LogDebugValue ( "FieldId: " + Field.FieldId );
      //this.LogDebugValue ( "Table Length: " + Field.ItemText.Length );
      //
      // Validate whether the formfield type is matrix and table. 
      //
      if ( Field.TypeId != Evado.Model.EvDataTypes.Special_Matrix
        && Field.TypeId != Evado.Model.EvDataTypes.Table )
      {
        //this.LogMethodEnd ( "processTableRowObject" );
        return;
      }

      // 
      // if the Itemtext value is empty then initialise it with the EvFormField table value.
      // 
      if ( Field.ItemText == String.Empty )
      {
        //this.LogDebugValue ( "Reset table value to form default." );
        Field.ItemText = EvSqlMethods.getString ( Row, EdRecordFields.DB_TABLE );
      }
      // 
      // Deserialize the formfield item text to the formfield table. 
      // 
      Field.Table = Evado.Model.Digital.EvcStatics.DeserialiseObject<EdRecordTable> ( Field.ItemText );

      // 
      // Empty the formfield itemtext so it will not cause problems when XML styling the record object.
      // 
      Field.ItemText = String.Empty;

      // 
      // Iterate through the table for setting the validation objects.
      // 
      for ( int i = 0; i < Field.Table.ColumnCount; i++ )
      {
        //
        // Addressing the 'NA' to negative infinity issue for non-numeric fields.
        //
        // Iterate through the table data converting the relevant cell values to NA.
        //
        for ( int j = 0; j < Field.Table.Rows.Count; j++ )
        {
          String cell = Field.Table.Rows [ j ].Column [ i ];

          if ( Field.Table.Header [ i ].TypeId != EvDataTypes.Numeric )
          {
            Field.Table.Rows [ j ].Column [ i ] = Evado.Model.EvStatics.convertNumNullToTextNull ( cell );
          }

          //this.LogDebugValue ( "R:" + j  + ", C:" + i + ", V: " + cell );

        }//END column iteration loop

      }//END newField iteration loop

    }//END processTableRowObject method

    // =====================================================================================
    /// <summary>
    /// This methoid processes the not available values. 
    /// </summary>
    /// <param name="Row">DataRow: an sql data row object</param>
    /// <param name="Field">EvFormField: a form field object</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. To address Null Numeric bug, convert from 1E-45F to Float Negative Infinity.
    /// 
    /// 2. Reset the numeric 'NA' for negative infinity issue and null value.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private void processNotAvailableValues (
      DataRow Row,
      EdRecordField Field )
    {
      //this.LogMethod ( "processNotAvailableValues method " );
      //
      // To address Null Numeric bug, convert from 1E-45F to Float Negative Infinity.
      //
      Field.ItemValue = Evado.Model.EvStatics.convertNumNullToTextNull ( Field.ItemValue );

      //
      // Reset the numeric 'NA' for negative infinity issue.
      //
      if ( Field.TypeId == Evado.Model.EvDataTypes.Selection_List
        || Field.TypeId == Evado.Model.EvDataTypes.External_Selection_List
        || Field.TypeId == Evado.Model.EvDataTypes.Horizontal_Radio_Buttons
        || Field.TypeId == Evado.Model.EvDataTypes.Radio_Button_List )
      {
        Field.Design.Options = Field.Design.Options.Replace ( " :", ":" );
        if ( Evado.Model.Digital.EvcStatics.hasNumericNul ( Field.ItemValue ) == true )
        {
          Field.ItemValue = Evado.Model.Digital.EvcStatics.CONST_NUMERIC_NOT_AVAILABLE;
        }
        if ( Field.ItemValue == "0"
          && Field.Design.Options.Contains ( Evado.Model.Digital.EvcStatics.CONST_NUMERIC_NOT_AVAILABLE + ":" ) == true
          && Field.Design.Options.Contains ( @"0:" ) == false )
        {
          Field.ItemValue = Evado.Model.Digital.EvcStatics.CONST_NUMERIC_NOT_AVAILABLE;
        }
      }
      if ( Field.TypeId != Evado.Model.EvDataTypes.Numeric )
      {
        if ( Evado.Model.Digital.EvcStatics.hasNumericNul ( Field.ItemValue ) == true )
        {
          Field.ItemValue = Evado.Model.Digital.EvcStatics.CONST_NUMERIC_NOT_AVAILABLE;
        }
      }

      //this.LogMethodEnd ( "processNotAvailableValues" );
    }//END processNotAvailableValues method

    #endregion

    #region Recor field list Queries

    // =====================================================================================
    /// <summary>
    /// This class returns a list of formfield object retrieved by the record Guid. 
    /// </summary>
    /// <param name="Record">EvForm: (Mandatory) The record object.</param>
    /// <param name="IncludeComments">bool: true = include field comments.</param>
    /// <returns>List of EvFormField: a formfield object.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Return an empty formfield object if the record's Guid is empty 
    /// 
    /// 2. Define the sql query parameters and sql query string 
    /// 
    /// 3. Execute the sql query string with parameters and store the results on data table. 
    /// 
    /// 4. Iterate through the table and extract the data row to the formfield object. 
    /// 
    /// 5. Add object result to the formfields list. 
    /// 
    /// 6. Return the formfields list.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EdRecordField> getRecordFieldList (
      EdRecord Record )
    {
      this.LogMethod ( "getRecordFieldList method. " );
      this.LogDebug ( "Record.Guid: " + Record.Guid );
      //
      // Initialise the methods variables and objects.
      //
      List<EdRecordField> recordFieldList = new List<EdRecordField> ( );
      EdRecordField recordField = new EdRecordField ( );
      Guid lastRecordFieldGuid = Guid.Empty;

      // 
      // Validate whether the record Guid is not empty. 
      // 
      if ( Record.Guid == Guid.Empty )
      {
        return recordFieldList;
      }

      // 
      // Define the SQL query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(PARM_RECORD_GUID, SqlDbType.UniqueIdentifier),
      };
      cmdParms [ 0 ].Value = Record.Guid;

      // 
      // Define the query string.
      // 
      _Sql_QueryString = SQL_QUERY + " WHERE ( " + EdRecordValues.DB_RECORD_GUID + " =" + EdRecordValues.PARM_RECORD_GUID + ") "
        + "ORDER BY " + EdRecordFields.DB_ORDER + "; ";

      this.LogDebug ( _Sql_QueryString );

      // 
      // Execute the query against the database.
      // 
      using ( DataTable table = EvSqlMethods.RunQuery ( _Sql_QueryString, cmdParms ) )
      {
        if ( table.Rows.Count == 0 )
        {
          return recordFieldList;
        }

        // 
        // Iterate through the results extracting the role information.
        // 
        for ( int count = 0; count < table.Rows.Count; count++ )
        {
          // 
          // Extract the table row
          // 
          DataRow row = table.Rows [ count ];

          Guid recordValueGuid = EvSqlMethods.getGuid ( row, EdRecordValues.DB_VALUES_GUID );

          //
          // Empty fields are skipped.
          //
          if ( recordValueGuid == Guid.Empty )
          {
            continue;
          }
          // If the field guid has changed then it is a new field.
          // So add the previous field then get the data for the new field.
          //
          if ( lastRecordFieldGuid != recordValueGuid )
          {
            this.LogDebug ( "Add field to record field list." );
            //
            // Add the last field to the list.
            //
            if ( recordField.Guid != Guid.Empty )
            {
              recordFieldList.Add ( recordField );
            }

            // 
            // Get the object data from the row.
            // 
            recordField = this.getRowData ( row );

            //
            // Update the lst field guid to enable the other field values to be collected.
            //
            lastRecordFieldGuid = recordField.Guid;

          }//END create new field object.

          else
          {

            this.LogDebug ( "Secondary field value." );

            switch ( recordField.TypeId )
            {
              case Evado.Model.EvDataTypes.Special_Matrix:
              case Evado.Model.EvDataTypes.Table:
                {
                  this.getTableCellValue ( row, recordField );
                  break;
                }
              case Evado.Model.EvDataTypes.Check_Box_List:
                {
                  this.getCheckBoxValue ( row, recordField );
                  break;
                }
            }
          }//END value records that need to update the current field.

        }//ENR record iteration loop.

      }//ENd using statement

      //
      // Add the last field to the list.
      //
      if ( recordField.Guid != Guid.Empty )
      {
        recordFieldList.Add ( recordField );
      }

      // 
      // Return the formfields list.
      // 
      return recordFieldList;

    }//END getRecordFieldList method.

    // =====================================================================================
    /// <summary>
    /// this method processes a form field type that is a numeric.
    /// </summary>
    /// <param name="Row">The DataRow containing the field</param>
    /// <param name="FormField"> Evado.Model.Forms.EvFormField object, containg the field data.</param>
    // -------------------------------------------------------------------------------------
    private void getCheckBoxValue ( DataRow Row, Evado.Model.Digital.EdRecordField FormField )
    {
      LogMethod ( "getCheckBoxValue" );
      //
      // Get the field value.
      //
      String columnId = EvSqlMethods.getString ( Row, EdRecordValues.DB_VALUES_COLUMN_ID );
      bool value = EvSqlMethods.getBool ( Row, EdRecordValues.DB_VALUES_NUMERIC );

      this.LogDebug ( " columnId: " + columnId + ", value: " + value );

      if ( value == false )
      {
        LogMethodEnd ( "getCheckBoxValue" );
        return;
      }

      //
      // iterate through the list looking for a matching option value.
      //
      foreach ( EvOption option in FormField.Design.OptionList )
      {
        if ( option.Value == columnId )
        {
          FormField.ItemValue += ";" + option.Value;
        }
      }

      LogMethodEnd ( "getCheckBoxValue" );
    }//END getCheckBoxValue method

    // =====================================================================================
    /// <summary>
    /// this method updates a table cell value form the query result set.
    /// Note:  the cell indexes are zero based.
    /// </summary>
    /// <param name="Row">The DataRow containing the field</param>
    /// <param name="FormField"> Evado.Model.Forms.EvFormField object, containg the field data.</param>
    // -------------------------------------------------------------------------------------
    private void getTableCellValue ( DataRow Row, Evado.Model.Digital.EdRecordField FormField )
    {
      LogMethod ( "getTableCellValue" );
      //
      // Validate that the table exists.
      //
      if ( FormField.Table == null )
      {
        this.LogDebug ( "FormField.Table is null" );
        return;
      }

      //
      // Get the field row values.
      //
      int row = EvSqlMethods.getInteger ( Row, EdRecordValues.DB_VALUES_ROW );
      String columnId = EvSqlMethods.getString ( Row, EdRecordValues.DB_VALUES_COLUMN_ID );

      //
      // get the colum no
      //
      int col = getHeaderColumnNo ( FormField.Table.Header, columnId );

      //
      // get the column row value.
      //
      string value = this.getTableColumnValue ( Row, col, FormField );
      LogMethodEnd ( "getTableColumnValue" );

      //
      // Add the value to the table.
      //
      if ( row < FormField.Table.Rows.Count )
      {
        FormField.Table.Rows [ row ].Column [ col ] = value;
      }
      this.LogDebug ( " Row: " + row + " col: " + col + ", value: " + FormField.Table.Rows [ row ].Column [ col ] );

      LogMethodEnd ( "getTableCellValue" );
    }//END getTableCellValue method


    // =====================================================================================
    /// <summary>
    /// This method returns the column no for a specific columnid
    /// </summary>
    /// <param name="Header">Array of EdRecordTableHeader objects</param>
    /// <param name="ColumnId">String column identifier</param>
    /// <returns>Int: column number</returns>
    // -------------------------------------------------------------------------------------
    private int getHeaderColumnNo ( EdRecordTableHeader [ ] Header, String ColumnId )
    {
      int columNo = 0;

      for ( int i = 0; i < Header.Length; i++ )
      {
        if ( Header [ i ].ColumnId == ColumnId )
        {
          return i;
        }
      }
      return columNo;

    }//ENd getHeaderColumnNo method

    // =====================================================================================
    /// <summary>
    /// this method updates a table cell value form the query result set.
    /// Note:  the cell indexes are zero based.
    /// </summary>
    /// <param name="Row">The DataRow containing the field</param>
    /// <param name="Col">Int: The column index in the able.</param>
    /// <param name="RecordField"> Evado.Model.Forms.EvFormField object, containg the field data.</param>
    // -------------------------------------------------------------------------------------
    private String getTableColumnValue ( DataRow Row, int Col, Evado.Model.Digital.EdRecordField RecordField )
    {
      LogMethod ( "getTableColumnValue" );
      //
      // Select the column to retieve the value from
      //
      switch ( RecordField.Table.Header [ Col ].TypeId )
      {
        case EvDataTypes.Numeric:
          {
            float fltValue = EvSqlMethods.getFloat ( Row, EdRecordValues.DB_VALUES_NUMERIC );
            return fltValue.ToString ( );
          }
        case EvDataTypes.Yes_No:
          {
            bool bYesNo = EvSqlMethods.getBool ( Row, EdRecordValues.DB_VALUES_NUMERIC );
            string value = "No";
            if ( bYesNo == true )
            {
              value = "Yes";
            }
            return value;
          }
        case EvDataTypes.Date:
          {
            DateTime dtValue = EvSqlMethods.getDateTime ( Row, EdRecordValues.DB_VALUES_DATE );

            if ( dtValue == Evado.Model.EvStatics.CONST_DATE_NULL )
            {
              return String.Empty;
            }

            //
            // Return the value.
            //
            return dtValue.ToString ( "dd MMM yyyy" );
          }
        default:
          {
            return EvSqlMethods.getString ( Row, EdRecordValues.DB_VALUES_STRING );
          }
      }//END table column type switch.

    }//END getTableColumnValue method

    #endregion

    #region Form Field Report methods.

    // =====================================================================================
    /// <summary>
    /// This method should be overriden by all of the classes that want to provide data for the reports.
    /// </summary>
    /// <param name="Report">EvReport: a Report data object</param>
    /// <returns>EvReport: a Report data object</returns>
    /// <remarks>
    /// 
    /// 1. Set the report title, datasource and date.
    /// 
    /// 2. Add six report headers
    /// 
    /// 3. Define the sql query parameters and sql query string. 
    /// 
    /// 4. Execute the sql query string and store the results on data table. 
    /// 
    /// 5. Loop through the table and extract the data row to the data row object. 
    /// 
    /// 6. If data row has no value, create new data row object. 
    /// 
    /// 7. Add data row object values to the Report object. 
    /// 
    /// 8. Return the Report data object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvReport getMonitoringReport ( EvReport Report )
    {
      this.LogMethod ( "getReport method. " );
      this.LogDebug ( "Query count: " + Report.Queries.Length );
      this.LogDebug ( "Columns count: " + Report.Columns.Count );

      return Report;

      this.LogDebug ( "Parameters: " );
      for ( int i = 0; i < Report.Queries.Length; i++ )
      {
        this.LogDebug ( Report.Queries [ i ].SelectionSource + " = " + Report.Queries [ i ].Value );
      }


      //
      // Initialize the Method debug log, a report rows list and a formfield object
      //
      List<EvReportRow> reportRows = new List<EvReportRow> ( );
      EdRecordField formField = new EdRecordField ( );
      int inNoColumns = Report.Columns.Count;

      //
      // Set the report title, datasource and date.
      //
      Report.ReportTitle = "Form Field Properties";
      Report.DataSourceId = EvReport.ReportSourceCode.FormFields;
      Report.ReportDate = DateTime.Now;

      //
      // Add six report headers
      //
      if ( inNoColumns < 7 )
      {
        Report.Columns = new List<EvReportColumn> ( );

        Report.Columns.Add ( new EvReportColumn ( ) );
        Report.Columns.Add ( new EvReportColumn ( ) );
        Report.Columns.Add ( new EvReportColumn ( ) );
        Report.Columns.Add ( new EvReportColumn ( ) );
        Report.Columns.Add ( new EvReportColumn ( ) );
        Report.Columns.Add ( new EvReportColumn ( ) );
        Report.Columns.Add ( new EvReportColumn ( ) );
        Report.Columns.Add ( new EvReportColumn ( ) );

        // 
        // Set the trial column
        // 
        Report.Columns [ 0 ].HeaderText = "MilestoneId";
        Report.Columns [ 0 ].SectionLvl = 0;
        Report.Columns [ 0 ].GroupingIndex = false;
        Report.Columns [ 0 ].DataType = EvReport.DataTypes.Text;
        Report.Columns [ 0 ].SourceField = "MilestoneId";
        Report.Columns [ 0 ].GroupingType = EvReport.GroupingTypes.None;
        Report.Columns [ 0 ].StyleWidth = "120px";

        // 
        // Set the trial column
        // 
        Report.Columns [ 1 ].HeaderText = "VisitId";
        Report.Columns [ 1 ].SectionLvl = 0;
        Report.Columns [ 1 ].GroupingIndex = false;
        Report.Columns [ 1 ].DataType = EvReport.DataTypes.Text;
        Report.Columns [ 1 ].SourceField = "VisitId";
        Report.Columns [ 1 ].GroupingType = EvReport.GroupingTypes.None;
        Report.Columns [ 1 ].StyleWidth = "120px";

        // 
        // Set the trial column
        // 
        Report.Columns [ 2 ].HeaderText = "ActivityId";
        Report.Columns [ 2 ].SectionLvl = 0;
        Report.Columns [ 2 ].GroupingIndex = false;
        Report.Columns [ 2 ].DataType = EvReport.DataTypes.Text;
        Report.Columns [ 2 ].SourceField = "ActivityId";
        Report.Columns [ 2 ].GroupingType = EvReport.GroupingTypes.None;
        Report.Columns [ 2 ].StyleWidth = "120px";

        // 
        // Set the trial column
        // 
        Report.Columns [ 3 ].HeaderText = "RecordId";
        Report.Columns [ 3 ].SectionLvl = 0;
        Report.Columns [ 3 ].GroupingIndex = false;
        Report.Columns [ 3 ].DataType = EvReport.DataTypes.Text;
        Report.Columns [ 3 ].SourceField = "RecordId";
        Report.Columns [ 3 ].GroupingType = EvReport.GroupingTypes.None;
        Report.Columns [ 3 ].StyleWidth = "120px";

        // 
        // Set the trial column
        // 
        Report.Columns [ 4 ].HeaderText = "Instance";
        Report.Columns [ 4 ].SectionLvl = 0;
        Report.Columns [ 4 ].GroupingIndex = false;
        Report.Columns [ 4 ].DataType = EvReport.DataTypes.Text;
        Report.Columns [ 4 ].SourceField = "Instance";
        Report.Columns [ 4 ].GroupingType = EvReport.GroupingTypes.None;
        Report.Columns [ 4 ].StyleWidth = "60px";

        // 
        // Set the trial column
        // 
        Report.Columns [ 5 ].HeaderText = "Value";
        Report.Columns [ 5 ].SectionLvl = 0;
        Report.Columns [ 5 ].GroupingIndex = false;
        Report.Columns [ 5 ].DataType = EvReport.DataTypes.Text;
        Report.Columns [ 5 ].SourceField = "Value";
        Report.Columns [ 5 ].GroupingType = EvReport.GroupingTypes.None;
        Report.Columns [ 5 ].StyleWidth = "200px";
      }

      // 
      // Define the query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( EdRecords.PARM_APPLICATION_ID, SqlDbType.NVarChar, 10 ),
        new SqlParameter( EdRecords.PARM_LAYOUT_ID, SqlDbType.NVarChar, 10 ),
        new SqlParameter( EdRecordFields.PARM_FIELD_ID, SqlDbType.NVarChar, 10 ),
      };

      //
      // Extract the parameters from the parameter list.
      //
      for ( int i = 0; i < Report.Queries.Length; i++ )
      {
        if ( Report.Queries [ i ].SelectionSource == EvReport.SelectionListTypes.Current_Application )
        {
          cmdParms [ 0 ].Value = Report.Queries [ i ].Value;
        }

        if ( Report.Queries [ i ].SelectionSource == EvReport.SelectionListTypes.Subject_Id )
        {
          cmdParms [ 1 ].Value = Report.Queries [ i ].Value;
        }

        if ( Report.Queries [ i ].SelectionSource == EvReport.SelectionListTypes.LayoutId )
        {
          cmdParms [ 2 ].Value = Report.Queries [ i ].Value;
        }

        if ( Report.Queries [ i ].SelectionSource == EvReport.SelectionListTypes.None
          && Report.Queries [ i ].QueryParameters == "FieldId" )
        {
          cmdParms [ 3 ].Value = Report.Queries [ i ].Value;
        }
      }//END parameter iteration loop.

      //
      // Generate the SQL query string.
      //
      _Sql_QueryString = SQL_MONITORING_REPORT_QUERY
        + " WHERE (TrialId = @TrialId) AND (SubjectId = @SubjectId) AND (FormId = @FormId) AND (FieldId = @FieldId) "
        + " ORDER BY TCI_Order";

      this.LogDebug ( "SQL QUERY: " + _Sql_QueryString );

      //
      // Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( _Sql_QueryString, cmdParms ) )
      {
        this.LogDebug ( "EvSqlMethods Debug: " + EvSqlMethods.Log );

        this.LogDebug ( "Returned Records: " + table.Rows.Count );

        // 
        // Iterate through the results extracting the row information.
        // 
        for ( int count = 0; count < table.Rows.Count; count++ )
        {
          // 
          // Extract the table row
          // 
          DataRow row = table.Rows [ count ];

         //this.getMonitorQueryRowData ( Report, row );

        }//END record interation loop.

      }//END using statement.

      this.LogDebug ( "Field count: " + reportRows.Count );

      //
      // Add new report row if there is no report data
      //
      if ( Report.DataRecords.Count == 0 )
      {
        EvReportRow reportRow = new EvReportRow ( Report.Columns.Count );

        reportRow.ColumnValues [ 0 ] = formField.FieldId;
        reportRow.ColumnValues [ 1 ] = formField.Title;
        reportRow.ColumnValues [ 2 ] = EvStatics.getEnumStringValue ( formField.TypeId );
        reportRow.ColumnValues [ 3 ] = String.Empty;

        Report.DataRecords.Add ( reportRow );

      }//END ADD EMPTY ROW

      // 
      // Return the result array.
      // 
      return Report;
    }

    #endregion

    #region Retrieval Queries

    // =====================================================================================
    /// <summary>
    /// This class retrieves the formfield table based on Guid
    /// </summary>
    /// <param name="Guid">Guid: a formfield global unique identifier</param>
    /// <returns>EvFormField: a formfield object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Return an empty formfield object if the Guid is empty. 
    /// 
    /// 2. Define the sql query parameters and sql query string. 
    /// 
    /// 3. Execute the sql query string and store the results on data table. 
    /// 
    /// 4. Extract the first data row to the formfield object. 
    /// 
    /// 5. Return the formfield data object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private EdRecordField GetItem (
      Guid Guid )
    {
      this.LogMethod ( "GetItem method" );
      this.LogDebug ( "Guid: " + Guid );
      // 
      // Initialize the debug log and a return formfield object. 
      // 
      EdRecordField field = new EdRecordField ( );

      // 
      // Validate the Guid is not empty. 
      // 
      if ( Guid == Guid.Empty )
      {
        return field;
      }

      // 
      // Define the query parameters.
      // 
      SqlParameter cmdParms = new SqlParameter ( PARM_VALUE_GUID, SqlDbType.UniqueIdentifier );
      cmdParms.Value = Guid;

      // 
      // Define the query string.
      // 
      _Sql_QueryString = SQL_QUERY + " WHERE (TRI_Guid = @Guid); ";

      // 
      // Execute the query against the database.
      // 
      using ( DataTable table = EvSqlMethods.RunQuery ( _Sql_QueryString, cmdParms ) )
      {
        // 
        // If not rows the return
        // 
        if ( table.Rows.Count == 0 )
        {
          return field;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        // 
        // Process the results.
        // 
        field = this.getRowData ( row );

      }//EMD using statement.

      // 
      // Pass back the data object.
      // 
      return field;

    }//END GetItem method
    #endregion

    #region EvRecordField Update queries

    //
    // Store the record state to control when to output record values.
    //
    EdRecordObjectStates _RecordState = EdRecordObjectStates.Null;
    int _ValueCount = 0;

    // =====================================================================================
    /// <summary>
    /// This class updates the fields on formfield table using field list, RecordUid and usercommon name.
    /// </summary>
    /// <param name="FormRecord">EvForm object</param>
    /// <returns>EvEventCodes: an event code for updating fields</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Iterate through the formfields object. 
    /// 
    /// 2. If Guid is empty, add new field.
    /// 
    /// 3. Return the event code for updating items. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes UpdateFields (
      EdRecord FormRecord )
    {
      this.LogMethod ( "UpdateFields method " );
      this.LogDebug ( "RecordFieldList.Count: " + FormRecord.Fields.Count );
      this.LogDebug ( "SubmitRecord: " + FormRecord.State );
      // 
      // Initialize the method debug log and the return event code. 
      // 
      EvEventCodes iReturn = EvEventCodes.Ok;

      List<SqlParameter> ParmList = new List<SqlParameter> ( );
      StringBuilder SqlUpdateStatement = new StringBuilder ( );
      this._RecordState = FormRecord.State;


      SqlParameter prm = new SqlParameter ( EdRecordValues.PARM_RECORD_GUID, SqlDbType.UniqueIdentifier );
      prm.Value = FormRecord.Guid;
      ParmList.Add ( prm );

      //
      // Delete the sections
      //
      SqlUpdateStatement.AppendLine ( "DELETE FROM ED_ENTITY_LAYOUT_SECTIONS "
      + "WHERE " + EdRecordValues.DB_RECORD_GUID + "= " + EdRecordValues.PARM_RECORD_GUID + ";  \r\n\r\n" );



      // 
      // Iterate through the formfields object. 
      // 
      foreach ( EdRecordField field in FormRecord.Fields )
      {
        if ( field == null )
        {
          //this.LogDebugValue ( "FIELD NULL" );
          continue;
        }

        //
        // If the field guid is empty create a new one.
        //
        if ( field.Guid == Guid.Empty )
        {
          field.Guid = Guid.NewGuid ( );
        }

        //
        // Create the list of update queries and parameters.
        //
        this.createUpdateQueryAndParameters (
          SqlUpdateStatement,
       ParmList,
       field );

      }//END FormField Update Iteration.

      //
      // Convert the list to an array of SqlPararmeters.
      //
      SqlParameter [ ] parms = new SqlParameter [ ParmList.Count ];

      for ( int i = 0; i < ParmList.Count; i++ )
      {
        parms [ i ] = ParmList [ i ];
      }

      //
      // Execute the update command.
      //
      try
      {
        if ( EvSqlMethods.QueryUpdate ( SqlUpdateStatement.ToString ( ), parms ) == 0 )
        {
          return EvEventCodes.Database_Record_Update_Error;
        }
      }
      catch ( Exception Ex )
      {
        this.LogDebug ( Evado.Model.EvStatics.getException ( Ex ) );
      }

      return EvEventCodes.Ok;

      // 
      // Return Ok 
      // 
      return EvEventCodes.Ok;

    }//END UpdateFields method 

    // =====================================================================================
    /// <summary>
    /// This class update fields on formfield table using formfield object. 
    /// </summary>
    /// <param name="RecordField">EvFormField: a formfield data object</param>
    /// <returns>EvEventCodes: an event code for updating fields</returns>
    // -------------------------------------------------------------------------------------
    private void createUpdateQueryAndParameters (
      StringBuilder SqlUpdateStatement,
      List<SqlParameter> ParmList,
      EdRecordField RecordField )
    {
      this.LogMethod ( "updateField method. " );

      //
      // If readonly field exit method.
      //
      if ( RecordField.isReadOnly == true )
      {
        return;
      }

      switch ( RecordField.TypeId )
      {

        default:
          {
            this.updateSingleValueField ( SqlUpdateStatement, ParmList, RecordField );
            break;
          }
      }

    }//END updateField method

    // =====================================================================================
    /// <summary>
    /// This class update fields on formfield table using formfield object. 
    /// </summary>
    /// <param name="RecordField">EvFormField: a formfield data object</param>
    /// <returns>EvEventCodes: an event code for updating fields</returns>
    // -------------------------------------------------------------------------------------
    private void updateSingleValueField (
      StringBuilder SqlUpdateStatement,
      List<SqlParameter> ParmList,
      EdRecordField RecordField )
    {
      this.LogMethod ( "updateField method. " );
      this.LogDebug ( "ValueCount: " + _ValueCount );

      // 
      // Define the record field Guid
      // 
      SqlParameter prm = new SqlParameter ( EdRecordValues.PARM_FIELD_GUID + "_" + this._ValueCount, SqlDbType.UniqueIdentifier );
      prm.Value = RecordField.FormFieldGuid;
      ParmList.Add ( prm );

      // 
      // Define the record field Guid
      // 
      prm = new SqlParameter ( EdRecordValues.PARM_VALUE_GUID + "_" + this._ValueCount, SqlDbType.UniqueIdentifier );
      prm.Value = RecordField.Guid;
      ParmList.Add ( prm );

      // 
      // Define the record field Guid
      // 
      prm = new SqlParameter ( EdRecordValues.PARM_FIELD_GUID + "_" + this._ValueCount, SqlDbType.UniqueIdentifier );
      prm.Value = RecordField.FormFieldGuid;
      ParmList.Add ( prm );
      // 
      // Define the record column identifier
      // 
      prm = new SqlParameter ( EdRecordValues.PARM_VALUE_COLUMN_ID + "_" + this._ValueCount, SqlDbType.UniqueIdentifier );
      prm.Value = "0";
      ParmList.Add ( prm );

      // 
      // Define the record field Guid
      // 
      prm = new SqlParameter ( EdRecordValues.PARM_VALUE_ROW + "_" + this._ValueCount, SqlDbType.UniqueIdentifier );
      prm.Value = 0;
      ParmList.Add ( prm );


      switch ( RecordField.TypeId )
      {
        case EvDataTypes.Numeric:
          {
            prm = new SqlParameter ( EdRecordValues.PARM_VALUE_NUMERIC + "_" + this._ValueCount, SqlDbType.UniqueIdentifier );
            prm.Value = RecordField.FormFieldGuid;
            ParmList.Add ( prm );

            break;
          }
        case EvDataTypes.Date:
          {
            prm = new SqlParameter ( EdRecordValues.PARM_VALUE_DATE + "_" + this._ValueCount, SqlDbType.UniqueIdentifier );
            prm.Value = RecordField.FormFieldGuid;
            ParmList.Add ( prm );

            break;
          }
        case EvDataTypes.Free_Text:
          {
            prm = new SqlParameter ( EdRecordValues.PARM_VALUE_TEXT + "_" + this._ValueCount, SqlDbType.UniqueIdentifier );
            prm.Value = RecordField.FormFieldGuid;
            ParmList.Add ( prm );
            break;
          }
        default:
          {
            prm = new SqlParameter ( EdRecordValues.PARM_VALUE_STRING + "_" + this._ValueCount, SqlDbType.UniqueIdentifier );
            prm.Value = RecordField.FormFieldGuid;
            ParmList.Add ( prm );
            break;
          }
      }//End switch statement


      //
      // Create the add query .
      //
      SqlUpdateStatement.AppendLine ( " INSERT INTO ED_RECORD_VALUES  "
      + "(" + EdRecordValues.DB_RECORD_GUID
      + ", " + EdRecordValues.DB_FIELD_GUID
      + ", " + EdRecordValues.DB_VALUES_GUID
      + ", " + EdRecordValues.DB_VALUES_COLUMN_ID
      + ", " + EdRecordValues.DB_VALUES_ROW
      + ", " + EdRecordValues.DB_VALUES_STRING
      + ", " + EdRecordValues.DB_VALUES_NUMERIC
      + ", " + EdRecordValues.DB_VALUES_DATE
      + ", " + EdRecordValues.DB_VALUES_TEXT
      + "  ) " );
      SqlUpdateStatement.AppendLine ( "VALUES (" );
      SqlUpdateStatement.AppendLine (
        " " + EdRecordValues.PARM_RECORD_GUID
       + ", " + EdRecordValues.PARM_FIELD_GUID + this._ValueCount
       + ", " + EdRecordValues.PARM_VALUE_GUID + this._ValueCount
       + ", " + EdRecordValues.PARM_VALUE_COLUMN_ID + this._ValueCount
       + ", " + EdRecordValues.PARM_VALUE_ROW + this._ValueCount
       + ", " + EdRecordValues.PARM_VALUE_STRING + this._ValueCount
       + ", " + EdRecordValues.PARM_VALUE_DATE + this._ValueCount
       + ", " + EdRecordValues.PARM_VALUE_NUMERIC + this._ValueCount
       + ", " + EdRecordValues.PARM_VALUE_TEXT + this._ValueCount + " );\r\n" );

    }//END method.

    #endregion

  }//END EvFormRecordFields class

}//END namespace Evado.Dal.Clinical
