/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\ApplicationService.cs" 
 *  company="EVADO HOLDING PTY. LTD.">
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
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Configuration;

using Evado.Bll;
using Evado.Model;
using Evado.Bll.Digital;
using Evado.Model.Digital;
/// using Evado.Web;


namespace Evado.UniForm.Digital
{
  public partial class EuAdapter : Evado.Model.UniForm.ApplicationAdapterBase
  {
    private const string CONST_MENU_GROUP_FIELD_ID = "GRP";

    #region Class generate NO home page methods method

    // ==================================================================================
    /// <summary>
    /// This method gets the application object from the list.
    /// 
    /// </summary>
    /// <returns>ClientApplicationData</returns>
    // ----------------------------------------------------------------------------------
    public Evado.Model.UniForm.AppData generateNoProfilePage ( )
    {
      // 
      // initialise the methods variables and objects.
      // 
      this.LogMethod ( "generateNoProfilePage" );
      Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
      clientDataObject.Id = Guid.NewGuid ( );
      String stHeaderDescription = String.Empty;

      stHeaderDescription += "##" + EdLabels.Home_Page_Access_Denied_Header + "##"
        + " \r\n" + EdLabels.Home_Page_Access_Denied_Message + "\r\n";

      Evado.Model.UniForm.Page page = new Evado.Model.UniForm.Page ( );

      Evado.Model.UniForm.Group group = clientDataObject.Page.AddGroup (
        EdLabels.Default_Home_Page_Title,
        String.Empty,
        Evado.Model.UniForm.EditAccess.Enabled );
      group.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
      group.Description = stHeaderDescription;

      this.LogIllegalAccess (
        this.ClassNameSpace + "generateNoProfilePage",
        this.Session.UserProfile.Roles.ToString ( ) );

      // 
      // add the exit page.
      // 
      clientDataObject.Page.Exit = this.ExitCommand;

      // 
      // Return the home page.
      // 
      return clientDataObject;

    }///END generateHomePage method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class generate home page methods method
    // ==================================================================================
    /// <summary>
    /// This method gets the application object from the list.
    /// 
    /// </summary>
    /// <param name="PageCommand">ClientPateEvado.Model.UniForm.Command object</param>
    /// <returns>Evado.Model.UniForm.AppData</returns>
    // ----------------------------------------------------------------------------------
    public Evado.Model.UniForm.AppData generateHomePage (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "generateHomePage" );
      this.LogDebug ( "PageCommand: " + PageCommand.getAsString ( false, true ) );
      // 
      // initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      String stHeaderDescription = String.Empty;
      this.Session.PageId = EvPageIds.Null;

      // 
      // Log access to page.
      // 
      this.LogAction ( this.ClassNameSpace + "generateHomePage" );

      // 
      // Initialise the menus class for generating the home page menus.
      // 
      this._MenuUtility = new EuMenuUtility (
        this.Session,
        this.ClassParameters );

      //
      // get the selected pageMenuGroup item 
      //
      this.getSelectedGroupItem ( PageCommand );

      String pageTitle = Evado.Model.Digital.EdLabels.Default_Home_Page_Title;
      if ( this._AdapterObjects.Settings.HomePageHeaderText != String.Empty )
      {
        pageTitle = this._AdapterObjects.Settings.HomePageHeaderText;
      }

      //
      // Initialise the page object.
      //
      Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData (
        pageTitle,
        pageTitle,
        Evado.Model.UniForm.EditAccess.Enabled );

      clientDataObject.Id = Guid.NewGuid ( );
      clientDataObject.Page.Id = clientDataObject.Id;

      clientDataObject.Page.PageId = EuAdapter.ADAPTER_ID + EvPageIds.Home_Page;
      clientDataObject.Page.PageDataGuid = clientDataObject.Page.Id;
      clientDataObject.Page.SetLeftColumnWidth ( 15 );

      //
      // update the home apge selection values.
      //
      this.updateSelectionValue ( PageCommand );

      //
      // Add the command error messages.
      //
      this.addHomePageErrorMessages ( clientDataObject, PageCommand );

      //
      // Add the home page command to the page layout.
      //
      this.getHomePageCommand ( clientDataObject.Page );

      //
      // get the project and organisation selection list group.
      //
      this.getHomePage_SelectedGroup (
        clientDataObject.Page,
        PageCommand );

      // 
      // if menu items eise then generate dashboard items.
      // 
      if ( this._AdapterObjects.MenuList.Count > 0 )
      {
        // 
        // generate the page menu for the user.
        // 
        this.generateMenuGroups ( clientDataObject.Page, false );

        // 
        // Generate the trial dashboard
        //
        // TO BE ENABLED WHEN THE TRIAL CONFIGURATION AND DESIGN COMPONENTS ARE ENABLED.
        // 
        //this.getApplicationDashboard ( clientDataObject.Page );

        // 
        // Generate the alert list.
        // 
        //this.generateAlertList ( clientDataObject );

        this.LogDebugClass ( this._MenuUtility.Log );
        //
        // Generate the current user list.
        //
        //this.generateCurrentUserList ( clientDataObject.Page, PageCommand );

        this.LogValue ( "Page Command List count: " + clientDataObject.Page.CommandList.Count );

      }//MENU items exist.

      //
      // Generate the current user list.
      //
      this.generateCurrentUserList (
        clientDataObject.Page,
        PageCommand );

      // 
      // Append the exit groupCommand
      // 
      clientDataObject.Page.Exit = this.ExitCommand;


      this.LogMethodEnd ( "generateHomePage" );
      // 
      // Return the home page.
      // 
      return clientDataObject;

    }//END generateHomePage method

    //==================================================================================
    /// <summary>
    /// This method updates the home page selection settings..
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object</param>
    //----------------------------------------------------------------------------------
    private void updateSelectionValue ( Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "updateSelectionValue method" );

      string stAlertSelect = PageCommand.GetParameter ( EuAdapter.CONST_ALERT_SELECT );


      this.LogMethodEnd ( "updateSelectionValue" );
    }

    //==================================================================================
    /// <summary>
    /// This method updates the home page selection settings..
    /// </summary>
    /// <param name="ClientDataObject">Evado.Model.UniForm.AppData object</param>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object</param>
    //----------------------------------------------------------------------------------
    private void addHomePageErrorMessages (
      Evado.Model.UniForm.AppData ClientDataObject,
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "addHomePageErrorMessages method" );

      //
      // If the page groupCommand title is close old alert then run close alerts.
      //
      /*
    if ( PageCommand.Title == EdLabels.HomePage_Close_Old_Alerts_Command_Title )
    {
      this.LogValue ( "Page Command is close old alerts." );
     EvEventCodes returnCode = this.closeOldAlerts ( );

      if ( returnCode != EvEventCodes.Ok )
      {
        ClientDataObject.Message = EdLabels.HomePage_Close_Alert_Update_Error;
      }
    }//END execute close old alerts.
      */

      // 
      // include an error message if a missing page reference is countered.
      // 
      if ( PageCommand.Object != EuAdapterClasses.Home_Page.ToString ( ) )
      {
        this.LogDebug ( EdLabels.HomePage_Missing_Page_Error + PageCommand.Title );
        this.LogDebug ( "PageCommand: " + PageCommand.getAsString ( false, true ) );

        ClientDataObject.Message = EdLabels.HomePage_Missing_Page_Error + PageCommand.Title;
      }

      this.LogMethodEnd ( "addHomePageErrorMessages" );
    }//END addHomePageErrorMessages method

    //==================================================================================
    /// <summary>
    /// This method add the global commands to the home page.
    /// </summary>
    //----------------------------------------------------------------------------------
    private void getHomePageCommand ( Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getPageCommand method" );

      //
      // if the project is a patient informed consent project display command to access patients
      // if project organisations is set.
      //
      //
      // display the consent record export page.
      //
      var pageCommand = PageObject.addCommand (
        EdLabels.UserProfile_Update_Profile_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Users.ToString ( ),
        Model.UniForm.ApplicationMethods.Get_Object );

      pageCommand.AddParameter (
        EdUserProfile.UserProfileFieldNames.UserId.ToString ( ),
        this.Session.UserProfile.UserId );

      pageCommand.SetPageId ( EvPageIds.User_Profile_Update_Page );
      this.LogMethodEnd ( "getPageCommand" );
    }//END getPageCommand method

    //==================================================================================
    /// <summary>
    /// This method generates the project and project organisation selection list.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object</param>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object</param>
    //----------------------------------------------------------------------------------
    private void getHomePage_SelectedGroup (
      Evado.Model.UniForm.Page PageObject,
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getHomePage_SelectedGroup method" );
      this.LogValue ( "hasEvadoAdministrationAccess: " +
        this.Session.UserProfile.hasEvadoAdministrationAccess );
      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.UniForm.Group pageGroup = new Model.UniForm.Group ( );
      Evado.Model.UniForm.Field groupField = new Model.UniForm.Field ( );
      Evado.Model.UniForm.Command groupCommand = new Model.UniForm.Command ( );

      this.LogValue ( "The trial selection list has more than one project in it." );

      // 
      // Create the trial selection pageMenuGroup
      // 
      pageGroup = new Model.UniForm.Group (
        EdLabels.HomePage_Selection_Group_Title,
        String.Empty,
        Evado.Model.UniForm.EditAccess.Enabled );

      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      // 
      // Create a custom groupCommand to process the selection.
      // 
      groupCommand = pageGroup.addCommand (
        EdLabels.HomePage_Select_Project,
        EuAdapter.ADAPTER_ID,
         EuAdapterClasses.Home_Page.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Custom_Method );

      groupCommand.setCustomMethod ( Evado.Model.UniForm.ApplicationMethods.Get_Object );

      this.LogValue ( "groupCommand: " + groupCommand.getAsString ( false, true ) );

      //
      // Only add the group if it contains selection fields. 
      //
      if ( pageGroup.FieldList.Count > 0 )
      {
        PageObject.AddGroup ( pageGroup );
      }


      this.LogMethodEnd ( "getHomePage_SelectedGroup" );

    }//END getHomePage_ProjectSelectedGroup method.

    //==================================================================================
    /// <summary>
    /// This method updates the pageMenuGroup menu option.
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object</param>
    //-----------------------------------------------------------------------------------
    private void getSelectedGroupItem (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getSelectedGroupItem" );
      this.LogValue ( "Old MenuGroupItem: " + this.Session.MenuGroupItem.Title );
      //
      // Initialise the methods variables and objects.
      //
      String selectedMenuGroup = PageCommand.GetParameter ( EuAdapter.CONST_MENU_GROUP_FIELD_ID );
      this.LogValue ( "selectedMenuGroup : " + selectedMenuGroup );

      if ( selectedMenuGroup == String.Empty )
      {
        return;
      }

      //
      // Iterate through the user menu list to return the selected menu item.
      //
      foreach ( EvMenuItem item in this._AdapterObjects.MenuList )
      {
        if ( item.Group == selectedMenuGroup
          && item.GroupHeader == true )
        {
          this.Session.MenuGroupItem = item;
        }
      }

      this.LogValue ( "New MenuGroupItem: " + this.Session.MenuGroupItem.Title );

    }//END getSelectedGroupItem method

    // ==================================================================================
    /// <summary>
    /// This method generates the home page menu based on user roles.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object</param>
    /// <param name="OnlyAdministrationMenu">Bool: display administration menu.</param>
    // ----------------------------------------------------------------------------------
    public void generateMenuGroups (
      Evado.Model.UniForm.Page PageObject,
      bool OnlyAdministrationMenu )
    {
      this.LogMethod ( "generateMenuGroups" );
      this.LogValue ( "WebSiteIdentifier: " + this._AdapterObjects.PlatformId );
      this.LogValue ( "OnlyAdministrationMenu: " + OnlyAdministrationMenu );
      this.LogValue ( "selectedMenuGroup : " + this.Session.MenuGroupItem.Group );
      this.LogDebug ( "User Role: " + this.Session.UserProfile.Roles );
      // 
      // Initialise the methods variables and objects.
      // 
      int countOfGroups = 0;
      Evado.Model.UniForm.Group pageHeaderMenuGroup = new Model.UniForm.Group ( );
      List<EvMenuItem> menuHeaders = new List<EvMenuItem> ( );
      //
      // select admin menu pageMenuGroup .
      //
      if ( OnlyAdministrationMenu == true )
      {
        this.generateAdminMenuGroups ( PageObject );

        return;
      }

      //
      // Iterate through the menu items to extract the menu groups.
      //
      foreach ( EvMenuItem groupHeader in this._AdapterObjects.MenuList )
      {
        /*
        if ( groupHeader.Group == "TR" )
        {
          this.LogDebugFormat ( "Group: {0}, PageId: {1}, Title: {2}, Roles: {3}", groupHeader.Group, groupHeader.PageId, groupHeader.Title, groupHeader.Modules );
        }
        */

        // 
        // if the pageMenuGroup is not a header skip it.
        // 
        if ( groupHeader.GroupHeader == false )
        {
          continue;
        }

        /*
        this.LogDebugFormat ( "Group: {0}, User Role: {1}, HR: {2}, EDC: {3}, CTMS: {4}, PR: (5)",
          groupHeader.Group,
          this.Session.UserProfile.RoleId,
          groupHeader.RoleList,
           this.Session.Project.Data.EnableEdc,
           this.Session.Project.Data.EnableCtms,
           this.Session.Project.Data.EnablePatientRecords );
        */
        //
        // Validate the menu item
        //
        if ( groupHeader.SelectMenuHeader (
          this.Session.UserProfile.Roles ) == false )
        {
          //this.LogDebugValue ( "SKIPPED HEADER" );
          continue;
        }

        countOfGroups++;
        menuHeaders.Add ( groupHeader );

      }//END iteration.

      this.LogDebug ( "Header group count : " + countOfGroups );
      // 
      // Create the menu pageheaders MenuGroup object
      // 
      pageHeaderMenuGroup = new Model.UniForm.Group (
          EdLabels.HomePage_Menu_Header_Group_Title,
          Evado.Model.UniForm.EditAccess.Enabled );
      pageHeaderMenuGroup.CmdLayout = Evado.Model.UniForm.GroupCommandListLayouts.Vertical_Orientation;
      pageHeaderMenuGroup.Layout = Model.UniForm.GroupLayouts.Dynamic;
      pageHeaderMenuGroup.SetPageColumnCode ( Model.UniForm.PageColumnCodes.Left );

      // 
      // Iterate through teh groups building the user _Menus.
      // 
      foreach ( EvMenuItem groupHeader in menuHeaders )
      {

        //
        // If the group header length 1 set the header to the menu group item.
        //
        if ( countOfGroups == 1 )
        {
          this.Session.MenuGroupItem.Group = groupHeader.Group;
          //this.LogDebugValue ( "MenuGroupItem group: " + groupHeader.Group + " - " + groupHeader.Title );
        }


        //this.LogValue ( "Header group: " + groupHeader.Group + " - " + groupHeader.Title );

        // 
        // Add a header groupCommand 
        // 
        Evado.Model.UniForm.Command command = pageHeaderMenuGroup.addCommand (
            groupHeader.Title,
            EuAdapter.ADAPTER_ID,
            EuAdapterClasses.Home_Page.ToString ( ),
            Evado.Model.UniForm.ApplicationMethods.Custom_Method );

        command.setCustomMethod ( Model.UniForm.ApplicationMethods.Get_Object );

        command.AddParameter (
          EuAdapter.CONST_MENU_GROUP_FIELD_ID,
          groupHeader.Group );

        if ( this.Session.MenuGroupItem.Group == groupHeader.Group )
        {
          command.Type = Model.UniForm.CommandTypes.Null;

          //pageMenuGroup.SetPageColumnCode ( Model.UniForm.PageColumnCodes.Left );

          //this.LogDebugValue ( "groupHeader.Title: " + groupHeader.Title );

          this.getMenuGroupItems ( groupHeader, pageHeaderMenuGroup );
        }

      }//END pageMenuGroup header iteration loop.

      this.LogValue ( "CommandList.Count: " + pageHeaderMenuGroup.CommandList.Count );
      //
      // Display menu header group if there is more than one group item.
      //
      if ( pageHeaderMenuGroup.CommandList.Count > 1 )
      {
        PageObject.AddGroup ( pageHeaderMenuGroup );
      }

      this.LogMethodEnd ( "generateMenuGroups" );

    }//END generateMenuGroups method

    // ==================================================================================
    /// <summary>
    /// This method gets the generates a number of page pageMenuGroup _Menus depending upon the user roles.
    /// 
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object</param>
    // ----------------------------------------------------------------------------------
    public void generateAdminMenuGroups (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "generateAdminMenuGroups" );
      this.LogValue ( "WebSiteIdentifier: " + this._AdapterObjects.PlatformId );

      // 
      // Initialise the methods variables and objects.
      // 
      List<Evado.Model.UniForm.Command> menuCommandList = new List<Evado.Model.UniForm.Command> ( );

      // 
      // Create the menu pageheaders MenuGroup object
      // 
      Evado.Model.UniForm.Group pageHeaderMenuGroup = new Model.UniForm.Group (
         EdLabels.HomePage_Menu_Header_Group_Title,
         String.Empty,
         Evado.Model.UniForm.EditAccess.Enabled );
      pageHeaderMenuGroup.CmdLayout = Evado.Model.UniForm.GroupCommandListLayouts.Vertical_Orientation;
      pageHeaderMenuGroup.Layout = Model.UniForm.GroupLayouts.Dynamic;
      pageHeaderMenuGroup.SetPageColumnCode ( Model.UniForm.PageColumnCodes.Left );

      // 
      // Iterate through the groups building the user _Menus.
      // 
      foreach ( EvMenuItem groupHeader in this._AdapterObjects.MenuList )
      {
        //
        // Display only Admin menu if OnlyAdminMenu = true 
        //
        if ( groupHeader.Group != EvMenuItem.CONST_MENU_ADMIN_GROUP_ID
          && groupHeader.Group != EvMenuItem.CONST_MENU_PROJECT_MANAGEMENT_GROUP_ID )
        {
          continue;
        }

        // 
        // if the pageMenuGroup is not a header skip it.
        // 
        if ( groupHeader.GroupHeader == false )
        {
          continue;
        }

        //
        // Validate the menu EDC components should be displayed.
        //
        if ( groupHeader.SelectMenuHeader (
          this.Session.UserProfile.Roles ) == false )
        {
          continue;
        }

        menuCommandList = new List<Evado.Model.UniForm.Command> ( );

        this.LogDebug ( "Header: " + groupHeader.Group + " - " + groupHeader.Title );
        // 
        // Add a header groupCommand 
        // 
        Evado.Model.UniForm.Command command = pageHeaderMenuGroup.addCommand (
            groupHeader.Title,
            EuAdapter.ADAPTER_ID,
            EuAdapterClasses.Home_Page.ToString ( ),
            Evado.Model.UniForm.ApplicationMethods.Custom_Method );

        command.setCustomMethod ( Model.UniForm.ApplicationMethods.Get_Object );

        command.AddParameter (
          EuAdapter.CONST_MENU_GROUP_FIELD_ID,
          groupHeader.Group );

        // 
        // Iterate through the menu extracting the groups items.
        // 
        foreach ( EvMenuItem item in this._AdapterObjects.MenuList )
        {
          // 
          // Create a new pageMenuGroup if the headers do not match.
          // 
          if ( item.GroupHeader == true )
          {
            //this.LogDebugValue ( "Group Header" );
            continue;
          }

          // 
          // Create a new groupHeader if the headers  do not match.
          // 
          if ( groupHeader.Group != item.Group )
          {
            //this.LogDebugValue ( "Not a Member of the current header group" );
            continue;
          }

          // 
          // Create a new pageMenuGroup if the headers  do not match.
          // 
          if ( this.Session.MenuGroupItem.Group != item.Group )
          {
            //this.LogDebugValue ( "Not a member of the selected group" );
            continue;
          }

          this.LogDebug ( "PageId {0}", item.PageId );

          if ( item.Group == EvMenuItem.CONST_MENU_PROJECT_MANAGEMENT_GROUP_ID
            && item.PageId != EvPageIds.Trial_View_Page
            && item.PageId != EvPageIds.Report_Template_View
            && item.PageId != EvPageIds.Report_Template_Form
            && item.PageId != EvPageIds.Operational_Report_Page )
          {
            this.LogDebug ( "{0} is NOT selected.", item.PageId );
            continue;
          }

          //this.LogDebugFormat ( "Item PageId: {0} Title: {1}", item.PageId, item.Title );

          //
          // Validate the menu item
          //
          if ( item.SelectMenuItem (
            this.Session.UserProfile.Roles ) == false )
          {
            continue;
          }
          command.Type = Model.UniForm.CommandTypes.Null;

          //this.LogDebugFormat ( "Selected: Item PageId: {0} Title: {1}", item.PageId, item.Title );
          // 
          // Create the groupCommand
          // 
          var groupCommand = this._MenuUtility.getMenuItemCommandObject ( item );

          this.LogDebugClass ( this._MenuUtility.Log );

          if ( groupCommand != null )
          {
            groupCommand.Title = groupCommand.Title;
            groupCommand.Title = "- " + groupCommand.Title;
            pageHeaderMenuGroup.addCommand ( groupCommand );
          }

        }//END item iteration loop

      }//END pageMenuGroup menu item iteration loop.

      //
      // Add the menu pageMenuGroup if there are commands in the pageMenuGroup.
      //

      if ( pageHeaderMenuGroup.CommandList.Count > 0 )
      {
        PageObject.AddGroup ( pageHeaderMenuGroup );
      }

      this.LogMethodEnd ( "generateAdminMenuGroups" );

    }//END generateAdminMenuGroups method

    // ==================================================================================
    /// <summary>
    /// This method generates a menu pageMenuGroup menu items as a new pageMenuGroup.
    /// </summary>
    /// <param name="GroupHeader">Evado.Model.Digital.EvMenuItem object</param>
    /// <param name="GroupObject">Evado.Model.UniForm.Page object</param>
    // ----------------------------------------------------------------------------------
    public void getMenuGroupItems (
      EvMenuItem GroupHeader,
      Evado.Model.UniForm.Group GroupObject )
    {
      this.LogMethod ( "getMenuGroupItems" );
      this.LogDebug ( "GroupHeader Group: " + GroupHeader.Group );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Command groupCommand = new Evado.Model.UniForm.Command ( );
      List<Evado.Model.UniForm.Command> commandList = new List<Model.UniForm.Command> ( );

      // 
      // Iterate through the menu extracting the groups items.
      // 
      foreach ( EvMenuItem item in this._AdapterObjects.MenuList )
      {
        // 
        // Create a new pageMenuGroup if the headers do not match.
        // 
        if ( item.GroupHeader == true )
        {
          continue;
        }

        if ( GroupHeader.Group != item.Group )
        {
          continue;
        }

        this.LogDebug ( "PageId: {0}, User Role: {1}, Item Roles: {2}",
          item.PageId,
          this.Session.UserProfile.Roles,
          item.RoleList );

        //
        // Validate the menu item
        //
        if ( item.SelectMenuItem (
          this.Session.UserProfile.Roles ) == false )
        {
          this.LogDebug ( "SKIPPED item not selected." );
          continue;
        }

        // 
        // Create the groupCommand
        // 
        groupCommand = this._MenuUtility.getMenuItemCommandObject ( item );

        this.LogDebug ( this._MenuUtility.Log );

        if ( groupCommand != null )
        {
          groupCommand.Title = groupCommand.Title;
          groupCommand.Title = "- " + groupCommand.Title;
          commandList.Add ( groupCommand );
        }
        else
        {
          this.LogDebug ( "Command is null." );
        }

        this.LogDebug ( "Command list count {0}.",
          commandList.Count );
      }//END pageMenuGroup menu item iteration loop.

      //
      // Add the menu pageMenuGroup if there are commands in the pageMenuGroup.
      //
      if ( commandList.Count > 0 )
      {
        foreach ( Evado.Model.UniForm.Command command in commandList )
        {
          GroupObject.addCommand ( command );
        }
      }
      this.LogMethodEnd ( "getMenuGroupItems" );

    }//END getMenuGroupItems method

    // ==================================================================================
    /// <summary>
    /// This method gets generates the site dashboard for the user.
    /// 
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object</param>
    // ----------------------------------------------------------------------------------
    public void generateUserRoleGroup (
      Evado.Model.UniForm.Page PageObject )
    {
      // 
      // Initialise the methods variables and objects.
      // 
      this.LogMethod ( "generateUserRoleGroup" );

      // 
      // Define the pageMenuGroup.
      // 
      Evado.Model.UniForm.Group group = PageObject.AddGroup (
        EdLabels.HomePage_Welcome_Message
        + this.Session.UserProfile.CommonName );
      group.AddParameter ( Evado.Model.UniForm.GroupParameterList.Pixel_Width, 300 );

      String profile = String.Empty;

      //
      // Display the customer the user is associated with if it exist.
      //
      /*
      if ( this.Session.UserProfile.Customer != null )
      {
        profile += String.Format (
           EvCustomerLabels.Customer_User_Name_Format,
           this.Session.UserProfile.Customer.CustomerNo, 
           this.Session.UserProfile.Customer.Name );
      }
      profile += String.Format (
         EdLabels.HomePage_User_Role_Message,
          Evado.Model.EvStatics.enumValueToString ( this.Session.UserProfile.Roles ) );
      
      */
      group.Description = profile;


    }///END generateUerRoleGroup method
     ///
    /*
    // ==================================================================================
    /// <summary>
    /// This method generates the trial dashboard for the user.
    /// 
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object</param>
    // ----------------------------------------------------------------------------------
    public void getApplicationDashboard (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getApplicationDashboard" );
      this.LogDebug ( "Configration Access: " + this.Session.UserProfile.hasManagementAccess );
      this.LogDebug ( "ProjectDashboardComponents: " + this.Session.UserProfile.ProjectDashboardComponents );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Field pageField = new Model.UniForm.Field ( );
      if ( this.Session.UserProfile.ProjectDashboardComponents == String.Empty )
      {
        //this.Session.UserProfile.ProjectDashboardComponents = EvProject.ProjectDashboardOptions.Recruitment_Chart.ToString();
      }

      string [ ] displayOptions = this.Session.UserProfile.ProjectDashboardComponents.Split ( ';' );


      if ( this.Session.UserProfile.hasManagementAccess )
      {
        return;
      }

      Evado.Model.UniForm.Group pageGroup = new Model.UniForm.Group (
        EdLabels.HomePage_Application_Dashboard,
        String.Empty,
        Evado.Model.UniForm.EditAccess.Enabled );
      pageGroup.CmdLayout = Evado.Model.UniForm.GroupCommandListLayouts.Horizontal_Orientation;
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;
      pageGroup.DescriptionAlignment = Model.UniForm.GroupDescriptionAlignments.Center_Align;

      //
      // Define the group description containing the project and it's status.
      //
      pageGroup.Description = String.Format (
        EdLabels.HomePage_Application_Dashboard_Description,
        this.Session.Application.ApplicationId,
        this.Session.Application.Title,
        this.Session.Application.StateDesc );


      //
      // Iterate through the list if menu item and select the items for the 
      // project dashboard.
      //


      foreach ( EvMenuItem item in this._AdapterObjects.MenuList )
      {
        //
        // if the user does not have config access or is not group is not
        // project dashboard continue.
        //
        if ( this.Session.UserProfile.hasManagementAccess == false
          || item.Group != EvMenuItem.CONST_PROJECT_DASHBOARD_GROUP )
        {
          continue;
        }

        //
        // Validate the menu item
        //
        if ( item.SelectMenuItem (
          this.Session.UserProfile.Roles ) == false )
        {
          continue;
        }

        Evado.Model.UniForm.Command command = this._MenuUtility.getMenuItemCommandObject ( item );
        if ( command != null )
        {
          pageGroup.addCommand ( command );
        }
      }//END menu iteration loop

      // 
      // If the groupCommand list exists then output it
      // 
      if ( pageGroup.CommandList.Count > 0 )
      {
        PageObject.AddGroup ( pageGroup );
      }

      this.LogDebugClass ( this._MenuUtility.Log );
      this.LogMethodEnd ( "getApplicationDashboard" );

    }//END generateProjectDashboard method
    */
    // ==================================================================================
    /// <summary>
    /// This method gets generates the site dashboard for the user.
    /// 
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object</param>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object</param>    
    // ----------------------------------------------------------------------------------
    public void generateCurrentUserList (
      Evado.Model.UniForm.Page PageObject,
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "generateCurrentUserList" );
      this.LogValue ( "User is administrator: "
        + this.Session.UserProfile.hasAdministrationAccess );

      //
      // initialise the methods variables and objects.
      //
      Evado.Model.UniForm.Group pageGroup = new Model.UniForm.Group ( );
      Evado.Model.UniForm.Command groupCommand = new Model.UniForm.Command ( );
      String userId = String.Empty;
      String stEntryKey = PageCommand.GetParameter ( EuAdapter.CONST_HASHE_ITEM_KEY_SELECT );

      // 
      // if the project organisation is not a data collection site reset the organisation object.
      // 
      if ( this.Session.UserProfile.hasAdministrationAccess == false )
      {
        this.LogValue ( "The user does not have adminstrator role." );

        this.LogMethodEnd ( "generateCurrentUserList" );

        return;
      }

      if ( PageCommand.Method == Model.UniForm.ApplicationMethods.Delete_Object
        && PageCommand.hasParameter ( EuAdapter.CONST_HASHE_ITEM_KEY_SELECT ) == true
        && stEntryKey != String.Empty )
      {
        this.LogValue ( "stEntryKey: " + stEntryKey );

        this.deleteGlobalObjectEntries ( stEntryKey );

      }

      //
      // Define the group object.
      //
      pageGroup = PageObject.AddGroup (
        EdLabels.Admin_User_Session_Group_Title,
        Evado.Model.UniForm.EditAccess.Enabled );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;

      //
      // debug list the global objects.
      //
      foreach ( System.Collections.DictionaryEntry entry in this.GlobalObjectList )
      {
        stEntryKey = entry.Key.ToString ( );

        this.LogValue ( "Entry key: " + stEntryKey );

        if ( stEntryKey.Contains ( Evado.Model.UniForm.EuStatics.GLOBAL_DATE_STAMP ) == true )
        {
          userId = stEntryKey.Replace ( Evado.Model.UniForm.EuStatics.GLOBAL_DATE_STAMP, String.Empty );

          this.LogValue ( "User " + userId + " selected." );

          groupCommand = pageGroup.addCommand (
            userId,
            EuAdapter.ADAPTER_ID,
            EuAdapterClasses.Home_Page.ToString ( ),
            Model.UniForm.ApplicationMethods.Delete_Object );

          if ( userId.ToLower ( ) == this.Session.UserProfile.UserId.ToLower ( ) )
          {
            groupCommand.Method = Model.UniForm.ApplicationMethods.Null;
          }

          groupCommand.AddParameter ( EuAdapter.CONST_HASHE_ITEM_KEY_SELECT, stEntryKey );

        }
      }

      this.LogMethodEnd ( "generateCurrentUserList" );

    }//END generateSiteSubjectList method

    // ==================================================================================
    /// <summary>
    /// This method returns the Uri for the selected serviceId
    /// </summary>
    /// <param name="Entry">System.Collections.DictionaryEntry: dictionary entry</param>
    /// <returns>String containing the UnIFORM Uri</returns>
    // ----------------------------------------------------------------------------------
    private void deleteGlobalObjectEntries ( String EntryKey )
    {
      this.LogMethod ( "Evado.UniForm.Service.deleteOldGlobalObjects method" );
      this.LogValue ( "EntryKey: " + EntryKey );

      //
      // Initialise the methods variables and objects.
      //
      String UserName = EntryKey;
      UserName = UserName.ToUpper ( );
      UserName = UserName.Replace ( Evado.Model.UniForm.EuStatics.GLOBAL_DATE_STAMP, String.Empty );

      this.LogValue ( "User: " + UserName );
      String ClinicalObject_Key = UserName + Evado.Model.Digital.EvcStatics.SESSION_CLINICAL_OBJECT;

      this.LogValue ( "Clinical Key: " + ClinicalObject_Key );

      String HistoryList_Key = UserName + Evado.Model.UniForm.EuStatics.GLOBAL_COMMAND_HISTORY;

      this.LogValue ( "Command History Key: " + HistoryList_Key );

      this.GlobalObjectList.Remove ( EntryKey );
      this.GlobalObjectList.Remove ( ClinicalObject_Key );
      this.GlobalObjectList.Remove ( HistoryList_Key );

    }//END deleteGlobalObjectEntries method

    /*
    // ===============================================================================
    /// <summary>
    /// Description:
    ///  Connects to the database and fils the datagrid..
    /// 
    /// </summary>
    // ----------------------------------------------------------------------------------
    private void generateAlertList (
      Evado.Model.UniForm.AppData ApplicationData )
    {
      try
      {
        this.LogMethod ( "generateAlertList" );
        this.LogValue ( "Project.ProjectId: " + this.Session.Application.ApplicationId );
        // 
        // Initialise the methods variables and objects.
        // 
        EvAlerts projectAlerts = new EvAlerts ( );
        EvAlert.AlertTypes alertType = EvAlert.AlertTypes.Null;
        Evado.Model.UniForm.Command alertSelect;

        // 
        // Define the alert pageMenuGroup.
        // 
        Evado.Model.UniForm.Group group = new Evado.Model.UniForm.Group (
          EdLabels.HomePage_Alert_List,
          String.Empty,
          Evado.Model.UniForm.EditAccess.Enabled );
        group.CmdLayout = Evado.Model.UniForm.GroupCommandListLayouts.Vertical_Orientation;
        group.AddParameter ( Evado.Model.UniForm.GroupParameterList.Pixel_Width, 500 );

        // 
        // Query and database.
        // 
        List<EvAlert> alertView = projectAlerts.getView (
          this.Session.Application,
          new EvOrganisation (),
          this.Session.UserProfile,
          EvAlert.AlertStates.Not_Closed,
          alertType );

        this.LogDebugClass ( projectAlerts.Log );

        // 
        // Display the trial alerts pageMenuGroup if the alert view had objects.
        // 
        if ( alertView.Count > 0 )
        {
          
            alertSelect = ApplicationData.Page.addCommand (
              EdLabels.HomePage_Alert_SAE_Select_Title,
              EuAdapter.APPLICATION_ID,
              EuAdapterClasses.Home_Page.ToString ( ),
              Model.UniForm.ApplicationMethods.Custom_Method );

            alertSelect.setCustomMethod ( Model.UniForm.ApplicationMethods.List_of_Objects );

            alertSelect.AddParameter (
              EuAdapter.CONST_ALERT_SELECT,
               Evado.Model.Digital.EvcStatics.STRING_BOOLEAN_TRUE );

            alertSelect.AddParameter (
              Model.Digital.EdApplication.ApplicationFieldNames.ApplicationId.ToString ( ),
              this.Session.Application.ApplicationId );

          //
          // IF the alert list is longer than 50 items provide a groupCommand to close
          // alert that are more than a week old.
          //
          if ( alertView.Count > 50 )
          {
           alertSelect = ApplicationData.Page.addCommand (
              EdLabels.HomePage_Close_Old_Alerts_Command_Title,
              EuAdapter.APPLICATION_ID,
              EuAdapterClasses.Home_Page.ToString ( ),
              Model.UniForm.ApplicationMethods.Save_Object );

            alertSelect.AddParameter (
              Model.Digital.EdApplication.ApplicationFieldNames.ApplicationId.ToString ( ),
              this.Session.Application.ApplicationId );
          }

          // 
          // Create the Alert list.
          // 
          for ( int count = 0; count < this._ApplicationObjects.AlertListLength && count < alertView.Count; count++ )
          {
            EvAlert alert = alertView [ count ];
            String stAlertTite = alert.Subject;
            Evado.Model.UniForm.Command command = group.addCommand ( stAlertTite,
              EuAdapter.APPLICATION_ID,
              EuAdapterClasses.Alert.ToString ( ),
              Model.UniForm.ApplicationMethods.Get_Object );

            command.SetGuid ( alert.Guid );

          }//END alert view.

          // 
          // Append the alert pageMenuGroup to the page object.
          //
          ApplicationData.Page.GroupList.Add ( group );

        }//END Alert exist


      } // End Try
      catch ( Exception Ex )
      {
        this.LogException ( Ex );

        ApplicationData.Message = EdLabels.HomePage_Alert_List_Error;
      }  // End catch.  
      this.LogMethodEnd ( "generateAlertList" );

    }  // End generateAlertList method
    */
    /*
    // ===============================================================================
    /// <summary>
    /// Description:
    ///  Connects to the database and fils the datagrid..
    /// 
    /// </summary>
    // ----------------------------------------------------------------------------------
    private EvEventCodes closeOldAlerts ( )
    {
      try
      {
        this.LogMethod ( "closeOldAlerts method."
          + ", Project.ProjectId: " + this.Session.Application.ApplicationId );
        // 
        // Initialise the methods variables and objects.
        // 
        EvAlerts projectAlerts = new EvAlerts ( );
        EvAlert.AlertTypes alertType = EvAlert.AlertTypes.Null;

        // 
        // Query and database.
        // 
        List<EvAlert> alertView = projectAlerts.getView (
          this.Session.Application,
          new EvOrganisation ( ),
          this.Session.UserProfile,
          EvAlert.AlertStates.Not_Closed,
          alertType );

        this.LogValue ( projectAlerts.Log );

        this.LogValue ( "Iterating through the list of alerts." );

        //
        // Iterate through the list of alerts closing the alert that are
        // more have been open more than 7 days.
        //
        foreach ( EvAlert alert in alertView )
        {
          if ( alert.State == EvAlert.AlertStates.Raised
            && alert.Raised < DateTime.Now.AddDays ( -7 ) )
          {
            this.LogValue ( alert.Subject + " - " + alert.Raised );

            alert.UserCommonName = this.Session.UserProfile.CommonName;
            alert.UpdatedByUserId = this.Session.UserProfile.UserId;
            alert.State = EvAlert.AlertStates.Closed;

            projectAlerts.saveAlert ( alert );
          }
        }


      } // End Try
      catch ( Exception Ex )
      {
        this.LogException ( Ex );

        return EvEventCodes.Database_General_Error;
      }  // End catch.  

      return EvEventCodes.Ok;

    }  // End generateAlertList method
    */
    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

  }///END ApplicationService class

}///END NAMESPACE
