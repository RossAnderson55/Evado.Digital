/***************************************************************************************
 * <copyright file="BLL\EvActivities.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvActivities business object.
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
  /// This business object manages the EvTrialMilestones in the system.
  /// </summary>
  public class EvActivities : EvBllBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EvActivities ( )
    {
      this.ClassNameSpace = "Evado.Bll.Clinical.EvActivities.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EvActivities ( EvClassParameters Settings )
    {
      this.ClassParameter = Settings;
      this.ClassNameSpace = "Evado.Bll.Clinical.EvActivities.";

      if ( this.ClassParameter.LoggingLevel == 0 )
      {
        this.ClassParameter.LoggingLevel = Evado.Dal.EvStaticSetting.LoggingLevel;
      }

      this._DalActivities = new Evado.Dal.Clinical.EvActivities ( Settings );
    }
    #endregion

    #region Class variables and property
    //
    //  Create instantiate the DAL class 
    // 
    private Evado.Dal.Clinical.EvActivities _DalActivities = new Evado.Dal.Clinical.EvActivities( );
    #endregion

    #region Class methods
    // =====================================================================================
    /// <summary>
    /// This class returns a list of Activity object based on VisitId, Types and withForms condition
    /// </summary>
    /// <param name="ProjectId">string: a trial identifier</param>
    /// <param name="Type">List of EvActivity.ActivityTypes: A list of Activity Types</param>
    /// <param name="WithForms">Boolean: true, if the Activities have forms</param>
    /// <returns>List of EvActivity: A list containing Activity objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the selection list of Actvity ResultData objects. 
    /// 
    /// 2. Return the selection list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvActivity> getActivityList ( 
      String ProjectId, 
      EvActivity.ActivityTypes Type, 
      bool WithForms )
    {
      this.LogMethod ( "getActivityList method. " );
      this.LogDebug( "ProjectId: " + ProjectId );
      this.LogDebug(  "Type: " + Type );
      this.LogDebug(  "WithForms: " + WithForms );
      // 
      // Execute the selectionList method.
      // 
      List<EvActivity> View = this._DalActivities.getActivityList ( ProjectId, Type, WithForms );

      this.LogClass(  this._DalActivities.Log );

      return View;

    }//End getMilestoneList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of Option object based on VisitId
    /// </summary>
    /// <param name="ProjectId">string: a trial identifier</param>
    /// <returns>List of EvOption: A list containing option objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the selection list of Option ResultData objects. 
    /// 
    /// 2. Return the selection list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> getOptionList ( string ProjectId )
    {
      this.LogMethod ( "getOptionList method. " );
      this.LogDebug( "ProjectId: " + ProjectId );
      // 
      // Execute the selectionList method.
      // 
      List<EvOption> list = this._DalActivities.getOptionList( ProjectId );

      this.LogClass(  this._DalActivities.Log );

      return list;

    }//End getList method.

    // =====================================================================================
    /// <summary>
    /// This class retrieves the Activity object based on Guid
    /// </summary>
    /// <param name="Guid">Guid: The Activity's Global unique identifier</param>
    /// <returns>EvActivity: The activity object</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Execute the method for retrieving Activity object based on Guid. 
    /// 
    /// 2. Return the Activity object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvActivity getActivity( Guid Guid )
    {
      this.LogMethod ( "getActivity method. " );

      EvActivity activity = new EvActivity( );
      // 
      // Retrieve the user object from the database.
      // 
      activity = this._DalActivities.getActivity( Guid );

      this.LogClass(  this._DalActivities.Log );

      return activity;

    }//END getActivity method

    // =====================================================================================
    /// <summary>
    /// This class saves items on Activity table. 
    /// </summary>
    /// <param name="Activity">EvActivity: An Activity object</param>
    /// <returns>EvEventCodes: An event code for saving items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for deleting Activity object items, if the action is delete. 
    /// 
    /// 2. Execute the method for adding new Activity object, if the Guid is empty. 
    /// 
    /// 3. Else, execute the method for updating Activity object items. 
    /// 
    /// 4. Exit, if the execution runs fail. 
    /// 
    /// 5. Else, return an event code for saving items. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes saveActivity( EvActivity Activity )
    {
      this.LogMethod ( "saveActivity method" );
      this.LogDebug ( "Actvity:" + Activity.ActivityId );
      // 
      // Instantiate the local variables
      // 
       EvEventCodes iReturn = 0;

       // 
       // Validate whether the Activity identifier is not empty
       // 
       if ( Activity.ActivityId == String.Empty )
       {
         return EvEventCodes.Identifier_Activity_Id_Error;
       }

       //
       // Validate whether the activity trial identifier is not empty
       //
       if ( Activity.ProjectId == String.Empty )
       {
         return EvEventCodes.Identifier_Project_Id_Error;
       }

      // 
      // If MilestoneId null then set to superseded
      // 
      if ( Activity.Action == EvActivity.ActivitiesActionsCodes.Delete )
      {
       this.LogDebug(  " >> Delete Activity." );

        iReturn = this._DalActivities.deleteActivity( Activity );
        this.LogClass(  this._DalActivities.Log );

      this.LogDebug(  "iReturn: " + iReturn );

        return iReturn;
      }

      // 
      // Add a new trial Report to the database
      // 
      if ( Activity.Guid == Guid.Empty )
      {
        this.LogDebug(  " >> Add Activity" );

        iReturn = this._DalActivities.addActivity( Activity );

        this.LogClass(  this._DalActivities.Log );

      this.LogDebug(  "return: " + iReturn );
        return iReturn;
      }

      // 
      // If there is NO Xml content then update the trial Report properties.
      // 
      this.LogDebug(  " >> Update Activity" );

      iReturn = this._DalActivities.updateActivity( Activity );

      this.LogClass(  this._DalActivities.Log );
      return iReturn;

    }//END saveActivity method 
    #endregion

  }//END EvActivities Class.

}//END namespace Evado.Bll.Clinical 
