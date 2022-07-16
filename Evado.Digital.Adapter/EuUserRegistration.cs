/***************************************************************************************
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
  public partial class EuUserRegistration : EuClassAdapterBase
  {
    #region Class Initialisation
    /// <summary>
    /// This method initialises the class.
    /// </summary>
    public EuUserRegistration ( )
    {
      this.ClassNameSpace = "Evado.UniForm.Clinical.EuDemoUserRegistration.";
    }

    /// <summary>
    /// This method initialises the class and passs in the user profile.
    /// </summary>
    public EuUserRegistration (
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

      this.LoggingLevel = Settings.LoggingLevel;

      this.ClassNameSpace = "Evado.UniForm.Clinical.EuDemoUserRegistration.";
      this.LogInitMethod ( "EuDemoUserRegistration initialisation" );
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
    private const String CONST_DEMO_USER_PREFIX = "DM";

    public const String CONST_DEMO_ORGANISATION = "DEMO01";

    private Evado.Digital.Bll.EdUserprofiles _Bll_UserProfiles = new Evado.Digital.Bll.EdUserprofiles ( );

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

        var pageId = PageCommand.GetPageId<Evado.Digital.Model.EdStaticPageIds> ( );

        this.LogDebug ( "Page Id {0}.", pageId );
        //
        // Load the customer group.
        //
        this.getAdsCustomerGroup ( );

        // 
        // Determine the method to be called
        // 
        switch ( PageCommand.Method )
        {
          case Evado.UniForm.Model.EuMethods.List_of_Objects:
          case Evado.UniForm.Model.EuMethods.Get_Object:
          case Evado.UniForm.Model.EuMethods.Create_Object:
          case Evado.UniForm.Model.EuMethods.Save_Object:
          case Evado.UniForm.Model.EuMethods.Delete_Object:
            {
              switch ( pageId )
              {
                case Evado.Digital.Model.EdStaticPageIds.User_Registration_Page:
                default:
                  {
                    clientDataObject = this.getDataObject_RegistrationPage ( PageCommand );
                    break;
                  }
                case Evado.Digital.Model.EdStaticPageIds.Demo_User_Exit_Page:
                  {
                    this.LogValue ( " Save Object method" );

                    clientDataObject = this.getDataObject_ExitPage ( PageCommand );
                    break;
                  }
              }
              break;
            }
        }//END Switch

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

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Create User methods

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.EuCommand object.</param>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.UniForm.Model.EuAppData getDataObject_RegistrationPage (
      Evado.UniForm.Model.EuCommand PageCommand )
    {
      this.LogValue ( Evado.UniForm.Model.EuStatics.CONST_METHOD_START
        + this.ClassNameSpace + "getDataObject_RegistrationPage" );
      this.LogDebug ( "AdminOrganisation.OrgId: " +this.Session.SelectedUserType.ToString() );
      this.LogDebug ( "Demo Expiry {0}", this.AdapterObjects.Settings.DemoAccountExpiryDays.ToString() );
      try
      {
        // 
        // Initialise the methods variables and objects.
        //      
        Evado.UniForm.Model.EuAppData clientDataObject = new Evado.UniForm.Model.EuAppData ( );

        // 
        // Create the milestone object and add it to the clinical session object.
        // 
        this.Session.AdminUserProfile = new Evado.Digital.Model.EdUserProfile ( );
        this.Session.AdminUserProfile.Guid = Evado.Digital.Model.EvcStatics.CONST_NEW_OBJECT_ID;
        this.Session.AdminUserProfile.ExpiryDate = DateTime.Now.AddDays (
          this.AdapterObjects.Settings.DemoAccountExpiryDays );
        this.Session.AdminUserProfile.UserId = this.createDemoUderId ( );
        this.Session.AdminUserProfile.FamilyName = this.Session.AdminUserProfile.UserId;
        this.Session.AdminUserProfile.GivenName = this.Session.AdminUserProfile.UserId;
        this.Session.AdminUserProfile.UserType = "End_User";

        this.LogDebug ( "AdminUserProfile.UserId: {0} ", this.Session.AdminUserProfile.UserId );
        this.LogDebug ( "AdminUserProfile.ExpiryDate: {0} ", this.Session.AdminUserProfile.ExpiryDate );

        this.getDataObject_RegistrationPage ( clientDataObject );

        this.LogMethodEnd ( "getDataObject_RegistrationPage" );

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

      this.LogMethodEnd ( "getDataObject_RegistrationPage" );
      return this.Session.LastPage; ;

    }//END method

    // ==================================================================================
    /// <summary>
    /// THis method creates the demonstration user identifier.
    /// </summary>
    /// <returns>Stringt</returns>
    //  ----------------------------------------------------------------------------------
    private String createDemoUderId ( )
    {
      //
      // Initialise the methods variables and objects.
      //
      String userId = String.Empty;

      //
      // get the list of users.
      //
      int userCount = this._Bll_UserProfiles.UserCount (String.Empty );
      userCount++;

      userId = EuUserRegistration.CONST_DEMO_USER_PREFIX + userCount.ToString ( "##000" );

      return userId;
    }

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="ClientDataObject">Evado.UniForm.Model.EuAppData object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_RegistrationPage (
      Evado.UniForm.Model.EuAppData ClientDataObject )
    {
      this.LogMethod ( "getDataObject_RegistrationPage" );
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


      this.getDataObject_Debug_Group ( ClientDataObject.Page );

      //
      // Display the instructions 
      //
      this.getDataObject_InstructionsGroup ( ClientDataObject.Page );

      //
      // Add the page commands to the page.
      //
      this.getDataObject_PageCommands ( ClientDataObject.Page );

      //
      // Add the field pageMenuGroup to the page.
      //
      this.getDataObject_FieldGroup ( ClientDataObject.Page );

      this.LogMethodEnd ( "getDataObject_RegistrationPage" );

    }//END getDataObject_RegistrationPage Method


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
        EdLabels.Demo_Registration_Save_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.User_Registration.ToString ( ),
        Evado.UniForm.Model.EuMethods.Custom_Method );

      pageCommand.setCustomMethod ( Evado.UniForm.Model.EuMethods.Get_Object );

      pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Demo_User_Exit_Page );


    }//END getclientDataObject_Commands Method

    // ==============================================================================
    /// <summary>
    /// This method creates the demonstration user registration instructions.
    /// </summary>
    /// <param name="Page">Evado.UniForm.Model.EuPage object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_InstructionsGroup (
      Evado.UniForm.Model.EuPage Page )
    {
      this.LogMethod ( "getDataObject_InstructionsGroup" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.EuGroup pageGroup = new Evado.UniForm.Model.EuGroup ( );

      // 
      // create the page pageMenuGroup
      // 
      pageGroup = Page.AddGroup (
         String.Empty,
         Evado.UniForm.Model.EuEditAccess.Disabled );
      pageGroup.Layout = Evado.UniForm.Model.EuGroupLayouts.Full_Width;

      //
      // Define the markdown options.
      //
      MarkdownSharp.MarkdownOptions markDownOptions = new MarkdownSharp.MarkdownOptions ( );
      markDownOptions.AutoHyperlink = true;
      markDownOptions.AutoNewlines = true;
      markDownOptions.EmptyElementSuffix = "/>";
      markDownOptions.EncodeProblemUrlCharacters = true;
      markDownOptions.LinkEmails = true;
      markDownOptions.StrictBoldItalic = true;

      //
      // Initialise the markdown class
      //
      MarkdownSharp.Markdown markDown = new MarkdownSharp.Markdown ( markDownOptions );

      //
      // perform the html body transformation.
      //
      string description = markDown.Transform ( this.AdapterObjects.ContentTemplates.DemoRegistrationInstuctions );

      pageGroup.Description = description;

      //
      // Add a streamed video if exists.
      //
      this.getStreamedVideoField ( pageGroup );

      this.LogMethodEnd ( "getDataObject_InstructionsGroup" );

    }

    //  =================================================================================
    /// <summary>
    ///   This method generates the streamed video form field object as html markup.
    /// </summary>
    /// <param name="GroupField">Evado.UniForm.Model.EuPageGroup object.</param>
    //  ---------------------------------------------------------------------------------
    private void getStreamedVideoField (
      Evado.UniForm.Model.EuGroup PageGroup )
    {
      this.LogMethod ( "getStreamedVideoField" );

      //
      // if there is not video donot create the video field.
      //
      if ( this.AdapterObjects.Settings.DemoRegistrationVideoUrl == String.Empty )
      {
        return;
      }

      // 
      // Initialise local variables.
      // 
      Evado.UniForm.Model.EuField groupField = PageGroup.createField ( );
      groupField.Type = Evado.Model.EvDataTypes.Streamed_Video;
      groupField.Value = this.AdapterObjects.Settings.DemoRegistrationVideoUrl;
      groupField.Description = String.Empty;

      int iWidth = 800;
      int iHeight = 0;

      this.LogDebug ( "iWidth: " + iWidth );
      this.LogDebug ( "iHeight: " + iHeight );
      if ( iWidth > 0 )
      {
        groupField.AddParameter ( Evado.UniForm.Model.EuFieldParameters.Width, iWidth.ToString ( ) );
      }
      if ( iHeight > 0 )
      {
        groupField.AddParameter ( Evado.UniForm.Model.EuFieldParameters.Height, iHeight.ToString ( ) );
      }

      this.LogDebug ( "Value: " + groupField.Value );

      return;

    }//END getStreamedVideoField method.

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
      // Create the user id object
      // 
      groupField = pageGroup.createTextField (
         Evado.Digital.Model.EdUserProfile.FieldNames.UserId.ToString ( ),
        EdLabels.User_Profile_Identifier_Field_Label,
        this.Session.AdminUserProfile.UserId,
        80 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;
      groupField.EditAccess = Evado.UniForm.Model.EuEditAccess.Disabled;


      // 
      // Create the comon name object
      // 
      groupField = pageGroup.createTextField (
         Evado.Digital.Model.EdUserProfile.FieldNames.CommonName,
        EdLabels.Dem_Registration_CommonName_Field_Label,
        this.Session.AdminUserProfile.CommonName,
        80 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;
      groupField.Mandatory = true;

      // 
      // Create the user's email address object
      // 
      groupField = pageGroup.createTelephoneNumberField (
         Evado.Digital.Model.EdUserProfile.FieldNames.Telephone.ToString ( ),
        EdLabels.UserProfile_Telephone_Field_Label,
        this.Session.AdminUserProfile.Telephone );
      groupField.Layout = EuAdapter.DefaultFieldLayout;
      groupField.Mandatory = true;

      // 
      // Create the user's email address object
      // 
      groupField = pageGroup.createEmailAddressField (
         Evado.Digital.Model.EdUserProfile.FieldNames.Email_Address.ToString ( ),
        EdLabels.UserProfile_Email_Field_Label,
        this.Session.AdminUserProfile.EmailAddress.Address );
      groupField.Layout = EuAdapter.DefaultFieldLayout;
      groupField.Mandatory = true;
      groupField.setBackgroundColor (
        Evado.UniForm.Model.EuFieldParameters.BG_Mandatory,
        Evado.UniForm.Model.EuBackgroundColours.Red );

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
        EdLabels.Demo_Registration_Save_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.User_Registration.ToString ( ),
        Evado.UniForm.Model.EuMethods.Custom_Method );

      groupCommand.setCustomMethod ( Evado.UniForm.Model.EuMethods.Get_Object );

      groupCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Demo_User_Exit_Page );

      this.LogMethodEnd ( "getDataObject_GroupCommands" );
    }//END getDataObject_GroupCommands method.

    // ==============================================================================
    /// <summary>
    /// This method outputs a debug group in the registration page.
    /// </summary>
    /// <param name="Page"> Evado.UniForm.Model.EuPage object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_Debug_Group (
      Evado.UniForm.Model.EuPage PageObject )
    {
      if ( this.LoggingLevel < 6 )
      {
        return;
      }

      var pageGroup = PageObject.AddGroup ( "", Evado.UniForm.Model.EuEditAccess.Disabled );

      System.Text.StringBuilder description = new StringBuilder ( );

      description.AppendLine ( "Demonstration User details: " );
      description.AppendFormat ( "UserId: {0} \r\n", this.Session.AdminUserProfile.UserId );
      description.AppendFormat ( "GivenName: {0} \r\n", this.Session.AdminUserProfile.GivenName );
      description.AppendFormat ( "FamilyName: {0} \r\n", this.Session.AdminUserProfile.FamilyName );
      description.AppendFormat ( "RoleId: {0} \r\n", this.Session.AdminUserProfile.Roles );

      // 
      // Create the  name object
      // 
      this.LogValue ( "Given Name:" + this.Session.AdminUserProfile.GivenName );
      this.LogValue ( "Family Name:" + this.Session.AdminUserProfile.FamilyName );



      this.LogDebug ( "Group Description:\r\n " + description.ToString ( ) );

      pageGroup.Description = description.ToString ( );

    }

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
    private Evado.UniForm.Model.EuAppData getDataObject_ExitPage (
      Evado.UniForm.Model.EuCommand PageCommand )
    {
      this.LogMethod ( "getDataObject_ExitPage" );
      this.LogValue ( "Parameter: " + PageCommand.getAsString ( false, false ) );
      // 
      // Initialise the methods variables and objects.
      //      
      Evado.UniForm.Model.EuAppData clientDataObject = new Evado.UniForm.Model.EuAppData ( );
      EvEventCodes result;

      //
      // register the user details.
      //
      result = this.RegisterUserDetails ( PageCommand );

      //
      // create the registration exit page.

      clientDataObject = this.GetUserRegistrationExitPage ( result );

      this.LogMethodEnd ( "getDataObject_ExitPage" );
      return clientDataObject;

    }//END getDataObject_ExitPage method

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.ClientClientDataObjectEvado.UniForm.Model.EuCommand object.</param>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private EvEventCodes RegisterUserDetails (
      Evado.UniForm.Model.EuCommand PageCommand )
    {
      this.LogMethod ( "RegisterUserDetails" );
      try
      {
        // 
        // Initialise the methods variables and objects.
        //      
        Evado.UniForm.Model.EuAppData clientDataObject = new Evado.UniForm.Model.EuAppData ( );
        EvEventCodes result;

        // 
        // Update the object.
        // 
        if ( this.updateObjectValue ( PageCommand ) == false )
        {
          this.ErrorMessage = EdLabels.UserProfile_Value_Update_Error_Message;

          this.LogMethodEnd ( "RegisterUserDetails" );
          return EvEventCodes.Value_Update_Processing_Error;
        }

        this.LogValue ( "Guid: " + this.Session.AdminUserProfile.Guid );
        this.LogValue ( "UserId: " + this.Session.AdminUserProfile.UserId );
        this.LogValue ( "CommonName: " + this.Session.AdminUserProfile.CommonName );

        this.Session.AdminUserProfile.UserId = EvStatics.CleanSamUserId (
          this.Session.AdminUserProfile.UserId );

        if ( PageCommand.hasParameter ( EuUserRegistration.CONST_NEW_PASSWORD_PARAMETER ) == true )
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
        if ( stSaveAction == EuUserRegistration.CONST_DELETE_ACTION )
        {
          this.Session.AdminUserProfile.CommonName = String.Empty;
        }

        result = this.saveUserProfile ( );

        if ( result != EvEventCodes.Ok )
        {
          this.LogMethodEnd ( "RegisterUserDetails" );
          return result;
        }

        this.LogMethodEnd ( "RegisterUserDetails" );

        return EvEventCodes.Ok;

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

      this.LogMethodEnd ( "RegisterUserDetails" );
      return EvEventCodes.Database_Record_Update_Error;

    }//END RegisterUserDetails method

    // ==================================================================================
    /// <summary>
    /// THis method validates that the user id is not duplicated in the ADS
    /// </summary>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.UniForm.Model.EuAppData GetUserRegistrationExitPage (
      EvEventCodes UpdateResult )
    {
      this.LogMethod ( "GetUserRegistrationExitPage method" );
      //
      // Initialise the methods variables and objects.
      //
      Evado.UniForm.Model.EuAppData clientDataObject = new Evado.UniForm.Model.EuAppData ( );
      String groupDescription = this.AdapterObjects.ContentTemplates.DemoRegistrationConfirmation;

      if ( UpdateResult != EvEventCodes.Ok )
      {
        this.LogDebug ( "ERROR: Update Result Error: {0}", UpdateResult );

        groupDescription = this.AdapterObjects.ContentTemplates.DemoRegistrationError;
      }

      //
      // set the client ResultData object properties
      //
      clientDataObject.Id = this.Session.AdminUserProfile.Guid;
      clientDataObject.Page.Id = this.Session.AdminUserProfile.Guid;
      clientDataObject.Title = EdLabels.User_Profile_Page_Title
        + this.Session.AdminUserProfile.CommonName;

      clientDataObject.Page.Title = clientDataObject.Title;
      clientDataObject.Page.PageId = Evado.Digital.Model.EdStaticPageIds.User_Profile_Page.ToString ( );
      clientDataObject.Page.EditAccess = Evado.UniForm.Model.EuEditAccess.Enabled;

      var pageGroup = clientDataObject.Page.AddGroup (
        String.Empty,
        Evado.UniForm.Model.EuEditAccess.Disabled );
      pageGroup.Layout = Evado.UniForm.Model.EuGroupLayouts.Full_Width;

      //
      // Define the markdown options.
      //
      MarkdownSharp.MarkdownOptions markDownOptions = new MarkdownSharp.MarkdownOptions ( );
      markDownOptions.AutoHyperlink = true;
      markDownOptions.AutoNewlines = true;
      markDownOptions.EmptyElementSuffix = "/>";
      markDownOptions.EncodeProblemUrlCharacters = true;
      markDownOptions.LinkEmails = true;
      markDownOptions.StrictBoldItalic = true;

      //
      // Initialise the markdown class
      //
      MarkdownSharp.Markdown markDown = new MarkdownSharp.Markdown ( markDownOptions );

      //
      // perform the html body transformation.
      //
      string description = markDown.Transform ( groupDescription );

      pageGroup.Description = description;


      this.LogMethodEnd ( "GetUserRegistrationExitPage" );
      return clientDataObject;
    }

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
          && parameter.Name != EuUserRegistration.CONST_ADDRESS_FIELD_ID
          && parameter.Name != EuUserRegistration.CONST_CURRENT_FIELD_ID
          && parameter.Name != EuUserRegistration.CONST_NEW_PASSWORD_PARAMETER )
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