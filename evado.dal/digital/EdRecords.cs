/* <copyright file="DAL\EvRecords.cs" company="EVADO HOLDING PTY. LTD.">
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
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;
using System.Text;

using Evado.Model;
using Evado.Model.Digital;


namespace Evado.Dal.Digital
{

  /// <summary>
  /// A business Component used to manage Ethics roles
  /// The Evado.Evado.EvRecord is used in most methods 
  /// and is used to store serializable information about an account
  /// </summary>
  public class EdRecords : EvDalBase
  {
    #region class initialisation method.
    /// <summary>
    /// This method initialises the schedule DAL class.
    /// </summary>
    public EdRecords ( )
    {
      this.ClassNameSpace = "Evado.Dal.Digital.EvFormRecords.";
    }

    /// <summary>
    /// This method initialises the schedule DAL class.
    /// </summary>
    public EdRecords ( EvClassParameters ClassParameters )
    {
      this.ClassParameters = ClassParameters;
      this.ClassNameSpace = "Evado.Dal.Digital.EvFormRecords.";

    }

    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class constants and variables.

    //  ==================================================================================
    // 
    // Define the selectionList query string.
    // 
    // The max visitSchedule length setting. 
    private static string _maximumListLength = ConfigurationManager.AppSettings [ "MaximumSelectionListLength" ];
    private int _MaxViewLength = 1000;

    private const string SQL_QUERY_RECORD_VIEW = "Select * FROM ED_RECORD_VIEW ";

    // 
    // Define the stored procedure names.
    // 
    private const string STORED_PROCEDURE_RECORD_CREATE = "USR_RECORD_CREATE";
    private const string STORED_PROCEDURE_RECORD_DELETE = "USR_RECORD_DELETE";
    private const string STORED_PROCEDURE_RECORD_LOCK = "USR_RECORD_LOCK";
    private const string STORED_PROCEDURE_RECORD_UNLOCK = "USR_RECORD_UNLOCK";
    private const string STORED_PROCEDURE_RECORD_UPDATE = "USR_RECORD_UPDATE";

    //
    // This constant defines the table fields/columns
    //
    public const string DB_RECORD_GUID = "EDR_GUID";
    public const string DB_STATE = "EDR_STATE";
    public const string DB_RECORD_ID = "RECORD_ID";
    public const string DB_SOURCE_ID = "EDR_SOURCE_ID";
    public const string DB_RECORD_DATE = "EDR_RECORD_DATE";
    public const string DB_VISABILITY = "EDR_VISABILITY";
    public const string DB_AI_DATA_INDEX = "EDR_AI_DATA_INDEX";
    public const string DB_SIGN_OFFS = "EDR_SIGN_OFFS";
    public const string DB_BOOKED_OUT_USER_ID = "EDR_BOOKED_OUT_USER_ID";
    public const string DB_BOOKED_OUT = "EDR_BOOKED_OUT";
    public const string DB_UPDATED_BY_USER_ID = "EDR_UPDATED_BY_USER_ID";
    public const string DB_UPDATED_BY = "EDR_UPDATED_BY";
    public const string DB_UPDATED_DATE = "EDR_UPDATED_DATE";
    public const string DB_SERIAL_ID = "EDR_SERIAL_ID";
    public const string DB_DELETED = "EDR_DELETED";
    // 
    // Define the query parameter constants.
    // 

    //
    // The field and parameter values for the SQl customer filter 
    //
    private const string DB_CUSTOMER_GUID = "CU_GUID";
    private const string PARM_CUSTOMER_GUID = "@CUSTOMER_GUID";

    private const string PARM_RECORD_GUID = "@GUID";
    private const string PARM_LAYOUT_GUID = "@LAYOUT_GUID";
    private const string PARM_STATE = "@STATE";
    private const string PARM_RECORD_ID = "@RECORD_ID";
    private const string PARM_SOURCE_ID = "@SOURCE_ID";
    private const string PARM_RECORD_DATE = "@RECORD_DATE";
    private const string PARM_VISABILITY = "@VISABILITY";
    private const string PARM_AI_DATA_INDEX = "@AI_DATA_INDEX";
    private const string PARM_UPDATED_BY_USER_ID = "@UPDATED_BY_USER_ID";
    private const string PARM_UPDATED_BY = "@UPDATED_BY";
    private const string PARM_UPDATED_DATE = "@UPDATED_DATE";
    private const string PARM_BOOKED_OUT = "@BOOKED_OUT";
    private const string PARM_SIGN_OFFS = "@SIGN_OFFS";

    // 
    // Used for the ItemQuery
    // 
    private const string PARM_FIELD_ID = "@FIELD_ID";
    private const string PARM_TEXT_VALUE = "@TEXT_VALUE";
    private const string PARM_TYPE_ID = "@TYPE_ID";

    //
    // This variable is used to skip retrieving comments when updating the form record.
    //
    bool _SkipRetrievingComments = false;

    // 
    // Instantiate the variables and properties.
    // 
    private string _sqlQueryString = String.Empty;


    EdRecordSections _Dal_FormSections = new EdRecordSections ( );
    #endregion

    #region Set Query Parameters

    //  ==================================================================================
    /// <summary>
    /// This class sets the array of parameters. 
    /// </summary>
    /// <returns>SqlParameter: an array of SqlParameters</returns>
    /// <remarks>
    /// This method consists of the following step: 
    /// 
    /// 1. Create the array of sql parameters.
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private static SqlParameter [ ] GetParameters ( )
    {
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( EdRecords.PARM_RECORD_GUID, SqlDbType.UniqueIdentifier),
        new SqlParameter( EdRecords.PARM_STATE, SqlDbType.VarChar, 20),
        new SqlParameter( EdRecords.PARM_SOURCE_ID, SqlDbType.NVarChar, 40), 
        new SqlParameter( EdRecords.PARM_RECORD_DATE, SqlDbType.DateTime),
        new SqlParameter( EdRecords.PARM_VISABILITY, SqlDbType.NVarChar, 30), 
        new SqlParameter( EdRecords.PARM_AI_DATA_INDEX, SqlDbType.NVarChar, 1000), 
        new SqlParameter( EdRecords.PARM_SIGN_OFFS, SqlDbType.NVarChar),
        new SqlParameter( EdRecords.PARM_UPDATED_BY_USER_ID, SqlDbType.NVarChar, 100),
        new SqlParameter( EdRecords.PARM_UPDATED_BY, SqlDbType.NVarChar, 100),
        new SqlParameter( EdRecords.PARM_UPDATED_DATE, SqlDbType.DateTime),
      };
      return cmdParms;
    }

    // =====================================================================================
    /// <summary>
    /// This class binds values to query parameters.
    /// </summary>
    /// <param name="CommandParameters">SqlParameter: an array of sql parameters</param>
    /// <param name="Record">EvForm: a form object containing the parmeter values.</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. If the monitor name exists then set the signoff to true.
    /// 
    /// 2. Set record date and Guid if they do not exist
    /// 
    /// 3. Update the items from form object to the array of sql query parameters. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private void SetParameters (
      SqlParameter [ ] CommandParameters,
      EdRecord Record )
    {
      // 
      // Set the record date if is not already set.
      // 
      if ( Record.RecordDate == Evado.Model.EvStatics.CONST_DATE_NULL )
      {
        Record.RecordDate = DateTime.Now;
      }

      // 
      // Set the Global identifier if not set.
      // 
      if ( Record.Guid == Guid.Empty )
      {
        Record.Guid = Guid.NewGuid ( );
      }

      // 
      // Load the command parmameter values
      // 
      CommandParameters [ 0 ].Value = Record.Guid;
      CommandParameters [ 1 ].Value = Record.State;
      CommandParameters [ 2 ].Value = Record.SourceId;
      CommandParameters [ 3 ].Value = Record.RecordDate;
      CommandParameters [ 4 ].Value = Evado.Model.EvStatics.SerialiseObject<List<EdUserSignoff>> ( Record.Signoffs );
      CommandParameters [ 5 ].Value = this.ClassParameters.UserProfile.UserId;
      CommandParameters [ 6 ].Value = this.ClassParameters.UserProfile.CommonName;
      CommandParameters [ 7 ].Value = DateTime.Now;


    }//END SetParameters class.

    //  ==================================================================================
    /// <summary>
    /// This class sets the array of createPrameters. 
    /// </summary>
    /// <returns>SqlParameter: an array of SqlParameters</returns>
    /// <remarks>
    /// This class consists of the following steps: 
    /// 
    /// 1. Create an array of sql query parameters. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private static SqlParameter [ ] GetCreateParameters ( )
    {
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( EdRecords.PARM_RECORD_GUID, SqlDbType.UniqueIdentifier),
        new SqlParameter( EdRecordLayouts.PARM_LAYOUT_ID, SqlDbType.NVarChar, 10),
        new SqlParameter( EdRecords.PARM_UPDATED_BY_USER_ID, SqlDbType.NVarChar, 100),
        new SqlParameter( EdRecords.PARM_UPDATED_BY, SqlDbType.NVarChar, 100),
        new SqlParameter( EdRecords.PARM_UPDATED_DATE, SqlDbType.DateTime),
      };
      return cmdParms;

    }//END GetCreateParameters method

    // =====================================================================================
    /// <summary>
    /// This class binds the values to the array of createParameters
    /// </summary>
    /// <param name="CommandParameters">SqlParameter: an array of sql parameters</param>
    /// <param name="Record">EvForm: a form data object containing the parmeter values.</param>
    /// <remarks>
    /// This method consists of the following step: 
    /// 
    /// 1. Load the form object values to the array of sql parameters. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private void SetCreateParameters (
      SqlParameter [ ] CommandParameters,
      EdRecord Record )
    {
      // 
      // Load the command parmameter values
      // 
      CommandParameters [ 0 ].Value = Record.Guid;
      CommandParameters [ 1 ].Value = Record.LayoutId;
      CommandParameters [ 2 ].Value = this.ClassParameters.UserProfile.UserId;
      CommandParameters [ 3 ].Value = this.ClassParameters.UserProfile.CommonName;
      CommandParameters [ 4 ].Value = DateTime.Now;

    }//END SetCreateParameters class.

    //  ==================================================================================
    /// <summary>
    /// This class sets the array of createPrameters. 
    /// </summary>
    /// <returns>SqlParameter: an array of SqlParameters</returns>
    /// <remarks>
    /// This class consists of the following steps: 
    /// 
    /// 1. Create an array of sql query parameters. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private static SqlParameter [ ] GetDeleteParameters ( )
    {
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( EdRecords.PARM_RECORD_GUID, SqlDbType.UniqueIdentifier),
        new SqlParameter( EdRecords.PARM_UPDATED_BY_USER_ID, SqlDbType.NVarChar, 100),
        new SqlParameter( EdRecords.PARM_UPDATED_BY, SqlDbType.NVarChar, 100),
        new SqlParameter( EdRecords.PARM_UPDATED_DATE, SqlDbType.DateTime),
      };
      return cmdParms;

    }//END GetCreateParameters method

    // =====================================================================================
    /// <summary>
    /// This class binds the values to the array of createParameters
    /// </summary>
    /// <param name="CommandParameters">SqlParameter: an array of sql parameters</param>
    /// <param name="Record">EvForm: a form data object containing the parmeter values.</param>
    /// <remarks>
    /// This method consists of the following step: 
    /// 
    /// 1. Load the form object values to the array of sql parameters. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private void SetDeleteParameters (
      SqlParameter [ ] CommandParameters,
      EdRecord Record )
    {
      // 
      // Load the command parmameter values
      // 
      CommandParameters [ 0 ].Value = Record.Guid;
      CommandParameters [ 1 ].Value = this.ClassParameters.UserProfile.UserId;
      CommandParameters [ 2 ].Value = this.ClassParameters.UserProfile.CommonName;
      CommandParameters [ 3 ].Value = DateTime.Now;

    }//END SetCreateParameters class.
    #endregion

    #region Form Record Reader

    // =====================================================================================
    /// <summary>
    /// This class extracts sqlReader to the form object.
    /// </summary>
    /// <param name="Row">DataRow: a sql data row object containing the query results</param>
    /// <param name="SummaryQuery">Boolean: true, if the queryState query is selected</param>
    /// <returns>EvForm: a form object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Update the trial record object with compatible values from datarow
    /// 
    /// 2. Add the content value if it is not a queryState query. 
    /// 
    /// 3. Reformat Comments into new format if necessary
    /// 
    /// 4. Update the state and type to the form object. 
    /// 
    /// 5. Return the Form data object. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private EdRecord getRowData (
      DataRow Row,
      bool SummaryQuery )
    {
      //this.LogMethod ( "getRowData method" );
      // 
      // Initialise the return form object and the form selected condition.
      // 
      EdRecord record = new EdRecord ( );

      // 
      // Update the trial record object with compatible values from datarow.
      // 
      record.Guid = EvSqlMethods.getGuid ( Row, EdRecords.DB_RECORD_GUID );
      record.LayoutGuid = EvSqlMethods.getGuid ( Row, EdRecordLayouts.DB_LAYOUT_GUID );
      record.SourceId = EvSqlMethods.getString ( Row, EdRecords.DB_SOURCE_ID );
      record.LayoutId = EvSqlMethods.getString ( Row, EdRecordLayouts.DB_LAYOUT_ID );
      record.RecordId = EvSqlMethods.getString ( Row, EdRecords.DB_RECORD_ID );
      record.Design.Title = EvSqlMethods.getString ( Row, EdRecordLayouts.DB_TITLE );
      record.State = Evado.Model.EvStatics.parseEnumValue<EdRecordObjectStates> (
        EvSqlMethods.getString ( Row, EdRecords.DB_STATE ) );
      record.RecordDate = EvSqlMethods.getDateTime ( Row, EdRecords.DB_RECORD_DATE );

      //
      // Skip detailed content if a queryState query
      //
      if ( SummaryQuery == false )
      {
        record.Design.HttpReference = EvSqlMethods.getString ( Row, EdRecordLayouts.DB_HTTP_REFERENCE );
        record.Design.Instructions = EvSqlMethods.getString ( Row, EdRecordLayouts.DB_INSTRUCTIONS );
        record.Design.Description = EvSqlMethods.getString ( Row, EdRecordLayouts.DB_DESCRIPTION );
        record.Design.UpdateReason = Evado.Model.EvStatics.parseEnumValue<EdRecord.UpdateReasonList> (
          EvSqlMethods.getString ( Row, EdRecordLayouts.DB_UPDATE_REASON ) );
        record.Design.RecordCategory = EvSqlMethods.getString ( Row, EdRecordLayouts.DB_RECORD_CATEGORY );

        record.Design.TypeId = Evado.Model.EvStatics.parseEnumValue<EdRecordTypes> (
           EvSqlMethods.getString ( Row, EdRecordLayouts.DB_TYPE_ID ) );
        record.Design.Version = EvSqlMethods.getFloat ( Row, EdRecordLayouts.DB_VERSION );

        record.Design.JavaScript = EvSqlMethods.getString ( Row, EdRecordLayouts.DB_JAVA_SCRIPT );
        record.Design.hasCsScript = EvSqlMethods.getBool ( Row, EdRecordLayouts.DB_HAS_CS_SCRIPT );
        record.Design.Language = EvSqlMethods.getString ( Row, EdRecordLayouts.DB_LANGUAGE );

        record.Design.ReadAccessRoles = EvSqlMethods.getString ( Row, EdRecordLayouts.DB_READ_ACCESS_ROLES );
        record.Design.EditAccessRoles = EvSqlMethods.getString ( Row, EdRecordLayouts.DB_EDIT_ACCESS_ROLES );

        record.Design.ParentEntities = EvSqlMethods.getString ( Row, EdRecordLayouts.DB_PARENT_ENTITIES );
        record.Design.DefaultPageLayout = EvSqlMethods.getString ( Row, EdRecordLayouts.DB_DEFAULT_PAGE_LAYOUT );

        string value = EvSqlMethods.getString ( Row, EdRecordLayouts.DB_LINK_CONTENT_SETTING );
        if ( value != String.Empty )
        {
          record.Design.LinkContentSetting =
            Evado.Model.EvStatics.parseEnumValue<EdRecord.LinkContentSetting> ( value );
        }
        record.Design.DisplayRelatedEntities = EvSqlMethods.getBool ( Row, EdRecordLayouts.DB_DISPLAY_ENTITIES );
        record.Design.DisplayAuthorDetails = EvSqlMethods.getBool ( Row, EdRecordLayouts.DB_DISPLAY_AUTHOR_DETAILS );
        record.Design.RecordPrefix = EvSqlMethods.getString ( Row, EdRecordLayouts.DB_RECORD_PREFIX );
        record.Design.ParentType = EvSqlMethods.getString<EdRecord.ParentTypeList> ( Row, EdRecordLayouts.DB_PARENT_TYPE );
        record.Design.ParentEntities = EvSqlMethods.getString ( Row, EdRecordLayouts.DB_PARENT_ENTITIES );

        record.Updated = EvSqlMethods.getString ( Row, EdRecords.DB_UPDATED_BY );
        if ( record.Updated != string.Empty )
        {
          record.Updated += " on " + EvSqlMethods.getDateTime ( Row, EdRecords.DB_UPDATED_DATE ).ToString ( "dd MMM yyyy HH:mm" );
        }
        record.BookedOutBy = EvSqlMethods.getString ( Row, EdRecords.DB_BOOKED_OUT );

        //
        // fill the comment list.
        //
        this.fillCommentList ( record );

        string approved = "Version: " + record.Design.Version.ToString ( "0.00" );
        approved += " by: " + EvSqlMethods.getString ( Row, EdRecordLayouts.DB_UPDATED_BY );
        approved += " on " + EvSqlMethods.getDateTime ( Row, EdRecordLayouts.DB_UPDATED_DATE ).ToString ( "dd MMM yyyy" );
        record.Design.Approval = approved;
      }

      return record;

    }//END getRowData class

    // =====================================================================================
    /// <summary>
    /// This method fills the comment list to the form object. 
    /// </summary>
    /// <param name="FormRecord">EvFormField: a form object</param>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Fill the record comment list.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private void fillCommentList ( EdRecord FormRecord )
    {
      //this.LogMethod ( "fillCommentList method." );
      //
      // Initialize the method values and objects.
      //
      EdRecordComments formRecordComments = new EdRecordComments ( );

      //
      // Fill the record comment list.
      //
      FormRecord.CommentList = formRecordComments.getCommentList (
        FormRecord.Guid, Guid.Empty,
        EdFormRecordComment.CommentTypeCodes.Form,
        EdFormRecordComment.AuthorTypeCodes.Not_Set );

      //this.LogClass ( formRecordComments.Log );

      //this.LogMethodEnd ( "fillCommentList" );

    }//END fillCommentList method

    #endregion

    #region FormRecord views and query methods.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of form object retrieved by query parameters. 
    /// </summary>
    /// <param name="QueryParameters">EvQueryParameters: (Mandatory) QueryParameter object.</param>
    /// <returns>List of EvForm: a list of form object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define the parameters for the startDate and finishDate 
    /// 
    /// 2. Define the sql query parameters and the sql query string. 
    /// 
    /// 3. Execute the sql query string and store the results on the data table. 
    /// 
    /// 4. Loop through the data table and extract the data row to the form object. 
    /// 
    /// 5. Add formfield items and linkText to the form object if they exist. 
    /// 
    /// 6. Add the form object to the Forms list. 
    /// 
    /// 7. Return the Forms list. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public int getRecordCount (
      EdQueryParameters QueryParameters )
    {
      //
      // Initialize the method debug log, a return form list and a number of result count. 
      //
      this.LogMethod ( "getRecordCount method." );

      List<EdRecord> view = new List<EdRecord> ( );
      int inResultCount = 0;

      // 
      // Define the query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( EdRecordLayouts.PARM_LAYOUT_ID, SqlDbType.NVarChar, 10),
      };
      cmdParms [ 0 ].Value = QueryParameters.LayoutId;

      //
      // Generate the SQL query string.
      //
      _sqlQueryString = this.createSqlQueryStatement ( QueryParameters );
      this.LogValue ( _sqlQueryString );

      this.LogValue ( " Execute Query" );
      //
      //Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( _sqlQueryString, cmdParms ) )
      {
        inResultCount = table.Rows.Count;

      }//END using method

      // 
      // Get the array length
      // 
      this.LogDebug ( " Returned records: " + inResultCount );

      // 
      // Return the result array.
      // 
      return inResultCount;

    } // Close getRecordList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of form object retrieved by query parameters. 
    /// </summary>
    /// <param name="QueryParameters">EvQueryParameters: (Mandatory) QueryParameter object.</param>
    /// <returns>List of EvForm: a list of form object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define the parameters for the startDate and finishDate 
    /// 
    /// 2. Define the sql query parameters and the sql query string. 
    /// 
    /// 3. Execute the sql query string and store the results on the data table. 
    /// 
    /// 4. Loop through the data table and extract the data row to the form object. 
    /// 
    /// 5. Add formfield items and linkText to the form object if they exist. 
    /// 
    /// 6. Add the form object to the Forms list. 
    /// 
    /// 7. Return the Forms list. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public List<EdRecord> getRecordList (
      EdQueryParameters QueryParameters )
    {
      this.LogMethod ( "getRecordList method." );
      //
      // Initialize the method debug log, a return form list and a number of result count. 
      //
      List<EdRecord> view = new List<EdRecord> ( );
      int inResultCount = 0;

      // 
      // Define the query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      { new SqlParameter( EdRecordLayouts.PARM_LAYOUT_ID, SqlDbType.NVarChar, 10),
      };
      cmdParms [ 0 ].Value = QueryParameters.LayoutId;

      //
      // Generate the SQL query string.
      //
      _sqlQueryString = this.createSqlQueryStatement ( QueryParameters );
      this.LogDebug ( _sqlQueryString );

      this.LogDebug ( " Execute Query" );
      //
      //Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( _sqlQueryString, cmdParms ) )
      {
        // 
        // Iterate through the results extracting the role information.
        // 
        for ( int count = 0; count < table.Rows.Count; count++ )
        {
          // 
          // Extract the table row
          // 
          DataRow row = table.Rows [ count ];

          EdRecord record = this.getRowData ( row, QueryParameters.IncludeSummary );

          // 
          // Attach fields and other trial data.
          // 
          if ( QueryParameters.IncludeRecordValues == true )
          {
            if ( inResultCount < QueryParameters.RecordRangeStart
              || inResultCount >= ( QueryParameters.RecordRangeFinish ) )
            {
              this.LogDebug ( "Count: " + inResultCount + " >> not within record range." );

              //
              // Increment the result count.
              //
              inResultCount++;

              continue;
            }

            // 
            // Get the trial record fields
            // 
            this.getRecordData ( record );

            //
            // Attach the entity list.
            //
            this.getEntities ( record );

            //
            // attach the record sections.
            //
            this.GetRecordSections ( record );

            //
            // Increment the result count.
            //
            inResultCount++;

            // 
          }//END query selection state not set.

          view.Add ( record );

        }//END record iteration loop

      }//END using method

      // 
      // Get the array length
      // 
      this.LogValue ( " Returned records: " + view.Count );

      // 
      // Return the result array.
      // 
      this.LogMethodEnd ( "getRecordList" );
      return view;

    } // Close getRecordList method.

    // =====================================================================================
    /// <summary>
    /// This class defines an SQL query string based on the query parameters. 
    /// </summary>
    /// <param name="QueryParameters">EvQueryParameters: (Mandatory) EvQueryParameters.</param>
    /// <returns>String: a SQL query string</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Generate the sql query string. 
    /// 
    /// 2. Return the sql query string. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private String createSqlQueryStatement (
      EdQueryParameters QueryParameters )
    {
      //
      // Initialize the local sql query string. 
      //
      StringBuilder sqlQueryString = new StringBuilder ( );

      sqlQueryString.AppendLine ( SQL_QUERY_RECORD_VIEW );
      sqlQueryString.AppendLine ( " WHERE ( ( " + EdRecords.DB_DELETED + " = 0 )" );

      if ( QueryParameters.LayoutId != String.Empty )
      {
        sqlQueryString.AppendLine ( " AND ( " + EdRecordLayouts.DB_LAYOUT_ID + " = " + EdRecordLayouts.PARM_LAYOUT_ID + " ) " );
      }

      //
      // Reccord state filter.
      //

      String StateSelection = " AND ( ";

      // 
      // Open the state query expression 
      // 
      if ( QueryParameters.NotSelectedState == true )
      {
        StateSelection = " AND NOT ( ";
      }

      if ( QueryParameters.States.Count > 0 )
      {

        if ( QueryParameters.States.Count == 1 )
        {
          if ( QueryParameters.States [ 0 ] == EdRecordObjectStates.Null )
          {
            sqlQueryString.AppendLine ( " AND NOT ( " + EdRecords.DB_STATE + "= '" + EdRecordObjectStates.Withdrawn + "' ) " );
          }
          else
          {
            if ( QueryParameters.States [ 0 ] == EdRecordObjectStates.Draft_Record )
            {
              sqlQueryString.AppendLine ( StateSelection + EdRecords.DB_STATE + "= '" + EdRecordObjectStates.Draft_Record + "' "
              + "OR  " + EdRecords.DB_STATE + " = '" + EdRecordObjectStates.Empty_Record + "' "
              + "OR " + EdRecords.DB_STATE + " = '" + EdRecordObjectStates.Completed_Record + "') " );
            }
            else
            {
              sqlQueryString.AppendLine ( StateSelection + EdRecords.DB_STATE + " = '" + QueryParameters.States [ 0 ] + "') " );
            }
          }
        }
        else
        {
          // 
          // Open the state query expression 
          // 
          if ( QueryParameters.NotSelectedState == true )
          {
            sqlQueryString.AppendLine ( " AND NOT ( " );
          }
          else
          {
            sqlQueryString.AppendLine ( " AND ( " );
          }

          //
          // Iterate through the list of 
          for ( int i = 0; i < QueryParameters.States.Count; i++ )
          {
            EdRecordObjectStates state = QueryParameters.States [ i ];
            if ( state == EdRecordObjectStates.Null )
            {
              continue;
            }
            if ( i > 1 )
            {
              sqlQueryString.AppendLine ( "AND " );
            }

            if ( state == EdRecordObjectStates.Draft_Record )
            {
              sqlQueryString.AppendLine ( " (" + EdRecords.DB_STATE + "= '" + EdRecordObjectStates.Draft_Record + "' "
                + "OR  " + EdRecords.DB_STATE + " = '" + EdRecordObjectStates.Empty_Record + "' "
                + "OR " + EdRecords.DB_STATE + " = '" + EdRecordObjectStates.Completed_Record + "') " );
            }
            else
            {
              sqlQueryString.AppendLine ( " ( " + EdRecords.DB_STATE + " = '" + state + "') " );
            }

          }//ENd iteration loop
          // 
          // Add the multi state query verbs
          // 
          sqlQueryString.AppendLine ( ") " );

        }//END multiple state selection

      }//END state query

      sqlQueryString.AppendLine ( ") ORDER BY " + EdRecords.DB_RECORD_ID + ";" );

      //
      // Return the sql query string. 
      //
      return sqlQueryString.ToString ( );
    }

    // =====================================================================================
    /// <summary>
    /// This class returns a list of form object based on VisitId, VisitId, FormId and state
    /// </summary>
    /// <param name="ApplicationId">string: (Mandatory) a trial identifier.</param>
    /// <param name="LayoutId">string: (Optional) a form identifier.</param>
    /// <param name="State">EvForm.FormObjecStates: (Optional) a form state.</param>
    /// <returns>List of EvForm: a list of form object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define the sql query parameters and the sql query string.
    /// 
    /// 2. Execute the sql query string and store the results on the data table. 
    /// 
    /// 3. Loop through the table and extract data row to the form object. 
    /// 
    /// 4. Add the formfield items to the form object. 
    /// 
    /// 5. Add the form object to the Forms list. 
    /// 
    /// 6. Return the Forms list. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public List<EdRecord> getRecordList (
      String LayoutId,
      EdRecordObjectStates State )
    {
      this.LogMethod ( "getRecordList method " );
      this.LogValue ( "LayoutId: " + LayoutId );
      this.LogValue ( "State: " + State );

      //
      // Initialize the debuglog, a return list of form object and a formRecord field object. 
      //
      List<EdRecord> view = new List<EdRecord> ( );
      StringBuilder sqlQueryString = new StringBuilder ( );

      // 
      // Define the query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( EdRecordLayouts.PARM_LAYOUT_ID, SqlDbType.NVarChar, 10),
      };
      cmdParms [ 0 ].Value =LayoutId;

      // 
      // Generate the SQL query string.
      // 
      sqlQueryString.AppendLine ( SQL_QUERY_RECORD_VIEW );
      sqlQueryString.AppendLine ( " WHERE  ( " + EdRecordLayouts.DB_DELETED + " = 0 )" );

      if ( LayoutId != String.Empty )
      {
        sqlQueryString.AppendLine ( " AND ( " + EdRecordLayouts.DB_LAYOUT_ID + " = " + EdRecordLayouts.PARM_LAYOUT_ID + " ) " );
      }

      if ( State != EdRecordObjectStates.Null )
      {
        sqlQueryString.AppendLine ( " ( " + EdRecords.DB_STATE + " = '" + State + "') " );
      }

      sqlQueryString.AppendLine ( ") ORDER BY RecordId" );

      this.LogDebug ( sqlQueryString.ToString ( ) );

      this.LogDebug ( " Execute Query" );

      //
      //Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( _sqlQueryString, cmdParms ) )
      {
        // 
        // Iterate through the results extracting the role information.
        // 
        for ( int count = 0; count < table.Rows.Count; count++ )
        {
          // 
          // Extract the table row
          // 
          DataRow row = table.Rows [ count ];

          EdRecord record = this.getRowData ( row, true );

          // 
          // Get the trial record items
          // 
          this.getRecordData ( record );

          //
          // Attach the entity list.
          //
          this.getEntities ( record );

          //
          // attach the record sections.
          //
          this.GetRecordSections ( record );

          // 
          // Add the result to the arraylist.
          // 
          view.Add ( record );

          // 
          // TestReport the visitSchedule count is less than the max size.
          // 
          if ( view.Count > _MaxViewLength )
          {
            break;
          }

        }//END for loop

      }//END Using

      // 
      // Get the array length
      // 
      this.LogValue ( "Returned records: " + view.Count );

      // 
      // Return the result array.
      // 
      return view;

    }//END getView method.

    // =====================================================================================
    /// <summary>
    /// This class retrieves an option list based on query parameters and the useGuid condition
    /// </summary>
    /// <param name="Query">EvQueryParameters: The Query selection values.</param>
    /// <param name="useGuid">Boolean: true, if the option uses Guid.</param>
    /// <returns>List of EvOption: a list of option object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Get a list of form object using query parameters to the form list. 
    /// 
    /// 2. Loop through the form list and extract the option value and description to the Options list. 
    /// 
    /// 3. Return the Options list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> getOptionList (
      EdQueryParameters Query,
      bool useGuid )
    {
      this.LogMethod ( "getOptionList method. " );
      this.LogValue ( "EvQueryParameters parameters:" );

      //
      // Initialize the debug log, a return list of options and an option object.
      //
      List<EvOption> list = new List<EvOption> ( );

      // 
      // Add the null first option to the selection visitSchedule.
      // 
      EvOption option = new EvOption ( );
      if ( useGuid )
      {
        option = new EvOption ( Guid.Empty.ToString ( ), String.Empty );
      }
      list.Add ( option );

      // 
      // Execute method and return the values.
      // 
      List<EdRecord> view = this.getRecordList ( Query );

      // 
      // if the selectionList exits process it.
      // 
      if ( view.Count > 0 )
      {
        // 
        // Iterate through the array visitSchedule extracting the trial records.
        // 
        foreach ( EdRecord record in view )
        {
          option = new EvOption ( record.RecordId, String.Empty );

          if ( useGuid == true )
          {
            option.Value = record.Guid.ToString ( );
          }

          option.Description += record.RecordId + " - " + record.Design.Title;

          // 
          // Append the SelectionObject object to the ArrayList.
          // 
          list.Add ( option );

        }//End iteration loop

      }//END selectionList visitSchedule exists.

      // 
      // Get the array length
      // 
      this.LogValue ( " Returned records: " + list.Count );

      // 
      // Return the visitSchedule array
      // 
      return list;

    }//END geList method

    #endregion

    #region Retrieval methods

    // =====================================================================================
    /// <summary>
    /// This method retrieves a form object based on Guid
    /// </summary>
    /// <param name="RecordGuid">Guid: (Mandatory) Global Unique object identifier (long integer).</param>
    /// <returns>EvForm: a form data object.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Return an empty Form object if the Guid is empty. 
    /// 
    /// 2. Define the sql query parameters and sql query string. 
    /// 
    /// 3. Execute the sql query string and store the results on data table. 
    /// 
    /// 4. Loop through the table and extract the datarow to the form object. 
    /// 
    /// 5. Attach the formfield items to the form object 
    /// 
    /// 6. Return the form data object. 
    /// </remarks>
    //  ------------------------------------------------------------------------------------
    public EdRecord getRecord (
      Guid RecordGuid )
    {
      this.LogMethod ( "getRecord method." );
      this.LogDebug ( "Guid: " + RecordGuid );

      //
      // Initialize the debug log, a return form object and a formfield object.
      //
      EdRecord record = new EdRecord ( );

      // 
      // Validate whether the Guid is not metpy. 
      // 
      if ( RecordGuid == Guid.Empty )
      {
        return record;
      }

      // 
      // Set the query parameter values
      // 
      SqlParameter cmdParms = new SqlParameter ( PARM_RECORD_GUID, SqlDbType.UniqueIdentifier );
      cmdParms.Value = RecordGuid;

      // 
      // Generate SQL query string
      // 
      _sqlQueryString = SQL_QUERY_RECORD_VIEW + " WHERE ( " + EdRecords.DB_RECORD_GUID + "=" + EdRecords.PARM_RECORD_GUID + ") ;";

      //
      // Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( _sqlQueryString, cmdParms ) )
      {
        // 
        // If not rows the return
        // 
        if ( table.Rows.Count == 0 )
        {
          return record;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        // 
        // Fill the role object.
        // 
        record = this.getRowData ( row, false );

      }//END Using method

      // 
      // Attach fields and other trial data.
      // 
      this.getRecordData ( record );

      //
      // load layout fields if record field list is empty.
      //
      this.getLayoutFields ( record );

      //
      // Attache the entity list.
      //
      this.getEntities ( record );

      //
      // Update the form record section references.
      //
      this.GetRecordSections ( record );

      // 
      // Return the trial record.
      // 
      return record;

    }//END getRecord method

    // =====================================================================================
    /// <summary>
    /// This class gets a form object based on the record identifier
    /// </summary>
    /// <param name="SourceId">string: (Mandatory) record identifier.</param>
    /// <returns>EvForm: a form data object.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Return an empty Form object if the RecordId is empty
    /// 
    /// 2. Define the sql query parameters and sql query string. 
    /// 
    /// 3. Execute the sql query string and store the results on data table. 
    /// 
    /// 4. Extract the first datarow to the form object. 
    /// 
    /// 5. Attach the formfield items to the form object 
    /// 
    /// 6. Return the Form data object. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EdRecord getRecordBySource (
      String SourceId )
    {
      //
      // Initialize the debug log, a return form object and a formfield object. 
      //
      this.LogMethod ( "getRecord method. " );
      this.LogDebug ( "SourceId: " + SourceId );

      EdRecord record = new EdRecord ( );

      // 
      // Validate whether the RecordId is not empty. 
      // 
      if ( SourceId == String.Empty )
      {
        return record;
      }

      // 
      // Define the parameters for the query
      // 
      SqlParameter cmdParms = new SqlParameter ( PARM_SOURCE_ID, SqlDbType.NVarChar, 20 );
      cmdParms.Value = SourceId;

      _sqlQueryString = SQL_QUERY_RECORD_VIEW + " WHERE (" + EdRecords.DB_SOURCE_ID + "= " + PARM_SOURCE_ID + " );";

      //
      // Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( _sqlQueryString, cmdParms ) )
      {
        // 
        // If not rows the return
        // 
        if ( table.Rows.Count == 0 )
        {
          return record;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        // 
        // Fill the role object.
        // 
        record = this.getRowData ( row, true );

      }//END Using method

      // 
      // Attach fields and other trial data.
      // 
      this.getRecordData ( record );

      //
      // load layout fields if record field list is empty.
      //
      this.getLayoutFields ( record );

      //
      // Attache the entity list.
      //
      this.getEntities ( record );

      //
      // Update the form record section references.
      //
      this.GetRecordSections ( record );

      // 
      // Return the trial record.
      // 
      return record;

    }//END getItem method

    // =====================================================================================
    /// <summary>
    /// This class gets a record object using RecordId and the form state. 
    /// </summary>
    /// <param name="RecordId">string: (Mandatory) record identifier.</param>
    /// <returns>EvForm: a form data object.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Return an empty Form object if the RecordId is empty.
    /// 
    /// 2. Define the sql query parameters and sql query string. 
    /// 
    /// 3. Execute the sql query string and store the results on data table. 
    /// 
    /// 4. Extract the first datarow to the form object. 
    /// 
    /// 5. Attach the formfield items to the form object 
    /// 
    /// 6. Return the form data object. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EdRecord getRecord ( String RecordId )
    {
      //
      // Initialize the debug log, a return form object and a formfield object. 
      //
      this.LogMethod ( "getRecord method. " );
      this.LogDebug ( "RecordId: " + RecordId );

      EdRecord record = new EdRecord ( );

      // 
      // TestReport that the data object has a valid record identifier.
      // 
      if ( RecordId == String.Empty )
      {
        return record;
      }

      // 
      // Define the parameters for the query
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(PARM_RECORD_ID, SqlDbType.NVarChar, 20),
      };
      cmdParms [ 0 ].Value = RecordId;

      _sqlQueryString = SQL_QUERY_RECORD_VIEW + " WHERE ( " + EdRecords.DB_RECORD_ID + " = " + PARM_RECORD_ID + " );";

      //
      // Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( _sqlQueryString, cmdParms ) )
      {
        // 
        // If not rows the return
        // 
        if ( table.Rows.Count == 0 )
        {
          return record;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        // 
        // Fill the role object.
        // 
        record = this.getRowData ( row, false );

      }//END Using method

      // 
      // Attach fields and other trial data.
      // 
      this.getRecordData ( record );

      //
      // load layout fields if record field list is empty.
      //
      this.getLayoutFields ( record );

      //
      // Attache the entity list.
      //
      this.getEntities ( record );

      //
      // Update the form record section references.
      //
      this.GetRecordSections ( record );

      // 
      // Return the trial record.
      // 
      return record;

    }//END getRecord method

    // =====================================================================================
    /// <summary>
    /// This method updates the form field section references.
    /// </summary>
    /// <param name="FormRecord">EvForm: a form record object</param>
    // ----------------------------------------------------------------------------------
    private void GetRecordSections (
      EdRecord FormRecord )
    {
      this.LogMethod ( "GetRecordSections method." );
      //
      // Initialise the methods variables and objects.
      //
      EdRecordSections sections = new EdRecordSections ( this.ClassParameters );

      FormRecord.Design.FormSections = sections.getSectionList ( FormRecord.LayoutGuid );

      this.LogClass ( sections.Log );

      this.LogMethodEnd ( "GetRecordSections" );
    }//END UpdateFieldSectionReferences method.

    // =====================================================================================
    /// <summary>
    /// This method attaches the other trial data to the record that is needed for the record 
    ///  to be updated.
    /// This data is only to be attached if the record state is editable.  As newField validation
    ///  is not necessary in any other state.
    /// </summary>
    /// <param name="Record">EvForm: (Mandatory) a form data object.</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Get the form record fields 
    /// 
    /// 2. Attach the trial data and milestone data to the form record object. 
    /// 
    /// 3. If the record is in an editable state attach the 
    /// other trial data for record validation
    /// 
    /// 4. If the milestone object exists set the prior to attribute.
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private void getRecordData (
      EdRecord Record )
    {
      this.LogMethod ( "getRecordData." );
      this.LogDebug ( "State: " + Record.StateDesc );
      // 
      // Initialise the methods variables and objects.
      // 
      EdRecordValues dal_RecordValues = new EdRecordValues ( this.ClassParameters );
      // 
      // Get the record fields
      // 
      Record.Fields = dal_RecordValues.getRecordFieldList ( Record );
      this.LogClass ( dal_RecordValues.Log );
      this.LogValue ( "Field count: " + Record.Fields.Count );
      this.LogMethodEnd ( "getRecordData" );

    }//END getRecordData method

    // =====================================================================================
    /// <summary>
    /// This method attaches the other trial data to the record that is needed for the record 
    ///  to be updated.
    /// This data is only to be attached if the record state is editable.  As newField validation
    ///  is not necessary in any other state.
    /// </summary>
    /// <param name="Record">EvForm: (Mandatory) a form data object.</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Get the form record fields 
    /// 
    /// 2. Attach the trial data and milestone data to the form record object. 
    /// 
    /// 3. If the record is in an editable state attach the 
    /// other trial data for record validation
    /// 
    /// 4. If the milestone object exists set the prior to attribute.
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private void getLayoutFields (
      EdRecord Record )
    {
      this.LogMethod ( "getLayoutFields." );
      this.LogDebug ( "State: " + Record.StateDesc );

      if ( Record.Fields.Count > 0
         || Record.State != EdRecordObjectStates.Empty_Record )
      {
        return;
      }

      // 
      // Initialise the methods variables and objects.
      // 
      EdRecordFields dal_RecordFields = new EdRecordFields ( this.ClassParameters );
      // 
      // Get the record fields
      // 
      var fieldlist = dal_RecordFields.GetFieldList ( Record.LayoutGuid );

      for ( int i = 0; i < fieldlist.Count; i++ )
      {
        EdRecordField field = fieldlist [ i ];
        field.FieldGuid = field.Guid;
        field.Guid = Guid.NewGuid ( );

        Record.Fields.Add ( field );
      }

      this.LogClass ( dal_RecordFields.Log );
      this.LogValue ( "Field count: " + Record.Fields.Count );
      this.LogMethodEnd ( "getLayoutFields" );

    }//END getRecordData method

    // ==================================================================================
    /// <summary>
    /// This method retrieves the layout's field objects.
    /// </summary>
    /// <param name="Record">EdRecord object</param>
    //  ---------------------------------------------------------------------------------
    private void getEntities ( EdRecord Record )
    {
      //
      // initialise the methods variables and objects.
      //
      EdRecordEntities dal_RecordEntities = new EdRecordEntities ( this.ClassParameters );

      //
      // if no entities exit.
      //
      if ( Record.Entities.Count == 0 )
      {
        return;
      }

      // 
      // Retrieve the instrument items.
      // 
      Record.Entities = dal_RecordEntities.getEntityList ( Record );
      this.LogClass ( dal_RecordEntities.Log );
    }

    #endregion

    #region Form Record Update queries

    // =====================================================================================
    /// <summary>
    /// This method adds the EvRecord object to the database
    /// </summary>
    /// <param name="Record">EvForm: a form data object</param>
    /// <returns>EvForm: a new form data object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Create new DB row GUID, if the record's Guid is empty.
    /// 
    /// 3. Define the sql query parameters and execute the storeprocedure for creating new items. 
    /// 
    /// 4. Return the form data object. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EdRecord createRecord ( EdRecord Record )
    {
      this.LogMethod ( "createRecord, " );
      this.LogDebug ( "FormId: " + Record.LayoutId );

      // 
      // Create the GUID for the database
      // 
      if ( Record.Guid == Guid.Empty )
      {
        Record.Guid = Guid.NewGuid ( );
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = GetCreateParameters ( );
      SetCreateParameters ( cmdParms, Record );

      this.LogDebug ( EvSqlMethods.ListParameters ( cmdParms ) );

      try
      {
        //
        // Execute the update command.
        //
        EvSqlMethods.StoreProcUpdate ( STORED_PROCEDURE_RECORD_CREATE, cmdParms );
      }
      catch( Exception ex )
      {
        this.LogException ( ex );
      }
      // 
      // Return unique identifier of the new data object.
      // 
      EdRecord record =  this.getRecord ( Record.Guid );

      //
      // Get the empty field objects for the new record.
      //
      this.getLayoutFields ( record );

      this.updateRecordData (record );

      this.LogMethodEnd ( "createRecord" );

      return record;
    } //END createRecord method.

    // =====================================================================================
    /// <summary>
    /// This method creates a new record with the fields prefilled from the preious instance
    ///  of the record.
    /// </summary>
    /// <param name="Record">EvForm: a form object</param>
    /// <returns>EvForm: a form object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Get a list of the patient records to find the last instance of the form type.
    /// 
    /// 2. Create a new instance of the record.
    /// 
    /// 3. Update the new record with the previous record's value.
    /// 
    /// 4. Return the new form record object. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EdRecord createNewUpdateableRecord ( EdRecord Record )
    {
      //
      // Initialize the method debug log and the new form object. 
      //
      this.LogMethod ( "createNewUpdateableRecord method. " );

      EdRecord newRecord = new EdRecord ( );

      //
      // Return the new record
      //
      return newRecord;

    } //END createNewUpdateableRecord method.

    // =====================================================================================
    /// <summary>
    /// This method updates the values in the new record with the values from the previous 
    ///  record of this type collected for the currently selected patient.
    /// </summary>
    /// <param name="RecordList">List of EvForm: a list of existing record object.</param>
    /// <param name="NewRecord">EvForm: a form object containing the new record object.</param>
    /// <returns>EvForm: the last form record object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Iterate through the list of records to retrieve the last record that matches
    /// the form identifier.
    /// 
    /// 2. Return the form data object.
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private EdRecord getLastRecord (
      List<EdRecord> RecordList,
      EdRecord NewRecord )
    {
      //
      // Initialise the last form record object
      //
      EdRecord lastRecord = new EdRecord ( );

      // 
      // Iterate through the list of records to retrieve the last record that matches
      // the form identifier.
      //
      foreach ( EdRecord record in RecordList )
      {
        if ( record.LayoutId == NewRecord.LayoutId )
        {
          lastRecord = record;
        }

      }//END record list iteration loop

      //
      // Return the retrieve record.
      //
      return lastRecord;

    }//END getLastRecord  method

    // =====================================================================================
    /// <summary>
    /// This method returns true if the values in the new record is updated with the values from the previous 
    ///  record of this type collected for the currently selected patient.
    /// </summary>
    /// <param name="NewRecord">EvForm: a form object containing the new record object.</param>
    /// <param name="OldRecord">EvForm: a form object containing the old record object.</param>
    /// <returns>Boolean: true, if the record is updated with new values.</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Return false if the new record object is empty.
    /// 
    /// 2. Iterate through the new records fields and update the fields values from the
    /// historical records.
    /// 
    /// 3. Return true if the new record object is updated.
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private bool updateNewRecordValues (
      EdRecord NewRecord,
      EdRecord OldRecord )
    {
      //
      // Initialize the method debug log
      //
      this.LogMethod ( "updateNewRecordValues method. "
        + " old FormId: " + OldRecord.LayoutId
        + " old RecordId: " + OldRecord.RecordId
        + " new FormId: " + NewRecord.LayoutId
        + " new RecordId: " + NewRecord.RecordId );

      //
      // Validate whether the new record object is not empty.
      //
      if ( NewRecord.Fields.Count == 0 )
      {
        return false;
      }

      //
      // Iterate through the new records fields and initialise the fields that have 
      // historical values.
      //
      foreach ( EdRecordField field in NewRecord.Fields )
      {
        this.LogValue ( " FieldId: " + field.FieldId );

        //
        // Get the field from the old record to retrieve its values.
        //
        EdRecordField oldField = this.getRecordField ( OldRecord, field.FieldId );

        //
        // If the oldfield exists,i.e. it has a non empty Guid 
        // Initialise the new field with its value.
        //
        if ( oldField.Guid != Guid.Empty )
        {
          this.LogValue ( " >> Field value updated" );

          //
          // Update the field data values.
          //
          field.ItemText = oldField.ItemText;
          field.ItemValue = oldField.ItemValue;

          //
          // Process table cell data of a table or matrix field type.
          //
          if ( field.TypeId == Evado.Model.EvDataTypes.Table
            || field.TypeId == Evado.Model.EvDataTypes.Special_Matrix )
          {
            this.LogValue ( " >> Update Table" );

            this.updateTableValues ( field, oldField );

          }//END table or matrix field type

        }//END old field exists update new field.

      }//END field iteration loop

      return true;

    }//END updateNewRecordValues method

    // =====================================================================================
    /// <summary>
    /// This method updates the values in the new record with the values from the previous 
    ///  record of this type collected for the currently selected patient.
    /// </summary>
    /// <param name="NewRecordField">EvForm: a form record object containing the new record object.</param>
    /// <param name="OldRecordField">EvForm: a form record object containing the old record object.</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Validate whether both new and old form record objects have items.
    /// 
    /// 2. Iterate through the rows in the table for updating the value in the new record.
    /// 
    /// 3. Iterate through the row columns updating the cell values using the 
    /// the column header text as the key
    /// 
    /// 4. Retrieve the value for the current row and column from the previous record
    /// 
    /// 5. Update the table cell value in the new record.
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private void updateTableValues (
      EdRecordField NewRecordField,
      EdRecordField OldRecordField )
    {
      //
      // Validate whether both new and old form record objects have items.
      //
      if ( NewRecordField.Table != null && OldRecordField.Table != null )
      {
        this.LogValue ( " >> new table exists" );
        if ( OldRecordField.Table != null )
        {
          this.LogValue ( " >> old table exists" );

          //
          // Iterate through the rows in the table for updating the value in the new record.
          //
          for ( int iRow = 0; iRow < NewRecordField.Table.Rows.Count; iRow++ )
          {
            //
            // Iterate through the row columns updating the cell values using the 
            // the column header text as the key.
            //
            for ( int iColumn = 0; iColumn < NewRecordField.Table.Header.Length; iColumn++ )
            {
              //
              // Extract the column header from the new record.
              //
              EdRecordTableHeader columnHeader = NewRecordField.Table.Header [ iColumn ];

              //
              // Retrieve the value for the current row and column from the previous record.
              //
              String cellValue = this.getTableCellData ( OldRecordField.Table, iRow, columnHeader );

              // 
              // Update the table cell value in the new record.
              //
              NewRecordField.Table.Rows [ iRow ].Column [ iColumn ] = cellValue;

            }//END field column iteration loop

          }//END table row iteration loop
        }
      }//END table object exist in both new and old field.
    }//END updateTableValues class

    // =====================================================================================
    /// <summary>
    ///  This method get the value of a table column.
    ///  The iteration process is designed to cater for the scenario where a tables columns
    ///  have been repositioned. i.e. a column has been added in the middle of the table.
    /// </summary>
    /// <param name="OldFieldTable">EvFormFieldTable: a formfield table containing the rows of data to be retrieved.</param>
    /// <param name="SelectedRow">Integer: The row count in the table.</param>
    /// <param name="ColumnHeader">EvFormFieldTableColumnHeader: a column header object containing the new fields table column data.</param>
    /// <returns>String: a table cell value string</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Iterate through the column header 
    /// 
    /// 2. Return the column value if the column header exists
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private String getTableCellData (
      EdRecordTable OldFieldTable,
      int SelectedRow,
      EdRecordTableHeader ColumnHeader )
    {
      //
      // iterate through the columns of the table looking for a column header that matches the 
      // passed column header text and return the column value and data type.
      //
      for ( int iColumn = 0; iColumn < OldFieldTable.Header.Length; iColumn++ )
      {
        EdRecordTableHeader header = OldFieldTable.Header [ iColumn ];

        if ( header.Text == ColumnHeader.Text
          && header.TypeId == ColumnHeader.TypeId )
        {
          return OldFieldTable.Rows [ SelectedRow ].Column [ iColumn ];
        }
      }

      return String.Empty;

    }//END getTableCellData method

    // =====================================================================================
    /// <summary>
    ///  This class matches the form field using fieldId. 
    /// </summary>
    /// <param name="Record">EvForm: a form object containing the record object..</param>
    /// <param name="FieldId">String: The string value of a form field identifier.</param>
    /// <returns>EvFormField: a formfield object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Iterate through the form record object 
    /// 
    /// 2. Return the retrieve formfield object if formId exists.
    /// 
    /// 3. Else return new formfield object.
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private EdRecordField getRecordField ( EdRecord Record, String FieldId )
    {
      //
      // iterate through the list of fields to retrieve the matching field by it identifier.
      //
      foreach ( EdRecordField field in Record.Fields )
      {
        if ( field.FieldId == FieldId )
        {
          return field;
        }

      }//END field iteration loop

      return new EdRecordField ( );

    }//END getRecorField method

    // =====================================================================================
    /// <summary>
    /// This method updates the form table with the record data object.
    /// </summary>
    /// <param name="Record">EvForm: a form data object</param>
    /// <returns>EvEventCodes: an event code for updating form record object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit if the old record's Guid is empty
    /// 
    /// 2. Define the sql query parameters and execute the storeprocedure. 
    /// 
    /// 3. Update the formfield items
    /// 
    /// 4. Add changed items to datachange table. 
    /// 
    /// 5. Add commentlist to the record table. 
    /// 
    /// 6. Return the event code for updating items. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvEventCodes updateRecord ( EdRecord Record )
    {
      //
      // Initialize the method debug log, internal variables and objects. 
      //
      this.LogMethod ( "updateRecord, " );
      this.LogDebug ( "UserProfile.RoleId: " + this.ClassParameters.UserProfile.Roles );
      this.LogDebug ( "Guid: " + Record.Guid );
      this.LogDebug ( "RecordId: " + Record.RecordId );
      this.LogDebug ( "FormGuid: " + Record.LayoutGuid );
      this.LogDebug ( "State: " + Record.State );

      int databaseRecordAffected = 0;
      EvDataChanges dataChanges = new EvDataChanges ( this.ClassParameters );
      EvEventCodes eventCode = EvEventCodes.Ok;

      //
      // Set the skip retrieve comments to true to not retrieve comments
      // when validating the record object for saving.
      //
      this._SkipRetrievingComments = true;

      // 
      // Validate whether the record object exists.
      // 
      this.LogDebug ( "Get previous record" );
      EdRecord previousRecord = this.getRecord ( Record.RecordId );
      if ( previousRecord.Guid == Guid.Empty )
      {
        return EvEventCodes.Identifier_Record_Id_Error;
      }

      // 
      // Set the data change object values.
      // 
      EvDataChange dataChange = this.setChangeRecord ( Record, previousRecord );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = GetParameters ( );
      SetParameters ( cmdParms, Record );

      //
      // Execute the update command.
      //
      databaseRecordAffected = EvSqlMethods.StoreProcUpdate ( STORED_PROCEDURE_RECORD_UPDATE, cmdParms );
      if ( databaseRecordAffected == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      //
      // Update the records field values..
      //
      this.updateRecordData ( Record );

      //
      // Update the records comments.
      //
      this.updateRecordComments ( Record );
      // 
      // Add the data change values to the database.
      // 
      dataChanges.AddItem ( dataChange );
      this.LogValue ( "DataChange status: " + dataChanges.Log );

      //
      // Add record comments.
      //
      // 
      // Return event exit code.
      // 
      return eventCode;

    } //END updateRecord method.

    // =====================================================================================
    /// <summary>
    /// This method update the records values.
    /// </summary>
    /// <param name="Record">EdRecord: object.</param>
    //  ----------------------------------------------------------------------------------
    private EvEventCodes updateRecordData (
      EdRecord Record )
    {
      this.LogMethod ( "updateRecordData." );
      this.LogDebug ( "State: " + Record.StateDesc );
      // 
      // Initialise the methods variables and objects.
      // 
      EdRecordValues dal_RecordValues = new EdRecordValues ( this.ClassParameters );

      // 
      this.LogValue ( "Record.Fields.Count: " + Record.Fields.Count );
      if ( Record.Fields.Count == 0 )
      {
        return EvEventCodes.Ok;
      }
      // 
      // update record values
      // 
      var result = dal_RecordValues.UpdateFields ( Record );
      this.LogClass ( dal_RecordValues.Log );

      if ( result != EvEventCodes.Ok )
      {
        this.LogEvent ( "Record value update error encountered, error value '" + result + "'" );
      }

      this.LogMethodEnd ( "updateRecordData" );
      return result;
    }//END getRecordData method


    // =====================================================================================
    /// <summary>
    /// This method update the records values.
    /// </summary>
    /// <param name="Record">EdRecord: object.</param>
    //  ----------------------------------------------------------------------------------
    private EvEventCodes updateRecordComments (
      EdRecord Record )
    {
      this.LogMethod ( "updateRecordComments." );
      this.LogDebug ( "State: " + Record.StateDesc );
      // 
      // Initialise the methods variables and objects.
      // 
      EdRecordComments recordComments = new EdRecordComments ( this.ClassParameters );

      if ( Record.CommentList.Count == 0 )
      {
        return EvEventCodes.Ok;
      }

      // 
      // update record values
      // 
      var result = recordComments.addNewComments ( Record.CommentList );
      this.LogValue ( "RecordComment Log: " + recordComments.Log );


      this.LogMethodEnd ( "updateRecordComments" );
      return result;
    }//END getRecordData method
    #endregion

    #region Update Difference methods

    // =====================================================================================
    /// <summary>
    /// This class sets the data change object for the update action.
    /// </summary>
    /// <param name="NewRecord">EvForm: a form object of new record version</param>
    /// <param name="OldRecord">EvForm: a form object of existing record data</param>
    /// <returns>EvDataChange: a datachange object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Add new items values to datachange object if they do not exist on the current form table. 
    /// 
    /// 2. Add new items values to the datachange formfield object. 
    /// 
    /// 3. Return the datachange object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvDataChange setChangeRecord ( EdRecord NewRecord, EdRecord OldRecord )
    {
      this.LogMethod ( "setChangeRecord method " );
      // 
      // Initialise the datachange object.
      // 
      EvDataChange dataChange = new EvDataChange ( );

      // 
      // Set the date change object values.
      //       
      dataChange.Guid = Guid.NewGuid ( );
      dataChange.TableName = EvDataChange.DataChangeTableNames.EvRecords;
      dataChange.RecordId = NewRecord.RecordId;
      dataChange.RecordGuid = NewRecord.Guid;
      dataChange.UserId = this.ClassParameters.UserProfile.UserId;
      dataChange.DateStamp = DateTime.Now;

      //
      // Add items values to the datachange object if they do not exist. 
      //
      dataChange.AddItem ( "RecordDate", OldRecord.RecordDate, NewRecord.RecordDate );

      dataChange.AddItem ( "State", OldRecord.State.ToString ( ), NewRecord.State.ToString ( ) );

      //
      // Iterate through the record content comments.
      //
      if ( OldRecord.CommentList.Count < NewRecord.CommentList.Count )
      {
        //
        // Iterate through the record content comments.
        //
        for ( int count = 0; count < NewRecord.CommentList.Count; count++ )
        {
          if ( count < OldRecord.CommentList.Count )
          {
            AddCommentToDataChange (
              "Comment_",
              dataChange,
              count,
              OldRecord.CommentList [ count ],
              NewRecord.CommentList [ count ] );
          }
          else
          {
            AddCommentToDataChange (
              "Comment_",
              dataChange,
              count, new EdFormRecordComment ( ),
              NewRecord.CommentList [ count ] );
          }
        }
      }

      //
      // Iterate through the signoffs.
      //
      for ( int count = 0; count < NewRecord.Signoffs.Count; count++ )
      {
        if ( OldRecord.Signoffs.Count <= count )
        {
          this.AddSignoffToDataChange (
            dataChange,
            count,
            new EdUserSignoff ( ),
            NewRecord.Signoffs [ count ] );
        }
        else
        {
          this.AddSignoffToDataChange (
            dataChange,
            count,
             OldRecord.Signoffs [ count ],
            NewRecord.Signoffs [ count ] );
        }
      }//END annotation 


      // 
      // Append the record new formField values.
      // 
      if ( NewRecord.Fields.Count > 0 )
      {
        this.LogValue ( " Updating field differences." );

        // 
        // Iterate through the form fields outputting the main newField values.
        // 
        foreach ( EdRecordField newField in NewRecord.Fields )
        {
          EdRecordField oldField = getRecordField ( OldRecord.Fields, newField.FieldId );

          // 
          // Set the form Field for difference data.
          // 
          this.setChangeRecordField ( dataChange, oldField, newField );

        }//END fields not null.

        this.LogValue ( " Data change processing of form fields" );

      }//END record has fields.

      // 
      // Return the data change object.
      // 
      return dataChange;

    }//END setChangeRecord method

    //===================================================================================
    /// <summary>
    /// This method add a comment to the data change list.
    /// </summary>
    /// <param name="Prefix">String: the data change prefix identifier</param>
    /// <param name="dataChange">EvDataChange object</param>
    /// <param name="count">int: the count of the change.</param>
    /// <param name="OldComment">EvFormRecordComment: old comment object</param>
    /// <param name="NewComment">EvFormRecordComment: new comment object</param>
    //---------------------------------------------------------------------------------
    private void AddCommentToDataChange (
      String Prefix,
      EvDataChange dataChange,
      int count,
     EdFormRecordComment OldComment,
      EdFormRecordComment NewComment )
    {

      dataChange.AddItem ( Prefix + "_AuthorType_" + count,
        OldComment.AuthorType,
        NewComment.AuthorType );

      dataChange.AddItem ( Prefix + "_CommentDate_" + count,
        OldComment.CommentDate,
        OldComment.CommentDate );

      dataChange.AddItem ( Prefix + "_CommentType_" + count,
        OldComment.CommentType,
       NewComment.CommentType );

      dataChange.AddItem ( Prefix + "_Content_" + count,
        OldComment.Content,
        NewComment.Content );

      dataChange.AddItem ( Prefix + "_NewComment_" + count,
        OldComment.NewComment,
        NewComment.NewComment );

      dataChange.AddItem ( Prefix + "_UserId_" + count,
        OldComment.UserId,
        NewComment.UserId );

      dataChange.AddItem ( Prefix + "_UserCommonName_" + count,
       OldComment.UserCommonName,
       NewComment.UserCommonName );

    }//End AddCommentToDataChange method


    //===================================================================================
    /// <summary>
    /// This method add a comment to the data change list.
    /// </summary>
    /// <param name="dataChange">EvDataChange object</param>
    /// <param name="count">int: the count of the change.</param>
    /// <param name="OldRecord">EvUserSignoff: old comment object</param>
    /// <param name="NewRecord">EvUserSignoff: new comment object</param>
    //---------------------------------------------------------------------------------
    private void AddSignoffToDataChange (
      EvDataChange dataChange,
      int count,
      EdUserSignoff OldRecord,
      EdUserSignoff NewRecord )
    {
      dataChange.AddItem ( "Signoff_Description_" + count,
        OldRecord.Description,
        NewRecord.Description );

      dataChange.AddItem ( "Signoff_SignedOffBy_" + count,
        OldRecord.SignedOffBy,
        NewRecord.SignedOffBy );

      dataChange.AddItem ( "Signoff_SignedOffUserId_" + count,
        OldRecord.SignedOffUserId,
        NewRecord.SignedOffUserId );

      dataChange.AddItem ( "Signoff_SignOffDate_" + count,
       OldRecord.SignOffDate,
        NewRecord.SignOffDate );

      dataChange.AddItem ( "Signoff_Type_" + count,
        OldRecord.Type,
        NewRecord.Type );
    }

    // =====================================================================================
    /// <summary>
    /// This method returns the selected EvFormField for a newField identifier.
    /// </summary>
    /// <param name="FieldList">List of EvFormField: a list of formfield object items</param>
    /// <param name="FieldId">String: a field identifier</param>
    /// <returns>EvFormField: a formfield object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Loop through the formfield object list. 
    /// 
    /// 2. Return the retrieving formfield if it exist. 
    /// 
    /// 3. Else return new formfield object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private EdRecordField getRecordField ( List<EdRecordField> FieldList, String FieldId )
    {
      foreach ( EdRecordField field in FieldList )
      {
        // 
        // Return the matching newField.
        // 
        if ( field.FieldId == FieldId )
        {
          return field;
        }
      }
      // 
      // IF none return empty object.
      // 
      return new EdRecordField ( );
    }//END getRecordField class

    // =====================================================================================
    /// <summary>
    /// This class sets the data change object for the record fields action.
    /// </summary>
    /// <param name="DataChange">EvDataChange: a datachange object</param>
    /// <param name="NewField">EvFormField: a formfield object of new record version</param>
    /// <param name="OldField">EvFormField: a formfield object of existing record data</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Add new items to formfield table if they do not exist. 
    /// 
    /// 2. Add comment to the formfield table. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private void setChangeRecordField (
      EvDataChange DataChange,
      EdRecordField OldField,
      EdRecordField NewField )
    {
      //
      // Initialize the debug log
      //
      this.LogValue ( " FieldId: old: " + OldField.FieldId + " new: " + NewField.FieldId );

      // 
      // Add new formfield value item if it does not exist.
      // 
      if ( OldField.ItemValue != NewField.ItemValue )
      {
        DataChange.AddItem (
           NewField.FieldId.Replace ( " ", "_" ) + "_Value",
           OldField.ItemValue,
           NewField.ItemValue );
      }

      // Add new formfield text item if it does not exist.
      if ( OldField.ItemText != NewField.ItemText )
      {
        DataChange.AddItem (
          NewField.FieldId.Replace ( " ", "_" ) + "_ValueText",
          OldField.ItemText,
          NewField.ItemText );
      }

    }//END setChangeRecordField method


    #endregion

    #region Lock Records methods

    // =====================================================================================
    /// <summary>
    /// This class locks the trial record for single user update.
    /// </summary>
    /// <param name="Record">EvForm: a form object</param>
    /// <returns>EvEventCodes: an event code for locking record</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the Record's Guid is empty 
    /// 
    /// 2. Define the sql query parameters and execute the storeprocedure for locking the record.
    /// 
    /// 3. Return the event code for locking records. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvEventCodes lockRecord ( EdRecord Record )
    {
      // 
      // Initialise the method debug log and the number of updated records.
      // 
      this.LogMethod ( "lockRecord method " );
      this.LogValue ( "Guid: " + Record.Guid );
      int RecordsUpdated = 0;

      // 
      // Validate whether the record Guid does not exist. 
      // 
      if ( Record.Guid == Guid.Empty )
      {
        return EvEventCodes.Identifier_Global_Unique_Identifier_Error;
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(PARM_RECORD_GUID, SqlDbType.UniqueIdentifier),
        new SqlParameter(PARM_UPDATED_BY_USER_ID, SqlDbType.NVarChar, 100),
        new SqlParameter(PARM_UPDATED_BY, SqlDbType.NVarChar, 100),
        new SqlParameter(PARM_UPDATED_DATE, SqlDbType.DateTime)
      };
      cmdParms [ 0 ].Value = Record.Guid;
      cmdParms [ 1 ].Value = this.ClassParameters.UserProfile.UserId;
      cmdParms [ 2 ].Value = this.ClassParameters.UserProfile.CommonName;
      cmdParms [ 3 ].Value = DateTime.Now;

      //
      // Execute the update command.
      //
      if ( ( RecordsUpdated = EvSqlMethods.StoreProcUpdate ( STORED_PROCEDURE_RECORD_LOCK, cmdParms ) ) == 0 )
      {
        this.LogValue ( " RecordsUpdated " + RecordsUpdated + " " );
        return EvEventCodes.Database_Record_UnLock_Error;
      }

      this.LogValue ( "RecordsUpdated " + RecordsUpdated + " " );
      // 
      // Return successful update state.
      // 
      return EvEventCodes.Ok;

    } //END lockItem method.

    // =====================================================================================
    /// <summary>
    /// This class unlocks the the trial record.
    /// </summary>
    /// <param name="Item">EvForm: a form object</param>
    /// <returns>EvEventCodes: an event code for unlocking records.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the Form's Guid is empty 
    /// 
    /// 2. Define the sql query parameters and execute the storeprocedure for unlocking the record.
    /// 
    /// 3. Return the event code for locking record. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvEventCodes unlockRecord ( EdRecord Item )
    {
      // 
      // Initialise the method debug log and the number of updated records.
      // 
      this.LogMethod ( "unlockRecord method " );
      this.LogValue ( "Guid: " + Item.Guid );
      int RecordsUpdated = 0;

      // 
      // Check that the data object has valid identifiers to add it to the database.
      // 
      if ( Item.Guid == Guid.Empty )
      {
        return EvEventCodes.Identifier_Global_Unique_Identifier_Error;
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(PARM_RECORD_GUID, SqlDbType.UniqueIdentifier),
        new SqlParameter(PARM_UPDATED_BY_USER_ID, SqlDbType.NVarChar, 100),
        new SqlParameter(PARM_UPDATED_BY, SqlDbType.NVarChar, 100),
        new SqlParameter(PARM_UPDATED_DATE, SqlDbType.DateTime),
        new SqlParameter(PARM_BOOKED_OUT, SqlDbType.NVarChar, 100),
      };
      cmdParms [ 0 ].Value = Item.Guid;
      cmdParms [ 1 ].Value = this.ClassParameters.UserProfile.UserId;
      cmdParms [ 2 ].Value = this.ClassParameters.UserProfile.CommonName;
      cmdParms [ 3 ].Value = DateTime.Now;
      cmdParms [ 4 ].Value = Item.BookedOutBy;

      //
      // Execute the update command.
      //
      if ( ( RecordsUpdated = EvSqlMethods.StoreProcUpdate ( STORED_PROCEDURE_RECORD_UNLOCK, cmdParms ) ) == 0 )
      {
        this.LogValue ( " RecordsUpdated " + RecordsUpdated + " " );
        return EvEventCodes.Database_Record_UnLock_Error;
      }
      this.LogValue ( " RecordsUpdated " + RecordsUpdated + " " );

      // 
      // Return successful update state.
      // 
      return EvEventCodes.Ok;

    } //END unlockRecord method.

    #endregion

    /// <summary>
    /// This class is handles the data access layer for the form records data object.
    /// </summary>
    public class sortOndate : IComparer<EdFormRecordComment>
    {
      /// <summary>
      /// This method sorts the list busing the ICompare interface
      /// </summary>
      /// <param name="a">EvFormRecordComment object</param>
      /// <param name="b">EvFormRecordComment object</param>
      /// <returns></returns>
      public int Compare ( EdFormRecordComment a, EdFormRecordComment b )
      {
        if ( a.CommentDate > b.CommentDate ) return 1;
        else if ( a.CommentDate < b.CommentDate ) return -1;
        else return 0;
      }
    }

  }//END EvFormRecords class

}//END namespace Evado.Dal.Digital
