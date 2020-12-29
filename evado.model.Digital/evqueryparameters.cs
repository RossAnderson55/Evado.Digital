/***************************************************************************************
 * <copyright file="QueryParameters.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the QueryParameters data object.
 *
 ****************************************************************************************/

using System;
using System.Collections; using System.Collections.Generic;

namespace Evado.Model.Digital
{

  /// <summary>
  /// Business entity used to model Therapy query object.
  /// </summary>
  [Serializable]
  public class EvQueryParameters
  {
    #region Initialization
    /// <summary>
    /// This method initialise object
    /// </summary>
    public EvQueryParameters( )
    { }

    /// <summary>
    /// This method initialise teh query parameter object with defined values.
    /// </summary>
    /// <param name="ProjectId">String Project identifier</param>
    public EvQueryParameters( String ProjectId )
    {
      this.ApplicationId = ProjectId;
    }

    /// <summary>
    /// This method initialise teh query parameter object with defined values.
    /// </summary>
    /// <param name="ProjectId">String Project identifier</param>
    /// <param name="OrgId">String: organisation identifier</param>
    public EvQueryParameters ( String ProjectId, string OrgId )
    {
      this.ApplicationId = ProjectId;
      this.OrgId = OrgId;
    }
    #endregion

    #region Public constants

    /// <summary>
    /// This constant defines a subject in trial of query parameters
    /// </summary>
    public const string Subject_Intrial = "inTrial";

    /// <summary>
    /// This constant defines a subject out trial of query parameters
    /// </summary>
    public const string Subject_OutOftrial = "OutOftrial";

    #endregion

    #region Public member variables

    /// <summary>
    /// This field defines the patient identifier.
    /// </summary>
    public int PatientId = -1;

    /// <summary>
    /// This field defines the sex selectio.
    /// </summary>
    public string Sex = String.Empty;

    /// <summary>
    /// This field defines the lower age limit
    /// </summary>
    public int AgeLower = 0;

    /// <summary>
    /// This field defines the age upper limit
    /// </summary>
    public int AgeUpper = 199;

    /// <summary>
    /// This field defines the lower BMI limit
    /// </summary>
    public float BmiLower = 0;

    /// <summary>
    /// This field defines the upper BMI limit.
    /// </summary>
    public float BmiUpper = 100;

    /// <summary>
    /// This field defines the disease selection.
    /// </summary>
    public string Disease = String.Empty;

    /// <summary>
    /// This field defines the category selection
    /// </summary>
    public string Category = String.Empty;

    /// <summary>
    /// This field defines the selected date of birth.
    /// </summary>
    public DateTime DateOfBirth = EvcStatics.CONST_DATE_NULL;

    /// <summary>
    /// This field defines the lower date range.
    /// </summary>
    public DateTime StartDate = EvcStatics.CONST_DATE_NULL;

    /// <summary>
    /// This field defines the upper date range
    /// </summary>
    public DateTime FinishDate = EvcStatics.CONST_DATE_NULL;

    /// <summary>
    /// This field defines the date period in months.
    /// </summary>
    public int DatePeriodMonths = 0;

    /// <summary>
    /// This field defines the objects are current.
    /// </summary>
    public bool IsCurrent = true;

    /// <summary>
    /// This field defines the result is depersonalised
    /// </summary>
    public bool DepersonalisedAccess = true;

    /// <summary>
    /// This field defines the record fields are to be included in the result.
    /// </summary>
    public bool IncludeRecordFields = false;

    /// <summary>
    /// This field defines the SAe fiels area to be included in the result.
    /// </summary>
    public bool IncludeSaeFields = false;

    /// <summary>
    /// This field defines the defines if comments are to be included in the result.
    /// </summary>
    public bool IncludeComments = false;

    /// <summary>
    /// This field defines the include summaries data in the result
    /// </summary>
    public bool IncludeSummary = true;

    /// <summary>
    /// This field defines the hide all results for test sites.
    /// </summary>
    public bool IncludeTestSites = false;

    /// <summary>
    /// This field defines the selection is do include only monitor records or visits.
    /// </summary>
    public bool HasBeenMonitored = false;

    /// <summary>
    /// This field defines the user visit data is to be selected.
    /// </summary>
    public bool UserVisitDate = false;
    /// <summary>
    /// This field defines the result is to inlcude full data set.
    /// </summary>
    public bool FullDataSet = false;

    /// <summary>
    /// This field defines the result set start range index
    /// </summary>
    public int RecordRangeStart = 0;
    /// <summary>
    /// This field defines the result set finish rand index.
    /// </summary>
    public int RecordRangeFinish = 100000000;
    /// <summary>
    /// This field defines the that the user has edit access to the records.
    /// </summary>
    public bool hasRecordEditAccess = false;

    //public bool SubjectRecordQueryStatus = false;


    //State
    /// <summary>
    /// This field defines the subject's screening identifier.
    /// </summary>
    public string State = String.Empty;
    /// <summary>
    /// This field defines the subject's state selection
    /// </summary>
    public string SubjectState = String.Empty;

    /// <summary>
    /// This field defines the to not select the subject or record state.
    /// </summary>
    public bool NotSelectedState = false;
    /// <summary>
    /// This field defines the to incude user Guid in output
    /// </summary>
    public bool UseGuid = false;

    /// <summary>
    /// This field defines the output is to be result table count.
    /// </summary>
    public bool CountOnly = false;

    //Ids
    /// <summary>
    /// This field defines a patient's primary Id
    /// </summary>
    public string PrimaryId = String.Empty;
    /// <summary>
    /// This field defines the project identifier
    /// </summary>
    public string ApplicationId = String.Empty;
    /// <summary>
    /// This field defines the schedule idenifier
    /// </summary>
    public int ScheduleId = 1;
    /// <summary>
    /// this field defines the selected organisation identifier
    /// </summary>
    public string OrgId = String.Empty;
    /// <summary>
    /// this field defines the selected object type
    /// </summary>
    public string ObjectType = String.Empty;
    /// <summary>
    /// This field defines the visit identifier
    /// </summary>
    public string VisitId = String.Empty;
    /// <summary>
    /// This field defines the milestone identifier
    /// </summary>
    public string MilestoneId = String.Empty;
    /// <summary>
    /// this field defines the subject identifier.
    /// </summary>
    public string SubjectId = String.Empty;
    /// <summary>
    /// this field defines the inter record reference identifer
    /// </summary>
    public string ReferenceId = String.Empty;
    /// <summary>
    /// This field defines the subject's external identifier.
    /// </summary>
    public string ExternalId = String.Empty;
    /// <summary>
    /// This field defines the subject's screening identifier.
    /// </summary>
    public string ScreeningId = String.Empty;
    /// <summary>
    /// This field defines the subject's sponsor identifier.
    /// </summary>
    public string SponsorId = String.Empty;
    /// <summary>
    /// This field defines the subject's randomised identifier.
    /// </summary>
    public string RandomisedId = String.Empty;
    /// <summary>
    /// This field defines the form identifier.
    /// </summary>
    public string FormId = String.Empty;
    /// <summary>
    /// This field defines the user's role identifier.
    /// </summary>
    public string RoleId = String.Empty;

    /// <summary>
    /// This field defines the user common name.
    /// </summary>
    public string UserId = String.Empty;

    /// <summary>
    /// This field defines the user common name.
    /// </summary>
    public string UserCommonName = String.Empty;

    /// <summary>
    /// This field defines the if the summarised results are required.
    /// </summary>
    public bool SummaryResult = false;
    #endregion

    #region Class property
    private string _FamilyName = String.Empty;
    /// <summary>
    /// This property contains a family name of query parameters
    /// </summary>
    public string FamilyName
    {
      get { return _FamilyName; }
      set { _FamilyName = value.Trim(); }
    }

    private string _FirstName = String.Empty;
    /// <summary>
    /// This property contains a first name of query parameters
    /// </summary>
    public string FirstName
    {
      get { return _FirstName; }
      set { _FirstName = value; }
    }
    /// <summary>
    /// This field defines the order of the output 
    /// </summary>
    public string OrderBy = String.Empty;

    /// <summary>
    /// this field defines the maximum list length of the output.
    /// </summary>
    public int MaxListLength = 1000000;
    /// <summary>
    /// This property contains a date of birth string of query parameters
    /// </summary>
    public string stDateOfBirth
    {
      get
      {
        return DateOfBirth.ToString( "dd MMM yyyy" );

      }//END get method

      set
      {
        // 
        // Convert the string to the datatime object.
        // 
        if ( DateTime.TryParse( value, out DateOfBirth ) == false )
        {
          // 
          // On failure set the DOB to null date.
          // 
          DateOfBirth = EvcStatics.CONST_DATE_NULL;

        }//END if TryParse fails 

      }//END set method

    }//END stDateOfBirth property.

    /// <summary>
    /// This property contains start date string of query parameters
    /// </summary>
    public string stStartDate
    {
      get
      {
        return this.StartDate.ToString( "dd MMM yyyy" );

      }//END get method

      set
      {
        // 
        // Convert the string to the datatime object.
        // 
        if ( DateTime.TryParse( value, out this.StartDate ) == false )
        {
          // 
          // On failure set the DOB to null date.
          // 
          this.StartDate = EvcStatics.CONST_DATE_NULL;

        }//END if TryParse fails 

      }//END set method

    }//END stStartDate property.

    /// <summary>
    /// This property contains finish date string of query parameters
    /// </summary>
    public string stFinishDate
    {
      get
      {
        return this.FinishDate.ToString( "dd MMM yyyy" );

      }//END get method

      set
      {
        // 
        // Convert the string to the datatime object.
        // 
        if ( DateTime.TryParse( value, out this.FinishDate ) == false )
        {
          // 
          // On failure set the DOB to null date.
          // 
          this.FinishDate = EvcStatics.CONST_DATE_NULL;

        }//END if TryParse fails 

      }//END set method

    }//END stFinishDate property

    #endregion

  }//END EvQueryParameters class

}//END namespace Evado.Model.Digital
