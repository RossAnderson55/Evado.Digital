// ***********************************************************************
// Assembly         : EvadoAdminUtil.ActiveDirectory
// Author           : Hanmoi
// Created          : 02-12-2013
//
// Last Modified By : Hanmoi
// Last Modified On : 02-15-2013
// ***********************************************************************
// <copyright file="EvUser.cs" company="Evado Pty Ltd">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;

namespace Evado.ActiveDirectoryServices
{
    /// <summary>
    /// Class EvUser
    /// </summary>
    public class EvUserProfile
    {
        /// <summary>
        /// Active Directory EvGroups Where User is registered.
        /// </summary>
        private List<EvRole> _evGroups;
        /// <summary>
        /// A unique evUser Id in active directory system.
        /// </summary>
        private readonly string _userId;
        /// <summary>
        /// A evUser evUserPassword
        /// </summary>
        private readonly string _evUserPassword;
       
        /// <summary>
        /// Initializes a new instance of the <see cref="EvUserProfile"/> class.
        /// </summary>
        public EvUserProfile()
        {
            _evGroups = new List<EvRole>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EvUserProfile"/> class.
        /// </summary>
        /// <param name="evUserId">A unique evUser Id.</param>
        public EvUserProfile(string evUserId)
            : this()
        {
            _userId = evUserId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EvUserProfile"/> class.
        /// </summary>
        /// <param name="evUserId">A unique evUser Id.</param>
        /// <param name="evUserPassword">The evUserPassword.</param>
        public EvUserProfile(string evUserId, string evUserPassword) :this()
        {
            _userId = evUserId;
            _evUserPassword = evUserPassword;
        }

        /// <summary>
        /// Gets a unique evUser Id.
        /// </summary>
        /// <value>The id.</value>
        public string UserId { get { return _userId; } }

        /// <summary>
        /// Gets the evUser evUserPassword.
        /// </summary>
        /// <value>The evUserPassword.</value>
        public string EvUserPassword { get { return _evUserPassword; } }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the Active directory groups.
        /// </summary>
        /// <value>The ad groups.</value>
        public List<EvRole> EvGroups 
        { 
            get { return _evGroups; } 
            set { _evGroups = value; } 
        }
       
        public DateTime? AccountExpirationDate { get; set; }

        public DateTime? AccountLockoutTime { get; set; }

        public int BadLogonCount { get; set; }

        public string Description { get; set; }

        public string DisplayName { get; set; }

        public bool? Enabled { get; set; }

        public string GivenName { get; set; }

        public DateTime? LastLogon { get; set; }

        public DateTime? LastPasswordSet { get; set; }

        public string MiddleName { get; set; }

        public bool PasswordNeverExpires { get; set; }

        public string SamAccountName { get; set; }

        public string Surname { get; set; }

        public bool UserCannotChangePassword { get; set; }

        public string UserPrincipalName { get; set; }

        public string VoiceTelephoneNumber { get; set; }

        /// <summary>
        /// Adds the active directory group into a list of groups.
        /// </summary>
        /// <param name="evGroup">The ev ad group.</param>
        public void AddEvGroup(EvRole evGroup)
        {
            _evGroups.Add(evGroup);
        }

        /// <summary>
        /// Get Active directory group at the index of.
        /// </summary>
        /// <param name="index">The list index in which group is.</param>
        /// <returns>EvGroup.</returns>
        public EvRole EvGroupAt(int index)
        {
            return _evGroups[index];
        }

        /// <summary>
        /// Finds active directory group with a unique group name.
        /// </summary>
        /// <param name="evGroupName">Name of the ads group.</param>
        /// <returns>EvGroup.</returns>
        public EvRole FindEvGroupByName(string evGroupName)
        {
            return _evGroups.Find(group => group.Name.Equals(evGroupName));
        }

        protected bool Equals(EvUserProfile other)
        {
            return string.Equals(_userId, other._userId) 
                && string.Equals(EmailAddress, other.EmailAddress) 
                && AccountExpirationDate.Equals(other.AccountExpirationDate) 
                && string.Equals(Description, other.Description) 
                && string.Equals(DisplayName, other.DisplayName) 
                && Enabled.Equals(other.Enabled) 
                && string.Equals(GivenName, other.GivenName) 
                && string.Equals(MiddleName, other.MiddleName) 
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
            return Equals((EvUserProfile) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (_userId != null ? _userId.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (EmailAddress != null ? EmailAddress.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ AccountExpirationDate.GetHashCode();
                hashCode = (hashCode*397) ^ (Description != null ? Description.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (DisplayName != null ? DisplayName.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ Enabled.GetHashCode();
                hashCode = (hashCode*397) ^ (GivenName != null ? GivenName.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (MiddleName != null ? MiddleName.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Surname != null ? Surname.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ UserCannotChangePassword.GetHashCode();
                hashCode = (hashCode*397) ^ (UserPrincipalName != null ? UserPrincipalName.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (VoiceTelephoneNumber != null ? VoiceTelephoneNumber.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}