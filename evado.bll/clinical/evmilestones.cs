/***************************************************************************************
 * <copyright file="BLL\EvMilestones.cs" company="EVADO HOLDING PTY. LTD.">
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
 ****************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using Evado.Dal;
//Evado. namespace references.
using Evado.Model;
using Evado.Model.Digital;


namespace Evado.Bll.Clinical
{
  /// <summary>
  /// This business object manages the EvMilestones in the system.
  /// </summary>
  public class EvMilestones : EvBllBase 
  {
    #region class initialisation method.

    //=================================================================================
    /// <summary>
    /// This method initialises the schedule BLL class.
    /// </summary>
    //----------------------------------------------------------------------------------
    public EvMilestones ( )
    {
      this.ClassNameSpace = "Evado.Bll.Clinical.EvMilestones.";
    }

    //=================================================================================
    /// <summary>
    /// This method initialises the schedule BLL class.
    /// </summary>
    /// <param name="Settings"></param>
    //----------------------------------------------------------------------------------
    public EvMilestones ( EvClassParameters Settings )
    {
      this.ClassParameter = Settings;
      this.ClassNameSpace = "Evado.Bll.Clinical.EvMilestones.";

      if ( this.ClassParameter.LoggingLevel == 0 )
      {
        this.ClassParameter.LoggingLevel = Evado.Dal.EvStaticSetting.LoggingLevel;
      }

      this._DAL_Milestones.ClassParameters = Settings;
    }

    #endregion

    #region Class Constants and Variables

    //
    //  Create instantiate the DAL class 
    // 
    private Evado.Dal.Clinical.EvMilestones _DAL_Milestones = new Evado.Dal.Clinical.EvMilestones ( );

    // 
    // Initialise static variables 
    // 
    // DebugLog stores the business object status conditions.
    // 
    private System.Text.StringBuilder _DebugLog = new System.Text.StringBuilder ( );

    /// <summary>
    /// This property contains the debug log string
    /// </summary>
    public string DebugLog
    {
      get
      {
        return _DebugLog.ToString ( );
      }
    }

    /// <summary>
    /// This property contains the Html debug log
    /// </summary>
    public string DebugLog_Html
    {
      get
      {
        return this.DebugLog.Replace ( "\r\n", "<br/>" );
      }
    }

    #endregion

    #region Class Preparation Views and Lists.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of Milestone objects based on the passed parameters. 
    /// </summary>
    /// <param name="ScheduleGuid">Guid: a global unique identifier</param>
    /// <param name="Type">EvMilestone.MilestoneTypes: a milestone QueryType</param>
    /// <returns>List of EvMilestone: a list of milestone objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving a list of milestone objects
    /// 
    /// 2. Return a list of milestone objects.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvMilestone> getMilestoneList (
      Guid ScheduleGuid,
      EvMilestone.MilestoneTypes Type )
    {
      this.LogMethod ( "Evado.Bll.Clinical.EvMilestones.getPreparationList method. " );
      this.LogValue ( "ScheduldGuid: " + ScheduleGuid );
      this.LogValue ( "Type: " + Type );

      // 
      // Execute the selectionList method.
      // 
      List<EvMilestone> milestoneList = this._DAL_Milestones.getMilestoneList ( ScheduleGuid, Type );
      this.LogValue ( this._DAL_Milestones.Log );

      return milestoneList;

    }//END getPreparationList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of Milestone objects based on ProjectId, Type and withActivities condition
    /// </summary>
    /// <param name="ProjectId">string: a trial identifier</param>
    /// <param name="ScheduleId">int: (Mandatory) The schedule identifier</param>
    /// <param name="Type">EvMilestone.MilestoneTypes: a milestone QueryType</param>
    /// <param name="withActivities">Boolean: true, if the list is selected with activities</param>
    /// <returns>List of EvMilestone: a list of milestone objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving a list of milestone objects 
    /// based on ProjectId, Type and withActivities condition
    /// 
    /// 2. Return a list of milestone objects.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvMilestone> getMilestoneList (
      String ProjectId,
      int ScheduleId,
      EvMilestone.MilestoneTypes Type,
      bool withActivities )
    {
      this.LogMethod ( "Evado.Bll.Clinical.EvMilestones.getPreparationList method." );
      this.LogValue ( "ProjectId: " + ProjectId );
      this.LogValue ( "Type: " + Type );

      // 
      // Execute the selectionList method.
      // 
      List<EvMilestone> view = this._DAL_Milestones.getMilestoneList ( ProjectId, ScheduleId, Type, withActivities );
      this.LogValue ( this._DAL_Milestones.Log );

      return view;

    }// End getPreparationList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of Milestone objects based on the passed parameters. 
    /// </summary>
    /// <param name="ScheduleGuid">Guid: a global unique identifier</param>
    /// <param name="Type">EvMilestone.MilestoneTypes: a milestone QueryType</param>
    /// <param name="usingMilestoneId">Boolean: true, if the list is selected by MilestoneId</param>
    /// <param name="isSelectionList">Boolean: true, if the selection list exists</param>
    /// <returns>List of EvMilestone: a list of options for milestone objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving a list of options for milestone objects
    /// 
    /// 2. Return a list of options for milestone objects.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> getMilestoneList (
      Guid ScheduleGuid,
      EvMilestone.MilestoneTypes Type,
      bool usingMilestoneId,
      bool isSelectionList )
    {
      this.LogMethod ( "Evado.Bll.Clinical.EvMilestones.getPreparationList method." );
      this.LogValue ( "ScheduldGuid: " + ScheduleGuid );
      this.LogValue ( "Type: " + Type );
      this.LogValue ( "usingMilestoneId: " + usingMilestoneId );
      this.LogValue ( "isSelectionList: " + isSelectionList );

      // 
      // Execute the selectionList method.
      // 
      List<EvOption> list = this._DAL_Milestones.getMilestoneSelectionList (
        ScheduleGuid,
        Type,
        usingMilestoneId );
      this.LogValue ( this._DAL_Milestones.Log );

      return list;

    }//END getPreparationList method.

    #endregion

    #region Class Operational Views and Lists.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of Milestone objects based on ProjectId, ArmIndex, Types and OrderBy
    /// </summary>
    /// <param name="ProjectId">string: (Mandatory) Project identifier</param>
    /// <param name="ScheduleId">EvTrialArm.ArmIndexes: (Mandatory) The Arm Index</param>
    /// <param name="MilestoneTypeList">List of EvMilestone.MilestoneTypes: Milestone types</param>
    /// <returns>List of EvMilestone: a list of milestone objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving a list of Milestone objects 
    /// 
    /// 2. Return a list of Milestone objects
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvMilestone> getIssuedScheduleMilestoneList (
      String ProjectId,
      int ScheduleId,
      List<EvMilestone.MilestoneTypes> MilestoneTypeList )
    {
      this.LogMethod ( "Evado.Bll.Clinical.EvMilestones.getView method." );
      this.LogValue ( "ProjectId: " + ProjectId );
      this.LogValue ( "ScheduleId: " + ScheduleId );
      this.LogValue ( "MilestoneType count: " + MilestoneTypeList.Count );

      // 
      // Execute the selectionList method.
      // 
      List<EvMilestone> view = this._DAL_Milestones.getIssuedScheduleMilestoneList ( ProjectId, ScheduleId, MilestoneTypeList, false );
      this.LogValue ( this._DAL_Milestones.Log );

      return view;

    }//END getMilestoneList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of Options for Milestone objects based on ProjectId, ArmIndex and Types
    /// </summary>
    /// <param name="ProjectId">string: (Mandatory) Project identifier</param>
    /// <param name="ArmIndex">EvTrialArm.ArmIndexes: (Mandatory) The Arm Index</param>
    /// <param name="Types">List of EvMilestone.MilestoneTypes: Milestone types</param>
    /// <param name="ScheduleState">EvSchedule.ScheduleStates enumeration value</param>
    /// <returns>List of EvMilestone: a list of Options for milestone objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving a list of Options for Milestone objects 
    /// 
    /// 2. Return a list of Milestone objects
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> getOptionList (
      string ProjectId,
      int ArmIndex,
      List<EvMilestone.MilestoneTypes> Types,
      EvSchedule.ScheduleStates ScheduleState )
    {
      this.LogMethod ( "Evado.Bll.Clinical.EvMilestones.getList method. " );
      this.LogValue ( "ProjectId: " + ProjectId );
      this.LogValue ( "ArmIndex: " + ArmIndex );
      this.LogValue ( "Types count: " + Types.Count );

      // 
      // Execute the selectionList method.
      // 
      List<EvOption> list = this._DAL_Milestones.getOptionList ( ProjectId, ArmIndex, Types, ScheduleState );
      this.LogValue ( this._DAL_Milestones.Log );

      return list;

    }// End getList method.

    #endregion

    #region Class Retrievals

    // =====================================================================================
    /// <summary>
    /// This class retrieves Milestone object based on Guid and Optional condition
    /// </summary>
    /// <param name="MilestoneGuid">string: a milestone global unique identifier</param>
    /// <returns>EvMilestone: a Milestone object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the Milestone object based on Guid and Operational condition
    /// 
    /// 2. Return a Milestone object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvMilestone getMilestone ( Guid MilestoneGuid )
    {
      this.LogMethod ( "Evado.Bll.Clinical.EvMilestones.getMilestone method." );
      this.LogValue ( "MilestoneGuid: " + MilestoneGuid );

      EvMilestone milestone = new EvMilestone ( );

      // 
      // Retrieve the user object from the database.
      // 
      milestone = this._DAL_Milestones.getMilestone ( MilestoneGuid );
      this.LogValue ( this._DAL_Milestones.Log );

      return milestone;

    }//END getMilestone method

    // =====================================================================================
    /// <summary>
    /// This class retrieves Milestone object based on ProjectId, ArmIndex and MilestoneId
    /// </summary>
    /// <param name="ProjectId">String: a Project identifier</param>
    /// <param name="ScheduledId">int: an arm index</param>
    /// <param name="MilestoneId">String: a Milestone identifier</param>
    /// <returns>EvMilestone: a Milestone object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the Milestone object based on ProjectId, ArmIndex and MilestoneId
    /// 
    /// 2. Return a Milestone object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvMilestone getMilestone (
      String ProjectId,
      int ScheduledId,
      String MilestoneId )
    {
      this.LogMethod ( "Evado.Bll.Clinical.EvMilestones.getMilestone method." );
      this.LogValue ( "ProjectId: " + ProjectId );
      this.LogValue ( "ArmIndex: " + ScheduledId );
      this.LogValue ( "MilestoneId: " + MilestoneId );

      EvMilestone milestone = this._DAL_Milestones.getMilestone ( ProjectId, ScheduledId, MilestoneId );
      this.LogValue ( this._DAL_Milestones.Log );
      return milestone;

    }//END getMilestone method

    #endregion

    #region Class Update methods

    // =====================================================================================
    /// <summary>
    /// This class processes the save items for Milestone table. 
    /// </summary>
    /// <param name="Milestone">EvMilestone: a milestone object</param>
    /// <returns>EvEventCodes: an event code for processing save items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the VisitId or MilestoneId or Guid or UserCommonName or ArmIndex is empty. 
    /// 
    /// 2. Execute the method for deleting items, if the action code is delete.
    /// 
    /// 3. Execute the method for adding items, if the Uid is empty. 
    /// 
    /// 4. Else, execute the method for updating items. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes saveItem ( EvMilestone Milestone )
    {
      this.LogMethod ( "Evado.Bll.Clinical.EvMilestones.Saving Milestone method. " );
      // 
      // Instantiate the local variables
      //
      EvEventCodes iReturn = 0;

      // 
      // Exit, if the VisitId or MilestoneId or Guid is empty. 
      // 
      if ( Milestone.ProjectId == String.Empty )
      {
        this.LogValue ( " >>  No ProjectId" );
        return EvEventCodes.Identifier_Project_Id_Error;
      }

      if ( Milestone.MilestoneId == String.Empty )
      {
        this.LogValue ( " >>  No MilestoneId" );
        return EvEventCodes.Identifier_Milestone_Id_Error;
      }

      if ( Milestone.ScheduleGuid == Guid.Empty )
      {
        this.LogValue ( " >> No Schedule Guid" );
        return EvEventCodes.Identifier_Schedule_Identifier_Error;
      }

      if ( Milestone.UserCommonName == String.Empty )
      {
        this.LogValue ( " >> No UserId" );
        return EvEventCodes.Identifier_User_Id_Error;
      }

      // 
      // If MilestoneId null then set to superseded
      // 
      if ( Milestone.Title == String.Empty
        || Milestone.Action == EvMilestone.MilestoneSaveActions.Delete )
      {
        this.LogValue ( "Delete milestone" );
        iReturn = this._DAL_Milestones.deleteMilestone ( Milestone );

        this.LogValue ( this._DAL_Milestones.Log );
        this.LogValue ( "iReturn: " + iReturn.ToString ( ) );

        return iReturn;
      }

      // 
      // Add a new trial Report to the database
      // 
      if ( Milestone.Guid == Guid.Empty )
      {
        this.LogValue ( "Add Milestone" );
        iReturn = this._DAL_Milestones.addMilestone ( Milestone );

        this.LogValue ( this._DAL_Milestones.Log );

        this.LogValue ( "iReturn: " + iReturn.ToString ( ) );

        return iReturn;
      }

      // 
      // If there is NO Xml content then update the trial Report properties.
      // 
      this.LogValue ( "Update Milestone" );
      iReturn = this._DAL_Milestones.updateMilestone ( Milestone );
      this.LogValue ( this._DAL_Milestones.Log );

      return iReturn;

    }//END saveItem method 

    // =====================================================================================
    /// <summary>
    /// This class deletes items from Milestone ResultData table. 
    /// </summary>
    /// <param name="UserProfile">Evado.Model.Digital.EvUserProfile: a User profile object</param>
    /// <param name="ScheduleGuid">Guid: a Schedule global unique identifier</param>
    /// <returns>EvEventCode: an event code for deleting items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving a list of Milestone objects. 
    /// 
    /// 2. Loop through the list of Milestone objects and execute the method for deleting items from Milestone table. 
    /// 
    /// 3. Return an event code for deleting items. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes deleteMilestones (
      Evado.Model.Digital.EvUserProfile UserProfile,
      Guid ScheduleGuid )
    {
      this.LogMethod ( "Evado.Bll.Clinical.EvMilestones.deleteMilestones method. " );
      this.LogValue ( "ScheduleGuid: " + ScheduleGuid );
      // 
      // Initialise the methods variables and objects.
      // 
      EvEventCodes iReturn = 0;

      // 
      // Execute the selectionList method.
      // 
      List<EvMilestone> view = this._DAL_Milestones.getMilestoneList (
        ScheduleGuid,
        EvMilestone.MilestoneTypes.Null );
      this.LogValue ( this._DAL_Milestones.Log );

      // 
      // Iterate through the milestones deleting each of them.
      // 
      foreach ( EvMilestone milestone in view )
      {
        milestone.UpdatedByUserId = UserProfile.UpdatedByUserId;
        milestone.UserCommonName = UserProfile.CommonName;

        iReturn = this._DAL_Milestones.deleteMilestone ( milestone );

        this.LogValue ( this._DAL_Milestones.Log );
      }

      return iReturn;

    }//END deleteMilestones class

    #endregion

  }//END EvMilestones Class.

}//END namespace Evado.Bll.Clinical
