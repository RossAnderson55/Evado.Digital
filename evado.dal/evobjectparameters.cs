/***************************************************************************************
 * <copyright file="dal\EvActivityForms.cs" company="EVADO HOLDING PTY. LTD.">
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


namespace Evado.Dal
{
  /// <summary>
  /// A business Component used to manage Ethics roles
  /// The Evado.Model.TrialVisitForm is used in most methods 
  /// and is used to store serializable information about an account
  /// </summary>
  public class EvObjectParameters : EvDalBase
  {
    #region Class Initialization

    /// <summary>
    /// This is the class initialisation method.
    /// </summary>
    public EvObjectParameters ( )
    {
      this.ClassNameSpace = "Evado.Dal.Digital.EvObjectParameters.";
    }

    /// <summary>
    /// This is the class initialisation method with settings configured.
    /// </summary>
    public EvObjectParameters ( EvClassParameters Settings )
    {
      this.ClassParameters = Settings;

      this.ClassNameSpace = "Evado.Dal.Digital.EvObjectParameters.";
    }

    #endregion

    #region Class Constant

    /// <summary>
    /// This constant defines a sql query for a view
    /// </summary>
    private const string _sqlQuery_View = "Select * FROM EV_OBJECT_PARAMETERS ";

    private const string PARM_OBJECT_GUID = "@GUID";
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
    private EvObjectParameter readDataRow ( DataRow Row )
    {
      // 
      // Initialise application parameter object
      // 
      EvObjectParameter parameter = new EvObjectParameter ( );

      // 
      // Extract the data object values from the data row object and add to the activity form object.
      // 
      parameter.Guid = EvSqlMethods.getGuid ( Row, "OBJ_GUID" );
      parameter.Order = EvSqlMethods.getInteger ( Row, "OBP_ORDER" );
      parameter.Name = EvSqlMethods.getString ( Row, "OBP_NAME" );
      parameter.DataType = EvStatics.parseEnumValue<EvDataTypes> ( EvSqlMethods.getString ( Row, "OBP_TYPE" ) );
      parameter.Value = EvSqlMethods.getString ( Row, "OBP_VALUE" );
      parameter.Options = EvSqlMethods.getString ( Row, "OBP_OPTIONS" );

      // 
      // Return the application parameter object.
      // 
      return parameter;

    }// End readDataRow method.

    #endregion

    #region List and queries methods

    // ==================================================================================
    /// <summary>
    /// This method returns the list of activity records based on the activityGuid and OrderBy value. 
    /// </summary>
    /// <param name="ObjectGuid">Guid: (Mandatory) The selection organistion's identifier</param>
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
    public List<EvObjectParameter> getParameterList ( Guid ObjectGuid )
    {
      this.LogMethod ( "getParameterList method. " );
      this.LogValue ( "ObjectGuid: " + ObjectGuid );
      //
      // Initialize the method status string, an sql query string and a return list of activity records
      //
      string sqlQueryString;
      List<EvObjectParameter> parameterList = new List<EvObjectParameter> ( );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( PARM_OBJECT_GUID, SqlDbType.UniqueIdentifier),
      };
      cmdParms [ 0 ].Value = ObjectGuid;

      // 
      // Generate the SQL query string
      // 
      sqlQueryString = _sqlQuery_View + "WHERE OBJ_GUID = " + PARM_OBJECT_GUID
        + " ORDER BY OBP_ORDER; ";

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

          EvObjectParameter parameter = this.readDataRow ( row );


          this.LogValue ( "parameter: " + parameter.Name + ", Selected: " + parameter.Value );

          parameterList.Add ( parameter );

        } //END interation loop.

      }//END using method

      // 
      // Update the numbering
      // 
      for ( int count = 0; count < parameterList.Count; count++ )
      {
        ( (EvObjectParameter) parameterList [ count ] ).Order = count * 5 + 5;
      }

      this.LogValue ( "view count: " + parameterList.Count );

      // 
      // Return the list containing the User data object.
      // 
      return parameterList;

    }//END getView method.

    #endregion

    #region Update methods

    // ==================================================================================
    /// <summary>
    /// This class updates the appliocation parameter records data object. 
    /// </summary>
    /// <param name="ParameterList">list of EvObjectParameter</param>
    /// <param name="ObjectGuid">parameter's Guid identifier.</param>
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
    public EvEventCodes updateItems ( List<EvObjectParameter> ParameterList, Guid ObjectGuid )
    {

      this.LogMethod ( "updateItems " );
      this.LogValue ( "ObjectGuid: " + ObjectGuid );
      this.LogValue ( "ParameterList count: " + ParameterList.Count );
      //
      // Initialize the Sql update query string. 
      //
      System.Text.StringBuilder SqlUpdateQuery = new System.Text.StringBuilder ( );

      if ( ParameterList.Count == 0 )
      {
        this.LogValue ( "No parameters in the list" );

        this.LogMethodEnd ( "updateItems " );
        return EvEventCodes.Ok;
      }

      //
      // Delete the milestone activities for this milestone.
      //
      SqlUpdateQuery.AppendLine ( "/** DELETE ALL OF OBJECT PARAMETERS FOR THE OBJECT **/" );
      SqlUpdateQuery.AppendLine ( " DELETE FROM EV_OBJECT_PARAMETERS " );
      SqlUpdateQuery.AppendLine ( " WHERE  (OBJ_GUID = '" + ObjectGuid + "') ; \r\n" );

      for ( int count = 0; count < ParameterList.Count; count++ )
      {
        EvObjectParameter objectParameter = ParameterList [ count ];
        //
        // Skip the non selected forms
        //
        if ( objectParameter == null )
        {
          continue;
        }

        objectParameter.Order = count + 1;

        //this.LogDebug ( "Name: {0}, Value: {1} >> ADDED", objectParameter.Name, objectParameter.Value );

        objectParameter.Guid = ObjectGuid;

        SqlUpdateQuery.AppendLine ( "Insert Into EV_OBJECT_PARAMETERS " );
        SqlUpdateQuery.AppendLine ( "( OBJ_GUID, OBP_ORDER, OBP_NAME, OBP_TYPE, OBP_VALUE, OBP_OPTIONS )  " );
        SqlUpdateQuery.AppendLine ( "values  " );
        SqlUpdateQuery.AppendLine ( "('" + objectParameter.Guid + "', " );
        SqlUpdateQuery.AppendLine ( " " + objectParameter.Order + ", " );
        SqlUpdateQuery.AppendLine ( "'" + objectParameter.Name + "', " );
        SqlUpdateQuery.AppendLine ( "'" + objectParameter.DataType.ToString ( ) + "', " );
        SqlUpdateQuery.AppendLine ( "'" + objectParameter.Value + "', " );
        SqlUpdateQuery.AppendLine ( " '" + objectParameter.Options + "' ); \r\n" );

      }//END form list iteration loop.

      //this.LogDebug ( "Sql Query: " + SqlUpdateQuery.ToString ( ) );

      if ( EvSqlMethods.QueryUpdate ( SqlUpdateQuery.ToString ( ), null ) == 0 )
      {
        this.LogValue ( "Update failed" );
        this.LogMethodEnd ( "updateItems " );
        return EvEventCodes.Database_Record_Update_Error;
      }

      this.LogValue ( "Update completed" );
      // 
      // Return code
      //       
      this.LogMethodEnd ( "updateItems " );
      return EvEventCodes.Ok;

    }//END updateItem class

    #endregion

  }//END EvActivityForms class

}//END namespace Evado.Dal.Digital
