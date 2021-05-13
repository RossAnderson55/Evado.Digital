/***************************************************************************************
 * <copyright file="Model\EvUserSignoff.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the data model for the User SignoffBy content.  
 *		
 ****************************************************************************************/

using System;
using System.Collections.Generic;

namespace Evado.Digital.Model
{

  /// <summary>
  /// Business entity used to model accounts
  /// </summary>
  [Serializable]
  public class EdUserSignoff
  {
    #region initialise class

    //====================================================================================
    /// <summary>
    /// This method initalises the class .
    /// </summary>
    //------------------------------------------------------------------------------------
    public EdUserSignoff ( )
    { }

    //====================================================================================
    /// <summary>
    /// This method initalises the class with the following parameters.
    /// </summary>
    /// <param name="Type">TypeCode: the type of signoff</param>
    /// <param name="SignedOffUserId">String: the user id of the person that signed off the object.</param>
    /// <param name="SignedOffBy">String: the name of the person that signed off the object.</param>
    //------------------------------------------------------------------------------------
    public EdUserSignoff ( 
      TypeCode Type,
      string SignedOffUserId,
      string SignedOffBy )
    {
      this._Type = Type;
      this._SignedOffUserId = SignedOffUserId;
      this._SignedOffBy = SignedOffBy;
      this._SignOffDate = DateTime.Now;

    }

    //====================================================================================
    /// <summary>
    /// This method initalises the class with the following parameters.
    /// </summary>
    /// <param name="Type">TypeCode: the type of signoff</param>
    /// <param name="SignedOffUserId">String: the user id of the person that signed off the object.</param>
    /// <param name="SignedOffBy">String: the name of the person that signed off the object.</param>
    /// <param name="Description">String: the description of the.</param>
    //------------------------------------------------------------------------------------
    public EdUserSignoff (
      TypeCode Type,
      string SignedOffUserId,
      string SignedOffBy,
      string Description )
    {
      this._Type = Type;
      this._SignedOffUserId = SignedOffUserId;
      this._SignedOffBy = SignedOffBy;
      this._SignOffDate = DateTime.Now;
      this._Description = Description;
    }

    #endregion

    #region Class Enumerators
    /// <summary>
    /// This enumeration list defines the type code of user sign off
    /// </summary>
    public enum TypeCode
    {
      /// <summary>
      /// This enumeration defines a common signoff 
      /// </summary>
      CommonSignoff = 0,

      /// <summary>
      /// This enumeration defines a sign off for registered trial
      /// </summary>
      TrialRegistered = -100,

      /// <summary>
      /// This enumeration defines a sign off for approved trial ethnics
      /// </summary>
      TrialEthicsApproved = -101,

      /// <summary>
      /// This enumeration defines a sign off for started trial recruitment
      /// </summary>
      TrialRecruitmentStarted = -103,

      /// <summary>
      /// This enumeration defines a sign off for closed trial recruitment
      /// </summary>
      TrialRecruitmentClosed = -104,

      /// <summary>
      /// This enumeration defines a sign off for locked trial
      /// </summary>
      TrialLocked = -105,

      /// <summary>
      /// This enumeration defines a trial unlocked sign off
      /// </summary>
      TrialUnLocked = -106,

      /// <summary>
      /// This enumeration defines a sign off for closed trial
      /// </summary>
      TrialClosed = -107,

      /// <summary>
      /// This enumeration defines a sign off for registered trial
      /// </summary>
      Trial_Registered = -110,

      /// <summary>
      /// This enumeration defines a sign off for approved trial ethics
      /// </summary>
      Trial_Ethics_Approved = -111,

      /// <summary>
      /// This enumeration defines a sign off for started trial recruitment
      /// </summary>
      Trial_Recruitment_Started = -113,

      /// <summary>
      /// This enumeration defines a sign off for closed trial recruitment
      /// </summary>
      Trial_Recruitment_Closed = -114,

      /// <summary>
      /// This enumeration defines a sign off for locked trial
      /// </summary>
      Trial_Locked = -115,

      /// <summary>
      /// This enumeration defines a sign off for unlocked trial
      /// </summary>
      Trial_UnLocked = -116,

      /// <summary>
      /// This enumeration defines a sign off for closed trial
      /// </summary>
      Trial_Closed = -117,

      /// <summary>
      /// This enumeration defines a sign off for export scheduled trial data 
      /// </summary>
      Export_Secheduled_Trial_Data = -118,

      /// <summary>
      /// This enumeration defines a sign off for export unscheduled trial data.
      /// </summary>
      Export_Unsecheduled_Trial_Data = -119,

      /// <summary>
      /// This enumeration defines a sign off for exprt scheduled lab test data
      /// </summary>
      Export_Scheduled_Lab_Test_Data = -120,

      /// <summary>
      /// This enumeration defines a sign off for export unsecheduled lab test data
      /// </summary>
      Export_Unsecheduled_Lab_Test_Data = -121,

      /// <summary>
      /// This enumeration defines a sign off for export trial record data
      /// </summary>
      Export_Trial_Record_Data = -122,

      /// <summary>
      /// This enumeration defines a sign off for export common record data
      /// </summary>
      Export_Common_Record_Data = -123,

      /// <summary>
      /// This enumeration defines a sign off for record author
      /// </summary>
      Record_Author_Signoff = -201,

      /// <summary>
      /// This enumeration defines a sign off for record reviewer
      /// </summary>
      Record_Reviewer_Signoff = -202,

      /// <summary>
      /// This enumeration defines a sign off for record reviewer query
      /// </summary>
      Record_Reviewer_Query_Signoff = -203,

      /// <summary>
      /// This enumeration defines a sign off for record approver
      /// </summary>
      Record_Approver_Signoff = -204,

      /// <summary>
      /// This enumeration defines a sign off for record monitor
      /// </summary>
      Record_Monitor_Signoff = -205,

      /// <summary>
      /// This enumeration defines a sign off for record data manager
      /// </summary>
      Record_DataManager_Signoff = -206,

      /// <summary>
      /// This enumeration defines a sign off for unsigned record
      /// </summary>
      Record_UnSigned = -207,

      /// <summary>
      /// This enumeration defines a sign off for submitted record. 
      /// </summary>
      Record_Submitted_Signoff = -208,

      /// <summary>
      /// This enumeration defines a sign off for completed record
      /// </summary>
      Record_Completed_Signoff = -209,

      /// <summary>
      /// This enumeration defines a sign off for verified record data source
      /// </summary>
      Record_Source_Data_Verified = -210,

      /// <summary>
      /// This enumeration defines a sign off for form review
      /// </summary>
      Form_Review_Signoff = -301,

      /// <summary>
      /// This enumeration defines a sign off for form reviewer
      /// </summary>
      Form_Reviewer_Signoff = -302,

      /// <summary>
      /// This enumeration defines a sign off for form approver
      /// </summary>
      Form_Approver_Signoff = -304,

      /// <summary>
      /// This enumeration defines a sign off for form withdrawal
      /// </summary>
      Form_Withdrawal_Signoff = -305,

      /// <summary>
      /// This enumeration defines a sign off for schedule review
      /// </summary>
      Schedule_Review_Signoff = -401,

      /// <summary>
      /// This enumeration defines a sign off for schedule reviewer
      /// </summary>
      Schedule_Reviewer_Signoff = -402,

      /// <summary>
      /// This enumeration defines a sign off for schedule approver
      /// </summary>
      Schedule_Approver_Signoff = -404,

      /// <summary>
      /// This enumeration defines a sign off for schedule withdrawal
      /// </summary>
      Schedule_Withdrawal_Signoff = -405,

      /// <summary>
      /// This enumeration defines a sign off for budget review
      /// </summary>
      Budget_Review_Signoff = -501,

      /// <summary>
      /// This enumeration defines a sign off for budger reviewer
      /// </summary>
      Budget_Reviewer_Signoff = -502,

      /// <summary>
      /// This enumeration defines a sign off for budget approver
      /// </summary>
      Budget_Approver_Signoff = -504,

      /// <summary>
      /// This enumeration defines a sign off for budget withdrawal 
      /// </summary>
      Budget_Withdrawal_Signoff = -505,

      /// <summary>
      /// This enumeration defines a sign off for attended visit
      /// </summary>
      Visit_Attended = -801,

      /// <summary>
      /// This enumeration defines a sign off for completed visit
      /// </summary>
      Visit_Completed_Signoff = -802,

      /// <summary>
      /// This enumeration defines a sign off for monitored visit
      /// </summary>
      Visit_Monitored_Signoff = -804,

      /// <summary>
      /// This enumeration defines a sign off for resolved visit issues
      /// </summary>
      Visit_Issues_Resolved_Signoff = -805,

      /// <summary>
      /// This enumeration defines a sign off for cancelled visit
      /// </summary>
      Visit_Cancelled_Signoff = -806,

      /// <summary>
      /// This enumeration defines a sign off for screened subject
      /// </summary>
      Subject_Screened_Signoff = -901,

      /// <summary>
      /// This enumeration defines a sign off for subject in treatment
      /// </summary>
      Subject_In_Treatment_Signoff = -902,

      /// <summary>
      /// This enumeration defines a sign off for subject in study
      /// </summary>
      Subject_In_Study_Signoff = -904,

      /// <summary>
      /// This enumeration defines a sign off for subject completed trial
      /// </summary>
      Subject_Completed_Trial_Signoff = -905,

      /// <summary>
      /// This enumeration defines a sign off for subject failed screening
      /// </summary>
      Subject_Failed_Screening_Signoff = -906,

      /// <summary>
      /// This enumeration defines a sign off for subject left trial
      /// </summary>
      Subject_Left_Trial_Signoff = -907,

      /// <summary>
      /// This enumeration defines a sign off for subject early withdrawal
      /// </summary>
      Subject_Early_Withdrawal_Signoff = -908,

      /// <summary>
      /// This enumeration defines a sign off for died subject
      /// </summary>
      Subject_Died_Signoff = -909,

      /// <summary>
      /// This enumeration defines an unsignoff subject
      /// </summary>
      Subject_UnSignoff = -910,

      /// <summary>
      /// This enumeration defines a signoff subject
      /// </summary>
      Subject_Signoff = -911,

      /// <summary>
      /// This enumeration defines a sign off for invoiced billing
      /// </summary>
      Billing_Invoiced_Signoff = -1001,

      /// <summary>
      /// This enumeration defines a sign off for paid billing. 
      /// </summary>
      Billing_Paid_Signoff = -1002,

      /// <summary>
      /// This enumeration defines a sign off for privacy consent
      /// </summary>
      Privacy_Consent_Signoff = -1003,

      /// <summary>
      /// This enumeration defines a sign off for withdrawal pricacy consent
      /// </summary>
      Withdraw_Privacy_Consent_Signoff = -1004,

      /// <summary>
      /// This enumeration defines a sign off for data collection consent
      /// </summary>
      Data_Consent_Signoff = -1005,

      /// <summary>
      /// This enumeration defines a sign off for withdrawal data collection consnet
      /// </summary>
      Withdraw_Data_Consent_Signoff = -1006,

      /// <summary>
      /// This enumeration defines a sign off for confirming consent
      /// </summary>
      Data_Sharing_Consent_Signoff = -1007,

      /// <summary>
      /// This enumeration defines a sign off for withdrawal confirmin consent
      /// </summary>
      Withdraw_Data_Sharing_Consent_Signoff = -1008,

      /// <summary>
      /// This enumeration defines a sign off for confirming consent
      /// </summary>
      Confirm_Consent_Signoff = -1009,

      /// <summary>
      /// This enumeration defines a sign off for withdrawal confirmin consent
      /// </summary>
      Withdraw_Confirm_Consent_Signoff = -1010,

      /// <summary>
      /// This enumeration defines a sign off for ethics approval
      /// </summary>
      Ethics_Approved_Signoff = -1011,

      /// <summary>
      /// This enumeration defines a informed consent record created.
      /// </summary>
      Informed_Consent_Record_Created = -1101,

      /// <summary>
      /// This enumeration defines a participant raised queries.
      /// </summary>
      Informed_Consent_Participant_Raised_Queries = -1102,

      /// <summary>
      /// This enumeration defines a participant provided incorred quizz answers.
      /// </summary>
      Informed_Consent_Incorrect_Quiz_Answers = -1103,

      /// <summary>
      /// This enumeration defines a participant did not provide informed consent
      /// </summary>
      Informed_Consent_Participant_Did_Not_Consent = -1104,

      /// <summary>
      /// This enumeration defines a informed consent given
      /// </summary>
      Informed_Consent_Given = -1105,

      /// <summary>
      /// This enumeration defines a informed consent given
      /// </summary>
      Informed_Consent_Not_Given = -1106,

      /// <summary>
      /// This enumeration defines a queries resolved and informed consent given.
      /// </summary>
      Queries_Resolved_And_Informed_Consent_Given = 1107,

      /// <summary>
      /// This enumeration defines a participant has withdrawn informed consent
      /// </summary>
      Informed_Consent_Withdrawn = -1108,

      /// <summary>
      /// This enumeration defines a that a participant must reconsent.
      /// </summary>
      Participant_ReConsent_Required = -1109,

      /// <summary>
      /// This enumeration defines a participant wet ink informed consent.
      /// </summary>
      Wet_Ink_Informed_Consent_Given = -1110,

    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Internal members

    private TypeCode _Type = TypeCode.CommonSignoff;
    private string _SignedOffUserId = String.Empty;
    private string _SignedOffBy = String.Empty;
    private DateTime _SignOffDate =  Evado.Model.EvStatics.CONST_DATE_NULL;
    private String _Description = String.Empty;

    #endregion

    #region Public members
    /// <summary>
    /// This property contains type code for sign off
    /// </summary>
    public TypeCode Type
    {
      get
      {
        return this._Type;
      }
      set
      {
        this._Type = value;

        if ( this._Type == TypeCode.TrialRegistered )
        {
          this._Type = TypeCode.Trial_Registered;
        }
        if ( this._Type == TypeCode.TrialEthicsApproved )
        {
          this._Type = TypeCode.Trial_Ethics_Approved;
        }
        if ( this._Type == TypeCode.TrialRecruitmentStarted )
        {
          this._Type = TypeCode.Trial_Recruitment_Started;
        }
        if ( this._Type == TypeCode.TrialRecruitmentClosed )
        {
          this._Type = TypeCode.Trial_Recruitment_Closed;
        }
        if ( this._Type == TypeCode.Trial_Locked )
        {
          this._Type = TypeCode.Trial_Locked;
        }
        if ( this._Type == TypeCode.Trial_UnLocked )
        {
          this._Type = TypeCode.Trial_UnLocked;
        }
        if ( this._Type == TypeCode.TrialClosed )
        {
          this._Type = TypeCode.Trial_Closed;
        }
      }
    }

    /// <summary>
    /// This property contains user identifier of those who signs off
    /// </summary>
    public string SignedOffUserId
    {
      get
      {
        return this._SignedOffUserId;
      }
      set
      {
        this._SignedOffUserId = value;
      }
    }

    /// <summary>
    /// This property contains user who signs off
    /// </summary>
    public string SignedOffBy
    {
      get
      {
        return this._SignedOffBy;
      }
      set
      {
        this._SignedOffBy = value;
      }
    }

    /// <summary>
    /// This property contains sign off description
    /// </summary>
    public string Description
    {
      get
      {
        if ( this._Description == String.Empty )
        {
          this._Description =  Evado.Model.EvStatics.enumValueToString ( Type );
        }

        return this._Description;
      }
      set
      {
        this._Description = value;
      }
    }

    /// <summary>
    /// This property contains sign off date
    /// </summary>
    public DateTime SignOffDate
    {
      get
      {
        if (this._SignedOffBy == String.Empty)
        {
          this._SignOffDate =  Evado.Model.EvStatics.CONST_DATE_NULL;
        }
        return this._SignOffDate;
      }
      set
      {
        this._SignOffDate = value;
      }
    }

    /// <summary>
    /// This property contains sign off date string. 
    /// </summary>
    public string stSignOffDate
    {
      get
      {
        if (this._SignedOffBy == String.Empty)
        {
          this._SignOffDate =  Evado.Model.EvStatics.CONST_DATE_NULL;
        }
        if (this._SignOffDate >  Evado.Model.EvStatics.CONST_DATE_NULL)
        {
          return this._SignOffDate.ToString("dd MMM yyyy HH:mm");
        }
        return String.Empty;
      }

      set
      {
        string dValue = value;
      }
    }

    #endregion

    #region public static methods

    // =====================================================================================
    /// <summary>
    /// This method outputs the contents of the SignoffBy Array
    /// </summary>
    /// <param name="SignoffList">List of EvUserSignoff: a list of user sign off</param>
    /// <param name="withHeader">Boolean: true, if header exists</param>
    /// <returns>string: a sign off log</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Validate whether the sign off list is not null
    /// 
    /// 2. Initialise the return html string. 
    /// 
    /// 3. Iterate through the sign off list.
    /// 
    /// 4. Output a signoff if it has a value
    /// 
    /// 5. Return the html sign off string. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public static string getSignoffLog(List<EdUserSignoff> SignoffList, bool withHeader)
    {
      //
      // Validate whether the sign off list is not null
      //
      if (SignoffList == null)
      {
        return "<p>Signoff list Null.</p>";
      }
      if (SignoffList.Count == 0)
      {
        return "<p>Signoff list empty.</p>";
      }

      // 
      // Initialise the return html string. 
      // 
      string stHtml = "<p>";

      if (withHeader == true)
      {
        stHtml += "<strong>Signoff log:</strong><br/>";
      }

      // 
      // Iterate through the sign off list.
      // 
      for (int count = 0; count < SignoffList.Count; count++)
      {
        // 
        // Process the item if exists.
        // 
        if (SignoffList != null)
        {
          EdUserSignoff signoff = SignoffList[count];

          // 
          // Output a signoff if it has a value.
          // 
          if (signoff.SignedOffBy != String.Empty)
          {
            stHtml += signoff.Description
              + Evado.Digital.Model.EdLabels.Label_by + signoff.SignedOffBy
              + Evado.Digital.Model.EdLabels.Label_on + signoff.stSignOffDate
              + "<br/>";
          }//END signoff exits.
        }

      }//END signoff list iteration loop

      // 
      // Close the table tag
      // 
      stHtml += "</p>";

      // 
      // Return the new signoff array.
      // 
      return stHtml;

    }//END getSignoffLog method

    #endregion

  }//END EvUserSignoff class

}//END namespace Evado.Digital.Model