/* <copyright file="DAL\EvRecords.cs" company="EVADO HOLDING PTY. LTD.">
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
  public class EdEntities : EvDalBase
  {
    #region class initialisation method.
    /// <summary>
    /// This method initialises the schedule DAL class.
    /// </summary>
    public EdEntities ( )
    {
      this.ClassNameSpace = "Evado.Dal.Digital.EdEntities.";
    }

    /// <summary>
    /// This method initialises the schedule DAL class.
    /// </summary>
    public EdEntities ( EvClassParameters ClassParameters )
    {
      this.ClassParameters = ClassParameters;
      this.ClassNameSpace = "Evado.Dal.Digital.EdEntities.";

    }

    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class constants and variables.

    //
    // define the entity query string.
    //
    private const string SQL_QUERY_ENTITY_VIEW = "Select * FROM ED_ENTITY_VIEW ";

    //
    // define the field filtered entity query string.
    //
    private const string DB_QUERY_ENTITY_ORG_QUERY = "Select * FROM ED_ENTITY_ORGANISATION_QUERY ";

    // 
    // Define the stored procedure names.
    // 
    private const string STORED_PROCEDURE_ENTITY_CREATE = "USR_ENTITY_CREATE";
    private const string STORED_PROCEDURE_ENTITY_DELETE = "USR_ENTITY_DELETE";
    private const string STORED_PROCEDURE_ENTITY_LOCK = "USR_ENTITY_LOCK";
    private const string STORED_PROCEDURE_ENTITY_UNLOCK = "USR_ENTITY_UNLOCK";
    private const string STORED_PROCEDURE_ENTITY_UPDATE = "USR_ENTITY_UPDATE";

    //
    // This constant defines the table fields/columns
    //
    /// <summary>
    /// The database entity guid column name
    /// </summary>
    public const string DB_ENTITY_GUID = "EDE_GUID";
    /// <summary>
    /// The database entity state column name
    /// </summary>
    public const string DB_STATE = "EDE_STATE";
    /// <summary>
    /// The database entity entity identifier column name
    /// </summary>
    public const string DB_ENTITY_ID = "EDE_ENTITY_ID";
    /// <summary>
    /// The database entity source identifier column name
    /// </summary>
    public const string DB_SOURCE_ID = "EDE_SOURCE_ID";
    /// <summary>
    /// The database entity entity date column name
    /// </summary>
    public const string DB_ENTITY_DATE = "EDE_ENTITY_DATE";
    /// <summary>
    /// The database entity parent organisatiOn identifier column name
    /// </summary>
    public const string DB_PARENT_ORG_ID = "EDE_PARENT_ORG_ID";
    /// <summary>
    /// The database entity parent user identifier column name
    /// </summary>
    public const string DB_PARENT_USER_ID = "EDE_PARENT_USER_ID";
    /// <summary>
    /// The database entity author user identifier column name
    /// </summary>
    public const string DB_AUTHOR_USER_ID = "EDE_AUTHOR_USER_ID";
    /// <summary>
    /// The database entity author common name  column name
    /// </summary>
    public const string DB_AUTHOR = "EDE_AUTHOR";
    /// <summary>
    /// The database entity parent layout identifier column name
    /// </summary>
    public const string DB_PARENT_LAYOUT_ID = "EDE_PARENT_LAYOUT_ID";

    /// <summary>
    /// The database entity parent object guid column name
    /// </summary>
    public const string DB_PARENT_GUID = "EDE_PARENT_GUID";
    /// <summary>
    /// The database entity entity access column name
    /// </summary>
    public const string DB_ENTITY_ACCESS = "EDE_ENTITY_ACCESS";
    /// <summary>
    /// The database entity data collection event identifer column name
    /// </summary>
    public const string DB_COLLECTION_EVENT_ID = "EDE_COLLECTION_EVENT_ID";
    /// <summary>
    /// The database entity AI index column name
    /// </summary>
    public const string DB_AI_DATA_INDEX = "EDE_AI_DATA_INDEX";
    /// <summary>
    /// The database entity entity visibility column name
    /// </summary>
    public const string DB_VISABILITY = "EDE_VISABILITY";
    /// <summary>
    /// The database entity signoff column name
    /// </summary>
    public const string DB_SIGN_OFFS = "EDE_SIGN_OFFS";
    /// <summary>
    /// This database entity layout field Id filter 0
    /// </summary>
    public const string DB_FILTER_VALUE_0 = "EDE_FILTER_VALUE_0";
    /// <summary>
    /// This database entity layout field Id filter 1
    /// </summary>
    public const string DB_FILTER_VALUE_1 = "EDE_FILTER_VALUE_1";
    /// <summary>
    /// This database entity layout field Id filter 2
    /// </summary>
    public const string DB_FILTER_VALUE_2 = "EDE_FILTER_VALUE_2";
    /// <summary>
    /// This database entity layout field Id filter 3
    /// </summary>
    public const string DB_FILTER_VALUE_3 = "EDE_FILTER_VALUE_3";
    /// <summary>
    /// This database entity layout field Id filter 4
    /// </summary>
    public const string DB_FILTER_VALUE_4 = "EDE_FILTER_VALUE_4";
    /// <summary>
    /// The database entity booked out user identifier column name
    /// </summary>
    public const string DB_BOOKED_OUT_USER_ID = "EDE_BOOKED_OUT_USER_ID";
    /// <summary>
    /// The database entity booked out user common name column name
    /// </summary>
    public const string DB_BOOKED_OUT = "EDE_BOOKED_OUT";
    /// <summary>
    /// The database entity updates by user identifier column name
    /// </summary>
    public const string DB_UPDATED_BY_USER_ID = "EDE_UPDATED_BY_USER_ID";
    /// <summary>
    /// The database entity update by user common name column name
    /// </summary>
    public const string DB_UPDATED_BY = "EDE_UPDATED_BY";
    /// <summary>
    /// The database entity update date column name
    /// </summary>
    public const string DB_UPDATED_DATE = "EDE_UPDATED_DATE";
    /// <summary>
    /// The database entity deleted column name
    /// </summary>
    public const string DB_DELETED = "EDE_DELETED";

    //
    // The field and parameter values for the SQl customer filter 
    //
    private const string PARM_ENTITY_GUID = "@GUID";
    private const string PARM_STATE = "@STATE";
    private const string PARM_ENTITY_ID = "@ENTITY_ID";
    private const string PARM_SOURCE_ID = "@SOURCE_ID";
    private const string PARM_ENTITY_DATE = "@ENTITY_DATE";
    private const string PARM_PARENT_ORG_ID = "@PARENT_ORG_ID";
    private const string PARM_PARENT_USER_ID = "@PARENT_USER_ID";
    private const string PARM_AUTHOR_USER_ID = "@AUTHOR_USER_ID";
    private const string PARM_AUTHOR = "@AUTHOR";
    private const string PARM_PARENT_LAYOUT_ID = "@PARENT_LAYOUT_ID";
    private const string PARM_PARENT_GUID = "@PARENT_GUID";
    private const string PARM_ENTITY_ACCESS = "@ENTITY_ACCESS";
    private const string PARM_COLLECTION_EVENT_ID = "@COLLECTION_EVENT_ID";
    private const string PARM_AI_DATA_INDEX = "@AI_DATA_INDEX";
    private const string PARM_VISABILITY = "@VISABILITY";
    private const string PARM_SIGN_OFFS = "@SIGN_OFFS";
    private const string PARM_BOOKED_OUT_USER_ID = "@BOOKED_OUT_USER_ID";
    private const string PARM_BOOKED_OUT = "@BOOKED_OUT";
    private const string PARM_UPDATED_BY_USER_ID = "@UPDATED_BY_USER_ID";
    private const string PARM_UPDATED_BY = "@UPDATED_BY";
    private const string PARM_UPDATED_DATE = "@UPDATED_DATE";

    // 
    // Used for the ItemQuery
    // 
    private const string PARM_FIELD_ID = "@FIELD_ID";
    private const string PARM_TEXT_VALUE = "@TEXT_VALUE";
    private const string PARM_TYPE_ID = "@TYPE_ID";

    private const string DB_FILTER_VALUE_ = "EDE_FILTER_VALUE_";
    private const string PARM_FILTER_VALUE = "@FILTER_VALUE_";

    private const string PARM_FILTER_VALUE_0 = "@FILTER_VALUE_0";
    private const string PARM_FILTER_VALUE_1 = "@FILTER_VALUE_1";
    private const string PARM_FILTER_VALUE_2 = "@FILTER_VALUE_2";
    private const string PARM_FILTER_VALUE_3 = "@FILTER_VALUE_3";
    private const string PARM_FILTER_VALUE_4 = "@FILTER_VALUE_4";

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
        new SqlParameter( EdEntities.PARM_ENTITY_GUID, SqlDbType.UniqueIdentifier),
        new SqlParameter( EdEntities.PARM_STATE, SqlDbType.VarChar, 20),
        new SqlParameter( EdEntities.PARM_SOURCE_ID, SqlDbType.NVarChar, 40), 
        new SqlParameter( EdEntities.PARM_ENTITY_DATE, SqlDbType.DateTime),
        new SqlParameter( EdEntities.PARM_VISABILITY, SqlDbType.NVarChar, 30), 
        new SqlParameter( EdEntities.PARM_ENTITY_ACCESS, SqlDbType.VarChar, 1000),
        new SqlParameter( EdEntities.PARM_AI_DATA_INDEX, SqlDbType.NVarChar, 1000), 
        new SqlParameter( EdEntities.PARM_SIGN_OFFS, SqlDbType.NVarChar),
        new SqlParameter( EdEntities.PARM_FILTER_VALUE_0, SqlDbType.NVarChar, 250),
        new SqlParameter( EdEntities.PARM_FILTER_VALUE_1, SqlDbType.NVarChar, 250),
        new SqlParameter( EdEntities.PARM_FILTER_VALUE_2, SqlDbType.NVarChar, 250),
        new SqlParameter( EdEntities.PARM_FILTER_VALUE_3, SqlDbType.NVarChar, 250),
        new SqlParameter( EdEntities.PARM_FILTER_VALUE_4, SqlDbType.NVarChar, 250),
        new SqlParameter( EdEntities.PARM_UPDATED_BY_USER_ID, SqlDbType.NVarChar, 100),
        new SqlParameter( EdEntities.PARM_UPDATED_BY, SqlDbType.NVarChar, 100),
        new SqlParameter( EdEntities.PARM_UPDATED_DATE, SqlDbType.DateTime),
      };
      return cmdParms;
    }

    // =====================================================================================
    /// <summary>
    /// This class binds values to query parameters.
    /// </summary>
    /// <param name="CommandParameters">SqlParameter: an array of sql parameters</param>
    /// <param name="Entity">EvForm: a form object containing the parmeter values.</param>
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
      EdRecord Entity )
    {
      // 
      // Set the record date if is not already set.
      // 
      if ( Entity.RecordDate == Evado.Model.EvStatics.CONST_DATE_NULL )
      {
        Entity.RecordDate = DateTime.Now;
      }

      // 
      // Set the Global identifier if not set.
      // 
      if ( Entity.Guid == Guid.Empty )
      {
        Entity.Guid = Guid.NewGuid ( );
      }


      /************************************************************************************
       * 
       * The multi-field selection function, uses summary fields that are single or 
       * multi-selection fields (selection list, radio-buttons, checkbox-list), to create 
       * Entity selections based on these field values.  The property below stored the 
       * fieldIds for 5 summary selection fields from the entity.  
       * 
       * The array is automatically generated, by iterating through the summary fields,
       * adding the selection field types to the list.
       * 
       * This list is then used to identify the selection fields to be created to select 
       * the Entities based on their these field's values.
       * 
       * The array of fild ids are then use to extract the field values and save them in
       * the ED_Entities table to enable the multi-part selection to be performed.
       * 
       ************************************************************************************/
      //
      // define the filter field value array.
      // 
      String [ ] filterFieldValue = new string [ 5 ];
      
      //
      // iterate through the fields updating the filterFieldValue values.
      //
      for ( int fieldCount = 0; fieldCount < Entity.FilterFieldIds.Length; fieldCount++ )
      {
        String fieldId = Entity.FilterFieldIds [ fieldCount ];
        filterFieldValue [ fieldCount ] = String.Empty;

        if ( fieldId == String.Empty )
        {
          continue;
        }

        //
        // iterate through the entity fields.
        //
        foreach ( EdRecordField field in Entity.Fields )
        {
          if ( field.FieldId != fieldId )
          {
            continue;
          }

          //
          // update the filter value list array if the field identifier match.
          //
          filterFieldValue [ fieldCount ] = field.ItemValue;

        }//End Field iteration loop.

      }//End FilterFieldId iteration loop.

      // 
      // Load the command parmameter values
      // 
      CommandParameters [ 0 ].Value = Entity.Guid;
      CommandParameters [ 1 ].Value = Entity.State;
      CommandParameters [ 2 ].Value = Entity.SourceId;
      CommandParameters [ 3 ].Value = Entity.RecordDate;
      CommandParameters [ 4 ].Value = Entity.Visabilty;
      CommandParameters [ 5 ].Value = Entity.EntityAccess;
      CommandParameters [ 6 ].Value = Entity.AiIndex;
      CommandParameters [ 7 ].Value = Evado.Model.EvStatics.SerialiseObject<List<EdUserSignoff>> ( Entity.Signoffs );
      CommandParameters [ 8 ].Value = filterFieldValue [ 0 ];
      CommandParameters [ 9 ].Value = filterFieldValue [ 1 ];
      CommandParameters [ 10 ].Value = filterFieldValue [ 2 ];
      CommandParameters [ 11 ].Value = filterFieldValue [ 3 ];
      CommandParameters [ 12 ].Value = filterFieldValue [ 4 ];
      CommandParameters [ 13 ].Value = this.ClassParameters.UserProfile.UserId;
      CommandParameters [ 14 ].Value = this.ClassParameters.UserProfile.CommonName;
      CommandParameters [ 15 ].Value = DateTime.Now;


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
        new SqlParameter( EdEntities.PARM_ENTITY_GUID, SqlDbType.UniqueIdentifier),
        new SqlParameter( EdEntityLayouts.PARM_LAYOUT_ID, SqlDbType.NVarChar, 10),
        new SqlParameter( EdEntities.PARM_PARENT_LAYOUT_ID, SqlDbType.NVarChar, 10),
        new SqlParameter( EdEntities.PARM_PARENT_GUID, SqlDbType.UniqueIdentifier),
        new SqlParameter( EdEntities.PARM_PARENT_USER_ID, SqlDbType.VarChar, 100),
        new SqlParameter( EdEntities.PARM_PARENT_ORG_ID, SqlDbType.VarChar, 20),
        new SqlParameter( EdEntities.PARM_AUTHOR_USER_ID, SqlDbType.VarChar, 100),
        new SqlParameter( EdEntities.PARM_AUTHOR, SqlDbType.VarChar, 100),
        new SqlParameter( EdEntities.PARM_COLLECTION_EVENT_ID, SqlDbType.NVarChar, 20),
        new SqlParameter( EdEntities.PARM_UPDATED_BY_USER_ID, SqlDbType.NVarChar, 100),
        new SqlParameter( EdEntities.PARM_UPDATED_BY, SqlDbType.NVarChar, 100),
        new SqlParameter( EdEntities.PARM_UPDATED_DATE, SqlDbType.DateTime),
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
      CommandParameters [ 2 ].Value = Record.ParentLayoutId;
      CommandParameters [ 3 ].Value = Record.ParentGuid;
      CommandParameters [ 4 ].Value = Record.ParentUserId;
      CommandParameters [ 5 ].Value = Record.ParentOrgId;
      CommandParameters [ 6 ].Value = this.ClassParameters.UserProfile.UserId;
      CommandParameters [ 7 ].Value = Record.Author;
      CommandParameters [ 8 ].Value = Record.DataCollectEventId;
      CommandParameters [ 9 ].Value = this.ClassParameters.UserProfile.UserId;
      CommandParameters [ 10 ].Value = this.ClassParameters.UserProfile.CommonName;
      CommandParameters [ 11 ].Value = DateTime.Now;

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
        new SqlParameter( EdEntities.PARM_ENTITY_GUID, SqlDbType.UniqueIdentifier),
        new SqlParameter( EdEntities.PARM_UPDATED_BY_USER_ID, SqlDbType.NVarChar, 100),
        new SqlParameter( EdEntities.PARM_UPDATED_BY, SqlDbType.NVarChar, 100),
        new SqlParameter( EdEntities.PARM_UPDATED_DATE, SqlDbType.DateTime),
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
      EdRecord entity = new EdRecord ( );

      entity.Design.IsEntity = true;

      // 
      // Update the trial record object with compatible values from datarow.
      // 
      entity.Guid = EvSqlMethods.getGuid ( Row, EdEntities.DB_ENTITY_GUID );
      entity.LayoutGuid = EvSqlMethods.getGuid ( Row, EdEntityLayouts.DB_LAYOUT_GUID );
      entity.SourceId = EvSqlMethods.getString ( Row, EdEntities.DB_SOURCE_ID );
      entity.LayoutId = EvSqlMethods.getString ( Row, EdEntityLayouts.DB_LAYOUT_ID );
      entity.RecordId = EvSqlMethods.getString ( Row, EdEntities.DB_ENTITY_ID );
      entity.Design.Title = EvSqlMethods.getString ( Row, EdEntityLayouts.DB_TITLE );
      entity.State = Evado.Model.EvStatics.parseEnumValue<EdRecordObjectStates> (
        EvSqlMethods.getString ( Row, EdEntities.DB_STATE ) );
      entity.RecordDate = EvSqlMethods.getDateTime ( Row, EdEntities.DB_ENTITY_DATE );

      entity.ParentUserId = EvSqlMethods.getString ( Row, EdEntities.DB_PARENT_USER_ID );
      entity.ParentOrgId = EvSqlMethods.getString ( Row, EdEntities.DB_PARENT_ORG_ID );
      entity.Author = EvSqlMethods.getString ( Row, EdEntities.DB_AUTHOR_USER_ID );
      entity.AuthorUserId = EvSqlMethods.getString ( Row, EdEntities.DB_AUTHOR_USER_ID );

      entity.Visabilty = EvSqlMethods.getString<EdRecord.VisabilityList> ( Row, EdEntities.DB_VISABILITY );
      entity.EntityAccess = EvSqlMethods.getString ( Row, EdEntities.DB_ENTITY_ACCESS );

      entity.ParentLayoutId = EvSqlMethods.getString ( Row, EdEntities.DB_PARENT_LAYOUT_ID );
      entity.ParentGuid = EvSqlMethods.getGuid ( Row, EdEntities.DB_PARENT_GUID );
      entity.DataCollectEventId = EvSqlMethods.getString ( Row, EdEntities.DB_COLLECTION_EVENT_ID );

      string value = EvSqlMethods.getString ( Row, EdEntityLayouts.DB_LINK_CONTENT_SETTING );
      if ( value != String.Empty )
      {
        entity.Design.LinkContentSetting =
          Evado.Model.EvStatics.parseEnumValue<EdRecord.LinkContentSetting> ( value );
      }

      entity.FilterFieldIds [ 0 ] = EvSqlMethods.getString ( Row, EdEntityLayouts.DB_FILTER_FIELD_0 );
      entity.FilterFieldIds [ 1 ] = EvSqlMethods.getString ( Row, EdEntityLayouts.DB_FILTER_FIELD_1 );
      entity.FilterFieldIds [ 2 ] = EvSqlMethods.getString ( Row, EdEntityLayouts.DB_FILTER_FIELD_2 );
      entity.FilterFieldIds [ 3 ] = EvSqlMethods.getString ( Row, EdEntityLayouts.DB_FILTER_FIELD_3 );
      entity.FilterFieldIds [ 4 ] = EvSqlMethods.getString ( Row, EdEntityLayouts.DB_FILTER_FIELD_4 );

      //
      // Skip detailed content if a queryState query
      //
      if ( SummaryQuery == false )
      {
        entity.Design.HttpReference = EvSqlMethods.getString ( Row, EdEntityLayouts.DB_HTTP_REFERENCE );
        entity.Design.Instructions = EvSqlMethods.getString ( Row, EdEntityLayouts.DB_INSTRUCTIONS );
        entity.Design.Description = EvSqlMethods.getString ( Row, EdEntityLayouts.DB_DESCRIPTION );
        entity.Design.UpdateReason = Evado.Model.EvStatics.parseEnumValue<EdRecord.UpdateReasonList> (
          EvSqlMethods.getString ( Row, EdEntityLayouts.DB_UPDATE_REASON ) );
        entity.Design.RecordCategory = EvSqlMethods.getString ( Row, EdEntityLayouts.DB_RECORD_CATEGORY );

        entity.Design.TypeId = Evado.Model.EvStatics.parseEnumValue<EdRecordTypes> (
           EvSqlMethods.getString ( Row, EdEntityLayouts.DB_TYPE_ID ) );
        entity.Design.Version = EvSqlMethods.getFloat ( Row, EdEntityLayouts.DB_VERSION );

        entity.Design.JavaScript = EvSqlMethods.getString ( Row, EdEntityLayouts.DB_JAVA_SCRIPT );
        entity.Design.hasCsScript = EvSqlMethods.getBool ( Row, EdEntityLayouts.DB_HAS_CS_SCRIPT );
        entity.Design.Language = EvSqlMethods.getString ( Row, EdEntityLayouts.DB_LANGUAGE );
        entity.Design.ReadAccessRoles = EvSqlMethods.getString ( Row, EdEntityLayouts.DB_READ_ACCESS_ROLES );
        entity.Design.EditAccessRoles = EvSqlMethods.getString ( Row, EdEntityLayouts.DB_EDIT_ACCESS_ROLES );

        entity.Design.ParentEntities = EvSqlMethods.getString ( Row, EdEntityLayouts.DB_PARENT_ENTITIES );
        entity.Design.DefaultPageLayout = EvSqlMethods.getString ( Row, EdEntityLayouts.DB_DEFAULT_PAGE_LAYOUT );
        entity.Design.DisplayRelatedEntities = EvSqlMethods.getBool ( Row, EdEntityLayouts.DB_DISPLAY_ENTITIES );
        entity.Design.DisplayAuthorDetails = EvSqlMethods.getBool ( Row, EdEntityLayouts.DB_DISPLAY_AUTHOR_DETAILS );
        entity.Design.RecordPrefix = EvSqlMethods.getString ( Row, EdEntityLayouts.DB_ENTITY_PREFIX );
        entity.Design.ParentType = EvSqlMethods.getString<EdRecord.ParentTypeList> ( Row, EdEntityLayouts.DB_PARENT_TYPE );
        entity.Design.AuthorAccess = EvSqlMethods.getString<EdRecord.AuthorAccessList> ( Row, EdEntityLayouts.DB_PARENT_ACCESS );
        entity.Design.ParentEntities = EvSqlMethods.getString ( Row, EdEntityLayouts.DB_PARENT_ENTITIES );
        entity.Design.HeaderFormat = EvSqlMethods.getString<EdRecord.HeaderFormat> ( Row, EdEntityLayouts.DB_HEADER_FORMAT );
        entity.Design.FooterFormat = EvSqlMethods.getString<EdRecord.FooterFormat> ( Row, EdEntityLayouts.DB_FOOTER_FORMAT );

        entity.Design.FieldReadonlyDisplayFormat = EvSqlMethods.getString<EdRecord.FieldReadonlyDisplayFormats> ( 
          Row, EdEntityLayouts.DB_FIELD_DISPLAY_FORMAT );

        entity.Updated = EvSqlMethods.getString ( Row, EdEntities.DB_UPDATED_BY );
        if ( entity.Updated != string.Empty )
        {
          entity.Updated += " on " + EvSqlMethods.getDateTime ( Row, EdEntities.DB_UPDATED_DATE ).ToString ( "dd MMM yyyy HH:mm" );
        }
        entity.BookedOutBy = EvSqlMethods.getString ( Row, EdEntities.DB_BOOKED_OUT );

        //
        // fill the comment list.
        //
        this.fillCommentList ( entity );

        string approved = "Version: " + entity.Design.Version.ToString ( "0.00" );
        approved += " by: " + EvSqlMethods.getString ( Row, EdEntityLayouts.DB_UPDATED_BY );
        approved += " on " + EvSqlMethods.getDateTime ( Row, EdEntityLayouts.DB_UPDATED_DATE ).ToString ( "dd MMM yyyy" );
        entity.Design.Approval = approved;
      }

      return entity;

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

    #region Entity views and query methods.

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
      String sqlQueryString = String.Empty;
      int inResultCount = 0;

      // 
      // Define the query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( EdEntityLayouts.PARM_LAYOUT_ID, SqlDbType.NVarChar, 10),
      };
      cmdParms [ 0 ].Value = QueryParameters.LayoutId;

      //
      // Generate the SQL query string.
      //
      sqlQueryString = this.createSqlQueryStatement ( QueryParameters );
      this.LogValue ( sqlQueryString );

      this.LogValue ( " Execute Query" );
      //
      //Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
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

    } // Close getRecordCount method.

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
    public List<EdRecord> getEntityList (
      EdQueryParameters QueryParameters )
    {
      this.LogMethod ( "getEntityList" );
      this.LogDebug ( "Org_City = {0}", QueryParameters.Org_City );
      this.LogDebug ( "Org_Country = {0}", QueryParameters.Org_Country );
      //
      // Initialize the method debug log, a return form list and a number of result count. 
      //
      List<EdRecord> view = new List<EdRecord> ( );
      String sqlQueryString = String.Empty;
      int inResultCount = 0;

      if ( QueryParameters.Org_City.Contains ( "_" ) == true )
      {
        String [ ] arrCity = QueryParameters.Org_City.Split ( '_' );
        QueryParameters.Org_Country = arrCity [ 0 ];
        QueryParameters.Org_City = arrCity [ 1 ];

       // this.LogDebug ( "Country {0} ", QueryParameters.Org_Country );
       // this.LogDebug ( "City {0} ", QueryParameters.Org_City );
      }

      if ( QueryParameters.Org_PostCode.Contains ( "_" ) == true )
      {
        String [ ] arrCity = QueryParameters.Org_PostCode.Split ( '_' );
        QueryParameters.Org_Country = arrCity [ 0 ];
        QueryParameters.Org_PostCode = arrCity [ 1 ];

       // this.LogDebug ( "Country {0} ", QueryParameters.Org_Country );
       // this.LogDebug ( "PostCode {0} ", QueryParameters.Org_PostCode );
      }

      // 
      // Define the query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( EdRecordLayouts.PARM_LAYOUT_ID, SqlDbType.NVarChar, 10),
        new SqlParameter( EdOrganisations.PARM_Address_City, SqlDbType.NVarChar, 50),
        new SqlParameter( EdOrganisations.PARM_COUNTRY, SqlDbType.NVarChar, 50),
      };
      cmdParms [ 0 ].Value = QueryParameters.LayoutId;
      cmdParms [ 1 ].Value = QueryParameters.Org_City;
      cmdParms [ 2 ].Value = QueryParameters.Org_Country;

      //
      // Generate the SQL query string.
      //
      sqlQueryString = this.createSqlQueryStatement ( QueryParameters );

     // this.LogDebug ( EvSqlMethods.getParameterSqlText ( cmdParms ) );

     // this.LogDebug ( sqlQueryString );

     // this.LogDebug ( "Execute Query" );
      //
      //Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
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

          EdRecord entity = this.getRowData ( row, QueryParameters.IncludeSummary );

         // this.LogDebug ( "record.Design.LinkContentSetting {0}.", record.Design.LinkContentSetting );
          // 
          // Attach fields and other trial data.
          // 
          if ( QueryParameters.IncludeRecordValues == true
            || entity.Design.LinkContentSetting == EdRecord.LinkContentSetting.First_Text_Field )
          {
            if ( inResultCount < QueryParameters.ResultStartRange
              || inResultCount >= ( QueryParameters.ResultFinishRange ) )
            {
              //this.LogDebug ( "Count: " + inResultCount + " >> not within record range." );

              //
              // Increment the result count.
              //
              inResultCount++;

              continue;
            }

            if ( entity.hasReadAccess ( this.ClassParameters.UserProfile.Roles ) == false )
            {
              this.LogDebug ( "User Role: {0}, does no have access to {1}, roles: {2}",
                this.ClassParameters.UserProfile.Roles,
                entity.LayoutId, 
                entity.Design.ReadAccessRoles );
              continue;
            }

            // 
            // Get the trial record fields
            // 
            this.GetEntityValues ( entity );

            //
            // Attach the entity list.
            //
            this.getRecordEntities ( entity );

            //
            // attach the record sections.
            //
            this.GetRecordSections ( entity );

            //
            // Increment the result count.
            //
            inResultCount++;

            // 
          }//END query selection state not set.

          view.Add ( entity );

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
      this.LogMethod ( "createSqlQueryStatement method." );
      this.LogDebug ( "EnableOrganisationFilter {0}.", QueryParameters.EnableOrganisationFilter );
      //
      // Initialize the local sql query string. 
      //
      StringBuilder sqlQueryString = new StringBuilder ( );


      //
      // Select the query view based on whether the organisation filter is operational.
      //
      if ( QueryParameters.EnableOrganisationFilter == false )
      {
        sqlQueryString.AppendLine ( EdEntities.SQL_QUERY_ENTITY_VIEW );
        sqlQueryString.AppendLine ( " WHERE ( ( " + EdEntities.DB_DELETED + " = 0 )" );
      }
      else
      {
        sqlQueryString.AppendLine ( EdEntities.DB_QUERY_ENTITY_ORG_QUERY );
        sqlQueryString.AppendLine ( " WHERE ( ( " + EdEntities.DB_DELETED + " = 0 )" );
      }

      //
      // filter by layout id if provided.
      //
      if ( QueryParameters.LayoutId != String.Empty )
      {
        sqlQueryString.AppendLine ( " AND ( " + EdEntityLayouts.DB_LAYOUT_ID + " = " + EdEntityLayouts.PARM_LAYOUT_ID + " ) " );
      }

      //
      // use organisation filters if organisation filter is enabled.
      //
      if ( QueryParameters.EnableOrganisationFilter == true )
      {
        //
        // filter by city if provided.
        //
        if ( QueryParameters.Org_City != String.Empty )
        {
          sqlQueryString.AppendLine ( " AND ( " + EdOrganisations.DB_ADDRESS_CITY + " = " + EdOrganisations.PARM_Address_City + " ) " );
        }

        //
        // filter by country if provided.
        //
        if ( QueryParameters.Org_Country != String.Empty )
        {
          sqlQueryString.AppendLine ( " AND ( " + EdOrganisations.DB_COUNTRY + " = " + EdOrganisations.PARM_COUNTRY + " ) " );
        }

        //
        // filter by country if provided.
        //
        if ( QueryParameters.Org_PostCode != String.Empty )
        {
          sqlQueryString.AppendLine ( " AND ( " + EdOrganisations.DB_ADDRESS_POST_CODE + " = " + EdOrganisations.PARM_ADDRESS_POST_CODE + " ) " );
        }
      }

      /************************************************************************************
       * 
       * The multi-field selection function, uses summary fields that are single or 
       * multi-selection fields (selection list, radio-buttons, checkbox-list), to create 
       * Entity selections based on these field values.  The property below stored the 
       * fieldIds for 5 summary selection fields from the entity.  
       * 
       * The array is automatically generated, by iterating through the summary fields,
       * adding the selection field types to the list.
       * 
       * This list is then used to identify the selection fields to be created to select 
       * the Entities based on their these field's values.
       * 
       * The selected field values stored in the Entity table when it is updated and the 
       * filter values then used to select the relevant Entity.
       * 
       ************************************************************************************/
      if ( QueryParameters.SelectionFilters [ 0 ] != null )
      {
        this.LogDebug ( "Selection filter query enabled." );
        //
        // iterate through the filter values creating a query element for each.
        //
        for ( int filterIndex = 0; filterIndex < QueryParameters.SelectionFilters.Length; filterIndex++ )
        {
          //
          // skip filters that are not set.
          //
          if ( QueryParameters.SelectionFilters [ filterIndex ] == null )
          {
            continue;
          }
          if ( QueryParameters.SelectionFilters [ filterIndex ] == String.Empty )
          {
            continue;
          }

          //
          // extract the filter value.
          //
          String value = QueryParameters.SelectionFilters [ filterIndex ];

          this.LogDebug ( "Index {0} = {1}.", filterIndex, value );

          //
          // determine if the filter value is multi-part or single value.
          //
          if ( value.Contains ( ";" ) == false )
          {
            LogDebug ( "Single value filter" );

            sqlQueryString.AppendLine ( " AND ( "
              + EdEntities.DB_FILTER_VALUE_ + filterIndex + " = '" + value + "' ) " );
          }
          else
          {
            LogDebug ( "Multi value filter value" );

            //
            // split value into an array of values.
            //
            string [ ] arValues = value.Split ( ';' );

            //
            // create a filter for each part value using the like statement as
            // the value will be in a multi-part format of ';' separated values.
            //
            foreach ( string str in arValues )
            {
              LogDebug ( "-Value {0}", str );

              sqlQueryString.AppendLine ( " AND ( "
                + EdEntities.DB_FILTER_VALUE_ + filterIndex + " like '%" + str + "%' ) " );
            }

          }//END multi-part filter value.
        }//END filter selection value iteration loop.

        sqlQueryString.AppendLine ( ") ORDER BY " + EdEntities.DB_ENTITY_ID + ";" );

        //
        // Return the sql query string. 
        //
        return sqlQueryString.ToString ( );
      }//END field filter selection.

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
            sqlQueryString.AppendLine ( " AND NOT ( " + EdEntities.DB_STATE + "= '" + EdRecordObjectStates.Withdrawn + "' ) " );
          }
          else
          {
            if ( QueryParameters.States [ 0 ] == EdRecordObjectStates.Draft_Record )
            {
              sqlQueryString.AppendLine ( StateSelection + EdEntities.DB_STATE + "= '" + EdRecordObjectStates.Draft_Record + "' "
              + "OR  " + EdEntities.DB_STATE + " = '" + EdRecordObjectStates.Empty_Record + "' "
              + "OR " + EdEntities.DB_STATE + " = '" + EdRecordObjectStates.Completed_Record + "') " );
            }
            else
            {
              sqlQueryString.AppendLine ( StateSelection + EdEntities.DB_STATE + " = '" + QueryParameters.States [ 0 ] + "') " );
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
              sqlQueryString.AppendLine ( " (" + EdEntities.DB_STATE + "= '" + EdRecordObjectStates.Draft_Record + "' "
                + "OR  " + EdEntities.DB_STATE + " = '" + EdRecordObjectStates.Empty_Record + "' "
                + "OR " + EdEntities.DB_STATE + " = '" + EdRecordObjectStates.Completed_Record + "') " );
            }
            else
            {
              sqlQueryString.AppendLine ( " ( " + EdEntities.DB_STATE + " = '" + state + "') " );
            }

          }//ENd iteration loop
          // 
          // Add the multi state query verbs
          // 
          sqlQueryString.AppendLine ( ") " );

        }//END multiple state selection

      }//END state query

      sqlQueryString.AppendLine ( ") ORDER BY " + EdEntities.DB_ENTITY_ID + ";" );

      //
      // Return the sql query string. 
      //
      return sqlQueryString.ToString ( );
    }

    // =====================================================================================
    /// <summary>
    /// This class returns a list of form object based on VisitId, VisitId, FormId and state
    /// </summary>
    /// <param name="Entity">EdRecord Entity Object.</param>
    /// <returns>List of EdRecord (Entity) objects</returns>
    /// <returns>List of EdRecord  objects</returns>
    //  ----------------------------------------------------------------------------------
    public List<EdRecord> getChildEntityList ( 
      EdRecord Entity )
    {
      this.LogMethod ( "getChildEntityList" );
      this.LogDebug ( "LayoutId {0}.", Entity.LayoutId );
      this.LogDebug ( "EntityId {0}.", Entity.EntityId );

      //
      // Initialize the debuglog, a return list of form object and a formRecord field object. 
      //
      List<EdRecord> entityList = new List<EdRecord> ( );
      StringBuilder sqlQueryString = new StringBuilder ( );
      Entity.ChildEntities = new List<EdRecord> ( );

      // 
      // Define the query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( EdEntities.PARM_PARENT_GUID, SqlDbType.UniqueIdentifier),
      };
      cmdParms [ 0 ].Value = Entity.Guid;

      // 
      // Generate the SQL query string.
      // 
      sqlQueryString.AppendLine ( SQL_QUERY_ENTITY_VIEW );
      sqlQueryString.AppendLine ( " WHERE (" + EdEntities.DB_PARENT_GUID + " = " + EdEntities.PARM_PARENT_GUID + " ) " );
      sqlQueryString.AppendLine ( " ORDER BY " + EdEntities.DB_ENTITY_ID + ";" );

      this.LogDebug ( EvSqlMethods.getParameterSqlText ( cmdParms ) );
      this.LogDebug ( sqlQueryString.ToString ( ) );

   //   this.LogDebug ( " Execute Query" );

      //
      //Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString.ToString ( ), cmdParms ) )
      {

        if ( table.Rows.Count == 0 )
        {
          this.LogDebug ( "No Child Entities found." );
          this.LogMethodEnd ( "getChildEntityList" );
          return entityList;
        }

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
          // id the user is the entity author then select all records.
          //
          if ( this.ClassParameters.UserProfile.UserId != Entity.AuthorUserId
            && record.State != EdRecordObjectStates.Submitted_Record )
          {
            this.LogDebug ( "Entity {0} SKIPPED not an authors document and not submitted.", record.RecordId );
            continue;
          }

          //
          // set the entity to only retrieve summary fields.
          //
          record.SelectOnlySummaryFields = true;

          // 
          // Get the trial record items
          // 
          this.GetEntityValues ( record );

          //
          // attach the record sections.
          //
          this.GetRecordSections ( record );

          // 
          // Add the result to the arraylist.
          // 
          entityList.Add ( record );

          // 
          // TestReport the visitSchedule count is less than the max size.
          // 
          if ( entityList.Count > this.ClassParameters.MaxResultLength )
          {
            break;
          }

        }//END for loop

      }//END Using

      // 
      // Get the array length
      // 
      this.LogValue ( "entityList.Count {0}. ", entityList.Count );

      // 
      // Return the result array.
      // 
      this.LogMethodEnd ( "getChildEntityList" );
      return entityList;

    }//END getEntityList method.

    // =====================================================================================
    /// <summary>
    /// This class retrieves an option list based on query parameters and the useGuid condition
    /// </summary>
    /// <param name="QueryParameters">EvQueryParameters: The Query selection values.</param>
    /// <param name="UseGuid">Boolean: true, if the option uses Guid.</param>
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
      EdQueryParameters QueryParameters,
      bool UseGuid )
    {
      this.LogMethod ( "getOptionList method. " );
      this.LogValue ( "EvQueryParameters parameters:" );

      //
      // Initialize the debug log, a return list of options and an option object.
      //
      List<EvOption> list = new List<EvOption> ( );
      EvOption option = new EvOption ( );
      String sqlQueryString = String.Empty;

      if ( UseGuid )
      {
        option = new EvOption ( Guid.Empty.ToString ( ), String.Empty );
      }
      list.Add ( option );

      if ( QueryParameters.Org_City.Contains ( "_" ) == true )
      {
        String [ ] arrCity = QueryParameters.Org_City.Split ( '_' );
        QueryParameters.Org_Country = arrCity [ 0 ];
        QueryParameters.Org_City = arrCity [ 1 ];

        this.LogDebug ( "Country {0} ", QueryParameters.Org_Country );
        this.LogDebug ( "City {0} ", QueryParameters.Org_City );
      }

      if ( QueryParameters.Org_PostCode.Contains ( "_" ) == true )
      {
        String [ ] arrCity = QueryParameters.Org_PostCode.Split ( '_' );
        QueryParameters.Org_Country = arrCity [ 0 ];
        QueryParameters.Org_PostCode = arrCity [ 1 ];

        this.LogDebug ( "Country {0} ", QueryParameters.Org_Country );
        this.LogDebug ( "PostCode {0} ", QueryParameters.Org_PostCode );
      }

      // 
      // Define the query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( EdRecordLayouts.PARM_LAYOUT_ID, SqlDbType.NVarChar, 10),
        new SqlParameter( EdOrganisations.PARM_Address_City, SqlDbType.NVarChar, 50),
        new SqlParameter( EdOrganisations.PARM_COUNTRY, SqlDbType.NVarChar, 50),
      };
      cmdParms [ 0 ].Value = QueryParameters.LayoutId;
      cmdParms [ 1 ].Value = QueryParameters.Org_City;
      cmdParms [ 2 ].Value = QueryParameters.Org_Country;

      this.LogDebug ( EvSqlMethods.getParameterSqlText ( cmdParms ) );

      //
      // Generate the SQL query string.
      //
      sqlQueryString = this.createSqlQueryStatement ( QueryParameters );

      this.LogDebug ( EvSqlMethods.getParameterSqlText ( cmdParms ) );
      this.LogDebug ( sqlQueryString );

      this.LogDebug ( "Execute Query" );
      //
      //Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString, cmdParms ) )
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

          option = new EvOption ( record.RecordId, String.Empty );

          if ( UseGuid == true )
          {
            option.Value = record.Guid.ToString ( );
          }

          option.Description += record.RecordId + " - " + record.Design.Title;

          // 
          // Append the SelectionObject object to the ArrayList.
          // 
          list.Add ( option );

        }//End iteration loop

      }//END using statement.

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

    #region Entity retrieval methods

    // =====================================================================================
    /// <summary>
    /// This method retrieves a form object based on Guid
    /// </summary>
    /// <param name="EntityGuid">Guid: (Mandatory) Global Unique object identifier.</param>
    /// <returns>EdRecord: a entitty data object.</returns>
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
    public EdRecord GetEntity (
      Guid EntityGuid )
    {
      this.LogMethod ( "GetEntity" );
      this.LogDebug ( "EntityGuid: " + EntityGuid );

      //
      // Initialize the debug log, a return form object and a formfield object.
      //
      EdRecord entity = new EdRecord ( );
      StringBuilder sqlQueryString = new StringBuilder ( );

      // 
      // Validate whether the Guid is not metpy. 
      // 
      if ( EntityGuid == Guid.Empty )
      {
        return entity;
      }

      // 
      // Set the query parameter values
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( EdEntities.PARM_ENTITY_GUID, SqlDbType.UniqueIdentifier),
      };
      cmdParms [ 0 ].Value = EntityGuid;

      // 
      // Generate SQL query string
      // 
      sqlQueryString.AppendLine ( SQL_QUERY_ENTITY_VIEW );
      sqlQueryString.AppendLine ( " WHERE ( " + EdEntities.DB_ENTITY_GUID + "=" + EdEntities.PARM_ENTITY_GUID + ") ;" );

      //
      // Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString.ToString ( ), cmdParms ) )
      {
        // 
        // If not rows the return
        // 
        if ( table.Rows.Count == 0 )
        {
          return entity;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        // 
        // Fill the role object.
        // 
        entity = this.getRowData ( row, false );

      }//END Using method

      // 
      // Attach fields and other trial data.
      // 
      this.GetEntityValues ( entity );

      //
      // Update the form record section references.
      //
      this.GetRecordSections ( entity );

      //
      // load layout fields if record field list is empty.
      //
      this.getLayoutFields ( entity );

      //
      // get the child entities for this entity.
      //
      entity.ChildEntities = this.getChildEntityList ( entity );

      //
      // Attache the child entity list.
      //
      this.getRecordEntities ( entity );

      // 
      // Return the trial record.
      // 
      return entity;

    }//END getRecord method

    // =====================================================================================
    /// <summary>
    /// This class gets a form object based on the record identifier
    /// </summary>
    /// <param name="SourceId">string: external source identifier.</param>
    /// <returns>EdRecord: a entitty data object.</returns>
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
    public EdRecord GetEntityBySource (
      String SourceId )
    {
      this.LogMethod ( "GetEntityBySource method. " );
      this.LogDebug ( "SourceId: " + SourceId );
      //
      // Initialize the debug log, a return form object and a formfield object. 
      //
      EdRecord entity = new EdRecord ( );
      StringBuilder sqlQueryString = new StringBuilder ( );

      // 
      // Validate whether the RecordId is not empty. 
      // 
      if ( SourceId == String.Empty )
      {
        return entity;
      }

      // 
      // Define the parameters for the query
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( EdEntities.PARM_SOURCE_ID, SqlDbType.NVarChar, 20 ),
      };
      cmdParms [ 0 ].Value = SourceId;

      sqlQueryString.AppendLine ( SQL_QUERY_ENTITY_VIEW );
      sqlQueryString.AppendLine ( " WHERE (" + EdEntities.DB_SOURCE_ID + "= " + EdEntities.PARM_SOURCE_ID + " );" );

      //
      // Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString.ToString ( ), cmdParms ) )
      {
        // 
        // If not rows the return
        // 
        if ( table.Rows.Count == 0 )
        {
          return entity;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        // 
        // Fill the role object.
        // 
        entity = this.getRowData ( row, true );

      }//END Using method

      //
      // Update the form record section references.
      //
      this.GetRecordSections ( entity );

      //
      // load layout fields if record field list is empty.
      //
      this.getLayoutFields ( entity );

      // 
      // Attach fields and other trial data.
      // 
      this.GetEntityValues ( entity );

      //
      // get the child entities for this entity.
      //
      entity.ChildEntities = this.getChildEntityList ( entity );

      //
      // Attache the entity list.
      //
      this.getRecordEntities ( entity );

      // 
      // Return the trial record.
      // 
      this.LogMethodEnd ( "GetEntityBySource" );
      return entity;

    }//END getItem method 

    // =====================================================================================
    /// <summary>
    /// This class gets a form object based on the record identifier
    /// </summary>
    /// <param name="LayoutId">String layout identifier</param>
    /// <param name="ParentGuid">string: Parent orgnisation identifier.</param>
    /// <returns>EdRecord: a entitty data object.</returns>
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
    public EdRecord GetItemByParentGuid (
      String LayoutId,
      Guid ParentGuid )
    {
      this.LogMethod ( "GetItemByParentGuid method. " );
      //
      // Initialize the debug log, a return form object and a formfield object. 
      //
      EdRecord entity = new EdRecord ( );
      StringBuilder sqlQueryString = new StringBuilder ( );

      // 
      // Validate whether the RecordId is not empty. 
      // 
      if ( ParentGuid == Guid.Empty )
      {
        return entity;
      }

      // 
      // Define the parameters for the query
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( EdEntityLayouts.PARM_LAYOUT_ID, SqlDbType.NVarChar, 20 ),
        new SqlParameter( EdEntities.PARM_PARENT_GUID, SqlDbType.UniqueIdentifier ),
      };
      cmdParms [ 0 ].Value = LayoutId;
      cmdParms [ 1 ].Value = ParentGuid;

      //
      // Define the SQL statement.
      //
      sqlQueryString.AppendLine ( SQL_QUERY_ENTITY_VIEW );
      sqlQueryString.AppendLine ( " WHERE (" + EdEntityLayouts.DB_LAYOUT_ID + "= " + EdEntityLayouts.PARM_LAYOUT_ID + " ) " );
      sqlQueryString.AppendLine ( "   AND (" + EdEntities.DB_PARENT_GUID + "= " + EdEntities.PARM_PARENT_GUID + " );" );

      //
      // Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString.ToString ( ), cmdParms ) )
      {
        // 
        // If not rows the return
        // 
        if ( table.Rows.Count == 0 )
        {
          return entity;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        // 
        // Fill the role object.
        // 
        entity = this.getRowData ( row, true );

      }//END Using method

      //
      // Update the form record section references.
      //
      this.GetRecordSections ( entity );

      //
      // load layout fields if record field list is empty.
      //
      this.getLayoutFields ( entity );

      // 
      // Attach fields and other trial data.
      // 
      this.GetEntityValues ( entity );

      //
      // get the child entities for this entity.
      //
      entity.ChildEntities = this.getChildEntityList ( entity );

      //
      // Attache the entity list.
      //
      this.getRecordEntities ( entity );

      // 
      // Return the trial record.
      // 
      this.LogMethodEnd ( "GetItemByParentGuid" );
      return entity;

    }//END GetItemByParentOrgId method

    // =====================================================================================
    /// <summary>
    /// This class gets a form object based on the record identifier
    /// </summary>
    /// <param name="LayoutId">String layout identifier</param>
    /// <param name="ParentOrgId">string: Parent orgnisation identifier.</param>
    /// <returns>EdRecord: a entitty data object.</returns>
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
    public EdRecord GetItemByParentOrgId (
      String LayoutId,
      String ParentOrgId )
    {
      this.LogMethod ( "GetItemByParentOrgId method. " );
      //
      // Initialize the debug log, a return form object and a formfield object. 
      //
      EdRecord entity = new EdRecord ( );
      StringBuilder sqlQueryString = new StringBuilder ( );

      // 
      // Validate whether the RecordId is not empty. 
      // 
      if ( ParentOrgId == String.Empty )
      {
        return entity;
      }

      // 
      // Define the parameters for the query
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( EdEntityLayouts.PARM_LAYOUT_ID, SqlDbType.NVarChar, 20 ),
        new SqlParameter( EdEntities.PARM_PARENT_ORG_ID, SqlDbType.NVarChar, 20 ),
      };
      cmdParms [ 0 ].Value = LayoutId;
      cmdParms [ 1 ].Value = ParentOrgId;

      //
      // Define the SQL statement.
      //
      sqlQueryString.AppendLine ( SQL_QUERY_ENTITY_VIEW );
      sqlQueryString.AppendLine ( " WHERE (" + EdEntityLayouts.DB_LAYOUT_ID + "= " + EdEntityLayouts.PARM_LAYOUT_ID + " ) " );
      sqlQueryString.AppendLine ( "   AND (" + EdEntities.DB_PARENT_ORG_ID + "= " + EdEntities.PARM_PARENT_ORG_ID + " );" );

      //
      // Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString.ToString ( ), cmdParms ) )
      {
        // 
        // If not rows the return
        // 
        if ( table.Rows.Count == 0 )
        {
          return entity;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        // 
        // Fill the role object.
        // 
        entity = this.getRowData ( row, true );

      }//END Using method

      //
      // Update the form record section references.
      //
      this.GetRecordSections ( entity );

      //
      // load layout fields if record field list is empty.
      //
      this.getLayoutFields ( entity );

      // 
      // Attach fields and other trial data.
      // 
      this.GetEntityValues ( entity );

      //
      // get the child entities for this entity.
      //
      entity.ChildEntities = this.getChildEntityList ( entity );

      //
      // Attache the entity list.
      //
      this.getRecordEntities ( entity );

      // 
      // Return the trial record.
      // 
      this.LogMethodEnd ( "GetItemByParentOrgId" );
      return entity;

    }//END GetItemByParentOrgId method

    // =====================================================================================
    /// <summary>
    /// This class gets a form object based on the record identifier
    /// </summary>
    /// <param name="LayoutId">String layout identifier</param>
    /// <param name="ParentUserId">string: parent user identifier.</param>
    /// <returns>EdRecord: a entitty data object.</returns>
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
    public EdRecord GetItemByParentUserId (
      String LayoutId,
      String ParentUserId )
    {
      this.LogMethod ( "GetItemByParentOrgId" );
      //
      // Initialize the debug log, a return form object and a formfield object. 
      //
      EdRecord entity = new EdRecord ( );
      StringBuilder sqlQueryString = new StringBuilder ( );

      // 
      // Validate whether the RecordId is not empty. 
      // 
      if ( LayoutId == String.Empty
        || ParentUserId == String.Empty )
      {
        this.LogMethodEnd ( "GetItemByParentOrgId" );
        return entity;
      }

      // 
      // Define the parameters for the query
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( EdEntityLayouts.PARM_LAYOUT_ID, SqlDbType.NVarChar, 20 ),
        new SqlParameter( EdEntities.PARM_PARENT_USER_ID, SqlDbType.NVarChar, 100 ),
      };
      cmdParms [ 0 ].Value = LayoutId;
      cmdParms [ 1 ].Value = ParentUserId;

      //
      // Define the SQL statement.
      //
      sqlQueryString.AppendLine ( SQL_QUERY_ENTITY_VIEW );
      sqlQueryString.AppendLine ( " WHERE (" + EdEntityLayouts.DB_LAYOUT_ID + "= " + EdEntityLayouts.PARM_LAYOUT_ID + " ) " );
      sqlQueryString.AppendLine ( "   AND (" + EdEntities.DB_PARENT_USER_ID + "= " + EdEntities.PARM_PARENT_USER_ID + " );" );

      //
      // Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString.ToString ( ), cmdParms ) )
      {
        // 
        // If not rows the return
        // 
        if ( table.Rows.Count == 0 )
        {
          return entity;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        // 
        // Fill the role object.
        // 
        entity = this.getRowData ( row, true );

      }//END Using method

      //
      // Update the form record section references.
      //
      this.GetRecordSections ( entity );

      //
      // load layout fields if record field list is empty.
      //
      this.getLayoutFields ( entity );

      // 
      // Attach fields and other trial data.
      // 
      this.GetEntityValues ( entity );

      //
      // get the child entities for this entity.
      //
      entity.ChildEntities = this.getChildEntityList ( entity );

      //
      // Attache the entity list.
      //
      this.getRecordEntities ( entity );

      // 
      // Return the trial record.
      // 
      this.LogMethodEnd ( "GetItemByParentOrgId" );
      return entity;

    }//END GetItemByParentOrgId method

    // =====================================================================================
    /// <summary>
    /// This class gets a record object using RecordId and the form state. 
    /// </summary>
    /// <param name="EntityId">string: (Mandatory) entity identifier.</param>
    /// <returns>EdRecord: a entitty data object.</returns>
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
    public EdRecord GetEntity ( String EntityId )
    {
      this.LogMethod ( "GetEntity" );
     // this.LogDebug ( "EntityId: " + EntityId );
      //
      // Initialize the debug log, a return form object and a formfield object. 
      //
      EdRecord entity = new EdRecord ( );
      StringBuilder sqlQueryString = new StringBuilder ( );

      // 
      // TestReport that the data object has a valid record identifier.
      // 
      if ( EntityId == String.Empty )
      {
        return entity;
      }

      // 
      // Define the parameters for the query
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(PARM_ENTITY_ID, SqlDbType.NVarChar, 20),
      };
      cmdParms [ 0 ].Value = EntityId;

      sqlQueryString.AppendLine ( SQL_QUERY_ENTITY_VIEW );
      sqlQueryString.AppendLine ( " WHERE ( " + EdEntities.DB_ENTITY_ID + " = " + PARM_ENTITY_ID + " );" );

      //
      // Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString.ToString ( ), cmdParms ) )
      {
        // 
        // If not rows the return
        // 
        if ( table.Rows.Count == 0 )
        {
          return entity;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        // 
        // Fill the role object.
        // 
        entity = this.getRowData ( row, false );

      }//END Using method

      //
      // Update the form record section references.
      //
      this.GetRecordSections ( entity );

      //
      // load layout fields if record field list is empty.
      //
      this.getLayoutFields ( entity );

      // 
      // Attach fields and other trial data.
      // 
      this.GetEntityValues ( entity );

      //
      // get the child entities for this entity.
      //
      entity.ChildEntities = this.getChildEntityList ( entity );

      this.LogDebug ( "Child Entity Count {0}.", entity.ChildEntities );
      //
      // Attache the entity list.
      //
      this.getRecordEntities ( entity );

      // 
      // Return the trial record.
      // 
      return entity;

    }//END getRecord method

    // =====================================================================================
    /// <summary>
    /// This class returns the guid identifier for an entity identifier
    /// </summary>
    /// <param name="EntityId">string: (Mandatory) entity identifier.</param>
    /// <returns>Guid identifier.</returns>
    //  ----------------------------------------------------------------------------------
    public Guid GetEntityGuid ( String EntityId )
    {
      this.LogMethod ( "GetEntityGuid" );
      // this.LogDebug ( "EntityId: " + EntityId );
      //
      // Initialize the debug log, a return form object and a formfield object. 
      //
      StringBuilder sqlQueryString = new StringBuilder ( );

      // 
      // TestReport that the data object has a valid record identifier.
      // 
      if ( EntityId == String.Empty )
      {
        return Guid.Empty;
      }

      // 
      // Define the parameters for the query
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(PARM_ENTITY_ID, SqlDbType.NVarChar, 20),
      };
      cmdParms [ 0 ].Value = EntityId;

      sqlQueryString.AppendLine ( SQL_QUERY_ENTITY_VIEW );
      sqlQueryString.AppendLine ( " WHERE ( " + EdEntities.DB_ENTITY_ID + " = " + PARM_ENTITY_ID + " );" );

      //
      // Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( sqlQueryString.ToString ( ), cmdParms ) )
      {
        // 
        // If not rows the return
        // 
        if ( table.Rows.Count == 0 )
        {
          return Guid.Empty;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        return EvSqlMethods.getGuid ( row, EdEntities.DB_ENTITY_GUID );

      }//END Using method

    }//END getRecord method

    // =====================================================================================
    /// <summary>
    /// This method updates the form field section references.
    /// </summary>
    /// <param name="Entity">EvForm: a form record object</param>
    // ----------------------------------------------------------------------------------
    private void GetRecordSections (
      EdRecord Entity )
    {
      this.LogMethod ( "GetRecordSections" );
      //
      // Initialise the methods variables and objects.
      //
      EdEntitySections sections = new EdEntitySections ( this.ClassParameters );

      Entity.Design.FormSections = sections.getSectionList ( Entity.LayoutGuid );

     // this.LogClass ( sections.Log );
     // this.LogDebug ( "Section Count {0}.", Entity.Design.FormSections.Count );

      this.LogMethodEnd ( "GetRecordSections" );
    }//END UpdateFieldSectionReferences method.

    // =====================================================================================
    /// <summary>
    /// This method attaches the other trial data to the record that is needed for the record 
    ///  to be updated.
    /// This data is only to be attached if the record state is editable.  As newField validation
    ///  is not necessary in any other state.
    /// </summary>
    /// <param name="Entity">EvForm: (Mandatory) a form data object.</param>
    //  ----------------------------------------------------------------------------------
    private void GetEntityValues (
      EdRecord Entity )
    {
      this.LogMethod ( "GetEntityData." );
   //   this.LogDebug ( "State: " + Entity.State );
      // 
      // Initialise the methods variables and objects.
      // 
      EdEntityValues dal_EntityValues = new EdEntityValues ( this.ClassParameters );

      // 
      // Get the record fields
      // 
      Entity.Fields = dal_EntityValues.GetlEntityValues ( Entity );
      //this.LogClass ( dal_EntityValues.Log );

      this.LogValue ( "Field count: " + Entity.Fields.Count );
      this.LogMethodEnd ( "GetEntityData" );

    }//END getRecordData method

    // =====================================================================================
    /// <summary>
    /// This method attaches the other trial data to the record that is needed for the record 
    ///  to be updated.
    /// This data is only to be attached if the record state is editable.  As newField validation
    ///  is not necessary in any other state.
    /// </summary>
    /// <param name="Entity">EvForm: (Mandatory) a form data object.</param>
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
      EdRecord Entity )
    {
      this.LogMethod ( "getLayoutFields." );
     // this.LogDebug ( "Entity Guid {0}, LayoutGuid: {1} ", Entity.Guid, Entity.LayoutGuid );

      if ( Entity.Fields.Count > 0 )
      {
     //   this.LogDebug ( "Field Count {0}.", Entity.Fields.Count );
        this.LogMethodEnd ( "getLayoutFields" );
        return;
      }

      // 
      // Initialise the methods variables and objects.
      // 
      EdEntityFields dal_EntityFields = new EdEntityFields ( this.ClassParameters );
      // 
      // Get the record fields
      // 
      var fieldList = dal_EntityFields.GetFieldList ( Entity.LayoutGuid );
      //this.LogDebugClass ( dal_EntityFields.Log );

      for ( int i = 0; i < fieldList.Count; i++ )
      {
        EdRecordField field = fieldList [ i ];
        field.FieldGuid = field.Guid;
        field.RecordGuid = Entity.Guid;
        field.Guid = Guid.NewGuid ( );

        Entity.Fields.Add ( field );

      //  this.LogDebug ( "(Value) Guid {0}. RecordGuid {1}, FieldGuid {2}.", field.Guid, field.RecordGuid, field.FieldGuid );
        //this.LogDebug ( "{0} - {1} > T: {2} ", field.FieldId, field.Title, field.TypeId );
      }

     // this.LogDebug ( "Section Count {0}.", Entity.Fields.Count );
      this.LogMethodEnd ( "getLayoutFields" );

    }//END getRecordData method

    // ==================================================================================
    /// <summary>
    /// This method retrieves the layout's field objects.
    /// </summary>
    /// <param name="Entity">EdRecord object</param>
    //  ---------------------------------------------------------------------------------
    private void getRecordEntities ( EdRecord Entity )
    {
      //
      // initialise the methods variables and objects.
      //
      EdRecords dal_RecordEntities = new EdRecords ( this.ClassParameters );

      //
      // if no entities exit.
      //
      if ( Entity.ChildEntities.Count == 0 )
      {
        return;
      }

      // 
      // Retrieve the instrument items.
      // 
      Entity.ChildRecords = dal_RecordEntities.getChildRecordList ( Entity );
      this.LogClass ( dal_RecordEntities.Log );
    }

    #endregion

    #region Entity Update queries

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
        EvSqlMethods.StoreProcUpdate ( STORED_PROCEDURE_ENTITY_CREATE, cmdParms );
      }
      catch ( Exception ex )
      {
        this.LogException ( ex );
      }
      // 
      // Return unique identifier of the new data object.
      // 
      EdRecord record = this.GetEntity ( Record.Guid );

      //
      // Get the empty field objects for the new record.
      //
      this.getLayoutFields ( record );

      this.updateRecordData ( record );

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
    public EvEventCodes UpdateItem ( EdRecord Record )
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
      // Validate whether the record object exists.
      // 
      this.LogDebug ( "Get previous record" );
      EdRecord previousRecord = this.GetEntity ( Record.RecordId );
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
      databaseRecordAffected = EvSqlMethods.StoreProcUpdate ( STORED_PROCEDURE_ENTITY_UPDATE, cmdParms );
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
    /// <param name="Entity">EdRecord: object.</param>
    //  ----------------------------------------------------------------------------------
    private EvEventCodes updateRecordData (
      EdRecord Entity )
    {
      this.LogMethod ( "updateRecordData." );
      this.LogDebug ( "State: " + Entity.StateDesc );
      // 
      // Initialise the methods variables and objects.
      // 
      EdEntityValues dal_EntityValues = new EdEntityValues ( this.ClassParameters );

      // 
      this.LogValue ( "Entity.Fields.Count: " + Entity.Fields.Count );
      if ( Entity.Fields.Count == 0 )
      {
        return EvEventCodes.Ok;
      }

      // 
      // update record values
      // 
      var result = dal_EntityValues.UpdateFields ( Entity );
      this.LogClass ( dal_EntityValues.Log );

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
        new SqlParameter(PARM_ENTITY_GUID, SqlDbType.UniqueIdentifier),
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
      if ( ( RecordsUpdated = EvSqlMethods.StoreProcUpdate ( STORED_PROCEDURE_ENTITY_LOCK, cmdParms ) ) == 0 )
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
        new SqlParameter(PARM_ENTITY_GUID, SqlDbType.UniqueIdentifier),
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
      if ( ( RecordsUpdated = EvSqlMethods.StoreProcUpdate ( STORED_PROCEDURE_ENTITY_UNLOCK, cmdParms ) ) == 0 )
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
