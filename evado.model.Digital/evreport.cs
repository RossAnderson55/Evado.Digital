/***************************************************************************************
 * <copyright file="model\EvReport.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvReport data object.
 *
 ****************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

namespace Evado.Model.Digital
{
  /// <summary>
  /// data  entity used to model accounts
  /// </summary>
  [Serializable]
  public class EvReport
  {
    #region Class initialisation

    // =====================================================================================
    /// <summary>
    ///  Default class initialisation
    ///  
    /// </summary>
    //  ----------------------------------------------------------------------------------
    public EvReport ( )
    {
      initializeClassMembers ( );

    }

    /// <summary>
    /// Initializes some class members.
    /// AFC 30 oct 2009
    /// </summary>
    public void initializeClassMembers ( )
    {
      this._Queries = new EvReportQuery [ 5 ];
      // 
      // Initialise teh query array.
      // 
      for ( int Count = 0; Count < this._Queries.Length; Count++ )
      {
        this._Queries [ Count ] = new EvReportQuery ( );
      }


      //AFC 30 Oct 2009
      //Initialize the layout types. All of them will be flat for now
      //
      for ( int count = 0; count < this._LayoutType.Length; count++ )
      {
        this._LayoutType [ count ] = LayoutTypes.Flat;
      }
    }//END EvReport method.

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Report class enumerators.
    /// <summary>
    /// This enumeration list defines type codes of report
    /// </summary>
    public enum ReportTypeCode
    {
      /// <summary>
      /// This enumeration defines null value or not selection state
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines a general type code of report
      /// </summary>
      General,

      /// <summary>
      /// This enumeration defines a budget type code of report
      /// </summary>
      Budget,

      /// <summary>
      /// This enumeration defines a schedule type code of report
      /// </summary>
      Schedule,

      /// <summary>
      /// This enumeration defines a clinical type code of report
      /// </summary>
      Clinical,

      /// <summary>
      /// This enumeration defines a billing type code of report
      /// </summary>
      Billing,

      /// <summary>
      /// This enumeration defines a validation type code of report
      /// </summary>
      Monitoring,

      /// <summary>
      /// This enumeration defines a data management type code of report
      /// </summary>
      Data_Management,

      /// <summary>
      /// This enumeration defines a validation type code of report
      /// </summary>
      Validation,
    }

    /// <summary>
    /// This enumeration list defins the scope of the report.  
    /// This property is used to associate a report with a page for 
    /// listing or display.
    /// </summary>
    public enum ReportScopeTypes
    {
      /// <summary>
      /// This enumeration defines null value or not selection state.
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines a generic type of report scope
      /// </summary>
      Operational_Reports,

      /// <summary>
      /// This enumeration defines a clinical code of report
      /// </summary>
      Monitoring_Reports,

      /// <summary>
      /// This enumeration defines a data management report scope
      /// </summary>
      Data_Management_Reports,

      /// <summary>
      /// This enumeration defines a billing summary type of report scope
      /// </summary>
      Site_Reports,
      /*
      /// <summary>
      /// This enumeration defines a billing summary type of report scope
      /// </summary>
     // Finance_Reports,

      /// <summary>
      /// This enumeration defines a budget type of report scope
      /// </summary>
      //Budget,

      /// <summary>
      /// This enumeration defines a billing summary type of report scope
      /// </summary>
      //Billing_Summary,

      /// <summary>
      /// This enumeration defines a billing detailed type of report scope
      /// </summary>
      //Billing_Detailed,
      */
      /// <summary>
      /// This enumeration defines a subject calendar of report scope
      /// </summary>
      Subject_Calendar,

      /// <summary>
      /// This enumeration defines a subject calendar of report scope
      /// </summary>
      SAE_Correlation,
    }

    /// <summary>
    /// This enumeration list defines codes of layout type
    /// </summary>
    public enum LayoutTypeCode
    {
      /// <summary>
      /// This enumeration defines a flat table layout type
      /// </summary>
      FlatTable = 0,

      /// <summary>
      /// This enumeration defines a grouped table layout type
      /// </summary>
      GroupedTable = 1,

      /// <summary>
      /// This enumeration defines a virtual column layout type
      /// </summary>
      VirtualColumn = 2,

      /// <summary>
      /// This enumeration defines a horizontal column layout type
      /// </summary>
      HorizontalColumn = 3,

      /// <summary>
      /// This enumeration defines a pie chart layout type
      /// </summary>
      PieChart = 4,
    }

    /// <summary>
    /// This enumeration list defines code of report source
    /// </summary>
    public enum ReportSourceCode
    {
      /// <summary>
      /// This enumeration defines null value or not selection state
      /// </summary>
      Null = 0,

      /// <summary>
      /// This enumeration defines an sql query as a report source
      /// </summary>
      SqlQuery,

      /// <summary>
      /// This enumeration defines a class as a report source
      /// </summary>
      Class,

      /// <summary>
      /// This enumeration defines a protocol violation as a report source
      /// </summary>
      ProtocolViolations,

      /// <summary>
      /// This enumeration defines a form field as a report source
      /// </summary>
      FormFields,

      /// <summary>
      /// This enumeration defines a common form field as a report source
      /// </summary>
      CommonFormFields,

      /// <summary>
      /// This enumeration defines a common form field as a report source
      /// </summary>
      Field_Monitoring_Query,

      /// <summary>
      /// This enumeration defines the subject demographics.
      /// </summary>
      Subject_Demographics,

      /// <summary>
      /// This enumeration defines the subject demographics.
      /// </summary>
      Subject_Record_Status,
    }

    /// <summary>
    /// This enumeration list defines data types
    /// </summary>
    public enum DataTypes
    {
      /// <summary>
      /// This enumeration defines a text data type
      /// </summary>
      Text = 0,

      /// <summary>
      /// This enumeration defines a float data type
      /// </summary>
      Float = 1,

      /// <summary>
      /// This enumeration defines a currency data type
      /// </summary>
      Currency = 2,

      /// <summary>
      /// This enumeration defines an integer data type
      /// </summary>
      Integer = 3,

      /// <summary>
      /// This enumeration defines a boolean data type
      /// </summary>
      Bool = 4,

      /// <summary>
      /// This enumeration defines a date data type
      /// </summary>
      Date = 5,

      /// <summary>
      /// This enumeration defines a hidden data type
      /// </summary>
      Hidden = 6,

      /// <summary>
      /// This enumeration defines a guid data type
      /// </summary>
      Guid = 7,

      /// <summary>
      /// This enumeration defines a percent data type
      /// </summary>
      Percent = 8
    }

    /// <summary>
    /// This enumeration list defines grouping types
    /// </summary>
    public enum GroupingTypes
    {
      /// <summary>
      /// This enumeration defines null value or not selection state
      /// </summary>
      None = 0,

      /// <summary>
      /// This enumeration defines a total grouping type
      /// </summary>
      Total = 1,

      /// <summary>
      /// This enumeration defines a minimun grouping type
      /// </summary>
      Minimum = 2,

      /// <summary>
      /// This enumeration defines a maximun grouping type
      /// </summary>
      Maximum = 3,

      /// <summary>
      /// This enumeration defines a count grouping type
      /// </summary>
      Count = 4
    }

    /// <summary>
    /// This enumeration list defines layout types
    /// </summary>
    public enum LayoutTypes
    {
      /// <summary>
      /// This enumeration defines a tabular layout type
      /// </summary>
      Tabular = 0,

      /// <summary>
      /// This enumeration defines a flat layout type
      /// </summary>
      Flat = 1,
    }

    /// <summary>
    /// This enumeration list defines types of selection list
    /// </summary>
    public enum SelectionListTypes
    {
      /// <summary>
      /// This enumeration defines null value or not selection state
      /// </summary>
      None = 0,

      /// <summary>
      /// This enumeration defines subject identifier type of selection list
      /// </summary>
      Subject_Id = 1,

      /// <summary>
      /// This enumeration defines site identifier type of selection list
      /// </summary>
      Site_Id = 2,

      /// <summary>
      /// This enumeration defines sex type of selection list
      /// </summary>
      Sex = 3,

      /// <summary>
      /// This enumeration defines status type of selection list
      /// </summary>
      Status = 4,

      /// <summary>
      /// This enumeration defines trial identifier type of selection list
      /// </summary>Project_Id
      Trial_Id = 5,

      /// <summary>
      /// This enumeration defines arm index type of selection list
      /// </summary>
      ScheduleId = 6,

      /// <summary>
      /// This enumeration defines all trial sites type of selection list
      /// </summary>
      All_Trial_Sites = 7,

      /// <summary>
      /// This enumeration defines a current trial type of selection list
      /// </summary>
      Current_Trial = 8,

      /// <summary>
      /// This enumeration defines a form identifier type of selection list
      /// </summary>
      Form_Id = 9,

      /// <summary>
      /// This enumeration defines a common form identifier type of selection list
      /// </summary>
      Common_Form_Id = 10,

      /// <summary>
      /// This enumeration defines a form field identifier type of selection list
      /// </summary>
      Form_Field_Id = 11,

      ///
      /// This property defines the record state type of selection list.
      ///
      Record_State = 12,

      // DPRECIATED ENUMERATIONS.

      /// <summary>
      /// This enumeration defines trial identifier type of selection list
      /// </summary>Project_Id
      Project_Id = 5,

      /// <summary>
      /// This enumeration defines all trial sites type of selection list
      /// </summary>
      All_Project_Sites = 7,

      /// <summary>
      /// This enumeration defines a current trial type of selection list
      /// </summary>
      Current_Project = 8,
    }

    /// <summary>
    /// This enumeration list defines state of operator
    /// </summary>
    public enum Operators
    {
      /// <summary>
      /// This enumeration defines an equal to state of operator
      /// </summary>
      Equals_to = 0,

      /// <summary>
      /// This enumeration defines a less than state of operator
      /// </summary>
      Less_than = 1,

      /// <summary>
      /// This enumeration defines a greater than state of operator
      /// </summary>
      Greater_than = 2
    }
    /// <summary>
    /// This enumeration list defines the report filenames for data update or extraction.
    /// </summary>
    public enum ReportClassFieldNames
    {
      /// <summary>
      /// This enumeration defines the Null Value or no selection state
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines the source identifier of report field names.
      /// </summary>
      SourceId,

      /// <summary>
      /// This enumeration defines the project identifier of report field names.
      /// </summary>
      ProjectId,

      /// <summary>
      /// This enumeration defines the report identifier of report field names.
      /// </summary>
      ReportId,

      /// <summary>
      /// This enumeration defines the report title of report field names.
      /// </summary>
      ReportTitle,

      /// <summary>
      /// This enumeration defines the report sub title of report field names..
      /// </summary>
      ReportSubTitle,

      /// <summary>
      /// This enumeration defines the report description of report field names..
      /// </summary>
      Description,

      /// <summary>
      /// This enumeration defines the report type of report field names.
      /// </summary>
      ReportType,

      /// <summary>
      /// This enumeration defines the report type of report field names.
      /// </summary>
      Category,

      /// <summary>
      /// This enumeration defines the report type of report field names.
      /// </summary>
      ReportScope,

      /// <summary>
      /// This enumeration defines the report form list of report field names.
      /// </summary>
      DataSourceId,

      /// <summary>
      /// This enumeration defines the report form list of report Validation names.
      /// </summary>
      SqlDataSource,

      /// <summary>
      /// This enumeration defines the report form list of report Validation names.
      /// </summary>
      LayoutTypeId,

      /// <summary>
      /// This enumeration defines the report type of report field names.
      /// </summary>
      IsAggregated,
    }

    #endregion

    #region Member variables

    /// <summary>
    /// The reports global unique identifier.  Used to identify the report template in the database.
    /// 
    /// </summary>
    private Guid _Guid = Guid.Empty;

    /// <summary>
    /// The report number identifies are report (with data) that has been saved to the database.
    /// 
    /// </summary>
    private int _ReportNo = 0;

    /// <summary>
    /// Report source identifier reports. 
    /// 
    /// </summary>
    private string _SourceId = String.Empty;

    /// <summary>
    /// Report category used to filter different reports. 
    /// 
    /// </summary>
    private string _Category = String.Empty;

    /// <summary>
    /// The trial identifier definine the type of report that will be generated.
    /// 
    /// </summary>
    private string _TrialId = String.Empty;


    /// <summary>
    /// The report identifier definine the type of report that will be generated.
    /// 
    /// </summary>
    private string _ReportId = String.Empty;

    /// <summary>
    ///  The titlle of the report.
    /// 
    /// </summary>
    private string _ReportTitle = String.Empty;

    /// <summary>
    /// The sub title of the report.
    /// 
    /// 
    /// </summary>
    private string _ReportSubTitle = String.Empty;

    /// <summary>
    /// The name of the person that generated the report i.e. ran the report.
    /// 
    /// </summary>
    private string _GeneratedBy = String.Empty;

    /// <summary>
    /// The date the report was generated.
    /// 
    /// 
    /// </summary>
    private DateTime _ReportDate = DateTime.Now;

    /// <summary>
    /// The data source identifier code see enumerator for options.
    /// 
    /// </summary>
    private ReportSourceCode _DataSourceId = ReportSourceCode.Null;

    /// <summary>
    /// The data source query of the source is Sql.
    /// The class name of the source if the datasourceid is Class.
    /// 
    /// </summary>
    private string _SqlDataSource = String.Empty;

    /// <summary>
    /// The name of the person that generated the report i.e. ran the report.
    /// 
    /// </summary>
    private int _NoColumns = 0;

    /// <summary>
    /// The report type identifier definine the type of report that will be generated.
    /// See the ennumerator for options.
    /// 
    /// </summary>
    private ReportTypeCode _ReportType = ReportTypeCode.General;

    /// <summary>
    /// The report layout type identifier. i.e. is the report tabular.
    /// The initial implementation will only be tabular other options will be 
    /// added at a later stage.
    /// 
    /// </summary>
    private LayoutTypeCode _LayoutTypeId = LayoutTypeCode.FlatTable;


    /// <summary>
    /// Define the visibility of the report. If the scope is generic, this
    /// report will be visible in the standar report selection form. Otherwise
    /// it will just be visible from a specific place. For example, budget report
    /// should be only visible from the Budget generation page.
    /// </summary>
    private ReportScopeTypes _ReportScopeType = ReportScopeTypes.Operational_Reports;

    /// <summary>
    /// The version identifies the layout version of the report, this is to be 
    /// incremented if the report layout is updated.
    /// 
    /// </summary>
    private int _Version = 0;
    /*
    /// <summary>
    /// The GroupingGroups defines the number of grouping Groups in the report.
    /// </summary>
    //private GroupCode _Group = GroupCode.NoGrouping;
    */

    /// <summary>
    /// The UpdatedByUserId is the user id of the user that is saving the report
    /// This variable is only used to pass the users name to the report BLL and DAL layers.
    /// 
    /// </summary>
    private string _UpdateUserId = String.Empty;


    /// <summary>
    /// The UpdatedBy is the name of the user that is saving the report
    /// This variable is only used to pass the users name to the report BLL and DAL layers.
    /// 
    /// </summary>
    private string _UserCommonName = String.Empty;

    /// <summary>
    ///  The State contains debug status of the class.
    /// 
    /// </summary>
    private System.Text.StringBuilder _Log = new System.Text.StringBuilder ( );

    /// <summary>
    /// This property returns the object log content.
    /// </summary>
    public String Log
    {
      get
      {
        return this._Log.ToString ( );
      }
    }

    /// <summary>
    ///  The Update contains update information for the UI layer.
    /// 
    /// </summary>
    private string _Updated = String.Empty;

    /// <summary>
    /// This member defines the layout of the group.
    /// The default is flat.
    /// 
    /// </summary>
    private LayoutTypes [ ] _LayoutType = new LayoutTypes [ 5 ];


    /// <summary>
    /// The Queries contains an array of Query objects, the objects define the attributes for report queries.
    /// 
    /// </summary>
    private EvReportQuery [ ] _Queries = new EvReportQuery [ 5 ];

    /// <summary>
    /// The Columns contain an array of Column objects, the column class defines the attributes of each data column in 
    /// the report data set and how that column is to be used in the report.
    /// 
    /// </summary>
    private List<EvReportColumn> _Columns = new List<EvReportColumn> ( );

    /// <summary>
    ///  The DataRecords contains an array of ReportRow objects, each report row object contain one row of report data.
    /// 
    /// </summary>
    private List<EvReportRow> _DataRecords = new List<EvReportRow> ( );

    /// <summary>
    /// This member records whether this is the first instance of the group.  In which
    /// case there is no need to insert a group footer.
    /// 
    /// </summary>
    private string [ ] _CurrentColumnValue = new string [ 5 ];

    /// <summary>
    /// True if the boolean columns should allow selection.
    /// </summary>
    private bool _SelectionOn = false;

    /// <summary>
    /// The detail of the report should be ordered by this column.
    /// </summary>
    private String _DetailIndexName;

    /// <summary>
    /// Description of the saved report.
    /// </summary>
    private String _Description;

    /// <summary>
    /// True if this is an aggregated report
    /// </summary>
    private bool _IsAggregated = false;

    /// <summary>
    /// Contains the header text for the site column.
    /// This value is used to filter the details for the sites.
    /// If this value is empty, means that no filter is requiered.
    /// </summary>
    private String _SiteHeaderText;

    /// <summary>
    /// Contains the software version the report was save at.
    /// </summary>
    private String _SoftwareVersion = String.Empty;

    /// <summary>
    /// True if this report requiere
    /// </summary>
    private bool _RequireUserTrial;

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class Properties
    /// <summary>
    /// This property contains a global unique identifier of a report
    /// </summary>
    public Guid Guid
    {
      get
      {
        return this._Guid;
      }
      set
      {
        this._Guid = value;
      }
    }

    /// <summary>
    /// This property contains a data source identifier of a report
    /// </summary>
    public String SourceId
    {
      get
      {
        return this._SourceId;
      }
      set
      {
        this._SourceId = value;
      }
    }

    /// <summary>
    /// This property indicates whether a report is aggregated
    /// </summary>
    public bool IsAggregated
    {
      get { return this._IsAggregated; }
      set { this._IsAggregated = value; }
    }

    /// <summary>
    /// This property contains a site column header text of a report
    /// </summary>
    public String SiteColumnHeaderText
    {
      get { return this._SiteHeaderText; }
      set { this._SiteHeaderText = value; }
    }

    /// <summary>
    /// This property indicates whether a report is user site filtered
    /// </summary>
    public bool IsUserSiteFiltered
    {
      get { return this._SiteHeaderText != String.Empty; }
    }

    /// <summary>
    /// This property contains a number of a report
    /// </summary>
    public int ReportNo
    {
      get
      {
        return this._ReportNo;
      }
      set
      {
        this._ReportNo = value;
      }
    }

    /// <summary>
    /// This property contains a report scope type object of a report
    /// </summary>
    public ReportScopeTypes ReportScope
    {
      get { return this._ReportScopeType; }
      set { this._ReportScopeType = value; }
    }

    /// <summary>
    /// This property contains a string number of a report
    /// </summary>
    public string stReportNo
    {
      get
      {
        return this._ReportNo.ToString ( "000000" );
      }
    }

    /// <summary>
    /// This property contains a category of a report
    /// </summary>
    public string Category
    {
      get
      {
        return this._Category;
      }
      set
      {
        this._Category = value;
      }
    }

    /// <summary>
    /// This property contains a trial identifier of a report
    /// </summary>
    public string TrialId
    {
      get
      {
        return this._TrialId;
      }
      set
      {
        this._TrialId = value;
      }
    }

    /// <summary>
    /// This property contains a report identifier of a report
    /// </summary>
    public string ReportId
    {
      get
      {
        return this._ReportId;
      }
      set
      {
        this._ReportId = value;
      }
    }

    /// <summary>
    /// This property contains a last report identifier of a report
    /// </summary>
    public string LastReportId { get; set; }

    /// <summary>
    /// This property contains a report title of a report
    /// </summary>
    public string ReportTitle
    {
      get
      {
        return this._ReportTitle;
      }
      set
      {
        this._ReportTitle = value;
      }
    }

    /// <summary>
    /// This property contains a report sub title of a report
    /// </summary>
    public string ReportSubTitle
    {
      get
      {
        return this._ReportSubTitle;
      }
      set
      {
        this._ReportSubTitle = value;
      }
    }

    /// <summary>
    /// This property contains a user who generates a report
    /// </summary>
    public string GeneratedBy
    {
      get
      {
        return this._GeneratedBy;
      }
      set
      {
        this._GeneratedBy = value;
      }
    }

    /// <summary>
    /// This property contains a generated date of a report
    /// </summary>
    public DateTime ReportDate
    {
      get
      {
        return this._ReportDate;
      }
      set
      {
        this._ReportDate = value;
      }
    }

    /// <summary>
    /// This property contains a generated date string of a report
    /// </summary>
    public string stReportDate
    {
      get
      {
        if ( this._ReportDate == EvcStatics.CONST_DATE_NULL || this._ReportDate == null )
        {
          this._ReportDate = DateTime.Now;
        }
        return this._ReportDate.ToString ( "dd MMM yyyy" );
      }
      set
      {
        if ( value == String.Empty )
        {
          this._ReportDate = EvcStatics.CONST_DATE_NULL;
          return;
        }
        DateTime date = this._ReportDate;

        if ( DateTime.TryParse ( value, out date ) == true )
        {
          this._ReportDate = date;
        }
      }
    }

    /// <summary>
    /// This property contains a data source identifier of a report
    /// </summary>
    public ReportSourceCode DataSourceId
    {
      get
      {
        return this._DataSourceId;
      }
      set
      {
        this._DataSourceId = value;
      }
    }

    /// <summary>
    /// This property contains an sql data source of a report
    /// </summary>
    public string SqlDataSource
    {
      get
      {
        return this._SqlDataSource;
      }
      set
      {
        this._SqlDataSource = value;
      }
    }

    /// <summary>
    /// This property contains a report type identifier of a report
    /// </summary>
    public ReportTypeCode ReportType
    {
      get
      {
        return this._ReportType;
      }
      set
      {
        this._ReportType = value;
      }
    }

    /// <summary>
    /// This property contains a layout type identifier object of a report
    /// </summary>
    public LayoutTypeCode LayoutTypeId
    {
      get
      {
        return this._LayoutTypeId;
      }
      set
      {
        this._LayoutTypeId = value;
      }
    }

    /// <summary>
    /// This property contains a version of a report
    /// </summary>
    public int Version
    {
      get
      {
        return this._Version;
      }
      set
      {
        this._Version = value;
      }
    }

    /// <summary>
    /// This property contains an update user identifier of a report
    /// </summary>
    public string UpdateUserId
    {
      get
      {
        return this._UpdateUserId;
      }
      set
      {
        this._UpdateUserId = value;
      }
    }

    /// <summary>
    /// This property contains a user common name who updates a report
    /// </summary>
    public string UserCommonName
    {
      get
      {
        return this._UserCommonName;
      }
      set
      {
        this._UserCommonName = value;
      }
    }

    /// <summary>
    /// This property contains an updated string of a report
    /// </summary>
    public string Updated
    {
      get
      {
        return this._Updated;
      }
      set
      {
        this._Updated = value;
      }
    }

    //
    //TODO remove this. It is not used, but I don't know what will happen with the actual xml reports.
    //
    /// <summary>
    /// This property contains a source string of a report
    /// </summary>
    public string Source
    {
      get
      {
        return "";
      }
      set
      {
        string dVar = value;
      }
    }

    /// <summary>
    /// This property contains column numbers of a report
    /// </summary>
    public int NoColumns
    {
      get
      {
        return this._NoColumns;
      }
    }

    /// <summary>
    /// This property contains a query array object of a report
    /// </summary>
    public EvReportQuery [ ] Queries
    {
      get
      {
        return this._Queries;
      }
      set
      {
        this._Queries = value;
      }
    }

    /// <summary>
    /// This property contains a column object list of a report
    /// </summary>
    public List<EvReportColumn> Columns
    {
      get
      {
        return this._Columns;
      }
      set
      {
        this._Columns = value;
        this._NoColumns = Columns.Count;
      }
    }

    /// <summary>
    /// This property contains a datarecord object list of a report
    /// </summary>
    public List<EvReportRow> DataRecords
    {
      get
      {
        return this._DataRecords;
      }
      set
      {
        this._DataRecords = value;
      }
    }

    /// <summary>
    /// This property contains a global unique identifier of a report
    /// </summary>
    public bool SelectionOn
    {
      get
      {
        return this._SelectionOn;
      }
      set
      {
        this._SelectionOn = value;
      }
    }

    /// <summary>
    /// This property indicates whether a report is detail index name
    /// </summary>
    public String DetailIndexName
    {
      get { return this._DetailIndexName; }
      set { this._DetailIndexName = value; }
    }

    /// <summary>
    /// This property contains description of a report
    /// </summary>
    public String Description
    {
      get { return this._Description; }
      set { this._Description = value; }
    }

    /// <summary>
    /// This property contains details of a report
    /// </summary>
    public string Details
    {
      get
      {
        return this._ReportTitle + " - "
          + ( this._ReportSubTitle != null && this._ReportSubTitle != string.Empty ? this._ReportSubTitle + " - " : "" )
          + Enum.GetName ( typeof ( EvReport.ReportTypeCode ), this._ReportType ) + " - "
          + this._Description;
      }
    }

    /// <summary>
    /// This property indicates whether a report require user trial
    /// </summary>
    public bool RequireUserTrial
    {
      get { return _RequireUserTrial; }
      set { _RequireUserTrial = value; }
    }

    /// <summary>
    /// This property contains a software version of a report
    /// </summary>
    public String SoftwareVersion
    {
      get { return this._SoftwareVersion; }
      set { this._SoftwareVersion = value; }
    }

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class Public Methods

    // ===================================================================================
    /// <summary>
    /// This method adda a new column to the list of columents.
    /// </summary>
    /// <param name="KeyValue">String: key value</param>
    /// <param name="Value">String value</param>
    /// <returns>True: successfully added, False: not added.</returns>
    //  ----------------------------------------------------------------------------------
    public bool updateQuery ( String KeyValue, String Value )
    {
      for ( int i = 0; i < this._Queries.Length; i++ )
      {
        if ( this._Queries [ i ].QueryId == KeyValue
          || this._Queries [ i ].FieldName == KeyValue )
        {
          this._Queries [ i ].Value = Value;

          return true;
        }

      }
        return false;
    }

    // ===================================================================================
    /// <summary>
    /// This method adda a new column to the list of columents.
    /// </summary>
    /// <param name="NewColumn">EvReportColum object to be added</param>
    /// <returns>True: successfully added, False: not added.</returns>
    //  ----------------------------------------------------------------------------------
    public bool addColumn ( EvReportColumn NewColumn )
    {
      foreach ( EvReportColumn column in this._Columns )
      {
        if ( column.ColumnId == NewColumn.ColumnId )
        {
          return false;
        }
      }

      //
      // Add the new column.
      //
      this._Columns.Add ( NewColumn );

      return true;
    }

    //==================================================================================
    /// <summary>
    /// This method retrieves the selected query.
    /// </summary>
    /// <param name="ColumnId">String: the column identifier.</param>
    /// <returns>EvReportQuery objects.</returns>
    //-----------------------------------------------------------------------------------
    public EvReportColumn getColumn ( String ColumnId )
    {
      //
      // exit if query list is null.
      //
      if ( this._Columns.Count == 0  )
      {
        return new EvReportColumn ( );
      }

      //
      // search the list for the matching query.
      //
      foreach ( EvReportColumn column in this.Columns )
      {
        if ( column.ColumnId == ColumnId )
        {
          return column;
        }
      }

      //
      // Returm empty objects.
      //
      return new EvReportColumn ( );

    }//END getColumn method

    // ===================================================================================
    /// <summary>
    /// This method trims empty report columns
    /// </summary>
    //  ----------------------------------------------------------------------------------
    public void trimColumns ( )
    {
      //
      // Iterate through the list of columns and remove any collumn that does not
      // have a columnId or header text.
      //
      for ( int iCount = 0; iCount < this._Columns.Count; iCount++ )
      {
        EvReportColumn column = this._Columns [ iCount ];
        if ( column.ColumnId == String.Empty
          || column.HeaderText == String.Empty )
        {
          this._Columns.RemoveAt ( iCount );
          iCount--;
        }
      }
    }//END trimColumns method

    // =====================================================================================
    /// <summary>
    /// This method initialse the report source objects within the current report object.  
    /// </summary>
    /// <param name="ReportSource">EvReportSource objects.</param>
    //  ----------------------------------------------------------------------------------
    public void initialiseReportDataSourceValues (
      EvReportSource ReportSource )
    {
      this.LogMethod ( "initialiseReportDataSourceValues method." );

      if ( this._SourceId != ReportSource.SourceId )
      {
        return;
      }

      //
      // query object iteration loop.
      //
      for ( int i = 0; i < this.Queries.Length; i++ )
      {
        EvReportQuery query = this.Queries [ i ];
        if ( query.QueryId == String.Empty )
        {
          EvReportQuery sourceQuery = ReportSource.getQueryBySelectionSource ( query.SelectionSource );
          query.QueryId = sourceQuery.QueryId;
          query.QueryTitle = sourceQuery.QueryTitle;
        }
      }//END query iteration loop.

      //
      // column object iteration loop.
      //
      for ( int i = 0; i < this.Columns.Count; i++ )
      {
        EvReportColumn column = this.Columns [ i ];
        if ( column.ColumnId == String.Empty )
        {
          EvReportColumn sourceColumn = ReportSource.getColumnBySourceField ( column.SourceField );
          column.ColumnId = sourceColumn.ColumnId;
        }
      }//END query iteration loop.

    }//ENd initialiseReportDataSourceValues method

    // =====================================================================================
    /// <summary>
    /// This method takes an arraylist of DataRecord objects and 
    /// imports them into the report object as an array of EvReportRow objects.
    ///  
    /// </summary>
    /// <param name="DataRecords">ArrayList: Arraylist containing EvReportRow objects.</param>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Demension the report rows.
    /// 
    /// 2. Process a flat report
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public void setDataRecords ( ArrayList DataRecords )
    {
      this.LogMethod ( "setDataRecords method." ); 
      // 
      // Demension the report rows.
      // 
      this.DataRecords = new List<EvReportRow> ( );

      // 
      // Process a flat report
      // 
      foreach ( EvReportRow row in DataRecords )
      {
        this.DataRecords.Add ( row );

      }//END processing interation loop

    }//END setDataRecords method

    //  ================================================================================
    /// <summary>
    /// Sets the value of this report class field name. Validate the format of the
    /// value. 
    /// </summary>
    /// <param name="fieldName">ActivityClassFieldNames: Name of the field to be setted.</param>
    /// <param name="value">String: value to be setted</param>
    /// <returns>EvEventCodes: indicating the successful update of the property value.</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Switch the fieldName and update value for the property defined by the report field names
    /// 
    /// 2. Return casting error, if field name type is empty
    /// </remarks>
    //  --------------------------------------------------------------------------------
    public EvEventCodes setValue ( ReportClassFieldNames fieldName, String value )
    {
      //
      // Switch the FieldName based on the report field names
      //
      switch ( fieldName )
      {
        case ReportClassFieldNames.SourceId:
          {
            this.SourceId = value;
            break;
          }
        case ReportClassFieldNames.ProjectId:
          {
            this.TrialId = value;
            break;
          }
        case ReportClassFieldNames.ReportId:
          {
            this.ReportId = value;
            break;
          }
        case ReportClassFieldNames.ReportTitle:
          {
            this.ReportTitle = value;
            break;
          }
        case ReportClassFieldNames.ReportSubTitle:
          {
            this.ReportSubTitle = value;
            break;
          }
        case ReportClassFieldNames.Description:
          {
            this.Description = value;
            break;
          }
        case ReportClassFieldNames.Category:
          {
            this.Category = value;
            break;
          }
        //
        // If Field Name type does not exist, return casting error
        //
        case ReportClassFieldNames.ReportType:
          {
            ReportTypeCode type = ReportTypeCode.General;
            if ( EvcStatics.Enumerations.tryParseEnumValue<ReportTypeCode> ( value, out type ) == false )
            {
              return EvEventCodes.Data_Enumeration_Casting_Error;
            }
            this.ReportType = type;
            break;
          }

        //
        // If Field Name type does not exist, return casting error
        //
        case ReportClassFieldNames.ReportScope:
          {
            ReportScopeTypes type = ReportScopeTypes.Operational_Reports;
            if ( EvcStatics.Enumerations.tryParseEnumValue<ReportScopeTypes> ( value, out type ) == false )
            {
              return EvEventCodes.Data_Enumeration_Casting_Error;
            }
            this.ReportScope = type;
            break;
          }

        //
        // If Field Name type does not exist, return casting error
        //
        case ReportClassFieldNames.DataSourceId:
          {
            ReportSourceCode type = ReportSourceCode.SqlQuery;
            if ( EvcStatics.Enumerations.tryParseEnumValue<ReportSourceCode> ( value, out type ) == false )
            {
              return EvEventCodes.Data_Enumeration_Casting_Error;
            }
            this.DataSourceId = type;
            break;
          }

        //
        // If Field Name type does not exist, return casting error
        //
        case ReportClassFieldNames.LayoutTypeId:
          {
            LayoutTypeCode type = LayoutTypeCode.FlatTable;
            if ( EvcStatics.Enumerations.tryParseEnumValue<LayoutTypeCode> ( value, out type ) == false )
            {
              return EvEventCodes.Data_Enumeration_Casting_Error;
            }
            this.LayoutTypeId = type;
            break;
          }

        case ReportClassFieldNames.IsAggregated:
          {
            this._IsAggregated = Evado.Model.EvStatics.getBool ( value );
            break;
          }

        case ReportClassFieldNames.SqlDataSource:
          {
            this.SqlDataSource = value;
            break;
          }
      }// End switch field name

      return EvEventCodes.Ok;

    }//End setValue method.

    #endregion

    #region Report Html formatting methods

    // =====================================================================================
    /// <summary>
    ///  This method returns the report formatted for output as html.
    ///  and calls The report generator to perform the Formating of the output.
    /// </summary>
    /// <param name="profile">EvUserProfile: a user profile</param>
    /// <returns>string: an Html report</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Generate an Html report defining by a user profile
    /// 
    /// 2. Return a generated report
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public string getReportAsHtml ( EvUserProfile profile )
    {
      this.LogMethod ( "getReportAsHtml method. " );

      EvReportGenerator report = new EvReportHtml ( this, profile );

      string stHtml = report.getReportText ( );

      this.LogDebug ( report.DebugLog );

      this.LogMethodEnd ( "getReportAsCsv" );
      return stHtml;

    }//END getReportAsHtml method

    // =====================================================================================
    /// <summary>
    ///  This method returns the report formatted for output as Csv.
    ///  and calls The report generator to perform the Formating of the output.
    /// </summary>
    /// <param name="separator">String separator</param>
    /// <param name="profile">EvUserProfile: a user profile</param>
    /// <returns>string: a csv report</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize a report format: status, title, layout, column number
    /// 
    /// 2. Generate a report in csv format defining by a report format, separator 
    /// and user profile.
    /// 
    /// 3. Return a generated report
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public string getReportAsCsv (
      String separator,
      EvUserProfile profile )
    {
      this.LogMethod ( "getReportAsCsv method " );
      this.LogDebugValue( "Title: " + this.ReportTitle);
      this.LogDebugValue( "Report Type: " + this.LayoutTypeId);
      this.LogDebugValue( "Header Columns: " + this.Columns.Count );

      EvReportGenerator report = new EvReportCsv ( this, separator, profile );

      var value = report.getReportText ( );

      this.LogDebug ( report.DebugLog );
      this.LogMethodEnd ( "getReportAsCsv" );
      return value;

    }// END getReportAsCsv

    // =====================================================================================
    /// <summary>
    ///  This method returns the report formatted for output as Csv.
    ///  and calls The report generator to perform the Formating of the output.
    /// </summary>
    /// <param name="separator">String separator</param>
    /// <param name="profile">EvUserProfile: a user profile</param>
    /// <returns>string: a csv report</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize a report format: status, title, layout, column number
    /// 
    /// 2. Generate a report in csv format defining by a report format, separator 
    /// and user profile.
    /// 
    /// 3. Return a generated report
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public string getReportFlatAsCsv (
      String separator,
      EvUserProfile profile )
    {
      this.LogMethod ( "getReportFlatAsCsv method " );
      this.LogDebugValue ( "Title: " + this.ReportTitle );
      this.LogDebugValue ( "Report Type: " + this.LayoutTypeId );
      this.LogDebugValue ( "Header Columns: " + this.Columns.Count );

      EvReportGenerator report = new EvReportCsv ( this, separator, profile );

      var value = report.getReportData ( );

      this.LogDebug ( report.DebugLog );
      this.LogMethodEnd ( "getReportFlatAsCsv" );

      return value;

    }// END getReportAsCsv    
    // =====================================================================================
    /// <summary>
    ///  This method returns the report formatted for output as Csv.
    ///  and calls The report generator to perform the Formating of the output.
    /// </summary>
    /// <param name="separator">String separator</param>
    /// <param name="profile">EvUserProfile: a user profile</param>
    /// <returns>string: a csv report</returns>
    //  ----------------------------------------------------------------------------------
    public string getResultAsCsv (
      String separator,
      EvUserProfile profile )
    {
      this.LogMethod ( "getResultAsCsv method " );
      //
      // Initialise local variables.
      //
      System.Text.StringBuilder outputData = new System.Text.StringBuilder ( );

      //
      // Create the output header.
      //
      string stColumn = String.Empty;
      foreach ( EvReportColumn column in this._Columns )
      {
        if ( stColumn != String.Empty )
        {
          stColumn += ",";
        }
        stColumn += "\"" + column.HeaderText + "\"";
      }

      outputData.AppendLine ( stColumn );

      //
      // Iterate through the data rows.
      //
      foreach ( EvReportRow dataRow in this._DataRecords )
      {
        //
        // Iterate through the values in data row.
        //
        string stValue = String.Empty;
        foreach ( string value in dataRow.ColumnValues )
        {
          if ( stColumn != String.Empty )
          {
            stValue += ",";
          }
          stValue += "\"" + value + "\"";
        }

        outputData.AppendLine ( stValue );
      }

      //
      // output the result set.
      //
      return outputData.ToString ( );

    }// END getResultAsCsv    
    #endregion

    #region Static methods

    // =====================================================================================
    /// <summary>
    /// This method initialse the report source objects within the current report object.  
    /// </summary>
    /// <returns>List of EvOption objects</returns>
    //  ----------------------------------------------------------------------------------
    public static List<EvOption> getReportLayoutOptionList ( )
    {
      //
      // Initialise the methods variables and objects.
      //
      List<EvOption> optionList = new List<EvOption> ( );

      optionList.Add ( Evado.Model.EvStatics.Enumerations.getOption (
        EvReport.LayoutTypeCode.FlatTable.ToString ( ) ) );

      optionList.Add ( Evado.Model.EvStatics.Enumerations.getOption (
        EvReport.LayoutTypeCode.GroupedTable.ToString ( ) ) );

      return optionList;

    }//ENd getReportLayoutOptionList method

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion


    #region Debug methods.

    private const string CONST_NAME_SPACE = "Evado.Model.Digital.EvReport.";
    // ==================================================================================
    /// <summary>
    /// This method resets the debug log to empty
    /// </summary>
    // ----------------------------------------------------------------------------------
    public void resetDebugLog ( )
    {
      this._Log = new System.Text.StringBuilder ( );
    }//END resetDebugLog class

    // ==================================================================================
    /// <summary>
    /// This method appends the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="DebugLogString">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    public void LogDebugValue ( String DebugLogString )
    {
      this._Log.AppendLine (
        DateTime.Now.ToString ( "dd-MMM-yy hh:mm:ss" ) + " : " + DebugLogString );
    }//END writeDebugLogLine class
    // ==================================================================================
    /// <summary>
    /// This method appends the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="DebugLogString">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    public void LogDebug ( String DebugLogString )
    {
      this._Log.Append (
        DateTime.Now.ToString ( "dd-MMM-yy hh:mm:ss" ) + " : " + DebugLogString );
    }//END writeDebugLogLine class

    //  ==================================================================================
    /// <summary>
    /// This class writes Debug line to method status.
    /// </summary>
    //  ----------------------------------------------------------------------------------
    public void LogMethod ( String Value )
    {
      this._Log.AppendLine ( Evado.Model.Digital.EvcStatics.CONST_METHOD_START
        + DateTime.Now.ToString ( "dd-MMM-yy hh:mm:ss" ) + " : "
        + CONST_NAME_SPACE + Value );
    }//END writeDebugLine class


    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="MethodName">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogMethodEnd ( String MethodName )
    {
        String value = Evado.Model.EvStatics.CONST_METHOD_END;
        value = value.Replace ( " END OF METHOD ", " END OF " + MethodName + " METHOD " );
        this._Log.AppendLine ( value );
    }
    #endregion
  }//END EvReport class 

}//END Namespace Evado.Model.Digital
