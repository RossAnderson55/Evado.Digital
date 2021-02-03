/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\UserProfiles.cs" company="EVADO HOLDING PTY. LTD.">
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
using  Evado.Model.Digital;
// using Evado.Web;

namespace Evado.UniForm.Digital
{
  /// <summary>
  /// This class profile UniFORM class adapter for the user profile classes
  /// </summary>
  public partial class EuUserProfiles : EuClassAdapterBase
  {
    #region Class Initialisation
    /// <summary>
    /// This method initialises the class.
    /// </summary>
    public EuUserProfiles ( )
    {
      this.ClassNameSpace = "Evado.UniForm.Clinical.EuUserProfiles.";
    }

    /// <summary>
    /// This method initialises the class and passs in the user profile.
    /// </summary>
    public EuUserProfiles (
      EuAdapterObjects AdapterObjects,
      EvUserProfileBase ServiceUserProfile,
      EuSession SessionObjects,
      String UniFormBinaryFilePath,
      EvClassParameters Settings)
    {
      this.GlobalObjects = AdapterObjects;
      this.ServiceUserProfile = ServiceUserProfile;
      this.Session = SessionObjects;
      this.UniForm_BinaryFilePath = UniFormBinaryFilePath;
      this.ClassParameters = Settings;

      this.ClassNameSpace = "Evado.UniForm.Clinical.EuUserProfiles.";
      this.LogInitMethod ( "UserProfiles initialisation" );
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

      if ( this.Session.SelectedUserType == EdUserProfile.UserTypesList.Null )
      {
        this.Session.SelectedUserType = EdUserProfile.UserTypesList.End_User;
      }

      this._Bll_UserProfiles = new Evado.Bll.Clinical.EvUserProfiles ( this.ClassParameters );

    }//END Method


    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class constants and variables.

    private const String CONST_ADDRESS_FIELD_ID = "ADDRESS";
    private const String CONST_CURRENT_FIELD_ID = "CURRENT";
    private const String CONST_NEW_PASSWORD_PARAMETER = "NPWD";
    private const String CONST_DELETE_ACTION = "DELETE";
    private const String CONST_DOWNLOAD_EXTENSION = "user-profiles.csv";

    private Evado.Bll.Clinical.EvUserProfiles _Bll_UserProfiles = new Evado.Bll.Clinical.EvUserProfiles ( );

    private EvPageIds PageId = EvPageIds.Null;

    private System.Text.StringBuilder _ProcessLog = null;

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class public methods

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
      this.LogValue ( "Parameter PageCommand " + PageCommand.getAsString ( false, false ) );

      try
      {
        // 
        // Initialise the methods variables and objects.
        // 
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );

        //
        // Load the customer group.
        //
        this.getAdsCustomerGroup ( );

        //
        // Retrieve the groupCommand parameters to determine the type of page to generate.
        //
        String stPageType = PageCommand.GetParameter ( Evado.Model.UniForm.CommandParameters.Page_Id );

        if ( stPageType != String.Empty )
        {
          this.PageId = Evado.Model.EvStatics.Enumerations.parseEnumValue<EvPageIds> ( stPageType );
        }
        this.LogValue ( "SessionObjects.PageType: " + this.PageId );

        //
        // Skip if the updating the current user properties.
        //
        if ( this.PageId != EvPageIds.User_Profile_Update_Page )
        {
          if ( this.Session.AdminUserProfile == null )
          {
            this.Session.AdminUserProfile = new Evado.Model.Digital.EdUserProfile ( );
          }

          if ( this.Session.AdminUserProfileList == null )
          {
            this.Session.AdminUserProfileList = new List<EdUserProfile> ( );
          }
          this.LogValue ( "AdminUserProfile.Guid: " + this.Session.AdminUserProfile.Guid );
        }

        // 
        // Determine the method to be called
        // 
        switch ( PageCommand.Method )
        {
          case Evado.Model.UniForm.ApplicationMethods.List_of_Objects:
            {

              switch ( this.PageId )
              {
                case EvPageIds.User_Profile_Update_Page:
                  {
                    clientDataObject = this.Session.LastPage; 
                    break;
                  }
                default:
                  {
                    clientDataObject = this.getListObject ( PageCommand );
                    break;
                  }
              }

              break;
            }
          case Evado.Model.UniForm.ApplicationMethods.Get_Object:
            {
              this.LogValue ( "Case: ApplicationMethods.Get_Object. " );

              switch ( this.PageId )
              {
                case EvPageIds.User_Profile_Update_Page:
                  {
                    clientDataObject = this.getObject_UserProfile ( PageCommand );
                    break;
                  }
                default:
                  {
                    clientDataObject = this.getObject ( PageCommand );
                    break;
                  }
              }
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


              switch ( this.PageId )
              {
                case EvPageIds.User_Profile_Update_Page:
                  {
                    clientDataObject = this.updateUserObject ( PageCommand );
                    break;
                  }
                default:
                  {
                    // 
                    // Update the object values
                    // 
                    clientDataObject = this.updateObject ( PageCommand );
                    break;
                  }
              }

              break;
            }

        }//END Swith

        // 
        // Handle returned exceptions.
        // 
        if ( clientDataObject == null )
        {
          clientDataObject = this.Session.LastPage;
          this.LogDebug ( " null application data returned." );
        }

        if (this.ErrorMessage != String.Empty  )
        {
          this.LogDebug ( "Append error message" );
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

    }//END updateObject method

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
    private Evado.Model.UniForm.AppData getListObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getListObject" );
      this.LogValue ( "PageCommand: " + PageCommand.getAsString ( false, true ) );

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.AppData clientDataObject = new Model.UniForm.AppData ( );
      EdUserProfile.UserTypesList userType = EdUserProfile.UserTypesList.Null;

      //
      // Determine if the user has access to this page and log and error if they do not.
      //
      if ( this.Session.UserProfile.hasAdministrationAccess == false )
      {
        this.LogIllegalAccess (
          this.ClassNameSpace + "getListObject",
          this.Session.UserProfile );

        this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

         return this.Session.LastPage;;
      }

      // 
      // Log access to page.
      // 
      this.LogPageAccess (
          this.ClassNameSpace + "getListObject",
        this.Session.UserProfile );

      this.LogValue ( "AdminOrganisation.OrgId: " + userType );

      // 
      // set the application ResultData object properties.
      // 
      clientDataObject.Id = Guid.NewGuid ( );
      clientDataObject.Page.Id = clientDataObject.Id;
      clientDataObject.Page.PageId = EvPageIds.User_View.ToString ( );
      clientDataObject.Page.PageDataGuid = clientDataObject.Id;
      clientDataObject.Title = EdLabels.User_Profile_Selection_Page_Title;
      clientDataObject.Page.Title = clientDataObject.Title;
      clientDataObject.Page.PageId = EvPageIds.Subject_View.ToString ( );
      clientDataObject.Page.PageDataGuid = clientDataObject.Page.Id;

      // 
      // get the current user type selection identifier.
      // 
      if ( PageCommand.hasParameter (
          EdUserProfile.UserProfileFieldNames.User_Type_Id.ToString() ) == true )
      {
        userType = PageCommand.GetParameter<EdUserProfile.UserTypesList> ( 
          EdUserProfile.UserProfileFieldNames.User_Type_Id );

        this.LogDebug ( "Parameter set User Type: " + userType );

      }//END user type parameter exists.

      //
      // set the page commands.
      //
      this.getListPage_Commands ( clientDataObject.Page );

      //
      // display the user upload group.
      //
      if ( this.PageId == EvPageIds.User_Upload_Page )
      {
        this.LogValue ( "Processing upload page." );
        this.getList_Upload_Group ( PageCommand, clientDataObject.Page );
      }

      //
      // Display the user download group
      //
      if ( this.PageId == EvPageIds.User_DownLoad_Page )
      {
        this.LogValue ( "Processing download page." );
        this.getList_Download_Group ( clientDataObject.Page );
      }

      // 
      // Add the organisation list list field.
      // 
      this.getUserTypeSelection ( clientDataObject.Page );

      //
      // Create the userprofile list group.
      //
      this.getList_UserProfiles_Group ( clientDataObject.Page );

      this.LogValue ( " data.Title: " + clientDataObject.Title );
      this.LogValue ( " data.Page.Title: " + clientDataObject.Page.Title );

      this.LogMethodEnd ( "getListObject" );
      return clientDataObject;

    }//END getListObject method.

    // ==================================================================================
    /// <summary>
    /// This methods returns a pageMenuGroup object contains a selection of organisations.
    /// </summary>
    /// <param name="PageObject">Application</param>
    /// <returns>Evado.Model.UniForm.Group object</returns>
    //  ---------------------------------------------------------------------------------
    public void getListPage_Commands (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getListPage_Commands" );
      //
      // initialise the methods variables and objects.
      //
      Evado.Model.UniForm.Command pageCommand = new Model.UniForm.Command ( );

      /*
      if ( this._ApplicationObjects.HelpUrl != String.Empty )
      {
        pageCommand = PageObject.addCommand (
           EdLabels.Label_Help_Command_Title,
           EuAdapter.APPLICATION_ID,
           EuAdapter.ApplicationObjects.Users.ToString ( ),
           Model.UniForm.ApplicationMethods.Get_Object );

        pageCommand.Type = Evado.Model.UniForm.CommandTypes.Html_Link;

        pageCommand.AddParameter ( Model.UniForm.CommandParameters.Link_Url,
         EvStatics.createHelpUrl (
          this._ApplicationObjects.HelpUrl,
           Evado.Model.Digital.EvPageIds.User_View ) );
      }
      */

      //
      // Display the download user profiles page command
      //
      if ( this.PageId != EvPageIds.User_DownLoad_Page )
      {
        pageCommand = PageObject.addCommand (
           EdLabels.UserProfile_Downoad_Command_Title,
           EuAdapter.ADAPTER_ID,
           EuAdapterClasses.Users.ToString ( ),
           Model.UniForm.ApplicationMethods.Custom_Method );

        pageCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.List_of_Objects );
        pageCommand.SetPageId ( EvPageIds.User_DownLoad_Page );
      }

      //
      // display the upload user profiles page command.
      //
      if ( this.PageId != EvPageIds.User_Upload_Page )
      {
        pageCommand = PageObject.addCommand (
           EdLabels.UserProfile_Upload_Command_Title,
           EuAdapter.ADAPTER_ID,
           EuAdapterClasses.Users.ToString ( ),
           Model.UniForm.ApplicationMethods.Custom_Method );

        pageCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.List_of_Objects );
        pageCommand.SetPageId ( EvPageIds.User_Upload_Page );
      }

      this.LogMethodEnd ( "getListPage_Commands" );

    }//END getListPage_Commands method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void getList_Upload_Group (
      Evado.Model.UniForm.Command PageCommand,
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getList_Upload_Group" );

      try
      {
        //
        // if the form template filename is empty display the selection field.
        //
        if ( PageCommand.hasParameter ( EuRecordLayouts.CONST_TEMPLATE_FIELD_ID ) == false )
        {
          this.LogValue ( "UploadFileName is empty" );

          this.getUserUploadSelectionGroup ( PageObject );
        }
        else
        {
          this.LogValue ( "Saving upload" );

          this.saveUserProfileUpload ( PageCommand, PageObject );
        }

        this.LogMethodEnd ( "getList_Upload_Group" );
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

      this.LogMethodEnd ( "getList_Upload_Group" );

    }//END getList_Upload_Group method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.AppData object.</param>
    //  ------------------------------------------------------------------------------
    private void getUserUploadSelectionGroup (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getUserProfileUploadDataObject" );

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
      pageGroup = PageObject.AddGroup (
        EdLabels.UserProfile_Upload_Group_Title,
        Evado.Model.UniForm.EditAccess.Enabled );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;

      pageGroup.SetCommandBackBroundColor (
        Model.UniForm.GroupParameterList.BG_Mandatory,
        Model.UniForm.Background_Colours.Red );

      groupField = pageGroup.createBinaryFileField (
        EuRecordLayouts.CONST_TEMPLATE_FIELD_ID,
        EdLabels.UserProfile_Upload_Field_Title,
        String.Empty,
        this.Session.UploadFileName );
      groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      groupField.AddParameter ( Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, "Yes" );

      groupCommand = pageGroup.addCommand (
        EdLabels.UserProfile_Upload_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Users.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Custom_Method );

      groupCommand.SetPageId ( EvPageIds.User_Upload_Page );
      groupCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.List_of_Objects );

    }//END getUserProfileUploadDataObject Method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.AppData object.</param>
    //  ------------------------------------------------------------------------------
    private void saveUserProfileUpload (
      Evado.Model.UniForm.Command PageCommand,
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "saveUserProfileUpload" );
      // 
      // Initialise the client ResultData object.
      // 
      EvEventCodes result = EvEventCodes.Ok;
      string fileName = PageCommand.GetParameter ( EuRecordLayouts.CONST_TEMPLATE_FIELD_ID );
      this.LogValue ( "fileName: " + fileName );

      String uploadFileName = this.UniForm_BinaryFilePath +
        fileName;
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );

      //
      // Define the general properties pageMenuGroup..
      //
      pageGroup = PageObject.AddGroup (
        String.Empty,
        Evado.Model.UniForm.EditAccess.Inherited );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;

      this.LogValue ( "uploadFileName: " + uploadFileName );

      //
      //Upload the User profile.
      //
      result = this.uploadUserProfiles ( fileName );

      if ( result != EvEventCodes.Ok )
      {
        this.ErrorMessage = "ERROR: " + EvStatics.enumValueToString ( result );
      }

      this.LogValue ( "processLog: " + this._ProcessLog.ToString ( ) );

      pageGroup.Description = this._ProcessLog.ToString ( ) ;
      //
      // reset the form template filename.
      //
      this.Session.UploadFileName = String.Empty;

    }//END getUserProfileUploadDataObject Method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    //  ------------------------------------------------------------------------------
    private void getList_Download_Group (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getList_Download_Group" );
      this.LogValue ( "UniForm_BinaryFilePath: " + this.UniForm_BinaryFilePath );
      this.LogValue ( "UniForm_BinaryServiceUrl: " + this.UniForm_BinaryServiceUrl );

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      System.Text.StringBuilder outputFile = new StringBuilder ( );
      Evado.Model.UniForm.Parameter parameter = new Evado.Model.UniForm.Parameter ( );
      String downloadFileName = String.Empty;
      String downloadUrl = String.Empty;

      //
      // Define the general properties pageMenuGroup..
      //
      pageGroup = PageObject.AddGroup (
        String.Empty,
        Evado.Model.UniForm.EditAccess.Inherited );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;

      List<EdUserProfile> userProfileList = this._Bll_UserProfiles.GetView ( 
        this.Session.SelectedUserType );

      //
      // if the user profile list exists display the download link.
      //
      if ( userProfileList.Count > 0 )
      {
        String csvText = "\"" + EdUserProfile.UserProfileFieldNames.UserId + "\","
          + "\"" + EdUserProfile.UserProfileFieldNames.Password + "\","
          + "\"" + EdUserProfile.UserProfileFieldNames.User_Type_Id + "\","
          + "\"" + EdUserProfile.UserProfileFieldNames.Prefix + "\","
          + "\"" + EdUserProfile.UserProfileFieldNames.Given_Name + "\","
          + "\"" + EdUserProfile.UserProfileFieldNames.Family_Name + "\","
          + "\"" + EdUserProfile.UserProfileFieldNames.CommonName + "\","
          + "\"" + EdUserProfile.UserProfileFieldNames.Title + "\","
          + "\"" + EdUserProfile.UserProfileFieldNames.Email_Address + "\","
          + "\"" + EdUserProfile.UserProfileFieldNames.RoleId + "\"";

        outputFile.AppendLine ( csvText );

        //
        // iterate through the users in the list.
        //
        foreach ( EdUserProfile user in userProfileList )
        {
          csvText = "\"" + user.UserId + "\","
            + "\"" + user.Password + "\","
            + "\"" + user.TypeId + "\","
            + "\"" + user.Prefix + "\","
            + "\"" + user.GivenName + "\","
            + "\"" + user.FamilyName + "\","
            + "\"" + user.CommonName + "\","
            + "\"" + user.Title + "\","
            + "\"" + user.EmailAddress + "\","
            + "\"" + user.Roles + "\"";

          outputFile.AppendLine ( csvText );
        }

        //
        // Define the form template filename.
        //
        if ( this.Session.SelectedUserType !=  EdUserProfile.UserTypesList.Null)
        {
          downloadFileName = this.Session.SelectedUserType + "-"
           + EuUserProfiles.CONST_DOWNLOAD_EXTENSION;
        }
        else
        {
          downloadFileName = EuUserProfiles.CONST_DOWNLOAD_EXTENSION;
        }

        downloadFileName =
          downloadFileName.Replace ( " ", "-" );

        this.LogValue ( "downloadFileName: " + downloadFileName );

        downloadUrl = this.UniForm_BinaryServiceUrl +
          downloadFileName;

        this.LogValue ( "downloadUrl: " + downloadUrl );

        //
        // Save the form layout to the UniFORM binary repository.
        //
        Evado.Model.EvStatics.Files.saveFile (
          this.UniForm_BinaryFilePath,
          downloadFileName,
          outputFile.ToString ( ) );


        groupField = pageGroup.createHtmlLinkField (
          String.Empty,
          downloadFileName,
          downloadUrl );
      }
      else
      {
        pageGroup.Description = EdLabels.UserProfile_Download_Empty_List_Message ;
      }
      this.LogMethodEnd ( "getList_Download_Group" );

    }//END getList_Download_Group method

    // ==================================================================================
    /// <summary>
    /// This methods returns a pageMenuGroup object contains a selection of organisations.
    /// </summary>
    /// <param name="PageObject">Application</param>
    /// <returns>Evado.Model.UniForm.Group object</returns>
    //  ---------------------------------------------------------------------------------
    public void getUserTypeSelection (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getUserTypeSelection" );

      // 
      // initialise the methods variables and objects.
      // 
      List<Evado.Model.EvOption> usertypeList = new List<Evado.Model.EvOption> ( );
      Evado.Model.UniForm.Field organisationSelectionField = new Evado.Model.UniForm.Field ( );

      Evado.Model.UniForm.Group selectionGroup = PageObject.AddGroup (
        EdLabels.UserProfile_List_Selection_Group,
        Evado.Model.UniForm.EditAccess.Enabled );
      selectionGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      //
      // get the list of organisations.
      //
      usertypeList.Add ( new EvOption ( ) );
      usertypeList.Add ( new EvOption ( EdUserProfile.UserTypesList.Customer.ToString ( ), EdUserProfile.UserTypesList.Customer.ToString ( ) ) );
      usertypeList.Add ( new EvOption ( EdUserProfile.UserTypesList.Evado.ToString ( ), EdUserProfile.UserTypesList.Evado.ToString ( ) ) );
      usertypeList.Add ( new EvOption ( EdUserProfile.UserTypesList.End_User.ToString ( ), EdUserProfile.UserTypesList.End_User.ToString ( ).Replace( "_"," ") ) );

      // 
      // Set the selection to the current site org id.
      // 
      organisationSelectionField = selectionGroup.createSelectionListField (
        EdUserProfile.UserProfileFieldNames.User_Type_Id,
        EdLabels.User_Profile_Organisation_List_Field_Label,
        this.Session.SelectedUserType.ToString(),
        usertypeList );
      organisationSelectionField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      organisationSelectionField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );

      // 
      // Create a custom groupCommand to process the selection.
      // 
      Evado.Model.UniForm.Command customCommand = selectionGroup.addCommand (
        EdLabels.User_Profile_Organisation_Selection_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Users.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Custom_Method );

      // 
      // Set the custom groupCommand parameter.
      // 
      customCommand.setCustomMethod ( Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

      this.LogMethodEnd ( "getUserTypeSelection" );

    }//END getOrganisationSelection method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private void getList_UserProfiles_Group (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getList_UserProfiles_Group" );
      try
      {
        //  
        // Create the list pageMenuGroup.
        // 
        Evado.Model.UniForm.Group PageGroup = PageObject.AddGroup (
          EdLabels.User_Profile_List_Group_Title,
          String.Empty,
          Evado.Model.UniForm.EditAccess.Enabled );
        PageGroup.CmdLayout = Evado.Model.UniForm.GroupCommandListLayouts.Vertical_Orientation;
        PageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

        // 
        // Add the save groupCommand
        // 
        Evado.Model.UniForm.Command newCommand = PageGroup.addCommand (
          EdLabels.User_Profile_New_User_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Users.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Create_Object );

        newCommand.AddParameter (
          EdUserProfile.UserProfileFieldNames.User_Type_Id.ToString(),
          this.Session.SelectedUserType.ToString() );

        newCommand.SetBackgroundColour (
          Model.UniForm.CommandParameters.BG_Default,
          Model.UniForm.Background_Colours.Purple );

        // 
        // get the list of customers.
        // 
        this.Session.AdminUserProfileList = this._Bll_UserProfiles.GetView (this.Session.SelectedUserType );

        this.LogClass ( this._Bll_UserProfiles.Log );

        this.LogValue ( " list count: " + this.Session.AdminUserProfileList.Count );

        // 
        // generate the page links.
        // 
        foreach (  Evado.Model.Digital.EdUserProfile userProfile in this.Session.AdminUserProfileList )
        {
          Evado.Model.UniForm.Command command = PageGroup.addCommand (
            userProfile.LinkText,
            EuAdapter.ADAPTER_ID,
            EuAdapterClasses.Users.ToString ( ),
            Evado.Model.UniForm.ApplicationMethods.Get_Object );

          command.SetGuid ( userProfile.Guid );

          command.setShortTitleParameter ( userProfile.CommonName );
        }
        this.LogMethodEnd ( "getList_UserProfiles_Group" );
      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.User_Profile_List_Error_Message;

        // 
        // Generate the log the error event.
        //
        this.LogException ( Ex );
      }

      this.LogMethodEnd ( "getList_UserProfiles_Group" );

    }//END getList_UserProfiles_Group method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class form template upload methods

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class get user profile methods
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
      Guid subjectGuid = Guid.Empty;
      Evado.Model.UniForm.AppData clientDataObject = new Model.UniForm.AppData ( );

      //
      // Determine if the user has access to this page and log and error if they do not.
      //
      if ( this.Session.UserProfile.hasAdministrationAccess == false )
      {
        this.LogIllegalAccess (
          this.ClassNameSpace + "getObject",
          this.Session.UserProfile );

        this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

         return this.Session.LastPage;;
      }

      // 
      // Log access to page.
      // 
      this.LogPageAccess (
          this.ClassNameSpace + "getObject",
        this.Session.UserProfile );

      //
      // Initialise the client ResultData object.
      //
      clientDataObject.Id = Guid.NewGuid ( );
      clientDataObject.Page.Id = clientDataObject.Id;
      clientDataObject.Page.PageDataGuid = clientDataObject.Id;

      try
      {
        Guid userGuid = PageCommand.GetGuid ( );

        if ( userGuid == Guid.Empty )
        {
          this.LogValue ( "userGuid is empty." );
           return this.Session.LastPage;;
        }

        this.LogValue ( "userGuid: " + userGuid );

        // 
        // Retrieve the customer object from the database via the DAL and BLL layers.
        // 
        this.Session.AdminUserProfile = this._Bll_UserProfiles.getItem ( userGuid );

        if ( this.Session.AdminUserProfile.Guid == Guid.Empty )
        {
          this.LogValue ( "this.SessionObjects.AdminUserProfile is empty." );

           return this.Session.LastPage;;
        }

        this.LogClass ( this._Bll_UserProfiles.Log );

        this.LogDebug ( "AdminUserProfile.Guid {0}. ", this.Session.AdminUserProfile.Guid );
        this.LogDebug ( "AdminUserProfile.UserId {0}.", this.Session.AdminUserProfile.UserId );

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
        this.ErrorMessage = EdLabels.User_Profile_Page_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

       return this.Session.LastPage;;

    }//END getObject method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="ClientDataObject">Evado.Model.UniForm.AppData object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject (
      Evado.Model.UniForm.AppData ClientDataObject )
    {
      this.LogMethod ( "getclientDataObject" );
      // 
      // Initialise the methods variables and objects.
      // 

      //
      // set the client ResultData object properties
      //
      ClientDataObject.Id = this.Session.AdminUserProfile.Guid;
      ClientDataObject.Page.Id = this.Session.AdminUserProfile.Guid;
      ClientDataObject.Title = EdLabels.User_Profile_Page_Title
        + this.Session.AdminUserProfile.CommonName;

      ClientDataObject.Page.Title = ClientDataObject.Title;
      ClientDataObject.Page.PageId = EvPageIds.User_Profile_Page.ToString ( );
      ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      this.LogValue ( "clientDataObject status: " + ClientDataObject.Page.EditAccess );
      //
      // Set the user edit access to the objects.
      //
      ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Disabled;

      if ( this.Session.UserProfile.hasAdministrationAccess == true )
      {
        ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
      }
      this.LogValue ( "Page.Status: " + ClientDataObject.Page.EditAccess );

      //
      // Add the page commands to the page.
      //
      this.getDataObject_PageCommands ( ClientDataObject.Page );

      //
      // Add the field pageMenuGroup to the page.
      //
      this.getDataObject_FieldGroup ( ClientDataObject.Page );

    }//END getclientDataObject Method

    // ==============================================================================
    /// <summary>
    /// This method adds the ResultData object page commands.
    /// </summary>
    /// <param name="Page"> Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_PageCommands (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getDataObject_PageCommands" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );

      // 
      // Add the save groupCommand
      // 
      pageCommand = Page.addCommand (
        EdLabels.User_Profile_Save_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Users.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Save_Object );

      // 
      // Define the save and delete groupCommand parameters
      // 
      pageCommand.SetGuid ( Page.Id );


      // 
      // Add the save groupCommand
      // 
      pageCommand = Page.addCommand (
        EdLabels.User_Profile_New_Password_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Users.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Save_Object );

      // 
      // Define the save and delete groupCommand parameters
      // 
      pageCommand.SetGuid ( Page.Id );
      pageCommand.AddParameter ( EuUserProfiles.CONST_NEW_PASSWORD_PARAMETER, "Yes" );

    }//END getclientDataObject_Commands Method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_FieldGroup (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getDataObject_FieldGroup" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Command groupCommand = new Evado.Model.UniForm.Command ( );
      List<EvOption> optionList = new List<EvOption> ( );



      // 
      // create the page pageMenuGroup
      // 
      pageGroup = Page.AddGroup (
         String.Empty,
         Evado.Model.UniForm.EditAccess.Inherited );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
      pageGroup.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      pageGroup.SetCommandBackBroundColor (
        Model.UniForm.GroupParameterList.BG_Mandatory,
        Model.UniForm.Background_Colours.Red );

      //
      // Add the groups commands.
      //
      this.getDataObject_GroupCommands ( pageGroup );

      optionList = EdUserProfile.GetUserTypeOptionList ( true );
      //
      // Add the user's organisation
      //
      groupField = pageGroup.createSelectionListField (
        EdUserProfile.UserProfileFieldNames.User_Type_Id.ToString(),
        EdLabels.UserProfile_User_Type_Field_Label,
        this.Session.SelectedUserType.ToString(),
        optionList );
      groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;
      groupField.Mandatory = true;
      groupField.setBackgroundColor (
        Model.UniForm.FieldParameterList.BG_Mandatory,
        Model.UniForm.Background_Colours.Red );

      // 
      // Create the user id object
      // 
      groupField = pageGroup.createTextField (
         Evado.Model.Digital.EdUserProfile.UserProfileFieldNames.UserId.ToString ( ),
        EdLabels.User_Profile_Identifier_Field_Label,
        this.Session.AdminUserProfile.UserId,
        80 );
      groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;
      groupField.Mandatory = true;
      groupField.setBackgroundColor (
        Model.UniForm.FieldParameterList.BG_Mandatory,
        Model.UniForm.Background_Colours.Red );

      if ( this.Session.AdminUserProfile.Guid !=  Evado.Model.Digital.EvcStatics.CONST_NEW_OBJECT_ID )
      {
        groupField.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
      }

      // 
      // Create the Password object
      // 
      /*
      if ( this.SessionObjects.AdminUserProfile.Guid ==  Evado.Model.Digital.EvStatics.CONST_NEW_OBJECT_ID )
      {
        groupField = pageGroup.createTextField (
           Evado.Model.Digital.EvUserProfile.UserProfileFieldNames.Password,
          EdLabels.User_Profile_Password_Field_Label,
          this.SessionObjects.AdminUserProfile.Password,
          80 );
        groupField.Layout = EuPageGenerator.ApplicationFieldLayout;

        groupField.Description =  EdLabels.User_Profile_Password_Field_Description );
      }
       */

      // 
      // Create the  name object
      // 
      this.LogValue ( "Given Name:" + this.Session.AdminUserProfile.GivenName );
      this.LogValue ( "Family Name:" + this.Session.AdminUserProfile.FamilyName );
      /*
      groupField = pageGroup.createTextField (
         Evado.Model.Digital.EvUserProfile.UserProfileFieldNames.Prefix,
        EdLabels.UserProfile_Prefix_Field_Label,
        this.Session.AdminUserProfile.Prefix, 10 );
      groupField.Layout = EuFormGenerator.ApplicationFieldLayout;
      */

      groupField = pageGroup.createTextField (
         Evado.Model.Digital.EdUserProfile.UserProfileFieldNames.Given_Name,
        EdLabels.UserProfile_GivenName_Field_Label,
        this.Session.AdminUserProfile.GivenName, 50 );
      groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;
      groupField.Mandatory = true;
      groupField.setBackgroundColor (
        Model.UniForm.FieldParameterList.BG_Mandatory,
        Model.UniForm.Background_Colours.Red );

      groupField = pageGroup.createTextField (
         Evado.Model.Digital.EdUserProfile.UserProfileFieldNames.Family_Name,
        EdLabels.UserProfile_FamilyName_Field_Label,
        this.Session.AdminUserProfile.FamilyName, 50 );
      groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;
      groupField.Mandatory = true;
      groupField.setBackgroundColor (
        Model.UniForm.FieldParameterList.BG_Mandatory,
        Model.UniForm.Background_Colours.Red );

      // 
      // Create the comon name object
      // 
      groupField = pageGroup.createTextField (
         Evado.Model.Digital.EdUserProfile.UserProfileFieldNames.CommonName,
        EdLabels.UserProfile_CommonName_Field_Label,
        String.Empty,
        this.Session.AdminUserProfile.CommonName,
        80 );
      groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;
      groupField.Mandatory = true;
      groupField.setBackgroundColor (
        Model.UniForm.FieldParameterList.BG_Mandatory,
        Model.UniForm.Background_Colours.Red );

      //
      // define the user address field.
      //
      if ( this.Session.CollectUserAddress == true )
      {
        this.LogDebug ( "Address_1:" + this.Session.UserProfile.Address_1 );
        this.LogDebug ( "Address_2:" + this.Session.UserProfile.Address_2 );
        this.LogDebug ( "AddressCity:" + this.Session.UserProfile.AddressCity );
        this.LogDebug ( "AddressState:" + this.Session.UserProfile.AddressState );
        this.LogDebug ( "AddressPostCode:" + this.Session.UserProfile.AddressPostCode );
        this.LogDebug ( "AddressCountry:" + this.Session.UserProfile.AddressCountry );
        // 
        // Create the customer name object
        //
        groupField = pageGroup.createAddressField (
          EuUserProfiles.CONST_ADDRESS_FIELD_ID,
          EdLabels.UserProfile_Address_Field_Label,
          this.Session.UserProfile.Address_1,
          this.Session.UserProfile.Address_2,
          this.Session.UserProfile.AddressCity,
          this.Session.UserProfile.AddressState,
          this.Session.UserProfile.AddressPostCode,
          this.Session.UserProfile.AddressCountry );
        groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;

        this.LogDebug ( "AddresS:" + groupField.Value );
      }

      // 
      // Create the user's email address object
      // 
      groupField = pageGroup.createEmailAddressField (
         Evado.Model.Digital.EdUserProfile.UserProfileFieldNames.Email_Address.ToString ( ),
        EdLabels.UserProfile_Email_Field_Label,
        this.Session.AdminUserProfile.EmailAddress );
      groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      // 
      // Create the user role id object
      // 
      this.LogValue ( "AdminUserProfile.RoleId: "
        + this.Session.AdminUserProfile.Roles );

      //
      // Generate the user role list.
      //
      List<EvOption> roleList =  Evado.Model.Digital.EdUserProfile.getRoleOptionList (
        this.GlobalObjects.AdapterSettings.RoleList,
        false );


      //
      // Generate the user role radio button list field object.
      //
      groupField = pageGroup.createRadioButtonListField (
         Evado.Model.Digital.EdUserProfile.UserProfileFieldNames.RoleId.ToString ( ),
        EdLabels.UserProfile_Role_Field_Label,
        EdLabels.UserProfile_Role_Field_Description,
        Evado.Model.EvStatics.getEnumStringValue ( this.Session.AdminUserProfile.Roles ),
        roleList );
      groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;
      groupField.Mandatory = true;
      groupField.setBackgroundColor (
        Model.UniForm.FieldParameterList.BG_Mandatory,
        Model.UniForm.Background_Colours.Red );

      if ( this.Session.UserProfile.hasEvadoAdministrationAccess )
      {

        groupField = pageGroup.createTextField (
           Evado.Model.Digital.EdUserProfile.UserProfileFieldNames.Expiry_Date.ToString ( ),
          EdLabels.UserProfile_Expiry_Date_Field_Label,
          EvStatics.getDateAsString( this.Session.AdminUserProfile.ExpiryDate ),
          15 );
        groupField.Layout = EuRecordGenerator.ApplicationFieldLayout;
      }

    }//END getDataObject_FieldGroup Method
    

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_GroupCommands (
      Evado.Model.UniForm.Group PageGroup )
    {
      this.LogMethod ( "getDataObject_GroupCommands" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Command groupCommand = new Evado.Model.UniForm.Command ( );

      // 
      // Add the save groupCommand
      // 
      groupCommand = PageGroup.addCommand (
        EdLabels.User_Profile_Save_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Users.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Save_Object );

      // 
      // Define the save and delete groupCommand parameters
      // 
      groupCommand.SetGuid (
        this.Session.UserProfile.Guid );

      //
      // Add the delete groupCommand object.
      //
      if ( this.Session.UserProfile.Guid != Guid.Empty )
      {
        groupCommand = PageGroup.addCommand (
           EdLabels.User_Profile_Delete_Command_Title,
           EuAdapter.ADAPTER_ID,
           EuAdapterClasses.Users.ToString ( ),
           Evado.Model.UniForm.ApplicationMethods.Save_Object );

        // 
        // Define the save and delete groupCommand parameters
        // 
        groupCommand.SetGuid (
          this.Session.UserProfile.Guid );
        groupCommand.AddParameter (
           Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
          EuUserProfiles.CONST_DELETE_ACTION );
      }

      this.LogMethodEnd ( "getDataObject_GroupCommands" );
    }//END getDataObject_GroupCommands method.

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Create User methods
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
      this.LogValue ( Evado.Model.UniForm.EuStatics.CONST_METHOD_START
        + this.ClassNameSpace + "createObject" );
      this.LogValue ( "AdminOrganisation.OrgId: " +this.Session.SelectedUserType.ToString() );
      try
      {
        // 
        // Initialise the methods variables and objects.
        //      
        Evado.Model.UniForm.AppData clientDataObject = new Model.UniForm.AppData ( );

        //
        // Determine if the user has access to this page and log and error if they do not.
        //
        if ( this.Session.UserProfile.hasManagementAccess == false )
        {
          this.LogIllegalAccess (
            this.ClassNameSpace + "createObject",
            this.Session.UserProfile );

          this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

           return this.Session.LastPage;;
        }

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "createObject",
          this.Session.UserProfile );

        // 
        // Create the milestone object and add it to the clinical session object.
        // 
        this.Session.AdminUserProfile = new  Evado.Model.Digital.EdUserProfile ( );
        this.Session.AdminUserProfile.Guid =  Evado.Model.Digital.EvcStatics.CONST_NEW_OBJECT_ID;
        this.Session.AdminUserProfile.ExpiryDate = EvStatics.getDateTime( "1 JAN 2100" );


        this.getDataObject ( clientDataObject );

        this.LogValue ( "Exit createObject" );

        return clientDataObject;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.User_Profile_Creation_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

       return this.Session.LastPage;;

    }//END method


    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Import user profile methods


    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.ClientClientDataObjectEvado.Model.UniForm.Command object.</param>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private EvEventCodes uploadUserProfiles ( String FileName )
    {
      this.LogMethod ( "updateObject" );
      this.LogValue ( "FileName: " + FileName );
      //
      // Initialise methods variables and objects.
      //
      this._ProcessLog = new StringBuilder ( );
      List<String> csvDataList = new List<string> ( );

      this.writeProcessLog ( "Commenced importing user profiles." );
      this.writeProcessLog ( "Upload filename: " + FileName );


      //
      // Read in the upload file as a list of string.
      //
      csvDataList = Evado.Model.EvStatics.Files.readFileAsList (
        this.UniForm_BinaryFilePath,
        FileName );

      this.LogValue ( "Files.DebugLog: " + Evado.Model.EvStatics.Files.DebugLog );

      this.LogValue ( "CSV list length: " + csvDataList.Count );

      //
      // Extract the list of user profile objects to be processed.
      //
      return getDataObjectFromCsv ( csvDataList );

    }//END uploadObjects method

    //===================================================================================
    /// <summary>
    /// This method converts a CSV datafile into a integration data object.
    /// </summary>
    /// <param name="CsvDataList">List of String: Csv encoded data object.</param>
    /// <returns>List of EvUserProfile objects</returns>
    //-----------------------------------------------------------------------------------
    private EvEventCodes getDataObjectFromCsv (
        List<String> CsvDataList )
    {
      this.LogMethod ( "GetDataObjectFromCsv method started." );
      this.LogValue ( "CsvDataList.Count: " + CsvDataList.Count );
      this.writeProcessLog ( "Processing in CSV data object." );

      //
      // Initialise the mathods variables and objects.
      //
      List<EdUserProfile> dataObjectList = new List<EdUserProfile> ( );
      String stFileContent = String.Empty;
      String line = String.Empty;
      List<String> Columns = new List<String> ( );
      EvEventCodes result = EvEventCodes.Ok;

      this.LogValue ( "CSV File read: data rows: " + CsvDataList.Count );

      if ( CsvDataList.Count < 2 )
      {
        this.writeProcessLog ( "Upload file does not have data" );
        return EvEventCodes.Data_Empty_Error;
      }

      Columns = getCsvColumns ( CsvDataList [ 0 ] );

      if ( Columns.Count < 8 )
      {
        this.writeProcessLog ( "The uploaded file does not have 8 columns" );
        return EvEventCodes.Data_Empty_Error;
      }

      //
      // Iterate through the csv rows.
      //
      for ( int csvRowCount = 1; csvRowCount < CsvDataList.Count; csvRowCount++ )
      {
        this.LogValue ( "row: " + csvRowCount );

        String csvRow = CsvDataList [ csvRowCount ];

        //
        // Get the CSV row as a string list.
        //
        List<String> dataRow = this.getCsvColumns ( csvRow );

        //
        // Update the user profile.
        //
        this.Session.AdminUserProfile = this.updateUserProfile ( Columns, dataRow );

        if ( this.Session.AdminUserProfile.Guid == Guid.Empty
          && String.IsNullOrEmpty ( this.Session.AdminUserProfile.Password ) == true )
        {
          this.LogValue ( "Creating a new password." );
          this.createDefaultPassword ( );
        }

        this.writeProcessLog ( "Processing User " + this.Session.AdminUserProfile.UserId );
        this.LogValue ( "AdminUserProfile: " + this.Session.AdminUserProfile.getUserProfile ( false ) );


        //
        // Save the user to ADS and the database.
        //
        result = this.saveUserProfile ( );

        if ( result != EvEventCodes.Ok )
        {
          this.LogValue ( "User " + this.Session.AdminUserProfile.UserId + " returned error "
            + EvStatics.enumValueToString ( result ) );

          this.writeProcessLog ( "User " + this.Session.AdminUserProfile.UserId + " returned error "
            + EvStatics.enumValueToString ( result ) );

          return result;
        }

        this.LogValue ( "User " + this.Session.AdminUserProfile.UserId + " successfully imported." );
        this.writeProcessLog ( "User " + this.Session.AdminUserProfile.UserId + " successfully imported." );

      }//END CSV row iteration loop.

      //
      // return the data object.
      //
      return result;

    }//END getDataObjectFromCsv method

    // ==================================================================================
    /// <summary>
    /// This method converts a CSV datafile into a integration data object.
    /// </summary>
    /// <param name="CsvDataList">List of String: Csv encoded data object.</param>
    /// <returns>List of EvUserProfile objects</returns>
    //-----------------------------------------------------------------------------------
    private List<String> getCsvColumns (
      String CsvDataRow )
    {
      this.LogMethod ( "getCsvColumns" );
      this.LogValue ( "CsvDataRow: " + CsvDataRow );
      //
      // Initialise the methods variables and objects.
      //
      List<String> HeaderList = new List<String> ( );

      if ( CsvDataRow.Contains ( "\",\"" ) == true )
      {
        CsvDataRow = CsvDataRow.Replace ( "\", \"", "\",\"" );
        CsvDataRow = CsvDataRow.Replace ( "\", \"", "\",\"" );
        CsvDataRow = CsvDataRow.Replace ( "\", \"", "\",\"" );
        CsvDataRow = CsvDataRow.Replace ( "\", \"", "\",\"" );
        CsvDataRow = CsvDataRow.Replace ( "\",\"", "~" );
        CsvDataRow = CsvDataRow.Replace ( "\"", "" );
        CsvDataRow = CsvDataRow.Replace ( ",", "~" );
      }
      else
      {
        CsvDataRow = CsvDataRow.Replace ( ",", "~" );
      }
      this.LogValue ( "ROW: " + CsvDataRow );

      String [ ] csvRowArray = CsvDataRow.Split ( '~' );

      this.LogValue ( "CSV data row length: " + csvRowArray.Length );

      //
      // Itrate through the array of header content 
      //
      for ( int i = 0; i < csvRowArray.Length; i++ )
      {
        String st = csvRowArray [ i ];

        HeaderList.Add ( st );
      }

      this.LogValue ( "HeaderList.Count: " + HeaderList.Count );

      return HeaderList;

    }//End getHeaderColums method

    // ==================================================================================
    /// <summary>
    /// This method converts a CSV datafile into a integration data object.
    /// </summary>
    /// <param name="CsvDataList">List of String: Csv encoded data object.</param>
    /// <returns>List of EvUserProfile objects</returns>
    //-----------------------------------------------------------------------------------
    private EdUserProfile updateUserProfile (
      List<String> Columns,
      List<String> DataRow )
    {
      this.LogMethod ( "getCsvColumns" );
      this.LogValue ( "Columns.Count: " + Columns.Count );
      this.LogValue ( "DataRow.Count: " + DataRow.Count );
      //
      // Initialise the methods variables and objects.
      //
      EdUserProfile userProfile = new EdUserProfile ( );
      EdUserProfile uploadedUserProfile = new EdUserProfile ( );
      EdUserProfile.UserProfileFieldNames field = EdUserProfile.UserProfileFieldNames.Null;

      //
      // Iterate through the columns filling the values into the uploaded user profile.
      //
      for ( int columnCount = 0; columnCount < Columns.Count && columnCount < DataRow.Count; columnCount++ )
      {
        String value = Columns [ columnCount ];

        this.LogValue ( "Column: " + value + ", Value: " + DataRow [ columnCount ] );

        if ( EvStatics.Enumerations.tryParseEnumValue<EdUserProfile.UserProfileFieldNames> (
          value, out field ) == true )
        {
          uploadedUserProfile.setValue ( field, DataRow [ columnCount ] );
        }
      }

      userProfile = this._Bll_UserProfiles.getItem ( uploadedUserProfile.UserId );

      if ( userProfile.Guid == Guid.Empty )
      {
        userProfile.UserId = uploadedUserProfile.UserId;
      }
      if ( userProfile.TypeId != uploadedUserProfile.TypeId )
      {
        userProfile.TypeId = uploadedUserProfile.TypeId;
      }
      if ( userProfile.Prefix != uploadedUserProfile.Prefix )
      {
        userProfile.Prefix = uploadedUserProfile.Prefix;
      }
      if ( userProfile.GivenName != uploadedUserProfile.GivenName )
      {
        userProfile.GivenName = uploadedUserProfile.GivenName;
      }
      if ( userProfile.FamilyName != uploadedUserProfile.FamilyName )
      {
        userProfile.CommonName = uploadedUserProfile.CommonName;
      }
      if ( userProfile.Title != uploadedUserProfile.Title )
      {
        userProfile.Title = uploadedUserProfile.Title;
      }
      if ( userProfile.Telephone != uploadedUserProfile.Telephone )
      {
        userProfile.Telephone = uploadedUserProfile.Telephone;
      }
      if ( userProfile.EmailAddress != uploadedUserProfile.EmailAddress )
      {
        userProfile.EmailAddress = uploadedUserProfile.EmailAddress;
      }
      if ( userProfile.Roles != uploadedUserProfile.Roles )
      {
        userProfile.Roles = uploadedUserProfile.Roles;
      }

      return userProfile;

    }//End updateUserProfile method


    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Update User methods
    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.ClientClientDataObjectEvado.Model.UniForm.Command object.</param>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData updateObject ( Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "updateObject" );
      this.LogValue ( "Parameter: " + PageCommand.getAsString ( false, false ) );

      this.LogValue ( "eClinical.AdminUserProfile:" );
      this.LogValue ( "Guid: " + this.Session.AdminUserProfile.Guid );
      this.LogValue ( "UserId: " + this.Session.AdminUserProfile.UserId );
      this.LogValue ( "CommonName: " + this.Session.AdminUserProfile.CommonName );

      try
      {
        // 
        // Initialise the methods variables and objects.
        //      
        Evado.Model.UniForm.AppData clientDataObject = new Model.UniForm.AppData ( );
        EvEventCodes result;

        //
        // Determine if the user has access to this page and log and error if they do not.
        //
        if ( this.Session.UserProfile.hasManagementAccess == false )
        {
          this.LogIllegalAccess (
            this.ClassNameSpace + "updateObject",
            this.Session.UserProfile );

          this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

          return this.Session.LastPage;
        }

        // 
        // Update the object.
        // 
        if ( this.updateObjectValue ( PageCommand ) == false )
        {
          this.ErrorMessage = EdLabels.UserProfile_Value_Update_Error_Message;

          return this.Session.LastPage;
        }

        this.Session.UserProfile.UserId = EvStatics.CleanSamUserId (
          this.Session.UserProfile.UserId );

        //
        // Update the address field.
        //
        this.updateAddressValue ( PageCommand );

        //
        // Perform new user ADS duplication validation.
        //
        if ( this.newUserDuplicateValidation ( ) == false )
        {
          this.ErrorMessage = EdLabels.UserProfile_Duplicate_User_Id_Error_Message;

          this.LogMethodEnd ( "saveUserProfile" );

          return this.Session.LastPage;
        }

        if ( PageCommand.hasParameter ( EuUserProfiles.CONST_NEW_PASSWORD_PARAMETER ) == true )
        {
          this.LogValue ( "Creating a new password." );
          this.createDefaultPassword ( );
        }

        this.LogValue ( "AdminUserProfile.Password: " + this.Session.AdminUserProfile.Password );

        // 
        // Get the save action message value.
        // 
        String stSaveAction = PageCommand.GetParameter (  Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION );

        // 
        // Resets the save action to the parameter passed in the page groupCommand.
        // 
        if ( stSaveAction == EuUserProfiles.CONST_DELETE_ACTION )
        {
          this.Session.AdminUserProfile.CommonName = String.Empty;
        }

        result = this.saveUserProfile ( );

        if ( result != EvEventCodes.Ok )
        {
          return this.Session.LastPage;
        }

        this.LogMethodEnd ( "updateObject" );

        return new Model.UniForm.AppData();

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.User_Profile_Save_Error_Message;

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
    /// THis method validates that the user id is not duplicated in the ADS
    /// </summary>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private bool newUserDuplicateValidation ( )
    {
      this.LogMethod ( "newUserDuplicateValidation" );
      this.LogValue ( "AdminUserProfile.UserId: " + this.Session.AdminUserProfile.UserId );
      this.LogValue ( "AdminUserProfile.Guid: " + this.Session.AdminUserProfile.Guid );

      //
      // Skip validation for existing user profiles.
      //
      if ( this.Session.AdminUserProfile.Guid != EvStatics.CONST_NEW_OBJECT_ID )
      {
        this.LogValue ( "Existing user" );
        return true;
      }

      bool duplicateUser = !this._Bll_UserProfiles.ExistingUserId ( this.Session.AdminUserProfile.UserId );

      this.LogClass ( this._Bll_UserProfiles.Log );

      this.LogMethodEnd ( "newUserDuplicateValidation" );
      return duplicateUser;
    }

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command updated.</param>
    /// <returns></returns>
    //  ----------------------------------------------------------------------------------
    private void updateAddressValue (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "updateAddressValue" );
      this.LogValue ( "Parameters.Count: " + PageCommand.Parameters.Count );

      //
      // Get the organisation's address 
      //
      String stAddress = PageCommand.GetParameter ( EuUserProfiles.CONST_ADDRESS_FIELD_ID );

      //
      // If there is no address object exit.
      //
      if ( stAddress == String.Empty )
      {
        this.LogValue ( "Address string empty" );
        this.LogMethodEnd ( "updateAddressValue" );
        return;
      }

      if ( stAddress.Contains ( ";" ) == false )
      {
        this.LogValue ( "Address missing delimiters." );
        this.LogMethodEnd ( "updateAddressValue" );
        return;
      }

      String [ ] arAddress = stAddress.Split ( ';' );

      if ( arAddress.Length > 5 )
      {
        this.Session.AdminUserProfile.Address_1 = arAddress [ 0 ];
        this.Session.AdminUserProfile.Address_2 = arAddress [ 1 ];
        this.Session.AdminUserProfile.AddressCity = arAddress [ 2 ];
        this.Session.AdminUserProfile.AddressState = arAddress [ 3 ];
        this.Session.AdminUserProfile.AddressPostCode = arAddress [ 4 ];
        this.Session.AdminUserProfile.AddressCountry = arAddress [ 5 ];
      }
      this.LogMethodEnd ( "updateAddressValue" );

    }//END updateAddressValue Method

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="Parameters">List of field values to be updated.</param>
    /// <returns></returns>
    //  ----------------------------------------------------------------------------------
    private bool updateObjectValue (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "updateObjectValue" );
      this.LogValue ( "Parameters.Count: " + PageCommand.Parameters.Count );
      this.LogValue ( "AdminUser.Guid: " + this.Session.AdminUserProfile.Guid );

      // 
      // Iterate through the parameter values updating the ResultData object
      // 
      foreach ( Evado.Model.UniForm.Parameter parameter in PageCommand.Parameters )
      {
        this.LogTextStart ( parameter.Name + " = " + parameter.Value );

        if ( parameter.Name.Contains (  Evado.Model.Digital.EvcStatics.CONST_GUID_IDENTIFIER ) == false
          && parameter.Name != Evado.Model.UniForm.CommandParameters.Custom_Method.ToString ( )
          && parameter.Name != Evado.Model.UniForm.CommandParameters.Page_Id.ToString ( )
          && parameter.Name !=  Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION
          && parameter.Name != EuUserProfiles.CONST_ADDRESS_FIELD_ID
          && parameter.Name != EuUserProfiles.CONST_CURRENT_FIELD_ID
          && parameter.Name != EuUserProfiles.CONST_NEW_PASSWORD_PARAMETER )
        {
          this.LogTextEnd ( " >> UPDATED" );
          try
          {
             Evado.Model.Digital.EdUserProfile.UserProfileFieldNames fieldName =
               Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue< Evado.Model.Digital.EdUserProfile.UserProfileFieldNames> (
              parameter.Name );

            this.Session.AdminUserProfile.setValue ( fieldName, parameter.Value );

          }
          catch ( Exception Ex )
          {
            this.LogException ( Ex );

            return false;
          }
        }
        else
        {
          this.LogTextEnd ( " >> SKIPPED" );
        }

      }// End iteration loop

      //
      // IF the AD user id is empty set it to save value as the UserID.
      //
      if ( this.Session.AdminUserProfile.ActiveDirectoryUserId == String.Empty )
      {
        this.Session.AdminUserProfile.ActiveDirectoryUserId = this.Session.AdminUserProfile.UserId;
      }

      return true;

    }//END updateObjectValue method.

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Process Log methods.
    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Content">String:  debug text.</param>
    //  ----------------------------------------------------------------------------------
    protected void writeProcessLog ( String Content )
    {
      this._ProcessLog.AppendLine (
           DateTime.Now.ToString ( "dd-MM-yy HH:mm:ss" )
         + ": " + Content );
    }
    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
  }
}//END namespace