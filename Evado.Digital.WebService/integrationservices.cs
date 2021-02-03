/***************************************************************************************
 * <copyright file="Evado.Digital.WebService\IntegratedServices.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the Client Service functions.
 *
 ****************************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using System.Web;
using System.Web.SessionState;
using System.Web.Security;
using System.IO;
using System.Runtime.Remoting;

using Evado.Model.UniForm;
using Evado.Model;


namespace Evado.UniForm
{
  /// <summary>
  /// This class defines the device client server class, and contains the methods the web service 
  /// calls when a device client is attaching to the web service.
  /// </summary>
  public partial class IntegrationServices
  {
    #region class initialisation methods

    // ==================================================================================
    /// <summary>
    /// The class initialisation method.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public IntegrationServices ( )
    {

    }

    // ==================================================================================
    /// <summary>
    /// The class initialisation method that also initialises the classes core properties.
    /// </summary>
    /// <param name="ClientVersion">the version of the current client.</param>
    /// <param name="GlobalObjects">Hashtable: containing global objects.</param>
    /// <param name="SessionState">HttpSessionState object from the web service or layer.</param>
    /// <param name="ApplicationPath">The web services application path.</param>
    /// <param name="ServiceUserProfile">User id of the curretn user.</param>
    /// <param name="UniForm_BinaryFilePath">The path to binary files to be accessed by the device clients.</param>
    /// <param name="UniForm_ServiceBinaryUrl">The URL to access a binary files from a device client.</param>
    // ----------------------------------------------------------------------------------
    public IntegrationServices (
      float ClientVersion,
      Hashtable GlobalObjects,
      String ApplicationPath,
      EvUserProfileBase ServiceUserProfile,
      String UniForm_BinaryFilePath,
      String UniForm_ServiceBinaryUrl )
    {
      this._ClientVersion = ClientVersion;
      this._GlobalObjects = GlobalObjects;
      this._ServiceUserProfile = ServiceUserProfile;
      this._ApplicationPath = ApplicationPath;
      this._ServiceUserProfile = ServiceUserProfile;
      this._UniForm_BinaryFilePath = UniForm_BinaryFilePath;
      this._UniForm_BinaryServiceUrl = UniForm_ServiceBinaryUrl;

      this.LogInitMethod ( "Initialisation method. " );
      this.logInitValue ( "- ClientVersion: " + this._ClientVersion );
      this.logInitValue ( "- ServiceUserProfile.UserId: " + this._ServiceUserProfile.UserId );
      this.logInitValue ( "- ServiceUserProfile New Authentication: " + this._ServiceUserProfile.NewAuthentication );
      this.logInitValue ( "- GlobalObjects count: " + this._GlobalObjects.Count );
      this.logInitValue ( "- ApplicationPath: " + this._ApplicationPath );
      this.logInitValue ( "- UniForm_BinaryFilePath: " + this._UniForm_BinaryFilePath );
      this.logInitValue ( "- UniForm_BinaryServiceUrl: " + this._UniForm_BinaryServiceUrl );
      //
      // Reset  the exit method for the object.
      //
      this._ExitCommand = Command.getLogoutCommand ( );
 
      this._CommandHistory.DebugOn = true;
      //
      // Initialise the command history object.
      //
      this._CommandHistory = new CommandHistory (
        this._GlobalObjects,
        this._ServiceUserProfile );

      this.logInitValue ( "Command History list: "
        + this._CommandHistory.listCommandHistory ( true ) );

      foreach ( System.Collections.DictionaryEntry entry in this._GlobalObjects )
      {
        this.logInitValue ( "Item: " + entry.Key );
      }

    }//END Initialisation Method

    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class Application List Enumeration

    private string CONST_DEFAULT_ADAPTER = "DEFAULT";

    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class Global Objects

    const string CONST_NAME_SPACE = "Evado.UniForm.IntegrationServices.";

    private float _ClientVersion = Evado.Model.UniForm.AppData.API_Version;
    /// <summary>
    /// This property contains the client's API version.
    /// </summary>
    public float ClientVersion
    {
      get { return _ClientVersion; }
      set { _ClientVersion = value; }
    }

    //private float _ApiVersion = Evado.Model.UniForm.AppData.API_Version;

    private Hashtable _GlobalObjects = new Hashtable ( );

    /// <summary>
    /// This property contains a hash table for global objects.
    /// </summary>
    public Hashtable GlobalObjects
    {
      get { return _GlobalObjects; }
      set { _GlobalObjects = value; }
    }

    private String _ApplicationPath;

    /// <summary>
    /// This property passes in the application path to the class
    /// </summary>

    public String ApplicationPath
    {
      get { return this._ApplicationPath; }
      set { this._ApplicationPath = value; }
    }

    private String _UniForm_BinaryFilePath;

    /// <summary>
    /// This property passes in the binary file path to the class.
    /// </summary>

    public String BinaryFilePath
    {
      get { return this._UniForm_BinaryFilePath; }
      set { this._UniForm_BinaryFilePath = value; }
    }

    private String _UniForm_BinaryServiceUrl;

    /// <summary>
    /// This property passes in the binary file path the class.
    /// </summary>
    public String BinaryServiceUrl
    {
      get { return this._UniForm_BinaryServiceUrl; }
      set { this._UniForm_BinaryServiceUrl = value; }
    }


    private String _EventLogSource = "Evado UniFORM";
    /// <summary>
    /// This property passes in the Event Source name.
    /// </summary>
    public String EventLogSource
    {
      get { return this._EventLogSource; }
      set { this._EventLogSource = value; }
    }

    //
    // This global object contains the users profile for access the UniFORM environment.
    //
    private EvUserProfileBase _ServiceUserProfile = new EvUserProfileBase ( );

    //
    // This global object contains the user's navigation history for navigating the page hierarchy.
    //
    private CommandHistory _CommandHistory = new CommandHistory ( );

    //
    // This object contains the client application data object for the implementation
    //
    private AppData _ClientDataObject = new AppData ( );

    //
    // This object contains the exit groupCommand to be added to the Client Application Data object prior 
    // sending the data object to the device client. 
    //
    private Command _ExitCommand = new Command ( );

    //
    // This variable containt the error message to be displayed on the device client when an error event occurs 
    // in the web or application service.
    //
    private String _ErrorMessage = String.Empty;

    //
    // The eclinical application service.
    //
    Evado.UniForm.Digital.EuAdapter _ApplicationAdapter =
          new Evado.UniForm.Digital.EuAdapter ( );

    private bool _NewUser = false;

    // 
    // Status stores the debug status information.
    // 
    private StringBuilder _ApplicationLog = new StringBuilder ( );

    /// <summary>
    /// This property returns the client service's Debuglog contents to the web service.
    /// </summary>
    public String Log
    {
      get { return this._ApplicationLog.ToString ( ); }
    }

    private int _LoggingLevel = 0;

    /// <summary>
    /// This property sets the debug state for the class.
    /// </summary>
    public int LoggingLevel
    {
      get { return _LoggingLevel; }
      set
      {
        this._LoggingLevel = value;

        if ( this._LoggingLevel < 0 )
        {
          this._LoggingLevel = 0;
        }
        if ( this._LoggingLevel > 5 )
        {
          this._LoggingLevel = 5;
        }
        if ( this._LoggingLevel < 1 )
        {
          this._ApplicationLog = new StringBuilder ( );
        }
      }
    }

    public const string ApplicationId = "Default";

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Public class Methods

    // ==================================================================================
    /// <summary>
    /// This method returns the application data object to be processed by the client.
    /// </summary>
    /// <param name="PageCommand">ClientPageCommand object: containing the groupCommand that 
    /// is called on web service</param>
    /// <returns>ClientApplicationData</returns>
    // ----------------------------------------------------------------------------------
    public AppData getPageObject ( Command PageCommand )
    {
      this.LogMethod ( "getPageObject method, " );
      this.LogValue ( "NewUser: " + this._NewUser );
      this.LogValue ( "PageCommand: " + PageCommand.getAsString ( false, false ) );
      try
      {
        this._CommandHistory.DebugOn = this.DebugOn;

        // If the user is new and there is a non home page groupCommand 
        // execute that groupCommand.
        //
        if ( this._NewUser == true )
        {
          this.LogValue ( "new user retreiving the last command" );
          List<Evado.Model.UniForm.Parameter> header = PageCommand.Header;

          Command lastCommand = this._CommandHistory.getLastCommand ( );
          this.LogValue ( "last command: " + lastCommand.Title );

          if ( lastCommand.Id != Guid.Empty
            && lastCommand.Object != Evado.UniForm.Digital.EuAdapterClasses.Home_Page.ToString ( ) )
          {
            this.LogValue ( "Executing the last command" );
            PageCommand = lastCommand;

            PageCommand.Header = header;
          }

        }//END new user getting the last history command.

        this.LogValue ( "AFTER NEW USER: PageCommand: " + PageCommand.getAsString ( false, false ) );

        //
        // Initialise the methods variables and objects.
        //
        this._ErrorMessage = String.Empty;

        this.LogValue ( "Short Title: " + PageCommand.GetParameter (CommandParameters.Short_Title ) );

        //
        // Set the groupCommand to a short title to it can be added to the groupCommand history.
        //
        PageCommand.setShortTitle ( );

        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        #region Save and Delete processes.
        //
        // If the method is to update a value then we need to undertake process processing after 
        // the method has been processed to return the user to the exit page.

        if ( PageCommand.Method == ApplicationMethods.Delete_Object
          || PageCommand.Method == ApplicationMethods.Save_Object )
        {
          this.LogMethod ( "SAVE OR DELETE METHODS." );

          //
          // The save process returns a empty data object with the exit groupCommand.
          // Retrieve the groupCommand and direct the user to that page.
          //
          this._ClientDataObject = this.getObjectData ( PageCommand );

          this.LogValue ( "Application.ID: " + this._ClientDataObject.Id + ", Page.Id: " + this._ClientDataObject.Page.Id );

          if ( this._ClientDataObject.Message != String.Empty )
          {
            this.LogValue ( "Error output" );

            this.LogValue ( "AppDataObject.Message: " + this._ClientDataObject.Message );

            return this._ClientDataObject;
          }

          //
          // Get the list page groupCommand as the default exit groupCommand.
          //
          this._ExitCommand = this._CommandHistory.getExitCommand ( PageCommand );

          this.LogValue ( "Save ExitCommand: " + this._ExitCommand.getAsString ( false, false ) );

          //
          // Set the page groupCommand to the saved objects exit groupCommand.  To call the correct exit page.
          //
          PageCommand = this._ExitCommand;

          this.LogValue ( "OK: next page command: " + PageCommand.getAsString ( false, false ) );

          //
          // out the error message to be added to the returning data object.
          //
          this._ErrorMessage = this._ClientDataObject.Message;

          this.LogValue ( "ErrorMessage: " + this._ErrorMessage );

          this.LogMethod ( "END SAVE OR DELETE METHODS." );

        }//END process save or delete object commands.

        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        #endregion

        //
        this.LogMethod ( "GET LIST, GET OR CREATE METHOD" );
        //
        // Get the list page groupCommand as the default exit groupCommand.
        //
        this._ExitCommand = this._CommandHistory.getExitCommand ( PageCommand );

        //this.LogDebugValue ( this._CommandHistory.Log );

        this.LogValue ( "ExitCommand: " + this._ExitCommand.getAsString ( false, false ) );

        //
        // get the data objec using the current page groupCommand.
        //
        this._ClientDataObject = this.getObjectData ( PageCommand );

        this._ErrorMessage = this._ClientDataObject.Message;

        this.LogValue ( "RETURNED: ErrorMessage: " + this._ErrorMessage );

        //
        // An empty Guid indicates a error has occured and return to the home page.
        //
        if ( this._ClientDataObject.Id == Guid.Empty )
        {
          return getDefaultPage ( PageCommand );
        }
      }
      catch ( Exception Ex )
      {
        EvApplicationEvents.LogError ( this._EventLogSource,
          Evado.Model.EvStatics.getException ( Ex ) );

        this.LogValue ( "Evado.UniForm.IntegrationServices.getPageObject method." );
        this.LogValue ( "Exception Event. " + Evado.Model.EvStatics.getException ( Ex ) );
      }

      this.LogValue ( "EXIT INTEGRATION SERVICE CLASS " );

      //
      // Return the client data object to the client.
      //
      return this._ClientDataObject;

    }//END getPageObject Method

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Private class methods

    // ==================================================================================
    /// <summary>
    /// This method selects the application object to be be processed.
    /// </summary>
    /// <param name="PageCommand">ClientPageCommand object: containing the groupCommand that 
    /// is called on web service</param>
    /// <returns>ClientApplicationData</returns>
    // ----------------------------------------------------------------------------------
    private AppData getObjectData (
      Command PageCommand )
    {
        this.LogMethod ( "getDataObject method. " );
        this.LogValue ( "ClientVersion: " + this._ClientVersion );
        this.LogValue ( "PageCommand " + PageCommand.getAsString ( false, false ) );
        this.LogValue ( "Exit Command: " + this._ExitCommand.getAsString ( false, false ) );
      //
      // Initialise the methods variables and objects.
      //
        AppData clientDataObject = new AppData ( );
          
      try
      {
        //
        // Initialise the Evado clinical adapter object.
        //
        this._ApplicationAdapter = new Evado.UniForm.Digital.EuAdapter (
          this._ClientVersion,
          this._GlobalObjects,
          this._ServiceUserProfile,
          this._ExitCommand,
          this._ApplicationPath,
          this._UniForm_BinaryFilePath,
          this._UniForm_BinaryServiceUrl );

        //
        // define the licensed modules for this installation.
        //
        String licensedModules = Evado.Model.Digital.EdModuleCodes.Administration_Module + ";"
         + Evado.Model.Digital.EdModuleCodes.Management_Module + ";"
         + Evado.Model.Digital.EdModuleCodes.Imaging_Module + ";"
         + Evado.Model.Digital.EdModuleCodes.Integration_Module;

        this._ApplicationAdapter.LicensedModules = licensedModules;

        this._ApplicationAdapter.LoggingLevel = this.LoggingLevel;

        this.LogValue ( "ApplicationAdapter.LoggingLevel: " + this._ApplicationAdapter.LoggingLevel );

        this.LogValue ( "PageCommand.Method: " + PageCommand.Method );
        //
        // if the page is a custom groupCommand reset it to the customcommang parameter if it not null
        //
        if ( PageCommand.Method == ApplicationMethods.Custom_Method
          && PageCommand.getCustomMethod ( ) != ApplicationMethods.Null )
        {
          PageCommand.Method = PageCommand.getCustomMethod ( );

          this.LogValue ( "ApplicationMethod reset to " + PageCommand.Method.ToString ( ) );
        }

        //
        // If not found create a new object and add it to the list then return it.
        //  
        switch (  PageCommand.ApplicationId )
        {
          case Evado.UniForm.Digital.EuAdapter.ADAPTER_ID:
            {
              this.LogValue ( "SELECTING APPLICATION AppId: '" + PageCommand.ApplicationId + "'"
                + " >> CALLING ECLINICAL APPLICATION" );
              //
              // Call the get page method to generate the next page.
              //
              clientDataObject = this._ApplicationAdapter.getPageObject ( PageCommand );

              if ( this._ErrorMessage != String.Empty )
              {
                clientDataObject.Message = this._ErrorMessage;
              }


              this._GlobalObjects = this._ApplicationAdapter.GlobalObjectList;

              this.LogApplication ( this._ApplicationAdapter.AdapterLog );

              this.LogValue ( "IntegrationServices.GlobalObject count: " + this._GlobalObjects.Count );

              this.LogValue ( "RETURN APPLICATION DATA OBJECT " );
              this.LogValue ( " - ID: " + clientDataObject.Id );
              this.LogValue ( " - Title: " + clientDataObject.Title );
              if ( clientDataObject.Page.Exit != null )
              {
                this.LogValue ( " - ExitCommand: " + clientDataObject.Page.Exit.getAsString ( false, false ) );
              }
              this.LogValue ( " - Message: " + clientDataObject.Message );

              //
              // Return the instance to the list.initialisation method
              //
              return clientDataObject;
            }
          default:
            {
              this.LogValue ( ">> GETTING DEFAULT PAGE. " );

              //
              // get the default page
              //
              return this.getDefaultPage ( PageCommand );
            }
        }
      }
      catch ( Exception Ex )
      {
        EvApplicationEvents.LogError ( this._EventLogSource,
          Evado.Model.EvStatics.getException ( Ex ) );

        this._ErrorMessage = "UniFORM service application error.";
      }

      return getDefaultPage ( PageCommand );

    }//END CallApplicationObject Method

    // ==================================================================================
    /// <summary>
    /// This method returns the Edit Enabled Test page. - readon only 
    /// 
    /// </summary>
    /// <returns>ClientApplicationData</returns>
    // ----------------------------------------------------------------------------------
    private void fixIdentifiers ( Page PageObject )
    {
      PageObject.Id = Guid.NewGuid ( );

      //
      // reset the groupCommand guids and field identifiers.
      //
      for ( int grpCount = 0; grpCount < PageObject.GroupList.Count; grpCount++ )
      {
        Group group = PageObject.GroupList [ grpCount ];
        group.Id = Guid.NewGuid ( );

        for ( int commandIndex = 0; commandIndex < group.CommandList.Count; commandIndex++ )
        {
          Command command = group.CommandList [ commandIndex ];
          command.Id = Guid.NewGuid ( );
        }

        //
        // Substitute the //ServiceBinaryUrl/ term with the services binary url and reset the field identifiers.
        //
        if ( group.Description != null )
        {
          string description = group.Description;
          description = description.Replace ( "//ServiceBinaryUrl/", this._UniForm_BinaryServiceUrl );
          group.Description = description;
        }

        for ( int fldCount = 0; fldCount < group.FieldList.Count; fldCount++ )
        {
          Field field = group.FieldList [ fldCount ];

          field.Id = Guid.NewGuid ( );
          if ( field.FieldId == String.Empty )
          {
            field.FieldId = "Fld-" + grpCount + "-" + fldCount;
          }
          field.Value = field.Value.Replace ( "//ServiceBinaryUrl/", this._UniForm_BinaryServiceUrl );
        }

      }
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Default Page Methods

    // ==================================================================================
    /// <summary>
    /// This method returns the default application data object1.
    /// 
    /// </summary>
    /// <returns>ClientApplicationData</returns>
    /// <param name="PageCommand">Evado.Model.UniForm.Command PageCommand</param>
    /// <returns>ClientApplicationData object</returns>
    // ----------------------------------------------------------------------------------
    private AppData getDefaultPage (
        Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getDefaultPage method. " );
      //
      // Initialise the methods variables and objects.
      //
     AppData clientDataObject = new AppData ( );

      Command homePageCommand = Command.getDefaultCommand ( );

      homePageCommand.Header = PageCommand.Header;

      this.LogDebugValue( "Default Home page command: " + homePageCommand.getAsString( true, false ) );

      //
      // Call the get page method to generate the next page.
      //
      clientDataObject = this._ApplicationAdapter.getPageObject ( homePageCommand );

      this.LogApplication ( this._ApplicationAdapter.AdapterLog );

      //
      // fix the page identifier to enuser they are unique
      //
      this.fixIdentifiers ( clientDataObject.Page );

      //
      // Set the home groupCommand id to be the same all of the time.
      //
      homePageCommand.Id = this._ClientDataObject.Id;

      //
      // Initialise the groupCommand history list.
      //
      this._CommandHistory.initialiseHistory ( homePageCommand );

      //
      // Set the error message if it exists.
      //
      clientDataObject.Message = this._ErrorMessage;

      //
      // Reset  the exit method for the object.
      //
      clientDataObject.Page.Exit = this._ExitCommand;


      this.LogDebugValue ( "PageObject:" );
      this.LogDebugValue ( "-ID: " + clientDataObject.Id );
      this.LogDebugValue ( "-Title: " + clientDataObject.Title );
      if ( clientDataObject.Page.Exit != null )
      {
        this.LogDebugValue ( "-ExitCommand: " + clientDataObject.Page.Exit.getAsString ( false, false ) );
      }
      this.LogDebugValue ( "-Message: " + clientDataObject.Message );
      //
      // return the application object.
      //
      this.LogMethodEnd ( "getDefaultPage" );
      return clientDataObject;

    }//END getDefaultPage

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Application Logging methods.

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogInitMethod ( String Value )
    {
      this._ApplicationLog.AppendLine ( Evado.Model.EvStatics.CONST_METHOD_START
      + DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": "
      + IntegrationServices.CONST_NAME_SPACE + Value );
    }
    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void logInitValue ( String Value )
    {
      this._ApplicationLog.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " + Value );
    }

    // ==================================================================================
    /// <summary>
    /// This method appends EVENT to the class log
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Ex">Exception object.</param>
    // ----------------------------------------------------------------------------------
    public void LogException ( Exception Ex )
    {
      String value = "NameSpace: " + IntegrationServices.CONST_NAME_SPACE
       + "\r\n" + EvStatics.getException ( Ex );

      if ( _LoggingLevel >= 0 )
      {
        this._ApplicationLog.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + " EXCEPTION EVENT: " );
        this._ApplicationLog.AppendLine ( value );
      }
    }//END LogException class


    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogMethod ( String Value )
    {
      if ( _LoggingLevel > 1 )
      {
        this._ApplicationLog.AppendLine ( Evado.Model.EvStatics.CONST_METHOD_START
        + DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": "
        + IntegrationServices.CONST_NAME_SPACE + Value );
      }
    }
    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="MethodName">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogMethodEnd ( String MethodName )
    {
      if ( this.LoggingLevel >= 1 )
      {
        String value = Evado.Model.EvStatics.CONST_METHOD_END;

        value = value.Replace ( " END OF METHOD ", " END OF " + MethodName + " METHOD " );

        this._ApplicationLog.AppendLine ( value );
      }
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogValue ( String Value )
    {
      if ( _LoggingLevel > 2 )
      {
        this._ApplicationLog.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " + Value );
      }
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogValue4 ( String Value )
    {
      if ( _LoggingLevel > 3 )
      {
        this._ApplicationLog.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " + Value );
      }
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogDebugValue ( String Value )
    {
      if ( _LoggingLevel > 4 )
      {
        this._ApplicationLog.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " + Value );
      }
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    private bool DebugOn
    {
      get
      {
        if ( _LoggingLevel == 5 )
        {
          return true;
        }
        return false;
      }
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogApplication ( String Value )
    {
      if ( _LoggingLevel > 2 )
      {
        this._ApplicationLog.AppendLine ( Value );
      }
    }


    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
  }
}//END namespace