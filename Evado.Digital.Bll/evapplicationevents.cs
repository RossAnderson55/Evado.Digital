/***************************************************************************************
 * <copyright file="BLL\EvApplicationEvents.cs" company="EVADO HOLDING PTY. LTD.">
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
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;

//Evado. namespace references.
using Evado.Model;
using Evado.Digital.Model;


namespace Evado.Digital.Bll
{
  /// <summary>
  /// A business to manage EvApplicationEvents. This class uses EvApplicationEvent ResultData object for its content.
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
      this.ClassNameSpace = "Evado.Digital.Bll.Clinical.EvApplicationEvents.";
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
      this.ClassNameSpace = "Evado.Digital.Bll.Clinical.EvApplicationEvents.";

      if ( this.Settings.LoggingLevel == 0 )
      {
        this.Settings.LoggingLevel = Evado.Digital.Dal.EvStaticSetting.LoggingLevel;
      }

      this._dalApplicationEvents = new Evado.Digital.Dal.EvApplicationEvents ( Settings );
    }
    #endregion

    #region Class properties.

    private String _ClassNameSpace = "Evado.Digital.Bll.";
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

    #region Log properties and methods.

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

        value = value.Replace ( " END OF METHOD ", " END OF " + this._ClassNameSpace + MethodName + " METHOD " );

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
    /// <param name="Ex">Exception object</param>
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
    /// This method appends the class log to the log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogClass ( String Value )
    {
      if ( this._Settings.LoggingLevel >= 3 )
      {
        this._Log.AppendLine ( Value );
      }
    }//END LogValue class

    // ==================================================================================
    /// <summary>
    /// This method appends the log string to the log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogValue ( String Value )
    {
      if ( this._Settings.LoggingLevel >= 4 )
      {
        this._Log.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " + Value );
      }
    }//END LogValue class

    // ==================================================================================
    /// <summary>
    /// This method appends the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogDebug ( String Value )
    {
      if ( this._Settings.LoggingLevel > 4 )
      {
        this._Log.AppendLine ( Value );
      }
    }//END LogValue class

    // ==================================================================================
    /// <summary>
    /// This method appends the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogDebugValue ( String Value )
    {
      if ( this._Settings.LoggingLevel > 4 )
      {
        this._Log.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " + Value );
      }
    }//END LogDebugValue class

    #endregion

    #region Initialise class objects and variables
    // 
    // Create instantiate the DAL class 
    // 
    private Evado.Digital.Dal.EvApplicationEvents _dalApplicationEvents = new Evado.Digital.Dal.EvApplicationEvents ( );

    #endregion

    #region Class query methods

    // =====================================================================================
    /// <summary>
    /// This class gets an ArrayList containing a selectionList of EvApplicationEvent ResultData objects.
    /// </summary>
    /// <param name="EventId">string: The event identifier</param>
    /// <param name="Type">int: The event QueryType</param>
    /// <param name="Category">string: The event category</param>
    /// <param name="DateStart">DateTime: The period start date</param>
    /// <param name="DateFinish">DateTime: The period finish date</param>
    /// <param name="UserName">string: The user that raised  the event</param>
    /// <returns>List of EvApplicationEvent: A list of ApplicationEvent objects.</returns>
    // -------------------------------------------------------------------------------------
    public List<EvApplicationEvent> getEventList (
      int EventId,
      string Type,
      string Category,
      DateTime DateStart,
      DateTime DateFinish,
      string UserName )
    {
      this.LogMethod ( "getEventList method. " );
      this.LogDebugValue( "EventId: " + EventId);
      this.LogDebugValue( "Type: " + Type);
      this.LogDebugValue( "Category: " + Category);
      this.LogDebugValue ( "DateStart: " + DateStart );
      this.LogDebugValue ( "DateFinish: " + DateFinish );
      this.LogDebugValue( "UserName: " + UserName );

      List<EvApplicationEvent> View = this._dalApplicationEvents.getApplicationEventList (
         EventId,
         Type,
         Category,
         DateStart,
         DateFinish,
         UserName );

      this.LogClass( this._dalApplicationEvents.Log );

      return View;

    }// End getMilestoneList method.

    #endregion

    #region Get object methods

    // =====================================================================================
    /// <summary>
    /// This class gets EvApplicationEvent ResultData object by its unique identifier.
    /// </summary>
    /// <param name="EventGuid">string: a unique identifier</param>
    /// <returns>EvApplicationEvent: An application event ResultData object</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Retrieve the application event object. 
    /// 
    /// 2. Return the application event object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvApplicationEvent getItem ( Guid EventGuid )
    {
      this.LogMethod ( "getEventList method. " );
      this.LogDebugValue( "EventGuid: " + EventGuid );

      EvApplicationEvent applicationEvent = this._dalApplicationEvents.getItem ( EventGuid );

      this.LogClass ( this._dalApplicationEvents.Log );

      return applicationEvent;

    }//END getItem class. 

    #endregion

    #region Save object methods

    // =====================================================================================
    /// <summary>
    /// This class adds items to the Application event datatable. 
    /// </summary>
    /// <param name="ApplicationEvent">EvApplicationEvent: an application event ResultData object</param>
    /// <returns>EvEventCodes: an event code for adding items</returns>
    /// <remarks>
    /// This class consists of the following steps: 
    /// 
    /// 1. Add items to the application events table. 
    /// 
    /// 2. Return the event code for adding items. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes AddEvent ( EvApplicationEvent ApplicationEvent )
    {
      this.LogMethod ( "AddEvent method. " );
      this.LogDebugValue( "EventId: " + ApplicationEvent.EventId );
      EvEventCodes result = EvEventCodes.Ok;

      if ( ApplicationEvent.UserName == String.Empty )
      {
        ApplicationEvent.UserName = "Evado Executive";  
      }

      //
      // If the action is set to delete the object.
      // 
      result = this._dalApplicationEvents.addEvent ( ApplicationEvent );

      this.LogClass ( this._dalApplicationEvents.Log );

      this.LogDebugValue ( "result: " + result );

      return result;

    }//END method AddEvent
    #endregion

    #region Static methods 
    // =====================================================================================
    /// <summary>
    /// This class adds items to the application event table. 
    /// </summary>
    /// <param name="ApplicationEvent">EvApplicationEvent: An application event object</param>
    /// <returns>EvEventCodes: an event code for adding items</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Execute the method for adding items to application event ResultData table. 
    /// 
    /// 2. Return event code for adding items. 
    /// 
    /// 3. Else, exit. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public static EvEventCodes NewEvent (
      EvApplicationEvent ApplicationEvent )
    {
      try
      {
        // 
        // Initialise the method variables
        // 
        Evado.Digital.Dal.EvApplicationEvents dalApplicationEvents = new Evado.Digital.Dal.EvApplicationEvents ( );

        //
        // If the action is set to delete the object.
        // 
        return dalApplicationEvents.addEvent ( ApplicationEvent );
      }
      catch
      {
        return EvEventCodes.Database_Communications_Error;
      }

    }//END AddEvent class

    #endregion
  }//END EvApplicationEvents Class.
}//END namespace Evado.Evado.BLL 
