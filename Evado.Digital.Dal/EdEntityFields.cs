/***************************************************************************************
 * <copyright file="dal\EvFormFields.cs" company="EVADO HOLDING PTY. LTD.">
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
using System.Xml.Serialization;
using System.IO;
using System.Text;

//Application specific class references.
using Evado.Model;
using Evado.Digital.Model;

namespace Evado.Digital.Dal
{
  /// <summary>
  /// This class is handles the data access layer for the form field data object.
  /// </summary>
  public class EdEntityFields : EvDalBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EdEntityFields ( )
    {
      this.ClassNameSpace = "Evado.Digital.Dal.Digital.EdEntityFields.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="ClassParameters">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EdEntityFields ( EvClassParameters ClassParameters )
    {
      this.ClassParameters = ClassParameters;
      this.ClassNameSpace = "Evado.Digital.Dal.Digital.EdEntityFields.";

      if ( this.ClassParameters.LoggingLevel == 0 )
      {
        this.ClassParameters.LoggingLevel = Evado.Digital.Dal.EvStaticSetting.LoggingLevel;
      }
    }

    #endregion

    #region Object Initialisation
    /* *********************************************************************************
     * 
     * Defines the classes constansts and global variables
     * 
     * *********************************************************************************/


    private const string SQL_FIELD_QUERY_VIEW = "Select *  FROM ED_ENTITY_FIELD_VIEW ";

    /// <summary>
    /// This constant defines the sql query for selecting all items from the formfield analysis list view. 
    /// </summary>
    private const string _sqlDataAnalysisQuery_List = "Select * FROM EvFormField_AnalysisList ";

    private string _sqlQueryString = String.Empty;

    #region Define the class storeprocedure.

    private const string STORED_PROCEDURE_ADD_ITEM = "USR_ENTITY_FIELD_ADD";
    private const string STORED_PROCEDURE_UPDATE_ITEM = "USR_ENTITY_FIELD_UPDATE";
    private const string STORED_PROCEDURE_DELTE_ITEM = "USR_ENTITY_FIELD_DELETE";
    #endregion

    #region Define the class parameters
    // database fields/columns.
    public const string DB_GUID = "EDELF_GUID";
    public const string DB_LAYOUT_GUID = "EDEL_GUID";
    public const string DB_LAYOUT_ID = "EDE_LAYOUT_ID";
    public const string DB_FIELD_ID = "FIELD_ID";
    public const string DB_TYPE_ID = "EDELF_TYPE_ID";
    public const string DB_ORDER = "EDELF_ORDER";
    public const string DB_TITLE = "EDELF_TITLE";
    public const string DB_INSTRUCTIONS = "EDELF_INSTRUCTIONS";
    public const string DB_HTTP_REFERENCE = "EDELF_HTTP_REFERENCE";
    public const string DB_SECTION_ID = "EDELF_SECTION_ID";
    public const string DB_OPTIONS = "EDELF_OPTIONS";
    public const string DB_SUMMARY_FIELD = "EDELF_SUMMARY_FIELD";
    public const string DB_MANDATORY = "EDELF_MANDATORY";
    public const string DB_AI_DATA_POINT = "EDELF_AI_DATA_POINT";
    public const string DB_ANALYTICS_DATA_POINT = "EDELF_ANALYTICS_DATA_POINT";
    public const string DB_HIDDEN = "EDELF_HIDDEN";
    public const string DB_EX_SELECTION_LIST_ID = "EDELF_EX_SELECTION_LIST_ID";
    public const string DB_EX_SELECTION_LIST_CATEGORY = "EDELF_EX_SELECTION_LIST_CATEGORY";
    public const string DB_DEFAULT_VALUE = "EDELF_DEFAULT_VALUE";
    public const string DB_UNIT = "EDELF_UNIT";
    public const string DB_UNIT_SCALING = "EDELF_UNIT_SCALING";
    public const string DB_VALIDATION_LOWER_LIMIT = "EDELF_VALIDATION_LOWER_LIMIT";
    public const string DB_VALIDATION_UPPER_LIMIT = "EDELF_VALIDATION_UPPER_LIMIT";
    public const string DB_ALERT_LOWER_LIMIT = "EDELF_ALERT_LOWER_LIMIT";
    public const string DB_ALERT_UPPER_LIMIT = "EDELF_ALERT_UPPER_LIMIT";
    public const string DB_NORMAL_LOWER_LIMITD = "EDELF_NORMAL_LOWER_LIMIT";
    public const string DB_NORMAL_UPPER_LIMIT = "EDELF_NORMAL_UPPER_LIMIT";
    public const string DB_FIELD_CATEGORY = "EDELF_FIELD_CATEGORY";
    public const string DB_ANALOGUE_LEGEND_START = "EDELF_ANALOGUE_LEGEND_START";
    public const string DB_ANALOGUE_LEGEND_FINISH = "EDELF_ANALOGUE_LEGEND_FINISH";
    public const string DB_FIELD_WIDTH = "EDELF_FIELD_WIDTH";
    public const string DB_FIELD_HEIGHT = "EDELF_FIELD_HEIGHT";
    public const string DB_JAVA_SCRIPT = "EDELF_JAVA_SCRIPT";
    public const string DB_TABLE = "EDELF_TABLE";
    public const string DB_INITIAL_OPTION_LIST = "EDELF_INITIAL_OPTION_LIST";
    public const string DB_INITIAL_VERSION = "EDELF_INITIAL_VERSION";
    public const string DB_DELETED = "EDELF_DELETED";
    public const string DB_LAYOUT_STATE = "EDEL_STATE";
    public const string DB_FIELD_LAYOUT = "EDELF_FIELD_LAYOUT";

    // query parmeters.
    private const string PARM_GUID = "@GUID";
    private const string PARM_LAYOUT_GUID = "@LAYOUT_GUID";
    public const string PARM_LAYOUT_ID = "@LAYOUT_ID";
    public const string PARM_FIELD_ID = "@FIELD_ID";
    public const string PARM_TYPE_ID = "@TYPE_ID";
    private const string PARM_ORDER = "@ORDER";
    public const string PARM_TITLE = "@TITLE";
    private const string PARM_INSTRUCTIONS = "@INSTRUCTIONS";
    private const string PARM_HTTP_REFERENCE = "@HTTP_REFERENCE";
    private const string PARM_SECTION_ID = "@SECTION_ID";
    private const string PARM_OPTIONS = "@OPTIONS";
    private const string PARM_SUMMARY_FIELD = "@SUMMARY_FIELD";
    private const string PARM_MANDATORY = "@MANDATORY";
    private const string PARM_AI_DATA_POINT = "@AI_DATA_POINT";
    private const string PARM_ANALYTICS_DATA_POINT = "@ANALYTICS_DATA_POINT";
    private const string PARM_HIDDEN = "@HIDDEN";
    private const string PARM_EX_SELECTION_LIST_ID = "EX_SELECTION_LIST_ID";
    private const string PARM_EX_SELECTION_LIST_CATEGOR = "EX_SELECTION_LIST_CATEGORY";
    private const string PARM_DEFAULT_VALUE = "@DEFAULT_VALUE";
    private const string PARM_UNIT = "@UNIT";
    private const string PARM_UNIT_SCALING = "@UNIT_SCALING";
    private const string PARM_VALIDATION_LOWER_LIMIT = "@VALIDATION_LOWER_LIMIT";
    private const string PARM_VALIDATION_UPPER_LIMIT = "@VALIDATION_UPPER_LIMIT";
    private const string PARM_ALERT_LOWER_LIMIT = "@ALERT_LOWER_LIMIT";
    private const string PARM_ALERT_UPPER_LIMIT = "@ALERT_UPPER_LIMIT";
    private const string PARM_NORMAL_LOWER_LIMIT = "@NORMAL_LOWER_LIMIT";
    private const string PARM_NORMAL_UPPER_LIMIT = "@NORMAL_UPPER_LIMIT";
    private const string PARM_FIELD_CATEGORY = "@FIELD_CATEGORY";
    private const string PARM_ANALOGUE_LEGEND_START = "@ANALOGUE_LEGEND_START";
    private const string PARM_ANALOGUE_LEGEND_FINISH = "@ANALOGUE_LEGEND_FINISH";
    private const string PARM_FIELD_WIDTH = "@FIELD_WIDTH";
    private const string PARM_FIELD_HEIGHT = "@FIELD_HEIGHT";
    private const string PARM_JAVA_SCRIPT = "@JAVA_SCRIPT";
    private const string PARM_TABLE = "@TABLE";
    private const string PARM_INITIAL_OPTION_LIST = "@INITIAL_OPTION_LIST";
    private const string PARM_INITIAL_VERSION = "@INITIAL_VERSION";
    private const string PARM_FIELD_LAYOUT = "@FIELD_LAYOUT";
    private const string PARM_DELETED = "@DELETED";

    #endregion

    #endregion

    #region Set Query Parameters

    // ==================================================================================
    /// <summary>
    /// This class sets an array of sql query parameters. 
    /// </summary>
    /// <returns>SqlParameter: an array of sql query parameters</returns>
    /// <remarks>
    /// This method consists of the following step: 
    /// 
    /// 1. Create an array of sql query parameters
    /// 
    /// 2. Return an array of sql query parameters. 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    private static SqlParameter [ ] GetParameters ( )
    {
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter( EdEntityFields.PARM_GUID, SqlDbType.UniqueIdentifier),
        new SqlParameter( EdEntityFields.PARM_LAYOUT_GUID, SqlDbType.UniqueIdentifier),
        new SqlParameter( EdEntityFields.PARM_LAYOUT_ID, SqlDbType.NVarChar, 10),
        new SqlParameter( EdEntityFields.PARM_FIELD_ID, SqlDbType.NVarChar, 20),
        new SqlParameter( EdEntityFields.PARM_TYPE_ID, SqlDbType.VarChar, 50),
        new SqlParameter( EdEntityFields.PARM_ORDER, SqlDbType.Int),
        new SqlParameter( EdEntityFields.PARM_TITLE, SqlDbType.VarChar, 150),
        new SqlParameter( EdEntityFields.PARM_INSTRUCTIONS, SqlDbType.NText),
        new SqlParameter( EdEntityFields.PARM_HTTP_REFERENCE, SqlDbType.VarChar, 250),
        new SqlParameter( EdEntityFields.PARM_SECTION_ID, SqlDbType.SmallInt),

        new SqlParameter( EdEntityFields.PARM_OPTIONS, SqlDbType.VarChar, 250),
        new SqlParameter( EdEntityFields.PARM_SUMMARY_FIELD, SqlDbType.Bit),
        new SqlParameter( EdEntityFields.PARM_MANDATORY, SqlDbType.Bit),
        new SqlParameter( EdEntityFields.PARM_AI_DATA_POINT, SqlDbType.Bit),
        new SqlParameter( EdEntityFields.PARM_ANALYTICS_DATA_POINT, SqlDbType.Bit),
        new SqlParameter( EdEntityFields.PARM_HIDDEN, SqlDbType.Bit),
        new SqlParameter( EdEntityFields.PARM_EX_SELECTION_LIST_ID, SqlDbType.NVarChar, 250),
        new SqlParameter( EdEntityFields.PARM_EX_SELECTION_LIST_CATEGOR, SqlDbType.NVarChar, 250),
        new SqlParameter( EdEntityFields.PARM_DEFAULT_VALUE, SqlDbType.NVarChar, 15),
        new SqlParameter( EdEntityFields.PARM_UNIT, SqlDbType.NVarChar, 15),

        new SqlParameter( EdEntityFields.PARM_UNIT_SCALING, SqlDbType.NVarChar, 250),
        new SqlParameter( EdEntityFields.PARM_VALIDATION_LOWER_LIMIT, SqlDbType.Float),
        new SqlParameter( EdEntityFields.PARM_VALIDATION_UPPER_LIMIT, SqlDbType.Float),
        new SqlParameter( EdEntityFields.PARM_ALERT_LOWER_LIMIT, SqlDbType.Float),
        new SqlParameter( EdEntityFields.PARM_ALERT_UPPER_LIMIT, SqlDbType.Float),
        new SqlParameter( EdEntityFields.PARM_NORMAL_LOWER_LIMIT, SqlDbType.Float),
        new SqlParameter( EdEntityFields.PARM_NORMAL_UPPER_LIMIT, SqlDbType.Float),
        new SqlParameter( EdEntityFields.PARM_FIELD_CATEGORY, SqlDbType.VarChar, 50),
        new SqlParameter( EdEntityFields.PARM_ANALOGUE_LEGEND_START, SqlDbType.VarChar, 30),
        new SqlParameter( EdEntityFields.PARM_ANALOGUE_LEGEND_FINISH, SqlDbType.VarChar, 30),

        new SqlParameter( EdEntityFields.PARM_JAVA_SCRIPT, SqlDbType.NText),
        new SqlParameter( EdEntityFields.PARM_TABLE, SqlDbType.NText),
        new SqlParameter( EdEntityFields.PARM_INITIAL_OPTION_LIST, SqlDbType.NVarChar, 250),
        new SqlParameter(EdEntityFields. PARM_INITIAL_VERSION, SqlDbType.Int),
        new SqlParameter( EdEntityFields.PARM_FIELD_LAYOUT, SqlDbType.NVarChar, 250),
        new SqlParameter( EdEntityFields.PARM_FIELD_WIDTH, SqlDbType.Int),
        new SqlParameter( EdEntityFields.PARM_FIELD_HEIGHT, SqlDbType.Int),

      };

      return cmdParms;
    }//END GetParameters class

    // ==================================================================================
    /// <summary>
    /// This class sets values from formfield object to the sql query parameter array. 
    /// </summary>
    /// <param name="cmdParms">SqlParameter: an array of sql query parameters</param>
    /// <param name="EntityField">EvFormField: a FormField data object</param>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. If the data type is a table then seriallise the formfield data table.
    /// 
    /// 2. Update the values from formfield object to the sql query parameters array. 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    private void setParameters ( SqlParameter [ ] cmdParms, EdRecordField EntityField )
    {
      //
      // Initialize the serialized table structure. 
      //
      string serialisedTableStructure = String.Empty;

      //
      // if the data type is a table then seriallise the formfield data table.
      //
      if ( EntityField.TypeId == Evado.Model.EvDataTypes.Special_Matrix
        || EntityField.TypeId == Evado.Model.EvDataTypes.Table )
      {
        this.LogValue ( "Column 1 header text: " + EntityField.Table.Header [ 0 ].Text );

        serialisedTableStructure = Evado.Model.EvStatics.SerialiseXmlObject<EdRecordTable> ( EntityField.Table );
      }

      if ( EntityField.Design.ValidationLowerLimit >= EntityField.Design.ValidationUpperLimit )
      {
        EntityField.Design.ValidationLowerLimit = EvcStatics.CONST_NUMERIC_MINIMUM;
        EntityField.Design.ValidationUpperLimit = EvcStatics.CONST_NUMERIC_MAXIMUM;
      }

      if ( EntityField.Design.AlertLowerLimit >= EntityField.Design.AlertUpperLimit )
      {
        EntityField.Design.AlertLowerLimit = EvcStatics.CONST_NUMERIC_MINIMUM;
        EntityField.Design.AlertUpperLimit = EvcStatics.CONST_NUMERIC_MAXIMUM;
      }

      if ( EntityField.Design.NormalRangeLowerLimit >= EntityField.Design.NormalRangeUpperLimit )
      {
        EntityField.Design.NormalRangeLowerLimit = EvcStatics.CONST_NUMERIC_MINIMUM;
        EntityField.Design.NormalRangeUpperLimit = EvcStatics.CONST_NUMERIC_MAXIMUM;
      }

      //
      // store the media configuration data in the JavaScript field.
      //
      if ( EntityField.TypeId == EvDataTypes.External_Image
        || EntityField.TypeId == EvDataTypes.Streamed_Video )
      {
        EntityField.Design.JavaScript = EntityField.RecordMedia.Data;
      }

      //
      // Update the values from formfield object to the sql query parameters array. 
      //
      cmdParms [ 0 ].Value = EntityField.Guid;
      cmdParms [ 1 ].Value = EntityField.LayoutGuid;
      cmdParms [ 2 ].Value = EntityField.LayoutId;
      cmdParms [ 3 ].Value = EntityField.FieldId;
      cmdParms [ 4 ].Value = EntityField.TypeId;
      cmdParms [ 5 ].Value = EntityField.Design.Order;
      cmdParms [ 6 ].Value = EntityField.Design.Title;
      cmdParms [ 7 ].Value = EntityField.Design.Instructions;
      cmdParms [ 8 ].Value = EntityField.Design.HttpReference;
      cmdParms [ 9 ].Value = EntityField.Design.SectionNo;

      cmdParms [ 10 ].Value = EntityField.Design.Options;
      cmdParms [ 11 ].Value = EntityField.Design.IsSummaryField;
      cmdParms [ 12 ].Value = EntityField.Design.Mandatory;
      cmdParms [ 13 ].Value = EntityField.Design.AiDataPoint;
      cmdParms [ 14 ].Value = EntityField.Design.AnalyticsDataPont;
      cmdParms [ 15 ].Value = EntityField.Design.HideField;
      cmdParms [ 16 ].Value = EntityField.Design.ExSelectionListId;
      cmdParms [ 17 ].Value = EntityField.Design.ExSelectionListCategory;
      cmdParms [ 18 ].Value = EntityField.Design.DefaultValue;
      cmdParms [ 19 ].Value = EntityField.Design.Unit;

      cmdParms [ 20 ].Value = EntityField.Design.UnitScaling;
      cmdParms [ 21 ].Value = EntityField.Design.ValidationLowerLimit;
      cmdParms [ 22 ].Value = EntityField.Design.ValidationUpperLimit;
      cmdParms [ 23 ].Value = EntityField.Design.AlertLowerLimit;
      cmdParms [ 24 ].Value = EntityField.Design.AlertUpperLimit;
      cmdParms [ 25 ].Value = EntityField.Design.NormalRangeLowerLimit;
      cmdParms [ 26 ].Value = EntityField.Design.NormalRangeUpperLimit;
      cmdParms [ 27 ].Value = EntityField.Design.FieldCategory;
      cmdParms [ 28 ].Value = EntityField.Design.AnalogueLegendStart;
      cmdParms [ 29 ].Value = EntityField.Design.AnalogueLegendFinish;

      cmdParms [ 30 ].Value = EntityField.Design.JavaScript;


      cmdParms [ 31 ].Value = serialisedTableStructure;
      cmdParms [ 32 ].Value = EntityField.Design.InitialOptionList;
      cmdParms [ 33 ].Value = EntityField.Design.InitialVersion;
      cmdParms [ 34 ].Value = EntityField.Design.FieldLayout;
      cmdParms [ 35 ].Value = EntityField.Design.FieldWidth;
      cmdParms [ 36 ].Value = EntityField.Design.FieldHeight;

    }//END setParameters class.
    #endregion

    #region Read FormField data

    // =====================================================================================
    /// <summary>
    /// This class reads the content of the data reader object into FormField business object.
    /// </summary>
    /// <param name="Row">DataRow: an sql data query row</param>
    /// <returns>EvFormField: a form field object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Update formfield object with the compatible data row items. 
    /// 
    /// 2. If the formfield typeId is table, iterate through the formfield table and set the validation rules
    /// 
    /// 3. If the selection validation options are missing, add them.
    /// 
    /// 4. If it is an external coding visitSchedule then add the relevant coding visitSchedule items.
    /// 
    /// 5. Resolve the numeric 'NA' to negative infinity issue.
    /// 
    /// 6. Update the instrument type to current enumeration.
    /// 
    /// 7. If formfield typeId is either analogue scale or horizontal radio buttons, 
    /// select the design by coding value
    /// 
    /// 8. Return the formfield object.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    private EdRecordField getRowData ( DataRow Row )
    {
      // 
      // Initialise xmltable string and a return formfield object. 
      // 
      string xmlTable = String.Empty;
      EdRecordField entityField = new EdRecordField ( );

      //
      // Update formfield object with the compatible data row items. 
      //
      entityField.RecordGuid = Guid.Empty;
      entityField.Guid = EvSqlMethods.getGuid ( Row, EdEntityFields.DB_GUID );
      entityField.FieldGuid = entityField.Guid;
      entityField.LayoutGuid = EvSqlMethods.getGuid ( Row, EdEntityFields.DB_LAYOUT_GUID );
      entityField.LayoutId = EvSqlMethods.getString ( Row, EdEntityFields.DB_LAYOUT_ID );
      entityField.FieldId = EvSqlMethods.getString ( Row, EdEntityFields.DB_FIELD_ID );
      String value = EvSqlMethods.getString ( Row, EdEntityFields.DB_TYPE_ID );
      entityField.Design.TypeId = Evado.Model.EvStatics.parseEnumValue<Evado.Model.EvDataTypes> ( value );

      entityField.Design.Title = EvSqlMethods.getString ( Row, EdEntityFields.DB_TITLE );
      entityField.Design.Instructions = EvSqlMethods.getString ( Row, EdEntityFields.DB_INSTRUCTIONS );
      entityField.Design.HttpReference = EvSqlMethods.getString ( Row, EdEntityFields.DB_HTTP_REFERENCE );
      entityField.Order = EvSqlMethods.getInteger ( Row, EdEntityFields.DB_ORDER );
      entityField.Design.SectionNo = EvSqlMethods.getInteger ( Row, EdEntityFields.DB_SECTION_ID );
      entityField.Design.Options = EvSqlMethods.getString ( Row, EdEntityFields.DB_OPTIONS );
      entityField.Design.IsSummaryField = EvSqlMethods.getBool ( Row, EdEntityFields.DB_SUMMARY_FIELD );
      entityField.Design.Mandatory = EvSqlMethods.getBool ( Row, EdEntityFields.DB_MANDATORY );
      entityField.Design.AiDataPoint = EvSqlMethods.getBool ( Row, EdEntityFields.DB_AI_DATA_POINT );
      entityField.Design.HideField = EvSqlMethods.getBool ( Row, EdEntityFields.DB_HIDDEN );
      entityField.Design.ExSelectionListId = EvSqlMethods.getString ( Row, EdEntityFields.DB_EX_SELECTION_LIST_ID );
      entityField.Design.ExSelectionListCategory = EvSqlMethods.getString ( Row, EdEntityFields.DB_EX_SELECTION_LIST_CATEGORY );
      entityField.Design.DefaultValue = EvSqlMethods.getString ( Row, EdEntityFields.DB_DEFAULT_VALUE );
      entityField.Design.Unit = EvSqlMethods.getString ( Row, EdEntityFields.DB_UNIT );
      entityField.Design.UnitScaling = EvSqlMethods.getString ( Row, EdEntityFields.DB_UNIT_SCALING );

      entityField.Design.ValidationLowerLimit = EvSqlMethods.getFloat ( Row, EdEntityFields.DB_VALIDATION_LOWER_LIMIT );
      entityField.Design.ValidationUpperLimit = EvSqlMethods.getFloat ( Row, EdEntityFields.DB_VALIDATION_UPPER_LIMIT );
      entityField.Design.AlertLowerLimit = EvSqlMethods.getFloat ( Row, EdEntityFields.DB_ALERT_LOWER_LIMIT );
      entityField.Design.AlertUpperLimit = EvSqlMethods.getFloat ( Row, EdEntityFields.DB_ALERT_UPPER_LIMIT );
      entityField.Design.NormalRangeLowerLimit = EvSqlMethods.getFloat ( Row, EdEntityFields.DB_NORMAL_LOWER_LIMITD );
      entityField.Design.NormalRangeUpperLimit = EvSqlMethods.getFloat ( Row, EdEntityFields.DB_NORMAL_UPPER_LIMIT );

      entityField.Design.FieldCategory = EvSqlMethods.getString ( Row, EdEntityFields.DB_FIELD_CATEGORY );
      entityField.Design.AnalogueLegendStart = EvSqlMethods.getString ( Row, EdEntityFields.DB_ANALOGUE_LEGEND_START );
      entityField.Design.AnalogueLegendFinish = EvSqlMethods.getString ( Row, EdEntityFields.DB_ANALOGUE_LEGEND_FINISH );
      entityField.Design.JavaScript = EvSqlMethods.getString ( Row, EdEntityFields.DB_JAVA_SCRIPT );
      xmlTable = EvSqlMethods.getString ( Row, DB_TABLE );
      entityField.Design.InitialOptionList = EvSqlMethods.getString ( Row, EdEntityFields.DB_INITIAL_OPTION_LIST );
      entityField.Design.InitialVersion = EvSqlMethods.getInteger ( Row, EdEntityFields.DB_INITIAL_VERSION );
      entityField.Design.FieldLayout = EvSqlMethods.getString ( Row, EdEntityFields.DB_FIELD_LAYOUT);
      entityField.Design.FieldWidth = EvSqlMethods.getInteger ( Row, EdEntityFields.DB_FIELD_WIDTH );
      entityField.Design.FieldHeight = EvSqlMethods.getInteger ( Row, EdEntityFields.DB_FIELD_HEIGHT );

      if ( entityField.Design.FieldLayout == null )
      {
        entityField.Design.FieldLayout = String.Empty ;
      }

      if ( entityField.Design.FieldWidth <5 )
      {
        entityField.Design.FieldWidth = 50;
      }
      if ( entityField.Design.FieldHeight < 2 )
      {
        entityField.Design.FieldHeight = 5;
      }

      if ( entityField.TypeId == EvDataTypes.External_Image
       || entityField.TypeId == EvDataTypes.Streamed_Video
       || entityField.TypeId == EvDataTypes.Image )
      {
        entityField.RecordMedia = new EdRecordMedia ( );

        entityField.RecordMedia.Data = entityField.Design.JavaScript;
      }

      //
      // if the data type is a table then deseriallise the formfield data table.
      //
      if ( entityField.TypeId == Evado.Model.EvDataTypes.Special_Matrix
        || entityField.TypeId == Evado.Model.EvDataTypes.Table )
      {
        this.LogValue ( "Table Length: " + xmlTable.Length.ToString ( ) );
        //
        // Initialise the table objects.
        //
        entityField.Table = new EdRecordTable ( );

        //
        // Validate whehter the Table has data.
        //
        if ( xmlTable != String.Empty )
        {
          entityField.Table =  Evado.Model.EvStatics.DeserialiseXmlObject<EdRecordTable> ( xmlTable );

          // 
          // Iterate through the table and set the validation rules
          // 
          for ( int i = 0; i < entityField.Table.ColumnCount; i++ )
          {
            //
            // Addressing the 'NA' to negative infinity issue for non-numeric fields.
            //
            // Iterate through the table data converting the relevant cell values to NA.
            //
            for ( int j = 0; j < entityField.Table.Rows.Count; j++ )
            {
              String cell = entityField.Table.Rows [ j ].Column [ i ];

              if ( entityField.Table.Header [ i ].TypeId != EvDataTypes.Numeric )
              {
                entityField.Table.Rows [ j ].Column [ i ] = Evado.Model.EvStatics.convertNumNullToTextNull ( cell );
              }

            }//END column iteration loop

          }//END newField iteration loop

        }// Table has data.

      }//END Table newField.

      //
      // Resolve the numeric 'NA' to negative infinity issue.
      //
      if ( entityField.ItemValue ==  Evado.Digital.Model.EvcStatics.CONST_NUMERIC_NULL.ToString ( )
        && entityField.TypeId != Evado.Model.EvDataTypes.Numeric )
      {
        entityField.ItemValue = "NA";
      }

      //
      // If formfield typeId is either analogue scale or horizontal radio buttons, select the design by coding value
      //

      if ( entityField.Design.ValidationLowerLimit >= entityField.Design.ValidationUpperLimit )
      {
        entityField.Design.ValidationLowerLimit = EvcStatics.CONST_NUMERIC_MINIMUM;
        entityField.Design.ValidationUpperLimit = EvcStatics.CONST_NUMERIC_MAXIMUM;
      }

      if ( entityField.Design.AlertLowerLimit >= entityField.Design.AlertUpperLimit )
      {
        entityField.Design.AlertLowerLimit = EvcStatics.CONST_NUMERIC_MINIMUM;
        entityField.Design.AlertUpperLimit = EvcStatics.CONST_NUMERIC_MAXIMUM;
      }

      if ( entityField.Design.NormalRangeLowerLimit >= entityField.Design.NormalRangeUpperLimit )
      {
        entityField.Design.NormalRangeLowerLimit = EvcStatics.CONST_NUMERIC_MINIMUM;
        entityField.Design.NormalRangeUpperLimit = EvcStatics.CONST_NUMERIC_MAXIMUM;
      }

      // 
      // Return the formfield object.
      // 
      return entityField;

    }//END getRowData method.

    // =====================================================================================
    /// <summary>
    /// This class reads the content of the data reader object into FormField business object.
    /// </summary>
    /// <param name="Row">DataRow: an sql data query row</param>
    /// <returns>EvFormField: a form field object</returns>
    // -------------------------------------------------------------------------------------
    private EdRecordField getChartOptionRowData ( DataRow Row )
    {
      // 
      // Initialise xmltable string and a return formfield object. 
      // 
      string xmlTable = String.Empty;
      EdRecordField formField = new EdRecordField ( );

      //
      // Update formfield object with the compatible data row items. 
      //
      formField.FieldId = EvSqlMethods.getString ( Row, EdEntityFields.DB_FIELD_ID );
      formField.Design.Title = EvSqlMethods.getString ( Row, EdEntityFields.DB_TITLE );
      String value = EvSqlMethods.getString ( Row, EdEntityFields.DB_TYPE_ID );
      formField.Design.TypeId = Evado.Model.EvStatics.parseEnumValue<Evado.Model.EvDataTypes> ( value );
      formField.Design.Options = EvSqlMethods.getString ( Row, EdEntityFields.DB_OPTIONS );

      // 
      // Return the formfield object.
      // 
      return formField;

    }//END getRowData method.
    #endregion

    #region Form Field Queries

    // =====================================================================================
    /// <summary>
    /// This class returns a list of formfield items retrieving by form Guid
    /// </summary>
    /// <param name="FormGuid">Guid: (Mandatory) The form GUID.</param>
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
    public List<EdRecordField> GetFieldList ( Guid FormGuid )
    {
      this.LogMethod ( "GetFieldList" );
      this.LogValue ( "FormGuid: " + FormGuid );
      //
      // Initialize the debug log and a return list of formfield
      //
      List<EdRecordField> view = new List<EdRecordField> ( );

      EdRecordField formField = new EdRecordField ( );
      //    EvFormSections FormSections = new EvFormSections ( );
      // 
      // Define the SQL query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter(EdEntityFields.PARM_LAYOUT_GUID, SqlDbType.UniqueIdentifier),
      };
      cmdParms [ 0 ].Value = FormGuid;

      // 
      // Define the query string.
      // 
      _sqlQueryString = SQL_FIELD_QUERY_VIEW 
        + " WHERE (" + EdEntityFields.DB_LAYOUT_GUID + "="+ EdEntityFields.PARM_LAYOUT_GUID + ") ";
      _sqlQueryString += " ORDER BY "+EdEntityFields.DB_ORDER +";";

      //_Status += "\r\n" + sqlQueryString;

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

          // EvFormField formField = this.getRowData ( row );
          formField = this.getRowData ( row );

          view.Add ( formField );
        }
      }
      this.LogValue ( "Count: " + view.Count.ToString ( ) );


      // 
      // Pass back the result arrray.
      // 
      this.LogMethodEnd ( "GetFieldList" );
      return view;

    }//END GetView method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of options using Guid and OrderBy
    /// </summary>
    /// <param name="LayoutGuid">Guid: a form global unique identifeir</param>
    /// <param name="OrderBy">string: an OrderBy string.</param>
    /// <returns>List of EvOption: a list of options</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define sql query parameters and sql query string. 
    /// 
    /// 2. Execute the sql query string with parameters and store the result on data table 
    /// 
    /// 3. Iterate through the table and extract data row to the option object. 
    /// 
    /// 4. Add option object to the Options list. 
    /// 
    /// 5. Return the Options list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> GetList ( Guid LayoutGuid )
    {
      //
      // Initialize the debug log, a return option list and an option object. 
      //
      this.LogMethod ( "GetList" );
      this.LogValue ( "LayoutGuid: " + LayoutGuid );

      List<EvOption> list = new List<EvOption> ( );
      EvOption option = new EvOption ( );
      list.Add ( option );

      var FieldList = this.GetFieldList ( LayoutGuid );

      // 
      // Execute the query against the database.
      // 
      // 
      // Iterate through the results extracting the role information.
      // 
      foreach ( EdRecordField field in FieldList )
      {

        // 
        // Process the results into the newField.
        // 
        option = new EvOption ( field.Guid,
            field.FieldId + " - " + field.Title );

        if ( option.Description.Length > 80 )
        {
          option.Description = option.Description.Substring ( 0, 80 ) + " ...";
        }

        // 
        // Append the new FormField object to the array.
        // 
        list.Add ( option );

      }//END iteration loop

      // 
      // Pass back the result arrray.
      // 
      return list;

    }//END GetList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of options using VisitId and a FormId
    /// </summary>
    /// <param name="ApplicationId">string: a trial identifeir</param>
    /// <param name="LayoutId">string: a form identifier</param>
    /// <param name="OnlySingleValueFields">bool: select only single value fields.</param>
    /// <returns>List of EvOption: a list of options</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Define the sql quey parameters and the sql query string. 
    /// 
    /// 2. Execute the sql query string with parameters and store the results on datatable. 
    /// 
    /// 3. Iterate through data table and extract data row to the option object 
    /// 
    /// 4. Add the option object to the list of the option object
    /// 
    /// 5. Return the Options list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> GetOptionList (
      String LayoutId,
      bool OnlySingleValueFields )
    {
      //
      // Initialize the debug log, a return list of options and an option object
      //
      this.LogMethod ( "GetOptionList. " );
      this.LogValue ( "LayoutId: " + LayoutId );

      List<EvOption> list = new List<EvOption> ( );
      EvOption option = new EvOption ( );
      list.Add ( option );

      // 
      // Define the SQL query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter( PARM_LAYOUT_ID, SqlDbType.NVarChar,10 ),
      };
      cmdParms [ 0 ].Value = LayoutId;

      // 
      // Define the query string.
      // 
      if ( OnlySingleValueFields == false )
      {
        _sqlQueryString = SQL_FIELD_QUERY_VIEW
          + "WHERE (" + EdEntityFields.DB_DELETED + "= 0 ) "
          + " AND (" + EdEntityFields.DB_LAYOUT_ID + "=" + EdEntityFields.PARM_LAYOUT_ID + ") "
          + " AND (" + EdEntityFields.DB_FIELD_ID + "=" + EdEntityFields.PARM_FIELD_ID + ") "
          + " AND (" + EdEntityFields.DB_LAYOUT_STATE + "'" + EdRecordObjectStates.Form_Issued + "') "
          + " ORDER BY " + EdEntityFields.DB_ORDER + ";";
      }
      else
      {
        _sqlQueryString = SQL_FIELD_QUERY_VIEW
          + "WHERE (" + EdEntityFields.DB_LAYOUT_ID + "=" + EdEntityFields.PARM_LAYOUT_ID + ") "
          + " AND (" + EdEntityFields.DB_FIELD_ID + "=" + EdEntityFields.PARM_FIELD_ID + ") "
          + " AND (" + EdEntityFields.DB_LAYOUT_STATE + "'" + EdRecordObjectStates.Form_Issued + "') "
          + " AND (" + EdEntityFields.DB_TYPE_ID + " <> '" + Evado.Model.EvDataTypes.Check_Box_List + "') "
          + " AND (" + EdEntityFields.DB_TYPE_ID + " <> '" + Evado.Model.EvDataTypes.Table + "') "
          + " AND (" + EdEntityFields.DB_TYPE_ID + " <> '" + Evado.Model.EvDataTypes.Special_Matrix + "') "
          + " AND (TCI_" + EdEntityFields.DB_TYPE_ID + "TypeId <> '" + Evado.Model.EvDataTypes.Free_Text + "') "
          + " ORDER BY " + EdEntityFields.DB_ORDER + ";";
      }

      this.LogValue ( _sqlQueryString );

      // 
      // Execute the query against the database.
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

          // 
          // Process the results into the newField.
          // 
          option = new EvOption (
              EvSqlMethods.getString ( row, EdEntityFields.DB_FIELD_ID ),
              EvSqlMethods.getString ( row, EdEntityFields.DB_FIELD_ID )
              + " - " + EvSqlMethods.getString ( row, EdEntityFields.DB_TITLE ) );

          if ( option.Description.Length > 80 )
          {
            option.Description = option.Description.Substring ( 0, 80 ) + " ...";
          }

          // 
          // Append the new FormField object to the array.
          // 
          list.Add ( option );

        }//END iteration loop

      }//END Using method

      // 
      // Pass back the result arrray.
      // 
      return list;

    }//END GetOptionList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of options using ProjectId and a safety report value.
    /// </summary>
    /// <param name="LayoutId">string: (Mandatory) The trial identifier.</param>
    /// <returns>List of EvOption: a list of options</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. If safetyReport is selected, provide an option for all safety Report items.
    /// 
    /// 2. Define the sql query parameters and sql query string
    /// 
    /// 3. Execute the sql query string with parameters and store the results on datatable. 
    /// 
    /// 4. Iterate through data table and extract data row to the option object 
    /// 
    /// 5. Add the option object to the Options list. 
    /// 
    /// 6. Return the Options list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> getChartOptionList (
      String LayoutId )
    {
      //
      // Initialize the debug log, a return list and an option object. 
      //
      this.LogMethod ( "getChartOptionList " );
      this.LogValue ( "LayoutId: " + LayoutId );

      List<EvOption> list = new List<EvOption> ( );
      EvOption option = new EvOption ( );
      list.Add ( option );

      // 
      // Define the SQL query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
        new SqlParameter( PARM_LAYOUT_ID, SqlDbType.NVarChar,10 ),
      };
      cmdParms [ 0 ].Value = LayoutId;

      // 
      // Define the query string.
      // 
      _sqlQueryString = _sqlDataAnalysisQuery_List
          + "WHERE (" + EdEntityFields.DB_LAYOUT_ID + "=" + EdEntityFields.PARM_LAYOUT_ID + ") "
          + " ORDER BY " + EdEntityFields.DB_ORDER + ";";

      this.LogDebug ( _sqlQueryString );

      // 
      // Execute the query against the database.
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

          //
          // Analytics option 
          var field = this.getChartOptionRowData ( row );

          //
          // if it is a non numeric field the continue to the next field.
          //
          if ( field.Design.hasNumericValues == false )
            {
            this.LogDebug ( "Form: {0}, Field: {1}, type: {2} is a non numeric field",
              field.LayoutId, field.FieldId, field.TypeId );
            continue;
            }
          this.LogDebug ( "Form: {0}, Field: {1}, type: {2} is a numeric field",
              field.LayoutId, field.FieldId, field.TypeId );


          option = new EvOption (
              LayoutId + EvChart.CONST_SOURCE_DELIMITER + field.FieldId,
              LayoutId + " : " + field.FieldId + " - " + field.Title );

          if ( option.Description.Length > 80 )
            {
            option.Description = option.Description.Substring ( 0, 80 ) + " ...";
            }
          // 
          // Append the new FormField object to the array.
          // 
          list.Add ( option );

        }//END iteration loop

      }//END Using method

      // 
      // Pass back the result arrray.
      // 
      return list;

    }//END GetList method.
    #endregion

    #region Form Field Report methods.

    // =====================================================================================
    /// <summary>
    /// This class generates the Report data object. 
    /// </summary>
    /// <param name="Report">EvReport: a report object</param>
    /// <returns>EvReport: a report object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Update the report title, datasource and report date. 
    /// 
    /// 2. If number of columns is less than four, add four new columns and their values
    /// 
    /// 3. Defines the sql query parameters and the sql query string. 
    /// 
    /// 4. Execute the sql query string with parameters and store the results on datatable. 
    /// 
    /// 5. Iterate through the table and extract data row to the formfield object. 
    /// 
    /// 6. Update the formfield object values to the reportRow columns. 
    /// 
    /// 7. Switch formfield typeId and update the third column value
    /// 
    /// 8. If the report data record is empty, add new report row with four columns. 
    /// 
    /// 9. Return the report object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EdReport getReport ( EdReport Report )
    {
      //
      // Initialize the debug log
      //
      this.LogMethod ( "getReport. " );
      this.LogValue ( "Query count: " + Report.Queries.Length );
      this.LogValue ( "Columns count: " + Report.Columns.Count );

      this.LogValue ( "Parameters: " );

      for ( int i = 0; i < Report.Queries.Length; i++ )
      {
        this.LogValue ( Report.Queries [ i ].SelectionSource + " = " + Report.Queries [ i ].Value );
      }

      // 
      // Initialize a list of report rows, a formfield object and the number of report columns
      // 
      List<EdReportRow> reportRows = new List<EdReportRow> ( );
      EdRecordField formField = new EdRecordField ( );
      int inNoColumns = Report.Columns.Count;

      //
      // Update the report title, datasource and report date. 
      //
      Report.ReportTitle = "Form Field Properties";
      Report.DataSourceId = EdReport.ReportSourceCode.FormFields;
      Report.ReportDate = DateTime.Now;
      //
      // Define the report columns
      //
      Report.Columns = new List<EdReportColumn> ( );

      // 
      // Set the trial column 1
      // 
      EdReportColumn column = new EdReportColumn ( );
      column.HeaderText = "FieldId";
      column.SectionLvl = 0;
      column.GroupingIndex = false;
      column.DataType = EdReport.DataTypes.Text;
      column.SourceField = "FieldId";
      column.GroupingType = EdReport.GroupingTypes.None;
      column.StyleWidth = "100px";

      Report.Columns.Add ( column );

      // 
      // Set the trial column 2
      // 
      column = new EdReportColumn ( );
      column.HeaderText = "Title";
      column.SectionLvl = 0;
      column.GroupingIndex = false;
      column.DataType = EdReport.DataTypes.Text;
      column.SourceField = "Subject";
      column.GroupingType = EdReport.GroupingTypes.None;
      column.StyleWidth = "200px";

      Report.Columns.Add ( column );

      // 
      // Set the trial column 3
      // 
      column = new EdReportColumn ( );
      column.HeaderText = "Type";
      column.SectionLvl = 0;
      column.GroupingIndex = false;
      column.DataType = EdReport.DataTypes.Text;
      column.SourceField = "Type";
      column.GroupingType = EdReport.GroupingTypes.None;
      column.StyleWidth = "100px";

      Report.Columns.Add ( column );

      // 
      // Set the trial column 4
      // 
      column = new EdReportColumn ( );
      column.HeaderText = "Parameters";
      column.SectionLvl = 0;
      column.GroupingIndex = false;
      column.DataType = EdReport.DataTypes.Text;
      column.SourceField = "Type";
      column.GroupingType = EdReport.GroupingTypes.None;
      column.StyleWidth = "400px";

      Report.Columns.Add ( column );

      // 
      // Set the trial column 5
      // 
      column = new EdReportColumn ( );
      column.HeaderText = "Hidden Field";
      column.SectionLvl = 0;
      column.GroupingIndex = false;
      column.DataType = EdReport.DataTypes.Bool;
      column.SourceField = "Hidden";
      column.GroupingType = EdReport.GroupingTypes.None;
      column.StyleWidth = "50px";

      Report.Columns.Add ( column );

      this.LogValue ( "Report.Column.count: " + Report.Columns.Count );

      foreach ( EdReportColumn col in Report.Columns )
      {
        this.LogValue ( "HeaderText: " + col.HeaderText );
      }

      // 
      // Define the query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ]
      {
       new SqlParameter( PARM_FIELD_ID, SqlDbType.NVarChar,20 ),
      };
          cmdParms [ 0 ].Value = string.Empty;

      //
      // Extract the parameters from the parameter list.
      //
      for ( int i = 0; i < Report.Queries.Length; i++ )
      {
        if ( Report.Queries [ i ].SelectionSource == EdReport.SelectionListTypes.LayoutId )
        {
          cmdParms [ 0 ].Value = Report.Queries [ i ].Value;
        }
        if ( Report.Queries [ i ].SelectionSource == EdReport.SelectionListTypes.Form_Field_Id )
        {
          cmdParms [ 1 ].Value = Report.Queries [ i ].Value;
        }
        if ( Report.Queries [ i ].SelectionSource == EdReport.SelectionListTypes.Form_Field_Id)
        {
          cmdParms [ 2 ].Value = Report.Queries [ i ].Value;
        }
      }//END parameter iteration loop.

      //
      // Generate the SQL query string.
      //
      _sqlQueryString = SQL_FIELD_QUERY_VIEW
          + "WHERE (" + EdEntityFields.DB_LAYOUT_ID + "=" + EdEntityFields.PARM_LAYOUT_ID + ") "
          + " AND (" + EdEntityFields.DB_FIELD_ID + "=" + EdEntityFields.PARM_FIELD_ID + ") "
          + " AND (" + EdEntityFields.DB_LAYOUT_STATE + "'" + EdRecordObjectStates.Form_Issued + "') "
          + " ORDER BY " + EdEntityFields.DB_ORDER + ";";

      this.LogValue ( "SQL QUERY: " + _sqlQueryString );

      //
      // Execute the query against the database.
      //
      using ( DataTable table = EvSqlMethods.RunQuery ( _sqlQueryString, cmdParms ) )
      {
        this.LogValue ( "Returned Records: " + table.Rows.Count );

        // 
        // Iterate through the results extracting the role information.
        // 
        for ( int count = 0; count < table.Rows.Count; count++ )
        {
          // 
          // Extract the table row to formfield object. 
          // 
          DataRow row = table.Rows [ count ];

          formField = this.getRowData ( row );

          this.LogValue ( "Field: " + formField.FieldId
            + " - " + formField.Title
            + " Type: " + formField.TypeId );

          EdReportRow reportRow = new EdReportRow ( Report.Columns.Count );

          //
          // Update the formfield values to the reportRow column
          //
          reportRow.ColumnValues [ 0 ] = formField.FieldId;
          reportRow.ColumnValues [ 1 ] = formField.Title;
          reportRow.ColumnValues [ 2 ] = EvStatics.getEnumStringValue ( formField.TypeId );

          //
          // Switch formfield typeId and update the third column value
          //
          switch ( formField.TypeId )
          {
            case Evado.Model.EvDataTypes.Check_Box_List:
            case Evado.Model.EvDataTypes.Radio_Button_List:
            case Evado.Model.EvDataTypes.Selection_List:
            case Evado.Model.EvDataTypes.Horizontal_Radio_Buttons:
              {
                reportRow.ColumnValues [ 3 ] = "Options: " + formField.Design.Options;
                break;
              }
            case Evado.Model.EvDataTypes.Numeric:
              {
                reportRow.ColumnValues [ 3 ] = "Val. Min: " + formField.Design.ValidationLowerLimit
                  + " Val. Max: " + formField.Design.ValidationUpperLimit
                  + " Alert. Min: " + formField.Design.AlertLowerLimit
                  + " Alert. Max: " + formField.Design.AlertUpperLimit;
                if ( formField.Design.Unit != String.Empty )
                {
                  reportRow.ColumnValues [ 3 ] += " Unit: " + formField.Design.UnitHtml;
                }
                break;
              }
            case Evado.Model.EvDataTypes.Table:
            case Evado.Model.EvDataTypes.Special_Matrix:
              {
                reportRow.ColumnValues [ 3 ] = String.Empty;
                foreach ( EdRecordTableHeader header in formField.Table.Header )
                {
                  reportRow.ColumnValues [ 3 ] += "Col: " + header.No
                    + " Title: " + header.Text
                    + " Type: " + header.TypeId + " ";
                }
                break;
              }
          }
          reportRow.ColumnValues [ 4 ] = formField.Design.HideField.ToString ( );

          //
          // Add the reportRow to the report data records. 
          //
          Report.DataRecords.Add ( reportRow );

        }//END record interation loop.

      }//END using statement.

      this.LogValue ( "Report row count: " + reportRows.Count );

      //
      // If the report data record is empty, add new report row with four columns. 
      //
      if ( Report.DataRecords.Count == 0 )
      {
        EdReportRow reportRow = new EdReportRow ( Report.Columns.Count );

        for ( int i = 0; i < Report.Columns.Count; i++ )
        {
          reportRow.ColumnValues [ i ] = String.Empty;
        }
        Report.DataRecords.Add ( reportRow );

      }//END ADD EMPTY ROW

      // 
      // Return the result array.
      // 
      return Report;
    }

    #endregion

    #region Retrieval Queries

    // ==================================================================================
    /// <summary>
    /// This class retrieves formfield values using field Guid
    /// </summary>
    /// <param name="FieldGuid">Guid: (Mandatory) a field global unique object identifier.</param>
    /// <returns>EvFormField: a FormField data object</returns>
    /// <remarks>
    /// This class consists of the following steps: 
    /// 
    /// 1. Return an empty formfield object if the formfield's Guid is empty 
    /// 
    /// 2. Define the sql query parameters and the sql query string. 
    /// 
    /// 3. Execute the sql query string with parameters and store the results on the datatable. 
    /// 
    /// 4. Extract the first datarow to the formfield object. 
    /// 
    /// 5. Return the formfield object.
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public EdRecordField getField ( Guid FieldGuid )
    {
      //
      // Initialize the debug log and a return formfield object.
      //
      this.LogMethod ( "getField" );
      this.LogValue ( "FieldGuid: " + FieldGuid );

      EdRecordField formField = new EdRecordField ( );

      // 
      // Validate whether the field Guid is not empty.
      // 
      if ( FieldGuid == Guid.Empty )
      {
        return formField;
      }

      // 
      // Define the query parameters.
      // 
      SqlParameter cmdParms = new SqlParameter ( EdEntityFields.PARM_GUID, SqlDbType.UniqueIdentifier );
      cmdParms.Value = FieldGuid;

      // 
      // Define the query string.
      // 
      _sqlQueryString = SQL_FIELD_QUERY_VIEW + " WHERE (" + EdEntityFields.DB_GUID + " = " + EdEntityFields.PARM_GUID + "); ";

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
          return formField;
        }

        // 
        // Extract the table row
        // 
        DataRow row = table.Rows [ 0 ];

        // 
        // Fill the role object.
        // 
        formField = this.getRowData ( row );

      }//END Using method

      // 
      // Pass back the data object.
      // 
      return formField;

    }// END getField method

    #endregion

    #region FormFields Update queries

    // ==================================================================================
    /// <summary>
    /// This class update items on formfield table using retrieving formfield items values. 
    /// </summary>
    /// <param name="FormField">EvFormField: a formfield object</param>
    /// <returns>EvEventCodes: an event code for updating items on formfield object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the Old formfield's Uid is not defined. 
    /// 
    /// 2. Add items to datachange object if they do not exist. 
    /// 
    /// 3. Define sql query parameters and execute the storeprocedure for updating items on formfield table. 
    /// 
    /// 4. Add datachange object values to the backup datachanges object. 
    /// 
    /// 5. Return the event code for updating items. 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public EvEventCodes UpdateFields ( EdRecord Layout )
    {
      this.LogMethod ( "UpdateFields method. " );
      this.LogValue ( "Layout.Fields.Count: " + Layout.Fields.Count );
      //
      // Initialize debug log and the old formfield object
      //

      foreach ( EdRecordField field in Layout.Fields )
      {
        field.LayoutGuid = Layout.Guid;

        var result = UpdateItem ( field );
        if ( result != EvEventCodes.Ok )
        {
          return result;
        }
      }

      this.LogMethod ( "UpdateFields" );
      return EvEventCodes.Ok;

    }//END UpdateFields method

    // ==================================================================================
    /// <summary>
    /// This class update items on formfield table using retrieving formfield items values. 
    /// </summary>
    /// <param name="FormField">EvFormField: a formfield object</param>
    /// <returns>EvEventCodes: an event code for updating items on formfield object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the Old formfield's Uid is not defined. 
    /// 
    /// 2. Add items to datachange object if they do not exist. 
    /// 
    /// 3. Define sql query parameters and execute the storeprocedure for updating items on formfield table. 
    /// 
    /// 4. Add datachange object values to the backup datachanges object. 
    /// 
    /// 5. Return the event code for updating items. 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public EvEventCodes UpdateItem ( EdRecordField FormField )
    {
      this.LogMethod ( "updateItem method. " );
      this.LogValue ( "FieldId: " + FormField.FieldId );
      //
      // Initialize debug log and the old formfield object
      //
      EvDataChanges dataChanges = new EvDataChanges ( );

      //
      // retrieve the previous version of the field.
      //
      EdRecordField oldItem = this.getField ( FormField.Guid );
      if ( oldItem.Guid == Guid.Empty )
      {
        this.LogValue ( "FormId not returned." );

        return EvEventCodes.Identifier_Form_Id_Error;
      }

      //
      // Create the data change object.
      //
      EvDataChange dataChange = this.SetDataChange ( oldItem, FormField );

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] _cmdParms = GetParameters ( );
      setParameters ( _cmdParms, FormField );

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( STORED_PROCEDURE_UPDATE_ITEM, _cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }
      // 
      // Add the change record
      // 
      dataChanges.AddItem ( dataChange );

      return EvEventCodes.Ok;

    }//END UpdateItem method

    // =====================================================================================
    /// <summary>
    ///  This method sets the data change values for the object and returns the data change
    ///  object.
    /// </summary>
    /// <param name="OldField">EvFormField: A old field object.</param>
    /// <param name="NewField">EvFormField: A new field object.</param>
    /// <returns>EvDataChange: A DataChange object.</returns>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Add items to datachange object if they do not exist on the old Subject Record object. 
    /// 
    /// 2. Return the datachange object. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private EvDataChange SetDataChange ( EdRecordField OldField, EdRecordField NewField )
    {
      this.LogMethod ( "SetDataChange method. " );
      // 
      // Initialise the methods variables and objects.
      // 
      EvDataChange dataChange = new EvDataChange ( );


      // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      // Compare the objects.
      // Initialize datachange object and a backup datachange object. 
      //
      dataChange.Guid = Guid.NewGuid ( );
      dataChange.TableName = EvDataChange.DataChangeTableNames.EvFormFields;
      //dataChange.RecordUid = 1;
      dataChange.RecordGuid = NewField.Guid;
      dataChange.UserId = this.ClassParameters.UserProfile.UserId;
      dataChange.DateStamp = DateTime.Now;

      //
      // Add items to the datachange object if they do not exist in the old form field object. 
      //
      dataChange.AddItem ( "ItemId", OldField.FieldId, NewField.FieldId );

      dataChange.AddItem ( "TypeId", OldField.TypeId.ToString ( ), NewField.TypeId.ToString ( ) );

      dataChange.AddItem ( "Subject", OldField.Design.Title, NewField.Design.Title );

      dataChange.AddItem ( "Instructions", OldField.Design.Instructions, NewField.Design.Instructions );

      dataChange.AddItem ( "Reference", OldField.Design.HttpReference, NewField.Design.HttpReference );

      dataChange.AddItem ( "Options", OldField.Design.Options, NewField.Design.Options );

      dataChange.AddItem ( "Unit", OldField.Design.Unit, NewField.Design.Unit );


      dataChange.AddItem ( "Design_JavaScript",
        OldField.Design.JavaScript,
        NewField.Design.JavaScript );





      dataChange.AddItem ( "Validation_AlertLowerLimit",
        OldField.Design.AlertLowerLimit,
        NewField.Design.AlertLowerLimit );

      dataChange.AddItem ( "Validation_AlertUpperLimit",
        OldField.Design.AlertUpperLimit,
        NewField.Design.AlertUpperLimit );

      dataChange.AddItem ( "Validation_NormaRangeLowerLimit",
        OldField.Design.NormalRangeLowerLimit,
        NewField.Design.NormalRangeLowerLimit );

      dataChange.AddItem ( "Validation_NormaRangeUpperLimit",
        OldField.Design.NormalRangeUpperLimit,
        NewField.Design.NormalRangeUpperLimit );

      dataChange.AddItem ( "Validation_ValidationLowerLimit",
        OldField.Design.ValidationLowerLimit,
        NewField.Design.ValidationLowerLimit );

      dataChange.AddItem ( "Validation_SafetyUpperLimit",
        OldField.Design.ValidationUpperLimit,
        NewField.Design.ValidationUpperLimit );

      if ( OldField.Table != null
        && NewField.Table != null )
      {
        for ( int index = 0; index < NewField.Table.Header.Length; index++ )
        {
          EdRecordTableHeader oldHeader = OldField.Table.Header [ index ];
          EdRecordTableHeader newHeader = NewField.Table.Header [ index ];

          dataChange.AddItem ( "Field_Table_Header_No_" + index, oldHeader.No, newHeader.No );

          dataChange.AddItem ( "Field_Table_Header_Text_" + index, oldHeader.Text, newHeader.Text );

          dataChange.AddItem ( "Field_Table_Header_TypeId_" + index, oldHeader.TypeId, newHeader.TypeId );

          dataChange.AddItem ( "Field_Table_Header_Width_" + index, oldHeader.Width, newHeader.Width );

          dataChange.AddItem ( "Field_Table_Header_ColumnId_" + index, oldHeader.ColumnId, newHeader.ColumnId );

          dataChange.AddItem ( "Field_Table_Header_OptionsOrUnit_" + index, oldHeader.OptionsOrUnit, newHeader.OptionsOrUnit );
        }

        if ( NewField.TypeId == Evado.Model.EvDataTypes.Special_Matrix
          && NewField.Table.Rows.Count > 0 )
        {
          for ( int count = 0; count < NewField.Table.Rows.Count; count++ )
          {
            if ( OldField.Table.Rows.Count <= count )
            {
              for ( int index = 0; index < NewField.Table.Header.Length; index++ )
              {
                dataChange.AddItem (
                  "Field_Matrix_R" + count + "C" + index,
                  "",
                  NewField.Table.Rows [ count ].Column [ index ] );
              }
            }
            else
            {
              for ( int index = 0; index < NewField.Table.Header.Length; index++ )
              {
                dataChange.AddItem (
                  "Field_Matrix_R" + count + "C" + index,
                  OldField.Table.Rows [ count ].Column [ index ],
                  NewField.Table.Rows [ count ].Column [ index ] );
              }
            }
          }
        }

      }//END Table difference block

      if ( NewField.Design != null
        && OldField.Design != null )
      {
        dataChange.AddItem (
          "Field_Design_AnalogueLegendFinish",
          OldField.Design.AnalogueLegendFinish,
          NewField.Design.AnalogueLegendFinish );

        dataChange.AddItem (
          "Field_Design_AnalogueLegendStart",
          OldField.Design.AnalogueLegendStart,
          NewField.Design.AnalogueLegendStart );

        dataChange.AddItem (
          "Field_Design_DataPoint",
          OldField.Design.AiDataPoint,
          NewField.Design.AiDataPoint );

        dataChange.AddItem (
          "Field_Design_DefaultValue",
          OldField.Design.DefaultValue,
          NewField.Design.DefaultValue );

        dataChange.AddItem (
          "Field_Design_DictionaryGuid",
          OldField.Design.DictionaryGuid,
          NewField.Design.DictionaryGuid );

        dataChange.AddItem (
          "Field_Design_ExSelectionListCategory",
          OldField.Design.ExSelectionListCategory,
          NewField.Design.ExSelectionListCategory );

        dataChange.AddItem (
          "Field_Design_ExSelectionListId",
          OldField.Design.ExSelectionListId,
          NewField.Design.ExSelectionListId );

        dataChange.AddItem (
          "Field_Design_FieldCategory",
          OldField.Design.FieldCategory,
          NewField.Design.FieldCategory );


        dataChange.AddItem (
          "Field_Design_HideField",
          OldField.Design.HideField,
          NewField.Design.HideField );


        dataChange.AddItem (
          "Field_Design_InitialOptionList",
          OldField.Design.InitialOptionList,
          NewField.Design.InitialOptionList );

        dataChange.AddItem (
          "Field_Design_Instructions",
          OldField.Design.Instructions,
          NewField.Design.Instructions );

        dataChange.AddItem (
          "Field_Design_Mandatory",
          OldField.Design.Mandatory,
          NewField.Design.Mandatory );

        dataChange.AddItem (
          "Field_Design_Options",
          OldField.Design.Options,
          NewField.Design.Options );

        dataChange.AddItem (
          "Field_Design_Order",
          OldField.Design.Order,
          NewField.Design.Order );

        dataChange.AddItem (
          "Field_Design_Reference",
          OldField.Design.HttpReference,
          NewField.Design.HttpReference );


        dataChange.AddItem (
          "Field_Design_Section",
          OldField.Design.SectionNo,
          NewField.Design.SectionNo );

        dataChange.AddItem (
          "Field_Design_Subject,",
          OldField.Design.Title,
          NewField.Design.Title );

        dataChange.AddItem (
          "Field_Design_SummaryField",
          OldField.Design.IsSummaryField,
          NewField.Design.IsSummaryField );

        dataChange.AddItem (
          "Field_Design_Unit",
          OldField.Design.Unit,
          NewField.Design.Unit );

        dataChange.AddItem (
          "Field_Design_UnitScaling",
          OldField.Design.UnitScaling,
          NewField.Design.UnitScaling );

      }//END field design difference block.

      return dataChange;

    }//END SetDataChange method

    // ==================================================================================
    /// <summary>
    /// This class adds items to formfield table.
    /// </summary>
    /// <param name="FormField">EvFormField: a formfield object</param>
    /// <returns>EvEventCodes: an event code for adding items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the FormUid or Uid is duplicated. 
    /// 
    /// 3. Define the sql query parameters and execute the storeprocedure for adding items. 
    /// 
    /// 4. Return the event code for adding items.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes AddItem ( EdRecordField FormField )
    {
      this.LogMethod ( "AddItem method. " );
      this.LogValue ( "FormGuid: " + FormField.LayoutGuid );
      this.LogValue ( "FieldId: " + FormField.FieldId );
      //
      // Initialize the debug log, a local sql query string and a new formfield object. 
      //
      FormField.Guid = Guid.NewGuid ( );

      //
      // Test for new field with the same id.
      //
      EdRecordField newField = this.GetNewField ( FormField );

      //
      // Validate whether the formUid and Uid do not exist. 
      //
      if ( newField.Guid != Guid.Empty )
      {
        return EvEventCodes.Data_Duplicate_Field_Error;
      }//END  FormUid

      // 
      // Define the SQL query parameters and load the query values.
      // 
      SqlParameter [ ] _cmdParms = GetParameters ( );
      setParameters ( _cmdParms, FormField );

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( STORED_PROCEDURE_ADD_ITEM, _cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      this.LogMethodEnd ( "AddItem" );
      return EvEventCodes.Ok;

    }//END AddItem class. 

    // =====================================================================================
    /// <summary>
    /// This method returns the selected Field with the field identifier.
    /// </summary>
    /// <param name="Field">EvFormField: a formfield object</param>
    /// <returns>EvEventCodes: an event code for deleting items from formfield table</returns>
    // -------------------------------------------------------------------------------------
    private EdRecordField GetNewField ( EdRecordField FormField )
    {
      this.LogMethod ( "validateFieldId method. " );
      this.LogValue ( "FormGuid: " + FormField.LayoutGuid );
      this.LogValue ( "FieldId: " + FormField.FieldId );
      //
      // Initialize the debug log, a local sql query string and a new formfield object. 
      //

      var fieldList = this.GetFieldList( FormField.LayoutGuid );

      foreach ( EdRecordField field in fieldList )
      {
        if ( field.FieldId == FormField.FieldId )
        {
          return field;
        }
      }

      this.LogMethodEnd ( "validateFieldId" );

      return new EdRecordField ( ); ;
    }

    // =====================================================================================
    /// <summary>
    /// This class deletes items from formfield table
    /// </summary>
    /// <param name="Field">EvFormField: a formfield object</param>
    /// <returns>EvEventCodes: an event code for deleting items from formfield table</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the VisitId or User common name is not defined 
    /// 
    /// 2. Define the sql query parameters and execute the storeprocedure for deleting items. 
    /// 
    /// 3. Return the event code for deleting the items from formfield table.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes DeleteItem ( EdRecordField Field )
    {
      //
      // Initialize the debug log
      //
      this.LogMethod ( "DeleteItem method." );
      this.LogValue ( "FieldId: " + Field.FieldId );

      // 
      // Validate whether the VisitId and Usercommon name exist. 
      // 
      if ( Field.Guid == Guid.Empty )
      {
        return EvEventCodes.Identifier_Global_Unique_Identifier_Error;
      }

      // 
      // Define the query parameters.
      // 
      SqlParameter [ ] _cmdParms = new SqlParameter [ ]
      {
        new SqlParameter(PARM_GUID, SqlDbType.UniqueIdentifier),
      };
      _cmdParms [ 0 ].Value = Field.Guid;

      //
      // Execute the update command.
      //
      if ( EvSqlMethods.StoreProcUpdate ( STORED_PROCEDURE_DELTE_ITEM, _cmdParms ) == 0 )
      {
        return EvEventCodes.Database_Record_Update_Error;
      }

      return EvEventCodes.Ok;

    }//END DeleteItem method
    #endregion

  }//END EvFormFields class

}//END namespace Evado.Digital.Dal
