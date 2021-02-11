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
using Evado.Dal;
using Evado.Model.Digital;


namespace Evado.Dal
{
  /// <summary>
  /// This class is the data access layeer based class used by all data access later classes
  /// </summary>
  public class EvDalBase
  {
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EvDalBase ( )
    {
      this._ClassParameters.LoggingLevel = Evado.Dal.EvStaticSetting.LoggingLevel;
    }

    #region Class properties.

    private String _ClassNameSpace = "Evado.Dal.";
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
    /// This property contains the class parameter data object. 
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

        dalApplicationEvents = new Evado.Dal.EvApplicationEvents ( this._ClassParameters );
      }
    }

    // 
    // Initialise the method variables
    // 
    Evado.Dal.EvApplicationEvents dalApplicationEvents = new Evado.Dal.EvApplicationEvents ( );

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Parameter Methods
    //===================================================================================
    /// <summary>
    /// This method returns a list of object parameters selected by the ObjectGuid
    /// </summary>
    /// <param name="ObjectGuid">Guid: object guid identifier</param>
    /// <returns>List of EvObjectParameter objects</returns>
    //-----------------------------------------------------------------------------------
    public List<EvObjectParameter> LoadObjectParameters ( Guid ObjectGuid )
    {
      this.LogMethod ( "UpdateObjectParameters " );
      //
      // Initialise the methods variables and objects.
      //
      EvObjectParameters dllObjectParameters = new EvObjectParameters ( ClassParameters );

      //
      // Retrieve the list of parameters
      //
      var parameters = dllObjectParameters.getParameterList ( ObjectGuid );

      this.LogDebugClass ( dllObjectParameters.Log );

      //
      // Return the list of parameters.
      //
      this.LogMethodEnd ( "UpdateObjectParameters " );
      return parameters;
    }


    //===================================================================================
    /// <summary>
    /// This method returns a list of object parameters selected by the ObjectGuid
    /// </summary>
    /// <param name="ParameterList"> List of EvObjectParameter</param>
    /// <param name="ObjectGuid">Guid: object guid identifier</param>
    /// <returns>List of EvObjectParameter objects</returns>
    //-----------------------------------------------------------------------------------
    public void UpdateObjectParameters ( List<EvObjectParameter> ParameterList, Guid ObjectGuid )
    {
      this.LogMethod ( "UpdateObjectParameters " );
      //
      // Initialise the methods variables and objects.
      //
      EvObjectParameters dllObjectParameters = new EvObjectParameters ( ClassParameters );

      if ( ParameterList.Count == 0 )
      {
        this.LogMethodEnd ( "UpdateObjectParameters " );
        return;
      }
      //
      // update the list of parameters
      //
      dllObjectParameters.updateItems ( ParameterList, ObjectGuid );

      this.LogDebugClass ( dllObjectParameters.Log );

      this.LogMethodEnd ( "UpdateObjectParameters " );
    }
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Log properties and methods.

    /// <summary>
    /// _Log is the StringBuilder object containing the class log.
    /// </summary>
    public System.Text.StringBuilder _Log = new System.Text.StringBuilder ( );
    /// <summary>
    /// This property contains the method debug log. 
    /// </summary>
    public string Log
    {
      get
      {
        return this._Log.ToString ( );
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
      if ( this._ClassParameters.LoggingLevel >= 3 )
      {
        String value = Evado.Model.EvStatics.CONST_METHOD_END;

        value = value.Replace ( " END OF METHOD ", " END OF DAL:" + MethodName + " METHOD " );

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
      this._Log.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + " EVENT: " + Value );
    }//END LogEvent class

    // ==================================================================================
    /// <summary>
    /// This method appends EXCEPTION EVENT to the class log
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Ex">Exception: exception object.</param>
    // ----------------------------------------------------------------------------------
    protected void LogException ( Exception Ex )
    {
      String value = "NameSpace: " + this.ClassNameSpace
       + "\r\nUser: " + this.ClassParameters.UserProfile.UserId
       + "\r\nUserCommonName: " + this.ClassParameters.UserProfile.CommonName
       + "\r\n" + EvStatics.getException ( Ex );

      // 
      // Initialise the method variables
      // 
      EvApplicationEvent applicationEvent = new EvApplicationEvent (
        EvApplicationEvent.EventType.Error,
        EvEventCodes.Ok,
        this.ClassNameSpace,
        value,
        this.ClassParameters.UserProfile.CommonName );

      this.AddEvent ( applicationEvent );

      //
      // Add to the local log.
      //
      this._Log.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + " EXCEPTION EVENT: " );
      this._Log.AppendLine ( value );

    }//END LogEvent class

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
        this.ClassParameters.UserProfile.CommonName );

      this.AddEvent ( applicationEvent );

      if ( this._ClassParameters.LoggingLevel >= 0 )
      {
        this._Log.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + " EXCEPTION EVENT: " );
        this._Log.AppendLine ( Description );
      }
    }//END LogError method

    // ==================================================================================
    /// <summary>
    /// This method appends the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Content">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogClass ( String Content )
    {
      if ( this._ClassParameters.LoggingLevel >= 3 )
      {
        this._Log.AppendLine (  Content );
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
      if ( this._ClassParameters.LoggingLevel >= 3 )
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
    protected void LogDebugClass ( String Content )
    {
      if ( this._ClassParameters.LoggingLevel > 4 )
      {
        this._Log.Append( Content );
      }
    }//END LogValue class

    // ==================================================================================
    /// <summary>
    /// This method appends the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Content">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogDebug ( String Content )
    {
      if ( this._ClassParameters.LoggingLevel > 4 )
      {
        this._Log.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " + Content );
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
        this._Log.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ":" +
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

  }//END EvDalBase Class.

}//END namespace Evado.Dal.Digital 
