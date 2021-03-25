/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\GlobalApplicationObjects.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c)  2002 - 2021  EVADO HOLDING PTY. LTD..  All rights reserved.
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
using Evado.Bll.Digital;
using Evado.Model.Digital;
// using Evado.Web;

namespace Evado.UniForm.Digital
{
  /// <summary>
  /// This class contains the session ResultData object
  /// </summary>
  [Serializable]
  public class EuGlobalObjects
  {
    #region Class Initialisation

    //===================================================================================
    /// <summary>
    /// This method initialises the class.
    /// </summary>
    //-----------------------------------------------------------------------------------
    public EuGlobalObjects ( )
    {
      this.ClassParameters = new EvClassParameters ( );
      this._ClassNameSpace = "Evado.UniForm.Digital.EuAdapterObjects.";
    }

    //===================================================================================
    /// <summary>
    /// This method initialises the class and passs in the user profile.
    /// </summary>
    //-----------------------------------------------------------------------------------
    public EuGlobalObjects (
      EvClassParameters ClassParameters )
    {
      this._ClassNameSpace = "Evado.UniForm.Digital.EuAdapterObjects.";
      this.ClassParameters = ClassParameters;

      this._LoggingLevel = this.ClassParameters.LoggingLevel;

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
      this.ClassParameters.LoggingLevel = this.LoggingLevel;

      this.LoadStaticEnvironmentalProperties ( );

      this.loadAdatperSettings ( );

      this.loadGlobalMenu ( );

      this.LoadEmailTemplates ( );

      this.loadSmtpServerProperties ( );

      this.LogMethodEnd ( "loadGlobalParameters" );

    }//END loadGlobalParameters method

    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
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

    private Evado.Model.Digital.EvClassParameters _ClassParameters = new Evado.Model.Digital.EvClassParameters ( );
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

      }
    }

    private Model.Digital.EdAdapterSettings _Settings = new Model.Digital.EdAdapterSettings ( );
    /// <summary>
    /// This property object contains the platform settings object.
    /// </summary>
    public Model.Digital.EdAdapterSettings Settings
    {
      get { return _Settings; }
      set { _Settings = value; }
    }

    #endregion

    #region Class Global properties and variables.

    // 
    // Constants
    //

    public const string Static_DefaultEventSource = "Application";


    Evado.UniForm.Digital.AssemblyAttributes _AssembyAttributes = new AssemblyAttributes ( );

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

    private List<EdPageLayout> _AllPageLayouts = new List<EdPageLayout> ( );
    /// <summary>
    /// this field list containt the all page Layouts.
    /// lists.
    /// </summary>
    public List<EdPageLayout> AllPageLayouts
    {
      get { return _AllPageLayouts; }
      set { _AllPageLayouts = value; }
    }

    /// <summary>
    /// this field list containt the currently loaded page Layouts. 
    /// lists.
    /// </summary>
    public List<EdPageLayout> PageLayouts
    {
      get
      {
        List<EdPageLayout> pageLayouts = new List<EdPageLayout> ( );

        foreach ( EdPageLayout layout in this._AllPageLayouts )
        {
          if ( layout.State == EdPageLayout.States.Issued )
          {
            pageLayouts.Add ( layout );
          }
        }

        return pageLayouts;
      }
    }

    // ==================================================================================
    /// <summary>
    /// This method returns a page layout object.
    /// </summary>
    /// <param name="PageLayoutId">String page layout identifier.</param>
    /// <returns>EdPageLayout object</return>
    // ----------------------------------------------------------------------------------
    public EdPageLayout getPageLayout ( String PageLayoutId )
    {
      //
      // iterate through the page layout lists.
      //
      foreach ( EdPageLayout pageLayout in this._AllPageLayouts )
      {
        if ( pageLayout.PageId.ToLower ( ) == PageLayoutId.ToLower ( ) )
        {
          return pageLayout;
        }
      }

      return null;
    }//END method

    private List<EdSelectionList> _SelectionLists = new List<EdSelectionList> ( );
    /// <summary>
    /// this field list containt the currently loaded external field selection lists.  Used by forms to fill coding
    /// lists.
    /// </summary>

    public List<EdSelectionList> AllSelectionLists
    {
      get { return _SelectionLists; }
      set { _SelectionLists = value; }
    }

    // ==================================================================================
    /// <summary>
    /// This method returns a list of options for selection list objects.
    /// </summary>
    /// <param name="forSelectionList">bool true for a selection list.</param>
    /// <returns>List of EvOption objects</return>
    // ----------------------------------------------------------------------------------
    public List<EvOption> getSelectionListOptions ( bool forSelectionList )
    {
      //
      // Initialise the methods variables and objects.
      //
      List<EvOption> optionList = new List<EvOption> ( );

      //
      // add an empty option for page selection list objects.
      //
      if ( forSelectionList == true )
      {
        optionList.Add ( new EvOption ( String.Empty ) );
      }

      //
      // iterate through the issues selection lists.
      //
      foreach ( EdSelectionList list in this._SelectionLists )
      {
        optionList.Add ( list.Option );
      }

      return optionList;
    }

    // ==================================================================================
    /// <summary>
    /// This method returns a list of options for selected Selection List.
    /// </summary>
    /// <param name="ListId">String: the list identifier.</param>
    /// <param name="Category">String: the list category.</param>
    /// <param name="CategoryOnly">bool true only return categories.</param>
    /// <returns>List of EvOption objects</return>
    // ----------------------------------------------------------------------------------
    public List<EvOption> getExternalSelectionOptions (
      String ListId,
      String Category,
      bool CategoryOnly )
    {
      //
      // Initialise the methods variables and objects.
      //
      List<EvOption> optionList = new List<EvOption> ( );
      string categories = String.Empty;

      //
      // iterate through the issues selection lists.
      //
      foreach ( EdSelectionList list in this._SelectionLists )
      {
        if ( list.ListId == ListId )
        {
          foreach ( EdSelectionList.Item item in list.Items )
          {
            if ( item.Value == String.Empty )
            {
              continue;
            }

            //
            // if category only is selected the only return the categories as a list.
            //
            if ( CategoryOnly == true )
            {
              if ( item.Category != String.Empty )
              {
                if ( categories.Contains ( item.Category ) == false )
                {
                  optionList.Add ( new EvOption ( item.Category ) );
                }
              }
              continue;
            }

            if ( Category == String.Empty )
            {
              optionList.Add ( item.GetOption ( true ) );
            }
            else if ( item.Category == Category )
            {
              optionList.Add ( item.GetOption ( false ) );
            }

          }// List item iteration loop
        }//END list selection
      }//Selection list iteration loop

      //
      // Return the selection list's options.
      //
      return optionList;

    }//END method.

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

    /// <summary>
    /// This property object contains a list of the web application menu item object..
    /// </summary>
    public List<EvMenuItem> MenuList { get; set; }

    // ==================================================================================
    /// <summary>
    /// This method returns a list of options for menu groups
    /// </summary>
    /// <returns>List of EvOption objects</return>
    // ----------------------------------------------------------------------------------
    public List<EvOption> getMenuSelectionList (  )
    {
      //
      // Initialise the methods variables and objects.
      //
      List<EvOption> optionList = new List<EvOption> ( );
      optionList.Add ( new EvOption ( String.Empty ) );
      String menuList = String.Empty ;

      //
      // iterate through the issues selection lists.
      //
      foreach ( EvMenuItem item in this.MenuList )
      {
        if ( item.GroupHeader == false )
        {
          continue;
        }

        if ( item.Group == EvMenuItem.CONST_MENU_STATIC_ADMIN_GROUP_ID
          || item.Group == EvMenuItem.CONST_MENU_STATIC_MANAGEMENT_GROUP_ID
          || item.Group == EvMenuItem.CONST_MENU_STATIC_RECORD_GROUP_ID
          || item.Group == EvMenuItem.CONST_MENU_STATIC_ANALYSIS_GROUP_ID
          || item.Group == EvMenuItem.CONST_MENU_AUDIT_GROUP_ID )
        {
          if ( menuList.Contains ( EvMenuItem.CONST_MAIN_MENU_ID ) == false )
          {
            optionList.Add ( new EvOption ( EvMenuItem.CONST_MAIN_MENU_ID, "Main Menu" ) );
            menuList += EvMenuItem.CONST_MAIN_MENU_ID + ";";
          }
        }


        if ( menuList.Contains ( item.Group ) == false )
        {
          optionList.Add ( new EvOption ( item.Group, item.Title ) );
          menuList += item.Group + ";";
        }
     }//END menu item iteration loop.

      return optionList;
    }//END menu selection method.

    // ==================================================================================
    /// <summary>
    /// This method returns a list of menu item for menu selection
    /// </summary>
    /// <returns>List of EvOption objects</return>
    // ----------------------------------------------------------------------------------
    public List<EvMenuItem> getMenuGroups ( String SelectedMenu )
    {
      //
      // Initialise the methods variables and objects.
      //
      List<EvMenuItem> menuList = new List<EvMenuItem> ( );

      //
      // iterate through the issues selection lists.
      //
      foreach ( EvMenuItem item in this.MenuList )
      {
        if ( item.GroupHeader == false )
        {
          continue;
        }

        //
        // generate the menu menu list.
        //
        if ( SelectedMenu == EvMenuItem.CONST_MAIN_MENU_ID )
        {
          if ( item.Group == EvMenuItem.CONST_MENU_STATIC_ADMIN_GROUP_ID
            || item.Group == EvMenuItem.CONST_MENU_STATIC_MANAGEMENT_GROUP_ID
            || item.Group == EvMenuItem.CONST_MENU_STATIC_RECORD_GROUP_ID
            || item.Group == EvMenuItem.CONST_MENU_STATIC_ANALYSIS_GROUP_ID
            || item.Group == EvMenuItem.CONST_MENU_AUDIT_GROUP_ID )
          {
            menuList.Add ( item );
          }
          continue;
        }

        //
        // generate the selecte menu group menuitem list.
        //
        if ( item.Group == SelectedMenu )
        {
          menuList.Add ( item );
        }
      }//END menu item iteration loop.

      return menuList;
    }//END menu selection method.

    /// <summary>
    /// This property contains a list of the adapters navigation commands.
    /// </summary>
    public List<EvOption> PageIdentifiers { get; set; }

    /// <summary>
    /// This property contains a list of the adapters compnent objects.
    /// Selecting a component will then execute a generation process to build the page.
    /// </summary>
    public List<EvOption> PageComponents { get; set; }

    /// <summary>
    /// This property object contains a list of organisations. 
    /// </summary>
    public List<EdOrganisation> OrganisationList { get; set; }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Entity object section

    private List<EdRecord> _AllEntityLayouts = new List<EdRecord> ( );
    /// <summary>
    /// This property object contains a list of entitys in the application
    /// </summary>
    public List<EdRecord> AllEntityLayouts
    {
      get { return this._AllEntityLayouts; }
      set
      {
        this._AllEntityLayouts = value;

        //
        // Create the entiy parent list.
        //
        this.createEntityParents ( );
      }
    }


    List<EdObjectParent> _EntityParents = new List<EdObjectParent> ( );
    // ==================================================================================
    /// <summary>
    /// This private method creates the entity parent list.
    /// </summary>
    // ----------------------------------------------------------------------------------
    private void createEntityParents ( )
    {
      this.LogInitMethod ( "createEntityParents" );

      _EntityParents = new List<EdObjectParent> ( );

      //
      // Iterate through the entity list.
      //
      foreach ( EdRecord entity in this._AllEntityLayouts )
      {

        this.LogInitValue ( "L: " + entity.LayoutId + ",  T: " + entity.Title + ", R" + entity.Design.EditAccessRoles );


        string [ ] parentEntities = entity.Design.ParentEntities.Split ( ';' );

        foreach ( string parent in parentEntities )
        {
          this._EntityParents.Add ( new EdObjectParent (
            parent, entity.LayoutId, entity.Title, entity.Design.EditAccessRoles ) );

        }//END parent iteration loop
      }//End entity iteration loop
    }//ENd method

    // ==================================================================================
    /// <summary>
    /// This method returns a list of string containing child layouts.
    /// </summary>
    /// <param name="LayoutId">String parent LayoutID</param>
    /// <returns>List of String containing child LaoutIds</returns>
    // ----------------------------------------------------------------------------------
    public EdRecord GetEntityLayout ( String LayoutId )
    {
      //
      // iterate through the list of entity and return the issued entity layout.
      //
      foreach ( EdRecord entity in this._AllEntityLayouts )
      {
        if ( entity.LayoutId == LayoutId
          && entity.State == EdRecordObjectStates.Form_Issued )
        {
          return entity;
        }
      }

      //
      // return null if not found.
      //
      return null;

    }//END method

    // ==================================================================================
    /// <summary>
    /// This method returns a list of string containing child layouts.
    /// </summary>
    /// <param name="ParentLayoutId">String parent LayoutID</param>
    /// <returns>List of String containing child LaoutIds</returns>
    // ----------------------------------------------------------------------------------
    public List<EdObjectParent> GetEntityChildren ( String ParentLayoutId )
    {
      List<EdObjectParent> childrenLayouts = new List<EdObjectParent> ( );

      //
      // iterate through the list of entity parents to retriee the 
      // children for a PatentLayout.
      //
      foreach ( EdObjectParent entity in this._EntityParents )
      {
        if ( entity.ParentLayoutId == ParentLayoutId )
        {
          childrenLayouts.Add ( entity );
        }
      }
      //
      // return the list of children.
      //
      return childrenLayouts;
    }

    /// <summary>
    /// This property contains the list of entity parents  relationships
    /// </summary>
    public List<EdObjectParent> EntityParents
    {
      get
      {
        return this._EntityParents;
      }
    }

    /// <summary>
    /// This property object contains a list of form templates 
    /// </summary>
    public List<EdRecord> IssuedEntityLayouts
    {
      get
      {
        List<EdRecord> recordLayoutList = new List<EdRecord> ( );

        foreach ( EdRecord layout in _AllEntityLayouts )
        {
          if ( layout.State == EdRecordObjectStates.Form_Issued )
          {
            recordLayoutList.Add ( layout );
          }
        }

        return recordLayoutList;
      }
    }

    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Record object section

    private List<EdRecord> _AllRecordLayoutList = new List<EdRecord> ( );
    /// <summary>
    /// This property object contains a list of form templates 
    /// </summary>
    public List<EdRecord> AllRecordLayouts
    {
      get { return _AllRecordLayoutList; }
      set { _AllRecordLayoutList = value; }
    }

    /// <summary>
    /// This property object contains a list of form templates 
    /// </summary>
    public List<EdRecord> IssuedRecordLayouts
    {
      get
      {
        List<EdRecord> recordLayoutList = new List<EdRecord> ( );

        foreach ( EdRecord layout in _AllRecordLayoutList )
        {
          if ( layout.State == EdRecordObjectStates.Form_Issued )
          {
            recordLayoutList.Add ( layout );
          }
        }

        return recordLayoutList;
      }
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


    ///  =======================================================================================
    /// <summary>
    /// This method loads the global menu objects
    /// </summary>
    //  ---------------------------------------------------------------------------------
    public void loadGlobalMenu ( )
    {
      this.LogMethod ( "loadGlobalMenu method" );
      this.LogDebug ( "PlatformId: " + this._PlatformId );

      try
      {
        // 
        // Initialse the methods objects and variables.
        // 
        EvMenus bll_Menu = new EvMenus ( this.ClassParameters );

        // 
        // Get the site setMenu.
        // 
        this.MenuList = bll_Menu.getView ( this._PlatformId, String.Empty );

        this.LogDebugClass ( bll_Menu.Log );
        this.LogInitValue ( "MenuList .Count: " + this.MenuList.Count );
        /*
        foreach ( EvMenuItem itm in this.MenuList )
        {
          this.LogDebug ( "PageId: {0}, Group {1}, Roles {2}: ", itm.PageId, itm.Group, itm.RoleList );
        }
        */
      }
      catch ( Exception Ex )
      {
        this.LogException ( Ex );
      }

      this.LogMethodEnd ( "loadGlobalMenu" );
    }//END loadGlobalMenu method

    ///  =======================================================================================
    /// <summary>
    /// Description:
    ///  Load the site properties object.
    /// 
    /// </summary>
    //  ---------------------------------------------------------------------------------
    private void loadAdatperSettings ( )
    {
      this.LogMethod ( "loadAdatperSettings method" );
      // 
      // Load the Web Site Properties
      // 
      Evado.Bll.Digital.EdAdapterConfig adapterConfig = new Evado.Bll.Digital.EdAdapterConfig ( );

      if ( this.ClassParameters != null )
      {
        adapterConfig = new Evado.Bll.Digital.EdAdapterConfig ( this.ClassParameters );
      }

      //
      // Get the Application profile.
      //
      this._Settings = adapterConfig.getItem ( "A" );

      this.LogDebugClass ( adapterConfig.Log );

      if ( ConfigurationManager.AppSettings [ EvcStatics.CONFIG_WEB_CLIENT_URL_KEY ] != null )
      {
        string value = ConfigurationManager.AppSettings [ EvcStatics.CONFIG_WEB_CLIENT_URL_KEY ];
        if ( value != String.Empty )
        {
          this._ApplicationUrl = value;
        }
      }

      this.LogDebug ( "ApplicationUrl: " + this._ApplicationUrl );

      if ( ConfigurationManager.AppSettings [ EuAdapter.CONFIG_PASSWORD_RESET_URL_KEY ] != null )
      {
        string value = ConfigurationManager.AppSettings [ EuAdapter.CONFIG_PASSWORD_RESET_URL_KEY ];
        if ( value != String.Empty )
        {
          this._PasswordResetUrl = value;
        }
      }
      this.LogDebug ( "PasswordResetUrl: " + this._PasswordResetUrl );

      if ( ConfigurationManager.AppSettings [ EuAdapter.CONFIG_SUPPORT_EMAIL_ADDRESS_KEY ] != null )
      {
        string value = ConfigurationManager.AppSettings [ EuAdapter.CONFIG_SUPPORT_EMAIL_ADDRESS_KEY ];
        if ( value != String.Empty )
        {
          this._SupportEmailAddress = value;
        }
      }
      this.LogDebug ( "SupportEmailAddress: " + this._SupportEmailAddress );

      if ( ConfigurationManager.AppSettings [ EuAdapter.CONFIG_NOREPLY_EMAIL_ADDRESS_KEY ] != null )
      {
        string value = ConfigurationManager.AppSettings [ EuAdapter.CONFIG_NOREPLY_EMAIL_ADDRESS_KEY ];
        if ( value != String.Empty )
        {
          this._NoReplyEmailAddress = value;
        }
      }
      this.LogDebug ( "NoReplyEmailAddress: " + this._NoReplyEmailAddress );

      this.LogDebug ( "Full version: " + _AssembyAttributes.FullVersion );

      this._Settings.Version = _AssembyAttributes.FullVersion;
      this._Settings.MinorVersion = _AssembyAttributes.MinorVersion;

      //
      // Set the Stide GUid used by encryption module.
      //
      ConfigurationManager.AppSettings [ "SiteGuid" ] = this._Settings.Guid.ToString ( );

      // 
      // Log the Site Properties on startup.
      // 
      this.LogDebug ( "Version: " + this._Settings.Version );

      this.LogMethodEnd ( "loadAdatperSettings" );
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

      if ( ConfigurationManager.AppSettings [ Evado.Model.Digital.EvcStatics.CONFIG_PATFORM_ID_KEY ] != null )
      {
        this._PlatformId = (string) ConfigurationManager.AppSettings [ Evado.Model.Digital.EvcStatics.CONFIG_PATFORM_ID_KEY ];
      }

      this.LogDebug ( "PlatformId: " + this._PlatformId );

      // 
      // Set the connection string settings.
      // 
      if ( ConfigurationManager.AppSettings [ Evado.Model.Digital.EvcStatics.CONFIG_HELP_URL_KEY ] != null )
      {
        this._HelpUrl = (String) ConfigurationManager.AppSettings [ Evado.Model.Digital.EvcStatics.CONFIG_HELP_URL_KEY ];

      }

      this.LogDebug ( "HelpUrl: '" + this._HelpUrl + "'" );

      // 
      // Log the Maximum selection list length on startup.
      // 
      this.LogDebug ( "MaximumSelectionListLength: " + this._Settings.MaximumSelectionListLength );

      this.LogMethodEnd ( "LoadStaticEnvironmentalProperties" );
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
        this.LogDebug ( "IntroductoryEmail_Title: " + this.ContentTemplates.IntroductoryEmail_Title );
        this.LogDebug ( "UpdatePasswordEmail_Title: " + this.ContentTemplates.UpdatePasswordEmail_Title );
        this.LogDebug ( "ResetPasswordEmail_Title: " + this.ContentTemplates.ResetPasswordEmail_Title );
        this.LogDebug ( "PasswordConfirmationEmail_Title: " + this.ContentTemplates.PasswordConfirmationEmail_Title );
      }

      this.LogMethodEnd ( "LoadEmailTemplates" );
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
      // Load the SMTP server properties is not loadedd.
      //
      if ( this._Settings.SmtpServer == String.Empty )
      {
        if ( ConfigurationManager.AppSettings [ Evado.Model.Digital.EvcStatics.CONFIG_SMTP_SERVER_KEY ] != null )
        {
          this._Settings.SmtpServer = (string) ConfigurationManager.AppSettings [ Evado.Model.Digital.EvcStatics.CONFIG_SMTP_SERVER_KEY ];
        }
        if ( ConfigurationManager.AppSettings [ Evado.Model.Digital.EvcStatics.CONFIG_SMTP_SERVER_PORT_KEY ] != null )
        {
          this._Settings.SmtpServerPort =
            EvStatics.getInteger ( ConfigurationManager.AppSettings [ Evado.Model.Digital.EvcStatics.CONFIG_SMTP_SERVER_PORT_KEY ] );
        }
        if ( ConfigurationManager.AppSettings [ Evado.Model.Digital.EvcStatics.CONFIG_SMTP_USER_KEY ] != null )
        {
          this._Settings.SmtpUserId = (string) ConfigurationManager.AppSettings [ Evado.Model.Digital.EvcStatics.CONFIG_SMTP_USER_KEY ];
        }
        if ( ConfigurationManager.AppSettings [ Evado.Model.Digital.EvcStatics.CONFIG_SMTP_USER_PASSWORD_KEY ] != null )
        {
          this._Settings.SmtpPassword = (string) ConfigurationManager.AppSettings [ Evado.Model.Digital.EvcStatics.CONFIG_SMTP_USER_PASSWORD_KEY ];
        }

      }//END no SMTP server.

      // 
      // Log the SMTP server.
      // 
      this.LogDebug ( " SmtpServer: " + this._Settings.SmtpServer );

      // 
      // Log the SMTP port setting
      // 
      this.LogDebug ( " SmtpServerPort: " + this._Settings.SmtpServerPort );

      // 
      // Log the SMTP User Id.
      // 
      this.LogDebug ( " SmtpUserId: " + this._Settings.SmtpUserId );

      // 
      // Log the SMTP user password.
      // 
      this.LogDebug ( " SmtpPassword: " + this._Settings.SmtpPassword );

      // 
      // Log the SMTP User Id.
      // 
      this.LogDebug ( " SmtpUserId: " + this._Settings.SmtpUserId );

      // 
      // Log the SMTP user password.
      // 
      this.LogDebug ( " EmailAlertTestAddress: " + this._Settings.EmailAlertTestAddress );


      this.LogMethodEnd ( "loadSmtpServerProperties" );
    }//END setSmtpServerProperties method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
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
        this.ClassParameters.LoggingLevel = LoggingEventLevel;

        if ( this.LoggingLevel < 1 )
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
      Evado.Model.Digital.EdUserProfile User )
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
        this.ClassParameters.UserProfile.CommonName );

      // this.AddEvent ( applicationEvent );

      //
      // Log the event to the application log.
      //
      this.LogDebug ( stEvent );

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
      Evado.Model.Digital.EdUserProfile User )
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
        this.ClassParameters.UserProfile.CommonName );

      // this.AddEvent ( applicationEvent );

      //
      // Log the event to the application log.
      //
      this.LogDebug ( stEvent );

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
      Evado.Model.Digital.EdUserProfile User )
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
        this.ClassParameters.UserProfile.CommonName );

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
        this.ClassParameters.UserProfile.CommonName );

      // this.AddEvent ( applicationEvent );

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
      EvApplicationEvent applicationEvent = new EvApplicationEvent (
        EvApplicationEvent.EventType.Error,
        EventId,
        this._ClassNameSpace,
        Description,
        this.ClassParameters.UserProfile.CommonName );

      // this.AddEvent ( applicationEvent );

      if ( this._ClassParameters.LoggingLevel >= 0 )
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
    protected void LogDebug ( String Value )
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
    protected void LogDebug ( String Format, params object [ ] args )
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
    protected void LogDebugClass ( String Value )
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
          this.ClassParameters );
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
