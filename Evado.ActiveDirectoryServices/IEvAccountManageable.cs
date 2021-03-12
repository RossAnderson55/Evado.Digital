/***************************************************************************************
 * <copyright file="IEvAccountManageable.cs company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c)  2002 - 2021  EVADO HOLDING PTY. LTD.  All rights reserved.
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
using System.Collections.Generic;

namespace Evado.ActiveDirectoryServices
{
    //  ==================================================================================
    /// <summary>
    /// Interface IEvAccountManageable
    /// </summary>
    // -------------------------------------------------------------------------------------
    public interface IEvAccountManageable
    {
        #region With parameter in relation to EvAdsUserProfile

        //  ==================================================================================
        /// <summary>
        /// Creates the new Evado System User.
        /// </summary>
        /// <param name="EvUser"><see cref="EvAdsUserProfile"/></param>
        /// <returns>EvAdsCallResult</returns>
        // -------------------------------------------------------------------------------------
        EvAdsUserProfile CreateNewUser(EvAdsUserProfile EvUser);

        //  ==================================================================================
        /// <summary>
        /// Updates the information of Evado System User's information.
        /// </summary>
        /// <param name="EvUser"><see cref="EvAdsUserProfile"/></param>
        /// <returns>EvAdsCallResult.</returns>
        // -------------------------------------------------------------------------------------
        EvAdsUserProfile UpdateAdUser(EvAdsUserProfile EvUser);

        //  ==================================================================================
        /// <summary>
        /// Deletes the Evado System User from system.
        /// </summary>
        /// <param name="EvUser"><see cref="EvAdsUserProfile"/></param>
        /// <returns>EvAdsCallResult.</returns>
        // -------------------------------------------------------------------------------------
        EvAdsCallResult DeleteUserFromAd(EvAdsUserProfile EvUser);

        //  ==================================================================================
        /// <summary>
        /// Finds the Evado System User by id.
        /// </summary>
        /// <param name="EvUserId">string</param>
        /// <param name="EvUser"><see cref="EvAdsUserProfile"/></param>
        /// <returns>EvAdsCallResult.</returns>
        // -------------------------------------------------------------------------------------
        EvAdsCallResult FindAdUserById(string EvUserId, out EvAdsUserProfile EvUser);
       
        #endregion

        #region With parameter in relation to EvAdsGroupProfile
        //  ==================================================================================
        /// <summary>
        /// Creates the new Evado System group(Role) in persistence media.
        /// </summary>
        /// <param name="EvGroup"><see cref="EvAdsGroupProfile"/></param>
        /// <returns>EvAdsCallResult.</returns>
        // -------------------------------------------------------------------------------------
        EvAdsCallResult CreateNewGroup(EvAdsGroupProfile EvGroup);

        #endregion

        //  ==================================================================================
        /// <summary>
        /// Registers the Evado user to A Evado System group(Role).
        /// </summary>
        /// <param name="EvUser"><see cref="EvAdsUserProfile"/></param>
        /// <param name="EvGroup"<see cref="EvAdsGroupProfile"/></param>
        /// <returns>EvAdsCallResult.</returns>
        // -------------------------------------------------------------------------------------
        EvAdsCallResult RegisterUserToAGroup(EvAdsUserProfile EvUser, EvAdsGroupProfile EvGroup);

        //  ==================================================================================
        /// <summary>
        /// Registers the Evado user to multiple Evado System groups(Roles).
        /// </summary>
        /// <param name="EvAdsUserProfile"><see cref="EvAdsUserProfile"/></param>
        /// <param name="EvGroups">List&lt;EvAdsGroupProfile&gt;</param>
        /// <returns>EvAdsCallResult.</returns>
        // -------------------------------------------------------------------------------------
        EvAdsCallResult RegisterUserToMultipleGroups(EvAdsUserProfile EvAdsUserProfile, List<EvAdsGroupProfile> EvGroups);

        //  ==================================================================================
        /// <summary>
        /// Unregisterings the Evado user to a single Evado System group(Role).
        /// </summary>
        /// <param name="EvUser"><see cref="EvAdsUserProfile"/></param>
        /// <param name="EvGroup">The ad EvAdsGroupProfile.</param>
        /// <returns>EvAdsCallResult.</returns>
        // -------------------------------------------------------------------------------------
        EvAdsCallResult UnregisterUserFromSingleGroup(EvAdsUserProfile EvUser, EvAdsGroupProfile EvGroup);

        //  ==================================================================================
        /// <summary>
        /// Unregisterings the Evado user from multiple Evado System groups(Roles).
        /// </summary>
        /// <param name="EvUser"><see cref="EvAdsUserProfile"/></param>
        /// <param name="EvGroups">List&lt;EvAdsGroupProfile&gt;</param>
        /// <returns>EvAdsCallResult.</returns>
        EvAdsCallResult UnregisterUserFromMultipleGroups(EvAdsUserProfile EvUser, List<EvAdsGroupProfile> EvGroups);

        //  ==================================================================================
        /// <summary>
        /// Get All evado users with group property or not according to param
        /// </summary>
        /// <param name="WithGroup">bool</param>
        /// <returns>List&lt;EvAdsUserProfile&gt;</returns>
        // -------------------------------------------------------------------------------------
        List<EvAdsUserProfile> AllEvUsers(bool WithGroup);

        //  ==================================================================================
        /// <summary>
        /// Get All evado users without group
        /// And Filtered by Filter value
        /// </summary>
        /// <param name="Filter">string</param>
        /// <returns>List&lt;EvAdsUserProfile&gt;</returns>
        // -------------------------------------------------------------------------------------
        List<EvAdsUserProfile> AllEvUsersAfterBeingFiltered(string Filter);

        //  ==================================================================================
        /// <summary>
        /// Get All evado groups
        /// </summary>
        /// <returns>List&lt;EvAdsGroupProfile&gt;</returns>
        // -------------------------------------------------------------------------------------
        List<EvAdsGroupProfile> getAllGroups();
    }
}
