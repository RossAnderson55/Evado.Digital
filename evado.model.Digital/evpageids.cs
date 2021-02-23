/***************************************************************************************
 * <copyright file="EvURL.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvURL data object.
 *
 ****************************************************************************************/

using System;
using System.Collections.Generic;

namespace Evado.Model.Digital
{
  /// <summary>
  /// The enumeration list define the pages identifiers in the application.
  /// </summary>
  public enum EvPageIds
  {
    /// <summary>
    /// This enumeration defines the null or not selected value.
    /// </summary>
    Null,

    /// <summary>
    /// This enumeration value is the home page identifier.
    /// </summary>
    Home_Page,

    /// <summary>
    /// This enumeration value is the login page identifier.
    /// </summary>
    Login_Page,

    /// <summary>
    /// This enumeration value is the access denied page identifier.
    /// </summary>
    Access_Denied,

    //Administration
    /// <summary>
    /// This enumeration value is the data version page identifier.
    /// </summary>
    Database_Version,

    /// <summary>
    /// This enumeration value is the site profile containing site properties page identifier.
    /// </summary>
    Application_Profile,

    /// <summary>
    /// This enumeration value is the user email template properties page identifier.
    /// </summary>
    Email_Templates_Page,

    /// <summary>
    /// This enumeration value is the application event page identifier.
    /// </summary>
    Application_Event,

    /// <summary>
    /// This enumeration value is the application event view page identifier.
    /// </summary>
    Application_Event_View,

    /// <summary>
    /// This enumeration value is the menu view page identifier.
    /// </summary>
    Menu_View,

    /// <summary>
    /// This enumeration value is the menu page page identifier.
    /// </summary>
    Menu_Page,

    /// <summary>
    /// This enumeration value is the ustomer settings page identifier.
    /// </summary>
    Customer_Page,

    /// <summary>
    /// This enumeration value is the ustomer settings view page identifier.
    /// </summary>
    Customer_View,

    /// <summary>
    /// This enumeration value is the organistion profile page identifier.
    /// </summary>
    Organisation_Page,

    /// <summary>
    /// This enumeration value is the organisation profile view page identifier.
    /// </summary>
    Organisation_View,

    /// <summary>
    /// This enumeration value is the user email template properties page identifier.
    /// </summary>
    Demo_User_Page,

    /// <summary>
    /// This enumeration value is the user email template properties page identifier.
    /// </summary>
    Demo_User_Exit_Page,

    /// <summary>
    /// This enumeration value is the user profile page identifier.
    /// </summary>
    User_Profile_Page,

    /// <summary>
    /// This enumeration value is the user profile page identifier.
    /// </summary>
    User_Profile_Update_Page,

    /// <summary>
    /// This enumeration value is the user profile password page.
    /// </summary>
    User_Profile_Password_Page,

    /// <summary>
    /// This enumeration value is the user upload page identifier.
    /// </summary>
    User_Upload_Page,

    /// <summary>
    /// This enumeration value is the user template page identifier.
    /// </summary>
    User_DownLoad_Page,

    /// <summary>
    /// This enumeration value is the user profile view page identifier.
    /// </summary>
    User_View,

    /// <summary>
    /// This enumeration value is the email alert test page identifier.
    /// </summary>
    Email_Alert_Test,

    //Trial General
    /// <summary>
    /// This enumeration value is the project profile page identifier.
    /// </summary>
    Trial_View_Page,

    /// <summary>
    /// This enumeration value is the projects configuration page identifier.
    /// </summary>
    Trial_Settings_Page,

    /// <summary>
    /// This enumeration value is the projects file upload page identifier.
    /// </summary>
    Trial_Binary_File_Page,

    /// <summary>
    /// This enumeration value is the projects file upload page identifier.
    /// </summary>
    Trial_Binary_File_List_Page,

    /// <summary>
    /// This enumeration value is the projects gobal object page identifier.
    /// </summary>
    Global_Project,

    /// <summary>
    /// This enumeration value is the Global project forms object page identifier.
    /// </summary>
    Global_Form_View,

    /// <summary>
    /// This enumeration value is the form properties page identifier.
    /// </summary>

    Global_Form_Properties_Section_Page,

    /// <summary>
    /// This enumeration value is the Global project common forms object page identifier.
    /// </summary>
    Global_Common_Form_View,

    /// <summary>
    /// This enumeration value is the form properties page identifier.
    /// </summary>
    Global_Common_Form_Properties_Section_Page,

    /// <summary>
    /// This enumeration value is the Global project schedule object page identifier.
    /// </summary>
    Global_Schedule_Page,

    /// <summary>
    /// This enumeration value is the registry profile page identifier.
    /// </summary>
    Registry_Configuration,

    /// <summary>
    /// This enumeration value is the project organisation view page identifier.
    /// </summary>
    Trial_Organisation_View_Page,

    /// <summary>
    /// This enumeration value is the project organisation page identifier.
    /// </summary>
    Trial_Organisation_Page,

    /// <summary>
    /// This enumeration value is the data point export page identifier.
    /// </summary>
    Data_Point_Export_Page,

    /// <summary>
    /// This enumeration value is the data point table export page identifier.
    /// </summary>
    Data_Point_Table_Export_Page,

    /// <summary>
    /// This enumeration value is the data download page identifier.
    /// </summary>
    Data_Download_Page,

    /// <summary>
    /// This enumeration value is the project record administration page identifier.
    /// This page provide administrator access to record properties.
    /// </summary>
    Record_Admin_Page,

    /// <summary>
    /// This enumeration value is the subject administration list page identifier.
    /// This page provide administrator access to subject properties.
    /// </summary>
    Subject_Admin_List_Page,

    /// <summary>
    /// This enumeration value is the subject administration page identifier.
    /// This page provide administrator access to subject properties.
    /// </summary>
    Subject_Admin_Page,

    /// <summary>
    /// This enumeration value is the subject administration exoirt page identifier.
    /// This page provide administrator access to subject properties.
    /// </summary>
    Subject_Download_Page,

    /// <summary>
    /// This enumeration value is the visit administration page identifier.
    /// This page provide administrator access to visit properties.
    /// </summary>
    Subject_Visit_Admin_Page,

    /// <summary>
    /// This enumeration value is the project record export page identifier.
    /// </summary>
    Record_Export_Page,

    /// <summary>
    /// This enumeration value is the  common record export page identifier.
    /// </summary>
    Common_Record_Export_Page,

    /// <summary>
    /// This enumeration value is the alert page identifier.
    /// </summary>
    Alert_View,

    /// <summary>
    /// This enumeration value is the alert view page identifier.
    /// </summary>
    Alert_Page,

    /// <summary>
    /// This enumeration value is the schedule page identifier.
    /// </summary>
    Schedule_Page,

    /// <summary>
    /// This enumeration value is the schedule view page identifier.
    /// </summary>
    Schedule_View,

    /// <summary>
    /// This enumeration value is the schedule upload page identifier.
    /// </summary>
    Schedule_Upload_Page,

    /// <summary>
    /// This enumeration value is the schedule download page identifier.
    /// </summary>
    Schedule_Download_Page,

    /// <summary>
    /// This enumeration value is the activity page identifier.
    /// </summary>
    Activity_Page,

    /// <summary>
    /// This enumeration value is the activity view page identifier.
    /// </summary>
    Activity_View_Page,

    /// <summary>
    /// This enumeration value is the activity upload page identifier.
    /// </summary>
    Activity_Upload,

    /// <summary>
    /// This enumeration value is the activity upload page identifier.
    /// </summary>
    Activity_Download,

    /// <summary>
    /// This enumeration value is the milestones page identifier.
    /// </summary>
    Milestones_Page,

    /// <summary>
    /// This enumeration value is the report page identifier.
    /// </summary>
    Operational_Report_Page,

    /// <summary>
    /// This enumeration value is the report page identifier.
    /// </summary>
    Data_Management_Report_Page,

    /// <summary>
    /// This enumeration value is the report page identifier.
    /// </summary>
    Site_Report_Page,

    /// <summary>
    /// This enumeration value is the report page identifier.
    /// </summary>
    Monitoring_Report_Page,

    /// <summary>
    /// This enumeration value is the report page identifier.
    /// </summary>
    Financial_Report_Page,

    /// <summary>
    /// This enumeration value is the saved report view page identifier.
    /// </summary>
    Report_Saved_View,

    /// <summary>
    /// This enumeration value is the report view page identifier.
    /// </summary>
    Operational_Report_List,

    /// <summary>
    /// This enumeration value is the form field property data management report List page identifier.
    /// </summary>
    Data_Management_Report_List,

    /// <summary>
    /// This enumeration value is the form field property financial report list page identifier.
    /// </summary>
    Financial_Report_List,

    /// <summary>
    /// This enumeration value is the form field property monitoring report list page identifier.
    /// </summary>
    Monitoring_Report_List,

    /// <summary>
    /// This enumeration value is the form field property site report list page identifier.
    /// </summary>
    Site_Report_List,

    /// <summary>
    /// This enumeration value is the report template page identifier.
    /// </summary>
    Report_Template_Form,

    /// <summary>
    /// This enumeration value is the report template selection page identifier.
    /// </summary>
    Report_Template_Column_Selection_Page,

    /// <summary>
    /// This enumeration value is the report template view page identifier.
    /// </summary>
    Report_Template_View,

    /// <summary>
    /// This enumeration value is the report template upload page identifier.
    /// </summary>
    Report_Template_Upload,

    /// <summary>
    /// This enumeration value is the report template download page identifier.
    /// </summary>
    Report_Template_Download,

    /// <summary>
    /// This enumeration value is the Serious Adverse Event correcltation report page identifier.
    /// </summary>
    SAE_Correlation_Report,


    /// <summary>
    /// This enumeration value is the project budgets versions page identifier.
    /// </summary>
    Budget_Version_View,

    /// <summary>
    /// This enumeration value is the project budgets page identifier.
    /// </summary>
    Budget_Page,

    /// <summary>
    /// This enumeration value is the project budget item page identifier.
    /// </summary>
    Budget_Item_Page,

    /// <summary>
    /// This enumeration value is the project budget import page identifier.
    /// </summary>
    Budget_Import_Page,

    /// <summary>
    /// This enumeration value is the project budget expport page identifier.
    /// </summary>
    Budget_Export_Page,

    /// <summary>
    /// This enumeration value is the project budget report page identifier.
    /// </summary>
    Budget_Report_Page,

    /// <summary>
    /// This enumeration value is the project billing page identifier.
    /// </summary>
    Billing_Record_Page,

    /// <summary>
    /// This enumeration value is the project billing view page identifier.
    /// </summary>
    Billing_Record_View,

    /// <summary>
    /// This enumeration value is the project billing detailed report page identifier.
    /// </summary>
    Billing_Detailed_Report,

    /// <summary>
    /// This enumeration value is the project report page identifier.
    /// </summary>
    Billing_Report,

    /// <summary>
    /// This enumeration value is the project manual billing page identifier.
    /// </summary>
    Billing_Manual_Page,

    // Registry
    /// <summary>
    /// This enumeration value is the Registry site home page identifier.
    /// </summary>
    Registry_Site_Home,

    /// <summary>
    /// This enumeration value is the registry patient navigation page identifier.
    /// </summary>
    Patient_Navigator,

    /// <summary>
    /// This enumeration value is the registry patient visit page identifier.
    /// </summary>
    Patient_Visit,

    /// <summary>
    /// This enumeration value is the registry patient visit view page identifier.
    /// </summary>
    Patient_Visit_View,

    /// <summary>
    /// This enumeration value is the registry patient visit view 1 page identifier.
    /// </summary>
    Patient_Visit_View1,

    /// <summary>
    /// This enumeration value is the registry patient visit view 2 page identifier.
    /// </summary>
    Patient_Visit_View2,

    /// <summary>
    /// This enumeration value is the registry patient visit schedule page identifier.
    /// </summary>
    Patient_Visit_Schedule_Page,

    /// <summary>
    /// This enumeration value is the registry patient visit Calendar view page identifier.
    /// </summary>
    Patient_Visit_Calendar,

    /// <summary>
    /// This enumeration value is the registry patient chronological visit view page identifier.
    /// </summary>
    Patient_Chronological_Visit,

    /// <summary>
    /// This enumeration value is the registry patient upload page identifier.
    /// </summary>
    Patient_Upload,

    /// <summary>
    /// This enumeration value is the registry patient download page identifier.
    /// </summary>
    Patient_Download,

    //FormRecord Design
    /// <summary>
    /// This enumeration value is the record layout view page identifier.
    /// </summary>
    Record_Layout_View,

    /// <summary>
    /// This enumeration value is the record layout page identifier.
    /// </summary>
    Record_Layout_Page,

    /// <summary>
    /// This enumeration value is the entity layout view page identifier.
    /// </summary>
    Entity_Layout_View,

    /// <summary>
    /// This enumeration value is the entity layout page identifier.
    /// </summary>
    Entity_Layout_Page,

    /// <summary>
    /// This enumeration value is the form annotated page identifier.
    /// </summary>
    Form_Annotated_Page,

    /// <summary>
    /// This enumeration value is the form draft page identifier.
    /// </summary>
    Form_Draft_Page,

    /// <summary>
    /// This enumeration value is the form properties page identifier.
    /// </summary>
    Form_Properties_Page,

    /// <summary>
    /// This enumeration value is the form properties page identifier.
    /// </summary>
    
    Form_Properties_Section_Page,

    /// <summary>
    /// This enumeration value is the form field page identifier.
    /// </summary>
    Form_Field_Page,

    /// <summary>
    /// This enumeration value is the form template upload page identifier.
    /// </summary>
    Form_Template_Upload,

    /// <summary>
    /// This enumeration value is the form template download page identifier.
    /// </summary>
    Form_Template_Download,

    /// <summary>
    /// This enumeration value is the data dictionary view page identifier.
    /// </summary>
    Data_Dictionary_View,

    /// <summary>
    /// This enumeration value is the data dictionary page identifier.
    /// </summary>
    Data_Dictionary_Page,

    /// <summary>
    /// This enumeration value is the data dictionary upload page identifier.
    /// </summary>
    Data_Dictionary_Upload,

    /// <summary>
    /// This enumeration value is the external selection list upload page identifier.
    /// </summary>
    External_Selection_List_Upload,

    /// <summary>
    /// This enumeration value is the external selection list page identifier.
    /// </summary>
    External_Selection_List_Page,

    /// <summary>
    /// This enumeration value is the external selection list view page identifier.
    /// </summary>
    External_Selection_List_View,


    /// <summary>
    /// This enumeration value is the entity page identifier.
    /// </summary>
    Entity_Page,

    /// <summary>
    /// This enumeration value is the entity page identifier.
    /// </summary>
    Entity_Admin_Page,

    /// <summary>
    /// This enumeration value is the entity list view page identifier.
    /// </summary>
    Entity_View,

    /// <summary>
    /// This enumeration value is the Entity query llist view page identifier.
    /// </summary>
    Entity_Query_View,

    /// <summary>
    /// This enumeration value is the Entity query llist view page identifier.
    /// </summary>
    Entity_Export_Page,


    //Trial Records
    /// <summary>
    /// This enumeration value is the subject record view page identifier.
    /// </summary>
    Ancillary_Record_View,

    /// <summary>
    /// This enumeration value is the subject record page identifier.
    /// </summary>
    Ancillary_Record_Page,

    /// <summary>
    /// This enumeration value is the project record page identifier.
    /// </summary>
    Record_Page,

    /// <summary>
    /// This enumeration value is the project record view page identifier.
    /// </summary>
    Site_Record_View,

    /// <summary>
    /// This enumeration value is the project records view page identifier.
    /// </summary>
    Records_View,

    /// <summary>
    /// This enumeration value is the project my records view page identifier.
    /// </summary>
    My_Records_View,

    /// <summary>
    /// This enumeration value is the project record monitor view page identifier.
    /// </summary>
    Record_Monitor_View,

    /// <summary>
    /// This enumeration value is the project record approval page identifier.
    /// </summary>
    Record_Approval_Page,

    /// <summary>
    /// This enumeration value is the informed consent view page identifier.
    /// </summary>
    Informed_Consent_View,

    /// <summary>
    /// This enumeration value is the informed consent view page identifier.
    /// </summary>
    Informed_Consent_Export_Page,

    /// <summary>
    /// This enumeration value is the informed consent record view page identifier.
    /// </summary>

    Informed_Consent_Record_View,

    /// <summary>
    /// This enumeration value is the informed consent subject list page identifier.
    /// </summary>
    Informed_Consent_Subject_View,

    /// <summary>
    /// This enumeration value is the informed consent reconsent page identifier.
    /// </summary>
    Informed_Consent_ReConsent_Page,

    /// <summary>
    /// This enumeration value is the informed consent  review  page identifier.
    /// </summary>
    Informed_Consent_Review_Page,

    /// <summary>
    /// This enumeration value is theinformed consent  page identifier.
    /// </summary>
    Informed_Consent_Page,

    /// <summary>
    /// This enumeration value is the informed consent remotes selection page identifier.
    /// </summary>
    Informed_Consent_Remote_Selection_Page,

    /// <summary>
    /// This enumeration value is the informed consent email log page identifier.
    /// </summary>
    Informed_Consent_Email_Log_Page,

    /// <summary>
    /// This enumeration value is the informed consent record page identifier.
    /// </summary>
    Informed_Consent_Record_Page,

    /// <summary>
    /// This enumeration value is theinformed consent  exit page identifier.
    /// </summary>
    Informed_Consent_Exit_Page,


    /// <summary>
    /// This enumeration value is the project signed record view page identifier.
    /// </summary>
    Signed_Records,

    /// <summary>
    /// This enumeration value is the common record administration page identifier.
    /// This page provide administrator access to record properties.
    /// </summary>
    Common_Record_Admin_Page,

    /// <summary>
    /// This enumeration value is the common record view page identifier.
    /// </summary>
    Common_Record_View,

    /// <summary>
    /// This enumeration value is the common record page identifier.
    /// </summary>
    Common_Record_Page,

    /// <summary>
    /// This enumeration value is the adverse event report view page identifier.
    /// </summary>
    Adverse_Event_View,

    /// <summary>
    /// This enumeration value is the adverse event report page identifier.
    /// </summary>
    Adverse_Event_Page,

    /// <summary>
    /// This enumeration value is the Protocol Exception view page identifier.
    /// </summary>
    Protocol_Exception_View,

    /// <summary>
    /// This enumeration value is the serious adverse event report page identifier.
    /// </summary>
    Protocol_Exception_Page,

    /// <summary>
    /// This enumeration value is the Protocol Exception view page identifier.
    /// </summary>
    Protocol_Variation_View,

    /// <summary>
    /// This enumeration value is the serious adverse event report page identifier.
    /// </summary>
    Protocol_Variation_Page,

    /// <summary>
    /// This enumeration value is the concomitant medication view page identifier.
    /// </summary>
    Conconmitant_Medication_View,

    /// <summary>
    /// This enumeration value is the concomitant medication page identifier.
    /// </summary>
    Conconmitant_Medication_Page,

    /// <summary>
    /// This enumeration value is the serious adverse event report view page identifier.
    /// </summary>
    Serious_Adverse_Event_View,

    /// <summary>
    /// This enumeration value is the serious adverse event report page identifier.
    /// </summary>
    Serious_Adverse_Event_Page,

    /// <summary>
    /// This enumeration value is the case report form page identifier.
    /// </summary>
    Case_Report_Form,

    /// <summary>
    /// This enumeration value is the Regulstory report page identifier.
    /// </summary>
    Regulatory_Report_Page,

    /// <summary>
    /// This enumeration value is the record query page identifier.
    /// </summary>
    Record_Query_Page,

    /// <summary>
    /// This enumeration value is the data analysis query page identifier.
    /// </summary>
    Data_Charting_Page,

    /// <summary>
    /// This enumeration value is the safety query page identifier.
    /// </summary>
    Safety_Query_Page,

    /// <summary>
    /// This enumeration value is the configuration audit trail page identifier.
    /// </summary>
    Audit_Configuration_Page,

    /// <summary>
    /// This enumeration value is the record audit trail page identifier.
    /// </summary>
    Audit_Records_Page,

    /// <summary>
    /// This enumeration value is the recprd item or field audit page identifier.
    /// </summary>
    Audit_Record_Items_Page,

    /*  DEPRECIATED PAGE IDENTIFIER 

    /// <summary>
    /// This enumeration value is the project profile page identifier.
    /// </summary>
    Project_View,

    /// <summary>
    /// This enumeration value is the projects configuration page identifier.
    /// </summary>
    Project_Configuration,

    /// <summary>
    /// This enumeration value is the projects file upload page identifier.
    /// </summary>
    Project_Binary_File_Page,

    /// <summary>
    /// This enumeration value is the projects file upload page identifier.
    /// </summary>
    Project_Binary_File_List_Page,

    /// <summary>
    /// This enumeration value is the project organisation view page identifier.
    /// </summary>
    Project_Organisation_View,

    /// <summary>
    /// This enumeration value is the project organisation page identifier.
    /// </summary>
    Project_Organisation_Page,

    */
  }//END enumeration 

} // Close namespace Evado.Model
