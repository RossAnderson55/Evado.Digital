/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\ApplicationService.cs" 
 *  company="EVADO HOLDING PTY. LTD.">
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
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Configuration;

using Evado.Bll;
using Evado.Model;
using Evado.Bll.Digital;
using Evado.Model.Digital;
/// using Evado.Web;


namespace Evado.UniForm.Digital
{
  public partial class EuAdapter : Evado.Model.UniForm.ApplicationAdapterBase
  {

    // ==================================================================================
    /// <summary>
    /// This method gets the application object from the list.
    /// 
    /// </summary>
    /// <returns>ClientApplicationData</returns>
    // ----------------------------------------------------------------------------------
    private bool loadUserProfile (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "loadUserProfile" );
      this.LogDebug ( "ServiceUserProfile UserId: " + this.ServiceUserProfile.UserId );

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Bll.Digital.EdUserprofiles userProfiles =
        new Evado.Bll.Digital.EdUserprofiles ( this.ClassParameters );
      //
      // if an anonoymous command is encountered create a user profile for a patient.
      //
      if ( PageCommand.Type == Evado.Model.UniForm.CommandTypes.Anonymous_Command
        || this.ServiceUserProfile.UserAuthenticationState == EvUserProfileBase.UserAuthenticationStates.Anonymous_Access )
      {
        this.LogEvent ( "Anonymous command encountered" );

        this.Session.UserProfile = new EdUserProfile ( );
        this.Session.UserProfile.UserId = this.ServiceUserProfile.UserId;
        this.Session.UserProfile.CommonName = this.ServiceUserProfile.UserId;
        this.Session.UserProfile.Roles = String.Empty;

        this.ClassParameters.UserProfile = this.Session.UserProfile;

        return true;
      }

      // 
      // Not user id exist false.
      // 
      if ( this.ServiceUserProfile.UserId == String.Empty )
      {
        this.LogEvent ( "Error: User DOES NOT EXIST" );

        return false;
      }

      // 
      // IF the profile exists and matches the current user return true.
      // 
      if ( this.Session.UserProfile.UserId.ToLower ( ) == this.ServiceUserProfile.UserId.ToLower ( ) )
      {
        this.ClassParameters.UserProfile = this.Session.UserProfile;

        this.LogDebug ( "User profile for{0} EXISTS IN SESSION", this.ServiceUserProfile.UserId );

        return true;
      }

      // 
      // Try to get the user profile form the passed user profile object.
      // 
      else if ( this.Session.UserProfile.UserId == String.Empty )
      {
        this.LogDebug ( "User profile for {0} IS EMPTY GENERATING NEW PROFILE.", this.ServiceUserProfile.UserId );

        this.Session.UserProfile = userProfiles.getItem ( this.ServiceUserProfile.UserId );

        this.LogDebugClass ( userProfiles.Log );
      }

      // 
      // if the user profile does not exist exit false.
      // 
      if ( this.Session.UserProfile.ExpiryDate <= DateTime.Now )
      {
        this.LogEvent ( "User profile for " + this.ServiceUserProfile.UserId + " HAS EXPIRED." );

        return false;
      }

      // 
      // if the user profile does not exist exit false.
      // 
      if ( this.Session.UserProfile.UserId == String.Empty )
      {
        this.LogAction ( "User profile for " + this.ServiceUserProfile.UserId + " DOES NOT EXIST." );

        return false;
      }

      this.LogAction ( "User profile for " + this.ServiceUserProfile.UserId + " GENERATED" );

      this.ClassParameters.UserProfile = this.Session.UserProfile;

      return true;

    }//END loadUserProfile method.

    // ==================================================================================
    /// <summary>
    /// This method gets the application object from the list.
    /// 
    /// </summary>
    /// <returns>ClientApplicationData</returns>
    // ----------------------------------------------------------------------------------
    private bool loadUserOrganisation ( )
    {
      this.LogMethod ( "loadUserOrganisation" );
      this.LogDebug ( "User OrgId: " + this.Session.UserProfile.OrgId );

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Bll.Digital.EdOrganisations bll_Organisations =
        new Evado.Bll.Digital.EdOrganisations ( this.ClassParameters );

      // 
      // IF the profile exists and matches the current user return true.
      // 
      if ( this.Session.Organisation.OrgId == this.Session.UserProfile.OrgId )
      {
        this.LogDebug ( "Organisation for {0} EXISTS IN SESSION", this.Session.Organisation.OrgId );

        this.LogMethodEnd ( "loadUserOrganisation" );
        return true;
      }

      // 
      // Try to get the user profile form the passed user profile object.
      // 
      else if ( this.Session.Organisation.OrgId == String.Empty )
      {
        this.LogDebug ( "Organisation for {0} IS EMPTY GENERATING NEW PROFILE.", this.Session.UserProfile.OrgId );

        this.Session.Organisation = bll_Organisations.getItem ( this.Session.UserProfile.OrgId );

        this.LogDebugClass ( bll_Organisations.Log );
      }

      // 
      // if the user profile does not exist exit false.
      // 
      if ( this.Session.Organisation.OrgId == String.Empty )
      {
        this.LogAction ( "Organisation for " + this.Session.Organisation.OrgId + " DOES NOT EXIST." );

        this.LogMethodEnd ( "loadUserOrganisation" );
        return false;
      }

      this.LogAction ( "Organisation for " + this.Session.Organisation.OrgId + " GENERATED" );

      this.LogMethodEnd ( "loadUserOrganisation" );
      return true;

    }//END loadUserProfile method.

    //===================================================================================
    /// <summary>
    /// This method executes the form list query 
    /// </summary>
    //-----------------------------------------------------------------------------------
    private void loadOrganisationList ( )
    {
      this.LogInitMethod ( "loadOrganisationList" );

      if ( this._AdapterObjects.OrganisationList == null )
      {
        this._AdapterObjects.OrganisationList = new List<EdOrganisation> ( );
      }

      if ( this._AdapterObjects.OrganisationList.Count > 0 )
      {
        this.LogInit ( "Organistion list layout loaded." );
        this.LogInit ( "END loadOrganisationList" );
        return;
      }

      //
      // Initialise the methods variables and object.
      //
      EdOrganisations bll_Organisations = new EdOrganisations ( this.ClassParameters );
      bll_Organisations.ClassParameter.LoggingLevel = 2;

      // 
      // Query the database to retrieve a list of the records matching the query parameter values.
      // 
      this._AdapterObjects.OrganisationList = bll_Organisations.getView ( );

      this.LogInitClass ( bll_Organisations.Log );

      this.LogInit ( "Organisation list count: " + this._AdapterObjects.OrganisationList.Count );

      this.LogInit ( "END loadOrganisationList" );

    }//END loadTrialFormList method

    //===================================================================================
    /// <summary>
    /// This method executes the form list query 
    /// </summary>
    //-----------------------------------------------------------------------------------
    private void loadEnityLayoutList ( )
    {
      this.LogInitMethod ( "loadEnityLayoutList" );
      this.LogDebug ( "AllEntityLayouts.Count: " + this._AdapterObjects.AllEntityLayouts.Count );

      //
      // Exit the method if the list exists or the loaded entiy layout is false.
      //
      if ( this._AdapterObjects.AllEntityLayouts.Count > 0 )
      {
        this.LogInit ( "Entity layouts loaded." );
        this.LogInit ( "END loadRecordLayoutList" );
        return;
      }

      //
      // Initialise the methods variables and object.
      //
      EdEntityLayouts bll_EntityLayouts = new EdEntityLayouts ( this.ClassParameters );
      bll_EntityLayouts.ClassParameter.LoggingLevel = 2;

      // 
      // Query the database to retrieve a list of the records matching the query parameter values.
      // 
      this._AdapterObjects.AllEntityLayouts = bll_EntityLayouts.GetRecordLayoutListWithFields (
        EdRecordTypes.Null,
        EdRecordObjectStates.Null );

      this.LogInitClass ( bll_EntityLayouts.Log );

      this.LogInit ( "AllEntityLayouts.Count: " + this._AdapterObjects.AllEntityLayouts.Count );

      //
      // update the page identifiers.
      //
      this.LoadPageIdentifiers ( );

      this.LogInit ( "END  loadEnityLayoutList" );

    }//END loadEnityLayoutList method

    //===================================================================================
    /// <summary>
    /// This method executes the form list query 
    /// </summary>
    //-----------------------------------------------------------------------------------
    private void loadRecordLayoutList ( )
    {
      this.LogInitMethod ( "loadRecordLayoutList" );
      this.LogInit ( "AllRecordLayouts.Count: " + this._AdapterObjects.AllRecordLayouts.Count );

      //
      // Exit the method if the list exists or the loaded entiy layout is false.
      //
      if ( this._AdapterObjects.AllRecordLayouts.Count > 0 )
      {
        this.LogInit ( "Record layouts loaded." );
        this.LogInit ( "END loadRecordLayoutList" );
        return;
      }

      //
      // Initialise the methods variables and object.
      //
      EdRecordLayouts bll_RecordLayouts = new EdRecordLayouts ( this.ClassParameters );
      bll_RecordLayouts.ClassParameter.LoggingLevel = 2;

      // 
      // Query the database to retrieve a list of the records matching the query parameter values.
      // 
      this._AdapterObjects.AllRecordLayouts = bll_RecordLayouts.GetRecordLayoutListWithFields (
        EdRecordTypes.Null,
        EdRecordObjectStates.Null );

      this.LogInitClass ( bll_RecordLayouts.Log );

      this.LogInit ( "AllRecordLayouts.Count: " + this._AdapterObjects.AllRecordLayouts.Count );

      //
      // update the page identifiers.
      //
      this.LoadPageIdentifiers ( );

      this.LogInit ( "END loadRecordLayoutList" );

    }//END loadRecordLayoutList method

    //===================================================================================
    /// <summary>
    /// This method executes the form list query 
    /// </summary>
    //-----------------------------------------------------------------------------------
    private void loadSelectionLists ( )
    {
      this.LogInitMethod ( "loadSelectionLists" );
      this.LogInit ( "SelectionLists.Count: " + this._AdapterObjects.SelectionLists.Count );

      //
      // Exit the method if the list exists or the loaded entiy layout is false.
      //
      if ( this._AdapterObjects.SelectionLists.Count > 0 )
      {
        this.LogInit ( "Record layouts loaded." );
        this.LogInit ( "END loadSelectionLists" );
        return;
      }

      //
      // Initialise the methods variables and object.
      //
      EdSelectionLists bll_SelectionLists = new EdSelectionLists ( this.ClassParameters );
      bll_SelectionLists.ClassParameter.LoggingLevel = 2;

      // 
      // Query the database to retrieve a list of the selection lists that are issued.
      // 
      this._AdapterObjects.SelectionLists = bll_SelectionLists.getView ( EdSelectionList.SelectionListStates.Issued );

      this.LogInitClass ( bll_SelectionLists.Log );

      this.LogInit ( "SelectionLists.Count: " + this._AdapterObjects.SelectionLists.Count );

      this.LogInit ( "END loadSelectionLists" );

    }//END loadSelectionLists method

    ///  =======================================================================================
    /// <summary>
    /// This method loads the navigational command list for all static, entities and record layouts
    /// </summary>
    //  ---------------------------------------------------------------------------------
    public void LoadPageIdentifiers ( )
    {
      this.LogInitMethod( "LoadPageIdentifiers method" );
      //
      // Initialise the methods variables and objects.
      //
      this._AdapterObjects.PageIdentifiers = new List<EvOption> ( );

      this._AdapterObjects.PageIdentifiers.Add ( new EvOption ( ) );

      //
      // add the static page identifiers.
      //

      this.LogInit ( "Generating the Static PageId list" );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Home_Page ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Access_Denied ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Alert_View ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Alert_Page ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Ancillary_Record_View ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Ancillary_Record_Page ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Application_Event ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Application_Event_View ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Application_Profile ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Database_Version ) );

      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Data_Dictionary_View ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Data_Dictionary_Page ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Data_Dictionary_Upload ) );

      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Form_Draft_Page ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Form_Properties_Page ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Form_Properties_Section_Page ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Form_Field_Page ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Form_Template_Upload ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Form_Template_Download ) );

      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Email_Templates_Page ) );

      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Entity_Page ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Entity_Admin_Page ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Entity_Export_Page ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Entity_Layout_View ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Entity_Layout_Page ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Entity_Query_View ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Entity_View ) );

      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Login_Page ) );

      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Menu_View ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Menu_Page ) );

      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Operational_Report_Page ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Operational_Report_List ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Organisation_Page ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Organisation_View ) );


      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Report_Saved_View ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Report_Template_Page ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Report_Template_Column_Selection_Page ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Report_Template_View ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Report_Template_Upload ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Report_Template_Download ) );

      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Record_Admin_Page ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Record_Page ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Records_View ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Record_Layout_View ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Record_Layout_Page ) );

      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Selection_List_Upload ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Selection_List_Page ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Selection_List_View ) );

      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.User_DownLoad_Page ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.User_Profile_Page ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.User_Profile_Update_Page ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.User_Profile_Password_Page ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.User_Upload_Page ) );
      this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.User_View ) );


      this.LogInit ( "Generating the Entities PageId list" );
      //
      // dynamic page identifiers for Entities by LayoutId
      //
      foreach ( EdRecord entityLayout in this._AdapterObjects.AllEntityLayouts )
      {
        if ( entityLayout.State != EdRecordObjectStates.Form_Issued )
        {
          continue;
        }

        this.LogInit ( "{0} - {1} > ParentType {2} ", entityLayout.LayoutId, entityLayout.Title, entityLayout.Design.ParentType );

        String pageId = EuAdapter.CONST_ENTITY_PAGE_ID_PREFIX + entityLayout.LayoutId;
        String pageLabel = pageId.Replace ( "_", " " );

        this.LogInit ( "Layout PageID: " + pageId );

        this._AdapterObjects.PageIdentifiers.Add ( new EvOption ( pageId, pageLabel ) );

        //
        // add the page identifier for child entities.
        //
        switch ( entityLayout.Design.ParentType )
        {
          case EdRecord.ParentTypeList.Organisation:
            {

              pageId = EuAdapter.CONST_ENTITY_PAGE_ID_PREFIX + entityLayout.LayoutId +EuAdapter.CONST_ORG_PARENT_PAGE_ID_SUFFIX;
              pageLabel = pageId.Replace ( "_", " " );

              this.LogInit ( "Org PageID: " + pageId );

              this._AdapterObjects.PageIdentifiers.Add ( new EvOption ( pageId, pageLabel ) );
              break;
            }
          case EdRecord.ParentTypeList.User:
            {
              pageId = EuAdapter.CONST_ENTITY_PAGE_ID_PREFIX + entityLayout.LayoutId + EuAdapter.CONST_USER_PARENT_PAGE_ID_SUFFIX;
              pageLabel = pageId.Replace ( "_", " " );

              this.LogInit ( "User PageID: " + pageId );

              this._AdapterObjects.PageIdentifiers.Add ( new EvOption ( pageId, pageLabel ) );

              break;
            }
          case EdRecord.ParentTypeList.Entity:
            {
              pageId = EuAdapter.CONST_ENTITY_PAGE_ID_PREFIX + entityLayout.LayoutId + EuAdapter.CONST_ENTITY_PARENT_PAGE_ID_SUFFIX;
              pageLabel = pageId.Replace ( "_", " " );

              this.LogInit ( "Entity PageID: " + pageId );

              this._AdapterObjects.PageIdentifiers.Add ( new EvOption ( pageId, pageLabel ) );

              break;
            }
        }//END switch statement.

      }//END Entity list iteration

      this.LogInit ( "Generating the Records PageId list" );
      //
      // dynamic page identifiers for Entities by LayoutId
      //
      foreach ( EdRecord recordLayouts in this._AdapterObjects.AllRecordLayouts )
      {
        if ( recordLayouts.State != EdRecordObjectStates.Form_Issued )
        {
          continue;
        }

        String pageId = EuAdapter.CONST_RECORD_PAGE_ID_PREFIX + recordLayouts.LayoutId;
        String pageLabel = pageId.Replace ( "_", " " );

        this._AdapterObjects.PageIdentifiers.Add ( new EvOption ( pageId, pageLabel ) );

        //
        // add the page identifier for child entities.
        //
        switch ( recordLayouts.Design.ParentType )
        {
          case EdRecord.ParentTypeList.Organisation:
            {
              pageId = EuAdapter.CONST_RECORD_PAGE_ID_PREFIX + recordLayouts.LayoutId + EuAdapter.CONST_ORG_PARENT_PAGE_ID_SUFFIX;
              pageLabel = pageId.Replace ( "_", " " );

              this.LogInit ( "PageID: " + pageId );

              this._AdapterObjects.PageIdentifiers.Add ( new EvOption ( pageId, pageLabel ) );

              break;
            }
          case EdRecord.ParentTypeList.User:
            {
              pageId = EuAdapter.CONST_RECORD_PAGE_ID_PREFIX + recordLayouts.LayoutId + EuAdapter.CONST_USER_PARENT_PAGE_ID_SUFFIX;
              pageLabel = pageId.Replace ( "_", " " );

              this.LogInit ( "PageID: " + pageId );

              this._AdapterObjects.PageIdentifiers.Add ( new EvOption ( pageId, pageLabel ) );

              break;
            }
          case EdRecord.ParentTypeList.Entity:
            {
              pageId = EuAdapter.CONST_RECORD_PAGE_ID_PREFIX + recordLayouts.LayoutId + EuAdapter.CONST_ENTITY_PARENT_PAGE_ID_SUFFIX;
              pageLabel = pageId.Replace ( "_", " " );

              this.LogInit ( "PageID: " + pageId );

              this._AdapterObjects.PageIdentifiers.Add ( new EvOption ( pageId, pageLabel ) );

              break;
            }
        }//END switch statement.

      }//END Record list iteration 

      this.LogInit ( "PageIdentifiers.Count: " + this._AdapterObjects.PageIdentifiers.Count );

      this.LogInitMethodEnd ( "LoadPageIdentifiers method" );

    }//END LoadPageIdentifiers method

  }///END EuAdapter class

}///END NAMESPACE
