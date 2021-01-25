/***************************************************************************************
 * <copyright file="EvOption.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2020 EVADO HOLDING PTY. LTD..  All rights reserved.
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
 *  This class contains the EvOption data object.
 *
 ****************************************************************************************/

using System;
using Newtonsoft.Json;

namespace Evado.Model.Digital
{
  /// <summary>
  /// This class defines a selection list option.
  /// </summary>
  [Serializable]
  public class EdRole
  {
    #region Public methods

    // ==================================================================================
    /// <summary>
    /// Constructor with specified initial values
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EdRole ( )
    {
    }

    // ==================================================================================
    /// <summary>
    /// Constructor with specified initial values
    /// </summary>
    /// <param name="RoleId">string: the value of the option</param>
    /// <param name="Description">string: the description of the option</param>
    // ----------------------------------------------------------------------------------
    public EdRole ( object RoleId, string Description )
    {
      this._RoleId = RoleId.ToString ( );
      this._Description = Description;
    }

    // ==================================================================================
    /// <summary>
    /// Constructor with specified initial values
    /// </summary>
    /// <param name="Value">string: the value of the option</param>
    /// <param name="Description">string: the description of the option</param>
    // ----------------------------------------------------------------------------------
    public EdRole ( string Value, string Description )
    {
      this._RoleId = Value.Trim();
      this._Description = Description.Trim();
    }

    #endregion

    #region Constants list

    public const string CONST_DESIGNER = "Designer";
    public const string CONST_ADMINISTRATOR = "Administrator";

    #endregion

    #region Property list
    private string _RoleId = String.Empty;
    /// <summary>
    /// This property contains the option selection value
    /// </summary>
    /// 
    [JsonProperty ( "r" )]
    public string RoleId
    {
      get { return _RoleId; }
      set { _RoleId = value.Trim ( ); }
    }

    private string _Description = String.Empty;

    /// <summary>
    /// This property contains the option description.
    /// </summary>
    [JsonProperty ( "d" )]
    public string Description
    {
      get { return _Description; }
      set { _Description = value.Trim ( ); }
    }

    #endregion

  }//END EvOption method

}//END namespace Evado.Model
