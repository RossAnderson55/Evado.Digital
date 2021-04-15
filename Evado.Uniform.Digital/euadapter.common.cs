/***************************************************************************************
 * <copyright file="Evado.UniForm.Digital\ApplicationService.cs" 
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

        this.LogDebug ( "User profile for {0} EXISTS IN SESSION", this.ServiceUserProfile.UserId );

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

      this.Session.UserProfile.OrgType = this.Session.Organisation.OrgType;
      this.Session.UserProfile.OrganisationName = this.Session.Organisation.Name;

      this.LogDebug ( "Org Type " + this.Session.Organisation.OrgType );

      this.LogAction ( "Organisation for " + this.Session.Organisation.OrgId + " GENERATED" );

      this.LogMethodEnd ( "loadUserOrganisation" );
      return true;

    }//END loadUserProfile method.

    // ==================================================================================
    /// <summary>
    /// This method gets the application object from the list.
    /// 
    /// </summary>
    /// <returns>ClientApplicationData</returns>
    // ----------------------------------------------------------------------------------
    private bool loadDefaultChildEntity ( )
    {
      this.LogMethod ( "loadDefaultChildEntity" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Bll.Digital.EdEntities bll_Entities =
        new Evado.Bll.Digital.EdEntities ( this.ClassParameters );
      bool defaultEnityFound = false;

      //
      // IF the dictionary is empty load the first entity if there is one for the user or organisation.
      //
      if ( this.Session.EntityDictionary.Count > 0 )
      {
        this.LogMethodEnd ( "loadDefaultChildEntity" );
        return true ;
      }

      //
      // iterate through the list of entitiy layouts.
      //
      foreach ( EdRecord layout in EuAdapter.AdapterObjects.AllEntityLayouts )
      {
        if ( defaultEnityFound == true
          && layout.Design.ParentType != EdRecord.ParentTypeList.User_Default
          && layout.Design.ParentType != EdRecord.ParentTypeList.Organisation_Default )
        {
          continue;
        }

        if ( layout.hasReadAccess(  this.Session.UserProfile.Roles ) == false )
        {
          continue;
        }

        //this.LogDebug ( "{0} - {1} PT: {2}.", layout.LayoutId, layout.Title, layout.Design.ParentType );
        //
        // try and retrieve the user's child entity.
        //
        if ( layout.Design.ParentType == EdRecord.ParentTypeList.User_Default )
        {
          this.LogDebug ( "User Default child" );

          var entity = bll_Entities.GetItemByParentUserId (
            layout.LayoutId,
            this.Session.UserProfile.UserId );

          if ( entity.Guid != Guid.Empty )
          {
            this.Session.Entity = entity;
            this.Session.PushEntity ( this.Session.Entity );
            //defaultEnityFound = true;
            break;
          }
        }
        else
        {
          this.LogDebug ( "User Default child" );
          //
          // try and retrieve the user's org entity.
          //
          var entity = bll_Entities.GetItemByParentOrgId (
            layout.LayoutId,
            this.Session.UserProfile.OrgId );

          if ( entity.Guid != Guid.Empty )
          {
            this.Session.Entity = entity;
            this.Session.PushEntity ( this.Session.Entity );
            //defaultEnityFound = true;

            break;
          }
        }

      }//END iteration loop

      this.LogDebug ( "{0} - {1}.", this.Session.Entity.EntityId, this.Session.Entity.CommandTitle );
      this.LogMethodEnd ( "loadDefaultChildEntity" );
      return true;

    }//END loadDefaultChildEntity method.

    //===================================================================================
    /// <summary>
    /// This method executes the form list query 
    /// </summary>
    //-----------------------------------------------------------------------------------
    private void loadOrganisationList ( )
    {
      this.LogInitMethod ( "loadOrganisationList" );


      if ( EuAdapter.AdapterObjects.OrganisationList.Count > 0 )
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
      EuAdapter.AdapterObjects.OrganisationList = bll_Organisations.getView ( );

      this.LogInitClass ( bll_Organisations.Log );

      this.LogInit ( "Organisation list count: " + EuAdapter.AdapterObjects.OrganisationList.Count );

      this.LogInit ( "END loadOrganisationList" );

    }//END loadTrialFormList method

    //===================================================================================
    /// <summary>
    /// This method executes the form list query 
    /// </summary>
    //-----------------------------------------------------------------------------------
    private void loadPageLayoutList ( )
    {
      this.LogInitMethod ( "loadPageLayoutList" );
      this.LogDebug ( "AllPageLayouts.Count: " + EuAdapter.AdapterObjects.AllPageLayouts.Count );

      //
      // Exit the method if the list exists or the loaded entiy layout is false.
      //
      if ( EuAdapter.AdapterObjects.AllPageLayouts.Count > 0 )
      {
        this.LogInit ( "Page layouts loaded." );
        this.LogInitMethodEnd ( "loadPageLayoutList" );
        return;
      }

      //
      // Initialise the methods variables and object.
      //
      EdPageLayouts bll_PageLayouts = new EdPageLayouts ( this.ClassParameters );
      bll_PageLayouts.ClassParameter.LoggingLevel = 2;

      // 
      // Query the database to retrieve a list of the records matching the query parameter values.
      // 
      EuAdapter.AdapterObjects.AllPageLayouts = bll_PageLayouts.getView ( EdPageLayout.States.Null );

      this.LogInitClass ( bll_PageLayouts.Log );

      foreach ( EdPageLayout pageLayout in EuAdapter.AdapterObjects.AllPageLayouts )
      {
        this.LogInit ( "{0} - {1} > UserType {2} ", pageLayout.PageId, pageLayout.Title, pageLayout.UserTypes );
      }

      this.LogInit ( "AllPageLayouts.Count: " + EuAdapter.AdapterObjects.AllPageLayouts.Count );

      this.LogInitMethodEnd ( "loadPageLayoutList" );

    }//END loadPageLayoutList method

    //===================================================================================
    /// <summary>
    /// This method executes the form list query 
    /// </summary>
    //-----------------------------------------------------------------------------------
    private void loadEnityLayoutList ( )
    {
      this.LogInitMethod ( "loadEnityLayoutList" );
      this.LogDebug ( "AllEntityLayouts.Count: " + EuAdapter.AdapterObjects.AllEntityLayouts.Count );

      //
      // Exit the method if the list exists or the loaded entiy layout is false.
      //
      if ( EuAdapter.AdapterObjects.AllEntityLayouts.Count > 0 )
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
      EuAdapter.AdapterObjects.AllEntityLayouts = bll_EntityLayouts.GetRecordLayoutListWithFields (
        EdRecordTypes.Null,
        EdRecordObjectStates.Null );

      this.LogInitClass ( bll_EntityLayouts.Log );

      this.LogInit ( "AllEntityLayouts.Count: " + EuAdapter.AdapterObjects.AllEntityLayouts.Count );

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
      this.LogInit ( "AllRecordLayouts.Count: " + EuAdapter.AdapterObjects.AllRecordLayouts.Count );

      //
      // Exit the method if the list exists or the loaded entiy layout is false.
      //
      if ( EuAdapter.AdapterObjects.AllRecordLayouts.Count > 0 )
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
      EuAdapter.AdapterObjects.AllRecordLayouts = bll_RecordLayouts.GetRecordLayoutListWithFields (
        EdRecordTypes.Null,
        EdRecordObjectStates.Null );

      this.LogInitClass ( bll_RecordLayouts.Log );

      this.LogInit ( "AllRecordLayouts.Count: " + EuAdapter.AdapterObjects.AllRecordLayouts.Count );

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
      this.LogInit ( "SelectionLists.Count: " + EuAdapter.AdapterObjects.AllSelectionLists.Count );

      //
      // Exit the method if the list exists or the loaded entiy layout is false.
      //
      if ( EuAdapter.AdapterObjects.AllSelectionLists.Count > 0 )
      {
        this.LogInit ( "No Selection Lists." );
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
      EuAdapter.AdapterObjects.AllSelectionLists = bll_SelectionLists.getView ( EdSelectionList.SelectionListStates.Null );

      this.LogInitClass ( bll_SelectionLists.Log );

      this.LogInit ( "SelectionLists.Count: " + EuAdapter.AdapterObjects.AllSelectionLists.Count );

      this.LogInit ( "END loadSelectionLists" );

    }//END loadSelectionLists method

    //===================================================================================
    /// <summary>
    /// This method executes the form list query 
    /// </summary>
    //-----------------------------------------------------------------------------------
    private List<EdSelectionList> getSelectionLists ( )
    {
      this.LogInitMethod ( "getSelectionLists" );
      this.LogInit ( "SelectionLists.Count: " + EuAdapter.AdapterObjects.AllSelectionLists.Count );

      //
      // Exit the method if the list exists or the loaded entiy layout is false.
      //
      if ( EuAdapter.AdapterObjects.AllSelectionLists.Count == 0 )
      {
        this.LogInit ( "No Selection Lists." );
        this.LogInitMethodEnd ( "getSelectionLists" );
        return new List<EdSelectionList> ( );
      }

      //
      // Initialise methods variables and objects
      //
      List<EdSelectionList> selectionList = new List<EdSelectionList> ( );

      //
      // iterate through the all selection list extracting the issued selection lists.
      //
      foreach ( EdSelectionList list in EuAdapter.AdapterObjects.AllSelectionLists )
      {
        if ( list.State == EdSelectionList.SelectionListStates.Issued )
        {
          selectionList.Add ( list );
        }
      }

      this.LogInit ( "selectionList.Count: " + selectionList.Count );
      this.LogInitMethodEnd ( "getSelectionLists" );
      return selectionList;

    }//END loadSelectionLists method

    ///  =======================================================================================
    /// <summary>
    /// This method loads the navigational page identifiers list for all static, entities and record layouts
    /// </summary>
    //  ---------------------------------------------------------------------------------
    public void LoadPageIdentifiers ( )
    {
      this.LogInitMethod( "LoadPageIdentifiers method" );

      if ( EuAdapter.AdapterObjects.PageIdentifiers == null )
      {
        EuAdapter.AdapterObjects.PageIdentifiers = new List<EvOption> ( );
      }
          

      if( EuAdapter.AdapterObjects.PageIdentifiers.Count > 0 )
      {  
        return;
      }

      //
      // Initialise the methods variables and objects.
      //
      EuAdapter.AdapterObjects.PageIdentifiers = new List<EvOption> ( );

      EuAdapter.AdapterObjects.PageIdentifiers.Add ( new EvOption ( ) );

      //
      // add the static page identifiers.
      //
      #region static page identifiers.
      //this.LogInit ( "Generating the Static PageId list" );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Home_Page ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Access_Denied ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Alert_View ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Alert_Page ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Ancillary_Record_View ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Ancillary_Record_Page ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Application_Event ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Application_Event_View ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Application_Profile ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Database_Version ) );

      //this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Data_Dictionary_View ) );
      //this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Data_Dictionary_Page ) );
      //this._AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Data_Dictionary_Upload ) );

      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Form_Draft_Page ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Form_Properties_Page ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Form_Properties_Section_Page ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Form_Field_Page ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Form_Template_Upload ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Form_Template_Download ) );

      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Email_Templates_Page ) );

      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Entity_Page ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Entity_Admin_Page ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Entity_Export_Page ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Entity_Layout_View ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Entity_Layout_Page ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Entity_Filter_View ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Entity_View ) );

      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Login_Page ) );

      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Menu_View ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Menu_Page ) );

      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Operational_Report_Page ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Operational_Report_List ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Organisation_Page ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Organisation_View ) );

      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Page_Layout_View ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Page_Layout_Page ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Page_Layout_Upload ) );

      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Report_Saved_View ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Report_Template_Page ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Report_Template_Column_Selection_Page ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Report_Template_View ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Report_Template_Upload ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Report_Template_Download ) );

      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Record_Admin_Page ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Record_Page ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Records_View ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Record_Layout_View ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Record_Layout_Page ) );

      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Selection_List_Upload ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Selection_List_Page ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.Selection_List_View ) );

      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.User_DownLoad_Page ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.User_Profile_Page ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.My_User_Profile_Update_Page ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.User_Profile_Password_Page ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.User_Upload_Page ) );
      EuAdapter.AdapterObjects.PageIdentifiers.Add ( EvStatics.getOption ( EdStaticPageIds.User_View ) );

      #endregion


      this.LogInit ( "Generating the PageLayout PageId list" );
      //
      // dynamic page identifiers for Entities by LayoutId
      //
      foreach ( EdPageLayout pageLayout in EuAdapter.AdapterObjects.AllPageLayouts )
      {
        if ( pageLayout.State !=  EdPageLayout.States.Issued )
        {
          continue;
        }

        String pageId = EuAdapter.CONST_PAGE_ID_PREFIX + pageLayout.PageId;
        String pageLabel = pageId.Replace ( "_", " " );

        this.LogInit ( "{0} = {1} - {2} > UserType {3} ", pageId, pageLayout.PageId, pageLayout.Title, pageLayout.UserTypes );

        EuAdapter.AdapterObjects.PageIdentifiers.Add ( new EvOption ( pageId, pageId + " - "+ pageLayout.Title ) );

      }//END page list iteration



      //this.LogInit ( "Generating the Entities PageId list" );
      //
      // dynamic page identifiers for Entities by LayoutId
      //
      foreach ( EdRecord entityLayout in EuAdapter.AdapterObjects.AllEntityLayouts )
      {
        if ( entityLayout.State != EdRecordObjectStates.Form_Issued )
        {
          this.LogInit ( "NOT ISSUED: {0} - {1} > ParentType {2} ", entityLayout.LayoutId, entityLayout.Title, entityLayout.Design.ParentType );
          continue;
        }

        String pageId = EuAdapter.CONST_ENTITY_PREFIX + entityLayout.LayoutId;
        String pageLabel = pageId.Replace ( "_", " " );
        this.LogInit ( "{0} = {1} - {2} > ParentType {3} ", pageId, entityLayout.LayoutId, entityLayout.Title, entityLayout.Design.ParentType );

        EuAdapter.AdapterObjects.PageIdentifiers.Add ( new EvOption ( pageId, pageLabel ) );

        pageId = EuAdapter.CONST_ENTITY_PREFIX + entityLayout.LayoutId + EuAdapter.CONST_AUTHOR_PAGE_ID_SUFFIX;
        pageLabel = pageId.Replace ( "_", " " );
        this.LogInit ( "{0} = {1} - {2} > ParentType {3} ", pageId, entityLayout.LayoutId, entityLayout.Title, entityLayout.Design.ParentType );

        EuAdapter.AdapterObjects.PageIdentifiers.Add ( new EvOption ( pageId, pageLabel ) );


        pageId = EuAdapter.CONST_ENTITY_PREFIX + entityLayout.LayoutId + EuAdapter.CONST_ENTITY_FILTERED_LIST_SUFFIX;
        pageLabel = pageId.Replace ( "_", " " );
        this.LogInit ( "{0} = {1} - {2} > ParentType {3} ", pageId, entityLayout.LayoutId, entityLayout.Title, entityLayout.Design.ParentType );

        EuAdapter.AdapterObjects.PageIdentifiers.Add ( new EvOption ( pageId, pageLabel ) );

        //
        // add the page identifier for child entities.
        //
        switch ( entityLayout.Design.ParentType )
        {
          case EdRecord.ParentTypeList.Organisation_Default:
            {
              pageId = EuAdapter.CONST_ENTITY_PREFIX + entityLayout.LayoutId +EuAdapter.CONST_ORG_PARENT_PAGE_ID_SUFFIX2;
              pageLabel = pageId.Replace ( "_", " " );

              this.LogInit ( "{0} = {1} - {2} > ParentType {3} ", pageId, entityLayout.LayoutId, entityLayout.Title, entityLayout.Design.ParentType );

              EuAdapter.AdapterObjects.PageIdentifiers.Add ( new EvOption ( pageId, pageLabel ) );
              break;
            }
          case EdRecord.ParentTypeList.Organisation:
            {
              pageId = EuAdapter.CONST_ENTITY_PREFIX + entityLayout.LayoutId + EuAdapter.CONST_ORG_PARENT_PAGE_ID_SUFFIX;
              pageLabel = pageId.Replace ( "_", " " );

              this.LogInit ( "{0} = {1} - {2} > ParentType {3} ", pageId, entityLayout.LayoutId, entityLayout.Title, entityLayout.Design.ParentType );

              EuAdapter.AdapterObjects.PageIdentifiers.Add ( new EvOption ( pageId, pageLabel ) );
              break;
            }
          case EdRecord.ParentTypeList.User_Default:
            {
              pageId = EuAdapter.CONST_ENTITY_PREFIX + entityLayout.LayoutId + EuAdapter.CONST_USER_PARENT_PAGE_ID_SUFFIX2;
              pageLabel = pageId.Replace ( "_", " " );

              this.LogInit ( "{0} = {1} - {2} > ParentType {3} ", pageId, entityLayout.LayoutId, entityLayout.Title, entityLayout.Design.ParentType );

              EuAdapter.AdapterObjects.PageIdentifiers.Add ( new EvOption ( pageId, pageLabel ) );

              break;
            }
          case EdRecord.ParentTypeList.User:
            {
              pageId = EuAdapter.CONST_ENTITY_PREFIX + entityLayout.LayoutId + EuAdapter.CONST_USER_PARENT_PAGE_ID_SUFFIX;
              pageLabel = pageId.Replace ( "_", " " );

              this.LogInit ( "{0} = {1} - {2} > ParentType {3} ", pageId, entityLayout.LayoutId, entityLayout.Title, entityLayout.Design.ParentType );

              EuAdapter.AdapterObjects.PageIdentifiers.Add ( new EvOption ( pageId, pageLabel ) );

              break;
            }
          case EdRecord.ParentTypeList.Entity:
            {
              pageId = EuAdapter.CONST_ENTITY_PREFIX + entityLayout.LayoutId + EuAdapter.CONST_ENTITY_PARENT_PAGE_ID_SUFFIX;
              pageLabel = pageId.Replace ( "_", " " );

              this.LogInit ( "{0} = {1} - {2} > ParentType {3} ", pageId, entityLayout.LayoutId, entityLayout.Title, entityLayout.Design.ParentType );

              EuAdapter.AdapterObjects.PageIdentifiers.Add ( new EvOption ( pageId, pageLabel ) );

              break;
            }
        }//END switch statement.

      }//END Entity list iteration

      //this.LogInit ( "Generating the Records PageId list" );
      //
      // dynamic page identifiers for Entities by LayoutId
      //
      foreach ( EdRecord recordLayouts in EuAdapter.AdapterObjects.AllRecordLayouts )
      {
        if ( recordLayouts.State != EdRecordObjectStates.Form_Issued )
        {
          continue;
        }

        String pageId = EuAdapter.CONST_RECORD_PREFIX + recordLayouts.LayoutId;
        String pageLabel = pageId.Replace ( "_", " " );

        this.LogInit ( "{0} = {1} - {2} > ParentType {3} ", pageId, recordLayouts.LayoutId, recordLayouts.Title, recordLayouts.Design.ParentType );

        EuAdapter.AdapterObjects.PageIdentifiers.Add ( new EvOption ( pageId, pageLabel ) );

        //
        // add the page identifier for child entities.
        //
        switch ( recordLayouts.Design.ParentType )
        {
          case EdRecord.ParentTypeList.Organisation:
            {
              pageId = EuAdapter.CONST_RECORD_PREFIX + recordLayouts.LayoutId + EuAdapter.CONST_ORG_PARENT_PAGE_ID_SUFFIX;
              pageLabel = pageId.Replace ( "_", " " );

              this.LogInit ( "{0} = {1} - {2} > ParentType {3} ", pageId, recordLayouts.LayoutId, recordLayouts.Title, recordLayouts.Design.ParentType );

              EuAdapter.AdapterObjects.PageIdentifiers.Add ( new EvOption ( pageId, pageLabel ) );

              break;
            }
          case EdRecord.ParentTypeList.User:
            {
              pageId = EuAdapter.CONST_RECORD_PREFIX + recordLayouts.LayoutId + EuAdapter.CONST_USER_PARENT_PAGE_ID_SUFFIX;
              pageLabel = pageId.Replace ( "_", " " );

              this.LogInit ( "{0} = {1} - {2} > ParentType {3} ", pageId, recordLayouts.LayoutId, recordLayouts.Title, recordLayouts.Design.ParentType );

              EuAdapter.AdapterObjects.PageIdentifiers.Add ( new EvOption ( pageId, pageLabel ) );

              break;
            }
          case EdRecord.ParentTypeList.Entity:
            {
              pageId = EuAdapter.CONST_RECORD_PREFIX + recordLayouts.LayoutId + EuAdapter.CONST_ENTITY_PARENT_PAGE_ID_SUFFIX;
              pageLabel = pageId.Replace ( "_", " " );

              this.LogInit ( "{0} = {1} - {2} > ParentType {3} ", pageId, recordLayouts.LayoutId, recordLayouts.Title, recordLayouts.Design.ParentType );

              EuAdapter.AdapterObjects.PageIdentifiers.Add ( new EvOption ( pageId, pageLabel ) );

              break;
            }
        }//END switch statement.

      }//END Record list iteration 

      this.LogInit ( "PageIdentifiers.Count: " + EuAdapter.AdapterObjects.PageIdentifiers.Count );

      this.LogInitMethodEnd ( "LoadPageIdentifiers method" );

    }//END LoadPageIdentifiers method

    ///  =======================================================================================
    /// <summary>
    /// This method loads the navigational page identifiers list for all static, entities and record layouts
    /// </summary>
    //  ---------------------------------------------------------------------------------
    public void LoadPageComponents ( )
    {
      this.LogInitMethod ( "LoadPageComponents method" );
      this.LogInit ( "AllEntityLayouts.Count: " + EuAdapter.AdapterObjects.AllEntityLayouts.Count );

      if ( EuAdapter.AdapterObjects.PageComponents == null )
      {
        EuAdapter.AdapterObjects.PageComponents = new List<EvOption> ( );
      }

      if ( EuAdapter.AdapterObjects.PageComponents.Count > 0 )
      {
        return;
      }
      //
      // Initialise the methods variables and objects.
      //
      EuAdapter.AdapterObjects.PageComponents = new List<EvOption> ( );

      //this.LogInit ( "Generating the Entities component list" );
      //
      // dynamic page identifiers for Entities by LayoutId
      //
      foreach ( EdRecord entityLayout in EuAdapter.AdapterObjects.AllEntityLayouts )
      {
        if ( entityLayout.State != EdRecordObjectStates.Form_Issued )
        {
          continue;
        }

        //this.LogInit ( "{0} - {1} ", entityLayout.LayoutId, entityLayout.Title );

        //
        // add the selection for the entity component.
        //
        String componentId = EuAdapter.CONST_ENTITY_PREFIX + entityLayout.LayoutId;
        String componentLabel = componentId.Replace ( "_", " " );

        //.LogInit ( "Entity componentId: " + componentId );

        EuAdapter.AdapterObjects.PageComponents.Add ( new EvOption ( componentId, componentLabel ) );

        //
        // add the selection for the entity list component.
        //
        componentId = EuAdapter.CONST_ENTITY_LIST_PREFIX + entityLayout.LayoutId;
        componentLabel = componentId.Replace ( "_", " " );

        //this.LogInit ( "Entity List componentId: " + componentId );

        EuAdapter.AdapterObjects.PageComponents.Add ( new EvOption ( componentId, componentLabel ) );

        //
        // add the selection for the entity filtered list component.
        //
        componentId = EuAdapter.CONST_ENTITY_FILTERED_LIST_PREFIX + entityLayout.LayoutId;
        componentLabel = componentId.Replace ( "_", " " );

        //this.LogInit ( "Entity Filtered List componentId: " + componentId );

        EuAdapter.AdapterObjects.PageComponents.Add ( new EvOption ( componentId, componentLabel ) );


      }//END Entity list iteration

      //this.LogInit ( "Generating the Records component list" );
      //
      // dynamic page identifiers for Records by LayoutId
      //
      foreach ( EdRecord recordLayout in EuAdapter.AdapterObjects.AllRecordLayouts )
      {
        if ( recordLayout.State != EdRecordObjectStates.Form_Issued )
        {
          continue;
        }

        //this.LogInit ( "{0} - {1} ", recordLayout.LayoutId, recordLayout.Title );

        //
        // add the selection for the entity component.
        //
        String componentId = EuAdapter.CONST_RECORD_PREFIX + recordLayout.LayoutId;
        String componentLabel = componentId.Replace ( "_", " " );

        //.LogInit ( "Record componentId: " + componentId );

        EuAdapter.AdapterObjects.PageComponents.Add ( new EvOption ( componentId, componentLabel ) );

        //
        // add the selection for the entity list component.
        //
        componentId = EuAdapter.CONST_RECORD_LIST_PREFIX + recordLayout.LayoutId;
        componentLabel = componentId.Replace ( "_", " " );

        //this.LogInit ( "Record List componentId: " + componentId );

        EuAdapter.AdapterObjects.PageComponents.Add ( new EvOption ( componentId, componentLabel ) );

        //
        // add the selection for the entity filtered list component.
        //
        componentId = EuAdapter.CONST_RECORD_FILTERED_LIST_PREFIX + recordLayout.LayoutId;
        componentLabel = componentId.Replace ( "_", " " );

        //this.LogInit ( "Record Filtered List componentId: " + componentId );

        EuAdapter.AdapterObjects.PageComponents.Add ( new EvOption ( componentId, componentLabel ) );

      }//END Record list iteration

      this.LogInit ( "PageComponents.Count: " + EuAdapter.AdapterObjects.PageComponents.Count );

      this.LogInit ( "Page component list:" );
      foreach ( EvOption option in EuAdapter.AdapterObjects.PageComponents )
      {
        this.LogInit ( "-{0} - Desc: {1} ", option.Value, option.Description );
      }

      this.LogInitMethodEnd ( "LoadPageComponents method" );

    }//END LoadPageComponents method

  }///END EuAdapter class

}///END NAMESPACE
