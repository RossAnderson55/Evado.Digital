/***************************************************************************************
 * <copyright file="EdApplicationSettings.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the data model for the EdApplicationSettings object .  
 * 
 *
 ****************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

namespace Evado.Model.Digital
{
  /// <summary>
  /// Business entity used to model Trial
  /// </summary>
  [Serializable]
  public class EdApplication : Evado.Model.EvParameters
  {
    #region Emunerators

    /// <summary>
    /// This enumeration list defines the trial states
    /// </summary>
    public enum ApplicationStates
    {
      /// <summary>
      /// This enumeration defines null value or not selection state
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines all state of trial 
      /// </summary>
      All,

    }

    /// <summary>
    /// This enumeration list defines the trial types
    /// </summary>
    public enum ApplicationTypes
    {
      /// <summary>
      /// This enumeration defines null value or not selection state
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines a training trial
      /// </summary>
      Normal,
    }

    /// <summary>
    /// The enumeration list defines the trial action codes
    /// </summary>
    public enum TrialSavelActionCodes
    {
      /// <summary>
      /// This enumeration defines a save action of a project.
      /// </summary>
      Save,

      /// <summary>
      /// This enumeration defines a Save trial action with delete state.
      /// </summary>
      Delete_Application
    }


    /// <summary>
    /// This enumeration list defines the  field names
    /// </summary>
    public enum ApplicationFieldNames
    {
      /// <summary>
      /// This enumeration define an identifier field name of a trial
      /// </summary>
      ApplicationId,

      /// <summary>
      /// This enumeration define an service field name of a trial
      /// </summary>
      Trial_Service,

      /// <summary>
      /// This enumeration define a local trial type enumerated value
      /// </summary>
      Application_Type,

      /// <summary>
      /// This enumeration define a title field name of a trial
      /// </summary>
      Title,

      /// <summary>
      /// This enumeration define a reference field name of a trial
      /// </summary>
      Reference,

      /// <summary>
      /// This enumeration define a description field name of a trial
      /// </summary>
      Description,

      /// <summary>
      /// This enumeration define a collecting binary data field name of a trial
      /// </summary>
      Enable_Binary_Data,

      /// <summary>
      /// This enumeration define a milestone scheduling method field name of a trial
      /// </summary>
      Milestone_Scheduling_Method,

      /// <summary>
      /// This enumeration define a Questionnaire subject form fields
      /// </summary>
      Confirmation_Email_Subject,

      /// <summary>
      /// This enumeration define a Patient_Consent Email Period form fields
      /// </summary>
      Confirmation_Email_Body,

    }
    #endregion

    #region Constants

    /// <summary>
    /// The constants defines a seperator character used for the getValue and setValue methods.
    /// </summary>
    private const char SEPERATOR = ';';

    #endregion

    #region Class Properties

    private Guid _Guid = Guid.Empty;
    /// <summary>
    /// This property contains the global unique identifier for the trial record.
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

    /// <summary>
    /// This property contains an customer global unique identifier of the customer
    /// </summary>
    public Guid CustomerGuid { get; set; }

    /// <summary>
    /// This property contains an customer name.
    /// </summary>
    public String CustomerName { get; set; }

    private EvCustomer.ServiceTypes _ServiceType = EvCustomer.ServiceTypes.Lite;
    /// <summary>
    /// This property contains the enumerated value for service type for this trial.
    /// </summary>
    public EvCustomer.ServiceTypes ServiceType
    {
      get
      {
        return this._ServiceType;
      }
      set
      {
        this._ServiceType = value;
      }
    }

    private string _ApplicationId = String.Empty;
    /// <summary>
    /// This property contains the trial identifier.  It is a 10 character string.
    /// This identifier is used as the prefix for all other records 
    /// associated with this trial.
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

    private ApplicationTypes _type = ApplicationTypes.Null;
    /// <summary>
    /// This property contains the enumerated value fo TrialTypes for this trial.
    /// </summary>
    public ApplicationTypes Type
    {
      get
      {
        return this._type;
      }
      set
      {
        this._type = value;
      }
    }

    /// <summary>
    /// This property contains the text value fo the trial type selection.
    /// </summary>
    public string TypeDescription
    {
      get
      {
        string stType = this._type.ToString ( );

        return stType.Replace ( "_", " " );
      }
      set
      {
        string v = value;
      }
    }

    private string _Title = String.Empty;
    /// <summary>
    /// This property contains the trial title.
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
      }
    }

    private string _HttpReference = String.Empty;
    /// <summary>
    /// This property contains the HTTP reference link for the trial.
    /// </summary>
    public string HttpReference
    {
      get
      {
        return this._HttpReference;
      }
      set
      {
        this._HttpReference = value;
      }
    }

    private string _Description = String.Empty;
    /// <summary>
    /// This property contains a description of the trial.
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

    private ApplicationStates _State = ApplicationStates.Null;
    /// <summary>
    /// This property contains the enumerated value of TrialStates for the trial's current state. 
    /// </summary>
    public ApplicationStates State
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
    /// This property contains the text value for the trial status.
    /// </summary>
    public string StateDesc
    {
      get
      {
        string stState = this._State.ToString ( );

        return stState.Replace ( "_", " " );
      }
      set
      {
        string v = value;
      }
    }

    #region Settings Group

    // =====================================================================================
    /// <summary>
    /// This property indicates binary data is being collected.
    /// </summary>
    // -------------------------------------------------------------------------------------
    public bool EnableBinaryData
    {
      get
      {
        return EvStatics.getBool (
          this.getParameter ( ApplicationFieldNames.Enable_Binary_Data ) );
      }
      set
      {
        this.setParameter ( ApplicationFieldNames.Enable_Binary_Data,
          EvDataTypes.Boolean, value.ToString ( ) );
      }
    }


    #endregion

    private string _Updated = String.Empty;
    /// <summary>
    /// This property contains the name of the person who last updated the trial object.
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


    private TrialSavelActionCodes _Action = TrialSavelActionCodes.Save;
    /// <summary>
    /// This property contains the enumerated trial actions codes value defining the trials save action.
    /// </summary>
    public TrialSavelActionCodes Action
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

    private string _UserCommonName = String.Empty;
    /// <summary>
    /// This property contains the common name of the user saving the trial object.
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

    private string _UpdatedByUserId = String.Empty;
    /// <summary>
    ///  This property contains the network identifier of the user that saved the trial object.
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

    private bool _IsAuthenticatedSignature = false;
    /// <summary>
    /// This property indicates that the user was revalidated prior to saving the trial object.
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


    private string _ConfirmationEmailSubject = String.Empty;
    /// <summary>
    ///  This property contains the subject of the confirmation email to be sent to the subject
    /// </summary>
    public string ConfirmationEmailSubject
    {
      get
      {
        return this._ConfirmationEmailSubject;
      }
      set
      {
        this._ConfirmationEmailSubject = value;
      }
    }

    private string _ConfirmationEmailBody = String.Empty;
    /// <summary>
    ///  This property contains the body of the confirmation email to be sent to the subject.
    /// </summary>
    public string ConfirmationEmailBody
    {
      get
      {
        return this._ConfirmationEmailBody;
      }
      set
      {
        this._ConfirmationEmailBody = value;
      }
    } //


    List<EvUserSignoff> _Signoffs = new List<EvUserSignoff> ( );
    // ==================================================================================
    /// <summary>
    /// This property contains a user signoff list of the trial.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public List<EvUserSignoff> Signoffs
    {
      get
      { return _Signoffs; }
      set
      { _Signoffs = value; }
    }

    /// <summary>
    /// This property contain the list of subjects in this trial.
    /// This property is used to extract an XML object of the trial data.
    /// </summary>
    public List<EvOption> ScheduleList { get; set; }


    // END Properties

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class methods

    // ==================================================================================
    /// <summary>
    /// This method sets the field value.
    /// </summary>
    /// <param name="FieldName">TrialFieldNames: a retrieved field name object</param>
    /// <param name="Value">string: a value of field name</param>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize internal variables
    /// 
    /// 2. Switch FieldName of the trial and update value
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public void setValue ( ApplicationFieldNames FieldName, string Value )
    {
      //
      // Initialize internal variables
      //
      DateTime date = EvcStatics.CONST_DATE_NULL;
      //float fltValue = 0;
      //int intValue = 0;
      bool bValue = false;

      //
      // Switch FieldName of the trial and update value
      //
      switch ( FieldName )
      {
        case ApplicationFieldNames.ApplicationId:
          {
            this._ApplicationId = Value;
            return;
          }
        case ApplicationFieldNames.Trial_Service:
          {
            this.ServiceType = EvcStatics.Enumerations.parseEnumValue<EvCustomer.ServiceTypes> ( Value );
            return;
          }

        case ApplicationFieldNames.Title:
          {
            this._Title = Value;
            return;
          }
        case ApplicationFieldNames.Reference:
          {
            this._HttpReference = Value;
            return;
          }
        case ApplicationFieldNames.Description:
          {
            this._Description = Value;
            return;
          }
        case ApplicationFieldNames.Application_Type:
          {
            this.Type = EvcStatics.Enumerations.parseEnumValue<ApplicationTypes> ( Value );
            return;
          }

        case ApplicationFieldNames.Enable_Binary_Data:
          {
            this.EnableBinaryData = EvcStatics.getBool ( Value );
            return;
          }
        default:

          return;

      }//END Switch

    }//END getValue method

    #endregion

  }//END EvTrial class

}//END namespace Evado.Model.Digital
