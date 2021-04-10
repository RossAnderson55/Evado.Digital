/***************************************************************************************
 * <copyright file="EvFormField.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvFormField data object.
 *
 ****************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;

namespace Evado.Model.Digital
{
  /// <summary>
  /// This class defines the Form Field design data object.
  /// </summary>
  [Serializable]
  public class EdRecordFieldDesign
  {
    /// <summary>
    ///  Class initialisation method.
    /// </summary>
    public EdRecordFieldDesign ( )
    {
      this.FieldWidth = 80;
      this.FieldHeight = 5;
    }

    #region Internal members

    private Guid _DictionaryGuid = Guid.Empty;
    private int _Order = 0;
    private int _SectionNo = -1;
    private string _Title = String.Empty;
    private string _Instructions = String.Empty;
    private string _HttpReference = String.Empty;
    private string _Options = String.Empty;
    private string _ExSelectionListId = String.Empty;
    private string _ExSelectionListCategory = String.Empty;
    private String _DefaultValue = String.Empty;
    private string _Unit = String.Empty;
    private string _UnitScaling = "0";
    private string _FieldCategory = String.Empty;
    private bool _SelectByCodingValue = true;
    private bool _Mandatory = false;
    private bool _AnalysticsDataPoint = false;
    private bool _HideField = false;
    private bool _SummaryField = false;
    private bool _AiDataPoint = true;
    private bool _MultiLineTextField = false;
    private bool _HorizontalButtons = false;
    private string _FormIds = String.Empty;
    private string _InitialOptionList = String.Empty;
    private int _InitialVersion = 0;

    private string _JavaScript = String.Empty;

    private string _AnalogueLegendStart = String.Empty;
    private string _AnalogueLegendFinish = String.Empty;
    private EdRecordTable _Table = null;
    #endregion

    #region class property

    EvDataTypes _TypeId = EvDataTypes.Null;
    /// <summary>
    /// This property contains a type identifier of a form field.
    /// </summary>
    public Evado.Model.EvDataTypes TypeId
    {
      get
      {
        return this._TypeId;
      }
      set
      {
        this._TypeId = value;
      }
    }

    /// <summary>
    /// This property contains a dictionary global unique identifier of form field design
    /// </summary>
    public Guid DictionaryGuid
    {
      get
      {
        return this._DictionaryGuid;
      }
      set
      {
        this._DictionaryGuid = value;
      }
    }

    /// <summary>
    /// This property contains an order of form field design
    /// </summary>
    public int Order
    {
      get
      {
        return this._Order;
      }
      set
      {
        this._Order = value;
      }
    }

    /// <summary>
    /// This property contains a section of form field design
    /// </summary>
    public int SectionNo
    {
      get
      {
        return this._SectionNo;
      }
      set
      {
        this._SectionNo = value;
      }
    }

    /// <summary>
    /// This property contains a subject of form field design
    /// </summary>
    public string Title
    {
      get
      {
        return this._Title;
      }
      set
      {
        this._Title = value.Trim ( );
      }
    }

    /// <summary>
    /// This property contains an instruction of form field design
    /// </summary>
    public string Instructions
    {
      get
      {
        return this._Instructions;
      }
      set
      {
        this._Instructions = value.Trim ( );

        this._Instructions = this._Instructions.Replace ( "\n", "~" );
        this._Instructions = this._Instructions.Replace ( "\r", "~" );
        this._Instructions = this._Instructions.Replace ( "~~", "~" );
        this._Instructions = this._Instructions.Replace ( "~", " \r\n" );
        this._Instructions = this._Instructions.Replace ( "[b]", "__" );
        this._Instructions = this._Instructions.Replace ( "[/b]", "__" );
        this._Instructions = this._Instructions.Replace ( "[i]", "_" );
        this._Instructions = this._Instructions.Replace ( "[/i]", "_" );
      }
    }

    /// <summary>
    /// This property contains a reference of form field design
    /// </summary>
    public string HttpReference
    {
      get
      {
        return this._HttpReference;
      }
      set
      {
        this._HttpReference = value.Trim ( );
      }
    }

    /// <summary>
    /// This property contains a selection list identifier of form field design
    /// </summary>
    public string ExSelectionListId
    {
      get
      {
        return this._ExSelectionListId;
      }
      set
      {
        this._ExSelectionListId = value;
      }
    }

    /// <summary>
    /// This property contains a selection list category of form field design
    /// </summary>
    public string ExSelectionListCategory
    {
      get
      {
        return this._ExSelectionListCategory;
      }
      set
      {
        this._ExSelectionListCategory = value;
      }
    }

    /// <summary>
    /// This property contains options of form field design
    /// </summary>
    public string Options
    {
      get
      {
        return this._Options;
      }
      set
      {
        this._Options = value;

        this.formatOptions ( );
      }
    }

    /// <summary>
    /// This method formats options that do not have a value to have a index instead.
    /// </summary>
    private void formatOptions ( )
    {
      this._Options = this._Options.Replace ( "; ", ";" );

      //
      // exit if options have values.
      //
      if ( this._Options.Contains ( ":" ) == true )
      {
        return;
      }

      String options = String.Empty;

      string [ ] arOptions = this._Options.Split ( ';' );

      for ( int i = 0; i < arOptions.Length; i++ )
      {
        if ( i > 0 )
        {
          options += ";";
        }

        options += i.ToString ( "#00" ) + ":" + arOptions [ i ];
      }

      this._Options = options;
    }

    /// <summary>
    /// This OptionList property contains a list of EvOption object containing the option selections.
    /// This list is generated from Options member (encoded list of options).
    /// </summary>
    public List<EvOption> OptionList
    {
      get
      {
        //
        // if the option string contain ':' value set the select by coding value to true.
        //
        if ( this._Options.Contains ( ":" ) == true
          && this._SelectByCodingValue == false )
        {
          this._SelectByCodingValue = true;
        }

        return EvcStatics.getStringAsOptionList ( this._Options);
      }
    }

    /// <summary>
    /// This property indicates if the field has numeric values.
    /// </summary>
    public bool hasNumericValues
    {
      get
      {
        switch ( this.TypeId )
        {
          case EvDataTypes.Numeric:
          case EvDataTypes.Integer:
          case EvDataTypes.Analogue_Scale:
          case EvDataTypes.Computed_Field:
            {
              return true;
            }

          case EvDataTypes.Radio_Button_List:
          case EvDataTypes.Horizontal_Radio_Buttons:
          case EvDataTypes.External_Selection_List:
          case EvDataTypes.Selection_List:
            {
              if ( this._Options != String.Empty )
              {
                foreach ( EvOption opt in this.OptionList )
                {
                  if ( EvStatics.isNumber ( opt.Value ) == false )
                  {
                    return false;
                  }
                }
                return true;
              }
              return false;
            }

          default:
            {
              return false;
            }
        }
      }
    }


    /// <summary>
    /// This property contains a JavaScript string for the validation rules
    /// </summary>
    public string JavaScript
    {
      get
      {
        return this._JavaScript;
      }
      set
      {
        this._JavaScript = value;
      }
    }

    /// <summary>
    /// This class property contains athe default numeric value.
    /// </summary>
    public String DefaultValue
    {
      get
      {
        return this._DefaultValue;
      }
      set
      {
        this._DefaultValue = value;
      }
    }

    /// <summary>
    /// This property contains a unit of form field design
    /// </summary>
    public string Unit
    {
      get
      {
        return this._Unit;
      }
      set
      {
        this._Unit = value;
      }
    }

    /// <summary>
    /// This property contains a unit scaling of form field design
    /// </summary>
    public string UnitScaling
    {
      get
      {
        return this._UnitScaling;
      }
      set
      {
        this._UnitScaling = value;
      }
    }

    /// <summary>
    /// This property contains a unit html of form field design
    /// </summary>
    public string UnitHtml
    {
      get
      {
        string sUnit = String.Empty;

        if ( this._UnitScaling != String.Empty
          && this._UnitScaling != "0" )
        {
          sUnit += "x10<span class='SuperScript'>" + this._UnitScaling + "</span>";
        }

        if ( this._Unit != String.Empty )
        {
          sUnit += this._Unit;
        }

        return sUnit;

      }
    }

    /// <summary>
    /// This property contains a field category of form field design
    /// </summary>
    public string FieldCategory
    {
      get
      {
        return this._FieldCategory.Trim ( );
      }
      set
      {
        this._FieldCategory = value;
      }
    }

    /// <summary>
    /// This property contains an analogue legend start of form field design
    /// </summary>
    public string AnalogueLegendStart
    {
      get
      {
        return this._AnalogueLegendStart.Trim ( );
      }
      set
      {
        this._AnalogueLegendStart = value;
      }
    }

    /// <summary>
    /// This property contains a analogue legend finish of form field design
    /// </summary>
    public string AnalogueLegendFinish
    {
      get
      {
        return this._AnalogueLegendFinish.Trim ( );
      }
      set
      {
        this._AnalogueLegendFinish = value;
      }
    }

    /// <summary>
    /// This property indicates whether a form field design is manadatory.
    /// </summary>
    public bool Mandatory
    {
      get
      {
        return this._Mandatory;
      }
      set
      {
        this._Mandatory = value;
      }
    }

    /// <summary>
    /// This property contains the page layout enumerated value 
    /// </summary>
    public String FieldLayout { get; set; }

    /// <summary>
    /// This property indicates a data point of form field design
    /// </summary>
    public bool AiDataPoint
    {
      get
      {
        return this._AiDataPoint;
      }
      set
      {
        this._AiDataPoint = value;

        //
        // Turn off the data point output if the data cannot be outputte.
        //
        switch ( this.TypeId )
        {
          case Evado.Model.EvDataTypes.Read_Only_Text:
          case Evado.Model.EvDataTypes.Sound:
          case Evado.Model.EvDataTypes.Html_Link:
          case Evado.Model.EvDataTypes.Video:
          case Evado.Model.EvDataTypes.Html_Content:
          case Evado.Model.EvDataTypes.Bar_Chart:
          case Evado.Model.EvDataTypes.Line_Chart:
          case Evado.Model.EvDataTypes.Pie_Chart:
          case Evado.Model.EvDataTypes.Donut_Chart:
          case Evado.Model.EvDataTypes.Stacked_Bar_Chart:
          case Evado.Model.EvDataTypes.Streamed_Video:
          case Evado.Model.EvDataTypes.External_Image:
          case Evado.Model.EvDataTypes.Special_Subsitute_Data:
            {
              this._AiDataPoint = false;
              break;
            }
        }
      }
    }
    /// <summary>
    /// This property indicates whether a form field design is analytics data point.
    /// </summary>
    public bool AnalyticsDataPont
    {
      get
      {
        return this._AnalysticsDataPoint;
      }
      set
      {
        this._AnalysticsDataPoint = value;
      }
    }

    /// <summary>
    /// This property indicates a hide field of form field design
    /// </summary>
    public bool HideField
    {
      get
      {
        return this._HideField;
      }
      set
      {
        this._HideField = value;
      }
    }

    /// <summary>
    /// This property contains the page field width value
    /// Used for text fields.
    /// </summary>
    public int FieldWidth { get; set; }

    /// <summary>
    /// This property contains the page field width value
    /// Used for free text fields.
    /// </summary>
    public int FieldHeight { get; set; }

    /// <summary>
    /// This property indicates a summary field of form field design
    /// </summary>
    public bool IsSummaryField
    {
      get
      {
        return this._SummaryField;
      }
      set
      {
        this._SummaryField = value;
      }
    }

    /// <summary>
    /// This property contains a table object of form field design
    /// </summary>
    public EdRecordTable Table
    {
      get
      {
        return this._Table;
      }
      set
      {
        this._Table = value;
      }
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
    /// This property contains an initial option list of form field design
    /// </summary>
    public string InitialOptionList
    {
      get
      {
        return this._InitialOptionList.Trim ( );
      }
      set
      {
        this._InitialOptionList = value;
      }
    }

    /// <summary>
    /// This property contains an initial version of form field design
    /// </summary>
    public int InitialVersion
    {
      get
      {
        return this._InitialVersion;
      }
      set
      {
        this._InitialVersion = value;
      }
    }

    #endregion

    #region Validation Rules

    private float _ValidationLowerLimit = EvcStatics.CONST_NUMERIC_MINIMUM;
    /// <summary>
    /// This property contains the numeric validation lower limit.
    /// </summary>
    public float ValidationLowerLimit
    {
      get { return this._ValidationLowerLimit; }
      set { this._ValidationLowerLimit = value; }
    }

    private float _ValidationUpperLimit = EvcStatics.CONST_NUMERIC_MAXIMUM;
    /// <summary>
    /// This property contains the numeric validation upper limit
    /// </summary>
    public float ValidationUpperLimit
    {
      get { return this._ValidationUpperLimit; }
      set { this._ValidationUpperLimit = value; }
    }

    private float _AlertLowerLimit = EvcStatics.CONST_NUMERIC_MINIMUM;
    /// <summary>
    /// This property contains the numeric alert lower limit.
    /// </summary>
    public float AlertLowerLimit
    {
      get { return this._AlertLowerLimit; }
      set { this._AlertLowerLimit = value; }
    }

    private float _AlertUpperLimit = EvcStatics.CONST_NUMERIC_MAXIMUM;
    /// <summary>
    /// This property contains the numeric alert upper limit
    /// </summary>
    public float AlertUpperLimit
    {
      get { return this._AlertUpperLimit; }
      set { this._AlertUpperLimit = value; }
    }

    private float _NormalRangeLowerLimit = EvcStatics.CONST_NUMERIC_MINIMUM;
    /// <summary>
    /// This property contains the numeric normal lower limit.
    /// </summary>
    public float NormalRangeLowerLimit
    {
      get { return this._NormalRangeLowerLimit; }
      set { this._NormalRangeLowerLimit = value; }
    }

    private float _NormalRangeUpperLimit = EvcStatics.CONST_NUMERIC_MAXIMUM;
    /// <summary>
    /// This property contains the numeric normal upper limit
    /// </summary>
    public float NormalRangeUpperLimit
    {
      get { return this._NormalRangeUpperLimit; }
      set { this._NormalRangeUpperLimit = value; }
    }

    #endregion

  }//END EvFormFieldDesign class

}//END namespace Evado.Model.Digital
