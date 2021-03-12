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
  public class EuReports : EuClassAdapterBase
  {
    #region Class Initialisation
    /// <summary>
    /// This method initialises the class.
    /// </summary>
    public EuReports ( )
    {
      this.ClassNameSpace = " Evado.UniForm.Clinical.EuReports.";
    }

    /// <summary>
    /// This method initialises the class and passs in the user profile.
    /// </summary>
    public EuReports (
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
      this.ClassNameSpace = " Evado.UniForm.Clinical.EuReports.";

      this.LogInitMethod ( "EuReports initialisation" );
      this.LogInit ( "ServiceUserProfile.UserId: " + ServiceUserProfile.UserId );
      this.LogInit ( "Session.UserProfile.UserId: " + this.Session.UserProfile.UserId );
      this.LogInit ( "Session.UserProfile.CommonName: " + this.Session.UserProfile.CommonName );
      this.LogInit ( "UniForm BinaryFilePath: " + this.UniForm_BinaryFilePath );
      this.LogInit ( "UniForm BinaryServiceUrl: " + this.UniForm_BinaryServiceUrl );

      this._Bll_Reports = new Evado.Bll.Digital.EvReports ( this.ClassParameters );
      this._Bll_ReportTemplates = new Evado.Bll.Digital.EvReportTemplates ( this.ClassParameters );

      //
      // Initialise the report object.
      //
      if ( this.Session.Report == null )
      {
        this.Session.Report = new EvReport ( );
      }
      if ( this.Session.ReportTemplateList == null )
      {
        this.Session.ReportTemplateList = new List<EvReport> ( );
      }

    }//END Method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class constants and variables.

    private Evado.Bll.Digital.EvReports _Bll_Reports = new Evado.Bll.Digital.EvReports ( );
    private Evado.Bll.Digital.EvReportTemplates _Bll_ReportTemplates = new Evado.Bll.Digital.EvReportTemplates ( );
    private bool _OutputReport = false;
    //
    // Initialise the page labels
    //
    private const string CONST_Report_SCOPE = "RSCP";
    private const string CONST_Report_TYPE = "RTYP";
    private const string CONST_Report_CATEGORY = "RCAT";
    private const string CONST_QUERY_FIELD_ID = "RQFV_";
    private const string CONST_SAVE_REPORT = "RPTS";

    private const string CONST_NAME_SPACE = "Evado.UniForm.Clinical.EuReports.";
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
      this.LogValue ( "PageCommand " + PageCommand.getAsString ( false, false ) );

      try
      {
        // 
        // Initialise the methods variables and objects.
        // 
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );

        // 
        // Set the page type to control the DB query type.
        // 
        string pageType = PageCommand.GetParameter (
            Evado.Model.UniForm.CommandParameters.Page_Id );

        this.LogValue ( "pageType: " + pageType );

        this.Session.setPageId ( pageType );

        this.LogValue ( "PageType: " + this.Session.PageId );
        // 
        // Determine the method to be called
        // 
        switch ( PageCommand.Method )
        {
          case Evado.Model.UniForm.ApplicationMethods.List_of_Objects:
            {
              clientDataObject = this.getReports_ListObject ( PageCommand );
              break;
            }
          case Evado.Model.UniForm.ApplicationMethods.Get_Object:
            {
              clientDataObject = this.getReport_Object ( PageCommand );
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
              //
              // There is not update process.
              //
              clientDataObject = this.updateObject ( PageCommand );
              clientDataObject = this.Session.LastPage;
              break;
            }

        }//END Switch

        // 
        // if a null value is returned, display the last page.
        // 
        if ( clientDataObject == null )
        {
          this.LogValue ( " null application data returned." );

          clientDataObject = this.Session.LastPage;
        }

        //
        // Append the error message if it exists.
        //
        if ( this.ErrorMessage != String.Empty )
        {
          clientDataObject.Message = this.ErrorMessage;
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
      return this.Session.LastPage;

    }//END getClientDataObject method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class default list methods

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData getReports_ListObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getReports_ListObject" );
      try
      {
        // 
        // Initialise the methods variables and objects.
        //      
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
        Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );

        //
        // Determine if the user has access to this page and log and error if they do not.
        //
        if ( this.Session.UserProfile.hasManagementAccess == false )
        {
          this.LogIllegalAccess (
           this.ClassNameSpace + "getReports_ListObject",
            this.Session.UserProfile );

          this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

          return this.Session.LastPage;
        }

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
         this.ClassNameSpace + "getReports_ListObject",
          this.Session.UserProfile );

        //
        // Update the selection values.
        //
        this.getReports_UpdateSelection ( PageCommand );

        //
        // Initialise the client ResultData object.
        //
        clientDataObject.Id = Guid.NewGuid ( );
        clientDataObject.Title = EdLabels.Reports_Operational_View_Page_Title;

        switch ( this.Session.PageId )
        {
          case EvPageIds.Data_Management_Report_List:
            {
              clientDataObject.Title = EdLabels.Reports_Data_Management_List_Page_Title;
              this.Session.ReportScope = EvReport.ReportScopeTypes.Data_Management_Reports;
              break;
            }
          case EvPageIds.Monitoring_Report_List:
            {
              clientDataObject.Title = EdLabels.Reports_Monitoring_List_Page_Title;
              this.Session.ReportScope = EvReport.ReportScopeTypes.Monitoring_Reports;
              break;
            }
          /*
              case EvPageIds.Financial_Report_List:
                {
                  clientDataObject.Title = EdLabels.Reports_Financial_List_Page_Title;
                  this.Session.ReportScope = EvReport.ReportScopeTypes.Finance_Reports;
                  break;
                }
            */
          case EvPageIds.Site_Report_List:
            {
              clientDataObject.Title = EdLabels.Reports_Site_List_Page_Title;
              this.Session.ReportScope = EvReport.ReportScopeTypes.Site_Reports;
              break;
            }
          case EvPageIds.Operational_Report_List:
          default:
            {
              clientDataObject.Title = EdLabels.Reports_Operational_View_Page_Title;
              this.Session.ReportScope = EvReport.ReportScopeTypes.Operational_Reports;
              break;
            }
        }//END PageId switch

        this.LogValue ( "Set ReportScope: " + this.Session.ReportScope );

        clientDataObject.Page.Title = clientDataObject.Title;

        //
        // Display the report selection list.
        //
        this.getReports_List_Selection_Group ( clientDataObject.Page );

        // 
        // Add the trial organisation list to the page.
        // 
        this.getReports_List_Group ( clientDataObject.Page );

        this.LogValue ( "data.Page.Title: " + clientDataObject.Page.Title );

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

      return this.Session.LastPage;

    }//END getListObject method.

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private void getReports_UpdateSelection (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getReports_UpdateSelection" );
      //
      // Define method variables and objects.
      //
      String parameterValue = String.Empty;

      this.Session.ReportScope = EvReport.ReportScopeTypes.Operational_Reports;

      this.Session.ReportType = PageCommand.GetParameter<EvReport.ReportTypeCode> ( EuReports.CONST_Report_TYPE );

      this.LogValue ( "ReportType: " + this.Session.ReportType );
      this.LogValue ( "ReportScope: " + this.Session.ReportScope );

      this.LogMethodEnd ( "getReports_UpdateSelection" );
    }//END update selection values.

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private void getReports_List_Selection_Group (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getReports_List_Selection_Group" );
      //
      // Initialise method variables and objects.
      Evado.Model.UniForm.Field groupField = new Model.UniForm.Field ( );
      Evado.Model.UniForm.Command groupCommand = new Model.UniForm.Command ( );
      List<EvOption> optionList = new List<EvOption> ( );

      // 
      // Create the new pageMenuGroup.
      // 
      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
        EdLabels.Reports_Selection_Group_Title,
        String.Empty,
        Evado.Model.UniForm.EditAccess.Enabled );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      groupField.Layout = EuAdapter.DefaultFieldLayout;
      groupField.AddParameter ( Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );

    }//END getReports_List_Selection_Group method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private void getReports_List_Group (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getReports_List_Group" );
      try
      {
        // 
        // Create the new pageMenuGroup.
        // 
        Evado.Model.UniForm.Group pageGroup = Page.AddGroup (
          EdLabels.Reports_List_Group_Title,
          String.Empty,
          Evado.Model.UniForm.EditAccess.Inherited );
        pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
        pageGroup.CmdLayout = Evado.Model.UniForm.GroupCommandListLayouts.Vertical_Orientation;

        // 
        // Query and database.
        // 
        this.Session.ReportTemplateList = this._Bll_ReportTemplates.getReportList (
          EvReport.ReportTypeCode.Null,
          this.Session.ReportScope );

        this.LogValue ( this._Bll_Reports.Log );

        // 
        // bind output to the datagrid.
        // 
        if ( this.Session.ReportTemplateList.Count == 0 )  // If nothing returned create a blank row.
        {
          this.Session.ReportTemplateList.Add ( new EvReport ( ) );
        }

        // 
        // generate the page links.
        // 
        foreach ( EvReport report in this.Session.ReportTemplateList )
        {
          String stReportTitle = report.ReportTitle + EdLabels.Space_Arrow_Right + report.ReportTitle;

          Evado.Model.UniForm.Command command = pageGroup.addCommand ( stReportTitle,
            EuAdapter.ADAPTER_ID,
            EuAdapterClasses.Reports.ToString ( ),
            Model.UniForm.ApplicationMethods.Get_Object );

          command.AddParameter ( Model.UniForm.CommandParameters.Page_Id,
             Evado.Model.Digital.EvPageIds.Operational_Report_Page );

          command.AddParameter ( Model.UniForm.CommandParameters.Short_Title,
            report.ReportId );

          command.SetGuid ( report.Guid );

        }//END trial Report list iteration loop

        this.LogValue ( "command object count: " + pageGroup.CommandList.Count );

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
    private Evado.Model.UniForm.AppData getReport_Object (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getReport_Object" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
      Guid reportTemplateGuid = Guid.Empty;
      String parameterValue = String.Empty;

      //
      // Determine if the user has access to this page and log and error if they do not.
      //
      if ( this.Session.UserProfile.hasManagementAccess == false
        && this.Session.UserProfile.hasManagementAccess == false )
      {
        this.LogIllegalAccess (
         this.ClassNameSpace + "getObject",
          this.Session.UserProfile );

        this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

        return this.Session.LastPage;
      }

      // 
      // Log access to page.
      // 
      this.LogPageAccess (
       this.ClassNameSpace + "getObject",
        this.Session.UserProfile );

      // 
      // if the parameter value exists then set the customerId
      // 
      reportTemplateGuid = PageCommand.GetGuid ( );
      this.LogValue ( "Report template Guid: " + reportTemplateGuid );
      this.LogValue ( "Report.Guid: " + this.Session.Report.Guid );

      // 
      // return if not trial id
      // 
      if ( reportTemplateGuid == Guid.Empty )
      {
        this.LogValue ( "REPORT TEMPLATE GUID IS EMPTY" );
        return this.Session.LastPage;
      }

      parameterValue = PageCommand.GetParameter ( EuReports.CONST_SAVE_REPORT );

      this._OutputReport = Evado.Model.Digital.EvcStatics.getBool ( parameterValue );
      this.LogValue ( "OutputReport: " + this._OutputReport );

      try
      {
        this.getReportTemplate ( reportTemplateGuid );

        //
        // update the reports query values.
        //
        bool parameterValueChanged = this.getReportUpdateQueryValues ( PageCommand );

        // 
        // Retrieve the customer object from the database via the DAL and BLL layers.
        // 
        if ( parameterValueChanged == true )
        {
          this.LogValue ( "parameterValueChanged true" );
          this.getReport_Execute_Query ( );
        }

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

      return this.Session.LastPage;

    }//END getObject method

    // ==============================================================================
    /// <summary>
    /// This method retrieves the report template.
    /// </summary>
    /// <param name="reportTemplateGuid">Guid Template unique identifier</param>
    //  ------------------------------------------------------------------------------
    private void getReportTemplate ( Guid reportTemplateGuid )
    {
      this.LogMethod ( "getReportTemplate" );


      if ( this.Session.Report.Guid == reportTemplateGuid )
      {
        this.LogValue ( "EXIT: template loaded." );
        this.LogMethodEnd ( "getReportTemplate" );
        return;
      }

      this.Session.Report = this._Bll_ReportTemplates.getReport ( reportTemplateGuid );

      this.LogValue ( this._Bll_ReportTemplates.Log );

      this.Session.Report.DataRecords = new List<EvReportRow> ( );

      this.LogValue ( "SessionObjects.Report.ReportId: " + this.Session.Report.ReportId );
      this.LogMethodEnd ( "getReportTemplate" );
    }

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private void getReport_Execute_Query ( )
    {
      this.LogMethod ( "getReport_Exexute_Query" );
      this.Session.Report.DataRecords = new List<EvReportRow> ( );

      this.Session.Report = this._Bll_Reports.getReport ( this.Session.Report );

      this.LogValue ( this._Bll_Reports.Log );

      this.Session.Report.GeneratedBy =
        this.Session.UserProfile.CommonName + " on " + DateTime.Now.ToString ( "dd-MMM-yyyy HH:mm" ); 

      this.LogValue ( "SessionObjects.Report.ReportId: " + this.Session.Report.ReportId );
      this.LogValue ( "Report data record count: " + this.Session.Report.DataRecords.Count );
    }

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>bool: Mandatory Parameters present.</returns>
    //  ------------------------------------------------------------------------------
    private bool getReportUpdateQueryValues (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getReportUpdateQueryValues" );
      this.LogDebug ( "Queries.Length {0}.", this.Session.Report.Queries.Length );
      //
      // Define method variables and objects.
      //
      String parameterValue = String.Empty;
      bool parametersChanged = false;

      //
      // If query array is empty exit true.
      //
      if ( this.Session.Report.Queries.Length == 0 )
      {
        this.LogDebug ( "No Query Values return true." );
        this.LogMethodEnd ( "getReportUpdateQueryValues" );
        return true;
      }

      if ( this.Session.Report.Queries [ 0 ].QueryId == String.Empty )
      {
        this.LogValue ( "No Query Values return true." );
        this.LogMethodEnd ( "getReportUpdateQueryValues" );
        return true;
      }

      //
      // Iterate through the array of query objects.
      //
      for ( int index = 0; index < this.Session.Report.Queries.Length; index++ )
      {
        String fieldId = EuReports.CONST_QUERY_FIELD_ID + index.ToString ( );
        EvReportQuery query = this.Session.Report.Queries [ index ];

        this.LogValue ( "Query: " + index + " Prompt: " + query.Prompt
          + " value name: " + query.ValueName
          + ", Value: " + query.Value
          + ", SelectionSource: " + query.SelectionSource );

        parameterValue = PageCommand.GetParameter ( fieldId );

        if ( parameterValue != query.Value )
        {
          query.Value = parameterValue;
          parametersChanged = true;
        }

        this.LogValue ( "Query Value " + index + ": " + query.Value );

      }//END query object iteration loop

      this.LogValue ( "parametersChanged: " + parametersChanged );

      this.LogMethodEnd ( "getReportUpdateQueryValues" );
      return parametersChanged;

    }//END update selection values.

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

      //
      // Initialise the client ResultData object.
      //
      ClientDataObject.Id = this.Session.Report.Guid;
      ClientDataObject.Title = EdLabels.Reports_Operational_Page_Title;

      switch ( this.Session.Report.ReportScope )
      {
        case EvReport.ReportScopeTypes.Data_Management_Reports:
          {
            ClientDataObject.Title = EdLabels.Reports_Data_Management_Page_Title;
            break;
          }
        case EvReport.ReportScopeTypes.Site_Reports:
          {
            ClientDataObject.Title = EdLabels.Reports_Data_Management_Page_Title;
            break;
          }
        case EvReport.ReportScopeTypes.Monitoring_Reports:
          {
            ClientDataObject.Title = EdLabels.Reports_Monitoring_Page_Title;
            break;
          }
        /*
      case EvReport.ReportScopeTypes.Finance_Reports:
        {
          ClientDataObject.Title = EdLabels.Reports_Finance_Page_Title;
          break;
        }
         */
        case EvReport.ReportScopeTypes.Operational_Reports:
        default:
          {
            ClientDataObject.Title = EdLabels.Reports_Operational_Page_Title;
            break;
          }
      }

      ClientDataObject.Page.Id = ClientDataObject.Id;
      ClientDataObject.Page.Title = ClientDataObject.Title;
      ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      //
      // add the page commands
      //
      this.getClientData_PageCommands ( ClientDataObject.Page );

      //
      // create the report output pageMenuGroup 
      //
      this.getReport_Output_Group ( ClientDataObject.Page );

      //
      // Set the query selection for the report.
      //
      this.getReport_Query_Selection_Group ( ClientDataObject.Page );

      //
      // Display the report output.
      //
      this.getReport_Display_Group ( ClientDataObject.Page );

      // 
      // Save the session ResultData so it is available for the next user generated groupCommand.
      // 


    }//END getClientDataObject Method

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

      // 
      // Add the generate report groupCommand
      // 
      pageCommand = PageObject.addCommand (
        EdLabels.Reports_Generate_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Reports.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Custom_Method );

      pageCommand.SetGuid ( this.Session.Report.Guid );

      pageCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.Get_Object );

      //
      // Insert the download groupCommand
      //
      pageCommand = PageObject.addCommand (
        EdLabels.Reports_Output_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Reports.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Custom_Method );

      pageCommand.SetGuid ( this.Session.Report.Guid );
      pageCommand.AddParameter ( EuReports.CONST_SAVE_REPORT, "1" );

      pageCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.Get_Object );

      this.LogMethodEnd ( "getClientData_GroupCommand" );
    }//END getClientData_PageCommands method.

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private void getReport_Query_Selection_Group (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getReport_Query_Selection_Group" );
      //
      // Initialise method variables and objects.
      //
      Evado.Model.UniForm.Field groupField = new Model.UniForm.Field ( );
      Evado.Model.UniForm.Command groupCommand = new Model.UniForm.Command ( );
      List<EvOption> optionList = new List<EvOption> ( );
      DateTime dValue = Evado.Model.Digital.EvcStatics.CONST_DATE_NULL;
      bool displayGenerateCommand = false;

      // 
      // Create the new pageMenuGroup.
      // 
      Evado.Model.UniForm.Group pageGroup = Page.AddGroup (
        EdLabels.Reports_Query_Selection_Group_Title,
        Evado.Model.UniForm.EditAccess.Enabled );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      //
      // Iterate through the report queries setting displaying the relevant query selection fields.
      //
      for ( int i = 0; i < this.Session.Report.Queries.Length; i++ )
      {
        EvReportQuery query = this.Session.Report.Queries [ i ];

        //
        // skip null or empty queries.
        //
        if ( query == null
          || query.Prompt == string.Empty )
        {
          continue;
        }

        //
        //AFC July 01 2010 Hide the query if it is type hidden.
        //
        if ( query.DataType == EvReport.DataTypes.Hidden )
        {
          continue;
        }

        //
        //If the selection source is empty means that the query should be a lowerText box.
        //else the query should be a drop down list.
        //
        if ( query.hasSelectionSource ( ) == false )
        {
          switch ( query.DataType )
          {
            case EvReport.DataTypes.Date:
              {
                dValue = Evado.Model.Digital.EvcStatics.getDateTime ( query.Value );

                groupField = pageGroup.createDateField (
                  EuReports.CONST_QUERY_FIELD_ID + i,
                  query.Prompt + " " + query.getOperatorString ( ),
                  dValue );
                break;
              }
            default:
              {
                groupField = pageGroup.createTextField ( EuReports.CONST_QUERY_FIELD_ID + i,
                  query.Prompt,
                  query.Value,
                  50 );
                break;
              }
          }//END ResultData type switch.

        }//END none source query field.
        else
        {
          optionList = Evado.Model.Digital.EvcStatics.getArrayAsList<EvOption> ( this.getQuerySelectionList ( query ) );

          groupField = pageGroup.createSelectionListField (
            EuReports.CONST_QUERY_FIELD_ID + i,
            query.Prompt + " " + query.getOperatorString ( ),
            query.Value,
            optionList );

        }//End source selection list.

        if ( query.Mandatory == true )
        {
          groupField.Mandatory = true;
        }
        displayGenerateCommand = true;

      }//end for loop.

      //
      // Add the selection pageMenuGroup groupCommand.
      //
      if ( displayGenerateCommand == true )
      {
        groupCommand = pageGroup.addCommand (
          EdLabels.Reports_Generate_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Reports.ToString ( ),
              Model.UniForm.ApplicationMethods.Custom_Method );

        groupCommand.SetGuid ( this.Session.Report.Guid );

        groupCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.Get_Object );

        groupCommand.AddParameter ( Model.UniForm.CommandParameters.Page_Id,
          this.Session.PageId );
      }
      this.LogMethodEnd ( "getReport_Query_Selection_Group" );

    }//END getReports_List_Selection_Group method

    //  =====================================================================
    /// <summary>
    /// Obtains the selection list for the query specified. The Selection
    /// source in the query should be "List_Identifier":"Parameters"
    /// "Parameters" are not mandatory.
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    /// ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    private EvOption [ ] getQuerySelectionList (
      EvReportQuery query )
    {
      this.LogMethod ( "getQuerySelectionList method " );
      this.LogValue ( "SelectionSource: " + query.SelectionSource );

      //TODO Fix this part
      String [ ] selectionParameters = query.SelectionSource.ToString ( ).Split ( ':' );

      String listId = selectionParameters [ 0 ];
      String parameters = string.Empty;

      //Parameters are not compulsory.
      if ( selectionParameters.Length > 1 )
      {
        parameters = selectionParameters [ 1 ];
      }

      // 
      // Parse the listId into the selection list types.
      // 
      EvReport.SelectionListTypes selectionListTypes = (EvReport.SelectionListTypes) Enum.Parse ( typeof ( EvReport.SelectionListTypes )
        , listId );

      query.SelectionList = this._Bll_ReportTemplates.getSelectionList (
        selectionListTypes,
        parameters,
        this.Session.UserProfile ).ToArray ( );

      this.LogMethodEnd ( "getQuerySelectionList" );

      return query.SelectionList;

    }//END getQuerySelectionList method.

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
      this.LogMethod ( "getReport_Display_Group" );
      //
      // Initialise method variables and objects.
      //
      Evado.Model.UniForm.Field groupField = new Model.UniForm.Field ( );
      Evado.Model.UniForm.Command pageCommand = new Model.UniForm.Command ( );

      PageObject.Title += " > " + this.Session.Report.ReportSubTitle;

      // Create the new pageMenuGroup.
      // 
      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
        EdLabels.Reports_Download_Group_Title,
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
          this.ErrorMessage = EdLabels.Report_column_Mismatch_Error_Message );

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

        pageGroup.Description = this.Session.Report.getReportAsHtml ( this.Session.UserProfile );

        this.LogValue ( this.Session.Report.Log );

      } //END report has content.
      else
      {
        groupField = pageGroup.createReadOnlyTextField (
          String.Empty,
          String.Empty,
        this.ErrorMessage = EdLabels.Report_Result_Empty_Message );

        this.LogMethodEnd ( "getReport_Display_Group" );

        return;

      }

    }//END getReport_Display_Group method

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Create report output pageMenuGroup

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private void getReport_Output_Group (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getReport_Output_Group" );
      //
      // Initialise method variables and objects.
      //
      Evado.Model.UniForm.Field groupField = new Model.UniForm.Field ( );
      Evado.Model.UniForm.Group pageGroup = new Model.UniForm.Group ( );
      String CssDirectorypath = this.AdapterObjects.ApplicationPath + @"\css\";

      if ( this._OutputReport == false )
      {
        this.LogValue ( "Do not output report." );
        return;
      }

      if ( this.Session.Report == null )
      {
        this.LogValue ( "Report is null" );
        return;
      }

      if ( this.Session.Report.DataRecords.Count == 0 )
      {
        this.LogValue ( "Report is empty" );
        return;
      }

      //
      // Define the output filenames
      //
      string htmlFilename = this.Session.Report.ReportId
        + "-" + DateTime.Now.ToString ( "dd-mmm-yy-HHmmss" ) + ".htm";
      htmlFilename = htmlFilename.ToLower ( );

      this.LogValue ( "htmlFilename: " + htmlFilename );

      string csvFilename = this.Session.Report.ReportId
        + "-" + DateTime.Now.ToString ( "dd-mmm-yy-HHmmss" ) + ".csv";
      csvFilename = csvFilename.ToLower ( );

      this.LogValue ( "csvFilename: " + csvFilename );

      string csvDataFilename = this.Session.Report.ReportId
        + "-" + DateTime.Now.ToString ( "dd-mmm-yy-HHmmss" ) + "_data.csv";
      csvDataFilename = csvDataFilename.ToLower ( );

      this.LogValue ( "csvDataFilename: " + csvDataFilename );


      string jsonDataFilename = this.Session.Report.ReportId
        + "-" + DateTime.Now.ToString ( "dd-mmm-yy-HHmmss" ) + "_json.txt ";
      jsonDataFilename = jsonDataFilename.ToLower ( );

      this.LogValue ( "jsonDataFilename: " + jsonDataFilename );
      //
      // Save the report as a stand alone html document.
      //
      String htmlReport = this.createHtmlReport ( CssDirectorypath );

      Evado.Model.Digital.EvcStatics.Files.saveFile ( this.UniForm_BinaryFilePath, htmlFilename, htmlReport );

      //
      // Save the report as a CSV file.
      //
      String csvReport = this.Session.Report.getReportAsCsv ( ",", this.Session.UserProfile );

      Evado.Model.Digital.EvcStatics.Files.saveFile ( this.UniForm_BinaryFilePath, csvFilename, csvReport );

      //
      // Save the data as a CSV file.
      //
      csvReport = this.Session.Report.getReportFlatAsCsv ( ",", this.Session.UserProfile );


      Evado.Model.Digital.EvcStatics.Files.saveFile ( this.UniForm_BinaryFilePath, csvDataFilename, csvReport );

      //
      // Save the data as a JSON object.
      //
      String jsonReport = this.getJasonSerialiseReport ( );

      this.LogDebug ( jsonReport );

      Evado.Model.Digital.EvcStatics.Files.saveFile ( this.UniForm_BinaryFilePath, jsonDataFilename, jsonReport );

      //
      // Create the download pageMenuGroup.
      // 
      pageGroup = Page.AddGroup (
        EdLabels.Reports_Download_Group_Title,
        Evado.Model.UniForm.EditAccess.Enabled );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      groupField = pageGroup.addField (
        String.Empty,
       this.Session.Report.ReportId + " "
        + this.Session.Report.ReportTitle + " "
        + EdLabels.Report_Html_Download_Link_Title,
        Evado.Model.EvDataTypes.Html_Link,
        htmlFilename );

      groupField = pageGroup.addField (
        String.Empty,
        String.Format (
          EdLabels.Report_CSV_Download_Link_Title,
          this.Session.Report.ReportId,
          this.Session.Report.ReportTitle ),
        Evado.Model.EvDataTypes.Html_Link,
        csvFilename );

      groupField = pageGroup.addField (
        String.Empty,
        String.Format (
          EdLabels.Report_Data_Download_Link_Title,
          this.Session.Report.ReportId,
          this.Session.Report.ReportTitle ),
        Evado.Model.EvDataTypes.Html_Link,
        csvDataFilename );

      groupField = pageGroup.addField (
        String.Empty,
        String.Format (
          EdLabels.Report_Json_Download_Link_Title,
          this.Session.Report.ReportId,
          this.Session.Report.ReportTitle ),
        Evado.Model.EvDataTypes.Html_Link,
        jsonDataFilename );



    }//END getReport_Save_Group method

    //  ==================================================================================
    /// <summary>
    /// This method JSON serialises the report structure.
    /// </summary>
    /// <returns>String: containing the JSON report content.</returns>
    //  ---------------------------------------------------------------------------------
    private string getJasonSerialiseReport ( )
    {
      //
      // Initialise the methods variables and objects.
      //
      String stJsonReport = string.Empty;
      Newtonsoft.Json.JsonSerializerSettings jsonSettings = new Newtonsoft.Json.JsonSerializerSettings
      {
        NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
      };

      //
      // create a JSON serialisation of the report object.
      //
      stJsonReport = Newtonsoft.Json.JsonConvert.SerializeObject (
        this.Session.Report, Newtonsoft.Json.Formatting.Indented, jsonSettings );

      //
      // Return the json serialised report.
      //
      return stJsonReport;

    }//END getJasonSerialiseReport method

    //  ==================================================================================
    /// <summary>
    ///  This method is executed when a derived channel ResultData is being updated.
    /// </summary>
    /// <param name="CssDirectorypath">String: the css directory path</param>
    /// <returns>Html text.</returns>
    //  ---------------------------------------------------------------------------------
    private String createHtmlReport (
      String CssDirectorypath )
    {
      this.LogMethod ( "createHtmlCrfPage" );
      this.LogValue ( "CssDirectorypath: " + CssDirectorypath );
      // 
      // Initialise the methods variables and objects.
      // 
      System.Text.StringBuilder sbHtmlText = new System.Text.StringBuilder ( );

      //
      // Append the header to the document
      //
      sbHtmlText.AppendLine ( this.getHtmlPageHeader ( CssDirectorypath ) );

      //
      // Generate the HTML
      // 
      sbHtmlText.AppendLine (
        this.Session.Report.getReportAsHtml (
        this.Session.UserProfile ) );

      //
      // Append the footer to the document
      //
      sbHtmlText.AppendLine ( this.getHtmlPageFooter ( ) );

      // 
      // stReturn the updates script result.
      //
      return sbHtmlText.ToString ( );

    }//END createHtmlCrfPage method

    //  ==================================================================================
    /// <summary>
    ///  This method is executed when a derived channel ResultData is being updated.
    /// </summary>
    /// <returns>Html text.</returns>
    //  ---------------------------------------------------------------------------------
    private String getHtmlPageHeader ( String CssDirectorypath )
    {
      this.LogMethod ( "getHtmlPageHeader" );
      this.LogValue ( "CssDirectorypath: " + CssDirectorypath );
      //
      // Initialise the method variables and objects.
      //
      String stHtml = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \r\n"
       + "\"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n"
       + " <html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n "
       + "<head>\r\n"
       + "<title>Report: " + this.Session.Report.ReportTitle + "</title>\r\n";

      stHtml += this.getStyles ( CssDirectorypath );

      stHtml += "</head>\r\n"
      + "<body>\r\n"
      + "<center>"
      + " <div id=\"page-body\">";

      return stHtml;

    }//END getHtmlPageHeader method

    //  ==================================================================================
    /// <summary>
    ///  This method generates a list of CSS styles for the document.
    /// </summary>
    /// <returns>Text contains CSS styles.</returns>
    //  ---------------------------------------------------------------------------------
    private String getStyles ( String CssDirectorypath )
    {
      this.LogMethod ( "getStyles" );
      this.LogValue ( "CssDirectorypath: " + CssDirectorypath );
      // 
      // Initialise the method variables and objects
      //
      String stStyles = String.Empty;

      //
      // Add open stages.
      //
      stStyles += "<style type=\"text/css\" media=\"screen, print\">\r\n";

      //
      // Get the file information.
      //
      System.IO.DirectoryInfo di = new System.IO.DirectoryInfo ( CssDirectorypath );

      // 
      // Get a reference to each file in that directory.
      // 
      System.IO.FileInfo [ ] fiArr = di.GetFiles ( );

      //
      // Iterate through the list of files.
      //
      for ( int index = 0; index < fiArr.Length; index++ )
      {

        if ( fiArr [ index ].Extension.Contains ( "css" ) == true
          && fiArr [ index ].Name.ToLower ( ).Contains ( "exportrpt" ) == true )
        {
          stStyles += "<!--- Style sheet: " + fiArr [ index ] + "-->\r\n";
          //
          // Read the style sheet
          //
          stStyles += this.readStyleSheet ( fiArr [ index ] );
        }

      }//END loop
      stStyles = stStyles.Replace ( "  ", " " );
      stStyles = stStyles.Replace ( "  ", " " );
      stStyles = stStyles.Replace ( "  ", " " );
      stStyles = stStyles.Replace ( "  ", " " );
      stStyles = stStyles.Replace ( ";", "; " );
      stStyles = stStyles.Replace ( "}", "}\r\n" );

      //
      // Add closing stages.
      //
      stStyles += "</style>";

      return stStyles;

    }//ENd getStyles method

    //  ==================================================================================
    /// <summary>
    ///  This method generates a list of CSS styles for the document.
    /// </summary>
    /// <returns>Text contains CSS styles.</returns>
    //  ---------------------------------------------------------------------------------
    private String readStyleSheet ( System.IO.FileInfo file )
    {
      //
      // Initialise methos variables and objects.
      //
      System.IO.TextReader reader;
      System.Text.StringBuilder cssLines = new System.Text.StringBuilder ( );
      String line = String.Empty;
      int inCount = 0;

      // 
      // Open the text reader with supplied file
      // 
      using ( reader = System.IO.File.OpenText ( file.FullName ) )
      {
        // 
        // Read the remainder of the file into the outputLog array list
        // 
        while ( ( line = reader.ReadLine ( ) ) != null && inCount < 10000 )
        {
          if ( line.Contains ( "@import" ) == true )
          {
            continue;
          }

          cssLines.Append ( line );

          inCount++;
        }
      }

      cssLines.AppendLine ( string.Empty );

      return cssLines.ToString ( );

    }//END readStyleSheet method.

    //  ==================================================================================
    /// <summary>
    ///  This method is executed when a derived channel ResultData is being updated.
    /// </summary>
    /// <returns>Html text.</returns>
    //  ---------------------------------------------------------------------------------
    private String getHtmlPageFooter ( )
    {
      //
      // Initialise the method variables and objects.
      //
      String stHtml = "</div>"
        + "</center>\r\n"
        + "</body>\r\n"
        + "</html>";

      return stHtml;

    }//END getHtmlPageFooter method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
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
      try
      {
        this.LogMethod ( "createObject" );

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          "Evado.UniForm.Clinical.Actvities.createObject",
          this.Session.UserProfile );

        // 
        // Initialise the methods variables and objects.
        //      
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
        this.Session.Report = new EvReport ( );
        this.Session.Report.Guid = Evado.Model.Digital.EvcStatics.CONST_NEW_OBJECT_ID;

        Guid reportTemplateGuid = Command.GetGuid ( );

        // 
        // Retrieve the customer object from the database via the DAL and BLL layers.
        // 
        this.Session.Report = this._Bll_ReportTemplates.getReport ( reportTemplateGuid );

        this.getDataObject ( clientDataObject );


        this.LogValue ( "Exit createObject method. ID: " + clientDataObject.Id + ", Title: " + clientDataObject.Title );

        return clientDataObject;

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

      return this.Session.LastPage;

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
      try
      {
        this.LogMethod ( "updateObject" );
        this.LogValue ( "Parameter PageCommand: "
          + PageCommand.getAsString ( false, true ) );
      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.Reports_Update_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }
      return this.Session.LastPage;

    }//END method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
  }
}//END namespace