/***************************************************************************************
 * <copyright file="BLL\EvFormFieldSelectionLists.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2020 EVADO HOLDING PTY. LTD..  All rights reserved.
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
using Evado.Digital.Model;


namespace Evado.Digital.Bll
{
  /// <summary>
  /// This business object manages thePageLayouts objects in the system.
  /// </summary>
  public class EdPageLayouts : EvBllBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EdPageLayouts ( )
    {
      this.ClassNameSpace = "Evado.Digital.Bll.Digital.EdPageLayouts.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EdPageLayouts ( EvClassParameters Settings )
    {
      this.ClassParameter = Settings;
      this.ClassNameSpace = "Evado.Digital.Bll.Digital.EdEntities.";

      this._dal_SelectionLists = new  Evado.Digital.Dal.EdPageLayouts ( Settings );
    }
    #endregion

    #region Class variables and properties
    // 
    // Create instantiate the DAL class 
    // 
    private  Evado.Digital.Dal.EdPageLayouts _dal_SelectionLists = new  Evado.Digital.Dal.EdPageLayouts ( );

    #endregion

    #region ExternalsalectionList Query methods

    // =====================================================================================
    /// <summary>
    /// This class returns a list of formfield selection list objects based on state and OrderBy string
    /// </summary>
    /// <param name="State">EvFormFieldSelectionList.PageLayoutStates: the selection list state</param>
    /// <returns>List of EvFormFieldSelectionList: a list of formfield selection list objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method to retrieve the list of formfield selection list objects
    /// 
    /// 2. Return the list of formfield selection list objects
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EdPageLayout> getView (
      EdPageLayout.States State )
    {
      this.LogMethod ( "getView" );
      this.LogDebug ( "State: " + State );

      List<EdPageLayout> view = this._dal_SelectionLists.getView ( State );
      this.LogClass ( this._dal_SelectionLists.Log );

      return view;

    }//End getMilestoneList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of options for the formfield selection list objects
    /// </summary>
    /// <param name="State">EvFormFieldSelectionList.PageLayoutStates: a selectionlist state</param>
    /// <param name="SelectByGuid">Boolean: true, if the list is selected by unique identifier</param>
    /// <returns>List of EvOption: a list of options for the formfield selection list objects</returns>
    /// <remarks>
    /// This class consists of the following steps:
    /// 
    /// 1. Execute the method for retrieving a list of options for the formfield selection list objects
    /// 
    /// 2. Return a list of options for the formfield selection list objects.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> getList
      ( EdPageLayout.States State,
      bool SelectByGuid )
    {
      this.LogMethod ( "getList method." );
      this.LogDebug ( "State: " + State );

      List<EvOption> list = this._dal_SelectionLists.getList ( State, SelectByGuid );
      this.LogClass ( this._dal_SelectionLists.Log );

      return list;

    }//END getList method

    #endregion

    #region RetrievePageLayoutt object methods

    // =====================================================================================
    /// <summary>
    /// This class retrieves the formfield selectionList object based on ListGuid. 
    /// </summary>
    /// <param name="ListGuid">Guid: a list's global unique identifier</param>
    /// <returns>EvFormFieldSelectionList: a formfield selectionList object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving a formfield selectionList object based on ListGuid
    /// 
    /// 2. Return a formfield selectionList object
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EdPageLayout getItem ( Guid ListGuid )
    {
      this.LogMethod ( "getItem" );
      this.LogDebug ( "ListGuid: " + ListGuid );
      // 
      // Initialise the method variables and objects.
      // 
      EdPageLayout item = this._dal_SelectionLists.getItem ( ListGuid );
      this.LogClass ( this._dal_SelectionLists.Log );

      return item;

    }//END getItem method

    #endregion

    #region UpdatePageLayout object methods

    // ===================================================================================
    /// <summary>
    /// This class saves items to the Formfield selectionlist ResultData table
    /// </summary>
    /// <param name="PageLayout">EvFormFieldSelectionList: a formfield selection list object</param>
    /// <returns>EvEventCodes: an event code for saving items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the PageId or UserCommonName is empty
    /// 
    /// 2. Loop through the selectionlist object and remove the empty items.
    /// 
    /// 3. Execute the method for deleting items, if the action code is delete. 
    /// 
    /// 4. Execute the method for adding items, if the Uid is not defined. 
    /// 
    /// 5. Else, execute the method for updating items.
    /// 
    /// 6. Return an event code of method execution.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes SaveItem ( EdPageLayout PageLayout )
    {
      this.LogMethod ( "saveItem" );
      // 
      // Initialise the local variables
      // 
      EvEventCodes iReturn = EvEventCodes.Ok;

      // 
      // Exit, if the PageId or UserCommonName is empty
      // 
      if ( PageLayout.PageId == String.Empty )
      {
        return EvEventCodes.Data_InvalidId_Error;
      }
      if ( PageLayout.State == EdPageLayout.States.Null )
      {
        PageLayout.State = EdPageLayout.States.Draft;
      }

      // 
      // If the Action is DELTE and the state is draft.
      // 
      if ( PageLayout.Title == String.Empty
        && PageLayout.Guid != Guid.Empty
        && PageLayout.State == EdPageLayout.States.Draft )
      {
        iReturn = this._dal_SelectionLists.deleteItem ( PageLayout );
        this.LogClass ( this._dal_SelectionLists.Log );

        return iReturn;
      }

      // 
      // Update thePageLayoutt state based on thePageLayoutt object Action property.
      // 
      this.updateState ( PageLayout );
      this.LogDebug ( " Record State: " + PageLayout.State );

      // 
      // If the unique identifier is null then add this object as a new 
      //PageLayoutt object.
      // 
      this.LogDebug ( "Save Object to Database." );

      if ( PageLayout.Guid == Guid.Empty )	// Add new record
      {
        this.LogDebug ( "AddPageLayoutt to database" );
        iReturn = this._dal_SelectionLists.addItem ( PageLayout );

        this.LogClass ( this._dal_SelectionLists.Log );

        return iReturn;
      }

      // 
      // If the checklist being approved then withdraw the pervious issues checklist
      // 
      if ( PageLayout.Action == EdPageLayout.SaveActions.Issue )
      {
        this.LogDebug ( "Withdraw the existing checklist" );
        iReturn = this._dal_SelectionLists.WithdrawIssuedList ( PageLayout );
        this.LogClass ( this._dal_SelectionLists.Log );

        PageLayout.State = EdPageLayout.States.Issued;
      }

      // 
      // Update the checklist object.
      // 
      this.LogDebug ( "UpdatePageLayoutt " );

      iReturn = this._dal_SelectionLists.updateItem ( PageLayout );		// UpdatePageLayoutt properties.
      this.LogClass ( this._dal_SelectionLists.Log );
      return iReturn;

    }//END saveItem class

    // =====================================================================================
    /// <summary>
    /// This class updates the state of formfield selectionlist object
    /// </summary>
    /// <param name="Item">EvFormFieldSelectionList: a formfield selectionlist object</param>
    /// <remarks>
    /// This method consists of the following step: 
    /// 
    /// 1. Update the formfield selectionlist object's items and its state based on its action codes. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private void updateState ( EdPageLayout Item )
    {
      this.LogMethod ( "updateState." );
      this.LogDebug ( "Action: " + Item.Action );

      // 
      // If action is save then minor version increment version and add UpdatedBy entry
      // 
      switch ( Item.Action )
      {
        case EdPageLayout.SaveActions.Draft:
          {
            this.LogDebug ( "Draft list" );

            Item.State = EdPageLayout.States.Draft;

            return;
          }

        case EdPageLayout.SaveActions.Issue:
          {
            Item.State = EdPageLayout.States.Draft;
            Item.Version++;

            return;
          }

        case EdPageLayout.SaveActions.Withdraw:
          {
            Item.State = EdPageLayout.States.Withdrawn;
            Item.Version++;

            return;
          }
      }//End switch statement

    }//END updateState method.

    #endregion

  }//END EvFormFieldSelectionLists Class.

}//END namespace Evado.Evado.BLL 
