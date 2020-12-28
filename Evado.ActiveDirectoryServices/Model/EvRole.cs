// ***********************************************************************
// Assembly         : EvadoAdminUtil.ActiveDirectory
// Author           : Hanmoi
// Created          : 02-13-2013
//
// Last Modified By : Hanmoi
// Last Modified On : 02-15-2013
// ***********************************************************************
// <copyright file="EvGroup.cs" company="Evado Pty Ltd">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;

namespace Evado.ActiveDirectoryServices
{
    /// <summary>
    /// Class EvGroup
    /// </summary>
    public class EvRole
    {
        private List<EvUserProfile> _evUsers;

        public EvRole()
        {
            _evUsers = new List<EvUserProfile>();
        }
        /// <summary>
        /// Gets or sets the unique group name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the group.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        public string DisplayName { get; set; }

        public GroupScope? GroupScope { get; set; }

        public bool? IsSecurityGroup { get; set; }

        public List<EvUserProfile> Members
        {
            get { return _evUsers; }
            set { _evUsers = value; }
        }

        public string SamAccountName { get; set; }

        public void AddEvUser(EvUserProfile evUser)
        {
            _evUsers.Add(evUser);
        }

        public EvUserProfile EvUserAt(int index)
        {
            return _evUsers[index];
        }

        public EvUserProfile FindEvUserById(string evUserId)
        {
            return _evUsers.Find(e => e.UserId.Equals(evUserId));
        }

        protected bool Equals(EvRole other)
        {
            return string.Equals(Name, other.Name) 
                && string.Equals(Description, other.Description) 
                && string.Equals(DisplayName, other.DisplayName) 
                && GroupScope == other.GroupScope 
                && IsSecurityGroup.Equals(other.IsSecurityGroup);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EvRole) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Description != null ? Description.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (DisplayName != null ? DisplayName.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ GroupScope.GetHashCode();
                hashCode = (hashCode*397) ^ IsSecurityGroup.GetHashCode();
               
                return hashCode;
            }
        }
    }
}