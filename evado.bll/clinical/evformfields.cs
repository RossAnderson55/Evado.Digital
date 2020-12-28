/***************************************************************************************
 * <copyright file="BLL\EvFormFields.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvCommonFormFields business object.
 *
 ****************************************************************************************/

using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;

//Evado. namespace references.
using Evado.Model;
using Evado.Dal;
using Evado.Model.Digital;

namespace Evado.Bll.Clinical
{
  /// <summary>
  /// A business to manage EvFormFields. This class uses ChecklistItem ResultData object for its content.
  /// </summary>
  public class EvFormFields : EvBllBase
  {
    
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EvFormFields ( )
    {
      this.ClassNameSpace = "Evado.Bll.Clinical.EvFormFields.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EvFormFields ( EvClassParameters Settings )
    {
      this.ClassParameter = Settings;
      this.ClassNameSpace = "Evado.Bll.Clinical.EvFormFields.";

      if ( this.ClassParameter.LoggingLevel == 0 )
      {
        this.ClassParameter.LoggingLevel = Evado.Dal.EvStaticSetting.LoggingLevel;
      }

      this._DalFormFields = new Evado.Dal.Clinical.EvFormFields ( Settings );
    }
    #endregion

    #region Initialise the instrument variables and objects
    //
    //  Initialise the class constants. 
    //    
    /// <summary>
    /// This constant defines the Save action option value.
    /// </summary>
    public const string Action_Save = "SAVE";

    /// <summary>
    /// This constant defines the Delete action option value.
    /// </summary>
    public const string Action_Delete = "DELETE";
    // 
    // Create instantiate the DAL class 
    // 
    private Evado.Dal.Clinical.EvFormFields _DalFormFields = new Evado.Dal.Clinical.EvFormFields ( );

    #endregion

    #region selectionList Query methods

    // =====================================================================================
    /// <summary>
    /// This class returns a list of formfield object based on form's Guid
    /// </summary>
    /// <param name="FormGuid">Guid: (Mandatory) The global unique identifier.</param>
    /// <returns>List of EvFormField: a list of form field object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the Formfield list based on form's Guid
    /// 
    /// 2. Return the formfield list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvFormField> GetView ( Guid FormGuid )
    {
      // 
      // Initialise the methods objects and variables.
      // 
      this.LogMethod ( "GetView" );
      this.LogValue ( "FormGuid: " + FormGuid );
      List<EvFormField> view = new List<EvFormField> ( );

      //
      // Query the database
      //
      view = _DalFormFields.GetView ( FormGuid );
      this.LogClass ( this._DalFormFields.Log );

      return view;

    }//End getRecordList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of formfield options object based on form's Guid and OrderBy string.
    /// </summary>
    /// <param name="FormGuid">Guid: (Mandatory) The global unique identifier.</param>
    /// <param name="OrderBy">string: a sorting order string</param>
    /// <returns>List of EvFormField: a list of form field object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the Formfield options list 
    /// based on form's Guid and OrderBy string.
    /// 
    /// 2. Return the formfield options list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> GetList ( Guid FormGuid, string OrderBy )
    {
      this.LogMethod ( "GetList" );
      // 
      // Initialise the methods objects and variables.
      // 
      this.LogMethod ( "FormGuid: " + FormGuid );
      List<EvOption> list = new List<EvOption> ( );

      //
      // Query the database
      //
      list = this._DalFormFields.GetList ( FormGuid, OrderBy );
      this.LogClass ( this._DalFormFields.Log );

      return list;

    }//End GetList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of formfield options object based on VisitId and FormId
    /// </summary>
    /// <param name="ProjectId">string: a trial identifier</param>
    /// <param name="FormId">string: a form identifier</param>
    /// <param name="OnlySignalValueFields">bool: select only single value fields.</param>
    /// <returns>List of EvFormField: a list of form field object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the Formfield options list
    /// based on the VisitId and FormId
    /// 
    /// 2. Return the formfield options list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> GetOptionList ( 
      String ProjectId, 
      String FormId,
      bool OnlySignalValueFields )
    {
      this.LogMethod ( "GetOptionList" );
      this.LogValue ( "ProjectId: " + ProjectId );
      this.LogValue ( "FormId: " + FormId );
      this.LogValue ( "OnlySignalValueFields: " + OnlySignalValueFields );
      // 
      // Initialise the methods objects and variables.
      // 
      List<EvOption> optionList = new List<EvOption> ( );

      //
      // Query the database
      //
      optionList = this._DalFormFields.GetOptionList ( ProjectId, FormId, OnlySignalValueFields );
      this.LogClass ( this._DalFormFields.Log );

      return optionList;

    }//End GetOptionList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of formfield options object based on VisitId
    /// </summary>
    /// <param name="TrialId">string: a trial identifier</param>
    /// <returns>List of EvFormField: a list of form field object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the Formfield options list
    /// based on the VisitId
    /// 
    /// 2. Return the formfield options list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> getChartOptionList ( string TrialId )
    {
      this.LogMethod ( "getChartOptionList" );
      this.LogValue ( "TrialId: " + TrialId );

      List<EvOption> list = this._DalFormFields.getChartOptionList ( TrialId );
      this.LogClass ( this._DalFormFields.Log );

      return list;

    }//END GetList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of formfield options object based on VisitId and SafetyReport condition
    /// </summary>
    /// <param name="ProjectId">string: a trial identifier</param>
    /// <returns>List of EvFormField: a list of form field object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the Formfield options list
    /// based on the VisitId and safetyReport condition.
    /// 
    /// 2. Return the formfield options list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> GetList ( string ProjectId )
    {
      this.LogMethod ( "GetList" );
      this.LogValue ( "ProjectId: " + ProjectId );

      List<EvOption> List = _DalFormFields.getChartOptionList ( ProjectId );
      this.LogValue ( _DalFormFields.Log );

      return List;

    }//End GetList method.
    #endregion

    #region Retrieve TestReport object methods
    
    // =====================================================================================
    /// <summary>
    /// This class retrieves the formfield object based on Guid.
    /// </summary>
    /// <param name="FieldGuid">Guid: a field global unique identifier</param>
    /// <returns>EvFormField: a formfield ResultData object</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Execute the method for retrieving formfield object values. 
    /// 
    /// 2. Return the formfield object.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvFormField getField ( Guid FieldGuid )
    {
      this.LogMethod ( "getField" );
      this.LogValue ( "FieldGuid: " + FieldGuid );
      EvFormField formField = new EvFormField ( );

      //
      // Query the database
      //
      formField = _DalFormFields.getField ( FieldGuid );
      this.LogClass ( _DalFormFields.Log );

      return formField;

    }//END GetItem class

    #endregion

    #region Update FormField object methods

    // =====================================================================================
    /// <summary>
    /// This class saves items to formfield ResultData table. 
    /// </summary>
    /// <param name="FormField">EvFormField: a formfield object</param>
    /// <returns>EvEventCodes: an event code for saving items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the FormGuid or UserCommonName is empty.
    /// 
    /// 2. Execute the method for deleting items, if the action code is delete
    /// 
    /// 3. Execute the method for adding items, if the formfield's Uid is empty.
    /// 
    /// 4. Else, execute the method for updating items. 
    /// 
    /// 5. Return an event code of the method execution.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes SaveItem ( EvFormField FormField )
    {
      this.LogMethod ( "saveItem Method" );
      this.LogValue ( "Guid: " + FormField.Guid );
      this.LogValue ( "FormGuid: " + FormField.FormGuid );
      this.LogValue ( "FieldId: " + FormField.FieldId );
      this.LogValue ( "ProjectId: " + FormField.TrialId );
      this.LogValue ( "Action: " + FormField.Action );
      // 
      // Define the local variables.
      // 
      EvEventCodes iReturn = EvEventCodes.Ok;

      // 
      // Validate whether the FormGuid and UserCommonName are not empty
      // 
      if ( FormField.FormGuid == Guid.Empty )
      {
        this.LogValue ( "Form Guid Error." );

        return EvEventCodes.Identifier_Global_Unique_Identifier_Error;
      }

      if ( FormField.UserCommonName == String.Empty )
      {
        this.LogValue ( "User common name missing." );

        return EvEventCodes.Identifier_User_Id_Error;
      }

      //
      // If the action is set to delete the object.
      // 
      if ( FormField.Action == EvFormFields.Action_Delete )
      {
        iReturn = _DalFormFields.DeleteItem ( FormField );
        this.LogClass ( _DalFormFields.Log );
        return iReturn;
      }

      //
      // If the newField FormUid is zero or empty, then create a new newField record.
      // 
      if ( FormField.Guid == Guid.Empty )
      {
        this.LogValue ( "Add New Object. " );
        iReturn = _DalFormFields.AddItem ( FormField );
        this.LogClass ( _DalFormFields.Log );
        return iReturn;
      }

      //
      // Update the newField record.
      // 
      iReturn = _DalFormFields.UpdateItem ( FormField );
      this.LogClass ( _DalFormFields.Log );
      return iReturn;

    }//END SaveItem class

    // =====================================================================================
    /// <summary>
    /// This class deletes items from the FormField table.
    /// </summary>
    /// <param name="Form">EvForm: a form object</param>
    /// <returns>EvEventCodes: an event code for deleting items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method to retrieve a list of formfield objects based on the form's Guid
    /// 
    /// 2. Loop through the formfield list and execute the method for deleting the formfield items. 
    /// 
    /// 3. Return an event code for deleting items.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes DeleteFields ( EvForm Form )
    {
      this.LogMethod ( "DeleteFields" );
      this.LogValue ( "Guid: " + Form.Guid );
      // 
      // Define the local variables.
      // 
      EvEventCodes iReturn = EvEventCodes.Ok;

      // 
      // Retrieve the TestReport items
      // 
      List<EvFormField> Items = this._DalFormFields.GetView ( Form.Guid );

      // 
      // Iterate through the items deleting them
      // 
      for ( int Count = 0; Count < Items.Count; Count++ )
      {
        EvFormField FormItem = (EvFormField) Items [ Count ];
        FormItem.UserCommonName = Form.UserCommonName;
        iReturn = _DalFormFields.DeleteItem ( FormItem );
        if ( iReturn < EvEventCodes.Ok )
        {
          this.LogClass ( this._DalFormFields.Log );
          return iReturn;
        }
      }
      return iReturn;

    } // Close DeleteFields method

    #endregion


  }//END EvFormFields Class.

}//END namespace Evado.Evado.BLL 
