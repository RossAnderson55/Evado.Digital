/***************************************************************************************
 * <copyright file="model\EvReportRow.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvReportRow data object.
 *
 ****************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

namespace Evado.Model.Digital
{
  /// <summary>
  /// data  entity used to model accounts
  /// </summary>
  [Serializable]
  public class EvReportRow
  {
    #region Static members
    /// <summary>
    /// This field defines the number of report columns
    /// </summary>
    public static int NoColumns = 1;

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class initialisation method
    /// <summary>
    /// This class method initialised ia a report row object.
    /// </summary>
    public EvReportRow()
    {
      this._ColumnValues[0] = String.Empty;
    }
    /// <summary>
    /// This class methods initialsies a report row with a defined number of columns.
    /// </summary>
    /// <param name="Columns">Int: number of columne in the row.</param>
    public EvReportRow(int Columns)
    {
      // 
      // Reinitialise the arrays.
      // 
      this._ColumnValues = new String[Columns];

      for (int Count = 0; Count < Columns; Count++)
      {
        this._ColumnValues[Count] = String.Empty;
      }
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class members

    private string[] _ColumnValues = new String[1];

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class members
    /// <summary>
    /// This property contains column values string array of report row.
    /// </summary>
    public string[] ColumnValues
    {
      get
      {
        return this._ColumnValues;
      }
      set
      {
        this._ColumnValues = value;
      }
    } //end _ColumnValues

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

  }//END EvReportRow class 

}//END Namespace Evado.Model.Digital
