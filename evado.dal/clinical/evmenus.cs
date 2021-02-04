/***************************************************************************************
 * <copyright file="dal\menus.aspx.cs" company="EVADO HOLDING PTY. LTD.">
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

//Application specific class references.
//using  Evado.Model.Digital;


namespace Evado.Dal.Clinical
{
  /// <summary>
  /// This class is handles the data access layer for the menu item data object.
  /// </summary>
  public class EvMenus : EvDalBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EvMenus ( )
    {
      this.ClassNameSpace = "Evado.Dal.Clinical.EvMenus.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EvMenus ( Evado.Model.Digital.EvClassParameters Settings )
    {
      this.ClassParameters = Settings;
      this.ClassNameSpace = "Evado.Dal.Clinical.EvMenus.";

      if ( this.ClassParameters.LoggingLevel == 0 )
      {
        this.ClassParameters.LoggingLevel = Evado.Dal.EvStaticSetting.LoggingLevel;
      }
    }

    #endregion

    #region Class Constants

    // 
    // This constant defines a selectionList query string.
    // 
    private const string _sqlQuery_View = "Select * FROM EV_MENUS ";

    #region Define the storeprocedure name
    // 
    // This constant defines the add item stored procedure.
    // 
    private const string _storedProcedureAddItem = "usr_Menu_add";

    // 
    // This constant defines the update item stored procedure.
    // 
    private const string _storedProcedureUpdateItem = "usr_Menu_update";

    // 
    // This constant defines the delete item stored procedure.
    // 
    private const string _storedProcedureDeleteItem = "usr_Menu_delete";
    #endregion

    #region Define query parameter
    // 
    // This constant defines a global unique identifier parameter
    // 
    private const string PARM_GUID = "@GUID";

    // 
    // This constant defines a page identifier parameter
    // 
    private const string PARM_PAGE_ID = "@PAGE_ID";

    // 
    // This constant defines a name parameter
    // 
    private const string PARM_TITLE = "@TITLE";


    // 
    // This constant defines an order parameter
    // 
    private const string PARM_ORDER = "@ORDER";

    // 
    // This constant defines a group parameter
    // 
    private const string PARM_GROUP = "@GROUP";

    // 
    // This constant defines a group header parameter
    // 
    private const string PARM_GROUP_HEADER = "@GROUP_HEADER";

    // 
    // This constant defines site 1 parameter
    // 
    private const string PARM_PLATFORM = "@PLATFORM";

    // 
    // This constant defines a module parameter
    // 
    private const string PARM_MODULES = "@MODULES";

    // 
    // This constant defines a role list parameter
    // 
    private const string PARM_ROLES = "@ROLES";
    #endregion

    #endregion

    #region Internal member variables
    //
    // Define the SQL query string variable.
    //
    private string _sqlQueryString = String.Empty;

    #endregion

    #region SQL Parameter methods

    // ==================================================================================
    /// <summary>
    /// This class returns an array of sql query parameters.  
    /// </summary>
    /// <returns>SqlParameter: a parameter object array list </returns>
    /// <remarks>
    /// This class consists of the following steps:
    /// 
    /// 1. Create an array of sql query parameters
    /// 
    /// 2. Return an arraylist of parameters
    /// </remarks>
    // ----------------------------------------------------------------------------------
    private static SqlParameter [ ] GetParameters ( )
    {
      //
      // Initialize an arraylist of sql parmeter object
      //
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        //
        // Add parameters into the arraylist
        //
        new SqlParameter( PARM_GUID, SqlDbType.UniqueIdentifier),
        new SqlParameter( PARM_PAGE_ID,SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_TITLE,SqlDbType.NVarChar, 20),
        new SqlParameter( PARM_ORDER,SqlDbType.SmallInt),
        new SqlParameter( PARM_GROUP,SqlDbType.NVarChar, 5),
        new SqlParameter( PARM_GROUP_HEADER,SqlDbType.Bit),
        new SqlParameter( PARM_PLATFORM,SqlDbType.NVarChar,10),
        new SqlParameter( PARM_MODULES,SqlDbType.NVarChar, 250),
        new SqlParameter( PARM_ROLES,SqlDbType.NVarChar, 250),
      };

      //
      // Return an arraylist of parameters
      //
      return cmdParms;

    }//END GetParameters method

    // ==================================================================================
    /// <summary>
    /// This class sets the query parameter values. 
    /// </summary>
    /// <param name="cmdParms">SqlParameter: an arraylist of Database parameters</param>
    /// <param name="MenuItem">EvMenuItem: a menus data object</param>
    /// <remarks>
    /// This method consists of the following step:
    /// 
    /// 1. Update MenuItem object'a values to the array of sql parameters. 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    private void SetParameters ( SqlParameter [ ] cmdParms,  Evado.Model.Digital.EvMenuItem MenuItem )
    {
      //
      // Update elements of MenuItem object to the cmdParms arraylist
      //
      cmdParms [ 0 ].Value = MenuItem.Guid;
      cmdParms [ 1 ].Value = MenuItem.PageId.ToString ( );
      cmdParms [ 2 ].Value = MenuItem.Title;
      cmdParms [ 3 ].Value = MenuItem.Order;
      cmdParms [ 4 ].Value = MenuItem.Group;
      cmdParms [ 5 ].Value = MenuItem.GroupHeader;
      cmdParms [ 6 ].Value = MenuItem.Platform;
      cmdParms [ 7 ].Value = String.Empty;
      cmdParms [ 8 ].Value = MenuItem.RoleList;

    }//END SetLetterParameters.

    #endregion

    #region ReaderData methods

    // =====================================================================================
    /// <summary>
    /// This class reads the content of the data reader object into Menus business object.    
    /// </summary>
    /// <param name="Row">DataRow: a sql data row</param>
    /// <returns>EvMenuItem: a row data of the menu item</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Add page identifier to a menu item object if it exists
    /// 
    /// 2. Append the row strings of EvSqlMethods object to the menu item object. 
    /// 
    /// 3. Rename the role visitSchedule members and add them to the menu item object. 
    /// 
    /// 4. Return the menu item object
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public  Evado.Model.Digital.EvMenuItem getRowData ( DataRow Row )
    {
      //this.LogMethod( "getRowData method" );

      //
      // Initialise the menu item object and a page identifier string. 
      //
       Evado.Model.Digital.EvMenuItem menu = new Evado.Model.Digital.EvMenuItem ( );

      menu.Guid = EvSqlMethods.getGuid ( Row, "MNU_GUID" );

      String stPageId = EvSqlMethods.getString ( Row, "MNU_PAGE_ID" );


      //
      // Add page identifier to a menu item object if it exists
      //
      if ( stPageId != String.Empty )
      {
        try
        {
          menu.PageId = Evado.Model.EvStatics.Enumerations.parseEnumValue<Evado.Model.Digital.EvPageIds> ( stPageId );
        }
        catch
        {
          menu.PageId = Evado.Model.Digital.EvPageIds.Null;
        }
      }

      //
      // Add the row string of EvSqlMethods object to the menu item object. 
      //
      menu.Title = EvSqlMethods.getString ( Row, "MNU_TITLE" );
      menu.Order = EvSqlMethods.getInteger ( Row, "MNU_ORDER" );
      menu.Group = EvSqlMethods.getString ( Row, "MNU_GROUP" );
      menu.GroupHeader = EvSqlMethods.getBool ( Row, "MNU_GROUP_HEADER" );
      menu.Platform = EvSqlMethods.getString ( Row, "MNU_PLATFORM" );
      menu.RoleList = EvSqlMethods.getString ( Row, "MNU_ROLES" );

      return menu;

    }//END getRowData method.

    #endregion

    #region Query methods

    // ==================================================================================
    /// <summary>
    /// This class gets a List containing a EvMenutItem data objects.
    /// </summary>
    /// <param name="PlatformId">String: (Optional) The Site identifier.</param>
    /// <param name="Group">String: (Optional) The Group identifier.</param>
    /// <returns>List of Evado.Model.Digital.EvMenuItem : a view of menu item list</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Define the SQL query parameters and a query string. 
    /// 
    /// 2. Execute the query and store the results on datatable. 
    /// 
    /// 3. Loop through the table and extract data row to the MenuItem object. 
    /// 
    /// 4. Add MenuItem object's values to the MenuItem list. 
    /// 
    /// 5. Return the MenuItem list. 
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public List< Evado.Model.Digital.EvMenuItem > getView ( 
      String PlatformId, 
      String Group )
    {
      // 
      // Initialize a debug log 
      //
      this.LogMethod( "getView method" );
      this.LogDebug ( "PlatformId: " + PlatformId );
      this.LogDebug ( "Group: " + Group );

      // 
      // Initialize a return list of menu item, site 1 and site 2. 
      // 
      List< Evado.Model.Digital.EvMenuItem > view = new List< Evado.Model.Digital.EvMenuItem > ( );

      // 
      // Define the SQL query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( PARM_PLATFORM, SqlDbType.VarChar,10),
        new SqlParameter( PARM_GROUP, SqlDbType.NVarChar, 50),
      };
      cmdParms [ 0 ].Value = PlatformId;
      cmdParms [ 1 ].Value = Group;

      // 
      // Define the query string.
      // 
      _sqlQueryString = _sqlQuery_View + " WHERE (MNU_PLATFORM = @PLATFORM)";

      if ( Group != String.Empty )
      {
        _sqlQueryString += " AND (Mnu_Group = @Group) ";
      }

      _sqlQueryString += " ORDER BY   Mnu_Order";

      this.LogDebug( _sqlQueryString );

      // 
      // Execute the query against the database.
      // 
      using ( DataTable table = EvSqlMethods.RunQuery ( _sqlQueryString, cmdParms ) )
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

           Evado.Model.Digital.EvMenuItem menu = this.getRowData ( row );

          // 
          // Append the value to the visit
          // 
          view.Add ( menu );

        } //END interation loop.

      }//END using method

      //
      // Update debug log with a view count number
      //
      this.LogDebug( "view count: " + view.Count );
      // 
      // Pass back the result arrray.
      // 
      return view;

    } // Close getView method.

    // ==================================================================================
    /// <summary>
    /// This class gets a ArrayList containing a selectionList of Menus data identifiers.
    /// </summary>
    /// <param name="PlatformId">string: (Optional) The Site identifier.</param>    
    /// <returns>List of Evado.Model.EvOption: a list of groups</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Define the SQL query parameters and a query string. 
    /// 
    /// 2. Execute the query and store the results on datatable. 
    /// 
    /// 3. Loop through the table and extract data row to the Option object. 
    /// 
    /// 4. Add Option object's values to the Options list. 
    /// 
    /// 5. Return the Option list. 
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public List<Evado.Model.EvOption> getGroupList ( 
      String PlatformId )
    {
      this.LogMethod ( "getGroupList, " );
      this.LogDebug ( " PlatformId: " + PlatformId );
      //
      // Initialize a debug log, a return list of options, an option object, site 1 and site 2
      //
      List<Evado.Model.EvOption> List = new List<Evado.Model.EvOption> ( );
      Evado.Model.EvOption option = new Evado.Model.EvOption ( );

      // 
      // Define the SQL query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( PARM_PLATFORM, SqlDbType.VarChar,10),
      };
      cmdParms [ 0 ].Value = PlatformId;

      // 
      // Define the query string.
      // 
      _sqlQueryString = _sqlQuery_View + " WHERE ( MNU_PLATFORM = @PLATFORM)"
        + " AND (MNU_GROUP_HEADER = 1) ORDER BY MNU_ORDER";

      this.LogDebug( _sqlQueryString );

      // 
      // Execute the query against the database.
      // 
      using ( DataTable table = EvSqlMethods.RunQuery ( _sqlQueryString, cmdParms ) )
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
          // Process the results into the visitSchedule.
          // 
          String stGroup = EvSqlMethods.getString ( row, "MNU_GROUP" );
          String stTitle = EvSqlMethods.getString ( row, "MNU_TITLE" );

          //
          // if the group does not exist add it to the list.
          //
          if ( GroupExists ( List, stGroup ) == false )
          {
            // 
            // Append the new Menus object to the array.
            // 
            List.Add ( new Evado.Model.EvOption ( stGroup, stTitle ) );
          }
        }
      }

      // 
      // Pass back the result arrray.
      // 
      return List;

    }//END getGroupList method.

    // ==================================================================================
    /// <summary>
    /// This method tests to see if the group exists.
    /// </summary>
    /// <param name="List">list of Evado.Model.EvOption: a list of option object.</param>
    /// <param name="Group">String: a group header</param>
    /// <returns>Boolean: true, if the group exists</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Loop through the list of option object
    /// 
    /// 2. Return true, if Group value exists in the list. 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    private bool GroupExists ( List<Evado.Model.EvOption> List, String Group )
    {
      //
      // Loop through the list of option object
      //
      foreach ( Evado.Model.EvOption optn in List )
      {
        //
        // Return true, if Group value exists in the list. 
        //
        if ( optn.Value == Group )
        {
          return true;
        }
      }//END foreach loop

      return false;

    }//END GroupExists method

    #endregion

    #region Retreival methods

    // =====================================================================================
    /// <summary>
    /// This class gets Menus data object by its unique object identifier.
    /// </summary>
    /// <param name="MenuGuid">Guid: (Mandatory)a global unique object identifier.</param>
    /// <returns>EvMenuItem: a menu data object</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Return an empty MenuItem object, if the Guid is empty.
    /// 
    /// 2. Define the SQL query parameters and a query string. 
    /// 
    /// 3. Execute the query string and store the results on datatable.
    /// 
    /// 4. Return an empty MenuItem object if the table has no value.
    /// 
    /// 5. Else, Extract the table row to the MenuItem object. 
    /// 
    /// 6. Return the MenuItem object. 
    ///  
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public  Evado.Model.Digital.EvMenuItem getItem ( Guid MenuGuid )
    {
      this.LogMethod ( "getItem" );
      this.LogDebug ( "MenuGuid: " + MenuGuid );
      //
      // Initialize a debug log and a menu item object
      //
       Evado.Model.Digital.EvMenuItem menuItem = new Evado.Model.Digital.EvMenuItem ( );

      // 
      // Check that there is a valid unique identifier.
      // 
      if ( MenuGuid == Guid.Empty )
      {
        return menuItem;
      }

      // 
      // Define the query parameters.
      // 
      SqlParameter cmdParms = new SqlParameter ( PARM_GUID, SqlDbType.UniqueIdentifier );
      cmdParms.Value = MenuGuid;

      // 
      // Define the query string.
      // 
      _sqlQueryString = _sqlQuery_View + " WHERE (Mnu_Guid = @Guid ); ";

      // 
      // Execute the query against the database.
      // 
      using ( DataTable table = EvSqlMethods.RunQuery ( _sqlQueryString, cmdParms ) )
      {
        // 
        // If not rows the return
        // 
        if ( table.Rows.Count == 0 )
        {
          return menuItem;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        // 
        // Process the results.
        // 
        menuItem = this.getRowData ( row );

      }//END Using statement

      // 
      // Pass back the data object.
      // 
      return menuItem;

    }//END getItem class

    // =====================================================================================
    /// <summary>
    /// This class gets Menus data object by its object identifier.
    /// </summary>
    /// <param name="Title">string: a name menu item</param>
    /// <returns>EvMenuItem: a menu item</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Return an empty MenuItem object, if the Name is empty
    /// 
    /// 2. Define the SQL query parameters and a query string. 
    /// 
    /// 3. Execute the query string and store the results on datatable.
    /// 
    /// 4. Return an empty MenuItem object if the table has no value.
    /// 
    /// 5. Else, Extract the table row to the MenuItem object. 
    /// 
    /// 6. Return the MenuItem object. 
    ///  
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public  Evado.Model.Digital.EvMenuItem getItem ( string Title )
    {
      this.LogMethod ( "getItem" );
      this.LogDebug ( "Title: " + Title );
      //
      // Initialize a debug log and a menu item object
      //
       Evado.Model.Digital.EvMenuItem menuItem = new Evado.Model.Digital.EvMenuItem ( );

      // 
      // Check that there is a valid name.
      // 
      if ( Title == String.Empty )
      {
        return menuItem;
      }

      // 
      // Define the query parameters.
      // 
      SqlParameter cmdParms = new SqlParameter ( PARM_TITLE, SqlDbType.NVarChar, 20 );
      cmdParms.Value = Title;

      // 
      // Define the query string.
      // 
      _sqlQueryString = _sqlQuery_View + " WHERE (Mnu_Title = @Title ); ";

      // 
      // Execute the query against the database.
      // 
      using ( DataTable table = EvSqlMethods.RunQuery ( _sqlQueryString, cmdParms ) )
      {
        // 
        // If not rows the return
        // 
        if ( table.Rows.Count == 0 )
        {
          return menuItem;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        // 
        // Process the results.
        // 
        menuItem = this.getRowData ( row );

      }//END Using statement

      return menuItem;

    }// Close method getMenus.    
    #endregion

    #region MenuItem Update methods

    // ==================================================================================
    /// <summary>
    /// This class updates Menus data object in the database using it unique object identifier.    
    /// </summary>    
    /// <param name="MenuItem">EvMenuItem: menus data object</param>
    /// <returns>Evado.Model.EvEventCodes: an update item code</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Exit, if the title is not defined. 
    /// 
    /// 2. Define the SQL query parameters and load the query values.
    /// 
    /// 3. Execute the update command.
    /// 
    /// 4. Return update item code
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Evado.Model.EvEventCodes updateItem (  Evado.Model.Digital.EvMenuItem MenuItem )
    {
      this.LogMethod ( "updateItem method. " );
      this.LogDebug ( "Guid: " + MenuItem.Guid );
      this.LogDebug ( "Title: " + MenuItem.Title );
      this.LogDebug ( "RoleList: " + MenuItem.RoleList );
      this.LogDebug ( "Platform: " +  MenuItem.Platform ); 
      //
      // Initialize a debug log
      //

      // 
      // Validate whether the menu title exists
      // 
      if ( MenuItem.Title == String.Empty )
      {
        return Evado.Model.EvEventCodes.Data_InvalidId_Error;
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] _cmdParms = GetParameters ( );
      SetParameters ( _cmdParms, MenuItem );

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( _storedProcedureUpdateItem, _cmdParms ) == 0 )
      {
        return Evado.Model.EvEventCodes.Database_Record_Update_Error;
      }

      return Evado.Model.EvEventCodes.Ok;

    }//END updateItem method

    // ==================================================================================
    /// <summary>
    /// This class adds an Menus data object to the database.
    /// </summary>
    /// <param name="MenuItem">EvMenuItem: Menus data object</param>
    /// <returns>Evado.Model.EvEventCodes: an add item code</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Exit, if the Title is empty. 
    /// 
    /// 2. Add the new menu item guid
    /// 
    /// 3. Define the SQL query parameters and load the query values.
    /// 
    /// 4. Execute the update command.
    /// 
    /// 5. Return update item code
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Evado.Model.EvEventCodes addItem (  Evado.Model.Digital.EvMenuItem MenuItem )
    {
      this.LogMethod ( "addItem method" );
      this.LogDebug ( "Name: " + MenuItem.Title );
      //
      // Initialize a debug log 
      //

      // 
      // Validate whether the menu title exists
      // 
      if ( MenuItem.Title == String.Empty )
      {
        return Evado.Model.EvEventCodes.Data_InvalidId_Error;
      }

      // 
      // Add the the menu item guid.
      // 
      MenuItem.Guid = Guid.NewGuid ( );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] _cmdParms = GetParameters ( );
      SetParameters ( _cmdParms, MenuItem );

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( _storedProcedureAddItem, _cmdParms ) == 0 )
      {
        return Evado.Model.EvEventCodes.Database_Record_Update_Error;
      }

      return Evado.Model.EvEventCodes.Ok;

    }//END addItem class

    // ==================================================================================
    /// <summary>
    /// This class deletes items from MenuItem table. 
    /// </summary>
    /// <param name="MenuItem">EvMenuItem: a menu item object</param>
    /// <returns>Evado.Model.EvEventCodes: a delete item code</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Exit, if the Guid is empty. 
    /// 
    /// 2. Define the SQL query parameters and load the query values.
    /// 
    /// 3. Execute the update command.
    /// 
    /// 4. Return update item code
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Evado.Model.EvEventCodes deleteItem (  Evado.Model.Digital.EvMenuItem MenuItem )
    {
      this.LogMethod ( "addItem method" );
      this.LogDebug ( "Guid: " + MenuItem.Guid );
      //
      // Initialize a debug log
      //

      //
      // Validate whether the menu guid exists
      //
      if ( MenuItem.Guid == Guid.Empty )
      {
        return Evado.Model.EvEventCodes.Data_InvalidId_Error;
      }

      // 
      // Define the query parameters.
      // 
      SqlParameter [ ] _cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(PARM_GUID, SqlDbType.UniqueIdentifier)
      };
      _cmdParms [ 0 ].Value = MenuItem.Guid;

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( _storedProcedureDeleteItem, _cmdParms ) == 0 )
      {
        return Evado.Model.EvEventCodes.Database_Record_Update_Error;
      }

      return Evado.Model.EvEventCodes.Ok;

    }//END deleteItem class

    #endregion

  }//END EvMenus class

}//END namespace Evado.Dal
