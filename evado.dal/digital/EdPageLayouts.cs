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
  public class EdPageLayouts : EvDalBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EdPageLayouts ( )
    {
      this.ClassNameSpace = "Evado.Dal.Digital.EdPageLayouts.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="ClassParameters">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EdPageLayouts ( EvClassParameters ClassParameters )
    {
      this.ClassParameters = ClassParameters;
      this.ClassNameSpace = "Evado.Dal.Digital.EdPageLayouts.";
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
    private const string SQL_VIEW_QUERY = "Select * FROM ED_PAGE_LAYOUTS ";

    /// <summary>
    /// This constant defines storeprocedure for adding items to formfield selection list table. 
    /// </summary>
    private const string _STORED_PROCEDURE_AddItem = "USR_PAGE_LAYOUT_ADD";

    /// <summary>
    /// This constant defines storeprocedure for updating items on formfield selection list table
    /// </summary>
    private const string _STORED_PROCEDURE_UpdateItem = "USR_PAGE_LAYOUT_UPDATE";

    /// <summary>
    /// This constant defines storeprocedure for deleting items from formfield selection list table
    /// </summary>
    private const string _STORED_PROCEDURE_DeleteItem = "USR_PAGE_LAYOUT_DELETE";

    /// <summary>
    /// This constant defines storeprocedure for withdrawing items from formfield selection list table
    /// </summary>
    private const string _STORED_PROCEDURE_WithdrawItem = "USR_PAGE_LAYOUT_WITHDRAWN";


    private const string DB_GUID = "EDPL_GUID";
    private const String DB_PAGE_ID = "EDPL_PAGE_ID";
    private const String DB_STATE = "EDPL_STATE";
    private const String DB_USER_TYPE = "UP_TYPE";
    private const String DB_TITLE = "EDPL_TITLE";
    private const String DB_HOME_PAGE = "EDPL_HOME_PAGE";
    private const String DB_MENU_LOCATION = "EDPL_MENU_LOCATION";
    private const String DB_PAGE_COMMANDS = "EDPL_PAGE_COMMANDS";

    private const String DB_HEADER_CONTENT = "EDPL_HEADER_CONTENT";
    private const String DB_HEADER_GROUP_LIST = "EDPL_HEADER_GROUP_LIST";

    private const String DB_LEFT_CONTENT = "EDPL_LEFT_CONTENT";
    private const String DB_LEFT_GROUP_LIST = "EDPL_LEFT_GROUP_LIST";
    private const String DB_LEFT_COLUMN_WIDTH = "EDPL_LEFT_COLUMN_WIDTH";

    private const String DB_CENTER_CONTENT = "EDPL_CENTER_CONTENT";
    private const String DB_CENTER_GROUP_LIST = "EDPL_CENTER_GROUP_LIST";

    private const String DB_RIGHT_CONTENT = "EDPL_RIGHT_CONTENT";
    private const String DB_RIGHT_GROUP_LIST = "EDPL_RIGHT_GROUP_LIST";
    private const String DB_RIGHT_COLUMN_WIDTH = "EDPL_RIGHT_COLUMN_WIDTH";

    private const String DB_VERSION = "EDPL_VERSION";
    private const String DB_UPDATED_BY_USER_ID = "EDPL_UPDATED_BY_USER_ID";
    private const String DB_UPDATED_BY = "EDPL_UPDATED_BY";
    private const String DB_UPDATED_DATE = "EDPL_UPDATED_DATE";
    private const String DB_DELETED = "EDPL_DELETED";

    /// <summary>
    /// This constant defines the parameter for global unique identifier of FormField selection list object
    /// </summary>

    private const String PARM_GUID = "@GUID";
    private const String PARM_PAGE_ID = "@PAGE_ID";
    private const String PARM_STATE = "@STATE";
    private const String PARM_USER_TYPE = "@USER_TYPE";
    private const String PARM_TITLE = "@TITLE";
    private const String PARM_HOME_PAGE = "@HOME_PAGE";
    private const String PARM_MENU_LOCATION = "@MENU_LOCATION";
    private const String PARM_PAGE_COMMANDS = "@PAGE_COMMANDS";

    private const String PARM_HEADER_CONTENT = "@HEADER_CONTENT";
    private const String PARM_HEADER_GROUP_LIST = "@HEADER_GROUP_LIST";

    private const String PARM_LEFT_CONTENT = "@LEFT_CONTENT";
    private const String PARM_LEFT_GROUP_LIST = "@LEFT_GROUP_LIST";
    private const String PARM_LEFT_COLUMN_WIDTH = "@LEFT_COLUMN_WIDTH";

    private const String PARM_CENTER_CONTENT = "@CENTER_CONTENT";
    private const String PARM_CENTER_GROUP_LIST = "@CENTER_GROUP_LIST";

    private const String PARM_RIGHT_CONTENT = "@RIGHT_CONTENT";
    private const String PARM_RIGHT_GROUP_LIST = "@RIGHT_GROUP_LIST";
    private const String PARM_RIGHT_COLUMN_WIDTH = "@RIGHT_COLUMN_WIDTH";

    private const String PARM_VERSION = "@VERSION";
    private const String PARM_UPDATED_BY_USER_ID = "@UPDATED_BY_USER_ID";
    private const String PARM_UPDATED_BY = "@UPDATED_BY";
    private const String PARM_UPDATED_DATE = "@UPDATED_DATE";

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
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
        new SqlParameter( EdPageLayouts.PARM_GUID, SqlDbType.UniqueIdentifier ),
        new SqlParameter( EdPageLayouts.PARM_PAGE_ID, SqlDbType.NVarChar, 10 ),
        new SqlParameter( EdPageLayouts.PARM_USER_TYPE, SqlDbType.NVarChar,50 ),
        new SqlParameter( EdPageLayouts.PARM_STATE, SqlDbType.NVarChar, 30),
        new SqlParameter( EdPageLayouts.PARM_TITLE, SqlDbType.NVarChar, 100), //4

        new SqlParameter( EdPageLayouts.PARM_HOME_PAGE, SqlDbType.Bit),
        new SqlParameter( EdPageLayouts.PARM_MENU_LOCATION, SqlDbType.NVarChar, 30),
        new SqlParameter( EdPageLayouts.PARM_PAGE_COMMANDS, SqlDbType.NVarChar, 250),
        new SqlParameter( EdPageLayouts.PARM_HEADER_CONTENT, SqlDbType.NText ),
        new SqlParameter( EdPageLayouts.PARM_HEADER_GROUP_LIST, SqlDbType.NVarChar, 250 ), //9

        new SqlParameter( EdPageLayouts.PARM_LEFT_CONTENT, SqlDbType.NText ),
        new SqlParameter( EdPageLayouts.PARM_LEFT_GROUP_LIST, SqlDbType.NVarChar, 250 ),
        new SqlParameter( EdPageLayouts.PARM_LEFT_COLUMN_WIDTH, SqlDbType.SmallInt ), //12

        new SqlParameter( EdPageLayouts.PARM_CENTER_CONTENT, SqlDbType.NText ),
        new SqlParameter( EdPageLayouts.PARM_CENTER_GROUP_LIST, SqlDbType.NVarChar, 250 ),

        new SqlParameter( EdPageLayouts.PARM_RIGHT_CONTENT, SqlDbType.NText ),
        new SqlParameter( EdPageLayouts.PARM_RIGHT_GROUP_LIST, SqlDbType.NVarChar, 250 ),
        new SqlParameter( EdPageLayouts.PARM_RIGHT_COLUMN_WIDTH, SqlDbType.SmallInt ), //17

        new SqlParameter( EdPageLayouts.PARM_VERSION, SqlDbType.Int ),
        new SqlParameter( EdPageLayouts.PARM_UPDATED_BY_USER_ID,SqlDbType.NVarChar, 100 ),
        new SqlParameter( EdPageLayouts.PARM_UPDATED_BY, SqlDbType.NVarChar, 100 ),
        new SqlParameter( EdPageLayouts.PARM_UPDATED_DATE, SqlDbType.DateTime ),
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
    private void SetParameters ( SqlParameter [ ] parms, EdPageLayout Item )
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
      parms [ 1 ].Value = Item.PageId;
      parms [ 2 ].Value = Item.UserType;
      parms [ 3 ].Value = Item.State;
      parms [ 4 ].Value = Item.Title;

      parms [ 5 ].Value = Item.HomePage;
      parms [ 6 ].Value = Item.MenuLocation;
      parms [ 7 ].Value = Item.PageCommands;
      parms [ 8 ].Value = Item.HeaderContent;
      parms [ 9 ].Value = Item.HeaderGroupList;

      parms [ 10 ].Value = Item.LeftColumnContent;
      parms [ 11 ].Value = Item.LeftColumnGroupList;
      parms [ 12 ].Value = Item.LeftColumnWidth;

      parms [ 13 ].Value = Item.CenterColumnContent;
      parms [ 14 ].Value = Item.CenterColumnGroupList;

      parms [ 15 ].Value = Item.RightColumnContent;
      parms [ 16 ].Value = Item.RightColumnGroupList;
      parms [ 17 ].Value = Item.RightColumnWidth;

      parms [ 18 ].Value = Item.Version;
      parms [ 19 ].Value = this.ClassParameters.UserProfile.UserId;
      parms [ 20 ].Value = this.ClassParameters.UserProfile.CommonName;
      parms [ 21 ].Value = DateTime.Now;

    }//END SetParameters class.

    #endregion

    #region PageLayout Reader

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
    private EdPageLayout readDataRow ( DataRow Row )
    {
      // 
      // Initialise the formfield selectionlist object.
      // 
      EdPageLayout Item = new EdPageLayout ( );

      //
      // Extract the compatible data row values to the formfield selectionlist object items.
      //
      Item.Guid = EvSqlMethods.getGuid ( Row, EdPageLayouts.DB_GUID );
      Item.PageId = EvSqlMethods.getString ( Row, EdPageLayouts.DB_PAGE_ID );
      Item.State = EvSqlMethods.getString<EdPageLayout.States> ( Row, EdPageLayouts.DB_STATE );
      Item.UserType = EvSqlMethods.getString ( Row, EdPageLayouts.DB_USER_TYPE );
      Item.Title = EvSqlMethods.getString ( Row, EdPageLayouts.DB_TITLE );

      Item.HomePage = EvSqlMethods.getBool ( Row, EdPageLayouts.DB_HOME_PAGE );
      Item.MenuLocation = EvSqlMethods.getString<EdPageLayout.MenuLocations> ( Row, EdPageLayouts.DB_MENU_LOCATION );
      Item.PageCommands = EvSqlMethods.getString ( Row, EdPageLayouts.DB_PAGE_COMMANDS );
      Item.HeaderContent = EvSqlMethods.getString ( Row, EdPageLayouts.DB_HEADER_CONTENT );
      Item.HeaderGroupList = EvSqlMethods.getString ( Row, EdPageLayouts.DB_HEADER_GROUP_LIST );

      Item.LeftColumnContent = EvSqlMethods.getString ( Row, EdPageLayouts.DB_LEFT_CONTENT );
      Item.LeftColumnGroupList = EvSqlMethods.getString ( Row, EdPageLayouts.DB_LEFT_GROUP_LIST );
      Item.LeftColumnWidth = (short) EvSqlMethods.getInteger ( Row, EdPageLayouts.DB_LEFT_COLUMN_WIDTH );

      Item.CenterColumnContent = EvSqlMethods.getString ( Row, EdPageLayouts.DB_CENTER_CONTENT );
      Item.CenterColumnGroupList = EvSqlMethods.getString ( Row, EdPageLayouts.DB_CENTER_GROUP_LIST );

      Item.RightColumnContent = EvSqlMethods.getString ( Row, EdPageLayouts.DB_RIGHT_CONTENT );
      Item.RightColumnGroupList = EvSqlMethods.getString ( Row, EdPageLayouts.DB_RIGHT_GROUP_LIST );
      Item.RightColumnWidth = (short) EvSqlMethods.getInteger ( Row, EdPageLayouts.DB_RIGHT_COLUMN_WIDTH );

      Item.Version = EvSqlMethods.getInteger ( Row, EdPageLayouts.DB_VERSION );
      Item.UpdatedByUserId = EvSqlMethods.getString ( Row, EdPageLayouts.DB_UPDATED_BY_USER_ID );
      Item.UpdatedBy = EvSqlMethods.getString ( Row, EdPageLayouts.DB_UPDATED_BY );
      Item.UpdatedDate += EvSqlMethods.getDateTime ( Row, EdPageLayouts.DB_UPDATED_DATE );

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
    public List<EdPageLayout> getView (
      EdPageLayout.States State )
    {
      //
      // Initialize the method status and a return list of formfield selectionlist object
      //
      this.LogMethod ( "getView method." );
      this._Log.AppendLine ( "State: " + State );

      List<EdPageLayout> view = new List<EdPageLayout> ( );
      String sqlQueryString = String.Empty;
      //
      // Define the sql query parameter and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(EdPageLayouts.PARM_STATE, SqlDbType.VarChar, 20)
      };
      cmdParms [ 0 ].Value = State.ToString ( );

      //
      // Define the sql query string. 
      // 
      sqlQueryString = SQL_VIEW_QUERY;
      if ( State != EdPageLayout.States.Null )
      {
        sqlQueryString += " WHERE ( " + EdPageLayouts.DB_STATE + " = " + EdPageLayouts.PARM_STATE + " ) "
          + " AND (" + EdPageLayouts.DB_DELETED + " = 0) "
          + " ORDER BY " + EdPageLayouts.DB_PAGE_ID + ";";
      }
      else
      {
        sqlQueryString += " WHERE (  " + EdPageLayouts.DB_STATE + " <> '" + EdPageLayout.States.Withdrawn + "' ) "
          + " AND (" + EdPageLayouts.DB_DELETED + " = 0) "
          + " ORDER BY " + EdPageLayouts.DB_PAGE_ID + ";";
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

          EdPageLayout pageLayout = this.readDataRow ( row );

          //
          // retrieve the parameter object values.
          //
          pageLayout.Parameters = this.LoadObjectParameters ( pageLayout.Guid );

          // 
          // Append the value to the visit
          // 
          view.Add ( pageLayout );

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
      EdPageLayout.States State,
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
        EdPageLayout item = selectionList [ count ];
        //
        // If SelectByGuid = True then optionId is to be the objects TestReport UID
        //
        if ( SelectByGuid == true )
        {
          option = new EvOption ( item.Guid.ToString ( ),
            String.Format ( "{0} - {1} ", item.PageId, item.Title ) );
        }

        //
        // If SelectByGuid = False then optionId is to be the objects ListId.
        //
        else
        {
          option = new EvOption ( item.PageId,
            String.Format ( "{0} - {1} ", item.PageId, item.Title ) );
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
    /// <param name="PageGuid">Guid: The Global unique identifier</param>
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
    public EdPageLayout getItem ( Guid PageGuid )
    {
      this.LogMethod ( "getItem" );
      this._Log.AppendLine ( "PageGuid: " + PageGuid );
      //
      // Initialise the local variables
      //
      EdPageLayout item = new EdPageLayout ( );
      String sqlQueryString = String.Empty;

      //
      // If TestReport UID is null return empty checlist object.
      //
      if ( PageGuid == Guid.Empty )
      {
        return item;
      }

      //
      // Initialise the query parameters.
      //
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(EdPageLayouts.PARM_GUID, SqlDbType.UniqueIdentifier ),
      };
      cmdParms [ 0 ].Value = PageGuid;

      // 
      // Generate the Selection query string.
      // 
      sqlQueryString = SQL_VIEW_QUERY + " WHERE " + EdPageLayouts.DB_GUID + " = " + EdPageLayouts.PARM_GUID + ";";

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

        //
        // retrieve the parameter object values.
        //
        item.Parameters = this.LoadObjectParameters ( item.Guid );

      }//END Using 

      //
      // Return Checklsit data object.
      //
      return item;

    }//END EvFormFieldSelectionList method

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
    public EvEventCodes updateItem ( EdPageLayout Item )
    {
      this.LogMethod ( "updateItem" );

      // 
      // Get the previous value
      // 
      EdPageLayout oldItem = getItem ( Item.Guid );
      if ( oldItem.Guid == Guid.Empty )
      {
        return EvEventCodes.Identifier_Global_Unique_Identifier_Error;
      }

      // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      // Compare the objects.
      // 
      EvDataChanges dataChanges = new EvDataChanges ( );
      EvDataChange dataChange = new EvDataChange ( );
      dataChange.TableName = EvDataChange.DataChangeTableNames.EdPageLayouts;
      dataChange.TrialId = String.Empty;
      dataChange.RecordUid = -1;
      dataChange.RecordGuid = Item.Guid;
      dataChange.UserId = Item.UpdatedByUserId;
      dataChange.DateStamp = DateTime.Now;

      //
      // Add items to datachange object if they exist. 
      //
      if ( Item.PageId != oldItem.PageId )
      {
        dataChange.AddItem ( "ListId", oldItem.PageId, Item.PageId );
      }
      if ( Item.Title != oldItem.Title )
      {
        dataChange.AddItem ( "Title", oldItem.Title, Item.Title );
      }
      if ( Item.UserType != oldItem.UserType )
      {
        dataChange.AddItem ( "UserType", oldItem.UserType, Item.UserType );
      }
      if ( Item.State != oldItem.State )
      {
        dataChange.AddItem ( "State", oldItem.State, Item.State );
      }
      if ( Item.HeaderContent != oldItem.HeaderContent )
      {
        dataChange.AddItem ( "HeaderContent", Item.HeaderContent, Item.HeaderContent );
      }
      if ( Item.HeaderGroupList != oldItem.HeaderGroupList )
      {
        dataChange.AddItem ( "HeaderGroupList", oldItem.HeaderGroupList, Item.HeaderGroupList );
      }
      if ( Item.LeftColumnContent != oldItem.LeftColumnContent )
      {
        dataChange.AddItem ( "LeftColumnContent", oldItem.LeftColumnContent, Item.LeftColumnContent );
      }
      if ( Item.LeftColumnGroupList != oldItem.LeftColumnGroupList )
      {
        dataChange.AddItem ( "LeftColumnGroupList", oldItem.LeftColumnGroupList, Item.LeftColumnGroupList );
      }
      if ( Item.LeftColumnWidth != oldItem.LeftColumnWidth )
      {
        dataChange.AddItem ( "LeftColumnCommandList", oldItem.LeftColumnWidth, Item.LeftColumnWidth );
      }

      if ( Item.CenterColumnContent != oldItem.CenterColumnContent )
      {
        dataChange.AddItem ( "CenterColumnContent", oldItem.CenterColumnContent, Item.CenterColumnContent );
      }
      if ( Item.LeftColumnGroupList != oldItem.LeftColumnGroupList )
      {
        dataChange.AddItem ( "LeftColumnGroupList", oldItem.LeftColumnGroupList, Item.LeftColumnGroupList );
      }

      if ( Item.RightColumnContent != oldItem.RightColumnContent )
      {
        dataChange.AddItem ( "RightColumnContent", oldItem.RightColumnContent, Item.RightColumnContent );
      }
      if ( Item.RightColumnGroupList != oldItem.RightColumnGroupList )
      {
        dataChange.AddItem ( "RightColumnGroupList", oldItem.RightColumnGroupList, Item.RightColumnGroupList );
      }
      if ( Item.RightColumnWidth != oldItem.RightColumnWidth )
      {
        dataChange.AddItem ( "RightColumnCommandList", oldItem.RightColumnWidth, Item.RightColumnWidth );
      }

      if ( Item.Version != oldItem.Version )
      {
        dataChange.AddItem ( "Version", oldItem.Version, Item.Version );
      }
      if ( Item.State != oldItem.State )
      {
        dataChange.AddItem ( "State", oldItem.State.ToString ( ), Item.State.ToString ( ) );
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
      // update the parameter object values.
      //
      this.UpdateObjectParameters ( Item.Parameters, Item.Guid );

      // 
      // Add the change record
      // 
      dataChanges.AddItem ( dataChange );

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
    public EvEventCodes addItem ( EdPageLayout Item )
    {
      this.LogMethod ( "addItem method. " );
      this.LogDebug ( "ListId: '" + Item.PageId );
      this.LogDebug ( "Version: '" + Item.Version + "'" );
      String sqlQueryString = String.Empty;

      //---------------------- Check for duplicate TestReport identifiers. ------------------
      // 
      // Define the query parameters
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( EdPageLayouts.PARM_PAGE_ID, SqlDbType.NVarChar, 10),
      };
      cmdParms [ 0 ].Value = Item.PageId;

      // 
      // Generate the SQL query string
      // 
      sqlQueryString = SQL_VIEW_QUERY + " WHERE (" + EdPageLayouts.DB_PAGE_ID + " = " + EdPageLayouts.PARM_PAGE_ID + ") "
        + " AND (" + EdPageLayouts.DB_DELETED + " = 0);";

      this.LogDebug ( "Duplication SQL Query:" + sqlQueryString );

      //
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
      {
        // 
        // If not rows the return
        // 
        if ( table.Rows.Count > 0 )
        {
          this.LogDebug ( "Duplication list found." );

          return EvEventCodes.Data_Duplicate_Id_Error;
        }
      }

      // 
      // Create the Guid
      // 
      Item.Guid = Guid.NewGuid ( );

      // 
      // Define the query parameters
      // 
      SqlParameter [ ] commandParameters = GetParameters ( );
      SetParameters ( commandParameters, Item );

      //this.LogDebug( EvSqlMethods.getParameterSqlText( commandParameters ) ) ;

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( _STORED_PROCEDURE_AddItem, commandParameters ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

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
    public EvEventCodes WithdrawIssuedList ( EdPageLayout Item )
    {
      this.LogMethod ( "WithdrawIssuedList method. " );
      // 
      // Define the query parameters
      // 
      SqlParameter [ ] commandParameters = new SqlParameter [ ]
      {
        new SqlParameter(EdPageLayouts.PARM_PAGE_ID, SqlDbType.NVarChar, 20),
        new SqlParameter(EdPageLayouts.PARM_UPDATED_BY_USER_ID, SqlDbType.NVarChar,100),
        new SqlParameter(EdPageLayouts.PARM_UPDATED_BY, SqlDbType.NVarChar, 100),
        new SqlParameter(EdPageLayouts.PARM_UPDATED_DATE, SqlDbType.DateTime)
      };
      commandParameters [ 0 ].Value = Item.PageId;
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
    public EvEventCodes deleteItem ( EdPageLayout Item )
    {
      this.LogMethod ( "deleteItem method. " );

      // 
      // Define the query parameters
      // 
      SqlParameter [ ] commandParameters = new SqlParameter [ ]
      {
        new SqlParameter(EdPageLayouts.PARM_GUID, SqlDbType.UniqueIdentifier),
        new SqlParameter(EdPageLayouts.PARM_UPDATED_BY_USER_ID, SqlDbType.NVarChar,100),
        new SqlParameter(EdPageLayouts.PARM_UPDATED_BY, SqlDbType.NVarChar, 100),
        new SqlParameter(EdPageLayouts.PARM_UPDATED_DATE, SqlDbType.DateTime)
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

    #endregion


  }//END EdPageLayouts class

}//END namespace Evado.Dal.Digital
