/***************************************************************************************
 * <copyright file="BLL\AncilliaryRecords.cs" company="EVADO HOLDING PTY. LTD.">
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
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Text;

//Evado. namespace references.
using Evado.Model;
using Evado.Model.Digital;
//using Evado.Model.Digital;


namespace Evado.Bll.Clinical
{
  /// <summary>
  /// This business object manages the SubjectRecords in the system.
  /// </summary>
  public class EvAncillaryRecords : EvBllBase
  {
    #region Class Initialization

    // ==================================================================================
    /// <summary>
    /// This is the class initialisation method.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EvAncillaryRecords ( )
    {
      this.ClassNameSpace = "Evado.Bll.eClinical.EvAncillaryRecords.";
    }

    // ==================================================================================
    /// <summary>
    /// This is the class initialisation method with settings configured.
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EvAncillaryRecords ( EvClassParameters Settings )
    {
      this.ClassParameter = Settings;

      if ( this.ClassParameter.LoggingLevel == 0 )
      {
        this.ClassParameter.LoggingLevel = Evado.Dal.EvStaticSetting.LoggingLevel;
      }

      this._dalSubjectRecords = new Evado.Dal.Clinical.EvAncillaryRecords ( this.ClassParameter );

      this.ClassNameSpace = "Evado.Bll.eClinical.EvAncillaryRecords.";
    }
    #endregion

    #region Object Initialisation
    /// <summary>
    /// This constant defines the save action for FirstSubject record object
    /// </summary>
    public const string ACTION_SAVE = "SAVE";

    /// <summary>
    /// This constant defines the new action for FirstSubject record object
    /// </summary>
    public const string ACTION_NEW = "NEW";

    /// <summary>
    /// This constant defines the signed action for FirstSubject record object
    /// </summary>
    public const string ACTION_SIGNED = "SIGNED";

    /// <summary>
    /// This constant defines the withdraw action for FirstSubject record object
    /// </summary>
    public const string ACTION_WITHDRAW = "CANCEL";

    //
    // Create instantiate the DAL class 
    // 
    private Evado.Dal.Clinical.EvAncillaryRecords _dalSubjectRecords = new Evado.Dal.Clinical.EvAncillaryRecords ( );

    // 
    // DebugLog stores the business object status conditions.
    // 
    private StringBuilder _DebugLog = new StringBuilder ( );
    /// <summary>
    /// This property contains the debuglog string. 
    /// </summary>
    public string DebugLog
    {
      get
      {
        return _DebugLog.ToString ( );
      }
    }

    /// <summary>
    /// This property contains the Html debuglog string
    /// </summary>
    public string DebugLogHtml
    {
      get
      {
        return DebugLog.Replace ( "\r\n", "<br/>" );
      }
    }

    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region trial FirstSubject selectionList queries

    // =====================================================================================
    /// <summary>
    /// This class returns a list of FirstSubject record objects based on the passed parameters
    /// </summary>
    /// <param name="TrialId">string: (Mandatory) trial identifier</param>
    /// <param name="SubjectId">string: (Optional) milestone identifier.</param>
    /// <param name="State">string: (Optional) the milestone record state</param>
    /// <param name="OrderBy">string: (Optional) the sorting order string.</param>
    /// <returns>List of EvSubjectRecord: a list of milestone record objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the list of milestone record objects
    /// 
    /// 2. Return the list of milestone record objects
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public List<EvAncillaryRecord> getView (
      string TrialId, string SubjectId, string State, string OrderBy )
    {
      this._DebugLog.AppendLine ( "Evado.Bll.AncillaryRecords.getView method. " );

      List<EvAncillaryRecord> View = this._dalSubjectRecords.getView (
       TrialId, SubjectId, State, OrderBy );

      this._DebugLog.AppendLine ( this._dalSubjectRecords.Log );

      return View;

    }//END getMilestoneList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of options for FirstSubject record objects based on the passed parameters
    /// </summary>
    /// <param name="TrialId">string: (Mandatory) trial identifier</param>
    /// <param name="SubjectId">string: (Optional) milestone identifier.</param>
    /// <param name="useGuid">Boolean: true, if the Guid is used</param>
    /// <returns>List of EvSubjectRecord: a list of milestone record objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the list of options for milestone record objects
    /// 
    /// 2. Return the list of options for milestone record objects
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public List<EvOption> getList (
      string TrialId, string SubjectId, bool useGuid )
    {
      this._DebugLog.AppendLine ( "Evado.Bll.AncillaryRecords.getList method. " );

      List<EvOption> List = this._dalSubjectRecords.getList ( TrialId, SubjectId, useGuid );
      this._DebugLog.AppendLine ( this._dalSubjectRecords.Log );

      return List;

    }//END getList method.

    // =====================================================================================
    /// <summary>
    /// This query performs a pivot query on SubjectRecord to produce a queryState
    /// of the trial FirstSubject states.
    /// </summary>
    /// <param name="TrialId">string: (Mandatory) trial identifier.</param>
    /// <param name="SubjectId">string: (Optional) milestone identifier.</param>
    /// <returns>List of EvFormRecordSummary: a list of form record queryState objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the list of form record queryState objects
    /// 
    /// 2. Return a list of form record queryState objects. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public List<EvFormRecordSummary> getRecordSummary ( string TrialId, string SubjectId )
    {
      this._DebugLog.AppendLine ( "Evado.Bll.AncillaryRecords.getRecordSummary method. " );

      List<EvFormRecordSummary> View = this._dalSubjectRecords.getRecordSummary ( TrialId, SubjectId );

      this._DebugLog.AppendLine ( this._dalSubjectRecords.Log );

      return View;

    }//END getRecordSummary method.

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region trial FirstSubject retrieval queries

    // =====================================================================================
    /// <summary>
    /// This class retrieves the milestone record object based on Guid
    /// </summary>
    /// <param name="RecordGuid">Guid: Global unique identifier</param>
    /// <returns>EvSubjectRecord: a milestone record object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the milestone record object based on Guid. 
    /// 
    /// 2. Return the milestone record object. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvAncillaryRecord getRecord ( Guid RecordGuid )
    {
      this._DebugLog.AppendLine ( "Evado.Bll.AncillaryRecords.SubjectRecords:getRecord method. " );
      this._DebugLog.AppendLine ( "RecordGuid: " + RecordGuid );

      // 
      // Initialise the method variables and objects.
      // 
      EvAncillaryRecord item = this._dalSubjectRecords.getRecord ( RecordGuid );
      this._DebugLog.AppendLine ( this._dalSubjectRecords.Log );

      //
      // Return newField
      //
      return item;

    }//END getRecord method

    // =====================================================================================
    /// <summary>
    /// This class retrieves the milestone record object based on RecordId
    /// </summary>
    /// <param name="RecordId">string: a record identifier</param>
    /// <returns>EvSubjectRecord: a milestone record object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the milestone record object based on RecordId. 
    /// 
    /// 2. Return the milestone record object. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvAncillaryRecord getRecord ( string RecordId )
    {
      this._DebugLog.AppendLine ( "Evado.Bll.AncillaryRecords.SubjectRecords:getRecord method." );
      this._DebugLog.AppendLine ( "RecordId: " + RecordId );
      // 
      // Initialise the method variables and objects.
      // 
      EvAncillaryRecord item = new EvAncillaryRecord ( );

      //
      // Execute the query
      //
      item = this._dalSubjectRecords.getRecord ( RecordId );
      this._DebugLog.AppendLine ( this._dalSubjectRecords.Log );

      //
      // Return newField
      //
      return item;

    }//END getRecord method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region trial FirstSubject Save methods

    // =====================================================================================
    /// <summary>
    /// This class processes the save items on FirstSubject record ResultData table
    /// </summary>
    /// <param name="AncillaryRecord">EvSubjectRecord: a milestone record object</param>
    /// <returns>EvEventCodes: an event code for processing the save items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the VisitId or SubjectId or Guid is empty.
    /// 
    /// 2. Execute the method for adding items, if the action code is new
    /// 
    /// 3. Else, execute the method for updating items. 
    /// 
    /// 4. Return an event code of method execution.
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvEventCodes saveItem ( EvAncillaryRecord AncillaryRecord )
    {
      // 
      // Instantiate the local variables
      // 
      this._DebugLog.AppendLine ( "Evado.Bll.AncillaryRecords.saveItem method. " );
      this._DebugLog.AppendLine ( "Guid: " + AncillaryRecord.Guid );
      this._DebugLog.AppendLine ( "ProjectId: " + AncillaryRecord.ProjectId );
      this._DebugLog.AppendLine ( "SubjectId: " + AncillaryRecord.SubjectId );
      this._DebugLog.AppendLine ( "Action: " + AncillaryRecord.Action );

      // 
      // Exit, if the VisitId or SubjectId or Guid is empty.
      // 
      if ( AncillaryRecord.ProjectId == String.Empty )
      {
        return EvEventCodes.Identifier_Project_Id_Error;
      }

      if ( AncillaryRecord.SubjectId == String.Empty )
      {
        return EvEventCodes.Identifier_Subject_Id_Error;
      }

      // 
      // Add a new trial Report to the database
      // 
      if ( AncillaryRecord.Action == EvAncillaryRecords.ACTION_NEW
        || AncillaryRecord.Guid == Guid.Empty )
      {
        this._DebugLog.AppendLine ( "Add New Record." );
        return addRecord ( AncillaryRecord );
      }

      // 
      // If there is NO Xml content then update the trial Report properties.
      // 
      this._DebugLog.AppendLine ( "Update Record." );
      return updateItem ( AncillaryRecord );

    } // Close method saveSubjectRecord

    // =====================================================================================
    /// <summary>
    /// This class adds new items on FirstSubject record ResultData table. 
    /// </summary>
    /// <param name="AncillaryRecord">EvSubjectRecord: a subjecyt record object</param>
    /// <returns>EvEventCodes: an event code for adding items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for adding items to milestone record ResultData table. 
    /// 
    /// 2. Return an event code for adding items. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private EvEventCodes addRecord ( EvAncillaryRecord AncillaryRecord )
    {
      this._DebugLog.AppendLine ( "Evado.Bll.AncillaryRecords. AddRecord method" );
      // 
      // Define local variables.
      // 
      EvEventCodes iReturn = EvEventCodes.Ok;

      // 
      // Update the state information in the trial Report.
      // 
      this.updateState ( AncillaryRecord );

      // 
      // Add the new trial record to the database.
      // 
      iReturn = this._dalSubjectRecords.addItem ( AncillaryRecord );
      this._DebugLog.AppendLine ( this._dalSubjectRecords.Log );

      // 
      // Return the update status.
      // 
      return iReturn;

    }//END addRecord method.

    // =====================================================================================
    /// <summary>
    /// This class updates items to FirstSubject record ResultData table. 
    /// </summary>
    /// <param name="AncillaryRecord">EvSubjectRecord: a subjecyt record object</param>
    /// <returns>EvEventCodes: an event code for adding items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for updating items to milestone record ResultData table. 
    /// 
    /// 2. Return an event code for updating items. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private EvEventCodes updateItem ( EvAncillaryRecord AncillaryRecord )
    {
      // 
      // Define the local variables.
      // 
      EvEventCodes iReturn = EvEventCodes.Ok;
      this._DebugLog.AppendLine ( "Evado.Bll.AncillaryRecords.UpdateItem method." );

      // 
      // Update the state information in the trial Report.
      // 
      this.updateState ( AncillaryRecord );

      // 
      // Update the trial record.
      // 
      iReturn = this._dalSubjectRecords.updateItem ( AncillaryRecord );
      this._DebugLog.AppendLine ( this._dalSubjectRecords.Log );

      if ( iReturn < EvEventCodes.Ok )
      {
        return iReturn;
      }

      // 
      // Return the update status.
      // 
      return iReturn;

    }//END updateItem method.

    // =====================================================================================
    /// <summary>
    /// This class locks items to a single user update
    /// </summary>
    /// <param name="AncillaryRecord">EvSubjectRecord: a subjecyt record object</param>
    /// <returns>EvEventCodes: an event code for adding items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for locking items 
    /// 
    /// 2. Return an event code for locking items. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvEventCodes lockItem ( EvAncillaryRecord AncillaryRecord )
    {
      // 
      // Initialise method variables
      // 
      this._DebugLog.AppendLine ( "Evado.Bll.AncillaryRecords.lockItem method. " );
      EvEventCodes iReturn = EvEventCodes.Ok;

      // 
      // Update the trial record.
      // 
      iReturn = this._dalSubjectRecords.LockItem ( AncillaryRecord );
      this._DebugLog.AppendLine ( this._dalSubjectRecords.Log );

      return iReturn;

    }//END lockItem method

    // =====================================================================================
    /// <summary>
    /// This class unlocks items to a single user update
    /// </summary>
    /// <param name="AncillaryRecord">EvSubjectRecord: a subjecyt record object</param>
    /// <returns>EvEventCodes: an event code for adding items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for unlocking items 
    /// 
    /// 2. Return an event code for unlocking items. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvEventCodes unlockItem ( EvAncillaryRecord AncillaryRecord )
    {
      // 
      // Initialise method variables
      // 
      this._DebugLog.AppendLine ( "Evado.Bll.AncillaryRecords.unlockItem method. " );
      EvEventCodes iReturn = EvEventCodes.Ok;
      // 
      // Update the trial record.
      // 
      iReturn = this._dalSubjectRecords.UnlockItem ( AncillaryRecord );
      this._DebugLog.AppendLine ( this._dalSubjectRecords.Log );
      return iReturn;

    }//END unlockItem method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Ancillary Record state update

    // =====================================================================================
    /// <summary>
    /// This class updates the Record state and approve records for the Record.
    /// </summary>
    /// <param name="AncillaryRecord">EvSubjectRecord: a milestone record object</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Update the Authenticated userId, if it exists. 
    /// 
    /// 2. Update the SubjectRecord object and userSignoff object 
    /// based on the milestone record's state and action
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private void updateState ( EvAncillaryRecord AncillaryRecord )
    {
      this._DebugLog.AppendLine ( "Evado.Bll.AncillaryRecords.updateState method. " );
      this._DebugLog.AppendLine ( "Action: " + AncillaryRecord.Action );

      // 
      // Define the local variables.
      // 
      string dt = DateTime.Now.ToString ( "MMM dd yyyy" );
      EvUserSignoff userSignoff = new EvUserSignoff ( );

      //
      // IF the signoff object is null then initialise it.
      //
      if ( AncillaryRecord.Signoffs == null )
      {
        AncillaryRecord.Signoffs = new List<EvUserSignoff> ( );
      }

      // 
      // If the instrument has an authenticated signoff pass the user id to the 
      // to the DAL layer and DB.
      // 
      string AuthenticatedUserId = String.Empty;
      if ( AncillaryRecord.IsAuthenticatedSignature == true )
      {
        AuthenticatedUserId = AncillaryRecord.UpdatedByUserId;
      }

      // 
      // Save the trial record to the database.
      // 
      // If state is null set it to created.
      // 
      if ( AncillaryRecord.State == EvFormObjectStates.Null )
      {
        AncillaryRecord.State = EvFormObjectStates.Draft_Record;
      }

      // 
      // If the record state is created then reset the approveal properties.
      // 
      if ( AncillaryRecord.State == EvFormObjectStates.Draft_Record )
      {
        AncillaryRecord.Reviewer = String.Empty;
        AncillaryRecord.ReviewDate = Evado.Model.Digital.EvcStatics.CONST_DATE_NULL;
        AncillaryRecord.Approver = String.Empty;
        AncillaryRecord.ApprovalDate = Evado.Model.Digital.EvcStatics.CONST_DATE_NULL;
      }

      // 
      // Perform author signoff of the record and save it to the database.
      // 
      if ( AncillaryRecord.Action == EvAncillaryRecords.ACTION_SIGNED )
      {
        this._DebugLog.AppendLine ( "Researcher Signoff record." );
        AncillaryRecord.State = EvFormObjectStates.Submitted_Record;
        AncillaryRecord.Researcher = AncillaryRecord.UserCommonName;
        AncillaryRecord.ResearcherUserId = AuthenticatedUserId;
        AncillaryRecord.ResearcherDate = DateTime.Now;

        userSignoff.Type = EvUserSignoff.TypeCode.Record_Author_Signoff;
        userSignoff.SignedOffUserId = AuthenticatedUserId;
        userSignoff.SignedOffBy = AncillaryRecord.UserCommonName;
        userSignoff.SignOffDate = AncillaryRecord.ResearcherDate;
        AncillaryRecord.Signoffs.Add ( userSignoff );



        return;
      }

      // 
      // Perform withdrawn of the trial record and save it to the database.
      // 
      if ( AncillaryRecord.Action == EvAncillaryRecords.ACTION_WITHDRAW
        && AncillaryRecord.Reviewer == String.Empty
        && AncillaryRecord.Approver == String.Empty )
      {
        this._DebugLog.AppendLine ( "Withdrawn Record." );
        AncillaryRecord.State = EvFormObjectStates.Withdrawn;

        return;
      }

    }//END updateState method.

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    // ++++++++++++++++++++++++++++++++++++++  END CLASS CODE  +++++++++++++++++++++++++++++++++

  }//END AncillaryRecords Class.

}//END namespace Evado.Bll.Clinical
