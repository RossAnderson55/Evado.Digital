/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\Subjects.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c)  2002 - 2021  EVADO HOLDING PTY. LTD..  All rights reserved.
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
 *  This class contains the AbstractedPage ResultData object.
 *
 ****************************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.SessionState;

using Evado.Model;
// using Evado.Web;

namespace Evado.UniForm.Digital
{
  /// <summary>
  /// This class defines the application base classs that is used to terminate the 
  /// hosted application objects.
  /// 
  /// This class terminates the Customer object.
  /// </summary>
  public class EuClassAdapterBase
  {
    #region Class constants and variables.

    private String _UniForm_BinaryFilePath;

    /// <summary>
    /// This property passes in the session state for the class.
    /// </summary>
    public String UniForm_BinaryFilePath
    {
      get { return this._UniForm_BinaryFilePath; }
      set { this._UniForm_BinaryFilePath = value; }
    }

    /// <summary>
    /// This property passes the image file path.
    /// </summary>
    public String UniForm_ImageFilePath
    {
      get
      {
        return _UniForm_BinaryFilePath + @"images\";
      }
    }

    private String _UniForm_BinaryServiceUrl = String.Empty;
    /// <summary>
    /// This property passes in the binary file path the class.
    /// </summary>
    public String UniForm_BinaryServiceUrl
    {
      get { return this._UniForm_BinaryServiceUrl; }
      set { this._UniForm_BinaryServiceUrl = value; }
    }

    /// <summary>
    /// This property passes the image service url
    /// </summary>
    public String UniForm_ImageServiceUrl
    {
      get
      {
        return _UniForm_BinaryServiceUrl + @"images/";
      }
    }

    private EvUserProfileBase _ServiceUserProfile;

    /// <summary>
    /// This property passes in the session state for the class.
    /// </summary>
    public EvUserProfileBase ServiceUserProfile
    {
      get { return this._ServiceUserProfile; }
      set { this._ServiceUserProfile = value; }
    }

    private EuGlobalObjects _GlobalObjects = new EuGlobalObjects ( );
    /// <summary>
    /// This property contains the global application parameters.
    /// </summary>
    public EuGlobalObjects AdapterObjects
    {
      get { return _GlobalObjects; }
      set { _GlobalObjects = value; }
    }

    private EuSession _Session = new EuSession ( );

    /// <summary>
    /// this variable contains the the user's profile.
    /// </summary>
    public EuSession Session
    {
      get { return _Session; }
      set { _Session = value; }
    }

    private String _ErrorMessage = String.Empty;

    /// <summary>
    /// This property defines the error message to be displayed to the user.
    /// </summary>
    public string ErrorMessage
    {
      get
      {
        return _ErrorMessage;
      }
      set { _ErrorMessage = value; }
    }

    protected String _FileRepositoryPath;

    /// <summary>
    /// This property contains the binary files repository path
    /// </summary>

    public String FileRepositoryPath
    {
      get { return this._FileRepositoryPath; }
      set { this._FileRepositoryPath = value; }
    }


    private Evado.Model.Digital.EvClassParameters _Settings = new Evado.Model.Digital.EvClassParameters ( );
    /// <summary>
    /// This property contains the setting data object. 
    /// </summary>
    public Evado.Model.Digital.EvClassParameters ClassParameters
    {
      get
      {
        return _Settings;
      }
      set
      {
        this._Settings = value;

        //this._Bll_ApplicationEvents = new Bll.EvApplicationEvents ( this._Settings );
      }
    }

    //private Evado.Bll.EvApplicationEvents _Bll_ApplicationEvents = new Bll.EvApplicationEvents ( );
   
    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class public methods

    /// <summary>
    /// This method gets the application object from the list.
    /// 
    /// </summary>
    /// <param name="PageCommand">ClientPateEvado.Model.UniForm.Command object</param>
    /// <returns>ClientApplicationData</returns>
    //  ----------------------------------------------------------------------------------
    public virtual Evado.Model.UniForm.AppData getDataObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getDataObject" );
      this.LogValue ( "Parameter PageCommand " + PageCommand.getAsString ( false, false ) );

      return new Evado.Model.UniForm.AppData ( );

    }//END getSubjectObject method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Debug methods.

    /// <summary>
    /// this object stores the application log content.
    /// </summary>
    private StringBuilder _AdapterLog = new StringBuilder ( );
    public const int LoggingEventLevel = 0;
    public const int LoggingMethodLevel = 3;
    public const int LoggingValueLevel = 4;
    public const int DebugValueLevel = 5;

    public string Log
    {
      get
      {
        return _AdapterLog.ToString ( );
      }
    }

    private int _LoggingLevel = 0;
    /// <summary>
    /// This property sets the debug state for the class.
    /// </summary>
    public int LoggingLevel
    {
      get { return _LoggingLevel; }
      set
      {
        this._LoggingLevel = value;

        if ( this._LoggingLevel < 0 )
        {
          this._LoggingLevel = 0;
        }
        if ( this._LoggingLevel > 5 )
        {
          this._LoggingLevel = 5;
        }
        if ( this._LoggingLevel < EuClassAdapterBase.LoggingMethodLevel )
        {
          this.resetAdapterLog ( ); ;
        }
      }
    }

    public String ClassNameSpace = "Evado.Model.UniForm.ApplicationServiceBase.";

    // ==================================================================================
    /// <summary>
    /// This method resets the debug log
    /// </summary>
    // ----------------------------------------------------------------------------------
    protected void resetAdapterLog ( )
    {
      this._AdapterLog = new StringBuilder ( );
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogEvent ( String Value )
    {
      EvApplicationEvent applicationEvent = new EvApplicationEvent (
        EvApplicationEvent.EventType.Warning,
        EvEventCodes.Ok,
        this.ClassNameSpace,
        Value,
        this.Session.UserProfile.CommonName );

      this.AddEvent ( applicationEvent );

      this._AdapterLog.AppendLine (
        DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + " EVENT:  " + Value );
    }

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
      EvEventCodes EventId )
    {
      // 
      // Initialise the method variables
      // 
      String stEvent = "EventId: " + EventId;

      EvApplicationEvent applicationEvent = new EvApplicationEvent (
        EvApplicationEvent.EventType.Error,
        EventId,
        this.ClassNameSpace,
        stEvent,
        this.Session.UserProfile.CommonName );

      this.AddEvent ( applicationEvent );

      this._AdapterLog.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" )
        + " EXCEPTION EVENT: " );
      this._AdapterLog.AppendLine ( stEvent );

    }//END LogError method

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
      String stEvent = "EventId: " +EventId
        + "\r\n" + Description ;

      EvApplicationEvent applicationEvent = new EvApplicationEvent (
        EvApplicationEvent.EventType.Error,
        EventId,
        this.ClassNameSpace,
        stEvent,
        this.Session.UserProfile.CommonName );

      this.AddEvent ( applicationEvent );

      this._AdapterLog.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" )
        + " EXCEPTION EVENT: " );
      this._AdapterLog.AppendLine ( stEvent );

    }//END LogError method

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    public void LogAction ( string Value )
    {
      //
      // create the application event
      //
      EvApplicationEvent applicationEvent = new EvApplicationEvent (
        EvApplicationEvent.EventType.Action,
        EvEventCodes.Ok,
        this.ClassNameSpace,
        Value,
        this.Session.UserProfile.CommonName );

      //
      // Log the application event.
      //
      this.AddEvent ( applicationEvent );

      //
      // Append the value to the text log.
      //
      this._AdapterLog.AppendLine (
        DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + " ACTION:  " + Value );
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the page command to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    public void LogAction ( Evado.Model.UniForm.Command PageCommand, bool WithParameters )
    {
      //
      // get the command values.
      //
      string Value = "PageCommand: " + PageCommand.getAsString ( false, WithParameters );

      //
      // Append the value to the text log.
      //
      this._AdapterLog.AppendLine (
        DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + " ACTION:  " + Value );

      //
      // create the aplplication event
      //
      EvApplicationEvent applicationEvent = new EvApplicationEvent (
        EvApplicationEvent.EventType.Action,
        EvEventCodes.Ok,
        this.ClassNameSpace,
        Value,
        this.Session.UserProfile.CommonName );

      //
      // Log the application event.
      //
      this.AddEvent ( applicationEvent );
    }

    // ==================================================================================
    /// <summary>
    /// This method appends EVENT to the class log
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Ex">Exception object.</param>
    // ----------------------------------------------------------------------------------
    public void LogException ( Exception Ex )
    {
      String value = "NameSpace: " + this.ClassNameSpace
       + "\r\nUser: " + this.Session.UserProfile.UserId
       + "\r\nUserCommonName: " + this.Session.UserProfile.CommonName
       + "\r\n" + EvStatics.getException ( Ex );

      // 
      // Initialise the method variables
      // 
      EvApplicationEvent applicationEvent = new EvApplicationEvent (
        EvApplicationEvent.EventType.Error,
        EvEventCodes.Ok,
        this.ClassNameSpace,
        value,
        this.Session.UserProfile.CommonName );

      this.AddEvent ( applicationEvent );

      if ( this._Settings.LoggingLevel >= 0 )
      {
        this._AdapterLog.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + " EXCEPTION EVENT: " );
        this._AdapterLog.AppendLine ( value );
      }
    }//END LogException class

    //  ===========================================================================
    /// <summary>
    /// This class creates the Evado EvEvent Source. This requires administrator privileges
    /// because it needs to write to the registry have.
    /// </summary>
    /// <param name="ClassMethodAccessed">String: a class method accessed string</param>
    /// <param name="User">Evado.Model.Digital.EvUserProfile: A user's profile</param>
    /// <returns>Boolean: true, If the event source was created successfully.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Set Site and EventLogSource2 to default value, if they are not empty.
    /// 
    /// 2. Update the application event value. 
    /// 
    /// 3. Adding items to application event table.
    /// 
    /// 4. Return false, if the adding runs fail
    /// 
    /// 5. Else, return ture. 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public void LogPageAccess (
     String ClassMethodAccessed,
      Evado.Model.Digital.EdUserProfile User )
    {
      // 
      // Initialise the method variables
      // 
      string stEvent = "UserId: " + User.UserId
            + " CommonName: " + User.CommonName
            + " opened this " + ClassMethodAccessed 
            + " method at " + DateTime.Now.ToString ( "dd MMM yyyy HH:mm:ss" );

      //
      // Append the value to the text log.
      //
      this._AdapterLog.AppendLine (
        DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + " ACTION:  " + stEvent );

      //
      // create the application event
      //
      EvApplicationEvent applicationEvent = new EvApplicationEvent (
        EvApplicationEvent.EventType.Action,
        EvEventCodes.Ok,
        this.ClassNameSpace,
        stEvent,
        User.CommonName );

      //
      // Log the event to the application log.
      //
      this.AddEvent ( applicationEvent );

    }//END LogPageAccess class

    //  ===========================================================================
    /// <summary>
    /// This class creates the Evado EvEvent Source. This requires administrator privileges
    /// because it needs to write to the registry have.
    /// </summary>
    /// <param name="ClassMethodAccessed">String: a class method accessed string</param>
    /// <param name="User">Evado.Model.Digital.EvUserProfile: A user's profile</param>
    /// <returns>Boolean: true, If the event source was created successfully.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Set Site and EventLogSource2 to default value, if they are not empty.
    /// 
    /// 2. Update the application event value. 
    /// 
    /// 3. Adding items to application event table.
    /// 
    /// 4. Return false, if the adding runs fail
    /// 
    /// 5. Else, return ture. 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public void LogIllegalAccess (
     String ClassMethodAccessed,
      Evado.Model.Digital.EdUserProfile User )
    {
      // 
      // Initialise the method variables
      // 
      System.Text.StringBuilder stEvent = new StringBuilder ( );

      stEvent.AppendFormat (
         "Illegal access attempt UserId: {0} name {1} opened {2} method at {3} ",
         User.UserId,
         User.CommonName,
         ClassMethodAccessed,
         DateTime.Now.ToString ( "dd MMM yyyy HH:mm:ss" ) );

      //
      // Log the event to the application log.
      //
      this.LogError ( EvEventCodes.User_Access_Error, stEvent.ToString ( ) );

    }//END LogPageAccess class

    //  ===========================================================================
    /// <summary>
    /// This class creates the Evado EvEvent Source. This requires administrator privileges
    /// because it needs to write to the registry have.
    /// </summary>
    /// <param name="ClassMethodAccessed">String: a class method accessed string</param>
    /// <param name="User">Evado.Model.Digital.EvUserProfile: A user's profile</param>
    /// <returns>Boolean: true, If the event source was created successfully.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Set Site and EventLogSource2 to default value, if they are not empty.
    /// 
    /// 2. Update the application event value. 
    /// 
    /// 3. Adding items to application event table.
    /// 
    /// 4. Return false, if the adding runs fail
    /// 
    /// 5. Else, return ture. 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public void LogIllegalAccess (
     String ClassMethodAccessed,
      String RoleId,
      Evado.Model.Digital.EdUserProfile User )
    {
      // 
      // Initialise the method variables
      // 
      System.Text.StringBuilder stEvent = new StringBuilder ( );

      stEvent.AppendFormat (
         "Illegal access attempt UserId: {0} name {1} opened {2} method at {3} ",
         User.UserId, 
         User.CommonName, 
         ClassMethodAccessed, 
         DateTime.Now.ToString ( "dd MMM yyyy HH:mm:ss" ) );
      
      stEvent.AppendFormat ( 
        "Page Role: {0} user Access: {1} ",
        User.Roles, 
        RoleId ); 

      //
      // Log the event to the application log.
      //
      this.LogError ( EvEventCodes.User_Access_Error, stEvent.ToString() );

    }//END LogPageAccess class

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogInitMethod ( String Value )
    {
      this._AdapterLog.AppendLine ( Evado.Model.EvStatics.CONST_METHOD_START
      + DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": "
      + this.ClassNameSpace + Value );
    }
    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogInitMethodEnd ( String MethodName )
    {
      String value = Evado.Model.EvStatics.CONST_METHOD_END;

      value = value.Replace ( " END OF METHOD ", " END OF " + MethodName + " METHOD " );

      this._AdapterLog.AppendLine ( value );
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="DebugLogString">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogInit ( String DebugLogString )
    {
        this._AdapterLog.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " + DebugLogString );
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogMethod ( String Value )
    {
      this._AdapterLog.AppendLine ( Evado.Model.EvStatics.CONST_METHOD_START
      + DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": "
      + this.ClassNameSpace + Value + " method");
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="MethodName">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogMethodEnd ( String MethodName )
    {
      if ( this.LoggingLevel >= EuClassAdapterBase.LoggingValueLevel )
      {
        String value = Evado.Model.EvStatics.CONST_METHOD_END;

        value = value.Replace ( " END OF METHOD ", " END OF " + MethodName + " METHOD " );

        this._AdapterLog.AppendLine ( value );
      }
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogValue ( String Value )
    {
      if ( this.LoggingLevel >= EuClassAdapterBase.LoggingValueLevel )
      {
        this._AdapterLog.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " + Value );
      }
    }

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
      if ( this.LoggingLevel >= EuClassAdapterBase.LoggingValueLevel )
      {
        this._AdapterLog.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " +
          String.Format ( Format, args ) );
      }
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogClass ( String Value )
    {
      if ( this.LoggingLevel >= EuClassAdapterBase.LoggingValueLevel )
      {
        this._AdapterLog.Append ( Value );
      }
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogTextStart ( String Value )
    {
      if ( this.LoggingLevel >= EuClassAdapterBase.LoggingValueLevel )
      {
        this._AdapterLog.Append ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " + Value );
      }
    }


    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogTextEnd ( String Value )
    {
      if ( this.LoggingLevel >= EuClassAdapterBase.LoggingValueLevel )
      {
        this._AdapterLog.AppendLine ( Value );
      }
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogText ( String Value )
    {
      if ( _LoggingLevel >= EuClassAdapterBase.LoggingValueLevel )
      {
        this._AdapterLog.Append ( Value );
      }
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogDebug ( String Value )
    {
      if ( this.LoggingLevel >= EuClassAdapterBase.DebugValueLevel )
      {
        this._AdapterLog.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " + Value );
      }
    }

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
      if ( this.LoggingLevel >= EuClassAdapterBase.DebugValueLevel )
      {
        this._AdapterLog.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " +
          String.Format ( Format, args ) );
      }
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogDebugClass ( String Value )
    {
      if ( this.LoggingLevel >= EuClassAdapterBase.DebugValueLevel )
      {
        this._AdapterLog.Append ( Value );
      }
    }

    // ==================================================================================
    /// <summary>
    /// This property define whether debug loggin is enabled.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public bool DebugOn
    {
      get
      {
        if ( this.LoggingLevel >= EuClassAdapterBase.DebugValueLevel )
        {
          return true;
        }
        return false;
      }
    }

    // =====================================================================================
    /// <summary>
    /// This class adds a new application event object to the database. 
    /// </summary>
    /// <param name="ApplicationEvent">EvApplicationEvent: An application event object</param>
    /// <returns>EvEventCodes: an event code for adding items</returns>
    // -------------------------------------------------------------------------------------
    private EvEventCodes AddEvent (
      EvApplicationEvent ApplicationEvent )
    {
      Evado.Bll.EvApplicationEvents bll_ApplicationEvents = new Bll.EvApplicationEvents (
        this.ClassParameters );

      try
      {
        return bll_ApplicationEvents.AddEvent ( ApplicationEvent );
      }
      catch(Exception Ex )
      {
        this._AdapterLog.AppendLine( Evado.Model.EvStatics.getException( Ex ) );

        return EvEventCodes.Database_Record_Update_Error;
      }

    }//END AddEvent method



    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
  }
}//END namespace