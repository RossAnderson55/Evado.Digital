using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

using Evado.Model;


namespace Evado.Model.UniForm
{
  /// <summary>
  /// This 
  /// </summary>
  public class ApplicationAdapterBase
  {
    #region Class Global Objects
    /// <summary>
    /// This object is the hashe table storing the user session data.
    /// </summary>
    private Hashtable _GlobalObjectList = new Hashtable ( );

    /// <summary>
    /// This property contains a hash table for global objects.
    /// </summary>
    public Hashtable GlobalObjectList
    {
      get { return _GlobalObjectList; }
      set { _GlobalObjectList = value; }
    }

    private String _ApplicationPath;

    /// <summary>
    /// This property contains an application path for the class.
    /// </summary>
    public String ApplicationPath
    {
      get { return this._ApplicationPath; }
      set { this._ApplicationPath = value; }
    }

    private String _UniForm_BinaryFilePath;

    /// <summary>
    /// This property contains a binary file path  for the class.
    /// </summary>
    public String UniForm_BinaryFilePath
    {
      get { return this._UniForm_BinaryFilePath; }
      set { this._UniForm_BinaryFilePath = value; }
    }

    private String _UniForm_BinaryServiceUrl;

    /// <summary>
    /// This property contains a service binary Url for the class.
    /// </summary>
    public String UniForm_BinaryServiceUrl
    {
      get { return this._UniForm_BinaryServiceUrl; }
      set { this._UniForm_BinaryServiceUrl = value; }
    }

    private EvUserProfileBase _ServiceUserProfile = new EvUserProfileBase( );

    /// <summary>
    /// This property contains a user profile.
    /// </summary>
    public EvUserProfileBase ServiceUserProfile
    {
      get { return this._ServiceUserProfile; }
      set { this._ServiceUserProfile = value; }
    }

    private Command _ExitCommand = new Command( );

    /// <summary>
    /// This property contains object of Command class .
    /// </summary>
    public Command ExitCommand 
    {
      get { return _ExitCommand; }
      set { _ExitCommand = value; }
    }


    private AppData _ClientDataObject = new AppData ( );

    /// <summary>
    /// This property contains client data object of AppData class.
    /// </summary>
    ///     
    public AppData ClientDataObject
    {
      get { return _ClientDataObject; }
      set { _ClientDataObject = value; }
    }
    
    /// <summary>
    /// This is the base class name space for the adapter class.
    /// </summary>
    protected String ClassNameSpace = "Evado.Model.UniForm.ApplicationServiceBase.";

    /// <summary>
    /// This constant defines session home page identifier.
    /// </summary>
    public const String SESSION_HOME_PAGE_IDENTIFIER = "HomePageIdentifier";
    /// <summary>
    /// This field contains the home page default idenifier.
    /// </summary>
    public Guid HomePageIdentifier = Guid.Empty;

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class Public Methods

    /// <summary>
    /// This field contains the adapter log string builder.
    /// </summary>
    protected StringBuilder _AdapterLog = new StringBuilder ( );
    private const int LoggingMethodLevel = 2;
    private const int LoggingValueLevel = 3;
    private const int DebugValueLevel = 4;

    /// <summary>
    ///  This property contains the debug log entries.
    /// </summary>
    public String AdapterLog
    {
      get { return this._AdapterLog.ToString ( ); }
    }

    private int _LoggingLevel = 0;
    /// <summary>
    /// This property contains the debug state for the class.
    /// </summary>
    public int LoggingLevel
    {
      set
      {
        _LoggingLevel = value;

        if ( this._LoggingLevel < 0 )
        {
          this._LoggingLevel = 0;
        }
        if ( this._LoggingLevel > 5 )
        {
          this._LoggingLevel = 5;
        }
        if ( this._LoggingLevel < 2 )
        {
          this._AdapterLog = new StringBuilder ( );
        }
      }
      get
      {
        return _LoggingLevel;
      }
    }

    // ==================================================================================
    /// <summary>
    /// This property defines if debug logging is enabled.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public bool DebugOn
    {
      get
      {
        if ( this._LoggingLevel > 2 )
        {
          return true;
        }
        return false;
      }
    }

    // ==================================================================================
    /// <summary>
    /// This method gets the application object from the list.
    /// </summary>
    /// <param name="PageCommand">Command: ClientPateCommand object</param>
    /// <returns>Evado.Model.UniForm.AppData</returns>
    // ----------------------------------------------------------------------------------
    public virtual AppData getPageObject( Command PageCommand )
    {
      return new AppData( );

    }//END getPageObject method

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Debug methods.

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    // ----------------------------------------------------------------------------------
    protected void resetApplicationLog ( )
    {
      this._AdapterLog = new StringBuilder();
    }

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
    /// <param name="DebugLogString">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogInitValue ( String DebugLogString )
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
    protected void LogInit ( String Value )
    {
     
        this._AdapterLog.Append (  Value );
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
      if ( this._LoggingLevel >= ApplicationAdapterBase.LoggingMethodLevel )
      {
        this._AdapterLog.AppendLine ( Evado.Model.EvStatics.CONST_METHOD_START
        + DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": "
        + this.ClassNameSpace + Value + " method." );
      }
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
      if ( this._LoggingLevel >= ApplicationAdapterBase.LoggingValueLevel )
      {
        String value = Evado.Model.EvStatics.CONST_METHOD_END;

        value = value.Replace ( " END OF METHOD ", " END OF " + MethodName + " METHOD " );

        this._AdapterLog.AppendLine ( value );
      }
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogValue ( String Value )
    {
      if ( this._LoggingLevel > 1 )
      {
        this._AdapterLog.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " + Value );
      }
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes log string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Format">String: format text.</param>
    /// <param name="args">Array of objects as parameters.</param>
    // ----------------------------------------------------------------------------------
    protected void LogFormat ( String Format, params object [ ] args )
    {
      if ( this._LoggingLevel > 1 )
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
    protected void LogAdapter ( String Value )
    {
      if ( this._LoggingLevel >= ApplicationAdapterBase.LoggingValueLevel )
      {
        this._AdapterLog.Append (  Value );
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
      if ( this._LoggingLevel >= ApplicationAdapterBase.LoggingValueLevel )
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
      if ( this._LoggingLevel >= ApplicationAdapterBase.LoggingValueLevel )
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
      if ( _LoggingLevel >= ApplicationAdapterBase.LoggingValueLevel )
      {
        this._AdapterLog.AppendLine ( Value );
      }
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogDebug ( String Value )
    {
      if ( this._LoggingLevel > ApplicationAdapterBase.DebugValueLevel )
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
    protected void LogDebug ( String Format, params object[] args )
    {
      if ( this._LoggingLevel > ApplicationAdapterBase.DebugValueLevel )
      {
        this._AdapterLog.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " +
          String.Format( Format, args ) );
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
      if ( this._LoggingLevel >= ApplicationAdapterBase.DebugValueLevel )
      {
        this._AdapterLog.Append ( Value );
      }
    }//END AddApplicationEvent class



    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

  }//END Service class

}//END NAMESPACE
