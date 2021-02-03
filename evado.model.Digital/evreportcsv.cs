/***************************************************************************************
 * <copyright file="model\EvReport.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2020 EVADO HOLDING PTY. LTD..  All rights reserved.
 *     
 *      The use and distribution terms for this software are contained in the file
 *      named \license.txt, which can be found in the root of this distribution.
 *      By using this software in any fashion, you are agreeing to be bound by the
 *      terms of this license.
 *     
 *      You must not remove this notice, or any other, from this software.
 *     
 * </copyright>
 * 
 * Description: 
 *  This class contains the EvReportCsv Data object..
 *
 ****************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Evado.Model.Digital
{
  /// <summary>
  /// This class will generate a csv report.
  /// </summary>
  class EvReportCsv : EvReportGenerator
  {

    #region Initialization
    public EvReportCsv ( EvReport report, string separator, EdUserProfile userProfile )
    {
      _report = report;
      _separator = separator;
    }
    #endregion

    #region Constant
    /// <summary>
    /// This constant defines a string delimiter for a report generator.
    /// </summary>
    private const String StringDelimiter = "\"";
    #endregion

    #region internal member variables
    private String ClassNameSpace = "Evado.Model.Digital.EvReportCsv.";
    /// <summary>
    ///  The State contains debug status of the class.
    /// </summary>
    StringBuilder _Log = new StringBuilder ( );
    /// <summary>
    /// this property contains the class log file.
    /// </summary>
    public String Log
    {
      get { return _Log.ToString ( ); }
    }

    /// <summary>
    /// The separator of each field on a csv file.
    /// </summary>
    string _separator = String.Empty;
    #endregion

    #region methods
    // ==================================================================================
    /// <summary>
    /// This is the method who will return the actual text of the report.
    /// Depending on the implementation, it will return an html report, 
    /// an xml report a csv report and so on.
    /// </summary>
    /// <returns>string: a report text</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Loads the herarchy model of Section of Objects.
    /// 
    /// 2. If there is no data then do not build the report
    /// 
    /// 3. Generates the report header. This includes the titles and the queries.
    /// 
    /// 4. Generates the grouped report. To pain a flat report, 
    /// only the detail should be specified.
    /// 
    /// 5. Return the report content formatted as Csv.
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public override string getReportData ( )
    {
      this.LogMethod ( "getReportData method." );
      //
      // Define the methods variables and objects.
      //
      StringBuilder stCsv = new StringBuilder ( );

      // 
      // If there is no data then do not build the report
      // 
      if ( this._report == null )
      {
        this.LogValue ( "No Data." );
        return String.Empty;
      }

      //
      // Add the query name to the output data.
      //
      String QueryData = String.Empty;
      int queryCount = 0;
      foreach ( EvReportQuery query in this._report.Queries )
      {
        if ( query == null )
        {
          continue;
        }

        if ( query.FieldName != String.Empty )
        {
          if ( queryCount > 0 )
          {
            QueryData += this._separator ;
          }
          QueryData += this.getTextCSV ( "Q:"+query.FieldName );

        }
        queryCount++;
      }//END query header iteration loop.

      if ( QueryData != String.Empty )
      {
        stCsv.Append ( QueryData );
        stCsv.Append ( this._separator );
      }

      //
      // Output the report column header.
      //
      for ( int col = 0; col < this._report.Columns.Count; col++ )
      {
        EvReportColumn column = this._report.Columns [ col ];
        if ( col > 0 )
        {
          stCsv.Append ( this._separator );
        }
        if ( column.ColumnId == String.Empty )
        {
          stCsv.Append ( this.getTextCSV ( column.SourceField ) );
        }
        else
        {
          stCsv.Append ( this.getTextCSV ( column.ColumnId ) );
        }
      }
      stCsv.AppendLine ( "" );



      //
      // Add the query values to the output data.
      //
      QueryData = String.Empty;
      queryCount = 0;
      foreach ( EvReportQuery query in this._report.Queries )
      {
        if ( query == null )
        {
          continue;
        }

        if ( query.FieldName != String.Empty )
        {
          if ( queryCount > 0 )
          {
            QueryData += this._separator ;
          }
          QueryData  += this.getTextCSV ( query.Value );

        }
        queryCount++;
      }//END query value iteration loop.

      //
      // Output the report data.
      //
      for ( int row = 0; row < this._report.DataRecords.Count; row++ )
      {
        if ( QueryData != String.Empty )
        {
          stCsv.Append ( QueryData );
          stCsv.Append ( this._separator );
        }

        //
        // the row of data.
        //
        EvReportRow rowData = this._report.DataRecords [ row ];
        
        //
        // column interation loop.
        //
        for ( int col = 0; col < rowData.ColumnValues.Length; col++ )
        {
          if ( col > 0 )
          {
            stCsv.Append ( this._separator );
          }

          stCsv.Append ( this.getTextCSV ( rowData.ColumnValues [ col ] ) );

        }//END column interation loop.

        stCsv.AppendLine ( "" );
      }//END row interation loop

      // 
      // Return the report content formatted as Csv.
      // 
      this.LogMethodEnd ( "getReportData" );
      return stCsv.ToString();

    }//End getReportData class

    // ==================================================================================
    /// <summary>
    /// This is the method who will return the actual text of the report.
    /// Depending on the implementation, it will return an html report, 
    /// an xml report a csv report and so on.
    /// </summary>
    /// <returns>string: a report text</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Loads the herarchy model of Section of Objects.
    /// 
    /// 2. If there is no data then do not build the report
    /// 
    /// 3. Generates the report header. This includes the titles and the queries.
    /// 
    /// 4. Generates the grouped report. To pain a flat report, 
    /// only the detail should be specified.
    /// 
    /// 5. Return the report content formatted as Csv.
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public override string getReportText ( )
    {
      this.LogMethod ( "getReportAsCsv method." );

      //
      // Loads the herarchy model of Section of Objects.
      //
      EvReportSection model = loadModel ( _report );

      //String with the csv formatting.
      string stCsv = String.Empty;

      // 
      // If there is no data then do not build the report
      // 
      if ( this._report == null )
      {
        this.LogValue ( "No Data." );
        return String.Empty;
      }
      //
      // Generates the report header. This includes the titles and the queries.
      //
      stCsv += getReportHeader ( );

      stCsv += "\n";

      //
      // Generates the grouped report. To pain a flat report, 
      // only the detail should be specified.
      //
      stCsv += getSection ( model );

      // 
      // Return the report content formatted as Csv.
      // 
      this.LogMethodEnd ( "getReportAsCsv" );
      return stCsv;

    }//End getReportText class

    //  =================================================================================
    /// <summary>
    /// This method generates the report header.
    /// </summary>
    /// <returns>String: a report header</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Generate a report title and subtitle
    /// 
    /// 2. Check that the query array exits prior to generating the header.
    /// 
    /// 3. Iterate through the query array generating each item in order of the array items
    /// 
    /// 4. If the item has a value then display it.
    /// 
    /// 5. Put the Report number in the top left cell
    /// </remarks>
    // ----------------------------------------------------------------------------------
    private String getReportHeader ( )
    {
      //
      // Generate a report title and subtitle
      //
      string stCsv = string.Empty;

      stCsv += getTextCSV ( _report.ReportTitle ) + "\n";

      if ( this._report.ReportSubTitle != null )
      {
        stCsv += getTextCSV ( this._report.ReportSubTitle ) + "\n";
      }//End report subtitle exists if.

      // 
      // Check that the query array exits prior to generating the header.
      // 
      if ( this._report.Queries != null )
      {
        // 
        // Iterate through the query array generating each item in order
        // of the array items.
        // 
        for ( int i = 0; i < this._report.Queries.Length; i++ )
        {
          // 
          // If the item has a value then display it.
          // 
          if ( this._report.Queries [ i ].Value != string.Empty )
          {
            stCsv += getTextCSV ( this._report.Queries [ i ].Prompt )
            + _separator
            + getTextCSV ( this._report.Queries [ i ].ValueName );

            //
            // The Report number should be always in the top left cell
            //
            if ( i == 0 )
            {
              stCsv += _separator + getTextCSV ( "Report No. " + this._report.ReportNo );
            }

            stCsv += "\n";

          }//END query value exists.

        }//END query interation loop

      }//END Query array exists.

      return stCsv;
    }//End getReportHeader class

    //  ================================================================================
    /// <summary>
    /// This class formats the text depending of the data type.
    /// </summary>
    /// <param name="text">String: a data type text</param>
    /// <param name="type">DataTypes: a data type object</param>
    /// <returns>String: a text in csv format</returns>
    /// <remarks>
    /// This method consists of the following step:
    /// 
    /// 1. If data type exists, convert a text to csv format by calling getTextCSV method.
    /// </remarks>
    /// 
    // ----------------------------------------------------------------------------------
    private String formatText ( String text, EvReport.DataTypes type )
    {
      if ( type == EvReport.DataTypes.Text )
      {
        return getTextCSV ( text );
      }

      return text;
    }//END formattext method.

    //  =================================================================================
    /// <summary>
    /// This class returns the text in the proper format.
    /// If the format changes, it will only affect this part of the code.
    /// </summary>
    /// <param name="text">String: a text</param>
    /// <returns>String: a csv text</returns>
    /// <remarks>
    /// This method consists of the following step:
    /// 
    /// 1. Append string delimiter to the beginning and ending of a text.
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    private String getTextCSV ( String text )
    {
      return StringDelimiter + text + StringDelimiter;
    }//END getTextCsv method.

    //  =================================================================================
    /// <summary>
    /// This class obtains the formatted section based on the model.
    /// </summary>
    /// <param name="section">EvReportSection: Section to be painted</param>    
    /// <returns>string: a report section</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize a csv string and columnlist array
    /// 
    /// 2. Set csv layout section: flat or tabulated
    /// 
    /// 3. Paints the children of this section if there is any.
    /// 
    /// 4. Iterate once for the headers, and then iterate again for all of the values
    /// 
    /// 5. If it is the first child, then add the header of the child section.
    /// 
    /// 6. If the section is not visible, dont paint it.
    /// 
    /// 7. Recursive call.
    /// 
    /// 8. After painting all of the child sections, we should add the total for this section.
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    private string getSection ( EvReportSection section )
    {
      //
      // Initialize a csv string and columnlist array
      //
      String stCsv = String.Empty;
      ArrayList columnList = section.ColumnList;

      //
      // Set csv layout section: flat or tabulated
      //
      if ( section.Layout == EvReportSection.LayoutTypes.Flat )
      {
        stCsv += getFlatSection ( section );
      }
      else
      {
        stCsv = getTabulatedSection ( section );
      }

      //
      // Paints the children of this section if there is any.
      //
      ArrayList children = section.ChildrenList;
      if ( children != null && children.Count != 0 )
      {
        //
        // Iterate once for the headers, and then iterate again for all of the values
        //
        for ( int i = 0; i < children.Count; i++ )
        {
          EvReportSection child = (EvReportSection) children [ i ];

          //
          // If it is the first child, then add the header of the child section.
          //
          if ( i == 0 )
          {
            stCsv += getSectionHeader ( child );
          }

          //
          // If the section is not visible, dont paint it.
          //
          if ( child.Visible == true )
          {
            //
            // Recursive call.
            //
            stCsv += getSection ( child );
          }

        } //End of the children iteration loop

        //
        // After painting all of the child sections, we should add the total for this section.
        //
        stCsv += addTotal ( section );
        stCsv += "\n";
      } //End if of the children count.

      return stCsv;
    } //END getsection method

    //===================================================================================
    /// <summary>
    /// This class paints the section in a flat format.
    /// </summary>
    /// <param name="section">EvReportSection: a section to be painted</param>
    /// <returns>string: a flate section</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize a csv string and a columnlist array.
    /// 
    /// 2. Go through all of the columns of this report.
    /// 
    /// 3. Format the current column with header, separator and format text
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    private string getFlatSection ( EvReportSection section )
    {
      //
      // Initialize a csv string and a columnlist array.
      //
      String stCsv = String.Empty;
      ArrayList columnList = section.ColumnList;

      stCsv += "\n";

      //
      // Go through all of the columns of this report.
      //
      for ( int i = 0; i < columnList.Count; i++ )
      {
        //
        // Format the current column with header, separator and format text
        //
        EvReportColumn currentColumn = (EvReportColumn) columnList [ i ];
        stCsv += getTextCSV ( currentColumn.HeaderText )
          + _separator
          + formatText ( (string) section.ColumnValuesByHeaderText [ currentColumn.HeaderText ], currentColumn.DataType )
          + "\n";
      }

      return stCsv;
    } //END getflatsection method.

    //  ================================================================================
    /// <summary>
    /// This class returns the section in a tabulated format.
    /// </summary>
    /// <param name="section">EvReportSection: a section to be painted</param>    
    /// <returns>String: a tabulated section</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize a csv string and a columnlist array
    /// 
    /// 2. Loop through the columnlist array until data type is not hidden
    /// 
    /// 3. Convert the csv string into a tabulated format
    /// </remarks>
    // ----------------------------------------------------------------------------------
    private String getTabulatedSection ( EvReportSection section )
    {
      //
      // Initialize a csv string and a columnlist array
      //
      String stCsv = String.Empty;
      ArrayList columnList = section.ColumnList;

      //
      // Loop through the columnlist array until data type is not hidden
      //
      for ( int j = 0; j < columnList.Count; j++ )
      {

        EvReportColumn currentColumn = (EvReportColumn) columnList [ j ];

        if ( currentColumn.DataType == EvReport.DataTypes.Hidden )
        {
          continue;
        }

        //
        // Convert the csv string into a tabulated format
        //
        stCsv += formatText ( (string) section.ColumnValuesByHeaderText [ currentColumn.HeaderText ], currentColumn.DataType );

        //
        // If this is the last value, dont add the separator.
        //
        if ( j < columnList.Count - 1 )
        {
          stCsv += _separator;
        }

      }//End of columns loop

      stCsv += "\n";
      return stCsv;
    }//End getTabulatedSection method

    //===================================================================================
    /// <summary>
    /// This class obtains the section header. The hader depends of the type of the section.
    /// Andres Castano 9 nov 2009
    /// </summary>
    /// <param name="section">EvReportSection: a report section</param>
    /// <returns>String: a section header</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize a csv string
    /// 
    /// 2. Creates the header for a tabular section.
    /// 
    /// 3. Iterate over all of the columns until the datatype is not hidden.
    /// 
    /// 4. If the headers are totals, and the grouping type is none, 
    /// then dont show the header.
    /// 
    /// 5. If this is the last value, dont add the separator.
    /// </remarks>
    // ----------------------------------------------------------------------------------
    private String getSectionHeader ( EvReportSection section )
    {
      //
      // Initialize a csv string
      //
      string stCsv = String.Empty;

      //
      // Creates the header for a tabular section.
      //
      if ( section.Layout == EvReportSection.LayoutTypes.Tabular )
      {
        //
        // Iterate over all of the columns until the datatype is not hidden.
        //
        for ( int i = 0; i < section.ColumnList.Count; i++ )
        {

          EvReportColumn currentColumn = (EvReportColumn) section.ColumnList [ i ];

          if ( currentColumn.DataType == EvReport.DataTypes.Hidden )
          {
            continue;
          }

          String headerText = getTextCSV ( currentColumn.HeaderText );

          //
          // If the headers are totals, and the grouping type is none, 
          // then dont show the header.
          //
          if ( section.OnlyShowHeadersForTotalColumns == true )
          {
            if ( currentColumn.GroupingType == EvReport.GroupingTypes.None )
            {
              headerText = String.Empty;
            }
          }

          stCsv += headerText;

          //
          // If this is the last value, dont add the separator.
          //
          if ( i < section.ColumnList.Count - 1 )
          {
            stCsv += _separator;
          }

        }// End of headers loop

        stCsv += "\n";

      }// End of if it is tabular

      return stCsv;
    }//End getSectionHeader method

    //===================================================================================
    /// <summary>
    /// This class adds the total row after a section.
    /// </summary>
    /// <param name="section">EvReportSection: a report section</param>
    /// <returns>String: a total string</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize a csv string and a tempRow string.
    /// 
    /// 2. Validate whether this section has totals.
    /// 
    /// 3. Iterate over all of the details columns.
    /// 
    /// 4. If title does not exist, continue loop and 
    /// place the total label in this column.
    /// 
    /// 5. If the title is already set, then paint the totals.
    /// 
    /// 6. If this is the last value, dont add the separator.
    /// </remarks>
    // ----------------------------------------------------------------------------------
    private String addTotal ( EvReportSection section )
    {
      //
      // Initialize a csv string and a tempRow string.
      //
      string stCsv = String.Empty;
      string tempRow = String.Empty;

      //
      // Validate whether this section has totals.
      // 
      if ( section.DetailColumnsList != null && section.Acumulator != null )
      {
        //
        // The title should be set one column before the first total, and should span from the first column to this.
        //
        bool isTitleSet = false;

        //
        // Iterate over all of the details columns.
        //
        for ( int j = 0; j < section.DetailColumnsList.Count; j++ )
        {
          //
          // If there is no title yet and we are not at the end of the array
          //
          if ( !isTitleSet && ( j + 1 ) < section.DetailColumnsList.Count )
          {

            EvReportColumn nextColumn = (EvReportColumn) section.DetailColumnsList [ j + 1 ];

            //
            // Obtains the acumulated value for the next column.
            //
            String nextValue = (String) section.Acumulator [ nextColumn.HeaderText ];

            //
            // If the accumulated value for the next column is not empty, then place the total label in this column.
            //
            if ( nextValue != null && nextValue != String.Empty )
            {
              if ( nextColumn.GroupingType == EvReport.GroupingTypes.Total )
              {
                tempRow += getTextCSV ( "Total " + section.GroupingColumnValue + ":" );
              }
              else if ( nextColumn.GroupingType == EvReport.GroupingTypes.Count )
              {
                tempRow += getTextCSV ( section.GroupingColumnValue + " total quantity:" );
              }

              isTitleSet = true;
            }
          }

          //
          // If the title is already set, then paint the totals.
          //
          else
          {
            EvReportColumn currentColumn = (EvReportColumn) section.DetailColumnsList [ j ];
            String value = (String) section.Acumulator [ currentColumn.HeaderText ];

            if ( value != null )
            {
              tempRow += formatText ( value, currentColumn.DataType );
            }

          }//end if is title set

          //
          // If this is the last value, dont add the separator.
          //
          if ( j < section.DetailColumnsList.Count - 1 )
          {
            tempRow += _separator;
          }

        }// end of detail column iteration.

        if ( isTitleSet )
        {
          stCsv = tempRow + "\n";
        }


      }

      return stCsv;
    }//End Add total
    #endregion

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogMethod ( String Value )
    {
      this._Log.AppendLine ( Evado.Model.EvStatics.CONST_METHOD_START
      + DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": "
      + this.ClassNameSpace + Value );
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="MethodName">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogMethodEnd ( String MethodName )
    {
      String value = Evado.Model.EvStatics.CONST_METHOD_END;

      value = value.Replace ( " END OF METHOD ", " END OF " + MethodName + " METHOD " );

      this._Log.AppendLine ( value );
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogValue ( String Value )
    {
      this._Log.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " + Value );
    }


    // ==================================================================================
    /// <summary>
    /// This method appendes debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Format">String: format text.</param>
    /// <param name="args">Array of objects as parameters.</param>
    // ----------------------------------------------------------------------------------
    protected void LogFormat ( String Format, params object [ ] args )
    {
      this._Log.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " +
        String.Format ( Format, args ) );
    }

  }//END EvReportCsv method
}//END namespace Evado.Model.Digital
