/***************************************************************************************
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


using Evado.Model;
using Evado.Digital.Bll;
using Evado.Digital.Model;

namespace Evado.Digital.Adapter
{
  /// <summary>
  /// This class defines the uniform interface object for handling menus.
  /// 
  /// </summary>
  public partial class EuNavigation : EuClassAdapterBase
  {
    #region Class Initialisation

    /// <summary>
    /// This method initialises the class and passs in the user profile.
    /// </summary>
    public EuNavigation (
      EuGlobalObjects AdapterObjects,
      EuSession Session,
      EvClassParameters ClassParameters )
    {
      this.ClassNameSpace = "Evado.UniForm.Clinical.EuNavigationCommands.";
      this.LogInitMethod ( "EuNavigationCommands initialisation" );
      this.ServiceUserProfile = ServiceUserProfile;
      this.Session = Session;
      this.AdapterObjects = AdapterObjects;
      this.ClassParameters = ClassParameters;

      this.LoggingLevel = this.ClassParameters.LoggingLevel;

    }//END Method

    #endregion

    // =====================================================================================
    /// <summary>
    /// This method generates the commands associated with the selected menu item.
    /// </summary>
    /// <param name="MenuItem">The menu object</param>
    /// <returns>Evado.UniForm.Model.EuCommand object.</returns>
    //  ------------------------------------------------------------------------------------
    public Evado.UniForm.Model.EuCommand GetNavigationCommand ( EvMenuItem MenuItem )
    {
      this.resetAdapterLog ( );
      this.LogMethod ( "getMenuCommandObject" );
      this.LogDebug ( "MenuItem: PageId: {0}, Title: {1}, Group: {2} ",
        MenuItem.PageId, MenuItem.Title, MenuItem.Group );

      return GetNavigationCommand ( MenuItem.PageId, MenuItem.Title, MenuItem.Parameters );
    }//END method

    // =====================================================================================
    /// <summary>
    /// This method generates the commands associated with the selected menu item.
    /// </summary>
    /// <param name="PageId">String: page identifier</param>
    /// <param name="Title">String: command  title</param>
    /// <returns>Evado.UniForm.Model.EuCommand object.</returns>
    //  ------------------------------------------------------------------------------------
    public Evado.UniForm.Model.EuCommand GetNavigationCommand ( 
      String PageId, 
      String Title,
      String Parameters)
    {
      this.resetAdapterLog ( );
      this.LogMethod ( "getMenuCommandObject" );
      this.LogDebug ( "PageId: {0}, Title: {1}.", PageId, Title );
      //
      // Initialise the methods variables and objects.
      //
      Evado.Digital.Model.EdStaticPageIds pageId = Evado.Digital.Model.EdStaticPageIds.Null;
      Evado.UniForm.Model.EuCommand pageCommand = new Evado.UniForm.Model.EuCommand ( );

      //
      // process static page identifeirs to create their commands.
      //
      if ( EvStatics.tryParseEnumValue<Evado.Digital.Model.EdStaticPageIds> ( PageId, out pageId ) == true )
      {
        #region Admin menu items

        //
        // Administration page commands
        //
        switch ( pageId )
        {
          case Evado.Digital.Model.EdStaticPageIds.Application_Profile:
            {
              pageCommand = new Evado.UniForm.Model.EuCommand (
                Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Application_Properties.ToString ( ),
                Evado.UniForm.Model.EuMethods.Get_Object );

              pageCommand.SetPageId ( PageId );

              return pageCommand;
            }

          case Evado.Digital.Model.EdStaticPageIds.Database_Version:
            {
              pageCommand = new Evado.UniForm.Model.EuCommand (
                Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Application_Properties.ToString ( ),
                Evado.UniForm.Model.EuMethods.Get_Object );

              pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Database_Version );

              return pageCommand;
            }

          case Evado.Digital.Model.EdStaticPageIds.Email_Templates_Page:
            {
              pageCommand = new Evado.UniForm.Model.EuCommand (
                Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Email_Templates.ToString ( ),
                Evado.UniForm.Model.EuMethods.Get_Object );

              pageCommand.SetPageId ( PageId );

              return pageCommand;
            }

          case Evado.Digital.Model.EdStaticPageIds.Application_Event_View:
            {
              pageCommand = new Evado.UniForm.Model.EuCommand (
                Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Events.ToString ( ),
                Evado.UniForm.Model.EuMethods.List_of_Objects );

              pageCommand.SetPageId ( PageId );

              return pageCommand;
            }

          case Evado.Digital.Model.EdStaticPageIds.Application_Event:
            {
              pageCommand = new Evado.UniForm.Model.EuCommand (
                Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Events.ToString ( ),
                Evado.UniForm.Model.EuMethods.Get_Object );

              pageCommand.SetPageId ( PageId );

              // 
              // Add the groupCommand parameters.
              // 
              pageCommand.SetGuid (
                this.Session.MenuItem.Guid );

              return pageCommand;
            }

          case Evado.Digital.Model.EdStaticPageIds.Organisation_View:
            {
              pageCommand = new Evado.UniForm.Model.EuCommand (
                Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Organisations.ToString ( ),
                Evado.UniForm.Model.EuMethods.List_of_Objects );

              pageCommand.SetPageId ( PageId );

              return pageCommand;
            }

          case Evado.Digital.Model.EdStaticPageIds.Organisation_Page:
            {
              pageCommand = new Evado.UniForm.Model.EuCommand (
                Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Organisations.ToString ( ),
                Evado.UniForm.Model.EuMethods.Get_Object );

              // 
              // Add the groupCommand parameters.
              // 
              pageCommand.SetGuid ( this.Session.MenuItem.Guid );

              pageCommand.SetPageId ( PageId );

              return pageCommand;
            }

          case Evado.Digital.Model.EdStaticPageIds.User_View:
            {
              pageCommand = new Evado.UniForm.Model.EuCommand (
                Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Users.ToString ( ),
                Evado.UniForm.Model.EuMethods.List_of_Objects );

              pageCommand.SetPageId ( PageId );

              return pageCommand;
            }

          case Evado.Digital.Model.EdStaticPageIds.My_User_Profile_Update_Page:
            {
              pageCommand = new Evado.UniForm.Model.EuCommand (
                Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Users.ToString ( ),
                Evado.UniForm.Model.EuMethods.Get_Object );

              pageCommand.SetPageId ( PageId );

              return pageCommand;
            }
          case Evado.Digital.Model.EdStaticPageIds.Email_User_Page:
            {
              string [] arParameters = Parameters.Split ( ';' );

              pageCommand = new Evado.UniForm.Model.EuCommand (
                Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Users.ToString ( ),
                Evado.UniForm.Model.EuMethods.Get_Object );

              pageCommand.SetPageId ( PageId );

              foreach ( string parm in arParameters )
              {
                if ( parm.Contains ( "=" ) == false )
                {
                  continue;
                }
                String [] arParm = parm.Split ( '=' );

                pageCommand.AddParameter ( arParm [ 0 ], arParm [ 1 ] );
              }

              return pageCommand;
            }

          case Evado.Digital.Model.EdStaticPageIds.Menu_View:
            {
              pageCommand = new Evado.UniForm.Model.EuCommand (
                Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Menu.ToString ( ),
                Evado.UniForm.Model.EuMethods.List_of_Objects );

              pageCommand.SetPageId ( PageId );

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
          case Evado.Digital.Model.EdStaticPageIds.Alert_View:
            {
              pageCommand = new Evado.UniForm.Model.EuCommand (
               Title,
               EuAdapter.ADAPTER_ID,
               EuAdapterClasses.Alert.ToString ( ),
                Evado.UniForm.Model.EuMethods.List_of_Objects );

              pageCommand.SetPageId ( PageId );

              return pageCommand;
            }

          case Evado.Digital.Model.EdStaticPageIds.Binary_File_List_Page:
            {
              pageCommand = new Evado.UniForm.Model.EuCommand (
                Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Binary_File.ToString ( ),
                Evado.UniForm.Model.EuMethods.List_of_Objects );

              pageCommand.SetPageId ( PageId );

              return pageCommand;
            }

          case Evado.Digital.Model.EdStaticPageIds.Page_Layout_View:
            {
              pageCommand = new Evado.UniForm.Model.EuCommand (
                Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Page_Layouts.ToString ( ),
                Evado.UniForm.Model.EuMethods.List_of_Objects );

              pageCommand.SetPageId ( PageId );

              return pageCommand;
            }

          case Evado.Digital.Model.EdStaticPageIds.Page_Layout_Page:
            {
              pageCommand = new Evado.UniForm.Model.EuCommand (
                Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Page_Layouts.ToString ( ),
                Evado.UniForm.Model.EuMethods.Get_Object );

              pageCommand.SetPageId ( PageId );

              return pageCommand;
            }

          case Evado.Digital.Model.EdStaticPageIds.Selection_List_View:
            {
              pageCommand = new Evado.UniForm.Model.EuCommand (
                Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Selection_Lists.ToString ( ),
                Evado.UniForm.Model.EuMethods.List_of_Objects );

              pageCommand.SetPageId ( PageId );

              return pageCommand;
            }

          case Evado.Digital.Model.EdStaticPageIds.Selection_List_Page:
            {
              pageCommand = new Evado.UniForm.Model.EuCommand (
                Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Selection_Lists.ToString ( ),
                Evado.UniForm.Model.EuMethods.Get_Object );

              pageCommand.SetPageId ( PageId );

              return pageCommand;
            }


          case Evado.Digital.Model.EdStaticPageIds.Record_Layout_View:
            {
              pageCommand = new Evado.UniForm.Model.EuCommand (
                Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Record_Layouts.ToString ( ),
                Evado.UniForm.Model.EuMethods.List_of_Objects );

              pageCommand.SetPageId ( PageId );

              return pageCommand;
            }

          case Evado.Digital.Model.EdStaticPageIds.Entity_Layout_View:
            {
              pageCommand = new Evado.UniForm.Model.EuCommand (
                Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Entity_Layouts.ToString ( ),
                Evado.UniForm.Model.EuMethods.List_of_Objects );

              pageCommand.SetPageId ( PageId );

              return pageCommand;
            }
        }//END  admin page switch statement

        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        #endregion

        #region  analysis and reporting menu items.
        //
        // Project analysis and reporting menu commands.
        //
        switch ( pageId )
        {
          case Evado.Digital.Model.EdStaticPageIds.Data_Charting_Page:
            {
              this.LogValue ( PageId + " ADDED" );
              pageCommand = new Evado.UniForm.Model.EuCommand (
                Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Analysis.ToString ( ),
                Evado.UniForm.Model.EuMethods.List_of_Objects );

              pageCommand.SetPageId ( PageId );

              return pageCommand;
            }
          case Evado.Digital.Model.EdStaticPageIds.Record_Query_Page:
            {
              this.LogValue ( PageId + " ADDED" );
              pageCommand = new Evado.UniForm.Model.EuCommand (
                Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Analysis.ToString ( ),
                Evado.UniForm.Model.EuMethods.List_of_Objects );

              pageCommand.SetPageId ( PageId );

              return pageCommand;
            }
          case Evado.Digital.Model.EdStaticPageIds.Audit_Configuration_Page:
            {
              pageCommand = new Evado.UniForm.Model.EuCommand (
                Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Application_Properties.ToString ( ),
                Evado.UniForm.Model.EuMethods.Get_Object );

              pageCommand.SetPageId ( PageId );

              return pageCommand;
            }
          case Evado.Digital.Model.EdStaticPageIds.Audit_Records_Page:
            {
              pageCommand = new Evado.UniForm.Model.EuCommand (
                Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Application_Properties.ToString ( ),
                Evado.UniForm.Model.EuMethods.Get_Object );

              pageCommand.SetPageId ( PageId );

              return pageCommand;
            }
          case Evado.Digital.Model.EdStaticPageIds.Audit_Record_Items_Page:
            {
              pageCommand = new Evado.UniForm.Model.EuCommand (
                Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Application_Properties.ToString ( ),
                Evado.UniForm.Model.EuMethods.Get_Object );

              pageCommand.SetPageId ( PageId );

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
          case Evado.Digital.Model.EdStaticPageIds.Report_Template_View:
            {
              this.LogValue ( PageId + " ADDED" );
              pageCommand = new Evado.UniForm.Model.EuCommand (
                Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.ReportTemplates.ToString ( ),
                Evado.UniForm.Model.EuMethods.List_of_Objects );

              pageCommand.SetPageId ( PageId );

              return pageCommand;
            }
          case Evado.Digital.Model.EdStaticPageIds.Report_Template_Page:
            {
              this.LogValue ( PageId + " ADDED" );
              pageCommand = new Evado.UniForm.Model.EuCommand (
                Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.ReportTemplates.ToString ( ),
                Evado.UniForm.Model.EuMethods.Get_Object );

              pageCommand.SetPageId ( PageId );

              return pageCommand;
            }
          case Evado.Digital.Model.EdStaticPageIds.Operational_Report_List:
            {
              this.LogValue ( PageId + " ADDED" );
              pageCommand = new Evado.UniForm.Model.EuCommand (
                Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Reports.ToString ( ),
                Evado.UniForm.Model.EuMethods.List_of_Objects );

              pageCommand.SetPageId ( PageId );

              return pageCommand;
            }

          case Evado.Digital.Model.EdStaticPageIds.Operational_Report_Page:
            {
              this.LogValue ( PageId + " ADDED" );
              pageCommand = new Evado.UniForm.Model.EuCommand (
                Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Reports.ToString ( ),
                Evado.UniForm.Model.EuMethods.Get_Object );

              pageCommand.SetPageId ( PageId );

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
          case Evado.Digital.Model.EdStaticPageIds.Ancillary_Record_View:
            {
              pageCommand = new Evado.UniForm.Model.EuCommand (
                Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Ancillary_Record.ToString ( ),
                Evado.UniForm.Model.EuMethods.List_of_Objects );

              pageCommand.SetPageId ( PageId );

              return pageCommand;
            }

          case Evado.Digital.Model.EdStaticPageIds.Records_View:
            {
              pageCommand = new Evado.UniForm.Model.EuCommand (
                Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Records.ToString ( ),
                Evado.UniForm.Model.EuMethods.List_of_Objects );

              pageCommand.SetPageId ( PageId );

              return pageCommand;
            }

          case Evado.Digital.Model.EdStaticPageIds.Record_Export_Page:
            {
              pageCommand = new Evado.UniForm.Model.EuCommand (
                Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Records.ToString ( ),
                Evado.UniForm.Model.EuMethods.List_of_Objects );

              pageCommand.SetPageId ( PageId );

              return pageCommand;
            }

          case Evado.Digital.Model.EdStaticPageIds.Record_Admin_Page:
            {
              pageCommand = new Evado.UniForm.Model.EuCommand (
                Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Records.ToString ( ),
                Evado.UniForm.Model.EuMethods.List_of_Objects );

              pageCommand.SetPageId ( PageId );

              return pageCommand;
            }


          case Evado.Digital.Model.EdStaticPageIds.Record_Page:
            {
              pageCommand = new Evado.UniForm.Model.EuCommand (
                Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Records.ToString ( ),
                Evado.UniForm.Model.EuMethods.Get_Object );

              pageCommand.SetPageId ( PageId );

              pageCommand.SetGuid ( this.Session.Record.Guid );

              if ( this.Session.Entity != null )
              {
                pageCommand.AddParameter ( EdRecord.FieldNames.TypeId,
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
          case Evado.Digital.Model.EdStaticPageIds.Entity_View:
          case Evado.Digital.Model.EdStaticPageIds.Entity_Filter_View:
            {
              pageCommand = new Evado.UniForm.Model.EuCommand (
                Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Entities.ToString ( ),
                Evado.UniForm.Model.EuMethods.List_of_Objects );

              pageCommand.SetPageId ( PageId );

              return pageCommand;
            }

          case Evado.Digital.Model.EdStaticPageIds.Entity_Export_Page:
            {
              pageCommand = new Evado.UniForm.Model.EuCommand (
                Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Entities.ToString ( ),
                Evado.UniForm.Model.EuMethods.List_of_Objects );

              pageCommand.SetPageId ( PageId );

              return pageCommand;
            }

          case Evado.Digital.Model.EdStaticPageIds.Entity_Admin_Page:
            {
              pageCommand = new Evado.UniForm.Model.EuCommand (
                Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Entities.ToString ( ),
                Evado.UniForm.Model.EuMethods.List_of_Objects );

              pageCommand.SetPageId ( PageId );

              return pageCommand;
            }

          case Evado.Digital.Model.EdStaticPageIds.Entity_Page:
            {
              pageCommand = new Evado.UniForm.Model.EuCommand (
                Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Entities.ToString ( ),
                Evado.UniForm.Model.EuMethods.Get_Object );

              pageCommand.SetPageId ( PageId );

              pageCommand.SetGuid ( this.Session.Entity.Guid );

              if ( this.Session.Entity != null )
              {
                pageCommand.AddParameter ( EdRecord.FieldNames.TypeId,
                  this.Session.Entity.TypeId );
              }
              return pageCommand;
            }

        }//END switch
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        #endregion

      }//END Static page ids.

      //
      // Create the command to to display a page layout..
      //
      if ( PageId.Contains ( EuAdapter.CONST_PAGE_ID_PREFIX ) == true )
      {
        string stPageId = PageId.Replace ( EuAdapter.CONST_PAGE_ID_PREFIX, String.Empty );

        this.LogDebug ( "PAGE: PageId: {0}, Title: {1} StPageId: {2}.", PageId, Title, stPageId );

        pageCommand = new Evado.UniForm.Model.EuCommand (
          Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Page.ToString ( ),
          Evado.UniForm.Model.EuMethods.List_of_Objects );

        pageCommand.SetPageId ( stPageId );
        pageCommand.AddParameter ( EuEntities.CONST_HIDE_SELECTION, "Yes" );

        return pageCommand;
      }

      //
      // Create the command to query Entities as a filtered list  
      // i.e. retrieves an organisation's child entity layout.
      //
      if ( PageId.Contains ( EuAdapter.CONST_AUTHOR_PAGE_ID_SUFFIX ) == true )
      {

        string layoutId = PageId.Replace ( EuAdapter.CONST_ENTITY_PREFIX, String.Empty );
        layoutId = layoutId.Replace ( EuAdapter.CONST_AUTHOR_PAGE_ID_SUFFIX, String.Empty );

        this.LogDebug ( "AUTHOR: PageId: {0}, Title: {1} Layout: {2}, Author Query.", PageId, Title, layoutId );

        pageCommand = new Evado.UniForm.Model.EuCommand (
          Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Entities.ToString ( ),
          Evado.UniForm.Model.EuMethods.List_of_Objects );

        pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Entity_View );
        pageCommand.AddParameter ( EdRecord.FieldNames.Layout_Id, layoutId );
        pageCommand.AddParameter ( EdRecord.FieldNames.Author, this.Session.UserProfile.UserId );
        pageCommand.AddParameter ( EdRecord.FieldNames.ParentUserId, this.Session.UserProfile.UserId );
        pageCommand.AddParameter ( EuEntities.CONST_HIDE_SELECTION, "Yes" );
        pageCommand.AddParameter ( EuEntities.CONST_AUTHOR_SELECTION, "Yes" );
        
        return pageCommand;
      }

      //
      // Create the command to query Entities as a filtered list  
      // i.e. retrieves an organisation's child entity layout.
      //
      if ( PageId.Contains ( EuAdapter.CONST_ENTITY_FILTERED_LIST_SUFFIX ) == true )
      {

        string layoutId = PageId.Replace ( EuAdapter.CONST_ENTITY_PREFIX, String.Empty );
        layoutId = layoutId.Replace ( EuAdapter.CONST_ENTITY_FILTERED_LIST_SUFFIX, String.Empty );

        this.LogDebug ( "FILTERED: PageId: {0}, Title: {1} Layout: {2}, Filtered Query.", PageId, Title, layoutId );

        pageCommand = new Evado.UniForm.Model.EuCommand (
          Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Entities.ToString ( ),
          Evado.UniForm.Model.EuMethods.List_of_Objects );

        pageCommand.SetPageId ( Evado.Digital.Model.EdStaticPageIds.Entity_Filter_View );
        pageCommand.AddParameter ( EdRecord.FieldNames.Layout_Id, layoutId );
        pageCommand.AddParameter ( EuEntities.CONST_EMPTY_SELECTION_FIELD, "Yes" ); 

        return pageCommand;
      }

      //
      // Create the command to access a Entity by its organisation parent's identifier.  
      // i.e. retrieves an organisation's child entity layout.
      //
      if ( PageId.Contains ( EuAdapter.CONST_ORG_PARENT_PAGE_ID_SUFFIX2 ) == true )
      {
        string layoutId = PageId.Replace ( EuAdapter.CONST_ENTITY_PREFIX, String.Empty );

        this.LogDebug ( "1 layoutId: {0}.", layoutId );

        layoutId = layoutId.Replace ( EuAdapter.CONST_ORG_PARENT_PAGE_ID_SUFFIX2, String.Empty );

        this.LogDebug ( "2 Template: {0}.", EuAdapter.CONST_ORG_PARENT_PAGE_ID_SUFFIX2 );
        this.LogDebug ( "2 layoutId: {0}.", layoutId );

        this.LogDebug ( "ORG: PageId: {0}, Title: {1} Layout: {2}, Org Parent.", PageId, Title, layoutId );

        pageCommand = new Evado.UniForm.Model.EuCommand (
          Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Entities.ToString ( ),
          Evado.UniForm.Model.EuMethods.Get_Object );

        pageCommand.SetPageId ( PageId );
        pageCommand.AddParameter ( EdRecord.FieldNames.Layout_Id, layoutId );
        pageCommand.AddParameter ( EuEntities.CONST_HIDE_SELECTION, "Yes" );
        pageCommand.AddParameter (
          EdRecord.FieldNames.ParentOrgId,
          this.Session.Organisation.OrgId );

        this.LogDebug ( "Command Method: {0}.", pageCommand.Method );

        return pageCommand;
      }

      //
      // Create the command to access a Entity by its organisation parent's identifier.  
      // i.e. retrieves an organisation's child entity layout.
      //
      if ( PageId.Contains ( EuAdapter.CONST_ORG_PARENT_PAGE_ID_SUFFIX ) == true )
      {
        string layoutId = PageId.Replace ( EuAdapter.CONST_ENTITY_PREFIX, String.Empty );

        this.LogDebug ( "1 layoutId: {0}.", layoutId );

        layoutId = layoutId.Replace ( EuAdapter.CONST_ORG_PARENT_PAGE_ID_SUFFIX, String.Empty );

        this.LogDebug ( "2 Template: {0}.", EuAdapter.CONST_ORG_PARENT_PAGE_ID_SUFFIX );
        this.LogDebug ( "2 layoutId: {0}.", layoutId );

        this.LogDebug ( "ORG: PageId: {0}, Title: {1} Layout: {2}, Org Parent.", PageId, Title, layoutId );

        pageCommand = new Evado.UniForm.Model.EuCommand (
          Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Entities.ToString ( ),
          Evado.UniForm.Model.EuMethods.List_of_Objects );

        pageCommand.SetPageId ( PageId );
        pageCommand.AddParameter ( EdRecord.FieldNames.Layout_Id, layoutId );
        pageCommand.AddParameter ( EuEntities.CONST_HIDE_SELECTION, "Yes" );
        pageCommand.AddParameter (
          EdRecord.FieldNames.ParentOrgId,
          this.Session.Organisation.OrgId );

        return pageCommand;
      }

      //
      // Create the command to access a Entity by its user parent's identifier.  
      // i.e. retrieves an user's child entity layout.
      //
      else if ( PageId.Contains ( EuAdapter.CONST_USER_PARENT_PAGE_ID_SUFFIX2 ) == true )
      {
        string layoutId = PageId.Replace ( EuAdapter.CONST_ENTITY_PREFIX, String.Empty );

        this.LogDebug ( "1 layoutId: {0}.", layoutId );
        layoutId = layoutId.Replace ( EuAdapter.CONST_USER_PARENT_PAGE_ID_SUFFIX2, String.Empty );

        this.LogDebug ( "2 layoutId: {0}.", layoutId );

        this.LogDebug ( "DEF USR: PageId: {0}, Title: {1} Layout: {2}, User  Parent.", PageId, Title, layoutId );

        pageCommand = new Evado.UniForm.Model.EuCommand (
          Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Entities.ToString ( ),
          Evado.UniForm.Model.EuMethods.Get_Object );

        pageCommand.SetPageId ( PageId );
        pageCommand.AddParameter ( EdRecord.FieldNames.Layout_Id, layoutId );
        pageCommand.AddParameter ( EuEntities.CONST_HIDE_SELECTION, "Yes" );
        pageCommand.AddParameter (
          EdRecord.FieldNames.ParentUserId,
          this.Session.UserProfile.UserId );

        return pageCommand;
      }

      //
      // Create the command to access a Entity by its user parent's identifier.  
      // i.e. retrieves an user's child entity layout.
      //
      else if ( PageId.Contains ( EuAdapter.CONST_USER_PARENT_PAGE_ID_SUFFIX ) == true )
      {
        string layoutId = PageId.Replace ( EuAdapter.CONST_ENTITY_PREFIX, String.Empty );
        layoutId = layoutId.Replace ( EuAdapter.CONST_USER_PARENT_PAGE_ID_SUFFIX, String.Empty );

        this.LogDebug ( "USER: PageId: {0}, Title: {1} Layout: {2}, User  Parent.", PageId, Title, layoutId );

        pageCommand = new Evado.UniForm.Model.EuCommand (
          Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Entities.ToString ( ),
          Evado.UniForm.Model.EuMethods.List_of_Objects );

        pageCommand.SetPageId ( PageId );
        pageCommand.AddParameter ( EdRecord.FieldNames.Layout_Id, layoutId );
        pageCommand.AddParameter ( EuEntities.CONST_HIDE_SELECTION, "Yes" );
        pageCommand.AddParameter (
          EdRecord.FieldNames.ParentUserId,
          this.Session.UserProfile.UserId );

        return pageCommand;
      }

      //
      // Create the command to access a Entity by its Entity parent.  
      // i.e. retrieves an Entity's child records layout.
      //
      else if ( PageId.Contains ( EuAdapter.CONST_ENTITY_PARENT_PAGE_ID_SUFFIX ) == true )
      {
        this.LogDebug ( "A PageId: {0}, Title: {1} Entity Parent.", PageId, Title ); 

        string layoutId = PageId.Replace ( EuAdapter.CONST_ENTITY_PARENT_PAGE_ID_SUFFIX, String.Empty );

        this.LogDebug ( "1 layoutId: {0}.", layoutId );

        layoutId = layoutId.Replace ( EuAdapter.CONST_ENTITY_PREFIX, String.Empty );

        this.LogDebug ( "2 layoutId: {0}.", layoutId );

        this.LogDebug ( "PARENT: PageId: {0}, Title: {1} Layout: {2}, User  Parent.", PageId, Title, layoutId );
        pageCommand = new Evado.UniForm.Model.EuCommand (
          Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Entities.ToString ( ),
          Evado.UniForm.Model.EuMethods.List_of_Objects );

        pageCommand.SetPageId ( PageId );
        pageCommand.AddParameter ( EdRecord.FieldNames.Layout_Id, layoutId );
        pageCommand.AddParameter ( EuEntities.CONST_HIDE_SELECTION, "Yes" );
        pageCommand.AddParameter (
          EdRecord.FieldNames.ParentGuid,
          this.Session.Entity.Guid );
        pageCommand.AddParameter (
          EdRecord.FieldNames.ParentLayoutId,
          this.Session.Entity.LayoutId ); 

        return pageCommand;
      }

      //
      // Create the command to access a Entity by its Entity parent.  
      // i.e. retrieves an Entity's child records layout.
      //
      else if ( PageId.Contains ( EuAdapter.CONST_ENTITY_PARENT_PAGE_ID_SUFFIX ) == true )
      {
        this.LogDebug ( "A PageId: {0}, Title: {1} Entity Parent.", PageId, Title );

        string layoutId = PageId.Replace ( EuAdapter.CONST_ENTITY_PARENT_PAGE_ID_SUFFIX, String.Empty );

        this.LogDebug ( "1 layoutId: {0}.", layoutId );

        layoutId = layoutId.Replace ( EuAdapter.CONST_ENTITY_PREFIX, String.Empty );

        this.LogDebug ( "2 layoutId: {0}.", layoutId );

        pageCommand = new Evado.UniForm.Model.EuCommand (
          Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Entities.ToString ( ),
          Evado.UniForm.Model.EuMethods.List_of_Objects );

        pageCommand.SetPageId ( PageId );
        pageCommand.AddParameter ( EdRecord.FieldNames.Layout_Id, layoutId );
        pageCommand.AddParameter ( EuEntities.CONST_HIDE_SELECTION, "Yes" );
        pageCommand.AddParameter (
          EdRecord.FieldNames.ParentGuid,
          this.Session.Entity.Guid );

        return pageCommand;
      }
      //
      // Create the command to access Entities by their layout identifers.
      //
      else if ( PageId.Contains ( EuAdapter.CONST_ENTITY_PREFIX ) == true )
      {
        this.LogDebug ( "PageId: {0}, Title: {1} Layout.", PageId, Title ); 
        string layoutId = PageId.Replace ( EuAdapter.CONST_ENTITY_PREFIX, String.Empty );

        pageCommand = new Evado.UniForm.Model.EuCommand (
          Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Entities.ToString ( ),
          Evado.UniForm.Model.EuMethods.List_of_Objects );

        pageCommand.SetPageId ( PageId );
        pageCommand.AddParameter ( EuEntities.CONST_HIDE_SELECTION, "Yes" );
        pageCommand.AddParameter ( EdRecord.FieldNames.Layout_Id, layoutId );

        return pageCommand;

      }

      //
      // Create the command to access records by their layout identifers.
      //
      if ( PageId.Contains ( EuAdapter.CONST_RECORD_PREFIX) == true )
      {
        string layoutId = PageId.Replace ( EuAdapter.CONST_RECORD_PREFIX, String.Empty );

        pageCommand = new Evado.UniForm.Model.EuCommand (
          Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Entities.ToString ( ),
          Evado.UniForm.Model.EuMethods.Get_Object );

        pageCommand.SetPageId ( PageId );
        pageCommand.AddParameter ( EdRecord.FieldNames.Layout_Id, layoutId );

        return pageCommand;

      }
      //
      // Create the command to access a record by its organisation parent's identifier.  
      // i.e. retrieves an organisation's child entity layout.
      //
      else if ( PageId.Contains ( EuAdapter.CONST_ORG_PARENT_PAGE_ID_SUFFIX ) == true )
      {
        string layoutId = PageId.Replace ( EuAdapter.CONST_ORG_PARENT_PAGE_ID_SUFFIX, String.Empty );

        pageCommand = new Evado.UniForm.Model.EuCommand (
          Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Records.ToString ( ),
          Evado.UniForm.Model.EuMethods.Get_Object );

        pageCommand.SetPageId ( PageId );
        pageCommand.AddParameter ( EdRecord.FieldNames.Layout_Id, layoutId );
        pageCommand.AddParameter (
          EdRecord.FieldNames.ParentOrgId,
          this.Session.Organisation.OrgId );

        return pageCommand;
      }

      //
      // Create the command to access a record by its user parent's identifier.  
      // i.e. retrieves an user's child entity layout.
      //
      else if ( PageId.Contains ( EuAdapter.CONST_ORG_PARENT_PAGE_ID_SUFFIX ) == true )
      {
        string layoutId = PageId.Replace ( EuAdapter.CONST_ORG_PARENT_PAGE_ID_SUFFIX, String.Empty );

        pageCommand = new Evado.UniForm.Model.EuCommand (
          Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Records.ToString ( ),
          Evado.UniForm.Model.EuMethods.Get_Object );

        pageCommand.SetPageId ( PageId );
        pageCommand.AddParameter ( EdRecord.FieldNames.Layout_Id, layoutId );
        pageCommand.AddParameter (
          EdRecord.FieldNames.ParentUserId,
          this.Session.UserProfile.UserId );

        return pageCommand;
      }

      //
      // Create the command to access a record by its Entity parent.  
      // i.e. retrieves an Entity's child records layout.
      //
      else if ( PageId.Contains ( EuAdapter.CONST_ENTITY_PARENT_PAGE_ID_SUFFIX ) == true )
      {
        string layoutId = PageId.Replace ( EuAdapter.CONST_ENTITY_PARENT_PAGE_ID_SUFFIX, String.Empty );

        pageCommand = new Evado.UniForm.Model.EuCommand (
          Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Records.ToString ( ),
          Evado.UniForm.Model.EuMethods.List_of_Objects );

        pageCommand.SetPageId ( PageId );
        pageCommand.AddParameter ( EdRecord.FieldNames.Layout_Id, layoutId );
        pageCommand.AddParameter ( EuEntities.CONST_HIDE_SELECTION, "Yes" );
        pageCommand.AddParameter (
          EdRecord.FieldNames.ParentGuid,
          this.Session.Entity.Guid );

        return pageCommand;
      }

      return null;

    }//END convertMenuItem method
  }//END CLASS
}//END namespace