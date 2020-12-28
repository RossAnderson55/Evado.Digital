/***************************************************************************************
 * <copyright file="ApplicationEvent.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2020 EVADO HOLDING PTY. LTD..  All rights reserved.
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
 *  This class contains the ApplicationEvent data object.
 *
 ****************************************************************************************/

using System;
using System.Collections.Generic;

namespace Evado.Model
{
  /// <summary>
  /// This data class defines the Application Event object content
  /// </summary>
  [Serializable]
  public class EvApplicationEvent
  {
    #region class initialisation methods

    /// <summary>
    /// Default constructor
    /// </summary>
    public EvApplicationEvent ( )
    {
    }

    //====================================================================================
    /// <summary>
    /// Default constructor and initlaise the object.
    /// </summary>
    /// <param name="Type">EventType enumerated value</param>
    /// <param name="EventId">EvEventCodes enumerated value</param>
    /// <param name="Category">String: the category of the event</param>
    /// <param name="Description">String: the description of the event.</param>
    /// <param name="UserName">String: name of the user that created the event.</param>
    // ----------------------------------------------------------------------------------
    public EvApplicationEvent (
      EventType Type,
      EvEventCodes EventId,
      String Category,
      String Description,
      String UserName )
    {
      this.EventId = (int) EventId;
      this.Type = Type;
      this.Category = Category;
      this.Description = Description;
      this.UserName = UserName;
    }
    #endregion

    #region Class enumerations

    /// <summary>
    /// Class Application Event type enumeration list.
    /// </summary>
    public enum EventType
    {
      /// <summary>
      /// This enumeration value defines event type null value
      /// </summary>
      Null,

      /// <summary>
      /// The enumeration value defines event type action
      /// </summary>
      Action,

      /// <summary>
      /// The enumeration value defines event type information
      /// </summary>
      Information,

      /// <summary>
      /// The enumeration value defines event type warning
      /// </summary>
      Warning,

      /// <summary>
      /// The enumeration value defines event type error
      /// </summary>
      Error,
    }

    #endregion

    #region Internal Members
    // 
    // Internal member variables
    // 
    private string _Uid = "0";
    private Guid _Guid = Guid.Empty;
    private int _EventId = 0;
    private DateTime _DateTime = DateTime.Parse ( "01 Jan 190" );
    private EventType _Type = EventType.Null;
    private string _Category = String.Empty;
    private string _UserName = String.Empty;
    private string _Description = String.Empty;
    private string _PageUrl = String.Empty;
    private string _CustomerId = String.Empty;
    private string _EventName = String.Empty;
    private string _EventDescription = String.Empty;
    private string _TypeDescription = String.Empty;

    #endregion

    #region Class  Properties Section


    /// <summary>
    /// This property defines application event's unique integer Identification
    /// </summary>
    public Guid Guid
    {
      get
      {
        return _Guid;
      }
      set
      {
        _Guid = value;
      }
    }
    /// <summary>
    /// This property defines application event's unique integer Identification
    /// </summary>
    public string Uid
    {
      get
      {
        return _Uid.PadLeft ( 7, '0' );
      }
      set
      {
        _Uid = value;
      }
    }

    /// <summary>
    /// This property defines application event identifier
    /// </summary>
    public int EventId
    {
      get
      {
        return _EventId;
      }
      set
      {
        _EventId = value;
      }
    }

    /// <summary>
    /// This property contains date time stamp of the Application Event 
    /// </summary>
    public DateTime DateTime
    {
      get
      {
        return _DateTime;
      }
      set
      {
        _DateTime = value;
      }
    }

    /// <summary>
    /// This property contains date of the Application Event
    /// </summary>
    public string Date
    {
      get
      {
        return _DateTime.ToString ( "dd MMM yyyy" );
      }
    }

    /// <summary>
    /// This property contains time of the Application Event
    /// </summary>
    public string Time
    {
      get
      {
        return _DateTime.ToString ( "HH:mm:ss" );
      }
    }

    /// <summary>
    /// This property contains event type of the Application Event
    /// </summary>
    public EventType Type
    {
      get
      {
        return _Type;
      }
      set
      {
        _Type = value;
      }
    }

    /// <summary>
    /// This property contains category of the Application Event
    /// </summary>
    public string Category
    {
      get
      {
        return _Category;
      }
      set
      {
        _Category = value;
      }
    }

    /// <summary>
    /// This property contains user name of the person that generated the Application Event
    /// </summary>
    public string UserName
    {
      get
      {
        return _UserName;
      }
      set
      {
        _UserName = value;
      }
    }

    /// <summary>
    /// This property contains event description of Application Event
    /// </summary>
    public string Description
    {
      get
      {
        return _Description;
      }
      set
      {
        _Description = value;
      }
    }

    /// <summary>
    /// This property contains summary of Application Event
    /// </summary>
    public string Summary
    {
      get
      {
        string sSummary = String.Empty;
        if ( _Description.Length > 200 )
        {
          sSummary = _Description.Substring ( 0, 200 );
        }
        else
        {
          sSummary = _Description;
        }
        return sSummary.Replace ( "\r\n", "<br/>" );
      }
    }

    /// <summary>
    /// This property contains page url of the Application Event
    /// </summary>
    public string PageUrl
    {
      get
      {
        return _PageUrl;
      }
      set
      {
        _PageUrl = value;
      }
    }

    /// <summary>
    /// This property contains site of the Application Event
    /// </summary>
    public string CustomerId
    {
      get
      {
        return _CustomerId;
      }
      set
      {
        _CustomerId = value;
      }
    }

    /// <summary>
    /// This property defines name of the Application Event
    /// </summary>
    public string EventName
    {
      get
      {
        return _EventName;
      }
      set
      {
        _EventName = value;
      }
    }

    /// <summary>
    /// This property contains type description of the Application Event
    /// </summary>
    public string TypeDescription
    {
      get
      {
        return _TypeDescription;
      }
      set
      {
        _TypeDescription = value;
      }
    }

    /// <summary>
    /// This property contains the content displayed in a list of ApplicationEvents.
    /// </summary>
    public String LinkText
    {
      get
      {

        String description = this._Description;
        EvEventCodes code = EvEventCodes.Ok;

        if ( description.Length > 80 )
        {
          description = this._Description.Substring ( 0, 80 );
        }

        if ( this._EventId < 0 )
        {
          try
          {
            code = (EvEventCodes) this._EventId;
          }
          catch
          {
            code = EvEventCodes.Ok;
          }
        }

        String stContent = EvStatics.Enumerations.enumValueToString ( code )
          + " > "
          + this._DateTime.ToString ( "dd MMM yyyy HH:mm" )
          + " >> "
          + description + " ...";

        if ( this._UserName != String.Empty
          && stContent.Contains ( this._UserName ) == false )
        {
          stContent += EvLabels.Space_Open_Bracket
          + this._UserName
          + EvLabels.Space_Close_Bracket;
        }

        stContent = stContent.Replace ( "\r\n", " " );

        return stContent;
      }
    }

    #endregion

    #region public static  methods

    #endregion

  } // Close class ApplicationEvent

} // Close namespace Evado.Model
