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
  public enum EdRecordTypes
  {
    /// <summary>
    /// This enumeratiion defines null value or not selection state. 
    /// </summary>
    Null = 0,

    /// <summary>
    /// This enumeration defines the trial record form type.
    /// </summary>
    Normal_Record ,

    /// <summary>
    /// This enumeration defines the updateable medical record form type.
    /// </summary>
    Updatable_Record,

    /// <summary>
    /// This enumeration defines a backward compatible 'Questionnaire'. 
    /// </summary>
    Questionnaire,

  }

}//END namespace Evado.Model.Digital
