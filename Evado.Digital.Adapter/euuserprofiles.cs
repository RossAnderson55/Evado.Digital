﻿/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\UserProfiles.cs" company="EVADO HOLDING PTY. LTD.">
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
      EuGlobalObjects AdapterObjects,
      EvUserProfileBase ServiceUserProfile,
      EuSession SessionObjects,
      String UniFormBinaryFilePath,
      String UniForm_BinaryServiceUrl,
      EvClassParameters Settings )
    {
      this.AdapterObjects = AdapterObjects;
      this.ServiceUserProfile = ServiceUserProfile;
      this.Session = SessionObjects;
      this.UniForm_BinaryFilePath = UniFormBinaryFilePath;
      this.UniForm_BinaryServiceUrl = UniForm_BinaryServiceUrl;
      this.ClassParameters = Settings;

      this.ClassNameSpace = "Evado.UniForm.Clinical.EuUserProfiles.";
      this.LogInitMethod ( "UserProfiles initialisation" );
      this.LogInit ( "ServiceUserProfile.UserId: " + ServiceUserProfile.UserId );
      this.LogInit ( "Session.UserProfile.Userid: " + this.Session.UserProfile.UserId );
      this.LogInit ( "Session.UserProfile.CommonName: " + this.Session.UserProfile.CommonName );
      this.LogInit ( "UniFormBinaryFilePath: " + this.UniForm_BinaryFilePath );
      this.LogInit ( "UniForm BinaryServiceUrl: " + this.UniForm_BinaryServiceUrl );

      this.LogInit ( "Settings:" );
      this.LogInit ( "-PlatformId: " + this.ClassParameters.PlatformId );
      this.LogInit ( "-ApplicationGuid: " + this.ClassParameters.AdapterGuid );
      this.LogInit ( "-LoggingLevel: " + Settings.LoggingLevel );
      this.LogInit ( "-UserId: " + Settings.UserProfile.UserId );
      this.LogInit ( "-CommonName: " + Settings.UserProfile.CommonName );

      if ( this.Session.SelectedUserType == String.Empty )
      {
        this.Session.SelectedUserType = "End_User";
      }

      if ( this.Session.AdminOrganisationList == null )
      {
        this.Session.AdminOrganisationList = new List<EdOrganisation> ( );
      }

      this._Bll_UserProfiles = new Evado.Digital.Bll.EdUserprofiles ( this.ClassParameters );

    }//END Method


    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class constants and variables.

    private const String CONST_ADDRESS_FIELD_ID = "ADDRESS";
    private const String CONST_CURRENT_FIELD_ID = "CURRENT";
    private const String CONST_NEW_PASSWORD_PARAMETER = "NPWD";
    private const String CONST_DELETE_ACTION = "DELETE";
    private const String CONST_DOWNLOAD_EXTENSION = "user-profiles.csv";


    private const String CONST_EMAIL_SUBJECT = "EMAIL_SUBJECT";
    private const String CONST_EMAIL_BODY = "EMAIL_BODY";

    private Evado.Digital.Bll.EdUserprofiles _Bll_UserProfiles = new Evado.Digital.Bll.EdUserprofiles ( );

    private Evado.Digital.Model.EdStaticPageIds PageId = Evado.Digital.Model.EdStaticPageIds.Null;

    private System.Text.StringBuilder _ProcessLog = null;

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class public methods

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
      this.LogValue ( "Parameter PageCommand " + PageCommand.getAsString ( false, false ) );

      try
      {
        // 
        // Initialise the methods variables and objects.
        // 
        Evado.UniForm.Model.EuAppData clientDataObject = new Evado.UniForm.Model.EuAppData ( );

        //
        // Load the customer group.
        //
        this.getAdsCustomerGroup ( );

        this.getUpdateSelectionParameters ( PageCommand );

        //
        // Retrieve the groupCommand parameters to determine the type of page to generate.
        //
        String stPageType = PageCommand.GetParameter ( Evado.UniForm.Model.EuCommandParameters.Page_Id );

        if ( stPageType != String.Empty )
        {
          this.PageId = Evado.Model.EvStatics.parseEnumValue<Evado.Digital.Model.EdStaticPageIds> ( stPageType );
        }
        this.LogValue ( "SessionObjects.PageType: " + this.PageId );

        //
        // Skip if the updating the current user properties.
        //
        if ( this.PageId != Evado.Digital.Model.EdStaticPageIds.My_User_Profile_Update_Page )
        {
          if ( this.Session.AdminUserProfile == null )
          {
            this.Session.AdminUserProfile = new Evado.Digital.Model.EdUserProfile ( );
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
          case Evado.UniForm.Model.EuMethods.List_of_Objects:
            {

              switch ( this.PageId )
              {
                case Evado.Digital.Model.EdStaticPageIds.My_User_Profile_Update_Page:
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
          case Evado.UniForm.Model.EuMethods.Get_Object:
            {
              this.LogValue ( "Case: ApplicationMethods.Get_Object. " );

              switch ( this.PageId )
              {
                case Evado.Digital.Model.EdStaticPageIds.My_User_Profile_Update_Page:
                  {
                    clientDataObject = this.getObject_MyUserProfile ( PageCommand );
                    break;
                  }
                case Evado.Digital.Model.EdStaticPageIds.Email_User_Page:
                  {
                    clientDataObject = this.getObject_EmailPage ( PageCommand );
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
          case Evado.UniForm.Model.EuMethods.Create_Object:
            {
              clientDataObject = this.createObject ( PageCommand );

              break;
            }
          case Evado.UniForm.Model.EuMethods.Save_Object:
          case Evado.UniForm.Model.EuMethods.Delete_Object:
            {
              this.LogValue ( " Save Object method" );


              switch ( this.PageId )
              {
                case Evado.Digital.Model.EdStaticPageIds.My_User_Profile_Update_Page:
                  {
                    clientDataObject = this.updateUserObject ( PageCommand );
                    break;
                  }
                case Evado.Digital.Model.EdStaticPageIds.Email_User_Page:
                  {
                    clientDataObject = this.sendEmail ( PageCommand );
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

        if ( this.ErrorMessage != String.Empty )
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

    // ==================================================================================
    /// <summary>
    /// This methods returns a pageMenuGroup object contains a selection of organisations.
    /// </summary>
    /// <param name="PageObject">Application</param>
    /// <returns>Evado.UniForm.Model.EuGroup object</returns>
    //  ---------------------------------------------------------------------------------
    public void getUpdateSelectionParameters (
      Evado.UniForm.Model.EuCommand PageCommand )
    {
      this.LogMethod ( "getUpdateSelectionParameters" );

      if ( this.Session.SelectedOrgId == null )
      {
        this.Session.SelectedOrgId = String.Empty;
      }

      //
      // Update the user selection.
      //
      this.Session.SelectedUserType = PageCommand.GetParameter (
        EdUserProfile.FieldNames.User_Type );

      //
      // Update the user organisation selection.
      //
      this.Session.SelectedOrgId = PageCommand.GetParameter (
        EdUserProfile.FieldNames.OrgId );

      this.LogMethodEnd ( "getUpdateSelectionParameters" );

    }// END getUpdateSelectionParameters method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class list methods

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.EuCommand object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.UniForm.Model.EuAppData getListObject (
      Evado.UniForm.Model.EuCommand PageCommand )
    {
      this.LogMethod ( "getListObject" );
      this.LogValue ( "PageCommand: " + PageCommand.getAsString ( false, true ) );

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.EuAppData clientDataObject = new Evado.UniForm.Model.EuAppData ( );
      String userType = String.Empty;

      //
      // Determine if the user has access to this page and log and error if they do not.
      //
      if ( this.Session.UserProfile.hasAdministrationAccess == false )
      {
        this.LogIllegalAccess (
          this.ClassNameSpace + "getListObject",
          this.Session.UserProfile );

        this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

        return this.Session.LastPage; ;
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
      clientDataObject.Page.PageId = Evado.Digital.Model.EdStaticPageIds.User_View.ToString ( );
      clientDataObject.Page.PageDataGuid = clientDataObject.Id;
      clientDataObject.Title = EdLabels.User_Profile_Selection_Page_Title;
      clientDataObject.Page.Title = clientDataObject.Title;
      clientDataObject.Page.PageDataGuid = clientDataObject.Page.Id;

      // 
      // get the current user type selection identifier.
      // 
      if ( PageCommand.hasParameter (
          EdUserProfile.FieldNames.User_Type.ToString ( ) ) == true )
      {
        userType = PageCommand.GetParameter (
          EdUserProfile.FieldNames.User_Type );

        this.LogDebug ( "Parameter set User Type: " + userType );

      }//END user type parameter exists.

      //
      // set the page commands.
      //
      this.getListPage_Commands ( clientDataObject.Page );

      //
      // display the user upload group.
      //
      if ( this.PageId == Evado.Digital.Model.EdStaticPageIds.User_Upload_Page )
      {
        this.LogValue ( "Processing upload page." );
        this.getList_Upload_Group ( PageCommand, clientDataObject.Page );
      }

      //
      // Display the user download group
      //
      if ( this.PageId == Evado.Digital.Model.EdStaticPageIds.User_DownLoad_Page )
      {
        this.LogValue ( "Processing download page." );
        this.getList_Download_Group ( clientDataObject.Page );
      }

      // 
      // Add the organisation list list field.
      // 
      this.getList_Selection_Group ( clientDataObject.Page );

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
    /// <returns>Evado.UniForm.Model.EuGroup object</returns>
    //  ---------------------------------------------------------------------------------
    public void getListPage_Commands (
      Evado.UniForm.Model.EuPage PageObject )
    {
      this.LogMethod ( "getListPage_Commands" );
      //
      // initialise the methods variables and objects.
      //
      Evado.UniForm.Model.EuCommand pageCommand = new Evado.UniForm.Model.EuCommand ( );

      /*
      if ( this._ApplicationObjects.HelpUrl != String.Empty )
      {
        pageCommand = PageObject.addCommand (
           EdLabels.Label_Help_Command_Title,
           EuAdapter.APPLICATION_ID,
           EuAdapter.ApplicationObjects.Users.ToString ( ),
           Evado.UniForm.Model.EuMethods.Get_Object );

        pageCommand.Type = Evado.UniForm.Model.EuCommandTypes.Html_Link;

        pageCommand.AddParameter ( Evado.UniForm.Model.EuCommandParameters.Link_Url,
         EvStatics.createHelpUrl (
          this._ApplicationObjects.HelpUrl,
           Evado.Digital.Model.EvPageIds.User_View ) );
      }
      */

      //
      // Display the download user profiles page command
      //
      if ( this.PageId != Evado.Digital.Model.EdStaticPageIds.User_DownLoad_Page )
      {
        pageCommand = PageObject.addCommand (
           EdLabels.UserProfile_Downoad_Command_Title,
           EuAdapter.ADAPTER_ID,
           EuAdapterClasses.Users.ToString ( ),
           Evado.UniForm.Model.EuMethods.Custom_Method );

        pageCommand.setCustomMethod ( Evado.UniForm.Model.EuMethods.List_of_Objects );
        pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.User_DownLoad_Page );
      }

      //
      // display the upload user profiles page command.
      //
      if ( this.PageId != Evado.Digital.Model.EdStaticPageIds.User_Upload_Page )
      {
        pageCommand = PageObject.addCommand (
           EdLabels.UserProfile_Upload_Command_Title,
           EuAdapter.ADAPTER_ID,
           EuAdapterClasses.Users.ToString ( ),
           Evado.UniForm.Model.EuMethods.Custom_Method );

        pageCommand.setCustomMethod ( Evado.UniForm.Model.EuMethods.List_of_Objects );
        pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.User_Upload_Page );
      }

      this.LogMethodEnd ( "getListPage_Commands" );

    }//END getListPage_Commands method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.EuCommand object.</param>
    /// <param name="PageObject">Evado.UniForm.Model.EuPage object.</param>
    //  ------------------------------------------------------------------------------
    private void getList_Upload_Group (
      Evado.UniForm.Model.EuCommand PageCommand,
      Evado.UniForm.Model.EuPage PageObject )
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
    /// <param name="PageObject">Evado.UniForm.Model.EuAppData object.</param>
    //  ------------------------------------------------------------------------------
    private void getUserUploadSelectionGroup (
      Evado.UniForm.Model.EuPage PageObject )
    {
      this.LogMethod ( "getUserProfileUploadDataObject" );

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
      pageGroup = PageObject.AddGroup (
        EdLabels.UserProfile_Upload_Group_Title,
        Evado.UniForm.Model.EuEditAccess.Enabled );
      pageGroup.Layout = Evado.UniForm.Model.EuGroupLayouts.Full_Width;

      pageGroup.SetCommandBackBroundColor (
        Evado.UniForm.Model.EuGroupParameters.BG_Mandatory,
        Evado.UniForm.Model.EuBackgroundColours.Red );

      groupField = pageGroup.createBinaryFileField (
        EuRecordLayouts.CONST_TEMPLATE_FIELD_ID,
        EdLabels.UserProfile_Upload_Field_Title,
        String.Empty,
        this.Session.UploadFileName );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      groupField.AddParameter ( Evado.UniForm.Model.EuFieldParameters.Snd_Cmd_On_Change, "Yes" );

      groupCommand = pageGroup.addCommand (
        EdLabels.UserProfile_Upload_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Users.ToString ( ),
        Evado.UniForm.Model.EuMethods.Custom_Method );

      groupCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.User_Upload_Page );
      groupCommand.setCustomMethod ( Evado.UniForm.Model.EuMethods.List_of_Objects );

    }//END getUserProfileUploadDataObject Method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">Evado.UniForm.Model.EuAppData object.</param>
    //  ------------------------------------------------------------------------------
    private void saveUserProfileUpload (
      Evado.UniForm.Model.EuCommand PageCommand,
      Evado.UniForm.Model.EuPage PageObject )
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
      Evado.UniForm.Model.EuGroup pageGroup = new Evado.UniForm.Model.EuGroup ( );

      //
      // Define the general properties pageMenuGroup..
      //
      pageGroup = PageObject.AddGroup (
        String.Empty,
        Evado.UniForm.Model.EuEditAccess.Inherited );
      pageGroup.Layout = Evado.UniForm.Model.EuGroupLayouts.Full_Width;

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

      pageGroup.Description = this._ProcessLog.ToString ( );
      //
      // reset the form template filename.
      //
      this.Session.UploadFileName = String.Empty;

    }//END getUserProfileUploadDataObject Method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.EuCommand object.</param>
    //  ------------------------------------------------------------------------------
    private void getList_Download_Group (
      Evado.UniForm.Model.EuPage PageObject )
    {
      this.LogMethod ( "getList_Download_Group" );
      this.LogValue ( "UniForm_BinaryFilePath: " + this.UniForm_BinaryFilePath );
      this.LogValue ( "UniForm_BinaryServiceUrl: " + this.UniForm_BinaryServiceUrl );

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.EuGroup pageGroup = new Evado.UniForm.Model.EuGroup ( );
      Evado.UniForm.Model.EuField groupField = new Evado.UniForm.Model.EuField ( );
      System.Text.StringBuilder outputFile = new StringBuilder ( );
      Evado.UniForm.Model.EuParameter parameter = new Evado.UniForm.Model.EuParameter ( );
      String downloadFileName = String.Empty;
      String downloadUrl = String.Empty;

      //
      // Define the general properties pageMenuGroup..
      //
      pageGroup = PageObject.AddGroup (
        String.Empty,
        Evado.UniForm.Model.EuEditAccess.Inherited );
      pageGroup.Layout = Evado.UniForm.Model.EuGroupLayouts.Full_Width;

      List<EdUserProfile> userProfileList = this._Bll_UserProfiles.GetView (
        this.Session.SelectedUserType,
        this.Session.SelectedOrgId );

      //
      // if the user profile list exists display the download link.
      //
      if ( userProfileList.Count > 0 )
      {
        String csvText = "\"" + EdUserProfile.FieldNames.UserId + "\","
          + "\"" + EdUserProfile.FieldNames.Password + "\","
          + "\"" + EdUserProfile.FieldNames.User_Type + "\","
          + "\"" + EdUserProfile.FieldNames.Prefix + "\","
          + "\"" + EdUserProfile.FieldNames.Given_Name + "\","
          + "\"" + EdUserProfile.FieldNames.Family_Name + "\","
          + "\"" + EdUserProfile.FieldNames.CommonName + "\","
          + "\"" + EdUserProfile.FieldNames.Title + "\","
          + "\"" + EdUserProfile.FieldNames.Email_Address + "\","
          + "\"" + EdUserProfile.FieldNames.RoleId + "\"";

        outputFile.AppendLine ( csvText );

        //
        // iterate through the users in the list.
        //
        foreach ( EdUserProfile user in userProfileList )
        {
          csvText = "\"" + user.UserId + "\","
            + "\"" + user.Password + "\","
            + "\"" + user.UserType + "\","
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
        if ( this.Session.SelectedUserType != String.Empty )
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
        pageGroup.Description = EdLabels.UserProfile_Download_Empty_List_Message;
      }
      this.LogMethodEnd ( "getList_Download_Group" );

    }//END getList_Download_Group method

    // ==================================================================================
    /// <summary>
    /// This methods returns a pageMenuGroup object contains a selection of organisations.
    /// </summary>
    /// <param name="PageObject">Application</param>
    /// <returns>Evado.UniForm.Model.EuGroup object</returns>
    //  ---------------------------------------------------------------------------------
    public void getList_Selection_Group (
      Evado.UniForm.Model.EuPage PageObject )
    {
      this.LogMethod ( "getList_Selection_Group" );

      // 
      // initialise the methods variables and objects.
      // 
      List<Evado.Model.EvOption> optionList = new List<Evado.Model.EvOption> ( );
      Evado.UniForm.Model.EuField groupField = new Evado.UniForm.Model.EuField ( );

      Evado.UniForm.Model.EuGroup pageGroup = PageObject.AddGroup (
        EdLabels.UserProfile_List_Selection_Group,
        Evado.UniForm.Model.EuEditAccess.Enabled );
      pageGroup.Layout = Evado.UniForm.Model.EuGroupLayouts.Full_Width;

      //
      // Create the organisation selection list.
      //
      optionList = new List<Evado.Model.EvOption> ( );
      optionList.Add ( new EvOption ( ) );

      foreach ( EdOrganisation org in this.AdapterObjects.OrganisationList )
      {
        optionList.Add ( new EvOption ( org.OrgId, org.LinkText ) );
      }

      // 
      // Set the selection to the current site org id.
      // 
      groupField = pageGroup.createSelectionListField (
        EdUserProfile.FieldNames.OrgId,
        EdLabels.User_Profile_Organisation_List_Field_Label,
        this.Session.SelectedUserType.ToString ( ),
        optionList );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      groupField.AddParameter ( Evado.UniForm.Model.EuFieldParameters.Snd_Cmd_On_Change, 1 );

      //
      // get the list of user types and create the user type selection field.
      //
      String userCategory = this.AdapterObjects.Settings.UserCategoryList;
      if ( userCategory == String.Empty )
      {
        userCategory = "UserCategory";
      }

      optionList = this.AdapterObjects.getSelectionOptions ( userCategory, String.Empty, false, true );

      if ( this.Session.UserProfile.hasAdministrationAccess == true )
      {
        optionList.Add ( new EvOption ( EdUserProfile.CONST_USER_TYPE_EVADO ) );
        optionList.Add ( new EvOption ( EdUserProfile.CONST_USER_TYPE_CUSTOMER ) );
      }
      EvStatics.sortOptionListValues ( optionList );

      // 
      // Set the selection to the current site org id.
      // 
      groupField = pageGroup.createSelectionListField (
        EdUserProfile.FieldNames.User_Type,
        EdLabels.UserProfile_User_Type_Field_Label,
        this.Session.SelectedUserType.ToString ( ),
        optionList );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      groupField.AddParameter ( Evado.UniForm.Model.EuFieldParameters.Snd_Cmd_On_Change, 1 );

      // 
      // Create a custom groupCommand to process the selection.
      // 
      Evado.UniForm.Model.EuCommand customCommand = pageGroup.addCommand (
        EdLabels.User_Profile_Organisation_Selection_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Users.ToString ( ),
        Evado.UniForm.Model.EuMethods.Custom_Method );

      // 
      // Set the custom groupCommand parameter.
      // 
      customCommand.setCustomMethod ( Evado.UniForm.Model.EuMethods.List_of_Objects );

      this.LogMethodEnd ( "getList_Selection_Group" );

    }//END getList_Selection_Group method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.EuCommand object.</param>
    /// <returns>Evado.UniForm.Model.EuAppData object</returns>
    //  ------------------------------------------------------------------------------
    private void getList_UserProfiles_Group (
      Evado.UniForm.Model.EuPage PageObject )
    {
      this.LogMethod ( "getList_UserProfiles_Group" );
      try
      {
        //  
        // Create the list pageMenuGroup.
        // 
        Evado.UniForm.Model.EuGroup PageGroup = PageObject.AddGroup (
          EdLabels.User_Profile_List_Group_Title,
          String.Empty,
          Evado.UniForm.Model.EuEditAccess.Enabled );
        PageGroup.CmdLayout = Evado.UniForm.Model.EuGroupCommandListLayouts.Vertical_Orientation;
        PageGroup.Layout = Evado.UniForm.Model.EuGroupLayouts.Full_Width;

        // 
        // Add the save groupCommand
        // 
        Evado.UniForm.Model.EuCommand newCommand = PageGroup.addCommand (
          EdLabels.User_Profile_New_User_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Users.ToString ( ),
          Evado.UniForm.Model.EuMethods.Create_Object );

        newCommand.AddParameter (
          EdUserProfile.FieldNames.User_Type.ToString ( ),
          this.Session.SelectedUserType.ToString ( ) );

        newCommand.SetBackgroundColour (
          Evado.UniForm.Model.EuCommandParameters.BG_Default,
          Evado.UniForm.Model.EuBackgroundColours.Purple );

        // 
        // get the list of customers.
        // 
        this.Session.AdminUserProfileList = this._Bll_UserProfiles.GetView (
        this.Session.SelectedUserType,
        this.Session.SelectedOrgId );

        this.LogClass ( this._Bll_UserProfiles.Log );

        this.LogValue ( " list count: " + this.Session.AdminUserProfileList.Count );

        // 
        // generate the page links.
        // 
        foreach ( Evado.Digital.Model.EdUserProfile userProfile in this.Session.AdminUserProfileList )
        {
          Evado.UniForm.Model.EuCommand command = PageGroup.addCommand (
            userProfile.CommandText,
            EuAdapter.ADAPTER_ID,
            EuAdapterClasses.Users.ToString ( ),
            Evado.UniForm.Model.EuMethods.Get_Object );

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
    /// <param name="PageCommand">Evado.UniForm.Model.EuCommand object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.UniForm.Model.EuAppData getObject (
      Evado.UniForm.Model.EuCommand PageCommand )
    {
      this.LogMethod ( "getObject" );
      // 
      // Initialise the methods variables and objects.
      // 
      Guid subjectGuid = Guid.Empty;
      Evado.UniForm.Model.EuAppData clientDataObject = new Evado.UniForm.Model.EuAppData ( );

      //
      // Determine if the user has access to this page and log and error if they do not.
      //
      if ( this.Session.UserProfile.hasAdministrationAccess == false )
      {
        this.LogIllegalAccess (
          this.ClassNameSpace + "getObject",
          this.Session.UserProfile );

        this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

        return this.Session.LastPage; ;
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
          return this.Session.LastPage; ;
        }

        this.LogValue ( "userGuid: " + userGuid );

        // 
        // Retrieve the customer object from the database via the DAL and BLL layers.
        // 
        this.Session.AdminUserProfile = this._Bll_UserProfiles.getItem ( userGuid );

        if ( this.Session.AdminUserProfile.Guid == Guid.Empty )
        {
          this.LogValue ( "this.SessionObjects.AdminUserProfile is empty." );

          return this.Session.LastPage; ;
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

      return this.Session.LastPage; ;

    }//END getObject method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="ClientDataObject">Evado.UniForm.Model.EuAppData object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject (
      Evado.UniForm.Model.EuAppData ClientDataObject )
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
      ClientDataObject.Page.PageId = Evado.Digital.Model.EdStaticPageIds.User_Profile_Page.ToString ( );
      ClientDataObject.Page.EditAccess = Evado.UniForm.Model.EuEditAccess.Enabled;

      this.LogValue ( "clientDataObject status: " + ClientDataObject.Page.EditAccess );
      //
      // Set the user edit access to the objects.
      //
      ClientDataObject.Page.EditAccess = Evado.UniForm.Model.EuEditAccess.Disabled;

      if ( this.Session.UserProfile.hasAdministrationAccess == true )
      {
        ClientDataObject.Page.EditAccess = Evado.UniForm.Model.EuEditAccess.Enabled;
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
    /// <param name="Page"> Evado.UniForm.Model.EuPage object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_PageCommands (
      Evado.UniForm.Model.EuPage Page )
    {
      this.LogMethod ( "getDataObject_PageCommands" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.EuCommand pageCommand = new Evado.UniForm.Model.EuCommand ( );

      // 
      // Add the save groupCommand
      // 
      pageCommand = Page.addCommand (
        EdLabels.User_Profile_Save_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Users.ToString ( ),
        Evado.UniForm.Model.EuMethods.Save_Object );

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
        Evado.UniForm.Model.EuMethods.Save_Object );

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
    /// <param name="Page">Evado.UniForm.Model.EuPage object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_FieldGroup (
      Evado.UniForm.Model.EuPage Page )
    {
      this.LogMethod ( "getDataObject_FieldGroup" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.EuField groupField = new Evado.UniForm.Model.EuField ( );
      Evado.UniForm.Model.EuGroup pageGroup = new Evado.UniForm.Model.EuGroup ( );
      Evado.UniForm.Model.EuCommand groupCommand = new Evado.UniForm.Model.EuCommand ( );
      List<EvOption> optionList = new List<EvOption> ( );

      Evado.UniForm.Model.EuEditAccess initialAccess = Evado.UniForm.Model.EuEditAccess.Enabled;

      if ( this.Session.AdminUserProfile.Guid != Evado.Digital.Model.EvcStatics.CONST_NEW_OBJECT_ID )
      {
        initialAccess = Evado.UniForm.Model.EuEditAccess.Disabled;
      }
      this.LogDebug ( "Initial EditAcess {0}.", initialAccess );

      // 
      // create the page pageMenuGroup
      // 
      pageGroup = Page.AddGroup (
         String.Empty,
         Evado.UniForm.Model.EuEditAccess.Inherited );
      pageGroup.Layout = Evado.UniForm.Model.EuGroupLayouts.Full_Width;
      pageGroup.EditAccess = Evado.UniForm.Model.EuEditAccess.Enabled;

      pageGroup.SetCommandBackBroundColor (
        Evado.UniForm.Model.EuGroupParameters.BG_Mandatory,
        Evado.UniForm.Model.EuBackgroundColours.Red );

      //
      // Add the groups commands.
      //
      this.getDataObject_GroupCommands ( pageGroup );

      //
      // Create the organisation selection list.
      //
      optionList = new List<Evado.Model.EvOption> ( );
      optionList.Add ( new EvOption ( ) );

      foreach ( EdOrganisation org in this.AdapterObjects.OrganisationList )
      {
        optionList.Add ( org.Option );
      }

      // 
      // Set the selection to the current site org id.
      // 
      groupField = pageGroup.createSelectionListField (
        EdUserProfile.FieldNames.OrgId,
        EdLabels.User_Profile_Organisation_List_Field_Label,
        this.Session.AdminUserProfile.OrgId.ToString ( ),
        optionList );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      // 
      // Create the user id object
      // 
      groupField = pageGroup.createTextField (
         Evado.Digital.Model.EdUserProfile.FieldNames.UserId.ToString ( ),
        EdLabels.User_Profile_Identifier_Field_Label,
        this.Session.AdminUserProfile.UserId,
        80 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;
      groupField.Mandatory = true;
      groupField.setBackgroundColor (
        Evado.UniForm.Model.EuFieldParameters.BG_Mandatory,
        Evado.UniForm.Model.EuBackgroundColours.Red );
      groupField.EditAccess = initialAccess;

      // 
      // Create the Password object
      // 
      /*
      if ( this.SessionObjects.AdminUserProfile.Guid ==  Evado.Digital.Model.EvStatics.CONST_NEW_OBJECT_ID )
      {
        groupField = pageGroup.createTextField (
           Evado.Digital.Model.EvUserProfile.UserProfileFieldNames.Password,
          EdLabels.User_Profile_Password_Field_Label,
          this.SessionObjects.AdminUserProfile.Password,
          80 );
        groupField.Layout = EuPageGenerator.ApplicationFieldLayout;

        groupField.Description =  EdLabels.User_Profile_Password_Field_Description );
      }
       */

      // 
      // Create the customer name object
      // 
      if ( this.AdapterObjects.Settings.hasHiddenUserProfileField ( EdUserProfile.FieldNames.Image_File_Name ) == false )
      {
        this.LogDebug ( "ImageFileName {0}.", this.Session.AdminUserProfile.ImageFileName );
        this.Session.AdminUserProfile.CurrentImageFileName = this.Session.AdminUserProfile.ImageFileName;

        groupField = pageGroup.createImageField (
          Evado.Digital.Model.EdUserProfile.FieldNames.Image_File_Name,
          EdLabels.UserProfile_ImageFileame_Field_Label,
          this.Session.AdminUserProfile.ImageFileName,
          300,
          300 );
        groupField.Layout = EuAdapter.DefaultFieldLayout;

        //
        // copy the user image file if it exists.
        //
        if ( this.Session.AdminUserProfile.ImageFileName != String.Empty )
        {
          try
          {
            String stTargetPath = this.UniForm_BinaryFilePath + this.Session.AdminUserProfile.ImageFileName;
            String stImagePath = this.UniForm_ImageFilePath + this.Session.AdminUserProfile.ImageFileName;

            this.LogDebug ( "Target path {0}.", stTargetPath );
            this.LogDebug ( "Image path {0}.", stImagePath );

            //
            // copy the file into the image directory.
            //
            System.IO.File.Copy ( stImagePath, stTargetPath, true );
          }
          catch ( Exception Ex )
          {
            this.LogException ( Ex );
          }
        }
      }
      else
      {
        this.Session.AdminUserProfile.ImageFileName = String.Empty;
      }

      //
      // add the user tilte field
      //
      if ( this.AdapterObjects.Settings.hasHiddenUserProfileField ( EdUserProfile.FieldNames.Title ) == false )
      {
        groupField = pageGroup.createTextField (
           Evado.Digital.Model.EdUserProfile.FieldNames.Title,
          EdLabels.UserProfile_Title_Field_Label,
          this.Session.AdminUserProfile.Title, 50 );
        groupField.Layout = EuAdapter.DefaultFieldLayout;
      }

      // 
      // Create the  name object
      // 
      this.LogValue ( "Given Name:" + this.Session.AdminUserProfile.GivenName );
      this.LogValue ( "Family Name:" + this.Session.AdminUserProfile.FamilyName );

      if ( this.AdapterObjects.Settings.hasHiddenUserProfileField ( EdUserProfile.FieldNames.Prefix ) == false )
      {
        groupField = pageGroup.createTextField (
           Evado.Digital.Model.EdUserProfile.FieldNames.Prefix,
          EdLabels.UserProfile_Prefix_Field_Label,
          this.Session.AdminUserProfile.Prefix, 10 );
        groupField.Layout = EuAdapter.DefaultFieldLayout;
      }

      bool userFamilyName = false;
      if ( this.AdapterObjects.Settings.hasHiddenUserProfileField ( EdUserProfile.FieldNames.Given_Name ) == false )
      {
        groupField = pageGroup.createTextField (
           Evado.Digital.Model.EdUserProfile.FieldNames.Given_Name,
          EdLabels.UserProfile_GivenName_Field_Label,
          this.Session.AdminUserProfile.GivenName, 50 );
        groupField.Layout = EuAdapter.DefaultFieldLayout;
        groupField.setBackgroundColor (
          Evado.UniForm.Model.EuFieldParameters.BG_Mandatory,
          Evado.UniForm.Model.EuBackgroundColours.Red );

        userFamilyName = true;
      }
      else
      {
        this.Session.AdminUserProfile.GivenName = this.Session.AdminUserProfile.UserId;
        userFamilyName = false;
      }

      if ( this.AdapterObjects.Settings.hasHiddenUserProfileField ( EdUserProfile.FieldNames.Family_Name ) == false )
      {
        groupField = pageGroup.createTextField (
           Evado.Digital.Model.EdUserProfile.FieldNames.Family_Name,
          EdLabels.UserProfile_FamilyName_Field_Label,
          this.Session.AdminUserProfile.FamilyName, 50 );
        groupField.Layout = EuAdapter.DefaultFieldLayout;
        groupField.Mandatory = true;
        groupField.setBackgroundColor (
          Evado.UniForm.Model.EuFieldParameters.BG_Mandatory,
          Evado.UniForm.Model.EuBackgroundColours.Red );

        userFamilyName = true;
      }
      else
      {
        this.Session.AdminUserProfile.FamilyName = this.Session.AdminUserProfile.UserId;
        userFamilyName = false;
      }

      // 
      // Create the comon name object
      // 
      groupField = pageGroup.createTextField (
         Evado.Digital.Model.EdUserProfile.FieldNames.CommonName,
        EdLabels.UserProfile_CommonName_Field_Label,
        String.Empty,
        this.Session.AdminUserProfile.CommonName,
        80 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      if ( userFamilyName == true )
      {
        groupField.EditAccess = Evado.UniForm.Model.EuEditAccess.Disabled;
      }
      else
      {
        groupField.Mandatory = true;
        groupField.setBackgroundColor (
          Evado.UniForm.Model.EuFieldParameters.BG_Mandatory,
          Evado.UniForm.Model.EuBackgroundColours.Red );
      }

      //
      // define the user address field.
      //
      #region User Address
      if ( this.AdapterObjects.Settings.EnableUserAddressUpdate == true )
      {
        if ( this.AdapterObjects.Settings.hasHiddenUserProfileField ( EdUserProfile.FieldNames.Address_1 ) == false )
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
          groupField.Layout = EuAdapter.DefaultFieldLayout;

          this.LogDebug ( "AddresS:" + groupField.Value );
        }
      }

      #endregion

      // 
      // Create the user's email address object
      // 
      groupField = pageGroup.createEmailAddressField (
         Evado.Digital.Model.EdUserProfile.FieldNames.Email_Address.ToString ( ),
        EdLabels.UserProfile_Email_Field_Label,
        this.Session.AdminUserProfile.EmailAddress.Address );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      // 
      // Create the user role id object
      // 
      this.LogValue ( "AdminUserProfile.RoleId: "
        + this.Session.AdminUserProfile.Roles );

      //
      // Generate the user role list.
      //
      optionList = this.AdapterObjects.Settings.GetRoleOptionList ( false );

      //
      // Generate the user role radio button list field object.
      //
      groupField = pageGroup.createCheckBoxListField (
         Evado.Digital.Model.EdUserProfile.FieldNames.RoleId.ToString ( ),
        EdLabels.UserProfile_Role_Field_Label,
        EdLabels.UserProfile_Role_Field_Description,
        this.Session.AdminUserProfile.Roles,
        optionList );
      groupField.Layout = EuAdapter.DefaultFieldLayout;
      groupField.Mandatory = true;
      groupField.setBackgroundColor (
        Evado.UniForm.Model.EuFieldParameters.BG_Mandatory,
        Evado.UniForm.Model.EuBackgroundColours.Red );

      //
      // get the list of organisations.
      //
      String userSelectionList = this.AdapterObjects.Settings.UserCategoryList;

      optionList = this.AdapterObjects.getSelectionOptions ( userSelectionList, String.Empty, false, true );

      if ( this.Session.UserProfile.hasAdministrationAccess == true )
      {
        optionList.Add ( new EvOption ( EdUserProfile.CONST_USER_TYPE_EVADO ) );
        optionList.Add ( new EvOption ( EdUserProfile.CONST_USER_TYPE_CUSTOMER ) );
      }
      EvStatics.sortOptionListValues ( optionList );

      // 
      // Set the selection to the current site org id.
      // 
      groupField = pageGroup.createSelectionListField (
        EdUserProfile.FieldNames.User_Type,
        EdLabels.UserProfile_User_Type_Field_Label,
        this.Session.AdminUserProfile.UserType,
        optionList );
      groupField.Layout = EuAdapter.DefaultFieldLayout;
      groupField.Mandatory = true;
      groupField.setBackgroundColor (
        Evado.UniForm.Model.EuFieldParameters.BG_Mandatory,
        Evado.UniForm.Model.EuBackgroundColours.Red );

      if ( this.Session.UserProfile.hasEvadoAdministrationAccess )
      {
        groupField = pageGroup.createTextField (
           Evado.Digital.Model.EdUserProfile.FieldNames.Expiry_Date.ToString ( ),
          EdLabels.UserProfile_Expiry_Date_Field_Label,
          EvStatics.getDateAsString ( this.Session.AdminUserProfile.ExpiryDate ),
          15 );
        groupField.Layout = EuAdapter.DefaultFieldLayout;
      }

    }//END getDataObject_FieldGroup Method


    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">Evado.UniForm.Model.EuPage object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_GroupCommands (
      Evado.UniForm.Model.EuGroup PageGroup )
    {
      this.LogMethod ( "getDataObject_GroupCommands" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.EuCommand groupCommand = new Evado.UniForm.Model.EuCommand ( );

      // 
      // Add the save groupCommand
      // 
      groupCommand = PageGroup.addCommand (
        EdLabels.User_Profile_Save_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Users.ToString ( ),
        Evado.UniForm.Model.EuMethods.Save_Object );

      // 
      // Define the save and delete groupCommand parameters
      // 
      groupCommand.SetGuid (
        this.Session.UserProfile.Guid );


      //
      // Add the delete groupCommand object.
      //
      if ( this.Session.UserProfile.Guid != Guid.Empty
        || ( this.Session.UserProfile.OrgId == EdAdapterSettings.EVADO_ORGANISATION
          && this.Session.AdminUserProfile.OrgId == EdAdapterSettings.EVADO_ORGANISATION ) )
      {
        groupCommand = PageGroup.addCommand (
           EdLabels.User_Profile_Delete_Command_Title,
           EuAdapter.ADAPTER_ID,
           EuAdapterClasses.Users.ToString ( ),
           Evado.UniForm.Model.EuMethods.Save_Object );

        // 
        // Define the save and delete groupCommand parameters
        // 
        groupCommand.SetGuid (
          this.Session.UserProfile.Guid );
        groupCommand.AddParameter (
           Evado.Digital.Model.EvcStatics.CONST_SAVE_ACTION,
          EuUserProfiles.CONST_DELETE_ACTION );
      }

      this.LogMethodEnd ( "getDataObject_GroupCommands" );
    }//END getDataObject_GroupCommands method.

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.EuCommand object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.UniForm.Model.EuAppData getObject_EmailPage (
      Evado.UniForm.Model.EuCommand PageCommand )
    {
      this.LogMethod ( "getObject_EmailPage" );
      // 
      // Initialise the methods variables and objects.
      // 
      Guid subjectGuid = Guid.Empty;
      Evado.UniForm.Model.EuAppData clientDataObject = new Evado.UniForm.Model.EuAppData ( );
      Evado.UniForm.Model.EuField groupField = new Evado.UniForm.Model.EuField ( );
      Evado.UniForm.Model.EuGroup pageGroup = new Evado.UniForm.Model.EuGroup ( );
      Evado.UniForm.Model.EuCommand groupCommand = new Evado.UniForm.Model.EuCommand ( );
      String userId = PageCommand.GetParameter ( EdUserProfile.FieldNames.UserId );
      this.LogDebug ( "PageCommand userId {0}. ", userId );

      // 
      // Log access to page.
      // 
      this.LogPageAccess (
          this.ClassNameSpace + "getObject_EmailPage",
        this.Session.UserProfile );

      //
      // set the client ResultData object properties
      //
      clientDataObject.Id = Guid.NewGuid();
      clientDataObject.Page.Id = this.Session.AdminUserProfile.Guid;
      clientDataObject.Title = EdLabels.UserProfile_Email_Page_Title;
      clientDataObject.Page.EditAccess = Evado.UniForm.Model.EuEditAccess.Enabled;

      try
      {
        // 
        // Retrieve the customer object from the database via the DAL and BLL layers.
        // 
       this.Session.AdminUserProfile = this._Bll_UserProfiles.getItem ( userId );

       this.LogDebugClass ( this._Bll_UserProfiles.Log );

       if ( this.Session.AdminUserProfile.Guid == Guid.Empty
         || this.Session.AdminUserProfile.EmailAddress.Address == String.Empty )
        {
          this.LogValue ( "userProfile or email address is empty." );

          this.LogMethodEnd ( "getObject_EmailPage" );
          return this.Session.LastPage; ;
        }

        this.LogClass ( this._Bll_UserProfiles.Log );

        this.LogDebug ( "AdminUserProfile.Guid {0}. ", this.Session.AdminUserProfile.Guid );
        this.LogDebug ( "AdminUserProfile.UserId {0}.", this.Session.AdminUserProfile.UserId );

        clientDataObject.Page.Title = clientDataObject.Title;

        clientDataObject.Title = String.Format ( EdLabels.UserProfile_Email_Page_Title1,
           this.Session.AdminUserProfile.CommonName );

        // 
        // create the page pageMenuGroup
        // 
        pageGroup = clientDataObject.Page.AddGroup (
           String.Empty,
           Evado.UniForm.Model.EuEditAccess.Enabled );
        pageGroup.Layout = Evado.UniForm.Model.EuGroupLayouts.Full_Width;
        pageGroup.EditAccess = Evado.UniForm.Model.EuEditAccess.Enabled;

        //
        // Add the subject field.
        //
        groupField = pageGroup.createTextField (
          EuUserProfiles.CONST_EMAIL_SUBJECT,
          EdLabels.UserProfile_Email_Subject_Field_Title,
          String.Empty, 100 );
        groupField.Layout = Evado.UniForm.Model.EuFieldLayoutCodes.Column_Layout;

        //
        // Add the body field.
        //
        groupField = pageGroup.createFreeTextField (
          EuUserProfiles.CONST_EMAIL_BODY,
          EdLabels.UserProfile_Email_Body_Field_Title,
          String.Empty, 50, 5 );
        groupField.Layout = Evado.UniForm.Model.EuFieldLayoutCodes.Column_Layout;


        groupCommand =pageGroup.addCommand (
           EdLabels.UserProfile_Send_Email_Command_Title,
           EuAdapter.ADAPTER_ID,
           EuAdapterClasses.Users.ToString ( ),
           Evado.UniForm.Model.EuMethods.Save_Object );

        // 
        // Define the save and delete groupCommand parameters
        // 
        groupCommand.SetPageId ( PageCommand.GetPageId ( ) );


        this.LogMethodEnd ( "getObject_EmailPage" );
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

      this.LogMethodEnd ( "getObject_EmailPage" );
      return this.Session.LastPage; ;

    }//END getObject_EmailPage method

    #region Create User methods
    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.EuCommand object.</param>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.UniForm.Model.EuAppData createObject (
      Evado.UniForm.Model.EuCommand PageCommand )
    {
      this.LogValue ( Evado.UniForm.Model.EuStatics.CONST_METHOD_START
        + this.ClassNameSpace + "createObject" );
      this.LogValue ( "AdminOrganisation.OrgId: " + this.Session.SelectedUserType.ToString ( ) );
      try
      {
        // 
        // Initialise the methods variables and objects.
        //      
        Evado.UniForm.Model.EuAppData clientDataObject = new Evado.UniForm.Model.EuAppData ( );

        //
        // Determine if the user has access to this page and log and error if they do not.
        //
        if ( this.Session.UserProfile.hasManagementAccess == false )
        {
          this.LogIllegalAccess (
            this.ClassNameSpace + "createObject",
            this.Session.UserProfile );

          this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

          return this.Session.LastPage; ;
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
        this.Session.AdminUserProfile = new Evado.Digital.Model.EdUserProfile ( );
        this.Session.AdminUserProfile.Guid = Evado.Digital.Model.EvcStatics.CONST_NEW_OBJECT_ID;
        this.Session.AdminUserProfile.ExpiryDate = EvStatics.getDateTime ( "1 JAN 2100" );
        this.Session.AdminUserProfile.UserType = String.Empty;
        this.Session.AdminUserProfile.UserCategory = String.Empty;


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

      return this.Session.LastPage; ;

    }//END method


    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Import user profile methods


    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.ClientClientDataObjectEvado.UniForm.Model.EuCommand object.</param>
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

      this.LogValue ( "Files.DebugLog: " + Evado.Model.EvStatics.Files.Log );

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
        this.LogValue ( "AdminUserProfile: " + this.Session.AdminUserProfile.getUserProfile ( ) );


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
      EdUserProfile.FieldNames field = EdUserProfile.FieldNames.Null;

      //
      // Iterate through the columns filling the values into the uploaded user profile.
      //
      for ( int columnCount = 0; columnCount < Columns.Count && columnCount < DataRow.Count; columnCount++ )
      {
        String value = Columns [ columnCount ];

        this.LogValue ( "Column: " + value + ", Value: " + DataRow [ columnCount ] );

        if ( EvStatics.tryParseEnumValue<EdUserProfile.FieldNames> (
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
      if ( userProfile.UserType != uploadedUserProfile.UserType )
      {
        userProfile.UserType = uploadedUserProfile.UserType;
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

    #region Send Email methods.
    // ===================================================================================
    /// THis method emails the user message
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.EuCommand object.</param>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.UniForm.Model.EuAppData sendEmail (
      Evado.UniForm.Model.EuCommand PageCommand )
    {
      this.LogMethod ( "sendEmail" );
      this.LogDebug ( "Parameter: " + PageCommand.getAsString ( false, true ) );

      this.LogDebug ( "eClinical.AdminUserProfile:" );
      this.LogDebug ( "Guid: " + this.Session.AdminUserProfile.Guid );
      this.LogDebug ( "UserId: " + this.Session.AdminUserProfile.UserId );
      this.LogDebug ( "CommonName: " + this.Session.AdminUserProfile.CommonName );

      try
      {
        // 
        // Initialise the methods variables and objects.
        //      
        Evado.UniForm.Model.EuAppData clientDataObject = new Evado.UniForm.Model.EuAppData ( );
        string EmailTitle = String.Empty;
        string EmailBody = String.Empty;
        EvEmail email = new EvEmail ( );
        EvEmail.EmailStatus emailStatus = EvEmail.EmailStatus.Null;
        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "sendEmail",
          this.Session.UserProfile );

        this.LogMethodEnd ( "sendEmail" );
        this.LogDebug ( "Sender email address : " + this.Session.UserProfile.EmailAddress );
        this.LogDebug ( "Receiver email address : " + this.Session.AdminUserProfile.EmailAddress );
        this.LogDebug ( "EmailTitle: " + EmailTitle );
        this.LogDebug ( "EmailBody: " + EmailBody );

        EmailTitle = PageCommand.GetParameter ( EuUserProfiles.CONST_EMAIL_SUBJECT );
        EmailBody = PageCommand.GetParameter ( EuUserProfiles.CONST_EMAIL_BODY );

        EmailBody += "</br></br>" + String.Format ( "From user {0} - {1} at {2} ", 
          this.Session.UserProfile.EmailAddress, 
          this.Session.UserProfile.CommonName,
          DateTime.Now.ToString( "dd-MMM-yyy HH:mm" ));
        //
        // Initialise the report alert class
        //
        email.SmtpServer = this.AdapterObjects.Settings.SmtpServer;
        email.SmtpServerPort = this.AdapterObjects.Settings.SmtpServerPort;
        email.SmtpUserId = this.AdapterObjects.Settings.SmtpUserId;
        email.SmtpPassword = this.AdapterObjects.Settings.SmtpPassword;

        //
        // Set the email alert to the recipents
        //
        emailStatus = email.sendEmail (
          EmailTitle,
          EmailBody,
          this.Session.UserProfile.EmailAddress,
          this.Session.AdminUserProfile.EmailAddress,
          String.Empty );

        this.LogValue ( "Email DebugLog: " + email.Log );

        //
        // Log email send error.
        //
        if ( emailStatus != EvEmail.EmailStatus.Email_Sent )
        {
          this.LogError ( EvEventCodes.Email_Send,
            "User Email Event Status: " + emailStatus
            + "\r\n" + email.Log );

          this.LogClass ( email.Log );
        }

        this.Session.AdminUserProfile = new EdUserProfile ( );

        return new Evado.UniForm.Model.EuAppData ( );

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
      this.Session.AdminUserProfile = new EdUserProfile ( );
      this.LogMethodEnd ( "sendEmail" );
      return this.Session.LastPage; ;

    }//END method


    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Update User methods
    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.ClientClientDataObjectEvado.UniForm.Model.EuCommand object.</param>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.UniForm.Model.EuAppData updateObject ( Evado.UniForm.Model.EuCommand PageCommand )
    {
      this.LogMethod ( "updateObject" );
      this.LogValue ( "Parameter: " + PageCommand.getAsString ( false, false ) );

      this.LogValue ( "Session.AdminUserProfile:" );
      this.LogValue ( "Guid: " + this.Session.AdminUserProfile.Guid );
      this.LogValue ( "UserId: " + this.Session.AdminUserProfile.UserId );
      this.LogValue ( "CommonName: " + this.Session.AdminUserProfile.CommonName );

      try
      {
        // 
        // Initialise the methods variables and objects.
        //      
        Evado.UniForm.Model.EuAppData clientDataObject = new Evado.UniForm.Model.EuAppData ( );
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

        this.LogValue ( "AdminUserProfile.CommonName: {0}. ", this.Session.AdminUserProfile.CommonName );

        //
        // get the user category for the currently selected user type selection.
        //
        if ( this.Session.AdminUserProfile.UserType == EdUserProfile.CONST_USER_TYPE_EVADO
          || this.Session.AdminUserProfile.UserType == EdUserProfile.CONST_USER_TYPE_CUSTOMER )
        {
          this.Session.AdminUserProfile.UserCategory = this.Session.AdminUserProfile.UserType;
        }
        else
        {
          this.Session.AdminUserProfile.UserCategory = this.AdapterObjects.getSelectionCategory (
            this.AdapterObjects.Settings.UserCategoryList,
            this.Session.AdminUserProfile.UserType );
        }
        this.LogValue ( "AdminUserProfile.UserType: {0}. ", this.Session.AdminUserProfile.UserType );
        this.LogValue ( "AdminUserProfile.UserCategory: {0}. ", this.Session.AdminUserProfile.UserCategory );

        this.Session.AdminUserProfile.UserId = EvStatics.CleanSamUserId (
          this.Session.AdminUserProfile.UserId );

        //
        // Update the address field.
        //
        this.updateAddressValue ( PageCommand );

        //
        // save the image file if it exists.
        //
        this.saveImageFile ( );

        this.Session.AdminUserProfile.setNames ( );
        this.LogValue ( "AdminUserProfile.CommonName: {0}. ", this.Session.AdminUserProfile.CommonName );

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
        String stSaveAction = PageCommand.GetParameter ( Evado.Digital.Model.EvcStatics.CONST_SAVE_ACTION );

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

        return new Evado.UniForm.Model.EuAppData ( );

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
    /// THis method copies the upload image file to the image directory.
    /// </summary>
    //  ----------------------------------------------------------------------------------
    private void saveImageFile ( )
    {
      this.LogMethod ( "saveImageFile" );

      if ( this.Session.AdminUserProfile.ImageFileName == null )
      {
        return;
      }

      if ( this.Session.AdminUserProfile.ImageFileName == String.Empty )
      {
        return;
      }

      if ( this.Session.AdminUserProfile.CurrentImageFileName == this.Session.AdminUserProfile.ImageFileName )
      {
        return;
      }


      //
      // Initialise the method variables and objects.
      //
      String stSourcePath = this.UniForm_BinaryFilePath + this.Session.AdminUserProfile.ImageFileName;
      String stImagePath = this.UniForm_ImageFilePath + this.Session.AdminUserProfile.ImageFileName;

      this.LogDebug ( "Source path {0}.", stSourcePath );
      this.LogDebug ( "Image path {0}.", stImagePath );

      //
      // Save the file to the directory repository.
      //
      try
      {
        System.IO.File.Copy ( stSourcePath, stImagePath, true );
      }
      catch ( Exception Ex )
      {
        this.LogException ( Ex );
      }
      this.LogMethodEnd ( "saveImageFile" );
    }//END saveImageFile method

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
    /// <param name="PageCommand">Evado.UniForm.Model.EuCommand updated.</param>
    /// <returns></returns>
    //  ----------------------------------------------------------------------------------
    private void updateAddressValue (
      Evado.UniForm.Model.EuCommand PageCommand )
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
      Evado.UniForm.Model.EuCommand PageCommand )
    {
      this.LogMethod ( "updateObjectValue" );
      this.LogValue ( "Parameters.Count: " + PageCommand.Parameters.Count );
      this.LogValue ( "AdminUser.Guid: " + this.Session.AdminUserProfile.Guid );

      // 
      // Iterate through the parameter values updating the ResultData object
      // 
      foreach ( Evado.UniForm.Model.EuParameter parameter in PageCommand.Parameters )
      {
        this.LogTextStart ( parameter.Name + " = " + parameter.Value );

        if ( parameter.Name.Contains ( Evado.Digital.Model.EvcStatics.CONST_GUID_IDENTIFIER ) == false
          && parameter.Name != Evado.UniForm.Model.EuCommandParameters.Custom_Method.ToString ( )
          && parameter.Name != Evado.UniForm.Model.EuCommandParameters.Page_Id.ToString ( )
          && parameter.Name != Evado.Digital.Model.EvcStatics.CONST_SAVE_ACTION
          && parameter.Name != EuUserProfiles.CONST_ADDRESS_FIELD_ID
          && parameter.Name != EuUserProfiles.CONST_CURRENT_FIELD_ID
          && parameter.Name != EuUserProfiles.CONST_NEW_PASSWORD_PARAMETER )
        {
          this.LogTextEnd ( " >> UPDATED" );
          try
          {
            Evado.Digital.Model.EdUserProfile.FieldNames fieldName =
              Evado.Model.EvStatics.parseEnumValue<Evado.Digital.Model.EdUserProfile.FieldNames> (
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