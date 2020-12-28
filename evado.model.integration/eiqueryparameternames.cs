/***************************************************************************************
 * <copyright file="Evado.Model.Integration\EiQueryParameterNames.cs" company="EVADO HOLDING PTY. LTD.">
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
    /// This enumeration defines the web service query parameter types to filter the result set.
  /// </summary>
  [Serializable]
    public enum EiQueryParameterNames
    {
      /// <summary>
      /// This enumeration defines a null value of unset data QueryType.
      /// </summary>
      Null ,

      /// <summary>
      /// This enumeration defines a customer identifier parameter value.
      /// </summary>
      Customer_Id,

      /// <summary>
      /// This enumeration defines a project identifier parameter value.
      /// </summary>
      Project_Id,

      /// <summary>
      /// This enumeration defines a trial identifier parameter value.
      /// </summary>
      Organisation_Id,

      /// <summary>
      /// This enumeration defines a subject identifier parameter value.
      /// </summary>
      Subject_Id,

      /// <summary>
      /// This enumeration defines a screening identifier parameter value.
      /// </summary>
      Screening_Id,

      /// <summary>
      /// This enumeration defines a sponsor identifier parameter value.
      /// </summary>
      Sponsor_Id = 5,

      /// <summary>
      /// This enumeration defines a randomised identifier parameter value.
      /// </summary>
      Randomised_Id,

      /// <summary>
      /// This enumeration defines a external identifier parameter value.
      /// </summary>    
      External_Id,

      /// <summary>
      /// This enumeration defines a patient identifier parameter value.
      /// Privacy protected.
      /// </summary>    
      Primary_Id,

      /// <summary>
      /// This enumeration defines a visit identifier parameter value.
      /// </summary>    
      Visit_Id,

      /// <summary>
      /// This enumeration defines a schedule identifier parameter value.
      /// </summary>      
      Schedule_Id,

      /// <summary>
      /// This enumeration defines a activity identifier parameter value.
      /// </summary>
      Activity_Id,

      /// <summary>
      /// This enumeration defines a milestone identifier parameter value.
      /// </summary>    
      Milestone_Id,

      /// <summary>
      /// This enumeration defines a record identifier parameter value.
      /// </summary>
      Record_Id,

      /// <summary>
      /// This enumeration defines a record identifier parameter value.
      /// </summary>
      Form_Id,

      /// <summary>
      /// This enumeration defines a start date of the query period .
      /// </summary>
      Start_Date,

      /// <summary>
      /// This enumeration defines a end date of the query period.
      /// </summary>

      End_Date,

      /// <summary>
      /// This enumeration defines the length of the results that is to be returned.
      /// </summary>

      MaxListLength,

      /// <summary>
      /// This enumeration defines whether the result set is to include record field values.
      /// </summary>

      IncludeRecordFields,

      /// <summary>
      /// This enumeration defines whether the result set is to include record metadata.
      /// </summary>

      IncludeMetaData,

      /// <summary>
      /// This enumeration defines the record status to be selected in the query.
      /// </summary>
      RecordStatus, 

    /// <summary>
    /// This property defines the length of the result set
    /// </summary>
    SetResultsetLength

    }//END EiQueryParameterNames enumeration

}//END Namespace Evado.Model.Integration
