/***************************************************************************************
 * <copyright file="Evado.Model.Integration\EiQueryTypes.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c)  2002 - 2021  EVADO HOLDING PTY. LTD..  All rights reserved.
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
    ///  this enumeration defines the types of query that are to be executed.
  /// </summary>
  [Serializable]
    public enum EiQueryTypes
    {
      /// <summary>
      /// This enumeration defines a null value of unset query QueryType.
      /// </summary>
      Null = 0,

      /// <summary>
      /// This enumeration defines that the web query is selecting ProjectRecord template.
      /// </summary>
      Layout_Template,

      /// <summary>
      /// This enumeration defines that the web query is selecting project (visit) records objects.
      /// </summary>
      Layout_Export,

      /// <summary>
      /// This enumeration defines that the web query is selecting importing project (visit) record data.
      /// </summary> 
      Layout_Import,

      /// <summary>
      /// This enumeration defines that the web query is selecting ProjectRecord template.
      /// </summary>
      Record_Template,

      /// <summary>
      /// This enumeration defines that the web query is selecting project (visit) records objects.
      /// </summary>
      Records_Export,

      /// <summary>
      /// This enumeration defines that the web query is selecting importing project (visit) record data.
      /// </summary> 
      Records_Import,

      /// <summary>
      /// This enumeration defines that the webe query is executing a report.
      /// </summary>
      Project_Report,
     

    }//END EvQueryTypes enumeration.

}//END Namespace Evado.Model.Integration
