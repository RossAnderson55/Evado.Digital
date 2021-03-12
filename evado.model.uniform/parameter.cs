/***************************************************************************************
 * <copyright file="Evado.Model.UniForm\AbstractedPage.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2013 - 2021 EVADO HOLDING PTY. LTD.  All rights reserved.
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
 *  This class contains the AbstractedPage data object.
 *
 ****************************************************************************************/
using System;
using Newtonsoft.Json;

namespace Evado.Model.UniForm
{
  /// <summary>
  /// This class defines the method parameter object structure.
  /// </summary>
  [Serializable]
  public class Parameter
  {
    #region class initialisation methods

    //  =================================================================================
    /// <summary>
    /// This method initialises the class with parameters and values.
    /// </summary>
    //  ---------------------------------------------------------------------------------
    public Parameter( )
    {
    }

    //  =================================================================================
    /// <summary>
    /// This method initialiseas the class with parameters.
    /// </summary>
    /// <param name="Name">String: Parameter name</param>
    /// <param name="Value">String: Parameter value</param>
    //  ---------------------------------------------------------------------------------
    public Parameter( String Name, String Value )
    {
      this._Name = Name.Trim( );
      this._Value = Value.Trim( );
    }

    //  =================================================================================
    /// <summary>
    /// This method initialises the class with parameters.
    /// </summary>
    /// <param name="Name">String: Parameter name</param>
    /// <param name="Value">Int: Parameter value</param>
    //  ---------------------------------------------------------------------------------
    public Parameter( String Name, int Value )
    {
      this._Name = Name.Trim( );
      this._Value = Value.ToString( );
    }

    //  =================================================================================
    /// <summary>
    /// This method initialises the class with parameters.
    /// </summary>
    /// <param name="Name">String: Parameter name</param>
    /// <param name="Value">Float: Parameter value</param>
    //  ---------------------------------------------------------------------------------
    public Parameter( String Name, float Value )
    {
      this._Name = Name.Trim( );
      this._Value = Value.ToString( );
    }

    //  =================================================================================
    /// <summary>
    /// This method initialises the class with parameters.
    /// </summary>
    /// <param name="Name">Parameter name</param>
    /// <param name="Value">Guid: Parameter value</param>
    //  ---------------------------------------------------------------------------------
    public Parameter( String Name, Guid Value )
    {
      this._Name = Name.Trim( );
      this._Value = Value.ToString( );
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class Properties

    private String _Name = String.Empty;

    /// <summary>
    /// This property contains the parameter name of the Parameter object.
    /// </summary>
    [JsonProperty ( "n" )]
    public String Name
    {
      get { return this._Name; }
      set { this._Name = value.Trim( ); }
    }

    private String _Value = String.Empty;

    /// <summary>
    /// This property contains the command parameter that will be used to identify this 
    /// Command when it is recieved by backend when processing the command.
    /// </summary>
    [JsonProperty ( "v" )]
    public String Value
    {
      get { return this._Value; }
      set { this._Value = value.Trim( ); }
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
  }
}//END namespace