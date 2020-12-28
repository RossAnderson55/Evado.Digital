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

//Evado. namespace references.
using Evado.Model;
using Evado.Dal;
using Evado.Model.Digital;

namespace Evado.Bll.Clinical
{
  /// <summary>
  /// This business object manages the EvActvityFormss in the system.
  /// </summary>
  public class EvActivityForms : EvBllBase
  {
    #region Class Initialization

    // ==================================================================================
    /// <summary>
    /// This is the class initialisation method.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EvActivityForms ( )
    {
      this.ClassNameSpace = "Evado.Bll.eClinical.EvActivityForms.";
    }

    // ==================================================================================
    /// <summary>
    /// This is the class initialisation method with settings configured.
    /// </summary>
    /// <param name="ClassParameter">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EvActivityForms ( EvClassParameters ClassParameter )
    {
      this.ClassParameter = ClassParameter;

      this._DalActivityForms = new Evado.Dal.Clinical.EvActivityForms ( this.ClassParameter );

      this.ClassNameSpace = "Evado.Bll.eClinical.EvActivityForms.";
    }
    #endregion

    #region Class Enumeration
    #endregion

    #region Class variables and properties.
    //
    //  Create instantiate the DAL class 
    // 
    private Evado.Dal.Clinical.EvActivityForms _DalActivityForms = new Evado.Dal.Clinical.EvActivityForms ( );

    #endregion

    #region Class Methods.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of Activity Record object based on Activity's Guid
    /// </summary>
    /// <param name="ActivityGuid">Guid: the Activity Guid</param>
    /// <returns>List of EvActivityForm: A list of Activity record objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving selection Activities Records list. 
    /// 
    /// 2. Return the selection list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvActvityForm> getList ( Guid ActivityGuid )
    {
      this.LogMethod ( "getList method.  " );
      this.LogValue ( "ActivityGuid: " + ActivityGuid );

      // 
      // Execute the selectionList method.
      // 
      List<EvActvityForm> view = this._DalActivityForms.getFormList ( ActivityGuid );
      this.LogClass ( this._DalActivityForms.Log );

      return view;

    }//End getMilestoneList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of Activity Record object based on Milestone's Guid
    /// </summary>
    /// <param name="MilestoneGuid">Guid: the Milestone Guid</param>
    /// <returns>List of EvActivityForm: A list of Activity record objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving selection Activities Records list. 
    /// 
    /// 2. Return the selection list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvActvityForm> getMilestoneList ( Guid MilestoneGuid )
    {
      this.LogMethod ( "getMilestoneList method.  " );
      this.LogValue ( "MilsteoneGuid: " + MilestoneGuid );

      // 
      // Execute the selectionList method.
      // 
      List<EvActvityForm> view = this._DalActivityForms.getMilestoneView ( MilestoneGuid );
      this.LogClass ( this._DalActivityForms.Log );

      return view;

    }//End getMilestoneView method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of Activity Record object based on VisitId and Activity's Guid
    /// </summary>
    /// <param name="TrialId">string: a trial identifier</param>
    /// <param name="ActivityGuid">Guid: An Activity's Guid</param>
    /// <returns>List of EvActivityForm: A list of Activity record objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving selection Activities Records list. 
    /// 
    /// 2. Return the selection list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvActvityForm> getSelectionList ( String TrialId, Guid ActivityGuid )
    {
      this.LogMethod ( "getSelectionList method." );
      this.LogValue ( "TrialId: " + TrialId );
      this.LogValue ( "ActivityGuid: " + ActivityGuid );
      this.LogValue ( "DalActivityForms.Settings.LoggingLevel: " + this._DalActivityForms.ClassParameters.LoggingLevel );

      // 
      // Execute the selectionList method.
      // 
      List<EvActvityForm> list = this._DalActivityForms.getSelectionView ( TrialId, ActivityGuid );
      this.LogClass ( this._DalActivityForms.Log );

      return list;

    }//End getSelectionList method.

    #endregion

  }//END EvActvityFormss Class.

}//END namespace Evado.Evado.BLL 
