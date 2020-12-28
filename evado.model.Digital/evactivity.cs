/***************************************************************************************
 * <copyright file="model\EvActivity.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvActivity data object.
 *
 ****************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

namespace Evado.Model.Digital
{

  /// <summary>
  /// This class defines the data model for  trial or registry schedule milestone activities. 
  /// </summary>
  [Serializable]
  public class EvActivity : EvHasSetValue<EvActivity.ActivityClassFieldNames>
  {
    #region Public Enumerators.
    /// <summary>
    /// This enumeration list defines the state of Activity Types
    /// </summary>
    public enum ActivityTypes
    {
      /// <summary>
      /// This enumeration defines null value or no selection state
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines an activity type for clinical activities
      /// </summary>
      Clinical,

      /// <summary>
      /// This enumeration defines an activity type for Non clinical activities
      /// </summary>
      Non_Clinical,

      /// <summary>
      /// This enumeration defines an activity type for patient record activities
      /// </summary>
      Patient_Record,

    }

    /// <summary>
    /// This enumeration list defines the state of Activity Selection
    /// </summary>
    public enum ActivitySelection
    {
      /// <summary>
      /// This enumeration defines the Null Value or  no selection state
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines an activity selection for A mandatory activity
      /// </summary>
      Mandatory,

      /// <summary>
      /// This enumeration defines an activity selection for an optional activity.
      /// </summary>
      Optional,

      /// <summary>
      /// This enumeration defines an activity selection for an optional activity.
      /// </summary>
      Non_Clinical
    }

    /// <summary>
    /// This enumeration list defines the activity states.
    /// </summary>
    public enum ActivityStates
    {
      /// <summary>
      /// This enumeration defines the Null Value or no selection state
      /// </summary>
      Null,

      /// <summary>
      /// The Activity record has not been selected in the database.
      /// </summary>
      Un_Selected,

      /// <summary>
      /// The Activity record has been created but not saved to the database.
      /// </summary>
      Created,

      /// <summary>
      /// The activity record has been completed.
      /// </summary>
      Completed,

      /// <summary>
      /// The activity record has been billed.
      /// </summary>
      Billed
    }

    /// <summary>
    /// This enumeration list defines the state of Activity Selection
    /// </summary>
    public enum ActivityValidation
    {
      /// <summary>
      /// This enumeration defines the Null Value or  no selection state
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines an activity is valid only for males
      /// </summary>
      Male_Subject_Only,

      /// <summary>
      /// This enumeration defines an activity is valid only for females.
      /// </summary>
      Female_Subject_Only
    }

    /************************************************************************************
     * 
     * Non-clinical activities associated with a clinical milestone will be signed off
     * as completed when the milestone is signed off as complete.
     * 
     ************************************************************************************/
    /// <summary>
    /// This enumeration list defines the activity filenames for data update or extraction.
    /// </summary>
    public enum ActivityClassFieldNames
    {
      /// <summary>
      /// This enumeration defines the Null Value or no selection state
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines the trial identifier of activity field names.
      /// </summary>
      ProjectId,

      /// <summary>
      /// This enumeration defines the activity identifier of activity field names.
      /// </summary>
      ActivityId,

      /// <summary>
      /// This enumeration defines the activity title of activity field names.
      /// </summary>
      Title,

      /// <summary>
      /// This enumeration defines the activity description of activity field names..
      /// </summary>
      Description,

      /// <summary>
      /// This enumeration defines the activity order of activity field names.
      /// </summary>
      Order,

      /// <summary>
      /// This enumeration defines the activity type of activity field names.
      /// </summary>
      Type,

      /// <summary>
      /// This enumeration defines the activity form list of activity field names.
      /// </summary>
      FormList,

      /// <summary>
      /// This enumeration defines the activity form list of activity Validation names.
      /// </summary>
      ValidationRule
    }

    /// <summary>
    /// The enumeration list defines the state of miilestone activity action codes.
    /// </summary>
    public enum ActivitiesActionsCodes
    {
      /// <summary>
      ///  This enumeration defines the default state, skip do nothing.
      /// </summary>
      Skip = -1,

      /// <summary>
      /// This enumeration defines save the action state.
      /// </summary>
      Save = 0,

      /// <summary>
      /// This enumeration defines delete the action state.
      /// </summary>
      Delete = 1,
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
    
    #region Properties

    private Guid _Guid = Guid.Empty;

    /// <summary>
    /// This property defines a global unique identifier
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

    private Guid _MilestoneGuid = Guid.Empty;

    /// <summary>
    /// This property defines a milestone of a global unique identifier
    /// </summary>
    public Guid MilestoneGuid
    {
      get
      {
        return this._MilestoneGuid;
      }
      set
      {
        this._MilestoneGuid = value;
      }
    }

    private Guid _MilestoneActivityGuid = Guid.Empty;
    /// <summary>
    /// This property defines a milestone activity of a global unique identifier
    /// </summary>
    public Guid MilestoneActivityGuid
    {
      get
      {
        return this._MilestoneActivityGuid;
      }
      set
      {
        this._MilestoneActivityGuid = value;
      }
    }

    private string _ProjectId = String.Empty;
    /// <summary>
    /// This property defines a trial identifier
    /// </summary>
    public string ProjectId
    {
      get
      {
        return this._ProjectId;
      }
      set
      {
        this._ProjectId = value;
      }
    }

    private string _OrgId = String.Empty;
    /// <summary>
    /// This property defines an organization identifier
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

    private string _ActivityId = String.Empty;
    /// <summary>
    /// This property defines an activity identifier
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

    private string _SubjectId = String.Empty;
    /// <summary>
    /// This property defines a subject identifier
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

    private string _VisitId = String.Empty;
    /// <summary>
    /// This property define a visit identifier
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

    private string _Title = String.Empty;
    /// <summary>
    /// This property contains an activity title
    /// </summary>
    public string Title
    {
      get
      {
        return this._Title;
      }
      set
      {
        this._Title = value;
        this._Description = value;
      }
    }

    private string _Description = String.Empty;
    /// <summary>
    /// This property contains an activity description
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

    private ActivityValidation _ValidationRule = ActivityValidation.Null;
    /// <summary>
    /// This property identifies the validation identifier
    /// </summary>

    public ActivityValidation ValidationRule
    {
      get { return _ValidationRule; }
      set { _ValidationRule = value; }
    }

    private int _InitialVersion = 1;
    /// <summary>
    /// This property defines an initial version of activity
    /// </summary>
    public int InitialVersion
    {
      get
      {
        return this._InitialVersion;
      }
      set
      {
        this._InitialVersion = value;
      }
    }

    /// <summary>
    /// This property contains the activity details
    /// </summary>
    public string LinkText
    {
      get
      {
        if ( this.ActivityId == String.Empty )
        {
          return EvLabels.Activity_Empty_Link_Text;
        }

        String linkText = String.Format (
             EvLabels.Activity_ID_Title_Type_Link_Text,
             this.ActivityId,
             this._Title,
             this._Type );

        if ( this._Description != String.Empty
          && this._Title != this._Description )
        {
          linkText += ", " + this._Description;
        }

        if ( this._FormList.Count > 0 )
        {
          linkText += String.Format ( EvLabels.Activity_Form_Count_Link_Text, this._FormList.Count );
        }

        return linkText;
      }
    }

    private EvActivity.ActivityTypes _Type = EvActivity.ActivityTypes.Clinical;
    /// <summary>
    /// This property defines the activity type object
    /// </summary>
    public EvActivity.ActivityTypes Type
    {
      get
      {
        return this._Type;
      }
      set
      {
        this._Type = value;
      }
    }
    /// <summary>
    /// This property indicates if the activity is clinical.
    /// </summary>
    public bool IsClinical
    {
      get
      {
        if ( this._Type == EvActivity.ActivityTypes.Clinical
          || this._Type == EvActivity.ActivityTypes.Patient_Record )
        {
          return true;
        }
        return false;
      }
    }

    private ActivityStates _Status = ActivityStates.Null;
    /// <summary>
    /// This property defines activity status
    /// </summary>
    public ActivityStates Status
    {
      get
      {
        return this._Status;
      }
      set
      {
        this._Status = value;
      }
    }

    /// <summary>
    /// This property contains the description of activity's status
    /// </summary>
    public string StateDesc
    {
      get
      {
        return EvcStatics.Enumerations.enumValueToString ( this._Status );
      }
      set
      {
        this._Status = EvcStatics.Enumerations.parseEnumValue<ActivityStates> ( value );
      }
    }

    private bool _IsMandatory = false;
    /// <summary>
    /// This property indicates whether the activity is mandatory
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
    /// This property contains an activity's manadatory display
    /// </summary>
    public String MandatoryDisp
    {
      get
      {
        if ( this._IsMandatory == true )
        {
          return "Yes";
        }
        return "No";

      }
      set
      {
        string v = value;
      }
    }

    private int _Order = 0;
    /// <summary>
    /// This property defines an activity order
    /// </summary>
    public int Order
    {
      get
      {
        return this._Order;
      }
      set
      {
        this._Order = value;
      }
    }

    private float _Quantity = 1;
    /// <summary>
    /// This property defines an activity quantity
    /// </summary>
    public float Quantity
    {
      get
      {
        return this._Quantity;
      }
      set
      {
        this._Quantity = value;
      }
    }

    private DateTime _CompletionDate = EvcStatics.CONST_DATE_NULL;
    /// <summary>
    /// This property defines a completion date of activity
    /// </summary>
    public DateTime CompletionDate
    {
      get
      {
        return this._CompletionDate;
      }
      set
      {
        this._CompletionDate = value;
      }
    }

    /// <summary>
    /// This property formats the completion date of activity
    /// </summary>
    public string stCompletionDate
    {
      get
      {
        if ( this._CompletionDate > EvcStatics.CONST_DATE_NULL )
        {
          return this._CompletionDate.ToString ( "dd MMM yyyy HH:mm " );
        }
        return String.Empty;
      }

      set
      {
        DateTime dValue = this._CompletionDate;

        if ( DateTime.TryParse ( value, out this._CompletionDate ) == false )
        {
          this._CompletionDate = dValue;
        }
      }
    }

    private string _CompletedBy = String.Empty;
    /// <summary>
    /// This property contains the user who complete the activity
    /// </summary>
    public string CompletedBy
    {
      get
      {
        return this._CompletedBy;
      }
      set
      {
        this._CompletedBy = value;
      }
    }

    private bool _ProtocolViolation = false;
    /// <summary>
    /// This property indicates whether the activity protocal is violation
    /// </summary>
    public bool ProtocolViolation
    {
      get
      {
        return this._ProtocolViolation;
      }
      set
      {
        this._ProtocolViolation = value;
      }
    }

    private string _Comments = String.Empty;
    /// <summary>
    /// This property contains the activtity's comments
    /// </summary>
    public string Comments
    {
      get
      {
        return this._Comments;
      }
      set
      {
        this._Comments = value;
      }
    }

    /// <summary>
    /// This property formats the activity's comments into Html
    /// </summary>
    public string CommentsHtml
    {
      get
      {
        return this._Comments.Replace ( "\r\n", "<br/>" ); ;
      }
      set
      {
        string v = value;
      }
    }

    private bool _Selected = false;
    /// <summary>
    /// This property indicates whether the activity is selected
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

    private List<EvActvityForm> _FormList = new List<EvActvityForm> ( );
    /// <summary>
    /// This property contains a form list of an activity
    /// </summary>
    public List<EvActvityForm> FormList
    {
      get
      {
        return this._FormList;
      }
      set
      {
        this._FormList = value;
      }
    }

    /// <summary>
    /// This property contains the record details of an activity
    /// </summary>
    public string RecordDetails
    {
      get
      {
        string stRecordDetails = String.Empty;

        // 
        // Activity Forms exists.
        // 
        if ( this._FormList.Count > 0 )
        {
          stRecordDetails += "Activity Forms:";

          foreach ( EvActvityForm form in this._FormList )
          {
            if ( form.FormId != String.Empty )
            {
              stRecordDetails += "\r\n" + form.FormId + " - " + form.FormTitle;
              if ( form.Mandatory == true )
              {
                stRecordDetails += " (Mandatory) ";
              }
            }
          }
        }//END Activity Forms exist.

        return stRecordDetails;
      }
      set
      {
        string v = value;
      }
    }

    private ActivitiesActionsCodes _Action = ActivitiesActionsCodes.Skip;
    /// <summary>
    /// This property defines an action code of an activity
    /// </summary>
    public ActivitiesActionsCodes Action
    {
      get
      {
        return this._Action;
      }
      set
      {
        this._Action = value;
      }
    }

    private DateTime _UpdatedDate = EvcStatics.CONST_DATE_NULL;
    /// <summary>
    /// This property defines a date of activity update
    /// </summary>
    public DateTime UpdatedDate
    {
      get
      {
        return this._UpdatedDate;
      }
      set
      {
        this._UpdatedDate = value;
      }
    }

    private string _UpdatedBy = String.Empty;
    /// <summary>
    /// This property defines a user who updates activity
    /// </summary>
    public string UpdatedBy
    {
      get
      {
        return this._UpdatedBy;
      }
      set
      {
        this._UpdatedBy = value;
      }
    }

    private string _UpdatedByUserId = String.Empty;
    /// <summary>
    /// This property defines a user identifier of those who updates activity
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

    private string _UserCommonName = String.Empty;
    /// <summary>
    /// This property defines a common name of those who updates activity
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


    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region public methods.
    //  =================================================================================
    /// <summary>
    /// This method creates a list of evOptions containing activity type options.
    /// </summary>
    /// <param name="Project">EvProject:  the project configuration object.</param>
    /// <param name="isSelectionList">Bool:  True = include blank option for selection list.</param>
    /// <returns>List of EvOption: List of All activities</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize the selection option list
    /// 
    /// 2. Create an option list object
    /// 
    /// 3. Add a null selection if the list is selected
    /// 
    /// 4. Add the activity types to the option list
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public static List<EvOption> getAllActivityTypeOptions (
      EdApplication Project,
      bool isSelectionList )
    {
      // 
      // Initialise the methods variables and objects
      // 
      List<EvOption> selectionList = new List<EvOption> ( );
      EvOption option = new EvOption ( );

      // 
      // If selection list is true add a Null selection.
      // 
      if ( isSelectionList == true )
      {
        selectionList.Add ( new EvOption ( ActivityTypes.Null.ToString ( ), String.Empty ) );
      }

      // 
      // Add the activity types to the list.
      // 
      selectionList.Add ( EvStatics.Enumerations.getOption ( ActivityTypes.Clinical ) );


      // 
      // Return the list of activity types
      // 
      return selectionList;

    }//END getAllActivities method

    //  ====================================================================
    /// <summary>
    /// This class obtains the list of clinical ActivitiTypes.
    /// 
    /// Author: Andres Castano
    /// 14 Dec 2009
    /// </summary>
    /// <returns>List of EvActivity.ActivityTypes: a list of Clinical Activities</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize the list of clinical activities
    /// 
    /// 2. Add the activity types to the list
    /// 
    /// </remarks>
    //  --------------------------------------------------------------------
    public static List<EvActivity.ActivityTypes> getClinicalActivityTypes ( )
    {
      // 
      // Initialise the list of clinical activities
      // 
      List<EvActivity.ActivityTypes> clinicalActivities = new List<EvActivity.ActivityTypes> ( );

      // 
      // Add the activity types to the list.
      // 
      clinicalActivities.Add ( ActivityTypes.Null );
      clinicalActivities.Add ( ActivityTypes.Clinical );

      // 
      // Return the list of activity types
      // 
      return clinicalActivities;

    }//END getClinicalActivities method

    //  ====================================================================
    /// <summary>
    /// This class obtains the list of non clinical ActivitiTypes.
    /// 
    /// Author: Andres Castano
    /// 14 Dec 2009
    /// </summary>
    /// <returns>List of EvActivity.ActivityTypes: List of Activity Types</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize the list of clinical activities
    /// 
    /// 2. Add the non activity types to the list
    /// 
    /// </remarks>
    //  --------------------------------------------------------------------
    public static List<EvActivity.ActivityTypes> getNonClinicalActivityTypes ( )
    {
      // 
      // Initialise the list of activity types
      // 
      List<EvActivity.ActivityTypes> clinicalActivities = new List<EvActivity.ActivityTypes> ( );

      // 
      // Add the non activity types to the list.
      // 
      clinicalActivities.Add ( ActivityTypes.Null );
      clinicalActivities.Add ( ActivityTypes.Non_Clinical );

      // 
      // Return the list of activity types
      // 
      return clinicalActivities;

    }//END getNonClinicalActivities method

    //  ====================================================================
    /// <summary>
    /// Creates a selection list from the list of types.
    /// 
    /// Author: Andres Castano
    /// 14 Dec 2009
    /// </summary>
    /// <param name="List">List: an activity type list</param>
    /// <returns>List of String: an option list</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize the option list
    /// 
    /// 2. Iterate through the activity type list
    /// 
    /// 3. Add activity types into the option list
    /// </remarks>
    //  --------------------------------------------------------------------
    public static List<String> getOptionList (
      List<EvActivity.ActivityTypes> List )
    {
      //
      // Initialize the option list
      //
      List<String> dataSource = new List<String> ( );

      //
      // Iterate through the activity type list
      //
      foreach ( EvActivity.ActivityTypes actType in List )
      {
        //
        // Add the activity types into the option list
        //
        dataSource.Add ( EvcStatics.Enumerations.enumValueToString ( actType ) );
      }
      return dataSource;

    }//END getOptionList method

    //  ================================================================================
    /// <summary>
    /// Sets the value of this activity class field name. Validate the format of the
    /// value. 
    /// </summary>
    /// <param name="fieldName">ActivityClassFieldNames: Name of the field to be setted.</param>
    /// <param name="value">String: value to be setted</param>
    /// <returns>EvEventCodes: indicating the successful update of the property value.</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Switch the fieldName and update value for the property defined by the activity field names
    /// 
    /// 2. Return casting error, if field name type is empty
    /// </remarks>
    //  --------------------------------------------------------------------------------
    public EvEventCodes setValue ( ActivityClassFieldNames fieldName, String value )
    {
      //
      // Switch the FieldName based on the activity field names
      //
      switch ( fieldName )
      {
        case ActivityClassFieldNames.ProjectId:
          {
            this.ProjectId = value;
            break;
          }
        case ActivityClassFieldNames.ActivityId:
          {
            this.ActivityId = value;
            break;
          }
        case ActivityClassFieldNames.Title:
          {
            this.Title = value;
            break;
          }
        case ActivityClassFieldNames.Description:
          {
            this.Description = value;
            break;
          }
        //
        // If Field Name type does not exist, return casting error
        //
        case ActivityClassFieldNames.Type:
          {
            ActivityTypes type = ActivityTypes.Null;
            if ( EvcStatics.Enumerations.tryParseEnumValue<ActivityTypes> ( value, out type ) == false )
            {
              return EvEventCodes.Data_Enumeration_Casting_Error;
            }
            this.Type = type;
            break;
          }
      }// End switch field name

      return EvEventCodes.Ok;

    }//End setValue method.

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

  } //END EvActivity class

} //END namespace Evado.Model
