/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\ApplicationService.cs" 
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
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Configuration;

using Evado.Bll;
using Evado.Model;
using Evado.Bll.Clinical;
using Evado.Model.Digital;
/// using Evado.Web;


namespace Evado.UniForm.Clinical
{
  public partial class EuAdapter : Evado.Model.UniForm.ApplicationAdapterBase
  {

    // ==================================================================================
    /// <summary>
    /// This method gets the application object from the list.
    /// 
    /// </summary>
    /// <param name="PageCommand">ClientPateEvado.Model.UniForm.Command object</param>
    /// <returns>Bool: True: trial object loaded</returns>
    // ----------------------------------------------------------------------------------
    private void getCustomer (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getCustomer" );
      this.LogDebug ( "CURRENT: Customer.Guid: '{0}'", this.Session.Customer.Guid );
      this.LogDebug ( "CURRENT: Customer.CustomerNo: '{0}'", this.Session.Customer.CustomerNo );

      // 
      // Initialise the methods variables and objects.
      // 
      Guid parameterGuid = this.Session.Customer.Guid;

      //
      // if the trial id parameter does not exist exit.
      //
      if ( PageCommand.hasParameter ( EvIdentifiers.CUSTOMER_GUID ) == true )
      {
        string value = PageCommand.GetParameter ( EvIdentifiers.CUSTOMER_GUID );
        if ( value != String.Empty )
        {
          parameterGuid = new Guid ( value );
        }
        else
        {
          parameterGuid = Guid.Empty;
        }
      }
      this.LogDebug ( "PARAMETER: Guid: '" + parameterGuid + "'" );

      // 
      // if the parameter value matches the current customer exit.
      // 
      if ( this.Session.Customer.Guid == parameterGuid )
      {
        this.LogDebug ( "EXIT: Parameter Matches the current customer." );
        this.LogMethodEnd ( "getCustomer" );
        return;
      }

      //
      // re-initialise the customer object.
      //
      this.Session.Customer = new EvCustomer ( );

      // 
      // if parameter trial is empty but a trial exists then rest it to empty.
      // 
      if ( parameterGuid == Guid.Empty )
      {
        this.LogDebug ( "Parmater Guid is empty." );
        this.LogMethodEnd ( "getCustomer" );
        return;
      }

      // 
      // iterate through the list of customers to select the customer.
      // 
      foreach ( EvCustomer customer in this._ApplicationObjects.CustomerList )
      {
        if ( customer.Guid == parameterGuid )
        {
          this.Session.Customer = customer;
          this.ClassParameters.CustomerGuid = customer.Guid;
        }
      }
      this.LogDebug ( "NEW: Customer.Guid: '{0}'", this.Session.Customer.Guid );
      this.LogDebug ( "NEW: Customer.CustomerNo: '{0}'", this.Session.Customer.CustomerNo );

      //
      // load the organisation list for the customer.
      //
      this.getCustomerOrganisationList ( );

      //
      // Reset the Study List.
      //
      this.Session.ApplicationList = new List<Model.Digital.EdApplication> ( );
      this.Session.Application = new Model.Digital.EdApplication ( );
      this.LogMethodEnd ( "getCustomer" );

    }//END getCustomer method.

    // ==================================================================================
    /// <summary>
    /// This method gets the customer's organisation list
    /// </summary>
    // ----------------------------------------------------------------------------------
    private void getCustomerOrganisationList ( )
    {
      this.LogMethod ( "getCustomerOrganisationList" );
      this.LogDebug ( "CustomerList.Count: " + this.Session.OrganisationList.Count );
      //
      // Initialise the methods variables and objects.
      //
      EvOrganisations bll_Organisations = new EvOrganisations ( this.ClassParameters );

      // 
      // Get the list of trial sites.
      // 
      if ( this.Session.OrganisationList.Count > 0 )
      {
        this.LogDebug ( "Existing organisation list count: " + this.Session.OrganisationList.Count );
        this.LogMethodEnd ( "getCustomerOrganisationList" );
        return;
      }

      //
      // get the list of customer organisations.
      //
      this.Session.OrganisationList = bll_Organisations.getView ( );
      this.LogDebugClass ( bll_Organisations.Log );

      this.LogDebug ( "The organisation list count: " + this.Session.OrganisationList.Count );

      this.LogMethodEnd ( "getCustomerOrganisationList" );
    }

    // ==================================================================================
    /// <summary>
    /// This method creates the trial selection list field.
    /// 
    /// </summary>
    // ----------------------------------------------------------------------------------
    private void GetTrialList ( )
    {
      this.LogMethod ( "GetTrialList" );
      this.LogDebug ( "TrialList.Count: " + this.Session.ApplicationList.Count );

      //
      // If the Customer object has not been set then don't create the list.
      //
      if ( this.Session.Customer.Guid == Guid.Empty )
      {
        this.LogDebug ( "EXIT: Customer has not been selected.." );

        this.LogMethodEnd ( "GetTrialList" );
        return;
      }

      // 
      // If the user had a list of trials then they do not need to be regenerated.
      // 
      if ( this.Session.ApplicationSelectionList.Count > 1
        && this.ServiceUserProfile.NewAuthentication == false )
      {
        this.LogDebug ( "EXIT: Project list exists and does not need to be updated." );

        this.LogMethodEnd ( "GetTrialList" );
        return;
      }

      // 
      // initialise the methods variables and objects.
      // 
      Bll.Clinical.EdApplications bll_Studies = new Bll.Clinical.EdApplications ( this.ClassParameters );

      // 
      // get the list of studies.
      // 
      this.Session.ApplicationList = bll_Studies.GetApplicationList (
        Model.Digital.EdApplication.ApplicationStates.Null);

      this.LogDebugClass ( bll_Studies.Log );

      this.LogDebug ( "TrialList.Count: " + this.Session.ApplicationList.Count );

      this.LogMethodEnd ( "GetTrialList" );
      return;

    }///END GetTrialList method.

    // ==================================================================================
    /// <summary>
    /// This method gets the trial object from the list.
    /// 
    /// </summary>
    /// <param name="PageCommand">ClientPateEvado.Model.UniForm.Command object</param>
    /// <returns>Bool: True: trial object loaded</returns>
    // ----------------------------------------------------------------------------------
    private void GetTrial (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "GetTrial" );
      this.LogDebug ( "Current Trial: '{0}'", this.Session.Application.ApplicationId );

      // 
      // Initialise the methods variables and objects.
      // 
      String stParameterTrialId = this.Session.Application.ApplicationId;

      //
      // Do not select a new trial object if this is a new trial
      //
      if ( this.Session.Application.Guid == EvStatics.CONST_NEW_OBJECT_ID )
      {
        this.LogDebug ( "New Trial." );
        this.LogMethodEnd ( "GetTrial" );
        return;
      }

      //
      // If there is only one trial in the trial list the load that trial.
      //
      if ( this.Session.ApplicationList.Count == 1
        && this.Session.Application.ApplicationId != this.Session.ApplicationList [ 0 ].ApplicationId )
      {
        this.Session.Application = this.Session.ApplicationList [ 0 ];

        this.LogMethodEnd ( "GetTrial" );
        return;
      }

      //
      // if the trial id parameter does not exist exit.
      //
      if ( PageCommand.hasParameter ( EvIdentifiers.APPLICATION_ID ) == true )
      {
        stParameterTrialId = PageCommand.GetParameter ( EvIdentifiers.APPLICATION_ID );
      }
      this.LogDebug ( "PARAMETER: TrialId: '" + stParameterTrialId + "'" );

      // 
      // if parameter is null trial the trial is set to empty.
      // 
      if ( stParameterTrialId == "Null" )
      {
        // 
        // set the trial object to empty.
        // 
        this.Session.Application = new Model.Digital.EdApplication ( );


        this.LogDebug ( "Parameter ProjectId is 'Null' set selection trial object to empty" );
        this.LogMethodEnd ( "GetTrial" );
        return;
      }

      //
      // IF trial ids match exit..
      //
      if ( ( this.Session.Application.ApplicationId == stParameterTrialId
           || stParameterTrialId == String.Empty )
        && ( this.Session.Application.Guid != Guid.Empty ) )
      {
        this.LogDebug ( "Trial exists or matches the parameter list." );
        this.LogMethodEnd ( "GetTrial" );
        return;
      }

      //
      // if the project ID is not in the current list of projects, reset it to an empty project object.
      //
      if ( this.hasTrial ( stParameterTrialId ) == false )
      {
        stParameterTrialId = String.Empty;
        this.Session.Application = new Model.Digital.EdApplication ( );
        this.LogDebug ( "Trial identifier not in the list. set empty trial." );
        this.LogMethodEnd ( "GetTrial" );
        return;
      }


      this.LogDebug ( "Loading a new trial." );
      // 
      // get the trial object for the selected trial.
      // 
      this.LoadTrial ( stParameterTrialId );

      this.LogMethodEnd ( "GetTrial" );
      return;

    }//END GetTrial method.

    // ==================================================================================
    /// <summary>
    /// This method gets the trial for the list of studies.
    /// 
    /// </summary>
    /// <param name="TrialId">String: study identifier</param>
    /// <returns>EvStudy object.</returns>
    // ----------------------------------------------------------------------------------
    private void LoadTrial ( String TrialId )
    {
      this.LogMethod ( "LoadTrial" );
      this.LogDebug ( "TrialId {0}. ", TrialId );
      //
      // if the list is empty exit with an empty study object.
      //
      if ( this.Session.ApplicationList.Count == 0 )
      {
        this.Session.Application = new Model.Digital.EdApplication ( );

        this.LogMethodEnd ( "LoadTrial." );
        return;
      }

      //
      // if the existing trial is loaded exit.
      //
      if ( this.Session.Application.ApplicationId == TrialId )
      {
        this.LogMethodEnd ( "LoadTrial." );
        return;
      }

      //
      // Iterate through the list of studies and return the matching studyId
      //
      foreach ( Model.Digital.EdApplication trial in this.Session.ApplicationList )
      {
        //this.LogDebugFormat ( "Interation StudyId {0}. ", study.StudyId );

        if ( trial.ApplicationId == TrialId )
        {
          this.LogDebug ( "SELECTED: TrialId {0}. ", TrialId );
          this.LogMethodEnd ( "LoadTrial" );
          this.Session.Application = trial;
          return;
        }
      }

      this.LogMethodEnd ( "LoadTrial." );

      this.Session.Application = new Model.Digital.EdApplication ( );

    }//END LoadTrial method

    //===================================================================================
    /// <summary>
    /// This method checks to see if the current session project is in the list of projects
    /// and returns true if it is present.
    /// </summary>
    /// <returns>True: project exists.</returns>
    //-----------------------------------------------------------------------------------
    private bool hasTrial ( String TrialId )
    {
      this.LogMethod ( "hasTrial" );
      this.LogDebug ( "Passed TrialId: {0}. ", TrialId );

      if ( TrialId == String.Empty )
      {
        this.LogDebug ( "True: Trial not selected" );
        this.LogMethodEnd ( "hasTrial" );
        return true;
      }

      //
      // Iterate through the list of projects and return true if the current project is in that list.
      //
      foreach ( Model.Digital.EdApplication trial in this.Session.ApplicationList )
      {
        if ( trial.ApplicationId == TrialId )
        {
          this.LogDebug ( "True: Trial matches an entry in the selection list." );
          this.LogMethodEnd ( "hasTrial" );
          return true;
        }
      }

      this.LogDebug ( "False: No matches found." );
      this.LogMethodEnd ( "hasTrial" );
      return false;

    }//END hasTrialmethod

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
      Evado.Bll.Clinical.EvUserProfiles userProfiles =
        new Evado.Bll.Clinical.EvUserProfiles ( this.ClassParameters );
      //
      // if an anonoymous command is encountered create a user profile for a patient.
      //
      if ( PageCommand.Type == Evado.Model.UniForm.CommandTypes.Anonymous_Command
        || this.ServiceUserProfile.UserAuthenticationState == EvUserProfileBase.UserAuthenticationStates.Anonymous_Access )
      {
        this.LogEvent ( "Anonymous command encountered" );

        this.Session.UserProfile = new EvUserProfile ( );
        this.Session.UserProfile.UserId = this.ServiceUserProfile.UserId;
        this.Session.UserProfile.CommonName = this.ServiceUserProfile.UserId;
        this.Session.UserProfile.RoleId = EvRoleList.Null;

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

      //
      // if the user is not an Evado administrator load the customer object as the currently set 
      // Customer object.
      //
      if ( this.Session.UserProfile.CustomerGuid != Guid.Empty
        && this.Session.UserProfile.CustomerGuid != this._ApplicationObjects.PlatformSettings.Guid )
      {
        this.ClassParameters.CustomerGuid = this.Session.UserProfile.CustomerGuid;

        this.Session.Customer = this.Session.UserProfile.Customer;
        this.LogDebug ( "Customer Name is {0}. ", this.Session.Customer.Name );
      }

      return true;

    }///END loadUserProfile method.

    //===================================================================================
    /// <summary>
    /// This method executes the form list query 
    /// </summary>
    //-----------------------------------------------------------------------------------
    private void loadTrialFormList ( )
    {
      this.LogMethod ( "loadTrialFormList" );
      this.LogDebug ( "ApplicationId: '" + this.Session.Application.ApplicationId + "'" );

      if ( this.Session.Application.ApplicationId == String.Empty )
      {
        this.LogDebug ( "ApplicationId not defined" );
        this.LogMethodEnd ( "loadTrialFormList" );
        return;
      }

      if ( this.Session.FormList.Count > 0 )
      {
        this.LogDebug ( "FormList loaded." );
        this.LogMethodEnd ( "loadTrialFormList" );
        return;
      }

      //
      // Initialise the methods variables and object.
      //
      EdRecordLayouts bll_Forms = new EdRecordLayouts ( this.ClassParameters );
      this.Session.FormType = EdRecordTypes.Null;
      this.Session.FormState = EdRecordObjectStates.Form_Issued;
      this.Session.FormsAdaperLoaded = true;

      // 
      // Query the database to retrieve a list of the records matching the query parameter values.
      // 
      this.Session.FormList = bll_Forms.GetRecordLayoutListWithFields (
        this.Session.Application.ApplicationId,
        this.Session.FormType,
        this.Session.FormState );

      this.LogDebugClass ( bll_Forms.Log );

      this.LogDebug ( " list count: " + this.Session.FormList.Count );

      this.LogMethodEnd ( "loadTrialFormList" );

    }//END loadTrialFormList method
  }///END EuAdapter class

}///END NAMESPACE
