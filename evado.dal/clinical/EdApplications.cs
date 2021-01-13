/* <copyright file="TrialVisit.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2020 EVADO HOLDING PTY. LTD..  All rights reserved.
 *     
 *      The use and distribution terms for this software are contained in the file
 *      named license.txt, which can be found in the root of this distribution.""
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
using System.Xml.Serialization;
using System.IO;

//References to Evado.Evado. specific libraries
using Evado.Model;
using Evado.Model.Digital;


namespace Evado.Dal.Clinical
{
  //  ====================================================================================
  /// <summary>
  /// The data access layer class to manage the database interface for the Project Object.
  /// </summary>
  // -------------------------------------------------------------------------------------
  public class EdApplications : EvDalBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EdApplications ( )
    {
      this.ClassNameSpace = "Evado.Dal.Clinical.EvTrials.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EdApplications ( EvClassParameters Settings )
    {
      this.ClassParameters = Settings;
      this.ClassNameSpace = "Evado.Dal.Clinical.EvTrials.";

      this.LogDebug ( "CustomerGuid: " + this.ClassParameters.CustomerGuid );
    }

    #endregion

    #region Constants and global class objects and variables.

    /// <summary>
    /// This constat selects all rows from a view EvTrial_View.
    /// </summary>
    private const string SQL_SELECT_QUERY = "Select * FROM ED_APPLICATION_SETTINGS ";

    /// <summary>
    /// This constant defines a stored procedure for adding a Project.
    /// </summary>
    private const string STORED_PROCEDURE_ADD_TRIAL = "USR_APPLICATION_SETTINGS_ADD";

    /// <summary>
    /// This constant defines a stored procedure for updating a Project. 
    /// </summary>
    private const string STORED_PROCEDURE_UPDATE_TRIAL = "USR_APPLICATION_SETTINGS_UPDATE";

    /// <summary>
    /// This constant defines a stored procedure for deleting a Project. 
    /// </summary>
    private const string STORED_PROCEDURE_DELETE_TRIAL = "USR_APPLICATION_SETTINGS_DELETE";

    //
    // Define the SQL parameter constants.
    //
   

    private const string DB_CUSTOMER_GUID = "CU_GUID";

    private const string PARM_Guid = "@Guid";
    private const string PARM_CUSTOMER_GUID = "@CUSTOMER_GUID";
    private const string PARM_APPLICATION_GUID = "@APPLICATION_GUID";
    private const string PARM_APPLICATON_ID = "@APPLICATION_ID";
    private const string PARM_STATE = "@STATE";
    private const string PARM_Title = "@Title";
    private const string PARM_HTTP_REFERENCE = "@HTTP_REFERENCE";
    private const string PARM_DESCRIPTION = "@DESCRIPTION";
    private const string PARM_CONFIRMATION_EMAIL_TITLE = "@CONFIRMATION_EMAIL_TITLE";
    private const string PARM_CONFIRMATION_EMAIL_BODY = "@CONFIRMATION_EMAIL_BODY";
    private const string PARM_UPDATE_USER_ID = "@UPDATE_USER_ID";
    private const string PARM_UPDATE_USER = "@UPDATE_USER";
    private const string PARM_UPDATE_DATE = "@UPDATE_DATE";
    private const string PARM_SIGN_OFFS = "@SIGN_OFFS";

    /// <summary>
    /// This variable contains a sql query string.
    /// </summary>
    private string _sqlQueryString = String.Empty;

    // ++++++++++++++++++++++++ END FORM OBJECT INITIALISATION SECTION ++++++++++++++++++++
    #endregion

    #region SQL Parameter Section

    // =====================================================================================
    /// <summary>
    ///  This method returns an array of sql query parameters.
    /// </summary>
    /// <returns>SqlParameter: An array of sql query parameters</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Create an array of sql query parameters. 
    /// 
    /// 2. Return an array of sql query parameters.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private static SqlParameter [ ] GetParameters ( )
    {
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter( PARM_Guid, SqlDbType.UniqueIdentifier),
        new SqlParameter( PARM_CUSTOMER_GUID, SqlDbType.UniqueIdentifier),
        new SqlParameter( PARM_STATE,SqlDbType.NVarChar, 20),
        new SqlParameter( PARM_Title, SqlDbType.NVarChar, 50),
        new SqlParameter( PARM_HTTP_REFERENCE, SqlDbType.NVarChar, 250),
        new SqlParameter( PARM_DESCRIPTION, SqlDbType.NText),
        new SqlParameter( PARM_CONFIRMATION_EMAIL_TITLE, SqlDbType.NVarChar,100),
        new SqlParameter( PARM_CONFIRMATION_EMAIL_BODY, SqlDbType.NText),
        new SqlParameter( PARM_SIGN_OFFS, SqlDbType.NText),
        new SqlParameter( PARM_UPDATE_USER_ID,SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_UPDATE_USER,SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_UPDATE_DATE,SqlDbType.DateTime ),
      };

      return cmdParms;
    }

    // =====================================================================================
    /// <summary>
    /// This method assigns the Project object's values to the array of sql query parameters.
    /// </summary>
    /// <param name="cmdParms">SqlParameter: an array of sql query parameters</param>
    /// <param name="Application">EvTrial: A Project object</param>
    /// <remarks>
    /// This method consists of the following step:
    /// 
    /// 1. Bind the Project Object's values to the array of sql query parameters.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private void SetParameters ( SqlParameter [ ] cmdParms, EdApplication Application )
    {
      // 
      // Set the parameter values.
      // 
      cmdParms [ 0 ].Value = Application.Guid;
      cmdParms [ 1 ].Value = this.ClassParameters.CustomerGuid;
      cmdParms [ 2 ].Value = Application.State.ToString ( );
      cmdParms [ 3 ].Value = Application.Title;
      cmdParms [ 4 ].Value = Application.HttpReference;
      cmdParms [ 5 ].Value = Application.Description;
      cmdParms [ 6 ].Value = Application.ConfirmationEmailSubject;
      cmdParms [ 7 ].Value = Application.ConfirmationEmailBody;
      cmdParms [ 8 ].Value = Evado.Model.EvStatics.SerialiseObject<List<EvUserSignoff>> ( Application.Signoffs );
      cmdParms [ 9 ].Value = Application.UpdatedByUserId;
      cmdParms [ 10 ].Value = Application.UserCommonName;
      cmdParms [ 11 ].Value = DateTime.Now; //Update Date
    }//END SetParameters method   

    #endregion

    #region Data Reader Section

    // =====================================================================================
    /// <summary>
    /// This method extracts the data row values to the Project object
    /// </summary>
    /// <param name="Row">DataRow: A data row object</param>
    /// <returns>EvTrial: A Project object. </returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Extract the compatible data row values to the Project object.
    /// 
    /// 2. Return the Project Object.
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private EdApplication getRowData ( DataRow Row )
    {
      // 
      // Initialise local variables
      // 
      EdApplication applicationSetting = new EdApplication ( );
      String value = String.Empty;
      // 
      // Extract the data values.
      // 
      applicationSetting.Guid = EvSqlMethods.getGuid ( Row, "AS_Guid" );
      applicationSetting.CustomerGuid = EvSqlMethods.getGuid ( Row, "CU_Guid" );

      applicationSetting.ApplicationId = EvSqlMethods.getString ( Row, "APPLICATION_ID" );
      applicationSetting.Title = EvSqlMethods.getString ( Row, "AS_Title" );
      applicationSetting.HttpReference = EvSqlMethods.getString ( Row, "AS_HTTP_REFERENCE" );
      applicationSetting.Description = EvSqlMethods.getString ( Row, "AS_DESCRIPTION" );
      value = EvSqlMethods.getString ( Row, "AS_SIGN_OFFS" );
      if ( value != String.Empty
        && value.Length > 41 )
      {
        applicationSetting.Signoffs = Evado.Model.Digital.EvcStatics.DeserialiseObject<List<EvUserSignoff>> ( value );
      }

      if ( applicationSetting.Signoffs == null )
      {
        applicationSetting.Signoffs = new List<EvUserSignoff> ( );
      }
      applicationSetting.State =
          Evado.Model.EvStatics.Enumerations.parseEnumValue<EdApplication.ApplicationStates> ( EvSqlMethods.getString ( Row, "AS_State" ) );

      applicationSetting.ConfirmationEmailSubject = EvSqlMethods.getString ( Row, "AS_CONFIRMATION_EMAIL_TITLE" );
      applicationSetting.ConfirmationEmailBody = EvSqlMethods.getString ( Row, "AS_CONFIRMATION_EMAIL_BODY" );

      applicationSetting.Updated = EvSqlMethods.getString ( Row, "AS_UPDATE_USER" );
      applicationSetting.Updated += " on " + EvSqlMethods.getDateTime ( Row, "AS_UPDATE_DATE" ).ToString ( "dd MMM yyyy HH:mm" );

      //
      // Return an EvProject object.
      //
      return applicationSetting;

    }// End getRowData method.
  
    #endregion

    #region Query/selectionList Methods Section

    // =====================================================================================
    /// <summary>
    /// This method returns a list of Project object based on the passed parameters.
    /// </summary>
    /// <param name="State">EvTrial.TrialStates: A Project State.</param>
    /// <param name="StateValueNotSelected">Boolean: true, if the state value is not selected.</param>
    /// <returns>List of EvTrial: A list containing EvTrial objects</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Define the sql query parameters and sql query string. 
    /// 
    /// 2. Execute the sql query string and store the results on datatable. 
    /// 
    /// 3. Loop through the table and extract data row to the Project Object. 
    /// 
    /// 4. Add the Project object's values to the Trials list. 
    /// 
    /// 5. Return the Trials list.
    /// </remarks>
    ///
    // -------------------------------------------------------------------------------------
    public List<EdApplication> GetApplicationList (
      EdApplication.ApplicationStates State,
      bool StateValueNotSelected )
    {
      this.LogMethod ( "GetTrialList method. " );
      this.LogDebug ( "CustomerGuid: " + this.ClassParameters.CustomerGuid );
      this.LogDebug ( "State: " + State );
      this.LogDebug ( "NotSelect: " + StateValueNotSelected );
      // 
      // Define the local variables
      // 
      List<EdApplication> view = new List<EdApplication> ( );

      // 
      // Generate the parameters for the query.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
        {
        new SqlParameter ( EdApplications.PARM_CUSTOMER_GUID, SqlDbType.UniqueIdentifier)
      };
      cmdParms [ 0 ].Value = this.ClassParameters.CustomerGuid;

      // 
      // Generate the SQL query string.
      // 
      _sqlQueryString = SQL_SELECT_QUERY
        + " WHERE  ( " + EdApplications.DB_CUSTOMER_GUID + " = " + EdApplications.PARM_CUSTOMER_GUID + " )\r\n";

      if ( State != EdApplication.ApplicationStates.All
        && State != EdApplication.ApplicationStates.Null )
      {
          if ( StateValueNotSelected == true )
          {
            _sqlQueryString += " AND NOT(AS_State = '" + State + "') ";
          }
          else
          {
            _sqlQueryString += " AND (AS_State = '" + State + "') ";
          }
      }
      _sqlQueryString += " ORDER BY APPLICATION_ID";

      this.LogDebug ( _sqlQueryString );

      try
      {
        // 
        // Execute the query against the database.
        // 
        using ( DataTable table = EvSqlMethods.RunQuery ( _sqlQueryString, cmdParms ) )
        {
          // 
          // Iterate through the results extracting the role information.
          // 
          for ( int Count = 0; Count < table.Rows.Count; Count++ )
          {
            // 
            // Extract the table row.
            // 
            DataRow row = table.Rows [ Count ];

            EdApplication application = this.getRowData ( row );

            //
            // load the application parameters.
            //
            application.Parameters = this.LoadObjectParameters ( application.Guid );

            view.Add ( application );
          }
        }
      }
      catch ( Exception Ex )
      {
        this.LogException ( Ex );
      }
      // 
      // Append the query result to the list.
      // 
      this.LogDebug ( "View Count: " + view.Count );

      //
      // Return a list of EvTrial object. 
      //
      this.LogMethodEnd ( "GetTrialList" );
      return view;

    }//END GetTrialList method.

    // =====================================================================================
    /// <summary>
    /// This method returns a list of selected options for queried Project object. 
    /// </summary>
    /// <param name="State">EvTrial.TrialStates: A Project State.</param>
    /// <param name="StateValueNotSelected">Boolean: true, if the state value is not selected.</param>
    /// <param name="WithGuidValue">Boolean: true, if the Project is selected with Guid value</param>
    /// <returns>List of EvTrial: A list containing EvTrial objects</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Define the sql query parameters and sql query string. 
    /// 
    /// 2. Execute the sql query string and store the results on datatable. 
    /// 
    /// 3. Loop through the table and extract data row to the Option Object. 
    /// 
    /// 4. Add the Option object's values to the Options list. 
    /// 
    /// 5. Return the Options list.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> getOptionList (
      EdApplication.ApplicationStates State,
      bool StateValueNotSelected,
      bool WithGuidValue )
    {
      this.LogMethod ( "getOptionList method. " );
      this.LogDebug ( "State: " + State );
      this.LogDebug ( "NotSelect=" + StateValueNotSelected );

      // 
      // Define the local variables.
      // 
      List<EvOption> list = new List<EvOption> ( );

      // 
      // Add the null object to list of EvOption object.
      // 
      EvOption option = new EvOption ( );
      list.Add ( option );

      var studyList = this.GetApplicationList (
                        State,
                        StateValueNotSelected );


      foreach ( EdApplication study in studyList )
      {
        //  
        //  Process the query result.
        //  
        if ( WithGuidValue == true )
        {
          option = new EvOption ( study.Guid.ToString ( ),
            String.Format ( "{0} - {1}", study.ApplicationId, study.Title ) );
        }
        else
        {
          option = new EvOption ( study.ApplicationId,
            String.Format ( "{0} - {1}", study.ApplicationId, study.Title ) );
        }

        // 
        // Append the SelectionObject object to the ArrayList.
        // 
        list.Add ( option );


      }//END using method

      this.LogDebug ( " list count: " + list.Count );

      // 
      // Return the selection trial selection list.
      // 
      this.LogMethodEnd ( "getOptionList" );
      return list;

    }//END getOptionList method

    #endregion

    #region Data Object Retrieval Methods Section

    // =====================================================================================
    /// <summary>
    ///  This methods retrieves Project data table based on Guid.
    /// </summary>
    /// <param name="ApplicationGuid">GUID: A Project's global unique identifier</param>
    /// <returns>EvProject: A Project object</returns>
    /// <remarks>
    /// This method consists of following methods. 
    /// 
    /// 1. Return an empty Project object, if the Guid is empty.
    /// 
    /// 2. Define the sql query parameters and sql query string. 
    /// 
    /// 3. Execute the sql query string and store the results on datatable. 
    /// 
    /// 4. Return an empty Project object, if the table has no value. 
    /// 
    /// 5. Else, extract the first row to the Project object. 
    /// 
    /// 6. Update the actual recruitment cound and schedule to the Project object. 
    /// 
    /// 7. Return the Project object. 
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EdApplication GetApplication ( Guid ApplicationGuid )
    {
      this.LogMethod ( "GetApplication method." );
      this.LogDebug ( "ApplicationGuid: " + ApplicationGuid );
      // 
      // Define the local variables
      // 
      EdApplication application = new EdApplication ( );

      // 
      // Check that the data object has a valid trial object unique identifier.
      // 
      if ( ApplicationGuid == Guid.Empty )
      {
        this.LogMethodEnd ( "GetApplication." );
        return application;
      }

      // 
      // Define the query prarameters.
      //
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter ( EdApplications.PARM_CUSTOMER_GUID, SqlDbType.UniqueIdentifier),
        new SqlParameter ( EdApplications.PARM_Guid, SqlDbType.UniqueIdentifier)
      };
      cmdParms [ 0 ].Value = this.ClassParameters.CustomerGuid;
      cmdParms [ 1 ].Value = ApplicationGuid;

      // 
      // Generate the SQL statement for the query.
      // 
      _sqlQueryString = SQL_SELECT_QUERY
        + "WHERE (" + EdApplications.DB_CUSTOMER_GUID + " = " + EdApplications.PARM_CUSTOMER_GUID + " )\r\n"
        + " AND (AS_GUID = " + EdApplications.PARM_Guid + ")\r\n";

      // 
      // Execute the query against the database.
      // 
      using ( DataTable table = EvSqlMethods.RunQuery ( _sqlQueryString, cmdParms ) )
      {
        // 
        // If no rows found, return an EvTrial object
        // 
        if ( table.Rows.Count == 0 )
        {
          this.LogMethodEnd ( "GetApplication." );
          return application;
        }

        // 
        // Extract the table row.
        // 
        DataRow row = table.Rows [ 0 ];

        // 
        // Fill the EvTrial object.
        // 
        application = this.getRowData ( row );

      }//END Using 

      //
      // load the application parameters.
      //
      application.Parameters = this.LoadObjectParameters ( application.Guid );

      this.LogDebug ( "State: " + application.State );
      // 
      // Return the EvStudy data object.
      // 
      this.LogMethodEnd ( "GetApplication." );
      return application;

    }//END GetTrial method

    // ===================================================================================
    /// <summary>
    ///  This methods retrieves Project data table based on VisitId.
    /// </summary>
    /// <param name="ApplicationId">string: A Project identifier</param>
    /// <returns>EvProject: A Project object</returns>
    /// <remarks>
    /// This method consists of following methods. 
    /// 
    /// 1. Return an empty Project object, if the Project is empty.
    /// 
    /// 2. Define the sql query parameters and sql query string. 
    /// 
    /// 3. Execute the sql query string and store the results on datatable. 
    /// 
    /// 4. Return an empty Project object, if the table has no value. 
    /// 
    /// 5. Else, extract the first row to the Project object. 
    /// 
    /// 6. Update the actual recruitment cound and schedule to the Project object. 
    /// 
    /// 7. Return the Project object. 
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EdApplication GetApplication ( string ApplicationId )
    {
      this.LogMethod ( "GetApplication method." );
      this.LogDebug ( "ApplicationId: " + ApplicationId );
      // 
      // Define the local variables.
      // 
      EvSchedules schedules = new EvSchedules ( );
      EdApplication application = new EdApplication ( );

      // 
      // Check that the trial data object has a valid VisitId.
      // 
      if ( ApplicationId == String.Empty )
      {
        this.LogMethodEnd ( "GetApplication." );
        return application;
      }
      // 
      // Define the query prarameters.
      //
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter ( EdApplications.PARM_CUSTOMER_GUID, SqlDbType.UniqueIdentifier),
        new SqlParameter ( EdApplications.PARM_APPLICATON_ID, SqlDbType.NVarChar, 10 )
      };
      cmdParms [ 0 ].Value = this.ClassParameters.CustomerGuid;
      cmdParms [ 1 ].Value = ApplicationId;

      // 
      // Generate the SQL statement for the query.
      // 
      _sqlQueryString = SQL_SELECT_QUERY
        + "WHERE (" + EdApplications.DB_CUSTOMER_GUID + " = " + EdApplications.PARM_CUSTOMER_GUID + " )\r\n"
        + " AND (APPLICATION_ID = " + EdApplications.PARM_APPLICATON_ID + ")\r\n";

      // 
      // Execute the query against the database.
      // 
      using ( DataTable table = EvSqlMethods.RunQuery ( _sqlQueryString, cmdParms ) )
      {
        // 
        // If no rows found, return an Evtrial object.
        // 
        if ( table.Rows.Count == 0 )
        {
          this.LogMethodEnd ( "GetApplication." );
          return application;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        // 
        // Fill the EvTrial object.
        // 
        application = this.getRowData ( row );

      }//END Using 

      //
      // load the application parameters.
      //
      application.Parameters = this.LoadObjectParameters ( application.Guid );

      // 
      // Return the EvTrial object.
      // 
      this.LogMethodEnd ( "GetApplication." );
      return application;

    }//END GetStudy method

    #endregion

    #region Data Object Update Section

    // ===================================================================================
    /// <summary>
    /// This method updates items to the Project data table.
    /// </summary>
    /// <param name="Application">EvTrial: A Project object.</param>
    /// <returns>EvEventCodes: An event code for updating items</returns>
    /// <remarks>
    /// This method consists of following steps.
    /// 
    /// 1. Exit, if the VisitId or OldTrialId is empty.
    /// 
    /// 2. Add items to datachange object, if they do not exist on the Old Project Object. 
    /// 
    /// 3. Set the sql query parameters and execute the storeprocedure for updating items. 
    /// 
    /// 4. Exit, if the storeprocedure runs fail. 
    /// 
    /// 5. Update the related Project's Arm object and exit, if the updating runs fail. 
    /// 
    /// 6. Add datachange object's values to the backup datachanges object.
    /// 
    /// 7. Return an event code for updating items. 
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes updateItem ( EdApplication Application )
    {
      this.LogMethod ( "updateItem method." );
      this.LogDebug ( "Guid: " + Application.Guid );
      this.LogDebug ( "ApplicationId: " + Application.ApplicationId );
      this.LogDebug ( "State: " + Application.State );
      // 
      // Initialise the methods variables and objects.
      // 
      EvDataChanges dataChanges = new EvDataChanges ( );

      // 
      // Validate whether the VisitId or Old VisitId is not empty.
      // 
      if ( Application.ApplicationId == String.Empty )
      {
        return EvEventCodes.Identifier_Project_Id_Error;
      }

      EdApplication oldApp = this.GetApplication ( Application.Guid );
      if ( oldApp.Guid == Guid.Empty )
      {
        return EvEventCodes.Data_Null_Data_Error;
      }

      // 
      // Compare the objects.
      // 
      EvDataChange dataChange = this.setChangeRecord ( Application, oldApp );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = GetParameters ( );
      SetParameters ( cmdParms, Application );

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( STORED_PROCEDURE_UPDATE_TRIAL, cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      this.UpdateObjectParameters ( Application.Parameters, Application.Guid );

      // 
      // Store the datachange object 
      // 
      dataChanges.AddItem ( dataChange );
      this.LogClass ( dataChanges.Log );

      // 
      // Return the Ok event code
      // 
      return EvEventCodes.Ok;

    }//END updateItem method   

    // ===================================================================================
    /// <summary>
    /// This method adds items to the Project data table. 
    /// </summary>
    /// <param name="Application">EvStudy: A Project Object</param>
    /// <returns>EvEventCodes: An event code for adding items.</returns>
    /// <remarks>
    /// This method consists of folloing steps. 
    /// 
    /// 1. Exit, if the VisitId or Guid is empty. 
    /// 
    /// 2. Create new DB row Guid. 
    /// 
    /// 3. Exit, if the Project's Guid is duplicated
    /// 
    /// 3. Define the sql query parameters and execute the storeprocedure for adding items. 
    /// 
    /// 4. Exit, if the storeprocedure runs fail. 
    /// 
    /// 5. Else, return an event code for adding items. 
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes AddApplication ( EdApplication Application )
    {
      this.LogMethod ( "AddStudy method." );
      this.LogDebug ( "Guid: " + Application.Guid );
      this.LogDebug ( "CustomerGuid: " + Application.CustomerGuid );
      this.LogDebug ( "StudyId: " + Application.ApplicationId );
      this.LogDebug ( "State: " + Application.State );
      // 
      // Initialise the methods variables and objects.
      // 
      EvDataChanges dataChanges = new EvDataChanges ( );

      // 
      // If a Guid of Project is not equal to empty guid, return a Data_Null_Data_Error message. 
      // 
      if ( Application.Guid != Guid.Empty )
      {
        return EvEventCodes.Data_Null_Data_Error;
      }

      // 
      // Create the Guid for the Project object.
      // 
      Application.Guid = Guid.NewGuid ( );

      //Study.StudyId = this.generateStudyId ( );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = GetParameters ( );
      SetParameters ( cmdParms, Application );

      this.LogDebug ( EvSqlMethods.ListParameters ( cmdParms ) );
      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( STORED_PROCEDURE_ADD_TRIAL, cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      // 
      // Return the Ok event code
      //
      return EvEventCodes.Ok;

    }//END AddStudy method

    // ===================================================================================
    /// <summary>
    ///  This method delets items from the Project data table.
    /// </summary>
    /// <param name="Application">EvTrial: A Project object.</param>
    /// <returns>EvEventCodes: An Event code for deleting items</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Exit, if the Guid or UserCommonName is empty. 
    /// 
    /// 2. Define the sql query parameters and execute the storeprocedure for deleting items. 
    /// 
    /// 3. Exit, if the storeprocedure runs fail. 
    /// 
    /// 4. Else, return an event code for deleting items. 
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes DeleteApplication ( EdApplication Application )
    {
      // 
      // Check that the Project Guid and Project UserCommonName is valid.
      // 
      if ( Application.Guid == Guid.Empty )
      {
        return EvEventCodes.Data_Null_Data_Error;
      }

      if ( Application.UserCommonName == String.Empty )
      {
        return EvEventCodes.Identifier_User_Id_Error;
      }

      // 
      // Generate the parameters for the query.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
        {
          new SqlParameter(PARM_Guid, SqlDbType.UniqueIdentifier),
          new SqlParameter(PARM_UPDATE_USER_ID, SqlDbType.NVarChar,100),
          new SqlParameter(PARM_UPDATE_USER, SqlDbType.NVarChar,30),
          new SqlParameter(PARM_UPDATE_DATE, SqlDbType.DateTime )
        };
      cmdParms [ 0 ].Value = Application.Guid;
      cmdParms [ 1 ].Value = Application.UpdatedByUserId;
      cmdParms [ 2 ].Value = Application.UserCommonName;
      cmdParms [ 3 ].Value = DateTime.Now;

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( STORED_PROCEDURE_DELETE_TRIAL, cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      //
      // Return the Ok event code
      //
      return EvEventCodes.Ok;

    }//END deleteTrial method.

    // =====================================================================================
    /// <summary>
    ///  This method creates the data change object for a trial update.  The object contains
    ///  the difference between the exist and new update.
    /// </summary>
    /// <param name="Application">EvTrial: A Project object</param>
    /// <param name="OldAplication">EvTrial: An Old Project Object</param>
    /// <returns>EvDataChange: A Datachange object</returns>
    /// <remarks>
    /// This method consists of following steps.
    /// 
    /// 1. Add items to datachange object, if they do not exist on the old Project object. 
    /// 
    /// 2. Return a datachange object.
    /// 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private EvDataChange setChangeRecord (
      EdApplication Application,
      EdApplication OldAplication )
    {
      // 
      // Initialise the method's variables and objects.
      // 
      EvDataChange dataChange = new EvDataChange ( );
      this.LogMethod ( "setChangeRecord method. " );

      // 
      // Set the data changes values.
      // 
      dataChange.Guid = Guid.NewGuid ( );
      dataChange.TableName = EvDataChange.DataChangeTableNames.EdApplicationSettings;
      dataChange.TrialId = Application.ApplicationId;
      dataChange.RecordGuid = Application.Guid;
      dataChange.UserId = Application.UpdatedByUserId;
      dataChange.DateStamp = DateTime.Now;

      //
      // Add items to datachange object, if they do not exist on the old Project object.
      //
      if ( Application.Title != OldAplication.Title )
      {
        dataChange.AddItem ( "Title", OldAplication.Title, Application.Title );
      }
      if ( Application.HttpReference != OldAplication.HttpReference )
      {
        dataChange.AddItem ( "HttpReference", OldAplication.HttpReference, Application.HttpReference );
      }
      if ( Application.Description != OldAplication.Description )
      {
        dataChange.AddItem ( "Description", OldAplication.Description, Application.Description );
      }
      if ( Application.State != OldAplication.State )
      {
        dataChange.AddItem ( "State", OldAplication.State.ToString(), Application.State.ToString() );
      }
      if ( Application.ConfirmationEmailSubject != OldAplication.ConfirmationEmailSubject )
      {
        dataChange.AddItem ( "ConfirmationEmailSubject", OldAplication.ConfirmationEmailSubject, Application.ConfirmationEmailSubject );
      }
      if ( Application.ConfirmationEmailBody != OldAplication.ConfirmationEmailBody )
      {
        dataChange.AddItem ( "ConfirmationEmailBody", OldAplication.ConfirmationEmailBody, Application.ConfirmationEmailBody );
      }


      this.LogDebug ( " setChangeRecord method completed. " );
      // 
      // Return the Data change object
      // 
      return dataChange;

    }//END setChangeRecord method
    #endregion

  } //END Studies class.

}//END  namespace Evado.Dal.Clinical