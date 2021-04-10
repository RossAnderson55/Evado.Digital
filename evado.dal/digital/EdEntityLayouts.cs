/* <copyright file="DAL\EvForms.cs" company="EVADO HOLDING PTY. LTD.">
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

namespace Evado.Dal.Digital
{
  /// <summary>
  /// This class is handles the data access layer for the form data object.
  /// </summary>
  public class EdEntityLayouts : EvDalBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EdEntityLayouts ( )
    {
      this.ClassNameSpace = "Evado.Dal.Digital.EdEntityLayouts.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EdEntityLayouts ( EvClassParameters Settings )
    {
      this.ClassParameters = Settings;
      this.ClassNameSpace = "Evado.Dal.Digital.EdEntityLayouts.";

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

    //
    // Define the Selection selectionList query string constant.
    //
    private const string _sqlQuery_View = "Select * FROM ED_ENTITY_LAYOUT_VIEW ";

    // 
    // The SQL Store Procedure constants
    // 
    private const string STORED_PROCEDURE_ADD_ITEM = "USR_ENTITY_LAYOUT_ADD";
    private const string STORED_PROCEDURE_UPDATE_ITEM = "USR_ENTITY_LAYOUT_UPDATE";
    private const string STORED_PROCEDURE_DELETE_ITEM = "USR_ENTITY_LAYOUT_DELETE";
    private const string STORED_PROCEDURE_WithdrawItem = "USR_ENTITY_LAYOUT_WITHDRAWN";
    private const string STORED_PROCEDURE_COPY_ITEM = "USR_ENTITY_LAYOUT_COPY";

    /// <summary>
    /// The database entity layout guid column name
    /// </summary>
    public const string DB_LAYOUT_GUID = "EDEL_GUID";
    /// <summary>
    /// The database entity layout identifier column name
    /// </summary>
    public const string DB_LAYOUT_ID = "EDE_LAYOUT_ID";
    /// <summary>
    /// The database entity layout state column name
    /// </summary>
    public const string DB_STATE = "EDEL_STATE";
    /// <summary>
    /// The database entity layout title column name
    /// </summary>
    public const string DB_TITLE = "EDEL_TITLE";
    /// <summary>
    /// The database entity http reference guid column name
    /// </summary>
    public const string DB_HTTP_REFERENCE = "EDEL_HTTP_REFERENCE";
    /// <summary>
    /// The database entity layout instructions column name
    /// </summary>
    public const string DB_INSTRUCTIONS = "EDEL_INSTRUCTIONS";
    /// <summary>
    /// The database entity layout update description  column name
    /// </summary>
    public const string DB_DESCRIPTION = "EDEL_DESCRIPTION";
    /// <summary>
    /// The database entity layout update reason  column name
    /// </summary>
    public const string DB_UPDATE_REASON = "EDEL_UPDATE_REASON";
    /// <summary>
    /// The database entity layout record category  column name
    /// </summary>
    public const string DB_RECORD_CATEGORY = "EDEL_RECORD_CATEGORY";
    /// <summary>
    /// The database entity layout  type column name
    /// </summary>
    public const string DB_TYPE_ID = "EDEL_TYPE_ID";
    /// <summary>
    /// The database entity layout version  column name
    /// </summary>
    public const string DB_VERSION = "EDEL_VERSION";
    /// <summary>
    /// The database entity layout java script column name
    /// </summary>
    public const string DB_JAVA_SCRIPT = "EDEL_JAVA_SCRIPT";
    /// <summary>
    /// The database entity layout has CS script column name
    /// </summary>
    public const string DB_HAS_CS_SCRIPT = "EDEL_HAS_CS_SCRIPT";
    /// <summary>
    /// The database entity layout language column name
    /// </summary>
    public const string DB_LANGUAGE = "EDEL_LANGUAGE";
    /// <summary>
    /// The database entity layout read access roles column name
    /// </summary>
    public const string DB_READ_ACCESS_ROLES = "EDEL_READ_ACCESS_ROLES";
    /// <summary>
    /// The database entity layout edit access roles column name
    /// </summary>
    public const string DB_EDIT_ACCESS_ROLES = "EDEL_EDIT_ACCESS_ROLES";
    /// <summary>
    /// The database entity layout parent type column name
    /// </summary>
    public const string DB_PARENT_TYPE = "EDEL_PARENT_TYPE";
    /// <summary>
    /// The database entity layout parent access column name
    /// </summary>
    public const string DB_PARENT_ACCESS = "EDEL_PARENT_ACCESS";
    /// <summary>
    /// The database entity layout parent entities column name
    /// </summary>
    public const string DB_PARENT_ENTITIES = "EDEL_PARENT_ENTITIES";
    /// <summary>
    /// The database entity layout default page layout column name
    /// </summary>
    public const string DB_DEFAULT_PAGE_LAYOUT = "EDEL_DEFAULT_PAGE_LAYOUT";
    /// <summary>
    /// The database entity layout link content setting column name
    /// </summary>
    public const string DB_LINK_CONTENT_SETTING = "EDEL_LINK_CONTENT_SETTING";
    /// <summary>
    /// The database entity layout display entities column name
    /// </summary>
    public const string DB_DISPLAY_ENTITIES = "EDEL_DISPLAY_ENTITIES";
    /// <summary>
    /// The database entity layout display author details column name
    /// </summary>
    public const string DB_DISPLAY_AUTHOR_DETAILS = "EDEL_DISPLAY_AUTHOR_DETAILS";
    /// <summary>
    /// The database entity layout prefix column name
    /// </summary>
    public const string DB_ENTITY_PREFIX = "EDEL_ENTITY_PREFIX";
    /// <summary>
    /// The database entity layout header format column name
    /// </summary>
    public const string DB_HEADER_FORMAT = "EDEL_HEADER_FORMAT";
    /// <summary>
    /// The database entity layout footer format column name
    /// </summary>
    public const string DB_FOOTER_FORMAT = "EDEL_FOOTER_FORMAT";
    /// <summary>
    /// The database entity layout footer format column name
    /// </summary>
    public const string DB_FIELD_DISPLAY_FORMAT = "EDEL_FIELD_DISPLAY_FORMAT";
    /// <summary>
    /// This database entity layout field Id filter 0
    /// </summary>
    public const string DB_FILTER_FIELD_0 = "EDEL_FILTER_FIELD_0";
    /// <summary>
    /// This database entity layout field Id filter 1
    /// </summary>
    public const string DB_FILTER_FIELD_1 = "EDEL_FILTER_FIELD_1";
    /// <summary>
    /// This database entity layout field Id filter 2
    /// </summary>
    public const string DB_FILTER_FIELD_2 = "EDEL_FILTER_FIELD_2";
    /// <summary>
    /// This database entity layout field Id filter 3
    /// </summary>
    public const string DB_FILTER_FIELD_3 = "EDEL_FILTER_FIELD_3";
    /// <summary>
    /// This database entity layout field Id filter 4
    /// </summary>
    public const string DB_FILTER_FIELD_4 = "EDEL_FILTER_FIELD_4";
    /// <summary>
    /// The database entity updates by user identifier column name
    /// </summary>
    public const string DB_UPDATED_BY_USER_ID = "EDEL_UPDATED_BY_USER_ID";
    /// <summary>
    /// The database entity update by user common name column name
    /// </summary>
    public const string DB_UPDATED_BY = "EDEL_UPDATED_BY";
    /// <summary>
    /// The database entity update date column name
    /// </summary>
    public const string DB_UPDATED_DATE = "EDEL_UPDATED_DATE";
    /// <summary>
    /// The database entity deleted column name
    /// </summary>
    public const string DB_DELETED = "EDEL_DELETED";


    /// <summary>
    /// Define the query parameter names.
    /// </summary>
    private const string PARM_LAYOUT_GUID = "@GUID";
    /// <summary>
    /// This constant defines the layuout identifier parameter
    /// </summary>
    public const string PARM_LAYOUT_ID = "@LAYOUT_ID";
    private const string PARM_STATE = "@STATE";
    private const string PARM_TITLE = "@TITLE";
    private const string PARM_HTTP_REFERENCE = "@HTTP_REFERENCE";
    private const string PARM_INSTRUCTIONS = "@INSTRUCTIONS";
    private const string PARM_DESCRIPTION = "@DESCRIPTION";
    private const string PARM_UPDATE_REASON = "@UPDATE_REASON";
    private const string PARM_RECORD_CATEGORY = "@RECORD_CATEGORY";
    private const string PARM_TYPE_ID = "@TYPE_ID";
    private const string PARM_VERSION = "@VERSION";
    private const string PARM_JAVA_SCRIPT = "@JAVA_SCRIPT";
    private const string PARM_HAS_CS_SCRIPT = "@HAS_CS_SCRIPT";
    private const string PARM_LANGUAGE = "@LANGUAGE";
    private const string PARM_READ_ACCESS_ROLES = "@READ_ACCESS_ROLES";
    private const string PARM_EDIT_ACCESS_ROLES = "@EDIT_ACCESS_ROLES";
    private const string PARM_PARENT_ENTITIES = "@PARENT_ENTITIES";
    private const string PARM_DEFAULT_PAGE_LAYOUT = "@DEFAULT_PAGE_LAYOUT";
    private const string PARM_LINK_CONTENT_SETTING = "@LINK_CONTENT_SETTING";
    private const string PARM_DISPLAY_ENTITIES = "@DISPLAY_ENTITIES";
    private const string PARM_DISPLAY_AUTHOR_DETAILS = "@DISPLAY_AUTHOR_DETAILS";
    private const string PARM_ENTITY_PREFIX = "@ENTITY_PREFIX";
    private const string PARM_PARENT_TYPE = "@PARENT_TYPE";
    private const string PARM_PARENT_ACCESS = "@PARENT_ACCESS";
    private const string PARM_HEADER_FORMAT = "@HEADER_FORMAT";
    private const string PARM_FOOTER_FORMAT = "@FOOTER_FORMAT";
    private const string PARM_FIELD_DISPLAY_FORMAT = "@FIELD_DISPLAY_FORMAT";
    /// <summary>
    /// This is the entity layout field id 0 parameter
    /// </summary>
    public const string PARM_FILTER_FIELD = "@FILTER_FIELD_";
    /// <summary>
    /// This is the entity layout field id 0 parameter
    /// </summary>
    public const string PARM_FILTER_FIELD_0 = "@FILTER_FIELD_0";
    /// <summary>
    /// This is the entity layout field id 1 parameter
    /// </summary>
    public const string PARM_FILTER_FIELD_1 = "@FILTER_FIELD_1";
    /// <summary>
    /// This is the entity layout field id 2 parameter
    /// </summary>
    public const string PARM_FILTER_FIELD_2 = "@FILTER_FIELD_2";
    /// <summary>
    /// This is the entity layout field id 3 parameter
    /// </summary>
    public const string PARM_FILTER_FIELD_3 = "@FILTER_FIELD_3";
    /// <summary>
    /// This is the entity layout field id 4 parameter
    /// </summary>
    public const string PARM_FILTER_FIELD_4 = "@FILTER_FIELD_4";

    private const string PARM_UPDATED_BY_USER_ID = "@UPDATED_BY_USER_ID";
    private const string PARM_UPDATED_BY = "@UPDATED_BY";
    private const string PARM_UPDATED_DATE = "@UPDATED_DATE";

    private const string PARM_Copy = "@Copy";

    // 
    // Define the SQL query string variable.
    //
    private string _sqlQueryString = String.Empty;

    // +++++++++++++++++++++++++++ END INITIALISATION SECTION ++++++++++++++++++++++++++++++
    #endregion

    #region Set Query Parameters

    // =====================================================================================
    /// <summary>
    /// This class sets an array of sql query parameters
    /// </summary>
    /// <returns>SqlParameter: an array of the sql query parameters</returns>
    /// <remarks>
    /// This method consists of the following step: 
    /// 
    /// 1. Create new array of sql query parameters.
    /// 
    /// 2. Return an array of sql query parameters. 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    private static SqlParameter [ ] GetParameters ( )
    {
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( EdEntityLayouts.PARM_LAYOUT_GUID, SqlDbType.UniqueIdentifier),
        new SqlParameter( EdEntityLayouts.PARM_LAYOUT_ID, SqlDbType.NVarChar, 20),
        new SqlParameter( EdEntityLayouts.PARM_STATE, SqlDbType.NVarChar, 20),
        new SqlParameter( EdEntityLayouts.PARM_TITLE, SqlDbType.NVarChar, 80),
        new SqlParameter( EdEntityLayouts.PARM_HTTP_REFERENCE, SqlDbType.NVarChar, 250),
        new SqlParameter( EdEntityLayouts.PARM_INSTRUCTIONS, SqlDbType.NText),
        new SqlParameter( EdEntityLayouts.PARM_DESCRIPTION, SqlDbType.NText),   
        new SqlParameter( EdEntityLayouts.PARM_UPDATE_REASON, SqlDbType.NVarChar, 50),
        new SqlParameter( EdEntityLayouts.PARM_RECORD_CATEGORY, SqlDbType.NVarChar, 100),
        new SqlParameter( EdEntityLayouts.PARM_TYPE_ID, SqlDbType.VarChar, 25),

        new SqlParameter( EdEntityLayouts.PARM_VERSION, SqlDbType.Float),
        new SqlParameter( EdEntityLayouts.PARM_JAVA_SCRIPT, SqlDbType.NText),   
        new SqlParameter( EdEntityLayouts.PARM_HAS_CS_SCRIPT, SqlDbType.Bit),
        new SqlParameter( EdEntityLayouts.PARM_LANGUAGE, SqlDbType.VarChar, 5),
        new SqlParameter( EdEntityLayouts.PARM_READ_ACCESS_ROLES, SqlDbType.NVarChar, 250),
        new SqlParameter( EdEntityLayouts.PARM_EDIT_ACCESS_ROLES, SqlDbType.NVarChar, 250),
        new SqlParameter( EdEntityLayouts.PARM_PARENT_ENTITIES, SqlDbType.NVarChar, 250),
        new SqlParameter( EdEntityLayouts.PARM_DEFAULT_PAGE_LAYOUT, SqlDbType.NVarChar, 50),
        new SqlParameter( EdEntityLayouts.PARM_LINK_CONTENT_SETTING, SqlDbType.NVarChar, 50),
        new SqlParameter( EdEntityLayouts.PARM_DISPLAY_ENTITIES, SqlDbType.Bit),

        new SqlParameter( EdEntityLayouts.PARM_DISPLAY_AUTHOR_DETAILS, SqlDbType.Bit),
        new SqlParameter( EdEntityLayouts.PARM_ENTITY_PREFIX, SqlDbType.NVarChar, 10),
        new SqlParameter( EdEntityLayouts.PARM_PARENT_TYPE, SqlDbType.NVarChar, 50),
        new SqlParameter( EdEntityLayouts.PARM_PARENT_ACCESS, SqlDbType.NVarChar, 50),
        new SqlParameter( EdEntityLayouts.PARM_HEADER_FORMAT, SqlDbType.NVarChar, 30),
        new SqlParameter( EdEntityLayouts.PARM_FOOTER_FORMAT, SqlDbType.NVarChar, 30),

        new SqlParameter( EdEntityLayouts.PARM_FILTER_FIELD_0, SqlDbType.NVarChar, 30),
        new SqlParameter( EdEntityLayouts.PARM_FILTER_FIELD_1, SqlDbType.NVarChar, 30),
        new SqlParameter( EdEntityLayouts.PARM_FILTER_FIELD_2, SqlDbType.NVarChar, 30),
        new SqlParameter( EdEntityLayouts.PARM_FILTER_FIELD_3, SqlDbType.NVarChar, 30),

        new SqlParameter( EdEntityLayouts.PARM_FILTER_FIELD_4, SqlDbType.NVarChar, 30),
        new SqlParameter( EdEntityLayouts.PARM_FIELD_DISPLAY_FORMAT, SqlDbType.NVarChar, 30),
        new SqlParameter( EdEntityLayouts.PARM_UPDATED_BY_USER_ID, SqlDbType.NVarChar,100),
        new SqlParameter( EdEntityLayouts.PARM_UPDATED_BY, SqlDbType.NVarChar,30),
        new SqlParameter( EdEntityLayouts.PARM_UPDATED_DATE, SqlDbType.DateTime),
      };
      return cmdParms;
    }//END GetParameters class

    // =====================================================================================
    /// <summary>
    /// This class binds values parameters 
    /// </summary>
    /// <param name="cmdParms">SqlParameter: an array of Database parameters</param>
    /// <param name="EntityLayout">EvForm: Values to bind to parameters</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. If the FormUid is emptry create a new value.
    /// 
    /// 2. Fill the parameters array with the values from the form object. 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    private void SetParameters ( SqlParameter [ ] cmdParms, EdRecord EntityLayout )
    {
      // 
      // If the FormUid is emptry create a new value.
      // 
      int filterCount = 0;
      if ( EntityLayout.Guid == Guid.Empty )
      {
        EntityLayout.Guid = Guid.NewGuid ( );
      }

      //
      // define the filter field identifiers for the entity.
      //
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
       * The selected fieldIds are then stored as filter field identifiers in the EntityLayouts 
       * table.
       * 
       ************************************************************************************/
      for ( int filterIndex = 0; filterIndex < EntityLayout.FilterFieldIds.Length; filterIndex++ )
      {
        EntityLayout.FilterFieldIds [ filterIndex ] = String.Empty;
      }

      foreach ( EdRecordField field in EntityLayout.Fields )
      {
        if ( field.Design.IsSummaryField == false )
        {
          continue;
        }

        //
        // break the iteration loop there are more than 5 values.
        //
        if ( filterCount >= EntityLayout.FilterFieldIds.Length )
        {
          continue;
        }

        //
        // only add selection fields 
        //
        switch ( field.TypeId )
        {
          case EvDataTypes.Yes_No:
          case EvDataTypes.Boolean:
          case EvDataTypes.Check_Box_List:
          case EvDataTypes.Selection_List:
          case EvDataTypes.External_Selection_List:
          case EvDataTypes.Horizontal_Radio_Buttons:
          case EvDataTypes.Radio_Button_List:
            {
              EntityLayout.FilterFieldIds [ filterCount ] = field.FieldId;
              filterCount++;
              continue;
            }
        }
      }


      // 
      // Fill the parameters array with the values from the form object. 
      // 
      cmdParms [ 0 ].Value = EntityLayout.Guid;
      cmdParms [ 1 ].Value = EntityLayout.LayoutId.Trim ( );
      cmdParms [ 2 ].Value = EntityLayout.State.ToString ( );
      cmdParms [ 3 ].Value = EntityLayout.Design.Title;
      cmdParms [ 4 ].Value = EntityLayout.Design.HttpReference;
      cmdParms [ 5 ].Value = EntityLayout.Design.Instructions;
      cmdParms [ 6 ].Value = EntityLayout.Design.Description;
      cmdParms [ 7 ].Value = EntityLayout.Design.UpdateReason.ToString ( );
      cmdParms [ 8 ].Value = EntityLayout.Design.RecordCategory;
      cmdParms [ 9 ].Value = EntityLayout.Design.TypeId.ToString ( );

      cmdParms [ 10 ].Value = EntityLayout.Design.Version;
      cmdParms [ 11 ].Value = EntityLayout.Design.JavaScript;
      cmdParms [ 12 ].Value = EntityLayout.Design.hasCsScript;
      cmdParms [ 13 ].Value = EntityLayout.Design.Language;
      cmdParms [ 14 ].Value = EntityLayout.Design.ReadAccessRoles;
      cmdParms [ 15 ].Value = EntityLayout.Design.EditAccessRoles;
      cmdParms [ 16 ].Value = EntityLayout.Design.ParentEntities;
      cmdParms [ 17 ].Value = EntityLayout.Design.DefaultPageLayout;
      cmdParms [ 18 ].Value = EntityLayout.Design.LinkContentSetting;
      cmdParms [ 19 ].Value = EntityLayout.Design.DisplayRelatedEntities;

      cmdParms [ 20 ].Value = EntityLayout.Design.DisplayAuthorDetails;
      cmdParms [ 21 ].Value = EntityLayout.Design.RecordPrefix;
      cmdParms [ 22 ].Value = EntityLayout.Design.ParentType;
      cmdParms [ 23 ].Value = EntityLayout.Design.AuthorAccess;
      cmdParms [ 24 ].Value = EntityLayout.Design.HeaderFormat;
      cmdParms [ 25 ].Value = EntityLayout.Design.FooterFormat;
      cmdParms [ 26 ].Value = EntityLayout.FilterFieldIds [ 0 ];
      cmdParms [ 27 ].Value = EntityLayout.FilterFieldIds [ 1 ];
      cmdParms [ 28 ].Value = EntityLayout.FilterFieldIds [ 2 ];
      cmdParms [ 29 ].Value = EntityLayout.FilterFieldIds [ 3 ];

      cmdParms [ 30 ].Value = EntityLayout.FilterFieldIds [ 4 ];
      cmdParms [ 31 ].Value = EntityLayout.Design.FieldReadonlyDisplayFormat;
      cmdParms [ 32 ].Value = this.ClassParameters.UserProfile.UserId;
      cmdParms [ 33 ].Value = this.ClassParameters.UserProfile.CommonName;
      cmdParms [ 34 ].Value = DateTime.Now;

    }//END SetParameters class.

    #endregion

    #region Layout Reader

    // =====================================================================================
    /// <summary>
    /// This class extracts the content of the reader and loads the Checklist object 
    /// </summary>
    /// <param name="Row">DataRow: a data row object containing the query results</param>
    /// <returns>EvForm: a form data object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Extract the compatible data row values to the form object. 
    /// 
    /// 2. Update the form state and type to current enumeration.
    /// 
    /// 3. Return the form data object.
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    private EdRecord getRowData ( DataRow Row )
    {
      //
      // Initialize the method status, a return form object and the title string.
      //
      this.LogMethod ( "getRowData method" );

      EdRecord layout = new EdRecord ( );

      string Title = String.Empty;

      // 
      // Extract the compatible data row values to the form object. 
      // 
      layout.Guid = EvSqlMethods.getGuid ( Row, EdEntityLayouts.DB_LAYOUT_GUID );
      layout.LayoutGuid = layout.Guid;
      layout.LayoutId = EvSqlMethods.getString ( Row, EdEntityLayouts.DB_LAYOUT_ID );
      layout.State = Evado.Model.EvStatics.parseEnumValue<EdRecordObjectStates> (
        EvSqlMethods.getString ( Row, DB_STATE ) );
      layout.Design.Title = EvSqlMethods.getString ( Row, EdEntityLayouts.DB_TITLE );
      layout.Design.HttpReference = EvSqlMethods.getString ( Row, EdEntityLayouts.DB_HTTP_REFERENCE );
      layout.Design.Instructions = EvSqlMethods.getString ( Row, EdEntityLayouts.DB_INSTRUCTIONS );
      layout.Design.Description = EvSqlMethods.getString ( Row, EdEntityLayouts.DB_DESCRIPTION );
      layout.Design.UpdateReason = Evado.Model.EvStatics.parseEnumValue<EdRecord.UpdateReasonList> (
        EvSqlMethods.getString ( Row, EdEntityLayouts.DB_UPDATE_REASON ) );
      layout.Design.RecordCategory = EvSqlMethods.getString ( Row, EdEntityLayouts.DB_RECORD_CATEGORY );

      layout.Design.TypeId = Evado.Model.EvStatics.parseEnumValue<EdRecordTypes> (
         EvSqlMethods.getString ( Row, EdEntityLayouts.DB_TYPE_ID ) );
      layout.Design.Version = EvSqlMethods.getFloat ( Row, EdEntityLayouts.DB_VERSION );

      layout.Design.JavaScript = EvSqlMethods.getString ( Row, EdEntityLayouts.DB_JAVA_SCRIPT );
      layout.Design.hasCsScript = EvSqlMethods.getBool ( Row, EdEntityLayouts.DB_HAS_CS_SCRIPT );
      layout.Design.Language = EvSqlMethods.getString ( Row, EdEntityLayouts.DB_LANGUAGE );
      layout.Design.ReadAccessRoles = EvSqlMethods.getString ( Row, EdEntityLayouts.DB_READ_ACCESS_ROLES );
      layout.Design.EditAccessRoles = EvSqlMethods.getString ( Row, EdEntityLayouts.DB_EDIT_ACCESS_ROLES );

      layout.Design.ParentEntities = EvSqlMethods.getString ( Row, EdEntityLayouts.DB_PARENT_ENTITIES );
      layout.Design.DefaultPageLayout = EvSqlMethods.getString ( Row, EdEntityLayouts.DB_DEFAULT_PAGE_LAYOUT );

      string value = EvSqlMethods.getString ( Row, EdEntityLayouts.DB_LINK_CONTENT_SETTING );
      if ( value != String.Empty )
      {
        layout.Design.LinkContentSetting =
          Evado.Model.EvStatics.parseEnumValue<EdRecord.LinkContentSetting> ( value );
      }
      layout.Design.DisplayRelatedEntities = EvSqlMethods.getBool ( Row, EdEntityLayouts.DB_DISPLAY_ENTITIES );
      layout.Design.DisplayAuthorDetails = EvSqlMethods.getBool ( Row, EdEntityLayouts.DB_DISPLAY_AUTHOR_DETAILS );
      layout.Design.RecordPrefix = EvSqlMethods.getString ( Row, EdEntityLayouts.DB_ENTITY_PREFIX );
      layout.Design.ParentType = EvSqlMethods.getString<EdRecord.ParentTypeList> ( Row, EdEntityLayouts.DB_PARENT_TYPE );
      layout.Design.AuthorAccess = EvSqlMethods.getString<EdRecord.AuthorAccessList> ( Row, EdEntityLayouts.DB_PARENT_ACCESS );
      layout.Design.ParentEntities = EvSqlMethods.getString ( Row, EdEntityLayouts.DB_PARENT_ENTITIES );
      layout.Design.HeaderFormat = EvSqlMethods.getString<EdRecord.HeaderFormat> ( Row, EdEntityLayouts.DB_HEADER_FORMAT );
      layout.Design.FooterFormat = EvSqlMethods.getString<EdRecord.FooterFormat> ( Row, EdEntityLayouts.DB_FOOTER_FORMAT );

      layout.Design.FieldReadonlyDisplayFormat = EvSqlMethods.getString<EdRecord.FieldReadonlyDisplayFormats> (
          Row, EdEntityLayouts.DB_FIELD_DISPLAY_FORMAT );

      layout.FilterFieldIds [ 0 ] = EvSqlMethods.getString ( Row, EdEntityLayouts.DB_FILTER_FIELD_0 );
      layout.FilterFieldIds [ 1 ] = EvSqlMethods.getString ( Row, EdEntityLayouts.DB_FILTER_FIELD_1 );
      layout.FilterFieldIds [ 2 ] = EvSqlMethods.getString ( Row, EdEntityLayouts.DB_FILTER_FIELD_2 );
      layout.FilterFieldIds [ 3 ] = EvSqlMethods.getString ( Row, EdEntityLayouts.DB_FILTER_FIELD_3 );
      layout.FilterFieldIds [ 4 ] = EvSqlMethods.getString ( Row, EdEntityLayouts.DB_FILTER_FIELD_4 );

      layout.Updated = EvSqlMethods.getString ( Row, EdEntityLayouts.DB_UPDATED_BY );
      layout.Updated += " on " + EvSqlMethods.getDateTime ( Row, EdEntityLayouts.DB_UPDATED_DATE ).ToString ( "dd MMM yyyy HH:mm" );

      //
      // get layout sections.
      //
      this.getSections ( layout );

      return layout;

    }//END getRowData method.

    // =====================================================================================
    /// <summary>
    /// This class extracts the content of the reader and loads the Checklist object 
    /// </summary>
    /// <param name="Layout">EdRecord: a data row object containing the query results</param>
    //  ---------------------------------------------------------------------------------
    private void getSections ( EdRecord Layout )
    {
      this.LogMethod ( "getSections" );
      //
      // Initialise the methods object and variables.
      //
      EdEntitySections dal_FormSections = new EdEntitySections ( this.ClassParameters );

      //
      // Retrieve the form selection list.
      //
      List<EdRecordSection> sectionList = dal_FormSections.getSectionList ( Layout.Guid );
      if ( sectionList.Count > 0 )
      {
        this.LogValue ( "Form section exist and have been read in." );
        Layout.Design.FormSections = sectionList;
      }
      else
      {
        this.LogValue ( "No form section exist." );
        dal_FormSections.UpdateItem ( Layout );

        this.LogValue ( dal_FormSections.Log );
      }
      this.LogMethodEnd ( "getSections" );
    }

    #endregion

    #region Form record Queries

    // =====================================================================================
    /// <summary>
    /// This class returns a list of form object items based on VisitId, RecordTypeId, state and orderby value
    /// </summary>
    /// <param name="ApplicationId">string: (Mandatory) a trial identifier for the query.</param>
    /// <param name="TypeId">EvFormRecordTypes: a form record type identifier.</param>
    /// <param name="State">EvForm.FormObjecStates: a form object state identifier.</param>
    /// <param name="WithFields">Boolean: true = include fields</param>
    /// <returns>List of EvForm: a list of form data object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Return an empty forms list if the trial identifier is empty 
    /// 
    /// 2. Define the sql query parameters and sql query string.
    /// 
    /// 3. Execute the sql query string and store the results on the data table 
    /// 
    /// 4. Loop through the table and extract the data row to the form object. 
    /// 
    /// 5. Add the form object value to the Forms list. 
    /// 
    /// 6. Return the Forms list. 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public List<EdRecord> getLayoutList (
      EdRecordTypes TypeId,
      EdRecordObjectStates State,
      bool WithFields )
    {
      this.LogMethod ( "GetFormList method." );
      this.LogDebug ( "TypeId: '{0}'", TypeId );
      this.LogDebug ( "State: '{0}'", State );
      //
      // Initialize the method status and a return list of form object 
      //
      List<EdRecord> view = new List<EdRecord> ( );

      //
      // Initalise the parameters for the query
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter( PARM_TYPE_ID, SqlDbType.VarChar, 30),
        new SqlParameter( PARM_STATE, SqlDbType.VarChar, 20)
      };
      cmdParms [ 0 ].Value = TypeId.ToString ( );
      cmdParms [ 1 ].Value = State.ToString ( );

      //
      // Build the query string
      // 
      _sqlQueryString = _sqlQuery_View
        + "WHERE ( (" + EdEntityLayouts.DB_DELETED + " = 0 )";

      if ( TypeId != EdRecordTypes.Null )
      {
        _sqlQueryString += "AND ( " + EdEntityLayouts.DB_TYPE_ID + " = " + EdEntityLayouts.PARM_TYPE_ID + " ) ";
      }

      if ( State != EdRecordObjectStates.Null )
      {
        _sqlQueryString += "AND ( " + EdEntityLayouts.DB_STATE + " = " + EdEntityLayouts.PARM_STATE + " ) ";
      }
      else
      {
        _sqlQueryString += "AND NOT( " + EdEntityLayouts.DB_STATE + " = '" + EdRecordObjectStates.Withdrawn + "' ) ";

      }

      _sqlQueryString += ") ORDER BY " + EdEntityLayouts.DB_LAYOUT_ID + "," + EdEntityLayouts.DB_STATE + ";";

      this.LogDebug ( _sqlQueryString );

      //
      //Execute the query against the database
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

          EdRecord layout = this.getRowData ( row );

          if ( WithFields == true )
          {
            this.getLayoutFields ( layout );
          }

          view.Add ( layout );
        }//END record iteration loop

      }//END using method
      this.LogValue ( " Count: " + view.Count.ToString ( ) );

      //
      // return the Array object.
      // 
      this.LogMethodEnd ( "GetFormList" );
      return view;

    }//END GetView method.

    // ==================================================================================
    /// <summary>
    /// This method retrieves the layout's field objects.
    /// </summary>
    /// <param name="Layout">EdRecord object</param>
    //  ---------------------------------------------------------------------------------
    private void getLayoutFields ( EdRecord Layout )
    {
      this.LogMethod ( "getLayoutFields" );
      //
      // initialise the methods variables and objects.
      //
      EdEntityFields dal_EntityFields = new EdEntityFields ( this.ClassParameters );

      // 
      // Retrieve the instrument items.
      // 
      Layout.Fields = dal_EntityFields.GetFieldList ( Layout.Guid );
      this.LogClass ( dal_EntityFields.Log );

      this.LogMethodEnd ( "getLayoutFields" );
    }

    // =====================================================================================
    /// <summary>
    /// This class returns a list of option object based on the VisitId, RecordTypeId, state and selectByGuid condition
    /// </summary>
    /// <param name="ApplicationId">string: (Mandatory) a trial identifier.</param>
    /// <param name="TypeId">EvFormRecordTypes: a form record type identifier</param>
    /// <param name="State">EvForm.FormObjecStates: a form object state identifier</param>
    /// <param name="SelectByGuid">Boolean: true, if the list is selected by Guid</param>
    /// <returns>List of EvOption: a list of option object items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define the sql query parameters and sql query string.
    /// 
    /// 2. Execute the sql query string and store the results on the data table 
    /// 
    /// 3. Loop through the table and extract the data row to the form object. 
    /// 
    /// 4. Add the form object value to the Options list. 
    /// 
    /// 5. Return the Options list. 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public List<EvOption> GetList (
      EdRecordTypes TypeId,
      EdRecordObjectStates State, bool SelectByGuid )
    {
      //
      // Initialize the method status, a return option list and an option object
      //
      this.LogMethod ( "GetList." );
      this.LogValue ( "TypeId: " + TypeId );
      this.LogValue ( "State: " + State );
      this.LogValue ( "SelectByUid: " + SelectByGuid );

      List<EvOption> list = new List<EvOption> ( );
      EvOption option = new EvOption ( );
      list.Add ( option );
      List<EdRecord> recordList = new List<EdRecord> ( );

      //
      // Build the query string
      // 
      recordList = this.getLayoutList ( TypeId, State, false );

      // 
      // Iterate through the results extracting the role information.
      // 
      foreach ( EdRecord record in recordList )
      {
        //
        // If SelectByGuid = True then optionId is to be the objects Checklist UID
        //
        if ( SelectByGuid == true )
        {
          option = new EvOption ( record.Guid.ToString ( ),
            record.LayoutId + " - " + record.Title );
        }
        //
        // If SelectByGuid = False then optionId is to be the objects FieldId.
        //
        else
        {
          option = new EvOption ( record.LayoutId,
            record.LayoutId + " - " + record.Title );
        }
        list.Add ( option );

      }//END iteration loop

      //
      // Return the ArrayList.
      //
      return list;

    }//END GetList class

    // =====================================================================================
    /// <summary>
    /// This class returns a list of option object based on form object
    /// </summary>
    /// <param name="Layout">EvForm: (Mandatory) The EvForm object being updated.</param>
    /// <returns>List of EvOption: a list of option object items</returns>
    /// <remarks>
    /// This method consists of the followings steps: 
    /// 
    /// 1. Define the sql query parameters and sql query string.
    /// 
    /// 2. Execute the sql query string and store the results on the data table 
    /// 
    /// 3. Loop through the table and extract the data row to the form object. 
    /// 
    /// 4. Add the form object value to the Options list. 
    /// 
    /// 5. Return the Options list. 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    private List<EvOption> GetValidationList ( EdRecord Layout )
    {
      //
      // Initialize the method status and the return option list
      //
      this.LogMethod ( "GetValidationList, method. " );
      this.LogValue ( "TypeId: " + Layout.Design.TypeId );

      List<EvOption> list = new List<EvOption> ( );

      //
      // Initialise the query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter( PARM_TYPE_ID, SqlDbType.VarChar, 30),
      };
      cmdParms [ 0 ].Value = Layout.Design.TypeId;

      //
      // Build the query string
      // 
      _sqlQueryString = _sqlQuery_View
        + "WHERE (" + EdEntityLayouts.DB_TYPE_ID + " = " + EdEntityLayouts.PARM_TYPE_ID + " ) "
        + " AND ( (" + EdEntityLayouts.DB_STATE + " = '" + EdRecordObjectStates.Form_Draft + "' ) "
        + "  OR (" + EdEntityLayouts.DB_STATE + " = '" + EdRecordObjectStates.Form_Reviewed + "') ) "
        + " ORDER BY " + EdEntityLayouts.DB_LAYOUT_ID + ";";

      this.LogValue ( _sqlQueryString );

      //
      // Execute the query against the database
      //
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

          EvOption option = new EvOption (
              EvSqlMethods.getString ( row, EdEntityLayouts.DB_LAYOUT_ID ),
              EvSqlMethods.getString ( row, EdEntityLayouts.DB_LAYOUT_ID )
              + " - " + EvSqlMethods.getString ( row, EdEntityLayouts.DB_TITLE ) );

          this.LogValue ( " " + option.Value + " " + option.Description );

          list.Add ( option );

        }//END iteration loop

      }//END Using method

      this.LogValue ( "count: " + list.Count );

      //
      // Return the ArrayList.
      //
      this.LogMethodEnd ( "GetValidationList" );
      return list;

    }//END GetValidationList class

    #endregion

    #region Retrieval Queries


    // =====================================================================================
    /// <summary>
    /// This class retrieves a form data object using FormGuid
    /// </summary>
    /// <param name="FormGuid">Guid: a form global unique identifier</param>
    /// <returns>EvForm: a form object</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Return an empty form object if the FormGuid is empty.
    /// 
    /// 3. Define the sql query parameters and sql query string. 
    /// 
    /// 4. Execute the sql query string and store the results on data table. 
    /// 
    /// 5. Extract the first data row to the return form object.
    /// 
    /// 6. Add the formfield items to the form object. 
    /// 
    /// 7. Return the form data object.
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public EdRecord GetLayout ( Guid FormGuid )
    {
      //
      // Initialize the method status, a return form object and a form field object 
      //
      this.LogMethod ( "getLayout merhod. " );
      this.LogValue ( "Form Guid: " + FormGuid );

      EdRecord layout = new EdRecord ( );
      //EvFormsSections FormSection = new EvFormsSections ( );
      layout.LayoutId = String.Empty;

      //
      // Validate whether the formGuid is not empty.
      //
      if ( FormGuid == Guid.Empty )
      {
        return layout;
      }

      // 
      // Generate the Selection query string.
      // 
      this._sqlQueryString = _sqlQuery_View
        + " WHERE (" + EdEntityLayouts.DB_LAYOUT_GUID + " = " + EdEntityLayouts.PARM_LAYOUT_GUID + ");";


      this.LogValue ( _sqlQueryString );
      //
      // Initialise the query parameters.
      //

      SqlParameter cmdParms = new SqlParameter ( PARM_LAYOUT_GUID, SqlDbType.UniqueIdentifier );
      cmdParms.Value = FormGuid;

      //
      //Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( _sqlQueryString, cmdParms ) )
      {
        // 
        // If not rows the return
        // 
        if ( table.Rows.Count == 0 )
        {
          return layout;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        // 
        // Fill the role object.
        // 
        layout = this.getRowData ( row );

      }//END Using method

      // 
      // Retrieve the layout field items.
      // 
      this.getLayoutFields ( layout );

      //
      // Return Checklist data object.
      //
      return layout;

    }//END getForm method

    // ====================================================================================
    /// <summary>
    /// This class returns a form data object using TrialId, FormId and Issued condition
    /// </summary>
    /// <param name="LayoutId">string: a form identifier</param>
    /// <param name="Issued">Boolean: true, if the form is issued</param>
    /// <returns>EvForm: a form data object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Return an empty form object if the FormGuid is empty.
    /// 
    /// 3. Define the sql query parameters and sql query string. 
    /// 
    /// 4. Execute the sql query string and store the results on data table. 
    /// 
    /// 5. Extract the first data row to the return form object.
    /// 
    /// 6. Add the formfield items to the form object. 
    /// 
    /// 7. Return the form data object. 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public EdRecord GetLayout (
      string LayoutId,
      bool Issued )
    {
      this.LogMethod ( "getForm method. " );
      this.LogDebug ( "LayoutId: " + LayoutId );
      //
      // Initialize the method status, a return form object and a formfield object
      //
      EdRecord layout = new EdRecord ( );

      //
      // If the FieldId is null then return empty instrument object.
      //
      if ( LayoutId == String.Empty )
      {
        return layout;
      }


      //
      // Initialise the query parameters.
      //
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter( EdEntityLayouts.PARM_LAYOUT_ID, SqlDbType.Char, 10)
      };
      cmdParms [ 0 ].Value = LayoutId;

      // 
      // Generate the Selection query string.
      // 
      this._sqlQueryString = _sqlQuery_View
        + " WHERE (" + EdEntityLayouts.DB_DELETED + " = 0 )"
        + "   AND (" + EdEntityLayouts.DB_LAYOUT_ID + " = " + EdEntityLayouts.PARM_LAYOUT_ID + " ) "
        + "   AND NOT (" + EdEntityLayouts.DB_STATE + " = '" + EdRecordObjectStates.Withdrawn + "' ) ";


      if ( Issued == true )
      {
        _sqlQueryString += " AND (" + EdEntityLayouts.DB_STATE + " = '" + EdRecordObjectStates.Form_Issued + "'); ";
      }
      else
      {
        _sqlQueryString += " AND NOT(" + EdEntityLayouts.DB_STATE + " = '" + EdRecordObjectStates.Form_Issued + "'); ";
      }
      this.LogValue ( _sqlQueryString );

      //
      //Execute the query against the database
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( _sqlQueryString, cmdParms ) )
      {
        // 
        // If not rows the return
        // 
        if ( table.Rows.Count == 0 )
        {
          return layout;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        // 
        // Fill the role object.
        // 
        layout = this.getRowData ( row );

      }//END Using method

      // 
      // Retrieve the layout fields.
      // 
      this.getLayoutFields ( layout );

      //
      // Return the Checklist data object.
      //
      return layout;

    }//END getForm method

    #endregion

    #region Form Update queries

    // =====================================================================================
    /// <summary>
    /// This class updates the items on form data table using a retrieved form data object.
    /// </summary>
    /// <param name="Layout">EvForm: a retrieved form data object</param>
    /// <returns>EvEventCodes: an event code for updating the form data table</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the Form's identifier is duplicated 
    /// 
    /// 2. Generate new DB Guid, if the Form's Guid is empty.
    /// 
    /// 3. Set the datachange object. 
    /// 
    /// 4. Define the sql query parameter and execute the storeprocedure for updating items. 
    /// 
    /// 5. Add datachange values to the backup data changes
    /// 
    /// 6. Return the event code for updating items.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes UpdateItem ( EdRecord Layout )
    {
      this.LogMethod ( "updateItem() method." );
      this.LogValue ( "RecordPrefix." + Layout.Design.RecordPrefix );
      // 
      // Initialise the method status and the old form object
      // 
      EvDataChanges dataChanges = new EvDataChanges ( );

      EdRecord oldForm = this.GetLayout ( Layout.Guid );
      this.LogValue ( "old form Form Id." + oldForm.LayoutId );

      //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      //
      // Get the list of forms for this trial.
      //
      List<EvOption> formList = this.GetValidationList ( Layout );

      //
      // Iterate through the forms to check that there are no duplicates.
      //
      foreach ( EvOption form in formList )
      {
        this.LogValue ( "oldForm FormId: " + oldForm.LayoutId
          + " Form FormId: " + Layout.LayoutId + " FormId: " + form.Value );
        //
        // IF the old form Id does not match the updated Form Id 
        // it has been changed. So the new form id needs to be validated.
        //
        if ( oldForm.LayoutId.ToLower ( ) != Layout.LayoutId.ToLower ( )
          && form.Value.ToLower ( ) == Layout.LayoutId.ToLower ( ) )
        {
          this.LogValue ( " >> Duplicate form Id." );

          return EvEventCodes.Data_Duplicate_Id_Error;
        }
      }//EMD milestone iteration loop.

      // 
      // Generate the DB row guid
      // 
      if ( Layout.Guid == Guid.Empty )
      {
        Layout.Guid = Guid.NewGuid ( );
      }

      // 
      // Set the data change object values.
      // 
      EvDataChange dataChange = this.setChangeRecord ( Layout, oldForm );

      // 
      // Define the query parameters
      // 
      SqlParameter [ ] cmdParms = GetParameters ( );
      SetParameters ( cmdParms, Layout );

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( STORED_PROCEDURE_UPDATE_ITEM, cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      //
      // Update the form sections.
      //
      this.LogValue ( "Saving sections." );

      EvEventCodes result = this.updateSections ( Layout );

      if ( result != EvEventCodes.Ok )
      {
        return result;
      }

      // 
      // Add the change record
      // 
      dataChanges.AddItem ( dataChange );

      return EvEventCodes.Ok;

    }//END UpdateItem class

    // =====================================================================================
    /// <summary>
    /// This class adds new items to form data table based on retrieved form data objecgt
    /// </summary>
    /// <param name="Layout">EvForm: a retrieved form data object</param>
    /// <returns>EvEventCodes: an event code for adding items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the form's Guid is duplicated. 
    /// 
    /// 2. Set new Guid for new form object. 
    /// 
    /// 3. Define the sql query parameters and execute the storeprocedure for adding items. 
    /// 
    /// 4. Add new related formfield items to form table if they exist.
    /// 
    /// 5. Return the event code for adding new items.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes AddItem ( EdRecord Layout )
    {
      this.LogMethod ( "AddItem method. " );
      this.LogValue ( "FormId: " + Layout.LayoutId );
      this.LogValue ( "Version: " + Layout.Design.Version );
      // 
      // Initialise the methods status, a formfield object and a new Guid
      // 

      //
      // Validate whether the Guid does not exist.
      //
      EdRecord currentForm = this.GetLayout ( Layout.LayoutId, false );

      if ( currentForm.Guid != Guid.Empty )
      {
        this.LogValue ( "Duplicate form Id. Version: " + Layout.Version );

        return EvEventCodes.Data_Duplicate_Id_Error;
      }

      // 
      // Set the Guid for new form
      // 
      Layout.Guid = Guid.NewGuid ( );

      // 
      // Define the query parameters
      // 
      SqlParameter [ ] cmdParms = GetParameters ( );
      SetParameters ( cmdParms, Layout );

      this.LogDebug ( EvSqlMethods.getParameterSqlText ( cmdParms ) );

      //
      // Execute the update command.
      //
      try
      {
        if ( EvSqlMethods.StoreProcUpdate ( STORED_PROCEDURE_ADD_ITEM, cmdParms ) == 0 )
        {
          this.LogValue ( "Errors adding the form object to the database." );
          this.LogValue ( EvSqlMethods.Log );

          return EvEventCodes.Database_Record_Update_Error;
        }
      }
      catch ( Exception Ex )
      {
        this.LogException ( Ex );

        return EvEventCodes.Database_Record_Update_Error;
      }

      //
      // Update the form sections.
      //
      this.LogValue ( "Saving sections." );

      EvEventCodes result = this.updateSections ( Layout );

      if ( result != EvEventCodes.Ok )
      {
        return result;
      }

      // 
      // If form fields of the new common form exists, add the new form fields to database. 
      // 
      if ( Layout.Guid != Guid.Empty
        && Layout.Fields.Count > 0 )
      {
        this.updateFields ( Layout );

      }//END if form fields. 

      // 
      // Return the object FormUid.
      // 
      return EvEventCodes.Ok;

    }//END AddItem method

    // ==================================================================================
    /// <summary>
    /// This method updates the record layout sections.
    /// </summary>
    /// <param name="Layout">EdRecord objecty.</param>
    /// <returns>EvEventCode enumerated value.</returns>
    // ----------------------------------------------------------------------------------
    private EvEventCodes updateFields ( EdRecord Layout )
    {
      this.LogMethod ( "updateFields" );
      //
      // Initialise the methods variables and objects.
      //
      EdRecordFields dal_LayoutFields = new EdRecordFields ( this.ClassParameters );

      //
      // update the layout sections.
      //
      EvEventCodes result = dal_LayoutFields.UpdateFields ( Layout );

      this.LogDebugClass ( dal_LayoutFields.Log );
      this.LogDebug ( "Result {0}.", result );
      this.LogMethodEnd ( "updateSections" );
      return result;

    }

    // ==================================================================================
    /// <summary>
    /// This method updates the record layout sections.
    /// </summary>
    /// <param name="Layout">EdRecord objecty.</param>
    /// <returns>EvEventCode enumerated value.</returns>
    // ----------------------------------------------------------------------------------
    private EvEventCodes updateSections ( EdRecord Layout )
    {
      this.LogMethod ( "updateSections" );
      this.LogDebug ( "Section Count: " + Layout.Design.FormSections.Count );
      //
      // Initialise the methods variables and objects.
      //
      EdEntitySections dal_LayoutSections = new EdEntitySections ( this.ClassParameters );

      //
      // update the layout sections.
      //
      EvEventCodes result = dal_LayoutSections.UpdateItem ( Layout );

      this.LogDebugClass ( dal_LayoutSections.Log );
      this.LogDebug ( "Result {0}.", result );
      this.LogMethodEnd ( "updateSections" );
      return result;

    }

    // =====================================================================================
    /// <summary>
    /// This class withdraws the currently issued version of the form object.
    /// </summary>
    /// <param name="Form">EvForm: a form data object</param>
    /// <returns>EvEventCodes: an event code for withdrawing issued form</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Validate whether the formId and user common name are not empty
    /// 
    /// 2. Define the sql query parameters and execute the storeprocedure for withdrawing issued form
    /// 
    /// 3. Return an event code for withdrawing issued form
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes WithdrawIssuedForm ( EdRecord Form )
    {
      // 
      // Validate whether the formId and user common name are not empty
      // 
      if ( Form.LayoutId == String.Empty )
      {
        return EvEventCodes.Identifier_Project_Id_Error;
      }

      // 
      // Define the query parameters
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter( EdEntityLayouts.PARM_LAYOUT_ID, SqlDbType.NVarChar, 10),
        new SqlParameter( EdEntityLayouts.PARM_UPDATED_BY_USER_ID, SqlDbType.NVarChar,100),
        new SqlParameter( EdEntityLayouts.PARM_UPDATED_BY, SqlDbType.NVarChar,30),
        new SqlParameter( EdEntityLayouts.PARM_UPDATED_DATE, SqlDbType.DateTime),
      };
      cmdParms [ 0 ].Value = Form.LayoutId;
      cmdParms [ 1 ].Value = this.ClassParameters.UserProfile.UserId;
      cmdParms [ 2 ].Value = this.ClassParameters.UserProfile.CommonName;
      cmdParms [ 3 ].Value = DateTime.Now;

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( STORED_PROCEDURE_WithdrawItem, cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      // 
      // Return the object FormUid.
      // 
      return EvEventCodes.Ok;

    }// End deleteYear method.

    // =====================================================================================
    /// <summary>
    /// This class deletes the items from form data table
    /// </summary>
    /// <param name="Form">EvForm: a form data object</param>
    /// <returns>EvEventCodes: an event code for deleting items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the form's Guid is duplicated or User Common Name is empty. 
    /// 
    /// 2. Define the sql query parameters and execute the storeprocedure for deleting items. 
    /// 
    /// 3. Return an event code for deleting items.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes DeleteItem ( EdRecord Form )
    {
      //
      // Initialize the method status
      //
      this.LogMethod ( "DeleteItem method. FormId; " + Form.LayoutId );

      // 
      // Validate whether the form Guid and user common name are not empty.
      // 
      if ( Form.Guid == Guid.Empty )
      {
        return EvEventCodes.Identifier_Global_Unique_Identifier_Error;
      }

      // 
      // Define the query parameters
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter(EdEntityLayouts.PARM_LAYOUT_GUID, SqlDbType.UniqueIdentifier ),
        new SqlParameter( EdEntityLayouts.PARM_UPDATED_BY_USER_ID, SqlDbType.NVarChar,100),
        new SqlParameter( EdEntityLayouts.PARM_UPDATED_BY, SqlDbType.NVarChar,30),
        new SqlParameter( EdEntityLayouts.PARM_UPDATED_DATE, SqlDbType.DateTime),
      };
      cmdParms [ 0 ].Value = Form.Guid;
      cmdParms [ 1 ].Value = this.ClassParameters.UserProfile.UserId;
      cmdParms [ 2 ].Value = this.ClassParameters.UserProfile.CommonName;
      cmdParms [ 3 ].Value = DateTime.Now;

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( STORED_PROCEDURE_DELETE_ITEM, cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      // 
      // Return the object FormUid.
      // 
      return EvEventCodes.Ok;

    }//End DeleteItem method.

    #endregion

    #region Set Difference methods

    // =====================================================================================
    /// <summary>
    /// This class sets the data change object for the update action.
    /// </summary>
    /// <param name="RecordForm">EvForm: a form object</param>
    /// <param name="OldRecordForm">EvForm: an old form object</param>
    /// <returns>EvDataChange: a data change object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Add new items to datachange object if they do not exist. 
    /// 
    /// 2. Return the datachange data object.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private EvDataChange setChangeRecord ( EdRecord RecordForm, EdRecord OldRecordForm )
    {
      //
      // Initialize the method status and the datachange object. 
      //
      this.LogMethod ( "setChangeRecord method. " );

      EvDataChange dataChange = new EvDataChange ( );
      dataChange.Guid = Guid.NewGuid ( );
      dataChange.TableName = EvDataChange.DataChangeTableNames.EvForms;
      dataChange.RecordGuid = RecordForm.Guid;
      dataChange.DateStamp = DateTime.Now;

      //
      // Add new items to datachange object if they do not exist. 
      //
      dataChange.AddItem ( "FormId", OldRecordForm.LayoutId, RecordForm.LayoutId );

      dataChange.AddItem ( "Title", OldRecordForm.Design.Title, RecordForm.Design.Title );

      dataChange.AddItem ( "Reference", OldRecordForm.Design.HttpReference, RecordForm.Design.HttpReference );

      dataChange.AddItem ( "Instructions", OldRecordForm.Design.Instructions, RecordForm.Design.Instructions );

      dataChange.AddItem ( "TypeId", OldRecordForm.Design.TypeId, RecordForm.Design.TypeId );

      dataChange.AddItem ( "Version", OldRecordForm.Design.Version, RecordForm.Design.Version );

      dataChange.AddItem ( "State", OldRecordForm.State, RecordForm.State );

      //
      // Process the form design object.
      //
      if ( OldRecordForm.Design != null
        && RecordForm.Design != null )
      {

        dataChange.AddItem ( "Design_FormCategory",
          OldRecordForm.Design.RecordCategory,
          RecordForm.Design.RecordCategory );

        for ( int count = 0; count < RecordForm.Design.FormSections.Count; count++ )
        {
          EdRecordSection oldSection = new EdRecordSection ( );
          EdRecordSection newSection = RecordForm.Design.FormSections [ count ];

          if ( count < OldRecordForm.Design.FormSections.Count )
          {
            oldSection = OldRecordForm.Design.FormSections [ count ];
          }
          dataChange.AddItem ( "Section_" + count + "_No",
            oldSection.No,
            newSection.No );

          dataChange.AddItem ( "Section_" + count + "_Section",
            oldSection.Title,
            newSection.Title );

          dataChange.AddItem ( "Section_" + count + "_Instructions",
            oldSection.Instructions,
            newSection.Instructions );

          dataChange.AddItem ( "Section_" + count + "_FieldName",
            oldSection.FieldId,
            newSection.FieldId );

          dataChange.AddItem ( "Section_" + count + "_FieldValue",
            oldSection.FieldValue,
            newSection.FieldValue );

          dataChange.AddItem ( "Section_" + count + "_DefaultDisplayRoles",
            oldSection.ReadAccessRoles,
            newSection.ReadAccessRoles );

          dataChange.AddItem ( "Section_" + count + "_DefaultEditRoles",
            oldSection.EditAccessRoles,
            newSection.EditAccessRoles );

          dataChange.AddItem ( "Section_" + count + "_Order",
            oldSection.Order,
            newSection.Order );

          dataChange.AddItem ( "Section_" + count + "_Visible",
            oldSection.OnOpenVisible,
            newSection.OnOpenVisible );

        }//END section iteration loop

        dataChange.AddItem ( "Design_hasCsScript",
          OldRecordForm.Design.hasCsScript,
          RecordForm.Design.hasCsScript );

        dataChange.AddItem ( "Design_HiddenFields",
          OldRecordForm.Design.JavaScript,
          RecordForm.Design.JavaScript );

        dataChange.AddItem ( "Design_Reference",
          OldRecordForm.Design.HttpReference,
          RecordForm.Design.HttpReference );
      }//END Form Design object
      // 
      // Return the data change object.
      // 
      return dataChange;

    }//END setChangeRecord method

    #endregion

    #region Copy form methods

    // =====================================================================================
    /// <summary>
    /// This class copies items to form table. 
    /// </summary>
    /// <param name="Layout">EvForm: a form data object</param>
    /// <param name="Copy">Boolean: true, if the form is copied</param>
    /// <returns>EvEventCodes: an event code for copying form object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the form's Guid is duplicated or user common name is empty. 
    /// 
    /// 3. Update the version if revising the form.
    /// 
    /// 4. Define the sql query parameters and execute the storeprocedure for copying form
    /// 
    /// 5. Return the event code for copying items.
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public EvEventCodes CopyLayout ( EdRecord Layout, bool Copy )
    {
      //
      // Initialize the method status, a number of affected records and a numeric version
      //
      this.LogMethod ( "CopyForm");
      this.LogDebug ( "LayoutId {0}. ", Layout.LayoutId );
      this.LogDebug ( "Copy {0}. ", Copy );

      int databaseRecordAffected = 0;
      string layoutId = Layout.LayoutId;
      string title = Layout.Title;
      float version = Layout.Design.Version;

      // 
      // Validate whether the form Guid and user common name are not empty
      // 
      if ( Layout.Guid == Guid.Empty )
      {
        return EvEventCodes.Identifier_Global_Unique_Identifier_Error;
      }

      // 
      // Update the version if revising the form.
      // 
      if ( Copy == false )
      {
        //---------------------- Check for duplicate Checklist identifiers. ------------------
        EdRecord currentForm = this.GetLayout ( Layout.LayoutId, false );

        if ( currentForm.Guid != Guid.Empty )
        {
          this.LogValue ( "Duplicate form Id. Version: " + Layout.Version );

          return EvEventCodes.Data_Duplicate_Id_Error;
        }

        version += 0.01F;
      }
      else
      {
        layoutId = Layout.LayoutId + "_CPY";
        title = Layout.Title + " (COPY)";
        version = 0.0F;
      }
      this.LogDebug ( "layoutId {0}. ", layoutId );
      this.LogDebug ( "title {0}. ", title );
      this.LogDebug ( "version {0}. ", version );

      // 
      // Define the query parameters
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter(EdEntityLayouts.PARM_LAYOUT_GUID, SqlDbType.UniqueIdentifier ),
        new SqlParameter( EdEntityLayouts.PARM_LAYOUT_ID, SqlDbType.NVarChar, 20),
        new SqlParameter( EdEntityLayouts.PARM_TITLE, SqlDbType.NVarChar, 80),
        new SqlParameter( EdEntityLayouts.PARM_VERSION, SqlDbType.Float),
        new SqlParameter( EdEntityLayouts.PARM_UPDATED_BY_USER_ID, SqlDbType.NVarChar,100),
        new SqlParameter( EdEntityLayouts.PARM_UPDATED_BY, SqlDbType.NVarChar,30),
        new SqlParameter( EdEntityLayouts.PARM_UPDATED_DATE, SqlDbType.DateTime),
      };
      cmdParms [ 0 ].Value = Layout.Guid;
      cmdParms [ 1 ].Value = layoutId;
      cmdParms [ 2 ].Value = title;
      cmdParms [ 3 ].Value = version;
      cmdParms [ 4 ].Value = this.ClassParameters.UserProfile.UserId;
      cmdParms [ 5 ].Value = this.ClassParameters.UserProfile.CommonName;
      cmdParms [ 6 ].Value = DateTime.Now;

      this.LogDebug( EvSqlMethods.getParameterSqlText( cmdParms ) );

      //
      // Execute the update command.
      //
      databaseRecordAffected = EvSqlMethods.StoreProcUpdate ( STORED_PROCEDURE_COPY_ITEM, cmdParms );
      if ( databaseRecordAffected == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      this.LogValue ( " Records affected: " + databaseRecordAffected );
      return EvEventCodes.Ok;

    }//END CopyForm method.
    #endregion

  }//END EvForms class

}//END namespace Evado.Dal.Digital
