/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\TrialOrganisations.cs" company="EVADO HOLDING PTY. LTD.">
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
using  Evado.Model.Digital;
// using Evado.Web;

namespace Evado.UniForm.Clinical
{
  /// <summary>
  /// This class manages the integration of Evado eclinical alerts with 
  /// Evado.UniFORM interface.
  /// </summary>
  public class EuAlerts : EuClassAdapterBase
  {
    #region Class Initialisation
    /// <summary>
    /// This method initialises the class.
    /// </summary>
    public EuAlerts ( )
    {
      this.ClassNameSpace = "Evado.UniForm.Clinical.EuAlerts.";
    }

    /// <summary>
    /// This method initialises the class and passs in the user profile.
    /// </summary>
    public EuAlerts (
      EuApplicationObjects ApplicationObjects,
      EvUserProfileBase ServiceUserProfile,
      EuSession SessionObjects,
      String UniFormBinaryFilePath,
      EvClassParameters Settings )
    {
      this.ClassNameSpace = "Evado.UniForm.Clinical.EuAlerts.";
      this.ApplicationObjects = ApplicationObjects;
      this.ServiceUserProfile = ServiceUserProfile;
      this.Session = SessionObjects;
      this.UniForm_BinaryFilePath = UniFormBinaryFilePath;
      this.ClassParameters = Settings;


      this.LogInitMethod ( "EuAlerts initialisation" );
      this.LogInit ( "ServiceUserProfile.UserId: " + ServiceUserProfile.UserId );
      this.LogInit ( "SessionObjects.Project.ProjectId: " + this.Session.Application.ApplicationId );
      this.LogInit ( "SessionObjects.UserProfile.Userid: " + this.Session.UserProfile.UserId );
      this.LogInit ( "SessionObjects.UserProfile.CommonName: " + this.Session.UserProfile.CommonName );
      this.LogInit ( "UniFormBinaryFilePath: " + this.UniForm_BinaryFilePath );

      this.LogInit ( "Settings:" );
      this.LogInit ( "-LoggingLevel: " + Settings.LoggingLevel );
      this.LogInit ( "-UserId: " + Settings.UserProfile.UserId );
      this.LogInit ( "-UserCommonName: " + Settings.UserProfile.CommonName );

      this._Bll_Alerts = new EvAlerts ( Settings );

    }//END Method


    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class constants and variables.

    private const string CONST_ALERT_TYPE = "ALERTTYPE";
    private const string CONST_ALERT_STATE = "ALERTSTATE";

    private Evado.Bll.Clinical.EvAlerts _Bll_Alerts = new Evado.Bll.Clinical.EvAlerts ( );
    private List<EvAlert> _AlertView = new List<EvAlert> ( );


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
    public Evado.Model.UniForm.AppData getClientDataObject ( Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getTrialAlertObject" );
      this.LogValue ( "UserProfile.CommonName: " + this.Session.UserProfile.CommonName );
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
              // 
              // Update the object values
              // 
              clientDataObject = this.updateObject ( PageCommand );

              //
              // if the alert is acknowledged the user needs to be diverted to the 
              // relevant object.
              //
              this.generateRedirection ( clientDataObject );

              break;
            }

        }//END Switch

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
        return clientDataObject;

      }
      catch ( Exception Ex )
      {
        this.LogException ( Ex );
      }
      return this.Session.LastPage;

    }//END getTrialAlertObject method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class private methods

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="ClientDataObject">Evado.Model.UniForm.AppData object.</param>
    /// <remarks>
    /// This method consists of following steps:
    /// 
    /// 1. Based on the selection of ALert Type, redirect the user to the specific object
    /// </remarks>
    //  ------------------------------------------------------------------------------
    private void generateRedirection ( 
      Evado.Model.UniForm.AppData ClientDataObject )
    {
      this.LogMethod ( "generateRedirectionCommand" );
      //
      // If the save action is not acknowledgement exit.
      //
      if ( this.Session.Alert.Action != EvAlert.AlertSaveActionCodes.Acknowledge_Alert )
      {
        this.LogMethodEnd ( "generateRedirectionCommand" );
        return;
      }

      this.LogValue ( "Alert.TypeId: " + this.Session.Alert.TypeId );
      this.LogValue ( "Alert.RecordId: " + this.Session.Alert.RecordId );

      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.UniForm.Command groupCommand =  ClientDataObject.Page.Exit;

      //
      // based on the alert type redirect the user to the relevant object.
      switch ( this.Session.Alert.TypeId )
      {
        case EvAlert.AlertTypes.Trial_Record:
          {
            groupCommand = new Model.UniForm.Command (
              EvLabels.Label_Record,
              EuAdapter.APPLICATION_ID,
              EuAdapterClasses.Scheduled_Record.ToString ( ),
              Model.UniForm.ApplicationMethods.Get_Object );

            groupCommand.AddParameter ( EvIdentifiers.RECORD_ID, this.Session.Alert.RecordId );

            break;
          }
        case EvAlert.AlertTypes.Adverse_Event_Report:
        case EvAlert.AlertTypes.AE_Notification_Alert:
          {
            groupCommand = new Model.UniForm.Command (
               EvLabels.Label_Record,
               EuAdapter.APPLICATION_ID,
               EuAdapterClasses.Common_Record.ToString ( ),
               Model.UniForm.ApplicationMethods.Get_Object );

            groupCommand.AddParameter (
              EdRecord.CONST_RECORD_TYPE,
              EvFormRecordTypes.Adverse_Event_Report.ToString ( ) ); 

            groupCommand.AddParameter ( EvIdentifiers.RECORD_ID, this.Session.Alert.RecordId );

            break;
          }
        case EvAlert.AlertTypes.Serious_Adverse_Event_Report:
        case EvAlert.AlertTypes.SAE_Notification_Alert:
          {
            groupCommand = new Model.UniForm.Command (
               EvLabels.Label_Record,
               EuAdapter.APPLICATION_ID,
               EuAdapterClasses.Common_Record.ToString ( ),
               Model.UniForm.ApplicationMethods.Get_Object );

            groupCommand.AddParameter (
              EdRecord.CONST_RECORD_TYPE,
              EvFormRecordTypes.Serious_Adverse_Event_Report.ToString ( ) ); 

            groupCommand.AddParameter ( EvIdentifiers.RECORD_ID, this.Session.Alert.RecordId );

            break;
          }
        case EvAlert.AlertTypes.Concomitant_Medication:
          {
            groupCommand = new Model.UniForm.Command (
               EvLabels.Label_Record,
               EuAdapter.APPLICATION_ID,
               EuAdapterClasses.Common_Record.ToString ( ),
               Model.UniForm.ApplicationMethods.Get_Object );

            groupCommand.AddParameter (
              EdRecord.CONST_RECORD_TYPE,
              EvFormRecordTypes.Concomitant_Medication.ToString ( ) ); 

            groupCommand.AddParameter ( EvIdentifiers.RECORD_ID, this.Session.Alert.RecordId );

            break;
          }
        case EvAlert.AlertTypes.Subject_Record:
          {
            groupCommand = new Model.UniForm.Command (
               EvLabels.Label_Record,
               EuAdapter.APPLICATION_ID,
               EuAdapterClasses.Subjects.ToString ( ),
               Model.UniForm.ApplicationMethods.Get_Object );

            groupCommand.AddParameter ( EvIdentifiers.SUBJECT_ID, this.Session.Alert.RecordId );

            break;
          }//END milestone selection.

      }//END alert type switch

      ClientDataObject.Page.Exit = groupCommand;

      this.LogMethodEnd ( "generateRedirectionCommand" );

    }//ENd generateRedirectionCommand method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand" >Evado.Model.UniForm.Command object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
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

        // 
        // If the user does not have monitor or ResultData manager roles exit the page.
        // 
        if ( this.Session.UserProfile.hasMultiSiteAccess == false
          && this.Session.UserProfile.hasRecordAccess == false )
        {
          this.LogIllegalAccess (
            this.ClassNameSpace + "getListObject",
            this.Session.UserProfile );

          this.ErrorMessage = EvLabels.Record_Access_Error_Message;

          return this.Session.LastPage;
        }

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "getListObject",
          this.Session.UserProfile );

        //
        // Initialise the client ResultData object.
        //
        clientDataObject.Title = EvLabels.Alert_View_Page_Title;
        clientDataObject.Page.Title = clientDataObject.Title;
        clientDataObject.Id = Guid.NewGuid ( );
        this.Session.AlertType = EvAlert.AlertTypes.Null;
        this.Session.AlertState = EvAlert.AlertStates.Not_Closed;
        string stValue = String.Empty;

        this.LogValue ( "data.Title: " + clientDataObject.Title );
        this.LogValue ( "data.Page.Title: " + clientDataObject.Page.Title );
        // 
        // get the groupCommand parameters.
        // 
        stValue = PageCommand.GetParameter ( EuAlerts.CONST_ALERT_TYPE );
        this.Session.AlertType =  Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue<EvAlert.AlertTypes> ( stValue );

        stValue = PageCommand.GetParameter ( EuAlerts.CONST_ALERT_STATE );
        this.Session.AlertState =  Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue<EvAlert.AlertStates> ( stValue );

        // 
        // Query and database.
        // 
        this._AlertView = this._Bll_Alerts.getView (
          this.Session.Application,
          new EvOrganisation(),
          this.Session.UserProfile,
          this.Session.AlertState,
          this.Session.AlertType);

        this.LogValue ( this._Bll_Alerts.Log );

        // 
        // bind output to the datagrid.
        // 
        if ( _AlertView.Count == 0 )  // If nothing returned create a blank row.
        {
          _AlertView.Add ( new EvAlert ( ) );
        }

        //
        // add the alert selection pageMenuGroup.
        //
        this.getListObject_Selection_Group ( clientDataObject.Page );

        // 
        // Add the alert list to the page.
        // 
        this.getListObject_List_Group ( clientDataObject.Page );


        return clientDataObject;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Alert_List_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return this.Session.LastPage;

    }//END getListObject method.
    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page.</param>
    //  ------------------------------------------------------------------------------
    private void getListObject_Selection_Group ( 
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getListObject_Selection_Group" );
      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.UniForm.Field groupField = new Model.UniForm.Field ( );
      Evado.Model.UniForm.Command command = new Model.UniForm.Command ( );
      List<EvOption> optionList = new List<EvOption>();

      // 
      // Create the new pageMenuGroup.
      // 
      Evado.Model.UniForm.Group pageGroup = Page.AddGroup (
        EvLabels.Alert_List_Selection_Group_Title,
        Evado.Model.UniForm.EditAccess.Enabled );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      //
      // generate the alert state option list.
      //
      optionList = EvAlert.getTypeList ( );

      //
      // create the alert type selection
      //
      groupField = pageGroup.createSelectionListField (
        EuAlerts.CONST_ALERT_TYPE,
        EvLabels.Alert_List_Alert_Type_Field_Label,
        this.Session.AlertType.ToString(),
        optionList );
      groupField.Layout = EuFormGenerator.ApplicationFieldLayout;
      groupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );

      //
      // generate the alert types option list.
      //
      optionList = EvAlert.getStateList ( );

      //
      // create the alert type selection field.
      //
      groupField = pageGroup.createSelectionListField (
        EuAlerts.CONST_ALERT_STATE,
        EvLabels.Alert_List_Alert_State_Field_Label,
        this.Session.AlertState.ToString ( ),
        optionList );
      groupField.Layout = EuFormGenerator.ApplicationFieldLayout;
      groupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );


      // 
      // Add the selection groupCommand
      // 
      command = pageGroup.addCommand (
        EvLabels.Alert_List_Selection_Command_Title,
        EuAdapter.APPLICATION_ID,
        EuAdapterClasses.Alert.ToString ( ),
         Evado.Model.UniForm.ApplicationMethods.Custom_Method );
      command.setCustomMethod ( Evado.Model.UniForm.ApplicationMethods.List_of_Objects );


    }//END getListObject_Selection_Group method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page.</param>
    //  ------------------------------------------------------------------------------
    private void getListObject_List_Group (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getListObject_List_Group" );
      try
      {
        // 
        // Create the new pageMenuGroup.
        // 
        Evado.Model.UniForm.Group listGroup = Page.AddGroup (
          EvLabels.Alert_List_Group_Title,
          Evado.Model.UniForm.EditAccess.Inherited_Access );
        listGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
        listGroup.CmdLayout = Evado.Model.UniForm.GroupCommandListLayouts.Vertical_Orientation;

        // 
        // generate the page links.
        // 
        foreach ( EvAlert alert in _AlertView )
        {
          String stAlertTite = alert.Subject;
          Evado.Model.UniForm.Command command = listGroup.addCommand ( stAlertTite,
            EuAdapter.APPLICATION_ID,
            EuAdapterClasses.Alert.ToString ( ),
            Model.UniForm.ApplicationMethods.Get_Object );

          command.SetGuid (alert.Guid );

        }//END Alert list iteration loop

        this.LogValue ( "listGroup command count: " + listGroup.CommandList.Count );

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Alert_List_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException( Ex );
      }

    }//END getListObject method.

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PagCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData getObject (
      Evado.Model.UniForm.Command PagCommand )
    {
      this.LogMethod ( "getObject" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
      Guid alertGuid = Guid.Empty;

      // 
      // If the user does not have monitor or ResultData manager roles exit the page.
      // 
      if ( this.Session.UserProfile.hasMultiSiteAccess == false
        && this.Session.UserProfile.hasRecordAccess == false )
      {
        this.LogIllegalAccess (
          this.ClassNameSpace + "getObject",
          this.Session.UserProfile );

        this.ErrorMessage = EvLabels.Record_Access_Error_Message;

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
      alertGuid = PagCommand.GetGuid();
      this.LogValue ( "alertGuid: " + alertGuid );

      // 
      // return if not trial id
      // 
      if ( alertGuid == Guid.Empty )
      {
        return clientDataObject;
      }

      this.LogValue ( "Alert Guid: " + alertGuid );

      try
      {
        // 
        // Retrieve the customer object from the database via the DAL and BLL layers.
        // 
        if ( this.Session.Alert.Guid != alertGuid )
        {
          this.Session.Alert = this._Bll_Alerts.getItem ( alertGuid );

          this.LogValue ( this._Bll_Alerts.Log );
        }
        this.LogValue ( "SessionObjects.Alert.AlertId: " + this.Session.Alert.AlertId );

        // 
        // return the client ResultData object for the customer.
        // 
        this.getClientDataObject ( clientDataObject );

        return clientDataObject;
      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Alert_Page_Error_Mesage;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return this.Session.LastPage;

    }//END getObject method

    // ==============================================================================
    /// <summary>
    /// This method checks that the parameters contain customer object.
    /// </summary>
    /// <param name="Parameters">List of paremeters to retrieve the selected object.</param>
    /// <returns>Guid object</returns>
    //  ------------------------------------------------------------------------------
    private Guid getGuid ( List<Evado.Model.UniForm.Parameter> Parameters )
    {
      if ( Parameters.Count > 0 )
      {
        for ( int i = 0; i < Parameters.Count; i++ )
        {
          if ( Parameters [ i ].Name ==  Evado.Model.Digital.EvcStatics.CONST_GUID_IDENTIFIER )
          {
            return new Guid ( Parameters [ i ].Value );
          }
        }
      }
      return Guid.Empty;
    }

    // ==============================================================================
    /// <summary>
    /// This method checks that the parameters contain customer object.
    /// </summary>
    /// <param name="Parameters">List of paremeters to retrieve the selected object.</param>
    /// <returns>String parmater value</returns>
    //  ------------------------------------------------------------------------------
    private String getAltertId ( List<Evado.Model.UniForm.Parameter> Parameters )
    {
      if ( Parameters.Count > 0 )
      {
        for ( int i = 0; i < Parameters.Count; i++ )
        {
          if ( Parameters [ i ].Name == EvAlert.AlertFieldNames.Alert_Id.ToString ( ) )
          {
            return Parameters [ i ].Value;
          }
        }
      }
      return String.Empty;
    }

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="ClientClientDataObjectObject">The customer object.</param>
    //  ------------------------------------------------------------------------------
    private void getClientDataObject ( Evado.Model.UniForm.AppData ClientClientDataObjectObject )
    {
      this.LogMethod ( "getClientClientDataObjectObject" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );
      Evado.Model.UniForm.Field pageField = new Evado.Model.UniForm.Field ( );
      bool userHasEditAccess = this.UserHasEditAccess ( );

      //
      // Initialise the client ResultData object.
      //
      ClientClientDataObjectObject.Id = this.Session.Alert.Guid;
      ClientClientDataObjectObject.Title = EvLabels.Alert_Page_Title;

      ClientClientDataObjectObject.Page.Id = ClientClientDataObjectObject.Id;
      ClientClientDataObjectObject.Page.Title = ClientClientDataObjectObject.Title;
      ClientClientDataObjectObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      //
      // If the user has edit access enable the page for the user.
      //
      if ( userHasEditAccess == true )
      {
        ClientClientDataObjectObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
      }

      // 
      // create the general page pageMenuGroup
      // 
      Evado.Model.UniForm.Group pageGroup = ClientClientDataObjectObject.Page.AddGroup (
        EvLabels.Alert_Page_Header_Group_Title,
        String.Empty,
        Evado.Model.UniForm.EditAccess.Inherited_Access );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;

      // 
      // Create the customer id object
      // 
      pageField = pageGroup.createReadOnlyTextField (
        EvAlert.AlertFieldNames.Alert_Id.ToString ( ),
        EvLabels.Label_Alert_Id,
        this.Session.Alert.ToOrgId );
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;

      // 
      // Add from user name
      // 
      pageField = pageGroup.createReadOnlyTextField (
        EvAlert.AlertFieldNames.From_User.ToString ( ),
        EvLabels.Alert_Page_From_Label,
        this.Session.Alert.FromUser );
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;

      // 
      // Add to user name
      // 
      if ( this.Session.Alert.ToUser != String.Empty )
      {
        pageField = pageGroup.createReadOnlyTextField (
          EvAlert.AlertFieldNames.To_User.ToString ( ),
          EvLabels.Alert_Page_To_Label,
          this.Session.Alert.ToUser );
        pageField.Layout = EuFormGenerator.ApplicationFieldLayout;
      }

      // 
      // Add alert milestone name
      // 
      pageField = pageGroup.createReadOnlyTextField (
        EvAlert.AlertFieldNames.Alert_Subject.ToString ( ),
        EvLabels.Alert_Page_Subject_Label,
        this.Session.Alert.Subject );
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;

      // 
      // Add alert message name
      // 
      pageField = pageGroup.createReadOnlyTextField (
        EvAlert.AlertFieldNames.Message.ToString ( ),
        EvLabels.Alert_Page_Message_Label,
        this.Session.Alert.Message );
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;

      // 
      // Add alert message name
      // 
      pageField = pageGroup.createReadOnlyTextField (
        EvAlert.AlertFieldNames.Alert_State.ToString ( ),
        EvLabels.Alert_Page_Status,
        this.Session.Alert.StateDesc );

      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;

      // 
      // include the acknowledged content if it exists.
      // 
      if ( this.Session.Alert.Acknowledged !=  Evado.Model.Digital.EvcStatics.CONST_DATE_NULL )
      {
        pageField = pageGroup.createReadOnlyTextField (
          EvAlert.AlertFieldNames.Acknowledged_Date.ToString ( ),
          EvLabels.Alert_Page_Acknowledged_Date_Label,
          this.Session.Alert.Acknowledged.ToString ( "dd MMM yyy HH:mm" ) );

        pageField.Layout = EuFormGenerator.ApplicationFieldLayout;

        pageField = pageGroup.createReadOnlyTextField (
          EvAlert.AlertFieldNames.Acknowledged_By.ToString ( ),
          EvLabels.Alert_Page_Acknowledged_By_Label,
          this.Session.Alert.AcknowledgedBy );

        pageField.Layout = EuFormGenerator.ApplicationFieldLayout;
      }

      // 
      // include the clodes content if it exists.
      // 
      if ( this.Session.Alert.Closed !=  Evado.Model.Digital.EvcStatics.CONST_DATE_NULL )
      {

        pageField = pageGroup.createReadOnlyTextField (
          EvAlert.AlertFieldNames.Closed_Date.ToString ( ),
          EvLabels.Alert_Page_Closed_Date_Label,
          this.Session.Alert.Closed.ToString ( "dd MMM yyy HH:mm" ) );

        pageField.Layout = EuFormGenerator.ApplicationFieldLayout;

        pageField = pageGroup.createReadOnlyTextField (
          EvAlert.AlertFieldNames.Closed_By.ToString ( ),
          EvLabels.Alert_Page_Closed_By_Label,
          this.Session.Alert.ClosedBy );

        pageField.Layout = EuFormGenerator.ApplicationFieldLayout;
      }

      this.LogValue ( "Edit Access: " + userHasEditAccess );
      this.LogValue ( "Alert.Status: " + this.Session.Alert.State );

      //
      // Add the record access command.
      //
      pageGroup.addCommand ( this.getRecordCommand ( ) );
      
      // 
      // Add page commands if the user has edit access to them.
      // 
      if ( userHasEditAccess == true )
      {
        this.LogValue ( "User has edit access" );

        switch ( this.Session.Alert.State )
        {
          case EvAlert.AlertStates.Raised:
            {
              this.LogValue ( "Alert.Status: Raised" );
              // 
              // Add the save groupCommand
              // 
              pageCommand = pageGroup.addCommand (
                EvLabels.Alert_Save_Command_Title,
                EuAdapter.APPLICATION_ID,
                EuAdapterClasses.Alert.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.Save_Object );

              pageCommand.AddParameter (
                 Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
                EvAlert.AlertSaveActionCodes.Save_Alert.ToString ( ));

              pageCommand.SetGuid( ClientClientDataObjectObject.Id );

              // 
              // Add acknowledge groupCommand.
              // 
              pageCommand = pageGroup.addCommand (
                EvLabels.Alert_Page_Acknowledge_Command,
                EuAdapter.APPLICATION_ID,
                EuAdapterClasses.Alert.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.Save_Object );

              pageCommand.SetGuid ( ClientClientDataObjectObject.Id );

              pageCommand.AddParameter (
                 Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
                EvAlert.AlertSaveActionCodes.Acknowledge_Alert.ToString ( ) );

              // 
              // Add the close groupCommand.
              // 
              pageCommand = pageGroup.addCommand (
                EvLabels.Alert_Page_Close_Command,
                EuAdapter.APPLICATION_ID,
                EuAdapterClasses.Alert.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.Save_Object );

              pageCommand.SetGuid ( ClientClientDataObjectObject.Id );

              pageCommand.AddParameter (
                 Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
                EvAlert.AlertSaveActionCodes.Close_Alert.ToString ( ));

              break;
            }//END raised state.

          case EvAlert.AlertStates.Acknowledged:
            {
              this.LogValue ( "Alert.Status: Acknowledged" );
              // 
              // Add the save groupCommand
              // 
              pageCommand = pageGroup.addCommand (
                EvLabels.Alert_Save_Command_Title,
                EuAdapter.APPLICATION_ID,
                EuAdapterClasses.Alert.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.Save_Object );

              pageCommand.AddParameter (
                 Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
                EvAlert.AlertSaveActionCodes.Save_Alert.ToString ( ));

              pageCommand.SetGuid ( ClientClientDataObjectObject.Id );

              // 
              // Add the close groupCommand.
              // 
              pageCommand = pageGroup.addCommand (
                EvLabels.Alert_Page_Close_Command,
                EuAdapter.APPLICATION_ID,
                EuAdapterClasses.Alert.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.Save_Object );

              pageCommand.SetGuid (  ClientClientDataObjectObject.Id );

              pageCommand.AddParameter (
                 Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
                EvAlert.AlertSaveActionCodes.Close_Alert.ToString ( ) );
              break;
            }//END acknowledge state.

        }//END Alert State switch

      }//END user edit access.

    }//END Method

    // ==================================================================================
    /// <summary>
    /// This method check whether the user has edit access to the alert object..
    /// </summary>
    /// <returns>Bool:  True:  user has edit access.</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.Model.UniForm.Command getRecordCommand ( )
    {
      this.LogMethod ( "UserHasEditAccess" );
      this.LogValue ( "Alert.TypeId:  " + this.Session.Alert.TypeId );
      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.UniForm.Command recordCommand = new Model.UniForm.Command (
        "Title",
        EuAdapter.APPLICATION_ID,
        "Object",
         Model.UniForm.ApplicationMethods.Get_Object ) ;

      switch ( this.Session.Alert.TypeId )
      {
        case EvAlert.AlertTypes.Adverse_Event_Report:
        case EvAlert.AlertTypes.AE_Notification_Alert:
          {
            recordCommand.Title = String.Format ( EvLabels.Alert_Page_Open_Record_Command_Title,
              this.Session.Alert.RecordId );
            recordCommand.Object = EuAdapterClasses.Common_Record.ToString ( );

            recordCommand.AddParameter ( EvIdentifiers.RECORD_ID, this.Session.Alert.RecordId );

            recordCommand.AddParameter (
              EdRecord.CONST_RECORD_TYPE,
              EvFormRecordTypes.Adverse_Event_Report.ToString ( ) ); 

            break;
          }
        case EvAlert.AlertTypes.Serious_Adverse_Event_Report:
        case EvAlert.AlertTypes.SAE_Notification_Alert:
          {
            recordCommand.Title = String.Format ( EvLabels.Alert_Page_Open_Record_Command_Title,
              this.Session.Alert.RecordId );
            recordCommand.Object = EuAdapterClasses.Common_Record.ToString ( );

            recordCommand.AddParameter ( EvIdentifiers.RECORD_ID, this.Session.Alert.RecordId );

            recordCommand.AddParameter (
              EdRecord.CONST_RECORD_TYPE,
              EvFormRecordTypes.Serious_Adverse_Event_Report.ToString ( ) ); 

            break;
          }
        case EvAlert.AlertTypes.Concomitant_Medication:
          {
            recordCommand.Title = String.Format ( EvLabels.Alert_Page_Open_Record_Command_Title,
              this.Session.Alert.RecordId );
            recordCommand.Object = EuAdapterClasses.Common_Record.ToString ( );

            recordCommand.AddParameter ( EvIdentifiers.RECORD_ID, this.Session.Alert.RecordId );

            recordCommand.AddParameter (
              EdRecord.CONST_RECORD_TYPE,
              EvFormRecordTypes.Concomitant_Medication.ToString ( ) ); 

            break;
          }
        case EvAlert.AlertTypes.Subject_Record:
          {
            recordCommand.Title = String.Format ( EvLabels.Alert_Page_Open_Record_Command_Title,
              this.Session.Alert.RecordId );
            recordCommand.Object = EuAdapterClasses.Subjects.ToString ( );

            recordCommand.AddParameter ( EvIdentifiers.RECORD_ID, this.Session.Alert.RecordId );

            break;
          }
        case EvAlert.AlertTypes.Trial_Record:
          {
            recordCommand.Title = String.Format ( EvLabels.Alert_Page_Open_Record_Command_Title,
              this.Session.Alert.RecordId );
            recordCommand.Object = EuAdapterClasses.Scheduled_Record.ToString ( );

            recordCommand.AddParameter ( EvIdentifiers.RECORD_ID, this.Session.Alert.RecordId );

            break;
          }

      }

      return recordCommand;

    }//END getRecordCommandMethod

    // ==================================================================================
    /// <summary>
    /// This method check whether the user has edit access to the alert object..
    /// </summary>
    /// <returns>Bool:  True:  user has edit access.</returns>
    //  ----------------------------------------------------------------------------------
    private bool UserHasEditAccess ( )
    {
      this.LogMethod ( "UserHasEditAccess" );
      this.LogValue ( "Alert.TypeId:  " + this.Session.Alert.TypeId );
      // 
      // Provide monitors and sponsors access to alert notifications.
      // 
      if ( this.Session.Alert.TypeId == EvAlert.AlertTypes.AE_Notification_Alert
         || this.Session.Alert.TypeId == EvAlert.AlertTypes.SAE_Notification_Alert
         || this.Session.Alert.TypeId == EvAlert.AlertTypes.Notiifcation_Alert )
      {
        if ( this.Session.UserProfile.hasMonitorAccess == true
          || this.Session.UserProfile.hasSponsorAccess == true
          || this.Session.UserProfile.hasDataManagerAccess == true )
        {
          return true;
        }
      }

      // 
      // Provide sites with access to query alerts.
      // 
      else if ( this.Session.Alert.TypeId == EvAlert.AlertTypes.Trial_Record
        || this.Session.Alert.TypeId == EvAlert.AlertTypes.Subject_Record
        || this.Session.Alert.TypeId == EvAlert.AlertTypes.Concomitant_Medication
        || this.Session.Alert.TypeId == EvAlert.AlertTypes.Adverse_Event_Report
        || this.Session.Alert.TypeId == EvAlert.AlertTypes.Serious_Adverse_Event_Report )
      {
        if ( this.Session.UserProfile.hasRecordEditAccess == true )
        {
          return true;
        }
      }

      return false;
    }

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="Command">Evado.Model.UniForm.Command object.</param>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData createObject ( Evado.Model.UniForm.Command Command )
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
        this.Session.Alert = new EvAlert ( );
        this.Session.Alert.Guid =  Evado.Model.Digital.EvcStatics.CONST_NEW_OBJECT_ID;

        this.getClientDataObject ( data );


        this.LogDebug ( "Exit createObject method. ID: " + data.Id + ", Title: " + data.Title );

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
        this.LogException(  Ex  );     
      }

      return this.Session.LastPage;

    }//END method

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.ClientClientDataObjectCommand object.</param>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData updateObject ( Evado.Model.UniForm.Command PageCommand )
    {
      try
      {
        this.LogMethod ( "updateObject" );
        this.LogValue ( "Parameter PageCommand: "
          + PageCommand.getAsString ( false, true ) );

        this.LogDebug ( "Clinical.Alert: " );
        this.LogDebug ( "Guid: " + this.Session.Alert.Guid );
        this.LogDebug ( "ProjectId: " + this.Session.Alert.ProjectId );
        this.LogDebug ( "Title: " + this.Session.Alert.Subject );
        EvAlert.AlertSaveActionCodes saveAction = EvAlert.AlertSaveActionCodes.Save_Alert;

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "updateObject",
          this.Session.UserProfile );

        // 
        // Initialise the update variables.
        // 
        this.Session.Alert.UpdatedByUserId = this.Session.UserProfile.UserId;
        this.Session.Alert.UserCommonName = this.Session.UserProfile.CommonName;

        // 
        // IF the guid is new object id  alue then set the save object for adding to the database.
        // 
        if ( this.Session.Alert.Guid == Evado.Model.Digital.EvcStatics.CONST_NEW_OBJECT_ID )
        {
          this.Session.Alert.Guid = Guid.Empty;
        }
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
        this.updateObjectValue ( PageCommand.Parameters );

        // 
        // Get the save action message value.
        // 
        String stSaveAction = PageCommand.GetParameter ( Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION );

        // 
        // Resets the save action to the parameter passed in the page groupCommand.
        // 
        if ( stSaveAction != String.Empty )
        {
          saveAction = Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue<EvAlert.AlertSaveActionCodes> ( stSaveAction );
        }
        this.Session.Alert.Action = saveAction;
        this.Session.Alert.UpdatedByUserId = this.Session.UserProfile.UserId;
        this.Session.Alert.UserCommonName = this.Session.UserProfile.UserCommonName;

        // 
        // update the object.
        // 
        EvEventCodes result = this._Bll_Alerts.saveAlert ( this.Session.Alert );

        // 
        // get the debug ResultData.
        // 
        this.LogValue ( this._Bll_Alerts.Log );

        // 
        // if an error state is returned create log the event.
        // 
        if ( result != EvEventCodes.Ok )
        {
          string StEvent = this._Bll_Alerts.Log + " returned error message: " + Evado.Model.Digital.EvcStatics.getEventMessage ( result );
          this.LogError ( EvEventCodes.Database_Record_Update_Error, StEvent );

          switch ( result )
          {
            case EvEventCodes.Identifier_Project_Id_Error:
              {
                this.ErrorMessage = EvLabels.Project_Identifier_Empty_Error_Message;
                break;
              }
            default:
              {
                this.ErrorMessage = EvLabels.Alert_Update_Error_Message;
                break;
              }
          }

          this.LogMethodEnd ( "updateObject" );
          return this.Session.LastPage;
        }

        this.LogMethodEnd ( "updateObject" );
        return new Model.UniForm.AppData();

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Alert_Update_Error_Message;

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
    private void updateObjectValue ( List<Evado.Model.UniForm.Parameter> Parameters )
    {
      this.LogMethod ( "updateObjectValue" );
      this.LogValue ( "Parameters.Count: " + Parameters.Count );
      this.LogValue ( "Project.Guid: " + this.Session.Application.Guid );

      // 
      // Iterate through the parameter values updating the ResultData object
      // 
      foreach ( Evado.Model.UniForm.Parameter parameter in Parameters )
      {
        this.LogValue ( " " + parameter.Name + " > " + parameter.Value );

        if ( parameter.Name.Contains ( "Guid" ) == false
          && parameter.Name != Evado.Model.UniForm.CommandParameters.Custom_Method.ToString()
          && parameter.Name !=  Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION )
        {
          this.LogValue ( " >> UPDATED" );
          try
          {
            EvAlert.AlertFieldNames fieldName =  Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue<EvAlert.AlertFieldNames> ( parameter.Name );

            this.Session.Alert.setValue ( fieldName, parameter.Value );

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