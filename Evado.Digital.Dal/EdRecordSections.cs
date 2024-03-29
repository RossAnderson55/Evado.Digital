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
  /// This class is handles the data access layer for the form section data object.
  /// </summary>
  public class EdRecordSections : EvDalBase
  {
    #region class initialisation method.
    /// <summary>
    /// This method initialises the schedule DAL class.
    /// </summary>
    public EdRecordSections ( )
    {
      this.ClassNameSpace = "Evado.Digital.Dal.Digital.EdRecordSections.";
    }

    /// <summary>
    /// This method initialises the schedule DAL class.
    /// </summary>
    public EdRecordSections ( EvClassParameters Settings )
    {
      this.ClassParameters = Settings;
      this.ClassNameSpace = "Evado.Digital.Dal.Digital.EdRecordSections.";

      if ( this.ClassParameters.LoggingLevel == 0 )
      {
        this.ClassParameters.LoggingLevel = Evado.Digital.Dal.EvStaticSetting.LoggingLevel;
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
    private const string _sqlQueryView = "Select * FROM ED_RECORD_SECTIONS ";

    private const string DB_LAYOUT_GUID = "EDRL_Guid";
    private const string DB_NUMBER = "EDRLS_NUMBER";
    private const string DB_ORDER = "EDRLS_ORDER";
    private const string DB_NAME = "EDRLS_NAME";
    private const string DB_INSTRUCTIONS = "EDRLS_INSTRUCTIONS";
    private const string DB_FIELD_NAME = "EDRLS_FIELD_NAME";
    private const string DB_FIELD_VALUE = "EDRLS_FIELD_VALUE";
    private const string DB_ON_MATCH_VISIBLE = "EDRLS_ON_MATCH_VISIBLE";
    private const string DB_VISIBLE = "EDRLS_VISIBLE";
    private const string DB_DEFAULT_DISPLAY_ROLES = "EDRLS_DEFAULT_DISPLAY_ROLES";
    private const string DB_DEFAULT_EDIT_ROLES = "EDRLS_DEFAULT_EDIT_ROLES";
    private const string DB_PERCENT_WIDTH = "EDRLS_PERCENT_WIDTH";


    //
    //  Define the SQL query string variable.
    //  
    private string _sqlQueryString = String.Empty;


    /// <summary>
    /// This constant defines the parameter for form global unique identifier of the formfield object
    /// </summary>
    private const string PARM_FORM_GUID = "@LAYOUT_GUID";
    private const string PARM_FORM_SECTION_GUID = "@GUID";
    private const string PARM_NUMBER = "@NUMBER";
    private const string PARM_ORDER = "@ORDER";
    private const string PARM_NAME = "@NAME";
    private const string PARM_INSTRUCTIONS = "@INSTRUCTIONS";
    private const string PARM_FIELD_NAME = "@FIELD_NAME";
    private const string PARM_FIELD_VALUE = "@FIELD_VALUE";
    private const string PARM_ON_MATCH_VISIBLE = "@ON_MATCH_VISIBLE";
    private const string PARM_VISIBLE = "@VISIBLE";
    private const string PARM_DEFAULT_DISPLAY_ROLES = "@DEFAULT_DISPLAY_ROLES";
    private const string PARM_DEFAULT_EDIT_ROLES = "@DEFAULT_EDIT_ROLES";
    private const string PARM_PERCENT_WIDTH = "@PERCENT_WIDTH";
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
    private  Evado.Digital.Model.EdRecordSection getRowData ( DataRow Row )
    {
      // 
      // Initialise xmltable string and a return formfield object. 
      // 
      string xmlTable = String.Empty;
       Evado.Digital.Model.EdRecordSection formSection = new  Evado.Digital.Model.EdRecordSection ( );

      //
      // Update formfield object with the compatible data row items. 
      //
      formSection.LayoutGuid = EvSqlMethods.getGuid ( Row, EdRecordSections.DB_LAYOUT_GUID );
      formSection.No = EvSqlMethods.getInteger ( Row, EdRecordSections.DB_NUMBER );;
      formSection.Title = EvSqlMethods.getString ( Row, EdRecordSections.DB_NAME );
      formSection.Order = EvSqlMethods.getInteger ( Row, EdRecordSections.DB_ORDER );
      formSection.FieldId = EvSqlMethods.getString ( Row, EdRecordSections.DB_FIELD_NAME );
      formSection.Instructions = EvSqlMethods.getString ( Row, EdRecordSections.DB_INSTRUCTIONS );
      formSection.FieldValue = EvSqlMethods.getString ( Row, EdRecordSections.DB_FIELD_VALUE );
      formSection.OnMatchVisible = EvSqlMethods.getBool ( Row, EdRecordSections.DB_ON_MATCH_VISIBLE );
      formSection.OnOpenVisible = EvSqlMethods.getBool ( Row, EdRecordSections.DB_VISIBLE );
      formSection.ReadAccessRoles = EvSqlMethods.getString ( Row, EdRecordSections.DB_DEFAULT_DISPLAY_ROLES );
      formSection.EditAccessRoles = EvSqlMethods.getString ( Row, EdRecordSections.DB_DEFAULT_EDIT_ROLES );
      formSection.PercentWidth = EvSqlMethods.getInteger ( Row, EdRecordSections.DB_PERCENT_WIDTH);

      return formSection;

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
    public List<EdRecordSection> getSectionList ( Guid FormGuid )
    {
      this.LogMethod ( "GetView method" );
      this.LogDebug ( "FormGuid: " + FormGuid );
      //
      // Initialize the debug log and a return list of formfield
      //
      List<EdRecordSection> sectionList = new List<EdRecordSection> ( );

      // 
      // Define the SQL query parameters.
      // 
      SqlParameter [ ] cmdParms = new SqlParameter [ ] 
      {
        new SqlParameter(PARM_FORM_GUID, SqlDbType.UniqueIdentifier),
      };
      cmdParms [ 0 ].Value = FormGuid;

      // 
      // Define the query string.
      // 
      _sqlQueryString = _sqlQueryView + " WHERE ( " + DB_LAYOUT_GUID + "  = " + PARM_FORM_GUID + ") "
        + "ORDER BY " + DB_ORDER + "; ";


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

          EdRecordSection section = this.getRowData ( row );

          section.Order = count * 2 + 1;

          sectionList.Add ( section );
        }
      }
      this.LogDebug ( "Count: " + sectionList.Count.ToString ( ) );

      // 
      // Pass back the result arrray.
      // 
      return sectionList;

    }//END GetView method.

    #endregion

    #region FormFields Update queries

    // ==================================================================================
    /// <summary>
    /// This class update items on EV_FORM_SECTION table using retrieving form section items values. 
    /// </summary>
    /// <param name="Form">EvForm: a form section object</param>
    /// <returns>EvEventCodes: an event code for updating items on formfield object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Dekete the data for the old Guid. 
    /// 
    /// 2. Insert the modified data for the new Guid.
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public EvEventCodes UpdateItem ( EdRecord Form )
    {
      this.LogMethod ( "updateItem method. " );
      this.LogDebug ( "Section Count: " + Form.Design.FormSections.Count );

      //
      // Initialize the debug status and the local variables
      //
      StringBuilder sbSQL_AddQuery = new StringBuilder ( );
      List<SqlParameter> parmList = new List<SqlParameter> ( );

      if ( Form.Guid == Guid.Empty )
      {
        return EvEventCodes.Identifier_Global_Unique_Identifier_Error;
      }

      //
      // if sections doe not exist exit.
      //
      if ( Form.Design.FormSections.Count == 0 )
      {
        return EvEventCodes.Ok;
      }

      //
      // Delete the sections
      //
      sbSQL_AddQuery.AppendLine ( "DELETE FROM ED_RECORD_SECTIONS "
      + "WHERE " + EdRecordSections.DB_LAYOUT_GUID + " = " + EdRecordSections.PARM_FORM_GUID + ";  \r\n" );

      SqlParameter prm = new SqlParameter ( EdRecordSections.PARM_FORM_GUID, SqlDbType.UniqueIdentifier );
      prm.Value = Form.Guid;
      parmList.Add ( prm );

      for ( int count = 0; count < Form.Design.FormSections.Count; count++ )
      {
        EdRecordSection section = Form.Design.FormSections [ count ];

        //
        // skip empty sections 
        //
        if ( section.Title == String.Empty )
        {
          continue;
        }

        //
        // Set the order if 0
        //
        if ( section.Order == 0 )
        {
          section.Order = count + 3;
        }

        //
        // define the section parameters.
        //
        prm = new SqlParameter ( EdRecordSections.PARM_NUMBER + "_" + count, SqlDbType.Int );
        prm.Value = section.No;
        parmList.Add ( prm );

        prm = new SqlParameter ( EdRecordSections.PARM_ORDER + "_" + count, SqlDbType.Int );
        prm.Value = section.Order;
        parmList.Add ( prm );

        prm = new SqlParameter ( EdRecordSections.PARM_NAME + "_" + count, SqlDbType.VarChar, 30 );
        prm.Value = section.Title;
        parmList.Add ( prm );

        prm = new SqlParameter ( EdRecordSections.PARM_INSTRUCTIONS + "_" + count, SqlDbType.NText );
        prm.Value = section.Instructions;
        parmList.Add ( prm );

        prm = new SqlParameter ( EdRecordSections.PARM_FIELD_NAME + "_" + count, SqlDbType.NVarChar, 20 );
        prm.Value = section.FieldId;
        parmList.Add ( prm );

        prm = new SqlParameter ( EdRecordSections.PARM_FIELD_VALUE + "_" + count, SqlDbType.NVarChar, 50 );
        prm.Value = section.FieldValue;
        parmList.Add ( prm );

        prm = new SqlParameter ( EdRecordSections.PARM_ON_MATCH_VISIBLE + "_" + count, SqlDbType.Bit );
        prm.Value = section.OnMatchVisible;
        parmList.Add ( prm );

        prm = new SqlParameter ( EdRecordSections.PARM_VISIBLE + "_" + count, SqlDbType.Bit );
        prm.Value = section.OnOpenVisible;
        parmList.Add ( prm );

        prm = new SqlParameter ( EdRecordSections.PARM_DEFAULT_DISPLAY_ROLES + "_" + count, SqlDbType.NVarChar, 250 );
        prm.Value = section.ReadAccessRoles;
        parmList.Add ( prm );

        prm = new SqlParameter ( EdRecordSections.PARM_DEFAULT_EDIT_ROLES + "_" + count, SqlDbType.NVarChar, 250 );
        prm.Value = section.EditAccessRoles;
        parmList.Add ( prm );

        prm = new SqlParameter ( EdRecordSections.PARM_PERCENT_WIDTH + "_" + count, SqlDbType.Int );
        prm.Value = section.EditAccessRoles;
        parmList.Add ( prm );

        //
        // Create the add query .
        //
        sbSQL_AddQuery.AppendLine ( " INSERT INTO ED_RECORD_SECTIONS  "
        + "(" + EdRecordSections.DB_LAYOUT_GUID
        + ", " + EdRecordSections.DB_NUMBER
        + ", " + EdRecordSections.DB_ORDER
        + ", " + EdRecordSections.DB_NAME
        + ", " + EdRecordSections.DB_INSTRUCTIONS
        + ", " + EdRecordSections.DB_FIELD_NAME
        + ", " + EdRecordSections.DB_FIELD_VALUE
        + ", " + EdRecordSections.DB_ON_MATCH_VISIBLE
        + ", " + EdRecordSections.DB_VISIBLE
        + ", " + EdRecordSections.DB_DEFAULT_DISPLAY_ROLES
        + ", " + EdRecordSections.DB_DEFAULT_EDIT_ROLES
        + ", " + EdRecordSections.DB_PERCENT_WIDTH
        + "  )  \r\n"
        + "VALUES ("
        + "  " + EdRecordSections.PARM_FORM_GUID
        + ", " + EdRecordSections.PARM_NUMBER + "_" + count
        + ", " + EdRecordSections.PARM_ORDER + "_" + count
        + ", " + EdRecordSections.PARM_NAME + "_" + count
        + ", " + EdRecordSections.PARM_INSTRUCTIONS + "_" + count
        + ", " + EdRecordSections.PARM_FIELD_NAME + "_" + count
        + ", " + EdRecordSections.PARM_FIELD_VALUE + "_" + count
        + ", " + EdRecordSections.PARM_ON_MATCH_VISIBLE + "_" + count
        + ", " + EdRecordSections.PARM_VISIBLE + "_" + count
        + ", " + EdRecordSections.PARM_DEFAULT_DISPLAY_ROLES + "_" + count
        + ", " + EdRecordSections.PARM_DEFAULT_EDIT_ROLES + "_" + count
        + ", " + EdRecordSections.PARM_PERCENT_WIDTH + "_" + count + " );  \r\n" );
      }

      if ( parmList.Count > 1 )
      {
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
        this.LogDebug ( sbSQL_AddQuery.ToString ( ) );
        this.LogDebug ( EvSqlMethods.getParameterSqlText ( parms ) );

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

      }//END parameter list greater then 1
      return EvEventCodes.Ok;

    }//END UpdateItem method


    #endregion

  }//END EvFormFields class

}//END namespace Evado.Digital.Dal
