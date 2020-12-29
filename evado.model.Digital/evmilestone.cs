/***************************************************************************************
 * <copyright file="model\EvMilestone.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvMilestone data object.
 *
 ****************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

namespace Evado.Model.Digital
{
  /// <summary>
  /// This class defines the data model for  trial or registry schedule milestone. 
  /// </summary>
  [Serializable]
  public class EvMilestone
  {
    #region Class Enumerators

    /// <summary>
    /// This enumeration list defines the type of milestone.
    /// </summary>
    public enum MilestoneTypes
    {
      /// <summary>
      /// This enumeration defines null value or not selection state.
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines a clinical type milestones
      /// </summary>
      Clinical,

      /// <summary>
      /// This enumeration defines a monitored type milestones
      /// </summary>
      Monitored,

      /// <summary>
      /// This enumeration defines a clinical questionnaire type milestone
      /// </summary>
      Questionnaire,

      /// <summary>
      /// This enumeration defines an unscheduled clinical type milestones
      /// </summary>
      UnScheduled,

      /// <summary>
      /// This enumeration defines a clinical questionnaire type milestone
      /// </summary>
      Implant_Visit,

      /// <summary>
      /// This enumeration defines an milestone is repeated
      /// </summary>
      Repeating_Milestone,

      /// <summary>
      /// This enumeration defines a manual type milestones
      /// </summary>
      Manual,

      /// <summary>
      /// This enumeration defines a three monthly type milestones
      /// </summary>
      Three_Monthly,

      /// <summary>
      /// This enumeration defines a six monthly type milestones
      /// </summary>
      Six_Monthly,

      /// <summary>
      /// This enumeration defines an annual type milestones
      /// </summary>
      Annual,

      /// <summary>
      /// This enumeration defines a non clinical type milestones
      /// </summary>
      Non_Clinical,

      /// <summary>
      /// This enumeration defines an milestone patient records
      /// </summary>
      Patient_Record,
    }

    /// <summary>
    /// This enumeration list defines the field name of milestone class
    /// </summary>
    public enum MilestoneClassFieldNames
    {
      /// <summary>
      /// This enumeration defines null value or not selection state.
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines a trial identifier field name of milestone class.
      /// </summary>
      ProjectId,

      /// <summary>
      /// This enumeration defines a milestone identifier field name of milestone class.
      /// </summary>
      MilestoneId,

      /// <summary>
      /// This enumeration defines an organization identifier field name of milestone class.
      /// </summary>
      OrgId,

      /// <summary>
      /// This enumeration defines a subject identifier field name of milestone class.
      /// </summary>
      SubjectId,

      /// <summary>
      /// This enumeration defines a visit identifier field name of milestone class.
      /// </summary>
      VisitId,

      /// <summary>
      /// This enumeration defines an arm index field name of milestone class.
      /// </summary>
      ScheduleId,

      /// <summary>
      /// This enumeration defines a description field name of milestone class.
      /// </summary>Title,
      Description,

      /// <summary>
      /// This enumeration defines an order field name of milestone class.
      /// </summary>
      Order,

      /// <summary>
      /// This enumeration defines a type field name of milestone class
      /// </summary>
      Type,

      /// <summary>
      /// This enumeration defines a title field name of milestone class
      /// </summary>
      Title,

      /// <summary>
      /// This enumeration defines an is clinical field name of milestone class.
      /// </summary>
      IsClinical,

      /// <summary>
      /// This enumeration defines an is mornitored field name of milestone class.
      /// </summary>
      IsMonitored,

      /// <summary>
      /// This enumeration defines a start date field name of milestone class
      /// </summary>
      StartDate,

      /// <summary>
      /// This enumeration defines a finish date field name of milestone class
      /// </summary>
      FinishDate,

      /// <summary>
      /// This enumeration defines a total cost of milestone class
      /// </summary>
      TotalCost,

      /// <summary>
      /// This enumeration defines a total price field name of milestone class
      /// </summary>
      TotalPrice,

      /// <summary>
      /// This enumeration defines a status field name of milestone class
      /// </summary>
      Status,

      /// <summary>
      /// This enumeration defines comment field name of milestone class
      /// </summary>
      Comment,

      /// <summary>
      /// This enumeration defines an annotation field name of milestone class
      /// </summary>
      Annotation,

      /// <summary>
      /// This enumeration defines an consent validation field name of milestone class
      /// </summary>
      Consent_Validation,

      /// <summary>
      /// This enumeration defines the inter-visit period field name of milestone class
      /// </summary>
      Inter_Visit_Period,

      /// <summary>
      /// This enumeration defines the visit period field name of milestone class
      /// </summary>
      Milestone_Range,

      /// <summary>
      /// This enumeration defines the RepeatNoTimes milestone field name of the milestone class
      /// </summary>
      Repeat_No_Times,

      /// <summary>
      /// This enumeration defines the Automatic_Shcedulgin milestone field name of the milestone class
      /// </summary>
      Enable_Automatic_Scheduling,

      /// <summary>
      /// This enumeration defines the mandatory activity field name of milestone class
      /// </summary>
      Mandatory_Activity,

      /// <summary>
      /// This enumeration defines the optional activity field name of milestone class
      /// </summary>
      Optional_Activity,

      /// <summary>
      /// This enumeration defines the optional activity field name of milestone class
      /// </summary>
      Non_Clinical_Activity,
    }

    /// <summary>
    /// This enumeration list defines the state of milestones.
    /// </summary>
    public enum MilestoneStates
    {
      /// <summary>
      /// This enumeration defines null value or not selection state.
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines a scheduled state of milestone.
      /// </summary>
      Scheduled,

      /// <summary>
      /// This enumeration defines an attended state of milestone.
      /// </summary>
      Attended,

      /// <summary>
      /// This enumeration defines a completed state of milestone.
      /// </summary>
      Completed,

      /// <summary>
      /// This enumeration defines a monitored state of milestone.
      /// </summary>
      Monitored,

      /// <summary>
      /// This enumeration defines an issues resolved state of milestone.
      /// </summary>
      Issues_Resolved,

      /// <summary>
      /// This enumeration defines an visit that has been scheduled but not required.
      /// </summary>
      Not_Required,

      /// <summary>
      /// This enumeration defines a cancelled state of milestone.
      /// </summary>
      Cancelled,

      /// <summary>
      /// This enumeration defines a clash state of milestone.
      /// </summary>
      Clash,

      /// <summary>
      /// This enumeration defines a skip state of milestone.
      /// </summary>
      Skip,
    }

    /// <summary>
    /// The enumerator list defines the schedule period of milestones
    /// </summary>
    public enum MilestoneSchedulePeriod
    {
      /// <summary>
      /// This enumeration defines null value or not selection state.
      /// </summary>
      Null = 0,

      /// <summary>
      /// This enumeration defines the three month period of milestone.
      /// </summary>
      Three_Months = 3,

      /// <summary>
      /// This enumeration defines the six month period of milestone.
      /// </summary>
      Six_Months = 6,

      /// <summary>
      /// This enumeration defines the twelve months period of milestone.
      /// </summary>
      One_Year = 12,

      /// <summary>
      /// This enumeration defines the twenty four months period of milestone.
      /// </summary>
      Two_Years = 24,

      /// <summary>
      /// This enumeration defines the twenty four months period of milestone.
      /// </summary>
      Three_Years = 36,

      /// <summary>
      /// This enumeration defines the twenty four months period of milestone.
      /// </summary>
      Four_Years = 48,

      /// <summary>
      /// This enumeration define the trial period of milestone.
      /// </summary>
      Project = 9999
    }

    /// <summary>
    /// This enumeration list defines the save action of milestons.
    /// </summary>
    public enum MilestoneSaveActions
    {
      /// <summary>
      /// This enumeration defines the null or not selected state.
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines save action of milestone.
      /// </summary>
      Save,

      /// <summary>
      /// This enumeration defines the milestone delete save action command.
      /// </summary>
      Delete,

      /// <summary>
      /// This enumeration defines the milestone reorder save action command.
      /// </summary>
      Reorder,

      /// <summary>
      /// This enumeration defines the visit scheduled save action command.
      /// </summary>
      Scheduled,

      /// <summary>
      /// This enumeration definees the visit attended save action command.
      /// </summary>
      Attend,

      /// <summary>
      /// This enumeration definees the visit completed save action command.
      /// </summary>
      Completed,

      /// <summary>
      /// This enumeration definees the visit monitored save action command.
      /// </summary>
      Monitored,

      /// <summary>
      /// This enumeration defienes the visit all issued resolved save action command.
      /// </summary>
      Issues_Resolved,

      /// <summary>
      /// This enumeration defienes the reopen visit save action command.
      /// </summary>
      Reopen_Visit,

      /// <summary>
      /// This enumeration defines the visit cancelled  save action command.
      /// </summary>
      Cancel,

    }

    #endregion

    #region Class constant

    /// <summary>
    /// This constant defines a new subject visit of milestone.
    /// </summary>
    public const String CONST_NEW_SUBJECT_VISIT = "_NEW";

    /// <summary>
    /// This constant defines the minimum number of days between visits.
    /// </summary>
    public const int CONST_MINIMUM_VISIT_PERIOD = 0;

    /// <summary>
    /// This constant defines the maximum number of days between visits.
    /// </summary>
    public const int CONST_MAXIMUM_VISIT_PERIOD = 2000;

    /// <summary>
    /// This constant defines the maximum order number for a milestone.
    /// </summary>
    public const int CONST_MAXIMUM_MILESTONE_ORDER = 200;

    #endregion

    #region Public Properties

    private Guid _Guid = Guid.Empty;
    /// <summary>
    /// The property contains a global unique identifier (Primary Key) of milestone.
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

    private Guid _ScheduleGuid = Guid.Empty;
    /// <summary>
    /// The property contains a schedule global identifier of milestone.
    /// </summary>
    public Guid ScheduleGuid
    {
      get
      {
        return this._ScheduleGuid;
      }
      set
      {
        this._ScheduleGuid = value;
      }
    }

    private Guid _MilestoneGuid = Guid.Empty;
    /// <summary>
    /// The property contains a milestone global unique identifier (Primary Key)
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

    private Guid _SubjectMilestoneGuid = Guid.Empty;
    /// <summary>
    /// The property contains a subject milestone global unique identifier (Primary Key)
    /// </summary>
    public Guid SubjectMilestoneGuid
    {
      get
      {
        return this._SubjectMilestoneGuid;
      }
      set
      {
        this._SubjectMilestoneGuid = value;
      }
    }

    private string _ProjectId = String.Empty;
    /// <summary>
    /// This property contains a trial identifier of milestone.
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
    /// This property defines an organization identifier of milestone.
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

    private int _ScheduleId = 1;
    /// <summary>
    /// This property contains a schedule identifier
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

    private string _MilestoneId = String.Empty;
    /// <summary>
    /// This property contains a milestone identifier
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

    private string _SubjectId = String.Empty;
    /// <summary>
    /// This property contains a subject identifier of milestone.
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
    /// This property contains a visit identifier of milestone.
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

    private string _VisitInstance = String.Empty;
    /// <summary>
    /// This property contains a subject visit instance of milestone.
    /// </summary>
    public string VisitInstance
    {
      get
      {
        return this._VisitInstance;
      }
      set
      {
        this._VisitInstance = value;
      }
    }

    private string _ScheduleTitle = String.Empty;
    /// <summary>
    /// This property contains a name of the arm of milestone.
    /// </summary>
    public string ScheduleTitle
    {
      get
      {
        return this._ScheduleTitle;
      }
      set
      {
        this._ScheduleTitle = value;
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

    private string _Title = String.Empty;
    /// <summary>
    /// This property contains a title of the milestone
    /// 
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
    /// This property contains a description of the milestone
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

    private int _Order = 0;
    /// <summary>
    /// This property contains an order in which the milestone is to be displayed when listed.
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

    private EvMilestone.MilestoneTypes _Type = EvMilestone.MilestoneTypes.Null;
    /// <summary>
    /// This property contains a type of milestone.
    /// </summary>
    public EvMilestone.MilestoneTypes Type
    {
      get
      {
        return this._Type;
      }
      set
      {
        this._Type = value;

        if ( this._Type == MilestoneTypes.Repeating_Milestone
          && this._RepeatNoTimes == EvMilestone.CONST_MIN_REPEAT_TIME )
        {
          this._RepeatNoTimes = EvMilestone.CONST_MAX_REPEAT_TIME;
        }
      }
    }

    /// <summary>
    /// This property contains a string type of milestone.
    /// </summary>
    public string stType
    {
      get
      {
        return EvcStatics.getEnumValueAsString ( this._Type );
      }
      set
      {

        EvMilestone.MilestoneTypes type = MilestoneTypes.Null;

        try
        {
          type = ( EvMilestone.MilestoneTypes ) Enum.Parse ( typeof ( EvMilestone.MilestoneTypes ), value );
        }
        catch
        {
          type = EvMilestone.MilestoneTypes.Null;
        }
        this._Type = type;
      }
    }

    //
    // NEW PROPERTY
    //
    private bool _EnablePatientEntry = false;

    /// <summary>
    /// This property whether the milestone is can be completed by a patient.
    /// </summary>
    public bool EnablePatientEntry
    {
      get
      {
        return _EnablePatientEntry;
      }
      set
      {
        _EnablePatientEntry = value;
      }
    }
    // 
    // NEW PROPERTY
    //
    /// <summary>
    /// This constant defines the maximum number of repeats a repeating milestone can have.
    /// This value indicates that the milestone is repeating an unlimted number of times.
    /// </summary>
    public const int CONST_MIN_REPEAT_TIME = 0;
    /// <summary>
    /// This constant defines the maximum number of repeats a repeating milestone can have.
    /// This value indicates that the milestone is repeating an unlimted number of times.
    /// </summary>
    public const int CONST_MAX_REPEAT_TIME = 99;

    private int _RepeatNoTimes = 0;
    /// <summary>
    /// This property defines the number of times the milestone is to be repeated.
    /// 0 = do not repeat the milestone.
    /// =>99 = indefinitely repeat the milestone until the next next non-repeating milestone InterMilestonePeriod value is reached.
    /// </summary>
    public int RepeatNoTimes
    {
      get
      {
        return this._RepeatNoTimes;
      }
      set
      {
        this._RepeatNoTimes = value;

        if ( this._RepeatNoTimes > CONST_MAX_REPEAT_TIME )
        {
          this._RepeatNoTimes = CONST_MAX_REPEAT_TIME;
        }

        if ( this._RepeatNoTimes < CONST_MIN_REPEAT_TIME )
        {
          this._RepeatNoTimes = CONST_MIN_REPEAT_TIME;
        }
      }
    }

    //
    // NEW PROPERTY
    //
    private bool _EnableAutomaticScheduling = false;

    /// <summary>
    /// This property whether the milestone is automatically scheduled after the previous visit has been completed.
    /// </summary>
    public bool EnableAutomaticScheduling
    {
      get
      {
        return _EnableAutomaticScheduling;
      }
      set
      {
        _EnableAutomaticScheduling = value;
      }
    }

    /// <summary>
    /// This property indicates whether a milestone is clinical
    /// If true the milestone is displayed to site users and can create potocol
    /// violations.
    /// </summary>
    public bool IsClinical
    {
      get
      {
        if ( this._Type == MilestoneTypes.Clinical
          || this._Type == MilestoneTypes.Questionnaire
          || this._Type == MilestoneTypes.Patient_Record
          || this._Type == MilestoneTypes.Monitored
          || this._Type == MilestoneTypes.UnScheduled
          || this._Type == MilestoneTypes.Implant_Visit )
        {
          return true;
        }
        return false;
      }
      set
      {
        bool Null = value;

      }
    }
    /// <summary>
    /// This property indicates whether this milestone has been monitored 
    /// Only used for clinical milestones.
    /// </summary>
    public bool IsMonitored
    {
      get
      {
        if ( this._Type == MilestoneTypes.Monitored )
        {
          return true;
        }
        return false;
      }
      set
      {
        bool Null = value;
      }
    }

    private String _SiteRoleId = String.Empty;
    /// <summary>
    /// This property contains the Evado site user role that is able to view and update records.
    /// Only site users or coordinating site users with this role will be able to see this milestone.
    /// </summary>
    public string SiteRoleId
    {
      get
      {
        return this._SiteRoleId;
      }
      set
      {
        this._SiteRoleId = value;
      }
    }

    private bool _MilestoneLaterThanConsentDate = false;
    /// <summary>
    /// This property indicates whether the visit is later than the consent date
    /// </summary>
    public bool MilestoneLaterThanConsentDate
    {
      get
      {
        return this._MilestoneLaterThanConsentDate;
      }
      set
      {
        this._MilestoneLaterThanConsentDate = value;
      }
    }

    private float _InterMilestonePeriod = EvMilestone.CONST_MINIMUM_VISIT_PERIOD;
    /// <summary>
    /// This property contains maximum days from previous visit.
    /// </summary>
    public float InterMilestonePeriod
    {
      get
      {
        return this._InterMilestonePeriod;
      }
      set
      {
        this._InterMilestonePeriod = value;

        if ( this._InterMilestonePeriod > EvMilestone.CONST_MAXIMUM_VISIT_PERIOD )
        {
          this._InterMilestonePeriod = EvMilestone.CONST_MAXIMUM_VISIT_PERIOD;
        }
      }
    }

    /// <summary>
    /// This property contains the SQL integer value of the inter milestone period (float)
    /// For backward compatibility.
    /// </summary>
    public int InterMilestonePeriod_In_Days
    {
      get
      {
        switch ( this.MilestonePeriodIncrement )
        {
          case EvSchedule.MilestonePeriodIncrements.Months:
            {
              return ( int ) Math.Ceiling ( this._InterMilestonePeriod * 365 / 12 );
            }
          case EvSchedule.MilestonePeriodIncrements.Weeks:
            {
              return ( int ) Math.Ceiling ( this._InterMilestonePeriod * 7 );
            }
          default:
            {
              return ( int ) Math.Ceiling ( this._InterMilestonePeriod );
            }
        }//END Switch.
      }
      set
      {
        double dblValue = value;
        int intValue = 0;
        double dblDiff = 0;
        switch ( this.MilestonePeriodIncrement )
        {
          case EvSchedule.MilestonePeriodIncrements.Months:
            {
              dblValue = dblValue / 365 * 12;
              intValue = ( int ) dblValue;
              dblDiff = dblValue - intValue;
              if ( dblDiff < 0.033 )
              {
                dblValue = ( int ) dblValue;
              }
              else
              {
                dblValue = Math.Round ( dblValue, 2 );
              }
              this._InterMilestonePeriod = ( float ) dblValue;
              return;
            }
          case EvSchedule.MilestonePeriodIncrements.Weeks:
            {
              dblValue = dblValue / 7;
              intValue = ( int ) dblValue;
              dblDiff = dblValue - intValue;
              if ( dblDiff < 0.014 )
              {
                dblValue = ( int ) dblValue;
              }
              else
              {
                dblValue = Math.Round ( dblValue, 1 );
              }
              this._InterMilestonePeriod = ( float ) dblValue;
              return;
            }
          default:
            {
              this._InterMilestonePeriod = ( float ) dblValue;
              return;
            }
        }//END Switch.
      }
    }


    private float _MilestoneRange = 0;

    /// <summary>
    /// This property contains the range within which the milestone must be attended
    /// the unit of the range is the same as the unit of the InterVisitPeriod in days, weeks or months.
    /// </summary>
    public float MilestoneRange
    {
      get { return _MilestoneRange; }
      set { _MilestoneRange = value; }
    }
    /// <summary>
    /// This property contains the SQL integer value of the inter milestone period (float)
    /// For backward compatibility.
    /// </summary>
    public int MilestoneRange_In_Days
    {
      get
      {
        switch ( this.MilestonePeriodIncrement )
        {
          case EvSchedule.MilestonePeriodIncrements.Months:
            {
              return ( int ) Math.Ceiling ( this._MilestoneRange * 365 / 12 );
            }
          case EvSchedule.MilestonePeriodIncrements.Weeks:
            {
              return ( int ) Math.Ceiling ( this._MilestoneRange * 7 );
            }
          default:
            {
              return ( int ) Math.Ceiling ( this._MilestoneRange );
            }
        }//END Switch.
      }
      set
      {
        double dblValue = value;
        int intValue = 0;
        double dblDiff = 0;
        switch ( this.MilestonePeriodIncrement )
        {
          case EvSchedule.MilestonePeriodIncrements.Months:
            {
              dblValue = dblValue / 365 * 12;
              intValue = ( int ) dblValue;
              dblDiff = dblValue - intValue;
              if ( dblDiff < 0.033 )
              {
                dblValue = ( int ) dblValue;
              }
              else
              {
                dblValue = Math.Round ( dblValue, 2 );
              }
              this._MilestoneRange = ( float ) dblValue;
              return;
            }
          case EvSchedule.MilestonePeriodIncrements.Weeks:
            {
              dblValue = dblValue / 7;
              dblDiff = dblValue - intValue;
              if ( dblDiff < 0.014 )
              {
                dblValue = ( int ) dblValue;
              }
              else
              {
                dblValue = Math.Round ( dblValue, 1 );
              }
              this._MilestoneRange = ( float ) dblValue;
              return;
            }
          default:
            {
              this._MilestoneRange = ( float ) dblValue;
              return;
            }
        }//END Switch.
      }
    }
    /// <summary>
    /// This property contains details html of milestone.
    /// </summary>
    public string Details
    {
      get
      {
        string stHtml = String.Empty;

        stHtml += this._MilestoneId;
        if ( stHtml != String.Empty )
        {
          stHtml += " - ";
        }
        stHtml += this._Title;

        if ( this.Description != this._Title )
        {
          if ( stHtml != String.Empty )
          {
            stHtml += "\r\n";
          }
          stHtml += this.Description;
        }
        return stHtml;
      }
    }

    /// <summary>
    /// This property contains link details of milestone.
    /// </summary>
    public String LinkText
    {
      get
      {
        String mandatoryActivityId = EvLabels.Label_Not_Set;
        String optionalActivityId = EvLabels.Label_Not_Set;

        var mandatoryActivity = this.getClinicalActivity ( true );
        var optionalActivity = this.getClinicalActivity ( false );
        if ( mandatoryActivity != null )
        {
          mandatoryActivityId = mandatoryActivity.ActivityId;
        }

        if ( optionalActivity != null )
        {
          optionalActivityId = mandatoryActivity.ActivityId;
        }



        if ( this.InterMilestonePeriod > 0 )
        {
          return String.Format ( EvLabels.Milestone_List_Period_Label,
            this.MilestoneId,
            this.Title,
            this.stType,
            this.Order,
            this.InterMilestonePeriod,
            this.MilestonePeriodIncrement,
            this.MilestoneRange,
            mandatoryActivityId,
            optionalActivityId );
        }
        return String.Format ( EvLabels.Milestone_List_Link_Label,
            this.MilestoneId,
            this.Title,
            this.stType,
            this.Order,
            mandatoryActivityId,
            optionalActivityId );
      }
    }

    private EvMilestoneData _Data = new EvMilestoneData ( );
    /// <summary>
    /// This property contains the additoin object date for this milestone.
    /// The object is stored in a serialised form and cannot be directly queried in the DB.
    /// </summary>
    public EvMilestoneData Data
    {
      get
      {
        return this._Data;
      }
      set
      {
        this._Data = value;
      }
    }

    private List<EvActivity> _ActivityList = new List<EvActivity> ( );
    /// <summary>
    /// This property contains a array of activities that are executed during this milestone.
    /// </summary>
    public List<EvActivity> ActivityList
    {
      get
      {
        return this._ActivityList;
      }
      set
      {
        this._ActivityList = value;
      }
    }

    /// <summary>
    /// This property returns the activity list formatted for HTML.
    /// </summary>
    public String ActivtieListAsHtml
    {
      get
      {
        // 
        // Initialize an activitylist string.
        // 
        string activityList = String.Empty;

        // 
        // Iterate through the array to list the mandatory activities.
        // 
        foreach ( EvActivity activity in this._ActivityList )
        {
          if ( ( activity.Type == EvActivity.ActivityTypes.Clinical )
            && activity.Title != String.Empty )
          {
            //
            // Add an item including activityId and title to the return list.
            //
            activityList += activity.ActivityId + " - " + activity.Title;
            if ( activity.IsMandatory == true )
            {
              activityList += EvLabels.Label_Mandatory;
            }
            activityList += "<br/>";
          }
        }

        if ( activityList == String.Empty )
        {
          activityList = "No activities listed";
        }

        // 
        // Return the list of activities
        // 
        return activityList;
      }
    }

    private List<EdRecord> _FormList = new List<EdRecord> ( );
    /// <summary>
    /// This property defines a form list object of milestone.
    /// </summary>
    public List<EdRecord> FormList
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


    private DateTime _ScheduleDate = EvcStatics.CONST_DATE_NULL;
    /// <summary>
    /// This property defines a scheduled date for this milestone.
    /// </summary>
    public DateTime ScheduleDate
    {
      get
      {
        return this._ScheduleDate;
      }
      set
      {
        this._ScheduleDate = value;
      }
    }

    /// <summary>
    /// This property defines a schedule date string of milestone.
    /// </summary>
    public string stScheduleDate
    {
      get
      {
        if ( this._ScheduleDate > EvcStatics.CONST_DATE_NULL )
        {
          return this._ScheduleDate.ToString ( "dd MMM yyyy" );
        }
        return String.Empty;
      }
      set
      {
        if ( value == String.Empty )
        {
          this._ScheduleDate = EvcStatics.CONST_DATE_NULL;
          return;
        }
        DateTime date = this._ScheduleDate;

        if ( DateTime.TryParse ( value, out date ) == true )
        {
          this._ScheduleDate = date;
        }
      }
    }

    /// <summary>
    /// This property defines a schedule time string of milestone.
    /// </summary>
    public string stScheduleTime
    {
      get
      {
        if ( this._ScheduleDate > EvcStatics.CONST_DATE_NULL )
        {
          return this._ScheduleDate.ToString ( "hh:mm tt" );
        }
        return String.Empty;
      }
    }

    private DateTime _StartDate = EvcStatics.CONST_DATE_NULL;
    /// <summary>
    /// This property contains the start date of milestone.
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

        if ( this._StartDate > EvcStatics.CONST_DATE_NULL
          && this._State != MilestoneStates.Scheduled )
        {
          this._ScheduleDate = this._StartDate;
        }
      }
    }

    /// <summary>
    /// This property contains a start date time string of milestone.
    /// </summary>
    public string stStartDateTime
    {
      get
      {
        if ( this._StartDate > EvcStatics.CONST_DATE_NULL )
        {
          return this._StartDate.ToString ( "dd MMM yyyy hh:mm tt" );
        }
        return String.Empty;
      }
    }

    /// <summary>
    /// This property contains a start date string of milestone.
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

    /// <summary>
    /// This property contains a start time string of milestone.
    /// </summary>
    public string stStartTime
    {
      get
      {
        if ( this._StartDate > EvcStatics.CONST_DATE_NULL )
        {
          return this._StartDate.ToString ( "hh:mm tt" );
        }
        return String.Empty;
      }
    }

    private DateTime _FinishDate = EvcStatics.CONST_DATE_NULL;
    /// <summary>
    /// This property contains the finish or completion date of milestone.
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
    /// This property contains a finish date time string of milestone.
    /// </summary>
    public string stFinishDateTime
    {
      get
      {
        if ( this._FinishDate > EvcStatics.CONST_DATE_NULL )
        {
          return this._FinishDate.ToString ( "dd MMM yyyy hh:mm tt" );
        }
        return String.Empty;
      }
    }

    /// <summary>
    /// This property contains a finish date string of milestone.
    /// </summary>
    public string stFinishDate
    {
      get
      {
        if ( this._FinishDate > EvcStatics.CONST_DATE_NULL )
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
        DateTime date = this.FinishDate;

        if ( DateTime.TryParse ( value, out date ) == true )
        {
          this._FinishDate = date;
        }
      }
    }

    /// <summary>
    /// This property contains a visit end time string of milestone.
    /// </summary>
    public string stVisitEndTime
    {
      get
      {
        if ( this._FinishDate > EvcStatics.CONST_DATE_NULL )
        {
          return this._FinishDate.ToString ( "hh:mm tt" );
        }
        return String.Empty;
      }
    }

    /// <summary>
    /// This property contains a date string of milestone.
    /// </summary>
    public string stDate
    {
      get
      {
        if ( this._StartDate > EvcStatics.CONST_DATE_NULL )
        {
          return this._StartDate.ToString ( "dd MMM yyyy" );
        }
        if ( this._ScheduleDate > EvcStatics.CONST_DATE_NULL )
        {
          return this._ScheduleDate.ToString ( "dd MMM yyyy" );
        }
        return String.Empty;
      }
    }

    private int _ResourceTime = 0;
    /// <summary>
    /// This property contains the resource time to execute the visit activities.
    /// </summary>    
    public int ResourceTime
    {
      get
      {
        return this._ResourceTime;
      }
      set
      {
        this._ResourceTime = value;
      }
    }

    List<EvFormRecordComment> _CommentList = new List<EvFormRecordComment> ( );
    /// <summary>
    /// This property contains a list of user comments object of milestone.
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

    private string _Comments = String.Empty;
    /// <summary>
    /// This property contains the visit comments string of milestone.
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

    private string _Summary = String.Empty;
    /// <summary>
    /// This property contains the visit summary string of milestone.
    /// </summary>
    public string Summary
    {
      get
      {
        return this._Summary;
      }
      set
      {
        this._Summary = value;
      }
    }

    private EvMilestone.MilestoneStates _State = MilestoneStates.Null;
    /// <summary>
    /// This property containss the visit status object of milestone.
    /// </summary>
    public EvMilestone.MilestoneStates State
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

    private bool _ProtocolViolation = false;
    /// <summary>
    /// This property indicates whether this milestone is a has a protocol violation.  
    /// 
    /// If true the milestone is displayed to site users and can create potocol
    /// violations.
    /// 
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

    /// <summary>
    /// This property contains state description of milestone.
    /// </summary>
    public string StateDesc
    {
      get
      {
        if ( this._State != MilestoneStates.Null )
        {
          string state = this._State.ToString ( );
          return state.Replace ( "_", " " );
        }
        return String.Empty;
      }
      set
      {
        string s = value;
      }
    }

    private string _BookedOutBy = String.Empty;
    /// <summary>
    /// This property contains the name of the person who has booked this object 
    /// out to edit it.
    /// 
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

    private float _TotalCost = 0;
    /// <summary>
    /// This property contains the total cost for execution of milestone.
    /// 
    /// </summary>
    public float TotalCost
    {
      get
      {
        return this._TotalCost;
      }
      set
      {
        this._TotalCost = value;
      }
    }

    private float _TotalPrice = 0;
    /// <summary>
    /// This property contains the billing price for the execution of milestone.
    /// </summary>
    public float TotalPrice
    {
      get
      {
        return this._TotalPrice;
      }
      set
      {
        this._TotalPrice = value;
      }
    }

    private MilestoneSaveActions _Action = MilestoneSaveActions.Null;
    /// <summary>
    /// This property contains the save action command object of milestone.
    /// </summary>
    public MilestoneSaveActions Action
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

    private string _Updated = String.Empty;
    /// <summary>
    /// This property contains a user who updates a milestone.
    /// </summary>
    public string UpdatedBy
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

    private string _UpdatedByUserId = String.Empty;
    /// <summary>
    /// This property contains a user identifier of those who update milestone.
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
    /// This propery contains a user common name of those who update milestone.
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

    private bool _Selected = false;
    /// <summary>
    /// This property indicates whether this milestone is a selected.
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

    private bool _IsAuthenticatedSignature = false;
    /// <summary>
    /// This property indicates whether the user's signature has been autenticated.
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

    private List<EdRecord> _RecordList = null;
    /// <summary>
    /// This property contains a list of the record object of milestone.
    /// </summary>
    public List<EdRecord> RecordList
    {
      get { return _RecordList; }
      set { _RecordList = value; }
    }

    private EvEventCodes _EventCode = EvEventCodes.Ok;
    /// <summary>
    /// This property contains an event code object of milestone.
    /// </summary>
    public EvEventCodes EventCode
    {
      get { return _EventCode; }
      set { _EventCode = value; }
    }


    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Public Methods

    // ==================================================================================
    /// <summary>
    /// This method returns the earliest start date
    /// </summary>
    /// <returns>DateTime: object</returns>
    // ----------------------------------------------------------------------------------
    public DateTime getEarliestValidStartDate ( )
    {
      //
      // if visit period is not set then set to the maximum.
      //
      if ( this._Data.PreviousVisitDate == EvcStatics.CONST_DATE_NULL )
      {
        return EvcStatics.CONST_DATE_NULL;
      }

      //
      // if visit period is not set then set to the maximum.
      //
      if ( this._MilestoneRange == 0
        || this._InterMilestonePeriod == EvMilestone.CONST_MAXIMUM_VISIT_PERIOD )
      {
        return this._Data.PreviousVisitDate;
      }

      //
      // Switch to set the milestone scheduled incremenent and period.
      //
      switch ( this._MilestonePeriodIncrement )
      {
        case EvSchedule.MilestonePeriodIncrements.Months:
          {
            float dayMilestoneRange = this._MilestoneRange * 364 / 12;
            int inMilestoneRange = ( int ) Math.Ceiling ( dayMilestoneRange );

            int dayInterMilestonePeriod = ( int ) this._InterMilestonePeriod;

            int inDays = dayInterMilestonePeriod - inMilestoneRange;

            return this._Data.PreviousVisitDate.AddDays ( inDays );
          }
        case EvSchedule.MilestonePeriodIncrements.Weeks:
          {
            float dayMilestoneRange = this._MilestoneRange * 7;
            int inMilestoneRange = ( int ) Math.Ceiling ( dayMilestoneRange );

            int dayInterMilestonePeriod = ( int ) this._InterMilestonePeriod;

            int inDays = dayInterMilestonePeriod - inMilestoneRange;

            return this._Data.PreviousVisitDate.AddDays ( inDays );
          }
        case EvSchedule.MilestonePeriodIncrements.Days:
        default:
          {
            float dayMilestoneRange = this._MilestoneRange;
            int inMilestoneRange = ( int ) Math.Ceiling ( dayMilestoneRange );

            int dayInterMilestonePeriod = ( int ) this._InterMilestonePeriod;

            int inDays = dayInterMilestonePeriod - inMilestoneRange;

            return this._Data.PreviousVisitDate.AddDays ( inDays );
          }
      }//END milestone period increment switch.

    }//END getEarliestValidStartDate method.


    // ==================================================================================
    /// <summary>
    /// This method returns the earliest start date
    /// </summary>
    /// <returns>DateTime: object</returns>
    // ----------------------------------------------------------------------------------
    public DateTime getLatestValidStartDate ( )
    {
      //
      // if visit period is not set then set to the maximum.
      //
      if ( this._Data.PreviousVisitDate == EvcStatics.CONST_DATE_NULL )
      {
        return EvcStatics.CONST_DATE_NULL;
      }

      //
      // Switch to set the milestone scheduled incremenent and period.
      //
      switch ( this._MilestonePeriodIncrement )
      {
        case EvSchedule.MilestonePeriodIncrements.Months:
          {
            float dayMilestoneRange = this._MilestoneRange * 364 / 12;
            int inMilestoneRange = ( int ) Math.Ceiling ( dayMilestoneRange );

            int dayInterMilestonePeriod = ( int ) this._InterMilestonePeriod;

            int inDays = dayInterMilestonePeriod + inMilestoneRange;

            return this._Data.PreviousVisitDate.AddDays ( inDays );
          }
        case EvSchedule.MilestonePeriodIncrements.Weeks:
          {
            float dayMilestoneRange = this._MilestoneRange * 7;
            int inMilestoneRange = ( int ) Math.Ceiling ( dayMilestoneRange );

            int dayInterMilestonePeriod = ( int ) this._InterMilestonePeriod;

            int inDays = dayInterMilestonePeriod + -inMilestoneRange;

            return this._Data.PreviousVisitDate.AddDays ( inDays );
          }
        case EvSchedule.MilestonePeriodIncrements.Days:
        default:
          {
            float dayMilestoneRange = ( int ) this._MilestoneRange;
            int inMilestoneRange = ( int ) Math.Ceiling ( dayMilestoneRange );

            int dayInterMilestonePeriod = ( int ) this._InterMilestonePeriod;

            int inDays = dayInterMilestonePeriod + -inMilestoneRange;

            return this._Data.PreviousVisitDate.AddDays ( inDays );
          }
      }//END milestone period increment switch.

    }//END getLatestValidStartDate method.

    /// =================================================================================
    /// <summary>
    /// This class provides a list of trial types.
    /// </summary>
    /// <returns>string: a field name</returns>
    /// <remarks>
    /// This class consists of the following steps:
    /// 
    /// 1. Initialise the fieldname of milestone class
    /// 
    /// 2. Try convert FieldName string into an enumerative fieldname object
    /// 
    /// 3. Switch fieldname and get the value defining by the fieldnames in milestone class
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public string getValue ( string FieldName )
    {
      // 
      // Initialise the fieldname of milestone class
      // 
      EvMilestone.MilestoneClassFieldNames fieldname = MilestoneClassFieldNames.Null;

      //
      // Try convert FieldName string into an enumerative fieldname object
      //
      try
      {
        fieldname = ( EvMilestone.MilestoneClassFieldNames ) Enum.Parse ( typeof ( EvMilestone.MilestoneClassFieldNames ), FieldName );
      }
      catch
      {
        fieldname = EvMilestone.MilestoneClassFieldNames.Null;
      }

      // 
      // Switch to determine which value to return.
      // 
      switch ( fieldname )
      {
        case EvMilestone.MilestoneClassFieldNames.ProjectId:
          return this._ProjectId;

        case EvMilestone.MilestoneClassFieldNames.MilestoneId:
          return this._MilestoneId;

        case EvMilestone.MilestoneClassFieldNames.OrgId:
          return this._OrgId;

        case EvMilestone.MilestoneClassFieldNames.SubjectId:
          return this._SubjectId;

        case EvMilestone.MilestoneClassFieldNames.VisitId:
          return this._VisitId;

        case EvMilestone.MilestoneClassFieldNames.StartDate:
          return this._StartDate.ToString ( "dd MMM yyyy" );

        case EvMilestone.MilestoneClassFieldNames.FinishDate:
          return this._FinishDate.ToString ( "dd MMM yyyy" );

        case EvMilestone.MilestoneClassFieldNames.TotalCost:
          return this._TotalCost.ToString ( "#####0" );

        case EvMilestone.MilestoneClassFieldNames.TotalPrice:
          return this._TotalPrice.ToString ( "#####0" );

        case EvMilestone.MilestoneClassFieldNames.Order:
          return this._Order.ToString ( "##0" );

        case EvMilestone.MilestoneClassFieldNames.ScheduleId:
          return this._ScheduleId.ToString ( );

        case EvMilestone.MilestoneClassFieldNames.Description:
          return this._Description;

        case EvMilestone.MilestoneClassFieldNames.Milestone_Range:
          {
            return this.MilestoneRange.ToString ( );
          }

        case EvMilestone.MilestoneClassFieldNames.Repeat_No_Times:
          {
            return this._RepeatNoTimes.ToString ( );
          }
        case EvMilestone.MilestoneClassFieldNames.Inter_Visit_Period:
          {
            return this._InterMilestonePeriod.ToString ( );
          }
        case EvMilestone.MilestoneClassFieldNames.Consent_Validation:
          {
            return this._MilestoneLaterThanConsentDate.ToString ( );
          }

        case EvMilestone.MilestoneClassFieldNames.Type:
          return this._Type.ToString ( );

        case EvMilestone.MilestoneClassFieldNames.Title:
          return this._Title;

        case EvMilestone.MilestoneClassFieldNames.Status:
          return this._State.ToString ( );

        default:
          {
            return String.Empty;

          }//END Default

      }//END Switch

    }//END getValue method

    /// =================================================================================
    /// <summary>
    /// This method sets the field value.    
    /// </summary>
    /// <param name="FieldName">string: a retrieved field name</param>
    /// <param name="Value">string: a value string for updating</param>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialise the methods variables and objects.
    /// 
    /// 2. Try convert the FieldName string into an enumerative fieldname object.
    /// 
    /// 3. Switch fieldname and update the string value to the fieldname of milestone class
    /// 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public void setValue ( String FieldName, String Value )
    {
      // 
      // Initialise the methods variables and objects.
      //
      DateTime date = EvcStatics.CONST_DATE_NULL;
      //float fltValue = 0;
      //int intValue = 0;
      EvMilestone.MilestoneClassFieldNames fieldname = MilestoneClassFieldNames.Null;

      //
      // Try convert the FieldName string into an enumerative fieldname object.
      //
      try
      {
        fieldname = ( EvMilestone.MilestoneClassFieldNames ) Enum.Parse ( typeof ( EvMilestone.MilestoneClassFieldNames ), FieldName );
      }
      catch
      {
        fieldname = EvMilestone.MilestoneClassFieldNames.Null;
      }

      this.setValue ( fieldname, Value );
    }

    /// =================================================================================
    /// <summary>
    /// This method sets the field value.    
    /// </summary>
    /// <param name="FieldName">string: a retrieved field name</param>
    /// <param name="Value">string: a value string for updating</param>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialise the methods variables and objects.
    /// 
    /// 2. Try convert the FieldName string into an enumerative fieldname object.
    /// 
    /// 3. Switch fieldname and update the string value to the fieldname of milestone class
    /// 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public void setValue ( MilestoneClassFieldNames FieldName, String Value )
    {
      // 
      // Initialise the methods variables and objects.
      //
      DateTime date = EvcStatics.CONST_DATE_NULL;
      float fltValue = 0;
      int intValue = 0;

      // 
      // Switch to determine which value to return.
      // 
      switch ( FieldName )
      {
        case EvMilestone.MilestoneClassFieldNames.ProjectId:
          this._ProjectId = Value;
          return;

        case EvMilestone.MilestoneClassFieldNames.MilestoneId:
          this._MilestoneId = Value;
          return;

        case EvMilestone.MilestoneClassFieldNames.OrgId:
          this._OrgId = Value;
          return;

        case EvMilestone.MilestoneClassFieldNames.SubjectId:
          this._SubjectId = Value;
          return;

        case EvMilestone.MilestoneClassFieldNames.VisitId:
          this._VisitId = Value;
          return;

        case EvMilestone.MilestoneClassFieldNames.Title:
          this._Title = Value;
          return;

        case EvMilestone.MilestoneClassFieldNames.Description:
          this._Description = Value;
          return;

        case EvMilestone.MilestoneClassFieldNames.Order:
          {
            if ( int.TryParse ( Value, out intValue ) == true )
            {
              this._Order = intValue;
            }
            return;
          }

        case EvMilestone.MilestoneClassFieldNames.Type:
          {
            try
            {
              this._Type = EvcStatics.Enumerations.parseEnumValue<EvMilestone.MilestoneTypes> ( Value );
            }
            catch
            {
              this._Type = MilestoneTypes.Null;
            }
            return;
          }


        case EvMilestone.MilestoneClassFieldNames.StartDate:
          {
            if ( DateTime.TryParse ( Value, out date ) == true )
            {
              this._StartDate = date;
            }
            return;
          }

        case EvMilestone.MilestoneClassFieldNames.FinishDate:
          {
            if ( DateTime.TryParse ( Value, out date ) == true )
            {
              this._FinishDate = date;
            }
            return;
          }

        case EvMilestone.MilestoneClassFieldNames.TotalCost:
          {
            if ( float.TryParse ( Value, out fltValue ) == true )
            {
              this._TotalCost = intValue;
            }
            return;
          }

        case EvMilestone.MilestoneClassFieldNames.TotalPrice:
          {
            if ( float.TryParse ( Value, out fltValue ) == true )
            {
              this._TotalPrice = intValue;
            }
            return;
          }

        case EvMilestone.MilestoneClassFieldNames.Status:
          {
            try
            {
              this._State = EvcStatics.Enumerations.parseEnumValue<EvMilestone.MilestoneStates> ( Value );
            }
            catch
            {
              this._State = EvMilestone.MilestoneStates.Null;
            }

            return;
          }
        case EvMilestone.MilestoneClassFieldNames.Milestone_Range:
          {
            this._MilestoneRange = EvcStatics.getFloat ( Value );

            return;
          }
        case EvMilestone.MilestoneClassFieldNames.Inter_Visit_Period:
          {
            this._InterMilestonePeriod = EvcStatics.getFloat ( Value );

            return;
          }
        case EvMilestone.MilestoneClassFieldNames.Repeat_No_Times:
          {
            this._RepeatNoTimes = EvcStatics.getInteger ( Value );

            return;
          }
        case EvMilestone.MilestoneClassFieldNames.Enable_Automatic_Scheduling:
          {
            this._EnableAutomaticScheduling = EvcStatics.getBool ( Value );

            return;
          }
        case EvMilestone.MilestoneClassFieldNames.Consent_Validation:
          {
            this._MilestoneLaterThanConsentDate = EvcStatics.getBool ( Value );

            return;
          }
        default:

          return;

      }//END Switch

    }//END setValue method

    /// =================================================================================
    /// <summary>
    ///   This method creates a selection list from the milestone array.
    /// </summary>
    /// <param name="IsMandatory">bool: return activity if true.</param>
    /// <returns>EvActivity: an activity object</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Iterate through the activities to fine the matching activity.
    /// 
    /// 2. If is mandatory and activity type is clinical return the activity
    /// </remarks>
    // ---------------------------------------------------------------------------------
    public EvActivity getClinicalActivity ( bool IsMandatory )
    {
      //
      // Iterate through the activity list looking for the first mandatory 
      // clinical activity.
      //
      foreach ( EvActivity activity in this._ActivityList )
      {
        if ( activity.IsMandatory == IsMandatory
          && activity.Type == EvActivity.ActivityTypes.Clinical )
        {
          return activity;
        }
      }

      //
      // Return null if not found.
      //
      return null;

    }//END getMandatoryClinicalActivity method.

    /// =================================================================================
    /// <summary>
    ///   This method creates a selection list from the milestone array.
    /// </summary>
    /// <returns>EvActivity: an activity object</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Iterate through the activities to fine the matching activity.
    /// 
    /// 2. If is mandatory and activity type is clinical return the activity
    /// </remarks>
    // ---------------------------------------------------------------------------------
    public List<EvActivity> getNonClinicalActivities ( )
    {
      //
      // Initialise the methods variables and objects.
      //
      List<EvActivity> activityList = new List<EvActivity> ( );

      //
      // Iterate through the activity list looking for the non clinical activities.
      //
      foreach ( EvActivity activity in this._ActivityList )
      {
        if ( activity.Type == EvActivity.ActivityTypes.Clinical
          || activity.Type == EvActivity.ActivityTypes.Clinical )
        {
          continue;
        }

        //
        // Add the non-clinical activities to the list.
        //
        activityList.Add ( activity );
      }

      //
      // Return null if not found.
      //
      return activityList;

    }//END getMandatoryClinicalActivity method.

    /// =================================================================================
    /// <summary>
    ///   This method creates a selection list from the milestone array.
    /// </summary>
    /// <param name="ActivityGuid">Guid: an activity global unique identifier</param>
    /// <returns>EvActivity: an activity object</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Iterate through the activities to fine the matching activity.
    /// 
    /// 2. If the GUIDs match return the activity
    /// </remarks>
    // ---------------------------------------------------------------------------------
    public EvActivity getActivity ( Guid ActivityGuid )
    {
      // 
      // Iterate through the activities to fine the matching activity.
      // 
      foreach ( EvActivity activity in this._ActivityList )
      {
        // 
        // If the GUIDs match return the activity
        // 
        if ( activity.Guid == ActivityGuid )
        {
          return activity;
        }

      }//END activity iteration loop

      // 
      // If nothing found return empty arm
      // 
      return new EvActivity ( );

    }//END getActivity method

    /// =================================================================================
    /// <summary>
    ///   This method creates a selection list from the milestone array.
    /// </summary>
    /// <param name="ActivityId">String: an activity identifier</param>
    /// <returns>EvActivity: an activity object</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Iterate through the activities to fine the matching activity.
    /// 
    /// 2. If the GUIDs match return the activity
    /// </remarks>
    // ---------------------------------------------------------------------------------
    public EvActivity getActivity ( String ActivityId )
    {
      // 
      // Iterate through the activities to fine the matching activity.
      // 
      foreach ( EvActivity activity in this._ActivityList )
      {
        // 
        // If the GUIDs match return the activity
        // 
        if ( activity.ActivityId == ActivityId )
        {
          return activity;
        }

      }//END activity iteration loop

      // 
      // If nothing found return empty arm
      // 
      return null;

    }//END getActivity method

    /// =================================================================================
    /// <summary>
    /// This method creates a selection list from the milestone array.
    /// </summary>
    /// <param name="Activity">EvActivity: an Activity object</param>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Iterate through the activities to find the matching activity.
    /// 
    /// 2. If the activity and mandatory match the update the activity.
    /// 
    /// 3. Update the guid and activity object.
    /// 
    /// 4. If the activity does not exist in the list add it.
    /// </remarks>
    // ---------------------------------------------------------------------------------
    public void saveActivity ( EvActivity Activity )
    {
      // 
      // Iterate through the activities to find the matching activity.
      // 
      for ( int count = 0; count < this._ActivityList.Count; count++ )
      {
        EvActivity activity = this._ActivityList [ count ];

        // 
        // If the activity and mandatory match the update the activity.
        // 
        if ( activity.ActivityId == Activity.ActivityId
          && activity.IsMandatory == Activity.IsMandatory )
        {
          //
          // Update the guid identity.
          //
          Activity.Guid = activity.Guid;

          //
          // update the object.
          //
          activity = Activity;

          //
          // exit
          //
          return;
        }

      }//END activity iteration loop

      //
      // If the activity does not exist in the list add it.
      //
      this._ActivityList.Add ( Activity );

    }//END saveActivity method

    /// =================================================================================
    /// <summary>
    ///   This method creates a selection list from the milestone array.
    /// </summary>
    /// <param name="IsMandatory">bool: return activity if true.</param>
    /// <param name="ActivityIsCompleted">bool: True indicates the activity has been completed.</param>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Iterate through the activities to fine the matching activity.
    /// 
    /// 2. If is mandatory and activity type is clinical update is state.
    /// </remarks>
    // ---------------------------------------------------------------------------------
    public void setClinicalActivityToCompleted (
      bool IsMandatory,
      bool ActivityIsCompleted )
    {
      //
      // Iterate through the activity updating the matching activity
      //
      foreach ( EvActivity activity in this._ActivityList )
      {
        if ( activity.IsMandatory == IsMandatory
          && activity.Type == EvActivity.ActivityTypes.Clinical )
        {
          //
          // Initialise the status to created to cater for a deselection outcome.
          //
          activity.Action = EvActivity.ActivitiesActionsCodes.Save;
          activity.Status = EvActivity.ActivityStates.Created;

          //
          // If the is completed is true set the status to completed.
          //
          if ( ActivityIsCompleted == true )
          {
            activity.Status = EvActivity.ActivityStates.Completed;
            activity.CompletedBy = this._UserCommonName;
            activity.CompletionDate = DateTime.Now;
          }
        }
      }

    }//END setClinicalActivityToCompleted method.

    /// =================================================================================
    /// <summary>
    ///   This method creates a selection list from the milestone array.
    /// </summary>
    /// <param name="ActivityIds">String: a ';' encoded list of activityIds.</param>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Iterate through the activities to fine the matching activity.
    /// 
    /// 2. If is mandatory and activity type is non clinical and activityid match update its state.
    /// </remarks>
    // ---------------------------------------------------------------------------------
    public void setClinicalActivitiesToCompleted (
      String ActivityIds )
    {
      String [ ] activityIds = ActivityIds.Split ( ';' );

      //
      // Iterate through the activity updating the matching activity
      //
      foreach ( EvActivity activity in this._ActivityList )
      {
        //
        // skip the clinical activities.
        if ( activity.Type != EvActivity.ActivityTypes.Clinical )
        {
          continue;
        }

        //
        // Initialise the activity to not selected.
        //
        activity.Action = EvActivity.ActivitiesActionsCodes.Save;
        activity.Status = EvActivity.ActivityStates.Created;

        //
        // iterate through the list of activityIds for a match.
        //
        foreach ( String activityId in activityIds )
        {
          //
          // if the activityIds match set the activity to completed.
          //
          if ( activity.ActivityId == activityId )
          {
            activity.Status = EvActivity.ActivityStates.Completed;
            activity.CompletedBy = this._UserCommonName;
            activity.CompletionDate = DateTime.Now;
          }
        }//END activityId iteration loop

      }//END activity list iteration loop

    }//END setNonClinicalActivityStatus method.

    /// =================================================================================
    /// <summary>
    ///   This method creates a selection list from the milestone array.
    /// </summary>
    /// <param name="ActivityIds">String: a ';' encoded list of activityIds.</param>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Iterate through the activities to fine the matching activity.
    /// 
    /// 2. If is mandatory and activity type is non clinical and activityid match update its state.
    /// </remarks>
    // ---------------------------------------------------------------------------------
    public void setNonClinicalActivitiesToCompleted (
      String ActivityIds )
    {
      String [ ] activityIds = ActivityIds.Split ( ';' );

      //
      // Iterate through the activity updating the matching activity
      //
      foreach ( EvActivity activity in this._ActivityList )
      {
        //
        // skip the clinical activities.
        if ( activity.Type == EvActivity.ActivityTypes.Clinical )
        {
          continue;
        }

        //
        // Initialise the activity to not selected.
        //
        activity.Action = EvActivity.ActivitiesActionsCodes.Save;
        activity.Status = EvActivity.ActivityStates.Created;

        //
        // iterate through the list of activityIds for a match.
        //
        foreach ( String activityId in activityIds )
        {
          //
          // if the activityIds match set the activity to completed.
          //
          if ( activity.ActivityId == activityId )
          {
            activity.Status = EvActivity.ActivityStates.Completed;
            activity.CompletedBy = this._UserCommonName;
            activity.CompletionDate = DateTime.Now;
          }
        }//END activityId iteration loop

      }//END activity list iteration loop

    }//END setNonClinicalActivityStatus method.

    #endregion

    #region Static Methods

    //  =================================================================================
    /// <summary>
    /// This class obtains the list of clinical MilestoneTypes.
    /// 
    /// Author: Andres Castano
    /// 14 Dec 2009
    /// </summary>
    /// <param name="Project">EvProject:  the project configuration object.</param>
    /// <param name="Schedule">EvSchedule:  the project schedule.</param>
    /// <param name="isSelectionList">Bool: True indicates selection list is required</param>
    /// <returns>List of EvOption object: a list of all milestones type options</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize the clinicalMilestones list object.
    /// 
    /// 2. Add clinicalMilestone properties.
    /// 
    /// 3. Return a clinicalMilestone list object.
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public static List<EvOption> getAllMilestoneList (
      EdApplication Project,
      EvSchedule Schedule,
      bool isSelectionList )
    {
      //
      // Initialize the clinicalMilestones list object.
      //
      List<EvOption> selectionList = new List<EvOption> ( );
      EvOption option = new EvOption ( );

      // 
      // If selection list is true add a Null selection.
      // 
      if ( isSelectionList == true )
      {
        selectionList.Add ( new EvOption ( MilestoneTypes.Null.ToString ( ), String.Empty ) );
      }

      //
      // Add clinicalMilestone properties.
      //
      selectionList.Add ( EvStatics.Enumerations.getOption ( MilestoneTypes.Clinical ) );
      selectionList.Add ( EvStatics.Enumerations.getOption ( MilestoneTypes.Questionnaire ) );
      selectionList.Add ( EvStatics.Enumerations.getOption ( MilestoneTypes.Implant_Visit ) );
      selectionList.Add ( EvStatics.Enumerations.getOption ( MilestoneTypes.Patient_Record ) );
      selectionList.Add ( EvStatics.Enumerations.getOption ( MilestoneTypes.Repeating_Milestone ) );
      selectionList.Add ( EvStatics.Enumerations.getOption ( MilestoneTypes.UnScheduled ) );
      selectionList.Add ( EvStatics.Enumerations.getOption ( MilestoneTypes.Monitored ) );
      selectionList.Add ( EvStatics.Enumerations.getOption ( MilestoneTypes.Non_Clinical ) );
      selectionList.Add ( EvStatics.Enumerations.getOption ( MilestoneTypes.Manual ) );
      selectionList.Add ( EvStatics.Enumerations.getOption ( MilestoneTypes.Three_Monthly ) );
      selectionList.Add ( EvStatics.Enumerations.getOption ( MilestoneTypes.Six_Monthly ) );
      selectionList.Add ( EvStatics.Enumerations.getOption ( MilestoneTypes.Annual ) );

      //
      // Return a clinicalMilestone list object.
      //
      return selectionList;

    }//END AllMilestones class

    //  =================================================================================
    /// <summary>
    /// This class obtains the list of preclinical MilestoneTypes.
    /// 
    /// Author: Andres Castano
    /// 14 Dec 2009
    /// </summary>
    /// <returns>List of EvMilestone.MilestoneTypes</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize the clinicalMilestones list object.
    /// 
    /// 2. Add properties to the clinicalMilestone list
    /// 
    /// 3. Return the clinicalMilestone list.
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public static List<EvOption> getVisitStateList (
      bool isSelectionList )
    {
      List<EvOption> optionList = new List<EvOption> ( );
      if ( isSelectionList == true )
      {
        optionList.Add ( new EvOption ( ) );
      }
      optionList.Add ( EvcStatics.Enumerations.getOption ( EvMilestone.MilestoneStates.Scheduled ) );
      optionList.Add ( EvcStatics.Enumerations.getOption ( EvMilestone.MilestoneStates.Attended ) );
      optionList.Add ( EvcStatics.Enumerations.getOption ( EvMilestone.MilestoneStates.Completed ) );
      optionList.Add ( EvcStatics.Enumerations.getOption ( EvMilestone.MilestoneStates.Monitored ) );
      optionList.Add ( EvcStatics.Enumerations.getOption ( EvMilestone.MilestoneStates.Issues_Resolved ) );
      optionList.Add ( EvcStatics.Enumerations.getOption ( EvMilestone.MilestoneStates.Cancelled ) );

      return optionList;

    }//END PreClinicalMilestones class

    //  =================================================================================
    /// <summary>
    /// This class obtains the list of preclinical MilestoneTypes.
    /// 
    /// Author: Mayank Gambhir
    /// 22 Jun 2017
    /// </summary>
    /// <returns>List of Email Period</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize the email period list object.
    /// 
    /// 2. Add properties to the email period list
    /// 
    /// 3. Return the email period list.
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public static List<EvOption> getEmailPeriod ( bool isSelectionList )//mayank
    {
      List<EvOption> optionList = new List<EvOption> ( );
      if ( isSelectionList == true )
      {
        optionList.Add ( new EvOption ( ) );
      }
      //optionList.Add(EvStatics.Enumerations.getOption(EvMilestone.MilestoneStates.Scheduled));
      //optionList.Add(EvStatics.Enumerations.getOption(EvMilestone.MilestoneStates.Attended));
      //optionList.Add(EvStatics.Enumerations.getOption(EvMilestone.MilestoneStates.Completed));
      //optionList.Add(EvStatics.Enumerations.getOption(EvMilestone.MilestoneStates.Monitored));
      //optionList.Add(EvStatics.Enumerations.getOption(EvMilestone.MilestoneStates.Issues_Resolved));
      //optionList.Add(EvStatics.Enumerations.getOption(EvMilestone.MilestoneStates.Cancelled));

      return optionList;

    }//END PreClinicalMilestones class

    //  =================================================================================
    /// <summary>
    /// This method returns a milestone matching the milestone id.
    /// </summary>
    /// <param name="MilestoneList">List of EvMilestone: a list of milestone objects.</param>
    /// <param name="MilestoneId">String: a Milestone identifier.</param>
    /// <returns>EvMilestone: a Milestone list object.</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Loop through the MilestoneList
    /// 
    /// 2. If MilestoneId exists, return milestone
    /// 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public static EvMilestone getListMilestone ( List<EvMilestone> MilestoneList, String MilestoneId )
    {
      //
      // Loop through the MilestoneList
      //
      foreach ( EvMilestone milestone in MilestoneList )
      {
        //
        // If MilestoneId exists, return milestone
        //
        if ( milestone.MilestoneId == MilestoneId )
        {
          return milestone;
        }
      }//END Iteration looop.

      return new EvMilestone ( );

    }//END getListMilestone method.
    #endregion

  }//END EvMilestone class

}//END namespace Evado.Model
