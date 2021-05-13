/* <copyright file="EvRecords.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2021 EVADO HOLDING PTY. LTD..  All rights reserved.
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

//Application specific class references.
using Evado.Model;
using Evado.Digital.Model;


namespace Evado.Digital.Dal
{
  /// <summary>
  /// Data Access Layer class for ApplicationEvent
  /// </summary>
  public class EvApplicationEvents
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EvApplicationEvents ( )
    {
      this.ClassNameSpace = "Evado.Digital.Dal.Digital.EvApplicationEvents.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EvApplicationEvents ( EvClassParameters Settings )
    {
      this.Settings = Settings;
      this.ClassNameSpace = "Evado.Digital.Dal.Digital.EvApplicationEvents.";

    }

    #endregion

    #region Initialise constants and variables

    // 
    // selectionList query string.
    // 
    private const string _sqlQuery_View = "Select TOP 1000 * " +
      "FROM EV_APPLICATION_EVENTS ";
    private const string DB_GUID = "AP_GUID";
    private const string DB_DATE_TIME = "AP_DATE_TIME";
    private const string DB_EVENT_ID = "AP_EVENT_ID";
    private const string DB_EVENT_TYPE = "AP_EVENT_TYPE";
    private const string DB_EVENT_CATEGORY = "AP_EVENT_CATEGORY";
    private const string DB_USER_NAME = "AP_USER_NAME";
    private const string DB_DESCRIPTION = "AP_DESCRIPTION";
    private const string DB_PAGE_URL = "AP_PAGE_URL";
    private const string DB_SITE = "AP_SITE";

    // 
    // EvEvent Insert query string
    // 
    private const string _STORED_PROCEDURE_AddItem = "EV_APPLICATION_EVENT_ADD";

    // 
    // Define the query parameter constants.
    // 
    private const string PARM_GUID = "@GUID";
    private const string PARM_EVENT_ID = "@EVENT_ID";
    private const string PARM_DATE_TIME = "@DATE_TIME";
    private const string PARM_EVENT_TYPE = "@EVENT_TYPE";
    private const string PARM_EVENT_CATEGORY = "@EVENT_CATEGORY";
    private const string PARM_USER_NAME = "@USER_NAME";
    private const string PARM_DESCRIPTION = "@DESCRIPTION";
    private const string PARM_PAGE_URL = "@PAGE_URL";
    private const string PARM_SITE = "@SITE";
    private const string PARM_DATE_STATE = "@DATE_STATE";
    private const string PARM_DATE_FINISH = "@DATE_FINISH";

    //
    //  Define the SQL query string variable.
    //      
    private string _sqlQueryString = String.Empty;

    //
    // Define the class EventId property and variable.
    //
    #endregion

    #region Data Reader section

    // ==================================================================================
    /// <summary>
    /// This class reads the content of the data reader object into ApplicationEvent business object.
    /// </summary>
    /// <param name="Row">DataRow: a data row object</param>
    /// <remarks>
    /// This method consists of the following step:
    /// 
    /// 1. Convert the elements from data reader object into the elements 
    /// in application event business object.
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public EvApplicationEvent getReaderData ( DataRow Row )
    {
      EvApplicationEvent Event = new EvApplicationEvent ( );
      //
      // Convert the elements from data reader object into the elements in application event business object.
      //
      Event.Guid = EvSqlMethods.getGuid ( Row, DB_GUID );
      Event.EventId = EvSqlMethods.getInteger ( Row, DB_EVENT_ID );
      Event.DateTime = EvSqlMethods.getDateTime ( Row, DB_DATE_TIME );
      string sType = EvSqlMethods.getString ( Row, DB_EVENT_TYPE );
      if ( sType == "A" ) { sType = "Action"; }
      if ( sType == "I" ) { sType = "Information"; }
      if ( sType == "W" ) { sType = "Warning"; }
      if ( sType == "E" ) { sType = "Error"; }

      Event.Type = Evado.Model.EvStatics.parseEnumValue<EvApplicationEvent.EventType> ( sType );

      Event.Category = EvSqlMethods.getString ( Row, DB_EVENT_CATEGORY );
      Event.UserName = EvSqlMethods.getString ( Row, DB_USER_NAME );
      Event.Description = EvSqlMethods.getString ( Row, DB_DESCRIPTION );
      Event.PageUrl = EvSqlMethods.getString ( Row, DB_PAGE_URL );
      Event.CustomerId = EvSqlMethods.getString ( Row, DB_SITE );

      return Event;
    }//END getRowData method.

    #endregion

    #region SQL Parameters section

    // =====================================================================================
    /// <summary>
    /// This class defines the SQL parameter for a query. 
    /// </summary>
    /// <returns>SqlParameter: an array of parameters</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize an sql parameter array and add the elements into the array.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private static SqlParameter [ ] GetParameters ( )
    {
      //
      // Initialize an sql parameter array and add its elements.
      //
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(PARM_EVENT_ID, SqlDbType.NVarChar, 10),
        new SqlParameter(PARM_EVENT_TYPE, SqlDbType.NVarChar, 50),
        new SqlParameter(PARM_EVENT_CATEGORY, SqlDbType.NVarChar, 100),
        new SqlParameter(PARM_USER_NAME, SqlDbType.NVarChar, 100),
        new SqlParameter(PARM_DESCRIPTION, SqlDbType.NText),
        new SqlParameter(PARM_PAGE_URL, SqlDbType.NVarChar, 100),
        new SqlParameter(PARM_SITE, SqlDbType.NVarChar, 10),
      };

      return cmdParms;
    }//END GetParameters method

    // =====================================================================================
    /// <summary>
    /// This class sets the query parameter values. 
    /// </summary>
    /// <param name="cmdParms">SqlParameter: a database parameters array</param>
    /// <param name="ApplicationEvent">EvApplicationEvent: an applicationEvent data object</param>
    /// <remarks>
    /// This method consists of the following step:
    /// 
    /// 1. Update value of the database parameters with the value from application event object' items
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private void SetParameters ( 
      SqlParameter [ ] cmdParms, 
      EvApplicationEvent ApplicationEvent )
    {
      cmdParms [ 0 ].Value = (int) ApplicationEvent.EventId;
      cmdParms [ 1 ].Value = ApplicationEvent.Type.ToString ( );
      cmdParms [ 2 ].Value = ApplicationEvent.Category;
      cmdParms [ 3 ].Value = ApplicationEvent.UserName;
      cmdParms [ 4 ].Value = ApplicationEvent.Description;
      cmdParms [ 5 ].Value = ApplicationEvent.PageUrl;
      cmdParms [ 6 ].Value = ApplicationEvent.CustomerId;

    }//END SetParameters method.

    #endregion

    #region Class Query method section

    // =====================================================================================
    /// <summary>
    /// This class gets a ArrayList containing a selectionList of ApplicationEvent data objects.
    /// </summary>
    /// <param name="EventId">int: The event identifier</param>
    /// <param name="EventType">String: The event type</param>
    /// <param name="Category">String: The event category</param>
    /// <param name="DateStart">DateTime: The period start date</param>
    /// <param name="DateFinish">DateTime: The period finish date</param>
    /// <param name="UserName">String: The user that raised  the event</param>
    /// <returns>List of ApplicationEvent: a list of application event parameters</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize a status string and a return view of the application event. 
    /// 
    /// 2. Format the started date and finished date and add them to the status string. 
    /// 
    /// 3. Define the sql query parameters and update them with the retrieving values
    /// 
    /// 4. Define the sql query string and execute the query string
    /// 
    /// 5. Scroll through the query result 
    /// 
    /// 6. Convert the result from a data reader event object to an application event business object
    /// 
    /// 7. Appent the converted result to a return view. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvApplicationEvent> getApplicationEventList (
      int EventId,
      String EventType,
      String Category,
      DateTime DateStart,
      DateTime DateFinish,
      String UserName)
    {
      this.LogMethod( "getView method" );
      this.LogDebugValue ( "EventId: " + EventId );
      this.LogDebugValue ( "Type: " + EventType );
      this.LogDebugValue ( "Category: " + Category );
      this.LogDebugValue ( "DateStart: " + DateStart.ToString( "dd MMM yy hh:mm" ) );
      this.LogDebugValue ( "DateFinish: " + DateFinish.ToString ( "dd MMM yy hh:mm" ) );
      this.LogDebugValue ( "UserName :" + UserName );
      //
      // Initialize a status of the application event. 
      //

      // 
      // Initialize a return view of application event and an event identifier. 
      // 
      List<EvApplicationEvent> applicationEventList = new List<EvApplicationEvent> ( );

      //
      // Increment the dates to the less or greater than.
      // Format the start date, if it exists
      //
      if ( DateStart > Evado.Model.EvStatics.CONST_DATE_NULL )
      {
        String stStartDate = DateStart.ToString ( "dd MMM yyyy" ) + " 00:00 AM";
        DateStart = DateTime.Parse ( stStartDate );
      }

      // 
      // Set the finish date to default of one day      
      // 
      if ( DateFinish == Evado.Model.EvStatics.CONST_DATE_NULL
        && DateStart >  Evado.Digital.Model.EvcStatics.CONST_DATE_NULL )
      {
        DateFinish = DateStart.AddDays ( 1 );
      }

      //
      // Format the finished date
      //
      String stDateFinish = DateFinish.ToString ( "dd MMM yyyy" ) + " 23:59 PM";
      DateFinish = DateTime.Parse ( stDateFinish );

      //
      // Add started date and finished date to the status string. 
      //
      this.LogDebugValue( "DateStart: " + DateStart.ToString ( "dd MMM yyyy HH:mm" ) );
      this.LogDebugValue( "DateFinish: " + DateFinish.ToString ( "dd MMM yyyy HH:mm" ) );

      // 
      // Define the SQL query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(PARM_EVENT_ID, SqlDbType.SmallInt ),
        new SqlParameter(PARM_EVENT_TYPE, SqlDbType.Char, 1),
        new SqlParameter(PARM_EVENT_CATEGORY, SqlDbType.NVarChar, 20),
        new SqlParameter(PARM_DATE_STATE, SqlDbType.DateTime),
        new SqlParameter(PARM_DATE_FINISH, SqlDbType.DateTime),
        new SqlParameter(PARM_USER_NAME, SqlDbType.NVarChar, 100),
      };

      //
      // Update retrieving values to the sql query parameters.
      //
      cmdParms [ 0 ].Value = EventId;
      cmdParms [ 1 ].Value = EventType;
      cmdParms [ 2 ].Value = Category;
      cmdParms [ 3 ].Value = DateStart;
      cmdParms [ 4 ].Value = DateFinish;
      cmdParms [ 5 ].Value = UserName;

      // 
      // Define the query string.
      // 
      _sqlQueryString = _sqlQuery_View;

      if ( EventId < 0 )
      {
        _sqlQueryString += " WHERE (" + PARM_EVENT_ID + " = " + DB_EVENT_ID + " ) ";

        if ( EventType != String.Empty )
        {
          _sqlQueryString += " AND (" + PARM_EVENT_TYPE + " = " + DB_EVENT_TYPE + ") ";
        }

        if ( Category != String.Empty )
        {
          _sqlQueryString += " AND (" + PARM_EVENT_CATEGORY + " = " + DB_EVENT_CATEGORY + ") ";
        }

        if ( UserName != String.Empty )
        {
          _sqlQueryString += " AND (" + PARM_USER_NAME + "  = " + DB_USER_NAME + ") ";
        }

        if ( DateStart >  Evado.Digital.Model.EvcStatics.CONST_DATE_NULL )
        {
          _sqlQueryString += " AND (" + DB_DATE_TIME + " >= " + PARM_DATE_STATE + ") "
            + " AND (" + DB_DATE_TIME + " <= " + PARM_DATE_FINISH + ") ";
        }
      }
      else if ( EventType != String.Empty )
      {
        _sqlQueryString += " WHERE (" + PARM_EVENT_TYPE + " = " + DB_EVENT_TYPE + ") ";

        if ( UserName != String.Empty )
        {
          _sqlQueryString += " AND (" + PARM_USER_NAME + "  = " + DB_USER_NAME + ") ";
        }

        if ( DateStart >  Evado.Digital.Model.EvcStatics.CONST_DATE_NULL )
        {
          _sqlQueryString += " AND (" + DB_DATE_TIME + " >= " + PARM_DATE_STATE + ") "
            + " AND (" + DB_DATE_TIME + " <= " + PARM_DATE_FINISH + ") ";
        }
      }
      else if ( Category != String.Empty )
      {
        _sqlQueryString += " WHERE (" + PARM_EVENT_CATEGORY + " = " + DB_EVENT_CATEGORY + ") ";

        if ( UserName != String.Empty )
        {
          _sqlQueryString += " AND (" + PARM_USER_NAME + "  = " + DB_USER_NAME + ") ";
        }

        if ( DateStart >  Evado.Digital.Model.EvcStatics.CONST_DATE_NULL )
        {
          _sqlQueryString += " AND (" + DB_DATE_TIME + " >= " + PARM_DATE_STATE + ") "
            + " AND (" + DB_DATE_TIME + " <= " + PARM_DATE_FINISH + ") ";
        }
      }
      else
      {
        if ( DateStart >  Evado.Digital.Model.EvcStatics.CONST_DATE_NULL )
        {
          _sqlQueryString += " WHERE (" + DB_DATE_TIME + " >= " + PARM_DATE_STATE + ") "
            + " AND (" + DB_DATE_TIME + " <= " + PARM_DATE_FINISH + ") ";

          if ( UserName != String.Empty )
          {
            _sqlQueryString += " AND (" + PARM_USER_NAME + "  = " + DB_USER_NAME + ") ";
          }
        }
        else
        {
          _sqlQueryString += " WHERE (" + PARM_USER_NAME + "  = " + DB_USER_NAME + ") ";
        }
      }

      _sqlQueryString += " ORDER BY " + DB_DATE_TIME + ";";

      this.LogDebugValue ( _sqlQueryString );

      //this.LogDebugValue ( EvSqlMethods.getParameters( cmdParms ) );

      // 
      // Execute the query against the database.
      // 
      using ( DataTable table = EvSqlMethods.RunQuery ( _sqlQueryString, cmdParms ) )
      {
        // 
        // Iterate through the results table for extracting the role information.
        // 
        for ( int Count = 0; Count < table.Rows.Count; Count++ )
        {
          DataRow row = table.Rows [ Count ];
          
          EvApplicationEvent Event = this.getReaderData ( row );
          Event.Uid = Count.ToString ( "00000" ); ;
 
          applicationEventList.Add ( Event );
        
        }//END record iteration loop.

      }//END using statement
      // 
      // Pass back the result arrray.
      // 
      return applicationEventList;

    }//END getView method.
    #endregion

    #region Get Event section

    // =====================================================================================
    /// <summary>
    /// This class gets ApplicationEvent data object by its unique object identifier.    
    /// </summary>
    /// <param name="EventGuid">String: (Mandatory) Unique identifier.</param>
    /// <returns>ApplicationEvent: an item of application event</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize a status string and an application event object
    /// 
    /// 2. Validate whether the Unique identifier is not zero
    /// 
    /// 3. Define the query parameters and the query string
    /// 
    /// 4. Execute the query string
    /// 
    /// 5. Convert the value from data reader event object to application event business object, 
    /// if value exists
    /// 
    /// 6. Return the application event value
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvApplicationEvent getItem ( 
      Guid EventGuid )
    {
      this.LogMethod ( "getItem method. " );
      this.LogDebugValue ( "EventGuid: " + EventGuid );
      //
      // Initialize a status string and an application event object
      //
      EvApplicationEvent applicationEvent = new EvApplicationEvent ( );

      // 
      // Check that there is a valid unique identifier.
      // 
      if ( EventGuid == Guid.Empty )
      {
        return applicationEvent;
      }

      // 
      // Define the query parameters.
      // 
      SqlParameter cmdParms = new SqlParameter ( PARM_GUID, SqlDbType.UniqueIdentifier );
      cmdParms.Value = EventGuid;

      // 
      // Define the query string.
      // 
      _sqlQueryString = _sqlQuery_View 
        + " WHERE (" + DB_GUID + " = "+ PARM_GUID + " ); ";

      using ( DataTable table = EvSqlMethods.RunQuery ( _sqlQueryString, cmdParms ) )
      {
        // 
        // If not rows the return
        // 
        if ( table.Rows.Count == 0 )
        {
          return applicationEvent;
        }

        // 
        // Extract the table row and store the data row to an activity object
        // 
        DataRow row = table.Rows [ 0 ];

        applicationEvent = this.getReaderData ( row );
      }

      // 
      // Pass back the data object.
      // 
      return applicationEvent;

    }//END getItem class

    #endregion

    #region Add EvEvent section

    // =====================================================================================
    /// <summary>
    /// This method adds an ApplicationEvent data object to the database.
    /// </summary>
    /// <param name="ApplicationEvent">EvApplicationEvent: an application event data object</param>
    /// <returns>EvEventCodes: a code of an event</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize the status string
    /// 
    /// 2. Check that the user identifier is valid
    /// 
    /// 3. Remove the domain name from user identifier
    /// 
    /// 4. Define the SQL query parameters and load the query values.
    /// 
    /// 5. Execute the update command.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes addEvent ( 
      EvApplicationEvent ApplicationEvent )
    {
      this.LogMethod ( "addEvent method. " );
      this.LogDebugValue ( "EventId: " + ApplicationEvent.EventId );
      // 
      // Check that the user identifier is valid
      // 
      if (  ApplicationEvent.EventId  > 1 )
      {
        return EvEventCodes.Data_InvalidId_Error;
      }

      //
      // Remove the domain name from user identifier
      //
      if ( ApplicationEvent.UserName != null )
      {
        //ApplicationEvent.UserName = EvStatics.removeDomainName ( ApplicationEvent.UserName );
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] _cmdParms = GetParameters ( );
      SetParameters ( _cmdParms, ApplicationEvent );

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( _STORED_PROCEDURE_AddItem, _cmdParms ) == 0 )
      {
        this.LogMethodEnd ( "addEvent" );
        return EvEventCodes.Database_Record_Update_Error;
      }

      this.LogMethodEnd ( "addEvent" );
      return EvEventCodes.Ok;

    } // Close method addTestReport    
    #endregion

    #region Based Class properties.

    private String _ClassNameSpace = "Evado.Digital.Dal.";
    /// <summary>
    /// This property contains the method debug log. 
    /// </summary>
    public string ClassNameSpace
    {
      get
      {
        return _ClassNameSpace;
      }
      set
      {
        this._ClassNameSpace = value;
      }
    }

    private EvClassParameters _Settings = new EvClassParameters ( );
    /// <summary>
    /// This property contains the setting data object. 
    /// </summary>
    public EvClassParameters Settings
    {
      get
      {
        return _Settings;
      }
      set
      {
        this._Settings = value;
      }
    }

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Base Class Log properties and methods.

    // 
    // DebugLog stores the business object status conditions.
    // 
    private System.Text.StringBuilder _Log = new System.Text.StringBuilder ( );
    /// <summary>
    /// This property contains the method debug log. 
    /// </summary>
    public string Log
    {
      get
      {
        return _Log.ToString ( );
      }
    }


    // ==================================================================================
    /// <summary>
    /// This method resets the debug log to empty
    /// </summary>
    // ----------------------------------------------------------------------------------
    protected void FlushLog ( )
    {
      this._Log = new System.Text.StringBuilder ( );
    }//END FlushLog class

    //  ==================================================================================
    /// <summary>
    /// This class writes Debug line to method status.
    /// </summary>
    //  ----------------------------------------------------------------------------------
    protected void LogMethod ( String MethodName )
    {
      if ( this._Settings.LoggingLevel >= 3 )
      {
        this._Log.AppendLine ( Evado.Model.EvStatics.CONST_METHOD_START
          + DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": "
          + this._ClassNameSpace + MethodName );
      }
    }//END LogMethod class

    //  ==================================================================================
    /// <summary>
    /// This class writes Debug line to method status.
    /// </summary>
    //  ----------------------------------------------------------------------------------
    protected void LogMethodEnd ( String MethodName )
    {
      if ( this._Settings.LoggingLevel >= 3 )
      {
        String value = Evado.Model.EvStatics.CONST_METHOD_END;

        value = value.Replace ( " END OF METHOD ", " END OF " + MethodName + " METHOD " );

        this._Log.AppendLine ( value );
      }
    }//END LogMethodEnd class

    // ==================================================================================
    /// <summary>
    /// This method appends EVENT to the class log
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogEvent ( String Value )
    {
      if ( this._Settings.LoggingLevel >= 0 )
      {
        this._Log.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + " EVENT: " + Value );
      }
    }//END LogEvent class

    // ==================================================================================
    /// <summary>
    /// This method appends EVENT to the class log
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Ex">Exception object.</param>
    // ----------------------------------------------------------------------------------
    protected void LogEvent ( Exception Ex )
    {
      if ( this._Settings.LoggingLevel >= 0 )
      {
        this._Log.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + " EXCEPTION EVENT: " );
        this._Log.AppendLine ( EvStatics.getException ( Ex ) );
      }
    }//END LogEvent class

    // ==================================================================================
    /// <summary>
    /// This method appends the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Content">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogClass ( String Content )
    {
      if ( this._Settings.LoggingLevel >= 3 )
      {
        this._Log.AppendLine ( Content );
      }
    }//END LogValue class

    // ==================================================================================
    /// <summary>
    /// This method appends the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Content">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogValue ( String Content )
    {
      if ( this._Settings.LoggingLevel >= 3 )
      {
        this._Log.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " + Content );
      }
    }//END LogValue class

    // ==================================================================================
    /// <summary>
    /// This method appends the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Content">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogDebugValue ( String Content )
    {
      if ( this._Settings.LoggingLevel > 4 )
      {
        this._Log.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " + Content );
      }
    }//END LogDebugValue class

    #endregion
  }//END EvApplicationEvents

}//END namespace Evado.Digital.Dal
