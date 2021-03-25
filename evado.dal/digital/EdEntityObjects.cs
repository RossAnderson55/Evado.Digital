/***************************************************************************************
 * <copyright file="dal\EvFormFields.cs" company="EVADO HOLDING PTY. LTD.">
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
using System.Xml.Serialization;
using System.IO;
using System.Text;

//Application specific class references.
using Evado.Model;
using Evado.Model.Digital;

namespace Evado.Dal.Digital
{
  /// <summary>
  /// This class is handles the data access layer for the form section data object.
  /// </summary>
  public class EdEntityObjects : EvDalBase
  {
    #region class initialisation method.
    /// <summary>
    /// This method initialises the schedule DAL class.
    /// </summary>
    public EdEntityObjects ( )
    {
      this.ClassNameSpace = "Evado.Dal.Digital.EdEntityObjects.";
    }

    /// <summary>
    /// This method initialises the schedule DAL class.
    /// </summary>
    public EdEntityObjects ( EvClassParameters Settings )
    {
      this.ClassParameters = Settings;
      this.ClassNameSpace = "Evado.Dal.Digital.EdEntityObjects.";

      if ( this.ClassParameters.LoggingLevel == 0 )
      {
        this.ClassParameters.LoggingLevel = Evado.Dal.EvStaticSetting.LoggingLevel;
      }

    }

    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Object Initialisation
    /* *********************************************************************************
     * 
     * Defines the classes constansts and global variables
     * 
     * *********************************************************************************/

    /// <summary>
    /// This constant defines a sql query string for selecting all items from form field view. 
    /// </summary>
    private const string _sqlQueryView = "Select * FROM ED_ENTITY_RECORD_JOIN ";
    /*
     [EDR_GUID] [uniqueidentifier] NOT NULL,
	[EDE_GUID] [uniqueidentifier] NULL,
	[EDE_LAYOUT_ID] [nvarchar](20) NULL,
	[EDEL_TITLE] [nvarchar](100) NULL
     */
    private const string DB_RECORD_LAYOUT_GUID = "EDRL_GUID";
    private const string DB_RECORD_GUID = "EDR_GUID";
    private const string DB_ENTITY_GUID = "EDE_GUID";
    private const string DB_ENTITY_LAYOUT_ID = "EDE_LAYOUT_ID";
    private const string DB_ENTITY_TITLE = "EDEL_TITLE";


    //
    //  Define the SQL query string variable.
    //  
    private string _sqlQueryString = String.Empty;


    /// <summary>
    /// This constant defines the parameter for form global unique identifier of the formfield object
    /// </summary>
    private const string PARM_RECORD_LAYOUT_GUID = "@RECORD_LAYOUT_GUID";
    private const string PARM_RECORD_GUID = "@RECORD_GUID";
    private const string PARM_ENTITY_GUID = "@ENTITY_GUID";
    private const string PARM_ENTITY_LAYOUT_ID = "@ENTITY_LAYOUT_ID";
    private const string PARM_ENTITY_TITLE = "@ENTITY_TITLE";
    #endregion

    #region Read FormField data

    // =====================================================================================
    /// <summary>
    /// This class reads the content of the data reader object into FormField business object.
    /// </summary>
    /// <param name="Row">DataRow: an sql data query row</param>
    /// <returns>EvFormField: a form field object</returns>
    // -------------------------------------------------------------------------------------
    private Evado.Model.Digital.EdRecordEntity getRowData ( DataRow Row )
    {
      // 
      // Initialise xmltable string and a return formfield object. 
      // 
      Evado.Model.Digital.EdRecordEntity entity = new Evado.Model.Digital.EdRecordEntity ( );

      //
      // Update formfield object with the compatible data row items. 
      //
      entity.RecordLayoutGuid = EvSqlMethods.getGuid ( Row, EdEntityObjects.DB_RECORD_LAYOUT_GUID );
      entity.RecordGuid = EvSqlMethods.getGuid ( Row, EdEntityObjects.DB_RECORD_GUID );
      entity.EntityGuid = EvSqlMethods.getGuid ( Row, EdEntityObjects.DB_ENTITY_GUID );
      entity.EntityLayoutId = EvSqlMethods.getString ( Row, EdEntityObjects.DB_ENTITY_LAYOUT_ID );
      entity.EntityTitle = EvSqlMethods.getString ( Row, EdEntityObjects.DB_ENTITY_TITLE );

      return entity;

    }//END getRowData method.

    #endregion

    #region Form Field Queries

    // =====================================================================================
    /// <summary>
    /// This class returns a list of formfield items retrieving by form Guid
    /// </summary>
    /// <param name="Record">EdRecord object.</param>
    /// <returns>List of EvFormField: a list of FormField items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define the sql query parameters and a sql query string. 
    /// 
    /// 2. Execute the sql query string with parameters and store the results on data table. 
    /// 
    /// 3. Iterate through the table and extract data row to the formfield data object. 
    /// 
    /// 4. Add the object values to the Formfield list. 
    /// 
    /// 5. Return the FormFields list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EdRecordEntity> getEntityList ( EdRecord Record )
    {
      this.LogMethod ( "getEntityList method" );
      this.LogDebug ( "Record.LayoutGuid: " + Record.LayoutGuid );
      this.LogDebug ( "Record.Guid: " + Record.Guid );
      //
      // Initialize the debug log and a return list of formfield
      //
      List<EdRecordEntity> entityList = new List<EdRecordEntity> ( );

      // 
      // Define the SQL query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( EdEntityObjects.PARM_RECORD_LAYOUT_GUID, SqlDbType.UniqueIdentifier),
        new SqlParameter( EdEntityObjects.PARM_RECORD_GUID, SqlDbType.UniqueIdentifier),
      };
      cmdParms [ 0 ].Value = Record.LayoutGuid;
      cmdParms [ 0 ].Value = Record.Guid;

      // 
      // Define the query string.
      // 
      if ( Record.Guid == Guid.Empty )
      {
        _sqlQueryString = _sqlQueryView + " WHERE ( " + EdEntityObjects.DB_RECORD_LAYOUT_GUID + "  = " + EdEntityObjects.PARM_RECORD_LAYOUT_GUID + ") "
          + "ORDER BY " + EdEntityObjects.DB_ENTITY_LAYOUT_ID + "; ";
      }
      else
      {
        _sqlQueryString = _sqlQueryView + " WHERE ( " + EdEntityObjects.DB_RECORD_GUID + "  = " + EdEntityObjects.PARM_RECORD_GUID + ") "
          + "ORDER BY " + EdEntityObjects.DB_ENTITY_LAYOUT_ID + "; ";
      }

      this.LogDebug ( _sqlQueryString );

      // 
      // Scroll through the results
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

          EdRecordEntity section = this.getRowData ( row );

          entityList.Add ( section );
        }
      }
      this.LogDebug ( "Count: " + entityList.Count.ToString ( ) );

      // 
      // Pass back the result arrray.
      // 
      this.LogMethodEnd ( "getEntityList" );
      return entityList;

    }//END GetView method.

    #endregion

    #region FormFields Update queries

    // ==================================================================================
    /// <summary>
    /// This class update items on EV_FORM_SECTION table using retrieving form section items values. 
    /// </summary>
    /// <param name="Record">EvForm: a form section object</param>
    /// <returns>EvEventCodes: an event code for updating items on formfield object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Dekete the data for the old Guid. 
    /// 
    /// 2. Insert the modified data for the new Guid.
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public EvEventCodes UpdateItem ( EdRecord Record )
    {
      this.LogMethod ( "updateItem method. " );
      this.LogDebug ( "Section Count: " + Record.Design.FormSections.Count );

      //
      // Initialize the debug status and the local variables
      //
      StringBuilder sbSQL_AddQuery = new StringBuilder ( );
      List<SqlParameter> parmList = new List<SqlParameter> ( );

      if ( Record.Guid == Guid.Empty )
      {
        return EvEventCodes.Identifier_Global_Unique_Identifier_Error;
      }

      //
      // if sections doe not exist exit.
      //
      if ( Record.Entities.Count == 0 )
      {
        return EvEventCodes.Ok;
      }

      //
      // Delete the sections
      // 
      if ( Record.Guid == Guid.Empty )
      {
        sbSQL_AddQuery.AppendLine ( "DELETE FROM ED_ENTITY_RECORD_JOIN "
          + " WHERE ( " + EdEntityObjects.DB_RECORD_LAYOUT_GUID + "  = '" + Record.LayoutGuid + "') " ); ;
      }
      else
      {
        sbSQL_AddQuery.AppendLine ( "DELETE FROM ED_ENTITY_RECORD_JOIN "
        + "WHERE " + EdEntityObjects.DB_RECORD_GUID + "= '" + Record.Guid + "';  \r\n\r\n" );
      }

      for ( int count = 0; count < Record.Entities.Count; count++ )
      {
        EdRecordEntity entity = Record.Entities [ count ];

        entity.RecordGuid = Record.Guid;

        SqlParameter prm = new SqlParameter ( EdEntityObjects.PARM_RECORD_LAYOUT_GUID + "_" + count, SqlDbType.UniqueIdentifier );
        prm.Value = Record.LayoutGuid;
        parmList.Add ( prm );

        prm = new SqlParameter ( EdEntityObjects.PARM_RECORD_GUID + "_" + count, SqlDbType.UniqueIdentifier );
        prm.Value = Record.Guid;
        parmList.Add ( prm );

        prm = new SqlParameter ( EdEntityObjects.PARM_ENTITY_GUID + "_" + count, SqlDbType.UniqueIdentifier );
        prm.Value = entity.EntityGuid;
        parmList.Add ( prm );

        prm = new SqlParameter ( EdEntityObjects.PARM_ENTITY_LAYOUT_ID + "_" + count, SqlDbType.VarChar, 20 );
        prm.Value = entity.EntityLayoutId;
        parmList.Add ( prm );

        prm = new SqlParameter ( EdEntityObjects.PARM_ENTITY_TITLE + "_" + count, SqlDbType.VarChar, 100 );
        prm.Value = entity.EntityTitle;
        parmList.Add ( prm );

        //
        // Create the add query .
        //
        sbSQL_AddQuery.AppendLine ( " INSERT INTO ED_ENTITY_RECORD_JOIN  "
        + "(" + EdEntityObjects.PARM_RECORD_LAYOUT_GUID
        + ", " + EdEntityObjects.PARM_RECORD_GUID
        + ", " + EdEntityObjects.PARM_ENTITY_GUID
        + ", " + EdEntityObjects.PARM_ENTITY_LAYOUT_ID
        + ", " + EdEntityObjects.PARM_ENTITY_TITLE
        + "  )  \r\n"
        + "VALUES ("
        + ", " + EdEntityObjects.PARM_RECORD_LAYOUT_GUID + "_" + count
        + ", " + EdEntityObjects.DB_RECORD_GUID + "_" + count
        + ", " + EdEntityObjects.DB_ENTITY_GUID + "_" + count
        + ", " + EdEntityObjects.DB_ENTITY_LAYOUT_ID + "_" + count
        + ", " + EdEntityObjects.DB_ENTITY_TITLE + "_" + count + " );  \r\n" );
      }

      //
      // Convert the list to an array of SqlPararmeters.
      //
      SqlParameter [ ] parms = new SqlParameter [ parmList.Count ];

      for ( int i = 0; i < parmList.Count; i++ )
      {
        parms [ i ] = parmList [ i ];
      }


      // 
      // Extract the parameters
      //
      this.LogDebug ( "Parameters:" );
      foreach ( SqlParameter prm in parms )
      {
        this.LogDebug ( "Typ: " + prm.DbType
          + ", Parm: " + prm.ParameterName
          + ", Val: " + prm.Value );
      }
      this.LogDebug ( sbSQL_AddQuery.ToString ( ) );

      //
      // Execute the update command.
      //
      try
      {
        if ( EvSqlMethods.QueryUpdate ( sbSQL_AddQuery.ToString ( ), parms ) == 0 )
        {
          return EvEventCodes.Database_Record_Update_Error;
        }
      }
      catch ( Exception Ex )
      {
        this.LogDebug ( Evado.Model.EvStatics.getException ( Ex ) );
      }

      return EvEventCodes.Ok;

    }//END UpdateItem method


    #endregion

  }//END EvFormFields class

}//END namespace Evado.Dal.Digital
