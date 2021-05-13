/***************************************************************************************
 * <copyright file="dal\EvSqlMethods.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2021 EVADO HOLDING PTY. LTD.  All rights reserved.
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

//===============================================================================
// This file is based on the Microsoft Data Access Application Block for .NET
// For more information please go to 
// http://msdn.microsoft.com/library/en-us/dnbda/html/daab-rm.asp
//===============================================================================

using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics; 

using Evado.Model;
using Evado.Digital.Model;

namespace Evado.Digital.Dal
{
  /// <summary>
  /// The SqlHelper class is intended to encapsulate high performance, 
  /// scalable best practices for common uses of SqlClient.
  /// </summary>
  public abstract class EvSqlMethods
  {
    #region class static variables.
    //
    // Database connection strings
    //
    private static string _EventLogSource = ConfigurationManager.AppSettings [ "EventLogSource" ];
    /// <summary>
    /// This property sets the event log source.
    /// </summary>
    public static string EventLogSource
    {
      get { return EvSqlMethods._EventLogSource; }
      set { EvSqlMethods._EventLogSource = value; }
    }

    private static int _CommandTimeOut = 30;

    /// <summary>
    /// this static property set the connection time out period.
    /// </summary>
    public static int CommandTimeOut
    {
      get { return EvSqlMethods._CommandTimeOut; }
      set { EvSqlMethods._CommandTimeOut = value; }
    }

    //
    // Hashtable to store cached parameters
    //
    private static Hashtable parmCache = Hashtable.Synchronized ( new Hashtable ( ) );
    /// <summary>
    /// This field contains the class debug text.
    /// </summary>
    public static string Log = String.Empty;

    #endregion

    #region Set Connection Settings Property
    
    private static String _ConnectionStringKey = "EVADO";
    private static string _connectionString = String.Empty;

    /// <summary>
    /// This static property sets the connection string key.
    /// </summary>
    public static String ConnectionStringKey
    {
      set
      {
        EvSqlMethods._ConnectionStringKey = value;

        EvSqlMethods._connectionString = ConfigurationManager.ConnectionStrings [ EvSqlMethods._ConnectionStringKey ].ConnectionString;
      }

      get
      {
        return EvSqlMethods._ConnectionStringKey;
      }
    }

    /// <summary>
    /// This static property sets the connection string.
    /// </summary>
    public static String ConnectionString
    {

      get
      {
        return EvSqlMethods._connectionString;
      }
    }

    #endregion

    #region Query Methods
    // ==================================================================================
    /// <summary>
    /// This class returns a string of SQL query parameters.
    /// </summary>
    /// <param name="CommandParameters">SqlParameter: an array of SqlParamters used to execute the command</param>
    /// <returns>DataTable: a DataSet of query results.</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialise the internal variables
    /// 
    /// 2. Validate whether the connection string is not empty
    /// 
    /// 3. Try - Validate the SqlQery string and execute the query
    /// 
    /// 4. Open connection, create sql command, call storeprocedure, fill in dataset, return
    /// a datatable and clear parameters.. 
    /// 
    /// 5. Catch - Extract parameters and write error log  
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public static String ListParameters ( SqlParameter [ ] CommandParameters )
    {
      // 
      // Initialise the local variables and objects
      // 
      String value = String.Empty  ;
        // 
        // Extract the parameters
        //
        if ( CommandParameters != null )
        {
          value += "Parameters:\r\n";
          foreach ( SqlParameter prm in CommandParameters )
          {
            value += "- Name: '" + prm.ParameterName
              + "', Value: '" + prm.Value 
              + "', Type: '" + prm.DbType + "' \r\n";
          }
        }

      // 
      // Return the results from the query.
      // 
        return value;

    }//END RunQuery method

    //===================================================================================
    /// <summary>
    /// This method geneates a SQL string containing the command parameters that
    /// can be appended to an SQL query.
    /// </summary>
    /// <param name="CommandParameters">Array or SqlParameter objects</param>
    /// <returns>String: containing SQL statements.</returns>
    //-----------------------------------------------------------------------------------
    public static String getParameterSqlText ( params SqlParameter [ ] CommandParameters )
    {
      String SqlString = "/******** SQL *********/\r\n";

      //
      // Iterate through the parameter list.
      // Declaring and setting the parameter value as SQL statements.
      //
      foreach ( SqlParameter cmdParm in CommandParameters )
      {

        if ( cmdParm.Size == 0 )
        {
          SqlString += "DECLARE " + cmdParm.ParameterName + " " + cmdParm.SqlDbType + "\r\n";
        }
        else
        {
          SqlString += "DECLARE " + cmdParm.ParameterName + " "
            + cmdParm.SqlDbType + "(" + cmdParm.Size + ") \r\n";
        }

        SqlString += "SET " + cmdParm.ParameterName + "='" + cmdParm.SqlValue + "'\r\n\r\n";

      }
      return SqlString;
    }

    // ==================================================================================
    /// <summary>
    /// This class executes a SqlCommand that returns a resultset against the database specified in the connection string 
    /// using the provided parameters.
    /// </summary>
    /// <param name="CommandType">CommandType: the CommandType (stored procedure, text, etc.)</param>
    /// <param name="CommandText">String: the stored procedure name or T-SQL command</param>
    /// <param name="CommandParameters">SqlParameter: an array of SqlParamters used to execute the command</param>
    /// <returns>SqlDataReader: A SqlDataReader containing the results</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize a status string of the methods
    /// 
    /// 2. Try validating the string connection and initialize the sqlcommand and sqlconnection    
    /// 
    /// 3. Try executing the command and storing the result to a sql data reader object.
    /// Catch parameter error message. 
    /// 
    /// 4. Catch connection error message. 
    /// 
    /// e.g.:  
    ///  SqlDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public static SqlDataReader ExecuteReader (
      CommandType CommandType,
      string CommandText,
      params SqlParameter [ ] CommandParameters )
    {
      //
      // Initialize a status string of the methods
      //
      Log = "Evado.Digital.Dal.EvSqlMethods.ExecuteReader method";
      string stError = String.Empty;
      try
      {
        //
        // If there is an empty connection string return an empty Sql data reader.
        //
        if ( _connectionString == String.Empty )
        {
          stError = "Empty connection string encountered."
            + "\r\n ConnectionStringKey: " + EvSqlMethods._ConnectionStringKey
            + "\r\n CommandText: " + CommandText;

          Evado.Model.EvStatics.WriteToEventLog ( _EventLogSource, stError, EventLogEntryType.Error );
          return null;
        }

        Log += "ConnectionStringKey: " + EvSqlMethods._ConnectionStringKey
          + "\r\n CommandText: " + CommandText;

        //
        // Initialize the sqlcommand and sqlconnection
        //
        SqlCommand cmd = new SqlCommand ( );
        SqlConnection conn = new SqlConnection ( _connectionString );

        //
        // we use a try/catch here because if the method throws an exception we want to 
        // close the connection throw code, because no datareader will exist, hence the 
        // commandBehaviour.CloseConnection will not work
        //
        try
        {
          //
          // Execute the command and store the result to a sql data reader object. 
          //
          PrepareCommand ( cmd, conn, null, CommandType, CommandText, CommandParameters );
          SqlDataReader rdr = cmd.ExecuteReader ( CommandBehavior.CloseConnection );
          cmd.Parameters.Clear ( );
          return rdr;
        }
        catch ( Exception Ex )
        {
          conn.Close ( );

          // 
          // Extract the parameters
          //
          string parameters = "\r\n Parameters:";
          foreach ( SqlParameter prm in CommandParameters )
          {
            parameters += "\r\n Typ: " + prm.DbType
              + ", Parm: " + prm.ParameterName
              + ", Val: " + prm.Value;
          }

          //
          // Create the event message
          //
          string eventMessage = "Connection String" + _connectionString
            + "\r\n Command text: \r\n" + CommandText
            + parameters
            + "\r\n Exception: \r\n" + Evado.Model.EvStatics.getException ( Ex );

          Evado.Model.EvStatics.WriteToEventLog ( _EventLogSource, eventMessage, EventLogEntryType.Error );

          throw ( Ex );

        }//END try catch of command execution. 

      }
      catch ( Exception Ex )
      {
        //
        // Create the event message
        //
        string eventMessage = "Connection exception:"
          + " \r\n connection key: " + EvSqlMethods._ConnectionStringKey
          + "\r\n Connection string: " + EvSqlMethods._connectionString
          + "\r\n Exception: \r\n" + Evado.Model.EvStatics.getException ( Ex );

         Evado.Digital.Model.EvcStatics.WriteToEventLog ( _EventLogSource, eventMessage, EventLogEntryType.Error );

        throw ( Ex );

      }//END try catch of connection checking

    }//END ExecuteReader method

    // ==================================================================================
    /// <summary>
    /// This class executes a query against the database.
    /// </summary>
    /// <param name="SqlQuery">string: a sql query string</param>
    /// <returns>DataTable: DataSet of query results.</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialise the method status, a sql adapter, a querydataset and a querytable
    /// 
    /// 2. Validate whether the connection string is not empty
    /// 
    /// 3. Try - Validate the SqlQery and execute the query
    /// 
    /// 4. Open connection, create sql command, call storeprocedure, fill in dataset and return
    /// a datatable. 
    /// 
    /// 5. Catch - error message: status, connection, query and exception. 
    /// </remarks>
    //  --------------------------------------------------------------------------------
    public static DataTable RunQuery ( string SqlQuery )
    {
      // 
      // Initialise the method status, a sql adapter, a querydataset and a querytable. 
      // 
      Log = "Evado.Digital.Dal.EvSqlMethods.RunQuery method";
      SqlDataAdapter adapter = new SqlDataAdapter ( );
      DataSet queryDataSet = new DataSet ( );
      DataTable queryTable = new DataTable ( );

      //
      // If there is an empty connection string return an empty Sql data reader.
      //
      if ( _connectionString == String.Empty )
      {
        string stError = "Empty connection string encountered."
          + "\r\n ConnectionStringKey: " + EvSqlMethods._ConnectionStringKey
          + "\r\n SqlQuery: " + SqlQuery;

        Evado.Model.EvStatics.WriteToEventLog ( _EventLogSource, stError, EventLogEntryType.Error );
        return queryTable;
      }

      // 
      // Set up the try catch structure
      // 
      try
      {
        // 
        // Validate the method parameters
        // 
        if ( SqlQuery == String.Empty )
        {
          return queryTable;
        }

        // 
        // Execute the query
        // 
        using ( SqlConnection connection = new SqlConnection ( _connectionString ) )
        {
          //
          // Open the connection.
          // 
          connection.Open ( );

          // 
          // Create a SqlCommand to retrieve query data.
          // 
          SqlCommand command = new SqlCommand ( SqlQuery, connection );
          command.CommandType = CommandType.Text;

          // 
          // Set the SqlDataAdapter's SelectCommand.
          // 
          adapter.SelectCommand = command;

          // 
          // Fill the DataSet.
          // 
          adapter.Fill ( queryDataSet );

          // 
          // Extract the event log table
          // 
          queryTable = queryDataSet.Tables [ 0 ];

        }
      }
      catch ( Exception Ex )
      {
        string eventMessage = "Status: + " + EvSqlMethods.Log
          + "\r\n Connection String" + _connectionString
          + "\r\n Query: \r\n" + SqlQuery
          + "\r\n Exception: \r\n" + Evado.Model.EvStatics.getException ( Ex );

         Evado.Digital.Model.EvcStatics.WriteToEventLog ( _EventLogSource, eventMessage, EventLogEntryType.Error );

        throw ( Ex );
      }
      // 
      // Return the results from the query.
      // 
      return queryTable;

    }//END RunQuery method

    // ==================================================================================
    /// <summary>
    /// This class executes a query against the database.
    /// </summary>
    /// <param name="SqlQuery">string: The query string</param>
    /// <param name="CommandParameters">SqlParameter: an array of SqlParamters used to execute the command</param>
    /// <returns>DataTable: a DataSet of query results.</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialise the internal variables
    /// 
    /// 2. Validate whether the connection string is not empty
    /// 
    /// 3. Try - Validate the SqlQery string and execute the query
    /// 
    /// 4. Open connection, create sql command, call storeprocedure, fill in dataset, return
    /// a datatable and clear parameters.. 
    /// 
    /// 5. Catch - Extract parameters and write error log  
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public static DataTable RunQuery ( string SqlQuery, params SqlParameter [ ] CommandParameters )
    {
      // 
      // Initialise the local variables and objects
      // 
      Log = "Evado.Digital.Dal.EvSqlMethods.RunQuery method";
      SqlCommand command = new SqlCommand ( );
      SqlDataAdapter adapter = new SqlDataAdapter ( );
      DataSet queryDataSet = new DataSet ( );
      DataTable queryTable = new DataTable ( );

      //
      // If there is an empty connection string return an empty Sql data reader.
      //
      if ( _connectionString == String.Empty )
      {
        string stError = "Empty connection string encountered."
          + "\r\n ConnectionStringKey: " + EvSqlMethods._ConnectionStringKey
          + "\r\n SqlQuery: " + SqlQuery;


         Evado.Digital.Model.EvcStatics.WriteToEventLog ( _EventLogSource, stError, EventLogEntryType.Error );
        return queryTable;
      }

      Log += "\r\nConnectionStringKey: " + EvSqlMethods._ConnectionStringKey;
      Log += "\r\n_connectionString: " + _connectionString;

      // 
      // Set up the try catch structure
      // 
      try
      {
        // 
        // Validate the method parameters
        // 
        if ( SqlQuery == String.Empty )
        {
          return queryTable;
        }

        // 
        // Execute the query
        // 
        using ( SqlConnection connection = new SqlConnection ( _connectionString ) )
        {
          // 
          // Prepare the command
          // 
          PrepareCommand ( command, connection, null,
            CommandType.Text, SqlQuery, CommandParameters );

          // 
          // Set the SqlDataAdapter's SelectCommand.
          // 
          adapter.SelectCommand = command;

          // 
          // Fill the DataSet.
          // 
          adapter.Fill ( queryDataSet );

          // 
          // Extract the event log table
          // 
          queryTable = queryDataSet.Tables [ 0 ];

          Log += "\r\n Table Count: " + queryDataSet.Tables.Count;
          // 
          // Clear Parameters
          // 
          command.Parameters.Clear ( );

        }//END executing query

      }
      catch ( Exception Ex )
      {
        // 
        // Extract the parameters
        //
        if ( CommandParameters != null )
        {
          Log += "\r\n Parameters:";
          foreach ( SqlParameter prm in CommandParameters )
          {
            Log += "\r\n Type: " + prm.DbType
              + ", Name: " + prm.ParameterName
              + ", Value: " + prm.Value;
          }
        }

        //
        // Create the event message
        //
        string eventMessage = "Status: + " + EvSqlMethods.Log
          + "\r\n Connection String" + _connectionString
          + "\r\n Query: \r\n" + SqlQuery
          + "\r\n Exception: \r\n" +  Evado.Digital.Model.EvcStatics.getException ( Ex );

         Evado.Digital.Model.EvcStatics.WriteToEventLog ( _EventLogSource, eventMessage, EventLogEntryType.Error );

        throw ( Ex );

      }//END try-catch

      // 
      // Return the results from the query.
      // 
      return queryTable;

    }//END RunQuery method

    #endregion

    #region Data Retrieval Methods

    #region Row methods

    // =====================================================================================
    /// <summary>
    /// This class checks whether the fieldname index has values
    /// </summary>
    /// <param name="Row">DataRow: a row data object</param>
    /// <param name="FieldName">string: a field name</param>
    /// <returns>Boolean: true, if the fieldName index has value</returns>
    // -------------------------------------------------------------------------------------
    public static bool hasValue ( DataRow Row, string FieldName )
    {
      if ( Row [ FieldName ] == DBNull.Value )
      {
        return false;
      }
      return true;

    }//END hasValue method

    // =====================================================================================
    /// <summary>
    /// This class reads the row object and returns the newField name as a Guid.
    /// </summary>
    /// <param name="Row">DataRow: a row data object</param>
    /// <param name="FieldName">string: a field name</param>
    /// <returns>Guid: a global unique identifier string</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Retrieve the Guid value based on the Fieldname index of the row
    /// 
    /// 2. Return the Guid if it exists. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public static Guid getGuid ( DataRow Row, string FieldName )
    {
      // 
      // Extract the newField if it is not null.
      // 
      if ( Row [ FieldName ] != DBNull.Value )
      {
        // 
        // Extract is to a string.
        // 
        string sGuid = Row [ FieldName ].ToString ( );

        // 
        // If string is 36 characters long parse it as a FormUid.
        // 
        if ( sGuid.Length == 36 )
        {
          try
          {
            return new Guid ( sGuid );
          }
          catch
          {
            return Guid.Empty;
          }

        }
        return Guid.Empty;
      }

      return Guid.Empty;

    }//END getGuid method

    // =====================================================================================
    /// <summary>
    /// This class reads the row object and returns the newField name as a Integer.
    /// </summary>
    /// <param name="Row">DataRow: a data row object</param>
    /// <param name="FieldName">string: a field name</param>
    /// <returns>Integer: the integer value</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Retrieve the integer value based on the Fieldname index of the row
    /// 
    /// 2. Return the integer value if it exists. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public static int getInteger ( DataRow Row, string FieldName )
    {
      int iValue = 0;

      if ( Row [ FieldName ] != DBNull.Value )
      {
        if ( int.TryParse ( Row [ FieldName ].ToString ( ), out iValue ) == false )
        {
          return  Evado.Digital.Model.EvcStatics.CONST_INTEGER_NULL;
        }
      }

      return iValue;

    }//END getInteger method

    // =====================================================================================
    /// <summary>
    /// This class reads the row object and returns the newField name as a long integer.
    /// </summary>
    /// <param name="Row">DataRow: a data row object</param>
    /// <param name="FieldName">string: a field name</param>
    /// <returns>Long: the long value</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Retrieve the long value based on the Fieldname index of the row
    /// 
    /// 2. Return the long value if it exists. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public static long getLong ( DataRow Row, string FieldName )
    {
      long iValue = 0;

      if ( Row [ FieldName ] != DBNull.Value )
      {
        if ( long.TryParse ( Row [ FieldName ].ToString ( ), out iValue ) == false )
        {
          return 0;
        }
      }

      return iValue;

    }//END getLong method

    // =====================================================================================
    /// <summary>
    /// This class reads the row object and returns the newField name as a DateTime value.
    /// </summary>
    /// <param name="Row">DataRow: a data row object</param>
    /// <param name="FieldName">string: a field name</param>
    /// <returns>DateTime: the date time value</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Retrieve the DateTime value based on the Fieldname index of the row
    /// 
    /// 2. Return the DateTime value if it exists. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public static DateTime getDateTime ( DataRow Row, string FieldName )
    {
      DateTime dValue = DateTime.Parse ( "1 Jan 1900" );

      if ( Row [ FieldName ] != DBNull.Value )
      {
        if ( DateTime.TryParse ( Row [ FieldName ].ToString ( ), out dValue ) == false )
        {
          return DateTime.Parse ( "1 Jan 1900" );
        }
      }

      return dValue;

    }//END getDateTime method

    // =====================================================================================
    /// <summary>
    /// This class reads the row object and returns the newField name as a string.
    /// </summary>
    /// <param name="Row">DataRow: a data row object</param>
    /// <param name="FieldName">string: a field name</param>
    /// <returns>string: the string value</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Retrieve the string value based on the Fieldname index of the row
    /// 
    /// 2. Return the string value if it exists. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public static string getString ( DataRow Row, string FieldName )
    {
      if ( Row [ FieldName ] != DBNull.Value )
      {
        return Row [ FieldName ].ToString ( ).Trim ( );
      }
      return String.Empty;

    }//END getString method

    // =====================================================================================
    /// <summary>
    /// This class reads the row object and returns the newField name as a byte value.
    /// </summary>
    /// <param name="Row">DataRow: a data row object</param>
    /// <param name="FieldName">string: a field name</param>
    /// <returns>byte: the byte value</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Retrieve the byte value based on the Fieldname index of the row
    /// 
    /// 2. Return the byte value if it exists. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public static byte [ ] getBytes ( DataRow Row, string FieldName )
    {
      if ( Row [ FieldName ] != DBNull.Value )
      {
        return (byte [ ]) Row [ FieldName ];
      }
      return null;

    }//END getBytes method

    // =====================================================================================
    /// <summary>
    /// This class reads the row object and returns the newField name as a float value.
    /// </summary>
    /// <param name="Row">DataRow: a data row object</param>
    /// <param name="FieldName">string: a field name</param>
    /// <returns>float: the float value</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Retrieve the float value based on the Fieldname index of the row
    /// 
    /// 2. Return the float value if it exists. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public static float getFloat ( DataRow Row, string FieldName )
    {
      float fValue = 0;

      if ( Row [ FieldName ] != DBNull.Value )
      {
        if ( float.TryParse ( Row [ FieldName ].ToString ( ), out fValue ) == false )
        {
          return 0;
        }
      }

      return fValue;

    }//END getFloat method

    // =====================================================================================
    /// <summary>
    /// This class reads the row object and returns the newField name as a bool value.
    /// </summary>
    /// <param name="Row">DataRow: a data row object</param>
    /// <param name="FieldName">string: a field name</param>
    /// <returns>bool: the bool value</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Retrieve the bool value based on the Fieldname index of the row
    /// 
    /// 2. Return the bool value if it exists. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public static bool getBool ( DataRow Row, string FieldName )
    {
      // 
      // if it exits.
      // 
      if ( Row [ FieldName ] != DBNull.Value )
      {
        string sValue = Row [ FieldName ].ToString ( ).ToLower ( );

        // 
        // If string value can be interpreted as a boolean true set true.
        // 
        if ( sValue.Contains ( "1" ) == true
          || sValue.Contains ( "true" ) == true
          || sValue.Contains ( "yes" ) == true
          || sValue.Contains ( "y" ) == true )
        {
          return true;
        }
      }

      return false;

    }//END getBoolean method

    // =====================================================================================
    /// <summary>
    /// This class reads the row object and returns the newField name as a enumerated type.
    /// </summary>
    /// <typeparam name="EnumerationType">Defines the enumerated list the value is to be covered to.</typeparam>
    /// <param name="Row">DataRow: a data row object</param>
    /// <param name="FieldName">string: a field name</param>
    /// <returns>string: the string value</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Retrieve the string value based on the Fieldname index of the row
    /// 
    /// 2. Return the string value if it exists. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public static EnumerationType getString<EnumerationType> ( DataRow Row, string FieldName )
    {
      if ( Row [ FieldName ] != DBNull.Value )
      {
        return Evado.Model.EvStatics.parseEnumValue<EnumerationType> ( Row [ FieldName ].ToString ( ) );
      }
      return default ( EnumerationType ); 

    }//END getString method

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Reader column count methods
    /*
    // =====================================================================================
    /// <summary>
    /// public getGuid static method
    /// 
    /// Description:
    /// Reads the row object and returns the newField name as a Gu
    /// </summary>
    /// <param name="Row">OdbcDataReader</param>
    /// <param name="_id.
    /// Document">Document</param>
    // -------------------------------------------------------------------------------------
    public static DocumentGuid getGuid ( SqlDataReader Reader, int Column )
    {
      if ( Reader [ Column ] != null )
      {
        string sValue = Reader [ Column ].ToString( ).Trim( );
        if ( sValue.Length == 36 )
        {
          return new DocumentGuid( sValue );
        }
      }

      return DocumentGuid.Empty;

    }//END getGuid static method

    // =====================================================================================
    /// <summary>
    /// public getInteger static method
    /// 
    /// Description:
    /// Reads the row object and returns the newField name as a Integer.
    /// 
    /// </summary>
    /// <param name="Row">OdbcDataReader</param>
    /// <param name="document">Document</param>
    // -------------------------------------------------------------------------------------
    public static int getInteger ( SqlDataReader Reader, int Column )
    {
      int iValue = 0;

      if ( Reader [ Column ] != null )
      {
        if ( int.TryParse( Reader [ Column ].ToString( ), out iValue ) == false )
        {
          return 0;
        }
      }

      return iValue;

    }//END getInteger static method

    // =====================================================================================
    /// <summary>
    /// public getDateTime static method
    /// 
    /// Description:
    /// Reads the row object and returns the newField name as a DateTime object.
    /// 
    /// </summary>
    /// <param name="Row">OdbcDataReader</param>
    /// <param name="document">Document</param>
    // -------------------------------------------------------------------------------------
    public static DateTime getDateTime ( SqlDataReader Reader, int Column )
    {
      DateTime dValue = DateTime.Parse( "1 Jan 1900" );

      if ( Reader [ Column ] != null )
      {
        if ( DateTime.TryParse( Reader [ Column ].ToString( ), out dValue ) == false )
        {
          return DateTime.Parse( "1 Jan 1900" );
        }
      }

      return dValue;

    }//END getDateTime static method

    // =====================================================================================
    /// <summary>
    /// public getString static method
    /// 
    /// Description:
    /// Reads the row object and returns the newField name as a string.
    /// 
    /// </summary>
    /// <param name="Row">OdbcDataReader</param>
    /// <param name="document">Document</param>
    // -------------------------------------------------------------------------------------
    public static string getString ( SqlDataReader Reader, int Column )
    {
      if ( Reader [ Column ] != null )
      {
        return Reader [ Column ].ToString( ).Trim( );
      }
      return String.Empty;

    }//END getString static method

    // =====================================================================================
    /// <summary>
    /// public getFloat static method
    /// 
    /// Description:
    /// Reads the row object and returns the newField name as a FormUid.
    /// 
    /// </summary>
    /// <param name="Row">OdbcDataReader</param>
    /// <param name="document">Document</param>
    // -------------------------------------------------------------------------------------
    public static float getFloat ( SqlDataReader Reader, int Column )
    {
      float fValue = 0 ;

      if ( Reader [ Column ] != null )
      {
        if ( float.TryParse( Reader [ Column ].ToString( ), out fValue ) == false )
        {
          return 0 ;
        }
      }

      return fValue;

    }//END getFloat static method
    */
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Update Methods

    // =====================================================================================
    /// <summary>
    /// This class executes a SqlCommand (that returns no resultset) against the database specified in the connection string 
    /// using a stored procedure and the provided parameters.
    /// </summary>
    /// <param name="StoreProcedureName">string: the name of the stored procedure to be used</param>
    /// <param name="CommandParameters">SqlParameter: an array of SqlParamters used to execute the command</param>
    /// <returns>Integer: an int representing the number of rows affected by the command</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize a result number, open connection and open transaction
    /// 
    /// 2. Try - execute non query command to the result number and commit transaction
    /// 
    /// 3. Catch - roll back transaction and write event log
    /// 
    /// e.g.:  
    ///  int result = StoreProcUpdate("PublishOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public static int StoreProcUpdate ( string StoreProcedureName, params SqlParameter [ ] CommandParameters )
    {
      Log = "Evado.Digital.Dal.EvSqlMethods.StoreProcUpdate method";

      //
      // Initialize a result number. 
      //
      int iResult = 0;

      //
      // Open a sql connection
      //
      using ( SqlConnection conn = new SqlConnection ( _connectionString ) )
      {
        conn.Open ( );

        //
        // Open a sql transaction
        //
        using ( SqlTransaction trans = conn.BeginTransaction ( ) )
        {
          try
          {
            //
            // Execute a non query command and commit the transaction
            //
            iResult = EvSqlMethods.ExecuteNonQuery (
              trans,
              CommandType.StoredProcedure,
              StoreProcedureName,
              CommandParameters );
            trans.Commit ( );
          }
          catch ( Exception Ex )
          {
            //
            // Catch an exception- Roll back the transaction and write out the eventlog. 
            //
            trans.Rollback ( );

            Log += "\r\n Connection String" + _connectionString
            + "\r\n StoreProcedureName: " + StoreProcedureName;

            // 
            // Extract the parameters
            //
            if ( CommandParameters != null )
            {
              Log += "\r\n Parameters:";
              foreach ( SqlParameter prm in CommandParameters )
              {
                Log += "\r\n Type: " + prm.DbType
                  + ", Name: " + prm.ParameterName
                  + ", Value: " + prm.Value;
              }
            }

            string eventMessage = "Status: + " + EvSqlMethods.Log
              + "\r\n Exception: \r\n" +  Evado.Digital.Model.EvcStatics.getException ( Ex );

            EventLog.WriteEntry ( _EventLogSource, eventMessage, EventLogEntryType.Error );
            throw ( Ex );

          }//END Try-Catch non sql query. 

        }//END using sqlTransaction

        return iResult;

      }//END using sqlconnection

    }//END StoreProcUpdate method

    // =====================================================================================
    /// <summary>
    /// This class executes a SqlCommand (that returns no resultset) against the database specified in the connection string 
    /// using a stored procedure and the provided parameters.
    /// </summary>
    /// <param name="UpdateCommand">string: the name of the stored procedure to be used</param>
    /// <param name="CommandParameters">SqlParameter: an array of SqlParamters used to execute the command</param>
    /// <returns>Integer: an int representing the number of rows affected by the command</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 
    /// e.g.:  
    ///  int result = StoreProcUpdate("PublishOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public static int QueryUpdate ( string UpdateCommand, params SqlParameter [ ] CommandParameters )
    {
      //
      // Initialize a result number
      //
      int iResult = 0;

      //
      // Open a sqlconnection
      //
      using ( SqlConnection conn = new SqlConnection ( _connectionString ) )
      {
        conn.Open ( );

        //
        // Open a sql transaction
        //
        using ( SqlTransaction trans = conn.BeginTransaction ( ) )
        {
          try
          {
            //
            // Execute the non query command to a result number and commit the transaction.
            //
            iResult = EvSqlMethods.ExecuteNonQuery (
              trans, CommandType.Text, UpdateCommand, CommandParameters );
            trans.Commit ( );
          }
          catch ( Exception Ex )
          {
            //
            // Rollback the transaction and write the eventlog message
            //
            trans.Rollback ( );

            string eventMessage = "Status: + " + EvSqlMethods.Log
              + "\r\n Connection String" + _connectionString
              + "\r\n Query: \r\n" + UpdateCommand
              + "\r\n Exception: \r\n" +  Evado.Digital.Model.EvcStatics.getException ( Ex );

             Evado.Digital.Model.EvcStatics.WriteToEventLog ( _EventLogSource, eventMessage, EventLogEntryType.Error );
            throw ( Ex );

          }//END try-catch non query execution

        }//END using sql transaction

        return iResult;

      }//END using sql connection

    }//END QueryUpdate method

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Private methods

    // ==================================================================================
    /// <summary>
    /// This class executes a SqlCommand (that returns no resultset) against the database specified in the connection string 
    /// using the provided parameters.
    /// </summary>
    /// <param name="connString">string: a valid connection string for a SqlConnection</param>
    /// <param name="cmdType">CommandType: the CommandType (stored procedure, text, etc.)</param>
    /// <param name="cmdText">string: the stored procedure name or T-SQL command</param>
    /// <param name="cmdParms">SqlParameter: an array of SqlParamters used to execute the command</param>
    /// <returns>Integer: an int representing the number of rows affected by the command</returns>
    /// <remarks>
    /// e.g.:  
    ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    // ----------------------------------------------------------------------------------
    private static int ExecuteNonQuery ( string connString, CommandType cmdType, string cmdText, params SqlParameter [ ] cmdParms )
    {
      //
      // Initialize a sql command
      //
      SqlCommand cmd = new SqlCommand ( );

      //
      // Open a sql connection
      //
      using ( SqlConnection conn = new SqlConnection ( connString ) )
      {
        //
        // Execute the command and add a numberic result to a return value. 
        //
        PrepareCommand ( cmd, conn, null, cmdType, cmdText, cmdParms );
        int val = cmd.ExecuteNonQuery ( );
        cmd.Parameters.Clear ( );
        return val;

      }//END using sql connection

    }//END ExecuteNonQuery class

    // ==================================================================================
    /// <summary>
    /// This class executes a SqlCommand (that returns no resultset) against an existing database connection 
    /// using the provided parameters.
    /// </summary>
    /// <param name="conn">SqlConnection: an existing database connection</param>
    /// <param name="cmdType">CommandType: the CommandType (stored procedure, text, etc.)</param>
    /// <param name="cmdText">string: the stored procedure name or T-SQL command</param>
    /// <param name="cmdParms">SqlParameter: an array of SqlParamters used to execute the command</param>
    /// <returns>Integer: an int representing the number of rows affected by the command</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize a sql command
    /// 
    /// 2. Execute the sql command and return the numeric value. 
    /// 
    /// e.g.:  
    ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    // ----------------------------------------------------------------------------------
    private static int ExecuteNonQuery ( SqlConnection conn, CommandType cmdType, string cmdText, params SqlParameter [ ] cmdParms )
    {
      //
      // Initialize a sql command
      //
      SqlCommand cmd = new SqlCommand ( );

      // 
      // Execute the sql command and return the numeric value. 
      //
      PrepareCommand ( cmd, conn, null, cmdType, cmdText, cmdParms );
      int val = cmd.ExecuteNonQuery ( );
      cmd.Parameters.Clear ( );
      return val;

    }//END ExecuteNonQuery method.

    // ==================================================================================
    /// <summary>
    /// This class executes a SqlCommand (that returns no resultset) using an existing SQL Transaction 
    /// using the provided parameters.
    /// </summary>
    /// <param name="trans">SqlTransaction: an existing sql transaction</param>
    /// <param name="cmdType">CommandType: the CommandType (stored procedure, text, etc.)</param>
    /// <param name="cmdText">string: the stored procedure name or T-SQL command</param>
    /// <param name="cmdParms">SqlParameter: an array of SqlParamters used to execute the command</param>
    /// <returns>Integer: an int representing the number of rows affected by the command</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize a sql command
    /// 
    /// 2. Execute the sql command and return the numeric value. 
    /// 
    /// e.g.:  
    ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    // ----------------------------------------------------------------------------------
    private static int ExecuteNonQuery ( SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter [ ] cmdParms )
    {
      //
      // Initialize a sql command. 
      //
      SqlCommand cmd = new SqlCommand ( );

      //
      // Execute the sql command and return the numeric value. 
      //
      PrepareCommand ( cmd, trans.Connection, trans, cmdType, cmdText, cmdParms );
      int val = cmd.ExecuteNonQuery ( );
      cmd.Parameters.Clear ( );
      return val;

    }//END ExecuteNonQuery class

    // ==================================================================================
    /// <summary>
    /// This class executes a SqlCommand that returns a resultset against the database specified in the connection string 
    /// using the provided parameters.
    /// </summary>
    /// <param name="connString">string: a valid connection string for a SqlConnection</param>
    /// <param name="cmdType">CommandType: the CommandType (stored procedure, text, etc.)</param>
    /// <param name="cmdText">string: the stored procedure name or T-SQL command</param>
    /// <param name="cmdParms">SqlParameter: an array of SqlParamters used to execute the command</param>
    /// <returns>SqlDataReader: A SqlDataReader containing the results</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize a sqlcommand and sqlconnection
    /// 
    /// 2. Try - Execute the sql command and return a sql data reader object. 
    /// 
    /// 3. Catch - Close the connection
    /// 
    /// e.g.:  
    ///  SqlDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    // ----------------------------------------------------------------------------------
    private static SqlDataReader ExecuteReader ( string connString, CommandType cmdType, string cmdText, params SqlParameter [ ] cmdParms )
    {
      //
      // Initialize a sqlcommand and sqlconnection
      //
      SqlCommand cmd = new SqlCommand ( );
      SqlConnection conn = new SqlConnection ( connString );

      // we use a try/catch here because if the method throws an exception we want to 
      // close the connection throw code, because no datareader will exist, hence the 
      // commandBehaviour.CloseConnection will not work
      try
      {
        //
        // Execute the sql command and return a sql data reader object. 
        //
        PrepareCommand ( cmd, conn, null, cmdType, cmdText, cmdParms );
        SqlDataReader rdr = cmd.ExecuteReader ( CommandBehavior.CloseConnection );
        cmd.Parameters.Clear ( );
        return rdr;
      }
      catch
      {
        //
        // Close the connection
        //
        conn.Close ( );
        throw;

      }//END try-catch execute the sql command

    }//END ExecuteReader class


    // ==================================================================================
    /// <summary>
    /// This class executes a SqlCommand that returns the first column of the first record against the database specified in the connection string 
    /// using the provided parameters.
    /// </summary>
    /// <param name="connString">string: a valid connection string for a SqlConnection</param>
    /// <param name="cmdType">CommandType: the CommandType (stored procedure, text, etc.)</param>
    /// <param name="cmdText">string: the stored procedure name or T-SQL command</param>
    /// <param name="cmdParms">SqlParameter: an array of SqlParamters used to execute the command</param>
    /// <returns>static object: An object that should be converted to the expected type using Convert.To{Type}</returns>
    /// <remarks>
    /// This class consists of the following steps: 
    /// 
    /// 1. Initialize a sql command and open a sql connection
    /// 
    /// 2. Execute the sql command and return a result object. 
    ///     
    /// e.g.:  
    ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private static object ExecuteScalar ( string connString, CommandType cmdType, string cmdText, params SqlParameter [ ] cmdParms )
    {
      //
      // Initialize a sql command
      //
      SqlCommand cmd = new SqlCommand ( );

      //
      // Open a sql connection 
      //
      using ( SqlConnection conn = new SqlConnection ( connString ) )
      {
        //
        // Execute the sql command and return a result object. 
        //
        PrepareCommand ( cmd, conn, null, cmdType, cmdText, cmdParms );
        object val = cmd.ExecuteScalar ( );
        cmd.Parameters.Clear ( );
        return val;

      }//END using sql connection

    }//END ExecuteScalar class. 

    // =====================================================================================
    /// <summary>
    /// This class executes a SqlCommand that returns the first column of the first record against an existing database connection 
    /// using the provided parameters.
    /// </summary>
    /// <param name="conn">SqlConnection: an existing database connection</param>
    /// <param name="cmdType">CommandType: the CommandType (stored procedure, text, etc.)</param>
    /// <param name="cmdText">string: the stored procedure name or T-SQL command</param>
    /// <param name="cmdParms">SqlParameter: an array of SqlParamters used to execute the command</param>
    /// <returns>static object: An object that should be converted to the expected type using Convert.To{Type}</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Initialize a sql command 
    /// 
    /// 2. Execute a sql command and return a result object. 
    /// 
    /// e.g.:  
    ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    /// </remarks> 
    // -------------------------------------------------------------------------------------
    private static object ExecuteScalar ( SqlConnection conn, CommandType cmdType, string cmdText, params SqlParameter [ ] cmdParms )
    {
      //
      // Initialize a sql command 
      //
      SqlCommand cmd = new SqlCommand ( );

      //
      // Execute a sql command and return a result object. 
      //
      PrepareCommand ( cmd, conn, null, cmdType, cmdText, cmdParms );
      object val = cmd.ExecuteScalar ( );
      cmd.Parameters.Clear ( );
      return val;

    }//END ExecuteScalar class

    // =====================================================================================
    /// <summary>
    /// This class adds parameter array to the cache
    /// </summary>
    /// <param name="cacheKey">string: Key to the parameter cache</param>
    /// <param name="cmdParms">SqlParameter: an array of SqlParamters to be cached</param>
    // -------------------------------------------------------------------------------------
    private static void CacheParameters ( string cacheKey, params SqlParameter [ ] cmdParms )
    {
      parmCache [ cacheKey ] = cmdParms;

    }//END CacheParameters class

    // =====================================================================================
    /// <summary>
    /// This class retrieves cached parameters
    /// </summary>
    /// <param name="cacheKey">string: key used to lookup parameters</param>
    /// <returns>SqlParameter: a cached SqlParamters array</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize a cached parameter object array
    /// 
    /// 2. Validate whether the cached parameter array is not null
    /// 
    /// 3. Initialize a cloned parameter array
    /// 
    /// 4. Loop through the cached parameter arrays and copy values to the clone parameter array
    /// 
    /// 5. Return a cloned parameter array. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private static SqlParameter [ ] GetCachedParameters ( string cacheKey )
    {
      //
      // Initialize a cached parameter object array
      //
      SqlParameter [ ] cachedParms = (SqlParameter [ ]) parmCache [ cacheKey ];

      //
      // Validate whether the cached parameter array is not null
      //
      if ( cachedParms == null )
      {
        return null;
      }

      //
      // Initialize a cloned parameter array
      //
      SqlParameter [ ] clonedParms = new SqlParameter [ cachedParms.Length ];

      //
      // Loop through the cached parameter arrays and copy values to the clone parameter array
      //
      for ( int i = 0, j = cachedParms.Length; i < j; i++ )
      {
        clonedParms [ i ] = (SqlParameter) ( (ICloneable) cachedParms [ i ] ).Clone ( );
      }

      //
      // Return a cloned parameter array. 
      //
      return clonedParms;

    }//END GetCachedParameters class

    // =====================================================================================
    /// <summary>
    /// This class prepares a command for execution
    /// </summary>
    /// <param name="cmd">SqlCommand: a sql command object</param>
    /// <param name="conn">SqlConnection: a sql connection object</param>
    /// <param name="trans">SqlTransaction: a sql transaction object</param>
    /// <param name="cmdType">CommandType: a command type e.g. stored procedure or text</param>
    /// <param name="cmdText">string: a command text, e.g. Select * from Products</param>
    /// <param name="cmdParms">SqlParameter: the sql parameters to use in the command</param>
    // -------------------------------------------------------------------------------------
    private static void PrepareCommand ( SqlCommand cmd, SqlConnection conn, SqlTransaction trans,
      CommandType cmdType, string cmdText, SqlParameter [ ] cmdParms )
    {

      //
      // Open the connection if the connection state is not yet open
      //
      if ( conn.State != ConnectionState.Open )
      {
        conn.Open ( );
      }

      //
      // Execute the commands: connection, commond text, transaction and add parameters. 
      //
      cmd.Connection = conn;
      cmd.CommandText = cmdText;

      if ( trans != null )
      {
        cmd.Transaction = trans;
      }
      cmd.CommandType = cmdType;
      cmd.CommandTimeout = _CommandTimeOut;

      if ( cmdParms != null )
      {
        foreach ( SqlParameter parm in cmdParms )
          cmd.Parameters.Add ( parm );
      }//END if command parameter

    }//END PrepareCommand class

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

  }//END EvSqlMethods class

}//END namespace Evado.Digital.Dal