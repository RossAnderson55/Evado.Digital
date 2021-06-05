/***************************************************************************************
 * <copyright file="Evado.Integration.Service.Global.\Global.asax.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2022 EVADO HOLDING PTY. LTD..  All rights reserved.
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
using System.ServiceModel.Activation;
using System.Configuration;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.Routing;
using System.Web.SessionState;
using System.Web.Security;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Web.ApplicationServices;

//Evado. namespace references.
using Evado.Model;
using Evado.UniForm.Model;
using Evado.Digital.Model;
using Evado.Integration.Model;

namespace Evado.Integration.Service
{
  /// <summary>
  /// This class is the global http application class for the integration web service.
  /// </summary>
  public class Global : HttpApplication
  {

    #region Global Variables and Objects

    private const string APPLICATION_EVENT_LOG_SOURCE = "EventLogSource";
    public const string APPLICATION_SERVICE_ROOT = "int/service/";
    public const string APPLICATION_BINARY_FILE_ROOT = "euws/temp/";

    private const string WEB_SERVICE_UNIFORM_CLIENT_ROUTING_VALUE = "service";

    // Variable containing the application path.  Used to generate the base URL.
    public static string EventLogSource = ConfigurationManager.AppSettings [ APPLICATION_EVENT_LOG_SOURCE ];

    /// <summary>
    /// THis object contains the assembly attributes.
    /// </summary>
    public static Evado.Integration.Service.WebAttributes AssemblyAttributes = new Evado.Integration.Service.WebAttributes ( );

    /// <summary>
    /// this field defines the application path.
    /// </summary>
    public static string ApplicationPath = String.Empty;

    /// <summary>
    /// this field defines the application path.
    /// </summary>
    public static string TempPath = @"temp\";

    /// <summary>
    /// this field defines the application path.
    /// </summary>
    public static string LogFilePath = @"logs\";

    /// <summary>
    /// THis global variale contains the registered device file name.
    /// </summary>
    public static string RegisteredDevicesFile = "RegisteredDevices.Txt";

    /// <summary>
    /// This global variable defines if the debug logging is running.
    /// </summary>
    public static bool DebugLogOn = false;

    /// <summary>
    /// This global variable defines if the debug Validation On bypassing Forms Authentcation.
    /// </summary>
    public static bool DebugValidationOn = false;

    /// <summary>
    /// This global variables defines the maximum login attempt setting.
    /// /// </summary>
    public static int MaxLoginAttempts = 3;

    /// <summary>
    /// This variables defines the image storageppath
    /// </summary>
    public static string UniForm_BinaryServiceUrl = "./temp/";

    /// <summary>
    /// This variables defines the image storage path
    /// </summary>
    public static string UniForm_BinaryFilePath = @"temp\";

    /// <summary>
    /// This field contains the service version.
    /// </summary>
    public static float ServiceVersion = 1.4F;

    /// <summary>
    /// This object contains a hash list of global objects.
    /// </summary>
    public static Hashtable GlobalObjects = new Hashtable ( );

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Application Start step

    //==================================================================================
    /// <summary>
    /// This event method performs the application start actions. 
    /// </summary>
    /// <param name="sender">sending object.</param>
    /// <param name="e"> Event arguments</param>
    // ---------------------------------------------------------------------------------
    void Application_Start ( object sender, EventArgs e )
    {
      Global._ServerLog = new StringBuilder ( );
      try
      {
        //
        // get the application path from the runtime.
        //
        Global.ApplicationPath = HttpRuntime.AppDomainAppPath;

        Global.WriteServiceMethod ( "Evado.Integration.Service.Global.Application_Start event method. " );

        //
        // read the web config values into the application.
        //
        this.SetGlobalValues ( );

        //
        // define the registered web rest routes.
        //
        this.RegisterRoutes ( );

        //
        // Output the service log.
        //
        Global.OutputServiceLog ( );

      }
      catch ( Exception Ex )
      {
        string EventMessage = "Evado.Integration.Service.Global.SetGlobalValues method.\r\n" +
          Evado.Model.EvStatics.getException ( Ex );

        EventLog.WriteEntry ( EventLogSource, EventMessage, EventLogEntryType.Error );

        Global.WriteServiceMethod ( EventMessage );

      } // Close catch   

    }//END Application_Start event method

    //===================================================================================
    /// <summary>
    /// This method sets the register routes for handline the HTTP responses.
    /// </summary>
    //-----------------------------------------------------------------------------------
    private void RegisterRoutes ( )
    {
      try
      {
        Global.WriteServiceMethod ( "Evado.Integration.Service.Global.RegisterRoutes method" );

        // Edit the base address of Service1 by replacing the "Service1" string below

        RouteTable.Routes.Add ( new ServiceRoute (
          Global.WEB_SERVICE_UNIFORM_CLIENT_ROUTING_VALUE,
          new WebServiceHostFactory ( ),
          typeof ( Evado.Integration.Service.Query ) ) );

      }
      catch ( Exception Ex )
      {
        string EventMessage = "Evado.Integration.Service.Global.RegisterRoutes method." + Ex.ToString ( );

        EventLog.WriteEntry ( EventLogSource, EventMessage, EventLogEntryType.Error );

        Global.WriteServiceMethod ( EventMessage );
      } // Close catch   

    }//END RegisterRoutes method

    //===================================================================================
    /// <summary>
    /// THis method sets the global values
    /// </summary>
    //-----------------------------------------------------------------------------------
    protected void SetGlobalValues ( )
    {
      Global.WriteServiceMethod ( "Evado.Integration.Service.Global.SetGlobalValues method" );
      String stConnectionSettingKey = "Evado";

      Global.ApplicationPath = HttpRuntime.AppDomainAppPath;
      Global.WriteServiceLogLine ( "Application path: " + Global.ApplicationPath );

      Global.TempPath = Global.ApplicationPath + Global.TempPath;
      Global.WriteServiceLogLine ( "Temp path: " + Global.TempPath );

      if ( ConfigurationManager.AppSettings [ Evado.Model.EvStatics.CONFIG_APPLICATION_LOG_KEY ] != null )
      {
        if ( ( ConfigurationManager.AppSettings [ Evado.Model.EvStatics.CONFIG_APPLICATION_LOG_KEY ] ).ToLower ( ) == "true" )
        {
          Global.DebugLogOn = true;
        }
      }
      Evado.Digital.Bll.EvStaticSetting.DebugOn = Global.DebugLogOn;

      Global.WriteServiceLogLine ( "Debug on: " + Global.DebugLogOn );

      //
      // Set the debug validation on, if true password is not validated
      //
      if ( ConfigurationManager.AppSettings [ Evado.Model.EvStatics.CONFIG_DEBUG_VALIDATION_KEY ] != null )
      {
        if ( ( ConfigurationManager.AppSettings [ Evado.Model.EvStatics.CONFIG_DEBUG_VALIDATION_KEY ] ).ToLower ( ) == "true" )
        {
          Global.DebugValidationOn = true;
        }
      }
      Global.WriteServiceLogLine ( "Debug Validation On: " + Global.DebugValidationOn );

      // 
      // Set the connection string settings.
      // 
      if ( ConfigurationManager.AppSettings [ "ConnectionSetting" ] != null )
      {
        stConnectionSettingKey = (String)
          ConfigurationManager.AppSettings [ "ConnectionSetting" ];

        Evado.Digital.Bll.EvStaticSetting.ConnectionStringKey = stConnectionSettingKey;

        Global.WriteServiceLogLine ( "ConnectionSetting code: " + stConnectionSettingKey );
      }

      Global.WriteServiceLogLine ( "DB ConnectionString :" +
        ConfigurationManager.ConnectionStrings [ stConnectionSettingKey ].ConnectionString );

      //
      // Set the application log path  LogPath
      //
      Global.LogFilePath = Global.ApplicationPath + Global.LogFilePath;
      Global.WriteServiceLogLine ( "Log file path: " + Global.LogFilePath );

      if ( ConfigurationManager.AppSettings [ Evado.Model.EvStatics.CONFIG_LOG_FILE_PATH ] != null )
      {
        string stConfigLogPath = ConfigurationManager.AppSettings [ Evado.Model.EvStatics.CONFIG_LOG_FILE_PATH ];

        Global.LogFilePath = Evado.Model.EvStatics.Files.updateDirectoryPath (
          Global.ApplicationPath,
          Global.LogFilePath,
          stConfigLogPath );

        Global.WriteServiceLogLine ( Evado.Model.EvStatics.Files.DebugLog );
      }

      Global.WriteServiceLogLine ( "Log file path: " + Global.LogFilePath );

      //
      // Set the application log path  RepositoryFilePath
      //
      Global.UniForm_BinaryFilePath = Global.ApplicationPath + Global.UniForm_BinaryFilePath;

      if ( ConfigurationManager.AppSettings [ Evado.UniForm.Model.EuStatics.CONFIG_UNIFORM_BINARY_FILE_KEY ] != null )
      {
        string stBinaryFilePath = ConfigurationManager.AppSettings [ Evado.UniForm.Model.EuStatics.CONFIG_UNIFORM_BINARY_FILE_KEY ];

        Global.UniForm_BinaryFilePath = Evado.Model.EvStatics.Files.updateDirectoryPath (
          Global.ApplicationPath,
          Global.UniForm_BinaryFilePath,
          stBinaryFilePath );

        Global.WriteServiceLogLine ( Evado.Model.EvStatics.Files.DebugLog );
      }

      Global.WriteServiceLogLine ( "UniForm binary File path: " + Global.UniForm_BinaryFilePath );

      //
      // Set the application log path
      //
      if ( ConfigurationManager.AppSettings [ Evado.UniForm.Model.EuStatics.CONFIG_UNIFORM_BINARY_URL_KEY ] != null )
      {
        string stBinaryFileUrl = ConfigurationManager.AppSettings [ Evado.UniForm.Model.EuStatics.CONFIG_UNIFORM_BINARY_URL_KEY ];

        if ( stBinaryFileUrl != String.Empty )
        {
          Global.UniForm_BinaryServiceUrl = stBinaryFileUrl;
        }

      }

      Global.WriteServiceLogLine ( "Binary filed URL: " + Global.UniForm_BinaryServiceUrl );

      string repositoryFilePath = Global.ApplicationPath + @"FilePath";
      Global.WriteServiceLogLine ( "DEFAULT File Repository: " + repositoryFilePath );

      if ( ConfigurationManager.AppSettings [ Evado.Digital.Model.EvcStatics.CONFIG_RESPOSITORY_FILE_PATH_KEY ] != null )
      {
        string stConfigRepositoryFilePath = ConfigurationManager.AppSettings [ Evado.Digital.Model.EvcStatics.CONFIG_RESPOSITORY_FILE_PATH_KEY ];

        Global.WriteServiceLogLine ( "Config repository file path: " + stConfigRepositoryFilePath );

        repositoryFilePath = Evado.Model.EvStatics.Files.updateDirectoryPath (
          Global.ApplicationPath,
          repositoryFilePath,
          stConfigRepositoryFilePath );

        Global.WriteServiceLogLine ( Evado.Model.EvStatics.Files.DebugLog );
      }

      ConfigurationManager.AppSettings [ Evado.Digital.Model.EvcStatics.CONFIG_RESPOSITORY_FILE_PATH_KEY ] = repositoryFilePath;

      Global.WriteServiceLogLine ( "SET Repository file path: " + repositoryFilePath );

      //
      // Set the service version.
      //
      Global.WriteServiceLogLine ( "MinorVersion: " + Global.AssemblyAttributes.MinorVersion );

      Global.ServiceVersion = Evado.Model.EvStatics.decodeMinorVersion (
        Global.AssemblyAttributes.MinorVersion );

      Global.WriteServiceLogLine ( "ServiceVersion: " + Global.ServiceVersion );

    }//END SetGlobalValues Method

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Application End section

    //  =======================================================================================
    /// <summary>
    ///  This method is trigerred when the application is ending.
    /// </summary>
    /// <param name="sender">Event object</param>
    /// <param name="e">Event arguments</param>
    //  ---------------------------------------------------------------------------------------
    protected void Application_End ( Object sender, EventArgs e )
    {
      Global.WriteServiceMethod ( "Evado.Integration.Service.Global.Application_End event method" );
      try
      {
        string sEvent = "EVADO Application End"
          + "\r\n Deleting " + Application.Contents.Count + " application variables. "; ;

        Global.WriteDebugLogLine ( "Deleting " + Application.Contents.Count + " application variables. " );

        //
        // Delete all of the session conteent.
        // 
        if ( Application != null )
        {
          Application.Clear ( );
        }

        // 
        // Write an event entry when the application ends.
        // 
        EventLog.WriteEntry ( 
          EventLogSource, 
          sEvent, 
          EventLogEntryType.Information );

        Global.OutputtDebugLog ( );
      }
      catch ( Exception Ex )
      {
        EventLog.WriteEntry ( 
          EventLogSource, 
          Evado.Model.EvStatics.getException ( Ex ), 
          EventLogEntryType.Error );
      } // Close catch   
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Static Application log methods

    //
    // Define the debug lot string builder.
    //
    private static System.Text.StringBuilder _ServerLog = new System.Text.StringBuilder ( );

    private const String CONST_SERVICE_LOG_FILE_NAME = @"\service-log-";

    //  =================================================================================
    /// <summary>
    ///   This static method removes a user from the online user list.
    /// 
    /// </summary>
    //   ---------------------------------------------------------------------------------
    public static void WriteServiceMethod ( String Value )
    {
      Global._ServerLog.AppendLine ( Evado.Model.EvStatics.CONST_METHOD_START + Value );
    }

    //  =================================================================================
    /// <summary>
    ///   This static method removes a user from the online user list.
    /// 
    /// </summary>
    //   ---------------------------------------------------------------------------------
    public static void WriteServiceLog ( String Value )
    {
      Global._ServerLog.Append ( Value );
    }

    //  =================================================================================
    /// <summary>
    ///   This static method removes a user from the online user list.
    /// 
    /// </summary>
    //   ---------------------------------------------------------------------------------
    public static void WriteServiceLogLine ( String Value )
    {
      Global._ServerLog.AppendLine ( Value );
    }

    //  =================================================================================
    /// <summary>
    ///   This static method removes a user from the online user list.
    /// 
    /// </summary>
    //   ---------------------------------------------------------------------------------
    public static void WriteServiceLogFormat ( String Template, String [ ] Values )
    {
      Global._ServerLog.AppendFormat ( Template, Values );
    }

    //  =================================================================================
    /// <summary>
    ///   This static method removes a user from the online user list.
    /// 
    /// </summary>
    //   ---------------------------------------------------------------------------------
    public static void OutputServiceLog ()
    {
      String ServiceLogFileName = Global.LogFilePath + CONST_SERVICE_LOG_FILE_NAME
        + DateTime.Now.ToString ( "yy-MM" ) + ".txt";

      String stContent = Evado.Model.EvStatics.getHtmlAsString ( Global._ServerLog.ToString ( ) );

      // 
      // Open the stream to the file.
      // 
      using ( System.IO.StreamWriter sw = new System.IO.StreamWriter ( ServiceLogFileName, true ) )
      {
        sw.Write ( stContent );

      }// End StreamWriter.

    }//END writeOutDebugLog method

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Static Debug log methods

    private static System.Text.StringBuilder _DebuLog = new System.Text.StringBuilder ( );

    private const String CONST_DEBUG_LOG_FILE_NAME = @"\debug-log";

    //  =================================================================================
    /// <summary>
    ///   This static method removes a user from the online user list.
    /// 
    /// </summary>
    //   ---------------------------------------------------------------------------------
    public static void ClearDebugLog ( )
    {
      Global._DebuLog = new StringBuilder ( );
    }

    //  =================================================================================
    /// <summary>
    ///   This static method removes a user from the online user list.
    /// 
    /// </summary>
    //   ---------------------------------------------------------------------------------
    public static void WriteDebugLogClass ( String Value )
    {
      //
      // IF Debug is turned off exit method.
      //
      if ( Global.DebugLogOn == false )
      {
        return;
      }

      Global._DebuLog.AppendLine (
        "\r\n\r\n=================================================================================="
      + "===================================\r\n" + Value );
    }

    //  =================================================================================
    /// <summary>
    ///   This static method removes a user from the online user list.
    /// 
    /// </summary>
    //   ---------------------------------------------------------------------------------
    public static void WriteDebugLogMethod ( String Value )
    {
      //
      // IF Debug is turned off exit method.
      //
      if ( Global.DebugLogOn == false )
      {
        return;
      }

      Global._DebuLog.AppendLine ( Evado.Model.EvStatics.CONST_METHOD_START + Value );
    }

    //  =================================================================================
    /// <summary>
    ///   This static method removes a user from the online user list.
    /// 
    /// </summary>
    //   ---------------------------------------------------------------------------------
    public static void WriteDebugLog ( String Value )
    {
      //
      // IF Debug is turned off exit method.
      //
      if ( Global.DebugLogOn == false )
      {
        return;
      }
      Global._DebuLog.Append ( Value );
    }

    //  =================================================================================
    /// <summary>
    ///   This static method removes a user from the online user list.
    /// 
    /// </summary>
    //   ---------------------------------------------------------------------------------
    public static void WriteDebugLogLine ( String Value )
    {
      //
      // IF Debug is turned off exit method.
      //
      if ( Global.DebugLogOn == false )
      {
        return;
      }
      Global._DebuLog.AppendLine ( Value );
    }

    //  =================================================================================
    /// <summary>
    ///   This static method removes a user from the online user list.
    /// 
    /// </summary>
    //   ---------------------------------------------------------------------------------
    public static void WriteDebugLogFormat ( String Template, String [ ] Values )
    {
      //
      // IF Debug is turned off exit method.
      //
      if ( Global.DebugLogOn == false )
      {
        return;
      }
      Global._DebuLog.AppendFormat ( Template, Values );
    }

    //  =================================================================================
    /// <summary>
    ///   This static method removes a user from the online user list.
    /// 
    /// </summary>
    //   ---------------------------------------------------------------------------------
    public static void OutputtDebugLog ( )
    {
      //
      // Define the filename
      //
      String LogFileName = Global.LogFilePath + CONST_DEBUG_LOG_FILE_NAME
        + DateTime.Now.ToString ( "yy-MM" ) + ".txt";

      //
      // IF Debug is turned off exit method.
      //
      if ( Global.DebugLogOn == false )
      {
        return;
      }

      //
      // if the debug log path is defined output the debug log to the given path.
      //
      if ( Global.ApplicationPath == String.Empty )
      {
        return;
      }

      //
      // Output the debug log to debug log page.
      //
      String stContent = String.Empty;

      if ( Global._DebuLog.Length == 0 )
      {
        stContent = "EVADO eClinical ASP.NET - DEBUG LOG\r\n"
          + "Saved: " + DateTime.Now.ToString ( "dd MMM yyyy HH:mm:ss" )
          + "\r\nNo Debug Content";
      }
      else
      {
        stContent += "EVADO eClinical ASP.NET - DEBUG LOG\r\n"
          + "Saved: " + DateTime.Now.ToString ( "dd MMM yyyy HH:mm:ss" )
          + "\r\n"
          + Global._DebuLog.ToString ( );
      }

      stContent = Evado.Model.EvStatics.getHtmlAsString ( stContent );

      // 
      // Open the stream to the file.
      // 
      using ( System.IO.StreamWriter sw = new System.IO.StreamWriter ( LogFileName ) )
      {
        sw.Write ( stContent );

      }// End StreamWriter.

    }//END writeOutDebugLog method


    #endregion

    #region Session Start step

    protected void Session_Start ( Object sender, EventArgs e )
    {

    }// Close Session_Start Event method.

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
  }
}
