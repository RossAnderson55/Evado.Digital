/***************************************************************************************
 * <copyright file="dal\EvCustomers.cs" company="EVADO HOLDING PTY. LTD.">
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
  public class EvCustomers : EvDalBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EvCustomers ( )
    {
      this.ClassNameSpace = "Evado.Dal.Clinical.EvCustomers.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EvCustomers ( EvClassParameters Settings )
    {
      this.ClassParameters = Settings;
      this.ClassNameSpace = "Evado.Dal.Clinical.EvCustomers.";

      if ( this.ClassParameters.LoggingLevel == 0 )
      {
        this.ClassParameters.LoggingLevel = Evado.Dal.EvStaticSetting.LoggingLevel;
      }

    }
    #endregion

    #region Initialise Class variables and parameters
    //
    // Static constants
    //
    private const string _sqlQuery_View = "Select * FROM EV_CUSTOMERS ";

    private const string _STORED_PROCEDURE_AddItem = "USR_CUSTOMER_ADD";
    private const string _STORED_PROCEDURE_UpdateItem = "USR_CUSTOMER_UPDATE";
    private const string _STORED_PROCEDURE_DeleteItem = "USR_CUSTOMER_DELETE";

    private const string DB_GUID = "CU_GUID";
	  private const string DB_CUSTOMER_NO = "CU_CUSTOMER_NO";
	  private const string DB_NAME = "CU_NAME";
	  private const string DB_ADDRESS_STREET_1 = "CU_ADDRESS_STREET_1";
	  private const string DB_ADDRESS_STREET_2 = "CU_ADDRESS_STREET_2";
	  private const string DB_ADDRESS_CITY = "CU_ADDRESS_CITY";
	  private const string DB_ADDRESS_POSTCODE = "CU_ADDRESS_POST_CODE";
	  private const string DB_ADDRESS_STATE = "CU_ADDRESS_STATE";
	  private const string DB_ADDRESS_COUNTRY = "CU_ADDRESS_COUNTRY";
	  private const string DB_TELEPHONE = "CU_TELEPHONE";
    private const string DB_EMAIL = "CU_EMAIL";
    private const string DB_ADMIN_NAME = "CU_ADMIN_NAME";
    private const string DB_ADMIN_EMAIL = "CU_ADMIN_EMAIL";
    private const string DB_NO_OF_STUDIES = "CU_NO_OF_STUDIES";
    private const string DB_SERVICE_TYPE = "CU_SERVICE_TYPE";
    private const string DB_STATE = "CU_STATE";
    private const string DB_ADS_GROUP = "CU_ADS_GROUP";
    private const string DB_HOME_PAGE_HEADER = "CU_HOME_PAGE_HEADER";
	  private const string DB_IS_CURRENT = "CU_IS_CURRENT";
	  private const string DB_UPDATE_BY_USER_ID = "CU_UPDATE_BY_USER_ID";
	  private const string DB_UPDATED_BY = "CU_UPDATE_BY";
    private const string DB_UPDATE_DATE = "CU_UPDATE_DATE";
    private const string DB_DELETED = "CU_DELETED";

    private const string PARM_Guid = "@Guid";
    private const string PARM_CUSTOMER_NO = "@CUSTOMER_NO";
    private const string PARM_ADMIN_NAME = "@ADMIN_NAME";
    private const string PARM_ADMIN_EMAIL = "@ADMIN_EMAIL";
    private const string PARM_NAME = "@NAME";
    private const string PARM_ADDRESS_STREET_1 = "@ADDRESS_STREET_1";
    private const string PARM_ADDRESS_STREET_2 = "@ADDRESS_STREET_2";
    private const string PARM_ADDRESS_CITY = "@ADDRESS_CITY";
    private const string PARM_ADDRESS_POST_CODE = "@ADDRESS_POST_CODE";
    private const string PARM_ADDRESS_STATE = "@ADDRESS_STATE";
    private const string PARM_ADDRESS_COUNTRY = "@ADDRESS_COUNTRY";
    private const string PARM_TELEPHONE = "@TELEPHONE";
    private const string PARM_EMAIL_ADDRESS = "@EMAIL";
    private const string PARM_NO_OF_STUDIES = "@NO_OF_STUDIES";
    private const string PARM_SERVICE_TYPE = "@SERVICE_TYPE";
    private const string PARM_STATE = "@STATE";
    private const string PARM_ADS_GROUP = "@ADS_GROUP";
    private const string PARM_HOME_PAGE_HEADER = "@HOME_PAGE_HEADER";
    private const string PARM_IS_CURRENT = "@IS_CURRENT";
    private const string PARM_UPDATE_BY_USER_ID = "@UPDATE_BY_USER_ID";
    private const string PARM_UPDATE_BY = "@UPDATE_BY";
    private const string PARM_UPDATE_DATE = "@UPDATE_DATE";

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
        new SqlParameter( EvCustomers.PARM_Guid, SqlDbType.UniqueIdentifier),
        new SqlParameter( EvCustomers.PARM_ADMIN_NAME, SqlDbType.NVarChar, 100),
        new SqlParameter( EvCustomers.PARM_ADMIN_EMAIL, SqlDbType.NVarChar, 100),
        new SqlParameter( EvCustomers.PARM_NAME, SqlDbType.NVarChar, 50),
        new SqlParameter( EvCustomers.PARM_ADDRESS_STREET_1, SqlDbType.NVarChar, 50),
        new SqlParameter( EvCustomers.PARM_ADDRESS_STREET_2, SqlDbType.NVarChar, 50),
        new SqlParameter( EvCustomers.PARM_ADDRESS_CITY, SqlDbType.NVarChar, 50),
        new SqlParameter( EvCustomers.PARM_ADDRESS_POST_CODE, SqlDbType.NVarChar, 10),
        new SqlParameter( EvCustomers.PARM_ADDRESS_STATE, SqlDbType.NVarChar, 50),
        new SqlParameter( EvCustomers.PARM_ADDRESS_COUNTRY, SqlDbType.NVarChar, 50),
        new SqlParameter( EvCustomers.PARM_TELEPHONE, SqlDbType.NVarChar, 15),
        new SqlParameter( EvCustomers.PARM_EMAIL_ADDRESS, SqlDbType.NVarChar, 100),
        new SqlParameter( EvCustomers.PARM_NO_OF_STUDIES, SqlDbType.Int),
        new SqlParameter( EvCustomers.PARM_SERVICE_TYPE, SqlDbType.NVarChar, 50),
        new SqlParameter( EvCustomers.PARM_STATE, SqlDbType.NVarChar, 50),
        new SqlParameter( EvCustomers.PARM_ADS_GROUP, SqlDbType.NVarChar, 50),
        new SqlParameter( EvCustomers.PARM_HOME_PAGE_HEADER, SqlDbType.NVarChar, 100),
        new SqlParameter( EvCustomers.PARM_IS_CURRENT, SqlDbType.Bit),
        new SqlParameter( EvCustomers.PARM_UPDATE_BY_USER_ID, SqlDbType.NVarChar, 100),
        new SqlParameter( EvCustomers.PARM_UPDATE_BY, SqlDbType.NVarChar, 100),
        new SqlParameter( EvCustomers.PARM_UPDATE_DATE, SqlDbType.DateTime)
      };

      return parms;
    }

    // =====================================================================================
    /// <summary>
    /// This class binds values to the sql query parameters arraylist. 
    /// </summary>
    /// <param name="cmdParms">SqlParameter: an arraylist of Database parameters</param>
    /// <param name="Organisation">EvCustomer: Organisation data object.</param>
    /// <remarks>
    /// This method consists of the following step: 
    /// 
    /// 1. Bind the values from Organization object to the arraylist of sql query parameters. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private void SetParameters ( SqlParameter [ ] cmdParms, EvCustomer Organisation )
    {
      cmdParms [ 0 ].Value = Organisation.Guid;
      cmdParms [ 1 ].Value = Organisation.Administrator;
      cmdParms [ 2 ].Value = Organisation.AdminEmailAddress;
      cmdParms [ 3 ].Value = Organisation.Name;
      cmdParms [ 4 ].Value = Organisation.AddressStreet_1;
      cmdParms [ 5 ].Value = Organisation.AddressStreet_2;
      cmdParms [ 6 ].Value = Organisation.AddressCity;
      cmdParms [ 7 ].Value = Organisation.AddressPostCode;
      cmdParms [ 8 ].Value = Organisation.AddressState;
      cmdParms [ 9 ].Value = Organisation.AddressCountry;
      cmdParms [ 10 ].Value = Organisation.Telephone;
      cmdParms [ 11 ].Value = Organisation.EmailAddress;
      cmdParms [ 12 ].Value = Organisation.NoOfStudies;
      cmdParms [ 13 ].Value = Organisation.ServiceType;
      cmdParms [ 14 ].Value = Organisation.State;
      cmdParms [ 15 ].Value = Organisation.AdsGroupName;
      cmdParms [ 16 ].Value = Organisation.HomePageHeader;
      cmdParms [ 17 ].Value = Organisation.Current;
      cmdParms [ 18 ].Value = Organisation.UpdatedByUserId;
      cmdParms [ 19 ].Value = Organisation.UserCommonName;
      cmdParms [ 20 ].Value = DateTime.Now;

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
    public EvCustomer readQueryRow ( DataRow Row )
    {
      // 
      // Initialise the Organisation
      // 
      EvCustomer customer = new EvCustomer ( );
      string value = string.Empty;

      // 
      // File the object.
      // 
      customer.Guid = EvSqlMethods.getGuid ( Row, EvCustomers.DB_GUID );
      customer.CustomerNo = EvSqlMethods.getInteger ( Row, EvCustomers.DB_CUSTOMER_NO );
      customer.Name = EvSqlMethods.getString ( Row, EvCustomers.DB_NAME );
      customer.AddressStreet_1 = EvSqlMethods.getString ( Row, EvCustomers.DB_ADDRESS_STREET_1 );
      customer.AddressStreet_2 = EvSqlMethods.getString ( Row, EvCustomers.DB_ADDRESS_STREET_2 );
      customer.AddressCity = EvSqlMethods.getString ( Row, EvCustomers.DB_ADDRESS_CITY );
      customer.AddressPostCode = EvSqlMethods.getString ( Row, EvCustomers.DB_ADDRESS_POSTCODE );
      customer.AddressState = EvSqlMethods.getString ( Row, EvCustomers.DB_ADDRESS_STATE );
      customer.AddressCountry = EvSqlMethods.getString ( Row, EvCustomers.DB_ADDRESS_COUNTRY );
      customer.Telephone = EvSqlMethods.getString ( Row, EvCustomers.DB_TELEPHONE );
      customer.EmailAddress = EvSqlMethods.getString ( Row, EvCustomers.DB_EMAIL );
      customer.Administrator = EvSqlMethods.getString ( Row, EvCustomers.DB_ADMIN_NAME );
      customer.AdminEmailAddress = EvSqlMethods.getString ( Row, EvCustomers.DB_ADMIN_EMAIL );
      customer.NoOfStudies = EvSqlMethods.getInteger ( Row, EvCustomers.DB_NO_OF_STUDIES );

      value = EvSqlMethods.getString ( Row, EvCustomers.DB_SERVICE_TYPE );
      if ( value != String.Empty )
      {
        customer.ServiceType = Evado.Model.EvStatics.Enumerations.parseEnumValue<EvCustomer.ServiceTypes> (
          value );
      }

       value = EvSqlMethods.getString ( Row, EvCustomers.DB_STATE );
      if ( value != String.Empty )
      {
        customer.State = Evado.Model.EvStatics.Enumerations.parseEnumValue<EvCustomer.CustomerStates> (
          value );
      }
      customer.AdsGroupName = EvSqlMethods.getString ( Row, EvCustomers.DB_ADS_GROUP );
      customer.HomePageHeader = EvSqlMethods.getString ( Row, EvCustomers.DB_HOME_PAGE_HEADER );
      customer.Administrator = EvSqlMethods.getString ( Row, EvCustomers.DB_ADMIN_NAME );
      customer.AdminEmailAddress = EvSqlMethods.getString ( Row, EvCustomers.DB_ADMIN_EMAIL );
      customer.Current = EvSqlMethods.getBool ( Row, EvCustomers.DB_IS_CURRENT );

      customer.UpdatedBy = EvSqlMethods.getString ( Row, EvCustomers.DB_UPDATED_BY );
      customer.UpdatedByUserId = EvSqlMethods.getString ( Row, EvCustomers.DB_UPDATE_BY_USER_ID);
      customer.UpdatedDate = EvSqlMethods.getDateTime ( Row, EvCustomers.DB_UPDATE_DATE );

      // 
      // Return the Organisation object.
      // 
      return customer;

    }//End readRow method.

    #endregion

    #region List Queries section

    // =====================================================================================
    /// <summary>
    /// This class returns a list of organization data object based on the passed parameters. 
    /// </summary>
    /// <param name="CustomerState">EvCustomer.CustomerStates enumerated value.</param>
    /// <param name="IsCurrent">Boolean: true, if select current organisations.</param>
    /// <returns>List of EvCustomer: a list of organization data object</returns>
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
    public List<EvCustomer> getView (
      EvCustomer.CustomerStates CustomerState,
      bool IsCurrent )
    {
      //
      // Initialize the method status, a return organization list and an IsTrue string value
      //
      this.LogMethod ( "getView method. " );

      List<EvCustomer> CustomerList = new List<EvCustomer> ( );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( EvCustomers.PARM_STATE, SqlDbType.NVarChar, 30),
      };
      cmdParms [ 0 ].Value = CustomerState;

      // 
      // Create the sql query string.
      // 
      sqlQueryString = _sqlQuery_View
        + " WHERE ( " + EvCustomers.DB_DELETED + " = 0 ) "; ;

      if ( CustomerState != EvCustomer.CustomerStates.Null )
      {
        sqlQueryString += " AND ( " + EvCustomers.DB_STATE + " = "+ EvCustomers.PARM_STATE+ " ) ";
      }
      if ( IsCurrent == true )
      {
        sqlQueryString += " AND ( "+ EvCustomers.DB_IS_CURRENT +" = 1 ) ";
      }
      sqlQueryString += "ORDER BY " + EvCustomers.DB_CUSTOMER_NO;

       this.LogDebug( sqlQueryString );

      // 
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, null ) )
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

          EvCustomer customer = this.readQueryRow ( row );

          CustomerList.Add ( customer );
        }
      }
       this.LogDebug( " View Count: " + CustomerList.Count );

      // 
      // Return the arraylist of organisations.
       // 
       this.LogMethodEnd ( "getView" );
      return CustomerList;

    }//End getView method.

    // =====================================================================================
    /// <summary>
    /// This class returns an option list based on the passed parameters. 
    /// </summary>
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
      bool IsCurrent )
    {
      //
      // Initialize the method status, a return option list, an option object and an 'IsTrue' string value
      //
      this.LogMethod ( "getList" );

      List<EvOption> list = new List<EvOption> ( );
      EvOption option = new EvOption ( );
      list.Add ( option );

      // 
      // Construct the Sql query string.
      //
      sqlQueryString = _sqlQuery_View;

      if ( IsCurrent == true )
      {
        sqlQueryString += " WHERE ( "+ EvCustomers.DB_IS_CURRENT +" = 1 ) ";
      }
      sqlQueryString += "ORDER BY " + EvCustomers.DB_CUSTOMER_NO;

      this.LogDebug ( sqlQueryString );

      // 
      // Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, null ) )
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
          option = new EvOption (
            EvSqlMethods.getString ( row, DB_GUID ),
            EvSqlMethods.getString ( row, DB_CUSTOMER_NO ) + " - " +
            EvSqlMethods.getString ( row, DB_NAME ) );

          list.Add ( option );

        }//End interation loop

      }//END using method

      this.LogDebug ( "list count: " + list.Count );

      // 
      // Return the Organisation selection visitSchedule.
      // 
      return list;

    }//END getList method

    #endregion

    #region Get Customer object section

    // =====================================================================================
    /// <summary>
    /// This class retrieves an organization data table based on Organization Guid
    /// </summary>
    /// <param name="OrgGuid">Guid: Organisation object global unique identifier</param>
    /// <returns>EvCustomer: an organisation data object</returns>
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
    public EvCustomer getItem ( Guid OrgGuid )
    {
      //
      // Initialize the method status and a return organization object. 
      //
      this.LogMethod ( "getItem method" );
       this.LogDebug( "OrgGuid: " + OrgGuid);

      EvCustomer customer = new EvCustomer ( );

      // 
      // Validate whether the Guid is not empty.
      // 
      if ( OrgGuid == Guid.Empty )
      {
        return customer;
      }

      // 
      // Define the query parameters
      // 
      SqlParameter cmdParms = new SqlParameter ( PARM_Guid, SqlDbType.UniqueIdentifier );
      cmdParms.Value = OrgGuid;

       this.LogDebug(  " SqlParameterValue: '" + cmdParms.Value + "'" );

      // 
      // Construct the Sql query string.
      // 
      sqlQueryString = _sqlQuery_View + " WHERE "+ EvCustomers.DB_GUID+ " = " + EvCustomers.PARM_Guid + "; ";

       this.LogDebug( "SQL: " + sqlQueryString );

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
           this.LogDebug(  "ROW NOT FOUND" );
         
          return customer;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        // 
        // Fill the role object.
        // 
        customer = this.readQueryRow ( row );

      }//END Using 

       this.LogDebug( "END getItem." );

      // 
      // Return Customer.
      // 
      return customer;

    }//END getItem method

    // =====================================================================================
    /// <summary>
    /// This class retrieves an organization data table based on Organization Guid
    /// </summary>
    /// <param name="Name">String: customer name</param>
    /// <returns>EvCustomer: an organisation data object</returns>
    // -------------------------------------------------------------------------------------
    public bool ItemExists ( String Name )
    {
      this.LogMethod ( "getItem method" );
      this.LogDebug ( "Name: " + Name );
      // 
      // Validate whether the Guid is not empty.
      // 
      if ( Name == String.Empty )
      {
        return false;
      }

      // 
      // Define the query parameters
      // 
      SqlParameter cmdParms = 
        new SqlParameter( PARM_ADMIN_NAME, SqlDbType.NVarChar, 100);
      cmdParms.Value = Name;

      this.LogDebug ( " SqlParameterValue: '" + cmdParms.Value + "'" );

      // 
      // Construct the Sql query string.
      // 
      sqlQueryString = _sqlQuery_View + " WHERE "+ EvCustomers.DB_NAME + " = "+ EvCustomers.PARM_ADMIN_NAME + "; ";

      this.LogDebug ( "SQL: " + sqlQueryString );

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

          return false;
        }
          return true;

      }//END Using 

    }//END getItem method

    #endregion

    #region Update Customer.

    // =====================================================================================
    /// <summary>
    /// This class updates items to the organization data object. 
    /// </summary>
    /// <param name="Customer">EvCustomer: an Organisation object</param>
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
    public EvEventCodes updateItem ( EvCustomer Customer )
    {
      this.LogMethod ( "updateItem method." );
      this.LogDebug ( "CustomerNo: " + Customer.CustomerNo );
      //
      // Initialize the method status and an old organization object
      //
      EvCustomer oldOrg = getItem ( Customer.Guid );

      //
      // Validate whether the Old organization exists. 
      //
      if ( oldOrg.Guid == Guid.Empty )
      {
         this.LogDebug( " Invalid Guid not object found." );
        return EvEventCodes.Data_InvalidId_Error;
      }

      // 
      // If the guid is null create a new guid.
      // 
      if ( Customer.Guid == Guid.Empty )
      {
         this.LogDebug( " Creating a new GUID." );
        Customer.Guid = Guid.NewGuid ( );
      }

      // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      // Compare the objects.
      // Initialize the datachange object and a backup datachanges object
      //
      EvDataChanges dataChanges = new EvDataChanges ( );
      EvDataChange dataChange = new EvDataChange ( );
      dataChange.TableName = EvDataChange.DataChangeTableNames.EvOrganisations;
      dataChange.RecordGuid = Customer.Guid;
      dataChange.UserId = Customer.UpdatedByUserId;
      dataChange.DateStamp = DateTime.Now;

      //
      // Add new items to the datachange object if they do not exist.
      //
      if ( Customer.Name != oldOrg.Name )
      {
        dataChange.AddItem ( "Name", oldOrg.Name, Customer.Name );
      }
      if ( Customer.AddressStreet_1 != oldOrg.AddressStreet_1 )
      {
        dataChange.AddItem ( "AddressStreet_1", oldOrg.AddressStreet_1, Customer.AddressStreet_1 );
      }
      if ( Customer.AddressStreet_2 != oldOrg.AddressStreet_2 )
      {
        dataChange.AddItem ( "AddressStreet_2", oldOrg.AddressStreet_2, Customer.AddressStreet_2 );
      }
      if ( Customer.AddressCity != oldOrg.AddressCity )
      {
        dataChange.AddItem ( "AddressCity", oldOrg.AddressCity, Customer.AddressCity );
      }
      if ( Customer.AddressState != oldOrg.AddressState )
      {
        dataChange.AddItem ( "AddressState", oldOrg.AddressState, Customer.AddressState );
      }
      if ( Customer.AddressStreet_1 != oldOrg.AddressStreet_1 )
      {
        dataChange.AddItem ( "AddressStreet_1", oldOrg.AddressStreet_1, Customer.AddressStreet_1 );
      }
      if ( Customer.AddressPostCode != oldOrg.AddressPostCode )
      {
        dataChange.AddItem ( "AddressPostCode", oldOrg.AddressPostCode, Customer.AddressPostCode );
      }
      if ( Customer.AddressCountry != oldOrg.AddressCountry )
      {
        dataChange.AddItem ( "AddressCountry", oldOrg.AddressCountry, Customer.AddressCountry );
      }
      if ( Customer.Telephone != oldOrg.Telephone )
      {
        dataChange.AddItem ( "Telephone", oldOrg.Telephone, Customer.Telephone );
      }
      if ( Customer.EmailAddress != oldOrg.EmailAddress )
      {
        dataChange.AddItem ( "Email", oldOrg.EmailAddress, Customer.EmailAddress );
      }
      if ( Customer.Administrator != oldOrg.Administrator )
      {
        dataChange.AddItem ( "Administrator", oldOrg.Administrator, Customer.Administrator );
      }
      if ( Customer.AdminEmailAddress != oldOrg.AdminEmailAddress )
      {
        dataChange.AddItem ( "AdminEmailAddress", oldOrg.AdminEmailAddress, Customer.AdminEmailAddress );
      }
      if ( Customer.NoOfStudies != oldOrg.NoOfStudies )
      {
        dataChange.AddItem ( "NoOfStudies", oldOrg.NoOfStudies.ToString ( ), Customer.NoOfStudies.ToString ( ) );
      }
      if ( Customer.ServiceType != oldOrg.ServiceType )
      {
        dataChange.AddItem ( "ServiceType", oldOrg.ServiceType.ToString ( ), Customer.ServiceType.ToString ( ) );
      }
      if ( Customer.State != oldOrg.State )
      {
        dataChange.AddItem ( "State", oldOrg.State.ToString ( ), Customer.State.ToString ( ) );
      }
      if ( Customer.Current != oldOrg.Current )
      {
        dataChange.AddItem ( "IsCurrent", oldOrg.Current.ToString ( ), Customer.Current.ToString ( ) );
      }

      // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      // 
      // Define the query parameters
      // 
       this.LogDebug(  " Setting Parameters." );
      SqlParameter [ ] cmdParms = GetParameters ( );
      SetParameters ( cmdParms, Customer );

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( _STORED_PROCEDURE_UpdateItem, cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      // 
      // Save the datachanges to the database.
      // 
      dataChanges.AddItem ( dataChange );
       this.LogDebug( "DataChange: " + dataChanges.Log );

      return EvEventCodes.Ok;

    }//END updateItem method

    // =====================================================================================
    /// <summary>
    /// This class adds new items to the organization data table. 
    /// </summary>
    /// <param name="Organisation">EvCustomer: Organisation object</param>
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
    public EvEventCodes addItem ( EvCustomer Organisation )
    {
      //
      // Initialize the method status and the old organization object. 
      //
      this.LogMethod ( "addItem method. " );
      this.LogDebug ( "CustomerNo: " + Organisation.CustomerNo );

      bool exists = this.ItemExists ( Organisation.Name );

      //
      // Validate whether the Old organization object's Guid is not empty.
      //
      if ( exists == true )
      {
         this.LogDebug( " Duplicate Name." );
        return EvEventCodes.Data_Duplicate_Id_Error;
      }

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
      if ( EvSqlMethods.StoreProcUpdate ( _STORED_PROCEDURE_AddItem, cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      return EvEventCodes.Ok;

    }//END addItem method 

    // =====================================================================================
    /// <summary>
    /// This method deletes the items from organization data table. 
    /// </summary>
    /// <param name="Customer">EvCustomer: an organisation data object</param>
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
    public EvEventCodes deleteItem ( EvCustomer Customer )
    {
      //
      // Initialize the method status and a trial orgnaization object. 
      //
      this.LogMethod ( "deleteItem method. " );
      this.LogDebug ( "CustomerNo: " + Customer.CustomerNo );

      // 
      // Define the query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(PARM_Guid, SqlDbType.UniqueIdentifier),
        new SqlParameter(PARM_UPDATE_BY, SqlDbType.NVarChar, 100),
        new SqlParameter(PARM_UPDATE_BY_USER_ID, SqlDbType.NVarChar, 100),
        new SqlParameter(PARM_UPDATE_DATE, SqlDbType.DateTime)
      };
      cmdParms [ 0 ].Value = Customer.Guid;
      cmdParms [ 1 ].Value = Customer.UserCommonName;
      cmdParms [ 2 ].Value = Customer.UpdatedByUserId;
      cmdParms [ 3 ].Value = DateTime.Now;

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( _STORED_PROCEDURE_DeleteItem, cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      return EvEventCodes.Ok;

    }//End deleteItem method.

    #endregion

  }//END EvCustomers class.

}//END Evado.Dal.Clinical