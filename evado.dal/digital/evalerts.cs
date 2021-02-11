/* <copyright file="DAL\TrialAlerts.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class handles the database query interface for the trial object.
 * 
 *  This class contains the following public properties:
 *   DebugLog:       Containing the exeuction status of this class, used for debugging the 
 *                 class from BLL or UI layers.
 * 
 *  This class contains the following public methods:
 *   getView:      Executes a selectionList quey returning an ArrayList of EvRecord objects.
 * 
 *   GetList:      Executes a query to generate an ArrayList of Selection OptionsOrUnit objects.
 * 
 *   getItem:  Executes a query to return a EvRecord object by the trial and visit id. 
 * 
 *   getItem: Executes a query to return a EvRecord object by it FormUid.
 * 
 *   updateTestReport:   Executes an query to update the database content and generate an date change
 *                 entry for the update.
 * 
 *   CreateItem:   Executes an query to create a EvRecord object to the database and return
 *                 its record identifier.
 * 
 *   createItem:      Executes an query to add a new EvRecord object to the database and return
 *                 its record identifier.
 * 
 *   DeleteItem:   Executes a query to logically delete the trial visit from the database.
 * 
 ****************************************************************************************/

using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;

//Application specific class references.
using Evado.Model;
using Evado.Model.Digital;


namespace Evado.Dal.Digital
{
  /// <summary>
  /// This class is handles the data access layer for the alert data object.
  /// </summary>
  public class EvAlerts : EvDalBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EvAlerts ( )
    {
      this.ClassNameSpace = "Evado.Dal.Clinical.EvAlerts.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EvAlerts ( EvClassParameters Settings )
    {
      this.ClassParameters = Settings;
      this.ClassNameSpace = "Evado.Dal.Clinical.EvAlerts.";

      if ( this.ClassParameters.LoggingLevel == 0 )
      {
        this.ClassParameters.LoggingLevel = Evado.Dal.EvStaticSetting.LoggingLevel;
      }
    }

    #endregion

    #region Class Initialization

    private static string _eventLogSource = ConfigurationManager.AppSettings [ "EventLogSource" ];
    #endregion

    #region Class Constants

    #region selectionList query string.

    /// <summary>
    /// This constant defines an sql query view. 
    /// </summary>
    private const string _sqlQuery_View = "Select  * FROM EvAlert_View ";
    #endregion

    #region Define the stored procedure names.

    /// <summary>
    /// This constant define a stored procedure for add item. 
    /// </summary>
    private const string _STORED_PROCEDURE_AddItem = "usr_Alert_add";

    /// <summary>
    /// This constant define a stored procedure for update item. 
    /// </summary>
    private const string _STORED_PROCEDURE_UpdateItem = "usr_Alert_update";

    /// <summary>
    /// This constant define a stored procedure for delete item. 
    /// </summary>
    private const string _STORED_PROCEDURE_DeleteItem = "usr_Alert_delete";

    #endregion

    #region Define the query parameter constants.

    /// <summary>
    /// This constant define a parameter for a global unique identifier  
    /// </summary>
    private const string _parmGuid = "@Guid";

    /// <summary>
    /// This constant define a parameter for a unique identifier
    /// </summary>
    private const string _parmUid = "@Uid";

    /// <summary>
    /// This constant define a parameter for a serial 
    /// </summary>
    private const string _parmSerial = "@Serial";

    /// <summary>
    /// This constant define a parameter for an alert identifier 
    /// </summary>
    private const string _parmAlertId = "@AlertId";

    /// <summary>
    /// This constant define a parameter for a trial identifier 
    /// </summary>
    private const string _parmTrialId = "@TrialId";

    /// <summary>
    /// This constant define a parameter for a record identifier 
    /// </summary>
    private const string _parmRecordId = "@RecordId";

    /// <summary>
    /// This constant define a parameter for an organization identifier 
    /// </summary>
    private const string _parmOrgId = "@OrgId";

    /// <summary>
    /// This constant define a parameter for a milestone 
    /// </summary>
    private const string _parmSubject = "@Subject";

    /// <summary>
    /// This constant define a parameter for a message 
    /// </summary>
    private const string _parmMessage = "@Message";

    /// <summary>
    /// This constant define a parameter for a reference 
    /// </summary>
    private const string _parmReference = "@Reference";

    /// <summary>
    /// This constant define a parameter for a user identifer of those who generates an alert 
    /// </summary>
    private const string _parmFromUserUserId = "@FromUserUserId";

    /// <summary>
    /// This constant define a parameter for a user who generate an alert 
    /// </summary>
    private const string _parmFromUser = "@FromUser";

    /// <summary>
    /// This constant define a parameter for raised alert  
    /// </summary>
    private const string _parmRaised = "@Raised";

    /// <summary>
    /// This constant define a parameter for a user who receive an alert  
    /// </summary>
    private const string _parmToUser = "@ToUser";

    /// <summary>
    /// This constant define a parameter for a user identifier of those who acknowledged the alert 
    /// </summary>
    private const string _parmAcknowledgedByUserId = "@AcknowledgedByUserId";

    /// <summary>
    /// This constant define a parameter for a user who acknowledge an alert
    /// </summary>
    private const string _parmAcknowledgedBy = "@AcknowledgedBy";

    /// <summary>
    /// This constant define a parameter for an acknowledged alert 
    /// </summary>
    private const string _parmAcknowledged = "@Acknowledged";

    /// <summary>
    /// This constant define a parameter for a user who closes an alert 
    /// </summary>
    private const string _parmClosedBy = "@ClosedBy";

    /// <summary>
    /// This constant define a parameter for a user identifier of those who close an alert 
    /// </summary>
    private const string _parmClosedByUserId = "@ClosedByUserId";

    /// <summary>
    /// This constant define a parameter for a closed alert 
    /// </summary>
    private const string _parmClosed = "@Closed";

    /// <summary>
    /// This constant define a parameter for a type identifier 
    /// </summary>
    private const string _parmTypeId = "@TypeId";

    /// <summary>
    /// This constant define a parameter for a state of an alert 
    /// </summary>
    private const string _parmState = "@State";

    /// <summary>
    /// This constant define a parameter for a user identifier of those who updates an alert 
    /// </summary>
    private const string _parmUpdatedByUserId = "@UpdatedByUserId";

    /// <summary>
    /// This constant define a parameter for a user who updates an alert 
    /// </summary>
    private const string _parmUpdatedBy = "@UpdatedBy";

    /// <summary>
    /// This constant define a parameter for an update date 
    /// </summary>
    private const string _parmUpdateDate = "@UpdateDate";

    /// <summary>
    /// This constant define a parameter for a user name of those who update an alert 
    /// </summary>
    private const string _parmUserName = "@UserName";

    /// <summary>
    /// This constant define a parameter for a booked out alert 
    /// </summary>
    private const string _parmBookedOut = "@BookedOutBy";
    #endregion

    #endregion

    #region Internal member variables
    //
    //  Define the SQL query string variable.
    //  
    private string _sqlQueryString = String.Empty;

    #endregion

    #region Class Property

    private string _status = String.Empty;
    /// <summary>
    /// This property contains the alert status.
    /// </summary>
    public string Status
    {
      get
      {
        return _status;
      }
    }

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region SQL Parameter methods

    // ==================================================================================
    /// <summary>
    /// This class defines the SQL parameter for a query.     
    /// </summary>
    /// <returns>SqlParameter: an array object of the sql parameters</returns>
    /// <remarks>
    /// This method consists of the following step:
    /// 
    /// 1. Create an array of sql parameters.
    /// 
    /// 2. Return an array of sql query parameters.
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    private static SqlParameter [ ] GetParameters ( )
    {
      //
      // Create a new array object of sql parameters.
      //
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
			{
        new SqlParameter( _parmGuid, SqlDbType.UniqueIdentifier),
				new SqlParameter( _parmAlertId, SqlDbType.NVarChar, 20),
        new SqlParameter( _parmTrialId, SqlDbType.NVarChar, 10),
        new SqlParameter( _parmRecordId, SqlDbType.NVarChar, 20),
        new SqlParameter( _parmOrgId, SqlDbType.NVarChar, 10),
        new SqlParameter( _parmSubject, SqlDbType.NVarChar, 80),
        new SqlParameter( _parmMessage, SqlDbType.NText),
        new SqlParameter( _parmFromUserUserId, SqlDbType.NVarChar, 100),
        new SqlParameter( _parmFromUser, SqlDbType.NVarChar, 100),
        new SqlParameter( _parmToUser, SqlDbType.NVarChar, 100),
        new SqlParameter( _parmRaised, SqlDbType.DateTime),
        new SqlParameter( _parmAcknowledgedByUserId, SqlDbType.NVarChar, 100),
        new SqlParameter( _parmAcknowledgedBy, SqlDbType.NVarChar, 100),
        new SqlParameter( _parmAcknowledged, SqlDbType.DateTime),
        new SqlParameter( _parmClosedByUserId, SqlDbType.NVarChar, 100),
        new SqlParameter( _parmClosedBy, SqlDbType.NVarChar, 100),
        new SqlParameter( _parmClosed, SqlDbType.DateTime),
        new SqlParameter( _parmTypeId, SqlDbType.NVarChar, 30),
        new SqlParameter( _parmState, SqlDbType.NVarChar, 20),
        new SqlParameter( _parmUpdatedByUserId, SqlDbType.NVarChar, 100),
        new SqlParameter( _parmUpdatedBy, SqlDbType.NVarChar, 100),
        new SqlParameter( _parmUpdateDate, SqlDbType.DateTime),
        new SqlParameter( _parmBookedOut, SqlDbType.NVarChar, 100),
      };

      return cmdParms;
    }//END GetParameters class

    // =====================================================================================
    /// <summary>
    /// This class sets the query parameter values.     
    /// </summary>
    /// <param name="cmdParms">SqlParameter: Database parameters</param>
    /// <param name="Alert">EvAlert: Project Alert data object</param>
    /// <remarks>
    /// This method consists of the following step:
    /// 
    /// 1. Append the alert object values to the array of sql query parameters. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private void SetParameters ( SqlParameter [ ] cmdParms, EvAlert Alert )
    {
      //
      // Append alert object values to the array object of sql parameters
      //
      cmdParms [ 0 ].Value = Alert.Guid;
      cmdParms [ 1 ].Value = Alert.AlertId;
      cmdParms [ 2 ].Value = Alert.ProjectId;
      cmdParms [ 3 ].Value = Alert.RecordId;
      cmdParms [ 4 ].Value = Alert.ToOrgId;
      cmdParms [ 5 ].Value = Alert.Subject;
      cmdParms [ 6 ].Value = Alert.Message;
      cmdParms [ 7 ].Value = Alert.FromUserUserId;
      cmdParms [ 8 ].Value = Alert.FromUser;
      cmdParms [ 9 ].Value = Alert.ToUser;
      cmdParms [ 10 ].Value = Alert.Raised;
      cmdParms [ 11 ].Value = Alert.AcknowledgedByUserId;
      cmdParms [ 12 ].Value = Alert.AcknowledgedBy;
      cmdParms [ 13 ].Value = Alert.Acknowledged;
      cmdParms [ 14 ].Value = Alert.ClosedByUserId;
      cmdParms [ 15 ].Value = Alert.ClosedBy;
      cmdParms [ 16 ].Value = Alert.Closed;
      cmdParms [ 17 ].Value = Alert.TypeId.ToString ( );
      cmdParms [ 18 ].Value = Alert.State.ToString ( );
      cmdParms [ 19 ].Value = Alert.UpdatedByUserId;
      cmdParms [ 20 ].Value = Alert.UserCommonName;
      cmdParms [ 21 ].Value = DateTime.Now;
      cmdParms [ 22 ].Value = Alert.BookedOutBy;

    }//END SetParameters class.

    #endregion

    #region Data Reader methods

    // =====================================================================================
    /// <summary>
    /// This class reads the content of the data reader object into TrialAlert business object.
    /// </summary>
    /// <param name="Row">DataRow: an sql data row</param>
    /// <returns>EvAlert: a readed data row object</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Extract the compatible data row values to the Alert object. 
    /// 
    /// 2. Return the alert object. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvAlert readDataRow ( DataRow Row )
    {
      // 
      // Initialise the alert object
      // 
      EvAlert alert = new EvAlert ( );

      // 
      // Update the alert object with sql data row fields.
      // 
      alert.Guid = EvSqlMethods.getGuid ( Row, "TAL_Guid" );
      alert.AlertId = EvSqlMethods.getString ( Row, "AlertId" );
      alert.ProjectId = EvSqlMethods.getString ( Row, "TrialId" );
      alert.ToOrgId = EvSqlMethods.getString ( Row, "OrgId" );
      alert.RecordId = EvSqlMethods.getString ( Row, "RecordId" );
      alert.Subject = EvSqlMethods.getString ( Row, "TAL_Subject" );
      alert.Message = EvSqlMethods.getString ( Row, "TAL_Message" );
      alert.FromUserUserId = EvSqlMethods.getString ( Row, "TAL_FromUserUserId" );
      alert.FromUser = EvSqlMethods.getString ( Row, "TAL_FromUser" );
      alert.ToUser = EvSqlMethods.getString ( Row, "TAL_ToUser" );
      alert.Raised = EvSqlMethods.getDateTime ( Row, "TAL_Raised" );
      alert.AcknowledgedByUserId = EvSqlMethods.getString ( Row, "TAL_AcknowledgedByUserId" );
      alert.AcknowledgedBy = EvSqlMethods.getString ( Row, "TAL_AcknowledgedBy" );
      alert.Acknowledged = EvSqlMethods.getDateTime ( Row, "TAL_Acknowledged" );
      alert.ClosedByUserId = EvSqlMethods.getString ( Row, "TAL_ClosedByUserId" );
      alert.ClosedBy = EvSqlMethods.getString ( Row, "TAL_ClosedBy" );
      alert.Closed = EvSqlMethods.getDateTime ( Row, "TAL_Closed" );

      //
      // Backward compatibility.
      //
      string stAlertTypeId = EvSqlMethods.getString ( Row, "TAL_TypeId" );
      if ( stAlertTypeId == "DLT_Notification_Alert" )
      {
        stAlertTypeId = EvAlert.AlertTypes.AE_Notification_Alert.ToString ( );
      }

      if ( stAlertTypeId == "Dose_Limit_Toxicity" )
      {
        stAlertTypeId = EvAlert.AlertTypes.Null.ToString ( );
      }

      alert.TypeId = Evado.Model.EvStatics.Enumerations.parseEnumValue<EvAlert.AlertTypes> ( stAlertTypeId );

      alert.State = Evado.Model.EvStatics.Enumerations.parseEnumValue<EvAlert.AlertStates> ( EvSqlMethods.getString ( Row, "TAL_State" ) );
      alert.UpdatedByUserId = EvSqlMethods.getString ( Row, "TAL_UpdatedByUserId" );
      alert.Updated = EvSqlMethods.getString ( Row, "TAL_UpdatedBy" );
      alert.Updated += " on " + EvSqlMethods.getDateTime ( Row, "TAL_UpdateDate" ).ToString ( "dd MMM yyyy HH:mm" );
      alert.BookedOutBy = EvSqlMethods.getString ( Row, "TAL_BookedOutBy" );

      //
      // Handle backward compatible type identifiers.
      // 
      if ( ( int ) alert.TypeId < 0 )
      {
        alert.TypeId = ( EvAlert.AlertTypes ) Math.Abs ( ( int ) alert.TypeId );
      }

      //
      // Handle backward compatible state identifiers.
      // 
      if ( ( int ) alert.State < 0 )
      {
        alert.State = ( EvAlert.AlertStates ) Math.Abs ( ( int ) alert.State );
      }

      return alert;

    }//END getRowData method.

    #endregion

    #region selectionList query methods

    // ==================================================================================
    /// <summary>
    /// This class gets a ArrayList containing a selectionList of TrialAlert data objects.
    /// </summary>
    /// <param name="ProjectId">String: (Optional) The trial identifier.</param>
    /// <param name="Notification">Boolean: true, if notification exists</param>
    /// <param name="OrgId">String: (Optional) The Facility identifier.</param>
    /// <param name="UserName">String:(Optional) The UserName identifier.</param>
    /// <param name="State">EvAlert.AlertStates: the alert state object</param>    
    /// <param name="AlertType">EvAlert.AlertTypes: an alert type object</param>   
    /// <returns>List of EvAlert: a list of alert objects</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Update Notification to false, if alert type is case report form. 
    /// 
    /// 2. Define the sql query parameters and sql query string 
    /// 
    /// 3. Execute the sql query and store values in datatable. 
    /// 
    /// 4. Iterate through the table value and extract data row values to the alert object. 
    /// 
    /// 5. Return alert object. 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public List<EvAlert> getView (
      String ProjectId,
      Boolean Notification,
      String OrgId,
      String UserName,
      EvAlert.AlertStates State,
      EvAlert.AlertTypes AlertType )
    {
      this.LogMethod ("getView method. " );
      this.LogDebug("ProjectId: " + ProjectId);
      this.LogDebug("OrgId: " + OrgId);
      this.LogDebug("UserName: " + UserName);
      this.LogDebug("Notification: " + Notification);
      this.LogDebug("Alert State: " + State);
      this.LogDebug("Alert AlertType: " + AlertType);
      this.LogDebug("EvAlert.NotificationEventOrganisation: " + EvAlert.NotificationEventOrganisation );
      //
      // Initialize the method status string and a return list of alert objects. 
      //
      List<EvAlert> view = new List<EvAlert>();

      //
      // Update Notification to false, if alert type is case report form. 
      //
      if (AlertType == EvAlert.AlertTypes.Query_Alert)
      {
        Notification = false;
      }


      this.LogDebug ( " Notification setting: " + Notification );

      // 
      // Define the SQL query parameters.
      // 
      SqlParameter[] cmdParms = new SqlParameter[] 
			{
        new SqlParameter(_parmTrialId, SqlDbType.NVarChar, 10),
        new SqlParameter(_parmOrgId, SqlDbType.NVarChar, 10),
        new SqlParameter(_parmUserName, SqlDbType.NVarChar, 100),
				new SqlParameter(_parmState, SqlDbType.VarChar, 20),
			};
      cmdParms[0].Value = ProjectId;
      cmdParms[1].Value = OrgId;
      cmdParms[2].Value = UserName;
      cmdParms[3].Value = State.ToString();

      // 
      // Define the query string.
      // 
      _sqlQueryString = _sqlQuery_View;

      if (ProjectId != String.Empty)
      {
        _sqlQueryString += " WHERE ((TrialId = @TrialId) ";

        if (UserName != String.Empty)
        {
          _sqlQueryString += " AND ( (TAL_FromUser = @UserName) OR  (TAL_ToUser = @UserName) )";
        }

        if (OrgId != String.Empty)
        {
          _sqlQueryString += " AND ( (OrgId = @OrgId) ) ";
        }
        else
        {
          if (Notification == true)
          {
            _sqlQueryString += " AND (OrgId = '" + EvAlert.NotificationEventOrganisation + " ') ";
          }
        }//END if-else OrgId

        if (AlertType != EvAlert.AlertTypes.Null)
        {
          if (AlertType == EvAlert.AlertTypes.Query_Alert)
          {
            _sqlQueryString += " AND (TAL_typeId <> '" + EvAlert.AlertTypes.SAE_Notification_Alert + "') "
             + " AND (TAL_typeId <> '" + EvAlert.AlertTypes.AE_Notification_Alert + "') ";
          }
          else if (AlertType == EvAlert.AlertTypes.Notiifcation_Alert)
          {
            _sqlQueryString += " AND ( TAL_typeId = '" + EvAlert.AlertTypes.SAE_Notification_Alert + "' "
            + " OR TAL_typeId = '" + EvAlert.AlertTypes.AE_Notification_Alert + "' ) ";
          }
          else if (AlertType == EvAlert.AlertTypes.SAE_Notification_Alert)
          {
            _sqlQueryString += " AND ( TAL_typeId = '" + EvAlert.AlertTypes.SAE_Notification_Alert + "' ) ";
          }
          else if (AlertType == EvAlert.AlertTypes.AE_Notification_Alert)
          {
            _sqlQueryString += " AND ( TAL_typeId = '" + EvAlert.AlertTypes.AE_Notification_Alert + "' ) ";
          }
        }//END If-elses AlertType not null
      }//END If VisitId not empty
      else if (OrgId != String.Empty)
      {
        _sqlQueryString += " WHERE ( ( (OrgId = @OrgId) ";

        if (Notification == true)
        {
          _sqlQueryString += " OR  (OrgId = '" + EvAlert.NotificationEventOrganisation + " ') ";
        }

        if (AlertType != EvAlert.AlertTypes.Null)
        {
          if (AlertType == EvAlert.AlertTypes.Query_Alert)
          {
            _sqlQueryString += " AND (TAL_typeId <> '" + EvAlert.AlertTypes.SAE_Notification_Alert + "') "
             + " AND (TAL_typeId <> '" + EvAlert.AlertTypes.AE_Notification_Alert + "') ";
          }
          else if (AlertType == EvAlert.AlertTypes.Notiifcation_Alert)
          {
            _sqlQueryString += " AND ( TAL_typeId = '" + EvAlert.AlertTypes.SAE_Notification_Alert + "' "
            + " OR TAL_typeId = '" + EvAlert.AlertTypes.AE_Notification_Alert + "' ) ";
          }
          else if (AlertType == EvAlert.AlertTypes.SAE_Notification_Alert)
          {
            _sqlQueryString += " AND ( TAL_typeId = '" + EvAlert.AlertTypes.SAE_Notification_Alert + "' ) ";
          }
          else if (AlertType == EvAlert.AlertTypes.AE_Notification_Alert)
          {
            _sqlQueryString += " AND ( TAL_typeId = '" + EvAlert.AlertTypes.AE_Notification_Alert + "' ) ";
          }
        }//END AlertType not null

        _sqlQueryString += " ) ";

      }//END if VisitId and OrgID not empty
      else
      {
        return view;
      }

      // 
      // Set the value of alert state 
      // 
      if (State == EvAlert.AlertStates.Not_Closed)
      {
        cmdParms[3].Value = EvAlert.AlertStates.Closed.ToString();

        _sqlQueryString += " AND not(TAL_State = '" + EvAlert.AlertStates.Closed + "') ";
      }
      else if (State != EvAlert.AlertStates.Null)
      {
        _sqlQueryString += " AND (TAL_State = @State) ";
      }

      // 
      // Set the sorting order for the query.
      // 
        _sqlQueryString += ") ORDER BY AlertId";

      this.LogDebug ( "" + _sqlQueryString );

      // 
      // Execute the query against the database.
      // 
      using (DataTable table = EvSqlMethods.RunQuery(_sqlQueryString, cmdParms))
      {
        // 
        // Iterate through the results extracting the role information.
        // 
        for (int Count = 0; Count < table.Rows.Count; Count++)
        {
          // 
          // Extract the table row and add values to the alert object
          // 
          DataRow row = table.Rows[Count];

          EvAlert alert = this.readDataRow(row);

          //
          // skip null alerts.
          //
          if (alert.TypeId == EvAlert.AlertTypes.Null)
          {
            continue;
          }

          view.Add(alert);

        } //END interation loop.

      }//END using method

      this.LogDebug ( " view count: " + view.Count.ToString() );

      // 
      // Pass back the result arrray.
      // 
      return view;

    } //END getView method.

    // ==================================================================================
    /// <summary>
    /// This class gets a ArrayList containing a selectionList of TrialAlert data objects.
    /// </summary>
    /// <param name="RecordId">string: a record identifier</param>
    /// <param name="AlertTypeId">EvAlert.AlertTypes: an alert type identifier</param>
    /// <param name="State">EvAlert.AlertStates: an alert state</param>
    /// <returns>ArrayList: an array list of the alert records</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Define the sql query parameters and the sql query string
    /// 
    /// 2. Execute the sql query string and store the values in datatable. 
    /// 
    /// 3. Iterate through the table and extract data row value to the Arraylist object 
    /// 
    /// 4. Return the Arraylist object. 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public ArrayList getViewByRecord ( 
      string RecordId, 
      EvAlert.AlertTypes AlertTypeId, 
      EvAlert.AlertStates State )
    {
      //
      // Initialize the method status and a return arraylist of the alert records
      //
      this.LogMethod ("getViewByRecord method. " );
      this.LogDebug( "RecordId: " + RecordId );
      this.LogDebug( "Alert TypeId: " + AlertTypeId );
      this.LogDebug( "Alert State: " + State );
      ArrayList view = new ArrayList ( );

      // 
      // Define the SQL query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(_parmRecordId, SqlDbType.NVarChar, 20),
        new SqlParameter(_parmTypeId, SqlDbType.Char, 1),
        new SqlParameter(_parmState, SqlDbType.NVarChar, 20),
      };
      cmdParms [ 0 ].Value = RecordId;
      cmdParms [ 1 ].Value = AlertTypeId;
      cmdParms [ 2 ].Value = State.ToString ( );

      // 
      // Define the query string.
      // 
      _sqlQueryString = _sqlQuery_View + "WHERE RecordId = @RecordId ";
      // 
      // If the state is passed as a parameter.
      //  
      if ( AlertTypeId != EvAlert.AlertTypes.Null )
      {
        _sqlQueryString += " AND (TAL_TypeId = @TypeId) ";
      }
      if ( State != EvAlert.AlertStates.Null )
      {
        _sqlQueryString += " AND (TAL_State = @State) ";
      }
      _sqlQueryString += " ORDER BY AlertId";

      this.LogDebug ( "" + _sqlQueryString );

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
          // Extract the table row
          // 
          DataRow row = table.Rows [ Count ];

          EvAlert alert = this.readDataRow ( row );

          view.Add ( alert );

        } //END interation loop.

      }//END using method

      this.LogDebug ( " view count: " + view.Count.ToString ( ) );
      // 
      // Pass back the result arrray.
      // 
      return view;

    }//END getViewByRecord method.

    // ==================================================================================
    /// <summary>
    /// This class gets a ArrayList containing a selectionList of TrialAlert data identifiers.
    /// </summary>
    /// <param name="ProjectId">string: (Optional) The trial identifier.</param>
    /// <param name="OrgId">string: (Optional) The Facility identifier.</param>
    /// <param name="UserName">string: (Optional) The UserName identifier.</param>
    /// <param name="State">EvAlert.AlertStates: (Optional) The state.</param>
    /// <param name="ByGuid">Boolean: true, if Guid exists.</param>
    /// <returns>List of EvOption: a list of options</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Define the sql query parameters and the sql query string
    /// 
    /// 2. Execute the query string and store its value into the datatable. 
    /// 
    /// 3. Iterate through the table and extract data row value into the option list. 
    /// 
    /// 4. Return the optons list. 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public List<EvOption> getList (
      string ProjectId, 
      string OrgId, 
      string UserName, 
      EvAlert.AlertStates State, 
      bool ByGuid )
    {
      //
      // Initialize the method status string, a retur option list, an option object
      //
      this.LogMethod ("getList method"  );
      this.LogDebug( "ProjectId: " + ProjectId );
      this.LogDebug( "OrgId: " + OrgId);
      this.LogDebug( "UserName: " + UserName );
      this.LogDebug( "State: " + State );

      List<EvOption> list = new List<EvOption> ( );
      EvOption option = new EvOption ( );
      list.Add ( option );

      // 
      // Define the SQL query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(_parmTrialId, SqlDbType.NVarChar, 10),
        new SqlParameter(_parmOrgId, SqlDbType.NVarChar, 10),
        new SqlParameter(_parmUserName, SqlDbType.NVarChar, 100),
        new SqlParameter(_parmState, SqlDbType.NVarChar, 20),
      };
      cmdParms [ 0 ].Value = ProjectId;
      cmdParms [ 1 ].Value = OrgId;
      cmdParms [ 2 ].Value = UserName;
      cmdParms [ 3 ].Value = State.ToString ( );

      // 
      // Define the query string.
      // 
      _sqlQueryString = _sqlQuery_View;

      if ( ProjectId != String.Empty )
      {
        _sqlQueryString += " WHERE (TrialId = @TrialId) ";

        if ( UserName != String.Empty )
        {
          _sqlQueryString += " AND ( (TAL_FromUser = @UserName) OR  (TAL_ToUser = @UserName) )";
        }
        if ( OrgId != String.Empty )
        {
          _sqlQueryString += " AND (OrgId = @OrgId) ";
        }
        else if ( State != EvAlert.AlertStates.Null )
        {
          _sqlQueryString += " AND (TAL_State = @State) ";
        }
      }//END if trialId is not empty
      else if ( OrgId != String.Empty )
      {
        _sqlQueryString += " WHERE  (OrgId = @OrgId) ";

        if ( State != EvAlert.AlertStates.Null )
        {
          _sqlQueryString += " AND (TAL_State = @State) ";
        }
      }//END if OrgId is not empty
      else
      {
        return list;
      }//END else

      _sqlQueryString += " ORDER BY AlertId";

      this.LogDebug ( _sqlQueryString );

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
          // Extract the table row
          // 
          DataRow row = table.Rows [ Count ];

          // 
          // Process the results into the visitSchedule.
          // 
          if ( ByGuid == true )
          {
            option = new EvOption (
              EvSqlMethods.getString ( row, "TAL_guid" ),
              EvSqlMethods.getString ( row, "AlertId" ) + " " + EvSqlMethods.getString ( row, "TAL_Subject" ) );
          }
          else
          {
            option = new EvOption (
              EvSqlMethods.getString ( row, "AlertId" ),
              EvSqlMethods.getString ( row, "AlertId" ) + " " + EvSqlMethods.getString ( row, "TAL_Subject" ) );
          }
          // 
          // Append the new TrialAlert object to the array.
          // 
          list.Add ( option );
        }
      }
      // 
      // Pass back the result arrray.
      // 
      return list;

    }//END getList method.

    #endregion

    #region Data object retrieval methods

    // ==================================================================================
    /// <summary>
    /// This class gets TrialAlert data object by its unique object identifier.
    /// </summary>
    /// <param name="AlertGuid">Guid: (Mandatory) The global unique object identifier.</param>
    /// <returns>EvAlert: TrialAlert data object</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Return an empty alert object, if the Alert Guid is empty
    /// 
    /// 2. Define the sql query parameter and the sql query string. 
    /// 
    /// 3. Execute the sql query string and store the value in datatable. 
    /// 
    /// 4. Extract the first row of table to the alert object. 
    /// 
    /// 5. Return the alert object. 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public EvAlert getItem ( Guid AlertGuid )
    {
      //
      // Initialize the method status string, an alert object. 
      //
      this.LogMethod ("getItem method methiopd. " );
      this.LogDebug ( "AlertGuid: " + AlertGuid ); 
      EvAlert alert = new EvAlert();

      // 
      // Check that there is a valid unique identifier.
      // 
      if (AlertGuid == Guid.Empty)
      {
        return alert;
      }

      // 
      // Define the query parameters.
      // 
      SqlParameter cmdParms = new SqlParameter(_parmGuid, SqlDbType.UniqueIdentifier);
      cmdParms.Value = AlertGuid;
      
      // 
      // Define the query string.
      // 
      _sqlQueryString = _sqlQuery_View + " WHERE (TAL_Guid = @Guid ); ";

      // 
      // Execute the query against the database.
      // 
      using (DataTable table = EvSqlMethods.RunQuery(_sqlQueryString, cmdParms))
      {
        // 
        // If not rows the return
        // 
        if (table.Rows.Count == 0)
        {
          return alert;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows[0];

        alert = this.readDataRow(row);

      }//END Using 

      // 
      // Pass back the data object.
      // 
      return alert;

    }//END getItem method. 

    // ==================================================================================
    /// <summary>
    /// This class gets TrialAlert data object by its object identifier.
    /// </summary>
    /// <param name="AlertId">string: an alert identifier</param>
    /// <returns>EvAlert: a Project Alert data object</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Return an empty alert object, if the Alert Guid is empty
    /// 
    /// 2. Define the sql query parameter and the sql query string. 
    /// 
    /// 3. Execute the sql query string and store the value in datatable. 
    /// 
    /// 4. Extract the first row of table to the return alert object. 
    /// 
    /// 5. Return the alert object. 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public EvAlert getItem ( string AlertId )
    {
      //
      // Initialize the method status string and the alert object. 
      //
      this.LogMethod ( "getItem method" );
      this.LogDebug ( "AlertId: " + AlertId );
      EvAlert alert = new EvAlert();

      // 
      // Check that there is a valid unique identifier.
      // 
      if (AlertId == String.Empty)
      {
        return alert;
      }

      // 
      // Define the query parameters.
      // 
      SqlParameter cmdParms = new SqlParameter(_parmAlertId, SqlDbType.NVarChar, 20);
      cmdParms.Value = AlertId;

      // 
      // Define the query string.
      // 
      _sqlQueryString = _sqlQuery_View + " WHERE (AlertId = @AlertId ); ";
      this.LogDebug (  _sqlQueryString );

      // 
      // Execute the query against the database.
      // 
      using (DataTable table = EvSqlMethods.RunQuery(_sqlQueryString, cmdParms))
      {
        // 
        // If not rows the return
        // 
        if (table.Rows.Count == 0)
        {
          return alert;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows[0];

        alert = this.readDataRow(row);

      }//END Using 

      return alert;

    }//END getItem class 

    #endregion

    #region Data object update methods

    // ==================================================================================
    /// <summary>
    /// This class adds an TrialAlert data object to the database.
    /// </summary>
    /// <param name="Alert">EvAlert: a Project Alert data object</param>
    /// <returns>EvEventCodes: an event code for adding item</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Exit, if the trialId or ToOrgId is empty
    /// 
    /// 2. Create new alert Guid
    /// 
    /// 3. Define the sql query parameters and execute the storeprocedure for adding items. 
    /// 
    /// 4. Return an event code for adding items
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public EvEventCodes addItem ( EvAlert Alert )
    {
      //
      // Initialize the method status string 
      //
      this.LogMethod ("addItem method. " );
      this.LogDebug ( "ProjectId: " + Alert.ProjectId );
      this.LogDebug ( "RecordId: " + Alert.RecordId );
      this.LogDebug ( "OrgId: " + Alert.ToOrgId );
      this.LogDebug ( "Type: " + Alert.TypeId ); ;

      // 
      // Check that the data object has valid identifiers to add it to the database.
      // 
      if ( Alert.ProjectId == String.Empty )
      {
        return EvEventCodes.Identifier_Project_Id_Error;
      }

      if ( Alert.ToOrgId == String.Empty )
      {
        return EvEventCodes.Identifier_Org_Id_Error;
      }

      // 
      // Create the new alert FormUid.
      // 
      Alert.Guid = Guid.NewGuid ( );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] _cmdParms = GetParameters ( );
      SetParameters ( _cmdParms, Alert );

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( _STORED_PROCEDURE_AddItem, _cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      // 
      // Return Ok.
      // 
      this.LogMethodEnd ( "updateItem" );

      return EvEventCodes.Ok;

    } //END addItem method. 

    // =====================================================================================
    /// <summary>
    /// This class updates TrialAlert data object in the database using it unique object identifier.
    /// </summary>
    /// <param name="Alert">EvAlert: a TrialAlert data object</param>
    /// <returns>EvEventCodes: an event code for updating item</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Exit, if the Guid or TriadId or ToOrgId is empty
    /// 
    /// 2. Add items to datachange object if they do not exist after comparing the alert objects. 
    /// 
    /// 3. Define the SQL query parameters and execute the storeprocedure for updating items.
    /// 
    /// 4. Append the datachange object values to the backup datachanges object.
    /// 
    /// 5. Return thd event code for updating items. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvEventCodes updateItem ( EvAlert Alert )
    {
      //
      // Initialize the method status string. 
      //
      this.LogMethod ("updateItem method " );
      this.LogDebug( "ProjectId: " + Alert.ProjectId);
      this.LogDebug( "RecordId: " + Alert.RecordId);
      this.LogDebug( "OrgId: " + Alert.ToOrgId);
      this.LogDebug( "Type: " + Alert.TypeId );

      // 
      // Check that the data object has valid identifiers to add it to the database.
      //  
      if ( Alert.Guid == Guid.Empty )
      {
        return EvEventCodes.Identifier_Global_Unique_Identifier_Error;
      }

      if ( Alert.ProjectId == String.Empty )
      {
        return EvEventCodes.Identifier_Project_Id_Error;
      }

      if ( Alert.ToOrgId == String.Empty )
      {
        return EvEventCodes.Identifier_Org_Id_Error;
      }

      // 
      // Get the old object for comparision
      // 
      EvAlert oldAlert = getItem ( Alert.Guid );

      // 
      // Verify that the object exists.
      // 
      if ( oldAlert.Guid == Guid.Empty )
      {
        this.LogDebug ( " Guid: " + oldAlert.Guid + " AlertId: " + oldAlert.AlertId );

        return EvEventCodes.Data_InvalidId_Error;
      }

      // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      // Compare the objects.
      // 
      EvDataChanges dataChanges = new EvDataChanges ( );
      EvDataChange dataChange = new EvDataChange ( );

      // 
      // Initialise the data change object.
      // 
      dataChange.Guid = Guid.NewGuid ( );
      dataChange.TableName = EvDataChange.DataChangeTableNames.EvAlerts;
      dataChange.RecordGuid = Alert.Guid;
      dataChange.TrialId = Alert.ProjectId;
      dataChange.UserId = Alert.UpdatedByUserId;
      dataChange.DateStamp = DateTime.Now;

      // 
      // Append the property changes.
      // 
      if ( Alert.Subject != oldAlert.Subject )
      {
        dataChange.AddItem ( "Subject", oldAlert.Subject, Alert.Subject );
      }
      if ( Alert.Message != oldAlert.Message )
      {
        dataChange.AddItem ( "Message", oldAlert.Message, Alert.Message );
      }
      if ( Alert.FromUserUserId != oldAlert.FromUserUserId )
      {
        dataChange.AddItem ( "FromUserUserId", oldAlert.FromUser, Alert.FromUserUserId );
      }
      if ( Alert.FromUser != oldAlert.FromUser )
      {
        dataChange.AddItem ( "FromUser", oldAlert.FromUser, Alert.FromUser );
      }
      if ( Alert.Raised != oldAlert.Raised )
      {
        dataChange.AddItem ( "Raised", oldAlert.Raised.ToString ( "dd MMM yy HH:mm:ss" ), Alert.Raised.ToString ( "dd MMM yy HH:mm:ss" ) );
      }
      if ( Alert.ToUser != oldAlert.ToUser )
      {
        dataChange.AddItem ( "ToUser", oldAlert.ToUser, Alert.ToUser );
      }
      if ( Alert.AcknowledgedByUserId != oldAlert.AcknowledgedByUserId )
      {
        dataChange.AddItem ( "AcknowledgedByUserId", oldAlert.AcknowledgedByUserId, Alert.AcknowledgedByUserId );
      }
      if ( Alert.AcknowledgedBy != oldAlert.AcknowledgedBy )
      {
        dataChange.AddItem ( "AcknowledgedBy", oldAlert.AcknowledgedBy, Alert.AcknowledgedBy );
      }
      if ( Alert.Acknowledged != oldAlert.Acknowledged )
      {
        dataChange.AddItem ( "Acknowledged",
          oldAlert.Acknowledged.ToString ( "dd MMM yy HH:mm:ss" ),
          Alert.Acknowledged.ToString ( "dd MMM yy HH:mm:ss" ) );
      }
      if ( Alert.ClosedByUserId != oldAlert.ClosedByUserId )
      {
        dataChange.AddItem ( "ClosedByUserId", oldAlert.ClosedByUserId, Alert.ClosedByUserId );
      }
      if ( Alert.ClosedBy != oldAlert.ClosedBy )
      {
        dataChange.AddItem ( "ClosedBy", oldAlert.ClosedBy, Alert.ClosedBy );
      }
      if ( Alert.Closed != oldAlert.Closed )
      {
        dataChange.AddItem ( "Closed",
          oldAlert.Closed.ToString ( "dd MMM yy HH:mm:ss" ),
          Alert.Closed.ToString ( "dd MMM yy HH:mm:ss" ) );
      }
      if ( Alert.TypeId != oldAlert.TypeId )
      {
        dataChange.AddItem ( "TypeId", oldAlert.TypeId.ToString ( ), Alert.TypeId.ToString ( ) );
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] _cmdParms = GetParameters ( );
      SetParameters ( _cmdParms, Alert );

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( _STORED_PROCEDURE_UpdateItem, _cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      // 
      // Append the datachange to the database.
      // 
      dataChanges.AddItem ( dataChange );

      // 
      // Return Ok.
      // 
      this.LogMethodEnd ( "updateItem" );

      return EvEventCodes.Ok;

    }//END updateItem class. 

    #endregion

  }//END EvAlerts class

}//END namespace Evado.Dal.Digital
