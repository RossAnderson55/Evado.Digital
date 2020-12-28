// ***********************************************************************
// Assembly         : EvadoAdminUtil.ActiveDirectory
// Author           : Hanmoi
// Created          : 02-14-2013
//
// Last Modified By : Hanmoi
// Last Modified On : 02-18-2013
// ***********************************************************************
// <copyright file="EvAdFacade.cs" company="Evado Pty Ltd">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;

namespace Evado.ActiveDirectoryServices
{
    /// <summary>
    /// Class EvAdFacade
    /// Managing User accounts with MS Active Directory Service.
    /// </summary>
    public class EvAdFacade : IEvAccountManageable
    {
        #region Const private fields

        private readonly EvAdConfig _evAdConfig;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="EvAdFacade"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        public EvAdFacade(EvAdConfig config)
        {
            _evAdConfig = config;
        }

        /// <summary>
        /// Creates the new evUser.
        /// </summary>
        /// <param name="evUser">EvUser.</param>
        /// <returns>EvAmCallResult.</returns>
        /// <example>
        /// <code numberLines="true">
        /// var userId = "User" + Guid.NewGuid().ToString();
        /// var userPassword = "evUserPassword" + Guid.NewGuid().ToString();
        /// var evUser = new EvUser(userId, userPassword)
        /// {
        ///    EmailAddress = @"test@evado.com.au"
        /// };
        /// 
        /// var evGroupName = "Group" + Guid.NewGuid().ToString();
        /// var evGroup = new EvGroup()
        /// {
        ///    Name = evGroupName,
        ///    Description = "New Description"
        /// };
        /// var evGroups = new List&lt;EvGroup&gt;();
        /// evGroups.Add(evGroup);
        /// evUser.EvGroups = evGroups;
        /// 
        /// var config = new EvAdConfig()
        ///        {
        ///            ContextType = ContextType.ApplicationDirectory,
        ///            Server = "localhost:389",
        ///            RootContainer = "dc=evado",
        ///            UsersContainer = "CN=Users,DC=evado",
        ///            GroupsContainer = "CN=Roles,DC=evado"
        ///        };
        ///   
        /// var adFacade = EvAdFacadeFactory.CreateFacade(config); 
        /// var callResult = adFacade.CreateNewUser(evUser);
        /// </code>
        /// </example>
        public EvAdsUserProfile CreateNewUser(EvAdsUserProfile evUser)
        {
            var validation = AdUserParamValidator(evUser);
            if (validation == false)
            {
                return null;
            }

            CreateUserPrincipalWithAdUser(evUser);

            EvAmCallResult callResult = 0;
            return FindAdUserById(evUser.SamAccountName, out callResult);
        }

        /// <summary>
        /// Updates the ad evUser.
        /// </summary>
        /// <param name="evUser">EvUser.</param>
        /// <returns>EvAmCallResult.</returns>
        /// <example>
        /// <code numberLines="true">
        /// var userId = "User" + Guid.NewGuid().ToString();
        /// var userPassword = "evUserPassword" + Guid.NewGuid().ToString();
        /// var evUser = new EvUser(userId, userPassword);
        /// 
        /// var config = new EvAdConfig()
        ///        {
        ///            ContextType = ContextType.ApplicationDirectory,
        ///            Server = "localhost:389",
        ///            RootContainer = "dc=evado",
        ///            UsersContainer = "CN=Users,DC=evado",
        ///            GroupsContainer = "CN=Roles,DC=evado"
        ///        };
        ///   
        /// var adFacade = EvAdFacadeFactory.CreateFacade(config); 
        /// var callResult = adFacade.UpdateAdUser(evUser);
        /// </code>
        /// </example>
        public EvAdsUserProfile UpdateAdUser(EvAdsUserProfile evUser)
        {

            Action<UserPrincipal> actionToUpdate = up =>
                {
                    // up.Name = evUser.UserId;
                    // up.SetPassword(evUser.EvUserPassword);

                    if (up.Description != evUser.Description)
                    {
                        if (string.IsNullOrEmpty(evUser.Description))
                        {
                            up.Description = null;
                        }
                        else
                        {
                            up.Description = evUser.Description;
                        }

                    }

                    if (up.DisplayName != evUser.DisplayName)
                    {
                        if (string.IsNullOrEmpty(evUser.DisplayName))
                        {
                            up.DisplayName = null;
                        }
                        else
                        {
                            up.DisplayName = evUser.DisplayName;
                        }
                    }

                    if (up.EmailAddress != evUser.EmailAddress)
                    {
                        if (string.IsNullOrEmpty(evUser.EmailAddress))
                        {
                            up.EmailAddress = null;
                        }
                        else
                        {
                            up.EmailAddress = evUser.EmailAddress;
                        }
                    }

                    if (up.GivenName != evUser.GivenName)
                    {
                        if (string.IsNullOrEmpty(evUser.GivenName))
                        {
                            up.GivenName = null;
                        }
                        else
                        {
                            up.GivenName = evUser.GivenName;
                        }
                    }

                    if (up.Surname != evUser.Surname)
                    {
                        if (string.IsNullOrEmpty(evUser.Surname))
                        {
                            up.Surname = null;
                        }
                        else
                        {
                            up.Surname = evUser.Surname;
                        }
                    }

                    if (up.UserPrincipalName != evUser.UserPrincipalName)
                    {
                        if (string.IsNullOrEmpty(evUser.UserPrincipalName))
                        {
                            up.UserPrincipalName = null;
                        }
                        else
                        {
                            up.UserPrincipalName = evUser.UserPrincipalName;
                        }
                    }

                    if (up.SamAccountName != evUser.SamAccountName)
                    {
                        if (string.IsNullOrEmpty(evUser.SamAccountName))
                        {
                            up.SamAccountName = null;
                        }
                        else
                        {
                            up.SamAccountName = evUser.SamAccountName;
                        }
                    }

                    if (up.VoiceTelephoneNumber != evUser.VoiceTelephoneNumber)
                    {
                        if (string.IsNullOrEmpty(evUser.VoiceTelephoneNumber))
                        {
                            up.VoiceTelephoneNumber = null;
                        }
                        else
                        {
                            up.VoiceTelephoneNumber = evUser.VoiceTelephoneNumber;
                        }
                    }

                    //up.Save();
                    //Enable User
                    up.Enabled = evUser.Enabled;
                    up.UserCannotChangePassword = evUser.UserCannotChangePassword;
                    up.AccountExpirationDate = evUser.AccountExpirationDate;
                    up.PasswordNeverExpires = evUser.PasswordNeverExpires;
                    up.Save();

                    CreateOrUpdateGroupPrincipal(evUser, up);
                };

            var result = TemplateForAdUserOperation(evUser, actionToUpdate);

            var rolesToBeDeleted = evUser.EvRoles.FindAll(role => role.ToBeDeletedFromUser == true);
            if (rolesToBeDeleted.Count > 0)
            {
                UnregisteringUserFromMultipleGroups(evUser, rolesToBeDeleted);
            }

            EvAmCallResult callResult = 0;

            return FindAdUserById(evUser.SamAccountName, out callResult);
        }

        /// <summary>
        /// Deletes the evUser from ad.
        /// </summary>
        /// <param name="evUser">EvUser.</param>
        /// <returns>EvAmCallResult.</returns>
        /// <example>
        /// <code numberLines="true">
        /// var userId = "User" + Guid.NewGuid().ToString();
        /// var userPassword = "evUserPassword" + Guid.NewGuid().ToString();
        /// var evUser = new EvUser(userId, userPassword);
        /// 
        /// var config = new EvAdConfig()
        ///        {
        ///            ContextType = ContextType.ApplicationDirectory,
        ///            Server = "localhost:389",
        ///            RootContainer = "dc=evado",
        ///            UsersContainer = "CN=Users,DC=evado",
        ///            GroupsContainer = "CN=Roles,DC=evado"
        ///        };
        ///   
        /// var adFacade = EvAdFacadeFactory.CreateFacade(config); 
        /// var callResult = adFacade.DeleteUserFromAd(evUser);
        /// </code>
        /// </example>
        public EvAmCallResult DeleteUserFromAd(EvAdsUserProfile evUser)
        {
            Action<UserPrincipal> actionToUpdate = up => up.Delete();

            var result = TemplateForAdUserOperation(evUser, actionToUpdate);
            return result;
        }

        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <param name="evUser">EvUser.</param>
        /// <returns>EvAmCallResult.</returns>
        /// <example>
        /// <code numberLines="true">
        /// var userId = "User" + Guid.NewGuid().ToString();
        /// var userPassword = "evUserPassword" + Guid.NewGuid().ToString();
        /// var evUser = new EvUser(userId, userPassword);
        /// 
        /// var config = new EvAdConfig()
        ///        {
        ///            ContextType = ContextType.ApplicationDirectory,
        ///            Server = "localhost:389",
        ///            RootContainer = "dc=evado",
        ///            UsersContainer = "CN=Users,DC=evado",
        ///            GroupsContainer = "CN=Roles,DC=evado"
        ///        };
        ///   
        /// var adFacade = EvAdFacadeFactory.CreateFacade(config); 
        /// var callResult = adFacade.ResetPassword(evUser);
        /// </code>
        /// </example>
        public EvAmCallResult ResetPassword(EvAdsUserProfile evUser)
        {
            Action<UserPrincipal> actionToResetPassword = up =>
                {
                    up.ExpirePasswordNow();
                    up.Save();
                };

            var result = TemplateForAdUserOperation(evUser, actionToResetPassword);
            return result;
        }

        /// <summary>
        /// Sets the new password.
        /// </summary>
        /// <param name="evUser">EvUser.</param>
        /// <param name="newPassword">string.</param>
        /// <returns>EvAmCallResult.</returns>
        /// <example>
        /// <code numberLines="true">
        /// var userId = "User" + Guid.NewGuid().ToString();
        /// var userPassword = "evUserPassword" + Guid.NewGuid().ToString();
        /// var evUser = new EvUser(userId, userPassword);
        /// var newPassword = "newpassword";
        /// var config = new EvAdConfig()
        ///        {
        ///            ContextType = ContextType.ApplicationDirectory,
        ///            Server = "localhost:389",
        ///            RootContainer = "dc=evado",
        ///            UsersContainer = "CN=Users,DC=evado",
        ///            GroupsContainer = "CN=Roles,DC=evado"
        ///        };
        ///   
        /// var adFacade = EvAdFacadeFactory.SetNewPassword(config); 
        /// var callResult = adFacade.SetNewPassword(evUser, newPassword);
        /// </code>
        /// </example>
        public EvAmCallResult SetNewPassword(EvAdsUserProfile evUser, string newPassword)
        {
            EvAmCallResult callResult;
            if (ValidateCredentials(evUser, out callResult) == false)
            {
                return EvAmCallResult.InvalidCredential;
            }

            var userPrincipal = GetUserPrincipal(evUser);
            using (userPrincipal)
            {
                userPrincipal.SetPassword(newPassword);
                userPrincipal.Save();
            }

            return EvAmCallResult.Success;
        }

        /// <summary>
        /// Finds the ad evUser by id.
        /// </summary>
        /// <param name="evUserId">string.</param>
        /// <param name="callResult">EvAmCallResultt.</param>
        /// <returns>EvUser.</returns>
        /// <example>
        /// <code numberLines="true">
        /// var userId = "User" + Guid.NewGuid().ToString();
        /// var userPassword = "evUserPassword" + Guid.NewGuid().ToString();
        /// var evUser = new EvUser(userId, userPassword);
        /// EvAmCallResult callResult;
        /// var config = new EvAdConfig()
        ///        {
        ///            ContextType = ContextType.ApplicationDirectory,
        ///            Server = "localhost:389",
        ///            RootContainer = "dc=evado",
        ///            UsersContainer = "CN=Users,DC=evado",
        ///            GroupsContainer = "CN=Roles,DC=evado"
        ///        };
        /// var adFacade = EvAdFacadeFactory.SetNewPassword(config); 
        /// var evUser = adFacade.FindAdUserById(evUserId, out callResult);
        /// </code>
        /// </example>
        public EvAdsUserProfile FindAdUserById(string evUserId, out EvAmCallResult callResult)
        {
            var validation = StringParamValidator(evUserId);
            if (validation == false)
            {
                callResult = EvAmCallResult.InvalidArgument;
                return null;
            }

            var userPrincipal = GetUserPrincipalWithId(evUserId);
            if (userPrincipal == null)
            {
                callResult = EvAmCallResult.ObjectNotFound;
                return null;
            }

            EvAdsUserProfile evUser = null;
            using (userPrincipal)
            {
                evUser = CreateEvUserWithUserPrincipal(userPrincipal, true);
            }

            callResult = EvAmCallResult.Success;
            return evUser;
        }

        /// <summary>
        /// Validates the credentials.
        /// </summary>
        /// <param name="evUser">EvUser.</param>
        /// <param name="callResult">EvAmCallResult.</param>
        /// <returns><c>true</c> if evUserId and Password are matched, <c>false</c> otherwise</returns>
        /// <example>
        /// <code numberLines="true">
        /// var userId = "User" + Guid.NewGuid().ToString();
        /// var userPassword = "evUserPassword" + Guid.NewGuid().ToString();
        /// var evUser = new EvUser(userId, userPassword);
        /// EvAmCallResult callResult;
        /// var config = new EvAdConfig()
        ///        {
        ///            ContextType = ContextType.ApplicationDirectory,
        ///            Server = "localhost:389",
        ///            RootContainer = "dc=evado",
        ///            UsersContainer = "CN=Users,DC=evado",
        ///            GroupsContainer = "CN=Roles,DC=evado"
        ///        };
        /// var adFacade = EvAdFacadeFactory.SetNewPassword(config); 
        /// var evUser = adFacade.ValidateCredentials(evUser, out callResult);
        /// </code>
        /// </example>
        public bool ValidateCredentials(EvAdsUserProfile evUser, out EvAmCallResult callResult)
        {
            var validation = AdUserParamValidator(evUser);
            if (validation == false)
            {
                callResult = EvAmCallResult.InvalidArgument;
                return false;
            }

            var principalContext = RootPrincipalContext();
            var authenticated = false;

            using (principalContext)
            {
                authenticated = principalContext.ValidateCredentials(evUser.UserId, evUser.EvUserPassword);
                if (authenticated == false)
                {
                    authenticated = principalContext.ValidateCredentials(evUser.UserId, evUser.EvUserPassword, ContextOptions.SimpleBind);
                }
            }

            callResult = EvAmCallResult.Success;
            return authenticated;
        }

        /// <summary>
        /// Ads the groups having evUser.
        /// </summary>
        /// <param name="evUserId">string.</param>
        /// <param name="callResult">EvAmCallResult.</param>
        /// <returns>List&lt;EvGroup&gt;</returns>
        /// <example>
        /// <code>
        /// var userId = "User" + Guid.NewGuid().ToString();
        /// var userPassword = "evUserPassword" + Guid.NewGuid().ToString();
        /// var evUser = new EvUser(userId, userPassword);
        /// EvAmCallResult callResult;
        /// var config = new EvAdConfig()
        ///        {
        ///            ContextType = ContextType.ApplicationDirectory,
        ///            Server = "localhost:389",
        ///            RootContainer = "dc=evado",
        ///            UsersContainer = "CN=Users,DC=evado",
        ///            GroupsContainer = "CN=Roles,DC=evado"
        ///        };
        /// var adFacade = EvAdFacadeFactory.SetNewPassword(config); 
        /// var evGroups = adFacade.AdGroupsHavingUser(evUser, out callResult);
        /// </code>
        /// </example>
        public List<EvAdsGroupProfile> AdGroupsHavingUser(string evUserId, out EvAmCallResult callResult)
        {
            var validation = StringParamValidator(evUserId);
            if (validation == false)
            {
                callResult = EvAmCallResult.InvalidArgument;
                return null;
            }

            var userPrincipal = GetUserPrincipalWithId(evUserId);

            if (userPrincipal == null)
            {
                callResult = EvAmCallResult.ObjectNotFound;
                return null;
            }
            using (userPrincipal)
            {
                var groupPrincipals = userPrincipal.GetGroups();
                using (groupPrincipals)
                {
                    callResult = EvAmCallResult.Success;
                    return groupPrincipals.Select(gp => CreateEvGroupWithGroupPrincipal((GroupPrincipal)gp, true)).ToList();
                }
            }
        }

        /// <summary>
        /// Creates the new evGroup.
        /// </summary>
        /// <param name="evGroup">EvGroup.</param>
        /// <returns>EvAmCallResult.</returns>
        /// <example>
        /// <code numberLines="true">
        /// var evGroupName = "Group" + Guid.NewGuid().ToString();
        /// var evGroup = new EvGroup()
        /// {
        ///     Name = evGroupName,
        ///     Description = "New Description"
        /// };
        /// var config = new EvAdConfig()
        ///        {
        ///            ContextType = ContextType.ApplicationDirectory,
        ///            Server = "localhost:389",
        ///            RootContainer = "dc=evado",
        ///            UsersContainer = "CN=Users,DC=evado",
        ///            GroupsContainer = "CN=Roles,DC=evado"
        ///        };
        /// var adFacade = EvAdFacadeFactory.SetNewPassword(config); 
        /// var callResult = adFacade.CreateNewGroup(evGroup);
        /// </code>
        /// </example>
        public EvAmCallResult CreateNewGroup(EvAdsGroupProfile evGroup)
        {
            var validation = AdGroupParamValidator(evGroup);
            if (validation == false)
            {
                return EvAmCallResult.InvalidArgument;
            }
            var principalContext = GroupsPrincipalContext();

            using (principalContext)
            {
                var gp = CreateGroupPrincipalWithAdGroup(evGroup);
                gp.Dispose();
            }

            return EvAmCallResult.Success;
        }

        /// <summary>
        /// Updates the evGroup.
        /// </summary>
        /// <param name="evGroup">EvGroup.</param>
        /// <returns>EvAmCallResult.</returns>
        /// <example>
        /// <code numberLines="true">
        /// var evGroupName = "Group" + Guid.NewGuid().ToString();
        /// var evGroup = new EvGroup()
        /// {
        ///     Name = evGroupName,
        ///     Description = "New Description"
        /// };
        /// evGroup.Description = "Again Description";
        /// 
        /// var config = new EvAdConfig()
        ///        {
        ///            ContextType = ContextType.ApplicationDirectory,
        ///            Server = "localhost:389",
        ///            RootContainer = "dc=evado",
        ///            UsersContainer = "CN=Users,DC=evado",
        ///            GroupsContainer = "CN=Roles,DC=evado"
        ///        };
        /// var adFacade = EvAdFacadeFactory.SetNewPassword(config); 
        /// var callResult = adFacade.UpdateGroup(evGroup);
        /// </code>
        /// </example>
        public EvAmCallResult UpdateGroup(EvAdsGroupProfile evGroup)
        {
            Action<GroupPrincipal> actionToUpdate = gp =>
                {
                    if (gp.Description != evGroup.Description)
                    {
                        if (string.IsNullOrEmpty(evGroup.Description))
                        {
                            gp.Description = null; 
                        }
                        else
                        {
                            gp.Description = evGroup.Description;
                        }
                    }
                };
            return TemplateForAdGroupOperation(evGroup, actionToUpdate);
        }

        /// <summary>
        /// Deletes the evGroup from AD.
        /// </summary>
        /// <param name="evGroup">EvGroup.</param>
        /// <returns>EvAmCallResult.</returns>
        /// <example>
        /// <code numberLines="true">
        /// var evGroupName = "Group" + Guid.NewGuid().ToString();
        /// var evGroup = new EvGroup()
        /// {
        ///     Name = evGroupName,
        ///     Description = "New Description"
        /// };
        /// 
        /// var config = new EvAdConfig()
        ///        {
        ///            ContextType = ContextType.ApplicationDirectory,
        ///            Server = "localhost:389",
        ///            RootContainer = "dc=evado",
        ///            UsersContainer = "CN=Users,DC=evado",
        ///            GroupsContainer = "CN=Roles,DC=evado"
        ///        };
        /// var adFacade = EvAdFacadeFactory.SetNewPassword(config); 
        /// var callResult = adFacade.DeleteGroupFromAD(evGroup);
        /// </code>
        /// </example>
        public EvAmCallResult DeleteGroupFromAD(EvAdsGroupProfile evGroup)
        {
            Action<GroupPrincipal> actionToDelete = gp => gp.Delete();
            return TemplateForAdGroupOperation(evGroup, actionToDelete);
        }

        /// <summary>
        /// Finds the name of the ad evGroup by.
        /// </summary>
        /// <param name="evGroupName">string.</param>
        /// <param name="callResult">EvAmCallResult.</param>
        /// <returns>EvGroup.</returns>
        /// <example>
        /// <code numberLines="true">
        /// var evGroupName = "Group" + Guid.NewGuid().ToString();
        /// var evGroup = new EvGroup()
        /// {
        ///     Name = evGroupName,
        ///     Description = "New Description"
        /// };
        /// 
        /// var config = new EvAdConfig()
        ///        {
        ///            ContextType = ContextType.ApplicationDirectory,
        ///            Server = "localhost:389",
        ///            RootContainer = "dc=evado",
        ///            UsersContainer = "CN=Users,DC=evado",
        ///            GroupsContainer = "CN=Roles,DC=evado"
        ///        };
        /// var adFacade = EvAdFacadeFactory.SetNewPassword(config);
        /// EvAmCallResult callResult; 
        /// var evGroup = adFacade.FindAdGroupByName(evGroupName, out callResult);
        /// </code>
        /// </example>
        public EvAdsGroupProfile FindAdGroupByName(string evGroupName, out EvAmCallResult callResult)
        {
            var validation = StringParamValidator(evGroupName);
            if (validation == false)
            {
                callResult = EvAmCallResult.InvalidArgument;
                return null;
            }

            var groupPrincipal = GetGroupPrincipalWithName(evGroupName);
            if (groupPrincipal == null)
            {
                callResult = EvAmCallResult.ObjectNotFound;
                return null;
            }

            EvAdsGroupProfile evGroup = null;
            using (groupPrincipal)
            {
                evGroup = CreateEvGroupWithGroupPrincipal(groupPrincipal, true);
            }

            callResult = EvAmCallResult.Success;
            return evGroup;
        }

        /// <summary>
        /// Registers the evUser to A evGroup.
        /// </summary>
        /// <param name="evUser">EvUser.</param>
        /// <param name="evGroup">EvGroup.</param>
        /// <returns>EvAmCallResult.</returns>
        /// <example>
        /// <code numberLines="true">
        /// var userId = "User" + Guid.NewGuid().ToString();
        /// var userPassword = "evUserPassword" + Guid.NewGuid().ToString();
        /// var evUser = new EvUser(userId, userPassword);
        /// 
        /// var evGroupName = "Group" + Guid.NewGuid().ToString();
        /// var evGroup = new EvGroup()
        /// {
        ///     Name = evGroupName,
        ///     Description = "New Description"
        /// };
        /// 
        /// var config = new EvAdConfig()
        ///        {
        ///            ContextType = ContextType.ApplicationDirectory,
        ///            Server = "localhost:389",
        ///            RootContainer = "dc=evado",
        ///            UsersContainer = "CN=Users,DC=evado",
        ///            GroupsContainer = "CN=Roles,DC=evado"
        ///        };
        /// var adFacade = EvAdFacadeFactory.SetNewPassword(config);
        /// var callResult = adFacade.RegisterUserToAGroup(evUser, evGroup);
        /// </code>
        /// </example>
        public EvAmCallResult RegisterUserToAGroup(EvAdsUserProfile evUser, EvAdsGroupProfile evGroup)
        {
            Action<UserPrincipal, GroupPrincipal> actionToRegister = (u, g) =>
            {
                g.Members.Add(u);
                g.Save();
            };
            var callResult = RegisteringAndUnregisteringTemplateForSingleOperation(evUser, evGroup, actionToRegister);

            return callResult;
        }

        /// <summary>
        /// Registers the evUser to multiple groups.
        /// </summary>
        /// <param name="evUser">EvUser.</param>
        /// <param name="evGroups">List&lt;EvGroup&gt;.</param>
        /// <returns>EvAmCallResult.</returns>
        /// <example>
        /// <code numberLines="true">
        /// var userId = "User" + Guid.NewGuid().ToString();
        /// var userPassword = "evUserPassword" + Guid.NewGuid().ToString();
        /// var evUser = new EvUser(userId, userPassword);
        /// 
        /// var evGroupName1 = "Group" + Guid.NewGuid().ToString();
        /// var evGroup1 = new EvGroup()
        /// {
        ///     Name = evGroupName,
        ///     Description = "New Description"
        /// };
        /// var evGroupName2 = "Group" + Guid.NewGuid().ToString();
        /// var evGroup2 = new EvGroup()
        /// {
        ///     Name = evGroupName,
        ///     Description = "New Description"
        /// };
        /// 
        /// var evGroups = new List&lt;EvGroup&gt;();
        /// evGroups.Add(evGroup1);
        /// evGroups.Add(evGroup2);
        ///
        /// var config = new EvAdConfig()
        ///        {
        ///            ContextType = ContextType.ApplicationDirectory,
        ///            Server = "localhost:389",
        ///            RootContainer = "dc=evado",
        ///            UsersContainer = "CN=Users,DC=evado",
        ///            GroupsContainer = "CN=Roles,DC=evado"
        ///        };
        /// var adFacade = EvAdFacadeFactory.SetNewPassword(config);
        /// var callResult = adFacade.RegisterUserToMultipleGroups(evUser, evGroups);
        /// </code>
        /// </example>
        public EvAmCallResult RegisterUserToMultipleGroups(EvAdsUserProfile evUser, List<EvAdsGroupProfile> evGroups)
        {
            Action<UserPrincipal, GroupPrincipal> actionToRegister = (u, g) =>
            {
                g.Members.Add(u);
                g.Save();
            };
            var callResult = RegisteringAndUnregisteringTemplateForMultipleOperation(evUser, evGroups, actionToRegister);

            return callResult;
        }

        /// <summary>
        /// Unregisterings the evUser from single evGroup.
        /// </summary>
        /// <param name="evUser">EvUser.</param>
        /// <param name="evGroup">EvGroup.</param>
        /// <returns>EvAmCallResult.</returns>
        /// <example>
        /// <code numberLines="true">
        /// var userId = "User" + Guid.NewGuid().ToString();
        /// var userPassword = "evUserPassword" + Guid.NewGuid().ToString();
        /// var evUser = new EvUser(userId, userPassword);
        /// 
        /// var evGroupName = "Group" + Guid.NewGuid().ToString();
        /// var evGroup = new EvGroup()
        /// {
        ///     Name = evGroupName,
        ///     Description = "New Description"
        /// };
        /// 
        /// var config = new EvAdConfig()
        ///        {
        ///            ContextType = ContextType.ApplicationDirectory,
        ///            Server = "localhost:389",
        ///            RootContainer = "dc=evado",
        ///            UsersContainer = "CN=Users,DC=evado",
        ///            GroupsContainer = "CN=Roles,DC=evado"
        ///        };
        /// var adFacade = EvAdFacadeFactory.SetNewPassword(config);
        /// var callResult = adFacade.UnregisteringUserFromSingleGroup(evUser, evGroup);
        /// </code>
        /// </example>
        public EvAmCallResult UnregisteringUserFromSingleGroup(EvAdsUserProfile evUser, EvAdsGroupProfile evGroup)
        {
            Action<UserPrincipal, GroupPrincipal> actionToUnregister = (u, g) =>
            {
                g.Members.Remove(u);
                g.Save();
            };
            var callResult = RegisteringAndUnregisteringTemplateForSingleOperation(evUser, evGroup, actionToUnregister);

            return callResult;
        }

        /// <summary>
        /// Unregisterings the evUser from multiple groups.
        /// </summary>
        /// <param name="evUser">EvUser.</param>
        /// <param name="evGroups">List&lt;EvGroup&gt;.</param>
        /// <returns>EvAmCallResult.</returns>
        /// <example>
        /// <code numberLines="true">
        /// var userId = "User" + Guid.NewGuid().ToString();
        /// var userPassword = "evUserPassword" + Guid.NewGuid().ToString();
        /// var evUser = new EvUser(userId, userPassword);
        /// 
        /// var evGroupName1 = "Group" + Guid.NewGuid().ToString();
        /// var evGroup1 = new EvGroup()
        /// {
        ///     Name = evGroupName,
        ///     Description = "New Description"
        /// };
        /// var evGroupName2 = "Group" + Guid.NewGuid().ToString();
        /// var evGroup2 = new EvGroup()
        /// {
        ///     Name = evGroupName,
        ///     Description = "New Description"
        /// };
        /// 
        /// var evGroups = new List&lt;EvGroup&gt;();
        /// evGroups.Add(evGroup1);
        /// evGroups.Add(evGroup2);
        ///
        /// var config = new EvAdConfig()
        ///        {
        ///            ContextType = ContextType.ApplicationDirectory,
        ///            Server = "localhost:389",
        ///            RootContainer = "dc=evado",
        ///            UsersContainer = "CN=Users,DC=evado",
        ///            GroupsContainer = "CN=Roles,DC=evado"
        ///        };
        /// var adFacade = EvAdFacadeFactory.SetNewPassword(config);
        /// var callResult = adFacade.UnregisteringUserFromMultipleGroups(evUser, evGroups);
        /// </code>
        /// </example>
        public EvAmCallResult UnregisteringUserFromMultipleGroups(EvAdsUserProfile evUser, List<EvAdsGroupProfile> evGroups)
        {
            Action<UserPrincipal, GroupPrincipal> actionToUnregister = (u, g) =>
            {
                g.Members.Remove(u);
                g.Save();
            };
            var callResult = RegisteringAndUnregisteringTemplateForMultipleOperation(evUser, evGroups, actionToUnregister);

            return callResult;
        }

        public List<EvAdsUserProfile> AllEvUsers()
        {
            var userPrincipalContext = UsersPrincipalContext();
            var userPrinciapl = new UserPrincipal(userPrincipalContext);

            var principalSearcher = new PrincipalSearcher(userPrinciapl);

            return (from UserPrincipal principal in principalSearcher.FindAll()
                    select CreateEvUserWithUserPrincipal(principal, true)
                    ).ToList();
        }

        public List<EvAdsGroupProfile> AllEvGroups()
        {
            var groupPrincipalContext = GroupsPrincipalContext();
            var groupPrincipal = new GroupPrincipal(groupPrincipalContext);

            var principalSearcher = new PrincipalSearcher(groupPrincipal);

            return (from GroupPrincipal principal in principalSearcher.FindAll()
                    select CreateEvGroupWithGroupPrincipal(principal, false)
                    ).ToList();
        }

        #region Private Functions
        /// <summary>
        /// Create PrincipalContext for container.
        /// </summary>
        /// <param name="container">String.</param>
        /// <returns>PrincipalContext.</returns>
        private PrincipalContext PrincipalContextForContainer(String container)
        {
            var principalContext = new PrincipalContext(_evAdConfig.ContextType,
                                                        _evAdConfig.Server, container, _evAdConfig.AdminName,
                                                        _evAdConfig.AdminPassword);
            return principalContext;
        }

        /// <summary>
        /// Create PrincipalContext for Root container.
        /// </summary>
        /// <returns>PrincipalContext.</returns>
        private PrincipalContext RootPrincipalContext()
        {
            return PrincipalContextForContainer(_evAdConfig.RootContainer);
        }

        /// <summary>
        /// Create PrincipalContext for Users container.
        /// </summary>
        /// <returns>PrincipalContext.</returns>
        private PrincipalContext UsersPrincipalContext()
        {
            return PrincipalContextForContainer(_evAdConfig.UsersContainer);
        }

        /// <summary>
        /// Create PrincipalContext for Groups container.
        /// </summary>
        /// <returns>PrincipalContext.</returns>
        private PrincipalContext GroupsPrincipalContext()
        {
            return PrincipalContextForContainer(_evAdConfig.GroupsContainer);
        }

        /// <summary>
        /// Creates the GroupPrincipal of MS ADS with EvGroup value.
        /// </summary>
        /// <param name="evGroup">EvGroup.</param>
        /// <returns>GroupPrincipal.</returns>
        private GroupPrincipal CreateGroupPrincipalWithAdGroup(EvAdsGroupProfile evGroup)
        {
            var groupsContext = GroupsPrincipalContext();
            var groupPrincipal = new GroupPrincipal(groupsContext);

            groupPrincipal.Description = evGroup.Description;
            groupPrincipal.DisplayName = evGroup.DisplayName;
            groupPrincipal.GroupScope = evGroup.GroupScope;
            groupPrincipal.IsSecurityGroup = evGroup.IsSecurityGroup;
            groupPrincipal.Name = evGroup.Name;

            groupPrincipal.Save();

            return groupPrincipal;
        }

        /// <summary>
        /// Creates the UserPrincipal of MS ADS with EvUser value.
        /// </summary>
        /// <param name="evUser">EvUser.</param>
        /// <returns>EvAmCallResult.</returns>
        private EvAmCallResult CreateUserPrincipalWithAdUser(EvAdsUserProfile evUser)
        {
            var usersContext = UsersPrincipalContext();
            var userPrincipal = new UserPrincipal(usersContext);
            using (userPrincipal)
            {
                //Set Attributes
                userPrincipal.Name = evUser.UserId;
                // userPrincipal.SetPassword(evUser.EvUserPassword);

                userPrincipal.Description = evUser.Description;
                userPrincipal.DisplayName = evUser.DisplayName;
                userPrincipal.EmailAddress = evUser.EmailAddress;
                userPrincipal.GivenName = evUser.GivenName;
                userPrincipal.MiddleName = evUser.MiddleName;

                userPrincipal.Surname = evUser.Surname;
                userPrincipal.UserPrincipalName = evUser.UserPrincipalName;
                userPrincipal.VoiceTelephoneNumber = evUser.VoiceTelephoneNumber;
                userPrincipal.SamAccountName = evUser.SamAccountName;
                userPrincipal.Save();

                //Enable User
                userPrincipal.Enabled = evUser.Enabled;
                userPrincipal.AccountExpirationDate = evUser.AccountExpirationDate;
                userPrincipal.PasswordNeverExpires = evUser.PasswordNeverExpires;
                userPrincipal.Save();

                CreateOrUpdateGroupPrincipal(evUser, userPrincipal);
            }

            return EvAmCallResult.Success;
        }

        /// <summary>
        /// Creates the or update  GroupPrincipal where UserPrincipal is registered.
        /// </summary>
        /// <param name="evUser">EvUser.</param>
        /// <param name="userPrincipal">UserPrincipal.</param>
        private void CreateOrUpdateGroupPrincipal(EvAdsUserProfile evUser, UserPrincipal userPrincipal)
        {
            var evGroups = evUser.EvRoles;
            var groupPrincipalsToAddUser = evGroups.Select(eg => GetGroupPrincipal(eg)
                                                                      ?? CreateGroupPrincipalWithAdGroup(eg)).ToList();

            foreach (var groupPrincipal in groupPrincipalsToAddUser)
            {
                if (groupPrincipal.Members.Contains(userPrincipal) == false)
                {
                    groupPrincipal.Members.Add(userPrincipal);
                    groupPrincipal.Save();
                }
            }
        }

        /// <summary>
        /// Creates the EvUser with GroupPrincipal of ADS.
        /// </summary>
        /// <param name="userPrincipal">UserPrincipal.</param>
        /// <returns>EvUser.</returns>
        private EvAdsUserProfile CreateEvUserWithUserPrincipal(UserPrincipal userPrincipal, bool withGroup)
        {
            var evUser = EvUserInstance(userPrincipal);

            if (withGroup == true)
            {
                var groupPrincipals = userPrincipal.GetGroups();

                foreach (var principal in groupPrincipals)
                {
                    var adGroup = CreateEvGroupWithGroupPrincipal((GroupPrincipal)principal, false);
                    evUser.AddEvRole(adGroup);
                }
            }

            return evUser;
        }

        private EvAdsUserProfile EvUserInstance(UserPrincipal userPrincipal)
        {
            var evUser = new EvAdsUserProfile(userPrincipal.SamAccountName)
                {
                    UserGuid = userPrincipal.Guid,
                    AccountExpirationDate = userPrincipal.AccountExpirationDate,
                    AccountLockoutTime = userPrincipal.AccountLockoutTime,
                    BadLogonCount = userPrincipal.BadLogonCount,
                    Description = userPrincipal.Description,
                    DisplayName = userPrincipal.DisplayName,
                    EmailAddress = userPrincipal.EmailAddress,
                    Enabled = userPrincipal.Enabled,
                    GivenName = userPrincipal.GivenName,
                    LastLogon = userPrincipal.LastLogon,
                    LastPasswordSet = userPrincipal.LastPasswordSet,
                    MiddleName = userPrincipal.MiddleName,
                    PasswordNeverExpires = userPrincipal.PasswordNeverExpires,
                    SamAccountName = userPrincipal.SamAccountName,
                    Surname = userPrincipal.Surname,
                    UserCannotChangePassword = userPrincipal.UserCannotChangePassword,
                    UserPrincipalName = userPrincipal.UserPrincipalName,
                    VoiceTelephoneNumber = userPrincipal.VoiceTelephoneNumber
                };
            return evUser;
        }

        /// <summary>
        /// Creates the EvGroup with GroupPrincipal of ADS.
        /// </summary>
        /// <param name="groupPrincipal">The evGroup principal.</param>
        /// <returns>EvGroup.</returns>
        private EvAdsGroupProfile CreateEvGroupWithGroupPrincipal(GroupPrincipal groupPrincipal, bool withMembers)
        {
            var evGroup = EvGroupInstance(groupPrincipal);

            if (withMembers == true)
            {
                var userPrincipal = groupPrincipal.GetMembers();

                foreach (var principal in userPrincipal)
                {
                    var up = principal as UserPrincipal;
                    if (up != null)
                    {
                        var evUser = EvUserInstance(up);
                        evGroup.AddEvUser(evUser);
                    }
                }
            }

            return evGroup;
        }

        private static EvAdsGroupProfile EvGroupInstance(GroupPrincipal groupPrincipal)
        {
            var evGroup = new EvAdsGroupProfile()
                {
                    GroupGuid = groupPrincipal.Guid,
                    Description = groupPrincipal.Description,
                    DisplayName = groupPrincipal.DisplayName,
                    GroupScope = groupPrincipal.GroupScope,
                    IsSecurityGroup = groupPrincipal.IsSecurityGroup,
                    Name = groupPrincipal.Name,
                    SamAccountName = groupPrincipal.SamAccountName
                };
            return evGroup;
        }

        /// <summary>
        /// Gets the  UserPrincipal of ADS.
        /// </summary>
        /// <param name="evUser">EvUser.</param>
        /// <returns>UserPrincipal.</returns>
        private UserPrincipal GetUserPrincipal(EvAdsUserProfile evUser)
        {
            var userPrincipal = GetUserPrincipalWithId(evUser.UserId);
            return userPrincipal;
        }

        /// <summary>
        /// Gets the  UserPrincipal of ADS with evUserId.
        /// </summary>
        /// <param name="evUserId">string.</param>
        /// <returns>UserPrincipal.</returns>
        private UserPrincipal GetUserPrincipalWithId(string evUserId)
        {
            var principalContext = RootPrincipalContext();

            var userPrincipal =
                UserPrincipal.FindByIdentity(principalContext, evUserId);
            return userPrincipal;
        }

        /// <summary>
        /// Gets the GroupPrincipal.
        /// </summary>
        /// <param name="evGroup">EvGroup.</param>
        /// <returns>GroupPrincipal.</returns>
        private GroupPrincipal GetGroupPrincipal(EvAdsGroupProfile evGroup)
        {
            var groupPrincipal = GetGroupPrincipalWithName(evGroup.Name);
            return groupPrincipal;
        }

        /// <summary>
        /// Gets the GroupPrincipal with Unique EvGroupName.
        /// </summary>
        /// <param name="evGroupName">string.</param>
        /// <returns>GroupPrincipal.</returns>
        private GroupPrincipal GetGroupPrincipalWithName(string evGroupName)
        {
            var principalContext = RootPrincipalContext();

            var groupPrincipal =
                GroupPrincipal.FindByIdentity(principalContext, IdentityType.Name, evGroupName);
            return groupPrincipal;
        }

        /// <summary>
        /// Validate the value of EvUser parameter.
        /// </summary>
        /// <param name="evUser">EvUser.</param>
        /// <returns><c>true</c> if evUser is not null, <c>false</c> otherwise</returns>
        private bool AdUserParamValidator(EvAdsUserProfile evUser)
        {
            return evUser != null;
        }

        /// <summary>
        /// Validate the value of string parameter.
        /// </summary>
        /// <param name="evUserId">The ad evUser id.</param>
        /// <returns><c>true</c> if evUserId is not empty and null , <c>false</c> otherwise</returns>
        private bool StringParamValidator(string evUserId)
        {
            return !String.IsNullOrEmpty(evUserId);
        }

        /// <summary>
        /// Validate the value of EvGroup parameter.
        /// </summary>
        /// <param name="evGroup">EvGroup.</param>
        /// <returns><c>true</c> if evGroup is not empty and null, <c>false</c> otherwise</returns>
        private bool AdGroupParamValidator(EvAdsGroupProfile evGroup)
        {
            return evGroup != null;
        }

        /// <summary>
        /// Templates Method for APIs that have a single EvUser parameter
        /// </summary>
        /// <param name="evUser">EvUser.</param>
        /// <param name="action">Action&lt;UserPrincipal&gt;.</param>
        /// <returns>EvAmCallResult.</returns>
        private EvAmCallResult TemplateForAdUserOperation(EvAdsUserProfile evUser, Action<UserPrincipal> action)
        {
            var validation = AdUserParamValidator(evUser);
            if (validation == false)
            {
                return EvAmCallResult.InvalidArgument;
            }

            var userPrincipal = GetUserPrincipal(evUser);

            if (userPrincipal == null)
            {
                return EvAmCallResult.ObjectNotFound;
            }

            using (userPrincipal)
            {
                action(userPrincipal);
            }
            return EvAmCallResult.Success;
        }

        /// <summary>
        /// Templates Method for APIs that have a single EvGroup parameter
        /// </summary>
        /// <param name="evGroup">EvGroup.</param>
        /// <param name="action">Action&lt;GroupPrincipal&gt;.</param>
        /// <returns>EvAmCallResult.</returns>
        private EvAmCallResult TemplateForAdGroupOperation(EvAdsGroupProfile evGroup, Action<GroupPrincipal> action)
        {
            var validation = AdGroupParamValidator(evGroup);
            if (validation == false)
            {
                return EvAmCallResult.InvalidArgument;
            }

            var groupPrincipal = GetGroupPrincipal(evGroup);
            if (groupPrincipal == null)
            {
                return EvAmCallResult.ObjectNotFound;
            }
            using (groupPrincipal)
            {
                action(groupPrincipal);
                return EvAmCallResult.Success;
            }
        }

        /// <summary>
        /// Registering the and unregistering template for single EvUser and EvGroup Parameter.
        /// </summary>
        /// <param name="evUser">EvUser.</param>
        /// <param name="evGroup">EvGroup.</param>
        /// <param name="action">Action&lt;UserPrincipal, GroupPrincipal&gt;.</param>
        /// <returns>EvAmCallResult.</returns>
        private EvAmCallResult RegisteringAndUnregisteringTemplateForSingleOperation(EvAdsUserProfile evUser, EvAdsGroupProfile evGroup, Action<UserPrincipal, GroupPrincipal> action)
        {
            var validationForAdUser = AdUserParamValidator(evUser);
            var validationForAdGroup = AdGroupParamValidator(evGroup);
            if (validationForAdUser == false || validationForAdGroup == false)
            {
                return EvAmCallResult.InvalidArgument;
            }

            var userPrincipal = GetUserPrincipal(evUser);
            var groupPrincipal = GetGroupPrincipal(evGroup);

            if (userPrincipal == null || groupPrincipal == null)
            {
                return EvAmCallResult.ObjectNotFound;
            }

            EvAmCallResult callResult;
            if (ValidateCredentials(evUser, out callResult) == false)
            {
                return EvAmCallResult.InvalidCredential;
            }

            using (groupPrincipal)
            {
                using (userPrincipal)
                {
                    action(userPrincipal, groupPrincipal);
                }
            }

            return EvAmCallResult.Success;
        }

        /// <summary>
        /// Registering the and unregistering template for single EvUser and multiple EvGroup Parameter.
        /// </summary>
        /// <param name="evUser">EvUser.</param>
        /// <param name="evGroups">List&lt;EvGroup>&gt;</param>
        /// <param name="action"> Action&lt;UserPrincipal, GroupPrincipal&gt;.</param>
        /// <returns>EvAmCallResult.</returns>
        private EvAmCallResult RegisteringAndUnregisteringTemplateForMultipleOperation(EvAdsUserProfile evUser, List<EvAdsGroupProfile> evGroups, Action<UserPrincipal, GroupPrincipal> action)
        {
            var validationForAdUser = AdUserParamValidator(evUser);

            if (validationForAdUser == false || evGroups == null)
            {
                return EvAmCallResult.InvalidArgument;
            }

            var userPrincipal = GetUserPrincipal(evUser);
            if (userPrincipal == null)
            {
                return EvAmCallResult.ObjectNotFound;
            }



            using (userPrincipal)
            {
                foreach (var adGroup in evGroups)
                {
                    var groupPrincipal = GetGroupPrincipal(adGroup);
                    var validationForAdGroup = AdGroupParamValidator(adGroup);
                    if (validationForAdGroup == false)
                    {
                        return EvAmCallResult.InvalidArgument;
                    }

                    if (groupPrincipal == null)
                    {
                        return EvAmCallResult.ObjectNotFound;
                    }

                    using (groupPrincipal)
                    {
                        action(userPrincipal, groupPrincipal);
                    }
                }
            }

            return EvAmCallResult.Success;
        }
        #endregion


    }
}