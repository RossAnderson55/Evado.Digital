/***************************************************************************************
 * <copyright file="dal\EvFormFields.cs" company="EVADO HOLDING PTY. LTD.">
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
using System.IO;
using System.Text;

//Application specific class references.
using Evado.Model;
using Evado.Model.Digital;

namespace Evado.Dal.Clinical
  {
  /// <summary>
  /// This class is handles the data access layer for the form field data object.
  /// </summary>
  public class EvFormFields : EvDalBase
    {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EvFormFields ( )
      {
      this.ClassNameSpace = "Evado.Dal.Clinical.EvFormFields.";
      }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="ClassParameters">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EvFormFields ( EvClassParameters ClassParameters )
      {
      this.ClassParameters = ClassParameters;
      this.ClassNameSpace = "Evado.Dal.Clinical.EvFormFields.";

      if ( this.ClassParameters.LoggingLevel == 0 )
        {
        this.ClassParameters.LoggingLevel = Evado.Dal.EvStaticSetting.LoggingLevel;
        }
      }

    #endregion

    #region Object Initialisation
    /* *********************************************************************************
     * 
     * Defines the classes constansts and global variables
     * 
     * *********************************************************************************/
    /// 
    /// Define the log source file
    /// 
    private static string _eventLogSource = ConfigurationManager.AppSettings [ "EventLogSource" ];

    /// <summary>
    /// This constant defines a sql query string for selecting all items from form field view. 
    /// </summary>
    private const string _sqlQueryView = "Select * \r\nFROM EvFormField_View ";

    /// <summary>
    /// This constant defines a sql query string for selecting all items from form field data index view. 
    /// </summary>
    private const string _sqlDataPointIndex = "Select * \r\nFROM EvFormField_DataIndex \r\n";

    /// <summary>
    /// This constant defines the data point index order for creating the index and 
    /// retrieving the fields.
    /// </summary>
    public const string SQL_SCHEDULED_DATA_POINT_INDEX = "SCHEDULE_ID, M_Order, MilestoneId, MA_Order, "
      + " MA_IsMandatory, ActivityId, ACF_Order, ACF_Mandatory, FormId, TCI_Order, FieldId";

    /// <summary>
    /// This constant defines the sql query for selecting all items from the formfield analysis list view. 
    /// </summary>
    private const string _sqlDataAnalysisQuery_List = "Select * FROM EvFormField_AnalysisList ";

    private string _sqlQueryString = String.Empty;

    #region Define the class storeprocedure.
    /// <summary>
    /// This constant defines the storeprocedure for adding items to the formfield table. 
    /// </summary>
    private const string _storedProcedureAddItem = "usr_FormField_add";

    /// <summary>
    /// This constant defines the storeprocedure for updating items on the formfield table. 
    /// </summary>
    private const string _storedProcedureUpdateItem = "usr_FormField_update";

    /// <summary>
    /// This constant defines the storeprocedure for deleting items from the formfield table. 
    /// </summary>
    private const string _storedProcedureDeleteItem = "usr_FormField_delete";
    #endregion

    #region Define the class parameters

    private const string DB_CUSTOMER_GUID = "CU_GUID";

    /// <summary>
    /// This constant defines a parameter for global unique identifier of CUSTOMER object
    /// </summary>
    private const string PARM_CUSTOMER_GUID = "@CUSTOMER_GUID";

    /// <summary>
    /// This constant defines the parameter for global unique identifier for the formfield object
    /// </summary>
    private const string PARM_Guid = "@Guid";

    /// <summary>
    /// This constant defines the parameter for form global unique identifier for the formfield object
    /// </summary>
    private const string PARM_FormGuid = "@FormGuid";

    /// <summary>
    /// This constant defines the parameter for order for the formfield object
    /// </summary>
    private const string PARM_FieldOrder = "@Order";

    /// <summary>
    /// This constant defines the parameter for trial identifier for the formfield object
    /// </summary>
    private const string PARM_TrialId = "@TrialId";

    /// <summary>
    /// This constant defines the parameter for field identifier for the formfield object
    /// </summary>
    private const string PARM_FieldId = "@FieldId";

    /// <summary>
    /// This constant defines the parameter for type identifier for the formfield object
    /// </summary>
    private const string PARM_TypeId = "@TypeId";

    /// <summary>
    /// This constant defines the parameter for subject for the formfield object
    /// </summary>
    private const string PARM_Subject = "@Subject";

    /// <summary>
    /// This constant defines the parameter for instructions for the formfield object
    /// </summary>
    private const string PARM_Instructions = "@Instructions";

    /// <summary>
    /// This constant defines the parameter for Options for the formfield object
    /// </summary>
    private const string PARM_Options = "@Options";

    /// <summary>
    /// This constant defines the parameter for Reference for the formfield object
    /// </summary>
    private const string PARM_Reference = "@Reference";

    /// <summary>
    /// This constant defines the parameter for milestone for the formfield object
    /// </summary>
    private const string PARM_ImageId = "@ImageId";

    /// <summary>
    /// This constant defines the parameter for Unit for the formfield object
    /// </summary>
    private const string PARM_Unit = "@Unit";

    /// <summary>
    /// This constant defines the parameter for review for the formfield object
    /// </summary>
    private const string PARM_Review = "@Review";

    /// <summary>
    /// This constant defines the parameter for xml validation rules for the formfield object
    /// </summary>
    private const string PARM_XmlValidationRules = "@XmlValidationRules";

    /// <summary>
    /// This constant defines the parameter for xml data for the formfield object
    /// </summary>
    private const string PARM_XmlData = "@XmlData";

    /// <summary>
    /// This constant defines the parameter for table for the formfield object
    /// </summary>
    private const string PARM_Table = "@Table";

    /// <summary>
    /// This constant defines the parameter for mandatory for the formfield object
    /// </summary>
    private const string PARM_Mandatory = "@Mandatory";

    /// <summary>
    /// This constant defines the parameter for datapoint for the formfield object
    /// </summary>
    private const string PARM_DatePoint = "@DataPoint";

    /// <summary>
    /// This constant defines the parameter for hide field for the formfield object
    /// </summary>
    private const string PARM_HideField = "@HideField";

    /// <summary>
    /// This constant defines the parameter for safety report for the formfield object
    /// </summary>
    private const string PARM_SafetyReport = "@SafetyReport";

    /// <summary>
    /// This constant defines the parameter for reviewer for the formfield object
    /// </summary>
    private const string PARM_Reviewer = "@Reviewer";

    /// <summary>
    /// This constant defines the parameter for reviewed date for the formfield object
    /// </summary>
    private const string PARM_ReviewedDate = "@ReviewedDate";

    /// <summary>
    /// This constant defines the parameter for approver for the formfield object
    /// </summary>
    private const string PARM_Approver = "@Approver";

    /// <summary>
    /// This constant defines the parameter for approval date for the formfield object
    /// </summary>
    private const string PARM_ApprovalDate = "@ApprovalDate";

    /// <summary>
    /// This constant defines the parameter for state for the formfield object
    /// </summary>
    private const string PARM_State = "@State";

    /// <summary>
    /// This constant defines the parameter for user identifier of those who updates the formfield object
    /// </summary>
    private const string PARM_UpdatedByUserId = "@UpdatedByUserId";

    /// <summary>
    /// This constant defines the parameter for user who updates the formfield object
    /// </summary>
    private const string PARM_UpdatedBy = "@UpdatedBy";

    /// <summary>
    /// This constant defines the parameter for updated date of the formfield object
    /// </summary>
    private const string PARM_UpdateDate = "@UpdateDate";

    /// <summary>
    /// This constant defines the parameter for form identifier of the formfield object
    /// </summary>
    private const string PARM_FormId = "@FormId";

    /// <summary>
    /// This constant defines the parameter for trial unique identifier of the formfield object
    /// </summary>
    private const string PARM_TrialUid = "@TrialUid";

    /// <summary>
    /// This constant defines the parameter for arm index of the formfield object
    /// </summary>
    private const string PARM_ScheduleId = "@ScheduleId";

    /// <summary>
    /// This constant defines the parameter for milestone identifier of the formfield object
    /// </summary>
    private const string PARM_MilestoneId = "@MilestoneId";
    /// <summary>
    /// This constant defines a string parameter as CDASH_METADATA for the Subject object. 
    /// </summary>
    private const string PARM_CDASH_METADATA = "@CDASH_METADATA";
    /// <summary>
    /// This constant defines a string parameter as UNIT_SCALING for the Subject object. 
    /// </summary>
    private const string PARM_UNIT_SCALING = "@UNIT_SCALING";
    /// <summary>
    /// This constant defines a string parameter as EX_SELECTION_LIST_ID for the Subject object. 
    /// </summary>
    private const string PARM_EX_SELECTION_LIST_ID = "@EX_SELECTION_LIST_ID";
    /// <summary>
    /// This constant defines a string parameter as EX_SELECTION_LIST_ID for the Subject object. 
    /// </summary>
    private const string PARM_EX_SELECTION_LIST_CATEGORY = "@EX_SELECTION_LIST_CATEGORY";
    /// <summary>
    /// This constant defines a string parameter as FIELD_CATEGORY for the Subject object. 
    /// </summary>
    private const string PARM_FIELD_CATEGORY = "@FIELD_CATEGORY";
    /// <summary>
    /// This constant defines a string parameter as DEFAULT_VALUE for the Subject object. 
    /// </summary>
    private const string PARM_DEFAULT_VALUE = "@DEFAULT_VALUE";
    /// <summary>
    /// This constant defines a string parameter as SELECT_BY_CODING_VALUE for the Subject object. 
    /// </summary>
    private const string PARM_SELECT_BY_CODING_VALUE = "@SELECT_BY_CODING_VALUE";
    /// <summary>
    /// This constant defines a string parameter as SUMMARY_FIELD for the Subject object. 
    /// </summary>
    private const string PARM_SUMMARY_FIELD = "@SUMMARY_FIELD";
    /// <summary>
    /// This constant defines a string parameter as MULTI_LINE_TEXT_FIELD for the Subject object. 
    /// </summary>
    private const string PARM_MULTI_LINE_TEXT_FIELD = "@MULTI_LINE_TEXT_FIELD";
    /// <summary>
    /// This constant defines a string parameter as HORIZONTAL_BUTTONS for the Subject object. 
    /// </summary>
    private const string PARM_HORIZONTAL_BUTTONS = "@HORIZONTAL_BUTTONS";
    /// <summary>
    /// This constant defines a string parameter as FORM_IDS for the Subject object. 
    /// </summary>
    private const string PARM_FORM_IDS = "@FORM_IDS";
    /// <summary>
    /// This constant defines a string parameter as INITIAL_OPTION_LIST for the Subject object. 
    /// </summary>
    private const string PARM_INITIAL_OPTION_LIST = "@INITIAL_OPTION_LIST";
    /// <summary>
    /// This constant defines a parameter for INITIAL_VERSION of common form field object
    /// </summary>
    private const string PARM_INITIAL_VERSION = "@INITIAL_VERSION";
    /// <summary>
    /// This constant defines a string parameter as OPTION_LIST for the Subject object.
    /// </summary>
    private const string PARM_OPTIONS = "@Options";
    /// <summary>
    /// This constant defines a string parameter as SECTION for the Subject object. 
    /// </summary>
    private const string PARM_SECTION = "@SECTION";
    /// <summary>
    /// This constant defines a string parameter as SECTION for the Subject object. 
    /// </summary>
    private const string PARM_ANALOGUE_SCALE = "@ANALOGUE_SCALE";
    /// <summary>
    /// This constant defines a string parameter as SECTION for the Subject object. 
    /// </summary>
    private const string PARM_ANALOGUE_LEGEND_START = "@ANALOGUE_LEGEND_START";
    /// <summary>
    /// This constant defines a string parameter as SECTION for the Subject object. 
    /// </summary>
    private const string PARM_ANALOGUE_LEGEND_FINISH = "@ANALOGUE_LEGEND_FINISH";
    /// <summary>
    /// This constant defines a string parameter as JAVA SCRIPt  object. 
    /// </summary>
    private const string PARM_JAVA_SCRIPT = "@JAVA_SCRIPT";

    private const string PARM_ValidationLowerLimit = "@ValidationLowerLimit";
    private const string PARM_ValidationUpperLimit = "@ValidationUpperLimit";
    private const string PARM_AlertLowerLimit = "@AlertLowerLimit";
    private const string PARM_AlertUpperLimit = "@AlertUpperLimit";
    private const string PARM_SafetyLowerLimit = "@SafetyLowerLimit";
    private const string PARM_SafetyUpperLimit = "@SafetyUpperLimit";
    private const string PARM_NOT_VALID_FOR_MALES = "@NOT_VALID_FOR_MALES";
    private const string PARM_NOT_VALID_FOR_FEMALES = "@NOT_VALID_FOR_FEMALES";
    private const string PARM_AFTER_DATE_OF_BIRTH = "@AFTER_DATE_OF_BIRTH";
    private const string PARM_AFTER_CONSENT_DATE = "@AFTER_CONSENT_DATE";
    private const string PARM_FIELD_NOT_VALID = "@FIELD_NOT_VALID";
    private const string PARM_FIELD_OPTIONS_NOT_VALID = "@FIELD_OPTIONS_NOT_VALID";
    private const string PARM_NUM_NORM_LOWER_LIMIT = "@NUM_NORM_LOWER_LIMIT";
    private const string PARM_NUM_NORM_UPPER_LIMIT = "@NUM_NORM_UPPER_LIMIT";
    private const string PARM_DAYS_FROM_RECORD_DATE = "@DAYS_FROM_RECORD_DATE";
    private const string PARM_DAYS_FROM_VISIT_DATE = "@DAYS_FROM_VISIT_DATE";


    #endregion

    #endregion

    #region Set Query Parameters

    // ==================================================================================
    /// <summary>
    /// This class sets an array of sql query parameters. 
    /// </summary>
    /// <returns>SqlParameter: an array of sql query parameters</returns>
    /// <remarks>
    /// This method consists of the following step: 
    /// 
    /// 1. Create an array of sql query parameters
    /// 
    /// 2. Return an array of sql query parameters. 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    private static SqlParameter [ ] GetParameters ( )
      {
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter( PARM_Guid, SqlDbType.UniqueIdentifier),
        new SqlParameter( PARM_FormGuid, SqlDbType.UniqueIdentifier),
        new SqlParameter( PARM_TrialId, SqlDbType.NVarChar, 10),
        new SqlParameter( PARM_FieldId, SqlDbType.NVarChar, 20),
        new SqlParameter( PARM_TypeId, SqlDbType.VarChar, 50),
        new SqlParameter( PARM_Subject, SqlDbType.VarChar, 150),
        new SqlParameter( PARM_Instructions, SqlDbType.NText),
        new SqlParameter( PARM_Reference, SqlDbType.VarChar, 250),
        new SqlParameter( PARM_ImageId, SqlDbType.VarChar, 50),
        new SqlParameter( PARM_Unit, SqlDbType.NVarChar, 15),

        new SqlParameter( PARM_XmlValidationRules, SqlDbType.NText),//10
        new SqlParameter( PARM_XmlData, SqlDbType.NText),
        new SqlParameter( PARM_Table, SqlDbType.NText),
        new SqlParameter( PARM_FieldOrder, SqlDbType.SmallInt),
        new SqlParameter( PARM_Mandatory, SqlDbType.Bit),
        new SqlParameter( PARM_SafetyReport, SqlDbType.Bit),
        new SqlParameter( PARM_DatePoint, SqlDbType.Bit),
        new SqlParameter( PARM_HideField, SqlDbType.Bit),
        new SqlParameter( PARM_UpdatedByUserId, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_UpdatedBy, SqlDbType.NVarChar, 100),

        new SqlParameter( PARM_UpdateDate, SqlDbType.DateTime), //20
        new SqlParameter( PARM_CDASH_METADATA, SqlDbType.NVarChar, 250),
        new SqlParameter( PARM_UNIT_SCALING, SqlDbType.NVarChar, 250),
        new SqlParameter( PARM_EX_SELECTION_LIST_ID, SqlDbType.NVarChar, 250),
        new SqlParameter( PARM_EX_SELECTION_LIST_CATEGORY, SqlDbType.NVarChar, 250),
        new SqlParameter( PARM_FIELD_CATEGORY, SqlDbType.NVarChar, 250),
        new SqlParameter( PARM_DEFAULT_VALUE, SqlDbType.NVarChar, 250),
        new SqlParameter( PARM_SELECT_BY_CODING_VALUE, SqlDbType.Bit),
        new SqlParameter( PARM_SUMMARY_FIELD, SqlDbType.Bit),
        new SqlParameter( PARM_MULTI_LINE_TEXT_FIELD, SqlDbType.Bit),

        new SqlParameter( PARM_HORIZONTAL_BUTTONS, SqlDbType.Bit),//30
        new SqlParameter( PARM_FORM_IDS, SqlDbType.NVarChar, 250),
        new SqlParameter( PARM_INITIAL_OPTION_LIST, SqlDbType.NVarChar, 250),
        new SqlParameter( PARM_INITIAL_VERSION, SqlDbType.Int),
        new SqlParameter( PARM_OPTIONS, SqlDbType.NVarChar),
        new SqlParameter( PARM_SECTION, SqlDbType.NVarChar, 250),
        new SqlParameter( PARM_ValidationLowerLimit, SqlDbType.Float),
        new SqlParameter( PARM_ValidationUpperLimit, SqlDbType.Float),
        new SqlParameter( PARM_AlertLowerLimit, SqlDbType.Float),
        new SqlParameter( PARM_AlertUpperLimit, SqlDbType.Float),

        new SqlParameter( PARM_SafetyLowerLimit, SqlDbType.Float),//40
        new SqlParameter( PARM_SafetyUpperLimit, SqlDbType.Float),
        new SqlParameter( PARM_NOT_VALID_FOR_MALES, SqlDbType.Bit),
        new SqlParameter( PARM_NOT_VALID_FOR_FEMALES, SqlDbType.Bit),
        new SqlParameter( PARM_AFTER_DATE_OF_BIRTH, SqlDbType.Bit),
        new SqlParameter( PARM_AFTER_CONSENT_DATE, SqlDbType.Bit),
        new SqlParameter( PARM_FIELD_NOT_VALID, SqlDbType.NVarChar, 500),
        new SqlParameter( PARM_FIELD_OPTIONS_NOT_VALID, SqlDbType.NVarChar, 500),
        new SqlParameter( PARM_NUM_NORM_LOWER_LIMIT, SqlDbType.Float),
        new SqlParameter( PARM_NUM_NORM_UPPER_LIMIT, SqlDbType.Float),

        new SqlParameter( PARM_DAYS_FROM_RECORD_DATE, SqlDbType.Int),//50
        new SqlParameter( PARM_DAYS_FROM_VISIT_DATE, SqlDbType.Int),
        new SqlParameter( PARM_ANALOGUE_SCALE, SqlDbType.VarChar, 20),
        new SqlParameter( PARM_ANALOGUE_LEGEND_START, SqlDbType.VarChar, 30),
        new SqlParameter( PARM_ANALOGUE_LEGEND_FINISH, SqlDbType.VarChar, 30),
        new SqlParameter( PARM_JAVA_SCRIPT, SqlDbType.NText),
        new SqlParameter( PARM_CUSTOMER_GUID, SqlDbType.UniqueIdentifier),
      };

      return cmdParms;
      }//END GetParameters class

    // ==================================================================================
    /// <summary>
    /// This class sets values from formfield object to the sql query parameter array. 
    /// </summary>
    /// <param name="cmdParms">SqlParameter: an array of sql query parameters</param>
    /// <param name="FormField">EvFormField: a FormField data object</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. If the data type is a table then seriallise the formfield data table.
    /// 
    /// 2. Update the values from formfield object to the sql query parameters array. 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    private void setParameters ( SqlParameter [ ] cmdParms, EvFormField FormField )
      {
      //
      // Initialize the serialized table structure. 
      //
      string serialisedTableStructure = String.Empty;

      //
      // if the data type is a table then seriallise the formfield data table.
      //
      if ( FormField.TypeId == Evado.Model.EvDataTypes.Special_Matrix
        || FormField.TypeId == Evado.Model.EvDataTypes.Table )
        {
        this.LogValue ( "Column 1 header text: " + FormField.Table.Header [ 0 ].Text );

        serialisedTableStructure = Evado.Model.EvStatics.SerialiseObject<EvFormFieldTable> ( FormField.Table );
        }

      if ( FormField.ValidationRules.ValidationLowerLimit >= FormField.ValidationRules.ValidationUpperLimit )
        {
        FormField.ValidationRules.ValidationLowerLimit = EvcStatics.CONST_NUMERIC_MINIMUM;
        FormField.ValidationRules.ValidationUpperLimit = EvcStatics.CONST_NUMERIC_MAXIMUM;
        }

      if ( FormField.ValidationRules.AlertLowerLimit >= FormField.ValidationRules.AlertUpperLimit )
        {
        FormField.ValidationRules.AlertLowerLimit = EvcStatics.CONST_NUMERIC_MINIMUM;
        FormField.ValidationRules.AlertUpperLimit = EvcStatics.CONST_NUMERIC_MAXIMUM;
        }

      //
      // Update the values from formfield object to the sql query parameters array. 
      //
      cmdParms [ 0 ].Value = FormField.Guid;
      cmdParms [ 1 ].Value = FormField.FormGuid;
      cmdParms [ 2 ].Value = FormField.TrialId;
      cmdParms [ 3 ].Value = FormField.FieldId;
      cmdParms [ 4 ].Value = FormField.TypeId;
      cmdParms [ 5 ].Value = FormField.Design.Title;
      cmdParms [ 6 ].Value = FormField.Design.Instructions;
      cmdParms [ 7 ].Value = FormField.Design.HttpReference;
      cmdParms [ 8 ].Value = String.Empty;
      cmdParms [ 9 ].Value = FormField.Design.Unit;

      cmdParms [ 10 ].Value = String.Empty;
      cmdParms [ 11 ].Value = String.Empty;
      cmdParms [ 12 ].Value = serialisedTableStructure;
      cmdParms [ 13 ].Value = FormField.Design.Order;
      cmdParms [ 14 ].Value = FormField.Design.Mandatory;
      cmdParms [ 15 ].Value = FormField.Design.SafetyReport;
      cmdParms [ 16 ].Value = FormField.Design.DataPoint;
      cmdParms [ 17 ].Value = FormField.Design.HideField;
      cmdParms [ 18 ].Value = FormField.UpdatedByUserId;
      cmdParms [ 19 ].Value = FormField.UserCommonName;

      cmdParms [ 20 ].Value = DateTime.Now;
      cmdParms [ 21 ].Value = FormField.cDashMetadata;
      cmdParms [ 22 ].Value = FormField.Design.UnitScaling;
      cmdParms [ 23 ].Value = FormField.Design.ExSelectionListId;
      cmdParms [ 24 ].Value = FormField.Design.ExSelectionListCategory;
      cmdParms [ 25 ].Value = FormField.Design.FieldCategory;
      cmdParms [ 26 ].Value = FormField.Design.DefaultValue;
      cmdParms [ 27 ].Value = FormField.Design.SelectByCodingValue;
      cmdParms [ 28 ].Value = FormField.Design.SummaryField;
      cmdParms [ 29 ].Value = FormField.Design.MultiLineTextField;

      cmdParms [ 30 ].Value = FormField.Design.HorizontalButtons;
      cmdParms [ 31 ].Value = FormField.Design.FormIds;
      cmdParms [ 32 ].Value = FormField.Design.InitialOptionList;
      cmdParms [ 33 ].Value = FormField.Design.InitialVersion;
      cmdParms [ 34 ].Value = FormField.Design.Options;
      cmdParms [ 35 ].Value = FormField.Design.Section;
      cmdParms [ 36 ].Value = FormField.ValidationRules.ValidationLowerLimit;
      cmdParms [ 37 ].Value = FormField.ValidationRules.ValidationUpperLimit;
      cmdParms [ 38 ].Value = FormField.ValidationRules.AlertLowerLimit;
      cmdParms [ 39 ].Value = FormField.ValidationRules.AlertUpperLimit;

      cmdParms [ 40 ].Value = EvcStatics.CONST_NUMERIC_MINIMUM;
      cmdParms [ 41 ].Value = EvcStatics.CONST_NUMERIC_MAXIMUM;
      cmdParms [ 42 ].Value = FormField.ValidationRules.NotValidForMale;
      cmdParms [ 43 ].Value = FormField.ValidationRules.NotValidForFemale;
      cmdParms [ 44 ].Value = FormField.ValidationRules.IsAfterBirthDate;
      cmdParms [ 45 ].Value = FormField.ValidationRules.IsAfterConsentDate;
      cmdParms [ 46 ].Value = FormField.ValidationRules.NotValid.EncodedRules;
      cmdParms [ 47 ].Value = FormField.ValidationRules.NotValidOptions.EncodedRules;
      cmdParms [ 48 ].Value = FormField.ValidationRules.NormalRangeLowerLimit;
      cmdParms [ 49 ].Value = FormField.ValidationRules.NormalRangeUpperLimit;

      cmdParms [ 50 ].Value = FormField.ValidationRules.WithinDaysOfRecordDate;
      cmdParms [ 51 ].Value = FormField.ValidationRules.WithinDaysOfVisitDate;
      cmdParms [ 52 ].Value = FormField.Design.AnalogueScale;
      cmdParms [ 53 ].Value = FormField.Design.AnalogueLegendStart;
      cmdParms [ 54 ].Value = FormField.Design.AnalogueLegendFinish;
      cmdParms [ 55 ].Value = FormField.Design.JavaScript;
      cmdParms [ 56 ].Value = this.ClassParameters.CustomerGuid;

      }//END setParameters class.
    #endregion

    #region Read FormField data

    // =====================================================================================
    /// <summary>
    /// This class reads the content of the data reader object into FormField business object.
    /// </summary>
    /// <param name="Row">DataRow: an sql data query row</param>
    /// <returns>EvFormField: a form field object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Update formfield object with the compatible data row items. 
    /// 
    /// 2. If the formfield typeId is table, iterate through the formfield table and set the validation rules
    /// 
    /// 3. If the selection validation options are missing, add them.
    /// 
    /// 4. If it is an external coding visitSchedule then add the relevant coding visitSchedule items.
    /// 
    /// 5. Resolve the numeric 'NA' to negative infinity issue.
    /// 
    /// 6. Update the instrument type to current enumeration.
    /// 
    /// 7. If formfield typeId is either analogue scale or horizontal radio buttons, 
    /// select the design by coding value
    /// 
    /// 8. Return the formfield object.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private EvFormField getRowData ( DataRow Row )
      {
      // 
      // Initialise xmltable string and a return formfield object. 
      // 
      string xmlTable = String.Empty;
      EvFormField formField = new EvFormField ( );

      //
      // Update formfield object with the compatible data row items. 
      //
      formField.CustomerGuid = EvSqlMethods.getGuid ( Row, DB_CUSTOMER_GUID );
      formField.Guid = EvSqlMethods.getGuid ( Row, "TCI_Guid" );
      //formField.Uid = EvSqlMethods.getInteger ( Row, "TCI_Uid" );
      formField.FormGuid = EvSqlMethods.getGuid ( Row, "TC_Guid" );
      //formField.FormUid = EvSqlMethods.getInteger ( Row, "TC_Uid" );
      //formField.FormItemUid = formField.Uid;

      formField.TrialId = EvSqlMethods.getString ( Row, "TrialId" );
      formField.FieldId = EvSqlMethods.getString ( Row, "FieldId" );
      String value = EvSqlMethods.getString ( Row, "TCI_TypeId" );
      formField.TypeId = Evado.Model.EvStatics.Enumerations.parseEnumValue<Evado.Model.EvDataTypes> ( value );

      xmlTable = EvSqlMethods.getString ( Row, "TCI_Table" );

      //
      // Extract the form fields validation rules
      //
      string xmlValidationRules = EvSqlMethods.getString ( Row, "TCI_XmlValidationRules" );
      if ( xmlValidationRules != String.Empty )
        {
        formField.ValidationRules =
          Evado.Model.EvStatics.DeserialiseObject<EvFormFieldValidationRules> ( xmlValidationRules );
        }
      else
        {
        formField.ValidationRules = new EvFormFieldValidationRules ( );

        formField.ValidationRules.ValidationLowerLimit = EvSqlMethods.getFloat ( Row, "TCI_ValidationLowerLimit" );
        formField.ValidationRules.ValidationUpperLimit = EvSqlMethods.getFloat ( Row, "TCI_ValidationUpperLimit" );
        formField.ValidationRules.AlertLowerLimit = EvSqlMethods.getFloat ( Row, "TCI_AlertLowerLimit" );
        formField.ValidationRules.AlertUpperLimit = EvSqlMethods.getFloat ( Row, "TCI_AlertLowerLimit" );
        formField.ValidationRules.NormalRangeLowerLimit = EvSqlMethods.getFloat ( Row, "TCI_NUM_NORM_LOWER_LIMIT" );
        formField.ValidationRules.NormalRangeUpperLimit = EvSqlMethods.getFloat ( Row, "TCI_NUM_NORM_UPPER_LIMIT" );

        formField.ValidationRules.NotValidForMale = EvSqlMethods.getBool ( Row, "TCI_NOT_VALID_FOR_MALES" );
        formField.ValidationRules.NotValidForFemale = EvSqlMethods.getBool ( Row, "TCI_NOT_VALID_FOR_FEMALES" );
        formField.ValidationRules.IsAfterBirthDate = EvSqlMethods.getBool ( Row, "TCI_AFTER_DATE_OF_BIRTH" );
        formField.ValidationRules.IsAfterConsentDate = EvSqlMethods.getBool ( Row, "TCI_AFTER_CONSENT_DATE" );

        formField.ValidationRules.WithinDaysOfRecordDate = EvSqlMethods.getInteger ( Row, "TCI_DAYS_FROM_RECORD_DATE" );
        formField.ValidationRules.WithinDaysOfVisitDate = EvSqlMethods.getInteger ( Row, "TCI_DAYS_FROM_VISIT_DATE" );

        formField.ValidationRules.NotValid.EncodedRules = EvSqlMethods.getString ( Row, "TCI_FIELD_NOT_VALID" );
        formField.ValidationRules.NotValidOptions.EncodedRules = EvSqlMethods.getString ( Row, "TCI_FIELD_OPTIONS_NOT_VALID" );

        }

      string xmlDesign = EvSqlMethods.getString ( Row, "TCI_XmlData" );
      if ( xmlDesign != String.Empty )
        {
        xmlDesign = xmlDesign.Replace ( "<TypeId />", "<TypeId>" + formField.TypeId + "</TypeId>" );
        xmlDesign = xmlDesign.Replace ( "Check_Button_List", "Check_Box_List" );
        xmlDesign = xmlDesign.Replace ( "Subject_Demographics", "Special_Subject_Demographics" );
        xmlDesign = xmlDesign.Replace ( "Medication_Summary", "Special_Medication_Summary" );
        xmlDesign = xmlDesign.Replace ( "Matrix", "Special_Matrix" );

        formField.Design = Evado.Model.Digital.EvcStatics.DeserialiseObject<EvFormFieldDesign> ( xmlDesign );
        }
      else
        {
        formField.Design.UnitScaling = EvSqlMethods.getString ( Row, "TCI_UNIT_SCALING" );
        formField.Design.ExSelectionListId = EvSqlMethods.getString ( Row, "TCI_EX_SELECTION_LIST_ID" );
        formField.Design.ExSelectionListCategory = EvSqlMethods.getString ( Row, "TCI_EX_SELECTION_LIST_CATEGORY" );
        formField.Design.FieldCategory = EvSqlMethods.getString ( Row, "TCI_FIELD_CATEGORY" );
        formField.Design.DefaultValue = EvSqlMethods.getString ( Row, "TCI_DEFAULT_VALUE" );
        formField.Design.SelectByCodingValue = EvSqlMethods.getBool ( Row, "TCI_SELECT_BY_CODING_VALUE" );
        formField.Design.SummaryField = EvSqlMethods.getBool ( Row, "TCI_SUMMARY_FIELD" );
        formField.Design.MultiLineTextField = EvSqlMethods.getBool ( Row, "TCI_MULTI_LINE_TEXT_FIELD" );
        formField.Design.HorizontalButtons = EvSqlMethods.getBool ( Row, "TCI_HORIZONTAL_BUTTONS" );
        formField.Design.FormIds = EvSqlMethods.getString ( Row, "TCI_FORM_IDS" );
        formField.Design.InitialOptionList = EvSqlMethods.getString ( Row, "TCI_INITIAL_OPTION_LIST" );
        formField.Design.InitialVersion = EvSqlMethods.getInteger ( Row, "TCI_INITIALVERSION" );
        formField.Design.Options = EvSqlMethods.getString ( Row, "TCI_OPTIONS" );
        formField.Design.Section = EvSqlMethods.getString ( Row, "TCI_SECTION" );
        formField.Design.Instructions = EvSqlMethods.getString ( Row, "TCI_INSTRUCTIONS" );
        formField.Design.HttpReference = EvSqlMethods.getString ( Row, "TCI_Reference" );

        value = EvSqlMethods.getString ( Row, "TCI_ANALOGUE_SCALE" );
        if ( value != String.Empty )
          {
          formField.Design.AnalogueScale = Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue<EvFormField.AnalogueScaleOptions> ( value );
          }
        formField.Design.AnalogueLegendStart = EvSqlMethods.getString ( Row, "TCI_ANALOGUE_LEGEND_START" );
        formField.Design.AnalogueLegendFinish = EvSqlMethods.getString ( Row, "TCI_ANALOGUE_LEGEND_FINISH" );
        }

      formField.Design.JavaScript = EvSqlMethods.getString ( Row, "TCI_JAVA_SCRIPT" );

      formField.Design.Title = EvSqlMethods.getString ( Row, "TCI_Subject" );
      formField.Design.Unit = EvSqlMethods.getString ( Row, "TCI_Unit" );
      formField.Design.Order = EvSqlMethods.getInteger ( Row, "TCI_Order" );
      formField.Design.Mandatory = EvSqlMethods.getBool ( Row, "TCI_Mandatory" );
      formField.Design.DataPoint = EvSqlMethods.getBool ( Row, "TCI_DataPoint" );
      formField.Design.SafetyReport = EvSqlMethods.getBool ( Row, "TCI_SafetyReport" );
      formField.Design.HideField = EvSqlMethods.getBool ( Row, "TCI_HideField" );
      formField.FormId = EvSqlMethods.getString ( Row, "FormId" );
      formField.UpdatedByUserId = EvSqlMethods.getString ( Row, "TCI_UpdatedByUserId" );
      formField.Updated = EvSqlMethods.getString ( Row, "TCI_UpdatedBy" );
      formField.Updated += " on " + EvSqlMethods.getDateTime ( Row, "TCI_UpdateDate" ).ToString ( "dd MMM yyyy HH:mm" );
      formField.cDashMetadata = EvSqlMethods.getString ( Row, "TCI_CDASH_METADATA" );

      //
      // if the data type is a table then deseriallise the formfield data table.
      //
      if ( formField.TypeId == Evado.Model.EvDataTypes.Special_Matrix
        || formField.TypeId == Evado.Model.EvDataTypes.Table )
        {
        this.LogValue ( "Table Length: " + xmlTable.Length.ToString ( ) );
        //
        // Initialise the table objects.
        //
        formField.Table = new EvFormFieldTable ( );

        //
        // Validate whehter the Table has data.
        //
        if ( xmlTable != String.Empty )
          {
          formField.Table = Evado.Model.Digital.EvcStatics.DeserialiseObject<EvFormFieldTable> ( xmlTable );

          // 
          // Iterate through the table and set the validation rules
          // 
          for ( int i = 0; i < formField.Table.ColumnCount; i++ )
            {
            //
            // Addressing the 'NA' to negative infinity issue for non-numeric fields.
            //
            // Iterate through the table data converting the relevant cell values to NA.
            //
            for ( int j = 0; j < formField.Table.Rows.Count; j++ )
              {
              String cell = formField.Table.Rows [ j ].Column [ i ];

              if ( cell == Evado.Model.Digital.EvcStatics.CONST_NUMERIC_NULL.ToString ( )
                && formField.Table.Header [ i ].TypeId != EvFormFieldTableColumnHeader.ItemTypeNumeric )
                {
                cell = "NA";
                }
              }//END column iteration loop

            }//END newField iteration loop

          }// Table has data.

        }//END Table newField.

      if ( formField.Table != null )
        {
        this.LogValue ( "Column 1 header text: " + formField.Table.Header [ 0 ].Text );
        }

      //
      // If the selection validation options are missing add them.
      //
      if ( ( formField.TypeId == Evado.Model.EvDataTypes.Selection_List
          || formField.TypeId == Evado.Model.EvDataTypes.Radio_Button_List
          || formField.TypeId == Evado.Model.EvDataTypes.Check_Box_List )
        && ( formField.ValidationRules.NotValidOptions == null ) )
        {
        formField.ValidationRules.NotValidOptions = new EvFormFieldValidationNotValid ( );
        }

      // 
      // If it is an external selection list, add the relevant coding visitSchedule items.
      // 
      if ( formField.TypeId == Evado.Model.EvDataTypes.External_Selection_List )
        {
        this.LogValue ( "External ListId: " + formField.Design.ExSelectionListId
          + " Category: " + formField.Design.ExSelectionListCategory );

        EvFormFieldSelectionLists externalCodingLists = new EvFormFieldSelectionLists ( );

        formField.Design.Options = externalCodingLists.getItemCodingList (
          formField.Design.ExSelectionListId,
          formField.Design.ExSelectionListCategory );

        this.LogValue ( " " + externalCodingLists.Log );

        }

      //
      // Resolve the numeric 'NA' to negative infinity issue.
      //
      if ( formField.ItemValue == Evado.Model.Digital.EvcStatics.CONST_NUMERIC_NULL.ToString ( )
        && formField.TypeId != Evado.Model.EvDataTypes.Numeric )
        {
        formField.ItemValue = "NA";
        }

      // 
      // Update the formfield type to current enumeration.
      // 
      if ( ( int ) formField.TypeId < 0 )
        {
        formField.TypeId = ( Evado.Model.EvDataTypes ) Math.Abs ( ( int ) formField.TypeId );
        }

      //
      // If formfield typeId is either analogue scale or horizontal radio buttons, select the design by coding value
      //
      if ( formField.TypeId == Evado.Model.EvDataTypes.Analogue_Scale
        || formField.TypeId == Evado.Model.EvDataTypes.Horizontal_Radio_Buttons )
        {
        formField.Design.SelectByCodingValue = true;
        }

      if ( formField.ValidationRules.ValidationLowerLimit >= formField.ValidationRules.ValidationUpperLimit )
        {
        formField.ValidationRules.ValidationLowerLimit = EvcStatics.CONST_NUMERIC_MINIMUM;
        formField.ValidationRules.ValidationUpperLimit = EvcStatics.CONST_NUMERIC_MAXIMUM;
        }

      if ( formField.ValidationRules.AlertLowerLimit >= formField.ValidationRules.AlertUpperLimit )
        {
        formField.ValidationRules.AlertLowerLimit = EvcStatics.CONST_NUMERIC_MINIMUM;
        formField.ValidationRules.AlertUpperLimit = EvcStatics.CONST_NUMERIC_MAXIMUM;
        }

      // 
      // Return the formfield object.
      // 
      return formField;

      }//END getRowData method.

    // =====================================================================================
    /// <summary>
    /// This class reads the content of the data reader object into FormField business object.
    /// </summary>
    /// <param name="Row">DataRow: an sql data query row</param>
    /// <returns>EvFormField: a form field object</returns>
    // -------------------------------------------------------------------------------------
    private EvFormField getChartOptionRowData ( DataRow Row )
      {
      // 
      // Initialise xmltable string and a return formfield object. 
      // 
      string xmlTable = String.Empty;
      EvFormField formField = new EvFormField ( );

      //
      // Update formfield object with the compatible data row items. 
      //
      formField.TrialId = EvSqlMethods.getString ( Row, "TrialId" );
      formField.FieldId = EvSqlMethods.getString ( Row, "FieldId" );
      formField.FormId = EvSqlMethods.getString ( Row, "FormId" );
      formField.Design.Title = EvSqlMethods.getString ( Row, "TCI_Subject" );
      String value = EvSqlMethods.getString ( Row, "TCI_TypeId" );
      formField.TypeId = Evado.Model.EvStatics.Enumerations.parseEnumValue<Evado.Model.EvDataTypes> ( value );
      formField.Design.Options = EvSqlMethods.getString ( Row, "TCI_OPTIONS" );

      // 
      // Return the formfield object.
      // 
      return formField;

      }//END getRowData method.
    #endregion

    #region Form Field Queries

    // =====================================================================================
    /// <summary>
    /// This class returns a list of formfield items retrieving by form Guid
    /// </summary>
    /// <param name="FormGuid">Guid: (Mandatory) The form GUID.</param>
    /// <returns>List of EvFormField: a list of FormField items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define the sql query parameters and a sql query string. 
    /// 
    /// 2. Execute the sql query string with parameters and store the results on data table. 
    /// 
    /// 3. Iterate through the table and extract data row to the formfield data object. 
    /// 
    /// 4. Add the object values to the Formfield list. 
    /// 
    /// 5. Return the FormFields list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvFormField> GetView ( Guid FormGuid )
      {
      this.LogMethod ( "GetView" );
      this.LogValue ( "FormGuid: " + FormGuid );
      //
      // Initialize the debug log and a return list of formfield
      //
      List<EvFormField> view = new List<EvFormField> ( );

      EvFormField formField = new EvFormField ( );
      //    EvFormSections FormSections = new EvFormSections ( );
      // 
      // Define the SQL query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter(PARM_FormGuid, SqlDbType.UniqueIdentifier),
      };
      cmdParms [ 0 ].Value = FormGuid;

      // 
      // Define the query string.
      // 
      _sqlQueryString = _sqlQueryView + " WHERE (TC_Guid = @FormGuid) ";

      _sqlQueryString += " ORDER BY TCI_Order";

      //_Status += "\r\n" + sqlQueryString;

      // 
      // Scroll through the results
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

          // EvFormField formField = this.getRowData ( row );
          formField = this.getRowData ( row );

          view.Add ( formField );
          }
        }
      this.LogValue ( "Count: " + view.Count.ToString ( ) );


      // 
      // Pass back the result arrray.
      // 
      return view;

      }//END GetView method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of options using Guid and OrderBy
    /// </summary>
    /// <param name="FormGuid">Guid: a form global unique identifeir</param>
    /// <param name="OrderBy">string: an OrderBy string.</param>
    /// <returns>List of EvOption: a list of options</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define sql query parameters and sql query string. 
    /// 
    /// 2. Execute the sql query string with parameters and store the result on data table 
    /// 
    /// 3. Iterate through the table and extract data row to the option object. 
    /// 
    /// 4. Add option object to the Options list. 
    /// 
    /// 5. Return the Options list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> GetList ( Guid FormGuid, string OrderBy )
      {
      //
      // Initialize the debug log, a return option list and an option object. 
      //
      this.LogMethod ( "GetList" );
      this.LogValue ( "FormGuid: " + FormGuid );

      List<EvOption> list = new List<EvOption> ( );
      EvOption option = new EvOption ( );
      list.Add ( option );

      // 
      // Define the SQL query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter(PARM_FormGuid, SqlDbType.UniqueIdentifier),
      };
      cmdParms [ 0 ].Value = FormGuid;

      // 
      // Define the query string.
      // 
      _sqlQueryString = _sqlQueryView + " WHERE (TC_Guid = @FormGuid) ";

      if ( OrderBy.Length == 0 )
        {
        _sqlQueryString += " ORDER BY TCI_Order";
        }
      else
        {
        _sqlQueryString += "ORDER BY " + OrderBy;
        }

      this.LogValue ( "\r\n" + _sqlQueryString );

      // 
      // Execute the query against the database.
      // 
      using ( DataTable table = EvSqlMethods.RunQuery ( _sqlQueryString, cmdParms ) )
        {
        // 
        // Iterate through the results extracting the role information.
        // 
        for ( int Count = 0; Count < table.Rows.Count; Count++ )
          {
          // 
          // Extract the table row
          // 
          DataRow row = table.Rows [ Count ];

          // 
          // Process the results into the newField.
          // 
          option = new EvOption (
              EvSqlMethods.getString ( row, "TC_Guid" ),
              EvSqlMethods.getString ( row, "FieldId" ) + " - " + EvSqlMethods.getString ( row, "TCI_Subject" ) );

          if ( option.Description.Length > 80 )
            {
            option.Description = option.Description.Substring ( 0, 80 ) + " ...";
            }

          // 
          // Append the new FormField object to the array.
          // 
          list.Add ( option );

          }//END iteration loop

        }//END Using method
      // 
      // Pass back the result arrray.
      // 
      return list;

      }//END GetList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of options using VisitId and a FormId
    /// </summary>
    /// <param name="TrialId">string: a trial identifeir</param>
    /// <param name="FormId">string: a form identifier</param>
    /// <param name="OnlySingleFields">bool: select only single value fields.</param>
    /// <returns>List of EvOption: a list of options</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define the sql quey parameters and the sql query string. 
    /// 
    /// 2. Execute the sql query string with parameters and store the results on datatable. 
    /// 
    /// 3. Iterate through data table and extract data row to the option object 
    /// 
    /// 4. Add the option object to the list of the option object
    /// 
    /// 5. Return the Options list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> GetOptionList (
      String TrialId,
      String FormId,
      bool OnlySingleFields )
      {
      //
      // Initialize the debug log, a return list of options and an option object
      //
      this.LogMethod ( "GetOptionList. " );
      this.LogValue ( "TrialId: " + TrialId );
      this.LogValue ( "FormId: " + FormId );

      List<EvOption> list = new List<EvOption> ( );
      EvOption option = new EvOption ( );
      list.Add ( option );

      // 
      // Define the SQL query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter( PARM_CUSTOMER_GUID, SqlDbType.UniqueIdentifier),
        new SqlParameter( PARM_TrialId, SqlDbType.NVarChar,10 ),
        new SqlParameter( PARM_FormId, SqlDbType.NVarChar,10 ),
      };
      cmdParms [ 0 ].Value = this.ClassParameters.CustomerGuid;
      cmdParms [ 1 ].Value = TrialId;
      cmdParms [ 2 ].Value = FormId;

      // 
      // Define the query string.
      // 
      if ( OnlySingleFields == false )
        {
        _sqlQueryString = _sqlQueryView + "WHERE (" + EvFormFields.DB_CUSTOMER_GUID + "=" + EvFormFields.PARM_CUSTOMER_GUID + ") "
          + "\r\n AND (TrialId = @TrialId) "
          + " AND (FormId = @FormId) "
          + " AND (TC_State = '" + EvFormObjectStates.Form_Issued + "') "
          + " ORDER BY TCI_Order";
        }
      else
        {
        _sqlQueryString = _sqlQueryView + "WHERE (" + EvFormFields.DB_CUSTOMER_GUID + "=" + EvFormFields.PARM_CUSTOMER_GUID + ") "
          + "\r\n AND (TrialId = @TrialId) "
          + " AND (FormId = @FormId) "
          + " AND (TC_State = '" + EvFormObjectStates.Form_Issued + "') "
          + " AND (TCI_TypeId <> '" + Evado.Model.EvDataTypes.Check_Box_List + "') "
          + " AND (TCI_TypeId <> '" + Evado.Model.EvDataTypes.Table + "') "
          + " AND (TCI_TypeId <> '" + Evado.Model.EvDataTypes.Special_Matrix + "') "
          + " AND (TCI_TypeId <> '" + Evado.Model.EvDataTypes.Free_Text + "') "
          + " ORDER BY TCI_Order";
        }

      this.LogValue ( _sqlQueryString );

      // 
      // Execute the query against the database.
      //  
      using ( DataTable table = EvSqlMethods.RunQuery ( _sqlQueryString, cmdParms ) )
        {
        // 
        // Iterate through the results extracting the role information.
        // 
        for ( int Count = 0; Count < table.Rows.Count; Count++ )
          {
          // 
          // Extract the table row
          // 
          DataRow row = table.Rows [ Count ];

          // 
          // Process the results into the newField.
          // 
          option = new EvOption (
              EvSqlMethods.getString ( row, "FieldId" ),
              EvSqlMethods.getString ( row, "FieldId" ) + " - " + EvSqlMethods.getString ( row, "TCI_Subject" ) );

          if ( option.Description.Length > 80 )
            {
            option.Description = option.Description.Substring ( 0, 80 ) + " ...";
            }

          // 
          // Append the new FormField object to the array.
          // 
          list.Add ( option );

          }//END iteration loop

        }//END Using method

      // 
      // Pass back the result arrray.
      // 
      return list;

      }//END GetOptionList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of options using ProjectId and a safety report value.
    /// </summary>
    /// <param name="TrialId">string: (Mandatory) The trial identifier.</param>
    /// <returns>List of EvOption: a list of options</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. If safetyReport is selected, provide an option for all safety Report items.
    /// 
    /// 2. Define the sql query parameters and sql query string
    /// 
    /// 3. Execute the sql query string with parameters and store the results on datatable. 
    /// 
    /// 4. Iterate through data table and extract data row to the option object 
    /// 
    /// 5. Add the option object to the Options list. 
    /// 
    /// 6. Return the Options list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> getChartOptionList (
      String TrialId )
      {
      //
      // Initialize the debug log, a return list and an option object. 
      //
      this.LogMethod ( "getChartOptionList " );
      this.LogValue ( "TrialId: " + TrialId );

      List<EvOption> list = new List<EvOption> ( );
      EvOption option = new EvOption ( );
      list.Add ( option );

      // 
      // Define the SQL query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter( PARM_CUSTOMER_GUID, SqlDbType.UniqueIdentifier),
        new SqlParameter( PARM_TrialId, SqlDbType.NVarChar,10 ),
      };
      cmdParms [ 0 ].Value = this.ClassParameters.CustomerGuid;
      cmdParms [ 1 ].Value = TrialId;

      // 
      // Define the query string.
      // 
      _sqlQueryString = _sqlDataAnalysisQuery_List
        + "WHERE (" + EvFormFields.DB_CUSTOMER_GUID + "=" + EvFormFields.PARM_CUSTOMER_GUID + ") "
          + "\r\n AND  WHERE (TrialId = @TrialId) ;";

      this.LogDebug ( _sqlQueryString );

      // 
      // Execute the query against the database.
      // 
      using ( DataTable table = EvSqlMethods.RunQuery ( _sqlQueryString, cmdParms ) )
        {
        // 
        // Iterate through the results extracting the role information.
        // 
        for ( int Count = 0; Count < table.Rows.Count; Count++ )
          {
          // 
          // Extract the table row
          // 
          DataRow row = table.Rows [ Count ];

          //
          // Analytics option 
          var field = this.getChartOptionRowData ( row );

          //
          // if it is a non numeric field the continue to the next field.
          //
          if ( field.Design.hasNumericValues == false )
            {
            this.LogDebug ( "Form: {0}, Field: {1}, type: {2} is a non numeric field",
              field.FormId, field.FieldId, field.TypeId );
            continue;
            }
          this.LogDebug ( "Form: {0}, Field: {1}, type: {2} is a numeric field",
              field.FormId, field.FieldId, field.TypeId );


          option = new EvOption (
              field.FormId + EvChart.CONST_SOURCE_DELIMITER + field.FieldId,
              field.FormId + " : " + field.FieldId + " - " + field.Title );

          if ( option.Description.Length > 80 )
            {
            option.Description = option.Description.Substring ( 0, 80 ) + " ...";
            }

          // 
          // Append the new FormField object to the array.
          // 
          list.Add ( option );

          }//END iteration loop

        }//END Using method

      // 
      // Pass back the result arrray.
      // 
      return list;

      }//END GetList method.
    #endregion

    #region Form Field Report methods.

    // =====================================================================================
    /// <summary>
    /// This class generates the Report data object. 
    /// </summary>
    /// <param name="Report">EvReport: a report object</param>
    /// <returns>EvReport: a report object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Update the report title, datasource and report date. 
    /// 
    /// 2. If number of columns is less than four, add four new columns and their values
    /// 
    /// 3. Defines the sql query parameters and the sql query string. 
    /// 
    /// 4. Execute the sql query string with parameters and store the results on datatable. 
    /// 
    /// 5. Iterate through the table and extract data row to the formfield object. 
    /// 
    /// 6. Update the formfield object values to the reportRow columns. 
    /// 
    /// 7. Switch formfield typeId and update the third column value
    /// 
    /// 8. If the report data record is empty, add new report row with four columns. 
    /// 
    /// 9. Return the report object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvReport getReport ( EvReport Report )
      {
      //
      // Initialize the debug log
      //
      this.LogMethod ( "getReport. " );
      this.LogValue ( "Query count: " + Report.Queries.Length );
      this.LogValue ( "Columns count: " + Report.Columns.Count );

      this.LogValue ( "Parameters: " );

      for ( int i = 0; i < Report.Queries.Length; i++ )
        {
        this.LogValue ( Report.Queries [ i ].SelectionSource + " = " + Report.Queries [ i ].Value );
        }

      // 
      // Initialize a list of report rows, a formfield object and the number of report columns
      // 
      List<EvReportRow> reportRows = new List<EvReportRow> ( );
      EvFormField formField = new EvFormField ( );
      int inNoColumns = Report.Columns.Count;

      //
      // Update the report title, datasource and report date. 
      //
      Report.ReportTitle = "Form Field Properties";
      Report.DataSourceId = EvReport.ReportSourceCode.FormFields;
      Report.ReportDate = DateTime.Now;
      //
      // Define the report columns
      //
      Report.Columns = new List<EvReportColumn> ( );

      // 
      // Set the trial column 1
      // 
      EvReportColumn column = new EvReportColumn ( );
      column.HeaderText = "FieldId";
      column.SectionLvl = 0;
      column.GroupingIndex = false;
      column.DataType = EvReport.DataTypes.Text;
      column.SourceField = "FieldId";
      column.GroupingType = EvReport.GroupingTypes.None;
      column.StyleWidth = "100px";

      Report.Columns.Add ( column );

      // 
      // Set the trial column 2
      // 
      column = new EvReportColumn ( );
      column.HeaderText = "Title";
      column.SectionLvl = 0;
      column.GroupingIndex = false;
      column.DataType = EvReport.DataTypes.Text;
      column.SourceField = "Subject";
      column.GroupingType = EvReport.GroupingTypes.None;
      column.StyleWidth = "200px";

      Report.Columns.Add ( column );

      // 
      // Set the trial column 3
      // 
      column = new EvReportColumn ( );
      column.HeaderText = "Type";
      column.SectionLvl = 0;
      column.GroupingIndex = false;
      column.DataType = EvReport.DataTypes.Text;
      column.SourceField = "Type";
      column.GroupingType = EvReport.GroupingTypes.None;
      column.StyleWidth = "100px";

      Report.Columns.Add ( column );

      // 
      // Set the trial column 4
      // 
      column = new EvReportColumn ( );
      column.HeaderText = "Parameters";
      column.SectionLvl = 0;
      column.GroupingIndex = false;
      column.DataType = EvReport.DataTypes.Text;
      column.SourceField = "Type";
      column.GroupingType = EvReport.GroupingTypes.None;
      column.StyleWidth = "400px";

      Report.Columns.Add ( column );

      // 
      // Set the trial column 5
      // 
      column = new EvReportColumn ( );
      column.HeaderText = "Hidden Field";
      column.SectionLvl = 0;
      column.GroupingIndex = false;
      column.DataType = EvReport.DataTypes.Bool;
      column.SourceField = "Hidden";
      column.GroupingType = EvReport.GroupingTypes.None;
      column.StyleWidth = "50px";

      Report.Columns.Add ( column );

      this.LogValue ( "Report.Column.count: " + Report.Columns.Count );

      foreach ( EvReportColumn col in Report.Columns )
        {
        this.LogValue ( "HeaderText: " + col.HeaderText );
        }

      // 
      // Define the query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter( PARM_CUSTOMER_GUID, SqlDbType.UniqueIdentifier),
        new SqlParameter( PARM_TrialId, SqlDbType.NVarChar,10 ),
        new SqlParameter( PARM_FormId, SqlDbType.NVarChar,10 ),
      };
      cmdParms [ 0 ].Value = this.ClassParameters.CustomerGuid;

      //
      // Extract the parameters from the parameter list.
      //
      for ( int i = 0; i < Report.Queries.Length; i++ )
        {
        if ( Report.Queries [ i ].SelectionSource == EvReport.SelectionListTypes.Current_Trial )
          {
          cmdParms [ 1 ].Value = Report.Queries [ i ].Value;
          }
        if ( Report.Queries [ i ].SelectionSource == EvReport.SelectionListTypes.Form_Id )
          {
          cmdParms [ 2 ].Value = Report.Queries [ i ].Value;
          }
        }//END parameter iteration loop.

      //
      // Generate the SQL query string.
      //
      _sqlQueryString = _sqlQueryView
        + "WHERE (" + EvFormFields.DB_CUSTOMER_GUID + "=" + EvFormFields.PARM_CUSTOMER_GUID + ") "
        + "\r\n AND(TrialId = @TrialId) "
        + "\r\n AND (FormId = @FormId) "
        + "\r\n AND (TC_State = '" + EvFormObjectStates.Form_Issued + "') "
        + " ORDER BY TCI_Order";

      this.LogValue ( "SQL QUERY: " + _sqlQueryString );

      //
      // Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( _sqlQueryString, cmdParms ) )
        {
        this.LogValue ( "Returned Records: " + table.Rows.Count );

        // 
        // Iterate through the results extracting the role information.
        // 
        for ( int count = 0; count < table.Rows.Count; count++ )
          {
          // 
          // Extract the table row to formfield object. 
          // 
          DataRow row = table.Rows [ count ];

          formField = this.getRowData ( row );

          this.LogValue ( "Field: " + formField.FieldId
            + " - " + formField.Title
            + " Type: " + formField.Type );

          EvReportRow reportRow = new EvReportRow ( Report.Columns.Count );

          //
          // Update the formfield values to the reportRow column
          //
          reportRow.ColumnValues [ 0 ] = formField.FieldId;
          reportRow.ColumnValues [ 1 ] = formField.Title;
          reportRow.ColumnValues [ 2 ] = formField.Type;

          //
          // Switch formfield typeId and update the third column value
          //
          switch ( formField.TypeId )
            {
            case Evado.Model.EvDataTypes.Check_Box_List:
            case Evado.Model.EvDataTypes.Radio_Button_List:
            case Evado.Model.EvDataTypes.Selection_List:
            case Evado.Model.EvDataTypes.Horizontal_Radio_Buttons:
                {
                reportRow.ColumnValues [ 3 ] = "Options: " + formField.Design.Options;
                break;
                }
            case Evado.Model.EvDataTypes.Numeric:
                {
                reportRow.ColumnValues [ 3 ] = "Val. Min: " + formField.ValidationRules.ValidationLowerLimit
                  + " Val. Max: " + formField.ValidationRules.ValidationUpperLimit
                  + " Alert. Min: " + formField.ValidationRules.AlertLowerLimit
                  + " Alert. Max: " + formField.ValidationRules.AlertUpperLimit;
                if ( formField.Design.Unit != String.Empty )
                  {
                  reportRow.ColumnValues [ 3 ] += " Unit: " + formField.Design.UnitHtml;
                  }
                break;
                }
            case Evado.Model.EvDataTypes.Table:
            case Evado.Model.EvDataTypes.Special_Matrix:
                {
                reportRow.ColumnValues [ 3 ] = String.Empty;
                foreach ( EvFormFieldTableColumnHeader header in formField.Table.Header )
                  {
                  reportRow.ColumnValues [ 3 ] += "Col: " + header.No
                    + " Title: " + header.Text
                    + " Type: " + header.TypeId + " ";
                  }
                break;
                }
            }
          reportRow.ColumnValues [ 4 ] = formField.Design.HideField.ToString ( );

          //
          // Add the reportRow to the report data records. 
          //
          Report.DataRecords.Add ( reportRow );

          }//END record interation loop.

        }//END using statement.

      this.LogValue ( "Report row count: " + reportRows.Count );

      //
      // If the report data record is empty, add new report row with four columns. 
      //
      if ( Report.DataRecords.Count == 0 )
        {
        EvReportRow reportRow = new EvReportRow ( Report.Columns.Count );

        for ( int i = 0; i < Report.Columns.Count; i++ )
          {
          reportRow.ColumnValues [ i ] = String.Empty;
          }
        Report.DataRecords.Add ( reportRow );

        }//END ADD EMPTY ROW

      // 
      // Return the result array.
      // 
      return Report;
      }

    #endregion

    #region Retrieval Queries

    // ==================================================================================
    /// <summary>
    /// This class gets FormField data object by its unique object identifier.
    /// </summary>
    /// <param name="Uid">Long: a unique identifier</param>
    /// <returns>EvFormField: a FormField data object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Validate whether the unique identifier is not empty. 
    /// 
    /// 2. Define the sql query parameters and the sql query string. 
    /// 
    /// 3. Execute the sql query string with parameters and store the results on the datatable. 
    /// 
    /// 4. Extract the first datarow to the formfield object. 
    /// 
    /// 5. Return the Formfield data object. 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public EvFormField getField ( long Uid )
      {
      //
      // Initialize the debug log and a return formfield object. 
      //
      this.LogMethod ( "getField" );
      this.LogValue ( "Uid: " + Uid );

      EvFormField formField = new EvFormField ( );

      // 
      // Validate whether the Unique identifier is not zero
      // 
      if ( Uid == 0 )
        {
        return formField;
        }

      // 
      // Define the query parameters.
      // 
      SqlParameter cmdParms = new SqlParameter ( PARM_Guid, SqlDbType.UniqueIdentifier );
      cmdParms.Value = Uid;

      // 
      // Define the query string.
      // 
      _sqlQueryString = _sqlQueryView + " WHERE (TCI_Guid = @Guid ); ";

      // 
      // Execute the query against the database.
      // 
      using ( DataTable table = EvSqlMethods.RunQuery ( _sqlQueryString, cmdParms ) )
        {
        // 
        // If not rows the return
        // 
        if ( table.Rows.Count == 0 )
          {
          return formField;
          }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        // 
        // Fill the role object.
        // 
        formField = this.getRowData ( row );

        }//END Using method

      // 
      // Pass back the data object.
      // 
      return formField;

      }//END getField method

    // ==================================================================================
    /// <summary>
    /// This class retrieves formfield values using field Guid
    /// </summary>
    /// <param name="FieldGuid">Guid: (Mandatory) a field global unique object identifier.</param>
    /// <returns>EvFormField: a FormField data object</returns>
    /// <remarks>
    /// This class consists of the following steps: 
    /// 
    /// 1. Return an empty formfield object if the formfield's Guid is empty 
    /// 
    /// 2. Define the sql query parameters and the sql query string. 
    /// 
    /// 3. Execute the sql query string with parameters and store the results on the datatable. 
    /// 
    /// 4. Extract the first datarow to the formfield object. 
    /// 
    /// 5. Return the formfield object.
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public EvFormField getField ( Guid FieldGuid )
      {
      //
      // Initialize the debug log and a return formfield object.
      //
      this.LogMethod ( "getField" );
      this.LogValue ( "FieldGuid: " + FieldGuid );

      EvFormField formField = new EvFormField ( );

      // 
      // Validate whether the field Guid is not empty.
      // 
      if ( FieldGuid == Guid.Empty )
        {
        return formField;
        }

      // 
      // Define the query parameters.
      // 
      SqlParameter cmdParms = new SqlParameter ( PARM_Guid, SqlDbType.UniqueIdentifier );
      cmdParms.Value = FieldGuid;

      // 
      // Define the query string.
      // 
      _sqlQueryString = _sqlQueryView + " WHERE (TCI_Guid = @Guid ); ";

      // 
      // Execute the query against the database.
      // 
      using ( DataTable table = EvSqlMethods.RunQuery ( _sqlQueryString, cmdParms ) )
        {
        // 
        // If not rows the return
        // 
        if ( table.Rows.Count == 0 )
          {
          return formField;
          }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        // 
        // Fill the role object.
        // 
        formField = this.getRowData ( row );

        }//END Using method

      // 
      // Pass back the data object.
      // 
      return formField;

      }// END getField method

    // ==================================================================================
    /// <summary>
    /// This class gets FormField data object using TrialId, FormUid and FieldId
    /// </summary>
    /// <param name="ProjectId">string: a trial identifier</param>
    /// <param name="FormGuid">Integer: a form unique identifier</param>
    /// <param name="FieldId">string: a field identifier</param>
    /// <returns>EvFormField: a FormField data object</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Return an empty formfield object if the TrialId and FieldId are empty
    /// 
    /// 2. Define sql query parameters and sql query string. 
    /// 
    /// 3. Execute the sql query string with parameters and store the results on data table. 
    /// 
    /// 4. Extract the first data row to the formfield object. 
    /// 
    /// 5. If formfield type is external selection list, add the relevant coding visitSchedule items.
    /// 
    /// 6. Return a formfield object. 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public EvFormField getField ( string ProjectId, Guid FormGuid, string FieldId )
      {
      //
      // Initialize the debug log and a return formfield object
      //
      this.LogMethod ( "getField" );
      this.LogValue ( "ProjectId: " + ProjectId
        + " FormGuid: " + FormGuid
        + " FieldId: " + FieldId );

      EvFormField formField = new EvFormField ( );

      // 
      // Validate whether the VisitId and FieldId are not empty
      // 
      if ( ProjectId == String.Empty && FieldId == String.Empty )
        {
        return formField;
        }

      // 
      // Define the query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter( PARM_TrialId, SqlDbType.NVarChar, 10),
        new SqlParameter( PARM_FormGuid, SqlDbType.UniqueIdentifier),
        new SqlParameter( PARM_FieldId, SqlDbType.NVarChar, 20),
      };
      cmdParms [ 0 ].Value = ProjectId;
      cmdParms [ 1 ].Value = FormGuid;
      cmdParms [ 2 ].Value = FieldId;

      // 
      // Define the query string.
      // 
      if ( FormGuid == Guid.Empty )
        {
        _sqlQueryString = _sqlQueryView + " WHERE (TrialId = @TrialId ) "
          + " AND (FieldId = @FieldId )  ";
        }
      else
        {
        _sqlQueryString = _sqlQueryView + " WHERE (TrialId = @TrialId ) "
         + " AND (TC_Guid = @FormGuid )  "
         + " AND (FieldId = @FieldId )  ";
        }

      // 
      // Execute the query against the database.
      // 
      using ( DataTable table = EvSqlMethods.RunQuery ( _sqlQueryString, cmdParms ) )
        {
        // 
        // If not rows the return
        // 
        if ( table.Rows.Count == 0 )
          {
          return formField;
          }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        // 
        // Fill the role object.
        // 
        formField = this.getRowData ( row );

        }//END Using method

      // 
      // If formfield type is external selection list, add the relevant coding visitSchedule items.
      // 
      if ( formField.TypeId == Evado.Model.EvDataTypes.External_Selection_List )
        {
        EvFormFieldSelectionLists externalCodingLists = new EvFormFieldSelectionLists ( );

        formField.Design.Options = externalCodingLists.getItemCodingList (
          formField.Design.ExSelectionListId,
          formField.Design.ExSelectionListCategory );
        }

      // 
      // Pass back the data object.
      // 
      return formField;

      }// END getField method

    #endregion

    #region FormFields Update queries

    // ==================================================================================
    /// <summary>
    /// This class update items on formfield table using retrieving formfield items values. 
    /// </summary>
    /// <param name="FormField">EvFormField: a formfield object</param>
    /// <returns>EvEventCodes: an event code for updating items on formfield object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the Old formfield's Uid is not defined. 
    /// 
    /// 2. Add items to datachange object if they do not exist. 
    /// 
    /// 3. Define sql query parameters and execute the storeprocedure for updating items on formfield table. 
    /// 
    /// 4. Add datachange object values to the backup datachanges object. 
    /// 
    /// 5. Return the event code for updating items. 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public EvEventCodes UpdateItem ( EvFormField FormField )
      {
      this.LogMethod ( "updateItem method. " );
      this.LogValue ( "ProjectId: " + FormField.TrialId );
      this.LogValue ( "FormId: " + FormField.FormId );
      this.LogValue ( "FieldId: " + FormField.FieldId );
      //
      // Initialize debug log and the old formfield object
      //
      EvDataChanges dataChanges = new EvDataChanges ( );

      //
      // retrieve the previous version of the field.
      //
      EvFormField oldItem = this.getField ( FormField.Guid );
      if ( oldItem.Guid == Guid.Empty )
        {
        this.LogValue ( "FormId not returned." );

        return EvEventCodes.Identifier_Form_Id_Error;
        }

      //
      // Create the data change object.
      //
      EvDataChange dataChange = this.SetDataChange ( oldItem, FormField );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] _cmdParms = GetParameters ( );
      setParameters ( _cmdParms, FormField );

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( _storedProcedureUpdateItem, _cmdParms ) == 0 )
        {
        return EvEventCodes.Database_Record_Update_Error;
        }
      // 
      // Add the change record
      // 
      dataChanges.AddItem ( dataChange );

      return EvEventCodes.Ok;

      }//END UpdateItem method

    // =====================================================================================
    /// <summary>
    ///  This method sets the data change values for the object and returns the data change
    ///  object.
    /// </summary>
    /// <param name="OldField">EvFormField: A old field object.</param>
    /// <param name="NewField">EvFormField: A new field object.</param>
    /// <returns>EvDataChange: A DataChange object.</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Add items to datachange object if they do not exist on the old Subject Record object. 
    /// 
    /// 2. Return the datachange object. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private EvDataChange SetDataChange ( EvFormField OldField, EvFormField NewField )
      {
      this.LogMethod ( "SetDataChange method. " );
      // 
      // Initialise the methods variables and objects.
      // 
      EvDataChange dataChange = new EvDataChange ( );


      // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      // Compare the objects.
      // Initialize datachange object and a backup datachange object. 
      //
      dataChange.Guid = Guid.NewGuid ( );
      dataChange.TableName = EvDataChange.DataChangeTableNames.EvFormFields;
      //dataChange.RecordUid = 1;
      dataChange.RecordGuid = NewField.Guid;
      dataChange.UserId = NewField.UpdatedByUserId;
      dataChange.DateStamp = DateTime.Now;

      //
      // Add items to the datachange object if they do not exist in the old form field object. 
      //
      dataChange.AddItem ( "ItemId", OldField.FieldId, NewField.FieldId );

      dataChange.AddItem ( "TypeId", OldField.TypeId.ToString ( ), NewField.TypeId.ToString ( ) );

      dataChange.AddItem ( "Subject", OldField.Design.Title, NewField.Design.Title );

      dataChange.AddItem ( "Instructions", OldField.Design.Instructions, NewField.Design.Instructions );

      dataChange.AddItem ( "Reference", OldField.Design.HttpReference, NewField.Design.HttpReference );

      dataChange.AddItem ( "Options", OldField.Design.Options, NewField.Design.Options );

      dataChange.AddItem ( "Unit", OldField.Design.Unit, NewField.Design.Unit );

      if ( OldField.ValidationRules != null
        && NewField.ValidationRules != null )
        {
        dataChange.AddItem ( "Validation_NotValidForFemale",
          OldField.ValidationRules.NotValidForFemale,
          NewField.ValidationRules.NotValidForFemale );

        dataChange.AddItem ( "Validation_NotValidForMale",
          OldField.ValidationRules.NotValidForMale,
          NewField.ValidationRules.NotValidForMale );

        dataChange.AddItem ( "Design_JavaScript",
          OldField.Design.JavaScript,
          NewField.Design.JavaScript );

        if ( OldField.ValidationRules.NotValidOptions != null
          && NewField.ValidationRules.NotValidOptions != null )
          {

          for ( int count = 0; count < NewField.ValidationRules.NotValidOptions.Rules.Count; count++ )
            {
            if ( OldField.ValidationRules.NotValidOptions.Rules.Count
              <= count )
              {
              dataChange.AddItem ( "Validation_Options_Rules_" + count,
                "",
                NewField.ValidationRules.NotValidOptions.Rules [ count ] );
              }
            else
              {
              dataChange.AddItem ( "Validation_Options_Rules_" + count,
                OldField.ValidationRules.NotValidOptions.Rules [ count ],
                NewField.ValidationRules.NotValidOptions.Rules [ count ] );
              }
            }
          }

        dataChange.AddItem ( "Validation_IsAfterBirthDate",
          OldField.ValidationRules.IsAfterBirthDate,
          NewField.ValidationRules.IsAfterBirthDate );

        dataChange.AddItem ( "Validation_IsAfterConsentDate",
          OldField.ValidationRules.IsAfterConsentDate,
          NewField.ValidationRules.IsAfterConsentDate );

        dataChange.AddItem ( "Validation_WithinDaysOfRecordDate",
          OldField.ValidationRules.WithinDaysOfRecordDate,
          NewField.ValidationRules.WithinDaysOfRecordDate );

        dataChange.AddItem ( "Validation_WithinDaysOfVisitDate",
          OldField.ValidationRules.WithinDaysOfVisitDate,
          NewField.ValidationRules.WithinDaysOfVisitDate );


        dataChange.AddItem ( "Validation_AlertLowerLimit",
          OldField.ValidationRules.AlertLowerLimit,
          NewField.ValidationRules.AlertLowerLimit );

        dataChange.AddItem ( "Validation_AlertUpperLimit",
          OldField.ValidationRules.AlertUpperLimit,
          NewField.ValidationRules.AlertUpperLimit );

        dataChange.AddItem ( "Validation_NormaRangeLowerLimit",
          OldField.ValidationRules.NormalRangeLowerLimit,
          NewField.ValidationRules.NormalRangeLowerLimit );

        dataChange.AddItem ( "Validation_NormaRangeUpperLimit",
          OldField.ValidationRules.NormalRangeUpperLimit,
          NewField.ValidationRules.NormalRangeUpperLimit );

        dataChange.AddItem ( "Validation_ValidationLowerLimit",
          OldField.ValidationRules.ValidationLowerLimit,
          NewField.ValidationRules.ValidationLowerLimit );

        dataChange.AddItem ( "Validation_SafetyUpperLimit",
          OldField.ValidationRules.ValidationUpperLimit,
          NewField.ValidationRules.ValidationUpperLimit );
        }

      if ( OldField.Table != null
        && NewField.Table != null )
        {
        for ( int index = 0; index < NewField.Table.Header.Length; index++ )
          {
          EvFormFieldTableColumnHeader oldHeader = OldField.Table.Header [ index ];
          EvFormFieldTableColumnHeader newHeader = NewField.Table.Header [ index ];

          dataChange.AddItem ( "Field_Table_Header_No_" + index, oldHeader.No, newHeader.No );

          dataChange.AddItem ( "Field_Table_Header_Text_" + index, oldHeader.Text, newHeader.Text );

          dataChange.AddItem ( "Field_Table_Header_TypeId_" + index, oldHeader.TypeId, newHeader.TypeId );

          dataChange.AddItem ( "Field_Table_Header_Width_" + index, oldHeader.Width, newHeader.Width );

          dataChange.AddItem ( "Field_Table_Header_ColumnId_" + index, oldHeader.ColumnId, newHeader.ColumnId );

          dataChange.AddItem ( "Field_Table_Header_OptionsOrUnit_" + index, oldHeader.OptionsOrUnit, newHeader.OptionsOrUnit );
          }

        if ( NewField.TypeId == Evado.Model.EvDataTypes.Special_Matrix
          && NewField.Table.Rows.Count > 0 )
          {
          for ( int count = 0; count < NewField.Table.Rows.Count; count++ )
            {
            if ( OldField.Table.Rows.Count <= count )
              {
              for ( int index = 0; index < NewField.Table.Header.Length; index++ )
                {
                dataChange.AddItem (
                  "Field_Matrix_R" + count + "C" + index,
                  "",
                  NewField.Table.Rows [ count ].Column [ index ] );
                }
              }
            else
              {
              for ( int index = 0; index < NewField.Table.Header.Length; index++ )
                {
                dataChange.AddItem (
                  "Field_Matrix_R" + count + "C" + index,
                  OldField.Table.Rows [ count ].Column [ index ],
                  NewField.Table.Rows [ count ].Column [ index ] );
                }
              }
            }
          }

        }//END Table difference block

      if ( NewField.Design != null
        && OldField.Design != null )
        {
        dataChange.AddItem (
          "Field_Design_AnalogueLegendFinish",
          OldField.Design.AnalogueLegendFinish,
          NewField.Design.AnalogueLegendFinish );

        dataChange.AddItem (
          "Field_Design_AnalogueLegendStart",
          OldField.Design.AnalogueLegendStart,
          NewField.Design.AnalogueLegendStart );

        dataChange.AddItem (
          "Field_Design_AnalogueScale",
          OldField.Design.AnalogueScale,
          NewField.Design.AnalogueScale );

        dataChange.AddItem (
          "Field_Design_DataPoint",
          OldField.Design.DataPoint,
          NewField.Design.DataPoint );

        dataChange.AddItem (
          "Field_Design_DefaultValue",
          OldField.Design.DefaultValue,
          NewField.Design.DefaultValue );

        dataChange.AddItem (
          "Field_Design_DictionaryGuid",
          OldField.Design.DictionaryGuid,
          NewField.Design.DictionaryGuid );

        dataChange.AddItem (
          "Field_Design_ExSelectionListCategory",
          OldField.Design.ExSelectionListCategory,
          NewField.Design.ExSelectionListCategory );

        dataChange.AddItem (
          "Field_Design_ExSelectionListId",
          OldField.Design.ExSelectionListId,
          NewField.Design.ExSelectionListId );

        dataChange.AddItem (
          "Field_Design_FieldCategory",
          OldField.Design.FieldCategory,
          NewField.Design.FieldCategory );

        dataChange.AddItem (
          "Field_Design_FormIds",
          OldField.Design.FormIds,
          NewField.Design.FormIds );

        dataChange.AddItem (
          "Field_Design_HideField",
          OldField.Design.HideField,
          NewField.Design.HideField );

        dataChange.AddItem (
          "Field_Design_HorizontalButtons,",
          OldField.Design.HorizontalButtons,
          NewField.Design.HorizontalButtons );

        dataChange.AddItem (
          "Field_Design_InitialOptionList",
          OldField.Design.InitialOptionList,
          NewField.Design.InitialOptionList );

        dataChange.AddItem (
          "Field_Design_Instructions",
          OldField.Design.Instructions,
          NewField.Design.Instructions );

        dataChange.AddItem (
          "Field_Design_Mandatory",
          OldField.Design.Mandatory,
          NewField.Design.Mandatory );

        dataChange.AddItem (
          "Field_Design_MultiLineTextField",
          OldField.Design.MultiLineTextField,
          NewField.Design.MultiLineTextField );

        dataChange.AddItem (
          "Field_Design_Options",
          OldField.Design.Options,
          NewField.Design.Options );

        dataChange.AddItem (
          "Field_Design_Order",
          OldField.Design.Order,
          NewField.Design.Order );

        dataChange.AddItem (
          "Field_Design_Reference",
          OldField.Design.HttpReference,
          NewField.Design.HttpReference );

        dataChange.AddItem (
          "Field_Design_SafetyReport",
          OldField.Design.SafetyReport,
          NewField.Design.SafetyReport );

        dataChange.AddItem (
          "Field_Design_Section",
          OldField.Design.Section,
          NewField.Design.Section );

        dataChange.AddItem (
          "Field_Design_SelectByCodingValue",
          OldField.Design.SelectByCodingValue,
          NewField.Design.SelectByCodingValue );

        dataChange.AddItem (
          "Field_Design_Subject,",
          OldField.Design.Title,
          NewField.Design.Title );

        dataChange.AddItem (
          "Field_Design_SummaryField",
          OldField.Design.SummaryField,
          NewField.Design.SummaryField );

        dataChange.AddItem (
          "Field_Design_Unit",
          OldField.Design.Unit,
          NewField.Design.Unit );

        dataChange.AddItem (
          "Field_Design_UnitScaling",
          OldField.Design.UnitScaling,
          NewField.Design.UnitScaling );

        }//END field design difference block.

      return dataChange;

      }//END SetDataChange method

    // ==================================================================================
    /// <summary>
    /// This class adds items to formfield table.
    /// </summary>
    /// <param name="FormField">EvFormField: a formfield object</param>
    /// <returns>EvEventCodes: an event code for adding items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the FormUid or Uid is duplicated. 
    /// 
    /// 3. Define the sql query parameters and execute the storeprocedure for adding items. 
    /// 
    /// 4. Return the event code for adding items.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes AddItem ( EvFormField FormField )
      {
      //
      // Initialize the debug log, a local sql query string and a new formfield object. 
      //
      this.LogMethod ( "AddItem method. " );
      this.LogValue ( "FormGuid: " + FormField.FormGuid );
      this.LogValue ( "FieldId: " + FormField.FieldId );

      string _sqlQueryString = String.Empty;

      EvFormField newField = this.getField ( FormField.TrialId, FormField.FormGuid, FormField.FieldId );

      //
      // Validate whether the formUid and Uid do not exist. 
      //
      if ( newField.FormGuid != Guid.Empty )
        {
        return EvEventCodes.Data_Duplicate_Field_Error;
        }//END  FormUid

      if ( newField.Guid != Guid.Empty
        && ( newField.Title != FormField.Title
         || newField.TypeId != FormField.TypeId ) )
        {
        return EvEventCodes.Data_Duplicate_Field_Error;
        }//END Uid

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] _cmdParms = GetParameters ( );
      setParameters ( _cmdParms, FormField );

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( _storedProcedureAddItem, _cmdParms ) == 0 )
        {
        return EvEventCodes.Database_Record_Update_Error;
        }

      return EvEventCodes.Ok;

      }//END AddItem class. 

    // =====================================================================================
    /// <summary>
    /// This class deletes items from formfield table
    /// </summary>
    /// <param name="Field">EvFormField: a formfield object</param>
    /// <returns>EvEventCodes: an event code for deleting items from formfield table</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the VisitId or User common name is not defined 
    /// 
    /// 2. Define the sql query parameters and execute the storeprocedure for deleting items. 
    /// 
    /// 3. Return the event code for deleting the items from formfield table.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes DeleteItem ( EvFormField Field )
      {
      //
      // Initialize the debug log
      //
      this.LogMethod ( "DeleteItem method." );
      this.LogValue ( "ProjectId: " + Field.TrialId );
      this.LogValue ( "FieldId: " + Field.FieldId );

      // 
      // Validate whether the VisitId and Usercommon name exist. 
      // 
      if ( Field.Guid == Guid.Empty )
        {
        return EvEventCodes.Identifier_Global_Unique_Identifier_Error;
        }
      if ( Field.UserCommonName == String.Empty )
        {
        return EvEventCodes.Identifier_User_Id_Error;
        }

      // 
      // Define the query parameters.
      // 
      SqlParameter [ ] _cmdParms = new SqlParameter [ ]
      {
        new SqlParameter(PARM_Guid, SqlDbType.UniqueIdentifier),
        new SqlParameter(PARM_UpdatedByUserId, SqlDbType.NVarChar,100),
        new SqlParameter(PARM_UpdatedBy, SqlDbType.NVarChar,30),
        new SqlParameter(PARM_UpdateDate, SqlDbType.DateTime)
      };
      _cmdParms [ 0 ].Value = Field.Guid;
      _cmdParms [ 1 ].Value = Field.UpdatedByUserId;
      _cmdParms [ 2 ].Value = Field.UserCommonName;
      _cmdParms [ 3 ].Value = DateTime.Now;

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( _storedProcedureDeleteItem, _cmdParms ) == 0 )
        {
        return EvEventCodes.Database_Record_Update_Error;
        }

      return EvEventCodes.Ok;

      }//END DeleteItem method
    #endregion

    }//END EvFormFields class

  }//END namespace Evado.Dal.Clinical
