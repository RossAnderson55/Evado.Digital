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
      OrgId,

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
    #endregion

    #region Class Properties

    /// <summary>
    /// This property contains an customer global unique identifier of an organization
    /// This foreign key links the organisation to the customer object.
    /// </summary>
    public Guid CustomerGuid { get; set; }

   // public string debug = String.Empty;

    private Guid _PatientGuid = Guid.Empty;

    /// <summary>
    /// This property contains the patient objects guid as a foreign key.
    /// </summary>
    public Guid PatientGuid
    {
      get { return _PatientGuid; }
      set { this._PatientGuid = value; }
    }

    /// <summary>
    /// This property contains the customer no for the user is associated with.
    /// This value is used for resolving uploaded user profiles.
    /// </summary>
    public int CustomerNo { get; set; }


    /// <summary>
    /// This property contains the customer object the user is associated with.
    /// </summary>
    public EvCustomer Customer { get; set; }

    private string _SignoffAnnotation = String.Empty;
    /// <summary>
    /// This property contains the signoff annotation for the user.
    /// </summary>
    public string SignoffAnnotation
    {
      get { return _SignoffAnnotation; }
      set { _SignoffAnnotation = value; }
    }

    private EvRoleList _RoleId = EvRoleList.Null;
    /// <summary>
    /// This property defines the user's role 
    /// </summary>
    public EvRoleList RoleId
    {
      get { return this._RoleId; }
      set
      {
        this._RoleId = value;

      }
    }

    /// <summary>
    /// This property defines the user's active directory role group. 
    /// </summary>
    public String ActiveDirectoryRoleGroup
    {
      get
      {
        return "ROL_" + this._RoleId.ToString ( ).ToUpper ( );
      }
    }

    private bool _DisplayQueryStatus = false;
    /// <summary>
    /// This property indicates if the user wants the record views to display query status.
    /// </summary>
    public bool DisplayQueryStatus
    {
      get { return this._DisplayQueryStatus; }
      set { this._DisplayQueryStatus = value; }
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
          + EvLabels.Space_Hypen
          + this.CommonName;

        if ( this._RoleId != EvRoleList.Null )
        {
          stLinkText += EvLabels.Space_Open_Bracket
            + EvLabels.User_Profile_Role_Label
            + this._RoleId
            + EvLabels.Space_Close_Bracket;
        }

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
        if ( this._RoleId == EvRoleList.Evado_Administrator )
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
        if ( this._RoleId == EvRoleList.Evado_Administrator
          || this._RoleId == EvRoleList.Evado_Manager )
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
        if ( this._RoleId == EvRoleList.Evado_Administrator
          || this._RoleId == EvRoleList.Evado_Manager
          || this._RoleId == EvRoleList.Evado_Staff )
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
        if ( this._RoleId == EvRoleList.Evado_Administrator
          || this._RoleId == EvRoleList.Administrator )
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
        if ( this._RoleId == EvRoleList.Evado_Administrator
          || this._RoleId == EvRoleList.Evado_Manager
          || this._RoleId == EvRoleList.Evado_Staff
          || this._RoleId == EvRoleList.Administrator
          || this._RoleId == EvRoleList.Manager )
        {
          return true;
        }

        return false;
      }
    }

    /// <summary>
    /// this propety indicated if the user has configuration read access.
    /// </summary>
    public bool hasManagementEditAccess
    {
      get
      {
        if ( this._RoleId == EvRoleList.Evado_Administrator
          || this._RoleId == EvRoleList.Administrator
          || this._RoleId == EvRoleList.Manager )
        {
          return true;
        }

        return false;
      }
    }

    /// <summary>
    /// this propety indicated if the user has multi site  data read access.
    /// </summary>
    public bool hasMultiSiteAccess
    {
      get
      { 
         // || this._RoleId == EvRoleList.Project_Budget
         // || this._RoleId == EvRoleList.Project_Finance

        if ( this._RoleId == EvRoleList.Evado_Administrator
          || this._RoleId == EvRoleList.Evado_Manager
          || this._RoleId == EvRoleList.Evado_Staff
          || this._RoleId == EvRoleList.Administrator
          || this._RoleId == EvRoleList.Manager
          || this._RoleId == EvRoleList.Coordinator )
        {
          return true;
        }
        return false;
      }
    }

    /// <summary>
    /// this propety indicated if the user has data read access.
    /// </summary>
    public bool hasRecordAccess
    {
      get
      {
        if ( this._RoleId == EvRoleList.Evado_Administrator
          || this._RoleId == EvRoleList.Evado_Manager
          || this._RoleId == EvRoleList.Administrator
          || this._RoleId == EvRoleList.Manager
          || this._RoleId == EvRoleList.Coordinator
          || this._RoleId == EvRoleList.Application_User )
        {
          return true;
        }
        return false;
      }
    }
    /// <summary>
    /// this propety indicated if the user has data edit access.
    /// </summary>
    public bool hasRecordEditAccess
    {
      get
      {
        if (  this._RoleId == EvRoleList.Administrator
          || this._RoleId == EvRoleList.Manager
          || this._RoleId == EvRoleList.Coordinator
          || this._RoleId == EvRoleList.Application_User )
        {
          return true;
        }
        return false;
      }
    }
    
    /// <summary>
    /// this propety indicated if the user is a site only user.
    /// </summary>
    public bool hasApplicationUserAccess
    {
      get
      {
        //
        // If the user is a record author or principal investigator they have record edit access.
        //
        if ( this._RoleId == EvRoleList.Application_User)
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
      sbText.AppendLine ( "RoleId: " + this.RoleId );


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
        sbText.AppendLine ( "hasConfigrationEditAccess: " + this.hasManagementEditAccess );
        sbText.AppendLine ( "hasMultiSiteAccess: " + this.hasMultiSiteAccess );
        sbText.AppendLine ( "hasRecordAccess: " + this.hasRecordAccess );
        sbText.AppendLine ( "hasRecordEditAccess: " + this.hasRecordEditAccess );
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
      this.OrgId = BaseUserProfile.OrgId;
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
        case UserProfileFieldNames.OrgId:
          {
            this.OrgId = value;
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
            if ( value == String.Empty )
            {
              this.RoleId = EvRoleList.Null;
              break;
            }
            this.RoleId = EvcStatics.Enumerations.parseEnumValue<EvRoleList> ( value );
            break;
          }
        case UserProfileFieldNames.Customer_No:
          {
            this.CustomerNo = EvStatics.getInteger (value);
            break;
          }
        case UserProfileFieldNames.Expiry_Date:
          {
            this.ExpiryDate = EvStatics.getDateTime (value);
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
    public static List<EvOption> getRoleList (
      EvOrganisation.OrganisationTypes OrganisationType,
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
        optionList.Add ( new EvOption ( EvRoleList.Null.ToString ( ), String.Empty ) );
      }

      switch ( OrganisationType )
      {
        case EvOrganisation.OrganisationTypes.Evado:
          {
            optionList.Add ( EvcStatics.Enumerations.getOption ( EvRoleList.Evado_Administrator ) );
            optionList.Add ( EvcStatics.Enumerations.getOption ( EvRoleList.Evado_Manager ) );
            optionList.Add ( EvcStatics.Enumerations.getOption ( EvRoleList.Evado_Staff ) );
            break;
          }
        case EvOrganisation.OrganisationTypes.Customer:
          {
            optionList.Add ( EvcStatics.Enumerations.getOption ( EvRoleList.Administrator ) );
            optionList.Add ( EvcStatics.Enumerations.getOption ( EvRoleList.Manager ) );
            break;
          }
        case EvOrganisation.OrganisationTypes.Management:
          {
            optionList.Add ( EvcStatics.Enumerations.getOption ( EvRoleList.Manager ) );
            break;
          }
        case EvOrganisation.OrganisationTypes.Coordinator:
          {
            optionList.Add ( EvcStatics.Enumerations.getOption ( EvRoleList.Coordinator ) );
            break;
          }
        case EvOrganisation.OrganisationTypes.Data_Collection:
          {
            optionList.Add ( EvcStatics.Enumerations.getOption ( EvRoleList.Application_User ) );
            break;
          }
        case EvOrganisation.OrganisationTypes.External:
          {
            optionList.Add ( EvcStatics.Enumerations.getOption ( EvRoleList.Application_User ) );
            break;
          }

      }//END switch statement

      //
      // If there is no domain exists, return the domain user identifier
      //
      return optionList;
    }
    #endregion

  }//END EvUserProfile class

}//END namespace Evado.Model.Digital