/***************************************************************************************
 * <copyright file=Evado.IntegrationClient\Program.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2020 EVADO HOLDING PTY. LTD..  All rights reserved.
 *     
 *      The use and distribution terms for this software are contained in the file
 *      named license.txt, which can be found in the root of this distribution.
 *      By using this software in any fashion, you are agreeing to be bound by the
 *      terms of this license.
 *     
 *      You must not remove this notice, or any other, from this software.
 *     
 * </copyright>
 * 
 * Description: 
 *  This class contains the EvCaseReportForms business object.
 *
 ****************************************************************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.IO;
using System.Net;


// Evado specific references
using Evado.Model;
using Evado.Model.Integration;

namespace Evado.IntegrationClient
{
  /// <summary>
  /// This class is the program's main integration client executable class object.
  /// </summary>
  class Program
  {
    #region initialise variables and objects.
    /// <summary>
    /// This string contains the service root URl. 
    /// </summary>
    private static string WebServiceUrl = string.Empty;

    /// <summary>
    /// This string contains the relative service url. 
    /// </summary>
    private static string RelativeWcfRestURL = "int/service/";

    /// <summary>
    /// This string contains the relative binary download url. 
    /// </summary>
    private static string RelativeBinaryDownloadURL = "images/temp/";

    /// <summary>
    /// This string contains the relative binary upload url. 
    /// </summary>
    private static string RelativeBinaryUploadURL = "images/defalut.aspx";

    private static string ClientVersion = "2_0";
    private static String _Server_SessionId = "SessionId";
    private static String _UserNetworkId = String.Empty;

    //public static Evado.Bll.Integration.EiServices _IntegrationServices = new EiServices ( );
    private static String CustomerId = String.Empty;
    private static String ProjectId = String.Empty;
    private static String OrgId = String.Empty;
    private static String FormId = String.Empty;
    private static String SubjectId = String.Empty;
    private static String VisitId = String.Empty;
    private static String MilestoneId = String.Empty;
    private static String ActivityId = String.Empty;

    private static String InputFileName = String.Empty;
    private static String XmlFileName = String.Empty;
    private static String CsvFileName = String.Empty;
    private static String LogFileName = String.Empty;
    private static System.Text.StringBuilder _ProcessLog = new StringBuilder ( );

    private static String FilePath = String.Empty;
    private const string Static_DefaultEventSource = "Application";
    private static EiQueryTypes QueryType = EiQueryTypes.Null;
    private static bool DebugOn = false;
    private static bool XmlResultFile = false;
    private static EiData _ImportData = new EiData ( );
    private static EiData _ExportData = new EiData ( );
    private static CookieContainer _CookieContainer = new CookieContainer ( );

    /// <summary>
    /// this field defines the Event Log Source
    /// </summary>
    public static string EventLogSource = Static_DefaultEventSource;

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    //==================================================================================
    /// <summary>
    /// This is the main program static method.
    /// </summary>
    /// <param name="args">array of string parameters.</param>
    //-----------------------------------------------------------------------------------
    static void Main ( string [ ] args )
    {
      try
      {
        Program.writeDebugLog ( "STARTED: Evado Integration Test." );

        //
        // get the application configuration values.
        //
        Program.getAppConfigValues ( );

        //
        // get the parameter values.
        //
        if ( Program.getParameterValues ( args ) == false )
        {
          return;
        }

        //
        // Define the input file 
        //
        if ( Program.readInputFile ( ) == false )
        {
          Program.getHelp ( );

          return;
        }

        //
        // if the customer id is empty delete it from the parameter list.
        //
        if ( Program.CustomerId == String.Empty )
        {
          Program._ImportData.DeleteQueryParameter ( EiQueryParameterNames.Customer_Id );
        }

        //
        // No parameters are provided then display help.
        //
        if ( Program.ProjectId == String.Empty )
        {
          Program.getHelp ( );

          return;
        }

        //
        // Send the query to the web service.
        switch ( Program.QueryType )
        {
          case EiQueryTypes.Common_Records_Import:
          case EiQueryTypes.Visit_Records_Import:
          case EiQueryTypes.Subjects_Import:
          case EiQueryTypes.Activities_Import:
            {
              Program.importQuery ( );
              break;
            }
          default:
            {
              Program.exportQuery ( );
              break;
            }

        }

        Program.writeDebugLog ( "FINISHED: Evado Integration Test." );
        // wait for the user to press enter
        //
        // System.Console.Read();


      }
      catch ( Exception Ex )
      {
        System.Console.Write ( EvStatics.getException ( Ex ) );
      }
    }//END Main method

    //  =================================================================================
    /// <summary>
    /// This method retrieves the application config values
    /// </summary>
    //  ---------------------------------------------------------------------------------
    private static void getAppConfigValues ( )
    {
      Program.writeDebugLogMethod ( "getAppConfigValues method started." );
      Program.writeProcessLog ( "Read in configuration parameters:" );

      //
      // Set the SMTP Server string.
      //
      if ( ConfigurationManager.AppSettings [ "FilePath" ] != null )
      {
        Program.FilePath = (string) ConfigurationManager.AppSettings [ "FilePath" ];

        Program.FilePath += @"\";
        Program.FilePath = Program.FilePath.Replace ( @"\\", @"\" ); 
      }

      Program.writeProcessLog ( "- FilePath: " + Program.FilePath );

      // 
      // Set the web service URl
      // 
      if ( ConfigurationManager.AppSettings [ "WebServiceUrl" ] != null )
      {
        Program.WebServiceUrl = (String) ConfigurationManager.AppSettings [ "WebServiceUrl" ].Trim ( );
      }

      Program.writeProcessLog ( "- WebServiceUrl: " + Program.WebServiceUrl );

      //
      // Set teh application log path
      //
      if ( ConfigurationManager.AppSettings [ "RelativeWcfRestURL" ] != null )
      {
        Program.RelativeWcfRestURL = ConfigurationManager.AppSettings [ "RelativeWcfRestURL" ];
      }
      Program.writeDebugLog ( "RelativeWcfRestURL: " + Program.RelativeBinaryDownloadURL );

      Program.RelativeBinaryDownloadURL = Program.concatinateHttpUrl ( Program.WebServiceUrl, Program.RelativeBinaryDownloadURL );


      Program.writeDebugLog ( "Formatted RelativeWcfRestURL: " + Program.RelativeWcfRestURL );

      //
      // Set teh application log path
      //
      if ( ConfigurationManager.AppSettings [ "RelativeBinaryDownloadURL" ] != null )
      {
        Program.RelativeBinaryDownloadURL = ConfigurationManager.AppSettings [ "RelativeBinaryDownloadURL" ].Trim ( );
      }

      Program.writeDebugLog ( "RelativeBinaryDownloadURL: " + Program.RelativeBinaryDownloadURL );

      Program.RelativeBinaryDownloadURL = Program.concatinateHttpUrl ( Program.WebServiceUrl, Program.RelativeBinaryDownloadURL );

      Program.writeDebugLog ( "Formatted RelativeBinaryDownloadURL: " + Program.RelativeBinaryDownloadURL );

      // 
      // Set the binary file url
      // 
      if ( ConfigurationManager.AppSettings [ "RelativeBinaryUploadURL" ] != null )
      {
        Program.RelativeBinaryUploadURL = ConfigurationManager.AppSettings [ "RelativeBinaryUploadURL" ].Trim ( );
      }

      Program.writeDebugLog ( "RelativeBinaryUploadURL: " + Program.RelativeBinaryUploadURL );

      Program.RelativeBinaryUploadURL = Program.concatinateHttpUrl ( Program.WebServiceUrl, Program.RelativeBinaryUploadURL );

      Program.writeDebugLog ( "Formatted RelativeBinaryUploadUR2: " + Program.RelativeBinaryUploadURL );


      //
      // Set the Set the ProjectId 
      //
      if ( ConfigurationManager.AppSettings [ "ProjectId" ] != null )
      {
        Program.ProjectId = (string) ConfigurationManager.AppSettings [ "ProjectId" ];
      }

      Program.writeProcessLog ( "- ProjectId: " + Program.ProjectId );

      //
      // Set the Set the ProjectId 
      //
      string queryType = String.Empty;
      try
      {
        if ( ConfigurationManager.AppSettings [ "QueryType" ] != null )
        {
          queryType = (string) ConfigurationManager.AppSettings [ "QueryType" ];

          Program.QueryType = Evado.Model.EvStatics.Enumerations.parseEnumValue<EiQueryTypes> ( queryType );
        }
      }
      catch
      {
        Program.writeProcessLog ( "QueryType: " + queryType + " is not a valid query type." );
      }

      Program.writeProcessLog ( "- QueryType: " + Program.QueryType );

      //
      // Set the Set the ProjectId 
      //
      if ( ConfigurationManager.AppSettings [ "OrgId" ] != null )
      {
        Program.OrgId = (string) ConfigurationManager.AppSettings [ "OrgId" ];
      }

      Program.writeProcessLog ( "- OrgId: " + Program.OrgId );

      //
      // Set the Set the ProjectId 
      //
      if ( ConfigurationManager.AppSettings [ "FormId" ] != null )
      {
        Program.FormId = (string) ConfigurationManager.AppSettings [ "FormId" ];
      }

      Program.writeProcessLog ( "- FormId: " + Program.FormId );

      //
      // Set the Set the ProjectId 
      //
      if ( ConfigurationManager.AppSettings [ "SubjectId" ] != null )
      {
        Program.SubjectId = (string) ConfigurationManager.AppSettings [ "SubjectId" ];
      }

      Program.writeProcessLog ( "- SubjectId: " + Program.SubjectId );

      //
      // Set the Set the ProjectId 
      //
      if ( ConfigurationManager.AppSettings [ "ImportFileName" ] != null )
      {
        Program.InputFileName = (string) ConfigurationManager.AppSettings [ "ImportFileName" ];
      }

      Program.writeProcessLog ( "- ImportFileName: " + Program.InputFileName );

      //
      // Set the Set the UserId 
      //
      Program.DebugOn = false;
      if ( ConfigurationManager.AppSettings [ "DeugOn" ] != null )
      {
        String sDebugOn = (string) ConfigurationManager.AppSettings [ "DeugOn" ];
        if ( sDebugOn.ToLower ( ) == "yes"
          || sDebugOn.ToLower ( ) == "true" )
        {
          Program.DebugOn = true;
        }
      }

      //
      // Set the Set the UserId 
      //
      Program.XmlResultFile = false;
      if ( ConfigurationManager.AppSettings [ "XmlResultFile" ] != null )
      {
        String sDebugOn = (string) ConfigurationManager.AppSettings [ "XmlResultFile" ];
        if ( sDebugOn.ToLower ( ) == "yes"
          || sDebugOn.ToLower ( ) == "true" )
        {
          Program.XmlResultFile = true;
        }
      }

      Program.writeProcessLog ( "- DebugOn: " + Program.DebugOn );

      Program.writeDebugLog ( "getAppConfigValues method finished." );

    }//END getAppConfigValues Method

    // ==================================================================================
    /// <summary>
    /// This static method concatinates a relative and root URl depending upon
    /// whether the relative URl contains a root domain name.
    /// </summary>
    /// <param name="RootUrl">String: root url </param>
    /// <param name="RelativeUrl">String relative url</param>
    /// <returns>String: contatinated url.</returns>
    // -----------------------------------------------------------------------------------
    public static String concatinateHttpUrl (
      String RootUrl,
      String RelativeUrl )
    {
      if ( RelativeUrl.Contains ( "http://" ) == false
        && RelativeUrl.Contains ( "https://" ) == false )
      {
        RelativeUrl = RootUrl + RelativeUrl;
      }

      return RelativeUrl;
    }

    //  =================================================================================
    /// <summary>
    /// This method retrieves the application config values
    /// </summary>
    //  ---------------------------------------------------------------------------------
    private static bool getParameterValues (
      string [ ] args )
    {
      Program.writeDebugLogMethod ( "getParameterValues method started." );
      Program.writeProcessLog ( "Read in the command line parameters: " );

      //
      // Extract the parameter list.
      //
      if ( args.Length == 0 )
      {
        Program.writeProcessLog ( "- No Arguments provided" );

        return true;
      }

      //
      // Iterate through the parameter list.
      //
      for ( int i = 0; i < args.Length; i++ )
      {
        string stArgument = args [ i ].Replace ( "/", String.Empty );


        if ( stArgument.Contains ( "HLP" ) == true
          || stArgument.Contains ( "HELP" ) == true )
        {
          Program.getHelp ( );

          return false;
        }

        //
        // Extract the trial's identifier name
        //
        if ( stArgument.Contains ( "QT=" ) == true )
        {
          stArgument = stArgument.Replace ( "QT=", String.Empty );
          EiQueryTypes queryType = EiQueryTypes.Null;

          if ( Evado.Model.EvStatics.Enumerations.tryParseEnumValue<EiQueryTypes> ( stArgument, out queryType ) == true )
          {
            Program.QueryType = queryType;
          }

          Program.writeProcessLog ( "- QueryType : " + Program.QueryType );
        }

        //
        // Extract the project's identifier name
        //
        if ( stArgument.Contains ( "PID=" ) == true )
        {
          stArgument = stArgument.Replace ( "PID=", String.Empty );

          Program.ProjectId = stArgument;
          Program.writeProcessLog ( "- ProjectId : " + Program.ProjectId );
        }

        //
        // Extract the organisation identifier name
        //
        if ( stArgument.Contains ( "OID=" ) == true )
        {
          stArgument = stArgument.Replace ( "OID=", String.Empty );

          Program.OrgId = stArgument;
          Program.writeProcessLog ( "- OrgId : " + Program.OrgId );
        }

        //
        // Extract the subject's identifier name
        //
        if ( stArgument.Contains ( "SID=" ) == true )
        {
          stArgument = stArgument.Replace ( "SID=", String.Empty );

          Program.SubjectId = stArgument;
          Program.writeProcessLog ( "- SubjectId : " + Program.SubjectId );
        }

        //
        // Extract the form's identifier name
        //
        if ( stArgument.Contains ( "FID=" ) == true )
        {
          stArgument = stArgument.Replace ( "FID=", String.Empty );

          Program.FormId = stArgument;
          Program.writeProcessLog ( "- Form : " + Program.FormId );
        }

        //
        // Extract the subject's identifier name
        //
        if ( stArgument.Contains ( "AID=" ) == true )
        {
          stArgument = stArgument.Replace ( "AID=", String.Empty );

          Program.ActivityId = stArgument;
          Program.writeProcessLog ( "- Activity : " + Program.ActivityId );
        }

        //
        // Extract the subject's identifier name
        //
        if ( stArgument.Contains ( "MID=" ) == true )
        {
          stArgument = stArgument.Replace ( "MID=", String.Empty );

          Program.MilestoneId = stArgument;
          Program.writeProcessLog ( "- Milestone : " + Program.MilestoneId );
        }

        //
        // Extract the subject's identifier name
        //
        if ( stArgument.Contains ( "VID=" ) == true )
        {
          stArgument = stArgument.Replace ( "VID=", String.Empty );

          Program.VisitId = stArgument;
          Program.writeProcessLog ( "- VisitId : " + Program.VisitId );
        }

        //
        // Extract the file path name
        //
        if ( stArgument.Contains ( "FP=" ) == true )
        {
          stArgument = stArgument.Replace ( "FP=", String.Empty );

          Program.FilePath = stArgument;

          Program.FilePath += @"\";
          Program.FilePath = Program.FilePath.Replace ( @"\\", @"\" ); 

          Program.writeProcessLog ( "- File Path : " + Program.FilePath );
        }

        //
        // Extract the trial's identifier name
        //
        if ( stArgument.Contains ( "IFN=" ) == true )
        {
          stArgument = stArgument.Replace ( "IFN=", String.Empty );

          Program.InputFileName = stArgument;

          Program.writeProcessLog ( "- Input file name : " + Program.InputFileName );
        }

      }//END Argument interation loop.

      return true;

    }//END getParameterValues method

    //  =================================================================================
    /// <summary>
    /// This method retrieves the application config values
    /// </summary>
    //  ---------------------------------------------------------------------------------
    private static void getHelp ( )
    {
      Program.writeDebugLogMethod ( "getHelp method started." );

      Console.WriteLine ( "Evado Integration command line Client help. " );

      //
      // app.config setting parameters.
      //
      Console.WriteLine ( "\r\nApp.Config setting values are: " );

      Console.WriteLine ( "- 'QueryType' defines the integration query type to be executed." );

      Console.WriteLine ( "- 'FilePath' defines the data file path. " );

      Console.WriteLine ( "- 'ImportFileName' defines the import data file name. " );

      Console.WriteLine ( "- 'ProjectId' defines the Evado project identifier the queries are associated with. " );

      Console.WriteLine ( "- 'OrgId' defines the Evado project organisation identifier the queries are associated with." );

      Console.WriteLine ( "- 'FormId' defines the Evado project form identifier the queries are associated with." );

      Console.WriteLine ( "- 'SubjectId' defines the Evado project subject identifier the queries are associated with." );

      Console.WriteLine ( "- 'WebServiceUrl' is the URL to the web service." );

      //
      // Command line parameters.
      //
      Console.WriteLine ( "\r\nCommand line arguements overrides the App.Config values." );
      Console.WriteLine ( "Command line arguements can be: " );

      Console.WriteLine ( "- 'QT=[value] ' defines the query type. " );

      Console.WriteLine ( "- 'PID=[value] ' defines the project the query is associated with.  " );

      Console.WriteLine ( "- 'OID=[value] ' defines the organsiation the query is associated with. " );

      Console.WriteLine ( "- 'SID=[value] ' defines the subject the query is associated with. " );

      Console.WriteLine ( "- 'FID=[value] ' defines the form the query is associated with. " );

      Console.WriteLine ( "- 'VID=[value] ' defines the visit the query is associated with. " );

      Console.WriteLine ( "- 'MID=[value] ' defines the milestone the query is associated with. " );

      Console.WriteLine ( "- 'FP=[value] ' defines the file path for import and export data. " );

      Console.WriteLine ( "- 'IFN=[value] ' defines the CSV filename contains import data. " );

      //
      // query type parameters.
      //
      Console.WriteLine ( "\r\nValid Query Types are: " );

      Console.WriteLine ( "- 'Export_Schedule' export the project sechedule. " );

      Console.WriteLine ( "- 'Export_Scheduled_Forms' export the project project forms. " );

      Console.WriteLine ( "- 'Export_Common_Forms' export the project common forms. " );

      //Console.WriteLine( "- 'Export_Patients' export the patients. "); 

      Console.WriteLine ( "- 'Export_Subjects' export the project subject demographics. " );

      Console.WriteLine ( "- 'Export_Subject_Visits' export the project subject visit data. " );

      Console.WriteLine ( "- 'Export_Adverse_Events' export the project subject adverse event data. " );

      Console.WriteLine ( "- 'Export_Comcomitant_Medications' export the project concomitant medication data. " );

      Console.WriteLine ( "- 'Export_Serious_Adverse_Events' export the project serious adverse event data. " );

      Console.WriteLine ( "- 'Export_Project_Records' export the project project records. " );

      //Console.WriteLine( "- 'Project_Report' export the a report's data. "); 

      //Console.WriteLine( "- 'Short_Tail_Statisticial_Export' export the project sechedule. "); 

      //Console.WriteLine( "- 'Long_Tail_Statistical_Export' export the project sechedule. "); 

      Console.WriteLine ( "- 'Patient_Template' generate  the project patient data template. " );

      Console.WriteLine ( "- 'Schedule_Template' generate the project sechedule data template. " );

      Console.WriteLine ( "- 'Subject_Template' generate the project sechedule data template. " );

      Console.WriteLine ( "- 'ProjectRecord_Template' generate the project sechedule data template. " );

      Console.WriteLine ( "- 'CommonRecord_Template' generate the project sechedule data template. " );

      Console.WriteLine ( "- 'Import_Schedule' import the project schedule data. " );

      //Console.WriteLine( "- 'Import_Patient' import the patient data. "); 

      Console.WriteLine ( "- 'Import_Subjects' import the project subject data. " );

      Console.WriteLine ( "- 'Import_Project_Records' import the project project record data. " );

      Console.WriteLine ( "- 'Import_Common_Records' import the project common record dtaa. " );

    }//END getParameterValues method

    //  =================================================================================
    /// <summary>
    /// This method sets the imput export filename.
    /// </summary>
    //  ---------------------------------------------------------------------------------
    private static bool readInputFile ( )
    {
      Program.writeDebugLogMethod ( "setFileNames method started." );
      Program.writeDebugLog ( "QueryType: " + Program.QueryType );
      Program.writeDebugLog ( "InputFileName: " + Program.InputFileName );

      //
      // Initialise the methods variables and objects.
      //
      String value = String.Empty;
      string filename = Program.FilePath + Program.ProjectId + @"\";
      filename = filename.ToLower ( );
      Program.writeDebugLog ( "filename: " + filename );
      //
      // Initialise the query data object.
      //
      Program._ImportData = new EiData ( );
      Program._ImportData.QueryType = Program.QueryType;
      Program._ImportData.AddQueryParameter ( EiQueryParameterNames.Customer_Id, Program.CustomerId );
      Program._ImportData.AddQueryParameter ( EiQueryParameterNames.Project_Id, Program.ProjectId );

      //
      // read inpit file if it has been defined into the query data object.
      //
      if ( Program.InputFileName != String.Empty )
      {
        Program._ImportData = Program.GetDataObjectFromCsv ( filename, Program.InputFileName );

        //
        // Update project values from input file.
        //
        value = Program._ImportData.GetQueryParameterValue ( EiQueryParameterNames.Customer_Id );
        if ( value != String.Empty )
        {
          Program.CustomerId = value;
        }

        //
        // Update project values from input file.
        //
        value = Program._ImportData.GetQueryParameterValue ( EiQueryParameterNames.Project_Id );
        if ( value != String.Empty )
        {
          Program.ProjectId = value;
        }

        //
        // Update organisation values from input file.
        //
        value = Program._ImportData.GetQueryParameterValue ( EiQueryParameterNames.Organisation_Id );
        if ( value != String.Empty )
        {
          Program.OrgId = value;
        }

        //
        // Update subject values from input file.
        //
        value = Program._ImportData.GetQueryParameterValue ( EiQueryParameterNames.Subject_Id );
        if ( value != String.Empty )
        {
          Program.SubjectId = value;
        }

        //
        // Update form values from input file.
        //
        value = Program._ImportData.GetQueryParameterValue ( EiQueryParameterNames.Form_Id );
        if ( value != String.Empty )
        {
          Program.FormId = value;
        }

        //
        // Update activity values from input file.
        //
        value = Program._ImportData.GetQueryParameterValue ( EiQueryParameterNames.Activity_Id );
        if ( value != String.Empty )
        {
          Program.ActivityId = value;
        }

        //
        // Update milestone values from input file.
        //
        value = Program._ImportData.GetQueryParameterValue ( EiQueryParameterNames.Milestone_Id );
        if ( value != String.Empty )
        {
          Program.MilestoneId = value;
        }

        //
        // Update milestone values from input file.
        //
        value = Program._ImportData.GetQueryParameterValue ( EiQueryParameterNames.Visit_Id );
        if ( value != String.Empty )
        {
          Program.VisitId = value;
        }
      }//END update query data from input file
      else
      {
        if ( Program.ProjectId == String.Empty )
        {
          return false;
        }

        if ( Program.OrgId != String.Empty )
        {
          Program._ImportData.AddQueryParameter ( EiQueryParameterNames.Organisation_Id, Program.OrgId );
        }
        if ( Program.SubjectId != String.Empty )
        {
          Program._ImportData.AddQueryParameter ( EiQueryParameterNames.Subject_Id, Program.SubjectId );
        }
        if ( Program.FormId != String.Empty )
        {
          Program._ImportData.AddQueryParameter ( EiQueryParameterNames.Form_Id, Program.FormId );
        }
        if ( Program.MilestoneId != String.Empty )
        {
          Program._ImportData.AddQueryParameter ( EiQueryParameterNames.Milestone_Id, Program.MilestoneId );
        }
        if ( Program.VisitId != String.Empty )
        {
          Program._ImportData.AddQueryParameter ( EiQueryParameterNames.Visit_Id, Program.VisitId );
        }
      }

      switch ( Program.QueryType )
      {
        case EiQueryTypes.Visit_Record_Template:
          {
            Program.CsvFileName = Program.ProjectId + "-" + Program.FormId + "-" + Program.QueryType + ".csv";
            Program.CsvFileName = Program.CsvFileName.ToLower ( );
            Program.LogFileName = Program.CsvFileName.Replace ( ".csv", "-log.txt" );
            break;
          }

        case EiQueryTypes.Common_Record_Template:
          {
            Program.CsvFileName = Program.ProjectId + "-" + Program.FormId + "-" + Program.QueryType + ".csv";
            Program.CsvFileName = Program.CsvFileName.ToLower ( );
            Program.LogFileName = Program.CsvFileName.Replace ( "-data.csv", "-log.txt" );
            break;
          }

        case EiQueryTypes.Visit_Records_Import:
        case EiQueryTypes.Common_Records_Import:
        case EiQueryTypes.Visit_Records_Export:
        case EiQueryTypes.Adverse_Events_Export:
        case EiQueryTypes.Serious_Adverse_Events_Export:
        case EiQueryTypes.Comcomitant_Medications_Export:
          {
            Program.CsvFileName = Program.ProjectId;

            if ( Program.FormId != String.Empty )
            {
              Program.CsvFileName += "-" + Program.FormId;
            }

            if ( Program.OrgId != String.Empty )
            {
              Program.CsvFileName += "-" + Program.OrgId;
            }

            if ( Program.SubjectId != String.Empty )
            {
              Program.CsvFileName += "-" + Program.SubjectId;
            }

            Program.CsvFileName += "-" + Program.QueryType + "-data.csv";
            Program.CsvFileName = Program.CsvFileName.ToLower ( );
            Program.LogFileName = Program.CsvFileName.Replace ( "-data.csv", "-log.txt" );
            break;
          }
        default:
          {

            Program.CsvFileName = Program.ProjectId + "-" + Program.QueryType + "-data.csv";
            Program.CsvFileName = Program.CsvFileName.ToLower ( );
            Program.LogFileName = Program.CsvFileName.Replace ( "-data.csv", "-log.txt" );
            break;
          }
      }

      Program.writeDebugLog ( "FilePath: " + Program.FilePath );
      Program.writeDebugLog ( "LogFileName: " + LogFileName );
      Program.writeDebugLog ( "CsvFileName: " + Program.CsvFileName );

      return true;
    }

    #region Execute query method.

    //  =================================================================================
    /// <summary>
    /// This project sends a query to the integration service.
    /// </summary>
    //  ---------------------------------------------------------------------------------
    private static void exportQuery ( )
    {
      Program.writeDebugLogMethod ( "exportQuery method started." );
      Program.writeDebugLog ( "QueryType: " + Program.QueryType );
      Program.writeDebugLog ( "ImportFileName: " + Program.InputFileName );
      Program.writeProcessLog ( "Executing a data export query." );
      Program.writeDebugLog ( "CsvFileName: " + Program.CsvFileName );

      //
      // Initialise the methods variables and objects.
      //
      Program.writeDebugLog ( "CSV queryData:" + Program._ImportData.getCsvOutput ( ) );
      string filename = Program.FilePath + Program.ProjectId + @"\";
      filename = filename.ToLower ( );
      Program.writeDebugLog ( "filename: " + filename );

      if ( Program._ImportData.QueryType == EiQueryTypes.Activities_Export )
      {
        Program._ImportData.DeleteQueryParameter ( EiQueryParameterNames.Form_Id );
        Program._ImportData.DeleteQueryParameter ( EiQueryParameterNames.Subject_Id );
        Program._ImportData.DeleteQueryParameter ( EiQueryParameterNames.Sponsor_Id );
        Program._ImportData.DeleteQueryParameter ( EiQueryParameterNames.Sponsor_Id );
        Program._ImportData.DeleteQueryParameter ( EiQueryParameterNames.External_Id );
        Program._ImportData.DeleteQueryParameter ( EiQueryParameterNames.Randomised_Id );
        Program._ImportData.DeleteQueryParameter ( EiQueryParameterNames.Record_Id );
      }

      //
      // send the query to the REST service.
      //
      Program._ExportData = Program.sendQuery ( Program._ImportData );

      if ( Program._ExportData == null )
      {
        Program._ExportData = new EiData ( );
        Program._ExportData.EventCode = EiEventCodes.WebServices_General_Failure_Error;
      }

      //
      // Get the process log.
      //
      Program._ProcessLog.Append ( Program._ExportData.ProcessLog );

      Program.writeDebugLog ( "CSV Result: " + Program._ExportData.getCsvOutput ( ) );

      //
      // Save the CSV file data.
      //
      Evado.Model.EvStatics.Files.saveFile ( filename, Program.CsvFileName, Program._ExportData.getCsvOutput ( ) );

      Program._ProcessLog.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy HH:mm:ss" )
         + ": " + "Saving results data to a CSV file." );

      //
      // Save the XML data file.
      //
      if ( Program.XmlResultFile == true )
      {
        String result = Evado.Model.EvStatics.SerialiseObject<EiData> ( Program._ExportData );

        Evado.Model.EvStatics.Files.saveFile ( filename, Program.XmlFileName, result );


        Program._ProcessLog.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy HH:mm:ss" )
             + ": " + "Saving results data to a XML file." );
      }

      //
      // Save the process log file data.
      //
      Evado.Model.EvStatics.Files.saveFile ( filename, Program.LogFileName, Program._ProcessLog.ToString ( ) );

      Program.writeDebugLog ( "executeQuery method Finished." );

    }//END executeQuery method

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region import data methods

    //  =================================================================================
    /// <summary>
    /// This project sends a query to the integration service.
    /// </summary>
    //  ---------------------------------------------------------------------------------
    private static void importQuery ( )
    {
      Program.writeDebugLogMethod ( "importQuery method started." );
      Program.writeDebugLog ( "QueryType: " + Program.QueryType );
      Program.writeProcessLog ( "Executing a data import query." );
      //
      // initialise the methods variables and objects.
      //
      Program.CsvFileName = Program.CsvFileName.Replace ( "-data.csv", "-results.csv" );
      Program.XmlFileName = Program.CsvFileName.Replace ( ".csv", ".xml" );

      Program.writeDebugLog ( "Import data:" + Program._ImportData.getCsvOutput ( ) );

      //
      // send the query to the service.
      //
      Program._ExportData = Program.sendQuery ( Program._ImportData );

      if ( Program._ExportData == null )
      {
        Program._ExportData = new EiData ( );
        Program._ExportData.EventCode = EiEventCodes.WebServices_General_Failure_Error;
      }

      //
      // Get the process log.
      //
      Program._ProcessLog.Append ( Program._ExportData.ProcessLog );

      Program.writeDebugLog ( "result: " + Program._ExportData.getAsString ( ) );

      //
      // Save the result to a CSV file.
      //
      Evado.Model.EvStatics.Files.saveFile ( Program.FilePath, Program.CsvFileName, Program._ExportData.getCsvOutput ( ) );

      Program._ProcessLog.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy HH:mm:ss" )
         + ": " + "Saving results data to a CSV file." );

      //
      // Serialise and save the results to an XML file.
      //
      if ( Program.XmlResultFile == true )
      {
        String result = Evado.Model.EvStatics.SerialiseObject<EiData> ( Program._ExportData );

        Evado.Model.EvStatics.Files.saveFile ( Program.FilePath, Program.XmlFileName, result );

        Program._ProcessLog.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy HH:mm:ss" )
         + ": " + "Saving results data to a XML file." );
      }

      //
      // Save the process log file data.
      //
      Evado.Model.EvStatics.Files.saveFile ( Program.FilePath, Program.LogFileName, Program._ProcessLog.ToString ( ) );

      Program.writeDebugLog ( "importQuery method Finished." );

    }//END main method

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region retrieve data from CSV file.

    //===================================================================================
    /// <summary>
    /// This static method converts a CSV datafile into a integration data object.
    /// </summary>
    /// <param name="Csv">String: Csv encoded data object.</param>
    /// <returns>Evado.Model.Integration.EiData object</returns>
    //-----------------------------------------------------------------------------------
    public static EiData GetDataObjectFromCsv (
        String FileDirectory,
        String FileName )
    {
      Program.writeDebugLogMethod ( "GetDataObjectFromCsv method started." );
      Program.writeDebugLog ( "FileDirectory: " + FileDirectory );
      Program.writeDebugLog ( "FileName: " + FileName );
      Program.writeProcessLog ( "Reading in data object from a file." );

      //
      // Initialise the mathods variables and objects.
      //
      EiData dataObject = new EiData ( );
      List<String> csvRowList = new List<string> ( );
      String stFileContent = String.Empty;
      int dataRowCount = -1;  // -1 indicated that the index is before the first column.
      String line = String.Empty;
      TextReader reader;
      dataObject.Columns = new List<EiColumnParameters> ( );

      if ( FileDirectory == String.Empty )
      {
        dataObject.EventCode = EiEventCodes.File_Directory_Path_Empty;
        return dataObject;
      }
      if ( FileName == String.Empty )
      {
        dataObject.EventCode = EiEventCodes.File_File_Name_Empty;
        return dataObject;
      }

      if ( Evado.Model.EvStatics.Files.hasDirectory ( FileDirectory ) == false )
      {
        dataObject.EventCode = EiEventCodes.File_Directory_Error;
        return dataObject;
      }

      try
      {
        String TempFileName = FileDirectory + FileName;
        // 
        // 
        // Open the text reader with supplied file
        // 
        using ( reader = File.OpenText ( TempFileName ) )
        {
          // 
          // Read the remainder of the file into the outputLog array list
          // 
          while ( ( line = reader.ReadLine ( ) ) != null )
          {
            //Program.writeDebugLog ( "LINE: " + line );

            csvRowList.Add ( line );
          }

        }

      }
      catch ( Exception Ex )
      {
        Program.writeProcessLog ( "Files content read failed." );
        Program.writeDebugLog ( Evado.Model.EvStatics.getException ( Ex ) );

        dataObject.EventCode = EiEventCodes.File_Save_Error;

        return dataObject;
      }


      Program.writeDebugLog ( "CSV File read: data rows: " + csvRowList.Count );

      //
      // Iterate through the csv rows.
      //
      for ( int csvRowCount = 0; csvRowCount < csvRowList.Count; csvRowCount++ )
      {
        Program.writeDebugLog ( "row: " + csvRowCount );

        String csvRow = csvRowList [ csvRowCount ];

        if ( csvRow.Contains ( "\",\"" ) == true )
        {
          csvRow = csvRow.Replace ( "\", \"", "\",\"" );
          csvRow = csvRow.Replace ( "\", \"", "\",\"" );
          csvRow = csvRow.Replace ( "\", \"", "\",\"" );
          csvRow = csvRow.Replace ( "\", \"", "\",\"" );
          csvRow = csvRow.Replace ( "\",\"", "~" );
          csvRow = csvRow.Replace ( "\"", "" );
          csvRow = csvRow.Replace ( ",", "~" );
        }
        else
        {
          csvRow = csvRow.Replace ( ",", "~" );
        }

        Program.writeDebugLog ( "CSV ROW: " + csvRow );

        String [ ] csvRowArray = csvRow.Split ( '~' );

        Program.writeDebugLog ( "CSV data row length: " + csvRowArray.Length );

        //
        // skip columns less than 2 cels.
        //
        if ( csvRowArray.Length < 2 )
        {
          Program.writeDebugLog ( "SKIP ROW: data row less than 2 columns" );

          continue;
        }

        if ( csvRowArray [ 0 ] == String.Empty )
        {
          Program.writeDebugLog ( "SKIP ROW: No data" );

          continue;
        }

        if ( csvRowArray.Length >= 2 )
        {
          if ( csvRowArray [ 0 ] == EiData.CONST_QUERY_TYPE )
          {
            dataObject.QueryType = Evado.Model.EvStatics.Enumerations.parseEnumValue<EiQueryTypes> ( csvRowArray [ 1 ] );

            Program.writeDebugLog ( "QUERY TYPE FOUND: QueryType: " + dataObject.QueryType );

            if ( dataObject.QueryType != Program.QueryType )
            {
              Program.writeProcessLog ( "Query type does not match.  Using file query type." );
            }

            continue;
          }
        }

        // Program.writeDebugLog ( "PROCESSING DATA" );

        //
        // Iterate throught the columns of the row.
        //
        for ( int columnCount = 1; columnCount < csvRowArray.Length; columnCount++ )
        {
          //
          // Because the first column is record identifier the data starts in the second column.
          //
          int dataIndex = columnCount - 1;

          //
          // Add parameter if column 0 has paramter value.
          //
          if ( csvRowArray [ 0 ] == EiData.CONST_PARAMETER )
          {
            if ( columnCount == 1 )
            {
              if ( csvRowArray [ 1 ] == "Name" )
              {
                //Program.writeDebugLog ( "PARAMETER Name" );
                continue;
              }

              //Program.writeDebugLog ( "PARAMETER FOUND: Name: " + csvRowArray [ 1 ] + ", Value: " + csvRowArray [ 2 ] );

              //
              // Add the parameter vale.
              //
              try
              {
                EiQueryParameterNames name = Evado.Model.EvStatics.Enumerations.parseEnumValue<EiQueryParameterNames> ( csvRowArray [ 1 ] );
                dataObject.AddQueryParameter ( name, csvRowArray [ 2 ] );

              }
              catch
              {
                Program.writeDebugLog ( "PARAMETER ERROR ENCOUNTERED." );

                dataObject.EventCode = EiEventCodes.Integration_Import_Parameter_Error;

                return dataObject;
              }

              continue;
            }//END 
          }

          //
          // Add parameter if column 0 has paramter value.
          //
          if ( csvRowArray [ 0 ] == EiData.CONST_COLUMN_FIELD_ID
            || csvRowArray [ 0 ] == EiData.CONST_COLUMN_NAME
            || csvRowArray [ 0 ] == EiData.CONST_COLUMN_DATA_TYPE
            || csvRowArray [ 0 ] == EiData.CONST_COLUMN_INDEX
            || csvRowArray [ 0 ] == EiData.CONST_COLUMN_METADATA )
          {
            //
            // Add the colum object if is missing.
            //
            //Program.writeDebugLog ( "ADD COLUMN PARAMETER OBJECT. " );
            if ( dataObject.Columns.Count <= dataIndex )
            {
              dataObject.Columns.Add ( new EiColumnParameters ( ) );
            }
            //
            // select the column data type to be updated
            //
            switch ( csvRowArray [ 0 ] )
            {
              case EiData.CONST_COLUMN_FIELD_ID:
                {
                  //Program.writeDebugLog ( "COLUMN FIELD ID ROW FOUND: " );
                  //
                  // Add evado field identifier.
                  //
                  dataObject.Columns [ dataIndex ].EvadoFieldId = csvRowArray [ columnCount ];
                  break;
                }
              case EiData.CONST_COLUMN_DATA_TYPE:
                {
                  //Program.writeDebugLog ( "COLUMN DATA TYPE ROW FOUND: " );
                  //
                  // Add column data type
                  //
                  String stDataType = csvRowArray [ columnCount ];
                  try
                  {
                    EiDataTypes datatype = Evado.Model.EvStatics.Enumerations.parseEnumValue<EiDataTypes> ( stDataType );

                    dataObject.Columns [ dataIndex ].DataType = datatype;
                  }
                  catch
                  {
                    //Program.writeDebugLog ( "COLUMN PARAMETER ERROR ENCOUNTERED." );

                    dataObject.EventCode = EiEventCodes.Integration_Import_Column_Data_Error; ;

                    return dataObject;
                  }
                  break;
                }
              case EiData.CONST_COLUMN_NAME:
                {
                  //Program.writeDebugLog ( "COLUMN NAME ROW FOUND: " );
                  //
                  // Add column name
                  //
                  dataObject.Columns [ dataIndex ].Name = csvRowArray [ columnCount ];
                  break;
                }
              case EiData.CONST_COLUMN_INDEX:
                {
                  //Program.writeDebugLog ( "COLUMN INDEX ROW FOUND: " );
                  //
                  // Add column index
                  //
                  dataObject.Columns [ dataIndex ].Index = Evado.Model.EvStatics.getBool ( csvRowArray [ columnCount ] );

                  break;
                }
              case EiData.CONST_COLUMN_METADATA:
                {
                  //Program.writeDebugLog ( "COLUMN METADATA ROW FOUND: " );
                  //
                  // Add column index
                  //
                  dataObject.Columns [ dataIndex ].MetaData = csvRowArray [ columnCount ];
                  break;
                }
            }//END Colum header switch

            continue;
          }//END data column paramater 

          //
          // Process the data row.
          //
          if ( csvRowArray [ 0 ] == EiData.CONST_DATA_ROW )
          {
            //Program.writeDebugLog ( "COLUMN DATA ROW FOUND: " );
            //
            // Add the data row when the first column of a row is encountered.
            //
            if ( dataIndex == 0 )
            {
              // Program.writeDebugLog ( "Add data row object. " );
              dataObject.AddDataRow ( );
              dataRowCount = ( dataObject.DataRows.Count - 1 );

              // Program.writeDebugLog ( "dataRowCount: " + dataRowCount );
            }

            if ( dataRowCount > -1 )
            {
              dataObject.DataRows [ dataRowCount ].Values [ dataIndex ] = csvRowArray [ columnCount ];
            }
          }//END data row.

        }//END column iteration loop.

      }//END Row iteration loop

      //
      // return the data object.
      //
      return dataObject;

    }//END static GetDataObjectFromCsv method

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Web Services methods.

    // ==================================================================================
    /// <summary>
    /// This method send the Command back to the server objects.
    /// </summary>
    // ---------------------------------------------------------------------------------
    private static EiData sendQuery ( EiData QueryData )
    {
      Program.writeDebugLogMethod ( "sendQuery method. " );
      Program.writeDebugLog ( "QueryData: " + QueryData.getAsString ( ) );

      //
      // Display a serialised instance of the object.
      //
      string serialisedText = String.Empty;
      string stWebServiceUrl = Program.WebServiceUrl;
      EiData ResultData = new EiData ( );
      HttpWebRequest request;

      //
      // Create the web service Url
      //
      stWebServiceUrl += Program.RelativeWcfRestURL + "V" + Program.ClientVersion
        + "/" + QueryData.QueryType + "?session=" + Program._Server_SessionId;

      Program.writeDebugLog ( "stWebServiceUrl: " + stWebServiceUrl );

      //
      // serialise the query data prior to sending to the web service.
      //
      Program.writeDebugLog ( "Serialising the querry data object" );

      serialisedText = Newtonsoft.Json.JsonConvert.SerializeObject ( QueryData );

      Program.writeDebugLog ( "Query: " + serialisedText );

      //
      // Initialise the web request.
      //
      Program.writeDebugLog ( "Creating the WebRequest." );


      try
      {
        request = (HttpWebRequest) WebRequest.Create ( stWebServiceUrl );
        request.Method = "POST";
        request.KeepAlive = true;
        request.CookieContainer = Program._CookieContainer;
        request.AuthenticationLevel = System.Net.Security.AuthenticationLevel.None;

        Program.SetBody ( request, serialisedText );

        // 
        // Get the web service response
        //
        Program.writeDebugLog ( "Sending the the WebRequest." );

        HttpWebResponse response = (HttpWebResponse) request.GetResponse ( );

        //
        // Extract the cookie collection from the response.
        //
        Program._CookieContainer.Add ( response.Cookies );

        //
        // Convert teh response in to a content string.
        //
        serialisedText = Program.ConvertResponseToString ( response );

        Program.writeDebugLog ( "JSON Serialised text length: " + serialisedText.Length );

        if ( Program.DebugOn == true )
        {
          // 
          // Open the stream to the file.
          // 
          using ( StreamWriter sw = new StreamWriter ( Program.FilePath + @"json-data.txt" ) )
          {
            sw.Write ( serialisedText );

          }// End StreamWriter.
        }

        Program.writeDebugLog ( "Deserialising JSON to Evado.Model.UniForm.AppData object." );

        ResultData = Newtonsoft.Json.JsonConvert.DeserializeObject<EiData> ( serialisedText );

      }
      catch ( Exception Ex )
      {
        Program.writeDebugLog ( "Web Service Error. " + Evado.Model.EvStatics.getException ( Ex ) ); ;

        System.Console.Write ( Evado.Model.EvStatics.getException ( Ex ) );
        ResultData = new EiData ( );
        ResultData.EventCode = EiEventCodes.WebServices_General_Failure_Error;
        ResultData.ErrorMessage = "The REST service or JSON deserialisation failed.";

      }

      Program.writeDebugLog ( "sendQuery method. FINISHED" );

      return ResultData;

    }//END sendPageCommand method

    // ==================================================================================
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request">HttpWebRequest object</param>
    /// <param name="requestBody">String: text body.</param>
    // ---------------------------------------------------------------------------------
    private static void SetBody (
      HttpWebRequest request,
      String requestBody )
    {
      if ( requestBody.Length > 0 )
      {
        using ( Stream requestStream = request.GetRequestStream ( ) )
        {
          using ( StreamWriter writer = new StreamWriter ( requestStream ) )
          {
            writer.Write ( requestBody );
          }
        }
      }
    }

    // ==================================================================================
    /// <summary>
    /// This method convers the returned repsonse into a string.
    /// </summary>
    /// <param name="response">HttpWebResponse object containing the web service respoinse</param>
    /// <returns>String containing the reponse content.</returns>
    // ---------------------------------------------------------------------------------
    private static String ConvertResponseToString (
      HttpWebResponse response )
    {
      Program.writeDebugLogMethod ( "ConvertResponseToString method. " );
      //
      // Extract the header for debug.
      //
      Program.writeDebugLog ( "Status code: " + (int) response.StatusCode + " " + response.StatusCode );

      foreach ( string key in response.Headers.Keys )
      {
        Program.writeDebugLog ( String.Format ( "{0}: {1}", key, response.Headers [ key ] ) );
      }

      string result = new StreamReader ( response.GetResponseStream ( ) ).ReadToEnd ( );

      Program.writeDebugLog ( "ConvertResponseToString method FINISHED." );

      return result;
    }

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region debug methods.

    //==================================================================================
    /// <summary>
    /// This method writes out the process log to the local console.
    /// </summary>
    /// <param name="content">String: content</param>
    //-----------------------------------------------------------------------------------
    private static void writeProcessLog ( String content )
    {
      if ( content != String.Empty )
      {
        Program._ProcessLog.AppendLine (
          DateTime.Now.ToString ( "dd-MM-yy HH:mm:ss" )
          + ": " + content );
      }
    }

    //==================================================================================
    /// <summary>
    /// This method writes out the method debug log to the local console.
    /// </summary>
    /// <param name="content">String: content</param>
    //-----------------------------------------------------------------------------------
    private static void writeDebugLogMethod ( String Content )
    {
      if ( DebugOn == false )
      {
        return;
      }
      System.Console.WriteLine ( "--------------------------------------------------"
        + "-------------------------------------------------------------------" );
      System.Console.WriteLine ( DateTime.Now.ToString ( "dd-MM-yy HH:mm:ss" ) + " DEBUG: "
        + "Evado.IntegrationClient.Program." + Content );
    }

    //==================================================================================
    /// <summary>
    /// This method writes out the debug log to the local console.
    /// </summary>
    /// <param name="content">String: content</param>
    //-----------------------------------------------------------------------------------
    private static void writeDebugLog ( String Content )
    {
      if ( DebugOn == false )
      {
        return;
      }
      System.Console.WriteLine ( DateTime.Now.ToString ( "dd-MM-yy HH:mm:ss" ) + " DEBUG: " + Content );
    }

    #endregion

  }//END Class

}//END NameSpace
