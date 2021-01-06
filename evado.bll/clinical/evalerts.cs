/***************************************************************************************
 * <copyright file="BLL\EvAlerts.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2020 EVADO HOLDING PTY. LTD..  All rights reserved.
 *     
 *      The use and distribution terms for this software are contained in the file
 *      named license.txt, which can be found in the root of this distribution.
 *      By using this software in any fashion, you are agreeing to be bound by the
 *      terms of this license.
 *     
 *      You must not remove this notice, or any other, from this software.
 *     
 * </copyright>
 * 
 * Description: 
 *  This class contains the EvAlerts business object.
 *
 ****************************************************************************************/

using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;

//Evado. namespace references.
using Evado.Model;
using Evado.Dal;
using Evado.Model.Digital;


namespace Evado.Bll.Clinical
{
  /// <summary>
  /// A business Component used to manage user roles
  /// The m_xfs.Model.Process is used in most methods 
  /// and is used to store serializable information about an account
  /// </summary>
  public class EvAlerts: EvBllBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EvAlerts ( )
    {
      this.ClassNameSpace = "Evado.Bll.Clinical.EvAlerts.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EvAlerts ( EvClassParameters Settings )
    {
      this.ClassParameter = Settings;
      this.ClassNameSpace = "Evado.Bll.Clinical.EvAlerts.";

      if ( this.ClassParameter.LoggingLevel == 0 )
      {
        this.ClassParameter.LoggingLevel = Evado.Dal.EvStaticSetting.LoggingLevel;
      }

      this._dalProjectAlerts = new Evado.Dal.Clinical.EvAlerts ( Settings );
    }
    #endregion

    #region Object Initialisation
    // 
    // Create instantiate the DAL class 
    // 
    private Evado.Dal.Clinical.EvAlerts _dalProjectAlerts = new Evado.Dal.Clinical.EvAlerts ( );
    
    #endregion

    #region trial alert selectionList queries

    // =====================================================================================
    /// <summary>
    /// This class returns a list of Alert objects based on the passed parameters. 
    /// </summary>
    /// <param name="Project">Evado.Model.Digital.EvProject: The project identifier.</param>
    /// <param name="ProjectOrganisation">Evado.Model.Digital.EvOrganisation: The Current User profile.</param>
    /// <param name="CurrentUser">Evado.Model.Digital.EvUserProfile: The Current User profile.</param>
    /// <param name="AlertState">EvAlert.AlertStates: (Optional) The alert state.</param>
    /// <param name="AlertType">EvAlert.AlertTypes: (Optional) The alert QueryType.</param>
    /// <returns>List of EvAlert: a list of Alert object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exeute the method for retrieving the selection Alerts list
    /// 
    /// 2. Return the selection list. 
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvAlert> getView (
       Evado.Model.Digital.EdApplication Project,
      Evado.Model.Digital.EvOrganisation ProjectOrganisation,
      Evado.Model.Digital.EvUserProfile CurrentUser,
      EvAlert.AlertStates AlertState,
      EvAlert.AlertTypes AlertType )
    {
      this.LogMethod ("getView method. " );
      this.LogDebug ( "ProjectId: " + Project.ApplicationId );
      this.LogDebug ( "OrgId: " + ProjectOrganisation.OrgId );
      this.LogDebug ( "UserName: " + CurrentUser.CommonName );
      this.LogDebug ( "Alert State: " + AlertState );

      //
      // Initialise the methods variables and objects.
      //
      String orgId = String.Empty;
      Boolean bNotification = false;

      //
      // If the user has record access then set it to the user organisation.
      // For 
      //
      if ( CurrentUser.hasRecordEditAccess == true )
      {
        orgId = ProjectOrganisation.OrgId;
      }

      //
      // Set the bNotification and userCommonName to default values,
      // if the role is ProjectManager or Monitor or Sponsor. 
      //
      if ( CurrentUser.hasManagementEditAccess == true)
      {
        bNotification = true;
        orgId = String.Empty;
      }

      this.LogDebug ( "bNotification: " + bNotification );
      this.LogDebug ( "orgId: " + orgId );
      //
      // Get the list of raised alerts.
      //
      List<EvAlert> view = this._dalProjectAlerts.getView (
        Project.ApplicationId,
        bNotification,
        orgId, 
        String.Empty,
        AlertState,
        AlertType );

      this.LogDebug ( this._dalProjectAlerts.Status );

      // 
      // Filter the studies based on user profile.
      // 
      this.FilterViews ( CurrentUser, Project, view );

      //
      // Return selectionList to UI
      //
      return view;

    }//End getMilestoneList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of Alert objects based on the passed parameters. 
    /// </summary>
    /// <param name="ProjectId">string: (Optional) The trial identifier.</param>
    /// <param name="Notification">Boolean: true, if the notification exists</param>
    /// <param name="OrgId">string: an organization identifier</param>
    /// <param name="State">EvAlert.AlertStates: (Optional) The alert state.</param>
    /// <param name="AlertType">EvAlert.AlertTypes: (Optional) The alert QueryType.</param>
    /// <returns>List of EvAlert: a list of Alert object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exeute the method for retrieving the selection Alerts list
    /// 
    /// 2. Return the selection list. 
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvAlert> getView (
      string ProjectId,
      Boolean Notification,
      string OrgId,
      EvAlert.AlertStates State,
      EvAlert.AlertTypes AlertType )
    {
      this.LogMethod ( "getView method. " );
      this.LogDebug ( "ProjectId: " + ProjectId );
      this.LogDebug ( "OrgId: " + OrgId );
      this.LogDebug ( "Notification: " + Notification );
      this.LogDebug ( "Alert State: " + State );

      List<EvAlert> view = this._dalProjectAlerts.getView (
        ProjectId,
        Notification,
        OrgId,
        String.Empty,
        State,
        AlertType );
      this.LogDebug ( this._dalProjectAlerts.Status );

      //
      // Return selectionList to UI
      //
      return view;

    }// End getMilestoneList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of Alert objects based on the passed parameters. 
    /// </summary>
    /// <param name="ProjectId">string: (Optional) The trial identifier.</param>
    /// <param name="Notification">Boolean: true, if the notification exists</param>
    /// <param name="OrgId">string: an organization identifier</param>
    /// <param name="State">EvAlert.AlertStates: (Optional) The alert state.</param>
    /// <param name="OrderBy">string: Sorting order.</param>
    /// <returns>List of EvAlert: a list of Alert object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exeute the method for retrieving the selection Alerts list
    /// 
    /// 2. Return the selection list. 
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvAlert> getView (
      string ProjectId,
      Boolean Notification,
      string OrgId,
      EvAlert.AlertStates State,
      string OrderBy )
    {
      this.LogMethod ( "getView method. " );
      this.LogDebug ( "ProjectId: " + ProjectId );
      this.LogDebug ( "OrgId: " + OrgId );
      this.LogDebug ( "State: " + State );

      //
      // Query the database.
      //
      List<EvAlert> view = this._dalProjectAlerts.getView (
        ProjectId,
        Notification,
        OrgId,
        String.Empty,
        State,
        EvAlert.AlertTypes.Null );
      this.LogDebug ( this._dalProjectAlerts.Status );

      //
      // Return selectionList to UI
      //
      return view;

    }//End getMilestoneList method.

    // =====================================================================================
    /// <summary>
    /// This class filters out the alert object items that the user should not be seeing.
    /// </summary>
    /// <param name="CurrentUser">Evado.Model.Digital.EvUserProfile: The currentMonth user's profile</param>
    /// <param name="Project">the current Project</param>
    /// <param name="View">List of EvAlert: a list of alert object.</param>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Loop through the Alerts list and validate whether the currentuser is not a ProjectStaff. 
    /// 
    /// 2. Remove the Alert object
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private void FilterViews (
      Evado.Model.Digital.EvUserProfile CurrentUser,
       Evado.Model.Digital.EdApplication Project,
      List<EvAlert> View )
    {
      this.LogMethod ( "FilterViews method. " );
      // 
      // Filter out the studies the user should not be seeing.
      // 
      for ( int iCount = 0; iCount < View.Count; iCount++ )
      {
        if ( CurrentUser.hasManagementAccess == false )
        {
          string sProjectId = ( (EvAlert) View [ iCount ] ).ProjectId;
          this.LogDebug( "ProjectId: " + sProjectId );

          //CurrentUser.setProjectRole ( Project );

          if ( CurrentUser.hasManagementEditAccess == true )
          {
            continue;
          }

          if ( CurrentUser.hasRecordEditAccess == true )
          {
            continue;
          }

        }
      }
      
      this.LogDebug( "Count: " + View.Count );
      this.LogMethodEnd ("FilterViews" );

    }//End FilterViews method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of option objects based on VisitId, OrgId, CurrentUser and State. 
    /// </summary>
    /// <param name="Project">string: (Optional) The trial identifier.</param>
    /// <param name="OrgId">string: (Optional) The Organisation identifier.</param>
    /// <param name="CurrentUser">Evado.Model.Digital.EvUserProfile: (Optional) The user profile.</param>
    /// <param name="State">EvAlert.AlertStates: (Optional) The option state.</param>
    /// <returns>List of EvOption: A list of Options</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Get the Options list of selection alert objects 
    /// 
    /// 2. Filter the options list with the current user profile. 
    /// 
    /// 3. Return the filtered options list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> getList (
      Model.Digital.EdApplication Project,
      String OrgId,
      Evado.Model.Digital.EvUserProfile CurrentUser,
      EvAlert.AlertStates State )
    {
      this.LogMethod ( "getList method." );
      this.LogDebug ( "ProjectId: " + Project.ApplicationId );
      this.LogDebug ( "OrgId: " + OrgId );
      this.LogDebug ( "UserName: " + CurrentUser.CommonName );
      this.LogDebug ( "State: " + State );

      //
      // Get the list of trial alerts.
      //
      List<EvOption> list = this._dalProjectAlerts.getList (
        Project.ApplicationId,
        OrgId,
        CurrentUser.CommonName,
        State,
        false );
      this.LogDebug ( this._dalProjectAlerts.Status );

      // 
      // Filter the studies based on user profile.
      // 
      this.FilterLists ( CurrentUser, Project, list );

      //
      // Return selectionList to UI
      //
      return list;

    }//End getList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of options based on the VisitId and ByGuid condition
    /// </summary>
    /// <param name="ProjectId">string: (Optional) The trial identifier.</param>
    /// <param name="ByGuid">Boolean: True, if the options list is selected by Guid.</param>
    /// <returns>List of EvOption: a list of option objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving a selection list of Option object. 
    /// 
    /// 2. Return a selection list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------

    public List<EvOption> getList ( string ProjectId, bool ByGuid )
    {
      this.LogMethod ( "getList method. " );
      this.LogDebug ( "ProjectId: " + ProjectId );

      List<EvOption> list = this._dalProjectAlerts.getList ( ProjectId, String.Empty, String.Empty, EvAlert.AlertStates.Null, ByGuid );
      this.LogDebug ( this._dalProjectAlerts.Status );

      return list;

    }//End getList method.

    // =====================================================================================
    /// <summary>
    /// This class filters out the Option object items that the user should not be seeing.
    /// </summary>
    /// <param name="CurrentUser">Evado.Model.Digital.EvUserProfile: a current user profile object</param>
    /// <param name="Project">The current project</param>
    /// <param name="View">List of EvOption: a list options</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Loop through the Options list and validate 
    /// whether the Current user is not the trailUser and VisitId is not empty.
    /// 
    /// 2. Remove the option items that has no role to access Project. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private void FilterLists (
      Evado.Model.Digital.EvUserProfile CurrentUser,
       Evado.Model.Digital.EdApplication Project,
      List<EvOption> View )
    {
      this.LogMethod ( "FilterLists method. " );

      // 
      // Filter out the studies the user should not be seeing.
      // 
      for ( int iCount = 0; iCount < View.Count; iCount++ )
      {
        string sProjectId = ( View [ iCount ] ).Value;

      this.LogDebug( "ProjectId: " + sProjectId );

        // 
        // If the User is Cro staff or the trial Id is null then skip the 
        // row in the currentSchedule.
        // 
        if ( CurrentUser.hasRecordAccess == false
          && sProjectId.Length > 0 )
        {

          //CurrentUser.setProjectRole ( Project );

          if ( CurrentUser.hasRecordAccess == false )
          {
            this.LogDebug ( " DELETED" );
            View.RemoveAt ( iCount );
            iCount--;
          }
        }//END If remove Option items
      }//END For loop

    }//End FilterLists method.

    #endregion

    #region trial alert retrieval queries

    // =====================================================================================
    /// <summary>
    /// This class retrieves an Alert ResultData table based on Alert's Guid. 
    /// </summary>
    /// <param name="AlertGuid">Guid: (Mandatory) the alert's global Unique identifier.</param>
    /// <returns>EvAlert: The Project alert ResultData object</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Execute the method for retrieving the Alert object
    /// 
    /// 2. Return the Alert object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvAlert getItem ( Guid AlertGuid )
    {
      this.LogMethod ( "getItem method. " );
      this.LogDebug ( "AlertGuid: " + AlertGuid );

      EvAlert _ProjectAlert = this._dalProjectAlerts.getItem ( AlertGuid );
      this.LogDebug ( this._dalProjectAlerts.Status );

      return _ProjectAlert;

    }//END getItem method

    // =====================================================================================
    /// <summary>
    /// This class retrieves an Alert ResultData table based on AlertId. 
    /// </summary>
    /// <param name="AlertId">string: an Alert identifier</param>
    /// <returns>EvAlert: The Project alert ResultData object</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Execute the method for retrieving the Alert ResultData object
    /// 
    /// 2. Return the Alert ResultData object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvAlert getItem ( string AlertId )
    {
      this.LogMethod ( "getItem method " );
      this.LogDebug ( "AlertId: " + AlertId );

      EvAlert _ProjectAlert = _dalProjectAlerts.getItem ( AlertId );
      this.LogDebug ( this._dalProjectAlerts.Status );

      return _ProjectAlert;

    }//END getItem method

    #endregion

    #region project alert Save methods

    // =====================================================================================
    /// <summary>
    /// This class saves items to the Alert ResultData table.
    /// </summary>
    /// <param name="Alert">EvAlert: A Project Alert ResultData object</param>
    /// <returns>EvEventCodes: an event code for saving items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Update the Alert object's state
    /// 
    /// 2. Execute the method for adding items, if the Alert's Guid is empty
    /// 
    /// 3. Else, execute the method for updating items. 
    /// 
    /// 4. Return the event code for the execution. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes saveAlert ( EvAlert Alert )
    {
      this.LogMethod ( "saveItem method " );
      this.LogDebug ( "Guid: " + Alert.Guid );
      this.LogDebug ( "AlertId: " + Alert.AlertId );
      this.LogDebug ( "TypeId: " + Alert.TypeId );
      this.LogDebug ( "Action: " + Alert.Action );
      // 
      // Define the local variables.
      // 
      EvEventCodes iReturn = EvEventCodes.Ok;

      //
      // Update the _ProjectAlert record state to reflect the requested action.
      // 
      this.updateState ( Alert );

      //
      // If the _ProjectAlert FormUid is zero or empty, then create a new alert record.
      // 
      if ( Alert.Guid == Guid.Empty )
      {
        iReturn = this._dalProjectAlerts.addItem ( Alert );
        this.LogDebug ( this._dalProjectAlerts.Status );
        return iReturn;
      }

      //
      // Update the alert record.
      // 
      iReturn = this._dalProjectAlerts.updateItem ( Alert );
      this.LogDebug ( this._dalProjectAlerts.Status );

      return iReturn;

    }//END saveAlert method

    // =====================================================================================
    /// <summary>
    /// This class updates the state of the Alert object. 
    /// </summary>
    /// <param name="Alert">EvAlert: The ProjectAlert object.</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Update the Alert object's values based on the Save action code. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private void updateState ( EvAlert Alert )
    {
      this.LogMethod ( "updateState, Action: " + Alert.Action );
      //
      // Set the DateTime strings for the update.
      // 
      string stDateTimeNow = DateTime.Now.ToString ( "MMM dd yyyy HH:mm" );

      //
      // Update _ProjectAlert state for Saved action.
      // 
      if ( Alert.Action == EvAlert.AlertSaveActionCodes.Raise_Alert )
      {
        Alert.State = EvAlert.AlertStates.Raised;
        Alert.FromUser = Alert.UserCommonName;
        Alert.Raised = DateTime.Now;
        this.LogDebug ( "Raised" );
      }

      //
      // Update _ProjectAlert states for recieve action.
      // 
      else if ( Alert.Action == EvAlert.AlertSaveActionCodes.Acknowledge_Alert )
      {
        Alert.State = EvAlert.AlertStates.Acknowledged;
        Alert.AcknowledgedByUserId = Alert.UpdatedByUserId;
        Alert.AcknowledgedBy = Alert.UserCommonName;
        Alert.Acknowledged = DateTime.Now;
        this.LogDebug ( "Acknowledged." );
      }

      //
      // Update _ProjectAlert states for Raise action.
      // 
      else if ( Alert.Action == EvAlert.AlertSaveActionCodes.Close_Alert
        && Alert.State == EvAlert.AlertStates.Raised )
      {
        Alert.State = EvAlert.AlertStates.Closed;
        Alert.AcknowledgedByUserId = Alert.UpdatedByUserId;
        Alert.AcknowledgedBy = Alert.UserCommonName;
        Alert.Acknowledged = DateTime.Now;

        Alert.ClosedBy = Alert.UserCommonName;
        Alert.ClosedByUserId = Alert.UpdatedByUserId;
        Alert.Closed = DateTime.Now;
        this.LogDebug ( "Notification - Alert Closed." );

      }

      //
      // Update _ProjectAlert states for Raise action.
      // 
      else if ( Alert.Action == EvAlert.AlertSaveActionCodes.Close_Alert )
      {
        Alert.State = EvAlert.AlertStates.Closed;
        Alert.ClosedBy = Alert.UserCommonName;
        Alert.ClosedByUserId = Alert.UpdatedByUserId;
        Alert.Closed = DateTime.Now;
        this.LogDebug ( "Alert Closed." );
      }

      // 
      // If a new comment has been added then append to the comments newField
      // 
      if ( Alert.NewMessage != String.Empty )
      {
        string sMessage = "\r\n" + Alert.NewMessage
          + "\r\nBy: " + Alert.UserCommonName
          + " at: " + stDateTimeNow
          + "\r\n__________________________________________________"
          + "\r\n" + Alert.Message;

        Alert.Message = sMessage;
      }

      this.LogDebug ( "Alert State: " + Alert.State );
      this.LogValue ( "Alert Status update completed" );

    }//END updateState method.

    #endregion

    #region project alert Processing update

    // =====================================================================================
    /// <summary>
    /// This class acknowledges items from the Alert ResultData table. 
    /// </summary>
    /// <param name="RecordId">string: (Mandatoy) A record identifier</param>
    /// <param name="TypeId">EvAlert.AlertTypes: (Mandatoy) An alert QueryType identifier</param>
    /// <param name="UserCommonName">string: (Mandatoy) User's Common Name</param>
    /// <returns>EvEventCodes: an event code for acknowleging items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Retrieve a list of alert object. 
    /// 
    /// 2. Exit, if the Alerts list is empty. 
    /// 
    /// 3. Else, extract the first row to the Alert object. 
    /// 
    /// 4. Update the Alert state and execute the method for acknowledging items. 
    /// 
    /// 5. Return an event code for acknowledging items. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes AcknowledgeAlert (
      String RecordId,
      EvAlert.AlertTypes TypeId,
      String UserCommonName )
    {
      this.LogMethod ( "AcknowledgeAlert method." );
      this.LogDebug ( "RecordId: " + RecordId );
      this.LogDebug ( "TypeId: " + TypeId );
      // 
      // Define the local variables.
      // 
      EvEventCodes iReturn = EvEventCodes.Ok;

      // 
      // Retrieve the Records Alerts.
      // 
      ArrayList alertList = this._dalProjectAlerts.getViewByRecord ( RecordId, TypeId, EvAlert.AlertStates.Raised );

      // 
      // No alerts to be Acknowledged.
      // 
      if ( alertList.Count == 0 )
      {
        return EvEventCodes.Ok;
      }

      EvAlert alert = (EvAlert) alertList [ 0 ];
      alert.State = EvAlert.AlertStates.Acknowledged;
      alert.Action = EvAlert.AlertSaveActionCodes.Acknowledge_Alert;
      alert.UserCommonName = UserCommonName;

      //
      // Update the _ProjectAlert record state to reflect the requested action.
      // 
      updateState ( alert );

      //
      // Update the _ProjectAlert record.
      // 
      iReturn = this._dalProjectAlerts.updateItem ( alert );

      this.LogDebug ( this._dalProjectAlerts.Status );
      return iReturn;

    }//END AcknowledgeAlert method

    // =====================================================================================
    /// <summary>
    /// This class closes the items on Alert ResultData table. 
    /// </summary>
    /// <param name="RecordId">string: (Mandatoy) A record identifier</param>
    /// <param name="UserCommonName">string: (Mandatoy) User's Common Name</param>
    /// <returns>EvEventCodes: an event code for closing items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Get an alerts list and exit, if the list is empty. 
    /// 
    /// 2. Else, loop through the alerts list. 
    /// 
    /// 3. Acknowledge the Alert's items if the Alert has raised state. 
    /// 
    /// 4. Execute the method to close the items. 
    /// 
    /// 5. Return an event code for closing items. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes CloseAlert (
      String RecordId,
      String UserCommonName )
    {
      this.LogMethod ( "CloseAlert method. " );
      this.LogDebug ( "RecordId: " + RecordId );
      this.LogDebug ( "UserCommonName: " + UserCommonName );
      // 
      // Define the local variables.
      // 
      EvEventCodes iReturn = EvEventCodes.Ok;

      // 
      // Retrieve the Records Alerts.
      // 
      ArrayList alertList = this._dalProjectAlerts.getViewByRecord ( RecordId,
        EvAlert.AlertTypes.Null,
        EvAlert.AlertStates.Null );

      this.LogDebug ( this._dalProjectAlerts.Status );

      // 
      // No alerts to be closed.
      // 
      if ( alertList.Count == 0 )
      {
        return EvEventCodes.Ok;
      }

      foreach ( EvAlert alert in alertList )
      {
        this.LogDebug ( "AlertId: " + alert.AlertId
          + " State: " + alert.State );

        if ( alert.State != EvAlert.AlertStates.Closed )
        {
          this.LogDebug ( "NotClosed the Alert" );

          alert.Action = EvAlert.AlertSaveActionCodes.Close_Alert;
          alert.UserCommonName = UserCommonName;

          // 
          // If alert raised then acknowledge it, prior to closing it.
          // 
          if ( alert.State == EvAlert.AlertStates.Raised )
          {
            this.LogDebug ( "Acknowledging the Alert" );
            alert.AcknowledgedBy = UserCommonName;
            alert.Acknowledged = DateTime.Now;
          }

          //
          // Update the _ProjectAlert record state to reflect the requested action.
          // 
          this.updateState ( alert );

          //
          // Update the _ProjectAlert record.
          // 
          iReturn = this._dalProjectAlerts.updateItem ( alert );

          this.LogDebug ( this._dalProjectAlerts.Status );
          if ( iReturn < EvEventCodes.Ok )
          {
            return iReturn;
          }

        }//END Not Closed Alerts.

      }
      return iReturn;

    } //END CloseAlert method

    #endregion

    #region Static method: Raise alert method.

    // =====================================================================================
    /// <summary>
    /// This class raises items on the Alert ResultData object. 
    /// </summary>
    /// <param name="ProjectId">string: A Project identifier</param>
    /// <param name="OrgId">string: an organization identifier</param>
    /// <param name="RecordId">string: A Record identifier</param>
    /// <param name="Message">string: An alert's message</param>
    /// <param name="FromUserName">string: a user name whose alert is sent from</param>
    /// <param name="ToUserName">string: a user name whose alert is sent to</param>
    /// <param name="TypeId">EvAlert.AlertTypes: An alert QueryType</param>
    /// <returns>EvEventCodes: an event code for raising items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Update the FirstSubject's value based on the RecordTypeId. 
    /// 
    /// 2. Set the Alert object's values and execute the method for adding Alert object's values to database. 
    /// 
    /// 3. Return the event code for raising items. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public static EvEventCodes RaiseAlert (
      String ProjectId,
      String OrgId,
      String RecordId,
      String Message,
      String FromUserName,
      String ToUserName,
      EvAlert.AlertTypes TypeId )
    {
      // 
      // Define the local variables.
      // 
      EvEventCodes iReturn = EvEventCodes.Ok;
      Evado.Dal.Clinical.EvAlerts dalProjectAlerts = new Evado.Dal.Clinical.EvAlerts ( );

      EvAlert alert = new EvAlert ( );
      string subject = String.Empty;

      //
      // Update the milestone's values based on the RecordTypeId. 
      //
      switch ( TypeId )
      {
        case EvAlert.AlertTypes.Trial_Record:
          {
            subject = String.Format (
              EvLabels.Alert_Data_Record_Alert_Message,
              RecordId,
              DateTime.Now.ToString ( "dd MMM yyyy" ) );
            break;
          }

        case EvAlert.AlertTypes.Adverse_Event_Report:
          {
            subject = String.Format (
              EvLabels.Alert_Adverse_Event_Alert_Subject,
              RecordId,
              DateTime.Now.ToString ( "dd MMM yyyy" ) );
            break;
          }

        case EvAlert.AlertTypes.Serious_Adverse_Event_Report:
          {
            subject = String.Format (
              EvLabels.Alert_Serious_Adverse_Event_Alert_Subject,
              RecordId,
              DateTime.Now.ToString ( "dd MMM yyyy" ) );
            break;
          }

        case EvAlert.AlertTypes.Concomitant_Medication:
          {
            subject = String.Format (
              EvLabels.Alert_Concomitant_Medication_Alert_Subject,
              RecordId,
              DateTime.Now.ToString ( "dd MMM yyyy" ) );
            break;
          }

        case EvAlert.AlertTypes.Subject_Record:
          {
            subject = String.Format (
              EvLabels.Alert_Subject_Alert_Subject,
              RecordId,
              DateTime.Now.ToString ( "dd MMM yyyy" ) );
            break;
          }
      }

      //
      // Update the Alert object's values. 
      //
      alert.ProjectId = ProjectId;
      alert.ToOrgId = OrgId;
      alert.RecordId = RecordId;
      alert.Subject = subject;
      alert.NewMessage = Message;
      alert.UserCommonName = FromUserName;
      alert.ToUser = ToUserName;
      alert.FromUser = FromUserName;
      alert.Action = EvAlert.AlertSaveActionCodes.Raise_Alert;
      alert.TypeId = TypeId;
      //
      // Add the Alert object's values to the database
      // 
      iReturn = dalProjectAlerts.addItem ( alert );

      EvApplicationEvent applicationEvent = new EvApplicationEvent (
        EvApplicationEvent.EventType.Action,
        EvEventCodes.Ok,
        "Evado.Bll.Clinical.EvAlerts",
        dalProjectAlerts.Status,
        FromUserName );

      EvApplicationEvents.NewEvent ( applicationEvent );

      return iReturn;

    }//END RaiseAlert method

    #endregion

  }//END EvAlerts Class.

}//END namespace Evado.Bll.Clinical
