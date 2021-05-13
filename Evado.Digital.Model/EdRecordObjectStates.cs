/***************************************************************************************
 * <copyright file="model\EvForm.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2021 EVADO HOLDING PTY. LTD..  All rights reserved.
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

namespace Evado.Digital.Model
{
    //  ===================================================================================
    /// <summary>
    /// This enumeration defines the form object states a form or record can have
    /// </summary>
    //  ----------------------------------------------------------------------------------- 
    public enum EdRecordObjectStates
    {
      /// <summary>
      /// This enumeration value defines the null or not set status value.
      /// </summary>
      Null = 0,

      /// <summary>
      /// This enumeration defines the form draft state.
      /// </summary>
      Form_Draft = 1,

      /// <summary>
      /// This enumeration defines the form reviewed state.
      /// </summary>
      Form_Reviewed = 2,
      /// <summary>
      /// 
      /// This enumeration defines the form issued state.
      /// </summary>
      Form_Issued = 3,

      /// <summary>
      /// This enumeration defines the form withdrawn or form records cancelled state.
      /// </summary>
      Withdrawn = 4,

      /// <summary>
      /// This enumeration defines the record is a Empty.
      /// </summary>
      Empty_Record = 5,

      /// <summary>
      /// This enumeration defines the record is a draft.
      /// </summary>
      Draft_Record = 6,

      /// <summary>
      /// This enumeration defines the record is a draft.
      /// </summary>
      Completed_Record = 7,

      /// <summary>
      /// This enumeration defines the record is submitted as completed record.
      /// </summary>
      Submitted_Record = 8,
    }

}//END namespace Evado.Digital.Model
