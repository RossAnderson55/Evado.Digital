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
using System.Text;

//Evado. namespace references.
using Evado.Model;
using Evado.Dal;
using Evado.Model.Digital;


namespace Evado.Bll.Clinical
{
  /// <summary>
  /// This business object manages the EvTrialMilestones in the system.
  /// </summary>
  public class EvMilestoneActivities
  {
    #region Class  Variables and objects.
    //
    //  Create instantiate the DAL class 
    // 
    private Evado.Dal.Clinical.EvMilestoneActivities _DalMilestoneActivities = new Evado.Dal.Clinical.EvMilestoneActivities ( );

    // 
    // DebugLog stores the business object status conditions.
    // 
    private StringBuilder _DebugLog = new StringBuilder ( );

    /// <summary>
    /// This property contains the method debug log
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
        return DebugLog.Replace ( "\r\n", "<br/>" );
      }
    }

    #endregion

    #region Class views and queries

    // =====================================================================================
    /// <summary>
    /// This class returns a list of Activity objects based on Guid and QueryType.
    /// </summary>
    /// <param name="MilestoneGuid">Guid: (Mandatory) The Milestone global unique identifier</param>
    /// <param name="Types">List of EvActivity.ActivityTypes: The activity QueryType</param>
    /// <returns>List of EvActivity: a list of activity object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the list of activity objects based on Guid and QueryType.
    /// 
    /// 2. Return a list of activity objects. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvActivity> getPreparationView ( Guid MilestoneGuid, List<EvActivity.ActivityTypes> Types )
    {
      this._DebugLog.AppendLine ( "Evado.Bll.Clinical.EcMilestoneActivities.getPreparationView. " + " MilestoneGuid: " + MilestoneGuid
        + " Types: " + Types.Count );

      // 
      // Execute the selectionList method.
      // 
      List<EvActivity> View = this._DalMilestoneActivities.getActivityList ( MilestoneGuid, Types );
      this._DebugLog.AppendLine ( this._DalMilestoneActivities.DebugLog );

      return View;

    }//END getPreparationView method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of Activity objects based on Guid, QueryType and NotType condition.
    /// </summary>
    /// <param name="MilestoneGuid">Guid: (Mandatory) The Milestone global unique identifier</param>
    /// <param name="Type">List of EvActivity.ActivityTypes: The activity QueryType</param>
    /// <param name="NotType">Boolean: true, if the activity QueryType is not in a QueryType format</param>
    /// <returns>List of EvActivity: a list of activity object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the list of activity objects based on Guid, QueryType and NotType condition.
    /// 
    /// 2. Return a list of activity objects. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvActivity> getView (
      Guid MilestoneGuid,
      EvActivity.ActivityTypes Type,
      bool NotType )
    {
      this._DebugLog.AppendLine ( "Evado.Bll.Clinical.EcMilestoneActivities.getView. "
        + " MilestoneGuid: " + MilestoneGuid
        + " Type: " + Type.ToString ( )
        + " NotType: " + NotType );

      // 
      // Execute the selectionList method.
      // 
      List<EvActivity> View = this._DalMilestoneActivities.getActivityList ( MilestoneGuid, Type, NotType, false );
      this._DebugLog.AppendLine ( this._DalMilestoneActivities.DebugLog );

      return View;

    }//END getMilestoneList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of Activity objects based on Guid, QueryType, NotType and withForms condition.
    /// </summary>
    /// <param name="MilestoneGuid">Guid: (Mandatory) The Milestone global unique identifier</param>
    /// <param name="Type">List of EvActivity.ActivityTypes: The activity QueryType</param>
    /// <param name="NotType">Boolean: true, if the activity QueryType is not in a QueryType format</param>
    /// <param name="WithForms">Boolean: true, if the activity list is selected with forms</param>
    /// <returns>List of EvActivity: a list of activity object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the list of activity objects 
    /// based on Guid, QueryType, NotType and withForms condition.
    /// 
    /// 2. Return a list of activity objects. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvActivity> getView (
      Guid MilestoneGuid,
      EvActivity.ActivityTypes Type,
      bool NotType,
      bool WithForms )
    {
      this._DebugLog.AppendLine ( "Evado.Bll.Clinical.EcMilestoneActivities.getView. " );
      this._DebugLog.AppendLine ( "MilestoneGuid: " + MilestoneGuid );
      this._DebugLog.AppendLine ( "Type: " + Type );
      this._DebugLog.AppendLine ( "NotType: " + NotType );

      // 
      // Execute the selectionList method.
      // 
      List<EvActivity> View = this._DalMilestoneActivities.getActivityList ( MilestoneGuid, Type, NotType, WithForms );
      this._DebugLog.AppendLine ( this._DalMilestoneActivities.DebugLog );

      return View;

    }//END getMilestoneList method.

    #endregion

  }//END EvMilestoneActivities Class.

}//END namespace Evado.Bll.Clinical
