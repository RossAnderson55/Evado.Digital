/***************************************************************************************
 * <copyright file="Evado.UniForm.Model.eClinical\EvIdentifiers.cs" company="EVADO HOLDING PTY. LTD.">
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
 ****************************************************************************************/
using System;
using System.Collections.Generic;

namespace Evado.Model.Digital
{
  /// <summary>
  /// This class contains the identifier for Evado indexes.
  /// </summary>
  [Serializable]
  public class EvIdentifiers
  {

    /// <summary>
    ///  This constant defines a display prefix of the identifiers
    /// </summary>
    public const string DISPLAY_PREFIX = "DISP_";

    /// <summary>
    ///  This constant defines a trial identifier
    /// </summary>
    public const string CUSTOMER_GUID = "CUSTOMER_GUID";

    /// <summary>
    ///  This constant defines a trial identifier
    /// </summary>
    public const string CUSTOMER_NO = "CUSTOMER_NO";

    /// <summary>
    ///  This constant defines a trial identifier
    /// </summary>
    public const string TRIAL_ID = "TrialId";

    /// <summary>
    ///  This constant defines a subject identifier
    /// </summary>
    public const string SUBJECT_ID = "SubjectId";

    /// <summary>
    ///  This constant defines a screening identifier
    /// </summary>
    public const string SCREENING_ID = "ScreeningId";

    /// <summary>
    ///  This constant defines a sponsor identifier
    /// </summary>
    public const string SPONSOR_ID = "SponsorId";

    /// <summary>
    /// This constant defines an external identifier
    /// </summary>
    public const string EXTERNAL_ID = "ExternalId";

    /// <summary>
    /// This constant defines a randomized identifier
    /// </summary>
    public const string RANDOMISED_ID = "RandomisedId";

    /// <summary>
    ///  This constant defines a visit identifier
    /// </summary>
    public const string VISIT_ID = "VisitId";

    /// <summary>
    ///  This constant defines a record identifier
    /// </summary>
    public const string RECORD_ID = "RecordId";

    /// <summary>
    ///  This constant defines a form identifier
    /// </summary>
    public const string FORM_ID = "FormId";

    /// <summary>
    ///  This constant defines a field identifier
    /// </summary>
    public const string FIELD_ID = "FieldId";

    /// <summary>
    ///  This constant defines a milestone identifier
    /// </summary>
    public const string MILESTONE_ID = "MilestoneId";

    /// <summary>
    ///  This constant defines an activity identifier
    /// </summary>
    public const string ACTIVITY_ID = "ActivityId";

    /// <summary>
    ///  This constant defines an Alert identifier
    /// </summary>
    public const string ALERT_ID = "AlertId";

    /// <summary>
    ///  This constant defines an Organization identifier 
    /// </summary>
    public const string ORGANISATION_ID = "OrgId";

    /// <summary>
    ///  This constant defines a Query State identifier.
    /// </summary>
    public const string QUERY_STATE = "QueryState";

    /// <summary>
    ///  This constant defines a Record Type identifier
    /// </summary>
    public const string RECORD_TYPE = "RecordType";

    /// <summary>
    ///  This constant defines a Record date identifier
    /// </summary>
    public const string RECORD_DATE = "RecDate";

    /// <summary>
    ///  This constant defines an object state identifier
    /// </summary>
    public const string OBJECT_STATE = "State";

    /// <summary>
    ///  The constant defines an activity is mandatory field identifier
    /// </summary>
    public const string ACTIVITY_IS_MANDATORY = "IsMandatoryActivity";

    /// <summary>
    ///  The constant defines a record is mandatory field identifier
    /// </summary>
    public const string RECORD_IS_MANDATORY = "IsMandatory";

    /// <summary>
    ///  The constant defines a record is mandatory field identifier
    /// </summary>
    public const string FORM_TITLE = "FormTitle";

    /// <summary>
    /// This constant defines a forms approval command paramenter.
    /// </summary>
    public const string FORM_APPROVAL = "FormApproval";

    /// <summary>
    ///  The constant defines a submitted by field identifier
    /// </summary>
    public const string SUBMITTED_BY = "Submitted";

    /// <summary>
    ///  The constant defines a reviewed by field identifier
    /// </summary>
    public const string REVIEWED_BY = "Reviewed";

    /// <summary>
    ///  The constant defines a monitored by field identifier
    /// </summary>
    public const string MONITORED_BY = "Monitored";

    /// <summary>
    ///  The constant defines a locked field identifier
    /// </summary>
    public const string LOCKED_BY = "Locked";

    /// <summary>
    ///  The constant defines a record subject field identifier
    /// </summary>

    public const string RECORD_SUBJECT_FIELD_ID = "RecordSubject";

    /// <summary>
    ///  The constant defines a start date field identifier
    /// </summary>
    public const string START_DATE_FIELD_ID = "StartDate";

    /// <summary>
    ///  The constant defines a finish date field identifier
    /// </summary>
    public const string FINISH_DATE_FIELD_ID = "FinishDate";

    /// <summary>
    ///  The constant defines a reference field identifier
    /// </summary>
    public const string REFERENCE_ID_FIELD = "ReferenceId";

    /// <summary>
    ///  The constant defines a date of birth field identifier
    /// </summary>
    public const string DATE_OF_BIRTH = "DateOfBirth";

    /// <summary>
    ///  The constant defines a consent date field identifier
    /// </summary>
    public const string CONSENT_DATE = "ConsentDate";

    /// <summary>
    ///  The constant defines a withdraw consent date field identifier
    /// </summary>
    public const string WITHDRAW_CONSENT_DATE = "WithdrawConsentDate";

    /// <summary>
    ///  The constant defines a data consent date field identifier
    /// </summary>
    public const string DATA_CONSENT_DATE = "DataConsentDate";

    /// <summary>
    ///  The constant defines a withdraw data consent date field identifier
    /// </summary>
    public const string WITHDRAW_DATA_CONSENT_DATE = "WithdrawDataConsentDate";

    /// <summary>
    ///  The constant defines a shareing consent date field identifier
    /// </summary>
    public const string SHARING_CONSENT_DATE = "SharingConsentDate";

    /// <summary>
    ///  The constant defines a withdrawn sharing consent date field identifier
    /// </summary>
    public const string WITHDRAW_SHARNG_CONSENT_DATE = "WithdrawSharingConsentDate";

    /// <summary>
    ///  The constant defines a confirmed consent date field identifier
    /// </summary>
    public const string CONFIRM_CONSENT_DATE = "ConfirmConsentDate";

    /// <summary>
    ///  The constant defines a withdraw confirmed consent date field identifier
    /// </summary>
    public const string WITHDRAW_CONFIRM_CONSENT_DATE = "ConfirmConsentDate";

    /// <summary>
    ///  The constant defines an age field identifier
    /// </summary>
    public const string AGE = "Age";

    /// <summary>
    ///  The constant defines a nick name field identifier
    /// </summary>
    public const string NICK_NAME = "NickName";

    /// <summary>
    ///  The constant defines a sex field identifier
    /// </summary>
    public const string SEX = "Sex";

    /// <summary>
    ///  The constant defines a height field identifier
    /// </summary>
    public const string HEIGHT = "Height";

    /// <summary>
    ///  The constant defines a Weight field identifier
    /// </summary>
    public const string WEIGHT = "Weight";

    /// <summary>
    ///  The constant defines a BMI field identifier
    /// </summary>
    public const string BMI = "Bmi";

    /// <summary>
    ///  The constant defines a disease field identifier
    /// </summary>
    public const string DISEASES = "Diseases";

    /// <summary>
    ///  The constant defines a trial disease field identifier
    /// </summary>
    public const string PROJECT_DISEASES = "ProjectlDiseases";

    /// <summary>
    ///  The constant defines a category field identifier
    /// </summary>
    public const string CATEGORIES = "Categories";

    /// <summary>
    ///  The constant defines a history field identifier
    /// </summary>
    public const string HISTORY = "History";


  }//END EvIdentifiers class

}//END namespace Evado.Model.Digital