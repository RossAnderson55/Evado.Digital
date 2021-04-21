/***************************************************************************************
 * <copyright file="dal\EvFormFieldSelectionLists.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2021 EVADO HOLDING PTY. LTD..  All rights reserved.
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
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;

//References to Evado specific libraries
using Evado.Model;
using Evado.Model.Digital;


namespace Evado.Dal.Digital
{
  /// <summary>
  /// This class is handles the data access layer for the form field selection list data object.
  /// </summary>
  public class EdSelectionLists : EvDalBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EdSelectionLists ( )
    {
      this.ClassNameSpace = "Evado.Dal.Digital.EdSelectionLists.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="ClassParameters">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EdSelectionLists ( EvClassParameters ClassParameters )
    {
      this.ClassParameters = ClassParameters;
      this.ClassNameSpace = "Evado.Dal.Digital.EdSelectionLists.";
    }

    #endregion

    #region Object Initialisation

    /* *********************************************************************************
     * 
     * Defines the classes constansts and global variables
     * 
     * *********************************************************************************/

    /// <summary>
    /// This constant defines sql query string for selecting all items from FormField selectionlist view.
    /// </summary>
    private const string SQL_VIEW_QUERY = "Select * FROM ED_SELECTION_LISTS ";

    #region Define class Storeprocedure.
    /// <summary>
    /// This constant defines storeprocedure for adding items to formfield selection list table. 
    /// </summary>
    private const string _STORED_PROCEDURE_AddItem = "USR_SELECTION_LIST_ADD";

    /// <summary>
    /// This constant defines storeprocedure for updating items on formfield selection list table
    /// </summary>
    private const string _STORED_PROCEDURE_UpdateItem = "USR_SELECTION_LIST_UPDATE";

    /// <summary>
    /// This constant defines storeprocedure for deleting items from formfield selection list table
    /// </summary>
    private const string _STORED_PROCEDURE_DeleteItem = "USR_SELECTION_LIST_DELETE";

    /// <summary>
    /// This constant defines storeprocedure for withdrawing items from formfield selection list table
    /// </summary>
    private const string _STORED_PROCEDURE_WithdrawItem = "USR_SELECTION_LIST_WITHDRAWN";


    private const string DB_GUID = "EDSL_GUID";
    private const String DB_LIST_ID = "EDSL_LIST_ID";
    private const String DB_STATE = "EDSL_STATE";
    private const String DB_TITLE = "EDSL_TITLE";
    private const String DB_DESCRIPTION = "EDSL_DESCRIPTION";
    private const String DB_ITEM_LIST = "EDSL_JSON_LIST";
    private const String DB_VERSION = "EDSL_VERSION";
    private const String DB_UPDATED_BY_USER_ID = "EDSL_UPDATED_BY_USER_ID";
    private const String DB_UPDATED_BY = "EDSL_UPDATED_BY";
    private const String DB_UPDATED_DATE = "EDSL_UPDATED_DATE";
    private const String DB_DELETED = "EDSL_DELETED";

    /// <summary>
    /// This constant defines the parameter for global unique identifier of FormField selection list object
    /// </summary>

    private const String PARM_GUID = "@GUID";
    private const String PARM_LIST_ID = "@LIST_ID";
    private const String PARM_STATE = "@STATE";
    private const String PARM_TITLE = "@TITLE";
    private const String PARM_DESCRIPTION = "@DESCRIPTION";
    private const String PARM_ITEM_LIST = "@JSON_LIST";
    private const String PARM_VERSION = "@VERSION";
    private const String PARM_UPDATED_BY_USER_ID = "@UPDATED_BY_USER_ID";
    private const String PARM_UPDATED_BY = "@UPDATED_BY";
    private const String PARM_UPDATED_DATE = "@UPDATED_DATE";
    #endregion

    #endregion

    #region Set Query Parameters

    // ==================================================================================
    /// <summary>
    /// This class sets the update query properties. 
    /// </summary>
    /// <returns>SqlParameter: an array of sql query parameters</returns>
    /// <remarks>
    /// This method consists of the following step:
    /// 
    /// 1. Create the array of sql query parameters. 
    /// 
    /// 2. Return the array of sql query parameters
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    private static SqlParameter [ ] GetParameters ( )
    {
      SqlParameter [ ] parms = new SqlParameter [ ] 
      {
        new SqlParameter( EdSelectionLists.PARM_GUID, SqlDbType.UniqueIdentifier ),
        new SqlParameter( EdSelectionLists.PARM_LIST_ID, SqlDbType.NVarChar, 20 ),
        new SqlParameter( EdSelectionLists.PARM_STATE, SqlDbType.NVarChar, 20 ),
        new SqlParameter( EdSelectionLists.PARM_TITLE, SqlDbType.NVarChar, 100),
        new SqlParameter( EdSelectionLists.PARM_DESCRIPTION, SqlDbType.NVarChar, 1000 ),
        new SqlParameter( EdSelectionLists.PARM_ITEM_LIST, SqlDbType.NText ),
        new SqlParameter( EdSelectionLists.PARM_VERSION, SqlDbType.Int ),
        new SqlParameter( EdSelectionLists.PARM_UPDATED_BY_USER_ID,SqlDbType.NVarChar, 100 ),
        new SqlParameter( EdSelectionLists.PARM_UPDATED_BY, SqlDbType.NVarChar, 100 ),
        new SqlParameter( EdSelectionLists.PARM_UPDATED_DATE, SqlDbType.DateTime ),
      };
      return parms;
    }//END GetParameters class

    // ==================================================================================
    /// <summary>
    /// This class sets the values to the parameters array.
    /// </summary>
    /// <param name="parms">SqlParameter: an array of sql parameters</param>
    /// <param name="Item">EvFormFieldSelectionList: a formfield selectionlist object</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Add new DB row Guid, if item's Guid is empty
    /// 
    /// 2. Update the items values from formfield selectionlist object to the array of sql parameters.
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    private void SetParameters ( SqlParameter [ ] parms, EdSelectionList Item )
    {
      //
      // Add new DB row Guid, if item's Guid is empty
      //
      if ( Item.Guid == Guid.Empty )
      {
        Item.Guid = Guid.NewGuid ( );
      }

      //
      // Update the items values from formfield selectionlist object to the array of sql parameters.
      //
      parms [ 0 ].Value = Item.Guid;
      parms [ 1 ].Value = Item.ListId;
      parms [ 2 ].Value = Item.State;
      parms [ 3 ].Value = Item.Title;
      parms [ 4 ].Value = Item.Description;
      parms [ 5 ].Value = Evado.Model.EvStatics.SerialiseObject<List<EdSelectionList.Item>> ( Item.Items );
      parms [ 6 ].Value = Item.Version;
      parms [ 7 ].Value = this.ClassParameters.UserProfile.UserId;
      parms [ 8 ].Value = this.ClassParameters.UserProfile.CommonName;
      parms [ 9 ].Value = DateTime.Now;

    }//END SetParameters class.

    #endregion

    #region ExternalSelectionList Reader

    // =====================================================================================
    /// <summary>
    /// This class extracts the content of the reader to the formfield selectionlist object.
    /// </summary>
    /// <param name="Row">DataRow: a data Reader containing the query results</param>
    /// <returns>EvFormFieldSelectionList: a formfield selectionlist object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Extract the compatible data row values to the formfield selectionlist object.
    /// 
    /// 2. Return the Formfield selectionlist object. 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    private EdSelectionList readDataRow ( DataRow Row )
    {
      // 
      // Initialise the formfield selectionlist object.
      // 
      EdSelectionList Item = new EdSelectionList ( );

      //
      // Extract the compatible data row values to the formfield selectionlist object items.
      //
      Item.Guid = EvSqlMethods.getGuid ( Row, EdSelectionLists.DB_GUID );
      Item.ListId = EvSqlMethods.getString ( Row, EdSelectionLists.DB_LIST_ID );
      Item.Title = EvSqlMethods.getString ( Row, EdSelectionLists.DB_TITLE );
      Item.Description = EvSqlMethods.getString ( Row, EdSelectionLists.DB_DESCRIPTION );
      Item.Version = EvSqlMethods.getInteger ( Row, EdSelectionLists.DB_VERSION );

      string xmlCodeItem = EvSqlMethods.getString ( Row, EdSelectionLists.DB_ITEM_LIST );
      if ( xmlCodeItem != String.Empty )
      {
        Item.Items = Evado.Model.Digital.EvcStatics.DeserialiseObject<List<EdSelectionList.Item>> ( xmlCodeItem );
      }
      Item.State = Evado.Model.EvStatics.parseEnumValue<EdSelectionList.SelectionListStates> (
        EvSqlMethods.getString ( Row, EdSelectionLists.DB_STATE ) );
      Item.UpdatedByUserId = EvSqlMethods.getString ( Row, EdSelectionLists.DB_UPDATED_BY_USER_ID );
      Item.UpdatedBy = EvSqlMethods.getString ( Row, EdSelectionLists.DB_UPDATED_BY );
      Item.UpdatedDate += EvSqlMethods.getDateTime ( Row, EdSelectionLists.DB_UPDATED_DATE );

      // 
      // Return item
      // 
      return Item;

    }//END readDataRow method.
    #endregion

    #region SelectionList Queries

    // =====================================================================================
    /// <summary>
    /// This class returns the list of formfield selectionlist object using state and orderBy values
    /// </summary>
    /// <param name="State">EvFormFieldSelectionList.SelectionListStates: an enumeration state</param>
    /// <returns>List of EvFormFieldSelectionList: a list of formfield selectionlist object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define the sql query parameter and sql query string
    /// 
    /// 2. Execute the sql query string with parameters and store the results on datatable. 
    /// 
    /// 3. Iterate through the table and extract datarow to the formfield selectionlist object
    /// 
    /// 4. Add the object values to the Formfield selection list.
    /// 
    /// 5. Return the Formfield selection list. 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public List<EdSelectionList> getView (
      EdSelectionList.SelectionListStates State )
    {
      //
      // Initialize the method status and a return list of formfield selectionlist object
      //
      this.LogMethod ( "getView method." );
      this._Log.AppendLine ( "State: " + State );

      List<EdSelectionList> view = new List<EdSelectionList> ( );
      String sqlQueryString = String.Empty;
      //
      // Define the sql query parameter and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(EdSelectionLists.PARM_STATE, SqlDbType.VarChar, 20)
      };
      cmdParms [ 0 ].Value = State.ToString ( );

      //
      // Define the sql query string. 
      // 
      sqlQueryString = SQL_VIEW_QUERY;
      if ( State != EdSelectionList.SelectionListStates.Null )
      {
        sqlQueryString += " WHERE ( " + EdSelectionLists.DB_STATE + " = " + EdSelectionLists.PARM_STATE + " ) "
          + " AND  (" + EdSelectionLists.DB_DELETED + " = 0 )"
          + " ORDER BY " + EdSelectionLists.DB_LIST_ID + ";";
      }
      else
      {
        sqlQueryString += " WHERE (  " + EdSelectionLists.DB_STATE + " <> '" + EdSelectionList.SelectionListStates.Withdrawn + "' ) "
          + " AND  (" + EdSelectionLists.DB_DELETED + " = 0 )"
          + " ORDER BY " + EdSelectionLists.DB_LIST_ID + ";";
      }


      this.LogDebug ( "SQL Query: " + sqlQueryString );
      //
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
      {
        // 
        // Iterate through the results extracting the role information.
        // 
        for ( int Count = 0; Count < table.Rows.Count; Count++ )
        {
          // 
          // Extract the table row
          // 
          DataRow row = table.Rows [ Count ];

          EdSelectionList selectionList = this.readDataRow ( row );

          // 
          // Append the value to the visit
          // 
          view.Add ( selectionList );

        } //END interation loop.

      }//END using statement

      this.LogDebug ( " view count: " + view.Count.ToString ( ) );
      //
      // return the Array object.
      // 
      return view;

    }// END getView method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of options based on the state and Guid values
    /// </summary>
    /// <param name="State">EvFormFieldSelectionList.SelectionListStates: The selection list state.</param>
    /// <param name="SelectByGuid">Boolean: True, if the selection is based on Guid.</param>
    /// <returns>List of EvOption: a list of option data objects</returns>
    /// <remarks>
    /// This method consists of the followings steps: 
    /// 
    /// 1. Define the sql query parameters and sql query string. 
    /// 
    /// 2. Execute the sql query string and store the results on data table. 
    /// 
    /// 3. Loop through the table and extract the data row to the option object. 
    /// 
    /// 4. Add the Option object values to the Options list. 
    /// 
    /// 5. Return the Options list. 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public List<EvOption> getList (
      EdSelectionList.SelectionListStates State,
      bool SelectByGuid )
    {
      this.LogMethod ( "getList method." );
      this._Log.AppendLine ( "State: " + State );
      this._Log.AppendLine ( "SelectByUid: " + SelectByGuid.ToString ( ) );

      //
      // Initialise the local variables
      //  
      List<EvOption> list = new List<EvOption> ( );
      EvOption option = new EvOption ( );
      list.Add ( option );

      //
      // get the list of selectionLists
      //
      var selectionList = this.getView ( State );

      // 
      // Iterate through the results extracting the role information.
      // 
      for ( int count = 0; count < selectionList.Count; count++ )
      {
        EdSelectionList item = selectionList [ count ];
        //
        // If SelectByGuid = True then optionId is to be the objects TestReport UID
        //
        if ( SelectByGuid == true )
        {
          option = new EvOption ( item.Guid.ToString ( ),
            String.Format ( "{0} - {1} ", item.ListId, item.Title ) );
        }

        //
        // If SelectByGuid = False then optionId is to be the objects ListId.
        //
        else
        {
          option = new EvOption ( item.ListId,
            String.Format ( "{0} - {1} ", item.ListId, item.Title ) );
        }

        list.Add ( option );

        if ( count > this.ClassParameters.MaxResultLength )
        {
          break;
        }

      } //END interation loop.

      //
      // Return the ArrayList.
      //
      return list;

    }//END getList class

    #endregion

    #region Retrieval Queries

    // =====================================================================================
    /// <summary>
    /// This class retrieves the formfield selection list based on Guid value
    /// </summary>
    /// <param name="ListGuid">Guid: The Global unique identifier</param>
    /// <returns>EvFormFieldSelectionList: The formfield selection list object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Return an empty selection list if the list's Guid is empty. 
    /// 
    /// 2. Define the sql query string and sql query parameters. 
    /// 
    /// 3. Execute the sql query string and store the results on data table. 
    /// 
    /// 4. Extract the first row to the formfield selection list object. 
    /// 
    /// 5. Return the formfield selection list data object. 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public EdSelectionList getItem ( Guid ListGuid )
    {
      this.LogMethod ( "getItem" );
      this._Log.AppendLine ( "ListGuid: " + ListGuid );
      //
      // Initialise the local variables
      //
      EdSelectionList item = new EdSelectionList ( );
      String sqlQueryString = String.Empty;

      //
      // If TestReport UID is null return empty checlist object.
      //
      if ( ListGuid == Guid.Empty )
      {
        return item;
      }

      //
      // Initialise the query parameters.
      //
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(EdSelectionLists.PARM_GUID, SqlDbType.UniqueIdentifier ),
      };
      cmdParms [ 0 ].Value = ListGuid;

      // 
      // Generate the Selection query string.
      // 
      sqlQueryString = SQL_VIEW_QUERY + " WHERE " + EdSelectionLists.DB_GUID + " = " + EdSelectionLists.PARM_GUID + ";";

      this.LogDebug ( sqlQueryString );

      //
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
      {
        // 
        // If not rows the return
        // 
        if ( table.Rows.Count == 0 )
        {
          return item;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        item = this.readDataRow ( row );

      }//END Using 

      //
      // Return Checklsit data object.
      //
      return item;

    }//END EvFormFieldSelectionList method

    // ====================================================================================
    /// <summary>
    /// This class retrieves the formfield selection list based on ListId and Issued condition
    /// </summary>
    /// <param name="ListId">string: the selection list identifier</param>
    /// <param name="Issued">Boolean: true, if the selection list is issued.</param>
    /// <returns>EvFormFieldSelectionList: a formfield selection list object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Return an empty selection list if the list's identifier is empty. 
    /// 
    /// 2. Define the sql query string and sql query parameters. 
    /// 
    /// 3. Execute the sql query string and store the results on data table. 
    /// 
    /// 4. Extract the first row to the formfield selection list object. 
    /// 
    /// 5. Return the formfield selection list data object. 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public EdSelectionList getItem (
      string ListId,
      bool Issued )
    {
      this.LogMethod ( "getItem, ListId = " + ListId );
      //
      // Initialise the local variables
      //
      EdSelectionList item = new EdSelectionList ( );
      String sqlQueryString = String.Empty;

      //
      // If the ListId is null then return empty Ethics object.
      //
      if ( ListId == string.Empty )
      {
        return item;
      }

      //
      // Initialise the query parameters.
      //
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(EdSelectionLists.PARM_LIST_ID, SqlDbType.NVarChar, 10),
      };
      cmdParms [ 0 ].Value = ListId;

      // 
      // Generate the Selection query string.
      // 
      sqlQueryString = SQL_VIEW_QUERY + " WHERE (" + EdSelectionLists.DB_LIST_ID + " = " + EdSelectionLists.PARM_LIST_ID + ") "
        + " AND  (" + EdSelectionLists.DB_DELETED + " = 0 )";

      if ( Issued == true )
      {
        sqlQueryString += " AND (" + EdSelectionLists.DB_STATE + " = '" + EdSelectionList.SelectionListStates.Issued + "'); ";
      }

      this.LogDebug ( sqlQueryString );


      //
      //Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
      {
        // 
        // If not rows the return
        // 
        if ( table.Rows.Count == 0 )
        {
          return item;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        item = this.readDataRow ( row );

      }//END Using 

      //
      // Return the TestReport data object.
      //
      return item;

    }// END getItem method

    #endregion

    #region SelectionList Update queries

    // =====================================================================================
    /// <summary>
    /// This class updates items on formfield selection list table. 
    /// </summary>
    /// <param name="Item">EvFormFieldSelectionList: a formfield selection list object</param>
    /// <returns>EvEventCodes: an event code for updating items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the Old list's Guid is empty. 
    /// 
    /// 2. Add items to datachange object if they exist after comparing with the old list. 
    /// 
    /// 3. Define the sql parameters and execute the storeprocedure for updating items. 
    /// 
    /// 4. Add the datachange values to the backup datachanges object. 
    /// 
    /// 5. Return the event code for updating the items. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes updateItem ( EdSelectionList Item )
    {
      this._Log = new System.Text.StringBuilder ( );
      this.LogMethod ( "updateItem" );

      // 
      // Get the previous value
      // 
      EdSelectionList oldItem = getItem ( Item.Guid );
      if ( oldItem.Guid == Guid.Empty )
      {
        return EvEventCodes.Identifier_Global_Unique_Identifier_Error;
      }

      //
      // trime the item options.
      //
      this.trimItemOptions ( Item );

      // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      // Compare the objects.
      // 
      EvDataChanges dataChanges = new EvDataChanges ( );
      EvDataChange dataChange = new EvDataChange ( );
      dataChange.TableName = EvDataChange.DataChangeTableNames.EdSelectionList;
      dataChange.TrialId = String.Empty;
      dataChange.RecordUid = -1;
      dataChange.RecordGuid = Item.Guid;
      dataChange.UserId = Item.UpdatedByUserId;
      dataChange.DateStamp = DateTime.Now;

      //
      // Add items to datachange object if they exist. 
      //
      if ( Item.ListId != oldItem.ListId )
      {
        dataChange.AddItem ( "ListId", oldItem.ListId, Item.ListId );
      }
      if ( Item.Title != oldItem.Title )
      {
        dataChange.AddItem ( "Title", oldItem.Title, Item.Title );
      }
      if ( Item.Description != oldItem.Description )
      {
        dataChange.AddItem ( "Description", oldItem.Description, Item.Description );
      }
      if ( Item.Version != oldItem.Version )
      {
        dataChange.AddItem ( "Version", oldItem.Version, Item.Version );
      }
      if ( Item.State != oldItem.State )
      {
        dataChange.AddItem ( "State", oldItem.State.ToString ( ), Item.State.ToString ( ) );
      }

      string oldCodeItem =
        Evado.Model.Digital.EvcStatics.SerialiseObject<List<EdSelectionList.Item>> ( Item.Items );

      string newCodeItem =
        Evado.Model.Digital.EvcStatics.SerialiseObject<List<EdSelectionList.Item>> ( Item.Items );

      if ( newCodeItem != oldCodeItem )
      {
        dataChange.AddItem ( "Items", oldCodeItem, newCodeItem );
      }
      // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

      // 
      // Define the query parameters
      // 
      SqlParameter [ ] commandParameters = GetParameters ( );
      SetParameters ( commandParameters, Item );

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( _STORED_PROCEDURE_UpdateItem, commandParameters ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }
      // 
      // Add the change record
      // 
      dataChanges.AddItem ( dataChange );

      //
      // Update the option list
      //
      this.updateOptions ( Item );

      return EvEventCodes.Ok;

    }//END updateItem class 

    // =====================================================================================
    /// <summary>
    /// This class adds new items to the formfield selection list table. 
    /// </summary>
    /// <param name="Item">EvFormFieldSelectionList: a formfield selection list object</param>
    /// <returns>EvEventCodes: an event code for adding items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit if the List's identifier is empty or duplicated
    /// 
    /// 2. Create new DB row Guid
    /// 
    /// 3. Define the sql query parameters and execute the sql storeprocedure for adding items. 
    /// 
    /// 4. Return the event code fore adding new items. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes addItem ( EdSelectionList Item )
    {
      this._Log = new System.Text.StringBuilder ( );
      this.LogMethod ( "addItem method. " );
      this.LogDebug ( "ListId: '" + Item.ListId );
      this.LogDebug ( "Version: '" + Item.Version + "'" );
      String sqlQueryString = String.Empty;

      //---------------------- Check for duplicate TestReport identifiers. ------------------

      var oldSelectionList = this.getItem ( Item.ListId, false );
      // 
      // returned list has a guid there is a duplicate.
      // 
      if ( oldSelectionList.Guid != Guid.Empty )
      {
        this.LogDebug ( "Duplication list found." );

        return EvEventCodes.Data_Duplicate_Id_Error;
      }

      //
      // trime the item options.
      //
      this.trimItemOptions ( Item );

      // 
      // Create the Guid
      // 
      Item.Guid = Guid.NewGuid ( );

      // 
      // Define the query parameters
      // 
      SqlParameter [ ] commandParameters = GetParameters ( );
      SetParameters ( commandParameters, Item );

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( _STORED_PROCEDURE_AddItem, commandParameters ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      //
      // Update the option list
      //
      this.updateOptions ( Item );

      //
      // Return the event code for adding new items. 
      //
      return EvEventCodes.Ok;

    }//END addItem class

    // =====================================================================================
    /// <summary>
    /// This class withdraw issued formfield selection list. 
    /// </summary>
    /// <param name="Item">EvFormFieldSelectionList: a formfield selection list object</param>
    /// <returns>EvEventCodes: an event code for withdrawing issued list</returns>
    /// <remarks>
    /// This class consists of the following steps: 
    /// 
    /// 1. Define the sql query parameters and execute the storeprocedure for withdrawing items.
    /// 
    /// 2. Return an event code for withdrawing items. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes WithdrawIssuedList ( EdSelectionList Item )
    {
      this._Log = new System.Text.StringBuilder ( );
      this.LogMethod ( "WithdrawIssuedList method. " );
      // 
      // Define the query parameters
      // 
      SqlParameter [ ] commandParameters = new SqlParameter [ ]
      {
        new SqlParameter(EdSelectionLists.PARM_LIST_ID, SqlDbType.NVarChar, 20),
        new SqlParameter(EdSelectionLists.PARM_UPDATED_BY_USER_ID, SqlDbType.NVarChar,100),
        new SqlParameter(EdSelectionLists.PARM_UPDATED_BY, SqlDbType.NVarChar, 100),
        new SqlParameter(EdSelectionLists.PARM_UPDATED_DATE, SqlDbType.DateTime)
      };
      commandParameters [ 0 ].Value = Item.ListId;
      commandParameters [ 1 ].Value = this.ClassParameters.UserProfile.UserId;
      commandParameters [ 2 ].Value = this.ClassParameters.UserProfile.CommonName;
      commandParameters [ 3 ].Value = DateTime.Now;

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( _STORED_PROCEDURE_WithdrawItem, commandParameters ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      return EvEventCodes.Ok;

    }//End WithdrawIssuedList method.

    // =====================================================================================
    /// <summary>
    /// This class deletes items from formfield selection list table. 
    /// </summary>
    /// <param name="Item">EvFormFieldSelectionList: a formfield selection list object</param>
    /// <returns>EvEventCodes: an event code for deleting items</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Define the sql query parameters and execute the storeprocedure for deleting the items. 
    /// 
    /// 2. Return an event code for deleting the items. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes DeleteItem ( EdSelectionList Item )
    {
      this._Log = new System.Text.StringBuilder ( );
      this.LogMethod ( "deleteItem method. " );

      // 
      // Define the query parameters
      // 
      SqlParameter [ ] commandParameters = new SqlParameter [ ]
      {
        new SqlParameter(EdSelectionLists.PARM_GUID, SqlDbType.UniqueIdentifier),
        new SqlParameter(EdSelectionLists.PARM_UPDATED_BY_USER_ID, SqlDbType.NVarChar,100),
        new SqlParameter(EdSelectionLists.PARM_UPDATED_BY, SqlDbType.NVarChar, 100),
        new SqlParameter(EdSelectionLists.PARM_UPDATED_DATE, SqlDbType.DateTime)
      };
      commandParameters [ 0 ].Value = Item.Guid;
      commandParameters [ 1 ].Value = this.ClassParameters.UserProfile.UserId;
      commandParameters [ 2 ].Value = this.ClassParameters.UserProfile.CommonName;
      commandParameters [ 3 ].Value = DateTime.Now;

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( _STORED_PROCEDURE_DeleteItem, commandParameters ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      return EvEventCodes.Ok;

    }// End deleteYear method.
    
    // ==================================================================================
    /// <summary>
    /// This class updates the appliocation parameter records data object. 
    /// </summary>
    /// <param name="Item">EdSelectionList object</param>
    /// <returns>EvEventCodes: an event code for update data object</returns>
    // ----------------------------------------------------------------------------------
    private int trimItemOptions ( EdSelectionList Item )
    {
      this.LogMethod ( "trimOptions " );
      string hasOptions = String.Empty ;
      int count = 0;

      this.LogDebug( "Items.Count {0}.", Item.Items.Count);

      //
      // iterate through the options in the list.
      //
      for ( int i = 0; i < Item.Items.Count; i++ )
      {
        if ( hasOptions.Contains ( Item.Items [ i ].Value ) == true )
        {
          Item.Items.RemoveAt ( i );
          i--;
          continue;
        }
        hasOptions += ";" + Item.Items [ i ].Value;

      }//END option item iteration loop.

      this.LogDebug ( "TRIMMED: Items.Count {0}.", Item.Items.Count );
      this.LogMethod ( "trimOptions " );
      return count;
    }

    // ==================================================================================
    /// <summary>
    /// This class updates the appliocation parameter records data object. 
    /// </summary>
    /// <param name="Item">EdSelectionList object</param>
    /// <returns>EvEventCodes: an event code for update data object</returns>
    // ----------------------------------------------------------------------------------
    private EvEventCodes updateOptions ( EdSelectionList Item )
    {
      this.LogMethod ( "updateOptions " );
      this.LogValue ( "ListId: {0}.", Item.ListId );
      this.LogValue ( "Item.Items.Count: {0}.", Item.Items.Count );
      //
      // Initialize the Sql update query string. 
      //
      System.Text.StringBuilder SqlUpdateQuery = new System.Text.StringBuilder ( );

      if ( Item.Items.Count == 0 )
      {
        this.LogValue ( "No options in the list" );

        this.LogMethodEnd ( "updateOptions " );
        return EvEventCodes.Ok;
      }

      //
      // Delete the milestone activities for this milestone.
      //
      SqlUpdateQuery.AppendLine ( "/** DELETE ALL OF OBJECT PARAMETERS FOR THE OBJECT **/" );
      SqlUpdateQuery.AppendLine ( " DELETE FROM ED_SELECTION_LIST_OPTIONS " );
      SqlUpdateQuery.AppendLine ( " WHERE  (EDSL_LIST_ID = '" + Item.ListId + "') ; \r\n" );

      for ( int count = 0; count < Item.Items.Count; count++ )
      {
        EdSelectionList.Item selectionItem = Item.Items [ count ];
        //
        // Skip the non selected forms
        //
        if ( selectionItem == null )
        {
          continue;
        }

        this.LogDebug ( "Value: {0}, Description: {1}, Category: {1} ", selectionItem.Value, selectionItem.Description, selectionItem.Category );

        SqlUpdateQuery.AppendLine ( "Insert Into ED_SELECTION_LIST_OPTIONS " );
        SqlUpdateQuery.AppendLine ( "( EDSL_LIST_ID, EDS0_NO, EDS0_VALUE, EDSO_DESCRIPTION, EDSO_CATEGORY )  " );
        SqlUpdateQuery.AppendLine ( "values  " );
        SqlUpdateQuery.AppendLine ( "('" + Item.ListId + "', " );
        SqlUpdateQuery.AppendLine ( " " + selectionItem.No + ", " );
        SqlUpdateQuery.AppendLine ( "'" + selectionItem.Value + "', " );
        SqlUpdateQuery.AppendLine ( "'" + selectionItem.Description + "', " );
        SqlUpdateQuery.AppendLine ( " '" + selectionItem.Category + "' ); \r\n" );

      }//END form list iteration loop.


      if ( EvSqlMethods.QueryUpdate ( SqlUpdateQuery.ToString ( ), null ) == 0 )
      {
        this.LogValue ( "Update failed" );
        this.LogMethodEnd ( "updateOptions " );
        return EvEventCodes.Database_Record_Update_Error;
      }

      this.LogValue ( "Update completed" );
      // 
      // Return code
      //       
      this.LogMethodEnd ( "updateOptions " );
      return EvEventCodes.Ok;

    }//END updateOptions class

    #endregion


  }//END EdSelectionLists class

}//END namespace Evado.Dal.Digital
