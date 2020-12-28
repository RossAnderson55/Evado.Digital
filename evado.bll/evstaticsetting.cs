using System;
using System.Collections.Generic;
using System.Text;

using Evado.Model;
using Evado.Model.Digital;

namespace Evado.Bll
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
    /// This property contains the connection string key for sql methods
    /// </summary>
    // ----------------------------------------------------------------------------------
    public static String EventLogSource
    {
      set
      {
        EvStaticSetting._eventLogSource = value;

        Evado.Dal.EvStaticSetting.EventLogSource = value;
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
      get { return EvStaticSetting._LoggingLevel; }
      set
      {
        EvStaticSetting._LoggingLevel = value;

        Evado.Dal.EvStaticSetting.LoggingLevel = EvStaticSetting._LoggingLevel;
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
      set
      {
        if ( value == true )
        {
          EvStaticSetting.LoggingLevel = 5;
        }
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

        Evado.Dal.EvStaticSetting.SiteGuid = value;
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

        Evado.Dal.EvStaticSetting.ConnectionStringKey = value;
      }
      get
      {
        return EvStaticSetting._connectionStringKey;
      }

    }//END setConnectionSetting class

    /// <summary>
    /// This method resets the connection string key to the default.
    /// </summary>
    public static void ResetConnectionString ( )
    {
      String defaultConnectionKey = "EVADO";

      if ( System.Configuration.ConfigurationManager.AppSettings [ "DefaultConnectionSetting" ] != null )
      {
        defaultConnectionKey = ( String ) System.Configuration.ConfigurationManager.AppSettings [ "DefaultConnectionSetting" ];
      }
      EvStaticSetting.ConnectionStringKey = defaultConnectionKey;
    }

  }//END DebugSetting class

}//END namespace Evado.Bll
