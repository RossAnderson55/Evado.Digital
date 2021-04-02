/***************************************************************************************
 * <copyright file="EvOrganisation.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvOrganisation data object.
 *
 ****************************************************************************************/

using System;
using System.Collections.Generic;

namespace Evado.Model.Digital
{
  /// <summary>
  /// Business entity used to model accounts
  /// </summary>
  [Serializable]
  public class EdOrganisation
  {
    /// <summary>
    /// This enumeration list defines the field names of organization
    /// </summary>
    public enum FieldNames
    {
      /// <summary>
      /// This enumeration defines null value or non selection state
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines an organization identifier field name of an organization
      /// </summary>
      OrgId,

      /// <summary>
      /// This enumeration defines a name field name of an organization
      /// </summary>
      Name,

      /// <summary>
      /// This enumeration defines a image filename for an organization
      /// </summary>
      Image_File_Name,

      /// <summary>
      /// This enumeration defines a organisation type field name of an organization
      /// </summary>
      Org_Type,

      /// <summary>
      /// This enumeration defines an address field delimited address of an organization
      /// </summary>
      Address,

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
      /// This enumeration defines an email field name of an organization
      /// </summary>
      Email_Address,
    }

    /// <summary>
    /// This enumeration list defines the action codes for Trial orgnization object.
    /// </summary>
    public enum ActionCodes
    {
      /// <summary>
      /// This enumeation defines the not select value.
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines the save action for the Trial organization object
      /// </summary>
      Save,

      /// <summary>
      /// This enumeration defines the ssuperseded action for the Trial organization object
      /// </summary>
      Delete_Object
    }

    #region Internal member variables

    private Guid _Guid = Guid.Empty;
    private Guid _OrgGuid = Guid.Empty;
    private string _OrgId = String.Empty;
    private string _Name = String.Empty;
    private string _AddressStreet_1 = String.Empty;
    private string _AddressStreet_2 = String.Empty;
    private string _AddressCity = String.Empty;
    private string _AddressPostCode = String.Empty;
    private string _AddressState = String.Empty;
    private string _Country = String.Empty;
    private string _Telephone = String.Empty;
    private string _EmailAddress = String.Empty;
    private string _OrgType = String.Empty;
    private string _UpdatedBy = String.Empty;
    private DateTime _UpdatedDate = EvcStatics.CONST_DATE_NULL;
    private string _UpdatedByUserId = String.Empty;
    private bool _IsAuthenticatedSignature = false;
    private ActionCodes _Action = ActionCodes.Null;

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region public properties

    /// <summary>
    /// This property contains a global unique identifier of an organization
    /// </summary>
    public Guid Guid
    {
      get
      {
        return this._Guid;
      }
      set
      {
        this._Guid = value;
      }
    }


    /// <summary>
    /// This property contains an organization identifier of an organization
    /// </summary>
    public string OrgId
    {
      get
      {
        return this._OrgId;
      }
      set
      {
        this._OrgId = value;
      }
    }


    /// <summary>
    /// This property contains a name of an organization
    /// </summary>
    public string Name
    {
      get
      {
        return this._Name;
      }
      set
      {
        this._Name = value;
      }
    }

    /// <summary>
    /// This property contains the image (logo) filename for the organisation.
    /// </summary>
    public string ImageFileName { get; set; }

    /// <summary>
    /// This property contains an address of an organization
    /// </summary>
    public string Address
    {
      get
      {
        return this._AddressStreet_1 + ";" 
          + this._AddressStreet_2 + ";" 
          + this._AddressCity + ";" 
          + this._AddressState + ";" 
          + this._AddressPostCode + ";" 
          + this._Country;
      }
      set
      {
        string [ ] arrAddess = value.Split ( ';' );

        if ( arrAddess.Length == 6   )
        {
          this._AddressStreet_1 = arrAddess [ 0 ];
          this._AddressStreet_2 = arrAddess [ 1 ];
          this._AddressCity = arrAddess [ 2 ];
          this._AddressState = arrAddess [ 3 ];
          this._AddressPostCode = arrAddess [ 4 ];
          this._Country = arrAddess [ 5 ];
        }
      }
    }

    /// <summary>
    /// This property contains an address of an organization
    /// </summary>
    public string AddressStreet_1
    {
      get
      {
        return this._AddressStreet_1;
      }
      set
      {
        this._AddressStreet_1 = value;
      }
    }

    /// <summary>
    /// This property contains an address of an organization
    /// </summary>
    public string AddressStreet_2
    {
      get
      {
        return this._AddressStreet_2;
      }
      set
      {
        this._AddressStreet_2 = value;
      }
    }

    /// <summary>
    /// This property contains an address of an organization
    /// </summary>
    public string AddressCity
    {
      get
      {
        return this._AddressCity;
      }
      set
      {
        this._AddressCity = value;
      }
    }

    /// <summary>
    /// This property contains an address of an organization
    /// </summary>
    public string AddressPostCode
    {
      get
      {
        return this._AddressPostCode;
      }
      set
      {
        this._AddressPostCode = value;
      }
    }

    /// <summary>
    /// This property contains an address of an organization
    /// </summary>
    public string AddressState
    {
      get
      {
        return this._AddressState;
      }
      set
      {
        this._AddressState = value;
      }
    }

    /// <summary>
    /// This property contains a country of an organization
    /// </summary>
    public string AddressCountry
    {
      get
      {
        return this._Country;
      }
      set
      {
        this._Country = value;
      }
    }

    /// <summary>
    /// This property contains a telephone of an organization
    /// </summary>
    public string Telephone
    {
      get
      {
        return this._Telephone;
      }
      set
      {
        this._Telephone = value;
      }
    }


    /// <summary>
    /// This property contains an email of an organization
    /// </summary>
    public string EmailAddress
    {
      get
      {
        return this._EmailAddress;
      }
      set
      {
        this._EmailAddress = value;
      }
    }

    /// <summary>
    /// This property contains the organisation type value.
    /// </summary>
    public string OrgType
    {
      get { return _OrgType; }
      set { _OrgType = value; }
    }

    /// <summary>
    /// This property contains a summary of an organization
    /// </summary>
    public string LinkText
    {
      get
      {
        String stLinkText = this._OrgId
          + evado.model.Properties.Resources.Space_Hypen
          + this._Name;

        if ( this._OrgType != String.Empty )
        {
          stLinkText += evado.model.Properties.Resources.Space_Coma
            + evado.model.Properties.Resources.Organisation_Type_Label
            + this._OrgType.Replace( "_", " ") ;
        }

        return stLinkText;
      }
    }

    /// <summary>
    /// This property contains a summary of an organization
    /// </summary>
    public EvOption Option
    {
      get
      {
        if ( this._OrgType == String.Empty )
        {
          return new EvOption (
              this._OrgId,
              this._OrgId
             + evado.model.Properties.Resources.Space_Hypen
             + this._Name );
        }

       return new EvOption (
           this._OrgId,
           this._OrgId
          + evado.model.Properties.Resources.Space_Hypen
          + this._Name
          + evado.model.Properties.Resources.Space_Coma
          + evado.model.Properties.Resources.Organisation_Type_Label
          + this._OrgType.Replace ( "_", " " ) );

      }
    }

    /// <summary>
    /// This property contains an updated string of an organization
    /// </summary>
    public string UpdatedBy
    {
      get
      {
        return this._UpdatedBy;
      }
      set
      {
        this._UpdatedBy = value;
      }
    }
    /// <summary>
    /// This property contains the updated date 
    /// </summary>
    public DateTime UpdatedDate
    {
      get { return _UpdatedDate; }
      set { _UpdatedDate = value; }
    }

    /// <summary>
    /// This property contains a user identifier of those who updates an organization
    /// </summary>
    public string UpdatedByUserId
    {
      get
      {
        return this._UpdatedByUserId;
      }
      set
      {
        this._UpdatedByUserId = value;
      }
    }

    /// <summary>
    /// This property contains indicates whether an organization has authenticated signature
    /// </summary>
    public bool IsAuthenticatedSignature
    {
      get
      {
        return this._IsAuthenticatedSignature;
      }
      set
      {
        this._IsAuthenticatedSignature = value;
      }
    }


    /// <summary>
    /// This property contains save action setting for the object.
    /// </summary>

    public ActionCodes Action
    {
      get { return _Action; }
      set { _Action = value; }
    }

    #endregion

    #region public methods

    // ==================================================================================
    /// <summary>
    /// This method provides a list of trial types.
    /// </summary>
    /// <param name="FieldName">EvOrganisation.OrganisationFieldNames: a Field Name object</param>
    /// <returns>string: a string value</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Switch FieldName and get value defining by the organization field name.
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public string getValue ( EdOrganisation.FieldNames FieldName )
    {
      //
      // Switch FieldName and get value defining by the organization field name.
      //
      switch ( FieldName )
      {
        case EdOrganisation.FieldNames.OrgId:
          return this._OrgId;
        case EdOrganisation.FieldNames.Name:
          return this._Name;

        case EdOrganisation.FieldNames.Image_File_Name:
          {
            return this.ImageFileName;
          }

        case EdOrganisation.FieldNames.Org_Type:
          return this._OrgType;

        case EdOrganisation.FieldNames.Address:
          return this.Address;

        case EdOrganisation.FieldNames.Address_1:
          return this.AddressStreet_1;

        case EdOrganisation.FieldNames.Address_2:
          return this.AddressStreet_2;

        case EdOrganisation.FieldNames.Address_City:
          return this.AddressCity;

        case EdOrganisation.FieldNames.Address_State:
          return this.AddressState;

        case EdOrganisation.FieldNames.Address_Post_Code:
          return this.AddressPostCode;

        case EdOrganisation.FieldNames.Address_Country:
          return this.AddressCountry;

        case EdOrganisation.FieldNames.Telephone:
          return this._Telephone;

        case EdOrganisation.FieldNames.Email_Address:
          return this._EmailAddress;
        default:
          return String.Empty;

      }//END Switch

    }//END getValue method

    // ==================================================================================
    /// <summary>
    /// This method sets the field value.
    /// </summary>
    /// <param name="FieldName">EvOrganisation.OrganisationFieldNames: a field name object</param>
    /// <param name="Value">string: a Value for updating</param>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize the internal variables
    /// 
    /// 2. Switch the FieldName and update the Value on the organization field names.
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public void setValue ( EdOrganisation.FieldNames FieldName, string Value )
    {
      //
      // Initialize the internal variables
      //

      //
      // Switch the FieldName and update the Value on the organization field names.
      //
      switch ( FieldName )
      {
        case EdOrganisation.FieldNames.OrgId:
          {
            this._OrgId = Value;
            return;
          }
        case EdOrganisation.FieldNames.Name:
          {
            this._Name = Value;
            return;
          }
        case EdOrganisation.FieldNames.Image_File_Name:
          {
            this.ImageFileName = Value;
            return;
          }
        case EdOrganisation.FieldNames.Address:
          {
            this.Address = Value;
          } return;

        case EdOrganisation.FieldNames.Address_1:
          {
            this._AddressStreet_1 = Value;
            return;
          }
        case EdOrganisation.FieldNames.Address_2:
          {
            this._AddressStreet_2 = Value;
            return;
          }
        case EdOrganisation.FieldNames.Address_City:
          {
            this._AddressCity = Value;
            return;
          }
        case EdOrganisation.FieldNames.Address_State:
          {
            this._AddressState = Value;
            return;
          }
        case EdOrganisation.FieldNames.Address_Post_Code:
          {
            this._AddressPostCode = Value;
            return;
          }
        case EdOrganisation.FieldNames.Address_Country:
          this._Country = Value;
          return;

        case EdOrganisation.FieldNames.Telephone:
          {
            this._Telephone = Value;
            return;
          }

        case EdOrganisation.FieldNames.Email_Address:
          {
            this._EmailAddress = Value;
            return;
          }

        case EdOrganisation.FieldNames.Org_Type:
          {
            this._OrgType =  Value;
            return;
          }

        default:

          return;

      }//END Switch

    }//END setValue method 

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Static methods

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

  }//END EvOrganisation class

}//END namespace Evado.Model.Clinical
