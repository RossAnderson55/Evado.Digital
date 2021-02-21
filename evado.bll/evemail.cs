using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.IO;
using System.Data;
using System.Net.Mail;

using Evado.Model;

/// <summary>
/// This class handles the business layer email transmission.
/// </summary>
  public class EvEmail: Evado.Bll.EvBllBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EvEmail ( )
    {
      this.ClassNameSpace = "Evado.Bll.Clinical.EvEmail.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EvEmail ( Evado.Model.Digital.EvClassParameters Settings )
    {
      this.ClassParameter = Settings;
      this.ClassNameSpace = "Evado.Bll.Clinical.EvEmail.";

    }

    // ==================================================================================
    /// <summary>
    /// Object initialisation method
    /// </summary>
    /// <param name="SmtpServer">SMTP Server Url</param>
    /// <param name="SmtpServerPort">SMTP Server port</param>
    // ---------------------------------------------------------------------------------
    public EvEmail(
      String SmtpServer, 
      int SmtpServerPort )
    {
      this._Log.AppendLine ( "Email method. " );
      this._Log.AppendLine ( "SmtpServer: " + SmtpServer );
      this._Log.AppendLine ( "SmtpServerPort : " + SmtpServerPort );
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
      this._Log.AppendLine("Email method. " );
      this._Log.AppendLine ( "SmtpServer: " + SmtpServer );
      this._Log.AppendLine ( "SmtpServerPort : " + SmtpServerPort );
      this._Log.AppendLine ( "SmtpUserId: " + SmtpUserId );
      this._Log.AppendLine ( "SmtpPassword: " + SmtpPassword );

      this._SmtpServer = SmtpServer;
      this._SmtpServerPort = SmtpServerPort;
      this._SmtpUserId = SmtpUserId;
      this._SmtpPassword = SmtpPassword;
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class global properties.

    private string _SmtpServer = "LocalHost";
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

    private int _SmtpServerPort = 587;
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

    private bool _EnableSsl = false;
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

    private string _SmtpUserId = String.Empty;
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

    private string _SmtpPassword = String.Empty;
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

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

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
      this.LogMethod("sendPlainEmail method. " );
       this.LogDebug( "SmtpServer: " + this._SmtpServer );
       this.LogDebug( "SmtpServerPort : " + this._SmtpServerPort );
       this.LogDebug( "SmtpUserId: " + this._SmtpUserId );
       this.LogDebug( "SmtpPassword: " + this._SmtpPassword );
       this.LogDebug( "Subject: " + Subject );
       this.LogDebug( "PlainBody: " + PlainBody );
       this.LogDebug( "SenderEmailAddress: " + SenderEmailAddress );
       this.LogDebug( "EmailAddreses: " + RecipantEmailAddresses );
       this.LogDebug( "AttachmentDirectoryPath: " + AttachmentDirectoryPath );

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
            this.LogDebug(  EvStatics.enumValueToString( EmailStatus.No_Reciever_Addresses) );

          return EmailStatus.No_Reciever_Addresses;
        }

        //
        // If the sender's email address is empty put in a dummy address.
        //
        if ( SenderEmailAddress == String.Empty )
        {
            this.LogDebug(  EvStatics.enumValueToString( EmailStatus.No_Sender_Address ) );

          return EmailStatus.No_Sender_Address;
        }

        //
        // Get the list of email addresses.
        //
        RecipantEmailAddresses = RecipantEmailAddresses.Replace( "(", "<" );
        RecipantEmailAddresses = RecipantEmailAddresses.Replace( ")", ">" );
        String[ ] arrRecipantEmailAddresses = RecipantEmailAddresses.Split( ';' );
        MailAddress fromAddress = new MailAddress( this._SmtpUserId );
        MailAddressCollection toAddresses = new MailAddressCollection( );
        MailAddress replyToAddress = new MailAddress( SenderEmailAddress );

         this.LogDebug(  "arrRecipantEmailAddresses length: " + arrRecipantEmailAddresses.Length );
        //
        // Create the to address list.
        //
        foreach ( String recipantEmailAddress in arrRecipantEmailAddresses )
        {
          if ( recipantEmailAddress == String.Empty )
          {
             this.LogDebug(  "Empty recipant" );
            continue;
          }

           this.LogDebug(  "recipantEmailAddress: " + recipantEmailAddress );
          if ( recipantEmailAddress.Contains( ">" ) == true )
          {
            string stRecipantEmailAddress = recipantEmailAddress.Replace ( ")", String.Empty );
            stRecipantEmailAddress = stRecipantEmailAddress.Replace ( "(", "<" );
            stRecipantEmailAddress = stRecipantEmailAddress.Replace ( ">", String.Empty );
            int index = stRecipantEmailAddress.IndexOf( '<' );
            string recipantntName = stRecipantEmailAddress.Substring( 0, index ).Trim( );
            string recipantntAddress = stRecipantEmailAddress.Substring( index + 1 ).Trim( );

              this.LogDebug(  "name: " + recipantntName + ", address: " + recipantntAddress );

             MailAddress toAddress = new MailAddress ( recipantntAddress.Trim ( ), recipantntName.Trim ( ) );
            toAddresses.Add( toAddress );
          }
          else
          {
            MailAddress toAddress = new MailAddress ( recipantEmailAddress.Trim ( ), recipantEmailAddress.Trim ( ) );
            toAddresses.Add( toAddress );

          }
        }//END interation loop

          this.LogDebug(  "toAddresses: " + toAddresses.Count );

        if ( toAddresses.Count == 0 )
        {
          return EmailStatus.No_Reciever_Addresses;
        }

          this.LogDebug(  "Creating Email message." );

        //
        // Initialise the email message content
        //
        MailMessage message = new MailMessage( fromAddress, toAddresses[ 0 ] );
        message.ReplyToList.Add( replyToAddress );
        message.Subject = Subject;
        message.IsBodyHtml = false;
        message.Body = PlainBody;

        //
        // Add the to recipant email address list.
        //
        for ( int i = 1; i < toAddresses.Count; i++ )
        {
          message.To.Add( toAddresses[ i ] );

        }//END interation loop
          this.LogDebug(  "message.To: " + message.To.Count );

        //
        // Get the report and save it in a temporary file.
        //
        if ( AttachmentDirectoryPath != String.Empty )
        {
          //
          // Create  the file attachment for this e-mail message.
          //
          Attachment attachment = new Attachment( AttachmentDirectoryPath );

          // Add the file attachment to this e-mail message.
          message.Attachments.Add( attachment );
            this.LogDebug(  "message.Attachments: " + message.Attachments.Count );
        }

        //
        // Initialise the Smtp Email client
        //
        SmtpClient smtpClient = new SmtpClient ( this._SmtpServer, this._SmtpServerPort );
        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network; 
        smtpClient.EnableSsl = this._EnableSsl;
         this.LogDebug(  "client.Host: " + smtpClient.Host );
         this.LogDebug(  "client.Port: " + smtpClient.Port );
         this.LogDebug(  "client.DeliveryMethod: " + smtpClient.DeliveryMethod );
         this.LogDebug(  "client.EnableSsl: " + smtpClient.EnableSsl );

        //
        // Initialise the email server credentials if it is needed.
        //
        if ( this._SmtpUserId != String.Empty )
        {
           this.LogDebug(  "SmtpUserId: " + this._SmtpUserId );
           this.LogDebug(  "SmtpPassword: " + this._SmtpPassword );

          smtpClient.Credentials = new System.Net.NetworkCredential( this._SmtpUserId, this._SmtpPassword );
        }

        //
        // Send the email message.
        //
        smtpClient.Send( message );

          this.LogDebug(  "Email Sent." );

        return EmailStatus.Email_Sent;
      }
      catch ( Exception Ex )
      {
         this.LogDebug(  "Event content:"  );
       this.LogDebug(  EvStatics.getException ( Ex ) );

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
    public EmailStatus sendEmail(
      String Subject,
      String HtmlBody,
      String SenderEmailAddress,
      String RecipantEmailAddresses,
      String AttachmentDirectoryPath )
    {
      this.LogMethod("sendEmail method. " );
       this.LogDebug(  "Subject: " + Subject );
      // this.LogDebugValue(  "HtmlBody: " + HtmlBody );
       this.LogDebug(  "SenderEmailAddress: " + SenderEmailAddress );
       this.LogDebug(  "RecipantEmailAddresses: " + RecipantEmailAddresses );
       this.LogDebug(  "AttachmentDirectoryPath: " + AttachmentDirectoryPath );
      try
      {
        //
        // Validate that the are the settings for the SMTP server.
        //
        if ( this._SmtpServer == String.Empty )
        {
            this.LogDebug(  EvStatics.enumValueToString( EmailStatus.No_SMTP_Url ) );
          return EmailStatus.No_SMTP_Url;
        }

        //
        // Validate that the are the settings for the SMTP server.
        //
        if ( RecipantEmailAddresses == String.Empty )
        {
            this.LogDebug(  EvStatics.enumValueToString( EmailStatus.No_Reciever_Addresses ) );
          return EmailStatus.No_Reciever_Addresses;
        }

        //
        // If the sender's email address is empty put in a dummy address.
        //
        if ( SenderEmailAddress == String.Empty )
        {
          SenderEmailAddress = "noreply@domain.com";
        }

        //
        // Get the list of email addresses.
        //
        MailAddress fromAddress = new MailAddress( this._SmtpUserId );
        MailAddressCollection recipantAddressList = new MailAddressCollection( );
        MailAddress replyToAddress = new MailAddress( SenderEmailAddress );

        //
        // Generate an array of email addresses.
        //
        String[ ] arrRecipantEmailAddresses = RecipantEmailAddresses.Split( ';' );

        //
        // Create the to address list.
        //
        foreach ( String recipantEmailAddress in arrRecipantEmailAddresses )
        {
           String stRecipantEmailAddress = recipantEmailAddress.Replace( "(", "<" );
           stRecipantEmailAddress = stRecipantEmailAddress.Replace( ")", ">" );

           if ( stRecipantEmailAddress.Contains( ">" ) == true )
          {
            stRecipantEmailAddress = stRecipantEmailAddress.Replace( ">", String.Empty );
            int index = stRecipantEmailAddress.IndexOf( '<' );
            string recipantntName = stRecipantEmailAddress.Substring( 0, index ).Trim( );
            string recipantntAddress = stRecipantEmailAddress.Substring( index + 1 ).Trim( );

             // this.LogDebugValue(  "name: " + recipantntName + ", address: " + recipantntAddress );

            MailAddress toAddress = new MailAddress( recipantntAddress, recipantntName );
            recipantAddressList.Add( toAddress );
          }
          else
          {
            if ( recipantEmailAddress.Length > 0 )
            {
              // this.LogDebugValue(  "name: " + recipantEmailAddress + ", address: " + recipantEmailAddress );

              MailAddress toAddress = new MailAddress( recipantEmailAddress, recipantEmailAddress );
              recipantAddressList.Add( toAddress );
            }
          }

        }//END interation loop

          this.LogDebug(  "toAddresses: " + recipantAddressList.Count );

        if ( recipantAddressList.Count == 0 )
        {
           this.LogDebug(  "No Addressee list." );
          return EmailStatus.No_Reciever_Addresses;
        }

          this.LogDebug(  "Creating Email message." );

        //
        // Initialise the email message content
        //
        MailMessage message = new MailMessage( fromAddress, recipantAddressList[ 0 ] );
        message.ReplyToList.Add( replyToAddress );
        message.Subject = Subject;

        message.IsBodyHtml = true;
        message.Body = HtmlBody;

        //
        // Add the to recipant email address list.
        //
        for ( int i = 1; i < recipantAddressList.Count; i++ )
        {
          message.To.Add( recipantAddressList[ i ] );

        }//END interation loop
          this.LogDebug(  "message.To: " + message.To.Count );

        //
        // Get the report and save it in a temporary file.
        //
        if ( AttachmentDirectoryPath != String.Empty )
        {
          //
          // Create  the file attachment for this e-mail message.
          //
          Attachment attachment = new Attachment( AttachmentDirectoryPath );

          // Add the file attachment to this e-mail message.
          message.Attachments.Add( attachment );
            this.LogDebug(  "message.Attachments: " + message.Attachments.Count );
        }

        //
        // Initialise the Smtp Email client
        //
        SmtpClient smtpClient = new SmtpClient ( this._SmtpServer, this._SmtpServerPort );
        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtpClient.EnableSsl = _EnableSsl;
         this.LogDebug(  "client.Host: " + smtpClient.Host );
         this.LogDebug(  "client.Port: " + smtpClient.Port );
         this.LogDebug(  "client.DeliveryMethod: " + smtpClient.DeliveryMethod );
         this.LogDebug(  "client.EnableSsl: " + smtpClient.EnableSsl );

        //
        // Initialise the email server credentials if it is needed.
        //
        if ( this._SmtpUserId != String.Empty )
        {
           this.LogDebug(  "SmtpUserId: " + this._SmtpUserId );
           this.LogDebug(  "SmtpPassword: " + this._SmtpPassword );

          smtpClient.Credentials = new System.Net.NetworkCredential( this._SmtpUserId, this._SmtpPassword );
        }

        //
        // Send the email message.
        //
        smtpClient.Send( message );

         this.LogDebug(  "Email Sent." );

        return EmailStatus.Email_Sent;
      }
      catch ( Exception Ex )
      {
          this.LogDebug(  "Event content: "  );
       this.LogDebug(  EvStatics.getException( Ex ) );

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
