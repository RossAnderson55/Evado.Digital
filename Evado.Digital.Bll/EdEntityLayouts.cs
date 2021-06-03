/***************************************************************************************
 * <copyright file="BLL\EvForms.cs" company="EVADO HOLDING PTY. LTD.">
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
 * Description: 
 *  This class contains the EvForms business object.
 *
 ****************************************************************************************/

using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;

//Evado. namespace references.
using Evado.Model;
using Evado.Digital.Model;

namespace Evado.Digital.Bll
{
  /// <summary>
  /// This business object manages the Forms objects in the system.
  /// </summary>
  public class EdEntityLayouts : EvBllBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EdEntityLayouts ( )
    {
      this.ClassNameSpace = "Evado.Digital.Bll.Digital.EdEntityLayouts.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EdEntityLayouts ( EvClassParameters Settings )
    {
      this.ClassParameters = Settings;
      this.ClassNameSpace = "Evado.Digital.Bll.Digital.EdEntityLayouts.";

      this._Dal_RecordLayouts = new  Evado.Digital.Dal.EdEntityLayouts ( Settings );
    }
    #endregion

    #region Class variables and properties

    // 
    // Create instantiate the DAL class 
    // 
    private  Evado.Digital.Dal.EdEntityLayouts _Dal_RecordLayouts = new  Evado.Digital.Dal.EdEntityLayouts ( );

    #endregion

    #region recordDataPointItems Query methods

    // =====================================================================================
    /// <summary>
    /// This class returns a list of form objects. 
    /// </summary>
    /// <param name="LayoutId">string: (Mandatory) a form identifier</param>
    /// <param name="TypeId">EvFormRecordTypes: a form QueryType</param>
    /// <param name="State">EvForm.FormObjecStates: a form state</param>
    /// <returns>List of EvForm: a list of form object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving a list of form objects. 
    /// 
    /// 2. Return a list of form objects. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EdRecord> getLayoutList ( 
      EdRecordTypes TypeId,
      EdRecordObjectStates State )
    {
      // 
      // Initialise the methods objects and variables.
      // 
      this.LogMethod ( "getLayoutList" );
      this.LogValue ( "TypeId: " + TypeId );
      this.LogValue ( "State: " + State  );

      List<EdRecord> _view = _Dal_RecordLayouts.getLayoutList ( TypeId, State, false );

      this.LogClass ( this._Dal_RecordLayouts.Log );

      this.LogMethodEnd ( "getLayoutList" );
      return _view;

    }//END GetFormList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of form objects. 
    /// </summary>
    /// <param name="TrialId">string: (Mandatory) a form identifier</param>
    /// <param name="TypeId">EvFormRecordTypes: a form QueryType</param>
    /// <param name="State">EvForm.FormObjecStates: a form state</param>
    /// <returns>List of EvForm: a list of form object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving a list of form objects. 
    /// 
    /// 2. Return a list of form objects. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EdRecord> GetRecordLayoutListWithFields (
      EdRecordTypes TypeId,
      EdRecordObjectStates State )
    {
      // 
      // Initialise the methods objects and variables.
      // 
      this.LogMethod ( "GetRecordLayoutListWithFields" );
      this.LogValue ( "TypeId: " + TypeId );
      this.LogValue ( "State: " + State );

      List<EdRecord> _view = _Dal_RecordLayouts.getLayoutList ( TypeId, State, true );

      this.LogClass ( this._Dal_RecordLayouts.Log );

      this.LogMethodEnd ( "GetRecordLayoutListWithFields" );
      return _view;

    }//END GetRecordLayoutListWithFields method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of option for selected form objects. 
    /// </summary>
    /// <param name="ApplicationId">string: (Mandatory) a form identifier</param>
    /// <param name="TypeId">EvFormRecordTypes: a form QueryType</param>
    /// <param name="State">EvForm.FormObjecStates: a form state</param>
    /// <param name="SelectByGuid">Boolean: true, if the list is selected by Guid.</param>
    /// <returns>List of EvForm: a list of form object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving a list of form objects. 
    /// 
    /// 2. Return a list of form objects. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> getList ( 
      EdRecordTypes TypeId, 
      EdRecordObjectStates State, 
      bool SelectByGuid )
    {
      this.LogMethod ( "GetList." );

      List<EvOption> List = this._Dal_RecordLayouts.GetList ( TypeId, State, SelectByGuid );
      this.LogClass ( this._Dal_RecordLayouts.Log );

      return List;

    }//END getList method

    #endregion

    #region Retrieve form object methods
    // =====================================================================================
    /// <summary>
    /// This class retrieves a form object based on Guid
    /// </summary>
    /// <param name="Guid">Guid: a global unique identifier</param>
    /// <returns>EvForm: a form ResultData object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving a form object based on Guid
    /// 
    /// 2. Return the form object
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EdRecord GetLayout ( Guid Guid )
    {
      // 
      // Initialise the methods objects and variables.
      // 
      this.LogMethod ( "GetLayout" );
      this.LogValue ( "Guid: " + Guid );
      EdRecord form = new EdRecord ( );

      //
      // Query the database
      //
      form = this._Dal_RecordLayouts.GetLayout ( Guid );
      this.LogClass ( this._Dal_RecordLayouts.Log );

      // 
      // Return the form object.
      //
      return form;

    }//END getForm method

    // ===================================================================================
    /// <summary>
    /// This class retrieves a form object based on TrialId and FormId
    /// </summary>
    /// <param name="LayoutId">string: a form identifier</param>
    /// <returns>EvForm: a form ResultData object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving a form object based on TrialId and FormId
    /// 
    /// 2. Return the form object
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EdRecord GetLayout ( 
      String LayoutId )
    {
      this.LogMethod ( "GetLayout" );
      this.LogValue ( "FormId:" + LayoutId );
      // 
      // Initialise the methods objects and variables.
      // 

      // Execute the DAL method to retrieve the form object and process the 
      // result.
      // 
      EdRecord Item = this._Dal_RecordLayouts.GetLayout ( LayoutId, true );
      this.LogClass ( this._Dal_RecordLayouts.Log );

      // 
      // Return the form object.
      //
      return Item;

    }//END getForm method

    // ===================================================================================
    /// <summary>
    /// This class retrieves an issued form object based on TrialId and FormId
    /// </summary>
    /// <param name="FormId">string: a form identifier</param>
    /// <returns>EvForm: a form ResultData object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving a form object based on TrialId and FormId
    /// 
    /// 2. Return the form object
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EdRecord GetIssuedItem (string FormId )
    {
       this.LogMethod ( "GetIssuedItem" );
      // 
      // Execute the DAL method to retrieve the form object and process the 
      // result.
      // 
      EdRecord Item = _Dal_RecordLayouts.GetLayout ( FormId, true );
      this.LogClass ( this._Dal_RecordLayouts.Log );


      // 
      // Return the form object.
      //
      return Item;

    }// Close GetIssuedItem method

    #endregion

    #region Update form object methods

    // ===================================================================================
    /// <summary>
    /// This class saves items to form ResultData table
    /// </summary>
    /// <param name="Layout">EvForm: a form object</param>
    /// <returns>EvEventCodes: an event code for saving items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the UserCommonName or VisitId or FormId is empty. 
    /// 
    /// 2. Execute the method for deleting items, if the action code is delete
    /// 
    /// 3. Execute the method for adding items, if the form Uid is empty.
    /// 
    /// 4. Else, execute the method for updating items
    /// 
    /// 5. Return an event code of the method execution.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes SaveItem ( EdRecord Layout )
    {
       this.LogMethod ( "saveForm" );
      this.LogValue ( "Action: " + Layout.SaveAction );
      // 
      // Initialise the local variables
      // 
      EdEntityFields formFields = new EdEntityFields ( this.ClassParameters );
      EvEventCodes iReturn = EvEventCodes.Ok;

      if ( Layout.LayoutId == String.Empty )
      {
        this.LogValue ( "Empty FormId" );
        return EvEventCodes.Identifier_Form_Id_Error;
      }

      // 
      // If the Action is DELETE and the state is draft.
      // 
      if ( Layout.SaveAction == EdRecord.SaveActionCodes.Layout_Deleted
        && Layout.State == EdRecordObjectStates.Form_Draft )
      {
        iReturn = this._Dal_RecordLayouts.DeleteItem ( Layout );
        if ( iReturn != EvEventCodes.Ok )
        {
          return iReturn;
        }

        // 
        // Withdraw the TestReport items associated with this TestReport.
        // 
        iReturn = formFields.DeleteFields ( Layout );
        return iReturn;
      }

      // 
      // Update the form state based on the form object Action property.
      // 
      this.updateState ( Layout );
      this.LogValue ( "Form State: " + Layout.State );

      // 
      // If the trial is in use and not being re-issued then it cannot be withdrawn.
      // 
      //  Check if trial has been used.  If so return an error.
      //  
      if ( Layout.State == EdRecordObjectStates.Withdrawn )
      {

      }//END trial state is withdrawn.

      // 
      // If the unique identifier is null then add this object as a new 
      // form object.
      // 
      this.LogValue ( "Save Form to Database." );

      if ( Layout.Guid == Guid.Empty )	// Add new form
      {
        this.LogValue ( "Add Form to database" );

        // 
        // Add the trial to the database.
        // 
        iReturn = this._Dal_RecordLayouts.AddItem ( Layout );
        this.LogClass ( this._Dal_RecordLayouts.Log );

        // 
        // Return the DAL DebugLog.
        // 
        return iReturn;

      }//END Add new trial.

      // 
      // If the trial being approved then withdraw the pervious issues trial
      // 
      if ( Layout.SaveAction == EdRecord.SaveActionCodes.Layout_Approved )
      {
        this.LogValue ( "Withdraw the form" );

        // 
        // Update the trial to withdraw is from use.
        // 
        iReturn = this._Dal_RecordLayouts.WithdrawIssuedForm ( Layout );
        this.LogClass ( this._Dal_RecordLayouts.Log );
      }

      // 
      // Update the trial object.
      // 
      this.LogValue ( "Update Form " );

      // 
      // Update the trial int the database.
      // 
      iReturn = this._Dal_RecordLayouts.UpdateItem ( Layout );
      this.LogClass ( this._Dal_RecordLayouts.Log );

      // 
      // Return the DAL DebugLog.
      // 
      return iReturn;

    }//END SaveItem method

    // =====================================================================================
    /// <summary>
    /// This class updates the form state and approve records for the trial.
    /// </summary>
    /// <param name="Layout">EvForm: a form object</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Update the Authenticated userId if it exists. 
    /// 
    /// 2. Update the form object's values and its state based on the form actions: review, approved, withdrawn or draft. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private void updateState ( EdRecord Layout )
    {
       this.LogMethod ( "updateState." );
      this.LogValue ( "Action: " + Layout.SaveAction );

      // 
      // Initialise the local variables
      //

      // 
      // If the form has an authenticated signoff pass the user id to the 
      // to the DAL layer and DB.
      // 
      string  AuthenticatedUserId = this.ClassParameters.UserProfile.UserId;  

      // 
      // Set the form to reviewed.
      // 
      if ( Layout.SaveAction == EdRecord.SaveActionCodes.Layout_Update )
      {
        this.LogValue ( "Layout updated" );

        Layout.Design.Version += 0.01F;

        return;
      }
      // 
      // Set the form to reviewed.
      // 
      if ( Layout.SaveAction == EdRecord.SaveActionCodes.Layout_Saved )
      {
        this.LogValue ( "Saving  Form" );

        Layout.State = EdRecordObjectStates.Form_Draft;
        Layout.Design.Version += 0.01F;

        return;
      }

      // 
      // Set the form to reviewed.
      // 
      if ( Layout.SaveAction == EdRecord.SaveActionCodes.Layout_Reviewed )
      {
        this.LogValue ( "Reviewing Form" );

        Layout.State = EdRecordObjectStates.Form_Reviewed;

        return;
      }

      if ( Layout.SaveAction == EdRecord.SaveActionCodes.Layout_Approved )
      {
        this.LogValue ( "Issuing Form" );

        Layout.State = EdRecordObjectStates.Form_Issued;
        int version = (int) Layout.Design.Version;
        Layout.Design.Version = version + 1.00F;
        return;
      }

      if ( Layout.SaveAction == EdRecord.SaveActionCodes.Layout_Withdrawn )
      {
        Layout.State = EdRecordObjectStates.Withdrawn;
        return;
      }

      // 
      // If none of the above occur, save the record
      // 
      this.LogValue ( "Saving Form. State:" + Layout.State );
      if ( Layout.State == EdRecordObjectStates.Null )
      {
        Layout.State = EdRecordObjectStates.Form_Draft;
      }
      Layout.Design.Version += 0.01F;

      return;

    }//END updateState method.

    #endregion

    #region Copy form methods

    // ===================================================================================
    /// <summary>
    /// This class copies items from form object to database. 
    /// </summary>
    /// <param name="Layout">EdRecord: a layout object</param>
    /// <returns>EvEventCodes: an event code for copying items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the Guid or UserCommonName is empty
    /// 
    /// 2. Execute the method for copying items to form ResultData table. 
    /// 
    /// 3. Return an event code for copying items
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes CopyLayout ( EdRecord Layout )
    {
      this.LogMethod ( "CopyLayout" );
      this.LogValue ( "LayoutGuid: " + Layout.Guid );
      // 
      // Initialise the local variables
      // 
      EvEventCodes iReturn = EvEventCodes.Ok;

      // 
      // Check that the user id is valid
      // 
      if ( Layout.Guid == Guid.Empty )
      {
        return EvEventCodes.Identifier_Global_Unique_Identifier_Error;
      }

      iReturn = this._Dal_RecordLayouts.CopyLayout ( Layout, true );
      this.LogClass ( this._Dal_RecordLayouts.Log );

      // 
      // Return the response.
      // 
      this.LogMethodEnd ( "CopyLayout" );
      return iReturn;
    }//END CopyForm class

    #endregion

    #region Revise form methods

    // ===================================================================================
    /// <summary>
    /// This class revises the form objects
    /// </summary>
    /// <param name="Form">EvForm: a form object</param>
    /// <param name="UserCommonName">string: a user common name</param>
    /// <returns>EvEventCodes: an event code for revising items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the Guid or User common name is empty
    /// 
    /// 2. Execute the method for revising items
    /// 
    /// 3. Return an event code for revising items
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes ReviseForm ( EdRecord Form )
    {
       this.LogMethod ( "ReviseForm method." );
      this.LogValue ( "FormGuid: " + Form.Guid );
      // 
      // Initialise the local variables
      // 
      EvEventCodes iReturn = EvEventCodes.Ok;

      // 
      // Check that the user id is valid
      // 
      if ( Form.Guid == Guid.Empty )
      {
        return EvEventCodes.Identifier_Global_Unique_Identifier_Error;
      }

      iReturn = this._Dal_RecordLayouts.CopyLayout ( Form, false );
      this.LogClass ( this._Dal_RecordLayouts.Log );

      // 
      // Return the response.
      // 
      return iReturn;
    }//END ReviseForm class

    #endregion

  }//END EvForms class

}//END namespace Evado.Digital.Bll
