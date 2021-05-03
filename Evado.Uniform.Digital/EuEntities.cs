/***************************************************************************************
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
  public partial class EuEntities : EuClassAdapterBase
  {
    #region Class Initialisation
    /// <summary>
    /// This method initialises the class.
    /// </summary>
    public EuEntities ( )
    {
      if ( this.Session.Entity == null )
      {
        this.Session.Entity = new EdRecord ( );
      }
      if ( this.Session.EntityLayout == null )
      {
        this.Session.EntityLayout = new EdRecord ( );
      }
      if ( this.Session.EntityList == null )
      {
        this.Session.EntityList = new List<EdRecord> ( );
      }
      if ( this.Session.Selected_EntityLayoutId == null )
      {
        this.Session.Selected_EntityLayoutId = String.Empty;
      }
      if ( this.Session.EntityDictionary == null )
      {
        this.Session.EntityDictionary = new List<EdRecord> ( );
      }
      for ( int filterIndex = 0; filterIndex < 5; filterIndex++ )
      {
        this.Session.EntitySelectionFilters [ filterIndex ] = String.Empty;
      }
      this.ClassNameSpace = "Evado.UniForm.Digital.EuEntities.";
    }

    /// <summary>
    /// This method initialises the class and passs in the user profile.
    /// </summary>
    public EuEntities (
      EuGlobalObjects AdapterObjects,
      EvUserProfileBase ServiceUserProfile,
      EuSession SessionObjects,
      String UniForm_BinaryFilePath,
      String UniForm_BinaryServiceUrl,
      EvClassParameters ClassParameters )
    {
      this.ClassNameSpace = "Evado.UniForm.Digital.EuEntities.";
      this.AdapterObjects = AdapterObjects;
      this.ServiceUserProfile = ServiceUserProfile;
      this.Session = SessionObjects;
      this.UniForm_BinaryFilePath = UniForm_BinaryFilePath;
      this.UniForm_BinaryServiceUrl = UniForm_BinaryServiceUrl;
      this.ClassParameters = ClassParameters;

      this.LogInitMethod ( "EuEntities initialisation" );
      this.LogInit ( "ServiceUserProfile.UserId: " + ServiceUserProfile.UserId );
      this.LogInit ( "SessionObjects.UserProfile.Userid: " + this.Session.UserProfile.UserId );
      this.LogInit ( "SessionObjects.UserProfile.CommonName: " + this.Session.UserProfile.CommonName );
      this.LogInit ( "UniForm BinaryFilePath: " + this.UniForm_BinaryFilePath );
      this.LogInit ( "UniForm BinaryServiceUrl: " + this.UniForm_BinaryServiceUrl );

      this.LogInit ( "Settings" );
      this.LogInit ( "-LoggingLevel: " + this.ClassParameters.LoggingLevel );
      this.LogInit ( "-UserId: " + this.ClassParameters.UserProfile.UserId );
      this.LogInit ( "-UserCommonName: " + this.ClassParameters.UserProfile.CommonName );

      this._Bll_Entities = new EdEntities ( ClassParameters );
      this.EnableEntityEditButtonUpdate = this.AdapterObjects.Settings.EnableEntityEditButtonUpdate;
      this.EnableEntitySaveButtonUpdate = this.AdapterObjects.Settings.EnableEntitySaveButtonUpdate;


      if ( this.Session.Entity == null )
      {
        this.Session.Entity = new EdRecord ( );
      }
      if ( this.Session.EntityLayout == null )
      {
        this.Session.EntityLayout = new EdRecord ( );
      }
      if ( this.Session.EntityList == null )
      {
        this.Session.EntityList = new List<EdRecord> ( );
      }
      if ( this.Session.Selected_EntityLayoutId == null )
      {
        this.Session.Selected_EntityLayoutId = String.Empty;
      }
      if ( this.Session.EntityDictionary == null )
      {
        this.Session.EntityDictionary = new List<EdRecord> ( );
      }
      if ( this.Session.SelectedState == null )
      {
        this.Session.SelectedState = String.Empty;
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

    private Evado.Bll.Digital.EdEntities _Bll_Entities = new Evado.Bll.Digital.EdEntities ( );
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
    /// <param name="PageCommand">ClientPateEvado.Model.UniForm.Command object</param>
    /// <returns>Evado.Model.UniForm.AppData</returns>
    //  ----------------------------------------------------------------------------------
    override public Evado.Model.UniForm.AppData getDataObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getDataObject" );
      this.LogValue ( "PageCommand: " + PageCommand.getAsString ( false, true ) );
      this.LogDebug ( "EnableEntityEditButtonUpdate: " + this.EnableEntityEditButtonUpdate );
      this.LogDebug ( "EnableEntitySaveButtonUpdate: " + this.EnableEntitySaveButtonUpdate );
      this.LogDebug ( "ButtonEditModeEnabled: " + this.Session.Entity.ButtonEditModeEnabled );
      this.LogDebug ( "RefreshEntityChildren: " + this.Session.RefreshEntityChildren );

      try
      {
        // 
        // Initialise the methods variables and objects.
        // 
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
        this.Session.PageId = PageCommand.GetPageId();

        this.LogDebug ( "PageId: {0}", this.Session.StaticPageId );

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

              switch ( this.Session.StaticPageId )
              {
                case EdStaticPageIds.Entity_Export_Page:
                  {
                    clientDataObject = this.getRecordExport_Object ( PageCommand );
                    break;
                  }
                case EdStaticPageIds.Entity_Filter_View:
                  {
                    clientDataObject = this.GetFilteredListObject ( PageCommand );
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

              switch ( this.Session.StaticPageId )
              {
                case EdStaticPageIds.Entity_User_Access_Page:
                  {
                    this.LogDebug ( "Entity_User_Access_Page" );
                    clientDataObject = this.getUserAccessObject ( PageCommand );
                    break;
                  }
                default:
                  {
                    this.LogDebug ( "Get Object method" );

                    clientDataObject = this.getObject ( PageCommand );
                    break;
                  }
              }//END StaticPageId switch

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

    // ===============================================================================
    /// <summary>
    /// This method generates a component group output.
    /// </summary>
    /// <param name="PageObject">Evado.Model.Uniform.Page object .</param>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object</param>
    /// <param name="Component">String: component identifier</param>
    /// <returns>EvEventCodes enumeration</returns>
    //  ----------------------------------------------------------------------------------
    public EvEventCodes getPageComponent (
      Evado.Model.UniForm.Page PageObject,
      Evado.Model.UniForm.Command PageCommand,
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
      if ( this.Session.EntityLayout == null )
      {
        this.Session.EntityLayout = new EdRecord ( );
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

          this.Session.EntityLayout = this.AdapterObjects.GetEntityLayout ( layoutId );
          this.LogDebug ( "Layout {0} - {1}", this.Session.EntityLayout.LayoutId, this.Session.EntityLayout.Title );

          return this.getListObject ( PageObject, layoutId );
        }

        //
        // get the filters entity list for the layout identifier.
        //
        if ( pageId.Contains ( EuAdapter.CONST_ENTITY_FILTERED_LIST_PREFIX ) == true )
        {
          this.LogDebug ( "Page Entity Filtered List selected" );
          var layoutId = pageId.Replace ( EuAdapter.CONST_ENTITY_FILTERED_LIST_PREFIX, String.Empty );

          this.Session.EntityLayout = this.AdapterObjects.GetEntityLayout ( layoutId );
          this.LogDebug ( "Layout {0} - {1}", this.Session.EntityLayout.LayoutId, this.Session.EntityLayout.Title );

          return this.GetFilteredListObject ( PageObject, layoutId );
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
      Evado.Model.UniForm.Page PageObject,
      String LayoutId )
    {
      this.LogMethod ( "getListObject" );
      this.LogValue ( "LayoutId: " + LayoutId );
      this.LogDebug ( "EntitySelectionLayoutId: " + this.Session.Selected_EntityLayoutId );
      this.LogDebug ( "UserProfile.Roles: " + this.Session.UserProfile.Roles );
      try
      {
        if ( LayoutId == String.Empty )
        {
          LayoutId = this.Session.Selected_EntityLayoutId;
        }

        //
        // get the selected entity.
        //
        this.Session.EntityLayout = this.AdapterObjects.GetEntityLayout ( LayoutId );

        this.LogDebug ( "Entity.ReadAccessRoles: " + this.Session.EntityLayout.Design.ReadAccessRoles );
        this.LogDebug ( "Entity.EditAccessRoles: " + this.Session.EntityLayout.Design.EditAccessRoles );
        this.LogDebug ( "Entity.LinkContentSetting: " + this.Session.EntityLayout.Design.LinkContentSetting );

        // 
        // If the user does not have monitor or ResultData manager roles exit the page.
        // 
        if ( this.Session.EntityLayout.hasReadAccess ( this.Session.UserProfile.Roles ) == false )
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
    /// This method generate filter list of entities..
    /// </summary>
    /// <param name="PageObject">Evado.Model.Uniform.Page object .</param>
    /// <param name="LayoutId">String: optional layout identifier.</param>
    /// <returns>EvEventCodes enumeration indicating the execution outcome.</returns>
    //  -----------------------------------------------------------------------------
    private EvEventCodes GetFilteredListObject (
      Evado.Model.UniForm.Page PageObject,
      String LayoutId )
    {
      this.LogMethod ( "GetFilteredListObject" );
      this.LogDebug ( "LayoutId: " + LayoutId );
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
        this.Session.EntityLayout = this.AdapterObjects.GetEntityLayout ( LayoutId );

        // 
        // If the user does not have monitor or ResultData manager roles exit the page.
        // 
        if ( this.Session.EntityLayout.hasReadAccess ( this.Session.UserProfile.Roles ) == false )
        {
          this.LogIllegalAccess (
            this.ClassNameSpace + "GetFilteredListObject",
            this.Session.UserProfile );

          this.ErrorMessage = EdLabels.Record_Access_Error_Message;

          this.LogMethodEnd ( "GetFilteredListObject" );
          return EvEventCodes.User_Access_Error;
        }
        // 
        // Log the user's access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "GetFilteredListObject",
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
        if ( this.enableQuery ( ) == true )
        {
          this.executeRecordQuery ( );
        }

        // 
        // Create the new pageMenuGroup for query selection.
        // 
        this.getFilteredList_SelectionGroup ( PageObject );

        // 
        // Create the pageMenuGroup containing commands to open the records.
        //         
        this.getEntity_Summary_ListGroup ( PageObject );

        this.LogMethodEnd ( "GetFilteredListObject" );
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

      this.LogMethodEnd ( "GetFilteredListObject" );
      return EvEventCodes.Page_Loading_General_Error;

    }//END GetFilteredListObject method.

    // ==============================================================================
    /// <summary>
    /// This method generates an entity data page.
    /// </summary>
    /// <param name="PageObject">Evado.Model.Uniform.Page object .</param>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>EvEventCodes enumeration indicating the execution outcome.</returns>
    //  ------------------------------------------------------------------------------
    private EvEventCodes getObject (
      Evado.Model.UniForm.Page PageObject,
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getObject" );
      try
      {
        //
        // Get the record.

        var result = this.GetEntity ( PageCommand );

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

        this.LogDebug ( "EntityDictionary count {0}.", this.Session.EntityDictionary.Count );

        foreach ( EdRecord entity in this.Session.EntityDictionary )
        {
          this.LogDebug ( "Loaded Entity {0} LayoutId {1} - {2}",
            entity.EntityId, entity.LayoutId, entity.Title );
        }

        this.LogValue ( "Entity.EntityId: " + this.Session.Entity.EntityId );

        // 
        // If the user does not have monitor or ResultData manager roles exit the page.
        // 
        if ( this.Session.Entity.hasReadAccess ( this.Session.UserProfile.Roles ) == false )
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
      if ( this.Session.Entity.BookedOutBy == String.Empty )
      {
        this.LogDebug ( "Record not locked" );
        this.LogMethodEnd ( "checkRecordLockStatus" );
        return false;
      }

      // 
      // Test if the record is already locked.
      // 
      if ( this.Session.Entity.BookedOutBy != String.Empty
        && this.Session.Entity.BookedOutBy != this.Session.UserProfile.CommonName )
      {
        this.ErrorMessage =
          String.Format ( EdLabels.Form_Record_Locked_Message,
          this.Session.Entity.RecordId,
          this.Session.Entity.BookedOutBy );

        //
        // If the user is an administrator display a command to unlock the record.
        //
        if ( this.Session.UserProfile.hasManagementAccess )
        {
          Evado.Model.UniForm.Command pageCommand = PageObject.addCommand (
            EdLabels.Form_UnLock_Command_Title,
            EuAdapter.ADAPTER_ID,
            EuAdapterClasses.Entities.ToString ( ),
             Model.UniForm.ApplicationMethods.Save_Object );

          pageCommand.SetGuid ( this.Session.Entity.Guid );
        }

        return true;
      }

      //DEBUG

      //
      // Do not lock the record is the user does not have update access.
      //
      if ( this.Session.EntityLayout.hasReadAccess ( this.Session.UserProfile.Roles ) == false )
      {
        return false;
      }

      //
      // Execute the record lock method.
      //
      EvEventCodes iReturn = this._Bll_Entities.lockItem ( this.Session.Entity );

      if ( iReturn != EvEventCodes.Ok )
      {
        return true;
      }

      // 
      // Set a lock value so we can unlock the record when exited without saving.
      // 
      this.Session.Entity.BookedOutBy = this.Session.UserProfile.CommonName;

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
      if ( this.Session.Entity.BookedOutBy == String.Empty
        || this.Session.Entity.BookedOutBy != this.Session.UserProfile.CommonName )
      {
        return true;
      }

      // 
      // Execute the unlock method to the database.
      // 
      EvEventCodes iReturn = this._Bll_Entities.unlockItem ( this.Session.Entity );

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
      EvEventCodes iReturn = this._Bll_Entities.unlockItem ( this.Session.Entity );

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

      //
      // If it is a custom method then empty the Entity list.
      //
      if ( PageCommand.hasParameter ( Model.UniForm.CommandParameters.Custom_Method ) == true )
      {
        this.Session.EntityList = new List<EdRecord> ( );
      }

      //
      // Update the user access page parameters.
      //
      if ( this.Session.StaticPageId == EdStaticPageIds.Entity_User_Access_Page )
      {
        if ( PageCommand.hasParameter ( EdUserProfile.FieldNames.User_Category ) == true )
        {
          this.Session.SelectedUserCategory = PageCommand.GetParameter ( EdUserProfile.FieldNames.User_Category );
        }
        this.LogValue ( "SelectedUserCategory: " + this.Session.SelectedUserCategory );

        if ( PageCommand.hasParameter ( EdUserProfile.FieldNames.User_Type ) == true )
        {
          this.Session.SelectedUserType = PageCommand.GetParameter ( EdUserProfile.FieldNames.User_Type );
        }
        this.LogValue ( "SelectedUserType: " + this.Session.SelectedUserType );

        if ( PageCommand.hasParameter ( EdUserProfile.FieldNames.Address_City ) == true )
        {
          this.Session.SelectedCity = PageCommand.GetParameter ( EdUserProfile.FieldNames.Address_City );
        }
        this.LogValue ( "SelectedCity: " + this.Session.SelectedCity );

        if ( PageCommand.hasParameter ( EdUserProfile.FieldNames.Address_State ) == true )
        {
          this.Session.SelectedState= PageCommand.GetParameter ( EdUserProfile.FieldNames.Address_State );
        }
        this.LogValue ( "SelectedState: " + this.Session.SelectedState );

        if ( PageCommand.hasParameter ( EdUserProfile.FieldNames.Address_Country) == true )
        {
          this.Session.SelectedCountry = PageCommand.GetParameter ( EdUserProfile.FieldNames.Address_Country );
        }
        this.LogValue ( "SelectedCountry: " + this.Session.SelectedCountry );

      }

      //
      // if the entity layout is defined in the page command then update its value.
      //
      if ( PageCommand.hasParameter ( EuEntities.CONST_HIDE_SELECTION ) == true )
      {
        this._HideSelectionGroup = true;
      }
      this.LogValue ( "HideSelectionGroup: " + this._HideSelectionGroup );

      //
      // if the entity layout is defined in the page command then update its value.
      //
      if ( PageCommand.hasParameter ( EuEntities.CONST_EDIT_MODE_FIELD ) == true )
      {
        this.Session.Entity.ButtonEditModeEnabled = true;
      }
      this.LogValue ( "ButtonEditModeEnabled: " + this.Session.Entity.ButtonEditModeEnabled );

      //
      // enable empty selection query.
      //
      if ( PageCommand.hasParameter ( EdRecord.FieldNames.ParentLayoutId ) == true )
      {
        var value = PageCommand.GetParameter ( EuEntities.CONST_EMPTY_SELECTION_FIELD );

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
        this.Session.EntityLayout = this.AdapterObjects.GetEntityLayout ( this.Session.Selected_EntityLayoutId );

        this.LogValue ( "EntityLayout.LayoutId: " + this.Session.EntityLayout.LayoutId );
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

        if ( this.Session.EntityTypeSelection != recordType )
        {
          this.Session.EntityTypeSelection = recordType;
          this.AdapterObjects.AllEntityLayouts = new List<EdRecord> ( );
        }
      }
      this.LogValue ( "EntityTypeSelection: " + this.Session.EntityTypeSelection );


      //
      // if the entity record status is defined in the page command then update its value.
      //
      if ( PageCommand.hasParameter ( EdRecord.FieldNames.Status.ToString ( ) ) == true )
      {
        var stateValue = PageCommand.GetParameter<EdRecordObjectStates> ( EdRecord.FieldNames.Status.ToString ( ) );

        if ( this.Session.EntityStateSelection != stateValue )
        {
          if ( stateValue != EdRecordObjectStates.Null )
          {
            this.AdapterObjects.AllEntityLayouts = new List<EdRecord> ( );
            this.Session.EntityStateSelection = stateValue;
          }
          else
          {
            this.AdapterObjects.AllEntityLayouts = new List<EdRecord> ( );
            this.Session.EntityStateSelection = EdRecordObjectStates.Null;
          }
        }
      }
      this.LogValue ( "EntityStateSelection: " + this.Session.EntityStateSelection );

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
      // Interate through the selection filters and save the value in the session entity selection filters array.
      //
      for ( int filterIndex = 0; filterIndex < this.Session.EntitySelectionFilters.Length; filterIndex++ )
      {
        //
        // reset the filter value.
        //
        this.Session.EntitySelectionFilters [ filterIndex ] = String.Empty;

        //
        // create the filter's parameter name.
        //
        String parameterName = EuEntities.CONST_SELECTION_FIELD + filterIndex;

        //
        // extract the filter's parameter value.
        //
        String value = PageCommand.GetParameter ( parameterName );

        //
        // if the value is not empty update the Entity selection filter.
        //
        if ( value != String.Empty )
        {
          this.Session.EntitySelectionFilters [ filterIndex ] = value;
          this.LogValue ( "EntitySelectionFilters {0} = '{1}'", filterIndex, this.Session.EntitySelectionFilters [ filterIndex ] );
        }

      }//END iteration loop

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
      this.LogDebug ( "PageCommand: {0}.", PageCommand.getAsString ( false, true ) );
      this.LogDebug ( "Entity_SelectedLayoutId: {0}.", this.Session.Selected_EntityLayoutId );
      try
      {
        //
        // Initialiset the methods variables and objects.
        //
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );

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
        this.ParentLayoutId = this.Session.Entity.LayoutId;
        this.ParentGuid = this.Session.Entity.Guid;
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
        this.Session.Entity = this.CreateNewEntity ( LayoutId, this.ParentGuid );

        if ( this.Session.Entity.Guid == Guid.Empty )
        {
          this.ErrorMessage = EdLabels.Form_Record_Creation_Error_Message;

          this.LogError ( EvEventCodes.Database_Record_Retrieval_Error,
            this.ErrorMessage );

          return this.Session.LastPage;
        }


        this.LogDebug ( "CREATED Entity Id: " + this.Session.Entity.EntityId );

        // 
        // Initialise the client object.
        // 
        clientDataObject.Page.Id = clientDataObject.Id;
        clientDataObject.Page.PageDataGuid = clientDataObject.Id;
        clientDataObject.Page.PageId = this.Session.Entity.LayoutId;
        clientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

        clientDataObject.Id = this.Session.Entity.Guid;
        clientDataObject.Title = this.Session.Entity.CommandTitle;

        if ( this.AdapterObjects.Settings.UseHomePageHeaderOnAllPages == true )
        {
          clientDataObject.Title = this.AdapterObjects.Settings.HomePageHeaderText;
        }

        clientDataObject.Page.Title = clientDataObject.Title;
        this.LogDebug
          ( "Title.Length: " + clientDataObject.Title.Length );

        clientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Disabled;

        // 
        // Generate the new page layout 
        // 
        this.getEntityClientData ( clientDataObject.Page );

        this.Session.EntityList = new List<EdRecord> ( );
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
    /// <returns>Evado.Model.Digital.EdRecord</returns>
    //  ----------------------------------------------------------------------------------
    private EdRecord CreateNewEntity (
      String LayoutId,
      Guid ParentGuid )
    {
      this.LogMethod ( "CreateNewEntity" );
      this.LogDebug ( "LayoutId {0}.", LayoutId );
      this.LogDebug ( "ParentGuid {0}.", ParentGuid );
      this.LogDebug ( this.Session.UserProfile.getUserProfile ( true ) );
      //
      // Initialiset the methods variables and objects.
      //
      Evado.Model.Digital.EdRecord newRecord = new Evado.Model.Digital.EdRecord ( );

      newRecord.Guid = Guid.NewGuid ( );
      newRecord.LayoutId = LayoutId;
      newRecord.AuthorUserId = this.Session.UserProfile.UserId;
      newRecord.RecordDate = DateTime.Now;

      this.Session.EntityLayout = this.AdapterObjects.GetEntityLayout ( LayoutId );

      if ( this.Session.EntityLayout.hasEditAccess ( this.Session.UserProfile.Roles ) == false )
      {
        this.LogDebug ( "User does not have create access." );
        this.LogMethodEnd ( "CreateNewEntity" );
        return new EdRecord ( );
      }
      //
      // Set the new entities parents 
      //
      switch ( this.Session.EntityLayout.Design.ParentType )
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
      var entity = this._Bll_Entities.CreateEntity ( newRecord );

      this.LogClass ( this._Bll_Entities.Log );

      this.LogMethodEnd ( "CreateNewEntity" );
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
    /// <returns>Evado.Model.UniForm.AppData</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData updateObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      try
      {
        this.LogMethod ( "updateObject" );
        this.LogValue ( "PageCommand: " + PageCommand.getAsString ( false, false ) );

        this.LogDebug ( "Record.Guid: " + this.Session.Entity.Guid );
        this.LogDebug ( "Title: {0} ", this.Session.Entity.Title );
        this.LogDebug ( "RecordId: {0} ", this.Session.Entity.RecordId );
        this.LogDebug ( "FormAccessRole: {0} ", this.Session.Entity.FormAccessRole );

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
        if ( this.Session.PageId == EdStaticPageIds.Entity_User_Access_Page.ToString ( ) )
        {
          this.updateObject_EntityAccessValues ( PageCommand );
        }
        else 
        if ( this.Session.PageId == EdStaticPageIds.Record_Admin_Page.ToString ( ) )
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
        this.Session.Entity.SaveAction =
           Evado.Model.EvStatics.parseEnumValue<EdRecord.SaveActionCodes> ( stSaveAction );

        this.LogDebug ( "Command Save Action: " + this.Session.Entity.SaveAction );

        // 
        // Resets the save action to the parameter passed in the page groupCommand.
        // 
        switch ( this.Session.Entity.SaveAction )
        {
          case EdRecord.SaveActionCodes.Save:
            {
              this.Session.Entity.SaveAction = EdRecord.SaveActionCodes.Save_Record;
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

                this.Session.Entity.SaveAction = EdRecord.SaveActionCodes.Save;
              }
              break;
            }
        }
        this.LogDebug ( "Actual Action: " + this.Session.Entity.SaveAction );

        // 
        // Execute the save record groupCommand to save the record values to the 
        // Evado database.
        // 
        EvEventCodes result = this._Bll_Entities.UpdateEntity (
          this.Session.Entity );

        this.LogClass ( this._Bll_Entities.Log );

        // 
        // If an error state is returned log the event.
        // 
        if ( result != EvEventCodes.Ok )
        {
          string stEvent = this._Bll_Entities.Log + " returned error message: "
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

        if ( this.Session.Entity.Design.ParentType == EdRecord.ParentTypeList.Entity )
        {
          this.LogDebug ( "Refreshing Entity Children" );
          this.Session.RefreshEntityChildren = true;
        }

        //
        // Force a refresh of hte form record list.
        //
        this.Session.EntityList = new List<EdRecord> ( );
        this.Session.Entity.ButtonEditModeEnabled = false;

       this.LogDebug( "Entity List Count {0}.", this.Session.EntityList.Count );

        if ( this.Session.Entity.SaveAction == EdRecord.SaveActionCodes.Layout_Approved )
        {
          this.AdapterObjects.AllEntityLayouts = new List<EdRecord> ( );
          this.AdapterObjects.PageComponents = new List<EvOption> ( );
        }
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

      String stDate = PageCommand.GetParameter ( EdRecord.FieldNames.RecordDate );
      this.Session.Entity.RecordDate = Evado.Model.Digital.EvcStatics.getDateTime ( stDate );

      String stState = PageCommand.GetParameter ( EdRecord.FieldNames.Status );
      this.Session.Entity.State = Evado.Model.EvStatics.parseEnumValue<EdRecordObjectStates> ( stState );

    }//END updateObject_AdminValues method.

    // ==================================================================================
    /// <summary>
    /// THis method updates the form record values with the groupCommand parameter values.
    /// </summary>
    /// <param name="PageCommand"> Evado.Model.UniForm.Command object.</param>
    //  ----------------------------------------------------------------------------------
    private void updateObject_EntityAccessValues (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "updateObject_UserAccessValues" );
      this.LogValue ( "Parameters.Count: " + PageCommand.Parameters.Count );

      if ( PageCommand.hasParameter ( EdRecord.FieldNames.EntityAccess ) == true )
      {
        this.Session.Entity.EntityAccess = PageCommand.GetParameter ( EdRecord.FieldNames.EntityAccess );
      }

      this.LogValue ( "Entity.EntityAccess: " + this.Session.Entity.EntityAccess );
      this.LogMethodEnd ( "updateObject_UserAccessValues" );

    }//END updateObject_UserAccessValues method.

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
        this.Session.Entity );

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
      foreach ( EdRecordField field in this.Session.Entity.Fields )
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
      foreach ( EdRecordField field in this.Session.Entity.Fields )
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