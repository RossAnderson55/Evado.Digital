/***************************************************************************************
 * <copyright file="Evado.Model\statics.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the static data objects.
 *
 ****************************************************************************************/

using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Collections.Generic;
using System.IO;

namespace Evado.Model.Digital
{
  /// <summary>
  /// This class provides static methods for use across the application.
  /// </summary>
  [Serializable]
  public partial class EvcStatics : Evado.Model.EvStatics
  {
    #region Static Constants

    public const String CONFIG_EVENT_LOG_KEY = "EventLogSource";

    /// <summary>
    /// This constant defines the global project 'GLOBAL'. 
    /// </summary>
    public const String CONST_GLOBAL_PROJECT = "GLOBAL";
    /// <summary>
    /// This constant defines the global project 'GLOBAL'. 
    /// </summary>
    public const String CONST_GLOBAL_FORM_PREFIX = "G_";

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region text substitution constants

    /// <summary>
    /// This consant defines the patient name substitution text value
    /// </summary>
    public const string TEXT_SUBSITUTION_PATIENT_NAME = "{PatientName}";

    /// <summary>
    /// This consant defines the person title substitution text value
    /// </summary>
    public const string TEXT_SUBSITUTION_TITLE = "{Title}";

    /// <summary>
    /// This consant defines the person first name substitution text value
    /// </summary>
    public const string TEXT_SUBSITUTION_FIRST_NAME = "{FirstName}";

    /// <summary>
    /// This consant defines the person family name substitution text value
    /// </summary>
    public const string TEXT_SUBSITUTION_FAMILY_NAME = "{FamilyName}";

    /// <summary>
    /// This consant defines the person email address substitution text value
    /// </summary>
    public const string TEXT_SUBSITUTION_EMAIL_ADDRESS = "{EmailAddress}";

    /// <summary>
    /// This consant defines the person user id substitution text value
    /// </summary>
    public const string TEXT_SUBSITUTION_USER_ID = "{UserId}";

    /// <summary>
    /// This consant defines the person password substitution text value
    /// </summary>
    public const string TEXT_SUBSITUTION_PASSWORD = "{Password}";

    /// <summary>
    /// This consant defines the person organisation Id (siteId) substitution text value
    /// </summary>
    public const string TEXT_SUBSITUTION_ORG_ID = "{OrgId}";

    /// <summary>
    /// This consant defines the organisation name substitution text value
    /// </summary>
    public const string TEXT_SUBSITUTION_ORG_NAME = "{OrgName}";

    /// <summary>
    /// This consant defines the customer name substitution text value
    /// </summary>
    public const string TEXT_SUBSITUTION_ADAPTER_TITLE = "{AdapterTitle}";


    /// <summary>
    /// This consant defines the password reset URL substitution text value
    /// </summary>
    public const string TEXT_SUBSITUTION_PASSWORD_RESET_URL = "{ResetUrl}";

    /// <summary>
    /// This consant defines the date stamp substitution text value
    /// </summary>
    public const string TEXT_SUBSITUTION_DATE_STAMP = "{DateStamp}";

    /// <summary>
    /// This consant defines the project identifier substitution text value
    /// </summary>
    public const string TEXT_SUBSITUTION_TRIAL_ID = "{TrialId}";

    /// <summary>
    /// This consant defines the project title substitution text value
    /// </summary>
    public const string TEXT_SUBSITUTION_TRIAL_TITLE = "{TrialTitle}";

    /// <summary>
    /// This consant defines the project description substitution text value
    /// </summary>
    public const string TEXT_SUBSITUTION_TRIAL_DESCRIPTION = "{TrialDescription}";

    /// <summary>
    /// This consant defines the project sponsor substitution text value
    /// </summary>
    public const string TEXT_SUBSITUTION_TRIAL_SPONSOR = "{TrialSponsor}";

    /// <summary>
    /// This consant defines the project protoco id substitution text value
    /// </summary>
    public const string TEXT_SUBSITUTION_TRIAL_PROTOCOL_ID = "{ProtocolId}";

    /// <summary>
    /// This consant defines the project principal investigator name  substitution text value
    /// </summary>
    public const string TEXT_SUBSITUTION_PRINCIPAL_INVESTIGATOR = "{PrincipalInvestigator}";

    /// <summary>
    /// This consant defines the project site id (orgId)  substitution text value
    /// </summary>
    public const string TEXT_SUBSITUTION_SITE_ID = "{SiteId}";

    /// <summary>
    /// This consant defines the project site name  substitution text value
    /// </summary>
    public const string TEXT_SUBSITUTION_SITE_NAME = "{SiteName}";

    /// <summary>
    /// This consant defines the project site address  substitution text value
    /// </summary>
    public const string TEXT_SUBSITUTION_SITE_ADDRESS = "{SiteAddress}";

    /// <summary>
    /// This consant defines the project site telephone number substitution text value
    /// </summary>
    public const string TEXT_SUBSITUTION_SITE_PHONE = "{SitePhoneNo}";

    /// <summary>
    /// This consant defines the project site email address  substitution text value
    /// </summary>
    public const string TEXT_SUBSITUTION_SITE_EMAIL = "{SiteEmailAddress}";

    /// <summary>
    /// This consant defines the project site investigator name substitution text value
    /// </summary>
    public const string TEXT_SUBSITUTION_SITE_INVESTIGATOR = "{SiteInvestigator}";


    /// <summary>
    /// This consant defines the schedule milestone id  substitution text value
    /// </summary>
    public const string TEXT_SUBSITUTION_MILESTONE_ID = "{MilestoneId}";

    /// <summary>
    /// This consant defines the schedule milestone title  substitution text value
    /// </summary>
    public const string TEXT_SUBSITUTION_MILESTONE_NAME = "{MilestoneTitle}";

    /// <summary>
    /// This consant defines the schedule milestone description substitution text value
    /// </summary>
    public const string TEXT_SUBSITUTION_MILESTONE_DESCRIPTION = "{MilesoneDescripton}";


    /// <summary>
    /// This consant defines the schedule milestone description substitution text value
    /// </summary>
    public const string TEXT_SUBSITUTION_ERROR_MESSAGE = "{ErrorMessage}";

    /// <summary>
    /// This consant defines the questionnaire URL substitution text value
    /// </summary>
    public const string TEXT_SUBSITUTION_QUESTIONNAIRE_URL = "{QuestionnaireUrl}";

    /// <summary>
    /// This consant defines the remote informed consent URL substitution text value
    /// </summary>
    public const string TEXT_SUBSITUTION_CONSENT_URL = "{ConsentUrl}";

    #endregion

    #region Web Config application setting key values.

    /// <summary>
    /// This constant defines the web config default trial identifier key value
    /// </summary>
    public const string CONFIG_DEFAULT_TRIAL_KEY = "DEFAULT_TRIAL_ID";

    /// <summary>
    /// This constant defines the web config site ID key value
    /// </summary>
    public const string CONFIG_PATFORM_ID_KEY = "PLATFORM";

    /// <summary>
    /// This constant defines the web config web client application url.
    /// </summary>
    public const string CONFIG_WEB_CLIENT_URL_KEY = "WEB_CLIENT_URL";

    /// <summary>
    /// This constant defines the web config question web client url.
    /// </summary>
    public const string CONFIG_QUESTIONNAIRE_URL_KEY = "QUESTIONNAIRE_URL";

    /// <summary>
    /// This constant defines the web config consent web client application url.
    /// </summary>
    public const string CONFIG_E_CONSENT_URL_KEY = "ECONSENT_URL";

    /// <summary>
    /// This constant defines the web config test email address key value
    /// </summary>
    public const string CONFIG_SMTP_TEST_ADDRESS_KEY = "EmailAlertTestAddress";

    /// <summary>
    /// This constant defines the web config test email daemon on key value
    /// </summary>
    public const string CONFIG_SMTP_TEST_DAEMON_KEY = "TestEmailDaemon";

    /// <summary>
    /// This constant defines the web config reulatory report key value
    /// </summary>
    public const string CONFIG_REGULATORY_RPTS_KEY = "RegulatoryReports";
    
    /// <summary>
    /// This constant defines the web config repository file path key value
    /// </summary>
    public const string CONFIG_RESPOSITORY_FILE_PATH_KEY = "RepositoryFilePath";

    /// <summary>
    /// This constant defines the web config loaded modules key value
    /// </summary>
    public const string CONFIG_LOADED_MODULES_KEY = "LoadedModules";

    /// <summary>
    /// This constant defines the web config hide subject static fields key value
    /// </summary>
    public const string CONFIG_HIDE_SUBJECT_FIELDS_KEY = "HideSubjectFields";

    /// <summary>
    /// This constant defines the web config maximum selectin list length key value
    /// </summary>
    public const string CONFIG_MAX_SELECTION_LENGTH_KEY = "MaximumSelectionListLength";

    /// <summary>
    /// This constant defines the web config de-identify patient data key value
    /// </summary>
    public const string CONFIG_DE_IDENTIFY_DATA_KEY = "DeIdentifySubjectData";

    /// <summary>
    /// This constant defines the web config Db connection string setting key value
    /// </summary>
    public const string CONFIG_IGNORE_FILES_LIST_KEY = "IgnoreFileList";

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region the Help URl generation method

    // =====================================================================================
    /// <summary>
    ///   This method creates a help page's URL.
    /// </summary>
    /// <param name="HelpPageUrl">string: (Mandatory) the HTTP base url for the help files..</param>
    /// <param name="PageId">EvApplicationPageIdList: (Mandatory) the page identifier enumerateion value.</param>
    /// <returns>string: http help page url reference.</returns>
    // -------------------------------------------------------------------------------------
    public static string createHelpUrl ( String HelpPageUrl, EdStaticPageIds PageId )
    {
      if ( HelpPageUrl == String.Empty
        || PageId == EdStaticPageIds.Null )
      {
        return String.Empty;
      }

      return HelpPageUrl + "/" + PageId.ToString ( ).ToLower ( ) + ".htm";

    }//END convertNulToNegInfl method 

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Age Calculations


    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region BMI methods

    //  =================================================================================
    /// <summary>
    /// StandardFormulat method
    /// 
    /// Description:
    ///  This method performs the standard BMI calculation and returns
    ///  a floating point number.
    /// </summary>
    /// <param name="Height"></param>
    /// <param name="Weight"></param>
    /// <returns></returns>
    //  ---------------------------------------------------------------------------------
    private static float StandardFormula ( float Height, float Weight )
    {
      // 
      // Initialis the local variables and objects
      // 
      float fltResult = 0;
      // 
      // Validate the input variables
      // 
      if ( Height > 0 && Weight > 0 )
      {
        Height = Height / 100;
        fltResult = Weight / ( Height * Height );

        if ( fltResult < 0 )
        {
          fltResult = 0;
        }

        return fltResult;
      }
      return 0;
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

  }

} //END namespace Evado.Model
