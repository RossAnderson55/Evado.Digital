/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\Organisations.cs" company="EVADO HOLDING PTY. LTD.">
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
  /// This class defines the selection list class
  /// 
  /// This class terminates the Selection Lists object.
  /// </summary>
  public class EuPageLayouts : EuClassAdapterBase
  {
    #region Class Initialisation
    /// <summary>
    /// This method initialises the class.
    /// </summary>
    public EuPageLayouts ( )
    {
      this.ClassNameSpace = "Evado.UniForm.Digital.EuPageLayouts.";

      if ( this.Session.AdminPageLayout == null )
      {
        this.Session.AdminPageLayout = new EdPageLayout ( );
      }

      if ( this.Session.UploadFileName == null )
      {
        this.Session.UploadFileName = String.Empty;
      }
    }

    /// <summary>
    /// This method initialises the class and passs in the user profile.
    /// </summary>
    public EuPageLayouts (
      EuGlobalObjects AdapterObjects,
      EvUserProfileBase ServiceUserProfile,
      EuSession SessionObjects,
      String UniFormBinaryFilePath,
      EvClassParameters Settings )
    {
      this.ClassNameSpace = "Evado.UniForm.Digital.EuPageLayouts.";
      this.AdapterObjects = AdapterObjects;
      this.ServiceUserProfile = ServiceUserProfile;
      this.Session = SessionObjects;
      this.UniForm_BinaryFilePath = UniFormBinaryFilePath;
      this.ClassParameters = Settings;


      this.LogInitMethod ( "EuPageLayouts initialisation" );
      this.LogInit ( "ServiceUserProfile.UserId: " + ServiceUserProfile.UserId );
      this.LogInit ( "SessionObjects.UserProfile.Userid: " + this.Session.UserProfile.UserId );
      this.LogInit ( "SessionObjects.UserProfile.CommonName: " + this.Session.UserProfile.CommonName );
      this.LogInit ( "UniFormBinaryFilePath: " + this.UniForm_BinaryFilePath );

      this.LogInit ( "Settings:" );
      this.LogInit ( "-PlatformId: " + this.ClassParameters.PlatformId );
      this.LogInit ( "-ApplicationGuid: " + this.ClassParameters.AdapterGuid );
      this.LogInit ( "-LoggingLevel: " + Settings.LoggingLevel );
      this.LogInit ( "-UserId: " + Settings.UserProfile.UserId );
      this.LogInit ( "-UserCommonName: " + Settings.UserProfile.CommonName );

      if ( this.Session.AdminPageLayout == null )
      {
        this.Session.AdminPageLayout = new EdPageLayout ( );
      }

      if ( this.Session.UploadFileName == null )
      {
        this.Session.UploadFileName = String.Empty;
      }

      this._Bll_PageLayouts = new Evado.Bll.Digital.EdPageLayouts ( this.ClassParameters );

    }//END Method


    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class constants and variables.

    private Evado.Bll.Digital.EdPageLayouts _Bll_PageLayouts = new Evado.Bll.Digital.EdPageLayouts ( );

    private bool ImportExportSelected = false;

    public const string CONST_IMP_EXP_FIELD_ID = "IM-EX";

    public const string CONST_TEMPLATE_FIELD_ID = "IFTF";

    public const string CONST_TEMPLATE_EXTENSION = ".pl.csv";


    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class public methods

    // ==================================================================================
    /// <summary>
    /// This method gets the trial site object.
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object</param>
    /// <returns>ClientApplicationData</returns>
    //  ----------------------------------------------------------------------------------
    public Evado.Model.UniForm.AppData getDataObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getDataObject" );
      this.LogValue ( "PageCommand Content: " + PageCommand.getAsString ( false, true ) );
      try
      {
        // 
        // Initialise the methods variables and objects.
        // 
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
        this.ImportExportSelected = false;

        //
        // if the import export parameter exists then import export enabled.
        //
        if ( PageCommand.hasParameter ( EuPageLayouts.CONST_IMP_EXP_FIELD_ID ) == true )
        {
          this.ImportExportSelected = true;
        }
        this.LogDebug ( "ImportExportSelected {0}. ", ImportExportSelected );

        this.Session.PageId = PageCommand.GetPageId ( );
        this.LogDebug ( "PageId {0}", this.Session.PageId );

        this.LogDebug ( "Command.Method {0}", PageCommand.Method );
        // 
        // Determine the method to be called
        // 
        switch ( PageCommand.Method )
        {
          case Evado.Model.UniForm.ApplicationMethods.List_of_Objects:
            {
              this.LogDebug ( "get list items" );
              clientDataObject = this.getListObject ( PageCommand );
              break;
            }
          case Evado.Model.UniForm.ApplicationMethods.Get_Object:
            {
              this.LogDebug ( "get object" );
              clientDataObject = this.getObject ( PageCommand );
              break;
            }
          case Evado.Model.UniForm.ApplicationMethods.Create_Object:
            {
              this.LogDebug ( "create object" );
              clientDataObject = this.createObject ( PageCommand );
              break;
            }
          case Evado.Model.UniForm.ApplicationMethods.Save_Object:
          case Evado.Model.UniForm.ApplicationMethods.Delete_Object:
            {
              this.LogDebug ( "Save object" );
              // 
              // Update the object values
              // 
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
        this.LogException ( Ex );
      }
      this.LogMethodEnd ( "getDataObject" );
      return this.Session.LastPage;

    }//END getDataObject method


    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class list methods
    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    public Evado.Model.UniForm.AppData getListObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getListObject" );
      try
      {
        // 
        // Initialise the methods variables and objects.
        //      
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );

        //
        // Determine if the user has access to this page and log and error if they do not.
        //
        if ( this.Session.UserProfile.hasAdministrationAccess == false )
        {
          this.LogIllegalAccess (
            this.ClassNameSpace + "getListObject",
            this.Session.UserProfile );

          this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

          return this.Session.LastPage; ;
        }

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "getListObject",
          this.Session.UserProfile );

        //
        // read in the form template upload filename.
        //
        string value = PageCommand.GetParameter ( EuPageLayouts.CONST_TEMPLATE_FIELD_ID );

        this.LogValue ( "Upload filename: " + value );

        if ( value != string.Empty )
        {
          this.Session.UploadFileName = value;
        }
        this.LogValue ( "FormTemplateFilename: " + this.Session.UploadFileName );

        //
        // get the selection lists.
        //
        this.GetPageLayoutList ( );

        //
        // Initialise the page objects.
        //
        clientDataObject.Title = EdLabels.PageLayouts_List_Page_Title;
        clientDataObject.Page.Title = clientDataObject.Title;
        clientDataObject.Id = Guid.NewGuid ( );


        //
        // import  command
        //
        var pageCommand = clientDataObject.Page.addCommand (
          EdLabels.PageLayout_Import_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Page_Layouts.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Custom_Method );

        // 
        // Define the groupCommand parameters
        // 
        pageCommand.SetGuid ( this.Session.AdminPageLayout.Guid );
        pageCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.List_of_Objects );
        pageCommand.AddParameter ( EuPageLayouts.CONST_IMP_EXP_FIELD_ID, "YES" );

        //
        // Display the selection list upload group if selected.
        //
        if ( this.ImportExportSelected == true )
        {
          //
          // if the form template filename is empty display the selection field.
          //
          if ( this.Session.UploadFileName == String.Empty )
          {
            this.LogValue ( "FormTemplateFilename is empty" );

            this.GetPageLayoutUploadDataObject ( clientDataObject );
          }
          else
          {
            this.LogValue ( "Processing the uploaded file." );

            this.GetPageLayoutUpload_Group ( clientDataObject );
          }

        }//END upload groups.

        // 
        // Add the trial organisation list to the page.
        // 
        this.getListGroup ( clientDataObject.Page );

        this.LogValue ( "data.Title: " + clientDataObject.Title );
        this.LogValue ( "data.Page.Title: " + clientDataObject.Page.Title );


        this.LogMethodEnd ( "getListObject" );
        return clientDataObject;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.SelectionList_List_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      this.LogMethodEnd ( "getListObject" );
      return this.Session.LastPage; ;

    }//END getListObject method.

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    public void GetPageLayoutList ( )
    {
      this.LogMethod ( "GetPageLayoutList" );

      if ( this.AdapterObjects.AllPageLayouts.Count > 0 )
      {
        this.LogMethodEnd ( "GetPageLayoutList" );
        return;
      }

      this.AdapterObjects.AllPageLayouts = this._Bll_PageLayouts.getView ( EdPageLayout.States.Null );

      this.LogDebugClass ( this._Bll_PageLayouts.Log );

      this.LogDebug ( "Selection list count {0}.", this.AdapterObjects.AllPageLayouts.Count );

      this.LogMethodEnd ( "GetPageLayoutList" );

    }//END getSelectionList method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    public void getListGroup (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getListGroup" );
      try
      {
        // 
        // Create the new pageMenuGroup.
        // 
        Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
          EdLabels.SelectionLists_List_Group_Title );
        pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
        pageGroup.CmdLayout = Evado.Model.UniForm.GroupCommandListLayouts.Vertical_Orientation;

        // 
        // Add the save groupCommand
        // 
        Evado.Model.UniForm.Command groupCommand = pageGroup.addCommand (
          EdLabels.SelectionLists_New_List_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Page_Layouts.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Create_Object );

        groupCommand.SetBackgroundColour (
          Model.UniForm.CommandParameters.BG_Default,
          Model.UniForm.Background_Colours.Purple );

        // 
        // generate the page links.
        // 
        foreach ( EdPageLayout listItem in this.AdapterObjects.AllPageLayouts )
        {
          // 
          // Add the trial organisation to the list of organisations as a groupCommand.
          // 
          Evado.Model.UniForm.Command command = pageGroup.addCommand (
            listItem.CommandText,
            EuAdapter.ADAPTER_ID,
            EuAdapterClasses.Page_Layouts.ToString ( ),
            Evado.Model.UniForm.ApplicationMethods.Get_Object );

          command.Id = listItem.Guid;
          command.SetGuid ( listItem.Guid );

        }//END trial organisation list iteration loop

        this.LogValue ( "command count: " + pageGroup.CommandList.Count );

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.SelectionList_List_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

    }//END getListObject method.

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class form template upload methods
    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="ClientDataObject">Evado.Model.UniForm.AppData object.</param>
    //  ------------------------------------------------------------------------------
    private void GetPageLayoutUploadDataObject (
      Evado.Model.UniForm.AppData ClientDataObject )
    {
      this.LogMethod ( "GetPageLayoutUploadDataObject" );

      //
      // set the page edit access.
      //
      ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
      if ( this.Session.UserProfile.hasManagementAccess == true )
      {
        ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
      }

      //
      // Add the file selection group.
      //
      this.getUpload_FileSelectionGroup ( ClientDataObject.Page );

    }//END GetPageLayoutUploadDataObject Method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void getUpload_FileSelectionGroup (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getUpload_FileSelectionGroup" );
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
      pageGroup = Page.AddGroup (
        String.Empty,
        Evado.Model.UniForm.EditAccess.Inherited );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;

      pageGroup.SetCommandBackBroundColor (
        Model.UniForm.GroupParameterList.BG_Mandatory,
        Model.UniForm.Background_Colours.Red );

      groupField = pageGroup.createBinaryFileField (
        EuPageLayouts.CONST_TEMPLATE_FIELD_ID,
        EdLabels.Form_Template_File_Selection_Field_Title,
        String.Empty,
        this.Session.UploadFileName );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      groupField.AddParameter ( Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, "Yes" );

      groupCommand = pageGroup.addCommand (
        EdLabels.SelectionList_Upload_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Entity_Layouts.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Custom_Method );

      groupCommand.SetPageId ( EdStaticPageIds.Form_Template_Upload );
      groupCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.List_of_Objects );

      this.LogMethodEnd ( "getUpload_FileSelectionGroup" );

    }//END getUpload_FileSelectionGroup Method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="ClientDataObject">Evado.Model.UniForm.AppData object.</param>
    //  ------------------------------------------------------------------------------
    private void GetPageLayoutUpload_Group (
      Evado.Model.UniForm.AppData ClientDataObject )
    {
      this.LogMethod ( "getSelectionListUpload_Group" );

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Guid formGuid = Guid.Empty;
      EdPageLayout pageLayout = new EdPageLayout ( );

      //
      // Define the general properties pageMenuGroup..
      //
      pageGroup = ClientDataObject.Page.AddGroup (
        EdLabels.Form_Template_Upload_Log_Group_Title,
        Evado.Model.UniForm.EditAccess.Inherited );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;

      //
      // Upload the the form by it form file name.
      //
      pageLayout = this.ReadCsvData (
        this.UniForm_BinaryFilePath,
        this.Session.UploadFileName );

      this.LogValue ( "Uploaded selection list is: " + pageLayout.PageId );

      //
      // save the uploaded form.
      //
      String processLog = this.SaveUploadedPageLayout ( pageLayout );

      this.LogValue ( "processLog: " + processLog );

      pageGroup.Description = processLog;

      //
      // reset the form template filename.
      //
      this.Session.UploadFileName = String.Empty;

      this.LogMethodEnd ( "getSelectionListUpload_Group" );

    }//END getPropertiesDataObject Method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private EdPageLayout ReadCsvData (
       String FileDirectory,
        String FileName )
    {
      this.LogMethod ( "ReadCsvData" );
      //
      // Initialise the methods variables and objects.
      //
      EdPageLayout pageLayout = new EdPageLayout ( );

      //
      // read in the file as list of string.
      //
      List<String> StringList = Evado.Model.EvStatics.Files.readFileAsList (
         FileDirectory,
         FileName );

      for ( int i = 0; i < StringList.Count; i++ )
      {
        String str = StringList [ i ];

        if ( str == String.Empty )
        {
          continue;
        }

        String [ ] stColumns = str.Split ( ';' );

        if ( stColumns.Length < 2 )
        {
          continue;
        }

        if ( stColumns[0] == String.Empty )
        {
          continue;
        }

        this.LogDebug ( "I {0}, name {1}, V: {2} ", i, stColumns [ 0 ], stColumns [ 1 ] );

        var columName = EvStatics.parseEnumValue<EdPageLayout.FieldNames> ( stColumns [ 0 ] );

        pageLayout.setValue ( columName, stColumns [ 1 ] );


      }//END CSV row iteration loop

      this.LogMethodEnd ( "ReadCsvData" );
      return pageLayout;

    }

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="ClientDataObject">Evado.Model.UniForm.AppData object.</param>
    //  ------------------------------------------------------------------------------
    private String SaveUploadedPageLayout (
      EdPageLayout uploadedPageLayout )
    {
      this.LogMethod ( "SaveUploadedSelectionList" );
      this.LogValue ( "Uploaded PageId is: " + uploadedPageLayout.PageId );
      //
      // initialise the methods variables and objects.
      //
      Guid formGuid = Guid.Empty;
      StringBuilder processLog = new StringBuilder ( );
      Evado.Model.EvEventCodes result = EvEventCodes.Ok;
      float version = 0.0F;

      processLog.AppendLine ( "Saving form: " + uploadedPageLayout.PageId + " " + uploadedPageLayout.Title );
      //
      // Get the list of forms to determine if there is an existing draft form.
      //
      if ( this.AdapterObjects.AllPageLayouts.Count == 0 )
      {
        this.GetPageLayoutList ( );
      }

      //
      // check if there is a draft form and delete it.
      //
      foreach ( EdPageLayout pageLayout in this.AdapterObjects.AllPageLayouts )
      {
        //
        // get the list issued version of the form.
        //
        if ( pageLayout.PageId == uploadedPageLayout.PageId
          && pageLayout.State == EdPageLayout.States.Issued )
        {
          version = pageLayout.Version;
        }

        //
        // delete any existing draft forms with form ID
        //
        if ( pageLayout.PageId == uploadedPageLayout.PageId
          && pageLayout.State == EdPageLayout.States.Draft )
        {
          processLog.AppendLine ( "Existing draft version of " + uploadedPageLayout.PageId + " " + uploadedPageLayout.Title + " found." );

          pageLayout.Action = EdPageLayout.SaveActions.Delete;

          result = this._Bll_PageLayouts.SaveItem ( pageLayout );

          if ( result == EvEventCodes.Ok )
          {
            processLog.AppendLine ( "Existing draft version of successfully deleted." );
          }
          else
          {
            processLog.AppendLine ( "Deletion process returned the following error: " +
              Evado.Model.EvStatics.enumValueToString ( result ) );

            return processLog.ToString ( );
          }
        }
      }

      processLog.AppendLine ( "Saving uploaded form to the database." );
      //
      // set the form's save parameters 

      uploadedPageLayout.State = EdPageLayout.States.Draft;
      uploadedPageLayout.Action = EdPageLayout.SaveActions.Save;
      uploadedPageLayout.Version = 0;
      uploadedPageLayout.Guid = Guid.Empty;

      //
      // Save the form
      //
      result = this._Bll_PageLayouts.SaveItem ( uploadedPageLayout );

      this.LogText ( this._Bll_PageLayouts.Log );

      if ( result == EvEventCodes.Ok )
      {
        processLog.AppendLine ( "Uploaded form successfully save to database." );
      }
      else
      {
        processLog.AppendLine ( "Save process returned the following error: " +
          Evado.Model.EvStatics.enumValueToString ( result ) );
      }

      return processLog.ToString ( );

    }//END saveUploadeForm method


    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class get object methods

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData getObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getObject" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
      Guid OrgGuid = Guid.Empty;

      //
      // Determine if the user has access to this page and log and error if they do not.
      //
      if ( this.Session.UserProfile.hasAdministrationAccess == false )
      {
        this.LogIllegalAccess (
          this.ClassNameSpace + "getObject",
          this.Session.UserProfile );

        this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

        return this.Session.LastPage; ;
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
      OrgGuid = PageCommand.GetGuid ( );
      this.LogValue ( "OrgGuid: " + OrgGuid );

      // 
      // return if not trial id
      // 
      if ( OrgGuid == Guid.Empty )
      {
        this.LogValue ( "Guid Empty get current object" );

        if ( this.Session.AdminPageLayout.Guid != Guid.Empty )
        {
          // 
          // return the client ResultData object for the customer.
          // 
          this.getDataObject ( clientDataObject );
        }
        else
        {
          this.LogValue ( "ERROR: current organisation guid empty" );
          this.ErrorMessage = EdLabels.Organisation_Guid_Empty_Message;
        }

        return clientDataObject;
      }
      this.LogValue ( "Query site Guid: " + OrgGuid );

      try
      {
        // 
        // Retrieve the customer object from the database via the DAL and BLL layers.
        // 
        this.Session.AdminPageLayout = this._Bll_PageLayouts.getItem ( OrgGuid );

        this.LogValue ( this._Bll_PageLayouts.Log );

        this.LogDebug ( "SessionObjects.AdminPageLayout.PageId: "
          + this.Session.AdminPageLayout.PageId );

        // 
        // return the client ResultData object for the customer.
        // 
        this.getDataObject ( clientDataObject );

        this.LogValue ( "Page.Title: " + clientDataObject.Page.Title );

        return clientDataObject;
      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.PageLayout_Page_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return this.Session.LastPage; ;

    }//END getObject method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="ClientDataObject">Evado.Model.UniForm.AppData object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private void getDataObject ( Evado.Model.UniForm.AppData ClientDataObject )
    {
      this.LogMethod ( "getDataObject" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );
      Evado.Model.UniForm.Field pageField = new Evado.Model.UniForm.Field ( );

      ClientDataObject.Id = this.Session.AdminPageLayout.Guid;

      ClientDataObject.Title = EdLabels.PageLayout_New_List_Page_Title;

      if ( this.Session.AdminPageLayout.PageId != String.Empty )
      {
        ClientDataObject.Title =
          String.Format ( EdLabels.PageLayout_Page_Title,
          this.Session.AdminPageLayout.PageId,
          this.Session.AdminPageLayout.Title );
      }
      ClientDataObject.Page.Id = ClientDataObject.Id;
      ClientDataObject.Page.Title = ClientDataObject.Title;
      ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;


      //
      // Set the user edit access to the objects.
      //
      if ( this.Session.UserProfile.hasAdministrationAccess == true )
      {
        ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
      }
      this.LogValue ( "Page.EditAccess: " + ClientDataObject.Page.EditAccess );

      //
      // Add the page commands 
      //
      this.getDataObject_PageCommands ( ClientDataObject.Page );

      if ( this.ImportExportSelected == true )
      {
        this.getPageLayoutDownloadGroup ( ClientDataObject.Page );
      }

      //
      // Add the detail group to the page.
      //
      this.getDataObject_GeneralGroup ( ClientDataObject.Page );

      //
      // display the header properties group.
      //
      if ( this.Session.AdminPageLayout.hasActiveLayout( EdPageLayout.LayoutComponentList.Page_Header ) == true )
      {
      this.getDataObjectHeaderGroup ( ClientDataObject.Page );
      }

      //
      // display the left column properties group.
      //
      if ( this.Session.AdminPageLayout.hasActiveLayout ( EdPageLayout.LayoutComponentList.Left_Column ) == true )
      {
        this.getDataObject_LeftColumnGroup ( ClientDataObject.Page );
        ClientDataObject.Page.SetLeftColumnWidth ( 25 );
      }

      //
      // display the center column properties group.
      //
        this.getDataObject_CenterColumnGroup ( ClientDataObject.Page );
      //
      // display the right column properties group.
      //
        if ( this.Session.AdminPageLayout.hasActiveLayout ( EdPageLayout.LayoutComponentList.Right_Column ) == true )
        {
          this.getDataObject_RightColumnGroup ( ClientDataObject.Page );

          ClientDataObject.Page.SetRightColumnWidth ( 25 );
        }
      this.LogMethodEnd ( "getDataObject" );

    }//END Method

    //================================================================================
    /// <summary>
    /// This method add the group commands to the grop.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Group object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_PageCommands (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getDataObject_PageCommands" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );

      // 
      // Add the save groupCommand
      // 
      if ( PageObject.EditAccess == Evado.Model.UniForm.EditAccess.Enabled )
      {
        //
        // Export  command
        //
        pageCommand = PageObject.addCommand (
          EdLabels.PageLayout_Export_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Page_Layouts.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Custom_Method );

        // 
        // Define the groupCommand parameters
        // 
        pageCommand.SetGuid ( this.Session.AdminPageLayout.Guid );
        pageCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.Get_Object );
        pageCommand.AddParameter ( EuPageLayouts.CONST_IMP_EXP_FIELD_ID, "YES" );

        //
        // save command.
        //
        pageCommand = PageObject.addCommand (
          EdLabels.PageLayout_Save_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Page_Layouts.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Save_Object );

        // 
        // Define the the issue groupCommand parameters
        // 
        pageCommand.SetGuid ( this.Session.AdminPageLayout.Guid );
        pageCommand.AddParameter (
           Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
          EdPageLayout.SaveActions.Save.ToString ( ) );
        //
        // Issue command
        //
        pageCommand = PageObject.addCommand (
          EdLabels.PageLayout_Issue_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Page_Layouts.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Save_Object );

        // 
        // Define the save and issue groupCommand parameters
        // 
        pageCommand.SetGuid ( this.Session.AdminPageLayout.Guid );
        pageCommand.AddParameter (
           Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
          EdPageLayout.SaveActions.Issue.ToString ( ) );

        //
        // Delete command
        //
        if ( this.Session.AdminPageLayout.State == EdPageLayout.States.Draft )
        {
          pageCommand = PageObject.addCommand (
            EdLabels.PageLayout_Delete_Command_Title,
            EuAdapter.ADAPTER_ID,
            EuAdapterClasses.Page_Layouts.ToString ( ),
            Evado.Model.UniForm.ApplicationMethods.Save_Object );

          // 
          // Define the save and delete groupCommand parameters
          // 
          pageCommand.SetGuid ( this.Session.AdminPageLayout.Guid );
          pageCommand.AddParameter (
             Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
            EdPageLayout.SaveActions.Delete.ToString ( ) );
        }
      }

      this.LogMethodEnd ( "getDataObject_PageCommands" );

    }//END getDataObject_GroupCommands Method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Command object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private void getPageLayoutDownloadGroup (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getSelectionListDownloadPage" );
      this.LogValue ( "UniForm_BinaryFilePath: " + this.UniForm_BinaryFilePath );

      //
      // if import export disabled exit method.
      //
      if ( this.ImportExportSelected == false )
      {
        return;
      }

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      Evado.Model.UniForm.Parameter parameter = new Evado.Model.UniForm.Parameter ( );
      String csvData = String.Empty;
      String templateUrl = String.Empty;
      String formTemplateFilename = String.Empty;
      //
      // exist if the form object is null.
      //
      if ( this.Session.AdminPageLayout == null )
      {
        this.LogValue ( " Form object is null" );
        this.LogMethodEnd ( "getSelectionListDownloadPage" );
        return;
      }

      //
      // exist if the form object is null.
      //
      if ( this.Session.AdminPageLayout.Guid == Guid.Empty
        || this.UniForm_BinaryFilePath == String.Empty
        || this.UniForm_BinaryServiceUrl == String.Empty )
      {
        this.LogValue ( " Form object, UniForm path or URL is empty." );
        this.LogMethodEnd ( "getSelectionListDownloadPage" );
        return;
      }

      //
      // Define the form template filename.
      //
      formTemplateFilename = this.Session.AdminPageLayout.PageId
         + "-" + this.Session.AdminPageLayout.Title
         + EuPageLayouts.CONST_TEMPLATE_EXTENSION;

      formTemplateFilename = formTemplateFilename.Replace ( " ", "-" );
      formTemplateFilename = formTemplateFilename.ToLower ( );

      this.LogValue ( "formTemplateFilename: " + formTemplateFilename );

      templateUrl = this.UniForm_BinaryServiceUrl +
        formTemplateFilename;

      this.LogValue ( "templateUrl: " + templateUrl );

      //
      // get the CSv selection list data.
      //
      csvData = this.CreateCsvData ( this.Session.AdminPageLayout );

      //
      // Save the form layout to the UniFORM binary repository.
      //
      Evado.Model.EvStatics.Files.saveFile (
        this.UniForm_BinaryFilePath,
        formTemplateFilename,
        csvData );

      //
      // Define the download group.
      //
      pageGroup = PageObject.AddGroup (
        EdLabels.SelectionList_Download_Group_Title,
        Evado.Model.UniForm.EditAccess.Inherited );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;

      groupField = pageGroup.createHtmlLinkField (
        String.Empty,
        formTemplateFilename,
        templateUrl );

      // 
      // Return the client ResultData object to the calling method.
      // 
      this.LogMethodEnd ( "getSelectionListDownloadPage" );
      return;

    }//END getFormTemplateUpload method

    // ==============================================================================
    /// <summary>
    /// This method returns a CSV string for the PageLayout
    /// </summary>
    /// <param name="PageLayout">Evado.Model.Digital.EdPageLayout object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private String CreateCsvData ( EdPageLayout PageLayout )
    {
      this.LogMethod ( "CreateCsvData" );
      //
      // Initialise the methods variables and objects.
      //
      StringBuilder sbCsvData = new StringBuilder ( );
      String outputFormat = "\"{0}\",\"{1}\"\r\n";

      sbCsvData.AppendFormat ( outputFormat, EdPageLayout.FieldNames.PageId, PageLayout.PageId );
      sbCsvData.AppendFormat ( outputFormat, EdPageLayout.FieldNames.Title, PageLayout.Title );
      sbCsvData.AppendFormat ( outputFormat, EdPageLayout.FieldNames.Version, PageLayout.Version );
      sbCsvData.AppendFormat ( outputFormat, EdPageLayout.FieldNames.User_Types, PageLayout.UserTypes );
      sbCsvData.AppendFormat ( outputFormat, EdPageLayout.FieldNames.HeaderContent, PageLayout.HeaderContent );
      sbCsvData.AppendFormat ( outputFormat, EdPageLayout.FieldNames.HeaderComponentList, PageLayout.HeaderComponentList );
      sbCsvData.AppendFormat ( outputFormat, EdPageLayout.FieldNames.LeftColumnContent, PageLayout.LeftColumnContent );
      sbCsvData.AppendFormat ( outputFormat, EdPageLayout.FieldNames.LeftColumnComponentList, PageLayout.LeftColumnComponentList );
      sbCsvData.AppendFormat ( outputFormat, EdPageLayout.FieldNames.LeftColumnWidth, PageLayout.LeftColumnWidth );
      sbCsvData.AppendFormat ( outputFormat, EdPageLayout.FieldNames.CenterColumnContent, PageLayout.CenterColumnContent );
      sbCsvData.AppendFormat ( outputFormat, EdPageLayout.FieldNames.CenterColumnGroupList, PageLayout.CenterColumnComponentList );
      sbCsvData.AppendFormat ( outputFormat, EdPageLayout.FieldNames.RightColumnContent, PageLayout.RightColumnContent );
      sbCsvData.AppendFormat ( outputFormat, EdPageLayout.FieldNames.RightColumnComponentList, PageLayout.RightColumnComponentList );
      sbCsvData.AppendFormat ( outputFormat, EdPageLayout.FieldNames.RightColumnWidth, PageLayout.RightColumnWidth );


      this.LogMethodEnd ( "CreateCsvData" );
      return sbCsvData.ToString ( );

    }

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.AppData object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private void getDataObject_GeneralGroup (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getDataObject_GeneralGroup" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      List<EvOption> optionList = new List<EvOption> ( );

      this.Session.AdminPageLayout.PageCommands = String.Empty;

      // 
      // create the page pageMenuGroup
      // 
      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
       EdLabels.PageLayout_General_Group_Title );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Page_Header;

      //
      // Add the group commands
      //
      this.getDataObject_GroupCommands ( pageGroup, true );

      // 
      // Create the page id object
      // 
      groupField = pageGroup.createTextField (
        EdPageLayout.FieldNames.PageId,
        EdLabels.PageLayout_Page_Id_Field_Label,
        this.Session.AdminPageLayout.PageId, 10 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;
      groupField.Mandatory = true;
      if ( this.Session.AdminPageLayout.Guid != Evado.Model.Digital.EvcStatics.CONST_NEW_OBJECT_ID )
      {
        groupField.EditAccess = Model.UniForm.EditAccess.Disabled;
      }

      // 
      // Create the page title object
      // 
      groupField = pageGroup.createTextField (
        EdPageLayout.FieldNames.Title,
        EdLabels.PageLayout_Title_Field_Label,
        this.Session.AdminPageLayout.Title,
        50 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;
      groupField.Mandatory = true;

      groupField.setBackgroundColor (
        Model.UniForm.FieldParameterList.BG_Mandatory,
        Model.UniForm.Background_Colours.Red );

      // 
      // Create the page default home page indicator object
      // 
      groupField = pageGroup.createBooleanField (
        EdPageLayout.FieldNames.DefaultHomePage,
        EdLabels.PageLayout_DefaultHomePage_Field_Label,
        this.Session.AdminPageLayout.HomePage);
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      // 
      // Create the page display main menu indicator object
      // 
      groupField = pageGroup.createBooleanField (
        EdPageLayout.FieldNames.DisplayMainMenu,
        EdLabels.PageLayout_DisplayMainMenu_Field_Label,
        this.Session.AdminPageLayout.DisplayMainMenu );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      // 
      // Create the user type selection.
      // 
      groupField = pageGroup.createTextField (
        EdPageLayout.FieldNames.DefaultPageEntity,
        EdLabels.PageLayout_Default_Page_Entity_Id_Field_Label,
        EdLabels.PageLayout_Default_Page_Entity_Id_Field_Description,
        this.Session.AdminPageLayout.DefaultPageEntity,
        20 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // create the user type selection list.
      //
      String userSelectionList = this.AdapterObjects.Settings.UserCategoryList;
      optionList = this.AdapterObjects.getSelectionOptions ( userSelectionList, String.Empty, false, false );

      if ( this.Session.UserProfile.hasAdministrationAccess == true )
      {
        optionList.Add ( new EvOption ( "EVADO", "Evado" ) );
        optionList.Add ( new EvOption ( "CUST", "Customer" ) );
      }
      EvStatics.sortOptionListValues ( optionList );



      // 
      // Create the user type selection.
      // 
      groupField = pageGroup.createCheckBoxListField (
        EdPageLayout.FieldNames.User_Types,
        EdLabels.PageLayout_User_Types_Field_Label,
        this.Session.AdminPageLayout.UserTypes,
        optionList );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // create the user type selection list.
      //
      optionList = EvStatics.getOptionsFromEnum( typeof( EdPageLayout.MenuLocations), true );

      // 
      // Create the user type selection.
      // 
      groupField = pageGroup.createSelectionListField (
        EdPageLayout.FieldNames.MenuLocation,
        EdLabels.PageLayout_Menu_Location_Field_Label,
        this.Session.AdminPageLayout.MenuLocation,
        optionList );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // create the user type selection list.
      //
      optionList = EvStatics.getOptionsFromEnum ( typeof ( EdPageLayout.LayoutComponentList ), false );

      // 
      // Create the user type selection.
      // 
      groupField = pageGroup.createCheckBoxListField (
        EdPageLayout.FieldNames.LayoutComponents,
        EdLabels.PageLayout_Component_Field_Label,
        this.Session.AdminPageLayout.ActiveLayoutComponents,
        optionList );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      // 
      // Create the page title object
      // 
      groupField = pageGroup.createTextField (
        EdPageLayout.FieldNames.Version,
        EdLabels.PageLayout_Version_Field_Label,
        this.Session.AdminPageLayout.Version.ToString(),
        10 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;
      groupField.EditAccess = Model.UniForm.EditAccess.Disabled;


      this.LogMethodEnd ( "getDataObject_GeneralGroup" );

    }//END getDataObject_GeneralGroup Method

    // ==============================================================================
    /// <summary>
    /// This method generates the left column properties group
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.AppData object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObjectHeaderGroup (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getDataObjectHeaderGroup" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );

      // 
      // create the page pageMenuGroup
      // 
      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
       EdLabels.PageLayout_Header_Group_Title );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Page_Header;

      //
      // Add the group commands
      //
      this.getDataObject_GroupCommands ( pageGroup, false );

      // 
      // Create the left column (top )content object
      // 
      groupField = pageGroup.createFreeTextField (
        EdPageLayout.FieldNames.HeaderContent,
        EdLabels.PageLayout_Header_Content_Field_Label,
        this.Session.AdminPageLayout.HeaderContent, 25, 10 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      // 
      // Create the left column group identifiers object
      // 
      groupField = pageGroup.createCheckBoxListField (
        EdPageLayout.FieldNames.HeaderComponentList,
        EdLabels.PageLayout_Header_Components_Field_Label,
        this.Session.AdminPageLayout.HeaderComponentList,
         this.AdapterObjects.PageComponents );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      this.LogMethodEnd ( "getDataObjectHeaderGroup" );

    }//END getDataObject_LeftColumnGroup Method

    // ==============================================================================
    /// <summary>
    /// This method generates the left column properties group
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.AppData object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_LeftColumnGroup (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getDataObject_LeftColumnGroup" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );

      // 
      // create the page pageMenuGroup
      // 
      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
       EdLabels.PageLayout_Left_Group_Title );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Dynamic;
      pageGroup.SetPageColumnCode ( Model.UniForm.PageColumnCodes.Left );

      //
      // Add the group commands
      //
      this.getDataObject_GroupCommands ( pageGroup, false );

      // 
      // Create the left column (top )content object
      // 
      groupField = pageGroup.createFreeTextField (
        EdPageLayout.FieldNames.LeftColumnContent,
        EdLabels.PageLayout_Left_Content_Field_Label,
        this.Session.AdminPageLayout.LeftColumnContent, 25, 10 );
      groupField.Layout = Model.UniForm.FieldLayoutCodes.Column_Layout;

      // 
      // Create the left column group identifiers object
      // 
      groupField = pageGroup.createCheckBoxListField (
        EdPageLayout.FieldNames.LeftColumnComponentList,
        EdLabels.PageLayout_Left_Components_Field_Label,
        this.Session.AdminPageLayout.LeftColumnComponentList,
         this.AdapterObjects.PageComponents );
      groupField.Layout = Model.UniForm.FieldLayoutCodes.Column_Layout;

      // 
      // Create the left column command identifier object
      // 
      groupField = pageGroup.createNumericField (
        EdPageLayout.FieldNames.LeftColumnWidth,
        EdLabels.PageLayout_Left_Command_List_Field_Label,
        this.Session.AdminPageLayout.LeftColumnWidth, 0, 33 );
      groupField.Layout = Model.UniForm.FieldLayoutCodes.Column_Layout;

      this.LogMethodEnd ( "getDataObject_LeftColumnGroup" );

    }//END getDataObject_LeftColumnGroup Method

    // ==============================================================================
    /// <summary>
    /// This method generates the center column properties group
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.AppData object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_CenterColumnGroup (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getDataObject_CenterColumnGroup" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );

      // 
      // create the page pageMenuGroup
      // 
      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
       EdLabels.PageLayout_Center_Group_Title );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
      pageGroup.SetPageColumnCode ( Model.UniForm.PageColumnCodes.Body );

      //
      // Add the group commands
      //
      this.getDataObject_GroupCommands ( pageGroup, false );

      // 
      // Create the left column (top )content object
      // 
      groupField = pageGroup.createFreeTextField (
        EdPageLayout.FieldNames.CenterColumnContent,
        EdLabels.PageLayout_Center_Content_Field_Label,
        this.Session.AdminPageLayout.CenterColumnContent, 25, 10 );
      groupField.Layout = Model.UniForm.FieldLayoutCodes.Column_Layout;

      // 
      // Create the left column group identifiers object
      // 
      groupField = pageGroup.createCheckBoxListField (
        EdPageLayout.FieldNames.CenterColumnGroupList,
        EdLabels.PageLayout_Center_Components_Field_Label,
        this.Session.AdminPageLayout.CenterColumnComponentList,
         this.AdapterObjects.PageComponents );
      groupField.Layout = Model.UniForm.FieldLayoutCodes.Column_Layout;

      this.LogMethodEnd ( "getDataObject_CenterColumnGroup" );

    }//END getDataObject_CenterColumnGroup Method

    // ==============================================================================
    /// <summary>
    /// This method generates the right column properties group
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.AppData object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_RightColumnGroup (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getDataObject_RightColumnGroup" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );

      // 
      // create the page pageMenuGroup
      // 
      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
       EdLabels.PageLayout_Right_Group_Title );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Dynamic;
      pageGroup.SetPageColumnCode ( Model.UniForm.PageColumnCodes.Right );

      //
      // Add the group commands
      //
      this.getDataObject_GroupCommands ( pageGroup, false );

      // 
      // Create the left column (top )content object
      // 
      groupField = pageGroup.createFreeTextField (
        EdPageLayout.FieldNames.RightColumnContent,
        EdLabels.PageLayout_Right_Content_Field_Label,
        this.Session.AdminPageLayout.RightColumnContent, 25, 10 );
      groupField.Layout = Model.UniForm.FieldLayoutCodes.Column_Layout;

      // 
      // Create the left column group identifiers object
      // 
      groupField = pageGroup.createCheckBoxListField (
        EdPageLayout.FieldNames.RightColumnComponentList,
        EdLabels.PageLayout_Right_Components_Field_Label,
        this.Session.AdminPageLayout.RightColumnComponentList,
         this.AdapterObjects.PageComponents);
      groupField.Layout = Model.UniForm.FieldLayoutCodes.Column_Layout;

      // 
      // Create the left column command identifier object
      // 
      groupField = pageGroup.createNumericField (
        EdPageLayout.FieldNames.RightColumnWidth,
        EdLabels.PageLayout_Right_Command_List_Field_Label,
        this.Session.AdminPageLayout.RightColumnWidth, 0, 33 );
      groupField.Layout = Model.UniForm.FieldLayoutCodes.Column_Layout;

      this.LogMethodEnd ( "getDataObject_RightColumnGroup" );

    }//END getDataObject_RightColumnGroup Method

    //================================================================================
    /// <summary>
    /// This method add the group commands to the grop.
    /// </summary>
    /// <param name="PageGroup">Evado.Model.UniForm.Group object.</param>
    /// <param name="IsGeneralGroup">True = Display General group commands.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_GroupCommands (
      Evado.Model.UniForm.Group PageGroup,
      bool IsGeneralGroup )
    {
      this.LogMethod ( "getDataObject_GroupCommands" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );

      // 
      // Add the save groupCommand
      // 
      if ( PageGroup.EditAccess == Evado.Model.UniForm.EditAccess.Enabled )
      {
        pageCommand = PageGroup.addCommand (
          EdLabels.PageLayout_Save_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Page_Layouts.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Save_Object );

        // 
        // Define the save and delete groupCommand parameters
        // 
        pageCommand.SetGuid ( this.Session.AdminPageLayout.Guid );
        pageCommand.AddParameter (
           Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
          EdPageLayout.SaveActions.Save.ToString ( ) );

        if ( IsGeneralGroup == false )
        {
          this.LogMethodEnd ( "getDataObject_GroupCommands" );
          return;
        }

        //
        // Issue command
        //
        pageCommand = PageGroup.addCommand (
          EdLabels.PageLayout_Issue_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Page_Layouts.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Save_Object );

        // 
        // Define the save and issue groupCommand parameters
        // 
        pageCommand.SetGuid ( this.Session.AdminPageLayout.Guid );
        pageCommand.AddParameter (
           Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
          EdPageLayout.SaveActions.Issue.ToString ( ) );

        //
        // Delete command
        //
        if ( this.Session.AdminPageLayout.State == EdPageLayout.States.Draft )
        {
          pageCommand = PageGroup.addCommand (
            EdLabels.PageLayout_Delete_Command_Title,
            EuAdapter.ADAPTER_ID,
            EuAdapterClasses.Page_Layouts.ToString ( ),
            Evado.Model.UniForm.ApplicationMethods.Save_Object );

          // 
          // Define the save and delete groupCommand parameters
          // 
          pageCommand.SetGuid ( this.Session.AdminPageLayout.Guid );
          pageCommand.AddParameter (
             Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
            EdPageLayout.SaveActions.Delete.ToString ( ) );
        }
      }

      this.LogMethodEnd ( "getDataObject_GroupCommands" );

    }//END getDataObject_GroupCommands Method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class create object methods

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="Command">Evado.UniForm.Model.ClientClientDataObjectEvado.Model.UniForm.Command object.</param>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData createObject ( Evado.Model.UniForm.Command Command )
    {
      this.LogMethod ( "createObject" );
      try
      {
        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "createObject",
          this.Session.UserProfile );

        // 
        // Initialise the methods variables and objects.
        //      
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );

        //
        // Determine if the user has access to this page and log and error if they do not.
        //
        if ( this.Session.UserProfile.hasAdministrationAccess == false )
        {
          this.LogIllegalAccess ( "getListObject",
            this.Session.UserProfile );

          this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

          return this.Session.LastPage; ;
        }

        //
        // Initialise the dlinical ResultData objects.
        //
        this.Session.AdminPageLayout = new EdPageLayout ( );
        this.Session.AdminPageLayout.Guid = Evado.Model.Digital.EvcStatics.CONST_NEW_OBJECT_ID;
        this.Session.AdminPageLayout.PageId = String.Empty;
        this.Session.AdminPageLayout.Title = String.Empty;
        this.Session.AdminPageLayout.State = EdPageLayout.States.Draft;
        this.Session.AdminPageLayout.UserTypes= String.Empty;
        this.Session.AdminPageLayout.CenterColumnContent= String.Empty;
        this.Session.AdminPageLayout.CenterColumnComponentList = String.Empty;
        this.Session.AdminPageLayout.HeaderContent = String.Empty;
        this.Session.AdminPageLayout.HeaderComponentList = String.Empty;
        this.Session.AdminPageLayout.LeftColumnWidth = 0;
        this.Session.AdminPageLayout.LeftColumnContent = String.Empty;
        this.Session.AdminPageLayout.LeftColumnComponentList = String.Empty;
        this.Session.AdminPageLayout.RightColumnWidth = 0;
        this.Session.AdminPageLayout.RightColumnContent = String.Empty;
        this.Session.AdminPageLayout.RightColumnComponentList = String.Empty;

        this.getDataObject ( clientDataObject );


        this.LogValue ( "Exit createObject method. ID: "
          + clientDataObject.Id + ", Title: " + clientDataObject.Title );

        return clientDataObject;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.PageLayout_Creation_Error_Message;

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
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.ClientClientDataObjectEvado.Model.UniForm.Command object.</param>
    /// <remarks>
    /// This method has following steps:
    /// 
    /// 1. Update the object values from command parameter values.
    /// 
    /// 2. Update the address fields of the customer.
    /// 
    /// 3. Save the updated fields to the respective tables in Evado Database.
    /// </remarks>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData updateObject ( Evado.Model.UniForm.Command PageCommand )
    {
      try
      {
        this.LogMethod ( "updateObject" );
        this.LogDebug ( "PageCommand: " + PageCommand.getAsString ( false, true ) );
        //
        // Initialise the methods variables and objects.
        //
        EdPageLayout.SaveActions saveAction = EdPageLayout.SaveActions.Save;

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "updateObject",
          this.Session.UserProfile );

        // 
        // Initialise the update variables.
        // 
        this.AdapterObjects.AllPageLayouts = new List<EdPageLayout> ( );

        // 
        // IF the guid is new object id  alue then set the save object for adding to the database.
        // 
        if ( this.Session.AdminPageLayout.Guid == Evado.Model.Digital.EvcStatics.CONST_NEW_OBJECT_ID )
        {
          this.Session.AdminPageLayout.Guid = Guid.Empty;
        }

        // 
        // Delete the object.
        // 
        if ( PageCommand.Method == Evado.Model.UniForm.ApplicationMethods.Delete_Object )
        {
          return new Model.UniForm.AppData ( );
        }

        // 
        // Update the object.
        // 
        this.updateObjectValue ( PageCommand.Parameters );


        this.LogDebug ( "AdminSelectionList:" );
        this.LogDebug ( "-Guid: " + this.Session.AdminPageLayout.Guid );
        this.LogDebug ( "-ListId: " + this.Session.AdminPageLayout.PageId );
        this.LogDebug ( "-Title: " + this.Session.AdminPageLayout.Title );
        this.LogDebug ( "-UserType: " + this.Session.AdminPageLayout.UserTypes );

        //
        // check that the mandatory fields have been filed.
        //
        if ( this.updateCheckMandatory ( ) == false )
        {
          this.LogMethodEnd ( "updateObject" );
          return this.Session.LastPage;
        }

        // 
        // Get the save action message value.
        // 
        String stSaveAction = PageCommand.GetParameter ( Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION );

        // 
        // Resets the save action to the parameter passed in the page groupCommand.
        // 
        if ( stSaveAction != String.Empty )
        {
          saveAction = Evado.Model.EvStatics.parseEnumValue<EdPageLayout.SaveActions> ( stSaveAction );
        }
        this.Session.AdminPageLayout.Action = saveAction;

        if ( saveAction == EdPageLayout.SaveActions.Delete )
        {
          this.Session.AdminPageLayout.Title = String.Empty;
        }

        // 
        // update the object.
        // 
        EvEventCodes result = this._Bll_PageLayouts.SaveItem ( this.Session.AdminPageLayout );

        // 
        // get the debug ResultData.
        // 
        this.LogValue ( this._Bll_PageLayouts.Log );

        // 
        // if an error state is returned create log the event.
        // 
        if ( result != EvEventCodes.Ok )
        {
          string StEvent = this._Bll_PageLayouts.Log + " returned error message: " + Evado.Model.Digital.EvcStatics.getEventMessage ( result );
          this.LogError ( EvEventCodes.Database_Record_Update_Error, StEvent );

          switch ( result )
          {
            case EvEventCodes.Data_Duplicate_Id_Error:
              {
                this.ErrorMessage =
                  String.Format (
                    EdLabels.OageLayout_Duplicate_Error_Message,
                    this.Session.AdminPageLayout.PageId );
                break;
              }
            default:
              {
                this.ErrorMessage = EdLabels.PageLayout_Update_Error_Message;
                break;
              }
          }

          this.LogMethodEnd ( "updateObject" );
          return this.Session.LastPage;
        }//END save error returned.

        //
        // empty the selection lists to force a reload on the next initialisation.
        //
        this.AdapterObjects.AllPageLayouts = new List<EdPageLayout> ( );
        if ( this.Session.AdminPageLayout.Action == EdPageLayout.SaveActions.Issue )
        {
          this.AdapterObjects.PageComponents = new List<EvOption> ( );
        }

        this.LogMethodEnd ( "updateObject" );
        return new Model.UniForm.AppData ( );

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.PageLayout_Update_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }
      this.LogMethodEnd ( "updateObject" );
      return this.Session.LastPage;

    }//END method

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns></returns>
    //  ----------------------------------------------------------------------------------
    private bool updateCheckMandatory ( )
    {
      this.LogMethod ( "updateCheckMandatory" );
      //
      // Define the methods variables and objects.
      //
      bool bReturn = true;
      this.ErrorMessage = String.Empty;

      //
      // Org name not defined.
      //
      if ( this.Session.AdminPageLayout.PageId == String.Empty )
      {
        if ( this.ErrorMessage != String.Empty )
        {
          this.ErrorMessage += "\r\n ";
        }
        this.ErrorMessage += EdLabels.PageLayout_Page_Id_Error_Message;

        bReturn = false;
      }

      //
      // Org name not defined.
      //
      if ( this.Session.AdminPageLayout.Title == String.Empty )
      {
        if ( this.ErrorMessage != String.Empty )
        {
          this.ErrorMessage += "\r\n ";
        }
        this.ErrorMessage += EdLabels.PageLayout_Title_Error_Message;

        bReturn = false;
      }

      return bReturn;
    }

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="Parameters">List of field values to be updated.</param>
    /// <returns></returns>
    //  ----------------------------------------------------------------------------------
    private void updateObjectValue (
      List<Evado.Model.UniForm.Parameter> Parameters )
    {
      this.LogMethod ( "updateObjectValue" );
      this.LogDebug ( "Parameters.Count: " + Parameters.Count );
      this.LogDebug ( "Customer.Guid: " + this.Session.AdminPageLayout.Guid );

      /// 
      /// Iterate through the parameter values updating the ResultData object
      /// 
      foreach ( Evado.Model.UniForm.Parameter parameter in Parameters )
      {
        if ( parameter.Name.Contains ( Evado.Model.Digital.EvcStatics.CONST_GUID_IDENTIFIER ) == false
          && parameter.Name != Evado.Model.UniForm.CommandParameters.Custom_Method.ToString ( )
          && parameter.Name != Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION )
        {
          this.LogDebug ( parameter.Name + " > " + parameter.Value + " >> UPDATED" );
          try
          {
            EdPageLayout.FieldNames fieldName =
               Evado.Model.EvStatics.parseEnumValue<EdPageLayout.FieldNames> (
              parameter.Name );

            this.Session.AdminPageLayout.setValue ( fieldName, parameter.Value );

          }
          catch ( Exception Ex )
          {
            this.LogException ( Ex );
          }
        }
        else
        {
          this.LogDebug ( parameter.Name + " > " + parameter.Value + " >> SKIPPED" );
        }

      }// End iteration loop

    }//END updateObjectValue method.

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
  }
}//END namespace