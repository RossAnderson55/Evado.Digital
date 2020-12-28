// ***********************************************************************
// Assembly         : EvadoAdminUtil.ActiveDirectory
// Author           : Hanmoi
// Created          : 02-18-2013
//
// Last Modified By : Hanmoi
// Last Modified On : 02-18-2013
// ***********************************************************************
// <copyright file="EvAdConfig.cs" company="Evado Pty Ltd">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.DirectoryServices.AccountManagement;

namespace Evado.ActiveDirectoryServices
{
    /// <summary>
    /// Class EvAdConfig
    /// </summary>
    public class EvAdConfig
    {
        /// <summary>
        /// Gets or sets the type of the context.
        /// </summary>
        /// <value>The type of the context.</value>
        public ContextType ContextType { get; set; }

        /// <summary>
        /// Gets or sets the server.
        /// </summary>
        /// <value>The server.</value>
        public string Server { get; set; }

        /// <summary>
        /// Gets or sets the root container.
        /// </summary>
        /// <value>The root container.</value>
        public string RootContainer { get; set; }

        /// <summary>
        /// Gets or sets the users container.
        /// </summary>
        /// <value>The users container.</value>
        public string UsersContainer { get; set; }

        /// <summary>
        /// Gets or sets the groups container.
        /// </summary>
        /// <value>The groups container.</value>
        public string GroupsContainer { get; set; }

        public string AdminName { get; set; }

        public string AdminPassword { get; set; }
    }
}