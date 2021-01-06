/* <copyright file="DAL\EvRecords.cs" company="EVADO HOLDING PTY. LTD.">
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
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;
using System.Text;

using Evado.Model;
using Evado.Model.Digital;


namespace Evado.Dal.Clinical
{

  /// <summary>
  /// A business Component used to manage Ethics roles
  /// The Evado.Evado.EvRecord is used in most methods 
  /// and is used to store serializable information about an account
  /// </summary>
  public class EdRecords : EvDalBase
  {
    #region class initialisation method.
    /// <summary>
    /// This method initialises the schedule DAL class.
    /// </summary>
    public EdRecords ( )
    {
      this.ClassNameSpace = "Evado.Dal.Clinical.EvFormRecords.";
    }

    /// <summary>
    /// This method initialises the schedule DAL class.
    /// </summary>
    public EdRecords ( EvClassParameters Settings )
    {
      this.ClassParameters = Settings;
      this.ClassNameSpace = "Evado.Dal.Clinical.EvFormRecords.";

      this._Dal_FormRecordFields = new EdRecordValues ( Settings );
      this._Dal_FormSections = new EdRecordSections ( this.ClassParameters );
    }

    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class constants and variables.

    //  ==================================================================================
    // 
    // Define the selectionList query string.
    // 
    // The max visitSchedule length setting. 
    private static string _maximumListLength = ConfigurationManager.AppSettings [ "MaximumSelectionListLength" ];
    private int _MaxViewLength = 1000;

    private const string _sqlQuery_View = "Select * FROM EvRecord_View ";

    // 
    // selectionList query string.
    // 
    private const string SQL_RECORD_STATUS_REPORT_QUERY = "Select * FROM EvRpt_SCHEDULE_FORM_LIST ";
    /// <summary>
    /// This constant defines the record GUID
    /// </summary>
    public const string DB_RECORDS_GUID = "TR_GUID";
    /// <summary>
    /// This constant defines the form template GUID
    /// </summary>
    public const string DB_FORMS_GUID = "TC_GUID";

    // 
    // Define the Subject Ethics query string.
    // 
    private const string _sqlQueryItemView = "Select * FROM EvRecordField_Query ";

    // 
    // Define the stored procedure names.
    // 
    private const string STORED_PROCEDURE_CreateItem = "usr_Record_create";
    private const string STORED_PROCEDURE_UpdateItem = "usr_Record_update";
    private const string STORED_PROCEDURE_DeleteItem = "usr_Record_delete";
    private const string STORED_PROCEDURE_LockItem = "usr_Record_lock";
    private const string STORED_PROCEDURE_UnlockItem = "usr_Record_unlock";
    private const string STORED_PROCEDURE_COPY_RECORD = "usr_Record_copy";

    // 
    // Define the query parameter constants.
    // 

    //
    // The field and parameter values for the SQl customer filter 
    //
    private const string DB_CUSTOMER_GUID = "CU_GUID";
    private const string PARM_CUSTOMER_GUID = "@CUSTOMER_GUID";

    private const string PARM_Guid = "@Guid";
    private const string PARM_FormGuid = "@FormGuid";
    private const string PARM_TypeId = "@TypeId";
    private const string PARM_VisitId = "@VisitId";
    private const string PARM_MilestoneId = "@MilestoneId";
    private const string PARM_ActivityId = "@ActivityId";
    private const string PARM_IsMandatoryActivity = "@IsMandatoryActivity";
    private const string PARM_IsMandatory = "@IsMandatory";
    private const string PARM_RecordId = "@RecordId";
    private const string PARM_SourceId = "@SourceId";
    private const string PARM_FormId = "@FormId";
    private const string PARM_RecordDate = "@RecordDate";
    private const string PARM_TrialId = "@TrialId";
    private const string PARM_FormProjectId = "@FormProjectId";
    private const string PARM_OrgId = "@OrgId";
    private const string PARM_SubjectId = "@SubjectId";
    private const string PARM_PATIENT_ID = "@PATIENT_ID";
    private const string PARM_MonitorSignoff = "@MonitorSignoff";
    private const string PARM_AssessmentState = "@AssessmentState";
    private const string PARM_Annotation = "@Annotation";
    private const string PARM_XmlData = "@XmlData";
    private const string PARM_Summary = "@Summary";
    private const string PARM_AuthoredByUserId = "@AuthoredByUserId";
    private const string PARM_AuthoredBy = "@AuthoredBy";
    private const string PARM_AuthoredDate = "@AuthoredDate";
    private const string PARM_QueriedByUserId = "@QueriedByUserId";
    private const string PARM_QueriedBy = "@QueriedBy";
    private const string PARM_QueriedDate = "@QueriedDate";
    private const string PARM_ReviewedByUserId = "@ReviewedByUserId";
    private const string PARM_ReviewedBy = "@ReviewedBy";
    private const string PARM_ReviewedDate = "@ReviewedDate";
    private const string PARM_ApprovedByUserId = "@ApprovedByUserId";
    private const string PARM_ApprovedBy = "@ApprovedBy";
    private const string PARM_ApprovalDate = "@ApprovalDate";
    private const string PARM_SignoffStatement = "@SignoffStatement";
    private const string PARM_MonitorUserId = "@MonitorUserId";
    private const string PARM_Monitor = "@Monitor";
    private const string PARM_MonitorDate = "@MonitorDate";
    private const string PARM_State = "@State";
    private const string PARM_QueryState = "@QueryState";
    private const string PARM_UpdatedByUserId = "@UpdatedByUserId";
    private const string PARM_UpdatedBy = "@UpdatedBy";
    private const string PARM_UpdateDate = "@UpdateDate";
    private const string PARM_BookedOut = "@BookedOutBy";
    private const string PARM_ScheduleId = "@ScheduleId";
    private const string PARM_SOURCE_ID = "@SOURCE_ID";

    private const string PARM_COUNTRY = "@COUNTRY";
    private const string PARM_SIGN_OFFS = "@SIGN_OFFS";
    private const string PARM_PRIOR_TO_TRIALS = "@PRIOR_TO_TRIALS";

    // 
    // Used for the ItemQuery
    // 
    private const string PARM_FieldId = "@FieldId";
    private const string PARM_TextValue = "@TextValue";
    private const string PARM_UserName = "@UserName";
    private const string PARM_StartDate = "@StartDate";
    private const string PARM_FinishDate = "@FinishDate";
    private const string PARM_ARM_INDEX = "@ARM_INDEX";

    //
    // This variable is used to skip retrieving comments when updating the form record.
    //
    bool _SkipRetrievingComments = false;

    // 
    // Instantiate the variables and properties.
    // 
    private string _sqlQueryString = String.Empty;


    private String _TextComments = String.Empty;
    private String _TextAnnotations = String.Empty;

    EdRecordValues _Dal_FormRecordFields = new EdRecordValues ( );

    EdRecordSections _Dal_FormSections = new EdRecordSections ( );
    #endregion

    #region Set Query Parameters

    //  ==================================================================================
    /// <summary>
    /// This class sets the array of parameters. 
    /// </summary>
    /// <returns>SqlParameter: an array of SqlParameters</returns>
    /// <remarks>
    /// This method consists of the following step: 
    /// 
    /// 1. Create the array of sql parameters.
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private static SqlParameter [ ] GetParameters ( )
    {
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( PARM_Guid, SqlDbType.UniqueIdentifier),
        new SqlParameter( PARM_FormGuid, SqlDbType.UniqueIdentifier),
        new SqlParameter( PARM_TrialId, SqlDbType.NVarChar, 10),
        new SqlParameter( PARM_OrgId, SqlDbType.NVarChar, 10),
        new SqlParameter( PARM_SubjectId, SqlDbType.NVarChar, 20),
        new SqlParameter( PARM_PATIENT_ID, SqlDbType.Int),
        new SqlParameter( PARM_VisitId, SqlDbType.NVarChar, 20),
        new SqlParameter( PARM_MilestoneId, SqlDbType.NVarChar, 10),
        new SqlParameter( PARM_ActivityId, SqlDbType.NVarChar, 20),
        new SqlParameter( PARM_IsMandatoryActivity, SqlDbType.Bit),

        new SqlParameter( PARM_IsMandatory, SqlDbType.Bit),
        new SqlParameter( PARM_RecordId, SqlDbType.NVarChar, 20), 
        new SqlParameter( PARM_RecordDate, SqlDbType.DateTime),
        new SqlParameter( PARM_MonitorSignoff, SqlDbType.Bit),
        new SqlParameter( PARM_Summary, SqlDbType.NText),
        new SqlParameter( PARM_Annotation, SqlDbType.NText),
        new SqlParameter( PARM_XmlData, SqlDbType.NText),
        new SqlParameter( PARM_AuthoredByUserId, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_AuthoredBy, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_AuthoredDate, SqlDbType.DateTime), 
       
        new SqlParameter( PARM_QueriedByUserId, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_QueriedBy, SqlDbType.NVarChar, 100), 
        new SqlParameter( PARM_QueriedDate, SqlDbType.DateTime),
        new SqlParameter( PARM_ReviewedByUserId, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_ReviewedBy, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_ReviewedDate, SqlDbType.DateTime),
        new SqlParameter( PARM_ApprovedByUserId, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_ApprovedBy, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_ApprovalDate, SqlDbType.DateTime),
        new SqlParameter( PARM_SignoffStatement,SqlDbType.NText),

        new SqlParameter( PARM_MonitorUserId, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_Monitor, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_MonitorDate, SqlDbType.DateTime),
        new SqlParameter( PARM_State, SqlDbType.VarChar, 20),
        new SqlParameter( PARM_QueryState, SqlDbType.VarChar, 10),
        new SqlParameter( PARM_UpdatedByUserId, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_UpdatedBy, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_UpdateDate, SqlDbType.DateTime),
        new SqlParameter( PARM_COUNTRY, SqlDbType.NVarChar, 250),
        new SqlParameter( PARM_SIGN_OFFS, SqlDbType.NVarChar),

        new SqlParameter( PARM_PRIOR_TO_TRIALS, SqlDbType.Bit),
        new SqlParameter( PARM_SOURCE_ID, SqlDbType.NVarChar, 40),
      };
      return cmdParms;
    }

    // =====================================================================================
    /// <summary>
    /// This class binds values to query parameters.
    /// </summary>
    /// <param name="CommandParameters">SqlParameter: an array of sql parameters</param>
    /// <param name="Record">EvForm: a form object containing the parmeter values.</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. If the monitor name exists then set the signoff to true.
    /// 
    /// 2. Set record date and Guid if they do not exist
    /// 
    /// 3. Update the items from form object to the array of sql query parameters. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private void SetParameters ( 
      SqlParameter [ ] CommandParameters, 
      EdRecord Record )
    {
      // 
      // Initialise the monitorsignoff condition
      // 
      bool bMonitorSignoff = false;

      // 
      // Set the record date if is not already set.
      // 
      if ( Record.RecordDate == Evado.Model.EvStatics.CONST_DATE_NULL )
      {
        Record.RecordDate = DateTime.Now;
      }

      // 
      // Set the Global identifier if not set.
      // 
      if ( Record.Guid == Guid.Empty )
      {
        Record.Guid = Guid.NewGuid ( );
      }

      // 
      // Load the command parmameter values
      // 
      CommandParameters [ 0 ].Value = Record.Guid;
      CommandParameters [ 1 ].Value = Record.LayoutGuid;
      CommandParameters [ 2 ].Value = Record.ApplicationId;
      CommandParameters [ 3 ].Value = String.Empty;
      CommandParameters [ 4 ].Value = String.Empty;
      CommandParameters [ 5 ].Value = String.Empty;
      CommandParameters [ 6 ].Value = String.Empty;
      CommandParameters [ 7 ].Value = Record.MilestoneId;
      CommandParameters [ 8 ].Value = Record.ActivityId;
      CommandParameters [ 9 ].Value = Record.IsMandatoryActivity;

      CommandParameters [ 10 ].Value = Record.IsMandatory;
      CommandParameters [ 11 ].Value = Record.RecordId;
      CommandParameters [ 12 ].Value = Record.RecordDate;
      CommandParameters [ 13 ].Value = bMonitorSignoff;
      CommandParameters [ 14 ].Value = String.Empty;
      CommandParameters [ 15 ].Value = String.Empty;
      CommandParameters [ 16 ].Value = String.Empty;
      CommandParameters [ 17 ].Value = String.Empty;
      CommandParameters [ 18 ].Value = String.Empty;
      CommandParameters [ 19 ].Value = EvStatics.CONST_DATE_NULL;

      CommandParameters [ 20 ].Value = String.Empty;
      CommandParameters [ 21 ].Value = String.Empty;
      CommandParameters [ 22 ].Value = EvStatics.CONST_DATE_NULL;
      CommandParameters [ 23 ].Value = String.Empty;
      CommandParameters [ 24 ].Value = String.Empty;
      CommandParameters [ 25 ].Value = EvStatics.CONST_DATE_NULL;
      CommandParameters [ 26 ].Value = String.Empty;
      CommandParameters [ 27 ].Value = String.Empty;
      CommandParameters [ 28 ].Value = EvStatics.CONST_DATE_NULL;
      CommandParameters [ 29 ].Value = String.Empty;

      CommandParameters [ 30 ].Value = String.Empty;
      CommandParameters [ 31 ].Value = String.Empty;
      CommandParameters [ 32 ].Value = EvStatics.CONST_DATE_NULL;
      CommandParameters [ 33 ].Value = Record.State.ToString ( );
      CommandParameters [ 34 ].Value = String.Empty;
      CommandParameters [ 35 ].Value = this.ClassParameters.UserProfile.UserId;
      CommandParameters [ 36 ].Value = this.ClassParameters.UserProfile.CommonName;
      CommandParameters [ 37 ].Value = DateTime.Now;
      CommandParameters [ 38 ].Value = Record.Design.Language;
      CommandParameters [ 39 ].Value = Evado.Model.EvStatics.SerialiseObject<List<EvUserSignoff>> ( Record.Signoffs );

      CommandParameters [ 40 ].Value = false;
      CommandParameters [ 41 ].Value = Record.SourceId;

    }//END SetParameters class.

    //  ==================================================================================
    /// <summary>
    /// This class sets the array of createPrameters. 
    /// </summary>
    /// <returns>SqlParameter: an array of SqlParameters</returns>
    /// <remarks>
    /// This class consists of the following steps: 
    /// 
    /// 1. Create an array of sql query parameters. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private static SqlParameter [ ] GetCreateParameters ( )
    {
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( PARM_Guid, SqlDbType.UniqueIdentifier),
        new SqlParameter( PARM_TrialId, SqlDbType.NVarChar, 10),
        new SqlParameter( PARM_OrgId, SqlDbType.NVarChar, 10),
          new SqlParameter( PARM_SubjectId, SqlDbType.NVarChar, 20),
          new SqlParameter( PARM_PATIENT_ID, SqlDbType.Int),
        new SqlParameter( PARM_MilestoneId, SqlDbType.NVarChar, 10),
        new SqlParameter( PARM_ActivityId, SqlDbType.NVarChar, 20),
        new SqlParameter( PARM_IsMandatoryActivity, SqlDbType.Bit),
        new SqlParameter( PARM_IsMandatory, SqlDbType.Bit),
        new SqlParameter( PARM_VisitId, SqlDbType.NVarChar, 20),
        new SqlParameter( PARM_FormId, SqlDbType.NVarChar, 10),
        new SqlParameter( PARM_UpdatedByUserId, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_UpdatedBy, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_FormProjectId, SqlDbType.NVarChar, 10),
      };
      return cmdParms;

    }//END GetCreateParameters method

    // =====================================================================================
    /// <summary>
    /// This class binds the values to the array of createParameters
    /// </summary>
    /// <param name="CommandParameters">SqlParameter: an array of sql parameters</param>
    /// <param name="Record">EvForm: a form data object containing the parmeter values.</param>
    /// <remarks>
    /// This method consists of the following step: 
    /// 
    /// 1. Load the form object values to the array of sql parameters. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private void SetCreateParameters (
      SqlParameter [ ] CommandParameters, 
      EdRecord Record )
    {
      //
      // If the form prefix is G_ it is a globalform 
      //
      string formProject = Record.ApplicationId;
      string prefix = Record.LayoutId.Substring ( 0, 2 );
      this.LogDebug ( "prefix: " + prefix );
      if ( prefix == "G_" )
      {
        formProject = EvcStatics.CONST_GLOBAL_PROJECT;
      }
      this.LogDebug ( "formProject: " + formProject );

      // 
      // Load the command parmameter values
      // 
      CommandParameters [ 0 ].Value = Record.Guid;
      CommandParameters [ 1 ].Value = Record.ApplicationId;
      CommandParameters [ 2 ].Value = String.Empty; ;
      CommandParameters [ 3 ].Value = String.Empty; ;
      CommandParameters [ 4 ].Value = String.Empty; ;
      CommandParameters [ 5 ].Value = Record.MilestoneId;
      CommandParameters [ 6 ].Value = Record.ActivityId;
      CommandParameters [ 7 ].Value = Record.IsMandatoryActivity;
      CommandParameters [ 8 ].Value = Record.IsMandatory;
      CommandParameters [ 9 ].Value = String.Empty; ;
      CommandParameters [ 10 ].Value = Record.LayoutId;
      CommandParameters [ 11 ].Value = this.ClassParameters.UserProfile.UserId;
      CommandParameters [ 12 ].Value = this.ClassParameters.UserProfile.CommonName;
      CommandParameters [ 13 ].Value = formProject;

    }//END SetCreateParameters class.

    #endregion

    #region Form Record Reader

    // =====================================================================================
    /// <summary>
    /// This class extracts sqlReader to the form object.
    /// </summary>
    /// <param name="Row">DataRow: a sql data row object containing the query results</param>
    /// <param name="SummaryQuery">Boolean: true, if the queryState query is selected</param>
    /// <returns>EvForm: a form object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Update the trial record object with compatible values from datarow
    /// 
    /// 2. Add the content value if it is not a queryState query. 
    /// 
    /// 3. Reformat Comments into new format if necessary
    /// 
    /// 4. Update the state and type to the form object. 
    /// 
    /// 5. Return the Form data object. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private EdRecord getRowData (
      DataRow Row,
      bool SummaryQuery )
    {
      //this.LogMethod ( "getRowData method" );
      // 
      // Initialise the return form object and the form selected condition.
      // 
      EdRecord record = new EdRecord ( );
      record.Selected = false;

      // 
      // Update the trial record object with compatible values from datarow.
      // 
      record.CustomerGuid = EvSqlMethods.getGuid ( Row, EdRecords.DB_CUSTOMER_GUID );
      record.Guid = EvSqlMethods.getGuid ( Row, EdRecords.DB_RECORDS_GUID );
      record.LayoutGuid = EvSqlMethods.getGuid ( Row, EdRecords.DB_FORMS_GUID );
      record.ApplicationId = EvSqlMethods.getString ( Row, "TrialId" );
      record.MilestoneId = EvSqlMethods.getString ( Row, "MilestoneId" );
      record.ActivityId = EvSqlMethods.getString ( Row, "ActivityId" );
      record.IsMandatoryActivity = EvSqlMethods.getBool ( Row, "MA_IsMandatory" );
      record.IsMandatory = EvSqlMethods.getBool ( Row, "TR_IsMandatory" );
      record.SourceId = EvSqlMethods.getString ( Row, "SOURCE_ID" );
      record.LayoutId = EvSqlMethods.getString ( Row, "FormId" );
      record.RecordId = EvSqlMethods.getString ( Row, "RecordId" );
      record.Design.Title = EvSqlMethods.getString ( Row, "TC_Title" );
      record.State = Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue<EdRecordObjectStates> (
        EvSqlMethods.getString ( Row, "TR_State" ) );
      record.RecordDate = EvSqlMethods.getDateTime ( Row, "TR_RecordDate" );

      //
      // Skip detailed content if a queryState query
      //
      if ( SummaryQuery == false )
      {
        string xmlDesign = EvSqlMethods.getString ( Row, "TC_XmlData" );
        if ( xmlDesign != String.Empty )
        {
          xmlDesign = xmlDesign.Replace ( "EvFormDesign", "EdRecordDesign" );
          xmlDesign = xmlDesign.Replace ( "Trial_Record", "Normal_Record" );
          record.Design = Evado.Model.Digital.EvcStatics.DeserialiseObject<EdRecordDesign> ( xmlDesign );
        }
        else
        {
          record.Design.JavaScript = EvSqlMethods.getString ( Row, "TC_JAVA_VALIDATION_SCRIPT" );
          record.Design.RecordCategory = EvSqlMethods.getString ( Row, "TC_FORM_CATEGORY" );
          record.Design.hasCsScript = EvSqlMethods.getBool ( Row, "TC_HAS_CS_SCRIPT" );
        }
        this._TextComments = EvSqlMethods.getString ( Row, "TR_Summary" );
        this._TextAnnotations = EvSqlMethods.getString ( Row, "TR_Annotation" );


        foreach ( EvFormSection sctn in record.Design.FormSections )
        {
          this.LogDebug ( "Section No: " + sctn.No + ", Section: " + sctn.Section + ", Title: " + sctn.Title );
        }

        List<EvFormSection> sectionList = _Dal_FormSections.getSectionList ( record.LayoutGuid );
        if ( sectionList.Count > 0 )
        {
          //this.LogDebugValue ( "Form section exist and have been read in." );
          record.Design.FormSections = sectionList;
        }
        else
        {
          //this.LogDebugValue ( "No form section exist." );
          this._Dal_FormSections.UpdateItem ( record );

          this.LogClass ( this._Dal_FormSections.Log );
        }

        record.Updated = EvSqlMethods.getString ( Row, "TR_UpdatedBy" );
        if ( record.Updated != string.Empty )
        {
          record.Updated += " on " + EvSqlMethods.getDateTime ( Row, "TR_UpdateDate" ).ToString ( "dd MMM yyyy HH:mm" );
        }
        record.BookedOutBy = EvSqlMethods.getString ( Row, "TR_BookedOutBy" );

        //
        // Draft records do not have comments.
        //
        /*
         * 
        if ( ( record.TypeId == EvFormRecordTypes.Informed_Consent_1
            || record.TypeId == EvFormRecordTypes.Informed_Consent_2
            || record.TypeId == EvFormRecordTypes.Informed_Consent_3
            || record.TypeId == EvFormRecordTypes.Informed_Consent_4 )
          && record.State == EvFormObjectStates.Draft_Record )
         */
        if ( record.State == EdRecordObjectStates.Draft_Record )
        {
          this._SkipRetrievingComments = true;
        }


        if ( this._SkipRetrievingComments == false )
        {
          //
          // fill the comment list.
          //
          this.fillCommentList ( record );

          //
          // Reformat Comments into new format if necessary
          //
          this.updateRecordComments ( record );
        }

      }//END SummaryQuery = false section

      String value = EvSqlMethods.getString ( Row, "TC_TypeId" );

      if ( value == "Questionnaire" )
      {
        value = EdRecordTypes.Questionnaire.ToString ( );
      }

      record.Design.TypeId = Evado.Model.EvStatics.Enumerations.parseEnumValue<EdRecordTypes> ( value );

      record.Design.Approval = "Approved";

      //record.Design.Approval = EvSqlMethods.getString ( Row, "TC_Approved" );

      return record;

    }//END getRowData class

    // =====================================================================================
    /// <summary>
    /// This method fills the comment list to the form object. 
    /// </summary>
    /// <param name="FormRecord">EvFormField: a form object</param>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Fill the record comment list.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private void fillCommentList ( EdRecord FormRecord )
    {
      //this.LogMethod ( "fillCommentList method." );
      //
      // Initialize the method values and objects.
      //
      EdRecordComments formRecordComments = new EdRecordComments ( );

      //
      // Fill the record comment list.
      //
      FormRecord.CommentList = formRecordComments.getCommentList (
        FormRecord.Guid, Guid.Empty,
        EvFormRecordComment.CommentTypeCodes.Form,
        EvFormRecordComment.AuthorTypeCodes.Not_Set );

      //this.LogClass ( formRecordComments.Log );

      //this.LogMethodEnd ( "fillCommentList" );

    }//END fillCommentList method

    // =====================================================================================
    /// <summary>
    /// This method updates the form record comment structure.
    /// </summary>
    /// <param name="FormRecord">EvForm: a form object</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the commentlist has value or if the coments and annotation are empty. 
    /// 
    /// 2. Deserialize the commentlist if it is xml. 
    /// 
    /// 3. Update the comments structure if they exist
    /// 
    /// 4. Update the annotations structure if they exist. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private void updateRecordComments ( EdRecord FormRecord )
    {
      //
      // Initialize the debug log 
      //
      //this.LogMethod ( "updateRecordComments method. " );

      //
      // Exit if the comment list is not empty.
      //
      if ( FormRecord.CommentList.Count > 0 )
      {
        return;
      }

      //
      // Exit if there are no comments or annotations.
      //
      if ( this._TextComments == String.Empty
        && this._TextAnnotations == String.Empty )
      {
        return;
      }

      //
      // Deserialize the comments if they are xml. 
      //
      if ( this._TextComments.Contains ( "<?xml version=" ) == true )
      {
        //this._DebugLog.AppendLine( "XML structure" );
        String stXmlComment = this._TextComments;
        stXmlComment = stXmlComment.Replace ( "EvFormComment", "EvFormRecordComment" );

        FormRecord.CommentList = Evado.Model.Digital.EvcStatics.DeserialiseObject<List<EvFormRecordComment>> ( stXmlComment );

        //
        // reset the comments new status is an xml source
        //
        foreach ( EvFormRecordComment comment in FormRecord.CommentList )
        {
          comment.RecordGuid = FormRecord.Guid;
          comment.RecordFieldGuid = Guid.Empty;
          comment.NewComment = true;
        }

        return;
      }

      //
      // Update the comments structure if they exist
      //
      if ( this._TextComments != string.Empty )
      {
        //this._DebugLog.AppendLine( "\r\nCOMMENTS EXISTS \r\n" );
        //
        // Convert the comment into the comment with a delimiter
        //
        String stComments = this._TextComments.Replace ( "\r\n\r\n", "^" );

        //
        // Convert the comments string into array of comments.
        //
        String [ ] arrComments = stComments.Split ( '^' );

        //
        // Iterate through the array of comments
        //
        for ( int i = ( arrComments.Length - 1 ); i >= 0; i-- )
        {
          this.ParseComments ( FormRecord, arrComments [ i ] );
        }


      }//END record comments exist.

      //
      // Update the annotations structure if they exist. 
      //
      if ( this._TextAnnotations != String.Empty )
      {
        //this._DebugLog.AppendLine( "\r\nANNOTATION EXISTS \r\n" );
        //
        // Convert the comment into the comment with a delimiter
        //
        String stAnnotation = this._TextAnnotations.Replace ( "\r\n\r\n", "^" );

        //
        // Convert the comments string into array of comments.
        //
        String [ ] arrAnnotations = stAnnotation.Split ( '^' );

        //
        // Iterate through the array of comments
        //
        for ( int i = ( arrAnnotations.Length - 1 ); i >= 0; i-- )
        {
          this.ParseComments ( FormRecord, arrAnnotations [ i ] );
        }

      }//END record annotation exists.

    }//END updateRecordComments method

    // =====================================================================================
    /// <summary>
    /// This method updates the form record comment structure.
    /// </summary>
    /// <param name="FormRecord">EvForm: a form object containing the form record</param>
    /// <param name="Comment">String: the comment text</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Convert the comment into an array of lines
    /// 
    /// 2. Iterate through the line to parse the message content.
    /// 
    /// 3. Append new comment to the list if it exists. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private void ParseComments ( EdRecord FormRecord, String Comment )
    {
      //this.LogMethod( "ParseComments method. " );
      //
      // Initialise method variables and objects.
      //
      const string delimiter_NameStart1 = "By ";
      const string delimiter_NameStart2 = "by ";
      const string delimiter_NameStart3 = "By: ";
      const string delimiter_NameStart4 = "by: ";
      const string delimiter_DateStart = " on ";
      int inNameStart = 0;
      int inDateStart = 0;
      int inNameLength = 0;
      string stLine = String.Empty;
      string stName = String.Empty;
      string stDate = String.Empty;
      string stContent = String.Empty;
      string stContent1 = String.Empty;
      string stContent2 = String.Empty;
      DateTime dtValue = Evado.Model.Digital.EvcStatics.CONST_DATE_NULL;
      bool bDateStampFound = false;
      EvFormRecordComment comment = new EvFormRecordComment ( );

      //
      // Convert the comment into an array of lines
      //
      Comment = Comment.Replace ( "\r\n", "\n" );

      String [ ] arrLines = Comment.Split ( '\n' );

      //this._DebugLog.AppendLine( "The comment has " + arrLines.Length + " lines. " );

      //
      // Iterate through the line to parse the message content.
      //
      for ( int inLine = 0; inLine < arrLines.Length; inLine++ )
      {
        //
        // set the date stamp to false, this will assume that the line is content text 
        // not name date stamp.
        //
        bDateStampFound = false;

        //
        // get the line as a string.
        //
        stLine = arrLines [ inLine ];

        this.LogValue ( "Line: " + inLine + " >> " + stLine );

        //
        // Check for first name type
        //
        if ( stLine.Contains ( delimiter_NameStart1 ) == true
          || stLine.Contains ( delimiter_NameStart2 ) == true
          || stLine.Contains ( delimiter_NameStart3 ) == true
          || stLine.Contains ( delimiter_NameStart4 ) == true )
        {
          //
          // Standardise the name start string.
          //
          stLine = stLine.Replace ( delimiter_NameStart2, delimiter_NameStart1 );
          stLine = stLine.Replace ( delimiter_NameStart3, delimiter_NameStart1 );
          stLine = stLine.Replace ( delimiter_NameStart4, delimiter_NameStart1 );

          //
          // find the start of the name
          //
          inNameStart = stLine.IndexOf ( delimiter_NameStart1 );
          inDateStart = stLine.IndexOf ( delimiter_DateStart );

          //
          // the name length will be difference between the date start and name start plus the delimiter.
          //
          inNameLength = inDateStart - inNameStart - delimiter_NameStart1.Length;

          //this._DebugLog.AppendLine( "inNameStart: " + inNameStart
          //  + ", inDateStart: " + inDateStart
          //  + ", inNameLength: " + inNameLength );

          //
          // if the text length is greater than zero process the comment.
          //  Assume that the string is a comment content.
          //
          if ( inNameLength > 0 )
          {
            //
            // check to see if there is content before the name start.
            //
            if ( inNameStart > 0 )
            {
              stContent1 = stLine.Substring ( 0, inNameStart );
            }

            //
            // retrieve the name string.
            //
            stName = stLine.Substring (
              inNameStart + delimiter_NameStart1.Length,
              inNameLength );

            stDate = stLine.Substring ( inDateStart + delimiter_DateStart.Length );

            //
            // validate that a date time object has been found.
            //
            if ( stDate.Length > 0 )
            {
              if ( DateTime.TryParse ( stDate, out dtValue ) == true )
              {
                bDateStampFound = true;
              }
            }// Date string found.

            // this._DebugLog.AppendLine( "Parsing Name line: \r\n Comment1: " + stContent1
            //   + "\r\n Name: " + stName
            //   + "\r\n Date: " + stDate );

          }//END Name text has been found.

          //
          // Process the stLine as a comment content.
          //
          if ( bDateStampFound == false )
          {
            if ( stContent != String.Empty )
            {
              stContent += "\r\n";
            }
            stContent = arrLines [ inLine ];

            //
            // reset the name and date as this is not a signoff line.
            //
            stName = String.Empty;
            stDate = String.Empty;
            dtValue = Evado.Model.Digital.EvcStatics.CONST_DATE_NULL;
          }
          else
          {
            //
            // if there is content prior to the name date stamp include in the content.
            //
            if ( stContent1 != string.Empty )
            {
              if ( stContent != String.Empty )
              {
                stContent += "\r\n";
              }
              stContent = stContent1;
            }
          }//END else - bDateStampFound

        }//END Line delimiter found.
        else
        {
          if ( stContent != String.Empty )
          {
            stContent += "\r\n";
          }
          stContent += stLine;
        }

      }//END line iteration loop

      //
      // Append new comment to the list if it exists. 
      //
      if ( stContent != String.Empty )
      {
        // this._DebugLog.AppendLine( "Comment: \r\n stContent: " + stContent
        //  + "\r\n Name: " + stName
        //  + "\r\n Date: " + stDate );

        comment = new EvFormRecordComment (
          FormRecord.Guid,
          EvFormRecordComment.AuthorTypeCodes.Not_Set,
          stName,
          stName,
          stContent );
        if ( stDate != String.Empty )
        {
          comment.CommentDate = dtValue;
        }
        comment.NewComment = true;

        //
        // Append the comment to the list.
        //
        FormRecord.CommentList.Add ( comment );
      }//END if content add

    }//ENd ParseComments method.


    // =====================================================================================
    /// <summary>
    /// This class extracts datarow values to the form object.
    /// </summary>
    /// <param name="Row">DataRow: The data reader contain the query results</param>
    /// <returns>EVform: a form Object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Extract the compatible data row values to the form object. 
    /// 
    /// 2. Return the form data object. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private EdRecord getItemQueryRowData ( DataRow Row )
    {
      // 
      // Initialise the return form object. 
      // 
      EdRecord record = new EdRecord ( );
      record.Selected = false;

      // 
      // Update the trial record object with query values.
      // 
      record.Guid = EvSqlMethods.getGuid ( Row, "TR_Guid" );
      record.ApplicationId = EvSqlMethods.getString ( Row, "TrialId" );
      record.LayoutId = EvSqlMethods.getString ( Row, "FormId" );
      record.Design.Title = EvSqlMethods.getString ( Row, "TC_Title" );
      record.RecordId = EvSqlMethods.getString ( Row, "RecordId" );
      record.RecordDate = EvSqlMethods.getDateTime ( Row, "TR_RecordDate" );
      this._TextComments = EvSqlMethods.getString ( Row, "TR_Summary" );
      this._TextAnnotations = EvSqlMethods.getString ( Row, "TR_Annotation" );
      record.State = Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue<EdRecordObjectStates> ( EvSqlMethods.getString ( Row, "TR_State" ) );
      record.LinkText = EvSqlMethods.getString ( Row, "TRI_TextValue" );

      //
      // fill the comment list.
      //
      this.fillCommentList ( record );

      //
      // Reformat Comments into new format if necessary
      //
      this.updateRecordComments ( record );

      // 
      // return the trial record
      // 
      return record;

    }//END getItemQueryRowData class

    #endregion

    #region FormRecord views and query methods.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of form object retrieved by query parameters. 
    /// </summary>
    /// <param name="QueryParameters">EvQueryParameters: (Mandatory) QueryParameter object.</param>
    /// <returns>List of EvForm: a list of form object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define the parameters for the startDate and finishDate 
    /// 
    /// 2. Define the sql query parameters and the sql query string. 
    /// 
    /// 3. Execute the sql query string and store the results on the data table. 
    /// 
    /// 4. Loop through the data table and extract the data row to the form object. 
    /// 
    /// 5. Add formfield items and linkText to the form object if they exist. 
    /// 
    /// 6. Add the form object to the Forms list. 
    /// 
    /// 7. Return the Forms list. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public int getRecordCount (
      EvQueryParameters QueryParameters )
    {
      //
      // Initialize the method debug log, a return form list and a number of result count. 
      //
      this.LogMethod ( "getRecordCount method." );

      List<EdRecord> view = new List<EdRecord> ( );
      int inResultCount = 0;

      //
      // Increment the dates to the less or greater than.
      //
      if ( QueryParameters.StartDate > Evado.Model.Digital.EvcStatics.CONST_DATE_NULL )
      {
        QueryParameters.StartDate = QueryParameters.StartDate.AddDays ( -1 );
      }
      if ( QueryParameters.FinishDate > Evado.Model.Digital.EvcStatics.CONST_DATE_NULL )
      {
        QueryParameters.FinishDate = QueryParameters.FinishDate.AddDays ( 1 );
      }

      if ( QueryParameters.OrgId.ToLower ( ).Contains ( "test" ) == true )
      {
        QueryParameters.IncludeTestSites = true;
      }

      this.LogDebug ( "Adjusted Dates: StartDate: " + QueryParameters.stStartDate
        + ", FinishDate: " + QueryParameters.stFinishDate );

      // 
      // Define the query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(PARM_TrialId, SqlDbType.NVarChar, 10),
        new SqlParameter(PARM_FormId, SqlDbType.NVarChar, 10),
        new SqlParameter(PARM_OrgId, SqlDbType.NVarChar, 10),
        new SqlParameter(PARM_MilestoneId, SqlDbType.NVarChar, 20),
        new SqlParameter(PARM_SubjectId, SqlDbType.NVarChar, 20),
        new SqlParameter(PARM_PATIENT_ID, SqlDbType.Int),
        new SqlParameter(PARM_VisitId, SqlDbType.NVarChar, 20),
        new SqlParameter(PARM_UserName, SqlDbType.NVarChar, 100),
        new SqlParameter(PARM_StartDate, SqlDbType.DateTime),
        new SqlParameter(PARM_FinishDate, SqlDbType.DateTime),
      };
      cmdParms [ 0 ].Value = QueryParameters.ApplicationId;
      cmdParms [ 1 ].Value = QueryParameters.FormId;
      cmdParms [ 2 ].Value = QueryParameters.OrgId;
      cmdParms [ 3 ].Value = QueryParameters.MilestoneId;
      cmdParms [ 4 ].Value = QueryParameters.SubjectId;
      cmdParms [ 5 ].Value = QueryParameters.PatientId;
      cmdParms [ 6 ].Value = QueryParameters.VisitId;
      cmdParms [ 7 ].Value = QueryParameters.UserCommonName;
      cmdParms [ 8 ].Value = QueryParameters.StartDate;
      cmdParms [ 9 ].Value = QueryParameters.FinishDate;

      //
      // Generate the SQL query string.
      //
      _sqlQueryString = this.createSQLquery ( QueryParameters );
      this.LogValue ( " " + _sqlQueryString );

      this.LogValue ( " Execute Query" );
      //
      //Execute the query against the database.
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


          String orgId = EvSqlMethods.getString ( row, "OrgId" );
          string subjectId = EvSqlMethods.getString ( row, "SubjectId" );
          EdRecord record = this.getRowData ( row, QueryParameters.IncludeSummary );

          //
          // If Hide test sites is turned on, then skip all records from test sites.
          //
          if ( QueryParameters.IncludeTestSites == false
            && orgId.ToLower ( ).Contains ( "test" ) == true )
          {
            continue;
          }

          if ( subjectId == String.Empty )
          {
            continue;
          }

          inResultCount++;

        }//END record iteration loop

      }//END using method

      // 
      // Get the array length
      // 
      this.LogValue ( " Returned records: " + inResultCount );

      // 
      // Return the result array.
      // 
      return inResultCount;

    } // Close getRecordList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of form object retrieved by query parameters. 
    /// </summary>
    /// <param name="QueryParameters">EvQueryParameters: (Mandatory) QueryParameter object.</param>
    /// <returns>List of EvForm: a list of form object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define the parameters for the startDate and finishDate 
    /// 
    /// 2. Define the sql query parameters and the sql query string. 
    /// 
    /// 3. Execute the sql query string and store the results on the data table. 
    /// 
    /// 4. Loop through the data table and extract the data row to the form object. 
    /// 
    /// 5. Add formfield items and linkText to the form object if they exist. 
    /// 
    /// 6. Add the form object to the Forms list. 
    /// 
    /// 7. Return the Forms list. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public List<EdRecord> getRecordList (
      EvQueryParameters QueryParameters )
    {
      this.LogMethod ( "getRecordList method." );
      //
      // Initialize the method debug log, a return form list and a number of result count. 
      //
      List<EdRecord> view = new List<EdRecord> ( );
      int inResultCount = 0;

      //
      // Increment the dates to the less or greater than.
      //
      if ( QueryParameters.StartDate > Evado.Model.Digital.EvcStatics.CONST_DATE_NULL )
      {
        QueryParameters.StartDate = QueryParameters.StartDate.AddDays ( -1 );
      }
      if ( QueryParameters.FinishDate > Evado.Model.Digital.EvcStatics.CONST_DATE_NULL )
      {
        QueryParameters.FinishDate = QueryParameters.FinishDate.AddDays ( 1 );
      }

      this.LogDebug ( "Adjusted Dates: StartDate: " + QueryParameters.stStartDate
        + ", FinishDate: " + QueryParameters.stFinishDate );

      // 
      // Define the query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(PARM_TrialId, SqlDbType.NVarChar, 10),
        new SqlParameter(PARM_FormId, SqlDbType.NVarChar, 10),
        new SqlParameter(PARM_OrgId, SqlDbType.NVarChar, 10),
        new SqlParameter(PARM_MilestoneId, SqlDbType.NVarChar, 20),
        new SqlParameter(PARM_SubjectId, SqlDbType.NVarChar, 20),
        new SqlParameter(PARM_PATIENT_ID, SqlDbType.Int),
        new SqlParameter(PARM_VisitId, SqlDbType.NVarChar, 20),
        new SqlParameter(PARM_UserName, SqlDbType.NVarChar, 100),
        new SqlParameter(PARM_StartDate, SqlDbType.DateTime),
        new SqlParameter(PARM_FinishDate, SqlDbType.DateTime),
      };
      cmdParms [ 0 ].Value = QueryParameters.ApplicationId;
      cmdParms [ 1 ].Value = QueryParameters.FormId;
      cmdParms [ 2 ].Value = QueryParameters.OrgId;
      cmdParms [ 3 ].Value = QueryParameters.MilestoneId;
      cmdParms [ 4 ].Value = QueryParameters.SubjectId;
      cmdParms [ 5 ].Value = QueryParameters.PatientId;
      cmdParms [ 6 ].Value = QueryParameters.VisitId;
      cmdParms [ 7 ].Value = QueryParameters.UserCommonName;
      cmdParms [ 8 ].Value = QueryParameters.StartDate;
      cmdParms [ 9 ].Value = QueryParameters.FinishDate;

      //
      // Generate the SQL query string.
      //
      _sqlQueryString = this.createSQLquery ( QueryParameters );
      this.LogDebug (  _sqlQueryString );

      this.LogDebug ( " Execute Query" );
      //
      //Execute the query against the database.
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

          EdRecord record = this.getRowData ( row, QueryParameters.IncludeSummary );

          // 
          // Attach fields and other trial data.
          // 
          if ( QueryParameters.IncludeRecordFields == true )
          {
            if ( inResultCount < QueryParameters.RecordRangeStart
              || inResultCount >= ( QueryParameters.RecordRangeFinish ) )
            {
              this.LogDebug ( "Count: " + inResultCount + " >> not within record range." );

              //
              // Increment the result count.
              //
              inResultCount++;

              continue;
            }

            // 
            // Get the trial record fields
            // 
            record.Fields = this._Dal_FormRecordFields.getRecordFieldList ( record, false );

            //
            // Increment the result count.
            //
            inResultCount++;

          // 
          // Generate the link text for views.
          // 
          record.LinkText = record.RecordId;


          record.LinkText += " > " + record.Design.Title + " ( " + record.StateDesc + " ) ";

          // 
          }//END query selection state not set.

          view.Add ( record );

        }//END record iteration loop

      }//END using method

      // 
      // Get the array length
      // 
      this.LogValue ( " Returned records: " + view.Count );

      // 
      // Return the result array.
      // 
      this.LogMethodEnd ( "getRecordList" );
      return view;

    } // Close getRecordList method.

    // =====================================================================================
    /// <summary>
    /// This class defines an SQL query string based on the query parameters. 
    /// </summary>
    /// <param name="QueryParameters">EvQueryParameters: (Mandatory) EvQueryParameters.</param>
    /// <returns>String: a SQL query string</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Generate the sql query string. 
    /// 
    /// 2. Return the sql query string. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private String createSQLquery (
      EvQueryParameters QueryParameters )
    {
      //
      // Initialize the local sql query string. 
      //
      String sqlQueryString = String.Empty;

      // 
      // Select only the patient if the project id is empty.
      // 
      if ( QueryParameters.PatientId >= 0
        && QueryParameters.ApplicationId == String.Empty )
      {
        // 
        // Define the string to query patient records.
        // 
        sqlQueryString = _sqlQuery_View
          + " WHERE ( (PATIENT_ID = @PATIENT_ID)  ";
      }
      else
      {
        sqlQueryString = _sqlQuery_View + " WHERE ( ( TrialId = @TrialId ) ";


        if ( QueryParameters.PatientId >= 0 )
        {
          // 
          // Define the string to query patient records.
          // 
          sqlQueryString = _sqlQuery_View
            + " AND (PATIENT_ID = @PATIENT_ID)  ";
        }
      }

      if ( QueryParameters.FormId != String.Empty )
      {
        sqlQueryString += " AND ( FormId = @FormId ) ";
      }

      if ( QueryParameters.MilestoneId != String.Empty )
      {
        sqlQueryString += " AND ( MilestoneId = @MilestoneId ) ";
      }

      if ( QueryParameters.OrgId != String.Empty )
      {
        sqlQueryString += " AND ( OrgId = @OrgId ) ";
      }

      if ( QueryParameters.SubjectId != String.Empty )
      {
        sqlQueryString += " AND ( SubjectId = @SubjectId ) ";
      }

      if ( QueryParameters.VisitId != String.Empty )
      {
        sqlQueryString += " AND ( VisitId = @VisitId ) ";
      }

      if ( QueryParameters.UserVisitDate == false )
      {
        if ( QueryParameters.StartDate > Evado.Model.Digital.EvcStatics.CONST_DATE_NULL )
        {
          sqlQueryString += " AND ( TR_RecordDate > @StartDate ) ";
        }

        if ( QueryParameters.FinishDate > Evado.Model.Digital.EvcStatics.CONST_DATE_NULL )
        {
          sqlQueryString += " AND ( TR_RecordDate < @FinishDate) ";
        }
      }
      else
      {
        if ( QueryParameters.StartDate > Evado.Model.Digital.EvcStatics.CONST_DATE_NULL )
        {
          sqlQueryString += " AND ( SM_StartDate > @StartDate ) ";
        }

        if ( QueryParameters.FinishDate > Evado.Model.Digital.EvcStatics.CONST_DATE_NULL )
        {
          sqlQueryString += " AND ( SM_StartDate < @FinishDate) ";
        }
      }

      //
      // User name filter
      //
      if ( QueryParameters.UserCommonName != String.Empty )
      {
        sqlQueryString += " AND ( TR_AuthoredBy = @UserName "
          + "OR TR_QueriedBy = @UserName  "
          + "OR TR_ReviewedBy = @UserName  "
          + "OR TR_ApprovedBy = @UserName  "
          + "OR TR_Monitor = @UserName  "
          + "OR TR_UpdatedBy = @UserName  ) ";
      }

      //
      // Reccord state filter.
      //
      if ( QueryParameters.State != String.Empty
       && QueryParameters.State != EdRecordObjectStates.Null.ToString ( ) )
      {
        // 
        // Open the state query expression 
        // 
        if ( QueryParameters.NotSelectedState == true )
        {
          sqlQueryString += " AND NOT( ";
        }
        else
        {
          sqlQueryString += " AND ( ";
        }

        // 
        // Does the state variable have multiple state items.
        // 
        if ( QueryParameters.State.Contains ( ";" ) == true )
        {
          // 
          // If multiple states then add them to the query string.
          // 
          string multipleStateQuery = String.Empty;

          // 
          // Iterate through the states adding the to they query.
          // 
          foreach ( string state in QueryParameters.State.Split ( ';' ) )
          {
            if ( QueryParameters.State == EdRecordObjectStates.Null.ToString ( ) )
            {
              continue;
            }

            // 
            // add the OR very for the next state in the query.
            // 
            if ( multipleStateQuery != String.Empty )
            {
              multipleStateQuery += " OR ";
            }

            // 
            // Add the next state qury verb
            // 
              multipleStateQuery += " ( TR_State = '" + state.Trim ( ) + "' ) ";
          }

          // 
          // Add the multi state query verbs
          // 
          sqlQueryString += multipleStateQuery;

        }
        else
        {
          if ( QueryParameters.State == EdRecordObjectStates.Draft_Record.ToString ( ) )
          {
            sqlQueryString += " (TR_State = '" + EdRecordObjectStates.Draft_Record + "' "
              + "OR  TR_State = '" + EdRecordObjectStates.Empty_Record + "' "
              + "OR TR_State = '" + EdRecordObjectStates.Completed_Record + "') ";
          }
          else
          {
              sqlQueryString += " TR_State = '" + QueryParameters.State.Trim ( ) + "' ";
          }

        }// END single state.

        // 
        // Close the state query expression.
        // 
        sqlQueryString += ") ";

      }//END DebugLog query generatation

      if ( QueryParameters.OrderBy.Length == 0 )
      {
        sqlQueryString += ") ORDER BY RecordId";
      }
      else
      {
        sqlQueryString += ") ORDER BY " + QueryParameters.OrderBy;
      }

      //
      // Return the sql query string. 
      //
      return sqlQueryString;
    }

    // =====================================================================================
    /// <summary>
    /// This class returns a list of form object based on VisitId, VisitId, FormId and state
    /// </summary>
    /// <param name="ProjectId">string: (Mandatory) a trial identifier.</param>
    /// <param name="VisitId">string: (Optional) a visit identifier.</param>
    /// <param name="FormId">string: (Optional) a form identifier.</param>
    /// <param name="State">EvForm.FormObjecStates: (Optional) a form state.</param>
    /// <returns>List of EvForm: a list of form object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define the sql query parameters and the sql query string.
    /// 
    /// 2. Execute the sql query string and store the results on the data table. 
    /// 
    /// 3. Loop through the table and extract data row to the form object. 
    /// 
    /// 4. Add the formfield items to the form object. 
    /// 
    /// 5. Add the form object to the Forms list. 
    /// 
    /// 6. Return the Forms list. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public List<EdRecord> getRecordList (
      String ProjectId,
      String VisitId,
      String FormId,
      EdRecordObjectStates State )
    {
      this.LogMethod ( "getRecordList method " );
      this.LogValue ( "ProjectId: " + ProjectId );
      this.LogValue ( "VisitId: " + VisitId );
      this.LogValue ( "FormId: " + FormId );
      this.LogValue ( "State: " + State );

      //
      // Initialize the debuglog, a return list of form object and a formRecord field object. 
      //
      List<EdRecord> view = new List<EdRecord> ( );

      // 
      // Define the query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(PARM_TrialId, SqlDbType.NVarChar, 10),
        new SqlParameter(PARM_FormId, SqlDbType.NVarChar, 10),
        new SqlParameter(PARM_VisitId, SqlDbType.NVarChar, 20),
        new SqlParameter(PARM_State, SqlDbType.VarChar, 25),
      };
      cmdParms [ 0 ].Value = ProjectId;
      cmdParms [ 1 ].Value = FormId;
      cmdParms [ 2 ].Value = VisitId;
      cmdParms [ 3 ].Value = State.ToString ( );

      // 
      // Generate the SQL query string.
      // 
      _sqlQueryString = _sqlQuery_View + " WHERE ( ( TrialId = @TrialId ) ";

      if ( VisitId != String.Empty )
      {
        _sqlQueryString += " AND ( VisitId = @VisitId ) ";
      }

      if ( FormId != String.Empty )
      {
        _sqlQueryString += " AND ( FormId = @FormId ) ";
      }

      if ( State != EdRecordObjectStates.Null )
      {
        _sqlQueryString += " AND (TR_State = @State) ";
      }

      _sqlQueryString += ") ORDER BY SubjectId, RecordId";

      //_Status += "\r\n " + sqlQueryString;

      this.LogValue ( " Execute Query" );

      //
      //Execute the query against the database.
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

          EdRecord record = this.getRowData ( row, true );

          // 
          // Get the trial record items
          // 
          record.Fields = _Dal_FormRecordFields.getRecordFieldList ( record, false );

          // 
          // Add the result to the arraylist.
          // 
          view.Add ( record );

          // 
          // TestReport the visitSchedule count is less than the max size.
          // 
          if ( view.Count > _MaxViewLength )
          {
            break;
          }

        }//END for loop

      }//END Using

      // 
      // Get the array length
      // 
      this.LogValue ( " Returned records: " + view.Count );

      // 
      // Return the result array.
      // 
      return view;

    }//END getView method.

    // =====================================================================================
    /// <summary>
    /// This class gets a list of current records from form object based on VisitId, OrgId, SubjectId, VisitId and OrderBy.
    /// </summary>
    /// <param name="ProjectId">string: (Mandatory) a Project identifier.</param>
    /// <param name="OrgId">string: (Optional) an organization identifier.</param>
    /// <param name="SubjectId">string: (Optional) a milestone identifier.</param>
    /// <param name="VisitId">string: (Optional) a visit identifier.</param>
    /// <param name="IncludeRecordFields">bool: true = include field records.</param>
    /// <returns>List of EvForm: a list of form object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define the sql query parameters and sql query string. 
    /// 
    /// 2. Execute the sql query string and store the results on data table. 
    /// 
    /// 3. Iterate through data table and extract the datarow to form object. 
    /// 
    /// 4. Add the form object values to the Forms list. 
    /// 
    /// 5. Return the forms list. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public List<EdRecord> getCurrentRecordList (
      String ProjectId,
      String OrgId,
      String SubjectId,
      String VisitId,
      bool IncludeRecordFields )
    {
      this.LogMethod ( "getCurrentRecordList method." );
      this.LogValue ( "ProjectId: " + ProjectId );
      this.LogValue ( "SubjectId: " + SubjectId );
      this.LogValue ( "VisitId: " + VisitId );
      this.LogValue ( "OrgId: " + OrgId );
      this.LogValue ( "IncludeRecordFields: " + IncludeRecordFields );

      //
      // Initialize the debug log and a return list of form object. 
      //
      List<EdRecord> recordList = new List<EdRecord> ( );

      // 
      // Define the query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(PARM_TrialId, SqlDbType.NVarChar, 10),
        new SqlParameter(PARM_OrgId, SqlDbType.NVarChar, 10),
        new SqlParameter(PARM_SubjectId, SqlDbType.NVarChar, 20),
        new SqlParameter(PARM_VisitId, SqlDbType.NVarChar, 20),
      };
      cmdParms [ 0 ].Value = ProjectId;
      cmdParms [ 1 ].Value = OrgId;
      cmdParms [ 2 ].Value = SubjectId;
      cmdParms [ 3 ].Value = VisitId;

      // 
      // Generate the SQL query string.
      // 
      _sqlQueryString = _sqlQuery_View + " WHERE ( ( TrialId = @TrialId ) ";

      if ( VisitId != String.Empty )
      {
        _sqlQueryString += " AND ( VisitId = @VisitId ) ";
      }
      if ( OrgId != String.Empty )
      {
        _sqlQueryString += " AND ( OrgId = @OrgId ) ";
      }
      if ( SubjectId != String.Empty )
      {
        _sqlQueryString += " AND ( SubjectId = @SubjectId ) ";
      }
      _sqlQueryString += " AND (TR_State <> '" + EdRecordObjectStates.Withdrawn + "' ) ";

      _sqlQueryString += ") ORDER BY RecordId";

      this.LogValue ( _sqlQueryString );

      this.LogValue ( " Execute Query" );

      //
      // Execute the query against the database.
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

          EdRecord record = this.getRowData ( row, false );

          // 
          // Get the trial record fields
          // 
          if ( IncludeRecordFields == true )
          {
            record.Fields = _Dal_FormRecordFields.getRecordFieldList ( record, false );
          }

          // 
          // Add the result to the arraylist.
          // 
          recordList.Add ( record );

          // 
          // TestReport the visitSchedule count is less than the max size.
          // 
          if ( recordList.Count > _MaxViewLength )
          {
            break;
          }

        }//END While loop

      }//END Using

      // 
      // Get the array length
      // 
      this.LogValue ( "Returned records: " + recordList.Count );

      // 
      // Return the result array.
      // 
      return recordList;

    }//END getCurrentRecordsView method.

    // =====================================================================================
    /// <summary>
    /// This class retrieves an option list based on query parameters and the useGuid condition
    /// </summary>
    /// <param name="Query">EvQueryParameters: The Query selection values.</param>
    /// <param name="useGuid">Boolean: true, if the option uses Guid.</param>
    /// <returns>List of EvOption: a list of option object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Get a list of form object using query parameters to the form list. 
    /// 
    /// 2. Loop through the form list and extract the option value and description to the Options list. 
    /// 
    /// 3. Return the Options list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> getOptionList (
      EvQueryParameters Query,
      bool useGuid )
    {
      this.LogMethod ( "getOptionList method. " );
      this.LogValue ( "EvQueryParameters parameters:" );
      this.LogValue ( "- ProjectId: " + Query.ApplicationId );
      this.LogValue ( "- SubjectId: " + Query.SubjectId );

      //
      // Initialize the debug log, a return list of options and an option object.
      //
      List<EvOption> list = new List<EvOption> ( );

      // 
      // Add the null first option to the selection visitSchedule.
      // 
      EvOption option = new EvOption ( );
      if ( useGuid )
      {
        option = new EvOption ( Guid.Empty.ToString ( ), String.Empty );
      }
      list.Add ( option );

      // 
      // Execute method and return the values.
      // 
      List<EdRecord> view = this.getRecordList ( Query );

      // 
      // if the selectionList exits process it.
      // 
      if ( view.Count > 0 )
      {
        // 
        // Iterate through the array visitSchedule extracting the trial records.
        // 
        foreach ( EdRecord record in view )
        {
          option = new EvOption ( record.RecordId, String.Empty );

          if ( useGuid == true )
          {
            option.Value = record.Guid.ToString ( );
          }

          option.Description += record.RecordId + " - " + record.Design.Title;

          // 
          // Append the SelectionObject object to the ArrayList.
          // 
          list.Add ( option );

        }//End iteration loop

      }//END selectionList visitSchedule exists.

      // 
      // Get the array length
      // 
      this.LogValue ( " Returned records: " + list.Count );

      // 
      // Return the visitSchedule array
      // 
      return list;

    }//END geList method

    // =====================================================================================
    /// <summary>
    /// This class gets a list of options based on VisitId, SubjectId and Uid condition
    /// </summary>
    /// <param name="ProjectId">string: (Mandatory) a trial identifier.</param>
    /// <param name="SubjectId">string: (Optional) a milestone identifier.</param>
    /// <param name="userGuid">Boolean: true, if Uid is selected</param>
    /// <returns>List of EvOption: a list of option object</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Define the sql query parameters and sql query string. 
    /// 
    /// 2. Execute the sql query string and store the results on data table. 
    /// 
    /// 3. Loop through the table and extract data row to option object. 
    /// 
    /// 4. Add the option object values to the Options list. 
    /// 
    /// 5. Return the Options list.
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public List<EvOption> getOptionList (
      String ProjectId,
      String SubjectId,
      bool userGuid )
    {
      this.LogMethod ( "getOptionList method." );
      this.LogValue ( "Direct parameters:" );
      this.LogValue ( "- ProjectId: " + ProjectId );
      this.LogValue ( "- SubjectId: " + SubjectId );

      //
      // Initialize the debug log, return option list, option object and the local variables. 
      //
      int Rows = 0;
      int maxListLength = 100;
      if ( int.TryParse ( _maximumListLength, out maxListLength ) == false )
      {
        maxListLength = 100;
      }
      List<EvOption> list = new List<EvOption> ( );

      // 
      // Add the null first option to the selection visitSchedule.
      // 
      EvOption option = new EvOption ( );
      list.Add ( option );

      // 
      // Define the query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(PARM_TrialId, SqlDbType.NVarChar, 10),
        new SqlParameter(PARM_SubjectId, SqlDbType.NVarChar, 20),
      };
      cmdParms [ 0 ].Value = ProjectId;
      cmdParms [ 1 ].Value = SubjectId;

      // 
      // Generate the SQL query string.
      // 
      _sqlQueryString = _sqlQuery_View + " WHERE ( TrialId = @TrialId ) ";

      if ( SubjectId.Length > 0 )
      {
        _sqlQueryString += " AND ( SubjectId = @SubjectId ) ";
      }
      _sqlQueryString += " ORDER BY RecordId";

      //_Status = "\r\n " + sqlQueryString;

      //
      // Execute the query against the database.
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

          option = new EvOption ( EvSqlMethods.getString ( row, "RecordId" ), String.Empty );

          if ( userGuid == true )
          {
            option.Value = EvSqlMethods.getString ( row, "TC_Guid" );
          }
          if ( SubjectId == String.Empty )
          {
            option.Description += "(" + EvSqlMethods.getString ( row, "SubjectId" ) + ") ";
          }
          option.Description += EvSqlMethods.getString ( row, "RecordId" ) + " - " + EvSqlMethods.getString ( row, "TC_Title" );

          // 
          // Append the SelectionObject object to the ArrayList.
          // 
          list.Add ( option );
          Rows++;

          if ( Rows > maxListLength )
          {
            break;
          }

        }//END recrod interation loop

      }//END using statement

      // 
      // Get the array length
      // 
      this.LogValue ( " Returned records: " + list.Count );

      // 
      // Return the result array.
      // 
      return list;

    } // Close getTemplateList method.

    // =====================================================================================
    /// <summary>
    /// This class indicates whether a form has been used.
    /// </summary>
    /// <param name="ProjectId">string: (Mandatory) a trial identifier.</param>
    /// <param name="FormId">string: (Mandatory) a form identifier</param>
    /// <returns>Boolean: true if the form is found</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Return false, if formId is empty. 
    /// 
    /// 3. Define the sql query parameters and sql query string
    /// 
    /// 4. Execute the sql query string and store the results on data reader. 
    /// 
    /// 5. Return true if data reader has values or false for the reverse case. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public bool checkIfFormUsed (
      String ProjectId,
      String FormId )
    {
      this.LogMethod ( "checkIfFormUsed method." );
      this.LogValue ( "Direct parameters:" );
      this.LogValue ( "- ProjectId: " + ProjectId );
      this.LogValue ( "- FormId: " + FormId );

      // 
      // Check that the UID is a valid variable.
      // 
      if ( FormId == String.Empty )
      {
        return false;
      }

      // 
      // Define the query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( PARM_TrialId, SqlDbType.NVarChar, 10 ),
        new SqlParameter( PARM_FormId, SqlDbType.NVarChar, 10 )
      };
      cmdParms [ 0 ].Value = ProjectId;
      cmdParms [ 1 ].Value = FormId;

      // 
      // Generate the SQL query string.
      // 
      _sqlQueryString = _sqlQuery_View
        + "WHERE ( TrialId = @TrialId ) AND ( FormId = @FormId ) ORDER BY RecordId";
      //_Status = "\r\n " + sqlQueryString;

      //
      // Execute the query against the database.
      //
      using ( SqlDataReader rdr = EvSqlMethods.ExecuteReader (
               CommandType.Text, _sqlQueryString, cmdParms ) )
      {
        //
        // If the reader is null then their was not connection string.
        //
        if ( rdr == null )
        {
          return false;
        }

        if ( rdr.Read ( ) )
        {
          return true;
        }
      }

      // 
      // None Found
      // 
      return false;

    }//END checkIfFormUsed method.
    // =====================================================================================
    /// <summary>
    /// This class performs a pivot query on EvRecord to produce a queryState
    /// of the trial Subject states.
    /// </summary>
    /// <param name="SubjectId">string: (Optional) a milestone identifier.</param>
    /// <returns>List of EvFormRecordSummary: a list of Form record queryState object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Set Group string value based on OrgId and SubjectId. 
    /// 
    /// 2. Define the sql query parameters and sql query string. 
    /// 
    /// 3. Execute the sql query string and store the results on data table. 
    /// 
    /// 4. Loop through the table and extract data row to the record queryState object. 
    /// 
    /// 5. Add the object values to the Form Record queryState list. 
    /// 
    /// 6. Return the Form Record Summary list. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvFormRecordSummary getRecordSummary (
      String SubjectId )
    {
      this.LogMethod ( "getRecordSummary method." );
      this.LogValue ( "- SubjectId: " + SubjectId );
      //
      // Initialize the method debug log, a return list of record queryState object and a group string. 
      //
      EvFormRecordSummary summary = new EvFormRecordSummary ( );
      string sGroup = "SubjectId";

      // 
      // Define the query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter(PARM_SubjectId, SqlDbType.NVarChar, 20),
      };

      cmdParms [ 0 ].Value = SubjectId;

      _sqlQueryString = "Select " + sGroup + ", "
        + "SUM(CASE TR_State WHEN '" + EdRecordObjectStates.Draft_Record + "' THEN 1 ELSE 0 END)  AS Draft_Record , "
        + "SUM(CASE TR_State WHEN 'Queried_Record' THEN 1 ELSE 0 END)  AS Queried_Record , "
        + "SUM(CASE TR_State WHEN 'Submitted_Record' THEN 1 ELSE 0 END)   AS Submitted_Record, "
        + "SUM(CASE TR_Monitor WHEN  'Monitor' THEN 0 ELSE 1 END) AS Source_Data_Verified, "
        + "SUM(CASE TR_State WHEN 'Locked_Record' THEN 1 ELSE 0 END) AS Locked_Record, "
        + "SUM(CASE TR_State WHEN '" + EdRecordObjectStates.Withdrawn + "' THEN 1 ELSE 0 END) AS Cancelled_Records, "
        + "SUM(CASE TR_QueryState WHEN 'Open' THEN 1 ELSE 0 END)  AS Open_Queried_Records , "
        + "SUM(CASE TR_QueryState WHEN 'Closed' THEN 1 ELSE 0 END) AS Closed_Queried_Records, "
        + "COUNT(TR_State) AS Total "
        + "FROM EvRecord_View "
        + "WHERE SubjectId = @SubjectId ";

      _sqlQueryString += "GROUP BY " + sGroup + ";";

      this.LogValue ( " " + _sqlQueryString );

      //
      // Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( _sqlQueryString, cmdParms ) )
      {
        if ( table.Rows.Count == 0 )
        {
          return summary;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        summary.SubjectId = EvSqlMethods.getString ( row, "SubjectId" );
        summary.DraftRecords = EvSqlMethods.getInteger ( row, "Draft_Record" ) + EvSqlMethods.getInteger ( row, "Queried_Record" );
        summary.SubmittedRecords = EvSqlMethods.getInteger ( row, "Submitted_Record" );
        summary.SourceDataReviewedRecords = EvSqlMethods.getInteger ( row, "Source_Data_Verified" );
        summary.LockedRecords = EvSqlMethods.getInteger ( row, "Locked_Record" );
        summary.CancelledRecords = EvSqlMethods.getInteger ( row, "Cancelled_Records" );
        summary.OpenQueriedRecords = EvSqlMethods.getInteger ( row, "Open_Queried_Records" );
        summary.ClosedQueriedRecords = EvSqlMethods.getInteger ( row, "Closed_Queried_Records" );
        summary.TotalRecords = EvSqlMethods.getInteger ( row, "Total" );
      }

      // 
      // Return the result array.
      // 
      return summary;

    }//END getRecordSummary method.

    // =====================================================================================
    /// <summary>
    /// This class performs a pivot query on EvRecord to produce a queryState
    /// of the trial Subject states.
    /// </summary>
    /// <param name="ProjectId">string: (Mandatory) a trial identifier.</param>
    /// <returns>List of EvFormRecordSummary: a list of Form record queryState object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Set Group string value based on OrgId and SubjectId. 
    /// 
    /// 2. Define the sql query parameters and sql query string. 
    /// 
    /// 3. Execute the sql query string and store the results on data table. 
    /// 
    /// 4. Loop through the table and extract data row to the record queryState object. 
    /// 
    /// 5. Add the object values to the Form Record queryState list. 
    /// 
    /// 6. Return the Form Record Summary list. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public List<EvFormRecordSummary> getSubjectRecordCount (
      String ProjectId )
    {
      this.LogMethod ( "getSubjectRecordCount method." );
      this.LogValue ( "Direct parameters:" );
      this.LogValue ( "- ProjectId: " + ProjectId );
      //
      // Initialize the method debug log, a return list of record queryState object and a group string. 
      //
      List<EvFormRecordSummary> formRecordSummarylist = new List<EvFormRecordSummary> ( );
      string sGroup = "SubjectId";


      // 
      // Define the query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(PARM_TrialId, SqlDbType.NVarChar, 10),
      };

      cmdParms [ 0 ].Value = ProjectId;

      _sqlQueryString = "Select " + sGroup + ", "
        + " COUNT(TR_State) AS Total \r\n"
        + "FROM EvRecord_View \r\n"
        + "WHERE TrialId = @TrialId \r\n"
        + "GROUP BY " + sGroup + "\r\n"
        + "ORDER BY " + sGroup + ";";

      this.LogValue ( _sqlQueryString );

      //
      // Execute the query against the database.
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

          EvFormRecordSummary summary = new EvFormRecordSummary ( );
          summary.SubjectId = EvSqlMethods.getString ( row, "SubjectId" );
          summary.TotalRecords = EvSqlMethods.getInteger ( row, "Total" );
          // 
          // Append the EvRecord to the result array.
          // 
          if ( summary.TotalRecords > 0 )
          {
            this.LogValue ( "Subject: " + summary.SubjectId + " " + summary.TotalRecords );
            formRecordSummarylist.Add ( summary );
          }
        }

      }
      this.LogValue ( "summaryList Count: " + formRecordSummarylist.Count );

      // 
      // Return the result array.
      // 
      return formRecordSummarylist;

    }//END getRecordSummary method.

    #endregion

    #region Form record status report methods.

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
    public EvReport getRecordStatusReport ( EvReport Report )
    {
      //
      // Initialize the Method debug log, a report rows list and a formfield object
      //
      this.LogMethod ( "getRecordStatusReport. " );
      this.LogValue ( "Query count: " + Report.Queries.Length );
      this.LogValue ( "Columns count: " + Report.Columns.Count );


      this.LogValue ( "Parameters: " );
      for ( int i = 0; i < Report.Queries.Length; i++ )
      {
        this.LogValue ( Report.Queries [ i ].SelectionSource + " = " + Report.Queries [ i ].Value );
      }

      //
      // Initialise the methods variables and objects.
      //
      List<EvReportRow> reportRows = new List<EvReportRow> ( );
      List<EdRecord> scheduleFormList = new List<EdRecord> ( ); ;
      List<EdRecord> subjectRecordList = new List<EdRecord> ( );
      String trialId = String.Empty;
      String subjectId = String.Empty;

      //
      // Extract the parameters from the parameter list.
      //
      for ( int i = 0; i < Report.Queries.Length; i++ )
      {
        if ( Report.Queries [ i ].SelectionSource == EvReport.SelectionListTypes.Current_Application )
        {
          trialId = Report.Queries [ i ].Value;
        }

        if ( Report.Queries [ i ].SelectionSource == EvReport.SelectionListTypes.Subject_Id )
        {
          subjectId = Report.Queries [ i ].Value;
        }
      }//END parameter iteration loop.

      this.LogValue ( "projectId: " + trialId );
      this.LogValue ( "subjectId: " + subjectId );

      //
      // Set the report title, datasource and date.
      //
      if ( Report.ReportTitle == String.Empty )
      {
        Report.ReportTitle = "Scheduled Subject CRF record status report.";
        Report.ReportSubTitle = "CRFs that do not have a RecordId are missing records.";
      }
      Report.DataSourceId = EvReport.ReportSourceCode.Subject_Record_Status;
      Report.ReportDate = DateTime.Now;
      Report.LayoutTypeId = EvReport.LayoutTypeCode.GroupedTable;
      Report.DataRecords = new List<EvReportRow> ( );

      //
      // If the milestone is empty return an empty report.
      //
      if ( subjectId == String.Empty
        || trialId == String.Empty )
      {
        return Report;
      }

      //
      // Set the report headers.
      //
      this.setScheduleFormReportColumns ( Report );

      //
      // Llist the columns.
      //
      for ( int i = 0; i < Report.Columns.Count; i++ )
      {
        this.LogValue ( "Column: " + i + ", Title: " + Report.Columns [ i ].HeaderText );
      }

      this.LogValue ( "Field count: " + reportRows.Count );

      #region create the record lists for the report.

      // return Report;
      //
      // retrieve the list of milestone's records.
      //
      subjectRecordList = getSubjectRecordList ( trialId, subjectId );

      #endregion

      //
      // Iterate through the each schedule form list object and update it with
      // a records parameters if the record exists.
      //
      if ( Report.DataRecords.Count == 0 )
      {
        foreach ( EdRecord form in scheduleFormList )
        {
          //
          // CREATE new report row if there is no report data
          //
          EvReportRow reportRow = new EvReportRow ( Report.Columns.Count );

          this.hasFormRecordObject ( subjectRecordList, form );


          reportRow.ColumnValues [ 0 ] = form.MilestoneId;
          reportRow.ColumnValues [ 1 ] = form.ActivityId;
          reportRow.ColumnValues [ 2 ] = form.LayoutId;
          reportRow.ColumnValues [ 3 ] = form.Title;
          reportRow.ColumnValues [ 4 ] = "No";
          if ( form.IsMandatory == true )
          {
            reportRow.ColumnValues [ 4 ] = "Yes";
          }
          reportRow.ColumnValues [ 5 ] = String.Empty;
          reportRow.ColumnValues [ 6 ] = form.RecordId;
          reportRow.ColumnValues [ 7 ] = form.StateDesc;

          this.LogValue ( "ColumnValues [ 1 ]: " + reportRow.ColumnValues [ 1 ] );

          Report.DataRecords.Add ( reportRow );

        }//END scheduled form list iteration loop.

      }//END ADD EMPTY ROW

      // 
      // Return the result array.
      // 
      return Report;
    }


    //====================================================================================
    /// <summary>
    /// This method generates the schedules form list.
    /// </summary>
    /// <param name="Report">EvReport object</param>
    //------------------------------------------------------------------------------------
    private void setScheduleFormReportColumns (
      EvReport Report )
    {
      this.LogMethod ( "setScheduleFormReportColumns methods. " );

      //
      // reset the column header descriptions of necessary.
      //
      if ( Report.Columns.Count == 8 )
      {
        //return; 
      }

      //
      // Initialise the methods variables and objects.
      //
      Report.Columns = new List<EvReportColumn> ( );
      EvReportColumn column;

      // 
      // Set the milestone  column 0
      // 
      column = new EvReportColumn ( );
      column.HeaderText = "Milestone";
      column.SectionLvl = 1;
      column.GroupingIndex = true;
      column.DataType = EvReport.DataTypes.Text;
      column.SourceField = "MilestoneId";
      column.GroupingType = EvReport.GroupingTypes.None;

      Report.Columns.Add ( column );

      // 
      // Set the activity column 1
      // 
      column = new EvReportColumn ( );
      column.HeaderText = "ActivityId";
      column.SectionLvl = 2;
      column.GroupingIndex = true;
      column.DataType = EvReport.DataTypes.Text;
      column.SourceField = "ActivityId";
      column.GroupingType = EvReport.GroupingTypes.None;

      Report.Columns.Add ( column );


      // 
      // Set the form column 3
      // 
      column = new EvReportColumn ( );
      column.HeaderText = "FormId";
      column.SectionLvl = 0;
      column.GroupingIndex = true;
      column.DataType = EvReport.DataTypes.Text;
      column.SourceField = "FormId";
      column.GroupingType = EvReport.GroupingTypes.None;
      column.StyleWidth = "80px";

      Report.Columns.Add ( column );

      // 
      // Set the form title column 4
      // 
      column = new EvReportColumn ( );
      column.HeaderText = "Title";
      column.SectionLvl = 0;
      column.GroupingIndex = false;
      column.DataType = EvReport.DataTypes.Text;
      column.SourceField = "FormTitle";
      column.GroupingType = EvReport.GroupingTypes.None;

      Report.Columns.Add ( column );

      // 
      // Set the form form is mandatory column 5
      // 
      column = new EvReportColumn ( );
      column.HeaderText = "Mandatory";
      column.SectionLvl = 0;
      column.GroupingIndex = false;
      column.DataType = EvReport.DataTypes.Text;
      column.SourceField = "Mandatory";
      column.GroupingType = EvReport.GroupingTypes.None;
      column.StyleWidth = "80px";

      Report.Columns.Add ( column );

      // 
      // Set the Visit column 1
      // 
      column = new EvReportColumn ( );
      column.HeaderText = "VisitId";
      column.SectionLvl = 0;
      column.GroupingIndex = false;
      column.DataType = EvReport.DataTypes.Text;
      column.SourceField = "VisitId";
      column.GroupingType = EvReport.GroupingTypes.None;
      column.StyleWidth = "140px";

      Report.Columns.Add ( column );

      // 
      // Set the record id column 6
      // 
      column = new EvReportColumn ( );
      column.HeaderText = "RecordId";
      column.SectionLvl = 0;
      column.GroupingIndex = false;
      column.DataType = EvReport.DataTypes.Text;
      column.SourceField = "RecordId";
      column.GroupingType = EvReport.GroupingTypes.None;
      column.StyleWidth = "140px";

      Report.Columns.Add ( column );

      // 
      // Set the record state column 7
      // 
      column = new EvReportColumn ( );
      column.HeaderText = "State";
      column.SectionLvl = 0;
      column.GroupingIndex = false;
      column.DataType = EvReport.DataTypes.Text;
      column.SourceField = "State";
      column.GroupingType = EvReport.GroupingTypes.None;

      Report.Columns.Add ( column );

    }//END setScheduleFormReportColumns method
    //====================================================================================
    /// <summary>
    /// This method generates the schedules form list.
    /// </summary>
    /// <param name="FormRecordList">List of milestone form record objects object</param>
    /// <param name="ScheduleForm">EvForm: the scheduled form object.</param>
    //------------------------------------------------------------------------------------
    private bool hasFormRecordObject (
      List<EdRecord> FormRecordList,
      EdRecord ScheduleForm )
    {
      this.LogMethod ( "hasFormRecordObject methods. " );
      //
      // Initialise the variable values.
      //
      ScheduleForm.Selected = false;
      ScheduleForm.State = EdRecordObjectStates.Null;

      //
      // Iterate through the form records looking for a matching record.
      //
      foreach ( EdRecord formRecord in FormRecordList )
      {
        //
        // skip queried and withdrawn records.
        //
        if ( formRecord.State == EdRecordObjectStates.Withdrawn )
        {
          continue;
        }

        //
        // match the milestone, activity and form.
        //
        if ( ScheduleForm.MilestoneId == formRecord.MilestoneId
          && ScheduleForm.ActivityId == formRecord.ActivityId
          && ScheduleForm.LayoutId == formRecord.LayoutId )
        {
          ScheduleForm.RecordId = formRecord.RecordId;
          ScheduleForm.State = formRecord.State;
          ScheduleForm.Selected = true;

          this.LogValue ( "RecordId: " + ScheduleForm.RecordId );
          this.LogValue ( "State: " + ScheduleForm.State );

          return true;
        }//END selection.

      }//END iteration loop.

      return false;
    }//END hasFormRecordObject method

    // =====================================================================================
    /// <summary>
    /// Description:
    /// Gets a selectionList of trial particpant records.
    /// 
    /// </summary>
    /// <param name="TrialId">String: (Mandatory) defines trial to the query.</param>
    /// <param name="SubjectId">String: (Mandatory) Defines the participant to the query.</param>
    /// <returns>List of EvForm objects</returns>
    //  ----------------------------------------------------------------------------------
    private List<EdRecord> getSubjectRecordList (
      String TrialId,
      String SubjectId )
    {
      this.LogMethod ( "getSubjectRecordList method. " );
      this.LogValue ( "ProjectId: " + TrialId );
      this.LogValue ( "SubjectId: " + SubjectId );
      // 
      // Define the local variables.
      // 
      List<EdRecord> recordList = new List<EdRecord> ( );


      // 
      // Define the query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(PARM_TrialId, SqlDbType.NVarChar, 10),
        new SqlParameter(PARM_SubjectId, SqlDbType.NVarChar, 20),
      };
      cmdParms [ 0 ].Value = TrialId;
      cmdParms [ 1 ].Value = SubjectId;

      // 
      // Generate the SQL query string.
      // 
      _sqlQueryString = _sqlQuery_View + " WHERE ( TrialId = @TrialId ) "
        + " AND ( SubjectId = @SubjectId ) "
        + " ORDER BY RecordId";

      //
      //Execute the query against the database.
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

          //
          // fill the record object.
          //
          EdRecord record = this.getRowData ( row, false );

          //
          // append the record.
          //
          recordList.Add ( record );

        }//END recrod interation loop

      }//END using statement

      // 
      // Get the array length
      // 
      this.LogValue ( "Returned records: " + recordList.Count );

      // 
      // Return the result array.
      // 
      return recordList;

    } // Close getSubjectRecordList method.

    // =====================================================================================
    /// <summary>
    /// getRowData method
    /// 
    /// Description:
    ///  Extract EvRecord data from SqlReader.
    /// 
    /// </summary>
    /// <param name="Row">The datatable reader contain the query results</param>
    /// <returns>EvForm</returns>
    //  ----------------------------------------------------------------------------------
    private EdRecord getScheduleFormsRowData ( DataRow Row )
    {
      // 
      // Initialise the trial record object.
      // 
      EdRecord form = new EdRecord ( );
      form.Selected = false;

      // 
      // Update the trial record object with query values.
      // 
      form.ApplicationId = EvSqlMethods.getString ( Row, "TrialId" );
      form.MilestoneId = EvSqlMethods.getString ( Row, "MilestoneId" );
      form.ActivityId = EvSqlMethods.getString ( Row, "ActivityId" );
      form.IsMandatoryActivity = EvSqlMethods.getBool ( Row, "MA_IsMandatory" );
      form.IsMandatory = EvSqlMethods.getBool ( Row, "ACF_Mandatory" );
      form.LayoutId = EvSqlMethods.getString ( Row, "FormId" );
      form.Design.Title = EvSqlMethods.getString ( Row, "TC_Title" );

      form.Design.TypeId = Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue<EdRecordTypes> ( EvSqlMethods.getString ( Row, "TC_TypeId" ) );

      return form;

    }// END getRowData

    //====================================================================================
    /// <summary>
    /// This method generates the schedules form list.
    /// </summary>
    /// <param name="ProjectId">String: trial identifier</param>
    /// <param name="ScheduleId">EvTrialArm.ScheduleIdes enumeration: trial identifier</param>
    /// <returns></returns>
    //------------------------------------------------------------------------------------
    private List<EdRecord> getScheduleFormList (
      String ProjectId,
      int ScheduleId )
    {
      this.LogMethod ( "getScheduleFormList method. " );
      this.LogValue ( "ProjectId: " + ProjectId );
      this.LogValue ( "ScheduleId: " + ScheduleId );
      //
      // Initialise the methods variables and objects.
      //
      List<EdRecord> scheduleFormList = new List<EdRecord> ( );
      // 
      // Define the query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( PARM_TrialId, SqlDbType.NVarChar, 10),
        new SqlParameter( PARM_ARM_INDEX, SqlDbType.Int),
      };
      cmdParms [ 0 ].Value = ProjectId;

      cmdParms [ 1 ].Value = 1;
      if ( ScheduleId > 0 )
      {
        cmdParms [ 1 ].Value = ScheduleId;
      }

      this.LogValue ( "Parameters:" );
      foreach ( SqlParameter prm in cmdParms )
      {
        this.LogValue ( "Typ: " + prm.DbType
          + " ParameterName: " + prm.ParameterName
          + " Value: " + prm.Value );
      }

      //
      // Generate the SQL query string.
      //
      _sqlQueryString = SQL_RECORD_STATUS_REPORT_QUERY
        + " WHERE (TC_State = '" + EdRecordObjectStates.Form_Issued + "') "
        + " AND (M_Type = '" + EvMilestone.MilestoneTypes.Clinical + "') "
        + " AND (TrialId = " + PARM_TrialId + ")  "
        + " ORDER BY M_Order, MA_order, FormId; ";

      this.LogValue ( "SQL QUERY: " + _sqlQueryString );

      //return scheduleFormList;
      //
      // Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( _sqlQueryString, cmdParms ) )
      {
        this.LogValue ( "EvSqlMethods Debug: " + EvSqlMethods.Log );

        this.LogValue ( "Returned Records: " + table.Rows.Count );

        // 
        // Iterate through the results extracting the row information.
        // 
        for ( int count = 0; count < table.Rows.Count; count++ )
        {
          // 
          // Extract the table row
          // 
          DataRow row = table.Rows [ count ];

          EdRecord form = this.getScheduleFormsRowData ( row );

          /*
          this.LogValue(  "MilestoneId: " + form.MilestoneId );
          this.LogValue(  "ActivityId: " + form.ActivityId );
          this.LogValue(  "FormId: " + form.FormId 
            + " " + form.Title );
          */

          scheduleFormList.Add ( form );

        }//END record interation loop.

      }//END using statement.

      this.LogValue ( "scheduleFormList count: " + scheduleFormList.Count );

      return scheduleFormList;
    }

    #endregion

    #region FormRecord record query methods

    // =====================================================================================
    /// <summary>
    /// This record retrieves the arraylist of form record objects
    /// </summary>
    /// <param name="ProjectId">string: (Mandatory) proejct identifier.</param>
    /// <param name="FormId">string: (Mandatory) a form identifier.</param>
    /// <param name="FieldId">string: (Mandatory) a formfield identifier.</param>
    /// <param name="Value">string: (Mandatory) a formfield value.</param>
    /// <returns>ArrayList: an arrylist of EvForm object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Return an empty Arraylist if the parameters are empty. 
    /// 
    /// 2. Define the sql query parameters and sql query string. 
    /// 
    /// 3. Execute the sql query string and store the results on the data table
    /// 
    /// 4. Loop through the table and extract data row to the form object
    /// 
    /// 5. Skip the queried and withdraw records. 
    /// 
    /// 6. Add the object values to the arraylist. 
    /// 
    /// 7. Return an arraylist of Form Records. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public List<EdRecord> getRecordByFieldValue (
      String ProjectId,
      String FormId,
      String FieldId,
      String Value )
    {
      //
      // Initialize the debug log and a return arraylist.
      //
      this.LogMethod ( "getRecordByFieldValue method. " );
      this.LogValue ( "ProjectId: " + ProjectId );
      this.LogValue ( "FormId: " + FormId );
      this.LogValue ( "FieldId: " + FormId );
      this.LogValue ( "Value: " + Value );

      List<EdRecord> recordList = new List<EdRecord> ( );

      // 
      // Validate that the parameters are valid.
      // 
      if ( ProjectId == String.Empty
        || FormId == String.Empty
        || FieldId == String.Empty
        || Value == String.Empty )
      {
        return recordList;
      }

      // 
      // Define the query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( PARM_TrialId, SqlDbType.NVarChar,10),
        new SqlParameter( PARM_FormId, SqlDbType.NVarChar,10),
        new SqlParameter( PARM_FieldId, SqlDbType.NVarChar,10),
        new SqlParameter( PARM_TextValue, SqlDbType.NVarChar, 500),
      };
      cmdParms [ 0 ].Value = ProjectId;
      cmdParms [ 1 ].Value = FormId;
      cmdParms [ 2 ].Value = FieldId;
      cmdParms [ 3 ].Value = Value;

      // 
      // Generate the SQL query string.
      // 
      _sqlQueryString = _sqlQueryItemView
        + " WHERE ( ( TrialId = @TrialId ) "
        + " AND ( FormId = @FormId ) "
        + " AND ( FieldId = @FieldId ) "
        + " AND ( TRI_TextValue = @TextValue ) "
        + " ORDER BY RecordId";

      this.LogValue ( " " + _sqlQueryString );

      //
      //Execute the query against the database.
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

          // 
          // Read the result into the trial record object.
          // 
          EdRecord record = this.getItemQueryRowData ( row );

          //
          // Skip queried and withdrawn records
          //
          if (  record.State == EdRecordObjectStates.Withdrawn )
          {
            continue;
          }

          // 
          // Add the result to the arraylist.
          // 
          recordList.Add ( record );

        }//END record itertion.

      }//END Using statement

      // 
      // Return the result array.
      // 
      return recordList;

    }//END getRecordByFieldValue method.

    #endregion

    #region Retrieval methods

    // =====================================================================================
    /// <summary>
    /// This method retrieves a form object based on Guid
    /// </summary>
    /// <param name="RecordGuid">Guid: (Mandatory) Global Unique object identifier (long integer).</param>
    /// <param name="IncludeComments">bool: true = include field comments.</param>
    /// <returns>EvForm: a form data object.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Return an empty Form object if the Guid is empty. 
    /// 
    /// 2. Define the sql query parameters and sql query string. 
    /// 
    /// 3. Execute the sql query string and store the results on data table. 
    /// 
    /// 4. Loop through the table and extract the datarow to the form object. 
    /// 
    /// 5. Attach the formfield items to the form object 
    /// 
    /// 6. Return the form data object. 
    /// </remarks>
    //  ------------------------------------------------------------------------------------
    public EdRecord getRecord (
      Guid RecordGuid,
      bool IncludeComments )
    {
      this.LogMethod ( "getRecord method." );
      this.LogDebug ( "Guid: " + RecordGuid );
      this.LogDebug ( "IncludeComments: " + IncludeComments );
      this.LogDebug ( "Settings.UserProfile.RoleId: " + this.ClassParameters.UserProfile.RoleId );
      this.LogDebug ( "Settings.LoggingLevel: " + this.ClassParameters.LoggingLevel );

      //
      // Initialize the debug log, a return form object and a formfield object.
      //
      EdRecord record = new EdRecord ( );

      // 
      // Validate whether the Guid is not metpy. 
      // 
      if ( RecordGuid == Guid.Empty )
      {
        return record;
      }

      // 
      // Set the query parameter values
      // 
      SqlParameter cmdParms = new SqlParameter ( PARM_Guid, SqlDbType.UniqueIdentifier );
      cmdParms.Value = RecordGuid;

      // 
      // Generate SQL query string
      // 
      _sqlQueryString = _sqlQuery_View + " WHERE (TR_Guid = @Guid) ;";

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
          return record;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        // 
        // Fill the role object.
        // 
        record = this.getRowData ( row, false );

      }//END Using method
      /*
      if ( ( record.TypeId == EvFormRecordTypes.Informed_Consent_1
          || record.TypeId == EvFormRecordTypes.Informed_Consent_2
          || record.TypeId == EvFormRecordTypes.Informed_Consent_3
          || record.TypeId == EvFormRecordTypes.Informed_Consent_4 )
        && record.State == EvFormObjectStates.Draft_Record )
       */
      if ( record.State == EdRecordObjectStates.Draft_Record )
      {
        IncludeComments = false;
      }

      // 
      // Attach fields and other trial data.
      // 
      this.getRecordData ( record, IncludeComments );

      //
      // Update the form record section references.
      //
      this.UpdateFieldSectionReferences ( record );

      // 
      // Return the trial record.
      // 
      return record;

    }//END getRecord method

    // =====================================================================================
    /// <summary>
    /// This method updates the form field section references.
    /// </summary>
    /// <param name="FormRecord">EvForm: a form record object</param>
    // ----------------------------------------------------------------------------------
    private void UpdateFieldSectionReferences (
      EdRecord FormRecord )
    {
      this.LogMethod ( "AddStaticFields method." );
      //
      // Iterate through the form fields resetting the section references.
      //
      foreach ( EdRecordField field in FormRecord.Fields )
      {
        foreach ( EvFormSection section in FormRecord.Design.FormSections )
        {
          if (field.Design.SectionNo == section.No )
          {
            field.Design.SectionNo = section.No;
          }
        }
      }
      this.LogMethodEnd ( "AddStaticFields" );
    }//END UpdateFieldSectionReferences method.


    // =====================================================================================
    /// <summary>
    /// This class gets a form object based on the record identifier
    /// </summary>
    /// <param name="RecordId">string: (Mandatory) record identifier.</param>
    /// <param name="IncludeComments">bool: true = include field comments.</param>
    /// <returns>EvForm: a form data object.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Return an empty Form object if the RecordId is empty
    /// 
    /// 2. Define the sql query parameters and sql query string. 
    /// 
    /// 3. Execute the sql query string and store the results on data table. 
    /// 
    /// 4. Extract the first datarow to the form object. 
    /// 
    /// 5. Attach the formfield items to the form object 
    /// 
    /// 6. Return the Form data object. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EdRecord getRecord (
      String RecordId,
      bool IncludeComments )
    {
      //
      // Initialize the debug log, a return form object and a formfield object. 
      //
      this.LogMethod ( "getRecord method. " );
      this.LogValue ( "RecordId: " + RecordId );
      this.LogValue ( "IncludeComments: " + IncludeComments );
      this.LogValue ( "Settings.UserProfile.RoleId: " + this.ClassParameters.UserProfile.RoleId );

      EdRecord record = new EdRecord ( );

      // 
      // Validate whether the RecordId is not empty. 
      // 
      if ( RecordId == String.Empty )
      {
        return record;
      }

      // 
      // Define the parameters for the query
      // 
      SqlParameter cmdParms = new SqlParameter ( PARM_RecordId, SqlDbType.NVarChar, 20 );
      cmdParms.Value = RecordId;

      _sqlQueryString = _sqlQuery_View + " WHERE ( RecordId = @RecordId );";

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
          return record;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        // 
        // Fill the role object.
        // 
        record = this.getRowData ( row, true );

      }//END Using method

      // 
      // Attach fields and other trial data.
      // 
      this.getRecordData ( record, IncludeComments );

      //
      // Update the form record section references.
      //
      this.UpdateFieldSectionReferences ( record );

      // 
      // Return the trial record.
      // 
      return record;

    }//END getItem method

    // =====================================================================================
    /// <summary>
    /// This class gets a form object based on the record identifier
    /// </summary>
    /// <param name="SourceId">string: (Mandatory) record identifier.</param>
    /// <param name="IncludeComments">bool: true = include field comments.</param>
    /// <returns>EvForm: a form data object.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Return an empty Form object if the RecordId is empty
    /// 
    /// 2. Define the sql query parameters and sql query string. 
    /// 
    /// 3. Execute the sql query string and store the results on data table. 
    /// 
    /// 4. Extract the first datarow to the form object. 
    /// 
    /// 5. Attach the formfield items to the form object 
    /// 
    /// 6. Return the Form data object. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EdRecord getRecordBySource (
      String SourceId,
      bool IncludeComments )
    {
      //
      // Initialize the debug log, a return form object and a formfield object. 
      //
      this.LogMethod ( "getRecord method. " );
      this.LogValue ( "SourceId: " + SourceId );
      this.LogValue ( "IncludeComments: " + IncludeComments );
      this.LogValue ( "Settings.UserProfile.RoleId: " + this.ClassParameters.UserProfile.RoleId );

      EdRecord record = new EdRecord ( );

      // 
      // Validate whether the RecordId is not empty. 
      // 
      if ( SourceId == String.Empty )
      {
        return record;
      }

      // 
      // Define the parameters for the query
      // 
      SqlParameter cmdParms = new SqlParameter ( PARM_SourceId, SqlDbType.NVarChar, 20 );
      cmdParms.Value = SourceId;

      _sqlQueryString = _sqlQuery_View + " WHERE ( SourceId = " + PARM_SourceId + " );";

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
          return record;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        // 
        // Fill the role object.
        // 
        record = this.getRowData ( row, true );

      }//END Using method

      // 
      // Attach fields and other trial data.
      // 
      this.getRecordData ( record, IncludeComments );

      //
      // Update the form record section references.
      //
      this.UpdateFieldSectionReferences ( record );

      // 
      // Return the trial record.
      // 
      return record;

    }//END getItem method

    // =====================================================================================
    /// <summary>
    /// This class gets a record object using RecordId and the form state. 
    /// </summary>
    /// <param name="RecordId">string: (Mandatory) record identifier.</param>
    /// <param name="State">string: (Mandatory) Subject state.</param>
    /// <returns>EvForm: a form data object.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Return an empty Form object if the RecordId is empty.
    /// 
    /// 2. Define the sql query parameters and sql query string. 
    /// 
    /// 3. Execute the sql query string and store the results on data table. 
    /// 
    /// 4. Extract the first datarow to the form object. 
    /// 
    /// 5. Attach the formfield items to the form object 
    /// 
    /// 6. Return the form data object. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EdRecord getRecord ( string RecordId, string State )
    {
      //
      // Initialize the debug log, a return form object and a formfield object. 
      //
      this.LogMethod ( "getRecord method. " );
      this.LogValue ( "RecordId: " + RecordId );
      this.LogValue ( "Settings.UserProfile.RoleId: " + this.ClassParameters.UserProfile.RoleId );

      EdRecord record = new EdRecord ( );

      // 
      // TestReport that the data object has a valid record identifier.
      // 
      if ( RecordId == String.Empty )
      {
        return record;
      }

      // 
      // Define the parameters for the query
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(PARM_RecordId, SqlDbType.NVarChar, 20),
        new SqlParameter(PARM_State, SqlDbType.Char, 5),
      };
      cmdParms [ 0 ].Value = RecordId;
      cmdParms [ 1 ].Value = State;

      _sqlQueryString = _sqlQuery_View + " WHERE ( RecordId = @RecordId )"
        + " AND ( TR_State = @State );";

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
          return record;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        // 
        // Fill the role object.
        // 
        record = this.getRowData ( row, false );

      }//END Using method

      // 
      // Attach fields and other trial data.
      // 
      this.getRecordData ( record, true );

      //
      // Update the form record section references.
      //
      this.UpdateFieldSectionReferences ( record );

      // 
      // Return the trial record.
      // 
      return record;

    }//END getRecord method

    // =====================================================================================
    /// <summary>
    /// This method attaches the other trial data to the record that is needed for the record 
    ///  to be updated.
    /// This data is only to be attached if the record state is editable.  As newField validation
    ///  is not necessary in any other state.
    /// </summary>
    /// <param name="Record">EvForm: (Mandatory) a form data object.</param>
    /// <param name="IncludeComments">bool: true = include field comments.</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Get the form record fields 
    /// 
    /// 2. Attach the trial data and milestone data to the form record object. 
    /// 
    /// 3. If the record is in an editable state attach the 
    /// other trial data for record validation
    /// 
    /// 4. If the milestone object exists set the prior to attribute.
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private void getRecordData (
      EdRecord Record,
      bool IncludeComments )
    {
      // 
      // Initialise the methods variables and objects.
      // 
      this.LogMethod ( "getRecordData." );
      this.LogDebug ( "IncludeComments: " + IncludeComments );
      this.LogDebug ( "State: " + Record.StateDesc );
      this.LogDebug ( "ProjectId: " + Record.ApplicationId );
      EdApplications projects = new EdApplications ( this.ClassParameters );

      // 
      // Get the trial record fields
      // 
      this.LogValue ( "RecordFields.Settings.LoggingLevel: " + this._Dal_FormRecordFields.ClassParameters.LoggingLevel );
      Record.Fields = this._Dal_FormRecordFields.getRecordFieldList ( Record, IncludeComments );
      this.LogClass ( this._Dal_FormRecordFields.Log );
      this.LogValue ( "Field count: " + Record.Fields.Count );
      this.LogMethodEnd ( "getRecordData" );

    }//END getRecordData method

    #endregion

    #region Form Record Update queries

    // =====================================================================================
    /// <summary>
    /// This method adds the EvRecord object to the database
    /// </summary>
    /// <param name="Record">EvForm: a form data object</param>
    /// <returns>EvForm: a new form data object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Create new DB row GUID, if the record's Guid is empty.
    /// 
    /// 3. Define the sql query parameters and execute the storeprocedure for creating new items. 
    /// 
    /// 4. Return the form data object. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EdRecord createRecord ( EdRecord Record )
    {
      this.LogMethod ( "createRecord, " );
      this.LogDebug ( "TrialId: " + Record.ApplicationId );
      this.LogDebug ( "MilestoneId: " + Record.MilestoneId );
      this.LogDebug ( "ActivityId: " + Record.ActivityId );
      this.LogDebug ( "IsMandatory: " + Record.IsMandatoryActivity );
      this.LogDebug ( "FormId: " + Record.LayoutId );

      //
      // Initialize the debug log and return form object. 
      //
      EdRecord newRecord = new EdRecord ( );

      // 
      // Create the GUID for the database
      // 
      if ( Record.Guid == Guid.Empty )
      {
        Record.Guid = Guid.NewGuid ( );
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = GetCreateParameters ( );
      SetCreateParameters ( cmdParms, Record );

      //
      // Execute the update command.
      //
      EvSqlMethods.StoreProcUpdate ( STORED_PROCEDURE_CreateItem, cmdParms );

      // 
      // Return unique identifier of the new data object.
      // 
      var record = this.getRecord ( Record.Guid, true );

      this.LogMethodEnd ( "createRecord" );

      return record;
    } //END createRecord method.

    // =====================================================================================
    /// <summary>
    /// This method creates a new record with the fields prefilled from the preious instance
    ///  of the record.
    /// </summary>
    /// <param name="Record">EvForm: a form object</param>
    /// <returns>EvForm: a form object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Get a list of the patient records to find the last instance of the form type.
    /// 
    /// 2. Create a new instance of the record.
    /// 
    /// 3. Update the new record with the previous record's value.
    /// 
    /// 4. Return the new form record object. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EdRecord createNewUpdateableRecord ( EdRecord Record )
    {
      //
      // Initialize the method debug log and the new form object. 
      //
      this.LogMethod ( "createNewUpdateableRecord method. "
        + "\r\n TrialId: " + Record.ApplicationId
        + ", MilestoneId: " + Record.MilestoneId
        + ", ActivityId: " + Record.ActivityId
        + ", IsMandatory: " + Record.IsMandatoryActivity
        + ", FormId: " + Record.LayoutId );

      EdRecord newRecord = new EdRecord ( );

      //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      // STEP I - Get a list of the patient records to find the last instance of the form type.
      this.LogValue ( " STEP I GET LIST OF PREVIOUS INSTANCES OF THE FORM. " );

      //
      // Define the sql query parameters and load the values.
      //
      EvQueryParameters query = new EvQueryParameters ( Record.ApplicationId );
      query.FormId = Record.LayoutId;
      query.MilestoneId = Record.MilestoneId;
      query.State = EdRecordObjectStates.Draft_Record.ToString ( )
        + ";" + EdRecordObjectStates.Submitted_Record.ToString ( );
      query.NotSelectedState = false;
      query.OrderBy = "TR_RecordDate";
      query.IncludeRecordFields = true;

      //
      // Query the milestone trial record to get a list of previous records for this patient.
      //
      List<EdRecord> recordList = this.getRecordList ( query );

      this.LogValue ( " Record instance count: " + recordList.Count );

      //
      // debug
      //
      for ( int i = 0; i < recordList.Count; i++ )
      {
        this.LogValue ( " " + i + ", RecordId: " + recordList [ i ].RecordId + " Date: " + recordList [ i ].RecordDate );
      }

      //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      // STEP II - Create a new instance of the record.
      //
      this.LogValue ( " STEP II CREATE NEW RECORD. \r\n" );

      newRecord = this.createRecord ( Record );

      this.LogValue ( " New RecordId: " + Record.RecordId );

      //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      // STEP III - Update the new record with the previous record's value.
      //
      this.LogValue ( " STEP III UPDATE NEW RECORD FIELD VALUES " );

      //
      // Get the last chronological record and prefill the new record.
      //
      if ( recordList.Count > 0 )
      {
        //
        // Retrieve the last record in the list.
        //
        EdRecord oldRecord = recordList [ recordList.Count - 1 ];

        //
        // Update the new record's value with those of the previous record.
        //
        this.updateNewRecordValues ( newRecord, oldRecord );

        //
        // Update the record if the fields have been updated.
        //
        this.updateRecord ( newRecord );

      }//END record list exist.

      //
      // Return the new record
      //
      return newRecord;

    } //END createNewUpdateableRecord method.

    // =====================================================================================
    /// <summary>
    /// This method updates the values in the new record with the values from the previous 
    ///  record of this type collected for the currently selected patient.
    /// </summary>
    /// <param name="RecordList">List of EvForm: a list of existing record object.</param>
    /// <param name="NewRecord">EvForm: a form object containing the new record object.</param>
    /// <returns>EvForm: the last form record object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Iterate through the list of records to retrieve the last record that matches
    /// the form identifier.
    /// 
    /// 2. Return the form data object.
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private EdRecord getLastRecord (
      List<EdRecord> RecordList,
      EdRecord NewRecord )
    {
      //
      // Initialise the last form record object
      //
      EdRecord lastRecord = new EdRecord ( );

      // 
      // Iterate through the list of records to retrieve the last record that matches
      // the form identifier.
      //
      foreach ( EdRecord record in RecordList )
      {
        if ( record.LayoutId == NewRecord.LayoutId )
        {
          lastRecord = record;
        }

      }//END record list iteration loop

      //
      // Return the retrieve record.
      //
      return lastRecord;

    }//END getLastRecord  method

    // =====================================================================================
    /// <summary>
    /// This method returns true if the values in the new record is updated with the values from the previous 
    ///  record of this type collected for the currently selected patient.
    /// </summary>
    /// <param name="NewRecord">EvForm: a form object containing the new record object.</param>
    /// <param name="OldRecord">EvForm: a form object containing the old record object.</param>
    /// <returns>Boolean: true, if the record is updated with new values.</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Return false if the new record object is empty.
    /// 
    /// 2. Iterate through the new records fields and update the fields values from the
    /// historical records.
    /// 
    /// 3. Return true if the new record object is updated.
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private bool updateNewRecordValues (
      EdRecord NewRecord,
      EdRecord OldRecord )
    {
      //
      // Initialize the method debug log
      //
      this.LogMethod ( "updateNewRecordValues method. "
        + " old FormId: " + OldRecord.LayoutId
        + " old RecordId: " + OldRecord.RecordId
        + " new FormId: " + NewRecord.LayoutId
        + " new RecordId: " + NewRecord.RecordId );

      //
      // Validate whether the new record object is not empty.
      //
      if ( NewRecord.Fields.Count == 0 )
      {
        return false;
      }

      //
      // Iterate through the new records fields and initialise the fields that have 
      // historical values.
      //
      foreach ( EdRecordField field in NewRecord.Fields )
      {
        this.LogValue ( " FieldId: " + field.FieldId );

        //
        // Get the field from the old record to retrieve its values.
        //
        EdRecordField oldField = this.getRecordField ( OldRecord, field.FieldId );

        //
        // If the oldfield exists,i.e. it has a non empty Guid 
        // Initialise the new field with its value.
        //
        if ( oldField.Guid != Guid.Empty )
        {
          this.LogValue ( " >> Field value updated" );

          //
          // Update the field data values.
          //
          field.ItemText = oldField.ItemText;
          field.ItemValue = oldField.ItemValue;

          //
          // Process table cell data of a table or matrix field type.
          //
          if ( field.TypeId == Evado.Model.EvDataTypes.Table
            || field.TypeId == Evado.Model.EvDataTypes.Special_Matrix )
          {
            this.LogValue ( " >> Update Table" );

            this.updateTableValues ( field, oldField );

          }//END table or matrix field type

        }//END old field exists update new field.

      }//END field iteration loop

      return true;

    }//END updateNewRecordValues method

    // =====================================================================================
    /// <summary>
    /// This method updates the values in the new record with the values from the previous 
    ///  record of this type collected for the currently selected patient.
    /// </summary>
    /// <param name="NewRecordField">EvForm: a form record object containing the new record object.</param>
    /// <param name="OldRecordField">EvForm: a form record object containing the old record object.</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Validate whether both new and old form record objects have items.
    /// 
    /// 2. Iterate through the rows in the table for updating the value in the new record.
    /// 
    /// 3. Iterate through the row columns updating the cell values using the 
    /// the column header text as the key
    /// 
    /// 4. Retrieve the value for the current row and column from the previous record
    /// 
    /// 5. Update the table cell value in the new record.
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private void updateTableValues (
      EdRecordField NewRecordField,
      EdRecordField OldRecordField )
    {
      //
      // Validate whether both new and old form record objects have items.
      //
      if ( NewRecordField.Table != null && OldRecordField.Table != null )
      {
        this.LogValue ( " >> new table exists" );
        if ( OldRecordField.Table != null )
        {
          this.LogValue ( " >> old table exists" );

          //
          // Iterate through the rows in the table for updating the value in the new record.
          //
          for ( int iRow = 0; iRow < NewRecordField.Table.Rows.Count; iRow++ )
          {
            //
            // Iterate through the row columns updating the cell values using the 
            // the column header text as the key.
            //
            for ( int iColumn = 0; iColumn < NewRecordField.Table.Header.Length; iColumn++ )
            {
              //
              // Extract the column header from the new record.
              //
              EdRecordTableHeader columnHeader = NewRecordField.Table.Header [ iColumn ];

              //
              // Retrieve the value for the current row and column from the previous record.
              //
              String cellValue = this.getTableCellData ( OldRecordField.Table, iRow, columnHeader );

              // 
              // Update the table cell value in the new record.
              //
              NewRecordField.Table.Rows [ iRow ].Column [ iColumn ] = cellValue;

            }//END field column iteration loop

          }//END table row iteration loop
        }
      }//END table object exist in both new and old field.
    }//END updateTableValues class

    // =====================================================================================
    /// <summary>
    ///  This method get the value of a table column.
    ///  The iteration process is designed to cater for the scenario where a tables columns
    ///  have been repositioned. i.e. a column has been added in the middle of the table.
    /// </summary>
    /// <param name="OldFieldTable">EvFormFieldTable: a formfield table containing the rows of data to be retrieved.</param>
    /// <param name="SelectedRow">Integer: The row count in the table.</param>
    /// <param name="ColumnHeader">EvFormFieldTableColumnHeader: a column header object containing the new fields table column data.</param>
    /// <returns>String: a table cell value string</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Iterate through the column header 
    /// 
    /// 2. Return the column value if the column header exists
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private String getTableCellData (
      EdRecordTable OldFieldTable,
      int SelectedRow,
      EdRecordTableHeader ColumnHeader )
    {
      //
      // iterate through the columns of the table looking for a column header that matches the 
      // passed column header text and return the column value and data type.
      //
      for ( int iColumn = 0; iColumn < OldFieldTable.Header.Length; iColumn++ )
      {
        EdRecordTableHeader header = OldFieldTable.Header [ iColumn ];

        if ( header.Text == ColumnHeader.Text
          && header.TypeId == ColumnHeader.TypeId )
        {
          return OldFieldTable.Rows [ SelectedRow ].Column [ iColumn ];
        }
      }

      return String.Empty;

    }//END getTableCellData method

    // =====================================================================================
    /// <summary>
    ///  This class matches the form field using fieldId. 
    /// </summary>
    /// <param name="Record">EvForm: a form object containing the record object..</param>
    /// <param name="FieldId">String: The string value of a form field identifier.</param>
    /// <returns>EvFormField: a formfield object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Iterate through the form record object 
    /// 
    /// 2. Return the retrieve formfield object if formId exists.
    /// 
    /// 3. Else return new formfield object.
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private EdRecordField getRecordField ( EdRecord Record, String FieldId )
    {
      //
      // iterate through the list of fields to retrieve the matching field by it identifier.
      //
      foreach ( EdRecordField field in Record.Fields )
      {
        if ( field.FieldId == FieldId )
        {
          return field;
        }

      }//END field iteration loop

      return new EdRecordField ( );

    }//END getRecorField method

    // =====================================================================================
    /// <summary>
    /// This method updates the form table with the record data object.
    /// </summary>
    /// <param name="Record">EvForm: a form data object</param>
    /// <returns>EvEventCodes: an event code for updating form record object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit if the old record's Guid is empty
    /// 
    /// 2. Define the sql query parameters and execute the storeprocedure. 
    /// 
    /// 3. Update the formfield items
    /// 
    /// 4. Add changed items to datachange table. 
    /// 
    /// 5. Add commentlist to the record table. 
    /// 
    /// 6. Return the event code for updating items. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvEventCodes updateRecord ( EdRecord Record )
    {
      //
      // Initialize the method debug log, internal variables and objects. 
      //
      this.LogMethod ( "updateRecord, " );
      this.LogDebug ( "Settings.UserProfile.RoleId: " + this.ClassParameters.UserProfile.RoleId );
      this.LogDebug ( "Guid: " + Record.Guid );
      this.LogDebug ( "RecordId: " + Record.RecordId );
      this.LogDebug ( "FormGuid: " + Record.LayoutGuid );
      this.LogDebug ( "ProjectId: " + Record.ApplicationId ); ;
      this.LogDebug ( "State: " + Record.State );

      int databaseRecordAffected = 0;
      EvDataChanges dataChanges = new EvDataChanges ( );
      EdRecordComments recordComments = new EdRecordComments ( );
      EvEventCodes eventCode = EvEventCodes.Ok;

      //
      // Set the skip retrieve comments to true to not retrieve comments
      // when validating the record object for saving.
      //
      this._SkipRetrievingComments = true;

      // 
      // Validate whether the record object exists.
      // 
      EdRecord oldRecord = this.getRecord ( Record.RecordId, true );
      if ( oldRecord.Guid == Guid.Empty )
      {
        return EvEventCodes.Identifier_Record_Id_Error;
      }

      // 
      // Set the data change object values.
      // 
      EvDataChange dataChange = this.setChangeRecord ( Record, oldRecord );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = GetParameters ( );
      SetParameters ( cmdParms, Record );

      //
      // Execute the update command.
      //
      databaseRecordAffected = EvSqlMethods.StoreProcUpdate ( STORED_PROCEDURE_UpdateItem, cmdParms );
      if ( databaseRecordAffected == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      // 
      // Update the formfield items
      // 
      this.LogValue ( "Record.Fields.Count: " + Record.Fields.Count );
      if ( Record.Fields.Count > 0 )
      {
        EvEventCodes iReturn = this._Dal_FormRecordFields.UpdateFields ( Record );
        this.LogValue ( "Saving Form Fields" + this._Dal_FormRecordFields.Log );
      }

      // 
      // Add the data change values to the database.
      // 
      dataChanges.AddItem ( dataChange );
      this.LogValue ( "DataChange status: " + dataChanges.Log );

      //
      // Add record comments.
      //
      eventCode = recordComments.addNewComments ( Record.CommentList );
      this.LogValue ( "RecordComment Log: " + recordComments.Log );

      // 
      // Return event exit code.
      // 
      return eventCode;

    } //END updateRecord method.

    #endregion

    #region Update Difference methods

    // =====================================================================================
    /// <summary>
    /// This class sets the data change object for the update action.
    /// </summary>
    /// <param name="NewRecord">EvForm: a form object of new record version</param>
    /// <param name="OldRecord">EvForm: a form object of existing record data</param>
    /// <returns>EvDataChange: a datachange object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Add new items values to datachange object if they do not exist on the current form table. 
    /// 
    /// 2. Add new items values to the datachange formfield object. 
    /// 
    /// 3. Return the datachange object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvDataChange setChangeRecord ( EdRecord NewRecord, EdRecord OldRecord )
    {
      this.LogMethod ( "setChangeRecord method " );
      // 
      // Initialise the datachange object.
      // 
      EvDataChange dataChange = new EvDataChange ( );

      // 
      // Set the date change object values.
      //       
      dataChange.Guid = Guid.NewGuid ( );
      dataChange.TableName = EvDataChange.DataChangeTableNames.EvRecords;
      dataChange.TrialId = NewRecord.ApplicationId;
      dataChange.RecordId = NewRecord.RecordId;
      dataChange.RecordGuid = NewRecord.Guid;
      dataChange.UserId = this.ClassParameters.UserProfile.UserId;
      dataChange.DateStamp = DateTime.Now;

      //
      // Add items values to the datachange object if they do not exist. 
      //
      dataChange.AddItem ( "TrialId", OldRecord.ApplicationId, NewRecord.ApplicationId );

      dataChange.AddItem ( "RecordDate", OldRecord.RecordDate, NewRecord.RecordDate );

      dataChange.AddItem ( "State", OldRecord.State.ToString ( ), NewRecord.State.ToString ( ) );

      //
      // Iterate through the record content comments.
      //
      if ( OldRecord.CommentList.Count < NewRecord.CommentList.Count )
      {
        //
        // Iterate through the record content comments.
        //
        for ( int count = 0; count < NewRecord.CommentList.Count; count++ )
        {
          if ( count < OldRecord.CommentList.Count )
          {
            AddCommentToDataChange (
              "Comment_",
              dataChange,
              count,
              OldRecord.CommentList [ count ],
              NewRecord.CommentList [ count ] );
          }
          else
          {
            AddCommentToDataChange (
              "Comment_",
              dataChange,
              count, new EvFormRecordComment ( ),
              NewRecord.CommentList [ count ] );
          }
        }
      }

      //
      // Iterate through the signoffs.
      //
      for ( int count = 0; count < NewRecord.Signoffs.Count; count++ )
      {
        if ( OldRecord.Signoffs.Count <= count )
        {
          this.AddSignoffToDataChange (
            dataChange,
            count,
            new EvUserSignoff ( ),
            NewRecord.Signoffs [ count ] );
        }
        else
        {
          this.AddSignoffToDataChange (
            dataChange,
            count,
             OldRecord.Signoffs [ count ],
            NewRecord.Signoffs [ count ] );
        }
      }//END annotation 


      // 
      // Append the record new formField values.
      // 
      if ( NewRecord.Fields.Count > 0 )
      {
        this.LogValue ( " Updating field differences." );

        // 
        // Iterate through the form fields outputting the main newField values.
        // 
        foreach ( EdRecordField newField in NewRecord.Fields )
        {
          EdRecordField oldField = getRecordField ( OldRecord.Fields, newField.FieldId );

          // 
          // Set the form Field for difference data.
          // 
          this.setChangeRecordField ( dataChange, oldField, newField );

        }//END fields not null.

        this.LogValue ( " Data change processing of form fields" );

      }//END record has fields.

      // 
      // Return the data change object.
      // 
      return dataChange;

    }//END setChangeRecord method

    //===================================================================================
    /// <summary>
    /// This method add a comment to the data change list.
    /// </summary>
    /// <param name="Prefix">String: the data change prefix identifier</param>
    /// <param name="dataChange">EvDataChange object</param>
    /// <param name="count">int: the count of the change.</param>
    /// <param name="OldComment">EvFormRecordComment: old comment object</param>
    /// <param name="NewComment">EvFormRecordComment: new comment object</param>
    //---------------------------------------------------------------------------------
    private void AddCommentToDataChange (
      String Prefix,
      EvDataChange dataChange,
      int count,
     EvFormRecordComment OldComment,
      EvFormRecordComment NewComment )
    {

      dataChange.AddItem ( Prefix + "_AuthorType_" + count,
        OldComment.AuthorType,
        NewComment.AuthorType );

      dataChange.AddItem ( Prefix + "_CommentDate_" + count,
        OldComment.CommentDate,
        OldComment.CommentDate );

      dataChange.AddItem ( Prefix + "_CommentType_" + count,
        OldComment.CommentType,
       NewComment.CommentType );

      dataChange.AddItem ( Prefix + "_Content_" + count,
        OldComment.Content,
        NewComment.Content );

      dataChange.AddItem ( Prefix + "_NewComment_" + count,
        OldComment.NewComment,
        NewComment.NewComment );

      dataChange.AddItem ( Prefix + "_UserId_" + count,
        OldComment.UserId,
        NewComment.UserId );

      dataChange.AddItem ( Prefix + "_UserCommonName_" + count,
       OldComment.UserCommonName,
       NewComment.UserCommonName );

    }//End AddCommentToDataChange method


    //===================================================================================
    /// <summary>
    /// This method add a comment to the data change list.
    /// </summary>
    /// <param name="dataChange">EvDataChange object</param>
    /// <param name="count">int: the count of the change.</param>
    /// <param name="OldRecord">EvUserSignoff: old comment object</param>
    /// <param name="NewRecord">EvUserSignoff: new comment object</param>
    //---------------------------------------------------------------------------------
    private void AddSignoffToDataChange (
      EvDataChange dataChange,
      int count,
      EvUserSignoff OldRecord,
      EvUserSignoff NewRecord )
    {
      dataChange.AddItem ( "Signoff_Description_" + count,
        OldRecord.Description,
        NewRecord.Description );

      dataChange.AddItem ( "Signoff_SignedOffBy_" + count,
        OldRecord.SignedOffBy,
        NewRecord.SignedOffBy );

      dataChange.AddItem ( "Signoff_SignedOffUserId_" + count,
        OldRecord.SignedOffUserId,
        NewRecord.SignedOffUserId );

      dataChange.AddItem ( "Signoff_SignOffDate_" + count,
       OldRecord.SignOffDate,
        NewRecord.SignOffDate );

      dataChange.AddItem ( "Signoff_Type_" + count,
        OldRecord.Type,
        NewRecord.Type );
    }

    // =====================================================================================
    /// <summary>
    /// This method returns the selected EvFormField for a newField identifier.
    /// </summary>
    /// <param name="FieldList">List of EvFormField: a list of formfield object items</param>
    /// <param name="FieldId">String: a field identifier</param>
    /// <returns>EvFormField: a formfield object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Loop through the formfield object list. 
    /// 
    /// 2. Return the retrieving formfield if it exist. 
    /// 
    /// 3. Else return new formfield object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private EdRecordField getRecordField ( List<EdRecordField> FieldList, String FieldId )
    {
      foreach ( EdRecordField field in FieldList )
      {
        // 
        // Return the matching newField.
        // 
        if ( field.FieldId == FieldId )
        {
          return field;
        }
      }
      // 
      // IF none return empty object.
      // 
      return new EdRecordField ( );
    }//END getRecordField class

    // =====================================================================================
    /// <summary>
    /// This class sets the data change object for the record fields action.
    /// </summary>
    /// <param name="DataChange">EvDataChange: a datachange object</param>
    /// <param name="NewField">EvFormField: a formfield object of new record version</param>
    /// <param name="OldField">EvFormField: a formfield object of existing record data</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Add new items to formfield table if they do not exist. 
    /// 
    /// 2. Add comment to the formfield table. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private void setChangeRecordField (
      EvDataChange DataChange,
      EdRecordField OldField,
      EdRecordField NewField )
    {
      //
      // Initialize the debug log
      //
      this.LogValue ( " FieldId: old: " + OldField.FieldId + " new: " + NewField.FieldId );

      // 
      // Add new formfield value item if it does not exist.
      // 
      if ( OldField.ItemValue != NewField.ItemValue )
      {
        DataChange.AddItem (
           NewField.FieldId.Replace ( " ", "_" ) + "_Value",
           OldField.ItemValue,
           NewField.ItemValue );
      }

      // Add new formfield text item if it does not exist.
      if ( OldField.ItemText != NewField.ItemText )
      {
        DataChange.AddItem (
          NewField.FieldId.Replace ( " ", "_" ) + "_ValueText",
          OldField.ItemText,
          NewField.ItemText );
      }

    }//END setChangeRecordField method


    #endregion

    #region Lock Records methods

    // =====================================================================================
    /// <summary>
    /// This class locks the trial record for single user update.
    /// </summary>
    /// <param name="Record">EvForm: a form object</param>
    /// <returns>EvEventCodes: an event code for locking record</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the Record's Guid is empty 
    /// 
    /// 2. Define the sql query parameters and execute the storeprocedure for locking the record.
    /// 
    /// 3. Return the event code for locking records. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvEventCodes lockRecord ( EdRecord Record )
    {
      // 
      // Initialise the method debug log and the number of updated records.
      // 
      this.LogMethod ( "lockRecord method " );
      this.LogValue ( "Guid: " + Record.Guid );
      int RecordsUpdated = 0;

      // 
      // Validate whether the record Guid does not exist. 
      // 
      if ( Record.Guid == Guid.Empty )
      {
        return EvEventCodes.Identifier_Global_Unique_Identifier_Error;
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(PARM_Guid, SqlDbType.UniqueIdentifier),
        new SqlParameter(PARM_UpdatedByUserId, SqlDbType.NVarChar, 100),
        new SqlParameter(PARM_UpdatedBy, SqlDbType.NVarChar, 100),
        new SqlParameter(PARM_UpdateDate, SqlDbType.DateTime)
      };
      cmdParms [ 0 ].Value = Record.Guid;
      cmdParms [ 1 ].Value = this.ClassParameters.UserProfile.UserId;
      cmdParms [ 2 ].Value = this.ClassParameters.UserProfile.CommonName;
      cmdParms [ 3 ].Value = DateTime.Now;

      //
      // Execute the update command.
      //
      if ( ( RecordsUpdated = EvSqlMethods.StoreProcUpdate ( STORED_PROCEDURE_LockItem, cmdParms ) ) == 0 )
      {
        this.LogValue ( " RecordsUpdated " + RecordsUpdated + " " );
        return EvEventCodes.Database_Record_UnLock_Error;
      }

      this.LogValue ( "RecordsUpdated " + RecordsUpdated + " " );
      // 
      // Return successful update state.
      // 
      return EvEventCodes.Ok;

    } //END lockItem method.

    // =====================================================================================
    /// <summary>
    /// This class unlocks the the trial record.
    /// </summary>
    /// <param name="Item">EvForm: a form object</param>
    /// <returns>EvEventCodes: an event code for unlocking records.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the Form's Guid is empty 
    /// 
    /// 2. Define the sql query parameters and execute the storeprocedure for unlocking the record.
    /// 
    /// 3. Return the event code for locking record. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvEventCodes unlockRecord ( EdRecord Item )
    {
      // 
      // Initialise the method debug log and the number of updated records.
      // 
      this.LogMethod ( "unlockRecord method " );
      this.LogValue ( "Guid: " + Item.Guid );
      int RecordsUpdated = 0;

      // 
      // Check that the data object has valid identifiers to add it to the database.
      // 
      if ( Item.Guid == Guid.Empty )
      {
        return EvEventCodes.Identifier_Global_Unique_Identifier_Error;
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(PARM_Guid, SqlDbType.UniqueIdentifier),
        new SqlParameter(PARM_UpdatedByUserId, SqlDbType.NVarChar, 100),
        new SqlParameter(PARM_UpdatedBy, SqlDbType.NVarChar, 100),
        new SqlParameter(PARM_UpdateDate, SqlDbType.DateTime),
        new SqlParameter(PARM_BookedOut, SqlDbType.NVarChar, 100),
      };
      cmdParms [ 0 ].Value = Item.Guid;
      cmdParms [ 1 ].Value = this.ClassParameters.UserProfile.UserId;
      cmdParms [ 2 ].Value = this.ClassParameters.UserProfile.CommonName;
      cmdParms [ 3 ].Value = DateTime.Now;
      cmdParms [ 4 ].Value = Item.BookedOutBy;

      //
      // Execute the update command.
      //
      if ( ( RecordsUpdated = EvSqlMethods.StoreProcUpdate ( STORED_PROCEDURE_UnlockItem, cmdParms ) ) == 0 )
      {
        this.LogValue ( " RecordsUpdated " + RecordsUpdated + " " );
        return EvEventCodes.Database_Record_UnLock_Error;
      }
      this.LogValue ( " RecordsUpdated " + RecordsUpdated + " " );

      // 
      // Return successful update state.
      // 
      return EvEventCodes.Ok;

    } //END unlockRecord method.

    #endregion

    /// <summary>
    /// This class is handles the data access layer for the form records data object.
    /// </summary>
    public class sortOndate : IComparer<EvFormRecordComment>
    {
      /// <summary>
      /// This method sorts the list busing the ICompare interface
      /// </summary>
      /// <param name="a">EvFormRecordComment object</param>
      /// <param name="b">EvFormRecordComment object</param>
      /// <returns></returns>
      public int Compare ( EvFormRecordComment a, EvFormRecordComment b )
      {
        if ( a.CommentDate > b.CommentDate ) return 1;
        else if ( a.CommentDate < b.CommentDate ) return -1;
        else return 0;
      }
    }

  }//END EvFormRecords class

}//END namespace Evado.Dal.Clinical
