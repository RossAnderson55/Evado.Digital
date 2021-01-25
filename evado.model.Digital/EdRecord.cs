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
  public class EdRecord
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
      /// This enumeration defines the record withdrawn save action.
      /// </summary>
      Withdrawn_Record,
    }

    /// <summary>
    /// This enumeration list 
    /// </summary>
    public enum FormAccessRoles
    {
      /// <summary>
      /// This enumeration defines the form role is not set the user does not have access.
      /// </summary>
      Null = 0,

      /// <summary>
      /// This enumeration defines the form role is not set the user does not have access.
      /// </summary>
      None = 0,

      /// <summary>
      /// This enumeration defines the form access roles is a reader.
      /// </summary>
      Record_Reader,

      /// <summary>
      /// This enumeration defines the record author role this person is able to update the form content.
      /// </summary>
      Record_Author,

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
    /// This enumeration list defines the form class field names enumerated identifiers.
    /// </summary>
    public enum RecordFieldNames
    {
      /// <summary>
      /// This enumeration is the null value
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration identifies the trial identifier field.
      /// </summary>
      ApplivcationId,

      /// <summary>
      /// This enumeration identifies the form identifier field.
      /// </summary>
      Layout_Id,

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
      /// This enumeration identifies the data collection event identifier field.
      /// </summary>
      Data_Collect_Event_Id,

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
      /// This enumeration identifies the form reference field.
      /// </summary>
      Reference,

      /// <summary>
      /// This enumeration identifies the form form category field.
      /// </summary>
      FormCategory,

      /// <summary>
      /// This enumeration identifies the form type identifier field.
      /// </summary>
      TypeId,

      /// <summary>
      /// This enumeration identifies the form java validate script field.
      /// </summary>
      JavaScript,

      /// <summary>
      /// This enumeration identifies the form has CS script field.
      /// </summary>
      HasCsScript,

      /// <summary>
      /// This enumeration identifies the form user access role field
      /// </summary>
      RecordAccessRole

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
    public Guid CustomerGuid { get; set; }

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

    private Guid _LayoutGuid = Guid.Empty;
    /// <summary>
    /// This property contains a form global unique identifier of a form. 
    /// </summary>
    public Guid LayoutGuid
    {
      get
      {
        return this._LayoutGuid;
      }
      set
      {
        this._LayoutGuid = value;
      }
    }

    private string _ApplicationId = String.Empty;
    /// <summary>
    /// This property contains a application identifier of a form. 
    /// </summary>
    public string ApplicationId
    {
      get
      {
        return this._ApplicationId;
      }
      set
      {
        this._ApplicationId = value;
      }
    }

    private string _LayoutId = String.Empty;
    /// <summary>
    /// This property contains an identifier of a form. 
    /// </summary>
    public string LayoutId
    {
      get
      {
        return this._LayoutId;
      }
      set
      {
        this._LayoutId = value;
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
      }
    }

    private EdRecordObjectStates _State = EdRecordObjectStates.Null;
    /// <summary>
    /// This property contains a state of a form. 
    /// </summary>
    public EdRecordObjectStates State
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
    }


    private EdRecordDesign _Design = new EdRecordDesign ( );
    /// <summary>
    /// This property contains a design object of a form. 
    /// </summary>
    public EdRecordDesign Design
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

    private List<EdRecordField> _Fields = new List<EdRecordField> ( );
    /// <summary>
    /// This property contains a field list of a form. 
    /// </summary>
    public List<EdRecordField> Fields
    {
      get
      {
        return this._Fields;
      }
      set
      {
        this._Fields = value;
      }
    }

    private List<EdRecordEntity> _Entities = new List<EdRecordEntity> ( );
    /// <summary>
    /// This property contains a list of related entities
    /// </summary>
    public List<EdRecordEntity> Entities
    {
      get
      {
        return this._Entities;
      }
      set
      {
        this._Entities = value;
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



    private List<EdUserSignoff> _Signoffs = new List<EdUserSignoff> ( );

    /// <summary>
    /// This property contains a sign off list of a form content.
    /// </summary>
    public List<EdUserSignoff> Signoffs
    {
      get
      {
        return this._Signoffs;
      }
      set
      {
        this._Signoffs = value;
      }
    }
    //
    // These are record filters, containing a record's field identifiers
    //
    private string [ ] _RecordFilterFieldIds = new String [ 5 ];
    /// <summary>
    /// This property contains the record filter values.
    /// </summary>
    public string [ ] RecordFilterFieldIds
    {
      get { return _RecordFilterFieldIds; }
      set { _RecordFilterFieldIds = value; }
    }

    private string _DataCollectEventId = String.Empty;
    /// <summary>
    /// This property contains data collection event identifier for the record. 
    /// </summary>
    public string DataCollectEventId
    {
      get
      {
        return this._DataCollectEventId;
      }
      set
      {
        this._DataCollectEventId = value;
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

    private FormAccessRoles AccessRole = FormAccessRoles.Null;

    /// <summary>
    /// This property defines the form user access role.
    /// </summary>
    public FormAccessRoles FormAccessRole
    {
      get { return AccessRole; }
      set { AccessRole = value; }
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


    private List<EdFormRecordComment> _CommentList = new List<EdFormRecordComment> ( );
    /// <summary>
    /// This property contains a comment list of a form. 
    /// </summary>
    public List<EdFormRecordComment> CommentList
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

    /// <summary>
    /// This property indicates whether a forms fields are empty. 
    /// </summary>
    public bool isEmpty
    {
      get
      {
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
            EdRecordField field = this._Fields [ Row ];
            foreach ( EdRecordTableRow tableRow in field.Table.Rows )
            {
              for ( int col = 0; col < tableRow.Column.Length; col++ )
              {
                if ( field.Table.Header [ col ].TypeId != EvDataTypes.Special_Matrix
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
              EdRecordField field = this._Fields [ Row ];
              bool bValue = false;
              foreach ( EdRecordTableRow tableRow in field.Table.Rows )
              {
                for ( int col = 0; col < tableRow.Column.Length; col++ )
                {
                  if ( field.Table.Header [ col ].TypeId != EvDataTypes.Special_Matrix
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
    public EdRecordTypes TypeId
    {
      get
      {
        return this.Design.TypeId;
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
        foreach ( EdRecordField field in this._Fields )
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
        foreach ( EdRecordField field in this._Fields )
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


    //  =================================================================================
    /// <summary>
    ///  This method updates the form record state based on current data content.
    /// </summary>
    //  ---------------------------------------------------------------------------------
    public void SetUserAccess ( String UserRoleIds )
    {
      //
      // Initialise the methods objects and variables.
      //
      String [ ] arUserRoleIds = UserRoleIds.Split ( ';' );
      this.AccessRole = FormAccessRoles.None;

      //
      // User the state switch to determine the user's access to the record.@
      //
      switch ( this.State )
      {
        case EdRecordObjectStates.Form_Draft:
        case EdRecordObjectStates.Form_Reviewed:
        case EdRecordObjectStates.Form_Issued:
          {
            this.AccessRole = FormAccessRoles.Record_Reader;

            if ( UserRoleIds.Contains ( EdRole.CONST_DESIGNER ) == true )
            {
              this.AccessRole = FormAccessRoles.Form_Designer;
            }

            return;
          }
        case EdRecordObjectStates.Empty_Record:
        case EdRecordObjectStates.Draft_Record:
        case EdRecordObjectStates.Completed_Record:
        case EdRecordObjectStates.Submitted_Record:
          {
            bool bReadRole = EvStatics.CompareDelimtedStrings (
              this.Design.ReadAccessRoles,
              UserRoleIds );

            if( bReadRole == true )
            {
              this.AccessRole = FormAccessRoles.Record_Reader;
            }

            bool bEditRole = EvStatics.CompareDelimtedStrings (
              this.Design.EditAccessRoles,
              UserRoleIds );
            
            if( bReadRole == true )
            {
              this.AccessRole = FormAccessRoles.Record_Author;
            }
            return;
          }

        default:
          {
            bool bReadRole = EvStatics.CompareDelimtedStrings (
              this.Design.ReadAccessRoles,
              UserRoleIds );

            if ( bReadRole == true )
            {
              this.AccessRole = FormAccessRoles.Record_Reader;
            }
            break;
          }
      }

    }


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
      if ( this._State == EdRecordObjectStates.Null )
      {
        return;
      }

      if ( this._Fields.Count == 0 )
      {
        return;
      }

      if ( this.isEmpty == true )
      {
        this._State = EdRecordObjectStates.Empty_Record;
      }
      else
      {
        if ( this._State == EdRecordObjectStates.Empty_Record )
        {
          this._State = EdRecordObjectStates.Draft_Record;
        }
      }

      if ( this.State == EdRecordObjectStates.Submitted_Record
        && this.hasMandatoryFieldValues == false )
      {
        this.State = EdRecordObjectStates.Draft_Record;
      }

      if ( ( this.State == EdRecordObjectStates.Draft_Record
          || this._State == EdRecordObjectStates.Empty_Record )
        && ( this.hasMandatoryFieldValues == true ) )
      {
        this.State = EdRecordObjectStates.Completed_Record;
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
      foreach ( EdRecordField field in this._Fields )
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
      this.FormAccessRole = EdRecord.FormAccessRoles.Null;

      if ( UserProfile.hasEndUserRole ( this.Design.ReadAccessRoles ) == true )
      {
        this.FormAccessRole = EdRecord.FormAccessRoles.Record_Reader;
      }

      if ( UserProfile.hasDesignAccess == true )
      {
        this.FormAccessRole = EdRecord.FormAccessRoles.Form_Designer;
      }

      if ( UserProfile.hasEndUserRole ( this.Design.EditAccessRoles ) == true )
      {
        this.FormAccessRole = EdRecord.FormAccessRoles.Record_Author;
      }

    }//END setFormRole method

    //  =================================================================================
    /// <summary>
    /// This method sort the field into section field number order.
    /// </summary>
    //  ---------------------------------------------------------------------------------
    public void sortFields ( )
    {
      List<EdRecordField> fieldList = new List<EdRecordField> ( );
      int order = 1;
      string stSectionindex = String.Empty;
      const string CONST_DELIMITER = "~^";

      //
      // Iterate through the section adding the fields in section in order of occurance.
      //
      foreach ( EdRecordSection section in this._Design.FormSections )
      {
        //
        // Build a list of section indexes
        //
        stSectionindex += section.No + CONST_DELIMITER
          + section.Title.Trim ( ) + CONST_DELIMITER;

        foreach ( EdRecordField field in this._Fields )
        {
          if ( field.Design.SectionNo == section.No )
          {
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
      foreach ( EdRecordField field in this._Fields )
      {
        int section = field.Design.SectionNo;

        if ( stSectionindex.Contains ( section + CONST_DELIMITER ) == false )
        {
          field.Design.SectionNo = 0;
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
      RecordFieldNames fieldname = RecordFieldNames.Null;

      //
      // Try convert the FieldName into fieldname 
      //
      try
      {
        fieldname = (RecordFieldNames) Enum.Parse ( typeof ( RecordFieldNames ), FieldName );
      }
      catch
      {
        fieldname = RecordFieldNames.Null;
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
    public string getValue ( RecordFieldNames FieldName )
    {
      // 
      // Switch to the fieldname and return the related value.
      // 
      switch ( FieldName )
      {
        case RecordFieldNames.ApplivcationId:
          return this._ApplicationId;

        case RecordFieldNames.Layout_Id:
          return this.LayoutId;

        case RecordFieldNames.SourceId:
          return this._SourceId;

        case RecordFieldNames.Status:
          return this.StateDesc;

        case RecordFieldNames.IsMandatory:
          return this._IsMandatory.ToString ( );

        case RecordFieldNames.Title:
          return this._Design.Title;

        case RecordFieldNames.Instructions:
          return this._Design.Instructions;

        case RecordFieldNames.Description:
          {
            return this._Design.Description;
          }

        case RecordFieldNames.UpdateReason:
          {
            return this._Design.UpdateReason.ToString ( );
          }
        case RecordFieldNames.RecordAccessRole:
          {
            return this.AccessRole.ToString ( );
          }


        case RecordFieldNames.Reference:
          return this._Design.HttpReference;

        case RecordFieldNames.FormCategory:
          return this._Design.RecordCategory;

        case RecordFieldNames.TypeId:
          return this._Design.TypeId.ToString ( );

        case RecordFieldNames.JavaScript:
          return this._Design.JavaScript;

        case RecordFieldNames.HasCsScript:
          return this._Design.hasCsScript.ToString ( );

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
      RecordFieldNames fieldname = RecordFieldNames.Null;

      //
      // Try convert the FieldName into a fieldname enumerated object
      //
      try
      {
        fieldname = (RecordFieldNames) Enum.Parse ( typeof ( RecordFieldNames ), FieldName );
      }
      catch
      {
        fieldname = RecordFieldNames.Null;
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
    public void setValue ( RecordFieldNames FieldName, string Value )
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
        case RecordFieldNames.ApplivcationId:
          {
            this._ApplicationId = Value;
            return;
          }
        case RecordFieldNames.Layout_Id:
          {
            this._LayoutId = Value;
            return;
          }

        case RecordFieldNames.Status:
          {
            try
            {
              this._State = (EdRecordObjectStates) Enum.Parse ( typeof ( EdRecordObjectStates ), Value );
            }
            catch
            {
              this._State = EdRecordObjectStates.Null;
            }

            return;
          }
        case RecordFieldNames.SourceId:
          {
            this._SourceId = Value;
            return;
          }
        case RecordFieldNames.Data_Collect_Event_Id:
          {
            this.DataCollectEventId = Value;
            return;
          }
        case RecordFieldNames.IsMandatory:
          {
            this._IsMandatory = EvcStatics.getBool ( Value );
            return;
          }
        //
        // Design elements.
        //
        case RecordFieldNames.Title:
          {
            this._Design.Title = Value;
            return;
          }

        case RecordFieldNames.Instructions:
          {
            this._Design.Instructions = Value;
            return;
          }

        case RecordFieldNames.Description:
          {
            this._Design.Description = Value;
            return;
          }

        case RecordFieldNames.UpdateReason:
          {
            this._Design.UpdateReason = EvStatics.Enumerations.parseEnumValue<UpdateReasonList> ( Value );
            return;
          }
        case RecordFieldNames.RecordAccessRole:
          {
            this.AccessRole = EvStatics.Enumerations.parseEnumValue<FormAccessRoles> ( Value );
            return;
          }


        case RecordFieldNames.Reference:
          {
            this._Design.HttpReference = Value;
            return;
          }

        case RecordFieldNames.FormCategory:
          {
            this._Design.RecordCategory = Value;
            return;
          }
        case RecordFieldNames.TypeId:
          {
            this._Design.TypeId = EvcStatics.Enumerations.parseEnumValue<EdRecordTypes> ( Value );
            return;
          }
        case RecordFieldNames.HasCsScript:
          {
            this._Design.hasCsScript = EvcStatics.getBool ( Value );
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
    public static string getLabelValue ( EdRecordObjectStates State )
    {
      //
      // Initialize the label and label key
      //
      String stLabel = State.ToString ( ).Replace ( "_", " " );
      String stLabelKey = "EvForm_State_" + State;

      //
      // Get the label string value.
      //
      String stLabel1 = Evado.Model.Digital.EdLabels.ResourceManager.GetString ( stLabelKey );
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

      list.Add ( new EvOption ( EdRecordTypes.Null.ToString ( ), String.Empty ) );

      //
      // Add items from option object to the return list.
      //
      list.Add ( EvStatics.Enumerations.getOption ( EdRecordTypes.Normal_Record ) );

      list.Add ( EvStatics.Enumerations.getOption ( EdRecordTypes.Updatable_Record ) );

      list.Add ( EvStatics.Enumerations.getOption ( EdRecordTypes.Questionnaire ) );

      return list;
    }//END getRecordTypes method


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
    public static List<EvOption> getFormTypes ( )
    {
      //
      // Initialize a return list and an option object
      //
      List<EvOption> list = new List<EvOption> ( );

      list.Add ( new EvOption ( EdRecordTypes.Null.ToString ( ), String.Empty ) );

      //
      // Pull items' value from optoin object and add to the return list.
      //
      list.Add ( EvStatics.Enumerations.getOption ( EdRecordTypes.Normal_Record ) );

      list.Add ( EvStatics.Enumerations.getOption ( EdRecordTypes.Questionnaire ) );

      list.Add ( EvStatics.Enumerations.getOption ( EdRecordTypes.Updatable_Record ) );

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
      list.Add ( EvStatics.Enumerations.getOption ( EdRecordObjectStates.Form_Draft ) );

      list.Add ( EvStatics.Enumerations.getOption ( EdRecordObjectStates.Form_Reviewed ) );

      list.Add ( EvStatics.Enumerations.getOption ( EdRecordObjectStates.Form_Issued ) );

      list.Add ( EvStatics.Enumerations.getOption ( EdRecordObjectStates.Withdrawn ) );

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
      list.Add ( EvStatics.Enumerations.getOption ( EdRecordObjectStates.Draft_Record ) );

      list.Add ( EvStatics.Enumerations.getOption ( EdRecordObjectStates.Submitted_Record ) );


      list.Add ( EvStatics.Enumerations.getOption ( EdRecordObjectStates.Withdrawn ) );

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
        Evado.Model.Digital.EdLabels.All_Active_Project_Records_Selection_Option );
      list.Add ( Option );

      //
      // Pull items' value from an option object and add them to the return list.
      //
      Option = new EvOption (
        EdRecordObjectStates.Draft_Record.ToString ( ),
        Evado.Model.Digital.EdLabels.Record_State_Draft_Record );
      list.Add ( Option );

      Option = new EvOption (
        EdRecordObjectStates.Submitted_Record.ToString ( ),
        Evado.Model.Digital.EdLabels.Record_State_Submitted_Record );
      list.Add ( Option );

      Option = new EvOption (
        EdRecordObjectStates.Withdrawn.ToString ( ),
        Evado.Model.Digital.EdLabels.Record_State_Withdrawn );
      list.Add ( Option );

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
      Option = new EvOption ( EdRecordObjectStates.Draft_Record.ToString ( ),
        EvcStatics.Enumerations.enumValueToString ( EdRecordObjectStates.Draft_Record ) );
      list.Add ( Option );

      Option = new EvOption ( EdRecordObjectStates.Submitted_Record.ToString ( ),
        EvcStatics.Enumerations.enumValueToString ( EdRecordObjectStates.Submitted_Record ) );
      list.Add ( Option );

      return list;

    }//ENd getRptRecordStates method
    #endregion

  }//END class EvForm

}//END namespace Evado.Model.Digital
