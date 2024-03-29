/***************************************************************************************
 * <copyright file="model\EdRecord.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2021 EVADO HOLDING PTY. LTD..  All rights reserved.
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
 *  This class contains the EdRecord data object.
 *
 ****************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

namespace Evado.Digital.Model
{
  /// <summary>
  /// This class defines the evado form object.
  /// </summary>
  [Serializable]
  public class EdRecord
  {
    #region class initialisation

    public EdRecord ( )
    {
      this.Design.AuthorAccess = AuthorAccessList.Only_Author;
      this.Design.ParentType = ParentTypeList.User;
      this.Design.FieldReadonlyDisplayFormat = FieldReadonlyDisplayFormats.Default;
      this.Visabilty = VisabilityList.Public;
    }

    #endregion

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
      Layout_Saved,

      /// <summary>
      /// This enumeration defines the form reciew save action
      /// </summary>
      Layout_Reviewed,

      /// <summary>
      /// This enumeration defines the form approved save action.
      /// </summary>
      Layout_Approved,

      /// <summary>
      /// This enumeration defines the save form save action
      /// </summary>
      Layout_Update,

      /// <summary>
      /// This enumeratin defines the form withdrawn save action.
      /// </summary>
      Layout_Withdrawn,

      /// <summary>
      /// This enumeration defines the form deleted save action.
      /// </summary>
      Layout_Deleted,

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
    public enum ParentTypeList
    {
      /// <summary>
      /// This enumeration defines the form role is not set the user does not have access.
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines the object parent type to be a user.
      /// </summary>
      User,

      /// <summary>
      /// This enumeration defines the object parent type to be an organsiation.
      /// This enables all user within that organisation to access and edit the object.
      /// </summary>
      Organisation,

      /// <summary>
      /// This enumeration defines the object parent type to be a user default.
      /// </summary>
      User_Default,

      /// <summary>
      /// This enumeration defines the object parent type to be an organsiation default.
      /// This enables all user within that organisation to access and edit the object.
      /// </summary>
      Organisation_Default,

      /// <summary>
      /// This enumeration defines the object parent is another entity.
      /// this selection enables the 
      /// </summary>
      Entity,
    }

    /// <summary>
    /// This enumeration list defines the record author access
    /// </summary>
    public enum AuthorAccessList
    {
      /// <summary>
      /// This enumeration defines the form role is not set the user does not have access.
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines the only the author can edit the object content.
      /// </summary>
      Only_Author,

      /// <summary>
      /// This enumeration defines the only the author can edit the object content.
      /// The default display is in display only layout
      /// </summary>
      Only_Author_Selectable,

      /// <summary>
      /// This enumeration defines the all organisatino users associated with an object can edit the object content..
      /// </summary>
      Only_Organisation,

      /// <summary>
      /// This enumeration defines all edit accss roles have edit access to the object.
      /// </summary>
      Edit_Access_Roles,
    }

    /// <summary>
    /// This enumeration list defines the layout header format
    /// </summary>
    public enum HeaderFormat
    {
      /// <summary>
      /// This enumeration defines the form role is not set the user does not have access.
      /// </summary>
      Null = 0,

      /// <summary>
      /// This enumeration defines the only the author can edit the object content.
      /// </summary>
      Default = 0,

      /// <summary>
      /// This enumeration defines the not to display a layout header.
      /// </summary>
      No_Header,

      /// <summary>
      /// This enumeration defines author record/entity layout header setting
      /// </summary>
      Author_Header,

      /// <summary>
      /// This enumeration defines  author record/entity and save date layout header setting.
      /// </summary>
      Author_Date_Header,
    }
    /// <summary>
    /// This enumeration list defines the layout header format
    /// </summary>
    public enum FooterFormat
    {
      /// <summary>
      /// This enumeration defines the form role is not set the user does not have access.
      /// </summary>
      Null = 0,

      /// <summary>
      /// This enumeration defines the only the author can edit the object content.
      /// </summary>
      Default = 0,

      /// <summary>
      /// This enumeration defines footer to not be displayed.
      /// </summary>
      No_Footer,

      /// <summary>
      /// This enumeration defines footer to not display comments but display signatures.
      /// </summary>
      No_Comments,

      /// <summary>
      /// This enumeration defines footer to not display signatures but display comments.
      /// </summary>
      No_Signatures,

      /// <summary>
      /// This enumeration defines footer to display author information.
      /// </summary>
      Author_Footer,
    }

    /// <summary>
    /// This enumeration list defines the record author access
    /// </summary>
    public enum VisabilityList
    {
      /// <summary>
      /// This enumeration defines the form role is not set the user does not have access.
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines the only the author can edit the object content.
      /// </summary>
      Public,

      /// <summary>
      /// This enumeration defines the all organisatino users associated with an object can edit the object content..
      /// </summary>
      Private,

      /// <summary>
      /// This enumeration defines the all organisatino users associated with an object can edit the object content..
      /// </summary>
      Visible_Private,
    }

    /// <summary>
    /// This enumeration list 
    /// </summary>
    public enum FieldReadonlyDisplayFormats
    {
      /// <summary>
      /// The value is not set.
      /// </summary>
      Null = 0,
      /// <summary>
      /// This enumeration set the command link to display the default content.
      /// </summary>
      Default = 0,
      /// <summary>
      /// This enumeration sets the command link to display the record summary.
      /// </summary>
      Hide_Field_Titles,

      /// <summary>
      /// This enumeration set the command link to display the first record field content.
      /// </summary>
      Document_Layout,

      /// <summary>
      /// This enumeration set the command link to display the summary field content.
      /// </summary>
      Document_Layout_No_Titles,
    }

    /// <summary>
    /// This enumeration list 
    /// </summary>
    public enum LinkContentSetting
    {
      /// <summary>
      /// The value is not set.
      /// </summary>
      Null = 0,
      /// <summary>
      /// This enumeration set the command link to display the default content.
      /// </summary>
      Default = 0,

      /// <summary>
      /// This enumeration sets the command link to display the record summary.
      /// </summary>
      Display_Summary,

      /// <summary>
      /// This enumeration set the command link to display the first record field content.
      /// </summary>
      First_Text_Field,

      /// <summary>
      /// This enumeration set the command link to display the Abstract field content.
      /// </summary>
      Abstract,

      /// <summary>
      /// This enumeration set the command link to display the summary field content.
      /// </summary>
      Summary_Fields,
    }


    /// <summary>
    /// This enumeration list defines the form class field names enumerated identifiers.
    /// </summary>
    public enum FieldNames
    {
      /// <summary>
      /// This enumeration is the null value
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration identifies the form identifier field.
      /// </summary>
      Layout_Id,

      /// <summary>
      /// This enumeration identified the organisation identifier field.
      /// </summary>
      OrgId,

      /// <summary>
      /// This enumeration identifies the record identifier field.
      /// </summary>
      RecordId,

      /// <summary>
      /// This enumeration identifies the record date field.
      /// </summary>
      RecordDate,

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
      /// This enumeration identifies the form record is mandatory field.
      /// </summary>
      IsMandatory,

      /// <summary>
      /// This enumeration identifies the form title field.
      /// </summary>
      Object_Title,

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
      /// This enumeration identifies field titles are to be hiden in display model field.
      /// </summary>
      FieldReadonlyDisplayFormat,

      /// <summary>
      /// This enumeration identifies the record subject field.
      /// </summary>
      Author,

      /// <summary>
      /// This enumeration identifies the record subject field.
      /// </summary>
      AuthorUserId,

      /// <summary>
      /// This enumeration parent layout identifier  field.
      /// </summary>
      ParentLayoutId,

      /// <summary>
      /// This enumeration parent organisation identifier  field.
      /// </summary>
      ParentOrgId,

      /// <summary>
      /// This enumeration parent user identifier  field.
      /// </summary>
      ParentUserId,

      /// <summary>
      /// This enumeration parent layout guid field.
      /// </summary>
      ParentGuid,

      /// <summary>
      /// This enumeration identifies the form user access role field
      /// </summary>
      ReadAccessRoles,

      /// <summary>
      /// This enumeration identifies the form user edit role field
      /// </summary>
      EditAccessRoles,

      /// <summary>
      /// This enumeration identifies the record subject field.
      /// </summary>
      Viaibility,

      /// <summary>
      /// This enumeration identifies the default page layout field
      /// </summary>
      DefaultPageLayout,

      /// <summary>
      /// This enumeration identifies the record prefix field
      /// </summary>
      RecordPrefix,

      /// <summary>
      /// This enumeration identifies the link content setting  field
      /// </summary>
      LinkContentSetting,

      /// <summary>
      /// This enumeration identifies the header format  field
      /// </summary>
      HeaderFormat,

      /// <summary>
      /// This enumeration identifies the footer format  field
      /// </summary>
      FooterFormat,

      /// <summary>
      /// This enumeration identifies the display related entities field
      /// </summary>
      DisplayRelatedEntities,

      /// <summary>
      /// This enumeration indicates the author field 
      /// </summary>
      DisplayAuthorDetails,

      /// <summary>
      /// This enumeration identifies the form Parent type field.
      /// </summary>
      ParentType,

      /// <summary>
      /// This enumeration identifies the form Author Access  field.
      /// </summary>
      AuthorAccess,

      /// <summary>
      /// This enumeration identifies the form delimited ';' list of possible parent entities.  field.
      /// </summary>
      ParentEntities,

      /// <summary>
      /// This enumeration defines the text field width identifier.
      /// </summary>,
      FieldWidth,

      /// <summary>
      /// This enumeration defines the text field height identifier.
      /// </summary>
      FieldHeight,

      /// <summary>
      /// This enumeration defines the entity access to t.
      /// </summary>
      EntityAccess,

    }

    #endregion

    #region Internal member parameters
    /// <summary>
    /// This constant defines the field type identifier for form fields.
    /// </summary>
    public const string CONST_RECORD_TYPE = "RECTYP";

    /// <summary>
    /// This constant defines the number of fields in the filtered field query 
    /// </summary>
    public const int FILTER_FIELD_COUNT = 5;

    #endregion

    #region Class Properties

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

        this._LayoutId = this._LayoutId.Replace ( "'", String.Empty );
        this._LayoutId = this._LayoutId.Replace ( "/", String.Empty );
        this._LayoutId = this._LayoutId.Replace ( @"\", String.Empty );
        this._LayoutId = this._LayoutId.Replace ( @",", String.Empty );
        this._LayoutId = this._LayoutId.Replace ( @";", String.Empty );
        this._LayoutId = this._LayoutId.Replace ( @":", String.Empty );
        this._LayoutId = this._LayoutId.Replace ( "\"", String.Empty );
        this._LayoutId = this._LayoutId.Replace ( "=", String.Empty );
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

    /// <summary>
    /// This property contains a record identifier of a form. 
    /// </summary>
    public string EntityId
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
    private string _ParentOrgId = String.Empty;
    /// <summary>
    /// This property contains the organisation ID of the parent organisation associated with this object.
    /// </summary>
    public string ParentOrgId
    {
      get
      {
        return this._ParentOrgId;
      }
      set
      {
        this._ParentOrgId = value;
      }
    }
    private string _ParentUserId = String.Empty;
    /// <summary>
    /// This property contains the user id of the parent organisation associated with this object.
    /// </summary>
    public string ParentUserId
    {
      get
      {
        return this._ParentUserId;
      }
      set
      {
        this._ParentUserId = value;
      }
    }

    private string _AuthorUserId = String.Empty;
    /// <summary>
    /// This property contains a user id of the user associated with this record. 
    /// </summary>
    public string AuthorUserId
    {
      get
      {
        return this._AuthorUserId;
      }
      set
      {
        this._AuthorUserId = value;
      }
    }

    private string _Author = String.Empty;
    /// <summary>
    /// This property contains a user id of the author associated with this record. 
    /// </summary>
    public string Author
    {
      get
      {
        return this._Author;
      }
      set
      {
        this._Author = value;
      }
    }

    private string _ParentLayoutId = String.Empty;
    /// <summary>
    /// This property contains the parent object layout id with this object.
    /// </summary>
    public string ParentLayoutId
    {
      get
      {
        return this._ParentLayoutId;
      }
      set
      {
        this._ParentLayoutId = value;
      }
    }

    private Guid _ParentGuid = Guid.Empty;
    /// <summary>
    /// This property contains the parent object Guid associated with this object.
    /// </summary>
    public Guid ParentGuid
    {
      get
      {
        return this._ParentGuid;
      }
      set
      {
        this._ParentGuid = value;
      }
    }

    private string _EntityAccess = String.Empty;
    /// <summary>
    /// This property contains a delimited list ';' of entities that have access to this object. 
    /// And is enabled if visibility is set to 
    /// </summary>
    public string EntityAccess
    {
      get
      {
        if ( this.Visabilty == VisabilityList.Public )
        {
          this._EntityAccess = String.Empty;
        }

        return this._EntityAccess;
      }
      set
      {
        this._EntityAccess = value;
      }
    }
    bool _ButtonEditModeEnabled = false;
    /// <summary>
    /// This property enables the entity edit button
    /// </summary>
    public bool ButtonEditModeEnabled
    {
      get
      {

        return this._ButtonEditModeEnabled;
      }
      set
      {
        this._ButtonEditModeEnabled = value;
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
        return EvcStatics.enumValueToString ( this._State );
      }
    }

    /// <summary>
    /// This property defines the visibility of the record objects content to platform users.
    /// </summary>
    public EdRecord.VisabilityList Visabilty { get; set; }

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
    /// <summary>
    /// this property returns the filename for the first image field with a field value.
    /// </summary>
    public String ImageFileName
    {
      get
      {
        //
        // Iterate through the fields to find the first image field
        // with a value and return the filename.
        //
        foreach ( EdRecordField field in this.Fields )
        {
          if ( field.TypeId == Evado.Model.EvDataTypes.Image
            && field.ItemValue != String.Empty )
          {
            return field.ItemValue;
          }
        }

        return String.Empty;
      }
    }

    private List<EdRecord> _ChildEntities = new List<EdRecord> ( );
    /// <summary>
    /// This property contains a list of related entities
    /// </summary>
    public List<EdRecord> ChildEntities
    {
      get
      {
        return this._ChildEntities;
      }
      set
      {
        this._ChildEntities = value;
      }
    }

    private List<EdRecord> _ChildRecords = new List<EdRecord> ( );
    /// <summary>
    /// This property contains a list of related records
    /// </summary>
    public List<EdRecord> ChildRecords
    {
      get
      {
        return this._ChildRecords;
      }
      set
      {
        this._ChildRecords = value;
      }
    }

    /// <summary>
    /// This property contains a link text of a form. 
    /// </summary>
    public string CommandTitle
    {
      get
      {
        //
        // Return the design command title
        //
        if ( this._State == EdRecordObjectStates.Form_Draft
          || this._State == EdRecordObjectStates.Form_Reviewed
          || this._State == EdRecordObjectStates.Form_Issued )
        {
          return String.Format (
                    "{0} - {1}, Type: {2}, Version {3}, State {4}",
                    this.LayoutId,
                    this.Title,
                    this.TypeId,
                    this.Version,
                    this.StateDesc );
        }


        if ( this.RecordId == String.Empty )
        {
          this.RecordId = "RECORD-ID";
        }
        String link = String.Format (
                    EdLabels.Record_Page_Header_Text,
                    this.RecordId,
                    this.LayoutId,
                    this.Title );

        //
        // if a comment text then output an abstract of the linke..
        //
        if ( this.TypeId == EdRecordTypes.Comment_Record )
        {
          string value = this.getFirstFreeTextField ( true );

          if ( value != String.Empty )
          {
            return value;
          }
          return link;
        }

        switch ( this.Design.LinkContentSetting )
        {
          //
          // Abstract link
          //
          case LinkContentSetting.Abstract:
            {
              string value = this.getFirstFreeTextField ( true );

              if ( value != String.Empty )
              {
                return value;
              }
              return link;
            }
          //
          // Display the first field with a text value.
          //
          case LinkContentSetting.First_Text_Field:
            {
              String value = this.getFirstTextField ( );

              if ( value != String.Empty )
              {
                return value;
              }
              return link;
            }
          //
          // Summary display
          //
          case LinkContentSetting.Display_Summary:
            {
              if ( this.RecordSummary != String.Empty )
              {
                return this.RecordSummary;
              }
              return link;
            }

          default:
          case LinkContentSetting.Default:
            {
              if ( this.Design.DisplayAuthorDetails == true )
              {
                link += EdLabels.Label_by + this.Updated;
              }
              return link;
            }
        }//END stitch statement
      }//End get 
    }//END property


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
    /************************************************************************************
     * 
     * The multi-field selection function, uses summary fields that are single or 
     * multi-selection fields (selection list, radio-buttons, checkbox-list), to create 
     * Entity selections based on these field values.  The property below stored the 
     * fieldIds for 5 summary selection fields from the entity.  
     * 
     * The array is automatically generated, by iterating through the summary fields,
     * adding the selection field types to the list.
     * 
     * This list is then used to identify the selection fields to be created to select 
     * the Entities based on their these field's values.
     * 
     ************************************************************************************/
    private string [ ] _FilterFieldIds = new String [ EdRecord.FILTER_FIELD_COUNT ];
    /// <summary>
    /// This property contains the record filter values.
    /// </summary>
    public string [ ] FilterFieldIds
    {
      get { return _FilterFieldIds; }
      set { _FilterFieldIds = value; }
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

    private DateTime _RecordDate =  Evado.Model.EvStatics.CONST_DATE_NULL;
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

    String _RecordSummary = String.Empty;
    /// <summary>
    /// This property contains Record Summary data.
    /// </summary>
    public string RecordSummary
    {
      get
      {
        return this._RecordSummary;
      }
      set
      {
        this._RecordSummary = value;
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

    public FormAccessRoles FormAccessRole { set; get; }

    String _AiIndex = String.Empty;
    /// <summary>
    /// This property contains cDash metadata values. 
    /// </summary>
    public string AiIndex
    {
      get
      {
        return this._AiIndex;
      }
      set
      {
        this._AiIndex = value;
      }
    }

    /// <summary>
    /// This property indicates if a retreival is to get summary fields only.
    /// </summary>
    public bool SelectOnlySummaryFields { get; set; }

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
          if ( this._Fields [ Row ].TypeId != Evado.Model.EvDataTypes.Table
            || this._Fields [ Row ].TypeId != Evado.Model.EvDataTypes.Special_Matrix )
          {
            // 
            // if a field is queried return true.
            // 
            if ( ( this._Fields [ Row ].TypeId == Evado.Model.EvDataTypes.Free_Text
                || this._Fields [ Row ].TypeId == Evado.Model.EvDataTypes.Signature )
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
                if ( field.Table.Header [ col ].TypeId != Evado.Model.EvDataTypes.Special_Matrix
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
          if ( this._Fields [ Row ].TypeId == Evado.Model.EvDataTypes.Signature )
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
            if ( this._Fields [ Row ].TypeId == Evado.Model.EvDataTypes.Table
              || this._Fields [ Row ].TypeId == Evado.Model.EvDataTypes.Special_Matrix )
            {
              EdRecordField field = this._Fields [ Row ];
              bool bValue = false;
              foreach ( EdRecordTableRow tableRow in field.Table.Rows )
              {
                for ( int col = 0; col < tableRow.Column.Length; col++ )
                {
                  if ( field.Table.Header [ col ].TypeId != Evado.Model.EvDataTypes.Special_Matrix
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
    /// This property contains a selection list option object. 
    /// </summary>
    public Evado.Model.EvOption SelectionOption
    {
      get
      {
        return new Evado.Model.EvOption ( this.LayoutId, this.LayoutId + " - " + this.Title );
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

    private  Evado.Model.EvEventCodes _EventCode =  Evado.Model.EvEventCodes.Ok;
    /// <summary>
    /// This property contains an event code object of a form. 
    /// </summary>
    public  Evado.Model.EvEventCodes EventCode
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
          if ( field.TypeId == Evado.Model.EvDataTypes.Signature )
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
          if ( field.TypeId == Evado.Model.EvDataTypes.Signature )
          {
            signatureFieldCount++;
          }

          Evado.Model.EvSignatureBlock signatureBlock = Newtonsoft.Json.JsonConvert.DeserializeObject<Evado.Model.EvSignatureBlock> ( field.ItemText );

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

    // =====================================================================================
    /// <summary>
    /// This method returns the first text field.
    /// </summary>
    /// <param name="FirstFreeText">Bool: true = return first free text field.</param>
    /// <returns>string: field value.</returns>
    // -------------------------------------------------------------------------------------
    public string getFirstTextField ( )
    {
      String stValue = EdLabels.Record_Empty_Value + this.Title;
      foreach ( EdRecordField field in this.Fields )
      {
        if ( field.TypeId == Evado.Model.EvDataTypes.Text )
        {
          stValue = field.ItemValue;
          break;
        }
      }

      if ( this.Design.DisplayAuthorDetails == true )
      {
        stValue += EdLabels.Label_by + this.Updated;
      }
      return stValue;
    }

    // =====================================================================================
    /// <summary>
    /// This method returns the first text field.
    /// </summary>
    /// <param name="FirstFreeText">Bool: true = return first free text field.</param>
    /// <returns>string: field value.</returns>
    // -------------------------------------------------------------------------------------
    public EdRecordField getFirstHttpLinkField ( )
    {
      String stValue = String.Empty;
      foreach ( EdRecordField field in this.Fields )
      {
        if ( field.TypeId == Evado.Model.EvDataTypes.Http_Link )
        {
          return field;
        }
      }
      return null;
    }

    // =====================================================================================
    /// <summary>
    /// This method returns the first free text field.
    /// </summary>
    /// <param name="AsAbstract">Bool: true = return only the first paragraph of the field.</param>
    /// <returns>string: field value.</returns>
    // -------------------------------------------------------------------------------------
    public string getFirstFreeTextField ( bool AsAbstract )
    {
      String stValue = EdLabels.Record_Empty_Value + this.Title;

      foreach ( EdRecordField field in this.Fields )
      {
        if ( field.TypeId == Evado.Model.EvDataTypes.Free_Text )
        {
          stValue = field.ItemText;

          if ( AsAbstract == true )
          {
            int paraIndex = stValue.IndexOf ( '\n' );
            if ( paraIndex > 0 )
            {
              stValue = stValue.Substring ( 0, paraIndex );
            }
          }
          break;
        }
      }

      if ( this.Design.DisplayAuthorDetails == true )
      {
        stValue += "\r\n" + EdLabels.Label_by + this.Updated;
      }

      return stValue;
    }

    // =====================================================================================
    /// <summary>
    /// This method test to see if the user has a role contain in the roles delimited list.
    /// </summary>
    /// <param name="Roles">';' delimted string of roles</param>
    /// <returns>True: if the role exists.</returns>
    // -------------------------------------------------------------------------------------
    public bool hasReadAccess ( String Roles )
    {
      if ( Roles == null )
      {
        return false;
      }

      //
      // no defined read access roles indicated all users have access.
      //
      if ( this.Design.ReadAccessRoles == String.Empty )
      {
        return true;
      }

      //
      // an empty role is passed and the entity had read access roles return false.
      //
      if ( Roles == String.Empty
        && this.Design.ReadAccessRoles != String.Empty )
      {
        return false;
      }

      //
      // Iterate through the roles looking for a match.
      //
      foreach ( String role in Roles.Split ( ';' ) )
      {
        foreach ( String role1 in this.Design.ReadAccessRoles.Split ( ';' ) )
        {
          if ( role1.ToLower ( ) == role.ToLower ( ) )
          {
            return true;
          }
        }
      }
      return false;
    }//END method

    // =====================================================================================
    /// <summary>
    /// This method test to see if the user has a role contain in the roles delimited list.
    /// </summary>
    /// <param name="Roles">';' delimted string of roles</param>
    /// <returns>True: if the role exists.</returns>
    // -------------------------------------------------------------------------------------
    public bool hasEditAccess ( String Roles )
    {
      if ( Roles == null )
      {
        return false;
      }

      //
      // no defined read access roles indicated all users have access.
      //
      if ( this.Design.EditAccessRoles == String.Empty )
      {
        return true;
      }

      //
      // if roles are defined and an empty string is passed return false access.
      //
      if ( Roles == String.Empty
        && this.Design.EditAccessRoles != String.Empty )
      {
        return false;
      }

      //
      // iterate through the roles looking for a match.
      //
      foreach ( String role in Roles.Split ( ';' ) )
      {
        foreach ( String role1 in this.Design.EditAccessRoles.Split ( ';' ) )
        {
          if ( role1.ToLower ( ) == role.ToLower ( ) )
          {
            return true;
          }
        }
      }
      return false;
    }//END method

    //=====================================================================================
    /// <summary>
    /// This method selects and returns the selected field and returns null if not found.
    /// </summary>
    /// <param name="FieldId">String: the selected field identifier</param>
    /// <returns>EdRecordField object</returns>
    //-------------------------------------------------------------------------------------
    public EdRecordField GetFieldObject ( String FieldId )
    {
      FieldId = FieldId.Trim ( );
      //
      // iterate through the entity fields to find the selected field.
      //
      foreach ( EdRecordField field in this.Fields )
      {
        if ( field.FieldId.ToLower ( ) == FieldId.ToLower ( ) )
        {
          return field;
        }
      }

      return null;
    }//END GetFieldObject method

    //=====================================================================================
    /// <summary>
    /// This method sets the Form Role property 
    /// </summary>
    /// <param name="UserProfile">EvUserProfile Object</param>
    //-------------------------------------------------------------------------------------
    public bool createUserAccess ( EdUserProfile UserProfile )
    {

      //
      // Set the designer access.
      //
      if ( UserProfile.hasDesignAccess == true
        && ( this.State == EdRecordObjectStates.Form_Draft
          || this.State == EdRecordObjectStates.Form_Reviewed ) )
      {
        return false;
      }

      //
      // Set the default edit access 
      //
      if ( this.hasEditAccess ( UserProfile.Roles ) == true )
      {
        return true;
      }
      return false;

    }//END createUserAccess method

    //=====================================================================================
    /// <summary>
    /// This method sets the Form Role property 
    /// </summary>
    /// <param name="UserProfile">EvUserProfile Object</param>
    //-------------------------------------------------------------------------------------
    public void setUserAccess ( EdUserProfile UserProfile )
    {
      //
      // Swtich roleid to select the form role for the user.
      //
      this.FormAccessRole = EdRecord.FormAccessRoles.Null;

      //
      // Set the designer access.
      //
      if ( UserProfile.hasDesignAccess == true
        && ( this.State == EdRecordObjectStates.Form_Draft
          || this.State == EdRecordObjectStates.Form_Reviewed ) )
      {
        this.FormAccessRole = EdRecord.FormAccessRoles.Record_Author;
        return;
      }

      //
      // Set the object author access
      //
      switch ( this.Design.AuthorAccess )
      {
        case AuthorAccessList.Only_Author_Selectable:
          {
            if ( this.AuthorUserId == UserProfile.UserId )
            {
              this.FormAccessRole = EdRecord.FormAccessRoles.Record_Author;
              return;
            }

            /* 
             * FURTHER CONDITIONS WILL BE ADDED WHEN MULTIPLE USERS HAVE ACCESS TO THE ENTITY.
             */

            this.FormAccessRole = EdRecord.FormAccessRoles.Record_Reader;
            return;

          }
        case AuthorAccessList.Only_Author:
          {
            if ( this.AuthorUserId == UserProfile.UserId )
            {
              this.FormAccessRole = EdRecord.FormAccessRoles.Record_Author;
              return;
            }
            this.FormAccessRole = EdRecord.FormAccessRoles.Record_Reader;
            return;
          }
        case AuthorAccessList.Only_Organisation:
          {
            //
            // Set Record Author access when organisation only access is set and 
            // the user organisation is the same as the parent organsiations.
            //
            if ( this.ParentOrgId == UserProfile.OrgId )
            {
              this.FormAccessRole = EdRecord.FormAccessRoles.Record_Author;
            }
            else
            {
              this.FormAccessRole = EdRecord.FormAccessRoles.Record_Reader;
            }
            return;
          }

        default:
          {
            //
            // Set the reader access 
            //
            if ( this.hasReadAccess ( UserProfile.Roles ) == true )
            {
              this.FormAccessRole = EdRecord.FormAccessRoles.Record_Reader;
            }

            //
            // Set the default edit access 
            //
            if ( this.hasEditAccess ( UserProfile.Roles ) == true )
            {
              this.FormAccessRole = EdRecord.FormAccessRoles.Record_Author;
            }
            return;
          }
      }//END switch statement

    }//END setFormRole method

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
      FieldNames fieldname = FieldNames.Null;

      //
      // Try convert the FieldName into a fieldname enumerated object
      //
      try
      {
        fieldname = (FieldNames) Enum.Parse ( typeof ( FieldNames ), FieldName );
      }
      catch
      {
        fieldname = FieldNames.Null;
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
    public void setValue ( FieldNames FieldName, string Value )
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
        case FieldNames.Layout_Id:
          {
            this._LayoutId = Value;
            return;
          }

        case FieldNames.Status:
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
        case FieldNames.SourceId:
          {
            this._SourceId = Value;
            return;
          }
        case FieldNames.Data_Collect_Event_Id:
          {
            this.DataCollectEventId = Value;
            return;
          }
        case FieldNames.IsMandatory:
          {
            this._IsMandatory = EvcStatics.getBool ( Value );
            return;
          }
        //
        // Design elements.
        //
        case FieldNames.Object_Title:
          {
            this._Design.Title = Value;
            return;
          }

        case FieldNames.Instructions:
          {
            this._Design.Instructions = Value;
            return;
          }

        case FieldNames.Description:
          {
            this._Design.Description = Value;
            return;
          }

        case FieldNames.UpdateReason:
          {
            this._Design.UpdateReason =  Evado.Model.EvStatics.parseEnumValue<UpdateReasonList> ( Value );
            return;
          }
        case FieldNames.EditAccessRoles:
          {
            this.Design.EditAccessRoles = Value;
            return;
          }
        case FieldNames.ReadAccessRoles:
          {
            this.Design.ReadAccessRoles = Value;
            return;
          }
        case FieldNames.EntityAccess:
          {
            this.EntityAccess = Value;
            return;
          }
        case FieldNames.AuthorAccess:
          {
            this.Design.AuthorAccess = Evado.Model.EvStatics.parseEnumValue<EdRecord.AuthorAccessList> ( Value );
            return;
          }
        case FieldNames.ParentType:
          {
            this.Design.ParentType = Evado.Model.EvStatics.parseEnumValue<EdRecord.ParentTypeList> ( Value );
            return;
          }
        case FieldNames.ParentEntities:
          {
            this._Design.ParentEntities = Value;
            return;
          }
        case FieldNames.Viaibility:
          {
            this.Visabilty =  Evado.Model.EvStatics.parseEnumValue<EdRecord.VisabilityList> ( Value );
            return;
          }

        case FieldNames.Reference:
          {
            this._Design.HttpReference = Value;
            return;
          }

        case FieldNames.FormCategory:
          {
            this._Design.RecordCategory = Value;
            return;
          }
        case FieldNames.TypeId:
          {
            this._Design.TypeId = Evado.Model.EvStatics.parseEnumValue<EdRecordTypes> ( Value );
            return;
          }
        case FieldNames.HasCsScript:
          {
            this._Design.hasCsScript = EvcStatics.getBool ( Value );
            return;
          }
        case FieldNames.FieldReadonlyDisplayFormat:
          {
            this._Design.FieldReadonlyDisplayFormat =
               Evado.Model.EvStatics.parseEnumValue<FieldReadonlyDisplayFormats> ( Value );
            return;
          }
        case FieldNames.DefaultPageLayout:
          {
            this._Design.DefaultPageLayout = Value;
            return;
          }
        case FieldNames.RecordPrefix:
          {
            this._Design.RecordPrefix = Value;
            return;
          }
        case FieldNames.LinkContentSetting:
          {
            this.Design.LinkContentSetting =
              Evado.Model.EvStatics.parseEnumValue<EdRecord.LinkContentSetting> ( Value );
            return;
          }
        case FieldNames.HeaderFormat:
          {
            this.Design.HeaderFormat =
              Evado.Model.EvStatics.parseEnumValue<EdRecord.HeaderFormat> ( Value );
            return;
          }
        case FieldNames.FooterFormat:
          {
            this.Design.FooterFormat =
              Evado.Model.EvStatics.parseEnumValue<EdRecord.FooterFormat> ( Value );
            return;
          }

        case FieldNames.DisplayRelatedEntities:
          {
            this.Design.DisplayRelatedEntities = EvcStatics.getBool ( Value );
            return;
          }
        case FieldNames.DisplayAuthorDetails:
          {
            this.Design.DisplayAuthorDetails = EvcStatics.getBool ( Value );
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
      String stLabel1 = Evado.Digital.Model.EdLabels.ResourceManager.GetString ( stLabelKey );
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
    /// <returns>List of Evado.Model.EvOption: a list of record types</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize a return list and an option object.
    /// 
    /// 2. Add items from option object to the return list.
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public static List<Evado.Model.EvOption> getRecordTypes ( )
    {
      //
      // Initialize a return list and an option object.
      //
      List<Evado.Model.EvOption> list = new List<Evado.Model.EvOption> ( );

      list.Add ( new Evado.Model.EvOption ( EdRecordTypes.Null.ToString ( ), String.Empty ) );

      //
      // Add items from option object to the return list.
      //
      list.Add (  Evado.Model.EvStatics.getOption ( EdRecordTypes.Normal_Record ) );

      list.Add (  Evado.Model.EvStatics.getOption ( EdRecordTypes.Updatable_Record ) );

      list.Add (  Evado.Model.EvStatics.getOption ( EdRecordTypes.Questionnaire ) );

      return list;
    }//END getRecordTypes method


    //  =================================================================================
    /// <summary>
    ///  This methiod lists the common form types.
    /// </summary>
    /// <returns>List of Evado.Model.EvOption: a common form type list</returns>
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
    public static List<Evado.Model.EvOption> getFormTypes ( )
    {
      //
      // Initialize a return list and an option object
      //
      List<Evado.Model.EvOption> list = new List<Evado.Model.EvOption> ( );

      list.Add ( new Evado.Model.EvOption ( EdRecordTypes.Null.ToString ( ), String.Empty ) );

      //
      // Pull items' value from optoin object and add to the return list.
      //
      list.Add (  Evado.Model.EvStatics.getOption ( EdRecordTypes.Normal_Record ) );

      list.Add (  Evado.Model.EvStatics.getOption ( EdRecordTypes.Comment_Record ) );

      list.Add (  Evado.Model.EvStatics.getOption ( EdRecordTypes.Questionnaire ) );

      list.Add (  Evado.Model.EvStatics.getOption ( EdRecordTypes.Updatable_Record ) );


      return list;
    }//END getCommonFormTypes method

    //  =================================================================================
    /// <summary>
    ///  This method generates the list of Clinical Record states types.
    /// </summary>
    /// <returns>List of Evado.Model.EvOption: a form state list</returns>
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
    public static List<Evado.Model.EvOption> getFormStates ( )
    {
      //
      // Initialize a return list and an option object.
      //
      List<Evado.Model.EvOption> list = new List<Evado.Model.EvOption> ( );

      list.Add ( new Evado.Model.EvOption ( String.Empty, String.Empty ) );

      //
      // Pull items' value from option object and add them to a return list.
      //
      list.Add (  Evado.Model.EvStatics.getOption ( EdRecordObjectStates.Form_Draft ) );

      list.Add (  Evado.Model.EvStatics.getOption ( EdRecordObjectStates.Form_Reviewed ) );

      list.Add (  Evado.Model.EvStatics.getOption ( EdRecordObjectStates.Form_Issued ) );

      list.Add (  Evado.Model.EvStatics.getOption ( EdRecordObjectStates.Withdrawn ) );

      return list;
    }//END getFormStates method.

    //  =================================================================================
    /// <summary>
    ///  This class generates the list of Clinical Record states types.
    /// </summary>
    /// <returns>List of Evado.Model.EvOption: a subject record state list</returns>
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
    public static List<Evado.Model.EvOption> getSubjectRecordStates ( )
    {
      //
      // Initialize a return list and an option object.
      //
      List<Evado.Model.EvOption> list = new List<Evado.Model.EvOption> ( );

      list.Add ( new Evado.Model.EvOption ( String.Empty, String.Empty ) );

      //
      // Retrieve an option object and append the items' value to the return list.
      //
      list.Add (  Evado.Model.EvStatics.getOption ( EdRecordObjectStates.Draft_Record ) );

      list.Add (  Evado.Model.EvStatics.getOption ( EdRecordObjectStates.Submitted_Record ) );


      list.Add (  Evado.Model.EvStatics.getOption ( EdRecordObjectStates.Withdrawn ) );

      return list;
    }//END getSubjectRecordStates method.

    //  =================================================================================
    /// <summary>
    ///  This class generates the list of Clinical Record states types.
    /// </summary>
    /// <returns>List of Evado.Model.EvOption: a list  of options</returns>
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
    public static List<Evado.Model.EvOption> getRecordStates ( bool excludeCopies )
    {
      //
      // Initialize a return list and an option object.
      //
      List<Evado.Model.EvOption> list = new List<Evado.Model.EvOption> ( );

      Evado.Model.EvOption Option = new Evado.Model.EvOption (
        String.Empty,
        Evado.Digital.Model.EdLabels.All_Active_Project_Records_Selection_Option );
      list.Add ( Option );

      //
      // Pull items' value from an option object and add them to the return list.
      //
      Option = new Evado.Model.EvOption (
        EdRecordObjectStates.Draft_Record.ToString ( ),
        Evado.Digital.Model.EdLabels.Record_State_Draft_Record );
      list.Add ( Option );

      Option = new Evado.Model.EvOption (
        EdRecordObjectStates.Submitted_Record.ToString ( ),
        Evado.Digital.Model.EdLabels.Record_State_Submitted_Record );
      list.Add ( Option );

      Option = new Evado.Model.EvOption (
        EdRecordObjectStates.Withdrawn.ToString ( ),
        Evado.Digital.Model.EdLabels.Record_State_Withdrawn );
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
    public static List<Evado.Model.EvOption> getRptRecordStates ( )
    {
      List<Evado.Model.EvOption> list = new List<Evado.Model.EvOption> ( );

      Evado.Model.EvOption Option = new Evado.Model.EvOption ( String.Empty, String.Empty );
      list.Add ( Option );
      //
      // Pull items' value from an option object and add them to the return list.
      //
      Option = new Evado.Model.EvOption ( EdRecordObjectStates.Draft_Record.ToString ( ),
        Evado.Model.EvStatics.enumValueToString ( EdRecordObjectStates.Draft_Record ) );
      list.Add ( Option );

      Option = new Evado.Model.EvOption ( EdRecordObjectStates.Submitted_Record.ToString ( ),
        Evado.Model.EvStatics.enumValueToString ( EdRecordObjectStates.Submitted_Record ) );
      list.Add ( Option );

      return list;

    }//ENd getRptRecordStates method
    #endregion

  }//END class EvForm

}//END namespace Evado.Digital.Model
