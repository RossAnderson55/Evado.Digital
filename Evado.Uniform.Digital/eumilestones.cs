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
using Evado.Model.Digital;
// using Evado.Web;

namespace Evado.UniForm.Clinical
{
  /// <summary>
  /// This class manages the integration of Evado eclinical alerts with 
  /// Evado.UniFORM interface.
  /// </summary>
  public class EuMilestones : EuClassAdapterBase
  {
    #region Class Initialisation
    /// <summary>
    /// This method initialises the class.
    /// </summary>
    public EuMilestones ( )
    {
      this.ClassNameSpace = "Evado.UniForm.Clinical.EuMilestones.";
    }

    /// <summary>
    /// This method initialises the class and passs in the user profile.
    /// </summary>
    public EuMilestones (
      EuApplicationObjects ApplicationObjects,
      EvUserProfileBase ServiceUserProfile,
      EuSession SessionObjects,
      String UniFormBinaryFilePath,
      EvClassParameters Settings )
    {
      this.LogInitMethod ( "Milestones initialisation" );

      //
      // Initialise the class's global objects and variables.
      //
      this.ClassNameSpace = "Evado.UniForm.Clinical.EuMilestones.";
      this.ApplicationObjects = ApplicationObjects;
      this.ServiceUserProfile = ServiceUserProfile;
      this.Session = SessionObjects;
      this.UniForm_BinaryFilePath = UniFormBinaryFilePath;
      this.ClassParameters = Settings;

      this.LogInit ( "-ServiceUserProfile.UserId: " + ServiceUserProfile.UserId );
      this.LogInit ( "-SessionObjects.Project.ProjectId: " + this.Session.Application.ApplicationId );
      this.LogInit ( "-SessionObjects.UserProfile.Userid: " + this.Session.UserProfile.UserId );
      this.LogInit ( "-SessionObjects.UserProfile.CommonName: " + this.Session.UserProfile.CommonName );
      this.LogInit ( "-UniForm_BinaryFilePath: " + this.UniForm_BinaryFilePath );

      //
      // Initialise the milestones with the setting initialised.
      //
      this._Bll_Milestones = new Evado.Bll.Clinical.EvMilestones ( Settings );

      this.LogInit ( "Settings:" );
      this.LogInit ( "-LoggingLevel: " + Settings.LoggingLevel );
      this.LogInit ( "-UserId: " + Settings.UserProfile.UserId );
      this.LogInit ( "-UserCommonName: " + Settings.UserProfile.CommonName );

    }//END Method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class constants and variables.

    private Evado.Bll.Clinical.EvMilestones _Bll_Milestones = new Evado.Bll.Clinical.EvMilestones ( );

    private const string CONST_MANDATORY_ACTIVITY_FIELD_ID = "MAF";

    private const string CONST_OPTIONAL_ACTIVITY_FIELD_ID = "OAF";

    private const string CONST_NONCLINICAL_ACTIVITY_FIELD_ID = "NCAF";

    public const string CONST_MILESTONE_ORDER = "MRDR";


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
      this.LogMethod ( "getClientDataObject" );
      this.LogValue ( "PageCommand " + PageCommand.getAsString ( false, false ) );

      try
      {
        // 
        // Initialise the methods variables and objects.
        // 
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );

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

    }//END getTrialMilestoneObject method    

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class list  methods

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
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "getListObject",
          this.Session.UserProfile );

        clientDataObject.Title = EvLabels.Milestone_View_Page_Title;
        clientDataObject.Page.Title = clientDataObject.Title;
        clientDataObject.Id = Guid.NewGuid ( );

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
        this.ErrorMessage = EvLabels.Milestone_List_Error_Message;

        this.LogException ( Ex );
      }

      return this.Session.LastPage;

    }//END getListObject method.

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
        Evado.Model.UniForm.Group listGroup = Page.AddGroup (
          EvLabels.Milestone_List_Group_Title,
          String.Empty,
          Evado.Model.UniForm.EditAccess.Inherited_Access );
        listGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
        listGroup.CmdLayout = Evado.Model.UniForm.GroupCommandListLayouts.Vertical_Orientation;

        // 
        // Query and database.
        // 
        this.Session.MilestoneList = this._Bll_Milestones.getMilestoneList (
          this.Session.Schedule.Guid,
          EvMilestone.MilestoneTypes.Null );

        this.LogValue ( this._Bll_Milestones.DebugLog );

        // 
        // bind output to the datagrid.
        // 
        if ( this.Session.MilestoneList.Count == 0 )  // If nothing returned create a blank row.
        {
          this.Session.MilestoneList.Add ( new EvMilestone ( ) );
        }

        //
        // add a row for each form in the list.
        //
        foreach ( EvMilestone milestone in this.Session.MilestoneList )
        {
          groupCommand = listGroup.addCommand ( milestone.LinkText,
            EuAdapter.APPLICATION_ID,
            EuAdapterClasses.Milestones.ToString ( ),
            Model.UniForm.ApplicationMethods.Get_Object );

          groupCommand.SetGuid ( milestone.Guid );

        }//END milestone iteration loop

        this.LogValue ( "command object count: " + listGroup.CommandList.Count );

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Milestone_List_Error_Message;

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
      Guid milestoneGuid = Guid.Empty;
      EvActivities bllActivities = new EvActivities ( );

      // 
      // If the user does not have monitor or ResultData manager roles exit the page.
      // 
      if ( this.Session.UserProfile.hasManagementAccess == false )
      {
        this.LogIllegalAccess (
         this.ClassNameSpace + "getObject",
          this.Session.UserProfile );

        this.ErrorMessage = EvLabels.Record_Access_Error_Message;

        return this.Session.LastPage; ;
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
        // get the milestone object and activity list.
        //
        this.loadActivityListandMilestone ( PageCommand );

        // 
        // return the client ResultData object for the customer.
        // 
        this.getDataObject ( clientDataObject );

        return clientDataObject;
      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Milestone_Page_Error_Mesage;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return this.Session.LastPage; ;

    }//END getObject method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">List of paremeters to retrieve the selected object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private bool loadActivityListandMilestone (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getMilestoneObject" );
      // 
      // Initialise the methods variables and objects.
      // 
      Guid milestoneGuid = Guid.Empty;
      EvActivities bllActivities = new EvActivities ( );

      // 
      // get the list of activity milestones if it is empty.
      //
      if ( this.Session.ActivityList.Count == 0 )
      {
        this.Session.ActivityList = bllActivities.getActivityList (
          this.Session.Application.ApplicationId,
          EvActivity.ActivityTypes.Null,
          false );

        this.LogClass ( bllActivities.Log );
      }

      // 
      // if the parameter value exists then set the customerId
      // 
      milestoneGuid = PageCommand.GetGuid ( );
      this.LogValue ( "Milestone Guid: " + milestoneGuid );

      // 
      // return if not trial id
      // 
      if ( milestoneGuid == Guid.Empty )
      {
        this.LogMethodEnd ( "getMilestoneObject" );
        return false;
      }

      //
      // if the milestone is loaded then do not retrieve it.
      //
      if ( this.Session.Milestone.Guid == milestoneGuid )
      {
        this.LogMethodEnd ( "getMilestoneObject" );
        return true;
      }

      // 
      // Retrieve the milestone object if the current object is different.
      // 
      this.Session.Milestone = this._Bll_Milestones.getMilestone ( milestoneGuid );

      this.LogClass ( this._Bll_Milestones.DebugLog );

      this.LogValue ( "SessionObjects.Milestone.MilestoneId: "
        + this.Session.Milestone.MilestoneId );

      this.LogMethodEnd ( "getMilestoneObject" );

      return true;

    }//ENd getMilestoneObject method

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
      this.LogValue ( "Schedule.Version: " + this.Session.Schedule.Version );
      this.LogValue ( "InitialScheduleVersion: " + this.Session.Milestone.Data.InitialScheduleVersion );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );
      Evado.Model.UniForm.Field pageField = new Evado.Model.UniForm.Field ( );

      //
      // Initialise the client ResultData object.
      //
      ClientDataObject.Id = this.Session.Milestone.Guid;
      ClientDataObject.Title = EvLabels.Milestone_Page_Title
        + this.Session.Milestone.MilestoneId
        + EvLabels.Space_Hypen
        + this.Session.Milestone.Title;

      ClientDataObject.Page.Id = ClientDataObject.Id;
      ClientDataObject.Page.Title = ClientDataObject.Title;
      ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Disabled;

      //
      // If the user has edit access enable the page for the user.
      //
      if ( this.Session.UserProfile.hasManagementEditAccess == true
        && ( this.Session.Schedule.State == EvSchedule.ScheduleStates.Draft
          || this.Session.Schedule.State == EvSchedule.ScheduleStates.Reviewed ) )
      {
        ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
      }

      this.LogValue ( "ClientDataObject.Page.Status: " + ClientDataObject.Page.EditAccess );

      //
      // define the page commands.
      //
      this.getPageCommands ( ClientDataObject.Page );

      //
      // Create the general pageMenuGroup page.
      //
      this.getClientData_GeneralGroup ( ClientDataObject.Page );

      //
      // Create the milestone sheduling pageMenuGroup
      //
      this.getClientData_SchedulingGroup ( ClientDataObject.Page );

      //
      // Create the Activity pageMenuGroup page.
      //
      this.getClientData_ActvityGroup ( ClientDataObject.Page );

      this.LogMethodEnd ( "getDataObject" );

    }//END getClientDataObject Method

    // ==============================================================================
    /// <summary>
    /// This method add the group commands
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void getPageCommands ( Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getGroupCommands" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );

      //
      // If edit is disabled exit method.
      //
      if ( PageObject.EditAccess == Evado.Model.UniForm.EditAccess.Disabled )
      {
        this.LogMethodEnd ( "getGroupCommands" );
        return;
      }

      this.LogValue ( "User has edit access" );
      // 
      // Add the save groupCommand
      // 
      pageCommand = PageObject.addCommand (
        EvLabels.Milestone_Save_Command_Title,
        EuAdapter.APPLICATION_ID,
        EuAdapterClasses.Milestones.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Save_Object );

      pageCommand.AddParameter (
         Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
        EvMilestone.MilestoneSaveActions.Save.ToString ( ) );

      pageCommand.SetGuid ( this.Session.Milestone.Guid );

      if ( this.Session.Schedule.Version == this.Session.Milestone.Data.InitialScheduleVersion
        && this.Session.Milestone.Title != String.Empty )
      {
        // 
        // Add the delete groupCommand
        // 
        pageCommand = PageObject.addCommand (
          EvLabels.Milestone_Delete_Command_Title,
          EuAdapter.APPLICATION_ID,
          EuAdapterClasses.Milestones.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Save_Object );

        pageCommand.AddParameter (
           Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
          EvMilestone.MilestoneSaveActions.Delete.ToString ( ) );

        pageCommand.SetGuid ( this.Session.Milestone.Guid );

      }//END deletable milestone.

      this.LogMethodEnd ( "getGroupCommands" );

    }//ENDgetGroupCommands method

    // ==============================================================================
    /// <summary>
    /// This method add the group commands
    /// </summary>
    /// <param name="PageGroup">Evado.Model.UniForm.Group object.</param>
    //  ------------------------------------------------------------------------------
    private void getGroupCommands ( Evado.Model.UniForm.Group PageGroup )
    {
      this.LogMethod ( "getGroupCommands" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );

      //
      // If edit is disabled exit method.
      //
      if ( PageGroup.EditAccess == Evado.Model.UniForm.EditAccess.Disabled )
      {
        this.LogMethodEnd ( "getGroupCommands" );
        return;
      }

        this.LogValue ( "User has edit access" );
        // 
        // Add the save groupCommand
        // 
        pageCommand = PageGroup.addCommand (
          EvLabels.Milestone_Save_Command_Title,
          EuAdapter.APPLICATION_ID,
          EuAdapterClasses.Milestones.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Save_Object );

        pageCommand.AddParameter (
           Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
          EvMilestone.MilestoneSaveActions.Save.ToString ( ) );

        pageCommand.SetGuid ( this.Session.Milestone.Guid );

        if ( this.Session.Schedule.Version == this.Session.Milestone.Data.InitialScheduleVersion
          && this.Session.Milestone.Title != String.Empty )
        {
          // 
          // Add the delete groupCommand
          // 
          pageCommand = PageGroup.addCommand (
            EvLabels.Milestone_Delete_Command_Title,
            EuAdapter.APPLICATION_ID,
            EuAdapterClasses.Milestones.ToString ( ),
            Evado.Model.UniForm.ApplicationMethods.Save_Object );

          pageCommand.AddParameter (
             Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
            EvMilestone.MilestoneSaveActions.Delete.ToString ( ) );

          pageCommand.SetGuid ( this.Session.Milestone.Guid );

      }//END deletable milestone.

      this.LogMethodEnd ( "getGroupCommands" );

    }//ENDgetGroupCommands method

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
      bool isInitialVersion = false;

      if ( this.Session.Schedule.Version ==
        this.Session.Milestone.Data.InitialScheduleVersion )
      {
        isInitialVersion = true;
      }
      this.LogValue ( "isInitialVersion: " + isInitialVersion );

      // 
      // create the general page pageMenuGroup
      // 
      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
        EvLabels.Milestone_Page_Group_Title,
        String.Empty,
        Evado.Model.UniForm.EditAccess.Inherited_Access );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;

      pageGroup.EditAccess = PageObject.EditAccess;

      //
      // Add the group commands.
      //
      this.getGroupCommands ( pageGroup );

      // 
      // Create the trial object
      // 
      groupField = pageGroup.createReadOnlyTextField (
        String.Empty,
        EvLabels.Schedule_Title_Field_Label,
        this.Session.Schedule.ScheduleId + EvLabels.Space_Hypen
        + this.Session.Schedule.Title );
      groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      // 
      // Create the trial object
      // 
      groupField = pageGroup.createReadOnlyTextField (
        String.Empty,
        EvLabels.Label_Project_Id,
        this.Session.Application.ApplicationId
        + EvLabels.Space_Hypen
        + this.Session.Application.Title );
      groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      // 
      // Create the activity id object
      // 
      groupField = pageGroup.createTextField (
        EvMilestone.MilestoneClassFieldNames.MilestoneId.ToString ( ),
        EvLabels.Milestone_Page_MilestoneId_Field_Label,
        this.Session.Milestone.MilestoneId,
        10 );
      groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;
      groupField.Mandatory = true;

      if ( isInitialVersion == false )
      {
        groupField.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
      }
      // 
      // Create the activity title object
      // 
      groupField = pageGroup.createTextField (
        EvMilestone.MilestoneClassFieldNames.Title.ToString ( ),
        EvLabels.Milestone_Page_Title_Field_Label,
        this.Session.Milestone.Title,
        50 );
      groupField.Mandatory = true;
      groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      // 
      // Create the activity desription object
      // 
      groupField = pageGroup.createFreeTextField (
        EvMilestone.MilestoneClassFieldNames.Description.ToString ( ),
        EvLabels.Milestone_Page_Description_Field_Label,
        this.Session.Milestone.Description,
        50,
        4 );
      groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      //
      // Define the milestone order field.
      //
      groupField = pageGroup.createNumericField (
        EvMilestone.MilestoneClassFieldNames.Order.ToString ( ),
        EvLabels.Milestone_Page_Order_Field_Label,
        this.Session.Milestone.Order, 0, 200 );
      groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      // 
      // Create the activity title object
      // 
      if ( this.Session.Milestone.Data.InitialScheduleVersion != this.Session.Schedule.Version )
      {
        if ( this.Session.Milestone.Data.InitialScheduleVersion < 0 )
        {
          this.Session.Milestone.Data.InitialScheduleVersion = 0;
        }

        groupField = pageGroup.createReadOnlyTextField (
          String.Empty,
          EvLabels.Milestone_Initial_Version_Field_Label,
          this.Session.Milestone.Data.InitialScheduleVersion.ToString ( ) );
        groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;
      }

      //
      // File the milestone type selection list.
      //
      List<EvOption> optionList = EvMilestone.getAllMilestoneList (
          this.Session.Application,
          this.Session.Schedule,
          true );

      groupField = pageGroup.createSelectionListField (
        EvMilestone.MilestoneClassFieldNames.Type.ToString ( ),
        EvLabels.Milestone_Page_Type_Field_Label,
        this.Session.Milestone.Type.ToString ( ),
        optionList );

      groupField.Mandatory = true;
      groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      if ( isInitialVersion == false )
      {
        groupField.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
      }

    }//END getPage_GeneralGroup Method

    // ==============================================================================
    /// <summary>
    /// This method generates the header pageMenuGroup for the page.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void getClientData_SchedulingGroup (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getClientData_SchedulingGroup" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      int maxRangeValue = 365;
      String stUnit = String.Empty;
      String stDescription = String.Empty;

      //
      // if the milestone is not a scheduled milestone exit.
      //
      if ( this.Session.Milestone.Type == EvMilestone.MilestoneTypes.Annual
        || this.Session.Milestone.Type == EvMilestone.MilestoneTypes.Manual
        || this.Session.Milestone.Type == EvMilestone.MilestoneTypes.Six_Monthly
        || this.Session.Milestone.Type == EvMilestone.MilestoneTypes.Three_Monthly
        || this.Session.Milestone.Type == EvMilestone.MilestoneTypes.UnScheduled )
      {
        //
        // If the milestone patient consent or patient record there is not inter visit period.
        //
        this.Session.Milestone.MilestoneLaterThanConsentDate = false;
        this.Session.Milestone.MilestoneRange = 0F;
        this.Session.Milestone.InterMilestonePeriod = 0F;

        this.LogMethodEnd ( "getClientData_SchedulingGroup" );
        return;
      }

      //
      // If the milestone is non repeating then set repeat no times to 0.
      //
      if ( this.Session.Milestone.Type != EvMilestone.MilestoneTypes.Repeating_Milestone )
      {
        this.Session.Milestone.RepeatNoTimes = 0;
      }

      // 
      // create the general page pageMenuGroup
      // 
      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
        EvLabels.Milestone_Page_Scheduling_Group_Title,
        String.Empty,
        Evado.Model.UniForm.EditAccess.Inherited_Access );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;

      pageGroup.EditAccess = PageObject.EditAccess;

      //
      // Add the group commands.
      //
      this.getGroupCommands ( pageGroup );

      // 
      // Create the visit later than consent ResultData field.
      // 
      groupField = pageGroup.createBooleanField (
        EvMilestone.MilestoneClassFieldNames.Consent_Validation.ToString ( ),
        EvLabels.Milestone_Page_Consent_Validation_Field_Label,
        this.Session.Milestone.MilestoneLaterThanConsentDate );
      groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      // 
      // Create inter visit period field.
      // 
      groupField = pageGroup.createNumericField (
        EvMilestone.MilestoneClassFieldNames.Inter_Visit_Period.ToString ( ),
        EvLabels.Milestone_Page_Period_Validation_Field_Label,
        this.Session.Milestone.InterMilestonePeriod,
        0, 1095 );

      //
      // Set the scheduling period based on the schedule period incrment.
      //
      int minPeriodValue = 0;
      int maxPeriodValue = 10 * 52 ;

      switch ( this.Session.Schedule.MilestonePeriodIncrement )
      {
        case EvSchedule.MilestonePeriodIncrements.Weeks:
          {
            minPeriodValue = 0;
            maxPeriodValue = 1040;
            stUnit = EvLabels.Milestone_Period_Increment_Weeks.ToString ( );
            stDescription = String.Format ( EvLabels.Milestone_Period_Description_Text, minPeriodValue, maxPeriodValue, stUnit )
              + "\r\n" + EvLabels.Milestone_Period_Description_Text1;

            break;
          }
        case EvSchedule.MilestonePeriodIncrements.Months:
          {
            minPeriodValue = 0;
            maxPeriodValue = 10 * 12;
            stUnit = EvLabels.Milestone_Period_Increment_Months.ToString ( );
            stDescription = String.Format ( EvLabels.Milestone_Period_Description_Text, minPeriodValue, maxPeriodValue, stUnit )
            + "\r\n" + EvLabels.Milestone_Period_Description_Text1;
            break;
          }
        default:
          { // days upto 3 years

            minPeriodValue = 0;
            maxRangeValue = 365 * 3;
            stUnit = EvLabels.Milestone_Period_Increment_Days.ToString ( );
            stDescription = String.Format ( EvLabels.Milestone_Range_Description_Text, maxRangeValue, stUnit );
            break;
          }
      }
      groupField.AddParameter ( Model.UniForm.FieldParameterList.Min_Value, minPeriodValue );
      groupField.AddParameter ( Model.UniForm.FieldParameterList.Max_Value, maxPeriodValue );
      groupField.AddParameter ( Model.UniForm.FieldParameterList.Min_Alert, minPeriodValue );
      groupField.AddParameter ( Model.UniForm.FieldParameterList.Max_Alert, maxPeriodValue );
      groupField.Description =  stDescription ;
      groupField.AddParameter ( Model.UniForm.FieldParameterList.Unit, stUnit );
      groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      // 
      // Create visit date range field.
      // 
      groupField = pageGroup.createNumericField (
        EvMilestone.MilestoneClassFieldNames.Milestone_Range.ToString ( ),
        EvLabels.MIlestone_Page_Range_In_Days_Field_Label,
        this.Session.Milestone.MilestoneRange,
        0, maxRangeValue );
      groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      groupField.Description =  stDescription ;
      groupField.AddParameter ( Model.UniForm.FieldParameterList.Unit, stUnit );


      // 
      // Create field to enable automatic scheduling.
      // 
      groupField = pageGroup.createBooleanField (
        EvMilestone.MilestoneClassFieldNames.Enable_Automatic_Scheduling.ToString ( ),
        EvLabels.Milestone_Page_Automatic_Scheduling_Field_Label,
        EvLabels.Milestone_Page_Automatic_Scheduling_Description,
        this.Session.Milestone.EnableAutomaticScheduling );
      groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      // 
      // Create no of repeat visits if a repeating milestone.
      // 0 means the milestone is not repeated 
      // a number greater than 0 indicates the number of times the milestone is to be repeated
      // a value of 99 indicates that the milestone will be repeated until the non-repeating milestone's start date is reached.
      // 
      if ( this.Session.Milestone.Type == EvMilestone.MilestoneTypes.Repeating_Milestone )
      {
        groupField = pageGroup.createNumericField (
          EvMilestone.MilestoneClassFieldNames.Repeat_No_Times.ToString ( ),
          EvLabels.Milestone_Repeat_No_Times_Field_Label,
          EvLabels.Milestone_Repeat_No_Times_Field_Description,
          this.Session.Milestone.RepeatNoTimes,
          0, 99 );
        groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;
      }

      this.LogMethodEnd ( "getClientData_SchedulingGroup" );

    }//END getClientData_SchedulingGroup Method

    // ==============================================================================
    /// <summary>
    /// This method generates the header pageMenuGroup for the page.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void getClientData_ActvityGroup (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getClientData_ActvityGroup" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      bool isInitialVersion = false;

      if ( this.Session.Schedule.Version ==
        this.Session.Milestone.Data.InitialScheduleVersion )
      {
        isInitialVersion = true;
      }
      this.LogValue ( "isInitialVersion: " + isInitialVersion );

      // 
      // create the general page pageMenuGroup
      // 
      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
        EvLabels.Milestone_Page_Activity_Group_Title,
        String.Empty,
        Evado.Model.UniForm.EditAccess.Inherited_Access );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;

      pageGroup.EditAccess = PageObject.EditAccess;
      //
      // Add the group commands.
      //
      this.getGroupCommands ( pageGroup );

      // 
      // Create the mandatory activity selection list
      // 
      List<EvOption> selectinList = this.getActivitySelectionList ( true, true );
      string stActivityId = this.getClinicalActivity ( true ).ActivityId;

      groupField = pageGroup.createSelectionListField (
        EuMilestones.CONST_MANDATORY_ACTIVITY_FIELD_ID,
        EvLabels.Milestone_Page_Mandatory_Activity_Field_Label,
        stActivityId,
        selectinList );

      if ( isInitialVersion == false
        && groupField.Value != String.Empty )
      {
        groupField.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
      }

      groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      // 
      // Create the optional activity selection list if the milestone is a clinical or implant visit. 
      // 
      if ( this.Session.Milestone.Type == EvMilestone.MilestoneTypes.Clinical
        || this.Session.Milestone.Type == EvMilestone.MilestoneTypes.Implant_Visit
        || this.Session.Milestone.Type == EvMilestone.MilestoneTypes.Monitored
        || this.Session.Milestone.Type == EvMilestone.MilestoneTypes.UnScheduled )
      {
        stActivityId = this.getClinicalActivity ( false ).ActivityId;

        groupField = pageGroup.createSelectionListField (
          EuMilestones.CONST_OPTIONAL_ACTIVITY_FIELD_ID,
          EvLabels.Milestone_Page_Optional_Activity_Field_Label,
          stActivityId,
          selectinList );

        if ( isInitialVersion == false
          && groupField.Value != String.Empty )
        {
          groupField.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
        }

        groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;
      }

      // 
      // Create the non-clinical activity selection list
      // 
        stActivityId = this.getNonClinicalActivityIds ( );

        groupField = pageGroup.createCheckBoxListField (
          EuMilestones.CONST_NONCLINICAL_ACTIVITY_FIELD_ID,
          EvLabels.Milestone_Page_Ctms_Activity_Field_Label,
          stActivityId,
          this.getActivitySelectionList ( false, false ) );
        groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;


    }//END getPage_GeneralGroup Method

    // ==================================================================================
    /// <summary>
    /// THis method creates an activity selection list.
    /// </summary>
    /// <param name="IsMandatory">True: selection clinical activities.</param>
    /// <returns>List of EvOption objection</returns>
    //  ----------------------------------------------------------------------------------
    private EvActivity getClinicalActivity (
      bool IsMandatory )
    {
      this.LogMethod ( "getClinicalActivity" );
      this.LogValue ( "IsMandatory: " + IsMandatory );
      //
      // Iterate through the list of milestone activities looking for matching activities.
      //
      foreach ( EvActivity activity in this.Session.Milestone.ActivityList )
      {
        //
        // skip all non clinial activities.
        //
        if ( activity.IsClinical == false )
        {
          this.LogValue ( "Not Clinical Activity." );
          continue;
        }

        this.LogValue ( "Clinical activity: " + activity.ActivityId );
        //
        // Select activity that matches the IsMandatory criteria.
        //
        if ( IsMandatory == true )
        {
          if ( activity.IsMandatory == true )
          {
            this.LogValue ( "ActivityId: " + activity.ActivityId
              + " IsMandatory: " + activity.IsMandatory );

            return activity;
          }
        }
        else
        {
          if ( activity.IsMandatory == false )
          {
            this.LogValue ( "ActivityId: " + activity.ActivityId
              + " IsMandatory: " + activity.IsMandatory );

            return activity;
          }
        }

      }//END iteration loop

      return new EvActivity ( );

    }//END getClinicalActivity method

    // ==================================================================================
    /// <summary>
    /// THis method creates an activity selection list.
    /// </summary>
    /// <returns>List of EvOption objection</returns>
    //  ----------------------------------------------------------------------------------
    private String getNonClinicalActivityIds ( )
    {
      this.LogMethod ( "getNonClinicalActivityIds" );
      //
      // Initialise the methods variables and object.
      //
      String stActivityIds = String.Empty;

      //
      // Iterate through the list of milestone activities looking for matching activities.
      //
      foreach ( EvActivity activity in this.Session.Milestone.ActivityList )
      {
        //
        // Skip all clinical activities.
        //
        if ( activity.IsClinical == true )
        {
          continue;
        }

        //
        // Add the activity if the activityId is not empty.
        //
        if ( stActivityIds != String.Empty )
        {
          stActivityIds += ";";
        }
        stActivityIds += activity.ActivityId;

      }//END iteration loop

      this.LogValue ( "Non clinical Activities are=: " + stActivityIds );

      return stActivityIds;

    }//END getClinicalActivity method

    // ==================================================================================
    /// <summary>
    /// THis method creates an activity selection list.
    /// </summary>
    /// <param name="IsClinical">True: select clinical activities.</param>
    /// <param name="IsSelectionList">True: selection list.</param>
    /// <returns>List of EvOption objects</returns>
    //  ----------------------------------------------------------------------------------
    private List<EvOption> getActivitySelectionList (
      bool IsClinical,
      bool IsSelectionList )
    {
      this.LogMethod ( "getActivitySelectionList" );
      this.LogValue ( "IsClinical: " + IsClinical );
      //
      // Initialise the methods variables and objects.
      //
      List<EvOption> optionList = new List<EvOption> ( );

      if ( IsSelectionList == true )
      {
        optionList.Add ( new EvOption ( ) );
      }

      //
      // IF the activity is clinical and IsClinical is set add the activity as an 
      // option.
      //
      if ( IsClinical == true )
      {
        //
        // Iterate through the list of activities looking for matching activities.
        //
        foreach ( EvActivity activity in this.Session.ActivityList )
        {
          this.LogValue ( "ActivityId: " + activity.ActivityId + " type: " + activity.Type );
          //
          // IF the activity is clinical and IsClinical is set add the activity as an 
          // option.
          //
          if ( activity.IsClinical == true )
          {
            this.LogValue ( " is clinical" );
            optionList.Add ( new EvOption ( activity.ActivityId, activity.ActivityId
              + EvLabels.Space_Hypen
              + activity.Title ) );
          }

        }//END iteration loop
      }
      else
      {
        //
        // Iterate through the list of activities looking for matching activities.
        //
        foreach ( EvActivity activity in this.Session.ActivityList )
        {
          this.LogValue ( "ActivityId: " + activity.ActivityId + " type: " + activity.Type );
          //
          // IF the activity is clinical and IsClinical is set add the activity as an 
          // option.
          //
          if ( activity.IsClinical == false )
          {
            this.LogValue ( " is NON clinical" );

            optionList.Add ( new EvOption ( activity.ActivityId, activity.ActivityId
              + EvLabels.Space_Hypen
              + activity.Title ) );
          }

        }//END iteration loop

      }//END non clinical activities.

      //
      // return the list.
      //
      return optionList;

    }//END getActivitySelectionList method

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
    private Evado.Model.UniForm.AppData createObject (
      Evado.Model.UniForm.Command PageCommand )
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
        // create a list of the current activities for selection.
        //
        Evado.Bll.Clinical.EvActivities bllActivities = new EvActivities ( );
        int iOrder = 1;

        // 
        // query the database for a list of activities.
        // 
        if ( this.Session.ActivityList.Count == 0 )
        {
          this.Session.ActivityList = bllActivities.getActivityList (
            this.Session.Application.ApplicationId,
            EvActivity.ActivityTypes.Null,
            false );

          this.LogValue ( bllActivities.Log );
        }

        String value = PageCommand.GetParameter ( EuMilestones.CONST_MILESTONE_ORDER );
        if ( value != String.Empty )
        {
          iOrder = int.Parse ( value );
        }

        // 
        // Initialise the methods variables and objects.
        //      
        Evado.Model.UniForm.AppData data = new Evado.Model.UniForm.AppData ( );
        this.Session.Milestone = new EvMilestone ( );
        this.Session.Milestone.Guid = Evado.Model.Digital.EvcStatics.CONST_NEW_OBJECT_ID;
        this.Session.Milestone.ScheduleGuid = this.Session.Schedule.Guid;
        this.Session.Milestone.ProjectId = this.Session.Application.ApplicationId;
        this.Session.Milestone.Data.InitialScheduleVersion = this.Session.Schedule.Version;
        this.Session.Milestone.Data.OptionalScheduleVersion = this.Session.Schedule.Version;
        this.Session.Milestone.Data.CurrentVersion = this.Session.Schedule.Version;
        this.Session.Milestone.Order = iOrder;

          this.Session.Milestone.Type = EvMilestone.MilestoneTypes.Clinical;
        // 
        // Save the customer object to the session
        // 


        //
        // Display the new milestone.
        //
        this.getDataObject ( data );


        this.LogValue ( "Exit createObject method. ID: " + data.Id + ", Title: " + data.Title );

        return data;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Milestone_Creation_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return this.Session.LastPage; ;

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
      this.LogValue ( "Parameter PageCommand: "
        + PageCommand.getAsString ( false, true ) );
      this.LogValue ( "eClinical.Milestone" );
      this.LogValue ( "Guid: " + this.Session.Milestone.Guid );
      this.LogValue ( "ProjectId: " + this.Session.Milestone.ProjectId );
      this.LogValue ( "Title: " + this.Session.Milestone.Title );
      EvMilestone.MilestoneSaveActions saveAction = EvMilestone.MilestoneSaveActions.Save;
      try
      {

        //
        // If new is true then this is a new activity so set the Guid to empty
        // so the business layer creates new activcity in the database.
        //
        if ( this.Session.Milestone.Guid == Evado.Model.Digital.EvcStatics.CONST_NEW_OBJECT_ID )
        {
          this.Session.Milestone.Guid = Guid.Empty;
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
        // Update the activity object selections.
        //
        this.updateActivityObjects ( PageCommand );

        //
        // Validate the object ResultData to ensure it has all mandatory ResultData.
        //
        if ( this.validateMilestoneObject ( ) == false )
        {
          this.LogDebug ( "Return Null" );
          this.LogMethodEnd ( "updateObject" );
          return this.Session.LastPage;
        }

        //
        // set the correct default milestone types based on the schedule type.
        //
        this.setCorrectDefaultMilestoneType ( );

        // 
        // Get the save action message value.
        // 
        String stSaveAction = PageCommand.GetParameter ( Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION );

        // 
        // Resets the save action to the parameter passed in the page groupCommand.
        // 
        if ( stSaveAction != String.Empty )
        {
          saveAction = Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue<EvMilestone.MilestoneSaveActions> ( stSaveAction );
        }

        this.Session.Milestone.Data.CurrentVersion = this.Session.Schedule.Version;
        this.Session.Milestone.Action = saveAction;
        this.Session.Milestone.UpdatedByUserId = this.Session.UserProfile.UserId;
        this.Session.Milestone.UserCommonName = this.Session.UserProfile.CommonName;

        // 
        // update the object.
        // 
        EvEventCodes result = this._Bll_Milestones.saveItem ( this.Session.Milestone );

        // 
        // get the debug ResultData.
        // 
        this.LogDebug ( this._Bll_Milestones.DebugLog );

        // 
        // if an error state is returned create log the event.
        // 
        if ( result != EvEventCodes.Ok )
        {
          string stEvent = "Schedule  Milestone Error"
            + this._Bll_Milestones.DebugLog + " returned error message: "
            + Evado.Model.Digital.EvcStatics.getEventMessage ( result );

          this.LogError ( EvEventCodes.Database_Record_Update_Error,
            stEvent );

          switch ( result )
          {
            case EvEventCodes.Data_Duplicate_Id_Error:
              {
                this.ErrorMessage = String.Format (
                  EvLabels.Milestone_Duplicate_Identifier_Error_Messge,
                  this.Session.Milestone.MilestoneId );
                break;
              }
            case EvEventCodes.Identifier_Project_Id_Error:
              {
                this.ErrorMessage = EvLabels.Project_Identifier_Empty_Error_Message;
                break;
              }
            case EvEventCodes.Identifier_Schedule_Identifier_Error:
              {
                this.ErrorMessage = EvLabels.Schedule_Identifier_Empty_Error_Message;
                break;
              }
            case EvEventCodes.Identifier_Milestone_Id_Error:
              {
                this.ErrorMessage = EvLabels.Milestone_Identifier_Empty_Error_Message;
                break;
              }
            default:
              {
                this.ErrorMessage = EvLabels.Milestone_Update_Error_Message;
                break;
              }
          }

          this.LogMethodEnd ( "updateObject" );
          return this.Session.LastPage;
        }

        this.LogMethodEnd ( "updateObject" );
        return new Model.UniForm.AppData ( );
      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Milestone_Update_Error_Message;

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
    /// THis method updates the EvMilestone object member with groupCommand parameter values.
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.PageCommand object.</param>
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
        if ( parameter.Name.Contains ( Evado.Model.Digital.EvcStatics.CONST_GUID_IDENTIFIER ) == false
          && parameter.Name != Evado.Model.UniForm.CommandParameters.Custom_Method.ToString ( )
          && parameter.Name != Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION
          && parameter.Name != EuMilestones.CONST_MANDATORY_ACTIVITY_FIELD_ID
          && parameter.Name != EuMilestones.CONST_OPTIONAL_ACTIVITY_FIELD_ID
          && parameter.Name != EuMilestones.CONST_NONCLINICAL_ACTIVITY_FIELD_ID )
        {
          this.LogDebug ( " " + parameter.Name + " > " + parameter.Value + " >> UPDATED" );
          try
          {
            EvMilestone.MilestoneClassFieldNames fieldName = Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue<EvMilestone.MilestoneClassFieldNames> ( parameter.Name );

            this.Session.Milestone.setValue ( fieldName, parameter.Value );
          }
          catch ( Exception Ex )
          {
            this.LogException ( Ex );
          }
        }
        else
        {
          this.LogDebug ( " " + parameter.Name + " > " + parameter.Value + " >> SKIPPED" );
        }
      }// End iteration loop

    }//END method.

    // ==================================================================================
    /// <summary>
    /// THis method updates the EvMilestone activities values..
    /// </summary>
    //  ----------------------------------------------------------------------------------
    private bool validateMilestoneObject ( )
    {
      this.LogMethod ( "validateMilestoneObject" );
      //
      // Initialise the methods variables and objects.
      //
      bool validationResult = true;
      this.ErrorMessage = String.Empty;

      //
      // Validate that the project identifier exists.
      //
      if ( this.Session.Milestone.ProjectId == String.Empty )
      {
        this.ErrorMessage += EvLabels.Milestone_Validation_ProjectId_Missing;
        validationResult = false;
      }


      //
      // Validate that the milestone identifier exists.
      //
      if ( this.Session.Milestone.MilestoneId == String.Empty )
      {
        if ( this.ErrorMessage != String.Empty )
        {
          this.ErrorMessage += "\r\n";
        }
        this.ErrorMessage += EvLabels.Milestone_Validation_MilestoneId_Missing;
        validationResult = false;
      }

      this.LogMethodEnd ( "validateMilestoneObject" );
      return validationResult;

    }//END validateMilestoneObject method

    // ==================================================================================
    /// <summary>
    /// THis method updates the EvMilestone activities values..
    /// </summary>
    //  ----------------------------------------------------------------------------------
    private void setCorrectDefaultMilestoneType ( )
    {
      this.LogMethod ( "setCorrectDefaultMilestoneType" );
      //
      // Initialise the methods variables and objects.
      //
      switch ( this.Session.Schedule.Type )
      {
        case EvSchedule.ScheduleTypes.Non_Clinical:
          {
            if ( this.Session.Milestone.Type == EvMilestone.MilestoneTypes.Null )
            {
              this.Session.Milestone.Type = EvMilestone.MilestoneTypes.Non_Clinical;
            }
            break;
          }
        case EvSchedule.ScheduleTypes.PRO_Schedule:
          {
            if ( this.Session.Milestone.Type == EvMilestone.MilestoneTypes.Null )
            {
              this.Session.Milestone.Type = EvMilestone.MilestoneTypes.Patient_Record;
            }
            break;
          }

        default:
          {
            if ( this.Session.Milestone.Type == EvMilestone.MilestoneTypes.Null )
            {
              this.Session.Milestone.Type = EvMilestone.MilestoneTypes.Clinical;
            }
            break;
          }
      }//END schedule type switch
      this.LogMethodEnd ( "validateMilestoneType" );

    }//END validateMilestoneObject method

    // ==================================================================================
    /// <summary>
    /// THis method updates the EvMilestone activities values..
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.PageCommand object.</param>
    //  ----------------------------------------------------------------------------------
    private void updateActivityObjects (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "updateActivityObjects" );

      //
      // Initialise the methods variables and objects.
      //
      EvActivity activity = new EvActivity ( );

      //
      // Delete the existing list and re-created it.
      //
      this.Session.Milestone.ActivityList = new List<EvActivity> ( );

      //
      // Get the mandatory, optional and non-clinical activity Ids.
      //
      String stMandatoryActivityId = PageCommand.GetParameter ( EuMilestones.CONST_MANDATORY_ACTIVITY_FIELD_ID );
      String stOptionalActivityId = PageCommand.GetParameter ( EuMilestones.CONST_OPTIONAL_ACTIVITY_FIELD_ID );
      String stNonClinicalActivities = PageCommand.GetParameter ( EuMilestones.CONST_NONCLINICAL_ACTIVITY_FIELD_ID );

      this.LogValue ( "stMandatoryActivityId: " + stMandatoryActivityId );
      this.LogValue ( "stOptionalActivityId: " + stOptionalActivityId );
      this.LogValue ( "stNonClinicalActivities: " + stNonClinicalActivities );

      //
      // Update the mandatory clinical activity.
      //
      this.updateActivityObject (
        stMandatoryActivityId,
        true,
        true );

      //
      // Update the optional clinical activity.
      //
      this.updateActivityObject (
        stOptionalActivityId,
        true,
        false );

        String [ ] arrActivityId = stNonClinicalActivities.Split ( ';' );

        //
        // Iterate through the selected non-clinical activity Id.
        //
        foreach ( String activitId in arrActivityId )
        {
          //
          // Update the activity.
          //
          this.updateActivityObject (
            activitId,
            false,
            false );
        }


    }//END updateActivityObjects method.

    //  ==================================================================================	
    /// <summary>
    /// Updates the database with row changes.
    /// </summary>
    /// <param name="ActivityId">String: Activity identifier.</param>
    /// <param name="IsClinical">Bool: is a clinical activity.</param>
    /// <param name="IsMandatory">Bool: is a mandatory activity.</param>
    /// <returns>Bool: true completed successfully</returns>
    //  ---------------------------------------------------------------------------------
    private void updateActivityObject (
      String ActivityId,
      bool IsClinical,
      bool IsMandatory )
    {
      this.LogMethod ( "updateActivityObject" );
      this.LogValue ( "ActivityId: " + ActivityId );
      this.LogValue ( "IsClinical: " + IsClinical );
      this.LogValue ( "IsMandatory: " + IsMandatory );

      //
      // Only add activities if the activityId has a value.
      //
      if ( ActivityId == String.Empty )
      {
        this.LogValue ( "ActivityId: EMPTY " );
        return;
      }

      //
      // Initialise the methods variables and objects.
      //
      EvActivity milestoneActivity = new EvActivity ( );
      int order = this.Session.Milestone.ActivityList.Count;
      order = ( order + 1 ) * 5;

      //
      // All CTMS activities must optional.
      //
      if ( IsClinical == false )
      {
        IsMandatory = false;
      }

      // 
      // Initialise the activity.
      // 
      milestoneActivity = new EvActivity ( );
      milestoneActivity.ActivityId = ActivityId;
      milestoneActivity.ProjectId = this.Session.Application.ApplicationId;
      milestoneActivity.MilestoneGuid = this.Session.Milestone.Guid;
      milestoneActivity.Order = order;
      milestoneActivity.IsMandatory = false;

      //
      // Set the milestone attributes for mandatory or optional activities.
      //
      if ( IsMandatory == true )
      {
        milestoneActivity.IsMandatory = true;
      }

      this.LogValue ( "New Actvitity Object content:"
        + " G: " + milestoneActivity.MilestoneGuid
        + " A: " + milestoneActivity.ActivityId
        + " M: " + milestoneActivity.IsMandatory
        + " O: " + milestoneActivity.Order );

      //
      // Add the new activity to the milestone activity list.
      //
      this.Session.Milestone.ActivityList.Add ( milestoneActivity );

      return;

    }//END updateActivityObject method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
  }
}//END namespace