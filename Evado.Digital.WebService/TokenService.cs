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
using Evado.Model.UniForm;
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
  public class TokenService
  {
    #region Class initialisation methods
    //  ===================================================================================
    /// <summary>
    /// This method initialises the service and sets the context object for the users session.
    /// </summary>
    //  -----------------------------------------------------------------------------------
    public TokenService ( )
    {
      this._Context = HttpContext.Current;
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class Global objects and values.


    /// <summary>
    /// This object contains the HTTP context for the connection to the web client.
    /// </summary>
    private HttpContext _Context;

    /// <summary>
    /// This global parameter contains the current url.
    /// </summary>
    private String _CurrentServiceURL = String.Empty;

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
    [WebInvoke ( UriTemplate = "/{ClientVersion}", Method = "POST" )]
    public Stream updateTokenUserProfileService ( string ClientVersion, Stream content )
    {
      Global.ClearApplicationLog ( );
      this.LogMethod ( "updateTokenUserProfileService web service" );
      this.LogDebug ( "ClientVersion content {0}", ClientVersion );

      String SessionId = String.Empty;
      try
      {
        //
        // Initialise the methods variables and object.
        //
        string json = String.Empty;
        Evado.Model.EusTokenUserProfile tokenUserProfile = new Evado.Model.EusTokenUserProfile ( );
        String hostUrl = this._Context.Request.Headers [ "Host" ];

        this.LogDebug ( "hostUrl: {0} ", hostUrl );
        this.LogDebug ( "Request.Url: {0} ", this._Context.Request.Url );
        this.LogDebug ( "Request.UserHostAddress: {0} ", this._Context.Request.UserHostAddress );
        this.LogDebug ( "Request.UserHostName: {0} ", this._Context.Request.UserHostName );

        if ( Global.ValidTokenUserProfileIpAddresses.Contains ( this._Context.Request.UserHostAddress ) == false )
        {
          this.LogDebug ( "ValidTokenUserProfileIpAddresses NOT found"  );

          return this.WebServiceResponse ( "ERROR" );
        }

        //
        // first load the POST payload into a string
        // the POST content comes from the content param above
        // as it is the only param that is not listed in the URI template
        //
        string content_value = new StreamReader ( content ).ReadToEnd ( );

        this.LogDebug ( "Post content \r\n{0}", content_value );
        //
        // Log the transaction
        if ( content_value == String.Empty )
        {
          return this.WebServiceResponse ( "ERROR" );
        }

        //
        // serialise the client groupCommand
        //
        Evado.Model.EusTokenUserProfile UserProfie = JsonConvert.DeserializeObject<Evado.Model.EusTokenUserProfile> ( content_value );

        this.LogDebug ( "AppId: " + UserProfie.ApplicationID );
        this.LogDebug ( "TokenId: " + UserProfie.Token );
        this.LogDebug ( "UserId: " + UserProfie.UserId );
        this.LogDebug ( "GivenName: " + UserProfie.GivenName );
        this.LogDebug ( "FamilyName: " + UserProfie.FamilyName );
        this.LogDebug ( "EmailAddress: " + UserProfie.EmailAddress );
        this.LogDebug ( "UserType: " + UserProfie.UserType );
        this.LogDebug ( "UserStatus: " + UserProfie.UserStatus );

        EvEventCodes result = this.UpdateTokenUserProfile ( UserProfie );
        //
        //  send the web service response to the device app.
        //
        return this.WebServiceResponse ( result.ToString() );
      }
      catch ( Exception Ex )
      {
        string EventMessage = "Evado.Digital.WebService.TokenService event method.\r\n" + EvStatics.getException ( Ex );

        this.LogEvent ( EventMessage );

        return WebServiceResponse (EvEventCodes.Token_User_Profile_Update_Error.ToString() );

      } // Close catch   

    }//END Create web method. 

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Private Methods.
    // =====================================================================================
    // -------------------------------------------------------------------------------------

    // =====================================================================================
    /// <summary>
    /// This method updates the TokenUserProfile object in the database.
    /// </summary>
    /// <param name="UserProfile">EusTokenUserProfile object</param>
    /// <returns>EvEventCodes enumerated value.</returns>
    // -------------------------------------------------------------------------------------
    private EvEventCodes UpdateTokenUserProfile ( EusTokenUserProfile UserProfile )
    {
      this.LogMethod ( "UpdateTokenUserProfile" );
      //
      // initialise the methods variables and objects.
      //
      EvUserProfileBase serviceUserProfile = new EvUserProfileBase ( );
      serviceUserProfile.UserId = "TokenService";
      serviceUserProfile.CommonName = "Token Service";

      //
      // Set the device client service session state.
      //
      IntegrationServices integrationServices = new IntegrationServices (
        Global.ServiceVersion,
        Global.GlobalObjectList,
        Global.ApplicationPath,
        serviceUserProfile,
        Global.UniForm_BinaryFilePath,
        Global.UniForm_BinaryServiceUrl );

      integrationServices.LoggingLevel = 5; 
      integrationServices.EventLogSource = Global.EventLogSource;

      //
      // Update the token user profile.
      //
      EvEventCodes result = integrationServices.UpdateTokenUserProfile ( UserProfile );

      this.LogClass ( integrationServices.Log );

      this.LogMethodEnd ( "UpdateTokenUserProfile" );
      return result;
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Web Service and Session Methods.

    // =====================================================================================
    /// <summary>
    /// This method generates the Web service request to be sent to the device app client.
    /// </summary>
    /// <returns>Stream: of the content to be sent to the device client.</returns>
    // -------------------------------------------------------------------------------------
    public Stream WebServiceResponse ( String Result )
    {
      //
      // Initialise the methods variables and object.
      //
      this.LogMethod ( "WebServiceResponse" );
      string json = String.Empty;
      JsonSerializerSettings jsonSettings = new JsonSerializerSettings
        {
          NullValueHandling = NullValueHandling.Ignore
        };

      json = "{\"RESULT\":\"" + Result + "\"}";

      //
      // output the jason ad a memory stream to send to the client.
      //
      WebOperationContext.Current.OutgoingResponse.ContentType = "application/json";
      var result = new MemoryStream ( ASCIIEncoding.UTF8.GetBytes ( json ) );

      //
      // Log the user's transaction duration.
      //
      TimeSpan duration = DateTime.Now - this._StartTime;

      this.LogEvent ( "Token User Update transaction date " + this._StartTime.ToString ( "dd MMM yyyy HH:mm:ss" )
        + " duration " + duration.ToString ( ) );

      this.LogMethodEnd ( "WebServiceResponse" );
      Global.OutputApplicationLog ( );
      Global.OutputEventLog ( );

      //
      // Return the web service reponse.
      //
      return result;

    }//END WebServiceResponse method

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
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

    const string CONST_NAME_SPACE = "Evado.UniForm.TokenService.";

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
      + TokenService.CONST_NAME_SPACE + Value );
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
        + TokenService.CONST_NAME_SPACE + Value + " method" );
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
