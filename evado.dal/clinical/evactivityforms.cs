/***************************************************************************************
 * <copyright file="dal\EvActivityForms.cs" company="EVADO HOLDING PTY. LTD.">
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
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;

//References to Evado specific libraries

using Evado.Model;
using Evado.Model.Digital;


namespace Evado.Dal.Clinical
{
  /// <summary>
  /// A business Component used to manage Ethics roles
  /// The Evado.Model.TrialVisitForm is used in most methods 
  /// and is used to store serializable information about an account
  /// </summary>
  public class EvActivityForms : EvDalBase
  {
    #region Class Initialization

    /// <summary>
    /// This is the class initialisation method.
    /// </summary>
    public EvActivityForms ( )
    {
      this.ClassNameSpace = "Evado.Dal.Clinical.EvActivityForms.";
    }

    /// <summary>
    /// This is the class initialisation method with settings configured.
    /// </summary>
    public EvActivityForms ( EvClassParameters ClassParameters )
    {
      this.ClassParameters = ClassParameters ;

      this.ClassNameSpace = "Evado.Dal.Clinical.EvActivityForms.";
    }

    #endregion

    #region Class Constant

    #region Sql query string

    /// <summary>
    /// This constant defines a sql query for a view
    /// </summary>
    private const string _sqlQuery_View = "Select * FROM EvActivityForm_View ";

    /// <summary>
    /// This constant defines a sql query for a milestone view
    /// </summary>
    private const string _sqlQuery_MilestoneView = "Select * FROM EvActivityForm_MilestoneView ";

    /// <summary>
    /// This constant defines a sql query for the issued forms
    /// </summary>
    private const string _sqlQueryIssued_Forms = "Select * FROM EvForms_Issued ";
    #endregion

    #region The SQL Store Procedure constants
    /// <summary>
    /// This constant defines a stored procedure for add item
    /// </summary>
    private const string _storedProcedureAddItem = "usr_ActivityForm_add";

    /// <summary>
    /// This constant defines a stored procedure for update item
    /// </summary>
    private const string _storedProcedureUpdateItem = "usr_ActivityForm_update";

    /// <summary>
    /// This constant defines a stored procedure for delete item
    /// </summary>
    private const string _storedProcedureDeleteItem = "usr_ActivityForm_delete";
    #endregion

    #region SQL Parameters

    //
    // The field and parameter values for the SQl customer filter 
    //
    private const string DB_CUSTOMER_GUID = "CU_GUID";
    private const string PARM_CUSTOMER_GUID = "@CUSTOMER_GUID";

    /// <summary>
    /// This constant defines a parameter for a global unique identifier
    /// </summary>
    private const string _parmGuid = "@Guid";

    /// <summary>
    /// This constant defines a parameter for an activity global unique identifier
    /// </summary>
    private const string _parmActivityGuid = "@ActivityGuid";

    /// <summary>
    /// This constant defines a parameter for item identifier
    /// </summary>
    private const string _parmItemId = "@FormId";

    /// <summary>
    /// This constant defines a parameter for an order
    /// </summary>
    private const string _parmOrder = "@Order";

    /// <summary>
    /// This constant defines a parameter for a mandatory
    /// </summary>
    private const string _parmMandatory = "@Mandatory";

    /// <summary>
    /// This constant defines a parameter for a trial identifier
    /// </summary>
    private const string _parmTrialId = "@TrialId";

    /// <summary>
    /// This constant defines a parameter for an activity identifier
    /// </summary>
    private const string _parmActivityId = "@ActivityId";

    /// <summary>
    /// This constant defines a parameter for an initial version
    /// </summary>
    private const string _parmInitialVersion = "@InitialVersion";

    /// <summary>
    /// This constant defines a parameter for user identifier of those who updates activity forms
    /// </summary>
    private const string _parmUpdatedByUserId = "@UpdatedByUserId";

    /// <summary>
    /// This constant defines a parameter for user who updates activity form
    /// </summary>
    private const string _parmUpdatedBy = "@UpdatedBy";

    /// <summary>
    /// This constant defines a parameter for an update date
    /// </summary>
    private const string _parmUpdateDate = "@UpdateDate";

    /// <summary>
    /// This constant defines a parameter for a milestone global unique identifier
    /// </summary>
    private const string _parmMilestoneGuid = "@MilestoneGuid";

    /// <summary>
    /// This constant defines a parameter for a milestone global identifier
    /// </summary>
    private const string _parmMilestoneId = "@MilestoneId";

    /// <summary>
    /// This constant defines a parameter for an arm index 
    /// </summary>
    private const string _parmScheduleId = "@ScheduleId";
    #endregion

    #endregion

    #region Class Property

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Order Comparer

    /// <summary>
    /// this class defines the order comparer for actvity form object lists.
    /// </summary>
    public class ActvityFormOrderComparer : IComparer<EvActvityForm>
    {
      /// <summary>
      /// This method performs the order comparision 
      /// </summary>
      /// <param name="x">EvActvityForm object</param>
      /// <param name="y">EvActvityForm object</param>
      /// <returns>int </returns>
      public int Compare ( EvActvityForm x, EvActvityForm y )
      {
        if ( x.Order < y.Order )
        {
          return -1;
        }
        return 0;

      }//END compare method
    }//END class
    #endregion

    #region Data Reader methods

    // ==================================================================================
    /// <summary>
    /// This method reads the content of the data row object containing a query result
    /// into an Activity Record object.
    /// </summary>
    /// <param name="Row">DataRow: a data row record object</param>
    /// <returns>EvActivityForm: a data row object</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Extract the data object values from the data row object and add to the activity form object.
    /// 
    /// 2. Return the activity form object.
    /// </remarks>
    // ----------------------------------------------------------------------------------
    private EvActvityForm readDataRow ( DataRow Row )
    {
      // 
      // Initialise activity form object
      // 
      EvActvityForm activityForm = new EvActvityForm ( );

      // 
      // Extract the data object values from the data row object and add to the activity form object.
      // 
      activityForm.Guid = EvSqlMethods.getGuid ( Row, "ACF_Guid" );
      activityForm.ActivityGuid = EvSqlMethods.getGuid ( Row, "AC_Guid" );
      activityForm.FormId = EvSqlMethods.getString ( Row, "FormId" );
      activityForm.FormTitle = EvSqlMethods.getString ( Row, "TC_Title" );
      activityForm.Order = EvSqlMethods.getInteger ( Row, "ACF_Order" );
      activityForm.Mandatory = EvSqlMethods.getBool ( Row, "ACF_Mandatory" );
      activityForm.InitialVersion = EvSqlMethods.getInteger ( Row, "ACF_InitialVersion" );
      activityForm.Selected = true;

      // 
      // Return the activity form object.
      // 
      return activityForm;

    }// End readDataRow method.

    // ==================================================================================
    /// <summary>
    /// This method reads the content of the data row object containing a query result
    /// into an Activity object.
    /// </summary>
    /// <param name="Row">DataRow: a row data object</param>
    /// <returns>EvActivityForm: a milestone row object</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Extract the compatible data row values to the activity form object.
    /// 
    /// 2. Return the activity form object.
    /// </remarks>
    // ----------------------------------------------------------------------------------
    private EvActvityForm readMilestoneRow ( DataRow Row )
    {
      // 
      // Initialise the activity form object
      // 
      EvActvityForm activityForm = new EvActvityForm ( );

      // 
      // Extract the data object values from the data row object and add to the activity form object
      // 
      activityForm.Guid = EvSqlMethods.getGuid ( Row, "ACF_Guid" );
      activityForm.ActivityGuid = EvSqlMethods.getGuid ( Row, "AC_Guid" );
      activityForm.ActivityId = EvSqlMethods.getString ( Row, "ActivityId" );
      activityForm.FormId = EvSqlMethods.getString ( Row, "FormId" );
      activityForm.FormTitle = EvSqlMethods.getString ( Row, "TC_Title" );
      activityForm.Order = EvSqlMethods.getInteger ( Row, "ACF_Order" );
      activityForm.Mandatory = EvSqlMethods.getBool ( Row, "ACF_Mandatory" );
      activityForm.ActivityMandatory = EvSqlMethods.getBool ( Row, "MA_IsMandatory" );
      activityForm.Selected = true;

      // 
      // Return the activity form object.
      // 
      return activityForm;

    }// End readMilestoneRow method.

    #endregion

    #region List and queries methods

    // ==================================================================================
    /// <summary>
    ///  This method queries the database to retrieve a list of the issued forms templates
    ///  that could be associated with an activity.  The currently selected forms are also 
    ///  included in the query result.
    /// </summary>
    /// <param name="TrialId">string: (Mandatory) The trial id</param>
    /// <param name="ActivityGuid">Guid: (Mandatory) Visit unique identifer</param>
    /// <returns>List of EvActivityForm: an optionList contains EvActivityForm objects.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Get an issued form list and a selected form list. 
    /// 
    /// 2. Create a string list of issued form list and a string of selected form list. 
    /// 
    /// 3. Add the issued forms into the return selected form list, if there is no selected form string.
    /// 
    /// 4. Return the selected form list.
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public List<EvActvityForm> getSelectionView ( 
      String TrialId, 
      Guid ActivityGuid )
    {
      this.LogMethod ( "getSelectionView" );
      this.LogDebug ( "TrialId: " + TrialId );
      this.LogDebug ( "ActivityGuid: " + ActivityGuid );

      //
      // Initialize a method status string, a return selection list, a form list 
      //
      List<EvActvityForm> issuedFormList = new List<EvActvityForm> ( );
      List<EvActvityForm> actvityFormList = new List<EvActvityForm> ( );
      int inLastOrder = 5;

      //
      // Get a list of issued forms
      // 
      issuedFormList = this.getIssuedForms ( TrialId );
      this.LogDebug ( "isued form count {0}", issuedFormList.Count );

      // 
      // Get a list of selected forms 
      // 
      actvityFormList = this.getFormList ( ActivityGuid );
      this.LogDebug ( "activity form count {0}", actvityFormList.Count );

      //
      // Loop through the selected forms list and Create a string list of the forms that have been selected.
      //
      foreach ( EvActvityForm form in actvityFormList )
      {
        inLastOrder += 5;
      }//END issuedFormList forms iteration loop

      //
      // Loop through the selected forms list and Create a string list of the forms that have been selected.
      //
      foreach ( EvActvityForm form in issuedFormList )
      {
        form.Guid = Guid.Empty;
        form.Selected = false;
        form.Mandatory = false;
        form.Order = inLastOrder;

        inLastOrder += 5;

      }//END issuedFormList forms iteration loop

      // 
      // Loop through the issued forms list and and add the issued forms into the return selected form list.
      // 
      foreach ( EvActvityForm issuedForm in issuedFormList )
      {
        //
        // get an activity form if it ecists.
        //
        EvActvityForm actvityForm = getActvityForm (
          actvityFormList,
          issuedForm.FormId );

        //
        // if the activity form exists update the issued form object.
        //
        if ( actvityForm != null )
        {
          issuedForm.Guid = actvityForm.Guid;
          issuedForm.Mandatory = actvityForm.Mandatory;
          issuedForm.Selected = true;
          issuedForm.Order = actvityForm.Order;
        }

        this.LogValue ( "FormId: " + issuedForm.FormId + ", Selected:" + issuedForm.Selected + ", Mandatory:" + issuedForm.Mandatory );

      }//END issued forms iteration loop

      ActvityFormOrderComparer orderComparer = new ActvityFormOrderComparer ( );

      issuedFormList.Sort ( orderComparer );

      this.LogValue ( "Final list count: " + issuedFormList.Count );

      // 
      // Return the List containing the User data object.
      // 
      return issuedFormList;

    }//END getSelectionView method.

    //===================================================================================
    /// <summary>
    /// This method selects a form from the list.
    /// </summary>
    /// <param name="formList">list of EvActivityForm objects</param>
    /// <param name="FormId">String: form identifier</param>
    /// <returns>EvActvityForm object</returns>
    //-----------------------------------------------------------------------------------
    private EvActvityForm getActvityForm ( List<EvActvityForm> formList, String FormId )
    {
      foreach ( EvActvityForm form in formList )
      {
        if ( form.FormId == FormId )
        {
          return form;
        }//END form selection

      }//END form list interation loop.

      return null;
    }

    // ==================================================================================
    /// <summary>
    /// This method returns the list of activity records based on the activityGuid and OrderBy value. 
    /// </summary>
    /// <param name="ActivityGuid">Guid: (Mandatory) The selection organistion's identifier</param>
    /// <returns>List of EvActivityForm: a list contains selected data objects.</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Define the SQL query parameters and the sql query string
    /// 
    /// 2. Execute the sql query command and store the results on the data table. 
    /// 
    /// 3. Iterate through the result table and extract the data row to the Activity record object. 
    /// 
    /// 4. Add the Activity Record Object values to the Activity record list. 
    /// 
    /// 5. Update the numeric order of the Activity record list 
    /// 
    /// 6. Return the Activity Record List. 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public List<EvActvityForm> getFormList ( Guid ActivityGuid )
    {
      this.LogMethod ( "getFormList method. " );
      this.LogValue ( "ActivityGuid: " + ActivityGuid );
      //
      // Initialize the method status string, an sql query string and a return list of activity records
      //
      string sqlQueryString;
      List<EvActvityForm> view = new List<EvActvityForm> ( );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( _parmActivityGuid, SqlDbType.UniqueIdentifier),
      };
      cmdParms [ 0 ].Value = ActivityGuid;

      // 
      // Generate the SQL query string
      // 
      sqlQueryString = _sqlQuery_View + "WHERE AC_Guid = @ActivityGuid"
        + " ORDER BY ACF_Order; ";

      this.LogDebug ( sqlQueryString );

      //
      // Execute the query against the database      
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
      {
        // 
        // Iterate through the results extracting the role information.
        // 
        for ( int Count = 0; Count < table.Rows.Count; Count++ )
        {
          // 
          // Extract the table row
          // 
          DataRow row = table.Rows [ Count ];

          EvActvityForm activityForm = this.readDataRow ( row );


          this.LogValue ( "Activity: " + activityForm.FormId + ", Selected: " + activityForm.Selected );

          view.Add ( activityForm );

        } //END interation loop.

      }//END using method

      // 
      // Update the numbering
      // 
      for ( int count = 0; count < view.Count; count++ )
      {
        ( ( EvActvityForm ) view [ count ] ).Order = count * 5 + 5;
      }

      this.LogValue ( "view count: " + view.Count );

      // 
      // Return the list containing the User data object.
      // 
      return view;

    }//END getView method.

    // ==================================================================================
    /// <summary>
    /// This class returns a list of activity records based on the MilestoneGuid and OrderBy value. 
    /// </summary>
    /// <param name="MilestoneGuid">Guid: (Mandatory) The selection organistion's identifier</param>
    /// <returns>List of EvActivityForm: a list contains milestone data objects.</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Define the SQL query parameters and the sql query string.
    /// 
    /// 2. Execute the sql string and store results on the data table. 
    /// 
    /// 3. Loop through the data table and extract the row data to the Activity Record object
    /// 
    /// 4. Add the Activity record object values to the Activity record list. 
    /// 
    /// 5. Update the numbering order of the activity record list. 
    /// 
    /// 6. Return the activity record list
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public List<EvActvityForm> getMilestoneView (
      Guid MilestoneGuid )
    {
      this.LogMethod ( "getMilestoneView method. " );
      this.LogValue ( "MilestoneGuid: " + MilestoneGuid );
      //
      // Initialize a method status string, a sql query string and a return list of milestone data object. 
      //
      string sqlQueryString;
      List<EvActvityForm> view = new List<EvActvityForm> ( );

      // 
      // Define the SQL query parameters for milestone data object and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( _parmMilestoneGuid, SqlDbType.UniqueIdentifier),
      };
      cmdParms [ 0 ].Value = MilestoneGuid;

      // 
      // Generate the SQL query string for milestone data object
      // 
      sqlQueryString = _sqlQuery_MilestoneView + "WHERE M_Guid = @MilestoneGuid "
        + " ORDER BY ActivityId, ACF_Order; ";

      this.LogDebug ( sqlQueryString );

      //
      // Execute the query against the database      
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
      {
        // 
        // Iterate through the results extracting the role information.
        // 
        for ( int Count = 0; Count < table.Rows.Count; Count++ )
        {
          // 
          // Extract the table row
          // 
          DataRow row = table.Rows [ Count ];

          EvActvityForm activityForm = this.readMilestoneRow ( row );

          view.Add ( activityForm );

        } //END interation loop.

      }//END using method

      // 
      // Update the numbering
      // 
      for ( int count = 0; count < view.Count; count++ )
      {
        ( ( EvActvityForm ) view [ count ] ).Order = count * 5 + 5;
      }

      this.LogValue ( "view count: " + view.Count.ToString ( ) );

      // 
      // Return the list containing the milestone data object.
      // 
      return view;

    }//END getMilestoneView method.

    // ==================================================================================
    /// <summary>
    /// This method returns a list of Activity Records based on ProjectId and ActivityId
    /// </summary>
    /// <param name="ProjectId">String: (Mandatory) The Project identifier</param>
    /// <param name="ActivityId">String: an activity identifier</param>
    /// <returns>List of EvActivityForm: an arrayList contains User data objects.</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Define the SQL query parameters and the sql query string. 
    /// 
    /// 2. Execute the sql query command and store the results on the data table 
    /// 
    /// 3. Loop through the data table and extract the row data to the Activity Record object
    /// 
    /// 4. Add the Activity record object values to the Activity record list. 
    /// 
    /// 5. Update the numbering order of the activity record list. 
    /// 
    /// 6. Return the activity record list
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public List<EvActvityForm> getForms (
      String ProjectId,
      String ActivityId )
    {
      //
      // Initialize a method status string, an sql query string and a return list of selection data objects. 
      //
      this.LogMethod ( "getForms method. " );
      this.LogValue ( "TrialId: " + ProjectId );
      this.LogValue ( "ActivityId: " + ActivityId );
      string sqlQueryString;
      List<EvActvityForm> view = new List<EvActvityForm> ( );

      // 
      // Define the SQL query parameters for the selection data object and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( _parmTrialId, SqlDbType.NVarChar, 10),
        new SqlParameter( _parmActivityId, SqlDbType.NVarChar, 20),
      };
      cmdParms [ 0 ].Value = ProjectId;
      cmdParms [ 1 ].Value = ActivityId;

      // 
      // Generate the SQL query string for a selection list
      // 
      sqlQueryString = _sqlQuery_View + "WHERE ( (FORM_TRIAL_ID = @TrialId) "
        + " OR (FORM_TRIAL_ID = '" + EvcStatics.CONST_GLOBAL_PROJECT + "') ) ";

      if ( ActivityId != string.Empty )
      {
        sqlQueryString += " AND (ActivityId = @ActivityId) ";
      }
      sqlQueryString += " ORDER BY FormId; ";

      this.LogDebug ( sqlQueryString );

      //
      // Execute the query against the database      
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
      {
        // 
        // Iterate through the results extracting the role information.
        // 
        for ( int Count = 0; Count < table.Rows.Count; Count++ )
        {
          // 
          // Extract the table row
          // 
          DataRow row = table.Rows [ Count ];

          EvActvityForm activityForm = new EvActvityForm ( );
          activityForm.Guid = Guid.Empty; ;
          activityForm.ActivityGuid = EvSqlMethods.getGuid ( row, "AC_Guid" );
          activityForm.ActivityId = EvSqlMethods.getString ( row, "ActivityId" );
          activityForm.FormId = EvSqlMethods.getString ( row, "FormId" );
          activityForm.FormTitle = EvSqlMethods.getString ( row, "TC_Title" );
          activityForm.Mandatory = EvSqlMethods.getBool ( row, "ACF_Mandatory" );
          activityForm.Selected = true;

          view.Add ( activityForm );

        } //END interation loop.

      }//END using method

      // 
      // Update the numbering
      // 
      for ( int count = 0; count < view.Count; count++ )
      {
        ( ( EvActvityForm ) view [ count ] ).Order = count * 5 + 5;
      }


      this.LogValue ( "form object count: " + view.Count );
      // 
      // Return the LIST containing the User data object.
      // 
      return view;

    }//END getForms method.

    // ==================================================================================
    /// <summary>
    /// This method returns a list of Activity records based on ProjectId. 
    /// </summary>
    /// <param name="TrialId">String: (Mandatory) The Project identifier</param>
    /// <returns>List of EvActivityForm: an ArrayList contains issued data objects.</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Define the SQL query parameters and the sql query string. 
    /// 
    /// 2. Execute the sql query command and store the results on the data table 
    /// 
    /// 3. Loop through the data table and extract the row data to the Activity Record object
    /// 
    /// 4. Add the Activity record object values to the Activity record list. 
    /// 
    /// 5. Update the numbering order of the activity record list. 
    /// 
    /// 6. Return the activity record list
    /// </remarks>
    // ----------------------------------------------------------------------------------
    private List<EvActvityForm> getIssuedForms ( String TrialId )
    {
      this.LogMethod ( "getForms method. " );
      this.LogValue ( "TrialId: " + TrialId );
      //
      // Initialize a method status string, a sql query string and a return list of issued data object. 
      //
      string sqlQueryString;
      List<EvActvityForm> view = new List<EvActvityForm> ( );

      // 
      // Define the SQL query parameters for the issued data object and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter( PARM_CUSTOMER_GUID, SqlDbType.UniqueIdentifier),
        new SqlParameter( _parmTrialId, SqlDbType.NVarChar, 10),
      };
      cmdParms [ 0 ].Value = this.ClassParameters.CustomerGuid;
      cmdParms [ 1 ].Value = TrialId;

      // 
      // Generate the SQL query string
      // 
      sqlQueryString = _sqlQueryIssued_Forms
        + "WHERE (" + EvActivityForms.DB_CUSTOMER_GUID + " = " + EvActivityForms.PARM_CUSTOMER_GUID + " )\r\n"
        + " AND ( (TrialId = @TrialId) "
        + "    OR (TrialId = '" + EvcStatics.CONST_GLOBAL_PROJECT + "') ) " 
        + " ORDER BY FormId; ";

      this.LogDebug ( sqlQueryString );

      //
      // Execute the query against the database      
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
      {
        // 
        // Iterate through the results extracting the role information.
        // 
        for ( int Count = 0; Count < table.Rows.Count; Count++ )
        {
          // 
          // Extract the table row
          // 
          DataRow row = table.Rows [ Count ];

          EvActvityForm activityForm = new EvActvityForm ( );
          activityForm.Guid = Guid.Empty;
          activityForm.FormId = EvSqlMethods.getString ( row, "FormId" );
          activityForm.FormTitle = EvSqlMethods.getString ( row, "TC_Title" );

          view.Add ( activityForm );

        } //END interation loop.

      }//END using method

      // 
      // Update the numbering
      // 
      for ( int count = 0; count < view.Count; count++ )
      {
        ( ( EvActvityForm ) view [ count ] ).Order = count * 5 + 5;
      }

      this.LogValue ( "form object count: " + view.Count );
      // 
      // Return the list containing the User data object.
      // 
      return view;

    }//END getIssuedForms method.

    // ==================================================================================
    /// <summary>
    /// This method generates an option list retrieving from the activity global unique identifier. 
    /// </summary>
    /// <param name="ActivityGuid">Guid: (Mandatory) The activity global unique identifier</param>
    /// <returns>List of EvOption: an arrayList contains TrialVisitForm data objects.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define the SQL query parameters and the sql query string.
    /// 
    /// 2. Execute the sql query command and store the result on the data table.
    /// 
    /// 3. Extract the data row to the Option object. 
    /// 
    /// 4. Add the Option object to the options list. 
    /// 
    /// 5. Return the options list. 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public List<EvOption> getList ( Guid ActivityGuid )
    {
      this.LogMethod ( "getList. " );
      this.LogValue ( "ActivityGuid: " + ActivityGuid );
      //
      // Initialize a method status string, a sql query string, a return option list and an option object
      //
      string sqlQueryString;
      List<EvOption> list = new List<EvOption> ( );
      EvOption option = new EvOption ( );

      list.Add ( option );

      // 
      // Define the SQL query parameters for the option list and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( _parmActivityGuid, SqlDbType.UniqueIdentifier),
      };
      cmdParms [ 0 ].Value = ActivityGuid;

      // 
      // Generate the SQL query string
      // 
      sqlQueryString = _sqlQuery_View + "WHERE AC_Guid = @ActivityGuid "
        + " ORDER BY ACF_Order; ";

      this.LogDebug ( sqlQueryString );

      //
      // Execute the query against the database      
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
      {
        // 
        // Iterate through the results extracting the role information.
        // 
        for ( int Count = 0; Count < table.Rows.Count; Count++ )
        {
          // 
          // Extract the table row
          // 
          DataRow row = table.Rows [ Count ];

          bool bMandatory = EvSqlMethods.getBool ( row, "ACF_Mandatory" );

          option = new EvOption (
            EvSqlMethods.getString ( row, "FormId" ),
           EvSqlMethods.getString ( row, "FormId" ) + " - " + EvSqlMethods.getString ( row, "TC_Title" ) );

          // 
          // Add the mandatory comment if the instrument is mandatory.
          // 
          if ( bMandatory == true )
          {
            option.Description += " (Mandatory)";
          }
          list.Add ( option );

        } //END interation loop.

      }//END using method

      this.LogValue ( "list count: " + list.Count );

      // 
      // Return the ArrayList containing the User data object.
      // 
      return list;

    }//END getList method.

    #endregion

    #region ActivityForm Retrieval methods

    // ==================================================================================
    /// <summary>
    /// This class retrieves the Activity Record table based on Guid. 
    /// </summary>
    /// <param name="Guid">Guid: a global unique identifier</param>
    /// <returns>EvActivityForm: an activity records object</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Return an empty Activity Record object if the Guid is empty
    /// 
    /// 2. Define the sql query parameters and generate the sql query string. 
    /// 
    /// 3. Execute the sql query command and store the result on the data table. 
    /// 
    /// 4. If the data table is not null, extract the data row to the activity record object. 
    /// 
    /// 5. Return the activity record object. 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public EvActvityForm GetItem ( Guid Guid )
    {
      this.LogMethod ( "GetItem. " );
      this.LogValue ( "Guid: " + Guid );
      // 
      // Initialize a sql query string and a return activity record object
      // 
      string sqlQueryString;
      EvActvityForm actiivityForm = new EvActvityForm ( );

      // 
      // Check that the TrialVisitId is valid.
      // 
      if ( Guid == Guid.Empty )
      {
        return actiivityForm;
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter cmdParms = new SqlParameter ( _parmGuid, SqlDbType.UniqueIdentifier );
      cmdParms.Value = Guid;

      // 
      // Generate the SQL query string
      // 
      sqlQueryString = _sqlQuery_View + " WHERE (ACF_Guid = @Guid);";

      this.LogDebug ( sqlQueryString );

      //
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
      {
        // 
        // If not rows the return
        // 
        if ( table.Rows.Count == 0 )
        {
          return actiivityForm;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        actiivityForm = this.readDataRow ( row );

      }//END Using 

      // 
      // Return the TrialVisit data object.
      // 
      return actiivityForm;

    }//END GetItem method.

    #endregion

    #region ActivityForm update methods

    // ==================================================================================
    /// <summary>
    /// This class updates the activity record data object. 
    /// </summary>
    /// <param name="Activity">EvActivity: an activity object</param>
    /// <returns>EvEventCodes: an event code for update data object</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Exit, if the FormId or Activity's Guid or the Old activity object's Guid is empty.
    /// 
    /// 2. Generate the DB row Guid, if it does not exist. 
    /// 
    /// 3. Define the SQL query parameters and execute the storeprocedure for updating items.
    /// 
    /// 4. Return an event code for updating items. 
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public EvEventCodes updateItems ( EvActivity Activity )
    {
      this.LogMethod ( "updateItems " );
      this.LogValue ( "ActivityGuid: " + Activity.Guid );
      this.LogValue ( "Form count: " + Activity.FormList.Count );
      //
      // Initialize the Sql update query string. 
      //
      System.Text.StringBuilder SqlUpdateQuery = new System.Text.StringBuilder ( );
      bool savedItems = false;

      if ( Activity.FormList.Count == 0 )
      {
        this.LogValue ( "No forms in the list" );
        this.LogMethodEnd ( "updateItems " );
        return EvEventCodes.Ok;
      }

      //
      // Delete the milestone activities for this milestone.
      //
      SqlUpdateQuery.AppendLine ( "/** DELETE ALL OF ACTIVITy FORMS FOR THE ACTVITY **/" );
      SqlUpdateQuery.AppendLine ( " DELETE FROM EvActivityForms " );
      SqlUpdateQuery.AppendLine ( " WHERE  (AC_Guid = '" + Activity.Guid + "') ; \r\n" );
      //SqlUpdateQuery.AppendLine ( "GO " );

      foreach ( EvActvityForm activityForm in Activity.FormList )
      {
        //
        // Skip the non selected forms
        //
        if ( activityForm.Selected == false )
        {
          this.LogDebug ( "FormId: " + activityForm.FormId + " >> SKIPPED " );
          continue;
        }
        this.LogDebug ( "FormId: " + activityForm.FormId + " >> ADDED " );
        savedItems = true;

        activityForm.ActivityGuid = Activity.Guid;
        activityForm.ActivityId = Activity.ActivityId;

        if ( activityForm.Guid == Guid.Empty )
        {
          activityForm.Guid = Guid.NewGuid ( );
        }

        int iMandatory = 0;
        if ( activityForm.Mandatory == true )
        {
          iMandatory = 1;
        }

        SqlUpdateQuery.AppendLine ( "Insert Into EvActivityForms " );
        SqlUpdateQuery.AppendLine ( "(ACF_Guid, AC_Guid, FormId, ACF_Order, ACF_Mandatory, ACF_InitialVersion )  " );
        SqlUpdateQuery.AppendLine ( "values  " );
        SqlUpdateQuery.AppendLine ( "('" + activityForm.Guid + "', " );
        SqlUpdateQuery.AppendLine ( "'" + activityForm.ActivityGuid + "', " );
        SqlUpdateQuery.AppendLine ( "'" + activityForm.FormId + "', " );
        SqlUpdateQuery.AppendLine ( " " + activityForm.Order + ", " );
        SqlUpdateQuery.AppendLine ( " " + iMandatory + ",  " );
        SqlUpdateQuery.AppendLine ( " " + activityForm.InitialVersion + " ); \r\n" );

      }//END form list iteration loop.

      if ( savedItems == false )
      {
        this.LogMethodEnd ( "updateItems " );
        return EvEventCodes.Ok;
      }
      this.LogDebug ( "Sql Query: " + SqlUpdateQuery.ToString ( ) );

      if ( EvSqlMethods.QueryUpdate ( SqlUpdateQuery.ToString ( ), null ) == 0 )
      {
        this.LogError ( EvEventCodes.Database_Record_Update_Error, "EvActivityForms databasse update error." );
        this.LogMethodEnd ( "updateItems " );
        return EvEventCodes.Database_Record_Update_Error;
      }

      // 
      // Return code
      //       
      this.LogMethodEnd ( "updateItems " );
      return EvEventCodes.Ok;

    }//END updateItem class

    #endregion

  }//END EvActivityForms class

}//END namespace Evado.Dal.Clinical
