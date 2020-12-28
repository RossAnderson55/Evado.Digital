/***************************************************************************************
 * <copyright file="Evado.Model.UniForm\TableRow.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2013 - 2017 EVADO HOLDING PTY. LTD..  All rights reserved.
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
 *  This class contains the ClientPageTableRow data object.
 *
 ****************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

namespace Evado.Model.UniForm
{
  /// 
  /// Business entity used to model ClientPageField
  /// 
  [Serializable]
  public class TableRow
  {

    #region Class Intialisation Methods


    /// ==================================================================================
    /// <summary>
    /// This constructor intialises an empty string to _Column array. 
    /// </summary>
    
    // ----------------------------------------------------------------------------------
    public TableRow( )
    {
      for ( int i = 0; i < _Column.Length; i++ )
      {
        this._Column [ i ] = String.Empty;
      }//END _Column.Length iteration
    }//END TableRow method
    #endregion

    #region Class Constants
    private const int MaxRows = 50;

    #endregion

    #region Class PropertyList

    private int _No = 0;
    /// <summary>
    /// This property contains the row number of the table.
    /// </summary>
    public int No
    {
      get { return this._No; }
      set { this._No = value; }
    }
    private string [ ] _Column = new String [ Table.Columns ];
    /// <summary>
    /// This property contains an array of column data.
    /// </summary>
    public string [ ] Column
    {
      get { return this._Column; }
      set { this._Column = value; }
    }

    #endregion
  }//END ClientPageTableRow Class

} // Close namespace  Evado.Model.UniForm
