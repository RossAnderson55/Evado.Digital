/* <copyright file="BLL\EvSiteProfiles.cs" company="EVADO HOLDING PTY. LTD.">
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
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;

//Evado. namespace references.
using Evado.Model;
using Evado.Model.Digital;


namespace Evado.Bll.Digital
{
  /// <summary>
  /// A business Component used to manage user roles
  /// The m_xfs.Model.Process is used in most methods 
  /// and is used to store serializable information about an account
  /// </summary>
  public class EdAdapterConfig : EvBllBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EdAdapterConfig ( )
    {
      this.ClassNameSpace = "Evado.Bll.Clinical.EdAdapterSettings.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EdAdapterConfig ( EvClassParameters Settings )
    {
      this.ClassParameter = Settings;
      this.ClassNameSpace = "Evado.Bll.Clinical.EdAdapterSettings.";

      this._dll_AdapterSettings = new Evado.Dal.Digital.EdAdapterConfig ( Settings );
    }
    #endregion

    #region Class variable and properties
    // 
    // Create instantiate the DAL class containing the datbase access functions for the class.
    // 
    private Evado.Dal.Digital.EdAdapterConfig _dll_AdapterSettings = new Evado.Dal.Digital.EdAdapterConfig();

    #endregion 

    #region Class methods

    // =====================================================================================
    /// <summary>
    /// This class retrieves a Site Profile object based on SiteId
    /// </summary>
    /// <param name="ApplicationId">string: a site identifier</param>
    /// <returns>EvSiteProfile: a Site profile object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving a Site Properties object
    /// 
    /// 2. Return a Site Properties object
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public Evado.Model.Digital.EdAdapterSettings getItem ( string ApplicationId )
    {
      this.LogMethod ( "getItem Method." );
      this.LogDebug ( "ApplicationId: " + ApplicationId );

      Evado.Model.Digital.EdAdapterSettings adapterSettings = this._dll_AdapterSettings.getItem ( ApplicationId );

      this.LogDebugClass ( this._dll_AdapterSettings.Log );

      this.LogMethodEnd ( "getItem" );
      return adapterSettings;

    }//END getItemById class

    // =====================================================================================
    /// <summary>
    /// This class saves items to SiteProfile ResultData table. 
    /// </summary>
    /// <param name="Properties">EvSiteProfile: a site profile object</param>
    /// <returns>EvEventCodes: an event code for saving items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for updating items
    /// 
    /// 2. Return an event code for updating items. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes updateItem ( Evado.Model.Digital.EdAdapterSettings Properties )
    {
      this.LogMethod ( "Saving Record method" );
      EvEventCodes iReturn = EvEventCodes.Ok;

      //
      // Update the _SiteProperties record.
      // 
      iReturn = this._dll_AdapterSettings.updateItem ( Properties );

      this.LogDebugClass ( this._dll_AdapterSettings.Log );

      return iReturn;

    }//END updateItem class
    #endregion

  }//END EvSiteProfiles Class.

}//END namespace Evado.Bll.Digital
