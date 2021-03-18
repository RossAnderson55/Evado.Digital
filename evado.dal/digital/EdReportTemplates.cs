/* <copyright file="DAL\EvReportTemplates.cs" company="EVADO HOLDING PTY. LTD.">
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
 * Description:
 *  This class handles the database query interface for the EvReportTemplates object.
 * 
 *  This class contains the following public properties:
 *   DebugLog:       Containing the exeuction status of this class, used for debugging the 
 *                 class from BLL or UI layers.
 * 
 *  This class contains the following public methods:
 * 
 *   geTemplateList:      Executes a query to generate list of EvReport objects objects.
 * 
 *   getReport:    Executes a query to return a EvReport object by the UserProfileGuid; 
 * 
 *   getReport:    Executes a query to return a EvReport object by the FormUid; 
 * 
 *   getReport:    Executes a query to return a EvReport object by the RecordId; 
 * 
 *   updateReport:   Executee a query to update the database content and generate an date change
 *                 entry for the update.
 * 
 *   addReport:      Executee a query to add a new EvReport object to the database.
 * 
 *   deleteReport:   Executee a query to delete a new EvReport object to the database.
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
using Evado.Model.Digital;


namespace Evado.Dal.Digital
{
  /// <summary>
  /// A business Component used to manage Ethics reports
  /// The Evado.Evado.Report is used in most methods 
  /// and is used to store serializable information about an account
  /// </summary>
  public class EdReportTemplates : EvDalBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EdReportTemplates ( )
    {
      this.ClassNameSpace = "Evado.Dal.Digital.EdReportTemplates.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EdReportTemplates ( EvClassParameters Settings )
    {
      this.ClassParameters = Settings;
      this.ClassNameSpace = "Evado.Dal.Digital.EdReportTemplates.";
    }

    #endregion

    #region Initialise Class Constants and variables

    private static string _eventLogSource = ConfigurationManager.AppSettings [ "EventLogSource" ];

    private const string SQL_QUERY_REPORT_SOURCES = "Select *  FROM EV_REPORT_SOURCES ";

    private const string SQL_QUERY_REPORT_SOURCE_QUERIES = "Select *  FROM EV_REPORT_SOURCE_QUERIES ";

    private const string SQl_QUERY_REPORT_SOURCE_COLUMNS = "Select *  FROM EV_REPORT_SOURCE_COLUMNS ";

    private const string SQl_QUERY_REPORT_TEMPLATES = "Select *  FROM EvReportTemplates ";

    private const string SQL_QUERY_REPORT_TEMPLATE_QUERIES = "Select *  FROM EV_REPORT_TEMPLATE_QUERIES ";

    private const string SQl_QUERY_REPORT_TEMPLATE_COLUMNS = "Select *  FROM EV_REPORT_TEMPLATE_COLUMNS ";

    private const string SQl_QUERY_REPORT_CATEGORIES = "SELECT RT_Category AS Rpt_Category "
      + " FROM EvReportTemplates WHERE (TrialId = @TrialId) AND (RT_Superseded = 0) GROUP BY RT_Category";

    /// 
    /// The SQL Store Procedure constants
    /// 
    private const string _STORED_PROCEDURE_AddReport = "usr_ReportTemplate_add";
    private const string _STORED_PROCEDURE_UpdateReport = "usr_ReportTemplate_update";
    private const string _STORED_PROCEDURE_DeleteItem = "usr_ReportTemplate_delete";

    private const string DB_SOURCE_ID = "RTS_SOURCE_ID";
    private const string DB_SOURCE_NAME = "RTS_SOURCE_NAME";
    private const string DB_SOURCE_DESCRIPTION = "RTS_DESCRIPTION";
    private const string DB_REPORT_SOURCE = "RTS_SOURCE";
    private const string DB_SOURCE_SQL_QUERY = "RTS_SQL_QUERY";

    private const string DB_SOURCE_QUERY_ID = "RTSQ_QUERY_ID";
    private const string DB_SOURCE_QUERY_TITLE = "RTSQ_TITLE";
    private const string DB_SOURCE_COLUMN_ID = "RTSC_COLUMN_ID";

    private const string DB_REPORT_GUID = "RT_Guid";
    private const string DB_REPORT_ID = "ReportId";
    private const string DB_REPORT_TITLE = "RT_ReportTitle";
    private const string DB_REPORT_SUB_TITLE = "RT_ReportSubTitle";
    private const string DB_REPORT_VERSION = "RT_Version";
    private const string DB_REPORT_STATE = "RT_State";
    private const string DB_REPORT_CATEGORY = "RT_Category";
    private const string DB_REPORT_UPDATE_USER_ID = "RT_UpdatedByUserId";
    private const string DB_REPORT_UPDATE_BY = "RT_UpdatedBy";
    private const string DB_REPORT_UPDATE_DATE = "RT_UpdateDate";
    private const string DB_REPORT_DELETED = "RT_Superseded";
    private const string DB_REPORT_XML_REPORT = "RT_XmlReport";
    private const string DB_REPORT_TYPE = "RT_REPORT_TYPE";
    private const string DB_REPORT_SCOPE = "RT_REPORT_SCOPE";
    private const string DB_REPORT_IS_AGGRETATED = "RT_IS_AGGRETATED";
    private const string DB_REPORT_DATA_SOURCE_ID = "RT_DATA_SOURCE_ID";
    private const string DB_REPORT_SQL_SOURCE = "RT_SQL_SOURCE";
    private const string DB_REPORT_LAYOUT_TYPE_ID = "RT_LAYOUT_TYPE_ID";

    private const string DB_REPORT_QUERY_INDEX = "RTQ_INDEX";
    private const string DB_REPORT_QUERY_SELECTION_SOURCE = "RTQ_SELECTION_SOURCE";
    private const string DB_REPORT_QUERY_FIELD_NAME = "RTQ_FIELD_NAME";
    private const string DB_REPORT_QUERY_FIELD_VALUE = "RTQ_FIELD_VALUE";
    private const string DB_REPORT_QUERY_PROMPT = "RTQ_PROMPT";
    private const string DB_REPORT_QUERY_DATA_TYPE = "RTQ_DATA_TYPE";
    private const string DB_REPORT_QUERY_OPERATOR = "RTQ_OPERATOR";
    private const string DB_REPORT_QUERY_MANDATORY = "RTQ_MANDATORY";

    private const string DB_REPORT_COLUMN_ORDER = "RTC_ORDER";
    private const string DB_REPORT_COLUMN_HEADER_TEXT = "RTC_HEADER_TEXT";
    private const string DB_REPORT_COLUMN_SOURCE_FIELD = "RTC_SOURCE_FIELD";
    private const string DB_REPORT_COLUMN_STYLE_WIDTH = "RTC_STYLE_WIDTH";
    private const string DB_REPORT_COLUMN_DATA_TYPE = "RTC_DATA_TYPE";
    private const string DB_REPORT_COLUMN_VALUE_FORMATING = "RTC_VALUE_FORMATING";
    private const string DB_REPORT_COLUMN_VALUE_GROUPING_INDEX = "RTC_VALUE_GROUPING_INDEX";
    private const string DB_REPORT_COLUMN_VALUE_GROUPING_TYPE = "RTC_VALUE_GROUPING_TYPE";
    private const string DB_REPORT_COLUMN_VALUE_SECTION_LEVEL = "RTC_VALUE_SECTION_LEVEL";

    private const string PARM_Guid = "@Guid";
    private const string PARM_TRIAL_ID = "@TrialId";
    private const string PARM_REPORT_ID = "@ReportId";
    private const string PARM_ReportTitle = "@REPORT_TITLE";
    private const string PARM_ReportSubTitle = "@REPORT_SUB_TITLE";
    private const string PARM_REPORT_TYPE = "@REPORT_TYPE";
    private const string PARM_REPORT_SCOPE = "@REPORT_SCOPE";
    private const string PARM_REPORT_DATA_SOURCE_ID = "@DATA_SOURCE_ID";
    private const string PARM_REPORT_SQL_SOURCE = "@SQL_SOURCE";
    private const string PARM_REPORT_LAYOUT_TYPE_ID = "@LAYOUT_TYPE_ID";
    private const string PARM_Version = "@Version";
    private const string PARM_State = "@State";
    private const string PARM_CATEGORY = "@Category";
    private const string PARM_IS_AGGRETATED = "@IS_AGGRETATED";
    private const string PARM_XmlTemplate = "@XmlReport";
    private const string PARM_UpdatedByUserId = "@UpdatedByUserId";
    private const string PARM_UpdatedBy = "@UpdatedBy";
    private const string PARM_UpdateDate = "@UpdateDate";

    private const string PARM_SOURCE_ID = "@SOURCE_ID";
    private const string PARM_SOURCE_QUERY_ID = "@QUERY_ID";
    private const string PARM_SOURCE_COLUMN_ID = "RTSC_COLUMN_ID";


    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region SQL Query parmeter methods

    // =====================================================================================
    /// <summary>
    /// This class returns an array of sql query parameters. 
    /// </summary>
    /// <returns>SqlParameter: an array of sql query parameters</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Create an array of the sql query parameters. 
    /// 
    /// 2. Return an array of sql query parameters. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private static SqlParameter [ ] getReportsParameters ( )
    {
      SqlParameter [ ] parms = new SqlParameter [ ] 
      {
        new SqlParameter( PARM_Guid, SqlDbType.UniqueIdentifier),
        new SqlParameter( PARM_TRIAL_ID, SqlDbType.NVarChar, 10),
        new SqlParameter( PARM_REPORT_ID, SqlDbType.NVarChar, 20),
        new SqlParameter( PARM_ReportTitle, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_ReportSubTitle, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_REPORT_TYPE, SqlDbType.NVarChar, 50),
        new SqlParameter( PARM_REPORT_SCOPE, SqlDbType.NVarChar, 50),
        new SqlParameter( PARM_SOURCE_ID, SqlDbType.NVarChar, 30),
        new SqlParameter( PARM_REPORT_DATA_SOURCE_ID, SqlDbType.NVarChar, 50),
        new SqlParameter( PARM_REPORT_SQL_SOURCE, SqlDbType.NText),
        new SqlParameter( PARM_REPORT_LAYOUT_TYPE_ID, SqlDbType.NVarChar, 30),
        new SqlParameter( PARM_IS_AGGRETATED, SqlDbType.Bit),
        new SqlParameter( PARM_Version, SqlDbType.SmallInt),
        new SqlParameter( PARM_CATEGORY, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_XmlTemplate, SqlDbType.NText),
        new SqlParameter( PARM_UpdatedByUserId, SqlDbType.NVarChar, 100 ),
        new SqlParameter( PARM_UpdatedBy, SqlDbType.NVarChar, 100 ),
        new SqlParameter( PARM_UpdateDate, SqlDbType.DateTime )
      };

      return parms;
    }

    // =====================================================================================
    /// <summary>
    /// This class sets the values from report object to the array of sql query parameters. 
    /// </summary>
    /// <param name="parms">SqlParameter: an array of sql query parameters</param>
    /// <param name="Report">EvReport: a report data object</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Bind the values from Report object to the array of sql query parameters. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private void SetParameters ( SqlParameter [ ] parms, EdReport Report )
    {
      parms [ 0 ].Value = Report.Guid;
      parms [ 1 ].Value = String.Empty;
      parms [ 2 ].Value = Report.ReportId;
      parms [ 3 ].Value = Report.ReportTitle;
      parms [ 4 ].Value = Report.ReportSubTitle;
      parms [ 5 ].Value = Report.ReportType.ToString ( );
      parms [ 6 ].Value = Report.ReportScope.ToString ( );
      parms [ 7 ].Value = Report.SourceId;
      parms [ 8 ].Value = Report.DataSourceId.ToString ( );
      parms [ 9 ].Value = Report.SqlDataSource;
      parms [ 10 ].Value = Report.LayoutTypeId.ToString ( ); ;
      parms [ 11 ].Value = Report.IsAggregated;
      parms [ 12 ].Value = Report.Version;
      parms [ 13 ].Value = Report.Category;
      parms [ 14 ].Value = String.Empty; //
      parms [ 15 ].Value = Report.UpdateUserId;
      parms [ 16 ].Value = Report.UserCommonName;
      parms [ 17 ].Value = DateTime.Now;

    }//END SetParameters class.

    #endregion

    #region Data Reader methods

    // =====================================================================================
    /// <summary>
    /// This class extracts the data row values to the report object. 
    /// </summary>
    /// <param name="Row">DataRow: a data row object</param>
    /// <returns>EvReportSource: a report data object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Extract the compatible data row values to the report data object. 
    /// 
    /// 2. Return the Report data object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvReportSource readSourceRow ( DataRow Row )
    {
      // 
      // Initialise the methods variables and objects.
      // 
      EvReportSource reportSource = new EvReportSource ( );

      // 
      // Extract the data object values.
      // 
      reportSource.SourceId = EvSqlMethods.getString ( Row, EdReportTemplates.DB_SOURCE_ID );
      reportSource.Name = EvSqlMethods.getString ( Row, EdReportTemplates.DB_SOURCE_NAME );
      reportSource.Description = EvSqlMethods.getString ( Row, EdReportTemplates.DB_SOURCE_DESCRIPTION );

      String value = EvSqlMethods.getString ( Row, EdReportTemplates.DB_REPORT_SOURCE );

      reportSource.ReportSource =
        Evado.Model.EvStatics.parseEnumValue<EdReport.ReportSourceCode> (
        EvSqlMethods.getString ( Row, EdReportTemplates.DB_REPORT_SOURCE ) );

      reportSource.SqlQuery = EvSqlMethods.getString ( Row, EdReportTemplates.DB_SOURCE_SQL_QUERY );


      //
      // Return the Report object
      //
      return reportSource;

    }// End readRow method.

    // =====================================================================================
    /// <summary>
    /// This class extracts the data row values to the report object. 
    /// </summary>
    /// <param name="Row">DataRow: a data row object</param>
    /// <param name="IsSource">Bool: true indicated is source data.</param>
    /// <returns>EvReportQuery: a report query object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Extract the compatible data row values to the report data object. 
    /// 
    /// 2. Return the Report data object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EdReportQuery readQueryRow ( DataRow Row, bool IsSource )
    {
      // 
      // Initialise the methods variables and objects.
      // 
      EdReportQuery reportQuery = new EdReportQuery ( );

      // 
      // Extract the data object values.
      // 
      reportQuery.QueryId =
        EvSqlMethods.getString ( Row, EdReportTemplates.DB_SOURCE_QUERY_ID );
      reportQuery.QueryTitle =
        EvSqlMethods.getString ( Row, EdReportTemplates.DB_SOURCE_QUERY_TITLE );

      reportQuery.SelectionSource =
        EvSqlMethods.getString<EdReport.SelectionListTypes> ( Row, EdReportTemplates.DB_REPORT_QUERY_SELECTION_SOURCE );



      reportQuery.FieldName =
        EvSqlMethods.getString ( Row, EdReportTemplates.DB_REPORT_QUERY_FIELD_NAME );
      reportQuery.Prompt =
        EvSqlMethods.getString ( Row, EdReportTemplates.DB_REPORT_QUERY_PROMPT );
      reportQuery.DataType =
        EvSqlMethods.getString<EdReport.DataTypes> ( Row, EdReportTemplates.DB_REPORT_QUERY_DATA_TYPE );

      //
      // IF the source query is false add the report query columns.
      //
      if ( IsSource == false )
      {
        reportQuery.Index =
          EvSqlMethods.getInteger ( Row, EdReportTemplates.DB_REPORT_QUERY_INDEX );
        reportQuery.Mandatory =
          EvSqlMethods.getBool ( Row, EdReportTemplates.DB_REPORT_QUERY_MANDATORY );
        reportQuery.Operator =
          EvSqlMethods.getString<EdReport.Operators> ( Row, EdReportTemplates.DB_REPORT_QUERY_OPERATOR );
      }

      //
      // Return the Report object
      //
      return reportQuery;

    }// End readRow method.

    // =====================================================================================
    /// <summary>
    /// This class extracts the data row values to the report object. 
    /// </summary>
    /// <param name="Row">DataRow: a data row object</param>
    /// <param name="IsSource">Bool: true indicates is source data.</param>
    /// <returns>EvReportQuery: a report query object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Extract the compatible data row values to the report data object. 
    /// 
    /// 2. Return the Report data object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EdReportColumn readColumnRow ( DataRow Row, bool IsSource )
    {
      // 
      // Initialise the methods variables and objects.
      // 
      EdReportColumn reportColumn = new EdReportColumn ( );

      // 
      // Extract the data object values.
      // 
      reportColumn.ColumnId = EvSqlMethods.getString ( Row, EdReportTemplates.DB_SOURCE_COLUMN_ID );
      reportColumn.HeaderText = EvSqlMethods.getString ( Row, EdReportTemplates.DB_REPORT_COLUMN_HEADER_TEXT );
      reportColumn.SourceField = EvSqlMethods.getString ( Row, EdReportTemplates.DB_REPORT_COLUMN_SOURCE_FIELD );
      reportColumn.StyleWidth = EvSqlMethods.getString ( Row, EdReportTemplates.DB_REPORT_COLUMN_STYLE_WIDTH );
      reportColumn.DataType =
        EvSqlMethods.getString<EdReport.DataTypes> ( Row, EdReportTemplates.DB_REPORT_COLUMN_DATA_TYPE );
      reportColumn.ValueFormatingString = EvSqlMethods.getString ( Row, EdReportTemplates.DB_REPORT_COLUMN_VALUE_FORMATING );

      //
      // IF the source query is false add the report query columns.
      //
      if ( IsSource == false )
      {
        reportColumn.SourceOrder =
          EvSqlMethods.getInteger ( Row, EdReportTemplates.DB_REPORT_COLUMN_ORDER );
        reportColumn.GroupingIndex =
          EvSqlMethods.getBool ( Row, EdReportTemplates.DB_REPORT_COLUMN_VALUE_GROUPING_INDEX );
        reportColumn.GroupingType =
          EvSqlMethods.getString<EdReport.GroupingTypes> ( Row, EdReportTemplates.DB_REPORT_COLUMN_VALUE_GROUPING_TYPE );
        reportColumn.SectionLvl =
          EvSqlMethods.getInteger ( Row, EdReportTemplates.DB_REPORT_COLUMN_VALUE_SECTION_LEVEL );

        //
        // set the selection list for sorting in SQL
        //
        if ( reportColumn.SectionLvl == 99 )
        {
          reportColumn.SectionLvl = 0;
        }
      }

      //
      // Return the Report object
      //
      return reportColumn;

    }// End readRow method.

    // =====================================================================================
    /// <summary>
    /// This class extracts the data row values to the report object. 
    /// </summary>
    /// <param name="Row">DataRow: a data row object</param>
    /// <returns>EvReport: a report data object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Extract the compatible data row values to the report data object. 
    /// 
    /// 2. Return the Report data object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EdReport readTemplateRow ( DataRow Row )
    {
      // 
      // Initialise the methods variables and objects.
      // 
      EdReport report = new EdReport ( );
      String value = String.Empty;

      // 
      // Extract the data object values.
      // 
      report.Guid = EvSqlMethods.getGuid ( Row, EdReportTemplates.DB_REPORT_GUID );
      report.ReportId = EvSqlMethods.getString ( Row, EdReportTemplates.DB_REPORT_ID );
      report.LastReportId = report.ReportId;
      report.SourceId = EvSqlMethods.getString ( Row, EdReportTemplates.DB_SOURCE_ID );
      report.ReportTitle = EvSqlMethods.getString ( Row, EdReportTemplates.DB_REPORT_TITLE );
      report.ReportSubTitle = EvSqlMethods.getString ( Row, EdReportTemplates.DB_REPORT_SUB_TITLE );
      report.Category = EvSqlMethods.getString ( Row, EdReportTemplates.DB_REPORT_CATEGORY );
      report.IsAggregated =
        EvSqlMethods.getBool ( Row, EdReportTemplates.DB_REPORT_IS_AGGRETATED );
      this.LogDebug ( "report.IsAggregated: " + report.IsAggregated );

      value = EvSqlMethods.getString ( Row, EdReportTemplates.DB_REPORT_TYPE );
      if ( value != String.Empty )
      {
        report.ReportType = Evado.Model.EvStatics.
          parseEnumValue<EdReport.ReportTypeCode> ( value );
      }

      value = EvSqlMethods.getString ( Row, EdReportTemplates.DB_REPORT_SCOPE );
      if ( value != String.Empty )
      {
        report.ReportScope = Evado.Model.EvStatics.
          parseEnumValue<EdReport.ReportScopeTypes> ( value );
      }

      report.SourceId = EvSqlMethods.getString ( Row, EdReportTemplates.DB_SOURCE_ID );

      value = EvSqlMethods.getString ( Row, EdReportTemplates.DB_REPORT_DATA_SOURCE_ID );
      if ( value != String.Empty )
      {
        report.DataSourceId = Evado.Model.EvStatics.
          parseEnumValue<EdReport.ReportSourceCode> ( value );
      }

      value = EvSqlMethods.getString ( Row, EdReportTemplates.DB_REPORT_SQL_SOURCE );
      if ( value != String.Empty )
      {
        report.SqlDataSource = value;
      }

      value = EvSqlMethods.getString ( Row, EdReportTemplates.DB_REPORT_LAYOUT_TYPE_ID );
      if ( value != String.Empty )
      {
        report.LayoutTypeId = Evado.Model.EvStatics.
          parseEnumValue<EdReport.LayoutTypeCode> ( value );
      }

      report.Version = EvSqlMethods.getInteger ( Row, EdReportTemplates.DB_REPORT_VERSION );
      report.UpdateUserId = EvSqlMethods.getString ( Row, EdReportTemplates.DB_REPORT_UPDATE_USER_ID );

      report.Updated = EvSqlMethods.getString ( Row, EdReportTemplates.DB_REPORT_UPDATE_BY );

      report.Updated += " on " + EvSqlMethods.getDateTime ( Row, EdReportTemplates.DB_REPORT_UPDATE_DATE ).ToString ( "dd MMM yyyy HH:mm" );

      //
      // Return the Report object
      //
      return report;

    }// End readRow method.

    #endregion

    #region Database selectionList query methods

    // =====================================================================================
    /// <summary>
    /// This class returns a list of report objects based on the passed parameters. 
    /// </summary>
    /// <param name="ProjectId">string: (Mandatory) The Project identifier</param>
    /// <param name="ReportType">EvReport.ReportTypeCode: The Report type identifier</param>
    /// <param name="Category">string: the Category string</param>
    /// <param name="IncludeSuperseded">Boolean: true, if the report is superseded</param>
    /// <param name="ReportScope">EvReport.ReportScopeTypes: The report scope type for predefined report templates.</param>
    /// <returns>List of EvReport: the list of report data object containing the template.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define the sql query string and sql query parameters. 
    /// 
    /// 2. Execute the sql query string and store the results on the data table.
    /// 
    /// 3. Loop through the table and extract the row data to the Report object. 
    /// 
    /// 4. Add the report object to the report list. 
    /// 
    /// 5. Return the Report list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EdReport> getReportList (
      EdReport.ReportTypeCode ReportType,
      EdReport.ReportScopeTypes ReportScope,
      String Category,
      bool IncludeSuperseded )
    {
      this.LogMethod ( "getReportList method. " );
      this.LogDebug ( "ReportTypeId: " + ReportType );
      this.LogDebug ( "ReportScope: " + ReportScope );
      this.LogDebug ( "Category: " + Category );
      this.LogDebug ( "IncludeSuperseded: " + IncludeSuperseded );

      // 
      // Define the local variables
      // 
      string SqlQueryString;
      List<EdReport> view = new List<EdReport> ( );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( PARM_REPORT_SCOPE, SqlDbType.NVarChar, 50),
        new SqlParameter( PARM_REPORT_TYPE, SqlDbType.NVarChar, 50),
        new SqlParameter( PARM_CATEGORY, SqlDbType.NVarChar, 100),
      };

      cmdParms [ 0 ].Value = ReportScope.ToString ( );
      cmdParms [ 1 ].Value = ReportType;
      cmdParms [ 2 ].Value = Category;

      // 
      // Generate the SQL query string
      // 
      SqlQueryString = SQl_QUERY_REPORT_TEMPLATES + "WHERE (" + DB_REPORT_DELETED + " = 0 ) ";

      if ( ReportScope != EdReport.ReportScopeTypes.Null )
      {
        SqlQueryString += " AND (" + DB_REPORT_SCOPE + " = " + PARM_REPORT_SCOPE + ") ";
      }

      if ( ReportType != EdReport.ReportTypeCode.Null )
      {
        SqlQueryString += " AND (" + DB_REPORT_TYPE + " = " + PARM_REPORT_TYPE + " ) ";
      }

      if ( Category != String.Empty )
      {
        SqlQueryString += " AND (" + DB_REPORT_CATEGORY + " = " + PARM_CATEGORY + ") ";
      }

      if ( IncludeSuperseded == false )
      {
        SqlQueryString += " AND (" + DB_REPORT_DELETED + " = 0 ) ";
      }

      SqlQueryString += " ORDER BY " + DB_REPORT_ID + "; ";

      this.LogDebug ( SqlQueryString );

      //
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( SqlQueryString, cmdParms ) )
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

          //
          // Read the table row.
          //
          EdReport report = this.readTemplateRow ( row );

          this.LogDebug ( "Report: " + report.ReportId + ", Type: " + report.ReportType + ", Scope: " + report.ReportScope );
          //
          // Add the report to the view.
          //
          view.Add ( report );

        }//END iteration loop.

      }//END user statement

      this.LogDebug ( "View Count: " + view.Count.ToString ( ) );

      // 
      // Return the ArrayList containing the Report data object.
      // 
      return view;

    } // Close getView method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of report objects based on the passed parameters. 
    /// </summary>
    /// <returns>List of EvReport: the list of report data object containing the template.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define the sql query string and sql query parameters. 
    /// 
    /// 2. Execute the sql query string and store the results on the data table.
    /// 
    /// 3. Loop through the table and extract the row data to the Report object. 
    /// 
    /// 4. Add the report object to the report list. 
    /// 
    /// 5. Return the Report list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> getReportListAll ( )
    {
       
      this.LogMethod ( "getReportListAll method. " );

      // 
      // Define the local variables
      // 
      string SqlQueryString;
      List<EvOption> list = new List<EvOption> ( );
      EvOption option = new EvOption ( );

      // 
      // Generate the SQL query string
      // 
      SqlQueryString = SQl_QUERY_REPORT_TEMPLATES 
        + " WHERE  (" + DB_REPORT_DELETED + " = 0 ) " 
        + " ORDER BY " + DB_REPORT_ID + "; ";

      this.LogDebug ( SqlQueryString );

      //
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( SqlQueryString, null ) )
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
          option = new EvOption (
            EvSqlMethods.getString ( row, EdReportTemplates.DB_REPORT_ID ),
            EvSqlMethods.getString ( row, EdReportTemplates.DB_REPORT_TITLE ) );

          list.Add ( option );

        }//END iteration loop.

      }//END user statement

      this.LogDebug ( "View Count: " + list.Count.ToString ( ) );

      // 
      // Return the ArrayList containing the Report data object.
      // 
      return list;

    } // Close getView method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of Options based on ProjectId
    /// </summary>
    /// <param name="ProjectId">string: (Mandatory) The trial identifier</param>
    /// <returns>List of EvOption: the list of Option data objects.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define the sql query parameters and sql query string. 
    /// 
    /// 2. Execute the sql query string and store the results on data table. 
    /// 
    /// 3. Loop through data table and extract data row to the Option object. 
    /// 
    /// 4. Add the Option object to the Options list. 
    /// 
    /// 5. Return the Options list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> getCategoryList ( string ProjectId )
    {
      this.LogMethod ( "getCategoryList method. " );
      this.LogDebug ( "ProjectId: " + ProjectId );
      // 
      // Define the local variables
      // 
      string SqlQueryString;
      List<EvOption> list = new List<EvOption> ( );

      // 
      // Add the null first option to the selection visitSchedule.
      // 
      EvOption option = new EvOption ( );
      list.Add ( option );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( PARM_TRIAL_ID, SqlDbType.NVarChar, 10),
      };
      cmdParms [ 0 ].Value = ProjectId;

      // 
      // Generate the SQL query string
      // 
      SqlQueryString = SQl_QUERY_REPORT_CATEGORIES;
      //_Status += "\r\n" + sqlQueryString;

      //
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( SqlQueryString, cmdParms ) )
      {
        // 
        // Iterate through the results extracting the role information.
        // 
        for ( int Count = 0; Count < table.Rows.Count; Count++ )
        {
          // 
          // Extract the table row
          // 
          DataRow row = table.Rows [ Count ];
          option = new EvOption (
            EvSqlMethods.getString ( row, "Rpt_Category" ),
            EvSqlMethods.getString ( row, "Rpt_Category" ) );

          list.Add ( option );
        }
      }

      // 
      // Return the ArrayList containing the EvOption data object.
      // 
      return list;

    } // Close getCategoryList method.

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Data Source methods

    // =====================================================================================
    /// <summary>
    /// This class returns a list of report objects based on the passed parameters. 
    /// </summary>
    /// <returns>List of EvReportSource: the list of report data object containing the template.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define the sql query string and sql query parameters. 
    /// 
    /// 2. Execute the sql query string and store the results on the data table.
    /// 
    /// 3. Loop through the table and extract the row data to the Report object. 
    /// 
    /// 4. Add the report object to the report list. 
    /// 
    /// 5. Return the Report list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvReportSource> getSourceList ( )
    {
       
      this.LogMethod ( "getReportSourceList method. " );

      // 
      // Define the local variables
      // 
      string SqlQueryString;
      List<EvReportSource> sourceList = new List<EvReportSource> ( );

      // 
      // Generate the SQL query string
      // 
      SqlQueryString = SQL_QUERY_REPORT_SOURCES + "WHERE (RTS_DELETED = 0) ORDER BY RTS_SOURCE_ID ";

      this.LogDebug ( SqlQueryString );

      //
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( SqlQueryString, null ) )
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

          //
          // Read the table row.
          //
          EvReportSource reportSource = this.readSourceRow ( row );

          this.LogDebug ( "Report SourceId: " + reportSource.SourceId + ", Name: " + reportSource.Name );

          //
          // get the sources list of query objects
          //
          reportSource.QueryList = this.getSourceQuery ( reportSource.SourceId );

          //
          // get the sources list of column objects
          //
          reportSource.ColumnList = this.getSourceColumns ( reportSource.SourceId );

          //
          // Add the report to the view.
          //
          sourceList.Add ( reportSource );

        }//END iteration loop.

      }//END user statement

      this.LogDebug ( "Source list count: " + sourceList.Count.ToString ( ) );

      // 
      // Return the ArrayList containing the Report data object.
      // 
      return sourceList;

    } // Close getView method.

    // =====================================================================================
    /// <summary>
    /// This class retrieves Report data table based on Guid
    /// </summary>
    /// <param name="SourceId">Guid: The report's Global unique identifier</param>
    /// <returns>EvReport: the report data object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define the sql query parameters and sql query string. 
    /// 
    /// 2. Execute the sql query string and store the results on data table. 
    /// 
    /// 3. If the table has no value, return an empty report object. 
    /// 
    /// 4. Else, extract the first row data to the report object. 
    /// 
    /// 5. Return the Report object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private EvReportSource getSource ( String SourceId )
    {
      this.LogMethod ( "getSource method. " );
      this.LogDebug ( "SourceId: " + SourceId );
      // 
      // Define the local variables
      // 
      string SqlQueryString;
      EvReportSource reportSource = new EvReportSource ( );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( PARM_SOURCE_ID, SqlDbType.UniqueIdentifier),
      };
      cmdParms [ 0 ].Value = SourceId;

      // 
      // Generate the SQL query string
      // 
      SqlQueryString = SQL_QUERY_REPORT_SOURCES + "WHERE (" + DB_SOURCE_ID + "  = " + PARM_SOURCE_ID + " ; ";
      this.LogDebug ( SqlQueryString );

      //
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( SqlQueryString, cmdParms ) )
      {
        // 
        // If not rows the return
        // 
        if ( table.Rows.Count == 0 )
        {
          return reportSource;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        // 
        // Fill the Report object.
        // 
        reportSource = this.readSourceRow ( row );


      }//END Using 

      //
      // get the sources list of query objects
      //
      reportSource.QueryList = this.getSourceQuery ( reportSource.SourceId );

      //
      // get the sources list of column objects
      //
      reportSource.ColumnList = this.getSourceColumns ( reportSource.SourceId );

      // 
      // Return the Report data object.
      // 
      this.LogDebug ( "END getSource." );

      return reportSource;

    } // Close getSource method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of report objects based on the passed parameters. 
    /// </summary>
    /// <param name="SourceId">String: the source identifier</param>
    /// <returns>List of EvReportQuery: the list of report query objects.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define the sql query string and sql query parameters. 
    /// 
    /// 2. Execute the sql query string and store the results on the data table.
    /// 
    /// 3. Loop through the table and extract the row data to the Report object. 
    /// 
    /// 4. Add the report object to the report list. 
    /// 
    /// 5. Return the Report list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private List<EdReportQuery> getSourceQuery ( String SourceId )
    {
      //this.writeDebugLogMethod ( "getSourceQuery method. " );

      // 
      // Define the local variables
      // 
      string SqlQueryString;
      List<EdReportQuery> sourceQueryList = new List<EdReportQuery> ( );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( PARM_SOURCE_ID, SqlDbType.NVarChar, 50 ),
      };

      cmdParms [ 0 ].Value = SourceId;

      // 
      // Generate the SQL query string
      // 
      SqlQueryString = SQL_QUERY_REPORT_SOURCE_QUERIES
        + "WHERE (" + DB_SOURCE_ID + "=" + PARM_SOURCE_ID + ") ORDER BY " + DB_SOURCE_QUERY_ID + ";";

      //this.writeDebugLogLine ( SqlQueryString );

      //
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( SqlQueryString, cmdParms ) )
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

          //
          // Read the table row.
          //
          EdReportQuery reportQuery = this.readQueryRow ( row, true );


          //this.writeDebugLogLine ( "Report QueryID: " + reportQuery.QueryId + ", Title: " + reportQuery.QueryTitle );
          //
          // Add the report to the view.
          //
          sourceQueryList.Add ( reportQuery );

        }//END iteration loop.

      }//END user statement

      this.LogDebug ( "Source Query list count: " + sourceQueryList.Count.ToString ( ) );

      // 
      // Return the ArrayList containing the Report data object.
      // 
      return sourceQueryList;

    } // Close getReportSourceQuery method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of report objects based on the passed parameters. 
    /// </summary>
    /// <param name="SourceId">String: the source identifier</param>
    /// <returns>List of EvReportColumn: the list of report columns objects.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define the sql query string and sql query parameters. 
    /// 
    /// 2. Execute the sql query string and store the results on the data table.
    /// 
    /// 3. Loop through the table and extract the row data to the Report object. 
    /// 
    /// 4. Add the report object to the report list. 
    /// 
    /// 5. Return the Report list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private List<EdReportColumn> getSourceColumns ( String SourceId )
    {
      //this.writeDebugLogMethod ( "getSourceQuery method. " );

      // 
      // Define the local variables
      // 
      string SqlQueryString;
      List<EdReportColumn> sourceColumnList = new List<EdReportColumn> ( );
      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( PARM_SOURCE_ID, SqlDbType.NVarChar, 50 ),
      };

      cmdParms [ 0 ].Value = SourceId;

      // 
      // Generate the SQL query string
      // 
      SqlQueryString = SQl_QUERY_REPORT_SOURCE_COLUMNS + "WHERE (" + DB_SOURCE_ID + "=" + PARM_SOURCE_ID + ") ORDER BY " + DB_SOURCE_COLUMN_ID + ";";

      this.LogDebug ( SqlQueryString );

      //
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( SqlQueryString, cmdParms ) )
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

          //
          // Read the table row.
          //
          EdReportColumn reportColumn = this.readColumnRow ( row, true );


          //this.writeDebugLogLine ( "Report Column ID: " + reportColumn.ColumnId + ", Header text: " + reportColumn.HeaderText );
          //
          // Add the report to the view.
          //
          sourceColumnList.Add ( reportColumn );

        }//END iteration loop.

      }//END user statement

      this.LogDebug ( "Source column list count: " + sourceColumnList.Count.ToString ( ) );

      // 
      // Return the ArrayList containing the Report data object.
      // 
      return sourceColumnList;

    } // Close getSourceColumns method.

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Data Object Retrieval methods

    // =====================================================================================
    /// <summary>
    /// This class retrieves Report data table based on Guid
    /// </summary>
    /// <param name="ReportGuid">Guid: The report's Global unique identifier</param>
    /// <returns>EvReport: the report data object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define the sql query parameters and sql query string. 
    /// 
    /// 2. Execute the sql query string and store the results on data table. 
    /// 
    /// 3. If the table has no value, return an empty report object. 
    /// 
    /// 4. Else, extract the first row data to the report object. 
    /// 
    /// 5. Return the Report object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EdReport getReport ( Guid ReportGuid )
    {
       
      this.LogMethod ( "getReport method. " );
      this.LogDebug ( "ReportGuid: " + ReportGuid );
      // 
      // Define the local variables
      // 
      string SqlQueryString;
      EdReport report = new EdReport ( );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( EdReportTemplates.PARM_Guid, SqlDbType.UniqueIdentifier),
      };
      cmdParms [ 0 ].Value = ReportGuid;

      // 
      // Generate the SQL query string
      // 
      SqlQueryString = SQl_QUERY_REPORT_TEMPLATES + "WHERE (" + EdReportTemplates.DB_REPORT_GUID + "=" + EdReportTemplates.PARM_Guid + "); ";

      this.LogDebug ( SqlQueryString );

      //
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( SqlQueryString, cmdParms ) )
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
        report = this.readTemplateRow ( row );

      }//END Using 

      if ( report.ReportId != String.Empty )
      {
        //
        // get the column values.
        //
        report.Queries = this.getReportQueries ( report.ReportId );

        //
        // get the column values.
        //
        report.Columns = this.getReportColumns ( report.ReportId );
      }

      report.SiteColumnHeaderText = string.Empty;

      //Setting the report site header text for the filtering.
      /*
      if ( this.fldUserSiteFilter.Visible && this.fldUserSiteFilter.Checked == true )
      {
        foreach ( EvReportColumn column in this._Report.Columns )
        {
          if ( column.SourceField.Contains ( "OrgId" )
            && column.SectionLvl != 0 )
          {
            this._Report.SiteColumnHeaderText = column.HeaderText;
            break;
          }
        }
      }
      */

      this.LogDebug ( "Report: " + report.ReportId );

      // 
      // Return the Report data object.
      // 
      this.LogDebug ( "END getReport." );
      return report;

    } // Close getReport method.

    // =====================================================================================
    /// <summary>
    /// This class retrieves the report data object based on ProjectId and ReportId
    /// </summary>
    /// <param name="ProjectId">string: (Mandatory) Project identifier.</param>
    /// <param name="ReportId">string: (Mandatory) Report identifier.</param>
    /// <returns>EvReport: a report data object.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define the sql query parameters and sql query string. 
    /// 
    /// 2. Execute the sql query string and store the results on data table. 
    /// 
    /// 3. If the table has no value, return an empty report object. 
    /// 
    /// 4. Else, extract the first row data to the report object. 
    /// 
    /// 5. Return the Report object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private EdReport getReport (
      String ReportId )
    {
       
      this.LogMethod ( "getreportList method. " );
      this.LogDebug ( "ReportId: " + ReportId );

      // 
      // Define the local variables
      // 
      string SqlQueryString;
      EdReport report = new EdReport ( );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( EdReportTemplates.PARM_REPORT_ID, SqlDbType.NVarChar, 20),
      };
      cmdParms [ 0 ].Value = ReportId;

      // 
      // Generate the SQL query string
      // 
      SqlQueryString = SQl_QUERY_REPORT_TEMPLATES
        + "WHERE (" + EdReportTemplates.DB_REPORT_ID + "=" + EdReportTemplates.PARM_REPORT_ID + ") " 
        + "AND (" + EdReportTemplates.DB_REPORT_DELETED + "= 0); ";
     
      this.LogDebug ( SqlQueryString );

      //
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( SqlQueryString, cmdParms ) )
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
        report = this.readTemplateRow ( row );

      }//END Using 

      //
      // get the column values.
      //
      if ( report.Queries.Length == 0 )
      {
        report.Queries = this.getReportQueries ( report.ReportId );
      }

      //
      // get the column values.
      //
      if ( report.Columns.Count == 0 )
      {
        report.Columns = this.getReportColumns ( report.ReportId );
      }

      // 
      // Return the Report data object.
      // 
      this.LogDebug ( "END getReport." );

      return report;

    } // Close getReport method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of report query objects based on the passed parameters. 
    /// </summary>
    /// <param name="ReportId">String: the report identifier</param>
    /// <returns>an array of EvReportQuery: of report query objects.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define the sql query string and sql query parameters. 
    /// 
    /// 2. Execute the sql query string and store the results on the data table.
    /// 
    /// 3. Loop through the table and extract the row data to the Report object. 
    /// 
    /// 4. Add the report object to the report list. 
    /// 
    /// 5. Return the Report list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private EdReportQuery [ ] getReportQueries ( String ReportId )
    {
      this.LogMethod ( "getSourceQuery method. " );
      this.LogDebug ( "ReportId: " + ReportId );

      // 
      // Define the local variables
      // 
      string SqlQueryString;
      EdReportQuery [ ] arrRportQueries = new EdReportQuery [ 5 ];

      //
      // Initialise the query array;
      //
      for ( int i = 0; i < arrRportQueries.Length; i++ )
      {
        arrRportQueries [ i ] = new EdReportQuery ( );
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( EdReportTemplates.PARM_REPORT_ID, SqlDbType.NVarChar, 50 ),
      };

      cmdParms [ 0 ].Value = ReportId;

      // 
      // Generate the SQL query string
      // 
      SqlQueryString = SQL_QUERY_REPORT_TEMPLATE_QUERIES
        + "WHERE (" + EdReportTemplates.DB_REPORT_ID + "=" + EdReportTemplates.PARM_REPORT_ID + ") ORDER BY " + EdReportTemplates.DB_SOURCE_QUERY_ID + ";";

      this.LogDebug ( SqlQueryString );

      //
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( SqlQueryString, cmdParms ) )
      {
        // 
        // Iterate through the results extracting the Report information.
        // 
        for ( int count = 0; count < table.Rows.Count && count < arrRportQueries.Length; count++ )
        {
          // 
          // Extract the table row
          // 
          DataRow row = table.Rows [ count ];

          //
          // Read the table row.
          //
          EdReportQuery reportQuery = this.readQueryRow ( row, false );

          this.LogDebug ( "Report QueryID: " + reportQuery.QueryId + ", Title: " + reportQuery.QueryTitle );
          //
          // Add the report to the view.
          //
          arrRportQueries [ count ] = reportQuery;

        }//END iteration loop.

      }//END user statement

      this.LogDebug ( "Source Query array length: " + arrRportQueries.Length );

      // 
      // Return the ArrayList containing the Report data object.
      // 
      return arrRportQueries;

    } // Close getReportQuery method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of report column objects based on the passed parameters. 
    /// </summary>
    /// <param name="ReportId">String: the report identifier</param>
    /// <returns>List of EvReportColumn: the list of report columns objects.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define the sql query string and sql query parameters. 
    /// 
    /// 2. Execute the sql query string and store the results on the data table.
    /// 
    /// 3. Loop through the table and extract the row data to the Report object. 
    /// 
    /// 4. Add the report object to the report list. 
    /// 
    /// 5. Return the Report list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private List<EdReportColumn> getReportColumns ( String ReportId )
    {
      this.LogMethod ( "getReportColumns method. " );

      // 
      // Define the local variables
      // 
      string SqlQueryString;
      List<EdReportColumn> sourceColumnList = new List<EdReportColumn> ( );
      bool sourceIndexed = false;

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( EdReportTemplates.PARM_REPORT_ID, SqlDbType.NVarChar, 50 ),
      };

      cmdParms [ 0 ].Value = ReportId;

      // 
      // Generate the SQL query string
      // 
      SqlQueryString = SQl_QUERY_REPORT_TEMPLATE_COLUMNS + "WHERE (" + EdReportTemplates.DB_REPORT_ID + "=" + EdReportTemplates.PARM_REPORT_ID + ") "
        + " ORDER BY " +  EdReportTemplates.DB_REPORT_COLUMN_ORDER + ";";

      this.LogDebug ( SqlQueryString );

      //
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( SqlQueryString, cmdParms ) )
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

          //
          // Read the table row.
          //
          EdReportColumn reportColumn = this.readColumnRow ( row, false );

          if ( reportColumn.SourceOrder > 0 )
          {
            sourceIndexed = true;
          }

          this.LogDebug ( "Report ColumnId: " + reportColumn.ColumnId
            + ", Title: " + reportColumn.HeaderText
            + ", GroupingIndex: " + reportColumn.GroupingIndex
            + ", Title: " + reportColumn.GroupingType
            + ", SectionLvl: " + reportColumn.SectionLvl );
          //
          // Add the report to the view.
          //
          if ( reportColumn.ColumnId != String.Empty )
          {
            sourceColumnList.Add ( reportColumn );
          }
        }//END iteration loop.

      }//END user statement

      this.LogDebug ( "Report column list count: " + sourceColumnList.Count.ToString ( ) );

      if ( sourceIndexed == false )
      {
        for ( int count = 0; count < sourceColumnList.Count; count++ )
        {
          sourceColumnList [ count ].SourceOrder = ( count * 2 + 1 );
        }
      }

      // 
      // Return the ArrayList containing the Report data object.
      // 
      return sourceColumnList;

    } // Close getReportColumns method.

    #endregion

    #region Update Data methods    

    // =====================================================================================
    /// <summary>
    /// This class updates the items to the Report table based on the Report data object. 
    /// </summary>
    /// <param name="ReportTemplate">EvReport: a Report object</param>
    /// <returns>EvEventCodes: an event code for updating items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit,if the report object's ProjectId or Old report object's Guid is empty. 
    /// 
    /// 2. Add items from Report object to datachange object if they do not exist after comparing. 
    /// 
    /// 3. Define the sql query parameters and execute the storeprocedure for updating items. 
    /// 
    /// 4. Exit, if the storeprocedure runs fail
    /// 
    /// 5. Else, add the datachange object values to the backup datachanges object. 
    /// 
    /// 6. Return the event code for updating items. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes updateReport ( EdReport ReportTemplate )
    {
       
      this.LogMethod ( "updateReport method." );
      this.LogDebug ( "Guid: " + ReportTemplate.Guid );
      this.LogDebug ( "ReportId: " + ReportTemplate.ReportId );
      this.LogDebug ( "ReportTitle: " + ReportTemplate.ReportTitle );
      this.LogDebug ( "ReportSubTitle: " + ReportTemplate.ReportSubTitle );
      this.LogDebug ( "userCommonName: " + ReportTemplate.UserCommonName );

      // 
      // Define the local variables.
      // 
      int RowsAffected = 0;
      EvEventCodes result = EvEventCodes.Ok;
      EvDataChanges dataChanges = new EvDataChanges ( );
      EvDataChange dataChange = new EvDataChange ( );

      //
      // Validate whether the Old Report object's Guid is not empty.
      //
      EdReport oldReportTemplate = this.getReport ( ReportTemplate.Guid );
      if ( oldReportTemplate.Guid == Guid.Empty )
      {
        return EvEventCodes.Data_InvalidId_Error;
      }

      // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      // Compare the objects.
      // 
      dataChange = updateDataChanges ( oldReportTemplate, ReportTemplate );

      // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      // 
      // Define the SQL query parameters and load the query values.
      // 
      this.LogDebug ( "Update report details" );

      SqlParameter [ ] cmdParms = getReportsParameters ( );
      SetParameters ( cmdParms, ReportTemplate );

      //
      // Execute the update command.
      //
      if ( ( RowsAffected = EvSqlMethods.StoreProcUpdate ( _STORED_PROCEDURE_UpdateReport, cmdParms ) ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      //
      // Update the template queries.
      //
      result = this.updateTemplateQueries ( ReportTemplate );

      //
      // Update the template columns
      //
      result = this.updateTemplateColumns ( ReportTemplate );

      // 
      // Append the data change newField.
      // 
      dataChanges.AddItem ( dataChange );
      this.LogDebug ( "DataChange Status: \r\n" + dataChanges.Log );

      return EvEventCodes.Ok;

    }//END updateReport class

    //===================================================================================
    /// <summary>
    /// This method generates the change data object for this transaction.
    /// </summary>
    /// <param name="OldReportTemplate">EvReport: the old report object.</param>
    /// <param name="ReportTemplate">EvReport: the new report object.</param>
    /// <returns>EvDataChange object</returns>
    //-----------------------------------------------------------------------------------
    private EvDataChange updateDataChanges (
      EdReport OldReportTemplate,
      EdReport ReportTemplate )
    {
      //
      // Initialise the data change object.
      //
      EvDataChange dataChange = new EvDataChange ( );
      dataChange.TableName = EvDataChange.DataChangeTableNames.EvReportTemplates;
      dataChange.RecordUid = 0;
      dataChange.RecordGuid = ReportTemplate.Guid;
      dataChange.UserId = ReportTemplate.UpdateUserId;
      dataChange.DateStamp = DateTime.Now;

      //
      // Add items from Report object to the datachange object if they do not exist on the old Report object. 
      //
      if ( ReportTemplate.ReportId != OldReportTemplate.ReportId )
      {
        dataChange.AddItem ( "ReportId", OldReportTemplate.ReportId, ReportTemplate.ReportId );
      }
      if ( ReportTemplate.ReportTitle != OldReportTemplate.ReportTitle )
      {
        dataChange.AddItem ( "ReportTitle", OldReportTemplate.ReportTitle, ReportTemplate.ReportTitle );
      }
      if ( ReportTemplate.ReportSubTitle != OldReportTemplate.ReportSubTitle )
      {
        dataChange.AddItem ( "ReportSubTitle", OldReportTemplate.ReportSubTitle, ReportTemplate.ReportSubTitle );
      }
      if ( ReportTemplate.ReportType != OldReportTemplate.ReportType )
      {
        dataChange.AddItem ( "ReportTypeId", OldReportTemplate.ReportType.ToString ( ), ReportTemplate.ReportType.ToString ( ) );
      }
      if ( ReportTemplate.ReportScope != OldReportTemplate.ReportScope )
      {
        dataChange.AddItem ( "ReportScope", OldReportTemplate.ReportScope.ToString ( ), ReportTemplate.ReportScope.ToString ( ) );
      }
      if ( ReportTemplate.Version != OldReportTemplate.Version )
      {
        dataChange.AddItem ( "Version", OldReportTemplate.Version.ToString ( ), ReportTemplate.Version.ToString ( ) );
      }

      string oldXmlReport =
        Evado.Model.Digital.EvcStatics.SerialiseObject<EdReport> ( OldReportTemplate );

      string newXmlReport =
        Evado.Model.Digital.EvcStatics.SerialiseObject<EdReport> ( ReportTemplate );

      if ( ReportTemplate.Version != OldReportTemplate.Version )
      {
        dataChange.AddItem ( "XmlReport", oldXmlReport, newXmlReport );
      }

      //
      // Add the changes for queries
      //
      for ( int qCount = 0; qCount < ReportTemplate.Queries.Length & qCount < 5; qCount++ )
      {
        EdReportQuery newQuery = ReportTemplate.Queries [ qCount ];
        EdReportQuery oldQuery = new EdReportQuery ( );
        if ( qCount < OldReportTemplate.Queries.Length )
        {
          if ( OldReportTemplate.Queries [ qCount ] != null )
          {
            oldQuery = OldReportTemplate.Queries [ qCount ];
          }
        }

        if ( oldQuery.QueryId != newQuery.QueryId )
        {
          dataChange.AddItem ( "Query.QueryId", oldQuery.QueryId, newQuery.QueryId );
        }
        if ( oldQuery.Prompt != newQuery.Prompt )
        {
          dataChange.AddItem ( "Query.Prompt", oldQuery.Prompt, newQuery.Prompt );
        }
        if ( oldQuery.Index != newQuery.Index )
        {
          dataChange.AddItem ( "Query.Index", oldQuery.Index, newQuery.Index );
        }
        if ( oldQuery.DataType != newQuery.DataType )
        {
          dataChange.AddItem ( "Query.DataType", oldQuery.DataType.ToString ( ), newQuery.DataType.ToString ( ) );
        }
        if ( oldQuery.Mandatory != newQuery.Mandatory )
        {
          dataChange.AddItem ( "Query.Mandatory", oldQuery.Mandatory.ToString ( ), newQuery.Mandatory.ToString ( ) );
        }
        if ( oldQuery.FieldName != newQuery.FieldName )
        {
          dataChange.AddItem ( "Query.FieldName", oldQuery.FieldName.ToString ( ), newQuery.FieldName.ToString ( ) );
        }
        if ( oldQuery.Operator != newQuery.Operator )
        {
          dataChange.AddItem ( "Query.Operator", oldQuery.Operator.ToString ( ), newQuery.Operator.ToString ( ) );
        }
        if ( oldQuery.QueryTitle != newQuery.QueryTitle )
        {
          dataChange.AddItem ( "Query.QueryTitle", oldQuery.QueryTitle.ToString ( ), newQuery.QueryTitle.ToString ( ) );
        }
        if ( oldQuery.SelectionSource != newQuery.SelectionSource )
        {
          dataChange.AddItem ( "Query.SelectionSource", oldQuery.SelectionSource.ToString ( ), newQuery.SelectionSource.ToString ( ) );
        }
        if ( oldQuery.ValueName != newQuery.ValueName )
        {
          dataChange.AddItem ( "Query.ValueName", oldQuery.ValueName.ToString ( ), newQuery.ValueName.ToString ( ) );
        }
      }//END query iteration loop

      //
      // Add the changes for columns
      //
      for ( int cCount = 0; cCount < ReportTemplate.Columns.Count & cCount < 5; cCount++ )
      {
        EdReportColumn newColumn = ReportTemplate.Columns [ cCount ];
        EdReportColumn oldColumn = new EdReportColumn ( );
        if ( cCount < OldReportTemplate.Columns.Count )
        {
          oldColumn = OldReportTemplate.Columns [ cCount ];
        }

        if ( oldColumn.ColumnId != newColumn.ColumnId )
        {
          dataChange.AddItem ( "Column.ColumnId", oldColumn.ColumnId, newColumn.ColumnId );
        }
        if ( oldColumn.HeaderText != newColumn.HeaderText )
        {
          dataChange.AddItem ( "Column.HeaderText", oldColumn.HeaderText, newColumn.HeaderText );
        }
        if ( oldColumn.StyleWidth != newColumn.StyleWidth )
        {
          dataChange.AddItem ( "Column.StyleWidth", oldColumn.StyleWidth, newColumn.StyleWidth );
        }
        if ( oldColumn.ValueFormatingString != newColumn.ValueFormatingString )
        {
          dataChange.AddItem ( "Column.ValueFormatingString", oldColumn.ValueFormatingString.ToString ( ), newColumn.ValueFormatingString.ToString ( ) );
        }
        if ( oldColumn.DataType != newColumn.DataType )
        {
          dataChange.AddItem ( "Column.DataType", oldColumn.DataType.ToString ( ), newColumn.DataType.ToString ( ) );
        }
        if ( oldColumn.SourceField != newColumn.SourceField )
        {
          dataChange.AddItem ( "Column.SourceField", oldColumn.SourceField.ToString ( ), newColumn.SourceField.ToString ( ) );
        }
        if ( oldColumn.SourceOrder != newColumn.SourceOrder )
        {
          dataChange.AddItem ( "Column.SourceOrder", oldColumn.SourceOrder.ToString ( ), newColumn.SourceOrder.ToString ( ) );
        }
        if ( oldColumn.SectionLvl != newColumn.SectionLvl )
        {
          dataChange.AddItem ( "Column.SectionLvl", oldColumn.SectionLvl.ToString ( ), newColumn.SectionLvl.ToString ( ) );
        }
        if ( oldColumn.GroupingIndex != newColumn.GroupingIndex )
        {
          dataChange.AddItem ( "Column.GroupingIndex", oldColumn.GroupingIndex.ToString ( ), newColumn.GroupingIndex.ToString ( ) );
        }
        if ( oldColumn.GroupingType != newColumn.GroupingType )
        {
          dataChange.AddItem ( "Column.GroupingType", oldColumn.GroupingType.ToString ( ), newColumn.GroupingType.ToString ( ) );
        }
      }//END column iteration loop

      return dataChange;
    }

    // =====================================================================================
    /// <summary>
    /// This class adds items to the Report data table based on the Report data object. 
    /// 
    /// Author: Ross Anderson
    /// Date: 12/10/2005 
    /// </summary>
    /// <param name="ReportTemplate">EvReport: a report data object</param>
    /// <returns>EvEventCodes: an event code for adding items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the ProjectId is empty or UserCommonName is empty or Report's Guid is empty. 
    /// 
    /// 2. Define the sql query parameters and execute the storeprocedure for adding items. 
    /// 
    /// 3. Exit, if the storeprocedure rusn fail. 
    /// 
    /// 4. Else, return an event code for adding items. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes addReport ( EdReport ReportTemplate )
    {
      this.LogMethod ( "addReport method. " );
      this.LogDebug ( "Guid: " + ReportTemplate.Guid );
      this.LogDebug ( "ReportId: " + ReportTemplate.ReportId );
      this.LogDebug ( "ReportTitle: " + ReportTemplate.ReportTitle );
      this.LogDebug ( "ReportSubTitle: " + ReportTemplate.ReportSubTitle );
      this.LogDebug ( "UserCommonName: " + ReportTemplate.UserCommonName );

      // 
      // Define the local variables.
      // 
      string SqlQueryString = String.Empty;

      //
      // Validate whether the ProjectId or User common name is not empty. 
      //
      if ( ReportTemplate.ReportType == EdReport.ReportTypeCode.Null )
      {
        this.LogDebug ( "ReportType missing. " );
        return EvEventCodes.Identifier_General_ID_Error;
      }
      if ( ReportTemplate.UserCommonName == String.Empty )
      {
        this.LogDebug ( "CommonUser missing. " );
        return EvEventCodes.Identifier_User_Id_Error;
      }

      // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      // 
      // Check that the Report id is unique trial.
      // 
      EdReport report = this.getReport ( ReportTemplate.ReportId );
      if ( report.Guid != Guid.Empty )
      {
        this.LogDebug ( "Duplicate report Id. " );
        return EvEventCodes.Data_Duplicate_Id_Error;
      }

      // 
      // set the FormUid for the Report.
      // 
      ReportTemplate.Guid = Guid.NewGuid ( );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = getReportsParameters ( );
      this.SetParameters ( cmdParms, ReportTemplate );

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( _STORED_PROCEDURE_AddReport, cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      //
      // Update the template's queries.
      //
      this.updateTemplateQueries ( ReportTemplate );

      //
      // Update the template's columns.
      //
      this.updateTemplateColumns ( ReportTemplate );

      return EvEventCodes.Ok;

    }//END addReport class

    // =====================================================================================
    /// <summary>
    /// This class deletes items from the Report data table based on the report data object. 
    /// 
    /// Author: Ross Anderson
    /// Date: 12/10/2005 
    /// </summary>
    /// <param name="Report">EvReport: a report data object</param>
    /// <returns>EvEventCodes: an event code for deleting items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the ReportId or ProjectId or UserCommonName is empty. 
    /// 
    /// 2. Define the sql query parameters and execute the storeprocedure for deleting items. 
    /// 
    /// 3. Exit, if the storeprocedure runs fail
    /// 
    /// 4. Else, return an event code for deleting items. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes deleteItem ( EdReport Report )
    {
      this.LogMethod ( "deleteItem method. " );
      this.LogDebug ( "ReportId: " + Report.ReportId );

      if ( Report.UserCommonName == String.Empty )
      {
        return EvEventCodes.Identifier_User_Id_Error;
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( PARM_Guid, SqlDbType.UniqueIdentifier),
        new SqlParameter( PARM_UpdatedByUserId, SqlDbType.NVarChar,100),
        new SqlParameter( PARM_UpdatedBy, SqlDbType.NText),
        new SqlParameter( PARM_UpdateDate, SqlDbType.DateTime )
      };
      cmdParms [ 0 ].Value = Report.Guid;
      cmdParms [ 1 ].Value = this.ClassParameters.UserProfile.UserId;
      cmdParms [ 2 ].Value = this.ClassParameters.UserProfile.CommonName;
      cmdParms [ 3 ].Value = DateTime.Now;

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( _STORED_PROCEDURE_DeleteItem, cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      return EvEventCodes.Ok;

    }//END deleteItem method

    #endregion

    #region Template query update methods

    // =====================================================================================
    /// <summary>
    /// This method updates the Milestone's activities by iterating through the passes newField list.
    /// </summary>
    /// <param name="ReportTemplate">EvReport: the report template object.</param>
    /// <returns>EvEventCodes: an event code for updating activity object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Iterate through deletes the existing query objects.
    /// 
    /// 2. Iterates through the list of queries creating inser queries for each query. 
    /// 
    /// 3. Return the event code for updating the items. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes updateTemplateQueries (
      EdReport ReportTemplate )
    {
      this.LogMethod ( "updateTemplateQueries." );
      this.LogDebug  ( "ReportTemplate.Guid: " + ReportTemplate.Guid );
      this.LogDebug  ( "Queries.Length: " + ReportTemplate.Queries.Length );

      //
      // Initialize the debug log and the return event code. 
      //
      System.Text.StringBuilder SqlUpdateQuery = new System.Text.StringBuilder ( );
      bool executeUpdate = false;
      ReportTemplate.LastReportId = ReportTemplate.ReportId;

      //
      // Delete the milestone activities for this milestone.
      //
      SqlUpdateQuery.AppendLine ( "/** DELETE ALL OF REPORT TEOMPLATE QUERIES ROW FOR REPORT TEMPLATE **/" );
      SqlUpdateQuery.AppendLine ( " DELETE FROM EV_REPORT_TEMPLATE_QUERIES " );
      SqlUpdateQuery.AppendLine ( " WHERE  ReportId = '" + ReportTemplate.ReportId + "' ; " );

      // 
      // Iterate through each newField in the activity list for
      // Updating those activities that have changed.
      // 
      foreach ( EdReportQuery query in ReportTemplate.Queries )
      {
        //
        // if the activity id is empty continue to the next value.
        //
        if ( query.QueryId == String.Empty )
        {
          this.LogDebug  ( "QueryId: EMPTY" );
          query.QueryId = query.SelectionSource.ToString ( );
          query.QueryTitle = query.SelectionSource.ToString ( );
          //continue;
        }

        if ( query.QueryId == EdReport.SelectionListTypes.None.ToString ( )
          && query.FieldName == String.Empty )
        {
          continue;
        }

        if ( query.QueryId == EdReport.SelectionListTypes.None.ToString ( ) )
        {
          query.QueryId = query.FieldName;

          if ( query.FieldName.Length > 19 )
          {
            query.QueryId = query.FieldName.Substring ( 0, 19 );
          }
          query.QueryTitle = query.QueryId;
        }

        this.LogDebug  ( "QueryId: " + query.QueryId );
        this.LogDebug  ( "Mandatory: " + query.Mandatory );

        int iMandatory = 0;
        if ( query.Mandatory == true )
        {
          iMandatory = 1;
        }

        SqlUpdateQuery.AppendLine ( "/****  INSERT ACTIVITY " + query.QueryId + " ****/" );
        SqlUpdateQuery.AppendLine ( "Insert Into EV_REPORT_TEMPLATE_QUERIES " );
        SqlUpdateQuery.AppendLine ( " (ReportId, " );
        SqlUpdateQuery.AppendLine ( "  RTSQ_QUERY_ID, " );
        SqlUpdateQuery.AppendLine ( "  RTQ_INDEX, " );
        SqlUpdateQuery.AppendLine ( "  RTSQ_TITLE, " );
        SqlUpdateQuery.AppendLine ( "  RTQ_SELECTION_SOURCE, " );
        SqlUpdateQuery.AppendLine ( "  RTQ_FIELD_NAME, " );
        SqlUpdateQuery.AppendLine ( "  RTQ_PROMPT, " );
        SqlUpdateQuery.AppendLine ( "  RTQ_DATA_TYPE, " );
        SqlUpdateQuery.AppendLine ( "  RTQ_OPERATOR, " );
        SqlUpdateQuery.AppendLine ( "  RTQ_MANDATORY )" );
        SqlUpdateQuery.AppendLine ( "values " );
        SqlUpdateQuery.AppendLine ( " ('" + ReportTemplate.ReportId + "', " );
        SqlUpdateQuery.AppendLine ( "  '" + query.QueryId + "', " );
        SqlUpdateQuery.AppendLine ( "   " + query.Index + ", " );
        SqlUpdateQuery.AppendLine ( "  '" + query.QueryTitle + "', " );
        SqlUpdateQuery.AppendLine ( "  '" + query.SelectionSource + "', " );
        SqlUpdateQuery.AppendLine ( "  '" + query.FieldName + "', " );
        SqlUpdateQuery.AppendLine ( "  '" + query.Prompt + "', " );
        SqlUpdateQuery.AppendLine ( "  '" + query.DataType + "', " );
        SqlUpdateQuery.AppendLine ( "  '" + query.Operator + "', " );
        SqlUpdateQuery.AppendLine ( "  " + iMandatory + "); " );

      }//END iteration loop.

      this.LogDebug  ( "Sql Query: " + SqlUpdateQuery.ToString ( ) );

      if ( EvSqlMethods.QueryUpdate ( SqlUpdateQuery.ToString ( ), null ) == 0
        && executeUpdate == true )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      return EvEventCodes.Ok;

    }//END updateTemplateQueries method

    #endregion

    #region Template columns update methods

    // =====================================================================================
    /// <summary>
    /// This method updates the Milestone's activities by iterating through the passes newField list.
    /// </summary>
    /// <param name="ReportTemplate">EvReport: the report template object.</param>
    /// <returns>EvEventCodes: an event code for updating activity object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Iterate through deletes the existing query objects.
    /// 
    /// 2. Iterates through the list of queries creating inser queries for each query. 
    /// 
    /// 3. Return the event code for updating the items. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes updateTemplateColumns (
      EdReport ReportTemplate )
    {
      this.LogMethod ( "updateTemplateColumns." );
      this.LogDebug  ( "ReportTemplate.Columns.Count: " + ReportTemplate.Columns.Count );

      //
      // Initialize the debug log and the return event code. 
      //
      System.Text.StringBuilder SqlUpdateQuery = new System.Text.StringBuilder ( );
      if ( ReportTemplate.LastReportId == null )
      {
        ReportTemplate.LastReportId = String.Empty;
      }

      //
      // Delete the milestone activities for this milestone.
      //
      SqlUpdateQuery.AppendLine ( "/** DELETE ALL OF REPORT TEOMPLATE QUERIES ROW FOR REPORT TEMPLATE **/" );
      SqlUpdateQuery.AppendLine ( " DELETE FROM EV_REPORT_TEMPLATE_COLUMNS " );
      SqlUpdateQuery.AppendLine ( " WHERE  ReportId = '" + ReportTemplate.LastReportId + "' ; " );

      // 
      // Iterate through each newField in the activity list for
      // Updating those activities that have changed.
      // 
      foreach ( EdReportColumn column in ReportTemplate.Columns )
      {
        this.LogDebug  ( "ColumnId: " + column.ColumnId );

        if ( column.HeaderText == String.Empty )
        {
          this.LogDebug  ( "CONTINUE: Header test empty" );
          continue;
        }

        //
        // set the selection list for sorting in SQL
        //
        if ( column.SectionLvl == 0 )
        {
          column.SectionLvl = 99;
        }

        SqlUpdateQuery.AppendLine ( "/****  INSERT TEMPLATE COLUMNS " + column.ColumnId + " ****/" );
        SqlUpdateQuery.AppendLine ( "Insert Into EV_REPORT_TEMPLATE_COLUMNS " );
        SqlUpdateQuery.AppendLine ( " (ReportId, " );
        SqlUpdateQuery.AppendLine ( "  RTSC_COLUMN_ID, " );
        SqlUpdateQuery.AppendLine ( "  RTC_ORDER, " );
        SqlUpdateQuery.AppendLine ( "  RTC_HEADER_TEXT, " );
        SqlUpdateQuery.AppendLine ( "  RTC_SOURCE_FIELD, " );
        SqlUpdateQuery.AppendLine ( "  RTC_STYLE_WIDTH, " );
        SqlUpdateQuery.AppendLine ( "  RTC_DATA_TYPE, " );
        SqlUpdateQuery.AppendLine ( "  RTC_VALUE_FORMATING, " );
        SqlUpdateQuery.AppendLine ( "  RTC_VALUE_GROUPING_INDEX, " );
        SqlUpdateQuery.AppendLine ( "  RTC_VALUE_GROUPING_TYPE, " );
        SqlUpdateQuery.AppendLine ( "  RTC_VALUE_SECTION_LEVEL )" );
        SqlUpdateQuery.AppendLine ( "values " );
        SqlUpdateQuery.AppendLine ( " ('" + ReportTemplate.ReportId + "', " );
        SqlUpdateQuery.AppendLine ( "  '" + column.ColumnId + "', " );
        SqlUpdateQuery.AppendLine ( "   " + column.SourceOrder + ", " );
        SqlUpdateQuery.AppendLine ( "  '" + column.HeaderText + "', " );
        SqlUpdateQuery.AppendLine ( "  '" + column.SourceField + "', " );
        SqlUpdateQuery.AppendLine ( "  '" + column.StyleWidth + "', " );
        SqlUpdateQuery.AppendLine ( "  '" + column.DataType + "', " );
        SqlUpdateQuery.AppendLine ( "  '" + column.ValueFormatingString + "', " );
        SqlUpdateQuery.AppendLine ( "  '" + column.GroupingIndex + "', " );
        SqlUpdateQuery.AppendLine ( "  '" + column.GroupingType + "', " );
        SqlUpdateQuery.AppendLine ( "  '" + column.SectionLvl + "'); " );

      }//END iteration loop.

      this.LogDebug  ( "Sql Query: " + SqlUpdateQuery.ToString ( ) );

      if ( EvSqlMethods.QueryUpdate ( SqlUpdateQuery.ToString ( ), null ) == 0
        && ReportTemplate.Queries.Length > 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      return EvEventCodes.Ok;

    }//END updateTemplateColumns method

    #endregion


  }//END EvReportTemplates class

}//END namespace Evado.Dal.Digital
