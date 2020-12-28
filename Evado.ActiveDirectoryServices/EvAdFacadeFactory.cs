// ***********************************************************************
// Assembly         : EvadoAdminUtil.ActiveDirectory
// Author           : Hanmoi
// Created          : 02-18-2013
//
// Last Modified By : Hanmoi
// Last Modified On : 02-18-2013
// ***********************************************************************
// <copyright file="EvAdFacadeFactory.cs" company="Evado Pty Ltd">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.DirectoryServices.AccountManagement;

namespace Evado.ActiveDirectoryServices
{
    /// <summary>
    /// Class EvAdFacadeFactory
    /// </summary>
    public class EvAdFacadeFactory
    {
        public static EvAdFacade CreateFacade()
        {
            var config = new EvAdConfig()
            {
                Server = "192.168.10.53",
                ContextType = ContextType.Domain,
                RootContainer = "OU=Test Users,DC=evado,DC=local",
                UsersContainer = "OU=Test Users,DC=evado,DC=local",
                GroupsContainer = "OU=Test Users,DC=evado,DC=local",
                AdminName = @"evado",
                AdminPassword = "Invision!1"
            };

            return CreateFacade(config);
        }
        /// <summary>
        /// Creates the Evado Active Directory facade Object.
        /// </summary>
        /// <param name="config">Configuration value to setup active directory connection up</param>
        /// <returns>EvAdFacade</returns>
        /// <exception cref="System.ArgumentException">If EvAdConfig value is improper.</exception>
        /// <example>
        /// <code>
        ///  var config = new EvAdConfig()
        ///        {
        ///            ContextType = ContextType.ApplicationDirectory,
        ///            Server = "localhost:389",
        ///            RootContainer = "dc=evado",
        ///            UsersContainer = "CN=Users,DC=evado",
        ///            GroupsContainer = "CN=Roles,DC=evado"
        ///        };
        ///   
        ///    var adFacade = EvAdFacadeFactory.CreateFacade(config);
        /// </code>
        /// </example>
        public static EvAdFacade CreateFacade(EvAdConfig config)
        {
            config.ContextType = ContextType.Domain;

            if (ConfigParamValidator(config) == false)
            {
                throw new ArgumentException();
            }

            return new EvAdFacade(config);
        }

        /// <summary>
        /// Validate the config value.
        /// </summary>
        /// <param name="config">Configuration value to setup active directory connection up</param>
        /// <returns><c>true</c> if any of Config Property is improper, 
        /// <c>false</c> otherwise</returns>
        private static bool ConfigParamValidator(EvAdConfig config)
        {
            var validation = String.IsNullOrEmpty(config.Server) != true
                             && String.IsNullOrEmpty(config.RootContainer) != true
                             && String.IsNullOrEmpty(config.GroupsContainer) != true
                             && String.IsNullOrEmpty(config.UsersContainer) != true
                             && String.IsNullOrEmpty(config.AdminName) != true
                             && String.IsNullOrEmpty(config.AdminPassword) != true;

            return validation;
        }
    }
}