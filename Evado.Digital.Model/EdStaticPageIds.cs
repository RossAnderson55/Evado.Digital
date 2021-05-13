/***************************************************************************************
 * <copyright file="EvURL.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvURL data object.
 *
 ****************************************************************************************/

using System;
using System.Collections.Generic;

namespace Evado.Digital.Model
{
  /// <summary>
  /// The enumeration list define the pages identifiers in the application.
  /// </summary>
  public enum EdStaticPageIds
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
    /// This enumeration value is the user email user properties page identifier.
    /// </summary>
    Email_User_Page,

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
    /// This enumeration value is the organistion profile page identifier.
    /// </summary>
    Organisation_Page,

    /// <summary>
    /// This enumeration value is the organisation profile view page identifier.
    /// </summary>
    Organisation_View,

    /// <summary>
    /// This enumeration value is the user profile page identifier.
    /// </summary>
    User_Profile_Page,

    /// <summary>
    /// This enumeration is the value of the user registion page identifier.
    /// </summary>
    User_Registration_Page,

    /// <summary>
    /// This enumeration is the value of the user registion exit page identifier.
    /// </summary>
    Demo_User_Exit_Page,

    /// <summary>
    /// This enumeration value is the user profile page identifier.
    /// </summary>
    My_User_Profile_Update_Page,

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

    /// <summary>
    /// This enumeration value is the external selection list upload page identifier.
    /// </summary>
    Page_Layout_Upload,

    /// <summary>
    /// This enumeration value is the external selection list page identifier.
    /// </summary>
    Page_Layout_Page,

    /// <summary>
    /// This enumeration value is the external selection list view page identifier.
    /// </summary>
    Page_Layout_View,


    /// <summary>
    /// a trial binary file list page.
    /// </summary>
    Binary_File_List_Page,

    /// <summary>
    /// This enumeration value is the data download page identifier.
    /// </summary>
    Data_Download_Page,

    /// <summary>
    /// This enumeration value is the data charting page identifier.
    /// </summary>
    Data_Charting_Page,

    /// <summary>
    /// This enumeration value is the project record administration page identifier.
    /// This page provide administrator access to record properties.
    /// </summary>
    Record_Admin_Page,

    /// <summary>
    /// This enumeration value is the project record export page identifier.
    /// </summary>
    Record_Export_Page,

    /// <summary>
    /// This enumeration value is the alert page identifier.
    /// </summary>
    Alert_View,

    /// <summary>
    /// This enumeration value is the alert view page identifier.
    /// </summary>
    Alert_Page,

    /// <summary>
    /// This enumeration value is the report page identifier.
    /// </summary>
    Operational_Report_Page,

    /// <summary>
    /// This enumeration value is the saved report view page identifier.
    /// </summary>
    Report_Saved_View,

    /// <summary>
    /// This enumeration value is the report view page identifier.
    /// </summary>
    Operational_Report_List,

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
    /// this enumeration value is the report template form or page.
    /// </summary>
    Report_Template_Page,

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
    Selection_List_Upload,

    /// <summary>
    /// This enumeration value is the external selection list page identifier.
    /// </summary>
    Selection_List_Page,

    /// <summary>
    /// This enumeration value is the external selection list view page identifier.
    /// </summary>
    Selection_List_View,

    /// <summary>
    /// This enumeration value is the entity page identifier.
    /// </summary>
    Entity_Page,

    /// <summary>
    /// This enumeration value is the entity user access page identifier.
    /// used for entities that have private or controlled access.
    /// </summary>
    Entity_User_Access_Page,

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
    Entity_Filter_View,

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
    /// this enutmeration value is the record page.
    /// </summary>
    Record_Page,

    /// <summary>
    /// This enumeration value is the project records view page identifier.
    /// </summary>
    Records_View,

    /// <summary>
    /// This enumeration value is the record query page identifier.
    /// </summary>
    Record_Query_Page,

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

  }//END enumeration 

} // Close namespace Evado.Model
