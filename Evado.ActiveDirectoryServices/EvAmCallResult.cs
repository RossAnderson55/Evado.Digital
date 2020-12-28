// ***********************************************************************
// Assembly         : EvadoAdminUtil.ActiveDirectory
// Author           : Hanmoi
// Created          : 02-14-2013
//
// Last Modified By : Hanmoi
// Last Modified On : 02-15-2013
// ***********************************************************************
// <copyright file="EvAmCallResult.cs" company="Evado Pty Ltd">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Evado.ActiveDirectoryServices
{
    /// <summary>
    /// Enum EvAmCallResult
    /// Use to notify the result of calling APIs of IEvAccountManageable
    /// </summary>
    /// <see cref="IEvAccountManageable"/>
    public enum EvAmCallResult
    {
        /// <summary>
        /// Finished Successfully.
        /// </summary>
        Success,
        /// <summary>
        /// The value of arguments is invalid
        /// </summary>
        InvalidArgument,
        /// <summary>
        /// There was either no User or no Group matched Which was requested.
        /// </summary>
        ObjectNotFound,
        /// <summary>
        /// The password is not matched.
        /// </summary>
        InvalidCredential
    }
}