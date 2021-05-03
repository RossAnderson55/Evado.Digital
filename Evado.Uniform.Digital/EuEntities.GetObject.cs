/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\FormRecords.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c)  2002 - 2021  EVADO HOLDING PTY. LTD..  All rights reserved.
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
 *  This class contains the AbstractedPage ResultData object.
 *
 ****************************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.SessionState;

using Evado.Bll;
using Evado.Model;
using Evado.Bll.Digital;
using Evado.Model.Digital;
// using Evado.Web;

namespace Evado.UniForm.Digital
{
  /// <summary>
  /// This class defines the application base classs that is used to terminate the 
  /// hosted application objects.
  /// 
  /// This class terminates the Customer object.
  /// </summary>
  public partial class EuEntities : EuClassAdapterBase
  {
    #region Entity User Access Page.
    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    /// <param name="EntityId">String: Entity identifier</param>
    //  ------------------------------------------------------------------------------
    public void getObject (
      Evado.Model.UniForm.Page PageObject,
      Evado.Model.UniForm.Command PageCommand,
      String EntityId )
    {
      this.LogMethod ( "getObject" );
      try
      {
        //
        // load a new entity if it is not loaded.
        //
        if ( this.Session.Entity.EntityId != EntityId )
        {
          Guid entityGuid = this._Bll_Entities.GetEntityGuid ( EntityId );

          this.LogDebug ( "Guid {0} == EntityId {1}: ", EntityId, entityGuid );

          PageCommand.SetGuid ( entityGuid );
          //
          // Get the record.

          var result = this.GetEntity ( PageCommand );

          // 
          // if the guid is empty the parameter was not found to exit.
          // 
          if ( result != EvEventCodes.Ok )
          {
            this.ErrorMessage = EdLabels.Record_Retrieve_Error_Message;

            this.LogError ( EvEventCodes.Database_Record_Retrieval_Error, "Retrieved Record is empty." );
            this.LogMethodEnd ( "getObject" );
            return;
          }

          this.LogDebug ( "EntityDictionary count {0}.", this.Session.EntityDictionary.Count );
          this.LogValue ( "Entity.EntityId: " + this.Session.Entity.EntityId );
        }

        // 
        // If the user does not have monitor or ResultData manager roles exit the page.
        // 
        if ( this.Session.Entity.hasReadAccess ( this.Session.UserProfile.Roles ) == false )
        {
          this.LogIllegalAccess (
            this.ClassNameSpace + "getObject",
            this.Session.UserProfile );

          this.ErrorMessage = EdLabels.Record_Access_Error_Message;

          return;
        }

        //
        // reload the entity children
        //
        this.ReloadEntityChildren ( PageCommand );

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "getObject",
          this.Session.UserProfile );

        //
        // Rung the server script if server side scripts are enabled.
        //
        this.runServerScript ( EvServerPageScript.ScripEventTypes.OnOpen );

        // 
        // Generate the client ResultData object for the UniForm client.
        // 
        this.getEntityClientData ( PageObject );

        // 
        // Return the client ResultData object to the calling method.
        // 
        this.LogMethodEnd ( "getObject" );
      }
      catch ( Exception Ex )
      {
        // 
        // On an exception raised create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.Entity_Retrieve_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }


      this.LogMethodEnd ( "getObject" );
      return;

    }//END getObject method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData getObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getObject" );
      this.LogDebug ( "RefreshEntityChildren: {0}.", this.Session.RefreshEntityChildren );
      try
      {
        // 
        // Initialise the methods variables and objects.
        // 
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );

        //
        // Get the record.
        //
        var result = this.GetEntity ( PageCommand );

        //
        // add missing fields.
        //
        this.AddNewFields ( );

        //
        // reload the entity children if needed.
        //
        this.ReloadEntityChildren ( PageCommand );

        // 
        // if the guid is empty the parameter was not found to exit.
        // 
        if ( result != EvEventCodes.Ok )
        {
          this.ErrorMessage = EdLabels.Record_Retrieve_Error_Message;

          this.LogError ( EvEventCodes.Database_Record_Retrieval_Error, "Retrieved Record is empty." );
          this.LogMethodEnd ( "getObject" );
          return this.Session.LastPage;
        }

        this.LogDebug ( "EntityDictionary count {0}.", this.Session.EntityDictionary.Count );
        this.LogDebug ( "Entity.EntityId: {0}.", this.Session.Entity.EntityId );
        this.LogDebug ( "Entity.Design.ReadAccessRoles: {0}.", this.Session.Entity.Design.ReadAccessRoles );

        // 
        // If the user does not have monitor or ResultData manager roles exit the page.
        // 
        if ( this.Session.Entity.hasReadAccess ( this.Session.UserProfile.Roles ) == false )
        {
          this.LogIllegalAccess (
            this.ClassNameSpace + "getObject",
            this.Session.UserProfile );

          this.ErrorMessage = EdLabels.Record_Access_Error_Message;

          return this.Session.LastPage; ;
        }

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "getObject",
          this.Session.UserProfile );
        //
        // Rung the server script if server side scripts are enabled.
        //
        this.runServerScript ( EvServerPageScript.ScripEventTypes.OnOpen );

        // 
        // Initialise the client object.
        // 
        clientDataObject.Page.Id = clientDataObject.Id;
        clientDataObject.Page.PageDataGuid = clientDataObject.Id;
        clientDataObject.Page.PageId = this.Session.Entity.LayoutId;
        clientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Disabled;

        clientDataObject.Id = this.Session.Entity.Guid;
        clientDataObject.Title = this.Session.Entity.CommandTitle;

        if ( this.AdapterObjects.Settings.UseHomePageHeaderOnAllPages == true )
        {
          clientDataObject.Title = this.AdapterObjects.Settings.HomePageHeaderText;
        }


        if ( this.Session.Entity.Design.LinkContentSetting == EdRecord.LinkContentSetting.First_Text_Field )
        {
          clientDataObject.Title = this.Session.Entity.CommandTitle;
        }
        clientDataObject.Page.Title = clientDataObject.Title;

        // 
        // Generate the client ResultData object for the UniForm client.
        // 
        this.getEntityClientData ( clientDataObject.Page );

        // 
        // Return the client ResultData object to the calling method.
        // 
        this.LogMethodEnd ( "getObject" );
        return clientDataObject;
      }
      catch ( Exception Ex )
      {
        // 
        // On an exception raised create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.Entity_Retrieve_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }


      this.LogMethodEnd ( "getObject" );
      return this.Session.LastPage; ;

    }//END getObject method

    //  =============================================================================== 
    /// <summary>
    ///  This method retrieves the entity into memory
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    //  ---------------------------------------------------------------------------------
    private EvEventCodes GetEntity (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "GetEntity" );
      this.LogDebug ( "EntityDictionary count {0}.", this.Session.EntityDictionary.Count );

      //
      // if the page command has a layout identifier then assume parental identifier retrieval.
      //
      if ( PageCommand.hasParameter ( EdRecord.FieldNames.Layout_Id ) == true )
      {
        var result = this.GetEntityByParent ( PageCommand );

        this.LogMethodEnd ( "GetEntity" );
        return result;
      }

      //
      // Initialise the methods variables and objects.
      //
      Guid entityGuid = PageCommand.GetGuid ( );

      //
      // return a retrieval error message guid is empty.
      //
      if ( entityGuid == Guid.Empty
        && this.Session.Entity.Guid == Guid.Empty )
      {
        return EvEventCodes.Identifier_Global_Unique_Identifier_Error;
      }

      //
      // If the record Guid match then the record is loaded so exit.
      //
      if ( entityGuid == this.Session.Entity.Guid )
      {
        this.LogDebug ( "Entity Loaded" );
        this.LogMethodEnd ( "GetEntity" );

        this.Session.RefreshEntityChildren = true;

        return EvEventCodes.Ok;
      }

      //
      // Attempt to pull the entity from the entity dictionary.
      //
      this.Session.Entity = this.Session.PullEntity ( entityGuid );

      //
      // if the pull returned entity exists exit.
      //
      if ( this.Session.Entity != null )
      {
        this.Session.RefreshEntityChildren = true;

        this.LogDebug ( "Entity Loaded from dictionary" );
        this.LogMethodEnd ( "GetEntity" );
        return EvEventCodes.Ok;
      }


      // 
      // Retrieve the record object from the database via the DAL and BLL layers.
      // 
      this.Session.Entity = this._Bll_Entities.GetEntity ( entityGuid );

      this.LogClass ( this._Bll_Entities.Log );

      this.LogDebug ( "Entity {0}, Title {1}.", this.Session.Entity.RecordId, this.Session.Entity.Title );
      this.LogDebug ( "There are {0} of fields in the record.", this.Session.Entity.Fields.Count );

      //
      // return a retrieval error message if the resulting common record guid is empty.
      //
      if ( this.Session.Entity.Guid == Guid.Empty )
      {
        this.LogMethodEnd ( "GetEntity" );
        return EvEventCodes.Database_Record_Retrieval_Error;
      }

      //
      // push the new entity onto the directory list.
      //
      this.Session.PushEntity ( this.Session.Entity );

      this.Session.RefreshEntityChildren = false;

      this.LogMethodEnd ( "GetEntity" );
      return EvEventCodes.Ok;

    }//ENd GetEntity method
    
    //  =============================================================================== 
    /// <summary>
    ///  This method adds any fields that exist in the template layout but not the 
    ///  Entity.
    /// </summary>
    //  ---------------------------------------------------------------------------------
    private void AddNewFields ( )
    {
      this.LogMethod ( "AddNewFields" );
      this.LogDebug ( "Entity Field count {0}.", this.Session.Entity.Fields.Count );


      //
      // if the administrator cannot update issued object then full verisoning is 
      // operating so new template fields cannot be added to entities.
      //
      if ( this.AdapterObjects.Settings.EnableAdminUpdateOfIssuedObjects == false )
      {
        return;
      }

      this.Session.EntityLayout = this.AdapterObjects.GetEntityLayout (
        this.Session.Entity.LayoutId );
      this.LogDebug ( "Layout Field count {0}.", this.Session.EntityLayout.Fields.Count );

      //
      // iterate through the layout field looking for an existing entity field
      // and add the missing fields.
      //
      foreach ( EdRecordField field in this.Session.EntityLayout.Fields )
      {
        this.LogDebug ( "Field Guid {0}, FieldID {1}.", field.Guid, field.FieldId );
        EdRecordField entityField = this.Session.Entity.GetFieldObject ( field.FieldId );

        if ( entityField != null )
        {
          this.LogDebug ( "Field Guid {0}, FieldGuid {1}, FieldId (2).", entityField.Guid, entityField.FieldGuid, entityField.FieldId );
        }
        //
        // if the returned field is null then field was not found so add a field object 
        // to the entity to collect the relevant data.
        //
        if ( entityField == null )
        {
          this.LogDebug ( "Null Field Id {0}.", field.FieldId );
          EdRecordField newField = new EdRecordField ( );
          newField.Guid = Guid.NewGuid ( );
          newField.FieldGuid = field.FieldGuid;
          newField.LayoutGuid = field.LayoutGuid;
          newField.RecordGuid = field.RecordGuid;
          newField.FieldId = field.FieldId;
          newField.LayoutId = field.LayoutId;
          newField.RecordMedia = field.RecordMedia;
          newField.Table = field.Table;
          newField.Design = field.Design;

          this.Session.Entity.Fields.Add ( newField );
        }

      }//END layout field iteration loop

      this.LogDebug ( "NEW Entity Field count {0}.", this.Session.Entity.Fields.Count );
      this.LogMethodEnd ( "AddNewFields" );

    }//ENd ReloadEntityChildren method

    //  =============================================================================== 
    /// <summary>
    ///  This method retrieves the entity into memory
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    //  ---------------------------------------------------------------------------------
    private void ReloadEntityChildren (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "ReloadEntityChildren" );

      if ( this.Session.RefreshEntityChildren == false
        || PageCommand.Method != Model.UniForm.ApplicationMethods.Get_Object )
      {
        return;
      };
      this.LogDebug ( "Layout {0}, Entity {1}, Title {2}.",
        this.Session.Entity.LayoutId,
        this.Session.Entity.EntityId,
        this.Session.Entity.Title );

      // 
      // Retrieve the record object from the database via the DAL and BLL layers.
      // 
      this.Session.Entity.ChildEntities = this._Bll_Entities.getChildEntityList ( this.Session.Entity );

      this.LogClass ( this._Bll_Entities.Log );

      this.LogDebug ( "There are {0} of chiled entities.", this.Session.Entity.ChildEntities.Count );

      //
      // Refresh record children
      //
      EdRecords bll_Records = new EdRecords ( this.ClassParameters );

      this.Session.Entity.ChildRecords = bll_Records.getChildRecordList ( this.Session.Entity );

      this.LogClass ( bll_Records.Log );
      this.LogDebug ( "There are {0} of child records.", this.Session.Entity.ChildRecords.Count );

      this.Session.RefreshEntityChildren = false;

      this.LogMethodEnd ( "ReloadEntityChildren" );

    }//ENd ReloadEntityChildren method


    //  =============================================================================== 
    /// <summary>
    /// This method retrieves any by its LayoutId and Parent identifiers.
    /// </summary>
    //  ---------------------------------------------------------------------------------
    private EvEventCodes GetEntityByParent (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "GetEntityByParent" );
      //
      // Initialise the methods variables and objects.
      //
      String layoutId = String.Empty;
      String orgId = String.Empty;
      String userId = String.Empty;
      Guid parentGuid = Guid.Empty;

      //
      // retrieve the parent references from the command parameters.
      //
      if ( PageCommand.hasParameter ( EdRecord.FieldNames.Layout_Id ) == true )
      {
        layoutId = PageCommand.GetParameter ( EdRecord.FieldNames.Layout_Id );
      }

      //
      // There can only be one parental identifier and the organisation identifier is taking precedence
      // over the user parental identifier.
      //
      if ( PageCommand.hasParameter ( EdRecord.FieldNames.ParentOrgId ) == true )
      {
        orgId = PageCommand.GetParameter ( EdRecord.FieldNames.ParentOrgId );
      }
      else
        if ( PageCommand.hasParameter ( EdRecord.FieldNames.ParentUserId ) == true )
        {
          userId = PageCommand.GetParameter ( EdRecord.FieldNames.ParentUserId );
        }
        else
          if ( PageCommand.hasParameter ( EdRecord.FieldNames.ParentGuid ) == true )
          {
            parentGuid = PageCommand.GetParameter<Guid> ( EdRecord.FieldNames.ParentGuid );
          }

      this.LogDebug ( "LayoutId {0}, orgId {1}, userId {2} parent Guid {3}",
        layoutId, orgId, userId, parentGuid );

      //
      // return a retrieval error message guid is empty.
      //
      if ( layoutId == String.Empty
        && orgId == String.Empty
        && userId == String.Empty
        && parentGuid == Guid.Empty )
      {
        this.LogMethodEnd ( "GetEntityByParent" );
        return EvEventCodes.Identifier_General_ID_Error;
      }

      this.LogDebug ( "Retieving from Dictionary" );
      //
      // Attempt to pull the entity from the entity dictionary.
      //
      if ( parentGuid != Guid.Empty )
      {
        this.Session.Entity = this.Session.PullEntity ( layoutId, parentGuid );
      }
      else
      {
        this.Session.Entity = this.Session.PullEntity ( layoutId, orgId, userId );
      }

      //
      // if the pull returned entity exists exit.
      //
      if ( this.Session.Entity != null )
      {
        this.LogDebug ( "Entity Loaded from dictionary" );
        this.LogMethodEnd ( "GetEntityByParent" );
        return EvEventCodes.Ok;
      }

      //
      // Retrieve the record object from the database via the DAL and BLL layers.
      //
      if ( parentGuid != Guid.Empty )
      {
        this.LogDebug ( "Retrieving Entity by parental Guid" );

        this.Session.Entity = this._Bll_Entities.GetItemByParentGuid ( layoutId, parentGuid );
      }
      else
        if ( orgId != String.Empty )
        {
          this.LogDebug ( "Retrieving Entity by organisation parental identifier" );

          this.Session.Entity = this._Bll_Entities.GetItemByParentOrgId ( layoutId, orgId );
        }
        else
        {
          this.LogDebug ( "Retrieving Entity by organisation parental identifier" );

          this.Session.Entity = this._Bll_Entities.GetItemByParentUserId ( layoutId, userId );

          //
          // Create the new entity.
          //
          this.Session.Entity = this.CreateNewEntity ( layoutId, Guid.Empty );
        }

      this.LogClass ( this._Bll_Entities.Log );

      //
      // Create the new entity if non was created.
      //
      if ( this.Session.Entity.Guid == Guid.Empty )
      {
        this.Session.Entity = this.CreateNewEntity ( layoutId, parentGuid );
        this.LogDebug ( "Entity {0}, Title {1}.", this.Session.Entity.EntityId, this.Session.Entity.Title );
        this.LogDebug ( "There are {0} of fields in the record.", this.Session.Entity.Fields.Count );
      }

      //
      // return a retrieval error message if the resulting common record guid is empty.
      //
      if ( this.Session.Entity.Guid == Guid.Empty )
      {
        this.LogMethodEnd ( "GetEntityByParent" );
        return EvEventCodes.Database_Record_Retrieval_Error;
      }

      //
      // push the new entity onto the directory list.
      //
      this.Session.PushEntity ( this.Session.Entity );

      this.LogMethodEnd ( "GetEntityByParent" );
      return EvEventCodes.Ok;

    }//ENd GetEntity method

    //  =============================================================================== 
    /// <summary>
    ///  This method initiates the execution of the server side CS scripts.
    /// </summary>
    //  ---------------------------------------------------------------------------------
    private bool runServerScript (
      EvServerPageScript.ScripEventTypes ScriptType )
    {
      this.LogMethod ( "runServerScript" );
      this.LogValue ( "RecordId " + this.Session.Entity.RecordId );
      this.LogValue ( "hasCsScript = " + this.Session.Entity.Design.hasCsScript );

      // 
      // if the formField has a CS Script execute the onPostBackForm method.
      // 
      if ( this.Session.Entity.Design.hasCsScript == true )
      {
        this.LogValue ( "Server script executing." );

        //
        // Define the page to retrieve the script
        //
        this._ServerPageScript.CsScriptPath = this.AdapterObjects.ApplicationPath + @"\csScripts\";


        // 
        // Execute the onload script.
        // 
        EvEventCodes iReturn = this._ServerPageScript.runScript (
          ScriptType,
          this.Session.Entity );

        this.LogValue ( "Server page script debug log: " + this._ServerPageScript.DebugLog );

        if ( iReturn != EvEventCodes.Ok )
        {
          this.ErrorMessage =
            "CsScript:" + ScriptType + " method failed \r\n"
            + Evado.Model.EvStatics.enumValueToString ( iReturn ) + "\r\n";

          this.LogError ( EvEventCodes.Business_Logic_General_Process_Error,
            this.ErrorMessage );

          this.LogMethodEnd ( "getObject" );
          return false;

        }//END processing error return 

      }//END processing Cs formField script.

      this.LogMethodEnd ( "getObject" );
      return true;

    }//END runServerScript method

    // ==============================================================================
    /// <summary>
    /// This method creates a save groupCommand.
    /// </summary>
    /// <returns>Evado.Model.UniForm.Command object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.UniForm.Command createEntitySaveCommand (
      Guid RecordGuid,
      String SaveCommandTitle,
      String SaveAction )
    {
      this.LogMethod ( "createRecordSaveCommand" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Parameter parameter = new Evado.Model.UniForm.Parameter ( );

      // 
      // create the save groupCommand.
      // 
      Evado.Model.UniForm.Command saveCommand = new Evado.Model.UniForm.Command (
        SaveCommandTitle,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Entities.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Save_Object );

      // 
      // Define the save groupCommand parameters.
      // 
      saveCommand.SetGuid ( RecordGuid );

      saveCommand.AddParameter (
        Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
       SaveAction );

      return saveCommand;
    }

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.AppData object.</param>
    //  ------------------------------------------------------------------------------
    public void getEntityClientData (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getClientData" );
      /*
      this.LogDebug ( "UserProfile.Roles: " + this.Session.UserProfile.Roles );
      this.LogDebug ( "Entity.LayoutId: " + this.Session.Entity.LayoutId );
      this.LogDebug ( "Entity.Title: " + this.Session.Entity.Title );
      this.LogDebug ( "Entity.DefaultPageLayout: " + this.Session.Entity.Design.DefaultPageLayout );
      this.LogDebug ( "Entity.FieldReadonlyDisplayFormat: " + this.Session.Entity.Design.FieldReadonlyDisplayFormat );
      this.LogDebug ( "Entity.ReadAccessRoles: " + this.Session.Entity.Design.ReadAccessRoles );
      this.LogDebug ( "Entity.EditAccessRoles: " + this.Session.Entity.Design.EditAccessRoles );
      this.LogDebug ( "Entity.AuthorAccess: " + this.Session.Entity.Design.AuthorAccess );
      this.LogDebug ( "Entity.ParentType: " + this.Session.Entity.Design.ParentType );
      this.LogDebug ( "Entity.ParentGuid: " + this.Session.Entity.ParentGuid );
      this.LogDebug ( "Entity.ParentOrgId: " + this.Session.Entity.ParentOrgId );
      this.LogDebug ( "Entity.ParentUserId: " + this.Session.Entity.ParentUserId );
      this.LogDebug ( "EnableEntityEditButtonUpdate: " + this.EnableEntityEditButtonUpdate );
      this.LogDebug ( "ButtonEditModeEnabled: " + this.Session.Entity.ButtonEditModeEnabled );
      */

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      Evado.Model.UniForm.Parameter parameter = new Evado.Model.UniForm.Parameter ( );

      //
      // Initialise the page generator.
      //
      EuRecordGenerator pageGenerator = new EuRecordGenerator (
        this.AdapterObjects,
        this.ServiceUserProfile,
        this.Session,
        this.UniForm_BinaryFilePath,
        this.UniForm_BinaryServiceUrl,
        this.ClassParameters );

      //
      // The locked status of the record.
      //
      if ( this.checkRecordLockStatus ( PageObject ) == false )
      {
        this.LogDebug ( "The record is not locked." );

        this.getDataObject_PageCommands ( PageObject );
      }

      this.LogDebug ( "GENERATE LAYOUT" );

      //
      // Display the administrator group for the group.
      //
      if ( this.Session.UserProfile.hasAdministrationAccess == true
        && this.AdapterObjects.Settings.EnableAdminGroupOnEntityPages == true )
      {
        pageGroup = PageObject.AddGroup (
          EdLabels.Entity_Administrator_Header_Group_Title );
        pageGroup.EditAccess = Model.UniForm.EditAccess.Disabled;
        pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;

        groupField = pageGroup.createTextField (
          String.Empty,
          EdLabels.Entity_Entity_Identifier_Field_Title,
          this.Session.Entity.EntityId, 20 );

        groupField = pageGroup.createTextField (
          String.Empty,
          EdLabels.Entity_Entity_Identifier_Field_Title,
          this.Session.Entity.CommandTitle, 80 );
      }

      // 
      // Call the page generation method
      // 
      bool result = pageGenerator.generateLayout (
        this.Session.Entity,
        PageObject,
        this.UniForm_BinaryFilePath );

      this.LogClass ( pageGenerator.Log );

      //
      // Return null and an error message if the page generator exits with an error.
      //
      if ( result == false )
      {
        this.ErrorMessage = EdLabels.FormRecord_Page_Generation_Error;

        this.LogMethodEnd ( "getPatientConsentDataObject " );

        return;
      }

      //
      // generate the group commands.
      //
      this.getDataObject_GroupCommands ( PageObject );

      //
      // if the entity has child entities add a group to display them.
      //
      this.getDataObject_ChildEntities ( PageObject );

      //
      // display the record child objects.
      //
      this.getDataObject_ChildRecords ( PageObject );

      this.LogMethodEnd ( "getClientData" );

    }//END getClientData Method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_PageCommands (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getDataObject_PageCommands" );
      this.LogDebug ( "Entity.ButtonEditModeEnabled {0}. ", this.Session.Entity.ButtonEditModeEnabled );

      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );
      String stSubmitCommandTitle = EdLabels.Entity_Submit_Command;

      // 
      // Set the user access to the records content.
      // 
      this.Session.Entity.setUserAccess ( this.Session.UserProfile );
      this.LogDebug ( "Entity.FormAccessRole {0}. ", this.Session.Entity.FormAccessRole );

      if ( this.Session.Entity.FormAccessRole != EdRecord.FormAccessRoles.Record_Author )
      {
        this.LogMethodEnd ( "getDataObject_PageCommands" );
        return;
      }

      //
      // If edit access is controlled then display the command to edit the user access.
      //
      if ( this.Session.Entity.Design.AuthorAccess == EdRecord.AuthorAccessList.Edit_Access_Roles )
      {
        pageCommand = PageObject.addCommand (
          EdLabels.Entity_Edit_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Entities.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Get_Object );

        pageCommand.SetGuid ( this.Session.Entity.Guid );
        pageCommand.SetPageId ( EdStaticPageIds.Entity_User_Access_Page );
      }

      // 
      // Add the Edit command to the page.
      //
      if ( this.Session.Entity.ButtonEditModeEnabled == false
        && this.EnableEntityEditButtonUpdate == true
        && this.Session.Entity.State != EdRecordObjectStates.Empty_Record
        && this.Session.Entity.State != EdRecordObjectStates.Draft_Record )
      {
        pageCommand = PageObject.addCommand (
          EdLabels.Entity_Edit_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Entities.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Get_Object );

        pageCommand.SetGuid ( this.Session.Entity.Guid );

        pageCommand.AddParameter ( EuEntities.CONST_EDIT_MODE_FIELD, "yes" );

        this.Session.Entity.FormAccessRole = EdRecord.FormAccessRoles.Record_Reader;
      }
      else
      {
        // 
        // If the user has author access. 
        // 
        // Set the page status to edit enabled
        // 
        PageObject.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
        PageObject.DefaultGroupType = Evado.Model.UniForm.GroupTypes.Default;

        // 
        // Add the save groupCommand and add it to the page groupCommand list.
        // 
        if ( this.EnableEntitySaveButtonUpdate == true )
        {
          pageCommand = PageObject.addCommand (
            EdLabels.Entity_Save_Command_Title,
            EuAdapter.ADAPTER_ID,
            EuAdapterClasses.Entities.ToString ( ),
            Evado.Model.UniForm.ApplicationMethods.Save_Object );

          pageCommand.SetGuid ( this.Session.Entity.Guid );

          pageCommand.AddParameter (
            Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
           EdRecord.SaveActionCodes.Save_Record.ToString ( ) );
        }
        else
        {
          stSubmitCommandTitle = EdLabels.Entity_Save_Command_Title;
        }

        // 
        // Add the submit comment to the page groupCommand list.
        // 
        pageCommand = PageObject.addCommand (
          stSubmitCommandTitle,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Entities.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Save_Object );

        pageCommand.SetGuid ( this.Session.Entity.Guid );
        pageCommand.AddParameter (
          Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
         EdRecord.SaveActionCodes.Submit_Record.ToString ( ) );

        pageCommand.setEnableForMandatoryFields ( );

        // 
        // Add the wihdrawn groupCommand to the page groupCommand list.
        // 
        if ( ( this.Session.Entity.State == EdRecordObjectStates.Draft_Record
            || this.Session.Entity.State == EdRecordObjectStates.Empty_Record
            || this.Session.Entity.State == EdRecordObjectStates.Completed_Record ) )
        {
          pageCommand = PageObject.addCommand (
            EdLabels.Entity_Withdraw_Command,
            EuAdapter.ADAPTER_ID,
            EuAdapterClasses.Entities.ToString ( ),
            Evado.Model.UniForm.ApplicationMethods.Save_Object );

          pageCommand.SetGuid ( this.Session.Entity.Guid );
          pageCommand.AddParameter (
            Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
           EdRecord.SaveActionCodes.Withdrawn_Record.ToString ( ) );
        }

      }//END in edit mode.

      this.LogDebug ( "Session.Entity.FormAccessRole {0}. ", this.Session.Entity.FormAccessRole );
      this.LogMethodEnd ( "getDataObject_PageCommands" );

    }//END getClientData_SaveCommands method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_GroupCommands (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getDataObject_GroupCommands" );
      //
      // Display save command in the last group.
      //
      if ( this.Session.Entity.FormAccessRole != EdRecord.FormAccessRoles.Record_Author
        || PageObject.GroupList.Count == 0 )
      {
        this.LogMethodEnd ( "getDataObject_GroupCommands" );
        return;
      }

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );
      String stSubmitCommandTitle = EdLabels.Entity_Submit_Command;

      this.LogValue ( "Including save commands in the last group." );
      pageGroup = PageObject.GroupList [ PageObject.GroupList.Count - 1 ];

      // 
      // Add the save groupCommand and add it to the page groupCommand list.
      //  
      if ( this.EnableEntitySaveButtonUpdate == true )
      {
        pageCommand = pageGroup.addCommand (
          EdLabels.Entity_Save_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Entities.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Save_Object );

        pageCommand.SetGuid ( this.Session.Entity.Guid );

        pageCommand.AddParameter (
          Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
         EdRecord.SaveActionCodes.Save_Record.ToString ( ) );
      }
      else
      {
        stSubmitCommandTitle = EdLabels.Entity_Save_Command_Title;
      }

      // 
      // Add the submit comment to the page groupCommand list.
      // 
      pageCommand = pageGroup.addCommand (
        stSubmitCommandTitle,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Entities.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Save_Object );

      pageCommand.SetGuid ( this.Session.Entity.Guid );
      pageCommand.AddParameter (
        Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
       EdRecord.SaveActionCodes.Submit_Record.ToString ( ) );

      pageCommand.setEnableForMandatoryFields ( );

      // 
      // Add the wihdrawn groupCommand to the page groupCommand list.
      // 
      if ( ( this.Session.Entity.State == EdRecordObjectStates.Draft_Record
          || this.Session.Entity.State == EdRecordObjectStates.Empty_Record
          || this.Session.Entity.State == EdRecordObjectStates.Completed_Record ) )
      {
        pageCommand = pageGroup.addCommand (
          EdLabels.Entity_Withdraw_Command,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Entities.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Save_Object );

        pageCommand.SetGuid ( this.Session.Entity.Guid );
        pageCommand.AddParameter (
          Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
         EdRecord.SaveActionCodes.Withdrawn_Record.ToString ( ) );
      }

      this.LogMethodEnd ( "getDataObject_GroupCommands" );
    }

    private readonly string ChildEntityGroupID = "ChildEntities";

    // ==============================================================================
    /// <summary>
    /// This method displays the entity's chiled entities
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_ChildEntities (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getDataObject_ChildEntities" );
      this.LogDebug ( "DisplayRelatedEntities {0}.", this.Session.Entity.Design.DisplayRelatedEntities );
      this.LogDebug ( "Entity.ChildEntities.Count {0}.", this.Session.Entity.ChildEntities.Count );

      if ( this.Session.Entity.Design.DisplayRelatedEntities == false
        || this.Session.Entity.FormAccessRole != EdRecord.FormAccessRoles.Record_Reader )
      {
        this.LogDebug ( "No displaying related entities." );
        this.LogMethodEnd ( "getDataObject_ChildEntities" );
        return;
      }

      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.UniForm.Command groupCommand = new Evado.Model.UniForm.Command ( );
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );

      //
      // define the child entity group.
      //
      pageGroup = PageObject.AddGroup (
      String.Format ( EdLabels.Entities_Child_Entity_Group_Title, this.Session.Entity.getFirstTextField ( ) ) );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;
      pageGroup.GroupId = this.ChildEntityGroupID;

      pageGroup.CmdLayout = Model.UniForm.GroupCommandListLayouts.Vertical_Orientation;

      if ( this.Session.Entity.ChildEntities.Count <= 20 )
      {
        //pageGroup.CmdLayout = Model.UniForm.GroupCommandListLayouts.Tiled_Commands;
        //pageGroup.AddParameter ( Model.UniForm.GroupParameterList.Command_Width, "20%" );
        //pageGroup.AddParameter ( Model.UniForm.GroupParameterList.Command_Height, "50px" );
      }

      //
      // Add a create record command.
      //
      var entityChildren = this.AdapterObjects.GetEntityChildren ( this.Session.Entity.LayoutId );

      this.LogDebug ( "Entity Children count {0}. ", entityChildren.Count );

      //
      // iterate through the list of children add a create commend for each child
      // if the user has edit access to the child entity.
      //
      foreach ( EdObjectParent child in entityChildren )
      {
        this.LogDebug ( "Child roles {0} - UR {0}. ", child.ChildEditAccess, this.Session.UserProfile.Roles );
        //
        // if the user had edit access toteh entity add a create command for the entity.
        //
        if ( child.hasEditAccess( this.Session.UserProfile.Roles ) == true
          && child.IsRecord == false )
        {
          string title = String.Format ( EdLabels.Entity_New_Entity_Command_Title, child.ChildTitle );
          //
          // define the new entity command.
          //
          groupCommand = pageGroup.addCommand (
            title,
            EuAdapter.ADAPTER_ID,
            EuAdapterClasses.Entities,
            Model.UniForm.ApplicationMethods.Create_Object );

          groupCommand.AddParameter ( EdRecord.FieldNames.Layout_Id, child.ChildLayoutId );
          groupCommand.AddParameter ( EdRecord.FieldNames.ParentGuid, this.Session.Entity.Guid );
          groupCommand.SetGuid ( this.Session.Entity.Guid );

          groupCommand.SetBackgroundDefaultColour ( Model.UniForm.Background_Colours.Purple );
        }//ENd edit access
      }//END child interation loop

      //
      // Iterate through the child entities
      //
      foreach ( EdRecord child in this.Session.Entity.ChildEntities )
      {
        this.LogDebug ( "entity {0}, {1}, L {2}, LT {3}, FC {4}.",
          child.EntityId,
          child.Title,
          child.Design.LinkContentSetting,
          child.CommandTitle,
          child.Fields.Count );

        this.getChildGroupListCommand (
         child,
         pageGroup,
         child.Design.LinkContentSetting, false );

      }

      this.LogDebug ( "Command Count {0}.", pageGroup.CommandList.Count );
      this.LogMethodEnd ( "getDataObject_ChildEntities" );

    }//END getClientData_SaveCommands method


    // ==============================================================================
    /// <summary>
    /// This method displays the entity's chiled entities
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_ChildRecords (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getDataObject_ChildRecords" );
      this.LogDebug ( "DisplayRelatedEntities {0}.", this.Session.Entity.Design.DisplayRelatedEntities );
      this.LogDebug ( "Entity.ChildRecords.Count {0}.", this.Session.Entity.ChildRecords.Count );

      if ( this.Session.Entity.Design.DisplayRelatedEntities == false
        || this.Session.Entity.FormAccessRole != EdRecord.FormAccessRoles.Record_Reader )
      {
        this.LogDebug ( "No displaying related entities." );
        this.LogMethodEnd ( "getDataObject_ChildRecords" );
        return;
      }

      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.UniForm.Command groupCommand = new Evado.Model.UniForm.Command ( );
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );

      //
      // define the child entity group.
      //
      pageGroup = PageObject.AddGroup (
      String.Format ( EdLabels.Entities_Child_Entity_Group_Title, this.Session.Entity.getFirstTextField ( ) ) );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;
      pageGroup.GroupId = this.ChildEntityGroupID;

      pageGroup.CmdLayout = Model.UniForm.GroupCommandListLayouts.Vertical_Orientation;

      if ( this.Session.Entity.ChildEntities.Count <= 20 )
      {
        //pageGroup.CmdLayout = Model.UniForm.GroupCommandListLayouts.Tiled_Commands;
        //pageGroup.AddParameter ( Model.UniForm.GroupParameterList.Command_Width, "20%" );
        //pageGroup.AddParameter ( Model.UniForm.GroupParameterList.Command_Height, "50px" );
      }

      //
      // Add a create record command.
      //
      var entityChildren = this.AdapterObjects.GetEntityChildren ( this.Session.Entity.LayoutId );

      this.LogDebug ( "Entity Children count {0}. ", entityChildren.Count );

      //
      // iterate through the list of children add a create commend for each child
      // if the user has edit access to the child entity.
      //
      foreach ( EdObjectParent child in entityChildren )
      {
        this.LogDebug ( "Child roles {0} - UR {0}. ", child.ChildEditAccess, this.Session.UserProfile.Roles );
        //
        // if the user had edit access toteh entity add a create command for the entity.
        //
        if ( child.hasEditAccess ( this.Session.UserProfile.Roles ) == true
          && child.IsRecord == true )
        {
          string title = String.Format ( EdLabels.Entity_New_Entity_Command_Title, child.ChildTitle );
          //
          // define the new entity command.
          //
          groupCommand = pageGroup.addCommand (
            title,
            EuAdapter.ADAPTER_ID,
            EuAdapterClasses.Records,
            Model.UniForm.ApplicationMethods.Create_Object );

          groupCommand.AddParameter ( EdRecord.FieldNames.Layout_Id, child.ChildLayoutId );
          groupCommand.AddParameter ( EdRecord.FieldNames.ParentGuid, this.Session.Entity.Guid );
          groupCommand.SetGuid ( this.Session.Entity.Guid );

          groupCommand.SetBackgroundDefaultColour ( Model.UniForm.Background_Colours.Purple );
        }//ENd edit access
      }//END child interation loop

      //
      // Iterate through the child entities
      //
      foreach ( EdRecord child in this.Session.Entity.ChildRecords)
      {
        this.LogDebug ( "record {0}, {1}, L {2}, LT {3}, FC {4}.",
          child.EntityId,
          child.Title,
          child.Design.LinkContentSetting,
          child.CommandTitle,
          child.Fields.Count );

        this.getChildGroupListCommand (
         child,
         pageGroup,
         child.Design.LinkContentSetting, true );

      }

      this.LogDebug ( "Command Count {0}.", pageGroup.CommandList.Count );
      this.LogMethodEnd ( "getDataObject_ChildRecords" );

    }//END getDataObject_ChildRecords method

    // ==============================================================================
    /// <summary>
    /// This method appends the milestone groupCommand to the page milestone list pageMenuGroup
    /// </summary>
    /// <param name="CommandEntity">EvForm object</param>
    /// <param name="PageGroup"> Evado.Model.UniForm.Group</param>
    //  -----------------------------------------------------------------------------
    private Evado.Model.UniForm.Command getChildGroupListCommand (
      EdRecord CommandEntity,
      Evado.Model.UniForm.Group PageGroup,
      EdRecord.LinkContentSetting ParentLinkSetting,
      bool IsRecord )
    {
      this.LogMethod ( "getChildGroupListCommand" );
      this.LogDebug ( "CommandEntity.EntityId: " + CommandEntity.EntityId );
      this.LogDebug ( "LinkContentSetting: " + CommandEntity.Design.LinkContentSetting );
      this.LogDebug ( "TypeId: " + CommandEntity.TypeId );
      this.LogDebug ( "ParentLinkSetting: " + ParentLinkSetting );
      this.LogDebug ( "IsRecord: " + IsRecord );

      EuAdapterClasses adapterClass = EuAdapterClasses.Entities;
      if ( IsRecord == true )
      {
        adapterClass = EuAdapterClasses.Records;
      }

      //
      // Set the link setting.
      //
      if ( CommandEntity.Design.LinkContentSetting == EdRecord.LinkContentSetting.Null
        && ParentLinkSetting != EdRecord.LinkContentSetting.Null )
      {
        CommandEntity.Design.LinkContentSetting = ParentLinkSetting;
      }

      this.LogDebug ( "FINAL: LinkContentSetting: " + CommandEntity.Design.LinkContentSetting );
      this.LogDebug ( "CommandTitle: " + CommandEntity.CommandTitle );
      this.LogDebug ( "getFirstTextField: " + CommandEntity.getFirstTextField ( ) );

      //
      // Define the pageMenuGroup groupCommand.
      //
      Evado.Model.UniForm.Command groupCommand = PageGroup.addCommand (
          CommandEntity.CommandTitle,
          EuAdapter.ADAPTER_ID,
          adapterClass,
          Evado.Model.UniForm.ApplicationMethods.Get_Object );

      groupCommand.Id = CommandEntity.Guid;
      groupCommand.SetGuid ( CommandEntity.Guid );

      groupCommand.AddParameter (
        Model.UniForm.CommandParameters.Short_Title,
        EdLabels.Label_Record_Id + CommandEntity.RecordId );
      if ( CommandEntity.ImageFileName != String.Empty )
      {
        string relativeURL = EuAdapter.CONST_IMAGE_FILE_DIRECTORY + CommandEntity.ImageFileName;
        groupCommand.AddParameter ( Model.UniForm.CommandParameters.Image_Url, relativeURL );
      }

      if ( this.Session.UserProfile.hasAdministrationAccess == true )
      {
        groupCommand.Title = CommandEntity.EntityId + " >> " + groupCommand.Title;
      }

      return groupCommand;

    }//END getGroupListCommand method
    
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Entity User Access Page.

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData getUserAccessObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getObject" );
      this.LogDebug ( "RefreshEntityChildren: {0}.", this.Session.RefreshEntityChildren );
      try
      {
        // 
        // Initialise the methods variables and objects.
        // 
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );

        //
        // Get the record.
        //
        var result = this.GetEntity ( PageCommand );
        // 
        // if the guid is empty the parameter was not found to exit.
        // 
        if ( result != EvEventCodes.Ok )
        {
          this.ErrorMessage = EdLabels.Record_Retrieve_Error_Message;

          this.LogError ( EvEventCodes.Database_Record_Retrieval_Error, "Retrieved Record is empty." );
          this.LogMethodEnd ( "getObject" );
          return this.Session.LastPage;
        }

        this.LogDebug ( "EntityDictionary count {0}.", this.Session.EntityDictionary.Count );
        this.LogDebug ( "Entity.EntityId: {0}.", this.Session.Entity.EntityId );
        this.LogDebug ( "Entity.Design.ReadAccessRoles: {0}.", this.Session.Entity.Design.ReadAccessRoles );

        // 
        // If the user does not have monitor or ResultData manager roles exit the page.
        // 
        if ( this.Session.Entity.hasReadAccess ( this.Session.UserProfile.Roles ) == false )
        {
          this.LogIllegalAccess (
            this.ClassNameSpace + "getUserAccessObject",
            this.Session.UserProfile );

          this.ErrorMessage = EdLabels.Record_Access_Error_Message;

          return this.Session.LastPage; ;
        }

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "getUserAccessObject",
          this.Session.UserProfile );
        //
        // Rung the server script if server side scripts are enabled.
        //
        this.runServerScript ( EvServerPageScript.ScripEventTypes.OnOpen );

        // 
        // Initialise the client object.
        // 
        clientDataObject.Page.Id = clientDataObject.Id;
        clientDataObject.Page.PageDataGuid = clientDataObject.Id;
        clientDataObject.Page.PageId = this.Session.Entity.LayoutId;
        clientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Disabled;

        clientDataObject.Id = this.Session.Entity.Guid;
        clientDataObject.Title = this.Session.Entity.CommandTitle;

        if ( this.Session.Entity.Design.LinkContentSetting == EdRecord.LinkContentSetting.First_Text_Field )
        {
          clientDataObject.Title = 
            String.Format( EdLabels.Entity_User_Access_Page_Title,
            this.Session.Entity.CommandTitle );
        }
        clientDataObject.Page.Title = clientDataObject.Title;

        // 
        // Generate the client ResultData object for the UniForm client.
        // 
        this.getEntityClientData ( clientDataObject.Page );

        // 
        // Return the client ResultData object to the calling method.
        // 
        this.LogMethodEnd ( "getObject" );
        return clientDataObject;
      }
      catch ( Exception Ex )
      {
        // 
        // On an exception raised create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.Entity_Retrieve_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }


      this.LogMethodEnd ( "getObject" );
      return this.Session.LastPage; ;

    }//END getObject method
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

  }
}//END namespace