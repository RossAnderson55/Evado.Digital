/***************************************************************************************
 * <copyright file=Evado.UniForm.Clinical\SubjectRescords.cs" company="EVADO HOLDING PTY. LTD.">
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
  public class EuBillingRecords : EuClassAdapterBase
  {
    #region Class Initialisation
    /// <summary>
    /// This method initialises the class.
    /// </summary>
    public EuBillingRecords ( )
    {
      this.ClassNameSpace = "Evado.UniForm.Clinical.EuBillinRecords";
    }

    /// <summary>
    /// This method initialises the class and passs in the user profile.
    /// </summary>
    public EuBillingRecords (
      EuApplicationObjects ApplicationObjects,
      EvUserProfileBase ServiceUserProfile,
      EuSession SessionObjects,
      String UniFormBinaryFilePath,
      String UniFormServiceBinaryUrl,
      EvApplicationSetting Settings)
    {
      this.ApplicationObjects = ApplicationObjects;
      this.ServiceUserProfile = ServiceUserProfile;
      this.Session = SessionObjects;
      this.UniForm_BinaryFilePath = UniFormBinaryFilePath;
      this.UniForm_BinaryServiceUrl = UniFormServiceBinaryUrl;
      this.Settings = Settings;
      this.LoggingLevel = Settings.LoggingLevel;
      this.ClassNameSpace = "Evado.UniForm.Clinical.EuBillinRecords";

      this.LogInitMethod ( "initialisation method. " );
      this.LogInitValue ( "ServiceUserProfile.UserId: " + ServiceUserProfile.UserId );
      this.LogInitValue ( "SessionObjects.Project.ProjectId: " + this.Session.Project.ProjectId );
      this.LogInitValue ( "SessionObjects.UserProfile.Userid: " + this.Session.UserProfile.UserId );
      this.LogInitValue ( "SessionObjects.UserProfile.CommonName: " + this.Session.UserProfile.CommonName );
      this.LogInitValue ( "UniFormBinaryFilePath: " + this.UniForm_BinaryFilePath );
      this.LogInitValue ( "UniFormBinaryServiceUrl: " + this.UniForm_BinaryServiceUrl );

      this._Bll_BillingRecords = new Evado.Bll.Clinical.EvBillingRecords ( this.Settings ); 

      if ( this.Session.BillingRecordList == null )
      {
        this.Session.BillingRecordList = new List<EvBillingRecord> ( );
      }
      if ( this.Session.BillingRecord == null )
      {
        this.Session.BillingRecord = new EvBillingRecord ( );
      }
      if ( this.Session.BillingRecordItem == null )
      {
        this.Session.BillingRecordItem = new EvBillingRecordItem ( );
      }
      if ( this.Session.BillingOrgId == null )
      {
        this.Session.BillingOrgId = String.Empty;
      }
      if ( this.Session.BillingMilestoneId == null )
      {
        this.Session.BillingMilestoneId = String.Empty;
      }
      if ( this.Session.BillingSubjectId == null )
      {
        this.Session.BillingSubjectId = String.Empty;
      }
    }//END Method


    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class constants and variables.

    private Evado.Bll.Clinical.EvBillingRecords _Bll_BillingRecords = new Evado.Bll.Clinical.EvBillingRecords ( );

    private const String CONST_BILLING_ITEM_FIELD_ID = "BR_IMS";

    public const string CONST_UPDLOAD_FIELD_ID = "BUIF";

    public const string CONST_BILLING_PENDING_ICON = "{{ICON0}}";
    public const string CONST_BILLING_INVOICED_ICON = "{{ICON1}}";
    public const string CONST_BILLING_PAID_ICON = "{{ICON2}}";


    public const string ICON_BILLING_PENDING_URL = "icons/record-draft.png";
    public const string ICON_BILLING_INVOICED__URL = "icons/form-issued.png";
    public const string ICON_BILLING_PAID_URL = "icons/form-withdrawn.png";

    EvReport _BillingReportTemplate = new EvReport ( );

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
      this.LogValue ( "PageCommand " + PageCommand.getAsString ( false, true ) );

      try
      {
        // 
        // Initialise the methods variables and objects.
        // 
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );

        //
        // if the current report is not a budget report reset it.
        //
        this.LogValue ( "Report.ReportScope " + this.Session.Report.ReportScope );
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
            "Evado.UniForm.Clinical.EuBillinRecords module",
            this.Session.UserProfile );

          this.ErrorMessage = EvLabels.Illegal_Page_Access_Attempt;

          return this.Session.LastPage;
        }

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          "Evado.UniForm.Clinical.EuBillinRecords module",
          this.Session.UserProfile );

        //
        // Set the page command page id.
        //
        this.Session.setPageId ( PageCommand.GetPageId ( ) );

        this.LogValue ( "PageId: " + this.Session.PageId );

        //
        // update the selection values.
        //
        this.updateSelectionValue ( PageCommand );

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
                case EvPageIds.Billing_Report:
                  {
                    clientDataObject = this.getBillingRecordReportPage ( PageCommand );
                    break;
                  }
                default:
                  {
                    clientDataObject = this.getBillingRecordPage ( PageCommand );
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
              this.LogValue ( " Save Object method" );
              clientDataObject = new Evado.Model.UniForm.AppData ( );

              clientDataObject = this.updateBillinRecord_Object ( PageCommand );
              break;
            }

        }//END Swith

        if ( clientDataObject == null )
        {
          this.LogMethodEnd ( "Called method returned an error." );
        }

        //
        // Append the error message.
        //
        if ( this.ErrorMessage != String.Empty  )
        {
          clientDataObject.Message = this.ErrorMessage;
        }

        this.LogMethodEnd ( "getDataObject" );
        // 
        // return the client ResultData object.
        // 
        return clientDataObject;

      }
      catch ( Exception Ex )
      {
        this.LogException ( Ex );
      }
      return this.Session.LastPage;

    }//END getSubjectMilestoneObject method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private void updateSelectionValue (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "updateSelectionValue method. " );

      this.Session.BillingOrgId = PageCommand.GetParameter ( EvIdentifiers.ORGANISATION_ID);

      this.Session.BillingMilestoneId = PageCommand.GetParameter ( EvIdentifiers.MILESTONE_ID );

      this.Session.BillingSubjectId = PageCommand.GetParameter ( EvIdentifiers.SUBJECT_ID );

    }

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

        // 
        // Initialise the methods variables and objects.
        //      
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
        Evado.Model.UniForm.Command groupCommand = new Model.UniForm.Command ( );
        Evado.Model.UniForm.Group pageGroup = new Model.UniForm.Group ( );
        List<EvBillingRecord.BillingStates> states = new List<EvBillingRecord.BillingStates> ( );
        states.Add ( EvBillingRecord.BillingStates.Null );

        clientDataObject.Title = EvLabels.Budget_List_Page_Title;
        clientDataObject.Page.Title = clientDataObject.Title;
        clientDataObject.Id = Guid.NewGuid ( );
        this.LogValue ( "data.Page.Title: " + clientDataObject.Page.Title );

        //
        // Ouput the export group if selected.
        //
        if ( this.Session.UserProfile.hasFinanceEditAccess == true )
        {
          this.getBillingRecordListPageCommands ( clientDataObject.Page );
        }

        if ( this.Session.PageId == EvPageIds.Budget_Export_Page )
        {
          this.getBillingRecord_Export_Group ( clientDataObject.Page );
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
          EuBillingRecords.ICON_BILLING_PENDING_URL );

        clientDataObject.Page.setImageUrl (
          Model.UniForm.PageImageUrls.Image1_Url,
          EuBillingRecords.ICON_BILLING_INVOICED__URL );

        clientDataObject.Page.setImageUrl (
          Model.UniForm.PageImageUrls.Image2_Url,
          EuBillingRecords.ICON_BILLING_PAID_URL );

        // 
        // get the list of customers.
        // 
        this.Session.BillingRecordList = this._Bll_BillingRecords.getBillingRecordView (
          this.Session.Project.ProjectId, states );

        this.LogValue ( this._Bll_BillingRecords.DebugLog );
        this.LogValue ( "list count: " + this.Session.BillingRecordList.Count );

        //
        // Create a budget command if the budget list is emptry.
        //
        if ( this.Session.UserProfile.hasFinanceEditAccess == true )
        {
          groupCommand = pageGroup.addCommand (
            EvLabels.BillingRecord_Add_Record_Command_Title,
            EuAdapter.APPLICATION_ID,
            EuAdapterClasses.Billing_Records.ToString ( ),
            Model.UniForm.ApplicationMethods.Create_Object );

          groupCommand.SetPageId ( EvPageIds.Billing_Record_Page );

          groupCommand.SetBackgroundColour (
            Model.UniForm.CommandParameters.BG_Default,
            Model.UniForm.Background_Colours.Purple );
        }

        // 
        // generate the page links.
        // 
        foreach ( EvBillingRecord billingRecord in this.Session.BillingRecordList )
        {
          this.LogValue ( "BillingRecord InvoiceReference: " + billingRecord.InvoiceReference + ", Guid: " + billingRecord.Guid );

          groupCommand = pageGroup.addCommand (
            billingRecord.Details,
            EuAdapter.APPLICATION_ID,
            EuAdapterClasses.Billing_Records.ToString ( ),
            Evado.Model.UniForm.ApplicationMethods.Get_Object );

          groupCommand.SetGuid ( billingRecord.Guid );

          groupCommand.SetPageId ( EvPageIds.Billing_Record_Page );

          switch ( billingRecord.State )
          {
            case Evado.Model.Clinical.EvBillingRecord.BillingStates.Pending:
              {
                groupCommand.Title = EuBillingRecords.CONST_BILLING_PENDING_ICON + " ";
                break;
              }
            case Evado.Model.Clinical.EvBillingRecord.BillingStates.Invoiced:
              {
                groupCommand.Title = EuBillingRecords.CONST_BILLING_INVOICED_ICON + " ";
                break;
              }
            case Evado.Model.Clinical.EvBillingRecord.BillingStates.Paid:
              {
                groupCommand.Title = EuBillingRecords.CONST_BILLING_PAID_ICON + " ";
                break;
              }
            default:
              {
                groupCommand.Title = String.Empty;
                break;
              }
          }//END state switch.

          groupCommand.Title += String.Format (
            EvLabels.BillingRecord_List_Command_Title,
             billingRecord.Details );

          groupCommand.AddParameter ( Model.UniForm.CommandParameters.Short_Title,
             String.Format (
              EvLabels.BillIngRecord_List_Short_Title,
              billingRecord.BillingReportId ) );

        }//END ancillary command iteration loop.

        if ( pageGroup.CommandList.Count == 0 )
        {
          pageGroup.Description = EvLabels.BillinRecord_List_Page_No_Records_Label ;
        }


        this.LogValue ( "command object count: " + pageGroup.CommandList.Count );

        return clientDataObject;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.BillingRecord_Page_Error_Message;

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
    private void getBillingRecordListPageCommands (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getBillingRecordListPageCommands method. " );
      /*
      // 
      // initialise the save groupCommand.
      // 
      Evado.Model.UniForm.Command pageCommand = PageObject.addCommand (
        EvLabels.Budget_Export_Command_Title,
        EuAdapter.APPLICATION_ID,
        EuAdapter.ApplicationObjects.BillingRecords.ToString ( ),
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
        EuAdapter.ApplicationObjects.BillingRecords.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Custom_Method );

      // 
      // Define the pageCommand parameters.
      // 
      pageCommand.setCustomMethod ( ApplicationMethods.List_of_Objects );
      pageCommand.SetPageId ( EvPageIds.Budget_Import_Page );
      */
      return;

    }//END getBudgetPage_Commands method

    //===================================================================================
    /// <summary>
    /// This method generates the relevant page page field for the milestone record page.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object</param>
    //-----------------------------------------------------------------------------------
    private void getBillingRecord_Export_Group (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getBillingRecord_Export_Group method. " );

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

      pageGroup.Description =  String.Format (
          EvLabels.Budget_Export_Description,
          this.Session.Project.ProjectId );

      // 
      // Create the Project id object
      // 
      pageField = pageGroup.createReadOnlyTextField (
        String.Empty,
        EvLabels.Busget_Export_Process_Log,
        EvStatics.getStringAsHtml ( csvServices.ProcessLog ) );
      pageField.Layout = FieldLayoutCodes.Column_Layout;

      this.LogMethodEnd ( "getBudgetList_Export_Group" );

    }//END getBillingRecord_Export_Group method.

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
      if ( PageCommand.hasParameter ( EuBillingRecords.CONST_UPDLOAD_FIELD_ID ) == false )
      {
        this.LogValue ( "UploadFileName is empty" );

        this.getBillingRecordUploadGroup ( PageObject );
      }
      else
      {
        this.LogValue ( "Saving upload" );

        result = this.saveBillingRecordUpload ( PageCommand, PageObject );
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
    private void getBillingRecordUploadGroup (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getBillingRecordUploadGroup method. " );

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

      pageGroup.SetCommandBackBroundColor (
        Model.UniForm.GroupParameterList.BG_Mandatory,
        Model.UniForm.Background_Colours.Red );

      groupField = pageGroup.createBinaryFileField (
        EuBillingRecords.CONST_UPDLOAD_FIELD_ID,
        EvLabels.Budget_Upload_Field_Title,
        String.Empty,
        this.Session.UploadFileName );
      groupField.Layout = EuFormGenerator.ApplicationFieldLayout;

      groupField.AddParameter ( Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, "Yes" );

      groupCommand = pageGroup.addCommand (
        EvLabels.Budget_Upload_Command_Title,
        EuAdapter.APPLICATION_ID,
        EuAdapterClasses.Billing_Records.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Custom_Method );

      groupCommand.SetPageId ( EvPageIds.Budget_Import_Page );
      groupCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.List_of_Objects );

    }//END getBillingRecordUploadGroup Method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.AppData object.</param>
    //  ------------------------------------------------------------------------------
    private bool saveBillingRecordUpload (
      Evado.Model.UniForm.Command PageCommand,
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "saveBillingRecordUpload method. " );
      // 
      // Initialise the client ResultData object.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Command groupCommand = new Evado.Model.UniForm.Command ( );
      Evado.Bll.Integration.EiCsvServices csvServices = new Bll.Integration.EiCsvServices ( );
      Evado.Model.Integration.EiData result = new Model.Integration.EiData ( );
      List<String> csvDataList = new List<string> ( );
      string fileName = PageCommand.GetParameter ( EuBillingRecords.CONST_UPDLOAD_FIELD_ID );
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
        EuAdapterClasses.Billing_Records.ToString ( ),
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

      pageGroup.Description = result.ProcessLog ;


      // 
      // Generate the log the action event.
      // 

      this.LogEvent ( result.ProcessLog );


      return true;

    }//END saveBillingRecordUpload Method

    //*******************************************************************************
    #endregion

    #region Get Billing Record page.
    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData getBillingRecordPage (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getBillingRecordPage method. " );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
      Guid billingRecordGuid = Guid.Empty;

      // 
      // if the parameter value exists then set the customerId
      // 
      billingRecordGuid = PageCommand.GetGuid ( );
      this.LogValue ( "RecordGuid: " + billingRecordGuid );

      // 
      // return if not trial id
      // 
      if ( billingRecordGuid == Guid.Empty )
      {
        this.LogValue ( "Record GUID is empty." );
        return null;
      }

      try
      {
        if ( this.Session.BillingRecord.Guid != billingRecordGuid )
        {
          this.LogValue ( "Budget Guids do not match so load a new instance." );
          // 
          // Retrieve the customer object from the database via the DAL and BLL layers.
          // 
          this.Session.BillingRecord = this._Bll_BillingRecords.getBillingRecord ( billingRecordGuid );

          this.LogValue ( this._Bll_BillingRecords.DebugLog );
        }
        else
        {
          this.LogValue ( "Billing Record in memory so update session object." );
          // 
          // Update the object.
          // 
          this.updateBillingRecord_ObjectValue ( PageCommand );

          this.updateBillingRecord_ItemValue ( PageCommand );

        }//END retrieve new instance of budget.

        this.LogValue ( "BillingOrgId: " + this.Session.BillingOrgId );
        this.LogValue ( "BillingMilestoneId: " + this.Session.BillingMilestoneId );
        this.LogValue ( "BillingSubjectId: " + this.Session.BillingSubjectId );
        //
        // select the rows to be displayed.
        //
        foreach ( EvBillingRecordItem item in this.Session.BillingRecord.BillingItems )
        {
          item.Selected = false;

          if ( this.Session.BillingOrgId == String.Empty
            && this.Session.BillingMilestoneId == String.Empty
            && this.Session.BillingSubjectId == String.Empty )
          {
            item.Selected = true;
            continue;
          }

          if ( this.Session.BillingOrgId == item.OrgId )
          {
            item.Selected = true;
          }
          if ( this.Session.BillingMilestoneId == item.MilestoneId )
          {
            item.Selected = true;
          }
          if ( this.Session.BillingSubjectId == item.SubjectId )
          {
            item.Selected = true;
          }
        }

        this.LogValue ( "Billing Record GenerationDate: "
          + this.Session.BillingRecord.GenerationDate );

        // 
        // return the client ResultData object for the customer.
        // 
        this.getBillingRecord_DataObject ( clientDataObject );

        this.LogMethodEnd ( "getBillingRecordPage" );

        return clientDataObject;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.BillingRecord_Page_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      this.LogMethodEnd ( "getBillingRecordPage" );
      return null;

    }//END getObject method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="ClientDataObject">Evado.Model.UniForm.AppData object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private void getBillingRecord_DataObject (
      Evado.Model.UniForm.AppData ClientDataObject )
    {
      this.LogMethod ( "getBillingRecord_DataObject method. " );
      // 
      // Initialise the methods variables and objects.
      // 
      ClientDataObject.Id = Guid.NewGuid ( );
      ClientDataObject.Title =
        String.Format ( EvLabels.Budget_Page_Page_Title,
        this.Session.BillingRecord.GenerationDate.ToString ( "dd MMM yy" ),
        this.Session.Project.ProjectId,
        this.Session.Project.Title
        );

      ClientDataObject.Page.Id = ClientDataObject.Id;
      ClientDataObject.Page.Title = ClientDataObject.Title;
      ClientDataObject.Page.Status = Evado.Model.UniForm.EditAccess.Enabled;

      //
      // get the page commands.
      //
      this.getBillingRecord_PageCommands ( ClientDataObject.Page );

      this.LogValue ( "PageObject.Status: " + ClientDataObject.Page.Status );

      // 
      // create the page pageMenuGroup
      // 
      this.getBillingRecord_General_Group ( ClientDataObject.Page );

      // 
      // create the budget item group
      // 
      this.getBillingRecord_Item_Group ( ClientDataObject.Page );

      //
      // create the signoff group.
      //
      this.getBillingRecord_SignoffLog_Group ( ClientDataObject.Page );

      this.LogMethodEnd ( "getBillingRecord_DataObject" );

    }//END getBillingRecord_DataObject Method

    //===================================================================================
    /// <summary>
    /// This method generates the relevant page commands for the milestone record page.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object</param>
    //-----------------------------------------------------------------------------------
    private void getBillingRecord_PageCommands (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getObject_PageCommands method. " );
      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.UniForm.Command pageCommand = new Model.UniForm.Command ( );
      PageObject.Status = Evado.Model.UniForm.EditAccess.Enabled;

      //
      // Set the page access and commands based on the user role and the record state.
      //
      switch ( this.Session.BillingRecord.State )
      {
        case EvBillingRecord.BillingStates.Pending:
          {
            if ( this.Session.UserProfile.hasFinanceEditAccess == true )
            {
              PageObject.Status = Evado.Model.UniForm.EditAccess.Enabled;
              // 
              // Add the save budget command.
              // 
              pageCommand = PageObject.addCommand (
                EvLabels.BillingRecord_Save_Command_Title,
                EuAdapter.APPLICATION_ID,
                EuAdapterClasses.Billing_Records.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.Save_Object );

              pageCommand.SetPageId ( EvPageIds.Billing_Record_Page );
              pageCommand.SetGuid ( this.Session.BillingRecord.Guid );
              pageCommand.AddParameter (
                 Evado.Model.Clinical.EvcStatics.CONST_SAVE_ACTION,
                EvBillingRecords.BillingRecordsActions.Save.ToString ( ) );

              //
              // of a mew budger to not display delete or approaval buttons
              //
              if ( this.Session.BillingRecord.Guid != EvStatics.CONST_NEW_OBJECT_ID )
              {
                // 
                // Add the invoiced budget command.
                // 
                pageCommand = PageObject.addCommand (
                  EvLabels.BillingRecord_Invoiced_Command_Title,
                  EuAdapter.APPLICATION_ID,
                  EuAdapterClasses.Billing_Records.ToString ( ),
                  Evado.Model.UniForm.ApplicationMethods.Save_Object );

                pageCommand.SetPageId ( EvPageIds.Billing_Record_Page );
                pageCommand.SetGuid ( this.Session.BillingRecord.Guid );
                pageCommand.AddParameter (
                   Evado.Model.Clinical.EvcStatics.CONST_SAVE_ACTION,
                  EvBillingRecords.BillingRecordsActions.Invoice.ToString ( ) );
              }
            }
            break;
          }
        case EvBillingRecord.BillingStates.Do_Not_Invoice:
          {
            if ( this.Session.UserProfile.hasFinanceEditAccess == true )
            {
              // 
              // Add the invoiced budget command.
              // 
              pageCommand = PageObject.addCommand (
                EvLabels.BillingRecord_Invoiced_Command_Title,
                EuAdapter.APPLICATION_ID,
                EuAdapterClasses.Billing_Records.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.Save_Object );

              pageCommand.SetPageId ( EvPageIds.Billing_Record_Page );
              pageCommand.SetGuid ( this.Session.BillingRecord.Guid );
              pageCommand.AddParameter (
                 Evado.Model.Clinical.EvcStatics.CONST_SAVE_ACTION,
                EvBillingRecords.BillingRecordsActions.Invoice.ToString ( ) );
            }
            break;
          }
        case EvBillingRecord.BillingStates.Invoiced:
          {
            if ( this.Session.UserProfile.hasFinanceEditAccess == true )
            {
              // 
              // Add the invoiced budget command.
              // 
              pageCommand = PageObject.addCommand (
                EvLabels.BillingRecord_Invoiced_Command_Title,
                EuAdapter.APPLICATION_ID,
                EuAdapterClasses.Billing_Records.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.Save_Object );

              pageCommand.SetPageId ( EvPageIds.Billing_Record_Page );
              pageCommand.SetGuid ( this.Session.BillingRecord.Guid );
              pageCommand.AddParameter (
                 Evado.Model.Clinical.EvcStatics.CONST_SAVE_ACTION,
                EvBillingRecords.BillingRecordsActions.Paid.ToString ( ) );
            }
            break;
          }
        default:
          {
            PageObject.Status = Evado.Model.UniForm.EditAccess.Enabled;
            break;
          }
      }//END switch


      if ( this.Session.BillingRecord.Guid != EvStatics.CONST_NEW_OBJECT_ID )
      {
        pageCommand = PageObject.addCommand (
          EvLabels.Billing_Repprt_Page_Command_Title,
          EuAdapter.APPLICATION_ID,
          EuAdapterClasses.Billing_Records.ToString ( ),
          Model.UniForm.ApplicationMethods.Get_Object );


        pageCommand.SetPageId ( EvPageIds.Billing_Report );
      }

      this.LogMethodEnd ( "getObject_PageCommands" );

    }//END getObject_PageCommands method

    //===================================================================================
    /// <summary>
    /// This method generates the relevant page page field for the milestone record page.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object</param>
    //-----------------------------------------------------------------------------------
    private void getBillingRecord_General_Group (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getObject_General_Group method. " );
      this.LogValue ( "PageObject.Status: " + PageObject.Status );

      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      Evado.Model.UniForm.Group pageGroup = new Model.UniForm.Group ( );
      List<EvOption> optionList = new List<EvOption> ( );
      String stOrgList = String.Empty ;
      String stSubjectList = String.Empty ;

      //
      // generate the pageMenuGroup object.
      //
      pageGroup = PageObject.AddGroup (
        EvLabels.Budget_Page_General_Group_Title,
        PageObject.Status );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      //
      // Add the group commands
      //
      this.getBillingRecord_GroupCommands ( pageGroup );

      //
      // Add the invoice reference
      //
      if ( this.Session.BillingRecord.Guid != Evado.Model.Clinical.EvcStatics.CONST_NEW_OBJECT_ID )
      {
        groupField = pageGroup.createTextField (
          EvBillingRecord.BillingRecordFieldNames.InvoiceReference,
          EvLabels.BillingRecord_Invoice_Field_Title,
          this.Session.BillingRecord.InvoiceReference, 20 );
        groupField.Layout = EuFormGenerator.ApplicationFieldLayout;
      }

      optionList.Add ( new EvOption ( ) );
      
      //
      // Iterate through the billing record items to get the current organisations.
      //
      foreach ( EvBillingRecordItem item in this.Session.BillingRecord.BillingItems )
      {
        if ( stOrgList.Contains ( item.OrgId ) == false )
        {
          stOrgList += ";" + item.OrgId;
          var option = this.Session.getSiteOption ( item.OrgId );
          if ( option != null )
          {
            optionList.Add ( option );
          }
        }//END organisaition not in list.
      }//END iteration loop
      
      //
      // add the billing organisation selection list.
      //
      groupField = pageGroup.createSelectionListField (
       EvIdentifiers.ORGANISATION_ID,
       EvLabels.Label_Organisation_Id,
       this.Session.BillingOrgId,
        optionList );

      groupField.AddParameter ( FieldParameterList.Snd_Cmd_On_Change, "1" );

      //
      // Subject selection.
      //
      optionList = new List<EvOption> ( );
      optionList.Add ( new EvOption ( ) );

      //
      // Iterate through the billing record items to get the current organisations.
      //
      foreach ( EvBillingRecordItem item in this.Session.BillingRecord.BillingItems )
      {
        if ( stSubjectList.Contains ( item.SubjectId ) == false )
        {
          stSubjectList += ";" + item.SubjectId;
          optionList.Add ( new EvOption ( item.SubjectId ) );
        }//END subject not in list.
      }//END iteration loop

      //
      // add the billing organisation selection list.
      //
      groupField = pageGroup.createSelectionListField (
       EvIdentifiers.SUBJECT_ID,
       EvLabels.Label_Subject_Id,
       this.Session.BillingSubjectId,
        optionList );

      groupField.AddParameter ( FieldParameterList.Snd_Cmd_On_Change, "1" );

      this.LogMethodEnd ( "getObject_General_Group" );

    }//END getObject_General_Group method.

    //===================================================================================
    /// <summary>
    /// This method generates the relevant page commands for the milestone record page.
    /// </summary>
    /// <param name="PageGroup">Evado.Model.UniForm.Page object</param>
    //-----------------------------------------------------------------------------------
    private void getBillingRecord_GroupCommands (
      Evado.Model.UniForm.Group PageGroup )
    {
      this.LogMethod ( "getBillingRecord_GroupCommands method. " );
      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.UniForm.Command groupCommand = new Model.UniForm.Command ( );


      //
      // Set the page access and commands based on the user role and the record state.
      //
      switch ( this.Session.BillingRecord.State )
      {
        case EvBillingRecord.BillingStates.Pending:
          {
            if ( PageGroup.EditAccess == EditAccess.Enabled )
            {
              // 
              // Add the refresh billing command.
              // 
              groupCommand = PageGroup.addCommand (
                EvLabels.BillingRecord_Refresh_Command_Title,
                EuAdapter.APPLICATION_ID,
                EuAdapterClasses.Billing_Records.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.Custom_Method );

              groupCommand.SetPageId ( EvPageIds.Billing_Record_Page );
              groupCommand.SetGuid ( this.Session.BillingRecord.Guid );
              groupCommand.setCustomMethod ( ApplicationMethods.Get_Object );

              // 
              // Add the save billing command.
              // 
              groupCommand = PageGroup.addCommand (
                EvLabels.BillingRecord_Save_Command_Title,
                EuAdapter.APPLICATION_ID,
                EuAdapterClasses.Billing_Records.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.Save_Object );

              groupCommand.SetPageId ( EvPageIds.Billing_Record_Page );
              groupCommand.SetGuid ( this.Session.BillingRecord.Guid );
              groupCommand.AddParameter (
                 Evado.Model.Clinical.EvcStatics.CONST_SAVE_ACTION,
                EvBillingRecords.BillingRecordsActions.Save.ToString ( ) );

              //
              // of a mew budger to not display delete or approaval buttons
              //
              if ( this.Session.BillingRecord.Guid != EvStatics.CONST_NEW_OBJECT_ID )
              {
                // 
                // Add the invoiced billing command.
                // 
                groupCommand = PageGroup.addCommand (
                  EvLabels.BillingRecord_Invoiced_Command_Title,
                  EuAdapter.APPLICATION_ID,
                  EuAdapterClasses.Billing_Records.ToString ( ),
                  Evado.Model.UniForm.ApplicationMethods.Save_Object );

                groupCommand.SetPageId ( EvPageIds.Billing_Record_Page );
                groupCommand.SetGuid ( this.Session.BillingRecord.Guid );
                groupCommand.AddParameter (
                   Evado.Model.Clinical.EvcStatics.CONST_SAVE_ACTION,
                  EvBillingRecords.BillingRecordsActions.Invoice.ToString ( ) );
              }
            }
            break;
          }
        case EvBillingRecord.BillingStates.Do_Not_Invoice:
          {
            if ( this.Session.UserProfile.hasFinanceEditAccess == true )
            {
              // 
              // Add the invoiced billing command.
              // 
              groupCommand = PageGroup.addCommand (
                EvLabels.BillingRecord_Invoiced_Command_Title,
                EuAdapter.APPLICATION_ID,
                EuAdapterClasses.Billing_Records.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.Save_Object );

              groupCommand.SetPageId ( EvPageIds.Billing_Record_Page );
              groupCommand.SetGuid ( this.Session.BillingRecord.Guid );
              groupCommand.AddParameter (
                 Evado.Model.Clinical.EvcStatics.CONST_SAVE_ACTION,
                EvBillingRecords.BillingRecordsActions.Invoice.ToString ( ) );
            }
            break;
          }
        case EvBillingRecord.BillingStates.Invoiced:
          {
            if ( PageGroup.EditAccess == EditAccess.Enabled )
            {
              // 
              // Add the invoiced billing command.
              // 
              groupCommand = PageGroup.addCommand (
                EvLabels.BillingRecord_Invoiced_Command_Title,
                EuAdapter.APPLICATION_ID,
                EuAdapterClasses.Billing_Records.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.Save_Object );

              groupCommand.SetPageId ( EvPageIds.Billing_Record_Page );
              groupCommand.SetGuid ( this.Session.BillingRecord.Guid );
              groupCommand.AddParameter (
                 Evado.Model.Clinical.EvcStatics.CONST_SAVE_ACTION,
                EvBillingRecords.BillingRecordsActions.Paid.ToString ( ) );
            }
            break;
          }
      }//END switch


      if ( this.Session.BillingRecord.Guid != EvStatics.CONST_NEW_OBJECT_ID )
      {
        groupCommand = PageGroup.addCommand (
          EvLabels.Billing_Repprt_Page_Command_Title,
          EuAdapter.APPLICATION_ID,
          EuAdapterClasses.Billing_Records.ToString ( ),
          Model.UniForm.ApplicationMethods.Get_Object );

        groupCommand.SetPageId ( EvPageIds.Billing_Report );
      }


      this.LogMethodEnd ( "getBillingRecord_GroupCommands" );

    }//END getObject_PageCommands method

    //===================================================================================
    /// <summary>
    /// This method generates the relevant inary files groups for the milestone record page.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object</param>
    //-----------------------------------------------------------------------------------
    private void getBillingRecord_Item_Group (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getBillingRecord_Item_Group method. " );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Field pageField = new Evado.Model.UniForm.Field ( );
      Evado.Model.UniForm.Command groupCommand = new Evado.Model.UniForm.Command ( );

      //
      // create the budget item group 
      //
      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
        EvLabels.BillingRecord_Items_Group_Title,
        PageObject.Status );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      //
      // Add the refresh command.
      //
      if ( pageGroup.EditAccess == EditAccess.Enabled )
      {
        // 
        // Add the refresh billing command.
        // 
        groupCommand = pageGroup.addCommand (
          EvLabels.BillingRecord_Refresh_Command_Title,
          EuAdapter.APPLICATION_ID,
          EuAdapterClasses.Billing_Records.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Custom_Method );

        groupCommand.SetPageId ( EvPageIds.Billing_Record_Page );
        groupCommand.SetGuid ( this.Session.BillingRecord.Guid );
        groupCommand.setCustomMethod ( ApplicationMethods.Get_Object );
      }


      pageField = pageGroup.createTableField (
        CONST_BILLING_ITEM_FIELD_ID, String.Empty, 8 );

      pageField.Table.Header [ 0 ] = new TableColHeader ( );
      pageField.Table.Header [ 0 ].No = 1;
      pageField.Table.Header [ 0 ].Text = EvLabels.BillingRecord_Items_Column_0_Title;
      pageField.Table.Header [ 0 ].TypeId = Evado.Model.UniForm.TableColHeader.ItemTypeText;
      pageField.Table.Header [ 0 ].Width = "2";

      pageField.Table.Header [ 1 ] = new TableColHeader ( );
      pageField.Table.Header [ 1 ].No = 1;
      pageField.Table.Header [ 1 ].Text = EvLabels.BillingRecord_Items_Column_1_Title;
      pageField.Table.Header [ 1 ].TypeId = Evado.Model.UniForm.TableColHeader.ItemTypeReadOnly;
      pageField.Table.Header [ 1 ].Width = "40";

      pageField.Table.Header [ 2 ] = new TableColHeader ( );
      pageField.Table.Header [ 2 ].No = 2;
      pageField.Table.Header [ 2 ].Text = EvLabels.BillingRecord_Items_Column_2_Title;
      pageField.Table.Header [ 2 ].TypeId = Evado.Model.UniForm.TableColHeader.ItemTypeNumeric;
      pageField.Table.Header [ 2 ].Width = "8";

      pageField.Table.Header [ 3 ] = new TableColHeader ( );
      pageField.Table.Header [ 3 ].No = 3;
      pageField.Table.Header [ 3 ].Text = EvLabels.BillingRecord_Items_Column_3_Title;
      pageField.Table.Header [ 3 ].TypeId = Evado.Model.UniForm.TableColHeader.ItemTypeNumeric;
      pageField.Table.Header [ 3 ].Width = "8";

      pageField.Table.Header [ 4 ] = new TableColHeader ( );
      pageField.Table.Header [ 4 ].No = 4;
      pageField.Table.Header [ 4 ].Text = EvLabels.BillingRecord_Items_Column_4_Title;
      pageField.Table.Header [ 4 ].TypeId = Evado.Model.UniForm.TableColHeader.ItemTypeReadOnly;
      pageField.Table.Header [ 4 ].Width = "8";

      pageField.Table.Header [ 5 ] = new TableColHeader ( );
      pageField.Table.Header [ 5 ].No = 5;
      pageField.Table.Header [ 5 ].Text = EvLabels.BillingRecord_Items_Column_5_Title;
      pageField.Table.Header [ 5 ].TypeId = Evado.Model.UniForm.TableColHeader.ItemTypeYesNo;
      pageField.Table.Header [ 5 ].Width = "8";

      pageField.Table.Header [ 6 ] = new TableColHeader ( );
      pageField.Table.Header [ 6 ].No = 6;
      pageField.Table.Header [ 6 ].Text = EvLabels.BillingRecord_Items_Column_6_Title;
      pageField.Table.Header [ 6 ].TypeId = Evado.Model.UniForm.TableColHeader.ItemTypeYesNo;
      pageField.Table.Header [ 6 ].Width = "8";

      pageField.Table.Header [ 7 ] = new TableColHeader ( );
      pageField.Table.Header [ 7 ].No = 7;
      pageField.Table.Header [ 7 ].Text = EvLabels.BillingRecord_Items_Column_7_Title;
      pageField.Table.Header [ 7 ].TypeId = Evado.Model.UniForm.TableColHeader.ItemTypeReadOnly;
      pageField.Table.Header [ 7 ].Width = "15";

      //
      // Iterate through the list of budget items.
      //
      for ( int count=0; count< this.Session.BillingRecord.BillingItems.Count; count++ )
      {
        EvBillingRecordItem item = this.Session.BillingRecord.BillingItems[ count];
        //
        // Only display the selection items.
        //
        if ( item.Selected == false )
        {
          continue; 
        }

        Evado.Model.UniForm.TableRow row = new TableRow ( );
        row.No = count;
        row.Column [ 0 ] = (count+ 1 ).ToString ( );
        row.Column [ 1 ] = item.Details;
        row.Column [ 2 ] = item.UnitPrice.ToString ( );
        row.Column [ 3 ] = item.Quantity.ToString ( );
        row.Column [ 4 ] = item.TotalPrice.ToString ( );
        row.Column [ 5 ] = item.DoNoInvoice.ToString ( );
        row.Column [ 6 ] = item.Selected.ToString ( );
        row.Column [ 7 ] = item.State_Desc;


        pageField.Table.Rows.Add ( row );

      }//END budget item iteration loop

      this.LogMethodEnd ( "getBillingRecord_Item_Group" );

    }//END getObject_BudgetItem_Group method

    // ==============================================================================
    /// <summary>
    /// This method creates the project signoff pageMenuGroup
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page object object.</param>
    // ------------------------------------------------------------------------------
    private void getBillingRecord_SignoffLog_Group (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getClientDataObject_SignoffLog method. " );

      // 
      // Display comments it they exist.
      // 
      if ( this.Session.BillingRecord.Signoffs.Count == 0 )
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
        EvUserSignoff.getSignoffLog ( this.Session.BillingRecord.Signoffs, false ) );
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;

    }//END getObject_SignoffLog_Group Method

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
    private Evado.Model.UniForm.AppData getBillingRecordReportPage (
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
      if ( this.Session.BillingRecord.Guid == Guid.Empty )
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
          this.Session.BillingRecord.RecordId,
          this.Session.Project.ProjectId,
          this.Session.Project.Title );

        clientDataObject.Page.Id = clientDataObject.Id;
        clientDataObject.Page.Title = clientDataObject.Title;
        clientDataObject.Page.Status = Evado.Model.UniForm.EditAccess.Enabled;

        if ( this.Session.UserProfile.hasFinanceEditAccess == true
          && this.Session.BillingRecord.State == EvBillingRecord.BillingStates.Pending )
        {
          clientDataObject.Page.Status = Evado.Model.UniForm.EditAccess.Enabled;
          //
          // add the page refresh command
          //
          pageCommand = clientDataObject.Page.addCommand (
            EvLabels.Budget_Repprt_Page_Refresh_Command_Title,
            EuAdapter.APPLICATION_ID,
            EuAdapterClasses.Billing_Records.ToString ( ),
            Model.UniForm.ApplicationMethods.Custom_Method );

          pageCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.Get_Object );

          pageCommand.SetPageId ( EvPageIds.Budget_Report_Page );

          // 
          // Add the save budget command.
          // 
          pageCommand = clientDataObject.Page.addCommand (
            EvLabels.Budget_Save_Command_Title,
            EuAdapter.APPLICATION_ID,
            EuAdapterClasses.Billing_Records.ToString ( ),
            Evado.Model.UniForm.ApplicationMethods.Save_Object );

          pageCommand.SetPageId ( EvPageIds.Billing_Record_Page );
          pageCommand.SetGuid ( this.Session.BillingRecord.Guid );
          pageCommand.AddParameter (
             Evado.Model.Clinical.EvcStatics.CONST_SAVE_ACTION,
            EvBillingRecords.BillingRecordsActions.Save.ToString ( ) );
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
      Evado.Bll.Clinical.EvReports bll_Reports = new EvReports ( );
      Evado.Bll.EvStaticSetting.DebugOn = true;

      //
      // if the page command has a custom method then the page is to be refreshed.
      //
      if ( PageCommand.hasParameter ( Model.UniForm.CommandParameters.Custom_Method ) == true )
      {
        this.LogValue ( "Refresh the report" );
        return true;
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
      /*
      
      this._Report.Queries [ 0 ].Value = this.pvBillingRecordGuid.Text;
      this._Report.Queries [ 1 ].Value = this.pvTrialId.Text;
      this._Report.Queries [ 2 ].Value = this.pvBillingRptId.Text;
       */
      for ( int i = 0; i < this._BillingReportTemplate.Queries.Length; i++ )
      {
        if ( this._BillingReportTemplate.Queries [ i ].QueryId == "BillingRecordGuid" )
        {
          this._BillingReportTemplate.Queries [ i ].Value = this.Session.BillingRecord.Guid.ToString ( );
        }
        if ( this._BillingReportTemplate.Queries [ i ].QueryId == "BillingReportId" )
        {
          this._BillingReportTemplate.Queries [ i ].Value = this.Session.BillingRecord.RecordId.ToString ( );
        }
        if ( this._BillingReportTemplate.Queries [ i ].QueryId == "TrialId" )
        {
          this._BillingReportTemplate.Queries [ i ].Value = this.Session.BillingRecord.ProjectId;
        }
      }

      //
      // Generate the budget report objects.
      //
      try
      {
        this.Session.Report = bll_Reports.getReport (
          this._BillingReportTemplate );

        this.LogValue ( "budgets Log: " + bll_Reports.Log );
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
      this._BillingReportTemplate = new EvReport ( );

      //
      // Get the budget report template.
      //
      List<EvReport> budgetReport = reportTemplates.getReportList (
      String.Empty,
      EvReport.ReportTypeCode.Null,
      String.Empty,
      EvReport.ReportScopeTypes.Billing_Summary );

      this.LogValue ( "ReportTemplate Log: " + reportTemplates.Log );

      if ( budgetReport.Count != 1 )
      {
        this.ErrorMessage = String.Format (
          EvLabels.Budget_Report_Duplicate_Error_Message,
          budgetReport.Count );
        // 
        this.LogError (EvEventCodes.Business_Logic_General_Process_Error,
          this.ErrorMessage );

        return false;
      }

      //
      // Update the report object with first instance of the report template list.
      //
      this._BillingReportTemplate = reportTemplates.getReport ( budgetReport [ 0 ].Guid );

      //
      // If the template is empty then the report template is not defined.
      //
      if ( this._BillingReportTemplate.Guid == Guid.Empty )
      {
        this.ErrorMessage = EvLabels.Budget_Report_Template_Empty_Error_Message;
        // 
        this.LogError( EvEventCodes.Business_Logic_General_Process_Error,
          this.ErrorMessage );

        return false;
      }

      this.LogValue ( "ReportTemplate.Title: " + this._BillingReportTemplate.ReportTitle );
      this.LogValue ( "ReportTemplate.SqlDataSource: " + this._BillingReportTemplate.SqlDataSource );
      foreach ( EvReportQuery query in this._BillingReportTemplate.Queries )
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
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "createObject",
          this.Session.UserProfile );

        // 
        // Initialise the methods variables and objects.
        //      
        Evado.Model.UniForm.AppData data = new Evado.Model.UniForm.AppData ( );
        this.Session.BillingRecord = new EvBillingRecord ( );
        this.Session.BillingRecord.Guid = Evado.Model.Clinical.EvcStatics.CONST_NEW_OBJECT_ID;

        this.Session.BillingRecord.ProjectId = this.Session.Project.ProjectId;
        this.Session.BillingRecord.State = EvBillingRecord.BillingStates.Null;

        //
        // get the list of billing items that have not been allocated.
        //
        this.Session.BillingRecord.BillingItems = this._Bll_BillingRecords.getItemListForBillingRecord (
          this.Session.BillingRecord.ProjectId,
          Guid.Empty );

        this.LogClass ( this._Bll_BillingRecords.DebugLog );

        this.getBillingRecord_DataObject ( data );


        this.LogValue ( "Exit createObject method. ID: " + data.Id + ", Title: " + data.Title );

        return data;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = "Error raised when creating a billing record.";

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
    private Evado.Model.UniForm.AppData updateBillinRecord_Object (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "updateBillingRecord_Object method. " );
      this.LogValue ( "PageCommand: " + PageCommand.getAsString ( false, true ) );

      this.LogValue ( "Budget.Guid: " + this.Session.BillingRecord.Guid );
      this.LogValue ( "Budget.ProjectId: " + this.Session.BillingRecord.ProjectId );
      this.LogValue ( "PageCommand Guid: " + PageCommand.GetGuid ( ) );
      try
      {
        //
        // Initialise the methods variables and objects.
        //
        EvBillingRecords.BillingRecordsActions saveAction = EvBillingRecords.BillingRecordsActions.Save;

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "updateObject",
          this.Session.UserProfile );

        // 
        // Delete the object.
        // 
        if ( PageCommand.Method == Evado.Model.UniForm.ApplicationMethods.Delete_Object )
        {
          return this.Session.LastPage;
        }

        // 
        // Get the save action value.
        // 
        String stSaveAction = PageCommand.GetParameter ( Evado.Model.Clinical.EvcStatics.CONST_SAVE_ACTION );
        if ( stSaveAction != String.Empty )
        {
          saveAction =
            EvStatics.Enumerations.parseEnumValue<EvBillingRecords.BillingRecordsActions> ( stSaveAction );
        }
        this.LogValue ( "this.SessionObjects.BillingRecord.SaveAction: " + saveAction );

        //
        // if the guid is set to new object reset it to empty to add a new record.
        //
        if ( this.Session.BillingRecord.Guid == Evado.Model.Clinical.EvcStatics.CONST_NEW_OBJECT_ID )
        {
          this.Session.BillingRecord.Guid = Guid.Empty;
          this.Session.BillingRecord.ProjectId = this.Session.Project.ProjectId;
          saveAction = EvBillingRecords.BillingRecordsActions.Save;
        }

        // 
        // Initialise the update variables.
        // 
        this.Session.BillingRecord.UpdatedByUserId = this.Session.UserProfile.UserId;
        this.Session.BillingRecord.UpdatedBy = this.Session.UserProfile.CommonName;
        this.Session.BillingRecord.UpdatedDate = DateTime.Now;

        // 
        // Update the object.
        // 
        if ( this.updateBillingRecord_ObjectValue ( PageCommand ) == false )
        {
          this.ErrorMessage = EvLabels.Billing_Update_Error_Message;

          return this.Session.LastPage;
        }

        if ( this.updateBillingRecord_ItemValue ( PageCommand ) == false )
        {
          this.ErrorMessage = EvLabels.Billing_Update_Error_Message;

          return this.Session.LastPage;
        }

        // 
        // update the object.
        // 
        EvEventCodes result = this._Bll_BillingRecords.saveBillingRecord ( this.Session.BillingRecord, saveAction );

        // 
        // get the debug ResultData.
        // 
        this.LogValue ( this._Bll_BillingRecords.DebugLog );

        // 
        // if an error state is returned create log the event.
        // 
        if ( result != EvEventCodes.Ok )
        {
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
        this.Session.BillingRecord = new EvBillingRecord ( );
        this.Session.BillingRecordList = new List<EvBillingRecord> ( );

        return new AppData ( ); 

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Billing_Update_Error_Message;

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
    private bool updateBillingRecord_ObjectValue ( Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "updateBillingRecord_ObjectValue method. " );
      this.LogValue ( "Parameters.Count: " + PageCommand.Parameters.Count );
      this.LogValue ( "Budget.Guid: " + this.Session.BillingRecord.Guid );

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
          || parameter.Name.Contains( EuBillingRecords.CONST_BILLING_ITEM_FIELD_ID ) == true 
          || parameter.Name == EvIdentifiers.SUBJECT_ID
          || parameter.Name == EvIdentifiers.MILESTONE_ID
          || parameter.Name == EvIdentifiers.ORGANISATION_ID )
        {
          continue;
        }

        this.LogValue ( parameter.Name + " > " + parameter.Value + " >> UPDATED" );
        try
        {
          EvBillingRecord.BillingRecordFieldNames fieldName = Evado.Model.Clinical.EvcStatics.Enumerations.parseEnumValue<EvBillingRecord.BillingRecordFieldNames> ( parameter.Name );

          this.Session.BillingRecord.setValue ( fieldName, parameter.Value );
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

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="Parameters">List of field values to be updated.</param>
    /// <returns></returns>
    //  ----------------------------------------------------------------------------------
    private bool updateBillingRecord_ItemValue ( Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "updateBillingRecord_ItemValue method. " );
      this.LogValue ( "Parameters.Count: " + PageCommand.Parameters.Count );
      //
      // iterator through the rows.
      //
      for ( int row = 0; row < this.Session.BillingRecord.BillingItems.Count; row++ )
      {
        int rowIndex = row+1;
        string columnFieldId = EuBillingRecords.CONST_BILLING_ITEM_FIELD_ID + "_" + rowIndex + "_1";

        string value = PageCommand.GetParameter ( columnFieldId );

        this.LogValue ( "ParmId: " + columnFieldId + " Value: " + value );

        if ( value != String.Empty )
        {
          int itemIndex = int.Parse( value );
          itemIndex--;
          EvBillingRecordItem item = this.Session.BillingRecord.BillingItems [ itemIndex ];

          this.LogValue ( "Item: " + item.Details );

          //
          // Process unit price column value.
          //

          columnFieldId = EuBillingRecords.CONST_BILLING_ITEM_FIELD_ID + "_" + rowIndex + "_3";

          value = PageCommand.GetParameter ( columnFieldId );

          this.LogValue ( "ParmId: " + columnFieldId + " Value: " + value );

          float fltValue = float.Parse( value );

          item.UnitPrice = fltValue;

          //
          // Process quanity column value.
          //
          columnFieldId = EuBillingRecords.CONST_BILLING_ITEM_FIELD_ID + "_" + rowIndex + "_4";

          value = PageCommand.GetParameter ( columnFieldId );

          this.LogValue ( "ParmId: " + columnFieldId + " Value: " + value );

          fltValue = float.Parse( value );

          item.Quantity = fltValue;

          item.TotalPrice = item.UnitPrice * item.Quantity;

          //
          // Process do not invice column value.
          //
          columnFieldId = EuBillingRecords.CONST_BILLING_ITEM_FIELD_ID + "_" + rowIndex + "_6";

          value = PageCommand.GetParameter ( columnFieldId );

          this.LogValue ( "ParmId: " + columnFieldId + " Value: " + value );

          bool bValue = EvStatics.getBool( value );

          item.DoNoInvoice = bValue;

          //
          // Process do Select column value.
          //
          columnFieldId = EuBillingRecords.CONST_BILLING_ITEM_FIELD_ID + "_" + rowIndex + "_7";

          value = PageCommand.GetParameter ( columnFieldId );

          this.LogValue ( "ParmId: " + columnFieldId + " Value: " + value );

          bValue = EvStatics.getBool ( value );

          item.Selected = bValue;
        }

      }


      return true;
    }

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
  }
}//END namespace