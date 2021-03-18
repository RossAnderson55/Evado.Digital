/***************************************************************************************
 * <copyright file="Evado.Model.UniForm\statics.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2013 - 2021 EVADO HOLDING PTY. LTD.  All rights reserved.
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
 *  This class contains the static data objects.
 *
 ****************************************************************************************/

using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Collections.Specialized;
using System.Xml.Serialization;

using Evado.Model;

namespace Evado.Model.UniForm
{
  /// 
  /// Business entity used to model EvFormField
  /// 
  public class EuStatics : Evado.Model.EvStatics
  {
    //===================================================================================

    #region static enumerations
    /// <summary>
    /// This 
    /// </summary>
    public enum UpdateResultCodes
    {
      /// <summary>
      /// Value not test
      /// </summary>
      Null,

      /// <summary>
      /// Update object value validation failed.
      /// </summary>
      Validation_Failed,

      /// <summary>
      /// Update object value duplicate identifier
      /// </summary>
      Duplicate_ID_Error,

      /// <summary>
      /// Database update action failed.
      /// </summary>
      Save_Action_Failed,

      /// <summary>
      /// Database update action completed.
      /// </summary>
      Save_Completed,
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Static Constants

    /// <summary>
    /// This constant defines the device validated session key value.
    /// </summary>
    public const string SESSION_DEVICE_VALIDATED = "WCF_DEVICE_VALIDATED";

    /// <summary>
    /// This constant defines the user validated session key value.
    /// </summary>
    public const string SESSION_USER_VALIDATED = "WCF_USER_VALIDATED";

    /// <summary>
    /// This constant defines the user session identifier, session key value.
    /// </summary>
    public const string SESSION_SESSION_ID = "WCF_SESSION_ID";

    /// <summary>
    /// This constant defines the client version session key value.
    /// </summary>
    public const string SESSION_CLIENT_VERSION = "WCF_CLIENT_VERSION";

    /// <summary>
    /// This constant defines the user login count session key value.
    /// </summary>
    public const string SESSION_AD_USER_GROUPS = "USER_GROUPS";

    /// <summary>
    /// This constant defines the user login count session key value.
    /// </summary>
    public const string SESSION_LOGIN_COUNT = "LOGIN_COUNT";

    /// <summary>
    /// This constant defines the service user command history session key value.
    /// </summary>
    public const string SESSION_SERVICE_COMMAND_HISTORY_LIST = "SERVICE_COMMAND_HISTORY_LIST";

    /// <summary>
    /// This constant defines the UniFORM user profile session key value.
    /// </summary>
    public const string SESSION_UNIFORM_USER_PROFILE = "UNIFORM_USER_PROFILE";

    /// <summary>
    /// This constant defines the user identifier parameter key value.
    /// </summary>
    public const string PARAMETER_LOGIN_USER_TOKEN = "USER_TOKEN";

    /// <summary>
    /// This constant defines the user identifier parameter key value.
    /// </summary>
    public const string PARAMETER_LOGIN_USER_ID = "USER_ID";

    /// <summary>
    /// This constant defines the user password parameter key value.
    /// </summary>
    public const string PARAMETER_LOGIN_PASSWORD = "PASSWORD";

    /// <summary>
    /// This constant defines the user password parameter key value.
    /// </summary>
    public const string PARAMETER_NETWORK_ROLES = "ROLES";

    /// <summary>
    /// This constant defines the default text value.
    /// </summary>
    public const string CONST_WEB_CLIENT = "Web Client";

    /// <summary>
    /// This constant defines the default text value.
    /// </summary>
    public const string CONST_WEB_NET_VERSION = ".NET 4.0";

    /// <summary>
    /// This constant defines the field query suffx value.
    /// </summary>
    public const string CONST_FIELD_FIELD_QUERY_SUFFIX = "_Query";

    /// <summary>
    /// This constant defines the field annotation suffix value.
    /// </summary>
    public const string CONST_FIELD_ANNOTATION_SUFFIX = "_FAnnotation";

    /// <summary>
    /// This constant defines the field version 1.1 annocation suffix value.
    /// </summary>
    public const string CONST_FIELD_V11_ANNOTATION_SUFFIX = "_V11Annotation";

    /// <summary>
    /// This constant defines the field version 1.1 to 1.1 annocation suffix value.
    /// </summary>
    public const string CONST_FIELD_V11_ANNOTATION_SUFFIX_11 = "_V11Annotation_1_1";

    /// <summary>
    /// This constant defines the field version 1.1 to 1.2 annocation suffix value.
    /// </summary>
    public const string CONST_FIELD_V11_ANNOTATION_SUFFIX_12 = "_V11Annotation_1_2";


    /// <summary>
    /// This constant defines the application global object hash table key value.
    /// </summary>
    public const string GLOBAL_ECLINICAL_OBJECT = "ECLINICAL_GLOBAL_OBJECT";

    /// <summary>
    /// This constant defines the application file repository path key value.
    /// </summary>
    public const string GLOBAL_FILE_REPOSITORY_PATH = "FILE_REPOSITORY_PATH";

    /// <summary>
    /// This constant defines the user session clinical object key value.
    /// </summary>
    public const string GLOBAL_DATE_STAMP = "_DATE_STAMP";

    /// <summary>
    /// This constant defines the web config site ID key value
    /// </summary>
    public const string CONFIG_UNIFORM_SERVICE_ID_KEY = "ServiceId";

    /// <summary>
    /// This constant defines the web config debug key value
    /// </summary>
    public const string CONFIG_PAGE_HEADER_KEY = "HomePageHeader";

    /// <summary>
    /// This constant defines the web config UniFORM binary URL key value
    /// </summary>
    public const string CONFIG_TRUSTED_CLIENTS_KEY = "TRUSTED_CLIENTS";

    /// <summary>
    /// This constant defines the web config UniFORM binary URL key value
    /// </summary>
    public const string CONFIG_UNIFORM_BINARY_URL_KEY = "BinaryFileUrl";

    /// <summary>
    /// This constant defines the web config UniFORM binary URL key value
    /// </summary>
    public const string CONFIG_UNIFORM_BINARY_FILE_KEY = "BinaryFilePath";

    /// <summary>
    /// This constant defines the web config Db connection string setting key value
    /// </summary>
    public const string CONFIG_DELETE_HR_PERIOD_KEY = "DELETE_PERIOD_HRS";

    /// <summary>
    /// This constant defines the user session clinical object key value.
    /// </summary>
    public const string CONFIG_DELETE_USER_OBJECT_KEY = "DELETE_USER_GLOBAL_OBJECT";

    /// <summary>
    /// This constant defines the user session clinical object key value.
    /// </summary>
    public const string GLOBAL_COMMAND_HISTORY = "_HISTORY_OBJECT";

    /// <summary>
    /// This constant defines the user session user id key value.
    /// </summary>
    public const string SESSION_USER_ID = "USER_ID";

    /// <summary>
    /// This constant defines the web config debug validation on key value
    /// </summary>
    public const string CONFIG_DELETE_SESSION_ON_EXIT_KEY = "DELETE_USER_GLOBAL_OBJECT";

    /// <summary>
    /// This constant defines the  application object null value.
    /// </summary>
    public const string APPLICATION_OBJECT_NULL = "Null";

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Deserialization method
    // =====================================================================================
    /// <summary>
    /// This method deserialises an Xml object into a generic type.
    /// </summary>
    /// <param name="FileDirectoryPath">String: File directory path</param>
    /// <param name="FileName">String: File name</param>
    /// <returns>AppData: Object AppData </returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Initialise the methods variables and objects.
    /// 
    /// 2. If a empty field then return an empty object.
    /// 
    /// 3. Deserialise the xml data into a local data object.
    /// 
    /// 4. Close the organisation stream.
    /// 
    /// 5. Return the validation object.
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public static AppData DeserialiseApplicationData( String FileDirectoryPath, String FileName )
    {
      // 
      // Initialise the methods variables and objects.
      // 
      AppData dataObject = new AppData( );
      string stApplicationDataObjectPath = FileDirectoryPath + FileName + ".UniForm.xml";

      // 
      // If a empty field then return an empty object.
      // 
      if ( stApplicationDataObjectPath == String.Empty )
      {
        return dataObject;
      }

      // 
      // Deserialise the xml data into a local data object
      // 
      XmlSerializer serializer = new XmlSerializer( typeof( AppData ) );
      TextReader textReader = new StringReader( stApplicationDataObjectPath );
      try
      {
        // 
        // Deserialise the xml object into the type object.
        // 
        dataObject = (AppData) serializer.Deserialize( textReader );

        // 
        // Close the organisation stream.
        // 
        textReader.Close( );
      }
      catch
      {
        textReader.Close( );
        throw;
      }

      // 
      // Return the validation object.
      // 
      return dataObject;

    }//END DeserialiseApplicationData method.

    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Static Minor Version encoding methods.

    // =====================================================================================
    /// <summary>
    /// This method deserialises an Xml object into a generic type.
    /// </summary>
    /// <param name="MinorVersion">Float: minor version</param>
    /// <returns>String: Minor version encoded as a string.</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Initialise the methods variables and objects.
    /// 
    /// 2. convert value
    /// 
    /// 3. Return the minor version as a string.
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public static String decodeMinorVersion ( float MinorVersion )
    {
      // 
      // Initialise the methods variables and objects.
      // 
      String stMinorVersion = MinorVersion.ToString ( "00.00" );
      stMinorVersion = stMinorVersion.Replace ( ".", "_" );

      // 
      // Return the minor version at a floating number.
      // 
      return stMinorVersion;

    }//END decodeMinorVersion method.

    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Static File Management methods.

    // =====================================================================================
    /// <summary>
    ///  This method reads in a application data file.
    /// </summary>
    /// <param name="ApplicationPath">String: The application path name.</param>
    /// <param name="ApplicationName">String: The filename.</param>
    /// <param name="ApplicationData">AppData: The data filed.</param>
    /// <returns>String with bit value.</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Initialise the methods variables and objects.
    /// 
    /// 2. If ApplicationPath is empty, add string stStatus with string 'ApplicationPath.Length is zero'.
    /// 
    /// 3. If ApplicationName is empty, add string stStatus with string 'ApplicationName.Length is zero'.
    /// 
    /// 4. Open the text reader with supplied file.
    /// 
    /// 5. Encode html markup.
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public static string readApplicationDataPage( String ApplicationPath, String ApplicationName, out AppData ApplicationData )
    {
      //
      // Initialise the methods variables and objects.
      //
      ApplicationData = new AppData( );
      String stFileData = String.Empty;
      String stHomePagePathname = ApplicationPath + ApplicationName;
      TextReader reader;

      string stStatus = Evado.Model.EvStatics.CONST_METHOD_START + "Evado.Model.UniForm.Statics.readApplicationHomePage static method. "
        + "\r\n stHomePagePathname: " + stHomePagePathname;

      //
      // If ApplicationPath is empty, add string stStatus with string 'ApplicationPath.Length is zero'.
      //
      if ( ApplicationPath == String.Empty )
      {
        stStatus += "\r\n- ApplicationPath.Length is zero";

        return stStatus;
      }

      //
      // If ApplicationName is empty, add string stStatus with string 'ApplicationName.Length is zero'.
      //
      if ( ApplicationName == String.Empty )
      {
        stStatus += "\r\n- ApplicationName.Length is zero";

        return stStatus;
      }

      try
      {

        // 
        // Open the text reader with supplied file
        // 
        using ( reader = File.OpenText( stHomePagePathname ) )
        {
          stFileData = reader.ReadToEnd( );
        }

        if ( stFileData.Length == 0 )
        {
          stStatus += "\r\n- FileStream.Length is zero";
        }


        stFileData = stFileData.Replace ( "<Status>", "<EditAccess>" );
        stFileData = stFileData.Replace ( "</Status>", "</EditAccess>" );
        stFileData = stFileData.Replace ( "<p>", "[[p]]" );
        stFileData = stFileData.Replace ( "</p>", "[[/p]]" );
        stFileData = stFileData.Replace ( "<br/>", "[[br/]]" );
        stFileData = stFileData.Replace ( "<strong>", "[[strong]]" );
        stFileData = stFileData.Replace ( "</strong>", "[[/strong]]" );

        ApplicationData = Evado.Model.EvStatics.DeserialiseObject<AppData>( stFileData );

        // 
        // Encode html markup.
        //
        ApplicationData = EuStatics.EncodeHtml( ApplicationData );

      }
      catch ( Exception Ex )
      {
        stStatus += "\r\n Evado.Model.UniForm.Statics.readApplicationHomePage static method."
          + "\r\n exception." + EvStatics.getException( Ex );
      }

      return stStatus;

    }//END readApplicationDataPage method

    // =====================================================================================
    /// <summary>
    ///  This method encodes the html.
    /// </summary>
    /// <param name="ApplicationData">AppData: The data filed.</param>
    /// <returns>String with bit value.</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Iterate through the page groups to encode the HTML
    /// 
    /// 2. Iterate through the group fields to encode the HTML in readonly fields.
    /// 
    /// 3. Return AppData object.
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private static AppData EncodeHtml( AppData ApplicationData )
    {
      //
      // Iterate through the page groups to encode the HTML
      //
      foreach ( Evado.Model.UniForm.Group group in ApplicationData.Page.GroupList )
      {
        String description = group.Description;
        description = description.Replace ( "[[", "<" );
        description = description.Replace ( "]]", ">" );
        group.Description = description;

        //
        // Iterate through the group fields to encode the HTML in readonly fields.
        //
        foreach ( Evado.Model.UniForm.Field field in group.FieldList )
        {
          if ( field.Type == EvDataTypes.Read_Only_Text )
          {

            field.Value = field.Value.Replace( "[[", "<" );
            field.Value = field.Value.Replace( "]]", ">" );
          }
        }//END field iteration
      }//END group iteration


      return ApplicationData;
    }//END AppData method


    // =====================================================================================
    /// <summary>
    ///  This method is writing a  json object for a disk file.
    /// </summary>
    /// <param name="ApplicationPath">String: The application path name.</param>
    /// <param name="FileName">String: The filename</param>
    /// <param name="StringText">String: string text </param>
    /// <returns>String with bit value.</returns>
    /// <remarks>
    /// This method consists of follwoing steps. 
    /// 
    /// 1. Initialise the method variables and objects.
    /// 
    /// 2. If ApplicationPath is empty then add a formatted string 'ApplicationPath.Length is zero' to stStatus and return stStatus. 
    /// 
    /// 3. If filename is empty then add a formatted string 'ApplicationName.Length is zero' to stStatus and return stStatus.  
    /// 
    /// 4. Open the text reader with supplied file.
    /// 
    /// 5. Return a string 
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public static string writeJsonFile( String ApplicationPath, String FileName, String StringText )
    {
      //
      // Initialise the method variables and objects.
      //
      String stFileData = String.Empty;
      String stHomePagePathname = ApplicationPath + FileName + ".JSON";
      TextWriter textFile;

      string stStatus = Evado.Model.EvStatics.CONST_METHOD_START + "Evado.Model.UniForm.Statics.writeJsonFile static method. "
        + ", stHomePagePathname: " + stHomePagePathname;

      //
      // If ApplicationPath is empty then add a formatted string 'ApplicationPath.Length is zero' to stStatus and return stStatus. 
      //
      try
      {
        if ( ApplicationPath == String.Empty )
        {
          stStatus += "\r\n\r\n ApplicationPath.Length is zero";

          return stStatus;
        }

        //
        // If filename is empty then add a formatted string 'ApplicationName.Length is zero' to stStatus and return stStatus. 
        //
        if ( FileName == String.Empty )
        {
          stStatus += "\r\n\r\n ApplicationName.Length is zero";

          return stStatus;
        }

        // 
        // Open the text reader with supplied file
        // 
        using ( textFile = File.CreateText( stHomePagePathname ) )
        {
          textFile.Write( StringText );
        }


      }
      catch ( Exception Ex )
      {
        stStatus += "\r\n\r\n Evado.Model.UniForm.Statics.writeJsonFile static method Excpetion."
          + EvStatics.getException( Ex );
      }

      //
      // Retrun stStatus
      //
      return stStatus;

    }//END writeJsonFile method

    // =====================================================================================
    /// <summary>
    ///  This methods saves a text file to a directory.
    /// </summary>
    /// <param name="ApplicationPath">String: The application path </param>
    /// <param name="FileName">String: The filename</param>
    /// <param name="StringText">String: stringTest</param>
    /// <returns>String.</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Initialise the methods variables and objects.
    /// 
    /// 2. If ApplicationPath is empty then add a string 'ApplicationPath.Length is zero' to stStatus and return stStatus.
    /// 
    /// 3. If FileName is empty then add a formatted string 'ApplicationName.Length is zero' to stStatus and return stStatus.
    /// 
    /// 4. Open the text reader with supplied file.
    /// 
    /// 5. Return stStatus
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public static string saveTextFile( String ApplicationPath, String FileName, String StringText )
    {
      //
      // Initialise the methods variables and objects.
      //
      String stFileData = String.Empty;
      String stHomePagePathname = ApplicationPath + FileName;
      TextWriter textFile;

      string stStatus = Evado.Model.EvStatics.CONST_METHOD_START + "Evado.Model.UniForm.Statics.TextFileSave static method. "
        + ", stHomePagePathname: " + stHomePagePathname;

      //
      // If ApplicationPath is empty then add a formatted string ApplicationPath.Length is zero to stStatus and return stStatus.
      //
      try
      {
        if ( ApplicationPath == String.Empty )
        {
          stStatus += "\r\n\r\n ApplicationPath.Length is zero";

          return stStatus;
        }

        //
        // If FileName is empty then add a formatted string ApplicationName.Length is zero to stStatus and return stStatus.
        //
        if ( FileName == String.Empty )
        {
          stStatus += "\r\n\r\n ApplicationName.Length is zero";

          return stStatus;
        }

        // 
        // Open the text reader with supplied file
        // 
        using ( textFile = File.CreateText( stHomePagePathname ) )
        {
          textFile.Write( StringText );
        }


      }
      catch ( Exception Ex )
      {
        stStatus += "\r\n\r\n Evado.Model.UniForm.Statics.TextFileSave static method Excpetion." + EvStatics.getException( Ex );
      }

      //
      // Return stStatus. 
      //
      return stStatus;

    }//END saveTextFile method

    // =====================================================================================
    /// <summary>
    ///  This method appends string text to a text file.
    /// </summary>
    /// <param name="ApplicationPath">String: The Application path</param>
    /// <param name="FileName">String: The filename </param>
    /// <param name="StringText">String: StringText</param>
    /// <returns>String</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Initialise the methods variables and objects.
    /// 
    /// 2. If ApplicationPath is empty then add a formatted string 'ApplicationPath.Length is zero' to stStatus and return stStatus. 
    /// 
    /// 3. If FileName is empty then add a formatted string 'ApplicationName.Length is zero' to stStatus and return stStatus.  
    /// 
    /// 4. Open the text reader with supplied file.
    /// 
    /// 5. return stStatus.
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public static string appendTextFile( String ApplicationPath, String FileName, String StringText )
    {
      //
      // Initialise the methods variables and objects.
      //
      String stFileData = String.Empty;
      String stHomePagePathname = ApplicationPath + FileName;
      TextWriter textFile;

      string stStatus = Evado.Model.EvStatics.CONST_METHOD_START + "Evado.Model.UniForm.Statics.TextFileAppend static method. "
        + ", stHomePagePathname: " + stHomePagePathname;


      try
      {

        //
        // If ApplicationPath is empty then add a formatted string 'ApplicationPath.Length is zero' to stStatus and return stStatus.  
        //
        if ( ApplicationPath == String.Empty )
        {
          stStatus += "\r\n\r\n ApplicationPath.Length is zero";

          return stStatus;
        }

        //
        // If FileName is empty then add a formatted string 'ApplicationName.Length is zero' to stStatus and return stStatus.  
        //
        if ( FileName == String.Empty )
        {
          stStatus += "\r\n\r\n ApplicationName.Length is zero";

          return stStatus;
        }

        // 
        // Open the text reader with supplied file
        // 
        using ( textFile = File.AppendText( stHomePagePathname ) )
        {
          textFile.Write( StringText );
        }


      }
      catch ( Exception Ex )
      {
        stStatus += "\r\n\r\n Evado.Model.UniForm.Statics.TextFileAppend static method Excpetion." + EvStatics.getException( Ex );
      }

      //
      // Return stStatus 
      //
      return stStatus;

    }//END appendTestFile method

    // =====================================================================================
    /// <summary>
    ///  This method reads a text file
    /// </summary>
    /// <param name="ApplicationPath">String: The application path name.</param>
    /// <param name="ApplicationName">String: The filename.</param>
    /// <param name="StringText">String: The String</param>
    /// <returns>String.</returns>
    /// <remarks> 
    /// This method consists of following steps. 
    /// 
    /// 1. Initialise the methods variables and objects
    /// 
    /// 2. If ApplicationPath is empty then add a formatted string 'ApplicationPath.Length is zero' to stStatus and return stStatus.
    /// 
    /// 3. If ApplicationName is empty then add a formatted string 'ApplicationName.Length is zero' to stStatus and return stStatus.
    /// 
    /// 4. Open the text reader with supplied file.
    /// 
    /// 5. Return stStatus
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public static string readTextFile( String ApplicationPath, String ApplicationName, out String StringText )
    {
      //
      // Initialise the methods variables and objects.
      //
      String stFileData = String.Empty;
      String stHomePagePathname = ApplicationPath + ApplicationName;
      TextReader reader;
      StringText = String.Empty;

      string stStatus = Evado.Model.EvStatics.CONST_METHOD_START + "Evado.Model.UniForm.Statics.readJsLibrary static method. "
        + ", stHomePagePathname: " + stHomePagePathname;

      //
      // If ApplicationPath is empty then add a formatted string 'ApplicationPath.Length is zero' to stStatus and return stStatus. 
      //
      if ( ApplicationPath == String.Empty )
      {
        stStatus += "\r\n\r\n ApplicationPath.Length is zero";

        return stStatus;
      }

      //
      // If ApplicationName is empty then add a formatted string 'ApplicationName.Length is zero' to stStatus and return stStatus.
      // 
      if ( ApplicationName == String.Empty )
      {
        stStatus += "\r\n\r\n ApplicationName.Length is zero";

        return stStatus;
      }

      try
      {

        // 
        // Open the text reader with supplied file
        // 
        using ( reader = File.OpenText( stHomePagePathname ) )
        {
          StringText = reader.ReadToEnd( );
        }

        if ( StringText.Length == 0 )
        {
          stStatus += "\r\n\r\n FileStream.Length is zero";
        }

      }
      catch ( Exception Ex )
      {
        stStatus += "\r\n\r\n Evado.Model.UniForm.Statics.readApplicationHomePage static method Excpetion." + EvStatics.getException( Ex );
      }

      //
      // Return stStatus
      //
      return stStatus;

    }//END readTextFile method


    // =====================================================================================
    /// <summary>
    /// This method reads an image.
    /// </summary>
    /// <param name="ImageFilePath">String: The image path name.</param>
    /// <param name="ImageFileName">String: The image name.</param>
    /// <param name="BinaryObject">Byte[]: The binary filed.</param>
    /// <returns>String</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Initialise the methods variables and objects.
    /// 
    /// 2. If ImageFileName is empty then add a formatted string 'ImageFileName.Length is zero' to stStatus and return stStatus. 
    /// 
    /// 3. If ImageFilePath is empty then add a formatted string ' FileStream.Length is zero' to stStatus and return stStatus. 
    /// 
    /// 4. Open and read the file.
    /// 
    /// 5. Return stStatus
    /// 
    /// </remarks>
    /// 
    // -------------------------------------------------------------------------------------
    public static string readInImage( String ImageFilePath, String ImageFileName, out Byte [ ] BinaryObject )
    {

      //
      // Initialise the methods variables and objects.
      //
      BinaryObject = new Byte [ 0 ];
      String stImageFilePathname = ImageFilePath + ImageFileName;
      string stStatus = Evado.Model.EvStatics.CONST_METHOD_START + "Evado.Model.UniForm.Statics.readInImage static method. "
        + ", stImageFilePathname: " + stImageFilePathname;
      try
      {
        //
        // If ImageFileName is empty then add a formatted string 'ImageFileName.Length is zero' to stStatus and return stStatus. 
        //
        if ( ImageFileName == String.Empty )
        {
          stStatus += "\r\n\r\n ImageFileName.Length is zero";

          return stStatus;
        }

        //
        // If ImageFilePath is empty then add a formatted string ' FileStream.Length is zero' to stStatus and return stStatus. 
        //
        if ( ImageFilePath == String.Empty )
        {
          stStatus += "\r\n\r\n ImageFilePath.Length is zero";

          return stStatus;
        }

        //
        // Open and read the file.
        //
        using ( FileStream fileStream = new FileStream( stImageFilePathname, FileMode.Open, FileAccess.Read ) )
        {
          if ( fileStream.Length == 0 )
          {
            stStatus += "\r\n\r\n FileStream.Length is zero";

            return stStatus;
          }

          BinaryObject = new Byte [ fileStream.Length ];

          fileStream.Read( BinaryObject, 0, BinaryObject.Length );

          fileStream.Close( );
        }
      }
      catch ( Exception Ex )
      {
        stStatus += "\r\n\r\n Evado.Model.UniForm.Statics.writeOutImage static method Excpetion." + EvStatics.getException( Ex );
      }

      //
      // Return stStatus
      //
      return stStatus;

    }//END readInImage method

    // =====================================================================================
    /// <summary>
    ///  This method writes an image out to the image path location.
    /// </summary>
    /// <param name="ImageFilePath">String: The path to the image </param>
    /// <param name="ImageFileName">String: The file name of the image.</param>
    /// <param name="ImageObject">Byte[]: The image as an array of bytes</param>
    /// <returns> String. </returns>
    /// <remarks>
    /// This method consists of following steps.
    /// 
    /// 1. Initialise the methods variables and objects.
    /// 
    /// 2. If length of ImageObject is equal to 0 then assign formatted string 'Object length zero' to stStatus and return stStatus. 
    /// 
    /// 3. Open the stream to the file.
    /// 
    /// 4. Iterate throuch b byte array.
    /// 
    /// 5. Writes a byte to the current position in the file stream.
    /// 
    /// 6.  Close the file.
    /// 
    /// 7. Return stStatus. 
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public static string writeOutImage( String ImageFilePath, String ImageFileName, Byte [ ] ImageObject )
    {
      //
      // Initialise the methods variables and objects.
      //
      String stImageFilePathname = ImageFilePath + ImageFileName;

      string stStatus = Evado.Model.EvStatics.CONST_METHOD_START + "Evado.Model.UniForm.Statics.writeOutImage static method. "
        + ", stImageFilePathname: " + stImageFilePathname
        + ", ImageSize: " + ImageObject.Length;
      try
      {

        //
        // If length of ImageObject is equal to 0 then assign formatted string 'Object length zero' to stStatus and return stStatus.  
        //
        if ( ImageObject.Length == 0 )
        {
          stStatus = " >> Object length zero ";
          return stStatus;
        }

        // 
        // Open the stream to the file.
        // 
        using ( FileStream fs = new FileStream( stImageFilePathname, FileMode.Create ) )
        {
          // 
          // Iterate throuch b byte array.
          // 
          foreach ( Byte b in ImageObject )
          {
            //
            // Writes a byte to the current position in the file stream.
            //
            fs.WriteByte( b );
          }//END b iteration

          // 
          // Close the file.
          // 
          fs.Close( );

        }// End StreamWriter.
      }
      catch ( Exception Ex )
      {
        throw ( Ex );
      }

      //
      // Return stStatus
      //
      return stStatus;

    }//END writeOutImage method

    #region Class Enumeration
    /// <summary>
    /// This enumerated list contains upload file status codes.
    /// </summary>
    public enum HttpUploadFileStatusCodes
    {

      /// <summary>
      /// This enumeration defines that upload file status completed
      /// </summary>
      Completed,

      /// <summary>
      /// This enumeration defines file lenght is zero.
      /// </summary>
      File_Length_Zero,

      /// <summary>
      /// This enumeration defines upload file status transfer is failed 
      /// </summary>
      Transfer_Failed,

    }

    #endregion

    //  ==================================================================================
    /// <summary>
    /// This method sends an http upload file to a called service.
    /// </summary>
    /// <param name="ImageServiceUrl">String: The URL of the service</param>
    /// <param name="ImageFileName">String: The filename</param>
    /// <param name="ParameterName">String: The paremeter name.</param>
    /// <param name="ContentType">String: The content type</param>
    /// <returns>Enum: HttpUploadFileStatusCodes </returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Define the named value collection.
    /// 
    /// 2. Initialise method variables and objects. 
    /// 
    /// 3. Define the web request object
    /// 
    /// 4. Create the stream to pass the object into the request object. 
    /// 
    /// 5. Define the form Name Value Collection keys
    /// 
    /// 6. define the  file name and image content.
    /// 
    /// 7. Return HttpUploadFileStatusCodes.
    /// 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public static HttpUploadFileStatusCodes HttpUploadFile(
      String ImageServiceUrl,
      String ImageFileName,
      String ParameterName,
      String ContentType )
    {
      //
      // Define the named value collection.
      //
      NameValueCollection nvc = new NameValueCollection( );
      nvc.Add( "id", "TTR" );
      nvc.Add( "btn-submit-photo", "Upload" );

      //
      // Initialise method variables and objects.
      //
      string boundary = "---------------------------" + DateTime.Now.Ticks.ToString( "x" );
      byte [ ] boundarybytes = System.Text.Encoding.ASCII.GetBytes( "\r\n--" + boundary + "\r\n" );

      //
      // Define the web request object.
      //
      HttpWebRequest wr = (HttpWebRequest) WebRequest.Create( ImageServiceUrl );
      wr.ContentType = "multipart/form-data; boundary=" + boundary;
      wr.Method = "POST";
      wr.KeepAlive = true;
      wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

      //
      // Create the stream to pass the object into the request object.
      //
      Stream rs = wr.GetRequestStream( );

      //
      // Define the form Name Value Collection keys
      //
      string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
      foreach ( string key in nvc.Keys )
      {
        rs.Write( boundarybytes, 0, boundarybytes.Length );
        string formitem = string.Format( formdataTemplate, key, nvc [ key ] );
        byte [ ] formitembytes = System.Text.Encoding.UTF8.GetBytes( formitem );
        rs.Write( formitembytes, 0, formitembytes.Length );
      }
      rs.Write( boundarybytes, 0, boundarybytes.Length );

      //
      // define the  file name and image content.
      //
      string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
      string header = string.Format( headerTemplate, ParameterName, ImageFileName, ContentType );
      byte [ ] headerbytes = System.Text.Encoding.UTF8.GetBytes( header );
      rs.Write( headerbytes, 0, headerbytes.Length );

      FileStream fileStream = new FileStream( ImageFileName, FileMode.Open, FileAccess.Read );

      if ( fileStream.Length == 0 )
      {
        return HttpUploadFileStatusCodes.File_Length_Zero;
      }

      byte [ ] buffer = new byte [ 4096 ];
      int bytesRead = 0;
      while ( ( bytesRead = fileStream.Read( buffer, 0, buffer.Length ) ) != 0 )
      {
        rs.Write( buffer, 0, bytesRead );
      }
      fileStream.Close( );

      byte [ ] trailer = System.Text.Encoding.ASCII.GetBytes( "\r\n--" + boundary + "--\r\n" );
      rs.Write( trailer, 0, trailer.Length );
      rs.Close( );

      WebResponse wresp = null;
      try
      {
        wresp = wr.GetResponse( );
        Stream stream2 = wresp.GetResponseStream( );
        StreamReader reader2 = new StreamReader( stream2 );
        System.Console.WriteLine( string.Format( "File uploaded, server response is: {0}", reader2.ReadToEnd( ) ) );
      }
      catch
      {
        if ( wresp != null )
        {
          wresp.Close( );
          wresp = null;
        }
        throw;
      }
      finally
      {
        wr = null;
      }

      //
      // Return HttpUploadFileStatusCodes
      // 
      return HttpUploadFileStatusCodes.Completed;
    }



    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion


  } // Close Statics class

} // Close namespace Evado.Model.UniForm
