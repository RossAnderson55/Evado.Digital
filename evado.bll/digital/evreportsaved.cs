/***************************************************************************************
 * <copyright file="BLL\clinical\EvReportSaved.cs" company="EVADO HOLDING PTY. LTD.">
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
using System.Text;
using Evado.Model;
using System.Collections;
using Evado.Model.Digital;

namespace Evado.Bll.Digital
{
  /// <summary>
  /// This class is the business layout object to save generated reports for later display
  /// </summary>
  public class EvReportSaved: EvBllBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EvReportSaved ( )
    {
      this.ClassNameSpace = "Evado.Bll.Clinical.EvReportSaved.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EvReportSaved ( EvClassParameters Settings )
    {
      this.ClassParameter = Settings;
      this.ClassNameSpace = "Evado.Bll.Clinical.EvReportSaved.";

      if ( this.ClassParameter.LoggingLevel == 0 )
      {
        this.ClassParameter.LoggingLevel = Evado.Dal.EvStaticSetting.LoggingLevel;
      }

      this._dalReportSaved = new Evado.Dal.Digital.EvReportSaved ( Settings );
    }
    #endregion

    #region Class variables and properties
    // 
    // Instantiate the DAL Class\
    // 
    private Evado.Dal.Digital.EvReportSaved _dalReportSaved = new Evado.Dal.Digital.EvReportSaved ( );

    #endregion

    #region Class methods
    // =====================================================================================
    /// <summary>
    /// This class saves new report to database
    /// </summary>
    /// <param name="Report">EvReport: a report object</param>
    /// <returns>EvEventCodes: an event code for saving reports</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for adding report to database
    /// 
    /// 2. Return an event code for saving new report. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes saveNewReport ( EvReport Report )
    {
      this.LogMethod ( "saveNewReport method." );
      this.LogDebug ( "Adding Report." );
      EvEventCodes iReturn = EvEventCodes.Ok;

      iReturn = this._dalReportSaved.addReport ( Report );

      this.LogClass( this._dalReportSaved.Log );

      return iReturn;

    }//END saveNewReport class
    
    // =====================================================================================
    /// <summary>
    /// This class retrieves a list of report objects based on RecordTypeId and category
    /// </summary>
    /// <param name="TrialId">string: (Mandatory) The trial identifier</param>
    /// <param name="ReportTypeId">EvReport.ReportTypeCode: a report QueryType identifier</param>
    /// <param name="Category">string: A report category</param>
    /// <returns>List of EvReport: a list of report objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving a list of Report objects
    /// 
    /// 2. Return a list of Report objects
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvReport> getView ( string TrialId, EvReport.ReportTypeCode ReportTypeId, string Category )
    {
      this.LogMethod ( "getView method." );
      List<EvReport> view = this._dalReportSaved.getView ( TrialId, ReportTypeId, Category );

      this.LogClass ( this._dalReportSaved.Log );
      return view;

    } //EMD getMilestoneList method.

    // =====================================================================================
    /// <summary>
    /// This class retrieves a report object based on Guid
    /// </summary>
    /// <param name="Guid">Guid: a global unique identifier</param>
    /// <returns>EvReport: a report object</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Execute the method for retrieving a list Report participant
    /// 
    /// 2. Return a report object
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvReport getReport ( Guid Guid )
    {
      this.LogMethod ( "getReport method." );

      EvReport report = this._dalReportSaved.getReport ( Guid );

      this.LogClass ( this._dalReportSaved.Log );

      return report;

    }//END getReport class

    #endregion

  }//END EvReportSaved class

}//END namespace Evado.Bll.Digital
