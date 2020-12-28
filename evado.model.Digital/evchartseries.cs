/***************************************************************************************
 * <copyright file="EvChartSeries.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvChartSeries data object.
 *
 ****************************************************************************************/


using System;
using System.Collections.Generic;

namespace Evado.Model.Digital
{
  /// <summary>
  /// data  entity used to model accounts
  /// </summary>
  [Serializable]
  public class EvChartSeries
  {
    #region Class initialisation.

    /// <summary>
    /// The class initialisation method
    /// </summary>
    public EvChartSeries()
    {
    }
    /// <summary>
    /// Class initialisation method that initialises the value array.
    /// </summary>
    /// <param name="CategoryCount">The length of the value array.</param>
    public EvChartSeries( int CategoryCount)
    {
      for (int count = 0; count < CategoryCount; count++)
      {
        this.Values.Add(0F);
      }
    }

    #endregion

    #region Properties

    private string _ItemId = String.Empty;
    /// <summary>
    /// This property contains an item identifier of the chart series
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

    private string _SubjectId = String.Empty;
    /// <summary>
    /// This property contains a suject identifier of the chart series
    /// </summary>
    public string SubjectId
    {
      get
      {
        return this._SubjectId;
      }
      set
      {
        this._SubjectId = value;
      }
    }

    private string _Legend = String.Empty;
    /// <summary>
    /// This property contains a legend of the chart series
    /// </summary>
    public string Legend
    {
      get
      {
        return this._Legend;
      }
      set
      {
        this._Legend = value;
      }
    }

    private List<float> _Values = new List<float> ( );
    /// <summary>
    /// This property contains a value list of the chart series
    /// </summary>
    public List<float> Values
    {
      get
      {
        return this._Values;
      }
      set
      {
        this._Values = value;
      }
    }

    private string _Unit = String.Empty;
    /// <summary>
    /// This property contains a unit of the chart series
    /// </summary>
    public string Unit
    {
      get
      {
        return this._Unit;
      }
      set
      {
        this._Unit = value;
      }
    }

    /// <summary>
    /// This property contains a value 0 of the chart series
    /// </summary>
    public float Value_0
    {
      get
      {
        if (this._Values.Count > 1)
        {
          return this._Values[0];
        }
        return 0F;
      }
    }

    /// <summary>
    /// This property contains a value 1 of the chart series
    /// </summary>
    public float Value_1
    {
      get
      {
        if (this._Values.Count > 2)
        {
          return this._Values[1];
        }
        return 0F;
      }
    }

    /// <summary>
    /// This property contains a value 2 of the chart series
    /// </summary>
    public float Value_2
    {
      get
      {
        if (this._Values.Count > 3)
        {
          return this._Values[2];
        }
        return 0F;
      }
    }

    /// <summary>
    /// This property contains a value 3 of the chart series
    /// </summary>
    public float Value_3
    {
      get
      {
        if (this._Values.Count > 4)
        {
          return this._Values[3];
        }
        return 0F;
      }
    }

    /// <summary>
    /// This property contains a value 4 of the chart series
    /// </summary>
    public float Value_4
    {
      get
      {
        if (this._Values.Count > 5)
        {
          return this._Values[4];
        }
        return 0F;
      }
    }

    /// <summary>
    /// This property contains a value 5 of the chart series
    /// </summary>
    public float Value_5
    {
      get
      {
        if (this._Values.Count > 6)
        {
          return this._Values[5];
        }
        return 0F;
      }
    }

    /// <summary>
    /// This property contains a value 6 of the chart series
    /// </summary>
    public float Value_6
    {
      get
      {
        if (this._Values.Count > 7)
        {
          return this._Values[6];
        }
        return 0F;
      }
    }

    /// <summary>
    /// This property contains a value 7 of the chart series
    /// </summary>
    public float Value_7
    {
      get
      {
        if (this._Values.Count > 8)
        {
          return this._Values[7];
        }
        return 0F;
      }
    }

    /// <summary>
    /// This property contains a value 8 of the chart series
    /// </summary>
    public float Value_8
    {
      get
      {
        if (this._Values.Count > 9)
        {
          return this._Values[8];
        }
        return 0F;
      }
    }

    /// <summary>
    /// This property contains a value 9 of the chart series
    /// </summary>
    public float Value_9
    {
      get
      {
        if (this._Values.Count > 10)
        {
          return this._Values[9];
        }
        return 0F;
      }
    }
    #endregion

  }//END class EvChartSeries

}//END Namespace Evado.Model
