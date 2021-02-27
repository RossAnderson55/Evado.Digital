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
  public class EdObjectParent
  {
    #region Public methods

    // ==================================================================================
    /// <summary>
    /// Constructor with specified initial values
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EdObjectParent ( )
    {
    }

    // ==================================================================================
    /// <summary>
    /// Constructor with specified initial values
    /// </summary>
    /// <param name="ParentLayoutId">string: the parent layout id</param>
    /// <param name="ChildLayoutId">string: the child layout id</param>
    /// <param name="ChildTitle">string: the child layout title</param>
    /// <param name="ChildEditAccess">string: the child edit access</param>
    // ----------------------------------------------------------------------------------
    public EdObjectParent ( 
      String ParentLayoutId, 
      String ChildLayoutId,
      String ChildTitle,
      String ChildEditAccess )
    {
      this.ParentLayoutId = ParentLayoutId.ToString ( );
      this.ChildLayoutId = ChildLayoutId;
      this.ChildTitle = ChildTitle;
      this.ChildEditAccess = ChildEditAccess;
    }

    #endregion

    #region Property list
    /// <summary>
    /// This property contains the parent layoutId
    /// </summary>
    /// 
    [JsonProperty ( "pl" )]
    public string ParentLayoutId {get; set;}


    /// <summary>
    /// This property contains the child layoutId
    /// </summary>
    [JsonProperty ( "cl" )]
    public string ChildLayoutId { get; set; }


    /// <summary>
    /// This property contains the child title
    /// </summary>
    [JsonProperty ( "ct" )]
    public string ChildTitle { get; set; }


    /// <summary>
    /// This property contains the child edit access roles
    /// </summary>
    [JsonProperty ( "cea" )]
    public string ChildEditAccess { get; set; }

    #endregion

  }//END EvOption method

}//END namespace Evado.Model
