/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\Menus.cs" company="EVADO HOLDING PTY. LTD.">
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
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.SessionState;

using Evado.Bll;
using Evado.Model;
using Evado.Bll.Clinical;
using Evado.Model.Digital;

namespace Evado.UniForm.Digital
{
  /// <summary>
  /// This class defines the uniform interface object for handling menus.
  /// 
  /// </summary>
  public partial class EuMenuUtility : EuClassAdapterBase
  {
    #region Class Initialisation

    /// <summary>
    /// This method initialises the class and passs in the user profile.
    /// </summary>
    public EuMenuUtility (
      EuSession SessionObjects,
      EvClassParameters Settings )
    {
      this.ClassNameSpace = "Evado.UniForm.Clinical.EuMenuUtility.";
      this.ServiceUserProfile = ServiceUserProfile;
      this.Session = SessionObjects;
      this.ClassParameters = Settings;

      this.LoggingLevel = this.ClassParameters.LoggingLevel;

      this.LogInitMethod ( "MenuUtility initialisation" );
    }//END Method

    #endregion

    // =====================================================================================
    /// <summary>
    /// This method generates the commands associated with the selected menu item.
    /// </summary>
    /// <param name="MenuItem">The menu object</param>
    /// <returns>ClientClientDataObjectEvado.Model.UniForm.Command object.</returns>
    //  ------------------------------------------------------------------------------------
    public Evado.Model.UniForm.Command getMenuItemCommandObject ( EvMenuItem MenuItem )
    {
      this.resetAdapterLog ( );
      this.LogMethod ( "getMenuCommandObject" );
      this.LogDebug ( "PageId: {0}, Title: {1}, Group:  ",
        MenuItem.PageId,MenuItem.Title, MenuItem.Group );

      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command (
        "Title",
        Evado.Model.UniForm.CommandTypes.Normal_Command,
        EuAdapter.ADAPTER_ID, String.Empty,
        Evado.Model.UniForm.ApplicationMethods.Get_Object );

      #region Admin menu items

      //
      // Administration page commands
      //
      switch ( MenuItem.PageId )
      {
        case EvPageIds.Application_Profile:
          {
            pageCommand = new Model.UniForm.Command (
              MenuItem.Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Application_Properties.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.Get_Object );

            pageCommand.SetPageId ( MenuItem.PageId );

            return pageCommand;
          }

        case EvPageIds.Database_Version:
          {
            pageCommand = new Model.UniForm.Command (
              MenuItem.Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Application_Properties.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.Get_Object );

            pageCommand.SetPageId ( EvPageIds.Database_Version );

            return pageCommand;
          }

        case EvPageIds.Email_Templates_Page:
          {
            pageCommand = new Model.UniForm.Command (
              MenuItem.Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Email_Templates.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.Get_Object );

            pageCommand.SetPageId ( MenuItem.PageId );

            return pageCommand;
          }

        case EvPageIds.Application_Event_View:
          {
            pageCommand = new Model.UniForm.Command (
              MenuItem.Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Events.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

            pageCommand.SetPageId ( MenuItem.PageId );

            return pageCommand;
          }

        case EvPageIds.Application_Event:
          {
            pageCommand = new Model.UniForm.Command (
              MenuItem.Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Events.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.Get_Object );

            pageCommand.SetPageId ( MenuItem.PageId );

            // 
            // Add the groupCommand parameters.
            // 
            pageCommand.SetGuid (
              this.Session.MenuItem.Guid );

            return pageCommand;
          }

        case EvPageIds.Organisation_View:
          {
            pageCommand = new Model.UniForm.Command (
              MenuItem.Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Organisations.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

            pageCommand.SetPageId ( MenuItem.PageId );

            return pageCommand;
          }

        case EvPageIds.Organisation_Page:
          {
            pageCommand = new Model.UniForm.Command (
              MenuItem.Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Organisations.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.Get_Object );

            // 
            // Add the groupCommand parameters.
            // 
            pageCommand.SetGuid ( this.Session.MenuItem.Guid );

            pageCommand.SetPageId ( MenuItem.PageId );

            return pageCommand;
          }

        case EvPageIds.User_View:
          {
            pageCommand = new Model.UniForm.Command (
              MenuItem.Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Users.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

            pageCommand.SetPageId ( MenuItem.PageId );

            return pageCommand;
          }

        case EvPageIds.User_Profile_Update_Page:
          {
            pageCommand = new Model.UniForm.Command (
              MenuItem.Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Users.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.Get_Object );

            pageCommand.SetPageId ( MenuItem.PageId );

            return pageCommand;
          }

        case EvPageIds.Menu_View:
          {
            pageCommand = new Model.UniForm.Command (
              MenuItem.Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Menu.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

            pageCommand.SetPageId ( MenuItem.PageId );

            return pageCommand;
          }

      }//END  admin page switch statement

      //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      #endregion

      #region project configuration
      //
      // Project configuration menu commands.
      //
      switch ( MenuItem.PageId )
      {
        case EvPageIds.Alert_View:
          {
            pageCommand = new Model.UniForm.Command (
             MenuItem.Title,
             EuAdapter.ADAPTER_ID,
             EuAdapterClasses.Alert.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

            pageCommand.SetPageId ( MenuItem.PageId );

            return pageCommand;
          }

        case EvPageIds.Trial_Binary_File_List_Page:
          {
            pageCommand = new Model.UniForm.Command (
              MenuItem.Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Binary_File.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

            pageCommand.SetPageId ( MenuItem.PageId );

            return pageCommand;
          }


        case EvPageIds.Data_Point_Export_Page:
          {
            pageCommand = new Model.UniForm.Command (
              MenuItem.Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Applications.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.Get_Object );

            pageCommand.SetPageId ( MenuItem.PageId );

            return pageCommand;
          }


        case EvPageIds.Form_View:
          {
            pageCommand = new Model.UniForm.Command (
              MenuItem.Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Record_Layouts.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

            pageCommand.SetPageId ( MenuItem.PageId );

            return pageCommand;
          }
      }//END  admin page switch statement

      //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      #endregion

      #region project analysis and reporting menu items.
      //
      // Project analysis and reporting menu commands.
      //
      switch ( MenuItem.PageId )
      {
        case EvPageIds.Data_Charting_Page:
          {
            this.LogValue ( MenuItem.PageId + " ADDED" );
            pageCommand = new Model.UniForm.Command (
              MenuItem.Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Analysis.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

            pageCommand.SetPageId ( MenuItem.PageId );

            return pageCommand;
          }
        case EvPageIds.Record_Query_Page:
          {
            this.LogValue ( MenuItem.PageId + " ADDED" );
            pageCommand = new Model.UniForm.Command (
              MenuItem.Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Analysis.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

            pageCommand.SetPageId ( MenuItem.PageId );

            return pageCommand;
          }
        case EvPageIds.Audit_Configuration_Page:
          {
            pageCommand = new Model.UniForm.Command (
              MenuItem.Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Application_Properties.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.Get_Object );

            pageCommand.SetPageId ( MenuItem.PageId );

            return pageCommand;
          }
        case EvPageIds.Audit_Records_Page:
          {
            pageCommand = new Model.UniForm.Command (
              MenuItem.Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Application_Properties.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.Get_Object );

            pageCommand.SetPageId ( MenuItem.PageId );

            return pageCommand;
          }
        case EvPageIds.Audit_Record_Items_Page:
          {
            pageCommand = new Model.UniForm.Command (
              MenuItem.Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Application_Properties.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.Get_Object );

            pageCommand.SetPageId ( MenuItem.PageId );

            return pageCommand;
          }
      }

      //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      #endregion

      #region reporting menu items.
      //
      // Project analysis and reporting menu commands.
      //
      switch ( MenuItem.PageId )
      {
        case EvPageIds.Report_Template_View:
          {
            this.LogValue ( MenuItem.PageId + " ADDED" );
            pageCommand = new Model.UniForm.Command (
              MenuItem.Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.ReportTemplates.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

            pageCommand.SetPageId ( MenuItem.PageId );

            return pageCommand;
          }
        case EvPageIds.Report_Template_Form:
          {
            this.LogValue ( MenuItem.PageId + " ADDED" );
            pageCommand = new Model.UniForm.Command (
              MenuItem.Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.ReportTemplates.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.Get_Object );

            pageCommand.SetPageId ( MenuItem.PageId );

            return pageCommand;
          }
        case EvPageIds.Operational_Report_List:
        case EvPageIds.Monitoring_Report_List:
        case EvPageIds.Data_Management_Report_List:
        case EvPageIds.Financial_Report_List:
          {
            this.LogValue ( MenuItem.PageId + " ADDED" );
            pageCommand = new Model.UniForm.Command (
              MenuItem.Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Reports.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

            pageCommand.SetPageId ( MenuItem.PageId );

            return pageCommand;
          }

        case EvPageIds.Operational_Report_Page:
        case EvPageIds.Data_Management_Report_Page:
        case EvPageIds.Site_Report_Page:
        case EvPageIds.Monitoring_Report_Page:
        case EvPageIds.Financial_Report_Page:
        case EvPageIds.SAE_Correlation_Report:
        case EvPageIds.Subject_Calendar_Page:
          {
            this.LogValue ( MenuItem.PageId + " ADDED" );
            pageCommand = new Model.UniForm.Command (
              MenuItem.Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Reports.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.Get_Object );

            pageCommand.SetPageId ( MenuItem.PageId );

            return pageCommand;
          }
      }//END switch statement

      //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      #endregion

      #region record menu items.

      //
      // Project records menu commands.
      //
      switch ( MenuItem.PageId )
      {

        //  ------------------------------------------------------------------------------
        // milestone ancillary records pages.
        // 
        case EvPageIds.Ancillary_Record_View:
          {
            pageCommand = new Model.UniForm.Command (
              MenuItem.Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Ancillary_Record.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

            pageCommand.SetPageId ( MenuItem.PageId );

            return pageCommand;
          }

        //  ------------------------------------------------------------------------------
        // Project Record pages.
        // 
        case EvPageIds.Site_Record_View:
          {
            pageCommand = new Model.UniForm.Command (
              MenuItem.Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Records.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

            pageCommand.SetPageId ( MenuItem.PageId );

            return pageCommand;
          }

        case EvPageIds.Records_View:
          {
            pageCommand = new Model.UniForm.Command (
              MenuItem.Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Records.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

            pageCommand.SetPageId ( MenuItem.PageId );

            return pageCommand;
          }

        case EvPageIds.Record_Export_Page:
          {
            pageCommand = new Model.UniForm.Command (
              MenuItem.Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Records.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

            pageCommand.SetPageId ( MenuItem.PageId );

            return pageCommand;
          }

        case EvPageIds.Record_Admin_Page:
          {
            pageCommand = new Model.UniForm.Command (
              MenuItem.Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Records.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

            pageCommand.SetPageId ( MenuItem.PageId );

            return pageCommand;
          }


        case EvPageIds.Record_Page:
          {
            pageCommand = new Model.UniForm.Command (
              MenuItem.Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Records.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.Get_Object );

            pageCommand.SetPageId ( MenuItem.PageId );

            pageCommand.SetGuid ( this.Session.Record.Guid );

            if ( this.Session.Entity != null )
            {
              pageCommand.AddParameter ( EdRecord.RecordFieldNames.TypeId,
                this.Session.Entity.TypeId );
            }
            return pageCommand;
          }

      }

      //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      #endregion


      return null;

    }//END convertMenuItem method
  }//END CLASS
}//END namespace