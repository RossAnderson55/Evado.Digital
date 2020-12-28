/***************************************************************************************
 * <copyright file="EvPrincipalComparer.cs company="EVADO HOLDING PTY. LTD.">
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices.AccountManagement;

namespace Evado.ActiveDirectoryServices
{
    //  ==================================================================================
    /// <summary>
    /// Class EvPrincipalComparer
    /// Principal object comparer by GUID.
    /// </summary>
    //  ---------------------------------------------------------------------------------- 
    class EvPrincipalComparer : IEqualityComparer<Principal>
    {
        public bool Equals(Principal x, Principal y)
        {
            return x.Guid == y.Guid;
        }

        public int GetHashCode(Principal obj)
        {
            return obj.GetHashCode();
        }
    }
}
