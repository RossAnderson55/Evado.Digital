/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\EuAdapter.cs" 
 *  company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2020 EVADO HOLDING PTY. LTD.  All rights reserved.
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
 *  This class contains the AbstractedPage ResultData object.
 *
 ****************************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Configuration;

using Evado.Bll;
using Evado.Model;
using Evado.Bll.Clinical;
using Evado.Model.Digital;


namespace Evado.UniForm.Clinical
{
  /// <summary>
  /// This class manages the Evado.UniFORM.eClinical application services. Integrating
  /// Evado.eClinical with the Evado.UniFORM technology and user interface environment.
  /// </summary>
  public partial class EuAdapter : Evado.Model.UniForm.ApplicationAdapterBase
  {
    #region Class Initialisation Methods

    // ==================================================================================
    /// <summary>
    /// The ApplicationService class initialisation method.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EuAdapter ( )
    {
      this.ClassNameSpace = "Evado.UniForm.Clinical.EuAdapter.";

      Evado.Bll.EvStaticSetting.EventLogSource = this._EventLogSource;

      this.ClassParameters = new EvClassParameters ( );

      Evado.Bll.EvStaticSetting.DebugOn = false;

    }//END ApplicationService method

    // ==================================================================================
    /// <summary>
    /// The ApplicationService class initialisation method.
    /// </summary>
    /// <param name="ClientVersion">The current version of the client.</param>  
    /// <param name="GlobalObjects">Hashtable: Containing global application variables.</param>  
    /// <param name="ServiceUserProfile">HttpServiceUserProfile: containing the user session state and variables.</param>
    /// <param name="ServiceUserProfile">Evado.Model.EvUserProfile: the user base profile for the UniForm environment.</param>
    /// <param name="ExitCommand">Evado.Model.UniForm.Command: the next page's exit groupCommand, to return to the previous page.</param>
    /// <param name="ApplicationPath">String: The UNC path for the application.</param>
    /// <param name="UniForm_BinaryFilePath">String: The UNC path tp access the UniForm binary file objects</param>
    /// <param name="UniForm_BinaryServiceUrl">Url String: the url to access the UniForm binary objects.</param>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Load the application's global ResultData object.
    /// 
    /// 2. Load the user session objects retrieving the user's clinical ResultData object.
    /// 
    /// 3. Load global objects by callling this._ApplicationObjects.loadGlobalParameters ( ).
    /// 
    /// 4. Retrieve the application's web configuration file parameters.
    /// 
    /// Raise and log any exception events.
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public EuAdapter (
      float ClientVersion,
      Hashtable GlobalObjects,
      Evado.Model.EvUserProfileBase ServiceUserProfile,
      Evado.Model.UniForm.Command ExitCommand,
      String ApplicationPath,
      String UniForm_BinaryFilePath,
      String UniForm_BinaryServiceUrl )
    {
      this.ClassNameSpace = "Evado.UniForm.Clinical.EuAdapter.";
      this.LogInitMethod ( "EuAdapter initialisation" );
      try
      {
        this.ClassParameters = new EvClassParameters ( );
        this._ClientVersion = ClientVersion;
        this.GlobalObjectList = GlobalObjects;
        this.ServiceUserProfile = ServiceUserProfile;
        this.ExitCommand = ExitCommand;
        this.ApplicationPath = ApplicationPath;
        this.UniForm_BinaryFilePath = UniForm_BinaryFilePath;
        this.UniForm_BinaryServiceUrl = UniForm_BinaryServiceUrl;

        this.LogInitValue ( "Class Parameters: " );
        this.LogInitValue ( "-ClientVersion: " + this._ClientVersion );
        this.LogInitValue ( "-ServiceUserProfile UserId: " + this.ServiceUserProfile.UserId );
        this.LogInitValue ( "-ClientDataObject.Title: " + this.ClientDataObject.Title );
        this.LogInitValue ( "-UniForm_BinaryFilePath: " + this.UniForm_BinaryFilePath );
        this.LogInitValue ( "-UniForm_BinaryServiceUrl: " + this.UniForm_BinaryServiceUrl );
        this.LogInitValue ( "-ApplicationPath: " + this.ApplicationPath );
        this.LogInitValue ( "-ExitCommand: " + this.ExitCommand.getAsString ( false, true ) );

        //
        // if last page is null then the 
        if ( this.Session.LastPage == null )
        {
          this.Session.LastPage = new Model.UniForm.AppData ( );
        }

        Evado.Bll.EvStaticSetting.EventLogSource = this._EventLogSource;

        //
        // Define the settings object.
        //
        this.ClassParameters.UserProfile = new EvUserProfile ( this.ServiceUserProfile );
        this.ClassParameters.LoggingLevel = 5;

        this._ApplicationObjects = new EuApplicationObjects ( this.ClassParameters );
        this._ApplicationObjects.LoggingLevel = 5;
        //
        // Delete the old application objects.
        //
        this.DeleteOldTemporaryFiles ( );

        //
        // Load the global application objects.
        //
        this.loadGlobalObjects ( );

        //
        // Define the application Guid
        //
        this.ClassParameters.ApplicationGuid = this._ApplicationObjects.PlatformSettings.Guid;
        this.ClassParameters.PlatformId = this._ApplicationObjects.PlatformId;

        //
        // load the user's session object in to memory.
        //
        this.loadSessionObjects ( );

        //
        // set the setting customer GUID value.
        // 
        this.ClassParameters.CustomerGuid = this.Session.Customer.Guid;

        this.LogInitValue ( "ServiceUserProfile\r\n-AdsCustomerGroup: " + this.ServiceUserProfile.AdsCustomerGroup );

        this.LogInitValue ( "Settings:" );
        this.LogInitValue ( "-PlatformId: " + this.ClassParameters.PlatformId );
        this.LogInitValue ( "-CustomerGuid: " + this.ClassParameters.CustomerGuid );
        this.LogInitValue ( "-ApplicationGuid: " + this.ClassParameters.ApplicationGuid );
        this.LogInitValue ( "-LoggingLevel: " + ClassParameters.LoggingLevel );
        this.LogInitValue ( "-UserId: " + ClassParameters.UserProfile.UserId );
        this.LogInitValue ( "-UserCommonName: " + ClassParameters.UserProfile.CommonName );

        this.LogInitValue ( "ProjectSelectionList.Count: " + this.Session.TrialSelectionList.Count );
        this.LogInitValue ( "ClientDataObject.Page.Title: " + this.ClientDataObject.Page.Title );

        if ( this.Session.AdminUserProfile != null )
        {
          this.LogInitValue ( "AdminUserProfile.Guid: " + this.Session.AdminUserProfile.Guid );
          this.LogInitValue ( "AdminUserProfile.UserId: " + this.Session.AdminUserProfile.UserId );
          this.LogInitValue ( "AdminUserProfile.CommonName: " + this.Session.AdminUserProfile.CommonName );
        }

      }
      catch ( Exception Ex )
      {
        this.LogException ( Ex );

      }
      String end = Evado.Model.Digital.EvcStatics.CONST_METHOD_END;
      end = end.Replace ( "END OF METHOD", "END OF Evado.UniForm.Clinical.EuAdapter METHOD" );
      this.LogInitValue ( end );

    }//END Method.

    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class constants

    /// <summary>
    /// This constant defines the web config Db connection string setting key value
    /// </summary>
    public const string CONFIG_CONNECTION_SETTING_KEY = "ConnectionSetting";

    /// <summary>
    /// This constant defines default connection string configuration key
    /// </summary>
    public const string DEFAULT_CONNECTION_SETTING_KEY = "Evado";

    /// <summary>
    /// This constant defines the password reset url configuration key
    /// </summary>
    public const string CONFIG_PASSWORD_RESET_URL_KEY = "PASSWORD_RESET_URL";

    /// <summary>
    /// This constant defines the password support email address configuration key
    /// </summary>
    public const string CONFIG_SUPPORT_EMAIL_ADDRESS_KEY = "SUPPORT_EMAIL_ADDRESS";

    /// <summary>
    /// This constant defines the noreply email address configuration key
    /// </summary>
    public const string CONFIG_NOREPLY_EMAIL_ADDRESS_KEY = "NOREPLY_EMAIL_ADDRESS";

    /// <summary>
    /// This constant defines the noreply email address configuration key
    /// </summary>
    public const string CONFIG_ADS_ENABLED_ADDRESS_KEY = "ADS_ENABLED";

    /// <summary>
    /// This constant defines the Application identifier
    /// </summary>
    public const string APPLICATION_ID = "Evado_eClinical";

    public const string CONST_ALERT_SELECT = "ALERT_SELECT";

    public const string CONST_HASHE_ITEM_KEY_SELECT = "HIKS";

    public const int CONST_MENU_GROUP_WIDTH = 165;
    public const int CONST_HOME_PAGE_GROUP_DEFAULT_WIDTH = 900;
    public const int CONST_HOME_PAGE_GROUP_MARGINS = 100;
    public const int CONST_HOME_PAGE_GROUP_MAXIMUM_WIDTH = 1200;


    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class local objects.

    public static readonly string CONST_RECORD_STATE_SELECTION_DEFAULT = EdRecordObjectStates.Withdrawn + ";" + EdRecordObjectStates.Queried_Record_Copy;

    private EuApplicationObjects _ApplicationObjects = new EuApplicationObjects ( );

    private EuSession Session = new EuSession ( );

    private String ErrorMessage = String.Empty;

    EuMenuUtility _MenuUtility;

    private string _EventLogSource = ConfigurationManager.AppSettings [ Evado.Model.Digital.EvcStatics.CONFIG_EVENT_LOG_KEY ];

    private String _ConnectionSettingKey = EuAdapter.DEFAULT_CONNECTION_SETTING_KEY;

    private String _SessionObjectKey = String.Empty;

    private String _ClientObjectKey = String.Empty;

    //private String _PlatformId = String.Empty;

    private String _FileRepositoryPath = String.Empty;

    private EvEventCodes _EventCode = EvEventCodes.Ok;

    private Evado.Model.Digital.EvClassParameters _Settings = new Evado.Model.Digital.EvClassParameters ( );

    private Evado.Bll.EvApplicationEvents _Bll_ApplicationEvents = new Bll.EvApplicationEvents ( );

    private float _ApiVersion = Evado.Model.UniForm.AppData.API_Version;

    private float _ClientVersion = Evado.Model.UniForm.AppData.API_Version;

    private String _LicensedModules = EvModuleCodes.Administration_Module + ";"
      + EvModuleCodes.Clinical_Module + ";"
      + EvModuleCodes.Registry_Module + ";"
      + EvModuleCodes.Management_Module + ";"
      + EvModuleCodes.Patient_Module + ";"
      + EvModuleCodes.Patient_Recorded_Outcomes + ";"
      + EvModuleCodes.Patient_Recorded_Observation + ";"
      + EvModuleCodes.Imaging_Module + ";"
      + EvModuleCodes.Integration_Module + ";"
      + EvModuleCodes.Informed_Consent;

    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class Global properties.
    /// <summary>
    /// This property contains the setting data object. 
    /// </summary>
    public Evado.Model.Digital.EvClassParameters ClassParameters
    {
      get
      {
        return _Settings;
      }
      set
      {
        this._Settings = value;

        this._Bll_ApplicationEvents = new Bll.EvApplicationEvents ( this._Settings );
      }
    }

    /// <summary>
    /// This field contains the service version.
    /// </summary>
    public float ServiceVersion
    {
      get { return _ApiVersion; }
      set { _ApiVersion = value; }
    }

    /// <summary>
    /// This property contains the exit error event code for the class.
    /// </summary>
    public EvEventCodes EventCode
    {
      get { return _EventCode; }
      set { _EventCode = value; }
    }

    /// <summary>
    /// This property contains an encoded ';' list of licenced modules for this instance.
    /// </summary>
    public String LicensedModules
    {
      get { return this._LicensedModules; }
      set
      {
        this._LicensedModules = value;

        if ( this._ApplicationObjects.PlatformSettings != null )
        {
          this._ApplicationObjects.LicensedModules = this._LicensedModules;
        }
      }
    }

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Public class method

    // ==================================================================================
    /// <summary>
    /// This method gets generates the page object for the UniFORM client.
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object</param>
    /// <returns>Evado.Model.UniForm.AppData</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Verifies that a user identifier has been passed to the method.
    /// 
    /// 2. Loads the user profile object from the user session by calling this.loadUserProfile ( )
    /// if the user profile is missing exit the method.
    /// 
    /// 3. Loads the user's menu options to define the user navigation pathes by calling loadUserMenuOptions.
    /// 
    /// 4. If the groupCommand type is to go offline call this.getOfflineData ( PageCommand ) 
    ///  
    /// 5. Other wise call this.getApplicationObject ( PageCommand ) 
    /// 
    /// 6. Return the generated page definition object to the calling method.
    /// </remarks>
    // ----------------------------------------------------------------------------------
    override public Evado.Model.UniForm.AppData getPageObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getPageObject" );
      try
      {
        this.LogValue ( "Page Command: " + PageCommand.getAsString ( false, false ) );
        this.LogValue ( "Exit Command: " + this.ExitCommand.getAsString ( false, false ) );
        this.LogDebug ( "LastPage.Title: " + this.Session.LastPage.Title );
        this.LogDebug ( "Customer Guid {0}. ", this.Session.Customer.Guid );
        this.LogDebug ( "Customer No {0}. ", this.Session.Customer.CustomerNo );

        //
        // Turn on BLL debug to match the current class setting.
        //
        Evado.Bll.EvStaticSetting.DebugOn = this.DebugOn;
        this.ClassParameters.LoggingLevel = this.LoggingLevel;

        this._MenuUtility = new EuMenuUtility ( this.Session, this.ClassParameters );

        //
        // Set the web width.
        //
        String deviceName = PageCommand.GetHeaderValue ( Model.UniForm.CommandHeaderElements.DeviceName );

        //
        // Resolve application id and class id issues for default page commands.
        //
        if ( PageCommand.ApplicationId == Evado.Model.UniForm.EuStatics.CONST_DEFAULT )
        {
          PageCommand.ApplicationId = EuAdapter.APPLICATION_ID;
          PageCommand.Object = EuAdapterClasses.Home_Page.ToString ( );
        }

        // 
        // Initialise the methods variables and objects.
        // 
        Evado.Model.UniForm.AppData clientDataObject = new Model.UniForm.AppData ( );
        Evado.Bll.EvStaticSetting.SiteGuid = this._ApplicationObjects.PlatformSettings.Guid;

        // 
        // Load the user profile.
        // 
        if ( this.loadUserProfile ( PageCommand ) == false )
        {
          // 
          // The user does not have a valid profile exit with the no profile page.
          // 
          return generateNoProfilePage ( );
        }

        //
        // Process a demonstration user registration request. 
        //
        if ( PageCommand.Object == EuAdapterClasses.Demo_Registration.ToString ( ) )
        {
          this.LogDebug ( "Demonstration User Registation" );

          this.callDemonstrationRegistration ( PageCommand );

          this.LogMethodEnd ( "getPageObject" );
          return clientDataObject;
        }

        this.LogDebug ( this.Session.UserProfile.getUserProfile ( false ) );

        this.LogDebug ( "UserProfile.RoleId: " + this.Session.UserProfile.RoleId );

        if ( PageCommand.Object == "Questionnaire" )
        {
          PageCommand.Object = EuAdapterClasses.Patient_Outcomes.ToString ( );
        }

        //
        // get the customer object if it not already selected.
        //
        this.getCustomer ( PageCommand );

        //
        // Create the project selection list field.
        //
        this.GetTrialList ( );

        //
        // set the current project
        //
        this.GetTrial ( PageCommand );

        //
        // set the current project
        //
        this.loadTrialFormList ( );

        clientDataObject = this.getApplicationObject ( PageCommand );

        //
        // update the last page object.
        //
        this.Session.LastPage = clientDataObject;

        // 
        // Update the clinical ResultData session object.
        // 
        this.saveSessionObjects ( );

        this.LogDebug ( "Page Message: " + clientDataObject.Message );

        this.LogMethodEnd ( "getPageObject" );

        // 
        // return the Client ResultData object.
        // 
        return clientDataObject;

      }
      catch ( Exception Ex )
      {
        this.LogException ( Ex );
      }

      this.LogMethodEnd ( "getPageObject" );
      // 
      // return the previous client ResultData object.
      // 
      return this.Session.LastPage;

    }//END getPageObject method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Select Demonstration Registration  methods.

    // ==================================================================================
    /// <summary>
    /// This method calls the patient recorded observation module. 
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object</param>
    /// <returns>Evado.Model.UniForm.AppData</returns>
    // ----------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData callDemonstrationRegistration (
       Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "callDemonstrationRegistration" );
      this.LogValue ( "Page Command: " + PageCommand.getAsString ( false, true ) );

      //
      // set the demo user's organisation.
      //

      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.UniForm.AppData clientDataObject = new Model.UniForm.AppData ( );
      EvCustomers bll_Customer = new EvCustomers ( this.ClassParameters );
      EvOrganisations bll_organisations = new EvOrganisations ( this.ClassParameters );
      String description = String.Empty;

      clientDataObject.Id = Guid.NewGuid ( );
      clientDataObject.Page.Id = clientDataObject.Id;
      clientDataObject.Page.Title = "Demonstration User Registration Page";

      Guid customerGuid = PageCommand.GetGuid ( );

      this.LogDebug ( "Customer Guid {0}.", customerGuid );

      clientDataObject.Message = "Demonstration Registration page.";

      var pageGroup = clientDataObject.Page.AddGroup ( "", Model.UniForm.EditAccess.Disabled );

      description = PageCommand.getAsString ( false, true );

      //
      // get the customer object.
      //
      if ( customerGuid != Guid.Empty
        && this.Session.Customer.Guid != customerGuid )
      {
        this.LogDebug ( "Loading new customer object." );

        this.Session.Customer = bll_Customer.getItem ( customerGuid );
        this._Settings.CustomerGuid = this.Session.Customer.Guid;
      }

      this.LogDebug ( "{0} - {1}.", this.Session.Customer.CustomerNo, this.Session.Customer.Name );

      description += "\r\n Customer No: " + this.Session.Customer.CustomerNo
        + ", Name: " + this.Session.Customer.Name;

      //
      // get the organisation object.
      //  
      this.Session.AdminOrganisation = bll_organisations.getItem ( EuDemoUserRegistration.CONST_DEMO_ORGANISATION );

      this.LogDebug ( "{0} - {1}.", this.Session.AdminOrganisation.OrgId, this.Session.AdminOrganisation.Name );

      description += "\r\n Organisation OrgId: " + this.Session.AdminOrganisation.OrgId
        + ", Name: " + this.Session.AdminOrganisation.Name;

      this.LogValue ( "Group Description:\r\n " + description );
      pageGroup.Description = description;
      //
      // initialise patient adapter the service object.
      //
      EuDemoUserRegistration demonstrationUserRegistration = new EuDemoUserRegistration (
        this._ApplicationObjects,
        this.ServiceUserProfile,
        this.Session,
        this.UniForm_BinaryFilePath,
        this.ClassParameters );

      //
      // Call the get page method to generate the next page.
      //
      clientDataObject = demonstrationUserRegistration.getDataObject ( PageCommand );

      this.LogAdapter ( demonstrationUserRegistration.Log );

      //
      // Update the application and session objects.
      //
      this._ApplicationObjects = demonstrationUserRegistration.ApplicationObjects;
      this.Session = demonstrationUserRegistration.Session;

      //
      // update the last page object.
      //
      this.Session.LastPage = clientDataObject;

      // 
      // Update the clinical ResultData session object.
      // 
      this.saveSessionObjects ( );

      //
      // Return the generated client object
      //
      this.LogMethodEnd ( "callDemonstrationRegistration" );
      return clientDataObject;

    }//END callPatientAdapter method. 


    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Select Application Objects methods.

    // ==================================================================================
    /// <summary>
    /// This method selects the application object to be called.
    /// 
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object</param>
    /// <returns>Evado.Model.UniForm.AppData</returns>
    /// <remarks>
    /// This method selects the application class to be called based on the groupCommand.object value.
    /// The default selection is to generate the applications home page definition object.
    /// Then returns the page definition object to the calling method.
    /// </remarks>
    // ----------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData getApplicationObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getApplicationObject" );
      this.LogValue ( "PageCommand: " + PageCommand.getAsString ( false, false ) );
      this.LogDebug ( "Settings.UserProfile.RoleId: " + this.ClassParameters.UserProfile.RoleId );
      this.LogDebug ( "Settings.UserProfile.OrgId: " + this.ClassParameters.UserProfile.OrgId );
      //this.LogDebug ( "EvStaticSetting.ConnectionStringKey: " + Bll.EvStaticSetting.ConnectionStringKey );
      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.UniForm.AppData clientDataObject = new Model.UniForm.AppData ( );
      this.ErrorMessage = String.Empty;

      EuFormRecords formRecords = new EuFormRecords (
              this._ApplicationObjects,
              this.ServiceUserProfile,
              this.Session,
              this.UniForm_BinaryFilePath,
              this.UniForm_BinaryServiceUrl,
              this.ClassParameters );

      formRecords.unLockRecord ( );

      formRecords.LoggingLevel = this.LoggingLevel;

      // 
      // Save the application parameters to global objects.
      // 
      this.GlobalObjectList [ Evado.Model.UniForm.EuStatics.GLOBAL_ECLINICAL_OBJECT ] = this._ApplicationObjects;

      // 
      // Get the application object enumeration value.
      // 
      EuAdapterClasses adapterClass =
        Evado.Model.UniForm.EuStatics.Enumerations.parseEnumValue<EuAdapterClasses> ( PageCommand.Object );

      this.LogDebug ( "adapterClass: " + adapterClass );

      //
      // Select the class object to be displayed.
      //
      switch ( adapterClass )
      {
        case EuAdapterClasses.Application_Properties:
          {
            this.LogDebug ( " APPLICATION PROFILE CLASS SELECTED." );

            //
            // Log command and exit for illegal access attempty.
            //
            if ( PageCommand.Type == Evado.Model.UniForm.CommandTypes.Anonymous_Command )
            {
              return this.IllegalAnonymousAccessAttempt ( adapterClass );
            }

            // 
            // Initialise the methods variables and objects.
            // 
            EdPlatformSettings applicationProfiles = new EdPlatformSettings (
              this._ApplicationObjects,
              this.ServiceUserProfile,
              this.Session,
              this.UniForm_BinaryFilePath,
              this.ClassParameters );

            applicationProfiles.LoggingLevel = this.LoggingLevel;

            clientDataObject = applicationProfiles.getClientDataObject ( PageCommand );
            this.ErrorMessage = applicationProfiles.ErrorMessage;
            this.LogAdapter ( applicationProfiles.Log );

            break;

          }
        case EuAdapterClasses.Customers:
          {
            this.LogDebug ( " CUSTOMERS CLASS SELECTED." );

            //
            // Log command and exit for illegal access attempty.
            //
            if ( PageCommand.Type == Evado.Model.UniForm.CommandTypes.Anonymous_Command )
            {
              return this.IllegalAnonymousAccessAttempt ( adapterClass );
            }

            // 
            // Initialise the methods variables and objects.
            // 
            EuCustomers customers = new EuCustomers (
              this._ApplicationObjects,
              this.ServiceUserProfile,
              this.Session,
              this.UniForm_BinaryFilePath,
              this.ClassParameters );

            customers.LoggingLevel = this.LoggingLevel;

            clientDataObject = customers.getClientDataObject ( PageCommand );
            this.ErrorMessage = customers.ErrorMessage;
            this.LogAdapter ( customers.Log );

            break;

          }
        case EuAdapterClasses.Events:
          {
            this.LogDebug ( " APPLICATION EVENTS CLASS SELECTED." );

            //
            // Log command and exit for illegal access attempty.
            //
            if ( PageCommand.Type == Evado.Model.UniForm.CommandTypes.Anonymous_Command )
            {
              return this.IllegalAnonymousAccessAttempt ( adapterClass );
            }

            // 
            // Initialise the methods variables and objects.
            // 
            EuApplicationEvents applicationProfiles = new EuApplicationEvents (
              this._ApplicationObjects,
              this.ServiceUserProfile,
              this.Session,
              this.UniForm_BinaryFilePath,
              this.ClassParameters );

            applicationProfiles.LoggingLevel = this.LoggingLevel;

            clientDataObject = applicationProfiles.getClientDataObject ( PageCommand );
            this.ErrorMessage = applicationProfiles.ErrorMessage;
            this.LogAdapter ( applicationProfiles.Log );

            break;
          }
        case EuAdapterClasses.Email_Templates:
          {
            this.LogDebug ( " APPLICATION EMAIL TEMPLATES CLASS SELECTED." );

            //
            // Log command and exit for illegal access attempty.
            //
            if ( PageCommand.Type == Evado.Model.UniForm.CommandTypes.Anonymous_Command )
            {
              return this.IllegalAnonymousAccessAttempt ( adapterClass );
            }

            // 
            // Initialise the methods variables and objects.
            // 
            EuStaticContentTemplates emailTemplates = new EuStaticContentTemplates (
              this._ApplicationObjects,
              this.ServiceUserProfile,
              this.Session,
              this.UniForm_BinaryFilePath );

            emailTemplates.LoggingLevel = this.LoggingLevel;

            clientDataObject = emailTemplates.getDataObject ( PageCommand );
            this.ErrorMessage = emailTemplates.ErrorMessage;

            this.LogAdapter ( emailTemplates.Log );

            break;
          }
        case EuAdapterClasses.Organisations:
          {
            this.LogDebug ( " ORGANISATION CLASS SELECTED." );

            //
            // Log command and exit for illegal access attempty.
            //
            if ( PageCommand.Type == Evado.Model.UniForm.CommandTypes.Anonymous_Command )
            {
              return this.IllegalAnonymousAccessAttempt ( adapterClass );
            }

            // 
            // Initialise the methods variables and objects.
            // 
            EuOrganisations organisations = new EuOrganisations ( this._ApplicationObjects,
              this.ServiceUserProfile,
              this.Session,
              this.UniForm_BinaryFilePath,
              this.ClassParameters );

            organisations.LoggingLevel = this.LoggingLevel;

            clientDataObject = organisations.getClientDataObject ( PageCommand );
            this.ErrorMessage = organisations.ErrorMessage;
            this.LogAdapter ( organisations.Log );

            break;
          }
        case EuAdapterClasses.Users:
          {
            this.LogDebug ( " USERS CLASS SELECTED." );

            //
            // Log command and exit for illegal access attempty.
            //
            if ( PageCommand.Type == Evado.Model.UniForm.CommandTypes.Anonymous_Command )
            {
              return this.IllegalAnonymousAccessAttempt ( adapterClass );
            }

            // 
            // Initialise the methods variables and objects.
            // 
            EuUserProfiles userProfiles = new EuUserProfiles ( this._ApplicationObjects,
              this.ServiceUserProfile,
              this.Session,
              this.UniForm_BinaryFilePath,
              this.ClassParameters );

            userProfiles.LoggingLevel = this.LoggingLevel;

            clientDataObject = userProfiles.getDataObject ( PageCommand );
            this.ErrorMessage = userProfiles.ErrorMessage;
            this.LogAdapter ( userProfiles.Log );

            break;

          }
        case EuAdapterClasses.Menu:
          {
            this.LogDebug ( "MENU CLASS SELECTED." );

            //
            // Log command and exit for illegal access attempty.
            //
            if ( PageCommand.Type == Evado.Model.UniForm.CommandTypes.Anonymous_Command )
            {
              return this.IllegalAnonymousAccessAttempt ( adapterClass );
            }

            // 
            // Initialise the methods variables and objects.
            // 
            EuMenus menus = new EuMenus (
              this._ApplicationObjects,
              this.ServiceUserProfile,
              this.Session,
              this.UniForm_BinaryFilePath,
              this.ClassParameters );

            menus.LoggingLevel = this.LoggingLevel;

            clientDataObject = menus.getClientDataObject ( PageCommand );
            // 
            // Save the application parameters to global objects.
            // 
            this.GlobalObjectList [ Evado.Model.UniForm.EuStatics.GLOBAL_ECLINICAL_OBJECT ] = this._ApplicationObjects;

            this.ErrorMessage = menus.ErrorMessage;
            this.LogAdapter ( menus.Log );

            break;
          }

        case EuAdapterClasses.Projects:
          {
            this.LogDebug ( "PROJECT CLASS SELECTED." );

            //
            // Log command and exit for illegal access attempty.
            //
            if ( PageCommand.Type == Evado.Model.UniForm.CommandTypes.Anonymous_Command )
            {
              return this.IllegalAnonymousAccessAttempt ( adapterClass );
            }


            // 
            // Initialise the methods variables and objects.
            // 
            EdApplicationSettings studies = new EdApplicationSettings (
              this._ApplicationObjects,
              this.ServiceUserProfile,
              this.Session,
              this.UniForm_BinaryFilePath,
              this.UniForm_BinaryServiceUrl,
              this.ClassParameters );

            studies.LoggingLevel = this.LoggingLevel;

            clientDataObject = studies.getDataObject ( PageCommand );
            this.ErrorMessage = studies.ErrorMessage;
            this.LogAdapter ( studies.Log );

            //
            // If the project has been saved then refresh the selection list by setting it empty.
            //
            if ( PageCommand.Method == Model.UniForm.ApplicationMethods.Save_Object )
            {
              this.Session.TrialList = new List<Model.Digital.EdApplication> ( );
            }


            break;
          }

        case EuAdapterClasses.Binary_File:
          {
            this.LogDebug ( "PROJECT CLASS SELECTED." );

            //
            // Log command and exit for illegal access attempty.
            //
            if ( PageCommand.Type == Evado.Model.UniForm.CommandTypes.Anonymous_Command )
            {
              return this.IllegalAnonymousAccessAttempt ( adapterClass );
            }

            // 
            // Initialise the methods variables and objects.
            // 
            EuBinaryFiles binaryFiles = new EuBinaryFiles (
              this._ApplicationObjects,
              this.ServiceUserProfile,
              this.Session,
              this.UniForm_BinaryFilePath,
              this.UniForm_BinaryServiceUrl,
              this._FileRepositoryPath,
              this.ClassParameters );

            clientDataObject = binaryFiles.getDataObject ( PageCommand );
            this.ErrorMessage = binaryFiles.ErrorMessage;
            this.LogAdapter ( binaryFiles.Log );

            this.LogDebug ( "ADAPTER: Page ID {0}, title {1}, Groups {2}.",
              clientDataObject.Id,
              clientDataObject.Title,
              clientDataObject.Page.GroupList.Count );
            break;
          }

        case EuAdapterClasses.Alert:
          {
            this.LogDebug ( "ALERT CLASS SELECTED." );

            //
            // Log command and exit for illegal access attempty.
            //
            if ( PageCommand.Type == Evado.Model.UniForm.CommandTypes.Anonymous_Command )
            {
              return this.IllegalAnonymousAccessAttempt ( adapterClass );
            }

            // 
            // Initialise the methods variables and objects.
            // 
            EuAlerts alerts = new EuAlerts (
              this._ApplicationObjects,
              this.ServiceUserProfile,
              this.Session,
              this.UniForm_BinaryFilePath,
              this.ClassParameters );

            alerts.LoggingLevel = this.LoggingLevel;

            clientDataObject = alerts.getClientDataObject ( PageCommand );
            this.ErrorMessage = alerts.ErrorMessage;
            LogAdapter ( alerts.Log );

            break;
          }
        case EuAdapterClasses.Project_Forms:
          {
            this.LogDebug ( "PROJECT FORMS CLASS SELECTED." );

            //
            // Log command and exit for illegal access attempty.
            //
            if ( PageCommand.Type == Evado.Model.UniForm.CommandTypes.Anonymous_Command )
            {
              return this.IllegalAnonymousAccessAttempt ( adapterClass );
            }

            // 
            // Initialise the methods variables and objects.
            // 
            EuForms forms = new EuForms ( this._ApplicationObjects,
              this.ServiceUserProfile,
              this.Session,
              this.UniForm_BinaryFilePath,
              this.ClassParameters );

            forms.LoggingLevel = this.LoggingLevel;

            clientDataObject = forms.getDataObject ( PageCommand );
            this.ErrorMessage = forms.ErrorMessage;
            LogAdapter ( forms.Log );

            break;
          }

        case EuAdapterClasses.Project_Form_Fields:
          {
            this.LogDebug ( "PROJECT FORMS CLASS SELECTED." );

            //
            // Log command and exit for illegal access attempty.
            //
            if ( PageCommand.Type == Evado.Model.UniForm.CommandTypes.Anonymous_Command )
            {
              return this.IllegalAnonymousAccessAttempt ( adapterClass );
            }

            // 
            // Initialise the methods variables and objects.
            // 
            EuFormFields formFields = new EuFormFields ( this._ApplicationObjects,
              this.ServiceUserProfile,
              this.Session,
              this.UniForm_BinaryFilePath,
              this.ClassParameters );

            formFields.LoggingLevel = this.LoggingLevel;

            clientDataObject = formFields.getDataObject ( PageCommand );
            this.ErrorMessage = formFields.ErrorMessage;
            LogAdapter ( formFields.Log );

            break;
          }

        case EuAdapterClasses.Activities:
          {
            this.LogDebug ( "ACTIVITIES CLASS SELECTED." );

            //
            // Log command and exit for illegal access attempty.
            //
            if ( PageCommand.Type == Evado.Model.UniForm.CommandTypes.Anonymous_Command )
            {
              return this.IllegalAnonymousAccessAttempt ( adapterClass );
            }

            // 
            // Initialise the methods variables and objects.
            // 
            EuActivities actvities = new EuActivities ( this._ApplicationObjects,
              this.ServiceUserProfile,
              this.Session,
              this.UniForm_BinaryFilePath,
              this.ClassParameters );

            actvities.LoggingLevel = this.LoggingLevel;

            clientDataObject = actvities.getDataObject ( PageCommand );
            this.ErrorMessage = actvities.ErrorMessage;
            LogAdapter ( actvities.Log );

            break;
          }
        case EuAdapterClasses.Schedules:
          {
            this.LogDebug ( "SCHEDULES CLASS SELECTED." );

            //
            // Log command and exit for illegal access attempty.
            //
            if ( PageCommand.Type == Evado.Model.UniForm.CommandTypes.Anonymous_Command )
            {
              return this.IllegalAnonymousAccessAttempt ( adapterClass );
            }

            // 
            // Initialise the methods variables and objects.
            // 
            EuSchedules schedules = new EuSchedules ( this._ApplicationObjects,
              this.ServiceUserProfile,
              this.Session,
              this.UniForm_BinaryFilePath,
              this.ClassParameters );

            schedules.LoggingLevel = this.LoggingLevel;

            clientDataObject = schedules.getDataObject ( PageCommand );
            this.ErrorMessage = schedules.ErrorMessage;
            LogAdapter ( schedules.Log );

            break;
          }
        case EuAdapterClasses.Milestones:
          {
            this.LogDebug ( "MILESTONES CLASS SELECTED." );

            //
            // Log command and exit for illegal access attempty.
            //
            if ( PageCommand.Type == Evado.Model.UniForm.CommandTypes.Anonymous_Command )
            {
              return this.IllegalAnonymousAccessAttempt ( adapterClass );
            }

            // 
            // Initialise the methods variables and objects.
            // 
            EuMilestones milestones = new EuMilestones ( this._ApplicationObjects,
              this.ServiceUserProfile,
              this.Session,
              this.UniForm_BinaryFilePath,
              this.ClassParameters );

            milestones.LoggingLevel = this.LoggingLevel;

            clientDataObject = milestones.getDataObject ( PageCommand );
            this.ErrorMessage = milestones.ErrorMessage;
            LogAdapter ( milestones.Log );

            break;
          }
        case EuAdapterClasses.ReportTemplates:
          {
            this.LogDebug ( "REPORT TEMPLATESS CLASS SELECTED." );

            //
            // Log command and exit for illegal access attempty.
            //
            if ( PageCommand.Type == Evado.Model.UniForm.CommandTypes.Anonymous_Command )
            {
              return this.IllegalAnonymousAccessAttempt ( adapterClass );
            }

            // 
            // Initialise the methods variables and objects.
            // 
            EuReportTemplates reportTemplates = new EuReportTemplates ( this._ApplicationObjects,
              this.ServiceUserProfile,
              this.Session,
              this.UniForm_BinaryFilePath,
              this.UniForm_BinaryServiceUrl,
              this.ClassParameters );

            reportTemplates.LoggingLevel = this.LoggingLevel;

            clientDataObject = reportTemplates.getDataObject ( PageCommand );
            this.ErrorMessage = reportTemplates.ErrorMessage;
            LogAdapter ( reportTemplates.Log );

            break;
          }
        case EuAdapterClasses.Reports:
          {
            this.LogDebug ( "REPORTS CLASS SELECTED." );

            //
            // Log command and exit for illegal access attempty.
            //
            if ( PageCommand.Type == Evado.Model.UniForm.CommandTypes.Anonymous_Command )
            {
              return this.IllegalAnonymousAccessAttempt ( adapterClass );
            }

            // 
            // Initialise the methods variables and objects.
            // 
            EuReports reports = new EuReports ( this._ApplicationObjects,
              this.ServiceUserProfile,
              this.Session,
              this.UniForm_BinaryFilePath,
              this.UniForm_BinaryServiceUrl,
              this.ClassParameters );

            reports.LoggingLevel = this.LoggingLevel;

            clientDataObject = reports.getDataObject ( PageCommand );
            this.ErrorMessage = reports.ErrorMessage;
            LogAdapter ( reports.Log );

            break;
          }
        case EuAdapterClasses.Analysis:
          {
            this.LogDebug ( "ANALYSIS CLASS SELECTED." );

            //
            // Log command and exit for illegal access attempty.
            //
            if ( PageCommand.Type == Evado.Model.UniForm.CommandTypes.Anonymous_Command )
            {
              return this.IllegalAnonymousAccessAttempt ( adapterClass );
            }

            // 
            // Initialise the methods variables and objects.
            // 
            EuAnalysis analysis = new EuAnalysis ( this._ApplicationObjects,
              this.ServiceUserProfile,
              this.Session,
              this.UniForm_BinaryFilePath,
              this.UniForm_BinaryServiceUrl,
              this.ClassParameters );

            analysis.LoggingLevel = this.LoggingLevel;

            clientDataObject = analysis.getDataObject ( PageCommand );
            this.ErrorMessage = analysis.ErrorMessage;
            LogAdapter ( analysis.Log );

            break;
          }

        case EuAdapterClasses.Scheduled_Record:
          {
            this.LogDebug ( "PROJECT RECORDS CLASS SELECTED." );

            //
            // Log command and exit for illegal access attempty.
            //
            if ( PageCommand.Type == Evado.Model.UniForm.CommandTypes.Anonymous_Command )
            {
              return this.IllegalAnonymousAccessAttempt ( adapterClass );
            }

            //
            // IF the exit command is pointing to subject milestone object to create a new instance
            // set the method to get and the subject milestone is already created.
            //
            if ( this.ExitCommand.Object == EuAdapterClasses.Subject_Milestone.ToString ( )
              && this.ExitCommand.Method == Model.UniForm.ApplicationMethods.Create_Object )
            {
              this.ExitCommand.Method = Model.UniForm.ApplicationMethods.Get_Object;
            }

            // 
            // Create the common record object.
            // 
            formRecords.LoggingLevel = this.LoggingLevel;

            clientDataObject = formRecords.getDataObject ( PageCommand );
            this.ErrorMessage = formRecords.ErrorMessage;
            this.LogAdapter ( formRecords.Log );

            break;
          }
        case EuAdapterClasses.Home_Page:
        default:
          {
            this.LogDebug ( "HOME PAGE SELECTED." );

            // 
            // Return the instance to the list.
            // 
            return this.generateHomePage ( PageCommand );
          }
      }//END Switch

      // 
      // if an error then return the previous client ResultData object.
      // 
      if ( clientDataObject == null )
      {
        this.LogEvent ( "RETURNED: null client data object. " );
        clientDataObject = this.Session.LastPage;
      }

      if ( clientDataObject.Message != this.ErrorMessage )
      {
        clientDataObject.Message = this.ErrorMessage;
        this.LogEvent ( "ERROR MESSAGE RETURNED: " + clientDataObject.Message );
      }

      clientDataObject.Page.Exit = this.ExitCommand;

      this.LogDebug ( "Page Exit command: " + clientDataObject.Page.Exit.getAsString ( false, true ) );

      this.LogDebug ( "clientDataObject.Title: " + clientDataObject.Title );
      this.LogDebug ( "Page.GroupList.Count: " + clientDataObject.Page.GroupList.Count );

      this.LogMethodEnd ( "getApplicationObject" );

      return clientDataObject;
    }//END getApplicationObject method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Private methods.

    // ==================================================================================
    /// <summary>
    /// This method loads the global application objects.
    /// </summary>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Load the application object out of global object hasheList.
    /// 
    /// 2. If the object exist then exit.
    /// 
    /// 3. Loads Load the application object and update the hash list object.
    /// </remarks>
    // ----------------------------------------------------------------------------------
    private void loadGlobalObjects ( )
    {
      this.LogInitMethod ( "loadGlobalObjects" );
      this.LogInitValue ( "GlobalObjects.Count: " + this.GlobalObjectList.Count );
      this.LogInitValue ( "Settings.LoggingLevel: " + this.ClassParameters.LoggingLevel );

      this.LogInitValue ( "Default ConnectionSettingKey: " + this._ConnectionSettingKey );
      // 
      // Get the connection string key.
      // 
      if ( ConfigurationManager.AppSettings [ EuAdapter.CONFIG_CONNECTION_SETTING_KEY ] != null )
      {
        this._ConnectionSettingKey = ConfigurationManager.AppSettings [ EuAdapter.CONFIG_CONNECTION_SETTING_KEY ];

      }
      this.LogInitValue ( "_ConnectionSettingKey: '" + this._ConnectionSettingKey + "'" );

      Evado.Bll.EvStaticSetting.ConnectionStringKey = this._ConnectionSettingKey;

      this.LogInitValue ( "EvStaticSetting.ConnectionStringKey: '" + Evado.Bll.EvStaticSetting.ConnectionStringKey + "'" );

      // 
      // Get the respository file path.
      // 
      if ( ConfigurationManager.AppSettings [ Evado.Model.Digital.EvcStatics.CONFIG_RESPOSITORY_FILE_PATH_KEY ] != null )
      {
        this._FileRepositoryPath = ConfigurationManager.AppSettings [ Evado.Model.Digital.EvcStatics.CONFIG_RESPOSITORY_FILE_PATH_KEY ];
      }
      this.LogInitValue ( "FileRepositoryPath: '" + this._FileRepositoryPath + "'" );
      /*
      // 
      // Get the respository file path.
      // 
      this._PlatformId = "ADMIN";
      if ( ConfigurationManager.AppSettings [ Evado.Model.Digital.EvcStatics.CONFIG_PATFORM_ID_KEY ] != null )
      {
        this._PlatformId = ConfigurationManager.AppSettings [ Evado.Model.Digital.EvcStatics.CONFIG_PATFORM_ID_KEY ];
      }
      this.LogInitValue ( "PlatformId: '" + this._PlatformId + "'" );
      this.Settings.PlatformId = this._PlatformId;
      this._ApplicationObjects.PlatformId = this._PlatformId;
      */

      // 
      // load the global object.
      // 
      if ( this.GlobalObjectList.ContainsKey ( Evado.Model.UniForm.EuStatics.GLOBAL_ECLINICAL_OBJECT ) == true )
      {
        this._ApplicationObjects =
          ( EuApplicationObjects ) this.GlobalObjectList [ Evado.Model.UniForm.EuStatics.GLOBAL_ECLINICAL_OBJECT ];
      }

      //  
      // Load the paremeters from the web.config if not already loaded.
      // 
      if ( this._ApplicationObjects.PlatformSettings.Guid != Guid.Empty )
      {
        this.LogInitValue ( "APPLICATION OBJECT IS LOADED" );

        return;
      }

      // 
      // Update the application path.
      // 
      this._ApplicationObjects.ApplicationPath = this.ApplicationPath;

      this.LogInitValue ( "LOADING THE APPLICATION OBJECT VALUES" );
      //
      // The object is empty so load the application parameter values.
      //
      int loggingLevel = this._ApplicationObjects.LoggingLevel;
      this._ApplicationObjects.LoggingLevel = 5;

      this._ApplicationObjects.loadGlobalParameters ( );

      this.LogInit ( this._ApplicationObjects.Log );
      this.LogInitValue ( "Version: " + this._ApplicationObjects.PlatformSettings.Version );

      // 
      // Save the application parameters to global objects.
      // 
      this.GlobalObjectList [ Evado.Model.UniForm.EuStatics.GLOBAL_ECLINICAL_OBJECT ] = this._ApplicationObjects;

      this.LogInitValue ( "GlobalObjects: " + this.GlobalObjectList.Count );

      this.LogInitValue ( "GLOBAL APPLICATION OBJECT VALUES LOADED" );
      this._ApplicationObjects.LoggingLevel = loggingLevel;

    }//END loadGlobalApplicationObjects method

    // =====================================================================================
    /// <summary>
    /// This method deletes the old temporary files and must be called prior to loading
    /// the application objects for the first time after application start.
    /// 
    /// </summary>
    //  ----------------------------------------------------------------------------------
    protected void DeleteOldTemporaryFiles ( )
    {
      this.LogMethod ( "DeleteOldTemporaryFiles method " );
      try
      {
        //
        // initialise the methods variables and objects.
        //
        String stIgnoreFileList = String.Empty;

        //
        // exit if web site is defined.
        //
        if ( this._ApplicationObjects.PlatformId != String.Empty )
        {
          this.LogInitValue ( "The web site identifier exists to exit." );
          return;
        }

        // 
        // Test that an event source is exists if so read it in as set the local
        // event source.
        // 
        if ( ConfigurationManager.AppSettings [ Evado.Model.Digital.EvcStatics.CONFIG_IGNORE_FILES_LIST_KEY ] != null
          && ConfigurationManager.AppSettings [ Evado.Model.Digital.EvcStatics.CONFIG_IGNORE_FILES_LIST_KEY ] != String.Empty )
        {
          stIgnoreFileList = ConfigurationManager.AppSettings [ Evado.Model.Digital.EvcStatics.CONFIG_IGNORE_FILES_LIST_KEY ];
        }

        if ( stIgnoreFileList == String.Empty )
        {
          return;
        }

        stIgnoreFileList = stIgnoreFileList.ToLower ( );
        stIgnoreFileList = stIgnoreFileList.Replace ( "\r", ";" );
        stIgnoreFileList = stIgnoreFileList.Replace ( "\n", ";" );
        stIgnoreFileList = stIgnoreFileList.Replace ( ",", ";" );
        stIgnoreFileList = stIgnoreFileList.Replace ( ";;", ";" );
        String [ ] ignoreFileList = stIgnoreFileList.Split ( ';' );

        this.LogInitValue ( "ignore list: " + stIgnoreFileList );

        int deletedFileCount = Evado.Model.Digital.EvcStatics.Files.deleteOldFiles ( this.UniForm_BinaryFilePath, 2, ignoreFileList );

        //this.LogInitValue ( Evado.Model.Digital.EvcStatics.Files.DebugLog );
        this.LogInitValue ( "deletedFile count: " + deletedFileCount );
      }
      catch ( Exception Ex )
      {
        this.LogException ( Ex );
      }

    }//END DeleteOldTemporaryFiles method

    // ==================================================================================
    /// <summary>
    /// This method loads the global application objects.
    /// </summary>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Load the application object out of global object hasheList.
    /// 
    /// 2. If the object exist then exit.
    /// 
    /// 3. Loads Load the application object and update the hash list object.
    /// </remarks>
    // ----------------------------------------------------------------------------------
    private void loadSessionObjects ( )
    {
      this.LogInitMethod ( "loadSessionObjects" );

      // 
      // load eclinical session object.
      //
      if ( this.ServiceUserProfile.UserId == String.Empty )
      {
        //this.LogInitValue ( "user key empty." );
        this._SessionObjectKey = String.Empty;
        return;
      }

      this._SessionObjectKey = this.ServiceUserProfile.UserId + Evado.Model.Digital.EvcStatics.SESSION_CLINICAL_OBJECT;
      this._SessionObjectKey = this._SessionObjectKey.Replace ( ".", "_" );
      this._SessionObjectKey = this._SessionObjectKey.ToUpper ( );

      //this.LogInitValue ( "SessionObjectKey: " + this._SessionObjectKey );

      if ( this.GlobalObjectList [ this._SessionObjectKey ] != null )
      {
        this.Session = ( EuSession ) this.GlobalObjectList [ this._SessionObjectKey ];

        //this.LogInitValue ( "Session object loaded." );
      }

      this._ClientObjectKey = this.ServiceUserProfile.UserId + Evado.Model.Digital.EvcStatics.SESSION_CLIENT_DATA_OBJECT;
      this._ClientObjectKey = this._ClientObjectKey.Replace ( ".", "_" );
      this._ClientObjectKey = this._ClientObjectKey.ToUpper ( );

      //this.LogInitValue ( "ClientObjectKey: " + this._ClientObjectKey );

      if ( this.GlobalObjectList [ this._ClientObjectKey ] != null )
      {
        this.ClientDataObject = ( Evado.Model.UniForm.AppData ) this.GlobalObjectList [ this._ClientObjectKey ];

        //this.LogInitValue ( "Last client data object loaded." );
      }


      // 
      // if the last page is empty, meaning that the home page has been opened for the first time,
      // retrieve the ADS enabled property, this lets developers turn off the ADS access when it 
      // is not available.
      // 
      if ( this.Session.LastPage.Id == Guid.Empty )
      {
        if ( ConfigurationManager.AppSettings [ EuAdapter.CONFIG_ADS_ENABLED_ADDRESS_KEY ] != null )
        {
          String value = ConfigurationManager.AppSettings [ EuAdapter.CONFIG_ADS_ENABLED_ADDRESS_KEY ];

          this.Session.AdsEnabled = EvStatics.getBool ( value );
        }
      }
      this.LogInitValue ( "Session Clinical objects: " );
      this.LogInitValue ( "-AdsEnabled: " + this.Session.AdsEnabled );
      /*
      this.LogInitValue ( "-eClinical User Name: " + this.Session.UserProfile.CommonName );
      this.LogInitValue ( "-Current ProjectId: " + this.Session.Project.ProjectId );
      this.LogInitValue ( "-ProjectSiteList count: " + this.Session.ProjectSiteOptionList.Count );
      this.LogInitValue ( "-Current Site: " + this.Session.ProjectOrganisation.LinkText );
      this.LogInitValue ( "-Subject: " + this.Session.Subject.SubjectId );
      this.LogInitValue ( "-Record: " + this.Session.Record.RecordId );
      this.LogInitValue ( "-Common Record: " + this.Session.CommonRecord.RecordId );
      this.LogInitValue ( "-RecordType: " + this.Session.FormRecordType );
      this.LogInitValue ( "-RecordQuerySelection: " + this.Session.RecordQuerySelection );
      this.LogInitValue ( "-Last Page Title: " + this.ClientDataObject.Page.Title );
      */
      this.LogInitValue ( "SESSION OBJECTS LOADED" );

    }//END loadSessionObjects method

    // ==================================================================================
    /// <summary>
    /// This method Saves the clinical objects to user session object.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public void saveSessionObjects ( )
    {
      this.LogMethod ( "saveSessionObjects" );
      //this.LogDebugValue ( "SessionObjectKey: " + this._SessionObjectKey );
      //this.LogDebugValue ( "ClientObjectKey: " + this._ClientObjectKey );

      if ( this._SessionObjectKey == String.Empty )
      {
        return;
      }

      // 
      // Save the session ResultData to global object hashtable.
      //
      this.GlobalObjectList [ this._SessionObjectKey ] = this.Session;

      //
      // Save the last generated client data object
      //
      //this.LogDebugValue ( "ClientObject.Page.Title: " + this.ClientDataObject.Page.Title );
      //this.LogDebugValue ( "ClientObject.id: " + this.ClientDataObject.Id );


      this.GlobalObjectList [ this._ClientObjectKey ] = this.ClientDataObject;

      string Date_Key = this._SessionObjectKey.Replace (
         Evado.Model.Digital.EvcStatics.SESSION_CLINICAL_OBJECT,
        Evado.Model.UniForm.EuStatics.GLOBAL_DATE_STAMP );

      this.GlobalObjectList [ Date_Key ] = DateTime.Now.ToString ( "dd MMM yyyy HH:mm" );

      //
      // Save the application session for the next user generated groupCommand.
      //
      this.GlobalObjectList [ Evado.Model.UniForm.EuStatics.GLOBAL_ECLINICAL_OBJECT ] = this._ApplicationObjects;

      this.LogValue ( "GlobalObjects.Count: " + this.GlobalObjectList.Count );

      this.LogMethodEnd ( "saveSessionObject" );
    }//END saveSessionObjects method

    // ==================================================================================
    /// <summary>
    /// This method logs an illegal access attempt by an anonymous user.
    /// </summary>
    /// <param name="applicationObject">Adapter.ApplicationObjects enumerated list</param>
    /// <returns></returns>
    // ----------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData IllegalAnonymousAccessAttempt (
      EuAdapterClasses applicationObject )
    {
      this.ErrorMessage = EvLabels.Security_Illegal_Anonymoous_Access_Message;
      //
      // log an user access event.
      //
      this.LogError ( EvEventCodes.User_Access_Error,
        "ERROR: Anonymous command attempted to access " + applicationObject );

      return null;
    }

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class status methods.

    // ==================================================================================
    /// <summary>
    /// This property contains the Home page default command
    /// </summary>
    // ----------------------------------------------------------------------------------
    public static Evado.Model.UniForm.Command HomePageCommand
    {
      get
      {
        Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command (
          EvLabels.Default_Home_Page_Command_Title,
          EuAdapter.APPLICATION_ID,
           Evado.Model.Digital.EvcStatics.CONST_DEFAULT,
          Evado.Model.UniForm.ApplicationMethods.Get_Object );

        return pageCommand;
      }
    }//END HomePageCommand property.

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region logging methods

    //  ===========================================================================
    /// <summary>
    /// This class creates the Evado EvEvent Source. This requires administrator privileges
    /// because it needs to write to the registry have.
    /// </summary>
    /// <param name="ClassMethodAccessed">String: a class method accessed string</param>
    /// <param name="User">Evado.Model.Digital.EvUserProfile: A user's profile</param>
    /// <returns>Boolean: true, If the event source was created successfully.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Set Site and EventLogSource2 to default value, if they are not empty.
    /// 
    /// 2. Update the application event value. 
    /// 
    /// 3. Adding items to application event table.
    /// 
    /// 4. Return false, if the adding runs fail
    /// 
    /// 5. Else, return ture. 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public void LogIllegalAccess (
     String ClassMethodAccessed,
      String RoleId )
    {
      // 
      // Initialise the method variables
      // 
      System.Text.StringBuilder stEvent = new StringBuilder ( );

      stEvent.AppendFormat (
         "Illegal access attempt UserId: {0} name {1} opened {2} method at {3} ",
         this.ServiceUserProfile.UserId,
         this.ServiceUserProfile.UserId,
         ClassMethodAccessed,
         DateTime.Now.ToString ( "dd MMM yyyy HH:mm:ss" ) );

      //
      // Log the event to the application log.
      //
      this.LogError ( EvEventCodes.User_Access_Error, stEvent.ToString ( ) );

    }//END LogPageAccess class

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    public void LogAction ( String Value )
    {
      try
      {
        EvApplicationEvent applicationEvent = new EvApplicationEvent (
          EvApplicationEvent.EventType.Action,
          EvEventCodes.Ok,
          this.ClassNameSpace,
          Value,
          this.ClassParameters.UserProfile.CommonName );

        this.AddEvent ( applicationEvent );
      }
      catch
      {
        this._AdapterLog.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + " EXCEPTION EVENT NOT RECORDED " );
      }
      this._AdapterLog.AppendLine (
        DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + " ACTION:  " + Value );
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogEvent ( String Value )
    {
      EvApplicationEvent ApplicationEvent = new EvApplicationEvent (
        EvApplicationEvent.EventType.Warning,
        EvEventCodes.Ok,
        this.ClassNameSpace,
        Value,
        this.ClassParameters.UserProfile.CommonName );

      this.AddEvent ( ApplicationEvent );

      this._AdapterLog.AppendLine (
        DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + " EVENT:  " + Value );
    }

    // ==================================================================================
    /// <summary>
    /// This method appends EVENT to the class log
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Ex">Exception object.</param>
    // ----------------------------------------------------------------------------------
    protected void LogException ( Exception Ex )
    {
      String value = EvStatics.getException ( Ex );
      // 
      // Initialise the method variables
      // 
      try
      {
        EvApplicationEvent applicationEvent = new EvApplicationEvent (
          EvApplicationEvent.EventType.Error,
          EvEventCodes.Ok,
          this.ClassNameSpace,
          value,
          this.ClassParameters.UserProfile.CommonName );

        this.AddEvent ( applicationEvent );
      }
      catch
      {
        this._AdapterLog.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + " EXCEPTION EVENT NOT RECORDED " );
      }

      if ( this._Settings.LoggingLevel >= 0 )
      {
        this._AdapterLog.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + " EXCEPTION EVENT: " );
        this._AdapterLog.AppendLine ( value );
      }
    }//END LogException class

    // =====================================================================================
    /// <summary>
    /// This class checks whether the event's description,  Category and user name are written to the logError
    /// </summary>
    /// <param name="EventId">Integer: an event identifier</param>
    /// <param name="Description">string: an event's description</param>
    /// <param name="Category">string: an event category</param>
    /// <param name="UserName">string: a user name</param>
    /// <returns>Boolean: true, if the event is logged</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Fill the application event object. 
    /// 
    /// 2. Write the application event log
    /// 
    /// 3. Adding items to application event table. 
    /// 
    /// 4. Return true, if adding runs successfully. 
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public void LogError (
      EvEventCodes EventId,
      String Description )
    {
      // 
      // Initialise the method variables
      // 
      try
      {
        EvApplicationEvent applicationEvent = new EvApplicationEvent (
          EvApplicationEvent.EventType.Error,
          EventId,
          this.ClassNameSpace,
          Description,
          this.ClassParameters.UserProfile.CommonName );

        this.AddEvent ( applicationEvent );
      }
      catch
      {
        this._AdapterLog.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + " EXCEPTION EVENT NOT RECORDED " );
      }

      if ( this._Settings.LoggingLevel >= 0 )
      {
        this._AdapterLog.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + " EXCEPTION EVENT: " );
        this._AdapterLog.AppendLine ( Description );
      }
    }//END LogError method


    // =====================================================================================
    /// <summary>
    /// This class adds items to the application event table. 
    /// </summary>
    /// <param name="ApplicationEvent">EvApplicationEvent: An application event object</param>
    /// <returns>EvEventCodes: an event code for adding items</returns>
    // -------------------------------------------------------------------------------------
    private EvEventCodes AddEvent (
      EvApplicationEvent ApplicationEvent )
    {
      try
      {
        //
        // If the action is set to delete the object.
        // 
        return this._Bll_ApplicationEvents.AddEvent ( ApplicationEvent );

      }
      catch
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

    }//END AddEvent method

    #endregion

  }//END Service class

}///END NAMESPACE
