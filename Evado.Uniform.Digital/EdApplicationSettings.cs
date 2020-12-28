/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\Trials.cs" company="EVADO HOLDING PTY. LTD.">
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
// using Evado.Web;

namespace Evado.UniForm.Clinical
{
  /// <summary>
  /// This class defines the application base classs that is used to terminate the 
  /// 
  /// hosted application objects.
  /// 
  /// This class terminates the Customer object.
  /// </summary>
  public class EdApplicationSettings : EuClassAdapterBase
  {
    #region Class Initialisation
    /// <summary>
    /// This method initialises the class.
    /// </summary>
    public EdApplicationSettings ( )
    {
      this.ClassNameSpace = "Evado.UniForm.Clinical.EuTrials.";
    }

    /// <summary>
    /// This method initialises the class and passs in the user profile.
    /// </summary>
    public EdApplicationSettings (
      EuApplicationObjects ApplicationObjects,
      EvUserProfileBase ServiceUserProfile,
      EuSession SessionObjects,
      String UniForm_BinaryFilePath,
      String UniForm_ServiceBinaryUrl,
      EvClassParameters Settings )
    {
      this.ClassNameSpace = "Evado.UniForm.Clinical.EuTrials.";
      this.LogInitMethod ( "EuTrials initialisation" );

      //
      // Initialise the class object and parameters.
      //
      this.ApplicationObjects = ApplicationObjects;
      this.ServiceUserProfile = ServiceUserProfile;
      this.Session = SessionObjects;
      this.UniForm_BinaryFilePath = UniForm_BinaryFilePath;
      this.UniForm_BinaryServiceUrl = UniForm_ServiceBinaryUrl;
      this.ClassParameters = Settings;
      this.LoggingLevel = this.ClassParameters.LoggingLevel;

      this.LogInit ( "ServiceUserProfile.UserId: " + ServiceUserProfile.UserId );
      this.LogInit ( "SessionObjects.Trial.TrialId: " + this.Session.Application.ApplicationId );
      this.LogInit ( "SessionObjects.UserProfile.Userid: " + this.Session.UserProfile.UserId );
      this.LogInit ( "SessionObjects.UserProfile.CommonName: " + this.Session.UserProfile.CommonName );
      this.LogInit ( "UniForm_BinaryFilePath: " + this.UniForm_BinaryFilePath );
      this.LogInit ( "UniForm_BinaryServiceUrl: " + this.UniForm_BinaryServiceUrl );

      this.LogInit ( "Settings.CustomerGuid: " + this.ClassParameters.CustomerGuid );
      this.LogInit ( "Settings.ApplicationGuid: " + this.ClassParameters.ApplicationGuid );
      this.LogInit ( "Settings.UserProfile.UserId: " + this.ClassParameters.UserProfile.UserId );
      this.LogInit ( "Settings.UserProfile.OrgId: " + this.ClassParameters.UserProfile.OrgId );

      this._MenuUtility = new EuMenuUtility ( this.Session, this.ClassParameters );
      this._MenuUtility.LoggingLevel = this.LoggingLevel;
      this._Bll_Trials = new Evado.Bll.Clinical.EdApplications ( this.ClassParameters );

      this.LogInitMethodEnd ( "EuTrials" );

    }//END Method


    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class constants and variables.

    private Evado.Bll.Clinical.EdApplications _Bll_Trials = new Evado.Bll.Clinical.EdApplications ( );
    EuMenuUtility _MenuUtility;

    private const string CONST_EXPORT_SITE_SELECTION_FIELD_ID = "exp_ts";
    private const string CONST_EXPORT_CHNG_SUBJECT_SCHEDULE_FIELD_ID = "exp_sac";
    private const string CONST_EXPORT_INC_DRAFT_RECORDS_FIELD_ID = "exp_idr";
    private const string CONST_EXPORT_MILESTONE_TYPE_FIELD_ID = "exp_mT";
    private const string CONST_EXPORT_OUTPUT_FORMAT_FIELD_ID = "exp_OF";

    private const string CONST_EXPORT_COMMAND = "exp_CMD";

    private const string CONST_UPDATE_INDEX_COMMAND = "exp_ui";
    private const string CONST_UPDATE_DATA_COMMAND = "exp_ud";
    private const string CONST_OUTPUT_DATE_COMMAND = "exp_od";


    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class public methods

    // ==================================================================================
    /// <summary>
    /// This method gets the application ResultData object for a project groupCommand request. 
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Evado.Model.UniForm.AppData</returns>
    //  ----------------------------------------------------------------------------------
    override public Evado.Model.UniForm.AppData getDataObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getDataObject" );
      this.LogValue ( "PageCommand " + PageCommand.getAsString ( false, false ) );
      this.LogDebug ( "Current Project "
        + this.Session.Application.ApplicationId
        + " - " + this.Session.Application.Title );

      Evado.Bll.EvStaticSetting.LoggingLevel = this.LoggingLevel;

      try
      {
        // 
        // Initialise the methods variables and objects.
        // 
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
        Evado.Bll.EvStaticSetting.DebugOn = this.DebugOn;


        //
        // Determine if the user has access to this page and log and error if they do not.
        //
        if ( this.Session.UserProfile.hasTrialManagementAccess == false )
        {
          this.LogIllegalAccess (
            this.ClassNameSpace,
            "hasConfigrationAccess",
            this.Session.UserProfile );

          this.ErrorMessage = EvLabels.Illegal_Page_Access_Attempt;

          return this.Session.LastPage;
        }

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace,
          this.Session.UserProfile );

        this.Session.PageId = PageCommand.GetParameter<EvPageIds> (
          Evado.Model.UniForm.CommandParameters.Page_Id );

        this.LogDebug ( "PageId: " + this.Session.PageId );

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
              //
              // Select page layout based on record page type.
              //
              switch ( this.Session.PageId )
              {
                case EvPageIds.Data_Point_Export_Page:
                  {
                    clientDataObject = this.getExportObject ( PageCommand );
                    break;
                  }
                case EvPageIds.Trial_Settings_Page:
                default:
                  {
                    clientDataObject = this.getObject ( PageCommand );
                    break;
                  }
              }//END RecordPageType switch

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


    }//END getDataObject method

    // ==================================================================================
    /// <summary>
    /// This method gets the application object from the list.
    /// 
    /// </summary>
    /// <param name="PageCommand">ClientPateEvado.Model.UniForm.Command object</param>
    /// <returns>Bool: True: trial object loaded</returns>
    // ----------------------------------------------------------------------------------
    private bool getProject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getProject" );
      this.LogValue ( "Current ProjectId: " + this.Session.Application.ApplicationId );
      this.LogValue ( "Command: " + PageCommand.getAsString ( false, true ) );
      // 
      // Initialise the methods variables and objects.
      // 
      Guid parameterProjectGuid = Guid.Empty;
      String parameterProjectId = String.Empty;

      // 
      // if the parameter value exists then set the customerId
      // 
      parameterProjectGuid = PageCommand.GetGuid ( );
      parameterProjectId = PageCommand.GetParameter ( EvIdentifiers.TRIAL_ID );

      this.LogValue ( "Project GUID: " + parameterProjectGuid + ", ID: " + parameterProjectId );

      //
      // Test to determine if the trial selection has changed.
      //
      if ( ( this.Session.Application.Guid == parameterProjectGuid
          && parameterProjectGuid != Guid.Empty )
        || this.Session.Application.ApplicationId == parameterProjectId )
      {
        this.LogValue ( "Current ProjectId: match Exit." );

        return true;
      }

      // 
      // if parameter trial is empty but a trial exists then rest it to empty.
      // 
      if ( parameterProjectId == "Null" )
      {
        if ( this.Session.Application.ApplicationId != String.Empty )
        {
          // 
          // set the trial object to empty.
          // 
          this.Session.Application = new Model.Digital.EdApplication ( );

          this.LogValue ( "Parameter ProjectId is 'Null' set selection trial object to empty" );
        }
        return true;
      }

      // 
      // if the selected id is empty and trial is not empty select.
      // set the trial object to empty.
      // 
      // 
      // If the current trial is empty and the parameter id is set then set
      // set the current trial to the parameter trial identifier to load a new trial object.
      // 
      if ( parameterProjectId == this.Session.Application.ApplicationId )
      {
        this.LogValue ( "Current ProjectId: match Exit." );

        return true;
      }

      // 
      // Retrieve the customer object from the database via the DAL and BLL layers.
      // 
      if ( parameterProjectGuid != Guid.Empty )
      {
        this.Session.Application = this._Bll_Trials.GetApplication ( parameterProjectGuid );
      }
      else
      {
        this.Session.Application = this._Bll_Trials.GetApplication ( parameterProjectId );
      }

      this.LogClass ( this._Bll_Trials.Log );

      this.LogValue ( "Project.ProjectId: " + this.Session.Application.ApplicationId );

      //
      // If the global project is being saved empty the project object.
      //
      if ( this.Session.Application.ApplicationId == Evado.Model.Digital.EvcStatics.CONST_GLOBAL_PROJECT )
      {
        this.ApplicationObjects.GlobalStudy = this.Session.Application;
      }


      return false;

    }//END getProject method.

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
      try
      {
        this.LogMethod ( "getListObject" );
        // 
        // Initialise the methods variables and objects.
        //      
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
        bool hasRegistryModule = this.ApplicationObjects.ApplicationSettings.hasModule ( EvModuleCodes.Registry_Module );

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "getListObject",
          this.Session.UserProfile );

        clientDataObject.Title = EvLabels.Project_Get_List_Of_Projects_Page_Label;
        clientDataObject.Page.Title = EvLabels.Project_Get_List_Of_Projects_Page_Label;
        clientDataObject.Id = Guid.NewGuid ( );

        // 
        // Create the new pageMenuGroup.
        // 
        Evado.Model.UniForm.Group pageGroup = clientDataObject.Page.AddGroup (
          EvLabels.Project_List_Of_Projects_Group_Label,
          Evado.Model.UniForm.EditAccess.Enabled );
        pageGroup.CmdLayout = Evado.Model.UniForm.GroupCommandListLayouts.Vertical_Orientation;
        pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;



        // 
        // Add the new trial groupCommand list.
        // 
        Evado.Model.UniForm.Command newTrialCommand = pageGroup.addCommand (
        EvLabels.Project_Create_Project_Command_Title,
        EuAdapter.APPLICATION_ID,
        EuAdapterClasses.Projects.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Create_Object );

        newTrialCommand.SetBackgroundColour (
          Model.UniForm.CommandParameters.BG_Default,
          Model.UniForm.Background_Colours.Purple );

        // 
        // get the list of customers.
        // 
        List<Model.Digital.EdApplication> projectList = this._Bll_Trials.GetApplicationList (
          Model.Digital.EdApplication.ApplicationStates.Null );

        this.LogClass ( this._Bll_Trials.Log );
        this.LogValue ( "list count: " + projectList.Count );

        // 
        // generate the page links.
        // 
        foreach ( Model.Digital.EdApplication project in projectList )
        {

          Evado.Model.UniForm.Command command = pageGroup.addCommand (
           string.Format ( "{0} - {1} Type: {2}, Status: {3}",
            project.ApplicationId, project.Title, project.TypeDescription, project.StateDesc ),
          EuAdapter.APPLICATION_ID,
          EuAdapterClasses.Projects.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Get_Object );
          command.Id = project.Guid;

          command.AddParameter (
            Model.UniForm.CommandParameters.Guid,
            project.Guid );

          command.AddParameter (
            EvIdentifiers.TRIAL_ID,
            project.ApplicationId );
        }

        this.LogValue ( " data.Title: " + clientDataObject.Title );
        this.LogValue ( " data.Page.Title: " + clientDataObject.Page.Title );

        this.LogValue ( " command object count: " + pageGroup.CommandList.Count );

        return clientDataObject;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Project_Get_List_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return null;

    }//END getListObject method.

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class private get object methods

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData getObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getObject" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );

      //
      // Determine if the user has access to this page and log and error if they do not.
      //
      if ( this.Session.UserProfile.hasTrialManagementAccess == false )
      {
        this.LogIllegalAccess (
          this.ClassNameSpace + "getObject",
          "hasConfigrationAccess",
          this.Session.UserProfile );

        this.ErrorMessage = EvLabels.Illegal_Page_Access_Attempt;

        return null;
      }

      // 
      // Log access to page.
      // 
      this.LogPageAccess (
        this.ClassNameSpace + "getObject",
        this.Session.UserProfile );

      try
      {
        // 
        // Retrieve the customer object from the database via the DAL and BLL layers.
        // 
        if ( this.getProject ( PageCommand ) == false )
        {
          return null;
        }

          this.getClientDataObject ( clientDataObject );

        this.LogValue ( "FINISHED GETTING TRIAL OBJECT" );

        return clientDataObject;
      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Project_Get_Page_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return this.Session.LastPage;

    }//END getObject method

    // ==============================================================================
    /// <summary>
    /// This method returns the application ResultData object defines the project 
    /// configuraiton page.
    /// </summary>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private void getClientDataObject (
      Evado.Model.UniForm.AppData ClientDataObject )
    {
      this.LogMethod ( "getClientDataObject" );
      this.LogDebug ( "Project.Guid: {0}.", this.Session.Application.Guid );
      this.LogDebug ( "Trial ID: {0}", this.Session.Application.ApplicationId );
      // 
      // Initialise the methods variables and objects.
      //  
      ClientDataObject.Id = Guid.NewGuid ( );
      ClientDataObject.Page.Id = ClientDataObject.Id;
      ClientDataObject.Title =
        String.Format (
          EvLabels.Project_Page_Title_Label,
          this.Session.Application.ApplicationId,
          this.Session.Application.Title );
      ClientDataObject.Page.Title = ClientDataObject.Title;

      this.LogValue ( "ClientClientDataObject.Id: " + ClientDataObject.Id );

      //
      // Set the user edit access to the objects.
      //
      ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      if ( this.Session.UserProfile.hasConfigrationEditAccess == true )
      {
        ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
      }

      //
      // Get the page commands.
      //
      this.getProjectPage_Page_Commands ( ClientDataObject.Page );

      //
      // Get the trial general pageMenuGroup.
      //
      this.getProjectPage_General_Group ( ClientDataObject.Page );

      //
      // Get the trial property pageMenuGroup 
      //
      this.getApplication_Status_Group ( ClientDataObject.Page );

      //
      // Get the trial property pageMenuGroup 
      //
      this.getProjectPage_Settings_Group ( ClientDataObject.Page );

      //
      // Get the trial signoff log.
      //
      this.getProjectPage_SignoffLog_Group ( ClientDataObject.Page );


    }//END Method



    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page object object.</param>
    // ------------------------------------------------------------------------------
    private void getProjectPage_Page_Commands (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getProjectPage_Page_Commands" );

      //
      // Intialise the methods variables and objects.
      //
      Evado.Model.UniForm.Command pageCommand = new Model.UniForm.Command ( );

      if ( this.Session.UserProfile.hasConfigrationEditAccess == true )
      {
        // 
        // Add the save groupCommand
        // 
        pageCommand = Page.addCommand (
          EvLabels.Project_Save_Command_Title,
          EuAdapter.APPLICATION_ID,
          EuAdapterClasses.Projects.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Save_Object );

        pageCommand.SetGuid ( this.Session.Application.Guid );

        if ( this.Session.Application.State == Model.Digital.EdApplication.ApplicationStates.Null )
        {
          // 
          // Add the save groupCommand
          // 
          pageCommand = Page.addCommand (
            EvLabels.Project_Delete_Command_Title,
            EuAdapter.APPLICATION_ID,
            EuAdapterClasses.Projects.ToString ( ),
            Evado.Model.UniForm.ApplicationMethods.Delete_Object );

          pageCommand.SetGuid ( this.Session.Application.Guid );
        }
      }

      //
      // Get the navigation commands.
      //
      foreach ( EvMenuItem item in this.ApplicationObjects.MenuList )
      {
        //
        // If the menu item is included in the trial menu pageMenuGroup 
        // add it to the client ResultData object as a page groupCommand.
        //
        if ( item.Group == EvMenuItem.CONST_PROJECT_MENU_GROUP
          && item.GroupHeader == false )
        {
          pageCommand = this._MenuUtility.getMenuItemCommandObject ( item );
          if ( pageCommand != null )
          {
            Page.addCommand ( pageCommand );
          }
        }
      }

      this.LogValue ( this._MenuUtility.Log );

    }//END getProjectPage_Page_Commands method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page object object.</param>
    // ------------------------------------------------------------------------------
    private void getProjectPage_General_Group (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getProjectPage_General_Group" );
      //
      // initialise the methods variables and objects.
      //
      List<Evado.Model.EvOption> optionList = new List<Evado.Model.EvOption> ( );
      Evado.Model.UniForm.Field pageField;

      // 
      // create the page pageMenuGroup
      // 
      Evado.Model.UniForm.Group pageGroup = Page.AddGroup (
       EvLabels.Project_General_Group_Title,
        Evado.Model.UniForm.EditAccess.Inherited_Access );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      // 
      // Create the customer id object
      // 
      pageField = pageGroup.createTextField (
        EvIdentifiers.TRIAL_ID,
        EvLabels.Label_Project_Id,
        this.Session.Application.ApplicationId, 10 );

      pageField.EditAccess = Evado.Model.UniForm.EditAccess.Disabled;
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;

      // 
      // Create the trais type selection
      // 
      pageField = pageGroup.createSelectionListField (
         Model.Digital.EdApplication.ApplicationFieldNames.Trial_Service.ToString ( ),
         EvLabels.Trial_Service_Field_Label,
         this.Session.Application.ServiceType.ToString ( ),
         EvCustomer.GetServiceList ( false ) );

      pageField.EditAccess = Evado.Model.UniForm.EditAccess.Disabled;
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;
      pageField.Mandatory = true;

      if ( this.Session.UserProfile.RoleId == EvRoleList.Evado_Administrator )
      {
        pageField.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
      }
      else
      {
        if ( this.Session.Application.State == Model.Digital.EdApplication.ApplicationStates.Null )
        {
          pageField.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
        }
      }
      this.LogValue ( "Type pageField.EditAccess: " + pageField.EditAccess );
      // 
      // Create the customer name object
      // 
      pageField = pageGroup.createTextField (
        Model.Digital.EdApplication.ApplicationFieldNames.Title.ToString ( ),
        EvLabels.Project_Title_Field_Label,
        string.Empty,
        this.Session.Application.Title,
        100 );
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;
      pageField.Mandatory = true;

      // 
      // Create the customer Address object
      // 
      pageField = pageGroup.createFreeTextField (
        Model.Digital.EdApplication.ApplicationFieldNames.Description.ToString ( ),
        EvLabels.Project_Description_Field_Label,
        string.Empty,
        this.Session.Application.Description, 100, 5 );
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;
 
      this.getProjectPage_AddGroupCommands ( pageGroup );

      this.LogMethodEnd ( "getProjectPage_General_Group" );
    }//END getProjectPage_General_Group method

    //==================================================================================
    /// <summary>
    /// This method adds the group commands to the passed group.
    /// </summary>
    /// <param name="PageGroup">Evado.Model.UniForm.Group object</param>
    //-----------------------------------------------------------------------------------
    private void getProjectPage_AddGroupCommands (
      Evado.Model.UniForm.Group PageGroup )
    {
      this.LogMethod ( "getProjectPage_AddGroupCommands" );
      //
      // initialise the methods variables and objects.
      //
      Evado.Model.UniForm.Command groupCommand = new Model.UniForm.Command ( );

      if ( PageGroup == null )
      {
        return;
      }

      //
      // Add the save command is the user has edit access.
      //
      if ( this.Session.UserProfile.hasConfigrationEditAccess == true )
      {
        // 
        // Add the refresh groupCommand
        // 
        groupCommand = PageGroup.addCommand (
          EvLabels.Project_Refresh_Command_Title,
          EuAdapter.APPLICATION_ID,
          EuAdapterClasses.Projects.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Custom_Method );

        groupCommand.SetGuid ( this.Session.Application.Guid );
        groupCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.Get_Object );

        // 
        // Add the save groupCommand
        // 
        groupCommand = PageGroup.addCommand (
          EvLabels.Project_Save_Command_Title,
          EuAdapter.APPLICATION_ID,
          EuAdapterClasses.Projects.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Save_Object );

        groupCommand.SetGuid ( this.Session.Application.Guid );
      }

      //
      // Get the navigation commands.
      //
      foreach ( EvMenuItem item in this.ApplicationObjects.MenuList )
      {
        //
        // if the user does not ahve configu access or is not group is not
        // project dashboard continue.
        //
        if ( item.Group != EvMenuItem.CONST_PROJECT_MENU_GROUP
          || item.GroupHeader == true )
        {
          continue;
        }
        this.LogDebug ( "Item {0} - {1}, Mod: {2} ", item.PageId, item.Title, item.Modules );

        //
        // Validate the menu item
        //
        if ( item.SelectMenuItem (
          this.ApplicationObjects.ApplicationSettings.LoadedModuleList,
          this.Session.Application,
          new EvOrganisation ( ),
          this.Session.UserProfile.RoleId ) == false )
        {
          continue;
        }

        //
        // If the menu item is included in the trial menu pageMenuGroup 
        // add it to the client ResultData object as a page groupCommand.
        //
        groupCommand = _MenuUtility.getMenuItemCommandObject ( item );

        if ( groupCommand == null )
        {
          this.LogInit ( "Null Command is: " + item.PageId );
          continue;
        }

        PageGroup.addCommand ( groupCommand );

      }//END of menuitem iteration loop

      this.LogMethodEnd ( "getProjectPage_AddGroupCommands" );

    }//END METHoed

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page object object.</param>
    // ------------------------------------------------------------------------------
    private void getApplication_Status_Group (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getApplication_Status_Group" );
      //
      // initialise the methods variables and objects.
      //
      List<Evado.Model.EvOption> optionList = new List<Evado.Model.EvOption> ( );
      Evado.Model.UniForm.Field pageField;

      // 
      // Add the status pageMenuGroup
      // 
      Evado.Model.UniForm.Group pageGroup = Page.AddGroup (
       EvLabels.Project_Properties_Group_Title,
       String.Empty,
       Evado.Model.UniForm.EditAccess.Inherited_Access );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      pageGroup.SetCommandBackBroundColor (
        Model.UniForm.GroupParameterList.BG_Mandatory,
        Model.UniForm.Background_Colours.Red );

      this.LogValue ( "pageGroup.Static: " + pageGroup.EditAccess );

      //
      // Add the save command is the user has edit access.
      //
      if ( this.Session.UserProfile.hasConfigrationEditAccess == true )
      {
        // 
        // Add the save groupCommand
        // 
        Evado.Model.UniForm.Command groupCommand = pageGroup.addCommand (
          EvLabels.Project_Save_Command_Title,
          EuAdapter.APPLICATION_ID,
          EuAdapterClasses.Projects.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Save_Object );

        groupCommand.SetGuid ( this.Session.Application.Guid );
      }

      //
      // Display the these field is the trial service  is not Lite service.
      //
      if ( this.Session.Application.ServiceType != EvCustomer.ServiceTypes.Lite )
      {
        // 
        // Create the Project disease list.
        // 
        ArrayList diseaseList = this.ApplicationObjects.ApplicationSettings.getDiseaseTypeListOptions ( true );
      }

      this.LogMethodEnd ( "getApplication_Status_Group" );
    }//END getApplication_Status_Group Method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object object.</param>
    // ------------------------------------------------------------------------------
    private void getProjectPage_Settings_Group (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getProjectPage_Settings_Group" );
      this.LogValue ( "LoadedModules: " + this.ApplicationObjects.ApplicationSettings.LoadedModules );
      this.LogValue ( "Clinical_Module: " + this.ApplicationObjects.ApplicationSettings.hasModule ( EvModuleCodes.Clinical_Module ) );
      //
      // Initialise the methods variables and objects.
      //
      List<EvOption> optionList = new List<EvOption> ( );
      Evado.Model.UniForm.Field groupField = new Model.UniForm.Field ( );

      // 
      // Add the project settings group
      // 
      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
       EvLabels.Project_Settings_Group_Title );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      this.LogValue ( "pageGroup.Static: " + pageGroup.EditAccess );

      //
      // Add the save command is the user has edit access.
      //
      if ( pageGroup.EditAccess == Evado.Model.UniForm.EditAccess.Enabled )
      {
        // 
        // Add the save groupCommand
        // 
        Evado.Model.UniForm.Command groupCommand = pageGroup.addCommand (
          EvLabels.Project_Save_Command_Title,
          EuAdapter.APPLICATION_ID,
          EuAdapterClasses.Projects.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Save_Object );

        groupCommand.SetGuid ( this.Session.Application.Guid );
      }

     

      //
      // Display the auxiliary subject data collection if the trial service is not lite.
      //
      if ( this.Session.Application.ServiceType != EvCustomer.ServiceTypes.Lite )
      {
        // 
        // Create the Using binary files field
        // 
        if ( this.ApplicationObjects.ApplicationSettings.hasModule ( EvModuleCodes.Imaging_Module ) == true )
        {
          groupField = pageGroup.createBooleanField (
            Model.Digital.EdApplication.ApplicationFieldNames.Enable_Binary_Data.ToString ( ),
            EvLabels.Project_Binary_Data_Field_Label,
            this.Session.Application.EnableBinaryData );
          groupField.Layout = EuFormGenerator.ApplicationFieldLayout;
        }
        else
        {
          this.Session.Application.EnableBinaryData = false;
        }
      }//END non-lite trial service

    }//END getProjectPage_Settings_Group Method

     // ==============================================================================
    /// <summary>
    /// This method creates the project signoff pageMenuGroup
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page object object.</param>
    // ------------------------------------------------------------------------------
    private void getProjectPage_SignoffLog_Group (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getProjectPage_SignoffLog" );

      // 
      // Display comments it they exist.
      // 
      if ( this.Session.Application.Signoffs.Count == 0 )
      {
        this.LogValue ( EvLabels.Label_No_Signoff_Label );
        return;
      }

      // 
      // create the page pageMenuGroup
      // 
      Evado.Model.UniForm.Field pageField = new Evado.Model.UniForm.Field ( );
      Evado.Model.UniForm.Group pageGroup = Page.AddGroup (
        EvLabels.Label_Signoff_Log_Group_Title,
        Evado.Model.UniForm.EditAccess.Enabled );

      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      pageField = pageGroup.createReadOnlyTextField (
        String.Empty,
        EvLabels.Label_Signoff_Log_Field_Title,
        String.Empty,
        EvUserSignoff.getSignoffLog ( this.Session.Application.Signoffs, false ) );
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;

    }//END getProjectPage_SignoffLog Method

    // ==================================================================================	
    /// <summary>
    /// This method checks if the organsisation is present in the organisation list.
    /// </summary>
    /// <param name="OrganisationList">List of EvOption objects.</param>
    /// <param name="Value"></param>
    // <returns>Role object</returns>
    // ---------------------------------------------------------------------------------
    private bool isInList ( List<EvOption> OrganisationList, String Value )
    {
      foreach ( EvOption organisation in OrganisationList )
      {
        if ( organisation.Value.ToUpper ( ).Trim ( ) == Value.ToUpper ( ).Trim ( ) )
        {
          return true;
        }
      }
      return false;

    }//END isInList method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class private get consent configuration page


    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData getConsentConfigurationObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getObject" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );

      try
      {
        // 
        // Retrieve the customer object from the database via the DAL and BLL layers.
        // 
        if ( this.getProject ( PageCommand ) == false )
        {
          return null;
        }

          this.getClientDataObject ( clientDataObject );

        this.LogValue ( "FINISHED GETTING TRIAL OBJECT" );

        return clientDataObject;
      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Project_Get_Page_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return this.Session.LastPage;

    }//END getObject method

    #endregion

    #region Class private export object methods

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData getExportObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getExportObject" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );

      //
      // Determine if the user has access to this page and log and error if they do not.
      //
      if ( this.Session.UserProfile.hasTrialManagementAccess == false
        && this.Session.UserProfile.hasDataManagerAccess == false )
      {
        this.LogIllegalAccess (
          this.ClassNameSpace + "getExportObject",
          this.Session.UserProfile );

        this.ErrorMessage = EvLabels.Illegal_Page_Access_Attempt;

        return null;
      }

      // 
      // Log access to page.
      // 
      this.LogPageAccess (
        this.ClassNameSpace + "getExportObject",
        this.Session.UserProfile );
      try
      {
        // 
        // Exit the method if the project id is empty.
        // 
        if ( this.Session.Application.ApplicationId == String.Empty )
        {
          return null;
        }

        //
        // Update the export parameter values.
        //
        this.updateExportParameterValue ( PageCommand );

        // 
        // return the client ResultData object for the customer.
        // 
        this.getExport_PageObject ( clientDataObject );

        this.LogValue ( "FINISHED GETTING PROJECT EXPORT OBJECT" );

        return clientDataObject;
      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Project_Get_Export_Page_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return null;

    }//END getExportObject method

    // ==================================================================================
    /// <summary>
    /// THis mehtod updates the export parameter object with groupCommand parameter values.
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    //  ----------------------------------------------------------------------------------
    private void updateExportParameterValue ( Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "updateExportParameterValue" );
      this.LogValue ( "Parameters.Count: " + PageCommand.Parameters.Count );

      //
      // Initialise the methods variables and objects.
      //
      string stValue = String.Empty;

      this.Session.ExportParameters.Project = this.Session.Application;
      this.Session.ExportParameters.UserProfile = this.Session.UserProfile;
      this.Session.ExportParameters.OutputDirectoryPath = this.UniForm_BinaryFilePath;
      this.Session.ExportParameters.OutputDirectoryUrl = this.UniForm_BinaryServiceUrl;
      this.Session.ExportParameters.UserProfile = this.Session.UserProfile;

      this.Session.ExportParameters.SmtpServer = this.ApplicationObjects.ApplicationSettings.SmtpServer;
      this.Session.ExportParameters.SmtpServerPort = this.ApplicationObjects.ApplicationSettings.SmtpServerPort;
      this.Session.ExportParameters.SmtpUserId = this.ApplicationObjects.ApplicationSettings.SmtpUserId;
      this.Session.ExportParameters.SmtpPassword = this.ApplicationObjects.ApplicationSettings.SmtpPassword;

      //
      // Update the site selection.
      //
      stValue = PageCommand.GetParameter ( EdApplicationSettings.CONST_EXPORT_SITE_SELECTION_FIELD_ID );
      if ( stValue != string.Empty )
      {
        this.Session.ExportParameters.IncludeTestSites = !Evado.Model.Digital.EvcStatics.getBool ( stValue );
      }
      this.LogValue ( "ExportParameters.SelectTestSites: " + this.Session.ExportParameters.IncludeTestSites );

      //
      // Update the subjects that changed schedules.
      //
      stValue = PageCommand.GetParameter ( EdApplicationSettings.CONST_EXPORT_CHNG_SUBJECT_SCHEDULE_FIELD_ID );
      if ( stValue != string.Empty )
      {
        this.Session.ExportParameters.IncludeSubjectsWithChangedSchedule = Evado.Model.Digital.EvcStatics.getBool ( stValue );
      }
      this.LogValue ( "ExportParameters.IncludeSubjectsWithChangedSchedule: " + this.Session.ExportParameters.IncludeSubjectsWithChangedSchedule );

      //
      // Update the include draft records in export.
      //
      stValue = PageCommand.GetParameter ( EdApplicationSettings.CONST_EXPORT_INC_DRAFT_RECORDS_FIELD_ID );
      if ( stValue != string.Empty )
      {
        this.Session.ExportParameters.IncludeDraftRecords = Evado.Model.Digital.EvcStatics.getBool ( stValue );
      }
      this.LogValue ( "ExportParameters.IncludeDraftRecords: " + this.Session.ExportParameters.IncludeDraftRecords );


      //
      // Update the include draft records in export.
      //
      stValue = PageCommand.GetParameter ( EdApplicationSettings.CONST_EXPORT_MILESTONE_TYPE_FIELD_ID );
      if ( stValue != string.Empty )
      {
        this.Session.ExportParameters.ExportMilestoneType =
           Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue<EvExportParameters.ExportMilestoneTypes> ( stValue );
      }
      this.LogValue ( "ExportParameters.ExportMilestoneType: " + this.Session.ExportParameters.ExportMilestoneType );


      //
      // Update the include draft records in export.
      //
      stValue = PageCommand.GetParameter ( EdApplicationSettings.CONST_EXPORT_OUTPUT_FORMAT_FIELD_ID );
      if ( stValue != string.Empty )
      {
        this.Session.ExportParameters.OutputFormat =
           Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue<EvExportParameters.StatisticalOutputFormatCodes> ( stValue );
      }
      this.LogValue ( "ExportParameters.OutputFormat: " + this.Session.ExportParameters.OutputFormat );


    }//END updateExportParameterValue method.

    // ==============================================================================
    /// <summary>
    /// This method returns the application ResultData object defines the project 
    /// configuraiton page.
    /// </summary>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private void getExport_PageObject (
      Evado.Model.UniForm.AppData ClientDataObject )
    {
      this.LogMethod ( "getExport_PageObject" );
      this.LogValue ( "Project.Guid: " + this.Session.Application.Guid );
      EvStaticSetting.DebugOn = this.DebugOn;
      this.LogValue ( "EvStaticSetting.DebugOn: " + EvStaticSetting.DebugOn );
      // 
      // Initialise the methods variables and objects.
      //  
      ClientDataObject.Id = this.Session.Application.Guid;
      ClientDataObject.Page.Id = ClientDataObject.Id;
      ClientDataObject.Title = EvLabels.Data_Export_Page_Title_Label + this.Session.Application.ApplicationId;
      ClientDataObject.Page.Title = ClientDataObject.Title;

      this.LogValue ( "ClientDataObject.Id: " + ClientDataObject.Id );

      //
      // Set the user edit access to the objects.
      //
      ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      //
      // Get the trial general pageMenuGroup.
      //
      this.getExportPage_Selection_Group ( ClientDataObject.Page );

      //
      // Get the trial property pageMenuGroup 
      //
      this.getExportPage_Download_Group ( ClientDataObject.Page );

    }//END getExport_PageObject Method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page object object.</param>
    // ------------------------------------------------------------------------------
    private void getExportPage_Selection_Group (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getExportPage_Selection_Group" );
      //
      // initialise the methods variables and objects.
      //
      List<Evado.Model.EvOption> optionList = new List<Evado.Model.EvOption> ( );
      Evado.Model.UniForm.Command groupCommand = new Model.UniForm.Command ( );

      // 
      // create the page pageMenuGroup
      // 
      Evado.Model.UniForm.Group pageGroup = Page.AddGroup (
       EvLabels.Data_Export_Selection_Group_Title,
        Evado.Model.UniForm.EditAccess.Inherited_Access );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      // 
      // Create the customer id object
      // 
      Evado.Model.UniForm.Field grouField = pageGroup.createReadOnlyTextField (
        String.Empty,
        EvLabels.Label_Project_Id,
        this.Session.Application.ApplicationId + EvLabels.Space_Hypen + this.Session.Application.Title );

      grouField.Layout = EuFormGenerator.ApplicationFieldLayout;

      // 
      // Create the test site selection field
      // 
      grouField = pageGroup.createBooleanField (
        EdApplicationSettings.CONST_EXPORT_SITE_SELECTION_FIELD_ID,
        EvLabels.Data_Export_Test_Selection_Field_Title,
        !this.Session.ExportParameters.IncludeTestSites );
      grouField.Layout = EuFormGenerator.ApplicationFieldLayout;

      // 
      // Create the milestone who have changed arm field
      // 
      grouField = pageGroup.createBooleanField (
        EdApplicationSettings.CONST_EXPORT_CHNG_SUBJECT_SCHEDULE_FIELD_ID,
        EvLabels.Data_Export_Subject_Changed_Schedule_Field_Title,
        this.Session.ExportParameters.IncludeSubjectsWithChangedSchedule );
      grouField.Layout = EuFormGenerator.ApplicationFieldLayout;

      // 
      // Create the milestone who have changed arm field
      // 
      grouField = pageGroup.createBooleanField (
        EdApplicationSettings.CONST_EXPORT_INC_DRAFT_RECORDS_FIELD_ID,
        EvLabels.Data_Export_Inc_Draft_Records_Field_Title,
        this.Session.ExportParameters.IncludeDraftRecords );
      grouField.Layout = EuFormGenerator.ApplicationFieldLayout;

      // 
      // Create the milestone who have changed arm field
      // 
      optionList = Evado.Model.Digital.EvcStatics.Enumerations.getOptionsFromEnum ( typeof ( EvExportParameters.ExportMilestoneTypes ), true );

      grouField = pageGroup.createSelectionListField (
        EdApplicationSettings.CONST_EXPORT_MILESTONE_TYPE_FIELD_ID,
        EvLabels.Data_Export_Milestone_Type_Field_Title,
        this.Session.ExportParameters.ExportMilestoneType.ToString ( ),
        optionList );
      grouField.Layout = EuFormGenerator.ApplicationFieldLayout;

      // 
      // Create the milestone who have changed arm field
      // 
      optionList = Evado.Model.Digital.EvcStatics.Enumerations.getOptionsFromEnum ( typeof ( EvExportParameters.StatisticalOutputFormatCodes ), true );

      grouField = pageGroup.createSelectionListField (
        EdApplicationSettings.CONST_EXPORT_OUTPUT_FORMAT_FIELD_ID,
        EvLabels.Data_Export_Output_Format_Field_Title,
        this.Session.ExportParameters.OutputFormat.ToString ( ),
        optionList );
      grouField.Layout = EuFormGenerator.ApplicationFieldLayout;

      //
      // Add the update index groupCommand.
      //
      groupCommand = pageGroup.addCommand (
        EvLabels.Data_Export_Update_Index_Command_Title,
        EuAdapter.APPLICATION_ID,
        EuAdapterClasses.Projects.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Custom_Method );

      groupCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.Get_Object );

      groupCommand.SetGuid ( this.Session.Application.Guid );

      groupCommand.AddParameter ( EdApplicationSettings.CONST_EXPORT_COMMAND, EdApplicationSettings.CONST_UPDATE_INDEX_COMMAND );

      groupCommand.AddParameter (
        Model.UniForm.CommandParameters.Page_Id,
        EvPageIds.Data_Point_Export_Page );

      //
      // Add the update ResultData groupCommand.
      //
      groupCommand = pageGroup.addCommand (
        EvLabels.Data_Export_Update_Points_Command_Title,
        EuAdapter.APPLICATION_ID,
        EuAdapterClasses.Projects.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Custom_Method );

      groupCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.Get_Object );

      groupCommand.SetGuid ( this.Session.Application.Guid );

      groupCommand.AddParameter ( EdApplicationSettings.CONST_EXPORT_COMMAND, EdApplicationSettings.CONST_UPDATE_DATA_COMMAND );

      groupCommand.AddParameter (
        Model.UniForm.CommandParameters.Page_Id,
        EvPageIds.Data_Point_Export_Page );

      //
      // Add the update ResultData groupCommand.
      //
      groupCommand = pageGroup.addCommand (
        EvLabels.Data_Export_Output_Points_Command_Title,
        EuAdapter.APPLICATION_ID,
        EuAdapterClasses.Projects.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Custom_Method );

      groupCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.Get_Object );

      groupCommand.SetGuid ( this.Session.Application.Guid );

      groupCommand.AddParameter ( EdApplicationSettings.CONST_EXPORT_COMMAND, EdApplicationSettings.CONST_OUTPUT_DATE_COMMAND );

      groupCommand.AddParameter (
        Model.UniForm.CommandParameters.Page_Id,
        EvPageIds.Data_Point_Export_Page );



    }//END getProjectPage_General_Group method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page object object.</param>
    // ------------------------------------------------------------------------------
    private void getExportPage_Download_Group (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getExportPage_Download_Group" );
      //
      // initialise the methods variables and objects.
      //
      List<Evado.Model.EvOption> optionList = new List<Evado.Model.EvOption> ( );
      Evado.Model.UniForm.Field groupField = new Model.UniForm.Field ( );

      // 
      // Add the status pageMenuGroup
      // 
      Evado.Model.UniForm.Group pageGroup = Page.AddGroup (
       EvLabels.Data_Export_Download_Group_Title,
       String.Empty,
       Evado.Model.UniForm.EditAccess.Inherited_Access );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      String stTempFileName = String.Empty;
      String stTitle = String.Empty;

      //
      // Get the file information.
      //
      System.IO.DirectoryInfo di = new System.IO.DirectoryInfo ( this.UniForm_BinaryFilePath );

      // 
      // Get a reference to each file in that directory.
      // 
      System.IO.FileInfo [ ] fiArr = di.GetFiles ( );

      //
      // Iterate through the list of files.
      //
      for ( int index = 0; index < fiArr.Length; index++ )
      {
        String name = fiArr [ index ].Name.ToLower ( );

        this.LogValue ( "filename: " + fiArr [ index ].Name );

        if ( name.Contains ( this.Session.Application.ApplicationId.ToLower ( ) ) == false )
        {
          continue;
        }
        this.LogValue ( "Is a project file" );


        if ( name.Contains ( "data-point" ) == true
          || name.Contains ( "-data-" ) == true
          || name.Contains ( "-process-log-" ) == true )
        {
          this.LogValue ( "link created." );
          //
          // Generate the links for the log files.
          //
          stTitle = fiArr [ index ].Name;
          stTempFileName = stTitle.Replace ( "_", "-" );
          stTempFileName = stTempFileName.Replace ( " ", "-" );
          stTempFileName = stTempFileName.Replace ( "*", "" );

          groupField = pageGroup.createHtmlLinkField (
           String.Empty,
           stTitle,
           this.UniForm_BinaryServiceUrl + stTempFileName );
          groupField.Layout = EuFormGenerator.ApplicationFieldLayout;
        }
      }//END directory iteration loop.

      this.LogValue ( "Group html links count: " + pageGroup.FieldList.Count );

    }//END getProjectPage_Properties_Group Method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class private create object methods

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData createObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "createObject" );
      this.LogDebug ( "Settings.CustomerGuid: " + this.ClassParameters.CustomerGuid );
      this.LogDebug ( "Settings.ApplicationGuid: " + this.ClassParameters.ApplicationGuid );
      this.LogDebug ( "Session.Customer.Guid: " + this.Session.Customer.Guid );
      try
      {

        //
        // Determine if the user has access to this page and log and error if they do not.
        //
        if ( this.Session.UserProfile.hasTrialManagementAccess == false )
        {
          this.LogIllegalAccess ( this.ClassNameSpace + "createObject",
            this.Session.UserProfile );

          this.ErrorMessage = EvLabels.Illegal_Page_Access_Attempt;

          return null;
        }

        // 
        // Log access to page.
        // 
        this.LogPageAccess ( this.ClassNameSpace + "createObject",
          this.Session.UserProfile );

        // 
        // Initialise the methods variables and objects.
        //      
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
        this.Session.Application = new Model.Digital.EdApplication ( );
        this.Session.Application.Guid = Evado.Model.Digital.EvcStatics.CONST_NEW_OBJECT_ID;

        //
        // if the global project exists, use it to initialise the new project.
        //
        if ( this.ApplicationObjects.GlobalStudy.Guid != Guid.Empty )
        {
          this.Session.Application = this.ApplicationObjects.GlobalStudy;
          this.Session.Application.Guid = Evado.Model.Digital.EvcStatics.CONST_NEW_OBJECT_ID;
          this.Session.Application.Title = String.Empty;
          this.Session.Application.ApplicationId = String.Empty;
          this.Session.Application.Description = EvLabels.Project_Create_Project_Description;
          this.Session.Application.State = Model.Digital.EdApplication.ApplicationStates.Null;
          this.Session.Application.Signoffs = new List<EvUserSignoff> ( );
        }
        //this.Session.Trial.TrialId = "NewTrial";
        LogDebug ( "Trial ID {0}", this.Session.Application.ApplicationId );

        this.getClientDataObject ( clientDataObject );

        // 
        // Save clinical objects  to the session
        // 
        LogDebug ( "Exit createObject method. ID: {0}, Title: {1}", clientDataObject.Id, clientDataObject.Title );

        return clientDataObject;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Project_Creation_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return null;

    }//END createObject method

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
      this.LogMethod ( "updateObject" );
      this.LogValue ( "PageCommand: " + PageCommand.getAsString ( false, false ) );
      this.LogDebug ( "Settings.CustomerGuid: " + this.ClassParameters.CustomerGuid );
      this.LogDebug ( "Settings.ApplicationGuid: " + this.ClassParameters.ApplicationGuid );
      this.LogDebug ( "Session.Customer.Guid: " + this.Session.Customer.Guid );
      try
      {
        this.LogDebug ( "eClinical.Trial" );
        this.LogDebug ( "Guid: " + this.Session.Application.Guid );
        this.LogDebug ( "CustomerGuid: " + this.Session.Application.CustomerGuid );
        this.LogDebug ( "TrialId: " + this.Session.Application.ApplicationId );
        this.LogDebug ( "Title: " + this.Session.Application.Title );

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
        // Set the save action.
        // 
        string stAction = PageCommand.GetParameter ( Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION );

        if ( stAction != String.Empty )
        {
          this.Session.Application.Action = Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue
            <Model.Digital.EdApplication.TrialSavelActionCodes> ( stAction );
        }

        //
        // Substitute the new trial groupCommand id with a new guid for the new trial.
        //
        if ( this.Session.Application.Guid == Evado.Model.Digital.EvcStatics.CONST_NEW_OBJECT_ID )
        {
          this.Session.Application.Guid = Guid.Empty;
        }

        if ( this.Session.Application.CustomerGuid == Guid.Empty
          || this.Session.Application.Guid == Guid.Empty )
        {
          this.Session.Application.CustomerGuid = this.Session.Customer.Guid;
        }

        this.LogDebug ( "Trial.CustomerGuid: " + this.Session.Application.CustomerGuid );
        this.LogDebug ( "Trial.Guid: " + this.Session.Application.Guid );
        this.LogDebug ( "Trial.Action: " + this.Session.Application.Action );
        // 
        // Initialise the update variables.
        // 
        this.Session.Application.UpdatedByUserId = this.Session.UserProfile.UserId;
        this.Session.Application.UserCommonName = this.Session.UserProfile.CommonName;
        //
        // Delete the object.
        // 
        if ( PageCommand.Method == Evado.Model.UniForm.ApplicationMethods.Delete_Object )
        {
          this.Session.Application.Action = Model.Digital.EdApplication.TrialSavelActionCodes.Delete_Application;
        }

        // 
        // update the object.
        // 
        EvEventCodes result = this._Bll_Trials.SaveApplication ( this.Session.Application );

        // 
        // get the debug ResultData.
        // 
        this.LogDebugClass ( this._Bll_Trials.Log );

        // 
        // if an error state is returned create log the event.
        // 
        if ( result != EvEventCodes.Ok )
        {
          string StEvent = this._Bll_Trials.Log + " returned error message: " + Evado.Model.Digital.EvcStatics.getEventMessage ( result );
          this.LogError ( EvEventCodes.Database_Record_Update_Error, StEvent );

          switch ( result )
          {
            case EvEventCodes.Data_Duplicate_Id_Error:
              {
                this.ErrorMessage =
                  String.Format (
                    EvLabels.Project_Duplicate_Identifier_Error_Message,
                    this.Session.Activity.ActivityId );
                break;
              }
            case EvEventCodes.Identifier_Project_Id_Error:
              {
                this.ErrorMessage = EvLabels.Project_Identifier_Empty_Error_Message;
                break;
              }
            default:
              {
                this.ErrorMessage = EvLabels.Project_Update_Error_Message;
                break;
              }
          }
          return this.Session.LastPage;
        }

        //
        // Force a new instance to be loaded.
        //
        this.Session.Application = new Model.Digital.EdApplication ( );

        this.LogDebug ( "Empty the project option list is a project object is updated." );

        this.Session.TrialList = new List<Model.Digital.EdApplication> ( );

        return new Model.UniForm.AppData ( );

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Project_Update_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }
      return this.Session.LastPage;

    }//END method

    // ==================================================================================
    /// <summary>
    /// THis method updates the project object with the page variables..
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns></returns>
    //  ----------------------------------------------------------------------------------
    private void updateObjectValue (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "updateObjectValue" );
      this.LogValue ( "Parameters.Count: " + PageCommand.Parameters.Count );

      // 
      // Iterate through the parameter values updating the ResultData object
      // 
      foreach ( Evado.Model.UniForm.Parameter parameter in PageCommand.Parameters )
      {
        if ( parameter.Name == Evado.Model.Digital.EvcStatics.CONST_GUID_IDENTIFIER
          || parameter.Name == Evado.Model.UniForm.CommandParameters.Custom_Method.ToString ( )
          || parameter.Name == Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION
          || parameter.Name.Contains ( "exp_" ) == true )
        {
          this.LogDebug ( "{0} > {1} >> SKIPPED", parameter.Name, parameter.Value );
          continue;
        }
        try
        {
          Model.Digital.EdApplication.ApplicationFieldNames fieldName = Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue<Model.Digital.EdApplication.ApplicationFieldNames> ( parameter.Name );

          this.Session.Application.setValue ( fieldName, parameter.Value );

          this.LogDebug ( "{0} > {1} >> UPDATED", parameter.Name, parameter.Value );
        }
        catch ( Exception Ex )
        {
          this.LogException ( Ex );
        }

      }// End iteration loop

    }//END updateObjectValue method.

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
  }
}//END namespace