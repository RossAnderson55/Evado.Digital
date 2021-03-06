﻿/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\FormRecords.cs" company="EVADO HOLDING PTY. LTD.">
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
      if ( this.Session.RecordLayout == null )
      {
        this.Session.RecordLayout = new EdRecord ( );
      }
      if ( this.Session.RecordList == null )
      {
        this.Session.RecordList = new List<EdRecord> ( );
      }
      if ( this.Session.Selected_EntityLayoutId == null )
      {
        this.Session.Selected_EntityLayoutId = String.Empty;
      }
      this.ClassNameSpace = "Evado.UniForm.Digital.EuRecords.";
    }

    /// <summary>
    /// This method initialises the class and passs in the user profile.
    /// </summary>
    public EuRecords (
      EuGlobalObjects AdapterObjects,
      EvUserProfileBase ServiceUserProfile,
      EuSession SessionObjects,
      String UniForm_BinaryFilePath,
      String UniForm_BinaryServiceUrl,
      EvClassParameters ClassParameters )
    {
      this.ClassNameSpace = "Evado.UniForm.Digital.EuRecords.";
      this.AdapterObjects = AdapterObjects;
      this.ServiceUserProfile = ServiceUserProfile;
      this.Session = SessionObjects;
      this.UniForm_BinaryFilePath = UniForm_BinaryFilePath;
      this.UniForm_BinaryServiceUrl = UniForm_BinaryServiceUrl;
      this.ClassParameters = ClassParameters;

      this.LogInitMethod ( "EuRecords initialisation" );
      this.LogInit ( "ServiceUserProfile.UserId: " + ServiceUserProfile.UserId );
      this.LogInit ( "SessionObjects.UserProfile.Userid: " + this.Session.UserProfile.UserId );
      this.LogInit ( "SessionObjects.UserProfile.CommonName: " + this.Session.UserProfile.CommonName );
      this.LogInit ( "UniForm BinaryFilePath: " + this.UniForm_BinaryFilePath );
      this.LogInit ( "UniForm BinaryServiceUrl: " + this.UniForm_BinaryServiceUrl );

      this.LogInit ( "Settings" );
      this.LogInit ( "-LoggingLevel: " + this.ClassParameters.LoggingLevel );
      this.LogInit ( "-UserId: " + this.ClassParameters.UserProfile.UserId );
      this.LogInit ( "-UserCommonName: " + this.ClassParameters.UserProfile.CommonName );

      this._Bll_Records = new EdRecords ( ClassParameters );
      this.EnableEntityEditButtonUpdate = this.AdapterObjects.Settings.EnableEntityEditButtonUpdate;
      this.EnableEntitySaveButtonUpdate = this.AdapterObjects.Settings.EnableEntitySaveButtonUpdate;


      if ( this.Session.Record == null )
      {
        this.Session.Record = new EdRecord ( );
      }
      if ( this.Session.RecordLayout == null )
      {
        this.Session.RecordLayout = new EdRecord ( );
      }
      if ( this.Session.RecordList == null )
      {
        this.Session.RecordList = new List<EdRecord> ( );
      }
      if ( this.Session.Selected_EntityLayoutId == null )
      {
        this.Session.Selected_EntityLayoutId = String.Empty;
      }
      if ( this.Session.SelectedCountry == null )
      {
        this.Session.SelectedCountry = String.Empty;
      }
      if ( this.Session.SelectedCity == null )
      {
        this.Session.SelectedCity = String.Empty;
      }
      if ( this.Session.SelectedPostCode == null )
      {
        this.Session.SelectedPostCode = String.Empty;
      }
      if ( this.Session.SelectedUserCategory == null )
      {
        this.Session.SelectedUserCategory = String.Empty;
      }
      if ( this.Session.SelectedUserType == null )
      {
        this.Session.SelectedUserType = String.Empty;
      }



    }//END Method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class constants and variables.

    private Evado.Digital.Bll.EdRecords _Bll_Records = new Evado.Digital.Bll.EdRecords ( );
    private EvServerPageScript _ServerPageScript = new EvServerPageScript ( );

    bool _HideSelectionGroup = false;

    bool EnableEntitySaveButtonUpdate = false;
    bool EnableEntityEditButtonUpdate = false;

    Guid ParentGuid = Guid.Empty;
    String ParentLayoutId = String.Empty;

    private EdRecord queryLayout = null;
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
    public const string CONST_HIDE_SELECTION = "HSFID";
    /// <summary>
    /// This constand definee the include test sites property identifier
    /// </summary>
    public const string CONST_SELECTION_FIELD = "SELFID";

    /// <summary>
    /// This constand definee command field to indicated if empty selectin is enabled.
    /// </summary>
    public const string CONST_EMPTY_SELECTION_FIELD = "EMSEL";
    /// <summary>
    /// This constand definee the include test sites property identifier
    /// </summary>
    public const string CONST_EDIT_MODE_FIELD = "EM01";

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


    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class public methods

    // ===============================================================================
    /// <summary>
    /// This method gets the application object from the list.
    /// </summary>
    /// <param name="PageCommand">ClientPateEvado.UniForm.Model.Command object</param>
    /// <returns>Evado.UniForm.Model.AppData</returns>
    //  ----------------------------------------------------------------------------------
    override public Evado.UniForm.Model.AppData getDataObject (
      Evado.UniForm.Model.Command PageCommand )
    {
      this.LogMethod ( "getDataObject" );
      this.LogValue ( "PageCommand: " + PageCommand.getAsString ( false, true ) );
      this.LogDebug ( "EnableEntityEditButtonUpdate: " + this.EnableEntityEditButtonUpdate );
      this.LogDebug ( "EnableEntitySaveButtonUpdate: " + this.EnableEntitySaveButtonUpdate );
      this.LogDebug ( "ButtonEditModeEnabled: " + this.Session.Record.ButtonEditModeEnabled );
      this.LogDebug ( "RefreshEntityChildren: " + this.Session.RefreshEntityChildren );

      try
      {
        // 
        // Initialise the methods variables and objects.
        // 
        Evado.UniForm.Model.AppData clientDataObject = new Evado.UniForm.Model.AppData ( );

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
          case Evado.UniForm.Model.ApplicationMethods.List_of_Objects:
            {
              this.LogDebug ( "Get List of object method" );

              switch ( this.Session.StaticPageId )
              {
                case Evado.Digital.Model.EdStaticPageIds.Record_Export_Page:
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
          case Evado.UniForm.Model.ApplicationMethods.Get_Object:
            {
              this.LogDebug ( "Get Object method" );

              clientDataObject = this.getObject ( PageCommand );

              break;

            }//END get object case

          // 
          // Select the groupCommand to create a new record object.
          // 
          case Evado.UniForm.Model.ApplicationMethods.Create_Object:
            {
              this.LogDebug ( "Create Object method" );

              clientDataObject = this.createObject ( PageCommand );

              break;
            }//END create case

          // 
          // Select the method to update the record object.
          // 
          case Evado.UniForm.Model.ApplicationMethods.Save_Object:
            {
              this.LogDebug ( "Save Object method" );

              // 
              // Update the object values
              // 
              clientDataObject = this.updateObject ( PageCommand );

              break;

            }//END save case.

          case Evado.UniForm.Model.ApplicationMethods.Delete_Object:
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

    // ===============================================================================
    /// <summary>
    /// This method generates a component group output.
    /// </summary>
    /// <param name="PageObject">Evado.Model.Uniform.Page object .</param>
    /// <param name="PageCommand">Evado.UniForm.Model.Command object</param>
    /// <param name="Component">String: component identifier</param>
    /// <returns>EvEventCodes enumeration</returns>
    //  ----------------------------------------------------------------------------------
    public EvEventCodes getPageComponent (
      Evado.UniForm.Model.Page PageObject,
      Evado.UniForm.Model.Command PageCommand,
      String Component )
    {
      this.LogMethod ( "getPageComponent" );
      this.LogValue ( "PageCommand: " + PageCommand.getAsString ( false, true ) );
      this.LogValue ( "Component: " + Component );
      this.LogValue ( "EnableEntityEditButtonUpdate: " + this.EnableEntityEditButtonUpdate );
      this.LogValue ( "EnableEntitySaveButtonUpdate: " + this.EnableEntitySaveButtonUpdate );
      //
      // initialise the methods variables and objects.
      //
      if ( this.Session.RecordLayout == null )
      {
        this.Session.RecordLayout = new EdRecord ( );
      }

      try
      {
        //
        // UPdate the sessin variables.
        //
        this.updateSessionValue ( PageCommand );

        string pageId = PageCommand.GetPageId ( );

        if ( Component != String.Empty )
        {
          pageId = Component;
        }
        this.LogDebug ( "Page identifier {0}.", pageId );

        //
        // get the entity list for the layout identifier.
        //
        if ( pageId.Contains ( EuAdapter.CONST_ENTITY_LIST_PREFIX ) == true )
        {
          this.LogDebug ( "Page Entity List selected" );
          var layoutId = pageId.Replace ( EuAdapter.CONST_ENTITY_LIST_PREFIX, String.Empty );

          this._HideSelectionGroup = true;

          this.Session.RecordLayout = this.AdapterObjects.GetRecordLayout ( layoutId );
          this.LogDebug ( "Layout {0} - {1}", this.Session.RecordLayout.LayoutId, this.Session.RecordLayout.Title );

          return this.getListObject ( PageObject, layoutId );
        }

        //
        // get the layout 
        //
        if ( pageId.Contains ( EuAdapter.CONST_ENTITY_PREFIX ) == true )
        {
          this.LogDebug ( "Page Entity selected" );
          var layoutId = pageId.Replace ( EuAdapter.CONST_ENTITY_PREFIX, String.Empty );

          return this.getObject ( PageObject, PageCommand );
        }

      }
      catch ( Exception Ex )
      {
        // 
        // If an exception is created log the error and return an error.
        // 
        this.LogException ( Ex );
      }

      return EvEventCodes.Page_Loading_General_Error;

    }//END getRecordObject methods

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Page Component methods.
    // ==============================================================================
    /// <summary>
    /// This method generates an enity list page groups.
    /// </summary>
    /// <param name="PageObject">Evado.Model.Uniform.Page object .</param>
    /// <param name="LayoutId">String: optional layout identifier.</param>
    /// <returns>EvEventCodes enumeration indicating the execution outcome.</returns>
    //  -----------------------------------------------------------------------------
    private EvEventCodes getListObject (
      Evado.UniForm.Model.Page PageObject,
      String LayoutId )
    {
      this.LogMethod ( "getListObject" );
      this.LogValue ( "LayoutId: " + LayoutId );
      this.LogDebug ( "EntitySelectionLayoutId: " + this.Session.Selected_EntityLayoutId );
      try
      {
        if ( LayoutId == String.Empty )
        {
          LayoutId = this.Session.Selected_EntityLayoutId;
        }

        //
        // get the selected entity.
        //
        this.Session.RecordLayout = this.AdapterObjects.GetRecordLayout ( LayoutId );
        this.LogDebug ( "Entity.ReadAccessRoles: " + this.Session.RecordLayout.Design.ReadAccessRoles );
        this.LogDebug ( "Entity.EditAccessRoles: " + this.Session.RecordLayout.Design.EditAccessRoles );
        this.LogDebug ( "Entity.LinkContentSetting: " + this.Session.RecordLayout.Design.LinkContentSetting );

        // 
        // If the user does not have monitor or ResultData manager roles exit the page.
        // 
        if ( this.Session.RecordLayout.hasReadAccess ( this.Session.UserProfile.Roles ) == false )
        {
          this.LogIllegalAccess (
            this.ClassNameSpace + "getListObject",
            this.Session.UserProfile );

          this.ErrorMessage = EdLabels.Record_Access_Error_Message;

          return EvEventCodes.User_Access_Error;
        }

        // 
        // Log the user's access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "getListObject",
          this.Session.UserProfile );

        //
        // Update the layoutId if passed as a parameter.
        //
        if ( this.Session.Selected_EntityLayoutId != LayoutId
          && LayoutId != String.Empty )
        {
          this.Session.Selected_EntityLayoutId = LayoutId;
        }

        //
        // Execute the monitor list record query.
        //
        this.executeRecordQuery ( );

        // 
        // Create the new pageMenuGroup for query selection.
        // 
        this.getList_SelectionGroup ( PageObject );

        // 
        // Create the pageMenuGroup containing commands to open the records.
        //         
        this.getEntity_ListGroup ( PageObject );


        return EvEventCodes.Ok;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.Entity_View_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return EvEventCodes.Page_Loading_General_Error;

    }//END getListObject method.

    // ==============================================================================
    /// <summary>
    /// This method generates an entity data page.
    /// </summary>
    /// <param name="PageObject">Evado.Model.Uniform.Page object .</param>
    /// <param name="PageCommand">Evado.UniForm.Model.Command object.</param>
    /// <returns>EvEventCodes enumeration indicating the execution outcome.</returns>
    //  ------------------------------------------------------------------------------
    private EvEventCodes getObject (
      Evado.UniForm.Model.Page PageObject,
      Evado.UniForm.Model.Command PageCommand )
    {
      this.LogMethod ( "getObject" );
      try
      {
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
          return EvEventCodes.Database_Record_Retrieval_Error;
        }

        this.LogValue ( "Entity.RecordId: " + this.Session.Record.RecordId );

        // 
        // If the user does not have monitor or ResultData manager roles exit the page.
        // 
        if ( this.Session.Record.hasReadAccess ( this.Session.UserProfile.Roles ) == false )
        {
          this.LogIllegalAccess (
            this.ClassNameSpace + "getObject",
            this.Session.UserProfile );

          this.ErrorMessage = EdLabels.Record_Access_Error_Message;

          return EvEventCodes.User_Access_Error;
        }

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "getObject",
          this.Session.UserProfile );

        //
        // Rung the server script if server side scripts are enabled.
        //
        this.runServerScript ( EvServerPageScript.ScripEventTypes.OnOpen );

        // 
        // Generate the client ResultData object for the UniForm client.
        // 
        this.getEntityClientData ( PageObject );

        // 
        // Return the client ResultData object to the calling method.
        // 
        this.LogMethodEnd ( "getObject" );
        return EvEventCodes.Ok;
      }
      catch ( Exception Ex )
      {
        // 
        // On an exception raised create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.Entity_Retrieve_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }


      this.LogMethodEnd ( "getObject" );
      return EvEventCodes.Page_Loading_General_Error;

    }//END getObject method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class record locking and unlocking methods.

    // ==============================================================================
    /// <summary>
    /// This method checks the unlock status of the record.
    /// </summary>
    /// <param name="PageObject">Evado.UniForm.Model.Page object</param>
    /// <returns>True:  lock successful</returns>
    //  ------------------------------------------------------------------------------
    private bool checkRecordLockStatus (
      Evado.UniForm.Model.Page PageObject )
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
          Evado.UniForm.Model.Command pageCommand = PageObject.addCommand (
            EdLabels.Form_UnLock_Command_Title,
            EuAdapter.ADAPTER_ID,
            EuAdapterClasses.Records.ToString ( ),
             Evado.UniForm.Model.ApplicationMethods.Save_Object );

          pageCommand.SetGuid ( this.Session.Record.Guid );
        }

        return true;
      }

      //DEBUG

      //
      // Do not lock the record is the user does not have update access.
      //
      if ( this.Session.RecordLayout.hasReadAccess ( this.Session.UserProfile.Roles ) == false )
      {
        return false;
      }

      //
      // Execute the record lock method.
      //
      EvEventCodes iReturn = this._Bll_Records.lockItem ( this.Session.Record );

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
      EvEventCodes iReturn = this._Bll_Records.unlockItem ( this.Session.Record );

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
    public Evado.UniForm.Model.AppData unLockRecord_Admin ( )
    {
      // 
      // Test if the record is already locked.
      // 
      if ( this.Session.UserProfile.hasManagementAccess )
      {
        return new Evado.UniForm.Model.AppData ( );
      }

      // 
      // Execute the unlock method to the database.
      // 
      EvEventCodes iReturn = this._Bll_Records.unlockItem ( this.Session.Record );

      if ( iReturn != EvEventCodes.Ok )
      {
        this.ErrorMessage = EdLabels.Form_Record_Admin_Unlock_Error_Message;
        return this.Session.LastPage;
      }

      return new Evado.UniForm.Model.AppData ( );

    }//END unLockRecord_Admin method

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class common private methods.

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.Command object.</param>
    // ------------------------------------------------------------------------------
    private void updateSessionValue (
      Evado.UniForm.Model.Command PageCommand )
    {
      this.LogMethod ( "updateSessionValue" );

      //
      // If it is a custom method then empty the Entity list.
      //
      if ( PageCommand.hasParameter ( Evado.UniForm.Model.CommandParameters.Custom_Method ) == true )
      {
        this.Session.RecordList = new List<EdRecord> ( );
      }

      //
      // if the entity layout is defined in the page command then update its value.
      //
      if ( PageCommand.hasParameter ( EuRecords.CONST_HIDE_SELECTION ) == true )
      {
        this._HideSelectionGroup = true;
      }
      this.LogValue ( "HideSelectionGroup: " + this._HideSelectionGroup );

      //
      // if the entity layout is defined in the page command then update its value.
      //
      if ( PageCommand.hasParameter ( EuRecords.CONST_EDIT_MODE_FIELD ) == true )
      {
        this.Session.Record.ButtonEditModeEnabled = true;
      }
      this.LogValue ( "ButtonEditModeEnabled: " + this.Session.Record.ButtonEditModeEnabled );

      //
      // enable empty selection query.
      //
      if ( PageCommand.hasParameter ( EdRecord.FieldNames.ParentLayoutId ) == true )
      {
        var value = PageCommand.GetParameter ( EuRecords.CONST_EMPTY_SELECTION_FIELD );

        if ( EvStatics.getBool ( value ) == true )
        {
          this.Session.EnableEmptyQuerySelection = true;
        }
        else
        {
          this.Session.EnableEmptyQuerySelection = false;
        }
      }
      this.LogValue ( "EnableEmptyQuerySelection: " + this.Session.EnableEmptyQuerySelection );

      //
      // if the entity layout is defined in the page command then update its value.
      //
      if ( PageCommand.hasParameter ( EdRecord.FieldNames.Layout_Id.ToString ( ) ) == true )
      {
        this.Session.Selected_EntityLayoutId = PageCommand.GetParameter ( EdRecord.FieldNames.Layout_Id.ToString ( ) );
      }
      this.LogValue ( "Entity_SelectedLayoutId: " + this.Session.Selected_EntityLayoutId );

      if ( this.Session.Selected_EntityLayoutId != String.Empty )
      {
        this.Session.RecordLayout = this.AdapterObjects.GetRecordLayout ( this.Session.Selected_EntityLayoutId );

        this.LogValue ( "EntityLayout.LayoutId: " + this.Session.RecordLayout.LayoutId );
      }

      //
      // if the entity record type is defined in the page command then update its value.
      //
      if ( PageCommand.hasParameter ( EdRecord.FieldNames.ParentUserId ) == true )
      {
        var parentUserId = PageCommand.GetParameter ( EdRecord.FieldNames.ParentUserId );

        if ( parentUserId != String.Empty )
        {
          this.Session.SelectedUserId = parentUserId;
        }
      }
      this.LogValue ( "SelectedUserId: " + this.Session.SelectedUserId );

      //
      // if the entity record type is defined in the page command then update its value.
      //
      if ( PageCommand.hasParameter ( EdRecord.FieldNames.ParentOrgId ) == true )
      {
        var parentOrgId = PageCommand.GetParameter ( EdRecord.FieldNames.ParentOrgId );

        if ( parentOrgId != String.Empty )
        {
          this.Session.SelectedOrgId = parentOrgId;
        }
      }
      this.LogValue ( "SelectedOrgId: " + this.Session.SelectedOrgId );

      //
      // if the entity record type is defined in the page command then update its value.
      //
      if ( PageCommand.hasParameter ( EdRecord.CONST_RECORD_TYPE ) == true )
      {
        var recordType = PageCommand.GetParameter<EdRecordTypes> ( EdRecord.CONST_RECORD_TYPE );

        if ( this.Session.Selected_RecordType != recordType )
        {
          this.Session.Selected_RecordType = recordType;
          this.AdapterObjects.AllEntityLayouts = new List<EdRecord> ( );
        }
      }
      this.LogValue ( "EntityTypeSelection: " + this.Session.Selected_RecordType );


      //
      // if the entity record status is defined in the page command then update its value.
      //
      if ( PageCommand.hasParameter ( EdRecord.FieldNames.Status.ToString ( ) ) == true )
      {
        var stateValue = PageCommand.GetParameter<EdRecordObjectStates> ( EdRecord.FieldNames.Status.ToString ( ) );

        if ( this.Session.Selected_RecordState != stateValue )
        {
          if ( stateValue != EdRecordObjectStates.Null )
          {
            this.AdapterObjects.AllEntityLayouts = new List<EdRecord> ( );
            this.Session.Selected_RecordState = stateValue;
          }
          else
          {
            this.AdapterObjects.AllEntityLayouts = new List<EdRecord> ( );
            this.Session.Selected_RecordState = EdRecordObjectStates.Null;
          }
        }
      }
      this.LogValue ( "EntityStateSelection: " + this.Session.Selected_RecordState );

      //
      // if the selected organisation country exists updated its value.
      //
      if ( PageCommand.hasParameter ( EdOrganisation.FieldNames.Address_Country ) == true )
      {
        this.Session.SelectedCountry = PageCommand.GetParameter ( EdOrganisation.FieldNames.Address_Country );
      }
      this.LogValue ( "SelectedOrganisationCountry: " + this.Session.SelectedCountry );

      //
      // if the selected organisation city exists updated its value.
      //
      if ( PageCommand.hasParameter ( EdOrganisation.FieldNames.Address_City ) == true )
      {
        this.Session.SelectedCity = PageCommand.GetParameter ( EdOrganisation.FieldNames.Address_City );
      }
      this.LogValue ( "SelectedOrganisationCity: " + this.Session.SelectedCity );

      //
      // if the selected organisation city exists updated its value.
      //
      if ( PageCommand.hasParameter ( EdOrganisation.FieldNames.Address_Post_Code ) == true )
      {
        this.Session.SelectedPostCode = PageCommand.GetParameter ( EdOrganisation.FieldNames.Address_Post_Code );
      }
      this.LogValue ( "SelectedOrganisationPostCode: " + this.Session.SelectedPostCode );

      // 
      // Set the page type to control the DB query type.
      // 
      string pageId = PageCommand.GetPageId ( );

      this.LogValue ( "PageCommand pageId: " + pageId );
      //
      // if the page id is defined in the page command then update its value.
      //
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
         Evado.Model.EvStatics.enumValueToString ( EdRecordTypes.Normal_Record ) );

      recordTypes.Add ( option );
      option = new EvOption ( EdRecordTypes.Questionnaire.ToString ( ),
         Evado.Model.EvStatics.enumValueToString ( EdRecordTypes.Questionnaire ) );

      recordTypes.Add ( option );
      option = new EvOption ( EdRecordTypes.Updatable_Record.ToString ( ),
         Evado.Model.EvStatics.enumValueToString ( EdRecordTypes.Updatable_Record ) );

      return recordTypes;
    }

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class record list methods

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.Command object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  -----------------------------------------------------------------------------
    private Evado.UniForm.Model.AppData getListObject (
      Evado.UniForm.Model.Command PageCommand )
    {
      this.LogMethod ( "getListObject" );
      this.LogValue ( "Selected_EntityLayoutId: " + this.Session.Selected_EntityLayoutId );
      //this.LogValue ( "EntitySelectionState: " + this.Session.Selected_RecordState );
      //this.LogValue ( "EntitySelectionLayoutId: " + this.Session.Selected_EntityLayoutId );
      //this.LogValue ( "EntityLayout.ReadAccessRoles: " + this.Session.RecordLayout.Design.ReadAccessRoles );
      //this.LogValue ( "UserProfile.Roles: " + this.Session.UserProfile.Roles );
      try
      {
        // 
        // Initialise the methods variables and objects.
        // 
        Evado.UniForm.Model.AppData clientDataObject = new Evado.UniForm.Model.AppData ( );

        //
        // get the selected entity.
        //
        this.Session.RecordLayout = this.AdapterObjects.GetRecordLayout ( this.Session.Selected_EntityLayoutId );

        //
        // Set the parent entity variables.
        //
        this.ParentLayoutId = this.Session.Record.LayoutId;
        this.ParentGuid = this.Session.Record.Guid;
        if ( PageCommand.hasParameter ( EdRecord.FieldNames.ParentGuid ) == true )
        {
          this.ParentGuid = PageCommand.GetParameterAsGuid ( EdRecord.FieldNames.ParentGuid );
        }
        if ( PageCommand.hasParameter ( EdRecord.FieldNames.ParentLayoutId ) == true )
        {
          this.ParentLayoutId = PageCommand.GetParameter ( EdRecord.FieldNames.ParentLayoutId );
        }

        // 
        // If the user does not have monitor or ResultData manager roles exit the page.
        // 
        if ( this.Session.RecordLayout.hasReadAccess ( this.Session.UserProfile.Roles ) == false )
        {
          this.LogIllegalAccess (
            this.ClassNameSpace + "getListObject",
            this.Session.UserProfile );

          this.ErrorMessage = EdLabels.Record_Access_Error_Message;

          return this.Session.LastPage; ;
        }
        // 
        // Log the user's access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "getListObject",
          this.Session.UserProfile );

        this.LogDebug ( "LayoutId {0}", this.Session.RecordLayout.LayoutId );

        //
        // Execute the monitor list record query.
        //
        this.executeRecordQuery ( );

        // 
        // Initialise the client ResultData object.
        // 
        clientDataObject.Id = Guid.NewGuid ( );
        clientDataObject.Page.Id = clientDataObject.Id;
        clientDataObject.Page.PageDataGuid = clientDataObject.Id;
        clientDataObject.Title = EdLabels.Entity_View_Page_Title;

        if ( this.AdapterObjects.Settings.UseHomePageHeaderOnAllPages == true )
        {
          clientDataObject.Title = this.AdapterObjects.Settings.HomePageHeaderText;
        }
        else
        {
          clientDataObject.Title = this.Session.RecordLayout.Title;
        }

        clientDataObject.Page.Title = clientDataObject.Title;
        clientDataObject.Page.PageId = Evado.Digital.Model.EdStaticPageIds.Records_View.ToString ( );

        if ( this.Session.Record.hasEditAccess ( this.Session.UserProfile.Roles ) == true )
        {
          clientDataObject.Page.EditAccess = Evado.UniForm.Model.EditAccess.Enabled;
        }
        this.LogValue ( "Page.EditAccess: " + clientDataObject.Page.EditAccess );

        // 
        // Create the new pageMenuGroup for query selection.
        // 
        this.getList_SelectionGroup ( clientDataObject.Page );

        // 
        // Create the pageMenuGroup containing commands to open the records.
        //         
        this.getEntity_ListGroup ( clientDataObject.Page );

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
    /// This method creates the record view pageMenuGroup containing a list of commands to 
    /// open the form record.
    /// </summary>
    /// <param name="PageObject">Evado.Model.Uniform.Page object to add the pageMenuGroup to.</param>
    //  ------------------------------------------------------------------------------
    private void getList_SelectionGroup (
      Evado.UniForm.Model.Page PageObject )
    {
      this.LogMethod ( "getList_SelectionGroup" );
      this.LogDebug ( "IssuedEntityLayouts.Count {0}. ", this.AdapterObjects.IssuedEntityLayouts.Count );
      //
      // Initialise the methods variables and objects.
      //
      Evado.UniForm.Model.Group pageGroup = new Evado.UniForm.Model.Group ( );
      List<EvOption> optionList;
      Evado.UniForm.Model.Field selectionField;

      if ( this._HideSelectionGroup == true )
      {
        this.LogMethodEnd ( "getList_SelectionGroup" );
        return;
      }

      // 
      // Create the new pageMenuGroup for record selection.
      // 
      pageGroup = PageObject.AddGroup (
        EdLabels.Entities_Selection_Group_Title,
        Evado.UniForm.Model.EditAccess.Enabled );
      pageGroup.GroupType = Evado.UniForm.Model.GroupTypes.Default;
      pageGroup.Layout = Evado.UniForm.Model.GroupLayouts.Full_Width;
      pageGroup.AddParameter ( Evado.UniForm.Model.GroupParameterList.Offline_Hide_Group, true );

      // 
      // Add the record state selection option
      //
      optionList = new List<EvOption> ( );
      optionList.Add ( new EvOption ( ) );
      foreach ( EdRecord layout in this.AdapterObjects.IssuedEntityLayouts )
      {
        optionList.Add ( new EvOption ( layout.LayoutId,
          String.Format ( "{0} - {1}", layout.LayoutId, layout.Title ) ) );
      }

      selectionField = pageGroup.createSelectionListField (
        EdRecord.FieldNames.Layout_Id,
        EdLabels.Label_Form_Id,
        this.Session.Selected_EntityLayoutId,
        optionList );

      selectionField.Layout = EuAdapter.DefaultFieldLayout;
      selectionField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Snd_Cmd_On_Change, 1 );

      // 
      // Add the record state selection option
      // 
      optionList = EdRecord.getRecordStates ( false );

      selectionField = pageGroup.createSelectionListField (
        EdRecord.FieldNames.Status.ToString ( ),
        EdLabels.Record_State_Selection,
        this.Session.Selected_RecordState,
        optionList );

      selectionField.Layout = EuAdapter.DefaultFieldLayout;
      selectionField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Snd_Cmd_On_Change, 1 );

      // 
      // Add the selection groupCommand
      // 
      Evado.UniForm.Model.Command selectionCommand = pageGroup.addCommand (
        EdLabels.Select_Records_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Records.ToString ( ),
        Evado.UniForm.Model.ApplicationMethods.Custom_Method );

      selectionCommand.setCustomMethod ( Evado.UniForm.Model.ApplicationMethods.List_of_Objects );

    }//ENd getList_SelectionGroup method

    // ==============================================================================
    /// <summary>
    /// This method executed the form record query of the database.
    /// </summary>
    /// <param name="ParentGuid">Guid: parent guid</param>
    /// <remarks>
    /// This method returns a list of forms based on the selection type of form record.
    /// </remarks>
    //  ------------------------------------------------------------------------------
    private void executeRecordQuery ( )
    {
      this.LogMethod ( "executeRecordQuery" );
      this.LogDebug ( "EntityLayoutIdSelection: " + this.Session.Selected_EntityLayoutId );
      //this.LogDebug ( "EntityTypeSelection: " + this.Session.Selected_RecordType );
      //this.LogDebug ( "EntityStateSelection: " + this.Session.Selected_RecordState );
      //this.LogDebug ( "parentGuid: {0}.", this.ParentGuid );
      //
      // Initialise the methods variables and objects.
      //
      EdQueryParameters queryParameters = new EdQueryParameters ( );


      // 
      // Initialise the query values to the currently selected objects identifiers.
      // 
      queryParameters.Type = this.Session.Selected_RecordType;
      queryParameters.LayoutId = this.Session.Selected_EntityLayoutId;

      // 
      // Initialise the query state selection.
      // 
      queryParameters.States.Add ( EuAdapter.CONST_RECORD_STATE_SELECTION_DEFAULT );
      queryParameters.NotSelectedState = true;

      if ( this.Session.Selected_RecordState != EdRecordObjectStates.Null )
      {
        queryParameters.States.Add ( this.Session.Selected_RecordState );
        queryParameters.NotSelectedState = false;
      }

      //
      // set the parent object selection criteria.
      //
      queryParameters.ParentType = this.Session.RecordLayout.Design.ParentType;

      if ( this.Session.RecordLayout.Design.ParentType == EdRecord.ParentTypeList.Organisation )
      {
        queryParameters.ParentOrgId = this.Session.SelectedOrgId;
      }

      if ( this.Session.RecordLayout.Design.ParentType == EdRecord.ParentTypeList.User )
      {
        queryParameters.ParentUserId = this.Session.SelectedUserId;
      }

      if ( this.Session.RecordLayout.Design.ParentType == EdRecord.ParentTypeList.Entity )
      {
        queryParameters.ParentGuid = this.Session.Record.Guid;

        if ( this.ParentGuid != Guid.Empty )
        {
          queryParameters.ParentGuid = ParentGuid;
        }
      }

      //
      // Set the filter critier.
      //
      queryParameters.Org_City = this.Session.SelectedCity;
      queryParameters.Org_Country = this.Session.SelectedCountry;
      queryParameters.Org_PostCode = this.Session.SelectedPostCode;

      if ( queryParameters.Org_City != String.Empty
        || queryParameters.Org_Country != String.Empty
        || queryParameters.Org_PostCode != String.Empty )
      {
        queryParameters.EnableOrganisationFilter = true;
      }
      /*
      this.LogDebug ( "Selected LayoutId: '" + queryParameters.LayoutId + "'" );
      this.LogDebug ( "Selected ParentType: '" + queryParameters.ParentType + "'" );
      this.LogDebug ( "Selected ParentGuid: '" + queryParameters.ParentGuid + "'" );
      this.LogDebug ( "Selected Org_City: '" + queryParameters.Org_City + "'" );
      this.LogDebug ( "Selected Org_Country: '" + queryParameters.Org_Country + "'" );
      this.LogDebug ( "Selected Org_PostCode: '" + queryParameters.Org_PostCode + "'" );
      */

      // 
      // Query the database to retrieve a list of the records matching the query parameter values.
      // 
      if ( queryParameters.LayoutId != String.Empty )
      {
        this.LogDebug ( "Querying form records" );
        this.Session.RecordList = this._Bll_Records.getRecordList ( queryParameters );

        this.LogDebugClass ( this._Bll_Records.Log );
      }
      this.LogDebug ( "EntityList.Count: " + this.Session.RecordList.Count );

      this.LogMethodEnd ( "executeRecordQuery" );

    }//END executeRecordQuery method.

    // ==============================================================================
    /// <summary>
    /// This method creates the record view pageMenuGroup containing a list of commands to 
    /// open the form record.
    /// </summary>
    /// <param name="PageObject">Evado.Model.Uniform.Page object to add the pageMenuGroup to.</param>
    /// <param name="RecordList">List of EvForm: form record objects.</param>
    //  ------------------------------------------------------------------------------
    private void getEntity_ListGroup (
      Evado.UniForm.Model.Page PageObject )
    {
      this.LogMethod ( "getEntity_ListGroup" );
      this.LogDebug ( "PageObject.EditAccess {0}.", PageObject.EditAccess );
      this.LogDebug ( "this.Session.RecordLayout.Title {0}.", this.Session.RecordLayout.Title );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.Group pageGroup = new Evado.UniForm.Model.Group ( );
      Evado.UniForm.Model.Command groupCommand = new Evado.UniForm.Model.Command ( );

      // 
      // Create the record display pageMenuGroup.
      // 
      pageGroup = PageObject.AddGroup (
        EdLabels.Entity_List_Group_Title );
      pageGroup.CmdLayout = Evado.UniForm.Model.GroupCommandListLayouts.Vertical_Orientation;

      pageGroup.Layout = Evado.UniForm.Model.GroupLayouts.Full_Width;

      if ( this.Session.RecordLayout.Title != String.Empty )
      {
        pageGroup.Title = String.Format (
          EdLabels.Entity_List_Title_Group_Title,
          this.Session.RecordLayout.Title );
      }

      if ( this.Session.RecordList.Count > 0 )
      {
        pageGroup.Title += EdLabels.List_Count_Label + this.Session.RecordList.Count;
      }

      //
      // Add a create record command.
      //
      if ( this.Session.Selected_EntityLayoutId != String.Empty
        && pageGroup.EditAccess == Evado.UniForm.Model.EditAccess.Enabled
        && ( this.Session.RecordLayout.Design.ParentEntities.Contains ( this.ParentLayoutId ) == true
          || this.Session.RecordLayout.Design.ParentEntities == String.Empty ) )
      {
        groupCommand = pageGroup.addCommand (
          String.Format ( EdLabels.Entity_Create_New_List_Command_Title, this.Session.RecordLayout.Title ),
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Records,
          Evado.UniForm.Model.ApplicationMethods.Create_Object );

        groupCommand.SetBackgroundDefaultColour ( Evado.UniForm.Model.Background_Colours.Purple );

        groupCommand.AddParameter ( Evado.Digital.Model.EdRecord.FieldNames.Layout_Id,
        this.Session.Selected_EntityLayoutId );
        groupCommand.AddParameter ( EdRecord.FieldNames.ParentGuid, this.Session.Record.Guid );
        groupCommand.SetGuid ( this.Session.Record.Guid );
      }

      this.LogDebug ( "EntityList.Count: " + this.Session.RecordList.Count );
      // 
      // Iterate through the record list generating a groupCommand to access each record
      // then append the groupCommand to the record pageMenuGroup view's groupCommand list.
      // 
      foreach ( Evado.Digital.Model.EdRecord entity in this.Session.RecordList )
      {
        this.LogDebug ( "LCD {0}, LT {1}, FC {2}", entity.Design.LinkContentSetting, entity.CommandTitle, entity.Fields.Count );

        //
        // Create the group list groupCommand object.
        //
        this.getGroupListCommand (
          entity,
          pageGroup,
         this.Session.RecordLayout.Design.LinkContentSetting );

      }//END iteration loop

      this.LogValue ( "Group command count: " + pageGroup.CommandList.Count );

      this.LogMethodEnd ( "getRecord_ListGroup" );
    }//END createViewCommandList method

    // ==============================================================================
    /// <summary>
    /// This method appends the milestone groupCommand to the page milestone list pageMenuGroup
    /// </summary>
    /// <param name="CommandEntity">EvForm object</param>
    /// <param name="PageGroup"> Evado.UniForm.Model.Group</param>
    //  -----------------------------------------------------------------------------
    private Evado.UniForm.Model.Command getGroupListCommand (
      EdRecord CommandEntity,
      Evado.UniForm.Model.Group PageGroup,
      EdRecord.LinkContentSetting ParentLinkSetting )
    {
      this.LogMethod ( "getGroupListCommand" );
      this.LogDebug ( "CommandEntity.RecordId: " + CommandEntity.RecordId );
      this.LogDebug ( "LinkContentSetting: " + CommandEntity.Design.LinkContentSetting );
      this.LogDebug ( "TypeId: " + CommandEntity.TypeId );
      this.LogDebug ( "ParentLinkSetting: " + ParentLinkSetting );

      //
      // Set the link setting.
      //
      if ( CommandEntity.Design.LinkContentSetting == EdRecord.LinkContentSetting.Null
        && ParentLinkSetting != EdRecord.LinkContentSetting.Null )
      {
        CommandEntity.Design.LinkContentSetting = ParentLinkSetting;
      }

      this.LogDebug ( "FINAL: LinkContentSetting: " + CommandEntity.Design.LinkContentSetting );
      this.LogDebug ( "CommandTitle: " + CommandEntity.CommandTitle );
      this.LogDebug ( "getFirstTextField: " + CommandEntity.getFirstTextField ( ) );

      //
      // Define the pageMenuGroup groupCommand.
      //
      Evado.UniForm.Model.Command groupCommand = PageGroup.addCommand (
          CommandEntity.CommandTitle,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Records,
          Evado.UniForm.Model.ApplicationMethods.Get_Object );

      groupCommand.Id = CommandEntity.Guid;
      groupCommand.SetGuid ( CommandEntity.Guid );

      groupCommand.AddParameter (
        Evado.UniForm.Model.CommandParameters.Short_Title,
        EdLabels.Label_Record_Id + CommandEntity.RecordId );
      if ( CommandEntity.ImageFileName != String.Empty )
      {
        string relativeURL = EuAdapter.CONST_IMAGE_FILE_DIRECTORY + CommandEntity.ImageFileName;
        groupCommand.AddParameter ( Evado.UniForm.Model.CommandParameters.Image_Url, relativeURL );
      }

      if ( this.Session.UserProfile.hasAdministrationAccess == true )
      {
        groupCommand.Title = CommandEntity.RecordId + " >> " + groupCommand.Title;
      }

      return groupCommand;

    }//END getGroupListCommand method

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class private record export methods

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.Command object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.UniForm.Model.AppData getRecordExport_Object (
      Evado.UniForm.Model.Command PageCommand )
    {
      this.LogMethod ( "getRecordExport_Object" );

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.AppData clientDataObject = new Evado.UniForm.Model.AppData ( );
      List<EdRecord> recordList = new List<EdRecord> ( );
      try
      {

        // 
        // If the user does not have monitor or ResultData manager roles exit the page.
        // 
        if ( this.Session.Record.hasReadAccess ( this.Session.UserProfile.Roles ) == false )
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
        clientDataObject.Page.PageId = Evado.Digital.Model.EdStaticPageIds.Record_Export_Page.ToString ( );

        // 
        // Create the new pageMenuGroup for query selection.
        // 
        this.getRecordExport_SelectionGroup ( clientDataObject.Page );

        this.LogValue ( "FormId: " + this.Session.Selected_EntityLayoutId );
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
      Evado.UniForm.Model.Page Page )
    {
      this.LogMethod ( "getRecordExport_SelectionGroup" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.Group pageGroup = new Evado.UniForm.Model.Group ( );
      Evado.UniForm.Model.Command command = new Evado.UniForm.Model.Command ( );
      Evado.UniForm.Model.Field selectionField = new Evado.UniForm.Model.Field ( );
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
        Evado.UniForm.Model.EditAccess.Enabled );
      pageGroup.GroupType = Evado.UniForm.Model.GroupTypes.Default;
      pageGroup.Layout = Evado.UniForm.Model.GroupLayouts.Full_Width;
      pageGroup.AddParameter ( Evado.UniForm.Model.GroupParameterList.Offline_Hide_Group, true );

      //
      // Add the selection pageMenuGroup.
      //
      selectionField = pageGroup.createSelectionListField (
        EdRecord.FieldNames.Layout_Id,
        EdLabels.Label_Form_Id,
        this.Session.Selected_EntityLayoutId,
        this.Session.IssueFormList );

      selectionField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Define the include draft record selection option.
      //
      selectionField = pageGroup.createBooleanField (
        EuRecords.CONST_INCLUDE_DRAFT_RECORDS,
        EdLabels.Record_Export_Include_Draft_Record_Field_Title,
        this.Session.FormRecords_IncludeDraftRecords );
      selectionField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Define the include free text ResultData selection option.
      //
      selectionField = pageGroup.createBooleanField (
        EuRecords.CONST_INCLUDE_FREE_TEXT_DATA,
        EdLabels.Record_Export_Include_FreeText_data_Field_Title,
        this.Session.FormRecords_IncludeFreeTextData );
      selectionField.Layout = EuAdapter.DefaultFieldLayout;

      // 
      // Add the selection groupCommand
      // 
      command = pageGroup.addCommand (
        EdLabels.Record_Export_Selection_Group_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Records.ToString ( ),
         Evado.UniForm.Model.ApplicationMethods.Custom_Method );
      command.setCustomMethod ( Evado.UniForm.Model.ApplicationMethods.List_of_Objects );

    }//END getRecordExport_ListObject method

    // ==============================================================================
    /// <summary>
    /// This method creates the record view pageMenuGroup containing a list of commands to 
    /// open the form record.
    /// </summary>
    /// <param name="Page">Evado.Model.Uniform.Page object to add the pageMenuGroup to.</param>
    //  ------------------------------------------------------------------------------
    private void getRecordExport_DownloadGroup (
      Evado.UniForm.Model.Page Page )
    {
      this.LogMethod ( "getRecordExport_DownloadGroup" );
      this.LogDebug ( "FormRecords_IncludeFreeTextData: " + this.Session.FormRecords_IncludeFreeTextData );
      this.LogDebug ( "FormRecords_IncludeDraftRecords: " + this.Session.FormRecords_IncludeDraftRecords );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.Group pageGroup = new Evado.UniForm.Model.Group ( );
      Evado.UniForm.Model.Field groupField = new Evado.UniForm.Model.Field ( );
      EvFormRecordExport exportRecords = new EvFormRecordExport ( );
      EvExportParameters exportParameters = new EvExportParameters ( );
      EdQueryParameters queryParameters = new EdQueryParameters ( );
      int exportRecordFileLimit = 250;
      this.LogDebug ( "exportRecordFileLimit: " + exportRecordFileLimit );

      //
      // IF there are not parameters then exit.
      //
      if ( this.Session.Selected_EntityLayoutId == String.Empty )
      {
        this.LogDebug ( " Form {0}. ", this.Session.Selected_EntityLayoutId );
        this.LogMethodEnd ( "getRecordExport_DownloadGroup" );
        return;
      }


      exportParameters = new EvExportParameters (
        EvExportParameters.ExportDataSources.Project_Record,
        this.Session.Selected_EntityLayoutId );
      exportParameters.IncludeTestSites = false;
      exportParameters.IncludeFreeTextData = this.Session.FormRecords_IncludeFreeTextData;
      exportParameters.IncludeDraftRecords = this.Session.FormRecords_IncludeDraftRecords;

      queryParameters = new EdQueryParameters ( );
      queryParameters.LayoutId = this.Session.Selected_EntityLayoutId;

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
      int inResultCount = this._Bll_Records.getRecordCount ( queryParameters );

      this.LogClass ( this._Bll_Records.Log );

      this.LogDebug ( "inResultCount: " + inResultCount );
      // 
      // Create the record display pageMenuGroup.
      // 
      pageGroup = Page.AddGroup (
        EdLabels.Record_Export_Download_Group_Title,
        Evado.UniForm.Model.EditAccess.Enabled );
      pageGroup.CmdLayout = Evado.UniForm.Model.GroupCommandListLayouts.Vertical_Orientation;

      pageGroup.Layout = Evado.UniForm.Model.GroupLayouts.Full_Width;

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
             this.Session.Selected_EntityLayoutId );
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
             this.Session.Selected_EntityLayoutId );

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
    /// <param name="pageGroup">Evado.UniForm.Model.Group object</param>
    /// <param name="iteration">int: iteration loop</param>
    /// <param name="exportParameters">EvExportParameters object.</param>
    /// <param name="FormId">String form identifier</param>
    /// <returns>True export generated.</returns>
    //-----------------------------------------------------------------------------------
    private EvEventCodes exportRecordData (
      Evado.UniForm.Model.Group pageGroup,
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

      bool result = Evado.Digital.Model.EvcStatics.Files.saveFile (
        this.UniForm_BinaryFilePath,
        csvFileName,
        csvDownload );

      if ( result == false )
      {
        this.ErrorMessage = EdLabels.Record_Export_Error_Message;

        this.LogDebugClass ( Evado.Digital.Model.EvcStatics.Files.DebugLog );
        this.LogDebug ( "ReturnedEventCode: " + Evado.Digital.Model.EvcStatics.Files.ReturnedEventCode );
        this.LogDebug ( this.ErrorMessage );
        this.LogMethodEnd ( "exportRecordData" );
        return Evado.Digital.Model.EvcStatics.Files.ReturnedEventCode;
      }

      Evado.UniForm.Model.Field groupField = pageGroup.createHtmlLinkField (
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
    /// <param name="PageCommand">Evado.UniForm.Model.Command object.</param>
    /// <param name="PageObject">Evado.UniForm.Model.Page object.</param>
    /// <param name="RecordId">String: Entity identifier</param>
    //  ------------------------------------------------------------------------------
    public void getObject (
      Evado.UniForm.Model.Page PageObject,
      Evado.UniForm.Model.Command PageCommand,
      String RecordId )
    {
      this.LogMethod ( "getObject" );
      try
      {
        //
        // load a new entity if it is not loaded.
        //
        if ( this.Session.Record.RecordId != RecordId )
        {
          Guid entityGuid = this._Bll_Records.GetRecordGuid ( RecordId );

          this.LogDebug ( "Guid {0} == RecordId {1}: ", RecordId, entityGuid );

          PageCommand.SetGuid ( entityGuid );
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
            return;
          }

          this.LogValue ( "Entity.RecordId: " + this.Session.Record.RecordId );
        }

        // 
        // If the user does not have monitor or ResultData manager roles exit the page.
        // 
        if ( this.Session.Record.hasReadAccess ( this.Session.UserProfile.Roles ) == false )
        {
          this.LogIllegalAccess (
            this.ClassNameSpace + "getObject",
            this.Session.UserProfile );

          this.ErrorMessage = EdLabels.Record_Access_Error_Message;

          return;
        }

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "getObject",
          this.Session.UserProfile );

        //
        // Rung the server script if server side scripts are enabled.
        //
        this.runServerScript ( EvServerPageScript.ScripEventTypes.OnOpen );

        // 
        // Generate the client ResultData object for the UniForm client.
        // 
        this.getEntityClientData ( PageObject );

        // 
        // Return the client ResultData object to the calling method.
        // 
        this.LogMethodEnd ( "getObject" );
      }
      catch ( Exception Ex )
      {
        // 
        // On an exception raised create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.Entity_Retrieve_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }


      this.LogMethodEnd ( "getObject" );
      return;

    }//END getObject method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.Command object.</param>
    /// <returns>Evado.UniForm.Model.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.UniForm.Model.AppData getObject (
      Evado.UniForm.Model.Command PageCommand )
    {
      this.LogMethod ( "getObject" );
      try
      {
        // 
        // Initialise the methods variables and objects.
        // 
        Evado.UniForm.Model.AppData clientDataObject = new Evado.UniForm.Model.AppData ( );

        //
        // Get the record.
        //
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

        this.LogDebug ( "Entity.RecordId: {0}.", this.Session.Record.RecordId );
        this.LogDebug ( "Entity.Design.ReadAccessRoles: {0}.", this.Session.Record.Design.ReadAccessRoles );

        // 
        // If the user does not have monitor or ResultData manager roles exit the page.
        // 
        if ( this.Session.Record.hasReadAccess ( this.Session.UserProfile.Roles ) == false )
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
        // Rung the server script if server side scripts are enabled.
        //
        this.runServerScript ( EvServerPageScript.ScripEventTypes.OnOpen );

        // 
        // Initialise the client object.
        // 
        clientDataObject.Page.Id = clientDataObject.Id;
        clientDataObject.Page.PageDataGuid = clientDataObject.Id;
        clientDataObject.Page.PageId = this.Session.Record.LayoutId;
        clientDataObject.Page.EditAccess = Evado.UniForm.Model.EditAccess.Disabled;

        clientDataObject.Id = this.Session.Record.Guid;
        clientDataObject.Title = this.Session.Record.CommandTitle;

        if ( this.AdapterObjects.Settings.UseHomePageHeaderOnAllPages == true )
        {
          clientDataObject.Title = this.AdapterObjects.Settings.HomePageHeaderText;
        }


        if ( this.Session.Record.Design.LinkContentSetting == EdRecord.LinkContentSetting.First_Text_Field )
        {
          clientDataObject.Title = this.Session.Record.CommandTitle;
        }
        clientDataObject.Page.Title = clientDataObject.Title;

        // 
        // Generate the client ResultData object for the UniForm client.
        // 
        this.getEntityClientData ( clientDataObject.Page );

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
        this.ErrorMessage = EdLabels.Entity_Retrieve_Error_Message;

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
    ///  This method retrieves the entity into memory
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.Command object.</param>
    //  ---------------------------------------------------------------------------------
    private EvEventCodes GetRecord (
      Evado.UniForm.Model.Command PageCommand )
    {
      this.LogMethod ( "GetRecord" );

      //
      // if the page command has a layout identifier then assume parental identifier retrieval.
      //
      if ( PageCommand.hasParameter ( EdRecord.FieldNames.Layout_Id ) == true )
      {
        var result = this.GetRecordByParent ( PageCommand );

        this.LogMethodEnd ( "GetRecord" );
        return result;
      }

      //
      // Initialise the methods variables and objects.
      //
      Guid entityGuid = PageCommand.GetGuid ( );

      //
      // return a retrieval error message guid is empty.
      //
      if ( entityGuid == Guid.Empty
        && this.Session.Record.Guid == Guid.Empty )
      {
        return EvEventCodes.Identifier_Global_Unique_Identifier_Error;
      }

      //
      // If the record Guid match then the record is loaded so exit.
      //
      if ( entityGuid == this.Session.Record.Guid )
      {
        this.LogDebug ( "Entity Loaded" );
        this.LogMethodEnd ( "GetRecord" );

        this.Session.RefreshEntityChildren = true;

        return EvEventCodes.Ok;
      }

      //
      // Attempt to pull the entity from the entity dictionary.
      //
      this.Session.Record = this.Session.PullEntity ( entityGuid );

      //
      // if the pull returned entity exists exit.
      //
      if ( this.Session.Record != null )
      {
        this.Session.RefreshEntityChildren = true;

        this.LogDebug ( "Entity Loaded from dictionary" );
        this.LogMethodEnd ( "GetRecord" );
        return EvEventCodes.Ok;
      }


      // 
      // Retrieve the record object from the database via the DAL and BLL layers.
      // 
      this.Session.Record = this._Bll_Records.getRecord ( entityGuid );

      this.LogClass ( this._Bll_Records.Log );

      this.LogDebug ( "Entity {0}, Title {1}.", this.Session.Record.RecordId, this.Session.Record.Title );
      this.LogDebug ( "There are {0} of fields in the record.", this.Session.Record.Fields.Count );

      //
      // return a retrieval error message if the resulting common record guid is empty.
      //
      if ( this.Session.Record.Guid == Guid.Empty )
      {
        this.LogMethodEnd ( "GetRecord" );
        return EvEventCodes.Database_Record_Retrieval_Error;
      }

      //
      // push the new entity onto the directory list.
      //
      this.Session.PushEntity ( this.Session.Record );

      this.Session.RefreshEntityChildren = false;

      this.LogMethodEnd ( "GetRecord" );
      return EvEventCodes.Ok;

    }//ENd GetRecord method

    //  =============================================================================== 
    /// <summary>
    /// This method retrieves any by its LayoutId and Parent identifiers.
    /// </summary>
    //  ---------------------------------------------------------------------------------
    private EvEventCodes GetRecordByParent (
      Evado.UniForm.Model.Command PageCommand )
    {
      this.LogMethod ( "GetRecordByParent" );
      //
      // Initialise the methods variables and objects.
      //
      String layoutId = String.Empty;
      String orgId = String.Empty;
      String userId = String.Empty;
      Guid parentGuid = Guid.Empty;

      //
      // retrieve the parent references from the command parameters.
      //
      if ( PageCommand.hasParameter ( EdRecord.FieldNames.Layout_Id ) == true )
      {
        layoutId = PageCommand.GetParameter ( EdRecord.FieldNames.Layout_Id );
      }

      //
      // There can only be one parental identifier and the organisation identifier is taking precedence
      // over the user parental identifier.
      //
      if ( PageCommand.hasParameter ( EdRecord.FieldNames.ParentOrgId ) == true )
      {
        orgId = PageCommand.GetParameter ( EdRecord.FieldNames.ParentOrgId );
      }
      else
        if ( PageCommand.hasParameter ( EdRecord.FieldNames.ParentUserId ) == true )
        {
          userId = PageCommand.GetParameter ( EdRecord.FieldNames.ParentUserId );
        }
        else
          if ( PageCommand.hasParameter ( EdRecord.FieldNames.ParentGuid ) == true )
          {
            parentGuid = PageCommand.GetParameter<Guid> ( EdRecord.FieldNames.ParentGuid );
          }

      this.LogDebug ( "LayoutId {0}, orgId {1}, userId {2} parent Guid {3}",
        layoutId, orgId, userId, parentGuid );

      //
      // return a retrieval error message guid is empty.
      //
      if ( layoutId == String.Empty
        && orgId == String.Empty
        && userId == String.Empty
        && parentGuid == Guid.Empty )
      {
        this.LogMethodEnd ( "GetRecordByParent" );
        return EvEventCodes.Identifier_General_ID_Error;
      }

      //
      // if the pull returned entity exists exit.
      //
      if ( this.Session.Record != null )
      {
        this.LogDebug ( "Record Loaded from dictionary" );
        this.LogMethodEnd ( "GetRecordByParent" );
        return EvEventCodes.Ok;
      }

      //
      // Retrieve the record object from the database via the DAL and BLL layers.
      //
      if ( parentGuid != Guid.Empty )
      {
        this.LogDebug ( "Retrieving Record by parental Guid" );

        this.Session.Record = this._Bll_Records.GetItemByParentGuid ( layoutId, parentGuid );
      }
      else
        if ( orgId != String.Empty )
        {
          this.LogDebug ( "Retrieving Record by organisation parental identifier" );

          this.Session.Record = this._Bll_Records.GetItemByParentOrgId ( layoutId, orgId );
        }
        else
        {
          this.LogDebug ( "Retrieving Record by organisation parental identifier" );

          this.Session.Record = this._Bll_Records.GetItemByParentUserId ( layoutId, userId );

          //
          // Create the new entity.
          //
          this.Session.Record = this.CreateNewRecord ( layoutId, Guid.Empty );
        }

      this.LogClass ( this._Bll_Records.Log );

      //
      // Create the new entity if non was created.
      //
      if ( this.Session.Record.Guid == Guid.Empty )
      {
        this.Session.Record = this.CreateNewRecord ( layoutId, parentGuid );
        this.LogDebug ( "Entity {0}, Title {1}.", this.Session.Record.RecordId, this.Session.Record.Title );
        this.LogDebug ( "There are {0} of fields in the record.", this.Session.Record.Fields.Count );
      }

      //
      // return a retrieval error message if the resulting common record guid is empty.
      //
      if ( this.Session.Record.Guid == Guid.Empty )
      {
        this.LogMethodEnd ( "GetRecordByParent" );
        return EvEventCodes.Database_Record_Retrieval_Error;
      }

      //
      // push the new entity onto the directory list.
      //
      this.Session.PushEntity ( this.Session.Record );

      this.LogMethodEnd ( "GetRecordByParent" );
      return EvEventCodes.Ok;

    }//ENd GetRecord method

    //  =============================================================================== 
    /// <summary>
    ///  This method initiates the execution of the server side CS scripts.
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
        this._ServerPageScript.CsScriptPath = this.AdapterObjects.ApplicationPath + @"\csScripts\";


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
            + Evado.Model.EvStatics.enumValueToString ( iReturn ) + "\r\n";

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
    /// This method creates a save groupCommand.
    /// </summary>
    /// <returns>Evado.UniForm.Model.Command object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.UniForm.Model.Command createEntitySaveCommand (
      Guid RecordGuid,
      String SaveCommandTitle,
      String SaveAction )
    {
      this.LogMethod ( "createRecordSaveCommand" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.Parameter parameter = new Evado.UniForm.Model.Parameter ( );

      // 
      // create the save groupCommand.
      // 
      Evado.UniForm.Model.Command saveCommand = new Evado.UniForm.Model.Command (
        SaveCommandTitle,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Records.ToString ( ),
        Evado.UniForm.Model.ApplicationMethods.Save_Object );

      // 
      // Define the save groupCommand parameters.
      // 
      saveCommand.SetGuid ( RecordGuid );

      saveCommand.AddParameter (
        Evado.Digital.Model.EvcStatics.CONST_SAVE_ACTION,
       SaveAction );

      return saveCommand;
    }

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">Evado.UniForm.Model.AppData object.</param>
    //  ------------------------------------------------------------------------------
    public void getEntityClientData (
      Evado.UniForm.Model.Page PageObject )
    {
      this.LogMethod ( "getClientData" );
      /*
      this.LogDebug ( "UserProfile.Roles: " + this.Session.UserProfile.Roles );
      this.LogDebug ( "Entity.LayoutId: " + this.Session.Record.LayoutId );
      this.LogDebug ( "Entity.Title: " + this.Session.Record.Title );
      this.LogDebug ( "Entity.DefaultPageLayout: " + this.Session.Record.Design.DefaultPageLayout );
      this.LogDebug ( "Entity.FieldReadonlyDisplayFormat: " + this.Session.Record.Design.FieldReadonlyDisplayFormat );
      this.LogDebug ( "Entity.ReadAccessRoles: " + this.Session.Record.Design.ReadAccessRoles );
      this.LogDebug ( "Entity.EditAccessRoles: " + this.Session.Record.Design.EditAccessRoles );
      this.LogDebug ( "Entity.AuthorAccess: " + this.Session.Record.Design.AuthorAccess );
      this.LogDebug ( "Entity.ParentType: " + this.Session.Record.Design.ParentType );
      this.LogDebug ( "Entity.ParentGuid: " + this.Session.Record.ParentGuid );
      this.LogDebug ( "Entity.ParentOrgId: " + this.Session.Record.ParentOrgId );
      this.LogDebug ( "Entity.ParentUserId: " + this.Session.Record.ParentUserId );
      this.LogDebug ( "EnableEntityEditButtonUpdate: " + this.EnableEntityEditButtonUpdate );
      this.LogDebug ( "ButtonEditModeEnabled: " + this.Session.Record.ButtonEditModeEnabled );
      */

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.Group pageGroup = new Evado.UniForm.Model.Group ( );
      Evado.UniForm.Model.Field groupField = new Evado.UniForm.Model.Field ( );
      Evado.UniForm.Model.Parameter parameter = new Evado.UniForm.Model.Parameter ( );

      //
      // Initialise the page generator.
      //
      EuRecordGenerator pageGenerator = new EuRecordGenerator (
        this.AdapterObjects,
        this.ServiceUserProfile,
        this.Session,
        this.UniForm_BinaryFilePath,
        this.UniForm_BinaryServiceUrl,
        this.ClassParameters );

      //
      // The locked status of the record.
      //
      if ( this.checkRecordLockStatus ( PageObject ) == false )
      {
        this.LogDebug ( "The record is not locked." );

        this.getDataObject_PageCommands ( PageObject );
      }

      this.LogDebug ( "GENERATE LAYOUT" );

      //
      // Display the administrator group for the group.
      //
      if ( this.Session.UserProfile.hasAdministrationAccess == true
        && this.AdapterObjects.Settings.EnableAdminGroupOnEntityPages == true )
      {
        pageGroup = PageObject.AddGroup (
          EdLabels.Entity_Administrator_Header_Group_Title );
        pageGroup.EditAccess = Evado.UniForm.Model.EditAccess.Disabled;
        pageGroup.Layout = Evado.UniForm.Model.GroupLayouts.Full_Width;

        groupField = pageGroup.createTextField (
          String.Empty,
          EdLabels.Entity_Entity_Identifier_Field_Title,
          this.Session.Record.RecordId, 20 );

        groupField = pageGroup.createTextField (
          String.Empty,
          EdLabels.Entity_Entity_Identifier_Field_Title,
          this.Session.Record.CommandTitle, 80 );
      }

      // 
      // Call the page generation method
      // 
      bool result = pageGenerator.generateLayout (
        this.Session.Record,
        PageObject,
        this.UniForm_BinaryFilePath );

      this.LogClass ( pageGenerator.Log );

      //
      // Return null and an error message if the page generator exits with an error.
      //
      if ( result == false )
      {
        this.ErrorMessage = EdLabels.FormRecord_Page_Generation_Error;

        this.LogMethodEnd ( "getClientData " );

        return;
      }

      //
      // generate the group commands.
      //
      this.getDataObject_GroupCommands ( PageObject );


      this.LogMethodEnd ( "getClientData" );

    }//END getClientData Method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">Evado.UniForm.Model.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_PageCommands (
      Evado.UniForm.Model.Page PageObject )
    {
      this.LogMethod ( "getDataObject_PageCommands" );
      this.LogDebug ( "Entity.ButtonEditModeEnabled {0}. ", this.Session.Record.ButtonEditModeEnabled );

      //
      // Initialise the methods variables and objects.
      //
      Evado.UniForm.Model.Command pageCommand = new Evado.UniForm.Model.Command ( );
      String stSubmitCommandTitle = EdLabels.Entity_Submit_Command;

      // 
      // Set the user access to the records content.
      // 
      this.Session.Record.setUserAccess ( this.Session.UserProfile );
      this.LogDebug ( "Entity.FormAccessRole {0}. ", this.Session.Record.FormAccessRole );

      if ( this.Session.Record.FormAccessRole != EdRecord.FormAccessRoles.Record_Author )
      {
        this.LogMethodEnd ( "getDataObject_PageCommands" );
        return;
      }

      // 
      // Add the Edit command to the page.
      //
      if ( this.Session.Record.ButtonEditModeEnabled == false
        && this.EnableEntityEditButtonUpdate == true
        && this.Session.Record.State != EdRecordObjectStates.Empty_Record
        && this.Session.Record.State != EdRecordObjectStates.Draft_Record )
      {
        pageCommand = PageObject.addCommand (
          EdLabels.Entity_Edit_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Records.ToString ( ),
          Evado.UniForm.Model.ApplicationMethods.Get_Object );

        pageCommand.SetGuid ( this.Session.Record.Guid );

        pageCommand.AddParameter ( EuRecords.CONST_EDIT_MODE_FIELD, "yes" );

        this.Session.Record.FormAccessRole = EdRecord.FormAccessRoles.Record_Reader;
      }
      else
      {
        // 
        // If the user has author access. 
        // 
        // Set the page status to edit enabled
        // 
        PageObject.EditAccess = Evado.UniForm.Model.EditAccess.Enabled;
        PageObject.DefaultGroupType = Evado.UniForm.Model.GroupTypes.Default;

        // 
        // Add the save groupCommand and add it to the page groupCommand list.
        // 
        if ( this.EnableEntitySaveButtonUpdate == true )
        {
          pageCommand = PageObject.addCommand (
            EdLabels.Entity_Save_Command_Title,
            EuAdapter.ADAPTER_ID,
            EuAdapterClasses.Records.ToString ( ),
            Evado.UniForm.Model.ApplicationMethods.Save_Object );

          pageCommand.SetGuid ( this.Session.Record.Guid );

          pageCommand.AddParameter (
            Evado.Digital.Model.EvcStatics.CONST_SAVE_ACTION,
           EdRecord.SaveActionCodes.Save_Record.ToString ( ) );
        }
        else
        {
          stSubmitCommandTitle = EdLabels.Entity_Save_Command_Title;
        }

        // 
        // Add the submit comment to the page groupCommand list.
        // 
        pageCommand = PageObject.addCommand (
          stSubmitCommandTitle,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Records.ToString ( ),
          Evado.UniForm.Model.ApplicationMethods.Save_Object );

        pageCommand.SetGuid ( this.Session.Record.Guid );
        pageCommand.AddParameter (
          Evado.Digital.Model.EvcStatics.CONST_SAVE_ACTION,
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
            EdLabels.Entity_Withdraw_Command,
            EuAdapter.ADAPTER_ID,
            EuAdapterClasses.Records.ToString ( ),
            Evado.UniForm.Model.ApplicationMethods.Save_Object );

          pageCommand.SetGuid ( this.Session.Record.Guid );
          pageCommand.AddParameter (
            Evado.Digital.Model.EvcStatics.CONST_SAVE_ACTION,
           EdRecord.SaveActionCodes.Withdrawn_Record.ToString ( ) );
        }

      }//END in edit mode.

      this.LogDebug ( "Session.Record.FormAccessRole {0}. ", this.Session.Record.FormAccessRole );
      this.LogMethodEnd ( "getDataObject_PageCommands" );

    }//END getClientData_SaveCommands method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">Evado.UniForm.Model.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_GroupCommands (
      Evado.UniForm.Model.Page PageObject )
    {
      this.LogMethod ( "getDataObject_GroupCommands" );
      //
      // Display save command in the last group.
      //
      if ( this.Session.Record.FormAccessRole != EdRecord.FormAccessRoles.Record_Author
        || PageObject.GroupList.Count == 0 )
      {
        this.LogMethodEnd ( "getDataObject_GroupCommands" );
        return;
      }

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.Group pageGroup = new Evado.UniForm.Model.Group ( );
      Evado.UniForm.Model.Command pageCommand = new Evado.UniForm.Model.Command ( );
      String stSubmitCommandTitle = EdLabels.Entity_Submit_Command;

      this.LogValue ( "Including save commands in the last group." );
      pageGroup = PageObject.GroupList [ PageObject.GroupList.Count - 1 ];

      // 
      // Add the save groupCommand and add it to the page groupCommand list.
      //  
      if ( this.EnableEntitySaveButtonUpdate == true )
      {
        pageCommand = pageGroup.addCommand (
          EdLabels.Entity_Save_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Records.ToString ( ),
          Evado.UniForm.Model.ApplicationMethods.Save_Object );

        pageCommand.SetGuid ( this.Session.Record.Guid );

        pageCommand.AddParameter (
          Evado.Digital.Model.EvcStatics.CONST_SAVE_ACTION,
         EdRecord.SaveActionCodes.Save_Record.ToString ( ) );
      }
      else
      {
        stSubmitCommandTitle = EdLabels.Entity_Save_Command_Title;
      }

      // 
      // Add the submit comment to the page groupCommand list.
      // 
      pageCommand = pageGroup.addCommand (
        stSubmitCommandTitle,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Records.ToString ( ),
        Evado.UniForm.Model.ApplicationMethods.Save_Object );

      pageCommand.SetGuid ( this.Session.Record.Guid );
      pageCommand.AddParameter (
        Evado.Digital.Model.EvcStatics.CONST_SAVE_ACTION,
       EdRecord.SaveActionCodes.Submit_Record.ToString ( ) );

      pageCommand.setEnableForMandatoryFields ( );

      // 
      // Add the wihdrawn groupCommand to the page groupCommand list.
      // 
      if ( ( this.Session.Record.State == EdRecordObjectStates.Draft_Record
          || this.Session.Record.State == EdRecordObjectStates.Empty_Record
          || this.Session.Record.State == EdRecordObjectStates.Completed_Record ) )
      {
        pageCommand = pageGroup.addCommand (
          EdLabels.Entity_Withdraw_Command,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Records.ToString ( ),
          Evado.UniForm.Model.ApplicationMethods.Save_Object );

        pageCommand.SetGuid ( this.Session.Record.Guid );
        pageCommand.AddParameter (
          Evado.Digital.Model.EvcStatics.CONST_SAVE_ACTION,
         EdRecord.SaveActionCodes.Withdrawn_Record.ToString ( ) );
      }

      this.LogMethodEnd ( "getDataObject_GroupCommands" );
    }

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class create record methods

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.ClientDataObjectEvado.UniForm.Model.Command object.</param>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.UniForm.Model.AppData createObject (
      Evado.UniForm.Model.Command PageCommand )
    {
      this.LogMethod ( "createObject" );
      this.LogDebug ( "PageCommand: {0}.", PageCommand.getAsString ( false, true ) );
      this.LogDebug ( "Entity_SelectedLayoutId: {0}.", this.Session.Selected_EntityLayoutId );
      try
      {
        //
        // Initialiset the methods variables and objects.
        //
        Evado.UniForm.Model.AppData clientDataObject = new Evado.UniForm.Model.AppData ( );

        if ( this.Session.Selected_EntityLayoutId == String.Empty )
        {
          this.LogMethodEnd ( "createObject" );
          return this.Session.LastPage;
        }
        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "createObject",
          this.Session.UserProfile );

        // 
        // Initialise the methods variables and objects.
        //    
        string LayoutId = PageCommand.GetParameter ( EdRecord.FieldNames.Layout_Id );
        //
        // Set the parent entity variables.
        //
        this.ParentLayoutId = this.Session.Record.LayoutId;
        this.ParentGuid = this.Session.Record.Guid;
        if ( PageCommand.hasParameter ( EdRecord.FieldNames.ParentGuid ) == true )
        {
          this.ParentGuid = PageCommand.GetParameterAsGuid ( EdRecord.FieldNames.ParentGuid );
        }
        if ( PageCommand.hasParameter ( EdRecord.FieldNames.ParentLayoutId ) == true )
        {
          this.ParentLayoutId = PageCommand.GetParameter ( EdRecord.FieldNames.ParentLayoutId );
        }
        this.LogDebug ( "ParentGuid: {0}.", this.ParentGuid );
        this.LogDebug ( "ParentLayoutId: {0}.", this.ParentLayoutId );
        //
        // Create the new entity.
        //
        this.Session.Record = this.CreateNewRecord ( LayoutId, this.ParentGuid );

        if ( this.Session.Record.Guid == Guid.Empty )
        {
          this.ErrorMessage = EdLabels.Form_Record_Creation_Error_Message;

          this.LogError ( EvEventCodes.Database_Record_Retrieval_Error,
            this.ErrorMessage );

          return this.Session.LastPage;
        }


        this.LogDebug ( "CREATED Entity Id: " + this.Session.Record.RecordId );

        // 
        // Initialise the client object.
        // 
        clientDataObject.Page.Id = clientDataObject.Id;
        clientDataObject.Page.PageDataGuid = clientDataObject.Id;
        clientDataObject.Page.PageId = this.Session.Record.LayoutId;
        clientDataObject.Page.EditAccess = Evado.UniForm.Model.EditAccess.Enabled;

        clientDataObject.Id = this.Session.Record.Guid;
        clientDataObject.Title = this.Session.Record.CommandTitle;

        if ( this.AdapterObjects.Settings.UseHomePageHeaderOnAllPages == true )
        {
          clientDataObject.Title = this.AdapterObjects.Settings.HomePageHeaderText;
        }

        clientDataObject.Page.Title = clientDataObject.Title;
        this.LogDebug
          ( "Title.Length: " + clientDataObject.Title.Length );

        clientDataObject.Page.EditAccess = Evado.UniForm.Model.EditAccess.Disabled;

        // 
        // Generate the new page layout 
        // 
        this.getEntityClientData ( clientDataObject.Page );

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

    // ==================================================================================
    /// <summary>
    /// THis method creates a new entity
    /// </summary>
    /// <param name="LayoutId">String Layout identifier.</param>
    /// <param name="ParentGuid">Guid: parent Entity Guid.</param>
    /// <returns>Evado.Digital.Model.EdRecord</returns>
    //  ----------------------------------------------------------------------------------
    private EdRecord CreateNewRecord (
      String LayoutId,
      Guid ParentGuid )
    {
      this.LogMethod ( "CreateNewRecord" );
      this.LogDebug ( "LayoutId {0}.", LayoutId );
      this.LogDebug ( "ParentGuid {0}.", ParentGuid );
      this.LogDebug ( this.Session.UserProfile.getUserProfile ( true ) );
      //
      // Initialiset the methods variables and objects.
      //
      Evado.Digital.Model.EdRecord newRecord = new Evado.Digital.Model.EdRecord ( );

      newRecord.Guid = Guid.NewGuid ( );
      newRecord.LayoutId = LayoutId;
      newRecord.AuthorUserId = this.Session.UserProfile.UserId;
      newRecord.RecordDate = DateTime.Now;

      this.Session.RecordLayout = this.AdapterObjects.GetRecordLayout ( LayoutId );

      if ( this.Session.RecordLayout.hasEditAccess ( this.Session.UserProfile.Roles ) == false )
      {
        this.LogDebug ( "User does not have create access." );
        this.LogMethodEnd ( "CreateNewRecord" );
        return new EdRecord ( );
      }
      //
      // Set the new entities parents 
      //
      switch ( this.Session.RecordLayout.Design.ParentType )
      {
        case EdRecord.ParentTypeList.User:
          {
            newRecord.ParentUserId = this.Session.UserProfile.UserId;
            break;

          }
        case EdRecord.ParentTypeList.Organisation:
          {
            newRecord.ParentOrgId = this.Session.UserProfile.OrgId;
            break;
          }
        case EdRecord.ParentTypeList.Entity:
          {
            newRecord.ParentGuid = ParentGuid;
            break;
          }
      }//END switch statement

      this.LogDebug ( "LayoutId {0}.", newRecord.LayoutId );
      this.LogDebug ( "AuthorUserId {0}.", newRecord.AuthorUserId );
      this.LogDebug ( "ParentOrgId {0}.", newRecord.ParentOrgId );
      this.LogDebug ( "ParentUserId {0}.", newRecord.ParentUserId );
      this.LogDebug ( "ParentLayoutId {0}.", newRecord.ParentLayoutId );
      // 
      // Create the record.
      // 
      var entity = this._Bll_Records.CreateRecord ( newRecord );

      this.LogClass ( this._Bll_Records.Log );

      this.LogMethodEnd ( "CreateNewRecord" );
      return entity;
    }//END MEthod.

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class update record methods

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.Command object.</param>
    /// <returns>Evado.UniForm.Model.AppData</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.UniForm.Model.AppData updateObject (
      Evado.UniForm.Model.Command PageCommand )
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

        string stSaveAction = PageCommand.GetParameter ( Evado.Digital.Model.EvcStatics.CONST_SAVE_ACTION );
        this.LogValue ( " Save Action: " + stSaveAction );

        //
        // Rung the server script if server side scripts are enabled.
        //
        this.runServerScript ( EvServerPageScript.ScripEventTypes.OnUpdate );

        // 
        // Update the object.
        // 
        if ( this.Session.PageId == Evado.Digital.Model.EdStaticPageIds.Record_Admin_Page.ToString ( ) )
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
           Evado.Model.EvStatics.parseEnumValue<EdRecord.SaveActionCodes> ( stSaveAction );

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
        this.LogDebug ( "Actual Action: " + this.Session.Record.SaveAction );

        // 
        // Execute the save record groupCommand to save the record values to the 
        // Evado database.
        // 
        EvEventCodes result = this._Bll_Records.saveRecord (
          this.Session.Record, true );

        this.LogClass ( this._Bll_Records.Log );

        // 
        // If an error state is returned log the event.
        // 
        if ( result != EvEventCodes.Ok )
        {
          string stEvent = this._Bll_Records.Log + " returned error message: "
            + result + " = " + Evado.Digital.Model.EvcStatics.getEventMessage ( result );

          this.LogValue ( stEvent );

          this.LogError ( EvEventCodes.Database_Record_Update_Error, stEvent );

          this.ErrorMessage = EdLabels.Record_Update_Error_Message;

          if ( this.DebugOn == true )
          {
            this.ErrorMessage += EdLabels.Space_Hypen
              + Evado.Digital.Model.EvcStatics.getEventMessage ( result );
          }

          this.LogMethodEnd ( "updateObject" );
          return this.Session.LastPage;
        }

        if ( this.Session.Record.Design.ParentType == EdRecord.ParentTypeList.Entity )
        {
          this.LogDebug ( "Refreshing Entity Children" );
          this.Session.RefreshEntityChildren = true;
        }

        //
        // Force a refresh of hte form record list.
        //
        this.Session.RecordList = new List<EdRecord> ( );
        this.Session.Record.ButtonEditModeEnabled = false;

        if ( this.Session.Record.SaveAction == EdRecord.SaveActionCodes.Layout_Approved )
        {
          this.AdapterObjects.AllEntityLayouts = new List<EdRecord> ( );
          this.AdapterObjects.PageComponents = new List<EvOption> ( );
        }
        this.LogMethodEnd ( "updateObject" );
        return new Evado.UniForm.Model.AppData ( );

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
    /// <param name="PageCommand"> Evado.UniForm.Model.Command object.</param>
    //  ----------------------------------------------------------------------------------
    private void updateObject_AdminValues (
      Evado.UniForm.Model.Command PageCommand )
    {
      this.LogMethod ( "updateObject_AdminValues" );
      this.LogValue ( " Parameters.Count: " + PageCommand.Parameters.Count );

      String stDate = PageCommand.GetParameter ( EdRecord.FieldNames.RecordDate );
      this.Session.Record.RecordDate = Evado.Digital.Model.EvcStatics.getDateTime ( stDate );

      String stState = PageCommand.GetParameter ( EdRecord.FieldNames.Status );
      this.Session.Record.State = Evado.Model.EvStatics.parseEnumValue<EdRecordObjectStates> ( stState );

    }//END updateObject_AdminValues method.

    // ==================================================================================
    /// <summary>
    /// THis method updates the form record values with the groupCommand parameter values.
    /// </summary>
    /// <param name="PageCommand"> Evado.UniForm.Model.Command object.</param>
    //  ----------------------------------------------------------------------------------
    private void updateObjectValues (
      Evado.UniForm.Model.Command PageCommand )
    {
      this.LogMethod ( "updateObjectValue" );
      this.LogValue ( " Parameters.Count: " + PageCommand.Parameters.Count );

      // 
      // Initialise method variables and objects.
      // 
      EuRecordGenerator pageGenerator = new EuRecordGenerator (
        this.AdapterObjects,
        this.ServiceUserProfile,
        this.Session,
        this.UniForm_BinaryFilePath,
        this.UniForm_BinaryServiceUrl,
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
    /// <returns>Evado.UniForm.Model.Command object</returns>
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
      // Iterate through the   Evado.Digital.Model.EvForm TestItemList to set their state
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