/***************************************************************************************
 * <copyright file="bll\EvFormRecordFields.cs" company="EVADO HOLDING PTY. LTD.">
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
 ****************************************************************************************/

using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;

//Evado. namespace references.
using Evado.Model;
using Evado.Dal;
using Evado.Model.Digital;

namespace Evado.Bll.Clinical
{
  /// <summary>
  /// A business to manage EvFormFields. This class uses EvFormField ResultData object for its content.
  /// </summary>
  public class EvFormRecordFields : EvBllBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EvFormRecordFields ( )
    {
      this.ClassNameSpace = "Evado.Bll.Clinical.EvFormRecordFields.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EvFormRecordFields ( EvClassParameters Settings )
    {
      this.ClassParameter = Settings;
      this.ClassNameSpace = "Evado.Bll.Clinical.EvFormRecordFields.";

      if ( this.ClassParameter.LoggingLevel == 0 )
      {
        this.ClassParameter.LoggingLevel = Evado.Dal.EvStaticSetting.LoggingLevel;
      }

      this._Dal_FormRecordFields = new Evado.Dal.Clinical.EvFormRecordFields ( Settings );
    }
    #endregion
    #region Class constants
    /// <summary>
    /// This constant defines the save item action for form record fields
    /// </summary>
    public const string ActionSaveItem = "SaveItem";

    /// <summary>
    /// This constant defines the confirm item action for form record fields
    /// </summary>
    public const string ActionConfirmItem = "ConfirmItem";

    /// <summary>
    /// This constant defines the query item action for form record fields
    /// </summary>
    public const string ActionQueryItem = "QueryItem";

    /// <summary>
    /// This constant defines the ResultData cleansing action for form record fields
    /// </summary>
    public const string ActionDataCleansing = "DataCleansing";

    /// <summary>
    /// This constant defines the new recorded action for form record fields
    /// </summary>
    public const string RECORDID_NEW = "NEW";
    #endregion

    #region Object Initialisation
    // 
    // Create instantiate the DAL class 
    // 
    private Evado.Dal.Clinical.EvFormRecordFields _Dal_FormRecordFields = new Evado.Dal.Clinical.EvFormRecordFields();

    /// <summary>
    /// Define the class property and state variable.
    /// </summary>
    private System.Text.StringBuilder _DebugLog = new System.Text.StringBuilder ( );
    /// <summary>
    /// This property contains the debug log string
    /// </summary>
    public string DebugLog
    {
      get
      {
        return _DebugLog.ToString();
      }
    }

    /// <summary>
    /// This property contains the html debug log string.
    /// </summary>
    public string DebugLog_Html
    {
      get
      {
        return DebugLog.Replace("\r\n", "<br/>");
      }
    }

    #endregion

    #region trial FirstSubject trial selectionList Methods

    // =====================================================================================
    /// <summary>
    /// This class returns a list of options for form record field objects based on TrialId, FormId and FieldId
    /// </summary>
    /// <param name="TrialId">String: trial identifier</param>
    /// <param name="FormId">String: form identifier</param>
    /// <param name="FieldId">String: field identifier</param>
    /// <returns>List of EvOption: a list of options for form record field object.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the list of formfield options based on TrialId, FormId and FieldId
    ///  
    /// 2. Return the list of formfield options
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> GetItemValueList(string TrialId, string FormId, string FieldId)
    {
      this.LogMethod ( "getItemValueList. " );
      this.LogDebug ("FormId: " + FormId  );
      this.LogDebug ("ItemId: " + FieldId );
      // 
      // Initialise the method variables and objects.
      // 
      List<EvOption> list = new List<EvOption>();

      //
      // Execute the query
      //     
      list = this._Dal_FormRecordFields.GetItemValueList ( TrialId, FormId, FieldId );
      this.LogClass ( this._Dal_FormRecordFields.Log );

      this.LogMethodEnd ( "getItemValueList" );
      // 
      // Return the currentSchedule.
      // 
      return list;

    }//End GetItemValueList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of options for form record field objects based on ProjectId
    /// </summary>
    /// <param name="ProjectId">String: trial identifier</param>
    /// <returns>List of EvOption: a list of options for form record field object.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the list of formfield options based on ProjectId
    ///  
    /// 2. Return the list of formfield options
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> GetItemList(string ProjectId)
    {
      this.LogMethod ( "getItemList method." );
      this.LogDebug ("ProjectId: " + ProjectId ); ;

      List<EvOption> list = this._Dal_FormRecordFields.GetItemList ( ProjectId, false );
      this.LogClass ( this._Dal_FormRecordFields.Log );

      this.LogMethodEnd ( "GetItemList" );

      return list;

    }// End GetItemList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of options for form record field objects based on ProjectId 
    /// and SafetyReportItem condition
    /// </summary>
    /// <param name="ProjectId">String: trial identifier</param>
    /// <param name="SafetyReportItems">Boolean: true, if the safetyReportItems exist</param>
    /// <returns>List of EvOption: a list of options for form record field object.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the list of formfield options based on ProjectId 
    /// and SafetyReportItem condition
    ///  
    /// 2. Return the list of formfield options
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> GetItemList(string ProjectId, bool SafetyReportItems)
    {
      this.LogMethod ( "getItemList method." );
      this.LogDebug ("ProjectId: " + ProjectId );

      List<EvOption> view = _Dal_FormRecordFields.GetItemList ( ProjectId, SafetyReportItems );
      this.LogClass ( this._Dal_FormRecordFields.Log );

      this.LogMethodEnd ( "GetItemList" );

      return view;

    }//End GetItemList method.

    // =====================================================================================
    /// <summary>
    /// This class gets a ArrayList containing a currentSchedule of EvFormField ResultData objects for a FirstSubject.
    /// </summary>
    /// <param name="ProjectId">string: trial identifier</param>
    /// <param name="ItemId">string: an item identifier</param>
    /// <param name="Grouping">EvChart.GroupingOptions: a grouping object</param>
    /// <param name="Aggregation">EvChart.AggregationOptions: an aggregation object</param>
    /// <returns>List of EvDataItem: a list of ResultData item objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the ResultData item objects list
    /// 
    /// 2. Return the list of ResultData items objects
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvDataItem> GetAnalysisItems(
      string ProjectId,
      string ItemId,
      EvChart.GroupingOptions Grouping,
      EvChart.AggregationOptions Aggregation)
    {
      this.LogMethod ( "getAnalysisItems method." );
      this.LogDebug ("ProjectId: " + ProjectId );
      this.LogDebug ("ItemId: " + ItemId );
      this.LogDebug ("Grouping: " + Grouping );
      this.LogDebug ("Aggregation: " + Aggregation );
      // 
      // Execute the query
      // 
      List<EvDataItem> view = _Dal_FormRecordFields.GetAnalysisItems(
         ProjectId,
         ItemId,
         Grouping,
         Aggregation);
      this.LogClass ( this._Dal_FormRecordFields.Log );

      this.LogMethodEnd ( "getAnalysisItems" );

      // 
      // Return the selectionList
      // 
      return view;

    }//End GetAnalysisItems method.

    // =================================================================================
    /// <summary>
    /// This method retrieves a report object. 
    /// </summary>
    /// <param name="Report">EvReport: a report object containing the selection criteria</param>
    /// <returns>EvReport: a report object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the report object. 
    /// 
    /// 2. Return the report object. 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public EvReport getReport(EvReport Report)
    {
      this.LogMethod ( "getReport method." );

      Report = this._Dal_FormRecordFields.getMonitoringReport(Report);
      this.LogClass ( this._Dal_FormRecordFields.Log );

      this.LogMethodEnd ( "getReport" );

      return Report;
    }//END getReport class.

    #endregion

  }//END EvFormRecordFields Class.

}//END namespace Evado.Bll.Clinical
