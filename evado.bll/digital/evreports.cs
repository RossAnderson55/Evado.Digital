/* <copyright file="BLL\EvReports.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2022 EVADO HOLDING PTY. LTD..  All rights reserved.
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
 *  This class handles the database query interface for the EvReports object.
 * 
 *  This class contains the following public properties:
 *   DebugLog:       Containing the exeuction status of this class, used for debugging the 
 *                 class from UI layers.
 * 
 *  This class contains the following public methods:
 * 
 *   getReport:      Executes a query to generate an reports.
 * 
 ****************************************************************************************/

using System;
using System.Data;
using System.Collections; 
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;

//Evado. namespace references.
using Evado.Model;
using Evado.Model.Digital;


namespace Evado.Bll.Digital
{
  /// <summary>
  /// A business Component used to manage Organisation roles
  /// The Evado.evado.Model.TrialUser is used in most methods 
  /// and is used to store serializable information about an account
  /// </summary>
  public class EvReports : EvBllBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EvReports ( )
    {
      this.ClassNameSpace = "Evado.Bll.Clinical.EvReports.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EvReports ( EvClassParameters Settings )
    {
      this.ClassParameter = Settings;
      this.ClassNameSpace = "Evado.Bll.Clinical.EvReports.";

      if ( this.ClassParameter.LoggingLevel == 0 )
      {
        this.ClassParameter.LoggingLevel = Evado.Dal.EvStaticSetting.LoggingLevel;
      }

      this. _dalReports = new Evado.Dal.Digital.EvReports ( Settings );
    }
    #endregion 

    #region Global class properties and objects
    // 
    // Instantiate the DAL Class\_dal    
    // 
    private Evado.Dal.Digital.EvReports _dalReports = new Evado.Dal.Digital.EvReports ( );

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    // =====================================================================================
    /// <summary>
    /// getReport method
    /// 
    /// Description:
    /// Get an ArrayList of user ResultData objects. 
    /// 
    /// </summary>
    // -------------------------------------------------------------------------------------
    public  EvReport getReport ( EvReport Report )
    {
      this.LogMethod(  "getReport method. " );
      this.LogDebug(  "ReportId: " + Report.ReportId );
      this.LogDebug(  "SourceId: " + Report.DataSourceId );
      this.LogDebug(  "Source: " + Report.Source );
      this.LogDebug(  "RequireUserTrial: " + Report.RequireUserTrial );

      this.LogDebug(  "Report.Queries.Length: " + Report.Queries.Length );

      for( int index=0;index<Report.Queries.Length; index++ )
      {
        Evado.Model.Digital.EvReportQuery query = Report.Queries [ index ];
        this.LogDebug ( index + " > "+ query.getAsString() );
      }

      this.LogDebug ( "Report.Column.Count: " + Report.Queries.Length );
      for ( int index = 0; index < Report.Columns.Count; index++ )
      {
        Evado.Model.Digital.EvReportColumn column = Report.Columns [ index ];
        this.LogDebug ( index + " > " + column.getAsString ( ) );
      }

      //
      // initialise the methods variables and objects.
      //
      EvReport report = new EvReport( );

      switch ( Report.DataSourceId )
      {
        case EvReport.ReportSourceCode.FormFields:
          {
            Evado.Dal.Digital.EdRecordFields DAL_formField = new Evado.Dal.Digital.EdRecordFields ( this.ClassParameter );

            report = DAL_formField.getReport ( Report );

            this.LogClass ( "Form Fields Status: " + DAL_formField.Log );

            break;
          }
        case EvReport.ReportSourceCode.Field_Monitoring_Query:
          {
            Evado.Dal.Digital.EdRecordValues DAL_formRecordField = new Evado.Dal.Digital.EdRecordValues ( this.ClassParameter );

            report = Report; // DAL_formRecordField.getMonitoringReport ( Report );

            this.LogClass ( "Form Record Fields Status: " + DAL_formRecordField.Log );

            break;
          }
        case EvReport.ReportSourceCode.Subject_Demographics:
          {
            report =  null;

            break;
          }
        case EvReport.ReportSourceCode.Subject_Record_Status:
          {
            Evado.Dal.Digital.EdRecords dalRecords = new Evado.Dal.Digital.EdRecords ( this.ClassParameter );

            report = Report; // new dalRecords.getRecordStatusReport ( Report );
            //this.LogDebug(  dalRecords.Log );

            break;
          }
        case EvReport.ReportSourceCode.Class:
          {
            Type providerType = Type.GetType( Report.SqlDataSource );

            EvReportDataProviderBase provider = (EvReportDataProviderBase) providerType.GetConstructor (
              new Type [ 0 ] ).Invoke( new Object [ 0 ] );

            report = provider.getReport( Report );
            break;
          }
        default:
        case EvReport.ReportSourceCode.SqlQuery:
          {
            report = this._dalReports.getReport( Report );

            this.LogClass ( this._dalReports.Log );

            break;
          }
      }
      
      //Sets the correct date for this Report. Overrides the stored date since this is a new
      //Report.
      report.ReportDate = DateTime.Now;

      return report;

    } // Close getReport method.

    // ++++++++++++++++++++++++++++++++++  END OF SOURCE CODE +++++++++++++++++++++++++++++++++++++
  }//END EvReport class

} // Close namespace Evado.Bll
