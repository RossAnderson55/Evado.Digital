/***************************************************************************************
 * <copyright file="model\EvForm.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvForm data object.
 *
 ****************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

namespace Evado.Model.Digital
{
  /// <summary>
  /// This class defines the evado form object.
  /// </summary>
  [Serializable]
  public class EdRecordEntity
    {


    /// <summary>
    /// This property contains a global unique identifier of a Customer . 
      /// </summary>

      private Guid _RecordLayoutGuid = Guid.Empty;
      /// <summary>
      /// This property contains the record GUID
      /// </summary>
      public Guid RecordLayoutGuid
      {
        get
        {
          return this._RecordLayoutGuid;
        }
        set
        {
          this._RecordLayoutGuid = value;
        }
      }

    private Guid _RecordGuid = Guid.Empty;
    /// <summary>
    /// This property contains the record GUID
    /// </summary>
    public Guid RecordGuid
    {
      get
      {
        return this._RecordGuid;
      }
      set
      {
        this._RecordGuid = value;
      }
    }

    private Guid _EntityGuid = Guid.Empty;
    /// <summary>
    /// This property contains the entity GUID
    /// </summary>
    public Guid EntityGuid
    {
      get
      {
        return this._EntityGuid;
      }
      set
      {
        this._EntityGuid = value;
      }
    }

    private string _EntityLayoutId = String.Empty;
    /// <summary>
    /// This property contains an entity layout identifier
    /// </summary>
    public string EntityLayoutId
    {
      get
      {
        return this._EntityLayoutId;
      }
      set
      {
        this._EntityLayoutId = value;
      }
    }

    private string _EntityTitle = String.Empty;
    /// <summary>
    /// This property contains the entity layout title
    /// </summary>
    public string EntityTitle
    {
      get
      {
        return this._EntityTitle;
      }
      set
      {
        this._EntityTitle = value;
      }
    }

    private String _EntityId = String.Empty;
    /// <summary>
    /// This property contains a application identifier of a form. 
    /// </summary>
    public string EntityId
    {
      get
      {
        return this._EntityId;
      }
      set
      {
        this._EntityId = value;
      }
    }

    }//END class EdRecordEntity

}//END namespace Evado.Model.Digital
