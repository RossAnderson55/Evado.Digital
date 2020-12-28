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


namespace Evado.Bll.Clinical
{
  /// <summary>
  /// A business Component used to manage user roles
  /// The m_xfs.Model.Process is used in most methods 
  /// and is used to store serializable information about an account
  /// </summary>
  public class EdPlatforms : EvBllBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EdPlatforms ( )
    {
      this.ClassNameSpace = "Evado.Bll.Clinical.EdPlatformSettings.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EdPlatforms ( EvClassParameters Settings )
    {
      this.ClassParameter = Settings;
      this.ClassNameSpace = "Evado.Bll.Clinical.EdPlatformSettings.";

      if ( this.ClassParameter.LoggingLevel == 0 )
      {
        this.ClassParameter.LoggingLevel = Evado.Dal.EvStaticSetting.LoggingLevel;
      }

      this._Dll_ApplicationSettings = new Evado.Dal.Clinical.EdPlatforms ( Settings );
    }
    #endregion

    #region Class variable and properties
    // 
    // Create instantiate the DAL class containing the datbase access functions for the class.
    // 
    private Evado.Dal.Clinical.EdPlatforms _Dll_ApplicationSettings = new Evado.Dal.Clinical.EdPlatforms();

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
    public Evado.Model.Digital.EdPlatform getItem ( string ApplicationId )
    {
      this.LogMethod ( "getItem Method." );
      this.LogDebug ( "ApplicationId: " + ApplicationId );

      Evado.Model.Digital.EdPlatform platformSettings = this._Dll_ApplicationSettings.getItem ( ApplicationId );

      this.LogDebugClass ( this._Dll_ApplicationSettings.Log );

      this.LogMethodEnd ( "getItem" );
      return platformSettings;

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
    public EvEventCodes updateItem ( Evado.Model.Digital.EdPlatform Properties )
    {
      this.LogMethod ( "Saving Record method" );
      EvEventCodes iReturn = EvEventCodes.Ok;
      Properties.UpdateLog = "Updated by: " + Properties.UserCommonName
       + " at " + DateTime.Now.ToString("dd MMM yyyy HH:mm")
       + "\r\n" + Properties.UpdateLog;

      //
      // Update the _SiteProperties record.
      // 
      iReturn = this._Dll_ApplicationSettings.updateItem ( Properties );

      this.LogDebugClass ( this._Dll_ApplicationSettings.Log );

      return iReturn;

    }//END updateItem class
    #endregion

  }//END EvSiteProfiles Class.

}//END namespace Evado.Bll.Clinical
