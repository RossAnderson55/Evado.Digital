/***************************************************************************************
 * <copyright file="BLL\EvTrials.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2020 EVADO HOLDING PTY. LTD..  All rights reserved.
 *     
 *      The use and distribution terms for this software are contained in the file
 *      named license.txt, which can be found in the root of this distribution.
 *      By using this software in any fashion, you are agreeing to be bound by the
 *      terms of this license.
 *     
 *      You must not remove this notice, or any other, from this software.
 *     
 * </copyright>
 *
 ****************************************************************************************/

using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Xml;

//Evado. namespace references.
using Evado.Model;
using Evado.Dal;
using Evado.Model.Digital;

namespace Evado.Bll.Clinical
{
  /// <summary>
  /// This Project business layer object.
  /// </summary>
  public class EdApplications : EvBllBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EdApplications ( )
    {
      this.ClassNameSpace = "Evado.Bll.Clinical.EdApplicationSettings.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EdApplications ( EvClassParameters Settings )
    {
      this.ClassParameter = Settings;
      this.ClassNameSpace = "Evado.Bll.Clinical.EdApplicationSettings.";

      if ( this.ClassParameter.LoggingLevel == 0 )
      {
        this.ClassParameter.LoggingLevel = Evado.Dal.EvStaticSetting.LoggingLevel;
      }
      this.LogDebug ( "CustomerGuid: " + this.ClassParameter.CustomerGuid );
      this.LogDebug ( "ApplicationGuid: " + this.ClassParameter.PlatformGuid );
      this.LogDebug ( "UserProfile.UserId: " + this.ClassParameter.UserProfile.UserId );

      this._Dal_Applications = new Evado.Dal.Clinical.EdApplications ( Settings );
    }
    #endregion

    #region Initialise Class constants and variables
    // 
    // Create instantiate the trial DAL class 
    // 
    private Evado.Dal.Clinical.EdApplications _Dal_Applications = new Evado.Dal.Clinical.EdApplications ( );

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region query methods


    // =====================================================================================
    /// <summary>
    /// This class returns a list of trial objects based on userprofile, state, QueryType and orderby. 
    /// </summary>
    /// <param name="State">EvTrial.TrialStates: the trial state</param>
    /// <returns>List of EvTrial: a list of Project object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the methods for retrieving the list of trial objects and filterling the list. 
    /// 
    /// 2. Loop through the trial list and remove the trials with existing trial QueryType. 
    /// 
    /// 3. Return the list of trial objects. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<Model.Digital.EdApplication> GetApplicationList (
      Model.Digital.EdApplication.ApplicationStates State )
    {
      this.LogMethod ( "GetApplicationList Method." );
      this.LogDebug ( "State: " + State );

      // 
      // Execute the query.
      // 
      List<Model.Digital.EdApplication> applicationList = this._Dal_Applications.GetApplicationList ( State, false );
      this.LogClass ( this._Dal_Applications.Log );
      this.LogDebug ( "applicationList.Count: " + applicationList.Count );

      //
      // Return selectionList to UI
      //
      this.LogMethodEnd ( "GetApplicationList" );
      return applicationList;

    }//END GetApplicationList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of options for trial objects based on the passed parameters. 
    /// </summary>
    /// <param name="State">EvTrial.TrialStates: the trial state</param>
    /// <param name="StateValueNotSelected">Boolean: true, if the state value is not selected</param>
    /// <returns>List of EvTrial: a list of Project object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the methods for retrieving the list of options for trial objects and filterling the list. 
    /// 
    /// 2. Loop through the trial option list and remove the options with existing trial QueryType. 
    /// 
    /// 3. Return the list of options for trial objects. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> getList (
      Model.Digital.EdApplication.ApplicationStates State,
      bool StateValueNotSelected )
    {
      // 
      // Initialise the methods variables and object
      // 
      this.LogMethod ( "getList Method. " );
      this.LogDebug ( "State: " + State );

      string stOrgId = String.Empty;
      List<EvOption> optionList = new List<EvOption> ( );

      // 
      // Execute the query.
      // 
      optionList  = this._Dal_Applications.getOptionList (
        State,
        StateValueNotSelected, false );
      this.LogClass ( this._Dal_Applications.Log );

      //
      // Return selectionList to UI
      //
      return optionList;

    }//END getList method

   //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region application Retrieval methods

    // =====================================================================================
    /// <summary>
    /// This class retrieves the trial object based on Guid. 
    /// </summary>
    /// <param name="ApplicationGuid">Guid: a trial global unique identifier</param>
    /// <returns>EvTrial: a trial object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the trial object based on Guid.
    /// 
    /// 2. Return the trial object
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public Model.Digital.EdApplication GetApplication ( Guid ApplicationGuid )
    {
      this.LogMethod ( "GetApplication Method. " );
      this.LogDebug ( "Guid: " + ApplicationGuid );

      // 
      // Retrieve trial object.
      // 
      Model.Digital.EdApplication application = this._Dal_Applications.GetApplication ( ApplicationGuid );
      this.LogClass ( this._Dal_Applications.Log );

      // 
      // Return the trial object.
      // 
      this.LogMethodEnd ( "GetApplication" );
      return application;

    }//END getTrial method

    // ===================================================================================
    /// <summary>
    /// This class retrieves the trial object based on ProjectId. 
    /// </summary>
    /// <param name="ApplicationId">string: trial identifier</param>
    /// <returns>EvProject: a trial object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the trial object based on ProjectId.
    /// 
    /// 2. Return the trial object
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public Model.Digital.EdApplication GetApplication ( string ApplicationId )
    {
      this.LogMethod ( "GetApplication Method. " );
      this.LogDebug ( "TrialId: " + ApplicationId );
      // 
      // Execute the DAL method to retrieve the trial object and process the 
      // result.
      // 
      Model.Digital.EdApplication application = this._Dal_Applications.GetApplication ( ApplicationId );
      this.LogClass ( this._Dal_Applications.Log );

      this.LogMethodEnd ( "GetApplication" );
      return application;

    }//END getTrial method

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Application Update methods

    // ===================================================================================
    /// <summary>
    /// This class save items to trial ResultData table.
    /// </summary>
    /// <param name="Application">EvTrial: a trial object</param>
    /// <returns>EvEventCodes: an event code for saving items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for deleting items, if the action code is delete. 
    /// 
    /// 2. Execute the method for adding items, if the Guid is empty. 
    /// 
    /// 3. Else, execute the method for updating items on trial ResultData table. 
    /// 
    /// 4. Return an event code of the method execution. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes SaveApplication ( Model.Digital.EdApplication Application )
    {
      // 
      // Initialise the local variables
      // 
      this.LogMethod ( "SaveApplication method. " );
      this.LogDebug ( "Guid: " + Application.Guid );
      this.LogDebug ( "CustomerGuid: " + Application.CustomerGuid );
      this.LogDebug ( "Action: " + Application.Action );
      EvEventCodes result = 0;

      //
      // if the cuysatomer Guid is empty exit.
      //
      if ( Application.CustomerGuid == Guid.Empty )
      {
        this.LogDebug ( "Customer GUID is empty" );

        this.LogMethodEnd ( "SaveApplication" );
        return EvEventCodes.Identifier_Global_Unique_Identifier_Error;
      }

      // 
      // If the unique identifier is null then add this object as a new 
      // trial object.
      // 
      if ( Application.Guid == Guid.Empty )
      {
        this.LogDebug ( "Add Application object to database" );

        result = this._Dal_Applications.AddApplication ( Application );

        this.LogDebugClass (  this._Dal_Applications.Log );

        this.LogMethodEnd ( "SaveApplication" );
        return result;
      }

      // 
      // Update the trial object in the database.
      // 
      result = this._Dal_Applications.updateItem ( Application );

      this.LogDebug ( "Update DAL Status:\r\n" + this._Dal_Applications.Log );

      this.LogDebug ( "EVENT: " + result );

      this.LogMethodEnd ( "SaveApplication" );
      return result;

    }//END SaveApplication class

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    // ===========================================  END CLASS CODE  =============================

  }//END EvProjects Class.

}//END namespace Evado.Bll.Clinical
