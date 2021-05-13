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
using Evado.Digital.Model;

namespace Evado.Digital.Bll
{
  /// <summary>
  /// This business object manages the EvRecords in the system.
  /// </summary>
  public class EdRecords : EvBllBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EdRecords ( )
    {
      this.ClassNameSpace = "Evado.Digital.Bll.Clinical.EvFormRecords.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EdRecords ( EvClassParameters Settings )
    {
      this.ClassParameter = Settings;
      this.ClassNameSpace = "Evado.Digital.Bll.Clinical.EvFormRecords.";

      this._DalRecords = new  Evado.Digital.Dal.EdRecords ( Settings );

      this._DalForms = new  Evado.Digital.Dal.EdRecordLayouts ( Settings );
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
    private  Evado.Digital.Dal.EdRecords _DalRecords = new  Evado.Digital.Dal.EdRecords ( );
    private  Evado.Digital.Dal.EdRecordLayouts _DalForms = new  Evado.Digital.Dal.EdRecordLayouts ( );

    //
    // Instantiate the Business Logic.
    //
    //TrialSamples BllTrialSamples = new TrialSamples();
   //private EvAlerts _BllTrialAlerts = new EvAlerts ( );

    #endregion

    #region project record List queries

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
      EdQueryParameters QueryParameters )
    {
      this.LogMethod ( "getRecordCount " );
      this.LogValue ( "EvQueryParameters parameters." );
      this.LogValue ( "- FormId: " + QueryParameters.LayoutId );
      this.LogValue ( "- IncludeRecordFields: " + QueryParameters.IncludeRecordValues );
      this.LogValue ( "- States.Count: " + QueryParameters.States.Count );
      this.LogValue ( "- NotSelectedState: " + QueryParameters.NotSelectedState );
      this.LogValue ( "- RecordRangeStart: " + QueryParameters.ResultStartRange );
      this.LogValue ( "- RecordRangeFinish: " + QueryParameters.ResultFinishRange );
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
    public List<EdRecord> getRecordList (
      EdQueryParameters QueryParameters )
    {
      this.LogMethod ( "getRecordList " );
      this.LogDebug ( "EvQueryParameters parameters." );
      this.LogValue ( "- FormId: " + QueryParameters.LayoutId );
      this.LogValue ( "- IncludeRecordFields: " + QueryParameters.IncludeRecordValues );
      this.LogValue ( "- States.Count: " + QueryParameters.States.Count );
      this.LogValue ( "- NotSelectedState: " + QueryParameters.NotSelectedState );

      if ( QueryParameters.States != null )
      {
        foreach ( EdRecordObjectStates state in QueryParameters.States )
        {
          this.LogValue ( "- State: " + state );
        }
      }
      // 
      // Execute the query.
      // 
      List<EdRecord> view = this._DalRecords.getRecordList ( QueryParameters );

      this.LogClass ( this._DalRecords.Log );

      this.LogMethodEnd ( "getRecordList" );
      //
      // Return selectionList to UI
      //
      return view;

    }//END getRecordList method.

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
      EdQueryParameters Query,
      bool ByUid )
    {
      this.LogMethod ( "getOptionList" );

      List<EvOption> List = this._DalRecords.getOptionList ( Query, ByUid );

      this.LogClass ( this._DalRecords.Log );

      return List;
    }//END GetList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of form object based on VisitId, VisitId, FormId and state
    /// </summary>
    /// <param name="Entity">Entity Object.</param>
    /// <returns>List of EdRecord  objects</returns>
    //  ----------------------------------------------------------------------------------
    public List<EdRecord> getChildRecordList ( EdRecord Entity )
    {
      this.LogMethod ( "getChildRecordList" );
      this.LogDebug ( "LayoutId {0}.", Entity.LayoutId );
      this.LogDebug ( "EntityId {0}.", Entity.EntityId );
      //
      // Initialize the debuglog, a return list of form object and a formRecord field object. 
      //
      List<EdRecord> recordList = new List<EdRecord> ( );
      // 
      // Execute the query.
      // 
      recordList = this._DalRecords.getChildRecordList ( Entity );

      this.LogClass ( this._DalRecords.Log );

      this.LogMethodEnd ( "getChildRecordList" );
      return recordList;

    }//END getChildRecordList method.

    #endregion

    #region project record retrieval queries

    // =====================================================================================
    /// <summary>
    /// This class returns the guid identifier for an entity identifier
    /// </summary>
    /// <param name="EntityId">string: (Mandatory) entity identifier.</param>
    /// <returns>Guid identifier.</returns>
    //  ----------------------------------------------------------------------------------
    public Guid GetRecordGuid ( String EntityId )
    {
      this.LogMethod ( "GetRecordGuid" );
      this.LogValue ( "EntityId: " + EntityId );
      //
      // Execute the query
      //
      var entityGuid = this._DalRecords.GetRecordGuid ( EntityId );
      this.LogClass ( this._DalRecords.Log );

      this.LogMethodEnd ( "GetRecordGuid" );
      return entityGuid;

    }//END GetEntityGuid method

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
    public EdRecord getRecord ( Guid RecordGuid )
    {
      this.LogMethod( "getRecord " );
      this.LogValue ( "RecordGuid: " + RecordGuid );

      // 
      // Initialise the method variables and objects.
      // 
      EdRecord record = new EdRecord ( );

      //
      // Execute the query
      //
      record = this._DalRecords.getRecord ( RecordGuid );
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
    public EdRecord getRecord ( String RecordId )
    {
      this.LogMethod ( "GetRecord " );
      this.LogValue ( "RecordId: " + RecordId );
      // 
      // Initialise the method variables and objects.
      // 
      EdRecord record = new EdRecord ( );

      //
      // Execute the query
      //
      record = this._DalRecords.getRecord ( RecordId );
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
    public EdRecord getRecordBySource (
      String SourceId )
    {
      this.LogValue ( "DAL:EvRecord:getRecord " );
      this.LogValue ( "RecordId: " + SourceId );
      // 
      // Initialise the method variables and objects.
      // 
      EdRecord record = new EdRecord ( );

      //
      // Execute the query
      //
      record = this._DalRecords.getRecordBySource ( SourceId);
      this.LogClass ( this._DalRecords.Log );

      return record;

    }//END getRecord method

    // =====================================================================================
    /// <summary>
    /// This method retrieves the entity using it layout identifier and parent Guid.
    /// </summary>
    /// <param name="LayoutId">String layout identifier</param>
    /// <param name="ParentGuid">Guid: Parent Entity Guid identifier.</param>
    /// <returns>EdRecord: a entitty data object.</returns>
    //  ----------------------------------------------------------------------------------
    public EdRecord GetItemByParentGuid (
      String LayoutId,
      Guid ParentGuid )
    {
      this.LogMethod ( "GetItemByParentGuid" );
      this.LogValue ( "SourceId: " + ParentGuid );
      // 
      // Initialise the method variables and objects.
      // 
      EdRecord entity = new EdRecord ( );

      //
      // Execute the query
      //
      entity = this._DalRecords.GetItemByParentGuid ( LayoutId, ParentGuid );
      this.LogClass ( this._DalRecords.Log );

      this.LogMethodEnd ( "GetItemByParentGuid" );
      return entity;

    }//END GetItemByParentGuid method

    // =====================================================================================
    /// <summary>
    /// This method retrieves the entity using it layout identifier and parent org id.
    /// </summary>
    /// <param name="LayoutId">String layout identifier</param>
    /// <param name="ParentOrgId">string: Parent orgnisation identifier.</param>
    /// <returns>EdRecord: a entitty data object.</returns>
    //  ----------------------------------------------------------------------------------
    public EdRecord GetItemByParentOrgId (
      String LayoutId,
      String ParentOrgId )
    {
      this.LogMethod ( "GetItemByParentOrgId" );
      this.LogValue ( "SourceId: " + ParentOrgId );
      // 
      // Initialise the method variables and objects.
      // 
      EdRecord entity = new EdRecord ( );

      //
      // Execute the query
      //
      entity = this._DalRecords.GetItemByParentOrgId ( LayoutId, ParentOrgId );
      this.LogClass ( this._DalRecords.Log );

      this.LogMethodEnd ( "GetItemByParentOrgId" );
      return entity;

    }//END GetItemByParentOrgId method

    // =====================================================================================
    /// <summary>
    /// This method retrieves the entity using it layout identifier and parent user id.
    /// </summary>
    /// <param name="LayoutId">String layout identifier</param>
    /// <param name="ParentOrgId">string: Parent user identifier.</param>
    /// <returns>EdRecord: a entitty data object.</returns>
    //  ----------------------------------------------------------------------------------
    public EdRecord GetItemByParentUserId (
      String LayoutId,
      String ParentUserId )
    {
      this.LogMethod ( "GetItemByParentUserId" );
      this.LogValue ( "SourceId: " + ParentUserId );
      // 
      // Initialise the method variables and objects.
      // 
      EdRecord entity = new EdRecord ( );

      //
      // Execute the query
      //
      entity = this._DalRecords.GetItemByParentUserId ( LayoutId, ParentUserId );
      this.LogClass ( this._DalRecords.Log );

      this.LogMethodEnd ( "_DalRecords" );
      return entity;

    }//END GetItemByParentOrgId method

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
    public EvEventCodes lockItem ( EdRecord Record )
    {
      // 
      // Initialise method variables
      // 
      this.LogValue ( "<br/>Evado.Digital.Bll.Clinical.EvFormRecords.lockItem " );
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
    public EvEventCodes unlockItem ( EdRecord Record )
    {
      // 
      // Initialise method variables
      // 
      this.LogValue ( "<br/>Evado.Digital.Bll.Clinical.EvFormRecords.unlockItem " );
      EvEventCodes iReturn = EvEventCodes.Ok;
      // 
      // Update the trial record.
      // 
      iReturn = this._DalRecords.unlockRecord ( Record );
      this.LogValue ( "" + this._DalRecords.Log );
      return iReturn;

    }//END unlockItem method

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
    public EdRecord CreateRecord ( EdRecord Record )
    {
      this.LogMethod ( "createRecord method." );
      this.LogDebug ( "LayoutId: " + Record.LayoutId );

      // 
      // Instantiate the local variables
      //
      EdRecord record = new EdRecord ( );

      if ( Record.LayoutId == String.Empty )
      {
        this.LogValue ( " FormId Empty " );
        record.EventCode = EvEventCodes.Identifier_Form_Id_Error;
        this.LogMethodEnd ( "createRecord" );
        return record;
      }

      //
      // Retrieve the specified form to determine the form QueryType.
      //
      EdRecord form = this._DalForms.GetLayout (Record.LayoutId, true );

      this.LogDebugClass ( this._DalForms.Log );
      this.LogDebug ( "form Id: " + form.LayoutId );
      this.LogDebug ( "form title: " + form.Title );
      this.LogDebug ( "form TypeId: " + form.Design.TypeId );

      //
      // If the form QueryType is 'Updateable' then create a new copy of the record.
      // else create a new empty record.
      //
      if ( form.Design.TypeId == EdRecordTypes.Updatable_Record )
      {
        // 
        // Create a new copy of the record.
        // 
        this.LogDebug ( "UPDATEABLE RECORD: Create New Record." );
        record = this._DalRecords.createNewUpdateableRecord ( Record );
        this.LogClass ( this._DalRecords.Log );

        this.LogDebug ( "Record Event Code: " + record.EventCode
          + " > " + Evado.Model.EvStatics.enumValueToString ( record.EventCode ) );

        // 
        // Return the new record.
        // 
        this.LogMethodEnd ( "createRecord" );
        return record;

      }//END Updateable form QueryType. 

      // 
      // Create a new trial Report to the database
      // 
      this.LogDebug ( "Create New Record." );
      record = this._DalRecords.createRecord ( Record );
      this.LogClass ( this._DalRecords.Log );

      this.LogDebug ( "Record Event Code: " + Evado.Model.EvStatics.enumValueToString ( record.EventCode ) );

      // 
      // Return the new record.
      // 
      this.LogMethodEnd ( "createRecord" );
      return record;

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
      EdRecord FormRecord,
      bool HadEditAccess )
    {
      this.LogMethod ( "saveItem " );
      this.LogValue ( "FormRecord.Guid: " + FormRecord.Guid );
      this.LogValue ( "FormGuid: " + FormRecord.LayoutGuid );
      this.LogValue ( "Action: " + FormRecord.SaveAction );
      this.LogValue ( "UserRole.Edit: " + HadEditAccess );
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
      if ( FormRecord.LayoutGuid == Guid.Empty )
      {
        this.LogValue ( "Form ID is empty" );
        return EvEventCodes.Identifier_Form_Id_Error;
      }

      // 
      // Update the state information in the trial Report.
      // 
      this.updateFormState ( FormRecord, HadEditAccess );

      this.LogValue ( "Status: " + FormRecord.State );

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
      EdRecord FormRecord,
      bool HadEditAccess )
    {
      this.LogMethod ( "updateState " );
      this.LogValue ( "RecordId: " + FormRecord.RecordId );
      this.LogValue ( "Action: " + FormRecord.SaveAction );
      //
      // Initialise the methods variables and objects.
      //


      // 
      // Save the trial record to the database.
      // 
      // If state is null set it to created.
      // 
      if ( FormRecord.State == EdRecordObjectStates.Null )
      {
        FormRecord.State = EdRecordObjectStates.Empty_Record;
      }

      // 
      // If the Author edits the record reset the review and approval states.
      // 
      if ( ( HadEditAccess == true )
        && ( FormRecord.SaveAction == EdRecord.SaveActionCodes.Save_Record )
        && ( FormRecord.State == EdRecordObjectStates.Submitted_Record ) )
      {
        this.setDraftRecordStatus ( FormRecord );

        return;

      }//END reset record status

      // 
      // Perform author signoff of the record and save it to the database.
      // 
      if ( ( HadEditAccess == true )
        && ( FormRecord.SaveAction == EdRecord.SaveActionCodes.Submit_Record ) )
      {
        this.LogValue ( "Author submitting a record." );

        this.submitRecordSignoff ( FormRecord );

      }

      // 
      // Perform withdrawn of the trial record and save it to the database.
      // 
      if ( FormRecord.SaveAction == EdRecord.SaveActionCodes.Withdrawn_Record
        && ( FormRecord.State == EdRecordObjectStates.Empty_Record
          || FormRecord.State == EdRecordObjectStates.Draft_Record
          || FormRecord.State == EdRecordObjectStates.Completed_Record ) )
      {
        this.LogValue ( " Withdrawn Record." );
        FormRecord.State = EdRecordObjectStates.Withdrawn;

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
    private void setDraftRecordStatus ( EdRecord Record )
    {

      this.LogMethod ( "setDraftRecordStatus method." );

      Record.State = EdRecordObjectStates.Draft_Record;

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
    private void submitRecordSignoff ( EdRecord Record )
    {
      this.LogMethod ( "submitRecordSignoff method." );
      // 
      // Initialise the local variables
      // 
      EdUserSignoff userSignoff = new EdUserSignoff ( );

      Record.State = EdRecordObjectStates.Submitted_Record;

      // 
      // Append the signoff object.
      // 
      userSignoff.Type = EdUserSignoff.TypeCode.Record_Submitted_Signoff;
      userSignoff.SignedOffUserId = this.ClassParameter.UserProfile.UserId;
      userSignoff.SignedOffBy = this.ClassParameter.UserProfile.CommonName;
      userSignoff.SignOffDate = DateTime.Now;

      Record.Signoffs.Add ( userSignoff );

    }//END createSignoff method 

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
    private List<EdRecordField> processFormFields ( EdRecord Record )
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
        EdRecordField field = Record.Fields [ count ];

        this.LogValue ( " >> Field ID: " + field.FieldId + ", Subject: '" + field.Design.Title
          + "', Value: '" + field.ItemValue );

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

    #endregion

  }//END EvFormRecords Class.

}//END namespace Evado.Digital.Bll