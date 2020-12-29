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
  /// This enumeration list defines the types of form record.
  /// </summary>
  public enum EvFormRecordTypes
  {
    /// <summary>
    /// This enumeratiion defines null value or not selection state. 
    /// </summary>
    Null = 0,

    /// <summary>
    /// This enumeration defines the subject record form type.
    /// </summary>
    Subject_Record = 1,

    /// <summary>
    /// This enumeration defines the adverse event report form type.
    /// </summary>
    Adverse_Event_Report = 2,

    /// <summary>
    /// This enumeration defines the concomitant medication record form type.
    /// </summary>
    Concomitant_Medication = 3,

    /// <summary>
    /// This enumeration defines the Seriouse Adverse Event report form type.
    /// </summary>
    Serious_Adverse_Event_Report = 5,

    /// <summary>
    /// This enumeration defines the periodic followup records form type.
    /// </summary>
    Periodic_Followup = 6,

    /// <summary>
    /// This enumeration defines the assessment record form type.
    /// </summary>
    Assessment_Record = 7,

    /// <summary>
    /// This enumeration defines the trial record form type.
    /// </summary>
    Normal_Record = 8,

    /// <summary>
    /// This enumeration defines the updateable medical record form type.
    /// </summary>
    Updatable_Record = 9,

    /// <summary>
    /// This enumeration defines a backward compatible 'Questionnaire'. 
    /// </summary>
    Questionnaire = 10,

    /// <summary>
    /// This enumeration defines the protocol exception form type (Common form).
    /// </summary>
    Protocol_Exception = 11,

    /// <summary>
    /// This enumeration defines the protocol variation form type (Common form).
    /// </summary>
    Protocol_Variation = 12,

    /// <summary>
    /// This enumeration defines the patient consent  first consent form type (Common form).
    /// </summary>
    Informed_Consent = 13,

    /// <summary>
    /// This enumeration defines the patient consent form type (Common form).
    /// </summary>
    Patient_Record = 14,

    /// <summary>
    /// This enumeration defines the patient recorded outcomes record type.
    /// </summary>
    Patient_Recorded_Outcomes = 10,
  }

}//END namespace Evado.Model.Digital
