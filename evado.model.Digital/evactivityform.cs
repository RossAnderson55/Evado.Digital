/***************************************************************************************
 * <copyright file="model\EvOption.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvOption data object.
 *
 ****************************************************************************************/


using System;

namespace Evado.Model.Digital
{

  /// <summary>
  /// Business entity used to model accounts
  /// </summary>
  [Serializable]
  public class EvActvityForm
  {
    #region enumertion 

    #endregion

    #region internal variables
    //  Internal member variables
    private Guid _Guid = Guid.Empty;
    private int _ActivityUid = 0;
    private Guid _ActivityGuid = Guid.Empty;
    private string _ActivityId = String.Empty;
    private string _FormId = String.Empty;
    private string _FormTitle = String.Empty;
    private int _Order = 0;
    private bool _Mandatory = false;
    private bool _Selected = false;
    private bool _ActivityMandatory = false;

    // Intermediate business logic Selected
    private int _InitialVersion = 1;
    private string _ProjectId = String.Empty;
    #endregion

    #region Properties

    /// <summary>
    /// This property contains an activity record's global unique identifier
    /// </summary>
    public Guid Guid
    {
      get
      {
        return this._Guid;
      }
      set
      {
        this._Guid = value;
      }
    }

    /// <summary>
    /// This property contains an activity record's activity unique identifier
    /// </summary>
    public int ActivityUid
    {
      get
      {
        return _ActivityUid;
      }
      set
      {
        _ActivityUid = value;
      }
    }

    /// <summary>
    /// This property contains an activity global unique identifier of activity record
    /// </summary>
    public Guid ActivityGuid
    {
      get
      {
        return this._ActivityGuid;
      }
      set
      {
        this._ActivityGuid = value;
      }
    }

    /// <summary>
    /// This property contains an activity record's trial identifier
    /// </summary>
    public string ProjectId
    {
      get
      {
        return this._ProjectId;
      }
      set
      {
        this._ProjectId = value;
      }
    }

    /// <summary>
    /// This property contains an activity unique identifier of the activity record
    /// </summary>
    public string ActivityId
    {
      get
      {
        return this._ActivityId;
      }
      set
      {
        this._ActivityId = value;
      }
    }

    /// <summary>
    /// This property contains an form identifier of the activity record
    /// </summary>
    public string FormId
    {
      get
      {
        return this._FormId;
      }
      set
      {
        this._FormId = value;
      }
    }

    /// <summary>
    /// This property contains an form title of the activity record
    /// </summary>
    public string FormTitle
    {
      get
      {
        return this._FormTitle;
      }
      set
      {
        this._FormTitle = value;
      }
    }

    /// <summary>
    /// This property contains an order of the activity record
    /// </summary>
    public int Order
    {
      get
      {
        return this._Order;
      }
      set
      {
        this._Order = value;
      }
    }

    /// <summary>
    /// This property indicates a mandatory activity of the actvity record
    /// </summary>
    public bool ActivityMandatory
    {
      get
      {
        return this._ActivityMandatory;
      }
      set
      {
        this._ActivityMandatory = value;
      }
    }

    /// <summary>
    /// This property indicates whether an activity record is mandatory
    /// </summary>
    public bool Mandatory
    {
      get
      {
        return this._Mandatory;
      }
      set
      {
        this._Mandatory = value;
      }
    }

    /// <summary>
    /// This property returns the mandatory display of the activity record
    /// </summary>
    public string MandatoryDisp
    {
      get
      {
        string Disp = "No";
        if ( this._Mandatory == true )
        {
          Disp = "Yes";
        }
        return Disp;
      }
    }

    /// <summary>
    /// This property indicates whether the activity record is selected
    /// </summary>
    public bool Selected
    {
      get
      {
        return this._Selected;
      }
      set
      {
        this._Selected = value;
      }
    }

    /// <summary>
    /// This property contains an initial version of the activity record
    /// </summary>
    public int InitialVersion
    {
      get
      {
        return this._InitialVersion;
      }
      set
      {
        this._InitialVersion = value;
      }
    }

    #endregion

  }//END EvActivityForm method

}//END namespace Evado.Model
