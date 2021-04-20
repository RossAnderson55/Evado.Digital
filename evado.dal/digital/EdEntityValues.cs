/***************************************************************************************
 * <copyright file="dal\EvRecordFields.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2021 EVADO HOLDING PTY. LTD..  All rights reserved.
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


namespace Evado.Dal.Digital
{
  /// <summary>
  /// This class is handles the data access layer for the form record field data object.
  /// </summary>
  public class EdEntityValues : EvDalBase
  {
    #region class initialisation method.
    /// <summary>
    /// This method initialises the schedule DAL class.
    /// </summary>
    public EdEntityValues ( )
    {
      this.ClassNameSpace = "Evado.Dal.Digital.EdEntityValues.";
    }

    /// <summary>
    /// This method initialises the schedule DAL class.
    /// </summary>
    public EdEntityValues ( EvClassParameters Settings )
    {
      this.ClassParameters = Settings;
      this.ClassNameSpace = "Evado.Dal.Digital.EdEntityValues.";

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
    private const string SQL_QUERY_VIEW = "Select *  FROM ED_ENTITY_VALUE_VIEW ";


    #region Define the query parameter constants.

    // SQL database column names
    private const string DB_ENTITY_GUID = "EDE_GUID";
    private const string DB_VALUES_GUID = "EDEV_GUID";
    private const string DB_FIELD_GUID = "EDELF_GUID";

    private const string DB_VALUES_COLUMN_ID = "EDEV_COLUMN_ID";
    private const string DB_VALUES_ROW = "EDEV_ROW";
    private const string DB_VALUES_STRING = "EDEV_STRING";
    private const string DB_VALUES_NUMERIC = "EDEV_NUMERIC";
    private const string DB_VALUES_DATE = "EDEV_DATE";
    private const string DB_VALUES_TEXT = "EDEV_TEXT";

    // SQL parameter strings.
    private const string PARM_ENTITY_GUID = "@ENTITY_GUID";
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
      EdRecordField entityField = new EdRecordField ( );

      // 
      // Fill the evForm object.
      //
      entityField.Guid = EvSqlMethods.getGuid ( Row, EdEntityValues.DB_VALUES_GUID );
      entityField.RecordGuid = EvSqlMethods.getGuid ( Row, EdEntities.DB_ENTITY_GUID );
      entityField.LayoutGuid = EvSqlMethods.getGuid ( Row, EdEntityLayouts.DB_LAYOUT_GUID );
      entityField.FieldGuid = EvSqlMethods.getGuid ( Row, EdEntityValues.DB_FIELD_GUID );


      entityField.FieldId = EvSqlMethods.getString ( Row, EdEntityFields.DB_FIELD_ID );
      String value = EvSqlMethods.getString ( Row, EdEntityFields.DB_TYPE_ID );
      entityField.Design.TypeId = Evado.Model.EvStatics.parseEnumValue<Evado.Model.EvDataTypes> ( value );

      entityField.Design.Title = EvSqlMethods.getString ( Row, EdEntityFields.DB_TITLE );
      entityField.Design.Instructions = EvSqlMethods.getString ( Row, EdEntityFields.DB_INSTRUCTIONS );
      entityField.Design.HttpReference = EvSqlMethods.getString ( Row, EdEntityFields.DB_HTTP_REFERENCE );
      entityField.Design.SectionNo = EvSqlMethods.getInteger ( Row, EdEntityFields.DB_SECTION_ID );
      entityField.Design.Options = EvSqlMethods.getString ( Row, EdEntityFields.DB_OPTIONS );
      entityField.Design.IsSummaryField = EvSqlMethods.getBool ( Row, EdEntityFields.DB_SUMMARY_FIELD );
      entityField.Design.Mandatory = EvSqlMethods.getBool ( Row, EdEntityFields.DB_MANDATORY );
      entityField.Design.AiDataPoint = EvSqlMethods.getBool ( Row, EdEntityFields.DB_AI_DATA_POINT );
      entityField.Design.HideField = EvSqlMethods.getBool ( Row, EdEntityFields.DB_HIDDEN );
      entityField.Design.ExSelectionListId = EvSqlMethods.getString ( Row, EdEntityFields.DB_EX_SELECTION_LIST_ID );
      entityField.Design.ExSelectionListCategory = EvSqlMethods.getString ( Row, EdEntityFields.DB_EX_SELECTION_LIST_CATEGORY );
      entityField.Design.DefaultValue = EvSqlMethods.getString ( Row, EdEntityFields.DB_DEFAULT_VALUE );
      entityField.Design.Unit = EvSqlMethods.getString ( Row, EdEntityFields.DB_UNIT );
      entityField.Design.UnitScaling = EvSqlMethods.getString ( Row, EdEntityFields.DB_UNIT_SCALING );

      entityField.Design.ValidationLowerLimit = EvSqlMethods.getFloat ( Row, EdEntityFields.DB_VALIDATION_LOWER_LIMIT );
      entityField.Design.ValidationUpperLimit = EvSqlMethods.getFloat ( Row, EdEntityFields.DB_VALIDATION_UPPER_LIMIT );
      entityField.Design.AlertLowerLimit = EvSqlMethods.getFloat ( Row, EdEntityFields.DB_ALERT_LOWER_LIMIT );
      entityField.Design.AlertUpperLimit = EvSqlMethods.getFloat ( Row, EdEntityFields.DB_ALERT_UPPER_LIMIT );
      entityField.Design.NormalRangeLowerLimit = EvSqlMethods.getFloat ( Row, EdEntityFields.DB_NORMAL_LOWER_LIMITD );
      entityField.Design.NormalRangeUpperLimit = EvSqlMethods.getFloat ( Row, EdEntityFields.DB_NORMAL_UPPER_LIMIT );

      entityField.Design.FieldCategory = EvSqlMethods.getString ( Row, EdEntityFields.DB_FIELD_CATEGORY );
      entityField.Design.AnalogueLegendStart = EvSqlMethods.getString ( Row, EdEntityFields.DB_ANALOGUE_LEGEND_START );
      entityField.Design.AnalogueLegendFinish = EvSqlMethods.getString ( Row, EdEntityFields.DB_ANALOGUE_LEGEND_FINISH );
      entityField.Design.JavaScript = EvSqlMethods.getString ( Row, EdEntityFields.DB_JAVA_SCRIPT );
      entityField.Design.InitialOptionList = EvSqlMethods.getString ( Row, EdEntityFields.DB_INITIAL_OPTION_LIST );
      entityField.Design.InitialVersion = EvSqlMethods.getInteger ( Row, EdEntityFields.DB_INITIAL_VERSION );
      entityField.Design.FieldLayout = EvSqlMethods.getString ( Row, EdEntityFields.DB_FIELD_LAYOUT );
      entityField.Design.FieldWidth = EvSqlMethods.getInteger ( Row, EdEntityFields.DB_FIELD_WIDTH );
      entityField.Design.FieldHeight = EvSqlMethods.getInteger ( Row, EdEntityFields.DB_FIELD_HEIGHT );

      if ( entityField.Design.FieldLayout == null )
      {
        entityField.Design.FieldLayout = String.Empty;
      }

      if ( entityField.Design.FieldWidth < 5 )
      {
        entityField.Design.FieldWidth = 50;
      }
      if ( entityField.Design.FieldHeight < 2 )
      {
        entityField.Design.FieldHeight = 5;
      }

      if ( entityField.TypeId == EvDataTypes.External_Image
        || entityField.TypeId == EvDataTypes.Streamed_Video
        || entityField.TypeId == EvDataTypes.Image)
      {
        entityField.RecordMedia = new EdRecordMedia ( );

        entityField.RecordMedia.Data = entityField.Design.JavaScript;
      }


      //
      // if the field is a signature then decrypt the field.
      //
      if ( entityField.TypeId == EvDataTypes.Signature )
      {
        this.LogDebug ( "Encrypted Signature string" );
        EvEncrypt encrypt = new EvEncrypt ( this.ClassParameters.AdapterGuid, entityField.Guid );
        encrypt.ClassParameters = this.ClassParameters;

        value = encrypt.decryptString ( entityField.ItemText );
        this.LogDebug ( "clear string: " + value );
        entityField.ItemText = value;

        this.LogClass ( encrypt.Log );
      }

      //
      // Get the table or matric object.
      //
      this.processTableRowObject ( Row, entityField );

      //
      // Process the NA values in selectionlists.
      //
      this.processNotAvailableValues ( Row, entityField );

      //
      // Return the formfield object. 
      //
      this.LogMethodEnd ( "getRowData" );
      return entityField;

    }//END getRowData method.

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
        Field.ItemText = EvSqlMethods.getString ( Row, EdEntityFields.DB_TABLE );
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
    /// <param name="Entity">EvForm: (Mandatory) The record object.</param>
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
    public List<EdRecordField> GetlEntityValues (
      EdRecord Entity )
    {
      this.LogMethod ( "GetlEntityValues" );
      this.LogDebug ( "Entity.Guid: " + Entity.Guid );
      //
      // Initialise the methods variables and objects.
      //
      List<EdRecordField> recordFieldList = new List<EdRecordField> ( );
      EdRecordField recordField = new EdRecordField ( );
      Guid previousValueGuid = Guid.Empty;
      bool firstTextFound = false;

      // 
      // Validate whether the record Guid is not empty. 
      // 
      if ( Entity.Guid == Guid.Empty )
      {
        this.LogMethodEnd ( "GetlEntityValues" );
        return recordFieldList;
      }

      // 
      // Define the SQL query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(PARM_ENTITY_GUID, SqlDbType.UniqueIdentifier),
      };
      cmdParms [ 0 ].Value = Entity.Guid;

      // 
      // Define the query string.
      // 
      _Sql_QueryString = SQL_QUERY_VIEW + " WHERE ( " + EdEntityValues.DB_ENTITY_GUID + " =" + EdEntityValues.PARM_ENTITY_GUID + ") "
        + "ORDER BY " + EdEntityFields.DB_ORDER + "; ";

      this.LogDebug ( _Sql_QueryString );

      // 
      // Execute the query against the database.
      // 
      using ( DataTable table = EvSqlMethods.RunQuery ( _Sql_QueryString, cmdParms ) )
      {
        if ( table.Rows.Count == 0 )
        {
          this.LogDebug ( "Not returned values " );
          this.LogMethodEnd ( "GetlEntityValues" );
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

          Guid recordValueGuid = EvSqlMethods.getGuid ( row, EdEntityValues.DB_VALUES_GUID );

          //
          // Empty fields are skipped.
          //
          if ( recordValueGuid == Guid.Empty )
          {
            this.LogDebug ( "Skip the value Guid is empty." );
            continue;
          }

          this.LogDebug ( "previousValueGuid: {0}, recordValueGuid: {1}.", previousValueGuid, recordValueGuid );

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
            // skip all non summary field if summary fields is selected.
            //
            if ( Entity.SelectOnlySummaryFields == true
              && recordField.Design.IsSummaryField == false )
            {
              this.LogDebug ( "{0} is NOT a summary field so SKIPPED.", recordField.FieldId );
              continue;
            }

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
                recordField.ItemValue = EvSqlMethods.getString ( row, EdEntityValues.DB_VALUES_NUMERIC );
                this.LogDebug ( "recordField.ItemValue: {0}.", recordField.ItemValue );
                break;
              }
            case Evado.Model.EvDataTypes.Boolean:
            case Evado.Model.EvDataTypes.Yes_No:
              {
                bool bValue = EvSqlMethods.getBool ( row, EdEntityValues.DB_VALUES_NUMERIC );
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
                var dtValue = EvSqlMethods.getDateTime ( row, EdEntityValues.DB_VALUES_DATE );
                recordField.ItemValue = EvStatics.getDateAsString ( dtValue );
                this.LogDebug ( "recordField.ItemValue: {0}.", recordField.ItemValue );
                break;
              }
            case Evado.Model.EvDataTypes.Free_Text:
              {
                recordField.ItemText = EvSqlMethods.getString ( row, EdEntityValues.DB_VALUES_TEXT );
                this.LogDebug ( "recordField.ItemValue: {0}.", recordField.ItemText );
                break;
              }
            default:
              {
                if ( recordField.isReadOnly == true )
                {
                  break;
                }
                recordField.ItemValue = EvSqlMethods.getString ( row, EdEntityValues.DB_VALUES_STRING );
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
      this.LogDebug ( "recordFieldList.Count {0}. ", recordFieldList.Count );
      this.LogMethodEnd ( "GetlEntityValues" );
      return recordFieldList;

    }//END GetlEntityValues method.

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
      String columnId = EvSqlMethods.getString ( Row, EdEntityValues.DB_VALUES_COLUMN_ID );
      bool value = EvSqlMethods.getBool ( Row, EdEntityValues.DB_VALUES_NUMERIC );

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
      int row = EvSqlMethods.getInteger ( Row, EdEntityValues.DB_VALUES_ROW );
      String columnId = EvSqlMethods.getString ( Row, EdEntityValues.DB_VALUES_COLUMN_ID );

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
            float fltValue = EvSqlMethods.getFloat ( Row, EdEntityValues.DB_VALUES_NUMERIC );
            return fltValue.ToString ( );
          }
        case EvDataTypes.Yes_No:
          {
            bool bYesNo = EvSqlMethods.getBool ( Row, EdEntityValues.DB_VALUES_NUMERIC );
            string value = "No";
            if ( bYesNo == true )
            {
              value = "Yes";
            }
            return value;
          }
        case EvDataTypes.Date:
          {
            DateTime dtValue = EvSqlMethods.getDateTime ( Row, EdEntityValues.DB_VALUES_DATE );

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
            return EvSqlMethods.getString ( Row, EdEntityValues.DB_VALUES_STRING );
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
    /// <param name="Entity">EvForm object</param>
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
      EdRecord Entity )
    {
      this.LogMethod ( "UpdateFields method " );
      this.LogDebug ( "RecordFieldList.Count: " + Entity.Fields.Count );
      // 
      // Initialize the method debug log and the return event code. 
      // 
      List<SqlParameter> ParmList = new List<SqlParameter> ( );
      StringBuilder SqlUpdateStatement = new StringBuilder ( );

      //
      // Define the record Guid value for the update queies
      //
      SqlParameter prm = new SqlParameter ( EdEntityValues.PARM_ENTITY_GUID, SqlDbType.UniqueIdentifier );
      prm.Value = Entity.Guid;
      ParmList.Add ( prm );

      //
      // Delete the sections
      //
      SqlUpdateStatement.AppendLine ( "DELETE FROM ED_ENTITY_VALUES "
      + "WHERE " + EdEntityValues.DB_ENTITY_GUID + "= " + EdEntityValues.PARM_ENTITY_GUID + "; " );

      // 
      // Iterate through the formfields object. 
      // 
      foreach ( EdRecordField field in Entity.Fields )
      {
        if ( field == null )
        {
          this.LogDebug ( "FIELD NULL" );
          continue;
        }

        //
        // If the field guid is empty create a new one.
        //
        if ( field.Guid == Guid.Empty )
        {
          field.Guid = Guid.NewGuid ( );
        }
        //this.LogDebug ( "field.FormFieldGuid: {0} field.Guid: {1} fieldid: {2}", field.FieldGuid, field.Guid, field.FieldId );

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
        int result = EvSqlMethods.QueryUpdate ( SqlUpdateStatement.ToString ( ), parms );

        if ( result == 0 )
        {
          return EvEventCodes.Database_Record_Update_Error;
        }

        this.LogDebug ( "result: " + result.ToString() );
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
      this.LogMethod ( "GenerateUpdateQueryStatements" );

      //
      // If readonly field exit method.
      //
      if ( RecordField.isReadOnly == true )
      {
        return;
      }

      //
      // Swithc the select the field storage structure
      switch ( RecordField.TypeId )
      {
        case EvDataTypes.Check_Box_List:
          {
            this.updateCheckBoxValueField (
               SqlUpdateStatement,
               ParmList,
               RecordField );
            break;
          }
        case EvDataTypes.Table:
        case EvDataTypes.Special_Matrix:
          {
            break;
          }
        default:
          {
            this.updateSingleValueField (
              SqlUpdateStatement,
              ParmList,
              RecordField );
            break;
          }
      }

      this.LogMethodEnd ( "GenerateUpdateQueryStatements" );
    }//END updateField method

    // =====================================================================================
    /// <summary>
    /// This class update fields on formfield table using formfield object. 
    /// </summary>
    /// <param name="SqlUpdateStatement">StringBuilder: containing the SQL update statemenet</param>
    /// <param name="ParmList">list of SqlParameter objects</param>
    /// <param name="RecordField">EvFormField: a formfield data object</param>
    // -------------------------------------------------------------------------------------
    private void updateSingleValueField (
      StringBuilder SqlUpdateStatement,
      List<SqlParameter> ParmList,
      EdRecordField RecordField )
    {
      this.LogMethod ( "updateSingleValueField " );
      this.LogDebug ( "ValueCount: " + _ValueCount );

      // 
      // Define the record field Guid
      // 
      SqlParameter prm = new SqlParameter ( EdEntityValues.PARM_FIELD_GUID + "_" + this._ValueCount, SqlDbType.UniqueIdentifier );
      prm.Value = RecordField.FieldGuid;
      ParmList.Add ( prm );

      // 
      // Define the record field Guid
      // 
      prm = new SqlParameter ( EdEntityValues.PARM_VALUE_GUID + "_" + this._ValueCount, SqlDbType.UniqueIdentifier );
      prm.Value = RecordField.Guid;
      ParmList.Add ( prm );
      // 
      // Define the record column identifier
      // 
      prm = new SqlParameter ( EdEntityValues.PARM_VALUE_COLUMN_ID + "_" + this._ValueCount, SqlDbType.NVarChar, 10 );
      prm.Value = String.Empty;
      ParmList.Add ( prm );

      // 
      // Define the record field Guid
      // 
      prm = new SqlParameter ( EdEntityValues.PARM_VALUE_ROW + "_" + this._ValueCount, SqlDbType.SmallInt );
      prm.Value = 0;
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

            prm = new SqlParameter ( EdEntityValues.PARM_VALUE_NUMERIC + "_" + this._ValueCount, SqlDbType.Float );
            prm.Value = value;
            ParmList.Add ( prm );
            //
            // Create the add query .
            //
            SqlUpdateStatement.AppendLine ( " INSERT INTO ED_ENTITY_VALUES  "
            + "(" + EdEntityValues.DB_ENTITY_GUID
            + ", " + EdEntityValues.DB_FIELD_GUID
            + ", " + EdEntityValues.DB_VALUES_GUID
            + ", " + EdEntityValues.DB_VALUES_COLUMN_ID
            + ", " + EdEntityValues.DB_VALUES_ROW
            + ", " + EdEntityValues.DB_VALUES_NUMERIC
            + "  ) " );
            SqlUpdateStatement.AppendLine ( "VALUES (" );
            SqlUpdateStatement.AppendLine (
              " " + EdEntityValues.PARM_ENTITY_GUID
             + ", " + EdEntityValues.PARM_FIELD_GUID + "_" + this._ValueCount
             + ", " + EdEntityValues.PARM_VALUE_GUID + "_" + this._ValueCount
             + ", " + EdEntityValues.PARM_VALUE_COLUMN_ID + "_" + this._ValueCount
             + ", " + EdEntityValues.PARM_VALUE_ROW + "_" + this._ValueCount
             + ", " + EdEntityValues.PARM_VALUE_NUMERIC + "_" + +this._ValueCount + " );\r\n" );
            break;
          }
        case EvDataTypes.Numeric:
          {
            prm = new SqlParameter ( EdEntityValues.PARM_VALUE_NUMERIC + "_" + this._ValueCount, SqlDbType.Float );
            prm.Value = EvStatics.getFloat( RecordField.ItemValue );
            ParmList.Add ( prm );
            //
            // Create the add query .
            //
            SqlUpdateStatement.AppendLine ( " INSERT INTO ED_ENTITY_VALUES  "
            + "(" + EdEntityValues.DB_ENTITY_GUID
            + ", " + EdEntityValues.DB_FIELD_GUID
            + ", " + EdEntityValues.DB_VALUES_GUID
            + ", " + EdEntityValues.DB_VALUES_COLUMN_ID
            + ", " + EdEntityValues.DB_VALUES_ROW
            + ", " + EdEntityValues.DB_VALUES_NUMERIC
            + "  ) " );
            SqlUpdateStatement.AppendLine ( "VALUES (" );
            SqlUpdateStatement.AppendLine (
              " " + EdEntityValues.PARM_ENTITY_GUID
             + ", " + EdEntityValues.PARM_FIELD_GUID + "_" + this._ValueCount
             + ", " + EdEntityValues.PARM_VALUE_GUID + "_" + this._ValueCount
             + ", " + EdEntityValues.PARM_VALUE_COLUMN_ID + "_" + this._ValueCount
             + ", " + EdEntityValues.PARM_VALUE_ROW + "_" + this._ValueCount
             + ", " + EdEntityValues.PARM_VALUE_NUMERIC + "_" + +this._ValueCount + " );\r\n" );
            break;
          }
        case EvDataTypes.Date:
          {
            if ( RecordField.ItemValue == String.Empty )
            {
              RecordField.ItemValue = EvStatics.CONST_DATE_NULL.ToString ( "dd-MMM-yyyy" );
            }

            prm = new SqlParameter ( EdEntityValues.PARM_VALUE_DATE + "_" + this._ValueCount, SqlDbType.DateTime );
            prm.Value = RecordField.ItemValue;
            ParmList.Add ( prm );
            //
            // Create the add query .
            //
            SqlUpdateStatement.AppendLine ( " INSERT INTO ED_ENTITY_VALUES  "
            + "(" + EdEntityValues.DB_ENTITY_GUID
            + ", " + EdEntityValues.DB_FIELD_GUID
            + ", " + EdEntityValues.DB_VALUES_GUID
            + ", " + EdEntityValues.DB_VALUES_COLUMN_ID
            + ", " + EdEntityValues.DB_VALUES_ROW
            + ", " + EdEntityValues.DB_VALUES_DATE
            + "  ) " );
            SqlUpdateStatement.AppendLine ( "VALUES (" );
            SqlUpdateStatement.AppendLine (
              " " + EdEntityValues.PARM_ENTITY_GUID
             + ", " + EdEntityValues.PARM_FIELD_GUID + "_" + this._ValueCount
             + ", " + EdEntityValues.PARM_VALUE_GUID + "_" + this._ValueCount
             + ", " + EdEntityValues.PARM_VALUE_COLUMN_ID + "_" + this._ValueCount
             + ", " + EdEntityValues.PARM_VALUE_ROW + "_" + this._ValueCount
             + ", " + EdEntityValues.PARM_VALUE_DATE + "_" + this._ValueCount + " );\r\n" );

            break;
          }
        case EvDataTypes.Free_Text:
          {
            prm = new SqlParameter ( EdEntityValues.PARM_VALUE_TEXT + "_" + this._ValueCount, SqlDbType.NText );
            prm.Value = RecordField.ItemText;
            ParmList.Add ( prm );
            //
            // Create the add query .
            //
            SqlUpdateStatement.AppendLine ( " INSERT INTO ED_ENTITY_VALUES  "
            + "(" + EdEntityValues.DB_ENTITY_GUID
            + ", " + EdEntityValues.DB_FIELD_GUID
            + ", " + EdEntityValues.DB_VALUES_GUID
            + ", " + EdEntityValues.DB_VALUES_COLUMN_ID
            + ", " + EdEntityValues.DB_VALUES_ROW
            + ", " + EdEntityValues.DB_VALUES_TEXT
            + "  ) " );
            SqlUpdateStatement.AppendLine ( "VALUES (" );
            SqlUpdateStatement.AppendLine (
              " " + EdEntityValues.PARM_ENTITY_GUID
             + ", " + EdEntityValues.PARM_FIELD_GUID + "_" + this._ValueCount
             + ", " + EdEntityValues.PARM_VALUE_GUID + "_" + this._ValueCount
             + ", " + EdEntityValues.PARM_VALUE_COLUMN_ID + "_" + this._ValueCount
             + ", " + EdEntityValues.PARM_VALUE_ROW + "_" + this._ValueCount
             + ", " + EdEntityValues.PARM_VALUE_TEXT + "_" + this._ValueCount + " );\r\n" );

            break;
          }
        default:
          {
            prm = new SqlParameter ( EdEntityValues.PARM_VALUE_STRING + "_" + this._ValueCount, SqlDbType.NVarChar, 100 );
            prm.Value = RecordField.ItemValue;
            ParmList.Add ( prm );
            //
            // Create the add query .
            //
            SqlUpdateStatement.AppendLine ( " INSERT INTO ED_ENTITY_VALUES  "
            + "(" + EdEntityValues.DB_ENTITY_GUID
            + ", " + EdEntityValues.DB_FIELD_GUID
            + ", " + EdEntityValues.DB_VALUES_GUID
            + ", " + EdEntityValues.DB_VALUES_COLUMN_ID
            + ", " + EdEntityValues.DB_VALUES_ROW
            + ", " + EdEntityValues.DB_VALUES_STRING
            + "  ) " );
            SqlUpdateStatement.AppendLine ( "VALUES (" );
            SqlUpdateStatement.AppendLine (
              " " + EdEntityValues.PARM_ENTITY_GUID
             + ", " + EdEntityValues.PARM_FIELD_GUID + "_" + this._ValueCount
             + ", " + EdEntityValues.PARM_VALUE_GUID + "_" + this._ValueCount
             + ", " + EdEntityValues.PARM_VALUE_COLUMN_ID + "_" + this._ValueCount
             + ", " + EdEntityValues.PARM_VALUE_ROW + "_" + this._ValueCount
             + ", " + EdEntityValues.PARM_VALUE_STRING + "_" + this._ValueCount + " );\r\n" );
            break;
          }
      }//End switch statement

    }//END method.

    // =====================================================================================
    /// <summary>
    /// This class update fields on formfield table using formfield object. 
    /// </summary>
    /// <param name="SqlUpdateStatement">StringBuilder: containing the SQL update statemenet</param>
    /// <param name="ParmList">list of SqlParameter objects</param>
    /// <param name="EntityField">EvFormField: a formfield data object</param>
    // -------------------------------------------------------------------------------------
    private void updateCheckBoxValueField (
      StringBuilder SqlUpdateStatement,
      List<SqlParameter> ParmList,
      EdRecordField EntityField )
    {
      this.LogMethod ( "updateCheckBoxValueField" );
      this.LogDebug ( "ValueCount: {0}. ", this._ValueCount );
      this.LogDebug ( "Field: {0}, V:{1}.", EntityField.FieldId, EntityField.ItemValue );

      // 
      // Define the record field Guid
      // 
      SqlParameter prm = new SqlParameter ( EdEntityValues.PARM_FIELD_GUID + "_" + this._ValueCount, SqlDbType.UniqueIdentifier );
      prm.Value = EntityField.FieldGuid;
      ParmList.Add ( prm );

      // 
      // Define the record field Guid
      // 
      prm = new SqlParameter ( EdEntityValues.PARM_VALUE_GUID + "_" + this._ValueCount, SqlDbType.UniqueIdentifier );
      prm.Value = EntityField.Guid;
      ParmList.Add ( prm );

      foreach ( EvOption option in EntityField.Design.OptionList )
      {
        // 
        // Define the record column identifier
        // 
        prm = new SqlParameter ( EdEntityValues.PARM_VALUE_COLUMN_ID + "_" + this._ValueCount, SqlDbType.NVarChar, 10 );
        prm.Value = option.Value;
        ParmList.Add ( prm );

        // 
        // Define the record field Guid
        // 
        prm = new SqlParameter ( EdEntityValues.PARM_VALUE_ROW + "_" + this._ValueCount, SqlDbType.SmallInt );
        prm.Value = 0;
        ParmList.Add ( prm );

        //
        // set the value to 1 if the option is in the field value.
        //
        string value = "0";
        if ( EntityField.ItemValue.Contains ( option.Value ) == true )
        {
          value = "1";
        }
        this.LogDebug ( "Entity Value Col {0}, Value {1}.", option.Value, value );

        prm = new SqlParameter ( EdEntityValues.PARM_VALUE_NUMERIC + "_" + this._ValueCount, SqlDbType.Float );
        prm.Value = value;
        ParmList.Add ( prm );
        //
        // Create the add query .
        //
        SqlUpdateStatement.AppendLine ( " INSERT INTO ED_ENTITY_VALUES  "
        + "(" + EdEntityValues.DB_ENTITY_GUID
        + ", " + EdEntityValues.DB_FIELD_GUID
        + ", " + EdEntityValues.DB_VALUES_GUID
        + ", " + EdEntityValues.DB_VALUES_COLUMN_ID
        + ", " + EdEntityValues.DB_VALUES_ROW
        + ", " + EdEntityValues.DB_VALUES_NUMERIC
        + "  ) " );
        SqlUpdateStatement.AppendLine ( "VALUES (" );
        SqlUpdateStatement.AppendLine (
          " " + EdEntityValues.PARM_ENTITY_GUID
         + ", " + EdEntityValues.PARM_FIELD_GUID + "_" + this._ValueCount
         + ", " + EdEntityValues.PARM_VALUE_GUID + "_" + this._ValueCount
         + ", " + EdEntityValues.PARM_VALUE_COLUMN_ID + "_" + this._ValueCount
         + ", " + EdEntityValues.PARM_VALUE_ROW + "_" + this._ValueCount
         + ", " + EdEntityValues.PARM_VALUE_NUMERIC + "_" + +this._ValueCount + " );\r\n" );

      }//END ITERATION LOOP

      this.LogMethodEnd ( "updateCheckBoxValueField" );
    }//END method.

    #endregion

  }//END EvFormRecordFields class

}//END namespace Evado.Dal.Digital
