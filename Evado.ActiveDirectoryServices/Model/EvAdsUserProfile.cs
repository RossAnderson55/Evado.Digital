/***************************************************************************************
 * <copyright file="EvAdsUserProfile.cs company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c)  2002 - 2021  EVADO HOLDING PTY. LTD.  All rights reserved.
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
using Evado.Model;

namespace Evado.ActiveDirectoryServices
{
    public class EvAdsUserProfile
    {

      #region constructors
      //  ==================================================================================
      /// <summary>
      /// Initializes a new instance of the <see cref="EvAdsUserProfile"/> class.
      /// </summary>
      // -------------------------------------------------------------------------------------
      public EvAdsUserProfile ( )
      {
        _evGroups = new List<EvAdsGroupProfile> ( );
      }

      //  ==================================================================================
      /// <summary>
      /// Initializes a new instance of the <see cref="EvAdsUserProfile"/> class.
      /// </summary>
      /// <param name="EvUserId">A unique EvUser Id.</param>
      // -------------------------------------------------------------------------------------
      public EvAdsUserProfile ( string evUserId )
        : this ( )
      {
        _userId = evUserId;
      }

      //  ==================================================================================
      /// <summary>
      /// Initializes a new instance of the <see cref="EvAdsUserProfile"/> class.
      /// </summary>
      /// <param name="EvUserId">A unique EvUser Id.</param>
      /// <param name="evUserPassword">The evUserPassword.</param>
      // -------------------------------------------------------------------------------------
      public EvAdsUserProfile ( string evUserId, string evUserPassword )
        : this ( )
      {
        _userId = evUserId;
        _evUserPassword = evUserPassword;
      }

      #endregion //constructors


      #region public enumerations
      /// <summary>
      /// This enumerated list defines the AdGroup object member names.
      /// </summary>
      public enum UserMemberNames
      {
        User_Guid,
        OrganisationName,
        UserId,
        Password,
        EmailAddress,
        Groups,
        Description,
        DisplayName,
        Enabled,
        GivenName,
        PasswordNeverExpires,
        SamAccountName,
        Surname,
        UserCannotChangePassword,
        UserPrincipalName,
        VoiceTelephoneNumber,
        JobTitle,
        OrgId,
        PasswordResetToken,
        PasswordResetTokenExpiry,
      }

      //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      #endregion
        #region private properties
        private List<EvAdsGroupProfile> _evGroups;
        private string _userId;
        private readonly string _evUserPassword;

        #endregion // private properties

        #region public properties
        public Guid? UserGuid { get; set; }

        public string OrganisationName { get; set; }

        public string UserId { get { return _userId; } set { _userId = value; } }

       
        public string Password { get; set; }

      
        public string EmailAddress { get; set; }

    
        public List<EvAdsGroupProfile> EvGroups 
        { 
            get { return _evGroups; } 
            set { _evGroups = value; } 
        }
       
        public string Description { get; set; }

        public string DisplayName { get; set; }

        public bool? Enabled { get; set; }

        public string GivenName{ get; set; }

        public bool PasswordNeverExpires { get; set; }

        public string SamAccountName{ get; set; }

        public string Surname { get; set; }

        public bool UserCannotChangePassword { get; set; }

        public string UserPrincipalName { get; set; }

        public string VoiceTelephoneNumber { get; set; }
		
        public string JobTitle { get; set; }

        public string OrgId { get; set; }

        public string PasswordResetToken { get; set; }

        public DateTime? PasswordResetTokenExpiry { get; set; }

        #endregion //public properties

        #region public methods
        //  ==================================================================================
        /// <summary>
        /// Adds the active directory group into a list of groups.
        /// </summary>
        /// <param name="EvGroup">EvAdsGroupProfile.</param>
        // -------------------------------------------------------------------------------------
        public void AddEvGroup(EvAdsGroupProfile EvGroup)
        {
            _evGroups.Add(EvGroup);
        }

        //  ==================================================================================
        /// <summary>
        /// Check group if it needs to be deleted from AD
        /// </summary>
        /// <param name="EvGroupName">string.</param>
        // -------------------------------------------------------------------------------------
        public void CheckGroupToDeleteByName(string EvGroupName)
        {
            EvAdsGroupProfile role = FindEvGroupByName(EvGroupName);
            role.ToBeDeletedFromUser = true;
        }

        //  ==================================================================================
        /// <summary>
        /// Get Active directory group at the index of.
        /// </summary>
        /// <param name="Index">int.</param>
        /// <returns>EvAdsGroupProfile.</returns>
        // -------------------------------------------------------------------------------------
        public EvAdsGroupProfile EvGroupAt(int Index)
        {
            return _evGroups[Index];
        }

        //  ==================================================================================
        /// <summary>
        /// Finds active directory group with a unique group name.
        /// </summary>
        /// <param name="EvGroupName">string.</param>
        /// <returns>EvAdsGroupProfile.</returns>
        // -------------------------------------------------------------------------------------
        public EvAdsGroupProfile FindEvGroupByName(string EvGroupName)
        {
            return _evGroups.Find(group => group.Name.Equals(EvGroupName));
        }

        #endregion //public methods

        protected bool Equals(EvAdsUserProfile other)
        {
            return string.Equals(_userId, other._userId) 
                && string.Equals(EmailAddress, other.EmailAddress) 
                && string.Equals(Description, other.Description) 
                && string.Equals(DisplayName, other.DisplayName) 
                && Enabled.Equals(other.Enabled) 
                && string.Equals(GivenName, other.GivenName) 
                && string.Equals(SamAccountName, other.SamAccountName) 
                && string.Equals(Surname, other.Surname) 
                && UserCannotChangePassword.Equals(other.UserCannotChangePassword) 
                && string.Equals(UserPrincipalName, other.UserPrincipalName) 
                && string.Equals(VoiceTelephoneNumber, other.VoiceTelephoneNumber);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EvAdsUserProfile) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (_userId != null ? _userId.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (EmailAddress != null ? EmailAddress.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Description != null ? Description.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (DisplayName != null ? DisplayName.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ Enabled.GetHashCode();
                hashCode = (hashCode*397) ^ (GivenName != null ? GivenName.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Surname != null ? Surname.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ UserCannotChangePassword.GetHashCode();
                hashCode = (hashCode*397) ^ (UserPrincipalName != null ? UserPrincipalName.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (VoiceTelephoneNumber != null ? VoiceTelephoneNumber.GetHashCode() : 0);
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
        public EvEventCodes setValue ( UserMemberNames fieldName, String value )
        {
          //
          // Switch the FieldName based on the activity field names
          //
          switch ( fieldName )
          {
            case UserMemberNames.User_Guid:
              {
                this.UserGuid = new Guid( value );
                break;
              }
            case UserMemberNames.OrganisationName:
              {
                this.OrganisationName = value;
                break;
              }
            case UserMemberNames.UserId:
              {
                this.UserId = value;
                break;
              }
            case UserMemberNames.Password:
              {
                this.Password = value;
                break;
              }
            case UserMemberNames.EmailAddress:
              {
                this.EmailAddress = value;
                break;
              }
            case UserMemberNames.Description:
              {
                this.Description =  value ;
                break;
              }
            case UserMemberNames.DisplayName:
              {
                this.DisplayName = value;
                break;
              }
            case UserMemberNames.Enabled:
              {
                this.Enabled = Evado.Model.EvStatics.getBool( value );
                break;
              }
            case UserMemberNames.GivenName:
              {
                this.GivenName = value;
                break;
              }
            case UserMemberNames.PasswordNeverExpires:
              {
                this.PasswordNeverExpires = Evado.Model.EvStatics.getBool( value );
                break;
              }
            case UserMemberNames.SamAccountName:
              {
                this.SamAccountName = value;
                break;
              }
            case UserMemberNames.Surname:
              {
                this.Surname = value;
                break;
              }
            case UserMemberNames.UserCannotChangePassword:
              {
                this.UserCannotChangePassword = Evado.Model.EvStatics.getBool( value );
                break;
              }
            case UserMemberNames.UserPrincipalName:
              {
                this.UserPrincipalName = value;
                break;
              }
            case UserMemberNames.VoiceTelephoneNumber:
              {
                this.VoiceTelephoneNumber = value;
                break;
              }
            case UserMemberNames.JobTitle:
              {
                this.JobTitle = value;
                break;
              }
            case UserMemberNames.OrgId:
              {
                this.OrgId = value;
                break;
              }
            case UserMemberNames.PasswordResetToken:
              {
                this.PasswordResetToken = value;
                break;
              }
            case UserMemberNames.PasswordResetTokenExpiry:
              {
                this.PasswordResetTokenExpiry = Evado.Model.EvStatics.getDateTime( value );
                break;
              }
          }// End switch field name

          return EvEventCodes.Ok;

        }//End setValue method.
    }
}