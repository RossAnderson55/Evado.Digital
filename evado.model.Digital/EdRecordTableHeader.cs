/***************************************************************************************
 * <copyright file="EvFormFieldTableColumnHeader.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvFormFieldTableColumnHeader data object.
 *
 ****************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

namespace Evado.Model.Digital
{
  /// <summary>
  /// The Column Header Class definition.
  /// </summary>
  [Serializable]
  public class EdRecordTableHeader
  {

    #region Define the Object ScheduleStates as Constants.

    /// <summary>
    /// This contant defines an item type which is read only type.
    /// </summary>
    public const string ItemTypeReadOnly = "RO";

    /// <summary>
    /// This constant defines item type yes no for form field in table column 
    /// </summary>
    public const string ItemTypeYesNo = "YN";

    /// <summary>
    /// This constant defines item type text for form field in table column 
    /// </summary>
    public const string ItemTypeText = "TXT";

    /// <summary>
    /// This constant defines item type numeric for form field in table column 
    /// </summary>
    public const string ItemTypeNumeric = "NUM";

    /// <summary>
    /// This constant defines item type date for form field in table column 
    /// </summary>
    public const string ItemTypeDate = "DT";

    /// <summary>
    /// This constant defines item type matrix for form field in table column 
    /// </summary>
    public const string ItemTypeMatrix = "MAT";

    /// <summary>
    /// This constant defines item type radio button for form field in table column 
    /// </summary>
    public const string ItemTypeRadioButton = "RBL";

    /// <summary>
    /// This constant defines item type selection list for form field in table column 
    /// </summary>
    public const string ItemTypeSelectionList = "SL";

    /// <summary>
    /// This constant defines a number of columns for form field in table column 
    /// </summary>
    public const int COLUMNS = 10;
    #endregion

    #region Class property

    private int _No = 1;
    /// <summary>
    /// This property contains the header column number of table.
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
      get { return _ColumnId; }
      set { _ColumnId = value; }
    }

    private string _Text = String.Empty;
    /// <summary>
    /// This property contains the header column text of table.
    /// </summary>
    public string Text
    {
      get { return _Text; }
      set { _Text = value; }
    }

    private string _Width = String.Empty;
    /// <summary>
    /// This property contains the header column width of table.
    /// </summary>
    public string Width
    {
      get { return _Width; }
      set { _Width = value; }
    }

    private string _TypeId = String.Empty;
    /// <summary>
    /// This property contains the header column type identifier of table.
    /// </summary>
    public string TypeId
    {
      get { return this._TypeId; }
      set
      {
        this._TypeId = value.ToUpper ( );

        if ( this._TypeId == ItemTypeMatrix )
        {
          this._TypeId = ItemTypeReadOnly;
        }
      }
    }

    private string _OptionsOrUnit = String.Empty;
    /// <summary>
    /// This property contains the header column options or unit of table.
    /// </summary>
    public string OptionsOrUnit
    {
      get { return _OptionsOrUnit; }
      set { _OptionsOrUnit = value; }
    }

    private EvFormFieldValidationRules _ValidationRules = new EvFormFieldValidationRules ( );
    /// <summary>
    /// This property contains the header column validation rule object of table.
    /// </summary>
    public EvFormFieldValidationRules ValidationRules
    {
      get { return _ValidationRules; }
      set { _ValidationRules = value; }
    }

    String _cDashMetadata = String.Empty;
    /// <summary>
    /// This property contains cDash metadata values. 
    /// </summary>
    public string cDashMetadata
    {
      get
      {
        return this._cDashMetadata;
      }
      set
      {
        this._cDashMetadata = value;
      }
    }

    /// <summary>
    /// This property contains the header column xml text of table.
    /// </summary>
    public string xmlTh
    {
      get
      {
        string _xmlTh = "<th>";
        _xmlTh += "<Text>" + _Text + "</Text>";
        _xmlTh += "<Width>" + _Width + "</Width>";
        _xmlTh += "<TypeId>" + _TypeId + "</TypeId>";
        _xmlTh += "</th>\r\n";

        return _xmlTh;
      }
    }

    #endregion

    #region Public methods
    // =====================================================================================
    /// <summary>
    /// This method returns the Option list for the item.
    ///  
    /// Written by: Ross Anderson
    /// Date: 24/08/2005
    /// </summary>
    /// <returns>List of EvOptions</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize a return list
    /// 
    /// 2. Add a null option as first item for a selection list.
    /// 
    /// 3. Add items from option object to the return list.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public static List<EvOption> getTypeList ( EvDataTypes Datatype )
    {
      //
      // Initialize a return list
      //
      List<EvOption> List = new List<EvOption> ( );
      // 
      // Add a null option as first item for a selection list.
      // 
      EvOption Option = new EvOption();
      List.Add(Option);

      //
      // Add items from option object to the return list. 
      //
      Option = new EvOption("YN", "Boolean Column");
      List.Add(Option);

      Option = new EvOption("TXT", "Text Column");
      List.Add(Option);

      Option = new EvOption("NUM", "Numeric Column");
      List.Add(Option);

      Option = new EvOption("DT", "Date Column");
      List.Add(Option);

      Option = new EvOption("RBL", "Radio Button Column");
      List.Add(Option);

      Option = new EvOption("SL", "Selection List Column");
      List.Add(Option);

      if ( Datatype ==  EvDataTypes.Special_Matrix )
      {
        Option = new EvOption ( "RO", "Read Only" );
        List.Add ( Option );
      }
      
      // 
      //Return the completed Array List.
      //
      return List;

    }//END getTypeList method

    // =====================================================================================
    /// <summary>
    /// This class returns the Option list for the item.
    ///  
    /// Written by: Ross Anderson
    /// Date: 24/08/2005
    /// </summary>
    /// <returns>ArrayList: a list of options</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize a return list
    /// 
    /// 2. Add a null option as first item for a selection list.
    /// 
    /// 3. Loop ten rounds to add the width to the return list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public static List<EvOption> getWidthList ( )
    {
      //
      // Initialize a return list.
      //
      List<EvOption> List = new List<EvOption> ( );
      // 
      // Add a null option as first item for a selection list.
      // 
      EvOption Option = new EvOption("0", String.Empty);
      List.Add(Option);

      //
      // Loop ten rounds to add the width to the return list. 
      //
      for (int Count = 0; Count < 10; Count++)
      {
        string sWidth = (Count * 5 + 5).ToString();
        Option = new EvOption(sWidth, sWidth);
        List.Add(Option);
      }
      // 
      //Return the completed Array List.
      //
      return List;
    }//END getWidthList method

    // =====================================================================================
    /// <summary>
    /// This class returns the Option list for the item.
    ///  
    /// Written by: Ross Anderson
    /// Date: 24/08/2005
    /// </summary>
    /// <returns>ArrayList: a list of options</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize a return list
    /// 
    /// 2. Add a null option as first item for a selection list.
    /// 
    /// 3. Loop ten rounds to add the width to the return list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public static List<EvOption> getRowLengthList ( )
    {
      //
      // Initialize a return list.
      //
      List<EvOption> List = new List<EvOption> ( );
      EvOption Option = new EvOption ( );

      //
      // Loop five rounds to add the width to the return list. 
      //
      for ( int count = 0; count < 10; count++ )
      {
        string stRow = count.ToString ( );
        Option = new EvOption ( stRow, stRow );
        List.Add ( Option );
      }

      //
      // Loop ten rounds to add the width to the return list. 
      //
      for ( int Count = 0; Count < 20; Count++ )
      {
        string sWidth = ( Count * 2 + 10 ).ToString ( );
        Option = new EvOption ( sWidth, sWidth );
        List.Add ( Option );
      }
      // 
      //Return the completed Array List.
      //
      return List;
    }//END getWidthList method
    #endregion

  } //END EvFormFieldColumnHeader class 

} //END namespace Evado.Model.Digital
