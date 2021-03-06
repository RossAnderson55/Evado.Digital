﻿/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\ApplicationProfile.cs" company="EVADO HOLDING PTY. LTD.">
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
  /// This class terminates the Organisation object.
  /// </summary>
  public class EuStaticContentTemplates : EuClassAdapterBase
  {
    #region Class Initialisation
    /// <summary>
    /// This method initialises the class.
    /// </summary>
    public EuStaticContentTemplates ( )
    {
      this.ClassNameSpace = "Evado.UniForm.Clinical.EuStaticContentTemplates.";
    }

    /// <summary>
    /// This method initialises the class and passs in the user profile.
    /// </summary>
    public EuStaticContentTemplates (
      EuGlobalObjects ApplicationObjects,
      EvUserProfileBase ServiceUserProfile,
      EuSession SessionObjects,
      String UniFormBinaryFilePath )
    {
      this.AdapterObjects = ApplicationObjects;
      this.ServiceUserProfile = ServiceUserProfile;
      this.Session = SessionObjects;
      this.UniForm_BinaryFilePath = UniFormBinaryFilePath;
      this.ClassNameSpace = "Evado.UniForm.Clinical.EuStaticContentTemplates.";


      this.LogInitMethod ( "EuStaticContentTemplates initialisation" );
      this.LogInit ( "ServiceUserProfile.UserId: " + ServiceUserProfile.UserId );
      this.LogInit ( "SessionObjects.UserProfile.Userid: " + this.Session.UserProfile.UserId );
      this.LogInit ( "SessionObjects.UserProfile.CommonName: " + this.Session.UserProfile.CommonName );

    }//END Method


    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class constants and variables.

    private const String CONST_DISPLAY_PAGE = "DISP_PG";
    public const String CONST_EMAIL_TEMPLATE_FILENAME = "EMAIL_TMPLATE.XML";

    private bool _displayPage = false;
    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class public methods

    // ==================================================================================
    /// <summary>
    /// This method gets the trial site object.
    /// 
    /// </summary>
    /// <param name="PageCommand">ClientPateEvado.UniForm.Model.Command object</param>
    /// <returns>Evado.UniForm.Model.AppData</returns>
    //  ----------------------------------------------------------------------------------
    override public Evado.UniForm.Model.AppData getDataObject (
      Evado.UniForm.Model.Command PageCommand )
    {
      this.LogMethod ( "getClientDataObject" );

      this.LogValue ( "PageCommand Content: " + PageCommand.getAsString ( false, false ) );
      try
      {
        // 
        // Initialise the methods variables and objects.
        // 
        bool bResult = true;
        Evado.UniForm.Model.AppData clientDataObjectObject = new Evado.UniForm.Model.AppData ( );

        //
        // Determine if the user has access to this page and log and error if they do not.
        //
        if ( this.Session.UserProfile.hasAdministrationAccess == false )
        {
          this.LogIllegalAccess (
            this.ClassNameSpace + "getListObject",
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

        if ( this.AdapterObjects.ContentTemplates == null )
        {
          this.AdapterObjects.ContentTemplates = new EvStaticContentTemplates ( );

          this.AdapterObjects.ContentTemplates =
            EvStatics.Files.readXmlFile<EvStaticContentTemplates> (
            this.AdapterObjects.ApplicationPath, EuStaticContentTemplates.CONST_EMAIL_TEMPLATE_FILENAME );
        }

        if ( this.AdapterObjects.ContentTemplates == null )
        {
          this.AdapterObjects.ContentTemplates = new EvStaticContentTemplates ( );
        }
        if ( this.AdapterObjects.ContentTemplates.IntroductoryEmail_Title == null )
        {
          this.AdapterObjects.ContentTemplates.IntroductoryEmail_Title = String.Empty;
          this.AdapterObjects.ContentTemplates.IntroductoryEmail_Body = String.Empty;
          this.AdapterObjects.ContentTemplates.UpdatePasswordEmail_Title = String.Empty;
          this.AdapterObjects.ContentTemplates.UpdatePasswordEmail_Body = String.Empty;
          this.AdapterObjects.ContentTemplates.ResetPasswordEmail_Body = String.Empty;
          this.AdapterObjects.ContentTemplates.ResetPasswordEmail_Body = String.Empty;
          this.AdapterObjects.ContentTemplates.PasswordConfirmationEmail_Title = String.Empty;
          this.AdapterObjects.ContentTemplates.PasswordConfirmationEmail_Body = String.Empty;
        }

        _displayPage = false;
        string value = PageCommand.GetParameter ( EuStaticContentTemplates.CONST_DISPLAY_PAGE );

        if ( true == EvStatics.getBool ( value ) )
        {
          _displayPage = true;
        }

        // 
        // Set the page type to control the DB query type.
        // 
        string pageType = PageCommand.GetPageId ( );

        this.Session.setPageId ( pageType );

        this.LogValue ( "PageId: " + this.Session.PageId );

        // 
        // Determine the method to be called
        // 
        switch ( PageCommand.Method )
        {
          case Evado.UniForm.Model.ApplicationMethods.Get_Object:
            {
              clientDataObjectObject = this.getObject ( PageCommand );
              break;
            }
          case Evado.UniForm.Model.ApplicationMethods.Save_Object:
            {
              this.LogValue ( " Save Object method" );

              // 
              // Update the object values
              // 
              bResult = this.updateObject ( PageCommand );

              // 
              // Process an update error result.
              // 
              if ( bResult == false )
              {
                this.LogValue ( " Save method failed" );

                // 
                // Return the generated ResultData object.
                // 
                clientDataObjectObject = null;
              }
              break;
            }

        }//END Swith

        // 
        // Handle returned exceptions.
        // 
        if ( clientDataObjectObject == null )
        {
          this.LogValue ( " null application data returned." );
        }

        // 
        // Return the last client ResultData object.
        // 
        return clientDataObjectObject;

      }
      catch ( Exception Ex )
      {
        this.LogException ( Ex );
      }
      return new Evado.UniForm.Model.AppData ( );

    }//END getClientDataObject method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class get object methods

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.Command object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.UniForm.Model.AppData getObject (
      Evado.UniForm.Model.Command PageCommand )
    {
      this.LogMethod ( "getObject" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.AppData clientDataObject = new Evado.UniForm.Model.AppData ( );
      Guid OrgGuid = Guid.Empty;

      try
      {
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
    /// <param name="ClientDataObject">Evado.UniForm.Model.AppData object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private void getDataObject ( Evado.UniForm.Model.AppData ClientDataObject )
    {
      this.LogMethod ( "getDataObject" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.Command pageCommand = new Evado.UniForm.Model.Command ( );
      Evado.UniForm.Model.Field pageField = new Evado.UniForm.Model.Field ( );

      ClientDataObject.Id = Guid.NewGuid ( );
      ClientDataObject.Title = EdLabels.UserAdmin_Page_Title;

      ClientDataObject.Page.Id = ClientDataObject.Id;
      ClientDataObject.Page.Title = ClientDataObject.Title;
      ClientDataObject.Page.EditAccess = Evado.UniForm.Model.EditAccess.Enabled;
      this.LogValue ( "Page.Status: " + ClientDataObject.Page.EditAccess );

      //
      // Add the page commands
      //
      //this.addPageCommands ( ClientDataObject.Page );

      //
      // Create the demo registration group..
      //
      this.create_DemonRegistration_Group ( ClientDataObject.Page );

      //
      // Create the introductory email Group.
      //
      this.create_IntroductoryEmail_Group ( ClientDataObject.Page );

      //
      // create the reset pssword email group.
      //
      this.create_ResetPasswordEmail_Group ( ClientDataObject.Page );

      //
      // Create the passsword update email group
      //
      this.create_UpdatePasswordEmail_Group ( ClientDataObject.Page );

      //
      // create the reset pssword email group.
      //
      this.create_PasswordChangeEmail_Group ( ClientDataObject.Page );

    }//END getDataObject Method

    // ==============================================================================
    /// <summary>
    /// This method adds the page commands.
    /// </summary>
    /// <param name="PageObject">Evado.UniForm.Model.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void addPageCommands (
      Evado.UniForm.Model.Page PageObject )
    {
      this.LogMethod ( "addPageCommands" );
      //
      // Initialise the methods variables and objects.
      //
      Evado.UniForm.Model.Command pageCommand = new Evado.UniForm.Model.Command ( );

      if ( this._displayPage == true )
      {
        // 
        // Add the display groupCommand
        //
        pageCommand = PageObject.addCommand (
          EdLabels.UserAdmin_Edit_Page_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Email_Templates.ToString ( ),
          Evado.UniForm.Model.ApplicationMethods.Custom_Method );

        pageCommand.setCustomMethod ( Evado.UniForm.Model.ApplicationMethods.Get_Object );

        pageCommand.AddParameter ( EuStaticContentTemplates.CONST_DISPLAY_PAGE, "false" );

        pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Email_Templates_Page );

        return;
      }
      else
      {
        // 
        // Add the display groupCommand
        //
        pageCommand = PageObject.addCommand (
          EdLabels.UserAdmin_Display_Page_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Email_Templates.ToString ( ),
          Evado.UniForm.Model.ApplicationMethods.Custom_Method );

        pageCommand.setCustomMethod ( Evado.UniForm.Model.ApplicationMethods.Get_Object );

        pageCommand.AddParameter ( EuStaticContentTemplates.CONST_DISPLAY_PAGE, "true" );

        pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Email_Templates_Page );
      }

      // 
      // Add the save groupCommand
      //
      pageCommand = PageObject.addCommand (
        EdLabels.UserAdmin_Save_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Email_Templates.ToString ( ),
        Evado.UniForm.Model.ApplicationMethods.Save_Object );

    }//END addPageCommands method

    // ==============================================================================
    /// <summary>
    /// This method creates the introductory email grop
    /// </summary>
    /// <param name="PageObject">Evado.UniForm.Model.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void create_Display_Group (
      Evado.UniForm.Model.Group PageGroup,
      String Title,
      String Body )
    {
      this.LogMethod ( "create_Display_Group" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.Field pageField = new Evado.UniForm.Model.Field ( );

      Title = Title.Replace ( "\r\n\r\n", "\r\n \r\n" );
      Title = Title.Replace ( EvcStatics.TEXT_SUBSITUTION_FIRST_NAME, "First" );
      Title = Title.Replace ( EvcStatics.TEXT_SUBSITUTION_FAMILY_NAME, "Family" );
      Title = Title.Replace ( EvcStatics.TEXT_SUBSITUTION_ADAPTER_TITLE, "Customer name" );

      Body = Body.Replace ( "\r\n\r\n", "\r\n \r\n" );
      Body = Body.Replace ( EvcStatics.TEXT_SUBSITUTION_FIRST_NAME, "First" );
      Body = Body.Replace ( EvcStatics.TEXT_SUBSITUTION_FAMILY_NAME, "Family" );
      Body = Body.Replace ( EvcStatics.TEXT_SUBSITUTION_EMAIL_ADDRESS, "name@domain.com" );
      Body = Body.Replace ( EvcStatics.TEXT_SUBSITUTION_USER_ID, "userId" );
      Body = Body.Replace ( EvcStatics.TEXT_SUBSITUTION_PASSWORD, "password" );
      Body = Body.Replace ( EvcStatics.TEXT_SUBSITUTION_ORG_ID, "org ID" );
      Body = Body.Replace ( EvcStatics.TEXT_SUBSITUTION_ORG_NAME, "Org Name" );
      Body = Body.Replace ( EvcStatics.TEXT_SUBSITUTION_ADAPTER_TITLE, "Customer name" );
      Body = Body.Replace ( EvcStatics.TEXT_SUBSITUTION_PASSWORD_RESET_URL, "https://www1evado.com/reset" );
      Body = Body.Replace ( EvcStatics.TEXT_SUBSITUTION_DATE_STAMP, DateTime.Now.ToLongDateString ( ) + " at " + DateTime.Now.ToShortTimeString ( ) );

      // 
      // Create the home page title
      // 
      pageField = PageGroup.createReadOnlyTextField (
        String.Empty,
        EdLabels.UserAdmin_Email_Title_Field_Label,
        Title );
      pageField.Layout = EuAdapter.DefaultFieldLayout;

      // 
      // Create the home page title
      // 
      pageField = PageGroup.createReadOnlyTextField (
        String.Empty,
        EdLabels.UserAdmin_Email_Body_Field_Label,
        Body );
      pageField.Layout = EuAdapter.DefaultFieldLayout;

    }//END create_Display_Group method


    // ==============================================================================
    /// <summary>
    /// This method creates the introductory email grop
    /// </summary>
    /// <param name="PageObject">Evado.UniForm.Model.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void create_IntroductoryEmail_Group (
      Evado.UniForm.Model.Page PageObject )
    {
      this.LogMethod ( "create_IntroductoryEmail_Group" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.Command pageCommand = new Evado.UniForm.Model.Command ( );
      Evado.UniForm.Model.Field pageField = new Evado.UniForm.Model.Field ( );

      // 
      // create the page pageMenuGroup
      // 
      Evado.UniForm.Model.Group pageGroup = PageObject.AddGroup (
        EdLabels.UserAdmin_Introductory_Group_Title,
        Evado.UniForm.Model.EditAccess.Inherited );
      pageGroup.Layout = Evado.UniForm.Model.GroupLayouts.Full_Width;

      //
      // Add the group comands
      //
      this.addGroupCommands ( pageGroup );

      if ( this._displayPage == true )
      {
        this.create_Display_Group ( pageGroup,
          this.AdapterObjects.ContentTemplates.IntroductoryEmail_Title,
          this.AdapterObjects.ContentTemplates.IntroductoryEmail_Body );

        return;
      }

      // 
      // Create the home page title
      // 
      pageField = pageGroup.createTextField (
        EvStaticContentTemplates.ClassFieldNames.Introductory_Email_Title,
        EdLabels.UserAdmin_Email_Title_Field_Label,
        this.AdapterObjects.ContentTemplates.IntroductoryEmail_Title,
        80 );
      pageField.Layout = EuAdapter.DefaultFieldLayout;

      // 
      // Create the home page title
      // 
      pageField = pageGroup.createFreeTextField (
        EvStaticContentTemplates.ClassFieldNames.Introductory_Email_Body,
        EdLabels.UserAdmin_Email_Body_Field_Label,
        this.AdapterObjects.ContentTemplates.IntroductoryEmail_Body, 80, 30 );
      pageField.Layout = EuAdapter.DefaultFieldLayout;

      pageField.Description = EdLabels.UserAdmin_Email_Body_Description_Field_Label ;

    }//END create_IntroductoryEmail_Group Method

    // ==============================================================================
    /// <summary>
    /// This method creates the reset password email group
    /// </summary>
    /// <param name="PageObject">Evado.UniForm.Model.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void create_ResetPasswordEmail_Group (
      Evado.UniForm.Model.Page PageObject )
    {
      this.LogMethod ( "create_ResetPasswordEmail_Group" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.Command pageCommand = new Evado.UniForm.Model.Command ( );
      Evado.UniForm.Model.Field pageField = new Evado.UniForm.Model.Field ( );

      // 
      // create the page pageMenuGroup
      // 
      Evado.UniForm.Model.Group pageGroup = PageObject.AddGroup (
        EdLabels.UserAdmin_Reset_Password_Group_Title,
        Evado.UniForm.Model.EditAccess.Inherited );
      pageGroup.Layout = Evado.UniForm.Model.GroupLayouts.Full_Width;

      //
      // Add the group comands
      //
      this.addGroupCommands ( pageGroup );

      if ( this._displayPage == true )
      {
        this.create_Display_Group ( pageGroup,
          this.AdapterObjects.ContentTemplates.ResetPasswordEmail_Title,
          this.AdapterObjects.ContentTemplates.ResetPasswordEmail_Body );

        return;
      }

      // 
      // Create the home page title
      // 
      pageField = pageGroup.createTextField (
        EvStaticContentTemplates.ClassFieldNames.Reset_Password_Email_Title,
        EdLabels.UserAdmin_Email_Title_Field_Label,
        this.AdapterObjects.ContentTemplates.ResetPasswordEmail_Title,
        80 );
      pageField.Layout = EuAdapter.DefaultFieldLayout;

      // 
      // Create the home page title
      // 
      pageField = pageGroup.createFreeTextField (
        EvStaticContentTemplates.ClassFieldNames.Reset_Password_Email_Body,
        EdLabels.UserAdmin_Email_Body_Field_Label,
        this.AdapterObjects.ContentTemplates.ResetPasswordEmail_Body, 80, 20 );
      pageField.Layout = EuAdapter.DefaultFieldLayout;

      pageField.Description = 
        EdLabels.UserAdmin_PasswordReset_Body_Description_Field_Label ;

    }//END create_ResetPasswordEmail_Group Method

    // ==============================================================================
    /// <summary>
    /// This method creates the reset password email group
    /// </summary>
    /// <param name="PageObject">Evado.UniForm.Model.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void create_UpdatePasswordEmail_Group (
      Evado.UniForm.Model.Page PageObject )
    {
      this.LogMethod ( "create_UpdatePasswordEmail_Group" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.Command pageCommand = new Evado.UniForm.Model.Command ( );
      Evado.UniForm.Model.Field pageField = new Evado.UniForm.Model.Field ( );

      // 
      // create the page pageMenuGroup
      // 
      Evado.UniForm.Model.Group pageGroup = PageObject.AddGroup (
        EdLabels.UserAdmin_UpdatePassword_Group_Title,
        Evado.UniForm.Model.EditAccess.Inherited );
      pageGroup.Layout = Evado.UniForm.Model.GroupLayouts.Full_Width;

      //
      // Add the group comands
      //
      this.addGroupCommands ( pageGroup );

      if ( this._displayPage == true )
      {
        this.create_Display_Group ( pageGroup,
          this.AdapterObjects.ContentTemplates.UpdatePasswordEmail_Title,
          this.AdapterObjects.ContentTemplates.UpdatePasswordEmail_Body );

        return;
      }

      // 
      // Create the home page title
      // 
      pageField = pageGroup.createTextField (
        EvStaticContentTemplates.ClassFieldNames.Update_Password_Email_Title,
        EdLabels.UserAdmin_Email_Title_Field_Label,
        this.AdapterObjects.ContentTemplates.UpdatePasswordEmail_Title,
        80 );
      pageField.Layout = EuAdapter.DefaultFieldLayout;

      // 
      // Create the home page title
      // 
      pageField = pageGroup.createFreeTextField (
        EvStaticContentTemplates.ClassFieldNames.Update_Password_Email_Body,
        EdLabels.UserAdmin_Email_Body_Field_Label,
        this.AdapterObjects.ContentTemplates.UpdatePasswordEmail_Body, 80, 40 );
      pageField.Layout = EuAdapter.DefaultFieldLayout;

      pageField.Description = 
        EdLabels.UserAdmin_PasswordReset_Body_Description_Field_Label ;

    }//END create_ResetPasswordEmail_Group Method

    // ==============================================================================
    /// <summary>
    /// This method creates the password change group objectt.
    /// </summary>
    /// <param name="PageObject">Evado.UniForm.Model.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void create_PasswordChangeEmail_Group (
      Evado.UniForm.Model.Page PageObject )
    {
      this.LogMethod ( "create_PasswordChangeEmail_Group" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.Command pageCommand = new Evado.UniForm.Model.Command ( );
      Evado.UniForm.Model.Field pageField = new Evado.UniForm.Model.Field ( );

      // 
      // create the page pageMenuGroup
      // 
      Evado.UniForm.Model.Group pageGroup = PageObject.AddGroup (
        EdLabels.UserAdmin_Password_Confirmation_Group_Title,
        Evado.UniForm.Model.EditAccess.Inherited );
      pageGroup.Layout = Evado.UniForm.Model.GroupLayouts.Full_Width;

      //
      // Add the group comands
      //
      this.addGroupCommands ( pageGroup );

      if ( this._displayPage == true )
      {
        this.create_Display_Group ( pageGroup,
          this.AdapterObjects.ContentTemplates.PasswordConfirmationEmail_Title,
          this.AdapterObjects.ContentTemplates.PasswordConfirmationEmail_Body );

        return;
      }


      // 
      // Create the home page title
      // 
      pageField = pageGroup.createTextField (
        EvStaticContentTemplates.ClassFieldNames.Password_Confirmation_Email_Title,
        EdLabels.UserAdmin_Email_Title_Field_Label,
        this.AdapterObjects.ContentTemplates.PasswordConfirmationEmail_Title,
        80 );
      pageField.Layout = EuAdapter.DefaultFieldLayout;

      // 
      // Create the home page title
      // 
      pageField = pageGroup.createFreeTextField (
        EvStaticContentTemplates.ClassFieldNames.Password_Confirmation_Email_Body,
        EdLabels.UserAdmin_Email_Body_Field_Label,
        this.AdapterObjects.ContentTemplates.PasswordConfirmationEmail_Body, 80, 20 );
      pageField.Layout = EuAdapter.DefaultFieldLayout;

      pageField.Description = 
        EdLabels.UserAdmin_PasswordChange_Body_Description_Field_Label ;

    }//END create_PasswordChangeEmail_Group Method

    // ==============================================================================
    /// <summary>
    /// This method creates the password change group objectt.
    /// </summary>
    /// <param name="PageObject">Evado.UniForm.Model.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void create_DemonRegistration_Group (
      Evado.UniForm.Model.Page PageObject )
    {
      this.LogMethod ( "create_DemonRegistration_Group" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.Command pageCommand = new Evado.UniForm.Model.Command ( );
      Evado.UniForm.Model.Field pageField = new Evado.UniForm.Model.Field ( );

      // 
      // create the page pageMenuGroup
      // 
      Evado.UniForm.Model.Group pageGroup = PageObject.AddGroup (
        EdLabels.UserAdmin_Demo_Registration_Group_Title,
        Evado.UniForm.Model.EditAccess.Inherited );
      pageGroup.Layout = Evado.UniForm.Model.GroupLayouts.Full_Width;

      //
      // Add the group comands
      //
      this.addGroupCommands ( pageGroup );

      // 
      // Create instructions field
      // 
      pageField = pageGroup.createFreeTextField (
        EvStaticContentTemplates.ClassFieldNames.DemoRegistrationInstuctions,
        EdLabels.UserAdmin_Demo_Registration_Instructions_Field_Label,
        this.AdapterObjects.ContentTemplates.DemoRegistrationInstuctions,
        80, 20 );
      pageField.Layout = EuAdapter.DefaultFieldLayout;

      // 
      // Create instructions field
      // 
      pageField = pageGroup.createFreeTextField (
        EvStaticContentTemplates.ClassFieldNames.DemoRegistrationConfirmation,
        EdLabels.UserAdmin_Demo_Registration_Confirmation_Field_Label,
        this.AdapterObjects.ContentTemplates.DemoRegistrationConfirmation,
        80, 20 );
      pageField.Layout = EuAdapter.DefaultFieldLayout;

      // 
      // Create instructions field
      // 
      pageField = pageGroup.createFreeTextField (
        EvStaticContentTemplates.ClassFieldNames.DemoRegistrationError,
        EdLabels.UserAdmin_Demo_Registration_Error_Field_Label,
        this.AdapterObjects.ContentTemplates.DemoRegistrationError,
        80, 20 );
      pageField.Layout = EuAdapter.DefaultFieldLayout;

    }//END create_PasswordChangeEmail_Group Method


    // ==============================================================================
    /// <summary>
    /// This method add the group commands to the passed group.
    /// </summary>
    /// <param name="PageObject">Evado.UniForm.Model.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void addGroupCommands ( Evado.UniForm.Model.Group PageGroup )
    {
      //
      // Initialise the methods variables and objects.
      //
      Evado.UniForm.Model.Command pageCommand = new Evado.UniForm.Model.Command ( );

      if ( this._displayPage == true )
      {
        // 
        // Add the display groupCommand
        //
        pageCommand = PageGroup.addCommand (
          EdLabels.UserAdmin_Edit_Page_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Email_Templates.ToString ( ),
          Evado.UniForm.Model.ApplicationMethods.Custom_Method );

        pageCommand.setCustomMethod ( Evado.UniForm.Model.ApplicationMethods.Get_Object );

        pageCommand.AddParameter ( EuStaticContentTemplates.CONST_DISPLAY_PAGE, "false" );

        pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Email_Templates_Page );

        return;
      }
      else
      {
        // 
        // Add the display groupCommand
        //
        pageCommand = PageGroup.addCommand (
          EdLabels.UserAdmin_Display_Page_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Email_Templates.ToString ( ),
          Evado.UniForm.Model.ApplicationMethods.Custom_Method );

        pageCommand.setCustomMethod ( Evado.UniForm.Model.ApplicationMethods.Get_Object );

        pageCommand.AddParameter ( EuStaticContentTemplates.CONST_DISPLAY_PAGE, "true" );

        pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Email_Templates_Page );
      }

      // 
      // Add the save groupCommand
      //
      if ( this._displayPage == false )
      {
        pageCommand = PageGroup.addCommand (
          EdLabels.UserAdmin_Save_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Email_Templates.ToString ( ),
          Evado.UniForm.Model.ApplicationMethods.Save_Object );
      }
    }


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
    private Evado.UniForm.Model.AppData createObject ( Evado.UniForm.Model.Command Command )
    {
      try
      {
        this.LogMethod ( "createObject" );

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "createObject",
          this.Session.UserProfile );

        // 
        // Initialise the methods variables and objects.
        //      
        Evado.UniForm.Model.AppData clientDataObject = new Evado.UniForm.Model.AppData ( );

        //
        // Determine if the user has access to this page and log and error if they do not.
        //
        if ( this.Session.UserProfile.hasEvadoAccess == false )
        {
          this.LogIllegalAccess (
            this.ClassNameSpace + "createObject",
            this.Session.UserProfile );

          this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

          return null;
        }

        //
        // Initialise the dlinical ResultData objects.
        //
        this.AdapterObjects.ContentTemplates = new EvStaticContentTemplates ( );

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
    /// <param name="PageCommand">Evado.UniForm.Model.ClientClientDataObjectEvado.UniForm.Model.Command object.</param>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private bool updateObject ( Evado.UniForm.Model.Command PageCommand )
    {
      try
      {
        this.LogMethod ( "updateObject" );
        this.LogValue ( "Parameter PageEvado.UniForm.Model.Command: " + PageCommand.getAsString ( false, true ) );

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
        this.ClassNameSpace + "updateObject",
          this.Session.UserProfile );

        // 
        // Delete the object.
        // 
        if ( PageCommand.Method == Evado.UniForm.Model.ApplicationMethods.Delete_Object )
        {
          return true;
        }

        // 
        // Update the object.
        // 
        this.updateObjectValue ( PageCommand );




        bool result = EvStatics.Files.saveFile<EvStaticContentTemplates> (
          this.AdapterObjects.ApplicationPath,
          EuStaticContentTemplates.CONST_EMAIL_TEMPLATE_FILENAME,
          this.AdapterObjects.ContentTemplates );

        return true;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = "Error raised saving the email template page..";

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }
      return false;

    }//END method

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="Parameters">List of field values to be updated.</param>
    /// <returns></returns>
    //  ----------------------------------------------------------------------------------
    private void updateObjectValue ( Evado.UniForm.Model.Command PageCommand )
    {
      this.LogMethod ( "updateObjectValue" );
      this.LogValue ( " Parameters.Count: " + PageCommand.Parameters.Count );

      /// 
      /// Iterate through the parameter values updating the ResultData object
      /// 
      foreach ( Evado.UniForm.Model.Parameter parameter in PageCommand.Parameters )
      {
        this.LogTextStart ( parameter.Name + " > " + parameter.Value );

        if ( parameter.Name.Contains (  Evado.Digital.Model.EvcStatics.CONST_GUID_IDENTIFIER ) == false
          && parameter.Name != Evado.UniForm.Model.CommandParameters.Custom_Method.ToString ( )
          && parameter.Name !=  Evado.Digital.Model.EvcStatics.CONST_SAVE_ACTION )
        {
          this.LogText ( ">> UPDATED" );

          try
          {
            EvStaticContentTemplates.ClassFieldNames fieldName =
               Evado.Model.EvStatics.parseEnumValue<EvStaticContentTemplates.ClassFieldNames> (
              parameter.Name );

            this.AdapterObjects.ContentTemplates.setValue ( fieldName, parameter.Value );

          }
          catch ( Exception Ex )
          {
            this.LogException ( Ex );
          }
        }
        this.LogTextEnd ( "" );

      }// End iteration loop

    }//END updateObjectValue method.

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
  }
}//END namespace