/***************************************************************************************
 * <copyright file="EvMenuItem.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvMenuItem data object.
 *
 ****************************************************************************************/

using System;
using System.Collections.Generic;

namespace Evado.Model.Digital
{
  /// <summary>
  /// This enumeration list defines the states of module code.
  /// </summary>
  [Serializable]
  public enum EvModuleCodes
  {
    /// <summary>
    /// This enumeration selects All modules
    /// </summary>
    All_Modules = 0,

    /// <summary>
    /// This enumeration selects administration module.
    /// </summary>
    Administration_Module = 1,

    /// <summary>
    /// This enumeration selects the trial module.
    /// </summary>
    Trial_Module = 2,

    /// <summary>
    /// This enumeration selects the registry module.
    /// </summary>
    Registry_Module = 3,

    /// <summary>
    /// This enumeration selects the Management module.
    /// </summary>
    Management_Module = 4,

    /// <summary>
    /// This enumeration selects the Patient module.
    /// </summary>
    Patient_Module = 5,

    /// <summary>
    /// The Clinical Outcome Assessment module enumeration
    /// </summary>
    Clinical_Outcome_Assessments = 6,

    /// <summary>
    /// This enumeration selects the imaging module.
    /// </summary>
    Imaging_Module = 7,

    /// <summary>
    /// This enumeration selects the integration module.
    /// </summary>
    Integration_Module = 8,

    /// <summary>
    /// This enumeration selects the Electronic Consent module.
    /// </summary>
    Informed_Consent = 9,

    /// <summary>
    /// This enumeration selects the Auxiliary subject data module.
    /// </summary>
    Auxiliary_Subject_Data = 10,

    //
    // dreciated enumeration options.
    //
    /// <summary>
    /// This enumeration selects the depreciated clinical module.
    /// </summary>
    Clinical_Module = 2,

    /// <summary>
    /// This enumeration defines the null selectoin 
    /// </summary>
    Null = 99,

    // ******************************************************
    // Decpresicated values.

    /// <summary>
    /// The Patient Recorded Outcomes module enumeration
    /// </summary>
    Patient_Recorded_Outcomes = 6,

    /// <summary>
    /// The Patient Recorded Observation module enumeration
    /// </summary>
    Patient_Recorded_Observation = 11,
  }

}//END Namespace Evado.Model
