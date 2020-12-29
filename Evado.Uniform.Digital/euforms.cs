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
  public class EuForms : EuClassAdapterBase
  {
    #region Class Initialisation
    /// <summary>
    /// This method initialises the class.
    /// </summary>
    public EuForms ( )
    {
      this.ClassNameSpace = this.ClassNameSpace + "";
    }

    /// <summary>
    /// This method initialises the class and passs in the user profile.
    /// </summary>
    public EuForms (
      EuApplicationObjects ApplicationObjects,
      EvUserProfileBase ServiceUserProfile,
      EuSession SessionObjects,
      String UniFormBinaryFilePath,
      EvClassParameters Settings )
    {
      this.ApplicationObjects = ApplicationObjects;
      this.ServiceUserProfile = ServiceUserProfile;
      this.Session = SessionObjects;
      this.UniForm_BinaryFilePath = UniFormBinaryFilePath;
      this.ClassParameters = Settings;
      this.ClassNameSpace = "Evado.UniForm.Clinical.EuForms.";


      this.LogInitMethod ( "EuForms initialisation" );
      this.LogInit ( "-ServiceUserProfile.UserId: " + ServiceUserProfile.UserId );
      this.LogInit ( "-SessionObjects.Project.ProjectId: " + this.Session.Application.ApplicationId );
      this.LogInit ( "-SessionObjects.UserProfile.CommonName: " + this.Session.UserProfile.CommonName );
      this.LogInit ( "-UniFormBinaryFilePath: " + this.UniForm_BinaryFilePath );

      this.LogInit ( "Settings:" );
      this.LogInit ( "-LoggingLevel: " + Settings.LoggingLevel );
      this.LogInit ( "-UserId: " + Settings.UserProfile.UserId );
      this.LogInit ( "-UserCommonName: " + Settings.UserProfile.CommonName );

      this._Bll_Forms = new EvForms ( Settings );

    }//END Method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class constants and variables.

    private Evado.Bll.Clinical.EvForms _Bll_Forms = new Evado.Bll.Clinical.EvForms ( );
    private EvServerPageScript _ServerPageScript = new EvServerPageScript ( );

    public const string CONST_REFRESH = "RFSH";
    public const string CONST_SECTION_FIELD_PREFIX = "FSCTN";
    public const string CONST_UPDATE_SECTION_COMMAND_PARAMETER = "UPSECT";
    public const string CONST_FORM_COMMAND_PARAMETER = "CmdActn";
    public const string CONST_REVISE_FORM_COMMAND_VALUE = "REVISE";
    public const string CONST_COPY_FORM_COMMAND_VALUE = "COPY";
    public const string CONST_EXPORT_FORM_DATA = "EXPORT";
    public const string CONST_IMPORT_FORM_DATA = "IMPORT";
    public const string CONST_SAVE_ACTION_PARM = "ACTION";
    public const string CONST_PARTICIPANT_SIGNATURE_FIELD_ID = "PARTSIG";
    public const string CONST_CONFIRM_CONSENT_FIELD_ID = "PCTC";
    public const string CONST_CONFIRM_DATA_CONSENT_STATUS = "PDCST";
    public const string CONST_CONFIRM_SHARE_CONSENT_STATUS = "PSCST";
    public const string CONST_CONFIRM_CONFIRM_CONSENT_STATUS = "PCCST";

    public const string CONST_TEMPLATE_FIELD_ID = "IFTF";

    public const string CONST_TEMPLATE_EXTENSION = ".evado-form.xml";


    public const string ICON_FORM_DRAFT = "icons/form-draft.png";
    public const string ICON_FORM_REVIEWED = "icons/form-reviewed.png";
    public const string ICON_FORM_ISSUED = "icons/form-issued.png";

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class public methods

    // ===============================================================================
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
      this.LogMethod ( "getClientDataObject" );
      this.LogValue ( "PageCommand: " + PageCommand.getAsString ( false, true ) );

      try
      {
        // 
        // Initialise the methods variables and objects.
        // 
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );

        //
        // Retrieve the groupCommand parameters to determine the type of page to generate.
        //
        String stPageType = PageCommand.GetParameter (
          Evado.Model.UniForm.CommandParameters.Page_Id );
        String stCommandAction = PageCommand.GetParameter ( EuForms.CONST_FORM_COMMAND_PARAMETER );

        this.LogValue ( "stCommandAction: " + stCommandAction );

        if ( stPageType != String.Empty )
        {
          this.Session.PageId = Evado.Model.EvStatics.Enumerations.parseEnumValue<EvPageIds> ( stPageType );
        }
        this.LogValue ( "SessionObjects.PageType: " + this.Session.PageId );


        // 
        // Determine the method to be called
        // 
        switch ( PageCommand.Method )
        {
          // 
          // Generate a page containing a list of record commands
          // 
          case Evado.Model.UniForm.ApplicationMethods.List_of_Objects:
            {
              //
              // Call the copy form method.
              // Method exited if the groupCommand parameter is not a form revision or copy value.
              //
              this.copyForm ( PageCommand );

              //
              // call the lift object method.
              //
              clientDataObject = this.getListObject ( PageCommand );
              break;

            }//END get list of objects case

          // 
          // Select the method to retrieve a record object.
          // 
          case Evado.Model.UniForm.ApplicationMethods.Get_Object:
            {
              //
              // Reset global parameters for opening the form.
              //
              this.Session.Form.SaveAction = EdRecord.SaveActionCodes.Save;
              if ( this.Session.Form.ApplicationId != this.Session.Application.ApplicationId )
              {
                this.Session.Form = new EdRecord ( );
                this.Session.FormField = new EdRecordField ( );
              }

              //
              // Display the form properties page if the view type set to property page.
              //
              switch ( this.Session.PageId )
              {
                case EvPageIds.Form_Properties_Page:
                  {
                    clientDataObject = this.getFormPropertiesObject ( PageCommand );
                    break;
                  }
                case EvPageIds.Form_Properties_Section_Page:
                  {
                    clientDataObject = this.getFormPropertiesSectionObject ( PageCommand );
                    break;
                  }
                case EvPageIds.Form_Draft_Page:
                  {
                    clientDataObject = this.getFormDraftLayoutObject ( PageCommand );
                    break;
                  }
                case EvPageIds.Form_Template_Upload:
                  {
                    clientDataObject = this.getFormTemplateUploadPage ( PageCommand );
                    break;
                  }
                case EvPageIds.Form_Template_Download:
                  {
                    clientDataObject = this.getFormTemplateDownloadPage ( PageCommand );
                    break;
                  }
                default:
                  {
                    clientDataObject = this.getFormLayoutObject ( PageCommand );
                    break;
                  }
              }
              break;

            }//END get object case

          // 
          // Select the groupCommand to create a new record object.
          // 
          case Evado.Model.UniForm.ApplicationMethods.Create_Object:
            {
              clientDataObject = this.createObject ( PageCommand ); ;

              this.LogDebug ( Evado.Model.Digital.EvcStatics.SerialiseObject<Evado.Model.UniForm.AppData> ( clientDataObject ) );

              break;
            }//END create case

          // 
          // Select the method to update the record object.
          // 
          case Evado.Model.UniForm.ApplicationMethods.Save_Object:
          case Evado.Model.UniForm.ApplicationMethods.Delete_Object:
            {
              this.LogValue ( " Save Object method" );

              // 
              // Update the object values
              // 
              clientDataObject = this.updateObject ( PageCommand );

              break;

            }//END save case.

        }//END Switch

        // 
        // Handle returned exceptions.
        // 
        if ( clientDataObject == null )
        {
          this.LogValue ( " null application data returned." );

          this.LogError ( EvEventCodes.Data_Empty_Error, this.ClassNameSpace + "\r\n" +
            this.Log );
        }

        // 
        // return the client ResultData object.
        // 
        this.LogMethodEnd ( "getClientDataObject" );
        return clientDataObject;

      }
      catch ( Exception Ex )
      {
        // 
        // If an exception is created log the error and return an error.
        // 
        this.LogException ( Ex );
      }

      this.LogMethodEnd ( "getClientDataObject" );
      return new Evado.Model.UniForm.AppData ( );

    }//END getRecordObject methods

    // ==================================================================================
    /// <summary>
    /// This method Saves the clinical objects to user session object.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public void SaveSessionObjects ( )
    {
      this.LogMethod ( "SaveSessionObjects" );
      // 
      // Save the session ResultData so it is available for the next user generated groupCommand.
      // 

    }//END SaveSessionObjects method.

    //  =============================================================================== 
    /// <summary>
    /// runServerScript  method
    /// 
    /// Description:
    ///  This method initiates the execution of the server side CS scripts.
    /// 
    /// </summary>
    //  ---------------------------------------------------------------------------------
    private bool runServerScript ( EvServerPageScript.ScripEventTypes ScriptType )
    {
      this.LogMethod ( "runServerScript" );
      this.LogValue ( "RecordId " + this.Session.Form.LayoutId );
      this.LogValue ( "hasCsScript = " + this.Session.Form.Design.hasCsScript );

      // 
      // if the formField has a CS Script execute the onPostBackForm method.
      // 
      if ( this.Session.Form.Design.hasCsScript == true )
      {
        this.LogValue ( "Server script executing." );

        //
        // Define the page to retrieve the script
        //
        this._ServerPageScript.CsScriptPath = this.ApplicationObjects.ApplicationPath + @"csscripts\";


        // 
        // Execute the onload script.
        // 
        EvEventCodes iReturn = this._ServerPageScript.runScript (
          ScriptType,
          this.Session.Form );

        this.LogValue ( "Server page script debug log: " + this._ServerPageScript.DebugLog );

        this.LogValue ( "Form.ScriptMessage: " + this.Session.Form.ScriptMessage );

        if ( iReturn != EvEventCodes.Ok )
        {
          this.ErrorMessage =
            "CsScript:" + ScriptType + " method failed \r\n"
            + Evado.Model.Digital.EvcStatics.Enumerations.enumValueToString ( iReturn ) + "\r\n";

          this.LogError ( EvEventCodes.Business_Logic_General_Process_Error,
            this.ErrorMessage );

          return false;

        }//END processing error return 

      }//END processing Cs formField script.

      this.LogValue ( "Exit runServerScript processing " );

      return true;

    }//END runServerScript method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class form list methods

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <remarks>
    /// 1.This method queries the database to fetch the list of forms based on project Id, form state and form type.
    /// 
    /// 2. The retreived records are assigned to a selection group where the desired form can be selected.
    /// </remarks>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData getListObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      try
      {
        this.LogMethod ( "getListObject" );

        // 
        // Initialise the methods variables and objects.
        // 
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
        this.Session.FormList = new List<EdRecord> ( );
        this.Session.PageId = EvPageIds.Null;

        // 
        // If the user does not have monitor or ResultData manager roles exit the page.
        // 
        if ( this.Session.UserProfile.hasTrialManagementAccess == false )
        {
          this.LogIllegalAccess (
            this.ClassNameSpace + "getListObject",
            this.Session.UserProfile );

          this.ErrorMessage = EvLabels.Record_Access_Error_Message;

          return this.Session.LastPage;
        }

        // 
        // Log the user's access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "getListObject",
          this.Session.UserProfile );

        this.updateSelectionValues ( PageCommand );

        // 
        // Initialise the client ResultData object.
        // 
        clientDataObject.Id = Guid.NewGuid ( );
        clientDataObject.Page.Id = clientDataObject.Id;
        clientDataObject.Page.PageDataGuid = clientDataObject.Id;
        clientDataObject.Title = EvLabels.Form_Selection_Page_Title;
        clientDataObject.Page.Title = clientDataObject.Title;
        clientDataObject.Page.PageId = EvPageIds.Form_View.ToString ( );

        //
        // Define the icon urls.
        //
        clientDataObject.Page.setImageUrl (
          Model.UniForm.PageImageUrls.Image0_Url,
          EuForms.ICON_FORM_DRAFT );

        clientDataObject.Page.setImageUrl (
          Model.UniForm.PageImageUrls.Image1_Url,
          EuForms.ICON_FORM_REVIEWED );

        clientDataObject.Page.setImageUrl (
          Model.UniForm.PageImageUrls.Image2_Url,
          EuForms.ICON_FORM_ISSUED );

        //
        // Define the page commands
        //
        this.createFormList_PageCommands ( clientDataObject.Page );
        // 
        // Create the new pageMenuGroup for query selection.
        // 
        this.createFormSelectionGroup ( clientDataObject.Page );

        //
        // execute the form list query.
        //
        this.loadTrialFormList ( );

        // 
        // Create the pageMenuGroup containing commands to open the records.
        // 
        this.createFormList_Group ( clientDataObject.Page, this.Session.FormList );

        this.LogValue ( " data.Title: " + clientDataObject.Title );
        this.LogValue ( " data.Page.Title: " + clientDataObject.Page.Title );

        this.Session.FormsAdaperLoaded = false;

        return clientDataObject;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Record_View_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return null;

    }//END getListObject method.

    //===================================================================================
    /// <summary>
    /// This method loads the form list.
    /// </summary>
    //-----------------------------------------------------------------------------------
    private void loadTrialFormList ( )
    {
      this.LogMethod ( "loadTrialFormList method" );
      this.LogDebug ( "TrialId: '" + this.Session.Application.ApplicationId + "'" );

      if ( this.Session.Application.ApplicationId == String.Empty )
      {
        this.LogDebug ( "Trial not defined" );
        this.LogMethod ( "loadTrialFormList" );
        return;
      }

      if ( this.Session.FormList.Count > 0  
        && this.Session.FormsAdaperLoaded  == false ) 
      {
        this.LogMethod ( "loadTrialFormList method" );
        return;
      }

      //
      // Set the form display to the default.
      //
      if ( this.Session.FormsAdaperLoaded == true )
      {
        this.Session.FormType = EvFormRecordTypes.Null;
        this.Session.FormState = EdRecordObjectStates.Null;
      }

      this.LogDebug ( "FormState: '" + this.Session.FormState + "'" );
      this.LogDebug ( "FormType: '" + this.Session.FormType + "'" );

      // 
      // Query the database to retrieve a list of the records matching the query parameter values.
      // 
      this.Session.FormList = this._Bll_Forms.GetFormList (
        this.Session.Application.ApplicationId,
        this.Session.FormType,
        this.Session.FormState );

      this.LogDebugClass ( this._Bll_Forms.Log );
      this.LogDebug ( "Form list count: " + this.Session.FormList.Count );

      this.Session.FormsAdaperLoaded = false;

      this.LogMethod ( "loadTrialFormList method" );
    }//ENd loadTrialFormList method

    // ==============================================================================
    /// <summary>
    /// This method copy or revises the selected form.
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    //  ------------------------------------------------------------------------------
    private void copyForm (
      Evado.Model.UniForm.Command PageCommand )
    {
      try
      {
        this.LogMethod ( "copyForm" );

        // 
        // Initialise the methods variables and objects.
        // 
        EvForms forms = new EvForms ( );
        String stCommandAction = PageCommand.GetParameter ( EuForms.CONST_FORM_COMMAND_PARAMETER );

        //
        // if the groupCommand action is not a revise or copy groupCommand exit.
        //
        if ( stCommandAction != EuForms.CONST_COPY_FORM_COMMAND_VALUE
          && stCommandAction != EuForms.CONST_REVISE_FORM_COMMAND_VALUE )
        {
          this.LogValue ( "EXIT: not a revision or copy command." );

          return;
        }

        // 
        // If the user does not have monitor or ResultData manager roles exit the page.
        // 
        if ( this.Session.UserProfile.hasTrialManagementAccess == false )
        {
          this.LogIllegalAccess (
            this.ClassNameSpace + "getListObject",
            this.Session.UserProfile );

          this.ErrorMessage = EvLabels.Record_Access_Error_Message;

          return;
        }

        // 
        // Log the user's access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "getListObject",
          this.Session.UserProfile );

        //
        // get the form guid.
        //
        Guid formGuid = PageCommand.GetGuid ( );
        this.LogValue ( " recordGuid: " + formGuid );

        // 
        // if the guid is empty the parameter was not found to exit.
        // 
        if ( formGuid == Guid.Empty )
        {
          this.LogValue ( "formGuid is EMPTY" );

          return;
        }

        // 
        // Retrieve the record object from the database via the DAL and BLL layers.
        // 
        this.Session.Form = this._Bll_Forms.getForm ( formGuid );

        this.LogValue ( this._Bll_Forms.Log );

        this.LogValue ( "SessionObjects.Form.FormId: " + this.Session.Form.LayoutId );

        this.Session.Form.UpdatedByUserId = this.Session.UserProfile.UserId;
        this.Session.Form.UserCommonName = this.Session.UserProfile.CommonName;

        //
        // Call the method to revise the form layout.
        //
        if ( stCommandAction == EuForms.CONST_REVISE_FORM_COMMAND_VALUE )
        {
          this._Bll_Forms.ReviseForm ( this.Session.Form, this.Session.UserProfile.CommonName );
        }

        //
        // call the method to copy the form layout.
        //
        if ( stCommandAction == EuForms.CONST_COPY_FORM_COMMAND_VALUE )
        {
          this._Bll_Forms.CopyForm ( this.Session.Form, this.Session.UserProfile.CommonName );
        }

        //
        // If a revision or copy is made rebuild the list
        //
        this.Session.FormList = new List<EdRecord> ( );

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Record_View_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

    }//END getListObject method.

    // ==============================================================================
    /// <summary>
    /// This method creates the form selection pageMenuGroup.
    /// </summary>
    /// <returns>Evado.Model.UniForm.Group</returns>
    //  ------------------------------------------------------------------------------
    private void createFormSelectionGroup (
      Evado.Model.UniForm.Page ClientPage )
    {
      this.LogMethod ( "getList_PageComands" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Model.UniForm.Group ( );
      Evado.Model.UniForm.Command command = new Model.UniForm.Command ( );
      Evado.Model.UniForm.Field selectionField = new Model.UniForm.Field ( );
      List<EvOption> optionList = new List<EvOption> ( );

      // 
      // Create the new pageMenuGroup for query selection.
      // 
      pageGroup = ClientPage.AddGroup (
        EvLabels.Form_Selection_Group_Title,
        Evado.Model.UniForm.EditAccess.Enabled );

      pageGroup.GroupType = Evado.Model.UniForm.GroupTypes.Default;
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
      pageGroup.AddParameter ( Model.UniForm.GroupParameterList.Offline_Hide_Group, true );

      // 
      // Display form type selection list
      // 
      optionList = EdRecord.getFormTypes ( false );
      optionList [ 0 ].Value = EvFormRecordTypes.Null.ToString ( );

      selectionField = pageGroup.createSelectionListField (
        EdRecord.FormClassFieldNames.TypeId.ToString ( ),
       EvLabels.Form_Type_Selection_Label,
       this.Session.FormType.ToString ( ),
       optionList );

      selectionField.Layout = EuFormGenerator.ApplicationFieldLayout;
      selectionField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );

      // 
      // Display form type selection list
      // 
      optionList = EdRecord.getFormStates ( );
      optionList [ 0 ].Value = EvFormRecordTypes.Null.ToString ( );

      selectionField = pageGroup.createSelectionListField (
        EdRecord.FormClassFieldNames.Status.ToString ( ),
        EvLabels.Form_State_Selection_Label,
        this.Session.FormState.ToString ( ),
        optionList );

      selectionField.Layout = EuFormGenerator.ApplicationFieldLayout;
      selectionField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );


      // 
      // Add the selection groupCommand
      // 
      command = pageGroup.addCommand ( EvLabels.Select_Records_Command_Title,
        EuAdapter.APPLICATION_ID,
        EuAdapterClasses.Project_Forms.ToString ( ),
         Evado.Model.UniForm.ApplicationMethods.Custom_Method );
      command.setCustomMethod ( Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

    }//END createRecordSelectionGroup method

    // ==============================================================================
    /// <summary>
    /// This method updates the session objects..
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    //  ------------------------------------------------------------------------------
    private void updateSelectionValues (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "updateSelectionValues" );
      // 
      // Initialise the methods variables and objects.
      // 
      String parameterValue = String.Empty;

      // 
      // Get the form record type parameter value
      // 
      if ( PageCommand.hasParameter ( EdRecord.FormClassFieldNames.TypeId.ToString ( ) ) == true )
      {
        parameterValue = PageCommand.GetParameter ( EdRecord.FormClassFieldNames.TypeId.ToString ( ) );

        this.LogValue ( "Selected Form Type: " + parameterValue );

        this.Session.FormType = Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue<EvFormRecordTypes> ( parameterValue );
      }
      this.LogValue ( "SessionObjects.FormType: "
        + this.Session.FormType );

      // 
      // Get the form record type parameter value
      // 
      if ( PageCommand.hasParameter ( EdRecord.FormClassFieldNames.Status.ToString ( ) ) == true )
      {
        parameterValue = PageCommand.GetParameter ( EdRecord.FormClassFieldNames.Status.ToString ( ) );

        this.LogValue ( "Selected Form Type: " + parameterValue );

        this.Session.FormState = Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue<EdRecordObjectStates> ( parameterValue );
      }
      this.LogValue ( "SessionObjects.FormState: "
        + this.Session.FormState );

    }//END updateSessionObjects method

    // ==============================================================================
    /// <summary>
    /// This method creates the record view pageMenuGroup containing a list of commands to 
    /// open the form record.
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page object.</param>
    /// <param name="FormList">List<EvForm> object.</param>
    /// <returns>Evado.Model.UniForm.Group</returns>
    //  ------------------------------------------------------------------------------
    private void createFormList_PageCommands (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "createFormList_PageCommands" );

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Parameter parameter = new Evado.Model.UniForm.Parameter ( );
      Evado.Model.UniForm.Command pageCommand = new Model.UniForm.Command ( );

      pageCommand = Page.addCommand (
        EvLabels.Form_Template_Upload_Command_Title,
              EuAdapter.APPLICATION_ID,
              EuAdapterClasses.Project_Forms.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.Get_Object );

      // 
      // Define the save groupCommand parameters.
      // 
      pageCommand.SetPageId ( EvPageIds.Form_Template_Upload );
    }

    // ==============================================================================
    /// <summary>
    /// This method creates the record view pageMenuGroup containing a list of commands to 
    /// open the form record.
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page object.</param>
    /// <param name="FormList">List<EvForm> object.</param>
    /// <returns>Evado.Model.UniForm.Group</returns>
    //  ------------------------------------------------------------------------------
    private void createFormList_Group (
      Evado.Model.UniForm.Page Page,
      List<EdRecord> FormList )
    {
      this.LogMethod ( "createFormList_Group" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Model.UniForm.Group ( );
      Evado.Model.UniForm.Command groupCommand = new Model.UniForm.Command ( );

      // 
      // Create the record display pageMenuGroup.
      // 
      pageGroup = Page.AddGroup (
        EvLabels.Form_List_Label,
        Evado.Model.UniForm.EditAccess.Enabled );
      pageGroup.CmdLayout = Evado.Model.UniForm.GroupCommandListLayouts.Vertical_Orientation;

      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      pageGroup.Title += EvLabels.List_Count_Label + FormList.Count;

      //
      // Add new form button if the user has configuration write access.
      //
      if ( this.Session.UserProfile.hasConfigrationEditAccess == true )
      {
        //
        // Add the create new form page groupCommand.
        //
        groupCommand = pageGroup.addCommand (
          EvLabels.Form_List_New_Form_Command_Title,
          EuAdapter.APPLICATION_ID,
          EuAdapterClasses.Project_Forms.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Create_Object );

        groupCommand.SetBackgroundColour (
          Model.UniForm.CommandParameters.BG_Default,
          Model.UniForm.Background_Colours.Purple );

      }//END user had configurtion write access

      // 
      // Iterate through the record list generating a groupCommand to access each record
      // then append the groupCommand to the record pageMenuGroup view's groupCommand list.
      // 
      foreach ( Evado.Model.Digital.EdRecord form in FormList )
      {
        EuAdapterClasses appObject = EuAdapterClasses.Project_Forms;

        groupCommand = pageGroup.addCommand (
          form.LayoutId,
          EuAdapter.APPLICATION_ID,
          appObject.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Get_Object );

        groupCommand.AddParameter ( Model.UniForm.CommandParameters.Short_Title, form.LayoutId );

        groupCommand.SetGuid ( form.Guid );

        groupCommand.Title = String.Empty;
        //
        // Switch statement to select the correct ICON.
        //
        switch ( form.State )
        {
          case EdRecordObjectStates.Form_Draft:
            {
              groupCommand.AddParameter (
               Model.UniForm.CommandParameters.Image_Url,
               EuForms.ICON_FORM_DRAFT );
              break;
            }
          case EdRecordObjectStates.Form_Reviewed:
            {
              groupCommand.AddParameter (
               Model.UniForm.CommandParameters.Image_Url,
               EuForms.ICON_FORM_REVIEWED );
              break;
            }
          case EdRecordObjectStates.Form_Issued:
            {
              groupCommand.AddParameter (
               Model.UniForm.CommandParameters.Image_Url,
               EuForms.ICON_FORM_ISSUED );
              break;
            }
        }//END state switch.

        groupCommand.Title += form.LayoutId
          + EvLabels.Space_Hypen
          + form.Title
          + EvLabels.Space_Open_Bracket
          + EvLabels.Label_Version
          + form.Version
          + EvLabels.Space_Close_Bracket;

      }//END iteration loop

      this.LogValue ( "Group command count: " + pageGroup.CommandList.Count );

    }//END createFormList_Group method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class form template upload methods

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData getFormTemplateDownloadPage (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getFormTemplateDownloadPage" );
      this.LogValue ( "UniForm_BinaryFilePath: " + this.UniForm_BinaryFilePath );

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      Evado.Model.UniForm.Parameter parameter = new Evado.Model.UniForm.Parameter ( );
      String xmlText = String.Empty;
      String templateUrl = String.Empty;
      String formTemplateFilename = String.Empty;

      //
      // Determine if the user has access to this page and log and error if they do not.
      //
      if ( this.Session.UserProfile.hasTrialManagementAccess == false )
      {
        this.LogIllegalAccess (
          this.ClassNameSpace + "getFormTemplateDownloadPage",
          this.Session.UserProfile );

        this.ErrorMessage = EvLabels.Illegal_Page_Access_Attempt;

        return null;
      }

      // 
      // Log access to page.
      // 
      this.LogPageAccess (
        this.ClassNameSpace + "getFormTemplateDownloadPage",
        this.Session.UserProfile );

      //
      // exist if the form object is null.
      //
      if ( this.Session.Form == null )
      {
        this.LogValue ( " Form object is null" );
        return null;
      }

      //
      // exist if the form object is null.
      //
      if ( this.Session.Form.Guid == Guid.Empty
        || this.UniForm_BinaryFilePath == String.Empty
        || this.UniForm_BinaryServiceUrl == String.Empty )
      {
        this.LogValue ( " Form object, UniForm path or URL is empty." );

        return null;
      }

      //
      // Define the form template filename.
      //
      formTemplateFilename =
         this.Session.Application.ApplicationId
         + "-" + this.Session.Form.LayoutId
         + "-" + this.Session.Form.Title
         + "-ver-" + this.Session.Form.Design.Version
         + "-" + this.Session.Form.stApprovalDate
         + EuForms.CONST_TEMPLATE_EXTENSION;

      formTemplateFilename = formTemplateFilename.Replace ( " ", "-" );
      formTemplateFilename = formTemplateFilename.ToLower ( );

      this.LogValue ( "formTemplateFilename: " + formTemplateFilename );

      templateUrl = this.UniForm_BinaryServiceUrl +
        formTemplateFilename;

      this.LogValue ( "templateUrl: " + templateUrl );
      //
      // Serialise the form layout.
      //
      xmlText = Evado.Model.EvStatics.SerialiseObject<EdRecord> (
        this.Session.Form );

      //
      // Save the form layout to the UniFORM binary repository.
      //
      Evado.Model.EvStatics.Files.saveFile (
        this.UniForm_BinaryFilePath,
        formTemplateFilename,
        xmlText );

      // 
      // Initialise the client ResultData object.
      // 
      clientDataObject.Id = Guid.NewGuid ( );
      clientDataObject.Title = EvLabels.Form_Template_Page_Title;
      clientDataObject.Page.Id = clientDataObject.Id;
      clientDataObject.Page.PageDataGuid = clientDataObject.Id;

      //
      // Define the general properties pageMenuGroup..
      //
      pageGroup = clientDataObject.Page.AddGroup (
        String.Empty,
        Evado.Model.UniForm.EditAccess.Inherited_Access );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;

      groupField = pageGroup.createHtmlLinkField (
        String.Empty,
        formTemplateFilename,
        templateUrl );

      // 
      // Return the client ResultData object to the calling method.
      // 
      return clientDataObject;

    }//END getFormTemplateUpload method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData getFormTemplateUploadPage (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getFormTemplateUploadPage" );

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
      this.Session.Form = new EdRecord ( );
      Guid formGuid = Guid.Empty;
      EdRecord uploadForm = new EdRecord ( );

      //
      // Initialise the form template filename variable.
      //
      if ( this.Session.UploadFileName == null )
      {
        this.Session.UploadFileName = String.Empty;
      }

      //
      // Determine if the user has access to this page and log and error if they do not.
      //
      if ( this.Session.UserProfile.hasTrialManagementAccess == false )
      {
        this.LogIllegalAccess (
          this.ClassNameSpace + "getFormTemplateUpload",
          this.Session.UserProfile );

        this.ErrorMessage = EvLabels.Illegal_Page_Access_Attempt;

        return null;
      }

      // 
      // Log access to page.
      // 
      this.LogPageAccess (
        this.ClassNameSpace + "getFormTemplateUpload",
        this.Session.UserProfile );

      try
      {
        //
        // read in the form template upload filename.
        //
        string value = PageCommand.GetParameter ( EuForms.CONST_TEMPLATE_FIELD_ID );

        this.LogValue ( "Upload filename: " + value );

        if ( value != string.Empty )
        {
          this.Session.UploadFileName = value;
        }
        this.LogValue ( "FormTemplateFilename: " + this.Session.UploadFileName );

        // 
        // Initialise the client ResultData object.
        // 
        clientDataObject.Id = Guid.NewGuid ( );
        clientDataObject.Title = EvLabels.Form_Template_Page_Title;
        clientDataObject.Page.Id = clientDataObject.Id;
        clientDataObject.Page.PageDataGuid = clientDataObject.Id;

        //
        // if the form template filename is empty display the selection field.
        //
        if ( this.Session.UploadFileName == String.Empty )
        {
          this.LogValue ( "FormTemplateFilename is empty" );

          this.getTemplateUploadDataObject ( clientDataObject );
        }
        else
        {
          this.LogValue ( "Processing the uploaded file." );

          this.getTemplateUpload_Group ( clientDataObject );
        }

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
        this.ErrorMessage = EvLabels.Record_Retrieve_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return null;

    }//END getFormTemplateUpload method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="ClientDataObject">Evado.Model.UniForm.AppData object.</param>
    //  ------------------------------------------------------------------------------
    private void getTemplateUpload_Group (
      Evado.Model.UniForm.AppData ClientDataObject )
    {
      this.LogMethod ( "getTemplateUpload_Group" );

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Guid formGuid = Guid.Empty;
      EdRecord uploadForm = new EdRecord ( );

      //
      // Define the general properties pageMenuGroup..
      //
      pageGroup = ClientDataObject.Page.AddGroup (
        EvLabels.Form_Template_Upload_Log_Group_Title,
        Evado.Model.UniForm.EditAccess.Inherited_Access );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;

      //
      // Upload the the form by it form file name.
      //
      uploadForm = Evado.Model.EvStatics.Files.readXmlFile<EdRecord> (
        this.UniForm_BinaryFilePath,
        this.Session.UploadFileName );

      this.LogValue ( "Uploaded form is: " + uploadForm.LayoutId );

      //
      // save the uploaded form.
      //
      String processLog = this.saveUploadForm ( uploadForm );

      this.LogValue ( "processLog: " + processLog );

      pageGroup.Description = processLog ;

      //
      // reset the form template filename.
      //
      this.Session.UploadFileName = String.Empty;


    }//END getPropertiesDataObject Method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="ClientDataObject">Evado.Model.UniForm.AppData object.</param>
    //  ------------------------------------------------------------------------------
    private String saveUploadForm (
      EdRecord UploadedForm )
    {
      this.LogMethod ( "saveUploadeForm" );
      this.LogValue ( "Uploaded form is: " + UploadedForm.LayoutId );
      //
      // initialise the methods variables and objects.
      //
      Evado.Bll.Clinical.EvFormFields bll_Formfields = new EvFormFields ( );
      Guid formGuid = Guid.Empty;
      StringBuilder processLog = new StringBuilder ( );
      Evado.Model.EvEventCodes result = EvEventCodes.Ok;
      float version = 0.0F;

      processLog.AppendLine ( "Saving form: " + UploadedForm.LayoutId + " " + UploadedForm.Title );
      //
      // Get the list of forms to determine if there is an existing draft form.
      //
      if ( this.Session.FormList.Count == 0 )
      {
        this.loadTrialFormList ( );
      }

      //
      // check if there is a draft form and delete it.
      //
      foreach ( EdRecord form in this.Session.FormList )
      {
        //
        // get the list issued version of the form.
        //
        if ( form.LayoutId == UploadedForm.LayoutId
          && form.State == EdRecordObjectStates.Form_Issued )
        {
          version = form.Design.Version;
        }

        //
        // delete any existing draft forms with form ID
        //
        if ( form.LayoutId == UploadedForm.LayoutId
          && form.State == EdRecordObjectStates.Form_Draft )
        {
          processLog.AppendLine ( "Existing draft version of " + UploadedForm.LayoutId + " " + UploadedForm.Title + " found." );

          form.SaveAction = EdRecord.SaveActionCodes.Form_Deleted;
          form.UpdatedByUserId = this.Session.UserProfile.UserId;
          form.UserCommonName = this.Session.UserProfile.CommonName;

          result = this._Bll_Forms.SaveItem ( form );

          if ( result == EvEventCodes.Ok )
          {
            processLog.AppendLine ( "Existing draft version of successfully deleted." );
          }
          else
          {
            processLog.AppendLine ( "Deletion process returned the following error: " +
              Evado.Model.EvStatics.Enumerations.enumValueToString ( result ) );

            return processLog.ToString ( );
          }
        }
      }

      processLog.AppendLine ( "Saving uploaded form to the database." );
      //
      // set the form's save parameters 
      //
      UploadedForm.ApplicationId = this.Session.Application.ApplicationId;
      UploadedForm.State = EdRecordObjectStates.Form_Draft;
      UploadedForm.SaveAction = EdRecord.SaveActionCodes.Save;
      UploadedForm.Design.Version = version + 0.01F;
      UploadedForm.UpdatedByUserId = this.Session.UserProfile.UserId;
      UploadedForm.UserCommonName = this.Session.UserProfile.CommonName;
      UploadedForm.Guid = Guid.Empty;

      //
      // Save the form
      //
      result = this._Bll_Forms.SaveItem ( UploadedForm );

      this.LogText ( this._Bll_Forms.Log );

      if ( result == EvEventCodes.Ok )
      {
        processLog.AppendLine ( "Uploaded form successfully save to database." );
      }
      else
      {
        processLog.AppendLine ( "Save process returned the following error: " +
          Evado.Model.EvStatics.Enumerations.enumValueToString ( result ) );
      }

      return processLog.ToString ( );

    }//END saveUploadeForm method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="ClientDataObject">Evado.Model.UniForm.AppData object.</param>
    //  ------------------------------------------------------------------------------
    private void getTemplateUploadDataObject (
      Evado.Model.UniForm.AppData ClientDataObject )
    {
      this.LogMethod ( "getTemplateUploadDataObject" );


      //
      // set the page edit access.
      //
      ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
      if ( this.Session.UserProfile.hasConfigrationEditAccess == true )
      {
        ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
      }

      //
      // Add the file selection group.
      //
      this.getUpload_FileSelectionGroup ( ClientDataObject.Page );

    }//END getPropertiesDataObject Method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void getUpload_FileSelectionGroup (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getProperties_GeneralPageGroup" );
      // 
      // Initialise the methods variables and objects.
      // 
      Bll.Clinical.EdApplications trials = new Bll.Clinical.EdApplications ( );
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Command groupCommand = new Evado.Model.UniForm.Command ( );
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      Evado.Model.UniForm.Parameter parameter = new Evado.Model.UniForm.Parameter ( );

      //
      // Define the general properties pageMenuGroup..
      //
      pageGroup = Page.AddGroup (
        String.Empty,
        Evado.Model.UniForm.EditAccess.Inherited_Access );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;

      pageGroup.SetCommandBackBroundColor (
        Model.UniForm.GroupParameterList.BG_Mandatory,
        Model.UniForm.Background_Colours.Red );

      groupField = pageGroup.createBinaryFileField (
        EuForms.CONST_TEMPLATE_FIELD_ID,
        EvLabels.Form_Template_File_Selection_Field_Title,
        String.Empty,
        this.Session.UploadFileName );
      groupField.Layout = EuFormGenerator.ApplicationFieldLayout;

      groupField.AddParameter ( Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, "Yes" );

      groupCommand = pageGroup.addCommand (
        EvLabels.Form_Template_Upload_Command_Title,
        EuAdapter.APPLICATION_ID,
        EuAdapterClasses.Project_Forms.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Custom_Method );

      groupCommand.SetPageId ( EvPageIds.Form_Template_Upload );
      groupCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.Get_Object );


    }//END getUpload_FileSelectionGroup Method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class form property page methods

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData getFormPropertiesObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getFormPropertiesObject" );

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
      Guid formGuid = Guid.Empty;

      //
      // Determine if the user has access to this page and log and error if they do not.
      //
      if ( this.Session.UserProfile.hasTrialManagementAccess == false )
      {
        this.LogIllegalAccess (
          this.ClassNameSpace + "getFormPropertiesObject",
          this.Session.UserProfile );

        this.ErrorMessage = EvLabels.Illegal_Page_Access_Attempt;

        return null;
      }

      // 
      // Log access to page.
      // 
      this.LogPageAccess (
        this.ClassNameSpace + "getFormPropertiesObject",
        this.Session.UserProfile );

      try
      {
        if ( this.getFormObject ( PageCommand, false ) == false )
        {
          return null;
        }

        string value = PageCommand.GetParameter ( EuForms.CONST_UPDATE_SECTION_COMMAND_PARAMETER );

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
        return clientDataObject;
      }
      catch ( Exception Ex )
      {
        // 
        // On an exception raised create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Record_Retrieve_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return null;

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
      if ( this.Session.Form.Guid != Guid.Empty )
      {
        ClientDataObject.Id = Guid.NewGuid ( );
        ClientDataObject.Title =
          String.Format ( EvLabels.Form_Page_Title,
            this.Session.Form.LayoutId,
            this.Session.Form.Title );
        ClientDataObject.Page.Id = ClientDataObject.Id;
        ClientDataObject.Page.PageDataGuid = ClientDataObject.Id;
        ClientDataObject.Page.PageId = this.Session.Form.LayoutId;
      }
      else
      {
        ClientDataObject.Id = Guid.NewGuid ( );
        ClientDataObject.Title = EvLabels.Form_Page_New_Form_Title;
        ClientDataObject.Page.Id = ClientDataObject.Id;
        ClientDataObject.Page.PageDataGuid = ClientDataObject.Id;
      }
      ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      if ( this.Session.UserProfile.hasConfigrationEditAccess == true )
      {
        ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
      }

      //
      // Set the user's edit access if they have configuration edit access.
      //
      this.LogValue ( "HasConfigrationEditAccess: "
        + this.Session.UserProfile.hasConfigrationEditAccess );

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
      Bll.Clinical.EdApplications trials = new Bll.Clinical.EdApplications ( );
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );
      Evado.Model.UniForm.Field pageField = new Evado.Model.UniForm.Field ( );
      Evado.Model.UniForm.Parameter parameter = new Evado.Model.UniForm.Parameter ( );
      List<EvOption> optionList = new List<EvOption> ( );

      //
      // Define the general properties pageMenuGroup..
      //
      pageGroup = Page.AddGroup (
        EvLabels.Form_Properties_General_Group_Title,
        Evado.Model.UniForm.EditAccess.Inherited_Access );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;

      pageGroup.SetCommandBackBroundColor (
        Model.UniForm.GroupParameterList.BG_Mandatory,
        Model.UniForm.Background_Colours.Red );

      //
      // Add the save commandS for the page.
      //
      if ( this.Session.UserProfile.hasConfigrationEditAccess == true )
      {
        Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

        this.setFormSaveGroupCommands ( pageGroup );
      }

      //
      // Create the project selection list,
      // If the project is Global then only display the Global project identifier and not
      // a selection list.
      //
      if ( this.Session.Application.ApplicationId != Evado.Model.Digital.EvcStatics.CONST_GLOBAL_PROJECT )
      {
        pageField = pageGroup.createSelectionListField (
          EdRecord.FormClassFieldNames.ProjectId.ToString ( ),
          EvLabels.Label_Project_Id,
          this.Session.Form.ApplicationId,
          this.Session.TrialSelectionList );
        pageField.Layout = EuFormGenerator.ApplicationFieldLayout;
      }
      else
      {
        pageField = pageGroup.createTextField (
          EdRecord.FormClassFieldNames.ProjectId.ToString ( ),
          EvLabels.Label_Project_Id,
          this.Session.Form.ApplicationId, 20 );
        pageField.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
        pageField.Layout = EuFormGenerator.ApplicationFieldLayout;
      }

      //
      // Form type selection list.
      //
      optionList = EdRecord.getFormTypes ( false );

      pageField = pageGroup.createSelectionListField (
        EdRecord.FormClassFieldNames.TypeId.ToString ( ),
        EvLabels.Form_Type_Field_Label,
        this.Session.Form.Design.TypeId.ToString ( ),
        optionList );
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;

      //
      // Form title
      //
      pageField = pageGroup.createTextField (
        EdRecord.FormClassFieldNames.FormId.ToString ( ),
        EvLabels.Label_Form_Id,
        this.Session.Form.LayoutId,
        10 );
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;

      //
      // Form title
      //
      pageField = pageGroup.createTextField (
        EdRecord.FormClassFieldNames.Title.ToString ( ),
        EvLabels.Form_Title_Field_Label,
        this.Session.Form.Design.Title,
        50 );
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;

      //
      // Form Instructions
      //
      pageField = pageGroup.createFreeTextField (
        EdRecord.FormClassFieldNames.Instructions.ToString ( ),
        EvLabels.Form_Instructions_Field_Title,
        this.Session.Form.Design.Instructions,
        50, 4 );
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;

      //
      // Form Update reason
      //
      optionList = EvStatics.Enumerations.getOptionsFromEnum ( typeof ( EdRecord.UpdateReasonList ), false );

      pageField = pageGroup.createSelectionListField (
        EdRecord.FormClassFieldNames.UpdateReason.ToString ( ),
        EvLabels.Form_Update_Reason_Field_Title,
        this.Session.Form.UpdateReason,
        optionList );

      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;

      //
      // Form Change description
      //
      pageField = pageGroup.createFreeTextField (
        EdRecord.FormClassFieldNames.Description.ToString ( ),
        EvLabels.Form_Description_Field_Title,
        this.Session.Form.Description,
        90, 5 );

      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;

      //
      // Form reference
      //
      pageField = pageGroup.createTextField (
        EdRecord.FormClassFieldNames.Reference.ToString ( ),
        EvLabels.Form_Reference_Field_Label,
        this.Session.Form.Design.Reference,
        50 );
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;

      //
      // Form category
      //
      pageField = pageGroup.createTextField (
        EdRecord.FormClassFieldNames.FormCategory.ToString ( ),
        EvLabels.Form_Category_Field_Title,
        this.Session.Form.Design.RecordCategory,
        50 );
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;

      //
      // Form CS Script
      //
      pageField = pageGroup.createBooleanField (
        EdRecord.FormClassFieldNames.HasCsScript.ToString ( ),
        EvLabels.Form_Cs_Script_Field_Title,
        this.Session.Form.Design.hasCsScript );
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;

      //
      // Form hide annotation field
      //
      pageField = pageGroup.createBooleanField (
        EdRecord.FormClassFieldNames.OnEdit_HideFieldAnnotation.ToString ( ),
        EvLabels.Form_Hide_Annotation_Field_Title,
        this.Session.Form.Design.OnEdit_HideFieldAnnotation );
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;

      //
      // Form Form layout
      //
      this.Session.Form.FormLayout = Model.UniForm.FieldValueWidths.Default.ToString ( );

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

      foreach ( EdRecordField field in this.Session.Form.Fields )
      {
        optionList.Add ( new EvOption (
          field.FieldId,
          field.FieldId
          + EvLabels.Space_Hypen
          + field.Title ) );
      }

      //
      // Define the section properties pageMenuGroup..
      //
      pageGroup = Page.AddGroup (
        EvLabels.Form_Properties_Sections_Group_Title,
        Evado.Model.UniForm.EditAccess.Inherited_Access );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;
      pageGroup.CmdLayout = Model.UniForm.GroupCommandListLayouts.Vertical_Orientation;

      //
      // Add the new form section object.

      groupCommand = pageGroup.addCommand ( EvLabels.Form_Section_New_Section_Command_Title,
        EuAdapter.APPLICATION_ID,
        EuAdapterClasses.Project_Forms.ToString ( ),
        Model.UniForm.ApplicationMethods.Get_Object );

      groupCommand.AddParameter ( EvFormSection.FormSectionClassFieldNames.Sectn_No.ToString ( ), "-1" );
      groupCommand.SetPageId ( EvPageIds.Form_Properties_Section_Page );
      groupCommand.SetBackgroundDefaultColour ( Model.UniForm.Background_Colours.Purple );

      this.LogValue ( "No of form sections: " + this.Session.Form.Design.FormSections.Count );

      //
      // Iterate through the sections.
      //
      foreach ( EvFormSection formSection in this.Session.Form.Design.FormSections )
      {
        groupCommand = pageGroup.addCommand ( formSection.LinkText,
          EuAdapter.APPLICATION_ID,
          EuAdapterClasses.Project_Forms.ToString ( ),
          Model.UniForm.ApplicationMethods.Get_Object );

        groupCommand.AddParameter ( EvFormSection.FormSectionClassFieldNames.Sectn_No.ToString ( ), formSection.No );
        groupCommand.SetPageId ( EvPageIds.Form_Properties_Section_Page );
      }

      this.LogValue ( "After No of form sections: " + this.Session.Form.Design.FormSections.Count );
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
        string value = PageCommand.GetParameter ( EvFormSection.FormSectionClassFieldNames.Sectn_No.ToString ( ) );
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
        this.ErrorMessage = EvLabels.Form_Retrieve_Error_Message;

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
      Bll.Clinical.EdApplications projects = new Bll.Clinical.EdApplications ( );
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );
      Evado.Model.UniForm.Field pageField = new Evado.Model.UniForm.Field ( );
      Evado.Model.UniForm.Parameter parameter = new Evado.Model.UniForm.Parameter ( );
      List<EvOption> optionList = new List<EvOption> ( );
      optionList.Add ( new EvOption ( ) );

      foreach ( EdRecordField field in this.Session.Form.Fields )
      {
        optionList.Add ( new EvOption (
          field.FieldId,
          field.FieldId
          + EvLabels.Space_Hypen
          + field.Title ) );
      }

      // 
      // Initialise the client ResultData object.
      // 
      if ( this.Session.Form.Guid != Guid.Empty )
      {
        ClientDataObject.Id = Guid.NewGuid ( );
        ClientDataObject.Title =
          String.Format (
          EvLabels.FormProperties_Section_Page_Title,
          this.Session.Form.LayoutId,
          this.Session.Form.Title,
          this.Session.FormSection.LinkText );
        ClientDataObject.Page.Id = ClientDataObject.Id;
        ClientDataObject.Page.PageDataGuid = ClientDataObject.Id;
        ClientDataObject.Page.PageId = this.Session.Form.LayoutId;
      }
      else
      {
        ClientDataObject.Id = Guid.NewGuid ( );
        ClientDataObject.Title = EvLabels.Form_Page_New_Form_Title;
        ClientDataObject.Page.Id = ClientDataObject.Id;
        ClientDataObject.Page.PageDataGuid = ClientDataObject.Id;
      }
      ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      if ( this.Session.UserProfile.hasConfigrationEditAccess == true )
      {
        ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
      }

      //
      // Set the user's edit access if they have configuration edit access.
      //
      this.LogDebug ( "HasConfigrationEditAccess: " + this.Session.UserProfile.hasConfigrationEditAccess );


      if ( this.Session.UserProfile.hasConfigrationEditAccess == true )
      {
        ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
      }

      pageGroup = ClientDataObject.Page.AddGroup (
        EvLabels.FormProperties_Section_Group_Text,
        Evado.Model.UniForm.EditAccess.Inherited_Access );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;

      //
      // Form No
      //
      pageField = pageGroup.createTextField (
        EvFormSection.FormSectionClassFieldNames.Sectn_No.ToString ( ),
        EvLabels.Form_Section_No_Field_Label,
        this.Session.FormSection.No.ToString ( ),
        50 );
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;
      pageField.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      //
      // Form title
      //
      pageField = pageGroup.createTextField (
        EvFormSection.FormSectionClassFieldNames.Sectn_Title.ToString ( ),
        EvLabels.Form_Section_Title_Field_Label,
        this.Session.FormSection.Title,
        50 );
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;
      pageField.EditAccess = Evado.Model.UniForm.EditAccess.Inherited_Access;

      //
      // Form Instructions
      //
      pageField = pageGroup.createFreeTextField (
        EvFormSection.FormSectionClassFieldNames.Sectn_Instructions.ToString ( ),
        EvLabels.Form_Section_Instructions_Field_Label,
        EvLabels.Form_Section_Instructions_Field_Description,
        this.Session.FormSection.Instructions,
        90, 5 );
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;

      optionList = new List<EvOption> ( );

      this.LogDebug ( "FormSection.Order: " + this.Session.FormSection.Order );
      foreach ( EvFormSection section in this.Session.Form.Design.FormSections )
      {
        if ( section.Order < this.Session.FormSection.Order )
        {
          this.LogDebug ( "secttion.Order: " + section.Order + " BEFORE SECTION" );

          var value = String.Format ( EvLabels.Form_Section_Order_Before_Text, section.Title );
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
          var value = String.Format ( EvLabels.Form_Section_Order_After_Text, section.Title );
          optionList.Add ( new EvOption ( ( section.Order + 1 ).ToString ( ), value ) );
        }
      }

      //
      // The form section order 
      //
      pageField = pageGroup.createSelectionListField (
        EvFormSection.FormSectionClassFieldNames.Sectn_Order.ToString ( ),
        EvLabels.Form_Section_Order_Field_Label,
        this.Session.FormSection.Order.ToString ( ),
        optionList );
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;

      /*
      //
      // The form section field id 
      //
      pageField = pageGroup.createSelectionListField (
        EvFormSection.FormSectionClassFieldNames.Sectn_Field_Id.ToString ( ),
        EvLabels.Form_Section_Field_ID_Field_Label,
        this.SessionObjects.FormSection.FieldId,
        optionList );
      pageField.Layout = EuPageGenerator.ApplicationFieldLayout;

      //
      // Form Field value
      //
      pageField = pageGroup.createTextField (
        EvFormSection.FormSectionClassFieldNames.Sectn_Field_Value.ToString ( ),
        EvLabels.Form_Section_Field_Value_Field_Label,
        this.SessionObjects.FormSection.FieldValue,
        50 );
      pageField.Layout = EuPageGenerator.ApplicationFieldLayout;
      pageField.EditAccess = Evado.Model.UniForm.EditAccess.Inherited_Access;

      //
      // form secton on open display section.
      //
      pageField = pageGroup.createBooleanField (
        EvFormSection.FormSectionClassFieldNames.Sectn_On_Open_Visible.ToString ( ),
        EvLabels.Form_Section_Visible_On_Open_Field_Label,
        this.SessionObjects.FormSection.OnOpenVisible );
      pageField.Layout = EuPageGenerator.ApplicationFieldLayout;

      //
      // Form on field value match display field field
      //
      pageField = pageGroup.createBooleanField (
        EvFormSection.FormSectionClassFieldNames.Sectn_On_Match_Visible.ToString ( ),
        EvLabels.Form_Section_Visible_Field_Value_Matches_Field_Label,
        this.SessionObjects.FormSection.OnMatchVisible );
      pageField.Layout = EuPageGenerator.ApplicationFieldLayout;
      */
      //
      // get the list of form roles.
      //
      optionList = new List<EvOption> ( );

      optionList.Add ( EvStatics.Enumerations.getOption ( EdRecord.FormAccessRoles.Record_Author ) );
      optionList.Add ( EvStatics.Enumerations.getOption ( EdRecord.FormAccessRoles.Patient ) );
      optionList.Add ( EvStatics.Enumerations.getOption ( EdRecord.FormAccessRoles.Monitor ) );
      optionList.Add ( EvStatics.Enumerations.getOption ( EdRecord.FormAccessRoles.Data_Manager ) );
      optionList.Add ( EvStatics.Enumerations.getOption ( EdRecord.FormAccessRoles.Record_Reader ) );

      //
      // The form section user display roles 
      //
      pageField = pageGroup.createCheckBoxListField (
        EvFormSection.FormSectionClassFieldNames.Sectn_Display_Roles.ToString ( ),
        EvLabels.Form_Section_User_Display_Roles_Field_Label,
        EvLabels.Form_Section_User_Display_Roles_Field_Description,
        this.Session.FormSection.UserDisplayRoles,
        optionList );
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;

      //
      // The form section user edit roles 
      //
      /*
      pageField = pageGroup.createCheckBoxListField (
        EvFormSection.FormSectionClassFieldNames.Sectn_Edit_Roles.ToString ( ),
        EvLabels.Form_Section_User_Edit_Roles_Field_Label,
        EvLabels.Form_Section_User_Edit_Roles_Field_Description,
        this.SessionObjects.FormSection.UserEditRoles,
        optionList );
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;
      */
      //
      // Form Form layout
      //
      this.Session.Form.FormLayout = Model.UniForm.FieldValueWidths.Default.ToString ( );

      //
      // Add the command to save the page content.
      //
      pageCommand = pageGroup.addCommand (
        EvLabels.Form_Properties_Section_Save_Command_Title,
        EuAdapter.APPLICATION_ID,
        EuAdapterClasses.Project_Forms.ToString ( ),
        Model.UniForm.ApplicationMethods.Custom_Method );

      pageCommand.SetPageId ( EvPageIds.Form_Properties_Page );
      pageCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.Get_Object );
      pageCommand.SetGuid ( this.Session.Form.Guid );
      pageCommand.AddParameter ( EuForms.CONST_UPDATE_SECTION_COMMAND_PARAMETER, "1" );

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
      string value = PageCommand.GetParameter ( EuForms.CONST_UPDATE_SECTION_COMMAND_PARAMETER );
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
          EvFormSection.FormSectionClassFieldNames fieldName = Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue<EvFormSection.FormSectionClassFieldNames> ( parameter.Name );

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
      if ( this.Session.Form.Design.FormSections.Count > 0
        && this.Session.FormSection.No > 0 )
      {
        this.LogDebug ( "Updating the section values" );

        for ( int index = 0; index < this.Session.Form.Design.FormSections.Count; index++ )
        {
          if ( this.Session.Form.Design.FormSections [ index ].No == this.Session.FormSection.No )
          {
            this.Session.Form.Design.FormSections [ index ] = this.Session.FormSection;

            this.LogDebug ( this.Session.FormSection.LinkText + " >> UPDATED" );
          }
        }
      }

      //
      // Sort the section based on the current secton order.
      //
      this.Session.Form.Design.FormSections.Sort (
          delegate ( EvFormSection p1, EvFormSection p2 )
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
        this.Session.FormSection = new EvFormSection ( );
        this.Session.FormSection.No = this.getMaxSectionNo ( );
        this.Session.FormSection.No++;
        this.Session.FormSection.Order = getMaxSectionOrder ( );
        this.Session.FormSection.Order++;

        this.Session.Form.Design.FormSections.Add ( this.Session.FormSection );

        return true;
      }

      //
      // Iterate through the sections to find the section matching the No.
      //
      foreach ( EvFormSection section in this.Session.Form.Design.FormSections )
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

      foreach ( EvFormSection section in this.Session.Form.Design.FormSections )
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

      foreach ( EvFormSection section in this.Session.Form.Design.FormSections )
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

    #region Class form layout methods

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <param name="Refesh">Boolean: true = refresh object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private bool getFormObject (
      Evado.Model.UniForm.Command PageCommand,
      bool Refesh )
    {
      this.LogMethod ( "getFormObject" );
      this.LogValue ( "Refesh: " + Refesh );

      // 
      // Initialise the methods variables and objects.
      // 
      Guid formGuid = Guid.Empty;

      // 
      // Retrieve the record guid from the parameters
      // 
      formGuid = PageCommand.GetGuid ( );

      this.LogValue ( " recordGuid: " + formGuid );

      // 
      // if the guid is empty the parameter was not found to exit.
      // 
      if ( formGuid == Guid.Empty )
      {
        this.LogValue ( "formGuid is EMPTY" );

        return false;
      }

      if ( this.Session.Form.Guid == formGuid
        && Refesh == false )
      {
        this.LogValue ( "form in memory" );

        //
        // If empty refresh the fields as a new field may have been added.
        //
        if ( this.Session.Form.Fields.Count == 0 )
        {
          EvFormFields formFields = new EvFormFields ( );

          this.Session.Form.Fields = formFields.GetView ( this.Session.Form.Guid );

          this.LogValue ( "Updated: Form.Fields.Count: " + this.Session.Form.Fields.Count );
        }

        return true;
      }

      // 
      // Retrieve the record object from the database via the DAL and BLL layers.
      // 
      this.Session.Form = this._Bll_Forms.getForm ( formGuid );

      this.LogValue ( this._Bll_Forms.Log );

      this.LogValue ( " SessionObjects.Form.FormId: " + this.Session.Form.LayoutId );

      // 
      // Save the session ResultData so it is available for the next user generated groupCommand.
      // 
      this.SaveSessionObjects ( );

      return true;
    }//END getFormObject method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData getFormLayoutObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getFormLayoutObject" );

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
      Guid formGuid = Guid.Empty;

      //
      // Determine if the user has access to this page and log and error if they do not.
      //
      if ( this.Session.UserProfile.hasTrialManagementAccess == false )
      {
        this.LogIllegalAccess (
          this.ClassNameSpace + "getObject",
          this.Session.UserProfile );

        this.ErrorMessage = EvLabels.Illegal_Page_Access_Attempt;

        return null;
      }

      // 
      // Log access to page.
      // 
      this.LogPageAccess (
        this.ClassNameSpace + "getObject",
        this.Session.UserProfile );


      try
      {
        if ( this.getFormObject ( PageCommand, false ) == false )
        {
          return null;
        }

        //
        // if there are no field display the draft layout page.
        //
        if ( this.Session.Form.Fields.Count == 0 )
        {
          this.getFormDraftDataObject ( clientDataObject );

          return clientDataObject;
        }

        //
        // if there are is not page type defines and draft state display the draft page.
        //
        if ( this.Session.Form.State == EdRecordObjectStates.Form_Draft
          && this.Session.PageId == EvPageIds.Null )
        {
          this.Session.PageId = EvPageIds.Form_Draft_Page;
          this.getFormDraftDataObject ( clientDataObject );

          return clientDataObject;
        }

        if ( this.Session.PageId != EvPageIds.Form_Page
          && this.Session.PageId != EvPageIds.Form_Properties_Page
          && this.Session.PageId != EvPageIds.Form_Draft_Page
          && this.Session.PageId != EvPageIds.Form_Annotated_Page )
        {
          this.Session.PageId = EvPageIds.Form_Page;
        }

        //
        // Rung the server script if server side scripts are enabled.
        //
        this.runServerScript ( EvServerPageScript.ScripEventTypes.OnOpen );

        // 
        // Save the session ResultData so it is available for the next user generated groupCommand.
        // 
        this.SaveSessionObjects ( );

        //
        // generate the page layout.
        //
        this.getFormDataObject ( clientDataObject );

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
        this.ErrorMessage = EvLabels.Record_Retrieve_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return null;

    }//END getLayoutObject method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData getFormDraftLayoutObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getFormDraftLayoutObject" );

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
      Guid formGuid = Guid.Empty;

      //
      // Determine if the user has access to this page and log and error if they do not.
      //
      if ( this.Session.UserProfile.hasTrialManagementAccess == false )
      {
        this.LogIllegalAccess (
          this.ClassNameSpace + "getFormDraftLayoutObject",
          this.Session.UserProfile );

        this.ErrorMessage = EvLabels.Illegal_Page_Access_Attempt;

        return null;
      }

      // 
      // Log access to page.
      // 
      this.LogPageAccess (
        this.ClassNameSpace + "getFormDraftLayoutObject",
        this.Session.UserProfile );


      try
      {
        if ( this.getFormObject ( PageCommand, true ) == false )
        {
          return null;
        }

        // 
        // Save the session ResultData so it is available for the next user generated groupCommand.
        // 
        this.SaveSessionObjects ( );

        //
        // generate the page layout.
        //
        this.getFormDraftDataObject ( clientDataObject );

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
        this.ErrorMessage = EvLabels.Record_Retrieve_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return null;

    }//END getLayoutObject method

    // ==============================================================================
    /// <summary>
    /// This method creates the pages save commands objects.
    /// </summary>
    /// <returns>Evado.Model.UniForm.Page object</returns>
    //  ------------------------------------------------------------------------------
    private void setFormPageCommands (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "setFormSavePageCommands" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Parameter parameter = new Evado.Model.UniForm.Parameter ( );
      Evado.Model.UniForm.Command pageCommand = new Model.UniForm.Command ( );

      //
      // IF the user does not have edit access only display the annotation and full 
      // layout commands
      //
      if ( this.Session.UserProfile.hasConfigrationEditAccess == false )
      {
        this.setFormPageLayoutCommands ( PageObject, false );

        return;

      }//END non edit commands.

      //
      // Add the save groupCommand for the page.
      //
      switch ( this.Session.Form.State )
      {
        case EdRecordObjectStates.Form_Draft:
          {

            //
            // Add the same groupCommand.
            //
            pageCommand = PageObject.addCommand (
              EvLabels.Form_Save_Command_Title,
              EuAdapter.APPLICATION_ID,
              EuAdapterClasses.Project_Forms.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.Save_Object );

            // 
            // Define the save groupCommand parameters.
            // 
            pageCommand.SetGuid ( this.Session.Form.Guid );

            pageCommand.AddParameter (
              Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
             EdRecord.SaveActionCodes.Form_Saved.ToString ( ) );

            //
            // add the page layout selection commands.
            //
            this.setFormPageLayoutCommands ( PageObject, true );

            //
            // For a new form do not display the delete or review commands.
            //
            if ( this.Session.Form.Guid != Guid.Empty )
            {
              //
              // Add the same groupCommand.
              //
              pageCommand = PageObject.addCommand (
                EvLabels.Form_Delete_Command_Title,
                EuAdapter.APPLICATION_ID,
                EuAdapterClasses.Project_Forms.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.Save_Object );

              // 
              // Define the save groupCommand parameters.
              // 
              pageCommand.SetGuid ( this.Session.Form.Guid );

              pageCommand.AddParameter (
                Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
               EdRecord.SaveActionCodes.Form_Deleted.ToString ( ) );

              //
              // Add the same groupCommand.
              //
              if ( PageObject.GroupList.Count > 0 )
              {
                PageObject.GroupList [ 0 ].addCommand ( pageCommand );
              }

              if ( this.Session.Form.Fields.Count > 0 )
              {
                pageCommand.AddParameter (
                  Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
                 EdRecord.SaveActionCodes.Form_Deleted.ToString ( ) );
                //
                // Add the same groupCommand.
                //
                pageCommand = PageObject.addCommand (
                  EvLabels.Form_Review_Command_Title,
                  EuAdapter.APPLICATION_ID,
                  EuAdapterClasses.Project_Forms.ToString ( ),
                  Evado.Model.UniForm.ApplicationMethods.Save_Object );

                // 
                // Define the save groupCommand parameters.
                // 
                pageCommand.SetGuid ( this.Session.Form.Guid );

                pageCommand.AddParameter (
                  Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
                 EdRecord.SaveActionCodes.Form_Reviewed.ToString ( ) );

                //
                // Add the same groupCommand.
                //
                if ( PageObject.GroupList.Count > 0 )
                {
                  PageObject.GroupList [ 0 ].addCommand ( pageCommand );
                }
              }//END Form field exist.
            }//END form exists.

            return;
          }
        case EdRecordObjectStates.Form_Reviewed:
          {
            //
            // Add the same groupCommand.
            //
            pageCommand = PageObject.addCommand (
              EvLabels.Form_Save_Command_Title,
              EuAdapter.APPLICATION_ID,
              EuAdapterClasses.Project_Forms.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.Save_Object );

            // 
            // Define the save groupCommand parameters.
            // 
            pageCommand.SetGuid ( this.Session.Form.Guid );

            pageCommand.AddParameter (
              Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
             EdRecord.SaveActionCodes.Form_Saved.ToString ( ) );

            //
            // add the page layout selection commands.
            //
            this.setFormPageLayoutCommands ( PageObject, true );

            //
            // Add the same groupCommand.
            //
            if ( PageObject.GroupList.Count > 0 )
            {
              PageObject.GroupList [ 0 ].addCommand ( pageCommand );
            }

            //
            // Add the same groupCommand.
            //
            pageCommand = PageObject.addCommand (
              EvLabels.Form_Approved_Command_Title,
              EuAdapter.APPLICATION_ID,
              EuAdapterClasses.Project_Forms.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.Save_Object );

            // 
            // Define the save groupCommand parameters.
            // 
            pageCommand.SetGuid ( this.Session.Form.Guid );

            pageCommand.AddParameter (
              Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
             EdRecord.SaveActionCodes.Form_Approved.ToString ( ) );

            //
            // Add the same groupCommand.
            //
            if ( PageObject.GroupList.Count > 0 )
            {
              PageObject.GroupList [ 0 ].addCommand ( pageCommand );
            }
            return;
          }
        case EdRecordObjectStates.Form_Issued:
          {
            //
            // Add the same groupCommand.
            //
            /*
            pageCommand = PageObject.addCommand (
              EvLabels.Form_Withdrawn_Command_Title,
              Adapter.ApplicationId,
              Adapter.ApplicationObjects.Project_Forms.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.Save_Object );

            // 
            // Define the save groupCommand parameters.
            // 
            pageCommand.setParameterGuid ( this.SessionObjects.Form.Guid );

            pageCommand.AddParameter (
              Evado.Model.Digital.EvStatics.CONST_SAVE_ACTION,
             EvForm.SaveActionCodes.Form_Withdrawn.ToString ( ) );
            */

            //
            // add the page layout selection commands.
            //
            this.setFormPageLayoutCommands ( PageObject, false );

            //
            // Add the form template save command.
            //
            pageCommand = PageObject.addCommand (
              EvLabels.Form_Template_Download_Command_Title,
              EuAdapter.APPLICATION_ID,
              EuAdapterClasses.Project_Forms.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.Get_Object );

            // 
            // Define the groupCommand parameters.
            // 
            pageCommand.SetGuid ( this.Session.Form.Guid );

            pageCommand.SetPageId ( EvPageIds.Form_Template_Download );

            //
            // Add the copy groupCommand.
            //
            pageCommand = PageObject.addCommand (
              EvLabels.Form_Copy_Form_Command_Title,
              EuAdapter.APPLICATION_ID,
              EuAdapterClasses.Project_Forms.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

            // 
            // Define the groupCommand parameters.
            // 
            pageCommand.SetGuid ( this.Session.Form.Guid );

            pageCommand.AddParameter (
            EuForms.CONST_FORM_COMMAND_PARAMETER,
            EuForms.CONST_COPY_FORM_COMMAND_VALUE );

            //
            // Add the review groupCommand.
            //
            pageCommand = PageObject.addCommand (
              EvLabels.Form_Revise_Form_Command_Title,
              EuAdapter.APPLICATION_ID,
              EuAdapterClasses.Project_Forms.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

            // 
            // Define the groupCommand parameters.
            // 
            pageCommand.SetGuid ( this.Session.Form.Guid );

            pageCommand.AddParameter (
            EuForms.CONST_FORM_COMMAND_PARAMETER,
            EuForms.CONST_REVISE_FORM_COMMAND_VALUE );

            return;
          }
        default:
          {
            return;
          }
      }
    }//END setFormSaveCommands method

    // ==============================================================================
    /// <summary>
    /// This method creates the pages save commands objects.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object</param>
    /// <param name="EditAccess">Bool: true user has edit access</param>
    /// <returns>Evado.Model.UniForm.Page object</returns>
    //  ------------------------------------------------------------------------------
    private void setFormPageLayoutCommands (
      Evado.Model.UniForm.Page PageObject,
      bool EditAccess )
    {
      this.LogMethod ( "setFormPageLayoutCommands" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Parameter parameter = new Evado.Model.UniForm.Parameter ( );
      Evado.Model.UniForm.Command pageCommand = new Model.UniForm.Command ( );

      //
      // Add the properties page groupCommand.
      //
      if ( ( this.Session.PageId == EvPageIds.Form_Annotated_Page
          || this.Session.PageId == EvPageIds.Form_Page
          || this.Session.PageId == EvPageIds.Form_Draft_Page )
        && EditAccess == true )
      {
        pageCommand = PageObject.addCommand (
          EvLabels.Form_Properties_Command_Title,
          EuAdapter.APPLICATION_ID,
          EuAdapterClasses.Project_Forms.ToString ( ),
          Model.UniForm.ApplicationMethods.Get_Object );

        pageCommand.SetPageId ( EvPageIds.Form_Properties_Page );

        pageCommand.SetGuid ( this.Session.Form.Guid );
      }

      //
      // The draft button.
      //
      if ( this.Session.PageId == EvPageIds.Form_Annotated_Page
        || this.Session.PageId == EvPageIds.Form_Draft_Page
        || this.Session.PageId == EvPageIds.Form_Properties_Page )
      {
        //
        // The full layout button.
        //
        pageCommand = PageObject.addCommand (
          EvLabels.Form_Full_Layout_Command_Title,
          EuAdapter.APPLICATION_ID,
          EuAdapterClasses.Project_Forms.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Custom_Method );

        pageCommand.setCustomMethod (
          Model.UniForm.ApplicationMethods.Get_Object );

        pageCommand.SetGuid ( this.Session.Form.Guid );

        pageCommand.SetPageId ( EvPageIds.Form_Page );
      }

      if ( this.Session.PageId == EvPageIds.Form_Page
        || this.Session.PageId == EvPageIds.Form_Draft_Page
        || this.Session.PageId == EvPageIds.Form_Properties_Page )
      {
        //
        // The annotated layout button.
        //
        pageCommand = PageObject.addCommand (
          EvLabels.Form_Annotated_Layout_Command_Title,
          EuAdapter.APPLICATION_ID,
          EuAdapterClasses.Project_Forms.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Custom_Method );

        pageCommand.setCustomMethod (
          Model.UniForm.ApplicationMethods.Get_Object );

        pageCommand.SetGuid ( this.Session.Form.Guid );

        pageCommand.SetPageId ( EvPageIds.Form_Annotated_Page );
      }

      if ( ( this.Session.PageId == EvPageIds.Form_Annotated_Page
          || this.Session.PageId == EvPageIds.Form_Page
          || this.Session.PageId == EvPageIds.Form_Properties_Page )
        && EditAccess == true )
      {
        pageCommand = PageObject.addCommand (
          EvLabels.Form_Draft_Layout_Command_Title,
          EuAdapter.APPLICATION_ID,
          EuAdapterClasses.Project_Forms.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Custom_Method );

        pageCommand.setCustomMethod (
          Model.UniForm.ApplicationMethods.Get_Object );

        pageCommand.SetGuid ( this.Session.Form.Guid );

        pageCommand.SetPageId ( EvPageIds.Form_Draft_Page );
      }

    }//END method

    // ==============================================================================
    /// <summary>
    /// This method creates the pages save commands objects.
    /// </summary>
    /// <returns>Evado.Model.UniForm.Page object</returns>
    //  ------------------------------------------------------------------------------
    private void setFormSaveGroupCommands (
      Evado.Model.UniForm.Group PageGroup )
    {
      this.LogMethod ( "setFormSaveGroupCommands" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Parameter parameter = new Evado.Model.UniForm.Parameter ( );
      Evado.Model.UniForm.Command pageCommand = new Model.UniForm.Command ( );

      if ( PageGroup == null )
      {
        return;
      }
      if ( PageGroup.Id == Guid.Empty )
      {
        return;
      }

      //
      // Add the save groupCommand for the page.
      //
      switch ( this.Session.Form.State )
      {
        case EdRecordObjectStates.Form_Draft:
          {
            //
            // Add the same groupCommand.
            //
            pageCommand = PageGroup.addCommand (
              EvLabels.Form_Save_Command_Title,
              EuAdapter.APPLICATION_ID,
              EuAdapterClasses.Project_Forms.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.Save_Object );

            // 
            // Define the save groupCommand parameters.
            // 
            pageCommand.SetGuid ( this.Session.Form.Guid );

            if ( this.Session.Form.State == EdRecordObjectStates.Form_Draft )
            {
              pageCommand.SetPageId ( EvPageIds.Form_Draft_Page );
            }
            else
            {
              pageCommand.SetPageId ( EvPageIds.Form_Page );
            }
            pageCommand.AddParameter (
              Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
             EdRecord.SaveActionCodes.Form_Saved.ToString ( ) );

            //
            // For a new form do not display the delete or review commands.
            //
            if ( this.Session.Form.Guid != Guid.Empty )
            {
              //
              // Add the same groupCommand.
              //
              pageCommand = PageGroup.addCommand (
                EvLabels.Form_Delete_Command_Title,
                EuAdapter.APPLICATION_ID,
                EuAdapterClasses.Project_Forms.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.Save_Object );

              // 
              // Define the save groupCommand parameters.
              // 
              pageCommand.SetGuid ( this.Session.Form.Guid );
              pageCommand.SetPageId ( EvPageIds.Form_Page );

              pageCommand.AddParameter (
                Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
               EdRecord.SaveActionCodes.Form_Deleted.ToString ( ) );
              //
              // Add the same groupCommand.
              //
              pageCommand = PageGroup.addCommand (
                EvLabels.Form_Review_Command_Title,
                EuAdapter.APPLICATION_ID,
                EuAdapterClasses.Project_Forms.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.Save_Object );

              // 
              // Define the save groupCommand parameters.
              // 
              pageCommand.SetGuid ( this.Session.Form.Guid );
              pageCommand.SetPageId ( EvPageIds.Form_Page );

              pageCommand.AddParameter (
                Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
               EdRecord.SaveActionCodes.Form_Reviewed.ToString ( ) );
            }

            return;
          }
        case EdRecordObjectStates.Form_Reviewed:
          {
            //
            // Add the same groupCommand.
            //
            pageCommand = PageGroup.addCommand (
              EvLabels.Form_Save_Command_Title,
              EuAdapter.APPLICATION_ID,
              EuAdapterClasses.Project_Forms.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.Save_Object );

            // 
            // Define the save groupCommand parameters.
            // 
            pageCommand.SetGuid ( this.Session.Form.Guid );
            pageCommand.SetPageId ( EvPageIds.Form_Page );

            pageCommand.AddParameter (
              Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
             EdRecord.SaveActionCodes.Form_Saved.ToString ( ) );

            //
            // Add the same groupCommand.
            //
            pageCommand = PageGroup.addCommand (
              EvLabels.Form_Approved_Command_Title,
              EuAdapter.APPLICATION_ID,
              EuAdapterClasses.Project_Forms.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.Save_Object );

            // 
            // Define the save groupCommand parameters.
            // 
            pageCommand.SetGuid ( this.Session.Form.Guid );
            pageCommand.SetPageId ( EvPageIds.Form_Page );

            pageCommand.AddParameter (
              Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
             EdRecord.SaveActionCodes.Form_Approved.ToString ( ) );

            return;
          }
        case EdRecordObjectStates.Form_Issued:
          {
            //
            // Add the same groupCommand.
            //
            pageCommand = PageGroup.addCommand (
              EvLabels.Form_Withdrawn_Command_Title,
              EuAdapter.APPLICATION_ID,
              EuAdapterClasses.Project_Forms.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.Save_Object );

            // 
            // Define the save groupCommand parameters.
            // 
            pageCommand.SetGuid ( this.Session.Form.Guid );

            pageCommand.AddParameter (
              Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
             EdRecord.SaveActionCodes.Form_Withdrawn.ToString ( ) );

            //
            // Add the copy groupCommand.
            //
            pageCommand = PageGroup.addCommand (
              EvLabels.Form_Copy_Form_Command_Title,
              EuAdapter.APPLICATION_ID,
              EuAdapterClasses.Project_Forms.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

            // 
            // Define the groupCommand parameters.
            // 
            pageCommand.SetGuid ( this.Session.Form.Guid );

            pageCommand.AddParameter (
            EuForms.CONST_FORM_COMMAND_PARAMETER,
            EuForms.CONST_COPY_FORM_COMMAND_VALUE );

            //
            // Add the review groupCommand.
            //
            pageCommand = PageGroup.addCommand (
              EvLabels.Form_Revise_Form_Command_Title,
              EuAdapter.APPLICATION_ID,
              EuAdapterClasses.Project_Forms.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

            // 
            // Define the groupCommand parameters.
            // 
            pageCommand.SetGuid ( this.Session.Form.Guid );

            pageCommand.AddParameter (
            EuForms.CONST_FORM_COMMAND_PARAMETER,
            EuForms.CONST_REVISE_FORM_COMMAND_VALUE );

            return;
          }
        default:
          {
            return;
          }
      }
    }//END setFormSaveCommands method

    // ==============================================================================
    /// <summary>
    /// This method gets the form ResultData object page.
    /// </summary>
    /// <param name="ClientDataObject">Evado.Model.UniForm.AppData object.</param>
    //  ------------------------------------------------------------------------------
    private void getFormDataObject (
      Evado.Model.UniForm.AppData ClientDataObject )
    {
      this.LogMethod ( "getFormDataObject" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );
      Evado.Model.UniForm.Field pageField = new Evado.Model.UniForm.Field ( );
      Evado.Model.UniForm.Parameter parameter = new Evado.Model.UniForm.Parameter ( );
      EuFormGenerator pageGenerator = new EuFormGenerator (
        this.ApplicationObjects,
        this.Session,
        this.ClassParameters );

      // 
      // Initialise the client ResultData object.
      // 
      ClientDataObject.Id = this.Session.Form.Guid;
      ClientDataObject.Title =
        String.Format ( EvLabels.Form_Page_Title,
          this.Session.Form.LayoutId,
          this.Session.Form.Title );

      ClientDataObject.Page.Id = ClientDataObject.Id;
      ClientDataObject.Page.PageDataGuid = ClientDataObject.Id;
      ClientDataObject.Page.PageId = this.Session.Form.LayoutId;
      ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      //
      // Set the user's edit access if they have configuration edit access.
      //
      this.LogValue ( "HasConfigrationEditAccess: " + this.Session.UserProfile.hasConfigrationEditAccess );

      if ( this.Session.UserProfile.hasConfigrationEditAccess == true )
      {
        ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
      }

      //
      // Add the save commandS for the page.
      //
      this.setFormPageCommands ( ClientDataObject.Page );

      this.LogValue ( "GENERATE FORM" );

      // 
      // Call the page generation method
      // 
      pageGenerator.generateForm (
        this.Session.Form,
        ClientDataObject.Page,
        this.UniForm_BinaryFilePath );

      this.LogValue ( pageGenerator.Log );

    }//END getFormDataObject Method

    // ==============================================================================
    /// <summary>
    /// This method creates the draft form client ResultData object.
    /// </summary>
    /// <param name="ClientDataObject">Evado.Model.UniForm.AppData object.</param>
    //  ------------------------------------------------------------------------------
    private void getFormDraftDataObject (
      Evado.Model.UniForm.AppData ClientDataObject )
    {
      this.LogMethod ( "getFormDataObject" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );

      // 
      // Initialise the client ResultData object.
      // 
      ClientDataObject.Id = this.Session.Form.Guid;
      ClientDataObject.Title =
        String.Format ( EvLabels.Form_Page_Title,
          this.Session.Form.LayoutId,
          this.Session.Form.Title );

      ClientDataObject.Page.Id = ClientDataObject.Id;
      ClientDataObject.Page.PageDataGuid = ClientDataObject.Id;
      ClientDataObject.Page.PageId = this.Session.Form.LayoutId;
      ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      //
      // Set the user's edit access if they have configuration edit access.
      //
      this.LogValue ( "HasConfigrationEditAccess: " + this.Session.UserProfile.hasConfigrationEditAccess );

      this.LogValue ( "GENERATE FORM" );

      //
      // Add the form header pageMenuGroup.
      //
      this.createDraftHeaderFields ( ClientDataObject.Page );


      if ( this.Session.UserProfile.hasConfigrationEditAccess == true )
      {
        ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
      }
      //
      // Add the save commandS for the page.
      //
      this.setFormPageCommands ( ClientDataObject.Page );

      //
      // Add the form sections groups.
      //
      if ( this.Session.Form.Design.FormSections.Count > 0 )
      {
        foreach ( EvFormSection formSection in this.Session.Form.Design.FormSections )
        {
          this.createDraftSectionFields ( formSection, ClientDataObject.Page );

        }//END section iteration loop

      }//END form section exist.

      //
      // add a pageMenuGroup with fields that are not allocated to a section.
      //
      this.createDraftNonSectionFields ( ClientDataObject.Page );

    }//END getFormDataObject Method

    // ==================================================================================    
    /// <summary>
    /// This method creates the draft form section update page pageMenuGroup.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    //  ----------------------------------------------------------------------------------
    private void createDraftHeaderFields (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "createDraftHeaderFields" );

      this.LogValue ( "Title : " + PageObject.Title );

      // 
      // Initialise the methods variables and objects.
      //
      Evado.Model.UniForm.Field pageField = new Evado.Model.UniForm.Field ( );
      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
        EvLabels.Form_Header_Group_Title,
        Evado.Model.UniForm.EditAccess.Enabled );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;

      //
      // Form title
      //
      pageField = pageGroup.createReadOnlyTextField (
        String.Empty,
        EvLabels.Label_Form_Id,
        this.Session.Form.LayoutId );
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;

      //
      // Form title
      //
      pageField = pageGroup.createReadOnlyTextField (
        String.Empty,
        EvLabels.Form_Title_Field_Label,
        this.Session.Form.Design.Title );
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;


      //
      // Form Instructions
      //
      if ( this.Session.Form.Design.Instructions != String.Empty )
      {
        pageField = pageGroup.createReadOnlyTextField (
          String.Empty,
          EvLabels.Form_Instructions_Field_Title,
          this.Session.Form.Design.Instructions );
        pageField.Layout = EuFormGenerator.ApplicationFieldLayout;
      }

      //
      // Form reference
      //
      if ( this.Session.Form.Design.Reference != String.Empty )
      {
        pageField = pageGroup.createReadOnlyTextField (
          String.Empty,
          EvLabels.Form_Reference_Field_Label,
          this.Session.Form.Design.Reference );
        pageField.Layout = EuFormGenerator.ApplicationFieldLayout;
      }

      //
      // Form category
      //
      if ( this.Session.Form.Design.RecordCategory != String.Empty )
      {
        pageField = pageGroup.createReadOnlyTextField (
          String.Empty,
          EvLabels.Form_Category_Field_Title,
          this.Session.Form.Design.RecordCategory );
        pageField.Layout = EuFormGenerator.ApplicationFieldLayout;
      }

      //
      // Form status
      //
      pageField = pageGroup.createReadOnlyTextField (
        String.Empty,
        EvLabels.Form_Status_Field_Title,
        this.Session.Form.StateDesc );
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;

      //
      // Form Version
      //
      pageField = pageGroup.createReadOnlyTextField (
        String.Empty,
        EvLabels.Form_Version_Field_Title,
        this.Session.Form.Design.stVersion );
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;

      return;

    }//END createDraftHeaderFields method

    // ==================================================================================    
    /// <summary>
    /// This method creates the draft form section update page pageMenuGroup.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    /// <param name="FormSection">EvFormSection object.</param>
    //  ----------------------------------------------------------------------------------
    private void createDraftSectionFields (
      EvFormSection FormSection,
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getDraftSectionFields" );

      this.LogValue ( "Title : " + PageObject.Title );

      // 
      // Initialise the methods variables and objects.
      //
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Command groupCommand = new Evado.Model.UniForm.Command ( );

      //
      // If the section has a title then include the fields in that section.
      //
      if ( FormSection.Title == String.Empty )
      {
        this.LogValue ( "Section.Section title is empty" );
        return;
      }

      //
      // The section has a title so add commands to open the sections field property page.
      //
      this.LogValue ( "Generating group for section : " + FormSection.Title );

      //
      // define the page pageMenuGroup for the section.
      //
      pageGroup = PageObject.AddGroup (
        FormSection.Title,
        FormSection.Instructions,
        Evado.Model.UniForm.EditAccess.Inherited_Access );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;
      pageGroup.CmdLayout = Model.UniForm.GroupCommandListLayouts.Vertical_Orientation;

      //
      // Add new field command.
      //
      groupCommand = pageGroup.addCommand (
        EvLabels.Form_New_Field_Page_Command_Title,
        EuAdapter.APPLICATION_ID,
        EuAdapterClasses.Project_Form_Fields.ToString ( ),
        Model.UniForm.ApplicationMethods.Get_Object );

      groupCommand.AddParameter ( Model.UniForm.CommandParameters.Create_Object, "1" );

      groupCommand.AddParameter ( Model.UniForm.CommandParameters.Page_Id,
        EvPageIds.Form_Field_Page.ToString ( ) );

      groupCommand.AddParameter ( EuFormFields.CONST_FIELD_COUNT,
        this.Session.Form.Fields.Count.ToString ( ) );

      groupCommand.AddParameter ( EuFormFields.CONST_FORM_SECTION,
        FormSection.No.ToString ( ) );

      groupCommand.SetBackgroundColour (
        Model.UniForm.CommandParameters.BG_Default,
        Model.UniForm.Background_Colours.Purple );

      //
      // Iterate through the fields in the form, selecting those fields that are
      // associated with the current section.
      //
      foreach ( EdRecordField field in this.Session.Form.Fields )
      {
        //
        // if the field is not in the section skip to the next field.
        //
        if ( field.Design.Section != FormSection.Title
          && field.Design.Section != FormSection.No.ToString ( ) )
        {
          continue;
        }

        //
        // Add the fields groupCommand object.
        // using the Guid for the unique field identifier.
        //
        groupCommand = pageGroup.addCommand (
          field.LinkText,
          EuAdapter.APPLICATION_ID,
          EuAdapterClasses.Project_Form_Fields.ToString ( ),
          Model.UniForm.ApplicationMethods.Get_Object );

        groupCommand.SetGuid ( field.Guid );

      }//END section field iteration loop.

    }//END getDraftSectionFields method

    // ==================================================================================    
    /// <summary>
    /// This method creates DraftRecords non section page pageMenuGroup objects.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    //  ----------------------------------------------------------------------------------
    private void createDraftNonSectionFields (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getDraftNonSectionFields" );

      // 
      // Initialise the methods variables and objects.
      //
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Command groupCommand = new Evado.Model.UniForm.Command ( );

      //
      // define the page pageMenuGroup for the section.
      //
      pageGroup = PageObject.AddGroup (
        EvLabels.Form_No_Section_Field_Group_Title,
        String.Empty,
        Evado.Model.UniForm.EditAccess.Inherited_Access );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;
      pageGroup.CmdLayout = Model.UniForm.GroupCommandListLayouts.Vertical_Orientation;

      //
      // Add new field command.
      //
      groupCommand = pageGroup.addCommand (
        EvLabels.Form_New_Field_Page_Command_Title,
        EuAdapter.APPLICATION_ID,
        EuAdapterClasses.Project_Form_Fields.ToString ( ),
        Model.UniForm.ApplicationMethods.Get_Object );

      groupCommand.AddParameter ( Model.UniForm.CommandParameters.Create_Object, "1" );

      groupCommand.AddParameter ( Model.UniForm.CommandParameters.Page_Id,
        EvPageIds.Form_Field_Page.ToString ( ) );

      groupCommand.AddParameter ( EuFormFields.CONST_FIELD_COUNT,
        this.Session.Form.Fields.Count.ToString ( ) );

      groupCommand.AddParameter ( EuFormFields.CONST_FORM_SECTION,
        String.Empty );

      groupCommand.SetBackgroundColour (
        Model.UniForm.CommandParameters.BG_Default,
        Model.UniForm.Background_Colours.Purple );

      //
      // Iterate through the fields in the form, selecting those fields that are
      // associated with the current section.
      //
      foreach ( EdRecordField field in this.Session.Form.Fields )
      {
        //
        // if the field is in a section skip to the next field.
        //
        if ( field.Design.Section != String.Empty )
        {
          continue;
        }

        //
        // Add the fields groupCommand object.
        // using the Guid for the unique field identifier.
        //
        groupCommand = pageGroup.addCommand (
          field.LinkText,
          EuAdapter.APPLICATION_ID,
          EuAdapterClasses.Project_Form_Fields.ToString ( ),
          Model.UniForm.ApplicationMethods.Get_Object );

        groupCommand.SetGuid ( field.Guid );

      }//END section field iteration loop.

    }//END getDraftNonSectionFields method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class create new form methods

    // ==================================================================================
    /// <summary>
    /// THis method creates a new form instance and displays the form properties page to the user.
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command PageCommand object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData createObject (
      Evado.Model.UniForm.Command PageCommand )
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
        this.Session.Form = new Evado.Model.Digital.EdRecord ( );
        this.Session.Form.Guid = Evado.Model.Digital.EvcStatics.CONST_NEW_OBJECT_ID;
        this.Session.Form.Design.Version = 0.0F;
        this.Session.Form.State = EdRecordObjectStates.Form_Draft;
        this.Session.Form.ApplicationId = this.Session.Application.ApplicationId;

        //
        // Set the form type to the current setting if it is a project form.
        //
        if ( this.Session.FormType != EvFormRecordTypes.Informed_Consent
          && this.Session.FormType != EvFormRecordTypes.Patient_Record
          && this.Session.FormType != EvFormRecordTypes.Periodic_Followup
          && this.Session.FormType != EvFormRecordTypes.Questionnaire
          && this.Session.FormType != EvFormRecordTypes.Updatable_Record )
        {
          this.Session.Form.Design.TypeId = EvFormRecordTypes.Normal_Record;
        }
        else
        {
          this.Session.Form.Design.TypeId = this.Session.FormType;
        }
        // 
        // Save the customer object to the session
        // 


        // 
        // Generate the new page layout 
        // 
        this.getPropertiesDataObject ( clientDataObject );

        this.LogValue ( "Exit createObject method. ID: " + clientDataObject.Id + ", Title: " + clientDataObject.Title );

        // 
        // Return the ResultData object.
        // 
        return clientDataObject;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Record_Create_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return null;

    }//END method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class update form methods

    // ==================================================================================
    /// <summary>
    /// This method saves the ResultData object updating the field values contained in the 
    /// parameter list and returns an exit page.
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.Command object.</param>
    /// <remarks>
    /// This method consists of following steps:
    /// 
    /// 1. retrieved the form based on the Form Guid
    /// 
    /// 2. Updates the object values and the section values of the form.
    /// 
    /// 3. Saves the updated values to the respective tables in the Evado Database.
    /// </remarks>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData updateObject ( Evado.Model.UniForm.Command PageCommand )
    {
      try
      {
        this.LogInitMethod ( "updateObject" );

        this.LogValue ( "Parameter PageCommand: "
          + PageCommand.getAsString ( false, false ) );

        this.LogDebug ( "SessionObjects.Forms" + " Guid: " + this.Session.Form.Guid );
        this.LogDebug ( " ProjectId: " + this.Session.Form.ApplicationId );
        this.LogDebug ( " Title: " + this.Session.Form.Title );

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "updateObject",
          this.Session.UserProfile );

        //
        // Rung the server script if server side scripts are enabled.
        //
        Guid formGuid = PageCommand.GetGuid ( );

        //
        // If the form is being copied to a new project the form object will be set to empty.
        // this step refills the object for it to be updated.
        //
        if ( this.Session.Form.Guid == Guid.Empty
          && formGuid != Guid.Empty )
        {
          this.Session.Form = this._Bll_Forms.getForm ( formGuid );
        }

        //
        // Rung the server script if server side scripts are enabled.
        //
        this.runServerScript ( EvServerPageScript.ScripEventTypes.OnUpdate );

        // 
        // Initialise the update parameter values.
        // 
        if ( this.Session.Form.Guid == Evado.Model.Digital.EvcStatics.CONST_NEW_OBJECT_ID )
        {
          this.Session.Form.Guid = Guid.Empty;
        }
        this.Session.Form.UpdatedByUserId = this.Session.UserProfile.UserId;
        this.Session.Form.UserCommonName = this.Session.UserProfile.CommonName;

        // 
        // As Evado eClinical does not have a delete record function return true if groupCommand sent.
        // 
        if ( PageCommand.Method == Evado.Model.UniForm.ApplicationMethods.Delete_Object )
        {
          return new Model.UniForm.AppData ( );
        }

        // 
        // Update the object.
        // 
        this.updateObjectValues ( PageCommand );

        //
        // Update the section table values.
        //
        this.updateSectionValues ( PageCommand );

        //
        // Validate that the field identifier is valid.
        //
        if ( this.validateFormId ( PageCommand ) == false )
        {
          this.Session.LastPage.Message = this.ErrorMessage;
          return this.Session.LastPage;
        }


        String prefix = this.Session.Form.LayoutId.Substring ( 0, 2 );

        //
        // IF the prefix exists then remove it from the form id
        //
        if ( prefix == EvcStatics.CONST_GLOBAL_FORM_PREFIX )
        {
          Char [ ] cFormId = this.Session.Form.LayoutId.ToCharArray ( );

          cFormId [ 0 ] = ' ';
          cFormId [ 1 ] = ' ';

          this.Session.Form.LayoutId = cFormId.ToString ( ).Trim ( );
        }


        // 
        // Get the save action message value.
        // 
        this.Session.Form.SaveAction = EdRecord.SaveActionCodes.Form_Saved;
        String saveAction = PageCommand.GetParameter ( Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION );
        if ( saveAction != String.Empty )
        {
          this.Session.Form.SaveAction =
             Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue<EdRecord.SaveActionCodes> ( saveAction );
        }
        this.LogValue ( "Save Action: " + this.Session.Form.SaveAction );

        // 
        // Resets the save action to the parameter passed in the page groupCommand.
        // 
        if ( this.Session.Form.SaveAction == EdRecord.SaveActionCodes.Save )
        {
          this.Session.Form.SaveAction = EdRecord.SaveActionCodes.Form_Saved;
        }

        // 
        // Execute the save record groupCommand to save the record values to the 
        // Evado database.
        // 
        EvEventCodes result = this._Bll_Forms.SaveItem (
          this.Session.Form );

        this.LogValue ( this._Bll_Forms.Log );

        // 
        // If an error state is returned log the event.
        // 
        if ( result != EvEventCodes.Ok )
        {
          this.ErrorMessage = String.Format (
            EvLabels.Form_Update_Error_Message,
            EvStatics.getEventMessage ( result ) );

          string sStEvent = "Returned error message: "
            + result + " = " + Evado.Model.Digital.EvcStatics.getEventMessage ( result )
            + "\r\n" + this._Bll_Forms.Log;

          this.LogError ( EvEventCodes.Database_Record_Update_Error, sStEvent );

          this.Session.LastPage.Message = this.ErrorMessage;

          return this.Session.LastPage;
        }

        //
        // If a revision or copy is made rebuild the list
        //
        this.Session.FormList = new List<EdRecord> ( );

        this.LogMethodEnd ( "updateObject" );

        return new Model.UniForm.AppData ( );

      }
      catch ( Exception Ex )
      {
        // 
        // If an exception is raised create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Record_Update_Error_Message;

        // 
        // Log the error event to the server log and DB event log.
        // 
        this.LogException ( Ex );
      }

      this.LogMethodEnd ( "updateObject" );
      Session.LastPage.Message = this.ErrorMessage;
      return this.Session.LastPage;

    }//END method

    // ==================================================================================
    /// <summary>
    /// THis method validates that there is not a duplicate form identifier.
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command objects.</param>
    /// <returns>Bool: True is field ID is validate.</returns>
    //  ----------------------------------------------------------------------------------
    private bool validateFormId ( Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "validateFormId" );
      this.LogValue ( "FormField.Guid: " + this.Session.FormField.Guid + ", FieldId: " + this.Session.FormField.FieldId );

      //
      // Replace periods in the field identifier with underscore
      //
      this.Session.FormField.FieldId = this.Session.FormField.FieldId.Replace ( " ", "_" );

      //
      // Iterate through the form fields checking to ensure there are not duplicate field identifiers.
      //
      foreach ( EdRecord form in this.Session.FormList )
      {
        this.LogValue ( "Form.Guid: " + form.Guid + ", FormId: " + form.LayoutId );

        if ( form.LayoutId == this.Session.Form.LayoutId
          && this.Session.Form.Guid != form.Guid
          && this.Session.Form.State == form.State )
        {
          this.ErrorMessage = String.Format ( EvLabels.Form_Duplicate_ID_Error_Message,
            this.Session.Form.LayoutId );

          this.LogValue ( this.ErrorMessage );
          return false;
        }
      }//END iteration statement

      return true;
    }//END validateFormId method.

    // ==================================================================================
    /// <summary>
    /// Ttis method updates the form record values with the groupCommand parameter values.
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command objects.</param>
    //  ----------------------------------------------------------------------------------
    private void updateObjectValues (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogInitMethod ( "updateObjectValue" );
      this.LogValue ( "PageCommand.Parameters.Count: " + PageCommand.Parameters.Count );

      // 
      // Iterate through the parameter values updating the ResultData object
      // 
      foreach ( Evado.Model.UniForm.Parameter parameter in PageCommand.Parameters )
      {
        if ( parameter.Name.Contains ( Evado.Model.Digital.EvcStatics.CONST_GUID_IDENTIFIER ) == true
          || parameter.Name.Contains ( EuForms.CONST_SECTION_FIELD_PREFIX ) == true
          || parameter.Name.Contains ( Evado.Model.UniForm.Field.CONST_FIELD_ANNOTATION_SUFFIX ) == true
          || parameter.Name.Contains ( Evado.Model.UniForm.Field.CONST_FIELD_QUERY_SUFFIX ) == true
          || parameter.Name == Evado.Model.UniForm.CommandParameters.Custom_Method.ToString ( )
          || parameter.Name == Evado.Model.UniForm.CommandParameters.Page_Id.ToString ( )
          || parameter.Name == Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION
          || parameter.Name == EuForms.CONST_IMPORT_FORM_DATA
          || parameter.Name == EuForms.CONST_EXPORT_FORM_DATA
          || parameter.Name == EuFormGenerator.CONST_FORM_COMMENT_FIELD_ID
          || this.isFormField ( parameter.Name ) == true )
        {
          this.LogDebug ( parameter.Name + " > " + parameter.Value + " >> SKIPPED" );
          continue;
        }

        this.LogDebug ( parameter.Name + " > " + parameter.Value + " >> UPDATED" );
        try
        {
          EdRecord.FormClassFieldNames fieldName = Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue<EdRecord.FormClassFieldNames> ( parameter.Name );

          this.Session.Form.setValue ( fieldName, parameter.Value );

        }
        catch ( Exception Ex )
        {
          this.LogException ( Ex );
        }

      }// End iteration loop

      this.LogMethodEnd ( "updateObjectValue" );

    }//END updateObjectValue method.

    // ==================================================================================
    /// <summary>
    /// Ttis method method tests to ensure that informed consent forms have the 
    /// confirmed consent  field.
    /// </summary>
    /// <param name="ParameterName">String parameter name </param>
    /// <returns>True: found</returns>
    //  ----------------------------------------------------------------------------------
    private bool isFormField ( String ParameterName )
    {
      this.LogInitMethod ( "isFormField" );

      foreach ( EdRecordField field in this.Session.Form.Fields )
      {
        if ( field.FieldId == ParameterName )
        {
          return true;
        }
      }
      return false;
    }

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
  }
}//END namespace