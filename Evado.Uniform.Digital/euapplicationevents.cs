/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\ApplicationEvents.cs" company="EVADO HOLDING PTY. LTD.">
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
using  Evado.Model.Digital;
// using Evado.Web;

namespace Evado.UniForm.Digital
{
  /// <summary>
  /// This class defines the application base classs that is used to terminate the 
  /// hosted application objects.
  /// 
  /// This class terminates the Organisation object.
  /// </summary>
  public class EuApplicationEvents : EuClassAdapterBase
  {
    #region Class Initialisation
    /// <summary>
    /// This method initialises the class.
    /// </summary>
    public EuApplicationEvents ( )
    {
    }

    /// <summary>
    /// This method initialises the class and passs in the user profile.
    /// </summary>
    public EuApplicationEvents (
      EuGlobalObjects ApplicationObjects,
      EvUserProfileBase ServiceUserProfile,
      EuSession SessionObjects,
      String UniFormBinaryFilePath,
      EvClassParameters Settings )
    {
      this.ClassNameSpace = "Evado.UniForm.Clinical.EuAPplicationEvents. ";
      this.AdapterObjects = ApplicationObjects;
      this.ServiceUserProfile = ServiceUserProfile;
      this.Session = SessionObjects;
      this.UniForm_BinaryFilePath = UniFormBinaryFilePath;
      this.ClassParameters = Settings;

      this._Bll_ApplicationEvents = new Evado.Bll.EvApplicationEvents ( this.ClassParameters );

      this.LogInitMethod ( "EuApplicationEvents initialisation" );
      this.LogInit ( "ServiceUserProfile.UserId: " + ServiceUserProfile.UserId );
      this.LogInit ( "SessionObjects.UserProfile.Userid: " + this.Session.UserProfile.UserId );
      this.LogInit ( "SessionObjects.UserProfile.CommonName: " + this.Session.UserProfile.CommonName );
      this.LogInit ( "UniFormBinaryFilePath: " + this.UniForm_BinaryFilePath );

      this.LogInit ( "Settings:" );
      this.LogInit ( "-LoggingLevel: " + Settings.LoggingLevel );
      this.LogInit ( "-UserId: " + Settings.UserProfile.UserId );
      this.LogInit ( "-UserCommonName: " + Settings.UserProfile.CommonName );
    }//END Method


    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class constants and variables.

    private const String CONST_EVENT_FIELD_ID = "EV";
    private const String CONST_TYPE_FIELD_ID = "ETYP";
    private const String CONST_USER_FIELD_ID = "EUSR";
    private const String CONST_START_DATE_FIELD_ID = "ESD";
    private const String CONST_FINISH_DATE_FIELD_ID = "EFD";

    private Evado.Bll.EvApplicationEvents _Bll_ApplicationEvents = new Evado.Bll.EvApplicationEvents ( );

    private EvApplicationEvent _ApplicationEvent = new EvApplicationEvent ( );

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
    public Evado.Model.UniForm.AppData getClientDataObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getClientDataObject" );

      this.LogValue ( "PageCommand Content: " + PageCommand.getAsString ( false, false ) );
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
           this.ClassNameSpace + "getClientDataObject",
            this.Session.UserProfile );

          this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

          return this.Session.LastPage;
        }

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
         this.ClassNameSpace + "getClientDataObject",
          this.Session.UserProfile );

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

        }//END Swith

        // 
        // Handle returned exceptions.
        // 
        if ( clientDataObject == null )
        {
          this.LogValue ( " null application data returned." );
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
        this.LogMethodEnd ( "getClientDataObject" );
        return clientDataObject;

      }
      catch ( Exception Ex )
      {
        this.LogException ( Ex );
      }
      this.LogMethodEnd ( "getClientDataObject" );
      return this.Session.LastPage;

    }//END getClientDataObject method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class private methods

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
      try
      {
        this.LogMethod ( "getListObject" );
        // 
        // Initialise the methods variables and objects.
        //      
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
        EvUserProfiles userProfiles = new EvUserProfiles ( );
        String value = String.Empty;

        clientDataObject.Title = EdLabels.ApplicationEvent_List_Page_Title;
        clientDataObject.Page.Title = clientDataObject.Title;
        clientDataObject.Id = Guid.NewGuid ( );

        if ( this.AdapterObjects.HelpUrl != String.Empty )
        {
          Evado.Model.UniForm.Command helpCommand = clientDataObject.Page.addCommand (
           EdLabels.Label_Help_Command_Title,
           EuAdapter.ADAPTER_ID,
           EuAdapterClasses.Events.ToString ( ),
           Model.UniForm.ApplicationMethods.Get_Object );

          helpCommand.Type = Evado.Model.UniForm.CommandTypes.Html_Link;

          helpCommand.AddParameter ( Model.UniForm.CommandParameters.Link_Url,
           EvcStatics.createHelpUrl (
            this.AdapterObjects.HelpUrl,
             Evado.Model.Digital.EvPageIds.Application_Event_View ) );
        }
        if ( this.Session.EventStartDate ==  Evado.Model.Digital.EvcStatics.CONST_DATE_MIN_RANGE )
        {
          this.Session.EventStartDate =
           Evado.Model.Digital.EvcStatics.getDateTime ( DateTime.Now.ToString ( "dd MMM yyyy" ) );
          this.Session.EventFinishDate = this.Session.EventStartDate.AddDays ( 1 );
        }

        //
        // Retrieve the selection options.
        //
        this.Session.EventId = EvEventCodes.Null;

        value = PageCommand.GetParameter ( EuApplicationEvents.CONST_EVENT_FIELD_ID );
        if ( value != String.Empty )
        {
          this.Session.EventId =  Evado.Model.EvStatics.parseEnumValue<EvEventCodes> ( value );
        }

        this.Session.EventType = PageCommand.GetParameter ( EuApplicationEvents.CONST_TYPE_FIELD_ID );

        value = PageCommand.GetParameter ( EuApplicationEvents.CONST_START_DATE_FIELD_ID );
        if ( value != String.Empty )
        {
          this.Session.EventStartDate =  Evado.Model.Digital.EvcStatics.getDateTime ( value );
        }
        value = PageCommand.GetParameter ( EuApplicationEvents.CONST_FINISH_DATE_FIELD_ID );
        if ( value != String.Empty )
        {
          this.Session.EventFinishDate =  Evado.Model.Digital.EvcStatics.getDateTime ( value );
        }

        this.Session.EventUserName = PageCommand.GetParameter ( EuApplicationEvents.CONST_USER_FIELD_ID );

        this.LogValue ( "EventId: " + this.Session.EventId );
        this.LogValue ( "EventType: " + this.Session.EventType );
        this.LogValue ( "EventUserName: " + this.Session.EventUserName );
        this.LogValue ( "EventStartDate: " + this.Session.EventStartDate );
        this.LogValue ( "EventFinishDate: " + this.Session.EventFinishDate );

        // 
        // Create the new pageMenuGroup.
        // 
        this.getSelectionGroup ( clientDataObject.Page );
        // 
        // Add the trial applicationEvent list to the page.
        // 
        this.getListGroup ( clientDataObject.Page );

        this.LogValue ( "data.Title: " + clientDataObject.Title );
        this.LogValue ( "data.Page.Title: " + clientDataObject.Page.Title );


        return clientDataObject;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.ApplicationEvent_List_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      this.LogMethodEnd ( "getListObject" );

      return this.Session.LastPage;

    }//END getListObject method.

    // ==================================================================================
    /// <summary>
    /// This methods returns a pageMenuGroup object contains a selection of applicationEvents.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object</param>
    /// <returns>Evado.Model.UniForm.Group object</returns>
    //  ---------------------------------------------------------------------------------
    public void getSelectionGroup (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getSelectionGroup" );

      this.LogValue ( "EventStartDate: " + this.Session.EventStartDate );
      this.LogValue ( "EventFinishDate: " + this.Session.EventFinishDate );

      // 
      // initialise the methods variables and objects.
      // 
      List<Evado.Model.EvOption> optionList = new List<Evado.Model.EvOption> ( );
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );

      Evado.Model.UniForm.Group selectionGroup = PageObject.AddGroup (
        EdLabels.ApplicationEvent_Selection_Group_Title,
        Evado.Model.UniForm.EditAccess.Enabled );
      selectionGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      //
      // get the list of event ids.
      //
      optionList =  Evado.Model.EvStatics.getOptionsFromEnum ( typeof ( EvEventCodes ), true );

       Evado.Model.Digital.EvcStatics.sortOptionList ( optionList );

      // 
      // Set the selection to the event id
      // 
      groupField = selectionGroup.createSelectionListField (
        EuApplicationEvents.CONST_EVENT_FIELD_ID,
        EdLabels.ApplicationEvent_Event_Id_Selection_Field_Label,
        this.Session.EventId.ToString ( ),
        optionList );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      groupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );


      //
      // get the list of event ids.
      //
      optionList =  Evado.Model.EvStatics.getOptionsFromEnum ( typeof ( EvApplicationEvent.EventType ), true );
      // 
      // Set the selection to the type id
      // 
      groupField = selectionGroup.createSelectionListField (
        EuApplicationEvents.CONST_TYPE_FIELD_ID,
        EdLabels.ApplicationEvent_Type_Selection_Field_Label,
        this.Session.EventType,
        optionList );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      groupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );

      // 
      // Set the selection to the USER Name
      // 
      groupField = selectionGroup.createSelectionListField (
        EuApplicationEvents.CONST_USER_FIELD_ID,
        EdLabels.ApplicationEvent_UserName_Selection_Field_Label,
        this.Session.EventUserName,
        this.Session.EventUserSelectionList );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      groupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );

      // 
      // Set the selection to the start date
      // 
      groupField = selectionGroup.createDateField (
        EuApplicationEvents.CONST_START_DATE_FIELD_ID,
        EdLabels.ApplicationEvent_Start_Date_Selection_Field_Label,
        this.Session.EventStartDate ,
        EvStatics.getDateTime( "1 jan 2010"),
        DateTime.Now.AddYears( 1 ) );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      groupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );

      // 
      // Set the selection to the start date
      // 
      groupField = selectionGroup.createDateField (
        EuApplicationEvents.CONST_FINISH_DATE_FIELD_ID,
        EdLabels.ApplicationEvent_Finish_Date_Selection_Field_Label,
        this.Session.EventFinishDate,
        EvStatics.getDateTime( "1 jan 2010"),
        DateTime.Now.AddYears( 1 ) );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      groupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );


      // 
      // Create a custom groupCommand to process the selection.
      // 
      Evado.Model.UniForm.Command customCommand = selectionGroup.addCommand (
        EdLabels.ApplicationEvent_Selection_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Events.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Custom_Method );

      // 
      // Set the custom groupCommand parameter.
      // 
      customCommand.setCustomMethod ( Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

    }//END getSelectionGroup method

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
        Evado.Model.UniForm.Group listGroup = PageObject.AddGroup (
          EdLabels.ApplicationEvent_List_Group_Title,
          Evado.Model.UniForm.EditAccess.Inherited );
        listGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
        listGroup.CmdLayout = Evado.Model.UniForm.GroupCommandListLayouts.Vertical_Orientation;

        // 
        // get the list of customers.
        // 
        List<EvApplicationEvent> applicationEventList = this._Bll_ApplicationEvents.getEventList (
          ( int ) this.Session.EventId,
          this.Session.EventType,
          String.Empty,
          this.Session.EventStartDate,
          this.Session.EventFinishDate,
          this.Session.EventUserName );

        this.LogClass ( this._Bll_ApplicationEvents.Log );
        this.LogValue ( "list count: " + applicationEventList.Count );
        // 
        // generate the page links.
        // 
        foreach ( EvApplicationEvent applicationEvent in applicationEventList )
        {
          // 
          // Add the trial applicationEvent to the list of applicationEvents as a groupCommand.
          // 
          Evado.Model.UniForm.Command command = listGroup.addCommand (
            applicationEvent.LinkText,
            EuAdapter.ADAPTER_ID,
            EuAdapterClasses.Events.ToString ( ),
            Evado.Model.UniForm.ApplicationMethods.Get_Object );

          command.SetGuid ( applicationEvent.Guid );

        }//END trial applicationEvent list iteration loop

        this.LogValue ( "command count: " + listGroup.CommandList.Count );

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.ApplicationEvent_List_Error_Message;

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

      //
      // Determine if the user has access to this page and log and error if they do not.
      //
      if ( this.Session.UserProfile.hasManagementAccess == false )
      {
        this.LogIllegalAccess (
          "Evado.UniForm.Clinical.ApplicationEvents.getListObject",
          this.Session.UserProfile );

        this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

        return null;
      }

      // 
      // Log access to page.
      // 
      this.LogPageAccess (
        "Evado.UniForm.Clinical.ApplicationEvents.getObject",
        this.Session.UserProfile );

      // 
      // if the parameter value exists then set the customerId
      // 
      Guid objectGuid = PageCommand.GetGuid ( );
      this.LogValue ( "objectGuid: " + objectGuid );

      try
      {
        // 
        // Retrieve the customer object from the database via the DAL and BLL layers.
        // 
        this._ApplicationEvent = this._Bll_ApplicationEvents.getItem ( objectGuid );

        this.LogClass ( this._Bll_ApplicationEvents.Log );

        this.LogValue ( "EventId: " + this._ApplicationEvent.EventId );
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
        this.ErrorMessage = EdLabels.ApplicationEvent_Page_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return this.Session.LastPage;

    }//END getObject method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="ClientDataObject">Evado.Model.UniForm.AppData object.</param>
    /// <returns>ClientApplicationData object</returns>
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

      ClientDataObject.Id = this._ApplicationEvent.Guid;
      ClientDataObject.Title = EdLabels.ApplicationEvent_Page_Title;

      ClientDataObject.Page.Id = ClientDataObject.Id;
      ClientDataObject.Page.Title = ClientDataObject.Title;
      ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      //
      // Add the help button if the help url is defined.
      //
      if ( this.AdapterObjects.HelpUrl != String.Empty )
      {
        Evado.Model.UniForm.Command helpCommand = ClientDataObject.Page.addCommand (
         EdLabels.Label_Help_Command_Title,
         EuAdapter.ADAPTER_ID,
         EuAdapterClasses.Events.ToString ( ),
         Model.UniForm.ApplicationMethods.Get_Object );

        helpCommand.Type = Evado.Model.UniForm.CommandTypes.Html_Link;

        helpCommand.AddParameter ( Model.UniForm.CommandParameters.Link_Url,
           EvcStatics.createHelpUrl (
            this.AdapterObjects.HelpUrl,
             Evado.Model.Digital.EvPageIds.Application_Event ) );
      }

      // 
      // create the page pageMenuGroup
      // 
      Evado.Model.UniForm.Group pageGroup = ClientDataObject.Page.AddGroup (
        String.Empty,
        String.Empty,
        Evado.Model.UniForm.EditAccess.Inherited );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
      pageGroup.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      // 
      // Create the customer id object
      // 
      pageField = pageGroup.createReadOnlyTextField (
        String.Empty,
        EdLabels.ApplicationEvent_Date_Time_Field_Label,
        this._ApplicationEvent.DateTime.ToString ( "dd MMM yyyy HH:mm:ss" ) );
      pageField.Layout = EuAdapter.DefaultFieldLayout;

      // 
      // Create the event id object
      // {
      if ( this._ApplicationEvent.EventId == 0
        || this._ApplicationEvent.EventId == 1 )
      {
        this._ApplicationEvent.EventId = ( int ) EvEventCodes.Ok;
      }

      EvEventCodes code = ( EvEventCodes ) this._ApplicationEvent.EventId;

      String content = this._ApplicationEvent.EventId.ToString ( "000000" )
        + " > " +
         Evado.Model.EvStatics.enumValueToString ( code );

      pageField = pageGroup.createReadOnlyTextField (
        String.Empty,
        EdLabels.ApplicationEvent_Event_Id_Field_Label,
        content );
      pageField.Layout = EuAdapter.DefaultFieldLayout;

      // 
      // Create the type id object
      // 
      pageField = pageGroup.createReadOnlyTextField (
        String.Empty,
        EdLabels.ApplicationEvent_Type_Field_Label,
         Evado.Model.EvStatics.enumValueToString ( this._ApplicationEvent.Type ) );
      pageField.Layout = EuAdapter.DefaultFieldLayout;

      // 
      // Create the Category id object
      // 
      pageField = pageGroup.createReadOnlyTextField (
        String.Empty,
        EdLabels.ApplicationEvent_Category_Field_Label,
        this._ApplicationEvent.Category );
      pageField.Layout = EuAdapter.DefaultFieldLayout;

      // 
      // Create the description object
      // 
      pageField = pageGroup.createFreeTextField (
        String.Empty,
        EdLabels.ApplicationEvent_Description_Field_Label,
        this._ApplicationEvent.Description,
        80,
        40 );
      pageField.Layout = EuAdapter.DefaultFieldLayout;

      // 
      // Create the user object
      // 
      pageField = pageGroup.createReadOnlyTextField (
        String.Empty,
        EdLabels.ApplicationEvent_UserName_Field_Label,
        this._ApplicationEvent.UserName );
      pageField.Layout = EuAdapter.DefaultFieldLayout;

    }//END Method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
  }
}//END namespace