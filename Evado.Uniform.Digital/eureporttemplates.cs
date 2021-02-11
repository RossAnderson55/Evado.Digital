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
using Evado.Bll.Digital;
using  Evado.Model.Digital;
// using Evado.Web;

namespace Evado.UniForm.Digital
{
  /// <summary>
  /// This class defines the application base classs that is used to terminate the 
  /// hosted application objects.
  /// 
  /// This class terminates the Customer object.
  /// </summary>
  public class EuReportTemplates : EuClassAdapterBase
  {
    #region Class Initialisation
    /// <summary>
    /// This method initialises the class.
    /// </summary>
    public EuReportTemplates ( )
    {
      this.ClassNameSpace = "Evado.UniForm.Clinical.EuReportTemplates.";
    }

    /// <summary>
    /// This method initialises the class and passs in the user profile.
    /// </summary>
    public EuReportTemplates (
      EuGlobalObjects ApplicationObjects,
      EvUserProfileBase ServiceUserProfile,
      EuSession SessionObjects,
      String UniForm_BinaryFilePath,
      String UniForm_BinaryServiceUrl,
      EvClassParameters Settings )
    {
      this.AdapterObjects = ApplicationObjects;
      this.ServiceUserProfile = ServiceUserProfile;
      this.Session = SessionObjects;
      this.ClassParameters = Settings;
      this.UniForm_BinaryFilePath = UniForm_BinaryFilePath;
      this.UniForm_BinaryServiceUrl = UniForm_BinaryServiceUrl;
      this.ClassNameSpace = "Evado.UniForm.Clinical.EuReportTemplates.";


      this.LogInitMethod ( "ReportTemplates initialisation" );
      this.LogInit ( "ServiceUserProfile.UserId: " + ServiceUserProfile.UserId );
      this.LogInit ( "Session.UserProfile.UserId: " + this.Session.UserProfile.UserId );
      this.LogInit ( "Session.UserProfile.CommonName: " + this.Session.UserProfile.CommonName );
      this.LogInit ( "UniForm BinaryFilePath: " + this.UniForm_BinaryFilePath );
      this.LogInit ( "UniForm BinaryServiceUrl: " + this.UniForm_BinaryServiceUrl );

      this._Bll_ReportTemplates = new Evado.Bll.Digital.EvReportTemplates ( this.ClassParameters );

    }//END Method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class constants and variables.

    private Evado.Bll.Digital.EvReportTemplates _Bll_ReportTemplates = new Evado.Bll.Digital.EvReportTemplates ( );
    //
    // Initialise the page labels
    //
    public const string CONST_REPORT_PROJECT_ID = "RPID";
    public const string CONST_REPORT_SCOPE = "RSCP";
    public const string CONST_REPORT_TYPE = "RTYP";
    public const string CONST_REPORT_CATEGORY = "RCAT";
    private const string CONST_SAVE_REPORT = "RPTS";
    private const string CONST_QUERY_TABLE_FIELD_ID = "RQFT";
    private const string CONST_COLUMN_SELECTION_FIELD_ID = "RCSF";
    private const string CONST_COLUMN_TABLE_FIELD_ID = "RCFT";
    private const string CONST_UPLOAD_FIELD_ID = "RULFT";
    private const string CONST_SOURCE_RELOAD_FIELD_ID = "SRLD";

    private const string CONST_UPLOAD_NEW_FIELD_ID = "RULNFT";

    private const string CONST_NAME_SPACE = "Evado.UniForm.Clinical.EuReportTemplates.";

    bool _SelectionCommand = false;
    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class public methods

    // ==================================================================================
    /// <summary>
    /// This method gets the trial site object.
    /// 
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object</param>
    /// <returns>Evado.Model.UniForm.AppData</returns>
    //  ----------------------------------------------------------------------------------
    override public Evado.Model.UniForm.AppData getDataObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getDataObject" );
      this.LogDebug ( "PageCommand " + PageCommand.getAsString ( false, false ) );

      this.LogDebug ( "ReportTemplate.CommonName: " + this.Session.UserProfile.CommonName );

      try
      {
        // 
        // Initialise the methods variables and objects.
        // 
        bool sourcereload = false;
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );

        //
        // Determine if the user has access to this page and log and error if they do not.
        //
        if ( this.Session.UserProfile.hasManagementAccess == false )
        {
          this.LogIllegalAccess (
            this.ClassNameSpace + "ReportTemplates",
            this.Session.UserProfile );

          this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

           return this.Session.LastPage;;
        }

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          "Evado.UniForm.Clinical.ReportTemplates",
          this.Session.UserProfile );

        if ( PageCommand.hasParameter ( EuReportTemplates.CONST_SOURCE_RELOAD_FIELD_ID ) == true )
        {
          sourcereload = true;
        }

        //
        // Initialise the report object.
        //
        if ( this.Session.ReportSource == null )
        {
          this.Session.ReportSource = new EvReportSource ( );
          this.Session.ReportSource.QueryList = new List<EvReportQuery> ( );
          this.Session.ReportSource.ColumnList = new List<EvReportColumn> ( );
        }
        if ( this.Session.ReportTemplate == null )
        {
          this.Session.ReportTemplate = new EvReport ( );
        }
        if ( this.Session.ReportSourceList == null )
        {
          this.Session.ReportSourceList = new List<EvReportSource> ( );
        }
        if ( this.Session.ReportDesignTemplateList == null )
        {
          this.Session.ReportDesignTemplateList = new List<EvReport> ( );
        }
        if ( this.Session.ReportTemplateList_All == null )
        {
          this.Session.ReportTemplateList_All = new List<EvOption> ( );
        }

        if ( this.Session.ReportSourceList.Count == 0
          || sourcereload == true )
        {
          this.Session.ReportSourceList = this._Bll_ReportTemplates.getSourceList ( );

          this.LogDebug ( this._Bll_ReportTemplates.Log );
        }

        // 
        // Set the page type to control the DB query type.
        // 
        this.Session.setPageId ( PageCommand.GetPageId ( ) );

        this.LogDebug ( "PageId: " + this.Session.PageId );
        // 
        // Determine the method to be called
        // 
        switch ( PageCommand.Method )
        {
          case Evado.Model.UniForm.ApplicationMethods.List_of_Objects:
            {
              clientDataObject = this.getListObject ( PageCommand );
              break;
            }
          case Evado.Model.UniForm.ApplicationMethods.Get_Object:
            {
              clientDataObject = this.getObject ( PageCommand );
              break;
            }
          case Evado.Model.UniForm.ApplicationMethods.Create_Object:
            {
              clientDataObject = this.createObject ( PageCommand );
              break;
            }
          case Evado.Model.UniForm.ApplicationMethods.Save_Object:
          case Evado.Model.UniForm.ApplicationMethods.Delete_Object:
            {
              clientDataObject = this.updateObject ( PageCommand );
              break;
            }
        }//END Switch

        // 
        // Handle returned exceptions.
        // 
        if ( clientDataObject == null )
        {
          this.LogDebug ( " null application data returned." );
        }

        // 
        // return the client ResultData object.
        // 
        return clientDataObject;

      }
      catch ( Exception Ex )
      {
        this.LogException ( Ex );
      }
      return new Evado.Model.UniForm.AppData ( );

    }//END getClientDataObject method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class list methods

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData getListObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getReports_ListObject" );
      try
      {
        // 
        // Initialise the methods variables and objects.
        //      
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
        clientDataObject.Id = Guid.NewGuid ( );
        clientDataObject.Page.Id = clientDataObject.Id;

        this.getList_PageCommands ( clientDataObject.Page );

        //
        // Update the selection values.
        //
        this.getListObject_Update_Selection ( PageCommand );

        //
        // Upload the template
        //
        this.uploadReportTemplate ( PageCommand );

        //
        // Initialise the client ResultData object.
        //
        clientDataObject.Title = EdLabels.ReportTemplate_List_Page_Title;
        clientDataObject.Page.Title = clientDataObject.Title;

        //
        // display the upload link if requested.
        //
        this.getListObject_Upload_Group ( clientDataObject.Page );

        //
        // display the report template selection.
        //
        this.getListObject_Selection_Group ( clientDataObject.Page );

        // 
        // Add the trial organisation list to the page.
        // 
        this.getListObject_List_Group ( clientDataObject.Page );

        this.LogDebug ( "data.Page.Title: " + clientDataObject.Page.Title );

        return clientDataObject;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.Reports_List_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

       return this.Session.LastPage;;

    }//END getListObject method.

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private void getList_PageCommands (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getList_PageCommands" );

      // 
      // Initialise the methods variables and objects.
      //
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );

      //
      // Add the source refresh command
      //
      pageCommand = PageObject.addCommand (
        EdLabels.Report_emplates_Source_Data_Refresh_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.ReportTemplates.ToString ( ),
         Model.UniForm.ApplicationMethods.Custom_Method );

      pageCommand.AddParameter ( EuReportTemplates.CONST_SOURCE_RELOAD_FIELD_ID, "Yes" );
      pageCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.List_of_Objects );


      //
      // Add the upload command
      //
      if ( this.Session.PageId != EvPageIds.Report_Template_Upload )
      {
        pageCommand = PageObject.addCommand (
          EdLabels.ReportTemplate_Upload_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.ReportTemplates.ToString ( ),
          Model.UniForm.ApplicationMethods.Custom_Method );

        pageCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.List_of_Objects );

        pageCommand.AddParameter (
          Model.UniForm.CommandParameters.Page_Id,
          EvPageIds.Report_Template_Upload );

        return;
      }
    }


    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private void getListObject_Update_Selection (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getListObject_Update_Selection" );
      //
      // Define method variables and objects.
      //
      String parameterValue = String.Empty;
      this.Session.ReportCategory = String.Empty;


      if ( PageCommand.hasParameter ( Model.UniForm.CommandParameters.Custom_Method ) == true )
      {
        _SelectionCommand = true;
      }
      if ( PageCommand.hasParameter ( EuReportTemplates.CONST_REPORT_CATEGORY ) == true )
      {
        this.Session.ReportCategory = PageCommand.GetParameter ( EuReportTemplates.CONST_REPORT_CATEGORY );
      }
      // this.SessionObjects.ReportType = PageCommand.GetParameterValue<EvReport.ReportTypeCode> ( EuReportTemplates.CONST_REPORT_TYPE );

      if ( PageCommand.hasParameter ( EuReportTemplates.CONST_REPORT_SCOPE ) == true )
      {
        this.Session.ReportScope = PageCommand.GetParameter<EvReport.ReportScopeTypes> ( EuReportTemplates.CONST_REPORT_SCOPE );
      }
      this.LogDebug ( "ReportCategory: " + this.Session.ReportCategory );
      //this.LogDebugValue ( "ReportType: " + this.SessionObjects.ReportType );
      this.LogDebug ( "ReportScope: " + this.Session.ReportScope );

    }//END update selection values.

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private void getListObject_Upload_Group (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getListObject_Upload_Group" );
      //
      // Initialise method variables and objects.
      //
      Evado.Model.UniForm.Field groupField = new Model.UniForm.Field ( );
      Evado.Model.UniForm.Command groupCommand = new Model.UniForm.Command ( );

      //
      // exist if the page is not an upload page.
      //
      if ( this.Session.PageId != EvPageIds.Report_Template_Upload )
      {
        return;
      }

      if ( this.Session.ReportFileName == null )
      {
        this.Session.ReportFileName = String.Empty;
      }

      // 
      // Create the new pageMenuGroup.
      // 
      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
        EdLabels.ReportTemplate_Upload_Group_Title,
        Evado.Model.UniForm.EditAccess.Enabled );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      //
      // Define the report project selection list.
      //
      groupField = pageGroup.createBinaryFileField (
        EuReportTemplates.CONST_UPLOAD_FIELD_ID,
        EdLabels.ReportTemplate_Upload_Field_Title,
        this.Session.ReportFileName );

      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Add the normal save.
      //
      groupCommand = pageGroup.addCommand (
        EdLabels.ReportTemplate_Upload_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.ReportTemplates,
        Model.UniForm.ApplicationMethods.Custom_Method );

      groupCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.List_of_Objects );

      groupCommand.SetPageId ( EvPageIds.Report_Template_View );

      //
      // Add the save as new templae.
      //
      groupCommand = pageGroup.addCommand (
        EdLabels.ReportTemplate_Upload_New_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.ReportTemplates,
        Model.UniForm.ApplicationMethods.Custom_Method );

      groupCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.List_of_Objects );

      groupCommand.SetPageId ( EvPageIds.Report_Template_View );

      groupCommand.AddParameter ( EuReportTemplates.CONST_UPLOAD_NEW_FIELD_ID, "Yes" );


    }//END getReports_List_Upload_Group method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private bool uploadReportTemplate (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "uploadReportTemplate" );
      //
      // Define method variables and objects.
      //
      String parameterValue = String.Empty;

      //
      // Exit is the upload field identifier is not in the command.
      //
      if ( PageCommand.hasParameter ( EuReportTemplates.CONST_UPLOAD_FIELD_ID ) == false )
      {
        this.LogDebug ( "Upload field is empty." );
        return true;
      }

      this.LogDebug ( "Uploading the report template." );
      //
      // Initialise the methods variables and objects.
      //
      String fieldName = PageCommand.GetParameter ( EuReportTemplates.CONST_UPLOAD_FIELD_ID );
      String newfield = PageCommand.GetParameter ( EuReportTemplates.CONST_UPLOAD_NEW_FIELD_ID );

      this.LogDebug ( "fieldName: " + fieldName );
      this.LogDebug ( "newfield: " + newfield );

      //
      // If filename is empty.
      //
      if ( fieldName == String.Empty )
      {
        this.LogDebug ( "Filename is empty." );
        return true;
      }

      try
      {

        //
        // Read in the report template.
        //
        EvReport reportTemplate = Evado.Model.EvStatics.Files.readXmlFile<EvReport> (
          this.UniForm_BinaryFilePath,
          fieldName );

        this.LogDebug ( "Uploaded template ID: " + reportTemplate.ReportId );

        //
        // If there is not report  template add this as a new template.
        //
        if ( this.getReportTemplate ( reportTemplate.ReportId ) == false )
        {
          this.LogDebug ( "Template does not exist." );

          this.Session.ReportTemplate = reportTemplate;
          this.Session.ReportTemplate.Guid = Guid.Empty;
        }
        else
        {
          this.LogDebug ( "Template does exists." );
          //
          // template exists, but is to be updated.
          //
          if ( newfield != "Yes" )
          {
            this.LogDebug ( "Update existing  instance." );

            reportTemplate.Guid = this.Session.ReportTemplate.Guid;
            this.Session.ReportTemplate = reportTemplate;
          }
          else
          {
            this.LogDebug ( "new  instance." );

            this.Session.ReportTemplate = reportTemplate;
            this.Session.ReportTemplate.Guid = Guid.Empty;

            String reportId = this.Session.ReportTemplate.ReportId;
            if ( reportId.Length >= 15 )
            {
              reportId = reportId.Substring ( 0, 15 ) + "-NEW";
            }
            else
            {
              reportId = reportId + "-NEW";
            }

            this.Session.ReportTemplate.ReportId = reportId;
            this.Session.ReportTemplate.LastReportId = reportId;

          }//END new report instance

        }//END existing report

        this.LogDebug ( "Report Template Guid: " + this.Session.ReportTemplate.Guid );
        this.LogDebug ( "Report Template ID: " + this.Session.ReportTemplate.ReportId );

        //
        // set the save parameters.
        //
        this.Session.ReportTemplate.UpdateUserId = this.Session.UserProfile.UserId;
        this.Session.ReportTemplate.UserCommonName = this.Session.UserProfile.CommonName;
        this.LogDebug ( "ReportTemplate.UserCommonName: " + this.Session.ReportTemplate.UserCommonName );

        // 
        // update the object.
        // 
        EvEventCodes result = this._Bll_ReportTemplates.saveReport ( this.Session.ReportTemplate );

        // 
        // get the debug ResultData.
        // 
        this.LogDebugClass ( this._Bll_ReportTemplates.Log );

        // 
        // if an error state is returned create log the event.
        // 
        if ( result != EvEventCodes.Ok )
        {
          string StEvent = this._Bll_ReportTemplates.Log
            + " returned error message: " +  Evado.Model.Digital.EvcStatics.getEventMessage ( result );
          this.LogError (EvEventCodes.Database_Record_Update_Error, StEvent );

          this.ErrorMessage = "Report Template upload update error: "
            +  Evado.Model.Digital.EvcStatics.getEventMessage ( result );
          this.ErrorMessage = this.ErrorMessage.Replace ( "Identifier ", String.Empty );

          this.LogMethodEnd ( "getReports_UploadTemplate" );

          return false;
        }

        this.LogMethodEnd ( "getReports_UploadTemplate" );

        //
        // Empty the list of templates so it will be rebuilt the page is opened.
        //
        this.Session.ReportDesignTemplateList = new List<EvReport> ( );

        return true;


      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.Reports_List_Upload_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      this.LogMethodEnd ( "getReports_UploadTemplate" );

      return false;

    }//END getReports_UploadTemplate method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private void getListObject_Selection_Group (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getListObject_Selection_Group" );
      //
      // Initialise method variables and objects.
      Evado.Model.UniForm.Field groupField = new Model.UniForm.Field ( );
      Evado.Model.UniForm.Command groupCommand = new Model.UniForm.Command ( );
      List<EvOption> optionList = new List<EvOption> ( );

      // 
      // Create the new pageMenuGroup.
      // 
      Evado.Model.UniForm.Group pageGroup = Page.AddGroup (
        EdLabels.Reports_Selection_Group_Title,
        String.Empty,
        Evado.Model.UniForm.EditAccess.Enabled );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      //
      // Define the report Type selection
      //
      optionList = EvReportTemplates.getReportScopeList ( );

      groupField = pageGroup.createSelectionListField (
        EuReportTemplates.CONST_REPORT_SCOPE,
        EdLabels.Report_Scope_Field_Title,
        this.Session.ReportScope.ToString ( ),
        optionList );

      groupField.Layout = EuAdapter.DefaultFieldLayout;
      groupField.AddParameter ( Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );

      //
      // Add the selection pageMenuGroup groupCommand.
      //
      groupCommand = pageGroup.addCommand (
        EdLabels.Report_Selecton_Command_Title,
            EuAdapter.ADAPTER_ID,
            EuAdapterClasses.ReportTemplates.ToString ( ),
            Model.UniForm.ApplicationMethods.Custom_Method );

      groupCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.List_of_Objects );

      groupCommand.AddParameter ( Model.UniForm.CommandParameters.Page_Id,
         Evado.Model.Digital.EvPageIds.Operational_Report_List );

    }//END getReports_List_Selection_Group method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private void getListObject_List_Group (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getListObject_List_Group" );
      try
      {
        //
        // Initialise the methods variables and objects.
        //
        Evado.Model.UniForm.Command groupCommand = new Model.UniForm.Command ( );

        // 
        // Create the new pageMenuGroup.
        // 
        Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
          EdLabels.Reports_List_Group_Title,
          String.Empty,
          Evado.Model.UniForm.EditAccess.Inherited );
        pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
        pageGroup.CmdLayout = Evado.Model.UniForm.GroupCommandListLayouts.Vertical_Orientation;

        // 
        // Query and database.
        // 
        if ( this.Session.ReportDesignTemplateList.Count == 0
          || this._SelectionCommand == true )
        {
          this.Session.ReportDesignTemplateList = this._Bll_ReportTemplates.getReportList (
            EvReport.ReportTypeCode.Null,
            this.Session.ReportScope );

          this.Session.ReportTemplateList_All = this._Bll_ReportTemplates.getReportListAll ( );

          this.LogDebugClass ( this._Bll_ReportTemplates.Log );
        }

        if ( this.Session.ReportTemplateList_All.Count == 0 )
        {
          this.Session.ReportTemplateList_All = this._Bll_ReportTemplates.getReportListAll ( );

          this.LogDebugClass ( this._Bll_ReportTemplates.Log );
        }
        this.LogDebug ( "ReportDesignTemplateList.Count: " + this.Session.ReportDesignTemplateList.Count );

        // 
        // Add an empty report template to create a new report
        // 
        groupCommand = pageGroup.addCommand (
          EdLabels.ReportTemplate_New_Report_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.ReportTemplates.ToString ( ),
          Model.UniForm.ApplicationMethods.Create_Object );

        groupCommand.AddParameter ( Model.UniForm.CommandParameters.Page_Id,
           Evado.Model.Digital.EvPageIds.Report_Template_Form );

        groupCommand.SetBackgroundDefaultColour ( Model.UniForm.Background_Colours.Purple );

        // 
        // generate the page links.
        // 
        foreach ( EvReport template in this.Session.ReportDesignTemplateList )
        {
          String stReportTitle = String.Format (
            EdLabels.ReportTemplate_List_Command_Title,
            template.ReportId,
            template.ReportTitle,
            Evado.Model.EvStatics.enumValueToString ( template.ReportScope ) );

          groupCommand = pageGroup.addCommand (
             stReportTitle,
             EuAdapter.ADAPTER_ID,
             EuAdapterClasses.ReportTemplates.ToString ( ),
             Model.UniForm.ApplicationMethods.Get_Object );

          groupCommand.AddParameter ( Model.UniForm.CommandParameters.Page_Id,
             Evado.Model.Digital.EvPageIds.Report_Template_Form );

          groupCommand.AddParameter ( Model.UniForm.CommandParameters.Short_Title,
            template.ReportId );

          groupCommand.SetGuid ( template.Guid );

        }//END report template list iteration loop

        this.LogDebug ( "command object count: " + pageGroup.CommandList.Count );

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.Reports_List_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

    }//END getListObject method.

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class get object methods

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">List of paremeters to retrieve the selected object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData getObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getObject" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
      Guid reportTemplateGuid = Guid.Empty;
      String parameterValue = String.Empty;

      // 
      // if the parameter value exists then set the customerId
      // 
      reportTemplateGuid = PageCommand.GetGuid ( );
      this.LogDebug ( "Report template Guid: " + reportTemplateGuid );
      this.LogDebug ( "Current ReportTemplate.Guid: " + this.Session.ReportTemplate.Guid );

      // 
      // return if not trial id
      // 
      if ( reportTemplateGuid == Guid.Empty
        && this.Session.ReportTemplate.Guid == Guid.Empty )
      {
        this.LogDebug ( "REPORT TEMPLATE GUID IS EMPTY" );
         return this.Session.LastPage;;
      }

      try
      {
        //
        // retrieve the report template if it has not already been retrieved.
        //
        if ( this.Session.ReportTemplate.Guid != reportTemplateGuid )
        {
          this.LogDebug ( "Loading Report Template" );

          // 
          // Retrieve the customer object from the database via the DAL and BLL layers.
          // 
          this.Session.ReportTemplate = this._Bll_ReportTemplates.getReport ( reportTemplateGuid );

          this.LogDebugClass ( this._Bll_ReportTemplates.Log );
        }
        else
        {
          if ( PageCommand.hasParameter ( Model.UniForm.CommandParameters.Custom_Method ) == true )
          {
            this.LogDebug ( "Updating Report Template" );
            // 
            // Update the object.
            // 
            this.updateObjectValue ( PageCommand );

            //
            // Update the form object values.
            //
            this.updateQueryValues ( PageCommand );

            //
            // Update the form object values.
            //
            this.updateColumnValues ( PageCommand );
          }
        }

        this.LogDebug ( "SourceId: " + this.Session.ReportTemplate.SourceId );

        if ( this.Session.ReportTemplate.SourceId != String.Empty )
        {
          this.LogDebug ( "SourceId exists loading report source." );

          if ( this.getReportSource ( this.Session.ReportTemplate.SourceId ) == false )
          {
            this.ErrorMessage = String.Format (
              EdLabels.ReportTemplate_Source_Id_Error_Message,
              this.Session.ReportTemplate.SourceId );

             return this.Session.LastPage;;
          }
        }
        else
        {
          this.LogDebug ( "SourceId DOES NOT exists." );
        }

        this.updateTemplateSourceData ( );

        //
        // Update the report column selection parameters.
        //
        this.updateReportColumnSelection ( PageCommand );

        this.LogDebug ( "ReportId: " + this.Session.ReportTemplate.ReportId );
        this.LogDebug ( "ReportTitle: " + this.Session.ReportTemplate.ReportTitle );
        this.LogDebug ( "ReportScope: " + this.Session.ReportTemplate.ReportScope );
        this.LogDebug ( "ReportType: " + this.Session.ReportTemplate.ReportType );
        this.LogDebug ( "LayoutTypeId: " + this.Session.ReportTemplate.LayoutTypeId );

        // 
        // return the client ResultData object for the customer.
        // 
        this.getDataObject ( clientDataObject );

        return clientDataObject;

        // 
        // Save the customer object to the session
        // 

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.Reports_Page_Error_Mesage;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

       return this.Session.LastPage;;

    }//END getObject method

    // ==============================================================================
    /// <summary>
    /// This method updates the templates source data.
    /// </summary>
    /// <param name="ReportId">EvReport object containing the report template.</param>
    /// <returns>True: report was found.</returns>
    //  ------------------------------------------------------------------------------
    private void updateTemplateSourceData ( )
    {
      this.LogMethod ( "updateTemplateSourceData" );

      //
      // Get the project full option list 
      //
      if ( this.Session.ReportSource == null
        || this.Session.ReportTemplate == null )
      {
        return;
      }

      this.Session.ReportTemplate.SqlDataSource = this.Session.ReportSource.SqlQuery;
      this.Session.ReportTemplate.DataSourceId = this.Session.ReportSource.ReportSource;

      //
      // Update the query source data.
      //
      this.LogDebug ( "Updating query source parameters." );
      for ( int iQuery = 0; iQuery < this.Session.ReportTemplate.Queries.Length; iQuery++ )
      {
        EvReportQuery query = this.Session.ReportTemplate.Queries [ iQuery ];

        if ( query.QueryId == String.Empty )
        {
          continue;
        }

        EvReportQuery sourceQuery = this.Session.ReportSource.getQuery ( query.QueryId );

        query.QueryTitle = sourceQuery.QueryTitle;
        query.Prompt = sourceQuery.Prompt;
        query.FieldName = sourceQuery.FieldName;
        query.SelectionSource = sourceQuery.SelectionSource;
        query.DataType = sourceQuery.DataType;

        this.LogDebug ( "Query: " + query.QueryId + " updated." );
      }

      //
      // Update the column source data
      //
      this.LogDebug ( "Updating column source parameters." );
      for ( int iCol = 0; iCol < this.Session.ReportTemplate.Columns.Count; iCol++ )
      {
        EvReportColumn column = this.Session.ReportTemplate.Columns [ iCol ];

        EvReportColumn sourceColumn = this.Session.ReportSource.getColumn ( column.ColumnId );

        column.HeaderText = sourceColumn.HeaderText;
        column.SourceField = sourceColumn.SourceField;
        column.StyleWidth = sourceColumn.StyleWidth;
        column.DataType = sourceColumn.DataType;

        this.LogDebug ( "Column: " + column.ColumnId + " updated." );
      }


    }//END updateTemplateSourceDate method

    // ==============================================================================
    /// <summary>
    /// This method retrieves the report template from the list of report templates.
    /// </summary>
    /// <param name="ReportId">EvReport object containing the report source.</param>
    /// <returns>True: report was found.</returns>
    //  ------------------------------------------------------------------------------
    private bool getReportSource ( String SourceId )
    {
      this.LogMethod ( "getReportSource" );
      this.LogDebug ( "SourceId: " + SourceId );

      //
      // Reset the report source object.
      //
      this.Session.ReportSource = new EvReportSource ( );

      // 
      // Iterate through the report templates and retrieve the matching template
      // by it report ID.
      // 
      foreach ( EvReportSource source in this.Session.ReportSourceList )
      {
        if ( source.SourceId.ToLower ( ) == SourceId.ToLower ( ) )
        {
          this.Session.ReportSource = source;
          this.LogDebug ( "Source found and loaded into ReportSource." );
          this.LogDebug ( "ReportSource.SourceId: " + this.Session.ReportSource.SourceId );

          foreach ( EvReportColumn column in this.Session.ReportSource.ColumnList )
          {
            this.LogDebug ( "Column: " + column.ColumnId + " - " + column.HeaderText );
          }
          return true;
        }
      }//END report source iteration loop

      this.LogDebug ( "Source NOT found." );
      return false;

    }//END getReportSource method

    // ==============================================================================
    /// <summary>
    /// This method retrieves the report template from the list of report templates.
    /// </summary>
    /// <param name="ReportId">EvReport object containing the report source.</param>
    /// <returns>True: report was found.</returns>
    //  ------------------------------------------------------------------------------
    private bool getReportTemplate ( String ReportId )
    {
      this.LogMethod ( "getReportTemplate" );
      this.LogDebug ( "ReportId: " + ReportId );
      this.LogDebug ( "ReportDesignTemplateList.Count: " + this.Session.ReportDesignTemplateList.Count );

      //
      // Reset the report template object.
      //
      this.Session.ReportTemplate = new EvReport ( );

      // 
      // Iterate through the report templates and retrieve the matching template
      // by it report ID.
      // 
      foreach ( EvReport template in this.Session.ReportDesignTemplateList )
      {
        this.LogDebug ( "ReportID " + template.ReportId );

        if ( template.ReportId.ToLower ( ) == ReportId.ToLower ( ) )
        {
          this.Session.ReportTemplate = template;
          this.LogDebug ( "Report found and loaded into ReportTemplate." );
          this.LogDebug ( "ReportTemplate: " + this.Session.ReportTemplate.ReportId );

          foreach ( EvReportColumn column in this.Session.ReportTemplate.Columns )
          {
            this.LogDebug ( "Column: " + column.ColumnId + " - " + column.HeaderText );
          }
          return true;
        }
      }//END report source iteration loop

      this.LogDebug ( "Report NOT found." );
      return false;

    }//END getReportSource method

    // ==============================================================================
    /// <summary>
    /// This method retrieves the report template from the list of report templates.
    /// </summary>
    /// <param name="ReportId">EvReport object containing the report source.</param>
    /// <returns>True: Not a duplicate report.</returns>
    //  ------------------------------------------------------------------------------
    private bool duplicateReportValidation ( )
    {
      this.LogMethod ( "duplicateReportValidation" );
      this.LogDebug ( "ReportId: " + this.Session.ReportTemplate.ReportId );
      this.LogDebug ( "ReportDesignTemplateList.Count: " + this.Session.ReportTemplateList_All.Count );
      int reportIdCount = 0;

      // 
      // Iterate through the report templates and retrieve the matching template
      // by it report ID.
      // 
      foreach ( EvOption template in this.Session.ReportTemplateList_All )
      {
        if ( template.Value.ToLower ( ) == this.Session.ReportTemplate.ReportId.ToLower ( ) )
        {
          reportIdCount++;
        }
      }//END report source iteration loop

      //
      // if a report is found of the same name then return duplication = true
      //
      if ( this.Session.ReportTemplate.Guid == Evado.Model.EvStatics.CONST_NEW_OBJECT_ID
        & reportIdCount >01 )
      {
          this.LogDebug ( "New Report Duplicate Report Found" );
          return true;
      }

      //
      // if a more than one report found of the same name then return duplication = true
      //
      if ( reportIdCount > 1 )
      {
        this.LogDebug ( "Existing Report Duplicate Report Found" );
        return true;
      }

      this.LogDebug ( "No Duplicates found." );
      return false;

    }//END duplicateReportValidation method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="ClientDataObject">The customer object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject (
      Evado.Model.UniForm.AppData ClientDataObject )
    {
      this.LogMethod ( "getDataObject" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );
      Evado.Model.UniForm.Field pageField = new Evado.Model.UniForm.Field ( );

      String title = String.Format (
            EdLabels.ReportTemplate_Page_Title,
            this.Session.ReportTemplate.ReportId,
            this.Session.ReportTemplate.ReportTitle,
            this.Session.ReportTemplate.ReportType,
            this.Session.ReportTemplate.ReportScope,
            this.Session.ReportTemplate.Version );

      //
      // Initialise the client ResultData object.
      //
      ClientDataObject.Id = this.Session.ReportTemplate.Guid;
      ClientDataObject.Title = title;

      ClientDataObject.Page.Id = ClientDataObject.Id;
      ClientDataObject.Page.Title = ClientDataObject.Title;
      ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      if ( this.Session.UserProfile.hasAdministrationAccess == true
        || this.Session.UserProfile.hasManagementAccess == true )
      {
        ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
        if ( this.Session.ReportTemplate.Columns.Count == 0 )
        {
          this.Session.ReportTemplate.Columns.Add ( new EvReportColumn ( ) );
        }
        else
        {
          int index = this.Session.ReportTemplate.Columns.Count - 1;
          if ( this.Session.ReportTemplate.Columns [ index ].ColumnId != String.Empty )
          {
            this.Session.ReportTemplate.Columns.Add ( new EvReportColumn ( ) );
          }
        }
      }
      this.LogDebug ( "Edit Status: " + ClientDataObject.Page.EditAccess );
      //
      // add the page commands
      //
      this.getClientData_PageCommands ( ClientDataObject.Page );

      this.getDataObject_Download_Group ( ClientDataObject.Page );

      //
      // Set the general template properyties.
      //
      this.getDataObject_General_Group ( ClientDataObject.Page );

      //
      // Display the report query output.
      //
      this.getDataObject_Query_Group ( ClientDataObject.Page );

      //
      // select the page group to be displayed.
      //
      switch ( this.Session.PageId )
      {
        case EvPageIds.Report_Template_Column_Selection_Page:
          {
            //
            // Display the report collumn output.
            //
            this.getDataObject_Column_Selection_Group ( ClientDataObject.Page );
            break;
          }
        default:
          {
            //
            // Display the report collumn output.
            //
            this.getDataObject_Column_Structure_Group ( ClientDataObject.Page );
            break;
          }
      }

      this.LogMethodEnd ( "getDataObject" );

    }//END getDataObject Method

    // ==============================================================================
    /// <summary>
    /// This method updates the report column values.
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object</param>
    //  -----------------------------------------------------------------------------
    private void updateReportColumnSelection ( Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "updateReportColumnSelection" );
      this.LogDebug ( "ReportSource.SourceId: " + this.Session.ReportSource.SourceId );

      //
      // Exit if the selection field is not in the command.
      //
      if ( PageCommand.hasParameter ( EuReportTemplates.CONST_COLUMN_SELECTION_FIELD_ID ) == false )
      {
        this.LogDebug ( "The data field is not present." );
        return;
      }

      //
      // Initialise the methods variables and objects.
      //
      String selectedColumnIds = PageCommand.GetParameter (
        EuReportTemplates.CONST_COLUMN_SELECTION_FIELD_ID );

      this.LogDebug ( "selectedColumnIds: " + selectedColumnIds );

      #region Delete columns that are not selected.
      //
      // Delete the column sources that have not been selected.
      //
      if ( this.Session.ReportTemplate.Columns.Count > 0 )
      {
        this.LogDebug ( "DELETING UNSELECTED COLUMNS" );

        for ( int iCount = 0; iCount < this.Session.ReportTemplate.Columns.Count; iCount++ )
        {
          String columnId = this.Session.ReportTemplate.Columns [ iCount ].ColumnId;

          this.LogDebug ( "columnId: " + columnId );

          //
          // remove the template column source and decrement the counter.
          //
          if ( selectedColumnIds.Contains ( columnId ) == false )
          {
            this.LogDebug ( "DELETING: columnId: " + columnId );

            this.Session.ReportTemplate.Columns.RemoveAt ( iCount );
            iCount--;
          }
        }//END the Template report column iteraation loop.

      }//END template columns exist.

      #endregion

      #region add new columns.
      //
      // create an array of the selected column IDs
      //
      String [ ] arrSelectedColumnIds = selectedColumnIds.Split ( ';' );

      this.LogDebug ( " arrSelectedColumnIds.Length: " + arrSelectedColumnIds.Length );

      for ( int index = 0; index < arrSelectedColumnIds.Length; index++ )
      {
        if ( arrSelectedColumnIds [ index ] == String.Empty )
        {
          continue;
        }
        this.LogDebug ( "Array ColumnId: " + arrSelectedColumnIds [ index ] );

        //
        // Get the report template column
        //
        EvReportColumn templateColumn =
          this.Session.ReportTemplate.getColumn ( arrSelectedColumnIds [ index ] );

        this.LogDebug ( "templateColumn.ColumnId: " + templateColumn.ColumnId );

        EvReportColumn sourceColumn = this.Session.ReportSource.getColumn ( arrSelectedColumnIds [ index ] );

        //
        // If the column id is empty the column was not found so add it.
        //
        if ( templateColumn.ColumnId != String.Empty )
        {
          templateColumn.ColumnId = sourceColumn.ColumnId;
          templateColumn.HeaderText = sourceColumn.HeaderText;
          templateColumn.DataType = sourceColumn.DataType;
          templateColumn.SourceField = sourceColumn.SourceField;
        }
        else
        {
          this.LogDebug ( "ADD: columnId: " + arrSelectedColumnIds [ index ] );

          this.LogDebug ( "SOURCE: columnId: " + arrSelectedColumnIds [ index ] );

          templateColumn.ColumnId = sourceColumn.ColumnId;
          templateColumn.HeaderText = sourceColumn.HeaderText;
          templateColumn.DataType = sourceColumn.DataType;
          templateColumn.SourceField = sourceColumn.SourceField;

          this.LogDebug ( "ColumnId: " + templateColumn.ColumnId + " added to template" );

          this.Session.ReportTemplate.Columns.Add ( templateColumn );

        }//END template column is empty (not in the list)

        this.LogTextEnd ( "" );
      }//END selection column interation loop

      this.LogDebug ( "ReportTemplate.Columns.Count: " + this.Session.ReportTemplate.Columns.Count );

      #endregion

      this.LogMethodEnd ( "updateReportColumnSelection" );
    }

    // ==============================================================================
    /// <summary>
    /// This method compares a string value array with a value and returns true if it found.
    /// </summary>
    /// <param name="ValueArray">String value array</param>
    /// <param name="Value">String value</param>
    /// <returns>True: exists</returns>
    //  -----------------------------------------------------------------------------
    private bool hasValue ( String [ ] ValueArray, String Value )
    {
      foreach ( string value in ValueArray )
      {
        if ( value.ToLower ( ) == Value.ToLower ( ) )
        {
          return true;
        }
      }
      return false;
    }//END hasValue method

    // ==============================================================================
    /// <summary>
    /// This method generates the header pageMenuGroup for the page.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void getClientData_PageCommands (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getClientData_GroupCommands" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );

      if ( this.Session.UserProfile.hasAdministrationAccess == false
        && this.Session.UserProfile.hasManagementAccess == false )
      {
        return;
      }

      // 
      // Add the Data source selection page.
      // 
      switch ( this.Session.PageId )
      {
        case EvPageIds.Report_Template_Column_Selection_Page:
          {
            pageCommand = PageObject.addCommand (
              EdLabels.ReportTemplate_Page_Column_Structure_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.ReportTemplates.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.Custom_Method );

            pageCommand.SetGuid ( this.Session.ReportTemplate.Guid );
            pageCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.Get_Object );
            pageCommand.SetPageId ( EvPageIds.Report_Template_Form );

            break;
          }
        case EvPageIds.Report_Template_Form:
        default:
          {
            pageCommand = PageObject.addCommand (
              EdLabels.ReportTemplate_Page_Column_Selection_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.ReportTemplates.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.Custom_Method );

            pageCommand.SetGuid ( this.Session.ReportTemplate.Guid );
            pageCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.Get_Object );
            pageCommand.SetPageId ( EvPageIds.Report_Template_Column_Selection_Page );
            break;
          }
      }

      // 
      // Add the download template command
      // 
      pageCommand = PageObject.addCommand (
        EdLabels.ReportTemplate_Pate_Download_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.ReportTemplates.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Custom_Method );

      pageCommand.SetGuid ( this.Session.ReportTemplate.Guid );
      pageCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.Get_Object );
      pageCommand.SetPageId ( EvPageIds.Report_Template_Download );


      // 
      // Add the template save command
      // 
      pageCommand = PageObject.addCommand (
        EdLabels.ReportTemplate_Page_Save_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.ReportTemplates.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Save_Object );

      pageCommand.SetGuid ( this.Session.ReportTemplate.Guid );

      //
      // Insert the delete command
      //
      if ( this.Session.ReportTemplate.Guid != EvStatics.CONST_NEW_OBJECT_ID )
      {
        pageCommand = PageObject.addCommand (
          EdLabels.ReportTemplate_Page_Delete_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.ReportTemplates.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Delete_Object );

        pageCommand.SetGuid ( this.Session.ReportTemplate.Guid );
      }

      this.LogMethodEnd ( "getClientData_PageCommands" );

    }//END getClientData_PageCommands method.

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private void getDataObject_Download_Group (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getDataObject_Download_Group" );
      this.LogDebug ( "ReportTemplate.ReportId:" + this.Session.ReportTemplate.ReportId );

      //
      // exit if not a down load page.
      //
      if ( this.Session.PageId != EvPageIds.Report_Template_Download )
      {
        return;
      }

      //
      // Initialise method variables and objects.
      //
      PageObject.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      Evado.Model.UniForm.Field groupField = new Model.UniForm.Field ( );
      String xmlReport = String.Empty;
      String filename = this.Session.ReportTemplate.ReportId
        + "-" + this.Session.ReportTemplate.ReportTitle.Replace( " ", "-")
        + "-" + DateTime.Now.ToString ( "yy-MM-dd-hhmm" ) + ".EvReport.Xml";

      xmlReport = Evado.Model.EvStatics.SerialiseObject<EvReport> (
        this.Session.ReportTemplate );

      //
      // Save the file to disk for the link.
      //
      Evado.Model.EvStatics.Files.saveFile ( this.UniForm_BinaryFilePath, filename, xmlReport );

      //
      // Create the download link.
      //
      String linkUrl = this.UniForm_BinaryServiceUrl + filename;

      // 
      // Create the new pageMenuGroup.
      // 
      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
        EdLabels.ReportTemplate_Page_Download_Group_Title,
        Evado.Model.UniForm.EditAccess.Enabled );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      string fieldTitle = String.Format (
        EdLabels.ReportTemplate_Page_Download_Link_Text,
        this.Session.ReportTemplate.ReportId,
        this.Session.ReportTemplate.ReportTitle );

      //
      // Add the html field
      //
      groupField = pageGroup.createHtmlLinkField (
        String.Empty,
        fieldTitle,
        linkUrl );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      groupField.Description = EdLabels.ReportTemplate_Project_ID_Description ;

      this.LogMethodEnd ( "getDataObject_Download_Group" );

    }//END getDataObject_Download_Group method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private void getDataObject_General_Group (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getDataObject_General_Group" );
      this.LogDebug ( "ReportTemplate.IsAggregated:" + this.Session.ReportTemplate.IsAggregated );
      //
      // Initialise method variables and objects.
      //
      Evado.Model.UniForm.Field groupField = new Model.UniForm.Field ( );
      Evado.Model.UniForm.Command groupCommand = new Model.UniForm.Command ( );
      List<EvOption> optionList = new List<EvOption> ( );
      EvOption option = new EvOption ( );
      DateTime dValue =  Evado.Model.Digital.EvcStatics.CONST_DATE_NULL;

      // 
      // Create the new pageMenuGroup.
      // 
      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
        EdLabels.ReportTemplate_Page_General_Group_Title,
        Evado.Model.UniForm.EditAccess.Enabled );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;


      //
      // Define the source selection options.
      //
      optionList.Add ( new EvOption ( ) );

      if ( this.Session.ReportSourceList.Count > 0 )
      {
        foreach ( EvReportSource source in this.Session.ReportSourceList )
        {
          optionList.Add ( new EvOption ( source.SourceId, source.Name ) );
        }
      }

      this.LogDebug ( "optionList.Count: " + optionList.Count );

      this.LogDebug ( "SourceId: " + this.Session.ReportTemplate.SourceId );
      //
      // Add the source id field.
      //
      groupField = pageGroup.createSelectionListField (
        EvReport.ReportClassFieldNames.SourceId.ToString ( ),
        EdLabels.ReportTemplate_Source_ID_Field_Title,
        this.Session.ReportTemplate.SourceId,
        optionList );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      groupField.Description = EdLabels.ReportTemplate_Source_ID_Description ;
      groupField.AddParameter ( Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, "Yes" );

      //
      // Add the report id field.
      //
      this.LogDebug ( "ReportSource.Description: " + this.Session.ReportSource.Description );

      if ( this.Session.ReportSource.Description != String.Empty )
      {
        groupField = pageGroup.createReadOnlyTextField (
          EdLabels.ReportTemplate_Source_Description_Field_Title,
          this.Session.ReportSource.Description );
        groupField.Layout = EuAdapter.DefaultFieldLayout;
      }

      //
      // Add the report id field.
      //
      groupField = pageGroup.createTextField (
        EvReport.ReportClassFieldNames.ReportId.ToString ( ),
        EdLabels.ReportTemplate_Report_ID_Field_Title,
        this.Session.ReportTemplate.ReportId,
        20 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Add the report title field.
      //
      groupField = pageGroup.createTextField (
        EvReport.ReportClassFieldNames.ReportTitle.ToString ( ),
        EdLabels.ReportTemplate_Report_Title_Field_Title,
        this.Session.ReportTemplate.ReportTitle,
        80 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Add the report sub title field.
      //
      groupField = pageGroup.createTextField (
        EvReport.ReportClassFieldNames.ReportSubTitle.ToString ( ),
        EdLabels.ReportTemplate_Report_Sub_Title_Field_Title,
        this.Session.ReportTemplate.ReportSubTitle,
        80 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Layout options.
      //
      optionList = EvReport.getReportLayoutOptionList ( );
      //
      // Add the Rport Type  field.
      //
      groupField = pageGroup.createSelectionListField (
        EvReport.ReportClassFieldNames.LayoutTypeId,
        EdLabels.ReportTemplate_Layout_Field_Title,
        this.Session.ReportTemplate.LayoutTypeId,
        optionList );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Define the report type options.
      //
      /*
      optionList = EvReportTemplates.getReportTypeList (
        this.SessionObjects.UserProfile.RoleId );

      //
      // Add the Rport Type  field.
      //
      groupField = pageGroup.createSelectionListField (
        EvReport.ReportClassFieldNames.ReportType,
        EdLabels.ReportTemplate_Type_Field_Title,
        this.SessionObjects.ReportTemplate.ReportType,
        optionList );
      groupField.Layout = EuPageGenerator.ApplicationFieldLayout;
      */

      //
      // Define the report scope options.
      //
      optionList = EvReportTemplates.getReportScopeList ( );

      //
      // Add the Rport scop  field.
      //
      groupField = pageGroup.createSelectionListField (
        EvReport.ReportClassFieldNames.ReportScope,
        EdLabels.ReportTemplate_Scope_Field_Title,
        this.Session.ReportTemplate.ReportScope,
        optionList );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      groupField.Description = 
        EdLabels.ReportTemplate_Scope_Field_Description ;

      //
      // Add the Rport Type  field.
      //
      groupField = pageGroup.createBooleanField (
        EvReport.ReportClassFieldNames.IsAggregated,
        EdLabels.ReportTemplate_IsAgregated_Field_Title,
        this.Session.ReportTemplate.IsAggregated );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // A refresh command to update the report source Id.
      //
      this.getDataObject_GroupCommands ( pageGroup );


      this.LogMethodEnd ( "getDataObject_General_Group" );

    }//END getDataObject_General_Group method

    // ==============================================================================
    /// <summary>
    /// This method add the page group commansd
    /// </summary>
    /// <param name="PageGroup">Evado.Model.UniForm.Group object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_GroupCommands (
      Evado.Model.UniForm.Group PageGroup )
    {
      this.LogMethod ( "getDataObject_General_Group" );
      this.LogDebug ( "ReportTemplate.IsAggregated:" + this.Session.ReportTemplate.IsAggregated );
      //
      // Initialise method variables and objects.
      //
      Evado.Model.UniForm.Command groupCommand = new Model.UniForm.Command ( );
      //
      // A refresh command to update the report source Id.
      //
      groupCommand = PageGroup.addCommand (
        EdLabels.ReportTemplate_Page_Refresh_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.ReportTemplates.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Custom_Method );

      groupCommand.SetGuid ( this.Session.ReportTemplate.Guid );
      groupCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.Get_Object );

      if ( this.Session.ReportTemplate.Guid == EvStatics.CONST_NEW_OBJECT_ID )
      {
        groupCommand.SetPageId ( EvPageIds.Report_Template_Column_Selection_Page );
      }
      else
      {
        groupCommand.SetPageId ( EvPageIds.Report_Template_Form );
      }


      // 
      // Add the template save command
      // 
      groupCommand = PageGroup.addCommand (
        EdLabels.ReportTemplate_Page_Save_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.ReportTemplates.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Save_Object );

      groupCommand.SetGuid ( this.Session.ReportTemplate.Guid );

      //
      // Insert the delete command
      //
      if ( this.Session.ReportTemplate.Guid != EvStatics.CONST_NEW_OBJECT_ID )
      {
        groupCommand = PageGroup.addCommand (
          EdLabels.ReportTemplate_Page_Delete_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.ReportTemplates.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Delete_Object );

        groupCommand.SetGuid ( this.Session.ReportTemplate.Guid );
      }

      this.LogMethodEnd ( "getDataObject_General_Group" );
    }

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private void getDataObject_Query_Group (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getDataObject_Query_Group" );
      this.LogDebug ( "ReportTemplate.Queries.Length: " + this.Session.ReportTemplate.Queries.Length );
      this.LogDebug ( "ReportSource.QueryList.Count: " + this.Session.ReportSource.QueryList.Count );
      //
      // Initialise method variables and objects.
      //
      Evado.Model.UniForm.Field groupField = new Model.UniForm.Field ( );
      Evado.Model.UniForm.Command pageCommand = new Model.UniForm.Command ( );
      List<EvOption> sourceOptionList = new List<EvOption> ( );
      List<EvOption> mandatoryOptionList = new List<EvOption> ( );
      List<EvOption> operationOptionList = new List<EvOption> ( );

      if ( this.Session.ReportSource.QueryList.Count == 0 )
      {
        this.LogDebug ( "The report source query list is empty so do not display the group." );
        return;
      }

      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
        EdLabels.ReportTemplate_Query_Group_Title,
        Evado.Model.UniForm.EditAccess.Enabled );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      //
      // create the query option list 
      //
      sourceOptionList = this.Session.ReportSource.QueryOptionList ( true );

      mandatoryOptionList.Add ( new EvOption ( "True", "Yes" ) );
      mandatoryOptionList.Add ( new EvOption ( "False", "No" ) );

      operationOptionList = Evado.Model.EvStatics.Enumerations.getOptionsFromEnum ( typeof ( EvReport.Operators ), false );

      this.LogDebug ( "optionList.Count: " + sourceOptionList.Count );

      groupField = pageGroup.createTableField (
        CONST_QUERY_TABLE_FIELD_ID,
        EdLabels.ReportTemplate_Query_Selection_Field_Title,
        3 );

      groupField.Description =  EdLabels.ReportTemplate_Query_Group_Description ;

      groupField.Table.Header [ 0 ].No = 1;
      groupField.Table.Header [ 0 ].Text = EdLabels.ReportTemplate_Query_Column_1_Text;
      groupField.Table.Header [ 0 ].TypeId = EvDataTypes.Selection_List;
      groupField.Table.Header [ 0 ].OptionList = sourceOptionList;

      groupField.Table.Header [ 1 ].No = 2;
      groupField.Table.Header [ 1 ].Text = EdLabels.ReportTemplate_Query_Column_2_Text;
      groupField.Table.Header [ 1 ].TypeId = EvDataTypes.Selection_List;
      groupField.Table.Header [ 1 ].OptionList = mandatoryOptionList;

      groupField.Table.Header [ 2 ].No = 3;
      groupField.Table.Header [ 2 ].Text = EdLabels.ReportTemplate_Query_Column_3_Text;
      groupField.Table.Header [ 2 ].TypeId = EvDataTypes.Selection_List;
      groupField.Table.Header [ 2 ].OptionList = operationOptionList;

      //
      // Iterate through the queries creating a row for each query.
      //
      for ( int i = 0; i < this.Session.ReportTemplate.Queries.Length; i++ )
      {
        EvReportQuery query = this.Session.ReportTemplate.Queries [ i ];
        if ( query == null )
        {
          this.LogDebug ( "query null" );
          query = new EvReportQuery ( );
        }
        this.LogDebug ( "query.QueryId: " + query.QueryId
         + ", Mandatory: " + query.Mandatory );

        Evado.Model.UniForm.TableRow row = groupField.Table.addRow ( );
        row.No = 1;
        row.Column [ 0 ] = query.QueryId;
        row.Column [ 1 ] = query.Mandatory.ToString ( );
        row.Column [ 2 ] = query.Operator.ToString ( );

      }//END query interation loop

    }//END getReport_Display_Group method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private void getDataObject_Column_Selection_Group (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getDataObject_Column_Selection_Group" );
      this.LogDebug ( "ReportTemplate.Columns: " + this.Session.ReportTemplate.Columns.Count );
      this.LogDebug ( "ReportSource.Columns.Count: " + this.Session.ReportSource.ColumnList.Count );
      //
      // Initialise method variables and objects.
      //
      Evado.Model.UniForm.Field groupField = new Model.UniForm.Field ( );
      Evado.Model.UniForm.Command groupCommand = new Model.UniForm.Command ( );
      List<EvOption> columnOptionList = new List<EvOption> ( );
      List<EvOption> groupingTypeOptionList = new List<EvOption> ( );
      List<EvOption> sectionLevelOptionList = new List<EvOption> ( );

      //
      // If the source column list is empty exit.
      //
      if ( this.Session.ReportSource.ColumnList.Count == 0 )
      {
        this.LogDebug ( "The report source column list is empty so do not display the group." );
        return;
      }

      //
      // get the column selection list.
      //
      columnOptionList = this.Session.ReportSource.ColumnOptionList ( false );

      string currentSelection = String.Empty;
      foreach ( EvReportColumn column in this.Session.ReportTemplate.Columns )
      {
        if ( currentSelection != String.Empty )
        {
          currentSelection += ";";
        }
        currentSelection += column.ColumnId;
      }

      this.LogDebug ( "currentSelection: " + currentSelection );

      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
        EdLabels.ReportTemplate_Column_Selection_Group_Title,
        Evado.Model.UniForm.EditAccess.Enabled );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      groupField = pageGroup.createCheckBoxListField (
        EuReportTemplates.CONST_COLUMN_SELECTION_FIELD_ID,
        EdLabels.ReportTemplate_Column_Selection_Field_Title,
        currentSelection,
        columnOptionList );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // A refresh command to update the report source Id.
      //
      groupCommand = pageGroup.addCommand (
        EdLabels.ReportTemplate_Page_Refresh_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.ReportTemplates.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Custom_Method );

      groupCommand.SetGuid ( this.Session.ReportTemplate.Guid );
      groupCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.Get_Object );
      groupCommand.SetPageId ( EvPageIds.Report_Template_Form );

    }//END getDataObject_Column_Selection_Group method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private void getDataObject_Column_Structure_Group (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getDataObject_Column_Structure_Group" );
      this.LogDebug ( "ReportTemplate.Columns: " + this.Session.ReportTemplate.Columns.Count );
      this.LogDebug ( "ReportSource.Columns.Count: " + this.Session.ReportSource.ColumnList.Count );
      //
      // Initialise method variables and objects.
      //
      Evado.Model.UniForm.Field groupField = new Model.UniForm.Field ( );
      Evado.Model.UniForm.Command pageCommand = new Model.UniForm.Command ( );
      List<EvOption> columnOptionList = new List<EvOption> ( );
      List<EvOption> groupHeaderOptionList = new List<EvOption> ( );
      List<EvOption> groupingTypeOptionList = new List<EvOption> ( );
      List<EvOption> sectionLevelOptionList = new List<EvOption> ( );
      const int intNumerFlatColumns = 2;
      const int intNumerGroupColumns = 5;

      //
      // If the source column list is empty exit.
      //
      if ( this.Session.ReportSource.ColumnList.Count == 0 )
      {
        this.LogDebug ( "The report source column list is empty so do not display the group." );
        return;
      }

      //
      // Define the column structure group
      //
      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
        EdLabels.ReportTemplate_Column_Structure_Group_Title,
        Evado.Model.UniForm.EditAccess.Enabled );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      groupField = pageGroup.createTableField (
        EuReportTemplates.CONST_COLUMN_TABLE_FIELD_ID,
        EdLabels.ReportTemplate_Column_Structure_Field_Title,
       intNumerFlatColumns );

      groupField.Description = 
        EdLabels.ReportTemplate_Column_Structure_Description ;

      // 
      // If group report then add output columns
      //
      if ( this.Session.ReportTemplate.LayoutTypeId == EvReport.LayoutTypeCode.GroupedTable )
      {
        groupField.Table.Header = new Model.UniForm.TableColHeader [ intNumerGroupColumns ];
      }

      columnOptionList = this.Session.ReportSource.ColumnOptionList ( true );

      groupField.Table.Header [ 0 ] = new Model.UniForm.TableColHeader ( );
      groupField.Table.Header [ 0 ].No = 1;
      groupField.Table.Header [ 0 ].Text = EdLabels.ReportTemplate_Column_1_Text;
      groupField.Table.Header [ 0 ].TypeId = EvDataTypes.Read_Only_Text;
      groupField.Table.Header [ 0 ].Width = "40";

      groupField.Table.Header [ 1 ] = new Model.UniForm.TableColHeader ( );
      groupField.Table.Header [ 1 ].No = 2;
      groupField.Table.Header [ 1 ].Text = EdLabels.ReportTemplate_Column_2_Text;
      groupField.Table.Header [ 1 ].TypeId =  EvDataTypes.Numeric;
      groupField.Table.Header [ 1 ].Width = "10";

      //
      // Add the group repor design columns
      //
      if ( this.Session.ReportTemplate.LayoutTypeId == EvReport.LayoutTypeCode.GroupedTable )
      {
        //
        // create the query option list 
        //
        groupHeaderOptionList.Add ( new EvOption ( "True", "Yes" ) );
        groupHeaderOptionList.Add ( new EvOption ( "False", "No" ) );

        groupingTypeOptionList = Evado.Model.EvStatics.Enumerations.getOptionsFromEnum ( typeof ( EvReport.GroupingTypes ), true );

        sectionLevelOptionList.Add ( new EvOption ( "0", "Detail Level" ) );
        for ( int i = 1; i <= 5; i++ )
        {
          sectionLevelOptionList.Add ( new EvOption ( i.ToString ( ), "Level " + i ) );
        }

        this.LogDebug ( "groupHeaderOptionList.Count: " + groupHeaderOptionList.Count );
        this.LogDebug ( "columnOptionList.Count: " + columnOptionList.Count );
        this.LogDebug ( "sectionLevelOptionList.Count: " + sectionLevelOptionList.Count );

        groupField.Table.Header [ 2 ] = new Model.UniForm.TableColHeader ( );
        groupField.Table.Header [ 2 ].No = 3;
        groupField.Table.Header [ 2 ].Text = EdLabels.ReportTemplate_Column_3_Text;
        groupField.Table.Header [ 2 ].TypeId = EvDataTypes.Selection_List;
        groupField.Table.Header [ 2 ].OptionList = groupHeaderOptionList;
        groupField.Table.Header [ 2 ].Width = "15";

        groupField.Table.Header [ 3 ] = new Model.UniForm.TableColHeader ( );
        groupField.Table.Header [ 3 ].No = 4;
        groupField.Table.Header [ 3 ].Text = EdLabels.ReportTemplate_Column_4_Text;
        groupField.Table.Header [ 3 ].TypeId = EvDataTypes.Selection_List;
        groupField.Table.Header [ 3 ].OptionList = groupingTypeOptionList;
        groupField.Table.Header [ 3 ].Width = "15";

        groupField.Table.Header [ 4 ] = new Model.UniForm.TableColHeader ( );
        groupField.Table.Header [ 4 ].No = 5;
        groupField.Table.Header [ 4 ].Text = EdLabels.ReportTemplate_Column_5_Text;
        groupField.Table.Header [ 4 ].TypeId = EvDataTypes.Selection_List;
        groupField.Table.Header [ 4 ].OptionList = sectionLevelOptionList;
        groupField.Table.Header [ 4 ].Width = "10";

      }//END defineing the group table columns

      //
      // Iterate through the queries creating a row for each query.
      //
      this.LogDebug ( "ReportTemplate.Columns.Count: " + this.Session.ReportTemplate.Columns.Count );
      for ( int i = 0; i < this.Session.ReportTemplate.Columns.Count; i++ )
      {
        EvReportColumn column = this.Session.ReportTemplate.Columns [ i ];
        if ( column == null )
        {
          continue;
        }
        if ( column.ColumnId == String.Empty )
        {
          continue;
        }

        this.LogDebug ( "column.ColumnId: " + column.ColumnId + ",  HeaderText: " + column.HeaderText );

        Evado.Model.UniForm.TableRow row = groupField.Table.addRow ( );
        row.No = 1;
        row.Column [ 0 ] = column.HeaderText;
        row.Column [ 1 ] = column.SourceOrder.ToString ( );

        this.LogDebug ( "column.SourceOrder: " + column.SourceOrder );

        if ( this.Session.ReportTemplate.LayoutTypeId == EvReport.LayoutTypeCode.GroupedTable )
        {
          this.LogDebug ( "column.GroupingIndex: " + column.GroupingIndex );
          this.LogDebug ( "column.GroupingType: " + column.GroupingType );
          this.LogDebug ( "column.SectionLvl: " + column.SectionLvl );

          if ( column.SectionLvl < 0 || column.SectionLvl > 5 )
          {
            column.SectionLvl = 0;
          }
          row.Column [ 2 ] = column.GroupingIndex.ToString ( );
          row.Column [ 3 ] = column.GroupingType.ToString ( );
          row.Column [ 4 ] = column.SectionLvl.ToString ( );

        }//END group report.

      }//END query interation loop

    }//END getDataObject_Column_Selection_Group method

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class create object methods

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="Command">Evado.UniForm.Model.ClientClientDataObjectCommand object.</param>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData createObject (
      Evado.Model.UniForm.Command Command )
    {
      this.LogMethod ( "createObject" );
      try
      {
        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          "Evado.UniForm.Clinical.EuReportTemplates.createObject",
          this.Session.UserProfile );

        // 
        // Initialise the methods variables and objects.
        //      
        Evado.Model.UniForm.AppData data = new Evado.Model.UniForm.AppData ( );

        this.Session.ReportSourceList = this._Bll_ReportTemplates.getSourceList ( );

        this.Session.ReportTemplate = new EvReport ( );
        this.Session.ReportTemplate.Guid =  Evado.Model.Digital.EvcStatics.CONST_NEW_OBJECT_ID;
        this.Session.ReportTemplate.LayoutTypeId = EvReport.LayoutTypeCode.FlatTable;

        this.Session.PageId = EvPageIds.Report_Template_Form;

        this.getDataObject ( data );

        this.LogDebug ( "Exit createObject method. ID: " + data.Id + ", Title: " + data.Title );

        return data;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.Reports_Create_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

       return this.Session.LastPage;;

    }//END method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class update object methods

    // ==================================================================================
    /// <summary>
    /// This method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.ClientClientDataObjectCommand object.</param>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData updateObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "updateObject" );
      this.LogDebug ( "Parameter PageCommand: "
        + PageCommand.getAsString ( false, true ) );
      try
      {

        this.LogDebug ( "ReportTemplate: " );
        this.LogDebug ( " -Guid: " + this.Session.ReportTemplate.Guid );
        this.LogDebug ( " -ReportId: " + this.Session.ReportTemplate.ReportId );
        this.LogDebug ( " -ReportTitle: " + this.Session.ReportTemplate.ReportTitle );
        this.LogDebug ( " -ReportTemplate.SourceId: " + this.Session.ReportTemplate.SourceId );
        this.LogDebug ( " -Source.SourceId: " + this.Session.ReportSource.SourceId );

        //
        // Initialise the methods variables and objects.
        //
        this.Session.ReportDesignTemplateList = new List<EvReport> ( );
        EvEventCodes result = EvEventCodes.Ok;

        //
        // If new is true then this is a new activity so set the Guid to empty
        // so the business layer creates new activcity in the database.
        //
        if ( this.Session.ReportTemplate.Guid ==  Evado.Model.Digital.EvcStatics.CONST_NEW_OBJECT_ID )
        {
          this.Session.ReportTemplate.Guid = Guid.Empty;
        }

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          "Evado.UniForm.Clinical.EuReportTemplates.updateObject",
          this.Session.UserProfile );

        // 
        // Update the object.
        // 
        this.updateObjectValue ( PageCommand );

        //
        // Update the form object values.
        //
        this.updateQueryValues ( PageCommand ); ;

        //
        // Update the form object values.
        //
        this.updateColumnValues ( PageCommand );

        //
        // Validate that the report Id is not a duplicate.
        //
        if ( this.duplicateReportValidation ( ) == true )
        {
          this.LogDebug ( "Duplicate Report ID" );

          this.ErrorMessage = String.Format (
            EdLabels.ReportTemplate_Duplicate_Report_Error_Message,
            this.Session.ReportTemplate.ReportId );

          return this.Session.LastPage;
        }

        //
        // set the save parameters.
        //
        this.Session.ReportTemplate.UpdateUserId = this.Session.UserProfile.UserId;
        this.Session.ReportTemplate.UserCommonName = this.Session.UserProfile.CommonName;

        //
        // delete the report template by settting the title to string.empty.
        //
        if ( PageCommand.Method == Model.UniForm.ApplicationMethods.Delete_Object )
        {
          this.Session.ReportTemplate.ReportTitle = String.Empty;
        }

        //
        // Reset the lists if it is a new template.
        //
        if ( this.Session.ReportTemplate.Guid == Evado.Model.EvStatics.CONST_NEW_OBJECT_ID )
        {
          this.Session.ReportTemplateList_All = new List<EvOption> ( );
          this.Session.ReportDesignTemplateList = new List<EvReport> ( );
        }

        // 
        // update the object.
        // 
        result = this._Bll_ReportTemplates.saveReport ( this.Session.ReportTemplate );

        // 
        // get the debug ResultData.
        // 
        this.LogDebugClass ( this._Bll_ReportTemplates.Log );

        // 
        // if an error state is returned create log the event.
        // 
        if ( result != EvEventCodes.Ok )
        {
          string StEvent = this._Bll_ReportTemplates.Log + " returned error message: " + Evado.Model.Digital.EvcStatics.getEventMessage ( result );
          this.LogError ( EvEventCodes.Database_Record_Update_Error, StEvent );

          //
          // generate error messages for the result event codes
          switch ( result )
          {
            case EvEventCodes.Data_Duplicate_Id_Error:
              {
                this.ErrorMessage =
                  String.Format (
                    EdLabels.Report_Duplicate_Identifier_Error_Message,
                    this.Session.ReportTemplate.ReportId );
                break;
              }
            case EvEventCodes.Identifier_Project_Id_Error:
              {
                this.ErrorMessage = EdLabels.Project_Identifier_Empty_Error_Message;
                break;
              }
            case EvEventCodes.Identifier_Schedule_Identifier_Error:
              {
                this.ErrorMessage = EdLabels.Report_Identifier_Empty_Error_Message;
                break;
              }
            default:
              {
                this.ErrorMessage = EdLabels.Report_Template_Update_Error_Message;
                break;
              }
          }
          return this.Session.LastPage;
        }

        this.LogMethodEnd ( "updateObject" );
        return new Model.UniForm.AppData();
      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.Report_Template_Update_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }
      return this.Session.LastPage;;

    }//END updateObject method

    // ==================================================================================
    /// <summary>
    /// THis method updates the EvActivity object member with groupCommand parameter values.
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.PageCommand object.</param>
    //  ----------------------------------------------------------------------------------
    private void updateObjectValue (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "updateObjectValue" );
      this.LogDebug ( "Parameters.Count: " + PageCommand.Parameters.Count );

      // 
      // Iterate through the parameter values updating the ResultData object
      // 
      foreach ( Evado.Model.UniForm.Parameter parameter in PageCommand.Parameters )
      {
        if ( parameter.Name.Contains (  Evado.Model.Digital.EvcStatics.CONST_GUID_IDENTIFIER ) == true
          || parameter.Name.Contains ( Evado.Model.UniForm.CommandParameters.Page_Id.ToString ( ) ) == true
          || parameter.Name.Contains ( Evado.Model.UniForm.CommandParameters.Custom_Method.ToString ( ) ) == true
          || parameter.Name.Contains ( Evado.Model.UniForm.CommandParameters.Short_Title.ToString ( ) ) == true
          || parameter.Name.Contains ( EuReportTemplates.CONST_QUERY_TABLE_FIELD_ID ) == true
          || parameter.Name.Contains ( EuReportTemplates.CONST_COLUMN_TABLE_FIELD_ID ) == true
          || parameter.Name.Contains ( EuReportTemplates.CONST_COLUMN_SELECTION_FIELD_ID ) == true )
        {
          continue;
        }

        this.LogDebug ( parameter.Name + " > " + parameter.Value + " >> UPDATED" );
        try
        {
          EvReport.ReportClassFieldNames fieldName =
             Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue<EvReport.ReportClassFieldNames> ( parameter.Name );

          this.Session.ReportTemplate.setValue ( fieldName, parameter.Value );
        }
        catch ( Exception Ex )
        {
          this.LogException ( Ex );

          this.LogMethodEnd ( "updateObjectValue" );
        }

      }// End iteration loop

      this.LogMethodEnd ( "updateObjectValue" );

    }//END method.


    // ==================================================================================
    /// <summary>
    /// This method updates the Activity objects with groupCommand parameter values.
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.PageCommand object.</param>
    //  ----------------------------------------------------------------------------------
    private void updateQueryValues (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "updateQueryValues" );
      this.LogDebug ( "Parameters.Count: " + PageCommand.Parameters.Count );
      this.LogDebug ( "Queries.Length: " + this.Session.ReportTemplate.Queries.Length );
      //
      // Iterate through the activity's form list updating the values.
      //
      for ( int iCount = 0; iCount < this.Session.ReportTemplate.Queries.Length; iCount++ )
      {
        this.Session.ReportTemplate.Queries [ iCount ] = new EvReportQuery ( );

        //
        // get the parameter name.
        //
        String stParameterName = EuReportTemplates.CONST_QUERY_TABLE_FIELD_ID + "_" + ( iCount + 1 + "_1" );

        //
        // get the query identifier.
        //
        string tableValue = PageCommand.GetParameter ( stParameterName );
        this.LogDebug ( "Count: " + iCount + " tableValue: " + tableValue );

        //
        // if the queryID is not empty update it.
        //
        if ( tableValue != String.Empty )
        {
          this.LogDebug ( "Value Exists" );

          EvReportQuery sourceQuery = this.getQuery ( tableValue );

          this.LogDebug ( "sourceQuery: " + sourceQuery.QueryId
            + ", " + sourceQuery.QueryId
            + ", " + sourceQuery.QueryTitle
            + ", " + sourceQuery.Prompt
            + ", " + sourceQuery.SelectionSource
            + ", " + sourceQuery.DataType );

          sourceQuery.Index = iCount;
          sourceQuery.Operator = EvReport.Operators.Equals_to;
          this.Session.ReportTemplate.Queries [ iCount ] = sourceQuery;
          this.LogDebug ( "Queries.Prompt : " + this.Session.ReportTemplate.Queries [ iCount ].Prompt );

          //
          // get the parameter name.
          //
          stParameterName = EuReportTemplates.CONST_COLUMN_TABLE_FIELD_ID + ( iCount + 1 + "_2" );

          // 
          // get the table identifier.
          //
          tableValue = PageCommand.GetParameter ( stParameterName );
          this.LogDebug ( "Mandatory tableValue: " + tableValue );

          //
          // if the queryID is not empty update it.
          //
          if ( tableValue != String.Empty )
          {
            this.Session.ReportTemplate.Queries [ iCount ].Mandatory =
              Evado.Model.EvStatics.getBool ( tableValue );
          }

          //
          // get the parameter name.
          //
          stParameterName = EuReportTemplates.CONST_COLUMN_TABLE_FIELD_ID + ( iCount + 1 + "_3" );

          this.LogDebug ( "Operator parameter name: " + stParameterName );
          // 
          // get the table identifier.
          //
          tableValue = PageCommand.GetParameter ( stParameterName );
          this.LogDebug ( "Operator tableValue: " + tableValue );

          //
          // if the queryID is not empty update it.
          //
          if ( tableValue != String.Empty )
          {
            this.Session.ReportTemplate.Queries [ iCount ].Operator =
              Evado.Model.EvStatics.Enumerations.parseEnumValue<EvReport.Operators> ( tableValue );
          }


          this.LogDebug ( "Query " + iCount
            + ", " + this.Session.ReportTemplate.Queries [ iCount ].QueryId
            + ", " + this.Session.ReportTemplate.Queries [ iCount ].QueryTitle
            + ", " + this.Session.ReportTemplate.Queries [ iCount ].Prompt
            + ", " + this.Session.ReportTemplate.Queries [ iCount ].SelectionSource
            + ", " + this.Session.ReportTemplate.Queries [ iCount ].DataType
            + ", " + this.Session.ReportTemplate.Queries [ iCount ].FieldName
            + ", " + this.Session.ReportTemplate.Queries [ iCount ].Mandatory
            + ", " + this.Session.ReportTemplate.Queries [ iCount ].Operator );

        }//END PageCommand Value parameter exists.

      }//END query list interation loop.

      this.LogMethodEnd ( "updateQueryValues" );

    }//END updateQueryValues method

    //==================================================================================
    /// <summary>
    /// This method retrieves the selected query.
    /// </summary>
    /// <param name="QueryId">String: the queries identifier.</param>
    /// <returns>EvReportQuery objects.</returns>
    //-----------------------------------------------------------------------------------
    public EvReportQuery getQuery (
      String QueryId )
    {
      this.LogDebug ( "QueryId " + QueryId );
      //
      // search the list for the matching query.
      //
      foreach ( EvReportQuery query in this.Session.ReportSource.QueryList )
      {
        if ( query.QueryId == QueryId )
        {
          this.LogDebug ( "Query " + query.QueryId + " SELECTED" );
          return query;
        }
        this.LogDebug ( "Query " + query.QueryId + " NOT SELECTED" );
      }

      //
      // Returm empty objects.
      //
      return new EvReportQuery ( );

    }//END getQuery method

    // ==================================================================================
    /// <summary>
    /// This method updates the Activity objects with groupCommand parameter values.
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.PageCommand object.</param>
    //  ----------------------------------------------------------------------------------
    private void updateColumnValues (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "updateFormObjectValue" );
      this.LogDebug ( "Parameters.Count: " + PageCommand.Parameters.Count );
      this.LogDebug ( "Columns.Count: " + this.Session.ReportTemplate.Columns.Count );
      //
      // Initialise the methods variables and objects.
      //

      //
      //  Skip this method if the field does not exist.
      //
      if ( PageCommand.hasParameter ( EuReportTemplates.CONST_COLUMN_TABLE_FIELD_ID + "_1_2" ) == false )
      {
        return;
      }

      //
      // Iterate through the activity's form list updating the values.
      //
      for ( int iCount = 0; iCount < this.Session.ReportTemplate.Columns.Count; iCount++ )
      {
        EvReportColumn columnObject = this.Session.ReportTemplate.Columns [ iCount ];

        this.LogDebug ( "OBJECT: ColumnId: " + columnObject.ColumnId
          + ", HeaderText: " + columnObject.HeaderText
          + ", DataType: " + columnObject.DataType
          + ", SourceOrder: " + columnObject.SourceOrder );

        String stParameterName = EuReportTemplates.CONST_COLUMN_TABLE_FIELD_ID + "_" + ( iCount + 1 );

        this.LogDebug ( "stParameterName: " + stParameterName );
        String columnId = PageCommand.GetParameter ( stParameterName + "_1" );

        int sourceOrder = Evado.Model.EvStatics.getInteger ( PageCommand.GetParameter ( stParameterName + "_2" ) );
        bool groupingIndex = Evado.Model.EvStatics.getBool ( PageCommand.GetParameter ( stParameterName + "_3" ) );
        String groupingType = PageCommand.GetParameter ( stParameterName + "_4" );
        int sectionLevel = Evado.Model.EvStatics.getInteger ( PageCommand.GetParameter ( stParameterName + "_5" ) );

        this.LogDebug ( "PARAMETERS: sourceOrder: " + sourceOrder
          + ", groupingIndex: " + groupingIndex
          + ", groupingType: " + groupingType
          + ", sectionLevel: " + sectionLevel );

        // 
        // Set select the checklist if mandatory selected.
        // 
        columnObject.SourceOrder = sourceOrder;
        columnObject.GroupingIndex = groupingIndex;
        if ( groupingType != String.Empty )
        {
          columnObject.GroupingType = Evado.Model.EvStatics.Enumerations.parseEnumValue<EvReport.GroupingTypes> ( groupingType );
        }
        columnObject.SectionLvl = sectionLevel;

        this.LogDebug ( "UPDATED OBJECT: ColumnId: " + columnObject.ColumnId
          + ", HeaderText: " + columnObject.HeaderText
          + ", DataType: " + columnObject.DataType
          + ", SourceOrder: " + columnObject.SourceOrder
          + ", GroupingIndex: " + columnObject.GroupingIndex
          + ", GroupingType: " + columnObject.GroupingType
          + ", SectionLvl: " + columnObject.SectionLvl );

      }//END form list interation loop.

      //
      // Trim empty columns
      //
      this.Session.ReportTemplate.trimColumns ( );

      this.LogDebug ( "FINISHED: updateFormObjectValue" );
    }//END updateFormObjectValue method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
  }
}//END namespace