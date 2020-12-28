/***************************************************************************************
 * <copyright file="bll\EvDataAnalysis.cs" company="EVADO HOLDING PTY. LTD.">
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
 *
 ****************************************************************************************/

using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;

//Evado. namespace references.
using Evado.Model;
using Evado.Dal;
using Evado.Model.Digital;


namespace Evado.Bll.Clinical
{
  /// <summary>
  /// A business to manage aObjects. This class uses aObject ResultData object for its content.
  /// </summary>
  public class EvDataAnalysis : EvBllBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EvDataAnalysis ( )
    {
      this.ClassNameSpace = "Evado.Bll.Clinical.EvDataAnalysis.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EvDataAnalysis ( EvClassParameters Settings )
    {
      this.ClassParameter = Settings;
      this.ClassNameSpace = "Evado.Bll.Clinical.EvDataAnalysis.";

      if ( this.ClassParameter.LoggingLevel == 0 )
      {
        this.ClassParameter.LoggingLevel = Evado.Dal.EvStaticSetting.LoggingLevel;
      }

      this._dalRecordItems = new Evado.Dal.Clinical.EvFormRecordFields ( Settings );
    }
    #endregion

    #region Class variables and property
    // 
    // Create instantiate the DAL class 
    // 
    private Evado.Dal.Clinical.EvFormRecordFields _dalRecordItems = new Evado.Dal.Clinical.EvFormRecordFields( );

    #endregion

    #region RunQuery  methods

    // =====================================================================================
    /// <summary>
    /// This method queries the database using the query parameters in 
    /// the DataAnalysisQuery object.
    /// </summary>
    /// <param name="Chart">EvChart: A Chart object</param>
    /// <returns>EvChart: A Chart object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define the chart's legend based on chart grouping options. 
    /// 
    /// 2. Create Chart's legend based on the chart grouping options
    /// 
    /// 3. If the chart's safety reporting option is All items or exeptions, 
    /// execute the method for querying the chart records and 0 item tableColumn. 
    /// 
    /// 4. Else, loop through the chart object and execute the method for querying the chart records
    /// based on chart object and itemCount
    /// 
    /// 5. Return new Chart object, if the chart's series has no value. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvChart runQuery ( EvChart Chart )
    {
      this.LogMethod( "runQuery method. " );
      this.LogDebug ( "ProjectId: " + Chart.ProjectId );

      // 
      // Initialise the method variables and objects.
      // 
      int ReturnItems = 0;

      // 
      // Initialise the chart title
      // 
      Chart.Title = "Data analysis query for project " + Chart.ProjectId;

      // 
      // Define the category legend
      // 
      if ( Chart.Grouping == EvChart.GroupingOptions.Visit )
      {
        Chart.CategoryLegend = "Trial Visits";
      }
      if ( Chart.Grouping == EvChart.GroupingOptions.Date )
      {
        Chart.CategoryLegend = "Record Date";
      }

      this.LogDebug ( "Grouping: " + Chart.Grouping );

      //
      // Create chart's legend. 
      //
      this.createChartLegendList( Chart );

      // 
      // Select the chart QueryType
      // 
      if ( Chart.SafetyReporting == EvChart.SafetyReportingOptions.AllItems
        || Chart.SafetyReporting == EvChart.SafetyReportingOptions.Exceptions )
      {
        ReturnItems = queryRecords( Chart, 0 );
      }
      else
      {
        // 
        // If the source is a record then qury the trial FirstSubject TestItemList
        // 
        for ( int itemCount = 0; itemCount < Chart.QueryItems.Count; itemCount++ )
        {
          // 
          // If the itemId exists then execute the query.
          // 
          if ( Chart.QueryItems [ itemCount ].ItemId != String.Empty )
          {
              ReturnItems = this.queryRecords( Chart, itemCount );

          }//END trial exists.

        }//END newField interation loop

      }//END TestItemList selected.

      if ( Chart.Series.Count == 0 )
      {
        Chart.Series.Add( new EvChartSeries( ) );
      }

      return Chart;

    }//END runQuery method.

    // =====================================================================================
    /// <summary>
    /// This method generates the sample legend for the categories.
    /// </summary>
    /// <param name="chart">EvChart: The Chart object.</param>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. If the chart is grouped by date, exit. 
    /// 
    /// 2. If the chart is grouped by visit, add milestones to the chart's category. 
    /// 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private void createChartLegendList ( EvChart chart )
    {
      this.LogMethod( "getChartSampleLegend method." );
      this.LogDebug ( "ProjectId: " + chart.ProjectId );

      // 
      // Initialise method variables and object
      //
      EvMilestones milestones = new EvMilestones( this.ClassParameter);
      List<EvMilestone> milestoneList = new List<EvMilestone>(  );
      List<EvMilestone.MilestoneTypes> milestoneTypes = new List<EvMilestone.MilestoneTypes>();
      milestoneTypes.Add( EvMilestone.MilestoneTypes.Clinical );
      milestoneTypes.Add( EvMilestone.MilestoneTypes.Monitored );

      // 
      // If the index is instance then insert a numeric index.
      // 
      if ( chart.Grouping == EvChart.GroupingOptions.Date )
      {
        this.LogDebug (  "The Grouping is Date" );

        return;
      }

      // 
      // If the index is Visit then insert the trial milestones in order or execution
      // 
      if ( chart.Grouping == EvChart.GroupingOptions.Visit )
      {
        this.LogDebug (  "The Grouping is Visit" );

        // 
        // Get a currentSchedule of milestones.
        // 
        milestoneList = milestones.getIssuedScheduleMilestoneList( chart.ProjectId, chart.ScheduleId, milestoneTypes );

        this.LogDebug (  "Returned Milestone status: " + milestoneList.Count );

        // 
        // Extract the Visits into the chart time base.
        // 
        foreach ( EvMilestone milestone in  milestoneList )
        {
           chart.Categories.Add(  milestone.MilestoneId );

           this.LogDebug (  milestone.MilestoneId );
        }

      }//END  visit.

      this.LogDebug (  "END createChartLegendList method. " );
      return;

    }//END createChartLegendList Method

    // =====================================================================================
    /// <summary>
    /// This method queries the database using the query parameters in 
    /// the DataAnalysisQuery object.
    /// </summary>
    /// <param name="chart">EvChart: The Chart object.</param>
    /// <param name="ItemIndex">Integer: The item index</param>
    /// <returns>Integer: the number of query records</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for getting ResultData items list and extract the first row to chart series object.
    /// 
    /// 2. Loop through the ResultData items list 
    /// 
    /// 3. If the SubjectId exists, add new record to chartSeries object and set the SeriesCount to 0. 
    /// 
    /// 4. Else, add values to the matching category and increase the SeriesCount
    /// 
    /// 5. If the chart series object is not empty, add it to the chart object. 
    /// 
    /// 6. Return the counting number of DataRows Field list. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private int queryRecords ( EvChart chart, int ItemIndex )
    {
       this.LogMethod( "queryEvRecords method. " );
      this.LogDebug ( "ProjectId: " + chart.ProjectId );

      //
      // Initialise the method variables and objects
      // 
      List<EvDataItem> recordlList = new List<EvDataItem>( );
      EvChartSeries chartSeries = new EvChartSeries( );
      string itemId = String.Empty;
      int seriesCount = 0;

      // 
      // If the letter index is greater then the list return empty list.
      // 
      if ( ItemIndex >= chart.QueryItems.Count )
      {
        return 0;
      }

      // 
      // Query the record items
      // 
      recordlList = this._dalRecordItems.GetAnalysisItems(
        chart.ProjectId,
        chart.QueryItems [ ItemIndex ].ItemId,
        chart.Grouping,
        chart.QueryItems [ ItemIndex ].Aggregation);

      this.LogClass ( this._dalRecordItems.Log );
      this.LogDebug (  "Recordlist count: " + recordlList.Count  );

      // 
      // If there is query ResultData then load the output series.
      // 
      if ( recordlList.Count > 0 )
      {
        this.LogDebug (  "Processing List Items." );

        //
        // Initialise the first results series.
        //
        chartSeries = createNewSeries( recordlList[ 0 ], chart.Categories.Count );

        // 
        // Iterate through the ResultData extracting the ResultData values.
        // 
        this.LogDebug (  "Initial SubjectId :" + chartSeries.SubjectId );
        this.LogDebug (  "DataPoint : " + chartSeries.ItemId );

        this.LogDebug ( "'SubjectId' > 'DataPoint' > 'Unit' > 'Value'" );

        foreach ( EvDataItem dataItem in recordlList )
        {
          this.LogDebug (  "'" + dataItem.SubjectId 
           + "' > '" + dataItem.DataPoint
           + "' > '" + dataItem.Unit
           + "' > '" + dataItem.Value + "'" );

          // 
          // If there is a change in FirstSubject id then create a new series.
          // 
          if ( chartSeries.SubjectId != dataItem.SubjectId )
          {
            this.LogDebug ( "New Series for subject: " + dataItem.SubjectId );

            // 
            // Add the series to the chart
            // 
            chart.Series.Add( chartSeries );

            //
            // Initialise the new series.
            //
            chartSeries = createNewSeries( dataItem, chart.Categories.Count );

            seriesCount = 0;

            chartSeries.SubjectId = dataItem.SubjectId;

          }//END new FirstSubject new series

          // 
          // Add the value to the matching category.
          // 
          int index = this.getCateogryIndex( chart.Categories, dataItem.DataPoint );

          if ( index < chart.Categories.Count )
          {
            chartSeries.Values[index ] =  dataItem.Value ;
          }

          // 
          // Increment the series tableColumn;
          // 
          seriesCount++;

        }//END record list interation loop

        // 
        // If the list series is not empty add it to the chart.
        // 
        if ( chartSeries.ItemId != String.Empty )
        {
          chart.Series.Add( chartSeries );
        }

      }//Record tableColumn non zero.

      this.LogDebug (  "seriesCounter: " + seriesCount );
      this.LogDebug (  "Chart Series Count: " + chart.Series.Count );

      // 
      // Return the chart object.
      // 
      return recordlList.Count;

    }//END queryEvRecords method

    // =====================================================================================
    /// <summary>
    /// This method initialises a new chart series object.
    /// </summary>
    /// <param name="DataItem">EvDataItem: The ResultData item object containsing the ResultData value.</param>
    /// <param name="CategoryCount">Integer: The length of the Category in the chart.</param>
    /// <returns>EvChartSeries: The chart series object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Add the milestone detail and unit details to ChartSeries' legend, if they are not empty. 
    /// 
    /// 2. Return the ChartSeries object. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private EvChartSeries  createNewSeries( EvDataItem DataItem, int CategoryCount )
    {
      EvChartSeries chartSeries = new EvChartSeries( CategoryCount );

      chartSeries.ItemId = DataItem.FieldId;
      chartSeries.Unit = DataItem.Unit;

      // 
      // Define the legend for the series
      //
      chartSeries.Legend = DataItem.Subject;

      //
      // If the SubjectId is not empty, add the milestone detail to the chartSeries' Legend
      //
      if ( DataItem.SubjectId != String.Empty )
      {
        chartSeries.Legend = DataItem.SubjectId
          + " > " + DataItem.Subject; ;
      }

      //
      // If the Unit is not empty, add the Unit detail to the chartSeries' Legend
      //
      if ( DataItem.Unit != String.Empty )
      {
        chartSeries.Legend += " (" + DataItem.Unit + ")";
      }

      //
      // Replace "," with " ". 
      //
      chartSeries.Legend = chartSeries.Legend.Replace( ",", "" );

      //
      // Return the ChartSeries
      //
      return chartSeries;
    }

    // =====================================================================================
    /// <summary>
    /// This method returns the category index for the entered category.
    /// </summary>
    /// <param name="Categories">List of String: A list of Category strings</param>
    /// <param name="Category">string: a Category string to be indexed.</param>
    /// <returns>Integer: an index number of category</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Loop through the Categories list. 
    /// 
    /// 2. Return 999, if the Category is empty. 
    /// 
    /// 3. Else, return the category's index
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private int getCateogryIndex ( List<String> Categories, string Category )
    {
      // 
      // Iterate through the currentSchedule looking for the matching category.
      // 
      for ( int index = 0; index < Categories.Count; index++ )
      {
        if ( Categories [ index ] == String.Empty )
        {
          return 999;
        }
        if ( Categories [ index ] == Category )
        {
          return index;
        }
      }
      return 999;

    }//END getCateogryIndex method

    #endregion

  }//END EvDataAnalysis Class.

}//END namespace Evado.Evado.BLL 
