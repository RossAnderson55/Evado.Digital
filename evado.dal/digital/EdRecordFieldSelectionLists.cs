/***************************************************************************************
 * <copyright file="dal\EvFormFieldSelectionLists.cs" company="EVADO HOLDING PTY. LTD.">
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
  public class EdRecordFieldSelectionLists : EvDalBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EdRecordFieldSelectionLists ( )
    {
      this.ClassNameSpace = "Evado.Dal.Clinical.EvFormFieldSelectionLists.";
      if ( int.TryParse ( _stMaximumListLength, out _MaximumListLength ) == false )
      {
        _MaximumListLength = 100;
      }
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="ClassParameters">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EdRecordFieldSelectionLists ( EvClassParameters ClassParameters )
    {
      this.ClassParameters = ClassParameters;
      this.ClassNameSpace = "Evado.Dal.Clinical.EvFormFieldSelectionLists.";
      if ( int.TryParse ( _stMaximumListLength, out _MaximumListLength ) == false )
      {
        _MaximumListLength = 100;
      }
    }

    #endregion


    #region Object Initialisation

    /* *********************************************************************************
     * 
     * Defines the classes constansts and global variables
     * 
     * *********************************************************************************/
    // The max visitSchedule length setting. 
    private static string _stMaximumListLength = ConfigurationManager.AppSettings [ "MaximumSelectionListLength" ];
    private int _MaximumListLength = 100;

    /// <summary>
    /// This constant defines sql query string for selecting all items from FormField selectionlist view.
    /// </summary>
    private const string _sqlQuery_View = "Select * FROM EvFormFieldSelectionList_View ";

    #region Define class Storeprocedure. 
    /// <summary>
    /// This constant defines storeprocedure for adding items to formfield selection list table. 
    /// </summary>
    private const string _STORED_PROCEDURE_AddItem = "usr_FormFieldSelectionList_add";

    /// <summary>
    /// This constant defines storeprocedure for updating items on formfield selection list table
    /// </summary>
    private const string _STORED_PROCEDURE_UpdateItem = "usr_FormFieldSelectionList_update";

    /// <summary>
    /// This constant defines storeprocedure for deleting items from formfield selection list table
    /// </summary>
    private const string _STORED_PROCEDURE_DeleteItem = "usr_FormFieldSelectionList_delete";

    /// <summary>
    /// This constant defines storeprocedure for copying items from formfield selection list table
    /// </summary>
    private const string _STORED_PROCEDURE_CopyItem = "usr_FormFieldSelectionList_copy";

    /// <summary>
    /// This constant defines storeprocedure for withdrawing items from formfield selection list table
    /// </summary>
    private const string _STORED_PROCEDURE_WithdrawItem = "usr_FormFieldSelectionList_withdraw";
    #endregion

    #region Define class parameters. 
    /// <summary>
    /// This constant defines the parameter for global unique identifier of FormField selection list object
    /// </summary>
    private const string PARM_Guid = "@Guid";

    /// <summary>
    /// This constant defines the parameter for list identifier of FormField selection list object
    /// </summary>
    private const string PARM_ListId = "@ListId";

    /// <summary>
    /// This constant defines the parameter for title of FormField selection list object
    /// </summary>
    private const string PARM_Title = "@Title";

    /// <summary>
    /// This constant defines the parameter for instructions of FormField selection list object
    /// </summary>
    private const string PARM_Instructions = "@Instructions";

    /// <summary>
    /// This constant defines the parameter for xml validation rules of FormField selection list object
    /// </summary>
    private const string PARM_XmlValidationRules = "@XmlValidationRules";

    /// <summary>
    /// This constant defines the parameter for xml items of FormField selection list object
    /// </summary>
    private const string PARM_XmlItems = "@XmlItems";

    /// <summary>
    /// This constant defines the parameter for authors of FormField selection list object
    /// </summary>
    private const string PARM_AuhorList = "@Authors";

    /// <summary>
    /// This constant defines the parameter for users who reviews FormField selection list object
    /// </summary>
    private const string PARM_ReviewedBy = "@ReviewedBy";

    /// <summary>
    /// This constant defines the parameter for user identifier of those who reviews FormField selection list object
    /// </summary>
    private const string PARM_ReviewedByUserId = "@ReviewedByUserId";

    /// <summary>
    /// This constant defines the parameter for reviewed date of FormField selection list object
    /// </summary>
    private const string PARM_ReviewDate = "@ReviewDate";

    /// <summary>
    /// This constant defines the parameter for user who approves FormField selection list object
    /// </summary>
    private const string PARM_ApprovedBy = "@ApprovedBy";

    /// <summary>
    /// This constant defines the parameter for user identifier of those who approves FormField selection list object
    /// </summary>
    private const string PARM_ApprovedByUserId = "@ApprovedByUserId";

    /// <summary>
    /// This constant defines the parameter for approval date of FormField selection list object
    /// </summary>
    private const string PARM_ApprovalDate = "@ApprovalDate";

    /// <summary>
    /// This constant defines the parameter for version of FormField selection list object
    /// </summary>
    private const string PARM_Version = "@Version";

    /// <summary>
    /// This constant defines the parameter for state of FormField selection list object
    /// </summary>
    private const string PARM_State = "@State";

    /// <summary>
    /// This constant defines the parameter for user identifier of those who updates FormField selection list object
    /// </summary>
    private const string PARM_UpdatedByUserId = "@UpdatedByUserId";

    /// <summary>
    /// This constant defines the parameter for user who updates FormField selection list object
    /// </summary>
    private const string PARM_UpdatedBy = "@UpdatedBy";

    /// <summary>
    /// This constant defines the parameter for updated date of FormField selection list object
    /// </summary>
    private const string PARM_UpdateDate = "@UpdateDate";
    #endregion

    /// <summary>
    /// Define the SQL query string variable.
    /// </summary>
    private string _sqlQueryString = String.Empty;
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
    private static SqlParameter [] GetParameters( )
    {
      SqlParameter [] parms = new SqlParameter [] 
      {
        new SqlParameter( PARM_Guid, SqlDbType.UniqueIdentifier ),
        new SqlParameter( PARM_ListId, SqlDbType.NVarChar, 10 ),
        new SqlParameter( PARM_Title, SqlDbType.NVarChar, 80 ),
        new SqlParameter( PARM_Instructions, SqlDbType.NVarChar, 2000000000),
        new SqlParameter( PARM_XmlValidationRules, SqlDbType.NVarChar, 2000000000 ),
        new SqlParameter( PARM_XmlItems, SqlDbType.NVarChar, 2000000000 ),
        new SqlParameter( PARM_AuhorList, SqlDbType.NVarChar, 100000 ),
        new SqlParameter( PARM_ReviewedBy, SqlDbType.NVarChar, 100 ),
        new SqlParameter( PARM_ReviewedByUserId, SqlDbType.NVarChar, 100 ),
        new SqlParameter( PARM_ReviewDate, SqlDbType.DateTime ),
        new SqlParameter( PARM_ApprovedBy, SqlDbType.NVarChar, 100 ),
        new SqlParameter( PARM_ApprovedByUserId, SqlDbType.NVarChar, 100 ),
        new SqlParameter( PARM_ApprovalDate, SqlDbType.DateTime ),
        new SqlParameter( PARM_Version, SqlDbType.NVarChar, 5 ),
        new SqlParameter( PARM_State, SqlDbType.NVarChar, 10 ),
        new SqlParameter( PARM_UpdatedByUserId,SqlDbType.NVarChar, 100 ),
        new SqlParameter( PARM_UpdatedBy, SqlDbType.NVarChar, 100 ),
        new SqlParameter( PARM_UpdateDate, SqlDbType.DateTime ),
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
    private void SetParameters( SqlParameter [] parms, EdExternalSelectionList Item )
    {
      //
      // Add new DB row Guid, if item's Guid is empty
      //
      if ( Item.Guid == Guid.Empty )
      {
        Item.Guid = Guid.NewGuid( );
      }

      //
      // Update the items values from formfield selectionlist object to the array of sql parameters.
      //
      parms [ 0 ].Value = Item.Guid;
      parms [ 1 ].Value = Item.ListId;
      parms [ 2 ].Value = Item.Title;
      parms [ 3 ].Value = Item.Instructions;
      parms [ 4 ].Value = Evado.Model.EvStatics.SerialiseObject<EdExternalSelectionList.XmlValidationRules> ( Item.ValidationRules );
      parms [ 5 ].Value = Evado.Model.EvStatics.SerialiseObject<List<EdExternalSelectionList.CodeItem>> ( Item.Items );
      parms [ 6 ].Value = Item.Authors;
      parms [ 7 ].Value = Item.ReviewedBy;
      parms [ 8 ].Value = Item.ReviewedByUserId;
      parms [ 9 ].Value = Item.ReviewDate;
      parms [ 10 ].Value = Item.ApprovedBy;
      parms [ 11 ].Value = Item.ApprovedByUserId;
      parms [ 12 ].Value = Item.ApprovalDate;
      parms [ 13 ].Value = Item.Version;
      parms [ 14 ].Value = Item.State;
      parms [ 15 ].Value = Item.UpdatedByUserId;
      parms [ 16 ].Value = Item.UserCommonName;
      parms [ 17 ].Value = DateTime.Now;

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
    private EdExternalSelectionList readDataRow ( DataRow Row )
    {
      // 
      // Initialise the formfield selectionlist object.
      // 
      EdExternalSelectionList Item = new EdExternalSelectionList();

      //
      // Extract the compatible data row values to the formfield selectionlist object items.
      //
      Item.Guid = EvSqlMethods.getGuid( Row, "FSL_Guid" );
      Item.ListId = EvSqlMethods.getString( Row, "ListId" );
      Item.Title = EvSqlMethods.getString( Row, "FSL_Title" );
      Item.Instructions = EvSqlMethods.getString( Row, "FSL_Instructions" );

      string xmlValidationRules = EvSqlMethods.getString( Row, "FSL_XmlValidationRules" );
        if ( xmlValidationRules != String.Empty )
        {
          Item.ValidationRules = Evado.Model.EvStatics.DeserialiseObject<EdExternalSelectionList.XmlValidationRules> ( xmlValidationRules );
        }
        string xmlCodeItem = EvSqlMethods.getString( Row, "FSL_XmlItems" );
        if ( xmlCodeItem != String.Empty )
        {
          xmlCodeItem = xmlCodeItem.Replace( "<ArrayOfCodeItems ", "<ArrayOfCodeItem " );
          xmlCodeItem = xmlCodeItem.Replace( "</ArrayOfCodeItems>", "</ArrayOfCodeItem>" );
          xmlCodeItem = xmlCodeItem.Replace( "<CodeItems>", "<CodeItem>" );
          xmlCodeItem = xmlCodeItem.Replace( "</CodeItems>", "</CodeItem>" );

          Item.Items = Evado.Model.Digital.EvcStatics.DeserialiseObject<List<EdExternalSelectionList.CodeItem>>( xmlCodeItem );
        }
        Item.Authors = EvSqlMethods.getString( Row, "FSL_Authors" );
        Item.ReviewedBy = EvSqlMethods.getString( Row, "FSL_Reviewer" );
        Item.ReviewedByUserId = EvSqlMethods.getString( Row, "FSL_ReviewerUserId" );
        Item.ReviewDate = EvSqlMethods.getDateTime( Row, "FSL_ReviewDate" );
        Item.ApprovedBy = EvSqlMethods.getString( Row, "FSL_Approver" );
        Item.ApprovedByUserId = EvSqlMethods.getString( Row, "FSL_ApproverUserId" );
        Item.ApprovalDate = EvSqlMethods.getDateTime( Row, "FSL_ApprovalDate" );
        Item.Version = EvSqlMethods.getString( Row, "FSL_Version" );
        Item.State = Evado.Model.EvStatics.parseEnumValue<EdExternalSelectionList.SelectionListStates>(
          EvSqlMethods.getString( Row, "FSL_State" ) );
        Item.UpdatedByUserId = EvSqlMethods.getString( Row, "FSL_UpdatedByUserId" );
        Item.UpdatedBy = EvSqlMethods.getString( Row, "FSL_UpdatedBy" );
        Item.UpdatedDate += EvSqlMethods.getDateTime( Row, "FSL_UpdateDate" );

      // 
      // Return teh newField
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
    public List<EdExternalSelectionList> getView ( 
      EdExternalSelectionList.SelectionListStates State )
    {
      //
      // Initialize the method status and a return list of formfield selectionlist object
      //
      this.LogMethod( "getView method.");
      this._Log.AppendLine( "State: " + State );

      List<EdExternalSelectionList> view = new List<EdExternalSelectionList>( );

      //
      // Define the sql query parameter and load the query values.
      // 
      SqlParameter [] cmdParms = new SqlParameter [] 
      {
        new SqlParameter(PARM_State, SqlDbType.VarChar, 20)
      };
      cmdParms [ 0 ].Value = State.ToString();

      //
      // Define the sql query string. 
      // 
      _sqlQueryString = _sqlQuery_View;
      if ( State != EdExternalSelectionList.SelectionListStates.Null )
      {
        _sqlQueryString += " WHERE ( FSL_State = @State ) " 
          + " ORDER BY ListId, FSL_Version;";
      }
      else
      {
        _sqlQueryString += " WHERE ( FSL_State <> '" + EdExternalSelectionList.SelectionListStates.Withdrawn + "' ) "
          + " ORDER BY ListId, FSL_Version;";
      }


        this.LogDebug ( "SQL Query: " + _sqlQueryString);
      //
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery( _sqlQueryString, cmdParms ) )
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

          EdExternalSelectionList selectionList = this.readDataRow( row );

          // 
          // Append the value to the visit
          // 
          view.Add( selectionList );

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
      EdExternalSelectionList.SelectionListStates State, 
      bool SelectByGuid )
    {
      this.LogMethod ( "getList method." );
      this._Log.AppendLine( "State: " + State );
      this._Log.AppendLine( "SelectByUid: " + SelectByGuid.ToString( ) );

      //
      // Initialise the local variables
      //  
      List<EvOption> list = new List<EvOption>( );
      EvOption option = new EvOption( );
      list.Add( option );

      //
      // Initialise the query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(PARM_State, SqlDbType.VarChar, 20),
      };
      cmdParms [ 0 ].Value = State.ToString( );

      //
      // Build the query string
      // 
      _sqlQueryString = _sqlQuery_View;
      if ( State != EdExternalSelectionList.SelectionListStates.Null )
      {
        _sqlQueryString += " WHERE ( FSL_State = @State ) "
          + " ORDER BY ListId, FSL_Version;";
      }
      else
      {
        _sqlQueryString += " WHERE ( FSL_State <> '" + EdExternalSelectionList.SelectionListStates.Withdrawn + "' ) "
          + " ORDER BY ListId, FSL_Version;";
      }

      this.LogDebug ( "SQL Query: " + _sqlQueryString );

      //
      //Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery( _sqlQueryString, cmdParms ) )
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
          //
          // If SelectByGuid = True then optionId is to be the objects TestReport UID
          //
          if ( SelectByGuid == true )
          {
            option = new EvOption(
              EvSqlMethods.getString( row, "FSL_Guid" ),
              EvSqlMethods.getString( row, "ListId" ) + " - " + EvSqlMethods.getString( row, "FSL_Title" ) );
          }

          //
          // If SelectByGuid = False then optionId is to be the objects ListId.
          //
          else
          {
            option = new EvOption(
              EvSqlMethods.getString( row, "ListId" ),
              EvSqlMethods.getString( row, "ListId" ) + " - " + EvSqlMethods.getString( row, "FSL_Title" ) );
          }

          list.Add( option );

          if ( Count > _MaximumListLength )
          {
            break;
          }

        } //END interation loop.

      }//END using statement

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
    public EdExternalSelectionList getItem( Guid ListGuid )
    {
      this.LogMethod ( "getItemmethod." );
      this._Log.AppendLine( "ListGuid: " + ListGuid );
      //
      // Initialise the local variables
      //
      EdExternalSelectionList item = new EdExternalSelectionList( );

      //
      // If TestReport UID is null return empty checlist object.
      //
      if ( ListGuid == Guid.Empty )
      {
        return item;
      }

      // 
      // Generate the Selection query string.
      // 
      _sqlQueryString = _sqlQuery_View + " WHERE FSL_Guid = @Guid;";

      this.LogDebug ( _sqlQueryString );

      //
      // Initialise the query parameters.
      //
      SqlParameter cmdParms = new SqlParameter( PARM_Guid, SqlDbType.UniqueIdentifier);
      cmdParms.Value = ListGuid;

      //
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery( _sqlQueryString, cmdParms ) )
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

        item = this.readDataRow( row );

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
    public EdExternalSelectionList getItem( 
      string ListId, 
      bool Issued )
    {
      this.LogMethod ( "getItem, ListId = " + ListId );
      //
      // Initialise the local variables
      //
      EdExternalSelectionList item = new EdExternalSelectionList( );

      //
      // If the ListId is null then return empty Ethics object.
      //
      if ( ListId == string.Empty)
      {
        return item;
      }

      // 
      // Generate the Selection query string.
      // 
      _sqlQueryString = _sqlQuery_View + " WHERE (ListId = @ListId) ";

      if ( Issued == true )
      {
        _sqlQueryString += " AND (FSL_State = '" + EdExternalSelectionList.SelectionListStates.Issued + "'); ";
      }

      this._Log.AppendLine( _sqlQueryString );

      //
      // Initialise the query parameters.
      //
      SqlParameter [] cmdParms = new SqlParameter [] 
      {
        new SqlParameter(PARM_ListId, SqlDbType.NVarChar, 10),
      };
      cmdParms [ 0 ].Value = ListId;

      //
      //Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery( _sqlQueryString, cmdParms ) )
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

        item = this.readDataRow( row );

      }//END Using 

      //
      // Return the TestReport data object.
      //
      return item;

    }// END getTrial method

    // ====================================================================================
    /// <summary>
    /// This class gets the list of item coding based on ListId and Category value
    /// </summary>
    /// <param name="ListId">string: the selection list identifier</param>
    /// <param name="Category">string: The selection category.</param>
    /// <returns>string: the string of item coding list</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Return an empty string if ListId is empty. 
    /// 
    /// 2. Define the sql query string and sql query parameters. 
    /// 
    /// 3. Execute the sql query string and store the results on data table. 
    /// 
    /// 4. Extract the first row to the formfield selection list object. 
    /// 
    /// 5. Loop through the formfield selection list object's code items 
    /// and add the values to the Option string. 
    /// 
    /// 6. Return the Option string. 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public string getItemCodingList( 
      string ListId, 
      string Category )
    {
      this.LogMethod ( "getItemCodingList method. " );
      this._Log.AppendLine( "ListId = " + ListId);
      this._Log.AppendLine( "Category = " + Category );

      //
      // Initialise the local variables
      //
      EdExternalSelectionList item = new EdExternalSelectionList( );
      string EvOptions = string.Empty;

      //
      // If the ListId is null then return empty Ethics object.
      //
      if ( ListId == String.Empty )
      {
        this._Log.AppendLine( " List empty" );
        return EvOptions;
      }

      // 
      // Generate the Selection query string.
      // 
      _sqlQueryString = _sqlQuery_View + " WHERE (ListId = @ListId) "
        + " AND (FSL_State = '" + EdExternalSelectionList.SelectionListStates.Issued + "'); ";

      this._Log.AppendLine( _sqlQueryString );

      //
      // Initialise the query parameters.
      //
      SqlParameter [] cmdParms = new SqlParameter [] 
      {
        new SqlParameter(PARM_ListId, SqlDbType.NVarChar, 10),
      };
      cmdParms [ 0 ].Value = ListId;

      //
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery( _sqlQueryString, cmdParms ) )
      {
        // 
        // If not rows the return
        // 
        if ( table.Rows.Count > 0 )
        {
          // 
          // Extract the table row
          // 
          DataRow row = table.Rows [ 0 ];

          item = this.readDataRow( row );
        }

      }//END Using 

      this.LogDebug ( " Coding item count: " + item.Items.Count );
      // 
      // Extract the options to be used.
      // For Category = null all options are passed.
      // 
      foreach ( EdExternalSelectionList.CodeItem codeItem in item.Items )
      {
        if ( Category == String.Empty
          || codeItem.Category.Contains( Category ) )
        {
          if ( EvOptions != String.Empty )
          {
            EvOptions += ";";

          }
          EvOptions += codeItem.Value + ": " + codeItem.Description;
        }
      }

      //
      // Return the TestReport data object.
      //
      return EvOptions;

    }// END getItemCodingList method

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
    public EvEventCodes updateItem( EdExternalSelectionList Item )
    {
      this.LogMethod ( "updateItem method. " );

      // 
      // Get the previous value
      // 
      EdExternalSelectionList oldItem = getItem( Item.Guid );
      if ( oldItem.Guid == Guid.Empty )
      {
        return EvEventCodes.Identifier_Global_Unique_Identifier_Error;
      }

      // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      // Compare the objects.
      // 
      EvDataChanges dataChanges = new EvDataChanges( );
      EvDataChange dataChange = new EvDataChange( );
      dataChange.TableName = EvDataChange.DataChangeTableNames.EvFormFieldSelectionLists;
      dataChange.TrialId = Item.TrialId;
      dataChange.RecordUid = Item.Uid;
      dataChange.RecordGuid = Item.Guid;
      dataChange.UserId = Item.UpdatedByUserId;
      dataChange.DateStamp = DateTime.Now;

      //
      // Add items to datachange object if they exist. 
      //
      if ( Item.ListId != oldItem.ListId )
      {
        dataChange.AddItem( "ItemId", oldItem.ListId, Item.ListId );
      }
      if ( Item.Title != oldItem.Title )
      {
        dataChange.AddItem( "Title", oldItem.Title, Item.Title );
      }
      if ( Item.Reference != oldItem.Reference )
      {
        dataChange.AddItem( "Reference", oldItem.Reference, Item.Reference );
      }
      if ( Item.Instructions != oldItem.Instructions )
      {
        dataChange.AddItem( "Instructions", oldItem.Instructions, Item.Instructions );
      }
      if ( Item.Authors != oldItem.Authors )
      {
        dataChange.AddItem( "Authors", oldItem.Authors, Item.Authors );
      }
      if ( Item.ReviewedBy == oldItem.ReviewedBy )
      {
        dataChange.AddItem( "Reviewer", oldItem.ReviewedBy, Item.ReviewedBy );
      }
      if ( Item.ReviewedByUserId == oldItem.ReviewedByUserId )
      {
        dataChange.AddItem( "ReviewerUserId", oldItem.ReviewedByUserId, Item.ReviewedByUserId );
      }
      if ( Item.ReviewDate != oldItem.ReviewDate )
      {
        dataChange.AddItem( "ReviewDate", oldItem.ReviewDate.ToString( "dd MMM yyyy HH:mm:ss" ), Item.ReviewDate.ToString( "dd MMM yyyy HH:mm:ss" ) );
      }
      if ( Item.ApprovedBy != oldItem.ApprovedBy )
      {
        dataChange.AddItem( "Approver", oldItem.ApprovedBy, Item.ApprovedBy );
      }
      if ( Item.ApprovedByUserId != oldItem.ApprovedByUserId )
      {
        dataChange.AddItem( "ApproverUserId", oldItem.ApprovedByUserId, Item.ApprovedByUserId );
      }
      if ( Item.ApprovalDate != oldItem.ApprovalDate )
      {
        dataChange.AddItem( "ApprovalDate", oldItem.ApprovalDate.ToString( "dd MMM yyyy HH:mm:ss" ), Item.ApprovalDate.ToString( "dd MMM yyyy HH:mm:ss" ) );
      }
      if ( Item.Version != oldItem.Version )
      {
        dataChange.AddItem( "Version", oldItem.Version, Item.Version );
      }
      if ( Item.State != oldItem.State )
      {
        dataChange.AddItem( "State", oldItem.State.ToString(), Item.State.ToString() );
      }

      string oldXmlValidationRules = Evado.Model.Digital.EvcStatics.SerialiseObject<EdExternalSelectionList.XmlValidationRules>( Item.ValidationRules ) ;
      string newXmlValidationRules = Evado.Model.Digital.EvcStatics.SerialiseObject<EdExternalSelectionList.XmlValidationRules>( Item.ValidationRules ) ;
      if ( oldXmlValidationRules != newXmlValidationRules )
      {
        dataChange.AddItem( "XmlValidationRules", oldXmlValidationRules, newXmlValidationRules );
      }

      string oldCodeItem = 
        Evado.Model.Digital.EvcStatics.SerialiseObject<List<EdExternalSelectionList.CodeItem>>( Item.Items ) ;

      string newCodeItem = 
        Evado.Model.Digital.EvcStatics.SerialiseObject<List<EdExternalSelectionList.CodeItem>>( Item.Items ) ;

      if ( newCodeItem != oldCodeItem )
      {
        dataChange.AddItem( "Items", oldCodeItem, newCodeItem );
      }
      // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

      // 
      // Define the query parameters
      // 
      SqlParameter [] commandParameters = GetParameters( );
      SetParameters( commandParameters, Item );

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate( _STORED_PROCEDURE_UpdateItem, commandParameters ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }
      // 
      // Add the change record
      // 
      dataChanges.AddItem( dataChange );

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
    public EvEventCodes addItem( EdExternalSelectionList Item )
    {
      this.LogMethod ( "addItem method. " );
      this.LogDebug ( "ListId: '" + Item.ListId );
      this.LogDebug ( "Version: '" + Item.Version + "'" );

      //---------------------- Check for duplicate TestReport identifiers. ------------------
      // 
      // Define the query parameters
      // 
      SqlParameter [] cmdParms = new SqlParameter [] 
      {
        new SqlParameter( PARM_ListId, SqlDbType.NVarChar, 10),
        new SqlParameter( PARM_State, SqlDbType.NVarChar, 25),
      };
      cmdParms [ 0 ].Value = Item.ListId;
      cmdParms [ 1 ].Value = Item.State;

      // 
      // Generate the SQL query string
      // 
      _sqlQueryString = _sqlQuery_View + " WHERE (ListId = @ListId) " +
        " AND ( FSL_State = @State );";

      this._Log.AppendLine( "Duplication SQL Query:" + _sqlQueryString );

      //
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( _sqlQueryString, cmdParms ) )
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
      Item.Guid = Guid.NewGuid( );

      // 
      // Define the query parameters
      // 
      SqlParameter [] commandParameters = GetParameters( );
      SetParameters( commandParameters, Item );

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate( _STORED_PROCEDURE_AddItem, commandParameters ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      _sqlQueryString = _sqlQuery_View + " WHERE (ListId = @ListId) "
        + "AND (FSL_UpdatedBy = @UpdatedBy ) "
        + "AND (FSL_UpdateDate = @UpdateDate);";

      this._Log.AppendLine( _sqlQueryString );

      Item = this.getItem( Item.Guid );

      this.LogDebug ( "New Guid: " + Item.Guid );

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
    public EvEventCodes WithdrawIssuedList( EdExternalSelectionList Item )
    {
      this.LogMethod ( "WithdrawIssuedList method. " );
      // 
      // Define the query parameters
      // 
      SqlParameter [] commandParameters = new SqlParameter []
      {
        new SqlParameter(PARM_Guid, SqlDbType.UniqueIdentifier),
        new SqlParameter(PARM_UpdatedByUserId, SqlDbType.NVarChar,100),
        new SqlParameter(PARM_UpdatedBy, SqlDbType.NVarChar, 100),
        new SqlParameter(PARM_UpdateDate, SqlDbType.DateTime)
      };
      commandParameters [ 0 ].Value = Item.Guid;
      commandParameters [ 1 ].Value = Item.UpdatedByUserId;
      commandParameters [ 2 ].Value = Item.UserCommonName;
      commandParameters [ 3 ].Value = DateTime.Now;

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate( _STORED_PROCEDURE_WithdrawItem, commandParameters ) == 0 )
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
    public EvEventCodes deleteItem( EdExternalSelectionList Item )
    {
      this.LogDebug ( Evado.Model.Digital.EvcStatics.CONST_METHOD_START 
        + "Evado.Dal.Clinical.EvFiledSelectionLists.deleteItem method. " );

      // 
      // Define the query parameters
      // 
      SqlParameter [] commandParameters = new SqlParameter []
      {
        new SqlParameter(PARM_Guid, SqlDbType.UniqueIdentifier),
        new SqlParameter(PARM_UpdatedByUserId, SqlDbType.NVarChar,100),
        new SqlParameter(PARM_UpdatedBy, SqlDbType.NVarChar, 100),
        new SqlParameter(PARM_UpdateDate, SqlDbType.DateTime)
      };
      commandParameters [ 0 ].Value = Item.Guid;
      commandParameters [ 1 ].Value = Item.UpdatedByUserId;
      commandParameters [ 2 ].Value = Item.UserCommonName;
      commandParameters [ 3 ].Value = DateTime.Now;

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate( _STORED_PROCEDURE_DeleteItem, commandParameters ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      return EvEventCodes.Ok;

    }// End deleteYear method.

    #endregion

    #region Copy TestReport methods

    // =====================================================================================
    /// <summary>
    /// This class copies the items from Formfield selection list object. 
    /// </summary>
    /// <param name="Item">EvFormFieldSelectionList: a formfield selection list object</param>
    /// <returns>EvEventCodes: an event code for copying items.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the user common name is empty. 
    /// 
    /// 2. Define the sql query parameters and execute the storeprocedure for copying items. 
    /// 
    /// 3. Exit, if there is no item copied 
    /// 
    /// 4. Else, Return an event code for copying items. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvEventCodes CopyList( EdExternalSelectionList Item )
    {
      this._Log.AppendLine( Evado.Model.Digital.EvcStatics.CONST_METHOD_START 
        +  "Evado.Dal.Clinical.EvFiledSelectionLists.CopyTest method " );
      this.LogDebug ( "UserCommonName " + Item.UserCommonName );

      // 
      // Initialise the methods variables and objects.
      // 
      int databaseRecordAffected = 0;

      // 
      // Check that the data object has valid identifiers to add it to the database.
      // 
      if ( Item.UserCommonName == String.Empty )
      {
        return EvEventCodes.Identifier_User_Common_Name_Error;
      }

      //
      // give the copied list a new guid to be unique.
      //
      Item.Guid = Guid.NewGuid ( );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [] cmdParms = GetParameters( );
      SetParameters( cmdParms, Item );

      //
      // Execute the update command.
      //
      databaseRecordAffected = EvSqlMethods.StoreProcUpdate( _STORED_PROCEDURE_CopyItem, cmdParms );

      if ( databaseRecordAffected == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      this._Log.AppendLine( "Records affected: " + databaseRecordAffected );

      //
      // Return the event code for copying items. 
      //
      return EvEventCodes.Ok;

    }//END CopyList method.
    #endregion

  }//END EvFormFieldSelectionLists class

}//END namespace Evado.Dal.Digital
