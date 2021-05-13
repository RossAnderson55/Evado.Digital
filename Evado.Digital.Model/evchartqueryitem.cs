/***************************************************************************************
 * <copyright file="ChartQueryItem.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the ChartQueryItem data object.
 *
 ****************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

namespace Evado.Digital.Model
{
  /// <summary>
  /// This class defines a query item for the chart.
  /// </summary>
  [Serializable]
  public class EvChartQueryItem
  {

    #region Internal member variables

    private string _ItemId = String.Empty;
    private string _ItemName = String.Empty;
    //private EvChart.SourceOptions _Source = EvChart.SourceOptions.Record;
    private EvChart.AggregationOptions _Aggregation = EvChart.AggregationOptions.Average;

    #endregion

    #region class properties
    /// <summary>
    /// This property contains an item identifier of the chart query item
    /// </summary>
    public string ItemId
    {
      get
      {
        return this._ItemId;
      }
      set
      {
        this._ItemId = value;
      }
    }

    /// <summary>
    /// This property contains an item name of the chart query item
    /// </summary>
    public string ItemName
    {
      get
      {
        return this._ItemName;
      }
      set
      {
        this._ItemName = value;
      }
    }

    /*
    /// <summary>
    /// This property contains a source option object of the chart query item
    /// </summary>
    public EvChart.SourceOptions Source
    {
      get
      {
        return this._Source;
      }
      set
      {
        this._Source = value;
      }
    }
    */

    /// <summary>
    /// This property contains a aggregation option object of the chart query item
    /// </summary>
    public EvChart.AggregationOptions Aggregation
    {
      get
      {
        return this._Aggregation;
      }
      set
      {
        this._Aggregation = value;
      }
    }
    #endregion

  }//END class ChartQueryItem

}//END Namespace Evado.Model
