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


namespace Evado.Bll.Clinical
{
  /// <summary>
  /// This business object manages the ExternalSelectionLists objects in the system.
  /// </summary>
  public class EvFormFieldSelectionLists
  {
    #region Class Constants
    /// <summary>
    /// This constant defines the save action of formfield selection list object
    /// </summary>
    public const string Action_Saved = "SAVE";

    /// <summary>
    /// This constant defines the reviewed action of formfield selection list object
    /// </summary>
    public const string Action_Reviewed = "REVIEWED";

    /// <summary>
    /// This constant defines the approved action of formfield selection list object
    /// </summary>
    public const string Action_Issued = "APPROVED";

    /// <summary>
    /// This constant defines the withdrawn action of formfield selection list object
    /// </summary>
    public const string Action_Withdrawn = "WITHDRAWN";

    /// <summary>
    /// This constant defines the delete action of formfield selection list object
    /// </summary>
    public const string Action_Deleted = "DELETE";
    #endregion

    #region Class variables and properties
    // 
    // Create instantiate the DAL class 
    // 
    private Evado.Dal.Clinical.EdRecordFieldSelectionLists _dalExternalSelectionLists = new Evado.Dal.Clinical.EdRecordFieldSelectionLists();

    // 
    // The global variables for the busines object.
    //
    private System.Text.StringBuilder _DebugLog = new System.Text.StringBuilder ( );
    /// <summary>
    /// This property contains the method status
    /// </summary>
    public string DebugLog
    {
      get
      {
        return _DebugLog.ToString();
      }
    }

    /// <summary>
    /// This property contains the Html status
    /// </summary>
    public string DebugLog_Html
    {
      get { return DebugLog.Replace("\r\n", "<br/>"); }
    }

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
    public List<EdExternalSelectionList> getView(
      EdExternalSelectionList.SelectionListStates State)
    {
      this._DebugLog.AppendLine( Evado.Model.Digital.EvcStatics.CONST_METHOD_START 
        + "Evado.Bll.Clinical.EvFiledSelectionLists.getView method." );
      this._DebugLog.AppendLine( "State: " + State );

      List<EdExternalSelectionList> view = this._dalExternalSelectionLists.getView(State);
      this._DebugLog.AppendLine ( this._dalExternalSelectionLists.Log );

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
      (EdExternalSelectionList.SelectionListStates State, 
      bool SelectByGuid)
    {
      this._DebugLog.AppendLine ( Evado.Model.Digital.EvcStatics.CONST_METHOD_START
        + "Evado.Bll.Clinical.EvFiledSelectionLists.getList method." );
      this._DebugLog.AppendLine( "State: " + State  );

      List<EvOption> list = this._dalExternalSelectionLists.getList(State, SelectByGuid);
      this._DebugLog.AppendLine( this._dalExternalSelectionLists.Log );

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
    public EdExternalSelectionList getItem(Guid ListGuid)
    {
      this._DebugLog.AppendLine( Evado.Model.Digital.EvcStatics.CONST_METHOD_START 
        + "Evado.Bll.Clinical.EvFiledSelectionLists.getItem method." );
      this._DebugLog.AppendLine( "ListGuid: " + ListGuid );
      // 
      // Initialise the method variables and objects.
      // 
      EdExternalSelectionList item = this._dalExternalSelectionLists.getItem(ListGuid);
      this._DebugLog.AppendLine ( this._dalExternalSelectionLists.Log );

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
    public EdExternalSelectionList getItem(string ListId)
    {
      this._DebugLog.AppendLine( Evado.Model.Digital.EvcStatics.CONST_METHOD_START 
        + "Evado.Bll.Clinical.EvFiledSelectionLists.getItem method.  " );
      this._DebugLog.AppendLine ( "ListId: " + ListId  );

      // 
      // Execute the DAL method to retrieve the ExternalSelectionList object and process the 
      // result.
      // 
      EdExternalSelectionList Item = this._dalExternalSelectionLists.getItem(ListId, true);
      this._DebugLog.AppendLine ( this._dalExternalSelectionLists.Log );

      return Item;

    }//END getItem method

    #endregion

    #region Update ExternalSelectionList object methods

    // ===================================================================================
    /// <summary>
    /// This class saves items to the Formfield selectionlist ResultData table
    /// </summary>
    /// <param name="ExternalList">EvFormFieldSelectionList: a formfield selection list object</param>
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
    public EvEventCodes saveItem(EdExternalSelectionList ExternalList)
    {
      this._DebugLog.AppendLine( Evado.Model.Digital.EvcStatics.CONST_METHOD_START 
        + "Evado.Bll.Clinical.EvFiledSelectionLists.saveItem method " );
      this._DebugLog.AppendLine( "Action: " + ExternalList.Action );
      // 
      // Initialise the local variables
      // 
      EvEventCodes iReturn = EvEventCodes.Ok;

      // 
      // Exit, if the ListId or UserCommonName is empty
      // 
      if (ExternalList.ListId == String.Empty)
      {
        return EvEventCodes.Data_InvalidId_Error;
      }

      if (ExternalList.UserCommonName == String.Empty)
      {
        return EvEventCodes.Identifier_User_Id_Error;
      }

      // 
      // Remove the empty items from the list.
      // 
      for (int count = 0; count < ExternalList.Items.Count; count++)
      {
        EdExternalSelectionList.CodeItem item = (EdExternalSelectionList.CodeItem)ExternalList.Items[count];

        if (item.Value == String.Empty)
        {
          ExternalList.Items.Remove(item);
        }

        // 
        // Reset the item tableColumn.
        // 
        item.No = count;

      }//END item iteration loop.

      // 
      // Determine whether this person has been an author of the trial.  
      // if not add them to the auhors currentSchedule.
      // 
      string sAuthors = "~~" + ExternalList.Authors;
      int iIndex = sAuthors.LastIndexOf(ExternalList.UserCommonName);

      // 
      // Set the author newField if not set.
      // 
      if (ExternalList.Authors.Length == 0)
      {
        ExternalList.Authors += ExternalList.UserCommonName;
      }
      else if (iIndex < 1)
      {
        ExternalList.Authors += ", " + ExternalList.UserCommonName;
      }

      // 
      // If the Action is DELTE and the state is draft.
      // 
      if (ExternalList.Action == EvFormFieldSelectionLists.Action_Deleted
        && ExternalList.State == EdExternalSelectionList.SelectionListStates.Draft)
      {
        iReturn = this._dalExternalSelectionLists.deleteItem(ExternalList);
        this._DebugLog.AppendLine( this._dalExternalSelectionLists.Log );

        return iReturn;
      }

      // 
      // Update the ExternalSelectionList state based on the ExternalSelectionList object Action property.
      // 
      this.updateState(ExternalList);
      this._DebugLog.AppendLine( " Record State: " + ExternalList.State );

      // 
      // If the unique identifier is null then add this object as a new 
      // ExternalSelectionList object.
      // 
      this._DebugLog.AppendLine( "Save Object to Database." );

      if (ExternalList.Guid == Guid.Empty)	// Add new record
      {
        this._DebugLog.AppendLine( "Add ExternalSelectionList to database" );
        iReturn = this._dalExternalSelectionLists.addItem(ExternalList);

        this._DebugLog.AppendLine( this._dalExternalSelectionLists.Log );

        return iReturn;
      }

      // 
      // If the checklist being approved then withdraw the pervious issues checklist
      // 
      if (ExternalList.Action == EvFormFieldSelectionLists.Action_Issued)
      {
        this._DebugLog.AppendLine( "Withdraw the existing checklist" );
        iReturn = this._dalExternalSelectionLists.WithdrawIssuedList(ExternalList);
        this._DebugLog.AppendLine( this._dalExternalSelectionLists.Log );
      }

      // 
      // Update the checklist object.
      // 
      this._DebugLog.AppendLine( "Update ExternalSelectionList " );

      iReturn = this._dalExternalSelectionLists.updateItem(ExternalList);		// Update ExternalSelectionList properties.
      this._DebugLog.AppendLine ( this._dalExternalSelectionLists.Log );
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
    private void updateState(EdExternalSelectionList Item)
    {
      this._DebugLog.AppendLine( Evado.Model.Digital.EvcStatics.CONST_METHOD_START 
        + "Evado.Bll.Clinical.EvFiledSelectionLists.updateState." );
      this._DebugLog.AppendLine ( "Action: " + Item.Action );

      // 
      // Initialise the local variables
      // 
      float fltVersion = 0;
      if (float.TryParse(Item.Version, out fltVersion) == false)
      {
        fltVersion = 0;
      }

      // 
      // If the selectionlist object has an authenticated signoff pass the user id to the 
      // to the DAL layer and DB.
      // 
      string AuthenticatedUserId = String.Empty;
      if (Item.IsAuthenticatedSignature == true)
      {
        AuthenticatedUserId = Item.UpdatedByUserId;
      }

      // 
      // If action is save then minor version increment version and add UpdatedBy entry
      // 
      if (Item.Action == Action_Reviewed)
      {
        // 
        // If action is SourceDataReviewedRecords then:
        //		set state to SourceDataReviewedRecords (CR),
        //		minor version increment,
        //		set reviewer to user name
        //		set ReviewDate to DateTime now.
        //		add ExternalSelectionList UpdatedBy entry.
        // 
        this._DebugLog.AppendLine( " Reviewing ExternalSelectionList" );

        Item.State = EdExternalSelectionList.SelectionListStates.Reviewed;
        ;
        Item.ReviewedBy = Item.UserCommonName;
        Item.ReviewedByUserId = AuthenticatedUserId;
        Item.ReviewDate = DateTime.Now;
        fltVersion += (float)0.01;
        Item.Version = fltVersion.ToString();

        return;
      }

      if (Item.Action == Action_Issued)
      {
        // 
        // If action is Resolved then:
        //		set state to Issued (CI),
        //		major version increment,
        //		set ApprovedBy to user name
        //		set ApprovalDate date to DateTime now.
        //		add ExternalSelectionList UpdatedBy entry.
        // 
        this._DebugLog.AppendLine( " Issuing ExternalSelectionList" );

        Item.State = EdExternalSelectionList.SelectionListStates.Issued;
        Item.ApprovedByUserId = AuthenticatedUserId;
        Item.ApprovedBy = Item.UserCommonName;
        Item.ApprovalDate = DateTime.Now;
        int iVersion = (int)fltVersion + 1;
        Item.Version = iVersion.ToString();

        return;
      }

      if (Item.Action == Action_Withdrawn
        && Item.ApprovedBy != String.Empty)
      {
        // 
        // If action is Witdhrawn then:
        //		set state to Withdrawn (CW),
        //		add ExternalSelectionList UpdatedBy entry.
        // 
        Item.State = EdExternalSelectionList.SelectionListStates.Withdrawn;

        return;
      }

      // 
      // If none of the above occur, save the record
      // 
      this._DebugLog.AppendLine( " Saving ExternalSelectionList. State:" + Item.State );
      if (Item.State == EdExternalSelectionList.SelectionListStates.Null)
      {
        Item.State = EdExternalSelectionList.SelectionListStates.Draft;
      }
      fltVersion += (float)0.01;
      Item.Version = fltVersion.ToString();

      // 
      // Reset the approval values if in draft state.
      // 
      if (Item.State == EdExternalSelectionList.SelectionListStates.Draft)
      {
        Item.ReviewedBy = String.Empty;
        Item.ReviewedByUserId = String.Empty;
        Item.ReviewDate = Evado.Model.Digital.EvcStatics.CONST_DATE_NULL;
        Item.ApprovedBy = String.Empty;
        Item.ApprovedByUserId = String.Empty;
        Item.ApprovalDate = Evado.Model.Digital.EvcStatics.CONST_DATE_NULL;
      }

      return;

    }//END updateState method.

    #endregion

    #region Copy ExternalSelectionList methods

    // ===================================================================================
    /// <summary>
    /// This class copies items from formfield selectionlist object.
    /// </summary>
    /// <param name="ListGuid">Guid: a list's global unique identifier</param>
    /// <param name="UserCommonName">string: a user common name</param>
    /// <returns>EvEventCodes: an event code for copying items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the listGuid or UserCommonName is empty. 
    /// 
    /// 2. Execute the method for retrieving the list of formfield selectionlist object.
    /// 
    /// 3. Exit, if the selectionlist object has no value. 
    /// 
    /// 4. Update the selectionlist object's values 
    /// 
    /// 5. Execute the method for copying the list of formfield selectionlist object. 
    /// 
    /// 6. Return an event code for copying items.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes CopySelectionList(Guid ListGuid, string UserCommonName)
    {
      this._DebugLog.AppendLine( Evado.Model.Digital.EvcStatics.CONST_METHOD_START 
        + "Evado.Bll.Clinical.EvFiledSelectionLists.ExternalSelectionLists:CopyExternalSelectionList method." );
      this._DebugLog.AppendLine( "ListGuid: " + ListGuid );

      // 
      // Initialise the local variables
      // 
      EvEventCodes iReturn = EvEventCodes.Ok;
      EdExternalSelectionList item = new EdExternalSelectionList();

      // 
      // Exit, if the ListGuid or UserCommonName is empty
      // 
      if (ListGuid == Guid.Empty)
      {
        return EvEventCodes.Data_InvalidId_Error;
      }

      if (UserCommonName == String.Empty)
      {
        return EvEventCodes.Identifier_User_Id_Error;
      }

      // 
      // Execute the method to retrieve the Formfield selectionlist object
      // 
      item = this._dalExternalSelectionLists.getItem(ListGuid);
      this._DebugLog.AppendLine( this._dalExternalSelectionLists.Log );

      // 
      // Exit, if the selectionlist object is emtpy
      // 
      if (item.ListId == String.Empty)
      {
        this._DebugLog.AppendLine ( "ListId is Null" );
        return EvEventCodes.Data_Null_Data_Error;
      }

      // 
      // Update and save the object.
      // 
      item.UserCommonName = UserCommonName;
      item.State = EdExternalSelectionList.SelectionListStates.Draft;
      item.ListId += "_cpy";
      item.Title += " (Copy)";
      item.ReviewedBy = String.Empty;
      item.ReviewDate = Evado.Model.Digital.EvcStatics.CONST_DATE_NULL;
      item.ApprovedBy = String.Empty;
      item.ApprovalDate = Evado.Model.Digital.EvcStatics.CONST_DATE_NULL;
      item.Version = "0";

      iReturn = this._dalExternalSelectionLists.CopyList(item);
      this._DebugLog.AppendLine( this._dalExternalSelectionLists.Log );
      // 
      // Return the response.
      // 
      return iReturn;
    }//END CopyExternalSelectionList class.

    #endregion

    #region Revise ExternalSelectionList methods

    // ===================================================================================
    /// <summary>
    /// This class copies items from formfield selectionlist object.
    /// </summary>
    /// <param name="ListGuid">Guid: a list's global unique identifier</param>
    /// <param name="UserCommonName">string: a user common name</param>
    /// <returns>EvEventCodes: an event code for copying items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the listGuid or UserCommonName is empty. 
    /// 
    /// 2. Execute the method for retrieving the list of formfield selectionlist object.
    /// 
    /// 3. Exit, if the ListId is empty. 
    /// 
    /// 4. Update the selectionlist object's values 
    /// 
    /// 5. Execute the method for copying the list of formfield selectionlist object. 
    /// 
    /// 6. Return an event code for copying items.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes ReviseSelectionList(Guid ListGuid, string UserCommonName)
    {
      this._DebugLog.AppendLine( Evado.Model.Digital.EvcStatics.CONST_METHOD_START 
        + "Evado.Bll.Clinical.EvFiledSelectionLists.ExternalSelectionLists:ReviseExternalSelectionList method." );
      this._DebugLog.AppendLine ( " ListGuid: " + ListGuid );
      // 
      // Initialise the local variables
      // 
      EvEventCodes iReturn = EvEventCodes.Ok;
      EdExternalSelectionList item = new EdExternalSelectionList();

      // 
      // Exit, if the listGuid or UserCommonName is empty. 
      // 
      if (ListGuid == Guid.Empty)
      {
        return EvEventCodes.Data_InvalidId_Error;
      }

      if (UserCommonName == String.Empty)
      {
        return EvEventCodes.Identifier_User_Id_Error;
      }

      // 
      // Retrieve the source newField object.
      // 
      item = this._dalExternalSelectionLists.getItem(ListGuid);
      this._DebugLog.AppendLine( this._dalExternalSelectionLists.Log );

      // 
      // Exit, if the listId is empty. 
      // 
      if (item.ListId == String.Empty)
      {
        this._DebugLog.AppendLine( "ListId is Null" );
        return EvEventCodes.Data_Null_Data_Error;
      }

      float fltVersion = 0;
      if (float.TryParse(item.Version, out fltVersion) == false)
      {
        fltVersion = 0;
      }
      fltVersion += (float)0.01;
      item.Version = fltVersion.ToString();

      // 
      // Update and save the object.
      // 
      item.UserCommonName = UserCommonName;
      item.State = EdExternalSelectionList.SelectionListStates.Draft;
      item.ReviewedBy = String.Empty;
      item.ReviewDate = Evado.Model.Digital.EvcStatics.CONST_DATE_NULL;
      item.ApprovedBy = String.Empty;
      item.ApprovalDate = Evado.Model.Digital.EvcStatics.CONST_DATE_NULL;

      iReturn = _dalExternalSelectionLists.CopyList(item);
      this._DebugLog.AppendLine( this._dalExternalSelectionLists.Log );
      // 
      // Return the response.
      // 
      return iReturn;
    } // Close method ReviseExternalSelectionList

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

  }//END EvFormFieldSelectionLists Class.

}//END namespace Evado.Evado.BLL 
