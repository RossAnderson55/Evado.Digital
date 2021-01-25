/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\GlobalApplicationObjects.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the AbstractedPage ResultData object.
 *
 ****************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Configuration;
using System.Configuration;
using System.Diagnostics;

using Evado.Bll;
using Evado.Model;
using Evado.Bll.Clinical;
using  Evado.Model.Digital;
// using Evado.Web;

namespace Evado.UniForm.Clinical
{
  /// <summary>
  /// This class contains the session ResultData object
  /// </summary>
  [Serializable]
  public class EuApplicationObjects 
  {
    #region Class Initialisation

    //===================================================================================
    /// <summary>
    /// This method initialises the class.
    /// </summary>
    //-----------------------------------------------------------------------------------
    public EuApplicationObjects ( )
    {
      this.Settings = new EvClassParameters ( );
      this._ClassNameSpace = "Evado.UniForm.Clinical.EuApplicationObjects.";
    }

    //===================================================================================
    /// <summary>
    /// This method initialises the class and passs in the user profile.
    /// </summary>
    //-----------------------------------------------------------------------------------
    public EuApplicationObjects (
      EvClassParameters Settings )
    {
      this._ClassNameSpace = "Evado.UniForm.Clinical.EuApplicationObjects.";
      this.Settings = Settings;

      this._LoggingLevel = this.Settings.LoggingLevel;

    }//END Method

    #endregion

    #region Class enumerators

    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Public methods.

    //===================================================================================
    /// <summary>
    /// This method initialises the global object.
    /// </summary>
    //-----------------------------------------------------------------------------------
    public void loadGlobalParameters ( )
    {
      this.LogMethod ( "loadGlobalParameters" );
      this.Settings.LoggingLevel = this.LoggingLevel;

      this.LoadStaticEnvironmentalProperties ( );

      this.loadApplicationProperties ( );

      this.loadGlobalMenu ( );

      this.LoadEmailTemplates ( );

      this.loadSmtpServerProperties ( );

      this.LogMethodEnd ( "loadGlobalParameters" );

    }//END loadGlobalParameters method

    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class properties and variables.

    // 
    // Constants
    //

    public const string Static_DefaultEventSource = "Application";


    Evado.UniForm.Clinical.AssemblyAttributes _AssembyAttributes = new AssemblyAttributes();

    private string _ApplicationPath = String.Empty;
    /// <summary>
    /// this field defines the Event Log Source
    /// </summary>
    public string ApplicationPath
    {
      get { return _ApplicationPath; }
      set { _ApplicationPath = value; }
    }

    /// <summary>
    /// This property contains the full version of the assembly.
    /// </summary>
    public String FullVersion
    {
      get
      {
        return this._AssembyAttributes.FullVersion;
      }

    }

    private String _LicensedModules = EdModuleCodes.Administration_Module + ";"
      + EdModuleCodes.Management_Module + ";"
      + EdModuleCodes.Imaging_Module + ";"
      + EdModuleCodes.Integration_Module ;
    /// <summary>
    /// This property contains an encoded ';' list of licenced modules for this instance.
    /// </summary>
    public String LicensedModules
    {
      get { return this._LicensedModules; }
      set { this._LicensedModules = value; }
    }

    String _PlatformId = String.Empty;

    /// <summary>
    /// This field identifies one or more Evado web applications accessing the same database.
    /// </summary>
    public string PlatformId
    {
      get { return _PlatformId; }
      set { _PlatformId = value; }
    }

    private string _HelpUrl = String.Empty;

    /// <summary>
    /// This field contains the url to the help page site.
    /// </summary>
    public string HelpUrl
    {
      get { return _HelpUrl; }
      set { _HelpUrl = value; }
    }

    private string _RepositoryFilePath = String.Empty;
    /// <summary>
    /// This field defines the binary file repository path.
    /// 
    /// If the binary file path is empty then the binary file functionality is disbled and
    /// not binary files can be uploaded.
    /// </summary>
    public string RepositoryFilePath
    {
      get { return _RepositoryFilePath; }
      set { _RepositoryFilePath = value; }
    }

    private string _ExportFilePath = String.Empty;
    /// <summary>
    /// This field defines the file export file path
    /// </summary>
    public string ExportFilePath
    {
      get { return _ExportFilePath; }
      set { _ExportFilePath = value; }
    }

    /// <summary>
    /// This value contains the path to teh XSL transform file name, OBSOLETE
    /// </summary>
    private string relativelXslCrfFilePath = @"xsl\crf.xsl";

    /// <summary>
    /// This property contains the relavive CRF Xsl File path 
    /// </summary>
    public string RelativelXslCrfFilePath
    {
      get { return relativelXslCrfFilePath; }
      set { relativelXslCrfFilePath = value; }
    }

    private List<EdExternalSelectionList> _ExternalSelectionLists = new List<EdExternalSelectionList> ( );
    /// <summary>
    /// this field list containt the currently loaded external field selection lists.  Used by forms to fill coding
    /// lists.
    /// </summary>

    public List<EdExternalSelectionList> ExternalSelectionLists
    {
      get { return _ExternalSelectionLists; }
      set { _ExternalSelectionLists = value; }
    }


    List<Evado.Model.Digital.EvCustomer> _CustomerList = new List<Evado.Model.Digital.EvCustomer> ( );
    /// <summary>
    /// This property object contains the eClinical customer list object.
    /// </summary>
    public List<Evado.Model.Digital.EvCustomer> CustomerList
    {
      get
      {
        return this._CustomerList;
      }
      set
      {
        this._CustomerList = value;
      }
    }


    /// <summary>
    /// This property object contains the eClinical customer list object.
    /// </summary>
    public List<Evado.Model.EvOption> CustomerSelectionList
    {
      get
      {
        List<EvOption> optionList = new List<EvOption> ( );

        optionList.Add ( new EvOption ( ) );

        //
        // iterate through the list of customers creating option values..
        //
        foreach ( EvCustomer customer in this.CustomerList )
        {
          optionList.Add ( new EvOption (
            customer.Guid.ToString(),
            String.Format( 
             EvCustomerLabels.Customer_No_Name_Format,
             customer.CustomerNo,
             customer.Name ) ) );
        }

        return optionList;
      }
    }


    /// <summary>
    /// This property contains the email templates for user administration.
    /// </summary>
    public EvStaticContentTemplates ContentTemplates { get; set; }

    private string _ApplicationUrl = String.Empty;
    /// <summary>
    /// This property contains the application URL
    /// </summary>
    public string ApplicationUrl
    {
      get
      {
        return this._ApplicationUrl;
      }
      set
      {
        this._ApplicationUrl = value;
      }
    }

    private string _PasswordResetUrl = String.Empty;
    /// <summary>
    /// This property contains the password reset Urla
    /// </summary>
    public string PasswordResetUrl
    {
      get
      {
        return this._PasswordResetUrl;
      }
      set
      {
        this._PasswordResetUrl = value;
      }
    }

    private string _SupportEmailAddress = String.Empty;
    /// <summary>
    /// This property contains the password reset Urla
    /// </summary>
    public string SupportEmailAddress
    {
      get
      {
        return this._SupportEmailAddress;
      }
      set
      {
        this._SupportEmailAddress = value;
      }
    }

    private string _NoReplyEmailAddress = String.Empty;
    /// <summary>
    /// This property contains the password reset Urla
    /// </summary>
    public string NoReplyEmailAddress
    {
      get
      {
        return this._NoReplyEmailAddress;
      }
      set
      {
        this._NoReplyEmailAddress = value;
      }
    }

    private Model.Digital.EdAdapterParameters _PlatformSettings = new Model.Digital.EdAdapterParameters ( );
    /// <summary>
    /// This property object contains the platform settings object.
    /// </summary>
    public Model.Digital.EdAdapterParameters PlatformSettings
    {
      get { return _PlatformSettings; }
      set { _PlatformSettings = value; }
    }

    private List<EvMenuItem> _MenuList = new List<EvMenuItem> ( );
    /// <summary>
    /// This property object contains a list of the web application menu item object..
    /// </summary>
    public List<EvMenuItem> MenuList
    {
      get { return _MenuList; }
      set { _MenuList = value; }
    }

    private int _AlertListLength = 20;
    /// <summary>
    /// This property contains the list length for displaying alerts.
    /// </summary>
    public int AlertListLength
    {
      get { return _AlertListLength; }
      set { _AlertListLength = value; }
    }

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region class methods

     ///
    ///  =======================================================================================
    /// <summary>
    /// This method loades the global menu objects
    /// </summary>
    //  ---------------------------------------------------------------------------------
    public void loadGlobalMenu ( )
    {
      this.LogMethod ( "loadGlobalMenu method" );
      this.LogDebugValue ( "PlatformId: " + this._PlatformId );

      try
      {
        // 
        // Initialse the methods objects and variables.
        // 
        EvMenus bll_Menu = new EvMenus ( this.Settings );

        // 
        // Get the site setMenu.
        // 
        this.MenuList = bll_Menu.getView ( this._PlatformId, String.Empty );

        this.LogDebug ( bll_Menu.Log );
        this.LogInitValue ( "MenuList .Count: " + this.MenuList.Count );

        // 
        // Process menu items.
        // 
        /*
        for ( int count = 0; count < this.MenuList.Count; count++ )
        {
          EvMenuItem menuItem = this.MenuList [ count ];

          this.LogDebugFormat ( "Group: {0}, PageId: {1}, Title: {2}, Roles: {3}", menuItem.Group, menuItem.PageId, menuItem.Title, menuItem.Modules );
          //
          // Load the menu items that are associated with the loaded modules.
          //
          if ( this.PlatformSettings.hasModule ( menuItem.ModuleList ) == false )
          {
            this.LogDebugFormat ( "REMOVE: Group: {0}, PageId: {1}, Title: {2}, Roles: {3}", menuItem.Group, menuItem.PageId, menuItem.Title, menuItem.Modules );
            //
            // Remove the menu item from the list.
            //
            this.MenuList.RemoveAt ( count );
            count--;
            continue;
          }

        }//END menuitem iteration loop.
        */
      }
      catch ( Exception Ex )
      {
        this.LogInitValue ( "loadGlobalMenu exception:  " + Evado.Model.Digital.EvcStatics.getException ( Ex ) );
      }
      this.LogInitValue ( "Global Menu list count: " + this.MenuList.Count );

    }//END loadMenu method

    //  =================================================================================
    /// <summary>
    ///   This method create a list of module options.  The list is filled from LicensedModules. 
    /// </summary>
    /// <param name="Add_AllModules">boolean add All_Modules option to the list..</param>
    /// <returns>List of EvOption </returns>
    //  ---------------------------------------------------------------------------------
    public List<EvOption> getModuleList ( bool Add_AllModules )
    {
      //
      // Get the string value of the selected module
      // 
      List<EvOption> loadedModulesList = new List<EvOption> ( );

      String [ ] arrLicensedModules = this.LicensedModules.Split ( ';' );

      if ( Add_AllModules == true )
      {
        String str = EdModuleCodes.All_Modules.ToString ( );
        string description = str.Replace ( "_", " " );
        loadedModulesList.Add ( new EvOption ( str, description ) );
      }

      //
      // Create the Loaded modules list
      //
      foreach ( String str in arrLicensedModules )
      {
        string description = str.Replace ( "_", " " );
        loadedModulesList.Add ( new EvOption ( str, description ) );
      }

      return loadedModulesList;

    }//END addRole method.

    ///  =======================================================================================
    /// <summary>
    /// Description:
    ///  Load the site properties object.
    /// 
    /// </summary>
    //  ---------------------------------------------------------------------------------
    private void loadApplicationProperties ( )
    {
      this.LogMethod ( "loadApplicationProperties method" );
      // 
      // Load the Web Site Properties
      // 
      Bll.Clinical.EdAdapterSettings applicationProfiles = new Bll.Clinical.EdAdapterSettings (  );

      if ( this.Settings != null )
      {
        applicationProfiles = new Bll.Clinical.EdAdapterSettings ( this.Settings );
      }

      //
      // Get the Application profile.
      //
      this._PlatformSettings = applicationProfiles.getItem ( "A" );

      this.LogDebug ( applicationProfiles.Log );

      if ( ConfigurationManager.AppSettings [ EvcStatics.CONFIG_WEB_CLIENT_URL_KEY ] != null )
      {
        string value = ConfigurationManager.AppSettings [ EvcStatics.CONFIG_WEB_CLIENT_URL_KEY ];
        if ( value != String.Empty )
        {
          this._ApplicationUrl = value;
        }
      }

      this.LogDebugValue ( "ApplicationUrl: " + this._ApplicationUrl );

      if ( ConfigurationManager.AppSettings [ EuAdapter.CONFIG_PASSWORD_RESET_URL_KEY ] != null )
      {
        string value = ConfigurationManager.AppSettings [ EuAdapter.CONFIG_PASSWORD_RESET_URL_KEY ];
        if ( value != String.Empty )
        {
          this._PasswordResetUrl = value;
        }
      }
      this.LogDebugValue ( "PasswordResetUrl: " + this._PasswordResetUrl );

      if ( ConfigurationManager.AppSettings [ EuAdapter.CONFIG_SUPPORT_EMAIL_ADDRESS_KEY ] != null )
      {
        string value = ConfigurationManager.AppSettings [ EuAdapter.CONFIG_SUPPORT_EMAIL_ADDRESS_KEY ];
        if ( value != String.Empty )
        {
          this._SupportEmailAddress = value;
        }
      }
      this.LogDebugValue ( "SupportEmailAddress: " + this._SupportEmailAddress );

      if ( ConfigurationManager.AppSettings [ EuAdapter.CONFIG_NOREPLY_EMAIL_ADDRESS_KEY ] != null )
      {
        string value = ConfigurationManager.AppSettings [ EuAdapter.CONFIG_NOREPLY_EMAIL_ADDRESS_KEY ];
        if ( value != String.Empty )
        {
          this._NoReplyEmailAddress = value;
        }
      }
      this.LogDebugValue ( "NoReplyEmailAddress: " + this._NoReplyEmailAddress );

      this.LogDebugValue ( "Full version: " + _AssembyAttributes.FullVersion );

      this._PlatformSettings.Version = _AssembyAttributes.FullVersion;
      this._PlatformSettings.MinorVersion = _AssembyAttributes.MinorVersion;

      //
      // Set the Stide GUid used by encryption module.
      //
      ConfigurationManager.AppSettings [ "SiteGuid" ] = this._PlatformSettings.Guid.ToString ( );

      // 
      // Log the Site Properties on startup.
      // 
      this.LogDebugValue ( "Version: " + this._PlatformSettings.Version );

    }//ENd loadSiteProperties method

    ///  =======================================================================================
    /// <summary>
    /// Description:
    ///  Sets static global variables.
    /// 
    /// </summary>
    //  ---------------------------------------------------------------------------------
    private void LoadStaticEnvironmentalProperties ( )
    {
      this.LogMethod ( "LoadStaticEnvironmentalProperties method" );

      // 
      // Set the Website property.
      // 
      if ( this._PlatformId == String.Empty )
      {
        this._PlatformId = "PROD";
      }

      if ( ConfigurationManager.AppSettings [  Evado.Model.Digital.EvcStatics.CONFIG_PATFORM_ID_KEY ] != null )
      {
        this._PlatformId = (string) ConfigurationManager.AppSettings [  Evado.Model.Digital.EvcStatics.CONFIG_PATFORM_ID_KEY ];
      }

      this.LogDebugValue ( "PlatformId: " + this._PlatformId );

      // 
      // Set the connection string settings.
      // 
      if ( ConfigurationManager.AppSettings [  Evado.Model.Digital.EvcStatics.CONFIG_HELP_URL_KEY ] != null )
      {
        this._HelpUrl = (String) ConfigurationManager.AppSettings [  Evado.Model.Digital.EvcStatics.CONFIG_HELP_URL_KEY ];

      }

      this.LogDebugValue ( "HelpUrl: '" + this._HelpUrl + "'" );

      // 
      // Log the Maximum selection list length on startup.
      // 
      this.LogDebugValue ( "MaximumSelectionListLength: " + this._PlatformSettings.MaximumSelectionListLength );

    }//END setStaticEnvironmentalProperties method

    ///  =======================================================================================
    /// <summary>
    /// Description:
    ///  Sets static global variables.
    /// 
    /// </summary>
    //  ---------------------------------------------------------------------------------
    private void LoadEmailTemplates ( )
    {
      this.LogMethod ( "LoadEmailTemplates method" );

      //
      // Load the email templates.
      //
      if ( this.ContentTemplates == null )
      {
        this.ContentTemplates =
          EvStatics.Files.readXmlFile<EvStaticContentTemplates> (
          this._ApplicationPath, EuStaticContentTemplates.CONST_EMAIL_TEMPLATE_FILENAME );
      }

      if ( this.ContentTemplates != null )
      {
        this.LogDebugValue ( "IntroductoryEmail_Title: " + this.ContentTemplates.IntroductoryEmail_Title );
        this.LogDebugValue ( "UpdatePasswordEmail_Title: " + this.ContentTemplates.UpdatePasswordEmail_Title );
        this.LogDebugValue ( "ResetPasswordEmail_Title: " + this.ContentTemplates.ResetPasswordEmail_Title );
        this.LogDebugValue ( "PasswordConfirmationEmail_Title: " + this.ContentTemplates.PasswordConfirmationEmail_Title );
      }

    }

    ///  =======================================================================================
    /// <summary>
    /// Description:
    ///  Sets static SMTP server properties.
    /// 
    /// </summary>
    //  ---------------------------------------------------------------------------------
    private void loadSmtpServerProperties ( )
    {
      this.LogMethod ( "loadSmtpServerProperties method" );

      // 
      // Log the SMTP server.
      // 
      this.LogDebugValue ( " SmtpServer: " + this._PlatformSettings.SmtpServer );

      // 
      // Log the SMTP port setting
      // 
      this.LogDebugValue ( " SmtpServerPort: " + this._PlatformSettings.SmtpServerPort );

      // 
      // Log the SMTP User Id.
      // 
      this.LogDebugValue ( " SmtpUserId: " + this._PlatformSettings.SmtpUserId );

      // 
      // Log the SMTP user password.
      // 
      this.LogDebugValue ( " SmtpPassword: " + this._PlatformSettings.SmtpPassword );

      // 
      // Log the SMTP User Id.
      // 
      this.LogDebugValue ( " SmtpUserId: " + this._PlatformSettings.SmtpUserId );

      // 
      // Log the SMTP user password.
      // 
      this.LogDebugValue ( " EmailAlertTestAddress: " + this._PlatformSettings.EmailAlertTestAddress );


    }//END setSmtpServerProperties method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class Adapter properties


    private String _ErrorMessage = String.Empty;

    /// <summary>
    /// This property defines the error message to be displayed to the user.
    /// </summary>
    public string ErrorMessage
    {
      get
      {
        return _ErrorMessage;
      }
      set { _ErrorMessage = value; }
    }

    protected String _FileRepositoryPath;

    /// <summary>
    /// This property contains the binary files repository path
    /// </summary>

    public String FileRepositoryPath
    {
      get { return this._FileRepositoryPath; }
      set { this._FileRepositoryPath = value; }
    }

    private Evado.Model.Digital.EvClassParameters _Settings = new Evado.Model.Digital.EvClassParameters ( );
    /// <summary>
    /// This property contains the setting data object. 
    /// </summary>
    public Evado.Model.Digital.EvClassParameters Settings
    {
      get
      {
        return _Settings;
      }
      set
      {
        this._Settings = value;

      }
    }

    #endregion

    #region Debug methods.

    /// <summary>
    /// this object stores the application log content.
    /// </summary>
    private StringBuilder _AdapterLog = new StringBuilder ( );
    public const int LoggingEventLevel = 0;
    public const int LoggingMethodLevel = 3;
    public const int LoggingValueLevel = 4;
    public const int DebugValueLevel = 5;

    public string Log
    {
      get
      {
        return _AdapterLog.ToString ( );
      }
    }

    protected int _LoggingLevel = 1;
    /// <summary>
    /// This property sets the debug state for the class.
    /// </summary>
    public int LoggingLevel
    {
      get { return _LoggingLevel; }
      set
      {
        _LoggingLevel = value;
        this.Settings.LoggingLevel = LoggingEventLevel;

        if ( this.LoggingLevel < 1)
        {
          this.LoggingLevel = 0;
        }
        if ( this.LoggingLevel > 5 )
        {
          this.LoggingLevel = 5;
        }
        if ( this.LoggingLevel < EuClassAdapterBase.LoggingMethodLevel )
        {
          this.resetAdapterLog ( ); ;
        }
      }
    }

    protected String _ClassNameSpace = "Evado.Model.UniForm.ApplicationServiceBase.";

    // ==================================================================================
    /// <summary>
    /// This method resets the debug log
    /// </summary>
    // ----------------------------------------------------------------------------------
    protected void resetAdapterLog ( )
    {
      this._AdapterLog = new StringBuilder ( );
    }
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
    public void LogPageAccess (
     String ClassMethodAccessed,
      Evado.Model.Digital.EvUserProfile User )
    {
      // 
      // Initialise the method variables
      // 
      string stEvent = "UserId: " + User.UserId
            + " name: " + User.CommonName
            + " opened this " + ClassMethodAccessed + " method at " + DateTime.Now.ToString ( "dd MMM yyyy HH:mm:ss" );

      // 
      // Initialise the method variables
      // 
      EvApplicationEvent applicationEvent = new EvApplicationEvent (
        EvApplicationEvent.EventType.Action,
        EvEventCodes.User_Access_Granted,
        this._ClassNameSpace,
        stEvent,
        this.Settings.UserProfile.CommonName );

      // this.AddEvent ( applicationEvent );

      //
      // Log the event to the application log.
      //
      this.LogDebugValue ( stEvent );

    }//END LogPageAccess class

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
      Evado.Model.Digital.EvUserProfile User )
    {
      // 
      // Initialise the method variables
      // 
      string stEvent = "Illegal access attempt UserId: " + User.UserId
            + " name: " + User.CommonName
            + " opened this " + ClassMethodAccessed + " method at " + DateTime.Now.ToString ( "dd MMM yyyy HH:mm:ss" );

      // 
      // Initialise the method variables
      // 
      EvApplicationEvent applicationEvent = new EvApplicationEvent (
        EvApplicationEvent.EventType.Error,
        EvEventCodes.User_Access_Error,
        this._ClassNameSpace,
        stEvent,
        this.Settings.UserProfile.CommonName );

      // this.AddEvent ( applicationEvent );

      //
      // Log the event to the application log.
      //
      this.LogDebugValue ( stEvent );

    }//END LogPageAccess class


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
      String RoleId,
      Evado.Model.Digital.EvUserProfile User )
    {
      // 
      // Initialise the method variables
      // 
      System.Text.StringBuilder stEvent = new StringBuilder ( );

      stEvent.AppendFormat (
         "Illegal access attempt UserId: {0} name {1} opened {2} method at {3} ",
         User.UserId,
         User.CommonName,
         ClassMethodAccessed,
         DateTime.Now.ToString ( "dd MMM yyyy HH:mm:ss" ) );

      stEvent.AppendFormat (
        "Page Role: {0} user Access: {2} ",
        User.Roles,
        RoleId );

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
    protected void LogInitMethod ( String Value )
    {
      this._AdapterLog.AppendLine ( Evado.Model.EvStatics.CONST_METHOD_START
      + DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": "
      + this._ClassNameSpace + Value );
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="DebugLogString">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogInitValue ( String DebugLogString )
    {
      this._AdapterLog.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " + DebugLogString );
    }
    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogMethod ( String Value )
    {
      this._AdapterLog.AppendLine ( Evado.Model.EvStatics.CONST_METHOD_START
      + DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": "
      + this._ClassNameSpace + Value );
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="MethodName">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogMethodEnd ( String MethodName )
    {
      if ( this.LoggingLevel >= EuClassAdapterBase.LoggingValueLevel )
      {
        String value = Evado.Model.EvStatics.CONST_METHOD_END;

        value = value.Replace ( " END OF METHOD ", " END OF " + MethodName + " METHOD " );

        this._AdapterLog.AppendLine ( value );
      }
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
        this._ClassNameSpace,
        Value,
        this.Settings.UserProfile.CommonName );

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
      EvApplicationEvent applicationEvent = new EvApplicationEvent (
        EvApplicationEvent.EventType.Error,
        EvEventCodes.Ok,
        this._ClassNameSpace,
        value,
        this.Settings.UserProfile.CommonName );

      // this.AddEvent ( applicationEvent );

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
      EvApplicationEvent applicationEvent = new EvApplicationEvent (
        EvApplicationEvent.EventType.Error,
        EventId,
        this._ClassNameSpace,
        Description,
        this.Settings.UserProfile.CommonName );

      // this.AddEvent ( applicationEvent );

      if ( this._Settings.LoggingLevel >= 0 )
      {
        this._AdapterLog.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + " EXCEPTION EVENT: " );
        this._AdapterLog.AppendLine ( Description );
      }
    }//END LogError method


    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogValue ( String Value )
    {
      if ( this.LoggingLevel >= EuClassAdapterBase.LoggingValueLevel )
      {
        this._AdapterLog.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " + Value );
      }
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogClass ( String Value )
    {
      if ( this.LoggingLevel >= EuClassAdapterBase.LoggingValueLevel )
      {
        this._AdapterLog.Append ( Value );
      }
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogTextStart ( String Value )
    {
      if ( this.LoggingLevel >= EuClassAdapterBase.LoggingValueLevel )
      {
        this._AdapterLog.Append ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " + Value );
      }
    }


    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogTextEnd ( String Value )
    {
      if ( this.LoggingLevel >= EuClassAdapterBase.LoggingValueLevel )
      {
        this._AdapterLog.AppendLine ( Value );
      }
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogText ( String Value )
    {
      if ( _LoggingLevel >= EuClassAdapterBase.LoggingValueLevel )
      {
        this._AdapterLog.Append ( Value );
      }
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogDebugValue ( String Value )
    {
      if ( this.LoggingLevel >= EuClassAdapterBase.DebugValueLevel )
      {
        this._AdapterLog.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " + Value );
      }
    }
    // ==================================================================================
    /// <summary>
    /// This method appendes debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Format">String: format text.</param>
    /// <param name="args">Array of objects as parameters.</param>
    // ----------------------------------------------------------------------------------
    protected void LogDebugFormat ( String Format, params object [ ] args )
    {
      if ( this._LoggingLevel >= EuClassAdapterBase.DebugValueLevel )
      {
        this._AdapterLog.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ":" +
          String.Format ( Format, args ) );
      }
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogDebug ( String Value )
    {
      if ( this.LoggingLevel >= EuClassAdapterBase.DebugValueLevel )
      {
        this._AdapterLog.Append ( Value );
      }
    }

    // ==================================================================================
    /// <summary>
    /// This property define whether debug loggin is enabled.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public bool DebugOn
    {
      get
      {
        if ( this.LoggingLevel >= EuClassAdapterBase.DebugValueLevel )
        {
          return true;
        }
        return false;
      }
    }

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
        Evado.Bll.EvApplicationEvents _Bll_ApplicationEvents = new Bll.EvApplicationEvents (
          this.Settings );
        //
        // If the action is set to delete the object.
        // 
        return _Bll_ApplicationEvents.AddEvent ( ApplicationEvent );

      }
      catch
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

    }//END AddEvent method



    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
  }//END class Method

}//END namespace
