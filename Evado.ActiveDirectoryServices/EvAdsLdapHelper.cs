/***************************************************************************************
 * <copyright file="EvAdsLdapHelper.cs company="EVADO HOLDING PTY. LTD.">
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
using System.DirectoryServices;

namespace Evado.ActiveDirectoryServices
{
  //  ==================================================================================
  /// <summary>
  /// Class EvAdsLdapHelper
  /// To Provide additional feature in manipulating ADS
  /// Since System.DirectoryServices.AccountManagement namespace provides limited APIs
  /// System.DirectoryServices shall be used.
  /// This Class uses LDAP protocol directly to handle ADS.
  /// </summary>
  //  ---------------------------------------------------------------------------------- 
  public class EvAdsLdapHelper
  {
    #region readonly private fields

    private readonly string _ldapRootPath = string.Empty;
    private readonly string _username = string.Empty;
    private readonly string _password = string.Empty;

    private const string _ldapTitleProp = "title";
    private const string _ldapDepartmentProp = "department";
    private const string _ldapCompanyProp = "company";
    private const string _ldapInfo = "info";

    #endregion

    //  ==================================================================================
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="LdapString">string.</param>
    /// <param name="UserName">string.</param>
    /// <param name="        public EvAdsLdapHelper(string LdapString, string UserName, string Password)
    //  ---------------------------------------------------------------------------------- 
    public EvAdsLdapHelper ( string LdapString, string UserName, string Password )
    {
      _ldapRootPath = LdapString;
      _username = UserName;
      _password = Password;
    }

    #region Public Methods
    //  ==================================================================================
    /// <summary>
    /// GetAdditionalUserProperty
    /// To handle additional ADS properties which EvAdsFacade could not
    /// </summary>
    /// <param name="EvUser">EvAdsUserProfile.</param>
    /// <example>
    /// <code numberLines="true">
    /// string userId = "User" + Guid.NewGuid().ToString();
    /// EvAdsUserProfile evUser = new EvAdsUserProfile()
    /// {
    ///    EmailAddress = @"test@evado.com.au",
    ///    SamAccountName = userId
    /// };
    /// EvAdConfig config = new EvAdConfig()
    ///		{
    ///			Server = "192.168.10.53",
    ///			ContextType = ContextType.Domain,
    ///		    RootContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    UsersContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    GroupsContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    AdminName = @"evado",
    ///		    AdminPassword = "#######",
    ///		    LdapString = "LDAP://192.168.10.53/dc=evado,dc=local"
    ///     };
    ///   
    ///   
    /// EvAdsLdapHelper helper = new EvAdsLdapHelper(config.LdapString, config.AdminName, config.AdminPassword);
    /// helper.GetAdditionalUserProperty(evUser);
    /// </code>
    /// </example>
    //  ---------------------------------------------------------------------------------- 
    public void GetAdditionalUserProperty ( EvAdsUserProfile EvUser )
    {
      DirectoryEntry deUser = this.GetDirectoryEntry ( EvUser );

      if ( deUser == null )
      {
        return;
      }

      EvUser.JobTitle = GetProperty ( deUser, _ldapTitleProp );
      EvUser.OrgId = GetProperty(deUser, _ldapDepartmentProp);
      EvUser.OrganisationName = GetProperty(deUser, _ldapCompanyProp);

      string info = GetProperty ( deUser, _ldapInfo );

      if ( info != null )
      {
        string [ ] parts = info.Split ( ';' );

        EvUser.PasswordResetToken = parts [ 0 ];

        string resetExpiry = parts [ 1 ];

        if ( resetExpiry != null && resetExpiry != String.Empty )
        {
          EvUser.PasswordResetTokenExpiry = DateTime.ParseExact ( resetExpiry, "s", System.Globalization.CultureInfo.InvariantCulture );
        }
      }
    }

    //  ==================================================================================
    /// <summary>
    /// SetAdditionalUserProperty
    /// To handle additional ADS properties which EvAdsFacade could not
    /// </summary>
    /// <param name="EvUser">EvAdsUserProfile.</param>
    /// <example>
    /// <code numberLines="true">
    /// string userId = "User" + Guid.NewGuid().ToString();
    /// EvAdsUserProfile evUser = new EvAdsUserProfile()
    /// {
    ///    EmailAddress = @"test@evado.com.au",
    ///    SamAccountName = userId
    /// };
    /// evUser.JobTitle = "Tester 1";
    /// evUser.OrgId = "Evado";
    /// evUser.OrganizationName = "Evado Medical Solution";
    /// EvAdConfig config = new EvAdConfig()
    ///		{
    ///			Server = "192.168.10.53",
    ///			ContextType = ContextType.Domain,
    ///		    RootContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    UsersContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    GroupsContainer = "OU=Test Users,DC=evado,DC=local",
    ///		    AdminName = @"evado",
    ///		    AdminPassword = "#######",
    ///		    LdapString = "LDAP://192.168.10.53/dc=evado,dc=local"
    ///     };
    ///   
    ///   
    /// EvAdsLdapHelper helper = new EvAdsLdapHelper(config.LdapString, config.AdminName, config.AdminPassword);
    /// helper.SetAdditionalUserProperty(evUser);
    /// </code>
    /// </example>
    //  ---------------------------------------------------------------------------------- 
    public void SetAdditionalUserProperty ( EvAdsUserProfile EvUser )
    {
      DirectoryEntry deUser = GetDirectoryEntry ( EvUser );

      string passwordResetTokenExpiry = String.Empty;

      if ( EvUser.PasswordResetTokenExpiry != null )
      {
        passwordResetTokenExpiry = EvUser.PasswordResetTokenExpiry.Value.ToString ( "s", System.Globalization.CultureInfo.InvariantCulture );
      }

      if ( deUser != null )
      {
        if ( String.IsNullOrEmpty ( EvUser.JobTitle ) == false )
        {
          SetProperty ( deUser, _ldapTitleProp, EvUser.JobTitle );
        }
        if ( String.IsNullOrEmpty ( EvUser.OrgId ) == false )
        {
          SetProperty ( deUser, _ldapDepartmentProp, EvUser.OrgId );
        }
        if ( String.IsNullOrEmpty ( EvUser.OrgId ) == false )
        {
          SetProperty ( deUser, _ldapCompanyProp, EvUser.OrganisationName );
        }
        if ( String.IsNullOrEmpty ( EvUser.PasswordResetToken ) == false
          && String.IsNullOrEmpty ( passwordResetTokenExpiry ) == false )
        {
        SetProperty ( deUser, _ldapInfo, EvUser.PasswordResetToken + ";" + passwordResetTokenExpiry );


      }
        deUser.CommitChanges ( );
      }
    }
    #endregion //Public Methods

    #region Private Methods
    //  ==================================================================================
    /// <summary>
    /// Get ADS Property Using LDAP
    /// </summary>
    /// <param name="DE">DirectoryEntry.</param>
    /// <param name="PropertyName">string.</param>
    /// <returns>string.</returns>
    //  ---------------------------------------------------------------------------------- 
    private string GetProperty ( DirectoryEntry DE, string PropertyName )
    {
      string propertyValue = null;

      if ( DE.Properties [ PropertyName ].Value != null )
      {
        propertyValue = DE.Properties [ PropertyName ].Value.ToString ( );
      }
      return propertyValue;
    }

    //  ==================================================================================
    /// <summary>
    /// Get ADS Property Using LDAP
    /// </summary>
    /// <param name="DE">DirectoryEntry.</param>
    /// <param name="PropertyName">string.</param>
    /// <param name="NewValue">string.</param>
    //  ---------------------------------------------------------------------------------- 
    private void SetProperty ( DirectoryEntry DE, string PropertyName, string NewValue )
    {
      if ( DE != null )
      {
        DE.Properties [ PropertyName ].Value = NewValue == string.Empty ? null : NewValue;
      }
    }

    //  ==================================================================================
    /// <summary>
    /// Get DirectoryEntry for User after finding out with SamAccountName
    /// </summary>
    /// <param name="DE">DirectoryEntry.</param>
    /// <param name="PropertyName">string.</param>
    /// <param name="NewValue">string.</param>
    //  ---------------------------------------------------------------------------------- 
    private DirectoryEntry GetDirectoryEntry ( EvAdsUserProfile EvUser )
    {
      //
      // Find user by SamAccountName
      //
      DirectorySearcher deSearch = this.GetDirectorySearcher ( );

      if ( deSearch == null )
      {
        return null;
      }
      deSearch.Filter = SearchFilter ( "user", EvUser.SamAccountName );
      SearchResult result = deSearch.FindOne ( );

      DirectoryEntry deUser = result.GetDirectoryEntry ( );
      return deUser;
    }

    //  ==================================================================================
    /// <summary>
    /// Get DirectoryEntry for User after finding out with SamAccountName
    /// </summary>
    /// <param name="DE">DirectoryEntry.</param>
    /// <param name="PropertyName">string.</param>
    /// <param name="NewValue">string.</param>
    //  ---------------------------------------------------------------------------------- 
    private DirectorySearcher GetDirectorySearcher ( )
    {
      //
      // Exit if null
      //
      if ( String.IsNullOrEmpty ( _ldapRootPath ) == false
        || String.IsNullOrEmpty ( _username ) == false
        || String.IsNullOrEmpty ( _password ) == false )
      {
        return null;
      }

      //
      // Instantiate DirectorySearcher to find user in ADS
      //
      DirectoryEntry de = new DirectoryEntry ( );
      de.Path = _ldapRootPath;
      de.Username = _username;
      de.Password = _password;
      de.AuthenticationType = AuthenticationTypes.Secure;

      DirectorySearcher deSearch = new DirectorySearcher ( );
      deSearch.SearchRoot = de;

      return deSearch;
    }

    //  ==================================================================================
    /// <summary>
    /// Get Searchfilter to find DirectoryEntry for a User with SamAccountName
    /// </summary>
    /// <param name="DE">DirectoryEntry.</param>
    /// <param name="PropertyName">string.</param>
    /// <param name="NewValue">string.</param>
    //  ---------------------------------------------------------------------------------- 
    private string SearchFilter ( string ObjectType, string CN )
    {
      return string.Format ( "(&(objectClass={0}) (sAMAccountName={1}))", ObjectType, CN );
    }
    #endregion //Private Methods
  }
}