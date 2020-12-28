/***************************************************************************************
 * <copyright file="EvAmCallResult.cs company="EVADO HOLDING PTY. LTD.">
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
namespace Evado.ActiveDirectoryServices
{
  //  ==================================================================================
  /// <summary>
  /// Enum EvAmCallResult
  /// Use to notify the result of calling APIs of IEvAccountManageable
  /// </summary>
  /// <see cref="IEvAccountManageable"/>
  // -------------------------------------------------------------------------------------
  public enum EvAdsCallResult
  {
    /// <summary>
    /// Note set
    /// </summary
    Null,
    /// <summary>
    /// Finished Successfully.
    /// </summary>
    Success,

    /// <summary>
    /// The value of arguments is invalid
    /// </summary>
    Invalid_Argument,

    /// <summary>
    /// There was either no User or no Group matched Which was requested.
    /// </summary>
    Object_Not_Found,

    /// <summary>
    /// The password is not matched.
    /// </summary>
    Invalid_Credentials,

    /// <summary>
    /// Error occurred in the operation with ADS
    /// </summary>
    Operation_Failure_In_ADS
  }
}