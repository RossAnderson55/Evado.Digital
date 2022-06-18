/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\ApplicationEvents.cs" company="EVADO HOLDING PTY. LTD.">
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

 
using Evado.Model;
using Evado.Digital.Bll;
using Evado.Digital.Model;
// using Evado.Web;

namespace Evado.Digital.Adapter
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

      this._Bll_ApplicationEvents = new Evado.Digital.Bll.EvApplicationEvents ( this.ClassParameters );

      this.LogInitMethod ( "EuApplicationEvents initialisation" );
      this.LogInit ( "ServiceUserProfile.UserId: " + ServiceUserProfile.UserId );
      this.LogInit ( "SessionObjects.UserProfile.Userid: " + this.Session.UserProfile.UserId );
      this.LogInit ( "SessionObjects.UserProfile.CommonName: " + this.Session.UserProfile.CommonName );
      this.LogInit ( "UniFormBinaryFilePath: " + this.UniForm_BinaryFilePath );

      this.LogInit ( "Settings:" );
      this.LogInit ( "-LoggingLevel: " + Settings.LoggingLevel );
      this.LogInit ( "-UserId: " + Settings.UserProfile.UserId );
      this.LogInit ( "-UserCommonName: " + Settings.UserProfile.CommonName );

      if ( this.Session.ApplicationEventList == null )
      {
        this.Session.ApplicationEventList = new List<EvApplicationEvent> ( );
      }

      if ( this.AdapterObjects.EventCodeSelectionList == null )
      {
        this.AdapterObjects.EventCodeSelectionList = new List<EvOption> ( );
      }


    }//END Method


    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class constants and variables.

    private const String CONST_EVENT_FIELD_ID = "EV";
    private const String CONST_TYPE_FIELD_ID = "ETYP";
    private const String CONST_USER_FIELD_ID = "EUSR";
    private const String CONST_START_DATE_FIELD_ID = "ESD";
    private const String CONST_FINISH_DATE_FIELD_ID = "EFD";

    private Evado.Digital.Bll.EvApplicationEvents _Bll_ApplicationEvents = new Evado.Digital.Bll.EvApplicationEvents ( );

    private EvApplicationEvent _ApplicationEvent = new EvApplicationEvent ( );

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class public methods

    // ==================================================================================
    /// <summary>
    /// This method gets the trial site object.
    /// 
    /// </summary>
    /// <param name="PageCommand">ClientPateEvado.UniForm.Model.EuCommand object</param>
    /// <returns>ClientApplicationData</returns>
    //  ----------------------------------------------------------------------------------
    public Evado.UniForm.Model.EuAppData getClientDataObject (
      Evado.UniForm.Model.EuCommand PageCommand )
    {
      this.LogMethod ( "getClientDataObject" );
      this.LogValue ( "PageCommand Content: " + PageCommand.getAsString ( false, true ) );
      try
      {
        // 
        // Initialise the methods variables and objects.
        // 
        Evado.UniForm.Model.EuAppData clientDataObject = new Evado.UniForm.Model.EuAppData ( );

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
        // save the event selection parameters.
        //
        this.saveEventLogSelectionParameters ( PageCommand );

        // 
        // Determine the method to be called
        // 
        switch ( PageCommand.Method )
        {
          case Evado.UniForm.Model.EuMethods.List_of_Objects:
            {
              clientDataObject = this.getListObject ( PageCommand );

              break;
            }
          case Evado.UniForm.Model.EuMethods.Get_Object:
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
    /// <param name="PageCommand">Evado.UniForm.Model.EuCommand object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    public Evado.UniForm.Model.EuAppData getListObject (
      Evado.UniForm.Model.EuCommand PageCommand )
    {
      this.LogMethod ( "getListObject" );
      try
      {
        // 
        // Initialise the methods variables and objects.
        //      
        Evado.UniForm.Model.EuAppData clientDataObject = new Evado.UniForm.Model.EuAppData ( );

        //
        // initialise the list page.
        //
        clientDataObject.Title = EdLabels.ApplicationEvent_List_Page_Title;
        clientDataObject.Page.Title = clientDataObject.Title;
        clientDataObject.Id = Guid.NewGuid ( );

        this.queryApplicatinEventLog ( );

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
    /// This methods saves the event log selection parameters to the session object.
    /// </summary>
    //  ---------------------------------------------------------------------------------
    private void saveEventLogSelectionParameters (
      Evado.UniForm.Model.EuCommand PageCommand )
    {
      this.LogMethod ( "saveEventLogSelectionParameters" );
      //
      // Retrieve the selection options.
      //
      String value = String.Empty;
      this.Session.EventId = EvEventCodes.Null;

      //
      // Start date selection
      //
      if ( PageCommand.hasParameter ( EuApplicationEvents.CONST_START_DATE_FIELD_ID ) == true )
      {
        var startDate = EvStatics.getDateTime ( PageCommand.GetParameter ( EuApplicationEvents.CONST_START_DATE_FIELD_ID ) );

        this.LogValue ( "startDate: " + startDate );

        if ( this.Session.EventStartDate != startDate )
        {
          this.Session.EventStartDate = startDate;
          this.Session.ApplicationEventList = new List<EvApplicationEvent> ( );
        }
      }

      //
      // finish data selection.
      //
      if ( PageCommand.hasParameter ( EuApplicationEvents.CONST_FINISH_DATE_FIELD_ID ) == true )
      {
        var finishDate = EvStatics.getDateTime ( PageCommand.GetParameter ( EuApplicationEvents.CONST_FINISH_DATE_FIELD_ID ) );

        if ( this.Session.EventFinishDate != finishDate )
        {
          this.Session.EventFinishDate = finishDate;
          this.Session.ApplicationEventList = new List<EvApplicationEvent> ( );
        }
      }

      //
      // Event Identifier selection.
      //
      if ( PageCommand.hasParameter ( EuApplicationEvents.CONST_EVENT_FIELD_ID ) == true )
      {
        var eventId = PageCommand.GetParameter<EvEventCodes> ( EuApplicationEvents.CONST_EVENT_FIELD_ID );

        if ( eventId != this.Session.EventId )
        {
          this.Session.EventId = eventId;
          this.Session.ApplicationEventList = new List<EvApplicationEvent> ( );
        }
      }

      //
      // Event type selection.
      //
      this.Session.EventType = PageCommand.GetParameter ( EuApplicationEvents.CONST_TYPE_FIELD_ID );

      if ( PageCommand.hasParameter ( EuApplicationEvents.CONST_TYPE_FIELD_ID ) == true )
      {
        var eventId = PageCommand.GetParameter ( EuApplicationEvents.CONST_TYPE_FIELD_ID );

        if ( eventId != this.Session.EventType )
        {
          this.Session.EventType = eventId;
          this.Session.ApplicationEventList = new List<EvApplicationEvent> ( );
        }
      }

      //
      // user selection
      //
      this.Session.EventUserName = PageCommand.GetParameter ( EuApplicationEvents.CONST_USER_FIELD_ID );

      //
      // set the minimium event range.
      //
      if ( this.Session.EventStartDate == Evado.Digital.Model.EvcStatics.CONST_DATE_MIN_RANGE )
      {
        this.Session.EventStartDate =
         Evado.Digital.Model.EvcStatics.getDateTime ( DateTime.Now.ToString ( "dd MMM yyyy" ) );
        this.Session.EventFinishDate = this.Session.EventStartDate.AddDays ( 1 );
      }

      this.LogValue ( "EventId: " + this.Session.EventId );
      this.LogValue ( "EventType: " + this.Session.EventType );
      this.LogValue ( "EventUserName: " + this.Session.EventUserName );
      this.LogValue ( "EventStartDate: " + this.Session.EventStartDate );
      this.LogValue ( "EventFinishDate: " + this.Session.EventFinishDate );
      this.LogValue ( "ApplicationEventList.Count: " + this.Session.ApplicationEventList.Count );
      this.LogMethodEnd ( "saveEventLogSelectionParameters" );

    }//END Method

    // ==================================================================================
    /// <summary>
    /// This methods queries the application event log and returns a list of 
    /// Event log objects sved to session ApplicationEventLog list.
    /// </summary>
    //  ---------------------------------------------------------------------------------
    private void queryApplicatinEventLog ( )
    {
      this.LogMethod ( "queryApplicatinEventLog" );

      if ( this.Session.ApplicationEventList.Count > 0 )
      {
        this.LogDebug ( "No list update needed." );
        this.LogMethodEnd ( "queryApplicatinEventLog" );
        return;
      }

      // 
      // get the list of customers.
      // 
      this.Session.ApplicationEventList = this._Bll_ApplicationEvents.getEventList (
        (int) this.Session.EventId,
        this.Session.EventType,
        String.Empty,
        this.Session.EventStartDate,
        this.Session.EventFinishDate,
        String.Empty );

      this.LogClass ( this._Bll_ApplicationEvents.Log );

      this.LogValue ( "list count: " + this.Session.ApplicationEventList.Count );

      //
      // initialise the user selection values.
      //
      String hasUserName = String.Empty;
      this.Session.EventUserSelectionList = new List<EvOption> ( );
      this.Session.EventUserSelectionList.Add ( new EvOption ( ) );

      //
      // iterate through the user list
      //
      foreach ( EvApplicationEvent appEvent in this.Session.ApplicationEventList )
      {
        if ( hasUserName.Contains ( appEvent.UserId ) == true
          || appEvent.UserId == String.Empty )
        {
          continue;
        }

        if ( appEvent.UserId != String.Empty )
        {
          this.Session.EventUserSelectionList.Add ( new EvOption ( appEvent.UserId ) );
          hasUserName += ";" + appEvent.UserId;
        }
      }

      this.LogValue ( "User name list count: " + this.Session.EventUserSelectionList.Count );

      this.LogMethodEnd ( "queryApplicatinEventLog" );

    }//END method

    // ==================================================================================
    /// <summary>
    /// This methods returns a pageMenuGroup object contains a selection of applicationEvents.
    /// </summary>
    /// <param name="PageObject">Evado.UniForm.Model.EuPage object</param>
    /// <returns>Evado.UniForm.Model.EuGroup object</returns>
    //  ---------------------------------------------------------------------------------
    public void getSelectionGroup (
      Evado.UniForm.Model.EuPage PageObject )
    {
      this.LogMethod ( "getSelectionGroup" );

      this.LogValue ( "EventStartDate: " + this.Session.EventStartDate );
      this.LogValue ( "EventFinishDate: " + this.Session.EventFinishDate );

      // 
      // initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.EuField groupField = new Evado.UniForm.Model.EuField ( );
      int rangeStartDate = 2020;
      int rangeFinishDate = DateTime.Now.Year;

      //
      // create the selection group.
      //
      Evado.UniForm.Model.EuGroup selectionGroup = PageObject.AddGroup (
        EdLabels.ApplicationEvent_Selection_Group_Title,
        Evado.UniForm.Model.EuEditAccess.Enabled );
      selectionGroup.Layout = Evado.UniForm.Model.EuGroupLayouts.Full_Width;

      // 
      // Set the selection to the start date
      // 
      groupField = selectionGroup.createDateField (
        EuApplicationEvents.CONST_START_DATE_FIELD_ID,
        EdLabels.ApplicationEvent_Start_Date_Selection_Field_Label,
        this.Session.EventStartDate,
        rangeStartDate, rangeFinishDate );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      groupField.AddParameter ( Evado.UniForm.Model.EuFieldParameters.Snd_Cmd_On_Change, 1 );

      // 
      // Set the selection to the start date
      // 
      groupField = selectionGroup.createDateField (
        EuApplicationEvents.CONST_FINISH_DATE_FIELD_ID,
        EdLabels.ApplicationEvent_Finish_Date_Selection_Field_Label,
        this.Session.EventFinishDate,
        rangeStartDate, rangeFinishDate );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      groupField.AddParameter ( Evado.UniForm.Model.EuFieldParameters.Snd_Cmd_On_Change, 1 );

      //
      // get the list of event codes.
      //
      if ( this.AdapterObjects.EventCodeSelectionList.Count == 0 )
      {
        this.AdapterObjects.EventCodeSelectionList = Evado.Model.EvStatics.getOptionsFromEnum ( typeof ( EvEventCodes ), true );

        Evado.Digital.Model.EvcStatics.sortOptionListValues ( this.AdapterObjects.EventCodeSelectionList );
      }

      // 
      // Set the selection to the event id
      // 
      groupField = selectionGroup.createSelectionListField (
        EuApplicationEvents.CONST_EVENT_FIELD_ID,
        EdLabels.ApplicationEvent_Event_Id_Selection_Field_Label,
        this.Session.EventId.ToString ( ),
        this.AdapterObjects.EventCodeSelectionList );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      groupField.AddParameter ( Evado.UniForm.Model.EuFieldParameters.Snd_Cmd_On_Change, 1 );


      //
      // get the list of event ids.
      //
      List<EvOption> optionList = Evado.Model.EvStatics.getOptionsFromEnum ( typeof ( EvApplicationEvent.EventType ), true );
      // 
      // Set the selection to the type id
      // 
      groupField = selectionGroup.createSelectionListField (
        EuApplicationEvents.CONST_TYPE_FIELD_ID,
        EdLabels.ApplicationEvent_Type_Selection_Field_Label,
        this.Session.EventType,
        optionList );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      groupField.AddParameter ( Evado.UniForm.Model.EuFieldParameters.Snd_Cmd_On_Change, 1 );

      // 
      // Set the selection to the USER Name
      // 
      Evado.Digital.Model.EvcStatics.sortOptionListValues ( this.Session.EventUserSelectionList );

      groupField = selectionGroup.createSelectionListField (
        EuApplicationEvents.CONST_USER_FIELD_ID,
        EdLabels.ApplicationEvent_UserName_Selection_Field_Label,
        this.Session.EventUserName,
        this.Session.EventUserSelectionList );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      groupField.AddParameter ( Evado.UniForm.Model.EuFieldParameters.Snd_Cmd_On_Change, 1 );


      // 
      // Create a custom groupCommand to process the selection.
      // 
      Evado.UniForm.Model.EuCommand customCommand = selectionGroup.addCommand (
        EdLabels.ApplicationEvent_Selection_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Events.ToString ( ),
        Evado.UniForm.Model.EuMethods.Custom_Method );

      // 
      // Set the custom groupCommand parameter.
      // 
      customCommand.setCustomMethod ( Evado.UniForm.Model.EuMethods.List_of_Objects );

    }//END getSelectionGroup method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">Evado.UniForm.Model.EuPage object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    public void getListGroup (
      Evado.UniForm.Model.EuPage PageObject )
    {
      this.LogMethod ( "getListGroup" );
      try
      {
        // 
        // Create the new pageMenuGroup.
        // 
        Evado.UniForm.Model.EuGroup listGroup = PageObject.AddGroup (
          EdLabels.ApplicationEvent_List_Group_Title,
          Evado.UniForm.Model.EuEditAccess.Inherited );
        listGroup.Layout = Evado.UniForm.Model.EuGroupLayouts.Full_Width;
        listGroup.CmdLayout = Evado.UniForm.Model.EuGroupCommandListLayouts.Vertical_Orientation;

        // 
        // generate the page links.
        // 
        foreach ( EvApplicationEvent applicationEvent in this.Session.ApplicationEventList )
        {
          if ( applicationEvent.UserId != this.Session.EventUserName
            && this.Session.EventUserName != String.Empty)
          {
            continue;
          }

          // 
          // Add the trial applicationEvent to the list of applicationEvents as a groupCommand.
          // 
          Evado.UniForm.Model.EuCommand command = listGroup.addCommand (
            applicationEvent.LinkText,
            EuAdapter.ADAPTER_ID,
            EuAdapterClasses.Events.ToString ( ),
            Evado.UniForm.Model.EuMethods.Get_Object );

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
    /// <param name="PageCommand">Evado.UniForm.Model.EuCommand object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.UniForm.Model.EuAppData getObject (
      Evado.UniForm.Model.EuCommand PageCommand )
    {
      this.LogMethod ( "getObject" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.EuAppData clientDataObject = new Evado.UniForm.Model.EuAppData ( );

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
    /// <param name="ClientDataObject">Evado.UniForm.Model.EuAppData object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private void getDataObject (
      Evado.UniForm.Model.EuAppData ClientDataObject )
    {
      this.LogMethod ( "getDataObject" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.EuCommand pageCommand = new Evado.UniForm.Model.EuCommand ( );
      Evado.UniForm.Model.EuField pageField = new Evado.UniForm.Model.EuField ( );

      ClientDataObject.Id = this._ApplicationEvent.Guid;
      ClientDataObject.Title = EdLabels.ApplicationEvent_Page_Title;

      ClientDataObject.Page.Id = ClientDataObject.Id;
      ClientDataObject.Page.Title = ClientDataObject.Title;
      ClientDataObject.Page.EditAccess = Evado.UniForm.Model.EuEditAccess.Enabled;

      //
      // Add the help button if the help url is defined.
      //
      if ( this.AdapterObjects.HelpUrl != String.Empty )
      {
        Evado.UniForm.Model.EuCommand helpCommand = ClientDataObject.Page.addCommand (
         EdLabels.Label_Help_Command_Title,
         EuAdapter.ADAPTER_ID,
         EuAdapterClasses.Events.ToString ( ),
         Evado.UniForm.Model.EuMethods.Get_Object );

        helpCommand.Type = Evado.UniForm.Model.EuCommandTypes.Http_Link;

        helpCommand.AddParameter ( Evado.UniForm.Model.EuCommandParameters.Link_Url,
           EvcStatics.createHelpUrl (
            this.AdapterObjects.HelpUrl,
             Evado.Digital.Model.EdStaticPageIds.Application_Event ) );
      }

      // 
      // create the page pageMenuGroup
      // 
      Evado.UniForm.Model.EuGroup pageGroup = ClientDataObject.Page.AddGroup (
        String.Empty,
        String.Empty,
        Evado.UniForm.Model.EuEditAccess.Inherited );
      pageGroup.Layout = Evado.UniForm.Model.EuGroupLayouts.Full_Width;
      pageGroup.EditAccess = Evado.UniForm.Model.EuEditAccess.Enabled;

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
        this._ApplicationEvent.EventId = (int) EvEventCodes.Ok;
      }

      EvEventCodes code = (EvEventCodes) this._ApplicationEvent.EventId;

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
        this._ApplicationEvent.UserId );
      pageField.Layout = EuAdapter.DefaultFieldLayout;

    }//END Method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
  }
}//END namespace