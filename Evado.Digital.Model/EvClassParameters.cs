﻿/***************************************************************************************
 * <copyright file="Evado.Model.Digital\EvApplicationSetting.cs" company="EVADO HOLDING PTY. LTD.">
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
 * Description: 
 *  This class contains the EvCaseReportForms business object.
 *
 ****************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

using Evado.Model;

namespace Evado.Digital.Model
{
  /// <summary>
  /// This class manages the setting of the connection string key to be used by the 
  /// SQL method class to access the database.
  /// </summary>
  public class EvClassParameters
  {

    #region class initialisation methods.
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EvClassParameters ( )
    {
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EvClassParameters (
      EdUserProfile UserProfile,
      int Logginglevel,
      Guid ApplicationGuid )
    {
      this._LoggingLevel = Logginglevel;
      this.AdapterGuid = ApplicationGuid;
      this.UserProfile = UserProfile;

      String CustomerGroup = UserProfile.AdsCustomerGroup;

    }//END initialisation method.

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion


    private int _MaxResultLength = 1000;
    /// <summary>
    /// This property contains the method debug log. 
    /// </summary>
    public int MaxResultLength
    {
      get
      {
        return _MaxResultLength;
      }
      set
      {
        this._MaxResultLength = value;
      }
    }

    #region Logging Level property
    private int _LoggingLevel = 0;

    /// <summary>
    /// This property defines the logging level of the application.
    /// </summary>
    public int LoggingLevel
    {
      get { return this._LoggingLevel; }
      set
      {
        this._LoggingLevel = value;
      }
    }

    // ================================================================================
    /// <summary>
    /// This property contains the connection string key for sql methods
    /// </summary>
    // ----------------------------------------------------------------------------------
    public bool DebugOn
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
          _LoggingLevel = 5;
        }
      }

    }//END DebugOn

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Application identifier property

    private Guid _AdapterGuid = Guid.Empty;

    // ================================================================================
    /// <summary>
    /// This property contains the site guid 
    /// </summary>
    // ----------------------------------------------------------------------------------
    public Guid AdapterGuid
    {
      get
      {
        return this._AdapterGuid;
      }
      set
      {
        this._AdapterGuid = value;
      }
    }

    private Guid _CustomerGuid = Guid.Empty;

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region User properties

    /// <summary>
    /// This property contains the user common name 
    /// </summary>
    public EdUserProfile UserProfile { get; set; }

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Customer Id parameter
    private String _CustomerId = String.Empty;

    // ================================================================================
    /// <summary>
    /// This property contains the CustomerId for sql methods
    /// </summary>
    // ----------------------------------------------------------------------------------
    public String CustomerId
    {
      set
      {
        this._CustomerId = value;
      }
      get
      {
        return this._CustomerId;
      }

    }//END setConnectionSetting class

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region PatformId parameter

    private String _PlatformId = String.Empty;

    // ================================================================================
    /// <summary>
    /// This property contains the connection string key for sql methods
    /// </summary>
    // ----------------------------------------------------------------------------------
    public String PlatformId
    {
      set
      {
        this._PlatformId = value;
      }
      get
      {
        return this._PlatformId;
      }

    }//END setConnectionSetting class

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    //===================================================================================
    /// <summary>
    /// this returns the setting object values
    /// </summary>
    //----------------------------------------------------------------------------------
    public String getValues ( )
    {
      StringBuilder value = new StringBuilder();

      try
      {
        value.AppendLine ( "LoggingLevel: " + this._LoggingLevel );
        value.AppendLine ( "ApplicationGuid: " + this._CustomerGuid );
        value.AppendLine ( "UserId: " + this.UserProfile.UserId );
        value.AppendLine ( "UserProfile.CommonName: " + this.UserProfile.CommonName);
        value.AppendLine ( "UserProfile.RoleId: " + this.UserProfile.Roles );
        value.AppendLine ( "CustomerId: " + this.CustomerId );
        value.AppendLine ( "PlatformId: " + this._PlatformId );
      }
      catch
      {
        value.AppendLine ( "Null Values" );
      }
      return value.ToString();
    }


  }//END Setting class

}//END namespace Evado.Bll
