/***************************************************************************************
 * <copyright file="DAL\EvRecords.cs" company="EVADO HOLDING PTY. LTD.">
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
using System.Text;

//Evado. namespace references.
using Evado.Model;
using Evado.Model.Digital;

namespace Evado.Bll.Clinical
{
  /// <summary>
  /// This business object manages the EvRecords in the system.
  /// </summary>
  public class EvFormRecords : EvBllBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EvFormRecords ( )
    {
      this.ClassNameSpace = "Evado.Bll.Clinical.EvFormRecords.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EvFormRecords ( EvClassParameters Settings )
    {
      this.ClassParameter = Settings;
      this.ClassNameSpace = "Evado.Bll.Clinical.EvFormRecords.";

      this._DalRecords = new Evado.Dal.Clinical.EvFormRecords ( Settings );

      this._DalForms = new Evado.Dal.Clinical.EvForms ( Settings );
    }
    #endregion


    #region Class constant
    /// <summary>
    /// This constant defines the Selection action
    /// </summary>
    public const string ActionSelection = "Selection";

    #endregion

    #region Class varialbes and properties.

    //
    // Create instantiate the DAL class 
    // 
    private Evado.Dal.Clinical.EvFormRecords _DalRecords = new Evado.Dal.Clinical.EvFormRecords ( );
    private Evado.Dal.Clinical.EvForms _DalForms = new Evado.Dal.Clinical.EvForms ( );

    //
    // Instantiate the Business Logic.
    //
    //TrialSamples BllTrialSamples = new TrialSamples();
    private EvAlerts _BllTrialAlerts = new EvAlerts ( );

    #endregion

    #region project record List queries

    // =====================================================================================
    /// <summary>
    /// This class returns a list of form objects. 
    /// </summary>
    /// <param name="TrialId">string: (Mandatory) Project identifier.</param>
    /// <param name="OrgId">string: (Optional) Organization identifier.</param>
    /// <param name="SubjectId">string: (Optional) FirstSubject identifier.</param>
    /// <param name="VisitId">string: (Optional) A visit identifier.</param>
    /// <param name="State">string: (Optional) the form state.</param>
    /// <param name="NotSelectedState">Boolean: True, do not select these states, False: select states.</param>
    /// <param name="HideTestSites">Boolean: True, hide test sites, False: select all sites.</param>
    /// <param name="OrderBy">string: (Optional) the sorting order of the result set.</param>
    /// <returns>List of EvForm: a list of form objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the list of form objects. 
    /// 
    /// 2. Return the list of form objects. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public List<EvForm> getRecordList (
      string TrialId,
      string OrgId,
      string SubjectId,
      string VisitId,
      string State,
      bool NotSelectedState,
      bool HideTestSites,
      string OrderBy )
    {
      this.LogValue ( Evado.Model.Digital.EvcStatics.CONST_METHOD_START
        + "Evado.Bll.Clinical.EvRecords.getView method. "
      + ", TrialId: " + TrialId
      + ", OrgId: " + OrgId
      + ", SubjectId: " + SubjectId
      + ", VisitId: " + VisitId
      + ", State: " + State
      + ", NotSelectedState: " + NotSelectedState
      + ", HideTestSites: " + HideTestSites );

      // 
      // Initialise the query object
      // 
      EvQueryParameters query = new EvQueryParameters ( TrialId );
      query.SubjectId = SubjectId;
      query.VisitId = VisitId;
      query.OrgId = OrgId;
      query.State = State;
      query.NotSelectedState = NotSelectedState;
      query.IncludeTestSites = HideTestSites;
      query.OrderBy = OrderBy;
      query.HasBeenMonitored = true;

      // 
      // Execute the query.
      // 
      List<EvForm> view = this._DalRecords.getRecordList ( query );

      this.LogClass ( this._DalRecords.Log );

      //
      // Return selectionList to UI
      //
      return view;

    }//END getRecordList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of form objects. 
    /// </summary>
    /// <param name="TrialId">string: (Mandatory) Project identifier.</param>
    /// <param name="SubjectId">string: (Optional) FirstSubject identifier.</param>
    /// <param name="VisitId">string: (Optional) A visit identifier.</param>
    /// <param name="State">string: (Optional) the form state.</param>
    /// <param name="NotSelectedState">Boolean: True, do not select these states, False: select states.</param>
    /// <param name="HideTestSites">Boolean: True, hide test sites, False: select all sites.</param>
    /// <param name="OrderBy">string: (Optional) the sorting order of the result set.</param>
    /// <returns>List of EvForm: a list of form objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the list of form objects. 
    /// 
    /// 2. Return the list of form objects. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public List<EvForm> getRecordList (
      string TrialId,
      string SubjectId,
      string VisitId,
      string State,
      bool NotSelectedState,
      bool HideTestSites,
      string OrderBy )
    {
      this.LogValue ( Evado.Model.Digital.EvcStatics.CONST_METHOD_START
        + "Evado.Bll.Clinical.EvFormRecords.getView method. " );
      this.LogValue ( ", TrialId: " + TrialId
      + ", SubjectId: " + SubjectId
      + ", VisitId: " + VisitId
      + ", State: " + State
      + ", NotSelectedState: " + NotSelectedState
      + ", HideTestSites: " + HideTestSites );

      // 
      // Initialise the query object
      // 
      EvQueryParameters query = new EvQueryParameters ( TrialId );
      query.TrialId = TrialId;
      query.SubjectId = SubjectId;
      query.VisitId = VisitId;
      query.State = State;
      query.NotSelectedState = NotSelectedState;
      query.IncludeTestSites = HideTestSites;
      query.OrderBy = OrderBy;

      // 
      // Execute the query.
      // 
      List<EvForm> view = this._DalRecords.getRecordList ( query );

      this.LogClass ( this._DalRecords.Log );
      //
      // Return selectionList to UI
      //
      return view;

    }//END getRecordList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of form objects. 
    /// </summary>
    /// <param name="TrialId">String: (Mandatory) Project identifier.</param>
    /// <param name="SubjectId">String: (Optional) FirstSubject identifier.</param>
    /// <param name="FormId">String: a form identifier</param>
    /// <param name="RecordState">string: (Optional) the form state.</param>
    /// <param name="NotSelectedState">Boolean: True, do not select these states, False: select states.</param>
    /// <param name="IncludeRecordFields">Boolean: true, if the record fields are included</param>
    /// <returns>List of EvForm: a list of form objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the list of form objects. 
    /// 
    /// 2. Return the list of form objects. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public List<EvForm> getRecordList (
      String TrialId,
      String SubjectId,
      String FormId,
      String RecordState,
      bool NotSelectedState,
      bool IncludeRecordFields )
    {
      this.LogValue ( Evado.Model.Digital.EvcStatics.CONST_METHOD_START
        + "Evado.Bll.Clinical.EvFormRecords.getView method. " );
      this.LogValue ( "TrialId: " + TrialId
      + ", SubjectId: " + SubjectId
      + ", FormId: " + FormId
      + ", NotSelectedState: " + NotSelectedState
      + ", IncludeRecordFields: " + IncludeRecordFields );

      // 
      // Initialise the query object
      // 
      EvQueryParameters query = new EvQueryParameters ( TrialId );
      query.SubjectId = SubjectId;
      query.FormId = FormId;
      query.State = RecordState;
      query.NotSelectedState = NotSelectedState;
      query.IncludeRecordFields = IncludeRecordFields;

      // 
      // Execute the query.
      // 
      List<EvForm> view = this._DalRecords.getRecordList ( query );

      this.LogClass ( this._DalRecords.Log );

      //
      // Return selectionList to UI
      //
      return view;

    }//END getRecordList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of form objects. 
    /// </summary>
    /// <param name="TrialId">string: (Mandatory) Project identifier.</param>
    /// <param name="OrgId">string: an organization identifier</param>
    /// <param name="State">string: the form state</param>
    /// <param name="NotSelectedState">Boolean: True, do not select these states, False: select states.</param>
    /// <param name="HideTestSites">Boolean: true, if the test sites are hiden</param>
    /// <param name="OrderBy">string: a sorting query string</param>
    /// <param name="UserCommonName">string: a user common name</param>
    /// <returns>List of EvForm: a list of form objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the list of form objects. 
    /// 
    /// 2. Return the list of form objects. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public List<EvForm> getRecordList (
      string TrialId,
      string OrgId,
      string State,
      bool NotSelectedState,
      bool HideTestSites,
      string OrderBy,
      string UserCommonName )
    {
      this.LogValue ( Evado.Model.Digital.EvcStatics.CONST_METHOD_START
        + "Evado.Bll.Clinical.EvFormRecords.getView method. " );
      this.LogValue ( "TrialId: " + TrialId
      + ", OrgId: " + OrgId
      + ", State: " + State
      + ", NotSelectedState: " + NotSelectedState
      + ", HideTestSites: " + HideTestSites
      + ", UserCommonName: " + UserCommonName );
      // 
      // Initialise the query object
      // 
      EvQueryParameters query = new EvQueryParameters ( TrialId );
      query.UserCommonName = UserCommonName;
      query.TrialId = TrialId;
      query.OrgId = OrgId;
      query.State = State;
      query.NotSelectedState = NotSelectedState;
      query.IncludeTestSites = HideTestSites;
      query.OrderBy = OrderBy;

      // 
      // Execute the query.
      // 
      List<EvForm> view = this._DalRecords.getRecordList ( query );

      this.LogClass ( this._DalRecords.Log );

      //
      // Return selectionList to UI
      //
      return view;

    }//END getRecordList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of form objects. 
    /// </summary>
    /// <param name="QueryParameters">EvQueryParameters: a query parameters object</param>
    /// <returns>List of EvForm: a list of form objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the list of form objects. 
    /// 
    /// 2. Return the list of form objects. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public int getRecordCount (
      EvQueryParameters QueryParameters )
    {
      this.LogMethod ( "getRecordCount method. " );
      this.LogValue ( "EvQueryParameters parameters." );
      this.LogValue ( "- ProjectId: " + QueryParameters.TrialId );
      this.LogValue ( "- FormId: " + QueryParameters.FormId );
      this.LogValue ( "- OrgId: " + QueryParameters.OrgId );
      this.LogValue ( "- MilestoneId: " + QueryParameters.MilestoneId );
      this.LogValue ( "- SubjectId: " + QueryParameters.SubjectId );
      this.LogValue ( "- VisitId: " + QueryParameters.VisitId );
      this.LogValue ( "- IncludeRecordFields: " + QueryParameters.IncludeRecordFields );
      this.LogValue ( "- UserCommonName: " + QueryParameters.UserCommonName );
      this.LogValue ( "- State: " + QueryParameters.State );
      this.LogValue ( "- SubjectState: " + QueryParameters.SubjectState );
      this.LogValue ( "- NotSelectedState: " + QueryParameters.NotSelectedState );
      this.LogValue ( "- QueryState: " + QueryParameters.QueryState );
      this.LogValue ( "- StartDate: " + QueryParameters.stStartDate );
      this.LogValue ( "- FinishDate: " + QueryParameters.stFinishDate );
      this.LogValue ( "- UserVisitDate: " + QueryParameters.UserVisitDate );
      this.LogValue ( "- RecordRangeStart: " + QueryParameters.RecordRangeStart );
      this.LogValue ( "- RecordRangeFinish: " + QueryParameters.RecordRangeFinish );
      this.LogValue ( "- UserVisitDate: " + QueryParameters.UserVisitDate );
      // 
      // Execute the query.
      // 
      int inResultCount = this._DalRecords.getRecordCount ( QueryParameters );

      this.LogClass ( this._DalRecords.Log );

      this.LogMethodEnd ( "getRecordCount" );
      //
      // Return selectionList to UI
      //
      return inResultCount;

    }//END getRecordList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of form objects. 
    /// </summary>
    /// <param name="QueryParameters">EvQueryParameters: a query parameters object</param>
    /// <returns>List of EvForm: a list of form objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the list of form objects. 
    /// 
    /// 2. Return the list of form objects. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public List<EvForm> getRecordList (
      EvQueryParameters QueryParameters )
    {
      this.LogMethod ( "getRecordList method. " ); 
      this.LogDebug ( "EvQueryParameters parameters." );
      this.LogDebug ( "- TrialId: " + QueryParameters.TrialId );
      this.LogDebug ( "- FormId: " + QueryParameters.FormId );
      this.LogDebug ( "- OrgId: " + QueryParameters.OrgId );
      this.LogDebug ( "- MilestoneId: " + QueryParameters.MilestoneId );
      this.LogDebug ( "- SubjectId: " + QueryParameters.SubjectId );
      this.LogDebug ( "- PatientId: " + QueryParameters.PatientId );
      this.LogDebug ( "- VisitId: " + QueryParameters.VisitId );
      this.LogDebug ( "- IncludeRecordFields: " + QueryParameters.IncludeRecordFields );
      this.LogDebug ( "- UserCommonName: " + QueryParameters.UserCommonName );
      this.LogDebug ( "- State: " + QueryParameters.State );
      this.LogDebug ( "- SubjectState: " + QueryParameters.SubjectState );
      this.LogDebug ( "- NotSelectedState: " + QueryParameters.NotSelectedState );
      this.LogDebug ( "- IncludeTestSites: " + QueryParameters.IncludeTestSites );
      this.LogDebug ( "- QueryState: " + QueryParameters.QueryState );
      this.LogDebug ( "- StartDate: " + QueryParameters.stStartDate );
      this.LogDebug ( "- FinishDate: " + QueryParameters.stFinishDate );
      this.LogDebug ( "- UserVisitDate: " + QueryParameters.UserVisitDate );
      this.LogDebug ( "- RecordRangeStart: " + QueryParameters.RecordRangeStart );
      this.LogDebug ( "- RecordRangeFinish: " + QueryParameters.RecordRangeFinish );
      this.LogDebug ( "- UserVisitDate: " + QueryParameters.UserVisitDate );
      // 
      // Execute the query.
      // 
      List<EvForm> view = this._DalRecords.getRecordList ( QueryParameters );

      this.LogClass ( this._DalRecords.Log );

      this.LogMethodEnd ( "getRecordList" );
      //
      // Return selectionList to UI
      //
      return view;

    }//END getRecordList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of current form objects. 
    /// </summary>
    /// <param name="TrialId">string: (Mandatory) Project identifier.</param>
    /// <param name="OrgId">string: an organization identifier</param>
    /// <param name="SubjectId">string: a milestone identifier</param>
    /// <param name="VisitId">string: a visit identifier</param>
    /// <param name="IncludeRecordFields">bool: true = include form field ResultData</param>
    /// <returns>List of EvForm: a list of form objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the list of form objects. 
    /// 
    /// 2. Return the list of form objects. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public List<EvForm> GetCurrentRecordsView (
      string TrialId,
      string OrgId,
      string SubjectId,
      string VisitId,
      bool IncludeRecordFields )
    {
      this.LogValue ( "Evado.Bll.Clinical.EvFormRecords.getView method. " );
      this.LogValue ( "TrialId: " + TrialId );
      this.LogValue ( "VisitId: " + VisitId );
      this.LogValue ( "OrgId: " + OrgId );
      this.LogValue ( "SubjectId: " + SubjectId );
      this.LogValue ( "IncludeRecordFields: " + IncludeRecordFields );

      // 
      // Execute the query.
      // 
      List<EvForm> view = this._DalRecords.getCurrentRecordList (
        TrialId,
        OrgId,
        SubjectId,
        VisitId,
        IncludeRecordFields );

      this.LogClass ( this._DalRecords.Log );
      this.LogValue ( "Count: " + view.Count );

      //
      // Return selectionList to UI
      //
      return view;
    }//END GetCurrentRecordsView method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of last instance form objects. 
    /// </summary>
    /// <param name="TrialId">string: (Mandatory) Project identifier.</param>
    /// <param name="SubjectId">string: a milestone identifier</param>
    /// <param name="MilestoneId">string: a milestone identifier</param>
    /// <returns>List of EvForm: a list of form objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the list of form objects. 
    /// 
    /// 2. Loop through the list and add the formIds to the formIdlist string with a ";" character. 
    /// 
    /// 3. Loop through the formIdlist string array and add the matching formId to the lastRecord object
    /// 
    /// 4. Add the LastRecord object to the Last records list. 
    /// 
    /// 5. Return the last records list. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public List<EvForm> GetLastInstanceView (
      string TrialId,
      string SubjectId,
      string MilestoneId )
    {
      this.LogValue ( "Evado.Bll.Clinical.EvFormRecords.GetLastInstanceView method. " );
      this.LogValue ( "TrialId: " + TrialId
      + ", SubjectId: " + SubjectId
      + ", MilestoneId: " + MilestoneId );

      // 
      // Initialise the query object
      // 
      EvQueryParameters query = new EvQueryParameters ( TrialId );
      query.TrialId = TrialId;
      query.SubjectId = SubjectId;
      query.MilestoneId = MilestoneId;
      query.State = EvFormObjectStates.Withdrawn + ";" + EvFormObjectStates.Queried_Record_Copy;
      query.NotSelectedState = true;
      query.OrderBy = "FormId, TR_RecordDate; ";
      String FormIdList = String.Empty;
      List<EvForm> recordList = new List<EvForm> ( );
      EvForm lastRecord = new EvForm ( );

      // 
      // Execute the query.
      // 
      List<EvForm> view = this._DalRecords.getRecordList ( query );

      this.LogClass ( this._DalRecords.Log );

      //
      // Identify the forms in the list.
      //
      foreach ( EvForm record in view )
      {
        if ( FormIdList.Contains ( record.FormId ) == false )
        {
          FormIdList += ";" + record.FormId;
        }
      }

      this.LogValue ( "List of Forms: " + FormIdList + "\r\n" );

      //
      // if the form list exists.
      //
      if ( FormIdList != String.Empty )
      {
        //
        // Iterate through the list of records to find the last record for
        // formID QueryType in the list.
        //
        String [ ] arrFormIdList = FormIdList.Split ( ';' );

        for ( int i = 0; i < FormIdList.Length; i++ )
        {
          //
          // reset the record for a new formId.
          //
          lastRecord = new EvForm ( );

          foreach ( EvForm record in view )
          {
            //
            // Process the records that match the form id.
            //
            if ( arrFormIdList [ i ] == record.FormId )
            {
              if ( lastRecord.RecordDate < record.RecordDate )
              {
                lastRecord = record;
              }
            }

          }//Record iteration loop

          //
          // Add the last record to the record list.
          //
          if ( lastRecord.RecordId != String.Empty )
          {
            this.LogValue ( " RecordId: " + lastRecord.RecordId );

            recordList.Add ( lastRecord );
          }

        }//END form iteration loop
      }

      //
      // Return selectionList to UI
      //
      return recordList;

    }//END GetLastInstanceView method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of option for selected form objects based on query object and ByUid condition. 
    /// </summary>
    /// <param name="Query">EvQueryParameters: a query parameter object</param>
    /// <param name="ByUid">Boolean: true, if the list is selected by Uid</param>
    /// <returns>List of EvOption: a list of option objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the list of option objects based on query object and ByUid condition. 
    /// 
    /// 2. Return the list of option objects. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> getOptionList (
      EvQueryParameters Query,
      bool ByUid )
    {
      this.LogValue ( "Evado.Bll.Clinical.EvFormRecords.GetList method. " );
      this.LogValue ( "TrialId: " + Query.TrialId
      + ", OrgId: " + Query.OrgId
      + ", VisitId: " + Query.VisitId
      + ", State: " + Query.State
      + ", NotSelectedState: " + Query.NotSelectedState
      + ", HideTestSites: " + Query.IncludeTestSites
      + ", UserCommonName: " + Query.UserCommonName );

      List<EvOption> List = this._DalRecords.getOptionList ( Query, ByUid );

      this.LogClass ( this._DalRecords.Log );

      return List;
    }//END GetList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of option for selected form objects based 
    /// on TrialId, SubjectId and ByUid condition
    /// </summary>
    /// <param name="TrialId">string: the trial identifier</param>
    /// <param name="SubjectId">string: the milestone identifier</param>
    /// <param name="ByUid">Boolean: true, if the list is selected by Uid</param>
    /// <returns>List of EvOption: a list of option objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the list of option objects based 
    /// on TrialId, SubjectId and ByUid condition. 
    /// 
    /// 2. Return the list of option objects. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public List<EvOption> getOptionList (
      string TrialId,
      string SubjectId,
      bool ByUid )
    {
      List<EvOption> List = this._DalRecords.getOptionList ( TrialId, SubjectId, ByUid );

      this.LogClass ( this._DalRecords.Log );

      return List;

    }//END GetList method.

    // =====================================================================================
    /// <summary>
    /// This class checks whether the form is used.
    /// </summary>
    /// <param name="TrialId">string: a trial identifier</param>
    /// <param name="FormId">string: a form identifier</param>
    /// <returns>Boolean: true, if the form is used</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for checking whether the form is used. 
    /// 
    /// 2. Return true, if the form is used. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public bool CheckIfFormUsed (
      string TrialId,
      string FormId )
    {
      bool bReturn = this._DalRecords.checkIfFormUsed ( TrialId, FormId );

      this.LogClass ( this._DalRecords.Log );

      return bReturn;

    }//END CheckIfFormUsed method.

    // =====================================================================================
    /// <summary>
    /// This class retrieves an arraylist of form record objects
    /// </summary>
    /// <param name="ProjectId">string: a Project identifier</param>
    /// <param name="FormId">string: a form identifier</param>
    /// <param name="ItemId">string: an item identifier</param>
    /// <param name="Value">string: an item value</param>
    /// <returns>ArrayList: an arraylist of form record objects</returns>
    /// <remarks>
    /// This class contains the following steps: 
    /// 
    /// 1. Execute the method for retrieving an arraylist of form record objects
    /// 
    /// 2. Return an arraylist of form record objects.
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public List<EvForm> GetItemQuery ( string ProjectId, string FormId, string ItemId, string Value )
    {
      this.LogValue ( "Evado.Bll.Clinical.EvFormRecords.getItemQuery method" );
      this.LogValue ( "ProjectId: " + ProjectId +
        ", ChecklistId: " + FormId +
        ", ItemId: " + ItemId +
        ", Value: " + Value );
      // 
      // Initialise the method variables and objects.
      //
      List<EvForm> recordList = this._DalRecords.getRecordByFieldValue ( ProjectId, FormId, ItemId, Value );
      this.LogClass ( this._DalRecords.Log );

      return recordList;

    }//END GetItemQuery method.

    #endregion

    #region project record retrieval queries

    // =====================================================================================
    /// <summary>
    /// This class retrieves a form object based on Guid
    /// </summary>
    /// <param name="RecordGuid">Guid: a form's Global unique identifier</param>
    /// <returns>EvForm: a form object</returns>
    /// <remarks>
    /// This class consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the form object based on Guid
    /// 
    /// 2. Return a form object. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvForm getRecord ( Guid RecordGuid )
    {
      this.LogMethod( "getRecord method. " );
      this.LogValue ( "RecordGuid: " + RecordGuid );

      // 
      // Initialise the method variables and objects.
      // 
      EvForm record = new EvForm ( );

      //
      // Execute the query
      //
      record = this._DalRecords.getRecord ( RecordGuid, true );
      this.LogClass ( this._DalRecords.Log );

      return record;

    }//END getRecord method

    // =====================================================================================
    /// <summary>
    /// This class retrieves a form object based on Guid
    /// </summary>
    /// <param name="RecordGuid">Guid: a form's Global unique identifier</param>
    /// <returns>EvForm: a form object</returns>
    /// <remarks>
    /// This class consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the form object based on Guid
    /// 
    /// 2. Return a form object. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvForm getRecordNoFieldComments ( Guid RecordGuid )
    {
      this.LogValue ( "DAL:EvRecord:getRecord method. " );
      this.LogValue ( "RecordGuid: " + RecordGuid );

      // 
      // Initialise the method variables and objects.
      // 
      EvForm record = new EvForm ( );

      //
      // Execute the query
      //
      record = this._DalRecords.getRecord ( RecordGuid, false );
      this.LogClass ( this._DalRecords.Log );

      return record;

    }//END getRecord method

    // =====================================================================================
    /// <summary>
    /// This class retrieves a form object based on RecordId
    /// </summary>
    /// <param name="RecordId">String record identifier</param>
    /// <returns>EvForm: a form object</returns>
    /// <remarks>
    /// This class consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the form object based on RecordId
    /// 
    /// 2. Return a form object. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvForm getRecord ( String RecordId )
    {
      this.LogMethod ( "GetRecord method. " );
      this.LogValue ( "RecordId: " + RecordId );
      // 
      // Initialise the method variables and objects.
      // 
      EvForm record = new EvForm ( );

      //
      // Execute the query
      //
      record = this._DalRecords.getRecord ( RecordId, true );
      this.LogClass ( this._DalRecords.Log );

      this.LogMethodEnd ( "etRecord" );
      return record;

    }//END getRecord method

    // =====================================================================================
    /// <summary>
    /// This class retrieves a form object based on RecordId
    /// </summary>
    /// <param name="RecordId">String record identfier</param>
    /// <param name="IncludeComments">Boolean True = Include comments in output.</param>
    /// <returns>EvForm: a form object</returns>
    /// <remarks>
    /// This class consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the form object based on RecordId
    /// 
    /// 2. Return a form object. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvForm getRecord ( 
      String RecordId,
      bool IncludeComments )
    {
      this.LogMethod ( "GetRecord method. " );
      this.LogValue ( "RecordId: " + RecordId );
      // 
      // Initialise the method variables and objects.
      // 
      EvForm record = new EvForm ( );

      //
      // Execute the query
      //
      record = this._DalRecords.getRecord ( RecordId, IncludeComments );
      this.LogClass ( this._DalRecords.Log );

      this.LogMethodEnd ( "etRecord" );
      return record;

    }//END getRecord method

    // =====================================================================================
    /// <summary>
    /// This class retrieves a form object based on RecordId
    /// </summary>
    /// <param name="SourceId">String containing the external source identifier</param>
    /// <param name="IncludeComments">Boolean True = Include comments in output.</param>
    /// <returns>EvForm: a form object</returns>
    /// <remarks>
    /// This class consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the form object based on RecordId
    /// 
    /// 2. Return a form object. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvForm getRecordBySource (
      String SourceId,
      bool IncludeComments )
    {
      this.LogValue ( "DAL:EvRecord:getRecord method. " );
      this.LogValue ( "RecordId: " + SourceId );
      // 
      // Initialise the method variables and objects.
      // 
      EvForm record = new EvForm ( );

      //
      // Execute the query
      //
      record = this._DalRecords.getRecordBySource ( SourceId, IncludeComments );
      this.LogClass ( this._DalRecords.Log );

      return record;

    }//END getRecord method

    #endregion

    #region Lock and Unlock trial FirstSubject Save methods

    // =====================================================================================
    /// <summary>
    /// This class locks the form record for single user update.
    /// </summary>
    /// <param name="Record">EvForm: a form object</param>
    /// <returns>EvEventCodes: an event code for locking items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for locking form records. 
    /// 
    /// 2. Return an event code for locking form records.
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvEventCodes lockItem ( EvForm Record )
    {
      // 
      // Initialise method variables
      // 
      this.LogValue ( "<br/>Evado.Bll.Clinical.EvFormRecords.lockItem method. " );
      EvEventCodes iReturn = EvEventCodes.Ok;

      // 
      // Update the trial record.
      // 
      iReturn = this._DalRecords.lockRecord ( Record );
      this.LogValue ( "" + this._DalRecords.Log );
      return iReturn;

    }//END lockItem method

    // =====================================================================================
    /// <summary>
    /// This class unlocks the form record for single user update.
    /// </summary>
    /// <param name="Record">EvForm: a form object</param>
    /// <returns>EvEventCodes: an event code for locking items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for unlocking form records. 
    /// 
    /// 2. Return an event code for locking form records.
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvEventCodes unlockItem ( EvForm Record )
    {
      // 
      // Initialise method variables
      // 
      this.LogValue ( "<br/>Evado.Bll.Clinical.EvFormRecords.unlockItem method. " );
      EvEventCodes iReturn = EvEventCodes.Ok;
      // 
      // Update the trial record.
      // 
      iReturn = this._DalRecords.unlockRecord ( Record );
      this.LogValue ( "" + this._DalRecords.Log );
      return iReturn;

    }//END unlockItem method

    #endregion

    #region Form Record create mandatory record methods

    // =====================================================================================
    /// <summary>
    /// This method creates all of the records associated with a visit tp database.
    /// 
    /// It is used for visits that have one activity and one record.
    /// </summary>
    /// <param name="SubectMilestone">EvMilestone: milestone milestone object.</param>
    /// <param name="UserProfile">EvUserProfile: user profile object.</param>
    /// <returns>Integer: an event code for creating new form object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the mandatory activity object
    /// 
    /// 2. Execute the method for retrieving the list of activity record objects 
    /// based on the mandatory activity object
    /// 
    /// 3. Loop through the list and update values to the new form object. 
    /// 
    /// 4. Execute the method for creating new form object to database. 
    /// 
    /// 5. Return an event code for creating new form object
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public int createAllMandatoryVisitRecords ( EvMilestone SubectMilestone, Evado.Model.Digital.EvUserProfile UserProfile )
    {
      this.LogValue ( "Evado.Bll.EvFormRecords.createAllMandatoryVisitRecords method." );
      this.LogValue ( " TrialId: " + SubectMilestone.ProjectId
        + ", OrgId: " + SubectMilestone.OrgId
        + ", SubjectId: " + SubectMilestone.SubjectId
        + ", VisitId: " + SubectMilestone.VisitId
        + ", MilestoneId: " + SubectMilestone.MilestoneId
        + ", Activity count: " + SubectMilestone.ActivityList.Count );

      //
      // Initialise the methods variables and objects.
      //
      List<EvActvityForm> activityFormList = new List<EvActvityForm> ( );
      EvForm newFormRecord = new EvForm ( );
      EvActivity mandatoryActivity = getMandatoryActivity ( SubectMilestone.ActivityList );

      //
      // If the return value is null then there is not mandatory activity so exit the method.
      //
      if ( mandatoryActivity == null )
      {
        this.LogValue ( "No mandatory activities found for this milestone." );
        return ( int ) EvEventCodes.Object_Activity_Error;
      }

      //
      // get the list of forms for the mandatory activity.
      //
      activityFormList = this.getActivityForms ( mandatoryActivity );

      this.LogValue ( "Activity form list count:" + activityFormList.Count + "\r\n" );

      //
      // Iterate through the list creating the mandatory form records.
      //
      foreach ( EvActvityForm activityRecord in activityFormList )
      {
        //
        // Skip optional records.
        //
        if ( activityRecord.Mandatory == false )
        {
          continue;
        }

        //
        // Initialise the form record to be generated.
        //
        newFormRecord = new EvForm ( );
        newFormRecord.TrialId = SubectMilestone.ProjectId;
        newFormRecord.OrgId = SubectMilestone.OrgId;
        newFormRecord.SubjectId = SubectMilestone.SubjectId;
        newFormRecord.VisitId = SubectMilestone.VisitId;
        newFormRecord.MilestoneId = SubectMilestone.MilestoneId;
        newFormRecord.ActivityId = mandatoryActivity.ActivityId;
        newFormRecord.IsMandatoryActivity = mandatoryActivity.IsMandatory;
        newFormRecord.FormId = activityRecord.FormId;
        newFormRecord.IsMandatory = activityRecord.Mandatory;
        newFormRecord.UpdatedByUserId = UserProfile.UserId;
        newFormRecord.UserCommonName = UserProfile.CommonName;

        //
        // Submit the initialised record to be created.
        //
        EvForm createdRecord = this.createRecord ( newFormRecord );

        if ( createdRecord.Guid == Guid.Empty )
        {
          this.LogValue ( "\r\nRecord creation failed.\r\n" );

          return ( int ) EvEventCodes.Database_Record_Update_Error;
        }

        this.LogValue ( "Record: " + createdRecord.RecordId + " created." );

      }//END Record creation iteration loop.

      return ( int ) EvEventCodes.Ok;
    }//END createAllMandatoryVisitRecords class

    // =====================================================================================
    /// <summary>
    /// This method finds the first mandatory activity and returns it to the calling method.
    /// </summary>
    /// <param name="ActivityList">List of EvActivity: list of activitie objects.</param>
    /// <returns>EvActivity: an activity object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Loop through the list of activity objects and return mandatory activity object, if it is found
    /// 
    /// 2. Else, return null. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private EvActivity getMandatoryActivity ( List<EvActivity> ActivityList )
    {
      //
      // Iterate through the list of activities looking for the first mandatory activity.
      //
      foreach ( EvActivity activity in ActivityList )
      {
        //
        // If the mandatory activity is found return the activity.
        //
        if ( activity.IsMandatory == true )
        {
          return activity;
        }
      }//END iteration loop

      //
      // No activity is found return null.
      return null;

    }//END getMandatoryActivity method

    // =====================================================================================
    /// <summary>
    /// This method creates a list of Activity record objects based on the passed activity object.
    /// </summary>
    /// <param name="Activity">EvActivity: the activity object.</param>
    /// <returns>List of EvActivityRecord: a list of activity record objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Return the form list of activity, if the passed activity object contain a form list. 
    /// 
    /// 2. Else, execute the method for retrieving a list of activity record objects. 
    /// 
    /// 3. Return the list of activity record objects.
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private List<EvActvityForm> getActivityForms ( EvActivity Activity )
    {
      //
      // If the list exists return it.
      //
      if ( Activity.FormList.Count > 0 )
      {
        return Activity.FormList;
      }

      //
      //Initialise methods variables and objects.
      //
      EvActivityForms activityForms = new EvActivityForms ( );

      //
      // Get the list of EvActivityRecord objects for this activity.
      //
      return activityForms.getList ( Activity.Guid );

    }//END getActivityForms method

    #endregion

    #region Form Record update methods

    // =====================================================================================
    /// <summary>
    /// This class creates new form record to database. 
    /// </summary>
    /// <param name="Record">EvForm: The form object</param>
    /// <returns>EvForm: a form object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the form object's identifier values are empty. 
    /// 
    /// 2. If the form QueryType is 'Updateable' then create a new copy of the record.
    /// else create a new empty record.
    /// 
    /// 3. Return the form object. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvForm createRecord ( EvForm Record )
    {
      this.LogMethod ( "createRecord method." );
      this.LogDebug ( "ProjectId: " + Record.TrialId );
      this.LogDebug ( "OrgId: " + Record.OrgId );
      this.LogDebug ( "MilestoneId: " + Record.MilestoneId );
      this.LogDebug ( "ActivityId: " + Record.ActivityId );
      this.LogDebug ( "SubjectId: " + Record.SubjectId );
      this.LogDebug ( "VisitId: " + Record.VisitId );
      this.LogDebug ( "FormId: " + Record.FormId );

      // 
      // Instantiate the local variables
      //
      EvForm formRecord = new EvForm ( );

      // 
      // Check that the ResultData object has valid identifiers to add it to the database.
      //
      if ( Record.TrialId == String.Empty )
      {
        this.LogValue ( " Trial Empty " );
        Record.EventCode = EvEventCodes.Identifier_Project_Id_Error;
        this.LogMethodEnd ( "createRecord" );
        return Record;
      }
      if ( Record.OrgId == String.Empty )
      {
        this.LogValue ( " OrgId Empty " );
        Record.EventCode = EvEventCodes.Identifier_Org_Id_Error;
        return Record;
      }

      if ( Record.MilestoneId == String.Empty )
      {
        this.LogValue ( " MilestoneId Empty " );
        Record.EventCode = EvEventCodes.Identifier_Milestone_Id_Error;
        this.LogMethodEnd ( "createRecord" );
        return Record;
      }

      if ( Record.ActivityId == String.Empty )
      {
        this.LogValue ( " ActivityId Empty " );
        Record.EventCode = EvEventCodes.Identifier_Activity_Id_Error;
        this.LogMethodEnd ( "createRecord" );
        return Record;
      }

      if ( Record.FormId == String.Empty )
      {
        this.LogValue ( " FormId Empty " );
        formRecord.EventCode = EvEventCodes.Identifier_Form_Id_Error;
        this.LogMethodEnd ( "createRecord" );
        return formRecord;
      }

      if ( Record.SubjectId == String.Empty )
      {
        this.LogValue ( " SubjectId Empty " );
        Record.EventCode = EvEventCodes.Identifier_Subject_Id_Error;
        this.LogMethodEnd ( "createRecord" );
        return Record;
      }

      //
      // Retrieve the specified form to determine the form QueryType.
      //
      EvForm form = this._DalForms.getForm ( Record.TrialId, Record.FormId, true );

      this.LogDebugClass ( this._DalForms.Log );
      this.LogDebug ( "form ProjectId: " + form.TrialId );
      this.LogDebug ( "form Id: " + form.FormId );
      this.LogDebug ( "form title: " + form.Title );
      this.LogDebug ( "form TypeId: " + form.Design.TypeId );

      //
      // If the form QueryType is 'Updateable' then create a new copy of the record.
      // else create a new empty record.
      //
      if ( form.Design.TypeId == EvFormRecordTypes.Updatable_Record )
      {
        // 
        // Create a new copy of the record.
        // 
        this.LogDebug ( "UPDATEABLE RECORD: Create New Record." );
        formRecord = this._DalRecords.createNewUpdateableRecord ( Record );
        this.LogClass ( this._DalRecords.Log );

        this.LogDebug ( "Record Event Code: " + formRecord.EventCode
          + " > " + Evado.Model.Digital.EvcStatics.Enumerations.enumValueToString ( formRecord.EventCode ) );

        // 
        // Return the new record.
        // 
        this.LogMethodEnd ( "createRecord" );
        return formRecord;

      }//END Updateable form QueryType. 

      // 
      // Create a new trial Report to the database
      // 
      this.LogDebug ( "Create New Record." );
      formRecord = this._DalRecords.createRecord ( Record );
      this.LogClass ( this._DalRecords.Log );

      this.LogDebug ( "Record Event Code: " + Evado.Model.Digital.EvcStatics.Enumerations.enumValueToString ( formRecord.EventCode ) );

      // 
      // Return the new record.
      // 
      this.LogMethodEnd ( "createRecord" );
      return formRecord;

    }//END createRecord method

    // =====================================================================================
    /// <summary>
    /// This class saves form records to the database. 
    /// </summary>
    /// <param name="FormRecord">EvRForm: The trial form record object</param>
    /// <param name="HadEditAccess">EvRole: a user role object</param>
    /// <returns>EvEventCodes: an event code for saving form records</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the form object's identifiers are empty. 
    /// 
    /// 2. If a review query has been raised, process the query.
    /// 
    /// 3. Execute the method for updating form's state, closing form's alert and processing formfields
    /// 
    /// 4. Return an event code of method execution. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvEventCodes saveRecord (
      EvForm FormRecord,
      bool HadEditAccess )
    {
      this.LogMethod ( "saveItem method. " );
      this.LogValue ( "FormRecord.Guid: " + FormRecord.Guid );
      this.LogValue ( "FormGuid: " + FormRecord.FormGuid );
      this.LogValue ( "TrialId: " + FormRecord.TrialId );
      this.LogValue ( "SubjectId: " + FormRecord.SubjectId );
      this.LogValue ( "Action: " + FormRecord.SaveAction );
      this.LogValue ( "UserRole.Edit: " + HadEditAccess );
      this.LogValue ( "hasQueredItems: " + FormRecord.hasQueredItems );
      // 
      // Define the local variables.
      // 
      EvEventCodes iReturn = EvEventCodes.Ok;

      // 
      // Check that the ResultData object has valid identifiers to add it to the database.
      // 
      if ( FormRecord.Guid == Guid.Empty )
      {
        this.LogValue ( "Record Guid is empty" );
        return EvEventCodes.Identifier_Global_Unique_Identifier_Error;
      }
      if ( FormRecord.FormGuid == Guid.Empty )
      {
        this.LogValue ( "Form ID is empty" );
        return EvEventCodes.Identifier_Form_Id_Error;
      }
      if ( FormRecord.TrialId == String.Empty )
      {
        this.LogValue ( "Project ID is empty" );
        return EvEventCodes.Identifier_Project_Id_Error;
      }
      if ( FormRecord.OrgId == String.Empty )
      {
        this.LogValue ( "Org ID is empty" );
        return EvEventCodes.Identifier_Org_Id_Error;
      }
      if ( FormRecord.SubjectId == String.Empty )
      {
        this.LogValue ( "Subject ID is empty" );
        return EvEventCodes.Identifier_Subject_Id_Error;
      }


      // 
      // If a review query has been raised then process the query.
      // 
      if ( FormRecord.hasQueredItems == true
        && ( FormRecord.SaveAction == EvForm.SaveActionCodes.Review_Save
          || FormRecord.SaveAction == EvForm.SaveActionCodes.Monitor_Save
          || FormRecord.SaveAction == EvForm.SaveActionCodes.DataManager_Save ) )
      {
        this.LogValue ( "Processing a queried record." );

        return this.queryRecord ( FormRecord, HadEditAccess );

      }//END query processing

      // 
      // Update the state information in the trial Report.
      // 
      this.updateFormState ( FormRecord, HadEditAccess );

      this.LogValue ( "Status: " + FormRecord.State );

      // 
      // Update the trial record.
      // 
      iReturn = this.closeAlert ( FormRecord );
      if ( iReturn < EvEventCodes.Ok )
      {
        return iReturn;
      }

      // 
      // Update the instrument newField states.
      // 
      this.processFormFields ( FormRecord );

      // 
      // Update the trial record.
      // 
      iReturn = this._DalRecords.updateRecord ( FormRecord );
      this.LogClass ( this._DalRecords.Log );

      // 
      // error encountered return eror.
      // 
      if ( iReturn < EvEventCodes.Ok )
      {

        return iReturn;
      }

      // 
      // Return the update status.
      // 
      return iReturn;

    }//END saveRecord method.

    // =====================================================================================
    /// <summary>
    /// This class processes a review query.  By saving the currentMonth record as queried record and 
    /// add a new copy for the author to update.
    /// </summary>
    /// <param name="FormRecord">EvForm: a form object</param>
    /// <param name="HadEditAccess">EvRole: a user role object</param>
    /// <returns>EvEventCodes: an event code for processing query records</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the methods for processing the query message, updating form state and processing formfields. 
    /// 
    /// 2. Execute the method for updating the form object to database. 
    /// 
    /// 3. Execute the methods for copying the queries form records and sending form alert message. 
    /// 
    /// 4. Return an event code of method execution.
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private EvEventCodes queryRecord (
      EvForm FormRecord,
      bool HadEditAccess )
    {
      this.LogMethod ( "queryRecord method." );
      this.LogValue ( "RecordId: " + FormRecord.RecordId );

      // 
      // Define local variables.
      // 
      EvFormRecordFields recordFields = new EvFormRecordFields ( this.ClassParameter );
      EvEventCodes iReturn = EvEventCodes.Ok;
      string authoredBy = FormRecord.AuthoredBy;
      DateTime authoredByDate = FormRecord.AuthoredDate;
      string sMessage = String.Empty;

      // 
      // Extract the items that have been queried and add a comment to the 
      // annotation newField.
      // 
      sMessage = this.queryMessage ( FormRecord );
      this.LogValue ( "Message: " + sMessage );

      // 
      // Update the status.
      // 
      this.updateFormState ( FormRecord, HadEditAccess );

      // 
      // Update the instrument newField states.
      // 
      this.processFormFields ( FormRecord );

      // 
      // Save the record.
      // 
      this.LogValue ( " Updating the current record" );

      iReturn = this._DalRecords.updateRecord ( FormRecord );

      this.LogClass ( this._DalRecords.Log );

      if ( iReturn < EvEventCodes.Ok )
      {
        this.LogValue ( "Errors Saving RecordId: " + FormRecord.RecordId );
        return iReturn;
      }

      // 
      // copy the record .
      // 
      iReturn = this._DalRecords.copyQueriedRecord ( FormRecord );
      this.LogClass ( this._DalRecords.Log );

      // 
      // Process error event outcome
      // 
      if ( iReturn < EvEventCodes.Ok )
      {
        return iReturn;
      }

      // 
      // Indicate the copy process is finished.
      // 
      this.LogValue ( "Finished Copying Items" );

      // 
      // Raise an alert to the author role.
      // 
      iReturn = this.sendAlert ( FormRecord, sMessage, EvAlert.AlertTypes.Trial_Record );

      // 
      // Return the update status.
      // 
      return iReturn;

    }//END queryRecord method.

    //  =============================================================================== 
    /// <summary>
    /// This method identifies the items that have been queried and builds a query 
    /// message for inclusion in the annotation instrument newField.
    /// </summary>
    /// <param name="FormRecord">EvForm: a form object.</param>
    /// <returns>string: a query message string</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Set the comment author. 
    /// 
    /// 2. Loop through the formRecord object's fields and add the queries state fields to the query message. 
    /// 
    /// 3. Return the query message. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private string queryMessage ( EvForm FormRecord )
    {
      this.LogMethod ( "queryMessage method " );
      // 
      // Initialise the methods variables and objects.
      // 
      bool queriedFields = false;

      String stMessage = "Record reviewed by " +FormRecord.UserCommonName+ " on " + DateTime.Now.ToString ( "dd MMM yyyy" )
        + "\r\nThe following items values have been queried:";

      //
      // Set the comment author value.
      //
      EvFormRecordComment.AuthorTypeCodes authorType = EvFormRecordComment.AuthorTypeCodes.Monitor;

      if ( FormRecord.SaveAction == EvForm.SaveActionCodes.DataManager_Save )
      {
        authorType = EvFormRecordComment.AuthorTypeCodes.Data_Manager;
      }

      // 
      // Iterate through the items append those items that have been queried.
      // 
      foreach ( EvFormField field in FormRecord.Fields )
      {
        string fieldMessage = String.Empty;
        //
        // Select queried fields.
        //
        if ( field.State == EvFormField.FieldStates.Queried )
        {
          queriedFields = true;
          //
          // Output field title and value for non-tabular or freetext fields.
          //
          if ( field.TypeId != EvDataTypes.Table
            && field.TypeId != EvDataTypes.Special_Matrix
            && field.TypeId != EvDataTypes.Free_Text )
          {
            fieldMessage = "\r\n" + field.Design.Title + " = " + field.ItemValue;
          }
          else
          {
            fieldMessage = "\r\n" + field.Design.Title;

          }//END field output

          stMessage += fieldMessage;

        }//END select queried fields.

      }//END field iteration loop

      // 
      // Does the message have a value if so process the field.
      //
      if ( queriedFields == true )
      {
        //
        // Create the comment object.
        //
        EvFormRecordComment comment = new EvFormRecordComment (
          FormRecord.Guid,
          authorType,
          FormRecord.UpdatedByUserId,
          FormRecord.UserCommonName,
          stMessage );

        //
        // append the comment object to the commentlist.
        //
        FormRecord.CommentList.Add ( comment );

      } //END value has content

      // 
      // Place footer on the message
      // 
      stMessage += "\r\n\r\n" + EvLabels.Record_Resubmit_Text;

      // 
      // Return the message.
      // 
      return stMessage;

    }//END queryMessage method.

    // =====================================================================================
    /// <summary>
    /// This class creates an alert when a query is raised.
    /// </summary>
    /// <param name="FormRecord">EvForm: a form object</param>
    /// <param name="Message">string: a query message string</param>
    /// <param name="Alert">EvAlert.AlertTypes: an alert QueryType object</param>
    /// <returns>EvEventCodes: an event code for sending alert message</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Update the form object's values to the alert object.
    /// 
    /// 2. Execute the method for saving the alert object to database. 
    /// 
    /// 3. Return an event code for saving alert object to database.
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private EvEventCodes sendAlert (
      EvForm FormRecord,
      String Message,
      EvAlert.AlertTypes Alert )
    {
      this.LogMethod ( "sendAlert method. " );
      this.LogValue ( "RecordId: " + FormRecord.RecordId );
      this.LogValue ( "UserName: " + FormRecord.UserCommonName );

      // 
      // Define local variables.
      // 
      EvEventCodes iReturn = EvEventCodes.Ok;
      EvAlert trialAlert = new EvAlert ( );

      // 
      // Initialise the alert object properties
      // 
      trialAlert.ProjectId = FormRecord.TrialId;
      trialAlert.ToOrgId = FormRecord.OrgId;
      trialAlert.RecordId = FormRecord.RecordId;

      trialAlert.Subject = String.Format (
        EvLabels.Alert_Record_Queried_Subject_Text,
        FormRecord.RecordId,
        FormRecord.SubjectId,
        trialAlert.FromUser,
        DateTime.Now.ToString ( "dd MMM yyyy" ) );
      trialAlert.NewMessage = Message;
      trialAlert.UserCommonName = FormRecord.UserCommonName;
      trialAlert.ToUser = FormRecord.AuthoredBy;
      trialAlert.FromUser = FormRecord.UserCommonName;
      trialAlert.Action = EvAlert.AlertSaveActionCodes.Raise_Alert;
      trialAlert.TypeId = Alert;

      // 
      // Save the alert to the database.
      // 
      iReturn = this._BllTrialAlerts.saveAlert ( trialAlert );
      this.LogValue ( ( this._BllTrialAlerts.Log ) );

      // 
      // Process the errer exceptions
      // 
      if ( iReturn < EvEventCodes.Ok )
      {
        return iReturn;
      }

      this.LogValue ( "Completed Send Alert Processing" );

      // 
      // Return the update status.
      // 
      return iReturn;

    }//END sendAlert method.

    // =====================================================================================
    /// <summary>
    /// This class closes the event alert for this record if it exists. 
    /// </summary>
    /// <param name="FormRecord">EvForm: a form object</param>
    /// <returns>EvEventCodes: an event code for closing alert message</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. if the form action is "Submit", execute the method for closing alert message. 
    /// 
    /// 2. Return an event code for closing alert message.
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private EvEventCodes closeAlert ( EvForm FormRecord )
    {
      this.LogMethod ( "closeAlert method. " );
      this.LogValue ( "RecordId: " + FormRecord.RecordId );
      this.LogValue ( "UserName: " + FormRecord.UserCommonName );
      this.LogValue ( "Action: " + FormRecord.SaveAction );

      // 
      // Initialise local variables
      // 
      EvEventCodes iReturn = EvEventCodes.Ok;

      // 
      // Close alert if record being signed be authoredBy. 
      // 
      if ( FormRecord.SaveAction == EvForm.SaveActionCodes.Submit_Record )
      {
        this.LogValue ( " Closing alerts. " );
        iReturn = this._BllTrialAlerts.CloseAlert (
           FormRecord.RecordId,
           FormRecord.UserCommonName );

        this.LogValue ( "" + this._BllTrialAlerts.Log );

        // 
        // Process the exception events
        // 
        if ( iReturn != EvEventCodes.Ok )
        {
          return iReturn;
        }
      }

      // 
      // Add the status message for completing the close event process.
      // 
      this.LogValue ( "Completed Close Alert Processing" );

      // 
      // Return the update status.
      // 
      return iReturn;

    }//END closeAlert method.

    #endregion

    #region Form Record state update

    // =====================================================================================
    /// <summary>
    /// This class updates the form Record state and approve records for the FirstSubject.
    /// </summary>
    /// <param name="FormRecord">EvForm: a form object.</param>
    /// <param name="HadEditAccess">EvRole: a user role object</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Update the authenticated user, if it exists. 
    /// 
    /// 2. Update the form state based on the associated form object's values. 
    /// 
    /// 3. Execute the associated methods for processing sign off object. 
    /// 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private void updateFormState (
      EvForm FormRecord,
      bool HadEditAccess )
    {
      this.LogMethod ( "updateState method. " );
      this.LogValue ( "RecordId: " + FormRecord.RecordId );
      this.LogValue ( "Action: " + FormRecord.SaveAction );
      //
      // Initialise the methods variables and objects.
      //

      // 
      // If the instrument has an authenticated signoff pass the user id to the 
      // to the DAL layer and DB.
      // 
      string AuthenticatedUserId = String.Empty;
      if ( FormRecord.IsAuthenticatedSignature == true )
      {
        AuthenticatedUserId = FormRecord.UpdatedByUserId;
      }
      this.LogValue ( " AuthenticatedUserId: " + AuthenticatedUserId );

      // 
      // Set the query state if not defined in the database reset if correctly.
      // 
      if ( FormRecord.QueryState == EvForm.QueryStates.Null
        && FormRecord.QueriedBy != String.Empty )
      {
        FormRecord.QueryState = EvForm.QueryStates.Open;

        if ( FormRecord.State != EvFormObjectStates.Queried_Record )
        {
          FormRecord.QueryState = EvForm.QueryStates.Closed;
        }
      }//END query state not set correctly.

      // 
      // Save the trial record to the database.
      // 
      // If state is null set it to created.
      // 
      if ( FormRecord.State == EvFormObjectStates.Null )
      {
        FormRecord.State = EvFormObjectStates.Empty_Record;
      }

      // 
      // If the Author edits the record reset the review and approval states.
      // 
      if ( ( HadEditAccess == true )
        && ( FormRecord.SaveAction == EvForm.SaveActionCodes.Save_Record )
        && ( FormRecord.State == EvFormObjectStates.Submitted_Record
          || FormRecord.State == EvFormObjectStates.Source_Data_Verified ) )
      {
        this.setDraftRecordStatus ( FormRecord );

        return;

      }//END reset record status

      // 
      // Perform author signoff of the record and save it to the database.
      // 
      if ( ( HadEditAccess == true )
        && ( FormRecord.SaveAction == EvForm.SaveActionCodes.Submit_Record ) )
      {
        this.LogValue ( "Author submitting a record." );

        this.submitRecordSignoff ( FormRecord, AuthenticatedUserId );

        // 
        // Close the query state if the record is signed off by the author.
        // 
        if ( FormRecord.QueryState == EvForm.QueryStates.Open )
        {
          FormRecord.QueryState = EvForm.QueryStates.Closed;
        }
      }

      // 
      // trial record has been queried, set the state to queried and save the 
      // record to the database.  
      // A new copy will be created of the record for followup action.
      // 
      if ( ( FormRecord.SaveAction == EvForm.SaveActionCodes.Monitor_Save
          || FormRecord.SaveAction == EvForm.SaveActionCodes.Review_Save
          || FormRecord.SaveAction == EvForm.SaveActionCodes.DataManager_Save )
        && ( FormRecord.hasQueredItems == true ) )
      {
        this.LogValue ( "Reviewer query signoff" );

        this.queryRecordAction ( FormRecord, AuthenticatedUserId );

        return;
      }


      // 
      // Perform the monitor signoff and save the record to the database.
      // 
      if ( FormRecord.SaveAction == EvForm.SaveActionCodes.Monitor_Save
        && FormRecord.Monitor == String.Empty )
      {
        this.LogValue ( "Monitor signoff" );

        this.montorSubmitAction ( FormRecord, AuthenticatedUserId );

        return;
      }

      // 
      // Perform the reviewer signoff and save the record to the database.
      // 
      if ( FormRecord.SaveAction == EvForm.SaveActionCodes.Review_Save
        && FormRecord.ReviewedBy == String.Empty )
      {
        this.LogValue ( "Reviewer signoff" );

        this.reviewSubmitAction ( FormRecord, AuthenticatedUserId );

        return;
      }

      // 
      // Perform the final approval signoff and save the record to the database.
      // 
      if ( FormRecord.SaveAction == EvForm.SaveActionCodes.DataManager_Save
        && FormRecord.ApprovedBy == String.Empty )
      {
        this.LogValue ( "Data manager signoff" );

        this.dataManagerSubmitAction ( FormRecord, AuthenticatedUserId );

        return;
      }

      // 
      // Perform withdrawn of the trial record and save it to the database.
      // 
      if ( FormRecord.SaveAction == EvForm.SaveActionCodes.Withdrawn_Record
        && ( FormRecord.State == EvFormObjectStates.Empty_Record
          || FormRecord.State == EvFormObjectStates.Draft_Record
          || FormRecord.State == EvFormObjectStates.Completed_Record ) )
      {
        this.LogValue ( " Withdrawn Record." );
        FormRecord.State = EvFormObjectStates.Withdrawn;

        return;
      }

    }//END updateState method.

    // =====================================================================================
    /// <summary>
    /// This method performs the set the record values for the update state.
    /// </summary>
    /// <param name="Record">EvForm object: the form object.</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Reset the form object to default values and update the state to draft. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private void setDraftRecordStatus ( EvForm Record )
    {

      this.LogMethod ( "setDraftRecordStatus method." );
      Record.AuthoredBy = String.Empty;
      Record.AuthoredByUserId = String.Empty;
      Record.AuthoredDate = Evado.Model.Digital.EvcStatics.CONST_DATE_NULL;
      Record.ReviewedBy = String.Empty;
      Record.ReviewedByUserId = String.Empty;
      Record.ReviewedDate = Evado.Model.Digital.EvcStatics.CONST_DATE_NULL;
      Record.Monitor = String.Empty;
      Record.MonitorUserId = String.Empty;
      Record.MonitorDate = Evado.Model.Digital.EvcStatics.CONST_DATE_NULL;
      Record.ApprovedBy = String.Empty;
      Record.ApprovedByUserId = String.Empty;
      Record.ApprovalDate = Evado.Model.Digital.EvcStatics.CONST_DATE_NULL;
      Record.SignoffStatement = String.Empty;

      Record.State = EvFormObjectStates.Draft_Record;

    }//END setUpdateRecordState method. 

    // =====================================================================================
    /// <summary>
    /// This method creates the submitted sign off object.
    /// </summary>
    /// <param name="Record">EvForm: a form object.</param>
    /// <param name="AuthenticatedUserId">String: the users authenticated identifier.</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Reset the form object to default values and a state to be submitted
    /// 
    /// 2. Update the form object's values to the sign off object. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private void submitRecordSignoff ( 
      EvForm Record, 
      String AuthenticatedUserId )
    {
      this.LogMethod ( "submitRecordSignoff method." );
      // 
      // Initialise the local variables
      // 
      EvUserSignoff userSignoff = new EvUserSignoff ( );

      Record.State = EvFormObjectStates.Submitted_Record;
      Record.AuthoredByUserId = AuthenticatedUserId;
      Record.AuthoredBy = Record.UserCommonName;
      Record.AuthoredDate = DateTime.Now;
      Record.RecordDate = Record.AuthoredDate;
      Record.ReviewedBy = String.Empty;
      Record.ReviewedByUserId = String.Empty;
      Record.ReviewedDate = Evado.Model.Digital.EvcStatics.CONST_DATE_NULL;
      Record.Monitor = String.Empty;
      Record.MonitorUserId = String.Empty;
      Record.MonitorDate = Evado.Model.Digital.EvcStatics.CONST_DATE_NULL;
      Record.ApprovedBy = String.Empty;
      Record.ApprovedByUserId = String.Empty;
      Record.ApprovalDate = Evado.Model.Digital.EvcStatics.CONST_DATE_NULL;
      Record.SignoffStatement = String.Empty;
      Record.SignoffStatement = String.Empty;

      // 
      // Append the signoff object.
      // 
      userSignoff.Type = EvUserSignoff.TypeCode.Record_Submitted_Signoff;
      userSignoff.SignedOffUserId = AuthenticatedUserId;
      userSignoff.SignedOffBy = Record.UserCommonName;
      userSignoff.SignOffDate = Record.AuthoredDate;

      Record.RecordContent.Signoffs.Add ( userSignoff );

    }//END createSignoff method 

    // =====================================================================================
    /// <summary>
    /// This method creates the queried sign off object.
    /// </summary>
    /// <param name="Record">EvForm: a form object.</param>
    /// <param name="AuthenticatedUserId">String: the users authenticated identifier.</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Update the form object to the queried ResultData. 
    /// 
    /// 2. Update the form object's values to the sign off object. 
    /// 
    /// 3. Reset the sign off ResultData. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private void queryRecordAction (
      EvForm Record,
      String AuthenticatedUserId )
    {
      this.LogMethod ( "queryRecordAction method." );
      // 
      // Initialise the local variables
      // 
      EvUserSignoff userSignoff = new EvUserSignoff ( );

      Record.State = EvFormObjectStates.Queried_Record;
      Record.QueriedByUserId = AuthenticatedUserId;
      Record.QueriedBy = Record.UserCommonName;
      Record.QueriedDate = DateTime.Now;
      Record.QueryState = EvForm.QueryStates.Open;

      // 
      // Append the signoff object.
      // 
      userSignoff.Type = EvUserSignoff.TypeCode.Record_Reviewer_Query_Signoff;
      userSignoff.SignedOffUserId = AuthenticatedUserId;
      userSignoff.SignedOffBy = Record.UserCommonName;
      userSignoff.SignOffDate = Record.QueriedDate;

      Record.RecordContent.Signoffs.Add ( userSignoff );

      // 
      // Reset the signoff ResultData
      // 
      Record.ReviewedBy = String.Empty;
      Record.ReviewedByUserId = String.Empty;
      Record.ReviewedDate = Evado.Model.Digital.EvcStatics.CONST_DATE_NULL;
      Record.ApprovedBy = String.Empty;
      Record.ApprovedByUserId = String.Empty;
      Record.ApprovalDate = Evado.Model.Digital.EvcStatics.CONST_DATE_NULL;
      Record.SignoffStatement = String.Empty;


    }//END querySignoff method 

    // =====================================================================================
    /// <summary>
    /// This method creates the reviewed sign off object.
    /// </summary>
    /// <param name="Record">EvForm: a form object.</param>
    /// <param name="AuthenticatedUserId">String: the users authenticated identifier.</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Update the form object with the review ResultData
    /// 
    /// 2. Update the form object's values to the sign off object. 
    /// 
    /// 3. Reset the sign off ResultData. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private void reviewSubmitAction ( EvForm Record, string AuthenticatedUserId )
    {
      this.LogMethod ( "reviewSubmitAction method." );
      // 
      // Initialise the local variables
      // 
      EvUserSignoff userSignoff = new EvUserSignoff ( );

      Record.ReviewedByUserId = AuthenticatedUserId;
      Record.ReviewedBy = Record.UserCommonName;
      Record.ReviewedDate = DateTime.Now;
      Record.SignoffStatement = String.Empty;

      // 
      // Append the signoff object.
      // 
      userSignoff.Type = EvUserSignoff.TypeCode.Record_Source_Data_Verified;
      userSignoff.SignedOffUserId = AuthenticatedUserId;
      userSignoff.SignedOffBy = Record.UserCommonName;
      userSignoff.SignOffDate = Record.ReviewedDate;

      Record.RecordContent.Signoffs.Add ( userSignoff );

    }//END reviewSignoff method 

    // =====================================================================================
    /// <summary>
    /// This method creates the monitored sign off object.
    /// </summary>
    /// <param name="Record">EvForm: a form object.</param>
    /// <param name="AuthenticatedUserId">String: the users authenticated identifier.</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Update the form object with the monitored ResultData
    /// 
    /// 2. Update the form object's values to the sign off object. 
    /// 
    /// 3. Reset the sign off ResultData. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private void montorSubmitAction ( EvForm Record, string AuthenticatedUserId )
    {
      this.LogMethod ( "montorSubmitAction method." );
      // 
      // Initialise the local variables
      // 
      EvUserSignoff userSignoff = new EvUserSignoff ( );

      Record.State = EvFormObjectStates.Source_Data_Verified;
      Record.MonitorUserId = AuthenticatedUserId;
      Record.Monitor = Record.UserCommonName;
      Record.MonitorDate = DateTime.Now;

      // 
      // Append the signoff object.
      // 
      userSignoff.Type = EvUserSignoff.TypeCode.Record_Monitor_Signoff;
      userSignoff.SignedOffUserId = AuthenticatedUserId;
      userSignoff.SignedOffBy = Record.UserCommonName;
      userSignoff.SignOffDate = Record.MonitorDate;
      Record.RecordContent.Signoffs.Add ( userSignoff );

    }//END monitorSignoff method 

    // =====================================================================================
    /// <summary>
    /// This method creates the completed sign off object.
    /// </summary>
    /// <param name="Record">EvForm: a form object.</param>
    /// <param name="AuthenticatedUserId">String: the users authenticated identifier.</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Update the form object with the completed ResultData
    /// 
    /// 2. Update the form object's values to the sign off object. 
    /// 
    /// 3. Reset the sign off ResultData. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private void dataManagerSubmitAction ( EvForm Record, string AuthenticatedUserId )
    {
      this.LogMethod ( "dataManagerSubmitAction method." );
      // 
      // Initialise the local variables
      // 
      EvUserSignoff userSignoff = new EvUserSignoff ( );
      Record.State = EvFormObjectStates.Locked_Record;
      Record.ApprovedByUserId = AuthenticatedUserId;
      Record.ApprovedBy = Record.UserCommonName;
      Record.ApprovalDate = DateTime.Now;

      // 
      // Append the signoff object.
      // 
      userSignoff.Type = EvUserSignoff.TypeCode.Record_DataManager_Signoff;
      userSignoff.SignedOffUserId = AuthenticatedUserId;
      userSignoff.SignedOffBy = Record.UserCommonName;
      userSignoff.SignOffDate = Record.ApprovalDate;
      Record.RecordContent.Signoffs.Add ( userSignoff );

    }//END approvalSignoff method 

    #endregion

    #region Processing FormFields

    // =====================================================================================
    /// <summary>
    /// This class processes the form fields states.  Setting their status and where appropriate
    /// signing them off.
    /// 
    /// </summary>
    /// <param name="Record">EvForm: a form object</param>
    /// <returns>List of EvFormField: a form field object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Loop through the form object's fields
    /// 
    /// 2. Update the appropriate form fields's state and action. 
    /// 
    /// 3. Execute the method to update the form field state. 
    /// 
    /// 4. Add teh formfield object to the list of formfield objects. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private List<EvFormField> processFormFields ( EvForm Record )
    {
      this.LogMethod ( "processFormFields method." );
      this.LogValue ( "Field count: " + Record.Fields.Count );
      this.LogValue ( "Form state: " + Record.State );
      this.LogValue ( "Save Action: " + Record.SaveAction );

      // 
      // Iterate through the adverse event fields and update the newField's state and action.
      // 
      for ( int count = 0; count < Record.Fields.Count; count++ )
      {
        EvFormField field = Record.Fields [ count ];
        field.UpdatedByUserId = Record.UpdatedByUserId;
        field.UserCommonName = Record.UserCommonName;

        if ( field.State == EvFormField.FieldStates.Empty
          || field.State == EvFormField.FieldStates.With_Value )
        {
          this.LogValue ( " Field Action: ActionSaveItem. " );
          field.Action = EvFormRecordFields.ActionSaveItem;
        }

        if ( field.State == EvFormField.FieldStates.With_Value
          && Record.State == EvFormObjectStates.Source_Data_Verified )
        {
          this.LogValue ( " Field Action: ActionConfirmItem. " );
          field.Action = EvFormRecordFields.ActionConfirmItem;
        }

        if ( field.State == EvFormField.FieldStates.Queried )
        {
          this.LogValue ( " Field Action: ActionQueryItem. " );
          field.Action = EvFormRecordFields.ActionQueryItem;
        }

        if ( Record.SaveAction == EvForm.SaveActionCodes.Submit_Record
          && ( field.State == EvFormField.FieldStates.Confirmed
            || field.State == EvFormField.FieldStates.Queried ) )
        {
          this.LogValue ( " Form Action: signoff query. " );
          field.State = EvFormField.FieldStates.With_Value;
          field.Action = EvFormRecordFields.ActionSaveItem;
        }

        if ( Record.SaveAction == EvForm.SaveActionCodes.Save_Record
          && ( field.State == EvFormField.FieldStates.Confirmed
            || field.State == EvFormField.FieldStates.Queried ) )
        {
          this.LogValue ( " Form Action: save item. " );
          field.State = EvFormField.FieldStates.With_Value;
          field.Action = EvFormRecordFields.ActionSaveItem;
        }

        if ( Record.SaveAction == EvForm.SaveActionCodes.DataManager_Save
          && ( field.State == EvFormField.FieldStates.Confirmed
            || field.State == EvFormField.FieldStates.With_Value ) )
        {
          this.LogValue ( " Form Action: save item. " );
          field.State = EvFormField.FieldStates.Confirmed;
          field.Action = EvFormRecordFields.ActionSaveItem;
        }

        this.LogValue ( " >> Field ID: " + field.FieldId + ", Subject: '" + field.Design.Title
          + "', Value: '" + field.ItemValue
          + "', State: '" + field.State
          + "', Action: '" + field.Action + "' " );

        // 
        // Update the newField state.
        // 
        this.updateFieldState ( field );

        // 
        // Update the newField currentSchedule value
        // 
        Record.Fields [ count ] = field;

      }//END newField iteration loop.

      // 
      // When completed return the updated newField currentSchedule.
      // 
      return Record.Fields;

    }//END processFormFields method

    // =====================================================================================
    /// <summary>
    /// This Update the newField state and approve newField state.
    /// </summary>
    /// <param name="Field">EvFormField: a form field object</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Switch the form field action and update the state. 
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private void updateFieldState ( EvFormField Field )
    {
      this.LogMethod ( "updateFieldStatemethod " );

      if ( Field.Guid == Guid.Empty )
      {
        this.LogDebug ( "Action: " + Field.Action + " >> ADD New record" );
        Field.Action = EvFormRecordFields.ActionSaveItem;
        Field.State = EvFormField.FieldStates.Empty;
        Field.AuthoredBy = Field.UserCommonName;
        Field.AuthoredDate = DateTime.Now;
      }

      switch ( Field.Action )
      {
        case EvFormRecordFields.ActionDataCleansing:
          {

            return;
          }
        case EvFormRecordFields.ActionQueryItem:
          {
            this.LogDebug ( "Action: " + Field.Action + " >> Field Queried. " );
            Field.State = EvFormField.FieldStates.Queried;
            Field.ReviewedByUserId = Field.UpdatedByUserId;
            Field.ReviewedBy = Field.UserCommonName;
            Field.ReviewedDate = DateTime.Now;

            return;
          }
        case EvFormRecordFields.ActionConfirmItem:
          {
            this.LogDebug ( "Action: " + Field.Action + " >> Field Confirmed by Reviewer." );
            Field.State = EvFormField.FieldStates.Confirmed;
            Field.ReviewedByUserId = Field.UpdatedByUserId;
            Field.ReviewedBy = Field.UserCommonName;
            Field.ReviewedDate = DateTime.Now;

            return;
          }
        default:
          {
            this.LogDebug ( "Action: " + Field.Action + " >> Data Entered/Updated " );
            if ( Field.ItemValue != String.Empty
                || Field.ItemText != String.Empty )
            {
              Field.State = EvFormField.FieldStates.With_Value;
            }
            Field.AuthoredByUserId = Field.UpdatedByUserId;
            Field.AuthoredBy = Field.UserCommonName;
            Field.AuthoredDate = DateTime.Now;

            return;
          }
      }

    }//END updateFieldState method.

    #endregion

  }//END EvFormRecords Class.

}//END namespace Evado.Bll.Clinical