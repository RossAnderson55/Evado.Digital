/* <copyright file="DAL\EvReports.cs" company="EVADO HOLDING PTY. LTD.">
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
 * Description:
 *  This class handles the database query interface for the EvReports object.
 * 
 *  This class contains the following public properties:
 *   DebugLog:       Containing the exeuction status of this class, used for debugging the 
 *                 class from BLL or UI layers.
 * 
 *  This class contains the following public methods:
 * 
 *   getReport:      Executes a query to generate an reports.
 * 
 ****************************************************************************************/


using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;

//References to Evado specific libraries

using Evado.Model;
using Evado.Model.Digital;


namespace Evado.Dal.Digital
{
  /// <summary>
  /// Data Access Layer Class execute SQL query based reports
  /// 
  /// </summary>
  public class EdReports : EvDalBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EdReports ( )
    {
      this.ClassNameSpace = "Evado.Dal.Digital.EdReports.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EdReports ( EvClassParameters Settings )
    {
      this.ClassParameters = Settings;
      this.ClassNameSpace = "Evado.Dal.Digital.EdReports.";
    }

    #endregion

    #region Data Reader methods

    // =====================================================================================
    /// <summary>
    /// This class reads the content of the SqlDataReader into the Report data object.
    /// </summary>
    /// <param name="Row">DataRow: a data row object.</param>
    /// <param name="Columns">List of EvReportColumn: a list of report columns</param>
    /// <returns>EvReportRow: a report row object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Loop through the Report's columns list
    /// 
    /// 2. Switch data type to update the Report object's column values. 
    /// 
    /// 3. Return the Report object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private EvReportRow readRow ( DataRow Row, List<EvReportColumn> Columns )
    {
      //
      // Initialize the return report row object. 
      //
      EvReportRow reportRow = new EvReportRow ( Columns.Count );

      //
      // Loop through the report columns list.
      //
      for ( int columnCount = 0; columnCount < Columns.Count; columnCount++ )
      {
        //
        // Switch column's datatype for updating the column values. 
        //
        switch ( Columns [ columnCount ].DataType )
        {
          case EvReport.DataTypes.Bool:
            {
              reportRow.ColumnValues [ columnCount ] = EvSqlMethods.getBool ( Row, Columns [ columnCount ].SourceField ).ToString ( );
              break;
            }
          case EvReport.DataTypes.Date:
            {

              DateTime dateVal = EvSqlMethods.getDateTime ( Row, Columns [ columnCount ].SourceField );
              if ( dateVal != Evado.Model.EvStatics.CONST_DATE_NULL )
              {
                reportRow.ColumnValues [ columnCount ] = dateVal.ToString ( "dd MMM yyyy HH:mm:ss" );
              }
              else
              {
                reportRow.ColumnValues [ columnCount ] = "";
              }

              break;
            }
          case EvReport.DataTypes.Integer:
            {
              reportRow.ColumnValues [ columnCount ] = EvSqlMethods.getInteger ( Row, Columns [ columnCount ].SourceField ).ToString ( );
              break;
            }
          case EvReport.DataTypes.Float:
          case EvReport.DataTypes.Currency:
            {
              reportRow.ColumnValues [ columnCount ] = EvSqlMethods.getFloat ( Row, Columns [ columnCount ].SourceField ).ToString ( );
              break;
            }
          default:
            {
              String val = EvSqlMethods.getString ( Row, Columns [ columnCount ].SourceField );
              if ( val == "Null" )
              {
                val = String.Empty;
              }
              reportRow.ColumnValues [ columnCount ] = val;
              break;
            }

        }//END Type switch

      }//END column interation loop.

      //
      // Return the Report object
      //
      return reportRow;

    }// End readRow method.

    #endregion

    #region Database selectionList query methods

    // =====================================================================================
    /// <summary>
    /// This class retrieves the Report data table based on the selected report object. 
    /// </summary>
    /// <param name="Report">EvReport: (Mandatory) Report object with selection parameters</param>
    /// <returns>EvReport: a report data object.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define the sql qeury string. 
    /// 
    /// 2. Execute the sql query string and store the results on data table. 
    /// 
    /// 3. Loop through the table and extract data row to the Report object. 
    /// 
    /// 4. Return the Report data object.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvReport getReport ( EvReport Report )
    {
      this.LogMethod ( "getReport. " );
      this.LogDebug ( "ReportTypeId: " + Report.ReportType );
      this.LogDebug ( "Category: " + Report.Category );

      // 
      // Define the local variables
      // 
      string sqlQueryString;
      ArrayList view = new ArrayList ( );

      // 
      // Generate the SQL query string
      // 
      sqlQueryString = Report.SqlDataSource;
      bool isWhereSetted = false;

      if ( sqlQueryString.ToLower ( ).Contains ( "where" ) )
      {
        sqlQueryString = sqlQueryString + " ";
        isWhereSetted = true;
      }

      for ( int queryCount = 0; queryCount < Report.Queries.Length; queryCount++ )
      {

        if ( Report.Queries [ queryCount ].Value == null || Report.Queries [ queryCount ].Value == String.Empty )
        {
          continue;
        }

        if ( isWhereSetted == false )
        {
          sqlQueryString += " WHERE ";
          isWhereSetted = true;
        }
        else
        {
          sqlQueryString += " AND ";
        }

        sqlQueryString += Report.Queries [ queryCount ].FieldName + " " + Report.Queries [ queryCount ].getOperatorString ( ) + " ";

        switch ( Report.Queries [ queryCount ].DataType )
        {
          case EvReport.DataTypes.Text:
          case EvReport.DataTypes.Guid:
          case EvReport.DataTypes.Hidden:
            {
              sqlQueryString += "'" + Report.Queries [ queryCount ].Value + "'";
              break;
            }
          case EvReport.DataTypes.Date:
            {
              //TODO I dont know what to do with dates.
              sqlQueryString += "'" + Report.Queries [ queryCount ].Value + "'";
              break;
            }
          default:
            {
              sqlQueryString += Report.Queries [ queryCount ].Value;
              break;
            }

        }

      }//End of query iteration.

      sqlQueryString += getOrderByString ( Report );

      this.LogDebug ( sqlQueryString );

      //return Report;

      //
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, null ) )
      {

        // 
        // Iterate through the results extracting the Report information.
        // 
        for ( int Count = 0; Count < table.Rows.Count; Count++ )
        {
          // 
          // Extract the table row
          // 
          DataRow row = table.Rows [ Count ];

          Report.DataRecords.Add ( this.readRow ( row, Report.Columns ) );

        }//End For

      }//END Using

      this.LogDebug ( "View Count: " + Report.DataRecords.Count );

      // 
      // Return the ArrayList containing the Report data object.
      // 
      return Report;

    }  // Close getReport method.

    // =====================================================================================
    /// <summary>
    /// This class obtains the order by string from the columns of the Report.
    /// </summary>
    /// <param name="Report">EvReport: a report data object</param>
    /// <returns>string: a string for ordering the report columns</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Loop through the report columns and set the index name by identifier value. 
    /// 
    /// 2. Define the sql query string based on the indexNameById value. 
    /// 
    /// 3. Format the sql query string. 
    /// 
    /// 4. Return the sql query string. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private string getOrderByString ( EvReport Report )
    {
      this.LogMethod ( "getOrderByString. " );

      //
      // Initialize the local variables and objects
      //
      String sqlQueryString = string.Empty;
      string [ ] indexNamebyId = new string [ 6 ];

      indexNamebyId [ 0 ] = String.Empty;
      indexNamebyId [ 1 ] = String.Empty;
      indexNamebyId [ 2 ] = String.Empty;
      indexNamebyId [ 3 ] = String.Empty;
      indexNamebyId [ 4 ] = String.Empty;
      indexNamebyId [ 5 ] = String.Empty;

      //
      // Loop through the Report columns and set the index name by Id value. 
      //
      for ( int columnCounter = 0; columnCounter < Report.Columns.Count; columnCounter++ )
      {
        EvReportColumn column = Report.Columns [ columnCounter ];

        if ( column.SectionLvl < 0 || column.SectionLvl > 5 )
        {
          continue;
        }

        //
        // If the column is the detail, then the index is the first column.
        //
        if ( indexNamebyId [ column.SectionLvl ].Contains ( column.SourceField ) == false )
        {
          if ( indexNamebyId [ column.SectionLvl ] != String.Empty )
          {
            indexNamebyId [ column.SectionLvl ] += ", ";
          }
          indexNamebyId [ column.SectionLvl ] += column.SourceField;
        }

      }//END For index name by Id

      sqlQueryString += String.Empty;

      //
      // Set the sql query string values.
      //
      for ( int i = 1; i < 5; i++ )
      {
        if ( indexNamebyId [ i ] != String.Empty )
        {
          if ( sqlQueryString != String.Empty )
          {
            sqlQueryString += ", ";
          }
          sqlQueryString += indexNamebyId [ i ];
        }

      }//END For sql query string. 

      if ( indexNamebyId [ 0 ] != String.Empty )
      {
        if ( sqlQueryString != String.Empty )
        {
          sqlQueryString += ", ";
        }
        sqlQueryString += indexNamebyId [ 0 ];
      }

      sqlQueryString = " ORDER BY " + sqlQueryString;
      this.LogDebug ( "sqlQueryString: " + sqlQueryString );

      //
      // Return the sql query string. 
      //
      return sqlQueryString;
    }//END getOrderByString class

    #endregion

  }//END EvReports class

}//END namespace Evado.Dal.Digital
