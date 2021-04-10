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
  public class EuEntities : EuClassAdapterBase
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
      if ( this.Session.SelectedOrganisationCountry == null )
      {
        this.Session.SelectedOrganisationCountry = String.Empty;
      }
      if ( this.Session.SelectedOrganisationCity == null )
      {
        this.Session.SelectedOrganisationCity = String.Empty;
      }
      if ( this.Session.SelectedOrganisationPostCode == null )
      {
        this.Session.SelectedOrganisationPostCode = String.Empty;
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
    private const string CONST_SELECTION_FIELD = "SFID_";
    /// <summary>
    /// This constand definee the include test sites property identifier
    /// </summary>
    private const string CONST_EDIT_MODE_FIELD = "EM01";

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
      this.LogValue ( "EnableEntityEditButtonUpdate: " + this.EnableEntityEditButtonUpdate );
      this.LogValue ( "EnableEntitySaveButtonUpdate: " + this.EnableEntitySaveButtonUpdate );

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

              switch ( this.Session.StaticPageId )
              {
                case EdStaticPageIds.Entity_Export_Page:
                  {
                    clientDataObject = this.getRecordExport_Object ( PageCommand );
                    break;
                  }
                case EdStaticPageIds.Entity_Query_View:
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
      this.LogValue ( "EntitySelectionLayoutId: " + this.Session.Selected_EntityLayoutId );
      this.LogValue ( "Entity.ReadAccessRoles: " + this.Session.EntityLayout.Design.ReadAccessRoles );
      this.LogValue ( "Entity.EditAccessRoles: " + this.Session.EntityLayout.Design.EditAccessRoles );
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
        this.executeRecordQuery ( );

        // 
        // Create the new pageMenuGroup for query selection.
        // 
        this.getQueryList_SelectionGroup ( PageObject );

        // 
        // Create the pageMenuGroup containing commands to open the records.
        //         
        this.getEntity_ListGroup ( PageObject );

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
        this.Session.SelectedOrganisationCountry = PageCommand.GetParameter ( EdOrganisation.FieldNames.Address_Country );
      }
      this.LogValue ( "SelectedOrganisationCountry: " + this.Session.SelectedOrganisationCountry );

      //
      // if the selected organisation city exists updated its value.
      //
      if ( PageCommand.hasParameter ( EdOrganisation.FieldNames.Address_City ) == true )
      {
        this.Session.SelectedOrganisationCity = PageCommand.GetParameter ( EdOrganisation.FieldNames.Address_City );
      }
      this.LogValue ( "SelectedOrganisationCity: " + this.Session.SelectedOrganisationCity );

      //
      // if the selected organisation city exists updated its value.
      //
      if ( PageCommand.hasParameter ( EdOrganisation.FieldNames.Address_Post_Code ) == true )
      {
        this.Session.SelectedOrganisationPostCode = PageCommand.GetParameter ( EdOrganisation.FieldNames.Address_Post_Code );
      }
      this.LogValue ( "SelectedOrganisationPostCode: " + this.Session.SelectedOrganisationPostCode );



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
      this.LogValue ( "EntitySelectionState: " + this.Session.EntityStateSelection );
      this.LogValue ( "EntitySelectionLayoutId: " + this.Session.Selected_EntityLayoutId );
      this.LogValue ( "EntityLayout.ReadAccessRoles: " + this.Session.EntityLayout.Design.ReadAccessRoles );
      this.LogValue ( "UserProfile.Roles: " + this.Session.UserProfile.Roles );
      try
      {
        // 
        // Initialise the methods variables and objects.
        // 
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );

        //
        // get the selected entity.
        //
        this.Session.EntityLayout = this.AdapterObjects.GetEntityLayout ( this.Session.Selected_EntityLayoutId );

        // 
        // If the user does not have monitor or ResultData manager roles exit the page.
        // 
        if ( this.Session.EntityLayout.hasReadAccess ( this.Session.UserProfile.Roles ) == false )
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

        clientDataObject.Page.Title = clientDataObject.Title;
        clientDataObject.Page.PageId = EdStaticPageIds.Records_View.ToString ( );

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
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getList_SelectionGroup" );
      this.LogDebug ( "IssuedEntityLayouts.Count {0}. ", this.AdapterObjects.IssuedEntityLayouts.Count );
      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      List<EvOption> optionList;
      Evado.Model.UniForm.Field selectionField;

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
        Evado.Model.UniForm.EditAccess.Enabled );
      pageGroup.GroupType = Evado.Model.UniForm.GroupTypes.Default;
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
      pageGroup.AddParameter ( Model.UniForm.GroupParameterList.Offline_Hide_Group, true );

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
      selectionField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );

      // 
      // Add the record state selection option
      // 
      optionList = EdRecord.getRecordStates ( false );

      selectionField = pageGroup.createSelectionListField (
        EdRecord.FieldNames.Status.ToString ( ),
        EdLabels.Record_State_Selection,
        this.Session.EntityStateSelection,
        optionList );

      selectionField.Layout = EuAdapter.DefaultFieldLayout;
      selectionField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );

      // 
      // Add the selection groupCommand
      // 
      Evado.Model.UniForm.Command selectionCommand = pageGroup.addCommand (
        EdLabels.Select_Records_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Entities.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Custom_Method );

      selectionCommand.setCustomMethod ( Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

    }//ENd getList_SelectionGroup method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object using field filters.
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  -----------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData GetFilteredListObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "GetFilteredListObject" );
      this.LogDebug ( "EntitySelectionState: " + this.Session.EntityStateSelection );
      this.LogDebug ( "EntitySelectionLayoutId: " + this.Session.Selected_EntityLayoutId );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
      EdQueryParameters queryParameters = new EdQueryParameters ( );
      List<EdRecord> recordList = new List<EdRecord> ( );
      this.LogDebug ( "SelectedOrganisationCountry: " + this.Session.SelectedOrganisationCountry );
      this.LogDebug ( "SelectedOrganisationCity: " + this.Session.SelectedOrganisationCity );
      this.LogDebug ( "SelectedOrganisationPostCode: " + this.Session.SelectedOrganisationPostCode );

      try
      {
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
          return this.Session.LastPage; ;
        }

        // 
        // Log the user's access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "GetFilteredListObject",
          this.Session.UserProfile );

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

        clientDataObject.Page.Title = clientDataObject.Title;
        clientDataObject.Page.PageId = EdStaticPageIds.Records_View.ToString ( );

        // 
        // Create the new pageMenuGroup for query selection.
        // 
        this.getQueryList_SelectionGroup (
          clientDataObject.Page );

        // 
        // Create the pageMenuGroup containing commands to open the records.
        //         
        this.getEntity_ListGroup (
          clientDataObject.Page );

        this.LogValue ( "data.Page.Title: " + clientDataObject.Page.Title );


        this.LogMethodEnd ( "GetFilteredListObject" );
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

      this.LogMethodEnd ( "GetFilteredListObject" );
      return this.Session.LastPage; ;

    }//END GetFilteredListObject method.

    // ==============================================================================
    /// <summary>
    /// This method creates the record view pageMenuGroup containing a list of commands to 
    /// open the form record.
    /// </summary>
    /// <param name="PageObject">Evado.Model.Uniform.Page object to add the pageMenuGroup to.</param>
    /// <param name="subjects">EvSubjects subjects to add to selection groups</param>
    /// <param name="subjectVisits">EvSubjectMilestones visits for each subject</param>
    /// <param name="QueryParameters">EvQueryParameters: conting the query parameters</param>
    /// <param name="ApplicationObject">Adapter.ApplicationObjects object.</param>
    //  ------------------------------------------------------------------------------
    private void getQueryList_SelectionGroup (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getQueryList_SelectionGroup" );
      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      List<EvOption> optionList;
      Evado.Model.UniForm.Field groupField;

      // 
      // Create the new pageMenuGroup for record selection.
      // 
      pageGroup = PageObject.AddGroup (
        EdLabels.Entities_Selection_Group_Title,
        Evado.Model.UniForm.EditAccess.Enabled );
      pageGroup.GroupType = Evado.Model.UniForm.GroupTypes.Default;
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
      pageGroup.AddParameter ( Model.UniForm.GroupParameterList.Offline_Hide_Group, true );

      //
      // if the entity is not selected display an entity selection field.
      //
      if ( this.Session.Selected_EntityLayoutId == String.Empty )
      {
        // 
        // Add the record state selection option
        //
        optionList = new List<EvOption> ( );
        optionList.Add ( new EvOption ( ) );
        foreach ( EdRecord layout in this.AdapterObjects.IssuedEntityLayouts )
        {
          optionList.Add ( layout.SelectionOption );
        }

        groupField = pageGroup.createSelectionListField (
          EdRecord.FieldNames.Layout_Id,
          EdLabels.Label_Form_Id,
          this.Session.Selected_EntityLayoutId,
          optionList );

        groupField.Layout = EuAdapter.DefaultFieldLayout;
        groupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );

      }//END layoutId selection

      //
      // if the entity layout id has been defined display the entity selection fields.
      //
      else
      {
        //
        // retrieve the selected entity layout object
        //
        this.queryLayout = this.AdapterObjects.GetEntityLayout ( this.Session.Selected_EntityLayoutId );

        this.LogDebug ( "E: {0} S: {1}.", queryLayout.LayoutId, queryLayout.State );
        this.LogDebug ( "Entity {0}. ", queryLayout.CommandTitle );



        //
        // Insert the static filter selection for organisation city.
        //
        if ( this.AdapterObjects.Settings.StaticQueryFilterOptions.Contains ( EdOrganisation.FieldNames.Address_Country.ToString ( ) ) == true )
        {
          this.getQueryList_StaticOrgFilter ( pageGroup, EdOrganisation.FieldNames.Address_Country );
        }
        if ( this.AdapterObjects.Settings.StaticQueryFilterOptions.Contains ( EdOrganisation.FieldNames.Address_City.ToString ( ) ) == true )
        {
          this.getQueryList_StaticOrgFilter ( pageGroup, EdOrganisation.FieldNames.Address_City );
        }
        if ( this.AdapterObjects.Settings.StaticQueryFilterOptions.Contains ( EdOrganisation.FieldNames.Address_Post_Code.ToString ( ) ) == true )
        {
          this.getQueryList_StaticOrgFilter ( pageGroup, EdOrganisation.FieldNames.Address_Post_Code );
        }

        //
        // iterate through the filter field ids and display the filter field in the selection group.
        //
        for ( int filterIndex = 0; filterIndex < queryLayout.FilterFieldIds.Length; filterIndex++ )
        {
          string fieldId = queryLayout.FilterFieldIds [ filterIndex ];
          //this.LogDebug ( "Index {0}, FieldId {1}. ", filterIndex, fieldId );

          if ( fieldId == String.Empty )
          {
            continue;
          }

          //
          // retrieve the matching field object.
          //
          EdRecordField field = queryLayout.GetFieldObject ( fieldId );

          //
          // retrieve the current selection filter value.
          //
          string selectionFilter = this.Session.EntitySelectionFilters [ filterIndex ];

          //
          // create the selection field object for the selected field.
          //
          this.getQueryList_SelectionField (
            pageGroup,
            filterIndex,
            selectionFilter,
            field );
        }//END of the Selection filter iteration loop.

      }//END display enity filter fields.

      // 
      // Add the selection groupCommand
      // 
      Evado.Model.UniForm.Command selectionCommand = pageGroup.addCommand (
        EdLabels.Entities_Selection_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Entities.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Custom_Method );

      selectionCommand.setCustomMethod ( Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

      selectionCommand.SetPageId ( EdStaticPageIds.Entity_Query_View );
      ;
      this.LogDebug ( "Group Command Count {0}. ", pageGroup.CommandList.Count );
      this.LogMethodEnd ( "getQueryList_SelectionGroup" );

    }//ENd getQueryList_SelectionGroup method

    // ==============================================================================
    /// <summary>
    /// This method creates an entity filter selection field object.
    /// </summary>
    /// <param name="PageGroup">Evado.Model.Uniform.Group object to add the pageGroup .</param>
    /// <param name="FieldName">EdOrganisation.FieldNames enumerated list value</param>
    //  ------------------------------------------------------------------------------
    private void getQueryList_StaticOrgFilter (
      Evado.Model.UniForm.Group PageGroup,
      EdOrganisation.FieldNames FieldName )
    {
      this.LogMethod ( "getQueryList_StaticOrgFilter" );
      this.LogDebug ( "FieldName: {0}. ", FieldName );
      //
      // Initialise the methods variables and objects.
      //
      List<EvOption> optionList = new List<EvOption> ( );
      optionList.Add ( new EvOption ( ) );

      //
      // Select the field to be displayed.
      //
      switch ( FieldName )
      {
        case EdOrganisation.FieldNames.Address_Country:
          {
            optionList = this.AdapterObjects.GetOrganisationFilterList ( EdOrganisation.FieldNames.Address_Country, true );

            var field = PageGroup.createSelectionListField (
              FieldName,
              EdLabels.Organisation_Address_Country_Field_Label,
              this.Session.SelectedOrganisationCountry,
              optionList );
            field.Layout = EuAdapter.DefaultFieldLayout;
            field.AddParameter ( Evado.Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );
            break;
          }
        case EdOrganisation.FieldNames.Address_Post_Code:
          {
            optionList = this.AdapterObjects.GetOrganisationFilterList ( EdOrganisation.FieldNames.Address_Post_Code, true );

            var field = PageGroup.createSelectionListField (
              FieldName,
              EdLabels.Organisation_Address_PostCode_Field_Label,
              this.Session.SelectedOrganisationPostCode,
              optionList );
            field.Layout = EuAdapter.DefaultFieldLayout;
            field.AddParameter ( Evado.Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );
            break;
          }
        case EdOrganisation.FieldNames.Address_City:
          {
            optionList = this.AdapterObjects.GetOrganisationFilterList ( EdOrganisation.FieldNames.Address_City, true );

            var field = PageGroup.createSelectionListField (
              FieldName,
              EdLabels.Organisation_Address_City_Field_Label,
              this.Session.SelectedOrganisationCity,
              optionList );
            field.Layout = EuAdapter.DefaultFieldLayout;
            field.AddParameter ( Evado.Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );
            break;
          }
      }
      this.LogMethodEnd ( "getQueryList_StaticOrgFilter" );
    }

    // ==============================================================================
    /// <summary>
    /// This method creates an entity filter selection field object.
    /// </summary>
    /// <param name="PageGroup">Evado.Model.Uniform.Group object to add the pageGroup .</param>
    /// <param name="FieldName">EdOrganisation.FieldNames enumerated list value</param>
    //  ------------------------------------------------------------------------------
    private void getQueryList_StaticUserFilter (
      Evado.Model.UniForm.Group PageGroup,
      EdUserProfile.FieldNames FieldName )
    {
      this.LogMethod ( "getQueryList_StaticUserFilter" );
      this.LogDebug ( "FieldName: {0}. ", FieldName );
      //
      // Initialise the methods variables and objects.
      //
      String userSelectionList = this.AdapterObjects.Settings.UserCategoryList;
      List<EvOption> optionList = new List<EvOption> ( );
      optionList.Add ( new EvOption ( ) );

      //
      // Select the field to be displayed.
      //
      switch ( FieldName )
      {
        case EdUserProfile.FieldNames.User_Category:
          {
            optionList = this.AdapterObjects.getSelectionOptions ( userSelectionList, String.Empty, true, true );

            var field = PageGroup.createSelectionListField (
              FieldName,
              EdLabels.UserProfile_User_Category_Field_Label,
              this.Session.SelectedUserCategory,
              optionList );
            field.Layout = EuAdapter.DefaultFieldLayout;
            field.AddParameter ( Evado.Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );
            break;
          }
        case EdUserProfile.FieldNames.User_Type:
          {
            optionList = this.AdapterObjects.getSelectionOptions ( userSelectionList, String.Empty, false, true );

            var field = PageGroup.createSelectionListField (
              FieldName,
              EdLabels.UserProfile_User_Type_Field_Label,
              this.Session.SelectedUserType,
              optionList );
            field.Layout = EuAdapter.DefaultFieldLayout;
            field.AddParameter ( Evado.Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );
            break;
          }
      }
      this.LogMethodEnd ( "getQueryList_StaticUserFilter" );
    }

    // ==============================================================================
    /// <summary>
    /// This method creates an entity filter selection field object.
    /// </summary>
    /// <param name="PageGroup">Evado.Model.Uniform.Group object to add the pageGroup .</param>
    /// <param name="FilterIndex">Integer: filter index</param>
    /// <param name="SelectionFilter">String: filter value </param>
    /// <param name="Field">EdRecordField object.</param>
    //  ------------------------------------------------------------------------------
    private void getQueryList_SelectionField (
      Evado.Model.UniForm.Group PageGroup,
      int FilterIndex,
      String SelectionFilter,
      EdRecordField Field )
    {
      this.LogMethod ( "getQueryList_SelectionField" );

      this.LogDebug ( "FilterIndex: {0}, SelectionFilter: {1}. ", FilterIndex, SelectionFilter );
      this.LogDebug ( "F: {0}, T: {1}, Type {2}. ", Field.FieldId, Field.Title, Field.TypeId );

      List<EvOption> optionList = Evado.Model.UniForm.EuStatics.getStringAsOptionList (
        Field.Design.Options );

      List<EvOption> selectionOptionList = new List<EvOption> ( );
      selectionOptionList.Add ( new EvOption ( ) );
      foreach ( EvOption opt in optionList )
      {
        selectionOptionList.Add ( opt );
      }

      //
      // user the switch to select the selection data types.
      //
      switch ( Field.TypeId )
      {
        case EvDataTypes.Check_Box_List:
          {
            var field = PageGroup.createCheckBoxListField (
              EuEntities.CONST_SELECTION_FIELD + FilterIndex,
              Field.Title,
              SelectionFilter,
              optionList );
            field.Layout = EuAdapter.DefaultFieldLayout;
            field.AddParameter ( Evado.Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );

            break;
          }
        case EvDataTypes.Yes_No:
        case EvDataTypes.Boolean:
          {
            var field = PageGroup.createBooleanField (
              EuEntities.CONST_SELECTION_FIELD + FilterIndex,
              Field.Title,
              EvStatics.getBool ( SelectionFilter ) );
            field.Layout = EuAdapter.DefaultFieldLayout;
            field.AddParameter ( Evado.Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );

            break;
          }
        case EvDataTypes.Selection_List:
        case EvDataTypes.Radio_Button_List:
          {
            var field = PageGroup.createSelectionListField (
              EuEntities.CONST_SELECTION_FIELD + FilterIndex,
              Field.Title,
              SelectionFilter,
              selectionOptionList );
            field.Layout = EuAdapter.DefaultFieldLayout;
            field.AddParameter ( Evado.Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );

            break;
          }
        case EvDataTypes.External_Selection_List:
        case EvDataTypes.External_RadioButton_List:
          {
            this.LogDebug ( "External_Selection_List filter" );
            selectionOptionList = this.getQueryList_SelectionOptions ( Field, true );

            if ( selectionOptionList.Count <= 1 )
            {
              this.LogDebug ( "No Selection list options" );
              break;
            }

            var field = PageGroup.createSelectionListField (
              EuEntities.CONST_SELECTION_FIELD + FilterIndex,
              Field.Title,
              SelectionFilter,
              selectionOptionList );
            field.Layout = EuAdapter.DefaultFieldLayout;
            field.AddParameter ( Evado.Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );
            break;
          }
        case EvDataTypes.External_CheckBox_List:
          {
            this.LogDebug ( "External_CheckBox_List filter" );
            selectionOptionList = this.getQueryList_SelectionOptions ( Field, false );

            if ( selectionOptionList.Count == 0 )
            {
              this.LogDebug ( "No CheckBox list options" );
              break;
            }

            var field = PageGroup.createCheckBoxListField (
              EuEntities.CONST_SELECTION_FIELD + FilterIndex,
              Field.Title,
              SelectionFilter,
              selectionOptionList );
            field.Layout = EuAdapter.DefaultFieldLayout;
            field.AddParameter ( Evado.Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );

            break;
          }
      }//END switch statment

      this.LogMethodEnd ( "getQueryList_SelectionField" );
    }//END getQueryList_SelectionField Query

    // ==============================================================================
    /// <summary>
    /// This method creates the record view pageMenuGroup containing a list of commands to 
    /// open the form record.
    /// </summary>
    /// <param name="Field">EdRecordField object.</param>
    /// <param name="IsSelectionList">bool: True - is for a selection list.</param>
    //  ------------------------------------------------------------------------------
    private List<EvOption> getQueryList_SelectionOptions (
      EdRecordField Field,
      bool IsSelectionList )
    {
      this.LogMethod ( "getQueryList_SelectionOptions" );
      //
      // initialise the variables and objects.
      //
      List<EvOption> optionList = new List<EvOption> ( );
      String listId = Field.Design.ExSelectionListId;
      String category = Field.Design.ExSelectionListCategory.ToUpper ( );
      this.LogDebug ( "Field: {0}, List: {1}, Category: {2} ", Field.FieldId, listId, category );

      //
      // the category contains the category field then set the category value to this field value.
      //
      if ( category.Contains ( EdRecordField.CONST_CATEGORY_AUTI_FIELD_IDENTIFIER ) == true )
      {
        var autoCategory = category.Replace ( EdRecordField.CONST_CATEGORY_AUTI_FIELD_IDENTIFIER, String.Empty );

        this.LogDebug ( "autoCategory: {0} ", autoCategory );
        //
        // search the filterfields for a matching field identifier then retrieve the matching filter value
        // as the category for this selection list.
        //
        for ( int index = 0; index < this.queryLayout.FilterFieldIds.Length; index++ )
        {
          if ( this.queryLayout.FilterFieldIds [ index ] == autoCategory )
          {
            category = this.Session.EntitySelectionFilters [ index ];
          }
        }
        this.LogDebug ( "Auto Category: {0} ", category );

        //
        // the auto category selection value is empty exit.
        //
        if ( category == String.Empty )
        {
          return optionList;
        }
      }

      this.LogDebug ( "Parameters List: {0}, Category: {1} ", listId, category );
      //
      // get the external selection list options.
      //
      optionList = this.AdapterObjects.getSelectionOptions ( listId, category, false, IsSelectionList );

      this.LogDebug ( "optionList.Count: {0} ", optionList.Count );

      this.LogMethodEnd ( "getQueryList_SelectionOptions" );
      return optionList;

    }//ENd Method.

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
      this.LogDebug ( "EntityLayoutIdSelection: " + this.Session.Selected_EntityLayoutId );
      this.LogDebug ( "EntityTypeSelection: " + this.Session.EntityTypeSelection );
      this.LogDebug ( "EntityStateSelection: " + this.Session.EntityStateSelection );
      //
      // Initialise the methods variables and objects.
      //
      EdQueryParameters queryParameters = new EdQueryParameters ( );

      // 
      // Initialise the query values to the currently selected objects identifiers.
      // 
      queryParameters.Type = this.Session.EntityTypeSelection;
      queryParameters.LayoutId = this.Session.Selected_EntityLayoutId;

      //
      // pass the entity selection filters to the query.
      //
      queryParameters.SelectionFilters = this.Session.EntitySelectionFilters;

      // 
      // Initialise the query state selection.
      // 
      queryParameters.States.Add ( EuAdapter.CONST_RECORD_STATE_SELECTION_DEFAULT );
      queryParameters.NotSelectedState = true;

      if ( this.Session.EntityStateSelection != EdRecordObjectStates.Null )
      {
        queryParameters.States.Add ( this.Session.EntityStateSelection );
        queryParameters.NotSelectedState = false;
      }

      queryParameters.Org_City = this.Session.SelectedOrganisationCity;
      queryParameters.Org_Country = this.Session.SelectedOrganisationCountry;
      queryParameters.Org_PostCode = this.Session.SelectedOrganisationPostCode;

      if ( queryParameters.Org_City != String.Empty
        || queryParameters.Org_Country != String.Empty
        || queryParameters.Org_PostCode != String.Empty )
      {
        queryParameters.EnableOrganisationFilter = true;
      }

      this.LogDebug ( "Selected LayoutId: '" + queryParameters.LayoutId + "'" );
      this.LogDebug ( "Selected Org_City: '" + queryParameters.Org_City + "'" );
      this.LogDebug ( "Selected Org_Country: '" + queryParameters.Org_Country + "'" );
      this.LogDebug ( "Selected Org_PostCode: '" + queryParameters.Org_PostCode + "'" );

      // 
      // Query the database to retrieve a list of the records matching the query parameter values.
      // 
      if ( queryParameters.LayoutId != String.Empty )
      {
        this.LogDebug ( "Querying form records" );
        this.Session.EntityList = this._Bll_Entities.GetEntityList ( queryParameters );

        this.LogDebugClass ( this._Bll_Entities.Log );
      }
      this.LogDebug ( "EntityList.Count: " + this.Session.EntityList.Count );

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
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getRecord_ListGroup" );
      this.LogDebug ( "PageObject.EditAccess {0}.", PageObject.EditAccess );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Model.UniForm.Group ( );
      Evado.Model.UniForm.Command groupCommand = new Model.UniForm.Command ( );

      // 
      // Create the record display pageMenuGroup.
      // 
      pageGroup = PageObject.AddGroup (
        EdLabels.Entity_List_Group_Title );
      pageGroup.CmdLayout = Evado.Model.UniForm.GroupCommandListLayouts.Vertical_Orientation;

      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      if ( this.Session.EntityLayout.Title != String.Empty )
      {
        pageGroup.Title = String.Format (
          EdLabels.Entity_List_Title_Group_Title,
          this.Session.EntityLayout.Title );
      }

      if ( this.Session.EntityList.Count > 0 )
      {
        pageGroup.Title += EdLabels.List_Count_Label + this.Session.EntityList.Count;
      }

      //
      // Add a create record command.
      //
      if ( this.Session.Selected_EntityLayoutId != String.Empty
        && this.Session.PageId != EdStaticPageIds.Entity_Query_View.ToString ( )
        && pageGroup.EditAccess == Model.UniForm.EditAccess.Enabled )
      {
        groupCommand = pageGroup.addCommand (
          String.Format ( EdLabels.Entity_Create_New_List_Command_Title, this.Session.EntityLayout.Title ),
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Entities,
          Model.UniForm.ApplicationMethods.Create_Object );

        groupCommand.SetBackgroundDefaultColour ( Model.UniForm.Background_Colours.Purple );

        groupCommand.AddParameter ( Evado.Model.Digital.EdRecord.FieldNames.Layout_Id,
        this.Session.Selected_EntityLayoutId );
      }

      this.LogDebug ( "EntityList.Count: " + this.Session.EntityList.Count );
      // 
      // Iterate through the record list generating a groupCommand to access each record
      // then append the groupCommand to the record pageMenuGroup view's groupCommand list.
      // 
      foreach ( Evado.Model.Digital.EdRecord entity in this.Session.EntityList )
      {
        this.LogDebug ( "LCD {0}, LT {1}, FC {2}", entity.Design.LinkContentSetting, entity.CommandTitle, entity.Fields.Count );

        //
        // Create the group list groupCommand object.
        //
        this.getGroupListCommand (
          entity,
          pageGroup,
          EdRecord.LinkContentSetting.Null );

      }//END iteration loop

      this.LogValue ( "Group command count: " + pageGroup.CommandList.Count );

      this.LogMethodEnd ( "getRecord_ListGroup" );
    }//END createViewCommandList method

    // ==============================================================================
    /// <summary>
    /// This method appends the milestone groupCommand to the page milestone list pageMenuGroup
    /// </summary>
    /// <param name="CommandEntity">EvForm object</param>
    /// <param name="PageGroup"> Evado.Model.UniForm.Group</param>
    //  -----------------------------------------------------------------------------
    private Evado.Model.UniForm.Command getGroupListCommand (
      EdRecord CommandEntity,
      Evado.Model.UniForm.Group PageGroup,
      EdRecord.LinkContentSetting ParentLinkSetting )
    {
      this.LogMethod ( "getGroupListCommand" );
      this.LogDebug ( "CommandEntity.EntityId: " + CommandEntity.EntityId );
      this.LogDebug ( "LinkContentSetting: " + CommandEntity.Design.LinkContentSetting );
      this.LogDebug ( "ParentLinkSetting: " + ParentLinkSetting );

      //
      // Set the link setting.
      //
      if ( CommandEntity.Design.LinkContentSetting == EdRecord.LinkContentSetting.Null )
      {
        CommandEntity.Design.LinkContentSetting = ParentLinkSetting;
      }

      this.LogDebug ( "LinkContentSetting: " + CommandEntity.Design.LinkContentSetting );

      //
      // Define the pageMenuGroup groupCommand.
      //
      Evado.Model.UniForm.Command groupCommand = PageGroup.addCommand (
          CommandEntity.CommandTitle,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Entities,
          Evado.Model.UniForm.ApplicationMethods.Get_Object );

      groupCommand.Id = CommandEntity.Guid;
      groupCommand.SetGuid ( CommandEntity.Guid );

      groupCommand.AddParameter (
        Model.UniForm.CommandParameters.Short_Title,
        EdLabels.Label_Record_Id + CommandEntity.RecordId );
      if ( CommandEntity.ImageFileName != String.Empty )
      {
        string relativeURL = EuAdapter.CONST_IMAGE_FILE_DIRECTORY + CommandEntity.ImageFileName;
        groupCommand.AddParameter ( Model.UniForm.CommandParameters.Image_Url, relativeURL );
      }

      if ( this.Session.UserProfile.hasAdministrationAccess == true )
      {
        groupCommand.Title = CommandEntity.EntityId + " >> " + groupCommand.Title;
      }

      return groupCommand;

    }//END getGroupListCommand method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class private record export methods

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
        if ( this.Session.Entity.hasReadAccess ( this.Session.UserProfile.Roles ) == false )
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
        clientDataObject.Page.PageId = EdStaticPageIds.Record_Export_Page.ToString ( );

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
        EdRecord.FieldNames.Layout_Id,
        EdLabels.Label_Form_Id,
        this.Session.Selected_EntityLayoutId,
        this.Session.IssueFormList );

      selectionField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Define the include draft record selection option.
      //
      selectionField = pageGroup.createBooleanField (
        EuEntities.CONST_INCLUDE_DRAFT_RECORDS,
        EdLabels.Record_Export_Include_Draft_Record_Field_Title,
        this.Session.FormRecords_IncludeDraftRecords );
      selectionField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Define the include free text ResultData selection option.
      //
      selectionField = pageGroup.createBooleanField (
        EuEntities.CONST_INCLUDE_FREE_TEXT_DATA,
        EdLabels.Record_Export_Include_FreeText_data_Field_Title,
        this.Session.FormRecords_IncludeFreeTextData );
      selectionField.Layout = EuAdapter.DefaultFieldLayout;

      // 
      // Add the selection groupCommand
      // 
      command = pageGroup.addCommand (
        EdLabels.Record_Export_Selection_Group_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Entities.ToString ( ),
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
      int inResultCount = this._Bll_Entities.geyEntityCount ( queryParameters );

      this.LogClass ( this._Bll_Entities.Log );

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
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    /// <param name="EntityId">String: Entity identifier</param>
    //  ------------------------------------------------------------------------------
    public void getEntityClientData (
      Evado.Model.UniForm.Page PageObject,
      Evado.Model.UniForm.Command PageCommand,
      String EntityId )
    {
      this.LogMethod ( "getObject" );
      try
      {
        //
        // load a new entity if it is not loaded.
        //
        if ( this.Session.Entity.EntityId != EntityId )
        {
          Guid entityGuid = this._Bll_Entities.GetEntityGuid ( EntityId );

          this.LogDebug ( "Guid {0} == EntityId {1}: ", EntityId, entityGuid );

          PageCommand.SetGuid ( entityGuid );
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
            return;
          }

          this.LogDebug ( "EntityDictionary count {0}.", this.Session.EntityDictionary.Count );
          this.LogValue ( "Entity.EntityId: " + this.Session.Entity.EntityId );
        }

        // 
        // If the user does not have monitor or ResultData manager roles exit the page.
        // 
        if ( this.Session.Entity.hasReadAccess ( this.Session.UserProfile.Roles ) == false )
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
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData getObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getObject" );
      this.LogDebug ( "UserProfile.Roles: {0}.", this.Session.UserProfile.Roles );
      try
      {
        // 
        // Initialise the methods variables and objects.
        // 
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );

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
          return this.Session.LastPage;
        }

        this.LogDebug ( "EntityDictionary count {0}.", this.Session.EntityDictionary.Count );
        this.LogDebug ( "Entity.EntityId: {0}.", this.Session.Entity.EntityId );
        this.LogDebug ( "Entity.Design.ReadAccessRoles: {0}.", this.Session.Entity.Design.ReadAccessRoles );

        // 
        // If the user does not have monitor or ResultData manager roles exit the page.
        // 
        if ( this.Session.Entity.hasReadAccess ( this.Session.UserProfile.Roles ) == false )
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
        clientDataObject.Page.PageId = this.Session.Entity.LayoutId;
        clientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

        clientDataObject.Id = this.Session.Entity.Guid;
        clientDataObject.Title = this.Session.Entity.CommandTitle;

        if ( this.AdapterObjects.Settings.UseHomePageHeaderOnAllPages == true )
        {
          clientDataObject.Title = this.AdapterObjects.Settings.HomePageHeaderText;
        }


        clientDataObject.Page.Title = clientDataObject.Title;
        this.LogDebug ( "Title.Length: " + clientDataObject.Title.Length );

        clientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Disabled;


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
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    //  ---------------------------------------------------------------------------------
    private EvEventCodes GetEntity (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "GetEntity" );
      this.LogDebug ( "EntityDictionary count {0}.", this.Session.EntityDictionary.Count );

      //
      // if the page command has a layout identifier then assume parental identifier retrieval.
      //
      if ( PageCommand.hasParameter ( EdRecord.FieldNames.Layout_Id ) == true )
      {
        var result = this.GetEntityByParent ( PageCommand );

        this.LogMethodEnd ( "GetEntity" );
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
        && this.Session.Entity.Guid == Guid.Empty )
      {
        return EvEventCodes.Identifier_Global_Unique_Identifier_Error;
      }

      //
      // If the record Guid match then the record is loaded so exit.
      //
      if ( entityGuid == this.Session.Entity.Guid )
      {
        this.LogDebug ( "Entity Loaded" );
        this.LogMethodEnd ( "GetEntity" );
        return EvEventCodes.Ok;
      }

      //
      // Attempt to pull the entity from the entity dictionary.
      //
      this.Session.Entity = this.Session.PullEntity ( entityGuid );

      //
      // if the pull returned entity exists exit.
      //
      if ( this.Session.Entity != null )
      {
        this.LogDebug ( "Entity Loaded from dictionary" );
        this.LogMethodEnd ( "GetEntity" );
        return EvEventCodes.Ok;
      }

      // 
      // Retrieve the record object from the database via the DAL and BLL layers.
      // 
      this.Session.Entity = this._Bll_Entities.GetEntity ( entityGuid );

      this.LogClass ( this._Bll_Entities.Log );

      this.LogDebug ( "Entity {0}, Title {1}.", this.Session.Entity.RecordId, this.Session.Entity.Title );
      this.LogDebug ( "There are {0} of fields in the record.", this.Session.Entity.Fields.Count );

      //
      // return a retrieval error message if the resulting common record guid is empty.
      //
      if ( this.Session.Entity.Guid == Guid.Empty )
      {
        this.LogMethodEnd ( "GetEntity" );
        return EvEventCodes.Database_Record_Retrieval_Error;
      }

      //
      // push the new entity onto the directory list.
      //
      this.Session.PushEntity ( this.Session.Entity );

      this.LogMethodEnd ( "GetEntity" );
      return EvEventCodes.Ok;

    }//ENd GetEntity method

    //  =============================================================================== 
    /// <summary>
    /// This method retrieves any by its LayoutId and Parent identifiers.
    /// </summary>
    //  ---------------------------------------------------------------------------------
    private EvEventCodes GetEntityByParent (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "GetEntityByParent" );
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
        this.LogMethodEnd ( "GetEntityByParent" );
        return EvEventCodes.Identifier_General_ID_Error;
      }

      this.LogDebug ( "Retieving from Dictionary" );
      //
      // Attempt to pull the entity from the entity dictionary.
      //
      if ( parentGuid != Guid.Empty )
      {
        this.Session.Entity = this.Session.PullEntity ( layoutId, parentGuid );
      }
      else
      {
        this.Session.Entity = this.Session.PullEntity ( layoutId, orgId, userId );
      }

      //
      // if the pull returned entity exists exit.
      //
      if ( this.Session.Entity != null )
      {
        this.LogDebug ( "Entity Loaded from dictionary" );
        this.LogMethodEnd ( "GetEntityByParent" );
        return EvEventCodes.Ok;
      }

      //
      // Retrieve the record object from the database via the DAL and BLL layers.
      //
      if ( parentGuid != Guid.Empty )
      {
        this.LogDebug ( "Retrieving Entity by parental Guid" );

        this.Session.Entity = this._Bll_Entities.GetItemByParentGuid ( layoutId, parentGuid );
      }
      else
        if ( orgId != String.Empty )
        {
          this.LogDebug ( "Retrieving Entity by organisation parental identifier" );

          this.Session.Entity = this._Bll_Entities.GetItemByParentOrgId ( layoutId, orgId );
        }
        else
        {
          this.LogDebug ( "Retrieving Entity by organisation parental identifier" );

          this.Session.Entity = this._Bll_Entities.GetItemByParentUserId ( layoutId, userId );

          //
          // Create the new entity.
          //
          this.Session.Entity = this.CreateNewEntity ( layoutId, Guid.Empty );
        }

      this.LogClass ( this._Bll_Entities.Log );

      //
      // Create the new entity if non was created.
      //
      if ( this.Session.Entity.Guid == Guid.Empty )
      {
        this.Session.Entity = this.CreateNewEntity ( layoutId, parentGuid );
        this.LogDebug ( "Entity {0}, Title {1}.", this.Session.Entity.EntityId, this.Session.Entity.Title );
        this.LogDebug ( "There are {0} of fields in the record.", this.Session.Entity.Fields.Count );
      }

      //
      // return a retrieval error message if the resulting common record guid is empty.
      //
      if ( this.Session.Entity.Guid == Guid.Empty )
      {
        this.LogMethodEnd ( "GetEntityByParent" );
        return EvEventCodes.Database_Record_Retrieval_Error;
      }

      //
      // push the new entity onto the directory list.
      //
      this.Session.PushEntity ( this.Session.Entity );

      this.LogMethodEnd ( "GetEntityByParent" );
      return EvEventCodes.Ok;

    }//ENd GetEntity method

    //  =============================================================================== 
    /// <summary>
    ///  This method initiates the execution of the server side CS scripts.
    /// </summary>
    //  ---------------------------------------------------------------------------------
    private bool runServerScript (
      EvServerPageScript.ScripEventTypes ScriptType )
    {
      this.LogMethod ( "runServerScript" );
      this.LogValue ( "RecordId " + this.Session.Entity.RecordId );
      this.LogValue ( "hasCsScript = " + this.Session.Entity.Design.hasCsScript );

      // 
      // if the formField has a CS Script execute the onPostBackForm method.
      // 
      if ( this.Session.Entity.Design.hasCsScript == true )
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
          this.Session.Entity );

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
    /// <returns>Evado.Model.UniForm.Command object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.UniForm.Command createEntitySaveCommand (
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
        EuAdapterClasses.Entities.ToString ( ),
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
    /// <param name="PageObject">Evado.Model.UniForm.AppData object.</param>
    //  ------------------------------------------------------------------------------
    private void getEntityClientData (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getClientData" );
      this.LogDebug ( "UserProfile.Roles: " + this.Session.UserProfile.Roles );
      this.LogDebug ( "Entity.LayoutId: " + this.Session.Entity.LayoutId );
      this.LogDebug ( "Entity.Title: " + this.Session.Entity.Title );
      this.LogDebug ( "Entity.DefaultPageLayout: " + this.Session.Entity.Design.DefaultPageLayout );
      this.LogDebug ( "Entity.FieldReadonlyDisplayFormat: " + this.Session.Entity.Design.FieldReadonlyDisplayFormat );
      this.LogDebug ( "Entity.ReadAccessRoles: " + this.Session.Entity.Design.ReadAccessRoles );
      this.LogDebug ( "Entity.EditAccessRoles: " + this.Session.Entity.Design.EditAccessRoles );
      this.LogDebug ( "Entity.AuthorAccess: " + this.Session.Entity.Design.AuthorAccess );
      this.LogDebug ( "Entity.ParentType: " + this.Session.Entity.Design.ParentType );
      this.LogDebug ( "Entity.ParentGuid: " + this.Session.Entity.ParentGuid );
      this.LogDebug ( "Entity.ParentOrgId: " + this.Session.Entity.ParentOrgId );
      this.LogDebug ( "Entity.ParentUserId: " + this.Session.Entity.ParentUserId );
      this.LogDebug ( "EnableEntityEditButtonUpdate: " + this.EnableEntityEditButtonUpdate );
      this.LogDebug ( "ButtonEditModeEnabled: " + this.Session.Entity.ButtonEditModeEnabled );

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      Evado.Model.UniForm.Parameter parameter = new Evado.Model.UniForm.Parameter ( );

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
        pageGroup.EditAccess = Model.UniForm.EditAccess.Disabled;
        pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;

        groupField = pageGroup.createTextField (
          String.Empty,
          EdLabels.Entity_Entity_Identifier_Field_Title,
          this.Session.Entity.EntityId, 20 );

        groupField = pageGroup.createTextField (
          String.Empty,
          EdLabels.Entity_Entity_Identifier_Field_Title,
          this.Session.Entity.CommandTitle, 80 );
      }

      // 
      // Call the page generation method
      // 
      bool result = pageGenerator.generateLayout (
        this.Session.Entity,
        PageObject,
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
      // generate the group commands.
      //
      this.getDataObject_GroupCommands ( PageObject );

      //
      // if the entity has child entities add a group to display them.
      //
      this.getDataObject_ChildEntities ( PageObject );

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
      String stSubmitCommandTitle = EdLabels.Entity_Submit_Command;

      // 
      // Set the user access to the records content.
      // 
      this.Session.Entity.setUserAccess ( this.Session.UserProfile );
      this.LogDebug ( "Entity.FormAccessRole {0}. ", this.Session.Entity.FormAccessRole );

      if ( this.Session.Entity.FormAccessRole != EdRecord.FormAccessRoles.Record_Author )
      {
        this.LogMethodEnd ( "getDataObject_PageCommands" );
        return;
      }

      // 
      // Add the Edit command to the page.
      //
      if ( this.Session.Entity.ButtonEditModeEnabled == false
        && this.EnableEntityEditButtonUpdate == true
        && this.Session.Entity.State != EdRecordObjectStates.Empty_Record
        && this.Session.Entity.State != EdRecordObjectStates.Draft_Record )
      {
        pageCommand = PageObject.addCommand (
          EdLabels.Entity_Edit_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Entities.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Get_Object );

        pageCommand.SetGuid ( this.Session.Entity.Guid );

        pageCommand.AddParameter ( EuEntities.CONST_EDIT_MODE_FIELD, "yes" );

        this.Session.Entity.FormAccessRole = EdRecord.FormAccessRoles.Record_Reader;
      }
      else
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
        if ( this.EnableEntitySaveButtonUpdate == true )
        {
          pageCommand = PageObject.addCommand (
            EdLabels.Entity_Save_Command_Title,
            EuAdapter.ADAPTER_ID,
            EuAdapterClasses.Entities.ToString ( ),
            Evado.Model.UniForm.ApplicationMethods.Save_Object );

          pageCommand.SetGuid ( this.Session.Entity.Guid );

          pageCommand.AddParameter (
            Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
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
          EuAdapterClasses.Entities.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Save_Object );

        pageCommand.SetGuid ( this.Session.Entity.Guid );
        pageCommand.AddParameter (
          Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
         EdRecord.SaveActionCodes.Submit_Record.ToString ( ) );

        pageCommand.setEnableForMandatoryFields ( );

        // 
        // Add the wihdrawn groupCommand to the page groupCommand list.
        // 
        if ( ( this.Session.Entity.State == EdRecordObjectStates.Draft_Record
            || this.Session.Entity.State == EdRecordObjectStates.Empty_Record
            || this.Session.Entity.State == EdRecordObjectStates.Completed_Record ) )
        {
          pageCommand = PageObject.addCommand (
            EdLabels.Entity_Withdraw_Command,
            EuAdapter.ADAPTER_ID,
            EuAdapterClasses.Entities.ToString ( ),
            Evado.Model.UniForm.ApplicationMethods.Save_Object );

          pageCommand.SetGuid ( this.Session.Entity.Guid );
          pageCommand.AddParameter (
            Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
           EdRecord.SaveActionCodes.Withdrawn_Record.ToString ( ) );
        }

      }//END in edit mode.

      this.LogDebug ( "Session.Entity.FormAccessRole {0}. ", this.Session.Entity.FormAccessRole );
      this.LogMethodEnd ( "getDataObject_PageCommands" );

    }//END getClientData_SaveCommands method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_GroupCommands (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getDataObject_GroupCommands" );
      //
      // Display save command in the last group.
      //
      if ( this.Session.Entity.FormAccessRole != EdRecord.FormAccessRoles.Record_Author
        || PageObject.GroupList.Count == 0 )
      {
        this.LogMethodEnd ( "getDataObject_GroupCommands" );
        return;
      }

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );
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
          EuAdapterClasses.Entities.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Save_Object );

        pageCommand.SetGuid ( this.Session.Entity.Guid );

        pageCommand.AddParameter (
          Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
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
        EuAdapterClasses.Entities.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Save_Object );

      pageCommand.SetGuid ( this.Session.Entity.Guid );
      pageCommand.AddParameter (
        Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
       EdRecord.SaveActionCodes.Submit_Record.ToString ( ) );

      pageCommand.setEnableForMandatoryFields ( );

      // 
      // Add the wihdrawn groupCommand to the page groupCommand list.
      // 
      if ( ( this.Session.Entity.State == EdRecordObjectStates.Draft_Record
          || this.Session.Entity.State == EdRecordObjectStates.Empty_Record
          || this.Session.Entity.State == EdRecordObjectStates.Completed_Record ) )
      {
        pageCommand = pageGroup.addCommand (
          EdLabels.Entity_Withdraw_Command,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Entities.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Save_Object );

        pageCommand.SetGuid ( this.Session.Entity.Guid );
        pageCommand.AddParameter (
          Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
         EdRecord.SaveActionCodes.Withdrawn_Record.ToString ( ) );
      }

      this.LogMethodEnd ( "getDataObject_GroupCommands" );
    }

    private readonly string ChildEntityGroupID = "ChildEntities";

    // ==============================================================================
    /// <summary>
    /// This method displays the entity's chiled entities
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_ChildEntities (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getDataObject_ChildEntities" );
      this.LogDebug ( "DisplayRelatedEntities {0}.", this.Session.Entity.Design.DisplayRelatedEntities );
      this.LogDebug ( "Entity.ChildEntities.Count {0}.", this.Session.Entity.ChildEntities.Count );

      if ( this.Session.Entity.Design.DisplayRelatedEntities == false
        || this.Session.Entity.FormAccessRole != EdRecord.FormAccessRoles.Record_Reader )
      {
        this.LogDebug ( "No displaying related entities." );
        this.LogMethodEnd ( "getDataObject_ChildEntities" );
        return;
      }

      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.UniForm.Command groupCommand = new Evado.Model.UniForm.Command ( );
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );

      //
      // define the child entity group.
      //
      pageGroup = PageObject.AddGroup (
      String.Format( EdLabels.Entities_Child_Entity_Group_Title, this.Session.Entity.getFirstTextField( false ) ) );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;
      pageGroup.GroupId = this.ChildEntityGroupID;

      pageGroup.CmdLayout = Model.UniForm.GroupCommandListLayouts.Vertical_Orientation;

      if ( this.Session.Entity.ChildEntities.Count <= 20 )
      {
        //pageGroup.CmdLayout = Model.UniForm.GroupCommandListLayouts.Tiled_Commands;
        //pageGroup.AddParameter ( Model.UniForm.GroupParameterList.Command_Width, "20%" );
        //pageGroup.AddParameter ( Model.UniForm.GroupParameterList.Command_Height, "50px" );
      }

      //
      // Add a create record command.
      //
      var entityChildren = this.AdapterObjects.GetEntityChildren ( this.Session.Entity.LayoutId );

      this.LogDebug ( "Entity Children count {0}. ", entityChildren.Count );

      //
      // iterate through the list of children add a create commend for each child
      // if the user has edit access to the child entity.
      //
      foreach ( EdObjectParent child in entityChildren )
      {
        //
        // if the user had edit access toteh entity add a create command for the entity.
        //
        if ( this.Session.UserProfile.hasRole ( child.ChildEditAccess ) == true )
        {
          string title = String.Format ( EdLabels.Entity_New_Entity_Command_Title, child.ChildTitle );
          //
          // define the new entity command.
          //
          groupCommand = pageGroup.addCommand (
            title,
            EuAdapter.ADAPTER_ID,
            EuAdapterClasses.Entities,
            Model.UniForm.ApplicationMethods.Create_Object );

          groupCommand.AddParameter ( EdRecord.FieldNames.Layout_Id, child.ChildLayoutId );

          groupCommand.SetBackgroundDefaultColour ( Model.UniForm.Background_Colours.Purple );
        }//ENd edit access
      }//END child interation loop

      //
      // Iterate through the child entities
      //
      foreach ( EdRecord child in this.Session.Entity.ChildEntities )
      {
        this.LogDebug ( "entity {0}, {1}, L {2}, LT {3}, FC {4}.",
          child.EntityId,
          child.Title,
          child.Design.LinkContentSetting,
          child.CommandTitle,
          child.Fields.Count );

        this.getGroupListCommand (
         child,
         pageGroup,
         child.Design.LinkContentSetting );

      }

      this.LogDebug ( "Command Count {0}.", pageGroup.CommandList.Count );
      this.LogMethodEnd ( "getDataObject_ChildEntities" );

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
      this.LogDebug ( "Entity_SelectedLayoutId: " + this.Session.Selected_EntityLayoutId );
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
        Guid parentGuid = PageCommand.GetGuid ( );
        //
        // Create the new entity.
        //
        this.Session.Entity = this.CreateNewEntity ( LayoutId, parentGuid );

        if ( this.Session.Entity.Guid == Guid.Empty )
        {
          this.ErrorMessage = EdLabels.Form_Record_Creation_Error_Message;

          this.LogError ( EvEventCodes.Database_Record_Retrieval_Error,
            this.ErrorMessage );

          return this.Session.LastPage;
        }


        this.LogDebug ( "CREATED Record Id: " + this.Session.Entity.RecordId );

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
      //
      // Initialiset the methods variables and objects.
      //
      Evado.Model.Digital.EdRecord newRecord = new Evado.Model.Digital.EdRecord ( );

      newRecord.Guid = Guid.NewGuid ( );
      newRecord.LayoutId = LayoutId;
      newRecord.AuthorUserId = this.Session.UserProfile.UserId;
      newRecord.RecordDate = DateTime.Now;

      this.Session.EntityLayout = this.AdapterObjects.GetEntityLayout ( LayoutId );

      if ( this.Session.EntityLayout.hasEditAccess( this.Session.UserProfile.Roles ) == false )
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
        this.LogValue ( "Actual Action: " + this.Session.Entity.SaveAction );

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

        //
        // Force a refresh of hte form record list.
        //
        this.Session.EntityList = new List<EdRecord> ( );
        this.Session.Entity.ButtonEditModeEnabled = false;

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