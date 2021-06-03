/***************************************************************************************
 * <copyright file="BLL\EvActivities.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvActivities business object.
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
  /// This business object manages the EvTrialMilestones in the system.
  /// </summary>
  public class EvBllBase
  {
    #region initialise class.
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EvBllBase ( )
    {
      this._ClassParameters.LoggingLevel = Evado.Digital.Dal.EvStaticSetting.LoggingLevel;
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

    private EvClassParameters _ClassParameters = new EvClassParameters ( );
    /// <summary>
    /// This property contains the class parameters data object. 
    /// </summary>
    public EvClassParameters ClassParameters
    {
      get
      {
        return _ClassParameters;
      }
      set
      {
        this._ClassParameters = value;

        _CustomerId = this._ClassParameters.CustomerId;

        dalApplicationEvents = new Evado.Digital.Dal.EvApplicationEvents ( this._ClassParameters );
      }
    }        
    
    // 
    // Initialise the method variables
    // 
    Evado.Digital.Dal.EvApplicationEvents dalApplicationEvents = new Evado.Digital.Dal.EvApplicationEvents ( );

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Log properties and methods.

    /// <summary>
    /// This protected field contains the method log. 
    /// </summary>
    protected System.Text.StringBuilder _Log = new System.Text.StringBuilder ( );
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
      if ( this._ClassParameters.LoggingLevel >= 3 )
      {
        this._Log.AppendLine ( Evado.Model.EvStatics.CONST_METHOD_START
          + DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": "
          + this._ClassNameSpace + MethodName + " method.");
      }
    }//END LogMethod class

    //  ==================================================================================
    /// <summary>
    /// This class writes Debug line to method status.
    /// </summary>
    //  ----------------------------------------------------------------------------------
    protected void LogMethodEnd ( String MethodName )
    {
      if ( this._ClassParameters.LoggingLevel >= 3 )
      {
        String value = Evado.Model.EvStatics.CONST_METHOD_END;

        value = value.Replace ( " END OF METHOD ", " END OF BLL:" + MethodName + " METHOD " );

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
      // 
      // Initialise the method variables
      // 
      EvEventCodes iReturn = EvEventCodes.Ok;
      EvApplicationEvent ApplicationEvent = new EvApplicationEvent (
        EvApplicationEvent.EventType.Warning,
        EvEventCodes.Ok,
        this.ClassNameSpace,
        Value,
        this.ClassParameters.UserProfile.UserId );

      if ( this._ClassParameters.LoggingLevel >= 0 )
      {
        this._Log.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + " EVENT: " );
        this._Log.AppendLine ( Value );
      }

      //
      // If the action is set to delete the object.
      // 
      iReturn = this.AddEvent ( ApplicationEvent );

    }//END LogEvent class

    // ==================================================================================
    /// <summary>
    /// This method appends EVENT to the class log
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Ex">Exception object.</param>
    // ----------------------------------------------------------------------------------
    protected void LogException ( Exception Ex )
    {
      String value = "NameSpace: " + this.ClassNameSpace 
       + "\r\nUser: " + this.ClassParameters.UserProfile.UserId
       + "\r\nUserCommonName: " + this.ClassParameters.UserProfile.CommonName 
       + "\r\n" + EvStatics.getException ( Ex ) ;

      // 
      // Initialise the method variables
      // 
      EvApplicationEvent applicationEvent = new EvApplicationEvent (
        EvApplicationEvent.EventType.Error,
        EvEventCodes.Ok,
        this.ClassNameSpace,
        value,
        this.ClassParameters.UserProfile.UserId );

      this.AddEvent ( applicationEvent );

      //
      // Add to the local log.
      //
      this._Log.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + " EXCEPTION EVENT: " );
      this._Log.AppendLine ( value );

    }//END LogException class

    // =====================================================================================
    /// <summary>
    /// This class checks whether the event's description,  Category and user name are written to the logError
    /// </summary>
    /// <param name="EventId">Integer: an event identifier</param>
    /// <param name="Description">string: an event's description</param>
    /// <returns>Boolean: true, if the event is logged</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Fill the application event object. 
    /// 
    /// 2. Write the application event log
    /// 
    /// 3. Adding items to application event table. 
    /// 
    /// 4. Return true, if adding runs successfully. 
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public void LogError (
      EvEventCodes EventId,
      String Description )
    {
      // 
      // Initialise the method variables
      // 
      EvApplicationEvent applicationEvent = new EvApplicationEvent (
        EvApplicationEvent.EventType.Error,
        EventId,
        this.ClassNameSpace,
        Description,
        this.ClassParameters.UserProfile.UserId );

      this.AddEvent ( applicationEvent );

      if ( this._ClassParameters.LoggingLevel >= 0 )
      {
        this._Log.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + " EXCEPTION EVENT: " );
        this._Log.AppendLine ( Description );
      }
    }//END LogError method

    // ==================================================================================
    /// <summary>
    /// This method appends the class log to the log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogClass ( String Value )
    {
      if ( this._ClassParameters.LoggingLevel >= 3 )
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
      if ( this._ClassParameters.LoggingLevel >= 3 )
      {
        this._Log.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " + Value );
      }
    }//END LogValue class
    // ==================================================================================
    /// <summary>
    /// This method appendes debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Format">String: format text.</param>
    /// <param name="args">Array of objects as parameters.</param>
    // ----------------------------------------------------------------------------------
    protected void LogValue ( String Format, params object [ ] args )
    {
      if ( this._ClassParameters.LoggingLevel >= 3 )
      {
        this._Log.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " +
          String.Format ( Format, args ) );
      }
    }

    // ==================================================================================
    /// <summary>
    /// This method appends the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogDebugClass ( String Value )
    {
      if ( this._ClassParameters.LoggingLevel > 4 )
      {
        this._Log.Append ( Value );
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
      if ( this._ClassParameters.LoggingLevel > 4 )
      {
        this._Log.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " + Value );
      }
    }//END LogDebugValue method

    // ==================================================================================
    /// <summary>
    /// This method appendes debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Format">String: format text.</param>
    /// <param name="args">Array of objects as parameters.</param>
    // ----------------------------------------------------------------------------------
    protected void LogDebug ( String Format, params object [ ] args )
    {
      if ( this._ClassParameters.LoggingLevel > 4 )
      {
        this._Log.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " +
          String.Format ( Format, args ) );
      }
    }

    #endregion

    #region Add EvEvent methods

    private static String _CustomerId = "A";

    // =====================================================================================
    /// <summary>
    /// This class adds items to the application event table. 
    /// </summary>
    /// <param name="ApplicationEvent">EvApplicationEvent: An application event object</param>
    /// <returns>EvEventCodes: an event code for adding items</returns>
    // -------------------------------------------------------------------------------------
    public EvEventCodes AddEvent (
      EvApplicationEvent ApplicationEvent )
    {
      try
      {
        //
        // If the action is set to delete the object.
        // 
        return dalApplicationEvents.addEvent ( ApplicationEvent );
      }
      catch
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

    }//END AddApplicationEvent class

    #endregion


  }//END EvActivities Class.

}//END namespace Evado.Digital.Bll 
