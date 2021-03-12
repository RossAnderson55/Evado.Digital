/***************************************************************************************
 * <copyright file="EvDataItem.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2021 EVADO HOLDING PTY. LTD..  All rights reserved.
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
 *  This class contains the EvDataItem data object.
 *
 ****************************************************************************************/

using System;
using System.Collections.Generic;

namespace Evado.Model.Digital
{
  /// <summary>
  /// data  entity used to model accounts
  /// </summary>
  [Serializable]
  public class EvExportParameters
  {
    #region Initialisation methods.

    //===================================================================================
    /// <summary>
    /// This method is the default class initialisation.
    /// </summary>
    //-----------------------------------------------------------------------------------
    public EvExportParameters ( )
    {
    }

    //===================================================================================
    /// <summary>
    /// this method initialises the class with parameters.
    /// </summary>
    /// <param name="Project">Stirng project identifier</param>
    /// <param name="ExportDataSource">ExportDataSource enumeration value</param>
    /// <param name="FormId">String form identifier</param>
    //-----------------------------------------------------------------------------------
    public EvExportParameters (
      ExportDataSources ExportDataSource,
      String FormId )
    {
      this._ExportDataSource = ExportDataSource;
      this._LayoutId = FormId;
    }

    //===================================================================================
    /// <summary>
    /// this method initialises the class with parameters.
    /// </summary>
    /// <param name="Project">Stirng project identifier</param>
    /// <param name="ExportDataSource">ExportDataSource enumeration value</param>
    //-----------------------------------------------------------------------------------
    public EvExportParameters (
      ExportDataSources ExportDataSource )
    {
      this._ExportDataSource = ExportDataSource;
    }



    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Enumerated classes.

    //===================================================================================
    /// <summary>
    /// This enumerated list defines the export data sources.
    /// </summary>
    //-----------------------------------------------------------------------------------
    public enum ExportDataSources
    {
      /// <summary>
      ///  This enumeration defines the not selected or null selection state.
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines the export source to be a project record.
      /// </summary>
      Project_Record,

      /// <summary>
      /// This enumeration defines the export data source is a common record.
      /// </summary>
      Common_Record,

      /// <summary>
      /// This enumeration defines the export data source is subject demographics
      /// </summary>
      Subjects,

      /// <summary>
      /// This enumeration defines the export data source is data points
      /// </summary>
      Data_Points,
    }
    //===================================================================================
    /// <summary>
    /// This enumerated list defines the statistical output formats.
    /// </summary>
    //-----------------------------------------------------------------------------------
    public enum ExportMilestoneTypes
    {
      /// <summary>
      ///  This enumeration defines the not selected or null selection state.
      /// </summary>
      Null,

      /// <summary>
      ///  This enumeration defines the not selected or null selection state.
      /// </summary>
      All_Clinical,

      /// <summary>
      /// This enumeration defines the output in subject row format.
      /// </summary>
      Scheduled_Clinical_Milestones,

      /// <summary>
      /// This enumeration defines the output in data point row format.
      /// </summary>
      Unscheduled_Clinical_Milestones,
    }

    //===================================================================================
    /// <summary>
    /// This enumerated list defines the statistical output formats.
    /// </summary>
    //-----------------------------------------------------------------------------------
    public enum StatisticalOutputFormatCodes
    {
      /// <summary>
      ///  This enumeration defines the not selected or null selection state.
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines the output in subject row format.
      /// </summary>
      In_Subject_Row_Format,

      /// <summary>
      /// This enumeration defines the output in data point row format.
      /// </summary>
      In_Data_Point_Row_Format,

      /// <summary>
      /// This enumeration defines the output in a tabular data format.
      /// </summary>
      In_Tabular_Data_Format
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Common Export parameters.

    EdUserProfile _UserProfile = new EdUserProfile ( );
    /// <summary>
    /// This property contains a user profile object
    /// </summary>
    public EdUserProfile UserProfile
    {
      get
      {
        return _UserProfile;
      }
      set
      {
        this._UserProfile = value;
      }
    }

    private ExportDataSources _ExportDataSource = ExportDataSources.Null;

    /// <summary>
    /// This property defines the export data source.
    /// </summary>
    public ExportDataSources ExportDataSource
    {
      get
      {
        return _ExportDataSource;
      }
      set
      {
        _ExportDataSource = value;
      }
    }

    int _ScheduleId = 1;

    /// <summary>
    /// This property defines the schedule to be exported.
    /// </summary>
    public int ScheduleId
    {
      get { return _ScheduleId; }
      set { _ScheduleId = value; }
    }



    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Record Export parameters.


    string _LayoutId = String.Empty;
    /// <summary>
    /// This property contains a field identifier of the data item
    /// </summary>
    public string LayoutId
    {
      get
      {
        return _LayoutId;
      }
      set
      {
        this._LayoutId = value;
      }
    }


    bool _IncludeFreeTextData = false;
    /// <summary>
    /// This property indicated whether tests sites are selected in the export.
    /// </summary>
    public bool IncludeFreeTextData
    {
      get
      {
        return _IncludeFreeTextData;
      }
      set
      {
        this._IncludeFreeTextData = value;
      }
    }


    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Statistical Export parameters.

    bool _IncludeSubjectWithChangedArms = false;
    /// <summary>
    /// This property includes subjects that have changed trial arms.
    /// </summary>
    public bool IncludeSubjectsWithChangedSchedule
    {
      get
      {
        return _IncludeSubjectWithChangedArms;
      }
      set
      {
        _IncludeSubjectWithChangedArms = value;
      }
    }

    bool _HideTestSites = true;
    /// <summary>
    /// This property indicated whether tests sites are selecte in the export.
    /// </summary>
    public bool IncludeTestSites
    {
      get
      {
        return _HideTestSites;
      }
      set
      {
        this._HideTestSites = value;
      }
    }

    bool _IncludeDraftRecords = false;
    /// <summary>
    /// This property indicates whether submitted records are included in the export.
    /// </summary>
    public bool IncludeDraftRecords
    {
      get
      {
        return _IncludeDraftRecords;
      }
      set
      {
        this._IncludeDraftRecords = value;
      }
    }

    ExportMilestoneTypes _ExportMilestoneType = ExportMilestoneTypes.All_Clinical;
    /// <summary>
    /// This property defines the vist selection type.
    /// </summary>
    public ExportMilestoneTypes ExportMilestoneType
    {
      get
      {
        return this._ExportMilestoneType;
      }
      set
      {
        this._ExportMilestoneType = value;
      }
    }

    StatisticalOutputFormatCodes _OutputFormat = StatisticalOutputFormatCodes.In_Subject_Row_Format;
    /// <summary>
    /// This property defines the statistical output format.
    /// </summary>
    public StatisticalOutputFormatCodes OutputFormat
    {
      get
      {
        return this._OutputFormat;
      }
      set
      {
        this._OutputFormat = value;
      }
    }

    private String _OutputDirectoryPath = String.Empty;
    /// <summary>
    /// This property contains the directory path to save the output files.
    /// </summary>
    public String OutputDirectoryPath
    {
      get
      {
        return this._OutputDirectoryPath;
      }
      set
      {
        this._OutputDirectoryPath = value;
      }
    }

    private String _OutputDirectoryUrl = String.Empty;
    /// <summary>
    /// This property contains the URL path to save the output files.
    /// </summary>
    public String OutputDirectoryUrl
    {
      get
      {
        return this._OutputDirectoryUrl;
      }
      set
      {
        this._OutputDirectoryUrl = value;
      }
    }


    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region SMTP parameters

    private string _SmtpServer = "LocalHost";
    /// <summary>
    /// This class property contains SmtpServer for the object.
    /// </summary>
    public string SmtpServer
    {
      get
      {
        return this._SmtpServer;
      }
      set
      {
        this._SmtpServer = value;
      }
    }

    private int _SmtpServerPort = 587;
    /// <summary>
    /// This class property contains SmtpServerPort for the object.
    /// </summary>
    public int SmtpServerPort
    {
      get
      {
        return this._SmtpServerPort;
      }
      set
      {
        this._SmtpServerPort = value;
      }
    }

    private bool _EnableSsl = false;
    /// <summary>
    /// This class property enables SSL connection the SMTP server.
    /// </summary>
    public bool EnableSsl
    {
      get
      {
        return this._EnableSsl;
      }
      set
      {
        this._EnableSsl = value;
      }
    }

    private string _SmtpUserId = String.Empty;
    /// <summary>
    /// This class property contains SmtpServer for the object.
    /// </summary>
    public string SmtpUserId
    {
      get
      {
        return this._SmtpUserId;
      }
      set
      {
        this._SmtpUserId = value;
      }
    }

    private string _SmtpPassword = String.Empty;
    /// <summary>
    /// This class property contains SmtpServer for the object.
    /// </summary>
    public string SmtpPassword
    {
      get
      {
        return this._SmtpPassword;
      }
      set
      {
        this._SmtpPassword = value;
      }
    }

    #endregion

    #region class public methods.
    // ==================================================================================
    /// <summary>
    /// This method returns the current record export parameters.
    /// </summary>
    /// <returns>String object.</returns>
    // ----------------------------------------------------------------------------------

    public String getRecordExportParameters ( )
    {
      String stExportParameters = "Record Export Parameters:"
        + "\r\n -ExportDataSource: " + this.ExportDataSource
        + "\r\n -FormId: " + this.LayoutId
        + "\r\n -Include Free Text: " + this.IncludeFreeTextData
        + "\r\n -Include Draft Records: " + this.IncludeDraftRecords;

      return stExportParameters;
    }

    // ==================================================================================
    /// <summary>
    /// This method returns the current record export parameters.
    /// </summary>
    /// <returns>String object.</returns>
    // ----------------------------------------------------------------------------------
    public String getStatisticalExportParameters ( )
    {
      String stExportParameters = "Statistical Export Parameters:"
        + "\r\n -Hide Test Sites: " + this.IncludeTestSites
        + "\r\n -Include Submitted Records: " + this.IncludeDraftRecords
        + "\r\n -Include Free Text: " + this.IncludeFreeTextData
        + "\r\n -Export Visit Type: " + this.ExportMilestoneType
        + "\r\n -Output Format: " + this._OutputFormat;

      return stExportParameters;
    }

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

  }//END class TestDataPointIndex

}//END Namespace Evado.Model
