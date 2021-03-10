/***************************************************************************************
 * <copyright file="Dal\eClinical\EvSiteProfiles.cs" company="EVADO HOLDING PTY. LTD.">
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
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;

//Annotations to XML instrument System specific libraries

using Evado.Model;
using Evado.Model.Digital;


namespace Evado.Dal.Digital
{
  /// <summary>
  /// This class is handles the data access layer for the application profile data object.
  /// </summary>
  public class EdAdapterConfig : EvDalBase
  {
    #region class initailisation methods.
    // ==================================================================================
    /// <summary>
    /// This is the class initialisation method.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EdAdapterConfig ( )
    {
      this.ClassNameSpace = "Evado.Dal.Digital.EvApplicationProfiles.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EdAdapterConfig ( EvClassParameters Settings )
    {
      this.ClassParameters = Settings;
      this.ClassNameSpace = "Evado.Dal.Digital.EvApplicationProfiles.";

      if ( this.ClassParameters.LoggingLevel == 0 )
      {
        this.ClassParameters.LoggingLevel = Evado.Dal.EvStaticSetting.LoggingLevel;
      }
    }
    #endregion

    #region Object Initialisation
    /* *********************************************************************************
     * 
     * Defines the classes constansts and global variables
     * 
     * *********************************************************************************/
    // The log file source. 
 
    private const string SQL_ADAPTER_SETTINGS_QUERY = "Select * FROM ED_ADAPTER_SETTINGS";

    const string DB_FIELD_GUID = "AS_GUID";
    const string DB_FIELD_APPLICATION_ID = "AS_APPLICATION_ID";
    const string DB_FIELD_HOME_PAGE_HEADER = "AS_HOME_PAGE_HEADER";
    const string DB_FIELD_HELP_URL = "AS_HELP_URL";
    const string DB_FIELD_MAX_SELECTION_LENGTH = "AS_MAX_SELECTION_LENGTH";
    const string DB_FIELD_SMTP_SERVER = "AS_SMTP_SERVER";
    const string DB_FIELD_SMTP_PORT = "AS_SMTP_PORT";
    const string DB_FIELD_SMTP_USER_ID = "AS_SMTP_USER_ID";
    const string DB_FIELD_SMTP_PASSWORD = "AS_SMTP_PASSWORD";
    const string DB_FIELD_ALERT_EMAIL_ADDRESS = "AS_ALERT_EMAIL_ADDRESS";
    const string DB_FIELD_APPLICATION_URL = "AS_APPLICATION_URL";
    const string DB_FIELD_STATE = "AS_STATE";
    const string DB_FIELD_TITLE = "AS_TITLE";
    const string DB_FIELD_HTTP_REFERENCE = "AS_HTTP_REFERENCE";
    const string DB_FIELD_DESCRIPTION = "AS_DESCRIPTION";
    const string DB_FIELD_ROLES = "AS_ROLES";
    const string DB_FIELD_UPDATE_USER_ID = "AS_UPDATE_USER_ID";
    const string DB_FIELD_UPDATE_USER = "AS_UPDATE_USER";
    const string DB_FIELD_UPDATE_DATE = "AS_UPDATE_DATE";

    // 
    // Define the stored procedure names.
    // 
    private const string SQL_PROCEDURE_UPDATE = "USR_ADAPTER_SETTINGS_UPDATE";
    // 
    // Define the class constants
    // 
    const string PARM_GUID = "@GUID";
    const string PARM_APPLICATION_ID = "@APPLICATION_ID";
    const string PARM_HOME_PAGE_HEADER = "@HOME_PAGE_HEADER";
    const string PARM_HELP_URL = "@HELP_URL";
    const string PARM_MAX_SELECTION_LENGTH = "@MAX_SELECTION_LENGTH";
    const string PARM_SMTP_SERVER = "@SMTP_SERVER";
    const string PARM_SMTP_PORT = "@SMTP_PORT";
    const string PARM_SMTP_USER_ID = "@SMTP_USER_ID";
    const string PARM_SMTP_PASSWORD = "@SMTP_PASSWORD";
    const string PARM_ALERT_EMAIL_ADDRESS = "@ALERT_EMAIL_ADDRESS";
    const string PARM_APPLICATION_URL = "@APPLICATION_URL";
    const string PARM_STATE = "@STATE";
    const string PARM_TITLE = "@TITLE";
    const string PARM_HTTP_REFERENCE = "@HTTP_REFERENCE";
    const string PARM_DESCRIPTION = "@DESCRIPTION";
    const string PARM_ROLES = "@ROLES";
    const string PARM_UPDATE_USER_ID = "@UPDATE_USER_ID";
    const string PARM_UPDATE_USER = "@UPDATE_USER";
    const string PARM_UPDATE_DATE = "@UPDATE_DATE";

    #endregion

    #region Set Query Parameters

    // =====================================================================================
    /// <summary>
    /// This class return an array of sql query parameters. 
    /// </summary>
    /// <returns>SqlParameter: an array of sql query parameters</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Create an array of sql query parameters. 
    /// 
    /// 2. Return an array of sql query parameters. 
    /// </remarks>
    //  ------------------------------------------------------------------------------------
    private static SqlParameter [ ] GetParameters ( )
    {
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
	      new SqlParameter( EdAdapterConfig.PARM_GUID, SqlDbType.UniqueIdentifier ),
	      new SqlParameter( EdAdapterConfig.PARM_APPLICATION_ID, SqlDbType.Char, 1  ),
	      new SqlParameter( EdAdapterConfig.PARM_HOME_PAGE_HEADER, SqlDbType.NVarChar, 100 ),
        new SqlParameter( EdAdapterConfig.PARM_HELP_URL, SqlDbType.NVarChar, 50 ),
        new SqlParameter( EdAdapterConfig.PARM_MAX_SELECTION_LENGTH, SqlDbType.Int ),

	      new SqlParameter( EdAdapterConfig.PARM_SMTP_SERVER, SqlDbType.VarChar, 100 ),
	      new SqlParameter( EdAdapterConfig.PARM_SMTP_PORT, SqlDbType.Int ),
	      new SqlParameter( EdAdapterConfig.PARM_SMTP_USER_ID, SqlDbType.NVarChar, 100 ),
        new SqlParameter( EdAdapterConfig.PARM_SMTP_PASSWORD, SqlDbType.NVarChar, 50 ),
	      new SqlParameter( EdAdapterConfig.PARM_ALERT_EMAIL_ADDRESS, SqlDbType.NVarChar, 50 ),
        
	      new SqlParameter( EdAdapterConfig.PARM_APPLICATION_URL, SqlDbType.NVarChar, 250 ), 
	      new SqlParameter( EdAdapterConfig.PARM_TITLE, SqlDbType.NVarChar, 50 ), 
	      new SqlParameter( EdAdapterConfig.PARM_HTTP_REFERENCE, SqlDbType.NVarChar, 250 ), 
	      new SqlParameter( EdAdapterConfig.PARM_DESCRIPTION, SqlDbType.NText ), 
	      new SqlParameter( EdAdapterConfig.PARM_ROLES, SqlDbType.NVarChar, 500 ), 

	      new SqlParameter( EdAdapterConfig.PARM_UPDATE_USER_ID, SqlDbType.NVarChar,100 ), 
	      new SqlParameter( EdAdapterConfig.PARM_UPDATE_USER, SqlDbType.NVarChar, 100 ), 
	      new SqlParameter( EdAdapterConfig.PARM_UPDATE_DATE, SqlDbType.DateTime ),  
      };

      return cmdParms;
    }//END GetParameters method

    // =====================================================================================
    /// <summary>
    /// This method assigns the SiteProfile object's values to the arrray of sql parameters. 
    /// </summary>
    /// <param name="cmdParms">SqlParameter: An array of sql query parameters.</param>
    /// <param name="AdapterSettings">EvSiteProfile: A Site profile object.</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Bind the SiteProfile object's values to the array of sql query parameters. 
    /// </remarks>
    //  ------------------------------------------------------------------------------------
    private void SetParameters ( 
      SqlParameter [ ] cmdParms, 
      Evado.Model.Digital.EdAdapterSettings AdapterSettings )
    {
      cmdParms [ 0 ].Value = AdapterSettings.Guid;
      cmdParms [ 1 ].Value = "A";
      cmdParms [ 2 ].Value = AdapterSettings.HomePageHeaderText;
      cmdParms [ 3 ].Value = AdapterSettings.HelpUrl;
      cmdParms [ 4 ].Value = AdapterSettings.MaximumSelectionListLength;

      cmdParms [ 5 ].Value = AdapterSettings.SmtpServer;
      cmdParms [ 6 ].Value = AdapterSettings.SmtpServerPort;
      cmdParms [ 7 ].Value = AdapterSettings.SmtpUserId;
      cmdParms [ 8 ].Value = AdapterSettings.SmtpPassword;
      cmdParms [ 9 ].Value = AdapterSettings.EmailAlertTestAddress;

      cmdParms [ 10 ].Value = AdapterSettings.State;
      cmdParms [ 11 ].Value = AdapterSettings.Title;
      cmdParms [ 12 ].Value = AdapterSettings.HttpReference;
      cmdParms [ 13 ].Value = AdapterSettings.Description;
      cmdParms [ 14 ].Value = AdapterSettings.UserRoles;

      cmdParms [ 15 ].Value = this.ClassParameters.UserProfile.CommonName;
      cmdParms [ 16 ].Value = this.ClassParameters.UserProfile.UserId;
      cmdParms [ 17 ].Value = DateTime.Now;

    }//END SetParameters method

    #endregion

    #region Aplication Profile  Reader

    // =====================================================================================
    /// <summary>
    /// This method extracts the data reader values to the Site profile object.  
    /// </summary>
    /// <param name="Row">DataRow object.</param>
    /// <returns>EvSiteProfile: A site profile data object.</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Extract the compatible data reader object's values to the Site Profile Object. 
    /// 
    /// 2. Return a Site Profile Object. 
    /// 
    /// </remarks>
    //  ------------------------------------------------------------------------------------
    public Model.Digital.EdAdapterSettings getReaderData ( DataRow Row )
    {
      //this.LogMethod ( "getReaderData method. " );
      // 
      // Initialise method variables and objects.
      // 
      Evado.Model.Digital.EdAdapterSettings applicationSettings = new Evado.Model.Digital.EdAdapterSettings ( );

      // 
      // Load the query results into the EvProfile object.
      //
      applicationSettings.Guid = EvSqlMethods.getGuid ( Row, EdAdapterConfig.DB_FIELD_GUID );

      applicationSettings.ApplicationId = EvSqlMethods.getString ( Row, EdAdapterConfig.DB_FIELD_APPLICATION_ID );

      applicationSettings.HomePageHeaderText = EvSqlMethods.getString ( Row, EdAdapterConfig.DB_FIELD_HOME_PAGE_HEADER);

      applicationSettings.HelpUrl = EvSqlMethods.getString ( Row, EdAdapterConfig.DB_FIELD_HELP_URL );

      applicationSettings.MaximumSelectionListLength = EvSqlMethods.getInteger ( Row, EdAdapterConfig.DB_FIELD_MAX_SELECTION_LENGTH );

      applicationSettings.SmtpServer = EvSqlMethods.getString ( Row, EdAdapterConfig.DB_FIELD_SMTP_SERVER );

      applicationSettings.SmtpServerPort = EvSqlMethods.getInteger ( Row, EdAdapterConfig.DB_FIELD_SMTP_PORT );

      applicationSettings.SmtpUserId = EvSqlMethods.getString ( Row, EdAdapterConfig.DB_FIELD_SMTP_USER_ID );

      applicationSettings.SmtpPassword = EvSqlMethods.getString ( Row, EdAdapterConfig.DB_FIELD_SMTP_PASSWORD );

      applicationSettings.EmailAlertTestAddress = EvSqlMethods.getString ( Row, EdAdapterConfig.DB_FIELD_ALERT_EMAIL_ADDRESS );

      //applicationSettings.ap = EvSqlMethods.getString ( Row, EdAdapterSettings.DB_FIELD_APPLICATION_URL );

      applicationSettings.State = EvSqlMethods.getString ( Row, EdAdapterConfig.DB_FIELD_STATE );

      applicationSettings.Title = EvSqlMethods.getString ( Row, EdAdapterConfig.DB_FIELD_TITLE );

      applicationSettings.HttpReference = EvSqlMethods.getString ( Row, EdAdapterConfig.DB_FIELD_HTTP_REFERENCE );

      applicationSettings.Description = EvSqlMethods.getString ( Row, EdAdapterConfig.DB_FIELD_DESCRIPTION );

      applicationSettings.UserRoles = EvSqlMethods.getString ( Row, EdAdapterConfig.DB_FIELD_ROLES );

      applicationSettings.UpdatedBy = EvSqlMethods.getString ( Row, EdAdapterConfig.DB_FIELD_UPDATE_USER );

      applicationSettings.UpdatedByUserId = EvSqlMethods.getString ( Row, EdAdapterConfig.DB_FIELD_UPDATE_USER_ID );

      if ( applicationSettings.DemoAccountExpiryDays == 0 )
      {
        applicationSettings.DemoAccountExpiryDays = 28;
      }
      // 
      // Return the object.
      // 
      return applicationSettings;


    }//END getReaderData method


    #endregion

    #region Retrieval Queries

    // =====================================================================================
    /// <summary>
    /// This method retrieves the SiteProfile data table based on SiteId.
    /// </summary>
    /// <param name="ApplicationId">String: A site identifier.</param>
    /// <returns>EvSiteProfile: a Site Profile data object.</returns>
    /// <remarks>
    /// This method consisits of following steps. 
    /// 
    /// 1. Return an empty SiteProfile object, if the SiteId is not defined. 
    /// 
    /// 2. Define the sql query parameters and sql query string. 
    /// 
    /// 3. Execute the sql query string and store the results on data reader. 
    /// 
    /// 4. Return an empty SiteProfile object, if the data reader object has no value.
    /// 
    /// 5. Else, extract the data reader values to the SiteProfile object. 
    /// 
    /// 6. Return a SiteProfile Object. 
    /// </remarks>
    //  ------------------------------------------------------------------------------------
    public Evado.Model.Digital.EdAdapterSettings getItem ( string ApplicationId )
    {
      this.LogMethod ( "getItem method. " );

      // 
      // Define the local variables and objects.
      // 
      Evado.Model.Digital.EdAdapterSettings adapterParameters = new Evado.Model.Digital.EdAdapterSettings ( );
      String sqlQueryString = String.Empty;

      // 
      // Check that there is a valid unique identifier.
      // 
      if ( ApplicationId == String.Empty )
      {
        return adapterParameters;
      }

      // 
      // Define the query parameters.
      // 
      SqlParameter cmdParms = new SqlParameter ( EdAdapterConfig.PARM_APPLICATION_ID, SqlDbType.NVarChar, 1 );
      cmdParms.Value = ApplicationId;

      // 
      // Generate the query string.
      // 
      sqlQueryString = EdAdapterConfig.SQL_ADAPTER_SETTINGS_QUERY
        + " WHERE (" + EdAdapterConfig.DB_FIELD_APPLICATION_ID + " = " + EdAdapterConfig.PARM_APPLICATION_ID + " ); ";

      this.LogDebug ( sqlQueryString );

      // 
      // Execute the query against the database.
      // 
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
      {
        //
        // If the reader is null, retrieve from the site profile table 
        // and add record to the application profile table.
        if ( table.Rows.Count == 0 )
        {
          adapterParameters = this.getReaderData ( table.Rows [ 0 ] );

          return adapterParameters;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        adapterParameters = this.getReaderData ( row );
      }

      adapterParameters.Parameters = this.LoadObjectParameters ( adapterParameters.Guid );  

      this.LogDebug ( "Parameter list count: " + adapterParameters.Parameters.Count );

      //
      // Return an object containing an EvSiteProfile data object.
      //
      this.LogMethodEnd ( "getItem" );
      return adapterParameters;

    }//END getItemById method.

    #endregion

    #region AdapterConfig Update queries

    // =====================================================================================
    /// <summary>
    /// This method updates items to the SiteProfile data table.
    /// </summary>
    /// <param name="AdapterParameters">EvSiteProfile: A site Profile data object.</param>
    /// <returns>EvEventCodes: an event code for updating items.</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Add items to datachange object if they do not exist in Old SiteProfile object. 
    /// 
    /// 2. Define the sql query parameters and execute the storeprocedure for updating items
    /// 
    /// 3. Exit, if the storeprocedure runs fail.
    /// 
    /// 4. Add datachange object's values to the backup datachanges object. 
    /// 
    /// 5. Else, return an event code for updating items. 
    /// </remarks>
    //  ------------------------------------------------------------------------------------
    public EvEventCodes updateItem ( Evado.Model.Digital.EdAdapterSettings AdapterParameters )
    {
      this.LogMethod ( "updateItem method " );
      //
      // Create the data change object.
      //
      EvDataChanges dataChanges = new EvDataChanges ( );
      EvDataChange dataChange = this.createDataChange ( AdapterParameters );
      /*
      foreach ( EvObjectParameter prm in ApplicationSettings.Parameters )
      {
        this.LogDebug ( "Name: {0}, Value: {1}", prm.Name, prm.Value );
      }
      */

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] cmdParms = GetParameters ( );
      SetParameters ( cmdParms, AdapterParameters );

      try
      {
        //
        // Execute the update command.
        //
        if ( EvSqlMethods.StoreProcUpdate ( SQL_PROCEDURE_UPDATE, cmdParms ) == 0 )
        {
          return EvEventCodes.Database_Record_Update_Error;
        }

        //
        // Add the data change object if items have changed.
        //
        if ( dataChange.Items.Count > 0 )
        {
          dataChanges.AddItem ( dataChange );
        }
      }
      catch ( Exception ex )
      {
        this.LogValue ( Evado.Model.Digital.EvcStatics.getException ( ex ) );
      }

      //
      // Save the application parameters.
      //
      this.UpdateObjectParameters ( AdapterParameters.Parameters, AdapterParameters.Guid );

      //
      // Return an enumerated value EventCode status.
      //
      this.LogMethodEnd ( "updateItem" );
      return EvEventCodes.Ok;

    } //END updateItem method.

    //===================================================================================
    /// <summary>
    /// This method creates the data change object.
    /// 
    /// </summary>
    /// <param name="ApplicationProperties">EvApplicationProfile Object.</param>
    /// <returns>EvDataChange</returns>
    //-----------------------------------------------------------------------------------
    private EvDataChange createDataChange ( Evado.Model.Digital.EdAdapterSettings ApplicationProperties )
    {
      // 
      // Initialise the methods variables and objects.
      // 
      EvDataChange dataChange = new EvDataChange ( );

      // 
      // Get an item to be updated.
      // 
      Evado.Model.Digital.EdAdapterSettings oldItem = this.getItem ( ApplicationProperties.ApplicationId );

      // 
      // Compare the changes.
      // 
      EvDataChanges dataChanges = new EvDataChanges ( );
      dataChange.TableName = EvDataChange.DataChangeTableNames.EdAdapterSettings;
      dataChange.RecordUid = 1;
      dataChange.RecordGuid = ApplicationProperties.Guid;
      dataChange.UserId = this.ClassParameters.UserProfile.UserId;
      dataChange.DateStamp = DateTime.Now;

      //
      // Add new items to datachanges if they do not exist in the Old Site Profile object. 
      //
      if ( ApplicationProperties.ApplicationId != oldItem.ApplicationId )
      {
        dataChange.AddItem ( "Id",
          oldItem.ApplicationId,
          ApplicationProperties.ApplicationId );
      }

      //
      // Add new items to datachanges if they do not exist in the Old Site Profile object. 
      //


      dataChange.AddItem ( "MaximumSelectionListLength",
        oldItem.MaximumSelectionListLength,
        ApplicationProperties.MaximumSelectionListLength );


      dataChange.AddItem ( "SmtpServer",
        oldItem.SmtpServer,
        ApplicationProperties.SmtpServer );

      dataChange.AddItem ( "SmtpServerPort",
        oldItem.SmtpServerPort,
        ApplicationProperties.SmtpServerPort );

      dataChange.AddItem ( "SmtpUserId",
        oldItem.SmtpUserId,
        ApplicationProperties.SmtpUserId );

      dataChange.AddItem ( "EmailAlertTestAddress",
        oldItem.EmailAlertTestAddress,
        ApplicationProperties.EmailAlertTestAddress );

      //
      // Iterate through the parameters creating change object for each item.
      //
      for ( int i = 0; i < ApplicationProperties.Parameters.Count; i++ )
      {
        EvObjectParameter newParameter = ApplicationProperties.Parameters [ i ];
        //
        // Skip the non selected forms
        //
        if ( newParameter == null )
        {
          continue;
        }

        EvObjectParameter oldParameter = getParameter ( oldItem.Parameters, newParameter.Name );
        
        if ( oldParameter == null)
        {
          dataChange.AddItem ( "APP_" + newParameter.Name,
            String.Empty,
            newParameter.Value );
        }
        else
        {
          dataChange.AddItem ( "APP_" + newParameter.Name,
           oldParameter.Value,
            newParameter.Value );
        }
      }//END form list iteration loop.

      return dataChange;
    }

    /// <summary>
    /// This method returns the parameter object if exists in the parameter list.
    /// </summary>
    /// <param name="ParameterList"></param>
    /// <param name="Name"></param>
    /// <returns></returns>
    private EvObjectParameter getParameter ( List<EvObjectParameter> ParameterList, String Name )
    {
      //
      // If the list is null then return null 
      if ( ParameterList == null )
      {
        return null;
      }

      //
      // foreach item in the list return the parameter if the names match.
      //
      foreach ( EvObjectParameter parm in ParameterList )
      {
        if ( parm.Name.ToLower ( ) == Name.ToLower ( ) )
        {
          return parm;
        }
      }

      //
      // return null if the object is not found.
      return null;
    }//END getParameter method

    #endregion

  }//END EvSiteProfiles class

}//END namespace Evado.Dal.Digital
