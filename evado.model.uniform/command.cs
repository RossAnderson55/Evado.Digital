/***************************************************************************************
 * <copyright file="Evado.Model.UniForm\AbstractedPage.cs" company="EVADO HOLDING PTY. LTD.">
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
  /// This class defines the client page command object structure.
  /// </summary>
  [Serializable]
  public partial class Command
  {
    #region Class initialisation methods

    /// <summary>
    /// This method to initialise the class.
    /// </summary>
    public Command ( )
    {
    }

    /// <summary>
    /// This method to initialise the class.
    /// </summary>
    /// <param name="Title">String: command title</param>
    /// <param name="CommandType">CommandType: enumerated value</param>
    /// <param name="ApplicationId">String: application identifier</param>
    /// <param name="ApplicationObject">String: Application object identifier</param>
    /// <param name="ApplicationMethod">ApplicationMethods: method enumerated value</param>
    public Command ( String Title, CommandTypes CommandType, String ApplicationId, String ApplicationObject, ApplicationMethods ApplicationMethod )
    {
      this._Id = Guid.NewGuid ( );
      this._Title = Title;
      this._Type = CommandType;
      this._ApplicationId = ApplicationId;
      this._Object = ApplicationObject;
      this._Method = ApplicationMethod;
    }

    /// <summary>
    /// this method to initialise the class.
    /// </summary> 
    /// <param name="Title">String: command title</param>
    /// <param name="ApplicationId">String: application identifier</param>
    /// <param name="ApplicationObject">String: Application object identifier</param>
    /// <param name="ApplicationMethod">String method emerated value</param>
    public Command ( String Title, String ApplicationId, String ApplicationObject, ApplicationMethods ApplicationMethod )
    {
      this._Id = Guid.NewGuid ( );
      this._Title = Title;
      this._Type = CommandTypes.Normal_Command;
      this._ApplicationId = ApplicationId;
      this._Object = ApplicationObject;
      this._Method = ApplicationMethod;
    }
    #endregion

    #region Class property list

    private Guid _Id = Guid.Empty;
    /// <summary>
    ///  This property contains an identifier for the command object.
    /// </summary>
    public Guid Id
    {
      get { return this._Id; }
      set { this._Id = value; }
    }

    /// <summary>
    /// This property contains the method parameter that will be used to call  the hosted application.
    /// </summary>
    [JsonProperty ( "h" )]
    public List<Parameter> Header { get; set; }

    private String _Title = String.Empty;

    /// <summary>
    /// This property contains the title of the command.
    /// </summary>
    public String Title
    {
      get { return _Title; }
      set { _Title = value.Trim ( ); }
    }

    private CommandTypes _Type = CommandTypes.Null;
    /// <summary>
    /// This property contains the command type for this command object.
    /// </summary>
    [JsonProperty ( "t" )]
    public CommandTypes Type
    {
      get { return _Type; }
      set { _Type = value; }
    }

    private String _ApplicationId = String.Empty;

    /// <summary>
    /// This property contains the application the command is call.
    /// </summary>
    [JsonProperty ( "a" )]
    public String ApplicationId
    {
      get { return _ApplicationId; }
      set { _ApplicationId = value.Trim ( ); }
    }

    private String _Object = String.Empty;

    /// <summary>
    /// This property contains the application object the command is call.
    /// </summary>
    [JsonProperty ( "o" )]
    public String Object
    {
      get { return _Object; }
      set { _Object = value.Trim ( ); }
    }

    private ApplicationMethods _Method = ApplicationMethods.Null;

    /// <summary>
    /// This property contains the application method parameter that will be called.
    /// </summary>
    [JsonProperty ( "m" )]
    public ApplicationMethods Method
    {
      get { return this._Method; }
      set { this._Method = value; }
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

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class compare methods

    // ==================================================================================
    /// <summary>
    /// This method compares the existing command object with a passed command object.  
    /// To determine if they are the same command.
    /// </summary>
    /// <remarks> 
    /// This method consists of following steps. 
    /// 
    /// 1. Command Id's match return true
    /// 
    /// 2. command AppId, Object, Method and Titles match return true.
    /// 
    /// 3. return false
    /// 
    /// </remarks>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object</param>
    /// <returns>Bool: true = matching command</returns>
    // ---------------------------------------------------------------------------------
    public bool isCommand ( Command PageCommand )
    {
      //
      // Initialise the methods variables and objects.
      //
      String PageId = PageCommand.GetPageId();
      String currentPageId = this.GetPageId();
      Guid DataGuid = PageCommand.GetGuid ( );
      Guid currentDataGuid = this.GetGuid ( );

      //
      // if the command identifiers match same command return true;
      //
      if ( this._Id == PageCommand.Id )
      {
        return true;
      }

      //
      // Set the short title that is included in the command history.
      //
      PageCommand.setShortTitle ( );

      if ( PageCommand.Method == ApplicationMethods.Custom_Method )
      {
        ApplicationMethods method = PageCommand.getCustomMethod ( );

        //
        // If the AppId, Object, method and title match then same command return true.
        //
        if ( this._ApplicationId == PageCommand.ApplicationId
          && this._Object == PageCommand.Object
          && this._Method == method )
        {
          return true;
        }

        return false;
      }

      //
      // If the AppId, Object, method and title match then same command return true.
      //
      if ( this._ApplicationId == PageCommand.ApplicationId
        && this._Object == PageCommand.Object
        && this._Method == PageCommand.Method
        && PageId == currentPageId
        && DataGuid == currentDataGuid )
      {
        return true;
      }

      return false;

    }//END isCommand method.

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class header methods

    // ==================================================================================
    /// <summary>
    /// This method sets the page command header client Url value..
    /// </summary>
    /// <param name="ClientUrl">String: the client url value.</param>
    // ---------------------------------------------------------------------------------
    public void SetClientUrl ( String ClientUrl )
    {
      this.AddHeader (
        Evado.Model.UniForm.CommandHeaderElements.Client_Url,
        ClientUrl );
    }

    // ==================================================================================
    /// <summary>
    /// This method get the device identifier
    /// </summary>
    /// <returns>String: device identifier</returns>
    // ---------------------------------------------------------------------------------
    public String GetDeviceId (  )
    {
       return this.GetHeaderValue( Evado.Model.UniForm.CommandHeaderElements.DeviceId );
    }

    // ==================================================================================
    /// <summary>
    /// This method get the device name
    /// </summary>
    /// <returns>String: device name</returns>
    // ---------------------------------------------------------------------------------
    public String GetDeviceName ( )
    {
      return this.GetHeaderValue ( Evado.Model.UniForm.CommandHeaderElements.DeviceName );
    }

    // ==================================================================================
    /// <summary>
    /// This method add a parameter to the command's header list.
    /// </summary>
    /// <param name="Name">String: the name of the parameter.</param>
    /// <param name="Value">String: the value of the parameter.</param>
    /// <remarks> 
    /// This method consists of following steps. 
    /// 
    /// 1. Iterate throgh parameter list
    /// 
    /// 2. If parameter Name is equal to Name update parameter value
    /// 
    /// 3. Else do nothing 
    /// 
    /// </remarks>
    // ---------------------------------------------------------------------------------
    public void AddHeader ( CommandHeaderElements Name, String Value )
    {
      if ( this.Header == null )
      {
        this.Header = new List<Parameter> ( );
      }

      //
      // Search the parmeters for existing parameters.
      // and exit if update the value.
      // 
      foreach ( Parameter parameter in this.Header )
      {
        if ( parameter.Name == Name.ToString ( ) )
        {
          parameter.Value = Value;

          return;
        }
      }

      this.Header.Add ( new Parameter ( Name.ToString ( ), Value ) );

    }//END AddParameter method

    // ==================================================================================
    /// <summary>
    /// This method add a parameter to the command's parameter list..
    /// </summary>
    /// <param name="Name">CommandHeaderElements: the name of the parameter.</param>
    /// <returns> String value of the header element</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Iterate through parameter list then Search the parmeters for existing parameters
    ///     and exit if update the value.
    ///     
    /// 2. If parameter Name is equal to Name then return parameter Value.
    /// 
    /// 3.  Else return empty string 
    /// 
    /// </remarks>
    // ---------------------------------------------------------------------------------
    public String GetHeaderValue ( CommandHeaderElements Name )
    {
      if ( this.Header == null )
      {
        return String.Empty;
      }

      //
      // Search the parmeters for existing parameters.
      // and exit if update the value.
      // 
      foreach ( Parameter parameter in this.Header )
      {

        //
        // If parameter Name is equal to Name then return parameter Value.
        //
        if ( parameter.Name.ToLower ( ) == Name.ToString ( ).ToLower ( ) )
        {
          return parameter.Value;
        }
      }//END parameter iteration

      //
      // Else return empty string 
      //
      return String.Empty;

    }//END AddHeader method

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class parameter methods

    // ==================================================================================
    /// <summary>
    /// This method adds a parameter to the command's parameter list..
    /// </summary>
    /// <param name="Name">String: The name of the parameter.</param>
    /// <param name="Value">String: The value of the parameter.</param>
    //  ---------------------------------------------------------------------------------
    public void AddParameter ( String Name, String Value )
    {
      if ( Name == null
        || Value == null )
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
    /// This method adds a parameter to the command's parameter list..
    /// </summary>
    /// <param name="Name">String: The name of the parameter.</param>
    /// <param name="Value">String: The value of the parameter.</param>
    //  ---------------------------------------------------------------------------------
    public void AddParameter ( object Name, object Value )
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
    public void AddParameter ( String Name, object Value )
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
    public void AddParameter ( CommandParameters Name, object Value )
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
    /// This method test to see if the parameter is in the list.
    /// </summary>
    /// <param name="Name">String: the name of the parameter.</param>
    /// <returns>True: parameter exists</returns>
    // ---------------------------------------------------------------------------------
    public bool hasParameter ( CommandParameters Name )
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
    /// This method test to see if the parameter is in the list.
    /// </summary>
    /// <param name="Name">String: the name of the parameter.</param>
    /// <returns>True: parameter exists</returns>
    // ---------------------------------------------------------------------------------
    public bool hasParameter ( String Name )
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
    /// This method test to see if the parameter is in the list.
    /// </summary>
    /// <param name="Name">String: the name of the parameter.</param>
    /// <returns>True: parameter exists</returns>
    // ---------------------------------------------------------------------------------
    public bool hasParameter ( object Name )
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
    public void DeleteParameter ( FieldParameterList Name )
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
    /// This method adds a parameter to the command's parameter list..
    /// </summary>
    /// <param name="Name">String: The name of the parameter.</param>
    //  ---------------------------------------------------------------------------------
    public void DeleteParameter ( String Name )
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
    /// This method gets a parameter value.
    /// </summary>
    /// <param name="Name">String: the name of the parameter.</param>
    /// <returns> String value of the header element</returns>
    // ---------------------------------------------------------------------------------
    public String GetParameter ( CommandParameters Name )
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

    }//END GetParameter method

    // ==================================================================================
    /// <summary>
    /// This method gets a parameter value.
    /// </summary>
    /// <param name="Name">String: the name of the parameter.</param>
    /// <returns> String value of the header element</returns>
    // ---------------------------------------------------------------------------------
    public String GetParameter ( object Name )
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
          return  parameter.Value ;
        }
      }

      return String.Empty;

    }//END GetParameter method

    // ==================================================================================
    /// <summary>
    /// This method gets a parameter value.
    /// </summary>
    /// <param name="Name">String: the name of the parameter.</param>
    /// <returns> String value of the header element</returns>
    // ---------------------------------------------------------------------------------
    public Guid GetParameterAsGuid ( object Name )
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
          return EvStatics.getGuid( parameter.Value );
        }
      }

      return Guid.Empty;

    }//END GetParameter method

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
      // Iterate through the parameters to get the selectev value.
      //
      foreach ( Parameter parameter in this._ParameterList )
      {
        if ( parameter.Name.Trim ( ) == Name.Trim ( ) )
        {
          return parameter.Value;
        }
      }

      return string.Empty;

    }//END AddParameter method

    // ==================================================================================
    /// <summary>
    /// This method add a parameter to the command's parameter list..
    /// </summary>
    /// <param name="Name">GroupParameterList: the name of the parameter.</param>
    /// <returns >String value</returns>
    //  ---------------------------------------------------------------------------------
    public EnumeratedList GetParameter<EnumeratedList> ( object Name )
    {
      //
      // get the string value of the parameter list.
      //
      String name = Name.ToString ( );
      name = name.Trim ( );

      //
      // get the value
      //
      string value = this.GetParameter ( name );

      try
      {
        return EvStatics.parseEnumValue<EnumeratedList> ( value );
      }
      catch
      {
        // Try and return enumeration 'Null' value
        try
        {
          return EvStatics.parseEnumValue<EnumeratedList> ( "Null" );
        }
        catch
        {
          return default ( EnumeratedList );
        }
      }

    }//END GetParameter method

    // ==================================================================================
    /// <summary>
    /// This method sets the custom command parameter value.
    /// </summary>
    /// <param name="Value"> ApplicationMethods enumeration
    /// of application methods
    /// </param>
    // ---------------------------------------------------------------------------------
    public void setCustomMethod ( ApplicationMethods Value )
    {
      //
      // get the string value of the parameter list.
      //
      String value = Value.ToString ( );

      this.AddParameter ( CommandParameters.Custom_Method, value );
    }

    // ==================================================================================
    /// <summary>
    /// This method gets the custom command parameter value.
    /// </summary>
    /// <returns>ApplicationMethods enumeration </returns>
    /// <remarks>
    /// This method cosists of following steps. 
    /// 
    /// 1. Iterating through the parameter list and returning the method value for the custom command parameter.
    /// 
    /// 2. Else return not selected state.
    /// 
    /// </remarks>
    // ---------------------------------------------------------------------------------
    public ApplicationMethods getCustomMethod ( )
    {
      String value = this.GetParameter ( CommandParameters.Custom_Method );

      if ( value != String.Empty )
      {
        return EvStatics.parseEnumValue<ApplicationMethods> ( value );
      }

      //
      // Else return not selected state.
      //
      return ApplicationMethods.Null;
    }

    // ==================================================================================
    /// <summary>
    /// This method add a parameter to the command's parameter list..
    /// </summary>
    /// <param name="Value">Guid: the value of the parameter.</param>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1.  Iterate through parameter list then search the parmeters for existing parameters.
    ///     and exit if update the value.
    ///     
    /// 2. If parameter Name is equal to ECLINICAL GLOBAL OBJECT then update parameter value. 
    /// 
    /// 3. Else add parameter item
    /// 
    /// </remarks>
    // ---------------------------------------------------------------------------------
    public void SetGuid ( Guid Value )
    {
      //
      // get the string value of the parameter list.
      //
      String value = Value.ToString ( );

      this.AddParameter ( CommandParameters.Guid, value );

    }//END AddParameterGuid method

    // ==================================================================================
    /// <summary>
    /// This method gets the Guid parameter value.
    /// </summary>
    /// <returns>GUID value</returns>
    /// <remarks>
    /// This method consists of following steps.
    /// 
    /// 1. Iterate through prm list.
    /// 
    /// 2. if prm Name is equal to eclinical global object then return new Guid of prm Value
    /// 
    /// 3. Else return empty Guid 
    ///
    /// </remarks>
    // ---------------------------------------------------------------------------------
    public Guid GetGuid ( )
    {
      String value = this.GetParameter ( CommandParameters.Guid );

      if ( value != String.Empty )
      {
        return new Guid ( value );
      }

      //
      // Else return empty Guid 
      //
      return Guid.Empty;

    }//END getGuidParameter method 


    // ==================================================================================
    /// <summary>
    /// This method sets the short title command parameter.
    /// </summary>
    /// <param name="Value">The short title content.</param>
    // ---------------------------------------------------------------------------------
    public void setShortTitleParameter ( String Value )
    {
      //
      // get the string value of the parameter list.
      //
      String value = Value.ToString ( );

      this.AddParameter ( CommandParameters.Short_Title, value );
    }

    // ==================================================================================
    /// <summary>
    /// This method sets the command title to the short title value.
    /// </summary>
    // ---------------------------------------------------------------------------------
    public void setShortTitle ( )
    {
      String value = this.GetParameter ( CommandParameters.Short_Title );

      //
      // If the title is less than 20 characters and there is not short title 
      // then do not shorten the title.
      //
      if ( this._Title.Length < 20
        || value == String.Empty )
      {
        return;
      }

      if ( value == String.Empty )
      {
        //
        // If short title is not available use the default title shorting approach.
        //
        value = this._Object.Replace ( "_", " " );
        if ( this._Method == ApplicationMethods.List_of_Objects )
        {
          value += EuLabels.Label_Command_List;
        }
      }

      this._Title = value;
    }

    // ==================================================================================
    /// <summary>
    /// This method gets adds the enable for mandatory fields with values parameter.
    /// </summary>
    // ---------------------------------------------------------------------------------
    public void setEnableForMandatoryFields ( )
    {
      this.AddParameter ( CommandParameters.Enable_Mandatory_Fields, "1" );
    }

    // ==================================================================================
    /// <summary>
    /// This method gets the custom command parameter value.
    /// </summary>
    /// <returns>Boolean value</returns>
    /// <remarks>
    /// This method consists of following steps.
    /// 
    /// 1. Iterate throgh prm list.
    /// 
    /// 2. If prm Name is equal to constant Enable Mandatory Fields return bool value
    /// 
    /// 3. Else return false 
    /// 
    /// </remarks>
    // ---------------------------------------------------------------------------------
    public bool getEnableForMandatoryFields ( )
    {
      String value = this.GetParameter ( CommandParameters.Enable_Mandatory_Fields );

      return EvStatics.getBool ( value );

    }//END getEnableForMandatoryFields method 

    // ==================================================================================
    /// <summary>
    /// This method add a page type parameter to the command's parameter list..
    /// </summary>
    /// <param name="Value">String: the value of the parameter.</param>
    /// <remarks>
    /// This method consists of following steps.
    /// 
    /// 1. Iterate through a parameter list then search the parmeters for existing parameters.
    ///    and exit if update the value.
    ///    
    /// 2. Iterate through the existing parameters and update the parameter matching the 
    ///    standrd parameter.
    /// 
    /// </remarks>
    // ---------------------------------------------------------------------------------
    public void SetPageId ( object Value )
    {
      //
      // get the string value of the parameter list.
      //
      String value = Value.ToString ( );

      this.AddParameter ( CommandParameters.Page_Id, value );

    }//END AddPageType method

    // ==================================================================================
    /// <summary>
    /// This method add a parameter to the command's page type if it exists.
    /// </summary>
    /// <returns> String value of the header element</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Iterate throgh a list parameter then search the parameters for existing parameters
    /// and exit if update the value.
    /// 
    /// 2. If parameter Name is equal to Name then return parameter Value. 
    /// 
    /// 3. Else return empty string. 
    /// 
    /// </remarks>
    /// 
    // ---------------------------------------------------------------------------------
    public String GetPageId ( )
    {
      return this.GetParameter ( CommandParameters.Page_Id );

    }//END GetPageType method.

    // ==================================================================================
    /// <summary>
    /// This method add a parameter to the command's page type if it exists.
    /// </summary>
    /// <returns> String value of the header element</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Iterate throgh a list parameter then search the parameters for existing parameters
    /// and exit if update the value.
    /// 
    /// 2. If parameter Name is equal to Name then return parameter Value. 
    /// 
    /// 3. Else return empty string. 
    /// 
    /// </remarks>
    /// 
    // ---------------------------------------------------------------------------------
    public EnumeratedList GetPageId<EnumeratedList> ( )
    {
      String value = this.GetParameter ( CommandParameters.Page_Id );

      return EvStatics.parseEnumValue<EnumeratedList> ( value );

    }//END GetPageType method.

    // ==================================================================================
    /// <summary>
    /// This method add a parameter to for the background colour
    /// </summary>
    /// <param name="Value">Background_Colours: the selected colour's enumerated value.</param>
    /// <remarks>
    /// This method consists of following steps.
    /// 
    /// 1. Iterate through the list paramater to determine of the parameter already exists and update it.
    /// 
    /// 2. If parameter Name is equal to CommandParameters name, return
    /// 
    /// 3. Add a new parameter to the list.
    /// 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public void SetBackgroundDefaultColour ( Background_Colours Value )
    {
      //
      // Set the command parameter.
      //
      CommandParameters Name = CommandParameters.BG_Default;
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
    /// <remarks>
    /// This method consists of following steps.
    /// 
    /// 1. Iterate through the list paramater to determine of the parameter already exists and update it.
    /// 
    /// 2. If parameter Name is equal to CommandParameters name, return
    /// 
    /// 3. Add a new parameter to the list.
    /// 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public void SetBackgroundAlternativeColour ( Background_Colours Value )
    {
      //
      // Set the command parameter.
      //
      CommandParameters Name = CommandParameters.BG_Alternative; 
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
    /// <remarks>
    /// This method consists of following steps.
    /// 
    /// 1. Iterate through the list paramater to determine of the parameter already exists and update it.
    /// 
    /// 2. If parameter Name is equal to CommandParameters name, return
    /// 
    /// 3. Add a new parameter to the list.
    /// 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public void SetBackgroundHighlightedColour ( Background_Colours Value )
    {
      //
      // Set the command parameter.
      //
      CommandParameters Name = CommandParameters.BG_Highlighted;  
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
    /// <param name="Name">CommandParameters: The name of the parameter.</param>
    /// <param name="Value">Background_Colours: the selected colour's enumerated value.</param>
    /// <remarks>
    /// This method consists of following steps.
    /// 
    /// 1. Iterate through the list paramater to determine of the parameter already exists and update it.
    /// 
    /// 2. If parameter Name is equal to CommandParameters name, return
    /// 
    /// 3. Add a new parameter to the list.
    /// 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public void SetBackgroundColour ( CommandParameters Name, Background_Colours Value )
    {
      //
      // get the string value of the parameter list.
      //
      String value = Value.ToString ( );

      this.AddParameter ( Name, value );

    }//END AddParameter method

    // ==================================================================================
    /// <summary>
    /// This method gets the background colour selection.
    ///   DEPRECIATED.
    /// </summary>
    /// <param name="Name">String: the name of the parameter.</param>
    /// <returns >String value</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Iterate through prm list of _Parameters.
    /// 
    /// 2. If prm Name is equal to CommandParameters Name, return prm Value.
    /// 
    /// 3. Return an empty string  
    /// </remarks>

    //  ---------------------------------------------------------------------------------
    public Background_Colours GetBackBroundColor ( CommandParameters Name )
    {
      //
      // Exit if the parameter is not a background colour enumeration.
      //
      if ( Name != CommandParameters.BG_Default
        && Name != CommandParameters.BG_Alternative
        && Name != CommandParameters.BG_Highlighted )
      {
        return Background_Colours.Null;
      }
      //
      // get the string value of the parameter list.
      //
      String name = CommandParameters.Enable_Mandatory_Fields.ToString ( );
      name = name.Trim ( );

      String value = this.GetParameter ( Name );

      //
      // Return an empty string 
      //
      return EvStatics.parseEnumValue<Background_Colours> ( value );

    }//END GetParameter method

    // ==================================================================================
    /// <summary>
    /// This method add a parameter to the command's parameter list..
    /// </summary>
    /// <returns> PageReference value of the command</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Get the page data guid as a string.
    /// 
    /// 2. If stPageDateGuid is not empty convert it to a guid.
    /// 
    /// </remarks>
    // ---------------------------------------------------------------------------------
    public PageReference GetPageReference ( )
    {
      try
      {
        //
        // Get the page data guid as a string.
        //
        String stPageDateGuid = this.GetParameter ( CommandParameters.Page_Data_Guid );

        //
        // if not empty convert it to a guid.
        //
        if ( stPageDateGuid != String.Empty )
        {
          Guid pageDateGuid = new Guid ( stPageDateGuid );

          return new PageReference ( this._Id, pageDateGuid );
        }
      }
      catch { }

      return null;

    }//END PageReference method

    // ==================================================================================
    /// <summary>
    /// This method returns the contents of the page command.
    /// </summary>
    /// <param name="IncludeHeader">Bool: Flag for include header  </param>
    /// <param name="includeParameters">Bool: Flag for include parameter.</param>
    /// <returns>The contents of the page command.</returns>
    /// <remarks> 
    /// This method consists of following steps. 
    /// 
    /// 1. If _Header has value and Include header flage is true then add prm Name and Value to 
    ///    formatted string stOutput.
    ///    
    /// 2. If include parameter flag is true and _Parameters list has values then add prm Name 
    ///    and prm Value to formatted string stOutput.
    ///    
    /// 3. Else add parameter count to formatted string stOutput.
    /// 
    /// 4. Return string stOutput
    /// 
    /// </remarks>
    // ---------------------------------------------------------------------------------
    public String getAsString (
      bool IncludeHeader,
      bool includeParameters )
    {
      String stOutput = "Title= '" + this._Title
       + "', Typ='" + this._Type
       + "', App='" + this._ApplicationId
       + "', Obj='" + this._Object
       + "', Mth='" + this._Method + "'";

      //
      // If _Header has value and Include header flage is true then add prm Name and Value to 
      // formatted string stOutput.
      //
      if ( this.Header != null )
      {
        if ( this.Header.Count > 0
          && IncludeHeader == true )
        {
          stOutput = "ID:" + this._Id + ", " + stOutput;
          stOutput += "\r\nHeader Parameters:";
          foreach ( Parameter prm in this.Header )
          {
            stOutput += "\r\nName: " + prm.Name + " = '" + prm.Value + "'";
          }//END prm iteration 
        }
      }

      //
      // If include parameter flag is true and _Parameters list has values then add prm Name 
      // and prm Value to formatted string stOutput.
      //
      if ( includeParameters == true )
      {
        if ( this.Parameters.Count > 0 )
        {
          stOutput += "\r\nCommand Parameters:";
          foreach ( Parameter prm in this.Parameters )
          {
            stOutput += "\r\nName: " + prm.Name + " = '" + prm.Value + "'";
          }//END prm iteration
        }
        //
        // Else add parameter count to formatted string stOutput.
        //
        else
        {
          stOutput += "\r\nNo Parameters";
        }
      }

      //
      // Return string stOutput
      //
      return stOutput;
    }//END getAsString method 

    // ==================================================================================
    /// <summary>
    /// This method returns the contents of the page command.
    /// </summary>
    /// <returns>The contents of the page command.</returns>
    /// <remarks>
    /// This method consists of following steps.
    /// 
    /// 1. Iterate throgh a list parameter then
    ///    Add Name and Value to Parameters list.
    /// 
    /// 2. Return a command object.
    /// 
    /// </remarks>
    // ---------------------------------------------------------------------------------
    public Command copyObject ( )
    {
      Command command = new Command ( );
      command.Id = this._Id;
      command.Title = this._Title;
      command.Type = this._Type;
      command.ApplicationId = this._ApplicationId;
      command.Object = this._Object;
      command.Method = this._Method;

      //
      // Iterate throgh a list parameter then  add Name and Value to Parameters list.
      //
      foreach ( Parameter parameter in this.Parameters )
      {
        command.Parameters.Add ( new Parameter ( parameter.Name, parameter.Value ) );
      }//END parameter iteration 

      //
      // Return a command object. 
      //
      return command;
    }//END copyObject method 


    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region static default command methods

    // ==================================================================================
    /// <summary>
    /// This method returns a logout command.
    /// </summary>
    /// <returns>Evado.Model.UniForm.Command object</returns>
    // ----------------------------------------------------------------------------------
    public static Evado.Model.UniForm.Command getLogoutCommand ( )
    {
      Command pageCommand = new Command (
        EuLabels.Default_Logout_Command_Title,
        EuStatics.CONST_DEFAULT,
        EuStatics.CONST_DEFAULT,
        ApplicationMethods.Get_Object );
      pageCommand.Type = CommandTypes.Logout_Command;

      return pageCommand;
    }//END getLogooutCommand method.

    // ==================================================================================
    /// <summary>
    /// This method returns a default home page command.
    /// </summary>
    /// <returns>Evado.Model.UniForm.Command object</returns>
    // ----------------------------------------------------------------------------------
    public static Evado.Model.UniForm.Command getDefaultCommand ( )
    {
      Command pageCommand = new Command (
        EuLabels.Default_Home_Page_Command_Title,
        EuStatics.CONST_DEFAULT,
        EuStatics.CONST_DEFAULT,
        ApplicationMethods.Get_Object );

      return pageCommand;
    }//END getDefaultCommand method.

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
  }

}//END namespace