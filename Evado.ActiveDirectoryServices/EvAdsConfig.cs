/***************************************************************************************
 * <copyright file="EvAdConfig.cs company="EVADO HOLDING PTY. LTD.">
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
using System.DirectoryServices.AccountManagement;

namespace Evado.ActiveDirectoryServices
{

    /// <summary>
    /// Class EvAdConfig
    /// </summary>
    public class EvAdsConfig
    {
        public ContextType ContextType { get; set; }
       
        public string Server { get; set; }

        public string RootContainer { get; set; }
      
        public string UsersContainer { get; set; }

        public string GroupsContainer { get; set; }

        public string AdminName { get; set; }

        public string AdminPassword { get; set; }

        public string LdapString { get; set; }
    }
}