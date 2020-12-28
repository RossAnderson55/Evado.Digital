/***************************************************************************************
 * <copyright file="model\EvSchedule.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the Schedule data object.
 *
 ****************************************************************************************/

using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Evado.Model.Digital
{

  /// <summary>
  /// This class defines the data model for trial or registry schedule. 
  /// </summary>
  [Serializable]
  public class EvSchedule : EvHasSetValue<EvSchedule.ScheduleClassFieldNames>
  {

    #region State Enumerator
    /// <summary>
    /// This enumerated list defines the class field names for schedule object.
    /// </summary>
    public enum ScheduleClassFieldNames
    {
      /// <summary>
      /// This enumeration defines null value or not selection state
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines the project id value field
      /// </summary>
      TrialId,

      /// <summary>
      /// This enumeration defines the schedule Id field
      /// </summary>
      ScheduleId,

      /// <summary>
      /// This enumeration defines the schedule title field.
      /// </summary>
      Title,

      /// <summary>
      /// This enumeration defines the schedule description field.
      /// </summary>
      Description,

      /// <summary>
      /// This enumeration defines the schedule versin field.
      /// </summary>
      Version,

      /// <summary>
      /// This enumeration defines the schedule arm indexo field.
      /// </summary>
      ArmIndex,

      /// <summary>
      /// This enumeration defines the milestoine schedule increment
      /// </summary>
      Milestone_Period_Increment,

      /// <summary>
      /// This enumeration defines the schedule Type
      /// </summary>
      Schedule_Type,
    }

    /// <summary>
    /// The enumerator list defines the schedule types
    /// </summary>
    public enum ScheduleTypes
    {
      /// <summary>
      /// This enumerated value defines the clinical schedule.
      /// </summary>
      Clinical,

      /// <summary>
      /// This enumerated value defines a non-clinical schedule.  This includes:
      /// Manual milestones, Periodic milestones and status event milestones.
      /// </summary>
      Non_Clinical,

      /// <summary>
      /// This enumerated value defines a patient recorded observation schedule.
      /// </summary>
      PRO_Schedule,
    }

    /// <summary>
    /// The enumerator list defines the schedule milestone period increments
    /// </summary>
    public enum MilestonePeriodIncrements
    {
      /// <summary>
      /// This enumerated value indicates that the milestone period incrments are in days.
      /// </summary>
      Days = 0,

      /// <summary>
      /// This enumerated value indicates that the milestone period incrments are in weeks.
      /// </summary>
      Weeks = 1,

      /// <summary>
      /// This enumerated value indicates that the milestone period incrments are in months.
      /// </summary>
      Months = 2,
    }

    /// <summary>
    /// This enumeration list defines states of schedule
    /// </summary>
    public enum ScheduleStates
    {
      /// <summary>
      /// This enumeration defines null value or not selection state
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines a draft state of schedule
      /// </summary>
      Draft,

      /// <summary>
      /// This enumeration defines a reviewed state of schedule
      /// </summary>
      Reviewed,

      /// <summary>
      /// This enumeration defines an issued state of schedule
      /// </summary>
      Issued,

      /// <summary>
      /// This enumeration defines a withdrawn state of schedule
      /// </summary>
      Withdrawn
    }

    /// <summary>
    /// This enumeration list defines the actions of the Schedule object
    /// </summary>
    public enum ScheduleActions
    {
      /// <summary>
      /// This enumeration defines null value or not selection state
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines the save action of schedule object
      /// </summary>
      Save,

      /// <summary>
      /// This enumeration defines the review action of schedule object
      /// </summary>
      Review,

      /// <summary>
      /// This enumeration defines the approve action of schedule object
      /// </summary>
      Approve,

      /// <summary>
      /// This enumeration defines the delete action of schedule object
      /// </summary>
      Delete_Schedule,

      /// <summary>
      /// This enumeration defines the revise action of schedule object
      /// </summary>
      Revise
    }

    #endregion

    #region constants

    /// <summary>
    /// The minimum schedule identifier is 1
    /// </summary>
    public const int CONST_MINIMUM_SCHEDULE_ID = 1;

    /// <summary>
    /// The maximum schedule identifier is 20
    /// </summary>
    public const int CONST_MAXIMUM_SCHEDULE_ID = 20;


    #endregion

    #region properties members

    private Guid _Guid = Guid.Empty;
    /// <summary>
    /// This property contains a global unique identifier of schedule.
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
    private Guid _CustomerGuid = Guid.Empty;
    /// <summary>
    /// This property contains the customer Guid foriegn key to filter the subject by its customer.
    /// </summary>
    public Guid CustomerGuid
    {
      get
      {
        return this._CustomerGuid;
      }
      set
      {
        this._CustomerGuid = value;
      }
    }

    private string _TrialId = String.Empty;
    /// <summary>
    /// This property contains a trial identifier of schedule.
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

    private int _ScheduleId = 1;
    /// <summary>
    /// This property contains scheduleId of schedule.
    /// </summary>
    public int ScheduleId
    {
      get
      {
        return this._ScheduleId;
      }
      set
      {
        this._ScheduleId = value;
      }
    }

    private string _Title = String.Empty;
    /// <summary>
    /// This property contains title of schedule.
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

    private string _Description = String.Empty;
    /// <summary>
    /// This property contains description of schedule.
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

    private EvSchedule.ScheduleTypes _Type = ScheduleTypes.Clinical;

    /// <summary>
    /// This property defines the type of schedule the default is a 'Clinical' schedule.
    /// </summary>
    public EvSchedule.ScheduleTypes Type
    {
      get { return this._Type; }
      set
      {
        _Type = value;

        //
        // For non clinical schedules there can be only one schedule for schedule type.
        //
        if ( this._Type != ScheduleTypes.Clinical )
        {
          this._ScheduleId = 0;
        }
      }
    }

    private EvSchedule.MilestonePeriodIncrements _MilestonePeriodIncrement = EvSchedule.MilestonePeriodIncrements.Days;

    /// <summary>
    /// This property contains the milestone range increment for the
    /// </summary>
    public EvSchedule.MilestonePeriodIncrements MilestonePeriodIncrement
    {
      get { return _MilestonePeriodIncrement; }
      set { _MilestonePeriodIncrement = value; }
    }

    private EvSchedule.ScheduleStates _State = EvSchedule.ScheduleStates.Null;
    /// <summary>
    /// This property contains a state object of schedule.
    /// </summary>
    public EvSchedule.ScheduleStates State
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
    /// This property contains state description of schedule.
    /// </summary>
    public string StateDesc
    {
      get
      {
        return this._State.ToString ( );
      }
      set
      {
        string sValue = value;
      }
    }

    /// <summary>
    /// This property contains details of schedule.
    /// </summary>
    public string LinkText
    {
      get
      {
        return
          String.Format ( EvLabels.Schedule_Link_Text, 
          this._ScheduleId, 
          this._Title, 
          this._Version.ToString ( "00" ), 
          this.StateDesc );
      }
    }

    /// <summary>
    /// This property contains details of schedule.
    /// </summary>
    public string Details
    {
      get
      {
        string details = String.Empty;

        if ( this._Version >= 0 )
        {
          details = "Version: " + this._Version
             + " State: " + this._State.ToString ( )
             + "\r\n" + this._Description;

          if ( this._State == ScheduleStates.Issued
            || this._State == ScheduleStates.Withdrawn )
          {
            details += "\r\n Approved by " + this._ApprovedBy
              + " on " + this.stApprovedDate;
          }

        }
        return details;
      }
    }

    private int _Version = 1;
    /// <summary>
    /// This property contains a version of schedule.
    /// </summary>
    public int Version
    {
      get
      {
        return this._Version;
      }
      set
      {
        this._Version = value;
      }
    }

    /// <summary>
    /// This property contains a version string of schedule.
    /// </summary>
    public string stVersion
    {
      get
      {
        return this._Version.ToString ( "0000" ); ;
      }
    }

    private string _ApprovedByUserId = String.Empty;
    /// <summary>
    /// This property contains a user identifier of those who approves schedule.
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

    private string _ApprovedBy = String.Empty;
    /// <summary>
    /// This property contains a user who approves  schedule.
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

    private DateTime _ApprovedDate = EvcStatics.CONST_DATE_NULL;
    /// <summary>
    /// This property contains an approved date of schedule.
    /// </summary>
    public DateTime ApprovedDate
    {
      get
      {
        return this._ApprovedDate;
      }
      set
      {
        this._ApprovedDate = value;
      }
    }

    /// <summary>
    /// This property contains an approved date string of schedule.
    /// </summary>
    public string stApprovedDate
    {
      get
      {
        if ( this._ApprovedDate > EvcStatics.CONST_DATE_NULL )
        {
          return this._ApprovedDate.ToString ( "dd MMM yyyy" );
        }
        return String.Empty;
      }
      set
      {
        if ( value == String.Empty )
        {
          this._ApprovedDate = EvcStatics.CONST_DATE_NULL;
          return;
        }
        DateTime date = this._ApprovedDate;

        if ( DateTime.TryParse ( value, out date ) == true )
        {
          this._ApprovedDate = date;
        }
      }
    }


    private List<EvMilestone> _Milestones = new List<EvMilestone> ( );
    /// <summary>
    /// This property contains milestones object list of schedule.
    /// </summary>
    public List<EvMilestone> Milestones
    {
      get
      {
        return this._Milestones;
      }
      set
      {
        this._Milestones = value;
      }
    }

    private List<EvUserSignoff> _Signoffs = new List<EvUserSignoff> ( );
    /// <summary>
    /// This property contains signoffs object list of schedule.
    /// </summary>
    public List<EvUserSignoff> Signoffs
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

    /// <summary>
    /// This property indicates whether the schedule is issued.
    /// </summary>
    public bool IsIssued
    {
      get
      {
        if ( this._ApprovedDate > EvcStatics.CONST_DATE_NULL )
        {
          return true;
        }
        return false;
      }
      set
      {
        bool bvalue = value;
      }
    }

    private bool _IsAuthenticatedSignature = false;
    /// <summary>
    /// This property indicates whether the schedule has authenticated signature.
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

    private EvSchedule.ScheduleActions _Action = EvSchedule.ScheduleActions.Null;
    /// <summary>
    /// This property contains an action of schedule.
    /// </summary>
    public EvSchedule.ScheduleActions Action
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

    private string _UpdatedByUserId = String.Empty;
    /// <summary>
    /// This property contains a user identifier of those who updates schedule.
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
    /// This property contains a user common name of those who updates schedule.
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

    private string _UpdatedBy = String.Empty;
    /// <summary>
    /// This property contains a user who updates schedule.
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

    private DateTime _UpdateDate = EvcStatics.CONST_DATE_NULL;
    /// <summary>
    /// This property contains an updated date of schedule.
    /// </summary>
    public DateTime UpdateDate
    {
      get { return _UpdateDate; }
      set { _UpdateDate = value; }
    }

    #endregion

    /// <summary>
    /// This method returns the schedules selection option.
    /// </summary>
    /// <returns>EvOption object</returns>
    public EvOption getOption ( )
    {
      return new EvOption ( this._ScheduleId.ToString ( ), this._Title );
    }

    //  ================================================================================
    /// <summary>
    /// Sets the value of this activity class field name. Validate the format of the
    /// value. 
    /// </summary>
    /// <param name="fieldName">ScheduleClassFieldNames: Name of the field to be setted.</param>
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
    public EvEventCodes setValue ( ScheduleClassFieldNames fieldName, String value )
    {
      int iValue = 0;
      MilestonePeriodIncrements increment = this._MilestonePeriodIncrement;
      ScheduleTypes type = this._Type;
      //
      // Switch the FieldName based on the activity field names
      //
      switch ( fieldName )
      {
        case ScheduleClassFieldNames.TrialId:
          {
            this.TrialId = value;
            break;
          }
        case ScheduleClassFieldNames.Version:
          {
            this._Version = EvcStatics.getInteger ( value );
            break;
          }
        case ScheduleClassFieldNames.ArmIndex:
        case ScheduleClassFieldNames.ScheduleId:
          {
            if ( Int32.TryParse ( value, out iValue ) == true )
            {
              this._ScheduleId = iValue;
            }
            break;
          }
        case ScheduleClassFieldNames.Title:
          {
            this._Title = value;
            break;
          }
        case ScheduleClassFieldNames.Description:
          {
            this._Description = value;
            break;
          }
        case ScheduleClassFieldNames.Milestone_Period_Increment:
          {
            if ( EvStatics.Enumerations.tryParseEnumValue<MilestonePeriodIncrements> ( value, out increment ) == true )
            {
              this._MilestonePeriodIncrement = increment;
            }
            break;
          }
        case ScheduleClassFieldNames.Schedule_Type:
          {
            if ( EvStatics.Enumerations.tryParseEnumValue<ScheduleTypes> ( value, out type ) == true )
            {
              this._Type = type;
            }
            break;
          }
      }// End switch field name

      return EvEventCodes.Ok;

    }//End setValue method.

    //  =================================================================================
    /// <summary>
    /// This class creates a selection list of milestone period increments.
    /// </summary>
    /// <returns>List of EvOption: a list of milestone period increments</returns>
    //  ---------------------------------------------------------------------------------
    public static List<EvOption> getScheduleIdList ( )
    {
      //
      // Initialize a return option list.
      //
      List<EvOption> optionList = new List<EvOption> ( );

      optionList.Add ( new EvOption ( "0", EvLabels.Label_Select_All ) );
      for ( int i = EvSchedule.CONST_MINIMUM_SCHEDULE_ID; i <= EvSchedule.CONST_MAXIMUM_SCHEDULE_ID; i++ )
      {
        optionList.Add ( new EvOption ( i.ToString ( "##" ) ) );
      }

      return optionList;

    }//END getOptionList method

    //  =================================================================================
    /// <summary>
    /// This class creates a selection list of milestone types.
    /// </summary>
    /// <param name="Project">EvProject:  the project configuration object.</param>
    /// <returns>List of EvOption: a list of milestone period increments</returns>
    //  ---------------------------------------------------------------------------------
    public static List<EvOption> getScheduleTypeList (
      EdApplication Project  )
    {
      //
      // Initialize a return option list.
      //
      List<EvOption> optionList = new List<EvOption> ( );

      optionList.Add ( new EvOption ( EvSchedule.ScheduleStates.Null.ToString ( ), String.Empty ) );
      optionList.Add ( EvcStatics.Enumerations.getOption ( ScheduleTypes.Clinical ) );


      return optionList;

    }//END getOptionList method

    //  =================================================================================
    /// <summary>
    /// This class creates a selection list of milestone period increments.
    /// </summary>
    /// <returns>List of EvOption: a list of milestone period increments</returns>
    //  ---------------------------------------------------------------------------------
    public static List<EvOption> getStateList ( )
    {
      //
      // Initialize a return option list.
      //
      List<EvOption> optionList = new List<EvOption> ( );

      optionList.Add ( new EvOption ( EvSchedule.ScheduleStates.Null.ToString ( ), String.Empty ) );
      optionList.Add ( EvcStatics.Enumerations.getOption ( ScheduleStates.Draft ) );
      optionList.Add ( EvcStatics.Enumerations.getOption ( ScheduleStates.Reviewed ) );
      optionList.Add ( EvcStatics.Enumerations.getOption ( ScheduleStates.Issued ) );
      optionList.Add ( EvcStatics.Enumerations.getOption ( ScheduleStates.Withdrawn ) );

      return optionList;

    }//END getOptionList method

    //  =================================================================================
    /// <summary>
    /// This class creates a selection list of milestone period increments.
    /// </summary>
    /// <returns>List of EvOption: a list of milestone period increments</returns>
    //  ---------------------------------------------------------------------------------
    public static List<EvOption> getPeriodIncrementList ( )
    {
      //
      // Initialize a return option list.
      //
      List<EvOption> optionList = new List<EvOption> ( );

      optionList.Add ( EvcStatics.Enumerations.getOption ( MilestonePeriodIncrements.Days ) );
      optionList.Add ( EvcStatics.Enumerations.getOption ( MilestonePeriodIncrements.Weeks ) );
      optionList.Add ( EvcStatics.Enumerations.getOption ( MilestonePeriodIncrements.Months ) );

      return optionList;

    }//END getOptionList method

  }//END EvSchedule class

}//END namespace Evado.Model 
