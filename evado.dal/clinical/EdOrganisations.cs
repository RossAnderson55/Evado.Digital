/***************************************************************************************
 * <copyright file="dal\EvOrganisations.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2020 EVADO HOLDING PTY. LTD.  All rights reserved.
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
      this.ClassNameSpace = "Evado.Dal.Clinical.EvOrganisations.";
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
      this.ClassNameSpace = "Evado.Dal.Clinical.EvOrganisations.";

      this.LogDebug ( "ApplicationGuid: " + this.ClassParameters.AdapterGuid );

    }
    #endregion

    #region Initialise Class variables and parameters
    //
    // Static constants
    //
    private const string SQL_SELECT_QUERY = "Select * FROM EvOrganisations ";

    private const string SQL_SELECT_ORGANISATION_VIEW = "Select * FROM EV_ORGANISATION_VIEW ";

    private const string STORED_PROCEDURE_AddItem = "usr_Organisation_add";
    private const string STORED_PROCEDURE_UpdateItem = "usr_Organisation_update";
    private const string STORED_PROCEDURE_DeleteItem = "usr_Organisation_delete";

    private const string DB_FIELD_CUSTOMER_GUID = "CU_GUID";
    private const string DB_FIELD_DELETED= "O_SUPERSEDEDF";
    /// <summary>
    /// This constant defines a string parameter as Guid for the application object
    /// User by Evado administrator and staff.
    /// </summary>
    private const string PARM_APPLICATION_GUID = "@APPLICATION_GUID";

    private const string PARM_Guid = "@Guid";
    private const string PARM_CUSTOMER_GUID = "@CUSTOMER_GUID";
    private const string PARM_Uid = "@Uid";
    private const string PARM_OrgId = "@OrgId";
    private const string PARM_AdGroup = "@AD_GROUP";
    private const string PARM_Name = "@Name";
    private const string PARM_Address_1 = "@ADDRESS_1";
    private const string PARM_Address_2 = "@ADDRESS_2";
    private const string PARM_Address_City = "@ADDRESS_CITY";
    private const string PARM_Address_Post_Code = "@ADDRESS_POST_CODE";
    private const string PARM_Address_State = "@ADDRESS_STATE";
    private const string PARM_Country = "@Country";
    private const string PARM_Telephone = "@Telephone";
    private const string PARM_FaxPhone = "@FaxPhone";
    private const string PARM_EmailAddress = "@Email";
    private const string PARM_ORG_TYPE = "@ORG_TYPE";
    private const string PARM_Order = "@OrgOrder";
    private const string PARM_IsCurrent = "@IsCurrent";
    private const string PARM_UpdatedByUserId = "@UpdatedByUserId";
    private const string PARM_UpdatedBy = "@UpdatedBy";
    private const string PARM_UpdateDate = "@UpdateDate";

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
        new SqlParameter( PARM_Guid, SqlDbType.UniqueIdentifier),
        new SqlParameter( PARM_CUSTOMER_GUID, SqlDbType.UniqueIdentifier),
        new SqlParameter( PARM_OrgId, SqlDbType.NVarChar, 10),
        new SqlParameter( PARM_AdGroup, SqlDbType.NVarChar, 30),
        new SqlParameter( PARM_Name, SqlDbType.NVarChar, 50),
        new SqlParameter( PARM_Address_1, SqlDbType.NVarChar, 50),
        new SqlParameter( PARM_Address_2, SqlDbType.NVarChar, 50),
        new SqlParameter( PARM_Address_City, SqlDbType.NVarChar, 50),
        new SqlParameter( PARM_Address_Post_Code, SqlDbType.NVarChar, 10),
        new SqlParameter( PARM_Address_State, SqlDbType.NVarChar, 50),
        new SqlParameter( PARM_Country, SqlDbType.NVarChar, 50),
        new SqlParameter( PARM_Telephone, SqlDbType.NVarChar, 15),
        new SqlParameter( PARM_FaxPhone, SqlDbType.NVarChar, 15),
        new SqlParameter( PARM_EmailAddress, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_ORG_TYPE, SqlDbType.NVarChar, 50),
        new SqlParameter( PARM_Order, SqlDbType.SmallInt),
        new SqlParameter( PARM_IsCurrent, SqlDbType.Bit),
        new SqlParameter( PARM_UpdatedByUserId, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_UpdatedBy, SqlDbType.NVarChar, 100),
        new SqlParameter( PARM_UpdateDate, SqlDbType.DateTime)
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
    private void SetParameters ( SqlParameter [ ] cmdParms, EvOrganisation Organisation )
    {
      if ( Organisation.CustomerGuid == null )
      {
        Organisation.CustomerGuid = Guid.Empty;
      }

      cmdParms [ 0 ].Value = Organisation.Guid;
      cmdParms [ 1 ].Value = Organisation.CustomerGuid;
      cmdParms [ 2 ].Value = Organisation.OrgId;
      cmdParms [ 3 ].Value = Organisation.AdGroup;
      cmdParms [ 4 ].Value = Organisation.Name;
      cmdParms [ 5 ].Value = Organisation.AddressStreet_1;
      cmdParms [ 6 ].Value = Organisation.AddressStreet_2;
      cmdParms [ 7 ].Value = Organisation.AddressCity;
      cmdParms [ 8 ].Value = Organisation.AddressPostCode;
      cmdParms [ 9 ].Value = Organisation.AddressState;
      cmdParms [ 10 ].Value = Organisation.AddressCountry;
      cmdParms [ 11 ].Value = Organisation.Telephone;
      cmdParms [ 12 ].Value = Organisation.FaxPhone;
      cmdParms [ 13 ].Value = Organisation.EmailAddress;
      cmdParms [ 14 ].Value = Organisation.OrgType;
      cmdParms [ 15 ].Value = Organisation.Order;
      cmdParms [ 16 ].Value = Organisation.Current;
      cmdParms [ 17 ].Value = Organisation.UpdatedByUserId;
      cmdParms [ 18 ].Value = Organisation.UserCommonName;
      cmdParms [ 19 ].Value = DateTime.Now;

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
    public EvOrganisation readQueryRow ( DataRow Row )
    {
      // 
      // Initialise the Organisation
      // 
      EvOrganisation organisation = new EvOrganisation ( );

      // 
      // Fill the object.
      // 
      organisation.Guid = EvSqlMethods.getGuid ( Row, "O_Guid" );
      organisation.CustomerGuid = EvSqlMethods.getGuid ( Row, "CU_GUID" );
      organisation.OrgId = EvSqlMethods.getString ( Row, "OrgId" );
      organisation.AdGroup = EvSqlMethods.getString ( Row, "O_AD_GROUP" );
      organisation.Name = EvSqlMethods.getString ( Row, "O_Name" );
      organisation.AddressStreet_1 = EvSqlMethods.getString ( Row, "O_Address_1" );
      organisation.AddressStreet_2 = EvSqlMethods.getString ( Row, "O_Address_2" );
      organisation.AddressCity = EvSqlMethods.getString ( Row, "O_Address_City" );
      organisation.AddressPostCode = EvSqlMethods.getString ( Row, "O_Address_Post_code" );
      organisation.AddressState = EvSqlMethods.getString ( Row, "O_Address_State" );
      organisation.AddressCountry = EvSqlMethods.getString ( Row, "O_Country" );
      organisation.Telephone = EvSqlMethods.getString ( Row, "O_Telephone" );
      organisation.FaxPhone = EvSqlMethods.getString ( Row, "O_FaxPhone" );
      organisation.EmailAddress = EvSqlMethods.getString ( Row, "O_Email" );
      string stOrgType = EvSqlMethods.getString ( Row, "O_ORG_TYPE" );

      organisation.OrgType = EvOrganisation.OrganisationTypes.Null;
      if ( stOrgType != String.Empty )
      {
        organisation.OrgType = Evado.Model.EvStatics.Enumerations.parseEnumValue<EvOrganisation.OrganisationTypes> (
          stOrgType );
      }
      organisation.Order = EvSqlMethods.getInteger ( Row, "O_OrgOrder" );
      organisation.Current = EvSqlMethods.getBool ( Row, "O_IsCurrent" );

      organisation.UpdatedBy = EvSqlMethods.getString ( Row, "O_UpdatedBy" );
      organisation.UpdatedDate = EvSqlMethods.getDateTime ( Row, "O_UpdateDate" );

      if ( organisation.OrgId.ToLower ( ) == "evado" )
      {
        organisation.OrgType = EvOrganisation.OrganisationTypes.Evado;
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
    /// <param name="IsCurrent">Boolean: true, if select current organisations.</param>
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
    public List<EvOrganisation> getView (
      EvOrganisation.OrganisationTypes Type,
      bool IsCurrent )
    {
      //
      // Initialize the method status, a return organization list and an IsTrue string value
      //
      this.LogMethod ( "getView method. " );
      this.LogDebug ( "Type: " + Type );
      //
      // Initialise the methods variables and objects.
      //
      List<EvOrganisation> organisationList = new List<EvOrganisation> ( );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter ( EdOrganisations.PARM_APPLICATION_GUID, SqlDbType.UniqueIdentifier), 
      };
      cmdParms [ 1 ].Value = this.ClassParameters.AdapterGuid;

      //
      // if the user is not an Evado user then set the Evado identifier (ApplicationGuid) to empty
      // to ensure that Evado organisations are not displayed in the org list.
      //
      if ( this.ClassParameters.UserProfile.hasEvadoAccess == false )
      {
        cmdParms [ 1 ].Value = Guid.Empty;
      }

      // 
      // Create the sql query string.
      // 
      sqlQueryString = SQL_SELECT_QUERY
        + "WHERE (  (O_DELETED = 0 ) ";

      if ( IsCurrent == true )
      {
        sqlQueryString += " AND (O_IsCurrent = 1) ";
      }

      if ( Type != EvOrganisation.OrganisationTypes.Null )
      {
        sqlQueryString += " AND (O_ORG_TYPE = '" + Type + "') ";
      }

      sqlQueryString += " ORDER BY OrgId";

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

          EvOrganisation organisation = this.readQueryRow ( row );

          if ( organisation.OrgType == EvOrganisation.OrganisationTypes.Evado
            && this.ClassParameters.UserProfile.hasEvadoAccess == false )
          {
            continue;
          }

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
      EvOrganisation.OrganisationTypes Type,
      bool IsCurrent )
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
      var organisationList = this.getView ( Type, IsCurrent );

      //
      // iterate through the organisation list creating the selection list.
      //
      foreach ( EvOrganisation org in organisationList )
      {
        option = new EvOption (
          org.OrgId,
          String.Format ( "{0} - {1}, Type: {2}", org.OrgId, org.Name, org.stOrgType ) );

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
    public EvOrganisation getItem ( Guid OrgGuid )
    {
      //
      // Initialize the method status and a return organization object. 
      //
      this.LogMethod ( "getItem method" );
      this.LogDebug ( "OrgGuid: " + OrgGuid );

      EvOrganisation organisation = new EvOrganisation ( );

      // 
      // Validate whether the Guid is not empty.
      // 
      if ( OrgGuid == Guid.Empty )
      {
        return organisation;
      }

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter ( EdOrganisations.PARM_APPLICATION_GUID, SqlDbType.UniqueIdentifier), 
        new SqlParameter (  EdOrganisations.PARM_Guid, SqlDbType.UniqueIdentifier )
      };
      cmdParms [ 0 ].Value = this.ClassParameters.AdapterGuid;
      cmdParms [ 1 ].Value = OrgGuid;

      //
      // if the user is not an Evado user then set the Evado identifier (ApplicationGuid) to empty
      // to ensure that Evado organisations are not displayed in the org list.
      //
      if ( this.ClassParameters.UserProfile.hasEvadoAccess == false )
      {
        cmdParms [ 1 ].Value = Guid.Empty;
      }

      // 
      // Construct the Sql query string.
      // 
      sqlQueryString = SQL_SELECT_QUERY
        + "WHERE ( (" + EdOrganisations.DB_FIELD_DELETED + " = 0 )\r\n"
        + "     OR (" + EdOrganisations.DB_FIELD_CUSTOMER_GUID + " = " + EdOrganisations.PARM_APPLICATION_GUID + ") )\r\n"
        + " AND ( O_Guid =" + EdOrganisations.PARM_Guid + " ) ; ";

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
          this.LogDebug ( "ROW NOT FOUND" );

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
        if ( organisation.OrgType == EvOrganisation.OrganisationTypes.Evado
          && this.ClassParameters.UserProfile.hasEvadoAccess == false )
        {
          organisation = new EvOrganisation();
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
    public EvOrganisation getItem ( string OrgId )
    {
      //
      // Initialize the method status and a return organization object. 
      //
      this.LogMethod ( "getItem method" );
      this.LogDebug ( "OrgId: " + OrgId );
      // 
      // Initialise the local variables
      // 
      EvOrganisation organisation = new EvOrganisation ( );

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
        new SqlParameter (  EdOrganisations.PARM_OrgId, SqlDbType.NVarChar, 10 )
      };
      cmdParms [ 0 ].Value = OrgId;

      // 
      // Construct the Sql query string.
      // 
      sqlQueryString = SQL_SELECT_QUERY
        + "WHERE ( (" + EdOrganisations.DB_FIELD_DELETED + " = 0 ) )\r\n"
        + " AND ( OrgId =" + EdOrganisations.PARM_OrgId + " ) ; ";

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
        if ( organisation.OrgType == EvOrganisation.OrganisationTypes.Evado
          && this.ClassParameters.UserProfile.hasEvadoAccess == false )
        {
          organisation = new EvOrganisation ( );
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
    public EvEventCodes updateItem ( EvOrganisation Organisation )
    {
      //
      // Initialize the method status and an old organization object
      //
      this.LogMethod ( "updateItem method." );
      this.LogDebug ( "OrgId: " + Organisation.OrgId );

      EvOrganisation oldOrg = getItem ( Organisation.Guid );

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
        EvOrganisation newOrg = getItem ( Organisation.OrgId );
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
        dataChange.AddItem ( "Address", oldOrg.AddressStreet_1, Organisation.AddressStreet_1 );
      }
      if ( Organisation.Telephone != oldOrg.Telephone )
      {
        dataChange.AddItem ( "Telephone", oldOrg.Telephone, Organisation.Telephone );
      }
      if ( Organisation.FaxPhone != oldOrg.FaxPhone )
      {
        dataChange.AddItem ( "FaxPhone", oldOrg.FaxPhone, Organisation.FaxPhone );
      }
      if ( Organisation.EmailAddress != oldOrg.EmailAddress )
      {
        dataChange.AddItem ( "Email", oldOrg.EmailAddress, Organisation.EmailAddress );
      }
      if ( Organisation.OrgType != oldOrg.OrgType )
      {
        dataChange.AddItem ( "OrgType", oldOrg.OrgType.ToString ( ), Organisation.OrgType.ToString ( ) );
      }
      if ( Organisation.Current != oldOrg.Current )
      {
        dataChange.AddItem ( "IsCurrent", oldOrg.Current.ToString ( ), Organisation.Current.ToString ( ) );
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
      if ( EvSqlMethods.StoreProcUpdate ( STORED_PROCEDURE_UpdateItem, cmdParms ) == 0 )
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
    public EvEventCodes addItem ( EvOrganisation Organisation )
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
      if ( EvSqlMethods.StoreProcUpdate ( STORED_PROCEDURE_AddItem, cmdParms ) == 0 )
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
    public EvEventCodes deleteItem ( EvOrganisation organisation )
    {
      //
      // Initialize the method status and a trial orgnaization object. 
      //
      this.LogMethod ( "deleteItem method. " );
      this.LogDebug ( "OrgId: " + organisation.OrgId );

      // 
      // Validate whether the orgnization is not in used in the trial. 
      // 
      if ( organisation.OrgType == EvOrganisation.OrganisationTypes.Data_Collection )
      {
        return EvEventCodes.Business_Logic_Object_In_Use_Error;
      }

      // 
      // Define the query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(PARM_Guid, SqlDbType.UniqueIdentifier),
        new SqlParameter(PARM_UpdatedBy, SqlDbType.NVarChar, 100),
        new SqlParameter(PARM_UpdatedByUserId, SqlDbType.NVarChar, 100),
        new SqlParameter(PARM_UpdateDate, SqlDbType.DateTime)
      };
      cmdParms [ 0 ].Value = organisation.Guid;
      cmdParms [ 1 ].Value = organisation.UserCommonName;
      cmdParms [ 2 ].Value = organisation.UpdatedByUserId;
      cmdParms [ 3 ].Value = DateTime.Now;

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( STORED_PROCEDURE_DeleteItem, cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      return EvEventCodes.Ok;

    }//End deleteItem method.

    #endregion

  }//END EvOrganisations class.

}//END Evado.Dal.Clinical