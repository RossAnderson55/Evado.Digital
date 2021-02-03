/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\ApplicationProfile.cs" company="EVADO HOLDING PTY. LTD.">
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
  /// This class terminates the Organisation object.
  /// </summary>
  public class EdAdapterConfig : EuClassAdapterBase
  {
    #region Class Initialisation
    /// <summary>
    /// This method initialises the class.
    /// </summary>
    public EdAdapterConfig ( )
    {
      this.ClassNameSpace = "Evado.UniForm.Clinical.EdPlatformSettings.";
    }

    /// <summary>
    /// This method initialises the class and passs in the user profile.
    /// </summary>
    public EdAdapterConfig (
      EuAdapterObjects ApplicationObjects,
      EvUserProfileBase ServiceUserProfile,
      EuSession SessionObjects,
      String UniFormBinaryFilePath,
      EvClassParameters Settings )
    {
      this.GlobalObjects = ApplicationObjects;
      this.ServiceUserProfile = ServiceUserProfile;
      this.Session = SessionObjects;
      this.UniForm_BinaryFilePath = UniFormBinaryFilePath;
      this.ClassParameters = Settings;
      this.ClassNameSpace = "Evado.UniForm.Clinical.EdPlatformSettings.";


      this.LogInitMethod ( "EuApplicationSettings initialisation" );
      this.LogInit ( "-ServiceUserProfile.UserId: " + ServiceUserProfile.UserId );
      this.LogInit ( "-SessionObjects.UserProfile.Userid: " + this.Session.UserProfile.UserId );
      this.LogInit ( "-SessionObjects.UserProfile.CommonName: " + this.Session.UserProfile.CommonName );
      this.LogInit ( "-UniFormBinaryFilePath: " + this.UniForm_BinaryFilePath );
      this.LogInit ( "-Version: " + this.GlobalObjects.AdapterSettings.Version );

      this.LogInit ( "Settings:" );
      this.LogInit ( "-LoggingLevel: " + Settings.LoggingLevel );
      this.LogInit ( "-UserId: " + Settings.UserProfile.UserId );
      this.LogInit ( "-UserCommonName: " + Settings.UserProfile.CommonName );

      this._bll_AdapterConfig = new Bll.Clinical.EdAdapterConfig ( Settings );

    }//END Method


    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class constants and variables.

    private const String CONST_ADDRESS_FIELD_ID = "ADDRESS";
    private const String CONST_CURRENT_FIELD_ID = "CURRENT";

    private Evado.Bll.Clinical.EdAdapterConfig _bll_AdapterConfig = new Evado.Bll.Clinical.EdAdapterConfig ( );

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class public methods

    // ==================================================================================
    /// <summary>
    /// This method gets the trial site object.
    /// 
    /// </summary>
    /// <param name="PageCommand">ClientPateEvado.Model.UniForm.Command object</param>
    /// <returns>Evado.Model.UniForm.AppData</returns>
    //  ----------------------------------------------------------------------------------
    public Evado.Model.UniForm.AppData getClientDataObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getClientDataObject" );

      this.LogValue ( "PageCommand Content: " + PageCommand.getAsString ( false, false ) );
      try
      {
        // 
        // Initialise the methods variables and objects.
        // 
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );

        // 
        // Set the page type to control the DB query type.
        // 
        string pageType = PageCommand.GetPageId ( );

        this.Session.setPageId ( pageType );

        this.LogValue ( "PageType: " + this.Session.PageId );

        // 
        // Determine the method to be called
        // 
        switch ( PageCommand.Method )
        {
          case Evado.Model.UniForm.ApplicationMethods.List_of_Objects:
          case Evado.Model.UniForm.ApplicationMethods.Get_Object:
            {
              switch ( this.Session.PageId )
              {
                case EvPageIds.Database_Version:
                  {
                    clientDataObject = this.getDB_Update_Object ( PageCommand );
                    break;
                  }
                case EvPageIds.Audit_Configuration_Page:
                  {
                    clientDataObject = this.getConfig_Audit_Object ( PageCommand );
                    break;
                  }
                case EvPageIds.Audit_Records_Page:
                  {
                    clientDataObject = this.getRecord_Audit_Object ( PageCommand );
                    break;
                  }
                case EvPageIds.Audit_Record_Items_Page:
                  {
                    clientDataObject = this.getRecord_Item_Audit_Object ( PageCommand );
                    break;
                  }
                case EvPageIds.Application_Profile:
                default:
                  {
                    clientDataObject = this.getObject ( PageCommand );
                    break;
                  }
              }
              break;
            }
          case Evado.Model.UniForm.ApplicationMethods.Save_Object:
            {
              this.LogValue ( " Save Object method" );

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
          this.LogValue ( " null application data returned." );
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
        this.LogMethodEnd ( "getClientDataObject" );
        return clientDataObject;

      }
      catch ( Exception Ex )
      {
        this.LogException ( Ex );
      }
      this.LogMethodEnd ( "getClientDataObject" );
      return this.Session.LastPage;

    }//END getClientDataObject method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class get database update object methods

    private const string CONST_UPDATE_VERSION = "DBV";
    private const string CONST_UPDATE_ORDER = "DBO";

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData getDB_Update_Object (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getDB_Update_Object" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
      String parameterValue = String.Empty;
      this.Session.DatabaseUpdateVersion = EvDataBaseUpdate.UpdateVersionList.Version_5;

      //
      // Determine if the user has access to this page and log and error if they do not.
      //
      if ( this.Session.UserProfile.hasAdministrationAccess == false )
      {
        this.LogIllegalAccess (
          this.ClassNameSpace + ".getDB_Update_Object",
          this.Session.UserProfile );

        this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

        return null;
      }

      // 
      // Log access to page.
      // 
      this.LogPageAccess (
        this.ClassNameSpace + ".getDB_Update_Object",
        this.Session.UserProfile );

      try
      {
        //
        // Update the selection parameters.
        //
        if ( PageCommand.hasParameter ( EdAdapterConfig.CONST_UPDATE_VERSION ) == true )
        {
          this.Session.DatabaseUpdateVersion =
            PageCommand.GetParameter<EvDataBaseUpdate.UpdateVersionList> ( EdAdapterConfig.CONST_UPDATE_VERSION );
        }

        if ( PageCommand.hasParameter ( EdAdapterConfig.CONST_UPDATE_ORDER ) == true )
        {
          this.Session.DataBaseUpdateOrderBy =
            PageCommand.GetParameter<EvDataBaseUpdate.UpdateOrderBy> ( EdAdapterConfig.CONST_UPDATE_ORDER );
        }

        this.LogValue ( "UpdateVersion: " + this.Session.DatabaseUpdateVersion );
        this.LogValue ( "UpdateOrderBy: " + this.Session.DataBaseUpdateOrderBy );

        // 
        // return the client ResultData object for the customer.
        // 
        this.getDB_Update_DataObject ( clientDataObject );

        return clientDataObject;
      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.ApplicationProfile_Page_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return null;

    }//END getObject method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="ClientDataObject">Evado.Model.UniForm.AppData object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private void getDB_Update_DataObject ( Evado.Model.UniForm.AppData ClientDataObject )
    {
      this.LogMethod ( "getDB_Update_DataObject" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      EvDataBaseUpdates bllDatabaseUpdates = new EvDataBaseUpdates ( );
      List<EvDataBaseUpdate> updateList = new List<EvDataBaseUpdate> ( );
      List<EvOption> optionList = new List<EvOption> ( );

      //
      // Initialise the client ResultData object.
      //
      ClientDataObject.Id = this.GlobalObjects.AdapterSettings.Guid;
      ClientDataObject.Title = EdLabels.DataBase_Update_Page_Title;

      ClientDataObject.Page.Id = ClientDataObject.Id;
      ClientDataObject.Page.Title = ClientDataObject.Title;
      ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;


      //
      // generate the selection pageMenuGroup.
      //
      this.getDB_Update_SelectionGroup ( ClientDataObject.Page );

      //
      // generate the list pageMenuGroup.
      //
      this.getDB_Update_ListGroup ( ClientDataObject.Page );

    }//END Method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private void getDB_Update_SelectionGroup ( Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getDB_Update_SelectionGroup" );

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      Evado.Model.UniForm.Command groupCommand = new Evado.Model.UniForm.Command ( );
      List<EvOption> optionlist = new List<EvOption> ( );


      //
      // create the selection pageMenuGroup.
      //
      pageGroup = Page.AddGroup (
        EdLabels.DataBase_Update_Selection_Group_Title,
        Evado.Model.UniForm.EditAccess.Enabled );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;

      //
      // create the version selectoin field.
      //
      optionlist = Evado.Model.Digital.EvcStatics.Enumerations.getOptionsFromEnum ( typeof ( EvDataBaseUpdate.UpdateVersionList ), true );

      groupField = pageGroup.createSelectionListField (
        EdAdapterConfig.CONST_UPDATE_VERSION,
        EdLabels.DataBase_Update_Version_Field_Title,
        this.Session.DatabaseUpdateVersion.ToString ( ),
        optionlist );

      groupField.AddParameter ( Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, "1" );
      groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      //
      // create the version selectoin field.
      //
      optionlist = Evado.Model.Digital.EvcStatics.Enumerations.getOptionsFromEnum ( typeof ( EvDataBaseUpdate.UpdateOrderBy ), true );

      groupField = pageGroup.createSelectionListField (
        CONST_UPDATE_ORDER,
        EdLabels.DataBase_Update_Order_Field_Title,
        this.Session.DataBaseUpdateOrderBy.ToString ( ),
        optionlist );

      groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;
      groupField.AddParameter ( Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, "1" );

      //
      // Add the selection groupCommand.
      //
      groupCommand = pageGroup.addCommand (
        EdLabels.DataBase_Update_Selection_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Application_Properties.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Custom_Method );

      groupCommand.AddParameter (
        Model.UniForm.CommandParameters.Page_Id,
        EvPageIds.Database_Version );

      groupCommand.setCustomMethod (
        Model.UniForm.ApplicationMethods.Get_Object );




    }//END Method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private void getDB_Update_ListGroup ( Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getDB_Update_DataObject" );
      this.LogValue ( "UpdateVersion: " + this.Session.DatabaseUpdateVersion );
      this.LogValue ( "UpdateOrderBy: " + this.Session.DataBaseUpdateOrderBy );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      EvDataBaseUpdates bllDatabaseUpdates = new EvDataBaseUpdates ( this.ClassParameters );
      List<EvDataBaseUpdate> updateList = new List<EvDataBaseUpdate> ( );
      List<EvOption> optionList = new List<EvOption> ( );

      //
      // Retrieve the list of upadates.
      //
      updateList = bllDatabaseUpdates.getUpdateList (
        this.Session.DatabaseUpdateVersion,
        this.Session.DataBaseUpdateOrderBy );

      this.LogValue ( bllDatabaseUpdates.Log );

      if ( updateList.Count == 0 )
      {
        pageGroup.Description = EdLabels.Database_Update_No_Data_Message;
        return;
      }

      pageGroup = Page.AddGroup (
        EdLabels.DataBase_Update_Group_Title,
        Evado.Model.UniForm.EditAccess.Enabled );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;

      groupField = pageGroup.createTableField (
        String.Empty,
        EdLabels.Database_Update_Field_Title, 5 );
      groupField.Layout = Model.UniForm.FieldLayoutCodes.Column_Layout;

      groupField.Table.Header [ 0 ].Text = EdLabels.Database_Update_Table_Column_0;
      groupField.Table.Header [ 0 ].TypeId = EvDataTypes.Read_Only_Text;
      //groupField.Table.Header [ 0 ].Width = "4";

      groupField.Table.Header [ 1 ].Text = EdLabels.Database_Update_Table_Column_1;
      groupField.Table.Header [ 1 ].TypeId = EvDataTypes.Read_Only_Text;
      groupField.Table.Header [ 1 ].Width = "12";

      groupField.Table.Header [ 2 ].Text = EdLabels.Database_Update_Table_Column_2;
      groupField.Table.Header [ 2 ].TypeId = EvDataTypes.Read_Only_Text;
      //groupField.Table.Header [ 2 ].Width = "5";

      groupField.Table.Header [ 3 ].Text = EdLabels.Database_Update_Table_Column_3;
      groupField.Table.Header [ 3 ].TypeId = EvDataTypes.Read_Only_Text;
      groupField.Table.Header [ 3 ].Width = "20";

      groupField.Table.Header [ 4 ].Text = EdLabels.Database_Update_Table_Column_4;
      groupField.Table.Header [ 4 ].TypeId = EvDataTypes.Read_Only_Text;
      groupField.Table.Header [ 4 ].Width = "40";

      foreach ( EvDataBaseUpdate update in updateList )
      {
        Evado.Model.UniForm.TableRow row = new Model.UniForm.TableRow ( );

        row.Column [ 0 ] = update.UpdateNo.ToString ( "000" );
        row.Column [ 1 ] = update.UpdateDate.ToString ( "dd-MMM-yy" );
        row.Column [ 2 ] = update.Version;
        row.Column [ 3 ] = update.Objects;
        row.Column [ 4 ] = update.Description;

        groupField.Table.Rows.Add ( row );
      }

    }//END Method

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class get Config Audit object methods

    private const string CONST_AUDIT_TABLE = "AUDT";
    private const string CONST_AUDIT_VERSON = "AUDV";
    private const string CONST_AUDIT_SCHEDULE = "AUDSCH";
    private const string CONST_AUDIT_SCHEDULE_GUID = "AUDSCH_GUID";
    private const string CONST_AUDIT_RECORD_GUID = "AUDR";
    private const string CONST_AUDIT_RECORD_ITEM_GUID = "AUDRI";

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private void getAudit_Update_Session (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getAudit_Update_Session" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
      String parameterValue = String.Empty;

      if ( this.Session.AuditRecordGuid == null )
      {
        this.Session.AuditRecordGuid = Guid.Empty;
      }

      //
      // Update the selection parameters.
      //
      if ( PageCommand.hasParameter ( EdAdapterConfig.CONST_AUDIT_TABLE ) == true )
      {
        parameterValue = PageCommand.GetParameter ( EdAdapterConfig.CONST_AUDIT_TABLE );

        if ( parameterValue != this.Session.AuditTableName.ToString ( ) )
        {
          this.Session.AuditTableName = Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue<
            EvDataChange.DataChangeTableNames> ( parameterValue );

          this.Session.AuditRecordGuid = Guid.Empty;
          this.Session.AuditRecordItemGuid = Guid.Empty;
        }
      }

      if ( PageCommand.hasParameter ( EdAdapterConfig.CONST_AUDIT_RECORD_GUID ) == true )
      {
        parameterValue = PageCommand.GetParameter ( EdAdapterConfig.CONST_AUDIT_RECORD_GUID );

        if ( parameterValue != String.Empty
          && parameterValue != this.Session.AuditRecordGuid.ToString ( ) )
        {
          this.Session.AuditRecordGuid = new Guid ( parameterValue );

          this.Session.AuditRecordItemGuid = Guid.Empty;
        }
      }

      if ( PageCommand.hasParameter ( EdAdapterConfig.CONST_AUDIT_RECORD_ITEM_GUID ) == true )
      {
        parameterValue = PageCommand.GetParameter ( EdAdapterConfig.CONST_AUDIT_RECORD_ITEM_GUID );

        if ( parameterValue != String.Empty )
        {
          this.Session.AuditRecordItemGuid = new Guid ( parameterValue );
        }
      }

      // 
      // Selection for Application selection
      // 
      if ( this.Session.AuditTableName == EvDataChange.DataChangeTableNames.EdAdapterSettings )
      {
        this.Session.AuditRecordGuid = Guid.Empty;
      }

      this.LogValue ( "AuditTableName: " + this.Session.AuditTableName );
      this.LogValue ( "AuditRecordGuid: " + this.Session.AuditRecordGuid );
      this.LogValue ( "AuditRecordItemGuid: " + this.Session.AuditRecordItemGuid );

    }//END getAudit_Update_Session method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand"> Evado.Model.UniForm.Command object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData getConfig_Audit_Object (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getConfig_Audit_Object" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
      String parameterValue = String.Empty;

      //
      // Determine if the user has access to this page and log and error if they do not.
      //
      if ( this.Session.UserProfile.hasAdministrationAccess == false )
      {
        this.LogIllegalAccess (
          this.ClassNameSpace + ".getConfig_Audit_Object",
          this.Session.UserProfile );

        this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

        return null;
      }

      // 
      // Log access to page.
      // 
      this.LogPageAccess (
        this.ClassNameSpace + ".getConfig_Audit_Object",
        this.Session.UserProfile );

      try
      {
        this.getAudit_Update_Session ( PageCommand );

        // 
        // return the client ResultData object for the customer.
        // 
        this.getConfig_Audit_DataObject ( clientDataObject );

        return clientDataObject;
      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.Audit_Config_Page_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return null;

    }//END getConfig_Audit_Object method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="ClientDataObject">Evado.Model.UniForm.AppData object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private void getConfig_Audit_DataObject (
      Evado.Model.UniForm.AppData ClientDataObject )
    {
      this.LogMethod ( "getConfig_Audit_DataObject" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      List<EvOption> optionList = new List<EvOption> ( );

      //
      // Initialise the client ResultData object.
      //
      ClientDataObject.Id = this.GlobalObjects.AdapterSettings.Guid;
      ClientDataObject.Title = EdLabels.Audit_Config_Page_Title;

      ClientDataObject.Page.Id = ClientDataObject.Id;
      ClientDataObject.Page.Title = ClientDataObject.Title;
      ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      //
      // generate the selection pageMenuGroup.
      //
      this.getConfig_Audit_SelectionGroup (
        ClientDataObject.Page );

      //
      // generate the list pageMenuGroup.
      //
      this.getAudit_ListGroup (
        ClientDataObject.Page,
        this.Session.AuditRecordGuid );

    }//END Method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private void getConfig_Audit_SelectionGroup ( Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getConfig_Audit_SelectionGroup" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      Evado.Model.UniForm.Command groupCommand = new Evado.Model.UniForm.Command ( );
      List<EvOption> optionlist = new List<EvOption> ( );
      EvDataChanges bllDataChanges = new EvDataChanges ( );

      //
      // create the selection pageMenuGroup.
      //
      pageGroup = Page.AddGroup (
        EdLabels.Audit_Selection_Group_Title,
        Evado.Model.UniForm.EditAccess.Enabled );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;

      //
      // create the version selectoin field.
      //
      optionlist = EvDataChange.getConfigurationTablesNameList ( );

      groupField = pageGroup.createSelectionListField (
        EdAdapterConfig.CONST_AUDIT_TABLE,
        EdLabels.Audit_Table_Selection_Field_Title,
        this.Session.AuditTableName.ToString ( ),
        optionlist );

      groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;
      groupField.AddParameter ( Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );


      //
      // create the version selection field.
      //
      /*
      optionlist = bllDataChanges.getConfigurationRecordSelectionList (
        this.Session.Application.ApplicationId,
        this.Session.AuditScheduleGuid,
        this.Session.ScheduleId,
        this.Session.AuditTableName );

      this.LogText ( bllDataChanges.DebugLog );
      */
      //
      // Display the selection field if the selecions exist.
      //
      if ( optionlist.Count > 1 )
      {
        groupField = pageGroup.createSelectionListField (
          EdAdapterConfig.CONST_AUDIT_RECORD_GUID,
          EdLabels.Audit_Record_Selection_Field_Title,
          this.Session.AuditRecordGuid.ToString ( ),
          optionlist );

        groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;
        groupField.AddParameter ( Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );
      }

      //
      // Add the selection groupCommand.
      //
      groupCommand = pageGroup.addCommand (
        EdLabels.DataBase_Update_Selection_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Application_Properties.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Custom_Method );

      groupCommand.AddParameter (
        Model.UniForm.CommandParameters.Page_Id,
        EvPageIds.Audit_Configuration_Page );

      groupCommand.setCustomMethod (
        Model.UniForm.ApplicationMethods.Get_Object );




    }//END Method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page: page object.</param>
    /// <param name="RecordGuid">Guid: the record's guid identifier..</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private void getAudit_ListGroup (
      Evado.Model.UniForm.Page Page,
      Guid RecordGuid )
    {
      this.LogMethod ( "getConfig_Audit_DataObject" );
      this.LogValue ( "AuditTableName: " + this.Session.AuditTableName );
      this.LogValue ( "RecordGuid: " + RecordGuid );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      EvDataChanges bllDataChanges = new EvDataChanges ( );
      List<EvDataChange> dataChangeList = new List<EvDataChange> ( );
      List<EvOption> optionList = new List<EvOption> ( );

      //
      // If the parameters are empty or null then exit the method as no ResultData is retrieved.
      // 
      if ( this.Session.AuditTableName == EvDataChange.DataChangeTableNames.Null )
      {
        return;
      }

      //
      // Retrieve the list of upadates.
      //
      dataChangeList = bllDataChanges.GetDataChangeList (
        RecordGuid,
        this.Session.AuditTableName );

      this.LogValue ( bllDataChanges.DebugLog );

      //
      // If no results are returned then display a message indicating that 
      // no records where returned.
      //
      if ( dataChangeList.Count == 0 )
      {
        //
        // create the audit tail pageMenuGroup.
        //
        pageGroup = Page.AddGroup (
          EdLabels.Audit_Data_Group_Title,
          Evado.Model.UniForm.EditAccess.Enabled );
        pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;
        pageGroup.Description = EdLabels.Audit_Empty_Results_Message;
        return;
      }

      //
      // Data change item iteration loop.
      //
      for ( int index = 0; index < dataChangeList.Count; index++ )
      {
        EvDataChange dataChange = dataChangeList [ index ];
        //
        // create the audit tail pageMenuGroup.
        //
        pageGroup = Page.AddGroup (
          EdLabels.Audit_Data_Group_Title + EdLabels.Space_Hypen + index,
          Evado.Model.UniForm.EditAccess.Enabled );
        pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;

        //
        // Define the project identifier.
        //
        groupField = pageGroup.createReadOnlyTextField (
          String.Empty,
          EdLabels.Label_Project_Id,
          dataChange.TrialId );
        groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;

        //
        // Create the table of audit changes.
        //
        groupField = pageGroup.createTableField (
          String.Empty,
          EdLabels.Database_Update_Field_Title, 5 );
        groupField.Layout = Model.UniForm.FieldLayoutCodes.Column_Layout;

        groupField.Table.Header [ 0 ].Text = EdLabels.Audit_Data_Table_Column_0;
        groupField.Table.Header [ 0 ].TypeId = EvDataTypes.Read_Only_Text;

        groupField.Table.Header [ 1 ].Text = EdLabels.Audit_Data_Table_Column_1;
        groupField.Table.Header [ 1 ].TypeId = EvDataTypes.Read_Only_Text;

        groupField.Table.Header [ 2 ].Text = EdLabels.Audit_Data_Table_Column_2;
        groupField.Table.Header [ 2 ].TypeId = EvDataTypes.Read_Only_Text;

        groupField.Table.Header [ 3 ].Text = EdLabels.Audit_Data_Table_Column_3;
        groupField.Table.Header [ 3 ].TypeId = EvDataTypes.Read_Only_Text;

        for ( int count = 0; count < dataChange.Items.Count; count++ )
        {
          EvDataChangeItem dataChangeItem = dataChange.Items [ count ];

          if ( dataChangeItem.InitialValue.Contains ( "<EvFormDesign" ) == true
            || dataChangeItem.NewValue.Contains ( "<EvFormDesign" ) == true )
          {
            this.writeFormDesignValues (
                      groupField.Table.Rows,
                      count,
                      dataChangeItem );
          }
          else
          {
            if ( dataChangeItem.InitialValue.Contains ( "<EvProjectData" ) == true
              || dataChangeItem.NewValue.Contains ( "<EvProjectData" ) == true )
            {
              this.writeTrialDataValues (
                      groupField.Table.Rows,
                      count,
                      dataChangeItem );
            }
            else
            {

              if ( dataChangeItem.InitialValue.Contains ( "<EvUserSignoff" ) == true
                || dataChangeItem.NewValue.Contains ( "<EvUserSignoff" ) == true )
              {
                //
                // Signoffs
                //
                this.writeSignoffValues (
                  groupField.Table.Rows,
                  count,
                  dataChangeItem );
              }
              else
              {
                Evado.Model.UniForm.TableRow row = new Model.UniForm.TableRow ( );
                row.Column [ 0 ] = count.ToString ( );
                row.Column [ 1 ] = dataChangeItem.ItemId;
                row.Column [ 2 ] = dataChangeItem.InitialValue;
                row.Column [ 3 ] = dataChangeItem.NewValue;
                groupField.Table.Rows.Add ( row );
              }
            }
          }


        }//ENd ResultData chant item iteration .

        //
        // Define the user that updated the datachange object.
        //
        groupField = pageGroup.createReadOnlyTextField (
          String.Empty,
          EdLabels.Audit_Data_User_Field_Title,
          dataChange.UserId );

        groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;

        //
        // Define the user that updated the datachange object.
        //
        groupField = pageGroup.createReadOnlyTextField (
          String.Empty,
          EdLabels.Audit_Data_Date_Field_Title,
          dataChange.DateStamp.ToString ( "dd-MMM-yyy HH:mm:ss" ) );

        groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;


      }//End Data change iteration loop


    }//END Method

    //===================================================================================
    /// <summary>
    /// This method process the list of ResultData changes.
    /// </summary>
    /// <param name="RowList">List<Evado.Model.UniForm.TableRow>: the list of rows in table</param>
    /// <param name="Instance">int: th instance of the change</param>
    /// <param name="Item">EvDataChange object containing the change.</param>
    //-----------------------------------------------------------------------------------
    private void writeTrialDataValues (
      List<Evado.Model.UniForm.TableRow> RowList,
      int Instance,
      EvDataChangeItem Item )
    {
      System.Console.WriteLine ( "writeTrialDataValues" );

    }//END writeFormContentValues method

    //===================================================================================
    /// <summary>
    /// This method process the list of ResultData changes.
    /// </summary>
    /// <param name="RowList">List<Evado.Model.UniForm.TableRow>: the list of rows in the table</param>
    /// <param name="Instance">int: th instance of the change</param>
    /// <param name="Item">EvDataChange object containing the change.</param>
    //-----------------------------------------------------------------------------------
    private void writeSignoffValues (
      List<Evado.Model.UniForm.TableRow> RowList,
      int Instance,
      EvDataChangeItem Item )
    {
      this.LogMethod ( "writeSignoffValues" );
      //
      // Initialise the methods variables and objects.
      //
      List<EdUserSignoff> initialContent = new List<EdUserSignoff> ( );
      List<EdUserSignoff> newContent = new List<EdUserSignoff> ( );

      if ( Item.InitialValue == String.Empty
        && Item.NewValue == String.Empty )
      {
        return;
      }

      if ( Item.InitialValue != string.Empty )
      {
        try
        {
          initialContent = Evado.Model.Digital.EvcStatics.DeserialiseObject<List<EdUserSignoff>> ( Item.InitialValue );
        }
        catch
        {
          this.LogValue ( "ERROR: ItemValue: " + Item.InitialValue );
        }
      }

      if ( Item.NewValue != string.Empty )
      {
        try
        {
          newContent = Evado.Model.Digital.EvcStatics.DeserialiseObject<List<EdUserSignoff>> ( Item.NewValue );
        }
        catch
        {
          this.LogValue ( "ERROR: ItemValue: " + Item.InitialValue );
        }
      }

      Item.ItemId = "UserSignoff";

      //
      // Signoffs
      //
      this.writeSignoffField (
        RowList,
        Instance,
        Item.ItemId,
        "UserSignoff",
        initialContent,
        newContent );

    }//END writeFormContentValues method

    //===================================================================================
    /// <summary>
    /// This method process the list of ResultData changes.
    /// </summary>
    /// <param name="RowList">List<Evado.Model.UniForm.TableRow>: the list of rows in a table</param>
    /// <param name="Instance">int: th instance of the change</param>
    /// <param name="Item">EvDataChange object containing the change.</param>
    //-----------------------------------------------------------------------------------
    private void writeFormDesignValues (
      List<Evado.Model.UniForm.TableRow> RowList,
      int Instance,
      EvDataChangeItem Item )
    {
      System.Console.WriteLine ( "writeFormDesignValues" );
      //
      // Initialise the methods variables and objects.
      //
      EdRecordDesign initialContent = new EdRecordDesign ( );
      EdRecordDesign newContent = new EdRecordDesign ( );
      Evado.Model.UniForm.TableRow row = new Model.UniForm.TableRow ( );

      if ( Item.InitialValue == String.Empty
        && Item.NewValue == String.Empty )
      {
        return;
      }

      if ( Item.InitialValue != String.Empty )
      {
        initialContent = Evado.Model.Digital.EvcStatics.DeserialiseObject<EdRecordDesign> ( Item.InitialValue );
      }

      if ( Item.NewValue != String.Empty )
      {
        newContent = Evado.Model.Digital.EvcStatics.DeserialiseObject<EdRecordDesign> ( Item.NewValue );
      }

      Item.ItemId = "FormDesign";

      //
      // Title
      //
      this.addAuditReportRow (
        RowList,
        Instance,
        Item.ItemId + "Title",
        initialContent.Title.ToString ( ),
        newContent.Title.ToString ( ) );

      //
      // Reference
      //
      this.addAuditReportRow (
        RowList,
        Instance,
        Item.ItemId + "Reference",
        initialContent.HttpReference.ToString ( ),
        newContent.HttpReference.ToString ( ) );

      //
      // FormCategory
      //
      this.addAuditReportRow (
        RowList,
        Instance,
        Item.ItemId + "FormCategory",
        initialContent.RecordCategory.ToString ( ),
        newContent.RecordCategory.ToString ( ) );

      //
      // Instructions
      //
      this.addAuditReportRow (
        RowList,
        Instance,
        Item.ItemId + "Instructions",
        initialContent.Instructions.ToString ( ),
        newContent.Instructions.ToString ( ) );

      //
      // Description
      //
      this.addAuditReportRow (
        RowList,
        Instance,
        Item.ItemId + "Description",
        initialContent.Description.ToString ( ),
        newContent.Description.ToString ( ) );

      //
      // Description
      //
      this.addAuditReportRow (
        RowList,
        Instance,
        Item.ItemId + "UpdateReason",
        initialContent.UpdateReason.ToString ( ),
        newContent.UpdateReason.ToString ( ) );

      //
      // Version
      //
      this.addAuditReportRow (
        RowList,
        Instance,
        Item.ItemId + "Version",
        initialContent.Version.ToString ( ),
        newContent.Version.ToString ( ) );

      //
      // TypeId
      //
      this.addAuditReportRow (
        RowList,
        Instance,
        Item.ItemId + "TypeId",
        initialContent.TypeId.ToString ( ),
        newContent.TypeId.ToString ( ) );

    }//END writeFormContentValues method


    //===================================================================================
    /// <summary>
    /// This method process the list of ResultData changes.
    /// </summary>
    /// <param name="RowList">List<Evado.Model.UniForm.TableRow>: the list of rows in table.</param>
    /// <param name="Instance">int: th instance of the change</param>
    /// <param name="ItemId">String: the item identifeir</param>
    /// <param name="FieldId">String: the item field identifeir</param>
    /// <param name="InitialData">List of EvUserSignoff initial object.</param>
    /// <param name="NewData">List of EvUserSignoff new object</param>
    //-----------------------------------------------------------------------------------
    private void writeSignoffField (
      List<Evado.Model.UniForm.TableRow> RowList,
      int Instance,
      String ItemId,
      String FieldId,
      List<EdUserSignoff> InitialData,
      List<EdUserSignoff> NewData )
    {
      //
      // Validate the signatures need to be added.
      //
      if ( InitialData.Count == NewData.Count )
      {
        return;
      }

      //
      // initialise the methods variables and objects.
      int instance = 0;

      //
      // Iterate through the new ResultData adding relevant items.
      //
      for ( int count = 0; count < NewData.Count; count++ )
      {
        //
        // if the tableColumn is less than the length of the initial ResultData 
        // perform the comparision.
        //
        if ( count < InitialData.Count )
        {
          //
          // if the ResultData stamp matches then on update is necessary.
          //
          if ( InitialData [ count ].SignOffDate == NewData [ count ].SignOffDate )
          {
            continue;
          }

          //
          // Add signoff date
          //
          this.addAuditReportRow (
             RowList,
             Instance,
             ItemId + "_" + FieldId + "_" + instance + "_Date",
             InitialData [ count ].SignOffDate.ToString ( "dd MMM yyyy HH:mm" ),
             NewData [ count ].SignOffDate.ToString ( "dd MMM yyyy HH:mm" ) );

          //
          // Add signoff user
          //
          this.addAuditReportRow (
             RowList,
             Instance,
             ItemId + "_" + FieldId + "_" + instance + "_User",
             InitialData [ count ].SignedOffUserId,
             NewData [ count ].SignedOffUserId );

          //
          // Signoff type
          //
          this.addAuditReportRow (
             RowList,
             Instance,
             ItemId + "_" + FieldId + "_" + instance + "_Type",
             InitialData [ count ].Type.ToString ( ),
             NewData [ count ].Type.ToString ( ) );

          instance++;
        }
        else
        {
          if ( NewData [ count ].SignedOffUserId == String.Empty )
          {
            continue;
          }

          //
          // Add signoff date
          //
          this.addAuditReportRow (
             RowList,
             Instance,
             ItemId + "_" + FieldId + "_" + instance + "_Date",
              Evado.Model.Digital.EvcStatics.CONST_DATE_NULL.ToString ( "dd MMM yyyy HH:mm" ),
             NewData [ count ].SignOffDate.ToString ( "dd MMM yyyy HH:mm" ) );

          //
          // Add signoff user
          //
          this.addAuditReportRow (
             RowList,
             Instance,
             ItemId + "_" + FieldId + "_" + instance + "_User",
             String.Empty,
             NewData [ count ].SignedOffUserId );

          //
          // Signoff type
          //
          this.addAuditReportRow (
             RowList,
             Instance,
             ItemId + "_" + FieldId + "_" + instance + "_Type",
             String.Empty,
             NewData [ count ].Type.ToString ( ) );

          instance++;

        }//END add new signoff ResultData.

      }//END new ResultData iteration loop.

    }//END writeFormStaticField method

    //===================================================================================
    /// <summary>
    /// This method process the list of ResultData changes.
    /// </summary>
    /// <param name="RowList">List<Evado.Model.UniForm.TableRow>: the list of rows in table</param>
    /// <param name="Instance">int: th instance of the change</param>
    /// <param name="ItemId">String: the record identifeir</param>
    /// <param name="InitialValue">String: initial value.</param>
    /// <param name="NewValue">String: new value.</param>
    //-----------------------------------------------------------------------------------
    private void addAuditReportRow (
      List<Evado.Model.UniForm.TableRow> RowList,
      int Instance,
      String ItemId,
      String InitialValue,
      String NewValue )
    {
      Evado.Model.UniForm.TableRow row = new Model.UniForm.TableRow ( );
      //
      // if there is no values then exit.
      //
      if ( ( InitialValue == String.Empty
          && NewValue == String.Empty ) )
      {
        return;
      }

      //
      // if value are the same exit.
      //
      if ( ( InitialValue == NewValue ) )
      {
        return;
      }

      //
      // If there are line feeds strip them.
      //
      if ( InitialValue.Contains ( "\r" ) == true
        || InitialValue.Contains ( "\n" ) == true
        || InitialValue.Contains ( "<br/>" ) == true )
      {
        InitialValue = InitialValue.Replace ( "\n", "^" );
        InitialValue = InitialValue.Replace ( "\r", "^" );
        InitialValue = InitialValue.Replace ( "<br/>", "^" );
        InitialValue = InitialValue.Replace ( "^^", "^" );
      }

      if ( NewValue.Contains ( "\r" ) == true
        || NewValue.Contains ( "\n" ) == true
        || NewValue.Contains ( "<br/>" ) == true )
      {
        NewValue = NewValue.Replace ( "\n", "^" );
        NewValue = NewValue.Replace ( "\r", "^" );
        NewValue = NewValue.Replace ( "<br/>", "^" );
        NewValue = NewValue.Replace ( "^^", "^" );
      }

      //
      // Add a new row to the table.
      //
      row.Column [ 0 ] = Instance.ToString ( );
      row.Column [ 1 ] = ItemId;
      row.Column [ 2 ] = InitialValue;
      row.Column [ 3 ] = NewValue;
      RowList.Add ( row );

    }//END writeCsvRecord method


    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class get Record Audit object methods

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData getRecord_Audit_Object (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogValue ( Evado.Model.UniForm.EuStatics.CONST_METHOD_START
        + this.ClassNameSpace + ".getRecord_Audit_Object" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
      String parameterValue = String.Empty;

      //
      // Determine if the user has access to this page and log and error if they do not.
      //
      if ( this.Session.UserProfile.hasAdministrationAccess == false )
      {
        this.LogIllegalAccess (
          this.ClassNameSpace + ".getRecord_Audit_Object",
          this.Session.UserProfile );

        this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

        return null;
      }

      // 
      // Log access to page.
      // 
      this.LogPageAccess (
        this.ClassNameSpace + ".getRecord_Audit_Object",
        this.Session.UserProfile );

      try
      {
        this.getAudit_Update_Session ( PageCommand );

        // 
        // return the client ResultData object for the customer.
        // 
        this.getRecord_Audit_DataObject ( clientDataObject );

        return clientDataObject;
      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.Audit_Record_Page_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return null;

    }//END getRecord_Audit_Object method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="ClientDataObject">Evado.Model.UniForm.AppData object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private void getRecord_Audit_DataObject (
      Evado.Model.UniForm.AppData ClientDataObject )
    {
      this.LogValue ( Evado.Model.UniForm.EuStatics.CONST_METHOD_START
        + " Evado.UniForm.Clinical.ApplicationProperties.getRecord_Audit_DataObject" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      List<EvOption> optionList = new List<EvOption> ( );

      //
      // Initialise the client ResultData object.
      //
      ClientDataObject.Id = this.GlobalObjects.AdapterSettings.Guid;
      ClientDataObject.Title = EdLabels.Audit_Record_Page_Title;

      ClientDataObject.Page.Id = ClientDataObject.Id;
      ClientDataObject.Page.Title = ClientDataObject.Title;
      ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      //
      // generate the selection pageMenuGroup.
      //
      this.getRecord_Audit_SelectionGroup ( ClientDataObject.Page );

      //
      // generate the list pageMenuGroup.
      //
      this.getAudit_ListGroup ( ClientDataObject.Page,
        this.Session.AuditRecordGuid );

    }//END Method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private void getRecord_Audit_SelectionGroup ( Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getRecord_Audit_SelectionGroup" );
      this.LogValue ( "AuditTableName: " + this.Session.AuditTableName );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      Evado.Model.UniForm.Command groupCommand = new Evado.Model.UniForm.Command ( );
      List<EvOption> optionlist = new List<EvOption> ( );
      Evado.Bll.Clinical.EvDataChanges bllDataChanges = new Evado.Bll.Clinical.EvDataChanges ( );


      return;

    }//END Method

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class get Record Field Audit object methods

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData getRecord_Item_Audit_Object (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getRecord_Item_Audit_Object" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
      String parameterValue = String.Empty;

      //
      // Determine if the user has access to this page and log and error if they do not.
      //
      if ( this.Session.UserProfile.hasAdministrationAccess == false )
      {
        this.LogIllegalAccess (
          this.ClassNameSpace + ".getRecord_Item_Audit_Object",
          this.Session.UserProfile );

        this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

        return null;
      }

      // 
      // Log access to page.
      // 
      this.LogPageAccess (
        this.ClassNameSpace + ".getRecord_Item_Audit_Object",
        this.Session.UserProfile );

      try
      {
        this.getAudit_Update_Session ( PageCommand );

        // 
        // return the client ResultData object for the customer.
        // 
        this.getRecord_Item_Audit_DataObject ( clientDataObject );

        return clientDataObject;
      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.Audit_Record_Page_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return null;

    }//END getRecord_Item_Audit_Object method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="ClientDataObject">Evado.Model.UniForm.AppData object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private void getRecord_Item_Audit_DataObject (
      Evado.Model.UniForm.AppData ClientDataObject )
    {
      this.LogMethod ( "getRecord_Item_Audit_DataObject" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      List<EvOption> optionList = new List<EvOption> ( );

      //
      // Initialise the client ResultData object.
      //
      ClientDataObject.Id = this.GlobalObjects.AdapterSettings.Guid;
      ClientDataObject.Title = EdLabels.Audit_Config_Item_Page_Title;

      ClientDataObject.Page.Id = ClientDataObject.Id;
      ClientDataObject.Page.Title = ClientDataObject.Title;
      ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      //
      // generate the selection pageMenuGroup.
      //
      this.getRecord_Item_Audit_SelectionGroup (
        ClientDataObject.Page );

      //
      // generate the list pageMenuGroup.
      //
      this.getAudit_ListGroup (
        ClientDataObject.Page,
        this.Session.AuditRecordItemGuid );

    }//END getRecord_Item_Audit_DataObject Method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page: page object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private void getRecord_Item_Audit_SelectionGroup (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getRecord_Item_Audit_SelectionGroup" );
      this.LogValue ( "AuditTableName: " + this.Session.AuditTableName );
      this.LogValue ( "AuditRecordGuid: " + this.Session.AuditRecordGuid );
      this.LogValue ( "AuditRecordItemGuid: " + this.Session.AuditRecordItemGuid );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      Evado.Model.UniForm.Command groupCommand = new Evado.Model.UniForm.Command ( );
      List<EvOption> optionlist = new List<EvOption> ( );
      Evado.Bll.Clinical.EvDataChanges bllDataChanges = new Evado.Bll.Clinical.EvDataChanges ( );

      //
      // create the selection pageMenuGroup.
      //
      pageGroup = Page.AddGroup (
        EdLabels.Audit_Selection_Group_Title,
        Evado.Model.UniForm.EditAccess.Enabled );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;

      //
      // create the record table selection field.
      //
      optionlist = EvDataChange.getRecordItemTablesNameList ( );

      groupField = pageGroup.createSelectionListField (
        EdAdapterConfig.CONST_AUDIT_TABLE,
        EdLabels.Audit_Table_Selection_Field_Title,
        this.Session.AuditTableName.ToString ( ),
        optionlist );

      groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;
      groupField.AddParameter ( Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );

      //
      // create the record selection field.
      //
      if ( this.Session.AuditTableName != EvDataChange.DataChangeTableNames.Null )
      {
        optionlist = bllDataChanges.getItemRecordSelectionList (
          this.Session.AuditTableName );

        //
        // create the record selection list.
        //
        groupField = pageGroup.createSelectionListField (
          EdAdapterConfig.CONST_AUDIT_RECORD_GUID,
          EdLabels.Audit_Record_Selection_Field_Title,
          this.Session.AuditRecordGuid.ToString ( ),
          optionlist );

        groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;
        groupField.AddParameter ( Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );
      }

      //
      // create the record selection field.
      //
      if ( this.Session.AuditRecordGuid != Guid.Empty
        && this.Session.AuditTableName != EvDataChange.DataChangeTableNames.Null )
      {
        optionlist = bllDataChanges.getRecordItemSelectionList (
          this.Session.AuditRecordGuid,
          this.Session.AuditTableName );

        this.LogValue ( "getRecordItemSelectionList selection: " + bllDataChanges.DebugLog );

        if ( optionlist.Count > 0 )
        {
          //
          // create the record selection list.
          //
          groupField = pageGroup.createSelectionListField (
            EdAdapterConfig.CONST_AUDIT_RECORD_ITEM_GUID,
            EdLabels.Audit_Record_Item_Selection_Field_Title,
            this.Session.AuditRecordItemGuid.ToString ( ),
            optionlist );

          groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;
          groupField.AddParameter ( Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );

        }//END option list exists.

      }//END audit record selected.

      //
      // Add the selection groupCommand.
      //
      groupCommand = pageGroup.addCommand (
        EdLabels.DataBase_Update_Selection_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Application_Properties.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Custom_Method );

      groupCommand.AddParameter (
        Model.UniForm.CommandParameters.Page_Id,
        EvPageIds.Audit_Record_Items_Page );

      groupCommand.setCustomMethod (
        Model.UniForm.ApplicationMethods.Get_Object );

    }//END getRecord_Item_Audit_SelectionGroup Method

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class get application object methods

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
        this.GlobalObjects.AdapterSettings = this._bll_AdapterConfig.getItem ( "A" );

        this.LogClass ( this._bll_AdapterConfig.Log );

        this.GlobalObjects.AdapterSettings.Version = this.GlobalObjects.FullVersion;

        this.LogValue ( "Version: " + this.GlobalObjects.AdapterSettings.Version );
        // 
        // return the client ResultData object for the customer.
        // 
        this.getDataObject ( clientDataObject );

        return clientDataObject;
      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.ApplicationProfile_Page_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return null;

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

      ClientDataObject.Id = this.GlobalObjects.AdapterSettings.Guid;
      ClientDataObject.Title = EdLabels.ApplicationProfile_Page_Title;

      ClientDataObject.Page.Id = ClientDataObject.Id;
      ClientDataObject.Page.Title = ClientDataObject.Title;
      ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
      this.LogDebug ( "Page.Status: " + ClientDataObject.Page.EditAccess );

      // 
      // Add the save groupCommand
      //
      Evado.Model.UniForm.Command saveCommand = ClientDataObject.Page.addCommand (
        EdLabels.ApplicationProfiles_Save_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Application_Properties.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Save_Object );

      //
      // Create the General Group.

      this.createGeneralGroup ( ClientDataObject.Page );

      //
      // create the configuration pageMenuGroup object.
      //
      this.createConfigurationGroup ( ClientDataObject.Page );

      //  
      // Create the email properties group.
      //
      this.createEmailGroup ( ClientDataObject.Page );

      this.LogMethodEnd ( "getDataObject" );

    }//END Method

    // ==============================================================================
    /// <summary>
    /// This method add the groups commands
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void AddGroupCommands (
      Evado.Model.UniForm.Group PageGroup )
    {
      this.LogMethod ( "AddGroupCommands" );

      // 
      // Add the save groupCommand
      //
      Evado.Model.UniForm.Command saveCommand = PageGroup.addCommand (
        EdLabels.ApplicationProfiles_Save_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Application_Properties.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Save_Object );

      this.LogMethodEnd ( "AddGroupCommands" );

    }//ENd AddGroupCommands method

    // ==============================================================================
    /// <summary>
    /// This method returns a general pageMenuGroup object.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void createGeneralGroup (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "createGeneralGroup" );
      this.LogValue ( " version: " + this.GlobalObjects.AdapterSettings.Version );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );
      Evado.Model.UniForm.Field pageField = new Evado.Model.UniForm.Field ( );

      // 
      // create the page pageMenuGroup
      // 
      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
        EdLabels.ApplicationProfile_General_Group_Title,
        Evado.Model.UniForm.EditAccess.Inherited );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      //
      // Add the group commands.
      //
      this.AddGroupCommands ( pageGroup );

      // 
      // Create the home page title
      // 
      pageField = pageGroup.createTextField (
        Model.Digital.EdAdapterSettings.AdapterFieldNames.Version.ToString ( ),
        EdLabels.Application_Profile_Version_Field_Label,
        this.GlobalObjects.AdapterSettings.Version, 30 );
      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;
      pageField.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      // 
      // Create the home application URL
      // 
      pageField = pageGroup.createReadOnlyTextField (
        String.Empty,
        EdLabels.Application_Profile_Application_Url_Field_Title,
        this.GlobalObjects.ApplicationUrl );
      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      // 
      // Create the home Reset url
      // 
      pageField = pageGroup.createReadOnlyTextField (
        String.Empty,
        EdLabels.Application_Profile_Reset_Url_Field_Title,
        this.GlobalObjects.PasswordResetUrl );
      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      // 
      // Create the home support email address
      // 
      pageField = pageGroup.createReadOnlyTextField (
        String.Empty,
        EdLabels.Application_Support_Email_Address_Field_Title,
        this.GlobalObjects.SupportEmailAddress );
      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      // 
      // Create the home support email address
      // 
      pageField = pageGroup.createReadOnlyTextField (
        String.Empty,
        EdLabels.Application_NoReply_Email_Address_Field_Title,
        this.GlobalObjects.NoReplyEmailAddress );
      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;


    }//END createGeneralGroup Method

    // ==============================================================================
    /// <summary>
    /// This method returns a general configuration Group object.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void createConfigurationGroup (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "createConfigurationGroup" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );
      Evado.Model.UniForm.Field pageField = new Evado.Model.UniForm.Field ( );
      String stOptionListValue = String.Empty;

      // 
      // create the page pageMenuGroup
      // 
      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
        EdLabels.ApplicationProfile_Configuration_Group_Title,
        Evado.Model.UniForm.EditAccess.Inherited );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      //
      // Add the group commands.
      //
      this.AddGroupCommands ( pageGroup );

      //
      // create the demonstration account expiry period in days.
      //
      pageField = pageGroup.createFreeTextField (
        Model.Digital.EdAdapterSettings.AdapterFieldNames.Roles.ToString ( ),
        EdLabels.Config_Role_List_Field_Label,
        this.GlobalObjects.AdapterSettings.Roles, 50, 10);
      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      this.LogDebug ( "DemoAccountExpiryDays {0}", this.GlobalObjects.AdapterSettings.DemoAccountExpiryDays );
      //
      // create the demonstration account expiry period in days.
      //
      pageField = pageGroup.createNumericField (
        Model.Digital.EdAdapterSettings.AdapterFieldNames.DemoAccountExpiryDays.ToString ( ),
        EdLabels.Settings_Demo_Account_Expiry_Field_Label,
        this.GlobalObjects.AdapterSettings.DemoAccountExpiryDays, 0, 365 );
      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      //
      // create the demonstratoin page video URL .
      //
      pageField = pageGroup.createTextField (
        Model.Digital.EdAdapterSettings.AdapterFieldNames.DemoRegistrationVideoUrl.ToString ( ),
        EdLabels.Settings_Demo_Video_URL_Field_Label,
        this.GlobalObjects.AdapterSettings.DemoRegistrationVideoUrl, 100 );
      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      // 
      // Create the help url field
      // 
      pageField = pageGroup.createTextField (
        Model.Digital.EdAdapterSettings.AdapterFieldNames.HelpUrl.ToString ( ),
        EdLabels.Application_Profile_Help_Url_Field_Label,
        this.GlobalObjects.AdapterSettings.HelpUrl,
        50 );
      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      this.LogMethodEnd ( "createEmailGroup" );

    }//END createEmailGroup method


    // ==============================================================================
    /// <summary>
    /// This method returns a general configuration Group object.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void createEmailGroup (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "createEmailGroup" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );
      Evado.Model.UniForm.Field pageField = new Evado.Model.UniForm.Field ( );
      String stOptionListValue = String.Empty;

      // 
      // create the page pageMenuGroup
      // 
      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
        EdLabels.Application_Profile_Email_Group_Title,
        Evado.Model.UniForm.EditAccess.Inherited );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      //
      // Add the group commands.
      //
      this.AddGroupCommands ( pageGroup );

      // 
      // Create the SMTP Server
      // 
      pageField = pageGroup.createTextField (
        Model.Digital.EdAdapterSettings.AdapterFieldNames.SmtpServer.ToString ( ),
        EdLabels.Application_Profile_SMTP_Server_Field_Label,
        this.GlobalObjects.AdapterSettings.SmtpServer,
        50 );
      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      // 
      // Create the SMTP Server
      // 
      pageField = pageGroup.createNumericField (
        Model.Digital.EdAdapterSettings.AdapterFieldNames.SmtpServerPort.ToString ( ),
        EdLabels.Application_Profile_SMTP_Port_Field_Label,
        this.GlobalObjects.AdapterSettings.SmtpServerPort, 0, 650000 );
      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      // 
      // Create the SMTP user
      // 
      pageField = pageGroup.createTextField (
        Model.Digital.EdAdapterSettings.AdapterFieldNames.SmtpUserId.ToString ( ),
        EdLabels.Application_Profile_SMTP_User_Field_Label,
        this.GlobalObjects.AdapterSettings.SmtpUserId,
        50 );
      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      // 
      // Create the SMTP user
      // 
      pageField = pageGroup.createTextField (
        Model.Digital.EdAdapterSettings.AdapterFieldNames.SmtpPassword.ToString ( ),
        EdLabels.Application_Profile_SMTP_Password_Field_Label,
        this.GlobalObjects.AdapterSettings.SmtpPassword,
        50 );
      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      this.LogMethodEnd ( "createEmailGroup" );
    }//END createEmailGroup Method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class create  object methods

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="Command">Evado.UniForm.Model.Command object.</param>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData createObject ( Evado.Model.UniForm.Command Command )
    {
      try
      {
        this.LogMethod ( "createObject" );

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + ".createObject",
          this.Session.UserProfile );

        // 
        // Initialise the methods variables and objects.
        //      
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );

        //
        // Determine if the user has access to this page and log and error if they do not.
        //
        if ( this.Session.UserProfile.hasManagementAccess == false
          && this.Session.UserProfile.hasEndUserRole( this.Session.Record.Design.ReadAccessRoles ) == false )
        {
          this.LogIllegalAccess (
            this.ClassNameSpace + ".createObject",
            this.Session.UserProfile );

          this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

          return null;
        }

        //
        // Initialise the dlinical ResultData objects.
        //
        this.GlobalObjects.AdapterSettings = new Model.Digital.EdAdapterSettings ( );
        this.GlobalObjects.AdapterSettings.Guid = Guid.NewGuid ( );

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
        this.ErrorMessage = "Error raised when creating a trial site.";

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return null;

    }//END method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class update  object methods

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData updateObject ( Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "updateObject" );
      this.LogValue ( "CommamdParameters: " + PageCommand.getAsString ( false, true ) );

      try
      {
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
        // Update the object.
        // 
        this.updateObjectValue ( PageCommand.Parameters );

        foreach ( EvObjectParameter parm in this.GlobalObjects.AdapterSettings.Parameters )
        {
          this.LogDebug ( "Parameter N: {0}, V: {1}", parm.Name, parm.Value );
        }

        this.LogDebug ( "DemoAccountExpiryDays {0}", this.GlobalObjects.AdapterSettings.DemoAccountExpiryDays );

        // 
        // update the object.
        // 
        EvEventCodes result = this._bll_AdapterConfig.updateItem ( this.GlobalObjects.AdapterSettings );

        // 
        // get the debug ResultData.
        // 
        this.LogValue ( this._bll_AdapterConfig.Log );

        // 
        // if an error state is returned create log the event.
        // 
        if ( result != EvEventCodes.Ok )
        {
          string StEvent = this._bll_AdapterConfig.Log + " returned error message: " + Evado.Model.Digital.EvcStatics.getEventMessage ( result );
          this.LogError ( EvEventCodes.Database_Record_Update_Error, StEvent );

          switch ( result )
          {
            default:
              {
                this.ErrorMessage = EdLabels.ApplicationProfiles_Update_Error_Message;
                break;
              }
          }
          return this.Session.LastPage;
        }

        return new Model.UniForm.AppData ( );

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.ApplicationProfiles_Update_Error_Message;

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
    private void updateObjectValue ( List<Evado.Model.UniForm.Parameter> Parameters )
    {
      this.LogValue ( Evado.Model.UniForm.EuStatics.CONST_METHOD_START
        + " Evado.UniForm.Clinical.ApplicationProperties.updateObjectValue method. "
        + " Parameters.Count: " + Parameters.Count
        + " Customer.Guid: " + this.GlobalObjects.AdapterSettings.Guid );

      /// 
      /// Iterate through the parameter values updating the ResultData object
      /// 
      foreach ( Evado.Model.UniForm.Parameter parameter in Parameters )
      {
        if ( parameter.Name.Contains ( Evado.Model.Digital.EvcStatics.CONST_GUID_IDENTIFIER ) == true
          || parameter.Name == Evado.Model.UniForm.CommandParameters.Custom_Method.ToString ( )
          || parameter.Name == Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION
          || parameter.Name == EdAdapterConfig.CONST_ADDRESS_FIELD_ID
          || parameter.Name == EdAdapterConfig.CONST_CURRENT_FIELD_ID )
        {
          continue;
        }

        this.LogValue ( parameter.Name + " > " + parameter.Value + " >> UPDATED" );

        try
        {
          Model.Digital.EdAdapterSettings.AdapterFieldNames fieldName =
             Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue<Model.Digital.EdAdapterSettings.AdapterFieldNames> (
            parameter.Name );

          this.GlobalObjects.AdapterSettings.setValue ( fieldName, parameter.Value );

        }
        catch ( Exception Ex )
        {
          this.LogException ( Ex );
        }

      }// End iteration loop

    }//END updateObjectValue method.

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

  }
}//END namespace