/***************************************************************************************
 * <copyright file="EvFormFieldValidationRules.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvFormFieldValidationRules data object.
 *
 ****************************************************************************************/

using System;
using System.Collections; using System.Collections.Generic;

namespace Evado.Model.Digital
{
  /// 
  /// Business entity used to model EvFormField
  /// 
  [Serializable]
  public class EvFormFieldValidationRules
  {

    #region Internal member variables
    private bool _NotValidForMale = false;
    private bool _NotValidForFemale = false;

    private EvFormFieldValidationNotValid _NotValid = new EvFormFieldValidationNotValid ( );
    private EvFormFieldValidationNotValid _NotValidOptions = new EvFormFieldValidationNotValid ( );

    private float _ValidationLowerLimit = EvcStatics.CONST_NUMERIC_MINIMUM;
    private float _ValidationUpperLimit = EvcStatics.CONST_NUMERIC_MAXIMUM;
    private float _AlertLowerLimit = EvcStatics.CONST_NUMERIC_MINIMUM;
    private float _AlertUpperLimit = EvcStatics.CONST_NUMERIC_MAXIMUM;
    private float _NormalRangeLowerLimit = EvcStatics.CONST_NUMERIC_MINIMUM;
    private float _NormalRangeUpperLimit = EvcStatics.CONST_NUMERIC_MAXIMUM;
    private int _WithinDaysOfRecordDate = 365;
    private int _WithinDaysOfVisitDate = 365;

    private bool _IsAfterBirthDate = false;
    private bool _IsAfterConsentDate = false;

    #endregion

    #region Properties

    /// <summary>
    /// This property indicates whether the date validation rules value after DOB
    /// </summary>
    public bool IsAfterBirthDate
    {
      get { return this._IsAfterBirthDate; }
      set { this._IsAfterBirthDate = value; }
    }

    /// <summary>
    /// This property indicates whether the date validation rules value after consent date
    /// </summary>
    public bool IsAfterConsentDate
    {
      get { return this._IsAfterConsentDate; }
      set { this._IsAfterConsentDate = value; }
    }

    /// <summary>
    /// This property indicates whether the validation rules are not valid for male
    /// </summary>
    public bool NotValidForMale
    {
      get { return this._NotValidForMale; }
      set { this._NotValidForMale = value; }
    }

    /// <summary>
    /// This property indicates whether the validation rules are not valid for female
    /// </summary>
    public bool NotValidForFemale
    {
      get { return this._NotValidForFemale; }
      set { this._NotValidForFemale = value; }
    }

    /// <summary>
    /// This property contains the not valid object for the validation rules
    /// </summary>
    public EvFormFieldValidationNotValid NotValid
    {
      get { return this._NotValid; }
      set { this._NotValid = value; }
    }

    /// <summary>
    /// This property contains the numeric validation lower limit.
    /// </summary>
    public float ValidationLowerLimit
    {
      get { return this._ValidationLowerLimit; }
      set { this._ValidationLowerLimit = value; }
    }

    /// <summary>
    /// This property contains the numeric validation upper limit
    /// </summary>
    public float ValidationUpperLimit
    {
      get { return this._ValidationUpperLimit; }
      set { this._ValidationUpperLimit = value; }
    }

    /// <summary>
    /// This property contains the numeric alert lower limit.
    /// </summary>
    public float AlertLowerLimit
    {
      get { return this._AlertLowerLimit; }
      set { this._AlertLowerLimit = value; }
    }

    /// <summary>
    /// This property contains the numeric alert upper limit
    /// </summary>
    public float AlertUpperLimit
    {
      get { return this._AlertUpperLimit; }
      set { this._AlertUpperLimit = value; }
    }

    /// <summary>
    /// This property contains the numeric normal lower limit.
    /// </summary>
    public float NormalRangeLowerLimit
    {
      get { return this._NormalRangeLowerLimit; }
      set { this._NormalRangeLowerLimit = value; }
    }

    /// <summary>
    /// This property contains the numeric normal upper limit
    /// </summary>
    public float NormalRangeUpperLimit
    {
      get { return this._NormalRangeUpperLimit; }
      set { this._NormalRangeUpperLimit = value; }
    }
    /// <summary>
    /// This property contains the days within the record data validation
    /// </summary>
    public int WithinDaysOfRecordDate
    {
      get { return this._WithinDaysOfRecordDate; }
      set { this._WithinDaysOfRecordDate = value; }
    }

    /// <summary>
    /// This property contains the days within the visit data validation
    /// </summary>
    public int WithinDaysOfVisitDate
    {
      get { return this._WithinDaysOfVisitDate; }
      set { this._WithinDaysOfVisitDate = value; }
    }

    /// <summary>
    /// This property contains the option object for the validation rules
    /// </summary>
    public EvFormFieldValidationNotValid NotValidOptions
    {
      get { return this._NotValidOptions; }
      set { this._NotValidOptions = value; }
    }
    #endregion

    #region methods

    /// <summary>
    /// This method reset the validation numeric into the default value
    /// </summary>
    public void resetDefaults ( )
    {
      this.ValidationLowerLimit = EvcStatics.CONST_NUMERIC_MINIMUM;
      this.ValidationUpperLimit = EvcStatics.CONST_NUMERIC_MAXIMUM;
      this.AlertLowerLimit = EvcStatics.CONST_NUMERIC_MINIMUM;
      this.AlertUpperLimit = EvcStatics.CONST_NUMERIC_MAXIMUM;
      this.AlertLowerLimit = EvcStatics.CONST_NUMERIC_MINIMUM;
      this.AlertUpperLimit = EvcStatics.CONST_NUMERIC_MAXIMUM;
      this.NormalRangeLowerLimit = EvcStatics.CONST_NUMERIC_MINIMUM;
      this.NormalRangeUpperLimit = EvcStatics.CONST_NUMERIC_MAXIMUM;
    }

    #endregion

  }//END EvFormFieldValidationRules class 

}//END namespace Evado.Model.Digital
