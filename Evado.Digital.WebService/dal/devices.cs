/* <copyright file="sqldal\EvUserRegistrations.cs" company="EVADO HOLDING PTY. LTD.">
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
 ****************************************************************************************/

using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;

//References to Evado specific libraries

using Evado.Digital.WebService.Model;


namespace Evado.Digital.WebService.Dal
{
  /// <summary>
  /// A business Component used to manage Ethics roles
  /// The Evado.Evado.User is used in most methods 
  /// and is used to store serializable information about an account
  /// </summary>
  public class Devices
  {
    #region Initialise Class variables and objects

    private static string _eventLogSource = ConfigurationManager.AppSettings [ "EventLogSource" ];

    //
    // Static constants
    //

    /// <summary>
    /// This constant selects all rows from ULD_REGISTERED_DEVICES table.
    /// </summary>
    private const string _sqlQuery_View = "Select * FROM ULD_REGISTERED_DEVICES ";

    private const string PARM_IDENTIFIER = "@IDENTIFIER";

    private const string PARM_USER_ID = "@USER_ID";

    private const string PARM_SERVICE_ID = "@SERVICE_ID";

    private const string PARM_DEVICE_ID = "@DEVICE_ID";

    private const string PARM_DEVICE_NAME = "@DEVICE_NAME";

    private const string PARM_DEVICE_OS = "@DEVICE_OS";

    private const string PARM_REGISTRATION_DATE = "@REGISTRATION_DATE";

    private string _DebugLog = String.Empty;
    /// <summary>
    /// This property contains the status of a device. 
    /// </summary>
    public string DebugLog
    {
      get { return _DebugLog; }
    }

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region SQL Parameter methods

    // =====================================================================================
    /// <summary>
    /// This method sets the update query properties. 
    /// </summary>
    /// <returns> An object of SqlParameter class</returns>
    // -------------------------------------------------------------------------------------
    private static SqlParameter [ ] getItemsParameters( )
    {
      SqlParameter [ ] parms = new SqlParameter [ ] 
      {
        new SqlParameter( PARM_IDENTIFIER, SqlDbType.UniqueIdentifier),
        new SqlParameter( PARM_SERVICE_ID, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_USER_ID, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_DEVICE_ID, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_DEVICE_NAME, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_DEVICE_OS, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_REGISTRATION_DATE, SqlDbType.DateTime),
      };

      return parms;
    }//END getItemsParameters method 

    // =====================================================================================
    /// <summary>
    /// This method assigns SqlParameter values with Device properties.  
    /// </summary>
    /// <param name="Device">Object: A Device object</param>
    /// <param name="parms"> Array: A  SqlParameter object</param>
    // -------------------------------------------------------------------------------------
    private void setUsersParameters( SqlParameter [ ] parms, Device Device )
    {
      parms [ 0 ].Value = Device.Identifier;
      parms [ 1 ].Value = Device.ServiceId;
      parms [ 2 ].Value = Device.UserId;
      parms [ 3 ].Value = Device.DeviceId;
      parms [ 4 ].Value = Device.DeviceName;
      parms [ 5 ].Value = Device.DeviceOs;
      parms [ 6 ].Value = Device.RegistrationDate;

    }//END setUsersParameters.

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Data Reader methods

    // =====================================================================================
    /// <summary>
    /// This method reads the content of the SqlDataReader into the Facility data object.
    /// </summary>
    /// <param name="Row">Object: Represents a row of data in a System.Data.DataTable.</param>
    /// <returns>A Device Object</returns>
    /// <remarks>
    /// This method consists of following stpes. 
    /// 
    /// 1. Initialise the local varaible. 
    /// 
    /// 2. Update the object properties.
    /// 
    /// 3. Retrun the device object.
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public Device readRow( DataRow Row )
    {
      // 
      // Initialise the local variable.
      // 
      Device device = new Device( );

      // 
      // Update the object properties.
      // 
      device.Identifier = EvSqlMethods.getGuid( Row, "ULD_IDENTIFIER" );
      device.ServiceId = EvSqlMethods.getString( Row, "SERVICE_ID" );
      device.ServiceId = EvSqlMethods.getString( Row, "USER_ID" );
      device.DeviceId = EvSqlMethods.getString( Row, "DEVICE_ID" );
      device.DeviceName = EvSqlMethods.getString( Row, "ULD_DEVICE_NAME" );
      device.DeviceOs = EvSqlMethods.getString( Row, "ULD_DEVICE_OS" );
      device.RegistrationDate = EvSqlMethods.getDateTime( Row, "ULD_REGISTRATION_DATE" );

      // 
      // Retrun the device object.
      // 
      return device;

    }// End readRow method.

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Device Query methods

    // =====================================================================================
    /// <summary>
    /// This method gets a list of a device object from a database.  
    /// </summary>
    /// <param name="DeviceName">String: A device name</param>
    /// <param name="DeviceOS">String: A device OS</param>
    /// <param name="UserId">String: A user id for a user</param>
    /// <returns>List: A list of a device object.</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Define the local variables.
    /// 
    /// 2. Define the SQL query parameters and load the query values.
    /// 
    /// 3. Generate the SQL query string.
    /// 
    /// 4. Add the filter parameters if needed.
    /// 
    /// 5. Execute the query against the database.
    /// 
    /// 6. Iterate through the results extracting the role information.
    /// 
    /// 7. Extract the table row.
    /// 
    /// 8. Return the ArrayList containing the User data object.
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<Device> GetView( String DeviceName, String DeviceOS, String UserId )
    {
      this._DebugLog = "Evado.Digital.WebService.Dal.Services.GetView method. "
        + " DeviceName: " + DeviceName
        + " DeviceOS: " + DeviceOS;

      // 
      // Define the local variables
      // 
      string sqlQueryString;
      List<Device> view = new List<Device>( );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( PARM_DEVICE_NAME, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_DEVICE_OS, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_USER_ID, SqlDbType.NVarChar, 100),
      };
      cmdParms [ 0 ].Value = DeviceName;
      cmdParms [ 1 ].Value = DeviceOS;
      cmdParms [ 2 ].Value = UserId;

      // 
      // Generate the SQL query string
      // 
      sqlQueryString = _sqlQuery_View;

      //
      // Add the filter parameters if needed.
      //
      if ( DeviceName != String.Empty )
      {
        sqlQueryString += " WHERE ULD_DEVICE_NAME = @DEVICE_NAME ";

        if ( DeviceOS != String.Empty )
        {
          sqlQueryString += " WHERE ULD_DEVICE_OS = @DEVICE_OS ";
        }
        sqlQueryString += " ORDER BY USER_ID";
      }
      else
      {
        sqlQueryString += " WHERE USER_ID = @USER_ID ";

        sqlQueryString += " ORDER BY ULD_DEVICE_NAME";
      }


      this._DebugLog += "\r\n" + sqlQueryString;
      this._DebugLog += "\r\nSQLHelper Status: " + EvSqlMethods.Status;

      //
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery( sqlQueryString, cmdParms ) )
      {
        // 
        // Iterate through the results extracting the role information.
        // 
        for ( int count = 0; count < table.Rows.Count; count++ )
        {
          // 
          // Extract the table row
          // 
          DataRow row = table.Rows [ count ];

          Device profile = this.readRow( row );

          view.Add( profile );
        }//END count iteration
      }
      this._DebugLog += "\r\n View count: " + view.Count.ToString( );

      // 
      // Return the ArrayList containing the User data object.
      // 
      return view;

    } //END GetView method.

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Retrieve Query methods

    // =====================================================================================
    /// <summary>
    /// This method gets the information for a device. 
    /// </summary>
    /// <param name="DeviceId">String: A device Id of a device </param>
    /// <returns>Object: A device object. </returns>
    /// <remarks>
    /// This method consists of following steps. </remarks>
    /// 
    /// 1. Define local variables.
    /// 
    /// 2. Check the whether device id is valid or not. If DeviceId is equal to an empty string, return a Device object.
    /// 
    /// 3. Define the SQL query parameters and load the query values.
    /// 
    /// 4. Generate the SQL query string.
    /// 
    /// 5. Execute the query against the database.
    /// 
    /// 6. If no rows found, return a device object.
    /// 
    /// 7. Extract the table row.
    /// 
    /// 8. Fill the Device object.
    /// 
    /// 9. Return the Device data object.
    ///
    // -------------------------------------------------------------------------------------
    public Device GetItem( String DeviceId )
    {
      this._DebugLog += "Evado.Digital.WebService.Dal.Services.GetItem. method DeviceId: " + DeviceId;
      // 
      // Define local variables
      // 
      string sqlQueryString;
      Device userRegistration = new Device( );

      // 
      // Check the whether device id is valid or not. If DeviceId is equal to an empty string, return a Device object.
      // 
      if ( DeviceId == String.Empty )
      {
        this._DebugLog += "\r\nUser Id null";
        return userRegistration;
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter cmdParms = new SqlParameter( PARM_DEVICE_ID, SqlDbType.Char, 100 );
      cmdParms.Value = DeviceId;
      // 
      // Generate the SQL query string
      // 
      sqlQueryString = _sqlQuery_View + " WHERE (DEVICE_ID = @DEVICE_ID);";
      this._DebugLog += "\r\n" + sqlQueryString;

      //
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery( sqlQueryString, cmdParms ) )
      {
        // 
        // If no rows found, return a device object
        // 
        if ( table.Rows.Count == 0 )
        {
          this._DebugLog += "\r\n Query result empty.";
          return userRegistration;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        // 
        // Fill the Device object.
        // 
        userRegistration = this.readRow( row );

        this._DebugLog += "\r\nUserRegistration.DeviceId: " + userRegistration.DeviceId;

      }//END Using 

      // 
      // Return the Device data object.
      // 
      return userRegistration;

    }//END GetItem method

    // =====================================================================================
    /// <summary>
    /// This method gets the information for a device.
    /// </summary>
    /// <param name="Identifier">GUID: An Identifier for a device</param>
    /// <returns>Object: A Device Data object</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Define local variables.
    /// 
    /// 2. Check that the identifier Id is valid or not. if identifier is equal to a empty GUID, return a device object.
    /// 
    /// 3. Define the SQL query parameters and load the query values.
    /// 
    /// 4. Generate the SQL query string.
    /// 
    /// 5. Execute the query against the database.
    /// 
    /// 6. If no rows found, return a device object.
    /// 
    /// 7. Extract the table row.
    /// 
    /// 8. Fill the role object.
    /// 
    /// 9. Return the device data object.
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public Device GetItem( Guid Identifier )
    {
      this._DebugLog += "Evado.Digital.WebService.Dal.Services.GetItem method Identifier: " + Identifier;
      // 
      // Define local variables
      // 
      string sqlQueryString;
      Device userProfile = new Device( );

      // 
      // Check that the identifier Id is valid or not. if identifier is equal to a empty GUID, return a device object.
      // 
      if ( Identifier == Guid.Empty )
      {
        return userProfile;
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter cmdParms = new SqlParameter( PARM_IDENTIFIER, SqlDbType.UniqueIdentifier );
      cmdParms.Value = Identifier;

      // 
      // Generate the SQL query string
      // 
      sqlQueryString = _sqlQuery_View + " WHERE (ULD_IDENTIFIER = @IDENTIFIER);";

      this._DebugLog = "\r\n" + sqlQueryString;

      //
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery( sqlQueryString, cmdParms ) )
      {
        // 
        // If no rows found, return a device object 
        // 
        if ( table.Rows.Count == 0 )
        {
          return userProfile;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        // 
        // Fill the role object.
        // 
        userProfile = this.readRow( row );

      }//END Using 


      // 
      // Return the device data object.
      // 
      return userProfile;

    }//END GetItem method

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Update Data methods
    // =====================================================================================
    /// <summary>
    ///This method updates the device in the database. 
    /// The update and add process are the same as in each execution the currentMonth objects are 
    /// set to superseded and then a new object is inserted to the database.
    /// </summary>
    /// <param name="MobileDevice">Object: A Device object</param>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. If ServiceId of a Device object is equal to an empty string, return an error message.
    /// 
    /// 2. If DeviceId of a Device object is equal to an empty string, return an error message.
    /// 
    /// 3. If Indentifier of a Device is equal to an empty GUID, add a device.
    /// 
    /// 4. Update a device.
    /// 
    /// </remarks>


    // -------------------------------------------------------------------------------------
    public Evado.Model.EvEventCodes saveItem( Device MobileDevice )
    {
      this._DebugLog += "Evado.Digital.WebService.Dal.Services.saveItem method ";

      // 
      // If ServiceId of a Device object is equal to an empty string, return an error message.
      // 
      if ( MobileDevice.ServiceId == String.Empty )
      {
        this._DebugLog += "\r\n Service Id error";
        return Evado.Model.EvEventCodes.Identifier_Service_Id_Error;
      }

      //
      // If DeviceId of a Device object is equal to an empty string, return an error message.
      //
      if ( MobileDevice.DeviceId == String.Empty )
      {
        this._DebugLog += "\r\n Device Id error";
        return Evado.Model.EvEventCodes.Identifier_Device_Id_Error;
      }

      //
      // If Indentifier of a Device is equal to an empty GUID, add a device.
      //
      if ( MobileDevice.Identifier == Guid.Empty )
      {
        return this.AddItem( MobileDevice );
      }

      //
      // Update a device.
      //
      return this.UpdateItem( MobileDevice );

    } //END saveItem method

    // =====================================================================================
    /// <summary>
    /// This method updates the Device table. 
    /// </summary>
    /// <param name="MobileDevice">Object: A Device object</param>
    /// <returns>Object: Evado.Model.EvEventCodes </returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Define the local variables. 
    /// 
    /// 2. Get the old device id for update. if old device is not found, return an error messege.
    /// 
    /// 3. Define the SQL query parameters and load the query values.
    /// 
    /// 4. Define the update record query.  
    /// 
    /// 5. Execute the update groupCommand.
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private Evado.Model.EvEventCodes UpdateItem( Device MobileDevice )
    {
      this._DebugLog += "Evado.Digital.WebService.Dal.Services.UpdateItem method. "
        + " DeviceId: " + MobileDevice.DeviceId + "\r\n";

      // 
      // Define the local variables.
      // 
      Device oldDevice = new Device( );

      // 
      // Get the old device id for to verify that the device exists and instrument differential
      // comparision.
      // 
      oldDevice = this.GetItem( MobileDevice.DeviceId );
      if ( oldDevice.Identifier == Guid.Empty )
      {
        return Evado.Model.EvEventCodes.Data_InvalidId_Error;
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = getItemsParameters( );
      setUsersParameters( cmdParms, MobileDevice );

      //
      // Define the update record query  
      //
      String addQuery = "UPDATE	ULD_REGISTERED_DEVICES \r\n"
        + "SET\r\n"
        + " ULD_DEVICE_NAME = @ULD_DEVICE_NAME,  \r\n"
        + " ULD_DEVICE_OS = @ULD_DEVICE_OS,  \r\n"
        + " UR_REGISTRATION_DATE = @REGISTRATION_DATE \r\n"
        + " WHERE UR_IDENTIFIER = @IDENTIFIER;";

      //
      // Execute the update groupCommand.
      //
      if ( EvSqlMethods.QueryUpdate( addQuery, cmdParms ) == 0 )
      {
        return Evado.Model.EvEventCodes.Database_Record_Update_Error;
      }

      return Evado.Model.EvEventCodes.Ok;

    }//END UpdateItem method

    // =====================================================================================
    /// <summary>
    /// This method adds a record to the ULD_REGISTERED_DEVICES tables. 
    /// </summary>
    /// <param name="Device">Object: A Device object</param>
    /// <returns>Object:  Evado.Model.EvEventCodes </returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Define the local variables. 
    /// 
    /// 2. Define the GUID for the device of one is not allocated. 
    /// 
    /// 3. Define the query parameters and load the query values. 
    /// 
    /// 4. Define the add record update query.
    /// 
    /// 5. Execute the update groupCommand.
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private Evado.Model.EvEventCodes AddItem( Device Device )
    {
      // 
      // Define the local variables.
      // 
      this._DebugLog += "Evado.Digital.WebService.Dal.Services.addItem method. DeviceId: " + Device.DeviceId + "\r\n";

      Device oldDevice = this.GetItem( Device.DeviceId );
      if ( oldDevice.Identifier != Guid.Empty )
      {
        this._DebugLog += "\r\n Duplicate device";
      }

      // 
      // Define the GUID for the device of one is not allocated. 
      // 
      if ( Device.Identifier == Guid.Empty )
      {
        Device.Identifier = Guid.NewGuid( );
      }

      this._DebugLog += "\r\n Adding Device.";


      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = getItemsParameters( );
      setUsersParameters( cmdParms, Device );

      //
      // Define the add record update query
      //
      String addQuery = "Insert Into ULD_REGISTERED_DEVICES \r\n"
        + " (ULD_IDENTIFIER, USER_ID, SERVICE_ID, DEVICE_ID, ULD_DEVICE_NAME, ULD_DEVICE_OS, ULD_REGISTRATION_DATE ) \r\n"
        + "values \r\n"
        + " (@IDENTIFIER, @USER_ID, @SERVICE_ID, @DEVICE_ID, @DEVICE_NAME, @DEVICE_OS, @REGISTRATION_DATE);";

      this._DebugLog += "\r\n " + addQuery;

      //
      // Execute the update groupCommand.
      //
      if ( EvSqlMethods.QueryUpdate( addQuery, cmdParms ) == 0 )
      {
        return Evado.Model.EvEventCodes.Database_Record_Update_Error;
      }

      return Evado.Model.EvEventCodes.Ok;

    } //END AddItem method

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

  }//END Users class

} // Close namespace Evado.Dal
