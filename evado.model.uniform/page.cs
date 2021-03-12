/***************************************************************************************
 * <copyright file="Evado.Model.UniForm\Page.cs" company="EVADO HOLDING PTY. LTD.">
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
    /// This class contains the device client page description.
    /// </summary>
    [Serializable]
    public class Page
    {
    #region Class constants
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Properties

    private Guid _Id = Guid.Empty;

    /// <summary>
    ///  This property contains a Guid identifier for the Page object.
    /// </summary>
    public Guid Id
    {
      get { return this._Id; }
      set { this._Id = value; }
    }
     
    /// <summary>
    ///  This Property contains a Guid identifier for the object.
    /// </summary>
    [JsonProperty ( "pdid" )]
    public Guid PageDataGuid { get; set; }

    private EditAccess _Status = EditAccess.Disabled;

    /// <summary>
    /// This property contains definitions whether a group's fields are editable by the user
    /// when displayed in the device client.  
    /// Default is Edi_Disabled, valid values are Edit_Enabled or Edi_Disabled 
    /// </summary>
    [JsonProperty ( "st" )]
    public EditAccess EditAccess
    {
      get { return this._Status; }
      set
      {
        this._Status = EditAccess.Disabled;

        if ( value == EditAccess.Enabled )
        {
          this._Status = EditAccess.Enabled;
        }
      }
    }

    private String _PageId = String.Empty;

    /// <summary>
    ///  This property contains identification of page hierarchy.
    /// For the hierarchy to operate it is necessary for each identifier to be unique
    ///  within the hierarchy.
    /// </summary>
    [JsonProperty ( "pid" )]
    public String PageId
    {
      get { return this._PageId; }
      set { this._PageId = value; }
    }

    private GroupTypes _DefaultGroupType = GroupTypes.Default;


    /// <summary>
    /// This property contains defininition of page groups default type and controls; how a page layout is customized for specific
    /// purposes.
    /// </summary>
    [JsonProperty ( "dgt" )]
    public GroupTypes DefaultGroupType
    {
      get { return _DefaultGroupType; }
      set { _DefaultGroupType = value; }
    }

    private String _Title = String.Empty;
    /// <summary>
    /// This property contains the title of the Page.
    /// </summary>
    public String Title
    {
      get { return this._Title; }
      set { this._Title = value; }
    }

    private Command _Exit = new Command ( );
    /// <summary>
    /// This property contains exit command when leaving this page.
    /// </summary>
    public Command Exit
    {
      get { return _Exit; }
      set { _Exit = value; }
    }

    private List<Command> _CommandList = new List<Command> ( );

    /// <summary>
    /// This property contains the list of page commands.
    /// </summary>
    [JsonProperty ( "cl" )]
    public List<Command> CommandList
    {
      get { return this._CommandList; }
      set { this._CommandList = value; }
    }

    private List<Group> _GroupList = new List<Group> ( );


    /// <summary>
    /// This property contains a list of page group objects.
    /// </summary>
    [JsonProperty ( "gl" )]
    public List<Group> GroupList
    {
      get { return this._GroupList; }
      set { this._GroupList = value; }
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

    /// <summary>
    /// This property contains the names of the Java Script library for this page.
    /// </summary>
    [JsonProperty ( "lib" )]
    public String JsLibrary { get; set; }

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
    public void AddParameter ( String Name, String Value )
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
    public void AddParameter ( PageParameterList Name, object Value )
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
    public bool hasParameter ( PageParameterList Name )
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
    /// This method adds a parameter to the command's parameter list..
    /// </summary>
    /// <param name="Name">String: The name of the parameter.</param>
    //  ---------------------------------------------------------------------------------
    public void DeleteParameter ( PageParameterList Name )
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
    public String GetParameter ( PageParameterList Name )
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
    public String GetParameter ( String Name )
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
    /// This method turns on displaying groups and panels.
    /// Note: only the panels that are not in headers and columns will be displayed on 
    /// panels.
    /// </summary>
    // ---------------------------------------------------------------------------------
    public void SetAnonymousPageAccess ( )
    {
      this.AddParameter ( PageParameterList.Anonyous_Page_Access, "1" );

    }//END SetAnonymousPageAccess method

    // ==================================================================================
    /// <summary>
    /// This method indicates if the page is displaying groups as panels.
    /// </summary>
    // ---------------------------------------------------------------------------------
    public bool GetAnonymousPageAccess ( )
    {
      return this.hasParameter ( PageParameterList.Anonyous_Page_Access );

    }//END GetAnonymousPageAccess method

    // ==================================================================================
    /// <summary>
    /// This method turns on displaying groups and panels.
    /// Note: only the panels that are not in headers and columns will be displayed on 
    /// panels.
    /// </summary>
    // ---------------------------------------------------------------------------------
    public void SetDisplayGroupsAsPanels ( )
    {
      this.AddParameter ( PageParameterList.Display_Groups_As_Panels, "1" );

    }//END SetLeftColumnWidth method

    // ==================================================================================
    /// <summary>
    /// This method indicates if the page is displaying groups as panels.
    /// </summary>
    // ---------------------------------------------------------------------------------
    public bool GetDisplayGroupsAsPanels ( )
    {
      return this.hasParameter ( PageParameterList.Display_Groups_As_Panels );

    }//END SetLeftColumnWidth method

    // ==================================================================================
    /// <summary>
    /// This method sets the left column width as a percentage of page width between 0 and 50%.
    /// </summary>
    /// <param name="Value">int: the value of the parameter as a percentage of width.</param>
    // ---------------------------------------------------------------------------------
    public void SetLeftColumnWidth ( int Value )
    {
      if ( Value < 0 || Value > 50 )
      {
        Value = -1;
      }
      //
      // get the string value of the parameter list.
      //
      String value = Value.ToString ( );

      this.AddParameter ( PageParameterList.Left_Column_Width, value );

    }//END SetLeftColumnWidth method

    // ==================================================================================
    /// <summary>
    /// This method sgts the left column width as a percentage of page width.
    /// </summary>
    // ---------------------------------------------------------------------------------
    public int GetLeftColumnWidth ( )
    {
      //
      // get the string value of the parameter list.
      //
      int iValue = -1;
      String value = this.GetParameter ( PageParameterList.Left_Column_Width );

      if ( int.TryParse ( value, out iValue ) == true )
      {
        return iValue;
      }

      return -1;

    }//END SetLeftColumnWidth method

    // ==================================================================================
    /// <summary>
    /// This method sets the right column width as a percentage of the page width between 0 and 50%.
    /// </summary>
    /// <param name="Value">int: the value of the parameter as a percentage of width.</param>
    // ---------------------------------------------------------------------------------
    public void SetRightColumnWidth ( int Value )
    {
      if ( Value < 0 || Value > 50 )
      {
        Value = -1;
      }
      //
      // get the string value of the parameter list.
      //
      String value = Value.ToString ( );

      this.AddParameter ( PageParameterList.Right_Column_Width, value );

    }//END SetLeftColumnWidth method

    // ==================================================================================
    /// <summary>
    /// This method gets the left column width as a percentage of page width.
    /// </summary>
    // ---------------------------------------------------------------------------------
    public int GetRightColumnWidth ( )
    {
      //
      // get the string value of the parameter list.
      //
      int iValue = -1;
      String value = this.GetParameter ( PageParameterList.Right_Column_Width );

      if ( int.TryParse ( value, out iValue ) == true )
      {
        return iValue;
      }

      return -1;

    }//END SetLeftColumnWidth method
    
    // ==================================================================================
    /// <summary>
    /// This method returns the ucon URL value.
    /// </summary>
    //  ---------------------------------------------------------------------------------
    public void setImageUrl ( PageImageUrls Parameter, String ImageUrl )
    {
      string name = Parameter.ToString ( );

      this.AddParameter ( name, ImageUrl );

    }//END setGroupStatus method

    // ==================================================================================
    /// <summary>
    /// This method returns the ucon URL value.
    /// </summary>
    //  ---------------------------------------------------------------------------------
    public String getImageUrl ( PageImageUrls Parameter )
    {
      string name = Parameter.ToString ( );

      return this.GetParameter ( name );

    }//END setGroupStatus method

    // ==================================================================================
    /// <summary>
    /// This method adds a new command to the group
    /// </summary>
    /// <param name="PageCommand">Command: new page command.</param>
    /// <remarks>
    /// This method consists of following steps.
    /// 
    /// 1. Add a new command to the _CommandList
    /// 
    /// </remarks>
    // ---------------------------------------------------------------------------------
    public Command addCommand (
      Command PageCommand )
    {
      this._CommandList.Add ( PageCommand );

      return PageCommand;

    }//END addCommand method

    // ==================================================================================
    /// <summary>
    /// This method adds a new command to the group
    /// </summary>
    /// <param name="Title">String: command title</param>
    /// <param name="ApplicationId">String: application identifier</param>
    /// <param name="ApplicationObject">String: Application object identifier</param>
    /// <param name="ApplicationMethod">ApplicationMethods: method enumerated value</param>
    /// <returns>UniForm.Model.Field object.</returns>
    ///<remarks>
    /// This method consists of following steps
    /// 
    /// 1. Initialise the command object.
    /// 
    /// 2. Add the command to the commmand list.
    /// 
    /// 3. Return the command object 
    ///
    /// </remarks> 
    // ---------------------------------------------------------------------------------
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
      // Add the command to the command list.
      //
      this._CommandList.Add ( command );

      // 
      // Return the command object.
      //
      return command;

    }//END addCommand method


    // ==================================================================================
    /// <summary>
    /// This method adds a field group object to the page.
    /// </summary>
    /// <param name="Group">Group: the group object.</param>
    /// <remarks>
    /// This method consists of following steps
    /// 
    /// 1. Set the default group type.
    /// 
    /// 2. Add new group object to the Page object.
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public void AddGroup ( Group Group )
    {
      //
      // Set the default group type.
      //
      Group.GroupType = this._DefaultGroupType;

      //
      // Add the new group object to the Page object.
      //
      this._GroupList.Add ( Group );

    }

    // ==================================================================================
    /// <summary>
    /// This method adds a field group object to the page.
    /// </summary>
    /// <param name="Title">String: the title of the field group.</param>
    /// <returns>ClientPageGroup object.</returns>
    /// <remarks>
    /// This method consists of following steps
    /// 
    /// 1. Initialise the new group object.
    /// 
    /// 2. Set the default group type.
    /// 
    /// 3. Add the new group object to the Page object.
    /// 
    /// 4. Return the group object.
    /// 
    /// </remarks>
    /// 
    // ----------------------------------------------------------------------------------
    public Group AddGroup (
      String Title )
    {
      //
      // Initialise the new group object
      //
      Group group = new Group ( Title, this.EditAccess );

      //
      // Set the default group type and status.
      //
      group.GroupType = this._DefaultGroupType;
      group.EditAccess = this.EditAccess;

      //
      // Add the new group object to the Page object.
      //
      this._GroupList.Add ( group );

      //
      // Return the group object.
      //
      return group;

    }//END AddGroup method

    // ==================================================================================
    /// <summary>
    /// This method adds a field group object to the page.
    /// </summary>
    /// <param name="Title">String: the title of the field group.</param>
    /// <param name="EditStatus">ClientFieldEditsCodes: the default field edit state for the group.</param>
    /// <returns>ClientPageGroup object.</returns>
    /// <remarks>
    /// This method consists of following steps
    /// 
    /// 1. Initialise the new group object.
    /// 
    /// 2. Set the default group type.
    /// 
    /// 3. Add the new group object to the Page object.
    /// 
    /// 4. Return the group object.
    /// 
    /// </remarks>
    /// 
    // ----------------------------------------------------------------------------------
    public Group AddGroup (
      String Title,
      EditAccess EditStatus )
    {
      //
      // Initialise the new group object
      //
      Group group = new Group ( Title, EditStatus );

      //
      // Set the status if the group is inherited.
      //
      if ( EditStatus == EditAccess.Inherited
        || EditStatus == EditAccess.Null )
      {
        group.EditAccess = this.EditAccess;
      }

      //
      // Set the default group type.
      //
      group.GroupType = this._DefaultGroupType;

      //
      // Add the new group object to the Page object.
      //
      this._GroupList.Add ( group );

      //
      // Return the group object.
      //
      return group;

    }//END AddGroup method


    // ==================================================================================
    /// <summary>
    /// This method adds a field group object to the page.
    /// </summary>
    /// <param name="Title">String: the title of the field group.</param>
    /// <param name="Description">String: The description of the field group.</param>
    /// <param name="EditStatus">ClientFieldEditsCodes: the default field edit state for the group.</param>
    /// <returns>ClientPageGroup object.</returns>
    /// <remarks>
    /// This method consists of following steps
    /// 
    /// 1. Initialise the new group object.
    /// 
    /// 2. Set the default group type.
    /// 
    /// 3. Add the new group object to the Page object.
    /// 
    /// 4. Return the group object.
    /// 
    /// </remarks>
    /// 
    // ----------------------------------------------------------------------------------
    public Group AddGroup ( String Title, String Description, EditAccess EditStatus )
    {
      //
      // Initialise the new group object
      //
      Group group = new Group ( Title, Description, EditStatus );

      //
      // Set the status if the group is inherited.
      //
      if ( EditStatus == EditAccess.Inherited
        || EditStatus == EditAccess.Null )
      {
        group.EditAccess = this.EditAccess;
      }

      //
      // Set the default group type.
      //
      group.GroupType = this._DefaultGroupType;

      //
      // Add the new group object to the Page object.
      //
      this._GroupList.Add ( group );

      //
      // Return the group object.
      //
      return group;

    }//END AddGroup method

    // ==================================================================================
    /// <summary>
    /// This method retrieves a command from the page object.  
    /// </summary>
    /// <param name="GroupTitle">String: A command's title as a string.</param>
    /// <returns>Evado.Model.UniForm.Command object.</returns>
    /// <remarks>
    /// This method consists of following steps
    /// 
    /// 1. Iterate through the page command list looking for a matching command title.
    /// 
    /// 2. Iterate through the page group commands looking for a matching command title.
    /// 
    /// 3. Return null of none are found. 
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public bool hasGroup ( String GroupTitle )
    {
      //
      // Iterate through the page command list looking for a matching 
      // command title.
      //
      foreach ( Group group in this._GroupList )
      {

        // Compare command title and variable with the parameter Command title
        // command is returned if comparision is true

        if ( group.Title.ToLower ( ) == GroupTitle.ToLower ( ) )
        {
          return true;
        }
      }//END page command iteration loop

      // 
      // Return false if not found.
      // 
      return false;

    }//END getCommand Method

    // ==================================================================================
    /// <summary>
    /// This method retrieves a command from the page object.  
    /// </summary>
    /// <param name="CommandTitle">String: A command's title as a string.</param>
    /// <returns>Evado.Model.UniForm.Command object.</returns>
    /// <remarks>
    /// This method consists of following steps
    /// 
    /// 1. Iterate through the page command list looking for a matching command title.
    /// 
    /// 2. Iterate through the page group commands looking for a matching command title.
    /// 
    /// 3. Return null of none are found. 
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Command getCommand ( String CommandTitle )
    {
      //
      // If the command Id is empty retun null indicating nothing found.
      //
      if ( CommandTitle == String.Empty )
      {
        return null;
      }

      //
      // Iterate through the page command list looking for a matching 
      // command title.
      //
      foreach ( Command command in this._CommandList )
      {

        // Compare command title and variable with the parameter Command title
        // command is returned if comparision is true

        if ( command.Title.ToLower ( ) == CommandTitle.ToLower ( ) )
        {
          return command;
        }
      }//END page command iteration loop

      //
      // Iterate through the page group commands looking for a matching 
      // command title.
      //
      foreach ( Group group in this._GroupList )
      {
        foreach ( Command command in group.CommandList )
        {
          // Compare command title and variable with the parameter Command title
          // command is returned if comparision is true

          if ( command.Title.ToLower ( ) == CommandTitle.ToLower ( ) )
          {
            return command;
          }
        }//END page command iteration loop
      }

      // 
      // Return null of none are found.
      // 
      return null;

    }//END getCommand Method

    // ==================================================================================
    /// <summary>
    /// This method retrieves a command from the page object.  
    /// </summary>
    /// <param name="CommandId">GUID: the command identifier.</param>
    /// <returns>Evado.Model.UniForm.Command object.</returns>
    /// <remarks>
    /// This method consists of following steps
    /// 
    /// 1. Iterate through the page command list looking for a matching command title.
    /// 
    /// 2. Iterate through the page group commands looking for a matching command title.
    /// 
    /// 3. Return null of none are found. 
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Command getCommand ( Guid CommandId )
    {
      //
      // If the command Id is empty retun null indicating nothing found.
      //
      if ( CommandId == Guid.Empty )
      {
        return null;
      }

      //
      // Iterate through the page command list looking for a matching 
      // command title.
      //
      foreach ( Command command in this._CommandList )
      {

        // Compare command title and variable with the parameter Command title
        // command is returned if comparision is true

        if ( command.Id == CommandId )
        {
          return command;
        }
      }//END page command iteration loop

      //
      // Iterate through the page group commands looking for a matching 
      // command title.
      //
      foreach ( Group group in this._GroupList )
      {
        foreach ( Command command in group.CommandList )
        {
          // Compare command title and variable with the parameter Command title
          // command is returned if comparision is true

          if ( command.Id == CommandId )
          {
            return command;
          }
        }//END page command iteration loop
      }

      // 
      // Return null of none are found.
      // 
      return null;

    }//END getCommand Method

    // ==================================================================================
    /// <summary>
    /// This method retrieves a command from the page object.  
    /// </summary>
    /// <param name="CommandTitle">String: A command's title as a string.</param>
    /// <returns>Evado.Model.UniForm.Command object.</returns>
    /// <remarks>
    /// This method consists of following steps
    /// 
    /// 1. Iterate through the page command list looking for a matching command title and 
    ///    delete that command.
    /// 
    /// 2. Iterate through the page group commands looking for a matching command title and 
    ///    delete that command.
    /// 
    /// 3. Return false if the command is not removed. 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public bool deleteCommand ( String CommandTitle )
    {
      //
      // If the command Id is empty retun null indicating nothing found.
      //
      if ( CommandTitle == String.Empty )
      {
        return false;
      }

      //
      // Iterate through the page command list looking for a matching 
      // command title and delete that command.
      //
      for ( int i = 0; i < this._CommandList.Count; i++ )
      {
        Command command = this._CommandList [ i ];

        // Compare command title and variable with the parameter Command title
        // command is returned if comparision is true

        if ( command.Title.ToLower ( ) == CommandTitle.ToLower ( ) )
        {
          this._CommandList.RemoveAt ( i );
          i--;
          return true;
        }
      }//END page command iteration loop

      //
      // Iterate through the page group commands looking for a matching 
      // command title and delete that command.
      //
      foreach ( Group group in this._GroupList )
      {
        for ( int i = 0; i < group.CommandList.Count; i++ )
        {
          Command command = group.CommandList [ i ];

          // Compare command title and variable with the parameter Command title
          // command is returned if comparision is true

          if ( command.Title.ToLower ( ) == CommandTitle.ToLower ( ) )
          {
            this._CommandList.RemoveAt ( i );
            i--;
            return true;
          }

        }//END group command list iteration loop

      }//END group iteration loop

      // 
      // Return false if the command is not removed.
      // 
      return false;

    }//END deleteCommand Method

    // ==================================================================================
    /// <summary>
    /// This method deleted a command from the page object.  
    /// </summary>
    /// <param name="CommandId">String: A command's title as a string.</param>
    /// <returns>Evado.Model.UniForm.Command object.</returns>
    /// <remarks>
    /// This method consists of following steps
    /// 
    /// 1. Iterate through the page command list looking for a matching command title and 
    ///    delete that command.
    /// 
    /// 2. Iterate through the page group commands looking for a matching command title and 
    ///    delete that command.
    /// 
    /// 3. Return false if the command is not removed. 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public bool deleteCommand ( Guid CommandId )
    {
      //
      // If the command Id is empty retun null indicating nothing found.
      //
      if ( CommandId == Guid.Empty )
      {
        return false;
      }

      //
      // Iterate through the page command list looking for a matching 
      // command title and delete that command.
      //
      for ( int i = 0; i < this._CommandList.Count; i++ )
      {
        Command command = this._CommandList [ i ];

        // Compare command title and variable with the parameter Command title
        // command is returned if comparision is true

        if ( command.Id == CommandId )
        {
          this._CommandList.RemoveAt ( i );
          i--;
          return true;
        }
      }//END page command iteration loop

      //
      // Iterate through the page group commands looking for a matching 
      // command title and delete that command.
      //
      foreach ( Group group in this._GroupList )
      {
        for ( int i = 0; i < group.CommandList.Count; i++ )
        {
          Command command = group.CommandList [ i ];

          // Compare command title and variable with the parameter Command title
          // command is returned if comparision is true

          if ( command.Id == CommandId )
          {
            this._CommandList.RemoveAt ( i );
            i--;
            return true;
          }

        }//END group command list iteration loop

      }//END group iteration loop

      // 
      // Return false if the command is not removed.
      // 
      return false;

    }//END getCommand Method

    // =====================================================================================
    /// <summary>
    /// This method returns the contents of the page field.
    /// </summary>
    /// <param name="DataType">EvDataTypes: a data type object.</param>
    /// <param name="DataId"> String: data Id  </param>
    /// <returns>The contents of the page command.</returns>
    /// <remarks>
    /// This method consists of following steps
    /// 
    /// 1. Loop through group list
    /// 
    /// 2. Loop through field
    /// 
    /// 3. Validate the conditions
    /// 
    /// 4. Return string 
    ///  
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public String getFieldValue (
      EvDataTypes DataType,
      String DataId )
    {
      //
      // create a stValue string
      //

      String stValue = String.Empty;

      //
      // iterate through the list group
      //


      foreach ( Evado.Model.UniForm.Group group in this._GroupList )
      {

        //
        // iterate throuth the list field
        //

        foreach ( Field field in group.FieldList )
        {
          //
          // Compare field type to data type and filed type to Null
          //
          if ( field.Type == DataType || field.Type == EvDataTypes.Null )
          {
            if ( field.FieldId == DataId )
            {
              //
              // Return filed value if filed value is equal to data Id
              //

              return field.Value;
            }
          }
        }
      }

      return stValue;
    }//END getFieldValue method

    // =====================================================================================
    /// <summary>
    /// This method returns the contents of the page field.
    /// </summary>
    /// <param name="FieldId"> String: data Id  </param>
    /// <returns>The contents of the page command.</returns>
    /// <remarks>
    /// This method consists of following steps
    /// 
    /// 1. Loop through group list
    /// 
    /// 2. Loop through field
    /// 
    /// 3. Validate the conditions
    /// 
    /// 4. Return string 
    ///  
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public String getFieldValue (
      String FieldId )
    {

      //
      // create a stValue string
      //
      String stValue = String.Empty;

      //
      // If field guid return empty string.
      //
      if ( FieldId == String.Empty )
      {
        return stValue;
      }

      //
      // iterate through the list group
      //
      foreach ( Evado.Model.UniForm.Group group in this._GroupList )
      {
        //
        // iterate throuth the list field
        //

        foreach ( Field field in group.FieldList )
        {
          //
          // Compare field type to data type and filed type to Null
          //
          if ( field.FieldId.ToLower ( ) == FieldId.ToLower ( ) )
          {
            //
            // Return filed value if filed value is equal to data Id
            //

            return field.Value;
          }
        }
      }

      return stValue;

    }//END getFieldValue method

    // =====================================================================================
    /// <summary>
    /// This method returns the contents of the page field.
    /// </summary>
    /// <param name="FieldGuid"> Guid: data Id  </param>
    /// <returns>The contents of the page command.</returns>
    /// <remarks>
    /// This method consists of following steps
    /// 
    /// 1. Loop through group list
    /// 
    /// 2. Loop through field
    /// 
    /// 3. Validate the conditions
    /// 
    /// 4. Return string 
    ///  
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public String getFieldValue (
      Guid FieldGuid )
    {

      //
      // create a stValue string
      //
      String stValue = String.Empty;

      //
      // If field guid return empty string.
      //
      if ( FieldGuid == Guid.Empty )
      {
        return stValue;
      }

      //
      // iterate through the list group
      //
      foreach ( Evado.Model.UniForm.Group group in this._GroupList )
      {
        //
        // iterate throuth the list field
        //

        foreach ( Field field in group.FieldList )
        {
          //
          // Compare field type to data type and filed type to Null
          //
          if ( field.Id == FieldGuid )
          {
            //
            // Return filed value if filed value is equal to data Id
            //

            return field.Value;
          }
        }
      }

      return stValue;

    }//END getFieldValue method

    // =====================================================================================
    /// <summary>
    /// This method returns the contents of the page field.
    /// </summary>
    /// <param name="FieldGuid"> Guid: data Id  </param>
    /// <param name="FieldValue">String: field value.</param>
    /// <returns>True: field found and updated.</returns>
    /// <remarks>
    /// This method consists of following steps
    /// 
    /// 1. Iterate through group list
    /// 
    /// 2. Iterate through field
    /// 
    /// 3. Validate the conditions
    /// 
    /// 4. Return result 
    ///  
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public bool setFieldValue (
      Guid FieldGuid,
      String FieldValue )
    {
      //
      // If field guid return empty string.
      //
      if ( FieldGuid == Guid.Empty )
      {
        return false;
      }

      //
      // iterate through the list group
      //
      foreach ( Evado.Model.UniForm.Group group in this._GroupList )
      {
        //
        // iterate throuth the list field
        //
        foreach ( Field field in group.FieldList )
        {
          //
          // Compare field ID matach update the field value and return true.
          //
          if ( field.Id == FieldGuid )
          {
            field.Value = FieldValue;
            //
            // Return result
            //
            return true;
          }
        }
      }

      return false;

    }//END getFieldValue method

    // =====================================================================================
    /// <summary>
    /// This method returns the contents of the page field.
    /// </summary>
    /// <param name="FieldGuid"> Guid: data Id  </param>
    /// <param name="Row"> int: table row to be updated.</param>
    /// <param name="Column"> int: table column to be updated.</param>
    /// <param name="FieldValue">String: field value.</param>
    /// <returns>True: field found and updated.</returns>
    /// <remarks>
    /// This method consists of following steps
    /// 
    /// 1. Iterate through group list
    /// 
    /// 2. Iterate through field
    /// 
    /// 3. Validate the conditions
    /// 
    /// 4. Return result 
    ///  
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public bool setFieldValue (
      Guid FieldGuid,
      int Row,
      int Column,
      String FieldValue )
    {
      //
      // If field guid return empty string.
      //
      if ( FieldGuid == Guid.Empty )
      {
        return false;
      }

      //
      // iterate through the list group
      //
      foreach ( Evado.Model.UniForm.Group group in this._GroupList )
      {
        //
        // iterate throuth the list field
        //
        foreach ( Field field in group.FieldList )
        {
          //
          // if the Id does not match continue.
          //
          if ( field.Id != FieldGuid )
          {
            continue;
          }

          //
          // If the matching field is not a table or matrix exit.
          //
          if ( field.Type != EvDataTypes.Table
            || field.Type != EvDataTypes.Special_Matrix )
          {
            return false;
          }

          //
          // If the field table object is null exit.
          //
          if ( field.Table == null )
          {
            return false;
          }

          //
          // If the row or column could are outside the table site exit.
          //
          if ( field.Table.Rows.Count <= Row
            || field.Table.ColumnCount <= Column )
          {
            return false;
          }

          //
          // Update the table cell value.
          //
          field.Table.Rows [ Row ].Column [ Column ] = FieldValue;

          //
          // Return result
          //
          return true;
        }
      }

      return false;

    }//END getFieldValue method

    // ==================================================================================
    /// <summary>
    /// This method generates the PageData object for this page..
    /// </summary>
    /// <param name="AppId"> String: A variable to be initialized to page data AppId </param>
    /// <param name="Object"> String: A variable to be initialized to page data Object </param>
    /// <returns>Evado.Model.UniForm.PageData object</returns>
    /// <remarks>
    /// This method consists of following steps
    /// 
    /// 1. Initialise the methods variables and objects. 
    /// 
    /// 2. Iterate through the fields.
    /// 
    /// 3. Return a page data object containing the page content. 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public PageData getPageData (
      String AppId,
      String Object )
    {
      //
      // Initialise the methods variables and objects.
      //
      PageData pageData = new PageData ( );
      pageData.Id = this.Id;
      pageData.PageId = this._PageId;
      pageData.Status = this._Status;
      pageData.AppId = AppId;
      pageData.Object = Object;

      //
      // Iterate through the page fields.
      //
      foreach ( Group group in this._GroupList )
      {
        this.getPageDataFields ( pageData.DataList, group );
      }

      //
      // Return a page data object containing the page content.
      //
      return pageData;

    }//END getPageData method

    // =================================================================================
    /// <summary>
    /// This method generate the field page data parameters.
    /// </summary>
    /// <param name="DataObjectList"> List: A list of Evado.Model.UniForm.DataObject </param>
    /// <param name="Group"> Group: the group object. </param>
    /// <remarks>
    /// This method consists of following steps
    /// 
    /// 1. Initialise the methods variables and objects. 
    /// 
    /// 2. Iterate through the group fields. 
    /// 
    /// 3. Initialise iteration parameters.
    /// 
    /// 4. Prepare the field annotation parameters.
    /// 
    /// 5. Retrieve the annotation value if it exists. 
    /// 
    /// 6. If the field has existing annotations then add the annotation data as an existing annotation content.
    /// 
    /// 7. Add an annotation field object for the user entered annotation.
    /// 
    /// 8. Field type switch to process the different field types approproately.
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    private void getPageDataFields ( List<DataObj> DataObjectList, Group Group )
    {
      //
      // Initialise the methods variables and objects.
      //
      Parameter parameter = new Parameter ( );
      DataObj data = new DataObj ( );

      //
      // Iterate through the group fields.
      //
      foreach ( Field field in Group.FieldList )
      {
        //
        // Initialise iteration parameters.
        // 
        String stAnnotationParameter = String.Empty;

        // 
        // Prepare the field annotation parameters.
        // 
        if ( Group.GroupType == UniForm.GroupTypes.Annotated_Fields
          && field.FieldId != String.Empty
          && field.Type != EvDataTypes.Read_Only_Text )
        {
          //
          // Retrieve the annotation value if it exists.
          // 
          string stAnnotation = field.GetParameter ( FieldParameterList.Annotation );
          stAnnotation = stAnnotation.Replace ( "\r\n", "\\r\\n" );

          // 
          // If the field has existing annotations then add the annotation data as an exising annotation content.
          // 
          if ( stAnnotation != String.Empty )
          {
            stAnnotationParameter +=
                 "\"" + Evado.Model.UniForm.FieldParameterList.Annotation
               + "\":\"" + stAnnotation + "\",";
          }

          //
          // Add an annotation field object for the user entered annotation.
          // 
          stAnnotationParameter +=
               "\"" + Evado.Model.UniForm.FieldParameterList.Annotation
               + DataObj.CONST_FIELD_ANNOTATION_NEW_SUFFIX
             + "\":\"\"";
        }

        //
        // Field type switch to process the different field types approproately.
        //

        switch ( field.Type )
        {

          //
          //A switch case for Evado.Model.UniFrom.EvDataType.Table
          // 

          case EvDataTypes.Table:
            {
              this.getPageDataFieldTableData ( field, DataObjectList, stAnnotationParameter );

              break;
            }

          //
          // A switch case for Evado.Model.UniForm.EvDatatype.Image
          //


          case EvDataTypes.Image:
            {
              string stHash = field.GetParameter ( FieldParameterList.MD5_Hash );
              string stHashParameters = "\"" + Evado.Model.UniForm.FieldParameterList.MD5_Hash
               + "\":\"" + stHash + "\""
               + ",\"" + Evado.Model.UniForm.FieldParameterList.Field_Type
               + "\":" + ( int ) field.Type;

              //
              // Compare stAnnotationParameter to Null
              //

              if ( stAnnotationParameter != String.Empty )
              {
                stAnnotationParameter = "{" + stHashParameters + "," + stAnnotationParameter + "}";
              }

              //
              // Add data object parameters to DataObjectList
              //

              DataObjectList.Add ( new DataObj ( field.FieldId, field.Value, stAnnotationParameter ) );

              break;
            }

          //
          // Default switch case
          //


          default:
            {
              if ( stAnnotationParameter != String.Empty )
              {
                stAnnotationParameter = "{" + stAnnotationParameter + "}";
              }
              DataObjectList.Add ( new DataObj ( field.FieldId, field.Value, stAnnotationParameter ) );
              break;
            }
        }//END field type switch

      }//END field iteration loop 

    }//END getPageDataFields method

    // =================================================================================
    /// <summary>
    /// This method generate the field page data parameters.
    /// </summary>
    /// <param name="AnnotationParameters"> String: An Annotation Parameter </param>
    /// <param name="Field"> Field : The Field Object </param>
    /// <param name="ParameterList"> List: List of Evado.Model.UniForm.DataObj object </param>
    /// <remarks>
    /// This method consists of following steps
    /// 
    /// 1. Initailise the methods variables and objects.
    /// 
    /// 2. If the array length is greater than 5 add parameters and array indexes in ParameterList 
    /// 
    /// 3. If the field is empty or has length less than 6 fill ParameterList with empty values.
    /// 
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public void getPageDataFieldAddressData ( Field Field,
      List<DataObj> ParameterList,
      String AnnotationParameters )
    {
      //
      // Initialise the methods variables and objects.
      //
      string [ ] arrValues = Field.Value.Split ( ',' );
      if ( AnnotationParameters != String.Empty )
      {
        AnnotationParameters = "{" + AnnotationParameters + "}";
      }

      //
      // If the array length is greater than 5 add parameters and array indexes in ParameterList 
      //
      if ( arrValues.Length > 5 )
      {
        ParameterList.Add ( new DataObj ( Field.FieldId, arrValues [ 0 ] ) );
        ParameterList.Add ( new DataObj ( Field.FieldId, arrValues [ 1 ], AnnotationParameters, 1 ) );
        ParameterList.Add ( new DataObj ( Field.FieldId, arrValues [ 2 ], AnnotationParameters, 2 ) );
        ParameterList.Add ( new DataObj ( Field.FieldId, arrValues [ 3 ], AnnotationParameters, 3 ) );
        ParameterList.Add ( new DataObj ( Field.FieldId, arrValues [ 4 ], AnnotationParameters, 4 ) );
        ParameterList.Add ( new DataObj ( Field.FieldId, arrValues [ 5 ], AnnotationParameters, 5 ) );
      }

      //
      //If the field is empty or has length less than 6 fill ParameterList with empty values.
      //

      if ( Field.Value == String.Empty
       || arrValues.Length < 6 )
      {
        ParameterList.Add ( new DataObj ( Field.FieldId, String.Empty ) );
        ParameterList.Add ( new DataObj ( Field.FieldId, String.Empty, AnnotationParameters, 1 ) );
        ParameterList.Add ( new DataObj ( Field.FieldId, String.Empty, AnnotationParameters, 2 ) );
        ParameterList.Add ( new DataObj ( Field.FieldId, String.Empty, AnnotationParameters, 3 ) );
        ParameterList.Add ( new DataObj ( Field.FieldId, String.Empty, AnnotationParameters, 4 ) );
        ParameterList.Add ( new DataObj ( Field.FieldId, String.Empty, AnnotationParameters, 5 ) );
      }

    }//END getPageDataFieldAddressData method

    // =================================================================================
    /// <summary>
    /// This method generate the field page data parameters.
    /// </summary>
    /// <param name="AnnotationParameters"> String: A AnnotationParameter </param>
    /// <param name="DataObjectList"> List: A list of Evado.Model.UniForm.DataObj </param>
    /// <param name="Field"> Field: A Field object </param>
    /// <remarks>
    /// This method consists of following steps
    /// 
    /// 1. Initialise the methods variables and objects.
    /// 
    /// 2. If array length is greater than 1 add parameters and arrayValues to DataObjectList.
    /// 
    /// 3. Else add an empty value to FieldId and annotation parameter value.
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public void getPageDataFieldNameData (
      Field Field,
      List<DataObj> DataObjectList,
      String AnnotationParameters )
    {
      //
      // Initialise the methods variables and objects.
      //
      string [ ] arrValues = Field.Value.Split ( ',' );

      if ( AnnotationParameters != String.Empty )
      {
        AnnotationParameters = "{" + AnnotationParameters + "}";
      }

      //
      // If array length is greater than 1 add parameters and arrayValues to DataObjectList
      //
      if ( arrValues.Length > 1 )
      {
        DataObjectList.Add ( new DataObj ( Field.FieldId, arrValues [ 0 ] ) );

        DataObjectList.Add ( new DataObj ( Field.FieldId, arrValues [ 1 ], AnnotationParameters, 1 ) );
      }

      //
      // Else add an empty value to FieldId and annotation parameter value
      //
      else
      {
        DataObjectList.Add ( new DataObj ( Field.FieldId, String.Empty ) );

        DataObjectList.Add ( new DataObj ( Field.FieldId, Field.Value, AnnotationParameters, 1 ) );
      }

    }//END getPageDataFieldNameData method

    // =================================================================================
    /// <summary>
    /// This method generate the field page data parameters.
    /// </summary>
    /// <param name="Field">Field object: contain the field to be processes.</param>
    /// <param name="DataObjectList">List DataObj</param>
    /// <param name="AnnotationParameters">String: of json parameters</param>
    /// <remarks>
    /// This method consists of following steps.
    /// 
    /// 1. Initialise the methods variables and objects.
    /// 
    /// 2. Confirm that the field has a table.
    /// 
    /// 3. Iterate through the table rows.
    /// 
    /// 4. Iterate through the coumns.
    /// 
    /// 5. Create a value if the column has a header value.
    /// 
    /// 6. Create the parameter name and value.
    /// 
    /// 7. Add page data parameter.
    /// 
    /// </remarks>
    /// 
    // ----------------------------------------------------------------------------------
    public void getPageDataFieldTableData (
      Field Field,
      List<DataObj> DataObjectList,
      String AnnotationParameters )
    {
      //
      // Initialise the methods variables and objects
      //
      int row = 0;
      int col = 0;
      if ( AnnotationParameters != String.Empty )
      {
        AnnotationParameters = "{" + AnnotationParameters + "}";
      }

      //
      // confirm that the field has a table.
      //
      if ( Field.Table != null )
      {
        //
        // Iterate through the table rows.
        //
        for ( row = 0; row < Field.Table.Rows.Count; row++ )
        {
          //
          // Iterate through the row columns.
          //
          for ( col = 0; col < Field.Table.Header.Length; col++ )
          {
            //
            // Create a value if the column has a header value.
            //
            if ( Field.Table.Header [ col ].Text != String.Empty )
            {
              //
              // create the parameter name and value
              //
              string stValue = Field.Table.Rows [ row ].Column [ col ];

              //
              // Add page data parameter
              //
              DataObjectList.Add ( new DataObj ( Field.FieldId, stValue, AnnotationParameters, row, col ) );

            }//END header exist

          }//END column iteration loop 

        }//END row iteration loop

      }//END Table object exists.

    }//END getPageDataFieldTableData method

    // ==================================================================================
    /// <summary>
    /// This method get the group value
    /// </summary>
    /// <param name="Title">String: the title of the field group.</param>
    /// <returns>ClientPageGroup object.</returns>
    /// <remarks>
    /// This method consists of following steps.
    /// 
    /// 1. Iterate through the groups to find the matching group.
    /// 
    /// 2. If group title matches with parameter passed, return a group object
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Group GetGroup ( String Title )
    {
      //
      // Iterate through the groups to find the matching group.
      //
      foreach ( Group group in this._GroupList )
      {
        if ( group.Title.ToLower ( ) == Title.ToLower ( ) )
        {
          return group;
        }
      }// END group iteration loop

      return new Group ( );

    }//END GetGroup method

    // ==================================================================================
    /// <summary>
    /// This method retrieves a command from the page object.  
    /// </summary>
    /// <param name="FieldId">String: A command's title as a string.</param>
    /// <returns>Evado.Model.UniForm.Command object.</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Iterate through the page group commands looking for a matching command title.
    ///
    /// 2. If FieldId is equal to parameter passed, return field object.
    /// 
    /// 3. Return null of none are found.
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public Field getField ( String FieldId )
    {
      //
      // Iterate through the page group commands looking for a matching 
      // command title.
      //
      foreach ( Group group in this._GroupList )
      {
        foreach ( Field field in group.FieldList )
        {
          if ( field.FieldId.ToLower ( ) == FieldId.ToLower ( ) )
          {
            return field;
          }
        }//END page command iteration loop
      }

      // 
      // Return null of none are found.
      // 
      return null;

    }//END getCommand Method

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
  }
}//END namespace