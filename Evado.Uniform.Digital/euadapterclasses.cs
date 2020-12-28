/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\EuAdapterClasses.cs" 
 *  company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the AbstractedPage ResultData object.
 *
 ****************************************************************************************/
using System;

namespace Evado.UniForm.Clinical
{
     /// <summary>
    /// This is an enumeration of of application objects. 
    /// </summary>
    public enum EuAdapterClasses
    {
      /// <summary>
      /// This option is used to define not selected or null entry.
      /// </summary>
      Null,

      /// <summary>
      /// This option is used to define demonstration Registration.
      /// </summary>
      Demo_Registration,

      /// <summary>
      /// This option identifies the Home Page Class object.
      /// </summary>
      Home_Page,

      /// <summary>
      /// This option identifies the Menu class object.
      /// </summary>
      Menu,

      /// <summary>
      /// This option identifies the Customers class object.
      /// </summary>
      Customers,

      /// <summary>
      /// This option identifies the Organisation class object.
      /// </summary>
      Organisations,

      /// <summary>
      /// This option identifies the user class object.
      /// </summary>
      Users,

      /// <summary>
      /// This option identifies the application event class object.
      /// </summary>
      Events,

      /// <summary>
      /// This option identifies the project class object.
      /// </summary>
      Projects,

      /// <summary>
      /// This option identifies the Global Project class object.
      /// </summary>
      Global_Project,

      /// <summary>
      /// This option identifies the Global Forms class object.
      /// </summary>
      Global_Forms,

      /// <summary>
      /// This option identifies the Global form fields class object.
      /// </summary>
      Global_Form_Fields,

      /// <summary>
      /// This option identifies the global common forms class object.
      /// </summary>
      Global_Common_Forms,

      /// <summary>
      /// This option identifies the global common firm fields class object.
      /// </summary>
      Global_Common_Form_Fields,

      /// <summary>
      /// This option identifies the global schedule class object.
      /// </summary>
      Global_Schedules,

      /// <summary>
      /// This option identifies the schedule class object.
      /// </summary>
      Schedules,

      /// <summary>
      /// This option identifies the milestones class milestones object.
      /// </summary>
      Milestones,

      /// <summary>
      /// This option identifies the activies class object.
      /// </summary>
      Activities,

      /// <summary>
      /// This option identifies the project forms class object.
      /// </summary>
      Project_Forms,

      /// <summary>
      /// This option identifies the project form fields class object.
      /// </summary>
      Project_Form_Fields,

      /// <summary>
      /// This option identifies the Common form class object.
      /// </summary>
      Common_Forms,

      /// <summary>
      /// This option identifies the Common Form field class object.
      /// </summary>
      Common_Form_Fields,

      /// <summary>
      /// This option identifies the project organisation (Site) class object.
      /// </summary>
      Project_Organisation,

      /// <summary>
      /// This option identifies the project alert class object.
      /// </summary>
      Alert,

      /// <summary>
      /// This option identifies the project analysis class object.
      /// </summary>
      Analysis,

      /// <summary>
      /// This option identifies the project reporting template class object.
      /// </summary>
      ReportTemplates,

      /// <summary>
      /// This option identifies the project reporting class object.
      /// </summary>
      Reports,

      /// <summary>
      /// This option identifies the patients class object.
      /// </summary>
      Patients,

      /// <summary>
      /// This option identifies the patient recorded observation class object.
      /// This object acts as a separate home and environment for patients.
      /// </summary>
      Patient_Recorded_Observations,

      /// <summary>
      /// This option identifies the informed consent class object.
      /// </summary>
      Informed_Consent,

      /// <summary>
      /// This option identifies the informed consent remote class object.
      /// </summary>
      Informed_Consent_Remote,

      /// <summary>
      /// This option identifies the patient recorded outcomes (questionnaire) class object.
      /// </summary>
      Patient_Outcomes,

      /// <summary>
      /// This option identifies the milestone class object.
      /// </summary>
      Subjects,

      /// <summary>
      /// This option identifies the FirstSubject Visit class object.
      /// </summary>
      Subject_Milestone,

      /// <summary>
      /// This option identifies the project record class object.
      /// </summary>
      Ancillary_Record,

      /// <summary>
      /// This option identifies the project record class object.
      /// </summary>
      Scheduled_Record,

      /// <summary>
      /// This option identifies the common record class object.
      /// </summary>
      Common_Record,

      /// <summary>
      /// This option identifies the binary files class object.
      /// </summary>
      Binary_File,

      /// <summary>
      /// This option identifies the site profile class object.
      /// </summary>
      Application_Properties,

      /// <summary>
      /// This option identifies the site profile class object.
      /// </summary>
      Email_Templates,

      /// <summary>
      /// This option identifies the Account profile class object.
      /// </summary>
      Account_Profile,

      /// <summary>
      /// This option identifies the Budgets class object.
      /// </summary>
      Budgets,

      /// <summary>
      /// This option identifies the billing class object.
      /// </summary>
      Billing_Records,
    }

}///END NAMESPACE
