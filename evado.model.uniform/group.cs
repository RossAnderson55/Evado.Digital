/***************************************************************************************
 * <copyright file="Evado.Model.UniForm\Group.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the AbstractedPage data object.
 *
 ****************************************************************************************/
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Evado.Model.UniForm
{
  /// <summary>
  /// This class defines the page client structure.
  /// </summary>
  [Serializable]
  public partial class Group
  {
    #region Class Initialisation methods

    //   ================================================================================
    /// <summary>
    /// The initialisation method
    /// </summary>
    //  ---------------------------------------------------------------------------------
    public Group ( )
    {

    }//END Group method

    //   ================================================================================
    /// <summary>
    /// This method initialises class with parameters and value
    /// </summary>
    /// <param name="Title">String: Title of the group. </param>
    //  ---------------------------------------------------------------------------------
    public Group (
      String Title )
    {
      this.Id = Guid.NewGuid ( );
      this._Title = Title;

    }//END Group method

    //   ================================================================================
    /// <summary>
    /// This method initialises class with parameters and value
    /// </summary>
    /// <param name="Title">String: Title of the group. </param>
    /// <param name="Description">String: Group description value.</param>
    /// <param name="Status">EditCodes: The group edit status.</param>
    //  ---------------------------------------------------------------------------------
    public Group (
      String Title,
      String Description,
      EditAccess Status )
    {
      this.Id = Guid.NewGuid ( );
      this._Title = Title;
      this.Description = Description ;
      this.EditAccess = Status;

    }//END Group method

    //   ================================================================================
    /// <summary>
    /// This method initialises class with parameters and value.
    /// </summary>
    /// <param name="Title"> String: Title of the group. </param>
    /// <param name="Status"> EditCodes: The group edit status.</param>
    //  ---------------------------------------------------------------------------------
    public Group (
      String Title,
      EditAccess Status )
    {
      this.Id = Guid.NewGuid ( );
      this._Title = Title;
      this.EditAccess = Status;

    }//END Group method

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class enumerated lists

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Property members

    private Guid _Id = Guid.Empty;
    /// <summary>
    ///  This property contains an identifier for the group object.
    /// </summary>
    public Guid Id
    {
      get { return this._Id; }
      set { this._Id = value; }
    }

    /// <summary>
    /// This property contains the group identifier used by the adapter to identify different groups.
    /// </summary>
    [JsonIgnore]
    public String GroupId { get; set; }

    private String _Title = String.Empty;
    /// <summary>
    /// This Property contains the title of the group.
    /// </summary>
    public String Title
    {
      get { return _Title; }
      set { _Title = value; }
    }

    ///
    /// This property contains the group's description stored as a parameter.
    ///
    //[JsonIgnore]
    [JsonProperty ( "d" )]
    public String Description {get; set; }

    GroupDescriptionAlignments _DescriptionAlignment = GroupDescriptionAlignments.Left_Align;
    //===================================================================================
    /// <summary>
    /// This method set the description alignment parmaeter.
    /// </summary>
    //-----------------------------------------------------------------------------------
    [JsonProperty ( "da" )]
    public GroupDescriptionAlignments DescriptionAlignment
    {
      get
      {
        return _DescriptionAlignment;
      }
      set
      {
        _DescriptionAlignment = value;
      }
    }

    private GroupLayouts _Layout = GroupLayouts.Dynamic;
    /// <summary>
    /// This propery contains the GroupLayout enumerated value defining the group layout setting.
    /// </summary>
    [JsonProperty ( "ly" )]
    public GroupLayouts Layout
    {
      get
      {
        return _Layout;
      }
      set
      {
        _Layout = value;
      }
    }

    GroupTypes _GroupType = GroupTypes.Default;

    //  =================================================================================
    /// <summary>
    /// This property contains definition that how a page layout can be customized.
    /// </summary>
    //  ---------------------------------------------------------------------------------
    [JsonProperty ( "gt" )]
    public GroupTypes GroupType
    {
      get
      {
        return _GroupType;
      }
      set
      {
        this._GroupType = value;
      }
    }



    GroupCommandListLayouts _CommandLayout = GroupCommandListLayouts.Horizontal_Orientation;
    /// <summary>
    /// This property contains the page's command orientation.
    /// Normally lists of objects should be vertical and page commands horizontally.
    /// </summary>
    [JsonProperty ( "cl" )]
    public GroupCommandListLayouts CmdLayout
    {
      get
      {
        return _CommandLayout;
      }
      set
      {
        this._CommandLayout = value;
      }
    }

    private EditAccess _EditAccess = EditAccess.Inherited;
    /// <summary>
    /// This property contains a definition whether a group's fields are editable by the user
    /// when it is displayed in the device client.
    /// </summary>
    [JsonProperty ( "ea" )]
    public EditAccess EditAccess
    {
      get { return _EditAccess; }
      set { _EditAccess = value; }
    }

    private List<Field> _FieldList = new List<Field> ( );

    /// <summary>
    /// This property contains a list Field objec.
    /// </summary>
    [JsonProperty ( "fl" )]
    public List<Field> FieldList
    {
      get { return this._FieldList; }
      set { this._FieldList = value; }
    }

    private List<Parameter> _ParameterList = new List<Parameter> ( );
    /// <summary>
    /// This property contains a list of Parameter object.
    /// </summary>
    [JsonProperty ( "prm" )]
    public List<Parameter> Parameters
    {
      get
      {
        return _ParameterList;

      }//End get statement.

      set
      {
        _ParameterList = value;

      }//END set statement

    }//END property.

    private List<Command> _CommandList = new List<Command> ( );

    /// <summary>
    /// This property contains a list of Command object.
    /// </summary>
    [JsonProperty ( "cmd" )]
    public List<Command> CommandList
    {
      get { return this._CommandList; }
      set { this._CommandList = value; }
    }


    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class Methods

    // ==================================================================================
    /// <summary>
    /// This method sets the group status to the page statu if is set to inherited.
    /// </summary>
    /// <param name="PageStatus">FieldValueWidth: the value of the parameter.</param>
    //  ---------------------------------------------------------------------------------
    public void setGroupStatus ( Model.UniForm.EditAccess PageStatus )
    {
      if ( PageStatus == Model.UniForm.EditAccess.Enabled
        && this._EditAccess == EditAccess.Inherited )
      {
        this._EditAccess = PageStatus;
      }
    }//END setGroupStatus method

    // ==================================================================================
    /// <summary>
    /// This method adds a new field to the group.
    /// </summary>
    /// <param name="PageField">Field: new field object.</param>
    /// <remarks>
    /// This method consists of following steps.
    /// 
    /// 1. Add PageFiled to the _FieldList list.
    /// 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public void addField ( Field PageField )
    {
      this._FieldList.Add ( PageField );
    }

    // ==================================================================================
    /// <summary>
    /// This method adds a new field to the group.
    /// </summary>
    /// <param name="FieldId">field data identifier</param>
    /// <param name="Title">Field title.</param>
    /// <param name="DataType">Field data type</param>
    /// <param name="Value">String data value</param>
    /// <returns>UniForm.Field object.</returns>
    /// <remarks>
    /// This method consisits of following steps.
    /// 
    /// 1. Define the new field.
    /// 
    /// 2. Add the field to the field list.
    /// 
    /// 3. Return the filed object.
    /// 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public Field addField (
      object FieldId,
      String Title,
      EvDataTypes DataType,
      String Value )
    {
      //
      // define the new field.
      //
      Field field = new Field (
        FieldId.ToString ( ),
        Title,
        DataType,
        Value );

      //
      // Add the field to the field list.
      //
      this._FieldList.Add ( field );

      //
      // Return the field object.
      //
      return field;
    }

    // ==================================================================================
    /// <summary>
    /// This method adds a new command to the group
    /// </summary>
    /// <param name="PageCommand">Command: new page command.</param>
    /// <remarks>
    /// This method consists of following steps.
    /// 1. Add PageCommand to _CommandList.
    /// 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public void addCommand ( Command PageCommand )
    {
      this._CommandList.Add ( PageCommand );
    }

    // ==================================================================================
    /// <summary>
    /// This method adds a new command to the group
    /// </summary>
    /// <param name="Title">String: command title</param>
    /// <param name="ApplicationId">String: application identifier</param>
    /// <param name="ApplicationObject">String: Application object identifier</param>
    /// <param name="ApplicationMethod">ApplicationMethods: method enumerated value</param>
    /// <returns>UniForm.Field object.</returns>
    /// <remarks>
    /// This method consists of following steps.
    /// 
    /// 1. Initialise the command object. 
    /// 
    /// 2. Add the command to the command list.
    /// 
    /// 3. Return the Command object. 
    /// 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public Command addCommand (
      String Title,
      String ApplicationId,
      String ApplicationObject,
      ApplicationMethods ApplicationMethod )
    {
      //
      // Initialise the command object.
      //
      Command command = new Command ( Title, ApplicationId, ApplicationObject, ApplicationMethod );

      //
      // Add the comment to the command list.
      //
      this._CommandList.Add ( command );

      // 
      // Return the command object.
      //
      return command;
    }

    // ==================================================================================
    /// <summary>
    /// This method adds a new command to the group
    /// </summary>
    /// <param name="Title">String: command title</param>
    /// <param name="ApplicationId">String: application identifier</param>
    /// <param name="ApplicationObject">String: Application object identifier</param>
    /// <param name="ApplicationMethod">ApplicationMethods: method enumerated value</param>
    /// <returns>UniForm.Field object.</returns>
    /// <remarks>
    /// This method consists of following steps.
    /// 
    /// 1. Initialise the command object. 
    /// 
    /// 2. Add the command to the command list.
    /// 
    /// 3. Return the Command object. 
    /// 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public Command addCommand (
      String Title,
      String ApplicationId,
      object ApplicationObject,
      ApplicationMethods ApplicationMethod )
    {
      //
      // Initialise the command object.
      //
      Command command = new Command (
        Title,
        ApplicationId,
        ApplicationObject.ToString ( ),
        ApplicationMethod );

      //
      // Add the comment to the command list.
      //
      this._CommandList.Add ( command );

      // 
      // Return the command object.
      //
      return command;
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class Parameter Methods

    // ==================================================================================
    /// <summary>
    /// This method add a parameter to the command's parameter list..
    /// </summary>
    /// <param name="Name">GroupParameterList: The name of the parameter.</param>
    /// <param name="Value">int: the value of the parameter.</param>
    //  ---------------------------------------------------------------------------------
    public void AddParameter (
      GroupParameterList Name,
      object Value )
    {
      //
      // Exit for parameters that cannot be added with this parameter.
      //
      if ( Name == GroupParameterList.Field_Value_Column_Width
        || Name == GroupParameterList.Command_Height
        || Name == GroupParameterList.Page_Column
        || Name == GroupParameterList.BG_Default
        || Name == GroupParameterList.BG_Alternative
        || Name == GroupParameterList.BG_Highlighted
        || Name == GroupParameterList.BG_Mandatory
        || Name == GroupParameterList.BG_Validation
        || Name == GroupParameterList.BG_Alert
        || Name == GroupParameterList.BG_Normal )
      {
        return;
      }

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
    /// This method add a parameter to the command's parameter list..
    /// </summary>
    /// <param name="Name">GroupParameterList: The name of the parameter.</param>
    /// <param name="Value">bool: the value of the parameter.</param>
    //  ---------------------------------------------------------------------------------
    public void AddParameter (
      GroupParameterList Name,
      bool Value )
    {
      //
      // Exit for parameters that cannot be added with this parameter.
      //
      if ( Name == GroupParameterList.Field_Value_Column_Width
        || Name == GroupParameterList.Command_Height
        || Name == GroupParameterList.Page_Column
        || Name == GroupParameterList.BG_Default
        || Name == GroupParameterList.BG_Alternative
        || Name == GroupParameterList.BG_Highlighted
        || Name == GroupParameterList.BG_Mandatory
        || Name == GroupParameterList.BG_Validation
        || Name == GroupParameterList.BG_Alert
        || Name == GroupParameterList.BG_Normal )
      {
        return;
      }

      //
      // get the string value of the parameter list.
      //
      String name = Name.ToString ( );
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
    /// This method adsd a parameter to the command's parameter list..
    /// </summary>
    /// <param name="Name">String: The name of the parameter.</param>
    /// <returns> String value of the header element</returns>
    //  ---------------------------------------------------------------------------------
    public int GetParameterInt ( GroupParameterList Name )
    {
      String value = this.GetParameter ( Name );

      return EvStatics.getInteger ( value );

    }//END AddParameter method

    // ==================================================================================
    /// <summary>
    /// This method add a parameter to the command's parameter list..
    /// </summary>
    /// <param name="Name">GroupParameterList: the name of the parameter.</param>
    /// <returns >bool value</returns>
    //  ---------------------------------------------------------------------------------
    public bool hasParameter ( GroupParameterList Name )
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
    }

    // ==================================================================================
    /// <summary>
    /// This method adds a parameter to the command's parameter list..
    /// </summary>
    /// <param name="Name">String: The name of the parameter.</param>
    //  ---------------------------------------------------------------------------------
    public void DeleteParameter ( GroupParameterList Name )
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
    public String GetParameter ( GroupParameterList Name )
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
    /// This method add a parameter to the command's parameter list.
    /// </summary>
    /// <param name="Name">GroupParameterList: the name of the parameter.</param>
    //  ---------------------------------------------------------------------------------
    public void deleteGroupParameter ( GroupParameterList Name )
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

    }//END deleteParameter method 

    // ==================================================================================
    /// <summary>
    /// This method add a parameter to the command's parameter list..
    /// </summary>
    /// <param name="Name">GroupParameterList: The name of the parameter.</param>
    /// <param name="Value">int: the value of the parameter.</param>
    //  ---------------------------------------------------------------------------------
    private void SetParameter (
      GroupParameterList Name,
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
    /// This method add a parameter to the command's parameter list..
    /// </summary>
    /// <param name="Value">FieldValueWidth: the value of the parameter.</param>
    //  ---------------------------------------------------------------------------------
    public void SetPageColumnCode (
      PageColumnCodes Value )
    {
      //
      // get the string value of the parameter list.
      //
      String value = Value.ToString ( );

      //
      // Add the parameter to the parameter list.
      //
      this.SetParameter ( GroupParameterList.Page_Column, value );

    }//END addFieldValueWidth method

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
      this.AddParameter ( GroupParameterList.Field_Value_Column_Width, value );

    }//END addFieldValueWidth method

    // ==================================================================================
    /// <summary>
    /// This method add a parameter to the command's parameter list..
    /// </summary>
    /// <param name="Value">percentage value between 0 and 100</param>
    //  ---------------------------------------------------------------------------------
    public void SetCommandWidth ( double Value )
    {
      if ( Value < 0 )
      {
        return;
      }
      if ( Value > 100 )
      {
        Value = Value / 10;
      }
      if ( Value > 100 )
      {
        Value = Value / 10;
      }
      //
      // get the string value of the parameter list.
      //
      String value = Value.ToString ( );

      //
      // Add the parameter to the parameter list.
      //
      this.SetParameter ( GroupParameterList.Command_Width, value );

    }//END addFieldValueWidth method

    // ==================================================================================
    /// <summary>
    /// This method add a parameter to the command's parameter list..
    /// </summary>
    //  ---------------------------------------------------------------------------------
    public double GetCommandWidth ( )
    {
      double width = -1;
      //
      // Add the parameter to the parameter list.
      //
      string value = this.GetParameter ( GroupParameterList.Command_Width );
      value = value.Replace ( "%", "" );

      if ( value == string.Empty )
      {
        return -1;
      }

      if ( double.TryParse ( value, out width ) == false )
      {
        return -1;
      }
      return width;

    }//END addFieldValueWidth method

    // ==================================================================================
    /// <summary>
    /// This method add a parameter to the command's parameter list..
    /// </summary>
    /// <param name="Value">pixel value between 0 and 1000</param>
    //  ---------------------------------------------------------------------------------
    public void SetCommandHeight ( double Value )
    {
      if ( Value < 0 )
      {
        return;
      }
      if ( Value > 1000 )
      {
        Value = 1000;
      }
      //
      // get the string value of the parameter list.
      //
      String value = Value.ToString ( );

      //
      // Add the parameter to the parameter list.
      //
      this.SetParameter ( GroupParameterList.Command_Height, value );

    }//END addFieldValueWidth method

    // ==================================================================================
    /// <summary>
    /// This method get the value column width if set.
    /// </summary>
    /// <returns>FieldValueWidths enumeration</returns>
    //  ---------------------------------------------------------------------------------
    public FieldValueWidths getValueColumnWidth ( )
    {
      //
      // get the string value of the parameter list.
      //
      String value = this.GetParameter ( GroupParameterList.Field_Value_Column_Width );

      if ( value != String.Empty )
      {
        return EvStatics.parseEnumValue<FieldValueWidths> ( value );
      }

      return FieldValueWidths.Default;

    }//END getValueColumnWidth method

    // ==================================================================================
    /// <summary>
    /// This method add a parameter to the command's parameter list..
    /// </summary>
    //  ---------------------------------------------------------------------------------
    public double GetCommandHeight ( )
    {
      double height = -1;
      //
      // Add the parameter to the parameter list.
      //
      string value = this.GetParameter ( GroupParameterList.Command_Height );

      if ( value == string.Empty )
      {
        return -1;
      }

      if ( double.TryParse ( value, out height ) == false )
      {
        return -1;
      }
      return height;

    }//END addFieldValueWidth method


    // ==================================================================================
    /// <summary>
    /// This method add a parameter to for the background colour for commands
    /// </summary>
    /// <param name="Name">GroupParameterList: The name of the parameter.</param>
    /// <param name="Colour">Background_Colours: the selected colour's enumerated value.</param>
    /// <remarks>
    /// This method consists of following steps.
    /// 
    /// 1. Iterate through the list paramater to determine of the parameter already exists and update it.
    /// 
    /// 2. If parameter Name is equal to GroupParameterList name, return
    /// 
    /// 3. Add a new parameter to the list.
    /// 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public void SetCommandBackBroundColor (
      GroupParameterList Name,
      Background_Colours Colour )
    {
      //
      // Exit if the parameter is not a background colour enumeration.
      //
      if ( Name != GroupParameterList.BG_Default
        && Name != GroupParameterList.BG_Alternative
        && Name != GroupParameterList.BG_Highlighted )
      {
        return;
      }

      //
      // get the string value of the parameter list.
      //
      String value = Colour.ToString ( );

      //
      // Add the parameter to the parameter list.
      //
      this.SetParameter ( Name, value );

    }//END AddCommandBackBroundColor method

    // ==================================================================================
    /// <summary>
    /// This method add a parameter to for the background colour for fields
    /// </summary>
    /// <param name="Name">GroupParameterList: The name of the parameter.</param>
    /// <param name="Colour">Background_Colours: the selected colour's enumerated value.</param>
    /// <remarks>
    /// This method consists of following steps.
    /// 
    /// 1. Iterate through the list paramater to determine of the parameter already exists and update it.
    /// 
    /// 2. If parameter Name is equal to GroupParameterList name, return
    /// 
    /// 3. Add a new parameter to the list.
    /// 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public void SetFieldBackBroundColor (
      GroupParameterList Name,
      Background_Colours Colour )
    {
      //
      // Exit if the parameter is not a background colour enumeration.
      //
      if ( Name != GroupParameterList.BG_Default
        && Name != GroupParameterList.BG_Mandatory
        && Name != GroupParameterList.BG_Validation
        && Name != GroupParameterList.BG_Alert
        && Name != GroupParameterList.BG_Normal )
      {
        return;
      }

      //
      // get the string value of the parameter list.
      //
      String value = Colour.ToString ( );

      //
      // Add the parameter to the parameter list.
      //
      this.SetParameter ( Name, value );

    }//END SetFieldBackBroundColor method


    // ==================================================================================
    /// <summary>
    /// This method gets the background colour selection.
    /// </summary>
    /// <param name="Name">String: the name of the parameter.</param>
    /// <returns >String value</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Iterate through prm list of _Parameters.
    /// 
    /// 2. If prm Name is equal to GroupParameterList Name, return prm Value.
    /// 
    /// 3. Return an empty string  
    /// </remarks>

    //  ---------------------------------------------------------------------------------
    public string GetCssBackBroundColor ( GroupParameterList Name )
    {
      //
      // Exit if the parameter is not a background colour enumeration.
      //
      if ( Name != GroupParameterList.BG_Default
        && Name != GroupParameterList.BG_Alternative
        && Name != GroupParameterList.BG_Highlighted )
      {
        return String.Empty;
      }

      //
      // get the string value of the parameter list.
      //
      Background_Colours BgColour = Background_Colours.White;
      String CssColour = String.Empty;

      //
      // Get the parameter value as a string.
      //
      String value = this.GetParameter ( Name );

      try
      {
        BgColour = EvStatics.parseEnumValue<Background_Colours> ( value );
        CssColour = BgColour.ToString ( );
        CssColour = CssColour.Replace ( "BG_", String.Empty );
        CssColour = CssColour.ToLower ( );
      }
      catch { return CssColour; }


      //
      // Return an empty string 
      //
      return CssColour;

    }//END GetParameter method

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region class create methods

    // ==================================================================================
    /// <summary>
    /// THis method creates a text client page field object
    /// </summary>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps.
    /// 
    /// 1. Add the field to the group list.
    /// 
    /// 2. Return the field object.
    /// 
    /// </remarks>
    ///
    // ----------------------------------------------------------------------------------
    public Field createField ( )
    {
      Field pageField = new Field ( );

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Text;
      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END CreateFieldMethod

    // ==================================================================================
    /// <summary>
    /// This method creates a text client page field object
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="Value">String: text content</param>
    /// <param name="Size">Int: lenght of the field in characters</param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Add the field to the group list.
    /// 
    /// 2. Return the field object.
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createTextField (
      object FieldId,
      String FieldTitle,
      String Value,
      int Size )
    {
      if ( Value == null )
      {
        Value = String.Empty;
      }
      Field pageField = new Field ( );
      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Text;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      pageField.Value = Value;
      pageField.AddParameter ( FieldParameterList.Width, Size.ToString ( ) );
      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createTextField method 

    // ==================================================================================
    /// <summary>
    /// This method creates a text client page field object
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="FieldDescription">String: field description </param>
    /// <param name="Value">String: text content</param>
    /// <param name="Size">Int: lenght of the field in characters</param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Add the field to the group list.
    /// 
    /// 2. Return the field object.
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createTextField (
      object FieldId,
      String FieldTitle,
      String FieldDescription,
      String Value,
      int Size )
    {
      if ( Value == null )
      {
        Value = String.Empty;
      }
      Field pageField = new Field ( );
      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Text;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      pageField.Value = Value;
      if ( FieldDescription != String.Empty )
      {
        pageField.Description = FieldDescription ;
      }
      pageField.AddParameter ( FieldParameterList.Width, Size.ToString ( ) );
      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createTextField method 

    // ==================================================================================
    /// <summary>
    /// This method creates a text client page field object
    /// </summary>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="Value">String: text content</param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps.
    /// 
    /// 1. Add the field to the group list.
    /// 
    /// 2. Return the field object.
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createReadOnlyTextField (
      String FieldTitle,
      String Value )
    {
      if ( Value == null )
      {
        Value = String.Empty;
      }
      Field pageField = new Field ( );
      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Read_Only_Text;
      pageField.FieldId = String.Empty;
      pageField.Title = FieldTitle;
      pageField.Value = Value;
      pageField.EditAccess = EditAccess.Disabled;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createReadOnlyTextField method 

    // ==================================================================================
    /// <summary>
    /// This method creates a text client page field object
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="Value">String: text content</param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps.
    /// 
    /// 1. Add the field to the group list.
    /// 
    /// 2. Return the field object.
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createReadOnlyTextField (
      object FieldId,
      String FieldTitle,
      String Value )
    {
      if ( Value == null )
      {
        Value = String.Empty;
      }
      Field pageField = new Field ( );
      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Read_Only_Text;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      pageField.Value = Value;
      pageField.EditAccess = EditAccess.Disabled;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createReadOnlyTextField method 

    // ==================================================================================
    /// <summary>
    /// This method creates a text client page field object
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="FieldDescription">String: field description </param>
    /// <param name="Value">String: text content</param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Add the field to the group list.
    /// 
    /// 2. Return the field object. 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createReadOnlyTextField (
      object FieldId,
      String FieldTitle,
      String FieldDescription,
      String Value )
    {
      if ( Value == null )
      {
        Value = String.Empty;
      }
      Field pageField = new Field ( );
      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Read_Only_Text;
      pageField.EditAccess = EditAccess.Disabled;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      if ( FieldDescription != String.Empty )
      {
        pageField.Description = FieldDescription ;
      }
      pageField.Value = Value;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createReadOnlyTextField method 

    // ==================================================================================
    /// <summary>
    /// THis method creates a text client page field object
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="HtmlValue">String: text content</param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consits of following steps.
    /// 
    /// 1. Add the field to the group list.
    /// 
    /// 2. Return the field object. 
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createHtmlLinkField (
      object FieldId,
      String FieldTitle,
      String HtmlValue )
    {
      if ( HtmlValue == null )
      {
        HtmlValue = String.Empty;
      }
      Field pageField = new Field ( );
      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Html_Link;
      pageField.EditAccess = EditAccess.Disabled;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      pageField.Value = HtmlValue;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createHtmlField method 

    // ==================================================================================
    /// <summary>
    /// This method creates a free text client page field object
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="Value">String: text content</param>
    /// <param name="Size">Int: length of the field in characters</param>
    /// <param name="Rows">Int: height of the field in characters</param>
    /// <param name="FieldDescription">String: Description of Field </param>
    /// <returns>Field object</returns>
    ///<remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Add the field to the group list. 
    /// 
    /// 2. Return the field object.  
    /// 
    /// </remarks> 
    // ----------------------------------------------------------------------------------
    public Field createFreeTextField (
      object FieldId,
      String FieldTitle,
      String FieldDescription,
      String Value,
      int Size,
      int Rows )
    {
      if ( Value == null )
      {
        Value = String.Empty;
      }
      Field pageField = new Field ( );
      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Free_Text;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      if ( FieldDescription != String.Empty )
      {
        pageField.Description = FieldDescription ;
      }
      pageField.Value = Value;
      pageField.AddParameter ( FieldParameterList.Width, Size.ToString ( ) );
      pageField.AddParameter ( FieldParameterList.Height, Rows.ToString ( ) );
      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createFreeTextField method  

    // ==================================================================================
    /// <summary>
    /// This method creates a free text client page field object
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="Value">String: text content</param>
    /// <param name="Size">Int: length of the field in characters</param>
    /// <param name="Rows">Int: height of the field in characters</param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps.
    /// 
    /// 1. Add the field to the group list. 
    /// 
    /// 2. Return the field object.   
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createFreeTextField (
      object FieldId,
      String FieldTitle,
      String Value,
      int Size,
      int Rows )
    {
      if ( Value == null )
      {
        Value = String.Empty;
      }
      Field pageField = new Field ( );
      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Free_Text;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      pageField.Value = Value;
      pageField.AddParameter ( FieldParameterList.Width, Size.ToString ( ) );
      pageField.AddParameter ( FieldParameterList.Height, Rows.ToString ( ) );
      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createFreeTextField method 

    // ==================================================================================
    /// <summary>
    /// This method creates a boolean client page field object
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="Value">bool: field state</param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Add the field to the group list. 
    /// 
    /// 2. Return the field object. 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createBooleanField (
      object FieldId,
      String FieldTitle,
      bool Value )
    {
      Field pageField = new Field ( );
      String stTextValue = "Yes";

      //
      // If State is false set sttextValue to No.
      //
      if ( Value == false )
      {
        stTextValue = "No";
      }

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Boolean;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      pageField.Value = stTextValue;

      pageField.OptionList = new List<EvOption> ( );
      pageField.OptionList.Add ( new EvOption ( "Yes" ) );
      pageField.OptionList.Add ( new EvOption ( "No" ) );
      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createBooleanField method 

    // ==================================================================================
    /// <summary>
    /// This method creates a boolean client page field object
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="FieldDescription">String: field description </param>
    /// <param name="Value">bool: field state</param>
    /// <returns>Field object</returns>
    /// <remarks> 
    /// This method consists of following steps. 
    /// 
    /// 1. Add the field to the group list.
    /// 
    /// 2. Return the field to the group list. 
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createBooleanField (
      object FieldId,
      String FieldTitle,
      String FieldDescription,
      bool Value )
    {
      Field pageField = new Field ( );
      String stTextValue = "Yes";

      //
      // if State is false set stTextValue No.
      //
      if ( Value == false )
      {
        stTextValue = "No";
      }

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Boolean;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      if ( FieldDescription != String.Empty )
      {
        pageField.Description = FieldDescription ;
      }
      pageField.Value = stTextValue;
      pageField.AddParameter ( FieldParameterList.Width, "12" );

      pageField.OptionList = new List<EvOption> ( );
      pageField.OptionList.Add ( new EvOption ( "Yes" ) );
      pageField.OptionList.Add ( new EvOption ( "No" ) );
      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createBooleanField methods  

    // ==================================================================================
    /// <summary>
    /// This method creates a boolean client page field object
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="FieldDescription">String: field description </param>
    /// <param name="Value">String: filename</param>
    /// <returns>Field object</returns>
    /// <remarks> 
    /// This method consists of following steps. 
    /// 
    /// 1. Add the field to the group list.
    /// 
    /// 2. Return the field to the group list. 
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createBinaryFileField (
      object FieldId,
      String FieldTitle,
      String FieldDescription,
      String Value )
    {
      if ( Value == null )
      {
        Value = String.Empty;
      }
      Field pageField = new Field ( );

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Binary_File;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      if ( FieldDescription != String.Empty )
      {
        pageField.Description = FieldDescription ;
      }
      pageField.Value = Value;


      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createBooleanField methods  

    // ==================================================================================
    /// <summary>
    /// This method creates a boolean client page field object
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="Value">String: filename</param>
    /// <returns>Field object</returns>
    /// <remarks> 
    /// This method consists of following steps. 
    /// 
    /// 1. Add the field to the group list.
    /// 
    /// 2. Return the field to the group list. 
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createBinaryFileField (
      object FieldId,
      String FieldTitle,
      String Value )
    {
      if ( Value == null )
      {
        Value = String.Empty;
      }
      Field pageField = new Field ( );
      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Binary_File;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      pageField.Value = Value;
      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createBooleanField methods  

    // ==================================================================================
    /// <summary>
    /// This method creates a text client page field object
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="Value">DateTime: field content</param>
    /// <param name="MinimumDate">DateTime: minimum date</param>
    /// <param name="MaximumDate">DateTime: maximum date</param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. If MinimumDate is equal to 1 JAN 1900, set MaximumDate 1 Jan 2100.
    /// 
    /// 2. If DateContent is equal to CONST_DATE_NULL(1 JAN 1900) of EvStatics, Set pageField Value empty.
    /// 
    /// 3. Add the field to the group list. 
    /// 
    /// 4. Return the field object. 
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createDateField (
      object FieldId,
      String FieldTitle,
      DateTime Value,
      DateTime MinimumDate,
      DateTime MaximumDate )
    {
      if ( Value == null )
      {
        Value = EuStatics.CONST_DATE_NULL;
      }

      //
      // If MinimumDate is equal to 1 JAN 1900, set MaximumDate 1 Jan 2100.
      //
      if ( MinimumDate == EuStatics.CONST_DATE_NULL )
      {
        MaximumDate = DateTime.Parse ( "1 Jan 2100" );
      }

      Field pageField = new Field ( );
      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Date;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      pageField.Value = Value.ToString ( "dd MMM yyyy" );
      pageField.AddParameter ( FieldParameterList.Width, "12" );
      pageField.AddParameter ( FieldParameterList.Min_Value, MinimumDate.ToString ( "dd MMM yyyy" ) );
      pageField.AddParameter ( FieldParameterList.Max_Value, MaximumDate.ToString ( "dd MMM yyyy" ) );

      //
      // If DateContent is equal to CONST_DATE_NULL(1 JAN 1900) of EvStatics, Set pageField Value empty.
      //
      if ( Value == EuStatics.CONST_DATE_NULL )
      {
        pageField.Value = String.Empty;
      }

      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createDateField method 

    // ==================================================================================
    /// <summary>
    /// This method creates a text client page field object
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="Value">DateTime: field content</param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. If FiledDescription not equl to empty, Pass Description and FieldDescription to the method
    ///    AddParameter of th Field object 
    /// 
    /// 2. Add the field to the group list. 
    /// 
    /// 3. Return the field object. 
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createDateField (
      object FieldId,
      String FieldTitle,
      DateTime Value )
    {
      if ( Value == null )
      {
        Value = EuStatics.CONST_DATE_NULL;
      }
      Field pageField = new Field ( );
      DateTime MinimumDate = DateTime.Now.AddYears ( -100 );
      DateTime MaximumDate = DateTime.Now.AddYears ( +10 );
      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Date;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;

      pageField.Value = EuStatics.getDateAsString ( Value );
      pageField.AddParameter ( FieldParameterList.Width, "12" );
      pageField.AddParameter ( FieldParameterList.Min_Value, MinimumDate.ToString ( "dd MMM yyyy" ) );
      pageField.AddParameter ( FieldParameterList.Max_Value, MaximumDate.ToString ( "dd MMM yyyy" ) );

      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createDateField method 

    // ==================================================================================
    /// <summary>
    /// This method creates a text client page field object
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="Value">DateTime: field content</param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consits of following steps.
    /// 
    /// 1. Add the field to the group list. 
    /// 
    /// 2. Return the field object. 
    /// 
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createTimeField (
      object FieldId,
      String FieldTitle,
      DateTime Value )
    {
      if ( Value == null )
      {
        Value = EuStatics.CONST_DATE_NULL;
      }
      Field pageField = new Field ( );
      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Time;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      pageField.Value = Value.ToString ( "HH:mm:ss" );
      pageField.AddParameter ( FieldParameterList.Width, "5" );
      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createTimeField method 

    // ==================================================================================
    /// <summary>
    /// This method creates a text client page field object
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="FieldDescription">String: field description </param>
    /// <param name="Value">DateTime: field content</param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps.
    /// 
    ///1. If FiledDescription not equl to empty, Pass Description and FieldDescription to the method
    ///    AddParameter of th Field object
    ///    
    ///2. Add the field to the group list.
    ///
    ///3. Return the field object. 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createTimeField (
      object FieldId,
      String FieldTitle,
      String FieldDescription,
      DateTime Value )
    {
      if ( Value == null )
      {
        Value = EuStatics.CONST_DATE_NULL;
      }
      Field pageField = new Field ( );
      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Time;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;

      //
      // If FiledDescription not equl to empty, Pass Description and FieldDescription to the method
      //AddParameter of th Field object 
      //
      if ( FieldDescription != String.Empty )
      {
        pageField.Description = FieldDescription ;
      }
      pageField.Value = Value.ToString ( "HH:mm:ss" );
      pageField.AddParameter ( FieldParameterList.Width, "5" );
      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createTimeField method 

    // ==================================================================================
    /// <summary>
    /// This method creates a numeric client page field object
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="Value">float: field content</param>
    /// <param name="MinimumValue">float: minimum value</param>
    /// <param name="MaximumValue">float: maximum value</param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Add the field to the group list. 
    /// 
    /// 2. Return the field object. 
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createNumericField (
      object FieldId,
      String FieldTitle,
      float Value,
      float MinimumValue,
      float MaximumValue )
    {
      Field pageField = new Field ( );
      String stMinimum = MinimumValue.ToString ( );
      String stMaximum = MaximumValue.ToString ( );

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Numeric;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      pageField.Value = Value.ToString ( );
      pageField.AddParameter ( FieldParameterList.Width, "12" );
      pageField.AddParameter ( FieldParameterList.Min_Value, stMinimum );
      pageField.AddParameter ( FieldParameterList.Max_Value, stMaximum );

      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createNumericField method 

    // ==================================================================================
    /// <summary>
    /// This method creates a numeric client page field object
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="FieldDescription">String: field description </param>
    /// <param name="Value">float: field content</param>
    /// <param name="MinimumValue">float: minimum value</param>
    /// <param name="MaximumValue">float: maximum value</param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Add the field to the group list. 
    /// 
    /// 2. Return the field object. 
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createNumericField (
      object FieldId,
      String FieldTitle,
      String FieldDescription,
      float Value,
      float MinimumValue,
      float MaximumValue )
    {
      Field pageField = new Field ( );

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Numeric;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      if ( FieldDescription != String.Empty )
      {
        pageField.Description = FieldDescription ;
      }
      pageField.Value = Value.ToString ( );
      pageField.AddParameter ( FieldParameterList.Min_Value, MinimumValue.ToString ( ) );
      pageField.AddParameter ( FieldParameterList.Max_Value, MaximumValue.ToString ( ) );
      pageField.AddParameter ( FieldParameterList.Width, "12" );

      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createNumericField method 

    // ==================================================================================
    /// <summary>
    /// This method creates a numeric client page field object
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="Value">float: field content</param>
    /// <param name="MinimumValue">float: minimum value</param>
    /// <param name="MaximumValue">float: maximum value</param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Add the field to the group list. 
    /// 
    /// 2. Return the field object. 
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createIntegerRangeField (
      object FieldId,
      String FieldTitle,
      float Value,
      float MinimumValue,
      float MaximumValue )
    {
      Field pageField = new Field ( );

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Integer_Range;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      pageField.Value = Value.ToString ( );
      pageField.AddParameter ( FieldParameterList.Width, "10" );
      pageField.AddParameter ( FieldParameterList.Min_Value, MinimumValue.ToString ( ) );
      pageField.AddParameter ( FieldParameterList.Max_Value, MaximumValue.ToString ( ) );

      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;

    }//END createIntegerRangeField method 

    // ==================================================================================
    /// <summary>
    /// This method creates a numeric client page field object
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="FieldDescription">String: field description </param>
    /// <param name="Value">float: field content</param>
    /// <param name="MinimumValue">float: minimum value</param>
    /// <param name="MaximumValue">float: maximum value</param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Add the field to the group list. 
    /// 
    /// 2. Return the field object. 
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createIntegerRangeField (
      object FieldId,
      String FieldTitle,
      String FieldDescription,
      float Value,
      float MinimumValue,
      float MaximumValue )
    {
      Field pageField = new Field ( );

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Integer_Range;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      if ( FieldDescription != String.Empty )
      {
        pageField.Description = FieldDescription ;
      }
      pageField.Value = Value.ToString ( );
      pageField.AddParameter ( FieldParameterList.Min_Value, MinimumValue.ToString ( ) );
      pageField.AddParameter ( FieldParameterList.Max_Value, MaximumValue.ToString ( ) );
      pageField.AddParameter ( FieldParameterList.Width, "10" );

      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;

    }//END createIntegerRangeField method 

    // ==================================================================================
    /// <summary>
    /// This method creates a numeric client page field object
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="Value">float: field content</param>
    /// <param name="MinimumValue">float: minimum value</param>
    /// <param name="MaximumValue">float: maximum value</param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Add the field to the group list. 
    /// 
    /// 2. Return the field object. 
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createFloatRangeField (
      object FieldId,
      String FieldTitle,
      float Value,
      float MinimumValue,
      float MaximumValue )
    {
      Field pageField = new Field ( );

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Float_Range;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      pageField.Value = Value.ToString ( );
      pageField.AddParameter ( FieldParameterList.Width, "12" );
      pageField.AddParameter ( FieldParameterList.Min_Value, MinimumValue.ToString ( ) );
      pageField.AddParameter ( FieldParameterList.Max_Value, MaximumValue.ToString ( ) );

      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;

    }//END createFloatRangeField method 

    // ==================================================================================
    /// <summary>
    /// This method creates a numeric client page field object
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="FieldDescription">String: field description </param>
    /// <param name="Value">float: field content</param>
    /// <param name="MinimumValue">float: minimum value</param>
    /// <param name="MaximumValue">float: maximum value</param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Add the field to the group list. 
    /// 
    /// 2. Return the field object. 
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createFloatRangeField (
      object FieldId,
      String FieldTitle,
      String FieldDescription,
      float Value,
      float MinimumValue,
      float MaximumValue )
    {
      Field pageField = new Field ( );

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Float_Range;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      if ( FieldDescription != String.Empty )
      {
        pageField.Description = FieldDescription ;
      }
      pageField.Value = Value.ToString ( );
      pageField.AddParameter ( FieldParameterList.Min_Value, MinimumValue.ToString ( ) );
      pageField.AddParameter ( FieldParameterList.Max_Value, MaximumValue.ToString ( ) );
      pageField.AddParameter ( FieldParameterList.Width, "12" );

      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;

    }//END createFloatRangeField method 

    // ==================================================================================
    /// <summary>
    /// This method creates a radiobutton list client page field object
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="Value">String: field content</param>
    /// <param name="OptionList">List of Option: list of option objects</param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Find the lenght of the option list largest option item less than 50 characters. 
    /// 
    /// 2. Set the maximum selection width th 50 characters
    /// 
    /// 3. Add the field to the group list. 
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createRadioButtonListField (
      object FieldId,
      String FieldTitle,
      String Value,
      List<Evado.Model.EvOption> OptionList )
    {
      if ( Value == null )
      {
        Value = String.Empty;
      }
      Field pageField = new Field ( );
      int inColumns = 50;
      //
      // Find the length of the option list largest option item less than 50 characters.
      //
      foreach ( Evado.Model.EvOption option in OptionList )
      {
        //
        // Set the maximum selection width th 50 characters 
        //
        if ( option.Description.Length > inColumns && inColumns <= 50 )
        {

          inColumns = option.Description.Length;
        }
      }//END option iteration

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Radio_Button_List;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      pageField.Value = Value;
      pageField.AddParameter ( FieldParameterList.Width, inColumns.ToString ( ) );
      pageField.AddParameter ( FieldParameterList.Height, OptionList.Count.ToString ( ) );

      pageField.OptionList = OptionList;

      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;

    }//END createRadioButtonListField method


    // ==================================================================================
    /// <summary>
    /// This method creates a radiobutton list client page field object
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="Value">String: field content</param>
    /// <param name="OptionList">List of Option: list of option objects</param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Find the lenght of the option list largest option item less than 50 characters. 
    /// 
    /// 2. Set the maximum selection width th 50 characters
    /// 
    /// 3. Add the field to the group list. 
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createRadioButtonListField (
      object FieldId,
      String FieldTitle,
      object Value,
      List<Evado.Model.EvOption> OptionList )
    {
      if ( Value == null )
      {
        Value = String.Empty;
      }
      Field pageField = new Field ( );
      int inColumns = 50;
      //
      // Find the length of the option list largest option item less than 50 characters.
      //
      foreach ( Evado.Model.EvOption option in OptionList )
      {
        //
        // Set the maximum selection width th 50 characters 
        //
        if ( option.Description.Length > inColumns && inColumns <= 50 )
        {

          inColumns = option.Description.Length;
        }
      }//END option iteration

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Radio_Button_List;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      pageField.Value = Value.ToString ( );
      pageField.AddParameter ( FieldParameterList.Width, inColumns.ToString ( ) );
      pageField.AddParameter ( FieldParameterList.Height, OptionList.Count.ToString ( ) );

      pageField.OptionList = OptionList;

      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;

    }//END createRadioButtonListField method

    // ==================================================================================
    /// <summary>
    /// This method creates a radiobutton list client page field object
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="FieldDescription">String: field description </param>
    /// <param name="Value">String: field content</param>
    /// <param name="OptionList">List of Option: list of option objects</param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Find the length of the option list largest option item less thatn 50 characters.
    /// 
    /// 2. Set the maximum selection width th 50 characters.
    /// 
    /// 3. Add the field to the group list.
    /// 
    /// 4. Return the field object. 
    ///
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createRadioButtonListField (
      object FieldId,
      String FieldTitle,
      String FieldDescription,
      String Value,
      List<Evado.Model.EvOption> OptionList )
    {
      if ( Value == null )
      {
        Value = String.Empty;
      }
      Field pageField = new Field ( );
      int inColumns = 50;

      //
      // Find the length of the option list largest option item less thatn 50 characters.
      //
      foreach ( Evado.Model.EvOption option in OptionList )
      {
        //
        // Set the maximum selection width th 50 characters
        //
        if ( option.Description.Length > inColumns && inColumns <= 50 )
        {

          inColumns = option.Description.Length;
        }
      }//END option iteration

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Radio_Button_List;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      if ( FieldDescription != String.Empty )
      {
        pageField.Description = FieldDescription ;
      }
      pageField.Value = Value;
      pageField.AddParameter ( FieldParameterList.Width, inColumns.ToString ( ) );
      pageField.AddParameter ( FieldParameterList.Height, OptionList.Count.ToString ( ) );

      pageField.OptionList = OptionList;

      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;

    }//END createRadioButtonListField method 

    // ==================================================================================
    /// <summary>
    /// This method creates a radiobutton list client page field object
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="Value">String: field content</param>
    /// <param name="OptionList">List of Option: list of option objects</param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Find the length of the option list largest option item less thatn 50 characters.
    /// 
    /// 2. Set the maximum selection width th 50 characters
    /// 
    /// 3. Add the field to the group list.
    /// 
    /// 4. Return the field object. 
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Evado.Model.UniForm.Field createSelectionListField (
      object FieldId,
      String FieldTitle,
      object Value,
      List<Evado.Model.EvOption> OptionList )
    {
      if ( Value == null )
      {
        Value = String.Empty;
      }
      Field pageField = new Field ( );
      int inColumns = 50;

      //
      // Find the length of the option list largest option item less thatn 50 characters.
      //
      foreach ( Evado.Model.EvOption option in OptionList )
      {
        //
        // Set the maximum selection width th 50 characters
        //
        if ( option.Description.Length > inColumns
          && inColumns <= 50 )
        {

          inColumns = option.Description.Length;
        }
      }//END option iteration

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Selection_List;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      pageField.Value = Value.ToString ( );
      pageField.AddParameter ( FieldParameterList.Width, inColumns.ToString ( ) );
      pageField.AddParameter ( FieldParameterList.Height, OptionList.Count.ToString ( ) );

      pageField.OptionList = OptionList;

      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;

    }//END createSelectionListField method 

    // ==================================================================================
    /// <summary>
    /// This method creates a radiobutton list client page field object
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="Value">String: field content</param>
    /// <param name="OptionList">List of Option: list of option objects</param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Find the length of the option list largest option item less thatn 50 characters.
    /// 
    /// 2. Set the maximum selection width th 50 characters
    /// 
    /// 3. Add the field to the group list.
    /// 
    /// 4. Return the field object. 
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Evado.Model.UniForm.Field createSelectionListField (
      object FieldId,
      String FieldTitle,
      String Value,
      List<Evado.Model.EvOption> OptionList )
    {
      Field pageField = new Field ( );
      int inColumns = 50;

      //
      // Find the length of the option list largest option item less thatn 50 characters.
      //
      foreach ( Evado.Model.EvOption option in OptionList )
      {
        //
        // Set the maximum selection width th 50 characters
        //
        if ( option.Description.Length > inColumns
          && inColumns <= 50 )
        {

          inColumns = option.Description.Length;
        }
      }//END option iteration

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Selection_List;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      pageField.Value = Value;
      pageField.AddParameter ( FieldParameterList.Width, inColumns.ToString ( ) );
      pageField.AddParameter ( FieldParameterList.Height, OptionList.Count.ToString ( ) );

      pageField.OptionList = OptionList;

      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;

    }//END createSelectionListField method 



    // ==================================================================================
    /// <summary>
    /// This method creates a radiobutton list client page field object
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="FieldDescription">String: field description </param>
    /// <param name="Value">String: field content</param>
    /// <param name="OptionList">List of Option: list of option objects</param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps.
    /// 
    /// 1. Find the length of the option list largest option item less thatn 50 characters.
    /// 
    /// 2. Set the maximum selection width th 50 characters.
    /// 
    /// 3. Add the field to the group list. 
    /// 
    /// 4. Return the field object. 
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createSelectionListField (
      object FieldId,
      String FieldTitle,
      String FieldDescription,
      object Value,
      List<Evado.Model.EvOption> OptionList )
    {
      if ( Value == null )
      {
        Value = String.Empty;
      }
      Field pageField = new Field ( );
      int inColumns = 50;

      //
      // Find the length of the option list largest option item less thatn 50 characters.
      //
      foreach ( Evado.Model.EvOption option in OptionList )
      {
        //
        // Set the maximum selection width th 50 characters
        //
        if ( option.Description.Length > inColumns
          && inColumns <= 50 )
        {

          inColumns = option.Description.Length;
        }
      }//END option iteration 

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Selection_List;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      if ( FieldDescription != String.Empty )
      {
        pageField.Description = FieldDescription ;
      }
      pageField.Value = Value.ToString ( );
      pageField.AddParameter ( FieldParameterList.Width, inColumns.ToString ( ) );
      pageField.AddParameter ( FieldParameterList.Height, OptionList.Count.ToString ( ) );

      pageField.OptionList = OptionList;

      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createSelectionListField method 

    // ==================================================================================
    /// <summary>
    /// This method creates a checkbox list client page field object
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="Value">String: field content</param>
    /// <param name="OptionList">List of Option: list of option objects</param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps.
    /// 
    /// 1. Find the length of the option list largest option item less thatn 50 characters.
    /// 
    /// 2. Set the maximum selection width th 50 characters
    /// 
    /// 3. Add the field to the group list. 
    /// 
    /// 4. Return the group object. 
    ///
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createCheckBoxListField (
      object FieldId,
      String FieldTitle,
      object Value,
      List<EvOption> OptionList )
    {
      if ( Value == null )
      {
        Value = String.Empty;
      }
      Field pageField = new Field ( );
      int inColumns = 50;

      //
      // Find the length of the option list largest option item less thatn 50 characters.
      //
      foreach ( EvOption option in OptionList )
      {
        //
        // Set the maximum selection width th 50 characters
        //
        if ( option.Description.Length > inColumns
          && inColumns <= 50 )
        {

          inColumns = option.Description.Length;
        }
      }//END option iteration 

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Check_Box_List;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      pageField.Value = Value.ToString ( );
      pageField.AddParameter ( FieldParameterList.Width, inColumns.ToString ( ) );
      pageField.AddParameter ( FieldParameterList.Height, OptionList.Count.ToString ( ) );

      pageField.OptionList = OptionList;
      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createCheckBoxListField method 

    // ==================================================================================
    /// <summary>
    /// This method creates a checkbox list client page field object
    /// </summary>
    /// <param name="FieldId">String: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="FieldDescription">String: field description </param>
    /// <param name="Value">String: field content</param>
    /// <param name="OptionList">List of Option: list of option objects</param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Find the length of the option list largest option item less thatn 50 characters.
    /// 
    /// 2. Set the maximum selection width th 50 characters
    /// 
    /// 3. Add the field to the group list. 
    /// 
    /// 4. Return the field object. 
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createCheckBoxListField (
      object FieldId,
      String FieldTitle,
      String FieldDescription,
      object Value,
      List<EvOption> OptionList )
    {
      if ( Value == null )
      {
        Value = String.Empty;
      }
      Field pageField = new Field ( );
      int inColumns = 50;

      //
      // Find the length of the option list largest option item less thatn 50 characters.
      //
      foreach ( EvOption option in OptionList )
      {
        //
        // Set the maximum selection width th 50 characters
        //
        if ( option.Description.Length > inColumns
                  && inColumns <= 50 )
        {

          inColumns = option.Description.Length;
        }
      }//END option iteration 

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Check_Box_List;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      if ( FieldDescription != String.Empty )
      {
        pageField.Description = FieldDescription ;
      }
      pageField.Value = Value.ToString ( );
      pageField.AddParameter ( FieldParameterList.Width, inColumns.ToString ( ) );
      pageField.AddParameter ( FieldParameterList.Height, OptionList.Count.ToString ( ) );

      pageField.OptionList = OptionList;
      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createCheckBoxListField method 

    // ==================================================================================
    /// <summary>
    /// This method creates a image client page field object.
    /// 
    /// The image is directly downloaded from the server so we need to provide 
    /// output it as a file to the temp directory.
    /// 
    /// </summary>
    /// <param name="FieldId">String: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="ImageFileName">String: the image filename </param>
    /// <param name="Width">Int: length of the image in pixels (value less than 1 are not added) </param>
    /// <param name="Height">Int: height of the image in pixels (value less than 1 are not added) </param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. If Width is greater than 0, pass the Size and Width to AddParameter method.
    /// 
    /// 2. If Height is greater than 0, pass Rows and Height to AddParameter method. 
    /// 
    /// 3. Add the field to the group list.
    /// 
    /// 4. Return the field object. 
    /// 
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createImageField (
      object FieldId,
      String FieldTitle,
      String ImageFileName,
      int Width,
      int Height )
    {
      if ( ImageFileName == null )
      {
        ImageFileName = String.Empty;
      }
      Field pageField = new Field ( );

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Image;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      pageField.Value = ImageFileName;

      //
      // If Width is greater than 0, pass the Size and Width to AddParameter method. 
      //
      if ( Width > 0 )
      {
        pageField.AddParameter ( FieldParameterList.Width, Width.ToString ( ) );
      }

      //
      // If Height is greater than 0, pass Rows and Height to AddParameter method. 
      //
      if ( Height > 0 )
      {
        pageField.AddParameter ( FieldParameterList.Height, Height.ToString ( ) );
      }
      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;

    }//END createImageField method 

    // ==================================================================================
    /// <summary>
    /// This method creates a image client page field object.
    /// 
    /// The image is directly downloaded from the server so we need to provide 
    /// output it as a file to the temp directory.
    /// 
    /// </summary>
    /// <param name="FielIdId">String: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="FieldDescription">String: field description </param>
    /// <param name="ImageFileName">String: the image filename </param>
    /// <param name="Width">Int: length of the image in pixels (value less than 1 are not added) </param>
    /// <param name="Height">Int: height of the image in pixels (value less than 1 are not added) </param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. If FieldDescription is not empty, pass Description and FieldDescription to AddParameter method.
    /// 
    /// 2. If Width is greater than 0, pass Size and Width to AddParameter method. 
    /// 
    /// 3. If Height is greater than 0, pass Rows and Height to AddParameter method. 
    /// 
    /// 4. Add the field to the group list. 
    /// 
    /// 5. Return the field object. 
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createImageField (
      object FielIdId,
      String FieldTitle,
      String FieldDescription,
      String ImageFileName,
      int Width,
      int Height )
    {
      if ( ImageFileName == null )
      {
        ImageFileName = String.Empty;
      }
      Field pageField = new Field ( );

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Image;
      pageField.FieldId = FielIdId.ToString ( );
      pageField.Title = FieldTitle;

      //
      // If FieldDescription is not empty, pass Description and FieldDescription to AddParameter method.
      //

      if ( FieldDescription != String.Empty )
      {
        pageField.Description = FieldDescription ;
      }
      pageField.Value = ImageFileName;

      //
      // If Width is greater than 0, pass Size and Width to AddParameter method. 
      //
      if ( Width > 0 )
      {
        pageField.AddParameter ( FieldParameterList.Width, Width.ToString ( ) );
      }

      //
      // If Height is greater than 0, pass Rows and Height to AddParameter method. 
      //
      if ( Height > 0 )
      {
        pageField.AddParameter ( FieldParameterList.Height, Height.ToString ( ) );
      }

      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createImageField method 

    // ==================================================================================
    /// <summary>
    /// This method creates a bar code client page field object.
    /// 
    /// The image is directly downloaded from the server so we need to provide 
    /// output it as a file to the temp directory.
    /// 
    /// </summary>
    /// <param name="FieldId">String: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="Value">String: bar code value </param>
    /// <param name="Size">Int: length of the field in characters</param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Add the field to the group list. 
    /// 
    /// 2. Return the field object.
    ///
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createBarCode (
      object FieldId,
      String FieldTitle,
      String Value,
      int Size )
    {
      if ( Value == null )
      {
        Value = String.Empty;
      }
      Field pageField = new Field ( );

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Bar_Code;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      pageField.Value = Value;
      pageField.AddParameter ( FieldParameterList.Width, Size.ToString ( ) );
      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createBarCode method 

    // ==================================================================================
    /// <summary>
    /// This method creates a bar code client page field object.
    /// 
    /// The image is directly downloaded from the server so we need to provide 
    /// output it as a file to the temp directory.
    /// 
    /// </summary>
    /// <param name="FieldId">String: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="FieldDescription">String: field description </param>
    /// <param name="Value">String: barcode value </param>
    /// <param name="Size">Int: length of the field in characters</param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. If FieldDescription is not equal to empty, pass Description and FieldDescription to AddParameter method.
    /// 
    /// 2. Add the field to the group list.
    /// 
    /// 3. Return the field object. 
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createBarCode (
      object FieldId,
      String FieldTitle,
      String FieldDescription,
      String Value,
      int Size )
    {
      if ( Value == null )
      {
        Value = String.Empty;
      }
      Field pageField = new Field ( );

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Bar_Code;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;

      //
      // If FieldDescription is not equal to empty, pass Description and FieldDescription to AddParameter method. 
      //
      if ( FieldDescription != String.Empty )
      {
        pageField.Description = FieldDescription ;
      }
      pageField.Value = Value;
      pageField.AddParameter ( FieldParameterList.Width, Size.ToString ( ) );
      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createBarCode method

    // ==================================================================================
    /// <summary>
    /// This method creates a sound client page field object.
    /// 
    /// The image is directly downloaded from the server so we need to provide 
    /// output it as a file to the temp directory.
    /// 
    /// </summary>
    /// <param name="FieldId">String: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="FileName">String: Field FileName</param>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Add the field to the group list. 
    /// 
    /// 2. Return the field object. 
    /// 
    /// </remarks>
    /// <returns>Field object</returns>
    // ----------------------------------------------------------------------------------
    public Field createSoundField (
      object FieldId,
      String FieldTitle,
      String FileName )
    {
      if ( FileName == null )
      {
        FileName = String.Empty;
      }
      Field pageField = new Field ( );

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Sound;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      pageField.Value = FileName;
      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createSoundField

    // ==================================================================================
    /// <summary>
    /// This method creates a hidden page field object.
    /// 
    /// </summary>
    /// <param name="FieldId">String: the field data identifier</param>
    /// <param name="Value">String: Sound file enumerator</param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Add the field to the group list. 
    /// 
    /// 2. Retrun the field object.
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createHiddenField (
      object FieldId,
      String Value )
    {
      if ( Value == null )
      {
        Value = String.Empty;
      }
      Field pageField = new Field ( );

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Hidden;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Value = Value;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createHiddenField method

    // ==================================================================================
    /// <summary>
    /// This method creates a text client page field object
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="ColumnCount">Int: table column count</param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Add the field to the group list.
    /// 
    /// 2. Return the field object.
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createTableField (
      object FieldId,
      String FieldTitle,
      int ColumnCount )
    {
      Field groupField = new Field ( );
      groupField.Id = Guid.NewGuid ( );
      groupField.Type = EvDataTypes.Table;
      groupField.FieldId = FieldId.ToString ( );
      groupField.Title = FieldTitle;
      groupField.Table = new Table ( );
      groupField.Layout = Model.UniForm.FieldLayoutCodes.Column_Layout;

      groupField.EditAccess = this.EditAccess;

      //
      // Initialise the table header.
      //
      groupField.Table.Header = new TableColHeader [ ColumnCount ];

      for ( int i = 0; i < ColumnCount; i++ )
      {
        groupField.Table.Header [ i ] = new TableColHeader ( );
      }

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( groupField );

      //
      // Return the field object.
      //
      return groupField;
    }//END createTableField method 

    // ==================================================================================
    /// <summary>
    /// This method creates a text client page field object
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="FieldDescription">String: field description </param>
    /// <param name="ColumnCount">Int: table column count</param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Add the field to the group list.
    /// 
    /// 2. Return the field object.
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createTableField (
      object FieldId,
      String FieldTitle,
      String FieldDescription,
      int ColumnCount )
    {
      Field pageField = new Field ( );
      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Table;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      if ( FieldDescription != String.Empty )
      {
        pageField.Description = FieldDescription ;
      }
      pageField.Table = new Table ( );

      pageField.EditAccess = this.EditAccess;

      //
      // Initialise the table header.
      //
      pageField.Table.Header = new TableColHeader [ ColumnCount ];

      for ( int i = 0; i < ColumnCount; i++ )
      {
        pageField.Table.Header [ i ] = new TableColHeader ( );
      }

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createTableField method 

    // ==================================================================================
    /// <summary>
    /// This method creates a currency page field object.
    /// 
    /// </summary>
    /// <param name="FieldId">String: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="Value">String: barcode value </param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps.
    /// 
    /// 1. Add the field to the group list. 
    /// 
    /// 2. Retrun the field object. 
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createCurrencyCodeFIeld (
      object FieldId,
      String FieldTitle,
      String Value )
    {
      if ( Value == null )
      {
        Value = String.Empty;
      }
      Field pageField = new Field ( );

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Currency;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      pageField.Value = Value;
      pageField.AddParameter ( FieldParameterList.Width, "12" );
      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }// END createCurrencyCodeFIeld method


    // ==================================================================================
    /// <summary>
    /// This method creates a currency page field object.
    /// 
    /// </summary>
    /// <param name="FieldId">String: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="FieldDescription">String: field description </param>
    /// <param name="Value">String: barcode value </param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Add the field to the group list. 
    /// 
    /// 2. Return the field object. 
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createCurrencyCodeField (
      object FieldId,
      String FieldTitle,
      String FieldDescription,
      String Value )
    {
      if ( Value == null )
      {
        Value = String.Empty;
      }
      Field pageField = new Field ( );

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Currency;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      if ( FieldDescription != String.Empty )
      {
        pageField.Description = FieldDescription ;
      }
      pageField.Value = Value;
      pageField.AddParameter ( FieldParameterList.Width, "12" );
      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createCurrencyCodeField method

    // ==================================================================================
    /// <summary>
    /// This method creates a email address page field object.
    ///
    /// </summary>
    /// <param name="FieldId">String: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="Value">String: barcode value </param>
    /// <returns>Field object</returns>
    ///<remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Add the field to the group list.
    /// 
    /// 2. Return the field object. 
    /// 
    /// </remarks> 

    // ----------------------------------------------------------------------------------
    public Field createEmailAddressField (
      object FieldId,
      String FieldTitle,
      String Value )
    {
      if ( Value == null )
      {
        Value = String.Empty;
      }
      Field pageField = new Field ( );

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Email_Address;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      pageField.Value = Value;
      pageField.AddParameter ( FieldParameterList.Width, "80" );
      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createEmailAddressField method 

    // ==================================================================================
    /// <summary>
    /// This method creates email address page field object.
    /// 
    /// </summary>
    /// <param name="FieldId">String: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="FieldDescription">String: field description </param>
    /// <param name="Value">String: barcode value </param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. If FieldDescription is not equal to empty, Pass Description and FieldDescription to the AddParameter method
    /// 
    /// 2. Add the field value to the group list. 
    /// 
    /// 3. Return the field object. 
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createEmailAddressField (
      object FieldId,
      String FieldTitle,
      String FieldDescription,
      String Value )
    {
      if ( Value == null )
      {
        Value = String.Empty;
      }
      Field pageField = new Field ( );

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Email_Address;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;

      //
      // If FieldDescription is not equal to empty, Pass Description and FieldDescription to the AddParameter method
      //
      if ( FieldDescription != String.Empty )
      {
        pageField.Description = FieldDescription ;
      }
      pageField.Value = Value;
      pageField.AddParameter ( FieldParameterList.Width, 30 );
      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createEmailAddressField method 

    // ==================================================================================
    /// <summary>
    /// This method creates a telephone number page field object.
    ///
    /// </summary>
    /// <param name="FieldId">String: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="Value">String: barcode value </param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Add the field to the group list. 
    /// 
    /// 2. Return the field object. 
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createTelephoneNumberField (
      object FieldId,
      String FieldTitle,
      String Value )
    {
      if ( Value == null )
      {
        Value = String.Empty;
      }
      Field pageField = new Field ( );

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Telephone_Number;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      pageField.Value = Value;
      pageField.AddParameter ( FieldParameterList.Width, "15" );
      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createTelephoneNumberField method 

    // ==================================================================================
    /// <summary>
    /// This method creates a telephone number page field object.
    /// 
    /// </summary>
    /// <param name="FieldId">String: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="FieldDescription">String: field description </param>
    /// <param name="Value">String: barcode value </param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps.
    /// 
    /// 1. If FieldDescription is not equal to empty, Pass Description and FieldDescription to the AddParameter method
    /// 
    /// 2. Add the field to the group list. 
    /// 
    /// 3. Return the field object. 
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createTelephoneNumberField (
      object FieldId,
      String FieldTitle,
      String FieldDescription,
      String Value )
    {
      if ( Value == null )
      {
        Value = String.Empty;
      }
      Field pageField = new Field ( );

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Telephone_Number;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;

      //
      // If FieldDescription is not equal to empty, Pass Description and FieldDescription to the AddParameter method
      //
      if ( FieldDescription != String.Empty )
      {
        pageField.Description = FieldDescription ;
      }
      pageField.Value = Value;
      pageField.AddParameter ( FieldParameterList.Width, 15 );
      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }

    // ==================================================================================
    /// <summary>
    /// This method creates a Name page field object.
    ///
    /// </summary>
    /// <param name="FieldId">String: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="Value">String: barcode value </param>
    /// <param name="Size">Int: length of the field in characters</param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Add the field to the group list. 
    /// 
    /// 2. Return the field object. 
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createNameField (
      object FieldId,
      String FieldTitle,
      String Value)
    {
      if ( Value == null )
      {
        Value = String.Empty;
      }
      Field pageField = new Field ( );

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Name;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      pageField.Value = Value;
      pageField.AddParameter ( FieldParameterList.Width, "40" );
      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createNameField method 
    // ==================================================================================
    /// <summary>
    /// This method creates a Name page field object.
    ///
    /// </summary>
    /// <param name="FieldId">String: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="Value">String: barcode value </param>
    /// <param name="Size">Int: length of the field in characters</param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Add the field to the group list. 
    /// 
    /// 2. Return the field object. 
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createNameField (
      object FieldId,
      String FieldTitle,
      String Value,
      bool Format_Prefix,
      bool Format_Middle  )
    {
      if ( Value == null )
      {
        Value = String.Empty;
      }
      Field pageField = new Field ( );
      String format = String.Empty;

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Name;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      pageField.Value = Value;
      pageField.AddParameter ( FieldParameterList.Width, "40" );
      pageField.EditAccess = this.EditAccess;

      if ( Format_Prefix == true )
      {
        format += Field.CONST_NAME_FORMAT_PREFIX;
      }
      format += Field.CONST_NAME_FORMAT_GIVEN_NAME;

      if ( Format_Middle == true )
      {
        format += Field.CONST_NAME_FORMAT_MIDDLE_NAME;
      }
      format += Field.CONST_NAME_FORMAT_FAMILY_NAME;

      pageField.AddParameter ( FieldParameterList.Format, format );

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createNameField method 

    // ==================================================================================
    /// <summary>
    /// This method creates a name page field object.
    /// 
    /// </summary>
    /// <param name="FieldId">String: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="FieldDescription">String: field description </param>
    /// <param name="Value">String: barcode value </param>
    /// <param name="Size">Int: length of the field in characters</param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps.
    /// 
    /// 1. If FieldDescription is not equal to empty, Pass Description and FieldDescription to the AddParameter method
    /// 
    /// 2. Add the field to the group list. 
    /// 
    /// 3. Retrun the field object. 
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createNameField (
      object FieldId,
      String FieldTitle,
      String FieldDescription,
      String Value )
    {
      if ( Value == null )
      {
        Value = String.Empty;
      }
      Field pageField = new Field ( );

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Telephone_Number;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;

      //
      // If FieldDescription is not equal to empty, Pass Description and FieldDescription to the AddParameter method
      //
      if ( FieldDescription != String.Empty )
      {
        pageField.Description = FieldDescription ;
      }
      pageField.AddParameter ( FieldParameterList.Width, 40 );
      pageField.Value = Value;
      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createNameField method

    // ==================================================================================
    /// <summary>
    /// This method creates a Address page field object.
    ///
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="Value">String: barcode value </param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Add the field to the group list. 
    /// 
    /// 2. Return the field object. 
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createAddressField (
      object FieldId,
      String FieldTitle,
      String Value )
    {
      if ( Value == null )
      {
        Value = String.Empty;
      }
      Field pageField = new Field ( );

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Address;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      pageField.Value = Value;
      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createAddressField method 

    // ==================================================================================
    /// <summary>
    /// This method creates a Address page field object.
    ///
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="Address_1">String: address line 1 </param>    
    /// <param name="Address_2">String: address line 2 </param>
    /// <param name="Address_City">String: city </param>
    /// <param name="Address_State">String: state</param>
    /// <param name="Address_PostCode">String: post code</param>
    /// <param name="Addrees_Country">String: country </param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Add the field to the group list. 
    /// 
    /// 2. Return the field object. 
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createAddressField (
      object FieldId,
      String FieldTitle,
      String Address_1,
      String Address_2,
      String Address_City,
      String Address_State,
      String Address_PostCode,
      String Addrees_Country )
    {
      Field pageField = new Field ( );

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Address;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      pageField.Value = Address_1 + ";"
        + Address_2 + ";"
        + Address_City + ";"
        + Address_State + ";"
        + Address_PostCode + ";"
        + Addrees_Country + ";";
      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createAddressField method 

    // ==================================================================================
    /// <summary>
    /// This method creates a Address page field object.
    /// 
    /// </summary>
    /// <param name="FieldId">String: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="FieldDescription">String: field description </param>
    /// <param name="Value">String: barcode value </param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps.
    /// 
    /// 1. If FieldDescription is not equal to empty, Pass Description and FieldDescription to the AddParameter method
    /// 
    /// 2. Add the field to the group list. 
    /// 
    /// 3. Return the field object. 
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createAddressField (
      object FieldId,
      String FieldTitle,
      String FieldDescription,
      String Value )
    {
      if ( Value == null )
      {
        Value = String.Empty;
      }
      Field pageField = new Field ( );

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Address;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;

      //
      // If FieldDescription is not equal to empty, Pass Description and FieldDescription to the AddParameter method
      //

      if ( FieldDescription != String.Empty )
      {
        pageField.Description = FieldDescription ;
      }
      pageField.Value = Value;
      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;

    }//END createAddressField method

    // ==================================================================================
    /// <summary>
    /// This method creates an analogue page field object.
    ///
    /// </summary>
    /// <param name="FieldId">String: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="Value">String: barc code value </param>
    /// <param name="AnalogueLegendStart">String: Analogue legend start</param>
    /// <param name="AnalogueLegendFinish">String: Analogue legend finish</param>
    /// <param name="AnalogueMaximum">Int: Maximum Analogue</param>
    /// <param name="AnalogueMinimum">Int: Minimum Analogue</param>
    /// <param name="Increment">Int: Increment</param>
    /// <returns>Field object</returns>
    /// <remarks> 
    /// This method consists of following steps. 
    /// 
    /// 1. If AnalogueLegendStart is not equal to empty, Pass Min_Label and AnalogueLegendStart to AddParameter method.
    /// 
    /// 2. If AnalogueLegendFinish is not equal to empty, Pass Max_Label and AnalogueLegendFinish to AddParameter method
    /// 
    /// 3. Add the field to the group list.
    /// 
    /// 4. Return the field object. 
    /// 
    /// </remarks>
    /// 
    // ----------------------------------------------------------------------------------
    public Field createAnalogueField (
      object FieldId,
      String FieldTitle,
      String Value,
      String AnalogueLegendStart,
      String AnalogueLegendFinish,
      int AnalogueMinimum,
      int AnalogueMaximum,
      int Increment )
    {
      if ( Value == null )
      {
        Value = String.Empty;
      }
      Field pageField = new Field ( );

      //
      // Set the max and min default value.
      //
      if ( AnalogueMinimum == 0
        && AnalogueMaximum == 0 )
      {
        AnalogueMaximum = 100;
      }

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Analogue_Scale;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      pageField.Value = Value;
      pageField.EditAccess = this.EditAccess;

      pageField.AddParameter ( FieldParameterList.Increment, Increment );
      pageField.AddParameter ( FieldParameterList.Min_Value, AnalogueMinimum );
      pageField.AddParameter ( FieldParameterList.Max_Value, AnalogueMaximum );

      //
      // If AnalogueLegendStart is not equal to empty, Pass Min_Label and AnalogueLegendStart to AddParameter method
      //
      if ( AnalogueLegendStart != String.Empty )
      {
        pageField.AddParameter ( FieldParameterList.Min_Label, AnalogueLegendStart );
      }

      //
      // If AnalogueLegendFinish is not equal to empty, Pass Max_Label and AnalogueLegendFinish to AddParameter method
      //
      if ( AnalogueLegendFinish != String.Empty )
      {
        pageField.AddParameter ( FieldParameterList.Max_Label, AnalogueLegendFinish );
      }
      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createAnalogueField method 

    // ==================================================================================
    /// <summary>
    /// This method creates an analogue page field object. 
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="FieldDescription">String: field description </param>
    /// <param name="Value">String: barcode value </param>
    /// <param name="AnalogueLegendStart">String: Analogue legend start</param>
    /// <param name="AnalogueLegendFinish">String: Analogue legend finish</param>
    /// <param name="AnalogueMaximum">Int: Maximum Analogue</param>
    /// <param name="AnalogueMinimum">Int: Minimum Analogue</param>
    /// <param name="Increment">Int: Increment</param>
    /// <returns>Field object</returns>
    /// <remarks> 
    /// This method consists of following methods. 
    ///
    /// 1. If FieldDescription is not equal to empty, Pass Description and FieldDescription to AddParameter method. 
    /// 
    /// 2. If AnalogueLegendStart is not equal to empty, Pass Min_Label and AnalogueLegendStart to AddParameter method.
    /// 
    /// 3. If AnalogueLegendFinish is not equal to empty, Pass Max_Label and AnalogueLegendFinish to AddParameter method
    /// 
    /// 4. Add the field to the group list.
    /// 
    /// 5. Return the field object. 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createAnalogueField (
      object FieldId,
      String FieldTitle,
      String FieldDescription,
      String Value,
      String AnalogueLegendStart,
      String AnalogueLegendFinish,
      int AnalogueMinimum,
      int AnalogueMaximum,
      int Increment )
    {
      if ( Value == null )
      {
        Value = String.Empty;
      }
      Field pageField = new Field ( );
      if ( AnalogueMinimum == 0
        && AnalogueMaximum == 0 )
      {
        AnalogueMaximum = 100;
      }

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Address;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      pageField.Value = Value;
      pageField.EditAccess = this.EditAccess;

      //
      // If FieldDescription is not equal to empty, Pass Description and FieldDescription to AddParameter method.
      //
      if ( FieldDescription != String.Empty )
      {
        pageField.Description = FieldDescription ;
      }
      pageField.AddParameter ( FieldParameterList.Increment, Increment );
      pageField.AddParameter ( FieldParameterList.Min_Value, AnalogueMinimum );
      pageField.AddParameter ( FieldParameterList.Max_Value, AnalogueMaximum );

      //
      // If AnalogueLegendStart is not equal to empty, Pass Min_Label and AnalogueLegendStart to AddParameter method.
      //
      if ( AnalogueLegendStart != String.Empty )
      {
        pageField.AddParameter ( FieldParameterList.Min_Label, AnalogueLegendStart );
      }
      //
      // If AnalogueLegendFinish is not equal to empty, Pass Max_Label and AnalogueLegendFinish to AddParameter method
      //
      if ( AnalogueLegendFinish != String.Empty )
      {
        pageField.AddParameter ( FieldParameterList.Max_Label, AnalogueLegendFinish );
      }

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createAnalogueField method 

    // ==================================================================================
    /// <summary>
    /// This method creates a Signature page field object.
    ///
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="Value">String: barc code value </param>
    /// <param name="Width">Int: witdh of the field in pixels</param>
    /// <param name="Height">Int: height of the field in pixels</param>
    /// <returns>Field object</returns>
    // ----------------------------------------------------------------------------------
    public Field createSignatureField (
      object FieldId,
      String FieldTitle,
      String Value,
      int Width,
      int Height )
    {
      if ( Value == null )
      {
        Value = String.Empty;
      }
      Field pageField = new Field ( );

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Signature;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      pageField.Value = Value;
      pageField.EditAccess = this.EditAccess;
      if ( Width > 0 )
      {
        pageField.AddParameter ( FieldParameterList.Width, Width );
      }
      if ( Height > 0 )
      {
        pageField.AddParameter ( FieldParameterList.Height, Height );
      }

      //
      // If pageField Value is not equal to empty, set a formatted string to Value.
      //
      if ( pageField.Value == String.Empty )
      {
        pageField.Value = "{\"signature\":[],\"acceptedBy\":\"\"}";
      }

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createSignatureField method

    // ==================================================================================
    /// <summary>
    /// This method creates a Signature page field object.
    ///
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="Value">String: barc code value </param>
    /// <returns>Field object</returns>
    // ----------------------------------------------------------------------------------
    public Field createSignatureField (
      object FieldId,
      String FieldTitle,
      String Value )
    {
      if ( Value == null )
      {
        Value = String.Empty;
      }
      Field pageField = new Field ( );

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Signature;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      pageField.Value = Value;
      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createSignatureField method

    // ==================================================================================
    /// <summary>
    /// This method creates a Signature page field object.
    /// 
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="FieldDescription">String: field description </param>
    /// <param name="Value">String: barc code value </param>
    /// <param name="Width">Int: witdh of the field in pixels</param>
    /// <param name="Height">Int: height of the field in pixels</param>
    /// <returns>Field object</returns>
    // ----------------------------------------------------------------------------------
    public Field createSignatureField (
      object FieldId,
      String FieldTitle,
      String FieldDescription,
      String Value,
      int Width,
      int Height )
    {
      //
      // If pageField Value is not equal to empty, set a formatted string to Value.
      //
      if ( Value == null )
      {
        Value = "{\"Signature\":[],\"Name\":\"\",\"AcceptedBy\":\"\",\"DateStamp\":\"1900-01-01T00:00:00\"}";
      }
      if ( Value == String.Empty || Value == "null" )
      {
        Value = "{\"Signature\":[],\"Name\":\"\",\"AcceptedBy\":\"\",\"DateStamp\":\"1900-01-01T00:00:00\"}";
      }
      Field pageField = new Field ( );

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Signature;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;

      //
      // If FieldDescription is not equal to empty, pass Description and FieldDescription to AddParameter method. 
      //
      if ( FieldDescription != String.Empty )
      {
        pageField.Description = FieldDescription ;
      }
      if ( Width > 0 )
      {
        pageField.AddParameter ( FieldParameterList.Width, Width );
      }
      if ( Height > 0 )
      {
        pageField.AddParameter ( FieldParameterList.Height, Height );
      }
      pageField.Value = Value;
      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createSignatureField method

    // ==================================================================================
    /// <summary>
    /// This method creates a Signature page field object.
    /// 
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="FieldDescription">String: field description </param>
    /// <param name="Value">String: barc code value </param>
    /// <returns>Field object</returns>
    // ----------------------------------------------------------------------------------
    public Field createSignatureField (
      object FieldId,
      String FieldTitle,
      String FieldDescription,
      String Value )
    {
      //
      // If pageField Value is not equal to empty, set a formatted string to Value.
      //
      if ( Value == null )
      {
        Value = "{\"Signature\":[],\"Name\":\"\",\"AcceptedBy\":\"\",\"DateStamp\":\"1900-01-01T00:00:00\"}";
      }
      if ( Value == String.Empty )
      {
        Value = "{\"Signature\":[],\"Name\":\"\",\"AcceptedBy\":\"\",\"DateStamp\":\"1900-01-01T00:00:00\"}";
      }
      Field pageField = new Field ( );

      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Signature;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;

      //
      // If FieldDescription is not equal to empty, pass Description and FieldDescription to AddParameter method. 
      //
      if ( FieldDescription != String.Empty )
      {
        pageField.Description = FieldDescription ;
      }
      pageField.Value = Value;
      pageField.EditAccess = this.EditAccess;


      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createSignatureField method

    // ==================================================================================
    /// <summary>
    /// This method creates a password client page field object
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Add the field to the group list.
    /// 
    /// 2. Return the field object.
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createPasswordField (
      object FieldId,
      String FieldTitle )
    {
      Field pageField = new Field ( );
      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Password;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      pageField.Value = String.Empty;
      pageField.AddParameter ( FieldParameterList.Width, "50" );
      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createPasswordField method 

    // ==================================================================================
    /// <summary>
    /// This method creates a password client page field object
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="FieldDescription">String: field description </param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Add the field to the group list.
    /// 
    /// 2. Return the field object.
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createPasswordField (
      object FieldId,
      String FieldTitle,
      String FieldDescription )
    {
      Field pageField = new Field ( );
      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Password;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      pageField.AddParameter ( FieldParameterList.Width, "50" );
      pageField.EditAccess = this.EditAccess;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createPasswordField method 

    // ==================================================================================
    /// <summary>
    /// This method creates a single line chart field object
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="FieldDescription">String: field description </param>
    /// <param name="Value">Evado.Model.UniForm.PlotData: plot data</param>
    /// <param name="DisplayLegend">Bool: true = display legend</param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Add the field to the group list.
    /// 
    /// 2. Return the field object. 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createSingleLineChartField (
      object FieldId,
      String FieldTitle,
      String FieldDescription,
      Evado.Model.UniForm.PlotData Value,
      bool DisplayLegend )
    {
      //
      // exit is the plot data is null 
      //
      if ( Value == null )
      {
        return null;
      }

      //
      // Define the methods variables and objects.
      //
      Evado.Model.UniForm.Plot plot = new Model.UniForm.Plot ( );
      Evado.Model.UniForm.PlotData plotData = new Model.UniForm.PlotData ( );

      Value.Label = FieldId.ToString ( );
      Value.Type = Model.UniForm.PlotData.PlotType.Lines;
      plot.DisplayLegend = DisplayLegend;

      plot.Data.Add ( Value );

      string testData = Newtonsoft.Json.JsonConvert.SerializeObject ( plot );

      //
      // define the uniform field object.
      //
      Field pageField = new Field ( );
      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Line_Chart;
      pageField.EditAccess = EditAccess.Disabled;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      if ( FieldDescription != String.Empty )
      {
        pageField.Description = FieldDescription ;
      }
      pageField.Value = testData;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createSingleLineChartField method 

    // ==================================================================================
    /// <summary>
    /// This method creates a single line chart field object
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="FieldDescription">String: field description </param>
    /// <param name="Value">Evado.Model.UniForm.PlotData: plot data</param>
    /// <param name="DisplayLegend">Bool: true = display legend</param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Add the field to the group list.
    /// 
    /// 2. Return the field object. 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createBarChartField (
      object FieldId,
      String FieldTitle,
      String FieldDescription,
      Evado.Model.UniForm.PlotData Value,
      bool DisplayLegend )
    {
      //
      // exit is the plot data is null 
      //
      if ( Value == null )
      {
        return null;
      }

      //
      // Define the methods variables and objects.
      //
      Evado.Model.UniForm.Plot plot = new Model.UniForm.Plot ( );
      plot.DisplayLegend = DisplayLegend;

      Value.Label = FieldId.ToString ( );
      Value.Type = Model.UniForm.PlotData.PlotType.Bars;

      plot.Data.Add ( Value );


      string testData = Newtonsoft.Json.JsonConvert.SerializeObject ( plot );

      //
      // define the uniform field object.
      //
      Field pageField = new Field ( );
      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Line_Chart;
      pageField.EditAccess = EditAccess.Disabled;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      if ( FieldDescription != String.Empty )
      {
        pageField.Description = FieldDescription ;
      }
      pageField.Value = testData;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createBarChartField method 

    // ==================================================================================
    /// <summary>
    /// This method creates a pie chart chart field object
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="FieldDescription">String: field description </param>
    /// <param name="Value">String: text content</param>
    /// <param name="DisplayLegend">Bool: true = display legend</param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Add the field to the group list.
    /// 
    /// 2. Return the field object. 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createPieChartField (
      object FieldId,
      String FieldTitle,
      String FieldDescription,
      List<Evado.Model.UniForm.PlotData> Value,
      bool DisplayLegend )
    {
      //
      // exit is the plot data is null 
      //
      if ( Value == null )
      {
        return null;
      }

      //
      // Define the methods variables and objects.
      //
      Evado.Model.UniForm.Plot plot = new Model.UniForm.Plot ( );
      plot.DisplayLegend = DisplayLegend;

      plot.Data = Value;

      string testData = Newtonsoft.Json.JsonConvert.SerializeObject ( plot );

      //
      // define the uniform field object.
      //
      Field pageField = new Field ( );
      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Pie_Chart;
      pageField.EditAccess = EditAccess.Disabled;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      if ( FieldDescription != String.Empty )
      {
        pageField.Description = FieldDescription ;
      }
      pageField.Value = testData;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createPieChartField method 

    // ==================================================================================
    /// <summary>
    /// This method creates a pie chart chart field object
    /// </summary>
    /// <param name="FieldId">string: the field data identifier</param>
    /// <param name="FieldTitle">String: Field title</param>
    /// <param name="FieldDescription">String: field description </param>
    /// <param name="Value">String: text content</param>
    /// <param name="DisplayLegend">Bool: true = display legend</param>
    /// <returns>Field object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Add the field to the group list.
    /// 
    /// 2. Return the field object. 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field createDonutChartField (
      object FieldId,
      String FieldTitle,
      String FieldDescription,
      List<Evado.Model.UniForm.PlotData> Value,
      bool DisplayLegend )
    {
      //
      // exit is the plot data is null 
      //
      if ( Value == null )
      {
        return null;
      }

      //
      // Define the methods variables and objects.
      //
      Evado.Model.UniForm.Plot plot = new Model.UniForm.Plot ( );
      plot.DisplayLegend = DisplayLegend;

      plot.Data = Value;

      string testData = Newtonsoft.Json.JsonConvert.SerializeObject ( plot );

      //
      // define the uniform field object.
      //
      Field pageField = new Field ( );
      pageField.Id = Guid.NewGuid ( );
      pageField.Type = EvDataTypes.Donut_Chart;
      pageField.EditAccess = EditAccess.Disabled;
      pageField.FieldId = FieldId.ToString ( );
      pageField.Title = FieldTitle;
      if ( FieldDescription != String.Empty )
      {
        pageField.Description = FieldDescription ;
      }
      pageField.Value = testData;

      //
      // Add the field to the group list.
      //
      this._FieldList.Add ( pageField );

      //
      // Return the field object.
      //
      return pageField;
    }//END createDonutChartField method 

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
  }
}//END namespace