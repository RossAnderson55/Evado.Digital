/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\Records.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2020 EVADO HOLDING PTY. LTD..  All rights reserved.
 *     
 *      The use and distribution terms for this software are contained in the file
 *      named \license.txt, which can be found in the root of this distribution.
 *      By using this software in any fashion, you are agreeing to be bound by the
 *      terms of this license.
 *     
 *      You must not remove this notice, or any other, from this software.
 *     
 * </copyright>
 * 
 * Description: 
 *  This class contains the AbstractedPage ResultData object.
 *
 ****************************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.SessionState;

using Evado.Bll;
using Evado.Model;
using Evado.Bll.Digital;
using Evado.Model.Digital;
// using Evado.Web;

namespace Evado.UniForm.Digital
{
  /// <summary>
  /// This class defines the application analysis class
  /// 
  /// This class terminates the Customer object.
  /// </summary>
  public class EuAnalysis : EuClassAdapterBase
  {
    #region Class Initialisation
    /// <summary>
    /// This method initialises the class.
    /// </summary>
    public EuAnalysis ( )
    {
    }

    /// <summary>
    /// This method initialises the class and passs in the user profile.
    /// </summary>
    public EuAnalysis (
      EuGlobalObjects ApplicationObjects,
      EvUserProfileBase ServiceUserProfile,
      EuSession SessionObjects,
      String UniForm_BinaryFilePath,
      String UniForm_BinaryServiceUrl,
      EvClassParameters Settings )
    {

      this.ClassNameSpace = "Evado.UniForm.Clinical.EuAnalysis.";
      this.AdapterObjects = ApplicationObjects;
      this.ServiceUserProfile = ServiceUserProfile;
      this.Session = SessionObjects;
      this.UniForm_BinaryFilePath = UniForm_BinaryFilePath;
      this.UniForm_BinaryServiceUrl = UniForm_BinaryServiceUrl;
      this.ClassParameters = Settings;


      this.LogInitMethod ( "EuAnalysis initialisation" );
      this.LogInit ( "ServiceUserProfile.UserId: " + ServiceUserProfile.UserId );
      this.LogInit ( "SessionObjects.UserProfile.UserId: " + this.Session.UserProfile.UserId );
      this.LogInit ( "SessionObjects.UserProfile.CommonName: " + this.Session.UserProfile.CommonName );
      this.LogInit ( "UniForm BinaryFilePath: " + this.UniForm_BinaryFilePath );
      this.LogInit ( "UniForm BinaryServiceUrl: " + this.UniForm_BinaryServiceUrl );

      this.LogInit ( "Settings:" );
      this.LogInit ( "-LoggingLevel: " + Settings.LoggingLevel );
      this.LogInit ( "-UserId: " + Settings.UserProfile.UserId );
      this.LogInit ( "-UserCommonName: " + Settings.UserProfile.CommonName );


      this._Bll_FormFields = new Evado.Bll.Digital.EdRecordFields ( this.ClassParameters );
      this._Bll_FormRecords = new Evado.Bll.Digital.EdRecords ( this.ClassParameters );
      this._Bll_DataAnalysis = new Evado.Bll.Digital.EvDataAnalysis ( this.ClassParameters );

    }//END Method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class constants and variables.

    private Evado.Bll.Digital.EdRecordFields _Bll_FormFields = new Evado.Bll.Digital.EdRecordFields ( );
    private Evado.Bll.Digital.EdRecords _Bll_FormRecords = new Evado.Bll.Digital.EdRecords ( );

    private Evado.Bll.Digital.EvDataAnalysis _Bll_DataAnalysis = new Evado.Bll.Digital.EvDataAnalysis ( );

    private bool _RunQuery = false;
    private bool _ExportData = false;

    private const string CONST_QUERY_COMMAND = "DAQVCMD";
    private const string CONST_EXPORT_COMMAND = "DAQVEXP";
    private const string CONST_ITEM = "DAQVI_";
    private const string CONST_AGGREGATION = "DAQVA_";

    private const string CONST_FORM_ID = "TRQFID";
    private const string CONST_FIELD_ID = "TRQFFOD";
    private const string CONST_FIELD_VALUE = "TRQFV";

    private const string CONST_NAME_SPACE = "Evado.UniForm.Clinical.Analysis.";
    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class public methods

    // ==================================================================================
    /// <summary>
    /// This method gets the trial site object.
    /// 
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object</param>
    /// <returns>Evado.Model.UniForm.AppData</returns>
    //  ----------------------------------------------------------------------------------
    override public Evado.Model.UniForm.AppData getDataObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getDataObject" );
      this.LogValue ( "PageCommand " + PageCommand.getAsString ( false, false ) );

      try
      {
        // 
        // Initialise the methods variables and objects.
        // 
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );

        // 
        // Set the page type to control the DB query type.
        // 
        string pageType = PageCommand.GetPageId ( );

        this.Session.setPageId ( pageType );

        this.LogValue ( "PageType: " + this.Session.PageId );
        // 
        // Determine the method to be called
        // 
        switch ( PageCommand.Method )
        {
          case Evado.Model.UniForm.ApplicationMethods.List_of_Objects:
            {
              switch ( this.Session.PageId )
              {
                case EvPageIds.Data_Charting_Page:
                  {
                    clientDataObject = this.getChartObject ( PageCommand );
                    break;
                  }
                case EvPageIds.Record_Query_Page:
                  {
                    clientDataObject = this.getRecordQueryObject ( PageCommand );
                    break;
                  }
                default:
                  {
                    this.LogValue ( "Execute the default page" );

                    clientDataObject = this.getChartObject ( PageCommand );
                    break;

                    //return null;
                  }
              }//END RecordPageType switch
              break;
            }
          default:
            {
              // 
              // Return the generated ResultData object.
              // 
              break;
            }

        }//END Switch

        // 
        // Handle returned exceptions.
        // 
        if ( clientDataObject == null )
        {
          this.LogValue ( " null application data returned." );

          clientDataObject = this.Session.LastPage;
        }

        //
        // If an errot message exist display it.
        //
        if ( this.ErrorMessage != String.Empty )
        {
          clientDataObject.Message = this.ErrorMessage;
        }

        // 
        // return the client ResultData object.
        // 
        return clientDataObject;

      }
      catch ( Exception Ex )
      {
        this.LogException ( Ex );
      }
      return this.Session.LastPage;

    }//END getClientDataObject method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Private charting query methods

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData getChartObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getChartObject" );
      try
      {
        // 
        // Initialise the methods variables and objects.
        //      
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
        Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );

        //
        // Determine if the user has access to this page and log and error if they do not.
        //
        if (  this.Session.UserProfile.hasManagementAccess == false
          && this.Session.UserProfile.hasManagementAccess == false )
        {
          this.LogIllegalAccess (
            this.ClassNameSpace + "getChartObject",
            this.Session.UserProfile );

          this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

          return this.Session.LastPage;
        }

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "getChartObject",
          this.Session.UserProfile );

        //
        // Initialise the chart object and the selection arrays if it is null.
        //
        if ( this.Session.Chart == null )
        {
          this.Session.Chart = new EvChart ( );
          this.Session.ChartSourceOptionList = new List<EvOption> ( );

          for ( int index = 0; index < 5; index++ )
          {
            this.Session.Chart.QueryItems.Add ( new EvChartQueryItem ( ) );
          }

        }//END initialise the chart object.

        //
        // Update the selection values.
        //
        this.getChart_Update_Selection_Values ( PageCommand );

        //
        // Initialise the client ResultData object.
        //
        clientDataObject.Title = EdLabels.Analysis_Chart_Query_Page_Title;
        clientDataObject.Page.Title = clientDataObject.Title;
        clientDataObject.Id = Guid.NewGuid ( );

        // 
        // Generate the page commands and groups
        // 

        this.getChart_Selection_Group ( clientDataObject.Page );

        this.getChart_Display_Group ( clientDataObject.Page );

        this.getChart_Export_Group ( clientDataObject.Page );

        this.getChart_Page_Commands ( clientDataObject.Page );

        return clientDataObject;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.Analysis_Chart_Page_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return this.Session.LastPage;

    }//END getChartObject method.


    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private void getChart_Update_Selection_Values (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getChart_UpdateSelection" );
      //
      // Define method variables and objects.
      //
      String parameterValue = String.Empty;
      this._RunQuery = false;
      this._ExportData = false;

      //
      // sense whether the query needs to be run.
      //
      parameterValue = PageCommand.GetParameter ( EuAnalysis.CONST_EXPORT_COMMAND );
      if ( parameterValue != String.Empty )
      {
        this._ExportData = true;
      }

      //
      // Sense whether the export needs to be run.
      //
      parameterValue = PageCommand.GetParameter ( EuAnalysis.CONST_QUERY_COMMAND );
      if ( parameterValue != String.Empty )
      {
        this._RunQuery = true;
      }

      //
      // Iteration through list of query item parameters.
      //
      for ( int index = 0; index < this.Session.Chart.QueryItems.Count; index++ )
      {
        //
        // if the source string exists, update the parameters
        //
        parameterValue = PageCommand.GetParameter ( EuAnalysis.CONST_ITEM + index );

        if ( parameterValue != String.Empty )
        {
          this.Session.Chart.QueryItems [ index ].ItemId = parameterValue;

          this.Session.Chart.QueryItems [ index ].ItemName = this.getItemName ( parameterValue );

          parameterValue = PageCommand.GetParameter ( EuAnalysis.CONST_AGGREGATION + index );
          this.Session.Chart.QueryItems [ index ].Aggregation =
             Evado.Model.EvStatics.parseEnumValue<EvChart.AggregationOptions> (
            parameterValue );

          this.LogValue ( "[ " + index + " ].ItemId: " + this.Session.Chart.QueryItems [ index ].ItemId );
          this.LogValue ( "[ " + index + " ].ItemName: " + this.Session.Chart.QueryItems [ index ].ItemName );
          this.LogValue ( "[ " + index + " ].Aggregation: " + this.Session.Chart.QueryItems [ index ].Aggregation );

        }//END parameter exists.
        else
        {
          this.Session.Chart.QueryItems [ index ] = new EvChartQueryItem ( );
        }

      }//END iteration loop.

    }//END getChart_UpdateSelection values.

    //==================================================================================
    /// <summary>
    /// this method retrieves the item name for an item identifier.
    /// </summary>
    /// <param name="ItemId">String identifier</param>
    /// <returns>String: item name.</returns>
    //-----------------------------------------------------------------------------------
    private String getItemName ( String ItemId )
    {
      foreach ( EvOption option in this.Session.ChartSourceOptionList )
      {
        if ( option.Value == ItemId )
        {
          return option.Description;
        }

      }
      return string.Empty;

    }//END method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">List of paremeters to retrieve the selected object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private void getChart_Page_Commands (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getChart_Page_Commands" );
      //
      // Initialise method variables and objects.
      //
      Evado.Model.UniForm.Command groupCommand = new Model.UniForm.Command ( );

      //
      // Add the chart generation page groupCommand.
      //
      groupCommand = Page.addCommand (
        EdLabels.Analysis_Chart_Query_Command_Title,
            EuAdapter.ADAPTER_ID,
            EuAdapterClasses.Analysis.ToString ( ),
            Model.UniForm.ApplicationMethods.Custom_Method );

      groupCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.List_of_Objects );

      groupCommand.AddParameter ( EuAnalysis.CONST_QUERY_COMMAND, "1" );

      groupCommand.AddParameter ( Model.UniForm.CommandParameters.Page_Id,
         Evado.Model.Digital.EvPageIds.Data_Charting_Page );

      //
      // Add the chart export page groupCommand if ResultData exists.
      //
      if ( this.Session.Chart.Series.Count == 0 )
      {
        return;
      }
      if ( this.Session.Chart.Series [ 0 ].ItemId == String.Empty )
      {
        return;
      }

      groupCommand = Page.addCommand (
        EdLabels.Analysis_Chart_Export_Command_Title,
            EuAdapter.ADAPTER_ID,
            EuAdapterClasses.Analysis.ToString ( ),
            Model.UniForm.ApplicationMethods.Custom_Method );

      groupCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.List_of_Objects );

      groupCommand.AddParameter ( EuAnalysis.CONST_EXPORT_COMMAND, "1" );

      groupCommand.AddParameter ( Model.UniForm.CommandParameters.Page_Id,
         Evado.Model.Digital.EvPageIds.Data_Charting_Page );

    }//END getChart_Page_Commands method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">List of paremeters to retrieve the selected object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private void getChart_Selection_Group (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getChart_Selection_Group" );
      //
      // Initialise method variables and objects.
      //
      Evado.Model.UniForm.Field groupField = new Model.UniForm.Field ( );
      Evado.Model.UniForm.Command groupCommand = new Model.UniForm.Command ( );
      List<EvOption> optionList = new List<EvOption> ( );

      //
      // Fill the chart source selection list as a list of numeric form field objects.
      //
      if ( this.Session.ChartSourceOptionList.Count == 0 )
      {
        this.loadCharFieldOptions ( );
        /*
        this.Session.ChartSourceLists = this._Bll_FormFields.getChartOptionList (
          this.Session.Trial.TrialId );

        this.LogDebug ( this._Bll_FormFields.Log );
         */
      }
      this.LogDebug ( "ChartSourceLists count: " + this.Session.ChartSourceOptionList.Count );

      //
      // If there are no numeric values.
      //
      if ( this.Session.ChartSourceOptionList.Count == 0 )
      {
        Evado.Model.UniForm.Group pageGroup = Page.AddGroup (
          EdLabels.Analysis_Chart_Selection_Group_Title,
          EdLabels.Analysis_Chart_Selection_Group_No_Value_Message,
          Evado.Model.UniForm.EditAccess.Enabled );
        pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

        return;
      }

      //
      // Iterate through the querty items creating the item and the selection list.
      //
      for ( int index = 0; index < 4; index++ )
      {
        if ( this.Session.Chart.QueryItems.Count < ( index + 1 ) )
        {
          this.Session.Chart.QueryItems.Add ( new EvChartQueryItem ( ) );
        }

        // 
        // Create the new pageMenuGroup.
        // 
        Evado.Model.UniForm.Group pageGroup = Page.AddGroup (
          EdLabels.Analysis_Chart_Selection_Group_Title,
          String.Empty,
          Evado.Model.UniForm.EditAccess.Enabled );
        pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

        switch ( index )
        {
          case 1:
            {
              pageGroup.Title = EdLabels.Analysis_Chart_Selection_Group_Title_1;
              break;
            }
          case 2:
            {
              pageGroup.Title = EdLabels.Analysis_Chart_Selection_Group_Title_2;
              break;
            }
          case 3:
            {
              pageGroup.Title = EdLabels.Analysis_Chart_Selection_Group_Title_3;
              break;
            }
          case 4:
            {
              pageGroup.Title = EdLabels.Analysis_Chart_Selection_Group_Title_4;
              break;
            }
        }//END pageMenuGroup title switch.

        //
        // Define the report project selection list.
        //
        groupField = pageGroup.createSelectionListField (
          EuAnalysis.CONST_ITEM + index,
          EdLabels.Analysis_Chart_Selection_Source_Field_Label,
          this.Session.Chart.QueryItems [ index ].ItemId,
          this.Session.ChartSourceOptionList );

        groupField.Layout = EuAdapter.DefaultFieldLayout;

        //
        // Define the report Type selection
        //
        optionList = Evado.Model.EvStatics.getOptionsFromEnum ( typeof ( Evado.Model.Digital.EvChart.AggregationOptions ), true );

        groupField = pageGroup.createSelectionListField (
          EuAnalysis.CONST_AGGREGATION + index,
          EdLabels.Analysis_Chart_Selection_Aggregation_Field_Label,
          this.Session.Chart.QueryItems [ index ].Aggregation.ToString ( ),
          optionList );

        groupField.Layout = EuAdapter.DefaultFieldLayout;

      }//END iteration loop.

    }//END getChart_Selection_Group method


    // ==============================================================================
    /// <summary>
    /// This method loads the chart field option list.
    /// </summary>
    //  ------------------------------------------------------------------------------
    private void loadCharFieldOptions ( )
    {
      this.LogMethod ( "loadCharFieldOptions" );
      //this.LogDebug ( "Form list count: " + this.Session.FormList.Count );
      //
      // Initialise the methods variables and objects.
      //
      this.Session.ChartSourceOptionList = new List<EvOption> ( );
      this.Session.ChartSourceOptionList.Add ( new EvOption ( ) );

      foreach ( EdRecord form in this.AdapterObjects.IssuedRecordLayouts )
      {
        if ( form.State != EdRecordObjectStates.Form_Issued )
        {
          continue;
        }

        //this.LogDebug ( "Form ID {0}, State {1} , field.count {2}.",
        //  form.FormId, form.State, form.Fields.Count );

        foreach ( EdRecordField field in form.Fields )
        {
          //
          // Select numeric fields.
          //
          if ( field.Design.hasNumericValues == true )
          {
            //this.LogDebug ( "Numeric Field ID {0},  type {1}.", field.FieldId, field.TypeId );

            var option = this.createChartOption ( field );

            if ( option != null )
            {
              this.Session.ChartSourceOptionList.Add ( option );
            }
          }

        }//End field iteration loop

      }//END form iteration loop

      this.LogDebug ( "ChartSourceOptionList.Count {0}.", this.Session.ChartSourceOptionList.Count );


      this.LogMethodEnd ( "loadCharFieldOptions" );
    }

    // ==============================================================================
    /// <summary>
    /// This method creates the chart option object.
    /// </summary>
    //  ------------------------------------------------------------------------------
    private EvOption createChartOption ( EdRecordField Field )
    {
      //this.LogMethod ( "createChartOption" );
      //
      // Initialise the methods variables and objects.
      //
      EvOption option = new EvOption ( );

      if ( Field.LayoutId == null )
      {
        Field.LayoutId = String.Empty;
      }

      //
      // if it is a non numeric field the continue to the next field.
      //
      if ( Field.Design.hasNumericValues == false )
      {
        //this.LogDebug ( "Form: {0}, Field: {1}, type: {2} is a non numeric field",
        //  Field.FormId, Field.FieldId, Field.TypeId );
        //this.LogMethodEnd ( "createChartOption" );
        return null;
      }

      //this.LogDebug ( "Form: {0}, Field: {1}, type: {2} is a numeric field",
       //   Field.FormId, Field.FieldId, Field.TypeId );

      option = new EvOption (
          Field.LayoutId + EvChart.CONST_SOURCE_DELIMITER + Field.FieldId,
          Field.LayoutId + " : " + Field.FieldId + " - " + Field.Title );

      if ( option.Description.Length > 80 )
      {
        option.Description = option.Description.Substring ( 0, 80 ) + " ...";
      }

      //this.LogMethodEnd ( "createChartOption" );
      return option;
    }

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">List of paremeters to retrieve the selected object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    /// <remarks>
    /// This method consists of following steps:
    /// 
    /// 1. create a page group to display the summaries of queried data.
    /// 
    /// 2. Define a table and its headers 
    /// 
    /// 3. For each chart, add a row to the table.
    /// </remarks>
    //  ------------------------------------------------------------------------------
    private void getChart_Display_Group (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getChart_Display_Group" );
      //
      // Initialise method variables and objects.
      //
      Evado.Model.UniForm.Field groupField = new Model.UniForm.Field ( );
      Evado.Model.UniForm.Command groupCommand = new Model.UniForm.Command ( );
      int tableCategoryLength = 0;

      // 
      // Query and database.
      // 
      if ( this._RunQuery == true )
      {
        this.Session.Chart.Series = new List<EvChartSeries> ( );
        //this.Session.Chart = this._Bll_DataAnalysis.runQuery ( this.Session.Chart );

        this.LogValue ( this._Bll_DataAnalysis.Log );
      }

      if ( this.Session.Chart.Categories.Count == 0 )
      {
        this.ErrorMessage = "No categories were generated. ";

        this.LogValue ( "ERROR: " + this.ErrorMessage );

        return;
      }

      //
      // if there is no chart series there is nothing to display.
      //
      if ( this.Session.Chart.Series.Count == 0 )
      {
        return;
      }

      // 
      // Create the new pageMenuGroup.
      // 
      Evado.Model.UniForm.Group pageGroup = Page.AddGroup (
        EdLabels.Analysis_Chart_Display_Group_Title,
        String.Empty,
        Evado.Model.UniForm.EditAccess.Enabled );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      //
      // define the table pageMenuGroup field.
      //
      pageGroup.addField ( groupField );

      groupField.Type = EvDataTypes.Table;
      groupField.Layout = Model.UniForm.FieldLayoutCodes.Column_Layout;
      groupField.Table = new Model.UniForm.Table ( );

      // 
      // If nothing returned create a blank row.
      // 
      if ( this.Session.Chart.Series.Count == 0 )
      {
        this.LogValue ( "No charting data was returned." );
        this.Session.Chart.Series.Add ( new EvChartSeries ( ) );
      }

      //
      // Set the chart category length.
      //
      tableCategoryLength = this.Session.Chart.Categories.Count;

      if ( tableCategoryLength > 7 )
      {
        tableCategoryLength = 7;
      }
      groupField.Table.setHeader ( tableCategoryLength );

      this.LogValue ( "tableCategoryLength: " + tableCategoryLength );
      this.LogValue ( "Table.ColumnCount: " + groupField.Table.ColumnCount );
      //
      // reinitialise the table column tableColumn.
      //
      groupField.Table.Header [ 0 ].Text = EdLabels.Analysis_Chart_Group_Table_Column_0_Label;
      groupField.Table.Header [ 0 ].TypeId = EvDataTypes.Read_Only_Text;
      groupField.Table.Header [ 0 ].Width = "10";
      groupField.Table.Header [ 1 ].Text = EdLabels.Analysis_Chart_Group_Table_Column_1_Label;
      groupField.Table.Header [ 1 ].TypeId = EvDataTypes.Read_Only_Text;
      groupField.Table.Header [ 1 ].Width = "20";
      groupField.Table.Header [ 2 ].Text = EdLabels.Analysis_Chart_Group_Table_Column_2_Label;
      groupField.Table.Header [ 2 ].TypeId = EvDataTypes.Read_Only_Text;
      groupField.Table.Header [ 2 ].Width = "10";

      // 
      // Iterate through the columns setting the table header text
      // 
      for ( int index = 3; index < tableCategoryLength; index++ )
      {
        groupField.Table.Header [ index ].Text = this.Session.Chart.Categories [ index ];
        groupField.Table.Header [ index ].TypeId = EvDataTypes.Read_Only_Text;
        groupField.Table.Header [ index ].Width = "10";
      }

      // 
      // generate the page links.
      // 
      foreach ( EvChartSeries series in this.Session.Chart.Series )
      {
        Evado.Model.UniForm.TableRow row = new Model.UniForm.TableRow ( );

        this.LogValue ( "row.Column.Length: " + row.Column.Length );
        this.LogValue ( "series.Values.Count: " + series.Values.Count );

        row.Column [ 0 ] = series.ItemId;
        row.Column [ 1 ] = series.Legend;
        row.Column [ 2 ] = series.Unit;

        this.LogValue ( "row.Column [ 0 ]: " + row.Column [ 0 ] );
        this.LogValue ( "row.Column [ 1 ]: " + row.Column [ 1 ] );
        this.LogValue ( "row.Column [ 2 ]: " + row.Column [ 2 ] );

        for ( int index = 0; index < tableCategoryLength && index < series.Values.Count; index++ )
        {
          int column = index + 3;

          row.Column [ column ] = series.Values [ index ].ToString ( "###0.00" );

          this.LogValue ( "row.Column [ " + column + " ]: " + row.Column [ 2 ] );
        }

        groupField.Table.Rows.Add ( row );

      }//END chart value iteration loop

    }//END getChart_Display_Group method.

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">List of paremeters to retrieve the selected object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private void getChart_Export_Group (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getChart_Export_Group" );
      //
      // Initialise method variables and objects.
      //
      Evado.Model.UniForm.Field groupField = new Model.UniForm.Field ( );
      String filename =  "DAQ_"
          + DateTime.Now.ToString ( "yyyyMMdd_HHmm" ) + ".csv";

      if ( this._ExportData == false )
      {
        this.LogValue ( "Export command not called. " );
        return;
      }

      String chartCsvData = this.Session.Chart.getCsvOutput ( );

      //
      // Save the chart CSv content to the uniform output directory.
      //
      Evado.Model.Digital.EvcStatics.Files.saveFile ( this.UniForm_BinaryFilePath, filename, chartCsvData );

      String htmlLink = this.UniForm_BinaryServiceUrl + filename;
      // 
      // Create the new pageMenuGroup.
      // 
      Evado.Model.UniForm.Group pageGroup = Page.AddGroup (
        EdLabels.Analysis_Chart_Export_Group_Title,
        String.Empty,
        Evado.Model.UniForm.EditAccess.Inherited );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      groupField = pageGroup.createHtmlLinkField (
        String.Empty,
        EdLabels.Analysis_Chart_Download_Link_Field_Title,
        htmlLink );

    }//END getChart_Export_Group method.

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Private ResultData query query methods

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">List of paremeters to retrieve the selected object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData getRecordQueryObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getRecordQueryObject" );
      try
      {
        // 
        // Initialise the methods variables and objects.
        //      
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
        Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );

        //
        // Determine if the user has access to this page and log and error if they do not.
        //
        if (  this.Session.UserProfile.hasManagementAccess == false
          && this.Session.UserProfile.hasManagementAccess == false )
        {
          this.LogIllegalAccess (
            "Evado.UniForm.Clinical.Reports.getRecordQueryObject",
            this.Session.UserProfile );

          this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

          return this.Session.LastPage;
        }

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          "Evado.UniForm.Clinical.Reports.getRecordQueryObject",
          this.Session.UserProfile );
        //
        // Initialise the analysis object if not already initialised.
        //
        if ( this.Session.AnalysisQueryFormId == null )
        {
          this.LogValue ( "Initialise query clinical object parameters" );
          this.Session.AnalysisRecordlist = new List<EdRecord> ( );
          this.Session.AnalysisQueryFormId = String.Empty;
          this.Session.AnalysisQueryFormFieldId = String.Empty;
          this.Session.AnalysisQueryFormFieldValue = String.Empty;
          this.Session.AnalysisFormSelectionList = new List<EvOption> ( );
          this.Session.AnalysisFormFieldSelectionList = new List<EvOption> ( );
          this.Session.AnalysisFormFieldValueSelectionList = new List<EvOption> ( );
        }

        //
        // Update the selection values.
        //
        this.getRecordQuery_Update_Selection_Values ( PageCommand );

        //
        // Update the form, field and value selection lists.
        //
        this.getRecordQuery_SelectionLists ( );

        //
        // update the record list.
        //
        this.getRecordQuery_Records ( );

        //
        // Initialise the client ResultData object.
        //
        clientDataObject.Title = EdLabels.Analysis_Query_Page_Title;
        clientDataObject.Page.Title = clientDataObject.Title;
        clientDataObject.Id = Guid.NewGuid ( );

        // 
        // Generate the page commands and groups
        // 
        this.getRecordQuery_Selection_Group ( clientDataObject.Page );

        this.getRecordQuery_Export_Group ( clientDataObject.Page );

        this.getRecordQuery_Display_Group ( clientDataObject.Page );

        this.getRecordQuery_Page_Commands ( clientDataObject.Page );

        // 
        // return the resulting client data object.
        // 
        return clientDataObject;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.Analysis_Query_Page_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return this.Session.LastPage;

    }//END getRecordQueryObject method.

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">List of paremeters to retrieve the selected object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private void getRecordQuery_Update_Selection_Values (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getRecordQuery_UpdateSelection" );
      //
      // Define method variables and objects.
      //
      String parameterValue = String.Empty;
      this._RunQuery = false;
      this._ExportData = false;

      //
      // sense whether the query needs to be run.
      //
      parameterValue = PageCommand.GetParameter ( EuAnalysis.CONST_EXPORT_COMMAND );
      if ( parameterValue != String.Empty )
      {
        this._ExportData = true;
      }

      //
      // Sense whether the export needs to be run.
      //
      parameterValue = PageCommand.GetParameter ( EuAnalysis.CONST_QUERY_COMMAND );
      if ( parameterValue != String.Empty )
      {
        this._RunQuery = true;
      }

      //
      // if query is false do not update the query selection.
      //
      if ( this._RunQuery == false )
      {
        return;
      }

      //
      // Update the selected form identifier
      //
      this.Session.AnalysisQueryFormId = PageCommand.GetParameter ( EuAnalysis.CONST_FORM_ID );

      //
      // Update the selected form field identifier
      //
      this.Session.AnalysisQueryFormFieldId = PageCommand.GetParameter ( EuAnalysis.CONST_FIELD_ID );

      //
      // Update the selected form field value
      //
      this.Session.AnalysisQueryFormFieldValue = PageCommand.GetParameter ( EuAnalysis.CONST_FIELD_VALUE );

    }//END getRecordQuery_UpdateSelection values.

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private void getRecordQuery_SelectionLists ( )
    {
      this.LogMethod ( "getRecordQuery_SelectionLists" );
      this.LogValue ( "AnalysisQueryFormId: " + this.Session.AnalysisQueryFormId );
      this.LogValue ( "AnalysisQueryFormFieldId: " + this.Session.AnalysisQueryFormFieldId );
      //
      // Define method variables and objects.
      //
      String parameterValue = String.Empty;
      Evado.Bll.Digital.EdRecordLayouts bllForms = new Evado.Bll.Digital.EdRecordLayouts ( );
      Evado.Bll.Digital.EdRecordFields bllEvFormFields = new Evado.Bll.Digital.EdRecordFields ( );
      Evado.Bll.Digital.EvFormRecordFields trialRecordFields = new Evado.Bll.Digital.EvFormRecordFields ( );

      //
      // Get the form selection list.
      //
      if ( this.Session.AnalysisFormSelectionList.Count == 0 )
      {
        this.Session.AnalysisFormSelectionList = bllForms.getList (
            EdRecordTypes.Null,
            EdRecordObjectStates.Form_Issued,
            false );

        this.LogValue ( "Form selection list count: " + this.Session.AnalysisFormSelectionList.Count );
      }

      //
      // get the form field selection list.
      //
      if ( this.Session.AnalysisQueryFormId != String.Empty )
      {
        this.Session.AnalysisFormFieldSelectionList = bllEvFormFields.GetOptionList (
          this.Session.AnalysisQueryFormId,
          true );

        this.LogValue ( "Field selection list count: " + this.Session.AnalysisFormFieldSelectionList.Count );
      }
      else
      {
        this.Session.AnalysisFormFieldSelectionList = new List<EvOption> ( );
      }

      //
      // get the form field selection list.
      //
      if ( this.Session.AnalysisQueryFormFieldId != String.Empty )
      {/*
        this.Session.AnalysisFormFieldValueSelectionList = trialRecordFields.GetItemValueList (
          this.Session.Application.ApplicationId,
          this.Session.AnalysisQueryFormId,
          this.Session.AnalysisQueryFormFieldId );
        */
        this.LogValue ( "Value selection list count: " + this.Session.AnalysisFormFieldValueSelectionList.Count );
      }
      else
      {
        this.Session.AnalysisFormFieldValueSelectionList = new List<EvOption> ( );
      }




    }//END getRecordQuery_SelectionLists values.

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private void getRecordQuery_Records ( )
    {
      this.LogMethod ( "getRecordQuery_Records" );
      this.LogValue ( "AnalysisQueryFormId: " + this.Session.AnalysisQueryFormId );
      this.LogValue ( "AnalysisQueryFormFieldId: " + this.Session.AnalysisQueryFormFieldId );
      this.LogValue ( "AnalysisQueryFormFieldValue: " + this.Session.AnalysisQueryFormFieldValue );
      //
      // Define method variables and objects.
      //
      String parameterValue = String.Empty;
      Evado.Bll.Digital.EdRecords trialRecords = new Evado.Bll.Digital.EdRecords ( );

      if ( this.Session.AnalysisQueryFormId != String.Empty
        && this.Session.AnalysisQueryFormFieldId != String.Empty
        && this.Session.AnalysisQueryFormFieldValue != String.Empty )
      {
        // 
        // Execute the query.
        // 
        this.Session.AnalysisRecordlist = new List<EdRecord> ( ); 
        /*trialRecords.GetItemQuery (
          this.Session.Application.ApplicationId,
          this.Session.AnalysisQueryFormId,
          this.Session.AnalysisQueryFormFieldId,
          this.Session.AnalysisQueryFormFieldValue ); 
         */
      }
      else
      {
        this.Session.AnalysisRecordlist = new List<EdRecord> ( );
      }

      this.LogValue ( "Record list count: " + this.Session.AnalysisRecordlist.Count );

    }//END getRecordQuery_Records values.

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page"> Evado.Model.UniForm.Page object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private void getRecordQuery_Page_Commands (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getRecordQuery_Page_Commands" );
      this.LogValue ( "AnalysisRecordlist count: " + this.Session.AnalysisRecordlist.Count );
      //
      // Initialise method variables and objects.
      //
      Evado.Model.UniForm.Command groupCommand = new Model.UniForm.Command ( );

      //
      // Add the chart generation page groupCommand.
      //
      groupCommand = Page.addCommand (
        EdLabels.Analysis_Query_Update_Command_Title,
            EuAdapter.ADAPTER_ID,
            EuAdapterClasses.Analysis.ToString ( ),
            Model.UniForm.ApplicationMethods.Custom_Method );

      groupCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.List_of_Objects );

      groupCommand.AddParameter ( EuAnalysis.CONST_QUERY_COMMAND, "1" );

      groupCommand.AddParameter ( Model.UniForm.CommandParameters.Page_Id,
         Evado.Model.Digital.EvPageIds.Record_Query_Page );

      //
      // Add the chart export page groupCommand if ResultData exists.
      //
      if ( this.Session.AnalysisRecordlist.Count == 0 )
      {
        return;
      }
      groupCommand = Page.addCommand (
        EdLabels.Analysis_Query_Export_Command_Title,
            EuAdapter.ADAPTER_ID,
            EuAdapterClasses.Analysis.ToString ( ),
            Model.UniForm.ApplicationMethods.Custom_Method );

      groupCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.List_of_Objects );

      groupCommand.AddParameter ( EuAnalysis.CONST_EXPORT_COMMAND, "1" );

      groupCommand.AddParameter ( Model.UniForm.CommandParameters.Page_Id,
         Evado.Model.Digital.EvPageIds.Record_Query_Page );

    }//END getRecordQuery_Page_Commands method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private void getRecordQuery_Selection_Group (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getRecordQuery_Selection_Group" );
      //
      // Initialise method variables and objects.
      //
      Evado.Model.UniForm.Field groupField = new Model.UniForm.Field ( );
      Evado.Model.UniForm.Command groupCommand = new Model.UniForm.Command ( );
      List<EvOption> optionList = new List<EvOption> ( );

      // 
      // Create the new pageMenuGroup.
      // 
      Evado.Model.UniForm.Group pageGroup = Page.AddGroup (
        EdLabels.Analysis_Query_Selection_Group_Title,
        String.Empty,
        Evado.Model.UniForm.EditAccess.Enabled );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      //
      // Define the query form selection list.
      //
      if ( this.Session.AnalysisFormSelectionList.Count > 0 )
      {
        groupField = pageGroup.createSelectionListField (
          EuAnalysis.CONST_FORM_ID,
          EdLabels.Analysis_Query_Form_Selection_Field_Title,
          this.Session.AnalysisQueryFormId,
          this.Session.AnalysisFormSelectionList );

        groupField.Layout = EuAdapter.DefaultFieldLayout;
        groupField.AddParameter ( Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );
      }

      //
      // Define the query form field selection list.
      //
      if ( this.Session.AnalysisFormFieldSelectionList.Count > 0 )
      {
        groupField = pageGroup.createSelectionListField (
          EuAnalysis.CONST_FIELD_ID,
          EdLabels.Analysis_Query_Field_Selection_Field_Title,
          this.Session.AnalysisQueryFormFieldId,
          this.Session.AnalysisFormFieldSelectionList );

        groupField.Layout = EuAdapter.DefaultFieldLayout;
        groupField.AddParameter ( Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );
      }

      //
      // Define the query form field value selection list.
      //
      if ( this.Session.AnalysisFormFieldValueSelectionList.Count > 0 )
      {
        groupField = pageGroup.createSelectionListField (
          EuAnalysis.CONST_FIELD_VALUE,
          EdLabels.Analysis_Query_Value_Selection_Field_Title,
          this.Session.AnalysisQueryFormFieldValue,
          this.Session.AnalysisFormFieldValueSelectionList );

        groupField.Layout = EuAdapter.DefaultFieldLayout;
        groupField.AddParameter ( Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );
      }

      //
      // Add the chart generation page groupCommand.
      //
      groupCommand = pageGroup.addCommand (
        EdLabels.Analysis_Query_Update_Command_Title,
            EuAdapter.ADAPTER_ID,
            EuAdapterClasses.Analysis.ToString ( ),
            Model.UniForm.ApplicationMethods.Custom_Method );

      groupCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.List_of_Objects );

      groupCommand.AddParameter ( EuAnalysis.CONST_QUERY_COMMAND, "1" );

      groupCommand.AddParameter ( Model.UniForm.CommandParameters.Page_Id,
         Evado.Model.Digital.EvPageIds.Record_Query_Page );

    }//END getRecordQuery_Selection_Group method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private void getRecordQuery_Display_Group (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getRecordQuery_Display_Group" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Model.UniForm.Group ( );
      Evado.Model.UniForm.Command groupCommand = new Model.UniForm.Command ( );

      //
      // exit if there are no records in the list.
      //
      if ( this.Session.AnalysisRecordlist.Count == 0 )
      {
        return;
      }

      // 
      // Create the record display pageMenuGroup.
      // 
      pageGroup = Page.AddGroup (
        EdLabels.Analysis_Query_Record_List_Group_Title,
        Evado.Model.UniForm.EditAccess.Enabled );
      pageGroup.CmdLayout = Evado.Model.UniForm.GroupCommandListLayouts.Vertical_Orientation;

      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      pageGroup.Title += EdLabels.List_Count_Label + this.Session.AnalysisRecordlist.Count;

      // 
      // Iterate through the record list generating a groupCommand to access each record
      // then append the groupCommand to the record pageMenuGroup view's groupCommand list.
      // 
      foreach ( Evado.Model.Digital.EdRecord formRecord in this.Session.AnalysisRecordlist )
      {
        string stTitle = formRecord.RecordId
          + EdLabels.Space_Arrow_Right
          + formRecord.RecordId
          + EdLabels.Space_Open_Bracket
          + EdLabels.Label_Date
          + formRecord.stRecordDate
          + EdLabels.Space_Close_Bracket
          + EdLabels.Space_Open_Bracket
          + EdLabels.Label_Status
          + formRecord.StateDesc + EdLabels.Space_Close_Bracket;

        EuAdapterClasses appObject =
          EuConversions.convertRecordType ( this.Session.RecordType );

        groupCommand = pageGroup.addCommand (
          stTitle,
          EuAdapter.ADAPTER_ID,
          appObject.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Get_Object );

        groupCommand.SetGuid ( formRecord.Guid );

        groupCommand.AddParameter (
          Model.UniForm.CommandParameters.Short_Title,
          EdLabels.Label_Record_Id + formRecord.RecordId );

      }//END iteration loop

      this.LogValue ( "Group command count: " + pageGroup.CommandList.Count );

    }//END getRecordQuery_Display_Group method.

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page object.</param>
    /// <returns>Evado.Model.UniForm.AppData object</returns>
    //  ------------------------------------------------------------------------------
    private void getRecordQuery_Export_Group (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getRecordQuery_Export_Group" );

      if ( this._ExportData == false )
      {
        this.LogValue ( "Export command not called. " );
        return;
      }
      //
      // Initialise method variables and objects.
      //
      EdRecords trialRecords = new EdRecords ( );
      EdRecord record = new EdRecord ( );
      List<EdRecord> formRecordList = new List<EdRecord> ( );
      EvFormRecordExport recordExport = new EvFormRecordExport ( );
      System.Text.StringBuilder sOutput = new StringBuilder ( );
      Evado.Model.UniForm.Field groupField = new Model.UniForm.Field ( );
      String filename = "RECORD_"
          + DateTime.Now.ToString ( "yyyyMMdd_HHmm" ) + ".csv";
      //
      // Iterate through the record to fill a list of records witih all fields and comments.
      //
      foreach ( EdRecord rec in this.Session.AnalysisRecordlist )
      {
        // 
        // Retrieve the records
        // 
        record = trialRecords.getRecord ( rec.Guid );

        this.LogValue ( "RecordId: " + record.RecordId );
        //
        // Add the record to the record list.
        //
        formRecordList.Add ( record );
      }

      // 
      // Create the report header
      // 
      sOutput.AppendLine ( "Subject Test Results" );
      sOutput.AppendLine ( "Run on: " + DateTime.Now.ToString ( "dd MMM yyyy HH:mm" ) );
      sOutput.AppendLine ( " By " + this.Session.UserProfile.CommonName );
      sOutput.AppendLine ( "" );
      sOutput.AppendLine ( "Selection Criteria:" );
      sOutput.AppendLine ( "Form, " + this.Session.AnalysisQueryFormId );
      sOutput.AppendLine ( "field, " + this.Session.AnalysisQueryFormFieldId );
      sOutput.AppendLine ( "Value, " + this.Session.AnalysisQueryFormFieldValue );
      sOutput.AppendLine ( "" );
      sOutput.AppendLine ( "Field Legend: " );
      //
      // generate the record export file.
      //
      sOutput.AppendLine ( recordExport.createExportFile (
          formRecordList,
          this.Session.UserProfile,
          false,
          true ) );
      //
      // Save the chart CSv content to the uniform output directory.
      //
      Evado.Model.Digital.EvcStatics.Files.saveFile ( this.UniForm_BinaryFilePath, filename, sOutput.ToString ( ) );

      String htmlLink = this.UniForm_BinaryServiceUrl + filename;
      // 
      // Create the new pageMenuGroup.
      // 
      Evado.Model.UniForm.Group pageGroup = Page.AddGroup (
        EdLabels.Analysis_Chart_Export_Group_Title,
        String.Empty,
        Evado.Model.UniForm.EditAccess.Inherited );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      groupField = pageGroup.createHtmlLinkField (
        String.Empty,
        EdLabels.Analysis_Chart_Download_Link_Field_Title,
        htmlLink );

    }//END getRecordQuery_Export_Group method.

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
  }
}//END namespace