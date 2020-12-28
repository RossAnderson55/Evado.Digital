/***************************************************************************************
 * <copyright file="DAL\EvadoEncrypt.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the encryption and decryption methods for EVADO HOLDING PTY. LTD. DAL layer.
 *  
 *  The class has the following public properties:
 * 
 *   Status:  (ReadOnly) containing a description of the execution process.
 *
 *   Key:  set and gets the encryption key as a array of bytes.
 *
 *   IV:  set and gets the encryption IV as a array of bytes.
 *
 *  The class has the following public methods:
 *  
 *  EvadoEncrypt: initialises the class.
 *                Generates a random set of Keys and IVs.
 *  
 *  EvadoEncrypt: initialises the class.
 *                byte [] Key: the 24 byte encryption key
 *                byte [] IV:  the 8 byte encryption IV
 *  
 *  EvadoEncrypt: initialises the class.
 *                FormUid Guid_1: first FormUid to seed the encryption key.
 *                FormUid Guid_1: second FormUid to seed the encryption key.
 *
 *  encryptString: encrypts a string.
 * 
 *  decryptString: decrypts a string.
 * 
 *
 ****************************************************************************************/

using System;
using System.Configuration;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Evado.Dal
{
  /// <summary>
  /// This class handles Evado encrypt and decryption functions for the data access layer.
  /// </summary>
  class EvEncrypt : EvDalBase
  {
    #region Initialise class object.

    public EvEncrypt ( )
    {
      Guid Null = Guid.Empty;
      this.encodeKeyFromGuid ( Null, Null );
    }

    public EvEncrypt ( byte [ ] Key, byte [ ] IV )
    {
      this._Key = Key;
      this._IV = IV;
    }

    public EvEncrypt ( Guid Guid_1, Guid Guid_2 )
    {
      this.encodeKeyFromGuid ( Guid_1, Guid_2 );
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Initialise class global variables.

    // 
    // Define the key and IV arrays.
    // 
    private byte [ ] _Key = new byte [ 24 ];
    private byte [ ] _IV = new byte [ 8 ];

    private Evado.Model.EvEventCodes _eventId = Evado.Model.EvEventCodes.Ok;

    public Byte [ ] Key
    {
      get
      {
        return this._Key;
      }
      set
      {
        this._Key = value;
      }
    }

    public Byte [ ] IV
    {
      get
      {
        return this._IV;
      }
      set
      {
        this._IV = value;
      }
    }

    public Evado.Model.EvEventCodes EventId
    {
      get
      {
        return this._eventId;
      }
      set
      {
        this._eventId = value;
      }
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class methods.

    //  =================================================================================
    /// <summary>
    /// getString method
    /// 
    /// Description: 
    ///  This method returns an array of bytes as a string.
    /// 
    /// </summary>
    /// <param name="byteInput">Array of bytes.</param>
    /// <returns>string.</returns>
    //  ---------------------------------------------------------------------------------
    public string getString ( Byte [ ] byteInput )
    {
      return Convert.ToBase64String ( byteInput );
    }

    //  =================================================================================
    /// <summary>
    /// getString method
    /// 
    /// Description: 
    ///  This method returns a string as an array of bytes.
    /// 
    /// </summary>
    /// <param name="stringInput">string.</param>
    /// <returns>array of Bytes.</returns>
    //  ---------------------------------------------------------------------------------
    public Byte [ ] getByte ( string stringInput )
    {
      return Encoding.Unicode.GetBytes ( stringInput );
    }

    //  =================================================================================
    /// <summary>
    /// This method encrypts a string of clear text.
    /// 
    /// </summary>
    /// <param name="ClearString">String: clear text.</param>
    /// <returns>String: Encrypted string.</returns>
    //  ---------------------------------------------------------------------------------
    public string encryptString ( string ClearString )
    {
      this.LogMethod ( "encryptString method. " );
      // 
      // Define the reader and writer.
      // 
      string encryptedData = String.Empty;
      TripleDESCryptoServiceProvider tripleDes = new TripleDESCryptoServiceProvider ( );

      // 
      // If the key is empty return empty string.
      // 
      if ( this._Key [ 0 ] == 0 )
      {
        return String.Empty;
      }

      try
      {
        // 
        // Convert the string to a byte array.
        // 
        byte [ ] buffer = Encoding.Unicode.GetBytes ( ClearString );

        // 
        // Set the encryption keys.
        // 
        tripleDes.Key = this._Key;
        tripleDes.IV = this._IV;

        // 
        // Encrypt the clear data.
        // 
        ICryptoTransform ITransform = tripleDes.CreateEncryptor ( );
        encryptedData = Convert.ToBase64String ( ITransform.TransformFinalBlock ( buffer, 0, buffer.Length ) );

        this.LogMethodEnd ( "encryptString" );
        // 
        // Return the encrypted data.
        // 
        return encryptedData;
      }
      catch ( CryptographicException Ex )
      {
        this.LogException ( Ex );

        this._eventId = Evado.Model.EvEventCodes.Encryption_Encryption_Bad_Data_Error;

        throw;
      }


    }//END encryptString method

    //  =================================================================================
    /// <summary>
    /// This method decrypts a string of encrypted text.
    /// 
    /// </summary>
    /// <param name="EncryptedString"></param>
    /// <returns>Unencrypted string</returns>
    //  ---------------------------------------------------------------------------------
    public string decryptString ( String EncryptedString )
    {
      this.LogMethod ( "decryptString method. " );
      // 
      // Define the reader and writer.
      // 
      string clearText = String.Empty;
      TripleDESCryptoServiceProvider tripleDes = new TripleDESCryptoServiceProvider ( );

      // 
      // If the key is empty return empty string.
      // 
      if ( this._Key [ 0 ] == 0 )
      {
        return String.Empty;
      }

      try
      {
        // 
        // Convert the string to a byte array.
        //
        byte [ ] buffer = Convert.FromBase64String ( EncryptedString );

        // 
        // Set the encryption keys.
        // 
        tripleDes.Key = this._Key;
        tripleDes.IV = this._IV;

        // 
        // Encrypt the clear data.
        // 
        ICryptoTransform ITransform = tripleDes.CreateDecryptor ( );
        clearText = Encoding.Unicode.GetString ( ITransform.TransformFinalBlock ( buffer, 0, buffer.Length ) );

        this.LogMethodEnd ( "decryptString" );

        // 
        // Return the clear data.
        // 
        return clearText;

      }
      catch ( CryptographicException Ex )
      {
        this.LogException( Ex );

        this._eventId = Evado.Model.EvEventCodes.Encryption_Decryption_Bad_Data_Error;

        throw;
      }

    }//END decryptString method

    //  =================================================================================
    /// <summary>
    /// This method decrypts a string of encrypted text.
    /// 
    /// </summary>
    /// <param name="Guid_1">a Guid identifier used as part of the encryption key </param>
    /// <param name="Guid_2">a Guid identifier used as part of the encryption key </param>
    //  ---------------------------------------------------------------------------------
    private void encodeKeyFromGuid ( Guid Guid_1, Guid Guid_2 )
    {
      this.LogMethod ( "encodeKeyFromGuid method. " );
      this.LogDebug ( "Guid_1: " + Guid_1 + ", Guid_2: " + Guid_2 );

      // 
      // Initialise the method variables and objects.
      // 
      this._eventId = Evado.Model.EvEventCodes.Ok;

      byte [ ] byteGuid_1 = Guid_1.ToByteArray ( );
      byte [ ] byteGuid_2 = Guid_2.ToByteArray ( );
      byte [ ] byteGuids = new byte [ 32 ];

      for ( int index = 0; index < 16; index++ )
      {
        byteGuids [ index ] = byteGuid_1 [ index ];
        byteGuids [ ( index + 16 ) ] = byteGuid_2 [ index ];
      }
      this.LogDebug ( "byteGuids length: " + byteGuids.Length + " value: " + Convert.ToBase64String ( byteGuids ) );

      for ( int index = 0; index < 24; index++ )
      {
        this._Key [ index ] = byteGuids [ index ];
      }

      for ( int index = 24; index < 32; index++ )
      {
        this._IV [ ( index - 24 ) ] = byteGuids [ index ];
      }
      this.LogDebug ( "Key length: " + this._Key.Length );
      this.LogDebug ( "Key value: " + Convert.ToBase64String ( this._Key ) );
      this.LogDebug ( "IV length: " + this._IV.Length );
      this.LogDebug ( "IV value: " + Convert.ToBase64String ( this._IV ) );

      this.LogMethodEnd ( "encodeKeyFromGuid" );
    }//END encodeKeyFromGuid method

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion


  }//END EvadoEncrypt class

}//END DAL namespase
