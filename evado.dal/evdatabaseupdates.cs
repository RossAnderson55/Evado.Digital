/* <copyright file="EvRecords.cs" company="EVADO HOLDING PTY. LTD.">
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

//Application specific class references.
using Evado.Model;
using Evado.Model.Digital;


namespace Evado.Dal
{
  /// <summary>
  /// Data Access Layer class for ApplicationEvent
  /// </summary>
  public class EvDataBaseUpdates : EvDalBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EvDataBaseUpdates ( )
    {
      this.ClassNameSpace = "Evado.Dal.Digital.EvDataBaseUpdates.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EvDataBaseUpdates ( EvClassParameters Settings )
    {
      this.ClassParameters = Settings;
      this.ClassNameSpace = "Evado.Dal.Digital.EvDataBaseUpdates.";

      if ( this.ClassParameters.LoggingLevel == 0 )
      {
        this.ClassParameters.LoggingLevel = Evado.Dal.EvStaticSetting.LoggingLevel;
      }
    }

    #endregion


    #region Class Initialization

    private static string _eventLogSource = ConfigurationManager.AppSettings [ "EventLogSource" ];

    #endregion

    #region Class Constant
    // 
    // This constant defines a selectionList query string.
    // 
    private const string _sqlQuery_View = "Select * FROM DB_Object_Update ";

    #endregion

    #region Class Property


    #endregion

    #region Data Reader method section

    // ==================================================================================
    /// <summary>
    /// This class reads the content of the SqlDataReader into the Facility data object.
    /// </summary>
    /// <param name="Row">DataRow: a row of data row table</param>
    /// <returns>EvDataBaseUpdate: an update database</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialise an update object.
    /// 
    /// 2. Extract the data object values and update values to the update object.
    /// 
    /// 3. Return the update object.
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public EvDataBaseUpdate readDataRow ( DataRow Row )
    {
      // 
      // Initialise an update object.
      // 
      EvDataBaseUpdate update = new EvDataBaseUpdate ( );

      // 
      // Extract the data object values and update values to the update object.
      // 
      update.Guid = EvSqlMethods.getGuid ( Row, "DBU_Guid" );
      update.UpdateNo = EvSqlMethods.getInteger ( Row, "DBU_Update_No" );
      update.UpdateDate = EvSqlMethods.getDateTime ( Row, "DBU_Update_Date" );
      update.Version = EvSqlMethods.getString ( Row, "DBU_Version" );
      update.Objects = EvSqlMethods.getString ( Row, "DBU_Object" );
      update.Description = EvSqlMethods.getString ( Row, "DBU_Description" );

      //
      // Return the update object.
      //
      return update;

    }// End readDataRow method.    
    #endregion

    #region Class Query method section

    // ==================================================================================
    /// <summary>
    /// This class gets a List containing a selectionList of ApplicationEvent data objects.
    /// </summary>
    /// <param name="Version">EvDataBaseUpdate.UpdateVersionList: A sorting order of the list</param>
    /// <param name="OrderBy">EvDataBaseUpdate.DataBaseUpdateOrderBy: The sorting order enumeration.</param>
    /// <returns>List of EvDataBaseUpdate: a view of database update list</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize a status string and a return view of database update list.
    /// 
    /// 2. Define the query string.
    /// 
    /// 3. Execute the query against the database.
    /// 
    /// 4. Iterate through the results extracting the role information.
    /// 
    /// 5. Extract the table row
    /// 
    /// 6. Append the value to the visit
    /// 
    /// 7. Return the view
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public List<EvDataBaseUpdate> getUpdateList (
      EvDataBaseUpdate.UpdateVersionList Version,
      EvDataBaseUpdate.UpdateOrderBy OrderBy )
    {
      //
      // Initialize a status string and a return database update view list. 
      //
      this.LogMethod( "getUpdateList method. " );
      this.LogDebug ( "Version: " + Version );
      this.LogDebug ( "OrderBy: " + OrderBy );
      List<EvDataBaseUpdate> view = new List<EvDataBaseUpdate> ( );

      // 
      // Define the query string.
      // 
      string sqlQueryString = _sqlQuery_View;

      switch ( Version )
      {
        case EvDataBaseUpdate.UpdateVersionList.Version_2:
          {
            sqlQueryString += "WHERE DBU_Version LIKE '%R2%' \r\n";
            break;
          }
        case EvDataBaseUpdate.UpdateVersionList.Version_3:
          {
            sqlQueryString += "WHERE DBU_Version LIKE '%R3%' \r\n";
            break;
          }
        case EvDataBaseUpdate.UpdateVersionList.Version_4:
          {
            sqlQueryString += "WHERE DBU_Version LIKE '%R4%' \r\n";
            break;
          }
        case EvDataBaseUpdate.UpdateVersionList.Version_4_1:
          {
            sqlQueryString += "WHERE DBU_Version LIKE '%R4.1%' \r\n";
            break;
          }
        case EvDataBaseUpdate.UpdateVersionList.Version_4_2:
          {
            sqlQueryString += "WHERE DBU_Version LIKE '%R4.2%' \r\n";
            break;
          }
        case EvDataBaseUpdate.UpdateVersionList.Version_4_3:
          {
            sqlQueryString += "WHERE DBU_Version LIKE '%R4.3%' \r\n";
            break;
          }
        case EvDataBaseUpdate.UpdateVersionList.Version_4_4:
          {
            sqlQueryString += "WHERE DBU_Version LIKE '%R4.4%' \r\n";
            break;
          }
        case EvDataBaseUpdate.UpdateVersionList.Version_4_5:
          {
            sqlQueryString += "WHERE DBU_Version LIKE '%R4.5%' \r\n";
            break;
          }
        case EvDataBaseUpdate.UpdateVersionList.Version_4_6:
          {
            sqlQueryString += "WHERE DBU_Version LIKE '%R4.6%' \r\n";
            break;
          }
        case EvDataBaseUpdate.UpdateVersionList.Version_4_7:
          {
            sqlQueryString += "WHERE DBU_Version LIKE '%R4.7%' \r\n";
            break;
          }
        case EvDataBaseUpdate.UpdateVersionList.Version_4_8:
          {
            sqlQueryString += "WHERE DBU_Version LIKE '%R4.8%' \r\n";
            break;
          }
        case EvDataBaseUpdate.UpdateVersionList.Version_5:
          {
            sqlQueryString += "WHERE DBU_Version LIKE '%R5%' \r\n";
            break;
          }
        case EvDataBaseUpdate.UpdateVersionList.Version_6:
          {
            sqlQueryString += "WHERE DBU_Version LIKE '%R6%' \r\n";
            break;
          }
        case EvDataBaseUpdate.UpdateVersionList.Version_7:
          {
            sqlQueryString += "WHERE DBU_Version LIKE '%R7%' \r\n";
            break;
          }
      }

      switch ( OrderBy )
      {
        case EvDataBaseUpdate.UpdateOrderBy.Date:
          {
            sqlQueryString += " ORDER BY DBU_Update_Date, DBU_Update_No ;";
            break;
          }

        case EvDataBaseUpdate.UpdateOrderBy.Version:
          {
            sqlQueryString += " ORDER BY DBU_Version, DBU_Update_Date  ;";
            break;
          }
        default:
          {
            sqlQueryString += " ORDER BY DBU_Update_No ;";
            break;
          }
      }
      this.LogDebug( sqlQueryString );
      // 
      // Execute the query against the database.
      // 
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, null ) )
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

          EvDataBaseUpdate update = this.readDataRow ( row );

          // 
          // Append the value to the visit
          // 
          view.Add ( update );

        } //END interation loop.

      }//END Using statement

      // 
      // Pass back the result arrray.
      // 
      return view;

    }//END getView method.

    #endregion

  }//END EvDataBaseUpdates class

}//END namespace Evado.Dal
