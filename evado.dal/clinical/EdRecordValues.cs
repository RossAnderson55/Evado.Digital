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
    // 
    // selectionList query string.
    // 
    private const string SQL_QUERY_VALUES_VIEW = "Select *  FROM ED_RECORD_VALUE_VIEW ";

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

    private List<EdFormRecordComment> _FieldCommandList = new List<EdFormRecordComment> ( );

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
      this.LogMethod ( "getRowData method" );
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
      recordField.LayoutGuid = EvSqlMethods.getGuid ( Row, EdRecordLayouts.DB_LAYOUT_GUID );
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
        EvEncrypt encrypt = new EvEncrypt ( this.ClassParameters.AdapterGuid, recordField.Guid );
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
      this.LogMethodEnd ( "getRowData" );
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

    #region Record field list Queries

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
      Guid previousValueGuid = Guid.Empty;

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
      _Sql_QueryString = SQL_QUERY_VALUES_VIEW + " WHERE ( " + EdRecordValues.DB_RECORD_GUID + " =" + EdRecordValues.PARM_RECORD_GUID + ") "
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

          this.LogDebug ( "previousValueGuid: {0}, recordValueGuid: {1}.", previousValueGuid, recordValueGuid );

          //
          // Empty fields are skipped.
          //
          if ( recordValueGuid == Guid.Empty )
          {
            this.LogDebug ( "Skip the value Guid is empty." );
            continue;
          }

          // If the field guid has changed then it is a new field.
          // So add the previous field then get the data for the new field.
          //
          if ( previousValueGuid != recordValueGuid )
          {
            this.LogDebug ( "Change of recordValueGuid." );
            //
            // Add the last field to the list.
            //
            if ( recordField.Guid != Guid.Empty )
            {
              this.LogDebug ( "Add field to record field list." );
              recordFieldList.Add ( recordField );
            }

            // 
            // Get the object data from the row.
            // 
            recordField = this.getRowData ( row );

            //
            // Update the lst field guid to enable the other field values to be collected.
            //
            previousValueGuid = recordField.Guid;

          }//END create new field object.

          this.LogDebug ( "Read in value data." );

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
            case Evado.Model.EvDataTypes.Numeric:
              {
                recordField.ItemValue = EvSqlMethods.getString ( row, EdRecordValues.DB_VALUES_NUMERIC );
                this.LogDebug ( "recordField.ItemValue: {0}.", recordField.ItemValue );
                break;
              }
            case Evado.Model.EvDataTypes.Boolean:
            case Evado.Model.EvDataTypes.Yes_No:
              {
                bool bValue = EvSqlMethods.getBool ( row, EdRecordValues.DB_VALUES_NUMERIC);
                this.LogDebug ( "bValue: {0}.", bValue );
                recordField.ItemValue = "No";
                if ( bValue == true )
                {
                  recordField.ItemValue = "Yes";
                }

                this.LogDebug ( "recordField.ItemValue: {0} bool.", recordField.ItemValue );
                break;
              }
            case Evado.Model.EvDataTypes.Date:
              {
                var dtValue = EvSqlMethods.getDateTime ( row, EdRecordValues.DB_VALUES_DATE );
                recordField.ItemValue = EvStatics.getDateAsString ( dtValue );
                this.LogDebug ( "recordField.ItemValue: {0}.", recordField.ItemValue );
                break;
              }
            case Evado.Model.EvDataTypes.Free_Text:
              {
                recordField.ItemText = EvSqlMethods.getString ( row, EdRecordValues.DB_VALUES_TEXT );
                this.LogDebug ( "recordField.ItemValue: {0}.", recordField.ItemText );
                break;
              }
            default:
              {
                if ( recordField.isReadOnly == true )
                {
                  break;
                }
                recordField.ItemValue = EvSqlMethods.getString ( row, EdRecordValues.DB_VALUES_STRING );
                this.LogDebug ( "recordField.ItemValue: {0}.", recordField.ItemValue );
                break;
              }
          }


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

    #region EvRecordField Update queries

    //
    // Store the record state to control when to output record values.
    //
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
      List<SqlParameter> ParmList = new List<SqlParameter> ( );
      StringBuilder SqlUpdateStatement = new StringBuilder ( );

      //
      // Define the record Guid value for the update queies
      //
      SqlParameter prm = new SqlParameter ( EdRecordValues.PARM_RECORD_GUID, SqlDbType.UniqueIdentifier );
      prm.Value = FormRecord.Guid;
      ParmList.Add ( prm );

      //
      // Delete the sections
      //
      SqlUpdateStatement.AppendLine ( "DELETE FROM ED_RECORD_VALUES "
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
        this.LogDebug ( "field.FormFieldGuid: " + field.FormFieldGuid );
        this.LogDebug ( "field.Guid: " + field.Guid );

        //
        // Create the list of update queries and parameters.
        //
        this.GenerateUpdateQueryStatements (
           SqlUpdateStatement,
           ParmList,
           field );

        this._ValueCount++;

      }//END FormField Update Iteration.

      this.LogDebug ( SqlUpdateStatement.ToString ( ) );

      //
      // Convert the list to an array of SqlPararmeters.
      //
      SqlParameter [ ] parms = new SqlParameter [ ParmList.Count ];

      for ( int i = 0; i < ParmList.Count; i++ )
      {
        parms [ i ] = ParmList [ i ];
      }

      this.LogDebug ( EvSqlMethods.getParameterSqlText ( parms ) );

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


    }//END UpdateFields method 

    // =====================================================================================
    /// <summary>
    /// This class update fields on formfield table using formfield object. 
    /// </summary>
    /// <param name="SqlUpdateStatement">StringBuilder: containing the SQL update statemenet</param>
    /// <param name="ParmList">list of SqlParameter objects</param>
    /// <param name="RecordField">EvFormField: a formfield data object</param>
    /// <returns>EvEventCodes: an event code for updating fields</returns>
    // -------------------------------------------------------------------------------------
    private void GenerateUpdateQueryStatements (
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
            this.updateSingleValueField (
              SqlUpdateStatement,
              ParmList,
              RecordField,
              "", 0 );
            break;
          }
      }

    }//END updateField method

    // =====================================================================================
    /// <summary>
    /// This class update fields on formfield table using formfield object. 
    /// </summary>
    /// <param name="SqlUpdateStatement">StringBuilder: containing the SQL update statemenet</param>
    /// <param name="ParmList">list of SqlParameter objects</param>
    /// <param name="RecordField">EvFormField: a formfield data object</param>
    /// <param name="ColumnId">String: column identifier</param>
    /// <param name="Row">Row: row index</param>
    // -------------------------------------------------------------------------------------
    private void updateSingleValueField (
      StringBuilder SqlUpdateStatement,
      List<SqlParameter> ParmList,
      EdRecordField RecordField,
      String ColumnId,
      int Row )
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
      // Define the record column identifier
      // 
      prm = new SqlParameter ( EdRecordValues.PARM_VALUE_COLUMN_ID + "_" + this._ValueCount, SqlDbType.NVarChar, 10 );
      prm.Value = ColumnId;
      ParmList.Add ( prm );

      // 
      // Define the record field Guid
      // 
      prm = new SqlParameter ( EdRecordValues.PARM_VALUE_ROW + "_" + this._ValueCount, SqlDbType.SmallInt );
      prm.Value = Row;
      ParmList.Add ( prm );


      switch ( RecordField.TypeId )
      {
        case EvDataTypes.Yes_No:
        case EvDataTypes.Boolean:
          {
            string value = "0";
            bool bValue = EvStatics.getBool ( RecordField.ItemValue );
            if ( bValue == true )
            {
              value = "1";
            }

            prm = new SqlParameter ( EdRecordValues.PARM_VALUE_NUMERIC + "_" + this._ValueCount, SqlDbType.Float );
            prm.Value = value;
            ParmList.Add ( prm );
            //
            // Create the add query .
            //
            SqlUpdateStatement.AppendLine ( " INSERT INTO ED_RECORD_VALUES  "
            + "(" + EdRecordValues.DB_RECORD_GUID
            + ", " + EdRecordValues.DB_FIELD_GUID
            + ", " + EdRecordValues.DB_VALUES_GUID
            + ", " + EdRecordValues.DB_VALUES_COLUMN_ID
            + ", " + EdRecordValues.DB_VALUES_ROW
            + ", " + EdRecordValues.DB_VALUES_NUMERIC
            + "  ) " );
            SqlUpdateStatement.AppendLine ( "VALUES (" );
            SqlUpdateStatement.AppendLine (
              " " + EdRecordValues.PARM_RECORD_GUID
             + ", " + EdRecordValues.PARM_FIELD_GUID + "_" + this._ValueCount
             + ", " + EdRecordValues.PARM_VALUE_GUID + "_" + this._ValueCount
             + ", " + EdRecordValues.PARM_VALUE_COLUMN_ID + "_" + this._ValueCount
             + ", " + EdRecordValues.PARM_VALUE_ROW + "_" + this._ValueCount
             + ", " + EdRecordValues.PARM_VALUE_NUMERIC + "_" + +this._ValueCount + " );\r\n" );
            break;
          }
        case EvDataTypes.Numeric:
          {
            prm = new SqlParameter ( EdRecordValues.PARM_VALUE_NUMERIC + "_" + this._ValueCount, SqlDbType.Float );
            prm.Value = RecordField.ItemValue;
            ParmList.Add ( prm );
            //
            // Create the add query .
            //
            SqlUpdateStatement.AppendLine ( " INSERT INTO ED_RECORD_VALUES  "
            + "(" + EdRecordValues.DB_RECORD_GUID
            + ", " + EdRecordValues.DB_FIELD_GUID
            + ", " + EdRecordValues.DB_VALUES_GUID
            + ", " + EdRecordValues.DB_VALUES_COLUMN_ID
            + ", " + EdRecordValues.DB_VALUES_ROW
            + ", " + EdRecordValues.DB_VALUES_NUMERIC
            + "  ) " );
            SqlUpdateStatement.AppendLine ( "VALUES (" );
            SqlUpdateStatement.AppendLine (
              " " + EdRecordValues.PARM_RECORD_GUID
             + ", " + EdRecordValues.PARM_FIELD_GUID + "_" + this._ValueCount
             + ", " + EdRecordValues.PARM_VALUE_GUID + "_" + this._ValueCount
             + ", " + EdRecordValues.PARM_VALUE_COLUMN_ID + "_" + this._ValueCount
             + ", " + EdRecordValues.PARM_VALUE_ROW + "_" + this._ValueCount
             + ", " + EdRecordValues.PARM_VALUE_NUMERIC + "_" + +this._ValueCount + " );\r\n" );
            break;
          }
        case EvDataTypes.Date:
          {
            if ( RecordField.ItemValue == String.Empty )
            {
              RecordField.ItemValue = EvStatics.CONST_DATE_NULL.ToString ( "dd-MMM-yyyy" );
            }

            prm = new SqlParameter ( EdRecordValues.PARM_VALUE_DATE + "_" + this._ValueCount, SqlDbType.DateTime );
            prm.Value = RecordField.ItemValue;
            ParmList.Add ( prm );
            //
            // Create the add query .
            //
            SqlUpdateStatement.AppendLine ( " INSERT INTO ED_RECORD_VALUES  "
            + "(" + EdRecordValues.DB_RECORD_GUID
            + ", " + EdRecordValues.DB_FIELD_GUID
            + ", " + EdRecordValues.DB_VALUES_GUID
            + ", " + EdRecordValues.DB_VALUES_COLUMN_ID
            + ", " + EdRecordValues.DB_VALUES_ROW
            + ", " + EdRecordValues.DB_VALUES_DATE
            + "  ) " );
            SqlUpdateStatement.AppendLine ( "VALUES (" );
            SqlUpdateStatement.AppendLine (
              " " + EdRecordValues.PARM_RECORD_GUID
             + ", " + EdRecordValues.PARM_FIELD_GUID + "_" + this._ValueCount
             + ", " + EdRecordValues.PARM_VALUE_GUID + "_" + this._ValueCount
             + ", " + EdRecordValues.PARM_VALUE_COLUMN_ID + "_" + this._ValueCount
             + ", " + EdRecordValues.PARM_VALUE_ROW + "_" + this._ValueCount
             + ", " + EdRecordValues.PARM_VALUE_DATE + "_" + this._ValueCount + " );\r\n" );

            break;
          }
        case EvDataTypes.Free_Text:
          {
            prm = new SqlParameter ( EdRecordValues.PARM_VALUE_TEXT + "_" + this._ValueCount, SqlDbType.NText );
            prm.Value = RecordField.ItemText;
            ParmList.Add ( prm );
            //
            // Create the add query .
            //
            SqlUpdateStatement.AppendLine ( " INSERT INTO ED_RECORD_VALUES  "
            + "(" + EdRecordValues.DB_RECORD_GUID
            + ", " + EdRecordValues.DB_FIELD_GUID
            + ", " + EdRecordValues.DB_VALUES_GUID
            + ", " + EdRecordValues.DB_VALUES_COLUMN_ID
            + ", " + EdRecordValues.DB_VALUES_ROW
            + ", " + EdRecordValues.DB_VALUES_TEXT
            + "  ) " );
            SqlUpdateStatement.AppendLine ( "VALUES (" );
            SqlUpdateStatement.AppendLine (
              " " + EdRecordValues.PARM_RECORD_GUID
             + ", " + EdRecordValues.PARM_FIELD_GUID + "_" + this._ValueCount
             + ", " + EdRecordValues.PARM_VALUE_GUID + "_" + this._ValueCount
             + ", " + EdRecordValues.PARM_VALUE_COLUMN_ID + "_" + this._ValueCount
             + ", " + EdRecordValues.PARM_VALUE_ROW + "_" + this._ValueCount
             + ", " + EdRecordValues.PARM_VALUE_TEXT + "_" + this._ValueCount + " );\r\n" );

            break;
          }
        default:
          {
            prm = new SqlParameter ( EdRecordValues.PARM_VALUE_STRING + "_" + this._ValueCount, SqlDbType.NVarChar, 100 );
            prm.Value = RecordField.ItemValue;
            ParmList.Add ( prm );
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
            + "  ) " );
            SqlUpdateStatement.AppendLine ( "VALUES (" );
            SqlUpdateStatement.AppendLine (
              " " + EdRecordValues.PARM_RECORD_GUID
             + ", " + EdRecordValues.PARM_FIELD_GUID + "_" + this._ValueCount
             + ", " + EdRecordValues.PARM_VALUE_GUID + "_" + this._ValueCount
             + ", " + EdRecordValues.PARM_VALUE_COLUMN_ID + "_" + this._ValueCount
             + ", " + EdRecordValues.PARM_VALUE_ROW + "_" + this._ValueCount
             + ", " + EdRecordValues.PARM_VALUE_STRING + "_" + this._ValueCount + " );\r\n" );
            break;
          }
      }//End switch statement

    }//END method.

    #endregion

  }//END EvFormRecordFields class

}//END namespace Evado.Dal.Clinical
