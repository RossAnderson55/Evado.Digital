/***************************************************************************************
 * <copyright file="model\EvMilestone.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvMilestone data object.
 *
 ****************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

namespace Evado.Model.Digital
{

  /// <summary>
  /// Business entity used to model accounts
  /// </summary>
  [Serializable]
  public class EvMilestoneData
  {
    #region Class properties

    private List<EvUserSignoff> _Signoffs = new List<EvUserSignoff> ( );
    /// <summary>
    /// This property contains a user signoff object list of milestone data
    /// </summary>
    public List<EvUserSignoff> Signoffs
    {
      get
      {
        return this._Signoffs;
      }
      set
      {
        this._Signoffs = value;
      }
    }

    private int _InitialScheduleVersion = 1;
    /// <summary>
    /// This property contains an initial schedule version of milestone data
    /// </summary>
    public int InitialScheduleVersion
    {
      get
      {
        return this._InitialScheduleVersion;
      }
      set
      {
        this._InitialScheduleVersion = value;
      }
    }

    /// <summary>
    ///  The version the milestone was created with.
    /// 
    /// </summary>
    private int _OptionalScheduleVersion = 1;
    /// <summary>
    /// This property contains an initial schedule version of milestone data
    /// </summary>
    public int OptionalScheduleVersion
    {
      get
      {
        return this._OptionalScheduleVersion;
      }
      set
      {
        this._OptionalScheduleVersion = value;
      }
    }

    private int _CurrentVersion = 1;
    /// <summary>
    /// This property contains a current version of milestone data
    /// </summary>
    public int CurrentVersion
    {
      get
      {
        return this._CurrentVersion;
      }
      set
      {
        this._CurrentVersion = value;
      }
    }

    private DateTime _PreviousVisitDate = EvcStatics.CONST_DATE_NULL;
    /// <summary>
    /// This property contains the previous visit date.
    /// </summary>
    public DateTime PreviousVisitDate
    {
      get { return _PreviousVisitDate; }
      set { _PreviousVisitDate = value; }
    }

    #endregion

    #region Public class

    #endregion

  }//END EvMilestoneData class

}//END namespace Evado.Model
