/* <copyright file="DAL\EvForms.cs" company="EVADO HOLDING PTY. LTD.">
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

//References to Evado specific libraries
using Evado.Model;
using Evado.Model.Digital;

namespace Evado.Dal.Clinical
{
  /// <summary>
  /// This class is handles the data access layer for the form data object.
  /// </summary>
  public class EvForms : EvDalBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EvForms ( )
    {
      this.ClassNameSpace = "Evado.Dal.Clinical.EvForms.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EvForms ( EvClassParameters Settings )
    {
      this.ClassParameters = Settings;
      this.ClassNameSpace = "Evado.Dal.Clinical.EvForms.";

      if ( this.ClassParameters.LoggingLevel == 0 )
      {
        this.ClassParameters.LoggingLevel = Evado.Dal.EvStaticSetting.LoggingLevel;
      }

      this._Dal_FormSections = new EvFormSections ( this.ClassParameters );

      this._Dal_FormFields = new EvFormFields ( this.ClassParameters );
    }

    #endregion

    #region Object Initialisation
    /* *********************************************************************************
     * 
     * Defines the classes constansts and global variables
     * 
     * *********************************************************************************/

    //
    // Define the Selection selectionList query string constant.
    //
    private const string _sqlQuery_View = "Select * FROM EvForm_View ";

    // 
    // The SQL Store Procedure constants
    // 
    private const string _STORED_PROCEDURE_AddItem = "usr_Form_add";
    private const string _STORED_PROCEDURE_UpdateItem = "usr_Form_update";
    private const string _STORED_PROCEDURE_DeleteItem = "usr_Form_delete";
    private const string _STORED_PROCEDURE_WithdrawItem = "usr_Form_withdraw";
    private const string _STORED_PROCEDURE_CopyItem = "usr_Form_copy";

    //
    // The field and parameter values for the SQl customer filter 
    //
    private const string DB_CUSTOMER_GUID = "CU_GUID";
    private const string PARM_CUSTOMER_GUID = "@CUSTOMER_GUID";

    /// <summary>
    /// Define the query parameter names.
    /// </summary>
    private const string PARM_Guid = "@Guid";
    private const string PARM_Uid = "@Uid";
    private const string PARM_TypeId = "@TypeId";
    private const string PARM_FormId = "@FormId";
    private const string PARM_Title = "@Title";
    private const string PARM_Sampled = "@Sampled";
    private const string PARM_Reference = "@Reference";
    private const string PARM_Instructions = "@Instructions";
    private const string PARM_TrialId = "@TrialId";
    private const string PARM_XmlValidationRules = "@XmlValidationRules";
    private const string PARM_XmlData = "@XmlData";
    private const string PARM_Authors = "@Authors";
    private const string PARM_ReviewedByUserId = "@ReviewedByUserId";
    private const string PARM_ReviewedBy = "@ReviewedBy";
    private const string PARM_ReviewedDate = "@ReviewedDate";
    private const string PARM_ApprovedByUserId = "@ApprovedByUserId";
    private const string PARM_ApprovedBy = "@ApprovedBy";
    private const string PARM_ApprovalDate = "@ApprovalDate";
    private const string PARM_Version = "@Version";
    private const string PARM_State = "@State";
    private const string PARM_UpdatedByUserId = "@UpdatedByUserId";
    private const string PARM_UpdatedBy = "@UpdatedBy";
    private const string PARM_UpdateDate = "@UpdateDate";
    private const string PARM_Copy = "@Copy";
    //new columns
    private const string PARM_TEMPLATE_NAME = "@TEMPLATE_NAME";
    private const string PARM_JAVA_VALIDATION_SCRIPT = "@JAVA_VALIDATION_SCRIPT";
    private const string PARM_FORM_CATEGORY = "@FORM_CATEGORY";
    private const string PARM_HAS_CS_SCRIPT = "@HAS_CS_SCRIPT";
    private const string PARM_HAS_SECTION_NAVIGATION = "@HAS_SECTION_NAVIGATION";
    private const string PARM_HIDDEN_FIELDS = "@HIDDEN_FIELDS";
    private const string PARM_REGULATORY_TEMPLATE = "@REGULATORY_TEMPLATE";
    private const string PARM_HIDE_ANNOTATION_DURING_EDITING = "@HIDE_ANNOTATION_DURING_EDITING";
    private const string PARM_TRIAL_SITES = "@TRIAL_SITES";
    private const string PARM_GROUPS = "@GROUPS";
    private const string PARM_DESCRIPTION = "@DESCRIPTION";
    private const string PARM_UPDATE_REASON = "@UPDATE_REASON";




    /// <summary>
    /// This constant defines a string parameter as CDASH_METADATA for the Subject object. 
    /// </summary>
    private const string PARM_CDASH_METADATA = "@CDASH_METADATA";

    EvFormSections _Dal_FormSections = new EvFormSections ( );
    EvFormFields _Dal_FormFields = new EvFormFields ( );
    // 
    // Define the SQL query string variable.
    //
    private string _sqlQueryString = String.Empty;

    // +++++++++++++++++++++++++++ END INITIALISATION SECTION ++++++++++++++++++++++++++++++
    #endregion

    #region Set Query Parameters

    // =====================================================================================
    /// <summary>
    /// This class sets an array of sql query parameters
    /// </summary>
    /// <returns>SqlParameter: an array of the sql query parameters</returns>
    /// <remarks>
    /// This method consists of the following step: 
    /// 
    /// 1. Create new array of sql query parameters.
    /// 
    /// 2. Return an array of sql query parameters. 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    private static SqlParameter [ ] GetParameters ( )
    {
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( PARM_Guid, SqlDbType.UniqueIdentifier),
        new SqlParameter( PARM_TrialId, SqlDbType.NVarChar, 10),
        new SqlParameter( PARM_FormId, SqlDbType.NVarChar, 20),
        new SqlParameter( PARM_Title, SqlDbType.NVarChar, 80),
        new SqlParameter( PARM_Reference, SqlDbType.NVarChar, 250),
        new SqlParameter( PARM_Instructions, SqlDbType.NText),
        new SqlParameter( PARM_TypeId, SqlDbType.VarChar, 25),
        new SqlParameter( PARM_XmlValidationRules, SqlDbType.NText),
        new SqlParameter( PARM_XmlData, SqlDbType.NText),
        new SqlParameter( PARM_Authors, SqlDbType.NText),
        new SqlParameter( PARM_ReviewedByUserId, SqlDbType.NVarChar, 50),
        new SqlParameter( PARM_ReviewedBy, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_ReviewedDate, SqlDbType.DateTime),
        new SqlParameter( PARM_ApprovedByUserId, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_ApprovedBy, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_ApprovalDate, SqlDbType.DateTime ),
        new SqlParameter( PARM_Version, SqlDbType.NVarChar, 5),
        new SqlParameter( PARM_State, SqlDbType.NVarChar, 20),
        new SqlParameter( PARM_UpdatedByUserId, SqlDbType.NVarChar,100),
        new SqlParameter( PARM_UpdatedBy, SqlDbType.NVarChar,30),
        new SqlParameter( PARM_UpdateDate, SqlDbType.DateTime),
        new SqlParameter( PARM_CDASH_METADATA, SqlDbType.NVarChar, 250),

        new SqlParameter( PARM_TEMPLATE_NAME, SqlDbType.NVarChar, 250),
        new SqlParameter( PARM_JAVA_VALIDATION_SCRIPT, SqlDbType.NVarChar, 250),
        new SqlParameter( PARM_FORM_CATEGORY, SqlDbType.NVarChar, 250),
        new SqlParameter( PARM_HAS_CS_SCRIPT, SqlDbType.Bit),
        new SqlParameter( PARM_HAS_SECTION_NAVIGATION, SqlDbType.Bit),
        new SqlParameter( PARM_HIDDEN_FIELDS, SqlDbType.NVarChar, 250),
        new SqlParameter( PARM_REGULATORY_TEMPLATE, SqlDbType.Bit),
        new SqlParameter( PARM_HIDE_ANNOTATION_DURING_EDITING, SqlDbType.Bit),
        new SqlParameter( PARM_TRIAL_SITES, SqlDbType.NVarChar, 250), 
        new SqlParameter( PARM_DESCRIPTION, SqlDbType.NText),   
        new SqlParameter( PARM_UPDATE_REASON, SqlDbType.NVarChar, 50),

        new SqlParameter( PARM_CUSTOMER_GUID, SqlDbType.UniqueIdentifier),
      
      };
      return cmdParms;
    }//END GetParameters class

    // =====================================================================================
    /// <summary>
    /// This class binds values parameters 
    /// </summary>
    /// <param name="cmdParms">SqlParameter: an array of Database parameters</param>
    /// <param name="Form">EvForm: Values to bind to parameters</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. If the FormUid is emptry create a new value.
    /// 
    /// 2. Fill the parameters array with the values from the form object. 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    private void SetParameters ( SqlParameter [ ] cmdParms, EvForm Form )
    {
      // 
      // If the FormUid is emptry create a new value.
      // 
      EvFormSections formsections = new EvFormSections ( );
      if ( Form.Guid == Guid.Empty )
      {
        Form.Guid = Guid.NewGuid ( );
      }

      // 
      // Fill the parameters array with the values from the form object. 
      // 
      cmdParms [ 0 ].Value = Form.Guid;
      cmdParms [ 1 ].Value = Form.TrialId.Trim ( );
      cmdParms [ 2 ].Value = Form.FormId.Trim ( );
      cmdParms [ 3 ].Value = Form.Design.Title;
      cmdParms [ 4 ].Value = Form.Design.Reference;
      cmdParms [ 5 ].Value = Form.Design.Instructions;
      cmdParms [ 6 ].Value = Form.Design.TypeId.ToString ( );
      cmdParms [ 7 ].Value = String.Empty;
      cmdParms [ 8 ].Value = String.Empty;
      cmdParms [ 9 ].Value = Form.Authors;

      cmdParms [ 10 ].Value = Form.ReviewedByUserId;
      cmdParms [ 11 ].Value = Form.ReviewedBy;
      cmdParms [ 12 ].Value = Form.ReviewedDate;
      cmdParms [ 13 ].Value = Form.ApprovedByUserId;
      cmdParms [ 14 ].Value = Form.ApprovedBy;
      cmdParms [ 15 ].Value = Form.ApprovalDate;
      cmdParms [ 16 ].Value = Form.Design.Version;
      cmdParms [ 17 ].Value = Form.State.ToString ( );
      cmdParms [ 18 ].Value = Form.UpdatedByUserId;
      cmdParms [ 19 ].Value = Form.UserCommonName;

      cmdParms [ 20 ].Value = DateTime.Now;
      cmdParms [ 21 ].Value = Form.cDashMetadata;
      cmdParms [ 22 ].Value = Form.Design.TemplateName;
      cmdParms [ 23 ].Value = Form.Design.JavaValidationScript;
      cmdParms [ 24 ].Value = Form.Design.FormCategory;
      cmdParms [ 25 ].Value = Form.Design.hasCsScript;
      cmdParms [ 26 ].Value = Form.Design.hasSectionNavigation;
      cmdParms [ 27 ].Value = Form.Design.HiddenFields;
      cmdParms [ 28 ].Value = Form.Design.RegulatoryTemplate;
      cmdParms [ 29 ].Value = Form.Design.OnEdit_HideFieldAnnotation;

      cmdParms [ 30 ].Value = Form.Design.TrialSites;
      cmdParms [ 31 ].Value = Form.Description;
      cmdParms [ 32 ].Value = Form.UpdateReason.ToString ( );
      cmdParms [ 33 ].Value = this.ClassParameters.CustomerGuid;

      }//END SetParameters class.

    #endregion

    #region form Reader

    // =====================================================================================
    /// <summary>
    /// This class extracts the content of the reader and loads the Checklist object 
    /// </summary>
    /// <param name="Row">DataRow: a data row object containing the query results</param>
    /// <returns>EvForm: a form data object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Extract the compatible data row values to the form object. 
    /// 
    /// 2. Update the form state and type to current enumeration.
    /// 
    /// 3. Return the form data object.
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    private EvForm getRowData ( DataRow Row )
    {
      //
      // Initialize the method status, a return form object and the title string.
      //
      this.LogMethod ( "getRowData method" );

      EvForm form = new EvForm ( );
      EvFormSections _Dal_FormSections = new EvFormSections ( );

      string Title = String.Empty;

      // 
      // Extract the compatible data row values to the form object. 
      // 
      form.CustomerGuid = EvSqlMethods.getGuid ( Row, EvForms.DB_CUSTOMER_GUID );
      form.Guid = EvSqlMethods.getGuid ( Row, "TC_Guid" );
      form.TrialId = EvSqlMethods.getString ( Row, "TrialId" );
      form.FormId = EvSqlMethods.getString ( Row, "FormId" );

      string stXmlDesign = EvSqlMethods.getString ( Row, "TC_XmlData" );
      if ( stXmlDesign != string.Empty )
      {
        form.Design = Evado.Model.EvStatics.DeserialiseObject<EvFormDesign> ( stXmlDesign );

      }
      else
      {
        form.Design.TemplateName = EvSqlMethods.getString ( Row, "TC_TEMPLATE_NAME" );
        form.Design.JavaValidationScript = EvSqlMethods.getString ( Row, "TC_JAVA_VALIDATION_SCRIPT" );
        form.Design.FormCategory = EvSqlMethods.getString ( Row, "TC_FORM_CATEGORY" );
        form.Design.hasCsScript = EvSqlMethods.getBool ( Row, "TC_HAS_CS_SCRIPT" );
        form.Design.hasSectionNavigation = EvSqlMethods.getBool ( Row, "TC_HAS_SECTION_NAVIGATION" );
        form.Design.HiddenFields = EvSqlMethods.getString ( Row, "TC_HIDDEN_FIELDS" );
        form.Design.RegulatoryTemplate = EvSqlMethods.getBool ( Row, "TC_REGULATORY_TEMPLATE" );
        form.Design.OnEdit_HideFieldAnnotation = EvSqlMethods.getBool ( Row, "TC_HIDE_ANNOTATION_DURING_EDITING" );
        form.Design.TrialSites = EvSqlMethods.getString ( Row, "TC_TRIAL_SITES" );

      }

      //
      // Retrieve the form selection list.
      //
      List<EvFormSection> sectionList = _Dal_FormSections.getSectionList ( form.Guid );
      if ( sectionList.Count > 0 )
      {
        this.LogValue ( "Form section exist and have been read in." );
        form.Design.FormSections = sectionList;
      }
      else
      {
        this.LogValue ( "No form section exist." );
        _Dal_FormSections.UpdateItem ( form );

        this.LogValue ( _Dal_FormSections.Log );
      }

      form.Design.Title = EvSqlMethods.getString ( Row, "TC_Title" );
      form.Design.Reference = EvSqlMethods.getString ( Row, "TC_Reference" );
      form.Design.Instructions = EvSqlMethods.getString ( Row, "TC_Instructions" );
      form.Description = EvSqlMethods.getString ( Row, "TC_DESCRIPTION" );
      form.UpdateReason = Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue<EvForm.UpdateReasonList> (
        EvSqlMethods.getString ( Row, "TC_UPDATE_REASON" ) );
      if ( form.Design.TypeId == EvFormRecordTypes.Null )
      {
        string value = EvSqlMethods.getString ( Row, "TC_TypeId" );

        if ( value == "Questionnaire" )
        {
          value = EvFormRecordTypes.Questionnaire.ToString ( );
        }

        form.Design.TypeId = Evado.Model.EvStatics.Enumerations.parseEnumValue<EvFormRecordTypes> ( value );
      }

      form.Authors = EvSqlMethods.getString ( Row, "TC_Authors" );
      form.ReviewedByUserId = EvSqlMethods.getString ( Row, "TC_ReviewedByUserId" );
      form.ReviewedBy = EvSqlMethods.getString ( Row, "TC_ReviewedBy" );
      form.ReviewedDate = EvSqlMethods.getDateTime ( Row, "TC_ReviewedDate" );
      form.ApprovedByUserId = EvSqlMethods.getString ( Row, "TC_ApprovedByUserId" );
      form.ApprovedBy = EvSqlMethods.getString ( Row, "TC_ApprovedBy" );
      form.ApprovalDate = EvSqlMethods.getDateTime ( Row, "TC_ApprovalDate" );
      form.Design.Version = EvSqlMethods.getFloat ( Row, "TC_Version" );
      form.State = Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue<EvFormObjectStates> (
        EvSqlMethods.getString ( Row, "TC_State" ) );
      form.UpdatedByUserId = EvSqlMethods.getString ( Row, "TC_UpdatedByUserId" );
      form.Updated = EvSqlMethods.getString ( Row, "TC_UpdatedBy" );
      form.Updated += " on " + EvSqlMethods.getDateTime ( Row, "TC_UpdateDate" ).ToString ( "dd MMM yyyy HH:mm" );
      form.cDashMetadata = EvSqlMethods.getString ( Row, "TC_CDASH_METADATA" );

      // 
      // Update the form state to current enumeration.
      // 
      if ( ( int ) form.State < 0 )
      {
        form.State = ( EvFormObjectStates ) Math.Abs ( ( int ) form.State );
      }

      if ( ( int ) form.Design.TypeId < 0 )
      {
        form.Design.TypeId = ( EvFormRecordTypes ) Math.Abs ( ( int ) form.Design.TypeId );
      }

      if ( form.Design.TypeId == EvFormRecordTypes.Assessment_Record )
      {
        form.Design.TypeId = EvFormRecordTypes.Trial_Record;
      }

      if ( form.ApprovedBy != string.Empty )
      {
        form.Design.Approval = "Version: " + form.Version;
        form.Design.Approval += " approved by: " + form.ApprovedBy;
        form.Design.Approval += " on " + form.ApprovalDate.ToString ( "dd MMM yyyy" );
      }


      return form;

    }//END getRowData method.

    #endregion

    #region Form record Queries

    // =====================================================================================
    /// <summary>
    /// This class returns a list of form object items based on VisitId, RecordTypeId, state and orderby value
    /// </summary>
    /// <param name="TrialId">string: (Mandatory) a trial identifier for the query.</param>
    /// <param name="TypeId">EvFormRecordTypes: a form record type identifier.</param>
    /// <param name="State">EvForm.FormObjecStates: a form object state identifier.</param>
    /// <param name="WithFields">Boolean: true = include fields</param>
    /// <returns>List of EvForm: a list of form data object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Return an empty forms list if the trial identifier is empty 
    /// 
    /// 2. Define the sql query parameters and sql query string.
    /// 
    /// 3. Execute the sql query string and store the results on the data table 
    /// 
    /// 4. Loop through the table and extract the data row to the form object. 
    /// 
    /// 5. Add the form object value to the Forms list. 
    /// 
    /// 6. Return the Forms list. 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public List<EvForm> GetFormList ( 
      string TrialId,
      EvFormRecordTypes TypeId,
      EvFormObjectStates State,
      bool WithFields )
    {
      //
      // Initialize the method status and a return list of form object 
      //
      this.LogMethod ( "GetFormList method." );
      this.LogDebug ( "TrialId: '{0}'", TrialId );
      this.LogDebug ( "TypeId: '{0}'", TypeId );
      this.LogDebug ( "State: '{0}'", State );

      List<EvForm> view = new List<EvForm> ( );

      //
      // Validate whether the trial identifier is not empty 
      // 
      if ( TrialId == String.Empty )
      {
        this.LogValue ( "TrialId is null" );
        return view;
      }

      //
      // Initalise the parameters for the query
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( PARM_CUSTOMER_GUID, SqlDbType.UniqueIdentifier),
        new SqlParameter( PARM_TrialId, SqlDbType.NVarChar, 10),
        new SqlParameter( PARM_TypeId, SqlDbType.VarChar, 30),
        new SqlParameter( PARM_State, SqlDbType.VarChar, 20)
      };
      cmdParms [ 0 ].Value = this.ClassParameters.CustomerGuid;
      cmdParms [ 1 ].Value = TrialId;
      cmdParms [ 2 ].Value = TypeId.ToString ( );
      cmdParms [ 3 ].Value = State.ToString ( );

      //
      // Build the query string
      // 
      _sqlQueryString = _sqlQuery_View
        + "WHERE ( (" + EvForms.DB_CUSTOMER_GUID + " = " + EvForms.PARM_CUSTOMER_GUID + " )\r\n"
        + " AND (TrialId = @TrialId ) ";

      if ( TypeId != EvFormRecordTypes.Null )
      {
        _sqlQueryString += "AND ( TC_TypeId = @TypeId ) ";
      }

      if ( State != EvFormObjectStates.Null )
      {
        _sqlQueryString += "AND ( TC_State = @State ) ";
      }
      else
      {
        _sqlQueryString += "AND NOT( TC_State = '" + EvFormObjectStates.Withdrawn + "' ) ";

      }

      _sqlQueryString += ") ORDER BY FormId,TC_Version";

      this.LogValue ( _sqlQueryString );

      //
      //Execute the query against the database
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

          EvForm form = this.getRowData ( row );

          if ( WithFields == true )
          {
            // 
            // Retrieve the instrument items.
            // 
            form.Fields = this._Dal_FormFields.GetView ( form.Guid );
            this.LogClass ( this._Dal_FormFields.Log );
          }

          view.Add ( form );
        }//END record iteration loop

      }//END using method
      this.LogValue ( " Count: " + view.Count.ToString ( ) );

      //
      // return the Array object.
      // 
      this.LogMethodEnd ( "GetFormList" );
      return view;

    }//END GetView method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of option object based on the VisitId, RecordTypeId, state and selectByGuid condition
    /// </summary>
    /// <param name="TrialId">string: (Mandatory) a trial identifier.</param>
    /// <param name="TypeId">EvFormRecordTypes: a form record type identifier</param>
    /// <param name="State">EvForm.FormObjecStates: a form object state identifier</param>
    /// <param name="SelectByGuid">Boolean: true, if the list is selected by Guid</param>
    /// <returns>List of EvOption: a list of option object items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define the sql query parameters and sql query string.
    /// 
    /// 2. Execute the sql query string and store the results on the data table 
    /// 
    /// 3. Loop through the table and extract the data row to the form object. 
    /// 
    /// 4. Add the form object value to the Options list. 
    /// 
    /// 5. Return the Options list. 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public List<EvOption> GetList ( 
      string TrialId, 
      EvFormRecordTypes TypeId, 
      EvFormObjectStates State, bool SelectByGuid )
    {
      //
      // Initialize the method status, a return option list and an option object
      //
      this.LogMethod ( "GetList." );
      this.LogValue ( "TrialId: " + TrialId );
      this.LogValue ( "TypeId: " + TypeId );
      this.LogValue ( "State: " + State );
      this.LogValue ( "SelectByUid: " + SelectByGuid );

      List<EvOption> list = new List<EvOption> ( );
      EvOption option = new EvOption ( );
      list.Add ( option );

      //
      // Initialise the query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter( PARM_CUSTOMER_GUID, SqlDbType.UniqueIdentifier),
        new SqlParameter( PARM_TrialId, SqlDbType.NVarChar, 10),
        new SqlParameter( PARM_TypeId, SqlDbType.VarChar, 30),
        new SqlParameter( PARM_State, SqlDbType.VarChar, 20)
      };
      cmdParms [ 0 ].Value = this.ClassParameters.CustomerGuid;
      cmdParms [ 1 ].Value = TrialId;
      cmdParms [ 2 ].Value = TypeId.ToString ( );
      cmdParms [ 3 ].Value = State.ToString ( );

      //
      // Build the query string
      // 
      _sqlQueryString = _sqlQuery_View
        + " WHERE ( (" + EvForms.DB_CUSTOMER_GUID + " = " + EvForms.PARM_CUSTOMER_GUID + " )\r\n"
        + " AND ( TrialId = @TrialId ) ";

      if ( TypeId != EvFormRecordTypes.Null )
      {
        _sqlQueryString += "AND (TC_TypeId = @TypeId ) ";
      }
      if ( State != EvFormObjectStates.Null )
      {
        _sqlQueryString += "AND (TC_State = @State ) ";
      }
      _sqlQueryString += ") ORDER BY FormId";

      this.LogValue ( _sqlQueryString );

      //
      //Execute the query against the database
      //
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
          // If SelectByGuid = True then optionId is to be the objects Checklist UID
          //
          if ( SelectByGuid == true )
          {
            option = new EvOption (
              EvSqlMethods.getString ( row, "TC_Guid" ),
              EvSqlMethods.getString ( row, "FormId" ) + " - " + EvSqlMethods.getString ( row, "TC_Title" ) );
          }
          //
          // If SelectByGuid = False then optionId is to be the objects FieldId.
          //
          else
          {
            option = new EvOption (
              EvSqlMethods.getString ( row, "FormId" ),
              EvSqlMethods.getString ( row, "FormId" ) + " - " + EvSqlMethods.getString ( row, "TC_Title" ) );
          }
          list.Add ( option );

        }//END iteration loop

      }//END Using method

      //
      // Return the ArrayList.
      //
      return list;

    }//END GetList class

    // =====================================================================================
    /// <summary>
    /// This class returns a list of option object based on form object
    /// </summary>
    /// <param name="Form">EvForm: (Mandatory) The EvForm object being updated.</param>
    /// <returns>List of EvOption: a list of option object items</returns>
    /// <remarks>
    /// This method consists of the followings steps: 
    /// 
    /// 1. Define the sql query parameters and sql query string.
    /// 
    /// 2. Execute the sql query string and store the results on the data table 
    /// 
    /// 3. Loop through the table and extract the data row to the form object. 
    /// 
    /// 4. Add the form object value to the Options list. 
    /// 
    /// 5. Return the Options list. 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    private List<EvOption> GetValidationList ( EvForm Form )
    {
      //
      // Initialize the method status and the return option list
      //
      this.LogMethod ( "GetValidationList, method. " );
      this.LogValue ( "ProjectId: " + Form.TrialId );
      this.LogValue ( "TypeId: " + Form.Design.TypeId );

      List<EvOption> list = new List<EvOption> ( );

      //
      // Initialise the query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter( PARM_CUSTOMER_GUID, SqlDbType.UniqueIdentifier),
        new SqlParameter( PARM_TrialId, SqlDbType.NVarChar, 10),
        new SqlParameter( PARM_TypeId, SqlDbType.VarChar, 30),
      };
      cmdParms [ 0 ].Value = this.ClassParameters.CustomerGuid;
      cmdParms [ 1 ].Value = Form.TrialId;
      cmdParms [ 2 ].Value = Form.Design.TypeId;

      //
      // Build the query string
      // 
      _sqlQueryString = _sqlQuery_View
        + " WHERE ( (" + EvForms.DB_CUSTOMER_GUID + " = " + EvForms.PARM_CUSTOMER_GUID + " )\r\n"
        + " AND (TrialId = @TrialId ) "
        + " AND (TC_TypeId = @TypeId ) "
        + " AND ((TC_State = '" + EvFormObjectStates.Form_Draft + "' ) "
        + "  OR (TC_State = '" + EvFormObjectStates.Form_Reviewed + "' )) "
        + ") ORDER BY FormId";

      this.LogValue ( _sqlQueryString );

      //
      // Execute the query against the database
      //
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

          EvOption option = new EvOption (
              EvSqlMethods.getString ( row, "FormId" ),
              EvSqlMethods.getString ( row, "FormId" ) + " - " + EvSqlMethods.getString ( row, "TC_Title" ) );

          this.LogValue ( " " + option.Value + " " + option.Description );

          list.Add ( option );

        }//END iteration loop

      }//END Using method

      this.LogValue ( " END GetValidationList method count: " + list.Count );

      //
      // Return the ArrayList.
      //
      return list;

    }//END GetValidationList class

    #endregion

    #region Retrieval Queries


    // =====================================================================================
    /// <summary>
    /// This class retrieves a form data object using FormGuid
    /// </summary>
    /// <param name="FormGuid">Guid: a form global unique identifier</param>
    /// <returns>EvForm: a form object</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Return an empty form object if the FormGuid is empty.
    /// 
    /// 3. Define the sql query parameters and sql query string. 
    /// 
    /// 4. Execute the sql query string and store the results on data table. 
    /// 
    /// 5. Extract the first data row to the return form object.
    /// 
    /// 6. Add the formfield items to the form object. 
    /// 
    /// 7. Return the form data object.
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public EvForm getForm ( Guid FormGuid )
    {
      //
      // Initialize the method status, a return form object and a form field object 
      //
      this.LogMethod ( "getItemByGuid merhod. " );
      this.LogValue ( "Form Guid: " + FormGuid );

      EvForm Form = new EvForm ( );
      //EvFormsSections FormSection = new EvFormsSections ( );
      Form.FormId = String.Empty;

      //
      // Validate whether the formGuid is not empty.
      //
      if ( FormGuid == Guid.Empty )
      {
        return Form;
      }

      // 
      // Generate the Selection query string.
      // 
      this._sqlQueryString = _sqlQuery_View + " WHERE TC_Guid = @Guid;";


      this.LogValue ( _sqlQueryString );
      //
      // Initialise the query parameters.
      //

      SqlParameter cmdParms = new SqlParameter ( PARM_Guid, SqlDbType.UniqueIdentifier );
      cmdParms.Value = FormGuid;

      //
      //Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( _sqlQueryString, cmdParms ) )
      {
        // 
        // If not rows the return
        // 
        if ( table.Rows.Count == 0 )
        {
          return Form;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        // 
        // Fill the role object.
        // 
        Form = this.getRowData ( row );

      }//END Using method

      // 
      // Retrieve the instrument items.
      // 
      Form.Fields = this._Dal_FormFields.GetView ( Form.Guid );
      this.LogClass ( this._Dal_FormFields.Log );


      //Form.Design.FormSections = FormSection.GetView ( Form.Guid );
      //  this.LogValue ( FormItems.DebugLog );

      //
      // Return Checklsit data object.
      //
      return Form;

    }//END getForm method

    // ====================================================================================
    /// <summary>
    /// This class returns a form data object using TrialId, FormId and Issued condition
    /// </summary>
    /// <param name="TrialId">string: a trial identifier</param>
    /// <param name="FormId">string: a form identifier</param>
    /// <param name="Issued">Boolean: true, if the form is issued</param>
    /// <returns>EvForm: a form data object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Return an empty form object if the FormGuid is empty.
    /// 
    /// 3. Define the sql query parameters and sql query string. 
    /// 
    /// 4. Execute the sql query string and store the results on data table. 
    /// 
    /// 5. Extract the first data row to the return form object.
    /// 
    /// 6. Add the formfield items to the form object. 
    /// 
    /// 7. Return the form data object. 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public EvForm getForm ( 
      string TrialId, 
      string FormId, 
      bool Issued )
    {
      this.LogMethod ( "getForm method. " );
      this.LogDebug ( "TrialId: " + TrialId );
      this.LogDebug ( "FormId: " + FormId );
      //
      // Initialize the method status, a return form object and a formfield object
      //
      EvForm Form = new EvForm ( );

      //
      // If the FieldId is null then return empty instrument object.
      //
      if ( FormId == String.Empty )
      {
        return Form;
      }


      //
      // Initialise the query parameters.
      //
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter( PARM_CUSTOMER_GUID, SqlDbType.UniqueIdentifier),
        new SqlParameter(PARM_TrialId, SqlDbType.NVarChar, 10),
        new SqlParameter(PARM_FormId, SqlDbType.Char, 10)
      };
      cmdParms [ 0 ].Value = this.ClassParameters.CustomerGuid;
      cmdParms [ 1 ].Value = TrialId;
      cmdParms [ 2 ].Value = FormId;

      //
      // If the form prefix is G_ it is a globalform 
      //
      string prefix = FormId.Substring ( 0, 2 );
      this.LogDebug ( "prefix: " + prefix );

      if ( prefix == "G_" )
      {
        TrialId = EvcStatics.CONST_GLOBAL_PROJECT;
      }
      this.LogDebug ( "Queried ProjectId: " + TrialId );

      // 
      // Generate the Selection query string.
      // 
      this._sqlQueryString = _sqlQuery_View + " WHERE (TrialId = @TrialId) AND (FormId = @FormId) "
        + " AND TC_SUPERSEDED = 0 "
        + " AND NOT(TC_State = '" + EvFormObjectStates.Withdrawn + "') ";


      if ( Issued == true )
      {
        _sqlQueryString += " AND (TC_State = '" + EvFormObjectStates.Form_Issued + "'); ";
      }
      else
      {
        _sqlQueryString += " AND NOT(TC_State = '" + EvFormObjectStates.Form_Issued + "'); ";
      }
      this.LogValue ( _sqlQueryString );

      //
      //Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( _sqlQueryString, cmdParms ) )
      {
        // 
        // If not rows the return
        // 
        if ( table.Rows.Count == 0 )
        {
          return Form;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        // 
        // Fill the role object.
        // 
        Form = this.getRowData ( row );

      }//END Using method

      // 
      // Retrieve the instrument items.
      // 
      Form.Fields = _Dal_FormFields.GetView ( Form.Guid );

      //
      // Return the Checklist data object.
      //
      return Form;

    }//END getForm method

    #endregion

    #region Form Update queries

    // =====================================================================================
    /// <summary>
    /// This class updates the items on form data table using a retrieved form data object.
    /// </summary>
    /// <param name="Form">EvForm: a retrieved form data object</param>
    /// <returns>EvEventCodes: an event code for updating the form data table</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the Form's identifier is duplicated 
    /// 
    /// 2. Generate new DB Guid, if the Form's Guid is empty.
    /// 
    /// 3. Set the datachange object. 
    /// 
    /// 4. Define the sql query parameter and execute the storeprocedure for updating items. 
    /// 
    /// 5. Add datachange values to the backup data changes
    /// 
    /// 6. Return the event code for updating items.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes UpdateItem ( EvForm Form )
    {
      this.LogMethod ( "updateItem() method." );
      // 
      // Initialise the method status and the old form object
      // 
      EvDataChanges dataChanges = new EvDataChanges ( );

      EvForm oldForm = this.getForm ( Form.Guid );
      this.LogValue ( "old form Form Id." + oldForm.FormId );

      //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      //
      // Get the list of forms for this trial.
      //
      List<EvOption> formList = this.GetValidationList ( Form );

      //
      // Iterate through the forms to check that there are no duplicates.
      //
      foreach ( EvOption form in formList )
      {
        this.LogValue ( "oldForm FormId: " + oldForm.FormId
          + " Form FormId: " + Form.FormId + " FormId: " + form.Value );
        //
        // IF the old form Id does not match the updated Form Id 
        // it has been changed. So the new form id needs to be validated.
        //
        if ( oldForm.FormId.ToLower ( ) != Form.FormId.ToLower ( )
          && form.Value.ToLower ( ) == Form.FormId.ToLower ( ) )
        {
          this.LogValue ( " >> Duplicate form Id." );

          return EvEventCodes.Data_Duplicate_Id_Error;
        }
      }//EMD milestone iteration loop.

      // 
      // Generate the DB row guid
      // 
      if ( Form.Guid == Guid.Empty )
      {
        Form.Guid = Guid.NewGuid ( );
      }

      // 
      // Set the data change object values.
      // 
      EvDataChange dataChange = this.setChangeRecord ( Form, oldForm );

      // 
      // Define the query parameters
      // 
      SqlParameter [ ] cmdParms = GetParameters ( );
      SetParameters ( cmdParms, Form );

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( _STORED_PROCEDURE_UpdateItem, cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      //
      // Update the form sections.
      //
      EvEventCodes result = _Dal_FormSections.UpdateItem ( Form );

      this.LogValue ( _Dal_FormSections.Log );

      if ( result != EvEventCodes.Ok )
      {
        return result;
      }

      // 
      // Add the change record
      // 
      dataChanges.AddItem ( dataChange );

      return EvEventCodes.Ok;

    }//END UpdateItem class

    // =====================================================================================
    /// <summary>
    /// This class adds new items to form data table based on retrieved form data objecgt
    /// </summary>
    /// <param name="Form">EvForm: a retrieved form data object</param>
    /// <returns>EvEventCodes: an event code for adding items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the form's Guid is duplicated. 
    /// 
    /// 2. Set new Guid for new form object. 
    /// 
    /// 3. Define the sql query parameters and execute the storeprocedure for adding items. 
    /// 
    /// 4. Add new related formfield items to form table if they exist.
    /// 
    /// 5. Return the event code for adding new items.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes AddItem ( EvForm Form )
    {
      this.LogMethod ( "AddItem method. " );
      this.LogValue ( "ProjectId: " + Form.TrialId );
      this.LogValue ( "FormId: " + Form.FormId );
      this.LogValue ( "Version: " + Form.Design.Version );
      // 
      // Initialise the methods status, a formfield object and a new Guid
      // 

      //
      // Validate whether the Guid does not exist.
      //
      EvForm currentForm = this.getForm ( Form.TrialId, Form.FormId, false );

      if ( currentForm.Guid != Guid.Empty )
      {
        this.LogValue ( "Duplicate form Id. Version: " + Form.Version );

        return EvEventCodes.Data_Duplicate_Id_Error;
      }

      // 
      // Set the Guid for new form
      // 
      Form.Guid = Guid.NewGuid ( );

      // 
      // Define the query parameters
      // 
      SqlParameter [ ] cmdParms = GetParameters ( );
      SetParameters ( cmdParms, Form );

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( _STORED_PROCEDURE_AddItem, cmdParms ) == 0 )
      {
        this.LogValue ( "Errors adding the form object to the database." );
        this.LogValue ( EvSqlMethods.Log );

        return EvEventCodes.Database_Record_Update_Error;
      }

      //
      // Update the form sections.
      //
      this.LogValue ( "Saving sections." );

      EvEventCodes result = _Dal_FormSections.UpdateItem ( Form );

      this.LogValue ( _Dal_FormSections.Log );

      if ( result != EvEventCodes.Ok )
      {
        return result;
      }

      // 
      // If form fields of the new common form exists, add the new form fields to database. 
      // 
      if ( Form.Guid != Guid.Empty
        && Form.Fields.Count > 0 )
      {
        for ( int count = 0; count < Form.Fields.Count; count++ )
        {
          if ( Form.Fields [ count ].Design.Title != String.Empty )
          {
            // 
            // Initialise the newField to add to the database.
            // 
            Form.Fields [ count ].FormGuid = Form.Guid;
            Form.Fields [ count ].UpdatedByUserId = Form.UpdatedByUserId;
            Form.Fields [ count ].UserCommonName = Form.UserCommonName;

            // 
            // Add the newField to the database.
            // 
            _Dal_FormFields.AddItem ( Form.Fields [ count ] );
            this.LogClass ( _Dal_FormFields.Log );
          }
        }
      }//END if form fields. 

      // 
      // Return the object FormUid.
      // 
      return EvEventCodes.Ok;

    }//END AddItem method

    // =====================================================================================
    /// <summary>
    /// This class withdraws the currently issued version of the form object.
    /// </summary>
    /// <param name="Form">EvForm: a form data object</param>
    /// <returns>EvEventCodes: an event code for withdrawing issued form</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Validate whether the formId and user common name are not empty
    /// 
    /// 2. Define the sql query parameters and execute the storeprocedure for withdrawing issued form
    /// 
    /// 3. Return an event code for withdrawing issued form
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes WithdrawIssuedForm ( EvForm Form )
    {
      // 
      // Validate whether the formId and user common name are not empty
      // 
      if ( Form.FormId == String.Empty )
      {
        return EvEventCodes.Identifier_Project_Id_Error;
      }

      if ( Form.UserCommonName == String.Empty )
      {
        return EvEventCodes.Identifier_User_Id_Error;
      }

      // 
      // Define the query parameters
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter(PARM_TrialId, SqlDbType.NVarChar, 10),
        new SqlParameter(PARM_FormId, SqlDbType.NVarChar, 10),
        new SqlParameter(PARM_UpdatedByUserId, SqlDbType.NVarChar,100),
        new SqlParameter(PARM_UpdatedBy, SqlDbType.NVarChar, 100),
        new SqlParameter(PARM_UpdateDate, SqlDbType.DateTime)
      };
      cmdParms [ 0 ].Value = Form.TrialId;
      cmdParms [ 1 ].Value = Form.FormId;
      cmdParms [ 2 ].Value = Form.UpdatedByUserId;
      cmdParms [ 3 ].Value = Form.UserCommonName;
      cmdParms [ 4 ].Value = DateTime.Now;

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( _STORED_PROCEDURE_WithdrawItem, cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      // 
      // Return the object FormUid.
      // 
      return EvEventCodes.Ok;

    }// End deleteYear method.

    // =====================================================================================
    /// <summary>
    /// This class deletes the items from form data table
    /// </summary>
    /// <param name="Form">EvForm: a form data object</param>
    /// <returns>EvEventCodes: an event code for deleting items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the form's Guid is duplicated or User Common Name is empty. 
    /// 
    /// 2. Define the sql query parameters and execute the storeprocedure for deleting items. 
    /// 
    /// 3. Return an event code for deleting items.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes DeleteItem ( EvForm Form )
    {
      //
      // Initialize the method status
      //
      this.LogMethod ( "DeleteItem method. FormId; " + Form.FormId );

      // 
      // Validate whether the form Guid and user common name are not empty.
      // 
      if ( Form.Guid == Guid.Empty )
      {
        return EvEventCodes.Identifier_Global_Unique_Identifier_Error;
      }
      if ( Form.UserCommonName == String.Empty )
      {
        return EvEventCodes.Identifier_User_Id_Error;
      }

      // 
      // Define the query parameters
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter(PARM_Guid, SqlDbType.UniqueIdentifier ),
        new SqlParameter(PARM_UpdatedByUserId, SqlDbType.NVarChar,100),
        new SqlParameter(PARM_UpdatedBy, SqlDbType.NVarChar, 100),
        new SqlParameter(PARM_UpdateDate, SqlDbType.DateTime)
      };
      cmdParms [ 0 ].Value = Form.Guid;
      cmdParms [ 1 ].Value = Form.UpdatedByUserId;
      cmdParms [ 2 ].Value = Form.UserCommonName;
      cmdParms [ 3 ].Value = DateTime.Now;

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( _STORED_PROCEDURE_DeleteItem, cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      // 
      // Return the object FormUid.
      // 
      return EvEventCodes.Ok;

    }//End DeleteItem method.

    #endregion

    #region Set Difference methods

    // =====================================================================================
    /// <summary>
    /// This class sets the data change object for the update action.
    /// </summary>
    /// <param name="RecordForm">EvForm: a form object</param>
    /// <param name="OldRecordForm">EvForm: an old form object</param>
    /// <returns>EvDataChange: a data change object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Add new items to datachange object if they do not exist. 
    /// 
    /// 2. Return the datachange data object.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private EvDataChange setChangeRecord ( EvForm RecordForm, EvForm OldRecordForm )
    {
      //
      // Initialize the method status and the datachange object. 
      //
      this.LogMethod ( "setChangeRecord method. " );

      EvDataChange dataChange = new EvDataChange ( );
      dataChange.Guid = Guid.NewGuid ( );
      dataChange.TableName = EvDataChange.DataChangeTableNames.EvForms;
      dataChange.TrialId = RecordForm.TrialId;
      dataChange.RecordGuid = RecordForm.Guid;
      dataChange.UserId = RecordForm.UpdatedByUserId;
      dataChange.DateStamp = DateTime.Now;

      //
      // Add new items to datachange object if they do not exist. 
      //
      dataChange.AddItem ( "FormId", OldRecordForm.FormId, RecordForm.FormId );

      dataChange.AddItem ( "Title", OldRecordForm.Design.Title, RecordForm.Design.Title );

      dataChange.AddItem ( "Reference", OldRecordForm.Design.Reference, RecordForm.Design.Reference );

      dataChange.AddItem ( "Instructions", OldRecordForm.Design.Instructions, RecordForm.Design.Instructions );

      dataChange.AddItem ( "TypeId", OldRecordForm.Design.TypeId, RecordForm.Design.TypeId );

      dataChange.AddItem ( "Authors", OldRecordForm.Authors, RecordForm.Authors );

      dataChange.AddItem ( "ReviewedByUserId", OldRecordForm.ReviewedByUserId, RecordForm.ReviewedByUserId );

      dataChange.AddItem ( "ReviewedBy", OldRecordForm.ReviewedBy, RecordForm.ReviewedBy );

      dataChange.AddItem ( "ReviewedDate", OldRecordForm.ReviewedDate, RecordForm.ReviewedDate );

      dataChange.AddItem ( "ApprovedByUserId", OldRecordForm.ApprovedByUserId, RecordForm.ApprovedByUserId );

      dataChange.AddItem ( "ApprovedBy", OldRecordForm.ApprovedBy, RecordForm.ApprovedBy );

      dataChange.AddItem ( "ApprovalDate", OldRecordForm.ApprovalDate, RecordForm.ApprovalDate );

      dataChange.AddItem ( "Version", OldRecordForm.Design.Version, RecordForm.Design.Version );

      dataChange.AddItem ( "State", OldRecordForm.State, RecordForm.State );

      //
      // Process the form design object.
      //
      if ( OldRecordForm.Design != null
        && RecordForm.Design != null )
      {
        dataChange.AddItem ( "Design_Approval",
          OldRecordForm.Design.Approval,
          RecordForm.Design.Approval );

        dataChange.AddItem ( "Design_FinishDateInstructions",
          OldRecordForm.Design.FinishDateInstructions,
          RecordForm.Design.FinishDateInstructions );

        dataChange.AddItem ( "Design_FormCategory",
          OldRecordForm.Design.FormCategory,
          RecordForm.Design.FormCategory );

        for ( int count = 0; count < RecordForm.Design.FormSections.Count; count++ )
        {
          EvFormSection oldSection = new EvFormSection ( );
          EvFormSection newSection = RecordForm.Design.FormSections [ count ];

          if ( count < OldRecordForm.Design.FormSections.Count )
          {
            oldSection = OldRecordForm.Design.FormSections [ count ];
          }
          dataChange.AddItem ( "Section_" + count + "_No",
            oldSection.No,
            newSection.No );

          dataChange.AddItem ( "Section_" + count + "_Section",
            oldSection.Title,
            newSection.Title );

          dataChange.AddItem ( "Section_" + count + "_Instructions",
            oldSection.Instructions,
            newSection.Instructions );

          dataChange.AddItem ( "Section_" + count + "_FieldName",
            oldSection.FieldId,
            newSection.FieldId );

          dataChange.AddItem ( "Section_" + count + "_FieldValue",
            oldSection.FieldValue,
            newSection.FieldValue );

          dataChange.AddItem ( "Section_" + count + "_DefaultDisplayRoles",
            oldSection.UserDisplayRoles,
            newSection.UserDisplayRoles );

          dataChange.AddItem ( "Section_" + count + "_DefaultEditRoles",
            oldSection.UserEditRoles,
            newSection.UserEditRoles );

          dataChange.AddItem ( "Section_" + count + "_Order",
            oldSection.Order,
            newSection.Order );

          dataChange.AddItem ( "Section_" + count + "_Visible",
            oldSection.OnOpenVisible,
            newSection.OnOpenVisible );

        }//END section iteration loop

        dataChange.AddItem ( "Design_hasCsScript",
          OldRecordForm.Design.hasCsScript,
          RecordForm.Design.hasCsScript );

        dataChange.AddItem ( "Design_hasSectionNavigation",
          OldRecordForm.Design.hasSectionNavigation,
          RecordForm.Design.hasSectionNavigation );

        dataChange.AddItem ( "Design_HiddenFields",
          OldRecordForm.Design.HiddenFields,
          RecordForm.Design.HiddenFields );

        dataChange.AddItem ( "Design_HiddenFields",
          OldRecordForm.Design.JavaValidationScript,
          RecordForm.Design.JavaValidationScript );

        dataChange.AddItem ( "Design_RecordSubjectInstructions",
          OldRecordForm.Design.RecordSubjectInstructions,
          RecordForm.Design.RecordSubjectInstructions );

        dataChange.AddItem ( "Design_Reference",
          OldRecordForm.Design.Reference,
          RecordForm.Design.Reference );

        dataChange.AddItem ( "Design_RegulatoryTemplate",
          OldRecordForm.Design.RegulatoryTemplate,
          RecordForm.Design.RegulatoryTemplate );

        dataChange.AddItem ( "Design_StartDateInstructions",
          OldRecordForm.Design.StartDateInstructions,
          RecordForm.Design.StartDateInstructions );

        dataChange.AddItem ( "Design_SubjectExternalIdFormat",
          OldRecordForm.Design.SubjectExternalIdFormat,
          RecordForm.Design.SubjectExternalIdFormat );

        dataChange.AddItem ( "Design_SubjectExternalIdInstructions",
          OldRecordForm.Design.SubjectExternalIdInstructions,
          RecordForm.Design.SubjectExternalIdInstructions );

        dataChange.AddItem ( "Design_SubjectRandomisedIdFormat",
          OldRecordForm.Design.SubjectRandomisedIdFormat,
          RecordForm.Design.SubjectRandomisedIdFormat );

        dataChange.AddItem ( "Design_SubjectRandomisedIdInstructions",
          OldRecordForm.Design.SubjectRandomisedIdInstructions,
          RecordForm.Design.SubjectRandomisedIdInstructions );

        dataChange.AddItem ( "Design_SubjectScreeningIdFormat",
          OldRecordForm.Design.SubjectScreeningIdFormat,
          RecordForm.Design.SubjectScreeningIdFormat );

        dataChange.AddItem ( "Design_SubjectExternalIdFormat",
          OldRecordForm.Design.SubjectScreeningIdInstructions,
          RecordForm.Design.SubjectScreeningIdInstructions );

        dataChange.AddItem ( "Design_SubjectSponsorIdFormat",
          OldRecordForm.Design.SubjectSponsorIdFormat,
          RecordForm.Design.SubjectSponsorIdFormat );

        dataChange.AddItem ( "Design_SubjectSponsorIdInstructions",
          OldRecordForm.Design.SubjectSponsorIdInstructions,
          RecordForm.Design.SubjectSponsorIdInstructions );

        dataChange.AddItem ( "Design_TemplateName",
          OldRecordForm.Design.TemplateName,
          RecordForm.Design.TemplateName );

      }//END Form Design object
      // 
      // Return the data change object.
      // 
      return dataChange;

    }//END setChangeRecord method

    #endregion

    #region Copy form methods

    // =====================================================================================
    /// <summary>
    /// This class copies items to form table. 
    /// </summary>
    /// <param name="Form">EvForm: a form data object</param>
    /// <param name="Copy">Boolean: true, if the form is copied</param>
    /// <returns>EvEventCodes: an event code for copying form object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the form's Guid is duplicated or user common name is empty. 
    /// 
    /// 3. Update the version if revising the form.
    /// 
    /// 4. Define the sql query parameters and execute the storeprocedure for copying form
    /// 
    /// 5. Return the event code for copying items.
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvEventCodes CopyForm ( EvForm Form, bool Copy )
    {
      //
      // Initialize the method status, a number of affected records and a numeric version
      //
      this.LogMethod ( "CopyForm method. FormId; " + Form.FormId );

      int databaseRecordAffected = 0;

      // 
      // Validate whether the form Guid and user common name are not empty
      // 
      if ( Form.Guid == Guid.Empty )
      {
        return EvEventCodes.Identifier_Global_Unique_Identifier_Error;
      }
      if ( Form.UserCommonName == String.Empty )
      {
        return EvEventCodes.Identifier_User_Id_Error;
      }

      // 
      // Update the version if revising the form.
      // 
      if ( Copy == false )
      {
        //---------------------- Check for duplicate Checklist identifiers. ------------------
        EvForm currentForm = this.getForm ( Form.TrialId, Form.FormId, false );

        if ( currentForm.Guid != Guid.Empty )
        {
          this.LogValue ( "Duplicate form Id. Version: " + Form.Version );

          return EvEventCodes.Data_Duplicate_Id_Error;
        }

        Form.Design.Version += 0.01F;
      }
      else
      {
        Form.Design.Version = 0.00F;
      }

      // 
      // Define the query parameters
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter(PARM_Guid, SqlDbType.UniqueIdentifier ),
        new SqlParameter(PARM_Copy, SqlDbType.Bit),
        new SqlParameter(PARM_Version, SqlDbType.VarChar,10),
        new SqlParameter(PARM_UpdatedByUserId, SqlDbType.NVarChar,100),
        new SqlParameter(PARM_UpdatedBy, SqlDbType.NVarChar, 100),
        new SqlParameter(PARM_UpdateDate, SqlDbType.DateTime), 
      };
      cmdParms [ 0 ].Value = Form.Guid;
      cmdParms [ 1 ].Value = Copy;
      cmdParms [ 2 ].Value = Form.Design.Version;
      cmdParms [ 3 ].Value = Form.UpdatedByUserId;
      cmdParms [ 4 ].Value = Form.UserCommonName;
      cmdParms [ 5 ].Value = DateTime.Now;

      //
      // Execute the update command.
      //
      databaseRecordAffected = EvSqlMethods.StoreProcUpdate ( _STORED_PROCEDURE_CopyItem, cmdParms );
      if ( databaseRecordAffected == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      this.LogValue ( " Records affected: " + databaseRecordAffected );
      return EvEventCodes.Ok;

    }//END CopyForm method.
    #endregion

  }//END EvForms class

}//END namespace Evado.Dal.Clinical
