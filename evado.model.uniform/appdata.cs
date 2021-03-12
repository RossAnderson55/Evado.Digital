/***************************************************************************************
 * <copyright file="Evado.Model.UniForm\AppData.cs" company="EVADO HOLDING PTY. LTD.">
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
  /// This class contains the content of an abstracted page.
  /// </summary>
  [Serializable]
  public class AppData
  {
    #region Class initialisation

    // ==================================================================================
    /// <summary>
    /// Constructor with specified initial values
    /// </summary>
    // ----------------------------------------------------------------------------------
    public AppData ( )
    {
    }

    /// ==================================================================================
    /// <summary>
    /// Constructor with specified initial values. 
    /// </summary>
    /// <param name="ObjectTitle">String: Client Application Data object title.</param>
    /// <param name="DefaultPageFieldEditState">ClientFieldEditsCodes: Default page field edit state</param>
    // ----------------------------------------------------------------------------------
    public AppData ( String ObjectTitle, EditAccess DefaultPageFieldEditState )
    {
      this._Id = Guid.NewGuid ( );
      this._Page.Id = this._Id;
      this._Title = ObjectTitle;
      this._Page.Title = ObjectTitle;
      this._Page.EditAccess = DefaultPageFieldEditState;
    }
    // ==================================================================================
    /// <summary>
    /// Constructor with specified initaial values.
    /// </summary>
    /// <param name="ObjectTitle">String: Client Application Data object title.</param>
    /// <param name="PageTitle">String: Page title</param>
    /// <param name="DefaultPageFieldEditState">ClientFieldEditsCodes: Default page field edit state</param>
    // ----------------------------------------------------------------------------------
    public AppData ( String ObjectTitle, String PageTitle, EditAccess DefaultPageFieldEditState )
    {
      this._Id = Guid.NewGuid ( );
      this._Page.Id = Guid.NewGuid ( );
      this._Title = ObjectTitle;
      this._Page.Title = PageTitle;
      this._Page.EditAccess = DefaultPageFieldEditState;
    }


    #endregion

    #region Class Enumerators

    //  =================================================================================
    /// <summary>
    /// This enumeration defines the user loging status codes that are passed to the 
    /// UniFORM app client.
    /// </summary>
    //  ---------------------------------------------------------------------------------
    public enum StatusCodes
    {
      /// <summary>
      /// This enumeration indicates a null value.
      /// </summary>
      Null = 0,  // 0
      /// <summary>
      /// This enumeration indicates that the user's credentials have been validated.
      /// </summary>
      Login_Authenticated = 1,  // 1

      /// <summary>
      /// This enumeration indicates that the user is requested to login.
      /// </summary>
      Login_Request = 2,  // 2

      /// <summary>
      /// THis enumeration indications that the user's credentials have not been validated
      /// </summary>
      Login_Failed = 3,  // 3

      /// <summary>
      /// This enumeration indicates that the user has exceeded the login count.
      /// </summary>
      Login_Count_Exceeded = 4,  // 4

      /// <summary>
      /// This enumeration indicates that the device is not registered in this server.
      /// </summary>
      Device_Not_Registered = 5,  // 5

      /// <summary>
      /// This enumeration indicates that the device is being redirected to a new URI.
      /// </summary>
      Device_Redirection = 6, // 6

      /// <summary>
      /// This enumeration indicates that the server is expecting a synchronised command.
      /// </summary>
      Device_Off_Line = 7, // 7

      /// <summary>
      /// This enumeration indicates that the server is expecting a synchronised command.
      /// </summary>
      Syncrhonise_Device = 8, // 8
    }

    //  =================================================================================
    /// <summary>
    /// This enumeration list defines the parameter that can be passed the client.
    /// </summary>
    //  ---------------------------------------------------------------------------------
    public enum ParameterList
    {
      /// <summary>
      /// This enumerated value defines the Administration Server Url.
      /// </summary>
      Cfg_AdminUrl,

      /// <summary>
      /// This enumerated value defines the Application Server Url 1.
      /// </summary>
      Cfg_SvrUrl1,

      /// <summary>
      /// This enumerated value defines the Application Server Url 2.
      /// </summary>
      Cfg_SvrUrl2,

      /// <summary>
      /// This enumerated value defines the Application Server Url 3.
      /// </summary>
      Cfg_SvrUrl3,

      /// <summary>
      /// This enumerated value defines the relative Rest Url.
      /// </summary>
      Cfg_RelRestUrl,

      /// <summary>
      /// This enumerated value defines the relative download Url
      /// </summary>
      Cfg_RelDownloadUrl,

      /// <summary>
      /// This enumerated value defines the relative upload Url.
      /// </summary>
      Cfg_RelUploadUrl,

      /// <summary>
      /// This enumerated value defines whether the client is debug mode.
      /// </summary>
      Cfg_ClientDebug,

      /// <summary>
      /// This enumerated value defines the page background color.
      /// </summary>
      Default_Page_Background,

      /// <summary>
      /// This enumerated value defines the page text color.
      /// </summary>
      Default_Page_Color,

      /// <summary>
      /// This enumerated value defines the page default font.
      /// </summary>
      Default_Page_Font,

      /// <summary>
      /// This enumerated value defines the Page font size.
      /// </summary>
      Default_Page_Font_Size,

    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion 

    #region Class constants

    //  =================================================================================
    /// <summary>
    /// This constant defines client data API version.
    /// </summary>
    //  ---------------------------------------------------------------------------------
    public const float API_Version = 2.3F;


    ///++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Properties

    private Guid _Id = Guid.Empty;
    /// <summary>
    /// This property contains an identifier for the application data object.
    /// </summary>

    public Guid Id
    {
      get { return this._Id; }
      set { this._Id = value; }
    }

    private String _SessionId = String.Empty;

    /// <summary>
    /// This property contains the session id of the Page.
    /// </summary>
    [JsonProperty ( "sid" )]
    public String SessionId
    {
      get { return _SessionId; }
      set { _SessionId = value; }
    }

    private StatusCodes _Status = StatusCodes.Null;

    /// <summary>
    /// This property contains user login status.
    /// </summary>

    [JsonProperty ( "st" )]
    public StatusCodes Status
    {
      get { return _Status; }
      set { _Status = value; }
    }

    private String _Url = String.Empty;

    /// <summary>
    /// This property contains the service Url 
    /// </summary>

    public String Url
    {
      get { return this._Url; }
      set { this._Url = value; }
    }

    private String _Title = String.Empty;

    /// <summary>
    /// This contains the title of the Page.
    /// </summary>

    public String Title
    {
      get { return _Title; }
      set
      {
        _Title = value;
        this._Page.Title = this._Title;
      }
    }

    private String _Message = String.Empty;

    /// <summary>
    /// This property contains the  Error Message of the Page.
    /// </summary>
    [JsonProperty ( "msg" )]
    public String Message
    {
      get { return _Message; }
      set { _Message = value; }
    }

    private String _LogoFilename = String.Empty;

    /// <summary>
    /// The background image URL property
    /// </summary>
    [JsonProperty ( "logo" )]
    public String LogoFilename
    {
      get { return _LogoFilename; }
      set { _LogoFilename = value; }
    }

    private Page _Page = new Page ( );

    /// <summary>
    /// This property contains the page to be displayed on the client oject.
    /// </summary>

    public Page Page
    {
      get { return _Page; }
      set { _Page = value; }
    }

    private Offline _Offline = null;

    /// <summary>
    /// This property contains the offline storage objects.
    /// </summary>

    [JsonIgnore]
    public Offline Offline
    {
      get { return _Offline; }
      set { _Offline = value; }
    }

    private List<Parameter> _Parameters = new List<Parameter> ( );

    /// <summary>
    /// This property contains the method parameter that will be used to call the hosted application.
    /// </summary>
    [JsonProperty ( "prms" )]
    public List<Parameter> Parameters
    {
      get { return _Parameters; }
      set { _Parameters = value; }
    }
    #endregion

    #region Class paramter methods

    // ==================================================================================
    /// <summary>
    /// This method add a parameter to the command's parameter list..
    /// </summary>
    /// <param name="Name">String: the name of the parameter.</param>
    /// <param name="Value">String: the value of the parameter.</param>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Validate the Value for not being empty
    /// 
    /// 2. Loop through the _Parameter list
    /// 
    /// 3. Add Value to Name in _Parameter list 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public void SetParameter (
      ParameterList Name,
      String Value )
    {

      // Search the parmeters for existing parameters.
      // and exit if update the value.
      // 
      if ( Value == String.Empty )
      {
        return;
      }

      foreach ( Parameter parameter in this._Parameters )
      {
        if ( parameter.Name == Name.ToString ( ) )
        {
          parameter.Value = Value;

          return;
        }
      }

      this._Parameters.Add ( new Parameter ( Name.ToString ( ), Value ) );

    }//END AddParameter method

    // ==================================================================================
    /// <summary>
    /// This method add a parameter to the command's parameter list..
    /// </summary>
    /// <param name="Name">String: the name of the parameter.</param>
    /// <returns> String parameter  value</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Loop through the _Parameter list
    /// 
    /// 2. return found value 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public string GetParameter (
      ParameterList Name )
    {
      foreach ( Parameter parameter in this._Parameters )
      {
        if ( parameter.Name == Name.ToString ( ) )
        {
          return parameter.Value;
        }
      }

      return null;
    }//END AddParameter method

    #endregion

    #region Class object methods  
    
    // =====================================================================================
    /// <summary>
    /// This method retrieves a field object using its unique field identifier.
    /// </summary>
    /// <param name="DataId"> String: field Id</param> 
    /// <remarks>
    /// This method consists of following steps
    /// 
    /// 1. Iterate through group list
    /// 
    /// 2. Iterate through field list
    /// 
    /// 3. Set field Value if field equal to data Id.
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------

    public Evado.Model.UniForm.Field GetField (
      String DataId )
    {
      //
      // Loop through group list
      //
      foreach ( Evado.Model.UniForm.Group group in this._Page.GroupList )
      {
        //
        // Loop through field loop 
        //
        foreach ( Field field in group.FieldList )
        {
          //
          // Making comparision betwewn filed Id and data Id
          //

          if ( field.FieldId == DataId )
          {
            //
            // return the field object
            //
            return field;
          }
        }//END field Iteration
      }//END group iteration
      return null;
    }//END setFieldValue method

    // =====================================================================================
    /// <summary>
    /// This method update the contents of the page field value.
    /// </summary>
    /// <param name="DataId"> String: field Id</param> 
    /// <param name="Value"> String: field value</param>    
    /// <remarks>
    /// This method consists of following steps
    /// 
    /// 1. Iterate through group list
    /// 
    /// 2. Iterate through field list
    /// 
    /// 3. Set field Value if field equal to data Id.
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------

    public void SetFieldValue (
      String DataId,
      String Value )
    {
      //
      // Loop through group list
      //
      foreach ( Evado.Model.UniForm.Group group in this._Page.GroupList )
      {

        //
        // Loop through field loop 
        //

        foreach ( Field field in group.FieldList )
        {
          //
          // Making comparision betwewn filed Id and data Id
          //

          if ( field.FieldId == DataId )
          {
            //
            // Set value if field value is equal to data Id
            //


            field.Value = Value;
          }
        }//END field Iteration
      }//END group iteration
    }//END setFieldValue method

    // =====================================================================================
    /// <summary>
    /// This method update the contents of the page field value.
    /// </summary>
    /// <param name="DataId"> String: field Id</param> 
    /// <returns>String field value</returns>   
    /// <remarks>
    /// This method consists of following steps
    /// 
    /// 1. Iterate through group list
    /// 
    /// 2. Iterate through field list
    /// 
    /// 3. return field value as a string.
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public String GetFieldValue (
      String DataId )
    {
      //
      // Loop through group list
      //
      foreach ( Evado.Model.UniForm.Group group in this._Page.GroupList )
      {
        //
        // Loop through field loop 
        //
        foreach ( Field field in group.FieldList )
        {
          //
          // Making comparision betwewn filed Id and data Id
          //

          if ( field.FieldId == DataId )
          {
            //
            // Set value if field value is equal to data Id
            //


            return field.Value;
          }
        }//END field Iteration
      }//END group iteration

      return null;

    }//END setFieldValue method

    // =====================================================================================
    /// <summary>
    /// This method update the contents of the page field value.
    /// </summary>
    /// <param name="CommandId">Guid: command identifier</param>
    /// <returns>Evado.Model.UniForm.Command object</returns>   
    /// <remarks>
    /// This method consists of following steps
    /// 
    /// 1. Iterate through page command list
    /// 
    /// 2. Iterate through group list
    /// 
    /// 3. Iterate through command list
    /// 
    /// 4. return matching command object or null.
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public Command GetCommand (
      Guid CommandId )
    {
      //
      // Loop through group list
      //
      foreach ( Evado.Model.UniForm.Command command in this._Page.CommandList )
      {
        //
        // If a command id matches the passed commandId return the command.
        //
        if ( command.Id == CommandId )
        {
          return command;
        }
      }//END page command iteration loop

      //
      // Iterate through the page group list.
      //
      foreach ( Evado.Model.UniForm.Group group in this._Page.GroupList )
      {
        //
        // Iterate through the group command list
        //
        foreach ( Evado.Model.UniForm.Command command in group.CommandList )
        {
          //
          // If a command id matches the passed commandId return the command.
          //
          if ( command.Id == CommandId )
          {
            return command;
          }
        }//END field Iteration
      }//END group iteration

      return null;

    }//END GetCommand method

    #endregion

    #region Class output methods

    // =====================================================================================
    /// <summary>
    /// This method returns the contents of the page command.
    /// </summary>
    /// <returns>The contents of the page command.</returns>
    /// <remarks>
    /// This method consists of following stpes 
    /// 
    /// 1. Format stOutput String
    /// 
    /// 2. Return stOutput if Page and Page Exit are not equal to null
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public String getAtString ( )
    {

      //
      // Create a stOutput String
      //

      String stOutput = "AppData: Id: " + this._Id
       + "\r\n- SessionId: " + this._SessionId
       + "\r\n- ServiceUri: " + this._Url
       + "\r\n- Status: " + this._Status
       + "\r\n- Title: " + this._Title;

      //
      // Making comparision between Page and null
      //

      if ( this.Page != null )
      {
        stOutput += "\r\nPage Id: " + this.Page.Id
          + ", Page Title: " + this.Page.Title
          + ", Page Group count: " + this.Page.GroupList.Count;
        //
        // Making comparision between exit property of command class and null 
        //

        if ( this.Page.Exit != null )
        {
          stOutput += "\r\n Exit Command Title: " + this.Page.Exit.Title;
        }
      }

      return stOutput;
    }//END getAtString method

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
  }
}//END namespace