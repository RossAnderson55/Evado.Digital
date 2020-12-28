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
  public class EvFormRecordFields : EvDalBase
  {
    #region class initialisation method.
    /// <summary>
    /// This method initialises the schedule DAL class.
    /// </summary>
    public EvFormRecordFields ( )
    {
      this.ClassNameSpace = "Evado.Dal.Clinical.EvFormRecordFields.";
    }

    /// <summary>
    /// This method initialises the schedule DAL class.
    /// </summary>
    public EvFormRecordFields ( EvClassParameters Settings )
    {
      this.ClassParameters = Settings;
      this.ClassNameSpace = "Evado.Dal.Clinical.EvFormRecordFields.";

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

    /// <summary>
    /// This contant defines the database record field item guid.
    /// </summary>
    public const string DB_RECORD_FIELDS_GUID = "TRI_GUID";
    /// <summary>
    /// This constant defines the database record field guid
    /// </summary>
    public const string DB_FORM_FIELDS_GUID = "TCI_GUID";

    /// <summary>
    /// This constant defines a parameter for global unique identifier of form record fields object
    /// </summary>
    private const string PARM_Guid = "@Guid";

    /// <summary>
    /// This constant defines a parameter for record globale unique identifier of form record fields object
    /// </summary>
    private const string PARM_RecordGuid = "@RecordGuid";

    /// <summary>
    /// This constant defines a parameter for formfield global unique identifier of form record fields object
    /// </summary>
    private const string PARM_FormFieldGuid = "@FormFieldGuid";

    /// <summary>
    /// This constant defines a parameter for item text of form record fields object
    /// </summary>
    private const string PARM_StringValue = "@StringValue";

    /// <summary>
    /// This constant defines a parameter for item text of form record fields object
    /// </summary>
    private const string PARM_NumericValue = "@NumericValue";

    /// <summary>
    /// This constant defines a parameter for text value of form record fields object
    /// </summary>
    private const string PARM_TextValue = "@TextValue";

    /// <summary>
    /// This constant defines a parameter for table of form record fields object
    /// </summary>
    private const string PARM_Table = "@Table";

    /// <summary>
    /// This constant defines a parameter for annotation of form record fields object
    /// </summary>
    private const string PARM_Annotation = "@Annotation";

    /// <summary>
    /// This constant defines a parameter for state of form record fields object
    /// </summary>
    private const string PARM_State = "@State";

    /// <summary>
    /// This constant defines a parameter for user identifier of those whot authors form record fields object
    /// </summary>
    private const string PARM_AuthoredByUserId = "@AuthoredByUserId";

    /// <summary>
    /// This constant defines a parameter for user who authors form record fields object
    /// </summary>
    private const string PARM_AuthoredBy = "@AuthoredBy";

    /// <summary>
    /// This constant defines a parameter for authored date of form record fields object
    /// </summary>
    private const string PARM_AuthoredDate = "@AuthoredDate";

    /// <summary>
    /// This constant defines a parameter for user identifier of those who reviews form record fields object
    /// </summary>
    private const string PARM_ReviewedByUserId = "@ReviewedByUserId";

    /// <summary>
    /// This constant defines a parameter for user who reviews form record fields object
    /// </summary>
    private const string PARM_ReviewedBy = "@ReviewedBy";

    /// <summary>
    /// This constant defines a parameter for reviewed date of form record fields object
    /// </summary>
    private const string PARM_ReviewedDate = "@ReviewedDate";

    /// <summary>
    /// This constant defines a parameter for user identifier of those who updates form record fields object
    /// </summary>
    private const string PARM_UpdatedByUserId = "@UpdatedByUserId";

    /// <summary>
    /// This constant defines a parameter for user who updates form record fields object
    /// </summary>
    private const string PARM_UpdatedBy = "@UpdatedBy";

    /// <summary>
    /// This constant defines a parameter for updated date of form record fields object
    /// </summary>
    private const string PARM_UpdateDate = "@UpdateDate";

    /// <summary>
    /// This constant defines a parameter for of form record fields object
    /// </summary>
    private const string PARM_BookedOut = "@BookedOutBy";

    /// <summary>
    /// This constant defines a parameter for trial identifier of form record fields object
    /// </summary>
    private const string PARM_TrialId = "@TrialId";

    /// <summary>
    /// This constant defines a parameter for form identifier of form record fields object
    /// </summary>
    private const string PARM_FormId = "@FormId";

    /// <summary>
    /// This constant defines a parameter for field identifier of form record fields object
    /// </summary>
    private const string PARM_FieldId = "@FieldId";

    /// <summary>
    /// This constant defines a parameter for milestone identifier of form record fields object
    /// </summary>
    private const string PARM_SubjectId = "@SubjectId";

    /// <summary>
    /// This constant defines a parameter for trial visit identifier of form record fields object
    /// </summary>
    private const string PARM_TrialVisitId = "@TrialVisitId";

    /// <summary>
    /// This constant defines a parameter for milestone identifier of form record fields object
    /// </summary>
    private const string PARM_MilestoneId = "@MilestoneId";

    /// <summary>
    /// This constant defines a parameter for arm index of form record fields object
    /// </summary>
    private const string PARM_ScheduleId = "@ScheduleId";
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

    #region Set Query Parameters

    // =====================================================================================
    /// <summary>
    /// This class defines the SQL parameter for a query. 
    /// </summary>
    /// <returns>SqlParameter: an array of sql query parameters</returns>
    /// <remarks>
    /// This method consists of the following step:
    /// 
    /// 1. Create an array of sql query parameters. 
    /// 
    /// 2. Return an array of sql query parameters. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private SqlParameter [ ] GetParameters ( )
    {
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( PARM_Guid, SqlDbType.UniqueIdentifier ),
        new SqlParameter( PARM_RecordGuid, SqlDbType.UniqueIdentifier ),
        new SqlParameter( PARM_FormFieldGuid, SqlDbType.UniqueIdentifier ),
        new SqlParameter( PARM_StringValue, SqlDbType.NVarChar, 500 ),
        new SqlParameter( PARM_NumericValue, SqlDbType.Float ),
        new SqlParameter( PARM_TextValue, SqlDbType.NText),
        new SqlParameter( PARM_Annotation, SqlDbType.NText),
        new SqlParameter( PARM_State, SqlDbType.VarChar, 25 ),
        new SqlParameter( PARM_AuthoredByUserId, SqlDbType.NVarChar, 50 ),
        new SqlParameter( PARM_AuthoredBy, SqlDbType.NVarChar, 100 ),
        new SqlParameter( PARM_AuthoredDate, SqlDbType.DateTime),

        new SqlParameter( PARM_ReviewedByUserId, SqlDbType.NVarChar, 50 ),
        new SqlParameter( PARM_ReviewedBy, SqlDbType.NVarChar, 100 ),
        new SqlParameter( PARM_ReviewedDate, SqlDbType.DateTime),
        new SqlParameter( PARM_UpdatedByUserId, SqlDbType.NVarChar, 100 ),
        new SqlParameter( PARM_UpdatedBy, SqlDbType.NVarChar, 100 ),
        new SqlParameter( PARM_UpdateDate, SqlDbType.DateTime ),
        new SqlParameter( PARM_BookedOut, SqlDbType.NVarChar, 100 ),
      };

      return cmdParms;
    }//END GetParameters class

    // =====================================================================================
    /// <summary>
    /// This class sets the query parameter values. 
    /// </summary>
    /// <param name="cmdParms">SqlParameter: an array of sql query parameters</param>
    /// <param name="RecordField">EvFormField: a formfield data object</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. if the data type is a table then seriallise the formfield object.
    /// 
    /// 2. Update the values from formfield object to the array of sql query parameters. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private void SetParameters ( SqlParameter [ ] cmdParms, EvFormField RecordField )
    {
      this.LogMethod ( "SetParameters method" );
      //
      // Initialize the serialized table structure string and a numeric field value
      //
      string serialisedTableStructure = String.Empty;
      float numericValue = EvStatics.CONST_NUMERIC_EMPTY;

      switch ( RecordField.TypeId )
      {
        case EvDataTypes.Numeric:
        case EvDataTypes.Integer:
        case EvDataTypes.Analogue_Scale:
        case EvDataTypes.Computed_Field:
        case EvDataTypes.Radio_Button_List:
        case EvDataTypes.Selection_List:
        case EvDataTypes.Horizontal_Radio_Buttons:
          {
            String value = RecordField.ItemValue;
            if ( value == String.Empty )
            {
              break;
            }

            value = value.Replace ( "<", "" );
            value = value.Replace ( ">", "" );
            value = value.Trim ( );

            value = EvStatics.convertTextNullToNumNull ( value );

            if ( EvStatics.isNumber ( value ) == true )
            {
              numericValue = EvStatics.getFloat ( RecordField.ItemValue );
            }
            break;
          }
      }

      // 
      // Add the values to parameters
      // 
      cmdParms [ 0 ].Value = RecordField.Guid;
      cmdParms [ 1 ].Value = RecordField.RecordGuid;
      cmdParms [ 2 ].Value = RecordField.FormFieldGuid;
      cmdParms [ 3 ].Value = RecordField.ItemValue;
      cmdParms [ 4 ].Value = numericValue;

      cmdParms [ 5 ].Value = RecordField.ItemText;

      //
      // if the data type is a table then seriallise the formfield object.
      //
      if ( RecordField.Design.TypeId == Evado.Model.EvDataTypes.Special_Matrix
        || RecordField.Design.TypeId == Evado.Model.EvDataTypes.Table )
      {
        serialisedTableStructure = Evado.Model.EvStatics.SerialiseObject<EvFormFieldTable> ( RecordField.Table );
        cmdParms [ 5 ].Value = serialisedTableStructure;
        cmdParms [ 3 ].Value = String.Empty;
      }

      if ( RecordField.Design.TypeId == Evado.Model.EvDataTypes.Signature )
      {
        this.LogDebug ( "Encrypting the Signature value" );

        EvEncrypt encrypt = new EvEncrypt ( this.ClassParameters.ApplicationGuid, RecordField.Guid );

        string encryptedSignature = encrypt.encryptString ( RecordField.ItemText );

        cmdParms [ 5 ].Value = encryptedSignature;
        cmdParms [ 3 ].Value = String.Empty;
      }

      cmdParms [ 6 ].Value = String.Empty;
      cmdParms [ 7 ].Value = RecordField.State.ToString ( );
      cmdParms [ 8 ].Value = RecordField.AuthoredByUserId;
      cmdParms [ 9 ].Value = RecordField.AuthoredBy;
      cmdParms [ 10 ].Value = RecordField.AuthoredDate;
      cmdParms [ 11 ].Value = RecordField.ReviewedByUserId;
      cmdParms [ 12 ].Value = RecordField.ReviewedBy;
      cmdParms [ 13 ].Value = RecordField.ReviewedDate;
      cmdParms [ 14 ].Value = RecordField.UpdatedByUserId;
      cmdParms [ 15 ].Value = RecordField.UserCommonName;
      cmdParms [ 16 ].Value = DateTime.Now;
      cmdParms [ 17 ].Value = RecordField.BookedOutBy;

      this.LogMethodEnd ( "SetParameters" );
    }//END SetParameters method.

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
    private EvFormField getRowData (
      DataRow Row )
    {
      //this.LogMethod ( "getRowData method" );
      // 
      // Initialise method template table string, a return formfield object and an annotation string. 
      // 
      string stTemplateTable = String.Empty;
      EvFormField recordField = new EvFormField ( );

      // 
      // Fill the evForm object.l
      //
      recordField.Guid = EvSqlMethods.getGuid ( Row, EvFormRecordFields.DB_RECORD_FIELDS_GUID );
      recordField.RecordGuid = EvSqlMethods.getGuid ( Row, EvFormRecords.DB_RECORDS_GUID );
      recordField.FormGuid = EvSqlMethods.getGuid ( Row, EvFormRecords.DB_FORMS_GUID );
      recordField.FormFieldGuid = EvSqlMethods.getGuid ( Row, EvFormRecordFields.DB_FORM_FIELDS_GUID );

      recordField.TrialId = EvSqlMethods.getString ( Row, "TrialId" );
      recordField.FormId = EvSqlMethods.getString ( Row, "FormId" );
      recordField.FieldId = EvSqlMethods.getString ( Row, "FieldId" );
      String value = EvSqlMethods.getString ( Row, "TCI_TypeId" );

      switch ( value.ToLower ( ) )
      {
        case "boolean":
          {
            recordField.TypeId = EvDataTypes.Yes_No;
            break;
          }
        case "check_button_list":
          {
            recordField.TypeId = EvDataTypes.Check_Box_List;
            break;
          }
        case "matrix":
          {
            recordField.TypeId = EvDataTypes.Special_Matrix;
            break;
          }
        case "medication_summary":
        case "special_medication_summar":
          {
            recordField.TypeId = EvDataTypes.Special_Medication_Summary;
            break;
          }
        case "subject_demographics":
        case "special_subject_demograph":
        case "demographics":
          {
            recordField.TypeId = EvDataTypes.Special_Subject_Demographics;
            break;
          }
        default:
          {
            recordField.TypeId = Evado.Model.EvStatics.Enumerations.parseEnumValue<Evado.Model.EvDataTypes> ( value );
            break;
          }
      }
      recordField.Design.Title = EvSqlMethods.getString ( Row, "TCI_Subject" );
      recordField.Design.Unit = EvSqlMethods.getString ( Row, "TCI_Unit" );

      //
      // Extract the form design component set the member values.
      //
      string xmlDesign = EvSqlMethods.getString ( Row, "TCI_XmlData" );
      if ( xmlDesign != string.Empty )
      {
        xmlDesign = xmlDesign.Replace ( "<TypeId />", "<TypeId>" + recordField.TypeId + "</TypeId>" );
        xmlDesign = xmlDesign.Replace ( "Check_Button_List", "Check_Box_List" );
        xmlDesign = xmlDesign.Replace ( "Subject_Demographics", "Special_Subject_Demographics" );
        xmlDesign = xmlDesign.Replace ( "Medication_Summary", "Special_Medication_Summary" );
        xmlDesign = xmlDesign.Replace ( "Matrix", "Special_Matrix" );

        recordField.Design = Evado.Model.EvStatics.DeserialiseObject<EvFormFieldDesign> ( xmlDesign );
      }
      else
      {
        recordField.Design.UnitScaling = EvSqlMethods.getString ( Row, "TCI_UNIT_SCALING" );
        recordField.Design.ExSelectionListId = EvSqlMethods.getString ( Row, "TCI_EX_SELECTION_LIST_ID" );
        recordField.Design.FieldCategory = EvSqlMethods.getString ( Row, "TCI_FIELD_CATEGORY" );
        recordField.Design.DefaultValue = EvSqlMethods.getString ( Row, "TCI_DEFAULT_VALUE" );
        recordField.Design.SelectByCodingValue = EvSqlMethods.getBool ( Row, "TCI_SELECT_BY_CODING_VALUE" );
        recordField.Design.SummaryField = EvSqlMethods.getBool ( Row, "TCI_SUMMARY_FIELD" );
        recordField.Design.MultiLineTextField = EvSqlMethods.getBool ( Row, "TCI_MULTI_LINE_TEXT_FIELD" );
        recordField.Design.HorizontalButtons = EvSqlMethods.getBool ( Row, "TCI_HORIZONTAL_BUTTONS" );
        recordField.Design.FormIds = EvSqlMethods.getString ( Row, "TCI_FORM_IDS" );
        recordField.Design.InitialOptionList = EvSqlMethods.getString ( Row, "TCI_INITIAL_OPTION_LIST" );
        recordField.Design.Options = EvSqlMethods.getString ( Row, "TCI_OPTIONS" );
        recordField.Design.Section = EvSqlMethods.getString ( Row, "TCI_SECTION" );
        recordField.Design.Instructions = EvSqlMethods.getString ( Row, "TCI_INSTRUCTIONS" );
        recordField.Design.HttpReference = EvSqlMethods.getString ( Row, "TCI_Reference" );

        value = EvSqlMethods.getString ( Row, "TCI_ANALOGUE_SCALE" );
        if ( value != String.Empty )
        {
          recordField.Design.AnalogueScale = Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue<EvFormField.AnalogueScaleOptions> ( value );
        }
        recordField.Design.AnalogueLegendStart = EvSqlMethods.getString ( Row, "TCI_ANALOGUE_LEGEND_START" );
        recordField.Design.AnalogueLegendFinish = EvSqlMethods.getString ( Row, "TCI_ANALOGUE_LEGEND_FINISH" );
      }
      recordField.Design.JavaScript = EvSqlMethods.getString ( Row, "TCI_JAVA_SCRIPT" );

      //
      // Extract the form fields validation rules
      //
      string stValidationRules = EvSqlMethods.getString ( Row, "TCI_XmlValidationRules" );
      if ( stValidationRules != string.Empty )
      {
        recordField.ValidationRules =
          Evado.Model.Digital.EvcStatics.DeserialiseObject<EvFormFieldValidationRules> ( stValidationRules );

        // 
        // If the selection newField is null
        // 
        if ( ( recordField.TypeId == Evado.Model.EvDataTypes.Selection_List
            || recordField.TypeId == Evado.Model.EvDataTypes.External_Selection_List
            || recordField.TypeId == Evado.Model.EvDataTypes.Check_Box_List
            || recordField.TypeId == Evado.Model.EvDataTypes.Radio_Button_List )
          && recordField.ValidationRules.NotValidOptions == null )
        {
          recordField.ValidationRules.NotValidOptions = new EvFormFieldValidationNotValid ( );
        }

      }//END string exists

      else
      {
        recordField.ValidationRules = new EvFormFieldValidationRules ( );

        recordField.ValidationRules.ValidationLowerLimit = EvSqlMethods.getFloat ( Row, "TCI_ValidationLowerLimit" );
        recordField.ValidationRules.ValidationLowerLimit = EvSqlMethods.getFloat ( Row, "TCI_ValidationUpperLimit" );
        recordField.ValidationRules.AlertLowerLimit = EvSqlMethods.getFloat ( Row, "TCI_AlertLowerLimit" );
        recordField.ValidationRules.AlertLowerLimit = EvSqlMethods.getFloat ( Row, "TCI_AlertLowerLimit" );
        recordField.ValidationRules.NormalRangeLowerLimit = EvSqlMethods.getFloat ( Row, "TCI_NUM_NORM_LOWER_LIMIT" );
        recordField.ValidationRules.NormalRangeUpperLimit = EvSqlMethods.getFloat ( Row, "TCI_NUM_NORM_UPPER_LIMIT" );

        recordField.ValidationRules.NotValidForMale = EvSqlMethods.getBool ( Row, "TCI_NOT_VALID_FOR_MALES" );
        recordField.ValidationRules.NotValidForFemale = EvSqlMethods.getBool ( Row, "TCI_NOT_VALID_FOR_FEMALES" );
        recordField.ValidationRules.IsAfterBirthDate = EvSqlMethods.getBool ( Row, "TCI_AFTER_DATE_OF_BIRTH" );
        recordField.ValidationRules.IsAfterConsentDate = EvSqlMethods.getBool ( Row, "TCI_AFTER_CONSENT_DATE" );

        recordField.ValidationRules.WithinDaysOfRecordDate = EvSqlMethods.getInteger ( Row, "TCI_DAYS_FROM_RECORD_DATE" );
        recordField.ValidationRules.WithinDaysOfVisitDate = EvSqlMethods.getInteger ( Row, "TCI_DAYS_FROM_VISIT_DATE" );

        recordField.ValidationRules.NotValid.EncodedRules = EvSqlMethods.getString ( Row, "TCI_FIELD_NOT_VALID" );
        recordField.ValidationRules.NotValidOptions.EncodedRules = EvSqlMethods.getString ( Row, "TCI_FIELD_OPTIONS_NOT_VALID" );
      }

      recordField.Design.Order = EvSqlMethods.getInteger ( Row, "TCI_Order" );
      recordField.Design.Mandatory = EvSqlMethods.getBool ( Row, "TCI_Mandatory" );
      recordField.Design.HideField = EvSqlMethods.getBool ( Row, "TCI_HideField" );

      recordField.ItemValue = EvSqlMethods.getString ( Row, "TRI_TextValue" );
      recordField.ItemText = EvSqlMethods.getString ( Row, "TRI_ItemText" );

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

      _AnnotationText = EvSqlMethods.getString ( Row, "TRI_Annotation" );
      recordField.AuthoredByUserId = EvSqlMethods.getString ( Row, "TRI_AuthoredByUserId" );
      recordField.AuthoredBy = EvSqlMethods.getString ( Row, "TRI_AuthoredBy" );
      recordField.AuthoredDate = EvSqlMethods.getDateTime ( Row, "TRI_AuthoredDate" );
      recordField.ReviewedByUserId = EvSqlMethods.getString ( Row, "TRI_ReviewedByUserId" );
      recordField.ReviewedBy = EvSqlMethods.getString ( Row, "TRI_ReviewedBy" );
      recordField.ReviewedDate = EvSqlMethods.getDateTime ( Row, "TRI_ReviewedDate" );

      recordField.State = Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue<EvFormField.FieldStates> (
        EvSqlMethods.getString ( Row, "TRI_State" ) );

      recordField.UpdatedByUserId = EvSqlMethods.getString ( Row, "TRI_UpdatedByUserId" );
      recordField.Updated = EvSqlMethods.getString ( Row, "TRI_UpdatedBy" );
      recordField.Updated += " on " + EvSqlMethods.getDateTime ( Row, "TRI_UpdateDate" ).ToString ( "dd MMM yyyy HH:mm" );
      recordField.BookedOutBy = EvSqlMethods.getString ( Row, "TRI_BookedOutBy" );

      if ( recordField.TypeId == Evado.Model.EvDataTypes.Date )
      {
        recordField.ItemText = String.Empty;
      }
      // 
      // Ensure that the formfield state is not null.
      // 
      if ( recordField.State == EvFormField.FieldStates.Null )
      {
        recordField.State = EvFormField.FieldStates.Empty;
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
      // Update the formfield state to current state enumeration.
      //
      if ( ( int ) recordField.State < 0 )
      {
        recordField.State = ( EvFormField.FieldStates ) Math.Abs ( ( int ) recordField.State );
      }

      // 
      // Update the formfield type to current type enumeration.
      // 
      if ( ( int ) recordField.TypeId < 0 )
      {
        recordField.TypeId = ( Evado.Model.EvDataTypes ) Math.Abs ( ( int ) recordField.TypeId );
      }

      //
      // Return the formfield object. 
      //
      return recordField;

    }//END getRowData method.

    /*
    // =====================================================================================
    /// <summary>
    /// This method reads in a record field value
    /// </summary>
    /// <param name="Row">DataRow: an Sql DataReader object</param>
    /// <param name="RecordField">EvFormField: the record field object</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Reads in the value. 
    /// 
    /// 2. Updates the relevant record field value object
    /// 
    /// 3. Exit the method
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private void getRowDataValue (
      DataRow Row,
      EvFormField RecordField )
    {
      //
      // Initialise the methods variables and objects.
      //
      String stringValue = String.Empty;
      String columnValue = String.Empty;

      //
      // User the data type switch to select the value to be retrieves.
      //
      switch ( RecordField.TypeId )
      {
        case EvDataTypes.Check_Box_List:
          {
            columnValue = EvSqlMethods.getString ( Row, EvFormRecordFields.DBV_COLUMN );
            stringValue = EvSqlMethods.getString ( Row, EvFormRecordFields.DBV_VALUE );

            foreach ( EvOption option in RecordField.Design.OptionList )
            {
              if ( option.Value == columnValue
                && stringValue == "1" )
              {
                if ( RecordField.ItemValue != string.Empty )
                {
                  RecordField.ItemValue += ";";
                }
                RecordField.ItemValue += option.Value;
              }
            }

            return;
          }
        case EvDataTypes.Table:
        case EvDataTypes.Special_Matrix:
          {
            //
            // indexes are not ZERO based, value 0 indicates empty value.
            //
            columnValue = EvSqlMethods.getString ( Row, EvFormRecordFields.DBV_COLUMN );
            int row = EvSqlMethods.getInteger ( Row, EvFormRecordFields.DBV_ROW );

            if ( row == 0 || columnValue == String.Empty )
            {
              return;
            }

            //
            // reset the row and column index to zero base values.
            //
            row--;

            //
            // the row index must be less than the length of the row list.
            //
            if ( row >= RecordField.Table.Rows.Count )
            {
              return;
            }

            //
            // read table row data if the column header text matches the column value.
            //
            for ( int column = 0; column < 10; column++ )
            {
              if ( RecordField.Table.Header [ column ].Text == columnValue )
              {
                RecordField.Table.Rows [ row ].Column [ column ] = EvSqlMethods.getString ( Row, EvFormRecordFields.DBV_VALUE );
              }
            }
            return;
          }
        default:
          {
            RecordField.ItemValue = EvSqlMethods.getString ( Row, EvFormRecordFields.DBV_VALUE );
            return;
          }
      }
    }//END getRowDataValue method.
    */

    // =================================================================================
    /// <summary>
    /// This method files a report object with record field monitor report content.
    /// </summary>
    /// <param name="Report">EvReport: a report object.</param>
    /// <param name="Row">DataRow: a data row object containing the query row content</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Get the field type by parsing the enumeration value from the data row item
    /// 
    /// 2. Deserialize the validation rules string. 
    /// 
    /// 3. Update the column values based on the field types including string, check button list and table
    /// </remarks>
    // ----------------------------------------------------------------------------------
    private void getMonitorQueryRowData (
      EvReport Report,
      DataRow Row )
    {
      // 
      // Initialise method variables and objects.
      // 
      String stTemplateTable = String.Empty;
      String stAnnotationText = String.Empty;
      EvFormFieldDesign fieldDesign = new EvFormFieldDesign ( );
      EvFormFieldValidationRules fieldValidationRules = new EvFormFieldValidationRules ( );

      //
      // Get the field type by parsing the enumeration value from the data row item.
      //
      Evado.Model.EvDataTypes fieldTypeId = Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue<Evado.Model.EvDataTypes> (
        EvSqlMethods.getString ( Row, "TCI_TypeId" ) );

      string stDesign = EvSqlMethods.getString ( Row, "TCI_XmlData" );
      if ( stDesign != string.Empty )
      {
        fieldDesign = Evado.Model.Digital.EvcStatics.DeserialiseObject<EvFormFieldDesign> ( stDesign );
      }

      //
      // Deserialize the validation rules string. 
      //
      string stValidationRules = EvSqlMethods.getString ( Row, "TCI_XmlValidationRules" );
      if ( stValidationRules != string.Empty )
      {
        fieldValidationRules =
          Evado.Model.Digital.EvcStatics.DeserialiseObject<EvFormFieldValidationRules> ( stValidationRules );

      }//END string exists.

      //
      // Update the column values based on the field types including string, check button list and table. 
      //
      if ( fieldTypeId != Evado.Model.EvDataTypes.Special_Matrix
        && fieldTypeId != Evado.Model.EvDataTypes.Table
        && fieldTypeId != Evado.Model.EvDataTypes.Check_Box_List )
      {
        //
        // Initialise the report row.
        //
        EvReportRow reportRow = getReportColumn ( Report, Row, "1" );

        //
        // Iterate through the report columns to find a matching column.
        //
        for ( int iColumn = 0; iColumn < Report.Columns.Count; iColumn++ )
        {
          EvReportColumn column = Report.Columns [ iColumn ];

          if ( column.SourceField == "Value" )
          {
            if ( fieldTypeId == Evado.Model.EvDataTypes.Free_Text )
            {
              reportRow.ColumnValues [ iColumn ] = EvSqlMethods.getString ( Row, "TRI_ItemText" );
            }
            else
            {
              reportRow.ColumnValues [ iColumn ] = EvSqlMethods.getString ( Row, "TRI_TextValue" );
            }
          }

        }//END report column iteration loop.

        //
        // Add the report row to the report data records. 
        //
        Report.DataRecords.Add ( reportRow );

      }
      else
      {
        if ( fieldTypeId == Evado.Model.EvDataTypes.Check_Box_List )
        {
          string stValue = EvSqlMethods.getString ( Row, "TRI_ItemText" );

          //
          // Iterate through each of the options in the option list.
          //
          foreach ( EvOption option in fieldDesign.OptionList )
          {
            //
            // Initialise the report row.
            //
            EvReportRow reportRow = getReportColumn ( Report, Row, option.Value );

            //
            // Iterate through the report columns to find a matching column.
            //
            for ( int iColumn = 0; iColumn < Report.Columns.Count; iColumn++ )
            {
              EvReportColumn column = Report.Columns [ iColumn ];

              reportRow.ColumnValues [ iColumn ] = "False";

              if ( stValue.Contains ( option.Value ) == true )
              {
                reportRow.ColumnValues [ iColumn ] = "True";
              }

            }//END report column iteration loop.

            //
            // Add the report row to the report data records. 
            //
            Report.DataRecords.Add ( reportRow );
          }
        }

        // 
        // Process Table data.
        //
        else
        {
          String fieldText = EvSqlMethods.getString ( Row, "TRI_ItemText" );

          EvFormFieldTable table = Evado.Model.Digital.EvcStatics.DeserialiseObject<EvFormFieldTable> ( fieldText );

          //
          // Iterate through the table columns.
          //
          for ( int iColumn = 0; iColumn < table.Header.Length; iColumn++ )
          {
            //
            // If the header exists then process the row.
            //
            if ( table.Header [ iColumn ].Text != string.Empty )
            {
              //
              // Iterate through the rows for the column processing the values.
              //
              for ( int iRow = 0; iRow < table.Header.Length; iRow++ )
              {
                //
                // Initialise the report row.
                //
                EvReportRow reportRow = this.getReportColumn ( Report, Row, iRow + "," + table.Header [ iColumn ].Text );

                //
                // Iterate through the report columns to find a matching column.
                //
                for ( int iReportColumn = 0; iReportColumn < Report.Columns.Count; iReportColumn++ )
                {
                  EvReportColumn column = Report.Columns [ iReportColumn ];

                  reportRow.ColumnValues [ iReportColumn ] = table.Rows [ iRow ].Column [ iColumn ];

                }//END report column iteration loop.

              }//END row iteration loop

            }//END header exists.

          }//END table coluomn iteration loop.

        }//END table field data type

      }//End not single variable field

    }//END getMonitorQueryRowData method.

    // ==================================================================================
    /// <summary>
    /// This method returns a report column selected by the source field name.
    /// </summary>
    /// <param name="Report">EvReport: a report object</param>
    /// <param name="Row">DataRow: data table row</param>
    /// <param name="Instance">String: Instance string</param>
    /// <returns>EvReportRow: a report row data object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Iterate through the report's columns 
    /// 
    /// 2. Extract the matching column with the data row's values to the Report Row object
    /// 
    /// 3. Return the Report Row data object
    /// </remarks>
    // ----------------------------------------------------------------------------------
    private EvReportRow getReportColumn (
      EvReport Report,
      DataRow Row,
      String Instance )
    {
      //
      // Initialise the report row.
      //
      EvReportRow reportRow = new EvReportRow ( Report.Columns.Count );

      //
      // Iterate through the report's columns to find a matching column with the data row's values.
      //
      for ( int iColumn = 0; iColumn < Report.Columns.Count; iColumn++ )
      {
        EvReportColumn column = Report.Columns [ iColumn ];

        if ( column.SourceField == "MilestoneId" )
        {
          reportRow.ColumnValues [ iColumn ] = EvSqlMethods.getString ( Row, "MilestoneId" );
        }

        if ( column.SourceField == "VisitId" )
        {
          reportRow.ColumnValues [ iColumn ] = EvSqlMethods.getString ( Row, "VisitId" );
        }

        if ( column.SourceField == "ActivityId" )
        {
          reportRow.ColumnValues [ iColumn ] = EvSqlMethods.getString ( Row, "ActivityId" );
        }

        if ( column.SourceField == "RecordId" )
        {
          reportRow.ColumnValues [ iColumn ] = EvSqlMethods.getString ( Row, "RecordId" );
        }

        if ( column.SourceField == "FieldId" )
        {
          reportRow.ColumnValues [ iColumn ] = EvSqlMethods.getString ( Row, "FieldId" );
        }

        if ( column.SourceField == "Title" )
        {
          reportRow.ColumnValues [ iColumn ] = EvSqlMethods.getString ( Row, "TCI_Subject" );
        }

        if ( column.SourceField == "TypeId" )
        {
          reportRow.ColumnValues [ iColumn ] = EvSqlMethods.getString ( Row, "TCI_TypeId" );
        }

        if ( column.SourceField == "TypeId" )
        {
          reportRow.ColumnValues [ iColumn ] = EvSqlMethods.getString ( Row, "TCI_TypeId" );
        }

        if ( column.SourceField == "State" )
        {
          reportRow.ColumnValues [ iColumn ] = EvSqlMethods.getString ( Row, "TRI_State" );
        }
      }

      //
      // return the report row object.
      //
      return reportRow;

    }//END method

    // =====================================================================================
    /// <summary>
    /// This method fills the comment list of formfield object.
    /// </summary>
    /// <param name="RecordGuid">Guid: a retrieved formfield object</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Fill the comment list to formfield object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private void getAllFieldComments (
      Guid RecordGuid )
    {
      this.LogMethod ( "getAllFieldComments method." );
      //
      // skip retrieving comment if they exist. 
      //
      if ( _SkipRetrievingComments == true )
      {
        return;
      }

      //
      // Initialise the methods variable and objects.
      //
      EvFormRecordComments formRecordComments = new EvFormRecordComments ( );

      //
      // Fill the record comment list.
      //
      this._FieldCommandList = formRecordComments.getCommentList (
        RecordGuid,
        Guid.Empty,
        EvFormRecordComment.CommentTypeCodes.Form_Field,
        EvFormRecordComment.AuthorTypeCodes.Not_Set );

      this.LogClass ( formRecordComments.Log );

    }//END fillAllFieldComments method

    // =====================================================================================
    /// <summary>
    /// This method fills the comment list of formfield object.
    /// </summary>
    /// <param name="Field">EvFormField: a retrieved formfield object</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Fill the comment list to formfield object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private void selectFieldComments (
      EvFormField Field )
    {
      //this.LogMethod ( "selectFieldComments method." );
      //
      // Initialize the debug log and a formfield comments object. 
      //
      EvFormRecordComments formRecordComments = new EvFormRecordComments ( );
      Field.CommentList = new List<EvFormRecordComment> ( );

      //
      // append form field comments.
      //
      foreach ( EvFormRecordComment comment in this._FieldCommandList )
      {
        if ( comment.RecordFieldGuid == Field.Guid )
        {
          Field.CommentList.Add ( comment );
        }
      }

      //this.LogDebugValue ( "Field.CommentList.Count: " + Field.CommentList.Count );
      //this.LogMethodEnd ( "selectFieldComments " );

    }//END selectFieldComments method

    // =====================================================================================
    /// <summary>
    /// This method reformats formfield comments into a EvFormRecordComment structure.
    /// </summary>
    /// <param name="Field">EvFormField: a formfield object</param>
    /// <param name="AnnotationText">String: a string containing the annotation</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Validate whether the comment list and annotation text are not empty. 
    /// 
    /// 2. If annotation text is xml, deserialize the comment list 
    /// 
    /// 3. Loop through the comment list and reset the values. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private void updateFieldComments (
      EvFormField Field,
      String AnnotationText )
    {
      //this.LogMethod ( "updateFieldComments method. " );

      //
      // Validate whether the comment list is not empty. 
      //
      if ( Field.CommentList.Count > 0 )
      {
        //this.LogDebugValue ( "DB Comments exist\r\n" );

        return;
      }

      //
      // Validate whether the annotation text is not empty.
      //
      if ( AnnotationText == String.Empty )
      {
        //this.LogDebugValue ( "No Annotations" );
        return;
      }

      //
      // If the annotation is xml, deserialize the annotation text. 
      //
      if ( AnnotationText.Contains ( "<?xml version=" ) == true )
      {
        this.LogDebug ( "XML structure" );
        AnnotationText = AnnotationText.Replace ( "EvFormComment", "EvFormRecordComment" );

        Field.CommentList = Evado.Model.Digital.EvcStatics.DeserialiseObject<List<EvFormRecordComment>> ( AnnotationText );

        //
        // Iterate through the comment list and reset the values
        //
        foreach ( EvFormRecordComment comment in Field.CommentList )
        {
          comment.Content = comment.Content.Trim ( );
          comment.NewComment = true;
          comment.RecordGuid = Field.RecordGuid;
          comment.RecordFieldGuid = Field.Guid;

          if ( comment.Content.Length > 3 )
          {
            string stSearch = comment.Content.Substring ( comment.Content.Length - 3 );

            //this.LogDebugValue ( "Last 3 char: '" + stSearch + "'" );
            if ( stSearch.Contains ( "by" ) == true )
            {
              int inOn = comment.Content.LastIndexOf ( "by" );
              if ( inOn > 0 )
              {
                comment.Content = comment.Content.Substring ( 0, ( inOn ) );
              }
            }
            //this.LogDebugValue ( "Comment: '" + comment.Content + "'" );
          }
        }

        return;
      }
      //this.LogDebugValue ( "Process the text comments." );

      //
      // Convert the comment into the comment with a delimiter
      //
      String stComments = AnnotationText.Replace ( "\r\n", "^" );

      //
      // Convert the comments string into array of comments.
      //
      String [ ] arrComments = stComments.Split ( '^' );
      //this.LogDebugValue ( "There are " + arrComments.Length + "comments" );

      //
      // Iterate through the array of comments
      //
      for ( int i = ( arrComments.Length - 1 ); i >= 0; i-- )
      {
        this.ParseComments ( Field, arrComments [ i ] );
      }

      //
      // Empty the comments array.
      //
      //Field.Annotation = String.Empty;

    }//END updateFieldComments method

    // =====================================================================================
    /// <summary>
    /// This method updates the formfield comment structure.
    /// </summary>
    /// <param name="Field">EvForm object: containing the formfield object</param>
    /// <param name="Comment">String: the comment string</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Extract the date string, user name string and content string. 
    /// 
    /// 2. Update the content type based on delimiter values
    /// 
    /// 3. Update the content body string
    /// 
    /// 4. Append the comment to the list
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private void ParseComments (
      EvFormField Field,
      String Comment )
    {
      //
      // Initialize the debug log, the local variables and objects. 
      //
      this.LogMethod ( "ParseComments method. " );

      const string delimiter_NameStart = " by ";
      const string delimiter_DateStart = " on ";
      const string delimiter_Query = "Value Queried";
      const string delimiter_DM = "Jessie";
      int inLastBy = 0;
      int inLastOn = 0;
      int inDateStart = 0;
      int inNameStart = 0;
      string stName = String.Empty;
      string stDate = String.Empty;
      string stContent = String.Empty;
      DateTime dtValue = Evado.Model.Digital.EvcStatics.CONST_DATE_NULL;
      EvFormRecordComment comment = new EvFormRecordComment ( );

      //
      // If there is no comment there is nothing to process.
      //
      if ( Comment == String.Empty )
      {
        return;
      }

      comment.AuthorType = EvFormRecordComment.AuthorTypeCodes.Record_Author;

      //
      // Extract the date string.
      //
      inLastOn = Comment.LastIndexOf ( delimiter_DateStart );

      inDateStart = inLastOn + delimiter_DateStart.Length;

      stDate = Comment.Substring ( inDateStart );

      this.LogDebug ( "Date string: '" + stDate + "'" );

      //
      // Extract the user name
      //
      inLastBy = Comment.LastIndexOf ( delimiter_NameStart );

      inNameStart = inLastBy + delimiter_NameStart.Length;

      this.LogDebug ( "inLastBy: " + inLastBy + " inNameStart: " + inNameStart );

      if ( ( inLastOn - inNameStart ) > 0 )
      {
        stName = Comment.Substring ( inNameStart, inLastOn - inNameStart );
      }

      this.LogDebug ( "Name string: '" + stName + "'" );

      //
      // Extract the content.
      //
      if ( inLastBy > 0 )
      {
        stContent = Comment.Substring ( 0, inLastBy );
      }
      this.LogDebug ( "Content string: '" + stContent + "'" );

      if ( stContent.Contains ( delimiter_Query ) == true )
      {
        comment.AuthorType = EvFormRecordComment.AuthorTypeCodes.Monitor;
      }

      if ( stName.Contains ( delimiter_DM ) == true )
      {
        comment.AuthorType = EvFormRecordComment.AuthorTypeCodes.Data_Manager;
      }

      //
      // Update the content body string. 
      //
      if ( stContent != String.Empty )
      {
        //this.LogDebugValue ( "Comment: \r\n stContent: " + stContent
        //  + "\r\n Name: " + stName
        //  + "\r\n Date: " + stDate );

        comment.RecordGuid = Field.RecordGuid;
        comment.RecordFieldGuid = Field.Guid;
        comment.Content = stContent.Replace ( "\r\n", " " );
        comment.UserCommonName = stName;
        comment.CommentDate = DateTime.Parse ( stDate );
        comment.NewComment = true;

        //
        // Append the comment to the list.
        //
        Field.CommentList.Add ( comment );
      }

    }//ENd ParseComments method.

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
      EvFormField Field )
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
      EvFormFieldSelectionLists externalCodingLists = new EvFormFieldSelectionLists ( );

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
      EvFormField Field )
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
      // Initialising the methods variabeles and objects.
      //
      //Field.Table = new EvFormFieldTable ( );
      //Field.Design.TypeId = Field.TypeId.ToString ( );

      // 
      // if the Itemtext value is empty then initialise it with the EvFormField table value.
      // 
      if ( Field.ItemText == String.Empty )
      {
        //this.LogDebugValue ( "Reset table value to form default." );
        Field.ItemText = EvSqlMethods.getString ( Row, "TCI_Table" );
      }

      // 
      // Deserialize the formfield item text to the formfield table. 
      // 
      Field.Table = Evado.Model.Digital.EvcStatics.DeserialiseObject<EvFormFieldTable> ( Field.ItemText );

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

          if ( Field.Table.Header [ i ].TypeId != EvFormFieldTableColumnHeader.ItemTypeNumeric )
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
      EvFormField Field )
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
    public List<EvFormField> getRecordFieldList (
      EvForm Record,
      bool IncludeComments )
    {
      this.LogMethod ( "getRecordFieldList method. " );
      this.LogDebug ( "Record.Guid: " + Record.Guid );
      this.LogDebug ( "IncludeComments: " + IncludeComments );
      //
      // Initialise the methods variables and objects.
      //
      List<EvFormField> formFieldList = new List<EvFormField> ( );
      EvFormField recordField = new EvFormField ( );

      // 
      // Validate whether the record Guid is not empty. 
      // 
      if ( Record.Guid == Guid.Empty )
      {
        return formFieldList;
      }

      //
      // get the list of field comments.
      //
      if ( IncludeComments == true )
      {
        this.getAllFieldComments ( Record.Guid );
      }

      // 
      // Define the SQL query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(PARM_RecordGuid, SqlDbType.UniqueIdentifier),
      };
      cmdParms [ 0 ].Value = Record.Guid;

      // 
      // Define the query string.
      // 
      _Sql_QueryString = SQL_QUERY + " WHERE (TR_Guid = @RecordGuid) "
        + "ORDER BY TCI_Order; ";

      this.LogDebug ( _Sql_QueryString );

      // 
      // Execute the query against the database.
      // 
      using ( DataTable table = EvSqlMethods.RunQuery ( _Sql_QueryString, cmdParms ) )
      {
        if ( table.Rows.Count == 0 )
        {
          return formFieldList;
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

          // 
          // Get the object data from the row.
          // 
          recordField = this.getRowData ( row );


          //
          // if skip retrieving comments is selected, fill the comment list and format it
          //
          if ( IncludeComments == true )
          {
            //
            // Select the field comment list.
            //
            this.selectFieldComments ( recordField );

            //
            // Reformats text comments into EvFormRecordComment object structures.
            //
            this.updateFieldComments ( recordField, _AnnotationText );
          }

          // Append the new record field object to the array.
          // 
          formFieldList.Add ( recordField );

        }//ENR record iteration loop.

      }//ENd using statement

      // 
      // Return the formfields list.
      // 
      return formFieldList;

    }//END getRecordFieldList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of options retrieved by the TrialId, FormId and FieldId
    /// </summary>
    /// <param name="TrialId">String:  Project identifier.</param>
    /// <param name="FormId">Stirng:  Form identifier.</param>
    /// <param name="FieldId">String: Form field identifier.</param>
    /// <returns>List of EvOption: a list of option object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Return an empty options list if FormId or TrialId is empty. 
    /// 
    /// 2. Define the sql query parameters and the sql query string. 
    /// 
    /// 3. Execute the sql query string and store the results on data table. 
    /// 
    /// 4. Loop through table and extract data row to the datapoint items object. 
    /// 
    /// 5. Add the object values to the options list. 
    /// 
    /// 6. Return the options list.
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> GetItemValueList (
      String TrialId,
      String FormId,
      String FieldId )
    {
      this.LogMethod ( "GetItemValueList." );
      this.LogDebug ( "TrialId: " + TrialId );
      this.LogDebug ( "FormId: " + FormId );
      this.LogDebug ( "FieldId: " + FieldId );

      //
      // Initialize the method debug log, a return option list and an option object. 
      //
      List<EvOption> list = new List<EvOption> ( );
      EvOption option = new EvOption ( );
      list.Add ( option );

      // 
      // Validate whether the formId and VisitId are not empty. 
      // 
      if ( FormId == String.Empty
        || TrialId == String.Empty )
      {
        return list;
      }

      // 
      // Define the SQL query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter("@trialId", SqlDbType.NVarChar, 10),
        new SqlParameter("@FormId", SqlDbType.NVarChar, 10),
        new SqlParameter("@FieldId", SqlDbType.NVarChar, 10),
      };
      cmdParms [ 0 ].Value = TrialId;
      cmdParms [ 1 ].Value = FormId;
      cmdParms [ 2 ].Value = FieldId;

      // 
      // Define the query string.
      // 
      _Sql_QueryString = SQL_QUERY_VALUE_LIST
        + " WHERE (TrialId = @TrialId) "
        + " AND (FormId = @FormId) "
        + " AND (FieldId = @FieldId) ";

      //_Status += "\r\n" + sqlQueryString;

      // 
      // Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( _Sql_QueryString, cmdParms ) )
      {
        // 
        // Iterate through the results extracting the role information.
        // 
        for ( int count = 0; count < table.Rows.Count; count++ )
        {
          // 
          // Extract the table row
          // 
          DataRow row = table.Rows [ count ];

          // 
          // Process the results into the visitSchedule.
          // 
          option = new EvOption (
             EvSqlMethods.getString ( row, "TRI_TextValue" ),
            EvSqlMethods.getString ( row, "TRI_TextValue" ) );

          // 
          // Append the new ChecklistItem object to the array.
          // 
          list.Add ( option );

        }//END record interation loop.

      }//END using statement

      // 
      // Pass back the result arrray.
      // 
      return list;

    }//END GetItemValueList method.

    #endregion

    #region New Field safety items

    // =====================================================================================
    /// <summary>
    /// This class returns a list of options retrieved by the TrialId and a safety Report items condition
    /// </summary>
    /// <param name="ProjectId">String:  Project identifier.</param>
    /// <param name="SafetyReportItems">Boolean: true, if safety repor item is selected</param>
    /// <returns>List of EvOption: a list of option object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define the sql query parameters and the sql query string. 
    /// 
    /// 2. Execute the sql query string and store the results on data table. 
    /// 
    /// 3. Loop through table and extract data row with 70 Characters trungate to the datapoint items object. 
    /// 
    /// 4. Add the object values to the Options list. 
    /// 
    /// 5. Return the Options list
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> GetItemList ( string ProjectId, bool SafetyReportItems )
    {
      this.LogMethod ( "GetItemList. " );
      this.LogDebug ( "ProjectId: " + ProjectId );

      //
      // Initialize the method debug log, a return option list and an option object. 
      //
      string _sqlQueryString;
      List<EvOption> List = new List<EvOption> ( );
      EvOption option = new EvOption ( );
      List.Add ( option );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(PARM_TrialId, SqlDbType.NVarChar, 10),
      };
      cmdParms [ 0 ].Value = ProjectId;

      // 
      // Generate the SQL query string
      // 
      _sqlQueryString = "Select FormId, FieldId, TCI_Subject, TCI_Unit "
        + "FROM EvRecordField_View "
        + "WHERE (TrialId = @TrialId)  AND (TCI_TypeId = 'Numeric' ) "
        + "GROUP BY FormId, FieldId, TCI_Subject, TCI_Unit ; ";

      if ( SafetyReportItems == true )
      {
        _sqlQueryString = "Select FormId, FieldId, TCI_Subject, TCI_Unit "
          + "FROM EvRecordField_View "
          + "WHERE (TrialId = @TrialId)  AND (TCI_TypeId = 'Numeric' ) "
          + "AND TCI_SafetyReport = 1 "
          + "GROUP BY FormId, FieldId, TCI_Subject, TCI_Unit ; ";
      }
      this.LogDebug ( "" + _sqlQueryString );

      //
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( _sqlQueryString, cmdParms ) )
      {
        // 
        // Iterate through the results extracting the role information.
        // 
        for ( int count = 0; count < table.Rows.Count; count++ )
        {
          // 
          // Extract the table row
          // 
          DataRow row = table.Rows [ count ];

          string unit = EvSqlMethods.getString ( row, "TCI_Unit" );

          string optionId = EvSqlMethods.getString ( row, "FormId" )
            + "^" + EvSqlMethods.getString ( row, "FieldId" );

          string stOption = "(" + EvSqlMethods.getString ( row, "FormId" ) + ") " + EvSqlMethods.getString ( row, "TCI_Subject" );

          // 
          // Trungate the selection to 70 characters.
          // 
          if ( stOption.Length > 70 )
          {
            stOption = stOption.Substring ( 0, 70 ) + " ... ";
          }

          if ( unit != String.Empty )
          {
            stOption += " (" + unit + ")";
          }

          option = new EvOption ( optionId, stOption );

          List.Add ( option );
        }
      }

      // 
      // Return the list containing the Option data object.
      // 
      return List;

    }//END GetItemList method.

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


      this.LogDebug ( "Parameters: " );
      for ( int i = 0; i < Report.Queries.Length; i++ )
      {
        this.LogDebug ( Report.Queries [ i ].SelectionSource + " = " + Report.Queries [ i ].Value );
      }

      //
      // Initialize the Method debug log, a report rows list and a formfield object
      //
      List<EvReportRow> reportRows = new List<EvReportRow> ( );
      EvFormField formField = new EvFormField ( );
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
        new SqlParameter( PARM_TrialId, SqlDbType.NVarChar, 10 ),
        new SqlParameter( PARM_SubjectId, SqlDbType.NVarChar, 20 ),
        new SqlParameter( PARM_FormId, SqlDbType.NVarChar, 10 ),
        new SqlParameter( PARM_FieldId, SqlDbType.NVarChar, 10 ),
      };

      //
      // Extract the parameters from the parameter list.
      //
      for ( int i = 0; i < Report.Queries.Length; i++ )
      {
        if ( Report.Queries [ i ].SelectionSource == EvReport.SelectionListTypes.Current_Trial )
        {
          cmdParms [ 0 ].Value = Report.Queries [ i ].Value;
        }

        if ( Report.Queries [ i ].SelectionSource == EvReport.SelectionListTypes.Subject_Id )
        {
          cmdParms [ 1 ].Value = Report.Queries [ i ].Value;
        }

        if ( Report.Queries [ i ].SelectionSource == EvReport.SelectionListTypes.Form_Id )
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

          this.getMonitorQueryRowData ( Report, row );

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
        reportRow.ColumnValues [ 2 ] = formField.Type;
        reportRow.ColumnValues [ 3 ] = String.Empty;

        Report.DataRecords.Add ( reportRow );

      }//END ADD EMPTY ROW

      // 
      // Return the result array.
      // 
      return Report;
    }

    #endregion

    #region Data Analysis Queries

    // =====================================================================================
    /// <summary>
    /// This class returns a list of Data item object retrieved by the passed parameters.
    /// </summary>
    /// <param name="ProjectId">string: a trial identifier</param>
    /// <param name="FieldId">string: a formfield Identifier</param>
    /// <param name="Grouping">EvChart.GroupingOptions: a Groupsing type</param>
    /// <param name="Aggregation">EvChart.AggregationOptions: Aggregation type</param>
    /// <returns>List of EvDataItem: a list of data item object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Return the list of analysis items instance if the Aggregation options is instance
    /// 
    /// 2. Else return the list of analysis items aggregation. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvDataItem> GetAnalysisItems (
      string ProjectId,
      string FieldId,
      EvChart.GroupingOptions Grouping,
      EvChart.AggregationOptions Aggregation )
    {
      this.LogMethod ( "getAnalysisItems method." );
      this.LogDebug ( "ProjectId: " + ProjectId );
      this.LogDebug ( "FieldId: " + FieldId );
      this.LogDebug ( "Grouping: " + Grouping );
      this.LogDebug ( "Aggregation: " + Aggregation );

      // 
      // Path for instance query.
      // 
      if ( Aggregation == EvChart.AggregationOptions.Instance )
      {
        return this.GetAnalysisItemsInstance (
          ProjectId,
          FieldId,
          Grouping,
          Aggregation );
      }
      else
      {
        // Method for all other aggregations.
        return this.GetAnalysisItemsAggregation (
         ProjectId,
         FieldId,
         Grouping,
         Aggregation );
      }

    }//END GetAnalysisItems method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of analysis data item instance based on the passed parameters. 
    /// </summary>
    /// <param name="TrialId">string: a trial identifier</param>
    /// <param name="FieldId">string: a formfield Identifier</param>
    /// <param name="Grouping">EvChart.GroupingOptions: a Groupsing type</param>
    /// <param name="Aggregation">EvChart.AggregationOptions: Aggregation type</param>
    /// <returns>List of EvDataItem: a list of data item object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define the sql query parameters and the sql query string .
    /// 
    /// 2. Execute the sql query string and store the results on data table. 
    /// 
    /// 3. Loop through the table and extract the compatible data row value to the return object. 
    /// 
    /// 4. Add the object values to the Data items list. 
    /// 
    /// 5. Return the data items list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private List<EvDataItem> GetAnalysisItemsInstance (
      string TrialId,
      string FieldId,
      EvChart.GroupingOptions Grouping,
      EvChart.AggregationOptions Aggregation )
    {
      this.LogMethod ( "getAnalysisItemsInstance. " );
      this.LogDebug ( " TrialId: " + TrialId );
      this.LogDebug ( "FieldId: " + FieldId );
      this.LogDebug ( "Grouping: " + Grouping );
      this.LogDebug ( "Aggregation: " + Aggregation );

      //
      // Initialize the method debug log, a return list of data item and local variables. 
      //
      List<EvDataItem> View = new List<EvDataItem> ( );
      string [ ] itemIDKey = new string [ 1 ];
      if ( FieldId != String.Empty )
      {
        itemIDKey = FieldId.Split ( '^' );
        if ( itemIDKey.Length != 2 )
        {
          this.LogDebug ( "FieldId does not have the correct structure." );
          return View;
        }
      }

      // 
      // Define the SQL query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(PARM_TrialId, SqlDbType.NVarChar, 10),
        new SqlParameter(PARM_FormId, SqlDbType.NVarChar, 10),
        new SqlParameter(PARM_FieldId, SqlDbType.NVarChar, 10),
      };
      cmdParms [ 0 ].Value = TrialId;
      cmdParms [ 1 ].Value = String.Empty;
      cmdParms [ 2 ].Value = String.Empty;

      if ( FieldId != String.Empty )
      {
        cmdParms [ 1 ].Value = itemIDKey [ 0 ];
        cmdParms [ 2 ].Value = itemIDKey [ 1 ];
      }

      // 
      // Define the query string.
      //
      _Sql_QueryString = "Select FieldId, SubjectId, OrgId, TCI_Subject, TRI_NumericValue, TCI_Unit, ";

      if ( Grouping == EvChart.GroupingOptions.Visit )
      {
        _Sql_QueryString += " MilestoneId ";
      }
      else
      {
        _Sql_QueryString += " CONVERT( VARCHAR(12), Date, 106 ) as Date ";
      }

      _Sql_QueryString += "FROM EvRecordField_Analysis "
        + "WHERE (TrialId = @TrialId) "
        + "AND (FormId = @FormId) "
        + "AND (FieldId = @FieldId) ";
      // 
      // Sort the output based on the grouping type.
      // 
      if ( Grouping == EvChart.GroupingOptions.Visit )
      {
        _Sql_QueryString += " ORDER BY SubjectId, MilestoneId ";
      }
      else
      {
        _Sql_QueryString += " ORDER BY SubjectId, Date;";
      }

      this.LogDebug ( "" + _Sql_QueryString );

      // 
      // Execute the query against the database.
      // 
      using ( DataTable table = EvSqlMethods.RunQuery ( _Sql_QueryString, cmdParms ) )
      {
        // 
        // Iterate through the results extracting the role information.
        // 
        for ( int count = 0; count < table.Rows.Count; count++ )
        {
          // 
          // Extract the table row
          // 
          DataRow row = table.Rows [ count ];

          EvDataItem dataItem = new EvDataItem ( );

          String orgId = EvSqlMethods.getString ( row, "OrgId" );
          orgId = orgId.ToLower ( );

          if ( orgId.Contains ( "test" ) == true )
          {
            continue;
          }

          dataItem.FieldId = EvSqlMethods.getString ( row, "FieldId" );

          dataItem.SubjectId = EvSqlMethods.getString ( row, "SubjectId" ) + " ";

          if ( FieldId != String.Empty )
          {
            dataItem.Subject = "(" + itemIDKey [ 0 ] + ")";
          }
          if ( Aggregation != EvChart.AggregationOptions.Instance )
          {
            dataItem.Subject += Aggregation.ToString ( )
              + " of ";
          }
          dataItem.Subject += EvSqlMethods.getString ( row, "TCI_Subject" );

          dataItem.Value = EvSqlMethods.getFloat ( row, "TCI_Subject" );

          dataItem.Unit = EvSqlMethods.getString ( row, "TCI_Unit" );

          if ( Grouping == EvChart.GroupingOptions.Visit )
          {
            dataItem.DataPoint = EvSqlMethods.getString ( row, "MilestoneId" );
          }
          else
          {
            dataItem.DataPoint = EvSqlMethods.getString ( row, "Date" );
          }

          dataItem.Value = EvSqlMethods.getFloat ( row, "TRI_NumericValue" );

          // 
          // Append the new EvChart object to the array.
          // 
          View.Add ( dataItem );

        }//End for iteration loop.


      }
      this.LogDebug ( "view Count: " + View.Count );

      // 
      // Pass back the result arrray.
      // 
      return View;

    }//END GetAnalysisItemsInstance method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of analysis data items aggregation based on the passed parameters. 
    /// </summary>
    /// <param name="TrialId">string: a trial identifier</param>
    /// <param name="FieldId">string: a formfield Identifier</param>
    /// <param name="Grouping">EvChart.GroupingOptions: a Groupsing type</param>
    /// <param name="Aggregation">EvChart.AggregationOptions: Aggregation type</param>
    /// <returns>List of EvDataItem: a list of data item object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define the sql query parameters and the sql query string .
    /// 
    /// 2. Execute the sql query string and store the results on data table. 
    /// 
    /// 3. Loop through the table and extract the compatible data row valueds to the return object. 
    /// 
    /// 4. Add the object values to the Data items list. 
    /// 
    /// 5. Return the Data items list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private List<EvDataItem> GetAnalysisItemsAggregation (
      String TrialId,
      String FieldId,
      EvChart.GroupingOptions Grouping,
      EvChart.AggregationOptions Aggregation )
    {
      this.LogMethod ( "getAnalysisItemsAggregation" );
      this.LogDebug ( "TrialId: " + TrialId );
      this.LogDebug ( "FieldId: " + FieldId );
      this.LogDebug ( "Grouping: " + Grouping );
      this.LogDebug ( "Aggregation: " + Aggregation );
      //
      // Initialize the method debug log, a return list of data item and local variables. 
      //
      List<EvDataItem> View = new List<EvDataItem> ( );
      int dbRows = 0;
      string [ ] itemIDKey = FieldId.Split ( '^' );

      //
      // Validate whether the item identifier key has the correct structure. 
      //
      if ( itemIDKey.Length != 2 )
      {
        this.LogDebug ( "FieldId does not have the correct structure." );
        return View;
      }

      this.LogDebug ( "Form {0}, Field {1}.", itemIDKey [ 0 ], itemIDKey [ 1 ] );
      // 
      // Define the SQL query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(PARM_TrialId, SqlDbType.NVarChar, 10),
        new SqlParameter(PARM_FormId, SqlDbType.NVarChar, 10),
        new SqlParameter(PARM_FieldId, SqlDbType.NVarChar, 10),
      };
      cmdParms [ 0 ].Value = TrialId;
      cmdParms [ 1 ].Value = itemIDKey [ 0 ];
      cmdParms [ 2 ].Value = itemIDKey [ 1 ];

      // 
      // Define the query string.
      //
      _Sql_QueryString = "Select "
        + "CONVERT( VARCHAR(12), Avg( TRI_NumericValue )) AS Value, "
        + "CONVERT( VARCHAR(12), Max( TRI_NumericValue )) AS Max, "
        + "CONVERT( VARCHAR(12), Min( TRI_NumericValue )) AS Min, "
        + "TCI_Unit, FieldId, TCI_Subject, ";

      if ( Grouping == EvChart.GroupingOptions.Visit )
      {
        _Sql_QueryString += " MilestoneId ";
      }
      else
      {
        _Sql_QueryString += " CONVERT( VARCHAR(12), TR_RecordDate, 106 ) AS Date";
      }

      // 
      // Selection criteria
      // 
      _Sql_QueryString += "FROM EvRecordField_Analysis "
        + "WHERE (TrialId = @TrialId) "
        + "AND (FormId = @FormId) "
        + "AND (FieldId = @FieldId) ";

      // 
      // Add the Grouping statements
      // 
      _Sql_QueryString += "GROUP BY ";

      if ( Grouping == EvChart.GroupingOptions.Visit )
      {
        _Sql_QueryString += "MilestoneId, TCI_Unit, FieldId, TCI_Subject ;";
      }
      else if ( Grouping == EvChart.GroupingOptions.Date )
      {
        _Sql_QueryString += "Date, TCI_Unit, FieldId, TCI_Subject ;";
      }

      this.LogDebug ( _Sql_QueryString );

      // 
      // Execute the query against the database.
      // 
      using ( DataTable table = EvSqlMethods.RunQuery ( _Sql_QueryString, cmdParms ) )
      {
        // 
        // Iterate through the results extracting the role information.
        // 
        for ( int count = 0; count < table.Rows.Count; count++ )
        {
          // 
          // Extract the table row
          // 
          DataRow row = table.Rows [ count ];
          EvDataItem dataItem = new EvDataItem ( );

          if ( Aggregation == EvChart.AggregationOptions.Maximum )
          {
            dataItem.Value = EvSqlMethods.getFloat ( row, "Max" );
          }
          else if ( Aggregation == EvChart.AggregationOptions.Minimum )
          {
            dataItem.Value = EvSqlMethods.getFloat ( row, "Min" );
          }
          else
          {
            dataItem.Value = EvSqlMethods.getFloat ( row, "Value" );
          }

          dataItem.Unit = EvSqlMethods.getString ( row, "TCI_Unit" );

          dataItem.FieldId = EvSqlMethods.getString ( row, "FieldId" );

          if ( FieldId != String.Empty )
          {
            dataItem.Subject = "(" + itemIDKey [ 0 ] + ") ";
          }
          if ( Aggregation != EvChart.AggregationOptions.Instance )
          {
            dataItem.Subject += Aggregation.ToString ( )
              + " of ";
          }
          dataItem.Subject += EvSqlMethods.getString ( row, "TCI_Subject" );

          if ( Grouping == EvChart.GroupingOptions.Visit )
          {
            dataItem.DataPoint = EvSqlMethods.getString ( row, "MilestoneId" );
          }
          else
          {
            dataItem.DataPoint = EvSqlMethods.getString ( row, "Date" );
          }

          // 
          // Append the new EvChart object to the array.
          // 
          View.Add ( dataItem );

          dbRows++;
        }
      }
      this.LogDebug ( "Row Count: " + dbRows + " list Count: " + View.Count );
      // 
      // Pass back the result arrray.
      // 
      return View;

    }//END GetAnalysisItemsAggregation method.
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
    private EvFormField GetItem (
      Guid Guid )
    {
      this.LogMethod ( "GetItem method" );
      this.LogDebug ( "Guid: " + Guid );
      // 
      // Initialize the debug log and a return formfield object. 
      // 
      EvFormField field = new EvFormField ( );

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
      SqlParameter cmdParms = new SqlParameter ( PARM_Guid, SqlDbType.UniqueIdentifier );
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
    EvFormObjectStates _RecordState = EvFormObjectStates.Null;

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
      EvForm FormRecord )
    {
      this.LogMethod ( "UpdateFields method " );
      this.LogDebug ( "RecordFieldList.Count: " + FormRecord.Fields.Count );
      this.LogDebug ( "SubmitRecord: " + FormRecord.State );
      // 
      // Initialize the method debug log and the return event code. 
      // 
      EvEventCodes iReturn = EvEventCodes.Ok;
      this._RecordState = FormRecord.State;

      // 
      // Iterate through the formfields object. 
      // 
      foreach ( EvFormField field in FormRecord.Fields )
      {

        if ( field == null )
        {
          //this.LogDebugValue ( "FIELD NULL" );
          continue;
        }

        field.UserCommonName = FormRecord.UserCommonName;
        field.UpdatedByUserId = FormRecord.UpdatedByUserId;

        // 
        // Set the newField state.
        // 
        if ( field.ItemValue == String.Empty
          && field.State == EvFormField.FieldStates.Null )
        {
          field.State = EvFormField.FieldStates.Empty;
        }

        //
        // If Guid is empty, add new field else update field.
        // 
        if ( field.Guid == Guid.Empty )
        {
          this.addField ( field );
        }
        else
        {
          if ( field.Action != String.Empty )
          {
            iReturn = this.updateField ( field );

            if ( iReturn != EvEventCodes.Ok )
            {
              this.LogEvent ( "Guid: " + field.Guid
              + " FieldId: " + field.FieldId
              + " >> '" + field.ItemValue
              + "' S: " + field.State + " >> ERROR UPDATING FIELD." );

              return iReturn;
            }

          }
        }//END Update Action.

      }//END FormField Update Iteration.

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
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Set the skip retrieve comments to true to not retrieve comments
    /// when validating the record field object for saving.
    /// 
    /// 2. Validate whether the recordUid, FormItemUid and UserCommonName and old formfield object have value
    /// 
    /// 3. Define sql query parameters and execute the storeprocedure for updating fields. 
    /// 
    /// 4. Return the event code for updating items
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private EvEventCodes updateField (
      EvFormField RecordField )
    {
      this.LogMethod ( "updateField method. " );
      this.LogDebug ( "Guid: " + RecordField.Guid );
      this.LogDebug ( "RecordGuid: " + RecordField.RecordGuid );
      this.LogDebug ( "State: " + RecordField.State );
      this.LogDebug ( "Value: " + RecordField.ItemValue );
      this.LogDebug ( "FieldType: " + RecordField.Design.TypeId );
      // 
      // Initialise the methods variables and objects
      // 
      EvFormRecordComments recordComments = new EvFormRecordComments ( );
      EvEventCodes eventCode = EvEventCodes.Ok;

      //
      // Set the skip retrieve comments to true to not retrieve comments
      // when validating the record field object for saving.
      //
      this._SkipRetrievingComments = true;

      // 
      // Validate whether the recordUid, FormItemUid and UserCommonName and old formfield object have value
      // 
      if ( RecordField.RecordGuid == Guid.Empty )
      {
        this.LogDebug ( "RecordGuid Empty" );

        return EvEventCodes.Identifier_General_ID_Error;
      }

      if ( RecordField.FormFieldGuid == Guid.Empty )
      {
        this.LogDebug ( "FormFieldGuid Empty" );

        return EvEventCodes.Identifier_General_ID_Error;
      }

      if ( RecordField.UserCommonName == String.Empty )
      {
        this.LogDebug ( "UserId Empty" );

        return EvEventCodes.Identifier_User_Id_Error;
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = GetParameters ( );
      this.SetParameters ( cmdParms, RecordField );
      /*
      this.LogDebugValue ( "Parameters:" );
      foreach ( SqlParameter prm in cmdParms )
      {
        this.LogDebugValue ( "Type: " + prm.DbType
          + ", Name: " + prm.ParameterName
          + ", Value: " + prm.Value );
      }
       */
      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( _storedProcedureUpdateItem, cmdParms ) == 0 )
      {
        this.LogDebug ( "Record field database update error." );
        return EvEventCodes.Database_Record_Update_Error;
      }

      switch ( RecordField.Design.TypeId )
      {
        case EvDataTypes.Signature:
        case EvDataTypes.Special_Medication_Summary:
        case EvDataTypes.Special_Subject_Demographics:
        case EvDataTypes.Streamed_Video:
        case EvDataTypes.External_Image:
        case EvDataTypes.Html_Content:
        case EvDataTypes.Html_Link:
        case EvDataTypes.Image:
        case EvDataTypes.Read_Only_Text:
          {
            return EvEventCodes.Ok;
          }
      }

      if ( RecordField.Design.TypeId != EvDataTypes.Free_Text )
      {
        eventCode = this.UpdateFieldValues ( RecordField );

        if ( eventCode != EvEventCodes.Ok )
        {
          this.LogDebug ( "Field value update error." );
          return eventCode;
        }
      }

      //
      // Add record comments to event code. 
      //
      eventCode = recordComments.addNewComments ( RecordField.CommentList );

      this.LogDebug ( "Field Comment status: " + recordComments.Log );
      // 
      // Return event exit code .
      // 
      return eventCode;

    }//END updateField method

    // =====================================================================================
    /// <summary>
    /// This class adds new items to formfield table. 
    /// </summary>
    /// <param name="RecordField">EvFormField: a retrieved formfield data object</param>
    /// <returns>EvEventCodes: an event code for adding items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the recordUid is not defined or RecordGuid is empty. 
    /// 
    /// 2. Exit, if the FormItemUid is not defined or FormFieldGuid is empty
    /// 
    /// 3. Exit, if the usercommon name is empty. 
    /// 
    /// 4. Define the sql query parameters and execute the storeprocedure for adding items. 
    /// 
    /// 5. Return the event code for adding items. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes UpdateFieldValues ( EvFormField RecordField )
    {
      this.LogMethod ( "UpdateFieldValues method. " );
      //this.LogDebugValue ( "RecordFieldGuid: " + RecordField.Guid );

      //
      // Only update the record data values if the record has been submitted or locked.
      //
      if ( this._RecordState != EvFormObjectStates.Submitted_Record
        && this._RecordState != EvFormObjectStates.Locked_Record )
      {
        return EvEventCodes.Ok;
      }

      //
      // Initialize the debug log and the return event code. 
      //
      System.Text.StringBuilder SqlUpdateQuery = new System.Text.StringBuilder ( );

      //
      // Delete the milestone activities for this milestone.
      //
      SqlUpdateQuery.AppendLine ( "/** DELETE ALL OF FIEDL VALUES **/" );
      SqlUpdateQuery.AppendLine ( " DELETE FROM EV_RECORD_FIELD_VALUES " );
      SqlUpdateQuery.AppendLine ( " WHERE  TRI_GUID = '" + RecordField.Guid + "' ; " );
      SqlUpdateQuery.AppendLine ( "" );

      String value = RecordField.ItemValue.Replace ( "'", String.Empty );
      String text = RecordField.ItemText.Replace ( "'", String.Empty );

      switch ( RecordField.TypeId )
      {
        case EvDataTypes.Check_Box_List:
          {
            //
            // Iterate through each option setting the selection value into the database.
            //
            foreach ( EvOption option in RecordField.Design.OptionList )
            {
              //
              // create the column key, which must be less thatn 50 char
              //
              String columnKey = option.Value;
              columnKey = columnKey.Replace ( " ", String.Empty );
              if ( columnKey.Length > Evado.Model.Digital.EvcStatics.CONST_FIELD_VALUE_KEY_LENGTH )
              {
                columnKey = columnKey.Substring ( 0, Evado.Model.Digital.EvcStatics.CONST_FIELD_VALUE_KEY_LENGTH );
              }

              if ( RecordField.ItemValue.Contains ( option.Value ) == true )
              {
                SqlUpdateQuery.AppendLine ( "/** INSERT CHECK BOX LIST ITEM **/" );
                SqlUpdateQuery.AppendLine ( "Insert Into EV_RECORD_FIELD_VALUES " );
                SqlUpdateQuery.AppendLine ( " (TRI_GUID, " );
                SqlUpdateQuery.AppendLine ( "  TRFV_COLUMN, " );
                SqlUpdateQuery.AppendLine ( "  TRFV_ROW, " );
                SqlUpdateQuery.AppendLine ( "  TRFV_VALUE, " );
                SqlUpdateQuery.AppendLine ( "  TRFV_TEXT )" );
                SqlUpdateQuery.AppendLine ( "values " );
                SqlUpdateQuery.AppendLine ( " ('" + RecordField.Guid + "', " );
                SqlUpdateQuery.AppendLine ( "  '" + columnKey + "', " );
                SqlUpdateQuery.AppendLine ( "  0, " );
                SqlUpdateQuery.AppendLine ( " '1', " );
                SqlUpdateQuery.AppendLine ( " '' ); " );
              }
              else
              {
                SqlUpdateQuery.AppendLine ( "/** INSERT CHECK BOX LIST ITEM **/" );
                SqlUpdateQuery.AppendLine ( "Insert Into EV_RECORD_FIELD_VALUES " );
                SqlUpdateQuery.AppendLine ( " (TRI_GUID, " );
                SqlUpdateQuery.AppendLine ( "  TRFV_COLUMN, " );
                SqlUpdateQuery.AppendLine ( "  TRFV_ROW, " );
                SqlUpdateQuery.AppendLine ( "  TRFV_VALUE, " );
                SqlUpdateQuery.AppendLine ( "  TRFV_TEXT )" );
                SqlUpdateQuery.AppendLine ( "values " );
                SqlUpdateQuery.AppendLine ( " ('" + RecordField.Guid + "', " );
                SqlUpdateQuery.AppendLine ( "  '" + columnKey + "', " );
                SqlUpdateQuery.AppendLine ( "  0, " );
                SqlUpdateQuery.AppendLine ( " '0', " );
                SqlUpdateQuery.AppendLine ( " '' ); " );
              }
            }
            break;
          }
        case EvDataTypes.Integer_Range:
        case EvDataTypes.Date_Range:
        case EvDataTypes.Double_Range:
        case EvDataTypes.Float_Range:
          {
            if ( text.Contains ( ";" ) == true )
            {
              string [ ] arValue = text.Split ( ';' );
              SqlUpdateQuery.AppendLine ( "/** INSERT FREE TEXT VALUE **/" );
              SqlUpdateQuery.AppendLine ( "Insert Into EV_RECORD_FIELD_VALUES " );
              SqlUpdateQuery.AppendLine ( " (TRI_GUID, " );
              SqlUpdateQuery.AppendLine ( "  TRFV_COLUMN, " );
              SqlUpdateQuery.AppendLine ( "  TRFV_ROW, " );
              SqlUpdateQuery.AppendLine ( "  TRFV_VALUE, " );
              SqlUpdateQuery.AppendLine ( "  TRFV_TEXT )" );
              SqlUpdateQuery.AppendLine ( "values " );
              SqlUpdateQuery.AppendLine ( " ('" + RecordField.Guid + "', " );
              SqlUpdateQuery.AppendLine ( " 'LOWER', " );
              SqlUpdateQuery.AppendLine ( "  0, " );
              SqlUpdateQuery.AppendLine ( " '', " );
              SqlUpdateQuery.AppendLine ( " '" + arValue [ 0 ] + "' ); " );

              SqlUpdateQuery.AppendLine ( "/** INSERT FREE TEXT VALUE **/" );
              SqlUpdateQuery.AppendLine ( "Insert Into EV_RECORD_FIELD_VALUES " );
              SqlUpdateQuery.AppendLine ( " (TRI_GUID, " );
              SqlUpdateQuery.AppendLine ( "  TRFV_COLUMN, " );
              SqlUpdateQuery.AppendLine ( "  TRFV_ROW, " );
              SqlUpdateQuery.AppendLine ( "  TRFV_VALUE, " );
              SqlUpdateQuery.AppendLine ( "  TRFV_TEXT )" );
              SqlUpdateQuery.AppendLine ( "values " );
              SqlUpdateQuery.AppendLine ( " ('" + RecordField.Guid + "', " );
              SqlUpdateQuery.AppendLine ( "  'UPPER', " );
              SqlUpdateQuery.AppendLine ( "  0, " );
              SqlUpdateQuery.AppendLine ( " '', " );
              SqlUpdateQuery.AppendLine ( " '" + arValue [ 1 ] + "' ); " );

              break;
            }

            SqlUpdateQuery.AppendLine ( "/** INSERT ITEM VALUE **/" );
            SqlUpdateQuery.AppendLine ( "Insert Into EV_RECORD_FIELD_VALUES " );
            SqlUpdateQuery.AppendLine ( " (TRI_GUID, " );
            SqlUpdateQuery.AppendLine ( "  TRFV_COLUMN, " );
            SqlUpdateQuery.AppendLine ( "  TRFV_ROW, " );
            SqlUpdateQuery.AppendLine ( "  TRFV_VALUE, " );
            SqlUpdateQuery.AppendLine ( "  TRFV_TEXT )" );
            SqlUpdateQuery.AppendLine ( "values " );
            SqlUpdateQuery.AppendLine ( " ('" + RecordField.Guid + "', " );
            SqlUpdateQuery.AppendLine ( "  '', " );
            SqlUpdateQuery.AppendLine ( "  0, " );
            SqlUpdateQuery.AppendLine ( " '" + value + "', " );
            SqlUpdateQuery.AppendLine ( " '' ); " );
            break;

          }
        case EvDataTypes.Table:
        case EvDataTypes.Special_Matrix:
          {
            this.LogDebug ( "Table or Matrix" );

            //
            // Iterate through table columns
            //
            for ( int column = 0; column < RecordField.Table.Header.Length; column++ )
            {
              EvFormFieldTableColumnHeader header = RecordField.Table.Header [ column ];

              this.LogDebug ( "Header" + header.Text );

              //
              // skip all empty columns
              //
              if ( header.Text == String.Empty )
              {
                continue;
              }

              //
              // create the column key, which must be less thatn 50 char
              //
              String columnKey = header.Text.Replace ( "'", String.Empty );
              columnKey = columnKey.Replace ( " ", String.Empty );

              if ( columnKey.Length > Evado.Model.Digital.EvcStatics.CONST_FIELD_VALUE_KEY_LENGTH )
              {
                columnKey = columnKey.Substring ( 0, Evado.Model.Digital.EvcStatics.CONST_FIELD_VALUE_KEY_LENGTH );
              }

              this.LogDebug ( "ColumnKey: " + columnKey );

              //
              // Iterate through table rows.
              //
              for ( int row = 0; row < RecordField.Table.Rows.Count; row++ )
              {
                value = RecordField.Table.Rows [ row ].Column [ column ].Replace ( "'", "" );

                SqlUpdateQuery.AppendLine ( "/** INSERT TABLE CELL VALUE **/" );
                SqlUpdateQuery.AppendLine ( "Insert Into EV_RECORD_FIELD_VALUES " );
                SqlUpdateQuery.AppendLine ( " (TRI_GUID, " );
                SqlUpdateQuery.AppendLine ( "  TRFV_COLUMN, " );
                SqlUpdateQuery.AppendLine ( "  TRFV_ROW, " );
                SqlUpdateQuery.AppendLine ( "  TRFV_VALUE, " );
                SqlUpdateQuery.AppendLine ( "  TRFV_TEXT )" );
                SqlUpdateQuery.AppendLine ( "values " );
                SqlUpdateQuery.AppendLine ( " ('" + RecordField.Guid + "', " );
                SqlUpdateQuery.AppendLine ( "  '" + columnKey + "', " );
                SqlUpdateQuery.AppendLine ( "  " + ( row + 1 ) + ", " );
                SqlUpdateQuery.AppendLine ( " '" + value + "', " );
                SqlUpdateQuery.AppendLine ( " '' ); " );

              }//END row iteration loop

            }//END column Iteration loop
            break;
          }

        default:
          {
            SqlUpdateQuery.AppendLine ( "/** INSERT ITEM VALUE **/" );
            SqlUpdateQuery.AppendLine ( "Insert Into EV_RECORD_FIELD_VALUES " );
            SqlUpdateQuery.AppendLine ( " (TRI_GUID, " );
            SqlUpdateQuery.AppendLine ( "  TRFV_COLUMN, " );
            SqlUpdateQuery.AppendLine ( "  TRFV_ROW, " );
            SqlUpdateQuery.AppendLine ( "  TRFV_VALUE, " );
            SqlUpdateQuery.AppendLine ( "  TRFV_TEXT )" );
            SqlUpdateQuery.AppendLine ( "values " );
            SqlUpdateQuery.AppendLine ( " ('" + RecordField.Guid + "', " );
            SqlUpdateQuery.AppendLine ( "  '', " );
            SqlUpdateQuery.AppendLine ( "  0, " );
            SqlUpdateQuery.AppendLine ( " '" + value + "', " );
            SqlUpdateQuery.AppendLine ( " '' ); " );
            break;
          }
      }//END Switch statement

      //this.LogDebugValue ( "SQL Query: " + SqlUpdateQuery.ToString ( ) );

      if ( EvSqlMethods.QueryUpdate ( SqlUpdateQuery.ToString ( ), null ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      return EvEventCodes.Ok;

    }//END UpdateFieldValue class. 

    // =====================================================================================
    /// <summary>
    /// This class adds new items to formfield table. 
    /// </summary>
    /// <param name="RecordField">EvFormField: a retrieved formfield data object</param>
    /// <returns>EvEventCodes: an event code for adding items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the recordUid is not defined or RecordGuid is empty. 
    /// 
    /// 2. Exit, if the FormItemUid is not defined or FormFieldGuid is empty
    /// 
    /// 3. Exit, if the usercommon name is empty. 
    /// 
    /// 4. Define the sql query parameters and execute the storeprocedure for adding items. 
    /// 
    /// 5. Return the event code for adding items. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private EvEventCodes addField ( EvFormField RecordField )
    {
      this.LogMethod ( "addField method. " );
      this.LogDebug ( "RecordGuid: " + RecordField.RecordGuid );
      this.LogDebug ( "FormFieldGuid: " + RecordField.FormFieldGuid );
      this.LogDebug ( "State: " + RecordField.State );
      //
      // Initialize the method debug log 
      //
      EvFormRecordComments recordComments = new EvFormRecordComments ( );
      EvEventCodes eventCode = EvEventCodes.Ok;

      // 
      // Validate whether the recordUid and RecordGuid are not empty. 
      // 
      if ( RecordField.RecordGuid == Guid.Empty )
      {
        return EvEventCodes.Identifier_General_ID_Error;
      }

      //
      // Validate FormItemUid and FormFieldGuid are not empty
      //
      if ( RecordField.FormFieldGuid == Guid.Empty )
      {
        return EvEventCodes.Identifier_General_ID_Error;
      }

      //
      // Validate whether the usercommon name is not empty. 
      //
      if ( RecordField.UserCommonName == String.Empty )
      {
        return EvEventCodes.Identifier_User_Id_Error;
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = GetParameters ( );
      SetParameters ( cmdParms, RecordField );

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( _storedProcedureAddItem, cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      eventCode = this.UpdateFieldValues ( RecordField );

      if ( eventCode != EvEventCodes.Ok )
      {
        this.LogDebug ( "Field value update error." );
        return eventCode;
      }

      //
      // Add record comments to event code. 
      //
      eventCode = recordComments.addNewComments ( RecordField.CommentList );

      this.LogDebug ( "Field Comment status: " + recordComments.Log );

      return eventCode;

    }//END AddItem class. 

    // =====================================================================================
    /// <summary>
    /// This deletes the items from the formfield table. 
    /// </summary>
    /// <param name="RecordField">EvFormField: a formfield data object</param>
    /// <returns>EvEventCodes: an event code for deleting items.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the Guid or UserCommonName is empty.
    /// 
    /// 2. Define the sql query parameters and execute the storeprocedure for deleting the items. 
    /// 
    /// 3. Return the event code for deleting items. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private EvEventCodes DeleteItem ( EvFormField RecordField )
    {
      // 
      // Validate whether the Guid and UserCommonName are not empty.
      // 
      if ( RecordField.Guid == Guid.Empty )
      {
        return EvEventCodes.Identifier_Global_Unique_Identifier_Error;
      }

      if ( RecordField.UserCommonName == String.Empty )
      {
        return EvEventCodes.Identifier_User_Id_Error;
      }

      // 
      // Define the query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(PARM_Guid, SqlDbType.UniqueIdentifier),
        new SqlParameter(PARM_UpdatedBy, SqlDbType.NVarChar,30),
        new SqlParameter(PARM_UpdatedByUserId, SqlDbType.NVarChar,100),
        new SqlParameter(PARM_UpdateDate, SqlDbType.DateTime)
      };
      cmdParms [ 0 ].Value = RecordField.Guid;
      cmdParms [ 1 ].Value = RecordField.UserCommonName;
      cmdParms [ 2 ].Value = RecordField.UpdatedByUserId;
      cmdParms [ 3 ].Value = DateTime.Now;

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( _storedProcedureDeleteItem, cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      return EvEventCodes.Ok;

    }//END DeleteItem method

    #endregion

  }//END EvFormRecordFields class

}//END namespace Evado.Dal.Clinical
