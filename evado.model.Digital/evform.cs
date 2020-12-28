/***************************************************************************************
 * <copyright file="model\EvForm.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvForm data object.
 *
 ****************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

namespace Evado.Model.Digital
{
  /// <summary>
  /// This class defines the evado form object.
  /// </summary>
  [Serializable]
  public class EvForm
    {
    #region Class Enumerators

    //  ===================================================================================
    /// <summary>
    /// This enumeration defines the record save actions
    /// </summary>
    //  ----------------------------------------------------------------------------------- 
    public enum SaveActionCodes
      {
      /// <summary>
      /// This enumeration defines the default save action
      /// </summary>
      Save,

      /// <summary>
      /// This enumeration defines the save form save action
      /// </summary>
      Form_Saved,

      /// <summary>
      /// This enumeration defines the form reciew save action
      /// </summary>
      Form_Reviewed,

      /// <summary>
      /// This enumeration defines the form approved save action.
      /// </summary>
      Form_Approved,

      /// <summary>
      /// This enumeratin defines the form withdrawn save action.
      /// </summary>
      Form_Withdrawn,

      /// <summary>
      /// This enumeration defines the form deleted save action.
      /// </summary>
      Form_Deleted,

      /// <summary>
      /// This enumeration defines the record saved save action.
      /// </summary>
      Save_Record,

      /// <summary>
      /// This enumeration defines the record submitted save action.
      /// </summary>
      Submit_Record,

      /// <summary>
      /// this enumeration defines the record reviewed save action.
      /// </summary>
      Review_Save,

      /// <summary>
      /// This enumeration defines the record monitor save action.
      /// </summary>
      Monitor_Save,

      /// <summary>
      /// This enumeration defines the record data manageer save action.
      /// </summary>
      DataManager_Save,

      /// <summary>
      /// This enumeration defines the record withdrawn save action.
      /// </summary>
      Withdrawn_Record,

      /// <summary>
      /// This enumeration defines the record unlock save action.
      /// </summary>
      Unlock_Record,
      }

    /// <summary>
    /// This enumeration list defines the query states.
    /// </summary>
    public enum QueryStates
      {
      /// <summary>
      /// This enumeration defines the null query state.
      /// </summary>
      Null = 0,

      /// <summary>
      /// This enumeration defines the not queried state
      /// </summary>
      None = 1,

      /// <summary>
      /// This enumeration defines the open query state
      /// </summary>
      Open = 2,

      /// <summary>
      /// This enumeration defines the closed query state
      /// </summary>
      Closed = 3,
      }

    /// <summary>
    /// This enumeration list 
    /// </summary>
    public enum FormAccessRoles
      {
      /// <summary>
      /// This enumeration defines the form role is not set. So display only.
      /// </summary>
      Null = 0,

      /// <summary>
      /// This enumeration defines the record author role this person is able to update the form content.
      /// </summary>
      Record_Author,

      /// <summary>
      /// This enumeration defines the patient record editor roles this user is able to update the form contents 
      /// with registed access to various sections.
      /// </summary>
      Patient,

      /// <summary>
      /// This enumeration defines the record monitor role this person is able to review, query annotate the record if it 
      /// is in Submitted state.
      /// </summary>
      Monitor,

      /// <summary>
      /// TThis enumeration defines the record monitor role this person is able to review, query annotate the record if it 
      /// is in Submitted, or source data viewed state.  And edit the data if the record is in locked state.
      /// </summary>
      Data_Manager,

      /// <summary>
      /// This enumerate defiens the record view mode, and has the same access as the Null role
      /// </summary>
      Record_Reader,

      /// <summary>
      /// This enumerate defiens the record the form designer role.
      /// </summary>
      Form_Designer
      }


    /// <summary>
    /// This enumeration list 
    /// </summary>
    public enum UpdateReasonList
      {
      /// <summary>
      /// This enumeration defines the form role is not set. So display only.
      /// </summary>
      Null,
      /// <summary>
      /// This enumeration defines the first release.
      /// </summary>
      First_Release,

      /// <summary>
      /// This enumeration defines a form design change.
      /// </summary>
      Form_Design,

      /// <summary>
      /// This enumeration defines a minor change to the form layout.
      /// </summary>
      Minor_Update,

      /// <summary>
      /// This enumeration defines a major change for a protocol change.
      /// </summary>
      Protocol_Change,

      /// <summary>
      /// This enumeration defines a major change for a regulatory change.
      /// </summary>
      Regulatory_Change,
      }

    /// <summary>
    /// This enumeration list 
    /// </summary>
    public enum FormDisplayStates
      {
      /// <summary>
      /// This enumeration defines the display page layout selection
      /// </summary>
      Display = 0,

      /// <summary>
      /// This enumeration defines the edit page layout selection
      /// </summary>
      Edit = -80000,

      /// <summary>
      /// This enumeration defines the review page layout selection
      /// </summary>
      Review = -81000,

      /// <summary>
      /// This enumeration defines the data cleansing page layout selection
      /// </summary>
      DataCleansing = -82000,

      /// <summary>
      /// This enumeration defines the review design layout selection
      /// </summary>
      Design = -830000,
      }

    /// <summary>
    /// This enumeration sets the subject identifier field formats.
    /// </summary>
    public enum SubjectIdentifierFieldFormats
      {
      /// <summary>
      /// This enumeration selects a text formatting of the field identifier value.
      /// </summary>
      Text,

      /// <summary>
      /// This enumeration selects a integer validation of the field identifier value.
      /// </summary>
      Integer,

      /// <summary>
      /// This enumeration selects a 6 digit integrat validation of the field identifier value.
      /// </summary>
      Six_Digit_Integer,
      }

    /// <summary>
    /// This enumeration list defines the form class field names enumerated identifiers.
    /// </summary>
    public enum FormClassFieldNames
      {
      /// <summary>
      /// This enumeration is the null value
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration identifies the trial identifier field.
      /// </summary>
      ProjectId,

      /// <summary>
      /// This enumeration identifies the form identifier field.
      /// </summary>
      FormId,

      /// <summary>
      /// This enumeration identifies the milestone identifier field
      /// </summary>
      MilestoneId,

      /// <summary>
      /// This enumeration identified the organisation identifier field.
      /// </summary>
      OrgId,

      /// <summary>
      /// This enumeration identifies the subject identifier field.
      /// </summary>
      SubjectId,

      /// <summary>
      /// This enumeration identifies the record identifier field.
      /// </summary>
      RecordId,

      /// <summary>
      /// This enumeration identifies the visit identifier field.
      /// </summary>
      VisitId,

      /// <summary>
      /// This enumeration identifies the schedule identifier field.
      /// </summary>
      ScheduleId,

      /// <summary>
      /// This enumeration identifies the record date field.
      /// </summary>
      RecordDate,

      /// <summary>
      /// This enumeration identifies the record subject field.
      /// </summary>
      RecordSubject,

      /// <summary>
      /// This enumeration identifies the start date field.
      /// </summary>
      StartDate,

      /// <summary>
      /// This enumeration identifies the finish date field.
      /// </summary>
      FinishDate,

      /// <summary>
      /// This enumeration identifies the visit date field.
      /// </summary>
      VisitDate,

      /// <summary>
      /// This enumeration identifies the form annotation field.
      /// </summary>
      Annotation,

      /// <summary>
      /// This enumeration identifies the form comments field.
      /// </summary>
      Comments,

      /// <summary>
      /// This enumeration identifies the source identifier field.
      /// </summary>
      SourceId,

      /// <summary>
      /// This enumeration identifies the form or record status field.
      /// </summary>
      Status,

      /// <summary>
      /// This enumeration identifies the reference identifier field.
      /// </summary>
      ReferenceId,

      /// <summary>
      /// This enumeration identifies the activity identifier field.
      /// </summary>
      ActivityId,

      /// <summary>
      /// This enumeration identifies the activity is mandatory field.
      /// </summary>
      IsMandatoryActivity,

      /// <summary>
      /// This enumeration identifies the form record is mandatory field.
      /// </summary>
      IsMandatory,

      /// <summary>
      /// This enumeration identifies the form layout parameter value
      /// </summary>
      Form_Layout,

      /// <summary>
      /// This enumeration identifies the form title field.
      /// </summary>
      Title,

      /// <summary>
      /// This enumeration identifies the form Instruction field.
      /// </summary>
      Instructions,

      /// <summary>
      /// This enumeration identifies the form Description field.
      /// </summary>
      Description,

      /// <summary>
      /// This enumeration identifies the form Update Reason field.
      /// </summary>
      UpdateReason,

      /// <summary>
      /// This enumeration identifies the form Template name field.
      /// </summary>
      TemplateName,

      /// <summary>
      /// This enumeration identifies the form reference field.
      /// </summary>
      Reference,

      /// <summary>
      /// This enumeration identifies the form form category field.
      /// </summary>
      FormCategory,

      /// <summary>
      /// This enumeration identifies the form record subject instructions field.
      /// </summary>
      RecordSubjectInstructions,

      /// <summary>
      /// This enumeration identifies the form state date instructions field.
      /// </summary>
      StartDateInstructions,

      /// <summary>
      /// This enumeration identifies the form finish date instructions field.
      /// </summary>
      FinishDateInstructions,

      /// <summary>
      /// This enumeration identifies the form subject screening id instructions field.
      /// </summary>
      SubjectScreeningIdInstructions,

      /// <summary>
      /// This enumeration identifies the form subject sponsored id instructions field.
      /// </summary>
      SubjectSponsorIdInstructions,

      /// <summary>
      /// This enumeration identifies the form subject randomised id instructions field.
      /// </summary>
      SubjectRandomisedIdInstructions,

      /// <summary>
      /// This enumeration identifies the form subject external id instructions field.
      /// </summary>
      SubjectExternalIdInstructions,

      /// <summary>
      /// This enumeration identifies the form subject screening id format field.
      /// </summary>
      SubjectScreeningIdFormat,

      /// <summary>
      /// This enumeration identifies the form subject sponsor id format field.
      /// </summary>
      SubjectSponsorIdFormat,

      /// <summary>
      /// This enumeration identifies the form subject randomised id format field.
      /// </summary>
      SubjectRandomisedIdFormat,

      /// <summary>
      /// This enumeration identifies the form subject external id format field.
      /// </summary>
      SubjectExternalIdFormat,

      /// <summary>
      /// This enumeration identifies the form type identifier field.
      /// </summary>
      TypeId,

      /// <summary>
      /// This enumeration identifies the form java validate script field.
      /// </summary>
      JavaValidationScript,

      /// <summary>
      /// This enumeration identifies the form has CS script field.
      /// </summary>
      HasCsScript,

      /// <summary>
      /// This enumeration identifies the form has CS script field.
      /// </summary>
      OnEdit_HideFieldAnnotation,

      /// <summary>
      /// This enumeration identifies the form user access role field
      /// </summary>
      FormAccessRole

      }

    /// <summary>
    /// This enumeration list defines the query selection codes for monitor record views.
    /// </summary>
    public enum RecordQuerySelectionCodes
      {
      /// <summary>
      /// This enumeration defines null or empty selection.
      /// </summary>
      Null,
      /// <summary>
      /// This enumeration selects all records regardless of query state.
      /// </summary>
      All,
      /// <summary>
      /// This enumeration selects the completed records, i.e. submitted.
      /// </summary>
      Completed_Records,

      /// <summary>
      /// This enumeration selects the records that have not been queried.
      /// </summary>
      Not_Queried,

      /// <summary>
      /// This enumeration selects the records that have been queried.
      /// </summary>
      Open_Queries,

      /// <summary>
      /// This enumeratin selects the records that have been queried and
      /// the issues is resolved.
      /// </summary>
      Closed_Queries,
      }

    #endregion

    #region Internal member parameters
    /// <summary>
    /// This constant defines the field type identifier for form fields.
    /// </summary>
    public const string CONST_RECORD_TYPE = "RECTYP";

    #endregion

    #region Class Properties

    /// <summary>
    /// This property contains a global unique identifier of a Customer . 
    /// </summary>
    public Guid CustomerGuid{ get; set; }

    private EvFormDesign _Design = new EvFormDesign ( );
    /// <summary>
    /// This property contains a design object of a form. 
    /// </summary>
    public EvFormDesign Design
    {
      get
      {
        return this._Design;
      }
      set
      {
        this._Design = value;
      }
    }

    private List<EvFormField> _Fields = new List<EvFormField> ( );
    /// <summary>
    /// This property contains a field list of a form. 
    /// </summary>
    public List<EvFormField> Fields
    {
      get
      {
        return this._Fields;
      }
      set
      {
        this._Fields = value;

        foreach ( EvFormField field in this._Fields )
        {
          field.TrialId  = this._TrialId;
        }
      }
    }

    private string _LinkText = String.Empty;
    /// <summary>
    /// This property contains a link text of a form. 
    /// </summary>
    public string LinkText
    {
      get
      {
        return this._LinkText;
      }
      set
      {
        this._LinkText = value;
      }
    }

    private EvFormContent _RecordContent = new EvFormContent ( );
    /// <summary>
    /// This property contains a record content object of a form. 
    /// </summary>
    public EvFormContent RecordContent
    {
      get
      {
        return this._RecordContent;
      }
      set
      {
        this._RecordContent = value;
      }
    }

    private List<EvOption> _AeSelectionList = new List<EvOption> ( );
    /// <summary>
    /// This property contains a selection list of a form. 
    /// </summary>
    public List<EvOption> AeSelectionList
    {
      get
      {
        return this._AeSelectionList;
      }
      set
      {
        this._AeSelectionList = value;
      }
    }

    private Guid _Guid = Guid.Empty;
    /// <summary>
    /// This property contains a global unique identifier of a form. 
    /// </summary>
    public Guid Guid
    {
      get
      {
        return this._Guid;
      }
      set
      {
        this._Guid = value;
      }
    }

    private Guid _FormGuid = Guid.Empty;
    /// <summary>
    /// This property contains a form global unique identifier of a form. 
    /// </summary>
    public Guid FormGuid
    {
      get
      {
        return this._FormGuid;
      }
      set
      {
        this._FormGuid = value;
      }
    }

    private string _TrialId = String.Empty;
    /// <summary>
    /// This property contains a trial identifier of a form. 
    /// </summary>
    public string TrialId
    {
      get
      {
        return this._TrialId;
      }
      set
      {
        this._TrialId = value;
      }
    }

    private string _OrgId = String.Empty;
    /// <summary>
    /// This property contains a organization identifier of a form. 
    /// </summary>
    public string OrgId
    {
      get
      {
        return this._OrgId;
      }
      set
      {
        this._OrgId = value;
      }
    }

    private string _SubjectId = String.Empty;
    /// <summary>
    /// This property contains a subject identifier of a form. 
    /// </summary>
    public string SubjectId
    {
      get
      {
        return this._SubjectId;
      }
      set
      {
        this._SubjectId = value;
      }
    }

    int _PatientId = -1;
    /// <summary>
    /// This property contains a subject identifier of a form. 
    /// </summary>
    public int PatientId
    {
      get
      {
        return this._PatientId;
      }
      set
      {
        this._PatientId = value;
      }
    }

    private string _SponsorId = String.Empty;
    /// <summary>
    /// This property contains a sponsor identifier of a form. 
    /// </summary>
    public string SponsorId
    {
      get
      {
        return this._SponsorId;
      }
      set
      {
        this._SponsorId = value;
      }
    }

    private string _VisitId = String.Empty;
    /// <summary>
    /// This property contains a visit identifier of a form. 
    /// </summary>
    public string VisitId
    {
      get
      {
        return this._VisitId;
      }
      set
      {
        this._VisitId = value;
      }
    }

    int _ScheduleId = 0;

    /// <summary>
    /// This property contains the schedule identifier when the record was created.
    /// </summary>
    public int ScheduleId
    {
      get { return _ScheduleId; }
      set { _ScheduleId = value; }
    }

    private string _MilestoneId = String.Empty;
    /// <summary>
    /// This property contains a milestone identifier of a form. 
    /// </summary>
    public string MilestoneId
    {
      get
      {
        return this._MilestoneId;
      }
      set
      {
        this._MilestoneId = value;
      }
    }

    private string _ActivityId = String.Empty;
    /// <summary>
    /// This property contains an activity identifier of a form. 
    /// </summary>
    public string ActivityId
    {
      get
      {
        return this._ActivityId;
      }
      set
      {
        this._ActivityId = value;
      }
    }

    private bool _IsMandatoryActivity = false;
    /// <summary>
    /// This property indicates a mandatory activity of a form. 
    /// </summary>
    public bool IsMandatoryActivity
    {
      get
      {
        return this._IsMandatoryActivity;
      }
      set
      {
        this._IsMandatoryActivity = value;
      }
    }

    /// <summary>
    /// This property contains a mandatory activity string of a form. 
    /// </summary>
    public string stIsMandatoryActivity
    {
      get
      {
        if ( this._IsMandatoryActivity == true )
        {
          return "Yes";
        }
        return "No";
      }
    }

    private bool _IsMandatory = false;
    /// <summary>
    /// This property indicates whether a form is mandatory.
    /// </summary>
    public bool IsMandatory
    {
      get
      {
        return this._IsMandatory;
      }
      set
      {
        this._IsMandatory = value;
      }
    }

    /// <summary>
    /// This property contains a string of a mandatory form. 
    /// </summary>
    public string stIsMandatory
    {
      get
      {
        if ( this._IsMandatory == true )
        {
          return "Yes";
        }
        return "No";
      }
    }

    private string _FormId = String.Empty;
    /// <summary>
    /// This property contains an identifier of a form. 
    /// </summary>
    public string FormId
    {
      get
      {
        return this._FormId;
      }
      set
      {
        this._FormId = value;
      }
    }

    private string _SourceId = String.Empty;
    /// <summary>
    /// This property contains a source identifier of a form. 
    /// </summary>
    public string SourceId
    {
      get
      {
        return this._SourceId;
      }
      set
      {
        this._SourceId = value;
      }
    }

    private string _RecordId = String.Empty;
    /// <summary>
    /// This property contains a record identifier of a form. 
    /// </summary>
    public string RecordId
    {
      get
      {
        return this._RecordId;
      }
      set
      {
        this._RecordId = value;

        // 
        // If type Id is null then attempt to generate it from the 
        // record Id.
        // 
        if ( this._Design.TypeId == EvFormRecordTypes.Null
          && this._RecordId != String.Empty )
        {
          if ( this._RecordId.Contains ( "AE" ) == true )
          {
            this._Design.TypeId = EvFormRecordTypes.Adverse_Event_Report;
          }
          if ( this._RecordId.Contains ( "SA" ) == true )
          {
            this._Design.TypeId = EvFormRecordTypes.Serious_Adverse_Event_Report;
          }
          if ( this._RecordId.Contains ( "CM" ) == true )
          {
            this._Design.TypeId = EvFormRecordTypes.Concomitant_Medication;
          }
          if ( this._RecordId.Contains ( "TS" ) == true )
          {
            this._Design.TypeId = EvFormRecordTypes.Subject_Record;
          }
          if ( this._RecordId.Contains ( "R" ) == true )
          {
            this._Design.TypeId = EvFormRecordTypes.Trial_Record;
          }
        }
      }
    }

    private bool _hasSignature = false;
    /// <summary>
    /// This property contains identified  patient content signature exists.
    /// </summary>
    public bool hasSignature
    {
      get
      {
        return this._hasSignature;
      }
      set
      {
        this._hasSignature = value;
      }
    }

    /// <summary>
    /// This property contains the form layout enumeration as a string.
    /// </summary>
    public String FormLayout { get; set; }

    private FormAccessRoles _FormAccessRole = FormAccessRoles.Null;

    /// <summary>
    /// This property defines the form user access role.
    /// </summary>
    public FormAccessRoles FormAccessRole
    {
      get { return _FormAccessRole; }
      set { _FormAccessRole = value; }
    }



    private EvFormObjectStates _State = EvFormObjectStates.Null;
    /// <summary>
    /// This property contains a state of a form. 
    /// </summary>
    public EvFormObjectStates State
    {
      get
      {

        return this._State;
      }
      set
      {
        this._State = value;
      }
    }

    /// <summary>
    /// This property contains a state description of a form. 
    /// </summary>
    public string StateDesc
    {
      get
      {
        return EvcStatics.Enumerations.enumValueToString ( this._State );
      }
      set
      {
        string NUll = value;
      }
    }

    private QueryStates _QueryState = QueryStates.None;
    /// <summary>
    /// This property contains a query state of a form. 
    /// </summary>
    public QueryStates QueryState
    {
      get
      {
        return this._QueryState;
      }
      set
      {
        this._QueryState = value;
      }
    }

    /// <summary>
    /// This property contains a query state description of a form. 
    /// </summary>
    public string QueryStateDesc
    {
      get
      {
        switch ( ( int ) this._QueryState )
        {
          case ( int ) EvForm.QueryStates.Open:
            {
              return "Open Record Query";
            }
          case ( int ) EvForm.QueryStates.Closed:
            {
              return "Closed Record Query";
            }
          default:
            {
              return "Record not queried";
            }
        }
      }
      set
      {
        string NUll = value;
      }
    }

    private string _ReferenceId = String.Empty;
    /// <summary>
    /// This property contains a reference identifier of a form. 
    /// </summary>
    public string ReferenceId
    {
      get
      {
        return this._ReferenceId;
      }
      set
      {
        this._ReferenceId = value;
      }
    }

    private DateTime _RecordDate = EvcStatics.CONST_DATE_NULL;
    /// <summary>
    /// This property contains a record date of a form. 
    /// </summary>
    public DateTime RecordDate
    {
      get
      {
        return this._RecordDate;
      }
      set
      {
        this._RecordDate = value;
      }
    }

    /// <summary>
    /// This property contains a record date string of a form. 
    /// </summary>
    public string stRecordDate
    {
      get
      {
        if ( this._RecordDate > EvcStatics.CONST_DATE_NULL )
        {
          return this._RecordDate.ToString ( "dd MMM yyyy" );
        }
        return String.Empty;
      }
      set
      {
        if ( value == String.Empty )
        {
          this._RecordDate = EvcStatics.CONST_DATE_NULL;
          return;
        }
        DateTime date = this._RecordDate;

        if ( DateTime.TryParse ( value, out date ) == true )
        {
          this._RecordDate = date;
        }
      }
    }

    private DateTime _VisitDate = EvcStatics.CONST_DATE_NULL;
    /// <summary>
    /// This property contains the subject visit date.
    /// </summary>
    public DateTime VisitDate
    {
      get
      {
        return this._VisitDate;
      }
      set
      {
        this._VisitDate = value;
      }
    }

    /// <summary>
    /// This property contains a visit date string of a form. 
    /// </summary>
    public string stVisitDate
    {
      get
      {
        if ( this._VisitDate > EvcStatics.CONST_DATE_NULL )
        {
          return this._VisitDate.ToString ( "dd MMM yyyy" );
        }
        return String.Empty;
      }
      set
      {
        if ( value == String.Empty )
        {
          this._VisitDate = EvcStatics.CONST_DATE_NULL;
          return;
        }
        DateTime date = this._VisitDate;

        if ( DateTime.TryParse ( value, out date ) == true )
        {
          this._VisitDate = date;
        }
      }
    }

    /// <summary>
    /// This property contains a title of a form. 
    /// </summary>
    public string Title
    {
      get
      {
        return this.Design.Title;
      }
    }

    private string _Description = String.Empty;
    /// <summary>
    /// This property contains a description of a form containing update notes. 
    /// </summary>
    public string Description
    {
      get
      {
        return this._Description;
      }
      set
      {
        this._Description = value;
      }
    }


    private UpdateReasonList _UpdateReason = UpdateReasonList.Minor_Update;
    /// <summary>
    /// This property contains an update reason enumerated value 
    /// </summary>
    public UpdateReasonList UpdateReason
    {
      get
      {
        return this._UpdateReason;
      }
      set
      {
        this._UpdateReason = value;
      }
    }

    private string _RecordSubject = String.Empty;
    /// <summary>
    /// This property contains a record subject of a form. 
    /// </summary>
    public string RecordSubject
    {
      get
      {
        return this._RecordSubject;
      }
      set
      {
        this._RecordSubject = value;
      }
    }

    private DateTime _StartDate = EvcStatics.CONST_DATE_NULL;
    /// <summary>
    /// This property contains a start date of a form. 
    /// </summary>
    public DateTime StartDate
    {
      get
      {
        return this._StartDate;
      }
      set
      {
        this._StartDate = value;
      }
    }

    /// <summary>
    /// This property contains a start date string of a form. 
    /// </summary>
    public string stStartDate
    {
      get
      {
        if ( this._StartDate > EvcStatics.CONST_DATE_NULL )
        {
          return this._StartDate.ToString ( "dd MMM yyyy" );
        }
        return String.Empty;
      }
      set
      {
        if ( value == String.Empty )
        {
          this._StartDate = EvcStatics.CONST_DATE_NULL;
          return;
        }
        DateTime date = this._StartDate;

        if ( DateTime.TryParse ( value, out date ) == true )
        {
          this._StartDate = date;
        }
      }
    }

    private DateTime _FinishDate = EvcStatics.CONST_DATE_NULL;
    /// <summary>
    /// This property contains a finish date of a form. 
    /// </summary>
    public DateTime FinishDate
    {
      get
      {
        return this._FinishDate;
      }
      set
      {
        this._FinishDate = value;
      }
    }

    /// <summary>
    /// This property contains a finish date string of a form. 
    /// </summary>
    public string stFinishDate
    {
      get
      {
        if ( this._FinishDate > EvcStatics.CONST_DATE_COMPLETED )
        {
          return this._FinishDate.ToString ( "dd MMM yyyy" );
        }
        return String.Empty;
      }
      set
      {
        if ( value == String.Empty )
        {
          this._FinishDate = EvcStatics.CONST_DATE_NULL;
          return;
        }
        DateTime date = this._FinishDate;

        if ( DateTime.TryParse ( value, out date ) == true )
        {
          this._FinishDate = date;
        }
      }
    }

    /// <summary>
    /// This property indicates whether a common record of a form is completed. 
    /// </summary>
    public bool CommonRecordCompleted
    {
      get
      {
        if ( this._FinishDate > EvcStatics.CONST_DATE_COMPLETED )
        {
          return true;
        }
        return false;
      }
      set
      {
        bool a = value;
      }
    }

    private List<EvFormRecordComment> _CommentList = new List<EvFormRecordComment> ( );
    /// <summary>
    /// This property contains a comment list of a form. 
    /// </summary>
    public List<EvFormRecordComment> CommentList
    {
      get
      {
        return this._CommentList;
      }
      set
      {
        this._CommentList = value;
      }
    }

    /// <summary>
    /// This property indicates if the form is in Anonymous Access mode.
    /// </summary>
    public bool PatientAccess { get; set; }

    private string _Authors = String.Empty;
    /// <summary>
    /// This property contains authors of a form. 
    /// </summary>
    public string Authors
    {
      get
      {
        return this._Authors;
      }
      set
      {
        this._Authors = value;
      }
    }

    private string _AuthoredBy = String.Empty;
    /// <summary>
    /// This property contains a user who authorizes a form. 
    /// </summary>
    public string AuthoredBy
    {
      get
      {
        return this._AuthoredBy;
      }
      set
      {
        this._AuthoredBy = value;
      }
    }

    private DateTime _AuthoredDate = EvcStatics.CONST_DATE_NULL;
    /// <summary>
    /// This property contains an authorized date of a form. 
    /// </summary>
    public DateTime AuthoredDate
    {
      get
      {
        return this._AuthoredDate;
      }
      set
      {
        this._AuthoredDate = value;
      }
    }

    /// <summary>
    /// This property contains an authorized date string of a form. 
    /// </summary>
    public string stAuthoredDate
    {
      get
      {
        if ( this._AuthoredDate > EvcStatics.CONST_DATE_NULL )
        {
          return this._AuthoredDate.ToString ( "dd MMM yyyy" );
        }
        return String.Empty;
      }
      set
      {
        DateTime Null = this._AuthoredDate;
      }
    }

    private string _QueriedBy = String.Empty;
    /// <summary>
    /// This property contains a user who queries a form. 
    /// </summary>
    public string QueriedBy
    {
      get
      {
        return this._QueriedBy;
      }
      set
      {
        this._QueriedBy = value;
      }
    }

    private DateTime _QueriedDate = EvcStatics.CONST_DATE_NULL;
    /// <summary>
    /// This property contains a queried date of a form. 
    /// </summary>
    public DateTime QueriedDate
    {
      get
      {
        return this._QueriedDate;
      }
      set
      {
        this._QueriedDate = value;
      }
    }

    /// <summary>
    /// This property contains a queried date string of a form. 
    /// </summary>
    public string stQueriedDate
    {
      get
      {
        if ( this._QueriedDate > EvcStatics.CONST_DATE_NULL )
        {
          return this._QueriedDate.ToString ( "dd MMM yyyy" );
        }
        return String.Empty;
      }
      set
      {
        string Null = value;
      }
    }

    private string _ReviewedBy = String.Empty;
    /// <summary>
    /// This property contains a user who reviewed a form. 
    /// </summary>
    public string ReviewedBy
    {
      get
      {
        return this._ReviewedBy;
      }
      set
      {
        this._ReviewedBy = value;
      }
    }

    private DateTime _ReviewedDate = EvcStatics.CONST_DATE_NULL;
    /// <summary>
    /// This property contains a reviewed date of a form. 
    /// </summary>
    public DateTime ReviewedDate
    {
      get
      {
        return this._ReviewedDate;
      }
      set
      {
        this._ReviewedDate = value;
      }
    }

    /// <summary>
    /// This property contains a reviewed date string of a form. 
    /// </summary>
    public string stReviewedDate
    {
      get
      {
        if ( this._ReviewedDate > EvcStatics.CONST_DATE_NULL )
        {
          return this._ReviewedDate.ToString ( "dd MMM yyyy" );
        }
        return String.Empty;
      }
      set
      {
        string stDate = value;
      }
    }

    private string _ApprovedBy = String.Empty;
    /// <summary>
    /// This property contains a user who approved a form. 
    /// </summary>
    public string ApprovedBy
    {
      get
      {
        return this._ApprovedBy;
      }
      set
      {
        this._ApprovedBy = value;
      }
    }

    private DateTime _ApprovalDate = EvcStatics.CONST_DATE_NULL;
    /// <summary>
    /// This property contains an approval date of a form. 
    /// </summary>
    public DateTime ApprovalDate
    {
      get
      {
        return this._ApprovalDate;
      }
      set
      {
        this._ApprovalDate = value;
      }
    }

    /// <summary>
    /// This property contains an approaval date string of a form. 
    /// </summary>
    public string stApprovalDate
    {
      get
      {
        if ( this._ApprovalDate > EvcStatics.CONST_DATE_NULL )
        {
          return this._ApprovalDate.ToString ( "dd MMM yyyy" );
        }
        return String.Empty;
      }
      set
      {
        string stDate = value;
      }
    }

    private string _SignoffStatement = String.Empty;
    /// <summary>
    /// This property contains a sign off statement of a form. 
    /// </summary>
    public string SignoffStatement
    {
      get
      {
        return this._SignoffStatement;
      }
      set
      {
        this._SignoffStatement = value;
      }
    }


    String _cDashMetadata = String.Empty;
    /// <summary>
    /// This property contains cDash metadata values. 
    /// </summary>
    public string cDashMetadata
    {
      get
      {
        return this._cDashMetadata;
      }
      set
      {
        this._cDashMetadata = value;
      }
    }

    private string _Monitor = String.Empty;
    /// <summary>
    /// The property indicates whether a record has been monitored.
    /// </summary>
    public bool IsMoniotored
    {
      get
      {
        if ( this._Monitor == String.Empty )
        {
          return false;
        }
        return true;
      }
    }

    /// <summary>
    /// This property contains a monitor of a form. 
    /// </summary>
    public string Monitor
    {
      get
      {
        return this._Monitor;
      }
      set
      {
        this._Monitor = value;
      }
    }

    private DateTime _MonitorDate = EvcStatics.CONST_DATE_NULL;
    /// <summary>
    /// This property contains a monitor date of a form. 
    /// </summary>
    public DateTime MonitorDate
    {
      get
      {
        return this._MonitorDate;
      }
      set
      {
        this._MonitorDate = value;
      }
    }

    /// <summary>
    /// This property contains a monitor date string of a form. 
    /// </summary>
    public string stMonitorDate
    {
      get
      {
        if ( this._MonitorDate > EvcStatics.CONST_DATE_NULL )
        {
          return this._MonitorDate.ToString ( "dd MMM yyyy" );
        }
        return String.Empty;
      }
      set
      {
        string stDate = value;
      }
    }

    private string _Updated = String.Empty;
    /// <summary>
    /// This property contains an update string of a form. 
    /// </summary>
    public string Updated
    {
      get
      {
        return this._Updated;
      }
      set
      {
        this._Updated = value;
      }
    }

    private string _BookedOutBy = String.Empty;
    /// <summary>
    /// This property contains a user who books out a form. 
    /// </summary>
    public string BookedOutBy
    {
      get
      {
        return this._BookedOutBy;
      }
      set
      {
        this._BookedOutBy = value;
      }
    }


    private bool _Selected = false;
    /// <summary>
    /// This property indicates whether a form is selected. 
    /// </summary>
    public bool Selected
    {
      get
      {
        return this._Selected;
      }
      set
      {
        this._Selected = value;
      }
    }

    private bool _IsQueried = false;
    /// <summary>
    /// This property indicates wheter a form is queried. 
    /// </summary>
    public bool IsQueried
    {
      get
      {
        return this._IsQueried;
      }
      set
      {
        this._IsQueried = value;
      }
    }

    /// <summary>
    /// This property indicates whether a form has queried items. 
    /// </summary>
    public bool hasQueredItems
    {
      get
      {
        //
        // Check that the static fields have not been queried.
        //
        if ( this._RecordContent.RecordSubject.Queried == true )
        {
          return true;
        }
        if ( this._RecordContent.StartDate.Queried == true )
        {
          return true;
        }
        if ( this._RecordContent.FinishDate.Queried == true )
        {
          return true;
        }
        if ( this._RecordContent.ReferenceId.Queried == true )
        {
          return true;
        }


        // 
        // Iterate through the EvForm Items to set their state
        // 
        for ( int Row = 0; Row < this._Fields.Count; Row++ )
        {
          // 
          // if a field is queried return true.
          // 
          if ( this._Fields [ Row ].State == EvFormField.FieldStates.Queried )
          {
            return true;

          }//END Switch

        }//END field interation loop

        return false;
      }
      set
      {
        bool dummy = value;
      }
    }

    /// <summary>
    /// This property indicates whether a forms fields are empty. 
    /// </summary>
    public bool isEmpty
    {
      get
      {
        //
        // Check that the static fields have not been queried.
        //
        if ( this._Design.TypeId == EvFormRecordTypes.Adverse_Event_Report
          || this._Design.TypeId == EvFormRecordTypes.Concomitant_Medication
          || this._Design.TypeId == EvFormRecordTypes.Serious_Adverse_Event_Report )
        {
          if ( this._RecordSubject != String.Empty )
          {
            return false;
          }
          if ( this._StartDate != EvcStatics.CONST_DATE_NULL )
          {
            return false;
          }
        }


        // 
        // Iterate through the EvForm Items to set their state
        // 
        for ( int Row = 0; Row < this._Fields.Count; Row++ )
        {
          if ( this._Fields [ Row ].TypeId != EvDataTypes.Table
            || this._Fields [ Row ].TypeId != EvDataTypes.Special_Matrix )
          {
            // 
            // if a field is queried return true.
            // 
            if ( ( this._Fields [ Row ].TypeId == EvDataTypes.Free_Text
                || this._Fields [ Row ].TypeId == EvDataTypes.Signature )
              && this._Fields [ Row ].ItemText != String.Empty )
            {
              return false;
            }
            else
            {
              if ( this._Fields [ Row ].ItemValue != String.Empty )
              {
                return false;

              }
            }
          }
          else
          {
            EvFormField field = this._Fields [ Row ];
            foreach ( EvFormFieldTableRow tableRow in field.Table.Rows )
            {
              for ( int col = 0; col < tableRow.Column.Length; col++ )
              {
                if ( field.Table.Header [ col ].TypeId != EvFormFieldTableColumnHeader.ItemTypeMatrix
                  && tableRow.Column [ col ] != String.Empty )
                {
                  return false;
                }
              }
            }
          }

        }//END field interation loop

        return true;
      }
    }
    /// <summary>
    /// This property indicates whether a forms mandatory fields have values items. 
    /// </summary>
    public bool hasMandatoryFieldValues
    {
      get
      {
        //
        // Check that the static fields have not been queried.
        //
        if ( this._Design.TypeId == EvFormRecordTypes.Adverse_Event_Report
          || this._Design.TypeId == EvFormRecordTypes.Concomitant_Medication
          || this._Design.TypeId == EvFormRecordTypes.Serious_Adverse_Event_Report )
        {
          if ( this._RecordSubject == String.Empty )
          {
            return false;
          }
          if ( this._StartDate == EvcStatics.CONST_DATE_NULL )
          {
            return false;
          }
        }

        // 
        // Iterate through the EvForm Items to set their state
        // 
        for ( int Row = 0; Row < this._Fields.Count; Row++ )
        {
          if ( this._Fields [ Row ].TypeId == EvDataTypes.Signature )
          {
            // 
            // if a signature field is mandatory and empty
            // 
            if ( this._Fields [ Row ].Design.Mandatory == true
              && this._Fields [ Row ].ItemText == String.Empty )
            {
              return false;

            }//END
          }
          else
          {
            if ( this._Fields [ Row ].TypeId == EvDataTypes.Table
              || this._Fields [ Row ].TypeId == EvDataTypes.Special_Matrix )
            {
              EvFormField field = this._Fields [ Row ];
              bool bValue = false;
              foreach ( EvFormFieldTableRow tableRow in field.Table.Rows )
              {
                for ( int col = 0; col < tableRow.Column.Length; col++ )
                {
                  if ( field.Table.Header [ col ].TypeId != EvFormFieldTableColumnHeader.ItemTypeMatrix
                    && tableRow.Column [ col ] != String.Empty )
                  {
                    bValue = true;
                  }
                }
              }

              if ( bValue == false )
              {
                return false;
              }
            }
            else
            {
              // 
              // if a field is queried return true.
              // 
              if ( this._Fields [ Row ].Design.Mandatory == true
                && this._Fields [ Row ].ItemValue == String.Empty )
              {
                return false;

              }//END
            }
          }

        }//END field interation loop

        return true;
      }
      set
      {
        bool dummy = value;
      }
    }

    // 
    // Display fields.	
    // 
    private string _Organisation = String.Empty;
    /// <summary>
    /// This property contains an organization of a form.
    /// </summary>
    public string Organisation
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

    private string _VisitTitle = String.Empty;
    /// <summary>
    /// This property contains a visit title of a form. 
    /// </summary>
    public string VisitTitle
    {
      get
      {
        return this._VisitTitle;
      }
      set
      {
        this._VisitTitle = value;
      }
    }

    /// <summary>
    /// This property contains an external identifier of a form. 
    /// </summary>
    public string ExternalId
    {
      get
      {
        return this._ExternalId;
      }
      set
      {
        this._ExternalId = value;
      }
    }

    private string _ExternalId = String.Empty;
    /// <summary>
    /// This property contains a version of a form. 
    /// </summary>
    public string Version
    {
      get
      {
        return this.Design.stVersion;
      }
    }

    /// <summary>
    /// This property contains a type of a form. 
    /// </summary>
    public EvFormRecordTypes TypeId
    {
      get
      {
        return this.Design.TypeId;
      }
    }

    /// <summary>
    /// This property indicates if the form is a patient consent form 
    /// </summary>
    public bool isConsentForm
    {
      get
      {
        switch ( this.Design.TypeId )
        {
          case EvFormRecordTypes.Informed_Consent:
          //case EvFormRecordTypes.Informed_Consent_1:
          //case EvFormRecordTypes.Informed_Consent_2:
          //case EvFormRecordTypes.Informed_Consent_3:
          //case EvFormRecordTypes.Informed_Consent_4:
            {
              return true;
            }
        }
        return false;
      }
    }

    private string _AlertOrgId = String.Empty;
    /// <summary>
    /// This property contains an alert organization identifier of a form. 
    /// </summary>
    public string AlertOrgId
    {
      get
      {
        return this._AlertOrgId;
      }
      set
      {
        this._AlertOrgId = value;
      }
    }

    private string _AlertUserId = String.Empty;
    /// <summary>
    /// This property contains an alert user identifier of a form. 
    /// </summary>
    public string AlertUserId
    {
      get
      {
        return this._AlertUserId;
      }
      set
      {
        this._AlertUserId = value;
      }
    }

    private string _AuthoredByUserId = String.Empty;
    /// <summary>
    /// This property contains a user identifier who author a form. 
    /// </summary>
    public string AuthoredByUserId
    {
      get
      {
        return this._AuthoredByUserId;
      }
      set
      {
        this._AuthoredByUserId = value;
      }
    }

    private string _QueriedByUserId = String.Empty;
    /// <summary>
    /// This property contains a user identifier who queries a form. 
    /// </summary>
    public string QueriedByUserId
    {
      get
      {
        return this._QueriedByUserId;
      }
      set
      {
        this._QueriedByUserId = value;
      }
    }

    private string _ReviewedByUserId = String.Empty;
    /// <summary>
    /// This property contains a user identifier who reviews a form. 
    /// </summary>
    public string ReviewedByUserId
    {
      get
      {
        return this._ReviewedByUserId;
      }
      set
      {
        this._ReviewedByUserId = value;
      }
    }

    private string _ApprovedByUserId = String.Empty;
    /// <summary>
    /// This property contains a user identifier who approves a form. 
    /// </summary>
    public string ApprovedByUserId
    {
      get
      {
        return this._ApprovedByUserId;
      }
      set
      {
        this._ApprovedByUserId = value;
      }
    }

    private string _MonitorUserId = String.Empty;
    /// <summary>
    /// This property contains a user identifier who monitors a form. 
    /// </summary>
    public string MonitorUserId
    {
      get
      {
        return this._MonitorUserId;
      }
      set
      {
        this._MonitorUserId = value;
      }
    }

    private string _UserId = String.Empty;
    /// <summary>
    /// This property contains a user identifier of a form. 
    /// </summary>
    public string UserId
    {
      get
      {
        return this._UserId;
      }
      set
      {
        this._UserId = value;
      }
    }

    private string _UpdatedByUserId = String.Empty;
    /// <summary>
    /// This property contains a user identifier who updates a form. 
    /// </summary>
    public string UpdatedByUserId
    {
      get
      {
        return this._UpdatedByUserId;
      }
      set
      {
        this._UpdatedByUserId = value;
      }
    }

    private EvEventCodes _EventCode = EvEventCodes.Ok;
    /// <summary>
    /// This property contains an event code object of a form. 
    /// </summary>
    public EvEventCodes EventCode
    {
      get
      {
        return this._EventCode;
      }
      set
      {
        this._EventCode = value;
      }
    }

    private SaveActionCodes _SaveAction = SaveActionCodes.Save;
    /// <summary>
    /// This property contains an action of a form. 
    /// </summary>
    public SaveActionCodes SaveAction
    {
      get
      {
        return this._SaveAction;
      }
      set
      {
        this._SaveAction = value;

        if ( this._SaveAction == EvForm.SaveActionCodes.Save )
        {
          this._SaveAction = EvForm.SaveActionCodes.Form_Saved;
        }
      }
    }

    private string _UserCommonName = String.Empty;
    /// <summary>
    /// This property contains a user common name of a form. 
    /// </summary>
    public string UserCommonName
    {
      get
      {
        return this._UserCommonName;
      }
      set
      {
        this._UserCommonName = value;
      }
    }

    private bool _IsAuthenticatedSignature = false;
    /// <summary>
    /// This property indicates whether a form has authenticated signature. 
    /// </summary>
    public bool IsAuthenticatedSignature
    {
      get
      {
        return this._IsAuthenticatedSignature;
      }
      set
      {
        this._IsAuthenticatedSignature = value;
      }
    }

    private FormDisplayStates _FormDisplayState = FormDisplayStates.Display;
    /// <summary>
    /// This property contains a form display state object of a form. 
    /// </summary>
    public FormDisplayStates FormDisplayState
    {
      get
      {
        return this._FormDisplayState;
      }
      set
      {
        this._FormDisplayState = value;
      }
    }

    private string _ScriptMessage = String.Empty;
    /// <summary>
    /// This property contains a form message of a form. 
    /// </summary>
    public string ScriptMessage
    {
      get
      {
        return this._ScriptMessage;
      }
      set
      {
        this._ScriptMessage = value;
      }
    }

    private string _NextSection = String.Empty;
    /// <summary>
    /// This property contains a next selection of a form. 
    /// </summary>
    public string NextSection
    {
      get
      {
        return this._NextSection;
      }
      set
      {
        this._NextSection = value;
      }
    }

    private string _CurrentSection = String.Empty;
    /// <summary>
    /// This property contains a current selection of a form. 
    /// </summary>
    public string CurrentSection
    {
      get
      {
        return this._CurrentSection;
      }
      set
      {
        this._CurrentSection = value;
      }
    }

    private bool _AllSectionsVisible = true;
    /// <summary>
    /// This property indicates whether all selections are visible in a form. 
    /// </summary>
    public bool AllSectionsVisible
    {
      get
      {
        return this._AllSectionsVisible;
      }
      set
      {
        this._AllSectionsVisible = value;
      }
    }

    private float _AppVersion = 2.002001F;
    /// <summary>
    /// This property contains an application vertion of a form. 
    /// </summary>
    public float AppVersion
    {
      get
      {
        return this._AppVersion;
      }
      set
      {
        this._AppVersion = value;
      }
    }

    /// <summary>
    /// This property generates a summary of field values.
    /// </summary>
    public String FieldSummary
    {
      get
      {
        return createSubjectSummary ( );
      }
    }

    /// <summary>
    /// This property indicates if the form record has signature fields.
    /// </summary>
    public bool hasSignatureFields
    {
      get
      {
        //
        // Iterate through the fields looking for a signature field.
        //
        foreach ( EvFormField field in this._Fields )
        {
          if ( field.TypeId == EvDataTypes.Signature )
          {
            return true;
          }
        }

        return false;
      }
    }

    //===================================================================================
    /// <summary>
    /// This property indicates if the signature fields have values.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public bool hasAllSignatureFieldValues
    {
      get
      {
        int signatureFieldCount = 0;
        int signatureCount = 0;
        //
        // Iterate through the fields looking for a signature field.
        //
        foreach ( EvFormField field in this._Fields )
        {
          if ( field.TypeId == EvDataTypes.Signature )
          {
            signatureFieldCount++;
          }

          EvSignatureBlock signatureBlock = Newtonsoft.Json.JsonConvert.DeserializeObject<EvSignatureBlock> ( field.ItemText );

          if ( signatureBlock == null )
          {
            continue;
          }
          //
          // if the signature raster array count greater than 0 a signature has been captured.
          //
          if ( signatureBlock.Signature.Count > 0 )
          {
            signatureCount++;
          }
        }

        if ( signatureFieldCount > 0
          && signatureFieldCount == signatureCount )
        {
          return true;
        }
        return false;
      }
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region class methods

    // ==================================================================================
    /// <summary>
    /// Ttis method updates formId if has not been set.
    /// </summary>
    //  ----------------------------------------------------------------------------------
    public void updateCommonFormId ( )
    {
      if ( this.FormId != String.Empty )
      {
        return;
      }

     this._FormId =  EvForm.getCommonFormId ( this.TypeId );

    }//END updateObjectValue method.

    //  =================================================================================
    /// <summary>
    ///  This method updates the form record state based on current data content.
    /// </summary>
    //  ---------------------------------------------------------------------------------
    public void updateFormState ( )
    {
      //
      // No change if the object is any of the following states.
      //
      if ( this._State == EvFormObjectStates.Queried_Record
        || this._State == EvFormObjectStates.Queried_Record_Copy
        || this._State == EvFormObjectStates.Source_Data_Verified
        || this._State == EvFormObjectStates.Locked_Record
        || this._State == EvFormObjectStates.Null )
      {
        return;
      }

      if ( this._Fields.Count == 0 )
      {
        return;
      }

      if ( this.isEmpty == true )
      {
        this._State = EvFormObjectStates.Empty_Record;
      }
      else
      {
        if ( this._State == EvFormObjectStates.Empty_Record )
        {
          this._State = EvFormObjectStates.Draft_Record;
        }
      }

      if ( this.State == EvFormObjectStates.Submitted_Record
        && this.hasMandatoryFieldValues == false )
      {
        this.State = EvFormObjectStates.Draft_Record;
      }

      if ( ( this.State == EvFormObjectStates.Draft_Record
          || this._State == EvFormObjectStates.Empty_Record )
        && ( this.hasMandatoryFieldValues == true ) )
      {
        this.State = EvFormObjectStates.Completed_Record;
      }

    }//END updateFormState method

    //=====================================================================================
    /// <summary>
    /// This property generates a summary of field values.
    /// </summary>
    //-------------------------------------------------------------------------------------
    public String createSubjectSummary ( )
    {
      //
      // Initialise the methods variables and objects.
      //
      String htmlText = String.Empty;

      //
      // Get record header information.
      //
      htmlText += this.getSummaryField ( "Form Title", this._Design.Title );
      htmlText += this.getSummaryField ( "Record Date", this.stRecordDate );
      htmlText += this.getSummaryField ( "Record State", this.StateDesc );

      //
      // Iterate through the form fields to extract the fields that are form summary fields.
      //
      foreach ( EvFormField field in this._Fields )
      {
        if ( field.Design.SummaryField == true )
        {
          htmlText += this.getSummaryField ( field.Design.Title, field.ItemValue );
        }
      }

      if ( htmlText != String.Empty )
      {
        htmlText = " <table class='tblSummary' style='margin: 0;' > " + htmlText + "</table>";
      }
      else
      {
        htmlText = "<p>No field summary values.</p>\r\n";
      }


      //
      // Return the generated html text.
      //
      return htmlText;

    }//END Get Field summary

    // =======================================================================================
    /// <summary>
    /// This class returns a field summary entry
    /// </summary>
    /// <param name="Name">String: a field name</param>
    /// <param name="FieldValue">String: a field value</param>
    /// <returns>String: HTml string</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Validate whether the FieldValue is not empty
    /// 
    /// 2. Return a html string with Name and FieldValue
    /// </remarks>
    // ----------------------------------------------------------------------------------------
    private String getSummaryField ( 
      String Name, 
      String FieldValue )
    {
      if ( FieldValue != String.Empty )
      {
        return "<tr><td class='Prompt Width_40'>" + Name + ": "
                + "</td><td>" + FieldValue + "</td></tr>\r\n";
      }
      return String.Empty;
    }

    //=====================================================================================
    /// <summary>
    /// This method sets the Form Role property 
    /// </summary>
    /// <param name="UserProfile">EvUserProfile Object</param>
    //-------------------------------------------------------------------------------------
    public void setFormRole ( EvUserProfile UserProfile )
    {
      //
      // Swtich roleid to select the form role for the user.
      //
      switch ( UserProfile.RoleId )
      {
        case EvRoleList.Patient:
          {
            if ( this.State == EvFormObjectStates.Empty_Record
              || this.State == EvFormObjectStates.Draft_Record
              || this.State == EvFormObjectStates.Completed_Record )
            {
              this.FormAccessRole = EvForm.FormAccessRoles.Patient;
              break;
            }

            this.FormAccessRole = EvForm.FormAccessRoles.Record_Reader;
            break;
          }
        case EvRoleList.Site_User:
        case EvRoleList.Trial_Coordinator:
          {
            if ( UserProfile.hasRecordEditAccess == true
              && ( this.State == EvFormObjectStates.Empty_Record
                || this.State == EvFormObjectStates.Draft_Record
                || this.State == EvFormObjectStates.Completed_Record
                || this.State == EvFormObjectStates.Submitted_Record
                || this.State == EvFormObjectStates.Queried_Record ) )
            {
              this.FormAccessRole = EvForm.FormAccessRoles.Record_Author;
              break;
            }

            this.FormAccessRole = EvForm.FormAccessRoles.Record_Reader;
            break;
          }
        case EvRoleList.Monitor:
          {
            if ( this.State == EvFormObjectStates.Submitted_Record
              || this.State == EvFormObjectStates.Source_Data_Verified )
            {
              this.FormAccessRole = EvForm.FormAccessRoles.Monitor;
              break;
            }
            this.FormAccessRole = EvForm.FormAccessRoles.Record_Reader;
            break;
          }
        case EvRoleList.Data_Manager:
          {
            if ( this.State == EvFormObjectStates.Submitted_Record
              || this.State == EvFormObjectStates.Source_Data_Verified
              || this.State == EvFormObjectStates.Locked_Record )
            {
              this.FormAccessRole = EvForm.FormAccessRoles.Data_Manager;
              break;
            }
            this.FormAccessRole = EvForm.FormAccessRoles.Record_Reader;
            break;
          }
        default:
          {
            this.FormAccessRole = EvForm.FormAccessRoles.Record_Reader;
            break;
          }

      }//END switch statement


    }//END setFormRole method

    //  =================================================================================
    /// <summary>
    /// This method sort the field into section field number order.
    /// </summary>
    //  ---------------------------------------------------------------------------------
    public void sortFields ( )
    {
      List<EvFormField> fieldList = new List<EvFormField> ( );
      int order = 1;
      string stSectionindex = String.Empty;
      const string CONST_DELIMITER = "~^";

      //
      // Iterate through the section adding the fields in section in order of occurance.
      //
      foreach ( EvFormSection section in this._Design.FormSections )
      {
        //
        // Build a list of section indexes
        //
        stSectionindex += section.No + CONST_DELIMITER
          + section.Title.Trim ( ) + CONST_DELIMITER;

        foreach ( EvFormField field in this._Fields )
        {
          if ( field.Design.Section == section.No.ToString ( )
            || field.Design.Section.Trim ( ) == section.Title.Trim ( ) )
          {
            field.Design.Section = section.Title;
            field.Order = order;

            order++;
            order++;

            fieldList.Add ( field );

          }//END field selction

        }//END field iteration loop

      }//END section iteration loop 

      //
      // Add the fields that do not have a section.
      //
      foreach ( EvFormField field in this._Fields )
      {
        string section = field.Design.Section.Trim ( );

        if ( stSectionindex.Contains ( section + CONST_DELIMITER ) == false )
        {
          field.Design.Section = String.Empty;
          field.Order = order;

          order++;
          order++;

          fieldList.Add ( field );

        }//END field selction

      }//END field iteration loop

      //
      // Update the field list.
      //
      this._Fields = fieldList;

    }//END sortfields method

    // =====================================================================================
    /// <summary>
    /// This class provides a list of trial types.
    /// </summary>
    /// <param name="FieldName">string: a field name</param>
    /// <returns>string: a value based on the Fieldname</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize an internal fieldname as enumerated object
    /// 
    /// 2. Try convert a string FieldName into an enumerated object fieldname
    /// 
    /// 3. Switch fieldname and update value for the property defined by form class field names.
    /// </remarks>
    //  ------------------------------------------------------------------------------------
    public string getValue ( string FieldName )
    {
      // 
      // Initialise the fieldname as enumerated object
      // 
      FormClassFieldNames fieldname = FormClassFieldNames.Null;

      //
      // Try convert the FieldName into fieldname 
      //
      try
      {
        fieldname = (FormClassFieldNames) Enum.Parse ( typeof ( FormClassFieldNames ), FieldName );
      }
      catch
      {
        fieldname = FormClassFieldNames.Null;
      }

      return getValue ( fieldname );
    }

    // =====================================================================================
    /// <summary>
    /// This class provides a list of trial types.
    /// </summary>
    /// <param name="FieldName">string: a field name</param>
    /// <returns>string: a value based on the Fieldname</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize an internal fieldname as enumerated object
    /// 
    /// 2. Try convert a string FieldName into an enumerated object fieldname
    /// 
    /// 3. Switch fieldname and update value for the property defined by form class field names.
    /// </remarks>
    //  ------------------------------------------------------------------------------------
    public string getValue ( FormClassFieldNames FieldName )
    {
      // 
      // Switch to the fieldname and return the related value.
      // 
      switch ( FieldName )
      {
        case FormClassFieldNames.ProjectId:
          return this._TrialId;

        case FormClassFieldNames.FormId:
          return this.FormId;

        case FormClassFieldNames.MilestoneId:
          return this._MilestoneId;

        case FormClassFieldNames.OrgId:
          return this._OrgId;

        case FormClassFieldNames.SubjectId:
          return this._SubjectId;

        case FormClassFieldNames.VisitId:
          return this._VisitId;

        case FormClassFieldNames.StartDate:
          return this._StartDate.ToString ( "dd MMM yyyy" );

        case FormClassFieldNames.FinishDate:
          return this._FinishDate.ToString ( "dd MMM yyyy" );

        case FormClassFieldNames.VisitDate:
          return this._VisitDate.ToString ( "dd MMM yyyy" );

        case FormClassFieldNames.RecordSubject:
          return this._RecordSubject;
        /*
      case FormClassFieldNames.Annotation:
        return this._Annotation;

      case FormClassFieldNames.Comments:
        return this._Comments;
        */
        case FormClassFieldNames.SourceId:
          return this._SourceId;

        case FormClassFieldNames.Status:
          return this.StateDesc;

        case FormClassFieldNames.ActivityId:
          return this._ActivityId;

        case FormClassFieldNames.IsMandatory:
          return this._IsMandatory.ToString ( );

        case FormClassFieldNames.IsMandatoryActivity:
          return this._IsMandatoryActivity.ToString ( );

        case FormClassFieldNames.Title:
          return this._Design.Title;

        case FormClassFieldNames.Instructions:
          return this._Design.Instructions;

        case FormClassFieldNames.Description:
          {
            return this._Description;
          }

        case FormClassFieldNames.UpdateReason:
          {
            return this._UpdateReason.ToString ( );
          }
        case FormClassFieldNames.FormAccessRole:
          {
            return this._FormAccessRole.ToString ( );
          }

        case FormClassFieldNames.TemplateName:
          return this._Design.TemplateName;

        case FormClassFieldNames.Reference:
          return this._Design.Reference;

        case FormClassFieldNames.FormCategory:
          return this._Design.FormCategory;

        case FormClassFieldNames.RecordSubjectInstructions:
          return this._Design.RecordSubjectInstructions;

        case FormClassFieldNames.StartDateInstructions:
          return this._Design.StartDateInstructions;

        case FormClassFieldNames.FinishDateInstructions:
          return this._Design.FinishDateInstructions;

        case FormClassFieldNames.SubjectScreeningIdInstructions:
          return this._Design.SubjectScreeningIdInstructions;

        case FormClassFieldNames.SubjectSponsorIdInstructions:
          return this._Design.SubjectSponsorIdInstructions;

        case FormClassFieldNames.SubjectRandomisedIdInstructions:
          return this._Design.SubjectRandomisedIdInstructions;

        case FormClassFieldNames.SubjectExternalIdInstructions:
          return this._Design.SubjectExternalIdInstructions;

        case FormClassFieldNames.SubjectExternalIdFormat:
          return this._Design.SubjectExternalIdFormat.ToString ( );

        case FormClassFieldNames.SubjectScreeningIdFormat:
          return this._Design.SubjectScreeningIdFormat.ToString ( );

        case FormClassFieldNames.SubjectSponsorIdFormat:
          return this._Design.SubjectSponsorIdFormat.ToString ( );

        case FormClassFieldNames.SubjectRandomisedIdFormat:
          return this._Design.SubjectRandomisedIdFormat.ToString ( );

        case FormClassFieldNames.TypeId:
          return this._Design.TypeId.ToString ( );

        case FormClassFieldNames.JavaValidationScript:
          return this._Design.JavaValidationScript;

        case FormClassFieldNames.HasCsScript:
          return this._Design.hasCsScript.ToString ( );

        case FormClassFieldNames.OnEdit_HideFieldAnnotation:
          return this._Design.OnEdit_HideFieldAnnotation.ToString ( );

        default:
          {
            return String.Empty;

          }//END Default

      }//END Switch

    }//END getValue method

    // =====================================================================================
    /// <summary>
    ///   This method sets the field value.
    /// </summary>
    /// <param name="FieldName">string: a field name</param>
    /// <param name="Value">string: a value of field name</param>  
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize an internal variables and an enumerated object fieldname
    /// 
    /// 2. Try convert a string FieldName into an enumerated object fieldname
    /// 
    /// 3. Switch fieldname and update value for the property defined by form class field names.
    /// </remarks>
    //  ------------------------------------------------------------------------------------
    public void setValue ( string FieldName, string Value )
    {
      // 
      // Initialise the methods variables including date, float value, integer value, 
      // boolean value and Field name object
      //
      DateTime date = EvcStatics.CONST_DATE_NULL;
      //float fltValue = 0;
      //int intValue = 0;
      //bool bValue = false;
      FormClassFieldNames fieldname = FormClassFieldNames.Null;

      //
      // Try convert the FieldName into a fieldname enumerated object
      //
      try
      {
        fieldname = (FormClassFieldNames) Enum.Parse ( typeof ( FormClassFieldNames ), FieldName );
      }
      catch
      {
        fieldname = FormClassFieldNames.Null;
      }

      this.setValue ( fieldname, Value );
    }

    // =====================================================================================
    /// <summary>
    ///   This method sets the field value.
    /// </summary>
    /// <param name="FieldName">string: a field name</param>
    /// <param name="Value">string: a value of field name</param>  
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize an internal variables and an enumerated object fieldname
    /// 
    /// 2. Try convert a string FieldName into an enumerated object fieldname
    /// 
    /// 3. Switch fieldname and update value for the property defined by form class field names.
    /// </remarks>
    //  ------------------------------------------------------------------------------------
    public void setValue ( FormClassFieldNames FieldName, string Value )
    {
      // 
      // Initialise the methods variables including date, float value, integer value, 
      // boolean value and Field name object
      //
      DateTime date = EvcStatics.CONST_DATE_NULL;

      // 
      // Switch to determine which value to return.
      // 
      switch ( FieldName )
      {
        case FormClassFieldNames.ProjectId:
          {
            this._TrialId = Value;
            return;
          }
        case FormClassFieldNames.FormId:
          {
            this._FormId = Value;
            return;
          }
        case FormClassFieldNames.ActivityId:
          {
            this._ActivityId = Value;
            return;
          }
        case FormClassFieldNames.MilestoneId:
          {
            this._MilestoneId = Value;
            return;
          }
        case FormClassFieldNames.OrgId:
          {
            this._OrgId = Value;
            return;
          }
        case FormClassFieldNames.SubjectId:
          {
            this._SubjectId = Value;
            return;
          }
        case FormClassFieldNames.VisitId:
          {
            this._VisitId = Value;
            return;
          }
        case FormClassFieldNames.StartDate:
          {
            if ( DateTime.TryParse ( Value, out date ) == true )
            {
              this._StartDate = date;
            }
            return;
          }

        case FormClassFieldNames.FinishDate:
          {
            if ( DateTime.TryParse ( Value, out date ) == true )
            {
              this._FinishDate = date;
            }
            return;
          }


        case FormClassFieldNames.VisitDate:
          {
            if ( DateTime.TryParse ( Value, out date ) == true )
            {
              this._VisitDate = date;
            }
            return;
          }

        case FormClassFieldNames.RecordSubject:
          {
            this._RecordSubject = Value;
            return;
          }

        case FormClassFieldNames.Status:
          {
            try
            {
              this._State = (EvFormObjectStates) Enum.Parse ( typeof ( EvFormObjectStates ), Value );
            }
            catch
            {
              this._State = EvFormObjectStates.Null;
            }

            return;
          }
        case FormClassFieldNames.SourceId:
          {
            this._SourceId = Value;
            return;
          }
        case FormClassFieldNames.IsMandatory:
          {
            this._IsMandatory = EvcStatics.getBool ( Value );
            return;
          }
        //
        // Design elements.
        //
        case FormClassFieldNames.Title:
          {
            this._Design.Title = Value;
            return;
          }

        case FormClassFieldNames.Instructions:
          {
            this._Design.Instructions = Value;
            return;
          }

        case FormClassFieldNames.Description:
          {
            this._Description = Value;
            return;
          }

        case FormClassFieldNames.UpdateReason:
          {
            this._UpdateReason = EvStatics.Enumerations.parseEnumValue<UpdateReasonList> ( Value );
            return;
          }
        case FormClassFieldNames.FormAccessRole:
          {
            this._FormAccessRole = EvStatics.Enumerations.parseEnumValue<FormAccessRoles> ( Value );
            return;
          }

        case FormClassFieldNames.TemplateName:
          {
            this._Design.TemplateName = Value;
            return;
          }

        case FormClassFieldNames.Reference:
          {
            this._Design.Reference = Value;
            return;
          }

        case FormClassFieldNames.FormCategory:
          {
            this._Design.FormCategory = Value;
            return;
          }

        case FormClassFieldNames.SubjectScreeningIdInstructions:
          {
            this._Design.SubjectScreeningIdInstructions = Value;
            return;
          }

        case FormClassFieldNames.SubjectSponsorIdInstructions:
          {
            this._Design.SubjectSponsorIdInstructions = Value;
            return;
          }

        case FormClassFieldNames.SubjectRandomisedIdInstructions:
          {
            this._Design.SubjectRandomisedIdInstructions = Value;
            return;
          }

        case FormClassFieldNames.SubjectExternalIdInstructions:
          {
            this._Design.SubjectExternalIdInstructions = Value;
            return;
          }
        case FormClassFieldNames.IsMandatoryActivity:
          {
            this._IsMandatoryActivity = EvcStatics.getBool ( Value ); ;
            return;
          }
        case FormClassFieldNames.SubjectScreeningIdFormat:
          {
            this._Design.SubjectScreeningIdFormat =
              EvcStatics.Enumerations.parseEnumValue<SubjectIdentifierFieldFormats> ( Value );
            return;
          }
        case FormClassFieldNames.SubjectSponsorIdFormat:
          {
            this._Design.SubjectSponsorIdFormat =
              EvcStatics.Enumerations.parseEnumValue<SubjectIdentifierFieldFormats> ( Value );
            return;
          }
        case FormClassFieldNames.SubjectRandomisedIdFormat:
          {
            this._Design.SubjectRandomisedIdFormat =
              EvcStatics.Enumerations.parseEnumValue<SubjectIdentifierFieldFormats> ( Value );
            return;
          }
        case FormClassFieldNames.SubjectExternalIdFormat:
          {
            this._Design.SubjectExternalIdFormat =
              EvcStatics.Enumerations.parseEnumValue<SubjectIdentifierFieldFormats> ( Value );
            return;
          }
        case FormClassFieldNames.TypeId:
          {
            this._Design.TypeId = EvcStatics.Enumerations.parseEnumValue<EvFormRecordTypes> ( Value );
            return;
          }
        case FormClassFieldNames.HasCsScript:
          {
            this._Design.hasCsScript = EvcStatics.getBool ( Value );
            return;
          }

        case FormClassFieldNames.OnEdit_HideFieldAnnotation:
          {
            this._Design.OnEdit_HideFieldAnnotation = EvcStatics.getBool ( Value );
            return;
          }
        default:

          return;

      }//END Switch

    }//END setValue method
    #endregion

    #region Class static  Methods

    //  ==================================================================================
    /// <summary>
    /// This method returns the lable value for a role enumeration value.
    /// </summary>
    /// <param name="State">FormObjecStates: a state object.</param>
    /// <returns>String: a label value.</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize the label and label key.
    /// 
    /// 2. Get the label string value
    /// 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public static string getLabelValue ( EvFormObjectStates State )
    {
      //
      // Initialize the label and label key
      //
      String stLabel = State.ToString ( ).Replace ( "_", " " );
      String stLabelKey = "EvForm_State_" + State;

      //
      // Get the label string value.
      //
      String stLabel1 = EvLabels.ResourceManager.GetString ( stLabelKey );
      if ( stLabel1 != null )
      {
        stLabel = stLabel1;
      }

      return stLabel;
    }//END getLabelValue

    //  =================================================================================
    /// <summary>
    ///  The class generates a list of Clinical record types.
    /// </summary>
    /// <returns>List of EvOption: a list of record types</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize a return list and an option object.
    /// 
    /// 2. Add items from option object to the return list.
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public static List<EvOption> getRecordTypes ( )
    {
      //
      // Initialize a return list and an option object.
      //
      List<EvOption> list = new List<EvOption> ( );

      list.Add ( new EvOption ( EvFormRecordTypes.Null.ToString ( ), String.Empty ) );

      //
      // Add items from option object to the return list.
      //
      list.Add ( EvStatics.Enumerations.getOption ( EvFormRecordTypes.Trial_Record ) );

      list.Add ( EvStatics.Enumerations.getOption ( EvFormRecordTypes.Updatable_Record ) );

      list.Add ( EvStatics.Enumerations.getOption ( EvFormRecordTypes.Questionnaire ) );

      return list;
    }//END getRecordTypes method

    //  =================================================================================
    /// <summary>
    ///  The class generates a list of Clinical record types.
    /// </summary>
    /// <returns>List of EvOption: a list of record types</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize a return list and an option object.
    /// 
    /// 2. Add items from option object to the return list.
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public static List<EvOption> getRecordQuerySelectionList ( )
    {
      //
      // Initialize a return list and an option object.
      //
      List<EvOption> list = new List<EvOption> ( );
      //
      // Add items from option object to the return list.
      //
      list.Add ( EvStatics.Enumerations.getOption ( EvForm.RecordQuerySelectionCodes.All ) );

      list.Add ( EvStatics.Enumerations.getOption ( EvForm.RecordQuerySelectionCodes.Completed_Records ) );

      list.Add ( EvStatics.Enumerations.getOption ( EvForm.RecordQuerySelectionCodes.Not_Queried ) );

      list.Add ( EvStatics.Enumerations.getOption ( EvForm.RecordQuerySelectionCodes.Open_Queries ) );

      list.Add ( EvStatics.Enumerations.getOption ( EvForm.RecordQuerySelectionCodes.Closed_Queries ) );

      return list;
    }//END getRecordTypes method

    //===================================================================================
    /// <summary>
    ///  This methiod lists the common form types.
    /// </summary>
    /// <param name="Trial">EvProject object</param>
    /// <param name="RecordView">Bool: Record only view</param>
    /// <returns>List of EvOption: a common form type list</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize a return list and an option object.
    /// 
    /// 2. Pull items' value from option object.
    /// 
    /// 3. Add items' value to a return list.
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public static List<EvOption> getCommonFormTypes ( EdApplication Trial, bool RecordView )
    {
      //
      // Initialize a return list and an option object
      //
      List<EvOption> list = new List<EvOption> ( );

      list.Add ( new EvOption ( EvFormRecordTypes.Null.ToString ( ), String.Empty ) );

 
      return list;

    }//END getCommonFormTypes method.

    //  =================================================================================
    /// <summary>
    ///  This methiod lists the common form types.
    /// </summary>
    /// <returns>List of EvOption: a common form type list</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize a return list and an option object.
    /// 
    /// 2. Pull items' value from option object.
    /// 
    /// 3. Add items' value to a return list.
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public static List<EvOption> getFormTypes ( bool GlobalForms )
    {
      //
      // Initialize a return list and an option object
      //
      List<EvOption> list = new List<EvOption> ( );

      list.Add ( new EvOption ( EvFormRecordTypes.Null.ToString ( ), String.Empty ) );

      //
      // Pull items' value from optoin object and add to the return list.
      //
      list.Add ( EvStatics.Enumerations.getOption ( EvFormRecordTypes.Trial_Record ) );

      list.Add ( EvStatics.Enumerations.getOption ( EvFormRecordTypes.Questionnaire ) );

      if ( GlobalForms == true )
      {
        list.Add ( EvStatics.Enumerations.getOption ( EvFormRecordTypes.Patient_Record ) );
      }

      list.Add ( EvStatics.Enumerations.getOption ( EvFormRecordTypes.Updatable_Record ) );

      return list;
    }//END getCommonFormTypes method

    //  =================================================================================
    /// <summary>
    ///  This method generates the list of Clinical Record states types.
    /// </summary>
    /// <returns>List of EvOption: a form state list</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize a return list and an option object.
    /// 
    /// 2. Pull items' value from option object.
    /// 
    /// 3. Add items' value to a return list.
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public static List<EvOption> getFormStates ( )
    {
      //
      // Initialize a return list and an option object.
      //
      List<EvOption> list = new List<EvOption> ( );

      list.Add ( new EvOption ( String.Empty, String.Empty ) );

      //
      // Pull items' value from option object and add them to a return list.
      //
      list.Add ( EvStatics.Enumerations.getOption ( EvFormObjectStates.Form_Draft ) );

      list.Add ( EvStatics.Enumerations.getOption ( EvFormObjectStates.Form_Reviewed ) );

      list.Add ( EvStatics.Enumerations.getOption ( EvFormObjectStates.Form_Issued ) );

      list.Add ( EvStatics.Enumerations.getOption ( EvFormObjectStates.Withdrawn ) );

      return list;
    }//END getFormStates method.

    //  =================================================================================
    /// <summary>
    ///  This class generates the list of Clinical Record states types.
    /// </summary>
    /// <returns>List of EvOption: a subject record state list</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize a return list and an option object.
    /// 
    /// 2. Pull items' value from option object.
    /// 
    /// 3. Add items' value to a return list.
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public static List<EvOption> getSubjectRecordStates ( )
    {
      //
      // Initialize a return list and an option object.
      //
      List<EvOption> list = new List<EvOption> ( );

      list.Add ( new EvOption ( String.Empty, String.Empty ) );

      //
      // Retrieve an option object and append the items' value to the return list.
      //
      list.Add ( EvStatics.Enumerations.getOption ( EvFormObjectStates.Draft_Record ) );

      list.Add ( EvStatics.Enumerations.getOption ( EvFormObjectStates.Submitted_Record ) );

      list.Add ( EvStatics.Enumerations.getOption ( EvFormObjectStates.Queried_Record ) );

      list.Add ( EvStatics.Enumerations.getOption ( EvFormObjectStates.Source_Data_Verified ) );

      list.Add ( EvStatics.Enumerations.getOption ( EvFormObjectStates.Locked_Record ) );

      list.Add ( EvStatics.Enumerations.getOption ( EvFormObjectStates.Withdrawn ) );

      return list;
    }//END getSubjectRecordStates method.

    //  =================================================================================
    /// <summary>
    ///  This class generates the list of Clinical Record states types.
    /// </summary>
    /// <returns>List of EvOption: a list  of options</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize a return list and an option object.
    /// 
    /// 2. Pull items' value from option object.
    /// 
    /// 3. Add items' value to a return list.
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public static List<EvOption> getRecordStates ( bool excludeCopies )
    {
      //
      // Initialize a return list and an option object.
      //
      List<EvOption> list = new List<EvOption> ( );

      EvOption Option = new EvOption (
        String.Empty,
        EvLabels.All_Active_Project_Records_Selection_Option );
      list.Add ( Option );

      //
      // Pull items' value from an option object and add them to the return list.
      //
      Option = new EvOption (
        EvFormObjectStates.Draft_Record.ToString ( ),
        EvLabels.Record_State_Draft_Record );
      list.Add ( Option );

      Option = new EvOption (
        EvFormObjectStates.Submitted_Record.ToString ( ),
        EvLabels.Record_State_Submitted_Record );
      list.Add ( Option );

      Option = new EvOption (
        EvFormObjectStates.Queried_Record.ToString ( ),
        EvLabels.Record_State_Queried_Record );
      list.Add ( Option );

      Option = new EvOption (
        EvFormObjectStates.Source_Data_Verified.ToString ( ),
        EvLabels.Record_State_Source_Data_Verified );
      list.Add ( Option );

      Option = new EvOption (
        EvFormObjectStates.Locked_Record.ToString ( ),
        EvLabels.Record_State_Locked_Record );
      list.Add ( Option );

      if ( excludeCopies == false )
      {
        Option = new EvOption (
          EvFormObjectStates.Queried_Record_Copy.ToString ( ),
          EvLabels.Record_State_Queried_Record_Copy );
        list.Add ( Option );

        Option = new EvOption (
          EvFormObjectStates.Withdrawn.ToString ( ),
          EvLabels.Record_State_Withdrawn );
        list.Add ( Option );
      }

      return list;
    }//END getRecordStates method.

    //  =================================================================================
    /// <summary>
    ///  The list of Clinical Record states types.
    /// 
    /// </summary>
    /// <returns></returns>
    //  ---------------------------------------------------------------------------------
    public static List<EvOption> getRptRecordStates ( )
    {
      List<EvOption> list = new List<EvOption> ( );

      EvOption Option = new EvOption ( String.Empty, String.Empty );
      list.Add ( Option );
      //
      // Pull items' value from an option object and add them to the return list.
      //
      Option = new EvOption ( EvFormObjectStates.Draft_Record.ToString ( ),
        EvcStatics.Enumerations.enumValueToString ( EvFormObjectStates.Draft_Record ) );
      list.Add ( Option );

      Option = new EvOption ( EvFormObjectStates.Submitted_Record.ToString ( ),
        EvcStatics.Enumerations.enumValueToString ( EvFormObjectStates.Submitted_Record ) );
      list.Add ( Option );

      Option = new EvOption ( EvFormObjectStates.Queried_Record.ToString ( ),
        EvcStatics.Enumerations.enumValueToString ( EvFormObjectStates.Queried_Record ) );
      list.Add ( Option );

      Option = new EvOption ( EvFormObjectStates.Source_Data_Verified.ToString ( ),
        EvcStatics.Enumerations.enumValueToString ( EvFormObjectStates.Source_Data_Verified ) );
      list.Add ( Option );

      Option = new EvOption ( EvFormObjectStates.Locked_Record.ToString ( ),
        EvcStatics.Enumerations.enumValueToString ( EvFormObjectStates.Locked_Record ) );
      list.Add ( Option );

      return list;

    }//ENd getRptRecordStates method

    //  =================================================================================
    /// <summary>
    /// This static method returns the form identifier for a common form type.
    /// </summary>
    /// <param name="TypeId">FormRecordTypes: a type identifier</param>
    /// <returns>string: a common form identifier</returns>
    //  ---------------------------------------------------------------------------------
    public static string getCommonFormId ( 
      EvFormRecordTypes TypeId )
    {
      //
      // Return the form identifier for the frorm type.
      //
      switch ( TypeId )
      {
        case EvFormRecordTypes.Adverse_Event_Report:
          {
            return "AE";
          }
        case EvFormRecordTypes.Concomitant_Medication:
          {
            return "CCM";
          }
        case EvFormRecordTypes.Serious_Adverse_Event_Report:
          {
            return "SAE";
          }
        case EvFormRecordTypes.Protocol_Exception:
          {
            return "PE";
          }
        case EvFormRecordTypes.Protocol_Variation:
          {
            return "PV";
          }
        case EvFormRecordTypes.Subject_Record:
          {
            return "TS";
          }
        case EvFormRecordTypes.Periodic_Followup:
          {
            return "PPF";
          }
        case EvFormRecordTypes.Patient_Record:
          {
            return "PR";
          }
        case EvFormRecordTypes.Informed_Consent:
        //case EvFormRecordTypes.Informed_Consent_1:
          {
            return "IC";
          }
        /*
      case EvFormRecordTypes.Informed_Consent_2:
        {
          return "IC2";
          break;
        }
      case EvFormRecordTypes.Informed_Consent_3:
        {
          return "IC3";
        }
      case EvFormRecordTypes.Informed_Consent_4:
        {
          return "IC4";
        }*/
      }
      //
      // Retuirn the form id
      //
      return String.Empty;

    }//END getCommonFormId method.
    #endregion

  }//END class EvForm

}//END namespace Evado.Model.Digital
