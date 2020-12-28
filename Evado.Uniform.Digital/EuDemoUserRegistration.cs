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
using Evado.Model.Digital;
// using Evado.Web;

namespace Evado.UniForm.Clinical
{
  /// <summary>
  /// This class profile UniFORM class adapter for the user profile classes
  /// </summary>
  public partial class EuDemoUserRegistration : EuClassAdapterBase
  {
    #region Class Initialisation
    /// <summary>
    /// This method initialises the class.
    /// </summary>
    public EuDemoUserRegistration ( )
    {
      this.ClassNameSpace = "Evado.UniForm.Clinical.EuDemoUserRegistration.";
    }

    /// <summary>
    /// This method initialises the class and passs in the user profile.
    /// </summary>
    public EuDemoUserRegistration (
      EuApplicationObjects ApplicationObjects,
      EvUserProfileBase ServiceUserProfile,
      EuSession SessionObjects,
      String UniFormBinaryFilePath,
      EvClassParameters Settings )
    {
      this.ApplicationObjects = ApplicationObjects;
      this.ServiceUserProfile = ServiceUserProfile;
      this.Session = SessionObjects;
      this.UniForm_BinaryFilePath = UniFormBinaryFilePath;
      this.ClassParameters = Settings;

      this.LoggingLevel = Settings.LoggingLevel;

      this.ClassNameSpace = "Evado.UniForm.Clinical.EuDemoUserRegistration.";
      this.LogInitMethod ( "EuDemoUserRegistration initialisation" );
      this.LogInit ( "ServiceUserProfile.UserId: " + ServiceUserProfile.UserId );
      this.LogInit ( "SessionObjects.Project.ProjectId: " + this.Session.Application.ApplicationId );
      this.LogInit ( "SessionObjects.UserProfile.Userid: " + this.Session.UserProfile.UserId );
      this.LogInit ( "SessionObjects.UserProfile.CommonName: " + this.Session.UserProfile.CommonName );
      this.LogInit ( "UniFormBinaryFilePath: " + this.UniForm_BinaryFilePath );

      this.LogInit ( "Settings:" );
      this.LogInit ( "-PlatformId: " + this.ClassParameters.PlatformId );
      this.LogInit ( "-CustomerGuid: " + this.ClassParameters.CustomerGuid );
      this.LogInit ( "-ApplicationGuid: " + this.ClassParameters.ApplicationGuid );
      this.LogInit ( "-LoggingLevel: " + Settings.LoggingLevel );
      this.LogInit ( "-UserId: " + Settings.UserProfile.UserId );
      this.LogInit ( "-UserCommonName: " + Settings.UserProfile.CommonName );

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
    private const String CONST_DEMO_USER_PREFIX = "DM";

    public const String CONST_DEMO_ORGANISATION = "DEMO01";

    private Evado.Bll.Clinical.EvUserProfiles _Bll_UserProfiles = new Evado.Bll.Clinical.EvUserProfiles ( );

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

        var pageId = PageCommand.GetPageId<EvPageIds> ( );

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
          case Evado.Model.UniForm.ApplicationMethods.List_of_Objects:
          case Evado.Model.UniForm.ApplicationMethods.Get_Object:
          case Evado.Model.UniForm.ApplicationMethods.Create_Object:
          case Evado.Model.UniForm.ApplicationMethods.Save_Object:
          case Evado.Model.UniForm.ApplicationMethods.Delete_Object:
            {
              switch ( pageId )
              {
                case EvPageIds.Demo_User_Page:
                default:
                  {
                    clientDataObject = this.getDataObject_RegistrationPage ( PageCommand );
                    break;
                  }
                case EvPageIds.Demo_User_Exit_Page:
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
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData getDataObject_RegistrationPage (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogValue ( Evado.Model.UniForm.EuStatics.CONST_METHOD_START
        + this.ClassNameSpace + "getDataObject_RegistrationPage" );
      this.LogDebug ( "AdminOrganisation.OrgId: " + this.Session.AdminOrganisation.OrgId );
      this.LogDebug ( "Demo Expiry {0}", this.ApplicationObjects.ApplicationSettings.DemoAccountExpiryDays.ToString() );
      try
      {
        // 
        // Initialise the methods variables and objects.
        //      
        Evado.Model.UniForm.AppData clientDataObject = new Model.UniForm.AppData ( );

        // 
        // Create the milestone object and add it to the clinical session object.
        // 
        this.Session.AdminUserProfile = new Evado.Model.Digital.EvUserProfile ( );
        this.Session.AdminUserProfile.Guid = Evado.Model.Digital.EvcStatics.CONST_NEW_OBJECT_ID;
        this.Session.AdminUserProfile.OrgId = this.Session.AdminOrganisation.OrgId;
        this.Session.AdminUserProfile.CustomerGuid = this.Session.Customer.Guid;
        this.Session.AdminUserProfile.ExpiryDate = DateTime.Now.AddDays (
          this.ApplicationObjects.ApplicationSettings.DemoAccountExpiryDays );
        this.Session.AdminUserProfile.UserId = this.createDemoUderId ( );
        this.Session.AdminUserProfile.FamilyName = this.Session.AdminUserProfile.UserId;
        this.Session.AdminUserProfile.GivenName = this.Session.AdminUserProfile.UserId;
        this.Session.AdminUserProfile.RoleId = EvRoleList.Site_User;

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
        this.ErrorMessage = EvLabels.User_Profile_Creation_Error_Message;

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
      int userCount = this._Bll_UserProfiles.UserCount ( String.Empty );
      userCount++;

      userId = EuDemoUserRegistration.CONST_DEMO_USER_PREFIX + userCount.ToString ( "##000" );

      return userId;
    }

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="ClientDataObject">Evado.Model.UniForm.AppData object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_RegistrationPage (
      Evado.Model.UniForm.AppData ClientDataObject )
    {
      this.LogMethod ( "getDataObject_RegistrationPage" );
      //
      // set the client ResultData object properties
      //
      ClientDataObject.Id = this.Session.AdminUserProfile.Guid;
      ClientDataObject.Page.Id = this.Session.AdminUserProfile.Guid;
      ClientDataObject.Title = EvLabels.User_Profile_Page_Title
        + this.Session.AdminUserProfile.CommonName;

      ClientDataObject.Page.Title = ClientDataObject.Title;
      ClientDataObject.Page.PageId = EvPageIds.User_Profile_Page.ToString ( );
      ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;


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
        EvLabels.Demo_Registration_Save_Command_Title,
        EuAdapter.APPLICATION_ID,
        EuAdapterClasses.Demo_Registration.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Custom_Method );

      pageCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.Get_Object );

      pageCommand.SetPageId ( EvPageIds.Demo_User_Exit_Page );

      // 
      // Define the save and delete groupCommand parameters
      // 
      pageCommand.SetGuid ( this.Session.Customer.Guid );

    }//END getclientDataObject_Commands Method

    // ==============================================================================
    /// <summary>
    /// This method creates the demonstration user registration instructions.
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_InstructionsGroup (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getDataObject_InstructionsGroup" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );

      // 
      // create the page pageMenuGroup
      // 
      pageGroup = Page.AddGroup (
         String.Empty,
         Evado.Model.UniForm.EditAccess.Disabled );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

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
      string description = markDown.Transform ( this.ApplicationObjects.ContentTemplates.DemoRegistrationInstuctions );

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
    /// <param name="GroupField">Evado.Model.UniForm.PageGroup object.</param>
    //  ---------------------------------------------------------------------------------
    private void getStreamedVideoField (
      Evado.Model.UniForm.Group PageGroup )
    {
      this.LogMethod ( "getStreamedVideoField" );

      //
      // if there is not video donot create the video field.
      //
      if ( this.ApplicationObjects.ApplicationSettings.DemoRegistrationVideoUrl == String.Empty )
      {
        return;
      }

      // 
      // Initialise local variables.
      // 
      Evado.Model.UniForm.Field groupField = PageGroup.createField ( );
      groupField.Type = Evado.Model.EvDataTypes.Streamed_Video;
      groupField.Value = this.ApplicationObjects.ApplicationSettings.DemoRegistrationVideoUrl;
      groupField.Description = String.Empty;

      int iWidth = 800;
      int iHeight = 0;

      this.LogDebug ( "iWidth: " + iWidth );
      this.LogDebug ( "iHeight: " + iHeight );
      if ( iWidth > 0 )
      {
        groupField.AddParameter ( Model.UniForm.FieldParameterList.Width, iWidth.ToString ( ) );
      }
      if ( iHeight > 0 )
      {
        groupField.AddParameter ( Model.UniForm.FieldParameterList.Height, iHeight.ToString ( ) );
      }

      this.LogDebug ( "Value: " + groupField.Value );

      return;

    }//END getStreamedVideoField method.

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

      // 
      // create the page pageMenuGroup
      // 
      pageGroup = Page.AddGroup (
         String.Empty,
         Evado.Model.UniForm.EditAccess.Inherited_Access );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
      pageGroup.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      pageGroup.SetCommandBackBroundColor (
        Model.UniForm.GroupParameterList.BG_Mandatory,
        Model.UniForm.Background_Colours.Red );

      //
      // Add the groups commands.
      //
      this.getDataObject_GroupCommands ( pageGroup );

      // 
      // Create the user id object
      // 
      groupField = pageGroup.createTextField (
         Evado.Model.Digital.EvUserProfile.UserProfileFieldNames.UserId.ToString ( ),
        EvLabels.User_Profile_Identifier_Field_Label,
        this.Session.AdminUserProfile.UserId,
        80 );
      groupField.Layout = EuFormGenerator.ApplicationFieldLayout;
      groupField.EditAccess = Evado.Model.UniForm.EditAccess.Disabled;


      // 
      // Create the comon name object
      // 
      groupField = pageGroup.createTextField (
         Evado.Model.Digital.EvUserProfile.UserProfileFieldNames.CommonName,
        EvLabels.Dem_Registration_CommonName_Field_Label,
        this.Session.AdminUserProfile.CommonName,
        80 );
      groupField.Layout = EuFormGenerator.ApplicationFieldLayout;
      groupField.Mandatory = true;

      // 
      // Create the user's email address object
      // 
      groupField = pageGroup.createTelephoneNumberField (
         Evado.Model.Digital.EvUserProfile.UserProfileFieldNames.Telephone.ToString ( ),
        EvLabels.UserProfile_Telephone_Field_Label,
        this.Session.AdminUserProfile.Telephone );
      groupField.Layout = EuFormGenerator.ApplicationFieldLayout;
      groupField.Mandatory = true;

      // 
      // Create the user's email address object
      // 
      groupField = pageGroup.createEmailAddressField (
         Evado.Model.Digital.EvUserProfile.UserProfileFieldNames.Email_Address.ToString ( ),
        EvLabels.UserProfile_Email_Field_Label,
        this.Session.AdminUserProfile.EmailAddress );
      groupField.Layout = EuFormGenerator.ApplicationFieldLayout;
      groupField.Mandatory = true;
      groupField.setBackgroundColor (
        Model.UniForm.FieldParameterList.BG_Mandatory,
        Model.UniForm.Background_Colours.Red );

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
        EvLabels.Demo_Registration_Save_Command_Title,
        EuAdapter.APPLICATION_ID,
        EuAdapterClasses.Demo_Registration.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Custom_Method );

      groupCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.Get_Object );

      groupCommand.SetPageId ( EvPageIds.Demo_User_Exit_Page );

      groupCommand.SetGuid ( this.Session.Customer.Guid );

      this.LogMethodEnd ( "getDataObject_GroupCommands" );
    }//END getDataObject_GroupCommands method.

    // ==============================================================================
    /// <summary>
    /// This method outputs a debug group in the registration page.
    /// </summary>
    /// <param name="Page"> Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_Debug_Group (
      Evado.Model.UniForm.Page PageObject )
    {
      if ( this.LoggingLevel < 6 )
      {
        return;
      }

      var pageGroup = PageObject.AddGroup ( "", Model.UniForm.EditAccess.Disabled );

      System.Text.StringBuilder description = new StringBuilder ( );

      description.AppendFormat ( "Customer No: {0} - {1} \r\n", this.Session.Customer.CustomerNo, this.Session.Customer.Name );

      description.AppendFormat ( "Organisation Id: {0} - {1} \r\n", this.Session.AdminOrganisation.OrgId, this.Session.AdminOrganisation.Name );

      description.AppendLine ( "Demonstration User details: " );
      description.AppendFormat ( "OrgId: {0} \r\n", this.Session.AdminUserProfile.OrgId );
      description.AppendFormat ( "UserId: {0} \r\n", this.Session.AdminUserProfile.UserId );
      description.AppendFormat ( "GivenName: {0} \r\n", this.Session.AdminUserProfile.GivenName );
      description.AppendFormat ( "FamilyName: {0} \r\n", this.Session.AdminUserProfile.FamilyName );
      description.AppendFormat ( "RoleId: {0} \r\n", this.Session.AdminUserProfile.RoleId );

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
    /// <param name="PageCommand">Evado.UniForm.Model.ClientClientDataObjectEvado.Model.UniForm.Command object.</param>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData getDataObject_ExitPage (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getDataObject_ExitPage" );
      this.LogValue ( "Parameter: " + PageCommand.getAsString ( false, false ) );
      // 
      // Initialise the methods variables and objects.
      //      
      Evado.Model.UniForm.AppData clientDataObject = new Model.UniForm.AppData ( );
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
    /// <param name="PageCommand">Evado.UniForm.Model.ClientClientDataObjectEvado.Model.UniForm.Command object.</param>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private EvEventCodes RegisterUserDetails (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "RegisterUserDetails" );
      try
      {
        // 
        // Initialise the methods variables and objects.
        //      
        Evado.Model.UniForm.AppData clientDataObject = new Model.UniForm.AppData ( );
        EvEventCodes result;

        // 
        // Update the object.
        // 
        if ( this.updateObjectValue ( PageCommand ) == false )
        {
          this.ErrorMessage = EvLabels.UserProfile_Value_Update_Error_Message;

          this.LogMethodEnd ( "RegisterUserDetails" );
          return EvEventCodes.Value_Update_Processing_Error;
        }

        this.LogValue ( "Guid: " + this.Session.AdminUserProfile.Guid );
        this.LogValue ( "UserId: " + this.Session.AdminUserProfile.UserId );
        this.LogValue ( "CommonName: " + this.Session.AdminUserProfile.CommonName );

        this.Session.AdminUserProfile.UserId = EvStatics.CleanSamUserId (
          this.Session.AdminUserProfile.UserId );

        //
        // Set Customer Guid to associated the user with a customer.
        //
        // Evado user are associated with the Application Guid.
        //
        if ( this.Session.AdminUserProfile.Guid == EvStatics.CONST_NEW_OBJECT_ID )
        {
          this.Session.AdminUserProfile.CustomerGuid = this.Session.Customer.Guid;

          if ( this.Session.AdminUserProfile.OrgId == "Evado" )
          {
            this.Session.AdminUserProfile.CustomerGuid = this.ClassParameters.ApplicationGuid;
          }
        }

        if ( PageCommand.hasParameter ( EuDemoUserRegistration.CONST_NEW_PASSWORD_PARAMETER ) == true )
        {
          this.LogValue ( "Creating a new password." );
          this.createDefaultPassword ( );
        }

        this.LogValue ( "AdminUserProfile.Password: " + this.Session.AdminUserProfile.Password );

        // 
        // Get the save action message value.
        // 
        String stSaveAction = PageCommand.GetParameter ( Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION );

        // 
        // Resets the save action to the parameter passed in the page groupCommand.
        // 
        if ( stSaveAction == EuDemoUserRegistration.CONST_DELETE_ACTION )
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
        this.ErrorMessage = EvLabels.User_Profile_Save_Error_Message;

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
    private Evado.Model.UniForm.AppData GetUserRegistrationExitPage (
      EvEventCodes UpdateResult )
    {
      this.LogMethod ( "GetUserRegistrationExitPage method" );
      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.UniForm.AppData clientDataObject = new Model.UniForm.AppData ( );
      String groupDescription = this.ApplicationObjects.ContentTemplates.DemoRegistrationConfirmation;

      if ( UpdateResult != EvEventCodes.Ok )
      {
        this.LogDebug ( "ERROR: Update Result Error: {0}", UpdateResult );

        groupDescription = this.ApplicationObjects.ContentTemplates.DemoRegistrationError;
      }

      //
      // set the client ResultData object properties
      //
      clientDataObject.Id = this.Session.AdminUserProfile.Guid;
      clientDataObject.Page.Id = this.Session.AdminUserProfile.Guid;
      clientDataObject.Title = EvLabels.User_Profile_Page_Title
        + this.Session.AdminUserProfile.CommonName;

      clientDataObject.Page.Title = clientDataObject.Title;
      clientDataObject.Page.PageId = EvPageIds.User_Profile_Page.ToString ( );
      clientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      var pageGroup = clientDataObject.Page.AddGroup (
        String.Empty,
        Model.UniForm.EditAccess.Disabled );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

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

        if ( parameter.Name.Contains ( Evado.Model.Digital.EvcStatics.CONST_GUID_IDENTIFIER ) == false
          && parameter.Name != Evado.Model.UniForm.CommandParameters.Custom_Method.ToString ( )
          && parameter.Name != Evado.Model.UniForm.CommandParameters.Page_Id.ToString ( )
          && parameter.Name != Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION
          && parameter.Name != EuDemoUserRegistration.CONST_ADDRESS_FIELD_ID
          && parameter.Name != EuDemoUserRegistration.CONST_CURRENT_FIELD_ID
          && parameter.Name != EuDemoUserRegistration.CONST_NEW_PASSWORD_PARAMETER )
        {
          this.LogTextEnd ( " >> UPDATED" );
          try
          {
            Evado.Model.Digital.EvUserProfile.UserProfileFieldNames fieldName =
              Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue<Evado.Model.Digital.EvUserProfile.UserProfileFieldNames> (
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