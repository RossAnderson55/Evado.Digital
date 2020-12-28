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
    //  ===================================================================================
    /// <summary>
    /// This enumeration defines the form object states a form or record can have
    /// </summary>
    //  ----------------------------------------------------------------------------------- 
    public enum EvFormObjectStates
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

      /// <summary>
      /// This enumeration defines the record is queried.
      /// </summary>
      Queried_Record = 9,

      /// <summary>
      /// This enumeration defines the record is a source verified.
      /// </summary>
      Source_Data_Verified = 10,

      /// <summary>
      /// This enumeration defines the record is a completed and signed off by DM.
      /// </summary>
      Locked_Record = 11,

      /// <summary>
      /// This enumeration defines the a copy of a queried record.
      /// </summary>
      Queried_Record_Copy = 12,

    }

}//END namespace Evado.Model.Digital
