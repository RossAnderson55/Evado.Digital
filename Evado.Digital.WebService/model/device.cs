/***************************************************************************************
 * <copyright file="Evado.Digital.WebService\Model\Device.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2001 - 2012 EVADO HOLDING PTY. LTD..  All rights reserved.
 *     
 *      The use and distribution terms for this software are contained in the file
 *      named \license.txt, which can be found in the root of this distribution.
 *      By using this software in any fashion, you are agreeing to be bound by the
 *      terms of this license.
 *     
 *      You must not remove this notice, or any other, from this software.
 *     
 * </copyright>
 * 
 * Description: 
 *  This class contains the EvUserRegistration data object.
 *
 ****************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

namespace Evado.Digital.WebService.Model
{
  /// <summary>
  /// Business entity used to model accounts
  /// </summary>
  [Serializable]
  public class Device
  {

    #region Class PropertyList

    private Guid _Identifier = Guid.Empty;

    /// <summary>
    ///  This property contains a Guid identifier for the Device object.
    /// </summary>
    public Guid Identifier
    {
      get { return this._Identifier; }
      set { this._Identifier = value; }
    }

    private string _UserId = String.Empty;

    /// <summary>
    /// This Property contains a User Id of a user.
    /// </summary>
    public string UserId
    {
      get { return this._UserId; }
      set { this._UserId = value; }
    }

    /// <summary>
    /// This proprty contains a service Id. 
    /// </summary>
    private string _ServiceId = String.Empty;

    public string ServiceId
    {
      get { return this._ServiceId; }
      set { this._ServiceId = value; }
    }

    private string _DeviceId = String.Empty;

    /// <summary>
    /// This property contains device Id for a device.
    /// </summary>
    public string DeviceId
    {
      get { return this._DeviceId; }
      set { this._DeviceId = value; }
    }

    private string _DeviceName = String.Empty;

    /// <summary>
    /// This property contains Device Name for a device.
    /// </summary>
    public string DeviceName
    {
      get { return this._DeviceName; }
      set { this._DeviceName = value; }
    }

    private string _DeviceOs = String.Empty;

    /// <summary>
    /// This property contains device Os for a device.
    /// </summary>
    public string DeviceOs
    {
      get { return this._DeviceOs; }
      set { this._DeviceOs = value; }
    }

    private DateTime _RegistrationDate = Evado.Model.EvStatics.CONST_DATE_NULL;

    /// <summary>
    /// This property contains Registration Date of a device. 
    /// </summary>
    public DateTime RegistrationDate
    {
      get { return this._RegistrationDate; }
      set { this._RegistrationDate = value; }
    }

    #endregion

  } // Close class User

} // Close namespace Evado.Model