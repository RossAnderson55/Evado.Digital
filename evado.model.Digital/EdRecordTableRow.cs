/***************************************************************************************
 * <copyright file="EvFormFieldTableRow.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvFormFieldTableRow data object.
 *
 ****************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

namespace Evado.Model.Digital
{
  /// <summary>
  /// This class defines the form field table row data object.
  /// </summary>
  [Serializable]
  public class EdRecordTableRow
  {

    #region Public method
    /// <summary>
    /// This class loops the table row and clear its column.
    /// </summary>
    public EdRecordTableRow()
    {
      for (int i = 0; i < _Column.Length; i++)
      {
        this._Column[i] = String.Empty;
      }
    }
    #endregion

    #region Internal variables
    private const int MaxRows = 50;
    private int _No = 0;
    private string[] _Column = new String[EdRecordTable.Columns];
    #endregion

    #region Class property
    /// <summary>
    /// This property contains the row number of a table
    /// </summary>
    public int No
    {
      get { return this._No; }
      set { this._No = value; }
    }

    /// <summary>
    /// This property contains the row column of a table
    /// </summary>
    public string[] Column
    {
      get { return this._Column; }
      set { this._Column = value; }
    }

    /// <summary>
    /// This property contains the row column_0 of a table
    /// </summary>
    public string Column_0
    {
      get { return this._Column[0]; }
      set { this._Column[0] = value; }
    }

    /// <summary>
    /// This property contains the row column_1 of a table
    /// </summary>
    public string Column_1
    {
      get { return this._Column[1]; }
      set { this._Column[1] = value; }
    }

    /// <summary>
    /// This property contains the row column_2 of a table
    /// </summary>
    public string Column_2
    {
      get { return this._Column[2]; }
      set { this._Column[2] = value; }
    }

    /// <summary>
    /// This property contains the row column_3 of a table
    /// </summary>
    public string Column_3
    {
      get { return this._Column[3]; }
      set { this._Column[3] = value; }
    }

    /// <summary>
    /// This property contains the row column_4 of a table
    /// </summary>
    public string Column_4
    {
      get { return this._Column[4]; }
      set { this._Column[4] = value; }
    }

    /// <summary>
    /// This property contains the row column_5 of a table
    /// </summary>
    public string Column_5
    {
      get { return this._Column[5]; }
      set { this._Column[5] = value; }
    }

    /// <summary>
    /// This property contains the row column_6 of a table
    /// </summary>
    public string Column_6
    {
      get { return this._Column[6]; }
      set { this._Column[6] = value; }
    }

    /// <summary>
    /// This property contains the row column_7 of a table
    /// </summary>
    public string Column_7
    {
      get { return this._Column[7]; }
      set { this._Column[7] = value; }
    }

    /// <summary>
    /// This property contains the row column_8 of a table
    /// </summary>
    public string Column_8
    {
      get { return this._Column[8]; }
      set { this._Column[8] = value; }
    }

    /// <summary>
    /// This property contains the row column_9 of a table
    /// </summary>
    public string Column_9
    {
      get { return this._Column[9]; }
      set { this._Column[9] = value; }
    }
    #endregion

    /// <summary>
    /// This class gets the table row as a Xml text object.
    /// </summary>
    /// <returns>string: an xml text of table row</returns>
    public string xmlTr()
    {
      string _xmlTr = "<tr>\r\n";
      _xmlTr += "<td>" + this._Column[0] + "</td>";
      _xmlTr += "<td>" + this._Column[1] + "</td>";
      _xmlTr += "<td>" + this._Column[2] + "</td>";
      _xmlTr += "<td>" + this._Column[3] + "</td>";
      _xmlTr += "<td>" + this._Column[4] + "</td>";
      _xmlTr += "<td>" + this._Column[5] + "</td>";
      _xmlTr += "<td>" + this._Column[6] + "</td>";
      _xmlTr += "<td>" + this._Column[7] + "</td>";
      _xmlTr += "<td>" + this._Column[8] + "</td>";
      _xmlTr += "<td>" + this._Column[9] + "</td>\r\n";
      _xmlTr += "</tr>\r\n";

      return _xmlTr;
    }//END xmlTr method

  }//END EvFormFieldTableRow Class

}//END namespace Evado.Model.Digital
