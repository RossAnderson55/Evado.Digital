/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\Organisations.cs" company="EVADO HOLDING PTY. LTD.">
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
using Evado.Bll.Digital;
using Evado.Model.Digital;
// using Evado.Web;

namespace Evado.UniForm.Digital
{
  /// <summary>
  /// This class defines the selection list class
  /// 
  /// This class terminates the Organisation object.
  /// </summary>
  public class EuSelectionLists : EuClassAdapterBase
  {
    #region Class Initialisation
    /// <summary>
    /// This method initialises the class.
    /// </summary>
    public EuSelectionLists ( )
    {
      this.ClassNameSpace = "Evado.UniForm.Digital.EuSelectionLists.";

      if ( this.Session.AdminSelectionLists == null )
      {
        this.Session.AdminSelectionLists = new List<EdSelectionList> ( );
      }

      if ( this.Session.AdminSelectionList == null )
      {
        this.Session.AdminSelectionList = new EdSelectionList ( );
      }
    }

    /// <summary>
    /// This method initialises the class and passs in the user profile.
    /// </summary>
    public EuSelectionLists (
      EuGlobalObjects AdapterObjects,
      EvUserProfileBase ServiceUserProfile,
      EuSession SessionObjects,
      String UniFormBinaryFilePath,
      EvClassParameters Settings )
    {
      this.ClassNameSpace = "Evado.UniForm.Digital.EuSelectionLists.";
      this.AdapterObjects = AdapterObjects;
      this.ServiceUserProfile = ServiceUserProfile;
      this.Session = SessionObjects;
      this.UniForm_BinaryFilePath = UniFormBinaryFilePath;
      this.ClassParameters = Settings;


      this.LogInitMethod ( "EuSelectionLists initialisation" );
      this.LogInit ( "ServiceUserProfile.UserId: " + ServiceUserProfile.UserId );
      this.LogInit ( "SessionObjects.UserProfile.Userid: " + this.Session.UserProfile.UserId );
      this.LogInit ( "SessionObjects.UserProfile.CommonName: " + this.Session.UserProfile.CommonName );
      this.LogInit ( "UniFormBinaryFilePath: " + this.UniForm_BinaryFilePath );

      this.LogInit ( "Settings:" );
      this.LogInit ( "-PlatformId: " + this.ClassParameters.PlatformId );
      this.LogInit ( "-ApplicationGuid: " + this.ClassParameters.AdapterGuid );
      this.LogInit ( "-LoggingLevel: " + Settings.LoggingLevel );
      this.LogInit ( "-UserId: " + Settings.UserProfile.UserId );
      this.LogInit ( "-UserCommonName: " + Settings.UserProfile.CommonName );

      if ( this.Session.AdminSelectionLists == null )
      {
        this.Session.AdminSelectionLists = new List<EdSelectionList> ( );
      }

      if ( this.Session.AdminSelectionList == null )
      {
        this.Session.AdminSelectionList = new EdSelectionList ( );
      }

      this._Bll_SelectionLists = new Evado.Bll.Digital.EdSelectionLists ( this.ClassParameters );

    }//END Method


    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class constants and variables.

    private Evado.Bll.Digital.EdSelectionLists _Bll_SelectionLists = new Evado.Bll.Digital.EdSelectionLists ( );

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class public methods

    // ==================================================================================
    /// <summary>
    /// This method gets the trial site object.
    /// 
    /// </summary>
    /// <param name="PageCommand">ClientPateEvado.Model.UniForm.Command object</param>
    /// <returns>ClientApplicationData</returns>
    //  ----------------------------------------------------------------------------------
    public Evado.Model.UniForm.AppData getDataObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getDataObject" );
      this.LogValue ( "PageCommand Content: " + PageCommand.getAsString ( false, false ) );
      try
      {
        // 
        // Initialise the methods variables and objects.
        // 
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );

        this.LogDebug ( "Command.Method {0}", PageCommand.Method );
        // 
        // Determine the method to be called
        // 
        switch ( PageCommand.Method )
        {
          case Evado.Model.UniForm.ApplicationMethods.List_of_Objects:
            {
              this.LogDebug ( "get list items" );
              clientDataObject = this.getListObject ( PageCommand );
              break;
            }
          case Evado.Model.UniForm.ApplicationMethods.Get_Object:
            {
              this.LogDebug ( "get object" );
              clientDataObject = this.getObject ( PageCommand );
              break;
            }
          case Evado.Model.UniForm.ApplicationMethods.Create_Object:
            {
              this.LogDebug ( "create object" );
              clientDataObject = this.createObject ( PageCommand );
              break;
            }
          case Evado.Model.UniForm.ApplicationMethods.Save_Object:
          case Evado.Model.UniForm.ApplicationMethods.Delete_Object:
            {
              this.LogDebug ( "Save object" );
              // 
              // Update the object values
              // 
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

    }//END getDataObject method


    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class list methods
    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    public Evado.Model.UniForm.AppData getListObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getListObject" );
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

          return this.Session.LastPage; ;
        }

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "getListObject",
          this.Session.UserProfile );

        //
        // get the selection lists.
        //
        this.getSelectionList ( );

        //
        // Initialise the page objects.
        //
        clientDataObject.Title = EdLabels.SelectionLists_List_Page_Title;
        clientDataObject.Page.Title = clientDataObject.Title;
        clientDataObject.Id = Guid.NewGuid ( );

        // 
        // Add the trial organisation list to the page.
        // 
        this.getListGroup ( clientDataObject.Page );

        this.LogValue ( "data.Title: " + clientDataObject.Title );
        this.LogValue ( "data.Page.Title: " + clientDataObject.Page.Title );


        this.LogMethodEnd ( "getListObject" );
        return clientDataObject;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.Organisation_List_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      this.LogMethodEnd ( "getListObject" );
      return this.Session.LastPage; ;

    }//END getListObject method.


    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    public void getSelectionList ( )
    {
      this.LogMethod ( "getSelectionList" );

      if ( this.Session.AdminSelectionLists.Count > 0 )
      {
        this.LogMethodEnd ( "getSelectionList" );
        return;
      }

      this.Session.AdminSelectionLists = this._Bll_SelectionLists.getView ( EdSelectionList.SelectionListStates.Null );

      this.LogDebugClass ( this._Bll_SelectionLists.Log );

      this.LogDebug ( "Selection list count {0}.", this.Session.AdminSelectionLists.Count );

      this.LogMethodEnd ( "getSelectionList" );

    }//END getSelectionList method



    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    public void getListGroup (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getListGroup" );
      try
      {
        // 
        // Create the new pageMenuGroup.
        // 
        Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
          EdLabels.SelectionLists_List_Group_Title );
        pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
        pageGroup.CmdLayout = Evado.Model.UniForm.GroupCommandListLayouts.Vertical_Orientation;

        // 
        // Add the save groupCommand
        // 
        Evado.Model.UniForm.Command groupCommand = pageGroup.addCommand (
          EdLabels.SelectionLists_New_List_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Selection_Lists.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Create_Object );

        groupCommand.SetBackgroundColour (
          Model.UniForm.CommandParameters.BG_Default,
          Model.UniForm.Background_Colours.Purple );

        // 
        // get the list of customers.
        // 
        if ( this.Session.AdminSelectionLists.Count == 0 )
        {
          this.Session.AdminSelectionLists = this._Bll_SelectionLists.getView ( EdSelectionList.SelectionListStates.Null );
          this.LogValue ( this._Bll_SelectionLists.Log );
        }
        this.LogValue ( "list count: " + this.Session.AdminSelectionLists.Count );
        // 
        // generate the page links.
        // 
        foreach ( EdSelectionList listItem in this.Session.AdminSelectionLists )
        {
          // 
          // Add the trial organisation to the list of organisations as a groupCommand.
          // 
          Evado.Model.UniForm.Command command = pageGroup.addCommand (
            listItem.LinkText,
            EuAdapter.ADAPTER_ID,
            EuAdapterClasses.Selection_Lists.ToString ( ),
            Evado.Model.UniForm.ApplicationMethods.Get_Object );

          command.Id = listItem.Guid;
          command.SetGuid ( listItem.Guid );

        }//END trial organisation list iteration loop

        this.LogValue ( "command count: " + pageGroup.CommandList.Count );

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.SelectionList_List_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

    }//END getListObject method.

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
      Guid OrgGuid = Guid.Empty;

      //
      // Determine if the user has access to this page and log and error if they do not.
      //
      if ( this.Session.UserProfile.hasAdministrationAccess == false )
      {
        this.LogIllegalAccess (
          this.ClassNameSpace + "getObject",
          this.Session.UserProfile );

        this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

        return this.Session.LastPage; ;
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
      OrgGuid = PageCommand.GetGuid ( );
      this.LogValue ( "OrgGuid: " + OrgGuid );

      // 
      // return if not trial id
      // 
      if ( OrgGuid == Guid.Empty )
      {
        this.LogValue ( "Guid Empty get current object" );

        if ( this.Session.AdminSelectionList.Guid != Guid.Empty )
        {
          // 
          // return the client ResultData object for the customer.
          // 
          this.getDataObject ( clientDataObject );
        }
        else
        {
          this.LogValue ( "ERROR: current organisation guid empty" );
          this.ErrorMessage = EdLabels.Organisation_Guid_Empty_Message;
        }

        return clientDataObject;
      }
      this.LogValue ( "Query site Guid: " + OrgGuid );

      try
      {
        // 
        // Retrieve the customer object from the database via the DAL and BLL layers.
        // 
        this.Session.AdminSelectionList = this._Bll_SelectionLists.getItem ( OrgGuid );

        this.LogValue ( this._Bll_SelectionLists.Log );

        this.LogDebug ( "SessionObjects.AdminSelectionList.ListId: "
          + this.Session.AdminSelectionList.ListId );

        // 
        // return the client ResultData object for the customer.
        // 
        this.getDataObject ( clientDataObject );

        this.LogValue ( "Page.Title: " + clientDataObject.Page.Title );

        return clientDataObject;
      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.Organisation_Page_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return this.Session.LastPage; ;

    }//END getObject method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class get object methods

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="ClientDataObject">Evado.Model.UniForm.AppData object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private void getDataObject ( Evado.Model.UniForm.AppData ClientDataObject )
    {
      this.LogMethod ( "getDataObject" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );
      Evado.Model.UniForm.Field pageField = new Evado.Model.UniForm.Field ( );

      ClientDataObject.Id = this.Session.AdminSelectionList.Guid;

      ClientDataObject.Title = EdLabels.SelectionList_New_List_Page_Title;

      if ( this.Session.AdminSelectionList.ListId != String.Empty )
      {
        ClientDataObject.Title =
          String.Format ( EdLabels.SelectionLIsts_Page_Title,
          this.Session.AdminSelectionList.ListId,
          this.Session.AdminSelectionList.Title );
      }
      ClientDataObject.Page.Id = ClientDataObject.Id;
      ClientDataObject.Page.Title = ClientDataObject.Title;
      ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      //
      // Set the user edit access to the objects.
      //
      if ( this.Session.UserProfile.hasAdministrationAccess == true )
      {
        ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
      }
      this.LogValue ( "Page.EditAccess: " + ClientDataObject.Page.EditAccess );

      //
      // Add the page commands 
      //
      this.getDataObject_PageCommands ( ClientDataObject.Page );

      //
      // Add the detail group to the page.
      //
      this.getDataObject_DetailsGroup ( ClientDataObject.Page );

      //
      // Display the selection list option table.
      //
      this.getDataObject_OptionGroup ( ClientDataObject.Page );

      this.LogMethodEnd ( "getDataObject" );

    }//END Method

    //================================================================================
    /// <summary>
    /// This method add the group commands to the grop.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Group object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_PageCommands (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getDataObject_PageCommands" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );

      // 
      // Add the save groupCommand
      // 
      if ( PageObject.EditAccess == Evado.Model.UniForm.EditAccess.Enabled )
      {
        //
        // save command.
        //
        pageCommand = PageObject.addCommand (
          EdLabels.SelectionList_Save_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Selection_Lists.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Save_Object );

        // 
        // Define the save and delete groupCommand parameters
        // 
        pageCommand.SetGuid ( this.Session.AdminSelectionList.Guid );
        pageCommand.AddParameter (
           Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
          EdSelectionList.SaveActions.Save.ToString ( ) );

        //
        // Delete command
        //
        pageCommand = PageObject.addCommand (
          EdLabels.SelectionList_Delete_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Selection_Lists.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Save_Object );

        // 
        // Define the save and delete groupCommand parameters
        // 
        pageCommand.SetGuid ( this.Session.AdminSelectionList.Guid );
        pageCommand.AddParameter (
           Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
          EdSelectionList.SaveActions.Delete_Object.ToString ( ) );
      }

      this.LogMethodEnd ( "getDataObject_PageCommands" );

    }//END getDataObject_GroupCommands Method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.AppData object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private void getDataObject_DetailsGroup (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getDataObject_DetailsGroup" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );

      // 
      // create the page pageMenuGroup
      // 
      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
       EdLabels.SelectionList_General_Group_Title );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      //
      // Add the group commands
      //
      this.getDataObject_GroupCommands ( pageGroup );

      // 
      // Create the customer id object
      // 
      groupField = pageGroup.createTextField (
        EdSelectionList.SelectionListFieldNames.ListId.ToString ( ),
        EdLabels.SelectionList_List_Id_Field_Label,
        this.Session.AdminSelectionList.ListId, 10 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;
      groupField.Mandatory = true;
      if ( this.Session.AdminSelectionList.Guid != Evado.Model.Digital.EvcStatics.CONST_NEW_OBJECT_ID )
      {
        groupField.EditAccess = Model.UniForm.EditAccess.Disabled;
      }

      // 
      // Create the customer name object
      // 
      groupField = pageGroup.createTextField (
        EdSelectionList.SelectionListFieldNames.Title.ToString ( ),
        EdLabels.SelectionList_Title_Field_Label,
        this.Session.AdminSelectionList.Title,
        50 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;
      groupField.Mandatory = true;

      groupField.setBackgroundColor (
        Model.UniForm.FieldParameterList.BG_Mandatory,
        Model.UniForm.Background_Colours.Red );

      // 
      // Create the customer name object
      // 
      groupField = pageGroup.createFreeTextField (
        EdSelectionList.SelectionListFieldNames.Description,
        EdLabels.SelectionList_Description_Field_Label,
        this.Session.AdminSelectionList.Description,
        50, 10 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      this.LogMethodEnd ( "getDataObject_DetailsGroup" );

    }//END getDataObject_DetailsGroup Method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.AppData object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private void getDataObject_OptionGroup (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getDataObject_OptionGroup" );
      this.LogDebug ( "Item count {0}", 
        this.Session.AdminSelectionList.Items.Count );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      int rowCount_Inital = 20;
      int rowCount_Extend = 5;

      // 
      // create the page page Group
      // 
      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
        EdLabels.SelectionList_Option_Group_Title );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      //
      // Add the group commands
      //
      this.getDataObject_GroupCommands ( pageGroup );

      // 
      // Create the customer id object
      // 
      groupField = pageGroup.createTableField (
        EdSelectionList.SelectionListFieldNames.Items.ToString ( ),
        EdLabels.SelectionList_Options_Field_Label,
        this.Session.AdminSelectionList.ListId, 3 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      groupField.Table.Header [ 0 ].No = 1;
      groupField.Table.Header [ 0 ].Text = EdLabels.SelectionList_Table_Column_1_Title;
      groupField.Table.Header [ 0 ].ColumnId = groupField.Table.Header [ 0 ].Text;
      groupField.Table.Header [ 0 ].Width = "20";
      groupField.Table.Header [ 0 ].TypeId = EvDataTypes.Text;

      groupField.Table.Header [ 1 ].No = 2;
      groupField.Table.Header [ 1 ].Text = EdLabels.SelectionList_Table_Column_2_Title;
      groupField.Table.Header [ 1 ].ColumnId = groupField.Table.Header [ 1 ].Text;
      groupField.Table.Header [ 1 ].Width = "20";
      groupField.Table.Header [ 1 ].TypeId = EvDataTypes.Text;

      groupField.Table.Header [ 2 ].No = 3;
      groupField.Table.Header [ 2 ].Text = EdLabels.SelectionList_Table_Column_3_Title;
      groupField.Table.Header [ 2 ].ColumnId = groupField.Table.Header [ 2 ].Text;
      groupField.Table.Header [ 2 ].Width = "40";
      groupField.Table.Header [ 2 ].TypeId = EvDataTypes.Text;

      //
      // Add an initial row count of 20 rows
      //
      if ( this.Session.AdminSelectionList.Items.Count == 0 )
      {
        for ( int i = 0; i < rowCount_Inital; i++ )
        {
          groupField.Table.addRow ( );
        }
      }
      else
      {
        for ( int count = 0; count < this.Session.AdminSelectionList.Items.Count; count++ )
        {
          EdSelectionList.Item item = this.Session.AdminSelectionList.Items [ count ];

          var row = groupField.Table.addRow ( );
          row.No = count + 1;
          row.Column [ 0 ] = item.Category;
          row.Column [ 1 ] = item.Value;
          row.Column [ 2 ] = item.Description;
        }

        //
        // Add extract rows to allow for more options to be added.
        //
        for ( int i = 0; i < rowCount_Extend; i++ )
        {
          groupField.Table.addRow ( );
        }
      }

      this.LogMethodEnd ( "getDataObject_OptionGroup" );

    }//END getDataObject_DetailsGroup Method

    //================================================================================
    /// <summary>
    /// This method add the group commands to the grop.
    /// </summary>
    /// <param name="PageGroup">Evado.Model.UniForm.Group object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_GroupCommands (
      Evado.Model.UniForm.Group PageGroup )
    {
      this.LogMethod ( "getDataObject_GroupCommands" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );

      // 
      // Add the save groupCommand
      // 
      if ( PageGroup.EditAccess == Evado.Model.UniForm.EditAccess.Enabled )
      {
        pageCommand = PageGroup.addCommand (
          EdLabels.SelectionList_Save_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Selection_Lists.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Save_Object );

        // 
        // Define the save and delete groupCommand parameters
        // 
        pageCommand.SetGuid ( this.Session.AdminSelectionList.Guid );
        pageCommand.AddParameter (
           Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
          EdSelectionList.SaveActions.Save.ToString ( ) );

        //
        // Issue command
        //
        pageCommand = PageGroup.addCommand (
          EdLabels.SelectionList_Issue_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Selection_Lists.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Save_Object );

        // 
        // Define the save and issue groupCommand parameters
        // 
        pageCommand.SetGuid ( this.Session.AdminSelectionList.Guid );
        pageCommand.AddParameter (
           Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
          EdSelectionList.SaveActions.Issue_List.ToString ( ) );


        //
        // Delete command
        //
        pageCommand = PageGroup.addCommand (
          EdLabels.SelectionList_Delete_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Selection_Lists.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Save_Object );

        // 
        // Define the save and delete groupCommand parameters
        // 
        pageCommand.SetGuid ( this.Session.AdminSelectionList.Guid );
        pageCommand.AddParameter (
           Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
          EdSelectionList.SaveActions.Delete_Object.ToString ( ) );
      }

      this.LogMethodEnd ( "getDataObject_GroupCommands" );

    }//END getDataObject_GroupCommands Method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class create object methods

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="Command">Evado.UniForm.Model.ClientClientDataObjectEvado.Model.UniForm.Command object.</param>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData createObject ( Evado.Model.UniForm.Command Command )
    {
      this.LogMethod ( "createObject" );
      try
      {
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

        //
        // Determine if the user has access to this page and log and error if they do not.
        //
        if ( this.Session.UserProfile.hasAdministrationAccess == false )
        {
          this.LogIllegalAccess ( "getListObject",
            this.Session.UserProfile );

          this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

          return this.Session.LastPage; ;
        }

        //
        // Initialise the dlinical ResultData objects.
        //
        this.Session.AdminSelectionList = new EdSelectionList ( );
        this.Session.AdminSelectionList.Guid = Evado.Model.Digital.EvcStatics.CONST_NEW_OBJECT_ID;
        this.Session.AdminSelectionList.ListId = String.Empty;
        this.Session.AdminSelectionList.Title = String.Empty;
        this.Session.AdminSelectionList.Description = String.Empty;
        this.Session.AdminSelectionList.Items = new List<EdSelectionList.Item> ( );

        this.getDataObject ( clientDataObject );


        this.LogValue ( "Exit createObject method. ID: "
          + clientDataObject.Id + ", Title: " + clientDataObject.Title );

        return clientDataObject;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = "Error raised when creating a trial site.";

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return this.Session.LastPage;

    }//END method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class update object methods

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.ClientClientDataObjectEvado.Model.UniForm.Command object.</param>
    /// <remarks>
    /// This method has following steps:
    /// 
    /// 1. Update the object values from command parameter values.
    /// 
    /// 2. Update the address fields of the customer.
    /// 
    /// 3. Save the updated fields to the respective tables in Evado Database.
    /// </remarks>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData updateObject ( Evado.Model.UniForm.Command PageCommand )
    {
      try
      {
        this.LogMethod ( "updateObject" );
        this.LogDebug ( "PageCommand: " + PageCommand.getAsString ( false, true ) );
        //
        // Initialise the methods variables and objects.
        //
        EdSelectionList.SaveActions saveAction = EdSelectionList.SaveActions.Save;

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "updateObject",
          this.Session.UserProfile );

        // 
        // Initialise the update variables.
        // 
        this.Session.AdminSelectionLists = new List<EdSelectionList> ( );

        // 
        // IF the guid is new object id  alue then set the save object for adding to the database.
        // 
        if ( this.Session.AdminSelectionList.Guid == Evado.Model.Digital.EvcStatics.CONST_NEW_OBJECT_ID )
        {
          this.Session.AdminSelectionList.Guid = Guid.Empty;
        }

        // 
        // Delete the object.
        // 
        if ( PageCommand.Method == Evado.Model.UniForm.ApplicationMethods.Delete_Object )
        {
          return new Model.UniForm.AppData ( );
        }

        // 
        // Update the object.
        // 
        this.updateObjectValue ( PageCommand.Parameters );

        //
        // Update the table values.
        //
        this.updateObjectTableValues ( PageCommand.Parameters );

        this.LogDebug ( "AdminSelectionList:" );
        this.LogDebug ( "-Guid: " + this.Session.AdminSelectionList.Guid );
        this.LogDebug ( "-ListId: " + this.Session.AdminSelectionList.ListId );
        this.LogDebug ( "-Title: " + this.Session.AdminSelectionList.Title );
        this.LogDebug ( "-Description: " + this.Session.AdminSelectionList.Description );
        this.LogDebug ( "-Items.Count: " + this.Session.AdminSelectionList.Items.Count );

        //
        // check that the mandatory fields have been filed.
        //
        if ( this.updateCheckMandatory ( ) == false )
        {
          return this.Session.LastPage;
        }

        //
        // remove the empty rows from the list.
        //
        for ( int i = 0; i < this.Session.AdminSelectionList.Items.Count; i++ )
        {
          if ( this.Session.AdminSelectionList.Items [ i ].Value == String.Empty )
          {
            this.Session.AdminSelectionList.Items.RemoveAt ( i );
            i--;
          }
        }

        // 
        // Get the save action message value.
        // 
        String stSaveAction = PageCommand.GetParameter ( Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION );

        // 
        // Resets the save action to the parameter passed in the page groupCommand.
        // 
        if ( stSaveAction != String.Empty )
        {
          saveAction = Evado.Model.EvStatics.parseEnumValue<EdSelectionList.SaveActions> ( stSaveAction );
        }
        this.Session.AdminSelectionList.Action = saveAction;

        if ( saveAction == EdSelectionList.SaveActions.Delete_Object )
        {
          this.Session.AdminSelectionList.Title = String.Empty;
        }

        // 
        // update the object.
        // 
        EvEventCodes result = this._Bll_SelectionLists.saveItem ( this.Session.AdminSelectionList );

        // 
        // get the debug ResultData.
        // 
        this.LogValue ( this._Bll_SelectionLists.Log );

        // 
        // if an error state is returned create log the event.
        // 
        if ( result != EvEventCodes.Ok )
        {
          string StEvent = this._Bll_SelectionLists.Log + " returned error message: " + Evado.Model.Digital.EvcStatics.getEventMessage ( result );
          this.LogError ( EvEventCodes.Database_Record_Update_Error, StEvent );

          switch ( result )
          {
            case EvEventCodes.Data_Duplicate_Id_Error:
              {
                this.ErrorMessage =
                  String.Format (
                    EdLabels.SelectionList_Duplicate_Error_Message,
                    this.Session.AdminSelectionList.ListId );
                break;
              }
            case EvEventCodes.Identifier_Org_Id_Error:
              {
                this.ErrorMessage = EdLabels.SelectionList_Identifier_Empty_Error_Message;
                break;
              }
            default:
              {
                this.ErrorMessage = EdLabels.SelectionList_Update_Error_Message;
                break;
              }
          }
          return this.Session.LastPage;
        }//END save error returned.

        return new Model.UniForm.AppData ( );

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.SelectionList_Update_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }
      return this.Session.LastPage;

    }//END method

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns></returns>
    //  ----------------------------------------------------------------------------------
    private bool updateCheckMandatory ( )
    {
      this.LogMethod ( "updateCheckMandatory" );
      //
      // Define the methods variables and objects.
      //
      bool bReturn = true;
      this.ErrorMessage = String.Empty;

      //
      // Org name not defined.
      //
      if ( this.Session.AdminSelectionList.ListId == String.Empty )
      {
        if ( this.ErrorMessage != String.Empty )
        {
          this.ErrorMessage += "\r\n ";
        }
        this.ErrorMessage += EdLabels.SelectionList_List_Id_Error_Message;

        bReturn = false;
      }

      //
      // Org name not defined.
      //
      if ( this.Session.AdminSelectionList.Title == String.Empty )
      {
        if ( this.ErrorMessage != String.Empty )
        {
          this.ErrorMessage += "\r\n ";
        }
        this.ErrorMessage += EdLabels.SelectionList_Title_Error_Message;

        bReturn = false;
      }

      return bReturn;
    }

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="Parameters">List of field values to be updated.</param>
    /// <returns></returns>
    //  ----------------------------------------------------------------------------------
    private void updateObjectValue (
      List<Evado.Model.UniForm.Parameter> Parameters )
    {
      this.LogMethod ( "updateObjectValue" );
      this.LogDebug ( "Parameters.Count: " + Parameters.Count );
      this.LogDebug ( "Customer.Guid: " + this.Session.AdminSelectionList.Guid );

      /// 
      /// Iterate through the parameter values updating the ResultData object
      /// 
      foreach ( Evado.Model.UniForm.Parameter parameter in Parameters )
      {
        if ( parameter.Name.Contains ( Evado.Model.Digital.EvcStatics.CONST_GUID_IDENTIFIER ) == false
          && parameter.Name != Evado.Model.UniForm.CommandParameters.Custom_Method.ToString ( )
          && parameter.Name != Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION
          && parameter.Name.Contains ( EdSelectionList.SelectionListFieldNames.Items.ToString ( ) ) == false )
        {
          this.LogDebug ( parameter.Name + " > " + parameter.Value + " >> UPDATED" );
          try
          {
            EdSelectionList.SelectionListFieldNames fieldName =
               Evado.Model.EvStatics.parseEnumValue<EdSelectionList.SelectionListFieldNames> (
              parameter.Name );

            this.Session.AdminSelectionList.setValue ( fieldName, parameter.Value );

          }
          catch ( Exception Ex )
          {
            this.LogException ( Ex );
          }
        }
        else
        {
          this.LogDebug ( parameter.Name + " > " + parameter.Value + " >> SKIPPED" );
        }

      }// End iteration loop

    }//END updateObjectValue method.

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="Parameters">List of field values to be updated.</param>
    /// <returns></returns>
    //  ----------------------------------------------------------------------------------
    private void updateObjectTableValues (
      List<Evado.Model.UniForm.Parameter> Parameters )
    {
      this.LogMethod ( "updateObjectTableValues" );
      this.LogDebug ( "Parameters.Count: " + Parameters.Count );
      this.Session.AdminSelectionList.Items = new List<EdSelectionList.Item> ( );
      int count = 0;
      /// 
      /// Iterate through the parameter values updating the ResultData object
      /// 
      foreach ( Evado.Model.UniForm.Parameter parameter in Parameters )
      {
        if ( parameter.Name.Contains ( EdSelectionList.SelectionListFieldNames.Items.ToString ( ) ) == true )
        {
          this.LogDebug ( parameter.Name + " > " + parameter.Value + " >> UPDATED" );
          string name = parameter.Name;
          string [ ] arName = name.Split ( '_' );
          int arLength = arName.Length;
          int column = EvStatics.getInteger ( arName [ arLength - 1 ] );
          int row = EvStatics.getInteger ( arName [ arLength - 2 ] ) - 1;

          this.LogDebug ( "Row {0}, Col {1}", row, column );

          if ( row >= this.Session.AdminSelectionList.Items.Count )
          {
            this.LogDebug ( "Add option item" );
            this.Session.AdminSelectionList.Items.Add ( new EdSelectionList.Item ( ) );
            this.Session.AdminSelectionList.Items [ row ].No = row + 1;
          }

          //
          // User switch to determine which object value to updae.
          //
          switch ( column )
          {
            case 1:
              {
                this.LogDebug ( "Update Category" );
                this.Session.AdminSelectionList.Items [ row ].Category = parameter.Value;
                break;
              }
            case 2:
              {
                this.LogDebug ( "Update Value" );
                this.Session.AdminSelectionList.Items [ row ].Value = parameter.Value;
                break;
              }
            case 3:
              {
                this.LogDebug ( "Update Description" );
                this.Session.AdminSelectionList.Items [ row ].Description = parameter.Value;
                break;
              }
          }//END value switch
        }
        else
        {
          this.LogDebug ( parameter.Name + " > " + parameter.Value + " >> SKIPPED" );
        }

      }// End iteration loop

    }//END updateObjectTableValues method.

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
  }
}//END namespace