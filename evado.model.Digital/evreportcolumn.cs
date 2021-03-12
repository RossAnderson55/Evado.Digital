/***************************************************************************************
 * <copyright file="model\EvReportColumnHeader.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvReportColumnHeader data object.
 *
 ****************************************************************************************/

using System;
using System.Collections; using System.Collections.Generic;

namespace Evado.Model.Digital
{
  /// <summary>
  /// data  entity used to model accounts
  /// </summary>
  [Serializable]
  public class EvReportColumn
  {
    #region Initialisation method
    /// <summary>
    /// This method initialises th report column object.
    /// </summary>
    public EvReportColumn( )
    {
    }    
    #endregion

    #region Private members
    /// <summary>
    /// Contains the the column header text.
    /// 
    /// </summary>
    private Guid _Guid = Guid.Empty;

    private String _ColumnId = String.Empty;

    private int _SourceOrder = 0;

    /// <summary>
    /// Contains the the column header text.
    /// 
    /// </summary>
    private string _HeaderText = String.Empty;

    /// <summary>
    /// Contains the the source field name, could be a table column name or a object member name.
    /// 
    /// </summary>
    private string _SourceField = String.Empty;

    /// <summary>
    /// Contains the style width of the column, e.g. 50% or 140px.
    /// 
    /// </summary>
    private String _StyleWidth = String.Empty;

    /// <summary>
    /// Define the group this column is in.  group 0 is default.
    /// 
    /// </summary>
    private int _Group = 0;

    /// <summary>
    /// Defines if this column is a grouping index.
    /// 
    /// A value change of the grouping index will cause a new group to be created.
    /// 
    /// </summary>
    private bool _GroupingIndex = false; 

    /// <summary>
    /// This value defines the grouping type for this column. the default is none.
    /// Other options can are: Sum, Maximum, Minimum.
    /// 
    /// </summary>
    private EvReport.GroupingTypes _GroupingType = EvReport.GroupingTypes.None; 

    /// <summary>
    /// This member contains an array of floating point varables to store the group value at each 
    /// grouping level.
    /// 
    /// </summary>
    private float[] _GroupingValues = null; 

    /// <summary>
    /// This member defines the data type of the column and is used to determine whether the column 
    /// can be summed and how it can be formatted.
    /// 
    /// </summary>
    private EvReport.DataTypes _DataType = EvReport.DataTypes.Text; 

    /// <summary>
    /// This member contains the current column value of the group.
    /// 
    /// It is used to determine whether the the column value has changed as the formatting methods
    /// iterate through the data records.
    /// 
    /// </summary>
    private string _ValueFormatingString = String.Empty;

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class properties

    /// <summary>
    /// This property contains a global unique identifier of a report column
    /// </summary>
    public Guid Guid
    {
      get
      {
        return this._Guid;
      }
      set
      {
        this._Guid = value;
      }
    } //end HeaderText

    /// <summary>
    /// This property contains the source's column identifier.
    /// </summary>
    public String ColumnId
    {
      get { return _ColumnId; }
      set { _ColumnId = value; }
    }

    /// <summary>
    /// This property contains the source's coluomn order identifier.
    /// </summary>
    public int SourceOrder
    {
      get { return _SourceOrder; }
      set { _SourceOrder = value; }
    }

    /// <summary>
    /// This property contains a header text of a report column
    /// </summary>
    public string HeaderText
    {
      get
      {
        return this._HeaderText;
      }
      set
      {
        this._HeaderText = value;
      }
    } //end HeaderText

    /// <summary>
    /// This property contains a source field of a report column
    /// </summary>
    public string SourceField
    {
      get
      {
        return this._SourceField;
      }
      set
      {
        this._SourceField = value;
      }

    } //end _SourceField

    /// <summary>
    /// This property contains a style width of a report column
    /// </summary>
    public string StyleWidth
    {
      get
      {
        return this._StyleWidth;
      }
      set
      {
        this._StyleWidth = value;
      }

    } //end _StyleWidth

    /// <summary>
    /// This property contains a section level 1 of a report column
    /// </summary>
    public int SectionLvl
    {
      get
      {
        return this._Group;
      }
      set
      {
        this._Group = value;
      }

    } //end _Group

    /// <summary>
    /// This property indicates whether a report column is grouping index
    /// </summary>
    public bool GroupingIndex
    {
      get
      {
        return this._GroupingIndex;
      }
      set
      {
        this._GroupingIndex = value;
      }

    } //end _GroupingIndex

    /// <summary>
    /// This property contains a grouping type object of a report column
    /// </summary>
    public EvReport.GroupingTypes GroupingType
    {
      get
      {
        return this._GroupingType;
      }
      set
      {
        //
        // Set the column value based on this option.
        // 
        if ( this._DataType == EvReport.DataTypes.Text
          && this._DataType == EvReport.DataTypes.Bool
          && this._DataType == EvReport.DataTypes.Date )
        {
          this._GroupingType = EvReport.GroupingTypes.None;

          return;
        }

        // 
        // Set the value it is settable.
        // 
        this._GroupingType = value;

      }//END set 

    } //end GroupingType

    /// <summary>
    /// This property contains a of a report column
    /// </summary>
    public float[] GroupingValues
    {
      get
      {
        return this._GroupingValues;
      }
      set
      {
        this._GroupingValues = value;
      }

    } //end _GroupingValues

    /// <summary>
    /// This property contains a data type object of a report column
    /// </summary>
    public EvReport.DataTypes DataType
    {
      get
      {
        return this._DataType;
      }
      set
      {
        this._DataType = value;

        // 
        // Set the column value based on this option.
        // 
        if ( this._DataType == EvReport.DataTypes.Text
          && this._DataType == EvReport.DataTypes.Bool
          && this._DataType == EvReport.DataTypes.Date )
        {
          this._GroupingType = EvReport.GroupingTypes.None;
        }

      }//END set 

    } //end ColumnType

    /// <summary>
    /// This property contains a value formating string of a report column
    /// </summary>
    public string ValueFormatingString
    {
      get
      {
        return this._ValueFormatingString;
      }
      set
      {
        this._ValueFormatingString = value;
      }

    } //end _ValueFormatingString    
    #endregion

    /// <summary>
    /// This method returns a string value of the report column object.
    /// </summary>
    /// <returns></returns>
    public string getAsString ( )
    {
      String output = String.Empty;

      output += "CId: " + this._ColumnId;
      output += ", SO: " + this._SourceOrder;
      output += ", H: " + this._HeaderText;
      output += ", DF: " + this._SourceField;
      output += ", SW: " + this._StyleWidth;
      output += ", DT: " + this._DataType;
      output += ", SL: " + this.SectionLvl;
      output += ", GI: " + this.GroupingIndex;
      output += ", GT: " + this._GroupingType;

      return output;
    }

  }//END EvReportColumn class 

}//END Namespace Evado.Model.Digital
