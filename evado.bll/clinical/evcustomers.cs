/* <copyright file="BLL\EvCustomers.cs" company="EVADO HOLDING PTY. LTD. IT SYSTEMS">
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
  public class EvCustomers : EvBllBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EvCustomers ( )
    {
      this.ClassNameSpace = "Evado.Bll.Clinical.EvCustomers.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EvCustomers ( EvClassParameters Settings )
    {
      this.ClassParameter = Settings;
      this.ClassNameSpace = "Evado.Bll.Clinical.EvCustomers.";

      this._Dal_Customers = new Evado.Dal.Clinical.EvCustomers ( Settings );
    }
    #endregion

    #region Class variables and properties

    private Evado.Dal.Clinical.EvCustomers _Dal_Customers = new Evado.Dal.Clinical.EvCustomers ( );

    #endregion

    #region Class method
    // =====================================================================================
    /// <summary>
    /// This class returns a list of Customers objects
    /// </summary>
    /// <param name="CustomerState">EvCustomer.CustomerStatue enumerated value.</param>
    /// <returns>List of EvCustomer objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving a list of Organization objects based on OrderBy string
    /// 
    /// 2. Return a list of organization objects
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvCustomer> getView (
      EvCustomer.CustomerStates CustomerState )
    {
      this.LogMethod( "getView method." );

      List<EvCustomer> customerList = this._Dal_Customers.getView ( CustomerState,  false );

       this.LogDebugClass(  this._Dal_Customers.Log );

       this.LogMethodEnd ( "getView" );
      return customerList;

    }//END getView method.

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

      List<EvOption> optionList = this._Dal_Customers.getList (
        false );

       this.LogDebug(  this._Dal_Customers.Log );

      return optionList;

    }//END getList method.

    // =====================================================================================
    /// <summary>
    /// This class returns an organization object based on Guid
    /// </summary>
    /// <param name="OrgGuid">Guid: an organization unique identifier</param>
    /// <returns>EvCustomer: an Organisation ResultData object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving an Organization Object based on Guid. 
    /// 
    /// 2. Return an Organization object
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvCustomer getItem ( Guid OrgGuid )
    {
      this.LogMethod ( "getItem method. " );
       this.LogDebug(  "Guid: '" + OrgGuid + "'" );

      EvCustomer organisation = this._Dal_Customers.getItem ( OrgGuid );

       this.LogDebug(  this._Dal_Customers.Log );

      return organisation;

    }//END GetItem method

    // =====================================================================================
    /// <summary>
    /// This class processes the save items on Organization ResultData table. 
    /// </summary>
    /// <param name="Customer">EvCustomer: an Organisation ResultData object</param>
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
    public EvEventCodes saveItem ( EvCustomer Customer )
    {
      this.LogMethod ( "saveItem method." );
       this.LogDebug(  "Guid: " + Customer.Guid );
       this.LogDebug ( "CustomerId: " + Customer.CustomerId );
       this.LogDebug(  "Name: " + Customer.Name );

      // 
      // Define the local variables
      // 
      EvEventCodes iReturn = EvEventCodes.Ok;

      if ( Customer.UserCommonName == String.Empty )
      {
         this.LogDebug(  "User id empty" );
        return EvEventCodes.Identifier_User_Id_Error;
      }

      // 
      // If Faciity Name is null the delete the Organisation ResultData object from the database.
      // 
      if ( Customer.Name == String.Empty
        || Customer.Action == EvCustomer.ActionCodes.Delete_Object )	
      {
        iReturn = this._Dal_Customers.deleteItem ( Customer );
         this.LogDebug(  this._Dal_Customers.Log );
        return iReturn;
      }

      // 
      // If the Organisation ResultData object unique identifier is null or '0' then
      // add the Organisation ResultData object to the database.
      // 
      if ( Customer.Guid == Guid.Empty )
      {
        iReturn = this._Dal_Customers.addItem ( Customer );
         this.LogDebug(  this._Dal_Customers.Log );
        return iReturn;
      }

      //  
      //  Update the Organisation ResultData object in the database.
      //  
      iReturn = this._Dal_Customers.updateItem ( Customer );		// Update existing record.
       this.LogDebug(  this._Dal_Customers.Log );
      return iReturn;

    }//END saveItem class
    #endregion

  }//END EvCustomers class

}//END namespace Evado.Bll.Clinical