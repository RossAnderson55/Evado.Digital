﻿/***************************************************************************************
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

    }///END loadUserProfile method.
    ///
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
        this.LogInitValue ( "Organistion list layout loaded." );
        this.LogInitValue ( "END loadOrganisationList" );
        return;
      }

      //
      // Initialise the methods variables and object.
      //
      EdOrganisations bll_Organisations = new EdOrganisations ( this.ClassParameters );
      bll_Organisations.ClassParameter.LoggingLevel = 5;

      // 
      // Query the database to retrieve a list of the records matching the query parameter values.
      // 
      this._AdapterObjects.OrganisationList = bll_Organisations.getView ();

      this.LogInit( bll_Organisations.Log );

      this.LogInitValue ( "Organisation list count: " + this._AdapterObjects.OrganisationList.Count );

      this.LogInitValue ( "END loadOrganisationList" );

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
      if ( this._AdapterObjects.AllEntityLayouts.Count > 0
        && this.Session.LoadEntityLayoutList == false )
      {
        this.LogInitValue ( "Entity layouts loaded." );
        this.LogInitValue ( "END loadRecordLayoutList" );
        return;
      }

      //
      // Initialise the methods variables and object.
      //
      EdEntityLayouts bll_EntityLayouts = new EdEntityLayouts ( this.ClassParameters );

      // 
      // Query the database to retrieve a list of the records matching the query parameter values.
      // 
      this._AdapterObjects.AllEntityLayouts = bll_EntityLayouts.GetRecordLayoutListWithFields (
        EdRecordTypes.Null,
        EdRecordObjectStates.Null );

      this.LogInit ( bll_EntityLayouts.Log );

      this.Session.LoadEntityLayoutList = false;

      this.LogInitValue ( "AllEntityLayouts.Count: " + this._AdapterObjects.AllEntityLayouts.Count );

      this.LogInitValue ( "END  loadEnityLayoutList" );

    }//END loadEnityLayoutList method

    //===================================================================================
    /// <summary>
    /// This method executes the form list query 
    /// </summary>
    //-----------------------------------------------------------------------------------
    private void loadRecordLayoutList ( )
    {
      this.LogInitMethod ( "loadRecordLayoutList" );
      this.LogInitValue ( "AllRecordLayouts.Count: " + this._AdapterObjects.AllRecordLayouts.Count );

      //
      // Exit the method if the list exists or the loaded entiy layout is false.
      //
      if ( this._AdapterObjects.AllRecordLayouts.Count > 0
        || this.Session.LoadRecordLayoutList == false )
      {
        this.LogInitValue ( "Record layouts loaded." );
        this.LogInitValue ( "END loadRecordLayoutList" );
        return;
      }

      //
      // Initialise the methods variables and object.
      //
      EdRecordLayouts bll_RecordLayouts = new EdRecordLayouts ( this.ClassParameters );

      // 
      // Query the database to retrieve a list of the records matching the query parameter values.
      // 
      this._AdapterObjects.AllRecordLayouts = bll_RecordLayouts.GetRecordLayoutListWithFields (
        EdRecordTypes.Null,
        EdRecordObjectStates.Null );

      this.LogInit ( bll_RecordLayouts.Log );
      this.Session.LoadRecordLayoutList = false;

      this.LogInitValue ( "AllRecordLayouts.Count: " + this._AdapterObjects.AllRecordLayouts.Count );

      this.LogInitValue ( "END loadRecordLayoutList" );

    }//END loadRecordLayoutList method

    //===================================================================================
    /// <summary>
    /// This method executes the form list query 
    /// </summary>
    //-----------------------------------------------------------------------------------
    private void loadSelectionLists ( )
    {
      this.LogInitMethod ( "loadSelectionLists" );
      this.LogInitValue ( "SelectionLists.Count: " + this._AdapterObjects.SelectionLists.Count );

      //
      // Exit the method if the list exists or the loaded entiy layout is false.
      //
      if ( this._AdapterObjects.SelectionLists.Count > 0 )
      {
        this.LogInitValue ( "Record layouts loaded." );
        this.LogInitValue ( "END loadSelectionLists" );
        return;
      }

      //
      // Initialise the methods variables and object.
      //
      EdSelectionLists bll_SelectionLists = new EdSelectionLists ( this.ClassParameters );

      // 
      // Query the database to retrieve a list of the selection lists that are issued.
      // 
      this._AdapterObjects.SelectionLists = bll_SelectionLists.getView ( EdSelectionList.SelectionListStates.Issued );

      this.LogInit ( bll_SelectionLists.Log );
      this.Session.LoadRecordLayoutList = false;

      this.LogInitValue ( "SelectionLists.Count: " + this._AdapterObjects.SelectionLists.Count );

      this.LogInitValue ( "END loadSelectionLists" );

    }//END loadRecordLayoutList method

  }///END EuAdapter class

}///END NAMESPACE
