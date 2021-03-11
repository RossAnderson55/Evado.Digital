/***************************************************************************************
 * <copyright file="model\EdUserProfile.cs" company="EVADO HOLDING PTY. LTD.">
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
  public class EdUserProfile : Evado.Model.EvUserProfileBase
  {
    #region Initialisation method.
    //==================================================================================
    /// <summary>
    /// This method creates a user profile.
    /// </summary>
    //-----------------------------------------------------------------------------------
    public EdUserProfile ( )
    {
    }

    //==================================================================================
    /// <summary>
    /// This method creates a user profile from a BaseUserProfile.
    /// </summary>
    /// <param name="BaseUserProfile">EvUserProfileBase object.</param>
    //-----------------------------------------------------------------------------------
    public EdUserProfile ( EvUserProfileBase BaseUserProfile )
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
      /// This enumeration defines the organisation identifier in user profile
      /// </summary>
      OrgId,
      
      /// <summary>
      /// This enumeration defines the user image or photo filename.
      /// </summary>
      Image_File_Name,

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
      Default_Display_Parameters,

      /// <summary>
      /// This enumeration defines a site dashboard components to be displayed to the user.
      /// </summary>
      Current_Display_Parameters,

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

    public const string CONST_ADMINISTRATOR_ROLE = "Administrator";
    public const string CONST_MANAGER_ROLE = "Manager";
    public const string CONST_DESIGNER_ROLE = "Designer";
    public const string CONST_STAFF_ROLE = "Staff";

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

    String _OrgId = String.Empty;
    /// <summary>
    /// This property defines the user type and used to organisation to the platform.
    /// </summary>
    public String OrgId
    {
      get { return this._OrgId; }
      set
      {
        this._OrgId = value;

        if ( this._OrgId.ToLower ( ) == "evado" )
        {
          this._TypeId = UserTypesList.Evado;
        }
      }
    }

    /// <summary>
    /// This property contains the image (logo) filename for the organisation.
    /// </summary>
    public string ImageFileName { get; set; }

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
    /// This property contains the default display parameters to be displayed to the user.
    /// </summary>
    public String DefaultDisplayParameters
    {
      get
      {
        return this.getParameter ( EdUserProfile.UserProfileFieldNames.Default_Display_Parameters );
      }
      set
      {
        var debug = this.setParameter ( EdUserProfile.UserProfileFieldNames.Default_Display_Parameters, value );

        //this.debug += "\r\n"+ debug;
      }
    }

    /// <summary>
    /// This property contains the current display parameters displayed to the user.
    /// </summary>
    public String CurrentDisplayParameters
    {
      get
      {
        return this.getParameter ( EdUserProfile.UserProfileFieldNames.Current_Display_Parameters );
      }
      set
      {
        var debug = this.setParameter ( EdUserProfile.UserProfileFieldNames.Current_Display_Parameters, value );
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
          && ( this._Roles.Contains ( EdUserProfile.CONST_ADMINISTRATOR_ROLE ) == true ) )
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
          && ( this._Roles.Contains ( EdUserProfile.CONST_ADMINISTRATOR_ROLE ) == true
            || this._Roles.Contains ( EdUserProfile.CONST_DESIGNER_ROLE ) == true
            || this._Roles.Contains ( EdUserProfile.CONST_MANAGER_ROLE ) == true ) )
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
          && ( this._Roles.Contains ( EdUserProfile.CONST_ADMINISTRATOR_ROLE ) == true
            || this._Roles.Contains ( EdUserProfile.CONST_DESIGNER_ROLE ) == true
            || this._Roles.Contains ( EdUserProfile.CONST_MANAGER_ROLE ) == true
            || this._Roles.Contains ( EdUserProfile.CONST_STAFF_ROLE ) == true ) )
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
        if ( ( this.TypeId == UserTypesList.Evado )
          && ( this._Roles.Contains ( EdUserProfile.CONST_ADMINISTRATOR_ROLE ) == true ) )
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
          && ( this._Roles.Contains ( EdUserProfile.CONST_ADMINISTRATOR_ROLE ) == true
            || this._Roles.Contains ( EdUserProfile.CONST_DESIGNER_ROLE ) == true ) )
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
          && ( this._Roles.Contains ( EdUserProfile.CONST_ADMINISTRATOR_ROLE ) == true
            || this._Roles.Contains ( EdUserProfile.CONST_MANAGER_ROLE ) == true ) )
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
        if ( ( this.TypeId == UserTypesList.Evado
            || this.TypeId == UserTypesList.Customer )
          && ( this._Roles.Contains ( EdUserProfile.CONST_ADMINISTRATOR_ROLE ) == true
            || this._Roles.Contains ( EdUserProfile.CONST_MANAGER_ROLE ) == true
            || this._Roles.Contains ( EdUserProfile.CONST_DESIGNER_ROLE ) == true
            || this._Roles.Contains ( EdUserProfile.CONST_STAFF_ROLE ) == true ) )
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
    public bool hasRole ( String Roles )
    {
      if ( Roles == null )
      {
        return false;
      }
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
      sbText.AppendLine ( "OrgId: " + this.OrgId );
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
      sbText.AppendLine ( "TypeId: " + this.TypeId );

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
        case UserProfileFieldNames.OrgId:
          {
            this.OrgId = value;
            break;
          }
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
        case UserProfileFieldNames.Image_File_Name:
          {
            this.ImageFileName = value;
            break;
          }
        case UserProfileFieldNames.User_Type_Id:
          {
            this.TypeId = EvStatics.parseEnumValue<UserTypesList> ( value );
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
        case UserProfileFieldNames.Default_Display_Parameters:
          {
            this.DefaultDisplayParameters = value;
            break;
          }
        case UserProfileFieldNames.Current_Display_Parameters:
          {
            this.CurrentDisplayParameters = value;
            break;
          }
        case UserProfileFieldNames.RoleId:
          {
            this.Roles = value;
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
    public static List<EvOption> GetUserTypeOptionList ( bool IsSelectionList )
    {
      List<EvOption> optionList = new List<EvOption> ( );
      if ( IsSelectionList == true )
      {
        optionList.Add ( new EvOption ( ) );
      }

      optionList.Add ( new EvOption ( UserTypesList.End_User, UserTypesList.End_User.ToString ( ).Replace ( "_", "" ) ) );
      optionList.Add ( new EvOption ( UserTypesList.Customer, UserTypesList.Customer.ToString ( ) ) );
      optionList.Add ( new EvOption ( UserTypesList.Evado, UserTypesList.Evado.ToString ( ) ) );

      return optionList;
    }

    #endregion

  }//END EvUserProfile class

}//END namespace Evado.Model.Digital