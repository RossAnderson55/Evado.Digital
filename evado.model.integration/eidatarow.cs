/***************************************************************************************
 * <copyright file="Evado.Model.Integration\EiDataRow.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2020 EVADO HOLDING PTY. LTD..  All rights reserved.
 *     
 *      The use and distribution terms for this software are contained in the file
 *      named license.txt, which can be found in the root of this distribution.
 *      By using this software in any fashion, you are agreeing to be bound by the
 *      terms of this license.
 *     
 *      You must not remove this notice, or any other, from this software.
 *     
 * </copyright>
 * 
 * Description: 
 *  This class contains the EvCaseReportForms business object.
 *
 ****************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evado.Model.Integration
{
  /// <summary>
  /// This model class defines the Web Service Query structure.
  /// </summary>
  [Serializable]
  public class EiDataRow
  {
    /// <summary>
    /// This method initialises the class
    /// </summary>
    public EiDataRow ( )
    {
    }

    /// <summary>
    /// This initialisation method initialises the class 
    /// and initialises the values.
    /// </summary>
    /// <param name="Columns">List of string</param>
    public EiDataRow (
      int Columns )
    {
      this.Values = new String [ Columns ];

      for ( int i = 0; i < Columns; i++ )
      {
        this.Values [ i ] = String.Empty;
      }
    }

    /// <summary>
    /// This property defines the number of columns in the row.
    /// </summary>
    public int Columns
    {
      get
      {
        return this._Values.Length;
      }
      set
      {
        int columns = value;
        String [ ] oldValues = new String [ this._Values.Length ];

        //
        // copy the old row of values
        //
        for ( int i = 0; i < this._Values.Length; i++ )
        {
          oldValues [ i ] = this._Values [ i ];
        }

        //
        // Redimension the value array.
        //
        this._Values = new String [ columns ];

        //
        // copy the old values into the new values array.
        //
        for ( int i = 0; i < columns; i++ )
        {
          if ( 1 < oldValues.Length )
          {
            oldValues [ i ] = this._Values [ i ];
          }
        }

      }
    }

    String [ ] _Values = new String [ 0 ];
    /// <summary>
    /// This property defines the column name (external identifier)
    /// </summary>
    public String [ ] Values
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

    //===================================================================================
    /// <summary>
    /// This method upudates a value to the value list.
    /// </summary>
    /// <param name="Column">int column</param>
    /// <param name="Value">String value</param>
    /// <returns>Bool:  True = Success</returns>
    //-----------------------------------------------------------------------------------
    public bool updateValue ( int Column, String Value )
    {
      if ( Column < this._Values.Length )
      {
        this._Values [ Column ] = Value;
        return true;
      }
      return false;
    }

    //===================================================================================
    /// <summary>
    /// This method upudates a value to the value list.
    /// </summary>
    /// <param name="Column">int column</param>
    /// <param name="Value">String value</param>
    /// <returns>Bool:  True = Success</returns>
    //-----------------------------------------------------------------------------------
    public bool updateValue ( int Column, object Value )
    {
      if ( Column < this._Values.Length )
      {
        this._Values [ Column ] = Value.ToString();
        return true;
      }
      return false;
    }
  }//END EiDataRow

}//END Namespace Evado.Model.Integration
