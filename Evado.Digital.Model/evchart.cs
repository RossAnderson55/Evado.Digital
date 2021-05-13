/***************************************************************************************
 * <copyright file="EvChart.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvChart data object.
 *
 ****************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

namespace Evado.Digital.Model
{
  /// <summary>
  /// data  entity used to model accounts
  /// </summary>
  [Serializable]
  public class EvChart
  {
    #region Enumerations
    /// <summary>
    /// This enumeration list defines the state of grouping options
    /// </summary>
    public enum GroupingOptions
    {
      /// <summary>
      /// This enumeration defines trial visit state for grouping option.
      /// </summary>
      Visit = 0,

      /// <summary>
      /// This enumeration defines date state for grouping option.
      /// </summary>
      Date = 1
    }//END enum GroupingOptions

    /// <summary>
    /// This enumeration list defines the safey state of reporting options
    /// </summary>
    public enum SafetyReportingOptions
    {
      /// <summary>
      /// This enumeration defines null value or no selection state.
      /// </summary>
      Null = 0,

      /// <summary>
      /// This enumeration selects all items to be reported
      /// </summary>
      AllItems = 1,

      /// <summary>
      /// This enumeration defines selected items to be reported
      /// </summary>
      SelectedItems = 2,

      /// <summary>
      /// This enumeration selects exceptions to be reported.
      /// </summary>
      Exceptions = 3
    }//END enum SafetyReportingOptions

    /*
    /// <summary>
    /// This enumeration list defines the state of data source options
    /// </summary>
    public enum SourceOptions
    {
      /// <summary>
      /// This enumeration defines the record source for option
      /// </summary>
      Record = 0,

      /// <summary>
      /// This enumeration defines the test source for option.
      /// </summary>
      Test = 1,
    }//END enum SourceOptions
    */

    /// <summary>
    /// This enumeration list defines the state of aggregation options.
    /// </summary>
    public enum AggregationOptions
    {
      /// <summary>
      /// This enumeration defines an instance state of aggregation options
      /// </summary>
      Instance = 0,

      /// <summary>
      /// This enumeration defines an average state of aggregation options
      /// </summary>
      Average = 1,

      /// <summary>
      /// This enumeration defines a maximum state of aggregation options
      /// </summary>
      Maximum = 2,

      /// <summary>
      /// This enumeration defines a minimum state of aggregation options
      /// </summary>
      Minimum = 3
    }//END enum AggregationOptions

    #endregion

    #region Internal member variables

    private string _ProjectId = String.Empty;
    private string _ProjectTitle = String.Empty;
    private string _UserCommonName = String.Empty;
    private int _ScheduleId = 1;

    private SafetyReportingOptions _SafetyReporting = SafetyReportingOptions.Null;
    private GroupingOptions _Grouping = GroupingOptions.Visit;
    private List<EvChartQueryItem> _QueryItems = new List<EvChartQueryItem> ( );

    private List<EvChartSeries> _Series = new List<EvChartSeries> ( );
    private List<String> _Categories = new List<string> ( );
    private string _Title = String.Empty;
    private string _CategoryLegend = String.Empty;
    private string _ValueLegend = String.Empty;
    /// <summary>
    /// This consant defines the data csource field delimiter and processing CSv files.
    /// </summary>
    public const String CONST_SOURCE_DELIMITER = "^";

    #endregion

    #region Properties

    /// <summary>
    /// This property contains a trial identifier of a chart.
    /// </summary>
    public string ProjectId
    {
      get
      {
        return this._ProjectId;
      }
      set
      {
        this._ProjectId = value;
      }
    }

    /// <summary>
    /// This property contains a trial title of a chart.
    /// </summary>
    public string TrialTitle
    {
      get
      {
        return this._ProjectTitle;
      }
      set
      {
        this._ProjectTitle = value;
      }
    }

    /// <summary>
    /// This property contains an Schedule of a chart.
    /// </summary>
    public int ScheduleId
    {
      get
      {
        return this._ScheduleId;
      }
      set
      {
        this._ScheduleId = value;
      }
    }

    /// <summary>
    /// This property contains a user common name of a chart.
    /// </summary>
    public string UserCommonName
    {
      get
      {
        return this._UserCommonName;
      }
      set
      {
        this._UserCommonName = value;
      }
    }

    /// <summary>
    /// This property contains a safety reporting option of a chart.
    /// </summary>
    public SafetyReportingOptions SafetyReporting
    {
      get
      {
        return this._SafetyReporting;
      }
      set
      {
        this._SafetyReporting = value;
      }
    }

    /// <summary>
    /// This property contains a grouping option of a chart.
    /// </summary>
    public GroupingOptions Grouping
    {
      get
      {
        return this._Grouping;
      }
      set
      {
        this._Grouping = value;
      }
    }

    /// <summary>
    /// This property contains a query item list of a chart.
    /// </summary>
    public List<EvChartQueryItem> QueryItems
    {
      get
      {
        return this._QueryItems;
      }
      set
      {
        this._QueryItems = value;
      }
    }

    /// <summary>
    /// This property contains a title of a chart.
    /// </summary>
    public string Title
    {
      get
      {
        return this._Title;
      }
      set
      {
        this._Title = value;
      }
    }

    /// <summary>
    /// This property contains a category legend of a chart.
    /// </summary>
    public string CategoryLegend
    {
      get
      {
        return this._CategoryLegend;
      }
      set
      {
        this._CategoryLegend = value;
      }
    }

    /// <summary>
    /// This property contains a value legend of a chart.
    /// </summary>
    public string ValueLegend
    {
      get
      {
        return this._ValueLegend;
      }
      set
      {
        this._ValueLegend = value;
      }
    }

    /// <summary>
    /// This property contains a serie list of a chart.
    /// </summary>
    public List<EvChartSeries> Series
    {
      get
      {
        return this._Series;
      }
      set
      {
        this._Series = value;
      }
    }

    /// <summary>
    /// This property contains category list of a chart.
    /// </summary>
    public List<String> Categories
    {
      get
      {
        return this._Categories;
      }
      set
      {
        this._Categories = value;
      }
    }

    // End properties.

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class CsvOutput Method
    /// <summary>
    /// This method formats the chart data for output a CSV file
    /// </summary>
    /// <returns>string: a csv output</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. initialize the local variable and objects
    /// including sCsvReport, sLegend and number of categories.
    /// 
    /// 2. Output the query header
    /// 
    /// 3. Output the EvChart header
    /// 
    /// 4. Output the EvChart body
    /// </remarks>
    public string getCsvOutput ( )
    {
      // 
      // Initialise the local method variables and objects.
      // 
      System.Text.StringBuilder sbCsvReport = new System.Text.StringBuilder ( );
      System.Text.StringBuilder sbLegend = new System.Text.StringBuilder ( );
      int NumberOfCategories = 0;
      // 
      // Output the Query Header
      // 
      if ( this._SafetyReporting > SafetyReportingOptions.Null )
      {
        sbCsvReport.AppendLine ( "Safety Report Query" );
      }
      else
      {
        sbCsvReport.AppendLine ( "Data Analysis Query" );
      }

      sbCsvReport.AppendLine ( "Run by: " + this.UserCommonName
       + " on " + DateTime.Now.ToString ( "dd MMM yyyy" ) );
      sbCsvReport.AppendLine ( "Project: " + this._ProjectId
       + " - " + this._ProjectTitle );

      if ( this._SafetyReporting == SafetyReportingOptions.SelectedItems
        || this._SafetyReporting == SafetyReportingOptions.Null )
      {
        sbCsvReport.AppendLine ( "Series" );
        sbCsvReport.AppendLine ( "Source,Item, Aggregation" );
      }

      //
      // Loop through the query item and add items to the csvReport if they exist.
      //
      for ( int queryCount = 0; queryCount < this._QueryItems.Count; queryCount++ )
      {
        if ( this._QueryItems [ queryCount ].ItemId != String.Empty )
        {
          sbCsvReport.AppendLine ( this._QueryItems [ queryCount ].ItemId.Replace ( "^", " >> " )
           + "," + this._QueryItems [ queryCount ].Aggregation );
        }
      }
      // 
      // Output EvChart header
      // 
      sbCsvReport.AppendLine ( "Title:," + _Title );
      sbCsvReport.AppendLine ( "Category Legend:," + _CategoryLegend );

      sbLegend.Append ( "ItemId,Legend,Unit" );
      //
      // Loop through the category array.
      //
      for ( int iCategory = 0; iCategory < _Categories.Count; iCategory++ )
      {
        //
        // Add category items to Legend if they are not null or not have empty string.
        //
        if ( _Categories [ iCategory ] != null )
        {
          if ( _Categories [ iCategory ] != String.Empty )
          {
            sbLegend.Append ( "," + _Categories [ iCategory ] );

            NumberOfCategories = iCategory + 1;
          }
        }
      }

      sbCsvReport.AppendLine ( "Chart Columns: " + NumberOfCategories );

      // 
      // Output Body
      // 
      sbCsvReport.AppendLine ( sbLegend.ToString ( ) );

      for ( int iSeries = 0; iSeries < Series.Count; iSeries++ )
      {
        EvChartSeries _ChartSeries = (EvChartSeries) _Series [ iSeries ];
        // 
        // If the series exists then output it.
        // 
        if ( _ChartSeries.ItemId != String.Empty )
        {
          sbCsvReport.Append ( "\"" + _ChartSeries.ItemId + "\""
            + ",\"" + _ChartSeries.Legend + "\""
            + ",\"" + _ChartSeries.Unit + "\"" );
          // 
          // Output chart values
          // 
          for ( int iCategory = 0; iCategory < NumberOfCategories; iCategory++ )
          {
            sbCsvReport.Append ( "," + _ChartSeries.Values [ iCategory ].ToString( "####0.00")  );
          }

          sbCsvReport.Append ( "\r\n" );
        }
      }
      // 
      // Output Footer
      // 
      return sbCsvReport.ToString ( );

    }//END getCsvOutput method
    /*
    /// <summary>
    /// This class selects the source of chart.
    /// </summary>
    /// <param name="source"></param>
    /// <returns>string: a source of chart</returns>
    /// <remarks>
    /// This method consists of the following step:
    /// 
    /// 1. Switch source and return the source name for the property defined by source options.
    /// </remarks>
    private string GetSource ( EvChart.SourceOptions source )
    {
      switch ( source )
      {
        case SourceOptions.Test:
          return "Subject Test";
        default:
          return "Trial Record";

      }
    }
    */
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

  } // Close class TestDataLegendIndex

} // Close Namespace Evado.Model
