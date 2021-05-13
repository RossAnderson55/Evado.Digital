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
using  Evado.Model.Digital;
// using Evado.Web;

namespace Evado.UniForm.Clinical
{
  /// <summary>
  /// This class defines the application base classs that is used to terminate the 
  /// hosted application objects.
  /// 
  /// This class terminates the Customer object.
  /// </summary>
  public class EuAncillaryRecords : EuClassAdapterBase
  {
    #region Class Initialisation
    /// <summary>
    /// This method initialises the class.
    /// </summary>
    public EuAncillaryRecords ( )
    {
      this.ClassNameSpace = "Evado.UniForm.Clinical.AncillaryRecords.";
    }

    /// <summary>
    /// This method initialises the class and passs in the user profile.
    /// </summary>
    public EuAncillaryRecords (
      EuApplicationObjects ApplicationObjects,
      EvUserProfileBase ServiceUserProfile,
      EuSession SessionObjects,
      String UniFormBinaryFilePath,
      String UniFormServiceBinaryUrl,
      String FileRepositoryPath,
      EvClassParameters Settings )
    {
      this.ApplicationObjects = ApplicationObjects;
      this.ServiceUserProfile = ServiceUserProfile;
      this.Session = SessionObjects;
      this.UniForm_BinaryFilePath = UniFormBinaryFilePath;
      this.UniForm_BinaryServiceUrl = UniFormServiceBinaryUrl;
      this._FileRepositoryPath = FileRepositoryPath;
      this.ClassParameters = Settings;
      this.ClassNameSpace = "Evado.UniForm.Clinical.EuAncillaryRecords.";

      this.LogInitMethod ( "EuAncilliaryRecords initialisation" );
      this.LogInit ( "ServiceUserProfile.UserId: " + ServiceUserProfile.UserId );
      this.LogInit ( "SessionObjects.Project.ProjectId: " + this.Session.Trial.TrialId );
      this.LogInit ( "SessionObjects.UserProfile.Userid: " + this.Session.UserProfile.UserId );
      this.LogInit ( "SessionObjects.UserProfile.CommonName: " + this.Session.UserProfile.CommonName );
      this.LogInit ( "UniFormBinaryFilePath: " + this.UniForm_BinaryFilePath );
      this.LogInit ( "UniFormBinaryServiceUrl: " + this.UniForm_BinaryServiceUrl );
      this.LogInit ( "RepositoryFilePath: " + this._FileRepositoryPath );
;
      this.LogInit ( "Settings.LoggingLevel: " + Settings.LoggingLevel );
      this.LogInit ( "Settings.UserProfile.UserId: " + Settings.UserProfile.UserId );
      this.LogInit ( "Settings.UserProfile.CommonName: " + Settings.UserProfile.CommonName );

    }//END Method


    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class enumerations

    private enum SubjectRecordAccess
    {
      Record_Read_Only,

      Record_Author_Access,

      Record_Review_Access,

      Record_Monitor_Access,

      Record_Data_Manager_Access,

      Record_Approval_Access,
    }

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class constants and variables.

    private Evado.Bll.Clinical.EvAncillaryRecords _Bll_AncillaryRecords = new Evado.Bll.Clinical.EvAncillaryRecords ( );
  
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
    public Evado.Model.UniForm.AppData getClientDataObject ( Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getClientDataObject" );
      this.LogValue ( "Parameter PageCommand " + PageCommand.getAsString ( false, false ) );

      try
      {
        // 
        // Initialise the methods variables and objects.
        // 
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );

        String stViewType = PageCommand.GetParameter ( Evado.Model.UniForm.CommandParameters.Page_Id.ToString ( ) );

        this.LogValue ( " stViewType: " + stViewType );

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
              clientDataObject = this.getObject ( PageCommand );

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

    #region Class private methods

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
        this.LogMethod ( "getListObject" );
        this.LogValue ( "UserProfile.CommonName: " + this.Session.UserProfile.CommonName );

        //
        // Determine if the user has access to this page and log and error if they do not.
        //
        if ( this.Session.UserProfile.hasRecordAccess == false )
        {
          this.LogIllegalAccess (
            "Evado.UniForm.Clinical.AncilliaryRecords.getObject",
            this.Session.UserProfile );

          this.ErrorMessage = EvLabels.Illegal_Page_Access_Attempt;

          return null;
        }

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          "Evado.UniForm.Clinical.AncilliaryRecords.getListObject",
          this.Session.UserProfile );

        // 
        // Initialise the methods variables and objects.
        //      
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
        Evado.Model.UniForm.Command groupCommand = new Model.UniForm.Command ( );
        Evado.Model.UniForm.Group pageGroup = new Model.UniForm.Group ( );

        clientDataObject.Title = EvLabels.Ancillary_Record_List_Page_Title;
        clientDataObject.Page.Title = clientDataObject.Title;
        clientDataObject.Id = Guid.NewGuid ( );
        this.LogValue ( "data.Page.Title: " + clientDataObject.Page.Title );

        // 
        // Create the new pageMenuGroup.
        // 
        pageGroup = clientDataObject.Page.AddGroup (
        EvLabels.Ancillary_Record_List_Page_Group_Title,
        Evado.Model.UniForm.EditAccess.Enabled );
        pageGroup.CmdLayout = Evado.Model.UniForm.GroupCommandListLayouts.Vertical_Orientation;
        pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

        if ( this.Session.UserProfile.hasRecordEditAccess == true )
        {
          groupCommand = pageGroup.addCommand ( 
            EvLabels.Ancillary_Record_Add_Record_Command_Title, 
            EuAdapter.APPLICATION_ID, 
            EuAdapterClasses.Ancillary_Record.ToString ( ),
            Model.UniForm.ApplicationMethods.Create_Object );

          groupCommand.SetBackgroundColour (
            Model.UniForm.CommandParameters.BG_Default,
            Model.UniForm.Background_Colours.Purple );
        }

        // 
        // get the list of customers.
        // 
        List<EvAncillaryRecord> siteList = this._Bll_AncillaryRecords.getView (
          this.Session.Trial.TrialId,
          this.Session.Subject.SubjectId,
          String.Empty,
         String.Empty );

        this.LogValue ( this._Bll_AncillaryRecords.DebugLog );
        this.LogValue ( "list count: " + siteList.Count );

        // 
        // generate the page links.
        // 
        foreach ( EvAncillaryRecord ancillaryRecord in siteList )
        {

          groupCommand = pageGroup.addCommand(
            ancillaryRecord.RecordId, 
            EuAdapter.APPLICATION_ID,
            EuAdapterClasses.Ancillary_Record.ToString ( ),
            Evado.Model.UniForm.ApplicationMethods.Get_Object );

          groupCommand.SetGuid(  ancillaryRecord.Guid );

          groupCommand.Title = ancillaryRecord.Subject
            + EvLabels.Space_Open_Bracket
            + EvLabels.Label_Date
            + ancillaryRecord.stRecordDate
            + EvLabels.Space_Close_Bracket;
        }//END ancillary command iteration loop.

        if ( pageGroup.CommandList.Count == 0 )
        {
          pageGroup.Description = EvLabels.Ancillary_Record_List_Page_No_Records_Label ;
        }


        this.LogValue ( "command object count: " + pageGroup.CommandList.Count );

        return clientDataObject;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Ancillary_Record_Page_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return null;

    }//END getListObject method.

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
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
      Guid RecordGuid = Guid.Empty;
      this.Session.FileMetaDataList = new List<EvBinaryFileMetaData> ( );

      //
      // Determine if the user has access to this page and log and error if they do not.
      //
      if ( this.Session.UserProfile.hasRecordAccess == false )
      {
        this.LogIllegalAccess (
          "Evado.UniForm.Clinical.AncilliaryRecords.getObject",
          this.Session.UserProfile );

        this.ErrorMessage = EvLabels.Illegal_Page_Access_Attempt;

        return null;
      }

      // 
      // Log access to page.
      // 
      this.LogPageAccess (
        "Evado.UniForm.Clinical.AncilliaryRecords.getObject",
        this.Session.UserProfile );

      // 
      // if the parameter value exists then set the customerId
      // 
      RecordGuid = PageCommand.GetGuid ( );
      this.LogValue ( "RecordGuid: " + RecordGuid );

      // 
      // return if not trial id
      // 
      if ( RecordGuid == Guid.Empty )
      {
        this.LogValue ( "Record GUID is empty." );
        return null;
      }

      this.LogValue ( "Record exists" );

      try
      {
        // 
        // Retrieve the customer object from the database via the DAL and BLL layers.
        // 
        this.Session.AncillaryRecord = this._Bll_AncillaryRecords.getRecord ( RecordGuid );

        this.LogValue ( this._Bll_AncillaryRecords.DebugLog );

        this.LogValue ( "SubjectRecord.RecordId: "
          + this.Session.AncillaryRecord.RecordId );

        // 
        // return the client ResultData object for the customer.
        // 
        this.getClientDataObject ( clientDataObject );

        this.LogMethodEnd ( "getObject" );

        return clientDataObject;
      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Ancillary_Record_Page_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      this.LogMethodEnd ( "getObject" );
      return null;

    }//END getObject method

    // ==============================================================================
    /// <summary>
    /// This method sets the user access to the record object.
    /// </summary>
    /// <returns>RecordAccess object</returns>
    //  ------------------------------------------------------------------------------
    private SubjectRecordAccess setUserRecordAccess ( )
    {
      this.LogMethod ( "setUserRecordAccess" );
      //
      // Initialise methods variables and objects.
      //
      SubjectRecordAccess access = SubjectRecordAccess.Record_Read_Only;
      // 
      // Switch to select the user's access to the record.
      // 
      switch ( this.Session.AncillaryRecord.State )
      {
        case EvFormObjectStates.Draft_Record:
        case EvFormObjectStates.Queried_Record:
          {
            if ( this.Session.UserProfile.hasRecordEditAccess == true )
            {
              access = SubjectRecordAccess.Record_Author_Access;
            }
            break;
          }
        case EvFormObjectStates.Submitted_Record:
          {
            if ( this.Session.UserProfile.hasMonitorAccess == true )
            {
              access = SubjectRecordAccess.Record_Review_Access;
            }
            break;
          }
        case EvFormObjectStates.Source_Data_Verified:
          {
            if ( this.Session.UserProfile.hasDataManagerAccess == true )
            {
              access = SubjectRecordAccess.Record_Data_Manager_Access;
            }
            break;
          }
      }//END switch

      this.LogValue ( "Access: " + access );

      this.LogMethodEnd ( "setUserRecordAccess" );

      return access;
    }//END setUserRecordAccess

    // ==============================================================================
    /// <summary>
    /// This method creates a save groupCommand.
    /// </summary>
    /// <returns>Evado.Model.UniForm.Command object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.UniForm.Command createRecordSaveCommand (
      Guid RecordGuid,
      String Title,
      String SaveAction )
    {
      this.LogMethod ( "createRecordSaveCommand" );
      //
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Parameter parameter = new Evado.Model.UniForm.Parameter ( );

      // 
      // initialise the save groupCommand.
      // 
      Evado.Model.UniForm.Command saveCommand = new Evado.Model.UniForm.Command (
        Title,
        EuAdapter.APPLICATION_ID,
        EuAdapterClasses.Ancillary_Record.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Save_Object );


      // 
      // Define the groupCommand parameters.
      // 
      parameter = new Evado.Model.UniForm.Parameter ( "Guid", RecordGuid.ToString ( ) );
      saveCommand.Parameters.Add ( parameter );

      parameter = new Evado.Model.UniForm.Parameter ( "SaveAction", SaveAction );
      saveCommand.Parameters.Add ( parameter );

      return saveCommand;

    }//END createRecordSaveCommand method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="ClientDataObject">Evado.Model.UniForm.AppData object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private void getClientDataObject (
      Evado.Model.UniForm.AppData ClientDataObject )
    {
      this.LogMethod ( "getClientDataObject" );
      // 
      // Initialise the methods variables and objects.
      // 
      ClientDataObject.Id = this.Session.AncillaryRecord.Guid;
      ClientDataObject.Title = EvLabels.Ancillary_Record_Object_Page_Title
        + this.Session.AncillaryRecord.RecordId;

      ClientDataObject.Page.Id = ClientDataObject.Id;
      ClientDataObject.Page.Title = ClientDataObject.Title;
      ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      // 
      // Define the save and delete groupCommand parameters
      // 
      Evado.Model.UniForm.Parameter parameter = new Evado.Model.UniForm.Parameter (
        "Guid", ClientDataObject.Id.ToString ( ) );

      //
      // get the page commands.
      //
      this.getClientDataObject_PageCommands ( ClientDataObject.Page );

      // 
      // create the page pageMenuGroup
      // 
      this.getClientDataObject_FieldGroup ( ClientDataObject.Page );

      // 
      // create the page pageMenuGroup
      // 
      this.getClientDataObject_BinaryFileGroup ( ClientDataObject.Page );

      this.getClientDataObject_SignoffLog_Group ( ClientDataObject.Page );

      this.LogMethodEnd ( "getClientDataObject" );

    }//END getClientDataObject Method

    //===================================================================================
    /// <summary>
    /// This method generates the relevant page commands for the milestone record page.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object</param>
    //-----------------------------------------------------------------------------------
    private void getClientDataObject_PageCommands (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getClientDataObject_PageCommands" );
      //
      // Set the page access and commands based on the user role and the record state.
      //
      switch ( this.Session.AncillaryRecord.State )
      {
        case EvFormObjectStates.Draft_Record:
        case EvFormObjectStates.Queried_Record:
        case EvFormObjectStates.Submitted_Record:
          {
            if ( this.Session.UserProfile.hasRecordEditAccess == true )
            {
              PageObject.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

              // 
              // Add the save groupCommand and add it to the groupCommand list.
              // 
              Evado.Model.UniForm.Command saveCommand =  PageObject.addCommand( this.createRecordSaveCommand (
                this.Session.AncillaryRecord.Guid,
                EvLabels.Ancillary_Record_Save_Command_Title,
                EvAncillaryRecords.ACTION_SAVE ) );
              /*
              // 
              // Add the create groupCommand and add it to the groupCommand list.
              // 
              Evado.Model.UniForm.Command submitRecord = PageObject.addCommand( this.createRecordSaveCommand (
                this.SessionObjects.AncillaryRecord.Guid,
                EvLabels.Ancillary_Record_Submit_Command_Title,
                EvAncillaryRecords.ACTION_SIGNED ) );
               */
            }
            break;
          }
        default:
          {
            PageObject.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
            break;
          }
      }//END switch

      this.LogMethodEnd ( "getClientDataObject_PageCommands" );

    }//END getClientDataObject_PageCommands method

    //===================================================================================
    /// <summary>
    /// This method generates the relevant page page field for the milestone record page.
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page object</param>
    //-----------------------------------------------------------------------------------
    private void getClientDataObject_FieldGroup (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getClientDataObject_FieldGroup" );

      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.UniForm.Field pageField = new Evado.Model.UniForm.Field ( );
      Evado.Model.UniForm.Group pageGroup = new Model.UniForm.Group ( );

      //
      // generate the pageMenuGroup object.
      //
      pageGroup = Page.AddGroup (
        EvLabels.Ancillary_Record_Object_Page_Title,
        Evado.Model.UniForm.EditAccess.Inherited_Access );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      // 
      // Create the customer id object
      // 
      pageField = pageGroup.createReadOnlyTextField (
        EvAncillaryRecord.SubjectRecordFieldNames.RecordId.ToString ( ),
        EvLabels.Ancillary_Record_RecordId_Field_Label,
        this.Session.AncillaryRecord.RecordId );
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;

      // 
      // Create the customer name object
      // 
      pageField = pageGroup.createTextField (
        EvAncillaryRecord.SubjectRecordFieldNames.Subject.ToString ( ),
        EvLabels.Ancillary_Record_Record_Subject_Field_Label,
        this.Session.AncillaryRecord.Subject,
        50 );
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;

      // 
      // Create the record subject object
      // 
      pageField = pageGroup.createDateField (
        EvAncillaryRecord.SubjectRecordFieldNames.RecordDate.ToString ( ),
        EvLabels.Ancillary_Record_Date_Field_Label,
        this.Session.AncillaryRecord.RecordDate );

      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;

      // 
      // Create the record content object
      // 
      pageField = pageGroup.createFreeTextField (
        EvAncillaryRecord.SubjectRecordFieldNames.Record.ToString ( ),
        EvLabels.Ancillary_Record_Record_Content_Field_Label,
        this.Session.AncillaryRecord.Record,
        100, 15 );
      pageField.Layout = Model.UniForm.FieldLayoutCodes.Column_Layout;

    }//END getClientDataObject_FieldGroup method.

    //===================================================================================
    /// <summary>
    /// This method generates the relevant inary files groups for the milestone record page.
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page object</param>
    //-----------------------------------------------------------------------------------
    private void getClientDataObject_BinaryFileGroup (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getClientDataObject_BinaryFileGroup" );
      // 
      // Initialise the methods variables and objects.
      // 
      this.Session.FileMetaDataList = new List<EvBinaryFileMetaData> ( );

      //
      // Exit if binary ResultData is not being collected.
      //
      if ( this.Session.Trial.Data.EnableBinaryData == false )
      {
        this.LogValue ( "Trial not collecting binary data." );

        return;
      }

      //
      // Initialise the binary file object to retrieve binary files associated with the visit.
      // 
      EuBinaryFiles binaryFiles = new EuBinaryFiles (
              this.ApplicationObjects,
              this.ServiceUserProfile,
              this.Session,
              this.UniForm_BinaryFilePath,
              this.UniForm_BinaryServiceUrl,
              this._FileRepositoryPath,
              this.ClassParameters );

      binaryFiles.LoggingLevel = this.LoggingLevel;

      this.LogValue ( "Initialise the binary files class. " + binaryFiles.Log );

      Evado.Model.UniForm.Group group = binaryFiles.getListGroup ( EuAdapterClasses.Ancillary_Record, 200 );

      this.LogValue ( "binaryFiles.getListGroup debuglog: " + binaryFiles.Log );

      if ( group != null )
      {
        this.LogValue ( "Display group not null" );
        Page.AddGroup ( group );
      }

      //
      // If the user has edit access add binary upload pageMenuGroup.
      //
      if ( Page.EditAccess == Evado.Model.UniForm.EditAccess.Enabled )
      {
        //
        // Get the binary file pageMenuGroup
        //
        group = binaryFiles.getUploadGroup ( EuAdapterClasses.Ancillary_Record );

        this.LogValue ( "binaryFiles.getUploadGroup debuglog: " + binaryFiles.Log );

        if ( group != null )
        {
          this.LogValue ( "Upload group not null" );
          Page.AddGroup ( group );
        }
      }

      this.LogMethodEnd ( "getClientDataObject_BinaryFileGroup" );

    }//END getClientDataObject_BinaryFileGroup method

    // ==============================================================================
    /// <summary>
    /// This method creates the project signoff pageMenuGroup
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page object object.</param>
    // ------------------------------------------------------------------------------
    private void getClientDataObject_SignoffLog_Group (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getClientDataObject_SignoffLog" );

      // 
      // Display comments it they exist.
      // 
      if ( this.Session.AncillaryRecord.Signoffs == null )
      {
        this.Session.AncillaryRecord.Signoffs = new List<EvUserSignoff> ( );
      }
      // 
      // Display comments it they exist.
      // 
      if ( this.Session.AncillaryRecord.Signoffs.Count == 0 )
      {
        this.LogValue ( EvLabels.Label_No_Signoff_Label );
        return;
      }

      // 
      // create the page pageMenuGroup
      // 
      Evado.Model.UniForm.Field pageField = new Evado.Model.UniForm.Field ( );
      Evado.Model.UniForm.Group pageGroup = Page.AddGroup (
        EvLabels.Label_Signoff_Log_Group_Title,
        Evado.Model.UniForm.EditAccess.Enabled );

      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      // 
      // Create the record state object
      // 
      pageField = pageGroup.createReadOnlyTextField (
        String.Empty,
        EvLabels.Ancillary_Record_Status_Field_Label,
        this.Session.AncillaryRecord.StateDesc );

      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;

      pageField = pageGroup.createReadOnlyTextField (
        String.Empty,
        EvLabels.Label_Signoff_Log_Field_Title,
        String.Empty,
        EvUserSignoff.getSignoffLog ( this.Session.AncillaryRecord.Signoffs, false ) );
      pageField.Layout = EuFormGenerator.ApplicationFieldLayout;

    }//END getProjectPage_SignoffLog Method

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
        this.LogMethod ( "createObject" );
        // 
        // Initialise the methods variables and objects.
        //      
        Evado.Model.UniForm.AppData data = new Evado.Model.UniForm.AppData ( );
        this.Session.AncillaryRecord = new EvAncillaryRecord ( );
        this.Session.AncillaryRecord.Guid =  Evado.Model.Clinical.EvcStatics.CONST_NEW_OBJECT_ID;
        this.Session.AncillaryRecord.State = EvFormObjectStates.Draft_Record;
        this.Session.AncillaryRecord.RecordDate = DateTime.Now;

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          "Evado.UniForm.Clinical.SubjectRecords.createObject",
          this.Session.UserProfile );

        this.getClientDataObject ( data );


        this.LogValue ( "Exit createObject method. ID: " + data.Id + ", Title: " + data.Title );

        return data;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = "Error raised when creating a subject record.";

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return null;

    }//END method

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData updateObject ( 
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "updateObject" );
      this.LogValue ( "PageEvado.Model.UniForm.Command: " + PageCommand.getAsString ( false, false ) );

      this.LogValue ( "Guid: " + this.Session.Trial.Guid );
      this.LogValue ( "ProjectId: " + this.Session.Trial.TrialId );
      this.LogValue ( "Title: " + this.Session.Trial.Title );
      try
      {
        //
        // Initialise the methods variables and objects.
        //
        this.Session.AncillaryRecord.Action = EvAncillaryRecords.ACTION_SAVE;

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          "Evado.UniForm.Clinical.SubjectRecords.updateObject",
          this.Session.UserProfile );

        // 
        // Delete the object.
        // 
        if ( PageCommand.Method == Evado.Model.UniForm.ApplicationMethods.Delete_Object )
        {
          return this.Session.LastPage;
        }

        //
        // Initialise the binary file object to retrieve binary files associated with the visit.
        // 
        EuBinaryFiles binaryFiles = new EuBinaryFiles (
                this.ApplicationObjects,
                this.ServiceUserProfile,
                this.Session,
                this.UniForm_BinaryFilePath,
                this.UniForm_BinaryServiceUrl,
                this._FileRepositoryPath,
                this.ClassParameters );

        this.LogClass ( binaryFiles.Log );

        // 
        // Get the save action value.
        // 
        String stSaveAction = PageCommand.GetParameter (  Evado.Model.Clinical.EvcStatics.CONST_SAVE_ACTION );
        if ( stSaveAction != String.Empty )
        {
          this.Session.AncillaryRecord.Action = stSaveAction;
        }
        this.LogValue ( "AncillaryRecord.Action: " + this.Session.AncillaryRecord.Action );

        //
        // if the guid is set to new object reset it to empty to add a new record.
        //
        if ( this.Session.AncillaryRecord.Guid ==  Evado.Model.Clinical.EvcStatics.CONST_NEW_OBJECT_ID )
        {
          this.Session.AncillaryRecord.Guid = Guid.NewGuid ( );
          this.Session.AncillaryRecord.SubjectId = this.Session.Subject.SubjectId;
          this.Session.AncillaryRecord.ProjectId = this.Session.Subject.TrialId;
          this.Session.AncillaryRecord.Action = EvAncillaryRecords.ACTION_NEW;
        }

        if (
          this.Session.AncillaryRecord.RecordDate ==  Evado.Model.Clinical.EvcStatics.CONST_DATE_NULL )
        {
          this.Session.AncillaryRecord.RecordDate = DateTime.Now;
        }

        // 
        // Initialise the update variables.
        // 
        this.Session.AncillaryRecord.UpdatedByUserId = this.Session.UserProfile.UserId;
        this.Session.AncillaryRecord.UserCommonName = this.Session.UserProfile.CommonName;
        // 
        // Update the object.
        // 
        this.updateObjectValue ( PageCommand.Parameters );

        //
        // save the binary files to the file repository.
        // 
        binaryFiles.addBinaryData (
          PageCommand,
          this.Session.Subject.Guid,
          this.Session.Subject.SubjectId,
          this.Session.AncillaryRecord.Guid,
          this.Session.AncillaryRecord.RecordId,
          Guid.Empty, String.Empty );

        this.LogValue ( "BinaryFiles: " + binaryFiles.Log );

        // 
        // update the object.
        // 
        EvEventCodes result = this._Bll_AncillaryRecords.saveItem ( this.Session.AncillaryRecord );

        // 
        // get the debug ResultData.
        // 
        this.LogValue ( this._Bll_AncillaryRecords.DebugLog );

        // 
        // if an error state is returned create log the event.
        // 
        if ( result != EvEventCodes.Ok )
        {
          string StEvent = this._Bll_AncillaryRecords.Log + " returned error message: " + Evado.Model.Clinical.EvcStatics.getEventMessage ( result );
          this.LogError ( EvEventCodes.Database_Record_Update_Error, StEvent );

          switch ( result )
          {
            case EvEventCodes.Data_Duplicate_Id_Error:
              {
                this.ErrorMessage =
                  String.Format (
                    EvLabels.Activity_Duplicate_Identifier_Error_Message,
                    this.Session.Activity.ActivityId );
                break;
              }
            case EvEventCodes.Identifier_Project_Id_Error:
              {
                this.ErrorMessage = EvLabels.Project_Identifier_Empty_Error_Message;
                break;
              }
            case EvEventCodes.Identifier_Subject_Id_Error:
              {
                this.ErrorMessage = EvLabels.Subject_Identifier_Empty_Error_Message;
                break;
              }
            default:
              {
                this.ErrorMessage = EvLabels.Ancilliary_Record_Update_Error_Message;
                break;
              }
          }
          return this.Session.LastPage;
        }

        return new Model.UniForm.AppData();

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Ancilliary_Record_Update_Error_Message;

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
    private bool updateObjectValue ( List<Evado.Model.UniForm.Parameter> Parameters )
    {
      this.LogMethod ( "updateObjectValue" );
      this.LogValue ( "Parameters.Count: " + Parameters.Count );
      this.LogValue ( "Customer.Guid: " + this.Session.Trial.Guid );

      // 
      // Iterate through the parameter values updating the ResultData object
      // 
      foreach ( Evado.Model.UniForm.Parameter parameter in Parameters )
      {
        this.LogValue ( "" + parameter.Name + " > " + parameter.Value );

        if ( parameter.Name == EvAncillaryRecord.SubjectRecordFieldNames.Subject.ToString ( ) )
        {
          this.Session.AncillaryRecord.Subject = parameter.Value;
        }

        if ( parameter.Name == EvAncillaryRecord.SubjectRecordFieldNames.Record.ToString ( ) )
        {
          this.Session.AncillaryRecord.Record = parameter.Value;
        }

      }// End iteration loop

      return true;

    }//END method.

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
  }
}//END namespace