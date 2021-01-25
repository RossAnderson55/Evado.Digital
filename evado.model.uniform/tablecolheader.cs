/***************************************************************************************
 * <copyright file="Evado.Model.UniForm\TableColumnHeader.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2013 - 2017 EVADO HOLDING PTY. LTD..  All rights reserved.
 *     
 *      The use and distribution terms for this software are contained in the file
 *      named \license.txt, which can be found in the root of this distribution.S
 *      By using this software in any fashion, you are agreeing to be bound by the
 *      terms of this license.
 *     
 *      You must not remove this notice, or any other, from this software.
 *     
 * </copyright>
 * 
 * Description: 
 *  This class contains the ClientPageTableColHeader data object.
 *
 ****************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

using Evado.Model;

namespace Evado.Model.UniForm
{
  /// <summary>
  /// The Column Header Class definition.
  /// </summary>
  [Serializable]
  public class TableColHeader
  {
    #region Class Constants

    /// <summary>
    /// This contant defines an item type which is read only type.
    /// </summary>
    public const string ItemTypeReadOnly = "RO";
    /// <summary>
    /// This constant defines an item type which is yes no type.
    /// </summary>
    public const string ItemTypeYesNo = "YN";
    /// <summary>
    /// This constant defines an item type which is text type. 
    /// </summary>
    public const string ItemTypeText = "TXT";

    /// <summary>
    /// This constant defines an item type which is numeric type.
    /// </summary>
    public const string ItemTypeNumeric = "NUM";
    /// <summary>
    /// This constant defines an item type which is date type.
    /// </summary>
    public const string ItemTypeDate = "DT";
    /// <summary>
    /// This constant defines an item type which is radio button type. 
    /// </summary>
    public const string ItemTypeRadioButton = "RBL";
    /// <summary>
    /// This constant defines an item which is selection list type. 
    /// </summary>
    public const string ItemTypeSelectionList = "SL";

    /// <summary>
    /// This constant defines  column value of a table is 10.
    /// </summary>
    public const int COLUMNS = 10;

    #endregion

    #region Class Internal Variables

    /// <summary>
    /// This variable _No contains the value 1.
    /// </summary>
    private int _No = 1;
    /// <summary>
    /// This variable _Text contains an empty string. 
    /// </summary>
    private string _Text = String.Empty;
    /// <summary>
    /// This variable _Width contains an empty string.
    /// </summary>
    private string _Width = String.Empty;
    /// <summary>
    /// This varaible _ColumnWidth contains an empty string.
    /// </summary>
    private string _ColumnWidth = String.Empty;
    /// <summary>
    /// THis varaible _TypeId conntains an empty string.
    /// </summary>
    private EvDataTypes _TypeId = EvDataTypes.Text;
    /// <summary>
    /// This variable _OptionOrUnit contains an empty string.
    /// </summary>
    private string _OptionsOrUnit = String.Empty;
    /// <summary>
    /// THis variable _Unit contains an empty string.
    /// </summary>
    private string _Unit = String.Empty;

    #endregion

    #region Class PropertyList

    /// <summary>
    /// This property contains the value 1.
    /// </summary>
    public int No
    {
      get { return _No; }
      set { _No = value; }
    }

    private string _ColumnId = String.Empty;
    /// <summary>
    /// This property contains the header column text of table.
    /// </summary>
    public string ColumnId
    {
      get
      {
        //
        // if column id is empty the use the colunn text as the identifier.
        //
        if ( this._ColumnId == String.Empty )
        {
          this._ColumnId = _No.ToString ( "00" );
        }

        return _ColumnId;
      }
      set { _ColumnId = value; }
    }
    /// <summary>
    /// This property contains a text value for Table Col Header object.
    /// </summary>
    public string Text
    {
      get { return _Text; }
      set { _Text = value; }
    }
    /// <summary>
    /// This property contains a width for Table Col Header object.
    /// </summary>
    public string Width
    {
      get { return _Width; }
      set { _Width = value; }
    }

    /// <summary>
    /// This property contains a TypeId value for Table Col Header object.
    /// </summary>
    public EvDataTypes TypeId
    {
      get { return _TypeId; }
      set
      {
        _TypeId = value;
      }
    }
    /// <summary>
    /// This property contains a options or an unit value for Table Col Header Object.
    /// </summary>
    public string OptionsOrUnit
    {
      get { return _OptionsOrUnit; }
      set { _OptionsOrUnit = value; }
    }

    private String _MinimumValue = String.Empty;

    /// <summary>
    /// This property contains the minumum validation range for numbers and dates as a string.
    /// </summary>
    public String MinimumValue
    {
      get { return _MinimumValue; }
      set { _MinimumValue = value; }
    }

    private String _MaximumValue = String.Empty;

    /// <summary>
    /// This property contains the maximum validation range for numbers and dates as a string.
    /// </summary>
    public String MaximumValue
    {
      get { return _MaximumValue; }
      set { _MaximumValue = value; }
    }

    private List<EvOption> _OptionList = new List<EvOption>( );
    /// <summary>
    /// This property contains a selection list that is displayed on the device client.
    /// </summary>
    public List<EvOption> OptionList
    {
      get { return _OptionList; }
      set { _OptionList = value; }
    }

    #endregion

    #region Class Methods

    // ==================================================================================
    /// <summary>
    /// This method adds a option as first item for a selection list and returns the completed array list.
    /// </summary>
    /// <returns>A list of EvOption Object </returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Add a null option as first item for a selection list. 
    /// 
    /// 2. Returns the Option list for the item.
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public static ArrayList getFirstTypeList( )
    {
      ArrayList List = new ArrayList( );
      // 
      // Add a null option as first item for a selection list.
      // 
      Evado.Model.EvOption option = new Evado.Model.EvOption( );
      List.Add( option );

      option = new EvOption( "TXT", "Text Column" );
      List.Add( option );

      option = new EvOption( "MAT", "Matix Table (First Column Fixed)" );
      List.Add( option );

      // 
      //Returns the Option list for the item.
      //
      return List;

    }//END getFirstTypeList method


    // ==================================================================================
    /// <summary>
    /// This method adds a option as first item for a selection list and returns the completed array list.
    /// </summary>
    /// <returns> A list of EvOption object </returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Add a null option as first item for a selection list. 
    /// 
    /// 2. Returns the Option list for the item.
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public static ArrayList getTypeList( )
    {
      ArrayList List = new ArrayList( );
      // 
      // Add a null option as first item for a selection list.
      // 
      Evado.Model.EvOption option = new Evado.Model.EvOption( );
      List.Add( option );

      option = new EvOption( "YN", "Yes/No Column" );
      List.Add( option );

      option = new EvOption( "TXT", "Text Column" );
      List.Add( option );

      option = new EvOption( "NUM", "Numeric Column" );
      List.Add( option );

      option = new EvOption( "DT", "Date Column" );
      List.Add( option );

      option = new EvOption( "RBL", "Radio Button Column" );
      List.Add( option );

      option = new EvOption( "SL", "Selection List Column" );
      List.Add( option );
      // 
      //Return the completed Array List.
      //
      return List;
    }//END getTypeList method


    // ==================================================================================
    /// <summary>
    /// This method adds a option as first item for a selection list and returns the completed array list.
    /// </summary>
    /// <returns> A list of EvOption object </returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Add a null option as first item for a selection list. 
    /// 
    /// 2. Returns the Option list for the item.
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public static List<EvOption> getWidthList ( )
    {
      List<EvOption> List = new List<EvOption> ( );
      // 
      // Add a null option as first item for a selection list.
      // 
      Evado.Model.EvOption option = new Evado.Model.EvOption( "0", String.Empty );
      List.Add( option );

      for ( int Count = 0; Count < 10; Count++ )
      {
        string sWidth = ( Count * 5 + 5 ).ToString( );
        option = new EvOption( sWidth, sWidth );
        List.Add( option );
      }
      // 
      //Return the completed Array List.
      //
      return List;
    }//END getWidthList method

  } //END ClientPageFieldColumnHeader class 

} //END namespace  Evado.Model.UniForm
    #endregion