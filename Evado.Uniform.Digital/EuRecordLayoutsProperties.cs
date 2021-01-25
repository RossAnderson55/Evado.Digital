/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\Records.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2020 EVADO HOLDING PTY. LTD..  All rights reserved.
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
using Evado.Bll.Clinical;
using Evado.Model.Digital;
// using Evado.Web;

namespace Evado.UniForm.Clinical
{
  /// <summary>
  /// This class defines the application base classs that is used to terminate the 
  /// hosted application objects.
  /// 
  /// This class terminates the Customer object.
  /// </summary>
  public partial class EuRecordLayouts : EuClassAdapterBase
  {
    
    #region Class form property page methods

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData GetLayoutPropoerties_Object (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "GetLayoutPropoerties_Object" );

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
      Guid formGuid = Guid.Empty;

      //
      // Determine if the user has access to this page and log and error if they do not.
      //
      if ( this.Session.UserProfile.hasManagementAccess == false )
      {
        this.LogIllegalAccess (
          this.ClassNameSpace + "GetLayoutPropoerties_Object",
          this.Session.UserProfile );

        this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

        return null;
      }

      // 
      // Log access to page.
      // 
      this.LogPageAccess (
        this.ClassNameSpace + "GetLayoutPropoerties_Object",
        this.Session.UserProfile );

      try
      {
        if ( this.getLayoutObject ( PageCommand, false ) == false )
        {
          return this.Session.LastPage;
        }

        string value = PageCommand.GetParameter ( EuRecordLayouts.CONST_UPDATE_SECTION_COMMAND_PARAMETER );

        //
        // Update the session form object if a new section is added to the page.
        //
        if ( value == "1" )
        {
          // 
          // Update the object.
          // 
          this.updateObjectValues ( PageCommand );

          //
          // Update the section table values.
          //
          this.updateSectionValues ( PageCommand );
        }

        // 
        // Generate the client ResultData object for the UniForm client.
        // 
        this.getPropertiesDataObject ( clientDataObject );

        // 
        // Return the client ResultData object to the calling method.
        // 
        this.LogMethodEnd ( "GetLayoutPropoerties_Object" );
        return clientDataObject;
      }
      catch ( Exception Ex )
      {
        // 
        // On an exception raised create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.Record_Retrieve_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      this.LogMethodEnd ( "GetLayoutPropoerties_Object" );
      return this.Session.LastPage;

    }//END getFormPropertiesObject method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="ClientDataObject">Evado.Model.UniForm.AppData object.</param>
    //  ------------------------------------------------------------------------------
    private void getPropertiesDataObject (
      Evado.Model.UniForm.AppData ClientDataObject )
    {
      this.LogMethod ( "getPropertiesDataObject" );

      // 
      // Initialise the client ResultData object.
      // 
      if ( this.Session.RecordLayout.Guid != Guid.Empty )
      {
        ClientDataObject.Id = Guid.NewGuid ( );
        ClientDataObject.Title =
          String.Format ( EdLabels.Form_Page_Title,
            this.Session.RecordLayout.LayoutId,
            this.Session.RecordLayout.Title );
        ClientDataObject.Page.Id = ClientDataObject.Id;
        ClientDataObject.Page.PageDataGuid = ClientDataObject.Id;
        ClientDataObject.Page.PageId = this.Session.RecordLayout.LayoutId;
      }
      else
      {
        ClientDataObject.Id = Guid.NewGuid ( );
        ClientDataObject.Title = EdLabels.Form_Page_New_Form_Title;
        ClientDataObject.Page.Id = ClientDataObject.Id;
        ClientDataObject.Page.PageDataGuid = ClientDataObject.Id;
      }
      ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Disabled;

      if ( this.Session.UserProfile.hasManagementAccess == true )
      {
        ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
      }

      //
      // Set the user's edit access if they have configuration edit access.
      //
      this.LogValue ( "HasConfigrationEditAccess: "
        + this.Session.UserProfile.hasManagementAccess );

      this.LogValue ( "GENERATE FORM" );

      this.setFormPageLayoutCommands ( ClientDataObject.Page, true );

      //
      // Add the general field properties pageMenuGroup.
      //
      this.getProperties_GeneralPageGroup ( ClientDataObject.Page );

      //
      // Add the form sections properties pageMenuGroup.
      //
      this.getProperties_SectionsPageGroup ( ClientDataObject.Page );

    }//END getPropertiesDataObject Method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void getProperties_GeneralPageGroup (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getProperties_GeneralPageGroup" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );
      Evado.Model.UniForm.Field pageField = new Evado.Model.UniForm.Field ( );
      Evado.Model.UniForm.Parameter parameter = new Evado.Model.UniForm.Parameter ( );
      List<EvOption> optionList = new List<EvOption> ( );

      //
      // Define the general properties pageMenuGroup..
      //
      pageGroup = Page.AddGroup (
        EdLabels.Form_Properties_General_Group_Title,
        Evado.Model.UniForm.EditAccess.Inherited );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;

      pageGroup.SetCommandBackBroundColor (
        Model.UniForm.GroupParameterList.BG_Mandatory,
        Model.UniForm.Background_Colours.Red );

      //
      // Add the save commandS for the page.
      //
      if ( this.Session.UserProfile.hasManagementAccess == true )
      {
        Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

        this.setFormSaveGroupCommands ( pageGroup );
      }

      //
      // Create the project selection list,
      // If the project is Global then only display the Global project identifier and not
      // a selection list.
      //
        pageField = pageGroup.createTextField (
          EdRecord.RecordFieldNames.ApplivcationId.ToString ( ),
          EdLabels.Label_Project_Id,
          this.Session.RecordLayout.ApplicationId, 20 );
        pageField.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
        pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      //
      // Form type selection list.
      //
      optionList = EdRecord.getFormTypes ( );

      pageField = pageGroup.createSelectionListField (
        EdRecord.RecordFieldNames.TypeId.ToString ( ),
        EdLabels.Form_Type_Field_Label,
        this.Session.RecordLayout.Design.TypeId.ToString ( ),
        optionList );
      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      //
      // Form title
      //
      pageField = pageGroup.createTextField (
        EdRecord.RecordFieldNames.Layout_Id.ToString ( ),
        EdLabels.Label_Form_Id,
        this.Session.RecordLayout.LayoutId,
        10 );
      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      //
      // Form title
      //
      pageField = pageGroup.createTextField (
        EdRecord.RecordFieldNames.Title.ToString ( ),
        EdLabels.Form_Title_Field_Label,
        this.Session.RecordLayout.Design.Title,
        50 );
      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      //
      // Form Instructions
      //
      pageField = pageGroup.createFreeTextField (
        EdRecord.RecordFieldNames.Instructions.ToString ( ),
        EdLabels.Form_Instructions_Field_Title,
        this.Session.RecordLayout.Design.Instructions,
        50, 4 );
      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      //
      // Form Update reason
      //
      optionList = EvUserProfile.getRoleOptionList( 
        this.Session.Application.RoleList, false ) ;

      pageField = pageGroup.createCheckBoxListField (
        EdRecord.RecordFieldNames.RecordAccessRole.ToString ( ),
        EdLabels.Record_Layout_AccessRole_Field_Label,
        this.Session.RecordLayout.Design.UpdateReason,
        optionList );

      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      //
      // Form Update reason
      //
      optionList = EvStatics.Enumerations.getOptionsFromEnum ( typeof ( EdRecord.UpdateReasonList ), false );

      pageField = pageGroup.createSelectionListField (
        EdRecord.RecordFieldNames.UpdateReason.ToString ( ),
        EdLabels.Form_Update_Reason_Field_Title,
        this.Session.RecordLayout.Design.UpdateReason,
        optionList );

      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      //
      // Form Change description
      //
      pageField = pageGroup.createFreeTextField (
        EdRecord.RecordFieldNames.Description.ToString ( ),
        EdLabels.Form_Description_Field_Title,
        this.Session.RecordLayout.Design.Description,
        90, 5 );

      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      //
      // Form reference
      //
      pageField = pageGroup.createTextField (
        EdRecord.RecordFieldNames.Reference.ToString ( ),
        EdLabels.Form_Reference_Field_Label,
        this.Session.RecordLayout.Design.HttpReference,
        50 );
      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      //
      // Form category
      //
      pageField = pageGroup.createTextField (
        EdRecord.RecordFieldNames.FormCategory.ToString ( ),
        EdLabels.Form_Category_Field_Title,
        this.Session.RecordLayout.Design.RecordCategory,
        50 );
      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      //
      // Form CS Script
      //
      pageField = pageGroup.createBooleanField (
        EdRecord.RecordFieldNames.HasCsScript.ToString ( ),
        EdLabels.Form_Cs_Script_Field_Title,
        this.Session.RecordLayout.Design.hasCsScript );
      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;

    }//END getProperties_GeneralPageGroup Method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.AppData object.</param>
    //  ------------------------------------------------------------------------------
    private void getProperties_SectionsPageGroup (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getPropertiesDataObject" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Command groupCommand = new Evado.Model.UniForm.Command ( );
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      Evado.Model.UniForm.Parameter parameter = new Evado.Model.UniForm.Parameter ( );

      List<EvOption> optionList = new List<EvOption> ( );
      String stFieldList = String.Empty;
      optionList.Add ( new EvOption ( ) );

      foreach ( EdRecordField field in this.Session.RecordLayout.Fields )
      {
        optionList.Add ( new EvOption (
          field.FieldId,
          field.FieldId
          + EdLabels.Space_Hypen
          + field.Title ) );
      }

      //
      // Define the section properties pageMenuGroup..
      //
      pageGroup = Page.AddGroup (
        EdLabels.Form_Properties_Sections_Group_Title,
        Evado.Model.UniForm.EditAccess.Inherited );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;
      pageGroup.CmdLayout = Model.UniForm.GroupCommandListLayouts.Vertical_Orientation;

      //
      // Add the new form section object.

      groupCommand = pageGroup.addCommand ( EdLabels.Form_Section_New_Section_Command_Title,
        EuAdapter.APPLICATION_ID,
        EuAdapterClasses.Record_Layouts.ToString ( ),
        Model.UniForm.ApplicationMethods.Get_Object );

      groupCommand.AddParameter ( EdRecordSection.FormSectionClassFieldNames.Sectn_No.ToString ( ), "-1" );
      groupCommand.SetPageId ( EvPageIds.Form_Properties_Section_Page );
      groupCommand.SetBackgroundDefaultColour ( Model.UniForm.Background_Colours.Purple );

      this.LogValue ( "No of form sections: " + this.Session.RecordLayout.Design.FormSections.Count );

      //
      // Iterate through the sections.
      //
      foreach ( EdRecordSection formSection in this.Session.RecordLayout.Design.FormSections )
      {
        groupCommand = pageGroup.addCommand ( formSection.LinkText,
          EuAdapter.APPLICATION_ID,
          EuAdapterClasses.Record_Layouts.ToString ( ),
          Model.UniForm.ApplicationMethods.Get_Object );

        groupCommand.AddParameter ( EdRecordSection.FormSectionClassFieldNames.Sectn_No.ToString ( ), formSection.No );
        groupCommand.SetPageId ( EvPageIds.Form_Properties_Section_Page );
      }

      this.LogValue ( "After No of form sections: " + this.Session.RecordLayout.Design.FormSections.Count );
      // 
      // Save the session ResultData so it is available for the next user generated groupCommand.
      // 
      this.SaveSessionObjects ( );


    }//END getProperties_SectionsPageGroup Method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Form properties Section page.

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData getFormPropertiesSectionObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getFormPropertiesSection" );

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
      Guid formGuid = Guid.Empty;
      int SectionNo = 0;

      try
      {
        string value = PageCommand.GetParameter ( EdRecordSection.FormSectionClassFieldNames.Sectn_No.ToString ( ) );
        this.LogDebug ( "Parameter value: " + value );

        if ( value != String.Empty )
        {
          if ( int.TryParse ( value, out SectionNo ) == false )
          {
            SectionNo = 0;
          }
        }

        if ( this.getSection ( SectionNo ) == false )
        {
          return null;
        }

        // 
        // Generate the client ResultData object for the UniForm client.
        // 
        this.getPropertiesSectionDataObject ( clientDataObject );

        this.LogMethodEnd ( "getFormPropertiesSection" );
        // 
        // Return the client ResultData object to the calling method.
        // 
        return clientDataObject;
      }
      catch ( Exception Ex )
      {
        // 
        // On an exception raised create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.Form_Retrieve_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }
      this.LogMethodEnd ( "getFormPropertiesSection" );
      return null;

    }//END getFormPropertiesObject method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="ClientDataObject">Evado.Model.UniForm.AppData object.</param>
    //  ------------------------------------------------------------------------------
    private void getPropertiesSectionDataObject (
      Evado.Model.UniForm.AppData ClientDataObject )
    {
      this.LogMethod ( "getPropertiesSectionDataObject" );
      this.LogDebug ( "FormSection.No: " + this.Session.FormSection.No );
      this.LogDebug ( "FormSection.Title: " + this.Session.FormSection.Title );
      this.LogDebug ( "FormSection.DisplayRoles: " + this.Session.FormSection.UserDisplayRoles );
      this.LogDebug ( "FormSection.EditRoles: " + this.Session.FormSection.UserEditRoles );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );
      Evado.Model.UniForm.Field pageField = new Evado.Model.UniForm.Field ( );
      Evado.Model.UniForm.Parameter parameter = new Evado.Model.UniForm.Parameter ( );
      List<EvOption> optionList = new List<EvOption> ( );
      optionList.Add ( new EvOption ( ) );

      foreach ( EdRecordField field in this.Session.RecordLayout.Fields )
      {
        optionList.Add ( new EvOption (
          field.FieldId,
          field.FieldId
          + EdLabels.Space_Hypen
          + field.Title ) );
      }

      // 
      // Initialise the client ResultData object.
      // 
      if ( this.Session.RecordLayout.Guid != Guid.Empty )
      {
        ClientDataObject.Id = Guid.NewGuid ( );
        ClientDataObject.Title =
          String.Format (
          EdLabels.FormProperties_Section_Page_Title,
          this.Session.RecordLayout.LayoutId,
          this.Session.RecordLayout.Title,
          this.Session.FormSection.LinkText );
        ClientDataObject.Page.Id = ClientDataObject.Id;
        ClientDataObject.Page.PageDataGuid = ClientDataObject.Id;
        ClientDataObject.Page.PageId = this.Session.RecordLayout.LayoutId;
      }
      else
      {
        ClientDataObject.Id = Guid.NewGuid ( );
        ClientDataObject.Title = EdLabels.Form_Page_New_Form_Title;
        ClientDataObject.Page.Id = ClientDataObject.Id;
        ClientDataObject.Page.PageDataGuid = ClientDataObject.Id;
      }
      ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      if ( this.Session.UserProfile.hasManagementAccess == true )
      {
        ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
      }

      //
      // Set the user's edit access if they have configuration edit access.
      //
      this.LogDebug ( "HasConfigrationEditAccess: " + this.Session.UserProfile.hasManagementAccess );


      if ( this.Session.UserProfile.hasManagementAccess == true )
      {
        ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
      }

      pageGroup = ClientDataObject.Page.AddGroup (
        EdLabels.FormProperties_Section_Group_Text,
        Evado.Model.UniForm.EditAccess.Inherited );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;

      //
      // Form No
      //
      pageField = pageGroup.createTextField (
        EdRecordSection.FormSectionClassFieldNames.Sectn_No.ToString ( ),
        EdLabels.Form_Section_No_Field_Label,
        this.Session.FormSection.No.ToString ( ),
        50 );
      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;
      pageField.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      //
      // Form title
      //
      pageField = pageGroup.createTextField (
        EdRecordSection.FormSectionClassFieldNames.Sectn_Title.ToString ( ),
        EdLabels.Form_Section_Title_Field_Label,
        this.Session.FormSection.Title,
        50 );
      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;
      pageField.EditAccess = Evado.Model.UniForm.EditAccess.Inherited;

      //
      // Form Instructions
      //
      pageField = pageGroup.createFreeTextField (
        EdRecordSection.FormSectionClassFieldNames.Sectn_Instructions.ToString ( ),
        EdLabels.Form_Section_Instructions_Field_Label,
        EdLabels.Form_Section_Instructions_Field_Description,
        this.Session.FormSection.Instructions,
        90, 5 );
      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      optionList = new List<EvOption> ( );

      this.LogDebug ( "FormSection.Order: " + this.Session.FormSection.Order );
      foreach ( EdRecordSection section in this.Session.RecordLayout.Design.FormSections )
      {
        if ( section.Order < this.Session.FormSection.Order )
        {
          this.LogDebug ( "secttion.Order: " + section.Order + " BEFORE SECTION" );

          var value = String.Format ( EdLabels.Form_Section_Order_Before_Text, section.Title );
          optionList.Add ( new EvOption ( ( section.Order - 1 ).ToString ( ), value ) );
        }
        if ( section.Order == this.Session.FormSection.Order )
        {
          this.LogDebug ( "secttion.Order: " + section.Order + " CURRENT" );
          optionList.Add ( new EvOption (
            this.Session.FormSection.Order.ToString ( ),
            this.Session.FormSection.Title ) );
        }
        if ( section.Order > this.Session.FormSection.Order )
        {
          this.LogDebug ( "secttion.Order: " + section.Order + " AFTER SECTION" );
          var value = String.Format ( EdLabels.Form_Section_Order_After_Text, section.Title );
          optionList.Add ( new EvOption ( ( section.Order + 1 ).ToString ( ), value ) );
        }
      }

      //
      // The form section order 
      //
      pageField = pageGroup.createSelectionListField (
        EdRecordSection.FormSectionClassFieldNames.Sectn_Order.ToString ( ),
        EdLabels.Form_Section_Order_Field_Label,
        this.Session.FormSection.Order.ToString ( ),
        optionList );
      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      /*
      //
      // The form section field id 
      //
      pageField = pageGroup.createSelectionListField (
        EvFormSection.FormSectionClassFieldNames.Sectn_Field_Id.ToString ( ),
        EdLabels.Form_Section_Field_ID_Field_Label,
        this.SessionObjects.FormSection.FieldId,
        optionList );
      pageField.Layout = EuPageGenerator.ApplicationFieldLayout;

      //
      // Form Field value
      //
      pageField = pageGroup.createTextField (
        EvFormSection.FormSectionClassFieldNames.Sectn_Field_Value.ToString ( ),
        EdLabels.Form_Section_Field_Value_Field_Label,
        this.SessionObjects.FormSection.FieldValue,
        50 );
      pageField.Layout = EuPageGenerator.ApplicationFieldLayout;
      pageField.EditAccess = Evado.Model.UniForm.EditAccess.Inherited;

      //
      // form secton on open display section.
      //
      pageField = pageGroup.createBooleanField (
        EvFormSection.FormSectionClassFieldNames.Sectn_On_Open_Visible.ToString ( ),
        EdLabels.Form_Section_Visible_On_Open_Field_Label,
        this.SessionObjects.FormSection.OnOpenVisible );
      pageField.Layout = EuPageGenerator.ApplicationFieldLayout;

      //
      // Form on field value match display field field
      //
      pageField = pageGroup.createBooleanField (
        EvFormSection.FormSectionClassFieldNames.Sectn_On_Match_Visible.ToString ( ),
        EdLabels.Form_Section_Visible_Field_Value_Matches_Field_Label,
        this.SessionObjects.FormSection.OnMatchVisible );
      pageField.Layout = EuPageGenerator.ApplicationFieldLayout;
      */
      //
      // get the list of form roles.
      //
      optionList = new List<EvOption> ( );

      optionList.Add ( EvStatics.Enumerations.getOption ( EdRecord.FormAccessRoles.Record_Author ) );
      optionList.Add ( EvStatics.Enumerations.getOption ( EdRecord.FormAccessRoles.Record_Reader ) );

      //
      // The form section user display roles 
      //
      pageField = pageGroup.createCheckBoxListField (
        EdRecordSection.FormSectionClassFieldNames.Sectn_Display_Roles.ToString ( ),
        EdLabels.Form_Section_User_Display_Roles_Field_Label,
        EdLabels.Form_Section_User_Display_Roles_Field_Description,
        this.Session.FormSection.UserDisplayRoles,
        optionList );
      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      //
      // Add the command to save the page content.
      //
      pageCommand = pageGroup.addCommand (
        EdLabels.Form_Properties_Section_Save_Command_Title,
        EuAdapter.APPLICATION_ID,
        EuAdapterClasses.Record_Layouts.ToString ( ),
        Model.UniForm.ApplicationMethods.Custom_Method );

      pageCommand.SetPageId ( EvPageIds.Form_Properties_Page );
      pageCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.Get_Object );
      pageCommand.SetGuid ( this.Session.RecordLayout.Guid );
      pageCommand.AddParameter ( EuRecordLayouts.CONST_UPDATE_SECTION_COMMAND_PARAMETER, "1" );

      // 
      // Save the session ResultData so it is available for the next user generated groupCommand.
      // 
      this.SaveSessionObjects ( );
      this.LogMethodEnd ( "getPropertiesSectionDataObject" );

    }//END getPropertiesDataObject Method

    // ==================================================================================
    /// <summary>
    /// Ttis method updates the form record values with the groupCommand parameter values.
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command objects.</param>
    //  ----------------------------------------------------------------------------------
    private void updateSectionValues (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogInitMethod ( "updateSectionValues" );
      this.LogDebug ( "PageCommand.Parameters.Count: " + PageCommand.Parameters.Count );

      //
      // Exit if update section not set.
      //
      string value = PageCommand.GetParameter ( EuRecordLayouts.CONST_UPDATE_SECTION_COMMAND_PARAMETER );
      if ( value != "1" )
      {
        return;
      }

      // 
      // Iterate through the parameter values updating the ResultData object
      // 
      foreach ( Evado.Model.UniForm.Parameter parameter in PageCommand.Parameters )
      {
        if ( parameter.Name.Contains ( "Sectn_" ) == false )
        {
          this.LogDebug ( parameter.Name + " > " + parameter.Value + " >> SKIPPED" );
          continue;
        }

        this.LogDebug ( parameter.Name + " > " + parameter.Value + " >> UPDATED" );

        try
        {
          EdRecordSection.FormSectionClassFieldNames fieldName = Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue<EdRecordSection.FormSectionClassFieldNames> ( parameter.Name );

          this.Session.FormSection.setValue ( fieldName, parameter.Value );
        }
        catch ( Exception Ex )
        {
          LogValue ( "updateSectionValues method exception: \r\n"
            + Evado.Model.Digital.EvcStatics.getException ( Ex ) );
        }

      }// End iteration loop

      this.LogDebug ( "FormSection.No: " + this.Session.FormSection.No );
      this.LogDebug ( "FormSection.Title: " + this.Session.FormSection.Title );
      this.LogDebug ( "FormSection.Instructions: " + this.Session.FormSection.Instructions );
      this.LogDebug ( "FormSection.UserDisplayRoles: " + this.Session.FormSection.UserDisplayRoles );
      this.LogDebug ( "FormSection.UserEditRoles: " + this.Session.FormSection.UserEditRoles );

      //
      // Update teh common form section object.
      //
      if ( this.Session.RecordLayout.Design.FormSections.Count > 0
        && this.Session.FormSection.No > 0 )
      {
        this.LogDebug ( "Updating the section values" );

        for ( int index = 0; index < this.Session.RecordLayout.Design.FormSections.Count; index++ )
        {
          if ( this.Session.RecordLayout.Design.FormSections [ index ].No == this.Session.FormSection.No )
          {
            this.Session.RecordLayout.Design.FormSections [ index ] = this.Session.FormSection;

            this.LogDebug ( this.Session.FormSection.LinkText + " >> UPDATED" );
          }
        }
      }

      //
      // Sort the section based on the current secton order.
      //
      this.Session.RecordLayout.Design.FormSections.Sort (
          delegate ( EdRecordSection p1, EdRecordSection p2 )
          {
            return p1.Order.CompareTo ( p2.Order );
          }
      );

      this.LogMethodEnd ( "updateSectionValues" );

    }//END updateObjectValue method.

    // ==============================================================================
    /// <summary>
    /// This method returns the selection secton object.
    /// </summary>
    /// <param name="No">Integer section index..</param>
    /// <returns>EvFormSection object</returns>
    //  ------------------------------------------------------------------------------
    private bool getSection ( int No )
    {
      this.LogInitMethod ( "getSection" );
      this.LogDebug ( "Section No: " + No );
      //
      // If No is less that 1 this is a new section.
      //
      if ( No < 0 )
      {
        this.LogDebug ( "Add new Section" );
        this.Session.FormSection = new EdRecordSection ( );
        this.Session.FormSection.No = this.getMaxSectionNo ( );
        this.Session.FormSection.No++;
        this.Session.FormSection.Order = getMaxSectionOrder ( );
        this.Session.FormSection.Order++;

        this.Session.RecordLayout.Design.FormSections.Add ( this.Session.FormSection );

        return true;
      }

      //
      // Iterate through the sections to find the section matching the No.
      //
      foreach ( EdRecordSection section in this.Session.RecordLayout.Design.FormSections )
      {
        if ( section.No == No )
        {
          this.Session.FormSection = section;
          this.LogDebug ( "Get Section" );
          return true;
        }
      }

      return false;
    }

    // ==============================================================================
    /// <summary>
    /// This method get the maximum section number 
    /// </summary>
    /// <returns>Integer: max value.</returns>
    //  ------------------------------------------------------------------------------
    private int getMaxSectionNo ( )
    {
      int value = 0;

      foreach ( EdRecordSection section in this.Session.RecordLayout.Design.FormSections )
      {
        this.LogDebug ( "Section No:" + section.No + ", value: " + value );
        if ( value < section.No )
        {
          value = section.No;
        }
      }

      return value;
    }

    // ==============================================================================
    /// <summary>
    /// This method get the maximum section order 
    /// </summary>
    /// <returns>Integer: max value.</returns>
    //  ------------------------------------------------------------------------------
    private int getMaxSectionOrder ( )
    {
      int value = 0;

      foreach ( EdRecordSection section in this.Session.RecordLayout.Design.FormSections )
      {
        this.LogDebug ( "Section Order:" + section.Order + ", value: " + value );
        if ( value < section.Order )
        {
          value = section.Order;
        }
      }

      return value;
    }

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

  }
}//END namespace