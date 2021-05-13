/***************************************************************************************
 * <copyright file="Evado.Integration.Service\Query.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c)  2002 - 2021  EVADO HOLDING PTY. LTD..  All rights reserved.
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
using Evado.Model;

//
// The name space for the native integration adapter
//
namespace Evado.Integration.Service
{
  
  // 
  // Start the service and browse to http://<machine_name>:<port>/Service1/help to view the service's generated help page
  // NOTE: By default, a new instance of the service is created for each call; change the InstanceContextMode to Single if you want
  // a single instance of the service to process all calls.	
  //
  [ServiceContract]
  [AspNetCompatibilityRequirements ( RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed )]
  [ServiceBehavior ( InstanceContextMode = InstanceContextMode.PerSession )]
  //  ===================================================================================
  /// <summary>
  /// This class executes the web service to execute an integration serices query.
  /// </summary>
  //  -----------------------------------------------------------------------------------
  public class Query
  {
    #region Class initialisation methods
    //  ===================================================================================
    /// <summary>
    /// This method initialises the service and sets the context object for the users session.
    /// </summary>
    //  -----------------------------------------------------------------------------------
    public Query ( )
    {
      this._Context = HttpContext.Current;
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class Global objects and values.

    //
    // Constants
    //
    private const string CONST_LOGIN_USER_ID = "UserId";
    private const string CONST_LOGIN_PASSWORD = "Password";


    /// <summary>
    /// This object contains the HTTP context for the connection to the web client.
    /// </summary>
    private HttpContext _Context;

    /// <summary>
    /// This private data object contains the data that has been received from the external REST client
    /// </summary>
    private Evado.Model.Integration.EiData _ReceivedData = new Evado.Model.Integration.EiData ( );

    /// <summary>
    /// This private data object contains the data that will be sent to the external REST client
    /// </summary>
    private Evado.Model.Integration.EiData _ReturnData = new Evado.Model.Integration.EiData ( );

    /// <summary>
    /// this variable contains the the user's profile.
    /// </summary>
    private String _UserId = String.Empty;

    /// <summary>
    /// This global parameter contains the current url.
    /// </summary>
    private String _CurrentServiceURL = String.Empty;

    private float _ClientVersion = 1.0F;

    /// 
    /// Status stores the debug status information.
    /// 
    private StringBuilder _DebugLog = new StringBuilder ( );

    private DateTime _StartTime = DateTime.Now;

    private String _CommandTitle = String.Empty;

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region POST method used in this implementation.

    /**************************  IMPLEMENTATION NOTES **********************************
     *
     * This web service implementation need to have user authentication added as a step 
     * in the server request processing workflow.
     * 
     * User authentication is to be AD validated.
     * 
     ************************************************************************************/

    //  =================================================================================
    /// <summary>
    /// Web Method
    /// </summary>
    /// <param name="ApplicationId">Application identifier</param>
    /// <param name="ClientVersion">verson parameter</param>
    /// <param name="Command">groupCommand parameter</param>
    /// <param name="SessionId">session identifier</param>
    /// <param name="content">content</param>
    /// <returns>json object.</returns>
    //  ---------------------------------------------------------------------------------
    [WebInvoke ( UriTemplate = "/{Version}/{QueryType}?session={SessionId}", Method = "POST" )]
    public Stream getIntegrationObject ( string Version, string QueryType, string SessionId, Stream content )
    {
      Global.ClearDebugLog ( );
      Global.WriteServiceMethod ( "Evado.Integration.Service.Service.getIntegrationObject event method." );
      Global.WriteServiceLogLine ( "Version: " + Version );
      Global.WriteServiceLogLine ( "QueryType: " + QueryType );
      Global.WriteServiceLogLine ( "SessionId: " + SessionId );

      Global.WriteDebugLogMethod ( "Evado.Integration.Service.WcfRestService.getPageObject event method." );
      Global.WriteDebugLogLine ( "Version: " + Version );
      Global.WriteDebugLogLine ( "QueryType: " + QueryType );
      Global.WriteDebugLogLine ( "SessionId: " + SessionId );
      try
      {
        //
        // Initialise the methods variables and object.
        //
        string json = String.Empty;
        Evado.Digital.Bll.EiServices integrationServices = new Evado.Digital.Bll.EiServices ( );
        this._ClientVersion = Evado.Model.EvStatics.decodeMinorVersion ( Version );
        Evado.Digital.Bll.EvStaticSetting.DebugOn = Global.DebugLogOn;

        Global.WriteServiceLogLine ( "ClientVersion: " + this._ClientVersion );
        Global.WriteServiceLogLine ( "Context SessionId: " + this._Context.Session.SessionID );
        Global.WriteDebugLogLine ( "ClientVersion: " + this._ClientVersion );
        Global.WriteDebugLogLine ( "Context SessionId: " + this._Context.Session.SessionID );

        //
        // first load the POST payload into a string
        // the POST content comes from the content param above
        // as it is the only param that is not listed in the URI template
        //
        string content_value = new StreamReader ( content ).ReadToEnd ( );

        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        #region Step 1 set session and deserialise json
        //
        // Deserialise the commandobjects.
        // 
        Global.WriteDebugLogMethod ( "STEP 1 : Deserializing the json command object." );
        Global.WriteServiceMethod ( "STEP 1 : Deserializing the json command object." );

        //
        // if content value is empty then send a login request.
        //
        if ( content_value == String.Empty )
        {
          Global.WriteDebugLogLine ( "POST content is empty." );

          this._ReturnData = new Model.Integration.EiData ( );

          this._ReturnData.EventCode = Model.Integration.EiEventCodes.WebServices_JSON_Empty_Error;
          this._ReturnData.ErrorMessage = "Received JSON object was empty.";
          //
          //  send the web service response to the device app.
          //
          return this.generateWebServiceResponse ( );
        }

        Global.WriteDebugLogLine ( "CONTENT: " +  content_value );

        //
        // attempt to convert this JSON payload to a C# structure
        try
        {
          //
          // serialise the client groupCommand
          //
          this._ReceivedData = JsonConvert.DeserializeObject<Evado.Model.Integration.EiData> ( content_value );

          String eventMessage = "Query Recieved from " + this._UserId 
            + " querying project " + this._ReceivedData.GetQueryParameterValue ( Model.Integration.EiQueryParameterNames.Project_Id )
            + " executing query type " + this._ReceivedData.QueryType ;

          EventLog.WriteEntry ( Global.EventLogSource, eventMessage, EventLogEntryType.Information );

          Global.WriteDebugLogLine ( eventMessage );
        }
        catch ( Exception Ex )
        {

          this._ReturnData = new Model.Integration.EiData ( );

          this._ReturnData.EventCode = Model.Integration.EiEventCodes.WebServices_JSON_Deserialisation_Failed_Error;
          this._ReturnData.ErrorMessage = "JSON object deserialisation error.";

          String EventMessage = evado.model.Properties.Resources.JSON_DESERIALISATION_ERROR + "\r\n" + getExceptionAsString ( Ex );

          EventLog.WriteEntry ( Global.EventLogSource, this._DebugLog.ToString ( ), EventLogEntryType.Error );

          Global.WriteDebugLogLine ( EventMessage );

          Global.WriteServiceMethod ( EventMessage );

          return generateErrorWebServiceResponse ( Ex );
        }

        Global.WriteDebugLogLine ( this._ReceivedData.getAsString ( ) );

        #endregion

        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        #region Step 2: Process the user's groupCommand request

        Global.WriteDebugLogMethod ( "STEP 2 : Process the user's query request." );
        Global.WriteServiceMethod ( "STEP 2 : Process the user's query request." );

        Global.WriteDebugLogLine ( "EvStaticSetting.DebugOn: " + Evado.Digital.Bll.EvStaticSetting.DebugOn );

        //
        // execute the query
        //
        this._ReturnData = integrationServices.ProcessQuery ( this._ReceivedData );

        //
        // Return the debug log.
        //
        Global.WriteDebugLogLine ( integrationServices.Log );

        //
        // Return the debug log.
        //
        Global.WriteDebugLogLine ( "Process log: " + integrationServices.ProcessLog + " END log");


        //
        // Log errored return event codes.
        //
        if ( this._ReturnData == null )
        {
          this._ReturnData = new Model.Integration.EiData ( );
          this._ReturnData.EventCode = Model.Integration.EiEventCodes.Data_Null_Data_Error;
          this._ReturnData.ErrorMessage = "Query failed null data returned.";

          Global.WriteDebugLogLine ( "Integration Service return Event " + Evado.Model.Integration.EiEventCodes.Data_Null_Data_Error );

          EventLog.WriteEntry (
            Global.EventLogSource,
            "Integration Services return Event " + Evado.Model.Integration.EiEventCodes.Data_Null_Data_Error,
            EventLogEntryType.Error );

        }
        else
        {
          if ( this._ReturnData.EventCode != Model.Integration.EiEventCodes.Ok )
          {
            Global.WriteDebugLogLine ( "Integration Service return Event " + this._ReturnData.EventCode );

            EventLog.WriteEntry (
              Global.EventLogSource,
              "Integration Services return Event " + this._ReturnData.EventCode,
              EventLogEntryType.Error );
          }
        }

        //
        //  send the web service response to the device app.
        //
        return this.generateWebServiceResponse ( );

        #endregion

      }
      catch ( Exception Ex )
      {
        string EventMessage = "Evado.UniForm.Service.c event method.\r\n" + Ex.ToString ( );

        EventLog.WriteEntry ( Global.EventLogSource, EventMessage, EventLogEntryType.Error );

        Global.WriteServiceMethod ( EventMessage );

        return generateErrorWebServiceResponse ( Ex );

      } // Close catch   

    }//END Create web method.

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
      Global.WriteDebugLogLine (  Evado.Model.EvStatics.CONST_METHOD_START
        + "Evado.UniForm.Service.generateWebServiceResponse method. " );
      string jsonData = String.Empty;

      // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      // 
      Global.WriteDebugLogLine (  " SERIALISING the Data Object " );

      jsonData = JsonConvert.SerializeObject ( this._ReturnData, Formatting.Indented );

      //
      // Write the log
      //
      if ( Global.DebugLogOn == true )
      {
        Evado.Model.EvStatics.Files.saveFile ( Global.TempPath, @"jsonData.txt", jsonData );
      }

      //
      // output the jason ad a memory stream to send to the client.
      //
      WebOperationContext.Current.OutgoingResponse.ContentType = "application/json";
      var result = new MemoryStream ( ASCIIEncoding.UTF8.GetBytes ( jsonData ) );

      Global.WriteDebugLogLine (  " generating the memory stream output." );

      //
      // Log the user's transaction duration.
      //
      TimeSpan duration = DateTime.Now - this._StartTime;
      this.writeEventLog ( " User " + this._UserId
        + " send command: '" + this._CommandTitle + "'"
        + " transaction date " + this._StartTime.ToString ( "dd MMM yyyy HH:mm:ss" )
        + " duration " + duration.ToString ( ), EventLogEntryType.Information );

      Global.OutputtDebugLog (  );

      Global.OutputServiceLog ( );

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
      Global.WriteDebugLogLine (  Evado.Model.EvStatics.CONST_METHOD_START
        + "Evado.UniForm.Service.generateErrorWebServiceResponse method. " );
      string stFileName = @"eventlog.txt";
      string json = String.Empty;
      string stEventLogPath = Global.ApplicationPath + stFileName;
      string stServiceException = "SERVICE EVENT occured at " + DateTime.Now.ToString ( "dd MMM yyy HH:mm:ss" )
        + "\r\n\r\n" + Evado.Model.EvStatics.getException ( ServiceException );

      Global.WriteServiceLogLine ( stServiceException );

      Global.WriteDebugLogLine (  stServiceException );

      //
      // Convert the application data object to the client version.
      //

      // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      // 
      Global.WriteDebugLogLine (  " SERIALISING the Data Object " );

      json = JsonConvert.SerializeObject ( this._ReturnData, Formatting.Indented );

      //
      // output the jason ad a memory stream to send to the client.
      //
      WebOperationContext.Current.OutgoingResponse.ContentType = "application/json";
      var result = new MemoryStream ( ASCIIEncoding.UTF8.GetBytes ( json ) );

      Global.WriteDebugLogLine (  " generating ERROR the memory stream output." );

      //
      // Write the log
      //
      if ( Global.DebugLogOn == true )
      {
        Evado.Model.EvStatics.Files.saveFile ( Global.TempPath, @"jsonData.txt", json );
      }

      Global.OutputtDebugLog (   );

      Global.OutputServiceLog ( );

      //
      // Return the web service reponse.
      //
      return result;

    }//END generateWebServiceResponse method

    // =====================================================================================
    /// <summary>
    /// Description:
    ///  This method set the global variables from session objects.
    /// 
    /// </summary>
    // -------------------------------------------------------------------------------------
    private void loadSessionVariables ( )
    {
      Global.WriteDebugLogMethod (  "Evado.UniForm.Service.loadSessionVariables method." );
      /*
      if ( this._Context.Session [ Evado.Model.EvStatics.SESSION_USER_ID ] != null )
      {
        this._UserId = (string) this._Context.Session [ Evado.Model.EvStatics.SESSION_USER_ID ];
      }
      Global.WriteDebugLogLine (  "UserId: " + this._UserId );

      // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

      if ( this._Context.Session [ Evado.Model.EvStatics.SESSION_DEVICE_VALIDATED ] != null )
      {
        this._DeviceValidated = (bool) this._Context.Session [ Evado.Model.EvStatics.SESSION_DEVICE_VALIDATED ];
      }
      Global.WriteDebugLogLine (  "DeviceValidated: " + this._DeviceValidated );

      // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

      if ( this._Context.Session [ Evado.Model.EvStatics.SESSION_USER_VALIDATED ] != null )
      {
        this._UserValidated = (bool) this._Context.Session [ Evado.Model.EvStatics.SESSION_USER_VALIDATED ];
      }
      Global.WriteDebugLogLine (  "UserValidated: " + this._UserValidated );


      if ( this._Context.Session [ Evado.Model.EvStatics.APPLICATION_DEFAULT_TRIAL_ID ] == null )
      {
        if ( ConfigurationManager.AppSettings [ "DefaultTrialId" ] != null )
        {
          this._Context.Session [ Evado.Model.EvStatics.APPLICATION_DEFAULT_TRIAL_ID ] = ConfigurationManager.AppSettings [ "DefaultTrialId" ];
        }
      }
      */
    }//END getSessionVariables method

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Device validation methods


    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region User validation methods

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Transaction and Event logging methods


    // =====================================================================================
    /// <summary>
    /// getListAsString methods
    /// 
    /// Description:
    ///  This method returns an array of strings as a string.
    /// 
    /// </summary>
    // -------------------------------------------------------------------------------------
    public static string getExceptionAsString ( Exception Ex )
    {
      string stException = "Exception: ";

      stException += "\r\n Message: " + Ex.Message
        + "\r\n Source: " + Ex.Source
        + "\r\n TargetSite: " + Ex.TargetSite
        + "\r\n Inner Exeception: \r\n" + Ex.InnerException
        + "\r\n StackTrace: \r\n" + Ex.StackTrace;

      return stException;
    }

    //  ===========================================================================
    /// <summary>
    /// LogWarning method
    /// 
    /// Description:
    /// 
    /// </summary>
    /// <returns>If the event source was created successfully true is returned, otherwise false.</returns>
    //  ---------------------------------------------------------------------------------
    public void transactionEvent ( String EventMessage, EventLogEntryType LogType )
    {
      String stEventMessage = "UserID: " + this._UserId
        + "\r\n" + EventMessage;

      writeEventLog ( stEventMessage, LogType );

    }

    //  ===========================================================================
    /// <summary>
    /// LogWarning method
    /// 
    /// Description:
    /// 
    /// </summary>
    /// <returns>If the event source was created successfully true is returned, otherwise false.</returns>
    //  ---------------------------------------------------------------------------------
    public void writeEventLog ( string EventContent, EventLogEntryType LogType )
    {
      try
      {
        if ( EventContent.Length < 30000 )
        {
          EventLog.WriteEntry ( Global.EventLogSource, EventContent, LogType );

          return;

        }//END less than 30000

        int inLength = 30000;

        for ( int inStartIndex = 0; inStartIndex < EventContent.Length; inStartIndex += 30000 )
        {
          if ( EventContent.Length - inStartIndex < inLength )
          {
            inLength = EventContent.Length - inStartIndex;
          }
          string stContent = EventContent.Substring ( inStartIndex, inLength );

          EventLog.WriteEntry ( Global.EventLogSource, stContent, LogType );

        }//END EventContent interation loop
      }
      catch { }
    }//END WriteLog method 

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
    [WebInvoke ( UriTemplate = "{id}", Method = "PUT" )]
    public string Update ( string id )
    {
      // TODO: Update the given instance of SampleItem in the collection
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

  }
}
