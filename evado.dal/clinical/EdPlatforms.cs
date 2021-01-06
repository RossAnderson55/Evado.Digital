/***************************************************************************************
 * <copyright file="Dal\eClinical\EvSiteProfiles.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2020 EVADO HOLDING PTY. LTD..  All rights reserved.
 *     
 *      The use and distribution terms for this software are contained in the file
 *      named license.txt, which can be found in the root of this distribution.
 *      By using this software in any fashion, you are agreeing to be bound by the
 *      terms of this license.
 *     
 *      You must not remove this notice, or any other, from this software.
 *     
 * </copyright>
 * 
 * Description: 
 *  This class contains the EvCommonFormFields business object.
 *
 ****************************************************************************************/


using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;

//Annotations to XML instrument System specific libraries

using Evado.Model;
using Evado.Model.Digital;


namespace Evado.Dal.Clinical
{
  /// <summary>
  /// This class is handles the data access layer for the application profile data object.
  /// </summary>
  public class EdPlatforms : EvDalBase
  {
    #region class initailisation methods.
    // ==================================================================================
    /// <summary>
    /// This is the class initialisation method.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EdPlatforms ( )
    {
      this.ClassNameSpace = "Evado.Dal.Clinical.EvApplicationProfiles.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EdPlatforms ( EvClassParameters Settings )
    {
      this.ClassParameters = Settings;
      this.ClassNameSpace = "Evado.Dal.Clinical.EvApplicationProfiles.";

      if ( this.ClassParameters.LoggingLevel == 0 )
      {
        this.ClassParameters.LoggingLevel = Evado.Dal.EvStaticSetting.LoggingLevel;
      }
    }
    #endregion

    #region Object Initialisation
    /* *********************************************************************************
     * 
     * Defines the classes constansts and global variables
     * 
     * *********************************************************************************/
    // The log file source. 
 
    private const string SQL_APPLICATION_PROFILE_QUERY = "Select * "
      + "FROM ED_PLATFORM_SETTINGS";

    const string DB_FIELD_GUID = "APS_GUID";
    const string DB_FIELD_APPLICATION_ID = "APS_APPLICATION_ID";
    const string DB_FIELD_HOME_PAGE_HEADER = "APS_HOME_PAGE_HEADER";
    const string DB_FIELD_LICENSE_NO = "APS_LICENSE_NO";
    const string DB_FIELD_SITE_ORG_ID = "APS_SITE_ORG_ID";
    const string DB_FIELD_EARLY_WITHDRAWAL_OPTIONS = "APS_EARLY_WITHDRAWAL_OPTIONS";
    const string DB_FIELD_DISEASE_TYPE_OPTIONS = "APS_DISEASE_TYPE_OPTIONS";
    const string DB_FIELD_PATIENT_DISEASE_OPTIONS = "APS_PATIENT_DISEASE_OPTIONS";
    const string DB_FIELD_PATIENT_CATEGORY_OPTIONS = "APS_PATIENT_CATEGORY_OPTIONS";
    const string DB_FIELD_HIDDEN_DEMOGRAPHIC_FIELDS = "APS_HIDDEN_DEMOGRAPHIC_FIELDS";
    const string DB_FIELD_PI_SIGNOFF_STATEMENT = "APS_PI_SIGNOFF_STATEMENT";
    const string DB_FIELD_DISPLAY_VISIT_RESOURCES = "APS_DISPLAY_VISIT_RESOURCES";
    const string DB_FIELD_DEFAULT_TRIAL_ID = "APS_DEFAULT_TRIAL_ID";
    const string DB_FIELD_DB_ERROR_MESSAGE = "APS_DB_ERROR_MESSAGE";
    const string DB_FIELD_ERROR_MESSAGE = "APS_ERROR_MESSAGE";
    const string DB_FIELD_HELP_URL = "APS_HELP_URL";
    const string DB_FIELD_REGULATOR_REPORTS = "APS_REGULATOR_REPORTS";
    const string DB_FIELD_DISPLAY_HISTORY = "APS_DISPLAY_HISTORY";
    const string DB_FIELD_DEPERSONALISED_ACCESS = "APS_DEPERSONALISED_ACCESS";
    const string DB_FIELD_LOADED_MODULES = "APS_LOADED_MODULES";
    const string DB_FIELD_MAX_SELECTION_LENGTH = "APS_MAX_SELECTION_LENGTH";
    const string DB_FIELD_OVERRIDE_CONFIG_FILE = "APS_OVERRIDE_CONFIG_FILE";
    const string DB_FIELD_SMTP_SERVER = "APS_SMTP_SERVER";
    const string DB_FIELD_SMTP_PORT = "APS_SMTP_PORT";
    const string DB_FIELD_SMTP_USER_ID = "APS_SMTP_USER_ID";
    const string DB_FIELD_SMTP_PASSWORD = "APS_SMTP_PASSWORD";
    const string DB_FIELD_ALERT_EMAIL_ADDRESS = "APS_ALERT_EMAIL_ADDRESS";
    const string DB_FIELD_UPDATE_LOG = "APS_UPDATE_LOG";

    // 
    // Define the stored procedure names.
    // 
    private const string SQL_PROCEDURE_UPATE_ITEM = "USR_APPLICATION_PROFILE_UPDATE";
    // 
    // Define the class constants
    // 
    const string PARM_GUID = "@GUID";
    const string PARM_APPLICATION_ID = "@APPLICATION_ID";
    const string PARM_SITE_ORG_ID = "@SITE_ORG_ID";
    const string PARM_HOME_PAGE_HEADER = "@HOME_PAGE_HEADER";
    const string PARM_LICENSE_NO = "@LICENSE_NO";
    const string PARM_EARLY_WITHDRAWAL_OPTIONS = "@EARLY_WITHDRAWAL_OPTIONS";
    const string PARM_DISEASE_TYPE_OPTIONS = "@DISEASE_TYPE_OPTIONS";
    const string PARM_PATIENT_DISEASE_OPTIONS = "@PATIENT_DISEASE_OPTIONS";
    const string PARM_PATIENT_CATEGORY_OPTIONS = "@PATIENT_CATEGORY_OPTIONS";
    const string PARM_HIDDEN_DEMOGRAPHIC_FIELDS = "@HIDDEN_DEMOGRAPHIC_FIELDS";
    const string PARM_PI_SIGNOFF_STATEMENT = "@PI_SIGNOFF_STATEMENT";
    const string PARM_DISPLAY_VISIT_RESOURCES = "@DISPLAY_VISIT_RESOURCES";
    const string PARM_DEFAULT_TRIAL_ID = "@DEFAULT_TRIAL_ID";
    const string PARM_DB_ERROR_MESSAGE = "@DB_ERROR_MESSAGE";
    const string PARM_ERROR_MESSAGE = "@ERROR_MESSAGE";
    const string PARM_HELP_URL = "@HELP_URL";
    const string PARM_REGULATOR_REPORTS = "@REGULATOR_REPORTS";
    const string PARM_DISPLAY_HISTORY = "@DISPLAY_HISTORY";
    const string PARM_DEPERSONALISED_ACCESS = "@DEPERSONALISED_ACCESS";
    const string PARM_LOADED_MODULES = "@LOADED_MODULES";
    const string PARM_MAX_SELECTION_LENGTH = "@MAX_SELECTION_LENGTH";
    const string PARM_OVERRIDE_CONFIG_FILE = "@OVERRIDE_CONFIG_FILE";
    const string PARM_SMTP_SERVER = "@SMTP_SERVER";
    const string PARM_SMTP_PORT = "@SMTP_PORT";
    const string PARM_SMTP_USER_ID = "@SMTP_USER_ID";
    const string PARM_SMTP_PASSWORD = "@SMTP_PASSWORD";
    const string PARM_ALERT_EMAIL_ADDRESS = "@ALERT_EMAIL_ADDRESS";
    const string PARM_UPDATE_LOG = "@UPDATE_LOG";

    private const string PARM_SITE_ID = "@SiteId";
    private const string PARM_SITE_GUID = "@SiteGuid";
    private const string PARM_SITE_XML_DATA = "@XmlData";
    private const string PARM_SITE_UPDATE_LOG = "@UpdateLog";

    #endregion

    #region Set Query Parameters

    // =====================================================================================
    /// <summary>
    /// This class return an array of sql query parameters. 
    /// </summary>
    /// <returns>SqlParameter: an array of sql query parameters</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Create an array of sql query parameters. 
    /// 
    /// 2. Return an array of sql query parameters. 
    /// </remarks>
    //  ------------------------------------------------------------------------------------
    private static SqlParameter [ ] GetParameters ( )
    {
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
	      new SqlParameter( EdPlatforms.PARM_APPLICATION_ID, SqlDbType.Char, 1  ),
	      new SqlParameter( EdPlatforms.PARM_SITE_ORG_ID, SqlDbType.NVarChar, 10 ),
	      new SqlParameter( EdPlatforms.PARM_HOME_PAGE_HEADER, SqlDbType.NVarChar, 100 ),
	      new SqlParameter( EdPlatforms.PARM_LICENSE_NO, SqlDbType.UniqueIdentifier ),
	      new SqlParameter( EdPlatforms.PARM_EARLY_WITHDRAWAL_OPTIONS, SqlDbType.NText ),

	      new SqlParameter( EdPlatforms.PARM_DISEASE_TYPE_OPTIONS, SqlDbType.NText ),
	      new SqlParameter( EdPlatforms.PARM_PATIENT_DISEASE_OPTIONS, SqlDbType.NText ),
	      new SqlParameter( EdPlatforms.PARM_PATIENT_CATEGORY_OPTIONS, SqlDbType.NText ),
        new SqlParameter( EdPlatforms.PARM_HIDDEN_DEMOGRAPHIC_FIELDS, SqlDbType.NVarChar, 250 ),
	      new SqlParameter( EdPlatforms.PARM_PI_SIGNOFF_STATEMENT, SqlDbType.NText ),
        
        new SqlParameter( EdPlatforms.PARM_DISPLAY_VISIT_RESOURCES, SqlDbType.Bit ),
	      new SqlParameter( EdPlatforms.PARM_DEFAULT_TRIAL_ID, SqlDbType.VarChar, 10 ),
	      new SqlParameter( EdPlatforms.PARM_DB_ERROR_MESSAGE, SqlDbType.NVarChar, 100 ),
	      new SqlParameter( EdPlatforms.PARM_ERROR_MESSAGE, SqlDbType.NVarChar, 100 ),
        new SqlParameter( EdPlatforms.PARM_HELP_URL, SqlDbType.NVarChar, 50 ),

	      new SqlParameter( EdPlatforms.PARM_REGULATOR_REPORTS, SqlDbType.NText ),
	      new SqlParameter( EdPlatforms.PARM_DISPLAY_HISTORY, SqlDbType.Bit ),
	      new SqlParameter( EdPlatforms.PARM_DEPERSONALISED_ACCESS, SqlDbType.Bit ),
	      new SqlParameter( EdPlatforms.PARM_LOADED_MODULES, SqlDbType.VarChar, 250 ),
        new SqlParameter( EdPlatforms.PARM_MAX_SELECTION_LENGTH, SqlDbType.Int ),

	      new SqlParameter( EdPlatforms.PARM_OVERRIDE_CONFIG_FILE, SqlDbType.Bit ),
	      new SqlParameter( EdPlatforms.PARM_SMTP_SERVER, SqlDbType.VarChar, 100 ),
	      new SqlParameter( EdPlatforms.PARM_SMTP_PORT, SqlDbType.Int ),
	      new SqlParameter( EdPlatforms.PARM_SMTP_USER_ID, SqlDbType.NVarChar, 100 ),
        new SqlParameter( EdPlatforms.PARM_SMTP_PASSWORD, SqlDbType.NVarChar, 50 ),
	      new SqlParameter( EdPlatforms.PARM_ALERT_EMAIL_ADDRESS, SqlDbType.NVarChar, 50 ), 

	      new SqlParameter( EdPlatforms.PARM_UPDATE_LOG, SqlDbType.NText  ),
      };

      return cmdParms;
    }//END GetParameters method

    // =====================================================================================
    /// <summary>
    /// This method assigns the SiteProfile object's values to the arrray of sql parameters. 
    /// </summary>
    /// <param name="cmdParms">SqlParameter: An array of sql query parameters.</param>
    /// <param name="ApplicationSettings">EvSiteProfile: A Site profile object.</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Bind the SiteProfile object's values to the array of sql query parameters. 
    /// </remarks>
    //  ------------------------------------------------------------------------------------
    private void SetParameters ( 
      SqlParameter [ ] cmdParms, 
      Evado.Model.Digital.EdPlatform ApplicationSettings )
    {
      cmdParms [ 0 ].Value = "A";
      cmdParms [ 1 ].Value = String.Empty;
      cmdParms [ 2 ].Value = String.Empty;
      cmdParms [ 3 ].Value = Guid.Empty;
      cmdParms [ 4 ].Value = ApplicationSettings.EarlyWithdrawalOptions;

      cmdParms [ 5 ].Value = ApplicationSettings.DiseaseTypeListOptions;
      cmdParms [ 6 ].Value = ApplicationSettings.DiseaseListOptions;
      cmdParms [ 7 ].Value = ApplicationSettings.CategoryListOptions;
      cmdParms [ 8 ].Value = ApplicationSettings.HideSubjectFields;
      cmdParms [ 9 ].Value = ApplicationSettings.SignoffStatement;

      cmdParms [ 10 ].Value = false;
      cmdParms [ 11 ].Value = String.Empty;
      cmdParms [ 12 ].Value = String.Empty;
      cmdParms [ 13 ].Value = String.Empty;
      cmdParms [ 14 ].Value = ApplicationSettings.HelpUrl;

      cmdParms [ 15 ].Value = ApplicationSettings.RegulatoryReports;
      cmdParms [ 16 ].Value = false;
      cmdParms [ 17 ].Value = true;
      cmdParms [ 18 ].Value = ApplicationSettings.LoadedModules;
      cmdParms [ 19 ].Value = ApplicationSettings.MaximumSelectionListLength;

      cmdParms [ 20 ].Value = ApplicationSettings.OverRideConfig;
      cmdParms [ 21 ].Value = ApplicationSettings.SmtpServer;
      cmdParms [ 22 ].Value = ApplicationSettings.SmtpServerPort;
      cmdParms [ 23 ].Value = ApplicationSettings.SmtpUserId;
      cmdParms [ 24 ].Value = ApplicationSettings.SmtpPassword;
      cmdParms [ 25 ].Value = ApplicationSettings.EmailAlertTestAddress;

      cmdParms [ 26 ].Value = ApplicationSettings.UpdateLog;

    }//END SetParameters method

    #endregion

    #region Aplication Profile  Reader

    // =====================================================================================
    /// <summary>
    /// This method extracts the data reader values to the Site profile object.  
    /// </summary>
    /// <param name="Row">DataRow object.</param>
    /// <returns>EvSiteProfile: A site profile data object.</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Extract the compatible data reader object's values to the Site Profile Object. 
    /// 
    /// 2. Return a Site Profile Object. 
    /// 
    /// </remarks>
    //  ------------------------------------------------------------------------------------
    public Model.Digital.EdPlatform getReaderData ( DataRow Row )
    {
      //this.LogMethod ( "getReaderData method. " );
      // 
      // Initialise method variables and objects.
      // 
      Evado.Model.Digital.EdPlatform applicationSettings = new Evado.Model.Digital.EdPlatform ( );

      // 
      // Load the query results into the EvProfile object.
      //
      applicationSettings.Guid = EvSqlMethods.getGuid ( Row, EdPlatforms.DB_FIELD_GUID );

      applicationSettings.ApplicationId = EvSqlMethods.getString ( Row, EdPlatforms.DB_FIELD_APPLICATION_ID );

      applicationSettings.EarlyWithdrawalOptions = EvSqlMethods.getString ( Row, EdPlatforms.DB_FIELD_EARLY_WITHDRAWAL_OPTIONS );

      applicationSettings.DiseaseTypeListOptions = EvSqlMethods.getString ( Row, EdPlatforms.DB_FIELD_DISEASE_TYPE_OPTIONS );

      applicationSettings.DiseaseListOptions = EvSqlMethods.getString ( Row, EdPlatforms.DB_FIELD_PATIENT_DISEASE_OPTIONS );

      applicationSettings.CategoryListOptions = EvSqlMethods.getString ( Row, EdPlatforms.DB_FIELD_PATIENT_CATEGORY_OPTIONS );

      applicationSettings.HideSubjectFields = EvSqlMethods.getString ( Row, EdPlatforms.DB_FIELD_HIDDEN_DEMOGRAPHIC_FIELDS );

      applicationSettings.SignoffStatement = EvSqlMethods.getString ( Row, EdPlatforms.DB_FIELD_PI_SIGNOFF_STATEMENT );

      applicationSettings.HelpUrl = EvSqlMethods.getString ( Row, EdPlatforms.DB_FIELD_HELP_URL );

      applicationSettings.MaximumSelectionListLength = EvSqlMethods.getInteger ( Row, EdPlatforms.DB_FIELD_MAX_SELECTION_LENGTH );

      applicationSettings.OverRideConfig = EvSqlMethods.getBool ( Row, EdPlatforms.DB_FIELD_OVERRIDE_CONFIG_FILE );

      applicationSettings.SmtpServer = EvSqlMethods.getString ( Row, EdPlatforms.DB_FIELD_SMTP_SERVER );

      applicationSettings.SmtpServerPort = EvSqlMethods.getInteger ( Row, EdPlatforms.DB_FIELD_SMTP_PORT );

      applicationSettings.SmtpUserId = EvSqlMethods.getString ( Row, EdPlatforms.DB_FIELD_SMTP_USER_ID );

      applicationSettings.SmtpPassword = EvSqlMethods.getString ( Row, EdPlatforms.DB_FIELD_SMTP_PASSWORD );

      applicationSettings.EmailAlertTestAddress = EvSqlMethods.getString ( Row, EdPlatforms.DB_FIELD_ALERT_EMAIL_ADDRESS );

      applicationSettings.UpdateLog = EvSqlMethods.getString ( Row, EdPlatforms.DB_FIELD_UPDATE_LOG );

      applicationSettings.LoadedModules = EvSqlMethods.getString ( Row, EdPlatforms.DB_FIELD_LOADED_MODULES ) ;

      if ( applicationSettings.DemoAccountExpiryDays == 0 )
      {
        applicationSettings.DemoAccountExpiryDays = 28;
      }
      // 
      // Return the object.
      // 
      return applicationSettings;


    }//END getReaderData method

    // ==================================================================================
    /// <summary>
    /// This method updates the Menu role management to align it with the EvRoleList 
    /// enumerated list values.
    /// </summary>
    /// <param name="ApplicationProfile">EvMenuItem class object</param>
    // ----------------------------------------------------------------------------------
    private void updatModuleManagement ( Evado.Model.Digital.EdPlatform ApplicationProfile )
    {
      if ( ApplicationProfile.LoadedModules.Contains ( "Mobile" ) == true )
      {
        ApplicationProfile.LoadedModules = ApplicationProfile.LoadedModules.Replace (
          "Mobile",
          String.Empty );
      }

      if ( ApplicationProfile.LoadedModules.Contains ( "Administration_Module" ) == false
        && ApplicationProfile.LoadedModules.Contains ( "Administration" ) == true )
      {
        ApplicationProfile.LoadedModules = ApplicationProfile.LoadedModules.Replace (
          "Administration",
          Evado.Model.Digital.EvModuleCodes.Administration_Module.ToString ( ) );
      }

      if ( ApplicationProfile.LoadedModules.Contains ( "Clinical_Module" ) == true )
      {
        ApplicationProfile.LoadedModules = ApplicationProfile.LoadedModules.Replace (
          "Clinical_Module",
          Evado.Model.Digital.EvModuleCodes.Design_Module.ToString ( ) );
      }

      if ( ApplicationProfile.LoadedModules.Contains ( "Management_Module" ) == false
        && ApplicationProfile.LoadedModules.Contains ( "Management" ) == true )
      {
        ApplicationProfile.LoadedModules = ApplicationProfile.LoadedModules.Replace (
          "Management",
          Evado.Model.Digital.EvModuleCodes.Management_Module.ToString ( ) );
      }

      ApplicationProfile.LoadedModules = ApplicationProfile.LoadedModules.Replace ( ";;", ";" );
      ApplicationProfile.LoadedModules = ApplicationProfile.LoadedModules.Replace ( " ", String.Empty );
    }

    #endregion

    #region Retrieval Queries

    // =====================================================================================
    /// <summary>
    /// This method retrieves the SiteProfile data table based on SiteId.
    /// </summary>
    /// <param name="ApplicationId">String: A site identifier.</param>
    /// <returns>EvSiteProfile: a Site Profile data object.</returns>
    /// <remarks>
    /// This method consisits of following steps. 
    /// 
    /// 1. Return an empty SiteProfile object, if the SiteId is not defined. 
    /// 
    /// 2. Define the sql query parameters and sql query string. 
    /// 
    /// 3. Execute the sql query string and store the results on data reader. 
    /// 
    /// 4. Return an empty SiteProfile object, if the data reader object has no value.
    /// 
    /// 5. Else, extract the data reader values to the SiteProfile object. 
    /// 
    /// 6. Return a SiteProfile Object. 
    /// </remarks>
    //  ------------------------------------------------------------------------------------
    public Evado.Model.Digital.EdPlatform getItem ( string ApplicationId )
    {
      this.LogMethod ( "getItem method. " );

      // 
      // Define the local variables and objects.
      // 
      Evado.Model.Digital.EdPlatform SettingObject = new Evado.Model.Digital.EdPlatform ( );
      String sqlQueryString = String.Empty;

      // 
      // Check that there is a valid unique identifier.
      // 
      if ( ApplicationId == String.Empty )
      {
        return SettingObject;
      }

      // 
      // Define the query parameters.
      // 
      SqlParameter cmdParms = new SqlParameter ( EdPlatforms.PARM_APPLICATION_ID, SqlDbType.NVarChar, 1 );
      cmdParms.Value = ApplicationId;

      // 
      // Generate the query string.
      // 
      sqlQueryString = EdPlatforms.SQL_APPLICATION_PROFILE_QUERY
        + " WHERE (" + EdPlatforms.DB_FIELD_APPLICATION_ID + " = " + EdPlatforms.PARM_APPLICATION_ID + " ); ";

      this.LogDebug ( sqlQueryString );

      // 
      // Execute the query against the database.
      // 
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
      {
        //
        // If the reader is null, retrieve from the site profile table 
        // and add record to the application profile table.
        if ( table.Rows.Count == 0 )
        {
          SettingObject = this.getReaderData ( table.Rows [ 0 ] );

          return SettingObject;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        SettingObject = this.getReaderData ( row );
      }

      SettingObject.Parameters = this.LoadObjectParameters ( SettingObject.Guid );  

      this.LogDebug ( "Parameter list count: " + SettingObject.Parameters.Count );

      //
      // Return an object containing an EvSiteProfile data object.
      //
      this.LogMethodEnd ( "getItem" );
      return SettingObject;

    }//END getItemById method.

    #endregion

    #region ApplicationProfile Update queries

    // =====================================================================================
    /// <summary>
    /// This method adds an application profile object to the database.
    /// </summary>
    /// <param name="ApplicatonSettings">EvApplicationProfile object</param>
    /// <returns>EvEventCodes enumeration.</returns>
    //  ------------------------------------------------------------------------------------
    private EvEventCodes AddItem ( Evado.Model.Digital.EdPlatform ApplicatonSettings )
    {
      this.LogMethod ( "AddItem method. " );
      EvObjectParameters dbbObjectParameters = new EvObjectParameters ( this.ClassParameters );
      //
      // Create the update query string.
      //
      String SqlUpdateQuery = "Insert Into EV_APPLICATION_PROFILES \r\n"
        + " ( "
        + " APS_APPLICATION_ID, \r\n"
        + " APS_EARLY_WITHDRAWAL_OPTIONS, \r\n"
        + " APS_DISEASE_TYPE_OPTIONS, \r\n"
        + " APS_PATIENT_DISEASE_OPTIONS, \r\n"
        + " APS_PATIENT_CATEGORY_OPTIONS, \r\n"
        + " APS_HIDDEN_DEMOGRAPHIC_FIELDS, \r\n"
        + " APS_PI_SIGNOFF_STATEMENT, \r\n"
        + " APS_DB_ERROR_MESSAGE, \r\n"
        + " APS_ERROR_MESSAGE, \r\n"
        + " APS_HELP_URL, \r\n"
        + " APS_REGULATOR_REPORTS, \r\n"
        + " APS_LOADED_MODULES, \r\n"
        + " APS_MAX_SELECTION_LENGTH, \r\n"
        + " APS_OVERRIDE_CONFIG_FILE, \r\n"
        + " APS_DEPERSONALISED_ACCESS, \r\n"
        + " APS_SMTP_SERVER, \r\n"
        + " APS_SMTP_PORT, \r\n"
        + " APS_SMTP_USER_ID, \r\n"
        + " APS_SMTP_PASSWORD, \r\n"
        + " APS_ALERT_EMAIL_ADDRESS, \r\n"
        + " APS_UPDATE_LOG ) "
        + "values ( \r\n"
        + " 'A', \r\n"
        + " '" + ApplicatonSettings.EarlyWithdrawalOptions + "', \r\n"
        + " '" + ApplicatonSettings.DiseaseTypeListOptions + "', \r\n"
        + " '" + ApplicatonSettings.DiseaseListOptions + "', \r\n"
        + " '" + ApplicatonSettings.CategoryListOptions + "', \r\n"
        + " '" + ApplicatonSettings.HideSubjectFields + "', \r\n"
        + " '" + ApplicatonSettings.SignoffStatement + "', \r\n"
        + " '', \r\n"
        + " '', \r\n"
        + " '" + ApplicatonSettings.HelpUrl + "', \r\n"
        + " '" + ApplicatonSettings.RegulatoryReports + "', \r\n"
        + " '" + ApplicatonSettings.LoadedModules + "', \r\n"
        + " 100, \r\n"
        + " '" + ApplicatonSettings.OverRideConfig + "', \r\n"
        + " '" + true + "', \r\n"
        + " 'cpanel1.cloudplus.com', \r\n"
        + " 587, \r\n"
        + " 'noreply@evado.com', \r\n"
        + " '140-William-27', \r\n"
        + " '',  \r\n"
        + " '" + ApplicatonSettings.UpdateLog + "'); ";

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.QueryUpdate ( SqlUpdateQuery, null ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      //
      // Save the application parameters.
      //
      this.UpdateObjectParameters ( ApplicatonSettings.Parameters, ApplicatonSettings.Guid );

      //
      // Return an enumerated value EventCode status.
      //
      this.LogMethodEnd ( "AddItem" );
      return EvEventCodes.Ok;

    }//END AddItem method

    // =====================================================================================
    /// <summary>
    /// This method updates items to the SiteProfile data table.
    /// </summary>
    /// <param name="ApplicationSettings">EvSiteProfile: A site Profile data object.</param>
    /// <returns>EvEventCodes: an event code for updating items.</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Add items to datachange object if they do not exist in Old SiteProfile object. 
    /// 
    /// 2. Define the sql query parameters and execute the storeprocedure for updating items
    /// 
    /// 3. Exit, if the storeprocedure runs fail.
    /// 
    /// 4. Add datachange object's values to the backup datachanges object. 
    /// 
    /// 5. Else, return an event code for updating items. 
    /// </remarks>
    //  ------------------------------------------------------------------------------------
    public EvEventCodes updateItem ( Evado.Model.Digital.EdPlatform ApplicationSettings )
    {
      this.LogMethod ( "updateItem method " );
      //
      // Create the data change object.
      //
      EvDataChanges dataChanges = new EvDataChanges ( );
      EvDataChange dataChange = this.createDataChange ( ApplicationSettings );
      /*
      foreach ( EvObjectParameter prm in ApplicationSettings.Parameters )
      {
        this.LogDebug ( "Name: {0}, Value: {1}", prm.Name, prm.Value );
      }
      */

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = GetParameters ( );
      SetParameters ( cmdParms, ApplicationSettings );
      /*
      this.LogDebugValue ( "Parameters:" );
      foreach ( SqlParameter prm in cmdParms )
      {
        this.LogDebugValue ( "Type: " + prm.DbType
          + ", Name: " + prm.ParameterName
          + ", Value: " + prm.Value );
      }
       */ 
      try
      {
        //
        // Execute the update command.
        //
        if ( EvSqlMethods.StoreProcUpdate ( SQL_PROCEDURE_UPATE_ITEM, cmdParms ) == 0 )
        {
          return EvEventCodes.Database_Record_Update_Error;
        }

        //
        // Add the data change object if items have changed.
        //
        if ( dataChange.Items.Count > 0 )
        {
          dataChanges.AddItem ( dataChange );
        }
      }
      catch ( Exception ex )
      {
        this.LogValue ( Evado.Model.Digital.EvcStatics.getException ( ex ) );
      }

      //
      // Save the application parameters.
      //
      this.UpdateObjectParameters ( ApplicationSettings.Parameters, ApplicationSettings.Guid );

      //
      // Return an enumerated value EventCode status.
      //
      this.LogMethodEnd ( "updateItem" );
      return EvEventCodes.Ok;

    } //END updateItem method.

    //===================================================================================
    /// <summary>
    /// This method creates the data change object.
    /// 
    /// </summary>
    /// <param name="ApplicationProperties">EvApplicationProfile Object.</param>
    /// <returns>EvDataChange</returns>
    //-----------------------------------------------------------------------------------
    private EvDataChange createDataChange ( Evado.Model.Digital.EdPlatform ApplicationProperties )
    {
      // 
      // Initialise the methods variables and objects.
      // 
      EvDataChange dataChange = new EvDataChange ( );

      // 
      // Get an item to be updated.
      // 
      Evado.Model.Digital.EdPlatform oldItem = this.getItem ( ApplicationProperties.ApplicationId );

      // 
      // Compare the changes.
      // 
      EvDataChanges dataChanges = new EvDataChanges ( );
      dataChange.TableName = EvDataChange.DataChangeTableNames.EdmPlatformSettings;
      dataChange.RecordUid = 1;
      dataChange.RecordGuid = ApplicationProperties.Guid;
      dataChange.UserId = ApplicationProperties.UserId;
      dataChange.DateStamp = DateTime.Now;

      //
      // Add new items to datachanges if they do not exist in the Old Site Profile object. 
      //
      if ( ApplicationProperties.ApplicationId != oldItem.ApplicationId )
      {
        dataChange.AddItem ( "Id",
          oldItem.ApplicationId,
          ApplicationProperties.ApplicationId );
      }

      //
      // Add new items to datachanges if they do not exist in the Old Site Profile object. 
      //
      dataChange.AddItem ( "EarlyWithdrawalOptions",
        oldItem.EarlyWithdrawalOptions,
        ApplicationProperties.EarlyWithdrawalOptions );

      dataChange.AddItem ( "DiseaseTypeListOptions",
        oldItem.DiseaseTypeListOptions,
        ApplicationProperties.DiseaseTypeListOptions );

      dataChange.AddItem ( "DiseaseListOptions",
        oldItem.DiseaseListOptions,
        ApplicationProperties.DiseaseListOptions );

      dataChange.AddItem ( "CategoryListOptions",
        oldItem.CategoryListOptions,
        ApplicationProperties.CategoryListOptions );

      dataChange.AddItem ( "SignoffStatement",
        oldItem.SignoffStatement,
        ApplicationProperties.SignoffStatement );

      dataChange.AddItem ( "RegulatoryReports",
        oldItem.RegulatoryReports,
        ApplicationProperties.RegulatoryReports );

      dataChange.AddItem ( "LoadedModules",
        oldItem.LoadedModules,
        ApplicationProperties.LoadedModules );

      dataChange.AddItem ( "MaximumSelectionListLength",
        oldItem.MaximumSelectionListLength,
        ApplicationProperties.MaximumSelectionListLength );

      dataChange.AddItem ( "HideSubjectFields",
        oldItem.HideSubjectFields,
        ApplicationProperties.HideSubjectFields );

      dataChange.AddItem ( "SmtpServer",
        oldItem.SmtpServer,
        ApplicationProperties.SmtpServer );

      dataChange.AddItem ( "SmtpServerPort",
        oldItem.SmtpServerPort,
        ApplicationProperties.SmtpServerPort );

      dataChange.AddItem ( "SmtpUserId",
        oldItem.SmtpUserId,
        ApplicationProperties.SmtpUserId );

      dataChange.AddItem ( "EmailAlertTestAddress",
        oldItem.EmailAlertTestAddress,
        ApplicationProperties.EmailAlertTestAddress );

      //
      // Iterate through the parameters creating change object for each item.
      //
      for ( int i = 0; i < ApplicationProperties.Parameters.Count; i++ )
      {
        EvObjectParameter newParameter = ApplicationProperties.Parameters [ i ];
        //
        // Skip the non selected forms
        //
        if ( newParameter == null )
        {
          continue;
        }

        EvObjectParameter oldParameter = getParameter ( oldItem.Parameters, newParameter.Name );
        
        if ( oldParameter == null)
        {
          dataChange.AddItem ( "APP_" + newParameter.Name,
            String.Empty,
            newParameter.Value );
        }
        else
        {
          dataChange.AddItem ( "APP_" + newParameter.Name,
           oldParameter.Value,
            newParameter.Value );
        }
      }//END form list iteration loop.

      return dataChange;
    }

    /// <summary>
    /// This method returns the parameter object if exists in the parameter list.
    /// </summary>
    /// <param name="ParameterList"></param>
    /// <param name="Name"></param>
    /// <returns></returns>
    private EvObjectParameter getParameter ( List<EvObjectParameter> ParameterList, String Name )
    {
      //
      // If the list is null then return null 
      if ( ParameterList == null )
      {
        return null;
      }

      //
      // foreach item in the list return the parameter if the names match.
      //
      foreach ( EvObjectParameter parm in ParameterList )
      {
        if ( parm.Name.ToLower ( ) == Name.ToLower ( ) )
        {
          return parm;
        }
      }

      //
      // return null if the object is not found.
      return null;
    }//END getParameter method

    #endregion

  }//END EvSiteProfiles class

}//END namespace Evado.Dal.Clinical
