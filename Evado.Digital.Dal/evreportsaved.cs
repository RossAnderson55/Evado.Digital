/* <copyright file="DAL\EvReportSaved.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2021 EVADO HOLDING PTY. LTD..  All rights reserved.
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
 ****************************************************************************************/


using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;

//References to Evado specific libraries

using Evado.Model;
using Evado.Digital.Model;


namespace Evado.Digital.Dal
{
  /// <summary>
  /// This class is handles the data access layer for saving the generated reports data object.
  /// </summary>
  public class EvReportSaved : EvDalBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EvReportSaved ( )
    {
      this.ClassNameSpace = "Evado.Digital.Dal.Digital.EvReportSaved.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EvReportSaved ( EvClassParameters Settings )
    {
      this.ClassParameters = Settings;
      this.ClassNameSpace = "Evado.Digital.Dal.Digital.EvReportSaved.";

      if ( this.ClassParameters.LoggingLevel == 0 )
      {
        this.ClassParameters.LoggingLevel = Evado.Digital.Dal.EvStaticSetting.LoggingLevel;
      }
    }

    #endregion

    #region Initialise Class Constants and variables

    private static string _eventLogSource = ConfigurationManager.AppSettings [ "EventLogSource" ];

    private const string _sqlQuery_View = "Select *  FROM EvReportSaved ";

    // 
    // The SQL Store Procedure constants
    // 
    private const string _STORED_PROCEDURE_AddReport = "usr_ReportSaved_add";

    //private const string _STORED_PROCEDURE_UpdateReport = "usr_ReportTemplate_update";
    //private const string _STORED_PROCEDURE_Delete = "usr_ReportTemplate_delete";

    private const string _parmGuid = "@Guid";
    private const string _parmTrialId = "@TrialId";
    private const string _parmReportId = "@ReportId";
    private const string _parmReportNo = "@ReportNo";
    private const string _parm_ReportDate = "@ReportDate";
    private const string _parmReportTitle = "@ReportTitle";
    //private const string _parmReportSubTitle = "@ReportSubTitle";
    private const string _parmReportTypeId = "@ReportTypeId";
    private const string _parmCategory = "@Category";
    private const string _parmXmlTemplate = "@XmlReport";
    private const string _parmUpdatedByUserId = "@UpdatedByUserId";
    private const string _parmUpdatedBy = "@UpdatedBy";
    private const string _parmUpdateDate = "@UpdateDate";

    #endregion

    #region SQL Query parmeter methods

    // =====================================================================================
    /// <summary>
    /// This class return an array of sql query parameters.
    /// </summary>
    /// <returns>SqlParameter: an array of sql query parameters</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Create an array of sql query parameters. 
    /// 
    /// 2. Return an array of sql query parameters. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private static SqlParameter [ ] getReportsParameters ( )
    {
      SqlParameter [ ] parms = new SqlParameter [ ] 
      {
        new SqlParameter( _parmGuid, SqlDbType.UniqueIdentifier),
        new SqlParameter( _parmTrialId, SqlDbType.NVarChar, 10),
        new SqlParameter( _parmReportId, SqlDbType.NVarChar, 10),
        new SqlParameter( _parmReportNo, SqlDbType.Int),
        new SqlParameter( _parm_ReportDate, SqlDbType.DateTime),
        new SqlParameter( _parmReportTitle, SqlDbType.NVarChar, 100),
        new SqlParameter( _parmReportTypeId, SqlDbType.SmallInt),
        new SqlParameter( _parmCategory, SqlDbType.NVarChar, 100),
        new SqlParameter( _parmXmlTemplate, SqlDbType.NText),
        new SqlParameter( _parmUpdatedByUserId, SqlDbType.NVarChar, 100 ),
        new SqlParameter( _parmUpdatedBy, SqlDbType.NVarChar, 100 ),
        new SqlParameter( _parmUpdateDate, SqlDbType.DateTime )
      };

      return parms;
    }//END getReportsParameters class. 

    // =====================================================================================
    /// <summary>
    /// This class sets the values from Report object to the array of sql parameters. 
    /// </summary>
    /// <param name="parms">SqlParameter: an array of sql parameters</param>
    /// <param name="Report">EvReport: Values to bind to parameters</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Bind the values from Report object to the array of parameters. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private void SetParameters ( SqlParameter [ ] parms, EdReport Report )
    {
      parms [ 0 ].Value = Report.Guid;
      parms [ 1 ].Value = String.Empty ;
      parms [ 2 ].Value = Report.ReportId;
      parms [ 3 ].Value = Report.ReportNo;
      parms [ 4 ].Value = Report.ReportDate;
      parms [ 5 ].Value = Report.ReportTitle;
      parms [ 6 ].Value = Report.ReportType;
      parms [ 7 ].Value = Report.Category;
      parms [ 8 ].Value = Evado.Model.EvStatics.SerialiseXmlObject<EdReport> ( Report );
      parms [ 9 ].Value = Report.UpdateUserId;
      parms [ 10 ].Value = Report.UserCommonName;
      parms [ 11 ].Value = DateTime.Now;

    }//END SetParameters class.

    #endregion

    #region Data Reader methods

    // =====================================================================================
    /// <summary>
    /// This class extracts the data row values to the report object. 
    /// </summary>
    /// <param name="Row">DataRow: a data row object</param>
    /// <returns>EvReport: a report data object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Extract the compatible data row values to the Report data object. 
    /// 
    /// 2. Set the xml report string, if it is empty. 
    /// 
    /// 3. Return the Report data object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EdReport readRow ( DataRow Row )
    {
      // 
      // Initialise the methods variables and objects.
      // 
      EdReport report = new EdReport ( );

      //
      // Set the xml report string, if it is empty. 
      //
      string xmlReport = EvSqlMethods.getString ( Row, "RS_XmlReport" );
      if ( xmlReport != String.Empty )
      {
        xmlReport = xmlReport.Replace ( "utf-8", "utf-16" );

        xmlReport = xmlReport.Replace ( "<SelectionSource></SelectionSource>",
          "<SelectionSource>None</SelectionSource>" );

        xmlReport = xmlReport.Replace ( "<SelectionSource />",
          "<SelectionSource>None</SelectionSource>" );

        xmlReport = xmlReport.Replace ( "<SelectionSource>SubjectId</SelectionSource>",
          "<SelectionSource>Subject_Id</SelectionSource>" );

        xmlReport = xmlReport.Replace ( "<SelectionSource>SiteId</SelectionSource>",
          "<SelectionSource>Site_Id</SelectionSource>" );

        xmlReport = xmlReport.Replace ( "<SelectionSource>TrialId</SelectionSource>",
          "<SelectionSource>Trial_Id</SelectionSource>" );

        xmlReport = xmlReport.Replace ( "<SelectionSource>Trial_Id</SelectionSource>",
          "<SelectionSource>Project_Id</SelectionSource>" );

        xmlReport = xmlReport.Replace ( "<SelectionSource>ScheduleId</SelectionSource>",
          "<SelectionSource>Arm_Index</SelectionSource>" );

        xmlReport = xmlReport.Replace ( "<SelectionSource>AllTrialSites</SelectionSource>",
          "<SelectionSource>All_Trial_Sites</SelectionSource>" );

        xmlReport = xmlReport.Replace ( "<SelectionSource>CurrentTrial</SelectionSource>",
          "<SelectionSource>Current_Trial</SelectionSource>" );

        xmlReport = xmlReport.Replace ( "<SelectionSource>Current_Trial</SelectionSource>",
          "<SelectionSource>Current_Project</SelectionSource>" );

        xmlReport = xmlReport.Replace ( "<TrialId>", "<ProjectId>" );
        xmlReport = xmlReport.Replace ( "</TrialId>", "</ProjectId>" );
        xmlReport = xmlReport.Replace ( "<ReportTypeId>", "<ReportType>" );
        xmlReport = xmlReport.Replace ( "</ReportTypeId>", "</ReportType>" );

        report = Evado.Model.EvStatics.DeserialiseXmlObject<EdReport> ( xmlReport ); ;
      }

      //
      // Extract the compatible data row values to the Report object. 
      //
      report.Guid = EvSqlMethods.getGuid ( Row, "RS_Guid" );
      report.ReportId = EvSqlMethods.getString ( Row, "ReportId" );
      report.ReportTitle = EvSqlMethods.getString ( Row, "RS_ReportTitle" );
      //Report.ReportSubTitle = EvSqlMethods.getString( Row, "RS_ReportSubTitle" );
      report.ReportDate = EvSqlMethods.getDateTime ( Row, "RS_ReportDate" );
      report.ReportType = ( EdReport.ReportTypeCode ) EvSqlMethods.getInteger ( Row, "RS_ReportTypeId" );
      report.ReportNo = EvSqlMethods.getInteger ( Row, "RS_ReportNo" );

      report.UpdateUserId = EvSqlMethods.getString ( Row, "RS_UpdatedByUserId" );
      report.Updated = EvSqlMethods.getString ( Row, "RS_UpdatedBy" );
      report.Updated += " on " + EvSqlMethods.getDateTime ( Row, "RS_UpdateDate" ).ToString ( "dd MMM yyyy HH:mm" );

      report.GeneratedBy = EvSqlMethods.getString ( Row, "RS_UpdatedBy" );

      //
      // Return the Report object
      //
      return report;

    }// End readRow method.

    #endregion

    #region Database selectionList query methods

    // =====================================================================================
    /// <summary>
    /// This class returns a list of Reports based on the TrialId, RecordTypeId and Category value. 
    /// </summary>
    /// <param name="ProjectId">string: (Mandatory) The Project identifier</param>
    /// <param name="ReportTypeId">EvReport.ReportTypeCode: The Report Type Identifier</param>
    /// <param name="Category">EvReport.ReportTypeCode: The Report's category</param>
    /// <returns>List of EvReport: A list of Report data objects.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define the sql query parameters and sql query string
    /// 
    /// 2. Execute the sql query string and store the results on data table. 
    /// 
    /// 3. Loop through the table and extract the row data to the Report object. 
    /// 
    /// 4. Add the Report object value to the Report list. 
    /// 
    /// 5. Return the report list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EdReport> getView ( string ProjectId, EdReport.ReportTypeCode ReportTypeId, string Category )
    {
      this.LogMethod ( "getView. " );
      this.LogDebug ( "Projectid: " + ProjectId);
      this.LogDebug ( "ReportTypeId: " + ReportTypeId);
      this.LogDebug ( "Category: " + Category );

      // 
      // Define the local variables
      // 
      string _sqlQueryString;
      List<EdReport> view = new List<EdReport> ( );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( _parmTrialId, SqlDbType.NVarChar, 10),
        new SqlParameter( _parmReportTypeId, SqlDbType.SmallInt),
        new SqlParameter( _parmCategory, SqlDbType.NVarChar, 100),
      };
      cmdParms [ 0 ].Value = ProjectId;
      cmdParms [ 1 ].Value = ReportTypeId;
      cmdParms [ 2 ].Value = Category;

      // 
      // Generate the SQL query string
      // 
      _sqlQueryString = _sqlQuery_View + "WHERE (TrialId = @TrialId) ";

      if ( ReportTypeId != EdReport.ReportTypeCode.Null )
      {
        _sqlQueryString += " AND (RS_ReportTypeId = @ReportTypeId ) ";
      }

      if ( Category != String.Empty)
      {
        _sqlQueryString += " AND (RS_Category = @Category ) ";
      }

      _sqlQueryString += " ORDER BY ReportId, RS_ReportNo; ";

     this.LogDebug ( _sqlQueryString );

      //
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery( _sqlQueryString, cmdParms ) )
      {
        // 
        // Iterate through the results extracting the Report information.
        // 
        for ( int Count = 0; Count < table.Rows.Count; Count++ )
        {
          // 
          // Extract the table row
          // 
          DataRow row = table.Rows [ Count ];

          EdReport Report = this.readRow( row );

          view.Add( Report );
        }
      }

     this.LogDebug ("view Count: " + view.Count );

      // 
      // Return the ArrayList containing the Report data object.
     // 
     this.LogMethodEnd ( "getView." );
      return view;

    } // Close getView method.

    #endregion

    #region Data Object Retrieval methods

    // =====================================================================================
    /// <summary>
    /// This class retrieve the Report data table based on Guid value. 
    /// </summary>
    /// <param name="Guid">Guid: A report's global unique identifier</param>
    /// <returns>EvReport: a report data object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define the sql query parameters and sql query string. 
    /// 
    /// 2. Execute the sql query string and store the results on data table. 
    /// 
    /// 3. Extract the first row data to the report object. 
    /// 
    /// 4. Return the Report data object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EdReport getReport ( Guid Guid )
    {
     this.LogMethod ( "getReport. ");
      this.LogDebug ( "Guid: " + Guid );

      // 
      // Define the local variables
      // 
      string _sqlQueryString;
      EdReport report = new EdReport( );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( _parmGuid, SqlDbType.UniqueIdentifier),
      };
      cmdParms [ 0 ].Value = Guid;

      // 
      // Generate the SQL query string
      // 
      _sqlQueryString = _sqlQuery_View + "WHERE (RS_Guid = @Guid); ";
      this.LogDebug ( _sqlQueryString );

      //
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery(  _sqlQueryString, cmdParms ) )
      {
        // 
        // If not rows the return
        // 
        if ( table.Rows.Count == 0 )
        {
          return report;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        // 
        // Fill the Report object.
        // 
        report = this.readRow( row );

      }//END Using 

      // 
      // Return the Report data object.
      // 
     this.LogMethodEnd ( "getReport." );
      return report;

    } // Close getReport method.

    #endregion

    #region Update Data methods

    // =====================================================================================
    /// <summary>
    /// This class adds items to the Report table based on the selected report object. 
    /// 
    /// Author: Ross Anderson
    /// Date: 12/10/2005 
    /// </summary>
    /// <param name="Report">EvReport: a report data object</param>
    /// <returns>EvEventCodes: an event code for adding items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the VisitId or User Common Name is empty. 
    /// 
    /// 2. Define the sql query parameter and execute the storeprocedure for adding items. 
    /// 
    /// 3. Exit, if the storeprocedure runs fail
    /// 
    /// 4. Else, return the event code for adding items. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes addReport ( EdReport Report )
    {
      this.LogMethod ( "addReport. ");
      this.LogDebug ( "Guid: " + Report.Guid);
      this.LogDebug ( "ReportId: " + Report.ReportId);
      this.LogDebug ( "ReportTitle: " + Report.ReportTitle);
      this.LogDebug ( "ReportSubTitle: " + Report.ReportSubTitle);
      this.LogDebug ( "ReportCommonName: " + Report.UserCommonName );

      // 
      // Define the local variables.
      // 
      string _sqlQueryString = String.Empty;

      if ( Report.UserCommonName == String.Empty )
      {
       this.LogDebug ( "CommonUser missing. " );
        return EvEventCodes.Identifier_User_Common_Name_Error;
      }

      // 
      // set the FormUid for the Report.
      // 
      Report.Guid = Guid.NewGuid( );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = getReportsParameters( );
      SetParameters( cmdParms, Report );

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate( _STORED_PROCEDURE_AddReport, cmdParms ) == 0 )
      {
        this.LogMethodEnd ( "addReport." );
        return EvEventCodes.Database_Record_Update_Error;
      }

      this.LogMethodEnd ( "addReport." );
      return EvEventCodes.Ok;

    } // Close method addReport

    #endregion

  }//END EvReportSaved class

}//END namespace Evado.Digital.Dal
