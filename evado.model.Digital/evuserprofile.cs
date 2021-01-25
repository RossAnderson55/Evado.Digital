/***************************************************************************************
 * <copyright file="model\EvUserProfile.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvUserProfile data object.
 *
 ****************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;


namespace Evado.Model.Digital
{
  /// <summary>
  /// Business entity used to model accounts
  /// </summary>
  [Serializable]
  public class EvUserProfile : Evado.Model.EvUserProfileBase
  {
    #region Initialisation method.
    //==================================================================================
    /// <summary>
    /// This method creates a user profile.
    /// </summary>
    //-----------------------------------------------------------------------------------
    public EvUserProfile ( )
    {
    }

    //==================================================================================
    /// <summary>
    /// This method creates a user profile from a BaseUserProfile.
    /// </summary>
    /// <param name="BaseUserProfile">EvUserProfileBase object.</param>
    //-----------------------------------------------------------------------------------
    public EvUserProfile ( EvUserProfileBase BaseUserProfile )
    {
      this.ImportBaseUserProfile ( BaseUserProfile );
    }

    #endregion

    #region Enumerated classes
    /// <summary>
    /// This enumeration list defines field names used in User profile class
    /// </summary>
    public enum UserProfileFieldNames
    {
      /// <summary>
      /// This enumeration defines the null or not selection state
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines the user identifier in user profile
      /// </summary>
      UserId,

      /// <summary>
      /// This enumeration defines the user password in user profile
      /// </summary>
      Password,

      /// <summary>
      /// This enumeration defines the organization idenfier in user profile
      /// </summary>
      User_Type_Id,

      /// <summary>
      /// This enumeration defines Active Directory based on user identifier
      /// </summary>
      ActiveDirectoryUserId,

      /// <summary>
      /// This enumeration defines common name in user profile
      /// </summary>
      Prefix,

      /// <summary>
      /// This enumeration defines common name in user profile
      /// </summary>
      Given_Name,

      /// <summary>
      /// This enumeration defines common name in user profile
      /// </summary>
      Family_Name,

      /// <summary>
      /// This enumeration defines common name in user profile
      /// </summary>
      Suffix,

      /// <summary>
      /// This enumeration defines common name in user profile
      /// </summary>
      CommonName,

      /// <summary>
      /// This enumeration defines email address in user profile 
      /// </summary>
      Email_Address,

      /// <summary>
      /// This enumeration defines role identifier in user profile
      /// </summary>
      RoleId,

      /// <summary>
      /// This enumeration defines title in user profile
      /// </summary>
      Title,

      /// <summary>
      /// This enumeration defines an address field name of an organization
      /// </summary>
      Address_1,

      /// <summary>
      /// This enumeration defines an address field name of an organization
      /// </summary>
      Address_2,

      /// <summary>
      /// This enumeration defines an address city field name of an organization
      /// </summary>
      Address_City,

      /// <summary>
      /// This enumeration defines an address post code field name of an organization
      /// </summary>
      Address_Post_Code,

      /// <summary>
      /// This enumeration defines an address state field name of an organization
      /// </summary>
      Address_State,

      /// <summary>
      /// This enumeration defines a country field name of an organization
      /// </summary>
      Address_Country,

      /// <summary>
      /// This enumeration defines a telephone field name of an organization
      /// </summary>
      Telephone,

      /// <summary>
      /// This enumeration defines mobiule phone in user profile 
      /// </summary>
      Mobile_Phone,

      /// <summary>
      /// This enumeration defines a fax phone field name of an organization
      /// </summary>
      Fax_Phone,

      /// <summary>
      /// This enumeration defines a project dashboard components to be displayed to the user.
      /// </summary>
      Project_Dashboard_Components,

      /// <summary>
      /// This enumeration defines a site dashboard components to be displayed to the user.
      /// </summary>
      Site_Dashboard_Components,

      /// <summary>
      /// This enumeration defines a customer No associated with the user.
      /// </summary>
      Customer_No,

      /// <summary>
      /// This enumeration defines a exporty date for  the user.
      /// </summary>
      Expiry_Date,
    }


    /// <summary>
    /// This enumeration list user type code .
    /// These codes control the use access to the platform
    /// </summary>
    public enum UserTypesList
    {
      /// <summary>
      /// This enumeration defines the null or not selection state
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines the Evado users on the platform
      /// </summary>
      Evado,

      /// <summary>
      /// This enumeration defines the Customers users on the platform
      /// </summary>
      Customer,

      /// <summary>
      /// This enumeration defines the End users on the platform
      /// </summary>
      End_User,
    }
    #endregion

    #region Class contants

    public const string CONST_ADMINISTRATOR_ROLE = "ADMIN";
    public const string CONST_MANAGER_ROLE = "MGT";
    public const string CONST_DESIGNER_ROLE = "DSGN";
    public const string CONST_STAFF_ROLE = "STFF";

    public const String CONT_STATIC_ROLES = CONST_ADMINISTRATOR_ROLE + ":"
      + CONST_MANAGER_ROLE + ":"
      + CONST_DESIGNER_ROLE + ":"
      + CONST_STAFF_ROLE; 
    #endregion

    #region Class Properties

    UserTypesList _TypeId = UserTypesList.Customer;
    /// <summary>
    /// This property defines the user type and used to control user access to the platform.
    /// </summary>
    public UserTypesList TypeId
    {
      get { return this._TypeId; }
      set
      {
        this._TypeId = value;
      }
    }

    /// <summary>
    /// This property contains an customer global unique identifier of an organization
    /// This foreign key links the organisation to the customer object.
    /// </summary>
    public Guid CustomerGuid { get; set; }

    /// <summary>
    /// This property contains the customer no for the user is associated with.
    /// This value is used for resolving uploaded user profiles.
    /// </summary>
    public int CustomerNo { get; set; }


    /// <summary>
    /// This property contains the customer object the user is associated with.
    /// </summary>
    public EvCustomer Customer { get; set; }

    private String _Roles = String.Empty;
    /// <summary>
    /// This property defines the user's role 
    /// </summary>
    public String Roles
    {
      get { return this._Roles; }
      set
      {
        this._Roles = value;
      }
    }

    /// <summary>
    /// This property defines the user's active directory role group. 
    /// </summary>
    public String ActiveDirectoryRoleGroup
    {
      get
      {
        return "ROL_" + this._Roles.ToString ( ).ToUpper ( );
      }
    }

    /// <summary>
    /// This property contains the project dashboard components to be displayed to the user.
    /// </summary>
    public String ProjectDashboardComponents
    {
      get
      {
        return this.getParameter ( EvUserProfile.UserProfileFieldNames.Project_Dashboard_Components );
      }
      set
      {
        var debug = this.setParameter ( EvUserProfile.UserProfileFieldNames.Project_Dashboard_Components, value );

        //this.debug += "\r\n"+ debug;
      }
    }

    /// <summary>
    /// This property contains the site dashboard components to be displayed to the user.
    /// </summary>
    public String SiteDashboardComponents
    {
      get
      {
        return this.getParameter ( EvUserProfile.UserProfileFieldNames.Site_Dashboard_Components );
      }
      set
      {
        var debug = this.setParameter ( EvUserProfile.UserProfileFieldNames.Site_Dashboard_Components, value );
        //this.debug += "\r\n" + debug;
      }
    }

    /// <summary>
    /// This property contains a summary of an organization
    /// </summary>
    public string LinkText
    {
      get
      {
        String stLinkText = this.UserId
          + Evado.Model.Digital.EdLabels.Space_Hypen
          + this.CommonName;

        return stLinkText;
      }
    }

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class User Access methods and properties

    /// <summary>
    /// this propety indicated if the user has administration access.
    /// </summary>
    public bool hasEvadoAdministrationAccess
    {
      get
      {
        if ( ( this.TypeId == UserTypesList.Evado )
          && ( this._Roles.Contains ( EvUserProfile.CONST_ADMINISTRATOR_ROLE ) == true ) )
        {
          return true;
        }
        return false;
      }
    }

    /// <summary>
    /// this propety indicated if the user has administration access.
    /// </summary>
    public bool hasEvadoManagementAccess
    {
      get
      {
        if ( ( this.TypeId == UserTypesList.Evado )
          && ( this._Roles.Contains ( EvUserProfile.CONST_ADMINISTRATOR_ROLE ) == true
            || this._Roles.Contains ( EvUserProfile.CONST_DESIGNER_ROLE ) == true
            || this._Roles.Contains ( EvUserProfile.CONST_MANAGER_ROLE ) == true ) )
        {
          return true;
        }
        return false;
      }
    }

    /// <summary>
    /// this propety indicated if the user has administration access.
    /// </summary>
    public bool hasEvadoAccess
    {
      get
      {
        if ( ( this.TypeId == UserTypesList.Evado )
          && ( this._Roles.Contains ( EvUserProfile.CONST_ADMINISTRATOR_ROLE ) == true
             || this._Roles.Contains ( EvUserProfile.CONST_DESIGNER_ROLE ) == true
             || this._Roles.Contains ( EvUserProfile.CONST_MANAGER_ROLE ) == true
             || this._Roles.Contains ( EvUserProfile.CONST_STAFF_ROLE ) == true ) )
        {
          return true;
        }
        return false;
      }
    }

    /// <summary>
    /// this propety indicated if the user has administration access.
    /// </summary>
    public bool hasAdministrationAccess
    {
      get
      {
        if ( ( this.TypeId == UserTypesList.Evado
            || this.TypeId == UserTypesList.Customer )
          && ( this._Roles.Contains ( EvUserProfile.CONST_ADMINISTRATOR_ROLE ) == true ) )
        {
          return true;
        }
        return false;
      }
    }

    /// <summary>
    /// this propety indicated if the user has configuration read access.
    /// </summary>
    public bool hasDesignAccess
    {
      get
      {
        if ( ( this.TypeId == UserTypesList.Evado
            || this.TypeId == UserTypesList.Customer )
          && ( this._Roles.Contains ( EvUserProfile.CONST_ADMINISTRATOR_ROLE ) == true
            || this._Roles.Contains ( EvUserProfile.CONST_DESIGNER_ROLE ) == true ) )
        {
          return true;
        }

        return false;
      }
    }

    /// <summary>
    /// this propety indicated if the user has configuration read access.
    /// </summary>
    public bool hasManagementAccess
    {
      get
      {
        if ( ( this.TypeId == UserTypesList.Evado
            || this.TypeId == UserTypesList.Customer )
          && ( this._Roles.Contains ( EvUserProfile.CONST_ADMINISTRATOR_ROLE ) == true
            || this._Roles.Contains ( EvUserProfile.CONST_MANAGER_ROLE ) == true ) )
        {
          return true;
        }

        return false;
      }
    }

    /// <summary>
    /// this propety indicated if the user has configuration read access.
    /// </summary>
    public bool hasStaffAccess
    {
      get
      {
        if (  ( this.TypeId == UserTypesList.Evado
            || this.TypeId == UserTypesList.Customer )
          && ( this._Roles.Contains ( EvUserProfile.CONST_ADMINISTRATOR_ROLE ) == true
            || this._Roles.Contains ( EvUserProfile.CONST_MANAGER_ROLE ) == true
            || this._Roles.Contains ( EvUserProfile.CONST_DESIGNER_ROLE ) == true
            || this._Roles.Contains ( EvUserProfile.CONST_STAFF_ROLE ) == true ) )
        {
          return true;
        }

        return false;
      }
    }

    /// <summary>
    /// this propety indicated if the user has configuration read access.
    /// </summary>
    public bool hasEndUserAccess
    {
      get
      {
        if ( this.TypeId == UserTypesList.End_User )
        {
          return true;
        }

        return false;
      }
    }

    #endregion

    #region Class General Methods

    // =====================================================================================
    /// <summary>
    /// This method test to see if the user has a role contain in the roles delimited list.
    /// </summary>
    /// <param name="Roles">';' delimted string of roles</param>
    /// <returns>True: if the role exists.</returns>
    // -------------------------------------------------------------------------------------
    public bool hasEndUserRole ( String Roles )
    {
      foreach ( String role in Roles.Split ( ';' ) )
      {
        foreach ( String role1 in this._Roles.Split ( ';' ) )
        {
          if ( role1.ToLower ( ) == role.ToLower ( ) )
          {
            return true;
          }
        }
      }
      return false;
    }//END method

    // =====================================================================================
    /// <summary>
    /// This class gets the study profile for the user.
    /// </summary>
    /// <returns>string: a user profile string</returns>
    // -------------------------------------------------------------------------------------
    public string getUserProfile ( bool Roles )
    {
      //
      // Initialize a return text
      //
      StringBuilder sbText = new StringBuilder ( );

      //
      // Append user profile elements to the return text
      //
      sbText.AppendLine ( "User Profile for UserId: " + this.UserId );
      if ( this.Password != String.Empty )
      {
        sbText.AppendLine ( "Password: " + this.Password );
      }
      sbText.AppendLine ( "Prefix: " + this.Prefix );
      sbText.AppendLine ( "GivenName: " + this.GivenName );
      sbText.AppendLine ( "FamilyName: " + this.FamilyName );
      sbText.AppendLine ( "CommonName: " + this.CommonName );
      sbText.AppendLine ( "Title: " + this.Title );
      sbText.AppendLine ( "EmailAddress: " + this.EmailAddress );
      sbText.AppendLine ( "RoleId: " + this.Roles );


      if ( this.Customer != null )
      {
        sbText.AppendLine ( "Customer No : " + this.Customer.CustomerNo );
        sbText.AppendLine ( "Customer Name : " + this.Customer.Name );
      }

      if ( this.DomainGroupNames != String.Empty )
      {
        sbText.AppendLine ( "DomainGroupNames: " + this.DomainGroupNames );
      }

      if ( this.AdsCustomerGroup != String.Empty )
      {
        sbText.AppendLine ( "ADS Customer Group: " + this.AdsCustomerGroup );
      }

      if ( this.AdsRoleGroup != String.Empty )
      {
        sbText.AppendLine ( "ADS Role Group: " + this.AdsRoleGroup );
      }

      if ( Roles == true )
      {
        sbText.AppendLine ( "hasAdministrationAccess: " + this.hasAdministrationAccess );
        sbText.AppendLine ( "hasConfigrationAccess: " + this.hasManagementAccess );
      }

      // 
      // Return the text.
      // 
      return sbText.ToString ( );
    }//END getUserProfile class

    #region Class General Methods
    //====================================================================================
    /// <summary>
    /// This method imports the values from the base user profile.
    /// </summary>
    /// <param name="BaseUserProfile"></param>
    //------------------------------------------------------------------------------------
    public void ImportBaseUserProfile ( EvUserProfileBase BaseUserProfile )
    {
      this.UserId = BaseUserProfile.UserId;
      this.ActiveDirectoryUserId = BaseUserProfile.ActiveDirectoryUserId;
      this.Prefix = BaseUserProfile.Prefix;
      this.GivenName = BaseUserProfile.GivenName;
      this.Suffix = BaseUserProfile.Suffix;
      this.CommonName = BaseUserProfile.CommonName;
      this.EmailAddress = BaseUserProfile.EmailAddress;
      this.Address_1 = BaseUserProfile.Address_1;
      this.Address_2 = BaseUserProfile.Address_2;
      this.AddressCity = BaseUserProfile.AddressCity;
      this.AddressPostCode = BaseUserProfile.AddressPostCode;
      this.AddressState = BaseUserProfile.AddressState;
      this.AddressCountry = BaseUserProfile.AddressCountry;
      this.Telephone = BaseUserProfile.Telephone;
      this.Title = BaseUserProfile.Title;
      this.DomainGroups = BaseUserProfile.DomainGroups;


    }

    //===================================================================================
    /// <summary>
    /// This class defines value of the user profile
    /// </summary>
    /// <param name="fieldName">UserProfileClassFieldNames: an object of user profile class file names</param>
    /// <param name="value">String: a value for updating into the user profile</param>
    /// <returns>int: return zero number</returns>
    /// <remarks>
    /// This class consists of the following steps:
    /// 
    /// 1. Switch to the retrieving field name of user profile 
    /// 
    /// 2. Update the related field name with value
    /// </remarks>
    //-----------------------------------------------------------------------------------
    public EvEventCodes setValue ( UserProfileFieldNames fieldName, String value )
    {
      // this.debug = String.Empty;
      //
      // Update the value of user profile's field name based retrieving fieldname
      //
      switch ( fieldName )
      {
        case UserProfileFieldNames.UserId:
          {
            this.UserId = value;
            break;
          }
        case UserProfileFieldNames.Password:
          {
            this.Password = value;
            break;
          }
        case UserProfileFieldNames.User_Type_Id:
          {
            this.TypeId  = EvStatics.Enumerations.parseEnumValue<UserTypesList>(  value );
            break;
          }
        case UserProfileFieldNames.ActiveDirectoryUserId:
          {
            this.ActiveDirectoryUserId = value;
            break;
          }
        case UserProfileFieldNames.Prefix:
          {
            this.Prefix = value;
            break;
          }
        case UserProfileFieldNames.Given_Name:
          {
            this.GivenName = value;
            break;
          }
        case UserProfileFieldNames.Family_Name:
          {
            this.FamilyName = value;
            break;
          }
        case UserProfileFieldNames.Suffix:
          {
            this.Suffix = value;
            break;
          }
        case UserProfileFieldNames.CommonName:
          {
            this.CommonName = value;
            break;
          }
        case UserProfileFieldNames.Title:
          {
            this.Title = value;
            break;
          }
        case UserProfileFieldNames.Email_Address:
          {
            this.EmailAddress = value;
            break;
          }
        case UserProfileFieldNames.Address_1:
          {
            this.Address_1 = value;
            break;
          }
        case UserProfileFieldNames.Address_2:
          {
            this.Address_2 = value;
            break;
          }
        case UserProfileFieldNames.Address_City:
          {
            this.AddressCity = value;
            break;
          }
        case UserProfileFieldNames.Address_Post_Code:
          {
            this.AddressPostCode = value;
            break;
          }
        case UserProfileFieldNames.Address_Country:
          {
            this.AddressCountry = value;
            break;
          }
        case UserProfileFieldNames.Telephone:
          {
            this.Telephone = value;
            break;
          }
        case UserProfileFieldNames.Mobile_Phone:
          {
            this.MobilePhone = value;
            break;
          }
        case UserProfileFieldNames.Project_Dashboard_Components:
          {
            this.ProjectDashboardComponents = value;
            break;
          }
        case UserProfileFieldNames.Site_Dashboard_Components:
          {
            this.SiteDashboardComponents = value;
            break;
          }
        case UserProfileFieldNames.RoleId:
          {
            this.Roles = value;
            break;
          }
        case UserProfileFieldNames.Customer_No:
          {
            this.CustomerNo = EvStatics.getInteger ( value );
            break;
          }
        case UserProfileFieldNames.Expiry_Date:
          {
            this.ExpiryDate = EvStatics.getDateTime ( value );
            break;
          }
      }//End switch field name
      return 0;
    }//END EvEventCodes method

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #endregion

    #region static Methods

    //  ==================================================================================
    /// <summary>
    /// This methods listgs the static user roles.
    /// </summary>
    //  ---------------------------------------------------------------------------------
    public static List<EvOption> GetUserTypeOptionList( bool IsSelectionList)
    {
        List<EvOption> optionList = new List<EvOption> ( );
        if ( IsSelectionList == true )
        {
          optionList.Add ( new EvOption ( ) );
        }

        optionList.Add ( new EvOption ( UserTypesList.End_User, UserTypesList.End_User.ToString().Replace( "_","") ) );
        optionList.Add ( new EvOption ( UserTypesList.Customer,  UserTypesList.Customer.ToString()) );
        optionList.Add ( new EvOption ( UserTypesList.Evado, UserTypesList.Evado.ToString()) );

        return optionList;
    }

    //  ==================================================================================
    /// <summary>
    /// This methods listgs the static user roles.
    /// </summary>
    //  ---------------------------------------------------------------------------------
    public static List<EdRole> StaticRoles
    {
      get
      {
        List<EdRole> staticRoles = new List<EdRole> ( );

        staticRoles.Add ( new EdRole ( EvUserProfile.CONST_ADMINISTRATOR_ROLE, "Administrator" ) );
        staticRoles.Add ( new EdRole ( EvUserProfile.CONST_MANAGER_ROLE, "Manager" ) );
        staticRoles.Add ( new EdRole ( EvUserProfile.CONST_DESIGNER_ROLE, "Designer" ) );
        staticRoles.Add ( new EdRole ( EvUserProfile.CONST_STAFF_ROLE, "Staff" ) );

        return staticRoles;
      }
    }
    //  ==================================================================================	
    /// <summary>
    ///  This method removes the domain name from a use id.
    /// </summary>
    /// <param name="OrganisationType">EvOrganisation.OrganisationTypes: the user organisation type.</param>
    /// <param name="IsSelectionList">Bool: True = create a selection lists options</param>
    /// <returns>Strign: A string containing the domain user identifier.</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Create a list of options based on the user's organisation type.
    /// 
    /// 2. Return the role option list.
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public static List<EvOption> getRoleOptionList (
      List<EdRole> ApplicationRoles,
      bool IsSelectionList )
    {
      // 
      // Create an array of domain name for the activity.
      // 
      List<EvOption> optionList = new List<EvOption> ( );

      //
      // Initialise selection list.
      //
      if ( IsSelectionList == true )
      {
        optionList.Add ( new EvOption ( ) );
      }

      foreach ( EdRole role in EvUserProfile.StaticRoles )
      {
        optionList.Add ( new EvOption ( role.RoleId, role.Description ) );
      }

      foreach ( EdRole role in ApplicationRoles )
      {
        var option = new EvOption ( role.RoleId, role.Description );

        if ( optionList.Contains ( option ) == false )
        {
          optionList.Add ( option );
        }
      }

      //
      // If there is no domain exists, return the domain user identifier
      //
      return optionList;
    }
    #endregion

  }//END EvUserProfile class

}//END namespace Evado.Model.Digital