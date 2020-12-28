/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\Actvities.cs" company="EVADO HOLDING PTY. LTD.">
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
using Evado.Bll.Integration;
using Evado.Model.Digital;
// using Evado.Web;

namespace Evado.UniForm.Clinical
{
  /// <summary>
  /// This class manages the integration of Evado eclinical alerts with 
  /// Evado.UniFORM interface.
  /// </summary>
  public class EuActivities : EuClassAdapterBase
  {
    #region Class Initialisation
    /// <summary>
    /// This method initialises the class.
    /// </summary>
    public EuActivities ( )
    {
      this.ClassNameSpace = "Evado.UniFORM.Clinical.EuActivities.";
    }

    /// <summary>
    /// This method initialises the class and passs in the user profile.
    /// </summary>
    public EuActivities (
      EuApplicationObjects ApplicationObjects,
      EvUserProfileBase ServiceUserProfile,
      EuSession SessionObjects,
      String UniFormBinaryFilePath,
      EvClassParameters Settings )
    {
      this.ApplicationObjects = ApplicationObjects;
      this.ServiceUserProfile = ServiceUserProfile;
      this.Session = SessionObjects;
      this.UniForm_BinaryFilePath = UniFormBinaryFilePath;
      this.ClassParameters = Settings;
      this.ClassNameSpace = "Evado.UniFORM.Clinical.EuActivities.";

      this.LogInitMethod ( "EuActivities. initialisation" );
      this.LogInit ( " -ServiceUserProfile.UserId: " + ServiceUserProfile.UserId );
      this.LogInit ( " -SessionObjects.Project.ProjectId: " + this.Session.Application.ApplicationId );
      this.LogInit ( " -.UserProfile.Userid: " + this.Session.UserProfile.UserId );
      this.LogInit ( " -SessionObjects.UserProfile.CommonName: " + this.Session.UserProfile.CommonName );
      this.LogInit ( " -UniFormBinaryFilePath: " + this.UniForm_BinaryFilePath );

      this.LogInit ( "Settings:" );
      this.LogInit ( "-LoggingLevel: " + Settings.LoggingLevel );
      this.LogInit ( "-UserId: " + Settings.UserProfile.UserId );
      this.LogInit ( "-UserCommonName: " + Settings.UserProfile.CommonName );

      this._Bll_Activities = new EvActivities ( Settings );

    }//END Method


    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class constants and variables.

    private const string CONST_FORM_TABLE_FIELD_ID = "FORMS";
    private const string CONST_IMPORT_FIELD_ID = "FILENAME";

    private Evado.Bll.Clinical.EvActivities _Bll_Activities = new Evado.Bll.Clinical.EvActivities ( );


    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class public methods

    // ==================================================================================
    /// <summary>
    /// This method gets the project activity object.
    /// 
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object</param>
    /// <returns>Evado.Model.UniForm.AppData</returns>
    //  ----------------------------------------------------------------------------------
    override public Evado.Model.UniForm.AppData getDataObject (
       Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getDataObject" );
      this.LogDebug ( "PageCommand " + PageCommand.getAsString ( false, false ) );

      try
      {
        // 
        // Initialise the methods variables and objects.
        // 
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );

        string pageId = PageCommand.GetPageId ( );

        if ( pageId != String.Empty )
        {
          this.Session.PageId = Evado.Model.EvStatics.Enumerations.parseEnumValue<EvPageIds> ( pageId );
        }
        this.LogDebug ( "PageId " + this.Session.PageId );

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

    #region Class list object methods

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">List of paremeters to retrieve the selected object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData getListObject (
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
        if ( this.Session.UserProfile.hasTrialManagementAccess == false )
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
          this.ClassNameSpace + "getListObject",
          this.Session.UserProfile );

        clientDataObject.Title = EvLabels.Activity_View_Page_Title;
        clientDataObject.Page.Title = clientDataObject.Title;
        clientDataObject.Id = Guid.NewGuid ( );

        // 
        // Add the page commands.
        // 
        this.getList_PageCommands ( clientDataObject.Page );

        //
        // Display the download group.
        //
        this.getList_DownloadGroup ( clientDataObject.Page );

        //
        // Display the upload gorup.
        //
        this.getList_UploadGroup (
            PageCommand,
            clientDataObject.Page );

        // 
        // Add the activity list group
        // 
        this.getListGroup ( clientDataObject.Page );

        this.LogValue ( " data.Title: " + clientDataObject.Title );
        this.LogValue ( " data.Page.Title: " + clientDataObject.Page.Title );

        this.LogMethodEnd ( "getListObject" );

        return clientDataObject;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Activity_List_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      this.LogMethodEnd ( "getListObject" );

      return this.Session.LastPage;

    }//END getListObject method.

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">List of paremeters to retrieve the selected object.</param>
    //  ------------------------------------------------------------------------------
    private void getList_PageCommands (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getList_PageCommands" );
      // 
      // Initialise method's the variables and objects
      // 
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );

      if ( this.Session.UserProfile.hasTrialManagementAccess == false )
      {
        this.LogMethodEnd ( "getList_PageCommands" );

        return;
      }

      //
      // Add the download command
      //
      if ( this.Session.PageId != EvPageIds.Activity_Download )
      {
        pageCommand = Page.addCommand (
          EvLabels.Activity_Download_Command_Title,
          EuAdapter.APPLICATION_ID,
          EuAdapterClasses.Activities.ToString ( ), Model.UniForm.ApplicationMethods.Custom_Method );

        pageCommand.SetPageId ( EvPageIds.Activity_Download );
        pageCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.List_of_Objects );
      }

      //
      // Add the upload command
      //

      if ( this.Session.PageId != EvPageIds.Activity_Upload )
      {
        pageCommand = Page.addCommand (
          EvLabels.Activity_Upload_Command_Title,
          EuAdapter.APPLICATION_ID,
          EuAdapterClasses.Activities.ToString ( ), Model.UniForm.ApplicationMethods.Custom_Method );

        pageCommand.SetPageId ( EvPageIds.Activity_Upload );
        pageCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.List_of_Objects );
      }
      this.LogMethodEnd ( "getList_PageCommands" );

    }

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">List of paremeters to retrieve the selected object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    /// <remarks>
    /// The method consists of followinf steps:
    /// 
    /// 1. Create a new page menu group for the list of activities.
    /// 
    /// 2. Iterate through each activity in the activity list and add a get command for each activity.
    /// </remarks>
    //  ------------------------------------------------------------------------------
    private void getList_DownloadGroup (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getList_DownloadGroup" );
      //
      // Exit of activity download not selected.
      //
      if ( this.Session.PageId != EvPageIds.Activity_Download )
      {
        this.LogMethodEnd ( "getList_DownloadGroup" );
        return;
      }

      // 
      // Initialise method's the variables and objects
      // 
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      Evado.Bll.Integration.EiCsvServices services = new Evado.Bll.Integration.EiCsvServices ( );
      Evado.Model.Integration.EiData queryData = new Model.Integration.EiData ( );
      List<String> csvOutut = new List<string> ( );
      System.Text.StringBuilder outputData = new StringBuilder ( );
      String fileName = String.Empty;
      String fileUrl = String.Empty;

      queryData.AddQueryParameter (
        Model.Integration.EiQueryParameterNames.Project_Id,
        this.Session.Application.ApplicationId );
      queryData.QueryType = Model.Integration.EiQueryTypes.Activities_Export;

      //
      // Execute the export query.
      //
      csvOutut = services.ExportData ( queryData );

      //
      // Conver the sting list to a text file.
      //
      if ( csvOutut.Count > 0 )
      {
        foreach ( String str in csvOutut )
        {
          outputData.AppendLine ( str );
        }
      }

      //
      // Define the filename and file URL
      //
      fileName = this.Session.Application.ApplicationId
        + "-" + queryData.QueryType + "-data.csv";

      fileUrl = this.UniForm_BinaryServiceUrl + fileName;

      Evado.Model.EvStatics.Files.saveFile ( this.UniForm_BinaryFilePath, fileName, outputData.ToString ( ) );

      // 
      // Create the new pageMenuGroup.
      // 
      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
        EvLabels.Activity_List_Download_Group_Title,
        String.Empty,
        Evado.Model.UniForm.EditAccess.Inherited_Access );

      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      groupField = pageGroup.createHtmlLinkField (
        String.Empty,
        fileName,
        fileUrl );

      this.LogMethodEnd ( "getList_DownloadGroup" );
    }

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">List of paremeters to retrieve the selected object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    /// <remarks>
    /// The method consists of followinf steps:
    /// 
    /// 1. Create a new page menu group for the list of activities.
    /// 
    /// 2. Iterate through each activity in the activity list and add a get command for each activity.
    /// </remarks>
    //  ------------------------------------------------------------------------------
    private void getList_UploadGroup (
      Evado.Model.UniForm.Command PageCommand,
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getList_UploadGroup" );

      // 
      // Initialise method's the variables and objects
      // 
      Evado.Bll.Integration.EiCsvServices services = new Evado.Bll.Integration.EiCsvServices ( );
      Evado.Model.UniForm.Command groupCommand = new Evado.Model.UniForm.Command ( );
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      String filename = String.Empty;
      String processLog = String.Empty;

      //
      // Exit of activity download not selected.
      //
      if ( this.Session.PageId != EvPageIds.Activity_Upload )
      {
        return;
      }

      filename = PageCommand.GetParameter ( EuActivities.CONST_IMPORT_FIELD_ID );

      this.LogValue ( "FileName: " + filename );
      // 
      // Create the new pageMenuGroup.
      // 
      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
        EvLabels.Activity_List_Upload_Group_Title,
        Evado.Model.UniForm.EditAccess.Inherited_Access );

      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      if ( filename != String.Empty )
      {
        this.uploadActivityList ( filename, pageGroup );

        return;
      }

      //
      // Display the binary file upload field
      //
      groupField = pageGroup.createBinaryFileField (
        EuActivities.CONST_IMPORT_FIELD_ID,
        EvLabels.Activity_List_Upload_Field_Title,
        EvLabels.Activity_List_Upload_Description,
        String.Empty );
      groupField.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      //
      // Add the upload command
      //
      groupCommand = pageGroup.addCommand (
        EvLabels.Activity_Upload_Command_Title,
        EuAdapter.APPLICATION_ID,
        EuAdapterClasses.Activities.ToString ( ),
        Model.UniForm.ApplicationMethods.Custom_Method );

      groupCommand.SetPageId ( EvPageIds.Activity_Upload );
      groupCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.List_of_Objects );

      this.LogMethodEnd ( "getList_UploadGroup" );
    }

    // ==============================================================================
    /// <summary>
    /// This method uploads the list of project activities
    /// </summary>
    /// <param name="UploadFilename">String: Activity upload filename.</param>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void uploadActivityList (
      String UploadFilename,
      Evado.Model.UniForm.Group PageGroup )
    {
      this.LogMethod ( "uploadActivityList" );
      this.LogValue ( "UploadFilename: " + UploadFilename );
      // 
      // Initialise method's the variables and objects
      // 
      Evado.Bll.Integration.EiCsvServices services = new Evado.Bll.Integration.EiCsvServices ( );
      Evado.Model.Integration.EiData resultData = new Model.Integration.EiData ( );
      List<String> csvDataList = new List<String> ( );
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );

      if ( Evado.Bll.EvStaticSetting.DebugOn == false )
      {
        Evado.Bll.EvStaticSetting.DebugOn = this.DebugOn;
      }

      //
      // Read in the upload file as a list of string.
      //
      csvDataList = Evado.Model.EvStatics.Files.readFileAsList (
        this.UniForm_BinaryFilePath,
        UploadFilename );

      this.LogValue ( "Files.DebugLog: " + Evado.Model.EvStatics.Files.DebugLog );

      this.LogValue ( "CSV list length: " + csvDataList.Count );

      //
      // Pass the CSV data list for procssing.
      //
      resultData = services.ImportData (
        this.Session.UserProfile,
        Model.Integration.EiQueryTypes.Activities_Import,
        csvDataList );

      this.LogText ( services.Log );

      //
      // Display the upload filename.
      //
      groupField = PageGroup.createReadOnlyTextField (
        String.Empty, "Filename: ", UploadFilename );
      groupField.Layout = EuFormGenerator.ApplicationFieldLayout;

      //
      // display the process log.
      //
      groupField = PageGroup.createFreeTextField (
        String.Empty,
        EvLabels.Activity_List_Upload_Process_Log_Field_Title,
        resultData.ProcessLog,
        90,
        10 );
      groupField.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
      groupField.Layout = EuFormGenerator.ApplicationFieldLayout;

      //
      // Reset the activity list to ensure a refreshed list is generated.
      // i.e. including the new updated activities.
      //
      this.Session.ActivityList = new List<EvActivity> ( );
      this.LogMethodEnd ( "uploadActivityList" );

    }//END getListGroup method.

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">List of paremeters to retrieve the selected object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    /// <remarks>
    /// The method consists of followinf steps:
    /// 
    /// 1. Create a new page menu group for the list of activities.
    /// 
    /// 2. Iterate through each activity in the activity list and add a get command for each activity.
    /// </remarks>
    //  ------------------------------------------------------------------------------
    private void getListGroup (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getListGroup" );
      // 
      // Initialise method's the variables and objects
      // 
      Evado.Model.UniForm.Command groupCommand = new Evado.Model.UniForm.Command ( );

      // 
      // Create the new pageMenuGroup.
      // 
      Evado.Model.UniForm.Group listGroup = Page.AddGroup (
        EvLabels.Activity_List_Group_Title,
        String.Empty,
        Evado.Model.UniForm.EditAccess.Inherited_Access );
      listGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
      listGroup.CmdLayout = Evado.Model.UniForm.GroupCommandListLayouts.Vertical_Orientation;

      if ( this.Session.UserProfile.hasConfigrationEditAccess == true )
      {
        // 
        // Add create new milestone groupCommand
        // 
        groupCommand = listGroup.addCommand (
          EvLabels.Actvitiy_List_New_Actvity_Command_Title,
          EuAdapter.APPLICATION_ID,
          EuAdapterClasses.Activities.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Create_Object );

        groupCommand.SetBackgroundColour (
          Model.UniForm.CommandParameters.BG_Default,
          Model.UniForm.Background_Colours.Purple );
      }

      // 
      // Query and database.
      // 
      if ( this.Session.ActivityList.Count == 0 )
      {
        this.LogInit ( "Loading activity list." );
        this.Session.ActivityList = this._Bll_Activities.getActivityList (
          this.Session.Application.ApplicationId,
          EvActivity.ActivityTypes.Null,
          false );

        this.LogClass ( this._Bll_Activities.Log );
      }

      // 
      // generate the page links.
      // 
      foreach ( EvActivity activity in this.Session.ActivityList )
      {
        Evado.Model.UniForm.Command command = listGroup.addCommand (
          activity.LinkText,
          EuAdapter.APPLICATION_ID,
          EuAdapterClasses.Activities.ToString ( ),
          Model.UniForm.ApplicationMethods.Get_Object );

        command.SetGuid ( activity.Guid );

      }//END trial activity list iteration loop

      this.LogValue ( " command object count: " + listGroup.CommandList.Count );
      this.LogMethodEnd ( "getListGroup" );

    }//END getListGroup method.

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class get object methods

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">List of paremeters to retrieve the selected object.</param>
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
      Guid activityGuid = Guid.Empty;

      //
      // Determine if the user has access to this page and log and error if they do not.
      //
      if ( this.Session.UserProfile.hasTrialManagementAccess == false )
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
      activityGuid = PageCommand.GetGuid ( );
      this.LogValue ( "Activity Guid: " + activityGuid );

      // 
      // return if not trial id
      // 
      if ( activityGuid == Guid.Empty )
      {
        return this.Session.LastPage;
      }

      try
      {
        // 
        // Retrieve the customer object from the database via the DAL and BLL layers.
        // 
        this.Session.Activity = this._Bll_Activities.getActivity ( activityGuid );

        this.LogValue ( this._Bll_Activities.Log );

        this.LogDebug ( "Activity.ActivityId: " + this.Session.Activity.ActivityId );
        this.LogDebug ( "Type: " + this.Session.Activity.Type );

        // 
        // return the client ResultData object for the customer.
        // 
        this.getDataObject ( clientDataObject );

        this.LogMethodEnd ( "getObject" );

        return clientDataObject;
      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Activity_Page_Error_Mesage;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      this.LogMethodEnd ( "getObject" );

      return this.Session.LastPage;

    }//END getObject method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="ClientDataObject">The customer object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject (
      Evado.Model.UniForm.AppData ClientDataObject )
    {
      this.LogMethod ( "getDataObject" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );
      Evado.Model.UniForm.Field pageField = new Evado.Model.UniForm.Field ( );

      //
      // Initialise the client ResultData object.
      //
      ClientDataObject.Id = this.Session.Activity.Guid;
      ClientDataObject.Title = EvLabels.Activity_Page_Title
         + this.Session.Activity.ActivityId
         + EvLabels.Space_Hypen + this.Session.Activity.Title;

      ClientDataObject.Page.Id = ClientDataObject.Id;
      ClientDataObject.Page.Title = ClientDataObject.Title;
      ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Disabled;

      //
      // If the user has edit access enable the page for the user.
      //
      if ( this.Session.UserProfile.hasConfigrationEditAccess == true
      && ( this.Session.Schedule.State == EvSchedule.ScheduleStates.Draft
        || this.Session.Schedule.State == EvSchedule.ScheduleStates.Reviewed ) )
      {
        ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
      }

      this.LogDebug ( "Page.EditAccess: " +  ClientDataObject.Page.EditAccess );

      //
      // Create the general pageMenuGroup page.
      //
      this.getClientData_GeneralGroup ( ClientDataObject.Page );

      //
      // Create the general pageMenuGroup page.
      //
      this.getClientData_Forms_Group ( ClientDataObject.Page );

      this.LogMethodEnd ( "getDataObject" );

    }//END getClientDataObject Method

    // ==============================================================================
    /// <summary>
    /// This method generates the header pageMenuGroup for the page.
    /// </summary>
    /// <param name="PageGroup">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void getClientData_GroupCommands ( 
      Evado.Model.UniForm.Group PageGroup )
    {
      this.LogMethod ( "getClientData_GroupCommands" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );

      // 
      // Add page commands if the user has edit access to them.
      // 
      if ( PageGroup.EditAccess == Model.UniForm.EditAccess.Disabled )
      {
        return;
      }//END user edit access.

      this.LogValue ( "User has edit access" );
      // 
      // Add the save groupCommand
      // 
      pageCommand = PageGroup.addCommand (
        EvLabels.Activity_Save_Command_Title,
        EuAdapter.APPLICATION_ID,
        EuAdapterClasses.Activities.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Save_Object );

      pageCommand.AddParameter (
         Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
        EvActivity.ActivitiesActionsCodes.Save.ToString ( ) );

      pageCommand.SetGuid ( this.Session.Activity.Guid );

      // 
      // Add the delete groupCommand if the activity is at the current schedule version.
      // 
      if ( this.Session.Activity.InitialVersion ==
        this.Session.Schedule.Version
        && this.Session.Activity.Guid != Guid.Empty )
      {
        pageCommand = PageGroup.addCommand (
          EvLabels.Activity_Delete_Command_Title,
          EuAdapter.APPLICATION_ID,
          EuAdapterClasses.Activities.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Save_Object );

        pageCommand.AddParameter (
           Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
          EvActivity.ActivitiesActionsCodes.Delete.ToString ( ) );

        pageCommand.SetGuid ( this.Session.Activity.Guid );
      }


      this.LogMethodEnd ( "getClientData_GroupCommands" );

    }//END Group Command method

    // ==============================================================================
    /// <summary>
    /// This method generates the header pageMenuGroup for the page.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void getClientData_GeneralGroup (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getPage_GeneralGroup" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      List<EvOption> optionList = new List<EvOption> ( );
      bool newActivity = true;

      if ( this.Session.Activity.InitialVersion != this.Session.Schedule.Version )
      {
        newActivity = false;
      }

      // 
      // create the general page pageMenuGroup
      // 
      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
        EvLabels.Activity_Page_Header_Group_Title,
        String.Empty,
        PageObject.EditAccess );

      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;

      //
      // add the page commands
      //
      this.getClientData_GroupCommands ( pageGroup );

      // 
      // Create the trial id object
      // 
      groupField = pageGroup.createReadOnlyTextField (
        EvIdentifiers.TRIAL_ID,
        EvLabels.Label_Project_Id,
        this.Session.Application.ApplicationId
        + EvLabels.Space_Hypen
        + this.Session.Application.Title );
      groupField.Layout = EuFormGenerator.ApplicationFieldLayout;

      // 
      // Create the activity id object
      // 
      groupField = pageGroup.createTextField (
        EvActivity.ActivityClassFieldNames.ActivityId.ToString ( ),
        EvLabels.Activity_Page_ActivityId_Field_Label,
        this.Session.Activity.ActivityId,
        10 );
      groupField.Layout = EuFormGenerator.ApplicationFieldLayout;

      if ( newActivity == false )
      {
        groupField.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
      }

      // 
      // Create the activity title object
      // 
      groupField = pageGroup.createTextField (
        EvActivity.ActivityClassFieldNames.Title.ToString ( ),
        EvLabels.Activity_Page_Title_Field_Label,
        this.Session.Activity.Title,
        50 );
      groupField.Layout = EuFormGenerator.ApplicationFieldLayout;

      // 
      // Create the activity desription object
      // 
      groupField = pageGroup.createFreeTextField (
        EvActivity.ActivityClassFieldNames.Description.ToString ( ),
        EvLabels.Activity_Page_Description_Field_Label,
        this.Session.Activity.Description,
        50,
        4 );
      groupField.Layout = EuFormGenerator.ApplicationFieldLayout;


      // 
      // Create the activity title object
      // 
      if ( newActivity == false )
      {
        groupField = pageGroup.createReadOnlyTextField (
          String.Empty,
          EvLabels.Activity_Initial_Version_Field_Label,
          this.Session.Activity.InitialVersion.ToString ( ) );
        groupField.Layout = EuFormGenerator.ApplicationFieldLayout;
      }

      this.LogMethodEnd ( "getPage_GeneralGroup" );
    }//END getPage_GeneralGroup Method

    // ==============================================================================
    /// <summary>
    /// This method generates the form pageMenuGroup for the page.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    /// <remarks>
    /// This method consists of following steps:
    /// 
    /// 1. create form page group.
    /// 
    /// 2. create a table and table headers in the table field.
    /// 
    /// 3. Iterate through each form in form list and add a row for each one of them in the table.
    /// </remarks>
    //  ------------------------------------------------------------------------------
    private void getClientData_Forms_Group (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getClientData_Forms_Group" );
      this.LogDebug ( "Type: " + this.Session.Activity.Type );
      this.LogDebug ( "EditAccess: " + PageObject.EditAccess );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );
      Evado.Model.UniForm.Field pageField = new Evado.Model.UniForm.Field ( );
      List<EvActvityForm> formList = new List<EvActvityForm> ( );

      //
      // Do not display the form list if activity is new or non clinical.
      //
      if ( this.Session.Activity.Guid == Guid.Empty
        || this.Session.Activity.Type == EvActivity.ActivityTypes.Non_Clinical
        || this.Session.Activity.Type == EvActivity.ActivityTypes.Null )
      {
        this.LogValue ( "Activity does not have forms" );

        this.LogMethodEnd ( "getClientData_Forms_Group" );
        return;
      }

      //
      // If the page is in edit mode this display the list of currently selecte list of forms.
      //
      if ( PageObject.EditAccess == Evado.Model.UniForm.EditAccess.Disabled )
      {
        formList = this.Session.Activity.FormList;
        this.LogValue ( "Readonly: ActivityForm count: " + formList.Count );
      }
      else
      {
        EvActivityForms activityForms = new EvActivityForms ( this.ClassParameters );

        formList = activityForms.getSelectionList (
          this.Session.Application.ApplicationId,
          this.Session.Activity.Guid );

        this.LogClass ( activityForms.Log );

        this.LogValue ( "Selection: ActivityForm count: " + formList.Count );
      }

      this.Session.Activity.FormList = formList;

      // 
      // create the general page pageMenuGroup
      // 
      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
        EvLabels.Activity_Page_Forms_Group_Title,
        String.Empty,
        PageObject.EditAccess );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;

      //
      // add the page commands
      //
      this.getClientData_GroupCommands ( pageGroup );

      // 
      // Create the customer id object
      // 
      pageField = pageGroup.createTableField (
        EuActivities.CONST_FORM_TABLE_FIELD_ID,
        String.Empty, 5 );
      pageField.Layout = Model.UniForm.FieldLayoutCodes.Column_Layout;

      pageField.Table.Header [ 0 ].No = 1;
      pageField.Table.Header [ 0 ].Text = EvLabels.Activity_Page_Form_ID_Column_Title;
      pageField.Table.Header [ 0 ].TypeId = Evado.Model.UniForm.TableColHeader.ItemTypeReadOnly;
      pageField.Table.Header [ 0 ].Width = "20";

      pageField.Table.Header [ 1 ].No = 2;
      pageField.Table.Header [ 1 ].Text = EvLabels.Activity_Page_Form_Title_Column_Title;
      pageField.Table.Header [ 1 ].TypeId = Evado.Model.UniForm.TableColHeader.ItemTypeReadOnly;
      pageField.Table.Header [ 1 ].Width = "40";

      if ( PageObject.EditAccess == Evado.Model.UniForm.EditAccess.Disabled )
      {
        pageField.Table.Header [ 2 ].No = 3;
        pageField.Table.Header [ 2 ].Text = EvLabels.Activity_Page_Form_Mandatory_Column_Title;
        pageField.Table.Header [ 2 ].TypeId = Evado.Model.UniForm.TableColHeader.ItemTypeReadOnly;
        pageField.Table.Header [ 2 ].Width = "10";

        pageField.Table.Header [ 3 ].No = 4;
        pageField.Table.Header [ 3 ].Text = EvLabels.Activity_Page_Form_Selected_Column_Title;
        pageField.Table.Header [ 3 ].TypeId = Evado.Model.UniForm.TableColHeader.ItemTypeReadOnly;
        pageField.Table.Header [ 3 ].Width = "10";
      }
      else
      {
        pageField.Table.Header [ 2 ].No = 3;
        pageField.Table.Header [ 2 ].Text = EvLabels.Activity_Page_Form_Mandatory_Column_Title;
        pageField.Table.Header [ 2 ].TypeId = Evado.Model.UniForm.TableColHeader.ItemTypeYesNo;
        pageField.Table.Header [ 2 ].Width = "10";

        pageField.Table.Header [ 3 ].No = 4;
        pageField.Table.Header [ 3 ].Text = EvLabels.Activity_Page_Form_Selected_Column_Title;
        pageField.Table.Header [ 3 ].TypeId = Evado.Model.UniForm.TableColHeader.ItemTypeYesNo;
        pageField.Table.Header [ 3 ].Width = "10";

        pageField.Table.Header [ 4 ].No = 5;
        pageField.Table.Header [ 4 ].Text = EvLabels.Activity_Page_Form_Order_Column_Title;
        pageField.Table.Header [ 4 ].TypeId = Evado.Model.UniForm.TableColHeader.ItemTypeText;
        pageField.Table.Header [ 4 ].Width = "10";

      }

      //
      // add a row for each form in the list.
      //
      foreach ( EvActvityForm form in formList )
      {
        this.LogValue ( String.Format ( "Form: {0} Sel: {1} Man: {2}", form.FormId, form.Selected, form.Mandatory ) );
        Evado.Model.UniForm.TableRow row = new Model.UniForm.TableRow ( );

        row.Column [ 0 ] = form.FormId;
        row.Column [ 1 ] = form.FormTitle;

        if ( PageObject.EditAccess == Evado.Model.UniForm.EditAccess.Enabled )
        {
          row.Column [ 2 ] = Evado.Model.Digital.EvcStatics.getBoolAsString ( form.Mandatory );
          row.Column [ 3 ] = Evado.Model.Digital.EvcStatics.getBoolAsString ( form.Selected );
        }
        else
        {
          row.Column [ 2 ] = Evado.Model.Digital.EvcStatics.getBoolAsString ( form.Mandatory );
          row.Column [ 3 ] = Evado.Model.Digital.EvcStatics.getBoolAsString ( form.Selected );
          row.Column [ 4 ] = form.Order.ToString ( );
        }

        pageField.Table.Rows.Add ( row );

      }//END iteration looip

      this.LogMethodEnd ( "getClientData_Forms_Group" );

    }//END getClientData_Forms_Group Method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class create object methods

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="Command">Evado.UniForm.Model.ClientClientDataObjectCommand object.</param>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData createObject (
      Evado.Model.UniForm.Command Command )
    {
      try
      {
        this.LogMethod ( "createObject" );

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "createObject",
          this.Session.UserProfile );

        // 
        // Initialise the methods variables and objects.
        //      
        Evado.Model.UniForm.AppData data = new Evado.Model.UniForm.AppData ( );
        this.Session.Activity = new EvActivity ( );
        this.Session.Activity.Guid = Evado.Model.Digital.EvcStatics.CONST_NEW_OBJECT_ID;
        this.Session.Activity.InitialVersion = this.Session.Schedule.Version;
        this.Session.Activity.ProjectId = this.Session.Application.ApplicationId;

          this.Session.Activity.Type = EvActivity.ActivityTypes.Clinical;

        this.getDataObject ( data );

        this.LogMethodEnd ( "createObject" );
        return data;

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
    /// This method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.ClientClientDataObjectCommand object.</param>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData updateObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "updateObject" );
      this.LogDebug ( "Parameter PageCommand: "
        + PageCommand.getAsString ( false, true ) );

      this.LogValue ( "Activity: " );
      this.LogDebug ( "Guid: " + this.Session.Activity.Guid );
      this.LogDebug ( "ProjectId: " + this.Session.Activity.ProjectId );
      this.LogDebug ( "Title: " + this.Session.Activity.Title );

      try
      {
        EvActivity.ActivitiesActionsCodes saveAction = EvActivity.ActivitiesActionsCodes.Save;

        //
        // if updating an activity empty the list of activities so is recreated 
        // with the updated value.
        //
        this.Session.ActivityList = new List<EvActivity> ( );

        //
        // If new is true then this is a new activity so set the Guid to empty
        // so the business layer creates new activcity in the database.
        //
        if ( this.Session.Activity.Guid == Evado.Model.Digital.EvcStatics.CONST_NEW_OBJECT_ID )
        {
          this.Session.Activity.Guid = Guid.Empty;
        }

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "updateObject",
          this.Session.UserProfile );

        // 
        // Delete the object.
        // 
        if ( PageCommand.Method == Evado.Model.UniForm.ApplicationMethods.Delete_Object )
        {
          return this.Session.LastPage;
        }

        // 
        // Update the object.
        // 
        this.updateObjectValue ( PageCommand );

        //
        // Update the form object values.
        //
        this.updateFormObjectValue ( PageCommand );

        // 
        // Get the save action message value.
        // 
        String stSaveAction = PageCommand.GetParameter ( Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION );

        // 
        // Resets the save action to the parameter passed in the page groupCommand.
        // 
        if ( stSaveAction != String.Empty )
        {
          saveAction = Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue<EvActivity.ActivitiesActionsCodes> (
            stSaveAction );
        }
        this.Session.Activity.Action = saveAction;
        this.Session.Activity.UpdatedByUserId = this.Session.UserProfile.UserId;
        this.Session.Activity.UserCommonName = this.Session.UserProfile.UserCommonName;

        // 
        // update the object.
        // 
        EvEventCodes result = this._Bll_Activities.saveActivity ( this.Session.Activity );

        this.LogDebugClass ( this._Bll_Activities.Log );

        // 
        // if an error state is returned create log the event.
        // 
        if ( result != EvEventCodes.Ok )
        {
          string StEvent = this._Bll_Activities.Log + " returned error message: " + Evado.Model.Digital.EvcStatics.getEventMessage ( result );
          this.LogError ( EvEventCodes.Database_Record_Update_Error, StEvent );

          switch ( result )
          {
            case EvEventCodes.Data_Duplicate_Id_Error:
              {
                this.ErrorMessage =
                  String.Format (
                    EvLabels.Activity_Duplicate_Identifier_Error_Message,
                    this.Session.Activity.ActivityId );
                break;
              }
            case EvEventCodes.Identifier_Project_Id_Error:
              {
                this.ErrorMessage = EvLabels.Project_Identifier_Empty_Error_Message;
                break;
              }
            case EvEventCodes.Identifier_Activity_Id_Error:
              {
                this.ErrorMessage = EvLabels.Activity_Identifier_Empty_Error_Message;
                break;
              }
            default:
              {
                this.ErrorMessage = EvLabels.Activity_Update_Error_Message;
                break;
              }
          }
          return this.Session.LastPage;
        }

        return new Model.UniForm.AppData ( );
      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Activity_Update_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }
      return this.Session.LastPage;

    }//END method

    // ==================================================================================
    /// <summary>
    /// THis method updates the EvActivity object member with groupCommand parameter values.
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.PageCommand object.</param>
    //  ----------------------------------------------------------------------------------
    private void updateObjectValue ( Evado.Model.UniForm.Command PageCommand )
    {
      this.LogValue ( Evado.Model.UniForm.EuStatics.CONST_METHOD_START
        + " Evado.UniForm.Clinical.EuActvities.updateObjectValue" );
      this.LogValue ( "Parameters.Count: " + PageCommand.Parameters.Count );

      // 
      // Iterate through the parameter values updating the ResultData object
      // 
      foreach ( Evado.Model.UniForm.Parameter parameter in PageCommand.Parameters )
      {
        if ( parameter.Name.Contains ( Evado.Model.Digital.EvcStatics.CONST_GUID_IDENTIFIER ) == true
          || parameter.Name == Evado.Model.UniForm.CommandParameters.Custom_Method.ToString ( )
          || parameter.Name == Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION
          || parameter.Name.Contains ( EuActivities.CONST_FORM_TABLE_FIELD_ID ) == true )
        {
          continue;
        }

        this.LogValue ( parameter.Name + " > " + parameter.Value + " >> UPDATED" );
        try
        {
          EvActivity.ActivityClassFieldNames fieldName = Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue<EvActivity.ActivityClassFieldNames> ( parameter.Name );

          this.Session.Activity.setValue ( fieldName, parameter.Value );
        }
        catch ( Exception Ex )
        {
          this.LogException ( Ex );

          this.LogMethodEnd ( "updateObjectValue" );
        }

      }// End iteration loop

      this.LogMethodEnd ( "updateObjectValue" );

    }//END method.

    // ==================================================================================
    /// <summary>
    /// This method updates the Activity objects with groupCommand parameter values.
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.PageCommand object.</param>
    //  ----------------------------------------------------------------------------------
    private void updateFormObjectValue ( Evado.Model.UniForm.Command PageCommand )
    {
      this.LogValue ( Evado.Model.UniForm.EuStatics.CONST_METHOD_START
        + this.ClassNameSpace + "updateFormObjectValue" );
      this.LogValue ( "Parameters.Count: " + PageCommand.Parameters.Count );
      this.LogValue ( "Form.Count: " + this.Session.Activity.FormList.Count );

      //
      // Initialise the methods variables and objects.
      //
      EvActivityForms activityForms = new EvActivityForms ( );

      //
      // Iterate through the activity's form list updating the values.
      //
      for ( int iCount = 0; iCount < this.Session.Activity.FormList.Count; iCount++ )
      {
        EvActvityForm activityForm = this.Session.Activity.FormList [ iCount ];

        this.LogValue ( "OBJECT: FormId: " + activityForm.FormId
          + ", Mandatory: " + activityForm.Mandatory
          + ", Selected: " + activityForm.Selected
          + ", Order: " + activityForm.Order );

        String stParameterName = EuActivities.CONST_FORM_TABLE_FIELD_ID + "_" + ( iCount + 1 );

        bool mandatory = Evado.Model.Digital.EvcStatics.getBool ( PageCommand.GetParameter ( stParameterName + "_3" ) );
        bool selected = Evado.Model.Digital.EvcStatics.getBool ( PageCommand.GetParameter ( stParameterName + "_4" ) );
        int order = Evado.Model.Digital.EvcStatics.getInteger ( PageCommand.GetParameter ( stParameterName + "_5" ) );


        this.LogValue ( "PARAMETERS: Mandatory: " + mandatory
          + ", Selected: " + selected
          + ", Order: " + order );
        // 
        // Set select the checklist if mandatory selected.
        // 
        activityForm.Order = order;
        activityForm.Mandatory = mandatory;
        activityForm.Selected = selected;

        if ( activityForm.Mandatory == true )
        {
          activityForm.Selected = true;
        }

        this.LogValue ( "UPDATED OBJECT: Mandatory: " + activityForm.Mandatory
        + ", Selected: " + activityForm.Selected
        + ", Order: " + activityForm.Order );

        //
        // Update the form object if the selection has been changed or
        // it is a new selection.
        //
        if ( activityForm.Selected == true
          && activityForm.Guid == Guid.Empty )
        {
          activityForm.InitialVersion = this.Session.Schedule.Version;
        }

      }//END form list interation loop.

      this.LogValue ( "FINISHED: updateFormObjectValue" );
    }//END updateFormObjectValue method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
  }
}//END namespace