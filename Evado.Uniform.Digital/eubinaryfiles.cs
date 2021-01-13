/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\TrialOrganisations.cs" company="EVADO HOLDING PTY. LTD.">
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
  /// This class manages the integration of Evado eclinical binary files functions with 
  /// Evado.UniFORM interface.
  /// </summary>
  /// <remarks>Because the binary files are always access through either the Visit page or the FirstSubject
  /// Records page, this class does provide a full interface to the UniForm application services class. 
  /// But rather provide a page pageMenuGroup level component generation that can be called from within the 
  /// page using the binary file functionality.
  /// 
  /// This approach would allow the binary file functions to be called from other pages as well.
  /// </remarks>
  public class EuBinaryFiles : EuClassAdapterBase
  {
    #region Class Initialisation

    // ==================================================================================
    /// <summary>
    /// This method initialises the class.
    /// </summary>
    //  ----------------------------------------------------------------------------------
    public EuBinaryFiles ( )
    {
      this.ClassNameSpace = "Evado.UniForm.Clinical.EuBinaryFiles.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class and passs in the user profile.
    /// </summary>
    //  ----------------------------------------------------------------------------------
    public EuBinaryFiles (
      EuApplicationObjects ApplicationObjects,
      EvUserProfileBase ServiceUserProfile,
      EuSession SessionObjects,
      String BinaryFilePath,
      String BinaryServiceUrl,
      String FileRepositoryPath,
      EvClassParameters Settings )
    {
      this.ApplicationObjects = ApplicationObjects;
      this.ServiceUserProfile = ServiceUserProfile;
      this.Session = SessionObjects;
      this.UniForm_BinaryFilePath = BinaryFilePath;
      this.UniForm_BinaryServiceUrl = BinaryServiceUrl;
      this.FileRepositoryPath = FileRepositoryPath;
      this.ClassParameters = Settings;
      this.ClassNameSpace = "Evado.UniForm.Clinical.EuBinaryFiles.";

      this.LogInitMethod ( "EuBinaryFiles initialisation" );
      this.LogInit ( "ServiceUserProfile.UserId: " + ServiceUserProfile.UserId );
      this.LogInit ( "SessionObjects.Project.ProjectId: " + this.Session.Application.ApplicationId );
      this.LogInit ( "SessionObjects.UserProfile.Userid: " + this.Session.UserProfile.UserId );
      this.LogInit ( "SessionObjects.UserProfile.CommonName: " + this.Session.UserProfile.CommonName );
      this.LogInit ( "UniFormBinaryFilePath: " + this.UniForm_BinaryFilePath );
      this.LogInit ( "FileRepositoryPath: " + this._FileRepositoryPath );

      this.LogInit ( "Settings.LoggingLevel: " + Settings.LoggingLevel );
      this.LogInit ( "Settings.UserProfile.UserId: " + Settings.UserProfile.UserId );
      this.LogInit ( "Settings.UserProfile.CommonName: " + Settings.UserProfile.CommonName );

      this.LoggingLevel = Settings.LoggingLevel;

    }//END Method


    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class constants and variables.

    public const string CONST_BINARY_FILE = "BIN_FILE";
    public const string CONST_BINARY_FILE_PREFIX = "BIN_FILE_";
    public const string CONST_BINARY_TITLE_PREFIX = "BIN_TITLE_";

    private const string CONST_BINARY_FILE_ORG_ID = "BFUO";
    private const string CONST_BINARY_FILE_ID = "BFUID";
    private const string CONST_UPLOAD_PARM_ID = "UPLOAD";

    public const string CONST_DEFAULT_IMAGE_EXTENSION = ".jpg";


    private Evado.Bll.Clinical.EvBinaryFiles _Bll_BinaryFiles = new Evado.Bll.Clinical.EvBinaryFiles ( );
    List<EvOption> _MimeList = new List<EvOption> ( );
    private Evado.Model.Digital.EvBinaryFileMetaData _BinaryFile = new EvBinaryFileMetaData ( );
    private int _NoPageFileUploadFields = 5;
    private bool _FileUpload = false;

    /// <summary>
    /// This property defines the number of file upload fields to be displayed on the upload page.
    /// </summary>
    public int NoPageFileUploadFields
    {
      get
      {
        return _NoPageFileUploadFields;
      }
      set
      {
        this._NoPageFileUploadFields = value;
      }
    }

    private bool _DisplayFileTitle = false;

    /// <summary>
    /// This property indicates if the file title is to be displayed.
    /// </summary>
    public bool DisplayFileTitle
    {
      get
      {
        return _DisplayFileTitle;
      }
      set
      {
        this._DisplayFileTitle = value;
      }
    }

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region publc getDataObject method.

    // ==================================================================================
    /// <summary>
    /// This method gets the application ResultData object for a project groupCommand request. 
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Evado.Model.UniForm.AppData</returns>
    //  ----------------------------------------------------------------------------------
    override public Evado.Model.UniForm.AppData getDataObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getDataObject" );
      this.LogValue ( "PageCommand " + PageCommand.getAsString ( false, true ) );

      //
      // Initialise the binary file objects.
      //
      if ( this.Session.BinaryFileList == null )
      {
        this.Session.BinaryFileList = new List<EvBinaryFileMetaData> ( );
        this.Session.BinaryFile = new EvBinaryFileMetaData ( );
        this.Session.BinaryFileVersionList = new List<EvBinaryFileMetaData> ( );
        this.Session.BinaryFileId = String.Empty;
        this.Session.BinaryFileOrgId = String.Empty;
      }

      try
      {
        // 
        // Initialise the methods variables and objects.
        // 
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );

        // 
        // Retrieve the customer object from the database via the DAL and BLL layers.
        // 
        if ( this.Session.Application.ApplicationId == String.Empty )
        {
          return this.Session.LastPage;
        }

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
          case Evado.Model.UniForm.ApplicationMethods.Save_Object:
          case Evado.Model.UniForm.ApplicationMethods.Delete_Object:
            {
              clientDataObject = this.getObject ( PageCommand );

              break;
            }
          case Evado.Model.UniForm.ApplicationMethods.Create_Object:
            {
              clientDataObject = this.createObject ( PageCommand );

              break;
            }
        }//END Switch

        this.LogDebug ( "PUBLIC: Page ID {0}, title {1}, Groups {2}.",
            clientDataObject.Id,
            clientDataObject.Title,
            clientDataObject.Page.GroupList.Count );

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


        this.LogDebug ( "PUBLIC: Page ID {0}, title {1}, Groups {2}.",
          clientDataObject.Id,
          clientDataObject.Title,
          clientDataObject.Page.GroupList.Count );

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

    #region Class private list methods

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
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

        //
        // Determine if the user has access to this page and log and error if they do not.
        //
        if ( this.Session.UserProfile.hasManagementAccess == false )
        {
          this.LogIllegalAccess (
           this.ClassNameSpace + "getListObject",
            this.Session.UserProfile );

          this.ErrorMessage = EvLabels.Illegal_Page_Access_Attempt;

          return null;
        }

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "getListObject",
          this.Session.UserProfile );

        //
        // Update the session identifier values.
        //
        this.saveBinaryFile_Page_Parameters ( PageCommand );

        //
        // initialise the date object values.
        //
        clientDataObject.Title =
          String.Format (
          EvLabels.Binary_File_List_Of_Files_Page_Label,
          this.Session.Application.ApplicationId,
          this.Session.Application.Title );
        clientDataObject.Page.Title = clientDataObject.Title;
        clientDataObject.Id = Guid.NewGuid ( );

        // 
        // get the list of customers.
        // 
        this.Session.BinaryFileList = this._Bll_BinaryFiles.getBinaryFileList (
          this.Session.Application.ApplicationId,
          this.Session.BinaryFileOrgId,
          String.Empty );

        this.LogClass ( this._Bll_BinaryFiles.Log );

        this.LogDebug ( "BinaryFileList.Count: " + this.Session.BinaryFileList.Count );

        //
        // the binary file selection group.
        //
        this.getBinaryFileList_Selection_Group ( clientDataObject.Page );

        //
        // the binary file list group.
        //
        this.getBinaryFileList_Group ( clientDataObject.Page );

        this.LogMethodEnd ( "getListObject" );
        return clientDataObject;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Binary_Files_Get_List_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      this.LogMethodEnd ( "getListObject" );
      return this.Session.LastPage;

    }//END getListObject method.


    // ==============================================================================
    /// <summary>
    /// This method appends a group object containing the file's metadata.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object object.</param>
    // ------------------------------------------------------------------------------
    private void getBinaryFileList_Selection_Group (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getBinaryFileList_Selection_Group" );
      //
      // initialise the methods variables and objects.
      //
      Evado.Model.UniForm.Field groupField = new Model.UniForm.Field ( );
      Evado.Model.UniForm.Command groupCommand = new Model.UniForm.Command ( );

      // 
      // create the list page selection group
      // 
      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
       EvLabels.Binary_File_List_Selection_Group_Title,
        Evado.Model.UniForm.EditAccess.Enabled );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      // 
      // Create a custom groupCommand to process the selection.
      // 
      groupCommand = pageGroup.addCommand (
        EvLabels.HomePage_Select_Project,
        EuAdapter.APPLICATION_ID,
         EuAdapterClasses.Binary_File.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Custom_Method );

      groupCommand.setCustomMethod ( Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

      this.LogMethodEnd ( "getBinaryFileList_Selection_Group" );

    }//END getList_Selection_Group method

    // ==============================================================================
    /// <summary>
    /// This method appends a group object containing the file's metadata.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object object.</param>
    // ------------------------------------------------------------------------------
    private void getBinaryFileList_Group (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getList_Group" );
      //
      // initialise the methods variables and objects.
      //
      Evado.Model.UniForm.Field groupField = new Model.UniForm.Field ( );
      Evado.Model.UniForm.Command groupCommand = new Model.UniForm.Command ( );

      // 
      // Create the new list group.
      // 
      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
        EvLabels.Project_List_Of_Projects_Group_Label,
        Evado.Model.UniForm.EditAccess.Enabled );
      pageGroup.CmdLayout = Evado.Model.UniForm.GroupCommandListLayouts.Vertical_Orientation;
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      // 
      // Add the new trial groupCommand list.
      // 
      Evado.Model.UniForm.Command newFileCommand = pageGroup.addCommand (
      EvLabels.Binary_File_Create_Command_Title,
      EuAdapter.APPLICATION_ID,
      EuAdapterClasses.Binary_File.ToString ( ),
      Evado.Model.UniForm.ApplicationMethods.Create_Object );

      newFileCommand.SetBackgroundColour (
        Model.UniForm.CommandParameters.BG_Default,
        Model.UniForm.Background_Colours.Purple );


      // 
      // generate the page links.
      // 
      foreach ( EvBinaryFileMetaData file in this.Session.BinaryFileList )
      {
        //
        // If the registry module is not loaded do not display registry projects.
        //
        if ( file.FileExists == false )
        {
          continue;
        }

        Evado.Model.UniForm.Command command = pageGroup.addCommand (
          String.Format ( EvLabels.Binary_File_List_Command_Title,
           file.FileId,
           file.Title,
           file.Version,
           file.UpdatedByDate.ToString ( "dd MMM yyyy" ) ),
           EuAdapter.APPLICATION_ID,
           EuAdapterClasses.Binary_File,
           Model.UniForm.ApplicationMethods.Get_Object );

        command.AddParameter (
          EvIdentifiers.TRIAL_ID,
          file.TrialId );

        command.AddParameter (
          EvIdentifiers.ORGANISATION_ID,
          file.GroupId );

        command.AddParameter ( EuBinaryFiles.CONST_BINARY_FILE_ID,
          file.FileId );
      }

      this.LogDebug ( "pageGroup.CommandList.Count: " + pageGroup.CommandList.Count );

      this.LogMethodEnd ( "getList_Group" );

    }//END getList_Selection_Group method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class private get File upload object methods

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

      //
      // Determine if the user has access to this page and log and error if they do not.
      //
      if ( this.Session.UserProfile.hasManagementAccess == false )
      {
        this.LogIllegalAccess (
          this.ClassNameSpace + "getObject",
          "hasConfigrationAccess",
          this.Session.UserProfile );

        this.ErrorMessage = EvLabels.Illegal_Page_Access_Attempt;

        return this.Session.LastPage;
      }

      // 
      // Log access to page.
      // 
      this.LogPageAccess (
        this.ClassNameSpace + "getObject",
        this.Session.UserProfile );

      try
      {

        this.loadMimeTypes ( );

        this.saveBinaryFile_Page_Parameters ( PageCommand );

        //
        // Upload the binary data from the file list.
        //
        this.loadBinaryFileMetaData ( );

        var result = this.uploadBinaryFile ( PageCommand );

        //
        // exit the page if the upload failed.
        //
        if ( result != EvEventCodes.Ok )
        {
          this.LogMethodEnd ( "getObject" );
          return this.Session.LastPage;
        }


        this.getVersionedFileList ( );

        this.getClientDataObject ( clientDataObject );

        this.LogMethodEnd ( "getObject" );

        return clientDataObject;
      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Binary_Get_Page_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }
      this.LogMethodEnd ( "getObject" );
      return this.Session.LastPage;

    }//END getObject method

    // ==============================================================================
    /// <summary>
    /// This method updates the session variables for the binary file upload
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object object.</param>
    // ------------------------------------------------------------------------------
    private void saveBinaryFile_Page_Parameters (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getFileUpload_Update_Session" );
      String value = string.Empty;

      //
      // if the org Id parameter exists update the setting.
      //
      if ( PageCommand.hasParameter ( EvIdentifiers.ORGANISATION_ID ) == true )
      {
        value = PageCommand.GetParameter ( EvIdentifiers.ORGANISATION_ID );


        if ( this.Session.BinaryFileOrgId != value )
        {
          this.Session.BinaryFile = new EvBinaryFileMetaData ( );
          this.Session.BinaryFileList = new List<EvBinaryFileMetaData> ( );
          this.Session.BinaryFileId = String.Empty;
          this.Session.BinaryFileVersionList = new List<EvBinaryFileMetaData> ( );
        }

        this.Session.BinaryFileOrgId = value;
      }

      //
      // Update the binary file identifier.
      if ( PageCommand.hasParameter ( EuBinaryFiles.CONST_BINARY_FILE_ID ) == true )
      {
        value = PageCommand.GetParameter ( CONST_BINARY_FILE_ID );

        if ( value != String.Empty )
        {
          this.Session.BinaryFileId = value;
        }
      }

      this._FileUpload = false;
      if ( PageCommand.hasParameter ( EuBinaryFiles.CONST_UPLOAD_PARM_ID ) == true )
      {
        value = PageCommand.GetParameter ( EuBinaryFiles.CONST_UPLOAD_PARM_ID );

        this._FileUpload = EvStatics.getBool ( value );
      }

      this.LogDebug ( "BinaryFileOrgId: " + this.Session.BinaryFileOrgId );
      this.LogDebug ( "BinaryFileId: " + this.Session.BinaryFileId );
      this.LogDebug ( "FileUpload: " + this._FileUpload );

      this.LogMethodEnd ( "getFileUpload_Update_Session" );
    }

    // ==============================================================================
    /// <summary>
    /// This method loads the selected files metadata.
    /// </summary>
    //  ------------------------------------------------------------------------------
    private void loadBinaryFileMetaData ( )
    {
      this.LogMethod ( "loadBinaryFileMetaData" );
      this.LogDebug ( "Current: BinaryFile.FileId: " + this.Session.BinaryFile.FileId );

      if ( this.Session.BinaryFileList.Count == 0 )
      {
        this.Session.BinaryFile = new EvBinaryFileMetaData ( );
        this.LogMethodEnd ( "loadBinaryFileMetaData" );
        return;
      }

      foreach ( EvBinaryFileMetaData file in this.Session.BinaryFileList )
      {
        if ( file.FileId == this.Session.BinaryFileId )
        {
          this.Session.BinaryFile = file;
        }
      }

      this.LogDebug ( "BinaryFile.ProjectId: " + this.Session.BinaryFile.TrialId );
      this.LogDebug ( "BinaryFile.GroupId: " + this.Session.BinaryFile.GroupId );
      this.LogDebug ( "BinaryFile.FileId: " + this.Session.BinaryFile.FileId );
      this.LogDebug ( "BinaryFile.Title: " + this.Session.BinaryFile.Title );
      this.LogDebug ( "BinaryFile.Comments: " + this.Session.BinaryFile.Comments );
      this.LogDebug ( "BinaryFile.FileName: " + this.Session.BinaryFile.FileName );
      this.LogDebug ( "BinaryFile.Version: " + this.Session.BinaryFile.Version );

      this.LogMethodEnd ( "loadBinaryFileMetaData" );

    }//END Method

    // ==============================================================================
    /// <summary>
    /// This method create a list of version files of a specific FileId
    /// </summary>
    // ------------------------------------------------------------------------------
    private void getVersionedFileList ( )
    {
      this.LogMethod ( "getVersionedFileList" );
      EvBinaryFiles binaryFiles = new EvBinaryFiles ( this.ClassParameters );

      if ( this.Session.BinaryFileVersionList.Count > 0 )
      {
        return;
      }

      this.Session.BinaryFileVersionList = binaryFiles.GetVersionedFileList (
        this.Session.Application.ApplicationId,
        this.Session.BinaryFileOrgId,
        String.Empty,
        this.Session.BinaryFileId );

      this.LogDebug ( "BinaryFileVersionList.Count: " + this.Session.BinaryFileVersionList.Count );

      this.LogMethodEnd ( "getVersionedFileList" );

    }//ENd getVersionedFileList method

    // ==============================================================================
    /// <summary>
    /// This method create a list of version files of a specific FileId
    /// </summary>
    // ------------------------------------------------------------------------------
    private EvEventCodes uploadBinaryFile (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "uploadBinaryFile" );
      this.LogDebug ( "Upload file enabled {0}", this._FileUpload );

      if ( this._FileUpload == false )
      {
        this.LogMethodEnd ( "uploadBinaryFile" );
        return EvEventCodes.Ok;
      }
      this._FileUpload = false;

      //
      // Initialise the methods variables and objects.
      //
      this._BinaryFile = new EvBinaryFileMetaData (
        this.Session.BinaryFile.TrialGuid,
        this.Session.BinaryFile.GroupGuid,
        Guid.Empty,
        this.Session.BinaryFile.TrialId,
        this.Session.BinaryFile.GroupId,
        String.Empty,
        this.Session.BinaryFile.Title,
        String.Empty,
        this.Session.BinaryFile.MimeType );

      this._BinaryFile.Guid = Guid.NewGuid ( );
      this._BinaryFile.FileGuid = this.Session.BinaryFile.FileGuid;
      this._BinaryFile.FileId = this.Session.BinaryFile.FileId;
      this._BinaryFile.Version = this.Session.BinaryFile.Version;

      //this.LogDebug ( "BinaryFile.TrialId: " + this._BinaryFile.TrialId );
      //this.LogDebug ( "BinaryFile.GroupId: " + this._BinaryFile.GroupId );
      //this.LogDebug ( "BinaryFile.FileId: " + this._BinaryFile.FileId );
      //this.LogDebug ( "BinaryFile.Title: " + this._BinaryFile.Title );
      //this.LogDebug ( "BinaryFile.Comments: " + this._BinaryFile.Comments );
      //this.LogDebug ( "BinaryFile.FileName: " + this._BinaryFile.FileName );
      //this.LogDebug ( "BinaryFile.Version: " + this._BinaryFile.Version );
     
      //
      // If guid is empty then this is an new document not updating an existing doucment.
      //
      if ( this._BinaryFile.FileGuid == Guid.Empty )
      {
        this.LogDebug ( "binary file is empty." );
        Guid orgGuid = Guid.Empty;

        this._BinaryFile.FileGuid = Guid.NewGuid ( );
        this._BinaryFile.TrialId = this.Session.Application.ApplicationId;
        this._BinaryFile.TrialGuid = this.Session.Application.Guid;
        this._BinaryFile.GroupId = this.Session.BinaryFileOrgId;
        this._BinaryFile.GroupGuid = orgGuid;
      }//END emtpty binary object.

      //
      // Update from the page paramater values.
      //
      this.updateObjectValue ( PageCommand );

      this.LogDebug ( "UPDATED VALUES: " );
      //this.LogDebug ( "BinaryFile.TrialGuid: " + this._BinaryFile.TrialGuid );
      //this.LogDebug ( "BinaryFile.TrialId: " + this._BinaryFile.TrialId );
      //this.LogDebug ( "BinaryFile.GroupGuid: " + this._BinaryFile.GroupGuid );
      //this.LogDebug ( "BinaryFile.GroupId: " + this._BinaryFile.GroupId );
      //this.LogDebug ( "BinaryFile.FileGuid: " + this._BinaryFile.FileGuid );
      //this.LogDebug ( "BinaryFile.FileId: " + this._BinaryFile.FileId );
      //this.LogDebug ( "BinaryFile.Title: " + this._BinaryFile.Title );
      //this.LogDebug ( "BinaryFile.Comments: " + this._BinaryFile.Comments );
      //this.LogDebug ( "BinaryFile.FileName: " + this._BinaryFile.FileName );
      //this.LogDebug ( "BinaryFile.Version: " + this._BinaryFile.Version );

      //
      // save the binary file
      //
      var result = this.addBinaryFileData ( this._BinaryFile );

      if ( result != EvEventCodes.Ok )
      {
        switch ( result )
        {
          case EvEventCodes.File_Directory_Path_Empty:
            {
              this.ErrorMessage = EvLabels.Binary_File_Upload_Failed_Error_Message;
              break;
            }
          default:
            {
              this.ErrorMessage = EvLabels.Binary_File_Upload_Failed_Error_Message; 
              break;
            }
        }
        this.LogError (result, this.ErrorMessage );
        this.LogMethodEnd ( "uploadBinaryFile" );
        this._FileUpload = false;
        return result;
      }

      this.Session.BinaryFile = this._BinaryFile;
      this.Session.BinaryFileVersionList = new List<EvBinaryFileMetaData> ( );
      this.Session.BinaryFileList = new List<EvBinaryFileMetaData> ( );

      this._FileUpload = false;

      this.LogMethodEnd ( "uploadBinaryFile" );
      return EvEventCodes.Ok;

    }//ENd uploadBinaryFile method

    // ==================================================================================
    /// <summary>
    /// THis method updates the EvActivity object member with groupCommand parameter values.
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.PageCommand object.</param>
    //  ----------------------------------------------------------------------------------
    private void updateObjectValue (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "updateObjectValue" );
      this.LogValue ( "Parameters.Count: " + PageCommand.Parameters.Count );

      // 
      // Iterate through the parameter values updating the ResultData object
      // 
      foreach ( Evado.Model.UniForm.Parameter parameter in PageCommand.Parameters )
      {
        if ( parameter.Name == Evado.Model.Digital.EvcStatics.CONST_GUID_IDENTIFIER
          || parameter.Name == Evado.Model.UniForm.CommandParameters.Custom_Method.ToString ( )
          || parameter.Name == EvIdentifiers.TRIAL_ID
          || parameter.Name == EvIdentifiers.ORGANISATION_ID
          || parameter.Name == EuBinaryFiles.CONST_UPLOAD_PARM_ID )
        {
          continue;
        }

        try
        {
          EvBinaryFileMetaData.ClassFieldNames fieldName =
            Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue<EvBinaryFileMetaData.ClassFieldNames> ( parameter.Name );

          this._BinaryFile.setValue ( fieldName, parameter.Value );
        }
        catch ( Exception Ex )
        {
          this.LogValue ( parameter.Name + " > " + parameter.Value + " >> UPDATE FAILED" );
          this.LogException ( Ex );
          this.LogMethodEnd ( "updateObjectValue" );
        }

      }// End iteration loop

      this.LogMethodEnd ( "updateObjectValue" );

    }//END method.

    // ==============================================================================
    /// <summary>
    /// This method returns the application ResultData object defines the project 
    /// configuraiton page.
    /// </summary>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private void getClientDataObject (
      Evado.Model.UniForm.AppData ClientDataObject )
    {
      this.LogMethod ( "getClientDataObject" );
      // 
      // Initialise the methods variables and objects.
      //  
      ClientDataObject.Id = Guid.NewGuid ( );
      ClientDataObject.Page.PageDataGuid = ClientDataObject.Id;
      ClientDataObject.Page.Id = ClientDataObject.Id;
      ClientDataObject.Title =
        String.Format (
          EvLabels.Binary_File_Metadata_Page_Title,
          this.Session.BinaryFile.FileId,
          this.Session.BinaryFile.Title );
      ClientDataObject.Page.Title = ClientDataObject.Title;

      if ( this.Session.BinaryFile.FileId == String.Empty )
      {
        ClientDataObject.Title = EvLabels.Binary_File_Empty_Page_Title;
      }
      //
      // If the trial object is empty, index.e. new create a client ResultData id 
      // so it will be displayed.
      //
      if ( this.Session.BinaryFile.Guid == Guid.Empty )
      {
        ClientDataObject.Id = Guid.NewGuid ( );
        ClientDataObject.Id = ClientDataObject.Page.Id;
      }

      //
      // Set the user edit access to the objects.
      //
      ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Disabled;

      if ( this.Session.UserProfile.hasManagementEditAccess == true )
      {
        ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
      }

      //
      // Get the page commands.
      //
      this.getFileMetadata_Group ( ClientDataObject.Page );

      //
      // Get the trial general pageMenuGroup.
      //
      this.getFileVersionList_Group ( ClientDataObject.Page );

      //
      // get the binary file upload group.
      //
      this.getFileUpload_Group ( ClientDataObject.Page );

      this.LogMethodEnd ( "getClientDataObject" );

    }//END Method

    // ==============================================================================
    /// <summary>
    /// This method appends a group object containing the file's metadata.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object object.</param>
    // ------------------------------------------------------------------------------
    private void getFileMetadata_Group (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getFileMetadata_Group" );
      //
      // initialise the methods variables and objects.
      //
      Evado.Model.UniForm.Field groupField = new Model.UniForm.Field ( );
      Evado.Model.UniForm.Command groupCommand = new Model.UniForm.Command ( );

      //
      // If the guid is empty then it is an empty file object.
      //
      if ( this.Session.BinaryFile.Guid == Guid.Empty )
      {
        return;
      }

      // 
      // create the page pageMenuGroup
      // 
      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
       EvLabels.Binary_File_Metadata_Group_Title,
        Evado.Model.UniForm.EditAccess.Enabled );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      // 
      // Create the project id object
      // 
      groupField = pageGroup.createReadOnlyTextField (
        String.Empty,
        EvLabels.Label_Project_Id,
        this.Session.BinaryFile.TrialId );
      groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      // 
      // Create the project id object
      // 
      if ( this.Session.BinaryFile.GroupId != String.Empty )
      {
        groupField = pageGroup.createReadOnlyTextField (
          String.Empty,
          EvLabels.Label_Organisation_Id,
          this.Session.BinaryFile.GroupId );
        groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;
      }
      groupField = pageGroup.createReadOnlyTextField (
        String.Empty,
        EvLabels.Binary_File_Id_Field_Title,
        this.Session.BinaryFile.FileId );
      groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      groupField = pageGroup.createReadOnlyTextField (
        String.Empty,
        EvLabels.Binary_File_Title_Field_Title,
        this.Session.BinaryFile.Title );
      groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      groupField = pageGroup.createReadOnlyTextField (
        String.Empty,
        EvLabels.Binary_File_Comment_Field_Title,
        this.Session.BinaryFile.Comments );
      groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      groupField = pageGroup.createReadOnlyTextField (
        String.Empty,
        EvLabels.Binary_File_Version_Field_Title,
        this.Session.BinaryFile.Version.ToString ( "##" ) );
      groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      groupField = pageGroup.createReadOnlyTextField (
        String.Empty,
        EvLabels.Binary_File_Status_Field_Title,
         EvStatics.Enumerations.enumValueToString ( this.Session.BinaryFile.Status ) );
      groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      if ( this.Session.BinaryFile.UploadDate != EvStatics.CONST_DATE_NULL )
      {
        groupField = pageGroup.createReadOnlyTextField (
        String.Empty,
          EvLabels.Binary_File_Upload_Date_Field_Title,
          this.Session.BinaryFile.UploadDate.ToString ( "dd MMM yyyy" ) );
        groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;
      }

      if ( this.Session.BinaryFile.ReleaseDate != EvStatics.CONST_DATE_NULL )
      {
        groupField = pageGroup.createReadOnlyTextField (
        String.Empty,
          EvLabels.Binary_File_Release_Date_Field_Title,
          this.Session.BinaryFile.ReleaseDate.ToString ( "dd MMM yyyy" ) );
        groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;
      }

      if ( this.Session.BinaryFile.SupersededDate != EvStatics.CONST_DATE_NULL )
      {
        groupField = pageGroup.createReadOnlyTextField (
        String.Empty,
          EvLabels.Binary_File_Superseded_Date_Field_Title,
          this.Session.BinaryFile.SupersededDate.ToString ( "dd MMM yyyy" ) );
        groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;
      }

      groupField = pageGroup.createReadOnlyTextField (
        String.Empty,
        EvLabels.Binary_File_Name_Field_Title,
        this.Session.BinaryFile.FileName );
      groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      groupField = pageGroup.createReadOnlyTextField (
        String.Empty,
        EvLabels.Binary_File_MineType_Field_Title,
        this.Session.BinaryFile.MimeType );
      groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      this.LogMethodEnd ( "getFileMetadata_Group" );

    }//END getFileUpload_Selection_Group method

    // ==============================================================================
    /// <summary>
    /// This method generates a list of file versions.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object object.</param>
    // ------------------------------------------------------------------------------
    private void getFileVersionList_Group (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getFileVersionList_Group" );
      this.LogDebug ( "BinaryFileVersionList.Count: " + this.Session.BinaryFileVersionList.Count );
      //
      // initialise the methods variables and objects.
      //
      Evado.Model.UniForm.Field groupField = new Model.UniForm.Field ( );

      //
      // Only display the list if exists and the binary file object is not empty.
      //
      if ( this.Session.BinaryFileVersionList.Count == 0
        || this.Session.BinaryFile.Guid == Guid.Empty )
      {
        return;
      }

      // 
      // create the page versioned file group.
      // 
      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
       EvLabels.Project_General_Group_Title,
        Evado.Model.UniForm.EditAccess.Inherited );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      groupField = pageGroup.createTableField (
        String.Empty,
        EvLabels.Binary_File_Versioned_List_Field_Title, 3 );

      groupField.Table.Header = new Model.UniForm.TableColHeader [ 4 ];

      groupField.Table.Header [ 0 ] = new Model.UniForm.TableColHeader ( );
      groupField.Table.Header [ 0 ].No = 1;
      groupField.Table.Header [ 0 ].Text = EvLabels.Binary_File_Version_Column_0_Text; // Version
      groupField.Table.Header [ 0 ].TypeId = EvDataTypes.Text;

      groupField.Table.Header [ 1 ] = new Model.UniForm.TableColHeader ( );
      groupField.Table.Header [ 1 ].No = 2;
      groupField.Table.Header [ 1 ].Text = EvLabels.Binary_File_Version_Column_1_Text;  // Status
      groupField.Table.Header [ 1 ].TypeId = EvDataTypes.Text;

      groupField.Table.Header [ 2 ] = new Model.UniForm.TableColHeader ( );
      groupField.Table.Header [ 2 ].No = 3;
      groupField.Table.Header [ 2 ].Text = EvLabels.Binary_File_Version_Column_2_Text; // Comments
      groupField.Table.Header [ 2 ].TypeId = EvDataTypes.Text;

      groupField.Table.Header [ 3 ] = new Model.UniForm.TableColHeader ( );
      groupField.Table.Header [ 3 ].No = 4;
      groupField.Table.Header [ 3 ].Text = EvLabels.Binary_File_Version_Column_3_Text; // update date
      groupField.Table.Header [ 3 ].TypeId = EvDataTypes.Text;

      //
      // generate the versioned file table.
      //
      foreach ( EvBinaryFileMetaData file in this.Session.BinaryFileVersionList )
      {
        Evado.Model.UniForm.TableRow row = groupField.Table.addRow ( );

        row.Column [ 0 ] = file.Version.ToString ( "##" );
        row.Column [ 1 ] = EvStatics.Enumerations.enumValueToString ( file.Status );
        row.Column [ 2 ] = file.Comments;
        row.Column [ 3 ] = EvStatics.getDateAsString ( file.UpdatedByDate );
      }

      this.LogMethodEnd ( "getFileVersionList_Group" );

    }//END getProjectPage_General_Group method

    // ==============================================================================
    /// <summary>
    /// This method display a page to upload a new file verson.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object object.</param>
    // ------------------------------------------------------------------------------
    private void getFileUpload_Group (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getFileUpload_Group" );
      //
      // initialise the methods variables and objects.
      //
      Evado.Model.UniForm.Field groupField = new Model.UniForm.Field ( );
      Evado.Model.UniForm.Command groupCommand = new Model.UniForm.Command ( );

      // 
      // create the page pageMenuGroup
      // 
      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
       EvLabels.Binary_File_Upload_Group_Title,
        Evado.Model.UniForm.EditAccess.Enabled );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      //
      // Create the file identifer field
      //
      groupField = pageGroup.createTextField (
        EvBinaryFileMetaData.ClassFieldNames.FileId.ToString ( ),
        EvLabels.Binary_File_Id_Field_Title,
        this.Session.BinaryFile.FileId, 20 );
      groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      //
      // create the file title field
      //
      groupField = pageGroup.createTextField (
        EvBinaryFileMetaData.ClassFieldNames.Title.ToString ( ),
        EvLabels.Binary_File_Title_Field_Title,
        this.Session.BinaryFile.Title, 100 );
      groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      //
      // create the file comments/description field.
      //
      groupField = pageGroup.createTextField (
        EvBinaryFileMetaData.ClassFieldNames.Comments.ToString ( ),
        EvLabels.Binary_File_Comment_Field_Title,
        String.Empty, 100 );
      groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      //
      // create the binary file upload group.
      //
      groupField = pageGroup.createBinaryFileField (
        EvBinaryFileMetaData.ClassFieldNames.FileName.ToString ( ),
        EvLabels.Binary_File_Name_Field_Title,
        this.Session.BinaryFile.FileName );
      groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      //
      //Add the group UPLOAD command.
      //
      groupCommand = pageGroup.addCommand (
        EvLabels.Binary_File_Upload_Command_Title,
        EuAdapter.APPLICATION_ID,
        EuAdapterClasses.Binary_File,
        Model.UniForm.ApplicationMethods.Custom_Method );

      groupCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.Get_Object );
      groupCommand.AddParameter ( EuBinaryFiles.CONST_UPLOAD_PARM_ID, "1" );

      this.LogMethodEnd ( "getFileUpload_Group" );

    }//END getFileUpload_Group method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class private create object methods

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData createObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      try
      {
        this.LogMethod ( "createObject" );

        //
        // Determine if the user has access to this page and log and error if they do not.
        //
        if ( this.Session.UserProfile.hasManagementAccess == false )
        {
          this.LogIllegalAccess (
            this.ClassNameSpace + "createObject",
            this.Session.UserProfile );

          this.ErrorMessage = EvLabels.Illegal_Page_Access_Attempt;

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
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
        this._BinaryFile = new EvBinaryFileMetaData ( );
        this.Session.BinaryFile = new EvBinaryFileMetaData ( );
        this.Session.BinaryFileId = String.Empty;
        this.Session.BinaryFile.TrialId = this.Session.Application.ApplicationId;
        this.Session.BinaryFile.TrialGuid = this.Session.Application.Guid;

        //
        // Create the client page.
        //
        this.getClientDataObject ( clientDataObject );

        // 
        // Save clinical objects  to the session
        // 
        LogValue ( "Exit createObject method. ID: " + clientDataObject.Id + ", Title: " + clientDataObject.Title );

        return clientDataObject;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Binary_File_Creation_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return this.Session.LastPage;

    }//END createObject method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class pubic Group methods

    // ==============================================================================
    /// <summary>
    /// This method creates a pageMenuGroup containing links or commands to access binary file ResultData.
    /// Image files will be displayed as image thumb nails and non image files as http links
    /// to the file.
    /// </summary>
    /// <param name="ApplicationClassType">ApplicationService.ApplicationObjects: enumeration indicating the 
    /// type of application object that is calling the binary file function.</param>
    /// <returns>Evado.Model.UniForm.Group: pageMenuGroup containing the binary file references.</returns>
    //  ------------------------------------------------------------------------------
    public Evado.Model.UniForm.Group getProjectFileListGroup (
      String OrgId )
    {
      this.LogMethod ( "getProjectFileListGroup" );
      this.LogValue ( "OrgId: " + OrgId );
      this.LogValue ( "FileRepositoryPath: " + this._FileRepositoryPath );
      this.LogValue ( "UniForm BinaryPath: " + this.UniForm_BinaryFilePath );
      try
      {
        // 
        // Initialise the methods variables and objects.
        // 
        List<EvBinaryFileMetaData> binaryFileList = new List<EvBinaryFileMetaData> ( );
        bool bFileCopied = false;

        // 
        // Check that the file repository path defined.
        // 
        if ( this._FileRepositoryPath == String.Empty )
        {
          this.ErrorMessage = EvLabels.Binary_Files_Respository_Error_Message;
          this.LogValue ( EvLabels.Binary_Files_Respository_Error_Message );

          return null;
        }

        // 
        // Check that the UniFORM file path is defined.
        // 
        if ( this.UniForm_BinaryFilePath == String.Empty )
        {
          this.ErrorMessage = EvLabels.UniForm_Binary_Path_Error_Message;
          this.LogValue ( EvLabels.UniForm_Binary_Path_Error_Message );

          return null;
        }

        // 
        // Check that the UniFORM service url path is defined.
        // 
        if ( this.UniForm_BinaryServiceUrl == String.Empty )
        {
          this.ErrorMessage = EvLabels.UniForm_Binary_Service_Url_Error_Message;
          this.LogValue ( EvLabels.UniForm_Binary_Service_Url_Error_Message );

          return null;
        }

        // 
        // Create the binary file pageMenuGroup to display the images or links to the 
        // retrieved binary files.
        // 
        Evado.Model.UniForm.Group fileListGroup = new Model.UniForm.Group (
          EvLabels.Binary_Files_Project_List_Group_Title,
          Evado.Model.UniForm.EditAccess.Enabled );
        fileListGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
        fileListGroup.CmdLayout = Evado.Model.UniForm.GroupCommandListLayouts.Vertical_Orientation;
        fileListGroup.GroupType = Model.UniForm.GroupTypes.Default;

        //
        // Get the list of project files.
        //
        binaryFileList = this._Bll_BinaryFiles.getProjectFileList (
          this.Session.Application.ApplicationId,
          OrgId );

        this.LogClass ( this._Bll_BinaryFiles.Log );

        //   
        // if the list length is null then return a null object.
        // 
        if ( binaryFileList.Count == 0 )
        {
          fileListGroup.Description = EvLabels.Binary_File_List_Group_Empty_Message;
          this.LogValue ( "No file references returned." );

          return fileListGroup;
        }

        // 
        // Iterate through the list of binary file metadata objectsand
        // generate a list of fields that reference the objects.
        // 
        foreach ( EvBinaryFileMetaData binaryFile in binaryFileList )
        {
          binaryFile.RepositoryFilePath = this._FileRepositoryPath;

          this.LogValue ( "Binary file:  Title:" + binaryFile.Title );
          this.LogValue ( "MimeType:" + binaryFile.MimeType );
          this.LogValue ( "BinaryFileName: '" + binaryFile.FileName + "'" );
          this.LogValue ( "FilePath: '" + binaryFile.FullBinaryFilePath + "'" );
          this.LogValue ( "UniFormPath: '" + this.UniForm_BinaryFilePath + binaryFile.FileName + "'" );

          //
          // Copy the binary ResultData file into the UniFORM binary directory for user acsess.
          //
          bFileCopied = true;
          try
          {
            System.IO.File.Copy ( binaryFile.FullBinaryFilePath, this.UniForm_BinaryFilePath + binaryFile.FileName, true );
          }
          catch ( Exception Ex )
          {
            this.LogValue ( " exception: " + Evado.Model.Digital.EvcStatics.getException ( Ex ) );
            bFileCopied = false;
          }

          this.LogValue ( "bFileCopied: " + bFileCopied );

          // 
          // Create UniForm field object for the binary files.
          // 
          this.getProjectFileObject ( fileListGroup, binaryFile );

        }//END trial organisation list iteration loop

        this.LogMethodEnd ( "getProjectFileListGroup" );

        // 
        // Return true to indicate that the fields have been created.
        // 
        return fileListGroup;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Alert_List_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );

        this.LogMethodEnd ( "getProjectFileListGroup" );

        return null;
      }

    }//END getProjectFileListGroup method.

    // ==============================================================================
    /// <summary>
    /// This method adds binary file to the FileListGroup object
    /// </summary>
    /// <param name="FileListGroup">Evado.Model.UniForm.Group object.</param>
    /// <param name="BinaryFile">Evado.Model.EvBinaryFileMetaData object.</param>
    //  ------------------------------------------------------------------------------
    private void getProjectFileObject (
      Evado.Model.UniForm.Group FileListGroup,
      EvBinaryFileMetaData BinaryFile )
    {
      this.LogMethod ( "getProjectFileObject" );

      this.LogDebug ( "MimeType: " + BinaryFile.MimeType );

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Field field = new Model.UniForm.Field ( );

      string title = String.Format ( EvLabels.Binary_File_List_Command_Title,
           BinaryFile.FileId,
           BinaryFile.Title,
           BinaryFile.Version,
           BinaryFile.UpdatedByDate.ToString ( "dd MMM yyyy" ) );
      //
      // Create a http field if the mime type is a supported http type.
      // 
      field = FileListGroup.createHtmlLinkField (
        BinaryFile.FileId,
        title,
        BinaryFile.FileName );

      this.LogDebug ( "URL: " + field.Value );

      field.Layout = Model.UniForm.FieldLayoutCodes.Center_Justified;

      this.LogMethodEnd ( "getProjectFileObject" );

      return;

    }//END getFieldDisplayObject method

    // ==============================================================================
    /// <summary>
    /// This method creates a pageMenuGroup containing links or commands to access binary file ResultData.
    /// Image files will be displayed as image thumb nails and non image files as http links
    /// to the file.
    /// </summary>
    /// <param name="ApplicationClassType">ApplicationService.ApplicationObjects: enumeration indicating the 
    /// type of application object that is calling the binary file function.</param>
    /// <returns>Evado.Model.UniForm.Group: pageMenuGroup containing the binary file references.</returns>
    //  ------------------------------------------------------------------------------
    public Evado.Model.UniForm.Group getListGroup (
      EuAdapterClasses ApplicationClassType,
      int PixelWidth )
    {
      this.LogMethod ( "getListGroup" );
      this.LogValue ( "ApplicationClassType: " + ApplicationClassType );
      this.LogValue ( "PixelWidth: " + PixelWidth );
      this.LogValue ( "FileRepositoryPath: " + this._FileRepositoryPath );
      this.LogValue ( "UniForm BinaryPath: " + this.UniForm_BinaryFilePath );
      try
      {
        // 
        // Initialise the methods variables and objects.
        // 
        List<EvBinaryFileMetaData> binaryFileList = new List<EvBinaryFileMetaData> ( );
        bool bFileCopied = false;

        // 
        // Check that the file repository path defined.
        // 
        if ( this._FileRepositoryPath == String.Empty )
        {
          this.ErrorMessage = EvLabels.Binary_Files_Respository_Error_Message;
          this.LogValue ( EvLabels.Binary_Files_Respository_Error_Message );

          return null;
        }

        // 
        // Check that the UniFORM file path is defined.
        // 
        if ( this.UniForm_BinaryFilePath == String.Empty )
        {
          this.ErrorMessage = EvLabels.UniForm_Binary_Path_Error_Message;
          this.LogValue ( EvLabels.UniForm_Binary_Path_Error_Message );

          return null;
        }

        // 
        // Check that the UniFORM service url path is defined.
        // 
        if ( this.UniForm_BinaryServiceUrl == String.Empty )
        {
          this.ErrorMessage = EvLabels.UniForm_Binary_Service_Url_Error_Message;
          this.LogValue ( EvLabels.UniForm_Binary_Service_Url_Error_Message );

          return null;
        }

        // 
        // Create the binary file pageMenuGroup to display the images or links to the 
        // retrieved binary files.
        // 
        Evado.Model.UniForm.Group fileListGroup = new Model.UniForm.Group (
          EvLabels.Binary_Files_List_Group_Title,
          Evado.Model.UniForm.EditAccess.Enabled );
        fileListGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
        fileListGroup.CmdLayout = Evado.Model.UniForm.GroupCommandListLayouts.Vertical_Orientation;
        fileListGroup.GroupType = Model.UniForm.GroupTypes.Default;


        //
        // Retrieve the binary file metadata for the correct object type.
        // 
        switch ( ApplicationClassType )
        {
          case EuAdapterClasses.Ancillary_Record:
            {
              binaryFileList = this._Bll_BinaryFiles.getBinaryFileList (
                this.Session.Application.ApplicationId,
                String.Empty,
                this.Session.AncillaryRecord.RecordId );

              this.LogClass ( this._Bll_BinaryFiles.Log );

              break;
            }

          case EuAdapterClasses.Applications:
            {
              binaryFileList = this._Bll_BinaryFiles.getBinaryFileList (
                this.Session.Application.ApplicationId,
                String.Empty,
                String.Empty );

              this.LogClass ( this._Bll_BinaryFiles.Log );

              break;
            }

          default:
            {
              this.LogValue ( "Incorrect object type has been passed to the" );

              this.LogMethodEnd ( "getListGroup" );

              return null;
            }
        }//END ObjectType switch

        //   
        // if the list length is null then return a null object.
        // 
        if ( binaryFileList.Count == 0 )
        {
          fileListGroup.Description = EvLabels.Binary_File_List_Group_Empty_Message;
          this.LogValue ( "No file references returned." );

          return fileListGroup;
        }

        // 
        // Iterate through the list of binary file metadata objectsand
        // generate a list of fields that reference the objects.
        // 
        foreach ( EvBinaryFileMetaData binaryFile in binaryFileList )
        {
          binaryFile.RepositoryFilePath = this._FileRepositoryPath;

          this.LogValue ( "Binary file:  Title:" + binaryFile.Title );
          this.LogValue ( "MimeType:" + binaryFile.MimeType );
          this.LogValue ( "BinaryFileName: '" + binaryFile.FileName + "'" );
          this.LogValue ( "FilePath: '" + binaryFile.FullBinaryFilePath + "'" );
          this.LogValue ( "UniFormPath: '" + this.UniForm_BinaryFilePath + binaryFile.FileName + "'" );

          //
          // Copy the binary ResultData file into the UniFORM binary directory for user acsess.
          //
          bFileCopied = true;
          try
          {
            System.IO.File.Copy ( binaryFile.FullBinaryFilePath, this.UniForm_BinaryFilePath + binaryFile.FileName, true );
          }
          catch ( Exception Ex )
          {
            this.LogValue ( " exception: " + Evado.Model.Digital.EvcStatics.getException ( Ex ) );
            bFileCopied = false;
          }

          this.LogValue ( "bFileCopied: " + bFileCopied );

          // 
          // Create UniForm field object for the binary files.
          // 
          this.getFieldDisplayObject ( fileListGroup, binaryFile, PixelWidth );

        }//END trial organisation list iteration loop

        this.LogMethodEnd ( "getListGroup" );

        // 
        // Return true to indicate that the fields have been created.
        // 
        return fileListGroup;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Alert_List_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );

        this.LogMethodEnd ( "getListGroup" );

        return null;
      }

    }//END getListGroup method.

    // ==============================================================================
    /// <summary>
    /// This method adds binary file to the FileListGroup object
    /// </summary>
    /// <param name="FileListGroup">Evado.Model.UniForm.Group object.</param>
    /// <param name="BinaryFile">Evado.Model.EvBinaryFileMetaData object.</param>
    //  ------------------------------------------------------------------------------
    private void getFieldDisplayObject (
      Evado.Model.UniForm.Group FileListGroup,
      EvBinaryFileMetaData BinaryFile,
      int PixelWidth )
    {
      this.LogMethod ( "getFieldDisplayObject" );

      this.LogValue ( "MimeType: " + BinaryFile.MimeType );
      this.LogValue ( "IsImage: " + BinaryFile.IsImage );
      this.LogValue ( "IsSound: " + BinaryFile.IsSound );

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Field field = new Model.UniForm.Field ( );

      //
      // Create a image field if the mime type is a supported image type.
      // 
      if ( BinaryFile.IsImage == true )
      {
        field = FileListGroup.createImageField (
          BinaryFile.FileId,
          BinaryFile.Title,
          BinaryFile.FileName,
          0, 0 );

        field.Layout = Model.UniForm.FieldLayoutCodes.Center_Justified;
        field.AddParameter ( Model.UniForm.FieldParameterList.Width, PixelWidth );

        this.LogValue ( "URL: " + field.Value );

        this.LogMethodEnd ( "getFieldDisplayObject" );

        return;
      }

      //
      // Create a sound field if the mime type is a supported sound type.
      // 
      if ( BinaryFile.IsSound == true )
      {
        field = FileListGroup.createSoundField (
          BinaryFile.FileId,
          BinaryFile.Title,
          BinaryFile.FileName );

        this.LogValue ( "URL: " + field.Value );

        field.Layout = Model.UniForm.FieldLayoutCodes.Center_Justified;

        this.LogMethodEnd ( "getFieldDisplayObject" );

        return;
      }

      //
      // Create a video field if the mime type is a supported video type.
      // 
      if ( BinaryFile.IsVideo == true )
      {
        field = FileListGroup.addField (
          BinaryFile.FileId,
          BinaryFile.Title,
          Evado.Model.EvDataTypes.Video,
          BinaryFile.FileName );

        this.LogValue ( "URL: " + field.Value );

        field.Layout = Model.UniForm.FieldLayoutCodes.Center_Justified;

        this.LogMethodEnd ( "getFieldDisplayObject" );

        return;
      }

      //
      // Create a http field if the mime type is a supported http type.
      // 
      field = FileListGroup.createHtmlLinkField (
        BinaryFile.FileId,
        BinaryFile.Title,
        BinaryFile.FileName );

      this.LogValue ( "URL: " + field.Value );

      field.Layout = Model.UniForm.FieldLayoutCodes.Center_Justified;

      this.LogMethodEnd ( "getFieldDisplayObject" );

      return;

    }//END getFieldDisplayObject method

    #endregion

    #region Class Upload Group methods

    // ==============================================================================
    /// <summary>
    /// This method creates a pageMenuGroup containing upload fields or links to collect image
    /// or file ResultData.
    /// </summary>
    /// <param name="ApplicationClassType">ApplicationService.ApplicationObjects: enumeration indicating the 
    /// type of application object that is calling the binary file function.</param>
    /// <returns>Evado.Model.UniForm.Group: pageMenuGroup containing the binary file references.</returns>
    //  ------------------------------------------------------------------------------
    public Evado.Model.UniForm.Group getUploadGroup (
      EuAdapterClasses ApplicationClassType )
    {
      this.LogMethod ( "getUploadGroup" );
      this.LogValue ( "ObjectType: " + ApplicationClassType );
      try
      {
        // 
        // Initialise the methods variables and objects.
        // 
        List<EvBinaryFileMetaData> binaryFileList = new List<EvBinaryFileMetaData> ( );
        Evado.Model.UniForm.Field field = new Model.UniForm.Field ( );
        this.resetAdapterLog ( );

        // 
        // Create the binary file pageMenuGroup to display the images or links to the 
        // retrieved binary files.
        // 
        Evado.Model.UniForm.Group imageFileGroup = new Model.UniForm.Group (
          EvLabels.Binary_Files_Upload_Group_Title,
          Evado.Model.UniForm.EditAccess.Enabled );
        imageFileGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
        imageFileGroup.CmdLayout = Evado.Model.UniForm.GroupCommandListLayouts.Vertical_Orientation;
        imageFileGroup.GroupType = Model.UniForm.GroupTypes.Default;

        imageFileGroup.Description = EvLabels.Binary_Files_Description_Text;

        //
        // Iterate generating the file upload fields in the group.
        //
        for ( int i = 0; i < this._NoPageFileUploadFields; i++ )
        {
          int uploadIndex = i + 1;
          string uploadTitleFieldId = EuBinaryFiles.CONST_BINARY_TITLE_PREFIX + uploadIndex;
          string uploadFieldId = EuBinaryFiles.CONST_BINARY_FILE_PREFIX + uploadIndex;
          string fieldTitleLabel = String.Format ( EvLabels.Binary_Files_Upload_Field_Title, uploadIndex );
          string fieldUploadLabel = String.Format ( EvLabels.Binary_Files_Upload_Field_Title, uploadIndex );
          // 
          // Add the image upload title.
          // 
          field = imageFileGroup.createTextField (
           uploadTitleFieldId,
           fieldUploadLabel,
            String.Empty,
            80 );

          // 
          // Add the image upload field.
          // 
          field = imageFileGroup.createImageField (
           uploadFieldId,
           fieldUploadLabel,
            String.Empty,
            0, 0 );

          field.Layout = Model.UniForm.FieldLayoutCodes.Center_Justified;
        }

        this.LogMethodEnd ( "getUploadGroup" );

        // 
        // Return true to indicate that the fields have been created.
        // 
        return imageFileGroup;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Alert_List_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );

        this.LogMethodEnd ( "getUploadGroup" );

        return null;
      }
    }//END Method 

    #endregion

    #region Class Save methods

    // ==================================================================================
    /// <summary>
    /// This method saves the uploaded images to the database and file repository.
    /// 
    /// The 
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.ClientClientDataObjectEvado.Model.UniForm.Command object.</param>
    /// <param name="ObjectGuid">Guid: unique object identifier</param>
    /// <param name="ObjectId">String: an object identifier</param>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    public bool addBinaryData (
      Evado.Model.UniForm.Command PageCommand,
      Guid GroupGuid,
      String GroupId,
      Guid SubGroupGuid,
      String SubGroupId,
      Guid FileGuid,
      String FileId )
    {
      this.LogMethod ( "addBinaryData" );
      this.LogDebug ( "GroupGuid: " + GroupGuid );
      this.LogDebug ( "GroupId: " + GroupId );
      this.LogDebug ( "GroupGuid: " + GroupGuid );
      this.LogDebug ( "SubGroupGuid: " + SubGroupGuid );
      this.LogDebug ( "SubGroupId: " + SubGroupId );
      this.LogDebug ( "FileGuid: " + FileGuid );
      this.LogDebug ( "FileId: " + FileId );
      this.LogDebug ( "Command: " + PageCommand.getAsString ( false, true ) );
      try
      {
        // 
        // Delete the object.
        // 
        if ( PageCommand.Method == Evado.Model.UniForm.ApplicationMethods.Delete_Object )
        {
          return true;
        }

        // 
        // Initialise the methods variables and objects.
        // 
        bool bResult = false;
        //
        // Determine if the user has access to this page and log and error if they do not.
        //
        if ( this.Session.UserProfile.hasRecordAccess == false )
        {
          this.LogIllegalAccess (
            this.ClassNameSpace + "addBinaryData",
            this.Session.UserProfile );

          this.ErrorMessage = EvLabels.Illegal_Page_Access_Attempt;

          return false;
        }


        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "addBinaryData",
          this.Session.UserProfile );

        // 
        // Load the mime types.
        // 
        this.loadMimeTypes ( );

        // 
        // Iterate through the parameter values updating the ResultData object
        // 
        foreach ( Evado.Model.UniForm.Parameter parameter in PageCommand.Parameters )
        {
          if ( parameter.Name.Contains ( EuBinaryFiles.CONST_BINARY_FILE ) == true )
          {
            this.LogValue ( "FieldId: '" + parameter.Name + "'"
              + ", Filename: '" + parameter.Value + "'" );

            if ( parameter.Value != String.Empty )
            {
              // 
              // Update the object.
              // 
              bResult = this.addFileData (
                parameter,
                GroupGuid,
                GroupId,
                SubGroupGuid,
                SubGroupId,
                FileGuid,
                FileId );

              if ( bResult == false )
              {
                this.LogValue ( "FieldId: '" + parameter.Name + "'"
                  + " was not saved to the repository." );
                return false;
              }
            }//END filed name exists.

          }//END parmeter points refers to a binary file.

        }//END groupCommand parameter iteration loop

        this.LogMethodEnd ( "addBinaryData" );

        return true;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvLabels.Binary_Files_Save_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      this.LogMethodEnd ( "addBinaryData" );

      return false;

    }//END addBinaryData method

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="Parameter">List of field values to be updated.</param>
    /// <param name="GroupGuid">Guid: the Group unique object Guid.</param>
    /// <param name="GroupId">String: the Group identifier.</param>    
    /// <param name="SubGroupGuid">Guid the sub group unique object Guid.</param>
    /// <param name="SubGroupId">String: the sub group identifier.</param>  
    /// <param name="FileGuid">Guid the file unique object Guid.</param>
    /// <param name="FileId">String: the file identifier.</param>      
    /// <returns></returns>
    //  ----------------------------------------------------------------------------------
    private bool addFileData (
      Evado.Model.UniForm.Parameter Parameter,
      Guid GroupGuid,
      String GroupId,
      Guid SubGroupGuid,
      String SubGroupId,
      Guid FileGuid,
      String FileId )
    {
      this.LogMethod ( "addFileData" );
      this.LogDebug ( "GroupGuid: " + GroupGuid );
      this.LogDebug ( "GroupId: " + GroupId );
      this.LogDebug ( "SubGroupGuid: " + SubGroupGuid );
      this.LogDebug ( "SubGroupId: " + SubGroupId );
      this.LogDebug ( "FileGuid: " + FileGuid );
      this.LogDebug ( "FileId: " + FileId );

      //
      // Initialise the method variables and objects.
      //
      String stTitle = Parameter.Value;
      String stFileName = Parameter.Value.ToLower ( );
      String stMime = this.getMimeType ( stFileName );
      String stSourcePath = this.UniForm_BinaryFilePath + Parameter.Value;

      //
      // Check that the binary source file exists.
      //
      if ( System.IO.File.Exists ( stSourcePath ) == false )
      {
        this.LogValue ( "Source file does not exist" );

        this.LogMethodEnd ( "addFileData" );
        return false;
      }

      this.LogValue ( "Create BinaryFile object." );
      //
      // Initialise the metaData object
      // 
      EvBinaryFileMetaData binaryFile = new EvBinaryFileMetaData (
        FileGuid,
        this.Session.Application.Guid,
        GroupGuid,
        SubGroupGuid,
        this.Session.Application.ApplicationId,
        GroupId,
        SubGroupId,
        FileId,
        stTitle,
        stFileName,
        stMime );

      //
      // If a new file generate the GUID to define the filename.
      //
      if ( binaryFile.Guid == Guid.Empty )
      {
        binaryFile.Guid = Guid.NewGuid ( );
      }


      binaryFile.RepositoryFilePath = this._FileRepositoryPath;

      this.LogDebug ( "binaryFile.Guid: " + binaryFile.Guid );
      this.LogDebug ( "binaryFile.GroupId: " + binaryFile.GroupId );
      this.LogDebug ( "binaryFile.GroupGuid: " + binaryFile.GroupGuid );
      this.LogDebug ( "binaryFile.SubGroupId: " + binaryFile.SubGroupId );
      this.LogDebug ( "binaryFile.SubGroupGuid: " + binaryFile.SubGroupGuid );
      this.LogDebug ( "binaryFile.FileGuid: " + binaryFile.FileGuid );
      this.LogDebug ( "binaryFile.FileId: " + binaryFile.FileId );

      this.LogDebug ( "Binary file:  Title:" + binaryFile.Title );
      this.LogDebug ( "MimeType:" + binaryFile.MimeType );
      this.LogDebug ( "BinaryFileName: '" + binaryFile.FileName + "'" );
      this.LogDebug ( "FilePath: '" + binaryFile.FullBinaryFilePath + "'" );
      this.LogDebug ( "UniFormPath: '" + stSourcePath + "'" );

      //
      // Copy the binary ResultData file into the UniFORM binary directory for user acsess.
      //
      //
      // Create the binary files.
      //
      binaryFile.createBinaryDirectories ( );

      //
      // Save the file to the directory repository.
      //
      System.IO.File.Copy ( stSourcePath, binaryFile.FullBinaryFilePath, true );

      // 
      // Initialise the update variables.
      //
      if ( this._BinaryFile.UpdatedByDate == EvStatics.CONST_DATE_NULL )
      {
        this._BinaryFile.UpdatedByDate = DateTime.Now;
      }
      binaryFile.UpdatedByUserId = this.Session.UserProfile.UserId;
      binaryFile.UpdatedBy = this.Session.UserProfile.CommonName;

      // 
      // Save the binary file metadata to the database.
      // 
      this._Bll_BinaryFiles.SaveItem ( binaryFile, EvBinaryFileMetaData.ActionsCodes.Add );

      // 
      // get the debug ResultData.
      // 
      this.LogValue ( this._Bll_BinaryFiles.Log );

      this.LogMethodEnd ( "addFileData" );

      return true;

    }//END saveFileData method.     

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="Parameter">List of field values to be updated.</param>
    /// <param name="GroupGuid">Guid: the Group unique object Guid.</param>
    /// <param name="GroupId">String: the Group identifier.</param>    
    /// <param name="SubGroupGuid">Guid the sub group unique object Guid.</param>
    /// <param name="SubGroupId">String: the sub group identifier.</param>  
    /// <param name="FileGuid">Guid the file unique object Guid.</param>
    /// <param name="FileId">String: the file identifier.</param>      
    /// <returns></returns>
    //  ----------------------------------------------------------------------------------
    private EvEventCodes addBinaryFileData (
      EvBinaryFileMetaData binaryFile )
    {
      this.LogMethod ( "addBinaryFileData" );
      this.LogDebug ( "binaryFile.Guid: " + binaryFile.Guid );
      this.LogDebug ( "binaryFile.GroupGuid: " + binaryFile.GroupGuid );
      this.LogDebug ( "binaryFile.GroupId: " + binaryFile.GroupId );
      this.LogDebug ( "binaryFile.SubGroupGuid: " + binaryFile.SubGroupGuid );
      this.LogDebug ( "binaryFile.SubGroupId: " + binaryFile.SubGroupId );
      this.LogDebug ( "binaryFile.FileGuid: " + binaryFile.FileGuid );
      this.LogDebug ( "binaryFile.FileId: " + binaryFile.FileId );

      this.LogDebug ( "binaryFile.Title:" + binaryFile.Title );
      this.LogDebug ( "binaryFile.MimeType:" + binaryFile.MimeType );
      this.LogDebug ( "binaryFile.Version:" + binaryFile.Version );
      this.LogDebug ( "binaryFile.Comments:" + binaryFile.Comments );
      //
      // Initialise the method variables and objects.
      //
      String stSourcePath = this.UniForm_BinaryFilePath + binaryFile.FileName;
      this.LogDebug ( "stSourcePath:" + stSourcePath );
      //
      // Check that the binary source file exists.
      //
      if ( System.IO.File.Exists ( stSourcePath ) == false )
      {
        this.LogValue ( "Source file does not exist" );

        this.LogMethodEnd ( "addBinaryFileData" );
        return EvEventCodes.File_Directory_Path_Empty;
      }

      this.LogValue ( "Create BinaryFile object." );
      //
      // Initialise the metaData object
      // 
      binaryFile.MimeType = this.getMimeType ( binaryFile.FileName );
      this.LogDebug ( "MimeType: " + binaryFile.MimeType );


      binaryFile.RepositoryFilePath = this._FileRepositoryPath;

      this.LogDebug ( "FilePath: '" + binaryFile.FullBinaryFilePath + "'" );
      this.LogDebug ( "UniFormPath: '" + stSourcePath + "'" );

      //
      // Copy the binary ResultData file into the UniFORM binary directory for user acsess.
      //
      //
      // Create the binary files.
      //
      binaryFile.createBinaryDirectories ( );

      //
      // Save the file to the directory repository.
      //
      System.IO.File.Copy ( stSourcePath, binaryFile.FullBinaryFilePath, true );

      // 
      // Initialise the update variables.
      // 
      binaryFile.UpdatedByUserId = this.Session.UserProfile.UserId;
      binaryFile.UpdatedBy = this.Session.UserProfile.CommonName;

      this.LogDebug ( "binaryFile.FileName: '" + binaryFile.FileName + "'" );
      // 
      // Save the binary file metadata to the database.
      // 
      EvEventCodes result = this._Bll_BinaryFiles.SaveItem ( binaryFile, EvBinaryFileMetaData.ActionsCodes.Add );

      this.LogValue ( this._Bll_BinaryFiles.Log );
      if ( result != EvEventCodes.Ok )
      {
        this.LogMethodEnd ( "addBinaryFileData" );
        return EvEventCodes.Database_Update_Error;
      }
      this.LogMethodEnd ( "addBinaryFileData" );

      return EvEventCodes.Ok;

    }//END saveFileData method.

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="Parameter">List of field values to be updated.</param>
    /// <param name="GroupGuid">Guid: the Group unique object Guid.</param>
    /// <param name="GroupId">String: the Group identifier.</param>    
    /// <param name="SubGroupGuid">Guid the sub group unique object Guid.</param>
    /// <param name="SubGroupId">String: the sub group identifier.</param>  
    /// <param name="FileGuid">Guid the file unique object Guid.</param>
    /// <param name="FileId">String: the file identifier.</param>      
    /// <returns></returns>
    //  ----------------------------------------------------------------------------------
    private bool updateFileMetaData (
      EvBinaryFileMetaData binaryFile,
      bool ReleaseFile )
    {
      this.LogMethod ( "updateFileMetaData" );
      this.LogDebug ( "binaryFile.GroupId: " + binaryFile.GroupId );
      this.LogDebug ( "binaryFile.SubGroupId: " + binaryFile.SubGroupId );
      this.LogDebug ( "binaryFile.FileId: " + binaryFile.FileId );
      this.LogDebug ( "binaryFile.FileName: '" + binaryFile.FileName + "'" );
      this.LogDebug ( "binaryFile.Title:" + binaryFile.Title );
      this.LogDebug ( "binaryFile.MimeType:" + binaryFile.MimeType );
      this.LogDebug ( "binaryFile.Version:" + binaryFile.Version );
      this.LogDebug ( "binaryFile.Comments:" + binaryFile.Comments );
      this.LogDebug ( "ReleaseFile: " + ReleaseFile );
      // 
      // Initialise the update variables.
      // 

      binaryFile.UpdatedByUserId = this.Session.UserProfile.UserId;
      binaryFile.UpdatedBy = this.Session.UserProfile.CommonName;

      // 
      // update the binary file metadata to the database.
      // 
      if ( ReleaseFile == true )
      {
        this._Bll_BinaryFiles.SaveItem ( binaryFile, EvBinaryFileMetaData.ActionsCodes.Release );
      }
      else
      {
        this._Bll_BinaryFiles.SaveItem ( binaryFile, EvBinaryFileMetaData.ActionsCodes.Update );
      }
      // 
      // get the debug ResultData.
      // 
      this.LogValue ( this._Bll_BinaryFiles.Log );

      this.LogMethodEnd ( "updateFileMetaData" );

      return true;

    }//END updateFileMetaData method.

    // ==================================================================================
    /// <summary>
    /// This method generates the mime list to resolve the mime type from the extension.
    /// </summary>
    //  ----------------------------------------------------------------------------------
    public void loadMimeTypes ( )
    {
      this.LogMethod ( "loadMimeTypes" );

      // 
      // Create the mime list 
      // 
      try
      {
        this._MimeList = Evado.Model.Digital.EvcStatics.loadMimeTypes ( this._FileRepositoryPath, "mimetypes.csv" );

        this.LogValue ( "Mime List count: " + this._MimeList.Count );
      }
      catch ( Exception Ex )
      {
        this.LogException ( Ex );
      }


    }//END Method


    // ==================================================================================
    /// <summary>
    /// This method generates the mime list to resolve the mime type from the extension.
    /// </summary>
    /// <param name="Filename">String:  File name.</param>
    //  ----------------------------------------------------------------------------------
    public string getMimeType ( String Filename )
    {
      this.LogMethod ( "getMimeType" );
      this.LogValue ( "Mime list count: " + this._MimeList.Count );
      //
      // Initialise the methods variables and objects.
      //
      String stMimeType = "text/plain";
      int iExtention = Filename.LastIndexOf ( '.' );
      string stExtenstion = Filename.Substring ( iExtention );

      this.LogValue ( "Extension: " + stExtenstion );

      //
      // Iterate through the mime list looking for a matching extension and 
      // then return the relevant mime type.
      // 
      foreach ( EvOption option in this._MimeList )
      {
        if ( option.Value == stExtenstion )
        {
          return option.Description;
        }
      }//END interation loop

      //
      // If non is found assume the mime type is text.
      //
      return stMimeType;
    }

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

  }
}//END namespace