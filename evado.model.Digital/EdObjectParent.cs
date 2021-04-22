/***************************************************************************************
 * <copyright file="EvOption.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2021 EVADO HOLDING PTY. LTD..  All rights reserved.
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
      this.IsRecord = false;
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
      String ChildEditAccess,
      bool IsRecord )
    {
      this.ParentLayoutId = ParentLayoutId.ToString ( );
      this.ChildLayoutId = ChildLayoutId;
      this.ChildTitle = ChildTitle;
      this.ChildEditAccess = ChildEditAccess;
      this.IsRecord = IsRecord;
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

    /// <summary>
    /// This property indicates the child is a record.
    /// </summary>
    [JsonProperty ( "rec" )]
    public bool IsRecord { get; set; }

    #endregion

    #region class methods.


    // =====================================================================================
    /// <summary>
    /// This method test to see if the user has a role contain in the roles delimited list.
    /// </summary>
    /// <param name="Roles">';' delimted string of roles</param>
    /// <returns>True: if the role exists.</returns>
    // -------------------------------------------------------------------------------------
    public bool hasEditAccess ( String Roles )
    {
      if ( Roles == null )
      {
        return false;
      }

      //
      // no defined read access roles indicated all users have access.
      //
      if ( this.ChildEditAccess == String.Empty )
      {
        return true;
      }

      //
      // if roles are defined and an empty string is passed return false access.
      //
      if ( Roles == String.Empty
        && this.ChildEditAccess != String.Empty )
      {
        return false;
      }

      //
      // iterate through the roles looking for a match.
      //
      foreach ( String role in Roles.Split ( ';' ) )
      {
        foreach ( String role1 in this.ChildEditAccess.Split ( ';' ) )
        {
          if ( role1.ToLower ( ) == role.ToLower ( ) )
          {
            return true;
          }
        }
      }
      return false;
    }//END method

    #endregion

  }//END EdObjectParent method

}//END namespace Evado.Model.Digital
