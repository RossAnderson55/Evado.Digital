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

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object using field filters.
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  -----------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData GetFilteredListObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "GetFilteredListObject 1" );
      this.LogDebug ( "EntitySelectionState: " + this.Session.EntityStateSelection );
      this.LogDebug ( "EntitySelectionLayoutId: " + this.Session.Selected_EntityLayoutId );
      this.LogDebug ( "EnableEmptyQuerySelection: " + this.Session.EnableEmptyQuerySelection );

      this.LogDebug ( "SelectedOrganisationCountry: " + this.Session.SelectedCountry );
      this.LogDebug ( "SelectedOrganisationCity: " + this.Session.SelectedCity );
      this.LogDebug ( "SelectedOrganisationPostCode: " + this.Session.SelectedPostCode );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
      List<EdRecord> recordList = new List<EdRecord> ( );

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
        this.ParentLayoutId = PageCommand.GetParameter ( EuEntities.CONST_EMPTY_SELECTION_FIELD );
      }

      try
      {
        //
        // get the selected entity.
        //
        this.Session.EntityLayout = this.AdapterObjects.GetEntityLayout ( this.Session.Selected_EntityLayoutId );
        this.LogDebug ( "EntityLayout.Title: " + this.Session.EntityLayout.Title );

        // 
        // If the user does not have monitor or ResultData manager roles exit the page.
        // 
        if ( this.Session.EntityLayout.hasReadAccess ( this.Session.UserProfile.Roles ) == false )
        {
          this.LogIllegalAccess (
            this.ClassNameSpace + "GetFilteredListObject",
            this.Session.UserProfile );

          this.ErrorMessage = EdLabels.Record_Access_Error_Message;

          this.LogMethodEnd ( "GetFilteredListObject" );
          return this.Session.LastPage; ;
        }

        // 
        // Log the user's access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "GetFilteredListObject",
          this.Session.UserProfile );


        //
        // Execute the query list record query.
        //
        if ( this.enableQuery ( ) == true )
        {
          this.executeRecordQuery ( );
        }

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
        clientDataObject.Page.PageId = EdStaticPageIds.Records_View.ToString ( );

        // 
        // Create the new pageMenuGroup for query selection.
        // 
        this.getFilteredList_SelectionGroup (
          clientDataObject.Page );

        // 
        // Create the pageMenuGroup containing commands to open the records.
        //         
        this.getEntity_Summary_ListGroup ( clientDataObject.Page );

        this.LogValue ( "data.Page.Title: " + clientDataObject.Page.Title );


        this.LogMethodEnd ( "GetFilteredListObject" );
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

      this.LogMethodEnd ( "GetFilteredListObject" );
      return this.Session.LastPage; ;

    }//END GetFilteredListObject method.

    // ==============================================================================
    /// <summary>
    /// This method test for criteria for the selection query.
    /// </summary>
    /// <returns> true: query is enabled.</returns>
    //  ------------------------------------------------------------------------------
    private bool enableQuery ( )
    {
      this.LogMethod ( "enableQuery" );
      this.LogDebug ( "EnableEmptyQuerySelection {0}.", this.Session.EnableEmptyQuerySelection );

      if ( this.Session.EnableEmptyQuerySelection == true )
      {
        return true;
      }

      bool enableEmptyQuery = false;

      if ( this.Session.SelectedCity != String.Empty
        || this.Session.SelectedCountry != String.Empty
        || this.Session.SelectedPostCode != String.Empty )
      {
        this.LogDebug ( "Organisation filter enabled." );
        enableEmptyQuery = true;
      }

      foreach ( string str in this.Session.EntitySelectionFilters )
      {
        if ( str != String.Empty )
        {
          this.LogDebug ( "Field filter enabled." );
          enableEmptyQuery = true;
        }
      }

      this.LogDebug ( "enableEmptyQuery {0}.", enableEmptyQuery );

      if ( enableEmptyQuery == false )
      {
        this.Session.EntityList = new List<EdRecord> ( );
      }

      this.LogMethodEnd ( "enableQuery" );
      return enableEmptyQuery;
    }

    // ==============================================================================
    /// <summary>
    /// This method creates the record view pageMenuGroup containing a list of commands to 
    /// open the form record.
    /// </summary>
    /// <param name="PageObject">Evado.Model.Uniform.Page object to add the pageMenuGroup to.</param>
    /// <param name="subjects">EvSubjects subjects to add to selection groups</param>
    /// <param name="subjectVisits">EvSubjectMilestones visits for each subject</param>
    /// <param name="QueryParameters">EvQueryParameters: conting the query parameters</param>
    /// <param name="ApplicationObject">Adapter.ApplicationObjects object.</param>
    //  ------------------------------------------------------------------------------
    private void getFilteredList_SelectionGroup (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getFilteredList_SelectionGroup" );
      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.UniForm.Group pageGroup = null;
      Evado.Model.UniForm.Command selectionCommand = null;
      List<EvOption> optionList;
      Evado.Model.UniForm.Field groupField;

      //
      // if hide selection group is enabled exit 
      //
      if ( this._HideSelectionGroup == true )
      {
        return;
      }

      // 
      // Create the new pageMenuGroup for record selection.
      // 
      pageGroup = PageObject.AddGroup (
        EdLabels.Entities_Selection_Group_Title,
        Evado.Model.UniForm.EditAccess.Enabled );
      pageGroup.GroupType = Evado.Model.UniForm.GroupTypes.Default;
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
      pageGroup.AddParameter ( Model.UniForm.GroupParameterList.Offline_Hide_Group, true );

      //
      // if the entity is not selected display an entity selection field.
      //
      if ( this.Session.Selected_EntityLayoutId == String.Empty )
      {
        // 
        // Add the record state selection option
        //
        optionList = new List<EvOption> ( );
        optionList.Add ( new EvOption ( ) );
        foreach ( EdRecord layout in this.AdapterObjects.IssuedEntityLayouts )
        {
          optionList.Add ( layout.SelectionOption );
        }

        groupField = pageGroup.createSelectionListField (
          EdRecord.FieldNames.Layout_Id,
          EdLabels.Label_Form_Id,
          this.Session.Selected_EntityLayoutId,
          optionList );

        groupField.Layout = EuAdapter.DefaultFieldLayout;
        groupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );

        // 
        // Add the selection groupCommand
        // 
       selectionCommand = pageGroup.addCommand (
          EdLabels.Entities_Selection_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Entities.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Custom_Method );

        selectionCommand.setCustomMethod ( Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

        selectionCommand.SetPageId ( EdStaticPageIds.Entity_Filter_View );
        ;
        this.LogDebug ( "Group Command Count {0}. ", pageGroup.CommandList.Count );
        this.LogMethodEnd ( "getFilteredList_SelectionGroup" );

      }//END layoutId selection

      //
      // if the entity layout id has been defined display the entity selection fields.
      //
      //
      // retrieve the selected entity layout object
      //
      this.queryLayout = this.AdapterObjects.GetEntityLayout ( this.Session.Selected_EntityLayoutId );

      this.LogDebug ( "E: {0} S: {1}.", queryLayout.LayoutId, queryLayout.State );
      this.LogDebug ( "Entity {0}. ", queryLayout.CommandTitle );



      //
      // Insert the static filter selection for organisation city.
      //
      if ( this.AdapterObjects.Settings.StaticQueryFilterOptions.Contains ( EdOrganisation.FieldNames.Address_Country.ToString ( ) ) == true )
      {
        this.getQueryList_StaticOrgFilter ( pageGroup, EdOrganisation.FieldNames.Address_Country );
      }
      if ( this.AdapterObjects.Settings.StaticQueryFilterOptions.Contains ( EdOrganisation.FieldNames.Address_City.ToString ( ) ) == true )
      {
        this.getQueryList_StaticOrgFilter ( pageGroup, EdOrganisation.FieldNames.Address_City );
      }
      if ( this.AdapterObjects.Settings.StaticQueryFilterOptions.Contains ( EdOrganisation.FieldNames.Address_Post_Code.ToString ( ) ) == true )
      {
        this.getQueryList_StaticOrgFilter ( pageGroup, EdOrganisation.FieldNames.Address_Post_Code );
      }

      //
      // iterate through the filter field ids and display the filter field in the selection group.
      //
      for ( int filterIndex = 0; filterIndex < queryLayout.FilterFieldIds.Length; filterIndex++ )
      {
        string fieldId = queryLayout.FilterFieldIds [ filterIndex ];

        if ( fieldId == String.Empty )
        {
          continue;
        }

        this.LogDebug ( "fieldId {0}. ", fieldId );
        //
        // retrieve the matching field object.
        //
        EdRecordField field = queryLayout.GetFieldObject ( fieldId );

        if ( field == null )
        {
          continue;
        }

        this.LogDebug ( "field.FieldId {0} - {1}. ", field.FieldId, field.Title );

        //
        // retrieve the current selection filter value.
        //
        string selectionFilter = this.Session.EntitySelectionFilters [ filterIndex ];
        this.LogDebug ( "selectionFilter {0}. ", selectionFilter );

        //
        // create the selection field object for the selected field.
        //
        this.getFilteredList_SelectionField (
          pageGroup,
          filterIndex,
          selectionFilter,
          field );
      }//END of the Selection filter iteration loop.

      // 
      // Add the selection groupCommand
      // 
      selectionCommand = pageGroup.addCommand (
        EdLabels.Entities_Selection_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Entities.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Custom_Method );

      selectionCommand.setCustomMethod ( Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

      selectionCommand.SetPageId ( EdStaticPageIds.Entity_Filter_View );
      ;
      this.LogDebug ( "Group Command Count {0}. ", pageGroup.CommandList.Count );
      this.LogMethodEnd ( "getFilteredList_SelectionGroup" );

    }//ENd getFilteredList_SelectionGroup method

    // ==============================================================================
    /// <summary>
    /// This method creates an entity filter selection field object.
    /// </summary>
    /// <param name="PageGroup">Evado.Model.Uniform.Group object to add the pageGroup .</param>
    /// <param name="FieldName">EdOrganisation.FieldNames enumerated list value</param>
    //  ------------------------------------------------------------------------------
    private void getQueryList_StaticOrgFilter (
      Evado.Model.UniForm.Group PageGroup,
      EdOrganisation.FieldNames FieldName )
    {
      this.LogMethod ( "getQueryList_StaticOrgFilter" );
      this.LogDebug ( "FieldName: {0}. ", FieldName );
      //
      // Initialise the methods variables and objects.
      //
      List<EvOption> optionList = new List<EvOption> ( );
      optionList.Add ( new EvOption ( ) );

      //
      // Select the field to be displayed.
      //
      switch ( FieldName )
      {
        case EdOrganisation.FieldNames.Address_Country:
          {
            optionList = this.AdapterObjects.GetOrganisationFilterList ( EdOrganisation.FieldNames.Address_Country, true );

            var field = PageGroup.createSelectionListField (
              FieldName,
              EdLabels.Organisation_Address_Country_Field_Label,
              this.Session.SelectedCountry,
              optionList );
            field.Layout = EuAdapter.DefaultFieldLayout;
            field.AddParameter ( Evado.Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );
            break;
          }
        case EdOrganisation.FieldNames.Address_Post_Code:
          {
            optionList = this.AdapterObjects.GetOrganisationFilterList ( EdOrganisation.FieldNames.Address_Post_Code, true );

            var field = PageGroup.createSelectionListField (
              FieldName,
              EdLabels.Organisation_Address_PostCode_Field_Label,
              this.Session.SelectedPostCode,
              optionList );
            field.Layout = EuAdapter.DefaultFieldLayout;
            field.AddParameter ( Evado.Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );
            break;
          }
        case EdOrganisation.FieldNames.Address_City:
          {
            optionList = this.AdapterObjects.GetOrganisationFilterList ( EdOrganisation.FieldNames.Address_City, true );

            var field = PageGroup.createSelectionListField (
              FieldName,
              EdLabels.Organisation_Address_City_Field_Label,
              this.Session.SelectedCity,
              optionList );
            field.Layout = EuAdapter.DefaultFieldLayout;
            field.AddParameter ( Evado.Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );
            break;
          }
      }
      this.LogMethodEnd ( "getQueryList_StaticOrgFilter" );
    }

    // ==============================================================================
    /// <summary>
    /// This method creates an entity filter selection field object.
    /// </summary>
    /// <param name="PageGroup">Evado.Model.Uniform.Group object to add the pageGroup .</param>
    /// <param name="FieldName">EdOrganisation.FieldNames enumerated list value</param>
    //  ------------------------------------------------------------------------------
    private void getQueryList_StaticUserFilter (
      Evado.Model.UniForm.Group PageGroup,
      EdUserProfile.FieldNames FieldName )
    {
      this.LogMethod ( "getQueryList_StaticUserFilter" );
      this.LogDebug ( "FieldName: {0}. ", FieldName );
      //
      // Initialise the methods variables and objects.
      //
      String userSelectionList = this.AdapterObjects.Settings.UserCategoryList;
      List<EvOption> optionList = new List<EvOption> ( );
      optionList.Add ( new EvOption ( ) );

      //
      // Select the field to be displayed.
      //
      switch ( FieldName )
      {
        case EdUserProfile.FieldNames.User_Category:
          {
            optionList = this.AdapterObjects.getSelectionOptions ( userSelectionList, String.Empty, true, true );

            var field = PageGroup.createSelectionListField (
              FieldName,
              EdLabels.UserProfile_User_Category_Field_Label,
              this.Session.SelectedUserCategory,
              optionList );
            field.Layout = EuAdapter.DefaultFieldLayout;
            field.AddParameter ( Evado.Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );
            break;
          }
        case EdUserProfile.FieldNames.User_Type:
          {
            optionList = this.AdapterObjects.getSelectionOptions ( userSelectionList, String.Empty, false, true );

            var field = PageGroup.createSelectionListField (
              FieldName,
              EdLabels.UserProfile_User_Type_Field_Label,
              this.Session.SelectedUserType,
              optionList );
            field.Layout = EuAdapter.DefaultFieldLayout;
            field.AddParameter ( Evado.Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );
            break;
          }
      }
      this.LogMethodEnd ( "getQueryList_StaticUserFilter" );
    }

    // ==============================================================================
    /// <summary>
    /// This method creates an entity filter selection field object.
    /// </summary>
    /// <param name="PageGroup">Evado.Model.Uniform.Group object to add the pageGroup .</param>
    /// <param name="FilterIndex">Integer: filter index</param>
    /// <param name="SelectionFilter">String: filter value </param>
    /// <param name="Field">EdRecordField object.</param>
    //  ------------------------------------------------------------------------------
    private void getFilteredList_SelectionField (
      Evado.Model.UniForm.Group PageGroup,
      int FilterIndex,
      String SelectionFilter,
      EdRecordField Field )
    {
      this.LogMethod ( "getFilteredList_SelectionField" );

      this.LogDebug ( "FilterIndex: {0}, SelectionFilter: {1}. ", FilterIndex, SelectionFilter );
      //this.LogDebug ( "F: {0}, T: {1}, Type {2}. ", Field.FieldId, Field.Title, Field.TypeId );

      List<EvOption> optionList = Evado.Model.UniForm.EuStatics.getStringAsOptionList (
        Field.Design.Options );

      optionList.Sort ( ( n1, n2 ) => n1.Description.CompareTo ( n2.Description ) );

      List<EvOption> selectionOptionList = new List<EvOption> ( );
      selectionOptionList.Add ( new EvOption ( ) );
      foreach ( EvOption opt in optionList )
      {
        selectionOptionList.Add ( opt );
      }

      //
      // user the switch to select the selection data types.
      //
      switch ( Field.TypeId )
      {
        case EvDataTypes.Check_Box_List:
          {
            var field = PageGroup.createCheckBoxListField (
              EuEntities.CONST_SELECTION_FIELD + FilterIndex,
              Field.Title,
              SelectionFilter,
              optionList );
            field.Layout = EuAdapter.DefaultFieldLayout;
            field.AddParameter ( Evado.Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );

            break;
          }
        case EvDataTypes.Yes_No:
        case EvDataTypes.Boolean:
          {
            var field = PageGroup.createBooleanField (
              EuEntities.CONST_SELECTION_FIELD + FilterIndex,
              Field.Title,
              EvStatics.getBool ( SelectionFilter ) );
            field.Layout = EuAdapter.DefaultFieldLayout;
            field.AddParameter ( Evado.Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );

            break;
          }
        case EvDataTypes.Selection_List:
        case EvDataTypes.Radio_Button_List:
          {
            var field = PageGroup.createSelectionListField (
              EuEntities.CONST_SELECTION_FIELD + FilterIndex,
              Field.Title,
              SelectionFilter,
              selectionOptionList );
            field.Layout = EuAdapter.DefaultFieldLayout;
            field.AddParameter ( Evado.Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );

            break;
          }
        case EvDataTypes.External_Selection_List:
        case EvDataTypes.External_RadioButton_List:
          {
            this.LogDebug ( "External_Selection_List filter" );
            selectionOptionList = this.getFilteredList_SelectionOptions ( Field, true );

            if ( selectionOptionList.Count <= 1 )
            {
              this.LogDebug ( "No Selection list options" );
              break;
            }

            var field = PageGroup.createSelectionListField (
              EuEntities.CONST_SELECTION_FIELD + FilterIndex,
              Field.Title,
              SelectionFilter,
              selectionOptionList );
            field.Layout = EuAdapter.DefaultFieldLayout;
            field.AddParameter ( Evado.Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );
            break;
          }
        case EvDataTypes.External_CheckBox_List:
          {
            this.LogDebug ( "External_CheckBox_List filter" );
            selectionOptionList = this.getFilteredList_SelectionOptions ( Field, false );

            if ( selectionOptionList.Count == 0 )
            {
              this.LogDebug ( "No CheckBox list options" );
              break;
            }


            var field = PageGroup.createCheckBoxListField (
              EuEntities.CONST_SELECTION_FIELD + FilterIndex,
              Field.Title,
              SelectionFilter,
              selectionOptionList );
            field.Layout = EuAdapter.DefaultFieldLayout;
            field.AddParameter ( Evado.Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );

            break;
          }
      }//END switch statment

      this.LogMethodEnd ( "getFilteredList_SelectionField" );
    }//END getFilteredList_SelectionField Query

    // ==============================================================================
    /// <summary>
    /// This method creates the record view pageMenuGroup containing a list of commands to 
    /// open the form record.
    /// </summary>
    /// <param name="Field">EdRecordField object.</param>
    /// <returns>List of EvOption objects</returns>
    //  ------------------------------------------------------------------------------
    private List<EvOption> getFilteredList_SelectionOptions (
      EdRecordField Field,
      bool IsSelectionList )
    {
      this.LogMethod ( "getFilteredList_SelectionOptions" );
      //
      // initialise the variables and objects.
      //
      List<EvOption> optionList = new List<EvOption> ( );
      String listId = Field.Design.ExSelectionListId;
      String category = Field.Design.ExSelectionListCategory.ToUpper ( );
      this.LogDebug ( "Field: {0}, List: {1}, Category: {2} ", Field.FieldId, listId, category );

      //
      // the category contains the category field then set the category value to this field value.
      //
      if ( category.Contains ( EdRecordField.CONST_CATEGORY_AUTO_FIELD_IDENTIFIER ) == true )
      {
        var autoCategory = category.Replace ( EdRecordField.CONST_CATEGORY_AUTO_FIELD_IDENTIFIER, String.Empty );

        this.LogDebug ( "autoCategory: {0} ", autoCategory );
        //
        // search the filterfields for a matching field identifier then retrieve the matching filter value
        // as the category for this selection list.
        //
        for ( int index = 0; index < this.queryLayout.FilterFieldIds.Length; index++ )
        {
          if ( this.queryLayout.FilterFieldIds [ index ] == autoCategory )
          {
            category = this.Session.EntitySelectionFilters [ index ];
          }
        }
        this.LogDebug ( "Auto Category: {0} ", category );

        //
        // the auto category selection value is empty exit.
        //
        if ( category == String.Empty )
        {
          return optionList;
        }
      }

      this.LogDebug ( "Parameters List: {0}, Category: {1} ", listId, category );
      //
      // get the external selection list options.
      //
      optionList = this.AdapterObjects.getSelectionOptions ( listId, category, false, IsSelectionList );

      this.LogDebug ( "optionList.Count: {0} ", optionList.Count );

      this.LogMethodEnd ( "getFilteredList_SelectionOptions" );
      return optionList;

    }//ENd Method.

    // ==============================================================================
    /// <summary>
    /// This method creates the record view pageMenuGroup containing a list of commands to 
    /// open the form record.
    /// </summary>
    /// <param name="PageObject">Evado.Model.Uniform.Page object to add the pageMenuGroup to.</param>
    //  ------------------------------------------------------------------------------
    private void getEntity_Summary_ListGroup (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getEntity_Summary_ListGroup" );
      this.LogDebug ( "PageObject.EditAccess {0}.", PageObject.EditAccess );

      this.LogDebug ( "EntityList.Count: " + this.Session.EntityList.Count );
      // 
      // Iterate through the record list generating a groupCommand to access each record
      // then append the groupCommand to the record pageMenuGroup view's groupCommand list.
      // 
      foreach ( Evado.Model.Digital.EdRecord entity in this.Session.EntityList )
      {
        this.LogDebug ( "LCS {0}, LT {1}, FC {2}", entity.Design.LinkContentSetting, entity.CommandTitle, entity.Fields.Count );

        //
        // Create the group list groupCommand object.
        //
        this.getGroupSummaryListGroup (
          entity,
          PageObject );

      }//END iteration loop


      this.LogMethodEnd ( "getEntity_Summary_ListGroup" );
    }//END getEntity_Summary_ListGroup method

    // ==============================================================================
    /// <summary>
    /// This method appends the milestone groupCommand to the page milestone list pageMenuGroup
    /// </summary>
    /// <param name="Entity">EvForm object</param>
    /// <param name="PageGroup"> Evado.Model.UniForm.Group</param>
    //  -----------------------------------------------------------------------------
    private void getGroupSummaryListGroup (
      EdRecord Entity,
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getGroupSummaryListGroup" );
      this.LogDebug ( "Entity.EntityId: " + Entity.EntityId );
      this.LogDebug ( "LinkContentSetting: " + Entity.Design.LinkContentSetting );
      this.LogDebug ( "DefaultPageLayout: " + Entity.Design.DefaultPageLayout );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Model.UniForm.Group ( );
      Evado.Model.UniForm.Field groupField = new Model.UniForm.Field ( );
      Evado.Model.UniForm.Command groupCommand = new Model.UniForm.Command ( );


      // 
      // Create the record display pageMenuGroup.
      // 
      pageGroup = PageObject.AddGroup ( String.Empty );
      pageGroup.CmdLayout = Evado.Model.UniForm.GroupCommandListLayouts.Horizontal_Orientation;
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Dynamic;
      pageGroup.AddParameter ( Model.UniForm.GroupParameterList.Percent_Width, 25 );

      foreach ( EdRecordField field in Entity.Fields )
      {
        this.LogDebug ( "fid {0} - {1} V: {2}. ", field.FieldId, field.TypeId, field.ItemValue );

        if ( field.Design.IsSummaryField == false
          && field.TypeId != EvDataTypes.Image
          && field.TypeId != EvDataTypes.Text
          && field.TypeId != EvDataTypes.Numeric
          && field.TypeId != EvDataTypes.Telephone_Number
          && field.TypeId != EvDataTypes.Selection_List
          && field.TypeId != EvDataTypes.Radio_Button_List
          && field.TypeId != EvDataTypes.Check_Box_List
          && field.TypeId != EvDataTypes.External_Selection_List
          && field.TypeId != EvDataTypes.External_RadioButton_List
          && field.TypeId != EvDataTypes.External_CheckBox_List )
        {
          continue;
        }

        //
        // skip all empty items.
        //
        if ( field.ItemValue == String.Empty
          || field.ItemValue.Contains ( "E+37" ) == true
          || field.ItemValue.Contains ( "E-35" ) == true )
        {
          continue;
        }


        this.LogDebug ( "FieldId {0}, value {1} ", field.FieldId, field.ItemValue );

        //
        // Define the layout.
        //
        Model.UniForm.FieldLayoutCodes layout = Model.UniForm.FieldLayoutCodes.Left_Justified;

        switch ( field.TypeId )
        {
          case EvDataTypes.Image:
            {
              groupField = pageGroup.createImageField (
                String.Empty,
                field.Title,
                field.ItemValue, 125, 0 );
              groupField.EditAccess = Model.UniForm.EditAccess.Disabled;
              groupField.Layout = layout;

              continue;
            }
          case EvDataTypes.Selection_List:
          case EvDataTypes.Check_Box_List:
          case EvDataTypes.Radio_Button_List:
          case EvDataTypes.Horizontal_Radio_Buttons:
          case EvDataTypes.External_Selection_List:
          case EvDataTypes.External_RadioButton_List:
          case EvDataTypes.External_CheckBox_List:
            {
              var value = this.getSelectionDescription ( field, field.ItemValue );

              groupField = pageGroup.createReadOnlyTextField (
                String.Empty,
                field.Title,
                value );
              groupField.EditAccess = Model.UniForm.EditAccess.Disabled;
              groupField.Layout = layout;

              continue;
            }

          default:
            {


              groupField = pageGroup.createReadOnlyTextField (
                String.Empty,
                field.Title,
                field.ItemValue );
              groupField.EditAccess = Model.UniForm.EditAccess.Disabled;
              groupField.Layout = layout;
              continue;
            }
        }//END type switch statement.
      }//END field iteration loop

      //
      // Define the pageMenuGroup groupCommand.
      //
      groupCommand = pageGroup.addCommand (
          Entity.Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Entities,
          Evado.Model.UniForm.ApplicationMethods.Get_Object );

      groupCommand.Id = Entity.Guid;
      groupCommand.SetGuid ( Entity.Guid );

      //
      // Define the link groupCommand.
      //
      EdRecordField commandField = Entity.getFirstHttpLinkField ( );

      if ( commandField != null )
      {
        this.LogDebug ( "FieldId {0}, value {1} ", commandField.FieldId, commandField.ItemValue );

        if ( commandField.ItemValue != String.Empty )
        {
          string title = commandField.Title;
          string url = commandField.ItemValue;

          if ( url.Contains ( "^" ) == true )
          {
            string [ ] arValue = commandField.ItemValue.Split ( '^' );

            url = arValue [ 0 ];
            title = arValue [ 1 ];
          }

          groupCommand = pageGroup.addCommand (
              title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Entities,
              Evado.Model.UniForm.ApplicationMethods.Get_Object );
          groupCommand.Type = Model.UniForm.CommandTypes.Http_Link;

          groupCommand.AddParameter ( Model.UniForm.CommandParameters.Link_Url, url );
        }
      }

      this.LogMethodEnd ( "getGroupSummaryListGroup" );

    }//END getGroupListCommand method

    // ==============================================================================
    /// <summary>
    /// This method returns the desciption value for a selection option value.
    /// </summary>
    /// <param name="Entity">EvForm object</param>
    /// <param name="PageGroup"> Evado.Model.UniForm.Group</param>
    //  -----------------------------------------------------------------------------
    private string getSelectionDescription ( EdRecordField Field, String Value )
    {
      this.LogMethod ( "getSelectionDescription" );
      //
      // initialise the methods variables and objects.
      //
      List<EvOption> optionList = Field.Design.OptionList;
      string [ ] arValue = Field.ItemValue.Split ( ';' );
      String outputText = String.Empty;
      String valueList = String.Empty;

      //
      // search selection list if the list is external
      //
      if ( Field.TypeId == EvDataTypes.External_CheckBox_List
        || Field.TypeId == EvDataTypes.External_RadioButton_List
        || Field.TypeId == EvDataTypes.External_Selection_List )
      {
        optionList = this.AdapterObjects.getSelectionOptions (
          Field.Design.ExSelectionListId, String.Empty, false, false );
      }

      //
      // iterate through the list options to retrieve the matching description.
      //
      foreach ( String str in arValue )
      {
        foreach ( EvOption option in optionList )
        {
          if ( option.Value != str )
          {
            continue;
          }

          if ( valueList.Contains ( option.Value ) == true )
          {
            continue;
          }

          valueList += ";" + option.Value;

          if ( outputText != String.Empty )
          {
            outputText += @"[[/br]]";
          }

          outputText += option.Description;
        }
      }

      return outputText;
    }//END Method

  }
}//END namespace