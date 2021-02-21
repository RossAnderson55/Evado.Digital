/***************************************************************************************
 * <copyright file="Evado.Model.UniForm\Field.cs" company="EVADO HOLDING PTY. LTD.">
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
 *
 ****************************************************************************************/
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Evado.Model.UniForm
{
  /// <summary>
  /// This class contains defines the client page field object contents.
  /// </summary>
  [Serializable]
  public partial class Field
  {
    #region Class initialisation methods

    //  =================================================================================
    /// <summary>
    /// This method initialises the class with values.
    /// </summary>
    //  ---------------------------------------------------------------------------------
    public Field ( )
    {
    }

    //  =================================================================================
    /// <summary>
    /// This method initialises the class with values.
    /// </summary>
    /// <param name="DataId">String: A field data identifier</param>
    /// <param name="Title">String: A field title.</param>
    /// <param name="DataType">EvDataTypes: A field data type object</param>
    //  ---------------------------------------------------------------------------------
    public Field ( String DataId, String Title, Evado.Model.EvDataTypes DataType )
    {
      this._Id = Guid.NewGuid ( );
      this._FieldId = DataId;
      this._Title = Title;
      this._Type = DataType;

      if ( DataType == Evado.Model.EvDataTypes.Table
        || DataType == Evado.Model.EvDataTypes.Special_Matrix )
      {
        this.Table = new Table ( );
      }
    }

    //  =================================================================================
    /// <summary>
    /// This method initialises the class with values.
    /// </summary>
    /// <param name="DataId">String: A field data identifier</param>
    /// <param name="Title">String: A field title.</param>
    /// <param name="DataType">EvDataTypes: A field data type object</param>
    /// <param name="Value">String: A data value</param>
    //  ---------------------------------------------------------------------------------
    public Field ( String DataId, String Title, Evado.Model.EvDataTypes DataType, String Value )
    {
      this._Id = Guid.NewGuid ( );
      this._FieldId = DataId;
      this._Title = Title;
      this._Type = DataType;
      this._Value = Value;
    }


    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class Enumerations

    /// <summary>
    /// This enumeration contains the field target enumeration settings.
    /// </summary>
    public enum FieldTarget
    {
      /// <summary>
      /// This enumeration defines that the html link is to be opened within
      /// the UniFORM client.
      /// </summary>
      Internal = 0,

      /// <summary>
      /// This enumeration defines that the html link is to be opened outside
      /// the UniFORM client.
      /// </summary>
      External = 2
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region class constants
    /// <summary>
    /// This constant defines the field queyr suffix
    /// </summary>
    public const string CONST_FIELD_QUERY_SUFFIX = "_Query";
    /// <summary>
    /// This constant defines the Field annotation suffice
    /// </summary>
    public const string CONST_FIELD_ANNOTATION_SUFFIX = "_FAnnotation";
    #endregion 

    #region Class properties.

    private Guid _Id = Guid.Empty;
    /// <summary>
    ///  This property contains an identifier for the Field object.
    /// 
    /// </summary>
    public Guid Id
    {
      get { return this._Id; }
      set { this._Id = value; }
    }

    private String _FieldId = String.Empty;
    /// <summary>
    ///  This property contains the identifier to cross-reference device data
    ///  with the page object.
    /// </summary>
    [JsonProperty ( "fid" )]
    public String FieldId
    {
      get { return _FieldId; }
      set { _FieldId = value.Trim(); }
    }

    private String _Title = String.Empty;
    /// <summary>
    ///  This property contains the page field objects title.
    /// </summary>
    public String Title
    {
      get { return _Title; }
      set { _Title = value.Trim ( ); }
    }

    /// <summary>
    /// This property contains the group's description stored as a parameter.
    /// </summary>
    [JsonProperty ( "d" )]
    public String Description { get; set; }

    private FieldLayoutCodes _Layout = FieldLayoutCodes.Default;
    /// <summary>
    /// This property contains field jutification property setting.
    /// </summary>
    [JsonProperty ( "ly" )]
    public FieldLayoutCodes Layout
    {
      get { return _Layout; }
      set { _Layout = value; }
    }

    private Evado.Model.EvDataTypes _Type = Evado.Model.EvDataTypes.Null;
    /// <summary>
    /// This property contains page objects data type
    /// </summary>
    [JsonProperty ( "t" )]
    public Evado.Model.EvDataTypes Type
    {
      get { return _Type; }
      set { _Type = value; }
    }

    private bool _Mandatory = false;
    /// <summary>
    /// This property defines whether the field is mandatory or not.
    /// True: the field is mandatory.
    /// </summary>
    [JsonProperty ( "mad" )]
    public bool Mandatory
    {
      get { return _Mandatory; }
      set { _Mandatory = value; }
    }

    /// <summary>
    /// This property defines whether the field is mandatory or not.
    /// True: the field is mandatory.
    /// </summary>
    [JsonIgnore]
    public bool IsEnabled
    {
      get
      {
        if ( this._EditAccess == EditAccess.Disabled )
        {
          return false;
        }

        switch ( this._Type )
        {
          case Model.EvDataTypes.Computed_Field:
          case Model.EvDataTypes.External_Image:
          case Model.EvDataTypes.Html_Content:
          case Model.EvDataTypes.Html_Link:
          case Model.EvDataTypes.Line_Chart:
          case Model.EvDataTypes.Pie_Chart:
          case Model.EvDataTypes.Stacked_Bar_Chart:
          case Model.EvDataTypes.Read_Only_Text:
          case Model.EvDataTypes.Donut_Chart:
          case Model.EvDataTypes.Sound:
          case Model.EvDataTypes.Streamed_Video:
          case Model.EvDataTypes.Video:
            {
              return false;
            }
        }//END switch statment

        return true;

      }
    }

    /// <summary>
    /// This property indicates if the field is empty.
    /// </summary>
    [JsonIgnore]
    public bool isReadOnly
    {
      get
      {
        //
        // select the data type for testing.
        //
        switch ( this.Type )
        {
          case Model.EvDataTypes.External_Image:
          case Model.EvDataTypes.Html_Content:
          case Model.EvDataTypes.Html_Link:
          case Model.EvDataTypes.Line_Chart:
          case Model.EvDataTypes.Pie_Chart:
          case Model.EvDataTypes.Stacked_Bar_Chart:
          case Model.EvDataTypes.Read_Only_Text:
          case Model.EvDataTypes.Donut_Chart:
          case Model.EvDataTypes.Sound:
          case Model.EvDataTypes.Streamed_Video:
          case Model.EvDataTypes.Video:
            {
              return true;
            }
        }//ENd Switch statement

        return false;
      }
    }

    private List<Parameter> _ParameterList = new List<Parameter> ( );

    /// <summary>
    /// This member defines the method parameter that will be used to call  the hosted application.
    /// </summary>
    [JsonProperty ( "prm" )]
    public List<Parameter> Parameters
    {
      get { return _ParameterList; }
      set { _ParameterList = value; }
    }

    private String _Value = String.Empty;
    /// <summary>
    /// This property contains the text value for the date data type. 
    /// </summary>
    [JsonProperty ( "v" )]
    public String Value
    {
      get { return _Value; }
      set { _Value = value.Trim ( ); }
    }

    /// <summary>
    /// This properaty defines a table fields structure and contents
    /// </summary>
    [JsonProperty ( "tbl" )]
    public Table Table { get; set; }

    private EditAccess _EditAccess = EditAccess.Inherited;
    /// <summary>
    /// This property defines whether a field is editable by the user
    /// when displayed in the device client.
    /// </summary>
    [JsonProperty ( "ae" )]
    public EditAccess EditAccess
    {
      get { return _EditAccess; }
      set { _EditAccess = value; }
    }

    /// <summary>
    /// This property defines a selection list that is displayed on the device client.
    /// </summary>
    [JsonProperty ( "opt" )]
    public List<Evado.Model.EvOption> OptionList { get; set; }

    /// <summary>
    /// This property indicates if the field is empty.
    /// </summary>
    [JsonIgnore]
    public bool isEmpty
    {
      get
      {
        bool isEmpty = true;

        //
        // select the data type for testing.
        //
        switch ( this.Type )
        {
          case Model.EvDataTypes.Table:
          case Model.EvDataTypes.Special_Matrix:
            {
              if ( Table != null )
              {
                //
                // itereate through table cells looking for non readonly cells with values.
                //
                foreach ( TableRow row in this.Table.Rows )
                {
                  for ( int i = 0; i < row.Column.Length && i < Table.Header.Length; i++ )
                  {
                    if ( row.Column [ i ] != String.Empty
                      && Table.Header [ i ].TypeId !=  EvDataTypes.Read_Only_Text )
                    {
                      isEmpty = false;
                    }
                  }
                }
              }
              break;
            }
          case EvDataTypes.Radio_Button_List:
            {
              if ( this.Value != "Null"
                && this.Value != String.Empty )
              {
                isEmpty = false;
              }
              break;
            }
          default:
            {
              if ( this.Value != string.Empty )
              {
                isEmpty = false;
              }
              break;
            }

        }
        return isEmpty;
      }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class Methods

    // ==================================================================================
    /// <summary>
    /// This method adds a parameter to the command's parameter list..
    /// </summary>
    /// <param name="Name">String: The name of the parameter.</param>
    /// <param name="Value">String: The value of the parameter.</param>
    //  ---------------------------------------------------------------------------------
    public void AddParameter ( 
      String Name, 
      String Value )
    {
      //
      // get the string value of the parameter list.
      //
      String name = Name.ToString ( );
      name = name.Trim ( );
      String value = Value.ToString ( );

      foreach ( Parameter parameter in this._ParameterList )
      {
        if ( parameter.Name == name )
        {
          parameter.Value = value;

          return;
        }
      }

      this._ParameterList.Add ( new Parameter ( name, value ) );

    }//END AddParameter method

    // ==================================================================================
    /// <summary>
    /// This method adds a parameter to the command's parameter list..
    /// </summary>
    /// <param name="Name">String: The name of the parameter.</param>
    /// <param name="Value">String: The value of the parameter.</param>
    //  ---------------------------------------------------------------------------------
    public void AddParameter ( 
      FieldParameterList Name, 
      object Value )
    {
      //
      // get the string value of the parameter list.
      //
      String name = Name.ToString ( );
      name = name.Trim ( );
      String value = Value.ToString ( );

      foreach ( Parameter parameter in this._ParameterList )
      {
        if ( parameter.Name == name )
        {
          parameter.Value = value;

          return;
        }
      }

      this._ParameterList.Add ( new Parameter ( name, value ) );

    }//END AddParameter method


    // ==================================================================================
    /// <summary>
    /// This method test whether the parameter exists in the field.
    /// </summary>
    /// <param name="Name">String: The name of the parameter.</param>
    //  ---------------------------------------------------------------------------------
    public bool hasParameter ( 
      FieldParameterList Name )
    {
      //
      // get the string value of the parameter list.
      //
      String name = Name.ToString ( );
      name = name.Trim ( );

      //
      // Iterate through the parameters to get the selectev value.
      //
      foreach ( Parameter parameter in this._ParameterList )
      {
        if ( parameter.Name == name )
        {
          return true;
        }
      }

      //
      // Return result
      //
      return false;

    }//END AddParameter method

    // ==================================================================================
    /// <summary>
    /// This method test to see if the parameter is in the list.
    /// </summary>
    /// <param name="Name">String: the name of the parameter.</param>
    /// <returns>True: parameter exists</returns>
    // ---------------------------------------------------------------------------------
    public bool hasParameter ( 
      String Name )
    {
      //
      // get the string value of the parameter list.
      //
      String name = Name.ToString ( );
      name = name.Trim ( );

      //
      // Iterate through the parameters to get the selectev value.
      //
      foreach ( Parameter parameter in this._ParameterList )
      {
        if ( parameter.Name == name )
        {
          return true;
        }
      }

      //
      // Return result
      //
      return false;

    }//END hasParameter method

    // ==================================================================================
    /// <summary>
    /// This method adds a parameter to the command's parameter list..
    /// </summary>
    /// <param name="Name">String: The name of the parameter.</param>
    //  ---------------------------------------------------------------------------------
    public void DeleteParameter ( 
      FieldParameterList Name )
    {
      //
      // get the string value of the parameter list.
      //
      String name = Name.ToString ( );
      name = name.Trim ( );

      for ( int count = 0; count < this._ParameterList.Count; count++ )
      {
        Parameter parameter = this._ParameterList [ count ];

        if ( parameter.Name == name )
        {
          this._ParameterList.RemoveAt ( count );
          count--;
        }
      }
    }

    // ==================================================================================
    /// <summary>
    /// This method adsd a parameter to the command's parameter list..
    /// </summary>
    /// <param name="Name">String: The name of the parameter.</param>
    /// <returns> String value of the header element</returns>
    //  ---------------------------------------------------------------------------------
    public String GetParameter ( 
      FieldParameterList Name )
    {
      //
      // get the string value of the parameter list.
      //
      String name = Name.ToString ( );
      name = name.Trim ( );


      //
      // Iterate through the parameters to get the selectev value.
      //
      foreach ( Parameter parameter in this._ParameterList )
      {
        if ( parameter.Name == name )
        {
          return parameter.Value;
        }
      }

      return string.Empty;

    }//END AddParameter method

    // ==================================================================================
    /// <summary>
    /// This method adsd a parameter to the command's parameter list..
    /// </summary>
    /// <param name="Name">String: The name of the parameter.</param>
    /// <returns> String value of the header element</returns>
    //  ---------------------------------------------------------------------------------
    public String GetParameter ( 
      String Name )
    {
      //
      // get the string value of the parameter list.
      //
      String name = Name.ToString ( );
      name = name.Trim ( );

      //
      // Iterate through the parameters to get the selectev value.
      //
      foreach ( Parameter parameter in this._ParameterList )
      {
        if ( parameter.Name == name )
        {
          return parameter.Value;
        }
      }

      return string.Empty;

    }//END AddParameter method

    // ==================================================================================
    /// <summary>
    /// This method adsd a parameter to the command's parameter list..
    /// </summary>
    /// <param name="Name">String: The name of the parameter.</param>
    /// <returns> String value of the header element</returns>
    //  ---------------------------------------------------------------------------------
    public int GetParameterInt ( 
      FieldParameterList Name )
    {
      //
      // get the string value of the parameter list.
      //
      string value = this.GetParameter ( Name );

      return EvStatics.getInteger( value );

    }//END AddParameter method

    // ==================================================================================
    /// <summary>
    /// This method adsd a parameter to the command's parameter list..
    /// </summary>
    /// <param name="Name">String: The name of the parameter.</param>
    /// <returns> String value of the header element</returns>
    //  ---------------------------------------------------------------------------------
    public float GetParameterflt (
      FieldParameterList Name )
    {
      //
      // get the string value of the parameter list.
      //
      string value = this.GetParameter ( Name );

      return EvStatics.getFloat ( value );

    }//END AddParameter method

    // ==================================================================================
    /// <summary>
    /// This method adsd a parameter to the command's parameter list..
    /// </summary>
    /// <param name="Name">String: The name of the parameter.</param>
    /// <returns> String value of the header element</returns>
    //  ---------------------------------------------------------------------------------
    public DateTime GetParameterDate (
      FieldParameterList Name )
    {
      //
      // get the string value of the parameter list.
      //
      string value = this.GetParameter ( Name );

      return EvStatics.getDateTime ( value );

    }//END AddParameter method

    // ==================================================================================
    /// <summary>
    /// This method sets the Send Command on change parameter
    /// </summary>
    // ---------------------------------------------------------------------------------
    public void setSendCommandOnChange ( )
    {
      this.AddParameter ( FieldParameterList.Snd_Cmd_On_Change, "true" );
    }

    // ==================================================================================
    /// <summary>
    /// This method gets  the Send Command on change parameter
    /// </summary>
    /// <returns>True: Command is set. </returns>
    // ---------------------------------------------------------------------------------
    public bool getSendCommandOnChange ( )
    {
      String value = this.GetParameter ( FieldParameterList.Snd_Cmd_On_Change );

      return EvStatics.getBool ( value );
    }

    // ==================================================================================
    /// <summary>
    /// This method sets the background colour value
    /// </summary>
    /// <param name="Name">FieldParameterList: The name of the parameter.</param>
    /// <param name="Value">Background_Colours: the selected colour's enumerated value.</param>
    //  ---------------------------------------------------------------------------------
    public void setBackgroundColor ( 
      FieldParameterList Name, 
      Background_Colours Value )
    {
      //
      // Exit if the parameter is not a background colour enumeration.
      //
      if ( Name != FieldParameterList.BG_Default
        && Name != FieldParameterList.BG_Mandatory
        && Name != FieldParameterList.BG_Validation
        && Name != FieldParameterList.BG_Alert
        && Name != FieldParameterList.BG_Normal )
      {
        return;
      }
      //
      // get the string value of the parameter list.
      //
      String value = Value.ToString ( );

      this.AddParameter ( Name, value );


    }//END AddParameter method

    // ==================================================================================
    /// <summary>
    /// This method add a parameter to for the background colour
    /// </summary>
    /// <param name="Value">Background_Colours: the selected colour's enumerated value.</param>
    //  ---------------------------------------------------------------------------------
    public void setDefaultBackBroundColor ( 
      Background_Colours Value )
    {
      //
      // Exit if the parameter is not a background colour enumeration.
      //
      String name = FieldParameterList.BG_Default.ToString();

      //
      // get the string value of the parameter list.
      //
      String value = Value.ToString ( );

      this.AddParameter ( name, value );


    }//END AddParameter method

    // ==================================================================================
    /// <summary>
    /// This method add a parameter to for the background colour
    /// </summary>
    /// <param name="Value">Background_Colours: the selected colour's enumerated value.</param>
    //  ---------------------------------------------------------------------------------
    public void setMandatoryBackgroundColor ( 
      Background_Colours Value )
    {
      //
      // Exit if the parameter is not a background colour enumeration.
      //
      String name = FieldParameterList.BG_Mandatory.ToString ( );

      //
      // get the string value of the parameter list.
      //
      String value = Value.ToString ( );

      this.AddParameter ( name, value );


    }//END AddParameter method

    // ==================================================================================
    /// <summary>
    /// This method add a parameter to for the background colour
    /// </summary>
    /// <param name="Name">FieldParameterList: The name of the parameter.</param>
    //  ---------------------------------------------------------------------------------
    public Background_Colours GetBackgroundColor ( 
      FieldParameterList Name )
    {
      //
      // Exit if the parameter is not a background colour enumeration.
      //
      if ( Name != FieldParameterList.BG_Default
        && Name != FieldParameterList.BG_Mandatory
        && Name != FieldParameterList.BG_Validation
        && Name != FieldParameterList.BG_Alert
        && Name != FieldParameterList.BG_Normal )
      {
        return Background_Colours.Default;
      }

      //
      // get the string value of the parameter list.
      //
      String value = this.GetParameter( Name );
      Background_Colours colour = Background_Colours.Default;

      //
      //Iterate through the list paramater to determine of the parameter already exists and update it.
      //
      foreach ( Parameter parameter in this._ParameterList )
      {
        //
        //If parameter Name is equal to GroupParameterList Name, return
        //
        if (value != String.Empty )
        {
          colour = EvStatics.parseEnumValue<Background_Colours> ( parameter.Value );

          return colour;
        }
      }//END parameter iteration

      //
      // return the found colour
      //
      return colour;

    }//END AddParameter method

    // ==================================================================================
    /// <summary>
    /// This method add a parameter to for the background colour
    /// </summary>
    /// <returns>Background_Colours emunerated value</returns>
    //  ---------------------------------------------------------------------------------
    public Background_Colours getDefaultBackgroundColor ( )
    {
      //
      // get the string value of the parameter list.
      //
      String value = this.GetParameter ( FieldParameterList.BG_Default );
      Background_Colours colour = Background_Colours.Default;

      //
      //if the value exists reset the colour
      //
      if ( value != String.Empty )
      {
        if ( EvStatics.tryParseEnumValue<Background_Colours> ( value, out colour ) == true )
        {
          return colour;
        }
      }

      //
      // return the found colour
      //
      return Background_Colours.Default;

    }//END AddParameter method

    // ==================================================================================
    /// <summary>
    /// This method add a parameter to for the background colour
    /// </summary>
    /// <returns>Background_Colours emunerated value</returns>
    //  ---------------------------------------------------------------------------------
    public Background_Colours getMandatoryBackGroundColor (Background_Colours CurentColor )
    {
      //
      // get the string value of the parameter list.
      //
      String value = this.GetParameter ( FieldParameterList.BG_Mandatory );
      Background_Colours colour = Background_Colours.Red;

      //
      // If the field has a value it should have the default background.
      //
      if ( this._Value != String.Empty )
      {
        return CurentColor;
      }

      //
      //if the value exists reset the colour
      //
      if ( value != String.Empty )
      {
        if ( EvStatics.tryParseEnumValue<Background_Colours> ( value, out colour ) == true )
        {
          return colour;
        }
      }

      //
      // return the found colour
      //
      return Background_Colours.Red;

    }//END AddParameter method

    // ==================================================================================
    /// <summary>
    /// This method add a parameter to the command's parameter list..
    /// </summary>
    /// <param name="Value">FieldValueWidth: the value of the parameter.</param>
    //  ---------------------------------------------------------------------------------
    public void SetValueColumnWidth (
      FieldValueWidths Value )
    {
      //
      // get the string value of the parameter list.
      //
      String value = Value.ToString ( );

      //
      // Add the parameter to the parameter list.
      //
      this.AddParameter ( FieldParameterList.Field_Value_Column_Width, value );

    }//END addFieldValueWidth method

    // ==================================================================================
    /// <summary>
    /// This method get the value column width if set.
    /// </summary>
    //  ---------------------------------------------------------------------------------
    public FieldValueWidths getValueColumnWidth ( )
    {
      //
      // get the string value of the parameter list.
      //
      String value = this.GetParameter ( FieldParameterList.Field_Value_Column_Width );

      if ( value != String.Empty )
      {
        return EvStatics.parseEnumValue<FieldValueWidths> ( value );
      }

      return FieldValueWidths.Default;

    }//END getValueColumnWidth method

    // ==================================================================================
    /// <summary>
    /// This method fixes the numeric validation parameters.
    /// </summary>
    //  ---------------------------------------------------------------------------------
    public void FixNumericValidation ( )
    {
      //
      // Search the parmeters for existing parameters.
      // and exit if update the value.
      // 
      for ( int count = 0; count < this._ParameterList.Count; count++ )
      {
        Parameter parameter = this._ParameterList [ count ];
        if ( parameter.Name == FieldParameterList.Min_Value.ToString ( ) )
        {
          this.AddParameter ( "MinValue", parameter.Value );
        }
        if ( parameter.Name == FieldParameterList.Max_Value.ToString ( ) )
        {
          this.AddParameter ( "MaxValue", parameter.Value );
        }
        if ( parameter.Name == FieldParameterList.Min_Alert.ToString ( ) )
        {
          this.AddParameter ( "MinAlert", parameter.Value );
        }
        if ( parameter.Name == FieldParameterList.Max_Alert.ToString ( ) )
        {
          this.AddParameter ( "MaxAlert", parameter.Value );
        }
        if ( parameter.Name == FieldParameterList.Min_Normal.ToString ( ) )
        {
          this.AddParameter ( "MinNormal", parameter.Value );
        }
        if ( parameter.Name == FieldParameterList.Max_Normal.ToString ( ) )
        {
          this.AddParameter ( "MaxNormal", parameter.Value );
        }
      }

    }//END AddParameter method

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class Static Methods
    //  =================================================================================
    /// <summary>
    /// This method gets a fields edit status based on the edit status hierarchy.
    /// </summary>
    /// <param name="PageStatus">EditsCodes: Contains the page's edit status</param>
    /// <param name="GroupStatus">EditsCodes: Contains the group's edit status</param>
    /// <param name="FieldStatus">EditsCodes: Contains the field's edit status</param>
    /// <returns>EditCodes: Object EditCodes</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Initialise the method variables. 
    /// 
    /// 2. Set the group status if it is inherited. 
    /// 
    /// 3. Set the field status if it is inherited
    /// 
    /// 4. Return EditCodes object
    /// 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public static EditAccess getEditStatus (
      EditAccess PageStatus,
      EditAccess GroupStatus,
      EditAccess FieldStatus )
    {
      //
      // Initialise the methods varibles.
      //
      EditAccess status = FieldStatus;

      //
      // Set the group status if it is inherited.
      //
      if ( GroupStatus == EditAccess.Inherited )
      {
        GroupStatus = PageStatus;
      }

      //
      // Set the field status if it is inherited.
      //
      if ( FieldStatus == EditAccess.Inherited )
      {
        status = GroupStatus;
      }

      //
      // Return EditCodes object
      //
      return status;
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

  }//END CLASS

}//END namespace