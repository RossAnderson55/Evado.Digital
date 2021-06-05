/***************************************************************************************
 * <copyright file="Evado.UniForm.Model\Table.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2013 - 2021 EVADO HOLDING PTY. LTD.  All rights reserved.
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
 *  This class contains the ClientPageFieldTable data object.
 *
 ****************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

namespace Evado.UniForm.Model
{  /// 
  /// Business entity used to model ClientPageField
  /// 
  [Serializable]
  public class Table
  {

    #region Class Constants
    /// <summary>
    /// This contant defines the column of Table is 10.
    /// </summary>
    public const int Columns = 10;

    #endregion

    #region class Initialisation Methods.

    //  =================================================================================
    /// <summary>
    /// This method initialiseas the header array.
    /// </summary>
    //  ---------------------------------------------------------------------------------

    public Table ( )
    {
      // 
      // Initialise the header array.
      // 
      for ( int i = 0; i < 10; i++ )
      {
        this._Header [ i ] = new TableColHeader ( );
      }
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class PropertyList

    private TableColHeader [ ] _Header = new TableColHeader [ 10 ];
    /// <summary>
    /// This property contains the  Header of the TableColHeader object.
    /// </summary>
    public TableColHeader [ ] Header
    {
      get { return this._Header; }
      set { this._Header = value; }
    }

    private int _ColumnCount = 1;
    /// <summary>
    /// This property contains the count of the column.
    /// </summary>
    public int ColumnCount
    {
      get
      {
        this._ColumnCount = 0;
        if ( _Header != null )
        {
          for ( int i = 0; i < _Header.Length; i++ )
          {
            if ( this._Header [ i ].Text != String.Empty )
            {
              this._ColumnCount++;
            }
          }
        }
        return this._ColumnCount;
      }

    }

    private List<TableRow> _Rows = new List<TableRow> ( );
    /// <summary>
    /// This property contains the list of the TableRow object.
    /// </summary>
    public List<TableRow> Rows
    {
      get { return this._Rows; }
      set { this._Rows = value; }
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class Methods
    //=================================================================================
    /// <summary>
    /// This method adds a new row in the table.
    /// </summary>
    /// <returns>Evado.UniForm.Model.TableRow</returns>
    //---------------------------------------------------------------------------------
    public TableColHeader [ ] setHeader ( int ColumnCount )
    {
      this._Header = new Evado.UniForm.Model.TableColHeader [ ColumnCount ];

      for( int i=0; i<ColumnCount; i++ )
      {
        this._Header [ i ] = new TableColHeader ( );
      }

      return this._Header;
    }
    //=================================================================================
    /// <summary>
    /// This method adds a new row in the table.
    /// </summary>
    /// <returns>Evado.UniForm.Model.TableRow</returns>
    //---------------------------------------------------------------------------------
    public TableRow addRow ( )
    {
      Evado.UniForm.Model.TableRow row = new Evado.UniForm.Model.TableRow ( );

      this._Rows.Add ( row );

      return row;
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion


  } // Close ClientPageFieldTable class

} // Close namespace  Evado.UniForm.Model
