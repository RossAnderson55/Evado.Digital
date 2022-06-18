/***************************************************************************************
 * <copyright file="Evado.Digital.WebService\WcfRestService.cs" company="EVADO HOLDING PTY. LTD.">
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
 * Description: 
 *  This class contains the Client Service functions.
 *
 ****************************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

//Evado. namespace references.
using Evado.UniForm.Model;
using Evado.Model;
//using Evado.UniForm.Dal;
using Evado.UniForm;
//using Evado.UniForm.Web;

namespace Evado.Digital.WebService
{
  // 
  // Start the service and browse to http://<machine_name>:<port>/Service1/help to view the service's generated help page
  // NOTE: By default, a new instance of the service is created for each call; change the InstanceContextMode to Single if you want
  // a single instance of the service to process all calls.	
  //
  [ServiceContract]
  [AspNetCompatibilityRequirements ( RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed )]
  [ServiceBehavior ( InstanceContextMode = InstanceContextMode.PerSession )]
  // NOTE: If the service is renamed, remember to update the global.asax.cs file

  /// <summary>
  /// This method is the evado eClinical web serivce class
  /// </summary>
  public class ClientService
  {
    #region Class initialisation methods
    //  ===================================================================================
    /// <summary>
    /// This method initialises the service and sets the context object for the users session.
    /// </summary>
    //  -----------------------------------------------------------------------------------
    public ClientService ( )
    {
      this._Context = HttpContext.Current;

      //Global.deleteOldGlobalObjects ( );

    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class Global objects and values.

    //
    // Constants
    //
    public const string CONST_VALIDATION_STATE = "UserValidationState";
    public const string CONST_LOGIN_COUNT = "LoginCount";
    private const string CONST_USER_PROFILE_SUFFIX = "_UPF";


    /// <summary>
    /// This object contains the HTTP context for the connection to the web client.
    /// </summary>
    private HttpContext _Context;

    /// <summary>
    /// This private static class interfaces to the backend.
    /// </summary>
    private IntegrationServices _IntegrationServices = new IntegrationServices ( );

    /// <summary>
    /// This private class is the object passed to the client.
    /// </summary>
    private Evado.UniForm.Model.EuAppData _AppDataObject = new Evado.UniForm.Model.EuAppData ( );

    /// <summary>
    /// this variable contains the the user's profile.
    /// </summary>
    private EvUserProfileBase _ServiceUserProfile = new EvUserProfileBase ( );

    /// <summary>
    /// This global parameter contains the current url.
    /// </summary>
    private String _CurrentServiceURL = String.Empty;

    private float _ClientVersion = Evado.UniForm.Model.EuAppData.API_Version;

    private float _ServerVersion = Evado.UniForm.Model.EuAppData.API_Version;

    private DateTime _StartTime = DateTime.Now;

    private String _CommandTitle = String.Empty;

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region POST method used in this implementation.

    //  =================================================================================
    /// <summary>
    /// This method is executes the Evado eClinical REST web service 
    /// </summary>
    /// <param name="ClientVersion">verson parameter</param>
    /// <param name="command">groupCommand parameter</param>
    /// <param name="SessionId">session identifier</param>
    /// <param name="content">content</param>
    /// <returns>json object.</returns>
    //  ---------------------------------------------------------------------------------
    [WebInvoke ( UriTemplate = "/{ClientVersion}?command={command}&session={SessionId}", Method = "POST" )]
    public Stream getPageObject ( string ClientVersion, string command, string SessionId, Stream content )
    {
      Global.ClearApplicationLog ( );
      try
      {
        this.LogMethod ( "getPageObject web service method." );
        this.LogValue ( "ServerVersion: " + this._ServerVersion );
        this.LogValue ( "URL sessionId: " + SessionId );
        this.LogValue ( "Context SessionId: " + this._Context.Session.SessionID );
        this.LogValue ( "CurrentServiceURL: " + this._CurrentServiceURL );
        this.LogValue4 ( "URLClientVersion: " + ClientVersion );
        this.LogValue4 ( "LoggingLevel: " + Global.LoggingLevel );
        //
        // Initialise the methods variables and object.
        //
        string json = String.Empty;
        Evado.UniForm.Model.EuCommand PageCommand = new Evado.UniForm.Model.EuCommand ( );
        this._ClientVersion = Evado.UniForm.Model.EuStatics.decodeMinorVersion (
          ClientVersion );
        this.LogValue4 ( "ClientVersion: " + this._ClientVersion );

        //
        // Set the current url.
        //
        this._CurrentServiceURL = this._AppDataObject.Url;

        if ( this._CurrentServiceURL == String.Empty )
        {
          //
          // Extract the root url of the web service and return it to the device.
          //
          this._CurrentServiceURL = this._Context.Request.Url.AbsoluteUri;
          int restIndex = this._CurrentServiceURL.IndexOf ( Global.APPLICATION_SERVICE_ROOT );
          if ( restIndex > 0 )
          {
            this._CurrentServiceURL = this._CurrentServiceURL.Substring ( 0, restIndex );
          }
        }

        if ( Global.UniForm_BinaryServiceUrl.Contains ( "./temp/" ) == true )
        {
          Global.UniForm_BinaryServiceUrl = this._CurrentServiceURL + Global.APPLICATION_BINARY_FILE_ROOT;
        }

        //
        // first load the POST payload into a string
        // the POST content comes from the content param above
        // as it is the only param that is not listed in the URI template
        //
        string content_value = new StreamReader ( content ).ReadToEnd ( );

        //
        // Log the transaction
        //

        this.LogValue4 ( "Global Object count: " + Global.GlobalObjectList.Count
        + ", " + Evado.UniForm.Model.EuStatics.GLOBAL_CLINICAL_OBJECT + " exists: "
        + Global.GlobalObjectList.ContainsKey ( Evado.UniForm.Model.EuStatics.GLOBAL_CLINICAL_OBJECT ) );
        //this.LogDebugValue ( "JSON CONTENT: \r\n" + content_value );

        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        #region Step 1 set session and deserialise json
        //
        // Deserialise the commandobjects.
        // 
        this.LogEvent ( "STEP 1: Deserializing the json command object." );

        //
        // if content value is empty then send a login request.
        //
        if ( content_value == String.Empty )
        {
          this.LogValue4 ( "content value is empty. Send a login request." );

          //
          // Initiate requesting a login from the user.
          //
          this._AppDataObject = this.requestUserLogin ( String.Empty );

          //
          //  send the web service response to the device app.
          //
          return this.generateWebServiceResponse ( );
        }

        //
        // attempt to convert this JSON payload to a C# structure
        try
        {
          content_value = content_value.Replace ( "Title\":\" - ", "Title\":\"" );
          //
          // serialise the client groupCommand
          //
          PageCommand = JsonConvert.DeserializeObject<Evado.UniForm.Model.EuCommand> ( content_value );
        }
        catch ( Exception Ex )
        {
          String EventMessage = Evado.Model.EvmLabels.JSON_DESERIALISATION_ERROR + "\r\n" + EvStatics.getException ( Ex );

          this.LogEvent ( EventMessage );

          return generateErrorWebServiceResponse ( Ex );
        }

        this._CommandTitle = PageCommand.Title;
        Global.OutputApplicationLog ( );

        Global.OutputEventLog ( );


        #endregion

        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        #region STEP 2 : Validating the groupCommand object.

        this.LogEvent ( "STEP 2: Validating the command object." );
        //
        // If an invalid groupCommand object is recieved send a login request.
        //
        if ( PageCommand == null )
        {
          this.LogEvent ( "COMMAND NULL FAILED: return login request." );

          //
          // Initiate requesting a login from the user.
          //
          this._AppDataObject = this.requestUserLogin ( String.Empty );

          //
          //  send the web service response to the device app.
          //
          return this.generateWebServiceResponse ( );
        }

        this.LogDebug ( "PageCommand: " + PageCommand.getAsString ( false, false ) );

        Global.LogCommand ( PageCommand, this._Context.Session.SessionID );

        //
        // Log the user transaction
        //
        this.logUserTransaction ( PageCommand, ClientVersion, SessionId );

        //
        // Retrieve the service user profile.
        //
        this.getServiceUserProfile ( PageCommand );

        this.LogDebug ( "ServiceUserProfile.UserId: " + this._ServiceUserProfile.UserId );
        //this.LogDebugValue ( "ServiceUserProfile.DomainGroupNames: " + this._ServiceUserProfile.DomainGroupNames );
        this.LogDebug ( "ServiceUserProfile.IsAuthenticated: " + this._ServiceUserProfile.IsAuthenticated );

        #endregion

        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        #region Step 3: User Logout request

        this.LogEvent ( "STEP 3: Processing logout request." );

        //
        // log out the user.  
        //Evado.UniForm.Model.EuCommandTypes.Null command type is required for the mobile device client.
        //
        if ( PageCommand.Type ==Evado.UniForm.Model.EuCommandTypes.Logout_Command
          || PageCommand.Type ==Evado.UniForm.Model.EuCommandTypes.Null )
        {
          string stLogMessage = "Log out the user and force a user authentication.";

          this.LogEvent ( stLogMessage );

          //
          // Initiate requesting a login from the user.
          //
          this._AppDataObject = this.requestUserLogin ( String.Empty );


          //
          //  send the web service response to the device app.
          //
          var response = this.generateWebServiceResponse ( );

          //
          // for logout delete all session variables.
          //
          this._Context.Session [ this._ServiceUserProfile.UserId.ToUpper ( ) ] = null;

          //
          // return the result
          //
          return response;

        }//END log out request

        #endregion

        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        #region Step 4: Check Session Status

        this.LogEvent ( "STEP 4:  Check User Session Status." );

        this.LogValue4 ( "UserValidated: " + this._ServiceUserProfile.AuthenticationState );
        this.LogValue4 ( "PageCommand.Type: " + PageCommand.Type );

        //
        // if a new session is created then force a login transaction.
        //
        if ( ( this._Context.Session.IsNewSession == true )
          && ( PageCommand.Type !=Evado.UniForm.Model.EuCommandTypes.Login_Command )
          && ( PageCommand.Type !=Evado.UniForm.Model.EuCommandTypes.Network_Login_Command )
          && ( PageCommand.Type !=Evado.UniForm.Model.EuCommandTypes.Anonymous_Command ) )
        {
          string stLogMessage = "NewSession: " + this._Context.Session.IsNewSession
            + "\r\nThe session has changed SessionID: " + this._Context.Session.SessionID
            + " Transaction Session: " + SessionId + " force a user authentication.";

          this.LogValue4 ( "USER SESSION ID CHANGED: " + stLogMessage );

          this.LogEvent ( stLogMessage );

          this.LogValue ( "USER SESSION: User: " + this._ServiceUserProfile + " session expired"
            + " old session: " + SessionId + " new session: " + this._Context.Session.SessionID );

          //
          // Initiate requesting a login from the user.
          //
          this._AppDataObject = this.requestUserLogin ( String.Empty );
          this._AppDataObject.Page.Exit = new Evado.UniForm.Model.EuCommand ( );

          //
          //  send the web service response to the device app.
          //
          return this.generateWebServiceResponse ( );

        }//END session change.

        #endregion

        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        #region Step 5: Validate user credentials

        this.LogEvent ( "STEP 5: Validating the user's credentials." );
        this.LogDebug ( "UserAuthenticationState: {0} ", this._ServiceUserProfile.AuthenticationState );

        //
        // Revalidate the user if the command type is a re-authenticatin command.
        if ( PageCommand.Type ==Evado.UniForm.Model.EuCommandTypes.Re_Authentication_Command )
        {
          //
          // Skip validation if an anonymous command is received.
          //
          if ( this.ReValidateUserCredentials ( PageCommand ) == false )
          {
            this.LogEvent ( "USER AUTHENTICATION: User: " + this._ServiceUserProfile + " authentication failed" );
            //
            // Initiate requesting a login from the user.
            //
            this._AppDataObject = this.requestUserLogin ( Evado.Model.EvmLabels.AUTHENTICATION_ERROR_MESSAGE );

            //
            //  send the web service response to the device app.
            //
            return this.generateWebServiceResponse ( );

          }//END login in comment request
        }
        else
        {
          //
          // Skip validation if an anonymous command is received.
          //
          if ( this.validateUserCredentials ( PageCommand ) == false
            || this._ServiceUserProfile.AuthenticationState == EvUserProfileBase.UserAuthenticationStates.Not_Authenticated )
          {
            this.LogEvent ( "USER AUTHENTICATION: User: " + this._ServiceUserProfile + " authentication failed" );
            //
            // Initiate requesting a login from the user.
            //
            this._AppDataObject = this.requestUserLogin ( Evado.Model.EvmLabels.AUTHENTICATION_ERROR_MESSAGE );

            //
            //  send the web service response to the device app.
            //
            return this.generateWebServiceResponse ( );

          }//END login in comment request
        }

        if ( PageCommand.Type ==Evado.UniForm.Model.EuCommandTypes.Login_Command )
        {
          PageCommand.Title = Evado.Model.EvmLabels.HomePage_Command_Title;
          PageCommand.Type =Evado.UniForm.Model.EuCommandTypes.Normal_Command;
          PageCommand.Method = Evado.UniForm.Model.EuMethods.Get_Object;
        }

        // this.LogDebugValue ( "PageCommand: " + PageCommand.getAsString ( false, true ) );

        this.LogDebug ( "ServiceUserProfile.UserId: " + this._ServiceUserProfile.UserId );
        //this.LogDebugValue ( "ServiceUserProfile.DomainGroupNames: " + this._ServiceUserProfile.DomainGroupNames );
        this.LogDebug ( "ServiceUserProfile.IsAuthenticated: " + this._ServiceUserProfile.IsAuthenticated );
        #endregion

        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        #region STEP 6 : Validate device

        this.LogEvent ( "STEP 6: Validate device and register device." );

        //
        // validate whether the device is registered.
        //
        /*
        if ( this.validateDeviceRegistration ( PageCommand ) == false )
        {
          this._DeviceValidated = this.registerDevice ( PageCommand );

          this.LogValue ( "DEVICE VALIDATION: Device Id: "
            + PageCommand.GetHeaderValue ( CommandHeaderElements.DeviceId ) + " not registered. " );
        }
        */
        #endregion

        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        #region Step 7: Process the user's groupCommand request

        this.LogEvent ( "STEP 7: Process the user's command request." );

        //
        // Set the device client service session state.
        //
        this._IntegrationServices = new IntegrationServices (
          this._ClientVersion,
          Global.GlobalObjectList,
          Global.ApplicationPath,
          this._ServiceUserProfile,
          Global.UniForm_BinaryFilePath,
          Global.UniForm_BinaryServiceUrl );

        this._IntegrationServices.LoggingLevel = Global.LoggingLevel;
        this._IntegrationServices.EventLogSource = Global.EventLogSource;

        //
        // If the ApplicationId is empty then reset it to the default.
        //
        if ( PageCommand.ApplicationId == String.Empty )
        {
          PageCommand.ApplicationId = IntegrationServices.ApplicationId;
          PageCommand.Method = Evado.UniForm.Model.EuMethods.Get_Object;
        }

        this._IntegrationServices.ClientVersion = this._ClientVersion;
        //
        // Turns on debug logging if necessary.
        //
        this._IntegrationServices.LoggingLevel = Global.LoggingLevel;

        //
        // Call the client service class to process the groupCommand object.
        //
        this._AppDataObject = this._IntegrationServices.getPageObject ( PageCommand );

        //this.fixNumericValidation ( );

        this.LogClass ( this._IntegrationServices.Log );

        this.LogEvent ( "Integration Services RETURNED." );

        this.LogDebug ( this._AppDataObject.getAtString ( ) );

        if ( Global.LoggingLevel > 4 )
        {
          //this.LogDebugValue ( "Writting out app data object as xml file." );

          //String filename = "debug-"+ this._AppDataObject.Page.Title.Replace ( " ", "-" ) + ".cado.xml";
          //filename = filename.Replace ( ":", "" );
          //this.LogDebugValue ( "Filename: " + filename  );

          //EvStatics.Files.saveFile<Evado.UniForm.Model.EuAppData> ( Global.TempPath, filename, this._AppDataObject );
        }

        //
        // Log the page test cases.
        //
        Global.RecordTestCase ( PageCommand, this._AppDataObject.Page.Exit, this._AppDataObject );

        this.LogDebug ( "Global Object count: " + Global.GlobalObjectList.Count );
        this.LogDebug ( Evado.UniForm.Model.EuStatics.GLOBAL_CLINICAL_OBJECT + " exists: "
        + Global.GlobalObjectList.ContainsKey ( Evado.UniForm.Model.EuStatics.GLOBAL_CLINICAL_OBJECT ) );

        this.LogDebug ( "Client data object: " + this._AppDataObject.Title
          + " send to user: " + this._ServiceUserProfile.UserId );

        Global.LogExitCommand ( this._AppDataObject.Page.Exit, this._Context.Session.SessionID );

        if ( PageCommand.Method == Evado.UniForm.Model.EuMethods.Save_Object )
        {
          Global.OutputApplicationLog_Save ( );
        }

        //
        //  send the web service response to the device app.
        //
        return this.generateWebServiceResponse ( );

        #endregion

      }
      catch ( Exception Ex )
      {
        string EventMessage = "Evado.Digital.WebService.ClientService event method.\r\n" + EvStatics.getException ( Ex );

        this.LogEvent ( EventMessage );

        return generateErrorWebServiceResponse ( Ex );

      } // Close catch   

    }//END Create web method. RecordTestCases

    //===================================================================================
    /// <summary>
    /// This method fixes the numeric validation for mobile devices.
    /// Changing Min_Value to MinValue 
    /// </summary>
    //-----------------------------------------------------------------------------------
    private void fixNumericValidation ( )
    {
      //
      // Get the group list.
      //
      List<Evado.UniForm.Model.EuGroup> groupList = this._AppDataObject.Page.GroupList;

      //
      // Iterate through the groups.
      //
      foreach ( Evado.UniForm.Model.EuGroup group in groupList )
      {
        //
        // Iterate through the group field list.
        //
        for ( int count = 0; count < group.FieldList.Count; count++ )
        {
          Evado.UniForm.Model.EuField field = group.FieldList [ count ];

          if ( field.Type == EvDataTypes.Numeric )
          {
            field.FixNumericValidation ( );
          }
        }
      }
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Web Service and Session Methods.

    // =====================================================================================
    /// <summary>
    /// This method generates the Web service request to be sent to the device app client.
    /// </summary>
    /// <returns>Stream: of the content to be sent to the device client.</returns>
    // -------------------------------------------------------------------------------------
    public Stream generateWebServiceResponse ( )
    {
      //
      // Initialise the methods variables and object.
      //
      this.LogMethod ( "generateWebServiceResponse" );
      string json = String.Empty;
      JsonSerializerSettings jsonSettings = new JsonSerializerSettings
        {
          NullValueHandling = NullValueHandling.Ignore
        };

      //
      // Add the session id
      //
      this._AppDataObject.SessionId = this._Context.Session.SessionID;
      this._AppDataObject.Url = this._CurrentServiceURL;

      if ( this._AppDataObject.Status == Evado.UniForm.Model.EuAppData.StatusCodes.Null
        && ( this._ServiceUserProfile.AuthenticationState == EvUserProfileBase.UserAuthenticationStates.Authenticated
          || this._ServiceUserProfile.AuthenticationState == EvUserProfileBase.UserAuthenticationStates.Anonymous_Access ) )
      {
        this._AppDataObject.Status = Evado.UniForm.Model.EuAppData.StatusCodes.Login_Authenticated;
      }

      // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      // 
      this.LogDebug ( " SERIALISING the Data Object " );

      json = JsonConvert.SerializeObject ( this._AppDataObject, Formatting.Indented, jsonSettings );

      //
      // Write the log
      //
      if ( Global.LoggingLevel > 0 )
      {
        Evado.Model.EvStatics.Files.saveFile ( Global.TempPath, @"jsonData.txt", json );
      }
      if ( Global.LoggingLevel > 0 )
      {
        string stXml = EvStatics.SerialiseXmlObject<Evado.UniForm.Model.EuAppData> ( this._AppDataObject );
        Evado.Model.EvStatics.Files.saveFile ( Global.TempPath, @"AppDataObject.text", stXml );
      }

      //
      // output the jason ad a memory stream to send to the client.
      //
      WebOperationContext.Current.OutgoingResponse.ContentType = "application/json";
      var result = new MemoryStream ( ASCIIEncoding.UTF8.GetBytes ( json ) );

      this.LogValue4 ( " generating the memory stream output." );

      //
      // Log the user's transaction duration.
      //
      TimeSpan duration = DateTime.Now - this._StartTime;

      this.LogEvent ( "User " + this._ServiceUserProfile.UserId
        + " send command: '" + this._CommandTitle + "'"
        + " transaction date " + this._StartTime.ToString ( "dd MMM yyyy HH:mm:ss" )
        + " duration " + duration.ToString ( ) );

      //
      // debug list the global objects.
      //
      foreach ( System.Collections.DictionaryEntry entry in Global.GlobalObjectList )
      {
        this.LogValue4 ( "Item: " + entry.Key );
      }

      this.LogMethodEnd ( "generateWebServiceResponse" );
      Global.OutputApplicationLog ( );

      Global.OutputEventLog ( );

      //
      // Return the web service reponse.
      //
      return result;

    }//END generateWebServiceResponse method

    // =====================================================================================
    /// <summary>
    /// This method generates the Web service request to be sent to the device app client.
    /// </summary>
    /// <returns>Stream: of the content to be sent to the device client.</returns>
    // -------------------------------------------------------------------------------------
    public Stream generateErrorWebServiceResponse (
      Exception ServiceException )
    {
      //
      // Initialise the methods variables and object.
      //
      this.LogMethod ( "generateErrorWebServiceResponse " );
      string stFileName = @"eventlog.txt";
      string json = String.Empty;
      string stEventLogPath = Global.ApplicationPath + stFileName;
      string stServiceException = "SERVICE EVENT occured at " + DateTime.Now.ToString ( "dd MMM yyy HH:mm:ss" )
        + "\r\n\r\n" + Evado.Model.EvStatics.getException ( ServiceException );

      this.LogValue ( stServiceException );

      this.LogValue4 ( stServiceException );

      //
      // Add the session id
      //
      this._AppDataObject.SessionId = this._Context.Session.SessionID;
      this._AppDataObject.Url = this._CurrentServiceURL;
      if ( this._ServiceUserProfile.AuthenticationState == EvUserProfileBase.UserAuthenticationStates.Authenticated
        || this._ServiceUserProfile.AuthenticationState == EvUserProfileBase.UserAuthenticationStates.Anonymous_Access )
      {
        this._AppDataObject.Status = Evado.UniForm.Model.EuAppData.StatusCodes.Login_Authenticated;
      }
      this._AppDataObject.Message =
        "A web service event has occured.\r\n\r\n  The link provides further information on this problem. \r\n\r\n"
        + this._CurrentServiceURL + Global.APPLICATION_SERVICE_ROOT + stFileName;


      this._AppDataObject.Message = this._AppDataObject.Message.Replace ( "/rest", "/" );

      //
      // Convert the application data object to the client version.
      //

      // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      // 
      this.LogDebug ( " SERIALISING the Data Object " );


      //
      // Convert the application data to the client version.
      //
      json = JsonConvert.SerializeObject ( this._AppDataObject, Formatting.Indented );

      //
      // output the jason ad a memory stream to send to the client.
      //
      WebOperationContext.Current.OutgoingResponse.ContentType = "application/json";
      var result = new MemoryStream ( ASCIIEncoding.UTF8.GetBytes ( json ) );

      this.LogDebug ( " generating ERROR the memory stream output." );

      //
      // Write the log
      //
      if ( Global.LoggingLevel > 0 )
      {
        Evado.Model.EvStatics.Files.saveFile ( Global.TempPath, @"jsonData.txt", json );
      }

      this.LogMethodEnd ( "generateErrorWebServiceResponse" );
      Global.OutputApplicationLog ( );

      Global.OutputEventLog ( );

      //
      // Return the web service reponse.
      //
      return result;

    }//END generateWebServiceResponse method


    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Use profile methods

    //===================================================================================
    /// <summary>
    /// This method sets the user profile to null
    /// </summary>
    //-----------------------------------------------------------------------------------
    private void nullServiceUserProfile ( )
    {
      this.LogMethod ( "nullServiceUserProfile method." );
      //
      // for logout delete all session variables.
      //
      String userProfileSessionKey = this._ServiceUserProfile.UserId.ToUpper ( ) + CONST_USER_PROFILE_SUFFIX;
      this._Context.Session [ userProfileSessionKey ] = null;
    }

    //===================================================================================
    /// <summary>
    /// This method saves the user profile.
    /// </summary>
    //-----------------------------------------------------------------------------------
    private void setServiceUserProfile ( )
    {
      this.LogMethod ( "setServiceUserProfile method." );
      //
      // If the user profile has a user identifier save the user profile in session.
      //
      if ( this._ServiceUserProfile.UserId != String.Empty )
      {
        String userProfileSessionKey = this._ServiceUserProfile.UserId.ToUpper ( ) + CONST_USER_PROFILE_SUFFIX;
        this._Context.Session [ userProfileSessionKey ] = this._ServiceUserProfile;
      }
      this.LogMethodEnd ( "setServiceUserProfile" );
    }

    //===================================================================================
    /// <summary>
    /// This method saves the user profile.
    /// </summary>
    //-----------------------------------------------------------------------------------
    private void getServiceUserProfile ( Evado.UniForm.Model.EuCommand PageCommand )
    {
      this.LogMethod ( "getServiceUserProfile method." );
      //
      // Define the server user profile.
      //
      this._ServiceUserProfile = new EvUserProfileBase ( );

      //
      // Retrieve the userId from the page command.
      //
      string stUserId = PageCommand.GetHeaderValue ( Evado.UniForm.Model.EuCommandHeaderParameters.UserId );
      string userProfileSessionKey = stUserId.ToUpper ( ) + CONST_USER_PROFILE_SUFFIX;

      this.LogDebug ( "PageCommand.Head.UserId: " + stUserId );
      //
      // Using the user id retrieve the user service profile from session.
      //
      if ( stUserId != String.Empty )
      {
        if ( this._Context.Session [ userProfileSessionKey ] != null )
        {
          this._ServiceUserProfile = (EvUserProfileBase) this._Context.Session [ userProfileSessionKey ];
          this._ServiceUserProfile.NewAuthentication = false;
        }
        else
        {
          this.LogDebug ( "Create new Service User Profile" );
          this._ServiceUserProfile.UserId = stUserId;
          this._ServiceUserProfile.CommonName = stUserId;
          this._ServiceUserProfile.AuthenticationState = EvUserProfileBase.UserAuthenticationStates.Not_Authenticated;
          this._ServiceUserProfile.NewAuthentication = false;

          this._Context.Session [ userProfileSessionKey ] = this._ServiceUserProfile;
        }
      }
      this.LogMethodEnd ( "getServiceUserProfile" );
    }//END getUserProfile method

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region User validation methods

    // =====================================================================================
    /// <summary>
    /// Description:
    ///  This method validates a user credentials
    /// 
    /// </summary>
    // -------------------------------------------------------------------------------------
    public bool ReValidateUserCredentials (
      Evado.UniForm.Model.EuCommand PageCommand )
    {
      this.LogMethod ( "ReValidateUserCredentials method. " );
      //
      // Initialise the methods and objects.
      //
      this._ServiceUserProfile.NewAuthentication = false;
      this._ServiceUserProfile.AuthenticationState = EvUserProfileBase.UserAuthenticationStates.Not_Authenticated;
      string stPassword = PageCommand.GetParameter ( Evado.UniForm.Model.EuStatics.PARAMETER_LOGIN_PASSWORD );

      //
      // if debug validation is turned on, the ADS is by passed.
      //
      if ( Global.DebugValidationOn == true )
      {
        String message = "User: " + this._ServiceUserProfile.UserId + " DEBUG VALIDATION PASSED.";
        this.LogValue ( message );

        //
        // Set object values for DEBUG authentication.
        //
        this._AppDataObject.Status = Evado.UniForm.Model.EuAppData.StatusCodes.Login_Authenticated;
        this._ServiceUserProfile.AuthenticationState = EvUserProfileBase.UserAuthenticationStates.Authenticated;
        this._ServiceUserProfile.IsAuthenticated = true;
        this._ServiceUserProfile.LoginFailureCount = 0;
        this._ServiceUserProfile.NewAuthentication = true;
        //
        // Add user to the list of users..
        //
        //Global.CurrentUsers.Add ( this._ServiceUserProfile.UserId );

        this.setServiceUserProfile ( );

        this.LogMethodEnd ( "ReValidateUserCredentials" );
        return true;
      }

      if ( this._ServiceUserProfile.UserId == String.Empty
        || stPassword == String.Empty )
      {
        this.LogValue4 ( "User Id or password missing." );

        this.LogMethodEnd ( "ReValidateUserCredentials" );
        return false;
      }

      this.LogDebug ( "ATTEMPTING AUTHENTIATION: User credentials: "
        + "UserId:" + this._ServiceUserProfile.UserId
        + ", Password: " + stPassword );

      //
      // Validate the user's credentials against AD.
      //
      if ( Membership.ValidateUser ( this._ServiceUserProfile.UserId, stPassword ) == true )
      {
        String message = "User: " + this._ServiceUserProfile.UserId + " credentials AD VALIDATION PASSED.";

        this.LogValue ( message );

        EventLog.WriteEntry (
          Global.EventLogSource,
          message,
          EventLogEntryType.Information );

        //
        // Set object values for AD authentication.
        //
        this._AppDataObject.Status = Evado.UniForm.Model.EuAppData.StatusCodes.Login_Authenticated;
        this._ServiceUserProfile.AuthenticationState = EvUserProfileBase.UserAuthenticationStates.Authenticated;
        this._ServiceUserProfile.CommonName = this._ServiceUserProfile.UserId;
        this._ServiceUserProfile.IsAuthenticated = true;
        this._ServiceUserProfile.NewAuthentication = true;
        this._ServiceUserProfile.LoginFailureCount = 0;

        //
        // Store the base user profile.
        //
        this.setServiceUserProfile ( );

        this.LogMethodEnd ( "ReValidateUserCredentials" );
        return true;
      }

      //
      // log the failed validation attempt.
      //
      string message1 = "User: " + this._ServiceUserProfile.UserId + " VALIDATION FAILED.";

      this.LogValue ( message1 );

      this._ServiceUserProfile.AuthenticationState = EvUserProfileBase.UserAuthenticationStates.Not_Authenticated;
      this._ServiceUserProfile.LoginFailureCount++;

      //
      // Store the base user profile.
      //
      this.setServiceUserProfile ( );

      this.LogMethodEnd ( "ReValidateUserCredentials" );
      return false;

    }//END ReValidateUserCredentials method

    // =====================================================================================
    /// <summary>
    /// Description:
    ///  This method validates a user credentials
    /// 
    /// </summary>
    // -------------------------------------------------------------------------------------
    public bool validateUserCredentials (
      Evado.UniForm.Model.EuCommand PageCommand )
    {
      this.LogMethod ( "ValidateUserCredentials method. " );
      //
      // Initialise the methods and objects.
      //
      this._ServiceUserProfile.NewAuthentication = false;

      //
      // Verify that the groupCommand is login groupCommand.
      //
      if ( PageCommand.Type !=Evado.UniForm.Model.EuCommandTypes.Login_Command
        && PageCommand.Type !=Evado.UniForm.Model.EuCommandTypes.Anonymous_Command
        && PageCommand.Type !=Evado.UniForm.Model.EuCommandTypes.Network_Login_Command)
      {
        this.LogValue4 ( " A login command was not recieved."
          + "  Type: " + PageCommand.Type );

        this.LogMethodEnd ( "ValidateUserCredentials" );
        return true;
      }

      //
      // If the command type is a annoymous then authenticate the user annoymously and exit
      //
      if ( PageCommand.Type ==Evado.UniForm.Model.EuCommandTypes.Anonymous_Command
        && Global.EnableAnonymousLogin == true )
      {
        PageCommand.Title = "Home Page";
        this.LogDebug ( "PageCommand.Type = Anonymous_Command" );
        this._ServiceUserProfile.AuthenticationState = EvUserProfileBase.UserAuthenticationStates.Anonymous_Access;
        this._ServiceUserProfile.IsAuthenticated = true;
        this._ServiceUserProfile.NewAuthentication = true;

        this.setServiceUserProfile ( );

        this.LogMethodEnd ( "ValidateUserCredentials" );
        return true;
      }

      //
      // Extract the command parameter values.
      //
      string stUserToken = PageCommand.GetParameter ( Evado.UniForm.Model.EuStatics.PARAMETER_LOGIN_USER_TOKEN );
      string stUserId = PageCommand.GetParameter ( Evado.UniForm.Model.EuStatics.PARAMETER_LOGIN_USER_ID );
      string stPassword = PageCommand.GetParameter ( Evado.UniForm.Model.EuStatics.PARAMETER_LOGIN_PASSWORD );
      string stNetworkRoles = PageCommand.GetParameter ( Evado.UniForm.Model.EuStatics.PARAMETER_NETWORK_ROLES );
      string stDeviceId = PageCommand.GetDeviceId ( );

      //
      // Object values.
      //
      PageCommand.DeleteParameter ( Evado.UniForm.Model.EuStatics.PARAMETER_LOGIN_USER_ID );
      PageCommand.DeleteParameter ( Evado.UniForm.Model.EuStatics.PARAMETER_LOGIN_PASSWORD );
      this._ServiceUserProfile.UserId = stUserId;
      this._ServiceUserProfile.AuthenticationState = EvUserProfileBase.UserAuthenticationStates.Not_Authenticated;

      Global.deleteUsersGlobalObjects ( this._ServiceUserProfile );
      //
      // if debug validation is turned on, the ADS is by passed.
      //
      if ( Global.DebugValidationOn == true )
      {
        String message = "User: " + this._ServiceUserProfile.UserId + " DEBUG VALIDATION PASSED.";
        this.LogValue ( message );

        this.LogEvent ( message );

        //
        // Set object values for DEBUG authentication.
        //
        this._AppDataObject.Status = Evado.UniForm.Model.EuAppData.StatusCodes.Login_Authenticated;
        this._ServiceUserProfile.AuthenticationState = EvUserProfileBase.UserAuthenticationStates.Debug_Authentication;
        this._ServiceUserProfile.CommonName = this._ServiceUserProfile.UserId;
        this._ServiceUserProfile.IsAuthenticated = true;
        this._ServiceUserProfile.LoginFailureCount = 0;
        this._ServiceUserProfile.NewAuthentication = true;

        PageCommand.Type =Evado.UniForm.Model.EuCommandTypes.Normal_Command;
        //
        // Add user to the list of users..
        //
        //Global.CurrentUsers.Add ( this._ServiceUserProfile.UserId );

        this.setServiceUserProfile ( );

        this.LogMethodEnd ( "ValidateUserCredentials" );
        return true;
      }

      //
      // if network authenticatin is active, then the user was authenticated by ADS at the web client
      //
      if ( PageCommand.Type ==Evado.UniForm.Model.EuCommandTypes.Network_Login_Command
        && stDeviceId == Evado.UniForm.Model.EuStatics.CONST_WEB_CLIENT )
      {
        PageCommand.Title = "Home Page";
        String message = message = "User: " + this._ServiceUserProfile.UserId + " NETWORK AUTHENITCATED.";

        this.LogValue ( message );

        EventLog.WriteEntry ( Global.EventLogSource,
          message,
          EventLogEntryType.Information );

        //
        // Set object values for DEBUG authentication.
        //
        this._AppDataObject.Status = Evado.UniForm.Model.EuAppData.StatusCodes.Login_Authenticated;
        this._ServiceUserProfile.AuthenticationState = EvUserProfileBase.UserAuthenticationStates.Authenticated;
        this._ServiceUserProfile.CommonName = this._ServiceUserProfile.UserId;
        this._ServiceUserProfile.DomainGroupNames = stNetworkRoles;
        this._ServiceUserProfile.IsAuthenticated = true;
        this._ServiceUserProfile.LoginFailureCount = 0;

        PageCommand.Type =Evado.UniForm.Model.EuCommandTypes.Normal_Command;

        this.LogDebug ( "UserId: " + this._ServiceUserProfile.UserId );
        this.LogDebug ( "DomainGroupNames: " + this._ServiceUserProfile.DomainGroupNames );

        this.setServiceUserProfile ( );

        return true;
      }
  
      if ( stUserId == String.Empty
        || stPassword == String.Empty )
      {
        this.LogValue4 ( "User Id or password missing." );

        this.LogMethodEnd ( "ValidateUserCredentials" );
        return false;
      }

      this.LogDebug ( "ATTEMPTING AUTHENTIATION: User credentials: "
        + "UserId:" + stUserId
        + ", Password: " + stPassword );

      //
      // Validate the user's credentials against AD.
      //
      if ( Membership.ValidateUser ( stUserId, stPassword ) == true )
      {
        String message = "User: " + this._ServiceUserProfile.UserId + " credentials AD VALIDATION PASSED.";

        this.LogValue ( message );

        EventLog.WriteEntry (
          Global.EventLogSource,
          message,
          EventLogEntryType.Information );

        //
        // Set object values for AD authentication.
        //
        this._AppDataObject.Status = Evado.UniForm.Model.EuAppData.StatusCodes.Login_Authenticated;
        this._ServiceUserProfile.AuthenticationState = EvUserProfileBase.UserAuthenticationStates.Authenticated;
        this._ServiceUserProfile.CommonName = this._ServiceUserProfile.UserId;
        this._ServiceUserProfile.IsAuthenticated = true;
        this._ServiceUserProfile.NewAuthentication = true;
        this._ServiceUserProfile.LoginFailureCount = 0;

        PageCommand.Type =Evado.UniForm.Model.EuCommandTypes.Normal_Command;

        //
        // get the active directory user data for the application.
        //
        this.getAdUserData ( stUserId, stPassword );

        //
        // Store the base user profile.
        //
        this.setServiceUserProfile ( );

        this.LogMethodEnd ( "ValidateUserCredentials" );
        return true;
      }

      //
      // log the failed validation attempt.
      //
      string message1 = "User: " + this._ServiceUserProfile.UserId + " VALIDATION FAILED.";


      this.LogEvent ( message1 );

      this._ServiceUserProfile.AuthenticationState = EvUserProfileBase.UserAuthenticationStates.Not_Authenticated;
      this._ServiceUserProfile.LoginFailureCount++;

      //
      // Store the base user profile.
      //
      this.setServiceUserProfile ( );

      this.LogMethodEnd ( "ValidateUserCredentials" );
      return false;

    }//END validateUserCredentials method

    // =====================================================================================
    /// <summary>
    /// Description:
    ///  This method requests a user login on request.
    /// 
    /// </summary>
    // -------------------------------------------------------------------------------------
    public Evado.UniForm.Model.EuAppData requestUserLogin ( String ErrorMessage )
    {

      this.LogValue4 ( "Evado.Digital.WebService.ClientService.requestUserLogin method." );
      //
      // Initialise the methods variables and objects.
      //
      Evado.UniForm.Model.EuAppData dataObject = new Evado.UniForm.Model.EuAppData ( );

      this._ServiceUserProfile.AuthenticationState = EvUserProfileBase.UserAuthenticationStates.Not_Authenticated;

      //
      // delete the user from current list of users.
      //
      //Global.CurrentUsers.Remove ( this._ServiceUserProfile.UserId );

      //
      // Delete the user session data.
      //
      this.deleteUserSessionObjects ( );

      //
      // Increment the login count
      //
      //loginCount++;

      dataObject.Id = Guid.NewGuid ( );
      dataObject.Page.Id = dataObject.Id;
      dataObject.Page.Exit = new Evado.UniForm.Model.EuCommand ( );

      //
      // define the data object properties.
      //
      dataObject.Title = Evado.Model.EvmLabels.AUTHENTICATION_REQUEST_LOGIN;
      dataObject.Status = Evado.UniForm.Model.EuAppData.StatusCodes.Login_Request;
      dataObject.Message = ErrorMessage;
      dataObject.LogoFilename = Global.LogoFilename;

      dataObject.Page.Exit = new Evado.UniForm.Model.EuCommand ( Evado.Model.EvmLabels.AUTHENTICATION_LOGIN_CMD_TITLE,
             Evado.UniForm.Model.EuCommandTypes.Login_Command,
              Evado.UniForm.Model.EuStatics.CONST_DEFAULT,
              Evado.UniForm.Model.EuStatics.CONST_DEFAULT,
              Evado.UniForm.Model.EuMethods.Get_Object );

      //
      // Notify the user that the login request failed.
      //
      if ( this._ServiceUserProfile.LoginFailureCount >= Global.MaxLoginAttempts )
      {
        dataObject.Message = Evado.Model.EvmLabels.AUTHENTICATION_EXCEEDED_MAX_LOGIN_ATTEMPTS;
        dataObject.Status = Evado.UniForm.Model.EuAppData.StatusCodes.Login_Count_Exceeded;
      }

      this.setServiceUserProfile ( );

      return dataObject;

    }//END requestUserLogin method

    // ==================================================================================
    /// <summary>
    /// This method authenticates the user and retrieves the user groups for role management.
    /// </summary>
    /// <param name="username">String: The users name</param>
    /// <param name="password">String: The user's password</param>
    /// <returns>Bool: true authenticated</returns>
    // ----------------------------------------------------------------------------------
    public bool getAdUserData ( String username, String password )
    {
      this.LogMethod ( "getAdUserGroups method. " );

      //
      // Initialise the methods variables and objects.
      //
      String ldapPath = String.Empty;

      //
      // Retrieve the Ad connection string if it exists.
      //
      if ( ConfigurationManager.ConnectionStrings [ Evado.Model.EvStatics.CONFIG_LDAP_CONNECTION_STRING ] != null )
      {
        ldapPath = ConfigurationManager.ConnectionStrings [ Evado.Model.EvStatics.CONFIG_LDAP_CONNECTION_STRING ].ConnectionString;
      }

      //
      // Return an error if the ldap path is empty
      //
      if ( ldapPath == String.Empty )
      {
        this.LogEvent ( "LDAP path is empty" );

        return false;
      }

      //this.LogDebugValue ( "ldapPath: " + ldapPath );
      // this.LogDebugValue ( "username: " + username );
      //this.LogDebugValue ( "password: " + password );

      try
      {
        System.DirectoryServices.DirectoryEntry entry = new System.DirectoryServices.DirectoryEntry ( ldapPath, username, password );

        System.DirectoryServices.DirectorySearcher search = new System.DirectoryServices.DirectorySearcher ( entry );
        search.Filter = "(SAMAccountName=" + username + ")";
        search.PropertiesToLoad.Add ( "memberOf" );

        System.DirectoryServices.SearchResult result = search.FindOne ( );

        if ( null == result )
        {
          return false;
        }

        int propertyCount = result.Properties [ "memberOf" ].Count;

        //this.LogDebugValue ( "propertyCount: " + propertyCount );

        String dn;
        int equalsIndex, commaIndex;

        for ( int propertyCounter = 0; propertyCounter < propertyCount; propertyCounter++ )
        {
          dn = (String) result.Properties [ "memberOf" ] [ propertyCounter ];

          //this.LogDebugValue ( "dn: " + dn );

          equalsIndex = dn.IndexOf ( "=", 1 );
          commaIndex = dn.IndexOf ( ",", 1 );
          if ( -1 == equalsIndex )
          {
            continue;
          }

          String groupName = dn.Substring ( ( equalsIndex + 1 ), ( commaIndex - equalsIndex ) - 1 );

          //this.LogDebugValue ( "groupName: " + groupName );

          this._ServiceUserProfile.addDomainGroup ( groupName );

        }//END property iteration loop;

        //this.LogValue4 ( "UserId: " + username + ", DomainGroups: " + this._ServiceUserProfile.DomainGroupNames );

      }
      catch ( Exception ex )
      {
        this.LogValue4 ( "EXCEPTION:" + EvStatics.getException ( ex ) );
        return false;
      }

      return true;

    }//END getAdUserGroups method

    //==================================================================================
    /// <summary>
    /// This session deletes the user session objects.
    /// </summary
    //-----------------------------------------------------------------------------------
    private void deleteUserSessionObjects ( )
    {
      this.LogMethod ( "deleteUserSessionObjects method. " );

      if ( Global.DeleteSessionOnExit == false )
      {
        this.LogValue4 ( "DeleteSessionOnExit is false" );
        return;
      }

      String userId = this._ServiceUserProfile.UserId.ToUpper ( );
      this.LogValue4 ( "userId: " + userId );

      if ( userId == String.Empty )
      {
        return;
      }
      this.nullServiceUserProfile ( );

      String Date_Key = userId + Evado.UniForm.Model.EuStatics.GLOBAL_DATE_STAMP;

      this.LogValue4 ( "Date Key: " + Date_Key );

      String ClinicalObject_Key = userId + Evado.UniForm.Model.EuStatics.GLOBAL_SESSION_OBJECT;

      this.LogValue4 ( "Clinical Key: " + ClinicalObject_Key );

      String HistoryList_Key = userId + Evado.UniForm.Model.EuStatics.GLOBAL_COMMAND_HISTORY;

      this.LogValue4 ( "Command History Key: " + HistoryList_Key );

      Global.GlobalObjectList.Remove ( Date_Key );
      Global.GlobalObjectList.Remove ( ClinicalObject_Key );
      Global.GlobalObjectList.Remove ( HistoryList_Key );

    }//END deleteUserSessionObjects method

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Transaction and Event logging methods

    // =====================================================================================
    /// <summary>
    /// Description:
    ///  This method logs the user transaction sent to the service
    /// 
    /// </summary>
    // -------------------------------------------------------------------------------------
    private void logUserTransaction ( Evado.UniForm.Model.EuCommand PageCommand, string version, string session )
    {
      //
      // Log the User Command
      //
      StringBuilder sbCommandLog = new StringBuilder ( );

      sbCommandLog.AppendLine ( "PageCommand Transaction:" );
      sbCommandLog.AppendLine ( "Session Id: " + this._Context.Session.SessionID );
      sbCommandLog.AppendLine ( "Session objects:" );

      //
      // Log the user session keys.
      //
      foreach ( string sKey in this._Context.Session.Contents )
      {
        sbCommandLog.AppendLine ( string.Format ( "\tKey: {0}: {1}", sKey, this._Context.Session [ sKey ] ) );
      }

      //
      // Log the HTTP header elements
      //
      if ( Global.LoggingLevel > 4
        && this._Context.Request.Headers.Keys.Count > 0 )
      {
        sbCommandLog.AppendLine ( "Http headers: " );
        foreach ( string key in this._Context.Request.Headers.Keys )
        {
          sbCommandLog.AppendLine ( string.Format ( "\tkey: {0}: {1}", key, this._Context.Request.Headers [ key ] ) );
        }
      }

      sbCommandLog.AppendLine ( "URL Version: " + version );
      sbCommandLog.AppendLine ( "URL Session ID: " + session );
      sbCommandLog.AppendLine ( "Sent the following command transaction: " );
      sbCommandLog.AppendLine ( "-Command Id: " + PageCommand.Id );
      sbCommandLog.AppendLine ( "-Command Title: " + PageCommand.Title );
      sbCommandLog.AppendLine ( "-Command Type: " + PageCommand.Type );
      sbCommandLog.AppendLine ( "-Application Id: " + PageCommand.ApplicationId );
      sbCommandLog.AppendLine ( "-Application Object: " + PageCommand.Object );
      sbCommandLog.AppendLine ( "-Application Method: " + PageCommand.Method );

      if ( Global.LoggingLevel > 4
        && PageCommand.Header.Count > 0 )
      {
        sbCommandLog.AppendLine ( "Transaction Header:" );
        foreach ( Evado.UniForm.Model.EuParameter prm in PageCommand.Header )
        {
          sbCommandLog.AppendFormat ( "\tName:\t{0},\tValue:\t{1}", prm.Name, prm.Value );
        }
      }

      if ( Global.LoggingLevel > 4
        || PageCommand.Type !=Evado.UniForm.Model.EuCommandTypes.Login_Command )
      {
        if ( PageCommand.Parameters.Count > 0 )
        {
          sbCommandLog.AppendLine ( "Command Parameters" );
          foreach ( Evado.UniForm.Model.EuParameter prm in PageCommand.Parameters )
          {
            sbCommandLog.AppendLine ( string.Format ( "\tName:\t{0},\tValue:\t{1}", prm.Name, prm.Value ) );
          }
        }
        else
        {
          sbCommandLog.AppendLine ( "No parameters found." );
        }
      }
      else
      {
        sbCommandLog.AppendLine ( "User Credentials hidden." );
      }

      this.LogEvent ( sbCommandLog.ToString ( ) );

    }//END logUserTransaction method


    //*********************************************************************************************
    #endregion


    #region unused http methods


    // TODO: Implement the collection resource that will contain the SampleItem instances

    [WebGet ( UriTemplate = "" )]
    public string GetCollection ( )
    {
      // TODO: Replace the current implementation to return a collection of SampleItem instances
      return "{\"RESULT\":\"OK\"}";
    }

    //  =================================================================================
    /// <summary>
    /// Web Method
    /// </summary>
    /// <returns>json object.</returns>
    //  ---------------------------------------------------------------------------------
    [WebInvoke ( UriTemplate = "{id}", Method = "PUT" )]
    public string Update ( string id )
    {
      this.LogMethod ( "Update web service method." );
      this.LogDebug ( "id: " + id );

      Global.OutputApplicationLog ( );

      Global.OutputEventLog ( );

      return "{\"RESULT\":\"ERROR\"}";
    }


    //  =================================================================================
    /// <summary>
    /// Web Method
    /// </summary>
    /// <param name="id">Application identifier</param>
    /// <returns>json object.</returns>
    //  ---------------------------------------------------------------------------------
    [WebGet ( UriTemplate = "{id}" )]
    public string Get ( string id )
    {
      // TODO: Return the instance of SampleItem with the given id
      throw new NotImplementedException ( );
    }

    //  =================================================================================
    /// <summary>
    /// Web Method
    /// </summary>
    /// <returns>json object.</returns>
    //  ---------------------------------------------------------------------------------
    [WebInvoke ( UriTemplate = "{id}", Method = "DELETE" )]
    public void Delete ( string id )
    {
      // TODO: Remove the instance of SampleItem with the given id from the collection
      throw new NotImplementedException ( );
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class Logging methods.

    const string CONST_NAME_SPACE = "Evado.UniForm.ClientService.";

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    private void LogInitMethod ( String Value )
    {
      Global.LogService ( Evado.Model.EvStatics.CONST_METHOD_START
      + DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": "
      + ClientService.CONST_NAME_SPACE + Value );
    }
    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="MethodName">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    private void LogInitMethodEnd ( String MethodName )
    {
      String value = Evado.Model.EvStatics.CONST_METHOD_END;
      value = value.Replace ( " END OF METHOD ", " END OF " + MethodName + " METHOD " );
      Global.LogService ( value );
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    private void logInitValue ( String Value )
    {
      Global.LogService ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " + Value );
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    private void LogMethod ( String Value )
    {
      if ( Global.LoggingLevel > 1 )
      {
        Global.LogService ( Evado.Model.EvStatics.CONST_METHOD_START
        + DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": "
        + ClientService.CONST_NAME_SPACE + Value + " method" );
      }
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="MethodName">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    private void LogMethodEnd ( String MethodName )
    {
      if ( Global.LoggingLevel > 1 )
      {
        String value = Evado.Model.EvStatics.CONST_METHOD_END;
        value = value.Replace ( " END OF METHOD ", " END OF " + MethodName + " METHOD " );
        Global.LogService ( value );
      }
    }
    //  =================================================================================
    /// <summary>
    ///   This static method removes a user from the online user list.
    /// 
    /// </summary>
    //   ---------------------------------------------------------------------------------
    private void LogEvent ( String Value )
    {
      Global.LogEvent ( Value );

    }//ENd LogEvent method


    //  =================================================================================
    /// <summary>
    ///   This static method removes a user from the online user list.
    /// 
    /// </summary>
    //   ---------------------------------------------------------------------------------
    private void LogEvent ( Exception Value )
    {
      String value = EvStatics.getException ( Value );

      this.LogEvent ( value );

    }//END LogEvent method

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    private void LogValue ( String Value )
    {
      if ( Global.LoggingLevel > 2 )
      {
        Global.LogService ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " + Value );
      }
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    private void LogValue4 ( String Value )
    {
      if ( Global.LoggingLevel > 3 )
      {
        Global.LogService ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " + Value );
      }
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    private void LogDebug ( String Value )
    {
      if ( Global.LoggingLevel > 4 )
      {
        Global.LogService ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " + Value );
      }
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Format">String: format text.</param>
    /// <param name="args">Array of objects as parameters.</param>
    // ----------------------------------------------------------------------------------
    private void LogDebug ( String Format, params object [ ] args )
    {
      if ( Global.LoggingLevel > 4 )
      {
        Global.LogService ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ":" +
          String.Format ( Format, args ) );
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
        if ( Global.LoggingLevel == 5 )
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
    private void LogClass ( String Value )
    {
      if ( Global.LoggingLevel > 2 )
      {
        Global.LogService ( Value );
      }
    }


    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
  }
}
