/***************************************************************************************
 * <copyright file="BLL\EvSchedules.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvSchedules business object.
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
  /// This business object manages the EvSchedules in the system.
  /// </summary>
  public class EvSchedules : EvBllBase 
  {
    #region class initialisation method.

    //=================================================================================
    /// <summary>
    /// This method initialises the schedule BLL class.
    /// </summary>
    //----------------------------------------------------------------------------------
    public EvSchedules ( )
    {
      this.ClassNameSpace = "Evado.Bll.Clinical.EvSchedules.";
    }

    //=================================================================================
    /// <summary>
    /// This method initialises the schedule BLL class.
    /// </summary>
    /// <param name="Settings"></param>
    //----------------------------------------------------------------------------------
    public EvSchedules ( EvClassParameters Settings )
    {
      this.ClassParameter = Settings;
      this.ClassNameSpace = "Evado.Bll.Clinical.EvSchedules.";

      if ( this.ClassParameter.LoggingLevel == 0 )
      {
        this.ClassParameter.LoggingLevel = Evado.Dal.EvStaticSetting.LoggingLevel;
      }

      this._DAL_Schedules.ClassParameters = Settings;
    }

    #endregion

    #region Class Enumeration
    #endregion

    #region Class variables and properties
    //
    //  Create instantiate the DAL class 
    // 
    private Evado.Dal.Clinical.EvSchedules _DAL_Schedules = new Evado.Dal.Clinical.EvSchedules ( );
    #endregion

    #region Schedule view methods


    // =====================================================================================
    /// <summary>
    /// This class returns a list of Schedule objects based on ProjectId
    /// </summary>
    /// <param name="ProjectId">string a trial identifier</param>
    /// <param name="ScheduleId">Integer the schedule identier</param>
    /// <param name="ScheduleState">EvSchedule.ScheduleStates enunmerated list value</param>
    /// <returns>List of EvSchedule: a list of schedule objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving a list of schedule objects based on ProjectId. 
    /// 
    /// 2. Return a list of schedule objects
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvSchedule> getScheduleList ( 
      String ProjectId,
      int ScheduleId,
      EvSchedule.ScheduleStates ScheduleState )
    {
      this.LogMethod( "getScheduleList method. " );
      this.LogValue ( "ProjectId: " + ProjectId );
      this.LogValue ( "ScheduleId: " + ScheduleId );
      this.LogValue ( "ScheduleState: " + ScheduleState );

      // 
      // Execute the selectionList method.
      // 
      List<EvSchedule> scheduleList = this._DAL_Schedules.getScheduleList ( 
        ProjectId,
        ScheduleId,
        ScheduleState );

      this.LogClass ( this._DAL_Schedules.Log );

      return scheduleList;

    }//END getMilestoneList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of options for Schedule objects based on ProjectId
    /// </summary>
    /// <param name="ProjectId">string: a trial identifier</param>
    /// <param name="IsIssued">Boolean: true, if the schedule is issued</param>
    /// <param name="UseGuid">Boolean: true, user GUID identifiers</param>
    /// <param name="SelectionList">Boolean: true, if the selectionlist is selected</param>
    /// <returns>List of EvSchedule: a list of options for schedule objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving a list of options for schedule objects based on ProjectId. 
    /// 
    /// 2. Return a list of options for schedule objects
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> getOptionList ( 
      string ProjectId,
      bool IsIssued,
      bool UseGuid,
      bool SelectionList )
    {
      this.LogMethod ( "getList method. " );
      this.LogDebug ( "ProjectId: " + ProjectId );

      // 
      // Execute the selectionList method.
      // 
      List<EvOption> list = this._DAL_Schedules.getOptionList ( ProjectId, IsIssued, UseGuid, SelectionList );
      this.LogClass ( this._DAL_Schedules.Log );

      this.LogMethodEnd ( "getList" );
      return list;

    }//END getList method.

    #endregion

    #region Schedule retrieval methods

    //  =======================================================================
    /// <summary>
    /// This class returns the latest Version of the schedule.
    /// Author: Andres Felipe Castano
    /// </summary>
    /// <param name="ProjectId">string: a trial identifier</param>
    /// <returns>Integer: the latest issued version</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the list of Options for the scheduel objects
    /// 
    /// 2. Loop through the list and check for the latest version. 
    /// 
    /// 3. Return the latest version.
    /// </remarks>
    //  -----------------------------------------------------------------------
    public int getLatestIssuedVersion ( string ProjectId )
    {
      this.LogMethod ( "getLatestIssuedVersion method" );
      int latestVersion = 0;

      List<EvSchedule> list = this._DAL_Schedules.getScheduleList ( ProjectId, 1, EvSchedule.ScheduleStates.Issued );
      this.LogClass ( this._DAL_Schedules.Log );

      //
      // Loop through the Options list and check for the latest version
      //
      foreach ( EvSchedule schedule in list )
      {
        if ( schedule.Version > latestVersion )
        {
          latestVersion = schedule.Version;
        }

      }

      this.LogMethodEnd ( "getLatestIssuedVersion" );
      return latestVersion;

    }//END getLatestIssuedVersion class

    // =====================================================================================
    /// <summary>
    /// This class retrieves the Schedule object based on Guid
    /// </summary>
    /// <param name="Guid">Guid: a global unique identifier</param>
    /// <returns>EvSchedule: a schedule object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the Schedule object based on Guid
    /// 
    /// 2. Return the Schedule object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvSchedule getSchedule ( Guid Guid )
    {
      this.LogMethod( "getVisitSchedule. " );
      this.LogDebug ( "Guid: " + Guid );
      EvSchedule schedule = this._DAL_Schedules.getSchedule ( Guid );
      this.LogClass ( this._DAL_Schedules.Log );

      this.LogMethodEnd ( "getVisitSchedule" );
      return schedule;

    }//END getSchedule method

    // =====================================================================================
    /// <summary>
    /// This class retrieves the Schedule object based on ProjectId
    /// </summary>
    /// <param name="ProjectId">String: a Project identifier</param>
    /// <param name="ScheduleId">int: a Schedule identifier</param>
    /// <param name="withActivities">bool: include activities.</param>
    /// <returns>EvSchedule: a schedule object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the Schedule object based on ProjectId
    /// 
    /// 2. Return the Schedule object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvSchedule getSchedule ( 
      String ProjectId, 
      int ScheduleId,
      bool withActivities )
    {
      this.LogMethod ( "getVisitSchedule. " );
      this.LogDebug ( "ProjectId: " + ProjectId );
      this.LogDebug ( "ScheduleId: " + ScheduleId );
      this.LogDebug ( "withActivities: " + withActivities );

      //
      // Initialise the methods variables and objects.
      //
      EvSchedule schedule = this._DAL_Schedules.getSchedule ( ProjectId, ScheduleId, withActivities );

      this.LogClass ( this._DAL_Schedules.Log );

      this.LogMethodEnd ( "getVisitSchedule" );

      return schedule;

    }//END getSchedule method

    #endregion

    #region Schedule update methods

    // =====================================================================================
    /// <summary>
    /// This class processes the save items on Schedule ResultData table. 
    /// </summary>
    /// <param name="Schedule">EvSchedule: a schedule object</param>
    /// <returns>EvEventCodes: an event code for saving items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the UpdateByUserId or VisitId or UserCommonName is empty.
    /// 
    /// 2. Execute the method for deleting items, if the action code is delete. 
    /// 
    /// 3. Execute the method for revising items, if the action code is revise
    /// 
    /// 4. Execute the method for adding items, if the Guid is empty. 
    /// 
    /// 5. Else execute the method for updating items. 
    /// 
    /// 6. Return an event code of method execution. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes saveSchedule ( EvSchedule Schedule )
    {
      // 
      // Instantiate the local variables
      // 
      this.LogMethod ( "saveSchedule method." );
      this.LogDebug ( "Action: " + Schedule.Action );

      //
      // Initialise the methods variables and objects.
      //
      EvEventCodes iReturn = 0;

      // 
      // Check that the EvSchedule id is valid
      // 
      if ( Schedule.UpdatedByUserId == String.Empty )
      {
        this.LogValue ( "UpdatedByUserId = Empty. " );
        return EvEventCodes.Identifier_User_Id_Error;
      }

      if ( Schedule.TrialId == String.Empty )
      {
        this.LogValue ( "ProjectId = Empty. " );
        return EvEventCodes.Identifier_Project_Id_Error;
      }

      if ( Schedule.UserCommonName == String.Empty )
      {
        this.LogValue ( "UserId = Empty. " );
        return EvEventCodes.Identifier_User_Id_Error;
      }

      if ( Schedule.Action == EvSchedule.ScheduleActions.Null )
      {
        Schedule.Action = EvSchedule.ScheduleActions.Save;
      } 
      // 
      // If action withdrawn then set to superseded
      // 
      if ( Schedule.Action == EvSchedule.ScheduleActions.Delete_Schedule )
      {
        this.LogValue ( "Delete Schedule" );
        iReturn = this._DAL_Schedules.deleteSchedule ( Schedule );
        this.LogClass ( this._DAL_Schedules.Log );

        return iReturn;
      }

      // 
      // If  revise then create a new draft copy
      // 
      if ( Schedule.Action == EvSchedule.ScheduleActions.Revise )
      {
        this.LogValue ( "Revising Schedule" );
        iReturn = this._DAL_Schedules.reviseSchedule ( Schedule );
        this.LogClass ( this._DAL_Schedules.Log );

        return iReturn;
      }

      // 
      // Update the schedule.
      // 
      this.updateState ( Schedule );

      // 
      // Add a new trial Report to the database
      // 
      if ( Schedule.Guid == Guid.Empty )
      {
        this.LogValue ( "Add Schedule" );
        iReturn = this._DAL_Schedules.addSchedule ( Schedule );
        this.LogClass ( this._DAL_Schedules.Log );

        return iReturn;
      }

      // 
      // If there is NO Xml content then update the trial Report properties.
      // 
      this.LogValue ( "Update Schedule" );
      iReturn = this._DAL_Schedules.updateSchedule ( Schedule );
      this.LogClass ( this._DAL_Schedules.Log );

      return iReturn;

    }//END saveSchedule method 

    // =====================================================================================
    /// <summary>
    /// This class updates the state of the schedule object. 
    /// </summary>
    /// <param name="Schedule">EvSchedule: the schedule object.</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Update the Authenticated UserId if it exists. 
    /// 
    /// 2. Update the user sign off object based on the Schedule object action. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private void updateState ( EvSchedule Schedule )
    {
      this.LogMethod ( "updateState method." );
      this.LogValue ( "Action: " + Schedule.Action );

      // 
      // If the instrument has an authenticated signoff pass the user id to the 
      // to the DAL layer and DB.
      // 
      string AuthenticatedUserId = String.Empty;
      EvUserSignoff userSignoff = new EvUserSignoff ( );



      if ( Schedule.IsAuthenticatedSignature == true )
      {
        AuthenticatedUserId = Schedule.UpdatedByUserId;
      }

      // 
      // If action is save then minor Version increment Version and add UpdatedBy entry
      // 

      if ( Schedule.Action == EvSchedule.ScheduleActions.Review )
      {
        this.LogValue ( "Reviewing Schedule" );

        Schedule.State = EvSchedule.ScheduleStates.Reviewed;

        // 
        // Append the signoff object.
        // 
        userSignoff.Type = EvUserSignoff.TypeCode.Schedule_Review_Signoff;
        userSignoff.SignedOffUserId = AuthenticatedUserId;
        userSignoff.SignedOffBy = Schedule.UserCommonName;
        userSignoff.SignOffDate = DateTime.Now;
        userSignoff.Description = "Version: " + Schedule.Version.ToString ( );

        Schedule.Signoffs.Add ( userSignoff );

        return;
      }

      if ( Schedule.Action == EvSchedule.ScheduleActions.Approve )
      {
        this.LogValue ( "Issuing Schedule" );

        Schedule.State = EvSchedule.ScheduleStates.Issued;
        Schedule.ApprovedByUserId = AuthenticatedUserId;
        Schedule.ApprovedBy = Schedule.UserCommonName;
        Schedule.ApprovedDate = DateTime.Now;

        if ( Schedule.Version == 0 )
        {
          Schedule.Version++;
        }
        // 
        // Append the signoff object.
        // 
        userSignoff.Type = EvUserSignoff.TypeCode.Schedule_Approver_Signoff;
        userSignoff.SignedOffUserId = AuthenticatedUserId;
        userSignoff.SignedOffBy = Schedule.UserCommonName;
        userSignoff.SignOffDate = Schedule.ApprovedDate;
        userSignoff.Description = "Version: " + Schedule.Version.ToString ( );

        Schedule.Signoffs.Add ( userSignoff );

        return;
      }

      if ( Schedule.Action == EvSchedule.ScheduleActions.Delete_Schedule )
      {
        Schedule.State = EvSchedule.ScheduleStates.Withdrawn;

        // 
        // Append the signoff object.
        // 
        userSignoff.Type = EvUserSignoff.TypeCode.Schedule_Withdrawal_Signoff;
        userSignoff.SignedOffUserId = AuthenticatedUserId;
        userSignoff.SignedOffBy = Schedule.UserCommonName;
        userSignoff.SignOffDate = DateTime.Now;
        userSignoff.Description = "Version: " + Schedule.Version.ToString ( );

        Schedule.Signoffs.Add ( userSignoff );

        return;
      }

      // 
      // If none of the above occur, save the record
      // 
      this.LogValue ( "Saving Schedule. State:" + Schedule.State );

      // 
      // Reset the approval values if in draft state.
      // 
      if ( Schedule.State == EvSchedule.ScheduleStates.Draft )
      {
        Schedule.ApprovedBy = String.Empty;
        Schedule.ApprovedByUserId = String.Empty;
        Schedule.ApprovedDate = Evado.Model.Digital.EvcStatics.CONST_DATE_NULL;
      }

      return;

    }//END updateState method.

    //  ===============================================================
    /// <summary>
    /// This class deletes the schedule and all of the milestones.
    /// </summary>
    /// <param name="schedule">EvSchedule: a schedule object</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for deleting items from Milestone datatable. 
    /// 
    /// 2. Execute the method for deleting items from Schedule datatable. 
    /// </remarks>
    //  ---------------------------------------------------------------
    public void deleteSchedule ( EvSchedule schedule )
    {
      this.LogMethod ( "deleteSchedule. method. " );

      //
      // Initialise the methods variables and objects.
      //
      EvMilestones mileBll = new EvMilestones ( );
      Evado.Model.Digital.EvUserProfile profile = new Evado.Model.Digital.EvUserProfile ( );

      profile.UpdatedByUserId = schedule.UpdatedByUserId;
      profile.UserCommonName = schedule.UserCommonName;
      profile.UpdatedBy = schedule.UpdatedBy;

      mileBll.deleteMilestones ( profile, schedule.Guid );

      this._DAL_Schedules.deleteSchedule ( schedule );

    }//End of delete schedule method.

    #endregion

  }//END EvSchedules Class.

}//END namespace Evado.Evado.BLL 
