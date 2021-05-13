/***************************************************************************************
 * <copyright file="model\EvReportSection.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvReportSection data object.
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
  public class EdReportSection
  {

    #region Class enumerations
    /// <summary>
    /// This enumeration list defines layout types
    /// </summary>
    public enum LayoutTypes
    {
      /// <summary>
      /// This enumeration defines a tabular layout type
      /// </summary>
      Tabular = 0,

      /// <summary>
      /// This enumeration defines a flat layout type.
      /// </summary>
      Flat = 1,
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class Constants
    //===================================================================================

    /// <summary>
    /// This constant value represents the Section level value of the whole report.
    /// </summary>
    public const int Report_Level = -1;

    /// <summary>
    /// This constant value represents the section level value of the detail.
    /// </summary>
    public const int Detail_Level = 0;

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class members

    /// <summary>
    /// Stores the column values by the header of the column.
    /// </summary>
    private Hashtable _ColumnValuesByHeaderText = new Hashtable ( );

    /// <summary>
    /// Stores the list of column object belonging to this section.
    /// </summary>
    private ArrayList _columnsList = new ArrayList ( );

    /// <summary>
    /// Stores the column object and the key is the field name.
    /// </summary>
    private Dictionary<String, EdReportColumn> _ColumnBySourceField = new Dictionary<string, EdReportColumn> ( );

    /// <summary>
    /// List of the EvReportSection who are children of this section.
    /// Empty if this is the detail.
    /// </summary>
    private ArrayList _childrenList = new ArrayList ( );

    /// <summary>
    /// Parent of this section. Null if it is the top section.
    /// </summary>
    private EdReportSection _parent;

    /// <summary>
    /// Layout of this section.
    /// </summary>
    private LayoutTypes _layout;

    /// <summary>
    /// Acumulated value of the columns who has totals.
    /// </summary>
    private Hashtable _acumulator = new Hashtable ( );

    /// <summary>
    /// Level of this section.
    /// </summary>
    private int _sectionLevel;

    /// <summary>
    /// Columns of the detail. The detail is always the las child of a section.
    /// This is used for graph formating purposes.
    /// </summary>
    private ArrayList _detailColumnsList;

    /// <summary>
    /// This is the value of the grouping coulumn. Is used to paint the totals.
    /// </summary>
    private string _groupingColumnValue;

    /// <summary>
    /// Original row number of this report.
    /// </summary>
    private int _RowNumber;

    /// <summary>
    /// Sets the section visible on the report.
    /// </summary>
    private bool _Visible = true;

    /// <summary>
    /// True if it must be shown the header only for columns that has totals.
    /// </summary>
    private bool _OnlyShowHeadersForTotalColumns = false;

    #endregion

    #region Properties
    /// <summary>
    /// This property contains the grouping column value of the report section
    /// </summary>
    public string GroupingColumnValue
    {
      get { return _groupingColumnValue; }
      set { _groupingColumnValue = value; }
    }

    /// <summary>
    /// This property indicates whether the report section is visible
    /// </summary>
    public bool Visible
    {
      get { return _Visible; }
      set { _Visible = value; }
    }

    /// <summary>
    /// This property indicates whether the report section is only 
    /// show headers for total columns
    /// </summary>
    public bool OnlyShowHeadersForTotalColumns
    {
      get { return _OnlyShowHeadersForTotalColumns; }
      set { _OnlyShowHeadersForTotalColumns = value; }
    }

    /// <summary>
    /// This property contains detail column list array of the report section
    /// </summary>
    public ArrayList DetailColumnsList
    {
      get { return _detailColumnsList; }


      set
      {
        this._detailColumnsList = value;
        //Adds the detail header to the parent.
        if ( Parent != null )
        {
          Parent.DetailColumnsList = value;
        }
      }

    }

    /// <summary>
    /// This property contains a section level number of the report section
    /// </summary>
    public int SectionLevel
    {
      get { return _sectionLevel; }
      set { _sectionLevel = value; }
    }

    /// <summary>
    /// This property contains an accumulator of the report section
    /// </summary>
    public Hashtable Acumulator
    {
      get { return _acumulator; }
      set { _acumulator = value; }
    }

    /// <summary>
    /// This property contains a layout object of the report section
    /// </summary>
    public LayoutTypes Layout
    {
      get { return _layout; }
      set { _layout = value; }
    }

    /// <summary>
    /// This property contains a section parent object of the report section
    /// </summary>
    public EdReportSection Parent
    {
      get { return _parent; }
      set { _parent = value; }
    }

    /// <summary>
    /// This property contains a column value by header text object of the report section
    /// </summary>
    public Hashtable ColumnValuesByHeaderText
    {
      get { return _ColumnValuesByHeaderText; }
      set { _ColumnValuesByHeaderText = value; }
    }

    /// <summary>
    /// This property contains a children list array of the report section
    /// </summary>
    public ArrayList ChildrenList
    {
      get { return _childrenList; }
      set { _childrenList = value; }
    }

    /// <summary>
    /// This property contains a column list array of the report section
    /// </summary>
    public ArrayList ColumnList
    {
      get { return _columnsList; }
      set
      {
        this._columnsList = value;

        foreach ( EdReportColumn column in this._columnsList )
        {
          this._ColumnBySourceField [ column.SourceField ] = column;
        }

      }
    }

    /// <summary>
    /// This property contains a row number of the report section
    /// </summary>
    public int RowNumber
    {
      get { return _RowNumber; }
      set { _RowNumber = value; }
    }
    #endregion

    #region Public methods

    //  =================================================================================
    /// <summary>
    /// This class searches for the value of an specific field name on the parents of this section.
    /// If nothing is found, returns empty.
    /// </summary>    
    /// <param name="headerText">String: a header text</param>
    /// <param name="onlyOnParents">Boolean: true, if header is only on parents</param>
    /// <returns>String: a search value by header text</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. If search only on parents is false, return the header text
    /// 
    /// 2. If not only search on parents, search for a value based on the header text
    /// 
    /// 3. If not only search on parents, search for a value based on the header text
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public String searchValueByHeaderText ( String headerText, bool onlyOnParents )
    {
      //
      // If search only on parents is false, return the header text
      //
      if ( onlyOnParents == false )
      {

        if ( this._ColumnValuesByHeaderText.ContainsKey ( headerText ) == true )
        {
          return (String) this._ColumnValuesByHeaderText [ headerText ];
        }

      }//End if only on parents

      //
      // If not only search on parents, search for a value based on the header text
      //
      if ( this._parent != null )
      {
        return this._parent.searchValueByHeaderText ( headerText, false );
      }
      else
      {
        return String.Empty;
      }
    }//End of searchvalueByFieldNameOnParents class

    //  =================================================================================
    /// <summary>
    /// This class adds the value to the acumulator. 
    /// Each level should have an acumulator but the level 0 which is the detail
    /// When this value is added, this section will also add it to its parent.
    /// </summary>
    /// <param name="name">String: a name string</param>
    /// <param name="value">String: a value string</param>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. If this is the detail, dont add to the acumulator.
    /// 
    /// 2. If name does not exists, add name to acumulator
    /// 
    /// 3. Add new numeric values        
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public void addToAcumulator ( String name, String value )
    {
      //
      // If this is the detail, dont add to the acumulator.
      //
      if ( SectionLevel == 0 )
      {
        Acumulator = null;
      }
      else
      {
        String oldStringValue = (String) Acumulator [ name ];

        if ( oldStringValue == null || oldStringValue.Equals ( String.Empty ) )
        {
          oldStringValue = "0";
        }

        //
        // If name does not exists, add name to acumulator
        //
        if ( !Acumulator.Contains ( name ) )
        {
          Acumulator.Add ( name, oldStringValue );
        }

        //
        // Add new numeric values
        //
        double oldValue = double.Parse ( oldStringValue );
        double newValue = double.Parse ( value );
        double newTotal = oldValue + newValue;


        Acumulator [ name ] = "" + newTotal;
      }

      if ( Parent != null )
      {
        Parent.addToAcumulator ( name, value );
      }

    }//END addToAcumulator class

    #endregion

  }//END EvReportSection class 


}//END Namespace Evado.Model.Digital
