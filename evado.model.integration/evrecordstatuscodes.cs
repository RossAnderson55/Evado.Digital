/***************************************************************************************
 * <copyright file="Evado.Model.Integration\EvRecordStatusCodes.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2020 EVADO HOLDING PTY. LTD..  All rights reserved.
 *     
 *      The use and distribution terms for this software are contained in the file
 *      named license.txt, which can be found in the root of this distribution.
 *      By using this software in any fashion, you are agreeing to be bound by the
 *      terms of this license.
 *     
 *      You must not remove this notice, or any other, from this software.
 *     
 * </copyright>
 * 
 * Description: 
 *  This class contains the EvCaseReportForms business object.
 *
 ****************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evado.Model.Integration
{
    /// <summary>
    /// This enumeration defines the record state to be queried.
    ///  
    /// This parameter is used when querying, records, adverse events, 
    /// concomitant medications, serious adverse events. 
  /// </summary>
  [Serializable]
    public enum EvRecordStatusCodes
    {
      /// <summary>
      /// This enumeration defines a null value of unset query QueryType.
      /// </summary>
      Null = 0,

      /// <summary>
      /// This enumeration defines ths selection includes all records that are not withdrawn or are a queried copy.
      /// </summary>
      Default = 1,

      /// <summary>
      /// This enumeration defines ths selection include all records that have been submitted as a completed record.
      /// </summary>
      Submitted = 2,

      /// <summary>
      /// This enumeration defines ths selection includes all records that have been source data verified.
      /// </summary>
      Source_Data_Verified = 3,

      /// <summary>
      /// This enumeration defines ths selection includes all records that have been locked by the data manager..
      /// </summary>
      Locked_Records = 4,

      /// <summary>
      /// This enumeration defines ths selection includes all records that have been withdrawn, (logically deleted).
      /// </summary>
      Withdrawn = 5,

    }//END EvRecordStatusCodes enumeration.

}//END Namespace Evado.Model.Integration
