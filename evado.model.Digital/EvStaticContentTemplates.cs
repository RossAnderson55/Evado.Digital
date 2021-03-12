/***************************************************************************************
 * <copyright file="model\EvActivity.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2021 EVADO HOLDING PTY. LTD..  All rights reserved.
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
 ****************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

namespace Evado.Model.Digital
{

  /// <summary>
  /// This class defines the data model for  trial or registry schedule milestone activities. 
  /// </summary>
  [Serializable]
  public class EvStaticContentTemplates : EvHasSetValue<EvStaticContentTemplates.ClassFieldNames>
  {
    #region enumerations

    /// <summary>
    /// This enumeration list defines the  filenames for data update or extraction.
    /// </summary>
    public enum NotiificationTypes
    {
      /// <summary>
      /// This enumeration defines the Null Value or no selection state
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines the introductor email notification.
      /// </summary>
      Introductory_Email,

      /// <summary>
      /// This enumeration defines the password updated email notification
      /// </summary>
      Update_Password_Email,

      /// <summary>
      /// This enumeration defines the password reset notifications.
      /// </summary>
      Reset_Password_Email,

      /// <summary>
      /// This enumeration defines the password change  notification.
      /// </summary>
      Password_Change_Email
    }

    /// <summary>
    /// This enumeration list defines the  filenames for data update or extraction.
    /// </summary>
    public enum ClassFieldNames
    {
      /// <summary>
      /// This enumeration defines the Null Value or no selection state
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines the intdocutory email title field names.
      /// </summary>
      Introductory_Email_Title,

      /// <summary>
      /// This enumeration defines the introductionry email body field names.
      /// </summary>
      Introductory_Email_Body,

      /// <summary>
      /// This enumeration defines the password update email title field names.
      /// </summary>
      Update_Password_Email_Title,

      /// <summary>
      /// This enumeration defines the password update email body field names.
      /// </summary>
      Update_Password_Email_Body,

      /// <summary>
      /// This enumeration defines the password reset email title  field names.
      /// </summary>
      Reset_Password_Email_Title,

      /// <summary>
      /// This enumeration defines the password reset email body field names.
      /// </summary>
      Reset_Password_Email_Body,

      /// <summary>
      /// This enumeration defines the password change notification title field names.
      /// </summary>
      Password_Confirmation_Email_Title,

      /// <summary>
      /// This enumeration defines the password change notification body field names.
      /// </summary>
      Password_Confirmation_Email_Body,

      /// <summary>
      /// This enumeration defins the demonstration registration instuctions.
      /// </summary>
      DemoRegistrationInstuctions,

      /// <summary>
      /// This enumeration defins the demonstration registration confirmation.
      /// </summary>
      DemoRegistrationConfirmation,

      /// <summary>
      /// This enumeration defins the demonstration registration error.
      /// </summary>
      DemoRegistrationError

    }

    #endregion

    #region class consts


    #endregion

    #region Properties

    /// <summary>
    /// This property contains the ResetPasswordEmail_Title for new user introductor email.
    /// </summary>
    public String IntroductoryEmail_Title { get; set; }

    /// <summary>
    /// This property contains the markdown body for new user introductor email.
    /// </summary>
    public String IntroductoryEmail_Body { get; set; }

    /// <summary>
    /// This property contains the ResetPasswordEmail_Title for new user reset email.
    /// </summary>
    public String ResetPasswordEmail_Title { get; set; }

    /// <summary>
    /// This property contains the markdown body for new user reset email.
    /// </summary>
    public String ResetPasswordEmail_Body { get; set; }

    /// <summary>
    /// This property contains the update password email title.
    /// </summary>
    public String UpdatePasswordEmail_Title { get; set; }

    /// <summary>
    /// This property contains the markdown body for update password
    /// </summary>
    public String UpdatePasswordEmail_Body { get; set; }

    /// <summary>
    /// This property contains the ResetPasswordEmail_Title for new user reset email.
    /// </summary>
    public String PasswordConfirmationEmail_Title { get; set; }

    /// <summary>
    /// This property contains the markdown body for new user reset email.
    /// </summary>
    public String PasswordConfirmationEmail_Body { get; set; }

    /// <summary>
    /// This property contains the markdown demonstration registation instructions.
    /// </summary>
    public String DemoRegistrationInstuctions { get; set; }

    /// <summary>
    /// This property contains the markdown demonstation registration confirmation.
    /// </summary>
    public String DemoRegistrationConfirmation { get; set; }

    /// <summary>
    /// This property contains the markdown demonstation registration error.
    /// </summary>
    public String DemoRegistrationError { get; set; }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region public methods.


    //  ================================================================================
    /// <summary>
    /// Sets the value of this activity class field name. Validate the format of the
    /// value. 
    /// </summary>
    /// <param name="fieldName">ActivityClassFieldNames: Name of the field to be setted.</param>
    /// <param name="value">String: value to be setted</param>
    /// <returns>EvEventCodes: indicating the successful update of the property value.</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Switch the fieldName and update value for the property defined by the activity field names
    /// 
    /// 2. Return casting error, if field name type is empty
    /// </remarks>
    //  --------------------------------------------------------------------------------
    public EvEventCodes setValue ( ClassFieldNames fieldName, String value )
    {
      //
      // Switch the FieldName based on the activity field names
      //
      switch ( fieldName )
      {
        case ClassFieldNames.Introductory_Email_Title:
          {
            this.IntroductoryEmail_Title = value;
            break;
          }
        case ClassFieldNames.Introductory_Email_Body:
          {
            this.IntroductoryEmail_Body = value;
            break;
          }
        case ClassFieldNames.Reset_Password_Email_Title:
          {
            this.ResetPasswordEmail_Title = value;
            break;
          }
        case ClassFieldNames.Reset_Password_Email_Body:
          {
            this.ResetPasswordEmail_Body = value;
            break;
          }
        case ClassFieldNames.Update_Password_Email_Title:
          {
            this.UpdatePasswordEmail_Title = value;
            break;
          }
        case ClassFieldNames.Update_Password_Email_Body:
          {
            this.UpdatePasswordEmail_Body = value;
            break;
          }
        case ClassFieldNames.Password_Confirmation_Email_Title:
          {
            this.PasswordConfirmationEmail_Title = value;
            break;
          }
        case ClassFieldNames.Password_Confirmation_Email_Body:
          {
            this.PasswordConfirmationEmail_Body = value;
            break;
          }
        case ClassFieldNames.DemoRegistrationInstuctions:
          {
            this.DemoRegistrationInstuctions = value;
            break;
          }
        case ClassFieldNames.DemoRegistrationConfirmation:
          {
            this.DemoRegistrationConfirmation = value;
            break;
          }
        case ClassFieldNames.DemoRegistrationError:
          {
            this.DemoRegistrationError = value;
            break;
          }
          
      }// End switch field name

      return EvEventCodes.Ok;

    }//End setValue method.

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

  } //END EvActivity class

} //END namespace Evado.Model
