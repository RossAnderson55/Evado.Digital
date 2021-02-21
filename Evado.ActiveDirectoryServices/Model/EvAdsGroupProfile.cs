/***************************************************************************************
 * <copyright file="EvAdsGroupProfile.cs company="EVADO HOLDING PTY. LTD.">
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
using System.DirectoryServices.AccountManagement;

using Evado.Model;

namespace Evado.ActiveDirectoryServices
{
  //  ==================================================================================
  /// <summary>
  /// Class EvGroup
  /// </summary>
  //  ---------------------------------------------------------------------------------- 
  public class EvAdsGroupProfile
  {
    #region Initialisation methods.
    /// <summary>
    /// this is the class initialisation method.
    /// </summary>
    public EvAdsGroupProfile ( )
    {
      _evUsers = new List<EvAdsUserProfile> ( );
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region public enumerations
    /// <summary>
    /// This enumerated list defines the AdGroup object member names.
    /// </summary>
    public enum GroupMemberNames
    {
      Name,
      Description,
      DisplayName,
      GroupScope,
      IsSecurityGroup,
      ToBeDeletedFromUser,
      SamAccountName,
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region private members and constants

    private List<EvAdsUserProfile> _evUsers;

    public const string CONST_ROLE_GROUP_PREFIX = "ROL_";

    public const string CONST_CUSTOMER_GROUP_PREFIX = "CU_";

    public const string CONST_DATABASE_GROUP_PREFIX = "DB_";

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region public properties
    public Guid? GroupGuid { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string DisplayName { get; set; }

    public GroupScope? GroupScope { get; set; }

    public bool? IsSecurityGroup { get; set; }

    public bool ToBeDeletedFromUser { get; set; }

    public List<EvAdsUserProfile> Members
    {
      get { return _evUsers; }
      set { _evUsers = value; }
    }

    public string SamAccountName { get; set; }
    #endregion //public properties

    #region public methods
    //  ==================================================================================
    /// <summary>
    /// AddEvUser method
    /// </summary>
    /// <param name="EvUser">EvAdsUserProfile.</param>
    // -------------------------------------------------------------------------------------
    public void AddEvUser ( EvAdsUserProfile EvUser )
    {
      _evUsers.Add ( EvUser );
    }

    //  ==================================================================================
    /// <summary>
    /// Get EvUser with Index
    /// </summary>
    /// <param name="Index">int.</param>
    // -------------------------------------------------------------------------------------
    public EvAdsUserProfile EvUserAt ( int Index )
    {
      return _evUsers [ Index ];
    }

    //  ==================================================================================
    /// <summary>
    /// Get EvUser with User's SamAccountName
    /// </summary>
    /// <param name="EvUserId">string.</param>
    // -------------------------------------------------------------------------------------
    public EvAdsUserProfile FindEvUserById ( string EvUserId )
    {
      return _evUsers.Find ( e => e.UserId.Equals ( EvUserId ) );
    }
    #endregion //public methods

    protected bool Equals ( EvAdsGroupProfile other )
    {
      return string.Equals ( Name, other.Name )
          && string.Equals ( Description, other.Description )
          && string.Equals ( DisplayName, other.DisplayName )
          && GroupScope == other.GroupScope
          && IsSecurityGroup.Equals ( other.IsSecurityGroup );
    }

    public override bool Equals ( object obj )
    {
      if ( ReferenceEquals ( null, obj ) ) return false;
      if ( ReferenceEquals ( this, obj ) ) return true;
      if ( obj.GetType ( ) != this.GetType ( ) ) return false;
      return Equals ( ( EvAdsGroupProfile ) obj );
    }

    public override int GetHashCode ( )
    {
      unchecked
      {
        var hashCode = ( Name != null ? Name.GetHashCode ( ) : 0 );
        hashCode = ( hashCode * 397 ) ^ ( Description != null ? Description.GetHashCode ( ) : 0 );
        hashCode = ( hashCode * 397 ) ^ ( DisplayName != null ? DisplayName.GetHashCode ( ) : 0 );
        hashCode = ( hashCode * 397 ) ^ GroupScope.GetHashCode ( );
        hashCode = ( hashCode * 397 ) ^ IsSecurityGroup.GetHashCode ( );

        return hashCode;
      }
    }

    //  ================================================================================
    /// <summary>
    /// Sets the value of this activity class field name. Validate the format of the
    /// value. 
    /// </summary>
    /// <param name="fieldName">ActivityClassFieldNames: Name of the field to be setted.</param>
    /// <param name="value">String: value to be setted</param>
    /// <returns>EvEventCodes: indicating the successful update of the property value.</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Switch the fieldName and update value for the property defined by the activity field names
    /// 
    /// 2. Return casting error, if field name type is empty
    /// </remarks>
    //  --------------------------------------------------------------------------------
    public EvEventCodes setValue ( GroupMemberNames fieldName, String value )
    {
      //
      // Switch the FieldName based on the activity field names
      //
      switch ( fieldName )
      {
        case GroupMemberNames.Name:
          {
            this.Name = value;
            break;
          }
        case GroupMemberNames.Description:
          {
            this.Description = value;
            break;
          }
        case GroupMemberNames.DisplayName:
          {
            this.DisplayName = value;
            break;
          }
        case GroupMemberNames.GroupScope:
          {
            this.GroupScope =
              Evado.Model.EvStatics.parseEnumValue
              <System.DirectoryServices.AccountManagement.GroupScope> ( value );
            break;
          }
        case GroupMemberNames.IsSecurityGroup:
          {
            this.IsSecurityGroup = Evado.Model.EvStatics.getBool ( value );
            break;
          }
        case GroupMemberNames.ToBeDeletedFromUser:
          {
            this.ToBeDeletedFromUser = Evado.Model.EvStatics.getBool ( value );
            break;
          }
        case GroupMemberNames.SamAccountName:
          {
            this.SamAccountName = value;
            break;
          }
      }// End switch field name

      return EvEventCodes.Ok;

    }//End setValue method.

  }//END EvAdGRoupProfile object.
}