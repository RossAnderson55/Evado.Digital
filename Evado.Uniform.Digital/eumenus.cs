/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\Menus.cs" company="EVADO HOLDING PTY. LTD.">
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
using System.Collections;
using System.Text;
using System.Web;
using System.Web.SessionState;

using Evado.Bll;
using Evado.Model;
using Evado.Bll.Digital;
using Evado.Model.Digital;

namespace Evado.UniForm.Digital
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
      EuGlobalObjects ApplicationObject,
      EvUserProfileBase ServiceUserProfile,
      EuSession SessionObjects,
      String UniFormBinaryFilePath,
      EvClassParameters Settings )
    {
      this.ClassNameSpace = "Evado.UniForm.Clinical.EuMenus.";
      this.AdapterObjects = ApplicationObject;
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
          this.Session.MenuPlatformId = this.AdapterObjects.PlatformId;
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

          this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

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
          this.AdapterObjects.MenuList = new List<EvMenuItem> ( );
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
        if ( PageCommand.hasParameter ( EvMenuItem.MenuFieldNames.Platform.ToString ( ) ) == true )
        {
          this.Session.MenuPlatformId = PageCommand.GetParameter ( EvMenuItem.MenuFieldNames.Platform.ToString ( ) );
        }
        this.LogValue ( "UPDATED: MenuPlatformId: " + this.Session.MenuPlatformId );

        //
        // Update the global menu list, to pick up any changes that where made during an audit cycle.
        //
        this.loadGlobalMenu ( );

        //
        // ensure that all page layouts have menu groups headers.
        //
        this.updatePageLayoutGroups ( );

        //
        // Initialise the ResultData object.
        //
        clientDataObject.Title = EdLabels.Menu_Item_List;
        clientDataObject.Page.Title = clientDataObject.Title;
        clientDataObject.Id = Guid.NewGuid ( );

        // 
        // Add the save groupCommand
        // 
        clientDataObject.Page.addCommand (
          EdLabels.Menu_New_Item_Command_Title,
          EuAdapter.ADAPTER_ID,
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
        this.ErrorMessage = EdLabels.Menu_List_Error_Message;

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
      this.LogMethod ( "loadGlobalMenu" );
      this.LogDebug ( "PlatformId: " + this.ClassParameters.PlatformId );
      try
      {
        EvMenus _Bll_Menus = new EvMenus ( this.ClassParameters );

        if ( this.AdapterObjects.MenuList.Count > 0 )
        {
          this.LogMethodEnd ( "loadGlobalMenu" );
          return;
        }

        // 
        // Get the site setMenu.
        // 
        this.AdapterObjects.MenuList = _Bll_Menus.getView ( this.ClassParameters.PlatformId, String.Empty );

        this.LogDebugClass ( "menus.DebugLog: " + _Bll_Menus.Log );
        this.LogValue ( "GlobalMenuList.Count: " + this.AdapterObjects.MenuList.Count );

      }
      catch ( Exception Ex )
      {
        this.LogDebug ( "loadGlobalMenu exception:  " + Evado.Model.Digital.EvcStatics.getException ( Ex ) );
      }
      this.LogDebug ( "Global Menu list count: " + this.AdapterObjects.MenuList.Count );

      this.LogMethodEnd ( "loadGlobalMenu" );
    }//END loadMenu method

    // ==============================================================================
    /// <summary>
    /// This method updates the PageLayout Groups to ensure that all page layouts have
    /// menu groups for them.
    /// </summary>
    //  ------------------------------------------------------------------------------
    private void updatePageLayoutGroups ( )
    {
      this.LogMethod ( "updatePageLayoutGroups" );

      if ( this.AdapterObjects.MenuList.Count == 0 )
      {
        return;
      }

      //
      // iterate through each of the issued pages to add a menu header item 
      // if one does not exist.
      //
      foreach ( EdPageLayout page in this.AdapterObjects.AllPageLayouts )
      {
        if ( this.hasMenuHeader ( page ) == true )
        {
          continue;
        }

        this.addMenuHeader ( page );
      }

      this.LogMethodEnd ( "updatePageLayoutGroups" );
    }

    // ==============================================================================
    /// <summary>
    /// This method tests to see if the a page group header exists for the page.
    /// </summary>
    /// <param name="Page">EdPageLayout object</param>
    /// <returns>True: menu group header exist.</returns>
    //  ------------------------------------------------------------------------------
    private bool hasMenuHeader ( EdPageLayout Page )
    {
      this.LogMethod ( "hasMenuHeader" );

      foreach ( EvMenuItem item in this.AdapterObjects.MenuList )
      {
        if ( item.GroupHeader == true
          && item.Group.ToUpper ( ) == Page.PageId.ToUpper ( ) )
        {
          this.LogDebug ( "Page Group found" );
          return true;
        }
      }

      this.LogDebug ( "Page Group NOT found" );
      return false;
    }

    // ==============================================================================
    /// <summary>
    /// This method adds a menu header item for the page..
    /// </summary>
    /// <param name="Page">EdPageLayout object</param>
    /// <returns>True: menu group header created.</returns>
    //  ------------------------------------------------------------------------------
    private bool addMenuHeader ( EdPageLayout Page )
    {
      this.LogMethod ( "addMenuHeader" );
      //
      // initialise the methods variables and objects.
      //
      EvMenuItem newHeader = new EvMenuItem ( );
      int headerOrder = 0;

      //
      // iterate through the list of menu items 
      //
      foreach ( EvMenuItem item in this.AdapterObjects.MenuList )
      {
        if ( item.GroupHeader == false )
        {
          continue;
        }

        headerOrder = item.Order;
      }

      headerOrder++;

      //
      // define the new menu group header object.
      //
      newHeader.PageId = EdStaticPageIds.Home_Page.ToString ( );
      newHeader.Group = Page.PageId.ToUpper ( );
      newHeader.Title = Page.Title;
      newHeader.GroupHeader = true;
      newHeader.UserTypes = Page.UserTypes;
      newHeader.Order = headerOrder;
      newHeader.RoleList = "Administrator";
      newHeader.Platform = "ADMIN";

      var result = this._Bll_Menus.saveItem ( newHeader );

      if ( result != EvEventCodes.Ok )
      {
        return false;
      }

      //
      // define the new menu group header object.
      //
      newHeader = new EvMenuItem ( );
      newHeader.PageId = EdStaticPageIds.Home_Page.ToString ( );
      newHeader.Group = Page.PageId;
      newHeader.Title = Page.Title;
      newHeader.GroupHeader = true;
      newHeader.UserTypes = Page.UserTypes;
      newHeader.Order = headerOrder;
      newHeader.RoleList = "Administrator";
      newHeader.Platform = "PROD";

      result = this._Bll_Menus.saveItem ( newHeader );

      if ( result != EvEventCodes.Ok )
      {
        return false;
      }

      //
      // force a load of a fresh menu list.
      //
      this.AdapterObjects.MenuList = new List<EvMenuItem> ( );

      return true;
    }


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
        EdLabels.Menu_Selection,
        Evado.Model.UniForm.EditAccess.Enabled );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;

      //
      // Add the platform selection list
      //
      optionList.Add ( new EvOption (
        EuMenus.CONST_ADMIN_APPLICATION_IDENTIFIER,
        EdLabels.Menu_Admin_Platform_Option_Description ) );

      optionList.Add ( new EvOption (
        EuMenus.CONST_PRODUCTION_APPLICATION_IDENTIFIER,
        EdLabels.Menu_Production_Platform_Option_Description ) );

      pageField = pageGroup.createSelectionListField (
        EvMenuItem.MenuFieldNames.Platform.ToString ( ),
        EdLabels.Menu_Platform_Field_Label,
        this.Session.MenuPlatformId,
        optionList );
      pageField.Layout = EuAdapter.DefaultFieldLayout;
      pageField.AddParameter ( Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );

      // 
      // Create the pageMenuGroup selection list.
      // 
      List<Evado.Model.EvOption> groupList = this.GetGroupList ( );

      if ( groupList.Count > 1 )
      {
        pageField = pageGroup.createSelectionListField (
          EuSession.CONST_MENU_GROUP_ID,
          EdLabels.Menu_Group_ID,
          this.Session.MenuGroupIdentifier,
          groupList );
        pageField.Layout = EuAdapter.DefaultFieldLayout;
        pageField.AddParameter ( Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );
      }

      //
      // Add the selection command
      //
      Evado.Model.UniForm.Command selectionCommand = pageGroup.addCommand (
        EdLabels.Menu_Selection_Menu,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Menu.ToString ( ),
        Model.UniForm.ApplicationMethods.Custom_Method );

      // 
      // Set the custom groupCommand parametet.
      // 
      selectionCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.List_of_Objects );

    }

    // ==============================================================================
    /// <summary>
    /// This method creates a list of header options 
    /// based on the currently loaded menu item list.
    /// </summary>
    /// <returns>List of EvOption objects</returns>
    //  ------------------------------------------------------------------------------
    private List<EvOption> GetGroupList ( )
    {
      //
      // Initialise the methods variables and objects.
      //
      List<EvOption> optionList = new List<EvOption> ( );
      optionList.Add ( new EvOption ( ) );

      foreach ( EvMenuItem header in this.AdapterObjects.MenuList )
      {
        if( header.GroupHeader == true 
          && header.Platform == this.Session.MenuPlatformId )
        {
          optionList.Add ( new EvOption ( header.Group, header.Group + " - "+ header.Title ) );
        }
      }

      return optionList;
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
         EdLabels.Menu_Item_List,
          Evado.Model.UniForm.EditAccess.Inherited );

        pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
        pageGroup.CmdLayout = Evado.Model.UniForm.GroupCommandListLayouts.Vertical_Orientation;
        pageGroup.Title = EdLabels.Menu_Item_List;

        //
        // Display a message if the pageMenuGroup has not been selected.
        //
        if ( GroupId == String.Empty )
        {
          pageGroup.Description = EdLabels.Menu_List_Group_Not_Selected_Message;

          return;
        }

        // 
        // Add the save groupCommand
        // 
        groupCommand = pageGroup.addCommand (
          EdLabels.Menu_New_Item_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Menu.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Create_Object );

        groupCommand.SetBackgroundDefaultColour ( Model.UniForm.Background_Colours.Purple );

        // 
        // get the list of customers.
        // 
        foreach ( EvMenuItem item in this.AdapterObjects.MenuList )
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
          this.LogValue ( "P: " + menuItem.PageId + ", T: " + menuItem.Title + ", R: " + menuItem.RoleList );

          // 
          // Add the Menu Field to the list of organisations as a groupCommand.
          // 
          groupCommand = pageGroup.addCommand ( 
            menuItem.LinkText,
            EuAdapter.ADAPTER_ID,
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

          this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

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
          this.ErrorMessage = EdLabels.MenuItem_Guid_Empty_Message;

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
        this.ErrorMessage = EdLabels.MenuItem_Load_Page_Error_Message;

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
      if ( this.AdapterObjects.MenuList.Count > 0 )
      {
        foreach ( EvMenuItem item in this.AdapterObjects.MenuList )
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
      ClientDataObject.Title = EdLabels.Menu_Item;

      ClientDataObject.Page.Id = ClientDataObject.Id;
      ClientDataObject.Page.Title = ClientDataObject.Title;
      ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      // 
      // create the page pageMenuGroup
      // 
      Evado.Model.UniForm.Group pageGroup = ClientDataObject.Page.AddGroup (
        EdLabels.Menu_General_Group_Title,
        Evado.Model.UniForm.EditAccess.Inherited );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      //
      // Add the platform selection list
      //

      optionList.Add ( new EvOption (
        EuMenus.CONST_ADMIN_APPLICATION_IDENTIFIER,
        EdLabels.Menu_Admin_Platform_Option_Description ) );

      optionList.Add ( new EvOption (
        EuMenus.CONST_PRODUCTION_APPLICATION_IDENTIFIER,
        EdLabels.Menu_Production_Platform_Option_Description ) );

      pageField = pageGroup.createSelectionListField (
        String.Empty,
        EdLabels.Menu_Platform_Field_Label,
        this.Session.MenuPlatformId,
        optionList );
      pageField.EditAccess = Model.UniForm.EditAccess.Disabled;

      // 
      // Create the customer id object
      // 
      optionList = this.AdapterObjects.PageIdentifiers;

      Evado.Model.Digital.EvcStatics.sortOptionListValues ( optionList );

      pageField = pageGroup.createSelectionListField (
        EvMenuItem.MenuFieldNames.Page_Id.ToString ( ),
        EdLabels.Menu_Page_Id_Field_Label,
        this.Session.MenuItem.PageId.ToString ( ),
        optionList );
      pageField.Layout = EuAdapter.DefaultFieldLayout;

      // 
      // Create the customer name object
      // 
      pageField = pageGroup.createTextField (
        EvMenuItem.MenuFieldNames.Title.ToString ( ),
        EdLabels.Menu_Title_Field_Label,
        String.Empty,
        this.Session.MenuItem.Title,
        20 );
      pageField.Layout = EuAdapter.DefaultFieldLayout;

      // 
      // Create the customer name object
      // 
      pageField = pageGroup.createNumericField (
        EvMenuItem.MenuFieldNames.Order.ToString ( ),
        EdLabels.Menu_Order_Field_Label,
        this.Session.MenuItem.Order,
        0,
        200 );
      pageField.Layout = EuAdapter.DefaultFieldLayout;

      // 
      // Create the customer name object
      // 
      optionList = this.AdapterObjects.Settings.GetRoleOptionList ( false );
      string roles = this.Session.MenuItem.RoleList;

      pageField = pageGroup.createCheckBoxListField (
        EvMenuItem.MenuFieldNames.Role_List.ToString ( ),
        EdLabels.Menu_Role_List_Field_Label,
        roles,
        optionList );

      pageField.Layout = EuAdapter.DefaultFieldLayout;

      pageField.Description = EdLabels.Menu_Role_List_Field_Description;

      // 
      // Create the customer name object
      // 
      pageField = pageGroup.createTextField (
        EvMenuItem.MenuFieldNames.Parameters.ToString ( ),
        EdLabels.Menu_Parameters_Field_Label,
        String.Empty,
        this.Session.MenuItem.Parameters,
        20 );
      pageField.Layout = EuAdapter.DefaultFieldLayout;

      // 
      // Add the save groupCommand
      // 
      pageCommand = pageGroup.addCommand (
        EdLabels.Menu_Save_Command_Title,
        EuAdapter.ADAPTER_ID,
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
         EdLabels.Menu_Delete_Command_Title,
         EuAdapter.ADAPTER_ID,
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

          this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

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

        this.AdapterObjects.MenuList = new List<EvMenuItem> ( );

        this.getClientData ( clientDataObject );

        //this.LogDebug ( "Exit createObject method. ID: {0}, Title: {1} ", clientDataObject.Id, clientDataObject.Title );

        return clientDataObject;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.MenuItem_Load_Page_Error_Message;

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

          this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

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
          this.Session.MenuItem.PageId = String.Empty;
        }

        // 
        // update the object.
        // 
        EvEventCodes result = this._Bll_Menus.saveItem ( this.Session.MenuItem );

        // 
        // get the debug ResultData.
        // 
        this.LogClass ( this._Bll_Menus.Log );

        // 
        // if an error state is returned create log the event.
        // 
        if ( result != EvEventCodes.Ok )
        {
          string StEvent = this._Bll_Menus.Log + " returned error message: " + Evado.Model.Digital.EvcStatics.getEventMessage ( result );
          this.LogError ( EvEventCodes.Database_Record_Update_Error, StEvent );

          this.ErrorMessage = EdLabels.MenuItem_Update_Error_Message;

          this.LogMethodEnd ( "updateObject" );
          return this.Session.LastPage;
        }

        this.AdapterObjects.MenuList = new List<EvMenuItem> ( );

        this.LogMethodEnd ( "updateObject" );
        return new Model.UniForm.AppData ( );

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.MenuItem_Update_Error_Message;

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
        this.ErrorMessage += EdLabels.Menu_Group_Empty_Error_Message;
      }

      if ( this.Session.MenuItem.PageId == String.Empty )
      {
        if ( this.ErrorMessage != String.Empty )
        {
          this.ErrorMessage += "\r\n";
        }
        this.ErrorMessage += EdLabels.Menu_PageId_Empty_Error_Message;
      }

      if ( this.Session.MenuItem.Title == String.Empty )
      {
        if ( this.ErrorMessage != String.Empty )
        {
          this.ErrorMessage += "\r\n";
        }
        this.ErrorMessage += EdLabels.Menu_Title_Empty_Error_Message;
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
          EvMenuItem.MenuFieldNames fieldName = Evado.Model.EvStatics.parseEnumValue<EvMenuItem.MenuFieldNames> (
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