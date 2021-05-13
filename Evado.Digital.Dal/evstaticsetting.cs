using System;
using System.Collections.Generic;
using System.Text;

using Evado.Model;
using Evado.Digital.Model;

namespace Evado.Digital.Dal
{
  /// <summary>
  /// This class manages the setting of the connection string key to be used by the 
  /// SQL method class to access the database.
  /// </summary>
  public class EvStaticSetting
  {
    private static String _eventLogSource = "Application";

    // ================================================================================
    /// <summary>
    /// This property contains the Event Source
    /// </summary>
    // ----------------------------------------------------------------------------------
    public static String EventLogSource
    {
      set
      {
        EvStaticSetting._eventLogSource = value;

        Evado.Digital.Dal.EvSqlMethods.EventLogSource = EvStaticSetting._eventLogSource;
      }
      get
      {
        return EvStaticSetting._eventLogSource;
      }
    }//END setConnectionSetting class

    private static int _LoggingLevel = 0;

    /// <summary>
    /// This property defines the logging level of the application.
    /// </summary>
    public static int LoggingLevel
    {
      get
      {
        return EvStaticSetting._LoggingLevel;
      }
      set
      {
        EvStaticSetting._LoggingLevel = value;
      }
    }


    // ================================================================================
    /// <summary>
    /// This property contains the connection string key for sql methods
    /// </summary>
    // ----------------------------------------------------------------------------------
    public static bool DebugOn
    {
      get
      {
        if ( LoggingLevel > 4 )
        {
          return true;
        }
        return false;
      }

    }//END DebugOn

    private static Guid _SiteGuid = Guid.Empty;

    // ================================================================================
    /// <summary>
    /// This property contains the site guid 
    /// </summary>
    // ----------------------------------------------------------------------------------
    public static Guid SiteGuid
    {
      get
      {
        return EvStaticSetting._SiteGuid;
      }
      set
      {
        EvStaticSetting._SiteGuid = value;
      }
    }

    private static String _connectionStringKey = String.Empty;

    // ================================================================================
    /// <summary>
    /// This property contains the connection string key for sql methods
    /// </summary>
    // ----------------------------------------------------------------------------------
    public static String ConnectionStringKey
    {
      set
      {
        EvStaticSetting._connectionStringKey = value;

        Evado.Digital.Dal.EvSqlMethods.ConnectionStringKey = EvStaticSetting._connectionStringKey;
      }
      get
      {
        return EvStaticSetting._connectionStringKey;
      }
    }//END setConnectionSetting class


  }//END DebugSetting class

}//END Evado.Dal namespace
