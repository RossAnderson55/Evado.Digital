/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\Menus.cs" company="EVADO HOLDING PTY. LTD.">
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
using System.Collections;
using System.Text;
using System.Web;
using System.Web.SessionState;

using Evado.Bll;
using Evado.Model;
using Evado.Bll.Clinical;
using Evado.Model.Digital;

namespace Evado.UniForm.Clinical
{
  /// <summary>
  /// This class defines the uniform interface object for handling menus.
  /// 
  /// </summary>
  public partial class EuMenus : EuClassAdapterBase
  {
    #region Class Initialisation
    /// <summary>
    /// This method initialises the class.
    /// </summary>
    public EuMenus ( )
    {
      this.ClassNameSpace = "Evado.UniForm.Clinical.EuMenus.";
    }

    /// <summary>
    /// This method initialises the class and passs in the user profile.
    /// </summary>
    public EuMenus (
      EuApplicationObjects ApplicationObject,
      EvUserProfileBase ServiceUserProfile,
      EuSession SessionObjects,
      String UniFormBinaryFilePath,
      EvClassParameters Settings )
    {
      this.ClassNameSpace = "Evado.UniForm.Clinical.EuMenus.";
      this.ApplicationObjects = ApplicationObject;
      this.ServiceUserProfile = ServiceUserProfile;
      this.Session = SessionObjects;
      this.UniForm_BinaryFilePath = UniFormBinaryFilePath;
      this.ClassParameters = Settings;


      this.LogInitMethod ( "Menu initialisation" );
      this.LogInit ( "ServiceUserProfile.UserId: " + ServiceUserProfile.UserId );
      this.LogInit ( "SessionObjects.UserProfile.Userid: " + this.Session.UserProfile.UserId );
      this.LogInit ( "SessionObjects.UserProfile.CommonName: " + this.Session.UserProfile.CommonName );
      this.LogInit ( "UniFormBinaryFilePath: " + this.UniForm_BinaryFilePath );

      this._Bll_Menus = new EvMenus ( this.ClassParameters );

    }//END Method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class constants and variables.

    private const String CONST_DELETE_ACTION = "DELETE";

    private const String CONST_ADMIN_APPLICATION_IDENTIFIER = "ADMIN";
    private const String CONST_PRODUCTION_APPLICATION_IDENTIFIER = "PROD";

    private EvMenus _Bll_Menus = new EvMenus ( );

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class public methods

    // ==================================================================================
    /// <summary>
    /// This method gets the application object from the list.
    /// 
    /// </summary>
    /// <param name="PageCommand">ClientPateEvado.Model.UniForm.Command object</param>
    /// <returns>ClientApplicationData</returns>
    //  ----------------------------------------------------------------------------------
    public Evado.Model.UniForm.AppData getClientDataObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getClientDataObject" );
      this.LogValue ( "Parameter PageCommand " + PageCommand.getAsString ( false, true ) );

      try
      {
        // 
        // Initialise the methods variables and objects.
        // 
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );

        //
        // Initialise the menu platformID value.
        //
        if ( this.Session.MenuPlatformId == null )
        {
          this.Session.MenuPlatformId = this.ApplicationObjects.PlatformId;
        }

        // 
        // Determine the method to be called
        // 
        switch ( PageCommand.Method )
        {
          case Evado.Model.UniForm.ApplicationMethods.List_of_Objects:
            {
              clientDataObject = this.getListObject ( PageCommand );
              break;
            }
          case Evado.Model.UniForm.ApplicationMethods.Get_Object:
            {
              clientDataObject = this.getObject ( PageCommand );
              break;
            }
          case Evado.Model.UniForm.ApplicationMethods.Create_Object:
            {
              clientDataObject = this.createObject ( PageCommand );
              break;
            }
          case Evado.Model.UniForm.ApplicationMethods.Save_Object:
          case Evado.Model.UniForm.ApplicationMethods.Delete_Object:
            {
              clientDataObject = this.updateObject ( PageCommand );
              break;
            }

        }//END Switch
        // 
        // Handle returned exceptions.
        // 
        if ( clientDataObject == null )
        {
          this.LogDebug ( " null application data returned." );
          clientDataObject = this.Session.LastPage;
        }

        //
        // If an errot message exist display it.
        //
        if ( this.ErrorMessage != String.Empty )
        {
          clientDataObject.Message = this.ErrorMessage;
        }

        // 
        // return the client ResultData object.
        // 
        this.LogMethodEnd ( "getDataObject" );
        return clientDataObject;
      }
      catch ( Exception Ex )
      {
        this.LogException ( Ex );
      }
      this.LogMethodEnd ( "getDataObject" );
      return this.Session.LastPage;

    }//END getClientDataObject method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class private list methods

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData getListObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getListObject" );
      this.LogDebug ( "MenuPlatformId: " + this.Session.MenuPlatformId );
      this.LogDebug ( "MenuGroupIdentifer: " + this.Session.MenuGroupIdentifier );
      try
      {
        // 
        // Initialise the methods variables and objects.
        //      
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );


        //
        // Determine if the user has access to this page and log and error if they do not.
        //
        if ( this.Session.UserProfile.hasAdministrationAccess == false )
        {
          this.LogIllegalAccess (
            this.ClassNameSpace + "getListObject",
            this.Session.UserProfile );

          this.ErrorMessage = EvLabels.Illegal_Page_Access_Attempt;

          return this.Session.LastPage;
        }

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "getListObject",
          this.Session.UserProfile );

        //
        // if the customer command has been executed then refresh the menu list.
        //
        if ( PageCommand.hasParameter ( Model.UniForm.CommandParameters.Custom_Method ) == true )
        {
          this.ApplicationObjects.MenuList = new List<EvMenuItem> ( );
        }

        // 
        // Get the selected pageMenuGroup identifier.
        // 
        if ( PageCommand.hasParameter ( EuSession.CONST_MENU_GROUP_ID ) == true )
        {
          this.Session.MenuGroupIdentifier = PageCommand.GetParameter ( EuSession.CONST_MENU_GROUP_ID );
        }
        this.LogValue ( "UPDATED: MenuGroupIdentifer: " + this.Session.MenuGroupIdentifier );

        // 
        // Get the selected pageMenuGroup identifier.
        // 
        if ( PageCommand.hasParameter ( EvMenuItem.MenuFieldNames.Platform.ToString() ) == true )
        {
          this.Session.MenuPlatformId = PageCommand.GetParameter ( EvMenuItem.MenuFieldNames.Platform.ToString ( ) );
        }
        this.LogValue ( "UPDATED: MenuPlatformId: " + this.Session.MenuPlatformId );

        //
        // Update the global menu list, to pick up any changes that where made during an audit cycle.
        //
        this.loadGlobalMenu ( );

        //
        // Initialise the ResultData object.
        //
        clientDataObject.Title = EvLabels.Menu_Item_List;
        clientDataObject.Page.Title = clientDataObject.Title;
        clientDataObject.Id = Guid.NewGuid ( );

        // 
        // Add the save groupCommand
        // 
        clientDataObject.Page.addCommand (
          EvLabels.Menu_New_Item_Command_Title,
          EuAdapter.APPLICATION_ID,
          EuAdapterClasses.Menu.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Create_Object );

        // 
        // add the selection pageMenuGroup.
        // 
        this.getSelectionListGroup ( clientDataObject.Page );

        // 
        // Add the Menu Field list to the page.
        // 
        this.getList_Menu_List_Group (
          clientDataObject.Page,
          this.Session.MenuGroupIdentifier );

        this.LogDebug ( "Page.Title: " + clientDataObject.Page.Title );
        this.LogDebug ( "Page.CommandList.Count: " + clientDataObject.Page.CommandList.Count );

        //
        // Return the client ResultData object.
        //
        return clientDataObject;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Menu_List_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return this.Session.LastPage;

    }//END getListObject method.


    ///  =======================================================================================
    /// <summary>
    /// This method loades the global menu objects
    /// </summary>
    //  ---------------------------------------------------------------------------------
    public void loadGlobalMenu ( )
    {
      this.LogMethod ( "loadGlobalMenu method" );
      this.LogDebug ( "PlatformId: " + this.ClassParameters.PlatformId );
      try
      {
        EvMenus _Bll_Menus = new EvMenus ( this.ClassParameters );

        if ( this.ApplicationObjects.MenuList.Count > 0)
        {
          this.LogMethodEnd ( "loadGlobalMenu" );
          return;
        }
        // 
        // Initialse the methods objects and variables.
        // 
        foreach ( EvModuleCodes str in this.ApplicationObjects.ApplicationSettings.LoadedModuleList )
        {
          this.LogDebug ( "- " + str );
        }

        // 
        // Get the site setMenu.
        // 
        this.ApplicationObjects.MenuList = _Bll_Menus.getView ( this.ClassParameters.PlatformId, String.Empty );

        this.LogDebugClass ( "menus.DebugLog: " + _Bll_Menus.Log );
        this.LogValue ( "GlobalMenuList.Count: " + this.ApplicationObjects.MenuList.Count );

        // 
        // Process menu items.
        // 
        for ( int count = 0; count < this.ApplicationObjects.MenuList.Count; count++ )
        {
          EvMenuItem menuItem = this.ApplicationObjects.MenuList [ count ];

          this.LogDebug ( "Group: {0}, PageId: {1}, Title: {2}, Roles: {3}", menuItem.Group, menuItem.PageId, menuItem.Title, menuItem.Modules );
          //
          // Load the menu items that are associated with the loaded modules.
          //
          if ( this.ApplicationObjects.ApplicationSettings.hasModule ( menuItem.ModuleList ) == false )
          {
            this.LogDebug ( "REMOVE: Group: {0}, PageId: {1}, Title: {2}, Roles: {3}", menuItem.Group, menuItem.PageId, menuItem.Title, menuItem.Modules );
            //
            // Remove the menu item from the list.
            //
            this.ApplicationObjects.MenuList.RemoveAt ( count );
            count--;
            continue;
          }

        }//END menuitem iteration loop.

      }
      catch ( Exception Ex )
      {
        this.LogDebug ( "loadGlobalMenu exception:  " + Evado.Model.Digital.EvcStatics.getException ( Ex ) );
      }
      this.LogDebug ( "Global Menu list count: " + this.ApplicationObjects.MenuList.Count );

      this.LogMethodEnd ( "loadGlobalMenu" );
    }//END loadMenu method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private void getSelectionListGroup ( Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getSelectionListGroup" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Field pageField = new Evado.Model.UniForm.Field ( );
      List<EvOption> optionList = new List<EvOption> ( );
      EvOption option = new EvOption ( );

      //
      // Define the selection group object.
      //
      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
        EvLabels.Menu_Selection,
        Evado.Model.UniForm.EditAccess.Enabled );
        pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;

      //
      // Add the platform selection list
      //
      optionList.Add ( new EvOption ( EuMenus.CONST_ADMIN_APPLICATION_IDENTIFIER, "Administration Platform" ) );
      optionList.Add ( new EvOption ( EuMenus.CONST_PRODUCTION_APPLICATION_IDENTIFIER, "Production Platform" ) );

      pageField = pageGroup.createSelectionListField (
        EvMenuItem.MenuFieldNames.Platform.ToString(),
        "Platform identifier: ",
        this.Session.MenuPlatformId,
        optionList );
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;
      pageField.AddParameter ( Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );
      
      // 
      // Create the pageMenuGroup selection list.
      // 
      List<Evado.Model.EvOption> groupList = this._Bll_Menus.getGroupList (
        this.Session.MenuPlatformId );

      pageField = pageGroup.createSelectionListField (
        EuSession.CONST_MENU_GROUP_ID,
        EvLabels.Menu_Group_ID,
        this.Session.MenuGroupIdentifier,
        groupList );
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;
      pageField.AddParameter ( Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );

      //
      // Add the selection command
      //
      Evado.Model.UniForm.Command selectionCommand = pageGroup.addCommand (
        EvLabels.Menu_Selection_Menu,
        EuAdapter.APPLICATION_ID,
        EuAdapterClasses.Menu.ToString ( ),
        Model.UniForm.ApplicationMethods.Custom_Method );

      // 
      // Set the custom groupCommand parametet.
      // 
      selectionCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.List_of_Objects );

    }

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    /// <param name="GroupId">string: group identifier.</param>    
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private void getList_Menu_List_Group (
      Evado.Model.UniForm.Page PageObject,
      String GroupId )
    {
      this.LogMethod ( "getListGroup" );
      this.LogValue ( "GroupId: " + GroupId );
      try
      {
        //
        // initialise the methods variables and objects.
        //
        List<EvMenuItem> menuList = new List<EvMenuItem> ( );
        Evado.Model.UniForm.Command groupCommand = new Model.UniForm.Command ( );

        //
        // Create the list pageMenuGroup object.
        //
        Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
         EvLabels.Menu_Item_List,
          Evado.Model.UniForm.EditAccess.Inherited_Access );

        pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
        pageGroup.CmdLayout = Evado.Model.UniForm.GroupCommandListLayouts.Vertical_Orientation;
        pageGroup.Title = EvLabels.Menu_Item_List;

        //
        // Display a message if the pageMenuGroup has not been selected.
        //
        if ( GroupId == String.Empty )
        {
          pageGroup.Description = EvLabels.Menu_List_Group_Not_Selected_Message ;

          return;
        }

        // 
        // Add the save groupCommand
        // 
        groupCommand = pageGroup.addCommand (
          EvLabels.Menu_New_Item_Command_Title,
          EuAdapter.APPLICATION_ID,
          EuAdapterClasses.Menu.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Create_Object );

        groupCommand.SetBackgroundDefaultColour ( Model.UniForm.Background_Colours.Purple );

        // 
        // get the list of customers.
        // 
        foreach ( EvMenuItem item in this.ApplicationObjects.MenuList )
        {
          if ( item.Group.ToLower ( ) == GroupId.ToLower ( ) )
          {
            menuList.Add ( item );
          }
        }

        // 
        // generate the page links.
        // 
        foreach ( EvMenuItem menuItem in menuList )
        {
          this.LogValue ( "P: " + menuItem.PageId + ", N: " + menuItem.Title + ", M: " + menuItem.Modules + ", R: " + menuItem.RoleList );

          System.Text.StringBuilder sbTitle = new StringBuilder ( );
          sbTitle.Append ( Evado.Model.Digital.EvcStatics.Enumerations.enumValueToString ( menuItem.PageId )
           + EvLabels.Space_Hypen + menuItem.Title
           + EvLabels.Space_Arrow_Right
           + EvLabels.Menu_List_Order_Label
           + menuItem.Order
           + EvLabels.Space_Arrow_Right
           + EvLabels.Menu_List_Modules_Label );

          int startIndex = sbTitle.ToString ( ).Length;

          if ( menuItem.hasModule ( EvModuleCodes.All_Modules ) == true )
          {
            sbTitle.Append ( EvLabels.Menu_List_All_Modules_Label );
            sbTitle.Append ( EvLabels.Space_Coma );
          }
          else
          {
            if ( menuItem.hasModule ( EvModuleCodes.Clinical_Module ) == true )
            {
              sbTitle.Append ( EvLabels.Menu_List_Clinical_Modules_Label );
              sbTitle.Append ( EvLabels.Space_Coma );
            }

            if ( menuItem.hasModule ( EvModuleCodes.Registry_Module ) == true )
            {
              sbTitle.Append ( EvLabels.Menu_List_Registry_Modules_Label );
              sbTitle.Append ( EvLabels.Space_Coma );
            }

            if ( menuItem.hasModule ( EvModuleCodes.Patient_Module ) == true
              || menuItem.hasModule ( EvModuleCodes.Patient_Recorded_Outcomes ) == true )
            {
              sbTitle.Append ( EvLabels.Menu_List_Patient_Modules_Label );
              sbTitle.Append ( EvLabels.Space_Coma );
            }
          }

          sbTitle.Append ( EvLabels.Space_Arrow_Right
          + EvLabels.Menu_List_Role_Label );


          if ( menuItem.hasRole ( EvRoleList.Administrator ) == true )
          {
            sbTitle.Append ( EvLabels.Menu_List_Administrator_Label );
            sbTitle.Append ( EvLabels.Space_Coma );
          }

          if ( menuItem.hasRole ( EvRoleList.Trial_Manager ) == true )
          {
            sbTitle.Append ( EvLabels.Menu_List_Project_Manager_Label );
            sbTitle.Append ( EvLabels.Space_Coma );
          }

          if ( menuItem.hasRole ( EvRoleList.Trial_Coordinator ) == true )
          {
            sbTitle.Append ( EvLabels.Menu_List_Project_Coordinator_Label );
            sbTitle.Append ( EvLabels.Space_Coma );
          }

          if ( menuItem.hasRole ( EvRoleList.Site_User ) == true
            || menuItem.hasRole ( EvRoleList.Investigator ) == true )
          {
            sbTitle.Append ( EvLabels.Menu_List_Record_Author_Label );
            sbTitle.Append ( EvLabels.Space_Coma );
          }

          if ( menuItem.hasRole ( EvRoleList.Monitor ) == true
            || menuItem.hasRole ( EvRoleList.Data_Manager ) == true )
          {
            sbTitle.Append ( EvLabels.Menu_List_Monitor_Label );
            sbTitle.Append ( EvLabels.Space_Coma );
          }

          if ( menuItem.hasRole ( EvRoleList.Sponsor ) == true )
          {
            sbTitle.Append ( EvLabels.Menu_List_Sponsor_Label );
            sbTitle.Append ( EvLabels.Space_Coma );
          }

          String stTitle = sbTitle.ToString ( );

          if ( startIndex < stTitle.Length - 1 )
          {
            stTitle = stTitle.Substring ( 0, stTitle.Length - 2 );
          }

          // 
          // Add the Menu Field to the list of organisations as a groupCommand.
          // 
          groupCommand = pageGroup.addCommand ( stTitle,
            EuAdapter.APPLICATION_ID,
            EuAdapterClasses.Menu.ToString ( ),
            Evado.Model.UniForm.ApplicationMethods.Get_Object );

          groupCommand.SetGuid ( menuItem.Guid );

        }//END Menu Field list iteration loop


        this.LogValue ( " command object count: " + pageGroup.CommandList.Count );

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = "Error raised when generating a list of trial sites.";

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

    }//END getListObject method.

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class private get object methods

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData getObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getObject" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
      Guid menuGuid = Guid.Empty;
  
      try
      {
        //
        // Determine if the user has access to this page and log and error if they do not.
        //
        if ( this.Session.UserProfile.hasAdministrationAccess == false )
        {
          this.LogIllegalAccess (
            this.ClassNameSpace + "getObject",
            this.Session.UserProfile );

          this.ErrorMessage = EvLabels.Illegal_Page_Access_Attempt;

          return this.Session.LastPage;
        }

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "getObject",
          this.Session.UserProfile );

        // 
        // if the parameter value exists then set the customerId
        // 
        menuGuid = PageCommand.GetGuid ( );
        this.LogDebug ( "Menu Guid: " + menuGuid );

        // 
        // return if not trial id
        // 
        if ( menuGuid == Guid.Empty )
        {
          this.ErrorMessage = EvLabels.MenuItem_Guid_Empty_Message;

          return this.Session.LastPage;
        }
        //
        // Retrieve the menu item from the global list of menut items.
        //
        this.Session.MenuItem = this.getMenuItem ( menuGuid );

        this.LogDebug ( "MenuItem.PageId: " + this.Session.MenuItem.PageId );

        // 
        // return the client ResultData object for the customer.
        // 
        this.getClientData ( clientDataObject );

        return clientDataObject;
      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.MenuItem_Load_Page_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return this.Session.LastPage;

    }//END getObject method

    // ==============================================================================
    /// <summary>
    /// This method retrieves the menu item from the global menu list
    /// </summary>
    /// <param name="MenuItemGuid">Guid: the menu item guid primary key.</param>
    /// <returns> Evado.Model.Digital.EvMenuItem object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.Digital.EvMenuItem getMenuItem ( Guid MenuItemGuid )
    {
      if ( this.ApplicationObjects.MenuList.Count > 0 )
      {
        foreach ( EvMenuItem item in this.ApplicationObjects.MenuList )
        {
          if ( item.Guid == MenuItemGuid )
          {
            return item;
          }
        }
      }

      return new EvMenuItem ( );
    }

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="ClientDataObject">Evado.Model.UniForm.AppData object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private void getClientData (
      Evado.Model.UniForm.AppData ClientDataObject )
    {
      this.LogMethod ( "getDataObject" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );
      Evado.Model.UniForm.Field pageField = new Evado.Model.UniForm.Field ( );
      List<EvOption> optionList = new List<EvOption> ( );
      EvOption option = new EvOption ( );

      ClientDataObject.Id = this.Session.MenuItem.Guid;
      ClientDataObject.Title = EvLabels.Menu_Item;

      ClientDataObject.Page.Id = ClientDataObject.Id;
      ClientDataObject.Page.Title = ClientDataObject.Title;
      ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      // 
      // create the page pageMenuGroup
      // 
      Evado.Model.UniForm.Group pageGroup = ClientDataObject.Page.AddGroup (
        EvLabels.Menu_General_Group_Title,
        Evado.Model.UniForm.EditAccess.Inherited_Access );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      //
      // Add the platform selection list
      //

      optionList.Add ( new EvOption ( 
        EuMenus.CONST_ADMIN_APPLICATION_IDENTIFIER,
        EvLabels.Menu_Admin_Platform_Option_Description ) );

      optionList.Add ( new EvOption (
        EuMenus.CONST_PRODUCTION_APPLICATION_IDENTIFIER,
        EvLabels.Menu_Production_Platform_Option_Description ) );

      pageField = pageGroup.createSelectionListField (
        String.Empty,
        EvLabels.Menu_Platform_Field_Label,
        this.Session.MenuPlatformId,
        optionList );
      pageField.EditAccess = Model.UniForm.EditAccess.Disabled;

      // 
      // Create the customer id object
      // 
      optionList = Evado.Model.Digital.EvcStatics.Enumerations.getOptionsFromEnum (
        typeof ( EvPageIds ),
        true );

      Evado.Model.Digital.EvcStatics.sortOptionList ( optionList );

      pageField = pageGroup.createSelectionListField (
        EvMenuItem.MenuFieldNames.Page_Id.ToString ( ),
        EvLabels.Menu_Page_Id_Field_Label,
        this.Session.MenuItem.PageId.ToString ( ),
        optionList );
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;

      // 
      // Create the customer name object
      // 
      pageField = pageGroup.createTextField (
        EvMenuItem.MenuFieldNames.Title.ToString ( ),
        EvLabels.Menu_Title_Field_Label,
        String.Empty,
        this.Session.MenuItem.Title,
        20 );
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;

      // 
      // Create the customer name object
      // 
      pageField = pageGroup.createNumericField (
        EvMenuItem.MenuFieldNames.Order.ToString ( ),
        EvLabels.Menu_Order_Field_Label,
        this.Session.MenuItem.Order,
        0,
        200 );
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;

      // 
      // Create the modules the item is associated with
      // 
      List<EvOption> loadedModulesList = this.ApplicationObjects.getModuleList ( true );

      pageField = pageGroup.createCheckBoxListField (
        EvMenuItem.MenuFieldNames.Modules.ToString ( ),
        EvLabels.Menu_Modules_Field_Label,
        this.Session.MenuItem.Modules,
        loadedModulesList );
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;
      pageField.Description = EvLabels.Menu_Modules_Field_Description;

      // 
      // Create the customer name object
      // 
      List<EvOption> roleList = EvMenuItem.getRoleList (
        this.ApplicationObjects.ApplicationSettings.LoadedModules,
        false );
      string roles = this.Session.MenuItem.RoleList;

      pageField = pageGroup.createCheckBoxListField (
        EvMenuItem.MenuFieldNames.Role_List.ToString ( ),
        EvLabels.Menu_Role_List_Field_Label,
        roles,
        roleList );

      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;

      pageField.Description = EvLabels.Menu_Role_List_Field_Description ;

      // 
      // Add the save groupCommand
      // 
      pageCommand = pageGroup.addCommand (
        EvLabels.Menu_Save_Command_Title,
        EuAdapter.APPLICATION_ID,
        EuAdapterClasses.Menu.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Save_Object );

      // 
      // Define the save and delete groupCommand parameters
      // 
      pageCommand.SetGuid ( ClientDataObject.Id );

      //
      // Add the delete groupCommand object.
      //
      pageCommand = pageGroup.addCommand (
         EvLabels.Menu_Delete_Command_Title,
         EuAdapter.APPLICATION_ID,
         EuAdapterClasses.Menu.ToString ( ),
         Evado.Model.UniForm.ApplicationMethods.Save_Object );

      // 
      // Define the save and delete groupCommand parameters
      // 
      pageCommand.SetGuid ( ClientDataObject.Id );

      pageCommand.AddParameter ( Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION, EuMenus.CONST_DELETE_ACTION );

    }//END Method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class private create object methods

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="Command">Evado.Model.UniForm.Command object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData createObject ( Evado.Model.UniForm.Command Command )
    {
      this.LogMethod ( "createObject" );
      try
      {
        //
        // Determine if the user has access to this page and log and error if they do not.
        //
        if ( this.Session.UserProfile.hasAdministrationAccess == false )
        {
          this.LogIllegalAccess (
            this.ClassNameSpace + "createObject",
            this.Session.UserProfile );

          this.ErrorMessage = EvLabels.Illegal_Page_Access_Attempt;

          return this.Session.LastPage;
        }

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "createObject",
          this.Session.UserProfile );

        // 
        // Initialise the methods variables and objects.
        //      
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
        this.Session.MenuItem = new EvMenuItem ( );
        this.Session.MenuItem.Guid = Evado.Model.Digital.EvcStatics.CONST_NEW_OBJECT_ID;
        this.Session.MenuItem.Group = this.Session.MenuGroupIdentifier;
        this.Session.MenuItem.Platform = this.Session.MenuPlatformId;

        this.ApplicationObjects.MenuList = new List<EvMenuItem> ( );

        this.getClientData ( clientDataObject );

        //this.LogDebug ( "Exit createObject method. ID: {0}, Title: {1} ", clientDataObject.Id, clientDataObject.Title );

        return clientDataObject;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.MenuItem_Load_Page_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return this.Session.LastPage; ;

    }//END method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class private update object methods

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.ClientClientDataObjectEvado.Model.UniForm.Command object.</param>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData updateObject ( Evado.Model.UniForm.Command PageCommand )
    {
      try
      {
        this.LogMethod ( "updateObject" );
        this.LogDebug ( "Command: " + PageCommand.getAsString ( false, true ) );

        this.LogDebug ( "eClinical.MenuItem" );
        this.LogDebug ( "Guid: " + this.Session.MenuItem.Guid );
        this.LogDebug ( "PageId: " + this.Session.MenuItem.PageId );
        this.LogDebug ( "Title: " + this.Session.MenuItem.Title );
        this.LogDebug ( "Platform: " + this.Session.MenuItem.Platform ); 


        //
        // Determine if the user has access to this page and log and error if they do not.
        //
        if ( this.Session.UserProfile.hasAdministrationAccess == false )
        {
          this.LogIllegalAccess (
            this.ClassNameSpace + "updateObject",
            this.Session.UserProfile );

          this.ErrorMessage = EvLabels.Illegal_Page_Access_Attempt;

          return this.Session.LastPage;
        }

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "updateObject",
          this.Session.UserProfile );

        // 
        // Update the object.
        // 
        this.updateObjectValue ( PageCommand );

        //
        // Validate that all of the mandatory values exist.
        //
        if ( this.validateValues ( ) == false )
        {
          return this.Session.LastPage;
        }

        // 
        // Initialise the update variables.
        //  
        if ( this.Session.MenuItem.Guid == Evado.Model.Digital.EvcStatics.CONST_NEW_OBJECT_ID )
        {
          this.Session.MenuItem.Guid = Guid.Empty;
        }

        this.Session.MenuItem.UserId = this.Session.UserProfile.UserId;

        // 
        // Get the save action message value.
        // 
        String stSaveAction = PageCommand.GetParameter ( Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION );

        this.LogDebug ( "SaveAction: " + stSaveAction );

        // 
        // Resets the save action to the parameter passed in the page groupCommand.
        // 
        if ( stSaveAction == EuMenus.CONST_DELETE_ACTION )
        {
          this.LogDebug ( "DELETING MENU ITEM" );
          this.Session.MenuItem.Title = String.Empty;
          this.Session.MenuItem.PageId = EvPageIds.Null;
        }

        // 
        // update the object.
        // 
        EvEventCodes result = this._Bll_Menus.saveItem ( this.Session.MenuItem );

        // 
        // get the debug ResultData.
        // 
        this.LogValue ( this._Bll_Menus.Log );

        // 
        // if an error state is returned create log the event.
        // 
        if ( result != EvEventCodes.Ok )
        {
          string StEvent = this._Bll_Menus.Log + " returned error message: " + Evado.Model.Digital.EvcStatics.getEventMessage ( result );
          this.LogError ( EvEventCodes.Database_Record_Update_Error, StEvent );

          this.ErrorMessage = EvLabels.MenuItem_Update_Error_Message;

          this.LogMethodEnd ( "updateObject" );
          return this.Session.LastPage;
        }

        this.ApplicationObjects.MenuList = new List<EvMenuItem> ( );

        this.LogMethodEnd ( "updateObject" );
        return new Model.UniForm.AppData ( );

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.MenuItem_Update_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }
      this.LogMethodEnd ( "updateObject" );
      return this.Session.LastPage;

    }//END method

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="Parameters">List of field values to be updated.</param>
    /// <returns></returns>
    //  ----------------------------------------------------------------------------------
    private bool validateValues ( )
    {
      this.LogMethod ( "validateValues" );

      if ( this.Session.MenuItem.Group == String.Empty )
      {
        if ( this.ErrorMessage != String.Empty )
        {
          this.ErrorMessage += "\r\n";
        }
        this.ErrorMessage += EvLabels.Menu_Group_Empty_Error_Message;
      }

      if ( this.Session.MenuItem.PageId == EvPageIds.Null )
      {
        if ( this.ErrorMessage != String.Empty )
        {
          this.ErrorMessage += "\r\n";
        }
        this.ErrorMessage += EvLabels.Menu_PageId_Empty_Error_Message;
      }

      if ( this.Session.MenuItem.Title == String.Empty )
      {
        if ( this.ErrorMessage != String.Empty )
        {
          this.ErrorMessage += "\r\n";
        }
        this.ErrorMessage += EvLabels.Menu_Title_Empty_Error_Message;
      }

      if ( this.ErrorMessage != String.Empty )
      {
        return false;
      }

      return true;
    }

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="Parameters">List of field values to be updated.</param>
    /// <returns></returns>
    //  ----------------------------------------------------------------------------------
    private void updateObjectValue ( Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "updateObjectValue" );
      this.LogValue ( "Parameters.Count: " + PageCommand.Parameters.Count );

      // 
      // Iterate through the parameter values updating the ResultData object
      // 
      foreach ( Evado.Model.UniForm.Parameter parameter in PageCommand.Parameters )
      {
        this.LogValue ( parameter.Name + " > " + parameter.Value );

        if ( parameter.Name.Contains ( Evado.Model.Digital.EvcStatics.CONST_GUID_IDENTIFIER ) == true
          || parameter.Name == Evado.Model.UniForm.CommandParameters.Custom_Method.ToString ( )
          || parameter.Name == Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION )
        {
          continue;
        }

        this.LogValue ( " >> UPDATED" );
        try
        {
          EvMenuItem.MenuFieldNames fieldName = Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue<EvMenuItem.MenuFieldNames> (
            parameter.Name );

          this.Session.MenuItem.setValue ( fieldName, parameter.Value );

        }
        catch ( Exception Ex )
        {
          this.LogException ( Ex );
        }

      }// End iteration loop

    }//END method.

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
  }
}//END namespace