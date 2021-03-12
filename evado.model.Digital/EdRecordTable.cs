/***************************************************************************************
 * <copyright file="EvFormFieldTable.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvFormFieldTable data object.
 *
 ****************************************************************************************/

using System;
using System.Collections; using System.Collections.Generic;

namespace Evado.Model.Digital
{  
  /// <summary>
  /// This class defines the form field table data object.
  /// </summary>
    [Serializable]
    public class EdRecordTable
    {
      /// <summary>
      /// This constant defines the number of columns in a table.
      /// </summary>
      public const int Columns = 10;

      #region class initialisation methods.

      // ==================================================================================
      /// <summary>
      /// This class generates a form field table with 10 rows.
      /// </summary>
      // ----------------------------------------------------------------------------------
      public EdRecordTable( )
      {
        this._Rows = new List<EdRecordTableRow> ( );

        // 
        // Initialise the header array.
        // 
        for ( int i = 0; i < 10; i++ )
        {
          this._Header [ i ] = new EdRecordTableHeader( );
        }
      }//END EvFormFieldTable method.

      // ==================================================================================
      /// <summary>
      /// This class generates a form field table with a RowCount rows.
      /// </summary>
      /// <param name="RowCount">integer: a row count</param>
      // ----------------------------------------------------------------------------------
      public EdRecordTable( int RowCount )
      {
        // 
        // Redemension the array.
        // 
        this._Rows = new List<EdRecordTableRow> ( );

        // 
        // Initialise the array.
        // 
        for ( int i = 0; i < RowCount; i++ )
        {
          EdRecordTableRow row = new EdRecordTableRow ( );
          row.No = i + 1;
          this._Rows.Add( row) ;
        }

        // 
        // Initialise the header array.
        // 
        for ( int i = 0; i < 10; i++ )
        {
          this._Header [ i ] = new EdRecordTableHeader( );
        }

      }//END EvFormFieldTable method
      #endregion

      #region private variables.

      private EdRecordTableHeader [] _Header = new EdRecordTableHeader [ 10 ];
      private List<EdRecordTableRow> _Rows = new List<EdRecordTableRow>();
      private int _ColumnCount = 1;
      private String _PreFilledColumnList = "0";

      //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      #endregion

      #region properties.
      /// <summary>
      /// This property contains a header array of a form field table
      /// </summary>
      public EdRecordTableHeader [] Header
      {
        get { return this._Header; }
        set { this._Header = value; }
      }

      /// <summary>
      /// This property contains a column count of a form field table
      /// </summary>
      public int ColumnCount
      {
        get 
        {
          this._ColumnCount = 0;
          for ( int i = 0; i < _Header.Length; i++ )
          {
            if ( this._Header [ i ].Text != String.Empty )
            {
              this._ColumnCount++;
            }
          }
          return this._ColumnCount; }

        set { this._ColumnCount = value; }
      }

      /// <summary>
      /// This property contains a prefilled column list of a form field table
      /// </summary>
      public String PreFilledColumnList
      {
        get
        {
          return this._PreFilledColumnList;
        }

        set { this._PreFilledColumnList = value; }
      }

      /// <summary>
      /// This property contains a row array of a form field table
      /// </summary>
      public List<EdRecordTableRow> Rows
      {
        get { return this._Rows; }
        set { this._Rows = value; }
      }

      //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      #endregion

      #region methods.
      // ================================================================================
      /// <summary>
      /// This method redimensions the table rows.
      /// </summary>
      /// <param name="RowCount"></param>
      // --------------------------------------------------------------------------------
      public void SetRowCount ( int RowCount )
      {
        // 
        // selection matches the currentMonth lengh exit as nothing needs to be done.
        // 
        if ( RowCount == this._Rows.Count )
        {
          return;
        }

        // 
        // Create the new row arry.
        // 
        List<EdRecordTableRow> newRows = new List<EdRecordTableRow> ( );

        // 
        // Initialise the new array and fill it with the currentMonth values.
        // 
        for ( int row = 0; row < RowCount; row++ )
        {
          //
          // Initialise the new row.
          //
          newRows.Add ( new EdRecordTableRow ( ) );

          // 
          // Fill the new row with the old rows contents.
          // 
          if ( row < this._Rows.Count )
          {
            newRows [ row ] = this._Rows [ row ];
          }
          newRows [ row ].No = ( row + 1 );

        }//END row initialise interation

        // 
        // Update the row list.
        // 
        this._Rows = newRows;

      }//END SetRowCount method. 

      //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      #endregion

    }//END EvFormFieldTable class

}//END namespace Evado.Model.Digital
