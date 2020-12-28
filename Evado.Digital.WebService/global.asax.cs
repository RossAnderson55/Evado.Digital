/***************************************************************************************
 * <copyright file="Evado.Digital.WebService\Global.asax.cs" company="EVADO HOLDING PTY. LTD.">
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

//Evado namespace references.
using Evado.Model.UniForm;
using Evado.Model;
using Evado.Model.Digital;
//using Evado.UniForm.Dal;

namespace Evado.Digital.WebService
{
  /// <summary>
  /// This is the global htmpApplication object for the Evado eClinical web service.
  /// </summary>
  public class Global : HttpApplication
  {
    #region Global Variables and Objects

    public const string APPLICATION_SERVICE_ROOT = "euws/client/";
    public const string APPLICATION_BINARY_FILE_ROOT = "euws/temp/";

    private const string WEB_SERVICE_UNIFORM_CLIENT_ROUTING_VALUE = "client";

    private const string CONFIG_DEV_LOGGING = "DEV_LOGGING";


    // Variable containing the application path.  Used to generate the base URL.
    public static string EventLogSource = ConfigurationManager.AppSettings [ Evado.Model.EvStatics.CONFIG_EVENT_LOG_KEY ];

    /// <summary>
    /// THis object contains the assembly attributes.
    /// </summary>
    public static Evado.Digital.WebService.WebAttributes AssemblyAttributes = new Evado.Digital.WebService.WebAttributes ( );

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
    /// This global variable contains the logo filename.
    /// </summary>
    public static string LogoFilename = String.Empty;

    /// <summary>
    /// This global variable contains the service identifier
    /// </summary>
    public static string ServiceId = "Default_Service";

    /// <summary>
    /// This global variable defines if the debug logging is running.
    /// </summary>
    public static int LoggingLevel = 0;

    /// <summary>
    /// This global variable defines if the debug Validation On bypassing Forms Authentcation.
    /// </summary>
    public static bool DebugValidationOn = false;

    /// <summary>
    /// This global variables defines the maximum login attempt setting.
    /// /// </summary>
    public static int MaxLoginAttempts = 3;

    /// <summary>
    /// This global variable defines if the debug Validation On bypassing Forms Authentcation.
    /// </summary>
    public static bool DeleteSessionOnExit = false;

    /// <summary>
    /// This global variable contains the list of registered devices.
    /// </summary>
    public static string RegisteredDevices = String.Empty;

    /// <summary>
    /// This variable contains a list of service URLs.
    /// </summary>
    public static List<EvOption> ServiceUrls = new List<EvOption> ( );

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
    public static float ServiceVersion = 2.1F;


    /// <summary>
    /// This object contains a hash list of global objects.
    /// </summary>
    public static Hashtable GlobalObjectList = new Hashtable ( );


    private static Evado.UniForm.EuTestCaseRecorder TestCaseRecorder = null;

    private static int TestSectionNo = 1;

    /// <summary>
    /// This object contains a hash list of global objects.
    /// </summary>
    private static bool RecordTestCases = false;

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
      Global.LogMethod ( "Application_Start event method. " );
      try
      {
        //
        // get the application path from the runtime.
        //
        Global.ApplicationPath = HttpRuntime.AppDomainAppPath;

        //
        // Redefine the Global object used for all user sessions data.
        //
        Global.GlobalObjectList = new Hashtable ( );

        //
        // read the web config values into the application.
        //
        this.SetGlobalValues ( );

        //
        // define the registered web rest routes.
        //
        this.RegisterRoutes ( );

        //
        // Turn on test case recorder if needed.
        //
        if ( Global.RecordTestCases == true
          && Global.TestCaseRecorder == null )
        {
          Global.TestCaseRecorder = new Evado.UniForm.EuTestCaseRecorder ( Global.ApplicationPath );

          if ( LoggingLevel > 4 )
          {
            Global.TestCaseRecorder.LoggingOn = true;
          }

          Global.TestCaseRecorder.SectionNo = 1;
        }

        //
        // Delete the old global objects.
        //
        Global.deleteOldGlobalObjects ( );

        //
        // Output the service log.
        //
        Global.OutputEventLog ( );

      }
      catch ( Exception Ex )
      {
        string EventMessage = "Evado.Digital.WebService.SetGlobalValues method.\r\n" +
          Evado.Model.EvStatics.getException ( Ex );

        EventLog.WriteEntry ( EventLogSource, EventMessage, EventLogEntryType.Error );

        Global.LogMethod ( EventMessage );

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
        Global.LogMethod ( "RegisterRoutes method" );

        //
        // Define the route for the client service.
        //

        RouteTable.Routes.Add ( new ServiceRoute (
          Global.WEB_SERVICE_UNIFORM_CLIENT_ROUTING_VALUE,
          new WebServiceHostFactory ( ),
          typeof ( Evado.Digital.WebService.ClientService ) ) );

      }
      catch ( Exception Ex )
      {
        string EventMessage = "Evado.Digital.WebService.RegisterRoutes method.\r\n" + Ex.ToString ( );

        EventLog.WriteEntry ( EventLogSource, EventMessage, EventLogEntryType.Error );

        Global.LogMethod ( EventMessage );
      } // Close catch   

    }//END RegisterRoutes method

    //===================================================================================
    /// <summary>
    /// THis method sets the global values
    /// </summary>
    //-----------------------------------------------------------------------------------
    private void SetGlobalValues ( )
    {
      Global.LogMethod ( "SetGlobalValues method" );

      Global.ApplicationPath = HttpRuntime.AppDomainAppPath;
       Global.LogValue ( "Application path: " + Global.ApplicationPath );

      Global.TempPath = Global.ApplicationPath + Global.TempPath;
       Global.LogValue ( "Temp path: " + Global.TempPath );

      if ( ConfigurationManager.AppSettings [ Evado.Model.EvStatics.CONFIG_APPLICATION_LOG_KEY ] != null )
      {
        int loglevel = 0;
        string value = ConfigurationManager.AppSettings [ Evado.Model.EvStatics.CONFIG_APPLICATION_LOG_KEY ];

        if ( int.TryParse ( value, out loglevel ) == true )
        {
          Global.LoggingLevel = loglevel;
          if ( Global.LoggingLevel < 0 )
          {
            Global.LoggingLevel = 0;
          }
          if ( Global.LoggingLevel > 5 )
          {
            Global.LoggingLevel = 5;
          }

        }
      }
       Global.LogValue ( "Application logging level: " + Global.LoggingLevel );

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
       Global.LogValue ( "Debug Validation On: " + Global.DebugValidationOn );

      //
      // Set the debug validation on, if true password is not validated
      //
      if ( ConfigurationManager.AppSettings [ Global.CONFIG_DEV_LOGGING ] != null )
      {
        if ( ( ConfigurationManager.AppSettings [ Global.CONFIG_DEV_LOGGING ] ).ToLower ( ) == "true" )
        {
          Global.DevelopmentLogging = true;
        }
      }
      Global.LogValue ( "DevelopmentLogging: " + Global.DevelopmentLogging );

      //
      // Set the delete session on exit, if true 
      //
      if ( ConfigurationManager.AppSettings [ Evado.Model.UniForm.EuStatics.CONFIG_DELETE_SESSION_ON_EXIT_KEY ] != null )
      {
        Global.DeleteSessionOnExit = false;

        string stValue = ConfigurationManager.AppSettings [ Evado.Model.UniForm.EuStatics.CONFIG_DELETE_SESSION_ON_EXIT_KEY ];
        if ( stValue.ToLower ( ) == "true"
          || stValue.ToLower ( ) == "yes" )
        {
          Global.DeleteSessionOnExit = true;
        }
      }
       Global.LogValue ( "Delete User Session of Exit: " + Global.DeleteSessionOnExit );

      //
      // Set the debug record test cases if true
       //
       Global.RecordTestCases = false;
      if ( ConfigurationManager.AppSettings [ Evado.UniForm.EuTestCaseRecorder.CONST_TEST_CASE_PATH ] != null )
      {
        Global.RecordTestCases = true;
      }
      Global.LogValue ( "RecordTestCases: " + Global.RecordTestCases );

      
      //
      // Set the application log path
      //
      if ( ConfigurationManager.AppSettings [ Evado.Model.UniForm.EuStatics.CONFIG_UNIFORM_SERVICE_ID_KEY ] != null )
      {
        string stServiceId = ConfigurationManager.AppSettings [ Evado.Model.UniForm.EuStatics.CONFIG_UNIFORM_SERVICE_ID_KEY ];

        if ( stServiceId != String.Empty )
        {
          Global.ServiceId = stServiceId;
        }

      }
       Global.LogValue ( "Service Id: " + Global.ServiceId );

      //
      // Set the application log path  LogPath
      //
      Global.LogFilePath = Global.ApplicationPath + Global.LogFilePath;
       Global.LogValue ( "Log file path: " + Global.LogFilePath );

      if ( ConfigurationManager.AppSettings [ Evado.Model.EvStatics.CONFIG_LOG_FILE_PATH ] != null )
      {
        string stConfigLogPath = ConfigurationManager.AppSettings [ Evado.Model.EvStatics.CONFIG_LOG_FILE_PATH ];

         Global.LogValue ( "stConfigLogPath: " + stConfigLogPath );
        Global.LogFilePath = Evado.Model.EvStatics.Files.updateDirectoryPath (
          Global.ApplicationPath,
          Global.LogFilePath,
          stConfigLogPath );

        //Global.WriteServiceLogLine ( Evado.Model.EvStatics.Files.DebugLog );
      }

       Global.LogValue ( "Log file path: " + Global.LogFilePath );

      //
      // Set the application log path  RepositoryFilePath
      //
      Global.UniForm_BinaryFilePath = Global.ApplicationPath + Global.UniForm_BinaryFilePath;

      if ( ConfigurationManager.AppSettings [ Evado.Model.UniForm.EuStatics.CONFIG_UNIFORM_BINARY_FILE_KEY ] != null )
      {
        string stBinaryFilePath = ConfigurationManager.AppSettings [ Evado.Model.UniForm.EuStatics.CONFIG_UNIFORM_BINARY_FILE_KEY ];

        Global.UniForm_BinaryFilePath = Evado.Model.EvStatics.Files.updateDirectoryPath (
          Global.ApplicationPath,
          Global.UniForm_BinaryFilePath,
          stBinaryFilePath );

        //Global.WriteServiceLogLine ( Evado.Model.EvStatics.Files.DebugLog );
      }

       Global.LogValue ( "UniForm binary File path: " + Global.UniForm_BinaryFilePath );

      //
      // Set the application log path
      //
      if ( ConfigurationManager.AppSettings [ Evado.Model.UniForm.EuStatics.CONFIG_UNIFORM_BINARY_URL_KEY ] != null )
      {
        string stBinaryFileUrl = ConfigurationManager.AppSettings [ Evado.Model.UniForm.EuStatics.CONFIG_UNIFORM_BINARY_URL_KEY ];

        if ( stBinaryFileUrl != String.Empty )
        {
          Global.UniForm_BinaryServiceUrl = stBinaryFileUrl;
        }

      }

       Global.LogValue ( "Binary filed URL: " + Global.UniForm_BinaryServiceUrl );

      string repositoryFilePath = Global.ApplicationPath + @"FilePath";
       Global.LogValue ( "DEFAULT File Repository: " + repositoryFilePath );

      if ( ConfigurationManager.AppSettings [ Evado.Model.Digital.EvcStatics.CONFIG_RESPOSITORY_FILE_PATH_KEY ] != null )
      {
        string stConfigRepositoryFilePath = ConfigurationManager.AppSettings [ Evado.Model.Digital.EvcStatics.CONFIG_RESPOSITORY_FILE_PATH_KEY ];

         Global.LogValue ( "Config repository file path: " + stConfigRepositoryFilePath );

        repositoryFilePath = Evado.Model.EvStatics.Files.updateDirectoryPath (
          Global.ApplicationPath,
          repositoryFilePath,
          stConfigRepositoryFilePath );
      }

      ConfigurationManager.AppSettings [ Evado.Model.Digital.EvcStatics.CONFIG_RESPOSITORY_FILE_PATH_KEY ] = repositoryFilePath;

       Global.LogValue ( "SET Repository file path: " + repositoryFilePath );

      //
      // The logo file
      //
      if ( ConfigurationManager.AppSettings [ Evado.Model.EvStatics.CONFIG_LOGO_FILE_NAME ] != null )
      {
        string stConfigRepositoryFilePath = ConfigurationManager.AppSettings [ Evado.Model.EvStatics.CONFIG_LOGO_FILE_NAME ];

        if ( stConfigRepositoryFilePath != string.Empty )
        {
          Global.LogoFilename = stConfigRepositoryFilePath;
        }
      }
       Global.LogValue ( "LogolUrl: " + Global.LogoFilename );

      //
      // Read in the service URL for redirection.
      //
      if ( ConfigurationManager.AppSettings [ "WebServiceUris" ] != null )
      {
        string stWebServices = ConfigurationManager.AppSettings [ "WebServiceUris" ];
        stWebServices = stWebServices.Replace ( ",", ";" );
        string [ ] arrServices = stWebServices.Split ( ';' );

        for ( int i = 0; i < arrServices.Length; i++ )
        {
          String [ ] arr = arrServices [ i ].Split ( '^' );
          if ( arr.Length > 1 )
          {
            EvOption option = new EvOption ( arr [ 0 ].ToLower ( ), arr [ 1 ].ToLower ( ) );
            Global.ServiceUrls.Add ( option );

             Global.LogValue ( "'" + Global.ServiceUrls [ i ].Value + "' - '" + Global.ServiceUrls [ i ].Description + "'" );
          }
        }
      }

      //
      // Set the service version.
      //
       Global.LogValue ( "MinorVersion: " + Global.AssemblyAttributes.MinorVersion );

      Global.ServiceVersion = Evado.Model.EvStatics.decodeMinorVersion (
        Global.AssemblyAttributes.MinorVersion );

       Global.LogValue ( "ServiceVersion: " + Global.ServiceVersion );

      if ( ConfigurationManager.AppSettings [ Evado.Model.Digital.EvcStatics.CONFIG_WEB_CLIENT_URL_KEY ] != null )
      {
        string questionnaire_URL = ConfigurationManager.AppSettings [ Evado.Model.Digital.EvcStatics.CONFIG_WEB_CLIENT_URL_KEY ];

         Global.LogValue ( "questionnaire_URL: " + questionnaire_URL );

      }

      if ( ConfigurationManager.AppSettings [ "ConnectionSetting" ] != null )
      {
        string connectionStringKey = ConfigurationManager.AppSettings [ "ConnectionSetting" ];

         Global.LogValue ( "connectionStringKey: " + connectionStringKey );

      }

      // 
      // Open the stream to the file.
      // 
      using ( StreamReader sr = new StreamReader ( Global.ApplicationPath + RegisteredDevicesFile ) )
      {
        Global.RegisteredDevices = sr.ReadToEnd ( );

      }// End StreamWriter.

    }//END SetGlobalValues Method

    //===================================================================================
    /// <summary>
    /// This method fixes the numeric validation for mobile devices.
    /// Changing Min_Value to MinValue 
    /// </summary>
    //-----------------------------------------------------------------------------------
    public static void RecordTestCase (
      Evado.Model.UniForm.Command PageCommand,
      Evado.Model.UniForm.Command ExitCommand,
      Evado.Model.UniForm.AppData PageData )
    {
      Global.LogMethod ( "RecordTestCase method." );

      if ( Global.RecordTestCases == false )
      {
        Global.LogMethodEnd ( "RecordTestCase" );
        return;
      }

      Global.TestCaseRecorder.SectionNo  = Global.TestSectionNo;
      Global.TestCaseRecorder.saveTestCases ( PageCommand, ExitCommand, PageData );

      Global.LogService( Global.TestCaseRecorder.Log );

      Global.TestSectionNo++; 

      Global.LogMethodEnd ( "RecordTestCase" );
    }

    //===================================================================================
    /// <summary>
    /// THis method sets the global values
    /// </summary>
    //-----------------------------------------------------------------------------------
    public static void deleteOldGlobalObjects ( )
    {
      Global.LogMethod ( "deleteOldGlobalObjects method" );

      //
      // Initialise the methods variables and objects.
      //
      int periodHours = 0;

      if ( ConfigurationManager.AppSettings [ Evado.Model.UniForm.EuStatics.CONFIG_DELETE_HR_PERIOD_KEY ] != null )
      {
        string value = ConfigurationManager.AppSettings [ Evado.Model.UniForm.EuStatics.CONFIG_DELETE_HR_PERIOD_KEY ];

        periodHours = Evado.Model.EvStatics.getInteger ( value );

        Global.LogDebugValue ( "CONFIG: Delete period: " + value );

      }

      Global.LogDebugValue ( "hours period: " + periodHours );

      DateTime datePeriod = DateTime.Now.AddHours ( -periodHours );

      Global.LogDebugValue ( "datePeriod: " + datePeriod.ToString ( "dd-MMM-yy hh:mm" ) );

       Global.LogValue ( "Global Object list length: " + Global.GlobalObjectList.Count );

      //
      // debug list the global objects.
      //
      foreach ( System.Collections.DictionaryEntry entry in Global.GlobalObjectList )
      {
        String stKey = entry.Key.ToString ( );

        Global.LogDebugValue ( "Entry key: " + stKey );

        if ( stKey.Contains ( Evado.Model.UniForm.EuStatics.GLOBAL_DATE_STAMP ) == true )
        {
          DateTime dateStamp = ( DateTime ) Global.GlobalObjectList [ entry.Key ];

           Global.LogDebugValue ( "dateStamp: " + dateStamp.ToString ( "dd-MMM-yy hh:mm" ) );

          //
          // If the date stamp is older than the delete date delete object.
          //
          if ( dateStamp < datePeriod )
          {
            Global.deleteGlobalObjectEntries ( entry );
          }
        }
      }

      Global.LogValue ( "Global Object processed list length: " + Global.GlobalObjectList.Count );

      Global.LogMethodEnd ( "deleteOldGlobalObjects" );

    }//END SetGlobalValues Method

    // ==================================================================================
    /// <summary>
    /// This method returns the Uri for the selected serviceId
    /// </summary>
    /// <param name="Entry">System.Collections.DictionaryEntry: dictionary entry</param>
    /// <returns>String containing the UnIFORM Uri</returns>
    // ----------------------------------------------------------------------------------
    private static void deleteGlobalObjectEntries ( System.Collections.DictionaryEntry Entry )
    {
      Global.LogMethod ( "deleteOldGlobalObjects method" );
       Global.LogDebugValue ( "Item.key: " + Entry.Key.ToString ( ) );

      //
      // Initialise the methods variables and objects.
      //
      String UserName = Entry.Key.ToString ( );
      UserName = UserName.ToUpper ( );
      UserName = UserName.Replace ( Evado.Model.UniForm.EuStatics.GLOBAL_DATE_STAMP, String.Empty );

      Global.LogDebugValue ( "User: " + UserName );
      String ClinicalObject_Key = UserName + Evado.Model.Digital.EvcStatics.SESSION_CLINICAL_OBJECT;

      Global.LogDebugValue ( "Clinical Key: " + ClinicalObject_Key );

      String HistoryList_Key = UserName + Evado.Model.UniForm.EuStatics.GLOBAL_COMMAND_HISTORY;

      Global.LogDebugValue ( "Command History Key: " + HistoryList_Key );

      Global.GlobalObjectList.Remove ( Entry );
      Global.GlobalObjectList.Remove ( ClinicalObject_Key );
      Global.GlobalObjectList.Remove ( HistoryList_Key );

      Global.LogMethodEnd ( "deleteOldGlobalObjects" );

    }//END deleteGlobalObjectEntries method

    //===================================================================================
    /// <summary>
    /// This method returns the Uri for the selected serviceId
    /// </summary>
    /// <param name="ServiceUserProfile">EvBaseUserProfile object</param>
    // ----------------------------------------------------------------------------------
    public static void deleteUsersGlobalObjects ( EvUserProfileBase ServiceUserProfile )
    {
      Global.LogMethod ( "deleteUsersGlobalObjectEntries method" );
       Global.LogDebugValue ( "UserName: " + ServiceUserProfile.UserId );

      if ( ConfigurationManager.AppSettings [ Evado.Model.UniForm.EuStatics.CONFIG_DELETE_USER_OBJECT_KEY ] != null )
      {
        string value = ConfigurationManager.AppSettings [ Evado.Model.UniForm.EuStatics.CONFIG_DELETE_USER_OBJECT_KEY ];

        if ( value.ToLower ( ) == "no" )
        {
          Global.LogDebugValue ( "Not deleting user profile data" );
          return;
        }

      }

      if ( Global.GlobalObjectList == null )
      {
        return;
      }

      Global.LogDebugValue ( "Deleting global objects for user UserName: " + ServiceUserProfile );
      String userId = ServiceUserProfile.UserId.ToUpper ( );
      //
      // iterate through the list deleting the value.
      //
      IDictionaryEnumerator denum = Global.GlobalObjectList.GetEnumerator ( );
      DictionaryEntry dentry;

      while ( denum.MoveNext ( ) )
      {
        dentry = ( DictionaryEntry ) denum.Current;

        string key = dentry.Key.ToString() ;

        if ( key.Contains( userId ) == true )
        {
        dentry.Value = null;
        }
      }

      Global.LogMethodEnd ( "deleteUsersGlobalObjectEntries" );
    }//END deleteUsersGlobalObjects static method


    // ==================================================================================
    /// <summary>
    /// This method returns the Uri for the selected serviceId
    /// </summary>
    /// <param name="ServiceId">String: Service Id</param>
    /// <returns>String containing the UnIFORM Uri</returns>
    // ----------------------------------------------------------------------------------
    public static String getServiceUri ( String ServiceId )
    {
      foreach ( EvOption option in Global.ServiceUrls )
      {
        if ( option.Description == ServiceId.ToLower ( ) )
        {
          return option.Value;
        }
      }
      return String.Empty;
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Application event handling

    void Application_Error ( object sender, EventArgs e )
    {
      Global.LogMethod ( "Application_Error event method" );
      Exception exc = Server.GetLastError ( );

      if ( exc is HttpUnhandledException )
      {
        //
        // Log the exception event.
        //
        Global.LogEvent ( exc );

        EventLog.WriteEntry ( Global.EventLogSource, EvStatics.getException ( exc ) );

        EvStatics.Files.saveFile ( TempPath, "Exception_" + DateTime.Now.ToString ( "yyMMdd-HHmmss" ), EvStatics.getException ( exc ) );

        //
        // Save the logs to disc
        //
        Global.OutputEventLog ( );
        Global.OutputApplicationLog ( );
      }
    }

    #endregion

    #region Application End section

    //  =======================================================================================
    /// <summary>
    /// Description:
    ///  This method is trigerred when the application is ending.
    /// 
    /// </summary>
    /// <param name="sender">Event object</param>
    /// <param name="e">Event arguments</param>
    //  ---------------------------------------------------------------------------------------
    protected void Application_End ( Object sender, EventArgs e )
    {
      try
      {
        string sEvent = "EVADO Application End";

        // 
        // Open the stream to the file.
        // 
        using ( StreamWriter sw = new StreamWriter ( Global.ApplicationPath + RegisteredDevicesFile ) )
        {
          sw.Write ( RegisteredDevices );

        }// End StreamWriter.


        sEvent += "\r\n Deleting " + Application.Contents.Count + " application variables. ";
        // 
        // Write an event entry when the application ends.
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
        EventLog.WriteEntry ( EventLogSource, sEvent, EventLogEntryType.Information );
      }
      catch ( Exception Ex )
      {
        EventLog.WriteEntry ( EventLogSource, Evado.Model.EvStatics.getException ( Ex ), EventLogEntryType.Error );
      } // Close catch   
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Session Start step

    protected void Session_Start ( Object sender, EventArgs e )
    {

    }// Close Session_Start Event method.

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Static Application log methods

    //
    // Define the debug lot string builder.
    //
    private static System.Text.StringBuilder _EventLog = new System.Text.StringBuilder ( );
    private static System.Text.StringBuilder _CommandLog = new System.Text.StringBuilder ( );
    private static System.Text.StringBuilder _ApplicationLog = new System.Text.StringBuilder ( );

    private const String CONST_COMMAND_LOG_FILE_NAME = @"command-log-";
    private const String CONST_EVENT_LOG_FILE_NAME = @"event-log-";

    private const String CONST_APPLICATION_LOG_FILE_NAME = @"application-log-";

    private const String CONST_BASE_NAME_SPACE = "Evado.Digital.WebService.";

    private static bool DevelopmentLogging = false;

    //  =================================================================================
    /// <summary>
    ///   This static method removes a user from the online user list.
    /// 
    /// </summary>
    //   ---------------------------------------------------------------------------------
    public static void ClearApplicationLog ( )
    {
      Global._ApplicationLog = new StringBuilder ( );
    }

    //  =================================================================================
    /// <summary>
    ///   This static method removes a user from the online user list.
    /// 
    /// </summary>
    //   ---------------------------------------------------------------------------------
    public static void OutputEventLog ( )
    {
      String ServiceLogFileName = Global.LogFilePath
        + APPLICATION_SERVICE_ROOT
        + CONST_EVENT_LOG_FILE_NAME
        + DateTime.Now.ToString ( "yy-MM-dd" ) + ".log";

      if( DevelopmentLogging == true )
      {
        ServiceLogFileName = Global.LogFilePath
         + APPLICATION_SERVICE_ROOT
         + CONST_EVENT_LOG_FILE_NAME
         + ".log";
      }

      ServiceLogFileName = ServiceLogFileName.Replace ( "/", "-" );

      String stContent = Evado.Model.EvStatics.getHtmlAsString ( Global._EventLog.ToString ( ) );

      // 
      // Open the stream to the file.
      // 
      using ( System.IO.StreamWriter sw = new System.IO.StreamWriter ( ServiceLogFileName ) )
      {
        sw.Write ( stContent );

      }// End StreamWriter.

    }//END writeOutDebugLog method}


    //  =================================================================================
    /// <summary>
    ///   This static method removes a user from the online user list.
    /// 
    /// </summary>
    //   ---------------------------------------------------------------------------------
    public static void OutputApplicationLog ( )
    {
      //
      // Define the filename
      //
      String LogFileName = Global.LogFilePath
        + APPLICATION_SERVICE_ROOT
        + CONST_APPLICATION_LOG_FILE_NAME
        + DateTime.Now.ToString ( "yy-MM" ) + ".log";

      if ( DevelopmentLogging == true )
      {
        LogFileName = Global.LogFilePath
         + APPLICATION_SERVICE_ROOT
         + CONST_APPLICATION_LOG_FILE_NAME
         + ".log";
      }

      LogFileName = LogFileName.Replace ( "/", "-" );


      //
      // IF Debug is turned off exit method.
      //
      if ( Global.LoggingLevel < 1 )
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

      if ( Global._ApplicationLog.Length == 0 )
      {
        stContent = " APPLICATION LOG\r\n"
          + "Saved: " + DateTime.Now.ToString ( "dd MMM yyyy HH:mm:ss" )
          + "\r\nNo Debug Content";
      }
      else
      {
        stContent += " APPLICATION LOG\r\n"
          + "Saved: " + DateTime.Now.ToString ( "dd MMM yyyy HH:mm:ss" )
          + "\r\n"
          + Global._ApplicationLog.ToString ( );
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

    //  =================================================================================
    /// <summary>
    ///   This static method removes a user from the online user list.
    /// 
    /// </summary>
    //   ---------------------------------------------------------------------------------
    public static void OutputApplicationLog_Save ( )
    {
      //
      // Define the filename
      //
      String LogFileName = Global.LogFilePath
        + APPLICATION_SERVICE_ROOT
        + CONST_APPLICATION_LOG_FILE_NAME
        + DateTime.Now.ToString ( "yy-MM" ) 
        + "-SAVE.log";

      if ( DevelopmentLogging == true )
      {
        LogFileName = Global.LogFilePath
         + APPLICATION_SERVICE_ROOT
         + CONST_APPLICATION_LOG_FILE_NAME
         +  "-SAVE.log";
      }

      LogFileName = LogFileName.Replace ( "/", "-" );


      //
      // IF Debug is turned off exit method.
      //
      if ( Global.LoggingLevel < 1 )
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

      if ( Global._ApplicationLog.Length == 0 )
      {
        stContent = " APPLICATION LOG\r\n"
          + "Saved: " + DateTime.Now.ToString ( "dd MMM yyyy HH:mm:ss" )
          + "\r\nNo Debug Content";
      }
      else
      {
        stContent += " APPLICATION LOG\r\n"
          + "Saved: " + DateTime.Now.ToString ( "dd MMM yyyy HH:mm:ss" )
          + "\r\n"
          + Global._ApplicationLog.ToString ( );
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

    //  =================================================================================
    /// <summary>
    ///   This static method removes a user from the online user list.
    /// 
    /// </summary>
    //   ---------------------------------------------------------------------------------
    public static void LogCommand (
      Command PageCommand,
      String SessionId )
    {
      //
      // Initialise the methods variables and objects.
      //
      String CommandLogFileName = Global.LogFilePath
        + APPLICATION_SERVICE_ROOT
        + CONST_COMMAND_LOG_FILE_NAME
        + DateTime.Now.ToString ( "yy-MM" ) 
        + ".log";

      if ( DevelopmentLogging == true )
      {
        CommandLogFileName = Global.LogFilePath
        + APPLICATION_SERVICE_ROOT
        + CONST_COMMAND_LOG_FILE_NAME
        + ".log";
      }

      String stCommand = String.Empty;
      CommandLogFileName = CommandLogFileName.Replace ( "/", "-" );

      //
      // get the file information.
      //
      System.IO.FileInfo fi = new FileInfo ( CommandLogFileName );

      if ( fi.Exists == false )
      {
        stCommand = "\"Date Stamp\""
         + ",\"Type\""
         + ",\"UserId\""
         + ",\"SessionId\""
         + ",\"DeviceId\""
         + ",\"DeviceName\""
         + ",\"OSVersion\""
         + ",\"Url\""
         + ",\"Title\""
         + ",\"AppId\""
         + ",\"Object\""
         + ",\"Method\""
         + ",\"Type\""
         + ",\"Param Count\"";

        using ( System.IO.StreamWriter sw = fi.AppendText ( ) )
        {
          sw.WriteLine ( stCommand );

        }// End StreamWriter.
      }


      //
      // Generate the page groupCommand log record.
      //
      stCommand = "\"" + DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + "\""
        + ",\"Page Command\""
        + ",\"" + PageCommand.GetHeaderValue ( Evado.Model.UniForm.CommandHeaderElements.UserId ) + "\""
        + ",\"" + SessionId + "\""
        + ",\"" + PageCommand.GetHeaderValue ( Evado.Model.UniForm.CommandHeaderElements.DeviceId ) + "\""
        + ",\"" + PageCommand.GetHeaderValue ( Evado.Model.UniForm.CommandHeaderElements.DeviceName ) + "\""
        + ",\"" + PageCommand.GetHeaderValue ( Evado.Model.UniForm.CommandHeaderElements.OSVersion ) + "\""
        + ",\"" + PageCommand.GetHeaderValue ( Evado.Model.UniForm.CommandHeaderElements.Client_Url ) + "\""
        + ",\"" + PageCommand.Title + "\""
        + ",\"" + PageCommand.ApplicationId + "\""
        + ",\"" + PageCommand.Object + "\""
        + ",\"" + PageCommand.Method + "\""
        + ",\"" + PageCommand.Type + "\""
        + ",\"" + PageCommand.Parameters.Count + "\"";


      // 
      // Open the stream to the file.
      // 
      using ( System.IO.StreamWriter sw = fi.AppendText ( ) )
      {
        sw.WriteLine ( stCommand );

      }// End StreamWriter.

    }//END WriteCommandLog method. 

    //  =================================================================================
    /// <summary>
    ///   This static method removes a user from the online user list.
    /// 
    /// </summary>
    //   ---------------------------------------------------------------------------------
    public static void LogExitCommand (
      Command PageCommand,
      String SessionId )
    {
      Global.LogMethod ( "WRITE EXIT COMMAND LOG ENTRY" );

      if ( PageCommand == null )
      {
        return;
      }
      //
      // Initialise the methods variables and objects.
      //
      String CommandLogFileName = Global.LogFilePath
        + APPLICATION_SERVICE_ROOT
        + CONST_COMMAND_LOG_FILE_NAME
        + DateTime.Now.ToString ( "yy-MM" ) + ".log";
      String stCommand = String.Empty;
      CommandLogFileName = CommandLogFileName.Replace ( "/", "-" );

      //
      // get the file information.
      //
      System.IO.FileInfo fi = new FileInfo ( CommandLogFileName );

      if ( fi.Exists == false )
      {
        stCommand = "\"Date Stamp\""
         + ",\"Type\""
         + ",\"UserId\""
         + ",\"SessionId\""
         + ",\"DeviceId\""
         + ",\"DeviceName\""
         + ",\"OSVersion\""
         + ",\"URL\""
         + ",\"Title\""
         + ",\"AppId\""
         + ",\"Object\""
         + ",\"Method\""
         + ",\"Type\""
         + ",\"Param Count\"";

        using ( System.IO.StreamWriter sw = fi.AppendText ( ) )
        {
          sw.WriteLine ( stCommand );

        }// End StreamWriter.
      }


      //
      // Generate the page groupCommand log record.
      //
      stCommand = "\"" + DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + "\""
        + ",\"Exit Command\""
        + ",\"" + PageCommand.GetHeaderValue ( Evado.Model.UniForm.CommandHeaderElements.UserId ) + "\""
        + ",\"" + SessionId + "\""
        + ",\"" + PageCommand.GetHeaderValue ( Evado.Model.UniForm.CommandHeaderElements.DeviceId ) + "\""
        + ",\"" + PageCommand.GetHeaderValue ( Evado.Model.UniForm.CommandHeaderElements.DeviceName ) + "\""
        + ",\"" + PageCommand.GetHeaderValue ( Evado.Model.UniForm.CommandHeaderElements.OSVersion ) + "\""
        + ",\"" + PageCommand.GetHeaderValue ( Evado.Model.UniForm.CommandHeaderElements.Client_Url ) + "\""
        + ",\"" + PageCommand.Title + "\""
        + ",\"" + PageCommand.ApplicationId + "\""
        + ",\"" + PageCommand.Object + "\""
        + ",\"" + PageCommand.Method + "\""
        + ",\"" + PageCommand.Type + "\""
        + ",\"" + PageCommand.Parameters.Count + "\"";


      // 
      // Open the stream to the file.
      // 
      using ( System.IO.StreamWriter sw = fi.AppendText ( ) )
      {
        sw.WriteLine ( stCommand );

      }// End StreamWriter.

    }//END WriteCommandLog method. 

    //  =================================================================================
    /// <summary>
    ///   This static method removes a user from the online user list.
    /// 
    /// </summary>
    //   ---------------------------------------------------------------------------------
    public static void LogEvent ( String Value )
    {
      Global._EventLog.AppendLine ( "\r\n" + DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": EVENT: " + Value  );  
 
      Global._ApplicationLog.AppendLine ( "\r\n" + DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": EVENT: " + Value );
    
    }//ENd LogEvent method


    //  =================================================================================
    /// <summary>
    ///   This static method removes a user from the online user list.
    /// 
    /// </summary>
    //   ---------------------------------------------------------------------------------
    public static void LogEvent ( Exception Value )
    {
      String value = EvStatics.getException ( Value );

      Global.LogEvent( value );

    }//END LogEvent method
    
    //  =================================================================================
    /// <summary>
    ///   This static method removes a user from the online user list.
    /// 
    /// </summary>
    //   ---------------------------------------------------------------------------------
    public static void LogService ( String Value )
    {
      Global._ApplicationLog.AppendLine ( Value );
    }

    //  =================================================================================
    /// <summary>
    ///   This static method removes a user from the online user list.
    /// 
    /// </summary>
    //   ---------------------------------------------------------------------------------
    public static void LogMethod ( String Value )
    {
      //
      // log value if application logging level is exceeded.
      //
      if ( Global.LoggingLevel > 0 )
      {
        Global._ApplicationLog.AppendLine ( Evado.Model.EvStatics.CONST_METHOD_START
          + DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": "
          + CONST_BASE_NAME_SPACE + "ClientService."
          + Value );
      }

    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="MethodName">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    public static void LogMethodEnd ( String MethodName )
    {
      if ( Global.LoggingLevel > 0 )
      {
        String value = Evado.Model.EvStatics.CONST_METHOD_END;

        value = value.Replace ( " END OF METHOD ", " END OF " + MethodName + " METHOD " );

        Global._ApplicationLog.AppendLine ( value );
      }
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    private static void LogValue ( String Value )
    {
      if ( Global.LoggingLevel > 1 )
      {
        Global._EventLog.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " + Value );
      }
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    private static void LogValue2 ( String Value )
    {
      if ( Global.LoggingLevel > 2 )
      {
        Global._EventLog.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " + Value );
      }
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    private static void LogDebugValue ( String Value )
    {
      if ( Global.LoggingLevel > 4 )
      {
        Global._EventLog.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " + Value );
      }
    }

    #endregion
  }
}
