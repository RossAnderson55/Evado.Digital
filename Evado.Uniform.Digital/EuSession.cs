﻿/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\SessionObject.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2020 EVADO HOLDING PTY. LTD..  All rights reserved.
 *     
 *      The use and distribution terms for this software are contained in the file
 *      named \license.txt, which can be found in the root of this distribution.
 *      By using this software in any fashion, you are agreeing to be bound by the
 *      terms of this license.
 *     
 *      You must not remove this notice, or any other, from this software.
 *     
 * </copyright>
 * 
 * Description: 
 *  This class contains the AbstractedPage ResultData object.
 *
 ****************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Evado.Bll;
using Evado.Model;
using Evado.Bll.Clinical;
using Evado.Model.Digital;
// using Evado.Web;

namespace Evado.UniForm.Clinical
{
  /// <summary>
  /// This class contains the session ResultData object
  /// </summary>
  public class EuSession
  {
    #region global variables and consts

    public const String CONST_MENU_WEB_SITE_ID = "WebSiteId";
    public const String CONST_MENU_GROUP_ID = "GroupId";
    public const String CONST_PLATFORM_FIELD_ID = "PLATFORM";
    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Client properties
    private bool _AnonymousPageAccess = false;
    /// <summary>
    /// This property records whether the client is anonymous page access mode.
    /// </summary>
    public bool AnonymousPageAccess
    {
      get { return this._AnonymousPageAccess; }
      set { this._AnonymousPageAccess = value; }
    }

    private bool _PaperConsentPageAccess = false;
    /// <summary>
    /// This property records whether the client is paper consent page access mode.
    /// </summary>
    public bool WetInkConsentAccess
    {
      get { return this._PaperConsentPageAccess; }
      set { this._PaperConsentPageAccess = value; }
    }

    #endregion

    #region Application properties members


    public bool AdsEnabled = true;
    /// <summary>
    /// this property contains a list of ADS customer groups.
    /// </summary>
    public Evado.ActiveDirectoryServices.EvAdsGroupProfile AdsCustomerGroup { get; set; }

    /// <summary>
    /// THis property contains the list of event log user selection list 
    /// </summary>
    public List<EvOption> EventUserSelectionList { get; set; }

    private EvEventCodes _EventId = EvEventCodes.Ok;

    /// <summary>
    /// This property contains the application event selection event id
    /// </summary>
    public EvEventCodes EventId
    {
      get { return _EventId; }
      set { _EventId = value; }
    }

    private DateTime _EventStartDate = Evado.Model.Digital.EvcStatics.CONST_DATE_MIN_RANGE;

    /// <summary>
    /// This property contains the application event selection start date
    /// </summary>
    public DateTime EventStartDate
    {
      get { return _EventStartDate; }
      set { _EventStartDate = value; }
    }

    private DateTime _EventFinishDate = Evado.Model.Digital.EvcStatics.CONST_DATE_MAX_RANGE;
    /// <summary>
    /// This property contains the application event selection finish date.
    /// </summary>
    public DateTime EventFinishDate
    {
      get { return _EventFinishDate; }
      set { _EventFinishDate = value; }
    }

    private String _EventType = String.Empty;
    /// <summary>
    /// This property contains the application event selection event type value.
    /// </summary>
    public String EventType
    {
      get { return _EventType; }
      set { _EventType = value; }
    }

    private String _EventUserName = String.Empty;

    /// <summary>
    /// This property contains the application event selection user name.
    /// </summary>
    public String EventUserName
    {
      get { return _EventUserName; }
      set { _EventUserName = value; }
    }

    /// <summary>
    /// This property contains the last page object that was sent to the UniForm client. 
    /// </summary>
    public Evado.Model.UniForm.AppData LastPage { get; set; }

    private EvDataBaseUpdate.UpdateOrderBy _DataBaseUpdateOrderBy = EvDataBaseUpdate.UpdateOrderBy.UpdateNo;

    /// <summary>
    /// This property contains the application event selection user name.
    /// </summary>
    public EvDataBaseUpdate.UpdateOrderBy DataBaseUpdateOrderBy
    {
      get { return _DataBaseUpdateOrderBy; }
      set { _DataBaseUpdateOrderBy = value; }
    }

    private EvDataBaseUpdate.UpdateVersionList _DatabaseUpdateVersion = EvDataBaseUpdate.UpdateVersionList.Null;

    /// <summary>
    /// This property contains the application event selection user name.
    /// </summary>
    public EvDataBaseUpdate.UpdateVersionList DatabaseUpdateVersion
    {
      get { return _DatabaseUpdateVersion; }
      set { _DatabaseUpdateVersion = value; }
    }

    /// <summary>
    /// This property contains the audit table name for audit selection.
    /// </summary>
    public EvDataChange.DataChangeTableNames AuditTableName { get; set; }

    /// <summary>
    /// This property contains the audit record guid for the record selection.
    /// </summary>
    public Guid AuditScheduleGuid { get; set; }

    /// <summary>
    /// This property contains the audit record guid for the record selection.
    /// </summary>
    public Guid AuditRecordGuid { get; set; }

    /// <summary>
    /// This property contains the audit record item guid for the record selection.
    /// </summary>
    public Guid AuditRecordItemGuid { get; set; }

    DateTime _UserMenuLoadedAt = EvStatics.CONST_DATE_NULL;

    /// <summary>
    /// This property object contains the menu item for updating.
    /// </summary>
    public EvMenuItem MenuItem { get; set; }

    public String MenuPlatformId { get; set; }

    String _MenuGroupIdentifer = String.Empty;
    /// <summary>
    /// This propety contains the current menu pageMenuGroup identifier.
    /// </summary>
    public String MenuGroupIdentifier
    {
      get { return _MenuGroupIdentifer; }
      set { _MenuGroupIdentifer = value; }
    }

    EvMenuItem _MenuGroupItem = new EvMenuItem ( );
    /// <summary>
    /// This property object contains the menu item for updating.
    /// </summary>
    public EvMenuItem MenuGroupItem
    {
      get { return _MenuGroupItem; }
      set { _MenuGroupItem = value; }
    }

    bool _CollectUserAddress = false;
    /// <summary>
    /// This property indicates if the user address is to be collected.
    /// </summary>
    public bool CollectUserAddress
    {
      get { return _CollectUserAddress; }
      set { _CollectUserAddress = value; }
    }

    EvUserProfile _UserProfile = new EvUserProfile ( );
    /// <summary>
    /// This property contains the users eClinical user profile.
    /// </summary>
    public Evado.Model.Digital.EvUserProfile UserProfile
    {
      get
      {
        return this._UserProfile;
      }
      set
      {
        this._UserProfile = value;
      }
    }


    Evado.Model.Digital.EvExportParameters _ExportParameters = new EvExportParameters ( );
    /// <summary>
    /// This property defines the export parameter object.
    /// </summary>
    public Evado.Model.Digital.EvExportParameters ExportParameters
    {
      get { return _ExportParameters; }
      set { _ExportParameters = value; }
    }

    List<Evado.Model.Digital.EdApplication> _ApplicationList = new List<Evado.Model.Digital.EdApplication> ( );
    /// <summary>
    /// This property contains the list of trials as option objects.
    /// </summary>
    public List<Evado.Model.Digital.EdApplication> ApplicationList
    {
      get { return this._ApplicationList; }
      set { this._ApplicationList = value; }
    }

    /// <summary>
    /// This property contains the list of studies as option objects.
    /// </summary>
    public List<EvOption> ApplicationSelectionList
    {
      get
      {
        //
        // Initialise the properties objects and variables.
        //
        List<EvOption> optionList = new List<EvOption> ( );
        optionList.Add (
            new Evado.Model.EvOption ( "Null", String.Empty ) );

        if ( this.ApplicationList == null )
        {
          return optionList;
        }

        //
        // Iterate through the study list generate the selection list.
        //
        foreach ( Model.Digital.EdApplication app in this.ApplicationList )
        {
          optionList.Add ( new EvOption ( app.ApplicationId,
            string.Format ( "{0} - {1}", app.ApplicationId, app.Title ) ) );
        }

        return optionList;
      }
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Customer properties
    Evado.Model.Digital.EvCustomer _Customer = new EvCustomer ( );
    /// <summary>
    /// This property object contains the eClinical customer object.
    /// </summary>
    public Evado.Model.Digital.EvCustomer Customer
    {
      get
      {
        return this._Customer;
      }
      set
      {
        this._Customer = value;
      }
    }

    List<Evado.Model.Digital.EvOrganisation> _OrganisationList = new List<Evado.Model.Digital.EvOrganisation> ( );
    /// <summary>
    /// This property object contains a list of the application organisations object.
    /// </summary>
    public List<EvOrganisation> OrganisationList
    {
      get
      {
        return this._OrganisationList;
      }
      set
      {
        this._OrganisationList = value;
      }
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Application properties

    Evado.Model.Digital.EdApplication _Trial = new Evado.Model.Digital.EdApplication ( );
    /// <summary>
    /// This property object contains the trial object.
    /// </summary>
    public Evado.Model.Digital.EdApplication Application
    {
      get
      {
        return this._Trial;
      }
      set
      {
        this._Trial = value;
      }
    }
    public List<EvBinaryFileMetaData> FileMetaDataList { get; set; }

    Evado.Model.Digital.EvPageIds _PageId = new Evado.Model.Digital.EvPageIds ( );
    /// <summary>
    /// This property object contains the trial object.
    /// </summary>
    public Evado.Model.Digital.EvPageIds PageId
    {
      get
      {
        return this._PageId;
      }
      set
      {
        this._PageId = value;
      }
    }


    //===================================================================================
    /// <summary>
    /// This method sets the page type
    /// </summary>
    /// <param name="PageId">String: page type expression.</param>
    //-----------------------------------------------------------------------------------
    public void setPageId ( String PageId )
    {
      //
      // if the parameter is empty exit.
      //
      if ( PageId == String.Empty )
      {
        return;
      }

      //
      // set the old page type as a local variable.
      //
      EvPageIds oldPageId = this.PageId;

      //
      // Try to parse the page type.
      //
      try
      {
        this.PageId = Evado.Model.EvStatics.Enumerations.parseEnumValue<EvPageIds> ( PageId );
      }
      catch
      {
        //
        // in error event reset to the old page type.
        //
        this.PageId = oldPageId;
      }
    }

    List<Evado.Model.EvOption> _IssueFormList = new List<Evado.Model.EvOption> ( );
    /// <summary>
    /// This property contains a list of the sites in this trial.
    /// </summary>
    public List<Evado.Model.EvOption> IssueFormList
    {
      get { return _IssueFormList; }
      set { _IssueFormList = value; }
    }

    /// <summary>
    /// This property object contains a the current set binary file organisation identifier.
    /// </summary>
    public String BinaryFileOrgId { get; set; }
    /// <summary>
    /// This property object contains a the current set binary file identifier.
    /// </summary>
    public String BinaryFileId { get; set; }

    /// <summary>
    /// This property object contains a list of binary file metadata objects.
    /// </summary>
    public List<Evado.Model.Digital.EvBinaryFileMetaData> BinaryFileList { get; set; }

    /// <summary>
    /// This property object contains a binary file metadata objects.
    /// </summary>
    public Evado.Model.Digital.EvBinaryFileMetaData BinaryFile { get; set; }
    /// <summary>
    /// This property object contains a list of versioned file metadata objects for a FileId.
    /// </summary>
    public List<Evado.Model.Digital.EvBinaryFileMetaData> BinaryFileVersionList { get; set; }

    /// <summary>
    /// This property object contains the eClinical administration organisation object.
    /// </summary>
    public Evado.Model.Digital.EvOrganisation AdminOrganisation { get; set; }

    //
    // This property is a list of the eclinical administrator usres list.
    //
    public List<Evado.Model.Digital.EvUserProfile> AdminUserProfileList { get; set; }

    /// <summary>
    /// This property contains the users eClinical user profile used by the admin module.
    /// </summary>
    public Evado.Model.Digital.EvUserProfile AdminUserProfile { get; set; }

    EvOrganisation _Organisation = new EvOrganisation ( );
    /// <summary>
    /// This property object contains the eClinical trial object.
    /// </summary>
    public EvOrganisation Organisation
    {
      get
      {
        return this._Organisation;
      }
      set
      {
        this._Organisation = value;
      }
    }

    /// <summary>
    /// This property contains the form analysis record list.
    /// Default is null.
    /// </summary>
    public List<EdRecord> AnalysisRecordlist { get; set; }

    /// <summary>
    /// This property contains a analysis form selection list.
    /// Default is null.
    /// </summary>
    public List<EvOption> AnalysisFormSelectionList { get; set; }

    /// <summary>
    /// This property contains a analysis form field selection list.
    /// Default is null.
    /// </summary>
    public List<EvOption> AnalysisFormFieldSelectionList { get; set; }

    /// <summary>
    /// This property contains a analysis form field value selection list.
    /// Default is null.
    /// </summary>
    public List<EvOption> AnalysisFormFieldValueSelectionList { get; set; }

    /// <summary>
    /// This property contains the Record query form identifier value.
    /// </summary>
    public String AnalysisQueryFormId { get; set; }

    /// <summary>
    /// This property contains the Record query form field identifier value.
    /// </summary>
    public String AnalysisQueryFormFieldId { get; set; }

    /// <summary>
    /// This property contains the Record query form field ResultData value.
    /// </summary>
    public String AnalysisQueryFormFieldValue { get; set; }

    /// <summary>
    /// This property contains the EvChart object used to create chart ResultData analysis queries.
    /// </summary>
    public EvChart Chart { get; set; }

    /// <summary>
    /// This dictionary property contains option lists
    /// </summary>
    public List<EvOption> ChartSourceOptionList { get; set; }


    /// <summary>
    /// This property contains the list of report sources object.
    /// </summary>
    public List<EvReportSource> ReportSourceList { get; set; }

    /// <summary>
    /// This property contains the current report source object.
    /// </summary>
    public EvReportSource ReportSource { get; set; }

    /// <summary>
    /// This property contains the current report source object.
    /// </summary>
    public String ReportFileName { get; set; }

    /// <summary>
    /// This property object contains a list of eClinical EvReports objects.
    /// </summary>
    public List<EvReport> ReportDesignTemplateList { get; set; }

    /// <summary>
    /// This property object contains a list of eClinical EvReports objects.
    /// </summary>
    public List<EvOption> ReportTemplateList_All { get; set; }

    /// <summary>
    /// This property object contains a list of eClinical EvReports objects.
    /// </summary>
    public List<EvReport> ReportTemplateList { get; set; }

    /// <summary>
    /// This property contains the list of studies as option objects.
    /// </summary>
    public List<EvOption> ReportApplicationList
    {
      get
      {
        //
        // Initialise the properties objects and variables.
        //
        List<EvOption> optionList = new List<EvOption> ( );
        optionList.Add (
            new Evado.Model.EvOption ( EvcStatics.CONST_GLOBAL_PROJECT, "Global" ) );

        if ( this.ApplicationList == null )
        {
          return optionList;
        }

        //
        // Iterate through the study list generate the selection list.
        //
        foreach ( Model.Digital.EdApplication app in this.ApplicationList )
        {
          optionList.Add ( new EvOption ( app.ApplicationId,
            string.Format ( "{0} - {1}", app.ApplicationId, app.Title ) ) );
        }

        return optionList;
      }
    }
    /// <summary>
    /// This property contains the current report template object.
    /// </summary>
    public EvReport ReportTemplate { get; set; }

    /// <summary>
    /// This property contains the current report object.
    /// </summary>
    public EvReport Report { get; set; }

    private String _ReportStudyId = "GLOBAL";
    /// <summary>
    /// This property stores the current report type selection.
    /// </summary>
    public String ReportStudyId
    {
      get { return _ReportStudyId; }
      set { _ReportStudyId = value; }
    }

    /* private EvReport.ReportTypeCode _ReportTypeId = EvReport.ReportTypeCode.Null;
     /// <summary>
     /// This property stores the current report type selection.
     /// </summary>
     public EvReport.ReportTypeCode ReportType
     {
       get { return _ReportTypeId; }
       set { _ReportTypeId = value; }
     }
     */
    private String _ReportCategory = String.Empty;
    /// <summary>
    /// This property stores the current report category string.
    /// </summary>
    public String ReportCategory
    {
      get { return _ReportCategory; }
      set { _ReportCategory = value; }
    }

    private EvReport.ReportScopeTypes _ReportScope = EvReport.ReportScopeTypes.Null;
    /// <summary>
    /// This property stores the current report scope.
    /// </summary>
    public EvReport.ReportScopeTypes ReportScope
    {
      get { return _ReportScope; }
      set { _ReportScope = value; }
    }

    /// <summary>
    /// This property contains the form template upload filename
    /// </summary>
    public String UploadFileName { get; set; }

    //
    // this indicates if the forms where adapter loaded.
    //
    public bool FormsAdaperLoaded { get; set; }

    List<EdRecord> _FormList = new List<EdRecord> ( );
    /// <summary>
    /// This property object contains a list of eClinical evForm object for the currently selected record.
    /// </summary>
    public List<EdRecord> FormList
    {
      get { return _FormList; }
      set { _FormList = value; }
    }

    EdRecord _Form = new EdRecord ( );
    /// <summary>
    /// This property object contains the eClinical evForm object for the currently selected record.
    /// </summary>
    public EdRecord Form
    {
      get { return _Form; }
      set { _Form = value; }
    }

    EdRecordField _FormField = new EdRecordField ( );
    /// <summary>
    /// This property object contains the eClinical evFormField object for the currently selected record.
    /// </summary>
    public EdRecordField FormField
    {
      get { return _FormField; }
      set { _FormField = value; }
    }

    public String RecordSelectionState = String.Empty;

    public EdRecordTypes FormRecordType { get; set; }

    public List<EdRecord> AdminRecordList { get; set; }

    public List<EdRecord> RecordList { get; set; }

    String _FormRecords_Selected_FormId = String.Empty;
    /// <summary>
    /// COntains the currently selected form type.
    /// </summary>
    public String RecordSelectionFormId
    {
      get { return _FormRecords_Selected_FormId; }
      set { _FormRecords_Selected_FormId = value; }
    }

    private bool _FormRecords_IncludeTestSites = false;
    /// <summary>
    /// This property defines the  form record filter to include test sites.
    /// </summary>
    public bool FormRecords_IncludeTestSites
    {
      get { return _FormRecords_IncludeTestSites; }
      set { _FormRecords_IncludeTestSites = value; }
    }

    private bool _FormRecords_IncludeDraftRecords = false;
    /// <summary>
    /// This property defines the include draft record selection.
    /// </summary>
    public bool FormRecords_IncludeDraftRecords
    {
      get { return _FormRecords_IncludeDraftRecords; }
      set { _FormRecords_IncludeDraftRecords = value; }
    }

    private bool _FormRecords_IncludeFreeTextData = false;
    /// <summary>
    /// this property defines the include free text ResultData selection.
    /// </summary>
    public bool FormRecords_IncludeFreeTextData
    {
      get { return _FormRecords_IncludeFreeTextData; }
      set { _FormRecords_IncludeFreeTextData = value; }
    }

    EdRecordTypes _FormType = EdRecordTypes.Null;
    /// <summary>
    /// COntains the currently selected form type.
    /// </summary>
    public EdRecordTypes FormType
    {
      get { return _FormType; }
      set { _FormType = value; }
    }

    private int _SelectedFormVersion = 1;
    /// <summary>
    /// This property contains the selected form version for a version selection query.
    /// </summary>
    public int SelectedFormVersion
    {
      get
      {
        return this._SelectedFormVersion;
      }
      set
      {
        this._SelectedFormVersion = value;
      }
    }

    //===================================================================================
    /// <summary>
    /// This method set the form record type.
    /// </summary>
    /// <param name="FormType">EvFormRecordTypes enumerated list</param>
    //-----------------------------------------------------------------------------------
    public void setFormType ( String FormType )
    {
      EdRecordTypes recordType = EdRecordTypes.Null;

      if ( EvStatics.Enumerations.tryParseEnumValue<EdRecordTypes> ( FormType, out recordType ) == true )
      {
        this.FormType = recordType;
      }

      this.FormType = EdRecordTypes.Null;
    }

    EdRecordObjectStates _FormState = EdRecordObjectStates.Null;
    /// <summary>
    /// COntains the currently selected form state.
    /// </summary>
    public EdRecordObjectStates FormState
    {
      get { return _FormState; }
      set { _FormState = value; }
    }

    EvFormSection _FormSection = new EvFormSection ( );
    /// <summary>
    /// This property object contains the eClinical EvFormSeciotn object for the currently selected record.
    /// </summary>
    public EvFormSection FormSection
    {
      get { return _FormSection; }
      set { _FormSection = value; }
    }

    /// <summary>
    /// This property contains the list of Form Versions.
    /// </summary>
    public List<EvOption> FormVersionList { get; set; }

    List<EdRecord> _CommonFormList = new List<EdRecord> ( );
    /// <summary>
    /// This property object contains a list of eClinical evForm object for the currently selected record.
    /// </summary>
    public List<EdRecord> CommonFormList
    {
      get { return _CommonFormList; }
      set { _CommonFormList = value; }
    }

    EdRecord _CommonForm = new EdRecord ( );
    /// <summary>
    /// This property object contains the eClinical evForm object for the currently selected record.
    /// </summary>
    public EdRecord CommonForm
    {
      get { return _CommonForm; }
      set { _CommonForm = value; }
    }

    EvFormSection _CommonFormSection = new EvFormSection ( );
    /// <summary>
    /// This property object contains the eClinical EvFormSeciotn object for the currently selected record.
    /// </summary>
    public EvFormSection CommonFormSection
    {
      get { return _CommonFormSection; }
      set { _CommonFormSection = value; }
    }

    EdRecordField _CommonFormField = new EdRecordField ( );
    /// <summary>
    /// This property object contains the eClinical evFormField object for the currently selected record.
    /// </summary>
    public EdRecordField CommonFormField
    {
      get { return _CommonFormField; }
      set { _CommonFormField = value; }
    }

    EdRecordTypes _CommonFormType = EdRecordTypes.Null;
    /// <summary>
    /// COntains the currently selected form type.
    /// </summary>
    public EdRecordTypes CommonFormType
    {
      get { return _CommonFormType; }
      set { _CommonFormType = value; }
    }

    //===================================================================================
    /// <summary>
    /// This method set the common form type.
    /// </summary>
    /// <param name="FormType">EvFormRecordTypes enumerated list</param>
    //-----------------------------------------------------------------------------------
    public void setCommonFormType ( String FormType )
    {
      EdRecordTypes recordType = EdRecordTypes.Null;

      if ( EvStatics.Enumerations.tryParseEnumValue<EdRecordTypes> ( FormType, out recordType ) == true )
      {
        this._CommonFormType = recordType;
      }

      this._CommonFormType = EdRecordTypes.Null;
    }

    EdRecordObjectStates _CommonFormState = EdRecordObjectStates.Null;
    /// <summary>
    /// COntains the currently selected form state.
    /// </summary>
    public EdRecordObjectStates CommonFormState
    {
      get { return _CommonFormState; }
      set { _CommonFormState = value; }
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Site properties properties


    public EvAlert.AlertTypes AlertType { get; set; }

    public EvAlert.AlertStates AlertState { get; set; }

    /// <summary>
    /// This property object contains the user alert object.
    /// </summary>
    public EvAlert Alert { get; set; }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Schedule properties properties

    int _ScheduleId = 1;

    /// <summary>
    /// This property contains current sechedule id.
    /// </summary>
    public int ScheduleId
    {
      get { return _ScheduleId; }
      set { _ScheduleId = value; }
    }

    EvSchedule.ScheduleStates _ScheduleState = 0;

    /// <summary>
    /// This property contains the schedule version selections.
    /// </summary>
    public EvSchedule.ScheduleStates ScheduleState
    {
      get { return _ScheduleState; }
      set { _ScheduleState = value; }
    }

    /// <summary>
    /// This property object contains a list of eClinical EvSchedule objects.
    /// </summary>
    public List<EvSchedule> ScheduleList { get; set; }

    /// <summary>
    /// This property contains the currently selecte schedule.
    /// </summary>
    public EvSchedule Schedule { get; set; }

    List<EvMilestone> _MilestoneList = new List<EvMilestone> ( );
    /// <summary>
    /// This property object contains a list of eClinical EvMilestone objects.
    /// </summary>
    public List<EvMilestone> MilestoneList
    {
      get { return _MilestoneList; }
      set { _MilestoneList = value; }
    }

    EvMilestone _Milestone = new EvMilestone ( );
    /// <summary>
    /// This property object contains a EvMilestone object.
    /// </summary>
    public EvMilestone Milestone
    {
      get { return _Milestone; }
      set { _Milestone = value; }
    }

    List<EvActivity> _ActivityList = new List<EvActivity> ( );
    /// <summary>
    /// This property object contains a list of eClinical EvActivity objects.
    /// </summary>
    public List<EvActivity> ActivityList
    {
      get { return _ActivityList; }
      set { _ActivityList = value; }
    }

    private EvActivity _activity = new EvActivity ( );
    /// <summary>
    /// This property contains the currently selecte activty.
    /// </summary>
    public EvActivity Activity
    {
      get { return _activity; }
      set { _activity = value; }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region class methods

    // ==================================================================================
    /// <summary>
    /// This methods performs a MarkDown text substitution in the Content text passed into the
    /// method and returns text with the relevant text substitutions.
    /// </summary>
    /// <param name="Project">EvProject: The project object containing the values.</param>
    /// <param name="Content">String: MarkDown text containing the values to be substituted</param>
    /// <returns>Markdown String</returns>
    //  ---------------------------------------------------------------------------------
    public String substituteDataValue (
      String Content )
    {
      String content = Content;

      //
      // Substitute the project markup for actual data.
      //
      content = content.Replace ( EvcStatics.TEXT_SUBSITUTION_TRIAL_ID, this.Application.ApplicationId );
      content = content.Replace ( EvcStatics.TEXT_SUBSITUTION_TRIAL_TITLE, this.Application.Title );

      if ( this.Application.Description == String.Empty
        || this.Application.Description == EvLabels.Project_Create_Project_Description )
      {
        content = content.Replace ( EvcStatics.TEXT_SUBSITUTION_TRIAL_DESCRIPTION, String.Empty );
      }
      else
      {
        content = content.Replace ( EvcStatics.TEXT_SUBSITUTION_TRIAL_DESCRIPTION, this.Application.Description );
      }

      //
      // Return the substituted text 
      //
      return content;
    }

    // ==================================================================================
    /// <summary>
    /// This methods performs a MarkDown text substitution in the Content text passed into the
    /// method and returns text with the relevant text substitutions.
    /// </summary>
    /// <param name="Content">String: MarkDown text containing the values to be substituted</param>
    /// <param name="SubjectMilestone">EvMilestone: the subject milestone</param>
    /// <returns>Markdown String</returns>
    //  ---------------------------------------------------------------------------------
    public String substituteDataValue (
      String Content,
      String ErrorMessage )
    {
      String content = this.substituteDataValue ( Content );

      //
      // substitute the milestone values.
      //
      if ( ErrorMessage != String.Empty )
      {
        content = content.Replace ( EvcStatics.TEXT_SUBSITUTION_MILESTONE_DESCRIPTION, ErrorMessage );
      }
      else
      {
        content = content.Replace ( EvcStatics.TEXT_SUBSITUTION_MILESTONE_DESCRIPTION, String.Empty );
      }

      //
      // Return the substituted text content.
      //
      return content;
    }
    // ==================================================================================
    /// <summary>
    /// This methods performs a MarkDown text substitution in the Content text passed into the
    /// method and returns text with the relevant text substitutions.
    /// </summary>
    /// <param name="Content">String: MarkDown text containing the values to be substituted</param>
    /// <param name="SubjectMilestone">EvMilestone: the subject milestone</param>
    /// <returns>Markdown String</returns>
    //  ---------------------------------------------------------------------------------
    public String substituteDataValue (
      String Content,
      EvMilestone SubjectMilestone )
    {
      String content = this.substituteDataValue ( Content );

      //
      // substitute the milestone values.
      //
      content = content.Replace ( EvcStatics.TEXT_SUBSITUTION_MILESTONE_ID, SubjectMilestone.MilestoneId );

      content = content.Replace ( EvcStatics.TEXT_SUBSITUTION_MILESTONE_NAME, SubjectMilestone.Title );

      if ( SubjectMilestone.Description != String.Empty )
      {
        content = content.Replace ( EvcStatics.TEXT_SUBSITUTION_MILESTONE_DESCRIPTION, SubjectMilestone.Description );
      }
      else
      {
        content = content.Replace ( EvcStatics.TEXT_SUBSITUTION_MILESTONE_DESCRIPTION, String.Empty );
      }

      content = content.Replace ( EvcStatics.TEXT_SUBSITUTION_QUESTIONNAIRE_URL, String.Empty );

      //
      // Return the substituted text content.
      //
      return content;
    }

    // ==================================================================================
    /// <summary>
    /// This methods performs a MarkDown text substitution in the Content text passed into the
    /// method and returns text with the relevant text substitutions.
    /// </summary>
    /// <param name="Content">String: MarkDown text containing the values to be substituted</param>
    /// <param name="SubjectMilestone">EvMilestone: the subject milestone</param>
    /// <returns>Markdown String</returns>
    //  ---------------------------------------------------------------------------------
    public String substituteDataValue (
      String Content,
      EvMilestone SubjectMilestone,
      EdRecord FormRecord,
      String QuestionnaireUrl )
    {
      String content = this.substituteDataValue ( Content );

      //
      // substitute the milestone values.
      //
      content = content.Replace ( EvcStatics.TEXT_SUBSITUTION_MILESTONE_ID, SubjectMilestone.MilestoneId );
      content = content.Replace ( EvcStatics.TEXT_SUBSITUTION_MILESTONE_NAME, SubjectMilestone.Title );
      if ( SubjectMilestone.Description != String.Empty )
      {
        content = content.Replace ( EvcStatics.TEXT_SUBSITUTION_MILESTONE_DESCRIPTION, SubjectMilestone.Description );
      }
      else
      {
        content = content.Replace ( EvcStatics.TEXT_SUBSITUTION_MILESTONE_DESCRIPTION, String.Empty );
      }

      //
      // retreive the Questionnaire URL from the config and add record Guid to it.
      //
      if ( QuestionnaireUrl != String.Empty )
      {
        QuestionnaireUrl += FormRecord.Guid.ToString ( );

        content = content.Replace ( EvcStatics.TEXT_SUBSITUTION_QUESTIONNAIRE_URL, QuestionnaireUrl.Trim ( ) );
      }

      //
      // Return the substituted text content.
      //
      return content;
    }

    // ==================================================================================
    /// <summary>
    /// This methods performs a MarkDown text substitution in the Content text passed into the
    /// method and returns text with the relevant text substitutions.
    /// </summary>
    /// <param name="Content">String: MarkDown text containing the values to be substituted</param>
    /// <param name="SubjectMilestone">EvMilestone: the subject milestone</param>
    /// <returns>Markdown String</returns>
    //  ---------------------------------------------------------------------------------
    public String substituteDataValue (
      String Content,
      EdRecord CommonRecord,
      String ConsentUrl )
    {
      String content = this.substituteDataValue ( Content );


      //
      // retreive the Questionnaire URL from the config and add Guid to it.
      //
      if ( ConsentUrl != String.Empty )
      {
        ConsentUrl += CommonRecord.Guid.ToString ( );

        content = content.Replace ( EvcStatics.TEXT_SUBSITUTION_CONSENT_URL, ConsentUrl.Trim ( ) );
      }

      //
      // Return the substituted text content.
      //
      return content;
    }
    // ==================================================================================
    /// <summary>
    /// This methods performs a MarkDown text substitution in the Content text passed into the
    /// method and returns text with the relevant text substitutions.
    /// </summary>
    /// <param name="Content">String: MarkDown text containing the values to be substituted</param>
    /// <param name="SubjectMilestone">EvMilestone: the subject milestone</param>
    /// <returns>Markdown String</returns>
    //  ---------------------------------------------------------------------------------
    public String substituteDataValue (
      String Content,
      EvUserProfile PatientUseProfile )
    {
      String content = Content;

      //
      // substitute the user ID into the content.
      //
      if ( PatientUseProfile.UserId != String.Empty )
      {
        content = content.Replace (
          EvcStatics.TEXT_SUBSITUTION_USER_ID,
          PatientUseProfile.UserId.Trim ( ) );
      }
      else
      {
        content = content.Replace (
          EvcStatics.TEXT_SUBSITUTION_USER_ID, String.Empty );
      }

      //
      // substitute the user ID into the content.
      //
      if ( PatientUseProfile.Password != String.Empty )
      {
        content = content.Replace (
          EvcStatics.TEXT_SUBSITUTION_PASSWORD,
          PatientUseProfile.Password.Trim ( ) );
      }
      else
      {
        content = content.Replace (
          EvcStatics.TEXT_SUBSITUTION_PASSWORD, String.Empty );
      }

      //
      // Return the substituted text content.
      //
      return content;
    }

    private bool _BlockAllResets = false;
    /// <summary>
    /// This property blocks the automatic reset of objects when 
    /// Project, ProjectOrganisations or Subject objects change values.
    /// 
    /// True: block all resets
    /// </summary>
    public bool BlockAllResets
    {
      get { return _BlockAllResets; }
      set { _BlockAllResets = value; }
    }


    public EdRecord Record { get; set; }

    public EdRecord CommonRecord { get; set; }

    public EvAncillaryRecord AncillaryRecord = new EvAncillaryRecord ( );

    // ==================================================================================
    /// <summary>
    /// This methods resets the session object value on a trial change.
    /// </summary>
    //  ---------------------------------------------------------------------------------
    private void resetCustomerObjects ( )
    {
      this.Application = new Evado.Model.Digital.EdApplication ( );
      this.ApplicationList = new List<Model.Digital.EdApplication> ( );
      this.OrganisationList = new List<EvOrganisation> ( );
    }


    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

  }//END class Method

}//END namespace
