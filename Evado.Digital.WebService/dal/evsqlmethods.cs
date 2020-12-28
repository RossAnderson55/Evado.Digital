/***************************************************************************************
 * <copyright file="dal\EvSqlMethods.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2020 EVADO HOLDING PTY. LTD.  All rights reserved.
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

using Evado.Digital.WebService.Model;

namespace Evado.Digital.WebService.Dal
{
  /// <summary>
  /// The SqlHelper class is intended to encapsulate high performance, 
  /// scalable best practices for common uses of SqlClient.
  /// </summary>
  public abstract class EvSqlMethods
  {

    #region class static variables.

    //Database connection strings
    private static string _connectionString = ConfigurationManager.ConnectionStrings [ "UniFORM" ].ConnectionString;

    private static string _eventLogSource = ConfigurationManager.AppSettings [ "EventLogSource" ];

    // Hashtable to store cached parameters
    private static Hashtable parmCache = Hashtable.Synchronized( new Hashtable( ) );

    public static string Status = String.Empty;

    #endregion

    #region Query Methods

    // =====================================================================================
    /// <summary>
    /// ExecuteReader method
    /// 
    /// Execute a SqlCommand that returns a resultset against the database specified in the connection string 
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  SqlDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="CommandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="CommandText">the stored procedure name or T-SQL groupCommand</param>
    /// <param name="CommandParameters">an array of SqlParamters used to execute the groupCommand</param>
    /// <returns>A SqlDataReader containing the results</returns>
    // -------------------------------------------------------------------------------------
    public static SqlDataReader ExecuteReader(
      CommandType CommandType,
      string CommandText,
      params SqlParameter [ ] CommandParameters )
    {
      
      //
      // Initialise method variables and objects.
      //
      SqlCommand cmd = new SqlCommand( );
      SqlConnection conn = new SqlConnection( _connectionString );

      // we use a try/catch here because if the method throws an exception we want to 
      // close the connection throw code, because no datareader will exist, hence the 
      // commandBehaviour.CloseConnection will not work
      try
      {
        PrepareCommand( cmd, conn, null, CommandType, CommandText, CommandParameters );
        SqlDataReader rdr = cmd.ExecuteReader( CommandBehavior.CloseConnection );
        cmd.Parameters.Clear( );
        return rdr;
      }
      catch ( Exception Ex )
      {
        conn.Close( );

        // 
        // Extract the parameters
        //
        string parameters = String.Empty;
        if ( CommandParameters != null )
        {
          parameters = "\r\n Parameters:";
          foreach ( SqlParameter prm in CommandParameters )
          {
            parameters += "\r\n Typ: " + prm.DbType
              + "\r\n Typ: " + prm.ParameterName
              + "\r\n Typ: " + prm.Value;
          }
        }

        //
        // Create the event message
        //
        string eventMessage = "Connection String" + _connectionString
          + "\r\n Command text: \r\n" + CommandText
          + parameters
          + "\r\n Exception: \r\n" + Evado.Model.EvStatics.getException( Ex );

        Evado.Model.EvStatics.WriteToEventLog( _eventLogSource, eventMessage, EventLogEntryType.Error );

        throw ( Ex );
      }

    }//END getRowData static method

    // =================================================================================== 
    /// <summary>
    /// public getRowData static method
    /// 
    /// Description: 
    ///  Executes a query against the database.
    /// 
    /// </summary>
    /// <param name="SqlQuery">SQL query string</param>
    /// <returns>DataSet of query results.</returns>
    //  ----------------------------------------------------------------------------------
    public static DataTable RunQuery( string SqlQuery )
    {
      Status = "BLL:SqlMethods:RunQuery method";

      //
      // Initialise method variables and objects.
      //
      SqlDataAdapter adapter = new SqlDataAdapter( );
      DataSet queryDataSet = new DataSet( );
      DataTable queryTable = new DataTable( );

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
        using ( SqlConnection connection = new SqlConnection( _connectionString ) )
        {
          //
          // Open the connection.
          // 
          connection.Open( );

          // 
          // Create a SqlCommand to retrieve query data.
          // 
          SqlCommand command = new SqlCommand( SqlQuery, connection );
          command.CommandType = CommandType.Text;

          // 
          // Set the SqlDataAdapter's SelectCommand.
          // 
          adapter.SelectCommand = command;

          // 
          // Fill the DataSet.
          // 
          adapter.Fill( queryDataSet );

          // 
          // Extract the event log table
          // 
          queryTable = queryDataSet.Tables [ 0 ];

        }
      }
      catch ( Exception Ex )
      {
        string eventMessage = "Status: + " + EvSqlMethods.Status
          + "\r\n Connection String" + _connectionString
          + "\r\n Query: \r\n" + SqlQuery
          + "\r\n Exception: \r\n" + Evado.Model.EvStatics.getException( Ex );

        Evado.Model.EvStatics.WriteToEventLog( _eventLogSource, eventMessage, EventLogEntryType.Error );

        throw ( Ex );
      }
      // 
      // Return the results from the query.
      // 
      return queryTable;

    }// END getRowData method

    // =================================================================================== 
    /// <summary>
    /// public getRowData static method
    /// 
    /// Description: 
    ///  Executes a query against the database.
    /// 
    /// </summary>
    /// <param name="SqlQuery">The query string</param>
    /// <param name="CommandParameters">an array of SqlParamters used to execute the groupCommand</param>
    /// <returns>DataSet of query results.</returns>
    //  ----------------------------------------------------------------------------------
    public static DataTable RunQuery( string SqlQuery, params SqlParameter [ ] CommandParameters )
    {
      Status = "BLL:SqlMethods:RunQuery method";

      //
      // Initialise method variables and objects.
      //
      SqlCommand command = new SqlCommand( );
      SqlDataAdapter adapter = new SqlDataAdapter( );
      DataSet queryDataSet = new DataSet( );
      DataTable queryTable = new DataTable( );

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
        using ( SqlConnection connection = new SqlConnection( _connectionString ) )
        {
          // 
          // Prepare the groupCommand
          // 
          PrepareCommand( command, connection, null,
            CommandType.Text, SqlQuery, CommandParameters );

          // 
          // Set the SqlDataAdapter's SelectCommand.
          // 
          adapter.SelectCommand = command;

          // 
          // Fill the DataSet.
          // 
          adapter.Fill( queryDataSet );

          // 
          // Extract the event log table
          // 
          queryTable = queryDataSet.Tables [ 0 ];

          // 
          // Clear Parameters
          // 
          command.Parameters.Clear( );
        }
      }
      catch ( Exception Ex )
      {
        // 
        // Extract the parameters
        //
        string parameters = String.Empty;
        if ( CommandParameters != null )
        {
          parameters = "\r\n Parameters:";
          foreach ( SqlParameter prm in CommandParameters )
          {
            parameters += "\r\n Typ: " + prm.DbType
              + "\r\n Typ: " + prm.ParameterName
              + "\r\n Typ: " + prm.Value;
          }
        }

        //
        // Create the event message
        //

        string eventMessage = "Status: + " + EvSqlMethods.Status
          + "\r\n Connection String" + _connectionString
          + "\r\n Query: \r\n" + SqlQuery
          + parameters
          + "\r\n Exception: \r\n" + Evado.Model.EvStatics.getException( Ex );

        Evado.Model.EvStatics.WriteToEventLog( _eventLogSource, eventMessage, EventLogEntryType.Error );

        throw ( Ex );
      }

      // 
      // Return the results from the query.
      // 
      return queryTable;

    }//END getRowData method

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Data Retrieval Methods

    #region Row methods

    // =====================================================================================
    /// <summary>
    /// public getString static method
    /// 
    /// Description:
    /// Reads the row object and returns the newField name as a string.
    /// 
    /// </summary>
    /// <param name="Row">OdbcDataReader</param>
    /// <param name="FieldName">FieldName</param>
    // -------------------------------------------------------------------------------------
    public static bool hasValue( DataRow Row, string FieldName )
    {
      if ( Row [ FieldName ] == null )
      {
        return false;
      }
      return true;

    }//END getString static method

    // =====================================================================================
    /// <summary>
    /// public getGuid static method
    /// 
    /// Description:
    /// Reads the row object and returns the newField name as a FormUid.
    /// 
    /// </summary>
    /// <param name="Row">OdbcDataReader</param>
    /// <param name="FieldName">Document</param>
    // -------------------------------------------------------------------------------------
    public static Guid getGuid( DataRow Row, string FieldName )
    {
      /// 
      /// Extract the newField if it is not null.
      /// 
      if ( Row [ FieldName ] != null )
      {
        /// 
        /// Extract is to a string.
        /// 
        string sGuid = Row [ FieldName ].ToString( );

        /// 
        /// If string is 36 characters long parse it as a FormUid.
        /// 
        if ( sGuid.Length == 36 )
        {
          try
          {
            return new Guid( sGuid );
          }
          catch
          {
            return Guid.Empty;
          }

        }
        return Guid.Empty;
      }

      return Guid.Empty;

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
    /// <param name="FieldName">Document</param>
    // -------------------------------------------------------------------------------------
    public static int getInteger( DataRow Row, string FieldName )
    {
      int iValue = 0;

      if ( Row [ FieldName ] != null )
      {
        if ( int.TryParse( Row [ FieldName ].ToString( ), out iValue ) == false )
        {
          return 0;
        }
      }

      return iValue;

    }//END getInteger static method

    // =====================================================================================
    /// <summary>
    /// public getInteger static method
    /// 
    /// Description:
    /// Reads the row object and returns the newField name as a Integer.
    /// 
    /// </summary>
    /// <param name="Row">OdbcDataReader</param>
    /// <param name="FieldName">FieldName</param>
    // -------------------------------------------------------------------------------------
    public static long getLong( DataRow Row, string FieldName )
    {
      long iValue = 0;

      if ( Row [ FieldName ] != null )
      {
        if ( long.TryParse( Row [ FieldName ].ToString( ), out iValue ) == false )
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
    /// <param name="FieldName">FieldName</param>
    // -------------------------------------------------------------------------------------
    public static DateTime getDateTime( DataRow Row, string FieldName )
    {
      DateTime dValue = DateTime.Parse( "1 Jan 1900" );

      if ( Row [ FieldName ] != null )
      {
        if ( DateTime.TryParse( Row [ FieldName ].ToString( ), out dValue ) == false )
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
    /// <param name="FieldName">FieldName</param>
    // -------------------------------------------------------------------------------------
    public static string getString( DataRow Row, string FieldName )
    {
      if ( Row [ FieldName ] != null )
      {
        return Row [ FieldName ].ToString( ).Trim( );
      }
      return String.Empty;

    }//END getString static method

    // =====================================================================================
    /// <summary>
    /// public getString static method
    /// 
    /// Description:
    /// Reads the row object and returns the newField name as a string.
    /// 
    /// </summary>
    /// <param name="Row">OdbcDataReader</param>
    /// <param name="FieldName">FieldName</param>
    // -------------------------------------------------------------------------------------
    public static byte [ ] getBytes( DataRow Row, string FieldName )
    {
      if ( Row [ FieldName ] != null )
      {
        return (byte [ ]) Row [ FieldName ];
      }
      return null;

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
    /// <param name="FieldName">FieldName</param>
    // -------------------------------------------------------------------------------------
    public static float getFloat( DataRow Row, string FieldName )
    {
      float fValue = 0;

      if ( Row [ FieldName ] != null )
      {
        if ( float.TryParse( Row [ FieldName ].ToString( ), out fValue ) == false )
        {
          return 0;
        }
      }

      return fValue;

    }//END getFloat static method

    // =====================================================================================
    /// <summary>
    /// public getBool static method
    /// 
    /// Description:
    /// Reads the row object and returns the newField name as a bool.
    /// 
    /// </summary>
    /// <param name="Row">OdbcDataReader</param>
    /// <param name="FieldName">FieldName</param>
    // -------------------------------------------------------------------------------------
    public static bool getBool( DataRow Row, string FieldName )
    {
      /// 
      /// if it exits.
      /// 
      if ( Row [ FieldName ] != null )
      {
        string sValue = Row [ FieldName ].ToString( ).ToLower( );

        /// 
        /// If string value can be interpreted as a boolean true set true.
        /// 
        if ( sValue.Contains( "1" ) == true
          || sValue.Contains( "true" ) == true
          || sValue.Contains( "yes" ) == true
          || sValue.Contains( "y" ) == true )
        {
          return true;
        }
      }

      return false;

    }//END getBoolean static method

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

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Update Methods

    // =====================================================================================
    /// <summary>
    /// public StoreProcUpdate method
    /// 
    /// Execute a SqlCommand (that returns no resultset) against the database specified in the connection string 
    /// using a stored procedure and the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  int result = StoreProcUpdate("PublishOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="StoreProcedureName">the name of the stored procedure to be used.)</param>
    /// <param name="CommandParameters">an array of SqlParamters used to execute the groupCommand</param>
    /// <returns>an int representing the number of rows affected by the groupCommand</returns>
    // -------------------------------------------------------------------------------------
    public static int StoreProcUpdate( string StoreProcedureName, params SqlParameter [ ] CommandParameters )
    {
      int iResult = 0;

      using ( SqlConnection conn = new SqlConnection( _connectionString ) )
      {
        conn.Open( );
        using ( SqlTransaction trans = conn.BeginTransaction( ) )
        {
          try
          {
            iResult = EvSqlMethods.ExecuteNonQuery(
              trans,
              CommandType.StoredProcedure,
              StoreProcedureName,
              CommandParameters );
            trans.Commit( );
          }
          catch ( Exception Ex )
          {
            trans.Rollback( );
            EventLog.WriteEntry( _eventLogSource, Ex.Message.ToString( ), EventLogEntryType.Error );
            throw ( Ex );
          }
        }
        return iResult;
      }
    }//END StoreProcUpdate method

    // =====================================================================================
    /// <summary>
    /// public QueryUpdate method
    /// 
    /// Execute a SqlCommand (that returns no resultset) against the database specified in the connection string 
    /// using a stored procedure and the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  int result = StoreProcUpdate("PublishOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="UpdateCommand">the name of the stored procedure to be used.)</param>
    /// <param name="CommandParameters">an array of SqlParamters used to execute the groupCommand</param>
    /// <returns>an int representing the number of rows affected by the groupCommand</returns>
    // -------------------------------------------------------------------------------------
    public static int QueryUpdate( string UpdateCommand, params SqlParameter [ ] CommandParameters )
    {
      //
      // Initialise method variables and objects.
      //
      int iResult = 0;

      using ( SqlConnection conn = new SqlConnection( _connectionString ) )
      {
        conn.Open( );
        using ( SqlTransaction trans = conn.BeginTransaction( ) )
        {
          try
          {
            iResult = EvSqlMethods.ExecuteNonQuery(
              trans, CommandType.Text, UpdateCommand, CommandParameters );
            trans.Commit( );
          }
          catch ( Exception Ex )
          {
            trans.Rollback( );

            string eventMessage = "Status: + " + EvSqlMethods.Status
              + "\r\n Connection String" + _connectionString
              + "\r\n Query: \r\n" + UpdateCommand
              + "\r\n Exception: \r\n" + Evado.Model.EvStatics.getException( Ex );

            Evado.Model.EvStatics.WriteToEventLog( _eventLogSource, eventMessage, EventLogEntryType.Error );
            throw ( Ex );
          }
        }
        return iResult;
      }
    }//END QueryUpdate method


    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Private methods

    // =====================================================================================
    /// <summary>
    /// Execute a SqlCommand (that returns no resultset) against the database specified in the connection string 
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connString">a valid connection string for a SqlConnection</param>
    /// <param name="cmdType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="cmdText">the stored procedure name or T-SQL groupCommand</param>
    /// <param name="cmdParms">an array of SqlParamters used to execute the groupCommand</param>
    /// <returns>an int representing the number of rows affected by the groupCommand</returns>
    // -------------------------------------------------------------------------------------
    private static int ExecuteNonQuery( string connString, CommandType cmdType, string cmdText, params SqlParameter [ ] cmdParms )
    {

      SqlCommand cmd = new SqlCommand( );

      using ( SqlConnection conn = new SqlConnection( connString ) )
      {
        PrepareCommand( cmd, conn, null, cmdType, cmdText, cmdParms );
        int val = cmd.ExecuteNonQuery( );
        cmd.Parameters.Clear( );
        return val;
      }
    }

    // =====================================================================================
    /// <summary>
    /// Execute a SqlCommand (that returns no resultset) against an existing database connection 
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="conn">an existing database connection</param>
    /// <param name="cmdType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="cmdText">the stored procedure name or T-SQL groupCommand</param>
    /// <param name="cmdParms">an array of SqlParamters used to execute the groupCommand</param>
    /// <returns>an int representing the number of rows affected by the groupCommand</returns>
    // -------------------------------------------------------------------------------------
    private static int ExecuteNonQuery( SqlConnection conn, CommandType cmdType, string cmdText, params SqlParameter [ ] cmdParms )
    {

      SqlCommand cmd = new SqlCommand( );

      PrepareCommand( cmd, conn, null, cmdType, cmdText, cmdParms );
      int val = cmd.ExecuteNonQuery( );
      cmd.Parameters.Clear( );
      return val;
    }

    // =====================================================================================
    /// <summary>
    /// Execute a SqlCommand (that returns no resultset) using an existing SQL Transaction 
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="trans">an existing sql transaction</param>
    /// <param name="cmdType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="cmdText">the stored procedure name or T-SQL groupCommand</param>
    /// <param name="cmdParms">an array of SqlParamters used to execute the groupCommand</param>
    /// <returns>an int representing the number of rows affected by the groupCommand</returns>
    // -------------------------------------------------------------------------------------
    private static int ExecuteNonQuery( SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter [ ] cmdParms )
    {
      SqlCommand cmd = new SqlCommand( );
      PrepareCommand( cmd, trans.Connection, trans, cmdType, cmdText, cmdParms );
      int val = cmd.ExecuteNonQuery( );
      cmd.Parameters.Clear( );
      return val;
    }

    // =====================================================================================
    /// <summary>
    /// Execute a SqlCommand that returns a resultset against the database specified in the connection string 
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  SqlDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connString">a valid connection string for a SqlConnection</param>
    /// <param name="cmdType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="cmdText">the stored procedure name or T-SQL groupCommand</param>
    /// <param name="cmdParms">an array of SqlParamters used to execute the groupCommand</param>
    /// <returns>A SqlDataReader containing the results</returns>
    // -------------------------------------------------------------------------------------
    private static SqlDataReader ExecuteReader( string connString, CommandType cmdType, string cmdText, params SqlParameter [ ] cmdParms )
    {
      SqlCommand cmd = new SqlCommand( );
      SqlConnection conn = new SqlConnection( connString );

      // we use a try/catch here because if the method throws an exception we want to 
      // close the connection throw code, because no datareader will exist, hence the 
      // commandBehaviour.CloseConnection will not work
      try
      {
        PrepareCommand( cmd, conn, null, cmdType, cmdText, cmdParms );
        SqlDataReader rdr = cmd.ExecuteReader( CommandBehavior.CloseConnection );
        cmd.Parameters.Clear( );
        return rdr;
      }
      catch
      {
        conn.Close( );
        throw;
      }
    }


    // =====================================================================================
    /// <summary>
    /// Execute a SqlCommand that returns the first column of the first record against the database specified in the connection string 
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connString">a valid connection string for a SqlConnection</param>
    /// <param name="cmdType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="cmdText">the stored procedure name or T-SQL groupCommand</param>
    /// <param name="cmdParms">an array of SqlParamters used to execute the groupCommand</param>
    /// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
    // -------------------------------------------------------------------------------------
    private static object ExecuteScalar( string connString, CommandType cmdType, string cmdText, params SqlParameter [ ] cmdParms )
    {
      SqlCommand cmd = new SqlCommand( );

      using ( SqlConnection conn = new SqlConnection( connString ) )
      {
        PrepareCommand( cmd, conn, null, cmdType, cmdText, cmdParms );
        object val = cmd.ExecuteScalar( );
        cmd.Parameters.Clear( );
        return val;
      }
    }

    // =====================================================================================
    /// <summary>
    /// Execute a SqlCommand that returns the first column of the first record against an existing database connection 
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="conn">an existing database connection</param>
    /// <param name="cmdType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="cmdText">the stored procedure name or T-SQL groupCommand</param>
    /// <param name="cmdParms">an array of SqlParamters used to execute the groupCommand</param>
    /// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
    // -------------------------------------------------------------------------------------
    private static object ExecuteScalar( SqlConnection conn, CommandType cmdType, string cmdText, params SqlParameter [ ] cmdParms )
    {

      SqlCommand cmd = new SqlCommand( );

      PrepareCommand( cmd, conn, null, cmdType, cmdText, cmdParms );
      object val = cmd.ExecuteScalar( );
      cmd.Parameters.Clear( );
      return val;
    }

    // =====================================================================================
    /// <summary>
    /// add parameter array to the cache
    /// </summary>
    /// <param name="cacheKey">Key to the parameter cache</param>
    /// <param name="cmdParms">an array of SqlParamters to be cached</param>
    // -------------------------------------------------------------------------------------
    private static void CacheParameters( string cacheKey, params SqlParameter [ ] cmdParms )
    {
      parmCache [ cacheKey ] = cmdParms;
    }

    // =====================================================================================
    /// <summary>
    /// Retrieve cached parameters
    /// </summary>
    /// <param name="cacheKey">key used to lookup parameters</param>
    /// <returns>Cached SqlParamters array</returns>
    // -------------------------------------------------------------------------------------
    private static SqlParameter [ ] GetCachedParameters( string cacheKey )
    {
      SqlParameter [ ] cachedParms = (SqlParameter [ ]) parmCache [ cacheKey ];

      if ( cachedParms == null )
      {
        return null;
      }
      SqlParameter [ ] clonedParms = new SqlParameter [ cachedParms.Length ];

      for ( int i = 0, j = cachedParms.Length; i < j; i++ )
      {
        clonedParms [ i ] = (SqlParameter) ( (ICloneable) cachedParms [ i ] ).Clone( );
      }

      return clonedParms;
    }

    // =====================================================================================
    /// <summary>
    /// Prepare a groupCommand for execution
    /// </summary>
    /// <param name="cmd">SqlCommand object</param>
    /// <param name="conn">SqlConnection object</param>
    /// <param name="trans">SqlTransaction object</param>
    /// <param name="cmdType">Cmd type e.g. stored procedure or text</param>
    /// <param name="cmdText">Command text, e.g. Select * from Products</param>
    /// <param name="cmdParms">SqlParameters to use in the groupCommand</param>
    // -------------------------------------------------------------------------------------
    private static void PrepareCommand( SqlCommand cmd, SqlConnection conn, SqlTransaction trans,
      CommandType cmdType, string cmdText, SqlParameter [ ] cmdParms )
    {

      if ( conn.State != ConnectionState.Open )
      {
        conn.Open( );
      }

      cmd.Connection = conn;
      cmd.CommandText = cmdText;

      if ( trans != null )
      {
        cmd.Transaction = trans;
      }
      cmd.CommandType = cmdType;

      if ( cmdParms != null )
      {
        foreach ( SqlParameter parm in cmdParms )
          cmd.Parameters.Add( parm );
      }
    }
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
  }
}