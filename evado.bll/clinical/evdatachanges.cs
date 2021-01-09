/***************************************************************************************
 * <copyright file="bll\EvDataChanges.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2020 EVADO HOLDING PTY. LTD.  All rights reserved.
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

//Evado. namespace references.
using Evado.Model;
using Evado.Dal;
using Evado.Model.Digital;

namespace Evado.Bll.Clinical
{
  /// <summary>
  /// A business layer for the ResultData change objects.
  /// </summary>
  public class EvDataChanges: EvBllBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EvDataChanges ( )
    {
      this.ClassNameSpace = "Evado.Bll.Clinical.EvDataChanges.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EvDataChanges ( EvClassParameters Settings )
    {
      this.ClassParameter = Settings;
      this.ClassNameSpace = "Evado.Bll.Clinical.EvDataChanges.";

      this._dalChanges = new Evado.Dal.Clinical.EvDataChanges ( Settings );
    }
    #endregion

    #region Initialise the instrument variables and objects

    // 
    // Create instantiate the DAL class 
    // 
    private Evado.Dal.Clinical.EvDataChanges _dalChanges = new Evado.Dal.Clinical.EvDataChanges ( );
    /// <summary>
    /// Define the class property and state variable.
    /// </summary>
    private System.Text.StringBuilder _DebugLog = new System.Text.StringBuilder ( );

    /// <summary>
    /// This property contains the method status.
    /// </summary>
    public string DebugLog
    {
      get { return _DebugLog.ToString(); }
    }

    /// <summary>
    /// The property contains the Html status.
    /// </summary>
    public string DebugLog_Html
    {
      get { return DebugLog.Replace ( "\r\n", "<br/>" ); }
    }

    #endregion

    #region DataChange Query methods

    // =====================================================================================
    /// <summary>
    /// This class returns a list of datachange object based on the RecordGuid and tableName.
    /// </summary>
    /// <param name="RecordGuid">Guid: (Mandatory) The record's global unique identifier.</param>
    /// <param name="TableName">EvDataChange.DataChangeTableNames: the table name .</param>
    /// <returns>List of EvDataChange: a list of ResultData change objects.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the list of datachange objects based on Guid and table name
    /// 
    /// 2. Return the list of ResultData change object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvDataChange> GetDataChangeList ( 
      Guid RecordGuid, 
      EvDataChange.DataChangeTableNames TableName )
    {
      // 
      // Initialise the methods objects and variables.
      // 
      this._DebugLog.AppendLine (  Evado.Model.Digital.EvcStatics.CONST_METHOD_START
        + "Evado.Bll.Clinical.EvDataChanges.GetView" );
      this._DebugLog.AppendLine ( "RecordGuid: " + RecordGuid );
      this._DebugLog.AppendLine ( "Record Type: " + TableName );

      //
      // Query the database
      //
      List<EvDataChange> view = this._dalChanges.GetView ( RecordGuid, TableName );
      this._DebugLog.AppendLine (  this._dalChanges.Log );

      this._DebugLog.AppendLine (  "current schedule count: " + view.Count );

      return view;

    }//End getRecordList method.

    #endregion

    #region get Table name lists

    // =====================================================================================
    /// <summary>
    /// This class returns a list of record for the configuration records
    /// </summary>
    /// <param name="ProjectId">String: (Mandatory) The trial identifier.</param>
    /// <param name="VersionGuid">Guid: The version's global unique identifier</param>
    /// <param name="ArmIndex">EvTrialArm.ArmIndexes: The Arm Index</param>
    /// <param name="RecordTable">EvDataChange.DataChangeTableNames: The table name</param>
    /// <returns>List of EvOption: the list of selection options</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Switch the table name to execute the method for retrieving the associated options list. 
    /// 
    /// 2. Return the new options list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> getConfigurationRecordSelectionList ( String ProjectId,
      Guid VersionGuid,
      int ArmIndex,
      EvDataChange.DataChangeTableNames RecordTable )
    {
      // 
      // Initialise the methods objects and variables.
      // 
      this._DebugLog.AppendLine (  Evado.Model.Digital.EvcStatics.CONST_METHOD_START
        + "Evado.Bll.Clinical.EvDataChanges.getConfigurationRecordSelectionList method. " );
      this._DebugLog.AppendLine (  "ProjectId: " + ProjectId);
      this._DebugLog.AppendLine ( "ScheduleGuid: " + VersionGuid);
      this._DebugLog.AppendLine ( "ArmIndex: " + ArmIndex);
      this._DebugLog.AppendLine ( "RecordTable: " + RecordTable );

      // 
      // Fill the testReport selection list
      //
      switch ( RecordTable )
      {
        case EvDataChange.DataChangeTableNames.EvActivities:
          {
            return this.GetActivities ( ProjectId );
          }

        case EvDataChange.DataChangeTableNames.EvAlerts:
          {
            return this.GetTrialAlerts ( ProjectId );
          }

        case EvDataChange.DataChangeTableNames.EvForms:
          {
            return this.GetFormsList ( ProjectId );
          }


        case EvDataChange.DataChangeTableNames.EvOrganisations:
          {
            return this.GetOrganisationsList ( );
          }

        case EvDataChange.DataChangeTableNames.EvSchedules:
          {
            return this.GetSchedules ( ProjectId );
          }

        case EvDataChange.DataChangeTableNames.EvMilestones:
          {
            return this.GetMilestones ( VersionGuid );
          }

        case EvDataChange.DataChangeTableNames.EvUserProfiles:
          {
            return this.GetUserList ( );
          }
      }
      this._DebugLog.AppendLine (  "No matching list." );

      return new List<EvOption> ( );

    }//End getConfigurationRecordSelectionList method.

    // =====================================================================================
    /// <summary>
    /// This method gets a list of record for the record table QueryType
    /// </summary>
    /// <param name="Application">String: (Mandatory) The trial identifier.</param>
    /// <param name="SubjectId">String: the milestone identifier</param>
    /// <param name="RecordTable">EvDataChange.DataChangeTableNames: The table name</param>
    /// <returns>List of EvOption: the list of selection options</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Switch the table name and execute the method for retrieving the option list 
    /// 
    /// 2. Return the list of options. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> getRecordSelectionList ( String Application,
      String SubjectId,
      EvDataChange.DataChangeTableNames RecordTable )
    {
      // 
      // Initialise the methods objects and variables.
      // 
      this._DebugLog.AppendLine (  Evado.Model.Digital.EvcStatics.CONST_METHOD_START
        + "Evado.Bll.Clinical.EvDataChanges.getRecordSelectionList" );
      this._DebugLog.AppendLine (  "ProjectId: " + Application );
      this._DebugLog.AppendLine (  "SubjectId: " + SubjectId );
      this._DebugLog.AppendLine (  "RecordTable: " + RecordTable );

      // 
      // Fill the testReport selection list
      //
      switch ( RecordTable )
      {
        case EvDataChange.DataChangeTableNames.EvRecords:
          {
            return this.GetRecordList ( Application, SubjectId );
          }
        case EvDataChange.DataChangeTableNames.EvAncilliaryRecords:
          {
            return this.GetSubjectRecordsList ( Application, SubjectId );
          }
      }
      return new List<EvOption> ( );

    }//End getRecordSelectionList method.

    // =====================================================================================
    /// <summary>
    /// This method gets a list of record for the record table QueryType
    /// </summary>
    /// <param name="ProjectId">String: (Mandatory) The trial identifier.</param>
    /// <param name="RecordTable">EvDataChange.DataChangeTableNames: The table name</param>
    /// <returns>List of EvOption: the list of selection options</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Switch the table name and execute the method for retrieving the option list 
    /// 
    /// 2. Return the list of options. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> getItemRecordSelectionList ( 
      String ProjectId,
      EvDataChange.DataChangeTableNames RecordTable )
    {
      // 
      // Initialise the methods objects and variables.
      // 
      this._DebugLog.AppendLine (  Evado.Model.Digital.EvcStatics.CONST_METHOD_START
        + "Evado.Bll.Clinical.EvDataChanges.getItemRecordSelectionList. " );
      this._DebugLog.AppendLine (  "ProjectId: " + ProjectId );
      this._DebugLog.AppendLine (  "RecordTable: " + RecordTable );

      // 
      // Fill the testReport selection list
      //
      switch ( RecordTable )
      {
        case EvDataChange.DataChangeTableNames.EvFormFields:
          {
            return this.GetFormsList ( ProjectId );
          }
      }
      return new List<EvOption> ( );

    }//End getItemRecordSelectionList method.

    // =====================================================================================
    /// <summary>
    /// This method gets a list of record for the record table QueryType
    /// </summary>
    /// <param name="RecordGuid">Guid: (Mandatory) The record global unique identifier.</param>
    /// <param name="RecordTable">EvDataChange.DataChangeTableNames: (Mandatory) The record table name.</param>
    /// <returns>List of EvOption: a list Selection options contains the letter selections</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Switch the table name and execute the method for retrieving the option list 
    /// 
    /// 2. Return the list of options. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> getRecordItemSelectionList (
      Guid RecordGuid,
      EvDataChange.DataChangeTableNames RecordTable )
    {
      // 
      // Initialise the methods objects and variables.
      // 
      this._DebugLog.AppendLine (  Evado.Model.Digital.EvcStatics.CONST_METHOD_START
        + "Evado.Bll.Clinical.EvDataChanges.getRecordItemSelectionList " );
      this._DebugLog.AppendLine (  "RecordGuid: " + RecordGuid );
      this._DebugLog.AppendLine (  "RecordTable: " + RecordTable );

      // 
      // Fill the testReport selection list
      //
      switch ( RecordTable )
      {
        case EvDataChange.DataChangeTableNames.EvFormFields:
          {
            return this.GetFormFieldList ( RecordGuid );
          }
      }
      return new List<EvOption> ( );

    }//End getRecordItemSelectionList method.

    //  ==================================================================================
    /// <summary>
    /// This class returns a list of activities options objects. 
    /// </summary>
    /// <param name="ProjectId">String: a Project identifier</param>
    /// <returns>List of EvOption: a list of activities options</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving a list of activities
    /// 
    /// 2. Return the list of activities. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private List<EvOption> GetActivities ( String ProjectId )
    {
      EvActivities activities = new EvActivities ( );

      return activities.getOptionList ( ProjectId );
    }//END GetActivities class 


    //  ==================================================================================
    /// <summary>
    /// This class returns a list of schedule options objects. 
    /// </summary>
    /// <param name="ProjectId">String: a Project identifier</param>
    /// <returns>List of EvOption: a list of schedule options</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving a list of Schedule objects
    /// 
    /// 2. Return the list of Schedule objects. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private List<EvOption> GetSchedules ( String ProjectId )
    {
      EvSchedules schedules = new EvSchedules ( );

      return schedules.getOptionList ( ProjectId, false, true, true );
    }//END GetSchedules class

    //  ==================================================================================
    /// <summary>
    /// This class returns a list of Milestone options objects. 
    /// </summary>
    /// <param name="ScheduleGuid">Guid: a schedule global unique identifier</param>
    /// <returns>List of EvOption: a list of Milestone options</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving a list of Milestone objects
    /// 
    /// 2. Return the list of Milestone objects. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private List<EvOption> GetMilestones ( 
      Guid ScheduleGuid )
    {
       this._DebugLog.AppendLine( Evado.Model.Digital.EvcStatics.CONST_METHOD_END 
      + "Evadol.dll.eclinical.EvDataChanges.GetMilestones method." );

      EvMilestones milestones = new EvMilestones ( );

       List<EvOption> optionList = milestones.getMilestoneList ( ScheduleGuid, EvMilestone.MilestoneTypes.Null, false, false );

       this._DebugLog.AppendLine( milestones.DebugLog );

       return optionList;
    }//END GetMilestones class. 

    //  ==================================================================================
    /// <summary>
    /// This class returns a list of Form options objects
    /// </summary>
    /// <param name="TrialId">String: a trial identifier</param>
    /// <returns>List of EvOption: a list of Form options objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the form objects list. 
    /// 
    /// 2. Loop through the form object list and add object's values to the options list. 
    /// 
    /// 3. Return the options list. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private List<EvOption> GetFormsList ( string TrialId )
    {
      this._DebugLog.AppendLine ( "Evado.Bll.Clinical.EvDataChanges.GetFormsList" );
      // 
      // Initialise the methods variables.
      // 
      EdRecordLayouts forms = new EdRecordLayouts ( );
      List<EvOption> list = new List<EvOption> ( );

      // 
      // Add the blank first testReport.
      // 
      EvOption option = new EvOption ( );
      list.Add ( option );

      // 
      // Get the form objects list .
      // 
      List<EdRecord> view = forms.getLayoutList ( TrialId, EdRecordTypes.Null,
        EdRecordObjectStates.Null );

      this._DebugLog.AppendLine (  forms.Log );

      // 
      // Iterate through the form objects list. 
      // 
      foreach ( EdRecord form in view )
      {
        option = new EvOption ( form.Guid.ToString ( ),
         form.Title + " (" + form.Design.Version + " " + form.StateDesc + ")" );
        list.Add ( option );
      }

      return list;

    }//END GetFormsList method


    //  ==================================================================================
    /// <summary>
    /// This class returns a list of Organization options objects
    /// </summary>
    /// <returns>List of EvOption: a list of Organization options objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the Organization objects list. 
    /// 
    /// 2. Loop through the Organization object list and add object's values to the options list. 
    /// 
    /// 3. Return the options list. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private List<EvOption> GetOrganisationsList ( )
    {
      // 
      // Initialise the method variables and objects.
      // 
      EvOrganisations organisations = new EvOrganisations ( );
      List<EvOption> list = new List<EvOption> ( );

      // 
      // Add the blank first testReport.
      // 
      EvOption option = new EvOption ( );
      list.Add ( option );

      // 
      // Get the list of Organization objects
      // 
      List<EvOrganisation> view = organisations.getView ( );

      // 
      // Iterate through the list of Organization objects.
      // 
      foreach ( EvOrganisation org in view )
      {
        option = new EvOption ( org.Guid.ToString ( ),
         org.OrgId + " - " + org.Name );
        list.Add ( option );
      }

      return list;

    }//END GetOrganisationsList method

    //  ==================================================================================
    /// <summary>
    /// This class returns a list of Project alerts options objects. 
    /// </summary>
    /// <returns>List of EvOption: a list of Project alerts options objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving a list of Project alerts objects
    /// 
    /// 2. Return the list of Project alerts objects. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private List<EvOption> GetTrialAlerts ( string ProjectId )
    {
      EvAlerts trialAlerts = new EvAlerts ( );
      this._DebugLog.AppendLine (  "GetTrialAlerts method executed" );

      return trialAlerts.getList ( ProjectId, true ); ;
    }//END GetTrialAlerts class. 

    //  ==================================================================================
    /// <summary>
    /// This class returns a list of form options objects. 
    /// </summary>
    /// <returns>List of EvOption: a list of form options objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving a list of form objects
    /// 
    /// 2. Return the list of form objects. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private List<EvOption> GetFormList ( string ProjectId )
    {
      EdRecordLayouts forms = new EdRecordLayouts ( );
      return forms.getList ( ProjectId, EdRecordTypes.Null, EdRecordObjectStates.Form_Issued, true );
    }//END GetFormList class


    //  ==================================================================================
    /// <summary>
    /// This class returns a list of Milestone options objects
    /// </summary>
    /// <param name="ProjectId">String: a trial identifier</param>
    /// <param name="ScheduleId">int: Schedule id</param>
    /// <returns>List of EvOption: a list of Milestone options objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the Milestone objects list. 
    /// 
    /// 2. Loop through the Milestone object list and add object's values to the options list. 
    /// 
    /// 3. Return the options list. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private List<EvOption> GetMilestones ( 
      String ProjectId, 
      int ScheduleId )
    {
      this._DebugLog.AppendLine (  "Evado.Bll.Clinical.EvDataChanges.GetMilestones " );
      this._DebugLog.AppendLine (  "Trialid: " + ProjectId );
      this._DebugLog.AppendLine (  "ScheduleId: " + ScheduleId );

      // 
      // Initialise the methods variables and objects.
      // 
      List<EvOption> list = new List<EvOption> ( );
      EvMilestones trialMilestones = new EvMilestones ( );

      // 
      // Get the Milestone objects.
      // 
      List<EvMilestone> milestones = trialMilestones.getMilestoneList ( ProjectId, ScheduleId, EvMilestone.MilestoneTypes.Null, false );
      this._DebugLog.AppendLine ( trialMilestones.DebugLog );

      // 
      // Iterate through the milestone list creating the list.
      // 
      foreach ( EvMilestone milestone in milestones )
      {
        EvOption option = new EvOption ( milestone.Guid.ToString ( ),
         Evado.Model.Digital.EvcStatics.Enumerations.enumValueToString ( milestone.ScheduleId )
         + " - " + milestone.MilestoneId
         + " - " + milestone.Title + " (Ver: " + milestone.Data.CurrentVersion + ")" );

        list.Add ( option );
      }

      return list;
    }//END GetMilestones class

    //  ==================================================================================
    /// <summary>
    /// This class returns a list of user options objects. 
    /// </summary>
    /// <returns>List of EvOption: a list of user options objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving a list of user objects
    /// 
    /// 2. Return the list of user objects. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private List<EvOption> GetUserList ( )
    {
      EvUserProfiles users = new EvUserProfiles ( );
      List<EvOption> list = users.GetList ( String.Empty, true );

      return list;
    }//END GetUserList class

    //  ==================================================================================
    /// <summary>
    /// This class returns a list of record options objects. 
    /// </summary>
    /// <param name="ApplicationId">string: a Project identifier</param>
    /// <param name="SubjectId">string: a FirstSubject identifier</param>
    /// <returns>List of EvOption: a list of record options objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving a list of record objects
    /// 
    /// 2. Return the list of record objects. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private List<EvOption> GetRecordList ( string ApplicationId, string EntityId )
    {
      //
      // Initialise the method variables and objects.
      // 
      EdRecords trialRecords = new EdRecords ( );
      EdQueryParameters query = new EdQueryParameters ( ApplicationId );
      query.EntityId = EntityId;

      List<EvOption> list = trialRecords.getOptionList ( query, true );

      this._DebugLog.AppendLine ( trialRecords.Log );

      return list;

    }//END GetRecordList class. 

    //  ==================================================================================
    /// <summary>
    /// This class returns a list of milestone record options objects. 
    /// </summary>
    /// <param name="ProjectId">string: a Project identifier</param>
    /// <param name="SubjectId">string: a FirstSubject identifier</param>
    /// <returns>List of EvOption: a list of milestone record options objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving a list of milestone record objects
    /// 
    /// 2. Return the list of milestone record objects. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private List<EvOption> GetSubjectRecordsList ( string ProjectId, string SubjectId )
    {
      EvAncillaryRecords trialSubjectRecords = new EvAncillaryRecords ( );

      return trialSubjectRecords.getList ( ProjectId, SubjectId, true );
    }//END GetSubjectRecordsList class

    #endregion

    #region Unique identifers section
    /*
    //  ==================================================================================
    /// <summary>
    /// GetFormFieldList module
    /// 
    /// Description:
    ///  this method gets a list of form fields by thier Uid.
    /// 
    /// </summary>
    //  ----------------------------------------------------------------------------------
    private optionList<EvOption> GetFormFieldList ( int RecordUid )
    {
      EvFormFields formFields = new EvFormFields( );

      return formFields.GetList(RecordUid );
    }

    //  ==================================================================================
    /// <summary>
    /// GetCommonFormFieldList module
    /// 
    /// Description:
    ///  this method gets a list of form fields by thier Uid.
    /// 
    /// </summary>
    //  ----------------------------------------------------------------------------------
    private optionList<EvOption> GetCommonFormFieldList ( int RecordUid )
    {
      EvCommonFormFields commonFormFields = new EvCommonFormFields( );

      return commonFormFields.GetList( RecordUid );
    }

    //  ==================================================================================
    /// <summary>
    /// GetTestItemList module
    /// 
    /// Description:
    ///  this method gets a list of form fields by thier Uid.
    /// 
    /// </summary>
    //  ----------------------------------------------------------------------------------
    private optionList<EvOption> GetTestItemList ( int RecordUid )
    {
      this._Status += "\r\nGetTestItemList method RecordUid: " + RecordUid;

      EvTestItems testItems = new EvTestItems( );

      optionList<EvOption> list = testItems.getList( RecordUid );
      this._Status += "\r\n " + testItems.DebugLog;

      return list;
    }

    //  ==================================================================================
    /// <summary>
    /// GetTestItemList module
    /// 
    /// Description:
    ///  this method gets a list of form fields by thier Uid.
    /// 
    /// </summary>
    //  ----------------------------------------------------------------------------------
    private optionList<EvOption> GetTestReportItemList ( int RecordUid )
    {
      EvTestReportItems testReportItems = new EvTestReportItems( );

      return testReportItems.getList( RecordUid );
    }
    */
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Guid identifers section

    //  ==================================================================================
    /// <summary>
    /// This class returns a list of formfield option objects
    /// </summary>
    /// <param name="RecordGuid">Guid: a record global unqiue identifier</param>
    /// <returns>List of EvOption: a list of formfield options</returns>
    /// <remarks>
    /// This method consists of the following steps 
    /// 
    /// 1. Execute the method for retrieving a list of formfield objects
    /// 
    /// 2. Return a list of formfield objects. 
    /// 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private List<EvOption> GetFormFieldList ( Guid RecordGuid )
    {
      EdRecordFields formFields = new EdRecordFields ( );

      return formFields.GetList ( RecordGuid );
    }//END GetFormFieldList class


    #endregion

    // ================================================
    /// <summary>
    /// This class adds a data change object to the database.
    /// </summary>
    /// <param name="DataChange">EvDataChange: a datachange object</param>
    /// <returns>EvEventCodes: an event code for adding items to data change table.</returns>
    // -------------------------------------------------------------------------------------
    public EvEventCodes AddItem ( EvDataChange DataChange )
    {
      //
      // Initialize the method status and a no-items variable
      //
      this._DebugLog.AppendLine (  Evado.Model.EvStatics.CONST_METHOD_START + "Evado.Dal.Clinical.DataChanges.addItem method." );
      this._DebugLog.AppendLine (  "ProjectId: " + DataChange.TrialId );
      this._DebugLog.AppendLine (  "TypeId: " + DataChange.TableName );
      this._DebugLog.AppendLine (  "UserId: " + DataChange.UserId );
      this._DebugLog.AppendLine (  "DataChange.Items.Count count: " + DataChange.Items.Count );
      EvEventCodes result = EvEventCodes.Ok;

      result = this._dalChanges.AddItem ( DataChange );

      this._DebugLog.AppendLine (  this._dalChanges.Log );

      return result;
    }

  }//END EvDataChanges Class.

}//END namespace Evado.Evado.BLL 
