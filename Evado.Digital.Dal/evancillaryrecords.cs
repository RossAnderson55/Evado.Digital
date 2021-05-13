/***************************************************************************************
 * <copyright file="BLL\EvAncilliaryRecords.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2021 EVADO HOLDING PTY. LTD..  All rights reserved.
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
 *  This class contains the EvSubjectRecords business object.
 *
 ****************************************************************************************/

using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Text;

//Annotations to XML instrument System specific libraries

using Evado.Model;
using Evado.Digital.Model;


namespace Evado.Digital.Dal
{
  /// <summary>
  /// This class is handles the data access layer for the ancillary record data object.
  /// </summary>
  public class EvAncillaryRecords : EvDalBase
  {
    // ==================================================================================
    /// <summary>
    /// This is the class initialisation method.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EvAncillaryRecords ( )
    {
      this.ClassNameSpace = "Evado.Digital.Dal.Digital.EvAncillaryRecords.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EvAncillaryRecords ( EvClassParameters Settings )
    {
      this.ClassParameters = Settings;
      this.ClassNameSpace = "Evado.Digital.Dal.Digital.EvAncillaryRecords.";

      if ( this.ClassParameters.LoggingLevel == 0 )
      {
        this.ClassParameters.LoggingLevel = Evado.Digital.Dal.EvStaticSetting.LoggingLevel;
      }
    }

    #region Object Initialisation

    //  ==================================================================================
    /// 
    /// Define the selectionList query string.
    /// 
    private const string _sqlQuery_View = "Select * FROM EvSubjectRecord_View ";

    /// 
    /// Define the stored procedure names.
    /// 
    private const string _STORED_PROCEDURE_AddItem = "usr_SubjectRecord_add";
    private const string _STORED_PROCEDURE_UpdateItem = "usr_SubjectRecord_update";
    private const string _STORED_PROCEDURE_DeleteItem = "usr_SubjectRecord_delete";
    private const string _STORED_PROCEDURE_LockItem = "usr_SubjectRecord_lock";
    private const string _STORED_PROCEDURE_UnlockItem = "usr_SubjectRecord_unlock";

    /// 
    /// Define the query parameter constants.
    /// 
    private const string PARM_Guid = "@Guid";
    private const string PARM_RecordId = "@RecordId";
    private const string PARM_RecordDate = "@RecordDate";
    private const string PARM_TrialId = "@TrialId";
    private const string PARM_SubjectId = "@SubjectId";
    private const string PARM_Subject = "@Subject";
    private const string PARM_Record = "@Record";
    private const string PARM_BinaryLength = "@BinaryLength";
    private const string PARM_BinaryObject = "@BinaryObject";
    private const string PARM_BinaryType = "@BinaryType";
    private const string PARM_BinaryExtension = "@BinaryExtension";
    private const string PARM_XmlData = "@XmlData";
    private const string PARM_Researcher = "@Researcher";
    private const string PARM_ResearcherUserId = "@ResearcherUserId";
    private const string PARM_ResearcherDate = "@ResearcherDate";
    private const string PARM_Reviewer = "@Reviewer";
    private const string PARM_ReviewerUserId = "@ReviewerUserId";
    private const string PARM_ReviewDate = "@ReviewDate";
    private const string PARM_Approver = "@Approver";
    private const string PARM_ApproverUserId = "@ApproverUserId";
    private const string PARM_ApprovalDate = "@ApprovalDate";
    private const string PARM_State = "@State";
    private const string PARM_UpdatedByUserId = "@UpdatedByUserId";
    private const string PARM_UpdatedBy = "@UpdatedBy";
    private const string PARM_UpdateDate = "@UpdateDate";
    private const string PARM_BookedOut = "@BookedOutBy";
    private const string PARM_Signoffs = "@Signoffs";
    /// 
    /// Instantiate the variables and properties.
    /// 
    private string _sqlQueryString = String.Empty;

    // +++++++++++++++++++++++++++ END INITIALISATION SECTION ++++++++++++++++++++++++++++++
    #endregion

    #region Set Query Parameters

    //  ==================================================================================
    /// <summary>
    /// This method returns an array of sql query parameters. 
    /// </summary>
    /// <returns>SqlParameter: An array parms of sql parameters</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Create an array of sql query parameters. 
    /// 
    /// 2. Return an array of sql query parameters. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private static SqlParameter [ ] GetParameters( )
    {
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( PARM_Guid, SqlDbType.UniqueIdentifier),
        new SqlParameter( PARM_TrialId, SqlDbType.NVarChar, 10),
        new SqlParameter( PARM_SubjectId, SqlDbType.NVarChar, 20),
        new SqlParameter( PARM_RecordId, SqlDbType.NVarChar, 20),
        new SqlParameter( PARM_RecordDate, SqlDbType.DateTime),
        new SqlParameter( PARM_Subject, SqlDbType.NVarChar, 50),
        new SqlParameter( PARM_Record , SqlDbType.NText),
        new SqlParameter( PARM_BinaryLength , SqlDbType.Int),
        new SqlParameter( PARM_BinaryObject , SqlDbType.Image),
        new SqlParameter( PARM_BinaryType , SqlDbType.NVarChar,100),
        new SqlParameter( PARM_BinaryExtension, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_XmlData, SqlDbType.NText),
        new SqlParameter( PARM_Researcher, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_ResearcherUserId, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_ResearcherDate, SqlDbType.DateTime),
        new SqlParameter( PARM_Reviewer, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_ReviewerUserId, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_ReviewDate, SqlDbType.DateTime),
        new SqlParameter( PARM_Approver, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_ApproverUserId, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_ApprovalDate, SqlDbType.DateTime),
        new SqlParameter( PARM_State, SqlDbType.VarChar, 20),
        new SqlParameter( PARM_UpdatedByUserId, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_UpdatedBy, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_UpdateDate, SqlDbType.DateTime),
        new SqlParameter( PARM_BookedOut, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_Signoffs, SqlDbType.NText),
      };
      return cmdParms;
    }//END GetParameters class

    // =====================================================================================
    /// <summary>
    /// This method assigns the SubjectRecord object's values to the array of sql parameters. 
    /// </summary>
    /// <param name="cmdParms">SqlParameter: An array of sql parameters.</param>
    /// <param name="Record">EvSubjectRecord: A milestone record object</param>
    /// <remarks>
    /// This method consists of the following step: 
    /// 
    /// 1. Bind the milestone record object's values to the array of sql parameters. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private void SetParameters( SqlParameter [ ] cmdParms, EvAncillaryRecord Record )
    {
      // 
      // Bind the milestone record object's values to the array of sql parameters. 
      // 
      cmdParms [ 0 ].Value = Record.Guid;
      cmdParms [ 1 ].Value = Record.ProjectId;
      cmdParms [ 2 ].Value = Record.SubjectId;
      cmdParms [ 3 ].Value = Record.RecordId;
      cmdParms [ 4 ].Value = Record.RecordDate;
      cmdParms [ 5 ].Value = Record.Subject;
      cmdParms [ 6 ].Value = Record.Record;
      cmdParms [ 7 ].Value = Record.BinaryLength;
      cmdParms [ 8 ].Value = Record.BinaryObject;
      cmdParms [ 9 ].Value = Record.BinaryType;
      cmdParms [ 10 ].Value = Record.BinaryExtension;
      cmdParms [ 11 ].Value = Record.XmlData;
      cmdParms [ 12 ].Value = Record.Researcher;
      cmdParms [ 13 ].Value = Record.ResearcherUserId;
      cmdParms [ 14 ].Value = Record.ResearcherDate;
      cmdParms [ 15 ].Value = Record.Reviewer;
      cmdParms [ 16 ].Value = Record.ReviewerUserId;
      cmdParms [ 17 ].Value = Record.ReviewDate;
      cmdParms [ 18 ].Value = Record.Approver;
      cmdParms [ 19 ].Value = Record.ApproverUserId;
      cmdParms [ 20 ].Value = Record.ApprovalDate;
      cmdParms [ 21 ].Value = Record.State;
      cmdParms [ 22 ].Value = Record.UpdatedByUserId;
      cmdParms [ 23 ].Value = Record.UserCommonName;
      cmdParms [ 24 ].Value = DateTime.Now;
      cmdParms [ 25 ].Value = Record.BookedOutBy;
      cmdParms [ 26 ].Value = Evado.Model.EvStatics.SerialiseObject<List<EdUserSignoff>> ( Record.Signoffs );

    }//END SetParameters class.

    #endregion

    #region ancillary record Reader

    // =====================================================================================
    /// <summary>
    /// This method extracts the data row values to the Subject Record object. 
    /// </summary>
    /// <param name="Row">DataRow: A data row object</param>
    /// <returns>EvSubjectRecord: A milestone record object.</returns>
    /// <remarks>
    /// This method consists of following steps:  
    /// 
    /// 1. Extract the compatible data row values to the Subject record object. 
    /// 
    /// 2. Return the milestone record object. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private EvAncillaryRecord readRowData( DataRow Row )
    {
      // 
      // Initialise the method variables and objects.
      // 
      EvAncillaryRecord record = new EvAncillaryRecord( );

      // 
      // Extract the data object values.
      // 
      record.Guid = EvSqlMethods.getGuid( Row, "TSR_Guid" );
      record.ProjectId = EvSqlMethods.getString( Row, "TrialId" );
      record.SubjectId = EvSqlMethods.getString( Row, "SubjectId" );
      record.RecordId = EvSqlMethods.getString( Row, "RecordId" );
      record.RecordDate = EvSqlMethods.getDateTime( Row, "TSR_RecordDate" );
      record.Subject = EvSqlMethods.getString( Row, "TSR_Subject" );
      record.Record = EvSqlMethods.getString( Row, "TSR_Record" );

      // 
      // Perform binary object management
      // 
      record.BinaryLength = EvSqlMethods.getInteger( Row, "TSR_BinaryLength" );

      if ( record.BinaryLength > 0 )
      {
        record.BinaryObject = EvSqlMethods.getBytes( Row, "TSR_BinaryObject" );
        record.BinaryType = EvSqlMethods.getString( Row, "TSR_BinaryType" );
        record.BinaryExtension = EvSqlMethods.getString( Row, "TSR_BinaryExtension" );
      }

      record.XmlData = EvSqlMethods.getString( Row, "TSR_XmlData" );
      record.Researcher = EvSqlMethods.getString( Row, "TSR_Researcher" );
      record.ResearcherDate = EvSqlMethods.getDateTime( Row, "TSR_ResearcherDate" );
      record.Reviewer = EvSqlMethods.getString( Row, "TSR_Reviewer" );
      record.ReviewDate = EvSqlMethods.getDateTime( Row, "TSR_ReviewDate" );
      record.Approver = EvSqlMethods.getString( Row, "TSR_Approver" );
      record.ApprovalDate = EvSqlMethods.getDateTime( Row, "TSR_ApprovalDate" );
      record.State = Evado.Model.EvStatics.parseEnumValue<EdRecordObjectStates> ( EvSqlMethods.getString ( Row, "TSR_State" ) );
      record.UpdatedByUserId = EvSqlMethods.getString( Row, "TSR_UpdatedByUserId" );
      record.UpdatedBy = EvSqlMethods.getString( Row, "TSR_UpdatedBy" );
      record.UpdatedDate = EvSqlMethods.getDateTime( Row, "TSR_UpdateDate" );
      record.BookedOutBy = EvSqlMethods.getString( Row, "TSR_BookedOutBy" );
      record.Signoffs = Evado.Model.EvStatics.DeserialiseObject<List<EdUserSignoff>> ( EvSqlMethods.getString ( Row, "TSR_Signoffs" ) );

      // 
      // Return an object containing EvSubjectRecord object. 
      // 
      return record;

    }//END getRowData

    #endregion

    #region List Queries

    // =====================================================================================
    /// <summary>
    /// This method returns a list of Subject record object based on TrialId, SubjectId, State and OrderBy value
    /// </summary>
    /// <param name="ProjectId">String: A Project identifier.</param>
    /// <param name="SubjectId">String: A Subject identifier</param>
    /// <param name="State">String: A string that defines the record state to the query.</param>
    /// <param name="OrderBy">String: A string that defines the sorting order of the result set.</param>
    /// <returns>List of EvSubjectRecord: A list containing SubjectRecord object.</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Define the sql query parameters and sql query string.
    /// 
    /// 2. Execute the sql query string and store the results on datatable. 
    /// 
    /// 3. Loop through the table and extract data row to the SubjectRecord object. 
    /// 
    /// 4. Add SubjectRecord object's values to the list of Subject Records
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public List<EvAncillaryRecord> getView( string ProjectId,
      string SubjectId, string State, string OrderBy )
    {
      this.LogMethod ( "getView, " );
      this.LogValue ( "ProjectId: " + ProjectId );
      this.LogValue ( "SubjectId: " + SubjectId );
      // 
      // Define the local variable.
      // 
      List<EvAncillaryRecord> view = new List<EvAncillaryRecord>( );

      // 
      // Define the query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( PARM_TrialId, SqlDbType.NVarChar, 10),
        new SqlParameter( PARM_SubjectId, SqlDbType.NVarChar, 20),
        new SqlParameter( PARM_State, SqlDbType.VarChar, 20),
      };
      cmdParms [ 0 ].Value = ProjectId;
      cmdParms [ 1 ].Value = SubjectId;
      cmdParms [ 2 ].Value = State;

      // 
      // Generate the SQL query string.
      // 
      _sqlQueryString = _sqlQuery_View + " WHERE ( ( TrialId = @TrialId ) ";

      if ( SubjectId.Length > 0 )
      {
        _sqlQueryString += " AND ( SubjectId = @SubjectId ) ";
      }
      if ( State.Length > 0 )
      {
        _sqlQueryString += " AND (TSR_State = @State) ";
      }

      if ( OrderBy.Length == 0 )
      {
        _sqlQueryString += ") ORDER BY RecordId";
      }
      else
      {
        _sqlQueryString += ") ORDER BY " + OrderBy;
      }

      this.LogDebug ( _sqlQueryString );

      //
      // Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery( _sqlQueryString, cmdParms ) )
      {
        // 
        // Iterate through the results extracting the role information.
        // 
        for ( int count = 0; count < table.Rows.Count; count++ )
        {
          // 
          // Extract the table row.
          // 
          DataRow row = table.Rows [ count ];

          EvAncillaryRecord record = this.readRowData( row );

          view.Add( record );

        }//END count interation

      }//END Using statement

      // 
      // Return a list containing an EvSubjectRecord object.
      // 
      return view;

    }//END getView method.

    // =====================================================================================
    /// <summary>
    /// This method returns a list of Subject Record object based on TrialId, SubjectId and useGuid condition
    /// </summary>
    /// <param name="ProjectId">String: A Project identifier</param>
    /// <param name="EntityId">String: A Subject identifier</param>
    /// <param name="useGuid">Boolean: true, if the Guid is used</param>
    /// <returns>List of EvOption: A list containing an Option object.</returns>
    /// <remarks>
    /// This method consists of following steps:
    /// 
    /// 1. Define the sql query parameters and sql query string. 
    /// 
    /// 2. Execute the sql query string and store the results on datatable. 
    /// 
    /// 3. Loop through the table and extract the data row on Option object. 
    /// 
    /// 4. Add the Option object's values to the Options list. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public List<EvOption> getList( string EntityId, bool useGuid )
    {
      this.LogMethod ( "getList, " );
      this.LogValue ( "SubjectId: " + EntityId );
      // 
      // Define the local variables and objects.
      // 
      List<EvOption> list = new List<EvOption>( );

      EvOption option = new EvOption( );

      //
      // If useGuid is equal to true, add option object to the list.
      //
      if ( useGuid == true )
      {
        option = new EvOption( Guid.Empty.ToString( ), String.Empty );
      }
      list.Add( option );

      // 
      // Define the query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(PARM_SubjectId, SqlDbType.NVarChar, 20),
      };
      cmdParms [ 0 ].Value =  EntityId;

      // 
      // Generate the SQL query string.
      // 
      _sqlQueryString = _sqlQuery_View + " WHERE ( TrialId = @TrialId ) ";

      if ( EntityId.Length > 0 )
      {
        _sqlQueryString += " AND ( SubjectId = @SubjectId ) ";
      }
      _sqlQueryString += " ORDER BY RecordId";

      //_Status = "\r\n" + sqlQueryString;

      //
      //Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery( _sqlQueryString, cmdParms ) )
      {
        // 
        // Iterate through the table rows count list.
        // 
        for ( int count = 0; count < table.Rows.Count; count++ )
        {
          // 
          // Extract the table row.
          // 
          DataRow row = table.Rows [ count ];

          // 
          //  Process the query result.
          // 
          if ( useGuid == true )
          {
            option = new EvOption(
              EvSqlMethods.getString( row, "TSR_Guid" ),
              EvSqlMethods.getString( row, "RecordId" ) + " - " + EvSqlMethods.getDateTime( row, "TSR_Subject" ) );
          }
          else
          {
            option = new EvOption(
              EvSqlMethods.getString( row, "RecordId" ),
              EvSqlMethods.getString( row, "RecordId" ) + " - " + EvSqlMethods.getDateTime( row, "TSR_Subject" ) );
          }

          if ( EntityId == String.Empty )
          {
            option.Description = "(" + EvSqlMethods.getString( row, "SubjectId" ) + ") " + option.Description;
          }

          // 
          // Append the EvOption object to the list.
          // 
          list.Add( option );
        }
      }

      // 
      // Return a list containing an EvOption object.
      // 
      return list;

    }//END getList method.

    #endregion

    #region Retrieval Queries

    // =====================================================================================
    /// <summary>
    /// This method retrieves the SubjectRecord data table based on Record's Guid
    /// </summary>
    /// <param name="RecordGuid">Guid: A Subject Record's Global Unique identifier</param>
    /// <returns>EvSubjectRecord: A Subject data object</returns>
    /// <remarks>
    /// This method consists of following steps:
    /// 
    /// 1. Return an empty SubjectRecord object, if the Guid is empty. 
    /// 
    /// 2. Define the sql query parameters and sql query string. 
    /// 
    /// 3. Execute the sql query string and store the results on datatable. 
    /// 
    /// 4. Return an empty SubjectRecord object, if the table has no value. 
    /// 
    /// 5. Else, extract the first data row to the SubjectRecord object. 
    /// 
    /// 6. Return the SubjectRecord object. 
    /// </remarks>
    //  ------------------------------------------------------------------------------------
    public EvAncillaryRecord getRecord( Guid RecordGuid )
    {
      this.LogMethod ( "getRecord");
      this.LogValue( "RecordGuid: " + RecordGuid.ToString ( ) );
      // 
      // Define the local variables.
      // 
      EvAncillaryRecord record = new EvAncillaryRecord( );

      // 
      // Validate whether the Subject Record object's Guid is not empty.
      // 
      if ( RecordGuid == Guid.Empty )
      {
        return record;
      }

      // 
      // Set the query parameter values.
      // 
      SqlParameter cmdParms = new SqlParameter( PARM_Guid, SqlDbType.UniqueIdentifier );
      cmdParms.Value = RecordGuid;

      // 
      // Generate the SQL query string
      // 
      _sqlQueryString = _sqlQuery_View + " WHERE (TSR_Guid = @Guid) ;";

      //
      // Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery( _sqlQueryString, cmdParms ) )
      {
        // 
        // If no rows found, return an EvsubjectRecord object.
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
        // Fill the EvsubjectRecord object.
        // 
        record = this.readRowData( row );

      }//END Using statement

      // 
      // Return an object containing an EvsubjectRecord object.
      // 
      return record;

    }//END getRecord method

    // =====================================================================================
    /// <summary>
    /// This method retrieves the SubjectRecord data table based on RecordId
    /// </summary>
    /// <param name="RecordId">string: A Record identifier</param>
    /// <returns>An object containing an EvSubjectRecord object.</returns>
    /// <returns>EvSubjectRecord: A Subject data object</returns>
    /// <remarks>
    /// This method consists of following steps:
    /// 
    /// 1. Return an empty SubjectRecord object, if the RecordId is empty. 
    /// 
    /// 2. Define the sql query parameters and sql query string. 
    /// 
    /// 3. Execute the sql query string and store the results on datatable. 
    /// 
    /// 4. Return an empty SubjectRecord object, if the table has no value. 
    /// 
    /// 5. Else, extract the first data row to the SubjectRecord object. 
    /// 
    /// 6. Return the SubjectRecord object. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvAncillaryRecord getRecord( string RecordId )
    {
      this.LogMethod ( "getRecord" );
      this.LogValue ( " RecordId: " + RecordId );
      // 
      // Define the object.
      // 
      EvAncillaryRecord record = new EvAncillaryRecord( );

      // 
      // If Record does not exist, return an object containing EvSubjectRecord object.
      // 
      if ( RecordId == String.Empty )
      {
        return record;
      }

      // 
      // Define the parameters for the query
      // 
      SqlParameter cmdParms = new SqlParameter( PARM_RecordId, SqlDbType.NVarChar, 17 );
      cmdParms.Value = RecordId;

      _sqlQueryString = _sqlQuery_View + " WHERE ( RecordId = @RecordId );";

      //
      // Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery( _sqlQueryString, cmdParms ) )
      {
        // 
        // If not rows the return, an object containing EvSubjectRecord object.
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
        // Fill the EvSubjectRecord object.
        // 
        record = this.readRowData( row );

      }//END Using statement

      // 
      // Return an object containing an EvSubjectRecord object.
      // 
      return record;

    }//END getRecord method

    // =====================================================================================
    /// <summary>
    /// This method retrieves the SubjectRecord data table based on RecordId and state value.
    /// </summary>
    /// <param name="RecordId">string: A Record identifier</param>
    /// <param name="State">string: a milestone record's state</param>
    /// <returns>An object containing an EvSubjectRecord object.</returns>
    /// <returns>EvSubjectRecord: A Subject data object</returns>
    /// <remarks>
    /// This method consists of following steps:
    /// 
    /// 1. Return an empty SubjectRecord object, if the RecordId is empty. 
    /// 
    /// 2. Define the sql query parameters and sql query string. 
    /// 
    /// 3. Execute the sql query string and store the results on datatable. 
    /// 
    /// 4. Return an empty SubjectRecord object, if the table has no value. 
    /// 
    /// 5. Else, extract the first data row to the SubjectRecord object. 
    /// 
    /// 6. Return the SubjectRecord object. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvAncillaryRecord getRecord( string RecordId, string State )
    {
      this.LogMethod ( "getRecord" );
      this.LogValue ( "RecordId: " + RecordId );
      this.LogValue ( "State: " + State );
      // 
      // Define the local variables.
      // 
      EvAncillaryRecord record = new EvAncillaryRecord( );

      // 
      // If Record does not exist, return an object containing EvSubjectRecord object.
      // 
      if ( RecordId == String.Empty )
      {
        return record;
      }

      // 
      // Define the parameters for the query.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(PARM_RecordId, SqlDbType.NVarChar, 20),
        new SqlParameter(PARM_State, SqlDbType.Char, 5),
      };
      cmdParms [ 0 ].Value = RecordId;
      cmdParms [ 1 ].Value = State;

      _sqlQueryString = _sqlQuery_View + " WHERE ( RecordId = @RecordId )"
        + " AND ( TSR_State = @State );";

      //
      // Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery( _sqlQueryString, cmdParms ) )
      {
        // 
        // If no rows found, return an object containing an EvSubjectRecord object.
        // 
        if ( table.Rows.Count == 0 )
        {
          return record;
        }

        // 
        // Extract the table row.
        // 
        DataRow row = table.Rows [ 0 ];

        // 
        // Fill the EvSubjectRecord object.
        // 
        record = this.readRowData( row );

      }//END Using statement

      // 
      // Return an object containing an EvSubjectRecord object.
      // 
      return record;

    }//END getRecord method

    #endregion

    #region Ancilliary Record Update queries

    // =====================================================================================
    /// <summary>
    /// This method adds items to the Subject Record data table. 
    /// </summary>
    /// <param name="AncillaryRecord">EvSubjectRecord: A Subject Record object</param>
    /// <returns>EvEventCodes: An event code for adding items</returns>
    /// <remarks>
    /// This method consists of follwoing steps: 
    /// 
    /// 1. Create new DB row Guid, if it is empty. 
    /// 
    /// 2. Define the sql query parameters and execute the storeprocedure for adding items. 
    /// 
    /// 3. Exit, if the storeprocedure runs fail. 
    /// 
    /// 4. Return an event code for adding items. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvEventCodes addItem( EvAncillaryRecord AncillaryRecord )
    {
      this.LogMethod ( "addItem." );
      this.LogValue ( "RecordId: " + AncillaryRecord.RecordId );
      this.LogValue ( "ProjectId: " + AncillaryRecord.ProjectId );
      this.LogValue ( "SubjectId: " + AncillaryRecord.SubjectId );

      // 
      // Initialise the methods variables and objects.
      // 
      EvDataChanges dataChanges = new EvDataChanges( );
      Guid newGuid = Guid.NewGuid( );
      AncillaryRecord.RecordId = String.Empty;

      // 
      // If the SubjectRecord Guid is empty then create a new Guid.
      // 
      if ( AncillaryRecord.Guid == Guid.Empty )
      {
        AncillaryRecord.Guid = newGuid;
      }

      // 
      // Compare the objects.
      // 
      EvDataChange dataChange = SetDataChange( AncillaryRecord );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] _cmdParms = GetParameters( );
      SetParameters( _cmdParms, AncillaryRecord );

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate( _STORED_PROCEDURE_AddItem, _cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      // 
      // Return an enumerated value EventCode status.
      // 
      return EvEventCodes.Ok;

    } //END addItem method.

    // =====================================================================================
    /// <summary>
    /// This method updates items to Subject Record data table. 
    /// </summary>
    /// <param name="AncillaryRecord">EvAncillaryRecord: An ancillary record object object.</param>
    /// <returns>EvEventCodes: An event code for updating items</returns>
    /// <remarks>
    /// This method consists of following steps: 
    /// 
    /// 1. Exit, if the VisitId or SubjectId is empty.
    /// 
    /// 2. Add items to datachange object, if they do not exist on the old milestone record object. 
    /// 
    /// 3. Define the sql query parameters and execute the storeprocedure for updating items. 
    /// 
    /// 4. Exit, if the storeprocedure runs fail. 
    /// 
    /// 5. Add the datachange object's values to the backup datachanges object. 
    /// 
    /// 6. Return an event code for updating items. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvEventCodes updateItem( EvAncillaryRecord AncillaryRecord )
    {
      this.LogMethod ( "updateItem." );
      this.LogValue ( "RecordId: " + AncillaryRecord.RecordId );
      this.LogValue ( "ProjectId: " + AncillaryRecord.ProjectId );
      this.LogValue ( "SubjectId: " + AncillaryRecord.SubjectId );
      this.LogValue ( "State: " + AncillaryRecord.State );
      // 
      // Initialise the methods variables and objects.
      // 
      EvDataChanges dataChanges = new EvDataChanges( );

      // 
      // Get an existing object to verify the record exists.
      // 
      EvAncillaryRecord oldItem = getRecord( AncillaryRecord.Guid );
      if ( oldItem.Guid == Guid.Empty )
      {
        return EvEventCodes.Data_InvalidId_Error;
      }

      // 
      // Compare the objects.
      // 
      EvDataChange dataChange = SetDataChange( AncillaryRecord );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] _cmdParms = GetParameters( );
      SetParameters( _cmdParms, AncillaryRecord );

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate( _STORED_PROCEDURE_UpdateItem, _cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      // 
      // Add the data change to the database.
      // 
      dataChanges.AddItem( dataChange );

      // 
      // Return an enumerated value EventCode status.
      // 
      return EvEventCodes.Ok;

    }//END updateItem method.

    // =====================================================================================
    /// <summary>
    ///  This method sets the data change values for the object and returns the data change
    ///  object.
    /// </summary>
    /// <param name="AncillaryRecord">EvSubjectRecord: A Subject Record object.</param>
    /// <returns>EvDataChange: A DataChange object.</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Add items to datachange object if they do not exist on the old Subject Record object. 
    /// 
    /// 2. Return the datachange object. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private EvDataChange SetDataChange( EvAncillaryRecord AncillaryRecord )
    {
      // 
      // Initialise the methods variables and objects.
      // 
      EvDataChange dataChange = new EvDataChange( );

      // 
      // Retrieve the existing record object for comparision
      // 
      EvAncillaryRecord oldRecord = this.getRecord( AncillaryRecord.Guid );

      // 
      // Set the chanage objects values.
      // 
      dataChange.TableName = EvDataChange.DataChangeTableNames.EvAncilliaryRecords;
      dataChange.RecordGuid = AncillaryRecord.Guid;
      dataChange.TrialId = AncillaryRecord.ProjectId;
      dataChange.SubjectId = AncillaryRecord.SubjectId;
      dataChange.RecordId = AncillaryRecord.RecordId;
      dataChange.UserId = AncillaryRecord.UpdatedByUserId;
      dataChange.DateStamp = DateTime.Now;
      dataChange.RecordGuid = AncillaryRecord.Guid;

      //
      // Add items to datachange object, if they do not exist on the old SubjectRecord object. 
      //
      if ( AncillaryRecord.ProjectId != oldRecord.ProjectId )
      {
        dataChange.AddItem( "TrialId", oldRecord.ProjectId, AncillaryRecord.ProjectId );
      }
      if ( AncillaryRecord.SubjectId != oldRecord.SubjectId )
      {
        dataChange.AddItem( "SubjectId", oldRecord.SubjectId, AncillaryRecord.SubjectId );
      }
      if ( AncillaryRecord.RecordId != oldRecord.RecordId )
      {
        dataChange.AddItem( "RecordId", oldRecord.RecordId, AncillaryRecord.RecordId );
      }
      if ( AncillaryRecord.Subject != oldRecord.Subject )
      {
        dataChange.AddItem( "Subject", oldRecord.Subject, AncillaryRecord.Subject );
      }
      if ( AncillaryRecord.Record != oldRecord.Record )
      {
        dataChange.AddItem( "Record", oldRecord.Record, AncillaryRecord.Record );
      }
      if ( AncillaryRecord.BinaryLength != oldRecord.BinaryLength )
      {
        dataChange.AddItem( "BinaryLength", oldRecord.BinaryLength.ToString( ), AncillaryRecord.BinaryLength.ToString( ) );
      }
      if ( AncillaryRecord.BinaryType != oldRecord.BinaryType )
      {
        dataChange.AddItem( "BinaryType", oldRecord.BinaryType, AncillaryRecord.BinaryType );
      }
      if ( AncillaryRecord.Researcher != oldRecord.Researcher )
      {
        dataChange.AddItem( "Researcher", oldRecord.Researcher, AncillaryRecord.Researcher );
      }
      if ( AncillaryRecord.ResearcherUserId != oldRecord.ResearcherUserId )
      {
        dataChange.AddItem( "ResearcherUserId", oldRecord.ResearcherUserId, AncillaryRecord.ResearcherUserId );
      }
      if ( AncillaryRecord.ResearcherDate != oldRecord.ResearcherDate )
      {
        dataChange.AddItem( "ResearcherDate", oldRecord.ResearcherDate.ToString( "yyyy MMM dd HH:mm:ss" ),
          AncillaryRecord.ResearcherDate.ToString( "yyyy MMM dd HH:mm:ss" ) );
      }
      if ( AncillaryRecord.Reviewer != oldRecord.Reviewer )
      {
        dataChange.AddItem( "Reviewer", oldRecord.Reviewer, AncillaryRecord.Reviewer );
      }
      if ( AncillaryRecord.ReviewerUserId != oldRecord.ReviewerUserId )
      {
        dataChange.AddItem( "ReviewerUserId", oldRecord.ReviewerUserId, AncillaryRecord.ReviewerUserId );
      }
      if ( AncillaryRecord.ReviewDate != oldRecord.ReviewDate )
      {
        dataChange.AddItem( "ReviewDate", oldRecord.ReviewDate.ToString( "yyyy MMM dd HH:mm:ss" ),
          AncillaryRecord.ReviewDate.ToString( "yyyy MMM dd HH:mm:ss" ) );
      }
      if ( AncillaryRecord.Approver != oldRecord.Approver )
      {
        dataChange.AddItem( "Approver", oldRecord.Approver, AncillaryRecord.Approver );
      }
      if ( AncillaryRecord.ApproverUserId != oldRecord.ApproverUserId )
      {
        dataChange.AddItem( "ApproverUserId", oldRecord.ApproverUserId, AncillaryRecord.ApproverUserId );
      }
      if ( AncillaryRecord.ApprovalDate != oldRecord.ApprovalDate )
      {
        dataChange.AddItem( "ApprovalDate", oldRecord.ApprovalDate.ToString( "yyyy MMM dd HH:mm:ss" ),
          AncillaryRecord.ApprovalDate.ToString( "yyyy MMM dd HH:mm:ss" ) );
      }

      // 
      // Return the data change object containing an EvDataChange object.
      // 
      return dataChange;

    }//END SetDataChange method

    #endregion

    #region Lock queries

    // =====================================================================================
    /// <summary>
    /// This method locks the EvSubjectRecord for single user update.
    /// </summary>
    /// <param name="Item">EvAncillaryRecord: An Ancillary record object.</param>
    /// <returns>EvEventCodes: An event code for locking items</returns>
    /// <remarks>
    /// This method consists of following steps.
    /// 
    /// 1. Exit, if the item's Uid is not defined. 
    /// 
    /// 2. Define the sql query parameters and execute the storeprocedure for locking items. 
    /// 
    /// 3. Exit, if the storeprocedure runs fail. 
    /// 
    /// 4. Return an event code for locking items. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvEventCodes LockItem( EvAncillaryRecord Item )
    {
      // 
      // Initialise the method variables
      // 
      this.LogMethod ( "lockItem method " );
      this.LogValue ( "Guid: " + Item.Guid );
      this.LogValue ( "UserCommonName: " + Item.UserCommonName );
      int RecordsUpdated = 0;

      // 
      // Validate whether the Guid is defined. 
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
        new SqlParameter(PARM_Guid, SqlDbType.UniqueIdentifier),
        new SqlParameter(PARM_UpdatedByUserId, SqlDbType.NVarChar,100),
        new SqlParameter(PARM_UpdatedBy, SqlDbType.NVarChar, 100),
        new SqlParameter(PARM_UpdateDate, SqlDbType.DateTime),
      };
      cmdParms [ 0 ].Value = Item.Guid;
      cmdParms [ 1 ].Value = Item.UpdatedByUserId;
      cmdParms [ 2 ].Value = Item.UserCommonName;
      cmdParms [ 3 ].Value = DateTime.Now;

      //
      // Execute the update command.
      //
      if ( ( RecordsUpdated = EvSqlMethods.StoreProcUpdate( _STORED_PROCEDURE_LockItem, cmdParms ) ) == 0 )
      {
        this.LogDebug ( " RecordsUpdated " + RecordsUpdated + " " );
        return EvEventCodes.Database_Record_Update_Error;
      }
      this.LogDebug ( " RecordsUpdated " + RecordsUpdated + " " );

      // 
      // Return an enumerated value EventCode status.
      // 
      return EvEventCodes.Ok;

    } //END lockItem method.

    // =====================================================================================
    /// <summary>
    /// This method unlocks the EvSubjectRecord object.
    /// </summary>
    /// <param name="Item">EvAncillaryRecord: An ancillary record object object.</param>
    /// <returns>EvEventCodes: An event code for locking items</returns>
    /// <remarks>
    /// This method consists of following steps.
    /// 
    /// 1. Exit, if the item's Uid is not defined. 
    /// 
    /// 2. Define the sql query parameters and execute the storeprocedure for unlocking items. 
    /// 
    /// 3. Exit, if the storeprocedure runs fail. 
    /// 
    /// 4. Return an event code for unlocking items. 
    /// </remarks>

    //  ----------------------------------------------------------------------------------
    public EvEventCodes UnlockItem( EvAncillaryRecord Item )
    {
      // 
      // Initialise the method variables
      // 
      this.LogMethod ( "unlockItem method " );
      this.LogValue ( "Guid: " + Item.Guid );
      this.LogValue ( "UserCommonName: " + Item.UserCommonName );
      int RecordsUpdated = 0;

      // 
      // Validate whether the Guid is defined. 
      // 
      if ( Item.Guid == Guid.Empty)
      {
        return EvEventCodes.Identifier_Global_Unique_Identifier_Error;
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(PARM_Guid, SqlDbType.UniqueIdentifier),
        new SqlParameter(PARM_UpdatedByUserId, SqlDbType.NVarChar,100),
        new SqlParameter(PARM_UpdatedBy, SqlDbType.NVarChar, 100),
        new SqlParameter(PARM_UpdateDate, SqlDbType.DateTime),
      };
      cmdParms [ 0 ].Value = Item.Guid;
      cmdParms [ 1 ].Value = Item.UpdatedByUserId;
      cmdParms [ 2 ].Value = Item.UserCommonName;
      cmdParms [ 3 ].Value = DateTime.Now;

      //
      // Execute the update command.
      //
      if ( ( RecordsUpdated = EvSqlMethods.StoreProcUpdate( _STORED_PROCEDURE_UnlockItem, cmdParms ) ) == 0 )
      {
        this.LogDebug ( " RecordsUpdated " + RecordsUpdated + " " );
        return EvEventCodes.Database_Record_Update_Error;
      }
      this.LogDebug ( " RecordsUpdated " + RecordsUpdated + " " );

      // 
      // Return an enumerated value EventCode status.
      // 
      return EvEventCodes.Ok;

    }//END UnlockItem method.

    #endregion

  }//END EvSubjectRecords class

}//END namespace Evado.Digital.Dal
