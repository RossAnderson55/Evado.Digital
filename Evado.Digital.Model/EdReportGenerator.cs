/***************************************************************************************
 * <copyright file="model\EvReport.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2021 EVADO HOLDING PTY. LTD..  All rights reserved.
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
 *  This class contains the EvReportGenerator object.
 *
 ****************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Evado.Digital.Model
{
  /// <summary>
  /// This is an abstract class with commun functionality to all of the reports.
  /// 
  /// Andres Castano 9 November 2009
  /// </summary>
  public abstract class EvReportGenerator
  {
    #region Initialization
    /// <summary>
    /// This is the report which is going to be transformed into a string.
    /// </summary>
    protected EdReport _report;

    /// <summary>
    ///  The State contains debug status of the class.
    /// 
    /// </summary>
    protected StringBuilder _DebugLog = new StringBuilder ( );

    /// <summary>
    /// This property returns the object debuglog content.
    /// </summary>
    public String DebugLog
    {
      get
      {
        return this._DebugLog.ToString ( );
      }
    }
    /// <summary>
    /// Organization of the user generating this report.
    /// </summary>
    protected EdUserProfile _UserProfile = new EdUserProfile ( );
    #endregion

    #region Methods
    //  =================================================================================
    /// <summary>
    /// This is the main section and contains the whole report.
    /// It has some special characteristichs such as the level and the column values.
    /// </summary>
    /// <returns>EvReportSection: Top section of a report</returns>
    // ----------------------------------------------------------------------------------
    private EdReportSection getTopSection ( )
    {
      this.writeDebugLogMethod ( "getTopSection method." );
      EdReportSection wholeReport = new EdReportSection ( );
      wholeReport.SectionLevel = EdReportSection.Report_Level;
      wholeReport.GroupingColumnValue = "Report";
      wholeReport.Layout = EdReportSection.LayoutTypes.Flat;
      return wholeReport;

    }//END getTopSection method

    //===================================================================================
    /// <summary>
    /// This is the method who will return the actual text of the report.
    /// Depending on the implementation, it will return an html report, 
    /// an xml report a csv report and so on.
    /// </summary>    
    // ----------------------------------------------------------------------------------
    public abstract string getReportText ( );

    //===================================================================================
    /// <summary>
    /// This is the method who will return the actual data of the report.
    /// Depending on the implementation, it will return an html report, 
    /// an xml report a csv report and so on.
    /// </summary>    
    // ----------------------------------------------------------------------------------
    public abstract string getReportData ( );

    //===================================================================================
    /// <summary>
    /// This class loads the EvReportSection model from the flat table given in this report object.
    /// The model is a herarchy of EvReportSection Objects.
    /// AFC 6 nov 2009
    /// </summary>
    /// <returns>EvReportSection: a report section model</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize report section properties: top section, section, detail, low section
    /// 
    /// 2. Iterate through the report data records.
    /// 
    /// 3. Iterate through the columns.
    /// 
    /// 4. If column is not a detail column, use detail column functionality
    /// 
    /// 5. If not a detail column, use non-detail column functionality
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public EdReportSection loadModel ( EdReport report )
    {
      this.writeDebugLogMethod ( "loadModel method." );
      //
      // This is the main section and contains the whole report.
      // It has some special characteristichs such as the level and the column values.
      //
      EdReportSection wholeReport = getTopSection ( );

      //
      // Stores group column value for each level in the report.
      // This value is used to determine when a group value has changed, to then close that group and 
      // all child groups including the detailed group.
      //
      String [ ] lastIndexValue = new String [ 5 ];

      //
      // Define a section for each level in the report.
      //
      EdReportSection [ ] sections = new EdReportSection [ 5 ];

      //
      // Define the detailed section separately as it treated separately.
      //
      EdReportSection detail = null;

      //
      // This stores the lower section before the detail. Is used since this section
      // has a specialized format.
      //
      EdReportSection lowerSection = null;

      //
      // True if the section has changed in this row. 
      // This is used for initialize sections when iterating trhough columns.
      //
      bool [ ] sectionHasChangedInThisRow = new bool [ 5 ];

      //
      // Iterate through the report data records.
      //
      for ( int record = 0; record < report.DataRecords.Count; record++ )
      {
        //
        // Get a data report row
        //
        EdReportRow row = report.DataRecords [ record ];

        //
        // Iterate through the columns.
        //
        for ( int column = 0; column < row.ColumnValues.Length; column++ )
        {
          //
          // Retrieve the curren column
          //
          EdReportColumn currentColumn = report.Columns [ column ];

          if ( currentColumn.HeaderText == String.Empty )
          {
            continue;
          }

          //
          // Extract the value of the current column
          //
          String currentValue = row.ColumnValues [ column ];

          this.writeDebugLogLine ( "CurrentValue: " + currentValue );

          int sectionLvl = currentColumn.SectionLvl;

          //
          // If the column is not the detail level, then this must be a group column
          // 
          // So process the column using group column functionality.
          //
          if ( sectionLvl != EdReportSection.Detail_Level )
          {
            //
            // Test whether the group index has changed its value.
            //
            if ( currentColumn.GroupingIndex
              && lastIndexValue [ currentColumn.SectionLvl ] != currentValue )
            {
              //
              // Resets all the lower levels.
              // 
              for ( int level = currentColumn.SectionLvl; level < 5; level++ )
              {
                lastIndexValue [ level ] = null;
              }

              //
              // Open a new level.
              //
              lastIndexValue [ currentColumn.SectionLvl ] = currentValue;
              sections [ sectionLvl ] = new EdReportSection ( );

              sections [ sectionLvl ].RowNumber = record;
              //
              // Stores the parent. If the previous level is the detail, then the parent is the 
              // whole report. If not, the parent is the previous section
              //
              sections [ sectionLvl ].Parent = sectionLvl - 1 == EdReportSection.Detail_Level ? wholeReport : sections [ sectionLvl - 1 ];

              sections [ sectionLvl ].SectionLevel = sectionLvl;
              sections [ sectionLvl ].Layout = EdReportSection.LayoutTypes.Flat;
              sections [ sectionLvl ].GroupingColumnValue = currentValue;
              sectionHasChangedInThisRow [ sectionLvl ] = true;
              if ( sections [ sectionLvl ].Parent == null )
              {
                sections [ sectionLvl ].Parent = new EdReportSection ( );
              }
              sections [ sectionLvl ].Parent.ChildrenList.Add ( sections [ sectionLvl ] );

            }//END Group index has changed.

            if ( sectionHasChangedInThisRow [ sectionLvl ] )
            {
              sections [ sectionLvl ].ColumnValuesByHeaderText.Add ( currentColumn.HeaderText, currentValue );
              sections [ sectionLvl ].ColumnList.Add ( currentColumn );
            }

            //
            // If the lower section has a level less than the current section level, 
            // then the current become the lower section.
            //
            if ( lowerSection == null || lowerSection.SectionLevel < currentColumn.SectionLvl )
            {
              lowerSection = sections [ sectionLvl ];
            }
          }
          else
          {
            //
            // This is the section lvl 0 ("Detail").
            // This assumes that the sections are coninuously enumerated.
            //
            if ( lastIndexValue [ currentColumn.SectionLvl ] != ( "" + record ) )
            {

              if ( lowerSection == null )
              {
                lowerSection = wholeReport;
              }

              lastIndexValue [ currentColumn.SectionLvl ] = "" + record;

              detail = new EdReportSection ( );
              detail.RowNumber = record;
              detail.Layout = EdReportSection.LayoutTypes.Tabular;
              detail.Parent = lowerSection;

              if ( report.IsAggregated == true )
              {
                //
                // Is the report is aggregated, then the detail is not visible.
                //
                detail.Visible = false;

                detail.OnlyShowHeadersForTotalColumns = true;

              }//End if is aggregated.

              //
              // Process: If the site header of the report is not empty, and the value of this site
              // is different to the user organization, then dont show the detail.
              //
              else if ( report.IsUserSiteFiltered == true )
              {
                String detailOrganizationId = detail.searchValueByHeaderText ( report.SiteColumnHeaderText, true );

                //
                // If the user is not site staff, or site trial coordinator, or site principal investigator
                // dont show the details.
                //

              }//End if it is site filtered.
              else
              {
                detail.Visible = true;
                detail.OnlyShowHeadersForTotalColumns = false;
              }

              lowerSection.ChildrenList.Add ( detail );
              lowerSection.DetailColumnsList = detail.ColumnList;

            }

            detail.ColumnValuesByHeaderText.Add (
              currentColumn.HeaderText,
              currentValue );

            detail.ColumnList.Add ( currentColumn );

            if ( currentColumn.GroupingType == EdReport.GroupingTypes.Total )
            {
              detail.addToAcumulator ( currentColumn.HeaderText, currentValue );
            }

            // 
            // If the grouping type is count, then add 1 to the acumulator
            // if the value is not empty.
            //
            if ( currentColumn.GroupingType == EdReport.GroupingTypes.Count )
            {

              if ( currentValue.Trim ( ) != String.Empty )
              {
                detail.addToAcumulator ( currentColumn.HeaderText, "1" );
              }

            }

          }
        }

        for ( int i = 0; i < sectionHasChangedInThisRow.Length; i++ )
        {
          sectionHasChangedInThisRow [ i ] = false;
        }
        lowerSection = null;
      }

      return wholeReport;

    }//End loadModel method.


    //===================================================================================
    /// <summary>
    /// This class transforms a boolean string value into an object. To be true, the boolean should be
    /// true, yes or 1
    /// </summary>
    /// <param name="booleanString">string: a boolean string</param>
    /// <returns>Boolean: true, if boolean string is not null</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Format the boolean string
    /// 
    /// 2. Return true, if boolean string is not empty.
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public bool parseBoolean ( string booleanString )
    {
      this.writeDebugLogMethod ( "parseBoolean method." );
      booleanString = booleanString.Trim ( );
      booleanString = booleanString.ToLower ( );

      return booleanString != null
        && !booleanString.Equals ( String.Empty )
        && ( booleanString.Equals ( "true" )
        || booleanString.Equals ( "1" )
        || booleanString.Equals ( "yes" ) );

    }//End parseBoolean method

    #endregion

    #region Debug methods.

    private const string CONST_NAME_SPACE = "Evado.Digital.Model.EvReportGenerator.";
    // ==================================================================================
    /// <summary>
    /// This method resets the debug log to empty
    /// </summary>
    // ----------------------------------------------------------------------------------
    public void resetDebugLog ( )
    {
      this._DebugLog = new StringBuilder ( );
    }//END resetDebugLog class

    // ==================================================================================
    /// <summary>
    /// This method appends the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="DebugLogString">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    public void writeDebugLogLine ( String DebugLogString )
    {
      this._DebugLog.AppendLine (
        DateTime.Now.ToString ( "dd-MMM-yy hh:mm:ss" ) + " : " + DebugLogString );
    }//END writeDebugLogLine class

    //  ==================================================================================
    /// <summary>
    /// This class writes Debug line to method status.
    /// </summary>
    //  ----------------------------------------------------------------------------------
    public void writeDebugLogMethod ( String Value )
    {
      this._DebugLog.AppendLine ( Evado.Digital.Model.EvcStatics.CONST_METHOD_START
        + DateTime.Now.ToString ( "dd-MMM-yy hh:mm:ss" ) + " : "
        + CONST_NAME_SPACE + Value );
    }//END writeDebugLine class

    #endregion
  }//END EvReportGenerator method
}//END namespace Evado.Digital.Model
