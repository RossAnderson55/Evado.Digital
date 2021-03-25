/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\EuAdapter.cs" 
 *  company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c)  2002 - 2021  EVADO HOLDING PTY. LTD.  All rights reserved.
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
using Evado.Bll.Digital;
using Evado.Model.Digital;


namespace Evado.UniForm.Digital
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

        this.LogInit ( "Class Parameters: " );
        this.LogInit ( "-ClientVersion: " + this._ClientVersion );
        this.LogInit ( "-ServiceUserProfile UserId: " + this.ServiceUserProfile.UserId );
        this.LogInit ( "-ClientDataObject.Title: " + this.ClientDataObject.Title );
        this.LogInit ( "-UniForm_BinaryFilePath: " + this.UniForm_BinaryFilePath );
        this.LogInit ( "-UniForm_BinaryServiceUrl: " + this.UniForm_BinaryServiceUrl );
        this.LogInit ( "-ApplicationPath: " + this.ApplicationPath );
        this.LogInit ( "-ExitCommand: " + this.ExitCommand.getAsString ( false, true ) );

        Evado.Bll.EvStaticSetting.EventLogSource = this._EventLogSource;

        //
        // Define the settings object.
        //
        this.ClassParameters.UserProfile = new EdUserProfile ( this.ServiceUserProfile );
        this.ClassParameters.LoggingLevel = 5;

        this._AdapterObjects = new EuGlobalObjects ( this.ClassParameters );
        this._AdapterObjects.LoggingLevel = 5;
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
        this.ClassParameters.AdapterGuid = this._AdapterObjects.Settings.Guid;
        this.ClassParameters.PlatformId = this._AdapterObjects.PlatformId;

        //
        // load the user's session object in to memory.
        //
        this.loadSessionObjects ( );

        //
        // Load the organisation list.
        //
        this.loadOrganisationList ( );

        //
        // Load the page layouts 
        //
        this.loadPageLayoutList ( );

        //
        // load the external selection list (issued for use)
        //
        this.loadSelectionLists ( );

        //
        // load the list of issued entity layouts in the application.
        //
        this.loadEnityLayoutList ( );

        //
        // load the list of issued record layouts in the application
        //
        this.loadRecordLayoutList ( );

        //
        // load the page components.
        //
        this.LoadPageComponents ( );

        /*
        this.LogInit ( "Page identifier list:" );
        foreach ( EvOption option in this._AdapterObjects.PageIdentifiers )
        {
          this.LogInit ( "-PageID: {0} - Desc: {1} ", option.Value, option.Description );
        }
        */
        this.LogInit ( "ServiceUserProfile" );
        this.LogInit ( "-AdsCustomerGroup: " + this.ServiceUserProfile.AdsCustomerGroup );

        this.LogInit ( "Settings:" );
        this.LogInit ( "-PlatformId: " + this.ClassParameters.PlatformId );
        this.LogInit ( "-AdapterGuid: " + this.ClassParameters.AdapterGuid );
        this.LogInit ( "-LoggingLevel: " + ClassParameters.LoggingLevel );
        this.LogInit ( "-UserId: " + ClassParameters.UserProfile.UserId );
        this.LogInit ( "-UserCommonName: " + ClassParameters.UserProfile.CommonName );

        this.LogInit ( "ClientDataObject.Page.Title: " + this.ClientDataObject.Page.Title );

        if ( this.Session.AdminUserProfile != null )
        {
          this.LogInit ( "AdminUserProfile.Guid: " + this.Session.AdminUserProfile.Guid );
          this.LogInit ( "AdminUserProfile.UserId: " + this.Session.AdminUserProfile.UserId );
          this.LogInit ( "AdminUserProfile.CommonName: " + this.Session.AdminUserProfile.CommonName );
        }

      }
      catch ( Exception Ex )
      {
        this.LogException ( Ex );

      }
      String end = Evado.Model.Digital.EvcStatics.CONST_METHOD_END;
      end = end.Replace ( "END OF METHOD", "END OF Evado.UniForm.Clinical.EuAdapter METHOD" );
      this.LogInit ( end );

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
    public const string ADAPTER_ID = "Evado_Digital";

    /// <summary>
    /// This constant defines the application global object hash table key value.
    /// </summary>
    public const string SESSION_OBJECT = "_SESSION_OBJECT";

    /// <summary>
    /// This constant defines the application global object hash table key value.
    /// </summary>
    public const string GLOBAL_OBJECT = ADAPTER_ID + "_GLOBAL_OBJECT";

    public const string CONST_ALERT_SELECT = "ALERT_SELECT";

    public const string CONST_HASH_ITEM_KEY_SELECT = "HIKS";

    public const int CONST_MENU_GROUP_WIDTH = 165;
    public const int CONST_HOME_PAGE_GROUP_DEFAULT_WIDTH = 900;
    public const int CONST_HOME_PAGE_GROUP_MARGINS = 100;
    public const int CONST_HOME_PAGE_GROUP_MAXIMUM_WIDTH = 1200;

    /// <summary>
    /// This constant contains the page identifier prefix
    /// </summary>
    public const string CONST_PAGE_ID_PREFIX = "Page_";

    /// <summary>
    /// This constant contains the entity page identifier prefix
    /// </summary>
    public const string CONST_ENTITY_PREFIX = "Entity_";

    /// <summary>
    /// This constant contains the entity prefix for a entity's organisation parent page identifier.
    /// </summary>
    public const string CONST_ORG_PARENT_PAGE_ID_SUFFIX = "_Organisation_Parent";

    /// <summary>
    /// This constant contains the entity prefix for a entity's user parent page identifier.
    /// </summary>
    public const string CONST_USER_PARENT_PAGE_ID_SUFFIX = "User_Parent";

    /// <summary>
    /// This constant contains the entity prefix for a entity's Entity parent page identifier.
    /// </summary>
    public const string CONST_ENTITY_PARENT_PAGE_ID_SUFFIX = "_Entity_Parent";

    /// <summary>
    /// This constant contains the entity list component prefix
    /// </summary>
    public const string CONST_ENTITY_LIST_PREFIX = "Entity_List_";

    /// <summary>
    /// This constant contains the entity filtered list component prefix
    /// </summary>
    public const string CONST_ENTITY_FILTERED_LIST_PREFIX = "Entity_Filtered_";

    /// <summary>
    /// This constant contains the record list component prefix
    /// </summary>
    public const string CONST_RECORD_LIST_PREFIX = "Record_List_";

    /// <summary>
    /// This constant contains the record filtered list component prefix
    /// </summary>
    public const string CONST_RECORD_FILTERED_LIST_PREFIX = "Recordy_Filtered_";

    /// <summary>
    /// This constant contains the record page identifier prefix
    /// </summary>
    public const string CONST_RECORD_PREFIX = "Record_";

    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class local objects.

    public static readonly EdRecordObjectStates CONST_RECORD_STATE_SELECTION_DEFAULT = EdRecordObjectStates.Withdrawn;

    private EuGlobalObjects _AdapterObjects = new EuGlobalObjects ( );

    private EuSession Session = new EuSession ( );

    private String ErrorMessage = String.Empty;

    EuNavigation _MenuUtility;

    private string _EventLogSource = ConfigurationManager.AppSettings [ Evado.Model.Digital.EvcStatics.CONFIG_EVENT_LOG_KEY ];

    private String _ConnectionSettingKey = EuAdapter.DEFAULT_CONNECTION_SETTING_KEY;

    private String _SessionObjectKey = String.Empty;

    private String _ClientObjectKey = String.Empty;

    private String _PlatformId = String.Empty;

    private String _FileRepositoryPath = String.Empty;

    private EvEventCodes _EventCode = EvEventCodes.Ok;

    private Evado.Model.Digital.EvClassParameters _ClassParameters = new Evado.Model.Digital.EvClassParameters ( );

    private Evado.Bll.EvApplicationEvents _Bll_ApplicationEvents = new Bll.EvApplicationEvents ( );

    private float _ApiVersion = Evado.Model.UniForm.AppData.API_Version;

    private float _ClientVersion = Evado.Model.UniForm.AppData.API_Version;

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
        return _ClassParameters;
      }
      set
      {
        this._ClassParameters = value;

        this._Bll_ApplicationEvents = new Bll.EvApplicationEvents ( this._ClassParameters );
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

    // <summary>
    // This constant defines the eClinical application field layout default setting.
    // </summary>
    public const Evado.Model.UniForm.FieldLayoutCodes DefaultFieldLayout = Evado.Model.UniForm.FieldLayoutCodes.Left_Justified;

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
        this.LogValue ( "Page Command: {0}. ", PageCommand.getAsString ( false, false ) );
        this.LogValue ( "Exit Command: {0}. ", this.ExitCommand.getAsString ( false, false ) );

        //
        // Turn on BLL debug to match the current class setting.
        //
        this.ClassParameters.LoggingLevel = this.LoggingLevel;

        this._MenuUtility = new EuNavigation (
        this._AdapterObjects, 
        this.Session, 
        this.ClassParameters );

        //
        // Set the web width.
        //
        String deviceName = PageCommand.GetHeaderValue ( Model.UniForm.CommandHeaderElements.DeviceName );

        //
        // Resolve application id and class id issues for default page commands.
        //
        if ( PageCommand.ApplicationId == Evado.Model.UniForm.EuStatics.CONST_DEFAULT )
        {
          PageCommand.ApplicationId = EuAdapter.ADAPTER_ID;
          PageCommand.Object = EuAdapterClasses.Home_Page.ToString ( );
        }

        // 
        // Initialise the methods variables and objects.
        // 
        Evado.Model.UniForm.AppData clientDataObject = new Model.UniForm.AppData ( );
        Evado.Bll.EvStaticSetting.SiteGuid = this._AdapterObjects.Settings.Guid;

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
        this.LogDebug ( "User Roles: {0}, ", this.Session.UserProfile.Roles );
        this.LogDebug ( "User TypeId: {0}, ", this.Session.UserProfile.TypeId );

        //
        // load the user's organisation.
        //
        this.loadUserOrganisation ( );

        //
        // Process a demonstration user registration request. 
        //
        if ( PageCommand.Object == EuAdapterClasses.User_Registration.ToString ( ) )
        {
          this.LogDebug ( "Demonstration User Registation" );

          this.callDemonstrationRegistration ( PageCommand );

          this.LogMethodEnd ( "getPageObject" );
          return clientDataObject;
        }

        this.LogDebug ( this.Session.UserProfile.getUserProfile ( false ) );

        this.LogDebug ( "UserProfile.RoleId: " + this.Session.UserProfile.Roles );

        //
        // get the application object.
        //
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
      String description = String.Empty;

      clientDataObject.Id = Guid.NewGuid ( );
      clientDataObject.Page.Id = clientDataObject.Id;
      clientDataObject.Page.Title = "Demonstration User Registration Page";

      Guid customerGuid = PageCommand.GetGuid ( );

      this.LogDebug ( "Customer Guid {0}.", customerGuid );

      clientDataObject.Message = "Demonstration Registration page.";

      var pageGroup = clientDataObject.Page.AddGroup ( "", Model.UniForm.EditAccess.Disabled );

      description = PageCommand.getAsString ( false, true );

      description += "\r\n User Type: " + "End_User";

      this.LogValue ( "Group Description:\r\n " + description );
      pageGroup.Description = description;
      //
      // initialise patient adapter the service object.
      //
      EuUserRegistration demonstrationUserRegistration = new EuUserRegistration (
        this._AdapterObjects,
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
      this._AdapterObjects = demonstrationUserRegistration.AdapterObjects;
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
      this.LogDebug ( "UserProfile.RoleId: " + this.ClassParameters.UserProfile.Roles );
      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.UniForm.AppData clientDataObject = new Model.UniForm.AppData ( );
      this.ErrorMessage = String.Empty;
      //
      // Define the records class
      //
      EuRecords records = new EuRecords (
              this._AdapterObjects,
              this.ServiceUserProfile,
              this.Session,
              this.UniForm_BinaryFilePath,
              this.UniForm_BinaryServiceUrl,
              this.ClassParameters );

      records.unLockRecord ( );

      //
      // Initialise the entities class
      //
      EuEntities entities = new EuEntities (
              this._AdapterObjects,
              this.ServiceUserProfile,
              this.Session,
              this.UniForm_BinaryFilePath,
              this.UniForm_BinaryServiceUrl,
              this.ClassParameters );

      entities.unLockRecord ( );

      // 
      // Save the application parameters to global objects.
      // 
      this.GlobalObjectList [ EuAdapter.GLOBAL_OBJECT ] = this._AdapterObjects;

      // 
      // Get the application object enumeration value.
      // 
      EuAdapterClasses adapterClass =
        Evado.Model.EvStatics.parseEnumValue<EuAdapterClasses> ( PageCommand.Object );

      this.LogDebug ( "adapterClass: " + adapterClass );

      //
      // Log command and exit for illegal access attempty.
      //
      if ( PageCommand.Type == Evado.Model.UniForm.CommandTypes.Anonymous_Command )
      {
        return this.IllegalAnonymousAccessAttempt ( adapterClass );
      }

      //
      // Select the class object to be displayed.
      //
      switch ( adapterClass )
      {
        case EuAdapterClasses.Application_Properties:
          {
            this.LogDebug ( " APPLICATION PROFILE CLASS SELECTED." );

            // 
            // Initialise the methods variables and objects.
            // 
            EuAdapterConfig applicationProfiles = new EuAdapterConfig (
              this._AdapterObjects,
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
        case EuAdapterClasses.Events:
          {
            this.LogDebug ( " APPLICATION EVENTS CLASS SELECTED." );

            // 
            // Initialise the methods variables and objects.
            // 
            EuApplicationEvents applicationProfiles = new EuApplicationEvents (
              this._AdapterObjects,
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
            // Initialise the methods variables and objects.
            // 
            EuStaticContentTemplates emailTemplates = new EuStaticContentTemplates (
              this._AdapterObjects,
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
            // Initialise the methods variables and objects.
            // 
            EuOrganisations organisations = new EuOrganisations ( this._AdapterObjects,
              this.ServiceUserProfile,
              this.Session,
              this.UniForm_BinaryFilePath,
              this.UniForm_BinaryServiceUrl,
              this.ClassParameters );

            organisations.LoggingLevel = this.LoggingLevel;

            clientDataObject = organisations.getDataObject ( PageCommand );
            this.ErrorMessage = organisations.ErrorMessage;
            this.LogAdapter ( organisations.Log );

            break;

          }
        case EuAdapterClasses.Users:
          {
            this.LogDebug ( " USERS CLASS SELECTED." );

            // 
            // Initialise the methods variables and objects.
            // 
            EuUserProfiles userProfiles = new EuUserProfiles ( this._AdapterObjects,
              this.ServiceUserProfile,
              this.Session,
              this.UniForm_BinaryFilePath,
              this.UniForm_BinaryServiceUrl,
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
            // Initialise the methods variables and objects.
            // 
            EuMenus menus = new EuMenus (
              this._AdapterObjects,
              this.ServiceUserProfile,
              this.Session,
              this.UniForm_BinaryFilePath,
              this.ClassParameters );

            menus.LoggingLevel = this.LoggingLevel;

            clientDataObject = menus.getClientDataObject ( PageCommand );
            // 
            // Save the application parameters to global objects.
            // 
            this.GlobalObjectList [ EuAdapter.GLOBAL_OBJECT ] = this._AdapterObjects;

            this.ErrorMessage = menus.ErrorMessage;
            this.LogAdapter ( menus.Log );

            break;
          }

        case EuAdapterClasses.Binary_File:
          {
            this.LogDebug ( "PROJECT CLASS SELECTED." );

            // 
            // Initialise the methods variables and objects.
            // 
            EuBinaryFiles binaryFiles = new EuBinaryFiles (
              this._AdapterObjects,
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
        /*
      case EuAdapterClasses.Alert:
        {
          this.LogDebug ( "ALERT CLASS SELECTED." );

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
         */
        case EuAdapterClasses.Page_Layouts:
          {
            this.LogDebug ( " PAGE LAYOUTS CLASS SELECTED." );

            // 
            // Initialise the methods variables and objects.
            // 
            EuPageLayouts pageLayouts = new EuPageLayouts ( this._AdapterObjects,
              this.ServiceUserProfile,
              this.Session,
              this.UniForm_BinaryFilePath,
              this.ClassParameters );

            pageLayouts.LoggingLevel = this.LoggingLevel;

            clientDataObject = pageLayouts.getDataObject ( PageCommand );
            this.ErrorMessage = pageLayouts.ErrorMessage;
            this.LogAdapter ( pageLayouts.Log );

            break;

          }
        case EuAdapterClasses.Selection_Lists:
          {
            this.LogDebug ( " SELECTION LISTS CLASS SELECTED." );

            // 
            // Initialise the methods variables and objects.
            // 
            EuSelectionLists selectionLists = new EuSelectionLists ( this._AdapterObjects,
              this.ServiceUserProfile,
              this.Session,
              this.UniForm_BinaryFilePath,
              this.UniForm_BinaryServiceUrl,
              this.ClassParameters );

            selectionLists.LoggingLevel = this.LoggingLevel;

            clientDataObject = selectionLists.getDataObject ( PageCommand );
            this.ErrorMessage = selectionLists.ErrorMessage;
            this.LogAdapter ( selectionLists.Log );

            break;

          }
        case EuAdapterClasses.Record_Layouts:
          {
            this.LogDebug ( "RECORD LAYOUTS CLASS SELECTED." );

            // 
            // Initialise the methods variables and objects.
            // 
            EuRecordLayouts recordLayouts = new EuRecordLayouts ( this._AdapterObjects,
              this.ServiceUserProfile,
              this.Session,
              this.UniForm_BinaryFilePath,
              this.ClassParameters );

            recordLayouts.LoggingLevel = this.LoggingLevel;

            clientDataObject = recordLayouts.getDataObject ( PageCommand );
            this.ErrorMessage = recordLayouts.ErrorMessage;
            LogAdapter ( recordLayouts.Log );

            break;
          }

        case EuAdapterClasses.Record_Layout_Fields:
          {
            this.LogDebug ( "RECOR LAYOUT FIELDS CLASS SELECTED." );


            // 
            // Initialise the methods variables and objects.
            // 
            EuRecordFields recordLayoutFields = new EuRecordFields ( this._AdapterObjects,
              this.ServiceUserProfile,
              this.Session,
              this.UniForm_BinaryFilePath,
              this.ClassParameters );

            recordLayoutFields.LoggingLevel = this.LoggingLevel;

            clientDataObject = recordLayoutFields.getDataObject ( PageCommand );
            this.ErrorMessage = recordLayoutFields.ErrorMessage;
            LogAdapter ( recordLayoutFields.Log );

            break;
          }
        case EuAdapterClasses.Entity_Layouts:
          {
            this.LogDebug ( "ENTITY LAYOUTS CLASS SELECTED." );

            // 
            // Initialise the methods variables and objects.
            // 
            EuEntityLayouts entityLayouts = new EuEntityLayouts ( this._AdapterObjects,
              this.ServiceUserProfile,
              this.Session,
              this.UniForm_BinaryFilePath,
              this.ClassParameters );

            entityLayouts.LoggingLevel = this.LoggingLevel;

            clientDataObject = entityLayouts.getDataObject ( PageCommand );
            this.ErrorMessage = entityLayouts.ErrorMessage;
            LogAdapter ( entityLayouts.Log );

            break;
          }

        case EuAdapterClasses.Entity_Layout_Fields:
          {
            this.LogDebug ( "ENTITY LAYOUT FIELDS CLASS SELECTED." );
            // 
            // Initialise the methods variables and objects.
            // 
            EuEntityFields entityLayoutFields = new EuEntityFields ( this._AdapterObjects,
              this.ServiceUserProfile,
              this.Session,
              this.UniForm_BinaryFilePath,
              this.ClassParameters );

            entityLayoutFields.LoggingLevel = this.LoggingLevel;

            clientDataObject = entityLayoutFields.getDataObject ( PageCommand );
            this.ErrorMessage = entityLayoutFields.ErrorMessage;
            LogAdapter ( entityLayoutFields.Log );
            break;
          }
        case EuAdapterClasses.Entities:
          {
            this.LogDebug ( "ENTITY CLASS SELECTED." );

            entities.LoggingLevel = this.LoggingLevel;

            clientDataObject = entities.getDataObject ( PageCommand );
            this.ErrorMessage = entities.ErrorMessage;
            LogAdapter ( entities.Log );
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
            EuReportTemplates reportTemplates = new EuReportTemplates ( this._AdapterObjects,
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
            EuReports reports = new EuReports ( this._AdapterObjects,
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
            EuAnalysis analysis = new EuAnalysis ( this._AdapterObjects,
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

        case EuAdapterClasses.Records:
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
            // Create the common record object.
            // 
            records.LoggingLevel = this.LoggingLevel;

            clientDataObject = records.getDataObject ( PageCommand );
            this.ErrorMessage = records.ErrorMessage;
            this.LogAdapter ( records.Log );

            break;
          }
        case EuAdapterClasses.Home_Page:
        default:
          {
            this.LogDebug ( "HOME PAGE SELECTED." );

            String pageId = PageCommand.GetPageId();

            if ( pageId != String.Empty )
            {
              return this.generatePage ( PageCommand );
            }

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
      this.LogInit ( "GlobalObjects.Count: " + this.GlobalObjectList.Count );
      this.LogInit ( "Settings.LoggingLevel: " + this.ClassParameters.LoggingLevel );

      this.LogInit ( "Default ConnectionSettingKey: " + this._ConnectionSettingKey );
      // 
      // Get the connection string key.
      // 
      if ( ConfigurationManager.AppSettings [ EuAdapter.CONFIG_CONNECTION_SETTING_KEY ] != null )
      {
        this._ConnectionSettingKey = ConfigurationManager.AppSettings [ EuAdapter.CONFIG_CONNECTION_SETTING_KEY ];

      }
      this.LogInit ( "_ConnectionSettingKey: '" + this._ConnectionSettingKey + "'" );

      Evado.Bll.EvStaticSetting.ConnectionStringKey = this._ConnectionSettingKey;

      this.LogInit ( "EvStaticSetting.ConnectionStringKey: '" + Evado.Bll.EvStaticSetting.ConnectionStringKey + "'" );

      // 
      // Get the respository file path.
      // 
      if ( ConfigurationManager.AppSettings [ Evado.Model.Digital.EvcStatics.CONFIG_RESPOSITORY_FILE_PATH_KEY ] != null )
      {
        this._FileRepositoryPath = ConfigurationManager.AppSettings [ Evado.Model.Digital.EvcStatics.CONFIG_RESPOSITORY_FILE_PATH_KEY ];
      }
      this.LogInit ( "FileRepositoryPath: '" + this._FileRepositoryPath + "'" );

      // 
      // load the global object.
      // 
      if ( this.GlobalObjectList.ContainsKey ( EuAdapter.GLOBAL_OBJECT ) == true )
      {
        this._AdapterObjects =
          (EuGlobalObjects) this.GlobalObjectList [ EuAdapter.GLOBAL_OBJECT ];
      }
      // 
      // Get the respository file path.
      // 
      this._PlatformId = "ADMIN";
      if ( ConfigurationManager.AppSettings [ Evado.Model.Digital.EvcStatics.CONFIG_PATFORM_ID_KEY ] != null )
      {
        this._PlatformId = ConfigurationManager.AppSettings [ Evado.Model.Digital.EvcStatics.CONFIG_PATFORM_ID_KEY ];
      }
      this.LogInit ( "PlatformId: '" + this._PlatformId + "'" );
      this.ClassParameters.PlatformId = this._PlatformId;
      this._AdapterObjects.PlatformId = this._PlatformId;

      //  
      // Load the paremeters from the web.config if not already loaded.
      // 
      if ( this._AdapterObjects.Settings.Guid != Guid.Empty )
      {
        this.LogInit ( "APPLICATION OBJECT IS LOADED" );
        return;
      }

      this.LogInit ( "LOADING THE APPLICATION OBJECT VALUES" );

      // 
      // Update the application path.
      // 
      this._AdapterObjects.ApplicationPath = this.ApplicationPath;

      //
      // The object is empty so load the application parameter values.
      //
      int loggingLevel = this._AdapterObjects.LoggingLevel;
      this._AdapterObjects.LoggingLevel = 5;

      this._AdapterObjects.loadGlobalParameters ( );

      this.LogInitClass ( this._AdapterObjects.Log );
      this.LogInit ( "Version: " + this._AdapterObjects.Settings.Version );
      this.LogInit ( "HiddenOrganisationFields: " + this._AdapterObjects.Settings.HiddenOrganisationFields );
      this.LogInit ( "HiddenUserFields: " + this._AdapterObjects.Settings.HiddenUserFields );

      // 
      // Save the application parameters to global objects.
      // 
      this.GlobalObjectList [ EuAdapter.GLOBAL_OBJECT ] = this._AdapterObjects;

      this.LogInit ( "GlobalObjects: " + this.GlobalObjectList.Count );

      this.LogInit ( "GLOBAL APPLICATION OBJECT VALUES LOADED" );
      this._AdapterObjects.LoggingLevel = loggingLevel;

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
        if ( this._AdapterObjects.PlatformId != String.Empty )
        {
          this.LogInit ( "The web site identifier exists to exit." );
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

        this.LogInit ( "ignore list: " + stIgnoreFileList );

        int deletedFileCount = Evado.Model.Digital.EvcStatics.Files.deleteOldFiles ( this.UniForm_BinaryFilePath, 2, ignoreFileList );

        //this.LogInitValue ( Evado.Model.Digital.EvcStatics.Files.DebugLog );
        this.LogInit ( "deletedFile count: " + deletedFileCount );
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
      // if last page is null then the 
      //
      if ( this.Session.LastPage == null )
      {
        this.Session.LastPage = new Model.UniForm.AppData ( );
      }

      // 
      // load eclinical session object.
      //
      if ( this.ServiceUserProfile.UserId == String.Empty )
      {
        //this.LogInitValue ( "user key empty." );
        this._SessionObjectKey = String.Empty;
        return;
      }

      this._SessionObjectKey = this.ServiceUserProfile.UserId + EuAdapter.SESSION_OBJECT;
      this._SessionObjectKey = this._SessionObjectKey.Replace ( ".", "_" );
      this._SessionObjectKey = this._SessionObjectKey.ToUpper ( );

      this.LogInit ( "SessionObjectKey: " + this._SessionObjectKey );

      if ( this.GlobalObjectList.ContainsKey ( this._SessionObjectKey ) == true )
      {
        this.Session = (Evado.UniForm.Digital.EuSession) this.GlobalObjectList [ this._SessionObjectKey ];

        this.LogInit ( "Session object loaded." );
      }

      this._ClientObjectKey = this.ServiceUserProfile.UserId + Evado.Model.Digital.EvcStatics.SESSION_CLIENT_DATA_OBJECT;
      this._ClientObjectKey = this._ClientObjectKey.Replace ( ".", "_" );
      this._ClientObjectKey = this._ClientObjectKey.ToUpper ( );

      this.LogInit ( "ClientObjectKey: " + this._ClientObjectKey );

      if ( this.GlobalObjectList.ContainsKey ( this._ClientObjectKey ) == true )
      {
        this.ClientDataObject = (Evado.Model.UniForm.AppData) this.GlobalObjectList [ this._ClientObjectKey ];

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
      this.LogInit ( "Session Clinical objects: " );
      this.LogInit ( "-AdsEnabled: " + this.Session.AdsEnabled );
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
      this.LogInit ( "SESSION OBJECTS LOADED" );

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
        this.LogEvent ( "Session Key Empty." );
        this.LogMethodEnd ( "saveSessionObject" );
        return;
      }

      // 
      // Save the session object.
      //
      this.GlobalObjectList [ this._SessionObjectKey ] = (EuSession) this.Session;

      //
      // Save the last generated client data object
      //
      //this.LogDebugValue ( "ClientObject.Page.Title: " + this.ClientDataObject.Page.Title );
      //this.LogDebugValue ( "ClientObject.id: " + this.ClientDataObject.Id );


      this.GlobalObjectList [ this._ClientObjectKey ] = this.ClientDataObject;

      string Date_Key = this._SessionObjectKey;

      Date_Key = Date_Key.Replace ( EuAdapter.SESSION_OBJECT,
        Evado.Model.UniForm.EuStatics.GLOBAL_DATE_STAMP );

      this.LogValue ( "Date_Key: " + Date_Key );

      this.GlobalObjectList [ Date_Key ] = DateTime.Now.ToString ( "dd MMM yyyy HH:mm" );

      //
      // Save the application session for the next user generated groupCommand.
      //
      this.GlobalObjectList [ EuAdapter.GLOBAL_OBJECT ] = this._AdapterObjects;

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
      this.ErrorMessage = EdLabels.Security_Illegal_Anonymoous_Access_Message;
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
          EdLabels.Default_Home_Page_Command_Title,
          EuAdapter.ADAPTER_ID,
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

      if ( this._ClassParameters.LoggingLevel >= 0 )
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

      if ( this._ClassParameters.LoggingLevel >= 0 )
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
