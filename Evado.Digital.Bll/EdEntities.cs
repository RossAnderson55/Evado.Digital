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
  public class EdEntities : EvBllBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EdEntities ( )
    {
      this.ClassNameSpace = "Evado.Digital.Bll.Digital.EdEntities.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EdEntities ( EvClassParameters Settings )
    {
      this.ClassParameter = Settings;
      this.ClassNameSpace = "Evado.Digital.Bll.Digital.EdEntities.";

      this._DalEntities = new  Evado.Digital.Dal.EdEntities ( Settings );

      this._DalLayouts = new  Evado.Digital.Dal.EdEntityLayouts ( Settings );
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
    private  Evado.Digital.Dal.EdEntities _DalEntities = new  Evado.Digital.Dal.EdEntities ( );
    private  Evado.Digital.Dal.EdEntityLayouts _DalLayouts = new  Evado.Digital.Dal.EdEntityLayouts ( );


    #endregion

    #region entity List queries

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
    public int geyEntityCount (
      EdQueryParameters QueryParameters )
    {
      this.LogMethod ( "geyEntityCount" );
      this.LogValue ( "EdQueryParameters parameters." );
      this.LogValue ( "- FormId: " + QueryParameters.LayoutId );
      this.LogValue ( "- IncludeRecordFields: " + QueryParameters.IncludeRecordValues );
      this.LogValue ( "- States.Count: " + QueryParameters.States.Count );
      this.LogValue ( "- NotSelectedState: " + QueryParameters.NotSelectedState );
      this.LogValue ( "- RecordRangeStart: " + QueryParameters.ResultStartRange );
      this.LogValue ( "- RecordRangeFinish: " + QueryParameters.ResultFinishRange );
      // 
      // Execute the query.
      // 
      int inResultCount = this._DalEntities.getRecordCount ( QueryParameters );

      this.LogClass ( this._DalEntities.Log );

      this.LogMethodEnd ( "geyEntityCount" );
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
    public List<EdRecord> GetEntityList (
      EdQueryParameters QueryParameters )
    {
      this.LogMethod ( "GetEntityList" );
      this.LogDebug ( "Parameters." );
      this.LogValue ( "-LayoutId: " + QueryParameters.LayoutId );
      this.LogValue ( "-IncludeRecordValues: " + QueryParameters.IncludeRecordValues );
      this.LogValue ( "-States.Count: " + QueryParameters.States.Count );
      this.LogValue ( "-ParentType: " + QueryParameters.ParentType );
      /*
      this.LogDebug ( "-ParentUserId: {0}", QueryParameters.ParentUserId );
      this.LogDebug ( "-ParentOrgId: {0}", QueryParameters.ParentOrgId );
      this.LogDebug ( "-ParentGuid: {0}", QueryParameters.ParentGuid );
      this.LogDebug ( "-Org_City: {0}", QueryParameters.Org_City );
      this.LogDebug ( "-Org_Country: {0}", QueryParameters.Org_Country );
      this.LogValue ( "-NotSelectedState: " + QueryParameters.NotSelectedState );
      foreach ( EdRecordObjectStates state in QueryParameters.States )
      {
        this.LogDebug ( "-State: " + state );
      }
      this.LogDebug ( "-NotSelectedState: " + QueryParameters.SelectionFilters.Length );

      for ( int i = 0; i < QueryParameters.SelectionFilters.Length; i++ )
      {
        if ( QueryParameters.SelectionFilters [ i ] != null )
        {
          this.LogDebug ( "-Filter: {0} Value: {1}.  ", i, QueryParameters.SelectionFilters [ i ] );
        }
      }
      */
      // 
      // Execute the query.
      // 
      List<EdRecord> entityList = this._DalEntities.getEntityList ( QueryParameters );

      this.LogClass ( this._DalEntities.Log );

      //
      // Return selectionList to UI
      //
      this.LogMethodEnd ( "GetEntityList" );
      return entityList;

    }//END getRecordList method.


    // =====================================================================================
    /// <summary>
    /// This class returns a list of form object based on VisitId, VisitId, FormId and state
    /// </summary>
    /// <param name="Entity">Entity Object.</param>
    /// <returns>List of EdRecord  objects</returns>
    //  ----------------------------------------------------------------------------------
    public List<EdRecord> getChildEntityList ( EdRecord Entity )
    {
      this.LogMethod ( "getChildEntityList" );
      this.LogDebug ( "LayoutId {0}.", Entity.LayoutId );
      this.LogDebug ( "EntityId {0}.", Entity.EntityId );
      //
      // Initialize the debuglog, a return list of form object and a formRecord field object. 
      //
      List<EdRecord> entityList = new List<EdRecord> ( );
      // 
      // Execute the query.
      // 
      entityList = this._DalEntities.getChildEntityList ( Entity );

      this.LogClass ( this._DalEntities.Log );

      this.LogMethodEnd ( "getChildEntityList" );
      return entityList;

    }//END getEntityList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of option for selected form objects based on query object and ByUid condition. 
    /// </summary>
    /// <param name="QueryParameters">EvQueryParameters: a query parameter object</param>
    /// <param name="UseGuid">Boolean: true, if the list is selected by Uid</param>
    /// <returns>List of EvOption: a list of option objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the list of option objects based on query object and ByUid condition. 
    /// 
    /// 2. Return the list of option objects. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> GetOptionList (
      EdQueryParameters QueryParameters,
      bool UseGuid )
    {
      this.LogMethod ( "getOptionList" );

      List<EvOption> List = this._DalEntities.getOptionList ( QueryParameters, UseGuid );

      this.LogClass ( this._DalEntities.Log );

      this.LogMethodEnd ( "getOptionList" );
      return List;
    }//END GetList method.

    #endregion

    #region Entity retrieval queries

    // =====================================================================================
    /// <summary>
    /// This class retrieves a form object based on Guid
    /// </summary>
    /// <param name="RecordGuid">Guid: a form's Global unique identifier</param>
    /// <returns>EdRecord: a entitty data object.</returns>
    /// <remarks>
    /// This class consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the form object based on Guid
    /// 
    /// 2. Return a form object. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EdRecord GetEntity ( Guid RecordGuid )
    {
      this.LogMethod ( "GetEntity" );
      this.LogValue ( "RecordGuid: " + RecordGuid );

      // 
      // Initialise the method variables and objects.
      // 
      EdRecord record = new EdRecord ( );

      //
      // Execute the query
      //
      record = this._DalEntities.GetEntity ( RecordGuid );
      this.LogClass ( this._DalEntities.Log );

      this.LogMethodEnd ( "GetEntity" );
      return record;

    }//END getRecord method

    // =====================================================================================
    /// <summary>
    /// This class retrieves a form object based on RecordId
    /// </summary>
    /// <param name="EntityId">String record identifier</param>
    /// <returns>EdRecord: a entitty data object.</returns>
    /// <remarks>
    /// This class consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the form object based on RecordId
    /// 
    /// 2. Return a form object. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EdRecord GetEntity ( String EntityId )
    {
      this.LogMethod ( "GetEntity" );
      this.LogValue ( "EntityId: " + EntityId );
      // 
      // Initialise the method variables and objects.
      // 
      EdRecord entity = new EdRecord ( );

      //
      // Execute the query
      //
      entity = this._DalEntities.GetEntity ( EntityId );
      this.LogClass ( this._DalEntities.Log );

      this.LogMethodEnd ( "GetEntity" );
      return entity;

    }//END getRecord method

    // =====================================================================================
    /// <summary>
    /// This class returns the guid identifier for an entity identifier
    /// </summary>
    /// <param name="EntityId">string: (Mandatory) entity identifier.</param>
    /// <returns>Guid identifier.</returns>
    //  ----------------------------------------------------------------------------------
    public Guid GetEntityGuid ( String EntityId )
    {
      this.LogMethod ( "GetEntityGuid" );
      this.LogValue ( "EntityId: " + EntityId );
      //
      // Execute the query
      //
      var entityGuid = this._DalEntities.GetEntityGuid ( EntityId );
      this.LogClass ( this._DalEntities.Log );

      this.LogMethodEnd ( "GetEntityGuid" );
      return entityGuid;

    }//END GetEntityGuid method

    // =====================================================================================
    /// <summary>
    /// This class retrieves a form object based on RecordId
    /// </summary>
    /// <param name="SourceId">String containing the external source identifier</param>
    /// <returns>EdRecord: a entitty data object.</returns>
    /// <remarks>
    /// This class consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the form object based on RecordId
    /// 
    /// 2. Return a form object. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EdRecord GetEntityBySource (
      String SourceId )
    {
      this.LogMethod ( "GetEntityBySource" );
      this.LogValue ( "SourceId: " + SourceId );
      // 
      // Initialise the method variables and objects.
      // 
      EdRecord entity = new EdRecord ( );

      //
      // Execute the query
      //
      entity = this._DalEntities.GetEntityBySource ( SourceId );
      this.LogClass ( this._DalEntities.Log );

      this.LogMethodEnd ( "GetnEntityBySource" );
      return entity;

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
      entity = this._DalEntities.GetItemByParentGuid ( LayoutId, ParentGuid );
      this.LogClass ( this._DalEntities.Log );

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
      entity = this._DalEntities.GetItemByParentOrgId ( LayoutId, ParentOrgId );
      this.LogClass ( this._DalEntities.Log );

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
      entity = this._DalEntities.GetItemByParentUserId ( LayoutId, ParentUserId );
      this.LogClass ( this._DalEntities.Log );

      this.LogMethodEnd ( "GetItemByParentUserId" );
      return entity;

    }//END GetItemByParentOrgId method

    #endregion

    #region Lock and Unlock trial FirstSubject Save methods

    // =====================================================================================
    /// <summary>
    /// This class locks the form record for single user update.
    /// </summary>
    /// <param name="Entity">EvForm: a form object</param>
    /// <returns>EvEventCodes: an event code for locking items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for locking form records. 
    /// 
    /// 2. Return an event code for locking form records.
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvEventCodes lockItem ( EdRecord Entity )
    {
      // 
      // Initialise method variables
      // 
      this.LogMethod ( "lockItem method. " );
      EvEventCodes iReturn = EvEventCodes.Ok;

      // 
      // Update the trial record.
      // 
      iReturn = this._DalEntities.lockRecord ( Entity );
      this.LogDebugClass ( this._DalEntities.Log );
      return iReturn;

    }//END lockItem method

    // =====================================================================================
    /// <summary>
    /// This class unlocks the form record for single user update.
    /// </summary>
    /// <param name="Entity">EvForm: a form object</param>
    /// <returns>EvEventCodes: an event code for locking items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for unlocking form records. 
    /// 
    /// 2. Return an event code for locking form records.
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvEventCodes unlockItem ( EdRecord Entity )
    {
      // 
      // Initialise method variables
      // 
      this.LogMethod ( "unlockItem method. " );
      EvEventCodes iReturn = EvEventCodes.Ok;
      // 
      // Update the trial record.
      // 
      iReturn = this._DalEntities.unlockRecord ( Entity );
      this.LogDebugClass ( this._DalEntities.Log );
      return iReturn;

    }//END unlockItem method

    #endregion

    #region Form Record update methods

    // =====================================================================================
    /// <summary>
    /// This class creates new form record to database. 
    /// </summary>
    /// <param name="Entity">EvForm: The form object</param>
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
    public EdRecord CreateEntity ( EdRecord Entity )
    {
      this.LogMethod ( "CreateEntity method." );
      this.LogDebug ( "LayoutId: " + Entity.LayoutId );
      this.LogDebug ( "ParentType: " + Entity.Design.ParentType );
      this.LogDebug ( "ParentGuid: " + Entity.ParentGuid );
      this.LogDebug ( "ParentOrgId: " + Entity.ParentOrgId );
      this.LogDebug ( "ParentUserId: " + Entity.ParentUserId ); 

      // 
      // Instantiate the local variables
      //
      EdRecord record = new EdRecord ( );

      if ( Entity.LayoutId == String.Empty )
      {
        this.LogValue ( " FormId Empty " );
        record.EventCode = EvEventCodes.Identifier_Form_Id_Error;
        this.LogMethodEnd ( "createRecord" );
        return record;
      }

      // 
      // Create a new trial Report to the database
      // 
      this.LogDebug ( "Create New Record." );
      record = this._DalEntities.createRecord ( Entity );
      this.LogClass ( this._DalEntities.Log );

      this.LogDebug ( "Record Event Code: " + Evado.Digital.Model.EvcStatics.enumValueToString ( record.EventCode ) );

      // 
      // Return the new record.
      // 
      this.LogMethodEnd ( "CreateEntity" );
      return record;

    }//END createRecord method

    // =====================================================================================
    /// <summary>
    /// This class saves form records to the database. 
    /// </summary>
    /// <param name="Entity">EvRForm: The trial form record object</param>
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
    public EvEventCodes UpdateEntity (
      EdRecord Entity )
    {
      this.LogMethod ( "UpdateEntity" );
      this.LogValue ( "Entity.Guid: " + Entity.Guid );
      this.LogValue ( "LayoutGuid: " + Entity.LayoutGuid );
      this.LogValue ( "SaveAction: " + Entity.SaveAction );
      // 
      // Define the local variables.
      // 
      EvEventCodes iReturn = EvEventCodes.Ok;

      // 
      // Check that the ResultData object has valid identifiers to add it to the database.
      // 
      if ( Entity.Guid == Guid.Empty )
      {
        this.LogValue ( "Record Guid is empty" );
        return EvEventCodes.Identifier_Global_Unique_Identifier_Error;
      }
      if ( Entity.LayoutGuid == Guid.Empty )
      {
        this.LogValue ( "Form ID is empty" );
        return EvEventCodes.Identifier_Form_Id_Error;
      }

      // 
      // Update the state information in the trial Report.
      // 
      this.updateFormState ( Entity );

      this.LogValue ( "Status: " + Entity.State );

      // 
      // Update the instrument newField states.
      // 
      this.processFormFields ( Entity );

      // 
      // Update the trial record.
      // 
      iReturn = this._DalEntities.UpdateItem ( Entity );
      this.LogClass ( this._DalEntities.Log );

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
      this.LogMethodEnd ( "UpdateEntity" );
      return iReturn;

    }//END saveRecord method.

    #endregion

    #region Form Record state update

    // =====================================================================================
    /// <summary>
    /// This class updates the form Record state and approve records for the FirstSubject.
    /// </summary>
    /// <param name="Entity">EvForm: a form object.</param>
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
      EdRecord Entity )
    {
      this.LogMethod ( "updateState method. " );
      this.LogValue ( "RecordId: " + Entity.RecordId );
      this.LogValue ( "Action: " + Entity.SaveAction );
      //
      // Initialise the methods variables and objects.
      //


      // 
      // Save the trial record to the database.
      // 
      // If state is null set it to created.
      // 
      if ( Entity.State == EdRecordObjectStates.Null )
      {
        Entity.State = EdRecordObjectStates.Empty_Record;
      }

      // 
      // If the Author edits the record reset the review and approval states.
      // 
      if ( ( Entity.FormAccessRole == EdRecord.FormAccessRoles.Record_Author )
        && ( Entity.SaveAction == EdRecord.SaveActionCodes.Save_Record )
        && ( Entity.State == EdRecordObjectStates.Submitted_Record ) )
      {
        this.setDraftRecordStatus ( Entity );

        return;

      }//END reset record status

      // 
      // Perform author signoff of the record and save it to the database.
      // 
      if ( ( Entity.FormAccessRole == EdRecord.FormAccessRoles.Record_Author )
        && ( Entity.SaveAction == EdRecord.SaveActionCodes.Submit_Record ) )
      {
        this.LogValue ( "Author submitting a record." );

        this.submitRecordSignoff ( Entity );

      }

      // 
      // Perform withdrawn of the trial record and save it to the database.
      // 
      if ( Entity.SaveAction == EdRecord.SaveActionCodes.Withdrawn_Record
        && ( Entity.State == EdRecordObjectStates.Empty_Record
          || Entity.State == EdRecordObjectStates.Draft_Record
          || Entity.State == EdRecordObjectStates.Completed_Record ) )
      {
        this.LogValue ( " Withdrawn Record." );
        Entity.State = EdRecordObjectStates.Withdrawn;

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