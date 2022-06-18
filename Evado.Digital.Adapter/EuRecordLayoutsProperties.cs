/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\Records.cs" company="EVADO HOLDING PTY. LTD.">
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
  public partial class EuRecordLayouts : EuClassAdapterBase
  {
    
    #region Class form property page methods

    private Evado.UniForm.Model.EuEditAccess _DesignAccess = Evado.UniForm.Model.EuEditAccess.Null;

    private Evado.UniForm.Model.EuEditAccess _InitialAccess = Evado.UniForm.Model.EuEditAccess.Null; 

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.EuCommand object.</param>
    /// <returns>Evado.UniForm.Model.EuAppData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.UniForm.Model.EuAppData GetRecordProperties_Object (
      Evado.UniForm.Model.EuCommand PageCommand )
    {
      this.LogMethod ( "GetRecordProperties_Object" );

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.EuAppData clientDataObject = new Evado.UniForm.Model.EuAppData ( );
      Guid formGuid = Guid.Empty;

      //
      // Determine if the user has access to this page and log and error if they do not.
      //
      if ( this.Session.UserProfile.hasManagementAccess == false )
      {
        this.LogIllegalAccess (
          this.ClassNameSpace + "GetRecordProperties_Object",
          this.Session.UserProfile );

        this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

        return null;
      }

      // 
      // Log access to page.
      // 
      this.LogPageAccess (
        this.ClassNameSpace + "GetRecordProperties_Object",
        this.Session.UserProfile );

      try
      {
        if ( this.getRecordObject ( PageCommand, false ) == false )
        {
          return this.Session.LastPage;
        }

        // 
        // Generate the client ResultData object for the UniForm client.
        // 
        this.getPropertiesDataObject ( clientDataObject );

        // 
        // Return the client ResultData object to the calling method.
        // 
        this.LogMethodEnd ( "GetRecordProperties_Object" );
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

      this.LogMethodEnd ( "GetRecordProperties_Object" );
      return this.Session.LastPage;

    }//END GetRecordProperties_Object method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="ClientDataObject">Evado.UniForm.Model.EuAppData object.</param>
    //  ------------------------------------------------------------------------------
    private void getPropertiesDataObject (
      Evado.UniForm.Model.EuAppData ClientDataObject )
    {
      this.LogMethod ( "getPropertiesDataObject" );

      // 
      // Initialise the client data object.
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
      ClientDataObject.Page.EditAccess = Evado.UniForm.Model.EuEditAccess.Disabled;

      if ( this.Session.UserProfile.hasManagementAccess == true )
      {
        ClientDataObject.Page.EditAccess = Evado.UniForm.Model.EuEditAccess.Enabled;
      }
      //
      // initialise the methods variables and objects.
      //
      this._DesignAccess = ClientDataObject.Page.EditAccess;
        
      //
      // Design access is only available for un-issued layouts.  After issue
      // parameter changes can be made provided that don't change the data integrity.
      //
      if ( ( this.Session.RecordLayout.State == EdRecordObjectStates.Form_Issued
          || this.Session.RecordLayout.State == EdRecordObjectStates.Withdrawn )
        && ( this.Session.UserProfile.hasAdministrationAccess == false ) )
      {
        this._DesignAccess = Evado.UniForm.Model.EuEditAccess.Disabled;
      }

      //
      // Set the initial configuration access.
      //
      this._InitialAccess = this._DesignAccess;

      if ( this.Session.RecordLayout.Design.Version >= 1 )
      {
        this._InitialAccess = Evado.UniForm.Model.EuEditAccess.Disabled;
      }
      this.LogDebug ( "Design Access {0}, Initial Acccess {1}",
        this._DesignAccess, this._InitialAccess );



      //
      // Set the user's edit access if they have configuration edit access.
      //
      this.LogValue ( "HasConfigrationEditAccess: "
        + this.Session.UserProfile.hasManagementAccess );

      this.LogValue ( "GENERATE FORM" );

      this.GetDataObject_PageCommands ( ClientDataObject.Page );

      //
      // Add the page layout commands
      //
      this.GetDataObject_LayoutCommands ( ClientDataObject.Page );

      //
      // Add the general field properties pageMenuGroup.
      //
      this.getPropertiesPage_GeneralGroup ( ClientDataObject.Page );

      //
      // Add the settings group
      //
      this.getPropertiesPage_SettingGroup ( ClientDataObject.Page );

      //
      // Add the form sections properties pageMenuGroup.
      //
      this.getPropertiesPage_SectionsGroup ( ClientDataObject.Page );

    }//END getPropertiesDataObject Method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">Evado.UniForm.Model.EuPage object.</param>
    //  ------------------------------------------------------------------------------
    private void getPropertiesPage_GeneralGroup (
      Evado.UniForm.Model.EuPage Page )
    {
      this.LogMethod ( "getProperties_GeneralPageGroup" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.EuGroup pageGroup = new Evado.UniForm.Model.EuGroup ( );
      Evado.UniForm.Model.EuCommand pageCommand = new Evado.UniForm.Model.EuCommand ( );
      Evado.UniForm.Model.EuField groupField = new Evado.UniForm.Model.EuField ( );
      Evado.UniForm.Model.EuParameter parameter = new Evado.UniForm.Model.EuParameter ( );
      List<EvOption> optionList = new List<EvOption> ( );
      //
      // Define the general properties pageMenuGroup..
      //
      pageGroup = Page.AddGroup (
        EdLabels.Form_Properties_General_Group_Title);
      pageGroup.Layout = Evado.UniForm.Model.EuGroupLayouts.Full_Width;

      pageGroup.SetCommandBackBroundColor (
        Evado.UniForm.Model.EuGroupParameters.BG_Mandatory,
        Evado.UniForm.Model.EuBackgroundColours.Red );

      //
      // Define the form save commands.
      //
      this.GetDataObject_GroupCommands ( pageGroup );

      //
      // Define the page layout selection
      //
      this.GetDataObject_LayoutCommands ( pageGroup );

      //
      // Set the entity type
      //
      if ( this.Session.RecordLayout.Design.TypeId == EdRecordTypes.Null )
      {
        this.Session.RecordLayout.Design.TypeId = EdRecordTypes.Normal_Record;
      }

      //
      // Form title
      //
      groupField = pageGroup.createTextField (
        EdRecord.FieldNames.Layout_Id.ToString ( ),
        EdLabels.Label_Form_Id,
        this.Session.RecordLayout.LayoutId,
        10 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;
      groupField.EditAccess = this._InitialAccess;

      //
      // Form title
      //
      groupField = pageGroup.createTextField (
        EdRecord.FieldNames.RecordPrefix,
        EdLabels.Record_Layout_Record_Prefix_Field_Label,
        this.Session.RecordLayout.Design.RecordPrefix,
        5 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;
      groupField.EditAccess = this._InitialAccess;

      //
      // Form title
      //
      groupField = pageGroup.createTextField (
        EdRecord.FieldNames.Object_Title.ToString ( ),
        EdLabels.Form_Title_Field_Label,
        this.Session.RecordLayout.Design.Title,
        50 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Form reference
      //
      groupField = pageGroup.createTextField (
        EdRecord.FieldNames.Reference.ToString ( ),
        EdLabels.Form_Reference_Field_Label,
        this.Session.RecordLayout.Design.HttpReference,
        50 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Form Instructions
      //
      groupField = pageGroup.createFreeTextField (
        EdRecord.FieldNames.Instructions.ToString ( ),
        EdLabels.Form_Instructions_Field_Title,
        this.Session.RecordLayout.Design.Instructions,
        50, 4 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;


      //
      // Form category
      //
      groupField = pageGroup.createTextField (
        EdRecord.FieldNames.FormCategory.ToString ( ),
        EdLabels.Form_Category_Field_Title,
        this.Session.RecordLayout.Design.RecordCategory,
        50 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Form Update reason
      //
      optionList = EvStatics.getOptionsFromEnum ( typeof ( EdRecord.UpdateReasonList ), false );

      groupField = pageGroup.createSelectionListField (
        EdRecord.FieldNames.UpdateReason.ToString ( ),
        EdLabels.Form_Update_Reason_Field_Title,
        this.Session.RecordLayout.Design.UpdateReason,
        optionList );

      groupField.Layout = EuAdapter.DefaultFieldLayout;
      groupField.EditAccess = this._DesignAccess;

      //
      // Form Change description
      //
      groupField = pageGroup.createFreeTextField (
        EdRecord.FieldNames.Description.ToString ( ),
        EdLabels.Form_Description_Field_Title,
        this.Session.RecordLayout.Design.Description,
        90, 5 );

      groupField.Layout = EuAdapter.DefaultFieldLayout;
      groupField.EditAccess = this._DesignAccess;


    }//END getProperties_GeneralPageGroup Method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">Evado.UniForm.Model.EuPage object.</param>
    //  ------------------------------------------------------------------------------
    private void getPropertiesPage_SettingGroup (
      Evado.UniForm.Model.EuPage Page )
    {
      this.LogMethod ( "getPropertiesPage_SettingGroup" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.EuGroup pageGroup = new Evado.UniForm.Model.EuGroup ( );
      Evado.UniForm.Model.EuCommand pageCommand = new Evado.UniForm.Model.EuCommand ( );
      Evado.UniForm.Model.EuField groupField = new Evado.UniForm.Model.EuField ( );
      Evado.UniForm.Model.EuParameter parameter = new Evado.UniForm.Model.EuParameter ( );
      List<EvOption> optionList = new List<EvOption> ( );

      //
      // Define the general properties pageMenuGroup..
      //
      pageGroup = Page.AddGroup (
        EdLabels.Form_Properties_Settings_Group_Title );
      pageGroup.Layout = Evado.UniForm.Model.EuGroupLayouts.Full_Width;

      pageGroup.SetCommandBackBroundColor (
        Evado.UniForm.Model.EuGroupParameters.BG_Mandatory,
        Evado.UniForm.Model.EuBackgroundColours.Red );

      this.GetDataObject_GroupCommands ( pageGroup );

      //
      // Layout field readonly display format settings
      //
      optionList = EvStatics.getOptionsFromEnum ( typeof ( EdRecord.FieldReadonlyDisplayFormats ), false );


      groupField = pageGroup.createSelectionListField (
        EdRecord.FieldNames.FieldReadonlyDisplayFormat.ToString ( ),
        EdLabels.EntityLayout_FieldDisplayFormat_Field_Title,
        this.Session.RecordLayout.Design.FieldReadonlyDisplayFormat,
        optionList );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Form Update reason
      //
      optionList = this.AdapterObjects.Settings.GetRoleOptionList ( false );

      groupField = pageGroup.createCheckBoxListField (
        EdRecord.FieldNames.ReadAccessRoles.ToString ( ),
        EdLabels.Record_Layout_AccessRole_Field_Label,
        this.Session.RecordLayout.Design.EditAccessRoles,
        optionList );

      groupField.Layout = EuAdapter.DefaultFieldLayout;
      groupField.EditAccess = this._DesignAccess;


      groupField = pageGroup.createCheckBoxListField (
        EdRecord.FieldNames.EditAccessRoles.ToString ( ),
        EdLabels.Record_Layout_EditRole_Field_Label,
        this.Session.RecordLayout.Design.ReadAccessRoles,
        optionList );

      groupField.Layout = EuAdapter.DefaultFieldLayout;
      groupField.EditAccess = this._DesignAccess;

      //
      // Layout author only draft record access
      //
      optionList = EvStatics.getOptionsFromEnum ( typeof ( EdRecord.AuthorAccessList ), false );

      this.LogDebug ( "Author Access option list length: ", optionList );
      this.LogDebug ( "RecordLayout.Design.AuthorAccess: ", this.Session.RecordLayout.Design.AuthorAccess );

      groupField = pageGroup.createSelectionListField (
        EdRecord.FieldNames.AuthorAccess.ToString ( ),
        EdLabels.Record_Layout_Author_Access_Setting_Field_Title,
        this.Session.RecordLayout.Design.AuthorAccess,
        optionList );
      groupField.Layout = EuAdapter.DefaultFieldLayout;
      groupField.EditAccess = this._DesignAccess;

      //
      // Enable display if chiled entities.
      //
      groupField = pageGroup.createBooleanField (
        EdRecord.FieldNames.DisplayRelatedEntities.ToString ( ),
        EdLabels.EntityLayout_Display_Related_Entities_Field_Title,
        this.Session.RecordLayout.Design.DisplayRelatedEntities );

      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Layout author only edit record access
      //
      optionList = EvStatics.getOptionsFromEnum ( typeof ( EdRecord.ParentTypeList ), false );

      groupField = pageGroup.createSelectionListField (
        EdRecord.FieldNames.ParentType.ToString ( ),
        EdLabels.Record_Layout_Parent_Object_Type_Field_Title,
        this.Session.RecordLayout.Design.ParentType,
        optionList );
      groupField.Layout = EuAdapter.DefaultFieldLayout;
      groupField.EditAccess = this._DesignAccess;

      //
      // Select the hierachical entities this entity can be referenced to.
      //

      if ( this.Session.RecordLayout.Design.ParentType == EdRecord.ParentTypeList.Entity )
      {
        optionList = this.AdapterObjects.GetIssuedEntityOptions ( false );

        groupField = pageGroup.createCheckBoxListField (
          EdRecord.FieldNames.ParentEntities.ToString ( ),
          EdLabels.Record_Layout_Parent_Entity_Selection_Field_Title,
          this.Session.RecordLayout.Design.ParentEntities,
          optionList );
        groupField.Layout = EuAdapter.DefaultFieldLayout;
      }
      //
      // Default field layout 
      //
      optionList = EvStatics.getOptionsFromEnum ( typeof ( Evado.UniForm.Model.EuFieldLayoutCodes ), false );

      if ( this.Session.RecordLayout.Design.DefaultPageLayout == null )
      {
        this.Session.RecordLayout.Design.DefaultPageLayout = EuAdapter.DefaultFieldLayout.ToString ( );
      }

      groupField = pageGroup.createSelectionListField (
        EdRecord.FieldNames.DefaultPageLayout.ToString ( ),
        EdLabels.Form_Llink_Default_Layout_Field_Title,
        this.Session.RecordLayout.Design.DefaultPageLayout,
        optionList );

      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Record LLnk content setting
      //
      optionList = EvStatics.getOptionsFromEnum ( typeof ( EdRecord.LinkContentSetting ), false );

      if ( this.Session.RecordLayout.Design.LinkContentSetting == EdRecord.LinkContentSetting.Null )
      {
        this.Session.RecordLayout.Design.LinkContentSetting = EdRecord.LinkContentSetting.Default;
      }

      groupField = pageGroup.createSelectionListField (
        EdRecord.FieldNames.LinkContentSetting,
        EdLabels.Record_Link_Content_Setting_Field_Title,
        this.Session.RecordLayout.Design.LinkContentSetting,
        optionList );

      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Record header layout settings
      //
      optionList = EvStatics.getOptionsFromEnum ( typeof ( EdRecord.HeaderFormat ), false );

      if ( this.Session.RecordLayout.Design.HeaderFormat == EdRecord.HeaderFormat.Null )
      {
        this.Session.RecordLayout.Design.HeaderFormat = EdRecord.HeaderFormat.Default;
      }

      groupField = pageGroup.createSelectionListField (
        EdRecord.FieldNames.HeaderFormat,
        EdLabels.RecordLayout_Header_Format_Field_Title,
        this.Session.RecordLayout.Design.HeaderFormat,
        optionList );

      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Record footer layout settings
      //
      optionList = EvStatics.getOptionsFromEnum ( typeof ( EdRecord.FooterFormat ), false );

      if ( this.Session.RecordLayout.Design.FooterFormat == EdRecord.FooterFormat.Null )
      {
        this.Session.RecordLayout.Design.FooterFormat = EdRecord.FooterFormat.Default;
      }

      groupField = pageGroup.createSelectionListField (
        EdRecord.FieldNames.FooterFormat,
        EdLabels.RecordLayout_Footer_Format_Field_Title,
        this.Session.RecordLayout.Design.FooterFormat,
        optionList );

      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Form CS Script
      //
      groupField = pageGroup.createBooleanField (
        EdRecord.FieldNames.HasCsScript.ToString ( ),
        EdLabels.Form_Cs_Script_Field_Title,
        this.Session.RecordLayout.Design.hasCsScript );

      groupField.Layout = EuAdapter.DefaultFieldLayout;
      groupField.EditAccess = this._DesignAccess;

      this.LogMethodEnd ( "getPropertiesPage_SettingGroup" );
    }//END getProperties_GeneralPageGroup Method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">Evado.UniForm.Model.EuAppData object.</param>
    //  ------------------------------------------------------------------------------
    private void getPropertiesPage_SectionsGroup (
      Evado.UniForm.Model.EuPage Page )
    {
      this.LogMethod ( "getProperties_SectionsPageGroup" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.EuGroup pageGroup = new Evado.UniForm.Model.EuGroup ( );
      Evado.UniForm.Model.EuCommand groupCommand = new Evado.UniForm.Model.EuCommand ( );
      Evado.UniForm.Model.EuField groupField = new Evado.UniForm.Model.EuField ( );
      Evado.UniForm.Model.EuParameter parameter = new Evado.UniForm.Model.EuParameter ( );
      String stFieldList = String.Empty;

      //
      // Define the section properties pageMenuGroup..
      //
      pageGroup = Page.AddGroup (
        EdLabels.Form_Properties_Sections_Group_Title );
      pageGroup.Layout = Evado.UniForm.Model.EuGroupLayouts.Full_Width;
      pageGroup.CmdLayout = Evado.UniForm.Model.EuGroupCommandListLayouts.Vertical_Orientation;

      //
      // Add the new form section object.

      groupCommand = pageGroup.addCommand ( EdLabels.Form_Section_New_Section_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Record_Layouts.ToString ( ),
        Evado.UniForm.Model.EuMethods.Get_Object );

      groupCommand.AddParameter ( EdRecordSection.FormSectionClassFieldNames.Sectn_No.ToString ( ), "-1" );
      groupCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Form_Properties_Section_Page );
      groupCommand.SetBackgroundDefaultColour ( Evado.UniForm.Model.EuBackgroundColours.Purple );

      this.LogValue ( "No of form sections: " + this.Session.RecordLayout.Design.FormSections.Count );

      //
      // Iterate through the sections.
      //
      foreach ( EdRecordSection formSection in this.Session.RecordLayout.Design.FormSections )
      {
        groupCommand = pageGroup.addCommand ( formSection.LinkText,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Record_Layouts.ToString ( ),
          Evado.UniForm.Model.EuMethods.Get_Object );

        groupCommand.AddParameter ( EdRecordSection.FormSectionClassFieldNames.Sectn_No.ToString ( ), formSection.No );
        groupCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Form_Properties_Section_Page );
      }

      this.LogValue ( "After No of form sections: " + this.Session.RecordLayout.Design.FormSections.Count );

      this.LogMethodEnd ( "getProperties_SectionsPageGroup" );

    }//END getProperties_SectionsPageGroup Method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Form properties Section page.

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.EuCommand object.</param>
    /// <returns>Evado.UniForm.Model.EuAppData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.UniForm.Model.EuAppData getPropertiesSectionObject (
      Evado.UniForm.Model.EuCommand PageCommand )
    {
      this.LogMethod ( "getFormPropertiesSection" );

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.EuAppData clientDataObject = new Evado.UniForm.Model.EuAppData ( );
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
    /// <param name="ClientDataObject">Evado.UniForm.Model.EuAppData object.</param>
    //  ------------------------------------------------------------------------------
    private void getPropertiesSectionDataObject (
      Evado.UniForm.Model.EuAppData ClientDataObject )
    {
      this.LogMethod ( "getPropertiesSectionDataObject" );
      this.LogDebug ( "FormSection.No: " + this.Session.FormSection.No );
      this.LogDebug ( "FormSection.Title: " + this.Session.FormSection.Title );
      this.LogDebug ( "FormSection.DisplayRoles: " + this.Session.FormSection.ReadAccessRoles );
      this.LogDebug ( "FormSection.EditRoles: " + this.Session.FormSection.EditAccessRoles );
      this.LogDebug ( "HasConfigrationEditAccess: " + this.Session.UserProfile.hasManagementAccess );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.EuGroup pageGroup = new Evado.UniForm.Model.EuGroup ( );
      Evado.UniForm.Model.EuCommand pageCommand = new Evado.UniForm.Model.EuCommand ( );
      Evado.UniForm.Model.EuField pageField = new Evado.UniForm.Model.EuField ( );
      Evado.UniForm.Model.EuParameter parameter = new Evado.UniForm.Model.EuParameter ( );
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
      ClientDataObject.Page.EditAccess = Evado.UniForm.Model.EuEditAccess.Enabled;

      if ( this.Session.UserProfile.hasManagementAccess == true )
      {
        ClientDataObject.Page.EditAccess = Evado.UniForm.Model.EuEditAccess.Enabled;
      }

      if ( this.Session.RecordLayout.State != EdRecordObjectStates.Form_Issued
        && this.Session.RecordLayout.State != EdRecordObjectStates.Withdrawn )
      {
        ClientDataObject.Page.EditAccess = Evado.UniForm.Model.EuEditAccess.Disabled;
      }

      //
      // Set the user's edit access if they have configuration edit access.
      //


      if ( this.Session.UserProfile.hasManagementAccess == true )
      {
        ClientDataObject.Page.EditAccess = Evado.UniForm.Model.EuEditAccess.Enabled;
      }

      pageGroup = ClientDataObject.Page.AddGroup (
        EdLabels.FormProperties_Section_Group_Text,
        Evado.UniForm.Model.EuEditAccess.Inherited );
      pageGroup.Layout = Evado.UniForm.Model.EuGroupLayouts.Full_Width;

      //
      // Form No
      //
      pageField = pageGroup.createTextField (
        EdRecordSection.FormSectionClassFieldNames.Sectn_No.ToString ( ),
        EdLabels.Form_Section_No_Field_Label,
        this.Session.FormSection.No.ToString ( ),
        50 );
      pageField.Layout = EuAdapter.DefaultFieldLayout;
      pageField.EditAccess = Evado.UniForm.Model.EuEditAccess.Enabled;

      //
      // Form title
      //
      pageField = pageGroup.createTextField (
        EdRecordSection.FormSectionClassFieldNames.Sectn_Title.ToString ( ),
        EdLabels.Form_Section_Title_Field_Label,
        this.Session.FormSection.Title,
        50 );
      pageField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Form Instructions
      //
      pageField = pageGroup.createFreeTextField (
        EdRecordSection.FormSectionClassFieldNames.Sectn_Instructions.ToString ( ),
        EdLabels.Form_Section_Instructions_Field_Label,
        EdLabels.Form_Section_Instructions_Field_Description,
        this.Session.FormSection.Instructions,
        90, 5 );
      pageField.Layout = EuAdapter.DefaultFieldLayout;

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
      pageField.Layout = EuAdapter.DefaultFieldLayout;

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
      pageField.EditAccess = Evado.UniForm.Model.EuEditAccess.Inherited;

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
      // get the list of display roles.
      //
      optionList = new List<EvOption> ( );

      optionList = this.AdapterObjects.Settings.GetRoleOptionList ( false );

      //
      // The form section user display roles 
      //
      pageField = pageGroup.createCheckBoxListField (
        EdRecordSection.FormSectionClassFieldNames.Sectn_Display_Roles.ToString ( ),
        EdLabels.Form_Section_User_Display_Roles_Field_Label,
        EdLabels.Form_Section_User_Display_Roles_Field_Description,
        this.Session.FormSection.ReadAccessRoles,
        optionList );
      pageField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // get the list of edit roles.
      //
      pageField = pageGroup.createCheckBoxListField (
        EdRecordSection.FormSectionClassFieldNames.Sectn_Edit_Roles.ToString ( ),
        EdLabels.Form_Section_User_Edit_Roles_Field_Label,
        EdLabels.Form_Section_User_Edit_Roles_Field_Description,
        this.Session.FormSection.EditAccessRoles,
        optionList );
      pageField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Add the command to save the page content.
      //
      pageCommand = pageGroup.addCommand (
        EdLabels.Form_Properties_Section_Save_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Record_Layouts.ToString ( ),
        Evado.UniForm.Model.EuMethods.Custom_Method );

      pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Form_Properties_Page );
      pageCommand.setCustomMethod ( Evado.UniForm.Model.EuMethods.Get_Object );
      pageCommand.SetGuid ( this.Session.RecordLayout.Guid );
      pageCommand.AddParameter ( EuRecordLayouts.CONST_UPDATE_SECTION_COMMAND_PARAMETER, "1" );


    }//END getPropertiesDataObject Method

    // ==================================================================================
    /// <summary>
    /// Ttis method updates the form record values with the groupCommand parameter values.
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.EuCommand objects.</param>
    //  ----------------------------------------------------------------------------------
    private void updateSectionValues (
      Evado.UniForm.Model.EuCommand PageCommand )
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
      foreach ( Evado.UniForm.Model.EuParameter parameter in PageCommand.Parameters )
      {
        if ( parameter.Name.Contains ( "Sectn_" ) == false )
        {
          this.LogDebug ( parameter.Name + " > " + parameter.Value + " >> SKIPPED" );
          continue;
        }

        this.LogDebug ( parameter.Name + " > " + parameter.Value + " >> UPDATED" );

        try
        {
          EdRecordSection.FormSectionClassFieldNames fieldName = Evado.Model.EvStatics.parseEnumValue<EdRecordSection.FormSectionClassFieldNames> ( parameter.Name );

          this.Session.FormSection.setValue ( fieldName, parameter.Value );
        }
        catch ( Exception Ex )
        {
          LogValue ( "updateSectionValues method exception: \r\n"
            + Evado.Digital.Model.EvcStatics.getException ( Ex ) );
        }

      }// End iteration loop

      this.LogDebug ( "FormSection.No: " + this.Session.FormSection.No );
      this.LogDebug ( "FormSection.Title: " + this.Session.FormSection.Title );
      this.LogDebug ( "FormSection.Instructions: " + this.Session.FormSection.Instructions );
      this.LogDebug ( "FormSection.UserDisplayRoles: " + this.Session.FormSection.ReadAccessRoles );
      this.LogDebug ( "FormSection.UserEditRoles: " + this.Session.FormSection.EditAccessRoles );

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

      this.LogDebug ( "EntityLayout.Design.FormSections.Count.Count: " + this.Session.RecordLayout.Design.FormSections.Count );
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