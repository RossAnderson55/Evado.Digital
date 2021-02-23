/***************************************************************************************
 * <copyright file="dal\DataChanges.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class handles the database query interface for the trial Visit object.
 * 
 *  This class contains the following public properties:
 *   DebugLog:       Containing the exeuction status of this class, used for debugging the 
 *                 class from BLL or UI layers.
 * 
 *  This class contains the following public methods:
 *   getPreparationView:      Executes a selectionList quey returning an ArrayList of visit objects.
 * 
 *   getPreparationList:      Executes a query to generate an ArrayList of Selection OptionsOrUnit objects.
 * 
 *   getTrial:  Executes a query to return a TrialVisit object by the trial and visit id. 
 * 
 *   addTestReport:      Executee an query to add a new TrialVisit object to the database.
 * 
 ****************************************************************************************/

using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;

//Application specific class references.
using Evado.Model;
using Evado.Model.Digital;


namespace Evado.Dal.Digital
{
  /// <summary>
  /// This class is handles the data access layer for the data change data object.
  /// </summary>
  public class EvDataChanges: EvDalBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EvDataChanges ( )
    {
      this.ClassNameSpace = "Evado.Dal.Digital.EvDataChanges.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EvDataChanges ( EvClassParameters Settings )
    {
      this.ClassParameters = Settings;
      this.ClassNameSpace = "Evado.Dal.Digital.EvDataChanges.";

    }

    #endregion

    #region Clase Global  Initialisation
    /* *********************************************************************************
     * 
     * Defines the classes constansts and global variables
     * 
     * *********************************************************************************/
    // 
    // Define the log source file
    // 
    private static string _eventLogSource = ConfigurationManager.AppSettings [ "EventLogSource" ];
    private static string _stSiteGuid = ConfigurationManager.AppSettings [ "SiteGuid" ];

    /// <summary>
    /// This constant defines the sql query string for viewing datachanges. 
    /// </summary>
    private const string _sqlQueryView = "Select DC_Guid, DC_RecordGuid, DC_Uid, DC_RecordUid, "
      + "DC_TableName, DC_DataChange, DC_UserId, DC_DateStamp "
      + "FROM EvDataChanges ";

    /// <summary>
    /// This constrant defines the storeprocedure for adding items.
    /// </summary>
    private const string _STORED_PROCEDURE_AddItem = "usr_DataChanges_add";

    #region Define the query parameter constants.
    /// <summary>
    /// This constant defines a parameter for global unique identifier
    /// </summary>
    private const string _parmGuid = "@Guid";

    /// <summary>
    /// This constant defines a parameter for record global unique identifier
    /// </summary>
    private const string _parmRecordGuid = "@RecordGuid";

    /// <summary>
    /// This constant defines a parameter for unique identifier
    /// </summary>
    private const string _parmUid = "@Uid";

    /// <summary>
    /// This constant defines a parameter for record unique identifier
    /// </summary>
    private const string _parmRecordUid = "@RecordUid";

    /// <summary>
    /// This constant defines a parameter for table name
    /// </summary>
    private const string _parmTableName = "@TableName";

    /// <summary>
    /// This constant defines a parameter for data change
    /// </summary>
    private const string _parmDataChange = "@DataChange";

    /// <summary>
    /// This constant defines a parameter for user identifier
    /// </summary>
    private const string _parmUserId = "@UserId";

    /// <summary>
    /// This constant defines a parameter for date stamp
    /// </summary>
    private const string _parmDateStamp = "@DateStamp";
    #endregion

    //
    //  Define the SQL query string variable.
    //  
    private string _sqlQueryString = String.Empty;
    private static Guid _SiteGuid = Guid.Empty;
    /// <summary>
    /// This property is used to define part of the encryption keys.
    /// </summary>
    public static Guid SiteGuid
    {
      get { return _SiteGuid; }
      set { _SiteGuid = value; }
    }

    //byte [] byteEncryptKey = null;
    //byte [] byteDecryptKey = null;
    //byte [] byteEncryptIV = null;
    //byte [] byteDecryptIV = null;

    #endregion

    #region Set Query Parameters

    // =====================================================================================
    /// <summary>
    /// This class defines the SQL parameter for a query. 
    /// </summary>
    /// <returns>SqlParameter: an array of sql query parameters</returns>
    /// <remarks>
    /// This method consists of the following step:
    /// 
    /// 1. Create an array of sql query parameters. 
    /// 
    /// 2. Return an array list of sql query parameters.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private static SqlParameter [ ] GetParameters ( )
    {
      SqlParameter [ ] parms = new SqlParameter [ ] 
      {
        new SqlParameter( _parmGuid,SqlDbType.UniqueIdentifier),
        new SqlParameter( _parmRecordGuid,SqlDbType.UniqueIdentifier),
        new SqlParameter( _parmRecordUid,SqlDbType.BigInt),
        new SqlParameter( _parmTableName, SqlDbType.NVarChar, 50),
        new SqlParameter( _parmDataChange, SqlDbType.NText),
        new SqlParameter( _parmUserId, SqlDbType.NVarChar, 50),
        new SqlParameter( _parmDateStamp, SqlDbType.DateTime),
      };

      return parms;
    }//END GetParameters class

    // =====================================================================================
    /// <summary>
    /// This class sets the query parameter values. 
    /// </summary>
    /// <param name="parms">SqlParameter: Database parameters</param>
    /// <param name="Item">EvDataChange: Change data object</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Set Guid and Record Guid if they are not set
    /// 
    /// 2. Load the SQL query parameter from the datachange object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private void setParameters ( SqlParameter [ ] parms, EvDataChange Item )
    {
      // 
      // Set the Guid if it is not set.
      // 
      if ( Item.Guid == Guid.Empty )
      {
        Item.Guid = Guid.NewGuid ( );
      }

      // 
      // Set the RecordGuid if it is not set.
      // 
      if ( Item.RecordGuid == Guid.Empty )
      {
        Item.RecordGuid = Guid.NewGuid ( );
      }

      // 
      // Load the Sql Parameters
      // 
      parms [ 0 ].Value = Item.Guid;
      parms [ 1 ].Value = Item.RecordGuid;
      parms [ 2 ].Value = 0;
      parms [ 2 ].Value = Item.RecordUid;
      parms [ 3 ].Value = Item.TableName.ToString ( );
      parms [ 4 ].Value = this.encryptData ( Item );
      parms [ 5 ].Value = Item.UserId;
      parms [ 6 ].Value = Item.DateStamp;

    }//END setUpdateChangeParameters.

    // +++++++++++++++++++++++++ END QUERY PARAMETERS SECTION ++++++++++++++++++++++++++++++
    #endregion

    #region data change reader

    // =====================================================================================
    /// <summary>
    /// This class reads the content of the data reader object into Change business object.
    /// </summary>
    /// <param name="Row">DataRow: a data row object</param>
    /// <returns>EvDataChange: a data change object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Extract the data row values to the data change object. 
    /// 
    /// 2. Encrypt the data change if it is not encrypted. 
    /// 
    /// 3. Return the data change object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private EvDataChange getRowData ( DataRow Row )
    {
      // 
      // Initialise the data change object
      // 
      EvDataChange dataChange = new EvDataChange ( );

      // 
      // Extract the data row values to the data change object. 
      // 
      dataChange.Guid = EvSqlMethods.getGuid ( Row, "DC_Guid" );
      dataChange.RecordGuid = EvSqlMethods.getGuid ( Row, "DC_RecordGuid" );
      dataChange.Uid = EvSqlMethods.getLong ( Row, "DC_Uid" );
      dataChange.RecordUid = EvSqlMethods.getLong ( Row, "DC_RecordUid" );
      dataChange.TableName =
        Evado.Model.EvStatics.parseEnumValue<EvDataChange.DataChangeTableNames> (
        EvSqlMethods.getString ( Row, "DC_TableName" ) );
      string encrypted = EvSqlMethods.getString ( Row, "DC_DataChange" );
      dataChange.UserId = EvSqlMethods.getString ( Row, "DC_UserId" );
      dataChange.DateStamp = EvSqlMethods.getDateTime ( Row, "DC_DateStamp" );

      //
      // Check that the encrypted string exists.
      //
      if ( encrypted != String.Empty )
      {
        dataChange = this.decryptData ( encrypted, dataChange.Guid );

        // 
        // Check that items exist.
        // 
        if ( dataChange != null )
        {
          //
          // If the data change items exist process them.
          // 
          if ( dataChange.Items.Count > 0 )
          {
            // 
            // Iterate through the data change items updating the html coding
            // 
            for ( int count = 0; count < dataChange.Items.Count; count++ )
            {
              dataChange.Items [ count ].InitialValue =
                dataChange.Items [ count ].InitialValue.Replace ( "&amp;gt;", "&amp;gt; " );

              dataChange.Items [ count ].NewValue =
                dataChange.Items [ count ].NewValue.Replace ( "&amp;gt;", "&amp;gt; " );

            }//END iterate through the item values correcting html coding.

          }//END data change items exist.

        }//END items exit

      }//END encrypted date exists

      // 
      // Return the newField .
      // 
      return dataChange;

    }//END getRowData method.

    // +++++++++++++++++++++++++++ END RECORD READER SECTION  ++++++++++++++++++++++++++++++
    #endregion

    #region DataChange Queries

    // =====================================================================================
    /// <summary>
    /// This method queries the data changes for a record.
    /// </summary>
    /// <param name="RecordGuid">Guid: (Mandatory) Record global Unique identifier.</param>
    /// <param name="TableName">EvDataChange.DataChangeTableNames: (Mandatory) table name.</param>
    /// <returns>List of EvDataChange: a list of data change items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Defines the sql query parameters and the sql query string. 
    /// 
    /// 2. Execute the sql query string with parameters and store the results on datatable. 
    /// 
    /// 3. Loop through the table and extract data row to the data change object. 
    /// 
    /// 4. Add the object to the Datachanges list. 
    /// 
    /// 5. Return the Datachanges list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvDataChange> GetView ( 
      Guid RecordGuid, 
      EvDataChange.DataChangeTableNames TableName )
    {
      //
      // Initialize the status and the return data change list
      //
      this.LogMethod( "GetView method. " );
      this.LogDebug( "SiteGuid: " + EvDataChanges._SiteGuid );
      this.LogDebug( "RecordGuid: " + RecordGuid );
      this.LogDebug( "Record Type: " + TableName );
      List<EvDataChange> View = new List<EvDataChange> ( );

      //
      // If the site guid is empty exit.
      //
      if ( EvDataChanges._SiteGuid == Guid.Empty ) 
      {
        return new List<EvDataChange> ( );
      }

      // 
      // Define the SQL query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( _parmRecordGuid, SqlDbType.UniqueIdentifier ),
        new SqlParameter( _parmTableName, SqlDbType.NVarChar, 50) 
      };
      cmdParms [ 0 ].Value = RecordGuid;
      cmdParms [ 1 ].Value = TableName.ToString ( );

      // 
      // Define the query string.
      // 
      _sqlQueryString = _sqlQueryView + " WHERE (DC_RecordGuid = @RecordGuid) "
        + " AND (DC_TableName = @TableName) "
        + " ORDER BY DC_DateStamp;";

      this.LogClass(  _sqlQueryString );

      // 
      // Execute the query against the database.
      // 
      using ( DataTable table = EvSqlMethods.RunQuery ( _sqlQueryString, cmdParms ) )
      {
        // 
        // Iterate through the results extracting the role information.
        // 
        for ( int Count = 0; Count < table.Rows.Count; Count++ )
        {
          // 
          // Extract the table row
          // 
          DataRow row = table.Rows [ Count ];

          // 
          // Read the row data.
          // 
          EvDataChange Item = this.getRowData ( row );

          // 
          // Append the new Change object to the array.
          // 
          View.Add ( Item );
        }//END record iteration loop.

      }//END Using statement
      this.LogDebug( "Record Count: " + View.Count );

      // 
      // Pass back the result arrray.
      // 
      return View;

    } // Close GetView method.

    // ++++++++++++++++++++++++++++++ END VIEW QUERY SECTION +++++++++++++++++++++++++++++++
    #endregion

    #region DataChange Update queries

    // =====================================================================================
    /// <summary>
    /// This class adds a data change object to the database.
    /// </summary>
    /// <param name="DataChange">EvDataChange: a datachange object</param>
    /// <returns>EvEventCodes: an event code for adding items to data change table.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Loop through the datachange object items and validate whether the items are not empty
    /// 
    /// 2. Define the sql query parameters 
    /// 
    /// 3. Execute the storeprocedure for adding new items to datachange table. 
    /// 
    /// 4. Return the event code for adding items. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes AddItem ( EvDataChange DataChange )
    {
      //
      // Initialize the method status and a no-items variable
      //
      this.LogMethod( "addItem method." );
      this.LogDebug( "ProjectId: " + DataChange.TrialId );
      this.LogDebug( "TypeId: " + DataChange.TableName );
      this.LogDebug( "UserId: " + DataChange.UserId );
      this.LogDebug( "DataChange.Items.Count count: " + DataChange.Items.Count );
      bool NoItems = true;

      //
      // Loop through the datachange object and validate whether the items are not empty. 
      //
      foreach ( EvDataChangeItem item in DataChange.Items )
      {
        if ( item.ItemId != String.Empty )
        {
          NoItems = false;
        }
      }//END Foreach interation loop.

      // 
      // IF not newField then exit without saving.
      // 
      if ( NoItems == true )
      {
        this.LogDebug( " No Items found." );

        return EvEventCodes.Ok;
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] _cmdParms = GetParameters ( );
      setParameters ( _cmdParms, DataChange );

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( _STORED_PROCEDURE_AddItem, _cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      return EvEventCodes.Ok;

    }//END AddItem class. 
    #endregion

    #region Data encryption methods

    // =====================================================================================
    /// <summary>
    /// This method serialises and encrypts the data change object.
    /// </summary>
    /// <param name="DataChange">EvDataChange: data change object</param>
    /// <returns>string: an Encrypted string</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Serialize the datachange object to xml datachange string. 
    /// 
    /// 2. Check whether the Guid of datachange object exists. 
    /// 
    /// 3. Initialize the encrypt object and encrypt xml datachange string to the encrypted 
    /// datachange string. 
    /// 
    /// 4. Return the encrypted datachange string. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public string encryptData ( EvDataChange DataChange )
    {
      // 
      // Initialise the method status, an xml data change string and an encrypted data chagne string. 
      // 
      this.LogMethod( "encryptData method. " );
      this.LogDebug( "SiteGuid: " + EvDataChanges._SiteGuid );
      this.LogDebug( "DataChange.Guid: " + DataChange.Guid );
      string xmlDataChange = String.Empty;
      string encryptedDataChange = String.Empty;

      // 
      // serialise the datachange object.
      // 
      xmlDataChange = Evado.Model.Digital.EvcStatics.SerialiseObject<EvDataChange> ( DataChange );

      // 
      // Check whether the Guid exists. 
      // 
      if ( EvDataChanges._SiteGuid == Guid.Empty
        || DataChange.Guid == Guid.Empty )
      {
        return xmlDataChange;
      }

      xmlDataChange = xmlDataChange.Replace ( "\r\n", string.Empty );
      xmlDataChange = xmlDataChange.Replace ( ">   ", ">" );
      xmlDataChange = xmlDataChange.Replace ( ">   ", ">" );
      xmlDataChange = xmlDataChange.Replace ( ">  ", ">" );
      xmlDataChange = xmlDataChange.Replace ( "> ", ">" );

      xmlDataChange = xmlDataChange.Replace ( "   &lt;", "&lt;" );
      xmlDataChange = xmlDataChange.Replace ( "  &lt;", "&lt;" );
      xmlDataChange = xmlDataChange.Replace ( " &lt;", "&lt;" );
      xmlDataChange = xmlDataChange.Replace ( " &lt;", "&lt;" );

      //this.LogDebug( "xmlDataChange: {{" + xmlDataChange + "}}" );

      //return xmlDataChange;
      // 
      // Initialise the encryption object.
      // 
      EvEncrypt encrypt = new EvEncrypt ( EvDataChanges._SiteGuid, DataChange.Guid );

      // 
      // Encrypt the data change xml object.
      // 
      encryptedDataChange = encrypt.encryptString ( xmlDataChange );
      this.LogClass(  encrypt.Log );

      // 
      // Return the encrypted string.
      // 
      return encryptedDataChange;

    }//END encrypt Data method.

    // =====================================================================================
    /// <summary>
    /// This method decrypts the data change object.
    /// </summary>
    /// <param name="encryptedData">string: The encrypted data string</param>
    /// <param name="ItemGuid">Guid: The item Guid</param>
    /// <returns>EvDataChange: a datachange object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Try if encrypted data string exists, deserialize encrypted string to the return datachange object
    /// 
    /// 2. if not, decrypt the encrypted string to xml datachange string. 
    /// 
    /// 3. Deserialize xml datachange string to the datachange object. 
    /// 
    /// 4. Return the datachange object. 
    /// 
    /// 5. catch - write error message to log and return new datachange object. 
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private EvDataChange decryptData ( string encryptedData, Guid ItemGuid )
    {
      // 
      // Initialise the method status, a return datachange object and an xml datachange string. 
      // 
      this.LogMethod( "decryptData method. ");
      this.LogDebug( "encryptedData length: " + encryptedData.Length);
      this.LogDebug( "SiteGuid: " + EvDataChanges._SiteGuid);
      this.LogDebug( "ItemGuid: " + ItemGuid );
      EvDataChange dataChange = new EvDataChange ( );
      string xmlDataChange = String.Empty;

      try
      {
        // 
        // Check that whether the change is encrypted.
        // 
        if ( encryptedData.Contains ( "<EvDataChange xmlns" ) == true )
        {
          this.LogDebug( " Unencrypted audit trail. " );

          // 
          // deserialise the xml object.
          // 
          dataChange = Evado.Model.Digital.EvcStatics.DeserialiseObject<EvDataChange> ( encryptedData );

          //
          // if default reinitialise.
          //
          if ( dataChange == default ( EvDataChange ) )
          {
            dataChange = new EvDataChange ( );
          }

          // 
          // Return the encrypted string.
          // 
          return dataChange;
        }
        this.LogDebug( " Encrypted audit trail. " );

        // 
        // Check that we have Guid keys. 
        // 
        if ( EvDataChanges._SiteGuid != Guid.Empty
          && ItemGuid != Guid.Empty )
        {
          // 
          // Initialise the encryption object.
          // 
          EvEncrypt encrypt = new EvEncrypt ( EvDataChanges._SiteGuid, ItemGuid );

          // 
          // Encrypt the data change xml object.
          // 
          xmlDataChange = encrypt.decryptString ( encryptedData );
          this.LogClass(  encrypt.Log );

        }//END decrypting the data change object.

        this.LogDebug( " xmlDataChange length: " + xmlDataChange.Length );

        // 
        // Deserialise the datachange object.
        // 
        dataChange = Evado.Model.Digital.EvcStatics.DeserialiseObject<EvDataChange> ( xmlDataChange );

        //
        // if default reinitialise.
        //
        if ( dataChange == default ( EvDataChange ) )
        {
          dataChange = new EvDataChange ( );
        }
      }
      catch ( Exception Ex )
      {
        string eventMessage = "Decrypting Error. "
           + "\r\n Exception: \r\n" + Evado.Model.Digital.EvcStatics.getException ( Ex );

        this.LogClass(  eventMessage );

        Evado.Model.Digital.EvcStatics.WriteToEventLog ( _eventLogSource, eventMessage, EventLogEntryType.Error );

        dataChange = new EvDataChange ( );
      }

      // 
      // Return the encrypted string.
      // 
      return dataChange;

    }//END decryptData method.
    #endregion

  }//END EvDataChanges classKD

}//END namespace Evado.Dal.Digital
