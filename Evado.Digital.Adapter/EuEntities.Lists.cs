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

 
using Evado.Model;
using Evado.Digital.Bll;
using Evado.Digital.Model;
// using Evado.Web;

namespace Evado.Digital.Adapter
{
  /// <summary>
  /// This class defines the application base classs that is used to terminate the 
  /// hosted application objects.
  /// 
  /// This class terminates the Customer object.
  /// </summary>
  public partial class EuEntities : EuClassAdapterBase
  {
    #region Class record list methods

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.Command object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  -----------------------------------------------------------------------------
    private Evado.UniForm.Model.AppData getListObject (
      Evado.UniForm.Model.Command PageCommand )
    {
      this.LogMethod ( "getListObject" );
      this.LogDebug ( "Selected_EntityLayoutId: " + this.Session.Selected_EntityLayoutId );
      this.LogDebug ( "EntitySelectionState: " + this.Session.EntityStateSelection );
      this.LogDebug ( "EntitySelectionLayoutId: " + this.Session.Selected_EntityLayoutId );
      try
      {
        // 
        // Initialise the methods variables and objects.
        // 
        Evado.UniForm.Model.AppData clientDataObject = new Evado.UniForm.Model.AppData ( );
        this.EnableEntityEditButtonUpdate = false; 
        //
        // get the selected entity.
        //
        this.Session.EntityLayout = this.AdapterObjects.GetEntityLayout ( this.Session.Selected_EntityLayoutId );

        this.LogDebug ( "EntityLayout.ReadAccessRoles: " + this.Session.EntityLayout.Design.ReadAccessRoles );
        this.LogDebug ( "UserProfile.Roles: " + this.Session.UserProfile.Roles );

        // 
        // If the user does not have monitor or ResultData manager roles exit the page.
        // 
        if ( this.Session.EntityLayout.hasReadAccess ( this.Session.UserProfile.Roles ) == false )
        {
          this.LogIllegalAccess (
            this.ClassNameSpace + "getListObject",
            this.Session.UserProfile );

          this.ErrorMessage = EdLabels.Record_Access_Error_Message;

          return this.Session.LastPage; ;
        }
        // 
        // Log the user's access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "getListObject",
          this.Session.UserProfile );

        this.LogDebug ( "LayoutId {0}", this.Session.EntityLayout.LayoutId );

        //
        // Set the parent entity variables.
        //
        this.ParentLayoutId = this.Session.Entity.LayoutId;
        this.ParentGuid = this.Session.Entity.Guid;
        if ( PageCommand.hasParameter ( EdRecord.FieldNames.ParentGuid ) == true )
        {
          this.ParentGuid = PageCommand.GetParameterAsGuid ( EdRecord.FieldNames.ParentGuid );
        }
        if ( PageCommand.hasParameter ( EdRecord.FieldNames.ParentLayoutId ) == true )
        {
          this.ParentLayoutId = PageCommand.GetParameter ( EdRecord.FieldNames.ParentLayoutId );
        }

        //
        // Execute the monitor list record query.
        //
        this.executeRecordQuery ( );

        // 
        // Initialise the client ResultData object.
        // 
        clientDataObject.Id = Guid.NewGuid ( );
        clientDataObject.Page.Id = clientDataObject.Id;
        clientDataObject.Page.PageDataGuid = clientDataObject.Id;
        clientDataObject.Title = EdLabels.Entity_View_Page_Title;

        if ( this.AdapterObjects.Settings.UseHomePageHeaderOnAllPages == true )
        {
          clientDataObject.Title = this.AdapterObjects.Settings.HomePageHeaderText;
        }
        else
        {
          clientDataObject.Title = this.Session.EntityLayout.Title;
        }

        clientDataObject.Page.Title = clientDataObject.Title;
        clientDataObject.Page.PageId = Evado.Digital.Model.EdStaticPageIds.Records_View.ToString ( );

        if ( this.Session.Entity.hasEditAccess ( this.Session.UserProfile.Roles ) == true )
        {
          clientDataObject.Page.EditAccess = Evado.UniForm.Model.EditAccess.Enabled;
        }
        this.LogValue ( "Page.EditAccess: " + clientDataObject.Page.EditAccess );

        // 
        // Create the new pageMenuGroup for query selection.
        // 
        this.getList_SelectionGroup ( clientDataObject.Page );

        // 
        // Create the pageMenuGroup containing commands to open the records.
        //         
        this.getEntity_ListGroup ( clientDataObject.Page );

        this.LogValue ( "data.Page.Title: " + clientDataObject.Page.Title );


        return clientDataObject;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.Record_View_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return this.Session.LastPage; ;

    }//END getListObject method.

    // ==============================================================================
    /// <summary>
    /// This method creates the record view pageMenuGroup containing a list of commands to 
    /// open the form record.
    /// </summary>
    /// <param name="PageObject">Evado.Model.Uniform.Page object to add the pageMenuGroup to.</param>
    //  ------------------------------------------------------------------------------
    private void getList_SelectionGroup (
      Evado.UniForm.Model.Page PageObject )
    {
      this.LogMethod ( "getList_SelectionGroup" );
      this.LogDebug ( "IssuedEntityLayouts.Count {0}. ", this.AdapterObjects.IssuedEntityLayouts.Count );
      //
      // Initialise the methods variables and objects.
      //
      Evado.UniForm.Model.Group pageGroup = new Evado.UniForm.Model.Group ( );
      List<EvOption> optionList;
      Evado.UniForm.Model.Field selectionField;

      if ( this._HideSelectionGroup == true )
      {
        this.LogMethodEnd ( "getList_SelectionGroup" );
        return;
      }

      // 
      // Create the new pageMenuGroup for record selection.
      // 
      pageGroup = PageObject.AddGroup (
        EdLabels.Entities_Selection_Group_Title,
        Evado.UniForm.Model.EditAccess.Enabled );
      pageGroup.GroupType = Evado.UniForm.Model.GroupTypes.Default;
      pageGroup.Layout = Evado.UniForm.Model.GroupLayouts.Full_Width;
      pageGroup.AddParameter ( Evado.UniForm.Model.GroupParameterList.Offline_Hide_Group, true );

      // 
      // Add the record state selection option
      //
      optionList = new List<EvOption> ( );
      optionList.Add ( new EvOption ( ) );
      foreach ( EdRecord layout in this.AdapterObjects.IssuedEntityLayouts )
      {
        optionList.Add ( new EvOption ( layout.LayoutId,
          String.Format ( "{0} - {1}", layout.LayoutId, layout.Title ) ) );
      }

      selectionField = pageGroup.createSelectionListField (
        EdRecord.FieldNames.Layout_Id,
        EdLabels.Label_Form_Id,
        this.Session.Selected_EntityLayoutId,
        optionList );

      selectionField.Layout = EuAdapter.DefaultFieldLayout;
      selectionField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Snd_Cmd_On_Change, 1 );

      // 
      // Add the record state selection option
      // 
      optionList = EdRecord.getRecordStates ( false );

      selectionField = pageGroup.createSelectionListField (
        EdRecord.FieldNames.Status.ToString ( ),
        EdLabels.Record_State_Selection,
        this.Session.EntityStateSelection,
        optionList );

      selectionField.Layout = EuAdapter.DefaultFieldLayout;
      selectionField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Snd_Cmd_On_Change, 1 );

      // 
      // Add the selection groupCommand
      // 
      Evado.UniForm.Model.Command selectionCommand = pageGroup.addCommand (
        EdLabels.Select_Records_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Entities.ToString ( ),
        Evado.UniForm.Model.ApplicationMethods.Custom_Method );

      selectionCommand.setCustomMethod ( Evado.UniForm.Model.ApplicationMethods.List_of_Objects );

    }//ENd getList_SelectionGroup method

    // ==============================================================================
    /// <summary>
    /// This method executed the form record query of the database.
    /// </summary>
    /// <param name="ParentGuid">Guid: parent guid</param>
    /// <remarks>
    /// This method returns a list of forms based on the selection type of form record.
    /// </remarks>
    //  ------------------------------------------------------------------------------
    private void executeRecordQuery ( )
    {
      this.LogMethod ( "executeRecordQuery" );
      this.LogDebug ( "EntityLayoutIdSelection: " + this.Session.Selected_EntityLayoutId );
      this.LogDebug ( "Layout.ParentType : " + this.Session.EntityLayout.Design.ParentType );
      this.LogDebug ( "EntityTypeSelection: " + this.Session.EntityTypeSelection );
      this.LogDebug ( "EntityStateSelection: " + this.Session.EntityStateSelection );
      this.LogDebug ( "ParentGuid: {0}.", this.ParentGuid );
      //
      // Initialise the methods variables and objects.
      //
      EdQueryParameters queryParameters = new EdQueryParameters ( );

      // 
      // Initialise the query values to the currently selected objects identifiers.
      // 
      queryParameters.Type = this.Session.EntityTypeSelection;
      queryParameters.LayoutId = this.Session.Selected_EntityLayoutId;

      //
      // pass the entity selection filters to the query.
      //
      queryParameters.SelectionFilters = this.Session.EntitySelectionFilters;

      // 
      // Initialise the query state selection.
      // 
      queryParameters.States.Add ( EuAdapter.CONST_RECORD_STATE_SELECTION_DEFAULT );
      queryParameters.NotSelectedState = true;

      if ( this.Session.EntityStateSelection != EdRecordObjectStates.Null )
      {
        queryParameters.States.Add ( this.Session.EntityStateSelection );
        queryParameters.NotSelectedState = false;
      }

      if ( this.EnableAuthorSelection == true )
      {
        queryParameters.AuthorUserId = this.Session.SelectedUserId;
      }

      //
      // set the parent object selection criteria.
      //
      queryParameters.ParentType = this.Session.EntityLayout.Design.ParentType;

      if ( this.Session.EntityLayout.Design.ParentType == EdRecord.ParentTypeList.Organisation )
      {
        queryParameters.ParentOrgId = this.Session.SelectedOrgId;
      }

      if ( this.Session.EntityLayout.Design.ParentType == EdRecord.ParentTypeList.User )
      {
        queryParameters.ParentUserId = this.Session.SelectedUserId;
      }

      if ( this.Session.EntityLayout.Design.ParentType == EdRecord.ParentTypeList.Entity )
      {
        queryParameters.ParentGuid = this.Session.Entity.Guid;

        if ( this.ParentGuid != Guid.Empty )
        {
          queryParameters.ParentGuid = this.ParentGuid;
        }
      }

      //
      // Set the filter critier.
      //
      queryParameters.Org_City = this.Session.SelectedCity;
      queryParameters.Org_Country = this.Session.SelectedCountry;
      queryParameters.Org_PostCode = this.Session.SelectedPostCode;

      if ( queryParameters.Org_City != String.Empty
        || queryParameters.Org_Country != String.Empty
        || queryParameters.Org_PostCode != String.Empty )
      {
        queryParameters.EnableOrganisationFilter = true;
      }
      /*
      this.LogDebug ( "Selected LayoutId: '" + queryParameters.LayoutId + "'" );
      this.LogDebug ( "Selected ParentType: '" + queryParameters.ParentType + "'" );
      this.LogDebug ( "Selected ParentGuid: '" + queryParameters.ParentGuid + "'" );
      this.LogDebug ( "Selected Org_City: '" + queryParameters.Org_City + "'" );
      this.LogDebug ( "Selected Org_Country: '" + queryParameters.Org_Country + "'" );
      this.LogDebug ( "Selected Org_PostCode: '" + queryParameters.Org_PostCode + "'" );
      */

      // 
      // Query the database to retrieve a list of the records matching the query parameter values.
      // 
      if ( queryParameters.LayoutId != String.Empty )
      {
        this.LogDebug ( "Querying form records" );
        this.Session.EntityList = this._Bll_Entities.GetEntityList ( queryParameters );

        this.LogDebugClass ( this._Bll_Entities.Log );
      }
      this.LogDebug ( "EntityList.Count: " + this.Session.EntityList.Count );

      this.LogMethodEnd ( "executeRecordQuery" );

    }//END executeRecordQuery method.

    // ==============================================================================
    /// <summary>
    /// This method creates the record view pageMenuGroup containing a list of commands to 
    /// open the form record.
    /// </summary>
    /// <param name="PageObject">Evado.Model.Uniform.Page object to add the pageMenuGroup to.</param>
    /// <param name="RecordList">List of EvForm: form record objects.</param>
    //  ------------------------------------------------------------------------------
    private void getEntity_ListGroup (
      Evado.UniForm.Model.Page PageObject )
    {
      this.LogMethod ( "getEntity_ListGroup" );
      this.LogDebug ( "PageObject.EditAccess {0}.", PageObject.EditAccess );
      this.LogDebug ( "this.Session.EntityLayout.Title {0}.", this.Session.EntityLayout.Title );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.Group pageGroup = new Evado.UniForm.Model.Group ( );
      Evado.UniForm.Model.Command groupCommand = new Evado.UniForm.Model.Command ( );

      // 
      // Create the record display pageMenuGroup.
      // 
      pageGroup = PageObject.AddGroup (
        EdLabels.Entity_List_Group_Title );
      pageGroup.CmdLayout = Evado.UniForm.Model.GroupCommandListLayouts.Vertical_Orientation;

      pageGroup.Layout = Evado.UniForm.Model.GroupLayouts.Full_Width;

      if ( this.Session.EntityLayout.Title != String.Empty )
      {
        pageGroup.Title = String.Format (
          EdLabels.Entity_List_Title_Group_Title,
          this.Session.EntityLayout.Title );
      }

      if ( this.Session.EntityList.Count > 0 )
      {
        pageGroup.Title += EdLabels.List_Count_Label + this.Session.EntityList.Count;
      }

      //
      // Add a create record command.
      //
      if ( this.Session.Selected_EntityLayoutId != String.Empty
        && this.Session.PageId != Evado.Digital.Model.EdStaticPageIds.Entity_Filter_View.ToString ( )
        && pageGroup.EditAccess == Evado.UniForm.Model.EditAccess.Enabled
        && ( this.Session.EntityLayout.Design.ParentEntities.Contains ( this.ParentLayoutId ) == true
          || this.Session.EntityLayout.Design.ParentEntities == String.Empty ) )
      {
        groupCommand = pageGroup.addCommand (
          String.Format ( EdLabels.Entity_Create_New_List_Command_Title, this.Session.EntityLayout.Title ),
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Entities,
          Evado.UniForm.Model.ApplicationMethods.Create_Object );

        groupCommand.SetBackgroundDefaultColour ( Evado.UniForm.Model.Background_Colours.Purple );

        groupCommand.AddParameter ( Evado.Digital.Model.EdRecord.FieldNames.Layout_Id,
        this.Session.Selected_EntityLayoutId );
        groupCommand.AddParameter ( EdRecord.FieldNames.ParentGuid, this.Session.Entity.Guid );
        groupCommand.SetGuid ( this.Session.Entity.Guid );
      }

      this.LogDebug ( "EntityList.Count: " + this.Session.EntityList.Count );
      // 
      // Iterate through the record list generating a groupCommand to access each record
      // then append the groupCommand to the record pageMenuGroup view's groupCommand list.
      // 
      foreach ( Evado.Digital.Model.EdRecord entity in this.Session.EntityList )
      {
        this.LogDebug ( "LCD {0}, LT {1}, FC {2}", entity.Design.LinkContentSetting, entity.CommandTitle, entity.Fields.Count );

        //
        // Create the group list groupCommand object.
        //
        this.getGroupListCommand (
          entity,
          pageGroup,
         this.Session.EntityLayout.Design.LinkContentSetting );

      }//END iteration loop

      this.LogValue ( "Group command count: " + pageGroup.CommandList.Count );

      this.LogMethodEnd ( "getRecord_ListGroup" );
    }//END createViewCommandList method

    // ==============================================================================
    /// <summary>
    /// This method appends the milestone groupCommand to the page milestone list pageMenuGroup
    /// </summary>
    /// <param name="CommandEntity">EvForm object</param>
    /// <param name="PageGroup"> Evado.UniForm.Model.Group</param>
    //  -----------------------------------------------------------------------------
    private Evado.UniForm.Model.Command getGroupListCommand (
      EdRecord CommandEntity,
      Evado.UniForm.Model.Group PageGroup,
      EdRecord.LinkContentSetting ParentLinkSetting )
    {
      this.LogMethod ( "getGroupListCommand" );
      this.LogDebug ( "CommandEntity.EntityId: " + CommandEntity.EntityId );
      this.LogDebug ( "LinkContentSetting: " + CommandEntity.Design.LinkContentSetting );
      this.LogDebug ( "TypeId: " + CommandEntity.TypeId );
      this.LogDebug ( "ParentLinkSetting: " + ParentLinkSetting );

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
      Evado.UniForm.Model.Command groupCommand = PageGroup.addCommand (
          CommandEntity.CommandTitle,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Entities,
          Evado.UniForm.Model.ApplicationMethods.Get_Object );

      groupCommand.Id = CommandEntity.Guid;
      groupCommand.SetGuid ( CommandEntity.Guid );

      groupCommand.AddParameter (
        Evado.UniForm.Model.CommandParameters.Short_Title,
        EdLabels.Label_Record_Id + CommandEntity.RecordId );
      if ( CommandEntity.ImageFileName != String.Empty )
      {
        string relativeURL = EuAdapter.CONST_IMAGE_FILE_DIRECTORY + CommandEntity.ImageFileName;
        groupCommand.AddParameter ( Evado.UniForm.Model.CommandParameters.Image_Url, relativeURL );
      }

      if ( this.Session.UserProfile.hasAdministrationAccess == true )
      {
        groupCommand.Title = CommandEntity.EntityId + " >> " + groupCommand.Title;
      }

      return groupCommand;

    }//END getGroupListCommand method

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class private record export methods

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.Command object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.UniForm.Model.AppData getRecordExport_Object (
      Evado.UniForm.Model.Command PageCommand )
    {
      this.LogMethod ( "getRecordExport_Object" );

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.AppData clientDataObject = new Evado.UniForm.Model.AppData ( );
      List<EdRecord> recordList = new List<EdRecord> ( );
      try
      {

        // 
        // If the user does not have monitor or ResultData manager roles exit the page.
        // 
        if ( this.Session.Entity.hasReadAccess ( this.Session.UserProfile.Roles ) == false )
        {
          this.LogIllegalAccess (
            this.ClassNameSpace + "getRecordExport_Object",
            this.Session.UserProfile );

          this.ErrorMessage = EdLabels.Record_Access_Error_Message;

          return this.Session.LastPage;
        }

        // 
        // Log the user's access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "getRecordExport_Object",
          this.Session.UserProfile );

        // 
        // Initialise the client ResultData object.
        // 
        clientDataObject.Id = Guid.NewGuid ( );
        clientDataObject.Page.Id = clientDataObject.Id;
        clientDataObject.Page.PageDataGuid = clientDataObject.Id;
        clientDataObject.Title = EdLabels.Record_View_Page_Title;
        clientDataObject.Page.Title = clientDataObject.Title;
        clientDataObject.Page.PageId = Evado.Digital.Model.EdStaticPageIds.Record_Export_Page.ToString ( );

        // 
        // Create the new pageMenuGroup for query selection.
        // 
        this.getRecordExport_SelectionGroup ( clientDataObject.Page );

        this.LogValue ( "FormId: " + this.Session.Selected_EntityLayoutId );
        this.LogValue ( "UserCommonName: " + this.Session.UserProfile.CommonName );

        //
        // Create the export file and save if for download.
        //
        this.getRecordExport_DownloadGroup ( clientDataObject.Page );

        return clientDataObject;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.Record_View_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return this.Session.LastPage;

    }//END getListObject method.

    // ==============================================================================
    /// <summary>
    /// This method creates the record view pageMenuGroup containing a list of commands to 
    /// open the form record.
    /// </summary>
    /// <param name="Page">Evado.Model.Uniform.Page object to add the pageMenuGroup to.</param>
    //  ------------------------------------------------------------------------------
    private void getRecordExport_SelectionGroup (
      Evado.UniForm.Model.Page Page )
    {
      this.LogMethod ( "getRecordExport_SelectionGroup" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.Group pageGroup = new Evado.UniForm.Model.Group ( );
      Evado.UniForm.Model.Command command = new Evado.UniForm.Model.Command ( );
      Evado.UniForm.Model.Field selectionField = new Evado.UniForm.Model.Field ( );
      List<EvOption> optionList = new List<EvOption> ( );

      //
      // If the issued form list is empty fill it with the currently issued form selection objects.
      //
      if ( this.Session.IssueFormList.Count == 0 )
      {
        this.LogDebug ( "Issued Forms is empty so create list." );

        EdRecordLayouts forms = new EdRecordLayouts ( this.ClassParameters );

        this.Session.IssueFormList = forms.getList (
          EdRecordTypes.Null,
          EdRecordObjectStates.Form_Issued,
          false );
      }
      this.LogDebug ( "Issued Forms list count {0}.", this.Session.IssueFormList.Count );

      // 
      // Create the new pageMenuGroup for query selection.
      // 
      pageGroup = Page.AddGroup (
        EdLabels.Record_Selection_Group_Title,
        Evado.UniForm.Model.EditAccess.Enabled );
      pageGroup.GroupType = Evado.UniForm.Model.GroupTypes.Default;
      pageGroup.Layout = Evado.UniForm.Model.GroupLayouts.Full_Width;
      pageGroup.AddParameter ( Evado.UniForm.Model.GroupParameterList.Offline_Hide_Group, true );

      //
      // Add the selection pageMenuGroup.
      //
      selectionField = pageGroup.createSelectionListField (
        EdRecord.FieldNames.Layout_Id,
        EdLabels.Label_Form_Id,
        this.Session.Selected_EntityLayoutId,
        this.Session.IssueFormList );

      selectionField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Define the include draft record selection option.
      //
      selectionField = pageGroup.createBooleanField (
        EuEntities.CONST_INCLUDE_DRAFT_RECORDS,
        EdLabels.Record_Export_Include_Draft_Record_Field_Title,
        this.Session.FormRecords_IncludeDraftRecords );
      selectionField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Define the include free text ResultData selection option.
      //
      selectionField = pageGroup.createBooleanField (
        EuEntities.CONST_INCLUDE_FREE_TEXT_DATA,
        EdLabels.Record_Export_Include_FreeText_data_Field_Title,
        this.Session.FormRecords_IncludeFreeTextData );
      selectionField.Layout = EuAdapter.DefaultFieldLayout;

      // 
      // Add the selection groupCommand
      // 
      command = pageGroup.addCommand (
        EdLabels.Record_Export_Selection_Group_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Entities.ToString ( ),
         Evado.UniForm.Model.ApplicationMethods.Custom_Method );
      command.setCustomMethod ( Evado.UniForm.Model.ApplicationMethods.List_of_Objects );

    }//END getRecordExport_ListObject method

    // ==============================================================================
    /// <summary>
    /// This method creates the record view pageMenuGroup containing a list of commands to 
    /// open the form record.
    /// </summary>
    /// <param name="Page">Evado.Model.Uniform.Page object to add the pageMenuGroup to.</param>
    //  ------------------------------------------------------------------------------
    private void getRecordExport_DownloadGroup (
      Evado.UniForm.Model.Page Page )
    {
      this.LogMethod ( "getRecordExport_DownloadGroup" );
      this.LogDebug ( "FormRecords_IncludeFreeTextData: " + this.Session.FormRecords_IncludeFreeTextData );
      this.LogDebug ( "FormRecords_IncludeDraftRecords: " + this.Session.FormRecords_IncludeDraftRecords );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.Group pageGroup = new Evado.UniForm.Model.Group ( );
      Evado.UniForm.Model.Field groupField = new Evado.UniForm.Model.Field ( );
      EvFormRecordExport exportRecords = new EvFormRecordExport ( );
      EvExportParameters exportParameters = new EvExportParameters ( );
      EdQueryParameters queryParameters = new EdQueryParameters ( );
      int exportRecordFileLimit = 250;
      this.LogDebug ( "exportRecordFileLimit: " + exportRecordFileLimit );

      //
      // IF there are not parameters then exit.
      //
      if ( this.Session.Selected_EntityLayoutId == String.Empty )
      {
        this.LogDebug ( " Form {0}. ", this.Session.Selected_EntityLayoutId );
        this.LogMethodEnd ( "getRecordExport_DownloadGroup" );
        return;
      }


      exportParameters = new EvExportParameters (
        EvExportParameters.ExportDataSources.Project_Record,
        this.Session.Selected_EntityLayoutId );
      exportParameters.IncludeTestSites = false;
      exportParameters.IncludeFreeTextData = this.Session.FormRecords_IncludeFreeTextData;
      exportParameters.IncludeDraftRecords = this.Session.FormRecords_IncludeDraftRecords;

      queryParameters = new EdQueryParameters ( );
      queryParameters.LayoutId = this.Session.Selected_EntityLayoutId;

      queryParameters.States.Add ( EdRecordObjectStates.Withdrawn );
      queryParameters.States.Add ( EdRecordObjectStates.Draft_Record );
      queryParameters.States.Add ( EdRecordObjectStates.Empty_Record );
      queryParameters.States.Add ( EdRecordObjectStates.Completed_Record );

      if ( exportParameters.IncludeDraftRecords == true )
      {
        queryParameters.States.Add ( EdRecordObjectStates.Withdrawn );
      }
      queryParameters.NotSelectedState = true;
      queryParameters.IncludeSummary = true;
      queryParameters.IncludeRecordValues = false;
      queryParameters.IncludeComments = false;

      //
      // Create the export ResultData file.
      //
      int inResultCount = this._Bll_Entities.geyEntityCount ( queryParameters );

      this.LogClass ( this._Bll_Entities.Log );

      this.LogDebug ( "inResultCount: " + inResultCount );
      // 
      // Create the record display pageMenuGroup.
      // 
      pageGroup = Page.AddGroup (
        EdLabels.Record_Export_Download_Group_Title,
        Evado.UniForm.Model.EditAccess.Enabled );
      pageGroup.CmdLayout = Evado.UniForm.Model.GroupCommandListLayouts.Vertical_Orientation;

      pageGroup.Layout = Evado.UniForm.Model.GroupLayouts.Full_Width;

      if ( inResultCount == 0 )
      {
        LogDebug ( "Record Count is 0." );
        pageGroup.Description = EdLabels.Form_Record_Export_Empty_List_Error_Message;

        this.LogMethodEnd ( "getRecordExport_DownloadGroup" );
        return;
      }

      //
      // If the record list is less thant the exportrecord limit output.
      //
      if ( inResultCount <= exportRecordFileLimit )
      {
        //
        // export the ResultData.
        //
        this.exportRecordData (
            pageGroup,
            0,
           exportParameters,
             this.Session.Selected_EntityLayoutId );
      }
      else
      {
        //
        // Initialise the output loop parameters.
        int iRecordRangeStart = 0;
        int iRecordRangeFinish = exportRecordFileLimit;
        int iterations = ( inResultCount / exportRecordFileLimit ) + 1;

        this.LogValue ( "iterations: " + iterations );

        for ( int iLoop = 0; iLoop < iterations && iRecordRangeFinish <= inResultCount; iLoop++ )
        {

          //
          // export the ResultData.
          //
          var result = this.exportRecordData (
            pageGroup,
            iLoop,
             exportParameters,
             this.Session.Selected_EntityLayoutId );

          if ( result != EvEventCodes.Ok )
          {
            this.LogDebug ( "Result {0}.", result );
            this.LogMethodEnd ( "getRecordExport_DownloadGroup" );
            return;
          }

          //
          // Increment the start and finish for then next loop
          //
          iRecordRangeStart = iRecordRangeFinish + 1;
          if ( ( iRecordRangeFinish + exportRecordFileLimit ) < inResultCount )
          {

            iRecordRangeFinish += exportRecordFileLimit;
          }
          else
          {
            iRecordRangeFinish = inResultCount;
          }

        }//END loop iteration statement.
      }

      this.LogMethodEnd ( "getRecordExport_DownloadGroup" );
      return;
    }//END getRecordExport_DownloadGroup method

    //===================================================================================
    /// <summary>
    /// Thie method exports the record list to a export form.
    /// </summary>
    /// <param name="pageGroup">Evado.UniForm.Model.Group object</param>
    /// <param name="iteration">int: iteration loop</param>
    /// <param name="exportParameters">EvExportParameters object.</param>
    /// <param name="FormId">String form identifier</param>
    /// <returns>True export generated.</returns>
    //-----------------------------------------------------------------------------------
    private EvEventCodes exportRecordData (
      Evado.UniForm.Model.Group pageGroup,
      int iteration,
      EvExportParameters exportParameters,
      String FormId )
    {
      this.LogMethod ( "exportRecordData" );
      // 
      // Initialise the methods variables and objects.
      // 
      String csvDownload = String.Empty;
      String csvFileName = String.Empty;
      EvFormRecordExport exportRecords = new EvFormRecordExport ( this.ClassParameters );

      //
      // Generate the export download CSV ResultData file.
      //
      csvDownload = exportRecords.exportRecords (
        exportParameters,
        this.Session.UserProfile );

      if ( exportRecords.EventCode != EvEventCodes.Ok )
      {
        this.LogDebug ( "EventCode: " + exportRecords.EventCode );

        if ( exportRecords.EventCode == EvEventCodes.Data_Export_Empty_Record_List )
        {
          this.ErrorMessage = EdLabels.Form_Record_Export_Empty_List_Error_Message;
        }
        this.LogMethodEnd ( "exportRecordData" );
        return exportRecords.EventCode;
      }

      this.LogClass ( exportRecords.Log );
      //
      // Create the export file name.
      //
      csvFileName = FormId
        + "-Records-"
        + DateTime.Now.ToString ( "yy-MM-dd" ) + ".csv";

      if ( iteration > 0 )
      {
        csvFileName = FormId
        + "-Records-" + iteration
        + "-" + DateTime.Now.ToString ( "yy-MM-dd" ) + ".csv";
      }


      this.LogValue ( "csvDownload length: " + csvDownload.Length );
      this.LogValue ( "csvFileName: " + csvFileName );

      bool result = Evado.Digital.Model.EvcStatics.Files.saveFile (
        this.UniForm_BinaryFilePath,
        csvFileName,
        csvDownload );

      if ( result == false )
      {
        this.ErrorMessage = EdLabels.Record_Export_Error_Message;

        this.LogDebugClass ( Evado.Digital.Model.EvcStatics.Files.DebugLog );
        this.LogDebug ( "ReturnedEventCode: " + Evado.Digital.Model.EvcStatics.Files.ReturnedEventCode );
        this.LogDebug ( this.ErrorMessage );
        this.LogMethodEnd ( "exportRecordData" );
        return Evado.Digital.Model.EvcStatics.Files.ReturnedEventCode;
      }

      Evado.UniForm.Model.Field groupField = pageGroup.createHtmlLinkField (
        String.Empty,
        csvFileName,
      this.UniForm_BinaryServiceUrl + csvFileName );

      this.LogMethodEnd ( "exportRecordData" );
      return EvEventCodes.Ok;
    }//END exportRecordData method.

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

  }
}//END namespace