/***************************************************************************************
 * <copyright file="dal\EvOrganisations.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2021 EVADO HOLDING PTY. LTD.  All rights reserved.
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


namespace Evado.Dal.Digital
{
  /// <summary>
  /// This class is handles the data access layer for the organisation data object.
  /// </summary>
  public class EdOrganisations : EvDalBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EdOrganisations ( )
    {
      this.ClassNameSpace = "Evado.Dal.Digital.EvOrganisations.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EdOrganisations ( EvClassParameters Settings )
    {
      this.ClassParameters = Settings;
      this.ClassNameSpace = "Evado.Dal.Digital.EvOrganisations.";

      this.LogDebug ( "ApplicationGuid: " + this.ClassParameters.AdapterGuid );

    }
    #endregion

    #region Initialise Class variables and parameters
    //
    // Static constants
    //
    private const string SQL_SELECT_QUERY = "Select * FROM ED_ORGANISATION_VIEW ";

    private const string STORED_PROCEDURE_ADD_ITEM = "USR_ORGANISATION_ADD";
    private const string STORED_PROCEDURE_DELETE_ITEM = "USR_ORGANISATION_DELETE";
    private const string STORED_PROCEDURE_UPDATE_ITEM = "USR_ORGANISATION_UPDATE";

    /// <summary>
    /// This constant defines the database GUID field (Primary Key).
    /// </summary>
    public const string DB_GUID = "O_GUID";

    /// <summary>
    /// This constant defines the database organisation identifier field .
    /// </summary>
    public const string DB_ORG_ID = "ORG_ID";

    /// <summary>
    /// This constant defines the database organisation name field .
    /// </summary>
    public const string DB_NAME = "O_NAME";
    public const string DB_ORG_TYPE = "O_ORG_TYPE";

    /// <summary>
    /// This constant defines the database organisation street address 1 field .
    /// </summary>
    public const string DB_ADDRESS_1 = "O_ADDRESS_1";

    /// <summary>
    /// This constant defines the database organisation street address 2 field .
    /// </summary>
    public const string DB_ADDRESS_2 = "O_ADDRESS_2";

    /// <summary>
    /// This constant defines the database organisation city field .
    /// </summary>
    public const string DB_ADDRESS_CITY = "O_ADDRESS_CITY";

    /// <summary>
    /// This constant defines the database organisation post code field .
    /// </summary>
    public const string DB_ADDRESS_POST_CODE = "O_ADDRESS_POST_CODE";

    /// <summary>
    /// This constant defines the database organisation state field .
    /// </summary>
    public const string DB_ADDRESS_STATE = "O_ADDRESS_STATE";

    /// <summary>
    /// This constant defines the database country field .
    /// </summary>
    public const string DB_COUNTRY = "O_COUNTRY";

    /// <summary>
    /// This constant defines the database telephone field .
    /// </summary>
    public const string DB_TELEPHONE = "O_TELEPHONE";

    /// <summary>
    /// This constant defines the email address field .
    /// </summary>
    public const string DB_EMAIL_ADDRESS = "O_EMAIL_ADDRESS";

    /// <summary>
    /// This constant defines the organisation identifie field .
    /// </summary>
    public const string DB_IMAGE_FILENAME = "O_IMAGE_FILENAME";

    /// <summary>
    /// This constant defines the the userId who performed the last update field .
    /// </summary>
    public const string DB_UPDATED_BY_USER_ID = "O_UPDATED_BY_USER_ID";

    /// <summary>
    /// This constant defines the user who performed the last update field .
    /// </summary>
    public const string DB_UPDATED_By = "O_UPDATED_BY";

    /// <summary>
    /// This constant defines the last update date field .
    /// </summary>
    public const string DB_UPDATE_DATE = "O_UPDATED_DATE";

    /// <summary>
    /// This constant defines the deleted flag field .
    /// </summary>
    public const string DB_DELETED = "O_DELETED";

    private const string PARM_Guid = "@Guid";
    private const string PARM_ORG_ID = "@ORG_ID";
    private const string PARM_Name = "@Name";
    private const string PARM_ORG_TYPE = "@ORG_TYPE";
    private const string PARM_Address_1 = "@ADDRESS_1";
    private const string PARM_Address_2 = "@ADDRESS_2";
    /// <summary>
    /// This constant defines the organisation city parameter.
    /// </summary>
    public const string PARM_Address_City = "@ADDRESS_CITY";
    private const string PARM_Address_Post_Code = "@ADDRESS_POST_CODE";
    private const string PARM_Address_State = "@ADDRESS_STATE";
    /// <summary>
    /// This constant defines the organisation country parameter field.
    /// </summary>
    public const string PARM_COUNTRY = "@COUNTRY";
    private const string PARM_TELEPHONE = "@TELEPHONE";
    private const string PARM_EmailAddress = "@EMAIL_ADDRESS";
    private const string PARM_IMAGE_FILENAME = "@IMAGE_FILENAME";
    private const string PARM_UPDATED_BY_USER_ID = "@UPDATED_BY_USER_ID";
    private const string PARM_UPDATED_BY = "@UPDATED_BY";
    private const string PARM_UPDATE_DATE = "@UPDATED_DATE";

    private string sqlQueryString = String.Empty;

    #endregion

    #region Query Parameter section

    // =====================================================================================
    /// <summary>
    /// This method defines the sql query parameter arraylist. 
    /// </summary>
    /// <returns>SqlParameter: a arraylist of sql query parameters</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Create an arraylist of sql query parameters. 
    /// 
    /// 2. Return the arraylist of sql query parameters.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private static SqlParameter [ ] GetParameters ( )
    {
      SqlParameter [ ] parms = new SqlParameter [ ] 
      {
        new SqlParameter( EdOrganisations.PARM_Guid, SqlDbType.UniqueIdentifier),
        new SqlParameter( EdOrganisations.PARM_ORG_ID, SqlDbType.NVarChar, 10),
        new SqlParameter( EdOrganisations.PARM_Name, SqlDbType.NVarChar, 50),
        new SqlParameter( EdOrganisations.PARM_Address_1, SqlDbType.NVarChar, 50),
        new SqlParameter( EdOrganisations.PARM_Address_2, SqlDbType.NVarChar, 50),
        new SqlParameter( EdOrganisations.PARM_Address_City, SqlDbType.NVarChar, 50),
        new SqlParameter( EdOrganisations.PARM_Address_Post_Code, SqlDbType.NVarChar, 10),
        new SqlParameter( EdOrganisations.PARM_Address_State, SqlDbType.NVarChar, 50),
        new SqlParameter( EdOrganisations.PARM_COUNTRY, SqlDbType.NVarChar, 50),
        new SqlParameter( EdOrganisations.PARM_TELEPHONE, SqlDbType.NVarChar, 15),
        new SqlParameter( EdOrganisations.PARM_EmailAddress, SqlDbType.NVarChar, 100),
        new SqlParameter( EdOrganisations.PARM_ORG_TYPE, SqlDbType.NVarChar, 50),
        new SqlParameter( EdOrganisations.PARM_IMAGE_FILENAME, SqlDbType.NVarChar, 100),
        new SqlParameter( EdOrganisations.PARM_UPDATED_BY_USER_ID, SqlDbType.NVarChar, 100),
        new SqlParameter( EdOrganisations.PARM_UPDATED_BY, SqlDbType.NVarChar, 100),
        new SqlParameter( EdOrganisations.PARM_UPDATE_DATE, SqlDbType.DateTime)
      };

      return parms;
    }

    // =====================================================================================
    /// <summary>
    /// This class binds values to the sql query parameters arraylist. 
    /// </summary>
    /// <param name="cmdParms">SqlParameter: an arraylist of Database parameters</param>
    /// <param name="Organisation">EvOrganisation: Organisation data object.</param>
    /// <remarks>
    /// This method consists of the following step: 
    /// 
    /// 1. Bind the values from Organization object to the arraylist of sql query parameters. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private void SetParameters ( SqlParameter [ ] cmdParms, EdOrganisation Organisation )
    {
      cmdParms [ 0 ].Value = Organisation.Guid;
      cmdParms [ 1 ].Value = Organisation.OrgId;
      cmdParms [ 2 ].Value = Organisation.Name;
      cmdParms [ 3 ].Value = Organisation.AddressStreet_1;
      cmdParms [ 4 ].Value = Organisation.AddressStreet_2;
      cmdParms [ 5 ].Value = Organisation.AddressCity;
      cmdParms [ 6 ].Value = Organisation.AddressPostCode;
      cmdParms [ 7 ].Value = Organisation.AddressState;
      cmdParms [ 8 ].Value = Organisation.AddressCountry;
      cmdParms [ 9 ].Value = Organisation.Telephone;
      cmdParms [ 10 ].Value = Organisation.EmailAddress;
      cmdParms [ 11 ].Value = Organisation.OrgType;
      cmdParms [ 12 ].Value = Organisation.ImageFileName;
      cmdParms [ 13 ].Value = this.ClassParameters.UserProfile.UserId;
      cmdParms [ 14 ].Value = this.ClassParameters.UserProfile.CommonName;
      cmdParms [ 15 ].Value = DateTime.Now;

    }//END SetParameters class.

    #endregion

    #region Data Reader section

    // =====================================================================================
    /// <summary>
    /// This class reads the content of the SqlDataReader into the Organisation data object.
    /// </summary>
    /// <param name="Row">DataRow: a data row object</param>
    /// <returns>EvOrganization: an organization object</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Extract the compatible row data object to the organization object. 
    /// 
    /// 2. Return the organization data object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EdOrganisation readQueryRow ( DataRow Row )
    {
      // 
      // Initialise the Organisation
      // 
      EdOrganisation organisation = new EdOrganisation ( );

      // 
      // Fill the object.
      // 
      organisation.Guid = EvSqlMethods.getGuid ( Row, EdOrganisations.DB_GUID );
      organisation.OrgId = EvSqlMethods.getString ( Row, EdOrganisations.DB_ORG_ID );
      organisation.Name = EvSqlMethods.getString ( Row, EdOrganisations.DB_NAME );
      organisation.AddressStreet_1 = EvSqlMethods.getString ( Row, EdOrganisations.DB_ADDRESS_1 );
      organisation.AddressStreet_2 = EvSqlMethods.getString ( Row, EdOrganisations.DB_ADDRESS_2 );
      organisation.AddressCity = EvSqlMethods.getString ( Row, EdOrganisations.DB_ADDRESS_CITY );
      organisation.AddressPostCode = EvSqlMethods.getString ( Row, EdOrganisations.DB_ADDRESS_POST_CODE );
      organisation.AddressState = EvSqlMethods.getString ( Row, EdOrganisations.DB_ADDRESS_STATE );
      organisation.AddressCountry = EvSqlMethods.getString ( Row, EdOrganisations.DB_COUNTRY );
      organisation.Telephone = EvSqlMethods.getString ( Row, EdOrganisations.DB_TELEPHONE );
      organisation.EmailAddress = EvSqlMethods.getString ( Row, EdOrganisations.DB_EMAIL_ADDRESS );
      organisation.OrgType = EvSqlMethods.getString ( Row, EdOrganisations.DB_ORG_TYPE );
      organisation.ImageFileName = EvSqlMethods.getString ( Row, EdOrganisations.DB_IMAGE_FILENAME);

      organisation.UpdatedByUserId = EvSqlMethods.getString ( Row, EdOrganisations.DB_UPDATED_BY_USER_ID );
      organisation.UpdatedBy = EvSqlMethods.getString ( Row, EdOrganisations.DB_UPDATED_By );
      organisation.UpdatedDate = EvSqlMethods.getDateTime ( Row, EdOrganisations.DB_UPDATE_DATE );

      if ( organisation.OrgId.ToLower ( ) == "evado" )
      {
        organisation.OrgType = "Evado";
      }

      // 
      // Return the Organisation object.
      // 
      return organisation;

    }//End readRow method.

    #endregion

    #region List Queries section

    // =====================================================================================
    /// <summary>
    /// This class returns a list of organization data object based on the passed parameters. 
    /// </summary>
    /// <param name="Type">EvOrganisation.OrganisationTypes: the organisation type.</param>
    /// <returns>List of EvOrganisation: a list of organization data object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Set the IsTrue string to 0, if the selection type is not selected.
    /// 
    /// 2. Define the sql query string and execute the sql query string to a datatable
    /// 
    /// 3. Loop through the table and extract the row data to the organization object. 
    /// 
    /// 4. Add the organization object value to the Organizations list. 
    /// 
    /// 5. Return the organizations list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EdOrganisation> getOrganisationList (
      String Type )
    {
      //
      // Initialize the method status, a return organization list and an IsTrue string value
      //
      this.LogMethod ( "getView method. " );
      this.LogDebug ( "Type: " + Type );
      //
      // Initialise the methods variables and objects.
      //
      List<EdOrganisation> organisationList = new List<EdOrganisation> ( );


      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter ( EdOrganisations.PARM_ORG_TYPE, SqlDbType.NVarChar, 50 )
      };
      cmdParms [ 0 ].Value = Type;

      // 
      // Create the sql query string.
      // 
      sqlQueryString = SQL_SELECT_QUERY
        + "WHERE (" + EdOrganisations.DB_DELETED + " = 0) ";

      if ( Type != String.Empty )
      {
        sqlQueryString += " AND (" + EdOrganisations.DB_ORG_TYPE + " = " + EdOrganisations.PARM_ORG_TYPE + ") ";
      }

      sqlQueryString += " ORDER BY " + EdOrganisations.DB_ORG_ID + ";";

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

          EdOrganisation organisation = this.readQueryRow ( row );

          organisationList.Add ( organisation );
        }
      }
      this.LogDebug ( " View Count: " + organisationList.Count );

      // 
      // Return the arraylist of organisations.
      // 
      this.LogMethodEnd ( "getView" );
      return organisationList;

    }//End getView method.

    // =====================================================================================
    /// <summary>
    /// This class returns an option list based on the passed parameters. 
    /// </summary>
    /// <param name="Type">EvOrganisation.OrganisationTypes: an organization type</param>
    /// <param name="IsCurrent">Boolean: true, if the current organization is selected</param>
    /// <returns>List of EvOption: a list of option data object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Set the IsTrue string to 0, if not the selection type.
    /// 
    /// 2. Define the sql query string and execute the sql query string to a datatable
    /// 
    /// 3. Loop through the table and extract the row data to the option object. 
    /// 
    /// 4. Add the option object value to the options list. 
    /// 
    /// 5. Return the options list.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> getList (
      String Type )
    {
      this.LogMethod ( "getList" );
      this.LogDebug ( "Type: " + Type );
      //
      // Initialize the method variable and objects.
      //
      List<EvOption> list = new List<EvOption> ( );
      EvOption option = new EvOption ( );
      list.Add ( option );

      //
      // Get the list of organisations
      //
      var organisationList = this.getOrganisationList ( Type );

      //
      // iterate through the organisation list creating the selection list.
      //
      foreach ( EdOrganisation org in organisationList )
      {
        option = new EvOption (
          org.OrgId,
          String.Format ( "{0} - {1}, Type: {2}", org.OrgId, org.Name, org.OrgType ) );

        list.Add ( option );
      }

      this.LogDebug ( "Option list count: " + list.Count );

      // 
      // Return the Organisation selection visitSchedule.
      // 
      this.LogMethodEnd ( "getList" );
      return list;

    }//END getList method

    #endregion

    #region Get Organisation object section

    // =====================================================================================
    /// <summary>
    /// This class retrieves an organization data table based on Organization Guid
    /// </summary>
    /// <param name="OrgGuid">Guid: Organisation object global unique identifier</param>
    /// <returns>EvOrganisation: an organisation data object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Return an empty Organization object if the Guid is empty. 
    /// 
    /// 2. Define the sql query parameters and sql query string 
    /// 
    /// 3. Execute the sql query string and store the results on the datatable. 
    /// 
    /// 4. Extract the first row data to the organization object. 
    /// 
    /// 5. Return the organization data object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EdOrganisation getItem ( Guid OrgGuid )
    {
      //
      // Initialize the method status and a return organization object. 
      //
      this.LogMethod ( "getItem method" );
      this.LogDebug ( "OrgGuid: " + OrgGuid );

      EdOrganisation organisation = new EdOrganisation ( );

      // 
      // Validate whether the Guid is not empty.
      // 
      if ( OrgGuid == Guid.Empty )
      {
        this.LogDebug ( "OrgGuid empty" );
        this.LogMethodEnd ( "getItem" );
        return organisation;
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter (  EdOrganisations.PARM_Guid, SqlDbType.UniqueIdentifier )
      };
      cmdParms [ 0 ].Value = OrgGuid;

      // 
      // Construct the Sql query string.
      // 
      sqlQueryString = SQL_SELECT_QUERY
        + "WHERE (" + EdOrganisations.DB_GUID + " = " + EdOrganisations.PARM_Guid + ") ; ";

      this.LogDebug ( sqlQueryString );
      this.LogDebug ( EvSqlMethods.getParameterSqlText ( cmdParms ) );

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
          this.LogDebug ( "ROW NOT FOUND" );
          this.LogMethodEnd ( "getItem" );

          return organisation;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        // 
        // Fill the role object.
        // 
        organisation = this.readQueryRow ( row );

        //
        // if the use is not an Evado user and the organisation type is Evado 
        // return an empty organisation object.
        //
        if ( organisation.OrgType == "Evado"
          && this.ClassParameters.UserProfile.hasEvadoAccess == false )
        {
          organisation = new EdOrganisation ( );
        }

      }//END Using 

      // 
      // Return Organisation.
      // 
      this.LogMethodEnd ( "getItem" );
      return organisation;

    }//END getItem method

    // =====================================================================================
    /// <summary>
    /// This class retrieves an organization data table based on the organization identifier. 
    /// </summary>
    /// <param name="OrgId">string: Organisation identifier</param>
    /// <returns>EvOrganisation: Organisation data object.s</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Return an empty Organization object if the OrgId is empty. 
    /// 
    /// 2. Define the sql query parameters and sql query string 
    /// 
    /// 3. Execute the sql query string and store the results on the datatable. 
    /// 
    /// 4. Extract the first row data to the organization object. 
    /// 
    /// 5. Return the Organization data object.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EdOrganisation getItem ( string OrgId )
    {
      //
      // Initialize the method status and a return organization object. 
      //
      this.LogMethod ( "getItem method" );
      this.LogDebug ( "OrgId: " + OrgId );
      // 
      // Initialise the local variables
      // 
      EdOrganisation organisation = new EdOrganisation ( );

      // 
      // Validate whether the organizationId is not empty
      // 
      if ( OrgId == String.Empty )
      {
        return organisation;
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter (  EdOrganisations.PARM_ORG_ID, SqlDbType.NVarChar, 10 )
      };
      cmdParms [ 0 ].Value = OrgId;

      // 
      // Construct the Sql query string.
      // 
      sqlQueryString = SQL_SELECT_QUERY
        + "WHERE ( " + EdOrganisations.DB_ORG_ID + "  =" + EdOrganisations.PARM_ORG_ID + " ) ; ";

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
          this.LogDebug ( "No Rows returned" );

          return organisation;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        // 
        // Fill the role object.
        // 
        organisation = this.readQueryRow ( row );

        //
        // if the use is not an Evado user and the organisation type is Evado 
        // return an empty organisation object.
        //
        if ( organisation.OrgType =="Evado"
          && this.ClassParameters.UserProfile.hasEvadoAccess == false )
        {
          organisation = new EdOrganisation ( );
        }

      }//END Using 

      // 
      // Return Organisation.
      // 
      this.LogMethodEnd ( "getItem" );
      return organisation;

    }//END getItem method

    #endregion

    #region Update Organisation.

    // =====================================================================================
    /// <summary>
    /// This class updates items to the organization data object. 
    /// </summary>
    /// <param name="Organisation">EvOrganisation: an Organisation object</param>
    /// <returns>EvEventCodes: an event code for updating result</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the Old Organization's Guid is empty or the New Organization's identfier is duplicated. 
    /// 
    /// 2. Create new orgnization's Guid if it is empty. 
    /// 
    /// 3. Add new items to datachange object if they do not exist. 
    /// 
    /// 4. Define the sql query parameters and execute the storeprocedure for updating 
    /// 
    /// 5. Add datachange object values to the backup datachanges object. 
    /// 
    /// 6. Return an event code for updating items. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes updateItem ( EdOrganisation Organisation )
    {
      //
      // Initialize the method status and an old organization object
      //
      this.LogMethod ( "updateItem method." );
      this.LogDebug ( "OrgId: " + Organisation.OrgId );

      EdOrganisation oldOrg = getItem ( Organisation.Guid );

      //
      // Validate whether the Old organization exists. 
      //
      if ( oldOrg.Guid == Guid.Empty )
      {
        this.LogDebug ( " Invalid Guid not object found." );
        return EvEventCodes.Data_InvalidId_Error;
      }

      // 
      // Validate whether the new Organization Identifier is unique.
      // 
      if ( oldOrg.OrgId != Organisation.OrgId )
      {
        EdOrganisation newOrg = getItem ( Organisation.OrgId );
        if ( newOrg.Guid != Guid.Empty )
        {
          this.LogDebug ( " DuplicateId error" );
          return EvEventCodes.Data_Duplicate_Id_Error;
        }
      }

      // 
      // If the guid is null create a new guid.
      // 
      if ( Organisation.Guid == Guid.Empty )
      {
        this.LogDebug ( " Creating a new GUID." );
        Organisation.Guid = Guid.NewGuid ( );
      }

      // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      // Compare the objects.
      // Initialize the datachange object and a backup datachanges object
      //
      EvDataChanges dataChanges = new EvDataChanges ( );
      EvDataChange dataChange = new EvDataChange ( );
      dataChange.TableName = EvDataChange.DataChangeTableNames.EvOrganisations;
      dataChange.RecordGuid = Organisation.Guid;
      dataChange.UserId = Organisation.UpdatedByUserId;
      dataChange.DateStamp = DateTime.Now;

      //
      // Add new items to the datachange object if they do not exist.
      //
      if ( Organisation.Name != oldOrg.Name )
      {
        dataChange.AddItem ( "Name", oldOrg.Name, Organisation.Name );
      }
      if ( Organisation.AddressStreet_1 != oldOrg.AddressStreet_1 )
      {
        dataChange.AddItem ( "Address_1", oldOrg.AddressStreet_1, Organisation.AddressStreet_1 );
      }
      if ( Organisation.AddressStreet_2 != oldOrg.AddressStreet_2 )
      {
        dataChange.AddItem ( "AddressStreet_2", oldOrg.AddressStreet_2, Organisation.AddressStreet_2 );
      }
      if ( Organisation.AddressCity != oldOrg.AddressCity )
      {
        dataChange.AddItem ( "AddressCity", oldOrg.AddressCity, Organisation.AddressCity );
      }
      if ( Organisation.AddressState != oldOrg.AddressState )
      {
        dataChange.AddItem ( "AddressState", oldOrg.AddressState, Organisation.AddressState );
      }
      if ( Organisation.AddressPostCode != oldOrg.AddressPostCode )
      {
        dataChange.AddItem ( "AddressPostCode", oldOrg.AddressPostCode, Organisation.AddressPostCode );
      }
      if ( Organisation.AddressCountry != oldOrg.AddressCountry )
      {
        dataChange.AddItem ( "AddressCountry", oldOrg.AddressCountry, Organisation.AddressCountry );
      }
      if ( Organisation.Telephone != oldOrg.Telephone )
      {
        dataChange.AddItem ( "Telephone", oldOrg.Telephone, Organisation.Telephone );
      }
      if ( Organisation.EmailAddress != oldOrg.EmailAddress )
      {
        dataChange.AddItem ( "EmailAddress", oldOrg.EmailAddress, Organisation.EmailAddress );
      }
      if ( Organisation.OrgType != oldOrg.OrgType )
      {
        dataChange.AddItem ( "OrgType", oldOrg.OrgType.ToString ( ), Organisation.OrgType.ToString ( ) );
      }

      // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      // 
      // Define the query parameters
      // 
      this.LogDebug ( " Setting Parameters." );
      SqlParameter [ ] cmdParms = GetParameters ( );
      SetParameters ( cmdParms, Organisation );

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( EdOrganisations.STORED_PROCEDURE_UPDATE_ITEM, cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      // 
      // Save the datachanges to the database.
      // 
      dataChanges.AddItem ( dataChange );
      this.LogDebug ( "DataChange: " + dataChanges.Log );

      return EvEventCodes.Ok;

    }//END updateItem method

    // =====================================================================================
    /// <summary>
    /// This class adds new items to the organization data table. 
    /// </summary>
    /// <param name="Organisation">EvOrganisation: Organisation object</param>
    /// <returns>EvEventCodes: an event code for adding result</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Exit, if the Old organization's Guid is not empty.
    /// 
    /// 2. If the New Organization's Guid is null, create a new Guid.
    /// 
    /// 3. Define the sql query parmeter and execute the storeprocedure for adding items. 
    /// 
    /// 4. Return an event code for adding items.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes addItem ( EdOrganisation Organisation )
    {
      //
      // Initialize the method status and the old organization object. 
      //
      this.LogMethod ( "addItem method. " );

      // 
      // If the Guid is null create a new guid.
      // 
      if ( Organisation.Guid == Guid.Empty )
      {
        Organisation.Guid = Guid.NewGuid ( );
      }

      // 
      // Define the query parameters
      // 
      SqlParameter [ ] cmdParms = GetParameters ( );
      SetParameters ( cmdParms, Organisation );

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( EdOrganisations.STORED_PROCEDURE_ADD_ITEM, cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      return EvEventCodes.Ok;

    }//END addItem method 

    // =====================================================================================
    /// <summary>
    /// This method deletes the items from organization data table. 
    /// </summary>
    /// <param name="organisation">EvOrganisation: an organisation data object</param>
    /// <returns>EvEventCodes: an event code for deleting results</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit if the orgnization identifier is in used in the trial organization. 
    /// 
    /// 2. Define the sql query parameters and execute the store procedure for deleting items
    /// 
    /// 3. Return the event code for deleting items. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes deleteItem ( EdOrganisation organisation )
    {
      //
      // Initialize the method status and a trial orgnaization object. 
      //
      this.LogMethod ( "deleteItem method. " );
      this.LogDebug ( "OrgId: " + organisation.OrgId );

      // 
      // Define the query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(PARM_Guid, SqlDbType.UniqueIdentifier),
        new SqlParameter(PARM_UPDATED_BY, SqlDbType.NVarChar, 100),
        new SqlParameter(PARM_UPDATED_BY_USER_ID, SqlDbType.NVarChar, 100),
        new SqlParameter(PARM_UPDATE_DATE, SqlDbType.DateTime)
      };
      cmdParms [ 0 ].Value = organisation.Guid;
      cmdParms [ 1 ].Value = this.ClassParameters.UserProfile.UserId;
      cmdParms [ 2 ].Value = this.ClassParameters.UserProfile.CommonName;
      cmdParms [ 3 ].Value = DateTime.Now;

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( EdOrganisations.STORED_PROCEDURE_DELETE_ITEM, cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      return EvEventCodes.Ok;

    }//End deleteItem method.

    #endregion

  }//END EvOrganisations class.

}//END Evado.Dal.Clinical