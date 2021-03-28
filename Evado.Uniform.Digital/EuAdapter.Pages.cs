/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\ApplicationService.cs" 
 *  company="EVADO HOLDING PTY. LTD.">
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

    }//END generateNoProfilePage method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class generate page methods method

    // ==================================================================================
    /// <summary>
    /// This method generates a Page Layout 
    /// 
    /// </summary>
    /// <param name="PageCommand">ClientPateEvado.Model.UniForm.Command object</param>
    /// <returns>Evado.Model.UniForm.AppData</returns>
    // ----------------------------------------------------------------------------------
    public Evado.Model.UniForm.AppData generatePage (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "generatePage" );
      this.LogDebug ( "PageCommand: " + PageCommand.getAsString ( false, true ) );
      //
      // initialise the methods objects and variables.
      //
      EdPageLayout pageLayout = new EdPageLayout ( );
      Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
      Evado.Model.UniForm.Group pageGroup = new Model.UniForm.Group ( );
      Evado.Model.UniForm.Field groupField = new Model.UniForm.Field ( );
      Evado.Model.UniForm.Command groupCommand = new Model.UniForm.Command ( );
      bool enableLeftColumn = false;
      bool enableRightColumn = false;

      if ( this.Session.PageLayout == null )
      {
        this.Session.PageLayout = new EdPageLayout ( );
        this.Session.PageLayout.PageId = String.Empty;
      }

      string pageId = PageCommand.GetPageId ( );

      this.LogDebug ( "Current PageId {0}, Command PageId {1}. ", this.Session.PageLayout.PageId, pageId );

      if ( pageId == String.Empty )
      {
        this.LogDebug( "No Page ID provided ");
        this.LogMethodEnd ( "generatePage" );
        return this.Session.LastPage;
      }

      //
      // Update the page layout if it has changed.
      //
      if ( pageId != this.Session.PageLayout.PageId )
      {
        this.LogDebug ( "New paged will be loaded." );

        pageLayout = this._AdapterObjects.getPageLayout ( pageId );

        if ( pageLayout == null )
        {
          this.LogDebug ( "Page identifier not found" );

          this.ErrorMessage = EdLabels.PageLayout_Get_Empty_Error_Message;

          this.LogEvent (
            String.Format ( "Page Id {0} was not retrieved from the page layout list.", pageId ) );

          this.LogMethodEnd ( "generatePage" );
          return this.Session.LastPage;
        }

        //
        // update the current page layout.
        //
        this.Session.PageLayout = pageLayout;

      }//END change page layout.

      //
      // Initialise the page object.
      //
      clientDataObject.Id = this.Session.PageLayout.Guid;
      clientDataObject.Page.Id = clientDataObject.Id;
      clientDataObject.Title = this.Session.PageLayout.Title;

      Evado.Model.UniForm.Page page = clientDataObject.Page;

      page.Title = this.Session.PageLayout.Title;
      page.PageId = this.Session.PageLayout.PageId;
      page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      this.LogDebug ( "LeftColumnWidth {0}. ", this.Session.PageLayout.LeftColumnWidth );
      this.LogDebug ( "DisplayMainMenu {0}. ", this.Session.PageLayout.DisplayMainMenu );
      this.LogDebug ( "RightColumnWidth {0}. ", this.Session.PageLayout.RightColumnWidth );

      if ( this.Session.PageLayout.LeftColumnWidth > 0 )
      {
        page.SetLeftColumnWidth ( this.Session.PageLayout.LeftColumnWidth );
        enableLeftColumn = true;
      }

      if ( this.Session.PageLayout.DisplayMainMenu == true )
      {
        page.SetLeftColumnWidth ( 15 );
        enableLeftColumn = true;
      }
      this.LogDebug ( "page.left column width {0}.", page.GetLeftColumnWidth ( ) );

      if ( this.Session.PageLayout.RightColumnWidth > 0 )
      {
        page.SetRightColumnWidth ( this.Session.PageLayout.RightColumnWidth );
        enableRightColumn = true;
      }
      this.LogDebug ( "page.left column width {0}.", page.GetRightColumnWidth ( ) );

      //
      // create the main page menu.
      //
      this.generateMainMenu ( page );

      //
      // generate the page menu if it exists.
      //
      this.generatePageMenu ( page );

      //
      // generate the header comonents.
      //
      this.createPage_Header ( page, PageCommand );

      //
      // generate the left column content.
      //
      if ( enableLeftColumn == true )
      {
        this.createPage_LeftColumn ( page, PageCommand );
      }

      //
      // create the center body column components
      //
      this.createPage_CenterColumn ( page, PageCommand );

      //
      // generate the right column content.
      //
      if ( enableRightColumn == true )
      {
        this.createPage_RightColumn ( page, PageCommand );
      }

      this.LogMethodEnd ( "generatePage" );

      return clientDataObject;

    }//END generatePage method.

    // ==================================================================================
    /// <summary>
    /// This method generates the left column of a page layout. 
    /// </summary>
    /// <param name="PageObject">ClientPateEvado.Model.UniForm.Page object</param>
    /// <param name="PageCommand">ClientPateEvado.Model.UniForm.Command object</param>
    // ----------------------------------------------------------------------------------
    private void createPage_Header (
      Evado.Model.UniForm.Page PageObject,
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "createPage_Header" );
      // initialise the methods objects and variables.
      //
      Evado.Model.UniForm.Group pageGroup = new Model.UniForm.Group ( );
      Evado.Model.UniForm.Field groupField = new Model.UniForm.Field ( );
      Evado.Model.UniForm.Command groupCommand = new Model.UniForm.Command ( );

      //
      // Add the column's text content.
      //
      if ( this.Session.PageLayout.HeaderContent != String.Empty )
      {
        pageGroup = PageObject.AddGroup (
          String.Empty,
          this.Session.PageLayout.HeaderContent,
          Model.UniForm.EditAccess.Disabled );
        pageGroup.Layout = Model.UniForm.GroupLayouts.Page_Header;
      }

      //
      // generate the page header component list.
      //
      if ( this.Session.PageLayout.HeaderComponentList != String.Empty )
      {
        //
        // get the array of components.
        //
        string [ ] arrComponents = this.Session.PageLayout.HeaderComponentList.Split ( ';' );

        //
        // iterate through the components generating the page groups.
        //
        foreach ( String comp in arrComponents )
        {
          if ( comp.Contains ( EuAdapter.CONST_ENTITY_PREFIX ) == true )
          {
            EuEntities entities = new EuEntities (
                  this._AdapterObjects,
                  this.ServiceUserProfile,
                  this.Session,
                  this.UniForm_BinaryFilePath,
                  this.UniForm_BinaryServiceUrl,
                  this.ClassParameters );

            entities.getPageComponent ( PageObject, PageCommand );
          }//END entity page object.

        }// component iteration loop.

      }//END page header group list .

      this.LogMethodEnd ( "createPage_Header" );
    }//ENd createPage_LeftColumn method

    // ==================================================================================
    /// <summary>
    /// This method generates the left column of a page layout. 
    /// </summary>
    /// <param name="PageObject">ClientPateEvado.Model.UniForm.Page object</param>
    /// <param name="PageCommand">ClientPateEvado.Model.UniForm.Command object</param>
    // ----------------------------------------------------------------------------------
    private void createPage_LeftColumn (
      Evado.Model.UniForm.Page PageObject,
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "createPage_LeftColumn" );
      // initialise the methods objects and variables.
      //
      Evado.Model.UniForm.Group pageGroup = new Model.UniForm.Group ( );
      Evado.Model.UniForm.Field groupField = new Model.UniForm.Field ( );
      Evado.Model.UniForm.Command groupCommand = new Model.UniForm.Command ( );
      //
      // Add the column's text content.
      //
      if ( this.Session.PageLayout.LeftColumnContent != String.Empty )
      {
        pageGroup = PageObject.AddGroup (
          String.Empty,
          this.Session.PageLayout.LeftColumnContent,
          Model.UniForm.EditAccess.Disabled );
        pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;
        pageGroup.SetPageColumnCode ( Model.UniForm.PageColumnCodes.Left );
      }

      if ( this.Session.PageLayout.DisplayMainMenu == false)
      {
        //
        // if RightColumnComponentList exit add the components to the list.
        //
        if ( this.Session.PageLayout.LeftColumnComponentList != String.Empty )
        {
          //
          // get the array of components.
          //
          string [ ] arrComponents = this.Session.PageLayout.LeftColumnComponentList.Split ( ';' );

          //
          // iterate through the components generating the page groups.
          //
          foreach ( String comp in arrComponents )
          {
            if ( comp.Contains ( EuAdapter.CONST_ENTITY_PREFIX ) == true )
            {
              EuEntities entities = new EuEntities (
                    this._AdapterObjects,
                    this.ServiceUserProfile,
                    this.Session,
                    this.UniForm_BinaryFilePath,
                    this.UniForm_BinaryServiceUrl,
                    this.ClassParameters );

              entities.getPageComponent ( PageObject, PageCommand );
            }//END entity page object.

          }// component iteration loop.

        }//END left component exist.
      }//END no menu.

      this.LogMethodEnd ( "createPage_LeftColumn" );
    }//ENd createPage_LeftColumn method

    // ==================================================================================
    /// <summary>
    /// This method generates the left column of a page layout. 
    /// </summary>
    /// <param name="PageObject">ClientPateEvado.Model.UniForm.Page object</param>
    /// <param name="PageCommand">ClientPateEvado.Model.UniForm.Command object</param>
    // ----------------------------------------------------------------------------------
    private void createPage_CenterColumn (
      Evado.Model.UniForm.Page PageObject,
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "createPage_Center" );
      // 
      // initialise the methods objects and variables.
      //
      Evado.Model.UniForm.Group pageGroup = new Model.UniForm.Group ( );
      Evado.Model.UniForm.Field groupField = new Model.UniForm.Field ( );
      Evado.Model.UniForm.Command groupCommand = new Model.UniForm.Command ( );

      //
      // Add the column's text content.
      //
      if ( this.Session.PageLayout.CenterColumnContent != String.Empty )
      {
        pageGroup = PageObject.AddGroup (
          String.Empty,
          this.Session.PageLayout.CenterColumnContent,
          Model.UniForm.EditAccess.Disabled );
        pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;
      }

      //
      // generate the page header component list.
      //
      if ( this.Session.PageLayout.CenterColumnComponentList != String.Empty )
      {
        //
        // get the array of components.
        //
        string [ ] arrComponents = this.Session.PageLayout.CenterColumnComponentList.Split ( ';' );

        //
        // iterate through the components generating the page groups.
        //
        foreach ( String comp in arrComponents )
        {
          if ( comp.Contains ( EuAdapter.CONST_ENTITY_PREFIX ) == true )
          {
            EuEntities entities = new EuEntities (
                  this._AdapterObjects,
                  this.ServiceUserProfile,
                  this.Session,
                  this.UniForm_BinaryFilePath,
                  this.UniForm_BinaryServiceUrl,
                  this.ClassParameters );

            entities.getPageComponent ( PageObject, PageCommand );
          }//END entity page object.

        }//END component iteration loop.

      }//END page header group list .

      this.LogMethodEnd ( "createPage_Center" );

    }//END createPage_Center method

    // ==================================================================================
    /// <summary>
    /// This method generates the left column of a page layout. 
    /// </summary>
    /// <param name="PageObject">ClientPateEvado.Model.UniForm.Page object</param>
    /// <param name="PageCommand">ClientPateEvado.Model.UniForm.Command object</param>
    // ----------------------------------------------------------------------------------
    private void createPage_RightColumn (
      Evado.Model.UniForm.Page PageObject,
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "createPage_RightColumn" );
      // initialise the methods objects and variables.
      //
      Evado.Model.UniForm.Group pageGroup = new Model.UniForm.Group ( );
      Evado.Model.UniForm.Field groupField = new Model.UniForm.Field ( );
      Evado.Model.UniForm.Command groupCommand = new Model.UniForm.Command ( );
      //
      // Add the column's text content.
      //
      if ( this.Session.PageLayout.RightColumnContent != String.Empty )
      {
        pageGroup = PageObject.AddGroup (
          String.Empty,
          this.Session.PageLayout.RightColumnContent,
          Model.UniForm.EditAccess.Disabled );
        pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;
        pageGroup.SetPageColumnCode ( Model.UniForm.PageColumnCodes.Right );
      }

      //
      // if RightColumnComponentList exit add the components to the list.
      //
      if ( this.Session.PageLayout.RightColumnComponentList != String.Empty )
      {
        //
        // get the array of components.
        //
        string [ ] arrComponents = this.Session.PageLayout.RightColumnComponentList.Split ( ';' );

        //
        // iterate through the components generating the page groups.
        //
        foreach ( String comp in arrComponents )
        {
          if ( comp.Contains ( EuAdapter.CONST_ENTITY_PREFIX ) == true )
          {
            EuEntities entities = new EuEntities (
                  this._AdapterObjects,
                  this.ServiceUserProfile,
                  this.Session,
                  this.UniForm_BinaryFilePath,
                  this.UniForm_BinaryServiceUrl,
                  this.ClassParameters );

            entities.getPageComponent ( PageObject, PageCommand );
          }//END entity page object.

        }// component iteration loop.

      }//END RightColumnComponentList components


      this.LogMethodEnd ( "createPage_RightColumn" );
    }//ENd createPage_RightColumn method

    // ==================================================================================
    /// <summary>
    /// This method generates the home page menu based on user roles.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object</param>
    /// <param name="OnlyAdministrationMenu">Bool: display administration menu.</param>
    // ----------------------------------------------------------------------------------
    public void generateMainMenu (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "generateMainMenu" );
      this.LogValue ( "WebSiteIdentifier: " + this._AdapterObjects.PlatformId );
      this.LogValue ( "selectedMenuGroup : " + this.Session.MenuGroupItem.Group );
      this.LogDebug ( "User Role: " + this.Session.UserProfile.Roles );
      // 
      // Initialise the methods variables and objects.
      // 
      int countOfGroups = 0;
      Evado.Model.UniForm.Group pageHeaderMenuGroup = new Model.UniForm.Group ( );
      List<EvMenuItem> menuHeaders = new List<EvMenuItem> ( );

      //
      // Iterate through the menu items to extract the menu groups.
      //
      foreach ( EvMenuItem groupHeader in this._AdapterObjects.getMenuGroups ( EvMenuItem.CONST_MAIN_MENU_ID ) )
      {
        this.LogDebug ( "Group: {0}, PageId: {1}, Title: {2}, Roles: {3}", groupHeader.Group, groupHeader.PageId, groupHeader.Title, groupHeader.RoleList );

        // 
        // if the pageMenuGroup is not a header skip it.
        // 
        if ( groupHeader.GroupHeader == false )
        {
          continue;
        }

        this.LogDebug ( "Group: {0}, User Role: {1}, HR: {2},",
          groupHeader.Group,
          this.Session.UserProfile.Roles,
          groupHeader.RoleList );
        //
        // Validate the menu item
        //
        if ( groupHeader.SelectMenuHeader (
          this.Session.UserProfile.Roles ) == false )
        {
          //this.LogDebug ( "SKIPPED HEADER" );
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
          //this.LogDebug ( "MenuGroupItem group: " + groupHeader.Group + " - " + groupHeader.Title );
        }


        //this.LogDebug ( "Header group: " + groupHeader.Group + " - " + groupHeader.Title );

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

          //this.LogDebug ( "groupHeader.Title: " + groupHeader.Title );

          this.getMenuGroupItems ( groupHeader, pageHeaderMenuGroup.CommandList );
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

      this.LogMethodEnd ( "generateMainMenu" );

    }//END generateMenuGroups method

    // ==================================================================================
    /// <summary>
    /// This method generates the home page menu based on user roles.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object</param>
    /// <param name="OnlyAdministrationMenu">Bool: display administration menu.</param>
    // ----------------------------------------------------------------------------------
    public void generatePageMenu (
      Evado.Model.UniForm.Page PageObject)
    {
      this.LogMethod ( "generatePageMenu" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Model.UniForm.Group ( );
      List<Evado.Model.UniForm.Command> commandList = new List<Model.UniForm.Command> ( );

      //
      // the page menu and the left column menu cannot exist together.
      //
      if ( this.Session.PageLayout.DisplayMainMenu == true
        && this.Session.PageLayout.MenuLocation == EdPageLayout.MenuLocations.Left_Column )
      {
        return;
      }

      //
      // get the list of command for this menu.
      //
      commandList = this.getPagesMenuCommands ( );

      //
      // if no commands present exit.
      //
      if ( commandList.Count == 0 )
      {
        this.LogMethodEnd ( "generatePageMenu" );
        return;
      }

      // 
      // Create the menu pageheaders MenuGroup object
      // 
      switch ( this.Session.PageLayout.MenuLocation )
      {
        case EdPageLayout.MenuLocations.Left_Column:
          {
            pageGroup = PageObject.AddGroup (
                EdLabels.HomePage_Menu_Header_Group_Title,
                Evado.Model.UniForm.EditAccess.Disabled );

            pageGroup.CmdLayout = Evado.Model.UniForm.GroupCommandListLayouts.Vertical_Orientation;
            pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;
            pageGroup.SetPageColumnCode ( Model.UniForm.PageColumnCodes.Left );
            break;
          }
        case EdPageLayout.MenuLocations.Right_Column:
          {
            pageGroup = PageObject.AddGroup (
                EdLabels.HomePage_Menu_Header_Group_Title,
                Evado.Model.UniForm.EditAccess.Disabled );

            pageGroup.CmdLayout = Evado.Model.UniForm.GroupCommandListLayouts.Vertical_Orientation;
            pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;
            pageGroup.SetPageColumnCode ( Model.UniForm.PageColumnCodes.Right );
            break;
          }
        case EdPageLayout.MenuLocations.Page_Menu:
          {
            PageObject.CommandList = commandList;
            break;
          }
        case EdPageLayout.MenuLocations.Top_Center:
          {
            pageGroup = PageObject.AddGroup (
                EdLabels.HomePage_Menu_Header_Group_Title,
                Evado.Model.UniForm.EditAccess.Disabled );

            pageGroup.CmdLayout = Evado.Model.UniForm.GroupCommandListLayouts.Horizontal_Orientation;
            pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;
            break;
          }
      }

      this.LogMethodEnd ( "generatePageMenu" );

    }//END generateMenuGroups method

    // ==================================================================================
    /// <summary>
    /// This method generates a menu pageMenuGroup menu items as a new pageMenuGroup.
    /// </summary>
    /// <param name="GroupHeader">Evado.Model.Digital.EvMenuItem object</param>
    /// <param name="CommandList">Evado.Model.UniForm.Page object</param>
    // ----------------------------------------------------------------------------------
    public List<Evado.Model.UniForm.Command> getPagesMenuCommands (  )
    {
      this.LogMethod ( "getPagesMenuCommands" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Command groupCommand = new Evado.Model.UniForm.Command ( );
      List<Evado.Model.UniForm.Command> commandList = new List<Evado.Model.UniForm.Command>();

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

        if (  item.Group != this.Session.PageLayout.PageId )
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
        groupCommand = this._Navigation.GetNavigationCommand ( item );

        this.LogDebug ( this._Navigation.Log );

        if ( groupCommand != null )
        {
          groupCommand.Title = groupCommand.Title;
          groupCommand.Title = "- " + groupCommand.Title;
          commandList.Add ( groupCommand );
        }

      }//END pageMenuGroup menu item iteration loop.

      this.LogDebug ( "Command list count {0}.",
        commandList.Count );

      this.LogMethodEnd ( "getPagesMenuCommands" );
      return commandList;

    }//END getMenuGroupItems method

    // ==================================================================================
    /// <summary>
    /// This method generates a menu pageMenuGroup menu items as a new pageMenuGroup.
    /// </summary>
    /// <param name="GroupHeader">Evado.Model.Digital.EvMenuItem object</param>
    /// <param name="CommandList">Evado.Model.UniForm.Page object</param>
    // ----------------------------------------------------------------------------------
    public void getMenuGroupItems (
      EvMenuItem GroupHeader,
      List<Evado.Model.UniForm.Command> CommandList )
    {
      this.LogMethod ( "getMenuGroupItems" );
      this.LogDebug ( "GroupHeader Group: " + GroupHeader.Group );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Command groupCommand = new Evado.Model.UniForm.Command ( );

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
        groupCommand = this._Navigation.GetNavigationCommand ( item );

        this.LogDebug ( this._Navigation.Log );

        if ( groupCommand != null )
        {
          groupCommand.Title = groupCommand.Title;
          groupCommand.Title = "- " + groupCommand.Title;
          CommandList.Add ( groupCommand );
        }

      }//END pageMenuGroup menu item iteration loop.

      this.LogDebug ( "Command list count {0}.",
        CommandList.Count );

      this.LogMethodEnd ( "getMenuGroupItems" );

    }//END getMenuGroupItems method


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
      this.Session.PageId = String.Empty;

      // 
      // Log access to page.
      // 
      this.LogAction ( this.ClassNameSpace + "generateHomePage" );

      // 
      // Initialise the menus class for generating the home page menus.
      // 
      this._Navigation = new EuNavigation (
        this._AdapterObjects,
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

      clientDataObject.Page.PageId = EuAdapter.ADAPTER_ID + EdStaticPageIds.Home_Page;
      clientDataObject.Page.PageDataGuid = clientDataObject.Page.Id;
      clientDataObject.Page.SetLeftColumnWidth ( 15 );

      //
      // update the home apge selection values.
      //
      this.updateSelectionValue ( PageCommand );

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
        this.generateMainMenu ( clientDataObject.Page );

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

        this.LogDebugClass ( this._Navigation.Log );
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
      String stEntryKey = PageCommand.GetParameter ( EuAdapter.CONST_HASH_ITEM_KEY_SELECT );

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
        && PageCommand.hasParameter ( EuAdapter.CONST_HASH_ITEM_KEY_SELECT ) == true
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

          groupCommand.AddParameter ( EuAdapter.CONST_HASH_ITEM_KEY_SELECT, stEntryKey );

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
