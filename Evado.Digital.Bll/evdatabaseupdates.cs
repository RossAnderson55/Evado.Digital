/***************************************************************************************
 * <copyright file="BLL\EvDatabaseUpdates.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvDatabaseUpdates business object.
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


namespace Evado.Digital.Bll
{
  /// <summary>
  /// This business object manages the EvTrialMilestones in the system.
  /// </summary>
  public class EvDataBaseUpdates : EvBllBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EvDataBaseUpdates ( )
    {
      this.ClassNameSpace = "Evado.Digital.Bll.Clinical.EvDataBaseUpdates.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EvDataBaseUpdates ( Evado.Digital.Model.EvClassParameters Settings )
    {
      this.ClassParameter = Settings;
      this.ClassNameSpace = "Evado.Digital.Bll.Clinical.EvDataBaseUpdates.";

      if ( this.ClassParameter.LoggingLevel == 0 )
      {
        this.ClassParameter.LoggingLevel = Evado.Digital.Dal.EvStaticSetting.LoggingLevel;
      }

      this._DalDatabaseUpdates = new Evado.Digital.Dal.EvDataBaseUpdates ( Settings );
    }
    #endregion

    #region Class initialization and property.
    //
    //  Create instantiate the DAL class 
    // 
    private Evado.Digital.Dal.EvDataBaseUpdates _DalDatabaseUpdates = new Evado.Digital.Dal.EvDataBaseUpdates();

    #endregion

    // =====================================================================================
    /// <summary>
    /// This method gets an ArrayList contain the DatabaseUpdates objects 
    /// </summary>
    /// <param name="Version">EvDataBaseUpdate.UpdateVersionList: A sorting order of the list</param>
    /// <param name="OrderBy">EvDataBaseUpdate.DataBaseUpdateOrderBy: A sorting order of the list</param>
    /// <returns>List of EvDataBaseUpdate: a list of database update objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving a selection list of Application event objects. 
    /// 
    /// 2. Return the selection list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvDataBaseUpdate> getUpdateList(
      EvDataBaseUpdate.UpdateVersionList Version, 
      EvDataBaseUpdate.UpdateOrderBy OrderBy )
    {
      this.LogMethod ( "getUpdateList method. " );
      // 
      // Execute the selectionList method.
      // 
      List<EvDataBaseUpdate> view = this._DalDatabaseUpdates.getUpdateList ( Version, OrderBy );
      this.LogClass( this._DalDatabaseUpdates.Log );

      this.LogMethodEnd ( "getUpdateList" );

      return view;

    }//End getMilestoneList method.

  }//END EvDataBaseUpdates Class.

}//END namespace Evado.BLL 
