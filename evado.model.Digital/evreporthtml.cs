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
 *  This class contains the EvReportHtml Data object..
 *
 ****************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;


namespace Evado.Model.Digital
{

  /// <summary>
  /// This class creates a Html Report using a EvReportSection object.
  /// 
  /// AFC 9 November 2009.
  /// 
  /// </summary>
  class EvReportHtml : EvReportGenerator
  {
    #region Initialization
    public EvReportHtml (
      EvReport report,
      EvUserProfile userProfile )
    {
      this.writeDebugLogMethod ( "EvReportHtml initialisation method." );
      this._report = report;
      this._UserProfile = userProfile;
    }
    #endregion

    #region Methods

    public override string getReportData ( )
    {
      return string.Empty;
    }

    /// <summary>
    /// This class obtains the section header. The hader depends of the type of the section.
    /// AFC 6 nov 2009
    /// </summary>
    /// <param name="section">EvReportSection: section</param>
    /// <returns>String: a section tabular header</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Loop through the columnlist of the section and validate a hidden column
    /// 
    /// 2. Generate header cells
    /// </remarks>
    private String getSectionHeader ( EvReportSection section )
    {
      this.writeDebugLogMethod ( "getSectionHeader method." );
      string stHtml = String.Empty;

      if ( section.Layout == EvReportSection.LayoutTypes.Tabular )
      {
        stHtml = "<tr>";

        //
        // Loop through the columnlist of the section and validate a hidden column
        //
        for ( int i = 0; i < section.ColumnList.Count; i++ )
        {

          EvReportColumn currentColumn = (EvReportColumn) section.ColumnList [ i ];

          //
          // If the column is hiddent don't process it at all.
          //
          if ( currentColumn.DataType == EvReport.DataTypes.Hidden )
          {
            continue;
          }

          //
          // Generate the header cells
          //
          String headerCell = "<td class='Rpt_Header' ";

          if ( currentColumn.StyleWidth != String.Empty )
          {
            headerCell += "style='width:" + currentColumn.StyleWidth + "'";
          }

          headerCell += ">" + currentColumn.HeaderText + "</td>\r\n";

          //
          // If the headers are only for totals, and the grouping type is none, then dont show the header.
          //
          if ( section.OnlyShowHeadersForTotalColumns == true )
          {
            if ( currentColumn.GroupingType == EvReport.GroupingTypes.None )
            {
              headerCell = "<td>&nbsp;</td>";
            }
          }

          stHtml += headerCell;
        }

        stHtml += "</tr>\n\r";

      }

      return stHtml;
    }//END getSectionHeader method.

    //  =================================================================================
    /// <summary>
    /// This class obtains the formatted section based on the model.
    /// </summary>
    /// <param name="section">EvReportSection: a section to be painted</param>
    /// <param name="recordNumber">Integer: It is utilized to paint different backgrounds colors.</param>
    /// <returns>string: The formated section as a string.</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Validate whether the section layout is flat or tabulated
    /// 
    /// 2. Paints the children of this section if there is any.
    /// 
    /// 3. Iterate once for the headers, and then iterate again for all of the values
    /// 
    /// 4. After painting all of the child sections, add the section total.
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    private string getSection ( EvReportSection section, int recordNumber )
    {
      this.writeDebugLogMethod ( "getSection method." );
      String stHtml = String.Empty;
      ArrayList columnList = section.ColumnList;

      //
      // Validate whether the section layout is flat or tabulated
      //
      if ( section.Layout == EvReportSection.LayoutTypes.Flat )
      {
        stHtml += getFlatSection ( section );
      }
      else
      {
        stHtml += getTabulatedSection ( section, recordNumber );
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
          // If it is the first child, and it must show the header, then ad the header of the child section.
          //
          if ( i == 0 )
          {
            stHtml += getSectionHeader ( child );
          }

          //
          // If the section is not visible, dont paint it.
          //
          if ( child.Visible == true )
          {
            //
            // Recursive call.
            //
            stHtml += getSection ( child, i );
          }

        } //End of the children iteration loop

        //
        // After painting all of the child sections, we should add the total for this section.
        //
        stHtml += addTotal ( section );

      } //End if of the children count.

      return stHtml;
    }//END getSection method.

    //===================================================================================
    /// <summary>
    /// This class adds the total row after a section.
    /// </summary>
    /// <param name="section">EvReportSection: a report section</param>
    /// <returns>String: a total string</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Validate the secion is not null and empty
    /// 
    /// 2. Paint a new table row for total value
    /// 
    /// 3. Loop through the detail columns of the row.
    /// 
    /// 4. If title does not exists, paint title and add total value
    /// 
    /// 5. If title exists, add total value.
    /// </remarks>
    // ----------------------------------------------------------------------------------
    private String addTotal ( EvReportSection section )
    {
      this.writeDebugLogMethod ( "addTotal method." );
      int span = getMaxColSpan ( section );
      string stHtml = String.Empty;

      //
      // If this section is not null.
      //
      if ( section.DetailColumnsList == null
        && section.Acumulator == null )
      {
        return String.Empty;
      }

      //
      // If this section is not empty.
      //
      if ( section.DetailColumnsList.Count > 0
        && section.Acumulator.Count > 0 )
      {
        //
        // Paint total new table row
        // 
        stHtml += "<tr class='Rpt_Alt'>\n\r";
        //stHtml += "<tr class='" + getClassNameForDetail ( recordNumber + 1 ) + "'>\n\r";

        //
        // The title should be set one column before the first total, 
        // and should span from the first column to this.
        //
        bool isTitleSet = false;

        //
        // Iterate over all of the details columns.
        //
        for ( int j = 0; j < section.DetailColumnsList.Count; j++ )
        {

          String cell = null;

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
              int spanValue = j + 1;

              if ( nextColumn.GroupingType == EvReport.GroupingTypes.Total )
              {
                cell = "<td colspan='" + spanValue + "' class='Rpt_Total' style='background-color: white' ><strong>Total "
                + section.GroupingColumnValue + ":</strong></td>\r\n";
              }
              else if ( nextColumn.GroupingType == EvReport.GroupingTypes.Count )
              {
                cell = "<td colspan='" + spanValue + "' class='Rpt_Total' style='background-color: white' ><strong>" +
                    section.GroupingColumnValue + " total quantity:</strong></td>\r\n";
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
              cell = "<td class='Rpt_Total'><strong>"
                + EvcStatics.formatDoubleString ( value, currentColumn.ValueFormatingString )
                + "</strong></td>\r\n";
            }

          }


          if ( cell != null && cell != String.Empty )
          {
            stHtml += cell;
          }
          else if ( isTitleSet )
          {
            stHtml += "<td class='Rpt_Item'></td>\r\n";
          }

        }

        stHtml += "</tr>\n\r<tr><td class='Rpt_Item' colspan='" + span + "' style='width:100%'>&nbsp</td></tr>";

      }

      return stHtml;
    }//End addTotal method

    //  =================================================================================
    /// <summary>
    /// This class paints the section in a tabulated format.
    /// </summary>
    /// <param name="section">EvReportSection: a section to be painted</param>
    /// <param name="recordNumber">Integer: a number of the record</param>
    /// <returns>String: a tabulated section of the report</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize a html string and a columnlist array
    /// 
    /// 2. Add a row to a html string.
    /// 
    /// 3. Iterate through the columnlist of the row
    /// 
    /// 4. Validate whether the column is not hiddent
    /// 
    /// 5. Add a column to a html string.
    /// 
    /// 6. If numeric, move to the right but if not, move to center
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    private String getTabulatedSection ( EvReportSection section, int recordNumber )
    {
      this.writeDebugLogMethod ( "getTabulatedSection method." );
      //
      // Initialize a html string and a columnlist array
      //
      String stHtml = String.Empty;
      ArrayList columnList = section.ColumnList;

      //
      // Add a row to a html string.
      //
      stHtml += "<tr class='" + getClassNameForDetail ( recordNumber ) + "'>\n\r";

      //
      // Iterate through the columnlist of the row
      //
      for ( int j = 0; j < columnList.Count; j++ )
      {
        EvReportColumn currentColumn = (EvReportColumn) columnList [ j ];

        //
        // If the column is hidden, do nothing.
        //
        if ( currentColumn.DataType == EvReport.DataTypes.Hidden )
        {
          continue;
        }

        //
        // Add a column to Html string
        //
        stHtml += "<td class='" + getClassNameForDetail ( recordNumber ) + "' ";

        //
        // If the coulmn is numeric, align to the right.
        //
        if ( currentColumn.DataType == EvReport.DataTypes.Integer
          || currentColumn.DataType == EvReport.DataTypes.Float
          || currentColumn.DataType == EvReport.DataTypes.Currency )
        {
          stHtml += "style='text-align:right' ";
        }
        else if ( currentColumn.DataType == EvReport.DataTypes.Bool
          || currentColumn.DataType == EvReport.DataTypes.Date
          || currentColumn.DataType == EvReport.DataTypes.Percent )
        {
          stHtml += "style='text-align:center' ";
        }

        //
        // Close the column
        //
        stHtml += ">"
        + this.getFormattedValue ( section, currentColumn )
        + "</td>\r\n";
      }

      //
      // Close the row
      //
      stHtml += "</tr>\n\r";
      return stHtml;

    }//End getTabulatedSection class

    //  =================================================================================
    /// <summary>
    /// This class obtains the formatted value of the column. It can be a currency, or a check box etc.
    /// </summary>
    /// <param name="section">EvReportSection: a report section</param>
    /// <param name="column">EvReportColumn: a section column</param>
    /// <returns>string: a formatted value string</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize a html string, column value string and column pattern string.
    /// 
    /// 2. If a pattern string is not empty, return a formatted value column 
    /// 
    /// 3. Switch column datatype and update the html string with value defining by datatypes
    /// </remarks>
    // ----------------------------------------------------------------------------------
    private string getFormattedValue ( EvReportSection section, EvReportColumn column )
    {
      this.writeDebugLogMethod ( "getFormattedValue method." );
      //
      // Initialize a html string, column value string and column pattern string.
      //
      String stHtml = String.Empty;
      string currentValue = (string) section.ColumnValuesByHeaderText [ column.HeaderText ];
      String pattern = column.ValueFormatingString == null ? String.Empty : column.ValueFormatingString.Trim ( );

      //
      // If a pattern string is not empty, return a formatted value column 
      //
      if ( pattern != String.Empty )
      {
        String retVal = formatUsingMask ( currentValue, column );

        if ( retVal.Trim ( ) == String.Empty )
        {
          return "&nbsp;";
        }

        return retVal;
      }

      //
      // Switch column datatype and update the html string with value defining by datatypes
      //
      switch ( column.DataType )
      {
        case EvReport.DataTypes.Bool:
          stHtml += "<input type='checkbox' id='" + column.HeaderText + "-" + section.RowNumber + "' ";
          stHtml += parseBoolean ( currentValue ) ? "CHECKED " : "";
          stHtml += _report.SelectionOn ? "" : "DISABLED ";
          stHtml += "/>";
          return stHtml;
        case EvReport.DataTypes.Text:
          stHtml = currentValue;
          stHtml = stHtml.Replace ( "[[br]]", "[[BR]]" );
          stHtml = stHtml.Replace ( "[[BR]]", "<br/>" );
          stHtml = stHtml.Replace ( "\b", "<br/>" );
          return stHtml;
        case EvReport.DataTypes.Currency:
          return EvcStatics.formatDoubleString ( currentValue, "$###,##0" );
        case EvReport.DataTypes.Date:
          return EvcStatics.formatDateString ( currentValue, "dd/MM/yyyy" );
        case EvReport.DataTypes.Percent:
          try
          {
            double dVal = double.Parse ( currentValue );
            dVal = dVal / 100;
            return dVal.ToString ( "p1" );
          }
          catch ( Exception )
          {
            return currentValue;
          }
        default:
          return currentValue;

      }//END switch


    }//End getFormattedValue class

    //  =================================================================================
    /// <summary>
    /// This method paints the section in a flat format.
    /// </summary>
    /// <param name="section">EvReportSection: a section to be painted</param>
    /// <returns>string: a flat section of the report</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize a html string, a columnlist array and a span number
    /// 
    /// 2. Go through all of the columns of this report.
    /// 
    /// 3. If it is the first row, then open a new table row
    /// 
    /// 4. Append a span class to the html string
    /// 
    /// 5. If it is not the last row, then break line, else close table row.
    /// </remarks>
    // ----------------------------------------------------------------------------------
    private string getFlatSection ( EvReportSection section )
    {
      this.writeDebugLogMethod ( "getFlatSection method." );
      //
      // Initialize a html string, a columnlist array and a span number
      //
      String stHtml = String.Empty;
      ArrayList columnList = section.ColumnList;
      int span = getMaxColSpan ( section );

      //
      // Go through all of the columns of this report.
      //
      for ( int i = 0; i < columnList.Count; i++ )
      {
        //
        // If it is the first row, then open a new table row
        //
        if ( i == 0 )
        {
          stHtml += "<tr>\n\r"
          + "<td colspan='" + span + "' class='" + getClassNameForFlatRow ( section.SectionLevel ) + "'>";
        }

        EvReportColumn currentColumn = (EvReportColumn) columnList [ i ];

        //
        // Append a span class to the html string
        //
        stHtml += "<span class='Flat_Rpt_Field_Name'>" + currentColumn.HeaderText + ": </span><span class='Flat_Rpt_Field_Value'> "
        + formatUsingMask ( (String) section.ColumnValuesByHeaderText [ currentColumn.HeaderText ], currentColumn )
        + "</span>";

        //
        // If it is not the last row, then break line, else close table row.
        //
        if ( i != columnList.Count - 1 )
        {
          stHtml += "<br>";
        }
        else
        {
          stHtml += "</td></tr>\n\r";
        }

      }

      return stHtml;
    }//End getFlatSection class

    //  =================================================================================
    /// <summary>
    /// This class obtains the maximun coloum span for this section.
    /// Usually is the amount of colums of the detail of the report.
    /// </summary>
    /// <param name="section">EvReportSection: a report section</param>
    /// <returns>integer: a maximum column span number</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Validate whether the column details is not null
    /// 
    /// 2. Iterate over all of the detail columns, 
    /// 
    /// 3. Only add the not hidden columns to the span value.
    /// </remarks>
    // ----------------------------------------------------------------------------------
    private static int getMaxColSpan ( EvReportSection section )
    {
      int span = 0;
      //
      // Since the report is on big table, the upper sections should 
      // span to the number of columns in the detail.
      //
      if ( section.DetailColumnsList != null )
      {
        //
        // Iterate over all of the detail columns, 
        // but only add the not hidden columns to the span value.
        //
        for ( int i = 0; i < section.DetailColumnsList.Count; i++ )
        {
          EvReportColumn column = (EvReportColumn) section.DetailColumnsList [ i ];

          if ( column.DataType != EvReport.DataTypes.Hidden )
          {
            span++;
          }

        }

      }
      return span;
    }//End getMaxColSpan class

    //  =================================================================================
    /// <summary>
    /// This class formats the string value using the pattern
    /// </summary>
    /// <param name="value">String: value to be formatted in its string representation</param>    
    /// <param name="column">EvReportColumn: a column object</param>
    /// <returns>string: a string format using mask</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize a pattern string
    /// 
    /// 2. Validate whether the pattern is not null and empty
    /// 
    /// 3. Switch the datatype of column and update the numberic value defining by the datatypes. 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    private string formatUsingMask ( String value, EvReportColumn column )
    {
      //
      // Initialize a pattern string
      //
      String pattern = column.ValueFormatingString;

      //
      // Validate whether the pattern is not null and empty
      //
      if ( pattern == null || pattern.Equals ( String.Empty ) )
      {
        return value;
      }

      //
      // Switch the datatype of column and update the numberic value defining by the datatypes. 
      //
      switch ( column.DataType )
      {
        case EvReport.DataTypes.Currency:
        case EvReport.DataTypes.Float:
        case EvReport.DataTypes.Integer:
          {
            return EvcStatics.formatDoubleString ( value, pattern );
          }
        case EvReport.DataTypes.Date:
          {
            return EvcStatics.formatDateString ( value, pattern );
          }
        default:
          {
            return value;
          }
      }//END switch
    }//END formatUsingMask class

    // ==================================================================================
    /// <summary>
    /// This method generates the report grouped report output.
    /// 
    /// The grouping counts the data down through each level clustering the values at each layer.
    /// 
    /// Modified by: AFC 30 - Oct - 2009
    /// </summary>
    /// <param name="model">EvReportSection: a section model</param>
    /// <returns>string: a grouped report content</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialise a html string and model status
    /// 
    /// 2. Open the report content table and add section model to the html string.
    /// 
    /// 3. Return the report content formatted as Html.
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private string getGroupedReportContent ( EvReportSection model )
    {
      // 
      // Initialise a html string and model status
      // 
      string stHtml = String.Empty;

      this.writeDebugLogMethod ( "getGroupedReportContent method" );

      // 
      // Open the report content table and add section model to the html string.
      // 
      //stHtml += "<table style='width:100%' border='1'>\n\r";
      stHtml += "<table id='Rpt_ContentTable' cellspacing='0' rules='all' border='1' >\n\r";
      stHtml += getSection ( (EvReportSection) model, 0 );
      stHtml += "</table>\n\r";

      // 
      // Return the report content formatted as Html.
      // 
      return stHtml;

    }//END getGroupedReportContent class

    // ==================================================================================
    /// <summary>
    /// This class returns the proper style class name of a detail row depending on the row number.
    /// </summary>
    /// <param name="rowNo">integer: a row number</param>
    /// <returns>string: a class name detail</returns>
    /// <remarks>
    /// This class consists of the following steps:
    /// 
    /// 1. If row number is event, return report item class
    /// 
    /// 2. If row number is odd, return report all item class
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private string getClassNameForDetail ( int rowNo )
    {
      if ( rowNo % 2 == 0 )
      {
        return "Rpt_Item";
      }
      else
      {
        return "Rpt_Alt";
      }

    }//END getClassNameForDetail class

    /// <summary>
    /// This class returns the proper style class of a flat report row depending on the row number.
    /// </summary>
    /// <param name="rowNo">integer: a row number</param>
    /// <returns>string: a class name for flat row</returns>
    /// <remarks>
    /// This class consists of the following steps:
    /// 
    /// 1. If row number is event, return a flat report item class
    /// 
    /// 2. If row number is odd, return a flat report all item class
    /// </remarks>
    private string getClassNameForFlatRow ( int rowNo )
    {
      if ( rowNo % 2 == 0 )
      {
        return "Flat_Rpt_Item_Alt";
      }
      else
      {
        return "Flat_Rpt_Item";
      }

    }//END getClassNameForFlatRow class

    // ==================================================================================
    /// <summary>
    /// This class obtains the text of the report!
    /// </summary>
    /// <returns>string: a report text</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Load the herarchy model of Section of Objects.
    /// 
    /// 2. If there is no data the do not build the report
    /// 
    /// 3. Initialise the methods variables and objects.
    /// 
    /// 4. Paints the report header. This includes the titles and the queries.
    /// 
    /// 5. Paints the grouped report. To pain a flat report, only the detail 
    /// should be specified.
    /// 
    /// 6. Return the report content formatted as Html.
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public override string getReportText ( )
    {
      this.writeDebugLogMethod ( "Executing getReportText method." );

      //
      // Load the herarchy model of Section of Objects.
      //
      EvReportSection model = this.loadModel ( this._report );


      // 
      // If there is no data the do not build the report
      // 
      if ( this._report == null )
      {
        this.writeDebugLogLine ( "No Data." );

        return "<p>No Data</p>";
      }

      // 
      // Initialise the methods variables and objects.
      // 
      string stHtml = String.Empty;

      //
      // Paints the report header. This includes the titles and the queries.
      //
      stHtml += this.getReportHeader ( );

      //
      // Paints the grouped report. To pain a flat report, 
      // only the detail should be specified.
      //
      stHtml += this.getGroupedReportContent ( model );

      // 
      // Return the report content formatted as Html.
      // 
      return stHtml;
    } //END getReportText class

    // ==================================================================================
    /// <summary>
    ///  This method generates the report header layout for the report
    /// </summary>
    /// <returns>string: a report header</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialise the html string and report status
    /// 
    /// 2. Append the html string with: table row, report title, report number, date, 
    /// generated user
    /// 
    /// 3. If sub title exists, add sub title to the html string
    /// 
    /// 4. Validate whether the report query is not null
    /// 
    /// 5. Iterate through the query array generating each item in order of the array items.
    /// 
    /// 6. If the item has a value the display it.
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private string getReportHeader ( )
    {
      // 
      // Initialise the html string and report status
      // 
      string stHtml = String.Empty;
      this.writeDebugLogMethod ( "getReportHeader method" );

      //
      // Append table row and report title to the html string.
      //
      stHtml += "<table class='Rpt_Title'>"
        + " <tr>\r\n"
        + "<td style='text-align: Left;' >"
        + "<p class='Rpt_Title' >"
        + this._report.ReportTitle
        + "</p></td>\r\n"
        + "<td style='width: 30%; text-align: Right;' >\n\r";

      //
      // If report number is not null, add report number to html string
      //
      if ( this._report.ReportNo != 0 )
      {
        stHtml += "Report No. " + this._report.stReportNo + "<br/>\r\n";
      }

      //
      // Add date and user who generates report to the html string
      //
      stHtml += "Date: "
        + this._report.stReportDate;

      if ( this._report.GeneratedBy != null && this._report.GeneratedBy != String.Empty )
      {
        stHtml += "</br>\r\nGenerated by: "
          + this._report.GeneratedBy;
      }

      //
      // Close the row
      //
      stHtml += "</td></tr>\n\r";

      //
      // If report subtitle is not null or empty, add report subtitle to the html string. 
      //
      if ( this._report.ReportSubTitle != null
        && this._report.ReportSubTitle != String.Empty )
      {
        stHtml += "<tr><td style='text-align: Left;' >"
          + "<p class='Rpt_SubTitle'>"
          + this._report.ReportSubTitle
          + "</p></td></tr>\r\n";
      }

      stHtml += "</table>";

      stHtml += "<table class='Rpt_Parameters'>\r\n";

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

          if ( this._report.Queries [ i ].DataType == EvReport.DataTypes.Guid
            || this._report.Queries [ i ].DataType == EvReport.DataTypes.Hidden )
          {
            continue;
          }

          // 
          // If the item has a value the display it.
          // 
          if ( this._report.Queries [ i ].Prompt != String.Empty )
          {
            stHtml += "<tr>"
            + "<td class='Prompt'> "
            + this._report.Queries [ i ].Prompt
            + ":</td>\r\n"
            + "<td> "
            + this._report.Queries [ i ].Value
            + "</td>";
            stHtml += "</tr>\r\n";
          }

        }//END query interation loop

      }//END Query array exists.

      stHtml += "</table>";

      // 
      // Return the report content formatted as Html.
      // 
      return stHtml;

    }//END getReportHeader method
    #endregion
  }//END EvReportHtml
}//END namespace Evado.Model.Digital
