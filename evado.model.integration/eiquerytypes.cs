/***************************************************************************************
 * <copyright file="Evado.Model.Integration\EiQueryTypes.cs" company="EVADO HOLDING PTY. LTD.">
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
      /// This enumeration defines that the web query is selecting Patient template.
      /// </summary>
      Activity_Template,

      /// <summary>
      /// This enumeration defines that the web query is selecting importing activities data.
      /// </summary> 
      Activities_Import,

      /// <summary>
      /// This enumeration defines that the web query is selecting exprting activity objects.
      /// </summary>
      Activities_Export,

      /// <summary>
      /// This enumeration defines that the web query is selecting importing subject data.
      /// </summary> 
      Budget_Template,

      /// <summary>
      /// This enumeration defines that the web query is selecting exporting budget data.
      /// </summary> 
      Budget_Export,

      /// <summary>
      /// This enumeration defines that the web query is selecting importing subject data.
      /// </summary> 
      Budget_Import,

      /// <summary>
      /// This enumeration defines that the web query is selecting CommonRecord template.
      /// </summary>
      Common_Record_Template,

      /// <summary>
      /// This enumeration defines that the web query is selecting importing common record data.
      /// </summary> 
      Common_Records_Import,

      /// <summary>
      /// This enumeration defines that the web query is selecting exporting common form objects.
      /// </summary>
      Common_Forms_Export,

      /// <summary>
      /// This enumeration defines that the web query is selecting exporting adverse event objects.
      /// </summary>
      Adverse_Events_Export,

      /// <summary>
      /// This enumeration defines that the web query is selecting concomitant medication objects.
      /// </summary>
      Comcomitant_Medications_Export,

      /// <summary>
      /// This enumeration defines that the web query is selecting exporting serious adverse event objects.
      /// </summary>

      Serious_Adverse_Events_Export,

      /// <summary>
      /// This enumeration defines that the web query is selecting patient template data.
      /// </summary> 
      Patient_Template,

      /// <summary>
      /// This enumeration defines that the web query is selecting importing patient data.
      /// </summary> 
      Patients_Import,

      /// <summary>
      /// This enumeration defines that the web query is selecting export patient data.
      /// </summary> 
      Patients_Export,

      /// <summary>
      /// This enumeration defines that the web query is selecting export patient consent objects.
      /// </summary>
      Patient_Consent_Export,

      /// <summary>
      /// This enumeration defines that the web query is ex[prtomg import patient consent objects.
      /// </summary>
      Patient_Consent_Import,

      /// <summary>
      /// This enumeration defines that the web query is selecting patient consent template data.
      /// </summary> 
      Patient_Consent_Template,

      /// <summary>
      /// This enumeration defines that the web query is selecting Schedule template.
      /// </summary>
      Schedule_Template,

      /// <summary>
      /// This enumeration defines that the web query is selecting importing schedule data.
      /// </summary> 
      Schedule_Import,

      /// <summary>
      /// This enumeration defines that the web query is selecting exporting schedule objects.
      /// </summary>
      Schedule_Export,

      /// <summary>
      /// This enumeration defines that the web query is selecting Subject template.
      /// </summary>
      Subject_Template,

      /// <summary>
      /// This enumeration defines that the web query is selecting importing subject data.
      /// </summary> 
      Subjects_Import,

      /// <summary>
      /// This enumeration defines the web query is selecting exporting subject objects.
      /// </summary>
      Subjects_Export,

      /// <summary>
      /// This enumeration defines that the web query is selecting exporting subject visit objects.
      /// </summary>
      Subject_Visits_Export,

      /// <summary>
      /// This enumeration defines that the web query is selecting exporting visit forms objects.
      /// </summary>
      Visit_Forms_Export,

      /// <summary>
      /// This enumeration defines that the web query is selecting ProjectRecord template.
      /// </summary>
      Visit_Record_Template,

      /// <summary>
      /// This enumeration defines that the web query is selecting project (visit) records objects.
      /// </summary>
      Visit_Records_Export,

      /// <summary>
      /// This enumeration defines that the web query is selecting importing project (visit) record data.
      /// </summary> 
      Visit_Records_Import,

      /// <summary>
      /// This enumeration defines that the webe query is executing a report.
      /// </summary>
      Project_Report,

      /// <summary>
      /// This enumeration defines that the web query is selecting short tail statistical objects.
      /// 
      /// Query a subject’s trial data points as a short-tail statistical export where the 
      /// data exported as individual data points with their associated metadata.
      /// </summary>
      Short_Tail_Statisticial_Export,

      /// <summary>
      /// This enumeration defines that the web query is selecting long tail statistical objects.
      /// 
      /// Query a subject’s trial data points as a long-tail statistical export where the 
      /// data is exported as arrays of data points with references to the data points metadata.
      /// </summary> 
      Long_Tail_Statistical_Export,
     

    }//END EvQueryTypes enumeration.

}//END Namespace Evado.Model.Integration
