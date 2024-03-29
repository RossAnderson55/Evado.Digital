﻿/***************************************************************************************
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
    #region Class Initialisation
    /// <summary>
    /// This method initialises the class.
    /// </summary>
    public EuRecordLayouts ( )
    {
      this.ClassNameSpace = "Evado.UniForm.Digital.EuRecordLayouts.";
    }

    /// <summary>
    /// This method initialises the class and passs in the user profile.
    /// </summary>
    public EuRecordLayouts (
      EuGlobalObjects ApplicationObjects,
      EvUserProfileBase ServiceUserProfile,
      EuSession SessionObjects,
      String UniFormBinaryFilePath,
      EvClassParameters Settings )
    {
      this.AdapterObjects = ApplicationObjects;
      this.ServiceUserProfile = ServiceUserProfile;
      this.Session = SessionObjects;
      this.UniForm_BinaryFilePath = UniFormBinaryFilePath;
      this.ClassParameters = Settings;
      this.ClassNameSpace = "Evado.UniForm.Digital.EuRecordLayouts.";


      this.LogInitMethod ( "EuRecordLayouts initialisation" );
      this.LogInit ( "-ServiceUserProfile.UserId: " + ServiceUserProfile.UserId );
      this.LogInit ( "-SessionObjects.UserProfile.CommonName: " + this.Session.UserProfile.CommonName );
      this.LogInit ( "-UniFormBinaryFilePath: " + this.UniForm_BinaryFilePath );

      this.LogInit ( "Settings:" );
      this.LogInit ( "-LoggingLevel: " + Settings.LoggingLevel );
      this.LogInit ( "-UserId: " + Settings.UserProfile.UserId );
      this.LogInit ( "-UserCommonName: " + Settings.UserProfile.CommonName );

      this._Bll_RecordLayouts = new EdRecordLayouts ( Settings );

      if ( this.AdapterObjects.AllRecordLayouts == null )
      {
        this.AdapterObjects.AllRecordLayouts = new List<EdRecord> ( );
      }

      if ( this.Session.RecordLayout == null )
      {
        this.Session.RecordLayout = new EdRecord ( );
      }

      if ( this.Session.RecordField == null )
      {
        this.Session.RecordField = new EdRecordField ( );
      }

      if ( this.Session.Selected_RecordLayoutId == null )
      {
        this.Session.Selected_EntityLayoutId = String.Empty;
      }

    }//END Method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class constants and variables.

    private Evado.Digital.Bll.EdRecordLayouts _Bll_RecordLayouts = new Evado.Digital.Bll.EdRecordLayouts ( );
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
    /// <param name="PageCommand">ClientPateEvado.UniForm.Model.EuCommand object</param>
    /// <returns>ClientApplicationData</returns>
    //  ----------------------------------------------------------------------------------
    override public Evado.UniForm.Model.EuAppData getDataObject (
      Evado.UniForm.Model.EuCommand PageCommand )
    {
      this.LogMethod ( "getClientDataObject" );
      this.LogValue ( "PageCommand: " + PageCommand.getAsString ( false, true ) );

      try
      {
        // 
        // Initialise the methods variables and objects.
        // 
        Evado.UniForm.Model.EuAppData clientDataObject = new Evado.UniForm.Model.EuAppData ( );

        //
        // Set the page Id
        //
        if ( this.Session.PageId != Evado.Digital.Model.EdStaticPageIds.Form_Draft_Page.ToString ( )
          && this.Session.PageId != Evado.Digital.Model.EdStaticPageIds.Form_Properties_Page.ToString ( )
          && this.Session.PageId != Evado.Digital.Model.EdStaticPageIds.Entity_Layout_Page.ToString ( ) )
        {
          this.Session.PageId = Evado.Digital.Model.EdStaticPageIds.Form_Draft_Page.ToString ( );
          if ( this.Session.RecordLayout.State == EdRecordObjectStates.Form_Issued )
          {
            this.Session.PageId = Evado.Digital.Model.EdStaticPageIds.Entity_Layout_Page.ToString ( );
          }
        }

        String stPageId = PageCommand.GetParameter (
          Evado.UniForm.Model.EuCommandParameters.Page_Id );

        if ( stPageId != String.Empty )
        {
          this.Session.PageId = stPageId;
        }
        this.LogValue ( "this.Session.PageId : " + this.Session.PageId );

        string value = PageCommand.GetParameter ( EuRecordLayouts.CONST_UPDATE_SECTION_COMMAND_PARAMETER );

        //
        // update the selection values.
        //
        this.updateSessionValue ( PageCommand );

        //
        // Update the session form object if a new section is added to the page.
        //
        if ( value == "1"
          && ( this.Session.PageId == Evado.Digital.Model.EdStaticPageIds.Form_Properties_Section_Page.ToString ( )
            || this.Session.PageId == Evado.Digital.Model.EdStaticPageIds.Form_Properties_Page.ToString ( ) ) )
        {
          //
          // Update the section table values.
          //
          this.updateSectionValues ( PageCommand );
          // 
          // Update the object.
          // 
          this.updateObjectValues ( PageCommand );
        }
        this.LogDebug ( "Section Count: {0}.", this.Session.RecordLayout.Design.FormSections.Count );

        // 
        // Determine the method to be called
        // 
        switch ( PageCommand.Method )
        {
          // 
          // Generate a page containing a list of record commands
          // 
          case Evado.UniForm.Model.EuMethods.List_of_Objects:
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
          case Evado.UniForm.Model.EuMethods.Get_Object:
            {
              //
              // Reset global parameters for opening the form.
              //
              this.Session.RecordLayout.SaveAction = EdRecord.SaveActionCodes.Save;

              //
              // Display the form properties page if the view type set to property page.
              //
              switch ( this.Session.StaticPageId )
              {
                case Evado.Digital.Model.EdStaticPageIds.Form_Properties_Page:
                  {
                    clientDataObject = this.GetRecordProperties_Object ( PageCommand );
                    break;
                  }
                case Evado.Digital.Model.EdStaticPageIds.Form_Properties_Section_Page:
                  {
                    clientDataObject = this.getPropertiesSectionObject ( PageCommand );
                    break;
                  }
                case Evado.Digital.Model.EdStaticPageIds.Form_Draft_Page:
                  {
                    clientDataObject = this.getDraftLayoutObject ( PageCommand );
                    break;
                  }
                case Evado.Digital.Model.EdStaticPageIds.Form_Template_Upload:
                  {
                    clientDataObject = this.getTemplateUploadPage ( PageCommand );
                    break;
                  }
                case Evado.Digital.Model.EdStaticPageIds.Form_Template_Download:
                  {
                    clientDataObject = this.getTemplateDownloadPage ( PageCommand );
                    break;
                  }
                default:
                  {
                    clientDataObject = this.getEntityLayoutObject ( PageCommand );
                    break;
                  }
              }
              break;

            }//END get object case

          // 
          // Select the groupCommand to create a new record object.
          // 
          case Evado.UniForm.Model.EuMethods.Create_Object:
            {
              clientDataObject = this.createObject ( PageCommand ); ;

              this.LogDebug ( Evado.Model.EvStatics.SerialiseXmlObject<Evado.UniForm.Model.EuAppData> ( clientDataObject ) );

              break;
            }//END create case

          // 
          // Select the method to update the record object.
          // 
          case Evado.UniForm.Model.EuMethods.Save_Object:
          case Evado.UniForm.Model.EuMethods.Delete_Object:
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
      return new Evado.UniForm.Model.EuAppData ( );

    }//END getRecordObject methods

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.EuCommand object.</param>
    // ------------------------------------------------------------------------------
    private void updateSessionValue (
      Evado.UniForm.Model.EuCommand PageCommand )
    {
      this.LogMethod ( "updateSessionValue" );

      //
      // if the command has a customer method parameter it is a selection update command so reset the record layouts.
      //
      if ( PageCommand.hasParameter ( Evado.UniForm.Model.EuCommandParameters.Custom_Method ) == true )
      {
        this.AdapterObjects.AllRecordLayouts = new List<EdRecord> ( );
      }

      if ( PageCommand.hasParameter ( EdRecord.CONST_RECORD_TYPE ) == true )
      {
        var recordType = PageCommand.GetParameter<EdRecordTypes> ( EdRecord.CONST_RECORD_TYPE );

        if ( this.Session.Selected_RecordType != recordType )
        {
          this.Session.Selected_RecordType = recordType;
          this.AdapterObjects.AllRecordLayouts = new List<EdRecord> ( );
        }
      }
      this.LogValue ( "RecordType: " + this.Session.Selected_RecordType );


      if ( PageCommand.hasParameter ( EdRecord.FieldNames.Status.ToString ( ) ) == true )
      {
        var stateValue = PageCommand.GetParameter<EdRecordObjectStates> ( EdRecord.FieldNames.Status.ToString ( ) );

        if ( this.Session.Selected_RecordState != stateValue )
        {
          this.AdapterObjects.AllRecordLayouts = new List<EdRecord> ( );

          if ( stateValue != EdRecordObjectStates.Null )
          {
            this.Session.Selected_RecordState = stateValue;
          }
          else
          {
            this.Session.Selected_RecordState = EdRecordObjectStates.Null;
          }
        }
      }
      this.LogValue ( "RecordSelectionState: " + this.Session.Selected_RecordState );

      // 
      // Set the page type to control the DB query type.
      // 
      string pageId = PageCommand.GetPageId ( );

      this.LogValue ( "PageCommand pageId: " + pageId );

      if ( pageId != String.Empty )
      {
        this.Session.setPageId ( pageId );
      }
      this.LogValue ( "PageId: " + this.Session.PageId );

      this.LogMethodEnd ( "updateSessionValue" );

    }//END updateSessionValue method.

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
      this.LogValue ( "RecordId " + this.Session.RecordLayout.LayoutId );
      this.LogValue ( "hasCsScript = " + this.Session.RecordLayout.Design.hasCsScript );

      // 
      // if the formField has a CS Script execute the onPostBackForm method.
      // 
      if ( this.Session.RecordLayout.Design.hasCsScript == true )
      {
        this.LogValue ( "Server script executing." );

        //
        // Define the page to retrieve the script
        //
        this._ServerPageScript.CsScriptPath = this.AdapterObjects.ApplicationPath + @"csscripts\";


        // 
        // Execute the onload script.
        // 
        EvEventCodes iReturn = this._ServerPageScript.runScript (
          ScriptType,
          this.Session.RecordLayout );

        this.LogValue ( "Server page script debug log: " + this._ServerPageScript.DebugLog );

        this.LogValue ( "Form.ScriptMessage: " + this.Session.RecordLayout.ScriptMessage );

        if ( iReturn != EvEventCodes.Ok )
        {
          this.ErrorMessage =
            "CsScript:" + ScriptType + " method failed \r\n"
            + Evado.Model.EvStatics.enumValueToString ( iReturn ) + "\r\n";

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
    /// <param name="PageCommand">Evado.UniForm.Model.EuCommand object.</param>
    /// <remarks>
    /// 1.This method queries the database to fetch the list of forms based on project Id, form state and form type.
    /// 
    /// 2. The retreived records are assigned to a selection group where the desired form can be selected.
    /// </remarks>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.UniForm.Model.EuAppData getListObject (
      Evado.UniForm.Model.EuCommand PageCommand )
    {
      try
      {
        this.LogMethod ( "getListObject" );

        // 
        // Initialise the methods variables and objects.
        // 
        Evado.UniForm.Model.EuAppData clientDataObject = new Evado.UniForm.Model.EuAppData ( );
        this.Session.PageId = String.Empty;

        // 
        // If the user does not have monitor or ResultData manager roles exit the page.
        // 
        if ( this.Session.UserProfile.hasManagementAccess == false )
        {
          this.LogIllegalAccess (
            this.ClassNameSpace + "getListObject",
            this.Session.UserProfile );

          this.ErrorMessage = EdLabels.Record_Access_Error_Message;

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
        clientDataObject.Title = EdLabels.Form_Selection_Page_Title;
        clientDataObject.Page.Title = clientDataObject.Title;
        clientDataObject.Page.PageId = Evado.Digital.Model.EdStaticPageIds.Record_Layout_View.ToString ( );

        //
        // Define the icon urls.
        //
        clientDataObject.Page.setImageUrl (
          Evado.UniForm.Model.EuPageImageUrls.Image0_Url,
          EuRecordLayouts.ICON_FORM_DRAFT );

        clientDataObject.Page.setImageUrl (
          Evado.UniForm.Model.EuPageImageUrls.Image1_Url,
          EuRecordLayouts.ICON_FORM_REVIEWED );

        clientDataObject.Page.setImageUrl (
          Evado.UniForm.Model.EuPageImageUrls.Image2_Url,
          EuRecordLayouts.ICON_FORM_ISSUED );

        //
        // Define the page commands
        //
        this.createEntitList_PageCommands ( clientDataObject.Page );
        // 
        // Create the new pageMenuGroup for query selection.
        // 
        this.createFormSelectionGroup ( clientDataObject.Page );

        //
        // execute the form list query.
        //
        this.loadRecordLayoutList ( );

        // 
        // Create the pageMenuGroup containing commands to open the records.
        // 
        this.createRecordLayoutList_Group ( clientDataObject.Page );

        this.LogValue ( " data.Title: " + clientDataObject.Title );
        this.LogValue ( " data.Page.Title: " + clientDataObject.Page.Title );

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

      return null;

    }//END getListObject method.

    // ==============================================================================
    /// <summary>
    /// This method creates the record view pageMenuGroup containing a list of commands to 
    /// open the form record.
    /// </summary>
    /// <param name="Page">Evado.UniForm.Model.EuPage object.</param>
    /// <param name="FormList">List<EvForm> object.</param>
    /// <returns>Evado.UniForm.Model.EuGroup</returns>
    //  ------------------------------------------------------------------------------
    private void createEntitList_PageCommands (
      Evado.UniForm.Model.EuPage Page )
    {
      this.LogMethod ( "createFormList_PageCommands" );

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.EuParameter parameter = new Evado.UniForm.Model.EuParameter ( );
      Evado.UniForm.Model.EuCommand pageCommand = new Evado.UniForm.Model.EuCommand ( );

      // 
      // Add the selection groupCommand
      // 
      pageCommand = Page.addCommand (
        EdLabels.EntittLayout_List_Refresh_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Record_Layouts.ToString ( ),
         Evado.UniForm.Model.EuMethods.Custom_Method );
      pageCommand.setCustomMethod ( Evado.UniForm.Model.EuMethods.List_of_Objects );
      pageCommand.AddParameter ( EuRecordLayouts.CONST_REFRESH, "1" );

      //
      // Record template upload command
      //
      pageCommand = Page.addCommand (
        EdLabels.Form_Template_Upload_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Record_Layouts.ToString ( ),
              Evado.UniForm.Model.EuMethods.Get_Object );

      // 
      // Define the save groupCommand parameters.
      // 
      pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Form_Template_Upload );
    }

    //===================================================================================
    /// <summary>
    /// This method loads the form list.
    /// </summary>
    //-----------------------------------------------------------------------------------
    private void loadRecordLayoutList ( )
    {
      this.LogMethod ( "loadRecordLayoutList" );
      this.LogDebug ( "Selected_RecordType '{0}'", this.Session.Selected_RecordType );
      this.LogDebug ( "RecordFormState '{0}'", this.Session.Selected_RecordLayoutState );

      if ( this.AdapterObjects.AllRecordLayouts.Count > 0 )
      {
        this.LogMethod ( "loadRecordLayoutList method" );
        return;
      }

      this.LogDebug ( "Selected_RecordLayoutState: '" + this.Session.Selected_RecordLayoutState + "'" );
      this.LogDebug ( "Selected_RecordType: '" + this.Session.Selected_RecordType + "'" );

      // 
      // Query the database to retrieve a list of the records matching the query parameter values.
      // 
      this.AdapterObjects.AllRecordLayouts = this._Bll_RecordLayouts.getLayoutList (
        this.Session.Selected_RecordType,
        this.Session.Selected_RecordLayoutState );

      this.LogDebugClass ( this._Bll_RecordLayouts.Log );
      this.LogDebug ( "Layout list count: " + this.AdapterObjects.AllRecordLayouts.Count );

      this.LogMethodEnd ( "loadRecordLayoutList" );
    }//ENd loadRecordLayoutList method

    // ==============================================================================
    /// <summary>
    /// This method copy or revises the selected form.
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.EuCommand object.</param>
    //  ------------------------------------------------------------------------------
    private void copyForm (
      Evado.UniForm.Model.EuCommand PageCommand )
    {
      try
      {
        this.LogMethod ( "copyForm" );

        // 
        // Initialise the methods variables and objects.
        // 
        EdRecordLayouts forms = new EdRecordLayouts ( );
        String stCommandAction = PageCommand.GetParameter ( EuRecordLayouts.CONST_FORM_COMMAND_PARAMETER );
        this.LogDebug ( " stCommandAction: " + stCommandAction );

        //
        // if the groupCommand action is not a revise or copy groupCommand exit.
        //
        if ( stCommandAction != EuRecordLayouts.CONST_COPY_FORM_COMMAND_VALUE
          && stCommandAction != EuRecordLayouts.CONST_REVISE_FORM_COMMAND_VALUE )
        {
          this.LogValue ( "EXIT: not a revision or copy command." );
          return;
        }

        // 
        // If the user does not have monitor or ResultData manager roles exit the page.
        // 
        if ( this.Session.UserProfile.hasManagementAccess == false )
        {
          this.LogIllegalAccess (
            this.ClassNameSpace + "copyForm",
            this.Session.UserProfile );

          this.ErrorMessage = EdLabels.Record_Access_Error_Message;

          return;
        }

        // 
        // Log the user's access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "copyForm",
          this.Session.UserProfile );

        //
        // get the form guid.
        //
        Guid layoutGuid = PageCommand.GetGuid ( );
        this.LogDebug ( " layoutGuid: " + layoutGuid );

        // 
        // if the guid is empty the parameter was not found to exit.
        // 
        if ( layoutGuid == Guid.Empty )
        {
          this.LogEvent ( "copyForm is EMPTY" );

          return;
        }

        // 
        // Retrieve the record object from the database via the DAL and BLL layers.
        // 
        if ( layoutGuid != this.Session.RecordLayout.Guid )
        {
          this.Session.RecordLayout = this._Bll_RecordLayouts.GetLayout ( layoutGuid );

          this.LogClass ( this._Bll_RecordLayouts.Log );
        }

        this.LogDebug ( "SessionObjects.EntityLayout.LayoutId: " + this.Session.RecordLayout.LayoutId );

        //
        // Call the method to revise the form layout.
        //
        if ( stCommandAction == EuRecordLayouts.CONST_REVISE_FORM_COMMAND_VALUE )
        {
          this.LogDebug ( "Revising the form layout" );
          this._Bll_RecordLayouts.ReviseForm ( this.Session.RecordLayout );
        }

        //
        // call the method to copy the form layout.
        //
        if ( stCommandAction == EuRecordLayouts.CONST_COPY_FORM_COMMAND_VALUE )
        {
          this.LogDebug ( "Copying the form layout" );
          this._Bll_RecordLayouts.CopyForm ( this.Session.RecordLayout );
        }

        //
        // If a revision or copy is made rebuild the list
        //
        this.AdapterObjects.AllRecordLayouts = new List<EdRecord> ( );

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

    }//END getListObject method.

    // ==============================================================================
    /// <summary>
    /// This method creates the form selection pageMenuGroup.
    /// </summary>
    /// <returns>Evado.UniForm.Model.EuGroup</returns>
    //  ------------------------------------------------------------------------------
    private void createFormSelectionGroup (
      Evado.UniForm.Model.EuPage ClientPage )
    {
      this.LogMethod ( "getList_PageComands" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.EuGroup pageGroup = new Evado.UniForm.Model.EuGroup ( );
      Evado.UniForm.Model.EuCommand command = new Evado.UniForm.Model.EuCommand ( );
      Evado.UniForm.Model.EuField selectionField = new Evado.UniForm.Model.EuField ( );
      List<EvOption> optionList = new List<EvOption> ( );

      // 
      // Create the new pageMenuGroup for query selection.
      // 
      pageGroup = ClientPage.AddGroup (
        EdLabels.Form_Selection_Group_Title,
        Evado.UniForm.Model.EuEditAccess.Enabled );

      pageGroup.GroupType = Evado.UniForm.Model.EuGroupTypes.Default;
      pageGroup.Layout = Evado.UniForm.Model.EuGroupLayouts.Full_Width;
      pageGroup.AddParameter ( Evado.UniForm.Model.EuGroupParameters.Offline_Hide_Group, true );

      // 
      // Display form type selection list
      // 
      optionList = EdRecord.getFormStates ( );
      optionList [ 0 ].Value = EdRecordTypes.Null.ToString ( );

      selectionField = pageGroup.createSelectionListField (
        EdRecord.FieldNames.Status.ToString ( ),
        EdLabels.Form_State_Selection_Label,
        this.Session.Selected_RecordLayoutState.ToString ( ),
        optionList );

      selectionField.Layout = EuAdapter.DefaultFieldLayout;
      selectionField.AddParameter ( Evado.UniForm.Model.EuFieldParameters.Snd_Cmd_On_Change, 1 );

      // 
      // Add the selection groupCommand
      // 
      command = pageGroup.addCommand (
        EdLabels.Select_Layout_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Record_Layouts.ToString ( ),
         Evado.UniForm.Model.EuMethods.Custom_Method );
      command.setCustomMethod ( Evado.UniForm.Model.EuMethods.List_of_Objects );

    }//END createRecordSelectionGroup method

    // ==============================================================================
    /// <summary>
    /// This method updates the session objects..
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.EuCommand object.</param>
    //  ------------------------------------------------------------------------------
    private void updateSelectionValues (
      Evado.UniForm.Model.EuCommand PageCommand )
    {
      this.LogMethod ( "updateSelectionValues" );
      // 
      // Initialise the methods variables and objects.
      // 
      String parameterValue = String.Empty;

      // 
      // IF the refresh parameter exists the empty the entity lists so they are reloaded.
      // 
      if ( PageCommand.hasParameter ( EuRecordLayouts.CONST_REFRESH ) == true )
      {
        this.AdapterObjects.AllRecordLayouts = new List<EdRecord> ( );

        this.Session.RecordList = new List<EdRecord> ( );
      }

      // 
      // Get the form record type parameter value
      // 
      if ( PageCommand.hasParameter ( EdRecord.FieldNames.Status.ToString ( ) ) == true )
      {
        parameterValue = PageCommand.GetParameter ( EdRecord.FieldNames.Status.ToString ( ) );

        this.LogValue ( "Selected Form Type: " + parameterValue );

        this.Session.Selected_RecordLayoutState = Evado.Model.EvStatics.parseEnumValue<EdRecordObjectStates> ( parameterValue );
      }
      this.LogValue ( "SessionObjects.FormState: "
        + this.Session.Selected_RecordLayoutState );

    }//END updateSessionObjects method

    // ==============================================================================
    /// <summary>
    /// This method creates the record view pageMenuGroup containing a list of commands to 
    /// open the form record.
    /// </summary>
    /// <param name="Page">Evado.UniForm.Model.EuPage object.</param>
    /// <param name="FormList">List<EvForm> object.</param>
    /// <returns>Evado.UniForm.Model.EuGroup</returns>
    //  ------------------------------------------------------------------------------
    private void createRecordLayoutList_Group (
      Evado.UniForm.Model.EuPage Page )
    {
      this.LogMethod ( "createRecordLayoutList_Group" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.EuGroup pageGroup = new Evado.UniForm.Model.EuGroup ( );
      Evado.UniForm.Model.EuCommand groupCommand = new Evado.UniForm.Model.EuCommand ( );

      // 
      // Create the record display pageMenuGroup.
      // 
      pageGroup = Page.AddGroup (
        EdLabels.Form_List_Label,
        Evado.UniForm.Model.EuEditAccess.Enabled );
      pageGroup.CmdLayout = Evado.UniForm.Model.EuGroupCommandListLayouts.Vertical_Orientation;

      pageGroup.Layout = Evado.UniForm.Model.EuGroupLayouts.Full_Width;

      pageGroup.Title += EdLabels.List_Count_Label + this.AdapterObjects.AllRecordLayouts.Count;

      //
      // Add new form button if the user has configuration write access.
      //
      if ( this.Session.UserProfile.hasManagementAccess == true )
      {
        groupCommand = pageGroup.addCommand (
          EdLabels.Entity_List_New_Layout_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Record_Layouts.ToString ( ),
          Evado.UniForm.Model.EuMethods.Create_Object );

        groupCommand.SetBackgroundColour (
          Evado.UniForm.Model.EuCommandParameters.BG_Default,
          Evado.UniForm.Model.EuBackgroundColours.Purple );

      }//END user had configurtion write access

      // 
      // Iterate through the record list generating a groupCommand to access each record
      // then append the groupCommand to the record pageMenuGroup view's groupCommand list.
      // 
      foreach ( Evado.Digital.Model.EdRecord form in this.AdapterObjects.AllRecordLayouts )
      {
        this.LogDebug ( "ID {0}, T: {1}, S: {2}", form.LayoutId, form.Title, form.State );
        EuAdapterClasses appObject = EuAdapterClasses.Record_Layouts;

        groupCommand = pageGroup.addCommand (
          form.LayoutId,
          EuAdapter.ADAPTER_ID,
          appObject.ToString ( ),
          Evado.UniForm.Model.EuMethods.Get_Object );

        groupCommand.AddParameter ( Evado.UniForm.Model.EuCommandParameters.Short_Title, form.LayoutId );

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
               Evado.UniForm.Model.EuCommandParameters.Image_Url,
               EuRecordLayouts.ICON_FORM_DRAFT );
              break;
            }
          case EdRecordObjectStates.Form_Reviewed:
            {
              groupCommand.AddParameter (
               Evado.UniForm.Model.EuCommandParameters.Image_Url,
               EuRecordLayouts.ICON_FORM_REVIEWED );
              break;
            }
          case EdRecordObjectStates.Form_Issued:
            {
              groupCommand.AddParameter (
               Evado.UniForm.Model.EuCommandParameters.Image_Url,
               EuRecordLayouts.ICON_FORM_ISSUED );
              break;
            }
        }//END state switch.

        groupCommand.Title += form.CommandTitle;

      }//END iteration loop

      this.LogValue ( "Group command count: " + pageGroup.CommandList.Count );

      this.LogMethodEnd ( "createRecordLayoutList_Group" );
    }//END createRecordLayoutList_Group method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class form template upload methods

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.EuCommand object.</param>
    /// <returns>Evado.UniForm.Model.EuAppData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.UniForm.Model.EuAppData getTemplateDownloadPage (
      Evado.UniForm.Model.EuCommand PageCommand )
    {
      this.LogMethod ( "getFormTemplateDownloadPage" );
      this.LogValue ( "UniForm_BinaryFilePath: " + this.UniForm_BinaryFilePath );

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.EuAppData clientDataObject = new Evado.UniForm.Model.EuAppData ( );
      Evado.UniForm.Model.EuGroup pageGroup = new Evado.UniForm.Model.EuGroup ( );
      Evado.UniForm.Model.EuField groupField = new Evado.UniForm.Model.EuField ( );
      Evado.UniForm.Model.EuParameter parameter = new Evado.UniForm.Model.EuParameter ( );
      String xmlText = String.Empty;
      String templateUrl = String.Empty;
      String formTemplateFilename = String.Empty;

      //
      // Determine if the user has access to this page and log and error if they do not.
      //
      if ( this.Session.UserProfile.hasManagementAccess == false )
      {
        this.LogIllegalAccess (
          this.ClassNameSpace + "getFormTemplateDownloadPage",
          this.Session.UserProfile );

        this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

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
      if ( this.Session.RecordLayout == null )
      {
        this.LogValue ( " Form object is null" );
        return null;
      }

      //
      // exist if the form object is null.
      //
      if ( this.Session.RecordLayout.Guid == Guid.Empty
        || this.UniForm_BinaryFilePath == String.Empty
        || this.UniForm_BinaryServiceUrl == String.Empty )
      {
        this.LogValue ( " Form object, UniForm path or URL is empty." );

        return null;
      }

      //
      // Define the form template filename.
      //
      formTemplateFilename = this.Session.RecordLayout.LayoutId
         + "-" + this.Session.RecordLayout.Title
         + "-ver-" + this.Session.RecordLayout.Design.Version
         + EuRecordLayouts.CONST_TEMPLATE_EXTENSION;

      formTemplateFilename = formTemplateFilename.Replace ( " ", "-" );
      formTemplateFilename = formTemplateFilename.ToLower ( );

      this.LogValue ( "formTemplateFilename: " + formTemplateFilename );

      templateUrl = this.UniForm_BinaryServiceUrl +
        formTemplateFilename;

      this.LogValue ( "templateUrl: " + templateUrl );
      //
      // Serialise the form layout.
      //
      xmlText = Evado.Model.EvStatics.SerialiseXmlObject<EdRecord> (
        this.Session.RecordLayout );

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
      clientDataObject.Title = EdLabels.Form_Template_Page_Title;
      clientDataObject.Page.Id = clientDataObject.Id;
      clientDataObject.Page.PageDataGuid = clientDataObject.Id;

      //
      // Define the general properties pageMenuGroup..
      //
      pageGroup = clientDataObject.Page.AddGroup (
        String.Empty,
        Evado.UniForm.Model.EuEditAccess.Inherited );
      pageGroup.Layout = Evado.UniForm.Model.EuGroupLayouts.Full_Width;

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
    /// <param name="PageCommand">Evado.UniForm.Model.EuCommand object.</param>
    /// <returns>Evado.UniForm.Model.EuAppData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.UniForm.Model.EuAppData getTemplateUploadPage (
      Evado.UniForm.Model.EuCommand PageCommand )
    {
      this.LogMethod ( "getFormTemplateUploadPage" );

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.EuAppData clientDataObject = new Evado.UniForm.Model.EuAppData ( );
      this.Session.RecordLayout = new EdRecord ( );
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
      if ( this.Session.UserProfile.hasManagementAccess == false )
      {
        this.LogIllegalAccess (
          this.ClassNameSpace + "getFormTemplateUpload",
          this.Session.UserProfile );

        this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

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
        string value = PageCommand.GetParameter ( EuRecordLayouts.CONST_TEMPLATE_FIELD_ID );

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
        clientDataObject.Title = EdLabels.Form_Template_Page_Title;
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
        this.ErrorMessage = EdLabels.Record_Retrieve_Error_Message;

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
    /// <param name="ClientDataObject">Evado.UniForm.Model.EuAppData object.</param>
    //  ------------------------------------------------------------------------------
    private void getTemplateUpload_Group (
      Evado.UniForm.Model.EuAppData ClientDataObject )
    {
      this.LogMethod ( "getTemplateUpload_Group" );

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.EuGroup pageGroup = new Evado.UniForm.Model.EuGroup ( );
      Guid formGuid = Guid.Empty;
      EdRecord uploadForm = new EdRecord ( );

      //
      // Define the general properties pageMenuGroup..
      //
      pageGroup = ClientDataObject.Page.AddGroup (
        EdLabels.Form_Template_Upload_Log_Group_Title,
        Evado.UniForm.Model.EuEditAccess.Inherited );
      pageGroup.Layout = Evado.UniForm.Model.EuGroupLayouts.Full_Width;

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

      pageGroup.Description = processLog;

      //
      // reset the form template filename.
      //
      this.Session.UploadFileName = String.Empty;


    }//END getPropertiesDataObject Method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="ClientDataObject">Evado.UniForm.Model.EuAppData object.</param>
    //  ------------------------------------------------------------------------------
    private String saveUploadForm (
      EdRecord UploadedForm )
    {
      this.LogMethod ( "saveUploadeForm" );
      this.LogValue ( "Uploaded form is: " + UploadedForm.LayoutId );
      //
      // initialise the methods variables and objects.
      //
      Guid formGuid = Guid.Empty;
      StringBuilder processLog = new StringBuilder ( );
      Evado.Model.EvEventCodes result = EvEventCodes.Ok;
      float version = 0.0F;

      processLog.AppendLine ( "Saving form: " + UploadedForm.LayoutId + " " + UploadedForm.Title );
      //
      // Get the list of forms to determine if there is an existing draft form.
      //
      if ( this.AdapterObjects.AllRecordLayouts.Count == 0 )
      {
        this.loadRecordLayoutList ( );
      }

      //
      // check if there is a draft form and delete it.
      //
      foreach ( EdRecord layout in this.AdapterObjects.AllRecordLayouts )
      {
        //
        // get the list issued version of the form.
        //
        if ( layout.LayoutId == UploadedForm.LayoutId
          && layout.State == EdRecordObjectStates.Form_Issued )
        {
          version = layout.Design.Version;
        }

        //
        // delete any existing draft forms with form ID
        //
        if ( layout.LayoutId == UploadedForm.LayoutId
          && layout.State == EdRecordObjectStates.Form_Draft )
        {
          processLog.AppendLine ( "Existing draft version of " + UploadedForm.LayoutId + " " + UploadedForm.Title + " found." );

          layout.SaveAction = EdRecord.SaveActionCodes.Layout_Deleted;

          result = this._Bll_RecordLayouts.SaveItem ( layout );

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

      UploadedForm.State = EdRecordObjectStates.Form_Draft;
      UploadedForm.SaveAction = EdRecord.SaveActionCodes.Save;
      UploadedForm.Design.Version = version + 0.01F;
      UploadedForm.Guid = Guid.Empty;

      //
      // Save the form
      //
      result = this._Bll_RecordLayouts.SaveItem ( UploadedForm );

      this.LogText ( this._Bll_RecordLayouts.Log );

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

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="ClientDataObject">Evado.UniForm.Model.EuAppData object.</param>
    //  ------------------------------------------------------------------------------
    private void getTemplateUploadDataObject (
      Evado.UniForm.Model.EuAppData ClientDataObject )
    {
      this.LogMethod ( "getTemplateUploadDataObject" );


      //
      // set the page edit access.
      //
      ClientDataObject.Page.EditAccess = Evado.UniForm.Model.EuEditAccess.Enabled;
      if ( this.Session.UserProfile.hasManagementAccess == true )
      {
        ClientDataObject.Page.EditAccess = Evado.UniForm.Model.EuEditAccess.Enabled;
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
    /// <param name="Page">Evado.UniForm.Model.EuPage object.</param>
    //  ------------------------------------------------------------------------------
    private void getUpload_FileSelectionGroup (
      Evado.UniForm.Model.EuPage Page )
    {
      this.LogMethod ( "getProperties_GeneralPageGroup" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.EuGroup pageGroup = new Evado.UniForm.Model.EuGroup ( );
      Evado.UniForm.Model.EuCommand groupCommand = new Evado.UniForm.Model.EuCommand ( );
      Evado.UniForm.Model.EuField groupField = new Evado.UniForm.Model.EuField ( );
      Evado.UniForm.Model.EuParameter parameter = new Evado.UniForm.Model.EuParameter ( );

      //
      // Define the general properties pageMenuGroup..
      //
      pageGroup = Page.AddGroup (
        String.Empty,
        Evado.UniForm.Model.EuEditAccess.Inherited );
      pageGroup.Layout = Evado.UniForm.Model.EuGroupLayouts.Full_Width;

      pageGroup.SetCommandBackBroundColor (
        Evado.UniForm.Model.EuGroupParameters.BG_Mandatory,
        Evado.UniForm.Model.EuBackgroundColours.Red );

      groupField = pageGroup.createBinaryFileField (
        EuRecordLayouts.CONST_TEMPLATE_FIELD_ID,
        EdLabels.Form_Template_File_Selection_Field_Title,
        String.Empty,
        this.Session.UploadFileName );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      groupField.AddParameter ( Evado.UniForm.Model.EuFieldParameters.Snd_Cmd_On_Change, "Yes" );

      groupCommand = pageGroup.addCommand (
        EdLabels.Form_Template_Upload_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Record_Layouts.ToString ( ),
        Evado.UniForm.Model.EuMethods.Custom_Method );

      groupCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Form_Template_Upload );
      groupCommand.setCustomMethod ( Evado.UniForm.Model.EuMethods.Get_Object );


    }//END getUpload_FileSelectionGroup Method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class form layout methods

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.EuCommand object.</param>
    /// <param name="Refesh">Boolean: true = refresh object.</param>
    /// <returns>Evado.UniForm.Model.EuAppData object</returns>
    //  ------------------------------------------------------------------------------
    private bool getRecordObject (
      Evado.UniForm.Model.EuCommand PageCommand,
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

      if ( this.Session.RecordLayout.Guid == formGuid
        && Refesh == false )
      {
        this.LogValue ( "form in memory" );

        //
        // If empty refresh the fields as a new field may have been added.
        //
        if ( this.Session.RecordLayout.Fields.Count == 0 )
        {
          EdRecordFields layoutFields = new EdRecordFields ( this.ClassParameters );

          this.Session.RecordLayout.Fields = layoutFields.GetView ( this.Session.RecordLayout.Guid );

          this.LogValue ( "Updated: Form.Fields.Count: " + this.Session.RecordLayout.Fields.Count );
        }

        return true;
      }

      // 
      // Retrieve the record object from the database via the DAL and BLL layers.
      // 
      this.Session.RecordLayout = this._Bll_RecordLayouts.GetLayout ( formGuid );

      this.LogClass ( this._Bll_RecordLayouts.Log );

      this.LogDebug ( "SessionObjects.Form.LayoutId: " + this.Session.RecordLayout.LayoutId );
      this.LogDebug ( "Updated: Form.Fields.Count: " + this.Session.RecordLayout.Fields.Count );
      this.LogDebug ( "Form.ParentEntities: " + this.Session.RecordLayout.Design.ParentEntities );

      return true;
    }//END getFormObject method

    // ==============================================================================
    /// <summary>
    /// This method creates the pages save commands objects.
    /// </summary>
    /// <returns>Evado.UniForm.Model.EuPage object</returns>
    //  ------------------------------------------------------------------------------
    private void GetDataObject_PageCommands (
      Evado.UniForm.Model.EuPage PageObject )
    {
      this.LogMethod ( "GetDataObject_PageCommands" );
      this.LogDebug ( "Layout Design State {0}. ", this.Session.RecordLayout.State );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.EuParameter parameter = new Evado.UniForm.Model.EuParameter ( );
      Evado.UniForm.Model.EuCommand pageCommand = new Evado.UniForm.Model.EuCommand ( );

      //
      // Add the save groupCommand for the page.
      //
      switch ( this.Session.RecordLayout.State )
      {
        case EdRecordObjectStates.Form_Draft:
          {
            //
            // Add the same groupCommand.
            //
            pageCommand = PageObject.addCommand (
              EdLabels.Form_Save_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Record_Layouts.ToString ( ),
              Evado.UniForm.Model.EuMethods.Save_Object );

            // 
            // Define the save groupCommand parameters.
            // 
            pageCommand.SetGuid ( this.Session.RecordLayout.Guid );

            pageCommand.AddParameter (
              Evado.Digital.Model.EvcStatics.CONST_SAVE_ACTION,
             EdRecord.SaveActionCodes.Layout_Saved.ToString ( ) );

            //
            // For a new form do not display the delete or review commands.
            //
            if ( this.Session.RecordLayout.Guid != Guid.Empty )
            {
              //
              // Add the same groupCommand.
              //
              pageCommand = PageObject.addCommand (
                EdLabels.Form_Delete_Command_Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Record_Layouts.ToString ( ),
                Evado.UniForm.Model.EuMethods.Save_Object );

              // 
              // Define the save groupCommand parameters.
              // 
              pageCommand.SetGuid ( this.Session.RecordLayout.Guid );

              pageCommand.AddParameter (
                Evado.Digital.Model.EvcStatics.CONST_SAVE_ACTION,
               EdRecord.SaveActionCodes.Layout_Deleted.ToString ( ) );

              if ( this.Session.RecordLayout.Fields.Count > 0 )
              {
                pageCommand.AddParameter (
                  Evado.Digital.Model.EvcStatics.CONST_SAVE_ACTION,
                 EdRecord.SaveActionCodes.Layout_Deleted.ToString ( ) );
                //
                // Add the same groupCommand.
                //
                pageCommand = PageObject.addCommand (
                  EdLabels.Form_Review_Command_Title,
                  EuAdapter.ADAPTER_ID,
                  EuAdapterClasses.Record_Layouts.ToString ( ),
                  Evado.UniForm.Model.EuMethods.Save_Object );

                // 
                // Define the save groupCommand parameters.
                // 
                pageCommand.SetGuid ( this.Session.RecordLayout.Guid );

                pageCommand.AddParameter (
                  Evado.Digital.Model.EvcStatics.CONST_SAVE_ACTION,
                 EdRecord.SaveActionCodes.Layout_Reviewed.ToString ( ) );

              }//END Form field exist.
            }//END form exists.

            break;
          }
        case EdRecordObjectStates.Form_Reviewed:
          {
            //
            // Add the same groupCommand.
            //
            pageCommand = PageObject.addCommand (
              EdLabels.Form_Save_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Record_Layouts.ToString ( ),
              Evado.UniForm.Model.EuMethods.Save_Object );

            // 
            // Define the save groupCommand parameters.
            // 
            pageCommand.SetGuid ( this.Session.RecordLayout.Guid );

            pageCommand.AddParameter (
              Evado.Digital.Model.EvcStatics.CONST_SAVE_ACTION,
             EdRecord.SaveActionCodes.Layout_Saved.ToString ( ) );

            //
            // Add the same groupCommand.
            //
            pageCommand = PageObject.addCommand (
              EdLabels.Form_Approved_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Record_Layouts.ToString ( ),
              Evado.UniForm.Model.EuMethods.Save_Object );

            // 
            // Define the save groupCommand parameters.
            // 
            pageCommand.SetGuid ( this.Session.RecordLayout.Guid );

            pageCommand.AddParameter (
              Evado.Digital.Model.EvcStatics.CONST_SAVE_ACTION,
             EdRecord.SaveActionCodes.Layout_Approved.ToString ( ) );

            break;
          }
        case EdRecordObjectStates.Form_Issued:
          {
            //
            // Add the same groupCommand.
            //
            pageCommand = PageObject.addCommand (
              EdLabels.Form_Save_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Record_Layouts.ToString ( ),
              Evado.UniForm.Model.EuMethods.Save_Object );

            // 
            // Define the save groupCommand parameters.
            // 
            pageCommand.SetGuid ( this.Session.RecordLayout.Guid );

            pageCommand.AddParameter (
              Evado.Digital.Model.EvcStatics.CONST_SAVE_ACTION,
             EdRecord.SaveActionCodes.Layout_Update.ToString ( ) );

            //
            // Add the form template save command.
            //
            pageCommand = PageObject.addCommand (
              EdLabels.Form_Template_Download_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Record_Layouts.ToString ( ),
              Evado.UniForm.Model.EuMethods.Get_Object );

            // 
            // Define the groupCommand parameters.
            // 
            pageCommand.SetGuid ( this.Session.RecordLayout.Guid );

            pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Form_Template_Download );

            //
            // Add the copy groupCommand.
            //
            pageCommand = PageObject.addCommand (
              EdLabels.Form_Copy_Form_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Record_Layouts.ToString ( ),
              Evado.UniForm.Model.EuMethods.List_of_Objects );

            // 
            // Define the groupCommand parameters.
            // 
            pageCommand.SetGuid ( this.Session.RecordLayout.Guid );

            pageCommand.AddParameter (
            EuRecordLayouts.CONST_FORM_COMMAND_PARAMETER,
            EuRecordLayouts.CONST_COPY_FORM_COMMAND_VALUE );

            //
            // Add the review groupCommand.
            //
            pageCommand = PageObject.addCommand (
              EdLabels.Form_Revise_Form_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Record_Layouts.ToString ( ),
              Evado.UniForm.Model.EuMethods.List_of_Objects );

            // 
            // Define the groupCommand parameters.
            // 
            pageCommand.SetGuid ( this.Session.RecordLayout.Guid );

            pageCommand.AddParameter (
            EuRecordLayouts.CONST_FORM_COMMAND_PARAMETER,
            EuRecordLayouts.CONST_REVISE_FORM_COMMAND_VALUE );

            break;
          }
      }//END Switch statement 

      this.LogMethodEnd ( "GetDataObject_PageCommands" );

    }//END GetLayout_PageCommands method

    // ==============================================================================
    /// <summary>
    /// This method creates the pages save commands objects.
    /// </summary>
    /// <param name="PageObject">Evado.UniForm.Model.EuPage object</param>
    //  ------------------------------------------------------------------------------
    private void GetDataObject_LayoutCommands (
      Evado.UniForm.Model.EuPage PageObject )
    {
      this.LogMethod ( "GetDataObject_LayoutCommands" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.EuParameter parameter = new Evado.UniForm.Model.EuParameter ( );
      Evado.UniForm.Model.EuCommand pageCommand = new Evado.UniForm.Model.EuCommand ( );

      Evado.Digital.Model.EdStaticPageIds pageId = Evado.Digital.Model.EdStaticPageIds.Null;

      if ( EvStatics.tryParseEnumValue<Evado.Digital.Model.EdStaticPageIds> ( this.Session.PageId, out pageId ) == false )
      {
        pageId = Evado.Digital.Model.EdStaticPageIds.Null;
      }

      switch ( pageId )
      {
        case Evado.Digital.Model.EdStaticPageIds.Form_Draft_Page:
          {
            //
            // Add the properties page command
            //
            pageCommand = PageObject.addCommand (
              EdLabels.Form_Properties_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Record_Layouts.ToString ( ),
              Evado.UniForm.Model.EuMethods.Get_Object );

            pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Form_Properties_Page );

            pageCommand.SetGuid ( this.Session.RecordLayout.Guid );

            //
            // The full layout button.
            //
            pageCommand = PageObject.addCommand (
              EdLabels.Form_Full_Layout_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Record_Layouts.ToString ( ),
              Evado.UniForm.Model.EuMethods.Custom_Method );

            pageCommand.setCustomMethod (
              Evado.UniForm.Model.EuMethods.Get_Object );

            pageCommand.SetGuid ( this.Session.RecordLayout.Guid );

            pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Entity_Layout_Page );
            //
            // The annotated layout button.
            //
            pageCommand = PageObject.addCommand (
              EdLabels.Form_Annotated_Layout_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Record_Layouts.ToString ( ),
              Evado.UniForm.Model.EuMethods.Custom_Method );

            pageCommand.setCustomMethod (
              Evado.UniForm.Model.EuMethods.Get_Object );

            pageCommand.SetGuid ( this.Session.RecordLayout.Guid );

            pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Form_Annotated_Page );
            break;
          }
        case Evado.Digital.Model.EdStaticPageIds.Form_Properties_Page:
          {
            //
            // Add the draft page layout.
            //
            pageCommand = PageObject.addCommand (
              EdLabels.Form_Draft_Layout_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Record_Layouts.ToString ( ),
              Evado.UniForm.Model.EuMethods.Custom_Method );

            pageCommand.setCustomMethod (
              Evado.UniForm.Model.EuMethods.Get_Object );

            pageCommand.SetGuid ( this.Session.RecordLayout.Guid );

            pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Form_Draft_Page );

            //
            // The full layout button.
            //
            pageCommand = PageObject.addCommand (
              EdLabels.Form_Full_Layout_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Record_Layouts.ToString ( ),
              Evado.UniForm.Model.EuMethods.Custom_Method );

            pageCommand.setCustomMethod (
              Evado.UniForm.Model.EuMethods.Get_Object );

            pageCommand.SetGuid ( this.Session.RecordLayout.Guid );

            pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Entity_Layout_Page );

            //
            // The annotated layout button.
            //
            pageCommand = PageObject.addCommand (
              EdLabels.Form_Annotated_Layout_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Record_Layouts.ToString ( ),
              Evado.UniForm.Model.EuMethods.Custom_Method );

            pageCommand.setCustomMethod (
              Evado.UniForm.Model.EuMethods.Get_Object );

            pageCommand.SetGuid ( this.Session.RecordLayout.Guid );

            pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Form_Annotated_Page );
            break;
          }
        case Evado.Digital.Model.EdStaticPageIds.Entity_Layout_Page:
          {
            //
            // Add the draft page layout.
            //
            pageCommand = PageObject.addCommand (
              EdLabels.Form_Draft_Layout_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Record_Layouts.ToString ( ),
              Evado.UniForm.Model.EuMethods.Custom_Method );

            pageCommand.setCustomMethod (
              Evado.UniForm.Model.EuMethods.Get_Object );

            pageCommand.SetGuid ( this.Session.RecordLayout.Guid );

            pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Form_Draft_Page );

            //
            // Add the properties page command
            //
            pageCommand = PageObject.addCommand (
              EdLabels.Form_Properties_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Record_Layouts.ToString ( ),
              Evado.UniForm.Model.EuMethods.Get_Object );

            pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Form_Properties_Page );

            pageCommand.SetGuid ( this.Session.RecordLayout.Guid );

            //
            // The annotated layout button.
            //
            pageCommand = PageObject.addCommand (
              EdLabels.Form_Annotated_Layout_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Record_Layouts.ToString ( ),
              Evado.UniForm.Model.EuMethods.Custom_Method );

            pageCommand.setCustomMethod (
              Evado.UniForm.Model.EuMethods.Get_Object );

            pageCommand.SetGuid ( this.Session.RecordLayout.Guid );

            pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Form_Annotated_Page );
            break;
          }
        case Evado.Digital.Model.EdStaticPageIds.Form_Annotated_Page:
          {
            //
            // Add the draft page layout.
            //
            pageCommand = PageObject.addCommand (
              EdLabels.Form_Draft_Layout_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Record_Layouts.ToString ( ),
              Evado.UniForm.Model.EuMethods.Custom_Method );

            pageCommand.setCustomMethod (
              Evado.UniForm.Model.EuMethods.Get_Object );

            pageCommand.SetGuid ( this.Session.RecordLayout.Guid );

            pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Form_Draft_Page );

            //
            // Add the properties page command
            //
            pageCommand = PageObject.addCommand (
              EdLabels.Form_Properties_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Record_Layouts.ToString ( ),
              Evado.UniForm.Model.EuMethods.Get_Object );

            pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Form_Properties_Page );

            pageCommand.SetGuid ( this.Session.RecordLayout.Guid );
            //
            // The full layout button.
            //
            pageCommand = PageObject.addCommand (
              EdLabels.Form_Full_Layout_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Record_Layouts.ToString ( ),
              Evado.UniForm.Model.EuMethods.Custom_Method );

            pageCommand.setCustomMethod (
              Evado.UniForm.Model.EuMethods.Get_Object );

            pageCommand.SetGuid ( this.Session.RecordLayout.Guid );

            pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Entity_Layout_Page );
            break;
          }
      }//End Sticeh statement

      this.LogMethodEnd ( "GetDataObject_LayoutCommands" );

    }//END GetDataObject_LayoutCommands method

    // ==============================================================================
    /// <summary>
    /// This method creates the pages layout commands objects.
    /// </summary>
    /// <param name="PageObject">Evado.UniForm.Model.EuPage object</param>
    //  ------------------------------------------------------------------------------
    private void GetDataObject_GroupCommands (
      Evado.UniForm.Model.EuGroup PageGroup )
    {
      this.LogMethod ( "GetDataObject_GroupCommands" );

      if ( PageGroup.EditAccess != Evado.UniForm.Model.EuEditAccess.Enabled )
      {
        return;
      }

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.EuParameter parameter = new Evado.UniForm.Model.EuParameter ( );
      Evado.UniForm.Model.EuCommand pageCommand = new Evado.UniForm.Model.EuCommand ( );

      if ( PageGroup == null )
      {
        return;
      }

      //
      // Add the property page refresh
      //
      if ( this.Session.PageId == Evado.Digital.Model.EdStaticPageIds.Form_Properties_Page.ToString ( ) )
      {
        pageCommand = PageGroup.addCommand (
          EdLabels.Layout_Design_Refresh,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Record_Layouts.ToString ( ),
          Evado.UniForm.Model.EuMethods.Custom_Method );

        pageCommand.setCustomMethod ( Evado.UniForm.Model.EuMethods.Get_Object );
      }

      //
      // Add the save groupCommand for the page.
      //
      switch ( this.Session.RecordLayout.State )
      {
        case EdRecordObjectStates.Form_Draft:
          {
            //
            // Add the same groupCommand.
            //
            pageCommand = PageGroup.addCommand (
              EdLabels.Form_Save_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Record_Layouts.ToString ( ),
              Evado.UniForm.Model.EuMethods.Save_Object );

            // 
            // Define the save groupCommand parameters.
            // 
            pageCommand.SetGuid ( this.Session.RecordLayout.Guid );

            if ( this.Session.RecordLayout.State == EdRecordObjectStates.Form_Draft )
            {
              pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Form_Draft_Page );
            }
            else
            {
              pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Entity_Layout_Page );
            }
            pageCommand.AddParameter (
              Evado.Digital.Model.EvcStatics.CONST_SAVE_ACTION,
             EdRecord.SaveActionCodes.Layout_Saved.ToString ( ) );

            //
            // For a new form do not display the delete or review commands.
            //
            if ( this.Session.RecordLayout.Guid != Guid.Empty )
            {
              //
              // Add the same groupCommand.
              //
              pageCommand = PageGroup.addCommand (
                EdLabels.Form_Delete_Command_Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Record_Layouts.ToString ( ),
                Evado.UniForm.Model.EuMethods.Save_Object );

              // 
              // Define the save groupCommand parameters.
              // 
              pageCommand.SetGuid ( this.Session.RecordLayout.Guid );
              pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Entity_Layout_Page );

              pageCommand.AddParameter (
                Evado.Digital.Model.EvcStatics.CONST_SAVE_ACTION,
               EdRecord.SaveActionCodes.Layout_Deleted.ToString ( ) );
              //
              // Add the same groupCommand.
              //
              if ( this.Session.RecordLayout.Fields.Count > 0 )
              {
                pageCommand = PageGroup.addCommand (
                  EdLabels.Form_Review_Command_Title,
                  EuAdapter.ADAPTER_ID,
                  EuAdapterClasses.Record_Layouts.ToString ( ),
                  Evado.UniForm.Model.EuMethods.Save_Object );

                // 
                // Define the save groupCommand parameters.
                // 
                pageCommand.SetGuid ( this.Session.RecordLayout.Guid );
                pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Entity_Layout_Page );

                pageCommand.AddParameter (
                  Evado.Digital.Model.EvcStatics.CONST_SAVE_ACTION,
                 EdRecord.SaveActionCodes.Layout_Reviewed.ToString ( ) );
              }
            }

            break;
          }
        case EdRecordObjectStates.Form_Reviewed:
          {
            //
            // Add the same groupCommand.
            //
            pageCommand = PageGroup.addCommand (
              EdLabels.Form_Save_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Record_Layouts.ToString ( ),
              Evado.UniForm.Model.EuMethods.Save_Object );

            // 
            // Define the save groupCommand parameters.
            // 
            pageCommand.SetGuid ( this.Session.RecordLayout.Guid );
            pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Entity_Layout_Page );

            pageCommand.AddParameter (
              Evado.Digital.Model.EvcStatics.CONST_SAVE_ACTION,
             EdRecord.SaveActionCodes.Layout_Update.ToString ( ) );

            //
            // Add the same groupCommand.
            //
            pageCommand = PageGroup.addCommand (
              EdLabels.Form_Approved_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Record_Layouts.ToString ( ),
              Evado.UniForm.Model.EuMethods.Save_Object );

            // 
            // Define the save groupCommand parameters.
            // 
            pageCommand.SetGuid ( this.Session.RecordLayout.Guid );
            pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Entity_Layout_Page );

            pageCommand.AddParameter (
              Evado.Digital.Model.EvcStatics.CONST_SAVE_ACTION,
             EdRecord.SaveActionCodes.Layout_Approved.ToString ( ) );

            break;
          }
        case EdRecordObjectStates.Form_Issued:
          {
            //
            // Add the same groupCommand.
            //
            pageCommand = PageGroup.addCommand (
              EdLabels.Form_Save_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Record_Layouts.ToString ( ),
              Evado.UniForm.Model.EuMethods.Save_Object );

            // 
            // Define the save groupCommand parameters.
            // 
            pageCommand.SetGuid ( this.Session.RecordLayout.Guid );
            pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Entity_Layout_Page );

            pageCommand.AddParameter (
              Evado.Digital.Model.EvcStatics.CONST_SAVE_ACTION,
             EdRecord.SaveActionCodes.Layout_Update.ToString ( ) );

            //
            // Add the same groupCommand.
            //
            pageCommand = PageGroup.addCommand (
              EdLabels.Form_Withdrawn_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Record_Layouts.ToString ( ),
              Evado.UniForm.Model.EuMethods.Save_Object );

            // 
            // Define the save groupCommand parameters.
            // 
            pageCommand.SetGuid ( this.Session.RecordLayout.Guid );

            pageCommand.AddParameter (
              Evado.Digital.Model.EvcStatics.CONST_SAVE_ACTION,
             EdRecord.SaveActionCodes.Layout_Withdrawn.ToString ( ) );

            //
            // Add the copy groupCommand.
            //
            pageCommand = PageGroup.addCommand (
              EdLabels.Form_Copy_Form_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Record_Layouts.ToString ( ),
              Evado.UniForm.Model.EuMethods.List_of_Objects );

            // 
            // Define the groupCommand parameters.
            // 
            pageCommand.SetGuid ( this.Session.RecordLayout.Guid );

            pageCommand.AddParameter (
            EuRecordLayouts.CONST_FORM_COMMAND_PARAMETER,
            EuRecordLayouts.CONST_COPY_FORM_COMMAND_VALUE );

            //
            // Add the review groupCommand.
            //
            pageCommand = PageGroup.addCommand (
              EdLabels.Form_Revise_Form_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Record_Layouts.ToString ( ),
              Evado.UniForm.Model.EuMethods.List_of_Objects );

            // 
            // Define the groupCommand parameters.
            // 
            pageCommand.SetGuid ( this.Session.RecordLayout.Guid );

            pageCommand.AddParameter (
            EuRecordLayouts.CONST_FORM_COMMAND_PARAMETER,
            EuRecordLayouts.CONST_REVISE_FORM_COMMAND_VALUE );

            break;
          }
        default:
          {
            break;
          }
      }//End Switch 

      this.LogMethodEnd ( "GetDataObject_GroupCommands" );

    }//END GetDataObject_GroupCommands method

    // ==============================================================================
    /// <summary>
    /// This method creates the group layout commands objects.
    /// </summary>
    /// <param name="PageGroup">Evado.UniForm.Model.EuPage object</param>
    //  ------------------------------------------------------------------------------
    private void GetDataObject_LayoutCommands (
      Evado.UniForm.Model.EuGroup PageGroup )
    {
      this.LogMethod ( "GetDataObject_LayoutCommands" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.EuParameter parameter = new Evado.UniForm.Model.EuParameter ( );
      Evado.UniForm.Model.EuCommand pageCommand = new Evado.UniForm.Model.EuCommand ( );

      Evado.Digital.Model.EdStaticPageIds pageId = Evado.Digital.Model.EdStaticPageIds.Null;

      if ( EvStatics.tryParseEnumValue<Evado.Digital.Model.EdStaticPageIds> ( this.Session.PageId, out pageId ) == false )
      {
        pageId = Evado.Digital.Model.EdStaticPageIds.Null;
      }

      switch ( pageId )
      {
        case Evado.Digital.Model.EdStaticPageIds.Form_Draft_Page:
          {
            //
            // Add the properties page command
            //
            pageCommand = PageGroup.addCommand (
              EdLabels.Form_Properties_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Record_Layouts.ToString ( ),
              Evado.UniForm.Model.EuMethods.Get_Object );

            pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Form_Properties_Page );

            pageCommand.SetGuid ( this.Session.RecordLayout.Guid );

            //
            // The full layout button.
            //
            pageCommand = PageGroup.addCommand (
              EdLabels.Form_Full_Layout_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Record_Layouts.ToString ( ),
              Evado.UniForm.Model.EuMethods.Custom_Method );

            pageCommand.setCustomMethod (
              Evado.UniForm.Model.EuMethods.Get_Object );

            pageCommand.SetGuid ( this.Session.RecordLayout.Guid );

            pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Entity_Layout_Page );
            //
            // The annotated layout button.
            //
            pageCommand = PageGroup.addCommand (
              EdLabels.Form_Annotated_Layout_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Record_Layouts.ToString ( ),
              Evado.UniForm.Model.EuMethods.Custom_Method );

            pageCommand.setCustomMethod (
              Evado.UniForm.Model.EuMethods.Get_Object );

            pageCommand.SetGuid ( this.Session.RecordLayout.Guid );

            pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Form_Annotated_Page );
            break;
          }
        case Evado.Digital.Model.EdStaticPageIds.Form_Properties_Page:
          {
            //
            // Add the draft page layout.
            //
            pageCommand = PageGroup.addCommand (
              EdLabels.Form_Draft_Layout_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Record_Layouts.ToString ( ),
              Evado.UniForm.Model.EuMethods.Custom_Method );

            pageCommand.setCustomMethod (
              Evado.UniForm.Model.EuMethods.Get_Object );

            pageCommand.SetGuid ( this.Session.RecordLayout.Guid );

            pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Form_Draft_Page );

            //
            // The full layout button.
            //
            pageCommand = PageGroup.addCommand (
              EdLabels.Form_Full_Layout_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Record_Layouts.ToString ( ),
              Evado.UniForm.Model.EuMethods.Custom_Method );

            pageCommand.setCustomMethod (
              Evado.UniForm.Model.EuMethods.Get_Object );

            pageCommand.SetGuid ( this.Session.RecordLayout.Guid );

            pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Entity_Layout_Page );

            //
            // The annotated layout button.
            //
            pageCommand = PageGroup.addCommand (
              EdLabels.Form_Annotated_Layout_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Record_Layouts.ToString ( ),
              Evado.UniForm.Model.EuMethods.Custom_Method );

            pageCommand.setCustomMethod (
              Evado.UniForm.Model.EuMethods.Get_Object );

            pageCommand.SetGuid ( this.Session.RecordLayout.Guid );

            pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Form_Annotated_Page );
            break;
          }
        case Evado.Digital.Model.EdStaticPageIds.Entity_Layout_Page:
          {
            //
            // Add the draft page layout.
            //
            pageCommand = PageGroup.addCommand (
              EdLabels.Form_Draft_Layout_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Record_Layouts.ToString ( ),
              Evado.UniForm.Model.EuMethods.Custom_Method );

            pageCommand.setCustomMethod (
              Evado.UniForm.Model.EuMethods.Get_Object );

            pageCommand.SetGuid ( this.Session.RecordLayout.Guid );

            pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Form_Draft_Page );

            //
            // Add the properties page command
            //
            pageCommand = PageGroup.addCommand (
              EdLabels.Form_Properties_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Record_Layouts.ToString ( ),
              Evado.UniForm.Model.EuMethods.Get_Object );

            pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Form_Properties_Page );

            pageCommand.SetGuid ( this.Session.RecordLayout.Guid );

            //
            // The annotated layout button.
            //
            pageCommand = PageGroup.addCommand (
              EdLabels.Form_Annotated_Layout_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Record_Layouts.ToString ( ),
              Evado.UniForm.Model.EuMethods.Custom_Method );

            pageCommand.setCustomMethod (
              Evado.UniForm.Model.EuMethods.Get_Object );

            pageCommand.SetGuid ( this.Session.RecordLayout.Guid );

            pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Form_Annotated_Page );
            break;
          }
        case Evado.Digital.Model.EdStaticPageIds.Form_Annotated_Page:
          {
            //
            // Add the draft page layout.
            //
            pageCommand = PageGroup.addCommand (
              EdLabels.Form_Draft_Layout_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Record_Layouts.ToString ( ),
              Evado.UniForm.Model.EuMethods.Custom_Method );

            pageCommand.setCustomMethod (
              Evado.UniForm.Model.EuMethods.Get_Object );

            pageCommand.SetGuid ( this.Session.RecordLayout.Guid );

            pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Form_Draft_Page );

            //
            // Add the properties page command
            //
            pageCommand = PageGroup.addCommand (
              EdLabels.Form_Properties_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Record_Layouts.ToString ( ),
              Evado.UniForm.Model.EuMethods.Get_Object );

            pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Form_Properties_Page );

            pageCommand.SetGuid ( this.Session.RecordLayout.Guid );
            //
            // The full layout button.
            //
            pageCommand = PageGroup.addCommand (
              EdLabels.Form_Full_Layout_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Record_Layouts.ToString ( ),
              Evado.UniForm.Model.EuMethods.Custom_Method );

            pageCommand.setCustomMethod (
              Evado.UniForm.Model.EuMethods.Get_Object );

            pageCommand.SetGuid ( this.Session.RecordLayout.Guid );

            pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Entity_Layout_Page );
            break;
          }
      }//End Sticeh statement

      this.LogMethodEnd ( "GetDataObject_LayoutCommands" );

    }//END GetDataObject_LayoutCommands method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.EuCommand object.</param>
    /// <returns>Evado.UniForm.Model.EuAppData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.UniForm.Model.EuAppData getEntityLayoutObject (
      Evado.UniForm.Model.EuCommand PageCommand )
    {
      this.LogMethod ( "getEntityLayoutObject" );

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
          this.ClassNameSpace + "getEntityLayoutObject",
          this.Session.UserProfile );

        this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

        return null;
      }

      // 
      // Log access to page.
      // 
      this.LogPageAccess (
        this.ClassNameSpace + "getEntityLayoutObject",
        this.Session.UserProfile );


      try
      {
        if ( this.getRecordObject ( PageCommand, false ) == false )
        {
          return null;
        }

        //
        // if there are no field display the draft layout page.
        //
        if ( this.Session.RecordLayout.Fields.Count == 0 )
        {
          this.getDraftDataObject ( clientDataObject );

          return clientDataObject;
        }

        //
        // if there are is not page type defines and draft state display the draft page.
        //
        if ( this.Session.RecordLayout.State == EdRecordObjectStates.Form_Draft
          && this.Session.PageId == String.Empty )
        {
          this.Session.PageId = Evado.Digital.Model.EdStaticPageIds.Form_Draft_Page.ToString ( );
          this.getDraftDataObject ( clientDataObject );

          return clientDataObject;
        }

        if ( this.Session.PageId != Evado.Digital.Model.EdStaticPageIds.Entity_Layout_Page.ToString ( )
          && this.Session.PageId != Evado.Digital.Model.EdStaticPageIds.Form_Properties_Page.ToString ( )
          && this.Session.PageId != Evado.Digital.Model.EdStaticPageIds.Form_Draft_Page.ToString ( )
          && this.Session.PageId != Evado.Digital.Model.EdStaticPageIds.Form_Annotated_Page.ToString ( ) )
        {
          this.Session.PageId = Evado.Digital.Model.EdStaticPageIds.Entity_Layout_Page.ToString ( );
        }

        this.LogDebug ( "EntityLayout.DefaultPageLayout: " + this.Session.RecordLayout.Design.DefaultPageLayout );
        //
        // Rung the server script if server side scripts are enabled.
        //
        this.runServerScript ( EvServerPageScript.ScripEventTypes.OnOpen );

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
        this.ErrorMessage = EdLabels.Record_Retrieve_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return null;

    }//END getEntityLayoutObject method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.EuCommand object.</param>
    /// <returns>Evado.UniForm.Model.EuAppData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.UniForm.Model.EuAppData getDraftLayoutObject (
      Evado.UniForm.Model.EuCommand PageCommand )
    {
      this.LogMethod ( "getDraftLayoutObject" );

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
          this.ClassNameSpace + "getDraftLayoutObject",
          this.Session.UserProfile );

        this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

        return this.Session.LastPage;
      }

      // 
      // Log access to page.
      // 
      this.LogPageAccess (
        this.ClassNameSpace + "getDraftLayoutObject",
        this.Session.UserProfile );


      try
      {
        if ( this.getRecordObject ( PageCommand, true ) == false )
        {
          this.LogMethodEnd ( "getDraftLayoutObject" );
          return this.Session.LastPage;
        }

        //
        // generate the page layout.
        //
        this.getDraftDataObject ( clientDataObject );

        // 
        // Return the client ResultData object to the calling method.
        // 
        this.LogMethodEnd ( "getDraftLayoutObject" );
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

      this.LogMethodEnd ( "getDraftLayoutObject" );
      return this.Session.LastPage;

    }//END getLayoutObject method

    // ==============================================================================
    /// <summary>
    /// This method gets the form ResultData object page.
    /// </summary>
    /// <param name="ClientDataObject">Evado.UniForm.Model.EuAppData object.</param>
    //  ------------------------------------------------------------------------------
    private void getFormDataObject (
      Evado.UniForm.Model.EuAppData ClientDataObject )
    {
      this.LogMethod ( "getFormDataObject" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.EuGroup pageGroup = new Evado.UniForm.Model.EuGroup ( );
      Evado.UniForm.Model.EuCommand pageCommand = new Evado.UniForm.Model.EuCommand ( );
      Evado.UniForm.Model.EuField pageField = new Evado.UniForm.Model.EuField ( );
      Evado.UniForm.Model.EuParameter parameter = new Evado.UniForm.Model.EuParameter ( );
      EuRecordGenerator pageGenerator = new EuRecordGenerator (
        this.AdapterObjects,
        this.ServiceUserProfile,
        this.Session,
        this.UniForm_BinaryFilePath,
        this.UniForm_BinaryServiceUrl,
        this.ClassParameters );

      // 
      // Initialise the client ResultData object.
      // 
      ClientDataObject.Id = this.Session.RecordLayout.Guid;
      ClientDataObject.Title =
        String.Format ( EdLabels.Form_Page_Title,
          this.Session.RecordLayout.LayoutId,
          this.Session.RecordLayout.Title );

      ClientDataObject.Page.Id = ClientDataObject.Id;
      ClientDataObject.Page.PageDataGuid = ClientDataObject.Id;
      ClientDataObject.Page.PageId = this.Session.RecordLayout.LayoutId;
      ClientDataObject.Page.EditAccess = Evado.UniForm.Model.EuEditAccess.Enabled;

      //
      // Set the user's edit access if they have configuration edit access.
      //
      this.LogValue ( "HasConfigrationEditAccess: " + this.Session.UserProfile.hasManagementAccess );

      if ( this.Session.UserProfile.hasManagementAccess == true )
      {
        ClientDataObject.Page.EditAccess = Evado.UniForm.Model.EuEditAccess.Enabled;
      }

      //
      // Add the save commandS for the page.
      //
      this.GetDataObject_PageCommands ( ClientDataObject.Page );

      //
      // Add the page layout commands
      //
      this.GetDataObject_LayoutCommands ( ClientDataObject.Page );

      this.LogValue ( "GENERATE FORM" );

      // 
      // Call the page generation method
      // 
      pageGenerator.generateLayout (
        this.Session.RecordLayout,
        ClientDataObject.Page,
        this.UniForm_BinaryFilePath );

      this.LogValue ( pageGenerator.Log );

    }//END getFormDataObject Method

    // ==============================================================================
    /// <summary>
    /// This method creates the draft form client ResultData object.
    /// </summary>
    /// <param name="ClientDataObject">Evado.UniForm.Model.EuAppData object.</param>
    //  ------------------------------------------------------------------------------
    private void getDraftDataObject (
      Evado.UniForm.Model.EuAppData ClientDataObject )
    {
      this.LogMethod ( "getDraftDataObject" );
      this.LogDebug ( " Section count {0}.", this.Session.RecordLayout.Design.FormSections.Count );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.EuCommand pageCommand = new Evado.UniForm.Model.EuCommand ( );

      // 
      // Initialise the client ResultData object.
      // 
      ClientDataObject.Id = this.Session.RecordLayout.Guid;
      ClientDataObject.Title =
        String.Format ( EdLabels.Form_Page_Title,
          this.Session.RecordLayout.LayoutId,
          this.Session.RecordLayout.Title );

      ClientDataObject.Page.Id = ClientDataObject.Id;
      ClientDataObject.Page.PageDataGuid = ClientDataObject.Id;
      ClientDataObject.Page.PageId = this.Session.RecordLayout.LayoutId;
      ClientDataObject.Page.EditAccess = Evado.UniForm.Model.EuEditAccess.Disabled;

      //
      // Set the user's edit access if they have configuration edit access.
      //
      this.LogValue ( "HasConfigrationEditAccess: " + this.Session.UserProfile.hasManagementAccess );

      this.LogValue ( "GENERATE FORM" );

      //
      // Add the save commandS for the page.
      //
      this.GetDataObject_PageCommands ( ClientDataObject.Page );

      //
      // Add the page layout commands
      //
      this.GetDataObject_LayoutCommands ( ClientDataObject.Page );

      //
      // Add the form header pageMenuGroup.
      //
      this.createDraftHeaderFields ( ClientDataObject.Page );


      if ( this.Session.UserProfile.hasManagementAccess == true )
      {
        ClientDataObject.Page.EditAccess = Evado.UniForm.Model.EuEditAccess.Enabled;
      }

      //
      // Add the form sections groups.
      //
      if ( this.Session.RecordLayout.Design.FormSections.Count > 0 )
      {
        foreach ( EdRecordSection formSection in this.Session.RecordLayout.Design.FormSections )
        {
          this.createDraftSectionFields ( formSection, ClientDataObject.Page );

        }//END section iteration loop

      }//END form section exist.

      //
      // add a pageMenuGroup with fields that are not allocated to a section.
      //
      this.createDraftNonSectionFields ( ClientDataObject.Page );

      this.LogMethodEnd ( "getDraftDataObject" );
    }//END getFormDataObject Method

    // ==================================================================================    
    /// <summary>
    /// This method creates the draft form section update page pageMenuGroup.
    /// </summary>
    /// <param name="PageObject">Evado.UniForm.Model.EuPage object.</param>
    //  ----------------------------------------------------------------------------------
    private void createDraftHeaderFields (
      Evado.UniForm.Model.EuPage PageObject )
    {
      this.LogMethod ( "createDraftHeaderFields" );

      this.LogValue ( "Title : " + PageObject.Title );

      // 
      // Initialise the methods variables and objects.
      //
      Evado.UniForm.Model.EuField pageField = new Evado.UniForm.Model.EuField ( );
      Evado.UniForm.Model.EuGroup pageGroup = PageObject.AddGroup (
        EdLabels.Form_Header_Group_Title,
        Evado.UniForm.Model.EuEditAccess.Enabled );
      pageGroup.Layout = Evado.UniForm.Model.EuGroupLayouts.Full_Width;

      //
      // Add the save commandS for the page.
      //
      this.GetDataObject_GroupCommands ( pageGroup );

      //
      // Define the page layout selection
      //
      this.GetDataObject_LayoutCommands ( pageGroup );

      //
      // Form title
      //
      pageField = pageGroup.createReadOnlyTextField (
        String.Empty,
        EdLabels.Label_Form_Id,
        this.Session.RecordLayout.LayoutId );
      pageField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Form title
      //
      pageField = pageGroup.createReadOnlyTextField (
        String.Empty,
        EdLabels.Form_Title_Field_Label,
        this.Session.RecordLayout.Design.Title );
      pageField.Layout = EuAdapter.DefaultFieldLayout;


      //
      // Form Instructions
      //
      if ( this.Session.RecordLayout.Design.Instructions != String.Empty )
      {
        pageField = pageGroup.createReadOnlyTextField (
          String.Empty,
          EdLabels.Form_Instructions_Field_Title,
          this.Session.RecordLayout.Design.Instructions );
        pageField.Layout = EuAdapter.DefaultFieldLayout;
      }

      //
      // Form reference
      //
      if ( this.Session.RecordLayout.Design.HttpReference != String.Empty )
      {
        pageField = pageGroup.createReadOnlyTextField (
          String.Empty,
          EdLabels.Form_Reference_Field_Label,
          this.Session.RecordLayout.Design.HttpReference );
        pageField.Layout = EuAdapter.DefaultFieldLayout;
      }

      //
      // Form category
      //
      if ( this.Session.RecordLayout.Design.RecordCategory != String.Empty )
      {
        pageField = pageGroup.createReadOnlyTextField (
          String.Empty,
          EdLabels.Form_Category_Field_Title,
          this.Session.RecordLayout.Design.RecordCategory );
        pageField.Layout = EuAdapter.DefaultFieldLayout;
      }

      //
      // Form status
      //
      pageField = pageGroup.createReadOnlyTextField (
        String.Empty,
        EdLabels.Form_Status_Field_Title,
        this.Session.RecordLayout.StateDesc );
      pageField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Form Version
      //
      pageField = pageGroup.createReadOnlyTextField (
        String.Empty,
        EdLabels.Form_Version_Field_Title,
        this.Session.RecordLayout.Design.stVersion );
      pageField.Layout = EuAdapter.DefaultFieldLayout;

      return;

    }//END createDraftHeaderFields method

    // ==================================================================================    
    /// <summary>
    /// This method creates the draft form section update page pageMenuGroup.
    /// </summary>
    /// <param name="PageObject">Evado.UniForm.Model.EuPage object.</param>
    /// <param name="FormSection">EvFormSection object.</param>
    //  ----------------------------------------------------------------------------------
    private void createDraftSectionFields (
      EdRecordSection FormSection,
      Evado.UniForm.Model.EuPage PageObject )
    {
      this.LogMethod ( "getDraftSectionFields" );

      // 
      // Initialise the methods variables and objects.
      //
      Evado.UniForm.Model.EuGroup pageGroup = new Evado.UniForm.Model.EuGroup ( );
      Evado.UniForm.Model.EuCommand groupCommand = new Evado.UniForm.Model.EuCommand ( );
      bool addNewField = false;
      if ( this.Session.RecordLayout.State != EdRecordObjectStates.Form_Issued )
      {
        addNewField = true;
      }

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
        Evado.UniForm.Model.EuEditAccess.Inherited );
      pageGroup.Layout = Evado.UniForm.Model.EuGroupLayouts.Full_Width;
      pageGroup.CmdLayout = Evado.UniForm.Model.EuGroupCommandListLayouts.Vertical_Orientation;

      //
      // Add new field command.
      //
      if ( addNewField == true )
      {
        groupCommand = pageGroup.addCommand (
          EdLabels.Form_New_Field_Page_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Record_Layout_Fields.ToString ( ),
          Evado.UniForm.Model.EuMethods.Get_Object );

        groupCommand.AddParameter ( Evado.UniForm.Model.EuCommandParameters.Create_Object, "1" );

        groupCommand.AddParameter ( Evado.UniForm.Model.EuCommandParameters.Page_Id,
          Evado.Digital.Model.EdStaticPageIds.Form_Field_Page.ToString ( ) );

        groupCommand.AddParameter ( EuRecordFields.CONST_FIELD_COUNT,
          this.Session.RecordLayout.Fields.Count.ToString ( ) );

        groupCommand.AddParameter ( EuRecordFields.CONST_FORM_SECTION,
          FormSection.No.ToString ( ) );

        groupCommand.SetBackgroundColour (
          Evado.UniForm.Model.EuCommandParameters.BG_Default,
          Evado.UniForm.Model.EuBackgroundColours.Purple );
      }

      //
      // Iterate through the fields in the form, selecting those fields that are
      // associated with the current section.
      //
      foreach ( EdRecordField field in this.Session.RecordLayout.Fields )
      {
        //
        // if the field is not in the section skip to the next field.
        //
        if ( field.Design.SectionNo != FormSection.No )
        {
          continue;
        }

        //
        // Add the fields groupCommand object.
        // using the Guid for the unique field identifier.
        //
        groupCommand = pageGroup.addCommand (
          field.LinkText,
          EuAdapter.ADAPTER_ID,
         EuAdapterClasses.Record_Layout_Fields.ToString ( ),
          Evado.UniForm.Model.EuMethods.Get_Object );

        groupCommand.SetGuid ( field.Guid );

      }//END section field iteration loop.

    }//END getDraftSectionFields method

    // ==================================================================================    
    /// <summary>
    /// This method creates DraftRecords non section page pageMenuGroup objects.
    /// </summary>
    /// <param name="PageObject">Evado.UniForm.Model.EuPage object.</param>
    //  ----------------------------------------------------------------------------------
    private void createDraftNonSectionFields (
      Evado.UniForm.Model.EuPage PageObject )
    {
      this.LogMethod ( "getDraftNonSectionFields" );

      // 
      // Initialise the methods variables and objects.
      //
      Evado.UniForm.Model.EuGroup pageGroup = new Evado.UniForm.Model.EuGroup ( );
      Evado.UniForm.Model.EuCommand groupCommand = new Evado.UniForm.Model.EuCommand ( );
      bool addNewField = false;
      if ( this.Session.RecordLayout.State != EdRecordObjectStates.Form_Issued )
      {
        addNewField = true;
      }

      //
      // define the page pageMenuGroup for the section.
      //
      pageGroup = PageObject.AddGroup (
        EdLabels.Form_No_Section_Field_Group_Title,
        String.Empty,
        Evado.UniForm.Model.EuEditAccess.Inherited );
      pageGroup.Layout = Evado.UniForm.Model.EuGroupLayouts.Full_Width;
      pageGroup.CmdLayout = Evado.UniForm.Model.EuGroupCommandListLayouts.Vertical_Orientation;

      //
      // Add new field command.
      //
      if ( addNewField == true )
      {
        groupCommand = pageGroup.addCommand (
          EdLabels.Form_New_Field_Page_Command_Title,
          EuAdapter.ADAPTER_ID,
         EuAdapterClasses.Record_Layout_Fields.ToString ( ),
          Evado.UniForm.Model.EuMethods.Get_Object );

        groupCommand.AddParameter ( Evado.UniForm.Model.EuCommandParameters.Create_Object, "1" );

        groupCommand.AddParameter ( Evado.UniForm.Model.EuCommandParameters.Page_Id,
          Evado.Digital.Model.EdStaticPageIds.Form_Field_Page.ToString ( ) );

        groupCommand.AddParameter ( EuRecordFields.CONST_FIELD_COUNT,
          this.Session.RecordLayout.Fields.Count.ToString ( ) );

        groupCommand.AddParameter ( EuRecordFields.CONST_FORM_SECTION,
          "-1" );

        groupCommand.SetBackgroundColour (
          Evado.UniForm.Model.EuCommandParameters.BG_Default,
          Evado.UniForm.Model.EuBackgroundColours.Purple );
      }

      //
      // Iterate through the fields in the form, selecting those fields that are
      // associated with the current section.
      //
      foreach ( EdRecordField field in this.Session.RecordLayout.Fields )
      {
        //
        // if the field is in a section skip to the next field.
        //
        if ( field.Design.SectionNo != -1 )
        {
          continue;
        }

        //
        // Add the fields groupCommand object.
        // using the Guid for the unique field identifier.
        //
        groupCommand = pageGroup.addCommand (
          field.LinkText,
          EuAdapter.ADAPTER_ID,
         EuAdapterClasses.Record_Layout_Fields.ToString ( ),
          Evado.UniForm.Model.EuMethods.Get_Object );

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
    /// <param name="PageCommand">Evado.UniForm.Model.EuCommand PageCommand object.</param>
    /// <returns>Evado.UniForm.Model.EuAppData object</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.UniForm.Model.EuAppData createObject (
      Evado.UniForm.Model.EuCommand PageCommand )
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
        this.AdapterObjects.AllRecordLayouts = new List<EdRecord> ( );
        Evado.UniForm.Model.EuAppData clientDataObject = new Evado.UniForm.Model.EuAppData ( );
        this.Session.RecordLayout = new Evado.Digital.Model.EdRecord ( );
        this.Session.RecordLayout.Guid = Evado.Digital.Model.EvcStatics.CONST_NEW_OBJECT_ID;
        this.Session.RecordLayout.Design.Version = 0.0F;
        this.Session.RecordLayout.State = EdRecordObjectStates.Form_Draft;

        //
        // Set the form type to the current setting if it is a project form.
        //
        this.Session.RecordLayout.Design.TypeId = this.Session.Selected_RecordType;

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
        this.ErrorMessage = EdLabels.Record_Create_Error_Message;

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
    /// <param name="PageCommand">Evado.UniForm.Model.EuCommand object.</param>
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
    private Evado.UniForm.Model.EuAppData updateObject ( Evado.UniForm.Model.EuCommand PageCommand )
    {
      this.LogInitMethod ( "updateObject" );
      this.LogValue ( "Parameter PageCommand: " + PageCommand.getAsString ( false, false ) );

      this.LogDebug ( "SessionObjects.Forms" + " Guid: " + this.Session.RecordLayout.Guid );
      this.LogDebug ( " Title: " + this.Session.RecordLayout.Title );
      try
      {
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
        if ( this.Session.RecordLayout.Guid == Guid.Empty
          && formGuid != Guid.Empty )
        {
          this.Session.RecordLayout = this._Bll_RecordLayouts.GetLayout ( formGuid );
        }

        //
        // Rung the server script if server side scripts are enabled.
        //
        this.runServerScript ( EvServerPageScript.ScripEventTypes.OnUpdate );

        // 
        // Initialise the update parameter values.
        // 
        if ( this.Session.RecordLayout.Guid == Evado.Digital.Model.EvcStatics.CONST_NEW_OBJECT_ID )
        {
          this.Session.RecordLayout.Guid = Guid.Empty;
        }

        // 
        // As Evado eClinical does not have a delete record function return true if groupCommand sent.
        // 
        if ( PageCommand.Method == Evado.UniForm.Model.EuMethods.Delete_Object )
        {
          return new Evado.UniForm.Model.EuAppData ( );
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

        // 
        // Get the save action message value.
        // 
        this.Session.RecordLayout.SaveAction = EdRecord.SaveActionCodes.Layout_Saved;
        String saveAction = PageCommand.GetParameter ( Evado.Digital.Model.EvcStatics.CONST_SAVE_ACTION );
        if ( saveAction != String.Empty )
        {
          this.Session.RecordLayout.SaveAction =
             Evado.Model.EvStatics.parseEnumValue<EdRecord.SaveActionCodes> ( saveAction );
        }

        // 
        // Resets the save action to the parameter passed in the page groupCommand.
        // 
        if ( this.Session.RecordLayout.SaveAction == EdRecord.SaveActionCodes.Save )
        {
          this.Session.RecordLayout.SaveAction = EdRecord.SaveActionCodes.Layout_Saved;
        }
        this.LogValue ( "Save Action: " + this.Session.RecordLayout.SaveAction );

        // 
        // Execute the save record groupCommand to save the record values to the 
        // Evado database.
        // 
        EvEventCodes result = this._Bll_RecordLayouts.SaveItem ( this.Session.RecordLayout );

        this.LogClass ( this._Bll_RecordLayouts.Log );

        // 
        // If an error state is returned log the event.
        // 
        if ( result != EvEventCodes.Ok )
        {
          this.ErrorMessage = String.Format (
            EdLabels.Form_Update_Error_Message,
            EvStatics.getEventMessage ( result ) );

          string sStEvent = "Returned error message: "
            + result + " = " + Evado.Digital.Model.EvcStatics.getEventMessage ( result )
            + "\r\n" + this._Bll_RecordLayouts.Log;

          this.LogError ( EvEventCodes.Database_Record_Update_Error, sStEvent );

          this.Session.LastPage.Message = this.ErrorMessage;

          return this.Session.LastPage;
        }

        //
        // If a revision or copy is made rebuild the list
        //
        this.AdapterObjects.AllRecordLayouts = new List<EdRecord> ( );

        this.LogMethodEnd ( "updateObject" );

        return new Evado.UniForm.Model.EuAppData ( );

      }
      catch ( Exception Ex )
      {
        // 
        // If an exception is raised create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.Record_Update_Error_Message;

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
    /// <param name="PageCommand">Evado.UniForm.Model.EuCommand objects.</param>
    /// <returns>Bool: True is field ID is validate.</returns>
    //  ----------------------------------------------------------------------------------
    private bool validateFormId ( Evado.UniForm.Model.EuCommand PageCommand )
    {
      this.LogMethod ( "validateFormId" );

      //
      // Iterate through the form fields checking to ensure there are not duplicate field identifiers.
      //
      foreach ( EdRecord form in this.AdapterObjects.AllRecordLayouts )
      {
        this.LogValue ( "Form.Guid: " + form.Guid + ", FormId: " + form.LayoutId );

        if ( form.LayoutId == this.Session.RecordLayout.LayoutId
          && this.Session.RecordLayout.Guid != form.Guid
          && this.Session.RecordLayout.State == form.State )
        {
          this.ErrorMessage = String.Format ( EdLabels.Form_Duplicate_ID_Error_Message,
            this.Session.RecordLayout.LayoutId );

          this.LogValue ( this.ErrorMessage );
          this.LogMethodEnd ( "validateFormId" );
          return false;
        }
      }//END iteration statement

      this.LogMethodEnd ( "validateFormId" );
      return true;
    }//END validateFormId method.

    // ==================================================================================
    /// <summary>
    /// Ttis method updates the form record values with the groupCommand parameter values.
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.EuCommand objects.</param>
    //  ----------------------------------------------------------------------------------
    private void updateObjectValues (
      Evado.UniForm.Model.EuCommand PageCommand )
    {
      this.LogInitMethod ( "updateObjectValue" );
      this.LogValue ( "PageCommand.Parameters.Count: " + PageCommand.Parameters.Count );

      // 
      // Iterate through the parameter values updating the ResultData object
      // 
      foreach ( Evado.UniForm.Model.EuParameter parameter in PageCommand.Parameters )
      {
        if ( parameter.Name.Contains ( Evado.Digital.Model.EvcStatics.CONST_GUID_IDENTIFIER ) == true
          || parameter.Name.Contains ( EuRecordLayouts.CONST_SECTION_FIELD_PREFIX ) == true
          || parameter.Name.Contains ( Evado.UniForm.Model.EuField.CONST_FIELD_ANNOTATION_SUFFIX ) == true
          || parameter.Name.Contains ( Evado.UniForm.Model.EuField.CONST_FIELD_QUERY_SUFFIX ) == true
          || parameter.Name.Contains ( EuRecordLayouts.CONST_UPDATE_SECTION_COMMAND_PARAMETER ) == true
          || parameter.Name.Contains ( "Sectn_" ) == true
          || this.isFormField ( parameter.Name ) == true
          || parameter.Name == Evado.UniForm.Model.EuCommandParameters.Custom_Method.ToString ( )
          || parameter.Name == Evado.UniForm.Model.EuCommandParameters.Page_Id.ToString ( )
          || parameter.Name == Evado.Digital.Model.EvcStatics.CONST_SAVE_ACTION
          || parameter.Name == EuRecordLayouts.CONST_IMPORT_FORM_DATA
          || parameter.Name == EuRecordLayouts.CONST_EXPORT_FORM_DATA
          || parameter.Name == EuRecordGenerator.CONST_FORM_COMMENT_FIELD_ID )
        {
          this.LogDebug ( parameter.Name + " > " + parameter.Value + " >> SKIPPED" );
          continue;
        }

        this.LogDebug ( parameter.Name + " > " + parameter.Value + " >> UPDATED" );
        try
        {
          EdRecord.FieldNames fieldName = Evado.Model.EvStatics.parseEnumValue<EdRecord.FieldNames> ( parameter.Name );

          this.Session.RecordLayout.setValue ( fieldName, parameter.Value );

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

      foreach ( EdRecordField field in this.Session.RecordLayout.Fields )
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