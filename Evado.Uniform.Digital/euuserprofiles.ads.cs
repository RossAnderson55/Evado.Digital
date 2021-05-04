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

using Evado.Bll;
using Evado.Model;
using Evado.Bll.Digital;
using Evado.Model.Digital;
// using Evado.Web;

namespace Evado.UniForm.Digital
{
  /// <summary>
  /// This class profile UniFORM class adapter for the user profile classes
  /// </summary>
  public partial class EuUserProfiles : EuClassAdapterBase
  {
    #region Update user method.
    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private EvEventCodes saveUserProfile ( )
    {
      this.LogMethod ( "saveUserProfile" );

      try
      {
        //
        // Initialise the methods variables and objects.
        //
        EvEventCodes result = EvEventCodes.Ok;
        bool isANewUser = false;

        //
        // Reset the Guid if a new user is beeing created.
        //
        if ( this.Session.AdminUserProfile.Guid == EvStatics.CONST_NEW_OBJECT_ID )
        {
          this.Session.AdminUserProfile.Guid = Guid.Empty;

          if ( string.IsNullOrEmpty ( this.Session.AdminUserProfile.Password ) == true )
          {
            this.createDefaultPassword ( );
          }
          isANewUser = true;
        }

        // 
        // update the object.
        // 
        result = this._Bll_UserProfiles.saveItem ( this.Session.AdminUserProfile );

        // 
        // get the debug ResultData.
        // 
        this.LogDebugClass ( this._Bll_UserProfiles.Log );

        // 
        // if an error state is returned create log the event.
        // 
        if ( result != EvEventCodes.Ok )
        {
          string StEvent = this._Bll_UserProfiles.Log + " returned error message: " + Evado.Model.Digital.EvcStatics.getEventMessage ( result );
          this.LogError ( EvEventCodes.Database_Record_Update_Error, StEvent );

          this.ErrorMessage = EdLabels.User_Profile_Save_Error_Message;

          this.LogMethodEnd ( "saveUserProfile" );
          return result;
        }

        //
        // Update the ADS details.
        //
        result = this.updateAdUserProfile ( );

        // 
        // if an error state is returned create log the event.
        // 
        if ( result != EvEventCodes.Ok )
        {
          this.ErrorMessage = EdLabels.UserProfile_AD_Error_Message;

          string stEvent = " returned error message: "
            + Evado.Model.Digital.EvcStatics.getEventMessage ( result );

          this.LogError ( EvEventCodes.Database_Record_Update_Error, stEvent );

          this.LogMethodEnd ( "saveUserProfile" );
          return result;
        }

        //
        // if the password is empty not notification email needs to be sent to the user
        //
        if ( this.Session.AdminUserProfile.Password == String.Empty )
        {
          this.LogDebug ( "Password Empty" );
          this.LogMethodEnd ( "saveUserProfile" );
          return EvEventCodes.Ok;
        }

        if ( this.AdapterObjects.Settings.AdsGroupName == null
       || this.Session.AdsCustomerGroup.Name == null
       || this.Session.AdsEnabled == false )
        {
          this.LogDebug ( "ADS not enabled so Email notificaton not needed." );
          this.LogMethodEnd ( "saveUserProfile" );
          return EvEventCodes.Ok;
        }
        //
        // Set the email notification.
        //
        EvEmail.EmailStatus emStatus = EvEmail.EmailStatus.Ok;

        if ( isANewUser == true )
        {
          this.LogDebug ( "Sending Introductory Email" );
          emStatus = this.sendUserNotificationEmail ( EvStaticContentTemplates.NotiificationTypes.Introductory_Email );
        }
        else
        {
          this.LogDebug ( "Sending Password update Email" );
          emStatus = this.sendUserNotificationEmail ( EvStaticContentTemplates.NotiificationTypes.Update_Password_Email );
        }

        // 
        // if an error state is returned create log the event.
        // 
        if ( emStatus != EvEmail.EmailStatus.Email_Sent
          && emStatus != EvEmail.EmailStatus.Ok )
        {
          switch ( emStatus )
          {
            case EvEmail.EmailStatus.No_Reciever_Addresses:
              {
                this.ErrorMessage = EdLabels.EmailNotification_Receive_Address_Error_Message;
                break;
              }
            case EvEmail.EmailStatus.No_SMTP_Url:
              {
                this.ErrorMessage = EdLabels.EmailNotification_Configurtion_Error_Message;
                break;
              }
            case EvEmail.EmailStatus.Email_Send_Request_Failed:
            default:
              {
                this.ErrorMessage = EdLabels.EmailNotification_Send_Failure_Error_Message;
                break;
              }
          }

          string stEvent = "Email notitification error: " + emStatus;

          this.LogError ( EvEventCodes.Business_Logic_Email_Failure_Error, stEvent );

          this.LogMethodEnd ( "saveUserProfile" );
          return EvEventCodes.Business_Logic_Email_Failure_Error;

        }//END email erorr handling.

        this.LogMethodEnd ( "saveUserProfile" );
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

      this.LogMethodEnd ( "saveUserProfile" );

      return EvEventCodes.Database_Record_Update_Error;

    }//END saveUserProfile method

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Update ADS group method.

    // ==================================================================================
    /// <summary>
    /// THis method retrieves the ADS customer group object.
    /// </summary>
    /// <returns>EvEventCodes</returns>
    //  ----------------------------------------------------------------------------------
    public EvEventCodes getAdsCustomerGroup ( )
    {
      this.LogMethod ( "getAdsCustomerGroup" );
      this.LogDebug ( "Customer Group Name: " +
        this.AdapterObjects.Settings.AdsGroupName );

      //
      // if ADS is not enabled then set the AdsCustomerGroup to null 
      // This will disable ADS access.
      //
      if ( this.Session.AdsEnabled == false )
      {
        this.Session.AdsCustomerGroup = null;
        this.LogDebug ( "Active Directory is NOT enabled." );
        return EvEventCodes.Active_Directory_Not_Enabled;
      }

      if (  this.AdapterObjects.Settings.AdsGroupName == String.Empty )
      {
        return EvEventCodes.Active_Directory_Group_Not_Found;
      }

      if ( this.Session.AdsCustomerGroup != null )
      {
        this.LogValue ( "Customer Group Name: " +
           this.AdapterObjects.Settings.AdsGroupName + " EXISTS." );

        if ( this.Session.AdsCustomerGroup.Name ==  this.AdapterObjects.Settings.AdsGroupName )
        {
          this.LogMethodEnd ( "getAdsCustomerGroup" );
          return EvEventCodes.Ok;
        }
      }

      //
      // Initialise the ADS services
      //
      Evado.ActiveDirectoryServices.EvAdsProfiles adsProfiles = new ActiveDirectoryServices.EvAdsProfiles ( );
      ActiveDirectoryServices.EvAdsGroupProfile resultGroup = new ActiveDirectoryServices.EvAdsGroupProfile ( );
      //adsProfiles .DebugOn = this.DebugOn;

      //
      // Get the customer's Group name.
      //
      resultGroup = adsProfiles.GetGroup (
         this.AdapterObjects.Settings.AdsGroupName );

      //
      // If the user does not exist add them as a new user.
      //
      if ( resultGroup == null )
      {
        this.LogValue ( "result group null " );

        this.LogMethodEnd ( "getAdsCustomerGroup" );

        return EvEventCodes.Active_Directory_Group_Not_Found;
      }

      this.Session.AdsCustomerGroup = resultGroup;

      this.LogValue ( "ADS RETURNED VALUES:" );
      this.LogValue ( "adUserProfile.DisplayName: " + this.Session.AdsCustomerGroup.DisplayName );
      this.LogValue ( "adUserProfile.Name: " + this.Session.AdsCustomerGroup.Name );
      this.LogValue ( "adUserProfile.SamAccountName: " + this.Session.AdsCustomerGroup.SamAccountName );

      this.LogMethodEnd ( "getAdsCustomerGroup" );


      return EvEventCodes.Ok;

    }//ENd getAdsCustomerGroup method

    #endregion

    #region Update ADS User process

    // ==================================================================================
    /// <summary>
    /// THis method updates the ADS user details.
    /// </summary>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    public EvEventCodes updateAdUserProfile ( )
    {
      this.LogMethod ( "updateAdUserProfile" );
      this.LogDebug ( "this.Session.AdsEnabled: " + this.Session.AdsEnabled );
      //
      // Skip update if a new user.
      //
      if ( this.Session.AdsEnabled == false )
      {
        this.LogValue ( "ADS disabled" );
        this.LogMethodEnd ( "updateAdUserProfile" );
        return EvEventCodes.Ok;
      }

      //
      // Skip update if a new user.
      //
      if ( this.AdapterObjects.Settings.AdsGroupName == null
        || this.Session.AdsCustomerGroup.Name == null )
      {
        this.LogValue ( "Debug Authenication or AD Group is null" );
        this.LogMethodEnd ( "updateAdUserProfile" );
        return EvEventCodes.Ok;
      }
      this.LogDebug ( "AdminUserProfile.UserId: " + this.Session.AdminUserProfile.UserId );
      this.LogDebug ( "AdminUserProfile.Password: " + this.Session.AdminUserProfile.Password );
      this.LogDebug ( "AdminUserProfile.GivenName: " + this.Session.AdminUserProfile.GivenName );
      this.LogDebug ( "AdminUserProfile.FamilyName: " + this.Session.AdminUserProfile.FamilyName );
      this.LogDebug ( "AdsCustomerGroup.Name: " + this.Session.AdsCustomerGroup.Name );

      //
      // Initialise the ADS services
      //
      Evado.ActiveDirectoryServices.EvAdsProfiles adsProfiles = new ActiveDirectoryServices.EvAdsProfiles ( );
      ActiveDirectoryServices.EvAdsUserProfile outUser = new ActiveDirectoryServices.EvAdsUserProfile ( );
      ActiveDirectoryServices.EvAdsGroupProfile customerGroup = new ActiveDirectoryServices.EvAdsGroupProfile ( );
      bool newUser = false;

      this.LogValue ( "domain name: " + adsProfiles.DomainName );

      //
      // Attempt to retrieve the user profile.
      //
      Evado.ActiveDirectoryServices.EvAdsUserProfile adUserProfile =
        adsProfiles.GetUser ( this.Session.AdminUserProfile.UserId );

      //
      // If the user does not exist add them as a new user.
      //
      if ( adUserProfile == null )
      {
        this.LogValue ( "user null: initialise a new user." );
        //
        // create the default password.
        //
        this.createDefaultPassword ( );

        adUserProfile = new ActiveDirectoryServices.EvAdsUserProfile ( );
        newUser = true;
        adUserProfile.UserId = this.Session.AdminUserProfile.UserId;
        adUserProfile.SamAccountName = this.Session.AdminUserProfile.UserId;
        adUserProfile.UserPrincipalName = adUserProfile.UserId + "@" + adsProfiles.DomainName;
        adUserProfile.AddEvGroup ( this.Session.AdsCustomerGroup );
      }

      this.addCustomerGroup ( adUserProfile );

      adUserProfile.GivenName = this.Session.AdminUserProfile.GivenName;
      adUserProfile.Surname = this.Session.AdminUserProfile.FamilyName;
      adUserProfile.DisplayName = this.Session.AdminUserProfile.CommonName;
      adUserProfile.EmailAddress = this.Session.AdminUserProfile.EmailAddress;
      adUserProfile.Password = this.Session.AdminUserProfile.Password;
      adUserProfile.Enabled = true;
      string description = adUserProfile.Description;

      if ( adUserProfile.Description != String.Empty )
      {
        adUserProfile.Description += "\r\n ";
      }

      adUserProfile.Description +=
        "Updated by " + this.Session.AdminUserProfile.CommonName + " on " + DateTime.Now.ToString ( "dd-MMM-yyyy HH:mm" );

      if ( this.Session.UserProfile.Password != String.Empty )
      {
        adUserProfile.Password = this.Session.AdminUserProfile.Password;
      }

      //
      // update the active directory object.
      //
      ActiveDirectoryServices.EvAdsCallResult result = adsProfiles.SaveItem ( adUserProfile, newUser, out outUser );

      // 
      // get the debug log.
      // 
      this.LogValue ( "adsProfiles.ApplicationLog:" + adsProfiles.Log );

      if ( result != ActiveDirectoryServices.EvAdsCallResult.Success )
      {
        this.LogValue ( "EvAdsCallResult: " + result );
        this.LogMethodEnd ( "updateAdUserProfile" );

        return EvEventCodes.Active_Directory_General_Error;
      }

      this.LogMethodEnd ( "updateAdUserProfile" );

      return EvEventCodes.Ok;

    }//ENd updateAdUserProfile method

    // ==================================================================================
    /// <summary>
    /// THis method generates a default user password.
    /// </summary>
    //  ----------------------------------------------------------------------------------
    private void createDefaultPassword ( )
    {
      this.LogMethod ( "createDefaultPassword" );
      // 
      // Initialise the update variables.
      // 
      string password = String.Empty;

      if ( this.Session.AdminUserProfile.GivenName.Length > 1
        && this.Session.AdminUserProfile.FamilyName.Length > 1 )
      {
        password = this.Session.SelectedUserType.ToString() [ 0 ].ToString ( ).ToUpper ( );
        password += this.Session.SelectedUserType.ToString() [ 1 ].ToString ( ).ToLower ( );

        password += this.Session.AdminUserProfile.GivenName [ 0 ].ToString ( ).ToUpper ( );
        password += this.Session.AdminUserProfile.FamilyName [ 0 ].ToString ( ).ToLower ( );
        password += "-" + DateTime.Now.ToString ( "yyMMdd" );
      }
      else
      {
        if ( this.Session.AdminUserProfile.FamilyName.Length > 2 )
        {
          password = this.Session.SelectedUserType.ToString() [ 0 ].ToString ( ).ToUpper ( );
          password += this.Session.SelectedUserType.ToString() [ 1 ].ToString ( ).ToLower ( );

          password = this.Session.AdminUserProfile.FamilyName [ 0 ].ToString ( ).ToUpper ( );
          password += this.Session.AdminUserProfile.FamilyName [ 1 ].ToString ( ).ToLower ( );
          password += "-" + DateTime.Now.ToString ( "yyMMdd" );
        }
        else
        {
          password = this.Session.SelectedUserType.ToString() [ 0 ].ToString ( ).ToUpper ( );
          password += this.Session.SelectedUserType.ToString() [ 1 ].ToString ( ).ToLower ( );
          password += this.Session.SelectedUserType.ToString() [ 2 ].ToString ( ).ToLower ( );
          password += this.Session.SelectedUserType.ToString() [ 3 ].ToString ( ).ToLower ( );
          password += "-" + DateTime.Now.ToString ( "yyMMdd" );
        }
      }

      this.Session.AdminUserProfile.Password = password;

      this.LogValue ( "AdminUserProfile.Password: " + this.Session.AdminUserProfile.Password );

      this.LogMethodEnd ( "createDefaultPassword" );

    }//END createDefaultPassword method

    // ==================================================================================
    /// <summary>
    /// THis method generates a default user password.
    /// </summary>
    /// <param name="AdUserProfile">Evado.ActiveDirectoryServices.EvAdsUserProfile</param>
    //  ----------------------------------------------------------------------------------
    private void addCustomerGroup (
      Evado.ActiveDirectoryServices.EvAdsUserProfile AdUserProfile )
    {
      this.LogMethod ( "addCustomerGroup" );
      this.LogValue ( "adUserProfile.SamAccountName: " + AdUserProfile.SamAccountName );
      bool hasCustomerGroup = false;

      //
      // get the customer group from the users groups.
      //
      if ( AdUserProfile.EvGroups.Count > 0 )
      {
        //
        // Iterate through the user's groups.
        //
        foreach ( Evado.ActiveDirectoryServices.EvAdsGroupProfile group in AdUserProfile.EvGroups )
        {
          if ( group == null )
          {
            continue;
          }
          this.LogValue ( "profile.Name: " + group.Name );
          this.LogValue ( "Customer group found" );
          hasCustomerGroup = true;
        }
      }

      //
      // Add a group if is not present in the member list.
      //
      if ( ( AdUserProfile.EvGroups.Count == 0
        || hasCustomerGroup == false )
        && (  this.AdapterObjects.Settings.AdsGroupName != null ) )
      {
        AdUserProfile.EvGroups.Add ( this.Session.AdsCustomerGroup );
        this.LogValue ( "ADS Customer group added" );
      }

      this.LogMethodEnd ( "addCustomerGroup" );

    }//END addCustomerGroup method

    // ==================================================================================
    /// <summary>
    /// THis method genrates and send a user notification email. Send to new users and 
    /// when a user's password is updated by the administrator.
    /// </summary>
    //  ----------------------------------------------------------------------------------
    public EvEmail.EmailStatus sendUserNotificationEmail (
      EvStaticContentTemplates.NotiificationTypes Notification )
    {
      this.LogMethod ( "sendUserNotificationEmail" );
      this.LogDebug ( "Notification: " + Notification );
      this.LogDebug ( "ApplicationParameters.ApplicationUrl: " + this.AdapterObjects.ApplicationUrl );
      this.LogDebug ( "SupportEmailAddress: " + this.AdapterObjects.SupportEmailAddress );
      this.LogDebug ( "AdminUserProfile.UserId: " + this.Session.AdminUserProfile.UserId );
      this.LogDebug ( "AdminUserProfile.Password: " + this.Session.AdminUserProfile.Password );
      this.LogDebug ( "AdminUserProfile.EmailAddress: " + this.Session.AdminUserProfile.EmailAddress );
      // 
      // Initialise the update variables.
      // 
      string EmailTitle = String.Empty;
      string EmailBody = String.Empty;
      EvEmail email = new EvEmail ( );
      EvEmail.EmailStatus emailStatus = EvEmail.EmailStatus.Null;

      //
      // Validate that the necessary parametes exist.
      //
      if ( this.AdapterObjects.ApplicationUrl == String.Empty
        || this.Session.AdminUserProfile.EmailAddress == String.Empty
        || this.AdapterObjects.SupportEmailAddress == String.Empty )
      {
        this.LogValue ( "Parameters Missing" );
        this.LogMethodEnd ( "sendUserNotificationEmail" );
        return EvEmail.EmailStatus.Parameters_Missing_Error;
      }

      switch ( Notification )
      {
        case EvStaticContentTemplates.NotiificationTypes.Update_Password_Email:
          {
            EmailTitle = this.AdapterObjects.ContentTemplates.UpdatePasswordEmail_Title;
            EmailBody = this.AdapterObjects.ContentTemplates.UpdatePasswordEmail_Body;
            break;
          }
        case EvStaticContentTemplates.NotiificationTypes.Reset_Password_Email:
          {
            EmailTitle = this.AdapterObjects.ContentTemplates.ResetPasswordEmail_Title;
            EmailBody = this.AdapterObjects.ContentTemplates.ResetPasswordEmail_Body;
            break;
          }
        case EvStaticContentTemplates.NotiificationTypes.Password_Change_Email:
          {
            EmailTitle = this.AdapterObjects.ContentTemplates.PasswordConfirmationEmail_Title;
            EmailBody = this.AdapterObjects.ContentTemplates.PasswordConfirmationEmail_Body;
            break;
          }
        case EvStaticContentTemplates.NotiificationTypes.Introductory_Email:
        default:
          {
            EmailTitle = this.AdapterObjects.ContentTemplates.IntroductoryEmail_Title;
            EmailBody = this.AdapterObjects.ContentTemplates.IntroductoryEmail_Body;
            break;
          }
      }//End switch statementf

      EmailTitle = EmailTitle.Replace ( "\r\n\r\n", "\r\n \r\n" );
      EmailTitle = EmailTitle.Replace ( EvcStatics.TEXT_SUBSITUTION_FIRST_NAME,
        this.Session.AdminUserProfile.GivenName );

      EmailTitle = EmailTitle.Replace ( EvcStatics.TEXT_SUBSITUTION_FAMILY_NAME,
        this.Session.AdminUserProfile.FamilyName );

      EmailTitle = EmailTitle.Replace ( EvcStatics.TEXT_SUBSITUTION_ADAPTER_TITLE,
         this.AdapterObjects.Settings.Title );

      EmailBody = EmailBody.Replace ( "\r\n\r\n", "\r\n \r\n" );
      EmailBody = EmailBody.Replace ( EvcStatics.TEXT_SUBSITUTION_FIRST_NAME,
        this.Session.AdminUserProfile.GivenName );
      EmailBody = EmailBody.Replace ( EvcStatics.TEXT_SUBSITUTION_FAMILY_NAME,
        this.Session.AdminUserProfile.FamilyName );

      EmailBody = EmailBody.Replace ( EvcStatics.TEXT_SUBSITUTION_EMAIL_ADDRESS,
        this.Session.AdminUserProfile.EmailAddress );

      EmailBody = EmailBody.Replace ( EvcStatics.TEXT_SUBSITUTION_USER_ID,
        this.Session.AdminUserProfile.UserId );

      EmailBody = EmailBody.Replace ( EvcStatics.TEXT_SUBSITUTION_PASSWORD,
        this.Session.AdminUserProfile.Password );

      EmailBody = EmailBody.Replace ( EvcStatics.TEXT_SUBSITUTION_ORG_ID,
        this.Session.SelectedUserType.ToString() );

      EmailBody = EmailBody.Replace ( EvcStatics.TEXT_SUBSITUTION_ORG_NAME,
        this.Session.AdminUserProfile.OrganisationName );

      EmailBody = EmailBody.Replace ( EvcStatics.TEXT_SUBSITUTION_ADAPTER_TITLE,
        this.AdapterObjects.Settings.Title );


      EmailBody = EmailBody.Replace ( EvcStatics.TEXT_SUBSITUTION_PASSWORD_RESET_URL,
        this.AdapterObjects.PasswordResetUrl );

      EmailBody = EmailBody.Replace ( EvcStatics.TEXT_SUBSITUTION_DATE_STAMP,
        DateTime.Now.ToLongDateString ( ) + " at " + DateTime.Now.ToShortTimeString ( ) );


      this.LogValue ( "EmailTitle: " + EmailTitle );

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
      EmailBody = markDown.Transform ( EmailBody );

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
        this.AdapterObjects.SupportEmailAddress,
        this.Session.AdminUserProfile.EmailAddress,
        String.Empty );

      this.LogValue ( "Email DebugLog: " + email.Log );

      //
      // Log email send error.
      //
      if ( emailStatus != EvEmail.EmailStatus.Email_Sent )
      {
        this.LogError ( EvEventCodes.Database_Record_Update_Error,
          "User Notificatin Email Event Status: " + emailStatus
          + "\r\n" + email.Log );

        this.LogClass ( email.Log );
      }

      this.LogMethodEnd ( "sendUserNotificationEmail" );
      return emailStatus;

    }//END sendUserNotificationEmail method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
  }
}//END namespace