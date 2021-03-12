/***************************************************************************************
 * <copyright file="EvAlert.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvAlert data object.
 *
 ****************************************************************************************/


using System;
using System.Collections;
using System.Collections.Generic;

namespace Evado.Model.Digital
{
  /// <summary>
  /// Business entity used to model accounts
  /// </summary>
  [Serializable]
  public class EvAlert : EvHasSetValue<EvAlert.AlertFieldNames>
  {
    #region Class Enumerators

    //  ================================================================================
    /// <summary>
    /// This enumeration defines the types of alerts that can generated.
    /// </summary>
    //  --------------------------------------------------------------------------------
    public enum AlertTypes
    {
      /// <summary>
      /// This enumeration defines a null alert type.
      /// </summary>
      Null = 0,

      /// <summary>
      /// This enumeration defines a subject record alert type.
      /// </summary>
      Subject_Record = 1,

      /// <summary>
      /// This enumeration defines a adverse event alert type.
      /// </summary>
      Adverse_Event_Report = 2,

      /// <summary>
      /// This enumeration defines a concomitant medication alert type.
      /// </summary>
      Concomitant_Medication = 3,

      /// <summary>
      /// This enumeration defines aDLT alert type.
      /// </summary>
      Dose_Limit_Toxicity = 4,

      /// <summary>
      /// This enumeration defines a serious adverse event alert type.
      /// </summary>
      Serious_Adverse_Event_Report = 5,

      /// <summary>
      /// This enumeration defines a Periodic followup alert type.
      /// </summary>
      Periodic_Followup = 6,

      /// <summary>
      /// This enumeration defines a additiona followup alert type.
      /// </summary>
      Additional_Followup = 7,

      /// <summary>
      /// This enumeration defines a assessment record alert type.
      /// </summary>
      Assessment_Record = 8,

      /// <summary>
      /// This enumeration defines a trial record alert type.
      /// </summary>
      Trial_Record = 9,

      /// <summary>
      /// These items are for milestone alerts.
      /// </summary>
      Monthly_Milestone = 10,

      /// <summary>
      /// This enumeration defines a 3 month milestone alert type.
      /// </summary>
      Three_Monthly_Milestone = 11,

      /// <summary>
      /// This enumeration defines a 6 monthly milestone alert type.
      /// </summary>
      Six_Monthly_Milestone = 12,

      /// <summary>
      /// This enumeration defines a annual milestone alert type.
      /// </summary>
      Annual_Milestone = 13,

      /// <summary>
      /// This enumeration defines an automated milestone alert type.
      /// </summary>
      Auto_Milestone_Gen = 14,
      //
      // Notfication Alerts.
      //

      /// <summary>
      /// This enumeration defines a serious adverse event notification alert type.
      /// </summary>
      SAE_Notification_Alert = 15,

      /// <summary>
      /// This enumeration defines a adverse event alert type.
      /// </summary>
      AE_Notification_Alert = 16,


      /// <summary>
      /// This enumeration defines a CRF record query alert type.
      /// </summary>
      Query_Alert = 98,

      /// <summary>
      /// This enumeration defines a notification alert alert type.
      /// </summary>
      Notiifcation_Alert = 99,

      //
      //  Backward compatibilty.
      //
      /// <summary>
      /// Backward compatiblity value.
      /// </summary>
      AE = -2,
      /// <summary>
      /// Backward compatiblity value.
      /// </summary>
      CM = -3,
      /// <summary>
      /// Backward compatiblity value.
      /// </summary>
      DL = -4,
      /// <summary>
      /// Backward compatiblity value.
      /// </summary>
      SA = -5,
      /// <summary>
      /// Backward compatiblity value.
      /// </summary>
      TR = -9,

    }

    //  ================================================================================
    /// <summary>
    /// This enumeration list defines the alert states.
    /// </summary>
    //  --------------------------------------------------------------------------------
    public enum AlertStates
    {
      /// <summary>
      /// This enumeration defines a null alert state.
      /// </summary>
      Null = 0,

      /// <summary>
      /// This enumeration defines a raised alert state.
      /// </summary>
      Raised = 1,

      /// <summary>
      /// This enumeration defines a acknowledged alert state.
      /// </summary>
      Acknowledged = 2,

      /// <summary>
      /// This enumeration defines a closed alert state.
      /// </summary>
      Closed = 3,

      /// <summary>
      /// This enumeration defines a not clodes alert state.
      /// </summary>
      Not_Closed = 4,

      /// <summary>
      ///  Backward compatibilty.
      /// </summary>
      AL_CL = 4,

    }

    //  ================================================================================
    /// <summary>
    /// This enumeration list defines the alert save action codes
    /// </summary>
    //  --------------------------------------------------------------------------------
    public enum AlertSaveActionCodes
    {
      /// <summary>
      /// This enumeration defines null alert save action code.
      /// </summary>
      Null = 0,

      /// <summary>
      /// This enumeration defines raised alert save action code.
      /// </summary>
      Raise_Alert,

      /// <summary>
      /// This enumeration defines acknowledged alert save action code.
      /// </summary>
      Acknowledge_Alert,

      /// <summary>
      /// This enumeration defines save alert save action code.
      /// </summary>
      Save_Alert,

      /// <summary>
      /// This enumeration defines close alert save action code.
      /// </summary>
      Close_Alert,
    }

    //  ================================================================================
    /// <summary>
    /// This enumeration list defines the alert field name codes.
    /// </summary>
    //  --------------------------------------------------------------------------------
    public enum AlertFieldNames
    {

      /// <summary>
      /// This enumeration defines null field name.
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines alert Guid field name.
      /// </summary>
      Alert_Guid,

      /// <summary>
      /// This enumeration defines alert ID field name.
      /// </summary>
      Alert_Id,

      /// <summary>
      /// This enumeration defines trial ID field name.
      /// </summary>
      Trial_Id,

      /// <summary>
      /// This enumeration defines to organisation ID field name.
      /// </summary>
      To_Org_Id,

      /// <summary>
      /// This enumeration defines alert subject field name.
      /// </summary>
      Alert_Subject,

      /// <summary>
      /// This enumeration definesnew message field name.
      /// </summary>
      New_Message,

      /// <summary>
      /// This enumeration defines message field name.
      /// </summary>
      Message,

      /// <summary>
      /// This enumeration definesfrom user id field name.
      /// </summary>
      From_User_UserId,

      /// <summary>
      /// This enumeration defines from user name field name.
      /// </summary>
      From_User,

      /// <summary>
      /// This enumeration defines to user name field name.
      /// </summary>
      To_User,

      /// <summary>
      /// This enumeration defines raised date field name.
      /// </summary>
      Raised_Date,

      /// <summary>
      /// This enumeration defines acknowledged user id field name.
      /// </summary>
      Acknowledged_By_UserId,

      /// <summary>
      /// This enumeration defines acknowledged user name field name.
      /// </summary>
      Acknowledged_By,

      /// <summary>
      /// This enumeration defines acknowledged date field name.
      /// </summary>
      Acknowledged_Date,

      /// <summary>
      /// This enumeration defines closed by userid field name.
      /// </summary>
      Closed_By_UserId,

      /// <summary>
      /// This enumeration defines closed by user name field name.
      /// </summary>
      Closed_By,

      /// <summary>
      /// This enumeration defines closed date field name.
      /// </summary>
      Closed_Date,

      /// <summary>
      /// This enumeration defines alert type identifier field name.
      /// </summary>
      Type_Id,

      /// <summary>
      /// This enumeration defines alert state field name.
      /// </summary>
      Alert_State,
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class constants
    /// <summary>
    /// This constant defines a notification event organization. 
    /// </summary>
    public const String NotificationEventOrganisation = "Notify";

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class members
    /// <summary>
    ///  The GUID identifier for the object.
    /// 
    /// </summary>
    private Guid _Guid = Guid.Empty;

    /// <summary>
    /// The serial used to generate the next alert identifer for a trial.
    /// 
    /// </summary>
    private int _Serial = 0;

    /// <summary>
    /// The alert identifier
    /// 
    /// </summary>
    private string _AlertId = String.Empty;

    /// <summary>
    ///  The trial identifier.
    /// 
    /// </summary>
    private string _ProjectId = String.Empty;

    /// <summary>
    ///  The record identifier.
    /// 
    /// </summary>
    private string _RecordId = String.Empty;

    /// <summary>
    ///  The organisation the alert is sent to.
    /// 
    /// </summary>
    private string _ToOrgId = String.Empty;

    /// <summary>
    /// The subject of the alert.
    /// 
    /// </summary>
    private string _Subject = String.Empty;

    /// <summary>
    /// New message fields when responding to an alert
    /// Its content is appended to the alert message.
    /// 
    /// </summary>
    private string _NewMessage = String.Empty;

    /// <summary>
    /// The aler message body.
    /// 
    /// </summary>
    private string _Message = String.Empty;

    /// <summary>
    /// The userid of the person sending the alert.
    /// (Left empty for system generated alerts).
    /// 
    /// </summary>
    private string _FromUserUserId = String.Empty;

    /// <summary>
    /// The name of the person sending the alert.
    /// ( 'Evado Executive' will be used for systm generated alerts.)
    /// 
    /// </summary>
    private string _FromUser = String.Empty;

    /// <summary>
    /// The date the alert was raised.
    /// 
    /// </summary>
    private DateTime _Raised = EvcStatics.CONST_DATE_NULL;

    /// <summary>
    /// The person the alert is sent to.
    /// 
    /// </summary>
    private string _ToUser = String.Empty;

    /// <summary>
    /// The user id for person who acknowledged the alert.
    /// 
    /// </summary>
    private string _AcknowledgedByUserId = String.Empty;

    /// <summary>
    /// The person who acknowledged the alert.
    /// 
    /// </summary>
    private string _AcknowledgedBy = String.Empty;

    /// <summary>
    /// The date the alert was acknowledged.
    /// 
    /// </summary>
    private DateTime _Acknowledged = EvcStatics.CONST_DATE_NULL;

    /// <summary>
    /// The userid for the person who closed the alert.
    /// 
    /// </summary>
    private string _ClosedByUserId = String.Empty;

    /// <summary>
    /// The name of the person who cloased the alert.
    /// 
    /// </summary>
    private string _ClosedBy = String.Empty;

    /// <summary>
    ///  The date the alert was closed.
    /// 
    /// </summary>
    private DateTime _Closed = EvcStatics.CONST_DATE_NULL;

    /// <summary>
    /// The type of alert that was generated.
    /// </summary>
    private AlertTypes _TypeId = AlertTypes.Null;

    /// <summary>
    /// The state of the alert.
    /// 
    /// </summary>
    private AlertStates _State = AlertStates.Null;

    /// <summary>
    /// The name of the person who is currently editing the alert.
    /// 
    /// </summary>
    private string _BookedOutBy = String.Empty;

    /// <summary>
    /// The name of the person updating the alert.
    /// </summary>
    private string _UserCommonName = String.Empty;

    /// <summary>
    /// THe user ID of the person to updating the alert.
    /// 
    /// </summary>
    private string _UpdatedByUserId = String.Empty;

    /// <summary>
    /// text field to display the last person to update the alert.
    /// 
    /// </summary>
    private string _Updated = String.Empty;

    /// <summary>
    ///  THe update action.
    /// </summary>
    private AlertSaveActionCodes _Action = AlertSaveActionCodes.Null;

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class properties
    /// <summary>
    /// This propery contains a global unique identifier of an alert
    /// </summary>
    public Guid Guid
    {
      get
      {
        return _Guid;
      }
      set
      {
        _Guid = value;
      }
    }

    /// <summary>
    /// This propery contains a serial of an alert
    /// </summary>
    public int Serial
    {
      get
      {
        return _Serial;
      }
      set
      {
        _Serial = value;
      }
    }

    /// <summary>
    /// This propery contains an alert identifier of an alert
    /// </summary>
    public string AlertId
    {
      get
      {
        return _AlertId;
      }
      set
      {
        _AlertId = value;
      }
    }

    /// <summary>
    /// This propery contains a trial identifier of an alert
    /// </summary>
    public string ProjectId
    {
      get
      {
        return _ProjectId;
      }
      set
      {
        _ProjectId = value;
      }
    }

    /// <summary>
    /// This propery contains a record identifier of an alert
    /// </summary>
    public string RecordId
    {
      get
      {
        return _RecordId;
      }
      set
      {
        _RecordId = value;
      }
    }

    /// <summary>
    /// This propery contains an organization identifier that an alert is sent to
    /// </summary>
    public string ToOrgId
    {
      get
      {
        return _ToOrgId;
      }
      set
      {
        _ToOrgId = value;
      }
    }

    /// <summary>
    /// This propery contains a subject of an alert
    /// </summary>
    public string Subject
    {
      get
      {
        return _Subject;
      }
      set
      {
        _Subject = value;
      }
    }

    /// <summary>
    /// This propery contains a message of an alert
    /// </summary>
    public string Message
    {
      get
      {
        return _Message;
      }
      set
      {
        _Message = value;
      }
    }

    /// <summary>
    /// This propery contains a new message of an alert
    /// </summary>
    public string NewMessage
    {
      get
      {
        return _NewMessage;
      }
      set
      {
        _NewMessage = value;
      }
    }

    /// <summary>
    /// This propery contains a user that an alert is sent from
    /// </summary>
    public string FromUser
    {
      get
      {
        return _FromUser;
      }
      set
      {
        _FromUser = value;
      }
    }

    /// <summary>
    /// This propery contains a user identifier of those who generates an alert
    /// </summary>
    public string FromUserUserId
    {
      get
      {
        return _FromUserUserId;
      }
      set
      {
        _FromUserUserId = value;
      }
    }

    /// <summary>
    /// This propery contains a user that an alert is sent to
    /// </summary>
    public string ToUser
    {
      get
      {
        return _ToUser;
      }
      set
      {
        _ToUser = value;
      }
    }

    /// <summary>
    /// This propery contains a date that an alert is raised
    /// </summary>
    public DateTime Raised
    {
      get
      {
        return _Raised;
      }
      set
      {
        _Raised = value;
      }
    }

    /// <summary>
    /// This propery contains a user identifier that an alert is acknowledged by.
    /// </summary>
    public string AcknowledgedByUserId
    {
      get
      {
        return _AcknowledgedByUserId;
      }
      set
      {
        _AcknowledgedByUserId = value;
      }
    }

    /// <summary>
    /// This propery contains a user that an alert is acknowledged by.
    /// </summary>
    public string AcknowledgedBy
    {
      get
      {
        return _AcknowledgedBy;
      }
      set
      {
        _AcknowledgedBy = value;
      }
    }

    /// <summary>
    /// This propery contains a date time stamp that an alert is acknowledged.
    /// </summary>
    public DateTime Acknowledged
    {
      get
      {
        return _Acknowledged;
      }
      set
      {
        _Acknowledged = value;
      }
    }

    /// <summary>
    /// This propery contains a user identifier that an alert is closed by.
    /// </summary>
    public string ClosedByUserId
    {
      get
      {
        return _ClosedByUserId;
      }
      set
      {
        _ClosedByUserId = value;
      }
    }

    /// <summary>
    /// This propery contains a user that an alert is closed by.
    /// </summary>
    public string ClosedBy
    {
      get
      {
        return _ClosedBy;
      }
      set
      {
        _ClosedBy = value;
      }
    }

    /// <summary>
    /// This propery contains a date time stamp that an alert is closed.
    /// </summary>
    public DateTime Closed
    {
      get
      {
        return _Closed;
      }
      set
      {
        _Closed = value;
      }
    }

    /// <summary>
    /// This propery contains a type identifier of an alert
    /// </summary>
    public AlertTypes TypeId
    {
      get
      {
        return _TypeId;
      }
      set
      {
        _TypeId = value;
      }
    }

    /// <summary>
    /// This propery contains a state of an alert
    /// </summary>
    public AlertStates State
    {
      get
      {
        return _State;
      }
      set
      {
        _State = value;
      }
    }

    /// <summary>
    /// This propery contains a user who an alert is booked out by.
    /// </summary>
    public string BookedOutBy
    {
      get
      {
        return _BookedOutBy;
      }
      set
      {
        _BookedOutBy = value;
      }
    }

    /// <summary>
    /// This property defineds the details of an alert.
    /// </summary>
    public string Details
    {
      get
      {
        string details = String.Empty;

        if ( this._AlertId != string.Empty )
        {
          details += this._AlertId;
        }

        if ( this._Subject != string.Empty )
        {
          if ( details != string.Empty )
          {
            details += " - ";
          }

          details += this._Subject;
        }

        return details;

      }
    }

    /// <summary>
    /// This property contains the state description of an alert.
    /// </summary>
    public string StateDesc
    {
      get
      {
        return EvStatics.enumValueToString ( this._State );
      }
      set
      {
        string v = value;
      }
    }

    /// <summary>
    /// This property contains the save action codes of an alert. 
    /// </summary>
    public AlertSaveActionCodes Action
    {
      get
      {
        return _Action;
      }
      set
      {
        _Action = value;
      }
    }

    /// <summary>
    /// This property contains an updated alert. 
    /// </summary>
    public string Updated
    {
      get
      {
        return _Updated;
      }
      set
      {
        _Updated = value;
      }
    }

    /// <summary>
    /// This property contains a user identifier that an alert is updated by.
    /// </summary>
    public string UpdatedByUserId
    {
      get
      {
        return _UpdatedByUserId;
      }
      set
      {
        _UpdatedByUserId = value;
      }
    }

    /// <summary>
    /// This property defiens a user common name that an alert is updated by.
    /// </summary>
    public string UserCommonName
    {
      get
      {
        return _UserCommonName;
      }
      set
      {
        _UserCommonName = value;
      }
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region class public methods
    // =====================================================================================
    /// <summary>
    /// This class sets the value according with the field name.
    /// </summary>
    /// <param name="fieldname">AlertFieldNames: an alert's field name</param>
    /// <param name="Value">string: an alert's value</param>
    /// <returns>EvEventCodes: an event code</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize the local variables and objects of the method including: date, status and type
    /// 
    /// 2. Switch the fieldname and update value for the property defined by the alert field names.
    /// </remarks>
    //  ------------------------------------------------------------------------------------
    public EvEventCodes setValue ( EvAlert.AlertFieldNames fieldname, string Value )
    {
      // 
      // Initialise the methods local variables and objects.
      // 
      DateTime date = EvcStatics.CONST_DATE_NULL;
      EvAlert.AlertStates status = EvAlert.AlertStates.Null;
      EvAlert.AlertTypes type = EvAlert.AlertTypes.Null;

      // 
      // The switch determines the item to be updated.
      // 
      switch ( fieldname )
      {

        case EvAlert.AlertFieldNames.Alert_Id:
          {
            this._AlertId = Value;
            break;
          }
        case EvAlert.AlertFieldNames.Trial_Id:
          {
            this._ProjectId = Value;
            break;
          }
        case EvAlert.AlertFieldNames.To_Org_Id:
          {
            this._ToOrgId = Value;
            break;
          }
        case EvAlert.AlertFieldNames.To_User:
          {
            this._ToUser = Value;
            break;
          }
        case EvAlert.AlertFieldNames.Alert_Subject:
          {
            this._Subject = Value;
            break;
          }
        case EvAlert.AlertFieldNames.New_Message:
          this._NewMessage = Value;
          break;

        case EvAlert.AlertFieldNames.Message:
          this._Message = Value;
          break;

        case EvAlert.AlertFieldNames.From_User_UserId:
          this._FromUserUserId = Value;
          break;

        case EvAlert.AlertFieldNames.From_User:
          this.FromUser = Value;
          break;

        case EvAlert.AlertFieldNames.Raised_Date:
          if ( DateTime.TryParse ( Value, out date ) == true )
          {
            this._Raised = date;
            break;
          }
          else
          {
            return EvEventCodes.Data_Date_Casting_Error;
          }

        case EvAlert.AlertFieldNames.Acknowledged_By_UserId:
          {
            this._AcknowledgedByUserId = Value;
            break;
          }

        case EvAlert.AlertFieldNames.Acknowledged_By:
          {
            this._AcknowledgedBy = Value;
            break;
          }

        case EvAlert.AlertFieldNames.Acknowledged_Date:
          if ( DateTime.TryParse ( Value, out date ) == true )
          {
            this._Acknowledged = date;
            break;
          }
          else
          {
            return EvEventCodes.Data_Date_Casting_Error;
          }

        case EvAlert.AlertFieldNames.Closed_By_UserId:
          {
            this._ClosedByUserId.ToString ( );
            break;
          }

        case EvAlert.AlertFieldNames.Closed_By:
          {
            this._ClosedBy = Value;
            break;
          }

        case EvAlert.AlertFieldNames.Closed_Date:
          if ( DateTime.TryParse ( Value, out date ) == true )
          {
            this._Closed = date;
            break;
          }
          else
          {
            return EvEventCodes.Data_Date_Casting_Error;
          }

        case EvAlert.AlertFieldNames.Type_Id:
          {

            if ( EvStatics.tryParseEnumValue<EvAlert.AlertTypes> ( Value, out type ) == false )
            {
              return EvEventCodes.Data_Enumeration_Casting_Error;
            }

            this._TypeId = type;
            break;
          }

        case EvAlert.AlertFieldNames.Alert_State:
          {

            if ( EvStatics.tryParseEnumValue<EvAlert.AlertStates> ( Value, out status ) == false )
            {
              return EvEventCodes.Data_Enumeration_Casting_Error;
            }

            this._State = status;
            break;
          }
        default:

          return 0;

      }//END Switch

      return 0;

    }//END getValue method

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region static methods
    /// <summary>
    /// This static method returns a list of the alert states.
    /// </summary>
    /// <returns>List: List of Options</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Create a state option list
    /// 
    /// 2. Add states to an option list
    /// 
    /// </remarks>
    public static List<EvOption> getStateList ( )
    {
      //
      // Create an option list.
      //
      List<EvOption> options = new List<EvOption> ( );

      //
      // Add states to an option list.
      //
      options.Add ( new EvOption ( EvAlert.AlertStates.Null.ToString ( ), String.Empty ) );

      options.Add ( EvStatics.getOption ( EvAlert.AlertStates.Raised ) );

      options.Add ( EvStatics.getOption ( EvAlert.AlertStates.Acknowledged ) );

      options.Add ( EvStatics.getOption ( EvAlert.AlertStates.Closed ) );

      options.Add ( EvStatics.getOption ( EvAlert.AlertStates.Not_Closed ) );

      //
      // Return an option list.
      //
      return options;
    }

    /// <summary>
    /// This static method return a list of the alert types.
    /// </summary>
    /// <returns>List: List of Options</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Create a type option list
    /// 
    /// 2. Add type items to the option list.
    /// </remarks>
    public static List<EvOption> getTypeList ( )
    {
      //
      // Create a type option list.
      //
      List<EvOption> options = new List<EvOption> ( );

      //
      // Add type items to the option list
      //
      options.Add ( new EvOption ( EvAlert.AlertStates.Null.ToString ( ), String.Empty ) );

      options.Add ( EvStatics.getOption ( EvAlert.AlertTypes.Query_Alert ) );

      options.Add ( EvStatics.getOption ( EvAlert.AlertTypes.Notiifcation_Alert ) );

      options.Add ( EvStatics.getOption ( EvAlert.AlertTypes.SAE_Notification_Alert ) );

      options.Add ( EvStatics.getOption ( EvAlert.AlertTypes.AE_Notification_Alert ) );

      //
      // Return a type option list
      //
      return options;
    }

    #endregion

  }//END EvAlert method

}//END namespace Evado.Model.Digital
