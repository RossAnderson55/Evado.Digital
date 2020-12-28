/***************************************************************************************
 * <copyright file=Evado.UniForm.eClinical\SubjectRescords.cs" company="EVADO HOLDING PTY. LTD.">
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
using System.IO;
using System.Configuration;

using Evado.Bll;
using Evado.Model;
using Evado.Bll.Clinical;
using Evado.Model.Clinical;
using Evado.Model.UniForm;
// using Evado.Web;

namespace Evado.UniForm.Clinical
{
  /// <summary>
  /// This class defines the application base classs that is used to terminate the 
  /// hosted application objects.
  /// 
  /// This class terminates the Customer object.
  /// </summary>
  public class EuBudgets : EuClassAdapterBase
  {
    #region Class Initialisation
    /// <summary>
    /// This method initialises the class.
    /// </summary>
    public EuBudgets ( )
    {
      this.ClassNameSpace = "Evado.UniForm.Clinical.EuBudgets.";
    }

    /// <summary>
    /// This method initialises the class and passs in the user profile.
    /// </summary>
    public EuBudgets (
      EuApplicationObjects ApplicationObjects,
      EvUserProfileBase ServiceUserProfile,
      EuSession SessionObjects,
      String UniFormBinaryFilePath,
      String UniFormServiceBinaryUrl,
      EvApplicationSetting Settings )
    {
      this.ApplicationObjects = ApplicationObjects;
      this.ServiceUserProfile = ServiceUserProfile;
      this.Session = SessionObjects;
      this.UniForm_BinaryFilePath = UniFormBinaryFilePath;
      this.UniForm_BinaryServiceUrl = UniFormServiceBinaryUrl;
      this.Settings = Settings;
      this.LoggingLevel = Settings.LoggingLevel;
      this.ClassNameSpace = "Evado.UniForm.Clinical.EuBudgets.";

      this.LogInitMethod ( "initialisation method. " );
      this.LogInitValue ( "ServiceUserProfile.UserId: " + ServiceUserProfile.UserId );
      this.LogInitValue ( "SessionObjects.Project.ProjectId: " + this.Session.Project.ProjectId );
      this.LogInitValue ( "SessionObjects.UserProfile.Userid: " + this.Session.UserProfile.UserId );
      this.LogInitValue ( "SessionObjects.UserProfile.CommonName: " + this.Session.UserProfile.CommonName );
      this.LogInitValue ( "UniFormBinaryFilePath: " + this.UniForm_BinaryFilePath );
      this.LogInitValue ( "UniFormBinaryServiceUrl: " + this.UniForm_BinaryServiceUrl );

      this._Bll_Budgets = new Evado.Bll.Clinical.EvBudgets ( this.Settings );

      if ( this.Session.Budget == null )
      {
        this.Session.BudgetList = new List<EvBudget> ( );

        this.Session.Budget = new EvBudget ( );

        this.Session.BudgetItem = new EvBudgetItem ( );
      }

    }//END Method


    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class enumerations

    private enum SubjectRecordAccess
    {
      Record_Read_Only,

      Record_Author_Access,

      Record_Review_Access,

      Record_Monitor_Access,

      Record_Data_Manager_Access,

      Record_Approval_Access,
    }

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class constants and variables.

    private Evado.Bll.Clinical.EvBudgets _Bll_Budgets = new Evado.Bll.Clinical.EvBudgets ( );

    private const String CONST_BUDGET_ITEM_SITES_FIELD_ID = "BDGTIMS";

    public const string CONST_UPDLOAD_FIELD_ID = "BUIF";

    //public const string CONST_BUDGET_DRAFT_ICON = "{{ICON0}}";
    //public const string CONST_BUDGET_ISSUED_ICON = "{{ICON1}}";
    //public const string CONST_BUDGET_WITHDRAWN_ICON = "{{ICON2}}";


    public const string ICON_FORM_DRAFT = "icons/record-draft.png";
    public const string ICON_FORM_REVIEWED = "icons/form-reviewed.png";
    public const string ICON_FORM_ISSUED = "icons/form-issued.png";
    public const string ICON_FORM_WITHDAWN = "icons/form-withdrawn.png";

    EvReport _BudgetReportTemplate = new EvReport ( );

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class public methods

    ///  ===============================================================================
    /// <summary>
    /// This method gets the application object from the list.
    /// 
    /// </summary>
    /// <param name="PageCommand">ClientPateEvado.Model.UniForm.Command object</param>
    /// <returns>ClientApplicationData</returns>
    //  ----------------------------------------------------------------------------------
    override public Evado.Model.UniForm.AppData getDataObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getDataObject method. " );
      this.LogDebugValue ( "PageCommand " + PageCommand.getAsString ( false, true ) );
      Evado.Bll.EvStaticSetting.LoggingLevel = this.LoggingLevel;
      this.LogValue ( "EvStaticSetting.LoggingLevel: " + Evado.Bll.EvStaticSetting.LoggingLevel );

      try
      {
        // 
        // Initialise the methods variables and objects.
        // 
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );

        if ( this.Session.BudgetList == null )
        {
          this.Session.BudgetList = new List<EvBudget> ( );
        }
        if ( this.Session.Budget == null )
        {
          this.Session.Budget = new EvBudget ( );
        }
        if ( this.Session.BudgetItem == null )
        {
          this.Session.BudgetItem = new EvBudgetItem ( );
        }

        //
        // if the current report is not a budget report reset it.
        //
        this.LogDebugValue ( "Report.ReportScope " + this.Session.Report.ReportScope );
        if ( this.Session.Report.ReportScope != EvReport.ReportScopeTypes.Budget )
        {
          this.Session.Report = new EvReport ( );
        }

        //
        // Determine if the user has access to this page and log and error if they do not.
        //
        if ( this.Session.UserProfile.hasRecordAccess == false )
        {
          this.LogIllegalAccess (
           this.ClassNameSpace + "EuBudgets module",
            this.Session.UserProfile );

          this.ErrorMessage = EvLabels.Illegal_Page_Access_Attempt;

          return null;
        }

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "EuBudgets module",
          this.Session.UserProfile );
        //
        // Set the page command page id.
        //
        this.Session.setPageId ( PageCommand.GetPageId ( ) );

        this.LogDebugValue ( "PageId: " + this.Session.PageId );

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
              switch ( this.Session.PageId )
              {
                case EvPageIds.Budget_Item_Page:
                  {
                    clientDataObject = this.getBudgetItemObject ( PageCommand );
                    break;
                  }
                case EvPageIds.Budget_Report_Page:
                  {
                    clientDataObject = this.getBudgetReportPage ( PageCommand );
                    break;
                  }
                default:
                  {
                    clientDataObject = this.getBudgetPage ( PageCommand );
                    break;
                  }
              }
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
              switch ( this.Session.PageId )
              {
                case EvPageIds.Budget_Item_Page:
                  {
                    clientDataObject = this.updateBudgetItem_Object ( PageCommand );
                    break;
                  }
                default:
                  {
                    clientDataObject = this.updateBudget_Object ( PageCommand );
                    break;
                  }
              }//END switch statement

              break;
            }

        }//END Switch
        // 
        // Handle returned exceptions.
        // 
        if ( clientDataObject == null )
        {
          this.LogDebugValue ( " null application data returned." );
          clientDataObject = this.Session.LastPage;
        }

        //
        // If an errot message exist display it.
        //
        if ( this.ErrorMessage != String.Empty )
        {
          clientDataObject.Message = this.ErrorMessage;
        }

        // 
        // return the client ResultData object.
        // 
        this.LogMethodEnd ( "getDataObject" );
        return clientDataObject;

      }
      catch ( Exception Ex )
      {
        this.ErrorMessage = EvLabels.Budget_Page_Load_Error_Message;

        this.LogException ( Ex );
      }
      this.LogMethodEnd ( "getDataObject" );
      return new Evado.Model.UniForm.AppData ( );

    }//END getDataObject method

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
      try
      {
        this.LogMethod ( "getListObject method. " );
        this.LogValue ( "UserProfile.CommonName: " + this.Session.UserProfile.CommonName );

        // 
        // Initialise the methods variables and objects.
        //      
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
        Evado.Model.UniForm.Command groupCommand = new Model.UniForm.Command ( );
        Evado.Model.UniForm.Group pageGroup = new Model.UniForm.Group ( );

        clientDataObject.Title = EvLabels.Budget_List_Page_Title;
        clientDataObject.Page.Title = clientDataObject.Title;
        clientDataObject.Id = Guid.NewGuid ( );
        this.LogValue ( "data.Page.Title: " + clientDataObject.Page.Title );

        //
        // Ouput the export group if selected.
        //
        if ( this.Session.UserProfile.hasBudgetEditAccess == true )
        {
          this.getBudgetListPageCommands ( clientDataObject.Page );
        }

        if ( this.Session.PageId == EvPageIds.Budget_Export_Page )
        {
          this.getBudgetList_Export_Group ( clientDataObject.Page );
        }

        //
        // Output the improt group if selected.
        //
        if ( this.Session.PageId == EvPageIds.Budget_Import_Page )
        {
          this.getBudgetList_Import_Group ( PageCommand, clientDataObject.Page );
        }

        // 
        // Create the new pageMenuGroup.
        // 
        pageGroup = clientDataObject.Page.AddGroup (
        EvLabels.Budget_List_Page_Group_Title,
        Evado.Model.UniForm.EditAccess.Enabled );
        pageGroup.CmdLayout = Evado.Model.UniForm.GroupCommandListLayouts.Vertical_Orientation;
        pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

        //
        // Define the icon urls.
        //
        clientDataObject.Page.setImageUrl (
          Model.UniForm.PageImageUrls.Image0_Url,
          EuBudgets.ICON_FORM_DRAFT );

        clientDataObject.Page.setImageUrl (
          Model.UniForm.PageImageUrls.Image1_Url,
          EuBudgets.ICON_FORM_ISSUED );

        clientDataObject.Page.setImageUrl (
          Model.UniForm.PageImageUrls.Image2_Url,
          EuBudgets.ICON_FORM_WITHDAWN );

        // 
        // get the list of customers.
        // 
        this.Session.BudgetList = this._Bll_Budgets.getBudgetList (
          this.Session.Project.ProjectId );

        this.LogValue ( this._Bll_Budgets.Log );
        this.LogValue ( "list count: " + this.Session.BudgetList.Count );

        //
        // Create a budget command if the budget list is emptry.
        //
        if ( this.Session.UserProfile.hasBudgetEditAccess == true
          && this.Session.BudgetList.Count == 0 )
        {
          groupCommand = pageGroup.addCommand (
            EvLabels.Budget_Add_Record_Command_Title,
            EuAdapter.APPLICATION_ID,
            EuAdapterClasses.Budgets.ToString ( ),
            Model.UniForm.ApplicationMethods.Create_Object );

          groupCommand.SetPageId ( EvPageIds.Budget_Page );

          groupCommand.SetBackgroundColour (
            Model.UniForm.CommandParameters.BG_Default,
            Model.UniForm.Background_Colours.Purple );
        }

        // 
        // generate the page links.
        // 
        foreach ( EvBudget budget in this.Session.BudgetList )
        {
          this.LogValue ( "Budget version: " + budget.Version + ", Guid: " + budget.Guid );
          groupCommand = pageGroup.addCommand (
            budget.Description,
            EuAdapter.APPLICATION_ID,
            EuAdapterClasses.Budgets.ToString ( ),
            Evado.Model.UniForm.ApplicationMethods.Get_Object );

          groupCommand.SetGuid ( budget.Guid );

          groupCommand.SetPageId ( EvPageIds.Budget_Page );

          switch ( budget.State )
          {
            case Evado.Model.Clinical.EvBudget.BudgetState.Draft:
              {
                groupCommand.AddParameter (
                 Model.UniForm.CommandParameters.Image_Url,
                 EuBudgets.ICON_FORM_DRAFT );
                break;
              }
            case Evado.Model.Clinical.EvBudget.BudgetState.Issued:
              {
                groupCommand.AddParameter (
                 Model.UniForm.CommandParameters.Image_Url,
                 EuBudgets.ICON_FORM_ISSUED );
                break;
              }
            case Evado.Model.Clinical.EvBudget.BudgetState.Withdrawn:
              {

                groupCommand.AddParameter (
                 Model.UniForm.CommandParameters.Image_Url,
                 EuBudgets.ICON_FORM_ISSUED );
                break;
              }
            default:
              {
                groupCommand.Title = String.Empty;
                break;
              }
          }//END state switch.

          groupCommand.Title += String.Format (
            EvLabels.Budget_List_Command_Title,
             budget.Description,
             budget.Version,
             budget.StartDateAsString,
             budget.FinishDateAsString );

          groupCommand.AddParameter (
            Model.UniForm.CommandParameters.Short_Title,
             String.Format (
              EvLabels.Budget_List_Short_Title,
              budget.Version ) );

        }//END ancillary command iteration loop.

        if ( pageGroup.CommandList.Count == 0 )
        {
          pageGroup.Description =  EvLabels.Budget_List_Page_No_Records_Label ;
        }


        this.LogValue ( "command object count: " + pageGroup.CommandList.Count );

        return clientDataObject;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Budget_Page_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return null;

    }//END getListObject method.

    // ==============================================================================
    /// <summary>
    /// This method add the page command to the list page.
    /// </summary>
    /// <param name="PageObject"> Evado.Model.UniForm.Page object</param>
    //  ------------------------------------------------------------------------------
    private void getBudgetListPageCommands (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getBudgetPage_Commands method. " );

      // 
      // initialise the save groupCommand.
      // 
      Evado.Model.UniForm.Command pageCommand = PageObject.addCommand (
        EvLabels.Budget_Export_Command_Title,
        EuAdapter.APPLICATION_ID,
        EuAdapterClasses.Budgets.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Custom_Method );

      // 
      // Define the pageCommand parameters.
      // 
      pageCommand.setCustomMethod ( ApplicationMethods.List_of_Objects );
      pageCommand.SetPageId ( EvPageIds.Budget_Export_Page );

      // 
      // initialise the save groupCommand.
      // 
      pageCommand = PageObject.addCommand (
        EvLabels.Budget_Import_Command_Title,
        EuAdapter.APPLICATION_ID,
        EuAdapterClasses.Budgets.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Custom_Method );

      // 
      // Define the pageCommand parameters.
      // 
      pageCommand.setCustomMethod ( ApplicationMethods.List_of_Objects );
      pageCommand.SetPageId ( EvPageIds.Budget_Import_Page );

      return;

    }//END getBudgetPage_Commands method

    //===================================================================================
    /// <summary>
    /// This method generates the relevant page page field for the milestone record page.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object</param>
    //-----------------------------------------------------------------------------------
    private void getBudgetList_Export_Group (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getBudgetList_Export_Group method. " );

      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.UniForm.Field pageField = new Evado.Model.UniForm.Field ( );
      Evado.Model.UniForm.Group pageGroup = new Model.UniForm.Group ( );
      Evado.Bll.Integration.EiCsvServices csvServices = new Bll.Integration.EiCsvServices ( );
      Evado.Model.Integration.EiData queryObject = new Model.Integration.EiData ( );
      String linkUrl = String.Empty;
      String filename = String.Empty;

      queryObject.QueryType = Model.Integration.EiQueryTypes.Budget_Export;
      queryObject.AddQueryParameter ( Model.Integration.EiQueryParameterNames.Project_Id, this.Session.Project.ProjectId );

      List<String> csvDataList = csvServices.ExportData ( queryObject );

      this.LogValue ( csvServices.Log );

      filename = String.Format (
        EvLabels.Budget_Export_filename,
        this.Session.Project.ProjectId,
        DateTime.Now.ToString ( "yyyy-MMM-dd-hh-mm" ) );

      System.Text.StringBuilder output = new StringBuilder ( );

      foreach ( string row in csvDataList )
      {
        output.AppendLine ( row );
      }

      EvStatics.Files.saveFile ( this.UniForm_BinaryFilePath, filename, output.ToString ( ) );

      linkUrl = this.UniForm_BinaryServiceUrl = filename;

      //
      // generate the pageMenuGroup object.
      //
      pageGroup = PageObject.AddGroup (
        EvLabels.Budget_Export_Group_Title );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      pageField = pageGroup.createHtmlLinkField (
        String.Empty,
        filename,
        linkUrl );

      pageGroup.Description = String.Format (
          EvLabels.Budget_Export_Description,
          this.Session.Project.ProjectId ) ;

      // 
      // Create the Project id object
      // 
      pageField = pageGroup.createReadOnlyTextField (
        String.Empty,
        EvLabels.Busget_Export_Process_Log,
        EvStatics.getStringAsHtml ( csvServices.ProcessLog ) );
      pageField.Layout = FieldLayoutCodes.Column_Layout;

      this.LogMethodEnd ( "getBudgetList_Export_Group" );

    }//END getBudgetList_Export_Group method.

    //===================================================================================
    /// <summary>
    /// This method generates the relevant page page field for the milestone record page.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object</param>
    //-----------------------------------------------------------------------------------
    private bool getBudgetList_Import_Group (
      Evado.Model.UniForm.Command PageCommand,
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getBudgetList_Import_Group method. " );
      bool result = true;
      //
      // if the form template filename is empty display the selection field.
      //
      if ( PageCommand.hasParameter ( EuBudgets.CONST_UPDLOAD_FIELD_ID ) == false )
      {
        this.LogValue ( "UploadFileName is empty" );

        this.getBudgetUploadSelectionGroup ( PageObject );
      }
      else
      {
        this.LogValue ( "Saving upload" );

        result = this.saveBudgetUpload ( PageCommand, PageObject );
      }

      this.LogMethodEnd ( "getList_Upload_Group" );

      return result;

    }//END getBudgetList_Export_Group method.

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.AppData object.</param>
    //  ------------------------------------------------------------------------------
    private void getBudgetUploadSelectionGroup (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getBudgetUploadSelectionGroup method. " );

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Command groupCommand = new Evado.Model.UniForm.Command ( );
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      Evado.Model.UniForm.Parameter parameter = new Evado.Model.UniForm.Parameter ( );

      //
      // Define the general properties pageMenuGroup..
      //
      pageGroup = PageObject.AddGroup (
        EvLabels.Budget_Upload_Group_Title,
        Evado.Model.UniForm.EditAccess.Enabled );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;

      pageGroup.SetFieldBackBroundColor (
        Model.UniForm.GroupParameterList.BG_Mandatory,
        Model.UniForm.Background_Colours.Red );

      groupField = pageGroup.createBinaryFileField (
        EuBudgets.CONST_UPDLOAD_FIELD_ID,
        EvLabels.Budget_Upload_Field_Title,
        String.Empty,
        this.Session.UploadFileName );
      groupField.Layout = EuFormGenerator.ApplicationFieldLayout;

      groupField.AddParameter ( Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, "Yes" );

      groupCommand = pageGroup.addCommand (
        EvLabels.Budget_Upload_Command_Title,
        EuAdapter.APPLICATION_ID,
        EuAdapterClasses.Budgets.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Custom_Method );

      groupCommand.SetPageId ( EvPageIds.Budget_Import_Page );
      groupCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.List_of_Objects );

    }//END getUserProfileUploadDataObject Method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.AppData object.</param>
    //  ------------------------------------------------------------------------------
    private bool saveBudgetUpload (
      Evado.Model.UniForm.Command PageCommand,
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "saveBudgetUpload method. " );
      // 
      // Initialise the client ResultData object.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Command groupCommand = new Evado.Model.UniForm.Command ( );
      Evado.Bll.Integration.EiCsvServices csvServices = new Bll.Integration.EiCsvServices ( );
      Evado.Model.Integration.EiData result = new Model.Integration.EiData ( );
      List<String> csvDataList = new List<string> ( );
      string fileName = PageCommand.GetParameter ( EuBudgets.CONST_UPDLOAD_FIELD_ID );
      this.LogValue ( "fileName: " + fileName );

      //
      // Read in date file
      //
      csvDataList = EvStatics.Files.readFileAsList ( this.UniForm_BinaryFilePath, fileName );

      this.LogValue ( "csvDataList.Count: " + csvDataList.Count );

      //
      // Define the general properties pageMenuGroup..
      //
      pageGroup = PageObject.AddGroup (
        EvLabels.Budget_Import_Process_Log_Group_Title,
        Evado.Model.UniForm.EditAccess.Inherited_Access );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;


      groupCommand = pageGroup.addCommand (
        EvLabels.Budget_Upload_Close_Process_Log_Command_Title,
        EuAdapter.APPLICATION_ID,
        EuAdapterClasses.Budgets.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Custom_Method );

      groupCommand.SetPageId ( EvPageIds.Budget_Version_View );
      groupCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.List_of_Objects );

      try
      {
        //
        //Upload the User profile.
        //
        result = csvServices.ImportData (
          this.Session.UserProfile,
          Model.Integration.EiQueryTypes.Budget_Import, csvDataList );
      }
      catch ( Exception Ex )
      {
        // 
        // On an exception raised create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Budget_IMport_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );

        return false;
      }

      this.LogValue ( "Csv Services: " + csvServices.Log );

      this.LogValue ( "processLog: " + result.ProcessLog );

      pageGroup.Description =  result.ProcessLog ;


      // 
      // Generate the log the action event.
      // 
      this.LogEvent ( result.ProcessLog );


      return true;

    }//END getUserProfileUploadDataObject Method

    //*******************************************************************************
    #endregion

    #region Get Budget page.
    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData getBudgetPage (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getObject method. " );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
      Guid budgetGuid = Guid.Empty;

      // 
      // if the parameter value exists then set the customerId
      // 
      budgetGuid = PageCommand.GetGuid ( );
      this.LogValue ( "RecordGuid: " + budgetGuid );

      // 
      // return if not trial id
      // 
      if ( budgetGuid == Guid.Empty )
      {
        this.LogValue ( "Record GUID is empty." );
        return null;
      }

      try
      {
        if ( this.Session.Budget.Guid != budgetGuid
          || this.Session.Budget.BudgetItems.Count == 0 )
        {
          this.LogValue ( "Budget Guids do not match so load a new instance." );
          // 
          // Retrieve the customer object from the database via the DAL and BLL layers.
          // 
          this.Session.Budget = this._Bll_Budgets.getBudget ( budgetGuid );

          this.LogValue ( this._Bll_Budgets.Log );

          if ( this.Session.Budget.StartDate == EvStatics.CONST_DATE_NULL )
          {
            this.Session.Budget.StartDate = this.Session.Project.RecuitmentStarted;
          }
        }
        else
        {
          this.LogValue ( "Budget in memory so update session object." );
          // 
          // Update the object.
          // 
          this.updateBudget_ObjectValue ( PageCommand );

        }//END retrieve new instance of budget.

        this.LogValue ( "RETURNED: Budget.BudgetItems.Count: " + this.Session.Budget.BudgetItems.Count );

        //
        // Set budget margin if not set.
        //
        if ( this.Session.Budget.ProjectMargin < 0 )
        {
          //this.Session.Budget.ProjectMargin = this.Session.Project.Data.DefaultMargin;
        }

        this.LogValue ( "Budgets.Version: "
          + this.Session.Budget.Version );

        // 
        // return the client ResultData object for the customer.
        // 
        this.getBudget_DataObject ( clientDataObject );

        this.LogMethodEnd ( "getObject" );

        return clientDataObject;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Budget_Page_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      this.LogMethodEnd ( "getBudgetPage" );
      return null;

    }//END getObject method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="ClientDataObject">Evado.Model.UniForm.AppData object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private void getBudget_DataObject (
      Evado.Model.UniForm.AppData ClientDataObject )
    {
      this.LogMethod ( "getBudget_DataObject method. " );
      // 
      // Initialise the methods variables and objects.
      // 
      ClientDataObject.Id = Guid.NewGuid ( );
      ClientDataObject.Title =
        String.Format ( EvLabels.Budget_Page_Page_Title,
        this.Session.Budget.Version,
        this.Session.Project.ProjectId,
        this.Session.Project.Title
        );

      ClientDataObject.Page.Id = ClientDataObject.Id;
      ClientDataObject.Page.Title = ClientDataObject.Title;
      ClientDataObject.Page.Status = Evado.Model.UniForm.EditAccess.Enabled;

      //
      // get the page commands.
      //
      this.getBudget_PageCommands ( ClientDataObject.Page );

      // 
      // create the page pageMenuGroup
      // 
      this.getBudget_General_Group ( ClientDataObject.Page );

      // 
      // create the budget item group
      // 
      this.getBudget_BudgetItem_Group ( ClientDataObject.Page );

      //
      // create the signoff group.
      //
      this.getBudget_SignoffLog_Group ( ClientDataObject.Page );

      this.LogMethodEnd ( "getBudget_DataObject" );

    }//END getClientDataObject Method

    //===================================================================================
    /// <summary>
    /// This method generates the relevant page commands for the milestone record page.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object</param>
    //-----------------------------------------------------------------------------------
    private void getBudget_PageCommands (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getObject_PageCommands method. " );
      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.UniForm.Command pageCommand = new Model.UniForm.Command ( );
      PageObject.Status = Evado.Model.UniForm.EditAccess.Enabled;
      this.LogValue ( "hasBudgetEditAccess: " + this.Session.UserProfile.hasBudgetEditAccess );

      //
      // Set the page access and commands based on the user role and the record state.
      //
      switch ( this.Session.Budget.State )
      {
        case EvBudget.BudgetState.Draft:
          {
            if ( this.Session.UserProfile.hasBudgetEditAccess == true )
            {
              PageObject.Status = Evado.Model.UniForm.EditAccess.Enabled;
              // 
              // Add the save budget command.
              // 
              pageCommand = PageObject.addCommand (
                EvLabels.Budget_Save_Command_Title,
                EuAdapter.APPLICATION_ID,
                EuAdapterClasses.Budgets.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.Save_Object );

              pageCommand.SetPageId ( EvPageIds.Budget_Page );
              pageCommand.SetGuid ( this.Session.Budget.Guid );
              pageCommand.AddParameter (
                Evado.Model.Clinical.EvcStatics.CONST_SAVE_ACTION,
                EvBudget.BudgetSaveActions.Save.ToString ( ) );

              // 
              // Add the delete budget command.
              // 
              pageCommand = PageObject.addCommand (
                EvLabels.Budget_Delete_Command_Title,
                EuAdapter.APPLICATION_ID,
                EuAdapterClasses.Budgets.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.Delete_Object );

              pageCommand.SetPageId ( EvPageIds.Budget_Page );
              pageCommand.SetGuid ( this.Session.Budget.Guid );
              pageCommand.AddParameter (
                Evado.Model.Clinical.EvcStatics.CONST_SAVE_ACTION,
                EvBudget.BudgetSaveActions.Save.ToString ( ) );

              // 
              // Add the approval budget command.
              // 
              pageCommand = PageObject.addCommand (
                EvLabels.Budget_Issue_Command_Title,
                EuAdapter.APPLICATION_ID,
                EuAdapterClasses.Budgets.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.Save_Object );

              pageCommand.SetPageId ( EvPageIds.Budget_Page );
              pageCommand.SetGuid ( this.Session.Budget.Guid );
              pageCommand.AddParameter (
                Evado.Model.Clinical.EvcStatics.CONST_SAVE_ACTION,
                EvBudget.BudgetSaveActions.Approve.ToString ( ) );
            }
            break;
          }
        case EvBudget.BudgetState.Issued:
          {
            if ( this.Session.UserProfile.hasBudgetEditAccess == true )
            {
              // 
              // Add the revise budget command.
              // 
              pageCommand = PageObject.addCommand (
                EvLabels.Budget_Revise_Command_Title,
                EuAdapter.APPLICATION_ID,
                EuAdapterClasses.Budgets.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.Save_Object );

              pageCommand.SetPageId ( EvPageIds.Budget_Page );
              pageCommand.SetGuid ( this.Session.Budget.Guid );
              pageCommand.AddParameter (
                Evado.Model.Clinical.EvcStatics.CONST_SAVE_ACTION,
                EvBudget.BudgetSaveActions.Revise.ToString ( ) );
            }
            break;
          }
        default:
          {
            PageObject.Status = Evado.Model.UniForm.EditAccess.Enabled;
            break;
          }
      }//END switch

      //
      // Add the budget report command
      //
      pageCommand = PageObject.addCommand (
        EvLabels.Budget_Repprt_Page_Command_Title,
        EuAdapter.APPLICATION_ID,
        EuAdapterClasses.Budgets.ToString ( ),
        Model.UniForm.ApplicationMethods.Get_Object );


      pageCommand.SetPageId ( EvPageIds.Budget_Report_Page );

      this.LogMethodEnd ( "getObject_PageCommands" );

    }//END getObject_PageCommands method

    //===================================================================================
    /// <summary>
    /// This method generates the relevant page page field for the milestone record page.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object</param>
    //-----------------------------------------------------------------------------------
    private void getBudget_General_Group (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getObject_General_Group method. " );
      this.LogValue ( "PageObject.Status: " + PageObject.Status );
      this.LogValue ( "Budget.StartDate: " + this.Session.Budget.StartDate );
      this.LogValue ( "Budget.FinishDate: " + this.Session.Budget.FinishDate );

      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.UniForm.Field pageField = new Evado.Model.UniForm.Field ( );
      Evado.Model.UniForm.Group pageGroup = new Model.UniForm.Group ( );

      //
      // generate the pageMenuGroup object.
      //
      pageGroup = PageObject.AddGroup (
        EvLabels.Budget_Page_General_Group_Title );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
      pageField.EditAccess = PageObject.Status;

      //
      // Add the groups commands.
      //
      this.getBudget_GroupCommands ( pageGroup );

      // 
      // Create the Project id object
      // 
      pageField = pageGroup.createReadOnlyTextField (
        EvBudget.BudgetFieldNames.Version.ToString ( ),
        EvLabels.Budget_Version_Field_Title,
        this.Session.Budget.Version.ToString ( "00" ) );
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;

      // 
      // Create the customer name object
      // 
      if ( pageGroup.EditAccess == Evado.Model.UniForm.EditAccess.Enabled )
      {
        pageField = pageGroup.createFreeTextField (
          EvBudget.BudgetFieldNames.Description.ToString ( ),
          EvLabels.Budget_Version_Description_Field_Title,
          this.Session.Budget.Description,
          50, 3 );
        pageField.Layout = EuFormGenerator.ApplicationFieldLayout;
      }
      else
      {
        pageField = pageGroup.createReadOnlyTextField (
          EvBudget.BudgetFieldNames.Description.ToString ( ),
          EvLabels.Budget_Version_Description_Field_Title,
          this.Session.Budget.Description );
        pageField.Layout = EuFormGenerator.ApplicationFieldLayout;
      }

      // 
      // Create the budget state object
      // 
      pageField = pageGroup.createReadOnlyTextField (
        String.Empty,
        EvLabels.Budget_Status_Field_Label,
        EvStatics.Enumerations.enumValueToString ( this.Session.Budget.State ) );
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;

      // 
      // Create the budget project margin
      // 
      if ( pageField.EditAccess == EditAccess.Enabled )
      {
        //this.Session.Budget.ProjectMargin = this.Session.Project.Data.DefaultMargin;
      }

      pageField = pageGroup.createReadOnlyTextField (
        String.Empty,
        EvLabels.Budget_Project_Margin_Field_Label,
         this.Session.Budget.ProjectMargin.ToString ( "###" ) + "%" );
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;
      pageField.Description =  EvLabels.Budget_Project_Margin_Description_Label ;

      // 
      // Create the budget start date
      // 
      if ( this.Session.Budget.Version < 1
        && this.Session.Budget.State != EvBudget.BudgetState.Issued )
      {
        if ( this.Session.Budget.StartDate == EvStatics.CONST_DATE_NULL )
        {
          this.Session.Budget.StartDate = DateTime.Now;
        }

        pageField = pageGroup.createDateField (
          EvBudget.BudgetFieldNames.Start_Date.ToString ( ),
          EvLabels.Budget_Start_Date_Field_Label,
          this.Session.Budget.StartDate );
        pageField.Layout = EuFormGenerator.ApplicationFieldLayout;
        pageField.AddParameter ( Model.UniForm.FieldParameterList.Min_Value, "1 jan 2000" );
        pageField.AddParameter ( Model.UniForm.FieldParameterList.Max_Value, "31 DEC 2099" );
      }
      else
      {
        pageField = pageGroup.createReadOnlyTextField (
          EvBudget.BudgetFieldNames.Start_Date.ToString ( ),
          EvLabels.Budget_Start_Date_Field_Label,
          this.Session.Budget.StartDateAsString );
        pageField.Layout = EuFormGenerator.ApplicationFieldLayout;
      }

      // 
      // Create the budget finish date
      // 
      if ( this.Session.Budget.State != EvBudget.BudgetState.Issued )
      {
        pageField = pageGroup.createDateField (
          EvBudget.BudgetFieldNames.Finish_Date.ToString ( ),
          EvLabels.Budget_Finish_Date_Field_Label,
          this.Session.Budget.FinishDate );
        pageField.Layout = EuFormGenerator.ApplicationFieldLayout;
        pageField.AddParameter ( Model.UniForm.FieldParameterList.Min_Value, "1 jan 2000" );
        pageField.AddParameter ( Model.UniForm.FieldParameterList.Max_Value, "31 DEC 2099" );
      }
      else
      {
        pageField = pageGroup.createReadOnlyTextField (
          EvBudget.BudgetFieldNames.Finish_Date.ToString ( ),
          EvLabels.Budget_Finish_Date_Field_Label,
          this.Session.Budget.FinishDateAsString );
        pageField.Layout = EuFormGenerator.ApplicationFieldLayout;
      }

      this.LogMethodEnd ( "getObject_General_Group" );

    }//END getObject_General_Group method.

    //===================================================================================
    /// <summary>
    /// This method generates the relevant page commands for the milestone record page.
    /// </summary>
    /// <param name="PageGroup">Evado.Model.UniForm.Page object</param>
    //-----------------------------------------------------------------------------------
    private void getBudget_GroupCommands (
      Evado.Model.UniForm.Group PageGroup )
    {
      this.LogMethod ( "getBudget_GroupCommands method. " );
      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.UniForm.Command groupCommand = new Model.UniForm.Command ( );
      PageGroup.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      //
      // Set the page access and commands based on the user role and the record state.
      //
      switch ( this.Session.Budget.State )
      {
        case EvBudget.BudgetState.Draft:
          {
            if ( this.Session.UserProfile.hasBudgetEditAccess == true )
            {
              PageGroup.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
              // 
              // Add the save budget command.
              // 
              groupCommand = PageGroup.addCommand (
                EvLabels.Budget_Save_Command_Title,
                EuAdapter.APPLICATION_ID,
                EuAdapterClasses.Budgets.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.Save_Object );

              groupCommand.SetPageId ( EvPageIds.Budget_Page );
              groupCommand.SetGuid ( this.Session.Budget.Guid );
              groupCommand.AddParameter (
                Evado.Model.Clinical.EvcStatics.CONST_SAVE_ACTION,
                EvBudget.BudgetSaveActions.Save.ToString ( ) );

              // 
              // Exit the method if a new budget.
              // 
              if ( this.Session.Budget.Guid != EvStatics.CONST_NEW_OBJECT_ID )
              {
                // 
                // Add the delete budget command.
                // 
                groupCommand = PageGroup.addCommand (
                  EvLabels.Budget_Delete_Command_Title,
                  EuAdapter.APPLICATION_ID,
                  EuAdapterClasses.Budgets.ToString ( ),
                  Evado.Model.UniForm.ApplicationMethods.Delete_Object );

                groupCommand.SetPageId ( EvPageIds.Budget_Page );
                groupCommand.SetGuid ( this.Session.Budget.Guid );
                groupCommand.AddParameter (
                  Evado.Model.Clinical.EvcStatics.CONST_SAVE_ACTION,
                  EvBudget.BudgetSaveActions.Save.ToString ( ) );

                // 
                // Add the approval budget command.
                // 
                groupCommand = PageGroup.addCommand (
                  EvLabels.Budget_Issue_Command_Title,
                  EuAdapter.APPLICATION_ID,
                  EuAdapterClasses.Budgets.ToString ( ),
                  Evado.Model.UniForm.ApplicationMethods.Save_Object );

                groupCommand.SetPageId ( EvPageIds.Budget_Page );
                groupCommand.SetGuid ( this.Session.Budget.Guid );
                groupCommand.AddParameter (
                  Evado.Model.Clinical.EvcStatics.CONST_SAVE_ACTION,
                  EvBudget.BudgetSaveActions.Approve.ToString ( ) );

              }
            }
            break;
          }
        case EvBudget.BudgetState.Issued:
          {
            if ( this.Session.UserProfile.hasBudgetEditAccess == true )
            {
              PageGroup.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

              // 
              // Add the approval budget command.
              // 
              groupCommand = PageGroup.addCommand (
                EvLabels.Budget_Revise_Command_Title,
                EuAdapter.APPLICATION_ID,
                EuAdapterClasses.Budgets.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.Save_Object );

              groupCommand.SetPageId ( EvPageIds.Budget_Page );
              groupCommand.SetGuid ( this.Session.Budget.Guid );
              groupCommand.AddParameter (
                Evado.Model.Clinical.EvcStatics.CONST_SAVE_ACTION,
                EvBudget.BudgetSaveActions.Revise.ToString ( ) );

            }
            break;
          }
        default:
          {
            PageGroup.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
            break;
          }
      }//END switch

      //
      // Add the budget report command
      //
      groupCommand = PageGroup.addCommand (
        EvLabels.Budget_Repprt_Page_Command_Title,
        EuAdapter.APPLICATION_ID,
        EuAdapterClasses.Budgets.ToString ( ),
        Model.UniForm.ApplicationMethods.Get_Object );


      groupCommand.SetPageId ( EvPageIds.Budget_Report_Page );

      this.LogMethodEnd ( "getBudget_GroupCommands" );

    }//END getObject_PageCommands method

    //===================================================================================
    /// <summary>
    /// This method generates the relevant inary files groups for the milestone record page.
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page object</param>
    //-----------------------------------------------------------------------------------
    private void getBudget_BudgetItem_Group (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getObject_BudgetItem_Group method. " );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Field pageField = new Evado.Model.UniForm.Field ( );
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );

      //
      // create the budget item group 
      //
      Evado.Model.UniForm.Group pageGroup = Page.AddGroup (
        EvLabels.Budget_Item_List_Group_Title,
        Evado.Model.UniForm.EditAccess.Enabled );

      pageGroup.CmdLayout = Model.UniForm.GroupCommandListLayouts.Vertical_Orientation;
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      //
      // Iterate through the list of budget items.
      //
      foreach ( EvBudgetItem item in this.Session.Budget.BudgetItems )
      {
        //
        // For issued budgets skip all undefined budget items.
        //
        if ( item.ActivityId == String.Empty )
        {
          this.LogValue ( "Activity Id is empty." );
          continue;
        }

        //
        // Add the budget item command object.
        //
        pageCommand = pageGroup.addCommand (
          String.Format (
            EvLabels.Budget_Item_List_Command_Title,
            item.ActivityId,
            item.Title ),
          EuAdapter.APPLICATION_ID,
          EuAdapterClasses.Budgets,
          Model.UniForm.ApplicationMethods.Get_Object );

        if ( item.Guid == Guid.Empty )
        {
          this.LogValue ( "Empty Budget Item" );
          item.Guid = EvStatics.CONST_NEW_OBJECT_ID;
          item.Title = String.Format (
            EvLabels.BudgetItem_New_Item_Title, item.Title );

          if ( item.DefaultMargin == -1 )
          {
            item.DefaultMargin = this.Session.Budget.ProjectMargin;

            if ( item.DefaultMargin == -1 )
            {
              item.DefaultMargin = 0;
            }
          }
        }

        pageCommand.SetPageId ( EvPageIds.Budget_Item_Page );
        pageCommand.SetGuid ( item.Guid );

      }//END budget item iteration loop

      this.LogMethodEnd ( "getObject_BudgetItem_Group" );

    }//END getObject_BudgetItem_Group method

    // ==============================================================================
    /// <summary>
    /// This method creates the project signoff pageMenuGroup
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page object object.</param>
    // ------------------------------------------------------------------------------
    private void getBudget_SignoffLog_Group (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getClientDataObject_SignoffLog method. " );

      // 
      // Display comments it they exist.
      // 
      if ( this.Session.Budget.Signoffs.Count == 0 )
      {
        this.LogValue ( EvLabels.Label_No_Signoff_Label );
        return;
      }

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Field pageField = new Evado.Model.UniForm.Field ( );

      //
      // create teh signoff group 
      Evado.Model.UniForm.Group pageGroup = Page.AddGroup (
        EvLabels.Label_Signoff_Log_Group_Title,
        Evado.Model.UniForm.EditAccess.Enabled );

      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;

      pageField = pageGroup.createReadOnlyTextField (
        String.Empty,
        EvLabels.Label_Signoff_Log_Field_Title,
        String.Empty,
        EvUserSignoff.getSignoffLog ( this.Session.Budget.Signoffs, false ) );
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;

    }//END getObject_SignoffLog_Group Method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Get Buddget Item Object.

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData getBudgetItemObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getBudgetItemObject method. " );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
      Guid budgetItemGuid = Guid.Empty;

      // 
      // if the parameter value exists then set the customerId
      // 
      budgetItemGuid = PageCommand.GetGuid ( );
      this.LogValue ( "budgetItemGuid: " + budgetItemGuid );

      // 
      // return if not trial id
      // 
      if ( budgetItemGuid == Guid.Empty )
      {
        this.LogValue ( "Record GUID is empty." );
        return null;
      }

      this.LogValue ( "budgetItemGuid exists" );

      try
      {
        // 
        // Retrieve the customer object from the database via the DAL and BLL layers.
        // 
        if ( this.getBudgetItemObject ( budgetItemGuid ) == false )
        {
          return null;
        }

        this.LogValue ( "BudgetItem.ActivityId: "
          + this.Session.BudgetItem.ActivityId );

        if ( this.Session.BudgetItem.DefaultMargin < -0 )
        {
          //this.Session.BudgetItem.DefaultMargin =
          //  this.Session.Project.Data.DefaultMargin;
        }

        //
        // Update the budget item values.
        //
        if ( PageCommand.hasParameter (
          Model.UniForm.CommandParameters.Custom_Method ) == true )
        {
          this.LogValue ( "Updating the budget items " );

          this.updateBudgetItem_ObjectValue ( PageCommand );

          this.updateBudgetItemSite_ObjectValue ( PageCommand );

          //
          // Validate the budget item values.
          //
          this.updateBudgetItem_Validation ( );
        }

        // 
        // return the client ResultData object for the customer.
        // 
        this.getBudgetItem_DataObject ( clientDataObject );

        this.LogMethodEnd ( "getBudgetItemObject" );

        return clientDataObject;
      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Budget_Page_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      this.LogMethodEnd ( "getBudgetItemObject" );
      return null;

    }//END getObject method

    //===================================================================================
    /// <summary>
    /// This method get the budget item matching the passed Guid identifier.
    /// </summary>
    /// <param name="BudgetItemGuid">Guid object</param>
    /// <returns>True: item found</returns>
    //-----------------------------------------------------------------------------------
    private bool getBudgetItemObject ( Guid BudgetItemGuid )
    {
      this.LogMethod ( "getBudgetItemObject method. " );
      //
      // if the budget item guid = the selected budget item guid.
      //
      if ( this.Session.BudgetItem.Guid == BudgetItemGuid )
      {
        this.LogValue ( "BudgetItem is in momory" );
        return true;
      }

      //
      // Iterate through teh list of budget items.
      //
      if ( this.Session.Budget.BudgetItems.Count == 0 )
      {
        this.Session.EventId = EvEventCodes.Identifier_Budget_Item_Id_Error;
        this.ErrorMessage = EvStatics.enumValueToString ( this.Session.EventId );

        return false;
      }

      //
      // Iterate through the budget to get the relevant budget item.
      //
      foreach ( EvBudgetItem item in this.Session.Budget.BudgetItems )
      {
        if ( item.Guid == BudgetItemGuid )
        {
          this.Session.BudgetItem = item;

          return true;
        }
      }
      return false;
    }

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="ClientDataObject">Evado.Model.UniForm.AppData object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private void getBudgetItem_DataObject (
      Evado.Model.UniForm.AppData ClientDataObject )
    {
      this.LogMethod ( "getBudgetItem_DataObject method. " );
      // 
      // Initialise the methods variables and objects.
      // 
      ClientDataObject.Id = this.Session.Budget.Guid;
      ClientDataObject.Title =
        String.Format ( EvLabels.Budget_Item_Page_Title,
        this.Session.Budget.Version.ToString ( "00" ),
        this.Session.BudgetItem.ActivityId,
        this.Session.BudgetItem.Title );

      ClientDataObject.Page.Id = ClientDataObject.Id;
      ClientDataObject.Page.Title = ClientDataObject.Title;
      ClientDataObject.Page.Status = Evado.Model.UniForm.EditAccess.Enabled;

      //
      // get the page commands.
      //
      this.getBudgetItem_PageCommands ( ClientDataObject.Page );

      // 
      // create the page pageMenuGroup
      // 
      this.getBudgetItem_General_Group ( ClientDataObject.Page );

      // 
      // create the budget item group
      // 
      this.getBudgetItem_Site_Group ( ClientDataObject.Page );

      this.LogMethodEnd ( "getBudgetItem_DataObject" );

    }//END getClientDataObject Method

    //===================================================================================
    /// <summary>
    /// This method generates the relevant page commands for the milestone record page.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object</param>
    //-----------------------------------------------------------------------------------
    private void getBudgetItem_PageCommands (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getBudgetItem_PageCommands method. " );
      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.UniForm.Command pageCommand = new Model.UniForm.Command ( );
      PageObject.Status = Evado.Model.UniForm.EditAccess.Enabled;

      //
      // Set the page access and commands based on the user role and the record state.
      //
      if ( this.Session.Budget.State == EvBudget.BudgetState.Draft
        && this.Session.UserProfile.hasBudgetEditAccess == true )
      {
        PageObject.Status = Evado.Model.UniForm.EditAccess.Enabled;

        // 
        // Add the save groupCommand and add it to the groupCommand list.
        // 
        pageCommand = PageObject.addCommand (
         EvLabels.BudgetItem_Refresh_Command_Title,
         EuAdapter.APPLICATION_ID,
         EuAdapterClasses.Budgets.ToString ( ),
         Evado.Model.UniForm.ApplicationMethods.Custom_Method );

        pageCommand.SetPageId ( EvPageIds.Budget_Item_Page );

        pageCommand.SetGuid ( this.Session.BudgetItem.Guid );
        pageCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.Get_Object );

        // 
        // Add the save groupCommand and add it to the groupCommand list.
        // 
        pageCommand = PageObject.addCommand (
         EvLabels.BudgetItem_Save_Command_Title,
         EuAdapter.APPLICATION_ID,
         EuAdapterClasses.Budgets.ToString ( ),
         Evado.Model.UniForm.ApplicationMethods.Save_Object );

        pageCommand.SetPageId ( EvPageIds.Budget_Item_Page );

        pageCommand.SetGuid ( this.Session.BudgetItem.Guid );
      }

      this.LogMethodEnd ( "getBudgetItem_PageCommands" );

    }//END getObject_PageCommands method

    //===================================================================================
    /// <summary>
    /// This method generates the relevant page commands for the milestone record page.
    /// </summary>
    /// <param name="PageGroup">Evado.Model.UniForm.Page object</param>
    //-----------------------------------------------------------------------------------
    private void getBudgetItem_GroupCommands (
      Evado.Model.UniForm.Group PageGroup )
    {
      this.LogMethod ( "getBudgetItem_GroupCommands method. " );
      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.UniForm.Command groupCommand = new Model.UniForm.Command ( );

      //
      // Set the page access and commands based on the user role and the record state.
      //
      if ( this.Session.Budget.State == EvBudget.BudgetState.Draft
        && this.Session.UserProfile.hasBudgetEditAccess == true )
      {
        PageGroup.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

        // 
        // Add the save groupCommand and add it to the groupCommand list.
        // 
        groupCommand = PageGroup.addCommand (
         EvLabels.BudgetItem_Refresh_Command_Title,
         EuAdapter.APPLICATION_ID,
         EuAdapterClasses.Budgets.ToString ( ),
         Evado.Model.UniForm.ApplicationMethods.Custom_Method );

        groupCommand.SetPageId ( EvPageIds.Budget_Item_Page );

        groupCommand.SetGuid ( this.Session.BudgetItem.Guid );
        groupCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.Get_Object );

        // 
        // Add the save groupCommand and add it to the groupCommand list.
        // 
        groupCommand = PageGroup.addCommand (
         EvLabels.BudgetItem_Save_Command_Title,
         EuAdapter.APPLICATION_ID,
         EuAdapterClasses.Budgets.ToString ( ),
         Evado.Model.UniForm.ApplicationMethods.Save_Object );

        groupCommand.SetPageId ( EvPageIds.Budget_Item_Page );

        groupCommand.SetGuid ( this.Session.BudgetItem.Guid );
      }

      this.LogMethodEnd ( "getBudgetItem_PageCommands" );

    }//END getObject_PageCommands method

    //===================================================================================
    /// <summary>
    /// This method generates the relevant page page field for the milestone record page.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object</param>
    //-----------------------------------------------------------------------------------
    private void getBudgetItem_General_Group (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getBudgetItem_General_Group method. " );
      this.LogValue ( "PageObject.Status: " + PageObject.Status );
      this.LogValue ( "Budget.StartDate: " + this.Session.BudgetItem.ActivityId );

      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      Evado.Model.UniForm.Group pageGroup = new Model.UniForm.Group ( );
      List<EvOption> optionList = new List<EvOption> ( );

      //
      // generate the pageMenuGroup object.
      //
      pageGroup = PageObject.AddGroup (
        EvLabels.Budget_Item_Page_General_Group_Title );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      //
      // get the group's commands.
      //
      this.getBudgetItem_GroupCommands ( pageGroup );

      // 
      // Create the budget item default cost
      // 
      groupField = pageGroup.createNumericField (
        EvBudgetItem.BudgetItemFieldNames.Default_Cost.ToString ( ),
      EvLabels.Budget_Item_Default_Cost_Field_Title,
       this.Session.BudgetItem.DefaultCost, 0, 2000000 );
      groupField.Layout = EuFormGenerator.ApplicationFieldLayout;
      groupField.AddParameter ( Model.UniForm.FieldParameterList.Unit, "$" );
      groupField.setSendCommandOnChange ( );

      // 
      // Create the budget item default margin
      // 
      groupField = pageGroup.createNumericField (
        EvBudgetItem.BudgetItemFieldNames.Default_Margin.ToString ( ),
      EvLabels.Budget_Item_Default_Margin_Field_Title,
       this.Session.BudgetItem.DefaultMargin, 0, 200 );
      groupField.Layout = EuFormGenerator.ApplicationFieldLayout;
      groupField.AddParameter ( Model.UniForm.FieldParameterList.Unit, "%" );

      //
      // update the default price with trial default margin.
      //
      this.Session.BudgetItem.DefaultPrice =
        this.Session.BudgetItem.DefaultCost;

      if ( this.Session.BudgetItem.DefaultMargin > 0 )
      {
        float defaultMargin = this.Session.BudgetItem.DefaultMargin / 100F;
        this.LogValue ( "defaultMargin: " + defaultMargin );
        this.Session.BudgetItem.DefaultPrice =
          this.Session.BudgetItem.DefaultCost
          * ( 1 + defaultMargin );
      }

      this.LogValue ( "DefaultCost: " + this.Session.BudgetItem.DefaultCost
        + ", DefaultMargin: " + this.Session.BudgetItem.DefaultMargin
        + ", DefaultPrice : " + this.Session.BudgetItem.DefaultPrice );

      // 
      // Create the budget item default price
      // 
      groupField = pageGroup.createReadOnlyTextField (
        EvBudgetItem.BudgetItemFieldNames.Default_Price.ToString ( ),
      EvLabels.Budget_Item_Default_Price_Field_Title,
      "$ " + this.Session.BudgetItem.DefaultPrice.ToString ( "##0.00" ) );
      groupField.Layout = EuFormGenerator.ApplicationFieldLayout;
      groupField.AddParameter ( Model.UniForm.FieldParameterList.Unit, "$" );

      // 
      // Create the budget Cap Type
      // 
      optionList = EvStatics.Enumerations.getOptionsFromEnum ( typeof ( EvBudgetItem.ItemCappedType ), false );

      groupField = pageGroup.createSelectionListField (
        EvBudgetItem.BudgetItemFieldNames.Capped_Type.ToString ( ),
      EvLabels.Budget_Item_Capped_Type_Field_Title,
       this.Session.BudgetItem.CappedType, optionList );
      groupField.Layout = EuFormGenerator.ApplicationFieldLayout;

      // 
      // Create the budget Cap quantity
      // 
      if ( this.Session.BudgetItem.CappedType == EvBudgetItem.ItemCappedType.Not_Capped )
      {
        this.Session.BudgetItem.CappedQuantity = 0;
      }
      groupField = pageGroup.createNumericField (
        EvBudgetItem.BudgetItemFieldNames.Capped_Quantity.ToString ( ),
      EvLabels.Budget_Item_Capped_Quantity_Field_Title,
       this.Session.BudgetItem.CappedQuantity, 0, 200 );
      groupField.Layout = EuFormGenerator.ApplicationFieldLayout;

      this.LogMethodEnd ( "getBudgetItem_General_Group" );

    }//END getObject_General_Group method.

    //===================================================================================
    /// <summary>
    /// This method generates the relevant inary files groups for the milestone record page.
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page object</param>
    //-----------------------------------------------------------------------------------
    private void getBudgetItem_Site_Group (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getBudgetItem_Site_Group method. " );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );

      //
      // create the budget item group 
      //
      Evado.Model.UniForm.Group pageGroup = Page.AddGroup (
        EvLabels.Budget_Item_List_Group_Title,
        Evado.Model.UniForm.EditAccess.Inherited_Access );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      groupField = pageGroup.createTableField (
        EuBudgets.CONST_BUDGET_ITEM_SITES_FIELD_ID,
        EvLabels.Budget_Item_Site_Values_Field_Title, 5 );

      groupField.Description = EvLabels.Budget_Item_Site_Values_Field_Description ;
      //
      // Site name column
      //
      groupField.Table.Header [ 0 ].Text = EvLabels.Budget_Item_Site_Name_Column_Text;
      groupField.Table.Header [ 0 ].TypeId = Evado.Model.UniForm.TableColHeader.ItemTypeReadOnly;
      groupField.Table.Header [ 0 ].Width = "50";

      this.LogValue ( "Header 0: " + groupField.Table.Header [ 0 ].Text );
      //
      // Site cost column
      //
      groupField.Table.Header [ 1 ].Text = EvLabels.Budget_Item_Site_Cost_Column_Text;
      groupField.Table.Header [ 1 ].TypeId = Evado.Model.UniForm.TableColHeader.ItemTypeNumeric;
      groupField.Table.Header [ 1 ].Width = "12";

      this.LogValue ( "Header 1: " + groupField.Table.Header [ 1 ].Text );
      //
      // Site margin column
      //
      groupField.Table.Header [ 2 ].Text = EvLabels.Budget_Item_Site_Margin_Column_Text;
      groupField.Table.Header [ 2 ].TypeId = Evado.Model.UniForm.TableColHeader.ItemTypeNumeric;
      groupField.Table.Header [ 2 ].Width = "12";

      this.LogValue ( "Header 2: " + groupField.Table.Header [ 2 ].Text );

      //
      // Site margin column
      //
      groupField.Table.Header [ 3 ].Text = EvLabels.Budget_Item_Site_Trial_Margin_Column_Text;
      groupField.Table.Header [ 3 ].TypeId = Evado.Model.UniForm.TableColHeader.ItemTypeReadOnly;
      groupField.Table.Header [ 3 ].Width = "12";

      this.LogValue ( "Header 3: " + groupField.Table.Header [ 3 ].Text );

      //
      // Site margin column
      //
      groupField.Table.Header [ 4 ].Text = EvLabels.Budget_Item_Site_Price_Column_Text;
      groupField.Table.Header [ 4 ].TypeId = Evado.Model.UniForm.TableColHeader.ItemTypeReadOnly;
      groupField.Table.Header [ 4 ].Width = "12";

      this.LogValue ( "Header 4: " + groupField.Table.Header [ 4 ].Text );

      this.Session.Budget.updatePricingCalculations ( );
      //
      // Iterate through the list of budget items.
      //
      foreach ( EvBudgetItemOrganization site in this.Session.BudgetItem.Sites )
      {
        Evado.Model.UniForm.TableRow row = groupField.Table.addRow ( );

        this.LogValue ( "site.Name: " + site.Name
          + ", site.SiteCost: " + site.SiteCost
          + ", site.SiteMargin: " + site.SiteMargin
          + ", site.TrialSiteMargin : " + site.ProjectMargin
          + ", site.SitePrice: " + site.SitePrice );

        row.Column [ 0 ] = site.OrgId + EvLabels.Space_Hypen + site.Name;
        row.Column [ 1 ] = site.SiteCost.ToString ( "##0.00" );
        row.Column [ 2 ] = site.SiteMargin.ToString ( "#0.00" );
        row.Column [ 3 ] = site.ProjectMargin.ToString ( "#0.00" );
        row.Column [ 4 ] = site.SitePrice.ToString ( "##0.00" );
      }

      this.LogMethodEnd ( "getBudgetItem_Site_Group" );

    }//END getObject_BudgetItem_Group method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Get Budget Report page.
    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData getBudgetReportPage (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getBudgetReportPage method. " );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
      Evado.Model.UniForm.Group pageGroup = new Model.UniForm.Group ( );
      Evado.Model.UniForm.Command pageCommand = new Model.UniForm.Command ( );

      // 
      // return if not trial id
      // 
      if ( this.Session.Budget.Guid == Guid.Empty )
      {
        this.LogValue ( "budget GUID is empty." );
        return null;
      }

      try
      {
        //
        // If the report xml object is empty generate it
        //
        if ( this.generateReportObject ( PageCommand ) == false )
        {
          if ( this.ErrorMessage == String.Empty )
          {
            this.ErrorMessage = EvLabels.Budget_Report_Page_Error_Message;
          }

          this.LogValue ( "Error returned from report generator." );
          return null;
        }

        // 
        // Initialise the methods variables and objects.
        // 
        clientDataObject.Id = Guid.NewGuid ( );
        clientDataObject.Title =
          String.Format ( EvLabels.Budget_Report_Page_Page_Title,
          this.Session.Budget.Version,
          this.Session.Project.ProjectId,
          this.Session.Project.Title );

        clientDataObject.Page.Id = clientDataObject.Id;
        clientDataObject.Page.Title = clientDataObject.Title;
        clientDataObject.Page.Status = Evado.Model.UniForm.EditAccess.Enabled;

        if ( this.Session.UserProfile.hasBudgetEditAccess == true
          && this.Session.Budget.State == EvBudget.BudgetState.Draft )
        {
          clientDataObject.Page.Status = Evado.Model.UniForm.EditAccess.Enabled;
          //
          // add the page refresh command
          //
          pageCommand = clientDataObject.Page.addCommand (
            EvLabels.Budget_Repprt_Page_Refresh_Command_Title,
            EuAdapter.APPLICATION_ID,
            EuAdapterClasses.Budgets.ToString ( ),
            Model.UniForm.ApplicationMethods.Custom_Method );

          pageCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.Get_Object );

          pageCommand.SetPageId ( EvPageIds.Budget_Report_Page );

          // 
          // Add the save budget command.
          // 
          pageCommand = clientDataObject.Page.addCommand (
            EvLabels.Budget_Save_Command_Title,
            EuAdapter.APPLICATION_ID,
            EuAdapterClasses.Budgets.ToString ( ),
            Evado.Model.UniForm.ApplicationMethods.Save_Object );

          pageCommand.SetPageId ( EvPageIds.Budget_Page );
          pageCommand.SetGuid ( this.Session.Budget.Guid );
          pageCommand.AddParameter (
            Evado.Model.Clinical.EvcStatics.CONST_SAVE_ACTION,
            EvBudget.BudgetSaveActions.Save.ToString ( ) );
        }

        //
        // display the report
        //
        this.getReport_Display_Group ( clientDataObject.Page );

        this.LogMethodEnd ( "getBudgetReportPage" );

        return clientDataObject;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Budget_Report_Page_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      this.LogMethodEnd ( "getBudgetReportPage" );
      return null;

    }//END getBudgetReportPage method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private bool generateReportObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "generateReportObject method." );
      // 
      // Initialise the methods variables and objects.
      // 
      this.Session.Report = new EvReport ( );
      bool refresh = false;

      //
      // if the page command has a custom method then the page is to be refreshed.
      //
      if ( PageCommand.hasParameter ( Model.UniForm.CommandParameters.Custom_Method ) == true )
      {
        this.LogValue ( "Refresh the report" );
        refresh = true;
      }

      //
      // If the report xml object is empty generate it
      //
      if ( this.Session.Budget.XmlBudgetReport != String.Empty
        && refresh == false )
      {
        this.LogValue ( "Retrieving the report from budget object." );

        this.Session.Report = Evado.Model.Clinical.EvcStatics.DeserialiseObject<EvReport> ( this.Session.Budget.XmlBudgetReport );

        if ( this.Session.Report.Guid != Guid.Empty )
        {
          this.LogValue ( "Report found exiting" );

          return true;
        }
      }

      this.LogValue ( "Generating a new instance of the report." );

      //
      // Get the report template.
      //
      if ( this.getReportTemplate ( ) == false )
      {
        this.LogMethodEnd ( "generateReportObject" );
        return false;
      }

      //
      // Generate the budget report objects.
      //
      try
      {
        this.Session.Report = this._Bll_Budgets.getBudgetReport (
          this._BudgetReportTemplate,
          this.Session.Budget.ProjectId,
          this.Session.Budget.Version );

        this.LogValue ( "budgets Status: " + this._Bll_Budgets.Log );
      }
      catch ( Exception Ex )
      {
        this.LogValue ( "Generating budget report "
          + "Exception Event: " + Evado.Model.Clinical.EvcStatics.getException ( Ex ) );
        throw;
      }

      this.LogMethodEnd ( "generateReportObject" );
      return true;
    }//END generateReportObject method

    //  ==================================================================================	
    /// <summary>
    /// This methods retrieves the budget report template if is not loaded.
    /// </summary>
    //  ---------------------------------------------------------------------------------
    private bool getReportTemplate ( )
    {
      this.LogMethod ( "getReportTemplate method." );
      //
      // Initialise the methods variables and objects.
      //
      EvReportTemplates reportTemplates = new EvReportTemplates ( );
      this._BudgetReportTemplate = new EvReport ( );

      //
      // Get the budget report template.
      //
      List<EvReport> budgetReport = reportTemplates.getReportList (
      String.Empty,
      EvReport.ReportTypeCode.Null,
      String.Empty,
      EvReport.ReportScopeTypes.Budget );

      this.LogValue ( "ReportTemplate Log: " + reportTemplates.Log );

      if ( budgetReport.Count != 1 )
      {
        this.ErrorMessage = String.Format (
          EvLabels.Budget_Report_Duplicate_Error_Message,
          budgetReport.Count );
        // 
        this.LogError ( EvEventCodes.Business_Logic_General_Process_Error,
          this.ErrorMessage );

        return false;
      }

      //
      // Update the report object with first instance of the report template list.
      //
      this._BudgetReportTemplate = reportTemplates.getReport ( budgetReport [ 0 ].Guid );

      //
      // If the template is empty then the report template is not defined.
      //
      if ( this._BudgetReportTemplate.Guid == Guid.Empty )
      {
        this.ErrorMessage = EvLabels.Budget_Report_Template_Empty_Error_Message;

        this.LogError (
          EvEventCodes.Business_Logic_General_Process_Error,
          reportTemplates.Log );

        return false;
      }

      this.LogValue ( "ReportTemplate.Title: " + this._BudgetReportTemplate.ReportTitle );
      this.LogValue ( "ReportTemplate.SqlDataSource: " + this._BudgetReportTemplate.SqlDataSource );
      foreach ( EvReportQuery query in this._BudgetReportTemplate.Queries )
      {
        this.LogValue ( query.getAsString ( ) );
      }

      return true;

    } // Close getReportTemplate methhod.

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private void getReport_Display_Group (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getReport_Display_Group method. " );
      //
      // Initialise method variables and objects.
      //
      Evado.Model.UniForm.Field groupField = new Model.UniForm.Field ( );
      Evado.Model.UniForm.Command pageCommand = new Model.UniForm.Command ( );

      // Create the new pageMenuGroup.
      // 
      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
        EvLabels.Reports_Download_Group_Title,
        Evado.Model.UniForm.EditAccess.Enabled );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      //
      // Validate the report structure to stop errors.
      //
      if ( this.Session.Report.DataRecords.Count > 0 )
      {
        if ( this.Session.Report.Columns.Count != this.Session.Report.DataRecords [ 0 ].ColumnValues.Length )
        {
          groupField = pageGroup.createReadOnlyTextField (
            String.Empty,
            String.Empty,
          this.ErrorMessage = EvLabels.Report_column_Mismatch_Error_Message );

          this.LogMethodEnd ( "getReport_Display_Group" );

          return;
        }

        this.LogValue ( "Column count: " + this.Session.Report.Columns.Count );

        String columnValue = String.Empty;
        foreach ( EvReportColumn column in this.Session.Report.Columns )
        {
          if ( columnValue != String.Empty )
          {
            columnValue += ",";
          }
          columnValue += "'" + column.HeaderText + "' = " + column.GroupingIndex + " > " + column.SectionLvl;
        }
        this.LogValue ( columnValue );

        foreach ( EvReportRow row in this.Session.Report.DataRecords )
        {
          String rowValue = "";
          foreach ( string value in row.ColumnValues )
          {
            if ( columnValue != String.Empty )
            {
              columnValue += ",";
            }
            rowValue += "'" + value + "'";
          }
          this.LogValue ( rowValue );
        }

        this.LogValue ( "All Rows outputted." );

        //
        // Set the report title.
        //
        pageGroup.Title = this.Session.Report.ReportTitle;

        pageGroup.Description = this.Session.Report.getReportAsHtml ( this.Session.UserProfile ) ;

        this.LogValue ( this.Session.Report.DebugLog );

      } //END report has content.
      else
      {
        groupField = pageGroup.createReadOnlyTextField (
          String.Empty,
          String.Empty,
        this.ErrorMessage = EvLabels.Report_Result_Empty_Message );
      }

      this.LogMethodEnd ( "getReport_Display_Group" );

    }//END getReport_Display_Group method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class create methods

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="Command">Evado.Model.UniForm.Command object.</param>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData createObject (
      Evado.Model.UniForm.Command Command )
    {
      try
      {
        this.LogMethod ( "createObject method. " );
        // 
        // Initialise the methods variables and objects.
        //      
        Evado.Model.UniForm.AppData data = new Evado.Model.UniForm.AppData ( );
        this.Session.Budget = new EvBudget ( );
        this.Session.Budget.Guid = Evado.Model.Clinical.EvcStatics.CONST_NEW_OBJECT_ID;
        this.Session.Budget.State = EvBudget.BudgetState.Draft;
        this.Session.Budget.ProjectId = this.Session.Project.ProjectId;
        this.Session.Budget.ProjectMargin = 0; //this.Session.Project.Data.DefaultMargin;
        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "createObject",
          this.Session.UserProfile );

        this.getBudget_DataObject ( data );


        this.LogValue ( "Exit createObject method. ID: " + data.Id + ", Title: " + data.Title );

        return data;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = "Error raised when creating a subject record.";

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return null;

    }//END method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class update Budget methods

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData updateBudget_Object (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "updateBudget_Object method. " );
      this.LogValue ( "PageEvado.Model.UniForm.Command: " + PageCommand.getAsString ( false, false ) );

      this.LogValue ( "Guid: " + this.Session.Budget.Guid );
      this.LogValue ( "ProjectId: " + this.Session.Budget.ProjectId );
      this.LogValue ( "Title: " + this.Session.Budget.Description );
      try
      {
        //
        // Initialise the methods variables and objects.
        //
        this.Session.Budget.SaveAction = EvBudget.BudgetSaveActions.Save;

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "updateObject",
          this.Session.UserProfile );

        // 
        // Update the object.
        // 
        if ( this.updateBudget_ObjectValue ( PageCommand ) == false )
        {
          this.ErrorMessage = EvLabels.Budget_Update_Error_Message;

          return this.Session.LastPage;
        }

        // 
        // Delete the object.
        // 
        if ( PageCommand.Method == Evado.Model.UniForm.ApplicationMethods.Delete_Object )
        {
          return new AppData ( );
        }

        // 
        // Get the save action value.
        // 
        String stSaveAction = PageCommand.GetParameter ( Evado.Model.Clinical.EvcStatics.CONST_SAVE_ACTION );
        if ( stSaveAction != String.Empty )
        {
          this.Session.Budget.SaveAction =
            EvStatics.Enumerations.parseEnumValue<EvBudget.BudgetSaveActions> ( stSaveAction );
        }
        this.LogValue ( "Budget.SaveAction: " + this.Session.Budget.SaveAction );

        //
        // if the guid is set to new object reset it to empty to add a new record.
        //
        this.LogValue ( "Budget.Guid: " + this.Session.Budget.Guid );
        if ( this.Session.Budget.Guid == Evado.Model.Clinical.EvcStatics.CONST_NEW_OBJECT_ID )
        {
          this.Session.Budget.Guid = Guid.Empty;
          this.Session.Budget.ProjectId = this.Session.Project.ProjectId;
          this.Session.Budget.SaveAction = EvBudget.BudgetSaveActions.Save;
        }

        this.LogValue ( "Budget.Guid: " + this.Session.Budget.Guid );

        // 
        // Initialise the update variables.
        // 
        this.Session.Budget.UpdatedByUserId = this.Session.UserProfile.UserId;
        this.Session.Budget.UpdatedBy = this.Session.UserProfile.CommonName;
        this.Session.Budget.UpdatedDate = DateTime.Now;

        //
        // Append the report XMl object if it exists.
        //
        if ( this.Session.Report.ReportScope == EvReport.ReportScopeTypes.Budget
          && this.Session.Report.Guid != Guid.Empty )
        {
          String xmlBudgetReport = EvStatics.SerialiseObject<EvReport> ( this.Session.Report );
          this.Session.Budget.XmlBudgetReport = xmlBudgetReport;
        }

        // 
        // update the object.
        // 
        EvEventCodes result = this._Bll_Budgets.saveBudget ( this.Session.Budget );

        // 
        // get the debug ResultData.
        // 
        this.LogValue ( this._Bll_Budgets.Log );

        // 
        // if an error state is returned create log the event.
        // 
        if ( result != EvEventCodes.Ok )
        {
          string StEvent = this._Bll_Budgets.Log + " returned error message: " + Evado.Model.Clinical.EvcStatics.getEventMessage ( result );
          this.LogError ( EvEventCodes.Database_Record_Update_Error, StEvent );

          switch ( result )
          {
            case EvEventCodes.Identifier_Project_Id_Error:
              {
                this.ErrorMessage = EvLabels.Project_Identifier_Empty_Error_Message;
                break;
              }
            default:
              {
                this.ErrorMessage = EvLabels.Activity_Update_Error_Message;
                break;
              }
          }
          return this.Session.LastPage;
        }

        //
        // reset the budget list and object to ensure a new versions are loaded.
        //
        this.Session.Budget = new EvBudget ( );
        this.Session.BudgetList = new List<EvBudget> ( );

        return new AppData ( );

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Budget_Update_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }
      return this.Session.LastPage;

    }//END method

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="Parameters">List of field values to be updated.</param>
    /// <returns></returns>
    //  ----------------------------------------------------------------------------------
    private bool updateBudget_ObjectValue ( Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "updateObjectValue method. " );
      this.LogValue ( "Parameters.Count: " + PageCommand.Parameters.Count );
      this.LogValue ( "Budget.Guid: " + this.Session.Budget.Guid );

      // 
      // Iterate through the parameter values updating the ResultData object
      // 
      foreach ( Evado.Model.UniForm.Parameter parameter in PageCommand.Parameters )
      {
        if ( parameter.Name.Contains ( Evado.Model.Clinical.EvcStatics.CONST_GUID_IDENTIFIER ) == true
          || parameter.Name == Evado.Model.UniForm.CommandParameters.Custom_Method.ToString ( )
          || parameter.Name == Model.UniForm.CommandParameters.Short_Title.ToString ( )
          || parameter.Name == Evado.Model.Clinical.EvcStatics.CONST_SAVE_ACTION
          || parameter.Name == Model.UniForm.CommandParameters.Page_Id.ToString ( )
          || parameter.Name.Contains ( EuBudgets.CONST_BUDGET_ITEM_SITES_FIELD_ID ) == true )
        {
          continue;
        }

        this.LogValue ( parameter.Name + " > " + parameter.Value + " >> UPDATED" );
        try
        {
          EvBudget.BudgetFieldNames fieldName = Evado.Model.Clinical.EvcStatics.Enumerations.parseEnumValue<EvBudget.BudgetFieldNames> ( parameter.Name );

          this.Session.Budget.setValue ( fieldName, parameter.Value );
        }
        catch ( Exception Ex )
        {
          this.LogException ( Ex );

          // 
          // Create the error message to be displayed to the user.
          // 
          this.ErrorMessage = EvLabels.Budget_Update_Error_Message;

          this.LogMethodEnd ( "updateObjectValue" );

          return false;
        }//END Catch statement

      }//END parmater Iteration loop

      return true;

    }//END method.

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class update Budget item methods

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData updateBudgetItem_Object (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "updateBudgetItem_Object method. " );
      this.LogValue ( "Command: " + PageCommand.getAsString ( false, false ) );
      this.LogValue ( "Guid: " + this.Session.BudgetItem.Guid );
      this.LogValue ( "ProjectId: " + this.Session.BudgetItem.ProjectId );
      this.LogValue ( "Title: " + this.Session.BudgetItem.Title );
      try
      {
        //
        // Initialise the methods variables and objects.
        //
        EvEventCodes result = EvEventCodes.Ok;

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "updateBudgetItem_Object",
          this.Session.UserProfile );

        // 
        // Delete the object.
        // 
        if ( PageCommand.Method == Evado.Model.UniForm.ApplicationMethods.Delete_Object )
        {
          return new AppData ( );
        }

        //
        // if the guid is set to new object reset it to empty to add a new record.
        //
        if ( this.Session.BudgetItem.Guid == Evado.Model.Clinical.EvcStatics.CONST_NEW_OBJECT_ID )
        {
          this.Session.BudgetItem.Guid = Guid.Empty;
          this.Session.BudgetItem.ProjectId = this.Session.Budget.ProjectId;
          this.Session.BudgetItem.BudgetGuid = this.Session.Budget.Guid;
        }

        // 
        // Update the object.
        // 
        if ( this.updateBudgetItem_ObjectValue ( PageCommand ) == false )
        {
          this.ErrorMessage = EvLabels.Budget_Item_Update_Error_Message;

          return this.Session.LastPage;
        }

        //
        // Update the budget item site values.
        //
        if ( this.updateBudgetItemSite_ObjectValue ( PageCommand ) == false )
        {
          this.ErrorMessage = EvLabels.Budget_Item_Update_Error_Message;

          return this.Session.LastPage;
        }

        //
        // Validate the budget item values.
        //
        if ( this.updateBudgetItem_Validation ( ) == false )
        {
          return this.Session.LastPage;
        }

        // 
        // Initialise the update variables.
        // 
        this.Session.BudgetItem.UpdatedByUserId = this.Session.UserProfile.UserId;
        this.Session.BudgetItem.UserCommonName = this.Session.UserProfile.CommonName;
        this.Session.BudgetItem.UpdateDate = DateTime.Now;

        // 
        // update the object.
        // 
        result = this._Bll_Budgets.saveBudgetItem ( this.Session.BudgetItem );

        // 
        // get the debug ResultData.
        // 
        this.LogValue ( this._Bll_Budgets.Log );

        // 
        // if an error state is returned create log the event.
        // 
        if ( result != EvEventCodes.Ok )
        {
          // 
          // Create the error message to be displayed to the user.
          // 
          this.ErrorMessage = EvLabels.Budget_Item_Update_Error_Message;

          string StEvent = this._Bll_Budgets.Log + " returned error message: " + Evado.Model.Clinical.EvcStatics.getEventMessage ( result );

          this.LogError ( EvEventCodes.Database_Record_Update_Error, StEvent );

          switch ( result )
          {
            case EvEventCodes.Identifier_Project_Id_Error:
              {
                this.ErrorMessage = EvLabels.Project_Identifier_Empty_Error_Message;
                break;
              }
            default:
              {
                this.ErrorMessage = EvLabels.Activity_Update_Error_Message;
                break;
              }
          }
          return this.Session.LastPage;
        }

        //
        // Re-initialise the budget object to force the loading of a new budget with new components.
        //
        this.Session.Budget = new EvBudget ( );

        this.LogMethodEnd ( "updateBudgetItem_Object" );

        return new AppData ( );

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Budget_Item_Update_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      this.LogMethodEnd ( "updateBudgetItem_Object" );
      return this.Session.LastPage;

    }//END updateBudgetItem_Object method

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="Parameters">List of field values to be updated.</param>
    /// <returns></returns>
    //  ----------------------------------------------------------------------------------
    private bool updateBudgetItem_Validation ( )
    {
      this.LogMethod ( "updateBudgetItem_Validation method. " );
      // 
      // Initialise the methods variables and objects.
      // 
      this.ErrorMessage = String.Empty;

      //
      // validate that the default margin is not negative.
      //
      if ( this.Session.BudgetItem.DefaultCost < 0 )
      {
        this.ErrorMessage += EvLabels.Budget_Item_Default_Cost_Error_Message;
      }
      if ( this.ErrorMessage != String.Empty )
      {
        this.ErrorMessage += "\n\n";
      }

      //
      // validate that the default margin is not negative.
      //
      if ( this.Session.BudgetItem.DefaultMargin < 0 )
      {
        this.ErrorMessage += EvLabels.Budget_Item_Default_Margin_Error_Message;
      }
      if ( this.ErrorMessage != String.Empty )
      {
        this.ErrorMessage += "\n\n";
      }

      //
      // Validate that the capped quantity is defined for the current capped selection.
      //
      if ( this.Session.BudgetItem.CappedType != EvBudgetItem.ItemCappedType.Not_Capped
        && this.Session.BudgetItem.CappedQuantity == 0 )
      {
        this.ErrorMessage += String.Format (
          EvLabels.Budget_Item_Cap_Quantity_Error_Message,
          Model.EvStatics.enumValueToString ( this.Session.BudgetItem.CappedType ) );
      }
      if ( this.ErrorMessage != String.Empty )
      {
        this.ErrorMessage += "\n\n";
      }

      foreach ( EvBudgetItemOrganization site in this.Session.BudgetItem.Sites )
      {
        //
        // validate that the site costs are not negative.
        //
        if ( site.SiteCost < 0 )
        {
          this.ErrorMessage += String.Format (
            EvLabels.Budget_Item_Site_Cost_Error_Message,
            site.OrgId,
            site.Name );
        }
        if ( this.ErrorMessage != String.Empty )
        {
          this.ErrorMessage += "\n\n";
        }

        //
        // validate that the default margin is not negative.
        //
        if ( site.SiteCost < 0 )
        {
          this.ErrorMessage += String.Format (
            EvLabels.Budget_Item_Site_Margin_Error_Message,
            site.OrgId,
            site.Name );
        }
        if ( this.ErrorMessage != String.Empty )
        {
          this.ErrorMessage += "\n\n";
        }
      }//END Sites iteration loop.

      //
      // if an errror message has been created then there is an error so return false;
      //
      if ( this.ErrorMessage != String.Empty )
      {
        return false;
      }

      return true;
    }

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="Parameters">List of field values to be updated.</param>
    /// <returns></returns>
    //  ----------------------------------------------------------------------------------
    private bool updateBudgetItem_ObjectValue ( Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "updateBudgetItem_ObjectValue method. " );
      this.LogValue ( "Parameters.Count: " + PageCommand.Parameters.Count );
      this.LogValue ( "BudgetItem.Guid: " + this.Session.BudgetItem.Guid );

      // 
      // Iterate through the parameter values updating the ResultData object
      // 
      foreach ( Evado.Model.UniForm.Parameter parameter in PageCommand.Parameters )
      {
        if ( parameter.Name.Contains ( Evado.Model.Clinical.EvcStatics.CONST_GUID_IDENTIFIER ) == true
          || parameter.Name == Model.UniForm.CommandParameters.Custom_Method.ToString ( )
          || parameter.Name == Model.UniForm.CommandParameters.Short_Title.ToString ( )
          || parameter.Name == Model.UniForm.CommandParameters.Page_Id.ToString ( )
          || parameter.Name == Evado.Model.Clinical.EvcStatics.CONST_SAVE_ACTION
          || parameter.Name.Contains ( EuBudgets.CONST_BUDGET_ITEM_SITES_FIELD_ID ) == true )
        {
          continue;
        }

        this.LogValue ( parameter.Name + " > " + parameter.Value + " >> UPDATED" );
        try
        {
          EvBudgetItem.BudgetItemFieldNames fieldName =
            Model.EvStatics.Enumerations.parseEnumValue<EvBudgetItem.BudgetItemFieldNames> ( parameter.Name );

          this.Session.BudgetItem.setValue ( fieldName, parameter.Value );
        }
        catch ( Exception Ex )
        {
          // 
          // Create the error message to be displayed to the user.
          // 
          this.ErrorMessage = EvLabels.Budget_Item_Update_Error_Message;

          this.LogException ( Ex );

          this.LogMethodEnd ( "updateObjectValue" );
        }
      }// End iteration loop

      //
      // update the default price with trial default margin.
      //
      this.Session.BudgetItem.DefaultPrice =
        this.Session.BudgetItem.DefaultCost;

      if ( this.Session.BudgetItem.DefaultMargin > 0 )
      {
        float defaultMargin = this.Session.BudgetItem.DefaultMargin / 100F;
        this.Session.BudgetItem.DefaultPrice =
          this.Session.BudgetItem.DefaultCost
          * ( 1 + defaultMargin );
      }

      this.LogValue ( "DefaultCost: " + this.Session.BudgetItem.DefaultCost
        + ", DefaultMargin: " + this.Session.BudgetItem.DefaultMargin
        + ", DefaultPrice : " + this.Session.BudgetItem.DefaultPrice );

      this.LogMethodEnd ( "updateBudgetItem_ObjectValue" );

      return true;

    }//END method.

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="Parameters">List of field values to be updated.</param>
    /// <returns></returns>
    //  ----------------------------------------------------------------------------------
    private bool updateBudgetItemSite_ObjectValue ( Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "updateBudgetItemSite_ObjectValue method. " );
      //
      // Initialise the methods variables and objects.
      //
      String stParameterName = String.Empty;
      string tableValue = String.Empty;

      //
      // Budget item sites iteration loop
      //
      for ( int iCount = 0; iCount < this.Session.BudgetItem.Sites.Count; iCount++ )
      {
        EvBudgetItemOrganization site = this.Session.BudgetItem.Sites [ iCount ];

        //
        // get the site cost value.
        //
        stParameterName = EuBudgets.CONST_BUDGET_ITEM_SITES_FIELD_ID + "_" + ( iCount + 1 + "_2" );
        tableValue = PageCommand.GetParameter ( stParameterName );
        this.LogValue ( "Count: " + iCount + " stParameterName: " + stParameterName + " tableValue: " + tableValue );

        if ( tableValue != String.Empty )
        {
          site.SiteCost = EvStatics.getFloat ( tableValue );
        }
        this.LogValue ( "Count: " + iCount + " site.SiteCost: " + site.SiteCost );

        //
        // get the site margin value.
        //
        stParameterName = EuBudgets.CONST_BUDGET_ITEM_SITES_FIELD_ID + "_" + ( iCount + 1 + "_3" );
        tableValue = PageCommand.GetParameter ( stParameterName );
        this.LogValue ( "Count: " + iCount + " stParameterName: " + stParameterName + " tableValue: " + tableValue );

        if ( tableValue != String.Empty )
        {
          site.SiteMargin = EvStatics.getFloat ( tableValue );
        }
        this.LogValue ( "Count: " + iCount + " site.SiteMargin: " + site.SiteMargin );

        //
        // Update the budget pricing calculations
        //
        this.Session.Budget.updatePricingCalculations ( );

        this.LogValue ( "Count: " + iCount
          + " site.SiteCost: " + site.SiteCost
          + " site.SiteMargin: " + site.SiteMargin
          + " site.TrialSiteMargin : " + site.ProjectMargin
          + " site.SitePrice: " + site.SitePrice );

      }//END end of iteration loop

      this.LogMethodEnd ( "updateBudgetItemSite_ObjectValue" );

      return true;

    }//END updateBudgetItemSite_ObjectValue method.


    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
  }
}//END namespace