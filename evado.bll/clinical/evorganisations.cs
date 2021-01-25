/* <copyright file="BLL\EvOrganisations.cs" company="EVADO HOLDING PTY. LTD. IT SYSTEMS">
 *     
 *      Copyright (c) 2002 - 2020 EVADO HOLDING PTY. LTD. IT SYSTEMS.  All rights reserved.
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
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;

//Evado. namespace references.
using Evado.Model;
using Evado.Model.Digital;


namespace Evado.Bll.Clinical
{
  /// <summary>
  /// A business Component used to manage Organisations
  /// </summary>
  public class EvOrganisations : EvBllBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EvOrganisations ( )
    {
      this.ClassNameSpace = "Evado.Bll.Clinical.EvOrganisations.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EvOrganisations ( EvClassParameters Settings )
    {
      this.ClassParameter = Settings;

      this.ClassNameSpace = "Evado.Bll.Clinical.EvOrganisations.";

      this.LogDebug ( "Settings:" );
      this.LogDebug ( "-PlatformId: " + this.ClassParameter.PlatformId );
      this.LogDebug ( "-CustomerGuid: " + this.ClassParameter.CustomerGuid );
      this.LogDebug ( "-ApplicationGuid: " + this.ClassParameter.PlatformGuid );

      this._DalOrganisations = new Evado.Dal.Clinical.EvOrganisations ( Settings );
    }
    #endregion

    #region Class variables and properties

    private Evado.Dal.Clinical.EvOrganisations _DalOrganisations = new Evado.Dal.Clinical.EvOrganisations ( );

    #endregion

    #region Class method
    //====================================================================================
    /// <summary>
    /// This class retrieves a user profile with a specific connection String.
    /// </summary>
    /// <returns>list of Evado.Model.Digital.EvOrganisation objects</returns>
    // -------------------------------------------------------------------------------------
    public List<EvOrganisation> getView ( string ConnectionStringKey )
    {
      Evado.Bll.EvStaticSetting.ConnectionStringKey = ConnectionStringKey;

      List<EvOrganisation> organisationList = getView ( );

      Evado.Bll.EvStaticSetting.ResetConnectionString ( );

      return organisationList;
    }
    // =====================================================================================
    /// <summary>
    /// This class returns a list of Organization objects based on OrderBy string
    /// </summary>
    /// <returns>List of EvOrganization: a list of organization object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving a list of Organization objects based on OrderBy string
    /// 
    /// 2. Return a list of organization objects
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOrganisation> getView ()
    {
      this.LogMethod( "getView method." );

      List<EvOrganisation> organisationList = this._DalOrganisations.getView (
        EvOrganisation.OrganisationTypes.Null,
        false );

       this.LogDebug(  this._DalOrganisations.Log );

      return organisationList;

    }//END getMilestoneList method.
    // =====================================================================================
    /// <summary>
    /// This class returns a list of Organization objects based on OrderBy string
    /// </summary>
    /// <param name="OrgType">OrganisationTypes:selected organisation QueryType</param>
    /// <param name="NotOrgType">bool: false = select by organisation QueryType.</param>
    /// <returns>List of EvOrganization: a list of organization object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving a list of Organization objects based on OrderBy string
    /// 
    /// 2. Return a list of organization objects
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOrganisation> getView ( EvOrganisation.OrganisationTypes OrgType, bool NotOrgType )
    {
      this.LogMethod ( "getView method." );
       this.LogDebug(  "Type: " + OrgType );
       this.LogDebug(  "NotOrgType: " + NotOrgType );

      List<EvOrganisation> organisationList = this._DalOrganisations.getView (
        OrgType,
        NotOrgType );

       this.LogDebug(  this._DalOrganisations.Log );

      return organisationList;

    }//END getMilestoneList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of options for organization object based on OrderBy string
    /// </summary>
    /// <returns>List of EvOption: a list of options for organization ResultData objects.</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Execute the method for retrieving the list of options for Organization objects based on OrderBy string
    /// 
    /// 2. Return a list of options for Organization objects
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> getList ( )
    {
      this.LogMethod ( "getList method." );

      List<EvOption> optionList = this._DalOrganisations.getList (
        EvOrganisation.OrganisationTypes.Null,
        false );

       this.LogDebug(  this._DalOrganisations.Log );

      return optionList;

    }//END getList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of options for organization object based on Type and OrderBy string
    /// </summary>
    /// <param name="Type">EvOrganisation.OrganisationTypes: an organization QueryType</param>
    /// <returns>List of EvOption: a list of options for organization ResultData objects.</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Execute the method for retrieving the list of options for Organization objects based on Type and OrderBy string
    /// 
    /// 2. Return a list of options for Organization objects
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> getList ( EvOrganisation.OrganisationTypes Type )
    {
      this.LogMethod ( "getList method. " );
       this.LogDebug(  "Type: " + Type );

      List<EvOption> organisationList = this._DalOrganisations.getList ( 
        Type, 
        false );

       this.LogDebug(  this._DalOrganisations.Log );

      return organisationList;

    }//END getList method

    // =====================================================================================
    /// <summary>
    /// This class returns a list of options for organization object based on the passed parameters
    /// </summary>
    /// <param name="IsCurrent">Boolean: true, if select the current organization</param>
    /// <param name="Type">EvOrganisation.OrganisationTypes: an Organisation QueryType</param>
    /// <returns>List of EvOption: a list of options for organization ResultData objects.</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Execute the method for retrieving the list of options for Organization objects based on the passed parameters
    /// 
    /// 2. Return a list of options for Organization objects
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> getList ( 
      EvOrganisation.OrganisationTypes Type,
      bool IsCurrent )
    {
      this.LogMethod ( "getList method. " );
       this.LogDebug(  "Type: " + Type );
       this.LogDebug(  "IsCurrent: " + IsCurrent );

      List<EvOption> _Organisations = this._DalOrganisations.getList ( 
        Type, 
        IsCurrent );

       this.LogDebug(  this._DalOrganisations.Log );
      return _Organisations;

    }//END getList method.

    // =====================================================================================
    /// <summary>
    /// This class returns an organization object based on Guid
    /// </summary>
    /// <param name="OrgGuid">Guid: an organization unique identifier</param>
    /// <returns>EvOrganisation: an Organisation ResultData object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving an Organization Object based on Guid. 
    /// 
    /// 2. Return an Organization object
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvOrganisation getItem ( Guid OrgGuid )
    {
      this.LogMethod ( "getItem method. " );
       this.LogDebug(  "Guid: '" + OrgGuid + "'" );

      EvOrganisation organisation = this._DalOrganisations.getItem ( OrgGuid );

       this.LogDebug(  this._DalOrganisations.Log );

      return organisation;

    }//END GetItem method

    // =====================================================================================
    /// <summary>
    /// This class returns an organization object based on OrgId
    /// </summary>
    /// <param name="OrgId">string: an organization identifier</param>
    /// <returns>EvOrganisation: an Organisation ResultData object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving an Organization Object based on OrgId. 
    /// 
    /// 2. Return an Organization object
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvOrganisation getItem ( string OrgId )
    {
      this.LogMethod ( "getItem method. OrgId: '" + OrgId + "'" );

      EvOrganisation organisation = this._DalOrganisations.getItem ( OrgId );

       this.LogDebug(  this._DalOrganisations.Log );

      return organisation;

    }//END GetItem method

    // =====================================================================================
    /// <summary>
    /// This class processes the save items on Organization ResultData table. 
    /// </summary>
    /// <param name="Organisation">EvOrganisation: an Organisation ResultData object</param>
    /// <returns>EvEventCodes: an event code for processing save items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the OrgId or UserCommonName is empty
    /// 
    /// 2. Execute the method for deleting items, if the action code is delete. 
    /// 
    /// 3. Execute the method for adding items, if the Uid is empty.
    /// 
    /// 4. Else execute the method for updating items
    /// 
    /// 5. Return an event code of the method execution
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes saveItem ( EvOrganisation Organisation )
    {
      this.LogMethod ( "saveItem method." );
       this.LogDebug(  "Guid: " + Organisation.Guid );
       this.LogDebug(  "OrgId: " + Organisation.OrgId );
       this.LogDebug(  "Name: " + Organisation.Name );
       this.LogDebug(  "OrgType: " + Organisation.OrgType );
       this.LogDebug(  "UserName: " + Organisation.UserCommonName );

      // 
      // Define the local variables
      // 
      EvEventCodes iReturn = EvEventCodes.Ok;

      if ( Organisation.UserCommonName == String.Empty )
      {
         this.LogDebug(  "User id empty" );
        return EvEventCodes.Identifier_User_Id_Error;
      }

      // 
      // If Faciity Name is null the delete the Organisation ResultData object from the database.
      // 
      if ( Organisation.Name == String.Empty
        || Organisation.Action == EvOrganisation.ActionCodes.Delete_Object )	
      {
        iReturn = this._DalOrganisations.deleteItem ( Organisation );
         this.LogDebug(  this._DalOrganisations.Log );
        return iReturn;
      }

      // 
      // If the Organisation ResultData object unique identifier is null or '0' then
      // add the Organisation ResultData object to the database.
      // 
      if ( Organisation.Guid == Guid.Empty )
      {
        iReturn = this._DalOrganisations.addItem ( Organisation );
         this.LogDebug(  this._DalOrganisations.Log );
        return iReturn;
      }

      //  
      //  Update the Organisation ResultData object in the database.
      //  
      iReturn = this._DalOrganisations.updateItem ( Organisation );		// Update existing record.
       this.LogDebug(  this._DalOrganisations.Log );
      return iReturn;

    }//END saveItem class
    #endregion

  }//END EvOrganisations class

}//END namespace Evado.Bll.Clinical