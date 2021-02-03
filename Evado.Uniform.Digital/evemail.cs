/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\EvEmail.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2020 EVADO HOLDING PTY. LTD..  All rights reserved.
 *     
 *      The use and distribution terms for this software are contained in the file
 *      named license.txt, which can be found in the root of this distribution.
 *      By using this software in any fashion, you are agreeing to be bound by the
 *      terms of this license.
 *     
 *      You must not remove this notice, or any other, from this software.
 *     
 * </copyright>
 * 
 * Description: 
 *  This class contains the EvEmail business object.
 *
 ****************************************************************************************/
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.IO;
using System.Data;
using System.Net.Mail;

using Evado.Model;

namespace Evado.UniForm.Digital
{
  public class EvEmail
  {
    #region Class enumerators objects.

    /// <summary>
    /// EmailAlertStatus defines the email alert status.
    /// </summary>
    public enum EmailStatus
    {
      /// <summary>
      /// nothing set.
      /// </summary>
      Null,

      /// <summary>
      /// Return Ok
      /// </summary>
      Ok,

      /// <summary>
      /// The style sheet path is empty 
      /// </summary>
      Xsl_Style_Sheet_Path_Empty,

      /// <summary>
      /// The regulatory report is empty or null
      /// </summary>
      Regulatory_Report_Empty,

      /// <summary>
      /// No SMTP server
      /// </summary>
      No_SMTP_Url,

      /// <summary>
      /// No email addresses for sender.
      /// </summary>
      No_Sender_Address,

      /// <summary>
      /// No email addresses for recievers.
      /// </summary> 
      No_Reciever_Addresses,

      /// <summary>
      /// Email send request failed.
      /// </summary>
      Email_Send_Request_Failed,

      /// <summary>
      /// Email send request failed.
      /// </summary>
      Email_Event_Raised,

      /// <summary>
      /// Email send request failed.
      /// </summary>
      Parameters_Missing_Error,


      /// <summary>
      /// Email sent 
      /// </summary>
      Email_Sent,
    }


    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class initiation.

    // ==================================================================================
    /// <summary>
    /// Object initialisation method
    /// </summary>
    // ---------------------------------------------------------------------------------
    public EvEmail ( )
    { }

    // ==================================================================================
    /// <summary>
    /// Object initialisation method
    /// </summary>
    /// <param name="SmtpServer">SMTP Server Url</param>
    /// <param name="SmtpServerPort">SMTP Server port</param>
    // ---------------------------------------------------------------------------------
    public EvEmail (
      String SmtpServer,
      int SmtpServerPort )
    {
      this._DebugLog.AppendLine ( EvStatics.CONST_METHOD_START + "Evado.Model.EvMail.Email" );
      this._DebugLog.AppendLine ( "SmtpServer: " + SmtpServer );
      this._DebugLog.AppendLine ( "SmtpServerPort : " + SmtpServerPort );
      this._SmtpServer = SmtpServer;
      this._SmtpServerPort = SmtpServerPort;
    }

    // ==================================================================================
    /// <summary>
    /// Object initialisation method
    /// </summary>
    /// <param name="SmtpServer">SMTP Server Url</param>
    /// <param name="SmtpServerPort">SMTP Server port</param>
    /// <param name="SmtpUserId">SMTP User id</param>
    /// <param name="SmtpPassword">SMTP user password</param>
    // ---------------------------------------------------------------------------------
    public EvEmail (
      String SmtpServer,
      int SmtpServerPort,
      String SmtpUserId,
      String SmtpPassword )
    {
      this._DebugLog.AppendLine ( EvStatics.CONST_METHOD_START + "Evado.Model.EvMail.Email" );
      this._DebugLog.AppendLine ( "SmtpServer: " + SmtpServer );
      this._DebugLog.AppendLine ( "SmtpServerPort : " + SmtpServerPort );
      this._DebugLog.AppendLine ( "SmtpUserId: " + SmtpUserId );
      this._DebugLog.AppendLine ( "SmtpPassword: " + SmtpPassword );

      this._SmtpServer = SmtpServer;
      this._SmtpServerPort = SmtpServerPort;
      this._SmtpUserId = SmtpUserId;
      this._SmtpPassword = SmtpPassword;
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class global objects.

    protected string _SmtpServer = "LocalHost";
    /// <summary>
    /// This class property contains SmtpServer for the object.
    /// </summary>
    public string SmtpServer
    {
      set
      {
        this._SmtpServer = value;
      }
    }

    protected int _SmtpServerPort = 587;
    /// <summary>
    /// This class property contains SmtpServerPort for the object.
    /// </summary>
    public int SmtpServerPort
    {
      set
      {
        this._SmtpServerPort = value;
      }
    }

    protected bool _EnableSsl = false;
    /// <summary>
    /// This class property enables SSL connection the SMTP server.
    /// </summary>
    public bool EnableSsl
    {
      set
      {
        this._EnableSsl = value;
      }
    }

    protected string _SmtpUserId = String.Empty;
    /// <summary>
    /// This class property contains SmtpServer for the object.
    /// </summary>
    public string SmtpUserId
    {
      set
      {
        this._SmtpUserId = value;
      }
    }

    protected string _SmtpPassword = String.Empty;
    /// <summary>
    /// This class property contains SmtpServer for the object.
    /// </summary>
    public string SmtpPassword
    {
      set
      {
        this._SmtpPassword = value;
      }
    }

    protected System.Text.StringBuilder _DebugLog = new System.Text.StringBuilder ( );
    /// <summary>
    /// This class property contains status for the object.
    /// </summary>
    public string Log
    {
      get
      {
        return _DebugLog.ToString ( );
      }
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Send Email methods
    //  ==================================================================================
    /// <summary>
    ///
    ///  This method sends an email to the recipants in the email address list.
    /// 
    /// </summary>
    /// <param name="Subject">The email subject string</param>
    /// <param name="PlainBody">The email body</param>
    /// <param name="SenderEmailAddress">The sender's email address</param>
    /// <param name="RecipantEmailAddresses">Delimited string of recipant email addresses.</param>
    /// <param name="AttachmentDirectoryPath">Directory path to the attachment if it exists.</param>
    /// <returns>EmailStatus value</returns>
    //  ---------------------------------------------------------------------------------
    public EmailStatus sendPlainEmail (
      String Subject,
      String PlainBody,
      String SenderEmailAddress,
      String RecipantEmailAddresses,
      String AttachmentDirectoryPath )
    {
      this._DebugLog.AppendLine ( EvStatics.CONST_METHOD_START + "Evado.Model.EvMail.sendPlainEmail" );
      this._DebugLog.AppendLine ( "SmtpServer: " + this._SmtpServer );
      this._DebugLog.AppendLine ( "SmtpServerPort : " + this._SmtpServerPort );
      this._DebugLog.AppendLine ( "SmtpUserId: " + this._SmtpUserId );
      this._DebugLog.AppendLine ( "SmtpPassword: " + this._SmtpPassword );
      this._DebugLog.AppendLine ( "Subject: " + Subject );
      this._DebugLog.AppendLine ( "PlainBody: " + PlainBody );
      this._DebugLog.AppendLine ( "SenderEmailAddress: " + SenderEmailAddress );
      this._DebugLog.AppendLine ( "EmailAddreses: " + RecipantEmailAddresses );
      this._DebugLog.AppendLine ( "AttachmentDirectoryPath: " + AttachmentDirectoryPath );

      try
      {
        //
        // Validate that the are the settings for the SMTP server.
        //
        if ( this._SmtpServer == String.Empty )
        {
          return EmailStatus.No_SMTP_Url;
        }
        //
        // Validate that the are the settings for the SMTP server.
        //
        if ( RecipantEmailAddresses == String.Empty )
        {
          this._DebugLog.AppendLine ( EvStatics.Enumerations.enumValueToString ( EmailStatus.No_Reciever_Addresses ) );

          return EmailStatus.No_Reciever_Addresses;
        }

        //
        // If the sender's email address is empty put in a dummy address.
        //
        if ( SenderEmailAddress == String.Empty )
        {
          this._DebugLog.AppendLine ( EvStatics.Enumerations.enumValueToString ( EmailStatus.No_Sender_Address ) );

          return EmailStatus.No_Sender_Address;
        }

        //
        // Get the list of email addresses.
        //
        RecipantEmailAddresses = RecipantEmailAddresses.Replace ( "(", "<" );
        RecipantEmailAddresses = RecipantEmailAddresses.Replace ( ")", ">" );
        String [ ] arrRecipantEmailAddresses = RecipantEmailAddresses.Split ( ';' );
        MailAddress fromAddress = new MailAddress ( this._SmtpUserId );
        MailAddressCollection toAddresses = new MailAddressCollection ( );
        MailAddress replyToAddress = new MailAddress ( SenderEmailAddress );

        this._DebugLog.AppendLine ( "arrRecipantEmailAddresses length: " + arrRecipantEmailAddresses.Length );
        //
        // Create the to address list.
        //
        foreach ( String recipantEmailAddress in arrRecipantEmailAddresses )
        {
          if ( recipantEmailAddress == String.Empty )
          {
            this._DebugLog.AppendLine ( "Empty recipant" );
            continue;
          }

          this._DebugLog.AppendLine ( "recipantEmailAddress: " + recipantEmailAddress );
          if ( recipantEmailAddress.Contains ( ">" ) == true )
          {
            string stRecipantEmailAddress = recipantEmailAddress.Replace ( ")", String.Empty );
            stRecipantEmailAddress = stRecipantEmailAddress.Replace ( "(", "<" );
            stRecipantEmailAddress = stRecipantEmailAddress.Replace ( ">", String.Empty );
            int index = stRecipantEmailAddress.IndexOf ( '<' );
            string recipantntName = stRecipantEmailAddress.Substring ( 0, index ).Trim ( );
            string recipantntAddress = stRecipantEmailAddress.Substring ( index + 1 ).Trim ( );

            this._DebugLog.AppendLine ( "name: " + recipantntName + ", address: " + recipantntAddress );

            MailAddress toAddress = new MailAddress ( recipantntAddress.Trim ( ), recipantntName.Trim ( ) );
            toAddresses.Add ( toAddress );
          }
          else
          {
            MailAddress toAddress = new MailAddress ( recipantEmailAddress.Trim ( ), recipantEmailAddress.Trim ( ) );
            toAddresses.Add ( toAddress );

          }
        }//END interation loop

        this._DebugLog.AppendLine ( "toAddresses: " + toAddresses.Count );

        if ( toAddresses.Count == 0 )
        {
          return EmailStatus.No_Reciever_Addresses;
        }

        this._DebugLog.AppendLine ( "Creating Email message." );

        //
        // Initialise the email message content
        //
        MailMessage message = new MailMessage ( fromAddress, toAddresses [ 0 ] );
        message.ReplyToList.Add ( replyToAddress );
        message.Subject = Subject;
        message.IsBodyHtml = false;
        message.Body = PlainBody;

        //
        // Add the to recipant email address list.
        //
        for ( int i = 1; i < toAddresses.Count; i++ )
        {
          message.To.Add ( toAddresses [ i ] );

        }//END interation loop
        this._DebugLog.AppendLine ( "message.To: " + message.To.Count );

        //
        // Get the report and save it in a temporary file.
        //
        if ( AttachmentDirectoryPath != String.Empty )
        {
          //
          // Create  the file attachment for this e-mail message.
          //
          Attachment attachment = new Attachment ( AttachmentDirectoryPath );

          // Add the file attachment to this e-mail message.
          message.Attachments.Add ( attachment );
          this._DebugLog.AppendLine ( "message.Attachments: " + message.Attachments.Count );
        }

        //
        // Initialise the Smtp Email client
        //
        SmtpClient smtpClient = new SmtpClient ( this._SmtpServer, this._SmtpServerPort );
        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtpClient.EnableSsl = this._EnableSsl;
        this._DebugLog.AppendLine ( "client.Host: " + smtpClient.Host );
        this._DebugLog.AppendLine ( "client.Port: " + smtpClient.Port );
        this._DebugLog.AppendLine ( "client.DeliveryMethod: " + smtpClient.DeliveryMethod );
        this._DebugLog.AppendLine ( "client.EnableSsl: " + smtpClient.EnableSsl );

        //
        // Initialise the email server credentials if it is needed.
        //
        if ( this._SmtpUserId != String.Empty )
        {
          this._DebugLog.AppendLine ( "SmtpUserId: " + this._SmtpUserId );
          this._DebugLog.AppendLine ( "SmtpPassword: " + this._SmtpPassword );

          smtpClient.Credentials = new System.Net.NetworkCredential ( this._SmtpUserId, this._SmtpPassword );
        }

        //
        // Send the email message.
        //
        smtpClient.Send ( message );

        this._DebugLog.AppendLine ( "Email Sent." );

        return EmailStatus.Email_Sent;
      }
      catch ( Exception Ex )
      {
        this._DebugLog.AppendLine ( "Event content:" );
        this._DebugLog.AppendLine ( EvStatics.getException ( Ex ) );

        return EmailStatus.Email_Send_Request_Failed;
      }

    }//END sendEmail method

    //  ==================================================================================
    /// <summary>
    ///
    ///  This method sends an email to the recipants in the email address list.
    /// 
    /// </summary>
    /// <param name="Subject">The email subject string</param>
    /// <param name="HtmlBody">The email html body</param>
    /// <param name="SenderEmailAddress">The sender's email address</param>
    /// <param name="RecipantEmailAddresses">Delimited string of recipant email addresses.</param>
    /// <param name="AttachmentDirectoryPath">Directory path to the attachment if it exists.</param>
    /// <returns>EmailStatus value</returns>
    //  ---------------------------------------------------------------------------------
    public EmailStatus sendEmail (
      String Subject,
      String HtmlBody,
      String SenderEmailAddress,
      String RecipantEmailAddresses,
      String AttachmentDirectoryPath )
    {
      this._DebugLog.AppendLine ( EvStatics.CONST_METHOD_START + "Evado.Model.EvMail.sendEmail" );
      this._DebugLog.AppendLine ( "Subject: " + Subject );
      //this._DebugLog.AppendLine ( "HtmlBody: " + HtmlBody );
      this._DebugLog.AppendLine ( "SenderEmailAddress: " + SenderEmailAddress );
      this._DebugLog.AppendLine ( "RecipantEmailAddresses: " + RecipantEmailAddresses );
      this._DebugLog.AppendLine ( "AttachmentDirectoryPath: " + AttachmentDirectoryPath );
      try
      {
        //
        // Validate that the are the settings for the SMTP server.
        //
        if ( this._SmtpServer == String.Empty )
        {
          this._DebugLog.AppendLine ( EvStatics.Enumerations.enumValueToString ( EmailStatus.No_SMTP_Url ) );
          return EmailStatus.No_SMTP_Url;
        }

        //
        // Validate that the are the settings for the SMTP server.
        //
        if ( RecipantEmailAddresses == String.Empty )
        {
          this._DebugLog.AppendLine ( EvStatics.Enumerations.enumValueToString ( EmailStatus.No_Reciever_Addresses ) );
          return EmailStatus.No_Reciever_Addresses;
        }

        //
        // If the sender's email address is empty put in a dummy address.
        //
        if ( SenderEmailAddress == String.Empty )
        {
          SenderEmailAddress = this._SmtpUserId;
        }

        //
        // Get the list of email addresses.
        //
        MailAddress fromAddress = new MailAddress ( this._SmtpUserId );
        MailAddressCollection recipantAddressList = new MailAddressCollection ( );
        MailAddress replyToAddress = new MailAddress ( this._SmtpUserId );

        //
        // Generate an array of email addresses.
        //
        String [ ] arrRecipantEmailAddresses = RecipantEmailAddresses.Split ( ';' );

        //
        // Create the to address list.
        //
        foreach ( String recipantEmailAddress in arrRecipantEmailAddresses )
        {
          String stRecipantEmailAddress = recipantEmailAddress.Replace ( "(", "<" );
          stRecipantEmailAddress = stRecipantEmailAddress.Replace ( ")", ">" );

          if ( stRecipantEmailAddress.Contains ( ">" ) == true )
          {
            stRecipantEmailAddress = stRecipantEmailAddress.Replace ( ">", String.Empty );
            int index = stRecipantEmailAddress.IndexOf ( '<' );
            string recipantntName = stRecipantEmailAddress.Substring ( 0, index ).Trim ( );
            string recipantntAddress = stRecipantEmailAddress.Substring ( index + 1 ).Trim ( );

            //this._DebugLog.AppendLine ( "name: " + recipantntName + ", address: " + recipantntAddress );

            MailAddress toAddress = new MailAddress ( recipantntAddress, recipantntName );
            recipantAddressList.Add ( toAddress );
          }
          else
          {
            if ( recipantEmailAddress.Length > 0 )
            {
              //this._DebugLog.AppendLine ( "name: " + recipantEmailAddress + ", address: " + recipantEmailAddress );

              MailAddress toAddress = new MailAddress ( recipantEmailAddress, recipantEmailAddress );
              recipantAddressList.Add ( toAddress );
            }
          }

        }//END interation loop

        this._DebugLog.AppendLine ( "toAddresses: " + recipantAddressList.Count );

        if ( recipantAddressList.Count == 0 )
        {
          this._DebugLog.AppendLine ( "No Addressee list." );
          return EmailStatus.No_Reciever_Addresses;
        }

        this._DebugLog.AppendLine ( "Creating Email message." );

        //
        // Initialise the email message content
        //
        MailMessage message = new MailMessage ( fromAddress, recipantAddressList [ 0 ] );
        message.ReplyToList.Add ( replyToAddress );
        message.Subject = Subject;

        message.IsBodyHtml = true;
        message.Body = HtmlBody;

        //
        // Add the to recipant email address list.
        //
        for ( int i = 1; i < recipantAddressList.Count; i++ )
        {
          message.To.Add ( recipantAddressList [ i ] );

        }//END interation loop
        this._DebugLog.AppendLine ( "message.To: " + message.To.Count );

        //
        // Get the report and save it in a temporary file.
        //
        if ( AttachmentDirectoryPath != String.Empty )
        {
          //
          // Create  the file attachment for this e-mail message.
          //
          Attachment attachment = new Attachment ( AttachmentDirectoryPath );

          // Add the file attachment to this e-mail message.
          message.Attachments.Add ( attachment );
          this._DebugLog.AppendLine ( "message.Attachments: " + message.Attachments.Count );
        }

        //
        // Initialise the Smtp Email client
        //
        SmtpClient smtpClient = new SmtpClient ( this._SmtpServer, this._SmtpServerPort );
        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtpClient.EnableSsl = _EnableSsl;
        this._DebugLog.AppendLine ( "client.Host: " + smtpClient.Host );
        this._DebugLog.AppendLine ( "client.Port: " + smtpClient.Port );
        this._DebugLog.AppendLine ( "client.DeliveryMethod: " + smtpClient.DeliveryMethod );
        this._DebugLog.AppendLine ( "client.EnableSsl: " + smtpClient.EnableSsl );

        //
        // Initialise the email server credentials if it is needed.
        //
        if ( this._SmtpUserId != String.Empty )
        {
          this._DebugLog.AppendLine ( "SmtpUserId: " + this._SmtpUserId );
          this._DebugLog.AppendLine ( "SmtpPassword: " + this._SmtpPassword );

          smtpClient.Credentials = new System.Net.NetworkCredential ( this._SmtpUserId, this._SmtpPassword );
        }

        //
        // Send the email message.
        //
        smtpClient.Send ( message );

        this._DebugLog.AppendLine ( "Email Sent." );

        return EmailStatus.Email_Sent;
      }
      catch ( Exception Ex )
      {
        this._DebugLog.AppendLine ( "Event content: " );
        this._DebugLog.AppendLine ( EvStatics.getException ( Ex ) );

        return EmailStatus.Email_Send_Request_Failed;
      }

    }//END sendEmail method

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Static methods

    //  ===================================================================================
    /// <summary>
    /// This method validates an emil address.
    /// </summary>
    /// <param name="email">String: Email address</param>
    /// <returns>bool: True = Email address is valid</returns>
    //  -----------------------------------------------------------------------------------
    public static String formatEmail ( String Name, String EmailAddress )
    {
      String emailAddress = EmailAddress;

      if ( Name != String.Empty )
      {
        emailAddress = Name + " <" + EmailAddress + ")";
      }

      return emailAddress;
    }//END IsValidEmail method

    //  ===================================================================================
    /// <summary>
    /// This method validates an emil address.
    /// </summary>
    /// <param name="email">String: Email address</param>
    /// <returns>bool: True = Email address is valid</returns>
    //  -----------------------------------------------------------------------------------
    public static bool IsValidEmail ( string email )
    {
      try
      {
        var addr = new System.Net.Mail.MailAddress ( email );
        return addr.Address == email;
      }
      catch
      {
        return false;
      }
    }//END IsValidEmail method.

    #endregion

  }//END Mail class
}