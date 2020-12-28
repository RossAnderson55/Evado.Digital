/***************************************************************************************
 * <copyright file="EvAdFacadeFactory.cs company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2001 - 2013 EVADO HOLDING PTY. LTD.  All rights reserved.
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
using System.DirectoryServices.AccountManagement;

namespace Evado.ActiveDirectoryServices
{
  /// <summary>
  /// Class EvAdFacadeFactory
  /// </summary>
  public class EvAdsFacadeFactory
  {
    //  ==================================================================================
    /// <summary>
    /// Creates the Evado Active Directory facade Object.
    /// </summary>
    /// <param name="Config">Configuration value to setup active directory connection up</param>
    /// <returns>EvAdFacade</returns>
    /// <exception cref="System.ArgumentException">If EvAdConfig value is improper.</exception>
    /// <example>
    /// <code>
    /// EvAdConfig config = new EvAdConfig()
    ///		{
    ///			Server = "192.168.10.53",
    ///			ContextType = ContextType.Domain,
    ///		    RootContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    UsersContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    GroupsContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    AdminName = @"evado",
    ///		    AdminPassword = "#######",
    ///		    LdapString = "LDAP://192.168.10.53/dc=evado,dc=local"
    ///     };
    ///   
    /// EvAdsFacade adFacade = EvAdFacadeFactory.CreateFacade(config);
    /// </code>
    /// </example>
    // -------------------------------------------------------------------------------------
    public static EvAdsFacade CreateFacade ( EvAdsConfig Config )
    {
      Config.ContextType = ContextType.Domain;

      if ( ConfigParamValidator ( Config ) == false )
      {
        throw new ArgumentException ( "EvAdsConfig value is invalid. Some values are Null or Empty" );
      }

      return new EvAdsFacade ( Config );
    }

    //  ==================================================================================
    /// <summary>
    /// Validate the Config value.
    /// </summary>
    /// <param name="Config">Configuration value to setup active directory connection up</param>
    /// <returns><c>true</c> if any of Config Property is improper, 
    /// <c>false</c> otherwise</returns>
    // -------------------------------------------------------------------------------------
    private static bool ConfigParamValidator ( EvAdsConfig Config )
    {
      //
      // Check if string value is either Null or Empty
      //
      var validation = String.IsNullOrEmpty ( Config.Server ) != true
        && String.IsNullOrEmpty ( Config.RootContainer ) != true
        && String.IsNullOrEmpty ( Config.GroupsContainer ) != true
        && String.IsNullOrEmpty ( Config.UsersContainer ) != true
        && String.IsNullOrEmpty ( Config.AdminName ) != true
        && String.IsNullOrEmpty ( Config.AdminPassword ) != true
        && String.IsNullOrEmpty ( Config.LdapString ) != true;
      return validation;
    }
  }
}