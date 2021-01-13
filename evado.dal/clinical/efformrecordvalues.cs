/***************************************************************************************
 * <copyright file="dal\EvRecordFields.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2015 EVADO HOLDING PTY. LTD..  All rights reserved.
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
using System.Text;

//Application specific class references.
//using Evado.Model.Forms;
//using Evado.Dal;
//using Evado.Model;


namespace Evado.Dal.Forms
{
  /// <summary>
  /// Data Access Layer class for ChecklistItem
  /// </summary>
  public class EvFormRecordValues
  {
    #region Object Initialisation
    /* *********************************************************************************
     * 
     * Defines the classes constansts and global variables
     * 
     * *********************************************************************************/
    // The log file source. 
    private static string _eventLogSource = ConfigurationManager.AppSettings [ "EventLogSource" ];
    // 
    // Selection query string.
    // 
    private const string _sqlQuery_View = "Select * FROM EF_FORM_RECORD_FIELD_VIEW ";
    // 
    // Table name 
    // 
    private const string _TBL_FORM_RECORD_VALUES = "EF_FORM_RECORD_VALUES ";

    public const string _DB_FORM_RECORD_VALUE_GUID = "FORM_RECORD_VALUE_GUID";
    public const string _DB_FORM_RECORD_VALUE_COLUMN = "FORM_RECORD_VALUE_COLUMN";
    public const string _DB_FORM_RECORD_VALUE_ROW = "FORM_RECORD_VALUE_ROW";
    public const string _DB_FORM_RECORD_VALUE_STRING = "FORM_RECORD_VALUE_STRING";
    public const string _DB_FORM_RECORD_VALUE_NUMERIC = "FORM_RECORD_VALUE_NUMERIC";
    public const string _DB_FORM_RECORD_VALUE_DATE = "FORM_RECORD_VALUE_DATE";
    public const string _DB_FORM_RECORD_VALUE_TEXT = "FORM_RECORD_VALUE_TEXT";
    // 
    private const string _parmGuid = "@Guid";
    private const string _parmRecordGuid = "@RecordGuid";
    private const string _parmFormFieldGuid = "@FormFieldGuid";
    //
    //  Define the SQL query string variable.
    //  
    //
    private string _sqlQueryString = String.Empty;

    //
    // Define the class state property and variable.
    //
    private StringBuilder _DebugLog = new StringBuilder ( );

    public string DebugLog
    {
      get
      {
        return _DebugLog.ToString ( );
      }
    }

    // +++++++++++++++++++++++++++ END INITIALISATION SECTION ++++++++++++++++++++++++++++++
    #endregion

    #region RecordField Reader


    // =====================================================================================
    /// <summary>
    /// Method: getRowData
    /// 
    /// Description:
    /// Reads the content of the data reader object into ChecklistItem business object.
    /// 
    /// </summary>
    /// <param name="Row">SqlDataReader</param>
    // -------------------------------------------------------------------------------------
    private void getFieldObject ( DataRow Row, Evado.Model.Forms.EfFormField formField )
    {
      // 
      // Fill the identifiers
      //
      formField.Guid = EvSqlMethods.getGuid ( Row, EvFormRecordValues._DB_FORM_RECORD_VALUE_GUID );
      formField.RecordGuid = EvSqlMethods.getGuid ( Row, EvFormRecords._DB_FORM_RECORD_GUID );
      formField.FormFieldGuid = EvSqlMethods.getGuid ( Row, EvFormFields._DB_FORM_FIELD_GUID );
      formField.FormGuid = EvSqlMethods.getGuid ( Row, EvForms._DB_FORM_GUID );
      //
      // Fill the form field prpoperties
      //
      formField.FieldId = EvSqlMethods.getString ( Row, EvFormFields._DB_FORM_FIELD_ID );
      formField.Title = EvSqlMethods.getString ( Row, EvFormFields._DB_FORM_FIELD_TITLE );

      formField.TypeId = Evado.Model.EvStatics.Enumerations.parseEnumValue<Evado.Model.EvDataTypes> ( EvSqlMethods.getString ( Row, EvFormFields._DB_FORM_FIELD_TYPE_ID ) );

      string xmlValidationRules = EvSqlMethods.getString ( Row, EvFormFields._DB_FORM_FIELD_VALIDATION );
      if ( xmlValidationRules != String.Empty )
      {
        formField.ValidationRules =
          Evado.Model.EvStatics.DeserialiseObject<Evado.Model.Forms.EvFormFieldValidationRules> ( xmlValidationRules );
      }

      string xmlDesign = EvSqlMethods.getString ( Row, EvFormFields._DB_FORM_FIELD_DESIGN );
      if ( xmlDesign != String.Empty )
      {
        formField.Design = Evado.Model.EvStatics.DeserialiseObject<Evado.Model.Forms.EfFormFieldDesign> ( xmlDesign );
      }

      formField.Title = EvSqlMethods.getString ( Row, EvFormFields._DB_FORM_FIELD_TITLE );
      formField.Design.Unit = EvSqlMethods.getString ( Row, EvFormFields._DB_FORM_FIELD_UNIT );
      formField.Order = EvSqlMethods.getShort ( Row, EvFormFields._DB_FORM_FIELD_ORDER );
      formField.Design.GroupId = EvSqlMethods.getShort ( Row, EvFormFields._DB_FORM_FIELD_GROUP_ID );
      formField.Design.MandatoryField = EvSqlMethods.getBool ( Row, EvFormFields._DB_FORM_FIELD_MANDATORY );
      formField.Design.HiddenField = EvSqlMethods.getBool ( Row, EvFormFields._DB_FORM_FIELD_HIDDEN );

      this._DebugLog.AppendLine ( "Field Guid: " + formField.Guid + ", Id: " + formField.FieldId + ", TypeId: " + formField.TypeId );

      this.getValidationRules ( Row, formField );

      //
      // Process the different field values.
      //
      switch ( formField.TypeId )
      {
        case Evado.Model.EvDataTypes.Table:
        case Evado.Model.EvDataTypes.Special_Matrix:
          {
            this.getTableField ( Row, formField );
            break;
          }
        case Evado.Model.EvDataTypes.Date:
          {
            this.getDateField ( Row, formField );
            break;
          }
        case Evado.Model.EvDataTypes.Numeric:
          {
            this.getNumericField ( Row, formField );
            break;
          }
        case Evado.Model.EvDataTypes.External_Selection_List:
          {
            this._DebugLog.AppendLine ( "External ListId: " + formField.Design.ExSelectionListId
              + " Category: " + formField.Design.ExSelectionListCategory );

            this.getExternalSelectionField ( Row, formField );
            break;
          }
        case Evado.Model.EvDataTypes.Selection_List:
        case Evado.Model.EvDataTypes.Radio_Button_List:
          {
            this.getSelectionListField ( Row, formField );
            break;
          }
        case Evado.Model.EvDataTypes.Check_Box_List:
          {
            this.geCheckboxListField ( Row, formField );
            break;
          }
        case Evado.Model.EvDataTypes.Analogue_Scale:
          {
            this.getNumericField ( Row, formField );
            formField.Design.SelectByCodingValue = true;
            break;
          }
        case Evado.Model.EvDataTypes.Horizontal_Radio_Buttons:
          {
            this.getSelectionListField ( Row, formField );
            formField.Design.SelectByCodingValue = true;
            break;
          }
        case Evado.Model.EvDataTypes.Free_Text:
          {
            // 
            // Get the field value.
            //
            formField.Value = EvSqlMethods.getString ( Row, EvFormRecordValues._DB_FORM_RECORD_VALUE_TEXT );
            break;
          }
        default:
          {
            //
            // Get the field value.
            //
            formField.Value = EvSqlMethods.getString ( Row, EvFormRecordValues._DB_FORM_RECORD_VALUE_STRING );
            break;
          }
      }

    }//END getRowData method.

    // =====================================================================================
    /// <summary>
    /// this method processes a form field type that is a date.
    /// </summary>
    /// <param name="Row">The DataRow containing the field</param>
    /// <param name="FormField"> Evado.Model.Forms.EvFormField object, containg the field data.</param>
    // -------------------------------------------------------------------------------------
    private void getValidationRules ( DataRow Row,  Evado.Model.Forms.EfFormField FormField )
    {
      //
      // get the validation rules.
      //
      string xmlValidationRules = EvSqlMethods.getString ( Row, EvFormFields._DB_FORM_FIELD_VALIDATION );
      if ( xmlValidationRules != String.Empty )
      {
        FormField.ValidationRules =
          Evado.Model.EvStatics.DeserialiseObject<Evado.Model.Forms.EvFormFieldValidationRules> ( xmlValidationRules );
      }

      //
      // If the validation rules are null exit.
      //
      if ( FormField.ValidationRules == null )
      {
        return;
      }


      // 
      // If the selection newField is null
      // 
      if ( ( FormField.TypeId == Evado.Model.EvDataTypes.Selection_List
          || FormField.TypeId == Evado.Model.EvDataTypes.External_Selection_List
          || FormField.TypeId == Evado.Model.EvDataTypes.Check_Box_List
          || FormField.TypeId == Evado.Model.EvDataTypes.Radio_Button_List )
        && FormField.ValidationRules.NotValidOptions == null )
      {
        FormField.ValidationRules.NotValidOptions = new Evado.Model.Forms.EfFormFieldValidationNotValid ( );
      }

    }//END getValidation method.

    // =====================================================================================
    /// <summary>
    /// this method processes a form field type that is a date.
    /// </summary>
    /// <param name="Row">The DataRow containing the field</param>
    /// <param name="FormField"> Evado.Model.Forms.EvFormField object, containg the field data.</param>
    // -------------------------------------------------------------------------------------
    private void getDateField ( DataRow Row,  Evado.Model.Forms.EfFormField FormField )
    {
      //
      // Get the field data value.
      //
      DateTime dtValue = EvSqlMethods.getDateTime ( Row, EvFormRecordValues._DB_FORM_RECORD_VALUE_DATE );

      this._DebugLog.AppendLine ( "getDateField method. Date value: " + dtValue );

      //
      // if null date set to empty value.
      //
      if ( dtValue == Evado.Model.EvStatics.CONST_DATE_NULL )
      {
        FormField.Value = String.Empty;

        return;
      }

      //
      // Return the value.
      //
      FormField.Value = dtValue.ToString ( "dd MMM yyyy" );



    }

    // =====================================================================================
    /// <summary>
    /// this method processes a form field type that is a numeric.
    /// </summary>
    /// <param name="Row">The DataRow containing the field</param>
    /// <param name="FormField"> Evado.Model.Forms.EvFormField object, containg the field data.</param>
    // -------------------------------------------------------------------------------------
    private void getNumericField ( DataRow Row,  Evado.Model.Forms.EfFormField FormField )
    {
      //
      // Get the field numeric value.
      //
      float fltValue = EvSqlMethods.getFloat ( Row, EvFormRecordValues._DB_FORM_RECORD_VALUE_NUMERIC );

      FormField.Value = fltValue.ToString ( );
    }

    // =====================================================================================
    /// <summary>
    /// this method processes a form field type that is a numeric.
    /// </summary>
    /// <param name="Row">The DataRow containing the field</param>
    /// <param name="FormField"> Evado.Model.Forms.EvFormField object, containg the field data.</param>
    // -------------------------------------------------------------------------------------
    private void getSelectionListField ( DataRow Row,  Evado.Model.Forms.EfFormField FormField )
    {
      //
      // Get the field numeric value.
      //
      FormField.Value = EvSqlMethods.getString ( Row, EvFormRecordValues._DB_FORM_RECORD_VALUE_STRING );

      if ( Evado.Model.EvStatics.hasNumericNul ( FormField.Value ) == true )
      {
        FormField.Value = Evado.Model.EvStatics.CONST_NUMERIC_NOT_AVAILABLE;
      }
      bool bHasZero = false;
      bool bHasNA = false;

      foreach (  Evado.Model.EvOption option in FormField.Design.OptionList )
      {
        if ( option.Value == "0" )
        {
          bHasZero = true;
        }
        if ( option.Value == "NA" )
        {
          bHasNA = true;
        }
      }
      if ( FormField.Value == "0"
           && bHasNA == true
           && bHasZero == false )
      {
        FormField.Value = "NA";
      }
    }

    // =====================================================================================
    /// <summary>
    /// this method processes a form field type that is a numeric.
    /// </summary>
    /// <param name="Row">The DataRow containing the field</param>
    /// <param name="FormField"> Evado.Model.Forms.EvFormField object, containg the field data.</param>
    // -------------------------------------------------------------------------------------
    private void geCheckboxListField ( DataRow Row,  Evado.Model.Forms.EfFormField FormField )
    {
      //
      // Get the field value.
      //
      FormField.Design.SelectByCodingValue = false;
      int row = EvSqlMethods.getInteger ( Row, EvFormRecordValues._DB_FORM_RECORD_VALUE_ROW );
      bool value = EvSqlMethods.getBool ( Row, EvFormRecordValues._DB_FORM_RECORD_VALUE_NUMERIC );

      //
      // if the value is true then the option is checked.
      // so add it to the value list.
      //
      if ( value == true
        && row < FormField.Design.OptionList.Count )
      {
        FormField.Value += ";" + FormField.Design.OptionList [ row ].Value;
      }
    }

    // =====================================================================================
    /// <summary>
    /// this method processes a form field type that is a date.
    /// </summary>
    /// <param name="Row">The DataRow containing the field</param>
    /// <param name="FormField"> Evado.Model.Forms.EvFormField object, containg the field data.</param>
    // -------------------------------------------------------------------------------------
    private void getExternalSelectionField ( DataRow Row,  Evado.Model.Forms.EfFormField FormField )
    {
      //
      // Get the field numeric value.
      //
      FormField.Value = EvSqlMethods.getString ( Row, EvFormRecordValues._DB_FORM_RECORD_VALUE_STRING );

      //
      // Initialise the methods variables and objects.
      //
      //EvFormFieldSelectionLists externalCodingLists = new EvFormFieldSelectionLists ( );

      //
      // Retrieve the external selection list.
      //
      this._DebugLog.AppendLine ( "Ext ListId: " + FormField.Design.ExSelectionListId
        + " Category: " + FormField.Design.ExSelectionListCategory );

      //formField.Design.Options = externalCodingLists.getItemCodingList (
      // formField.Design.ExSelectionListId,
      // formField.Design.ExSelectionListCategory );

      FormField.TypeId = Evado.Model.EvDataTypes.Selection_List;
    }

    // =====================================================================================
    /// <summary>
    /// this method processes a form field type that is a matrix or a table.
    /// Note:  the cell indexes are zero based.
    /// </summary>
    /// <param name="Row">The DataRow containing the field</param>
    /// <param name="FormField"> Evado.Model.Forms.EvFormField object, containg the field data.</param>
    // -------------------------------------------------------------------------------------
    private void getTableField ( DataRow Row,  Evado.Model.Forms.EfFormField FormField )
    {
      //
      // If the field is not a table exit the method.
      //
      if ( FormField.TypeId != Evado.Model.EvDataTypes.Special_Matrix
       && FormField.TypeId != Evado.Model.EvDataTypes.Table )
      {
        return;
      }

      //
      // Initialise the table objects.
      //
      FormField.Table = FormField.Design.Table;

      this._DebugLog.AppendLine ( "FieldId: " + FormField.FieldId 
        + " Columns: " + FormField.Table.ColumnCount 
        + ", Rows: " + FormField.Table.Rows.Count );

      this.getTableCellValue ( Row, FormField );

      this._DebugLog.AppendLine ( "Column 1 header text: " + FormField.Table.Header [ 0 ].Text );

    }//END getTableField method

    // =====================================================================================
    /// <summary>
    /// this method processes a form field type that is a numeric.
    /// </summary>
    /// <param name="Row">The DataRow containing the field</param>
    /// <param name="FormField"> Evado.Model.Forms.EvFormField object, containg the field data.</param>
    // -------------------------------------------------------------------------------------
    private void getCheckBoxValue ( DataRow Row,  Evado.Model.Forms.EfFormField FormField )
    {
      //
      // Get the field value.
      //
      int row = EvSqlMethods.getInteger ( Row, EvFormRecordValues._DB_FORM_RECORD_VALUE_ROW );
      bool value = EvSqlMethods.getBool ( Row, EvFormRecordValues._DB_FORM_RECORD_VALUE_NUMERIC );

      this._DebugLog.AppendLine ( "getCheckBoxValue method. "
        + " Row: " + row + ", value: " + value );
      //
      // if the value is true then the option is checked.
      // so add it to the value list.
      //
      if ( value == true
        && row < FormField.Design.OptionList.Count )
      {
        FormField.Value += ";" + FormField.Design.OptionList [ row ].Value;
      }

    }//END getCheckBoxValue method

    // =====================================================================================
    /// <summary>
    /// this method updates a table cell value form the query result set.
    /// Note:  the cell indexes are zero based.
    /// </summary>
    /// <param name="Row">The DataRow containing the field</param>
    /// <param name="FormField"> Evado.Model.Forms.EvFormField object, containg the field data.</param>
    // -------------------------------------------------------------------------------------
    private void getTableCellValue ( DataRow Row,  Evado.Model.Forms.EfFormField FormField )
    {
      //
      // Validate that the table exists.
      //
      if ( FormField.Table == null )
      {
        this._DebugLog.AppendLine ( "FormField.Table is null" );
        return;
      }

      //
      // Get the field row values.
      //
      int row = EvSqlMethods.getInteger ( Row, EvFormRecordValues._DB_FORM_RECORD_VALUE_ROW );
      int col = EvSqlMethods.getInteger ( Row, EvFormRecordValues._DB_FORM_RECORD_VALUE_COLUMN );

      string value = this.getTableColumnValue ( Row, col, FormField );

      this._DebugLog.AppendLine ( "getTableCellValue method. "
        + " Row: " + row + " col: " + col + ", value: " + value );

      //
      // Add the value to the table.
      //
      if ( row < FormField.Table.Rows.Count )
      {
        FormField.Table.Rows [ row ].Column [ col ] = value;
      }

    }//END getTableCellValue method

    // =====================================================================================
    /// <summary>
    /// this method updates a table cell value form the query result set.
    /// Note:  the cell indexes are zero based.
    /// </summary>
    /// <param name="Row">The DataRow containing the field</param>
    /// <param name="Col">Int: The column index in the able.</param>
    /// <param name="FormField"> Evado.Model.Forms.EvFormField object, containg the field data.</param>
    // -------------------------------------------------------------------------------------
    private String getTableColumnValue ( DataRow Row, int Col,  Evado.Model.Forms.EfFormField FormField )
    {
      //
      // Select the column to retieve the value from
      //
      switch ( FormField.Table.Header [ Col ].TypeId )
      {
        case Evado.Model.Forms.EfFormFieldTableColumnTypes.Numeric:
          {
            float fltValue = EvSqlMethods.getFloat ( Row, EvFormRecordValues._DB_FORM_RECORD_VALUE_NUMERIC );
            return fltValue.ToString ( );
          }
        case Evado.Model.Forms.EfFormFieldTableColumnTypes.Yes_No:
          {
            bool bYesNo = EvSqlMethods.getBool ( Row, EvFormRecordValues._DB_FORM_RECORD_VALUE_NUMERIC );
            string value = "No";
            if ( bYesNo == true )
            {
              value = "Yes";
            }
            return value;
          }
        case Evado.Model.Forms.EfFormFieldTableColumnTypes.Date:
          {
            DateTime dtValue = EvSqlMethods.getDateTime ( Row, EvFormRecordValues._DB_FORM_RECORD_VALUE_DATE );

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
            return EvSqlMethods.getString ( Row, EvFormRecordValues._DB_FORM_RECORD_VALUE_STRING );
          }
      }//END table column type switch.

    }//END getTableColumnValue method

    // +++++++++++++++++++++++++++ END RECORD READER SECTION  ++++++++++++++++++++++++++++++
    #endregion

    #region RecordField Queries

    // =====================================================================================
    /// <summary>
    /// Description:
    /// Gets a list containing a selectionList of ChecklistItem data objects.
    /// 
    /// </summary>
    /// <param name="RecordGuid">(Mandatory) The record GUID.</param>
    /// <returns>List of  Evado.Model.Forms.EvFormField objects. </returns>
    // -------------------------------------------------------------------------------------
    public List<Evado.Model.Forms.EfFormField> GetView ( Guid RecordGuid )
    {
      // 
      // Define the local variables
      // 
      this._DebugLog.AppendLine ( "Evado.Dal.Forms.EvRecordFields.GetView method. "
        + " RecordGuid: " + RecordGuid );
      List<Evado.Model.Forms.EfFormField> view = new List<Evado.Model.Forms.EfFormField> ( );
      Guid lastRecordFieldGuid = Guid.Empty;
       Evado.Model.Forms.EfFormField formField = new  Evado.Model.Forms.EfFormField ( );

      // 
      // Validate that the record uid is valid.
      // 
      if ( RecordGuid == Guid.Empty )
      {
        return view;
      }

      // 
      // Define the SQL query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(_parmRecordGuid, SqlDbType.UniqueIdentifier),
      };
      cmdParms [ 0 ].Value = RecordGuid;

      // 
      // Define the query string.
      // 
      _sqlQueryString = _sqlQuery_View + " WHERE ( " + EvFormRecords._DB_FORM_RECORD_GUID + " = " + _parmRecordGuid + " ) "
        + "ORDER BY " + EvFormFields._DB_FORM_FIELD_GROUP_ID
        + "," + EvFormFields._DB_FORM_FIELD_ORDER
        + "," + EvFormRecordValues._DB_FORM_RECORD_VALUE_ROW
        + "," + EvFormRecordValues._DB_FORM_RECORD_VALUE_COLUMN + " ; ";

      this._DebugLog.AppendLine ( _sqlQueryString );

      // 
      // Execute the query against the database. 
      // 
      using ( DataTable table = EvSqlMethods.RunQuery ( _sqlQueryString, cmdParms ) )
      {
        this._DebugLog.AppendLine ( "Returned Rows: " + table.Rows.Count );
        // 
        // Iterate through the results extracting the role information.
        // 
        for ( int count = 0; count < table.Rows.Count; count++ )
        {
          // 
          // Extract the table row
          // 
          DataRow row = table.Rows [ count ];

          Guid fieldGuid = EvSqlMethods.getGuid ( row, EvFormRecordValues._DB_FORM_RECORD_VALUE_GUID );

          //
          // Empty fields are skipped.
          //
          if ( fieldGuid == Guid.Empty )
          {
            continue;
          }

          //
          // If the field guid has changed then it is a new field.
          // So add the previous field then get the data for the new field.
          //
          if ( lastRecordFieldGuid != fieldGuid )
          {
            this._DebugLog.AppendLine ( "Add field to record field list." );
            //
            // Add the last field to the list.
            //
            if ( formField.Guid != Guid.Empty )
            {
              view.Add ( formField );
            }

            //
            // Initialise the new form field.
            //
            formField = new  Evado.Model.Forms.EfFormField ( );

            // 
            // Get the object data from the row.
            // 
            this.getFieldObject ( row, formField );

            //
            // Update the lst field guid to enable the other field values to be collected.
            //
            lastRecordFieldGuid = formField.Guid;

          }//END create new field object.
          else
          {

            this._DebugLog.AppendLine ( "Secondary field value." );

            switch ( formField.TypeId )
            {
              case Evado.Model.EvDataTypes.Special_Matrix:
              case Evado.Model.EvDataTypes.Table:
                {
                  this.getTableCellValue ( row, formField );
                  break;
                }
              case Evado.Model.EvDataTypes.Check_Box_List:
                {
                  this.getCheckBoxValue ( row, formField );
                  break;
                }
            }
          }//END value records that need to update the current field.

        }//ENR record iteration loop.

        //
        // Add the last field to the list.
        //
        if ( formField.Guid != Guid.Empty )
        {
          view.Add ( formField );
        }

      }//ENd using statement

      // 
      // Pass back the result arrray.
      // 
      return view;

    } // Close getView method.

    // ++++++++++++++++++++++++++++++ END VIEW QUERY SECTION +++++++++++++++++++++++++++++++
    #endregion

    #region EvRecordValue Update queries

    // =====================================================================================
    /// <summary>
    /// Method: updateTestReport
    /// 
    /// Description:
    /// Update CustomField data object in the database using it unique object identifier.
    /// 
    /// </summary>
    /// <param name="Fields">List of  Evado.Model.Forms.EvFormField objects.</param>
    /// <returns></returns>
    // -------------------------------------------------------------------------------------
    public Evado.Model.EvEventCodes updateFields ( List<Evado.Model.Forms.EfFormField> Fields )
    {
      // 
      // Define the local variables.
      // 
      this._DebugLog.AppendLine ( "Evado.Dal.Forms.EvRecordFields.updateFields, Count: " + Fields.Count );
      StringBuilder SqlQueryString = new StringBuilder ( );

      // 
      // Iterate through the instrument items updating each 
      // 
      foreach ( Evado.Model.Forms.EfFormField field in Fields )
      {
        if ( field != null )
        {
          this._DebugLog.Append ( " field.Guid: " + field.Guid
            + " FieldId: " + field.FieldId
            + " >> '" + field.Value );

          //
          // If uid is zero then the newField new and add it to the database.
          //
          this._DebugLog.AppendLine ( " >> UPDATE ITEM " );

          SqlQueryString.AppendLine ( this.updateField ( field ) );

        }//END newField not null

      }//END FormField Update Iternation.

      this._DebugLog.AppendLine ( SqlQueryString.ToString ( ) );

      //return Evado.Model.EvEventCodes.Database_Record_Update_Error;

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.QueryUpdate ( SqlQueryString.ToString ( ), null ) == 0 )
      {
        return Evado.Model.EvEventCodes.Database_Record_Update_Error;
      }

      // 
      // Return value
      // 
      return Evado.Model.EvEventCodes.Ok;

    }//END UpdateItems method 

    // =====================================================================================
    /// <summary>
    /// Description:
    /// Update form record field value. Storing each value in a separate SQL column row.
    /// By generating and update SQL query for the field. 
    /// 
    /// </summary>
    /// <param name="FormField"> Evado.Model.Forms.EvFormField data object</param>
    // -------------------------------------------------------------------------------------
    public String updateField (  Evado.Model.Forms.EfFormField FormField )
    {
      this._DebugLog.AppendLine ( "\r\nEvado.Dal.Forms.EvFormRecordField.updateField method. "
        + ", Field.FieldId: " + FormField.FieldId
        + ", Field.TypeId: " + FormField.TypeId
        + ", Field.Value: '" + FormField.Value + "'" );

      //
      // Switch the process based on the type of field.
      //
      switch ( FormField.TypeId )
      {
        case Evado.Model.EvDataTypes.Boolean:
          {
            int intValue = 0;

            if ( Evado.Model.EvStatics.getBool ( FormField.Value ) == true )
            {
              intValue = 1;
            }

            return "\r\n UPDATE " + EvFormRecordValues._TBL_FORM_RECORD_VALUES + " SET "
              + EvFormRecordValues._DB_FORM_RECORD_VALUE_NUMERIC + " = " + intValue + " "
              + "WHERE "
              + EvFormRecordValues._DB_FORM_RECORD_VALUE_GUID + " = '" + FormField.Guid + "' "
              + "AND " + EvFormRecordValues._DB_FORM_RECORD_VALUE_ROW + " = 0 "
              + "AND " + EvFormRecordValues._DB_FORM_RECORD_VALUE_COLUMN + " = 0 ;";
          }
        case Evado.Model.EvDataTypes.Numeric:
        case Evado.Model.EvDataTypes.Analogue_Scale:
          {
            /// 
            /// If there is no value skip update.
            /// 
            if ( FormField.Value == String.Empty )
            {
              return String.Empty ;
            }
            /// 
            /// If there is a not available set the value to numeric null.
            /// 
            if ( FormField.Value == Evado.Model.EvStatics.CONST_NUMERIC_NOT_AVAILABLE )
            {
              FormField.Value = Evado.Model.EvStatics.CONST_NUMERIC_NULL.ToString ( );
            }

            /// 
            /// Create the update SQL statement
            /// 
            return "\r\n UPDATE 	" + EvFormRecordValues._TBL_FORM_RECORD_VALUES + " SET "
              + EvFormRecordValues._DB_FORM_RECORD_VALUE_NUMERIC + " = " + FormField.Value + " "
              + "WHERE "
              + EvFormRecordValues._DB_FORM_RECORD_VALUE_GUID + " = '" + FormField.Guid + "' "
              + "AND " + EvFormRecordValues._DB_FORM_RECORD_VALUE_ROW + " = 0 "
              + "AND " + EvFormRecordValues._DB_FORM_RECORD_VALUE_COLUMN + " = 0 ;";
          }
        case Evado.Model.EvDataTypes.Date:
          {
            return "\r\n UPDATE 	" + EvFormRecordValues._TBL_FORM_RECORD_VALUES + " SET "
               + EvFormRecordValues._DB_FORM_RECORD_VALUE_DATE + " = '" + FormField.Value + "' "
               + "WHERE "
               + EvFormRecordValues._DB_FORM_RECORD_VALUE_GUID + " = '" + FormField.Guid + "' "
               + "AND " + EvFormRecordValues._DB_FORM_RECORD_VALUE_ROW + " = 0 "
               + "AND " + EvFormRecordValues._DB_FORM_RECORD_VALUE_COLUMN + " = 0 ;";
          }
        case Evado.Model.EvDataTypes.Free_Text:
          {
            return "\r\n UPDATE 	" + EvFormRecordValues._TBL_FORM_RECORD_VALUES + " SET "
               + EvFormRecordValues._DB_FORM_RECORD_VALUE_TEXT + " = '" + FormField.Value + "' "
               + "WHERE "
               + EvFormRecordValues._DB_FORM_RECORD_VALUE_GUID + " = '" + FormField.Guid + "' "
               + "AND " + EvFormRecordValues._DB_FORM_RECORD_VALUE_ROW + " = 0 "
               + "AND " + EvFormRecordValues._DB_FORM_RECORD_VALUE_COLUMN + " = 0 ;";
          }
        case Evado.Model.EvDataTypes.Check_Box_List:
          {
            return updateCheckBoxList ( FormField );
          }
        case Evado.Model.EvDataTypes.Special_Matrix:
        case Evado.Model.EvDataTypes.Table:
          {
            return updateTable ( FormField );
          }
        default:
          {
            if ( FormField.Value.Length > 499 )
            {
              FormField.Value = FormField.Value.Substring ( 0, 499 );
            }

            return "\r\n UPDATE 	" + EvFormRecordValues._TBL_FORM_RECORD_VALUES + " SET "
               + EvFormRecordValues._DB_FORM_RECORD_VALUE_STRING + " = '" + FormField.Value + "' "
               + "WHERE "
               + EvFormRecordValues._DB_FORM_RECORD_VALUE_GUID + " = '" + FormField.Guid + "' "
               + "AND " + EvFormRecordValues._DB_FORM_RECORD_VALUE_ROW + " = 0 "
               + "AND " + EvFormRecordValues._DB_FORM_RECORD_VALUE_COLUMN + " = 0 ;";
          }
      }//END field type switch

    } // Close updateField method

    // =====================================================================================
    /// <summary>
    /// This method generates the add SQL statement for a checkbox list field.
    /// </summary>
    /// <param name="FormField"></param>
    /// <returns></returns>
    // -------------------------------------------------------------------------------------
    private String updateCheckBoxList (  Evado.Model.Forms.EfFormField FormField )
    {
      //
      // Initialise the methods variables and options.
      //
      string stSqlQueryText = String.Empty;
      int intChecked = 0;

      //
      // if not options then exit.
      //
      if ( FormField.Design.OptionList.Count == 0 )
      {
        return stSqlQueryText;
      }

      //
      // if the option exists in the field value at the check to 1 indicating selected.
      //
      if ( FormField.Value.Contains ( FormField.Design.OptionList [ 0 ].Value ) == true )
      {
        intChecked = 1;
      }

      //
      // generate the check box option value
      //
      stSqlQueryText += "\r\n UPDATE " + EvFormRecordValues._TBL_FORM_RECORD_VALUES + " SET "
            + EvFormRecordValues._DB_FORM_RECORD_VALUE_NUMERIC + " = " + intChecked + " "
            + "WHERE "
            + EvFormRecordValues._DB_FORM_RECORD_VALUE_GUID + " = '" + FormField.Guid + "' "
            + "AND " + EvFormRecordValues._DB_FORM_RECORD_VALUE_ROW + " = 0 "
            + "AND " + EvFormRecordValues._DB_FORM_RECORD_VALUE_COLUMN + " = 0; \r\n";

      //
      //  Delete the other rows value
      //
      stSqlQueryText += "\r\n DELETE FROM " + EvFormRecordValues._TBL_FORM_RECORD_VALUES
            + "WHERE (" + EvFormRecordValues._DB_FORM_RECORD_VALUE_GUID + " = '" + FormField.Guid + "' ) "
            + "AND (" + EvFormRecordValues._DB_FORM_RECORD_VALUE_ROW + " > 0 ); \r\n";
      //
      // Iterate through the field option creating a row for each option.
      //
      for ( int iRow = 1; iRow < FormField.Design.OptionList.Count; iRow++ )
      {
        //
        // Initialise the check selection
        //
        intChecked = 0;

        //
        // if the option exists in the field value at the check to 1 indicating selected.
        //
        if ( FormField.Value.Contains ( FormField.Design.OptionList [ iRow ].Value ) == true )
        {
          intChecked = 1;
        }
        //
        // generate the check box option value
        //
        stSqlQueryText += "\r\n INSERT INTO " + EvFormRecordValues._TBL_FORM_RECORD_VALUES + " ("
            + EvFormRecordValues._DB_FORM_RECORD_VALUE_GUID + ", "
            + EvFormFields._DB_FORM_FIELD_GUID + ", "
            + EvFormRecords._DB_FORM_RECORD_GUID + ", "
            + EvFormRecordValues._DB_FORM_RECORD_VALUE_ROW + ", "
            + EvFormRecordValues._DB_FORM_RECORD_VALUE_COLUMN + ", "
            + EvFormRecordValues._DB_FORM_RECORD_VALUE_NUMERIC + " ) "
            + "VALUES ("
            + "'" + FormField.Guid + "', "
            + "'" + FormField.FormFieldGuid + "', "
            + "'" + FormField.RecordGuid + "', "
            + iRow + ", "
            + "0, "
            + intChecked + " );";
      }//END list iteration loop

      stSqlQueryText += " \r\n";

      return stSqlQueryText;

    }//END addCheckBoxList method

    // =====================================================================================
    /// <summary>
    /// This method generates the add SQL statement for a checkbox list field.
    /// </summary>
    /// <param name="FormField"></param>
    /// <returns></returns>
    // -------------------------------------------------------------------------------------
    private String updateTable (  Evado.Model.Forms.EfFormField FormField )
    {
      //
      // Initialise the methods variables and options.
      //
      string stSqlQueryText = String.Empty;

      stSqlQueryText += "\r\n DELETE FROM " + EvFormRecordValues._TBL_FORM_RECORD_VALUES
            + "WHERE "
            + EvFormRecordValues._DB_FORM_RECORD_VALUE_GUID + " = '" + FormField.Guid + "' ";

      //
      // Iterate through the field option creating a row for each option.
      //
      for ( int iRow = 0; iRow < FormField.Table.Rows.Count; iRow++ )
      {
        //
        // Iterate through the columns in each row.
        //
        for ( int iCol = 0; iCol < FormField.Table.ColumnCount; iCol++ )
        {
         // if ( iRow == 0 && iCol == 0 )
         // {
         //   continue;
         // }

          //
          // Generate the date for the correct data type.
          //
          switch ( FormField.Table.Header [ iCol ].TypeId )
          {
            case Evado.Model.Forms.EfFormFieldTableColumnTypes.Numeric:
              {
                string stValue = FormField.Table.Rows [ iRow ].Column [ iCol ].Trim ( );

                /// 
                /// If there is no value skip update.
                /// 
                if ( stValue == String.Empty )
                {
                  break;
                }

                /// 
                /// If there is a not available set the value to numeric null.
                /// 
                if ( stValue == Evado.Model.EvStatics.CONST_NUMERIC_NOT_AVAILABLE )
                {
                  stValue = Evado.Model.EvStatics.CONST_NUMERIC_NULL.ToString ( );
                }

                /// 
                /// Create the update SQL statement
                /// 
                stSqlQueryText += "\r\n INSERT INTO " + EvFormRecordValues._TBL_FORM_RECORD_VALUES + " ("
                 + EvFormRecordValues._DB_FORM_RECORD_VALUE_GUID + ", "
                 + EvFormFields._DB_FORM_FIELD_GUID + ", "
                 + EvFormRecords._DB_FORM_RECORD_GUID + ", "
                 + EvFormRecordValues._DB_FORM_RECORD_VALUE_ROW + ", "
                 + EvFormRecordValues._DB_FORM_RECORD_VALUE_COLUMN + ", "
                 + EvFormRecordValues._DB_FORM_RECORD_VALUE_NUMERIC + " ) "
                 + "VALUES ("
                 + "'" + FormField.Guid + "', "
                 + "'" + FormField.FormFieldGuid + "', "
                 + "'" + FormField.RecordGuid + "', "
                 + iRow + ", "
                 + iCol + ", "
                 + stValue + " ); ";
                break;
              }//END numeric data type

            case Evado.Model.Forms.EfFormFieldTableColumnTypes.Yes_No:
              {
                int intValue = 0;

                if ( Evado.Model.EvStatics.getBool ( FormField.Table.Rows [ iRow ].Column [ iCol ] ) == true )
                {
                  intValue = 1;
                }
                //
                // Add the cell value
                //
                stSqlQueryText += "\r\n INSERT INTO " + EvFormRecordValues._TBL_FORM_RECORD_VALUES + " ("
                 + EvFormRecordValues._DB_FORM_RECORD_VALUE_GUID + ", "
                 + EvFormFields._DB_FORM_FIELD_GUID + ", "
                 + EvFormRecords._DB_FORM_RECORD_GUID + ", "
                 + EvFormRecordValues._DB_FORM_RECORD_VALUE_ROW + ", "
                 + EvFormRecordValues._DB_FORM_RECORD_VALUE_COLUMN + ", "
                 + EvFormRecordValues._DB_FORM_RECORD_VALUE_NUMERIC + " ) "
                 + "VALUES ("
                 + "'" + FormField.Guid + "', "
                 + "'" + FormField.FormFieldGuid + "', "
                 + "'" + FormField.RecordGuid + "', "
                 + iRow + ", "
                 + iCol + ", "
                 + intValue + " ); ";
                break;
              }//END boolean data type

            case Evado.Model.Forms.EfFormFieldTableColumnTypes.Date:
              {
                //
                // Add the cell value
                //
                stSqlQueryText += "\r\n INSERT INTO " + EvFormRecordValues._TBL_FORM_RECORD_VALUES + " ("
                  + EvFormRecordValues._DB_FORM_RECORD_VALUE_GUID + ", "
                  + EvFormFields._DB_FORM_FIELD_GUID + ", "
                  + EvFormRecords._DB_FORM_RECORD_GUID + ", "
                  + EvFormRecordValues._DB_FORM_RECORD_VALUE_ROW + ", "
                  + EvFormRecordValues._DB_FORM_RECORD_VALUE_COLUMN + ", "
                  + EvFormRecordValues._DB_FORM_RECORD_VALUE_DATE + " ) "
                  + "VALUES ("
                  + "'" + FormField.Guid + "', "
                  + "'" + FormField.FormFieldGuid + "', "
                  + "'" + FormField.RecordGuid + "', "
                  + iRow + ", "
                  + iCol + ", "
                  + "'" + FormField.Table.Rows [ iRow ].Column [ iCol ] + "' ); ";
                break;
              }//END date data type
            default:
              {
                //
                // Add the cell value
                //
                stSqlQueryText += "\r\n INSERT INTO " + EvFormRecordValues._TBL_FORM_RECORD_VALUES + " ("
                  + EvFormRecordValues._DB_FORM_RECORD_VALUE_GUID + ", "
                  + EvFormFields._DB_FORM_FIELD_GUID + ", "
                  + EvFormRecords._DB_FORM_RECORD_GUID + ", "
                  + EvFormRecordValues._DB_FORM_RECORD_VALUE_ROW + ", "
                  + EvFormRecordValues._DB_FORM_RECORD_VALUE_COLUMN + ", "
                  + EvFormRecordValues._DB_FORM_RECORD_VALUE_STRING + " ) "
                  + "VALUES ("
                  + "'" + FormField.Guid + "', "
                  + "'" + FormField.FormFieldGuid + "', "
                  + "'" + FormField.RecordGuid + "', "
                  + iRow + ", "
                  + iCol + ", "
                  + "'" + FormField.Table.Rows [ iRow ].Column [ iCol ] + "' ); ";
                break;
              }//END numeric data type

          }//END Column data type swith

        }//END column iteration loop

      }// END row iteration loop

      return stSqlQueryText;

    }//END addTable method

    // +++++++++++++++++++++++++++ END ITEM UPDATE SECTION ++++++++++++++++++++++++++++++
    #endregion

    // ++++++++++++++++++++++++++++++++++  END Records CLASS +++++++++++++++++++++++++++++++++++
  }

} // Close namespace Evado.Dal.Forms
