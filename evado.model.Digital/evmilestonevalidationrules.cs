/***************************************************************************************
 * <copyright file="EvMilestoneValidationRules.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvMilestoneValidationRules data object.
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
  public class EvMilestoneValidationRules
  {

    private bool _VisitLaterThanConsentDate = false;
    /// <summary>
    /// This property indicates whether the visit is later than the consent date
    /// </summary>
    public bool VisitLaterThanConsentDate
    {
      get
      {
        return this._VisitLaterThanConsentDate;
      }
      set
      {
        this._VisitLaterThanConsentDate = value;
      }
    }

    private int _MinimumDaysFromPreviousVisit = EvMilestone.CONST_MINIMUM_VISIT_PERIOD;
    /// <summary>
    /// This property contains minimum days from previous visit.
    /// </summary>
    public int MinimumDaysFromPreviousVisit
    {
      get
      {
        return this._MinimumDaysFromPreviousVisit;
      }
      set
      {
        this._MinimumDaysFromPreviousVisit = value;
      }
    }

    private int _MaximumDaysFromPreviousVisit = EvMilestone.CONST_MAXIMUM_VISIT_PERIOD;
    /// <summary>
    /// This property contains maximum days from previous visit.
    /// </summary>
    public int MaximumDaysFromPreviousVisit
    {
      get
      {
        return this._MaximumDaysFromPreviousVisit;
      }
      set
      {
        this._MaximumDaysFromPreviousVisit = value;

        if ( this._MaximumDaysFromPreviousVisit > EvMilestone.CONST_MAXIMUM_VISIT_PERIOD )
        {
          this._MaximumDaysFromPreviousVisit = EvMilestone.CONST_MAXIMUM_VISIT_PERIOD;
        }
      }
    }

  }//END EvMilestoneValidationRules class

}//END namespace Evado.Model.Digital
