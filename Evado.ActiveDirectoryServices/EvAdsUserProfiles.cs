/***************************************************************************************
 * <copyright file="dal\EvFormFields.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2001 - 2012 EVADO HOLDING PTY. LTD..  All rights reserved.
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
using Evado.Model;

//Evado. namespace references.
//using Evado.ActiveDirectoryServices.Model;
//using Evado.Model;


namespace Evado.ActiveDirectoryServices
{
  /// <summary>
  /// A business Component used to manage trial roles
  /// The m_xfs.Model.User is used in most methods 
  /// and is used to store serializable information about an account
  /// </summary>
    public class EvAdsUserProfiles
  {
        private const string ADS_COnnection_String = "";
    /// 
    /// Define the object state property 
    /// 
    private string _Status = String.Empty;

    public string Status
    {
      get { return _Status; }
    }
    public string StatusHtml
    {
      get
      {
        return this._Status.Replace( "\r\n", "<br/>" );
      }
    }

    // =====================================================================================
    /// <summary>
    /// GetView method
    /// 
    /// Description:
    /// Get a ArrayList contain the trial roles 
    /// 
    /// </summary>
    /// <returns>DataTable containing newField orgs identifiers</returns>
    // -------------------------------------------------------------------------------------
    public List<EvUserProfile> GetView ( string OrgId, string OrderBy )
    {
      this._Status += "BLL:EvUserProfiles:GetView method OrgId: " + OrgId;

      return new List<EvUserProfile>();

    } //END GetView method.

    // =====================================================================================
    /// <summary>
    /// GetList method
    /// 
    /// Description:
    /// Get a ArrayList contain the trial roles 
    /// </summary>
    /// <returns>DataTable containing newField orgs identifiers</returns>
    //  -------------------------------------------------------------------------------------
    public List<EvOption> GetList ( string OrgId, bool useGuid )
    {
      this._Status += "BLL:EvUserProfiles:GetView method OrgId: " + OrgId;

      return new List<EvOption>();

    }//END GetList method

    // =====================================================================================
    /// <summary>
    /// GetItem method
    /// 
    /// Description:
    /// Get a user's profile  
    /// </summary>
    /// <returns>User</returns>
    // -------------------------------------------------------------------------------------
    public EvUserProfile getItem ( string UserId )
    {
      this._Status += "BLL:EvUserProfiles:GetItemById method UserId: " + UserId;

      /// 
      /// Return the user profile.
      /// 
      return  new EvUserProfile();

    }//END GetItem method

    // =====================================================================================
    /// <summary>
    /// getTrial method
    /// 
    /// Description:
    /// Get the information for an Organisation 
    /// </summary>
    /// <returns>DataTable containing newField orges identifiers</returns>
    // -------------------------------------------------------------------------------------
    public EvUserProfile getItem ( Guid SidGuid )
    {
        this._Status += "BLL:EvUserProfiles:getItemByUid method UserProfileGuid: " + SidGuid;

        /// 
        /// Return the user profile.
        /// 
        return new EvUserProfile();

    }//END getTrial method

    // =====================================================================================
    /// <summary>
    /// saveLetter method
    /// 
    /// Description:
    /// Updates the user profile in the database. 
    /// The update and add process are the same as in each execution the currentMonth objects are 
    /// set to superseded and then a new object is inserted to the database.
    /// 
    /// </summary>
    /// <param name="User">User object</param>
    // -------------------------------------------------------------------------------------
    public int saveItem ( EvUserProfile UserProfile )
    {
      /// 
      /// Initialise the methods variables and objects.
      /// 
      int iReturn = 0;

      /// 
      /// Check that the user id is valid
      /// 
		/*
      if ( UserProfile.UserId == String.Empty )
      {
        return EvEvent.IdErrors.UserId;
      }
      if ( UserProfile.UserCommonName == String.Empty )
      {
        return EvEvent.ApplicationErrors.UserId;
      }
		*/
      return iReturn;

    } //END saveItem method

    // ++++++++++++++++++++++++++++++++++  END OF SOURCE CODE +++++++++++++++++++++++++++++++++++++
  }//END Users

} //END namespace Evado.Bll
