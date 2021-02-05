/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\FormRecords.cs" company="EVADO HOLDING PTY. LTD.">
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

namespace Evado.UniForm.Digital
{
  /// <summary>
  /// This class defines the application base classs that is used to terminate the 
  /// hosted application objects.
  /// 
  /// This class terminates the Customer object.
  /// </summary>
  public class EuRecords : EuClassAdapterBase
  {
    #region Class Initialisation
    /// <summary>
    /// This method initialises the class.
    /// </summary>
    public EuRecords ( )
    {
      if ( this.Session.Record == null )
      {
        this.Session.Record = new EdRecord ( );
      }
      if ( this.Session.RecordList == null )
      {
        this.Session.RecordList = new List<EdRecord> ( );
      }
      if ( this.Session.RecordSelectionLayoutId == null )
      {
        this.Session.RecordSelectionLayoutId = String.Empty;
      }

      this.ClassNameSpace = " Evado.UniForm.Clinical.EuRecords.";
    }

    /// <summary>
    /// This method initialises the class and passs in the user profile.
    /// </summary>
    public EuRecords (
      EuAdapterObjects ApplicationObjects,
      EvUserProfileBase ServiceUserProfile,
      EuSession SessionObjects,
      String UniForm_BinaryFilePath,
      String UniForm_BinaryServiceUrl,
      EvClassParameters Settings )
    {
      this.ClassNameSpace = " Evado.UniForm.Clinical.EuRecords.";
      this.GlobalObjects = ApplicationObjects;
      this.ServiceUserProfile = ServiceUserProfile;
      this.Session = SessionObjects;
      this.UniForm_BinaryFilePath = UniForm_BinaryFilePath;
      this.UniForm_BinaryServiceUrl = UniForm_BinaryServiceUrl;
      this.ClassParameters = Settings;

      this.LogInitMethod ( "EuRecords initialisation" );
      this.LogInit ( "ServiceUserProfile.UserId: " + ServiceUserProfile.UserId );
      this.LogInit ( "SessionObjects.UserProfile.Userid: " + this.Session.UserProfile.UserId );
      this.LogInit ( "SessionObjects.UserProfile.CommonName: " + this.Session.UserProfile.CommonName );
      this.LogInit ( "UniForm BinaryFilePath: " + this.UniForm_BinaryFilePath );
      this.LogInit ( "UniForm BinaryServiceUrl: " + this.UniForm_BinaryServiceUrl );


      this.LogInit ( "Settings" );
      this.LogInit ( "-LoggingLevel: " + Settings.LoggingLevel );
      this.LogInit ( "-UserId: " + Settings.UserProfile.UserId );
      this.LogInit ( "-UserCommonName: " + Settings.UserProfile.CommonName );

      this._Bll_FormRecords = new EdRecords ( Settings );

      if ( this.Session.Record == null )
      {
        this.Session.Record = new EdRecord ( );
      }
      if ( this.Session.RecordList == null )
      {
        this.Session.RecordList = new List<EdRecord> ( );
      }
      if ( this.Session.RecordSelectionLayoutId == null )
      {
        this.Session.RecordSelectionLayoutId = String.Empty;
      }



    }//END Method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class enumerations

    public enum RecordAccess
    {
      Record_Read_Only,

      Record_Author_Access,
    }

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class constants and variables.

    private Evado.Bll.Clinical.EdRecords _Bll_FormRecords = new EdRecords ( );
    private EvServerPageScript _ServerPageScript = new EvServerPageScript ( );

    //
    // Initialise the page labels
    //
    /// <summary>
    /// This constant defines the administrator field identifier prefix
    /// </summary>
    public const string CONST_ADMIN_FIELD_ID = "ADMIN_";
    /// <summary>
    /// This constant defines the include draft record property identifier.
    /// </summary>
    public const string CONST_INCLUDE_DRAFT_RECORDS = "IDR";
    /// <summary>
    /// This constant defines the include free text property identifier.
    /// </summary>
    public const string CONST_INCLUDE_FREE_TEXT_DATA = "IFTD";
    /// <summary>
    /// This constand definee the include test sites property identifier
    /// </summary>
    public const string CONST_INCLUDE_TEST_SITES = "IFTS";

    /// <summary>
    /// This constant defines the draft record icon URL.
    /// </summary>
    public const string ICON_RECORD_DRAFT = "icons/record-draft.png";

    /// <summary>
    /// This constant defines the submitted record icon URL.
    /// </summary>
    public const string ICON_RECORD_SUBMITTED = "icons/record-submitted.png";

    /// <summary>
    /// This constant defines the source date reviewed record icon URL.
    /// </summary>
    public const string ICON_RECORD_SDR = "icons/record-sdr.png";

    /// <summary>
    /// This constant defines the queried record icon URL.
    /// </summary>
    public const string ICON_RECORD_QUERIED = "icons/record-queried.png";

    /// <summary>
    /// This constant defines the locked record icon URL.
    /// </summary>
    public const string ICON_RECORD_LOCKED = "icons/record-locked.png";

    /// <summary>
    /// This constant defines the withdrawn record icon URL.
    /// </summary>
    public const string ICON_RECORD_DELETED = "icons/record-withdrawn.png";

    /// <summary>
    /// This constant defines the draft record icon URL.
    /// </summary>

    public const string ICON_QUERIED_OPEN = "icons/query-open.png";

    /// <summary>
    /// This constant defines the draft record icon URL.
    /// </summary>
    public const string ICON_QUERIED_CLOSED = "icons/query-closed.png";

    //public const string CONST_DRAFT_ICON = "{{ICON0}}";
    //public const string CONST_SUBMITTED_ICON = "{{ICON1}}";
    //public const string CONST_SDR_ICON = "{{ICON2}}";
    //public const string CONST_QUERIED_ICON = "{{ICON3}}";
    //public const string CONST_LOCKED_ICON = "{{ICON4}}";
    //public const string CONST_DELETED_QUERY_ICON = "{{ICON5}}";

    //public const string CONST_OPEN_QUERY_ICON = "{{ICON6}}";
    //public const string CONST_CLOSED_QUERY_ICON = "{{ICON7}}";

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class public methods

    // ===============================================================================
    /// <summary>
    /// This method gets the application object from the list.
    /// </summary>
    /// <param name="PageCommand">ClientPateEvado.Model.UniForm.Command object</param>
    /// <returns>Evado.Model.UniForm.AppData</returns>
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
        // UPdate the sessin variables.
        //
        this.updateSessionValue ( PageCommand );

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
              this.LogDebug ( "Get List of object method" );

              switch ( this.Session.PageId )
              {
                case EvPageIds.Record_Export_Page:
                  {
                    clientDataObject = this.getRecordExport_Object ( PageCommand );
                    break;
                  }
                default:
                  {
                    clientDataObject = this.getListObject ( PageCommand );
                    break;
                  }
              }//END RecordPageType switch

              break;

            }//END get list of objects case

          // 
          // Select the method to retrieve a record object.
          // 
          case Evado.Model.UniForm.ApplicationMethods.Get_Object:
            {
              this.LogDebug ( "Get Object method" );

              clientDataObject = this.getObject ( PageCommand );

              break;

            }//END get object case

          // 
          // Select the groupCommand to create a new record object.
          // 
          case Evado.Model.UniForm.ApplicationMethods.Create_Object:
            {
              this.LogDebug ( "Create Object method" );

              clientDataObject = this.createObject ( PageCommand );

              break;
            }//END create case

          // 
          // Select the method to update the record object.
          // 
          case Evado.Model.UniForm.ApplicationMethods.Save_Object:
            {
              this.LogDebug ( "Save Object method" );

              // 
              // Update the object values
              // 
              clientDataObject = this.updateObject ( PageCommand );

              break;

            }//END save case.

          case Evado.Model.UniForm.ApplicationMethods.Delete_Object:
            {
              this.LogDebug ( "Delete Object method" );

              // 
              // Update the object values
              // 
              clientDataObject = this.Session.LastPage;

              break;

            }//END save case.

        }//END Switch


        // 
        // Handle returned exceptions.
        // 
        if ( clientDataObject == null )
        {
          this.LogValue ( " null application data returned." );
          this.LogMethodEnd ( "getClientDataObject" );
          return this.Session.LastPage;
        }

        //
        // If there is an error message add it to the output object.
        //
        if ( this.ErrorMessage != String.Empty )
        {
          clientDataObject.Message = this.ErrorMessage;
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

      return this.Session.LastPage;

    }//END getRecordObject methods

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class record locking and unlocking methods.

    // ==============================================================================
    /// <summary>
    /// This method checks the unlock status of the record.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object</param>
    /// <returns>True:  lock successful</returns>
    //  ------------------------------------------------------------------------------
    private bool checkRecordLockStatus (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "checkRecordLockStatus" );

      //
      // Check if the record is not locked.
      //
      if ( this.Session.Record.BookedOutBy == String.Empty )
      {
        this.LogDebug ( "Record not locked" );
        this.LogMethodEnd ( "checkRecordLockStatus" );
        return false;
      }

      // 
      // Test if the record is already locked.
      // 
      if ( this.Session.Record.BookedOutBy != String.Empty
        && this.Session.Record.BookedOutBy != this.Session.UserProfile.CommonName )
      {
        this.ErrorMessage =
          String.Format ( EdLabels.Form_Record_Locked_Message,
          this.Session.Record.RecordId,
          this.Session.Record.BookedOutBy );

        //
        // If the user is an administrator display a command to unlock the record.
        //
        if ( this.Session.UserProfile.hasManagementAccess )
        {
          Evado.Model.UniForm.Command pageCommand = PageObject.addCommand (
            EdLabels.Form_UnLock_Command_Title,
            EuAdapter.ADAPTER_ID,
            EuAdapterClasses.Records.ToString ( ),
             Model.UniForm.ApplicationMethods.Save_Object );

          pageCommand.SetGuid ( this.Session.Record.Guid );
        }

        return true;
      }

      //DEBUG

      return true;

      //
      // Do not lock the record is the user does not have update access.
      //
      if ( this.Session.UserProfile.hasEndUserRole( this.Session.Record.Design.ReadAccessRoles ) == false )
      {
        return false;
      }

      //
      // Execute the record lock method.
      //
      EvEventCodes iReturn = this._Bll_FormRecords.lockItem ( this.Session.Record );

      if ( iReturn != EvEventCodes.Ok )
      {
        return true;
      }

      // 
      // Set a lock value so we can unlock the record when exited without saving.
      // 
      this.Session.Record.BookedOutBy = this.Session.UserProfile.CommonName;

      return false;
    }

    // ==============================================================================
    /// <summary>
    /// This method unlocks the record
    /// </summary>
    /// <returns>True:  lock successful</returns>
    //  ------------------------------------------------------------------------------
    public bool unLockRecord ( )
    {
      // 
      // Test if the record is already locked.
      // 
      if ( this.Session.Record.BookedOutBy == String.Empty
        || this.Session.Record.BookedOutBy != this.Session.UserProfile.CommonName )
      {
        return true;
      }

      // 
      // Execute the unlock method to the database.
      // 
      EvEventCodes iReturn = this._Bll_FormRecords.unlockItem ( this.Session.Record );

      if ( iReturn != EvEventCodes.Ok )
      {
        return false;
      }

      return true;

    }//END unLockRecord method

    // ==============================================================================
    /// <summary>
    /// This method unlocks the record
    /// </summary>
    /// <returns>True:  lock successful</returns>
    //  ------------------------------------------------------------------------------
    public Evado.Model.UniForm.AppData unLockRecord_Admin ( )
    {
      // 
      // Test if the record is already locked.
      // 
      if ( this.Session.UserProfile.hasManagementAccess )
      {
        return new Model.UniForm.AppData ( );
      }

      // 
      // Execute the unlock method to the database.
      // 
      EvEventCodes iReturn = this._Bll_FormRecords.unlockItem ( this.Session.Record );

      if ( iReturn != EvEventCodes.Ok )
      {
        this.ErrorMessage = EdLabels.Form_Record_Admin_Unlock_Error_Message;
        return this.Session.LastPage;
      }

      return new Evado.Model.UniForm.AppData ( );

    }//END unLockRecord_Admin method

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class common private methods.

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    // ------------------------------------------------------------------------------
    private void updateSessionValue (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "updateSessionValue" );


      if ( PageCommand.hasParameter ( EdRecord.RecordFieldNames.Layout_Id.ToString() ) == true )
      {
        this.Session.RecordSelectionLayoutId = PageCommand.GetParameter ( EdRecord.RecordFieldNames.Layout_Id.ToString() );
      }
      this.LogValue ( "RecordSelectionFormId: " + this.Session.RecordSelectionLayoutId );

      if ( PageCommand.hasParameter ( EdRecord.CONST_RECORD_TYPE ) == true )
      {
        var recordType = PageCommand.GetParameter<EdRecordTypes> ( EdRecord.CONST_RECORD_TYPE );

        if ( this.Session.RecordType != recordType )
        {
          this.Session.RecordType = PageCommand.GetParameter<EdRecordTypes> ( EdRecord.CONST_RECORD_TYPE );
          this.Session.AdminRecordList = new List<EdRecord> ( );
          this.Session.RecordType = recordType;
        }
      }
      this.LogValue ( "RecordType: " + this.Session.RecordType );


      if ( PageCommand.hasParameter ( EdRecord.RecordFieldNames.Status.ToString() ) == true )
      {
        var stateValue = PageCommand.GetParameter<EdRecordObjectStates> ( EdRecord.RecordFieldNames.Status.ToString() );

        if ( this.Session.RecordSelectionState != stateValue )
        {
          if ( stateValue != EdRecordObjectStates.Null )
          {
            this.Session.AdminRecordList = new List<EdRecord> ( );
            this.Session.RecordSelectionState = stateValue;
          }
          else
          {
            this.Session.AdminRecordList = new List<EdRecord> ( );
            this.Session.RecordSelectionState = EdRecordObjectStates.Null;
          }
        }
      }
      this.LogValue ( "RecordSelectionState: " + this.Session.RecordSelectionState );

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

    // ==============================================================================
    /// <summary>
    /// This method returns a list of record selection options.
    /// </summary>
    //  ------------------------------------------------------------------------------
    private List<EvOption> getRecordTypesList ( )
    {
      // 
      // Initialise the methods variables and objects.
      // 
      List<EvOption> recordTypes = new List<EvOption> ( );
      EvOption option = new EvOption ( );

      // 
      // Fill the TestReport selection types
      // 
      option = new EvOption ( EdRecordTypes.Normal_Record.ToString ( ),
         Evado.Model.Digital.EvcStatics.Enumerations.enumValueToString ( EdRecordTypes.Normal_Record ) );

      recordTypes.Add ( option );
      option = new EvOption ( EdRecordTypes.Questionnaire.ToString ( ),
         Evado.Model.Digital.EvcStatics.Enumerations.enumValueToString ( EdRecordTypes.Questionnaire ) );

      recordTypes.Add ( option );
      option = new EvOption ( EdRecordTypes.Updatable_Record.ToString ( ),
         Evado.Model.Digital.EvcStatics.Enumerations.enumValueToString ( EdRecordTypes.Updatable_Record ) );

      return recordTypes;
    }

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class record list methods

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  -----------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData getListObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getListObject" );
      this.LogValue ( "RecordSelectionState: " + this.Session.RecordSelectionState );
      this.LogValue ( "RecordSelectionLayoutId: " + this.Session.RecordSelectionLayoutId );
      try
      {
        // 
        // Initialise the methods variables and objects.
        // 
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
        EdQueryParameters queryParameters = new EdQueryParameters ( );
        List<EdRecord> recordList = new List<EdRecord> ( );

        // 
        // If the user does not have monitor or ResultData manager roles exit the page.
        // 
        /*
        if ( this.Session.UserProfile.hasEndUserRole( this.Session.Record.Design.ReadAccessRoles ) == false )
        {
          this.LogIllegalAccess (
            this.ClassNameSpace + "FormRecords.getListObject",
            this.Session.UserProfile );

          this.ErrorMessage = EdLabels.Record_Access_Error_Message;

          return this.Session.LastPage; ;
        }
        */
        // 
        // Log the user's access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "FormRecords.getListObject",
          this.Session.UserProfile );

        // 
        // Initialise the client ResultData object.
        // 
        clientDataObject.Id = Guid.NewGuid ( );
        clientDataObject.Page.Id = clientDataObject.Id;
        clientDataObject.Page.PageDataGuid = clientDataObject.Id;
        clientDataObject.Title = EdLabels.Record_View_Page_Title;
        clientDataObject.Page.Title = clientDataObject.Title;
        clientDataObject.Page.PageId = EvPageIds.Records_View.ToString ( );

        // 
        // Create the new pageMenuGroup for query selection.
        // 
        this.getList_SelectionGroup ( clientDataObject.Page );

        //
        // Execute the monitor list record query.
        //
        this.executeRecordQuery ( );

        // 
        // Create the pageMenuGroup containing commands to open the records.
        //         
        this.getRecord_ListGroup ( clientDataObject.Page );

        this.LogValue ( "data.Page.Title: " + clientDataObject.Page.Title );


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

      return this.Session.LastPage; ;

    }//END getListObject method.

    // ==============================================================================
    /// <summary>
    /// This method executed the form record query of the database.
    /// </summary>
    /// <param name="queryParameters">EvQueryParameters: conting the query parameters</param>
    /// <remarks>
    /// This method returns a list of forms based on the selection type of form record.
    /// </remarks>
    //  ------------------------------------------------------------------------------
    private void executeRecordQuery ( )
    {
      this.LogMethod ( "executeRecordQuery" );
      this.LogValue ( "FormRecordType: " + this.Session.RecordType );
      this.LogValue ( "RecordSelectionState: " + this.Session.RecordSelectionState );
      //
      // Initialise the methods variables and objects.
      //
      EdQueryParameters queryParameters = new EdQueryParameters ( );

      // 
      // Initialise the query values to the currently selected objects identifiers.
      // 
      queryParameters.LayoutId = this.Session.RecordSelectionLayoutId;

      // 
      // Initialise the query state selection.
      // 
      queryParameters.States.Add ( EuAdapter.CONST_RECORD_STATE_SELECTION_DEFAULT );
      queryParameters.NotSelectedState = true;

      if ( this.Session.RecordSelectionState != EdRecordObjectStates.Null )
      {
        queryParameters.States.Add ( this.Session.RecordSelectionState );
        queryParameters.NotSelectedState = false;
      }

      this.LogValue ( "TrialId: '" + queryParameters.ApplicationId + "'" );
      this.LogValue ( "FormId: '" + queryParameters.LayoutId + "'" );

      // 
      // Query the database to retrieve a list of the records matching the query parameter values.
      // 
      if ( queryParameters.ApplicationId != String.Empty
        && queryParameters.LayoutId != String.Empty )
      {
        this.LogValue ( "FormRecordType: " + this.Session.RecordType );

        this.LogValue ( "Querying form records" );
        this.Session.RecordList = this._Bll_FormRecords.getRecordList ( queryParameters );

        this.LogDebugClass ( this._Bll_FormRecords.Log );
        this.LogValue ( "list count: " + this.Session.RecordList.Count );
      }

      this.LogMethodEnd ( "executeRecordQuery" );

    }//END executeRecordQuery method.

    // ==============================================================================
    /// <summary>
    /// This method creates the record view pageMenuGroup containing a list of commands to 
    /// open the form record.
    /// </summary>
    /// <param name="Page">Evado.Model.Uniform.Page object to add the pageMenuGroup to.</param>
    /// <param name="subjects">EvSubjects subjects to add to selection groups</param>
    /// <param name="subjectVisits">EvSubjectMilestones visits for each subject</param>
    /// <param name="QueryParameters">EvQueryParameters: conting the query parameters</param>
    /// <param name="ApplicationObject">Adapter.ApplicationObjects object.</param>
    //  ------------------------------------------------------------------------------
    private void getList_SelectionGroup (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getList_SelectionGroup" );
      this.LogDebug ( "FormList.Count {0}. ", this.Session.RecordLayoutList.Count );
      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      List<EvOption> optionList;
      Evado.Model.UniForm.Field selectionField;

      // 
      // Create the new pageMenuGroup for record selection.
      // 
      pageGroup = Page.AddGroup (
        EdLabels.Record_Selection_Group_Title,
        Evado.Model.UniForm.EditAccess.Enabled );
      pageGroup.GroupType = Evado.Model.UniForm.GroupTypes.Default;
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
      pageGroup.AddParameter ( Model.UniForm.GroupParameterList.Offline_Hide_Group, true );

      // 
      // Add the record state selection option
      //
      optionList = new List<EvOption> ( );
      optionList.Add ( new EvOption ( ) );
      foreach ( EdRecord layout in this.Session.RecordLayoutList )
      {
        optionList.Add ( new EvOption ( layout.LayoutId,
          String.Format ( "{0} - {1}", layout.LayoutId, layout.Title ) ) );
      }

      selectionField = pageGroup.createSelectionListField (
        EdRecord.RecordFieldNames.Layout_Id,
        EdLabels.Label_Form_Id,
        this.Session.RecordSelectionLayoutId,
        optionList );

      selectionField.Layout = EuRecordGenerator.ApplicationFieldLayout;
      selectionField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );

      // 
      // Add the record state selection option
      // 
      optionList = EdRecord.getRecordStates ( false );

      selectionField = pageGroup.createSelectionListField (
        EdRecord.RecordFieldNames.Status.ToString(),
        EdLabels.Record_State_Selection,
        this.Session.RecordSelectionState,
        optionList );

      selectionField.Layout = EuRecordGenerator.ApplicationFieldLayout;
      selectionField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );

      // 
      // Add the selection groupCommand
      // 
      Evado.Model.UniForm.Command selectionCommand = pageGroup.addCommand ( EdLabels.Select_Records_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Records.ToString ( ),
         Evado.Model.UniForm.ApplicationMethods.Custom_Method );

      selectionCommand.setCustomMethod ( Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

    }//ENd getList_SelectionGroup method

    // ==============================================================================
    /// <summary>
    /// This method creates the record view pageMenuGroup containing a list of commands to 
    /// open the form record.
    /// </summary>
    /// <param name="Page">Evado.Model.Uniform.Page object to add the pageMenuGroup to.</param>
    /// <param name="RecordList">List of EvForm: form record objects.</param>
    //  ------------------------------------------------------------------------------
    private void getRecord_ListGroup (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "createRecordViewGroup" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Model.UniForm.Group ( );
      Evado.Model.UniForm.Command groupCommand = new Model.UniForm.Command ( );

      List<EdRecord> RecordList = this.Session.RecordList;

      // 
      // Create the record display pageMenuGroup.
      // 
      pageGroup = Page.AddGroup (
        EdLabels.Record_List_Group_Title,
        Evado.Model.UniForm.EditAccess.Enabled );
      pageGroup.CmdLayout = Evado.Model.UniForm.GroupCommandListLayouts.Vertical_Orientation;

      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      pageGroup.Title += EdLabels.List_Count_Label + RecordList.Count;

      //
      // Add a create record command.
      //
      if ( this.Session.RecordSelectionLayoutId != String.Empty )
      {
        groupCommand = pageGroup.addCommand (
          "New Record",
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Records, Model.UniForm.ApplicationMethods.Create_Object );
        groupCommand.SetBackgroundDefaultColour ( Model.UniForm.Background_Colours.Purple );
      }

      // 
      // Iterate through the record list generating a groupCommand to access each record
      // then append the groupCommand to the record pageMenuGroup view's groupCommand list.
      // 
      foreach ( Evado.Model.Digital.EdRecord formRecord in RecordList )
      {
        //
        // Create the group list groupCommand object.
        //
        groupCommand = this.getGroupListCommand (
          formRecord,
          pageGroup );

      }//END iteration loop

      this.LogValue ( "Group command count: " + pageGroup.CommandList.Count );

    }//END createViewCommandList method

    // ==============================================================================
    /// <summary>
    /// This method appends the milestone groupCommand to the page milestone list pageMenuGroup
    /// </summary>
    /// <param name="FormRecord">EvForm object</param>
    /// <param name="PageGroup"> Evado.Model.UniForm.Group</param>
    //  -----------------------------------------------------------------------------
    private Evado.Model.UniForm.Command getGroupListCommand (
      EdRecord FormRecord,
      Evado.Model.UniForm.Group PageGroup )
    {
      this.LogMethod ( "getGroupListCommand" );
      this.LogValue ( "FormRecord.RecordId: " + FormRecord.RecordId );

      //
      // Define the pageMenuGroup groupCommand.
      //
      Evado.Model.UniForm.Command groupCommand = PageGroup.addCommand (
          FormRecord.RecordId,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Records,
          Evado.Model.UniForm.ApplicationMethods.Get_Object );

      groupCommand.SetGuid ( FormRecord.Guid );

      groupCommand.AddParameter (
        Model.UniForm.CommandParameters.Short_Title,
        EdLabels.Label_Record_Id + FormRecord.RecordId );

      groupCommand.Title = String.Empty;

      if ( this.Session.UserProfile.hasEndUserRole( this.Session.Record.Design.ReadAccessRoles ) == false )
      {
        //
        // Switch to determine the icons and background colours.
        //
        switch ( FormRecord.State )
        {
          case EdRecordObjectStates.Draft_Record:
          case EdRecordObjectStates.Empty_Record:
          case EdRecordObjectStates.Completed_Record:
            {
              groupCommand.AddParameter (
               Model.UniForm.CommandParameters.Image_Url,
               EuRecords.ICON_RECORD_DRAFT );

              if ( this.Session.UserProfile.hasEndUserRole( this.Session.Record.Design.ReadAccessRoles ) == true )
              {

                groupCommand.SetBackgroundDefaultColour ( Model.UniForm.Background_Colours.Yellow );
              }
              break;
            }
          case EdRecordObjectStates.Submitted_Record:
            {
              groupCommand.AddParameter (
               Model.UniForm.CommandParameters.Image_Url,
               EuRecords.ICON_RECORD_SUBMITTED );

              break;
            }
        }//END state switch.
        //
        // Display multi-site user content.
        //
        groupCommand.Title =
          String.Format ( EdLabels.Form_Record_General_View_Title,
            FormRecord.RecordId,
            FormRecord.LayoutId,
            FormRecord.Title,
            String.Empty,
            String.Empty,
            String.Empty );

      }
      else
      {
        //
        // Switch to determine the icons and background colours.
        //
        switch ( FormRecord.State )
        {
          case EdRecordObjectStates.Draft_Record:
          case EdRecordObjectStates.Empty_Record:
          case EdRecordObjectStates.Completed_Record:
            {
              groupCommand.AddParameter (
               Model.UniForm.CommandParameters.Image_Url,
               EuRecords.ICON_RECORD_DRAFT );

              if ( FormRecord.IsMandatory == true )
              {
                groupCommand.SetBackgroundDefaultColour ( Model.UniForm.Background_Colours.Orange );
              }
              else
              {
                groupCommand.SetBackgroundDefaultColour ( Model.UniForm.Background_Colours.Yellow );
              }
              break;
            }
          case EdRecordObjectStates.Submitted_Record:
            {
              groupCommand.AddParameter (
               Model.UniForm.CommandParameters.Image_Url,
               EuRecords.ICON_RECORD_SUBMITTED );
              break;
            }
          case EdRecordObjectStates.Withdrawn:
            {
              groupCommand.AddParameter (
               Model.UniForm.CommandParameters.Image_Url,
                EuRecords.ICON_RECORD_DELETED );
              break;
            }
          default:
            {
              groupCommand.AddParameter (
               Model.UniForm.CommandParameters.Image_Url,
                EuRecords.ICON_RECORD_DRAFT );
              break;
            }
        }//END state switch.

        //
        // Display site user content.
        //
        groupCommand.Title = String.Format ( EdLabels.Form_Record_Site_View_Title,
            FormRecord.LayoutId,
            FormRecord.Title,
            String.Empty,
            String.Empty );

      }//END site users

      return groupCommand;

    }//END getGroupListCommand method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class record export methods

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData getRecordExport_Object (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getRecordExport_Object" );

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
      List<EdRecord> recordList = new List<EdRecord> ( );
      try
      {

        // 
        // If the user does not have monitor or ResultData manager roles exit the page.
        // 
        if ( this.Session.UserProfile.hasEndUserRole( this.Session.Record.Design.ReadAccessRoles ) == false )
        {
          this.LogIllegalAccess (
            this.ClassNameSpace + "getRecordExport_Object",
            this.Session.UserProfile );

          this.ErrorMessage = EdLabels.Record_Access_Error_Message;

          return this.Session.LastPage;
        }

        // 
        // Log the user's access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "getRecordExport_Object",
          this.Session.UserProfile );

        // 
        // Initialise the client ResultData object.
        // 
        clientDataObject.Id = Guid.NewGuid ( );
        clientDataObject.Page.Id = clientDataObject.Id;
        clientDataObject.Page.PageDataGuid = clientDataObject.Id;
        clientDataObject.Title = EdLabels.Record_View_Page_Title;
        clientDataObject.Page.Title = clientDataObject.Title;
        clientDataObject.Page.PageId = EvPageIds.Record_Export_Page.ToString ( );

        // 
        // Create the new pageMenuGroup for query selection.
        // 
        this.getRecordExport_SelectionGroup ( clientDataObject.Page );

        this.LogValue ( "FormId: " + this.Session.RecordSelectionLayoutId );
        this.LogValue ( "UserCommonName: " + this.Session.UserProfile.CommonName );

        //
        // Create the export file and save if for download.
        //
        this.getRecordExport_DownloadGroup ( clientDataObject.Page );

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

      return this.Session.LastPage;

    }//END getListObject method.

    // ==============================================================================
    /// <summary>
    /// This method creates the record view pageMenuGroup containing a list of commands to 
    /// open the form record.
    /// </summary>
    /// <param name="Page">Evado.Model.Uniform.Page object to add the pageMenuGroup to.</param>
    //  ------------------------------------------------------------------------------
    private void getRecordExport_SelectionGroup (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getRecordExport_SelectionGroup" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Model.UniForm.Group ( );
      Evado.Model.UniForm.Command command = new Model.UniForm.Command ( );
      Evado.Model.UniForm.Field selectionField = new Model.UniForm.Field ( );
      List<EvOption> optionList = new List<EvOption> ( );

      //
      // If the issued form list is empty fill it with the currently issued form selection objects.
      //
      if ( this.Session.IssueFormList.Count == 0 )
      {
        this.LogDebug ( "Issued Forms is empty so create list." );

        EdRecordLayouts forms = new EdRecordLayouts ( this.ClassParameters );

        this.Session.IssueFormList = forms.getList (
          EdRecordTypes.Null,
          EdRecordObjectStates.Form_Issued,
          false );
      }
      this.LogDebug ( "Issued Forms list count {0}.", this.Session.IssueFormList.Count );

      // 
      // Create the new pageMenuGroup for query selection.
      // 
      pageGroup = Page.AddGroup (
        EdLabels.Record_Selection_Group_Title,
        Evado.Model.UniForm.EditAccess.Enabled );
      pageGroup.GroupType = Evado.Model.UniForm.GroupTypes.Default;
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
      pageGroup.AddParameter ( Model.UniForm.GroupParameterList.Offline_Hide_Group, true );

      //
      // Add the selection pageMenuGroup.
      //
      selectionField = pageGroup.createSelectionListField (
        EdRecord.RecordFieldNames.Layout_Id,
        EdLabels.Label_Form_Id,
        this.Session.RecordSelectionLayoutId,
        this.Session.IssueFormList );

      selectionField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      //
      // Define the include draft record selection option.
      //
      selectionField = pageGroup.createBooleanField (
        EuRecords.CONST_INCLUDE_TEST_SITES,
        EdLabels.Record_Export_Include_Test_Sites_Field_Title,
        this.Session.FormRecords_IncludeTestSites );
      selectionField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      //
      // Define the include draft record selection option.
      //
      selectionField = pageGroup.createBooleanField (
        EuRecords.CONST_INCLUDE_DRAFT_RECORDS,
        EdLabels.Record_Export_Include_Draft_Record_Field_Title,
        this.Session.FormRecords_IncludeDraftRecords );
      selectionField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      //
      // Define the include free text ResultData selection option.
      //
      selectionField = pageGroup.createBooleanField (
        EuRecords.CONST_INCLUDE_FREE_TEXT_DATA,
        EdLabels.Record_Export_Include_FreeText_data_Field_Title,
        this.Session.FormRecords_IncludeFreeTextData );
      selectionField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      // 
      // Add the selection groupCommand
      // 
      command = pageGroup.addCommand (
        EdLabels.Record_Export_Selection_Group_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Records.ToString ( ),
         Evado.Model.UniForm.ApplicationMethods.Custom_Method );
      command.setCustomMethod ( Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

    }//END getRecordExport_ListObject method

    // ==============================================================================
    /// <summary>
    /// This method creates the record view pageMenuGroup containing a list of commands to 
    /// open the form record.
    /// </summary>
    /// <param name="Page">Evado.Model.Uniform.Page object to add the pageMenuGroup to.</param>
    //  ------------------------------------------------------------------------------
    private void getRecordExport_DownloadGroup (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getRecordExport_DownloadGroup" );
      this.LogDebug ( "FormRecords_IncludeTestSites: " + this.Session.FormRecords_IncludeTestSites );
      this.LogDebug ( "FormRecords_IncludeFreeTextData: " + this.Session.FormRecords_IncludeFreeTextData );
      this.LogDebug ( "FormRecords_IncludeDraftRecords: " + this.Session.FormRecords_IncludeDraftRecords );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Model.UniForm.Group ( );
      Evado.Model.UniForm.Field groupField = new Model.UniForm.Field ( );
      EvFormRecordExport exportRecords = new EvFormRecordExport ( );
      EvExportParameters exportParameters = new EvExportParameters ( );
      EdQueryParameters queryParameters = new EdQueryParameters ( );
      int exportRecordFileLimit = 250;
      this.LogDebug ( "exportRecordFileLimit: " + exportRecordFileLimit );

      //
      // IF there are not parameters then exit.
      //
      if ( this.Session.RecordSelectionLayoutId == String.Empty )
      {
        this.LogDebug ( " Form {0}. ", this.Session.RecordSelectionLayoutId );
        this.LogMethodEnd ( "getRecordExport_DownloadGroup" );
        return;
      }


      exportParameters = new EvExportParameters (
        EvExportParameters.ExportDataSources.Project_Record,
        this.Session.RecordSelectionLayoutId );
      exportParameters.IncludeTestSites = this.Session.FormRecords_IncludeTestSites;
      exportParameters.IncludeFreeTextData = this.Session.FormRecords_IncludeFreeTextData;
      exportParameters.IncludeDraftRecords = this.Session.FormRecords_IncludeDraftRecords;

      queryParameters = new EdQueryParameters ( );
      queryParameters.LayoutId = this.Session.RecordSelectionLayoutId;

      queryParameters.States.Add ( EdRecordObjectStates.Withdrawn );
      queryParameters.States.Add ( EdRecordObjectStates.Draft_Record );
      queryParameters.States.Add ( EdRecordObjectStates.Empty_Record );
      queryParameters.States.Add ( EdRecordObjectStates.Completed_Record );

      if ( exportParameters.IncludeDraftRecords == true )
      {
        queryParameters.States.Add ( EdRecordObjectStates.Withdrawn );
      }
      queryParameters.NotSelectedState = true;
      queryParameters.IncludeSummary = true;
      queryParameters.IncludeRecordValues = false;
      queryParameters.IncludeComments = false;

      //
      // Create the export ResultData file.
      //
      int inResultCount = this._Bll_FormRecords.getRecordCount ( queryParameters );

      this.LogClass ( this._Bll_FormRecords.Log );

      this.LogDebug ( "inResultCount: " + inResultCount );
      // 
      // Create the record display pageMenuGroup.
      // 
      pageGroup = Page.AddGroup (
        EdLabels.Record_Export_Download_Group_Title,
        Evado.Model.UniForm.EditAccess.Enabled );
      pageGroup.CmdLayout = Evado.Model.UniForm.GroupCommandListLayouts.Vertical_Orientation;

      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      if ( inResultCount == 0 )
      {
        LogDebug ( "Record Count is 0." );
        pageGroup.Description = EdLabels.Form_Record_Export_Empty_List_Error_Message;

        this.LogMethodEnd ( "getRecordExport_DownloadGroup" );
        return;
      }

      //
      // If the record list is less thant the exportrecord limit output.
      //
      if ( inResultCount <= exportRecordFileLimit )
      {
        //
        // export the ResultData.
        //
        this.exportRecordData (
            pageGroup,
            0,
           exportParameters,
             this.Session.RecordSelectionLayoutId );
      }
      else
      {
        //
        // Initialise the output loop parameters.
        int iRecordRangeStart = 0;
        int iRecordRangeFinish = exportRecordFileLimit;
        int iterations = ( inResultCount / exportRecordFileLimit ) + 1;

        this.LogValue ( "iterations: " + iterations );

        for ( int iLoop = 0; iLoop < iterations && iRecordRangeFinish <= inResultCount; iLoop++ )
        {

          //
          // export the ResultData.
          //
          var result = this.exportRecordData (
            pageGroup,
            iLoop,
             exportParameters,
             this.Session.RecordSelectionLayoutId );

          if ( result != EvEventCodes.Ok )
          {
            this.LogDebug ( "Result {0}.", result );
            this.LogMethodEnd ( "getRecordExport_DownloadGroup" );
            return;
          }

          //
          // Increment the start and finish for then next loop
          //
          iRecordRangeStart = iRecordRangeFinish + 1;
          if ( ( iRecordRangeFinish + exportRecordFileLimit ) < inResultCount )
          {

            iRecordRangeFinish += exportRecordFileLimit;
          }
          else
          {
            iRecordRangeFinish = inResultCount;
          }

        }//END loop iteration statement.
      }

      this.LogMethodEnd ( "getRecordExport_DownloadGroup" );
      return;
    }//END getRecordExport_DownloadGroup method

    //===================================================================================
    /// <summary>
    /// Thie method exports the record list to a export form.
    /// </summary>
    /// <param name="pageGroup">Evado.Model.UniForm.Group object</param>
    /// <param name="iteration">int: iteration loop</param>
    /// <param name="exportParameters">EvExportParameters object.</param>
    /// <param name="FormId">String form identifier</param>
    /// <returns>True export generated.</returns>
    //-----------------------------------------------------------------------------------
    private EvEventCodes exportRecordData (
      Evado.Model.UniForm.Group pageGroup,
      int iteration,
      EvExportParameters exportParameters,
      String FormId )
    {
      this.LogMethod ( "exportRecordData" );
      // 
      // Initialise the methods variables and objects.
      // 
      String csvDownload = String.Empty;
      String csvFileName = String.Empty;
      EvFormRecordExport exportRecords = new EvFormRecordExport ( this.ClassParameters );

      //
      // Generate the export download CSV ResultData file.
      //
      csvDownload = exportRecords.exportRecords (
        exportParameters,
        this.Session.UserProfile );

      if ( exportRecords.EventCode != EvEventCodes.Ok )
      {
        this.LogDebug ( "EventCode: " + exportRecords.EventCode );

        if ( exportRecords.EventCode == EvEventCodes.Data_Export_Empty_Record_List )
        {
          this.ErrorMessage = EdLabels.Form_Record_Export_Empty_List_Error_Message;
        }
        this.LogMethodEnd ( "exportRecordData" );
        return exportRecords.EventCode;
      }

      this.LogClass ( exportRecords.Log );
      //
      // Create the export file name.
      //
      csvFileName = FormId
        + "-Records-"
        + DateTime.Now.ToString ( "yy-MM-dd" ) + ".csv";

      if ( iteration > 0 )
      {
        csvFileName = FormId
        + "-Records-" + iteration
        + "-" + DateTime.Now.ToString ( "yy-MM-dd" ) + ".csv";
      }


      this.LogValue ( "csvDownload length: " + csvDownload.Length );
      this.LogValue ( "csvFileName: " + csvFileName );

      bool result = Evado.Model.Digital.EvcStatics.Files.saveFile (
        this.UniForm_BinaryFilePath,
        csvFileName,
        csvDownload );

      if ( result == false )
      {
        this.ErrorMessage = EdLabels.Record_Export_Error_Message;

        this.LogDebugClass ( Evado.Model.Digital.EvcStatics.Files.DebugLog );
        this.LogDebug ( "ReturnedEventCode: " + Evado.Model.Digital.EvcStatics.Files.ReturnedEventCode );
        this.LogDebug ( this.ErrorMessage );
        this.LogMethodEnd ( "exportRecordData" );
        return Evado.Model.Digital.EvcStatics.Files.ReturnedEventCode;
      }

      Evado.Model.UniForm.Field groupField = pageGroup.createHtmlLinkField (
        String.Empty,
        csvFileName,
      this.UniForm_BinaryServiceUrl + csvFileName );

      this.LogMethodEnd ( "exportRecordData" );
      return EvEventCodes.Ok;
    }//END exportRecordData method.

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class private get object methods

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData getObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getObject" );

      try
      {
        // 
        // Initialise the methods variables and objects.
        // 
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );

        // 
        // If the user does not have monitor or ResultData manager roles exit the page.
        // 
        if ( this.Session.UserProfile.hasDesignAccess == false )
        {
          this.LogIllegalAccess (
            this.ClassNameSpace + "getObject",
            this.Session.UserProfile );

          this.ErrorMessage = EdLabels.Record_Access_Error_Message;

          return this.Session.LastPage; ;
        }

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "getObject",
          this.Session.UserProfile );

        //
        // Get the record.

        var result = this.GetRecord ( PageCommand );
        // 
        // if the guid is empty the parameter was not found to exit.
        // 

        if ( result != EvEventCodes.Ok )
        {
          this.ErrorMessage = EdLabels.Record_Retrieve_Error_Message;

          this.LogError ( EvEventCodes.Database_Record_Retrieval_Error, "Retrieved Record is empty." );
          this.LogMethodEnd ( "getObject" );
          return this.Session.LastPage;
        }


        this.LogValue ( "Record.RecordId: " + this.Session.Record.RecordId );

        //
        // Rung the server script if server side scripts are enabled.
        //
        this.runServerScript ( EvServerPageScript.ScripEventTypes.OnOpen );

        // 
        // Generate the client ResultData object for the UniForm client.
        // 
        this.getClientData ( clientDataObject );

        // 
        // Return the client ResultData object to the calling method.
        // 
        this.LogMethodEnd ( "getObject" );
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


      this.LogMethodEnd ( "getObject" );
      return this.Session.LastPage; ;

    }//END getObject method

    //  =============================================================================== 
    /// <summary>
    /// runServerScript  method
    /// 
    /// Description:
    ///  This method initiates the execution of the server side CS scripts.
    /// 
    /// </summary>
    //  ---------------------------------------------------------------------------------
    private EvEventCodes GetRecord (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "GetRecord" );
      //
      // Initialise the methods variables and objects.
      //
      this._Bll_FormRecords = new EdRecords ( this.ClassParameters );
      Guid recordGuid = PageCommand.GetGuid ( );
      String recordId = PageCommand.GetParameter ( EdRecord.RecordFieldNames.RecordId.ToString() );

      //
      // If the record ids match then the record is loaded so exit.
      //
      if ( recordId == this.Session.Record.RecordId
        && recordId != String.Empty )
      {
        this.LogMethodEnd ( "GetRecord" );
        return EvEventCodes.Ok;
      }

      //
      // If the record Guid match then the record is loaded so exit.
      //
      if ( recordGuid == this.Session.Record.Guid
        && recordGuid != Guid.Empty )
      {
        this.LogMethodEnd ( "GetRecord" );
        return EvEventCodes.Ok;
      }

      //
      // Retrieve the record using the record identifier.
      //
      if ( recordId != String.Empty )
      {
        this.LogValue ( "recordId: " + recordId );
        // 
        // Retrieve the customer object from the database via the DAL and BLL layers.
        // 
        this.Session.Record = this._Bll_FormRecords.getRecord ( recordId );
      }
      else
      {
        // 
        // return if not trial id
        // 
        if ( recordGuid == Guid.Empty )
        {
          this.LogDebug ( "recordGuid is EMPTY" );
          this.LogMethodEnd ( "GetRecord" );
          return EvEventCodes.Identifier_Global_Unique_Identifier_Error;
        }

        // 
        // Retrieve the record object from the database via the DAL and BLL layers.
        // 
        this.Session.Record = this._Bll_FormRecords.getRecord ( recordGuid );
      }

      this.LogClass ( this._Bll_FormRecords.Log );

      this.LogDebug ( "There are {0} of fields in the record.", this.Session.Record.Fields.Count );

      //
      // return a retrieval error message if the resulting common record guid is empty.
      //
      if ( this.Session.Record.Guid == Guid.Empty )
      {
        return EvEventCodes.Database_Record_Retrieval_Error;
      }


      this.LogMethodEnd ( "GetRecord" );
      return EvEventCodes.Ok;
    }

    //  =============================================================================== 
    /// <summary>
    /// runServerScript  method
    /// 
    /// Description:
    ///  This method initiates the execution of the server side CS scripts.
    /// 
    /// </summary>
    //  ---------------------------------------------------------------------------------
    private bool runServerScript (
      EvServerPageScript.ScripEventTypes ScriptType )
    {
      this.LogMethod ( "runServerScript" );
      this.LogValue ( "RecordId " + this.Session.Record.RecordId );
      this.LogValue ( "hasCsScript = " + this.Session.Record.Design.hasCsScript );

      // 
      // if the formField has a CS Script execute the onPostBackForm method.
      // 
      if ( this.Session.Record.Design.hasCsScript == true )
      {
        this.LogValue ( "Server script executing." );

        //
        // Define the page to retrieve the script
        //
        this._ServerPageScript.CsScriptPath = this.GlobalObjects.ApplicationPath + @"\csScripts\";


        // 
        // Execute the onload script.
        // 
        EvEventCodes iReturn = this._ServerPageScript.runScript (
          ScriptType,
          this.Session.Record );

        this.LogValue ( "Server page script debug log: " + this._ServerPageScript.DebugLog );

        if ( iReturn != EvEventCodes.Ok )
        {
          this.ErrorMessage =
            "CsScript:" + ScriptType + " method failed \r\n"
            + Evado.Model.Digital.EvcStatics.Enumerations.enumValueToString ( iReturn ) + "\r\n";

          this.LogError ( EvEventCodes.Business_Logic_General_Process_Error,
            this.ErrorMessage );

          this.LogMethodEnd ( "getObject" );
          return false;

        }//END processing error return 

      }//END processing Cs formField script.

      this.LogMethodEnd ( "getObject" );
      return true;

    }//END runServerScript method


    // ==============================================================================
    /// <summary>
    /// This method sets the user access to the record object.
    /// </summary>
    /// <returns>RecordAccess object</returns>
    //  ------------------------------------------------------------------------------
    private RecordAccess setUserRecordAccess ( )
    {
      // 
      // Initialise method objects.
      // 
      this.LogMethod ( "setUserRecordAccess" );
      this.LogValue ( "RoleId: " + this.Session.UserProfile.Roles );
      this.LogValue ( "ActiveDirectoryUserId: " + this.Session.UserProfile.ActiveDirectoryUserId );
      this.LogValue ( "hasRecordEditAccess: " + this.Session.UserProfile.hasEndUserRole( this.Session.Record.Design.ReadAccessRoles ) );
      this.LogValue ( "Record state: " + this.Session.Record.StateDesc );

      // 
      // Switch to select the user's access to the record based on record state.
      // 
      switch ( this.Session.Record.State )
      {
        case EdRecordObjectStates.Draft_Record:
        case EdRecordObjectStates.Empty_Record:
        case EdRecordObjectStates.Completed_Record:
          {
            if ( this.Session.UserProfile.hasEndUserRole( this.Session.Record.Design.ReadAccessRoles ) == true )
            {
              // 
              // If the record state is draft of queried, and the user has Record Edit role
              // then grante the user author access to the record.
              // 
              return EuRecords.RecordAccess.Record_Author_Access;
            }
            break;
          }
        case EdRecordObjectStates.Submitted_Record:
          {
            if ( this.Session.UserProfile.hasEndUserRole( this.Session.Record.Design.ReadAccessRoles ) == true )
            {
              // 
              // If the record state is SubmittedRecords, and the user has Record Edit role
              // then grant the user author access to the record.
              // 
              return EuRecords.RecordAccess.Record_Author_Access;
            }
            break;
          }
      }//END switch

      // 
      // All other users have readonly access to the record.
      // 
      return RecordAccess.Record_Read_Only;

    }//END setUserRecordAccess

    // ==============================================================================
    /// <summary>
    /// This method creates a save groupCommand.
    /// </summary>
    /// <returns>Evado.Model.UniForm.Command object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.UniForm.Command createRecordSaveCommand (
      Guid RecordGuid,
      String SaveCommandTitle,
      String SaveAction )
    {
      this.LogMethod ( "createRecordSaveCommand" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Parameter parameter = new Evado.Model.UniForm.Parameter ( );

      // 
      // create the save groupCommand.
      // 
      Evado.Model.UniForm.Command saveCommand = new Evado.Model.UniForm.Command (
        SaveCommandTitle,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Records.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Save_Object );

      // 
      // Define the save groupCommand parameters.
      // 
      saveCommand.SetGuid ( RecordGuid );

      saveCommand.AddParameter (
        Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
       SaveAction );

      return saveCommand;
    }

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="ClientDataObject">Evado.Model.UniForm.AppData object.</param>
    //  ------------------------------------------------------------------------------
    private void getClientData (
      Evado.Model.UniForm.AppData ClientDataObject )
    {
      this.LogMethod ( "getClientData" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );
      Evado.Model.UniForm.Field pageField = new Evado.Model.UniForm.Field ( );
      Evado.Model.UniForm.Parameter parameter = new Evado.Model.UniForm.Parameter ( );

      EuRecordGenerator pageGenerator = new EuRecordGenerator (
        this.GlobalObjects,
        this.Session,
        this.ClassParameters );

      // 
      // Initialise the client ResultData object.
      // 
      ClientDataObject.Page.Id = ClientDataObject.Id;
      ClientDataObject.Page.PageDataGuid = ClientDataObject.Id;
      ClientDataObject.Page.PageId = this.Session.Record.LayoutId;
      ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      ClientDataObject.Id = this.Session.Record.Guid;
      ClientDataObject.Title = String.Format (
          EdLabels.Record_Page_Title_Subject_Prefix,
          String.Empty,
          this.Session.Record.RecordId,
          this.Session.Record.Title,
          this.Session.Record.StateDesc );


      ClientDataObject.Page.Title = ClientDataObject.Title;
      this.LogDebug
        ( "Title.Length: " + ClientDataObject.Title.Length );

      ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Disabled;

      //
      // The locked status of the record.
      //
      if ( this.checkRecordLockStatus ( ClientDataObject.Page ) == false )
      {
        this.LogValue ( "The record is not locked." );

        this.getDataObject_PageCommands ( ClientDataObject.Page );
      }
      ClientDataObject.Message = this.ErrorMessage;

      this.LogValue ( "GENERATE FORM" );

      // 
      // Call the page generation method
      // 
      bool result = pageGenerator.generateForm (
        this.Session.Record,
        ClientDataObject.Page,
        this.UniForm_BinaryFilePath );

      this.LogClass ( pageGenerator.Log );

      //
      // Return null and an error message if the page generator exits with an error.
      //
      if ( result == false )
      {
        this.ErrorMessage = EdLabels.FormRecord_Page_Generation_Error;

        this.LogMethodEnd ( "getPatientConsentDataObject " );

        return;
      }

      //
      // Display save command in the last group.
      //
      if ( ClientDataObject.Page.EditAccess == Model.UniForm.EditAccess.Enabled
        && ClientDataObject.Page.GroupList.Count > 0 )
      {
        this.LogValue ( "Including save commands in the last group." );
        Evado.Model.UniForm.Group group = ClientDataObject.Page.GroupList [ ClientDataObject.Page.GroupList.Count - 1 ];

        // 
        // Add the save groupCommand and add it to the page groupCommand list.
        // 
        pageCommand = this.createRecordSaveCommand (
            this.Session.Record.Guid,
            EdLabels.Record_Save_Command_Title,
            EdRecord.SaveActionCodes.Save_Record.ToString ( ) );

        group.addCommand ( pageCommand );

        // 
        // Add the submit comment to the page groupCommand list.
        // 
        pageCommand = this.createRecordSaveCommand (
          this.Session.Record.Guid,
          EdLabels.Record_Submit_Command,
          EdRecord.SaveActionCodes.Submit_Record.ToString ( ) );

        pageCommand.setEnableForMandatoryFields ( );

        group.addCommand ( pageCommand );

      }//END display state is edit.

      this.LogMethodEnd ( "getClientData" );

    }//END getClientData Method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
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
      // Set the user access to the records content.
      // 
      RecordAccess recordAccess = this.setUserRecordAccess ( );

      this.LogValue ( "Record Access: " + recordAccess );

      switch ( recordAccess )
      {
        case RecordAccess.Record_Read_Only:
          {
            break;
          }
        case RecordAccess.Record_Author_Access:
          {
            // 
            // If the user has author access. 
            // 
            // Set the page status to edit enabled
            // 
            PageObject.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
            PageObject.DefaultGroupType = Evado.Model.UniForm.GroupTypes.Default;


            // 
            // Add the save groupCommand and add it to the page groupCommand list.
            // 
            pageCommand = PageObject.addCommand (
              EdLabels.Record_Save_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Records.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.Save_Object );

            pageCommand.SetGuid ( this.Session.Record.Guid );

            pageCommand.AddParameter (
              Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
             EdRecord.SaveActionCodes.Save_Record.ToString ( ) );

            // 
            // Add the submit comment to the page groupCommand list.
            // 
            pageCommand = PageObject.addCommand (
              EdLabels.Record_Submit_Command,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Records.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.Save_Object );

            pageCommand.SetGuid ( this.Session.Record.Guid );
            pageCommand.AddParameter (
              Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
             EdRecord.SaveActionCodes.Submit_Record.ToString ( ) );

            pageCommand.setEnableForMandatoryFields ( );

            // 
            // Add the wihdrawn groupCommand to the page groupCommand list.
            // 
            if ( ( this.Session.Record.State == EdRecordObjectStates.Draft_Record
                || this.Session.Record.State == EdRecordObjectStates.Empty_Record
                || this.Session.Record.State == EdRecordObjectStates.Completed_Record ) )
            {
              pageCommand = PageObject.addCommand (
                EdLabels.Record_Withdraw_Command,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Records.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.Save_Object );

              pageCommand.SetGuid ( this.Session.Record.Guid );
              pageCommand.AddParameter (
                Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
               EdRecord.SaveActionCodes.Withdrawn_Record.ToString ( ) );
            }

            // 
            // Set the Record display state to edit enable the page.
            // 
            break;
          }
        default:
          {
            PageObject.EditAccess = Evado.Model.UniForm.EditAccess.Disabled;

            break;
          }
      } //END record access switch.

      this.LogMethodEnd ( "getDataObject_PageCommands" );

    }//END getClientData_SaveCommands method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class create record methods

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.ClientDataObjectEvado.Model.UniForm.Command object.</param>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData createObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "createObject" );
      this.LogDebug ( "RecordSelectionLayoutId: " + this.Session.RecordSelectionLayoutId );
      try
      {
        //
        // Initialiset the methods variables and objects.
        //
        Evado.Model.Digital.EdRecord newRecord = new Evado.Model.Digital.EdRecord ( );
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );

        if ( this.Session.RecordSelectionLayoutId == String.Empty )
        {
          this.LogMethodEnd ( "createObject" );
          return this.Session.LastPage;
        }
        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "FormRecords.createObject",
          this.Session.UserProfile );

        // 
        // Initialise the methods variables and objects.
        //    
        string LayoutId = PageCommand.GetParameter ( EdRecord.RecordFieldNames.Layout_Id );

        newRecord.Guid = Guid.NewGuid ( );
        newRecord.LayoutId = this.Session.RecordSelectionLayoutId;
        newRecord.RecordDate = DateTime.Now;

        // 
        // Create the record.
        // 
        this.Session.Record = this._Bll_FormRecords.createRecord ( newRecord );

        this.LogClass ( this._Bll_FormRecords.Log );

        if ( this.Session.Record.Guid == Guid.Empty )
        {
          this.ErrorMessage = EdLabels.Form_Record_Creation_Error_Message;

          this.LogError ( EvEventCodes.Database_Record_Retrieval_Error,
            this.ErrorMessage );

          return this.Session.LastPage;
        }


        this.LogDebug ( "CREATED Record Id: " + this.Session.Record.RecordId );

        // 
        // Generate the new page layout 
        // 
        this.getClientData ( clientDataObject );

        this.Session.RecordList = new List<EdRecord> ( );
        // 
        // Return the ResultData object.
        // 
        this.LogMethodEnd ( "createObject" );
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

      this.LogMethodEnd ( "createObject" );

      return this.Session.LastPage; ;

    }//END createObject method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class update record methods

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.Command object.</param>
    /// <returns>Evado.Model.UniForm.AppData</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData updateObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      try
      {
        this.LogMethod ( "updateObject" );
        this.LogValue ( "PageCommand: " + PageCommand.getAsString ( false, false ) );

        this.LogDebug ( "Record.Guid: " + this.Session.Record.Guid );
        this.LogDebug ( "Title: {0} ", this.Session.Record.Title );
        this.LogDebug ( "RecordId: {0} ", this.Session.Record.RecordId );
        this.LogDebug ( "FormAccessRole: {0} ", this.Session.Record.FormAccessRole );

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "updateObject",
          this.Session.UserProfile );

        string stSaveAction = PageCommand.GetParameter ( Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION );
        this.LogValue ( " Save Action: " + stSaveAction );

        //
        // Rung the server script if server side scripts are enabled.
        //
        this.runServerScript ( EvServerPageScript.ScripEventTypes.OnUpdate );

        // 
        // Update the object.
        // 
        if ( this.Session.PageId == EvPageIds.Record_Admin_Page )
        {
          this.updateObject_AdminValues ( PageCommand );
        }
        else
        {
          this.updateObjectValues ( PageCommand );
        }

        // 
        // Get the save action message value.
        // 
        this.Session.Record.SaveAction =
           Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue<EdRecord.SaveActionCodes> ( stSaveAction );

        this.LogDebug ( "Command Save Action: " + this.Session.Record.SaveAction );

        // 
        // Resets the save action to the parameter passed in the page groupCommand.
        // 
        switch ( this.Session.Record.SaveAction )
        {
          case EdRecord.SaveActionCodes.Save:
            {
              this.Session.Record.SaveAction = EdRecord.SaveActionCodes.Save_Record;
              break;
            }

          // 
          // Check that all mandatory fields have value if the user hits the submitted groupCommand.
          // 
          case EdRecord.SaveActionCodes.Submit_Record:
            {
              if ( this.validateRecordDataEntry ( ) == false )
              {
                this.ErrorMessage = EdLabels.Form_Record_Mandatory_Value_Error_Message;

                this.Session.Record.SaveAction = EdRecord.SaveActionCodes.Save;
              }
              break;
            }
        }
        this.LogValue ( "Actual Action: " + this.Session.Record.SaveAction );

        // 
        // Execute the save record groupCommand to save the record values to the 
        // Evado database.
        // 
        EvEventCodes result = this._Bll_FormRecords.saveRecord (
          this.Session.Record,
          this.Session.UserProfile.hasEndUserRole( this.Session.Record.Design.ReadAccessRoles ) );

        this.LogClass ( this._Bll_FormRecords.Log );

        // 
        // If an error state is returned log the event.
        // 
        if ( result != EvEventCodes.Ok )
        {
          string stEvent = this._Bll_FormRecords.Log + " returned error message: "
            + result + " = " + Evado.Model.Digital.EvcStatics.getEventMessage ( result );

          this.LogValue ( stEvent );

          this.LogError ( EvEventCodes.Database_Record_Update_Error, stEvent );

          this.ErrorMessage = EdLabels.Record_Update_Error_Message;

          if ( this.DebugOn == true )
          {
            this.ErrorMessage += EdLabels.Space_Hypen
              + Evado.Model.Digital.EvcStatics.getEventMessage ( result );
          }

          this.LogMethodEnd ( "updateObject" );
          return this.Session.LastPage;
        }

        //
        // Force a refresh of hte form record list.
        //
        this.Session.RecordList = new List<EdRecord> ( );

        this.LogMethodEnd ( "updateObject" );
        return new Model.UniForm.AppData ( );

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
      return this.Session.LastPage;

    }//END updateObject method

    // ==================================================================================
    /// <summary>
    /// THis method updates the form record values with the groupCommand parameter values.
    /// </summary>
    /// <param name="PageCommand"> Evado.Model.UniForm.Command object.</param>
    //  ----------------------------------------------------------------------------------
    private void updateObject_AdminValues (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "updateObject_AdminValues" );
      this.LogValue ( " Parameters.Count: " + PageCommand.Parameters.Count );

      String stDate = PageCommand.GetParameter ( EdRecord.RecordFieldNames.RecordDate );
      this.Session.Record.RecordDate = Evado.Model.Digital.EvcStatics.getDateTime ( stDate );

      String stState = PageCommand.GetParameter ( EdRecord.RecordFieldNames.Status );
      this.Session.Record.State = Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue<EdRecordObjectStates> ( stState );

    }//END updateObject_AdminValues method.

    // ==================================================================================
    /// <summary>
    /// THis method updates the form record values with the groupCommand parameter values.
    /// </summary>
    /// <param name="PageCommand"> Evado.Model.UniForm.Command object.</param>
    //  ----------------------------------------------------------------------------------
    private void updateObjectValues (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "updateObjectValue" );
      this.LogValue ( " Parameters.Count: " + PageCommand.Parameters.Count );
      // 
      // Initialise method variables and objects.
      // 
      EuRecordGenerator pageGenerator = new EuRecordGenerator (
        this.GlobalObjects,
        this.Session,
        this.ClassParameters );

      // 
      // Update the record values from the list of parameters
      // 
      pageGenerator.updateFormObject (
        PageCommand.Parameters,
        this.Session.Record );

      this.LogClass ( pageGenerator.Log );

      this.updateSignatures ( );

      this.LogMethodEnd ( "updateObjectValue" );
    }//END updateObjectValue method.

    // ==============================================================================
    /// <summary>
    /// This method creates a save groupCommand.
    /// </summary>
    /// <returns>Evado.Model.UniForm.Command object</returns>
    // ------------------------------------------------------------------------------
    private void updateSignatures ( )
    {
      this.LogMethod ( "updateSignatures" );

      //
      // Iterate through the record fields to process the signature values.
      //
      foreach ( EdRecordField field in this.Session.Record.Fields )
      {
        if ( field.TypeId == EvDataTypes.Signature
          && field.ItemText != String.Empty )
        {
          EvSignatureBlock signatureBlock = Newtonsoft.Json.JsonConvert.DeserializeObject<EvSignatureBlock> ( field.ItemText );

          //
          // if the signature raster array count is 0 then empty the signature field value.
          //
          if ( signatureBlock.Signature.Count == 0 )
          {
            field.ItemText = String.Empty;
          }
          else
          {
            if ( signatureBlock.Name == String.Empty )
            {
              // signatureBlock.Name = this.Session.Patient.PatientFullName;
            }
            signatureBlock.AcceptedBy = this.Session.UserProfile.CommonName;
            signatureBlock.DateStamp = DateTime.Now;

            field.ItemText = Newtonsoft.Json.JsonConvert.SerializeObject ( signatureBlock );
          }
        }
      }
      this.LogMethodEnd ( "updateSignatures" );
    }

    ///  =============================================================================== 
    /// <summary>
    /// This method checks that all of the mandatory field values have been entered, and 
    /// is executed prior to submitting the page as a completed record.
    /// </summary>
    /// <returns>True: All mandatory values have been entered, False: Not all mandatory values have been entered.</returns>
    //  ----------------------------------------------------------------------------------
    private bool validateRecordDataEntry ( )
    {
      this.LogMethod ( "checkMandatoryValuesEntered" );

      // 
      // Initialise the methods variables
      // 
      bool bValueEntered = true;

      // 
      // Iterate through the   Evado.Model.Digital.EvForm TestItemList to set their state
      // 
      foreach ( EdRecordField field in this.Session.Record.Fields )
      {
        this.LogValue ( "Field: " + field.FieldId
          + ", FT: " + field.TypeId
          + ", IV: " + field.ItemValue
          + ", IT: " + field.ItemText
          + ", M: " + field.Design.Mandatory );

        // 
        // Skip non mandatory fields
        // 
        if ( field.Design.Mandatory == false )
        {
          this.LogValue ( " >> Not Mandatory SKIP FIELD " );
          continue;
        }
        if ( field.Design.HideField == true )
        {
          this.LogValue ( " >> Hidden field SKIP " );
          continue;
        }

        // 
        // Skip non computed fields
        // 
        if ( field.TypeId == Evado.Model.EvDataTypes.Computed_Field )
        {
          this.LogValue ( " >> Computed Field SKIP FIELD " );
          continue;
        }

        // 
        // Process a table type
        // 
        if ( field.TypeId == Evado.Model.EvDataTypes.Table )
        {
          // 
          // Assume the table is empty.
          //  
          bool bTableEntered = false;

          // 
          // Iterate through the table.
          // 
          foreach ( EdRecordTableRow row in field.Table.Rows )
          {
            // 
            // If any sells have a value the set the cell ResultData to true.
            // 
            if ( row.Column [ 0 ] != String.Empty
              || row.Column [ 1 ] != String.Empty
              || row.Column [ 2 ] != String.Empty
              || row.Column [ 3 ] != String.Empty
              || row.Column [ 4 ] != String.Empty
              || row.Column [ 5 ] != String.Empty
              || row.Column [ 6 ] != String.Empty
              || row.Column [ 7 ] != String.Empty
              || row.Column [ 9 ] != String.Empty )
            {
              bTableEntered = true;
            }

          }//END iteration loop.

          // 
          // if there are not values then set the value entered to false.
          // 
          if ( bTableEntered == false )
          {
            bValueEntered = false;
          }
        }
        else if ( field.TypeId == Evado.Model.EvDataTypes.Special_Matrix )
        {
          // 
          // Assume table is empty
          // 
          bool bTableEntered = false;

          // 
          // Iterate through the table.
          // 
          foreach ( EdRecordTableRow row in field.Table.Rows )
          {
            // 
            // If any sells have a value the set the cell ResultData to true.
            // 
            if ( row.Column [ 1 ] != String.Empty
              || row.Column [ 2 ] != String.Empty
              || row.Column [ 3 ] != String.Empty
              || row.Column [ 4 ] != String.Empty
              || row.Column [ 5 ] != String.Empty
              || row.Column [ 6 ] != String.Empty
              || row.Column [ 7 ] != String.Empty
              || row.Column [ 9 ] != String.Empty )
            {
              bTableEntered = true;

            }//END TestReport for value.

          }//END table iteration.

          // 
          // if there are not values then set the value entered to false.
          // 
          if ( bTableEntered == false )
          {
            bValueEntered = false;
          }
        }
        else
        {
          // 
          // TestReport to see if the value exists.
          // 
          if ( field.ItemText == string.Empty
            && field.ItemValue == string.Empty )
          {
            bValueEntered = false;
          }

        }//END other fields.}

        this.LogValue ( " >> VE: " + bValueEntered );

      }//END FormRecord field iteration loop

      this.LogValue ( "Result is VE: " + bValueEntered );

      // 
      // If query is true then one TestReport letter was queried.
      // 
      if ( bValueEntered == true )
      {
        this.LogValue ( "ALL Mandatory fields have values." );
        return true;
      }

      this.LogValue ( "No ALL Mandatory fields have values." );
      return false;

    }// End checkMandatoryValuesEntered method.

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
  }
}//END namespace