﻿/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\Menus.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c)  2002 - 2021  EVADO HOLDING PTY. LTD..  All rights reserved.
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
using Evado.Bll.Digital;
using Evado.Model.Digital;

namespace Evado.UniForm.Digital
{
  /// <summary>
  /// This class defines the uniform interface object for handling menus.
  /// 
  /// </summary>
  public partial class EuNavigationCommands : EuClassAdapterBase
  {
    #region Class Initialisation

    /// <summary>
    /// This method initialises the class and passs in the user profile.
    /// </summary>
    public EuNavigationCommands (
      EuGlobalObjects AdapterObjects,
      EuSession Session,
      EvClassParameters Settings )
    {
      this.ClassNameSpace = "Evado.UniForm.Clinical.EuNavigationCommands.";
      this.LogInitMethod ( "EuNavigationCommands initialisation" );
      this.ServiceUserProfile = ServiceUserProfile;
      this.Session = Session;
      this.AdapterObjects = AdapterObjects;
      this.ClassParameters = Settings;

      this.LoggingLevel = this.ClassParameters.LoggingLevel;

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
        MenuItem.PageId, MenuItem.Title, MenuItem.Group );

      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command (
        "Title",
        Evado.Model.UniForm.CommandTypes.Normal_Command,
        EuAdapter.ADAPTER_ID, String.Empty,
        Evado.Model.UniForm.ApplicationMethods.Get_Object );


      EdStaticPageIds pageId = EdStaticPageIds.Null;

      if ( EvStatics.tryParseEnumValue<EdStaticPageIds> ( MenuItem.PageId, out pageId ) == true )
      {
        #region Admin menu items

        //
        // Administration page commands
        //
        switch ( pageId )
        {
          case EdStaticPageIds.Application_Profile:
            {
              pageCommand = new Model.UniForm.Command (
                MenuItem.Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Application_Properties.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.Get_Object );

              pageCommand.SetPageId ( MenuItem.PageId );

              return pageCommand;
            }

          case EdStaticPageIds.Database_Version:
            {
              pageCommand = new Model.UniForm.Command (
                MenuItem.Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Application_Properties.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.Get_Object );

              pageCommand.SetPageId ( EdStaticPageIds.Database_Version );

              return pageCommand;
            }

          case EdStaticPageIds.Email_Templates_Page:
            {
              pageCommand = new Model.UniForm.Command (
                MenuItem.Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Email_Templates.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.Get_Object );

              pageCommand.SetPageId ( MenuItem.PageId );

              return pageCommand;
            }

          case EdStaticPageIds.Application_Event_View:
            {
              pageCommand = new Model.UniForm.Command (
                MenuItem.Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Events.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

              pageCommand.SetPageId ( MenuItem.PageId );

              return pageCommand;
            }

          case EdStaticPageIds.Application_Event:
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

          case EdStaticPageIds.Organisation_View:
            {
              pageCommand = new Model.UniForm.Command (
                MenuItem.Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Organisations.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

              pageCommand.SetPageId ( MenuItem.PageId );

              return pageCommand;
            }

          case EdStaticPageIds.Organisation_Page:
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

          case EdStaticPageIds.User_View:
            {
              pageCommand = new Model.UniForm.Command (
                MenuItem.Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Users.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

              pageCommand.SetPageId ( MenuItem.PageId );

              return pageCommand;
            }

          case EdStaticPageIds.User_Profile_Update_Page:
            {
              pageCommand = new Model.UniForm.Command (
                MenuItem.Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Users.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.Get_Object );

              pageCommand.SetPageId ( MenuItem.PageId );

              return pageCommand;
            }

          case EdStaticPageIds.Menu_View:
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

        #region applilcation configuration
        //
        // Project configuration menu commands.
        //
        switch ( pageId )
        {
          case EdStaticPageIds.Alert_View:
            {
              pageCommand = new Model.UniForm.Command (
               MenuItem.Title,
               EuAdapter.ADAPTER_ID,
               EuAdapterClasses.Alert.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

              pageCommand.SetPageId ( MenuItem.PageId );

              return pageCommand;
            }

          case EdStaticPageIds.Binary_File_List_Page:
            {
              pageCommand = new Model.UniForm.Command (
                MenuItem.Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Binary_File.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

              pageCommand.SetPageId ( MenuItem.PageId );

              return pageCommand;
            }

          case EdStaticPageIds.Selection_List_View:
            {
              pageCommand = new Model.UniForm.Command (
                MenuItem.Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Selection_Lists.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

              pageCommand.SetPageId ( MenuItem.PageId );

              return pageCommand;
            }

          case EdStaticPageIds.Selection_List_Page:
            {
              pageCommand = new Model.UniForm.Command (
                MenuItem.Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Selection_Lists.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.Get_Object );

              pageCommand.SetPageId ( MenuItem.PageId );

              return pageCommand;
            }


          case EdStaticPageIds.Record_Layout_View:
            {
              pageCommand = new Model.UniForm.Command (
                MenuItem.Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Record_Layouts.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

              pageCommand.SetPageId ( MenuItem.PageId );

              return pageCommand;
            }

          case EdStaticPageIds.Entity_Layout_View:
            {
              pageCommand = new Model.UniForm.Command (
                MenuItem.Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Entity_Layouts.ToString ( ),
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
        switch ( pageId )
        {
          case EdStaticPageIds.Data_Charting_Page:
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
          case EdStaticPageIds.Record_Query_Page:
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
          case EdStaticPageIds.Audit_Configuration_Page:
            {
              pageCommand = new Model.UniForm.Command (
                MenuItem.Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Application_Properties.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.Get_Object );

              pageCommand.SetPageId ( MenuItem.PageId );

              return pageCommand;
            }
          case EdStaticPageIds.Audit_Records_Page:
            {
              pageCommand = new Model.UniForm.Command (
                MenuItem.Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Application_Properties.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.Get_Object );

              pageCommand.SetPageId ( MenuItem.PageId );

              return pageCommand;
            }
          case EdStaticPageIds.Audit_Record_Items_Page:
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
        switch ( pageId )
        {
          case EdStaticPageIds.Report_Template_View:
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
          case EdStaticPageIds.Report_Template_Page:
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
          case EdStaticPageIds.Operational_Report_List:
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

          case EdStaticPageIds.Operational_Report_Page:
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
        switch ( pageId )
        {

          //  ------------------------------------------------------------------------------
          // milestone ancillary records pages.
          // 
          case EdStaticPageIds.Ancillary_Record_View:
            {
              pageCommand = new Model.UniForm.Command (
                MenuItem.Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Ancillary_Record.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

              pageCommand.SetPageId ( MenuItem.PageId );

              return pageCommand;
            }

          case EdStaticPageIds.Records_View:
            {
              pageCommand = new Model.UniForm.Command (
                MenuItem.Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Records.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

              pageCommand.SetPageId ( MenuItem.PageId );

              return pageCommand;
            }

          case EdStaticPageIds.Record_Export_Page:
            {
              pageCommand = new Model.UniForm.Command (
                MenuItem.Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Records.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

              pageCommand.SetPageId ( MenuItem.PageId );

              return pageCommand;
            }

          case EdStaticPageIds.Record_Admin_Page:
            {
              pageCommand = new Model.UniForm.Command (
                MenuItem.Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Records.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

              pageCommand.SetPageId ( MenuItem.PageId );

              return pageCommand;
            }


          case EdStaticPageIds.Record_Page:
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

          //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        #endregion

          #region Entity commands
          //
          // Entitity menu commands
          //
          case EdStaticPageIds.Entity_View:
          case EdStaticPageIds.Entity_Query_View:
            {
              pageCommand = new Model.UniForm.Command (
                MenuItem.Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Entities.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

              pageCommand.SetPageId ( MenuItem.PageId );

              return pageCommand;
            }

          case EdStaticPageIds.Entity_Export_Page:
            {
              pageCommand = new Model.UniForm.Command (
                MenuItem.Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Entities.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

              pageCommand.SetPageId ( MenuItem.PageId );

              return pageCommand;
            }

          case EdStaticPageIds.Entity_Admin_Page:
            {
              pageCommand = new Model.UniForm.Command (
                MenuItem.Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Entities.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.List_of_Objects );

              pageCommand.SetPageId ( MenuItem.PageId );

              return pageCommand;
            }

          case EdStaticPageIds.Entity_Page:
            {
              pageCommand = new Model.UniForm.Command (
                MenuItem.Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Entities.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.Get_Object );

              pageCommand.SetPageId ( MenuItem.PageId );

              pageCommand.SetGuid ( this.Session.Entity.Guid );

              if ( this.Session.Entity != null )
              {
                pageCommand.AddParameter ( EdRecord.RecordFieldNames.TypeId,
                  this.Session.Entity.TypeId );
              }
              return pageCommand;
            }

        }//END switch
      }

      //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
          #endregion


      return null;

    }//END convertMenuItem method
  }//END CLASS
}//END namespace