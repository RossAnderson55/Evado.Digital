/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\Schedules.cs" company="EVADO HOLDING PTY. LTD.">
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
  /// This class manages the integration of Evado eclinical alerts with 
  /// Evado.UniFORM interface.
  /// </summary>
  public class EuSchedules : EuClassAdapterBase
  {
    #region Class Initialisation
    /// <summary>
    /// This method initialises the class.
    /// </summary>
    public EuSchedules ( )
    {
      this.ClassNameSpace = " Evado.UniForm.Clinical.EuSchedules.";
    }

    /// <summary>
    /// This method initialises the class and passs in the user profile.
    /// </summary>
    public EuSchedules (
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
      this.ClassNameSpace = " Evado.UniForm.Clinical.EuSchedules.";

      this.LogInitMethod ( "EuSchedules initialisation" );
      this.LogValue ( "ServiceUserProfile.UserId: " + ServiceUserProfile.UserId );
      this.LogValue ( "SessionObjects.Project.ProjectId: " + this.Session.Application.ApplicationId );
      this.LogValue ( "SessionObjects.UserProfile.Userid: " + this.Session.UserProfile.UserId );
      this.LogValue ( "SessionObjects.UserProfile.CommonName: " + this.Session.UserProfile.CommonName );
      this.LogValue ( "UniFormBinaryFilePath: " + this.UniForm_BinaryFilePath );

      //
      // Initialise schedules with the settings object.
      //
      this._Bll_Schedules = new EvSchedules ( Settings );

      this.LogInit ( "Settings.LoggingLevel: " + this.ClassParameters.LoggingLevel );
      this.LogInit ( "Settings.UserProfile.UserId: " + this.ClassParameters.UserProfile.UserId );
      this.LogInit ( "Settings.UserProfile.CommonName: " + this.ClassParameters.UserProfile.CommonName );
      this.LogInit ( "CustomerGuid: " + this.ClassParameters.CustomerGuid );

    }//END Method


    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class constants and variables.

    private const string CONST_MILESTONE_TABLE_FIELD_ID = "SM";
    private const string CONST_SCHEDULE_STATE = "SCH_ST";
    private const string CONST_IMPORT_FIELD_ID = "FILENAME";

    private Evado.Bll.Clinical.EvSchedules _Bll_Schedules = new Evado.Bll.Clinical.EvSchedules ( );


    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class public methods

    // ==================================================================================
    /// <summary>
    /// This method gets the trial site object.
    /// 
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object</param>
    /// <returns>Evado.Model.UniForm.AppData</returns>
    //  ----------------------------------------------------------------------------------
    override public Evado.Model.UniForm.AppData getDataObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getDataObject" );
      this.LogValue ( "PageCommand " + PageCommand.getAsString ( false, false ) );

      try
      {
        // 
        // Initialise the methods variables and objects.
        // 
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
        this.Session.PageId = EvPageIds.Schedule_View;

        //
        // Initialise the scheduling object if they are not already initialised.
        //
        if ( this.Session.ScheduleList == null )
        {
          this.LogDebug ( "Initialising the evSchedule List object" );
          this.Session.ScheduleList = new List<EvSchedule> ( );
        }

        if ( this.Session.Schedule == null )
        {
          this.LogDebug ( "Initialising the evSchedule object" );
          this.Session.Schedule = new EvSchedule ( );
        }

        this.LogDebug ( "Guid: " + this.Session.Schedule.Guid );
        this.LogDebug ( "CustomerGuid: " + this.Session.Schedule.CustomerGuid );
        this.LogDebug ( "TrialId: " + this.Session.Schedule.TrialId );
        this.LogDebug ( "Version: " + this.Session.Schedule.Version );

        string pageId = PageCommand.GetPageId ( );

        if ( pageId != String.Empty )
        {
          this.Session.PageId = Evado.Model.EvStatics.Enumerations.parseEnumValue<EvPageIds> ( pageId );
        }

        this.LogValue ( "current PageId " + this.Session.PageId );

        if ( PageCommand.hasParameter ( EvSchedule.ScheduleClassFieldNames.ScheduleId.ToString ( ) ) == true )
        {
          int scheduleId = EvStatics.getInteger (
            PageCommand.GetParameter ( EvSchedule.ScheduleClassFieldNames.ScheduleId.ToString ( ) ) );

          //
          // If the schedule is out of range set it minimum range.
          //
          if ( scheduleId < EvSchedule.CONST_MINIMUM_SCHEDULE_ID
           || scheduleId > EvSchedule.CONST_MAXIMUM_SCHEDULE_ID )
          {
            this.Session.ScheduleId = EvSchedule.CONST_MINIMUM_SCHEDULE_ID;
          }
          else
          {
            this.Session.ScheduleId = scheduleId;
          }
        }
        this.LogValue ( "ScheduleId: " + this.Session.ScheduleId );

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
        string value = String.Empty;

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "getListObject",
          this.Session.UserProfile );

        this.LogValue ( "selected ScheduleId: " + this.Session.ScheduleId );
        this.LogValue ( "selected ScheduleState: " + this.Session.ScheduleState );
        //
        // Initialise the schedule list page.
        //
        clientDataObject.Title = EvLabels.Schedule_View_Page_Title;
        clientDataObject.Page.Title = clientDataObject.Title;
        clientDataObject.Id = Guid.NewGuid ( );

        //
        // display the page commands
        //
        this.getList_PageCommands ( clientDataObject.Page );

        //
        // desplay the selection group.
        //
        this.getList_SelectionGroup ( clientDataObject.Page );

        //
        // Display the download group.
        //
        this.Schedule_DownloadGroup ( clientDataObject.Page );

        //
        // Display the upload gorup.
        //
        this.Schedule_UploadGroup ( PageCommand, clientDataObject.Page );

        // 
        // Add the trial organisation list to the page.
        // 
        this.getListGroup ( clientDataObject.Page );

        this.LogValue ( " data.Title: " + clientDataObject.Title );
        this.LogValue ( " data.Page.Title: " + clientDataObject.Page.Title );


        return clientDataObject;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Schedule_List_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return null;

    }//END getListObject method.

    // ==============================================================================
    /// <summary>
    /// This method generates the pages command list
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void getList_PageCommands (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getList_PageCommands" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );

      // 
      // Add create new milestone groupCommand
      // 
      pageCommand = PageObject.addCommand (
        EvLabels.Activity_View_Command_Title,
        EuAdapter.APPLICATION_ID,
        EuAdapterClasses.Activities.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

      if ( this.Session.UserProfile.hasTrialManagementAccess == false )
      {
        this.LogMethodEnd ( "getList_PageCommands" );

        return;
      }

      //
      // Add the download command
      //
      if ( this.Session.PageId != EvPageIds.Schedule_Download_Page )
      {
        pageCommand = PageObject.addCommand (
          EvLabels.Schedule_Download_Command_Title,
          EuAdapter.APPLICATION_ID,
          EuAdapterClasses.Schedules.ToString ( ),
          Model.UniForm.ApplicationMethods.Custom_Method );

        pageCommand.SetPageId ( EvPageIds.Schedule_Download_Page );
        pageCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.List_of_Objects );
      }

      //
      // Add the upload command
      //

      if ( this.Session.PageId != EvPageIds.Schedule_Upload_Page )
      {
        pageCommand = PageObject.addCommand (
          EvLabels.Schedule_Upload_Command_Title,
          EuAdapter.APPLICATION_ID,
          EuAdapterClasses.Schedules.ToString ( ),
          Model.UniForm.ApplicationMethods.Custom_Method );

        pageCommand.SetPageId ( EvPageIds.Activity_Upload );
        pageCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.List_of_Objects );
      }

      this.LogMethodEnd ( "getList_PageCommands" );

    }//END getList_PageCommands method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private void getList_SelectionGroup (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getList_SelectionGroup" );

      // 
      // Initialise the methods variables and objects.
      //      
      Evado.Model.UniForm.Command groupCommand = new Evado.Model.UniForm.Command ( );
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      List<EvOption> optionlist = new List<EvOption> ( );

      // 
      // Create the new pageMenuGroup.
      // 
      Evado.Model.UniForm.Group pageGroup = Page.AddGroup (
        EvLabels.Schedule_List_Selection_Group_Title,
        String.Empty,
        Evado.Model.UniForm.EditAccess.Enabled );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      //
      // Get the schedule id selection list.
      //
      optionlist = EvSchedule.getScheduleIdList ( );
      groupField = pageGroup.createSelectionListField (
        EvSchedule.ScheduleClassFieldNames.ScheduleId,
        EvLabels.Schedule_ID_Field_Label,
        this.Session.ScheduleId.ToString ( "##" ),
        optionlist );
      groupField.Layout = EuFormGenerator.ApplicationFieldLayout;
      groupField.AddParameter ( Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );

      //
      // get the schedule version selection list.
      optionlist = EvSchedule.getStateList ( );
      groupField = pageGroup.createSelectionListField (
        EuSchedules.CONST_SCHEDULE_STATE,
        EvLabels.Schedule_State_Field_Label,
        this.Session.ScheduleState.ToString ( ),
        optionlist );
      groupField.Layout = EuFormGenerator.ApplicationFieldLayout;
      groupField.AddParameter ( Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );

      groupCommand = pageGroup.addCommand (
        EvLabels.Schedule_List_Selection_Command_Title,
        EuAdapter.APPLICATION_ID,
        EuAdapterClasses.Schedules.ToString ( ),
         Model.UniForm.ApplicationMethods.Custom_Method );
      groupCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.List_of_Objects );

    }//END getListObject method.

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
    private void Schedule_DownloadGroup (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getList_DownloadGroup" );
      //
      // Exit of activity download not selected.
      //
      if ( this.Session.PageId != EvPageIds.Schedule_Download_Page )
      {
        this.LogMethodEnd ( "Schedule_DownloadGroup" );
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
      queryData.QueryType = Model.Integration.EiQueryTypes.Schedule_Export;

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
        EvLabels.Schedule_List_Download_Group_Title,
        String.Empty,
        Evado.Model.UniForm.EditAccess.Inherited_Access );

      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      groupField = pageGroup.createHtmlLinkField (
        String.Empty,
        fileName,
        fileUrl );

      this.LogMethodEnd ( "Schedule_DownloadGroup" );

    }//END Schedule_DownloadGroup method

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
    private void Schedule_UploadGroup (
      Evado.Model.UniForm.Command PageCommand,
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "Schedule_UploadGroup" );

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

      filename = PageCommand.GetParameter ( EuSchedules.CONST_IMPORT_FIELD_ID );

      this.LogValue ( "FileName: " + filename );
      // 
      // Create the new pageMenuGroup.
      // 
      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
        EvLabels.Schedule_List_Upload_Group_Title,
        Evado.Model.UniForm.EditAccess.Inherited_Access );

      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      if ( filename != String.Empty )
      {
        this.uploadScheduleList ( filename, pageGroup );

        return;
      }

      //
      // Display the binary file upload field
      //
      groupField = pageGroup.createBinaryFileField (
        EuSchedules.CONST_IMPORT_FIELD_ID,
        EvLabels.Schedule_List_Upload_Field_Title,
        EvLabels.Schedule_List_Upload_Description,
        String.Empty );
      groupField.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      //
      // Add the upload command
      //
      groupCommand = pageGroup.addCommand (
        EvLabels.Schedule_Upload_Command_Title,
        EuAdapter.APPLICATION_ID,
        EuAdapterClasses.Activities.ToString ( ),
        Model.UniForm.ApplicationMethods.Custom_Method );

      groupCommand.SetPageId ( EvPageIds.Activity_Upload );
      groupCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.List_of_Objects );

      this.LogMethodEnd ( "Schedule_UploadGroup" );

    }//Enc Schedule_UploadGroup method

    // ==============================================================================
    /// <summary>
    /// This method uploads the list of project activities
    /// </summary>
    /// <param name="UploadFilename">String: Activity upload filename.</param>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void uploadScheduleList (
      String UploadFilename,
      Evado.Model.UniForm.Group PageGroup )
    {
      this.LogMethod ( "uploadScheduleList" );
      this.LogValue ( "UploadFilename: " + UploadFilename );

      try
      {
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
          Model.Integration.EiQueryTypes.Schedule_Import,
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
          EvLabels.Schedule_List_Upload_Process_Log_Field_Title,
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

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Schedule_List_Upload_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      this.LogMethodEnd ( "uploadActivityList" );

    }//END getListGroup method.

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private void getListGroup (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getListGroup" );
      try
      {
        // 
        // Initialise the methods variables and objects.
        //      
        Evado.Model.UniForm.Command groupCommand = new Evado.Model.UniForm.Command ( );

        // 
        // Create the new pageMenuGroup.
        // 
        Evado.Model.UniForm.Group pageGroup = Page.AddGroup (
          EvLabels.Schedule_List_Group_Title,
          String.Empty,
          Evado.Model.UniForm.EditAccess.Inherited_Access );
        pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
        pageGroup.CmdLayout = Evado.Model.UniForm.GroupCommandListLayouts.Vertical_Orientation;

        // 
        // Query and database.
        // 
        this.Session.ScheduleList = this._Bll_Schedules.getScheduleList (
          this.Session.Application.ApplicationId,
          this.Session.ScheduleId,
          this.Session.ScheduleState );

        this.LogClass ( this._Bll_Schedules.Log );

        this.LogValue ( "Returned schedule count: " + this.Session.ScheduleList.Count );

        // 
        // bind output to the datagrid.
        // 
        if ( this.Session.ScheduleList.Count == 0 )  // If nothing returned create a blank row.
        {
          // 
          // Add the page commands.
          // 
          //this.getList_PageCommands ( Page );

          this.Session.ScheduleList.Add ( new EvSchedule ( ) );
        }

        // 
        // generate the page links.
        // 
        foreach ( EvSchedule schedule in this.Session.ScheduleList )
        {
          if ( schedule.Guid != Guid.Empty )
          {
            //
            // Switch statement to select the correct ICON.
            //
            switch ( schedule.State )
            {
              case EvSchedule.ScheduleStates.Draft:
                {
                  groupCommand.AddParameter (
                   Model.UniForm.CommandParameters.Image_Url,
                   EuForms.ICON_FORM_DRAFT );
                  break;
                }
              case EvSchedule.ScheduleStates.Reviewed:
                {
                  groupCommand.AddParameter (
                   Model.UniForm.CommandParameters.Image_Url,
                   EuForms.ICON_FORM_REVIEWED );
                  break;
                }
              case EvSchedule.ScheduleStates.Issued:
                {
                  groupCommand.AddParameter (
                   Model.UniForm.CommandParameters.Image_Url,
                   EuForms.ICON_FORM_ISSUED );
                  break;
                }
            }//END state switch.

            if ( schedule.Title == EvLabels.Schedule_Default_Title
              && schedule.ScheduleId > 1 )
            {
              schedule.Title = String.Format ( EvLabels.Schedule_Other_Title, schedule.ScheduleId );
            }

            Evado.Model.UniForm.Command command = pageGroup.addCommand (
              schedule.LinkText,
              EuAdapter.APPLICATION_ID,
              EuAdapterClasses.Schedules.ToString ( ),
              Model.UniForm.ApplicationMethods.Get_Object );

            command.AddParameter (
              Model.UniForm.CommandParameters.Short_Title,
              EvLabels.Label_Version + schedule.Version );

            command.SetGuid ( schedule.Guid );
          }
          else
          {
            Evado.Model.UniForm.Command command = pageGroup.addCommand (
              EvLabels.Schedule_Empty_Schedule_Command_Title,
              EuAdapter.APPLICATION_ID,
              EuAdapterClasses.Schedules.ToString ( ),
              Model.UniForm.ApplicationMethods.Create_Object );

            command.AddParameter (
              Model.UniForm.CommandParameters.Short_Title,
              EvLabels.Schedule_Empty_Schedule_Command_Title );

            command.SetBackgroundColour (
              Model.UniForm.CommandParameters.BG_Default,
              Model.UniForm.Background_Colours.Purple );
          }

        }//END schedule list iteration loop

        this.LogValue ( " command object count: " + pageGroup.CommandList.Count );

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Schedule_List_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

    }//END getListObject method.

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class get object methods

    // ==============================================================================
    /// <summary>
    /// This method returns a client application application data object
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
      Guid scheuleGuid = Guid.Empty;

      // 
      // Log access to page.
      // 
      this.LogPageAccess (
        this.ClassNameSpace + "getObject",
        this.Session.UserProfile );


      try
      {
        //
        // Load the schedule object if command schedule guid is not empty.
        //
        if ( this.loadScheduleObject ( PageCommand ) == false )
        {
          return clientDataObject;
        }

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
        this.ErrorMessage = EvLabels.Schedule_Page_Error_Mesage;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      this.LogDebug ( "Return Null" );
      this.LogMethodEnd ( "getObject" );
      return null;

    }//END getObject method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application application data object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private bool loadScheduleObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "loadScheduleObject" );
      // 
      // Initialise the methods variables and objects.
      // 
      Guid scheduleId = Guid.Empty;

      // 
      // if the parameter value exists then set the customerId
      // 
      scheduleId = PageCommand.GetGuid ( );
      this.LogValue ( "scheuleGuid: " + scheduleId );

      // 
      // return if not guids not defined.
      // 
      if ( scheduleId == Guid.Empty
        && this.Session.Schedule.Guid == Guid.Empty )
      {
        this.LogDebug ( "schedule Guid is empty" );
        this.LogMethodEnd ( "loadScheduleObject" );
        return false;
      }

      //
      // return true if the schedule loaded but command schedule is empty.
      //
      if ( this.Session.Schedule.Guid != Guid.Empty
       && scheduleId == Guid.Empty )
      {
        this.LogDebug ( "Schedule is already loaded." );
        this.LogMethodEnd ( "loadScheduleObject" );
        return true;
      }

      //
      // return true if the scheduleGuid is empty but one is in session.
      //
      if ( this.Session.Schedule.Guid == scheduleId )
      {
        this.LogDebug ( "Schedule is already loaded." );
        this.LogMethodEnd ( "loadScheduleObject" );
        return true;
      }

      // 
      // Retrieve the customer object from the database via the DAL and BLL layers.
      // 
      this.Session.Schedule = this._Bll_Schedules.getSchedule ( scheduleId );

      this.LogClass ( this._Bll_Schedules.Log );

      this.LogMethodEnd ( "loadScheduleObject" );
      return true;
    }


    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="ClientDataObject"> Evado.Model.UniForm.AppData object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject (
      Evado.Model.UniForm.AppData ClientDataObject )
    {
      this.LogMethod ( "getClientDataObject" );
      this.LogDebug ( "Edit Access: " + this.Session.UserProfile.hasConfigrationEditAccess );
      //
      // Initialise the client data object.
      //
      ClientDataObject.Id = this.Session.Schedule.Guid;
      ClientDataObject.Title = EvLabels.Schedule_Page_Title + this.Session.Schedule.Version;

      ClientDataObject.Page.Id = ClientDataObject.Id;
      ClientDataObject.Page.Title = ClientDataObject.Title;
      ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      //
      // If the user has edit access enable the page for the user.
      //
      if ( this.Session.UserProfile.hasConfigrationEditAccess == true
        && ( this.Session.Schedule.State == EvSchedule.ScheduleStates.Draft
          || this.Session.Schedule.State == EvSchedule.ScheduleStates.Reviewed ) )
      {
        ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
      }


      //
      // Get the page commands.
      //
      this.getClientData_PageCommands ( ClientDataObject.Page );

      //
      // Create the general pageMenuGroup page.
      //
      this.getClientData_GeneralGroup ( ClientDataObject.Page );

      //
      // Display the following if the schedule has a primary key.
      //
      if ( this.Session.Schedule.Guid != Guid.Empty )
      {
        //
        // Create the general pageMenuGroup page.
        //
        this.getClientData_MilestonesGroup ( ClientDataObject.Page );

        //
        // Get the schedule signoff record.
        //
        this.getClientData_SignoffLog ( ClientDataObject.Page );
      }

    }//END getClientDataObject Method

    // ==============================================================================
    /// <summary>
    /// This method generates the pages command list
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void getClientData_PageCommands (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getClientData_PageCommands" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );

      // 
      // Add create new milestone groupCommand
      // 
      pageCommand = PageObject.addCommand (
        EvLabels.Activity_View_Command_Title,
        EuAdapter.APPLICATION_ID,
        EuAdapterClasses.Activities.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

      if ( this.Session.UserProfile.hasTrialManagementAccess == false )
      {
        this.LogMethodEnd ( "getList_PageCommands" );

        return;
      }

      //
      // Add the download command
      //
      if ( this.Session.PageId != EvPageIds.Schedule_Download_Page )
      {
        pageCommand = PageObject.addCommand (
          EvLabels.Schedule_Download_Command_Title,
          EuAdapter.APPLICATION_ID,
          EuAdapterClasses.Schedules.ToString ( ),
          Model.UniForm.ApplicationMethods.Custom_Method );

        pageCommand.SetPageId ( EvPageIds.Schedule_Download_Page );
        pageCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.List_of_Objects );
      }

      //
      // Add the upload command
      //
      if ( this.Session.PageId != EvPageIds.Schedule_Upload_Page )
      {
        pageCommand = PageObject.addCommand (
          EvLabels.Schedule_Upload_Command_Title,
          EuAdapter.APPLICATION_ID,
          EuAdapterClasses.Schedules.ToString ( ),
          Model.UniForm.ApplicationMethods.Custom_Method );

        pageCommand.SetPageId ( EvPageIds.Activity_Upload );
        pageCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.List_of_Objects );
      }

      // 
      // Add page commands if the user has edit access to them.
      // 
      if ( this.Session.UserProfile.hasConfigrationEditAccess == true )
      {
        this.LogValue ( "User has edit access" );

        switch ( this.Session.Schedule.State )
        {
          // 
          // Add draft state commands.
          // 
          case EvSchedule.ScheduleStates.Draft:
            {
              // 
              // Add create new milestone groupCommand
              // 
              pageCommand = PageObject.addCommand (
                EvLabels.Schedule_New_Milestone_Command_Title,
                EuAdapter.APPLICATION_ID,
                EuAdapterClasses.Milestones.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.Create_Object );

              // 
              // Add the save groupCommand
              // 
              pageCommand = PageObject.addCommand (
                EvLabels.Schedule_Save_Command_Title,
                EuAdapter.APPLICATION_ID,
                EuAdapterClasses.Schedules.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.Save_Object );

              pageCommand.AddParameter (
                 Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
                EvSchedule.ScheduleActions.Save.ToString ( ) );

              pageCommand.SetGuid ( this.Session.Schedule.Guid );

              // 
              // Add the delete groupCommand
              // 
              pageCommand = PageObject.addCommand (
                EvLabels.Schedule_Delete_Command_Title,
                EuAdapter.APPLICATION_ID,
                EuAdapterClasses.Schedules.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.Save_Object );

              pageCommand.AddParameter (
                 Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
                EvSchedule.ScheduleActions.Delete_Schedule.ToString ( ) );

              pageCommand.SetGuid ( this.Session.Schedule.Guid );

              //
              // Add review groupCommand
              //
              pageCommand = PageObject.addCommand (
                EvLabels.Schedule_Review_Command_Title,
                EuAdapter.APPLICATION_ID,
                EuAdapterClasses.Schedules.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.Save_Object );

              pageCommand.AddParameter (
                 Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
                EvSchedule.ScheduleActions.Review.ToString ( ) );

              pageCommand.SetGuid ( this.Session.Schedule.Guid );

              break;
            }
          // Add reviewed state commands.
          // 
          case EvSchedule.ScheduleStates.Reviewed:
            {
              //
              // Add approval groupCommand
              //
              pageCommand = PageObject.addCommand (
                EvLabels.Schedule_Approve_Command_Title,
                EuAdapter.APPLICATION_ID,
                EuAdapterClasses.Schedules.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.Save_Object );

              pageCommand.AddParameter (
                 Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
                EvSchedule.ScheduleActions.Approve.ToString ( ) );

              pageCommand.SetGuid ( this.Session.Schedule.Guid );

              break;
            }
          // 
          // Add issued state commands.
          // 
          case EvSchedule.ScheduleStates.Issued:
            {
              if ( this.lastIssuedSchedule ( ) == true )
              {
                pageCommand = PageObject.addCommand (
                  EvLabels.Schedule_Revise_Command_Title,
                  EuAdapter.APPLICATION_ID,
                  EuAdapterClasses.Schedules.ToString ( ),
                  Evado.Model.UniForm.ApplicationMethods.Save_Object );

                pageCommand.AddParameter (
                   Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
                  EvSchedule.ScheduleActions.Revise.ToString ( ) );

                pageCommand.SetGuid ( this.Session.Schedule.Guid );
              }
              break;
            }

        }//END state switch 

      }//END user edit access.

      this.LogMethodEnd ( "getClientData_PageCommands" );

    }//END getClientData_PageCommands method

    // ==============================================================================
    /// <summary>
    /// This method tests to see if there are any schedules with a specific state 
    /// in the list of selected schedules.
    /// </summary>
    //  ------------------------------------------------------------------------------
    private bool lastIssuedSchedule ( )
    {
      this.LogMethod ( "lastIssuedSchedule" );

      for ( int count = 0; count < this.Session.ScheduleList.Count - 1; count++ )
      {
        EvSchedule schedule = this.Session.ScheduleList [ count ];

        if ( schedule.State != EvSchedule.ScheduleStates.Issued )
        {
          continue;
        }

        if ( schedule.Guid == this.Session.Schedule.Guid )
        {
          return false;
        }

      }//END iteration loop

      return true;
    }

    // ==============================================================================
    /// <summary>
    /// This method tests to see if there are any schedules with a specific state 
    /// in the list of selected schedules.
    /// </summary>
    /// <param name="State">EvSchedule.ScheduleStates enumerated list.</param>
    //  ------------------------------------------------------------------------------
    private bool hasScheduleState ( EvSchedule.ScheduleStates State )
    {
      foreach ( EvSchedule schedule in this.Session.ScheduleList )
      {
        if ( schedule.State == State )
        {
          return true;
        }

      }//END iteration loop

      return false;
    }

    // ==============================================================================
    /// <summary>
    /// This method generates the generate group fields and commands
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
      Evado.Model.UniForm.Command groupCommand = new Evado.Model.UniForm.Command ( );
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      List<EvOption> optionlist = new List<EvOption> ( );

      // 
      // create the general page pageMenuGroup
      // 
      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
        EvLabels.Schedule_Page_Header_Group_Title,
        String.Empty,
        Evado.Model.UniForm.EditAccess.Inherited_Access );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;

      pageGroup.SetCommandBackBroundColor (
        Model.UniForm.GroupParameterList.BG_Mandatory,
        Model.UniForm.Background_Colours.Red );

      //
      // Set the schedule id and title defaults.
      //
      if ( this.Session.Schedule.ScheduleId < 1 )
      {
        this.Session.Schedule.ScheduleId = 1;
        this.Session.Schedule.Title = EvLabels.Schedule_Default_Title;
      }

      //
      // reset the default schedule title if the schedule id is greater than 1
      //
      if ( this.Session.Schedule.Title == EvLabels.Schedule_Default_Title
        && this.Session.Schedule.ScheduleId > 1 )
      {
        this.Session.Schedule.Title = String.Format (
          EvLabels.Schedule_Other_Title,
          this.Session.Schedule.ScheduleId );
      }

      // 
      // Create the schedule id object
      // 
      groupField = pageGroup.createReadOnlyTextField (
        EvSchedule.ScheduleClassFieldNames.ScheduleId.ToString ( ),
        EvLabels.Schedule_ID_Field_Label,
        this.Session.Schedule.ScheduleId.ToString ( "00" ) );
      groupField.Layout = EuFormGenerator.ApplicationFieldLayout;

      // 
      // Create the schedule title object
      // 
      this.LogValue ( "Schedule.Title: " + this.Session.Schedule.Title );
      groupField = pageGroup.createTextField (
        EvSchedule.ScheduleClassFieldNames.Title.ToString ( ),
        EvLabels.Schedule_Title_Field_Label,
        this.Session.Schedule.Title,
        100 );
      groupField.Layout = EuFormGenerator.ApplicationFieldLayout;

      // 
      // Create the description object
      // 
      groupField = pageGroup.createFreeTextField (
        EvSchedule.ScheduleClassFieldNames.Description.ToString ( ),
        EvLabels.Schedule_Description_Field_Label,
        this.Session.Schedule.Description,
        50,
        4 );
      groupField.Layout = EuFormGenerator.ApplicationFieldLayout;

        this.Session.Schedule.Type = EvSchedule.ScheduleTypes.Clinical;

      // 
      // Create the milestone increment field object
      // 
      optionlist = EvSchedule.getPeriodIncrementList ( );

      groupField = pageGroup.createSelectionListField (
        EvSchedule.ScheduleClassFieldNames.Milestone_Period_Increment.ToString ( ),
        EvLabels.Schedule_Milestone_Increments_Field_Label,
        this.Session.Schedule.MilestonePeriodIncrement.ToString ( ),
        optionlist );

      groupField.Layout = EuFormGenerator.ApplicationFieldLayout;

      // 
      // Create the customer id object
      // 
      groupField = pageGroup.createReadOnlyTextField (
        String.Empty,
        EvLabels.Schedule_State_Field_Label,
        this.Session.Schedule.StateDesc );
      groupField.Layout = EuFormGenerator.ApplicationFieldLayout;

      //
      // Add the groups commands
      //
      this.getClientData_GroupCommands ( pageGroup );

    }//END getPage_GeneralGroup Method

    // ==============================================================================
    /// <summary>
    /// This method generatate the groups commands
    /// </summary>
    /// <param name="Group">Evado.Model.UniForm.Group object.</param>
    //  ------------------------------------------------------------------------------
    private void getClientData_GroupCommands (
      Evado.Model.UniForm.Group Group )
    {
      this.LogMethod ( "getClientData_GroupCommands" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );

      // 
      // Add page commands if the user has edit access to them.
      // 
      if ( this.Session.UserProfile.hasConfigrationEditAccess == true )
      {
        this.LogValue ( "User has edit access" );

        switch ( this.Session.Schedule.State )
        {
          // 
          // Add draft state commands.
          // 
          case EvSchedule.ScheduleStates.Draft:
            {
              // 
              // Add create new milestone groupCommand
              // 
              pageCommand = Group.addCommand (
                EvLabels.Schedule_New_Milestone_Command_Title,
                EuAdapter.APPLICATION_ID,
                EuAdapterClasses.Milestones.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.Create_Object );

              // 
              // Add the save groupCommand
              // 
              pageCommand = Group.addCommand (
                EvLabels.Schedule_Save_Command_Title,
                EuAdapter.APPLICATION_ID,
                EuAdapterClasses.Schedules.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.Save_Object );

              pageCommand.AddParameter (
                 Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
                EvSchedule.ScheduleActions.Save.ToString ( ) );

              pageCommand.SetGuid ( this.Session.Schedule.Guid );

              // 
              // Add the delete groupCommand
              // 
              pageCommand = Group.addCommand (
                EvLabels.Schedule_Delete_Command_Title,
                EuAdapter.APPLICATION_ID,
                EuAdapterClasses.Schedules.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.Save_Object );

              pageCommand.AddParameter (
                 Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
                EvSchedule.ScheduleActions.Delete_Schedule.ToString ( ) );

              pageCommand.SetGuid ( this.Session.Schedule.Guid );

              //
              // Add review groupCommand
              //
              pageCommand = Group.addCommand (
                EvLabels.Schedule_Review_Command_Title,
                EuAdapter.APPLICATION_ID,
                EuAdapterClasses.Schedules.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.Save_Object );

              pageCommand.AddParameter (
                 Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
                EvSchedule.ScheduleActions.Review.ToString ( ) );

              pageCommand.SetGuid ( this.Session.Schedule.Guid );

              break;
            }
          // Add reviewed state commands.
          // 
          case EvSchedule.ScheduleStates.Reviewed:
            {
              //
              // Add approval groupCommand
              //
              pageCommand = Group.addCommand (
                EvLabels.Schedule_Approve_Command_Title,
                EuAdapter.APPLICATION_ID,
                EuAdapterClasses.Schedules.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.Save_Object );

              pageCommand.AddParameter (
                 Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
                EvSchedule.ScheduleActions.Approve.ToString ( ) );

              pageCommand.SetGuid ( this.Session.Schedule.Guid );

              break;
            }
          // 
          // Add issued state commands.
          // 
          case EvSchedule.ScheduleStates.Issued:
            {
              if ( this.lastIssuedSchedule ( ) == true )
              {
                pageCommand = Group.addCommand (
                  EvLabels.Schedule_Revise_Command_Title,
                  EuAdapter.APPLICATION_ID,
                  EuAdapterClasses.Schedules.ToString ( ),
                  Evado.Model.UniForm.ApplicationMethods.Save_Object );

                pageCommand.AddParameter (
                   Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
                  EvSchedule.ScheduleActions.Revise.ToString ( ) );

                pageCommand.SetGuid ( this.Session.Schedule.Guid );
              }
              break;
            }

        }//END state switch 

      }//END user edit access.

    }//END getClientData_PageCommands method

    // ==============================================================================
    /// <summary>
    /// This method generates milestone list group
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void getClientData_MilestonesGroup (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getPage_GeneralGroup" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Command groupCommand = new Evado.Model.UniForm.Command ( );
      Evado.Model.UniForm.Field pageField = new Evado.Model.UniForm.Field ( );
      List<EvMilestone> milestoneList = new List<EvMilestone> ( );
      EvMilestones milestones = new EvMilestones ( );

      // 
      // create the general page pageMenuGroup
      // 
      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
        EvLabels.Schedule_Page_Milestones_Group_Title,
        String.Empty,
        Evado.Model.UniForm.EditAccess.Inherited_Access );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;

      pageGroup.CmdLayout = Evado.Model.UniForm.GroupCommandListLayouts.Vertical_Orientation;


      // 
      // Get the currentSchedule of milestones
      // 
      milestoneList = milestones.getMilestoneList (
       this.Session.Schedule.Guid,
       EvMilestone.MilestoneTypes.Null );

      this.LogValue ( "Milestone debug: " + milestones.DebugLog );

      // 
      // Add create new milestone groupCommand
      //
      if ( this.Session.Schedule.State == EvSchedule.ScheduleStates.Draft
        || this.Session.Schedule.Guid == Evado.Model.Digital.EvcStatics.CONST_NEW_OBJECT_ID )
      {
        groupCommand = pageGroup.addCommand (
          EvLabels.Schedule_New_Milestone_Command_Title,
          EuAdapter.APPLICATION_ID,
          EuAdapterClasses.Milestones.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Create_Object );

        groupCommand.SetBackgroundColour (
          Model.UniForm.CommandParameters.BG_Default,
          Model.UniForm.Background_Colours.Purple );

        groupCommand.AddParameter ( EuMilestones.CONST_MILESTONE_ORDER,
          ( milestoneList.Count * 2 + 1 ).ToString ( ) );
      }

      //
      // add a row for each form in the list.
      //
      foreach ( EvMilestone milestone in milestoneList )
      {
        groupCommand = pageGroup.addCommand ( milestone.LinkText,
           EuAdapter.APPLICATION_ID,
           EuAdapterClasses.Milestones.ToString ( ),
           Model.UniForm.ApplicationMethods.Get_Object );

        groupCommand.SetGuid ( milestone.Guid );

      }//END milestone iteration loop

    }//END getPage_GeneralGroup Method

    // ==============================================================================
    /// <summary>
    /// This method creates the schedule signoff group
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object object.</param>
    // ------------------------------------------------------------------------------
    private void getClientData_SignoffLog (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getClientData_SignoffLog" );

      // 
      // Display comments it they exist.
      // 
      if ( this.Session.Schedule.Signoffs.Count == 0 )
      {
        this.LogValue ( EvLabels.Label_No_Signoff_Label );
        return;
      }

      // 
      // create the page pageMenuGroup
      // 
      Evado.Model.UniForm.Field pageField = new Evado.Model.UniForm.Field ( );
      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
        EvLabels.Label_Signoff_Log_Field_Title,
        Evado.Model.UniForm.EditAccess.Enabled );

      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      pageField = pageGroup.createReadOnlyTextField (
        String.Empty,
        EvLabels.Label_Signoff_Log_Field_Title,
        String.Empty,
        EvUserSignoff.getSignoffLog ( this.Session.Schedule.Signoffs, false ) );
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;

    }//END getClientClientDataObjectObject Method


    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class create object methods

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.ClientClientDataObjectCommand object.</param>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData createObject ( Evado.Model.UniForm.Command PageCommand )
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

        if ( this.Session.ScheduleId < EvSchedule.CONST_MINIMUM_SCHEDULE_ID
        || this.Session.ScheduleId > EvSchedule.CONST_MAXIMUM_SCHEDULE_ID )
        {
          this.LogValue ( "The scheduleId is out of range." );

          return null;
        }


        // 
        // Initialise the methods variables and objects.
        //      
        Evado.Model.UniForm.AppData data = new Evado.Model.UniForm.AppData ( );
        this.Session.Schedule = new EvSchedule ( );
        this.Session.Schedule.CustomerGuid = this.Session.Customer.Guid;
        this.Session.Schedule.ScheduleId = this.Session.ScheduleId;
        this.Session.Schedule.State = EvSchedule.ScheduleStates.Draft;
        this.Session.Schedule.TrialId = this.Session.Application.ApplicationId;
        this.Session.Schedule.UpdatedByUserId = this.Session.UserProfile.UserId;
        this.Session.Schedule.UserCommonName = this.Session.UserProfile.CommonName;
        this.Session.Schedule.Action = EvSchedule.ScheduleActions.Save;

        //
        // Set the schedule's default title.
        //
        this.setDefaultScheduleTitle ( );

        //
        // Save the new schedule so milestones can been added.
        //
        this._Bll_Schedules.saveSchedule ( this.Session.Schedule );

        this.LogClass ( this._Bll_Schedules.Log );

        this.Session.ScheduleList = this._Bll_Schedules.getScheduleList (
          this.Session.Application.ApplicationId,
          this.Session.ScheduleId,
          this.Session.ScheduleState );

        //
        // If nothing is returned exit with an error.
        //
        if ( this.Session.ScheduleList.Count == 0 )
        {
          this.ErrorMessage = EvLabels.Schedule_Creating_Error_Message;
          return null;
        }

        for ( int index = 0; index < this.Session.ScheduleList.Count; index++ )
        {
          if ( this.Session.ScheduleList [ index ].State == EvSchedule.ScheduleStates.Draft )
          {
            this.Session.Schedule.Guid = this.Session.ScheduleList [ index ].Guid;
          }
        }
        this.Session.ScheduleList = new List<EvSchedule> ( );

        this.LogDebug ( "Schedule Guid: " + this.Session.Schedule.Guid );
        this.LogDebug ( "Schedule ScheduleId: " + this.Session.Schedule.ScheduleId );

        this.getDataObject ( data );


        this.LogValue ( "Exit createObject method. ID: " + data.Id );

        return data;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Schedule_Creating_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return null;

    }//END method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class update object methods

    // ==================================================================================
    /// <summary>
    /// This method sets the default schedule title.s
    /// </summary>
    //  ----------------------------------------------------------------------------------
    private void setDefaultScheduleTitle ( )
    {
      this.LogMethod ( "setDefaultScheduleTitle" );

      if ( this.Session.Schedule.ScheduleId == 1
        && this.Session.Schedule.Title == String.Empty )
      {
        this.Session.Schedule.Title = EvLabels.Schedule_Default_Title;
      }

      if ( this.Session.Schedule.ScheduleId > 1
        && ( this.Session.Schedule.Title == String.Empty
          || this.Session.Schedule.Title == EvLabels.Schedule_Default_Title ) )
      {
        this.Session.Schedule.Title = String.Format ( EvLabels.Schedule_Other_Title, this.Session.Schedule.ScheduleId );
      }

      string otherSchedule = String.Format ( EvLabels.Schedule_Other_Title, this.Session.Schedule.ScheduleId );

      this.LogDebug ( "otherSchedule: " + otherSchedule );
      this.LogDebug ( "Schedule.ScheduleId: " + this.Session.Schedule.ScheduleId );
      this.LogDebug ( "Schedule.Title: " + this.Session.Schedule.Title );
      this.LogDebug ( "Schedule.Type: " + this.Session.Schedule.Type );

      if ( this.Session.Schedule.Type == EvSchedule.ScheduleTypes.PRO_Schedule
        && ( this.Session.Schedule.Title == EvLabels.Schedule_Default_Title
          || this.Session.Schedule.Title.Trim ( ) == otherSchedule.Trim ( ) ) )
      {
        this.Session.Schedule.Title = EvLabels.Scheduled_PRO_Title;
      }

      this.LogDebug ( "New Schedule.Title: " + this.Session.Schedule.Title );
    }


    // ==================================================================================
    /// <summary>
    /// This method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.ClientClientDataObjectCommand object.</param>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData updateObject ( Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "updateObject" );
      this.LogValue ( "Parameter PageCommand: " + PageCommand.getAsString ( false, true ) );
      try
      {
        //
        // initialise the methods variables and objects.
        //
        this.LogDebug ( "Guid: " + this.Session.Schedule.Guid );
        this.LogDebug ( "TrialId: " + this.Session.Schedule.TrialId );
        this.LogDebug ( "Version: " + this.Session.Schedule.Version );
        EvSchedule.ScheduleActions saveAction = EvSchedule.ScheduleActions.Save;

        //
        // If new is true then this is a new activity so set the Guid to empty
        // so the business layer creates new activcity in the database.
        //
        if ( this.Session.Schedule.Guid == Evado.Model.Digital.EvcStatics.CONST_NEW_OBJECT_ID )
        {
          this.Session.Schedule.Guid = Guid.Empty;
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
          this.LogMethodEnd ( "updateObject" );
          return this.Session.LastPage;
        }

        // 
        // Update the object.
        // 
        this.updateObjectValue ( PageCommand );

        //
        // Set the schedule's default title.
        //
        this.setDefaultScheduleTitle ( );

        // 
        // Get the save action message value.
        // 
        String stSaveAction = PageCommand.GetParameter ( Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION );

        // 
        // Resets the save action to the parameter passed in the page groupCommand.
        // 
        if ( stSaveAction != String.Empty )
        {
          saveAction = Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue<EvSchedule.ScheduleActions> ( stSaveAction );
        }
        this.Session.Schedule.Action = saveAction;
        this.Session.Schedule.UpdatedByUserId = this.Session.UserProfile.UserId;
        this.Session.Schedule.UserCommonName = this.Session.UserProfile.CommonName;

        // 
        // update the object.
        // 
        EvEventCodes result = this._Bll_Schedules.saveSchedule ( this.Session.Schedule );

        // 
        // get the debug ResultData.
        // 
        this.LogClass ( this._Bll_Schedules.Log );

        // 
        // if an error state is returned create log the event.
        // 
        if ( result != EvEventCodes.Ok )
        {
          string stEvent = "eClinical Project Schedule Error" 
            + this._Bll_Schedules.Log + " returned error message: "
            + Evado.Model.Digital.EvcStatics.getEventMessage ( result );

          this.LogError ( EvEventCodes.Database_Record_Update_Error, stEvent );

          this.ErrorMessage = EvLabels.Schedule_Update_Error_Message;

          this.LogMethodEnd ( "updateObject" );
          return this.Session.LastPage;
        }

        this.LogMethodEnd ( "updateObject" );
        //
        // Update the form object values.
        //
        return new Model.UniForm.AppData ( );

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Schedule_Update_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }
      return this.Session.LastPage;

    }//END updateObject method

    // ==================================================================================
    /// <summary>
    /// THis method updates the EvSchedule object member with groupCommand parameter values.
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.PageCommand object.</param>
    //  ----------------------------------------------------------------------------------
    private void updateObjectValue ( Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "updateObjectValue" );
      this.LogValue ( "Parameters.Count: " + PageCommand.Parameters.Count );
      this.LogValue ( "Customer.Guid: " + this.Session.Application.Guid );

      // 
      // Iterate through the parameter values updating the ResultData object
      // 
      foreach ( Evado.Model.UniForm.Parameter parameter in PageCommand.Parameters )
      {
        this.LogValue ( " " + parameter.Name + " > " + parameter.Value );

        if ( parameter.Name.Contains ( Evado.Model.Digital.EvcStatics.CONST_GUID_IDENTIFIER ) == false
          && parameter.Name != Evado.Model.UniForm.CommandParameters.Custom_Method.ToString ( )
          && parameter.Name != EvSchedule.ScheduleClassFieldNames.ArmIndex.ToString ( )
          && parameter.Name != Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION
          && parameter.Name != EuSchedules.CONST_MILESTONE_TABLE_FIELD_ID )
        {
          this.LogValue ( " >> UPDATED" );
          try
          {
            EvSchedule.ScheduleClassFieldNames fieldName =
               Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue<EvSchedule.ScheduleClassFieldNames> ( parameter.Name );

            this.Session.Schedule.setValue ( fieldName, parameter.Value );
          }
          catch ( Exception Ex )
          {
            this.LogException ( Ex );
          }
        }

      }// End iteration loop

    }//END method.

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
  }
}//END namespace