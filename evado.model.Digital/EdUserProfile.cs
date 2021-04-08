/***************************************************************************************
 * <copyright file="model\EdUserProfile.cs" company="EVADO HOLDING PTY. LTD.">
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
    public enum UserSystemTypes
    {
      /// <summary>
      /// This enumeration defines the null or not selection state
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines the organisation identifier in user profile
      /// </summary>
      Evado,

      /// <summary>
      /// This enumeration defines the user image or photo filename.
      /// </summary>
      Customer,

      /// <summary>
      /// This enumeration defines the user identifier in user profile
      /// </summary>
      End_User,
    }

    /// <summary>
    /// This enumeration list defines field names used in User profile class
    /// </summary>
    public enum FieldNames
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
      /// This enumeration defines the customer defined user category in user profile
      /// </summary>
      User_Category,

      /// <summary>
      /// This enumeration defines the customer defined user type in user profile
      /// </summary>
      User_Type,

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
      Delimted_Name,

      /// <summary>
      /// This enumeration defines Given name in user profile
      /// </summary>
      Given_Name,

      /// <summary>
      /// This enumeration defines middle name in user profile
      /// </summary>
      Middle_Name,

      /// <summary>
      /// This enumeration defines family name in user profile
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

    /// <summary>
    /// This property contains delimited ';' use name
    /// </summary>
    public string DelimitedName
    {
      get
      {
        return this.Prefix + ";" + this.GivenName + ";" + this.MiddleName + ";" + this.FamilyName + ";";
      }
      set
      {
        string [ ] arName = value.Split ( ';' );

        switch ( arName.Length )
        {
          case 1:
            {
              this.FamilyName = arName [ 0 ];
              break;
            }
          case 2:
            {
              this.GivenName = arName [ 0 ];
              this.FamilyName = arName [ 1 ];
              break;
            }
          case 3:
            {
              this.Prefix = arName [ 0 ];
              this.GivenName = arName [ 1 ];
              this.FamilyName = arName [ 2 ];
              break;
            }
          case 4:
            {
              this.Prefix = arName [ 0 ];
              this.GivenName = arName [ 1 ];
              this.MiddleName = arName [ 2 ];
              this.FamilyName = arName [ 3 ];
              break;
            }
        }

      }
    }
    /// This property contains the user home page layout object.
    /// </summary>
    public EdPageLayout HomePage { get; set; }

    /// <summary>
    /// This property defines the user type and used to control user access to the platform.
    /// </summary>
    public String UserCategory { get; set; }

    /// <summary>
    /// This property defines the user type and used to control user access to the platform.
    /// </summary>
    public String UserType { get; set; }

    /// <summary>
    /// This property contains the image (logo) filename for the user.
    /// </summary>
    public string ImageFileName { get; set; }

    /// <summary>
    /// This property contains the current image (logo) filename for the user.
    /// </summary>
    public string CurrentImageFileName { get; set; }

    /// <summary>
    /// This property contains the user's organisation type
    /// </summary>
    public string OrgType { get; set; }

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
        return this.getParameter ( EdUserProfile.FieldNames.Default_Display_Parameters );
      }
      set
      {
        var debug = this.setParameter ( EdUserProfile.FieldNames.Default_Display_Parameters, value );
      }
    }

    /// <summary>
    /// This property contains the current display parameters displayed to the user.
    /// </summary>
    public String CurrentDisplayParameters
    {
      get
      {
        return this.getParameter ( EdUserProfile.FieldNames.Current_Display_Parameters );
      }
      set
      {
        var debug = this.setParameter ( EdUserProfile.FieldNames.Current_Display_Parameters, value );
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
        if ( ( this.OrgId == "Evado" )
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
        if ( ( this.OrgId == "Evado" )
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
        if ( ( this.OrgId == "Evado" )
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
        if ( ( this.OrgId == "Evado" )
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
        if ( ( this.OrgId == "Evado"
            || this.OrgType == "Customer" )
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
        if ( ( this.OrgId == "Evado"
            || this.OrgType == "Customer" )
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
        if ( ( this.OrgId == "Evado"
            || this.OrgType == "Customer" )
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
    /// this propety indicated if the user has end user access.
    /// </summary>
    public bool hasEndUserAccess
    {
      get
      {
        if ( this.OrgType != "Evado"
            || this.OrgType != "Customer" )
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
      sbText.AppendLine ( "OrgType: " + this.OrgType );
      sbText.AppendLine ( "TypeId: " + this.UserType );
      sbText.AppendLine ( "UserCategory: " + this.UserCategory );

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
    public EvEventCodes setValue ( FieldNames fieldName, String value )
    {
      // this.debug = String.Empty;
      //
      // Update the value of user profile's field name based retrieving fieldname
      //
      switch ( fieldName )
      {
        case FieldNames.OrgId:
          {
            this.OrgId = value;
            break;
          }
        case FieldNames.UserId:
          {
            this.UserId = value;
            break;
          }
        case FieldNames.Password:
          {
            this.Password = value;
            break;
          }
        case FieldNames.Image_File_Name:
          {
            this.ImageFileName = value;
            break;
          }
        case FieldNames.User_Type:
          {
            this.UserType = value ;
            break;
          }
        case FieldNames.User_Category:
          {
            this.UserCategory = value;
            break;
          }
        case FieldNames.ActiveDirectoryUserId:
          {
            this.ActiveDirectoryUserId = value;
            break;
          }
        case FieldNames.Prefix:
          {
            this.Prefix = value;
            break;
          }
        case FieldNames.Delimted_Name:
          {
           this.DelimitedName = value;
            break;
          }
        case FieldNames.Given_Name:
          {
            this.GivenName = value;
            break;
          }
        case FieldNames.Middle_Name:
          {
            this.MiddleName = value;
            break;
          }
        case FieldNames.Family_Name:
          {
            this.FamilyName = value;
            break;
          }
        case FieldNames.Suffix:
          {
            this.Suffix = value;
            break;
          }
        case FieldNames.CommonName:
          {
            this.CommonName = value;
            break;
          }
        case FieldNames.Title:
          {
            this.Title = value;
            break;
          }
        case FieldNames.Email_Address:
          {
            this.EmailAddress = value;
            break;
          }
        case FieldNames.Address_1:
          {
            this.Address_1 = value;
            break;
          }
        case FieldNames.Address_2:
          {
            this.Address_2 = value;
            break;
          }
        case FieldNames.Address_City:
          {
            this.AddressCity = value;
            break;
          }
        case FieldNames.Address_Post_Code:
          {
            this.AddressPostCode = value;
            break;
          }
        case FieldNames.Address_Country:
          {
            this.AddressCountry = value;
            break;
          }
        case FieldNames.Telephone:
          {
            this.Telephone = value;
            break;
          }
        case FieldNames.Mobile_Phone:
          {
            this.MobilePhone = value;
            break;
          }
        case FieldNames.Default_Display_Parameters:
          {
            this.DefaultDisplayParameters = value;
            break;
          }
        case FieldNames.Current_Display_Parameters:
          {
            this.CurrentDisplayParameters = value;
            break;
          }
        case FieldNames.RoleId:
          {
            this.Roles = value;
            break;
          }
        case FieldNames.Expiry_Date:
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

    #endregion

  }//END EvUserProfile class

}//END namespace Evado.Model.Digital