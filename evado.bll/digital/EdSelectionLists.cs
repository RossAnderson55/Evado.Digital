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
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;

//Evado. namespace references.
using Evado.Model;
using Evado.Model.Digital;


namespace Evado.Bll.Digital
{
  /// <summary>
  /// This business object manages the ExternalSelectionLists objects in the system.
  /// </summary>
  public class EdSelectionLists : EvBllBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EdSelectionLists ( )
    {
      this.ClassNameSpace = "Evado.Bll.Digital.EdSelectionLists.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EdSelectionLists ( EvClassParameters Settings )
    {
      this.ClassParameter = Settings;
      this.ClassNameSpace = "Evado.Bll.Digital.EdEntities.";

      this._dal_SelectionLists = new Evado.Dal.Digital.EdSelectionLists ( Settings );
    }
    #endregion

    #region Class variables and properties
    // 
    // Create instantiate the DAL class 
    // 
    private Evado.Dal.Digital.EdSelectionLists _dal_SelectionLists = new Evado.Dal.Digital.EdSelectionLists ( );

    #endregion

    #region ExternalsalectionList Query methods

    // =====================================================================================
    /// <summary>
    /// This class returns a list of formfield selection list objects based on state and OrderBy string
    /// </summary>
    /// <param name="State">EvFormFieldSelectionList.SelectionListStates: the selection list state</param>
    /// <returns>List of EvFormFieldSelectionList: a list of formfield selection list objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method to retrieve the list of formfield selection list objects
    /// 
    /// 2. Return the list of formfield selection list objects
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EdSelectionList> getView (
      EdSelectionList.SelectionListStates State )
    {
      this.LogMethod ( "getView" );
      this.LogDebug ( "State: " + State );

      List<EdSelectionList> view = this._dal_SelectionLists.getView ( State );
      this.LogClass ( this._dal_SelectionLists.Log );

      return view;

    }//End getMilestoneList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of options for the formfield selection list objects
    /// </summary>
    /// <param name="State">EvFormFieldSelectionList.SelectionListStates: a selectionlist state</param>
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
      ( EdSelectionList.SelectionListStates State,
      bool SelectByGuid )
    {
      this.LogMethod ( "getList method." );
      this.LogDebug ( "State: " + State );

      List<EvOption> list = this._dal_SelectionLists.getList ( State, SelectByGuid );
      this.LogClass ( this._dal_SelectionLists.Log );

      return list;

    }//END getList method

    #endregion

    #region Retrieve ExternalSelectionList object methods

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
    public EdSelectionList getItem ( Guid ListGuid )
    {
      this.LogMethod ( "getItem" );
      this.LogDebug ( "ListGuid: " + ListGuid );
      // 
      // Initialise the method variables and objects.
      // 
      EdSelectionList item = this._dal_SelectionLists.getItem ( ListGuid );
      this.LogClass ( this._dal_SelectionLists.Log );

      return item;

    }//END getItem method

    // ===================================================================================
    /// <summary>
    /// This class retrieves the formfield selectionList object based on ListId. 
    /// </summary>
    /// <param name="ListId">string: a list identifier</param>
    /// <returns>EvFormFieldSelectionList: a formfield selectionList object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving a formfield selectionList object based on ListId
    /// 
    /// 2. Return a formfield selectionList object
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EdSelectionList getItem ( string ListId )
    {
      this.LogMethod ( "getItem" );
      this.LogDebug ( "ListId: " + ListId );

      // 
      // Execute the DAL method to retrieve the ExternalSelectionList object and process the 
      // result.
      // 
      EdSelectionList Item = this._dal_SelectionLists.getItem ( ListId, true );
      this.LogClass ( this._dal_SelectionLists.Log );

      return Item;

    }//END getItem method

    #endregion

    #region Update ExternalSelectionList object methods

    // ===================================================================================
    /// <summary>
    /// This class saves items to the Formfield selectionlist ResultData table
    /// </summary>
    /// <param name="SelectionList">EvFormFieldSelectionList: a formfield selection list object</param>
    /// <returns>EvEventCodes: an event code for saving items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the ListId or UserCommonName is empty
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
    public EvEventCodes SaveItem ( EdSelectionList SelectionList )
    {
      this._Log = new System.Text.StringBuilder ( );
      this.LogMethod ( "saveItem" );
      // 
      // Initialise the local variables
      // 
      EvEventCodes iReturn = EvEventCodes.Ok;

      // 
      // If the Action is DELTE and the state is draft.
      // 
      if ( ( SelectionList.Title == String.Empty 
          || SelectionList.Action == EdSelectionList.SaveActions.Delete_Object )
        && SelectionList.Guid != Guid.Empty
        && SelectionList.State == EdSelectionList.SelectionListStates.Draft )
      {
        iReturn = this._dal_SelectionLists.deleteItem ( SelectionList );
        this.LogClass ( this._dal_SelectionLists.Log );

        return iReturn;
      }

      // 
      // Exit, if the ListId or UserCommonName is empty
      // 
      if ( SelectionList.ListId == String.Empty )
      {
        return EvEventCodes.Data_InvalidId_Error;
      }
      if ( SelectionList.State == EdSelectionList.SelectionListStates.Null )
      {
        SelectionList.State = EdSelectionList.SelectionListStates.Draft;
      }

      // 
      // Remove the empty items from the list.
      // 
      for ( int count = 0; count < SelectionList.Items.Count; count++ )
      {
        EdSelectionList.Item item = (EdSelectionList.Item) SelectionList.Items [ count ];

        if ( item.Value == String.Empty )
        {
          SelectionList.Items.Remove ( item );
        }

        // 
        // Reset the item tableColumn.
        // 
        item.No = count;

      }//END item iteration loop.

      if ( SelectionList.Guid == Guid.Empty
        && SelectionList.State == EdSelectionList.SelectionListStates.Draft )
      {
        SelectionList.State = EdSelectionList.SelectionListStates.Draft;

        iReturn = this._dal_SelectionLists.addItem ( SelectionList );
        this.LogClass ( this._dal_SelectionLists.Log );

        return iReturn;
      }


      // 
      // Update the ExternalSelectionList state based on the ExternalSelectionList object Action property.
      // 
      this.updateState ( SelectionList );
      this.LogDebug ( " Record State: " + SelectionList.State );

      // 
      // If the unique identifier is null then add this object as a new 
      // ExternalSelectionList object.
      // 
      this.LogDebug ( "Save Object to Database." );

      if ( SelectionList.Guid == Guid.Empty )	// Add new record
      {
        this.LogDebug ( "Add ExternalSelectionList to database" );
        iReturn = this._dal_SelectionLists.addItem ( SelectionList );

        this.LogClass ( this._dal_SelectionLists.Log );

        return iReturn;
      }

      // 
      // If the checklist being approved then withdraw the pervious issues checklist
      // 
      if ( SelectionList.Action == EdSelectionList.SaveActions.Issue_List )
      {
        this.LogDebug ( "Withdraw the existing checklist" );
        iReturn = this._dal_SelectionLists.WithdrawIssuedList ( SelectionList );
        this.LogClass ( this._dal_SelectionLists.Log );

        SelectionList.State = EdSelectionList.SelectionListStates.Issued;
      }

      // 
      // Update the checklist object.
      // 
      this.LogDebug ( "Update ExternalSelectionList " );

      iReturn = this._dal_SelectionLists.updateItem ( SelectionList );		// Update ExternalSelectionList properties.
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
    private void updateState ( EdSelectionList Item )
    {
      this.LogMethod ( "updateState." );
      this.LogDebug ( "Action: " + Item.Action );

      // 
      // If action is save then minor version increment version and add UpdatedBy entry
      // 
      switch ( Item.Action )
      {
        case EdSelectionList.SaveActions.Draft:
          {
            this.LogDebug ( "Draft list" );

            Item.State = EdSelectionList.SelectionListStates.Draft;

            return;
          }

        case EdSelectionList.SaveActions.Issue_List:
          {
            Item.State = EdSelectionList.SelectionListStates.Draft;
            Item.Version++;

            return;
          }

        case EdSelectionList.SaveActions.Withdraw_List:
          {
            Item.State = EdSelectionList.SelectionListStates.Withdrawn;
            Item.Version++;

            return;
          }
      }//End switch statement

    }//END updateState method.

    #endregion

  }//END EvFormFieldSelectionLists Class.

}//END namespace Evado.Evado.BLL 
