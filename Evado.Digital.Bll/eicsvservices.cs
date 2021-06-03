/***************************************************************************************
 * <copyright file="BLL\EvDatabaseUpdates.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvDatabaseUpdates business object.
 *
 ****************************************************************************************/

using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;

using Evado.Integration.Model;

namespace Evado.Digital.Bll
{
  /// <summary>
  /// This class handles the Evado integration services.
  /// </summary>
  public class EiCsvServices: EvBllBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EiCsvServices ( )
    {
      this.ClassNameSpace = "Evado.Digital.Bll.Clinical.EiCsvServices.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EiCsvServices ( Evado.Digital.Model.EvClassParameters Settings )
    {
      this.ClassParameters = Settings;
      this.ClassNameSpace = "Evado.Digital.Bll.Clinical.EiCsvServices.";

      if ( this.ClassParameters.LoggingLevel == 0 )
      {
        this.ClassParameters.LoggingLevel = Evado.Digital.Dal.EvStaticSetting.LoggingLevel;
      }

      new Evado.Digital.Bll.EiServices ( Settings );
    }
    #endregion

    #region class initialiseation methods

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region class properties

    private System.Text.StringBuilder _ProcessLog = new System.Text.StringBuilder ( );


    /// <summary>
    ///  This property contains the debug log entries.
    /// </summary>
    public String ProcessLog
    {
      get { return this._ProcessLog.ToString ( ); }
    }

    //public EiSubjects EiSubjects { get; set; }

    private static Evado.Digital.Bll.EiServices EI_Services = new Evado.Digital.Bll.EiServices ( );

    private String _ProjectId = String.Empty;

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Public Query method

    //===================================================================================
    /// <summary>
    /// This method executes a CSV import or export query. 
    /// </summary>
    /// <param name="QueryData"> Evado.Integration.Model.EiData.</param>
    /// <returns>List of String</returns>
    //-----------------------------------------------------------------------------------
    public List<String> ExportData (
        Evado.Integration.Model.EiData QueryData )
    {
      this.LogMethod ( "ExportData method." );
      this.LogDebug ( "QueryType: " + QueryData.QueryType );
      this.writeProcessLog ( "CSV Export Service - Commence processing query data." );
      //
      // Initialise the methods variables and objects.
      //
       Evado.Integration.Model.EiData resultData = new  Evado.Integration.Model.EiData ( );
      Evado.Digital.Model.EdQueryParameters queryParameters =
        new Evado.Digital.Model.EdQueryParameters ( );
      List<String> CsvOutput = new List<string> ( );

      //
      // Query the Ei service
      //
      resultData = EI_Services.ProcessQuery ( QueryData );

      this.LogClass ( EI_Services.Log );

      this._ProcessLog.AppendLine ( resultData.ProcessLog );

      if ( resultData.DataRows == null )
      {
        this.LogDebug ( "No data was generated by the query." );
        this.writeProcessLog ( "No data was generated by the query." );
        return new List<string> ( );
      }
      //
      // Convert the result data object into a CSV file.
      //
      var result =  this.getCsvFromDataObject ( resultData );

      this.LogMethodEnd ( "ExportData" );

      return result;

    }//END ImportCsvData method.

    //===================================================================================
    /// <summary>
    /// This method executes a CSV import or export query. 
    /// </summary>
    /// <param name="UserProfile">Evado.Digital.Model.EdUserProfile object.</param>
    /// <param name="QueryType">EiQueryTypes: Csv encoded data object.</param>
    /// <param name="CsvDataList">List of String: Csv encoded data object.</param>
    /// <returns> Evado.Integration.Model.EiData object</returns>
    //-----------------------------------------------------------------------------------
    public  Evado.Integration.Model.EiData ImportData (
        Evado.Digital.Model.EdUserProfile UserProfile,
        EiQueryTypes QueryType,
        List<String> CsvDataList )
    {
      this.LogMethod ( "ImportData method." );
      this.LogDebug ( "UserProfile: " + UserProfile.CommonName );
      this.LogDebug ( "QueryType: " + QueryType );
      this.writeProcessLog ( "CSV Import Service - Commence processing query data." );
      //
      // Initialise the methods variables and objects.
      //
       Evado.Integration.Model.EiData resultData = new  Evado.Integration.Model.EiData ( );
      Evado.Digital.Model.EdQueryParameters queryParameters =
        new Evado.Digital.Model.EdQueryParameters ( );

      //
      // Get extract the data from the CSV file.
      //
      EiData ImportData = this.getDataObjectFromCsv (
          CsvDataList,
          QueryType );

      /*
      if ( this.Settings.LoggingLevel > 4 )
      {
        foreach ( EiColumnParameters parm in ImportData.Columns )
        {
          string content = String.Format ( "Parm: Field: {0}, Type: {1}, Index: {2}", parm.EvadoFieldId, parm.DataType, parm.Index );
          this.LogValue ( content );
        }
      }
      */
       
      //
      // Execute the query.
      //
      resultData = EI_Services.ProcessQuery ( ImportData );

      this.LogClass ( EI_Services.Log );

      this._ProcessLog.AppendLine ( resultData.ProcessLog );

      this.LogMethodEnd ( "ImportData" );
      return resultData;

    }//END ImportCsvData method.

    #endregion

    #region private methods

    //===================================================================================
    /// <summary>
    /// This method converts a CSV datafile into a integration data object.
    /// </summary>
    /// <param name="CsvDataList">List of String: Csv encoded data object.</param>
    /// <param name="QueryType">EiQueryTypes: Csv encoded data object.</param>
    /// <returns> Evado.Integration.Model.EiData object</returns>
    //-----------------------------------------------------------------------------------
    private EiData getDataObjectFromCsv (
        List<String> CsvDataList,
        EiQueryTypes QueryType )
    {
      this.LogMethod ( "GetDataObjectFromCsv method" );
      this.LogDebug ( "CsvDataList.Count: " + CsvDataList.Count );
      this.LogDebug ( "QueryType: " + QueryType );
      this.writeProcessLog ( "Processing in CSV data object." );

      //
      // Initialise the mathods variables and objects.
      //
      EiData dataObject = new EiData ( );
      String stFileContent = String.Empty;
      String line = String.Empty;
      dataObject.DataRows = new List<EiDataRow> ( );
      dataObject.Columns = new List<EiColumnParameters> ( );
      dataObject.ParameterList = new List<EiQueryParameter> ( );

      this.LogDebug ( "CSV File read: data rows: " + CsvDataList.Count );

      try
      {
        //
        // Iterate through the csv rows.
        //
        for ( int csvRowIndex = 0; csvRowIndex < CsvDataList.Count; csvRowIndex++ )
        {
          this.LogDebug ( "csvRowIndex: " + csvRowIndex );

          String csvRow = CsvDataList [ csvRowIndex ];

          //
          // Skip all empty rows.
          //
          if ( csvRow == String.Empty )
          {
            this.LogDebug ( "SKIP ROW: empty data" );
            continue;
          }

          //
          // the csv content as an array of string.
          //
          String [ ] csvRowArray = GetCsvDataArray ( csvRow );

          this.LogDebug ( "CSV data row length: " + csvRowArray.Length );

          //
          // skip columns less than 2 cels.
          //
          if ( csvRowArray.Length < 2 )
          {
            this.LogDebug ( "SKIP ROW: data row less than 2 columns" );

            continue;
          }

          if ( csvRowArray [ 0 ] == String.Empty )
          {
            this.LogDebug ( "SKIP ROW: No data" );

            continue;
          }

          if ( csvRowArray [ 0 ] == EiData.CONST_QUERY_TYPE )
          {
            this.LogDebug ( "SKIP ROW: Query type" );
            continue;
          }

          //
          // Skip the paramater rows if they are present
          //
          if ( csvRowArray [ 0 ] == EiData.CONST_PARAMETER )
          {
            this.LogDebug ( "SKIP ROW: Parameter row" );
            continue;
          }

          dataObject.QueryType = QueryType;

          this.LogDebug ( "PROCESSING DATA" );

          //
          // Iterate throught the columns of the row.
          //
          for ( int csvColumnIndex = 1; csvColumnIndex < csvRowArray.Length; csvColumnIndex++ )
          {
            //
            // Add parameter if column 0 has paramter value.
            //
            if ( csvRowArray [ 0 ] == EiData.CONST_COLUMN_FIELD_ID
              || csvRowArray [ 0 ] == EiData.CONST_COLUMN_NAME
              || csvRowArray [ 0 ] == EiData.CONST_COLUMN_DATA_TYPE
              || csvRowArray [ 0 ] == EiData.CONST_COLUMN_INDEX )
            {
              this.processDataParameters (
                 dataObject,
                 csvRowArray,
                 csvColumnIndex );

              continue;
            }//END data column paramater 

            //
            // Process the data row.
            //
            if ( csvRowArray [ 0 ] == EiData.CONST_DATA_ROW )
            {
              this.processDataRow (
                dataObject,
                csvRowArray,
                csvColumnIndex );

            }//END data row.

          }//END column iteration loop.

        }//END Row iteration loop

        this.LogDebug ( "DataRows.Count: " + dataObject.DataRows.Count );

        //
        // Add the project Identifier.
        //
        if ( dataObject.DataRows != null )
        {
          int columnIndex = dataObject.getColumnNo (
            "Org_Id" );

          if ( dataObject.DataRows.Count > 0
            && columnIndex > -1 )
          {
            String orgId = dataObject.DataRows [ 0 ].Values [ columnIndex ];
            dataObject.ParameterList.Add ( new EiQueryParameter ( EiQueryParameterNames.Organisation_Id, orgId ) );

            this.LogDebug ( "OrgId: " + orgId );
          }
        }

      }
      catch ( Exception Ex )
      {
        this.LogEvent ( Evado.Model.EvStatics.getException ( Ex ) );

        return dataObject;
      }

      this.LogMethodEnd ( "GetDataObjectFromCsv" );
      //
      // return the data object.
      //
      return dataObject;

    }//END GetDataObjectFromCsv method
    
    //===================================================================================
    /// <summary>
    /// This  method converts a CSV data string into an array of string
    /// </summary>
    /// <param name="csvRow">String of CSV data.</param>
    /// <returns>List of String objects</returns>
    //-----------------------------------------------------------------------------------
    private String [ ] GetCsvDataArray ( String csvRow )
    {
      this.LogMethod ( "GetCsvDataArray method." );
      this.LogDebug ( "csvRow.Length: " + csvRow.Length );

      //
      // If the columns have quotation marks
      //
      if ( csvRow.Contains ( "\",\"" ) == true )
      {
        csvRow = csvRow.Replace ( "\", \"", "\",\"" );
        csvRow = csvRow.Replace ( "\", \"", "\",\"" );
        csvRow = csvRow.Replace ( "\", \"", "\",\"" );
        csvRow = csvRow.Replace ( "\", \"", "\",\"" );
        csvRow = csvRow.Replace ( "\",\"", "~" );
        csvRow = csvRow.Replace ( "\"", "" );
        csvRow = csvRow.Replace ( ",", "~" );
      }
      else
      {
        csvRow = csvRow.Replace ( ",", "~" );
      }
      this.LogDebug ( "ROW: " + csvRow );

      // this.LogDebugValue ( "CSV ROW: " + csvRow );

      String [ ] csvRowArray = csvRow.Split ( '~' );

      this.LogDebug ( "CSV data row length: " + csvRowArray.Length );

      return csvRowArray;
    }

    //===================================================================================
    /// <summary>
    /// This  method updates the data object parameter values
    /// </summary>
    /// <param name="DataObject">EiData object.</param>
    /// <param name="csvRowArray">String Array of CSV data.</param>
    /// <param name="CsvColumnIndex">integet index to CSV column.</param>
    //-----------------------------------------------------------------------------------
    private void processDataParameters (
      EiData DataObject,
      String [ ] csvRowArray,
      int CsvColumnIndex )
    {
      //this.LogMethod ( "processParameterObject method." );
      //
      // Becuase the first column is record identifier the data index points to the column .
      //
      int dataIndex = CsvColumnIndex - 1;

      //
      // Add a column parameter object if data index is greater then the column count.
      //
      if ( DataObject.Columns.Count <= dataIndex
        || dataIndex == 0 )
      {
        //this.LogDebugValue ( "ADD COLUMN PARAMETER OBJECT. " );

        DataObject.Columns.Add ( new EiColumnParameters ( ) );
      }
      //this.LogDebugValue ( "dataObject.Columns.Count: " + DataObject.Columns.Count );

      //
      // select the column field to be updated
      //
      if ( csvRowArray [ 0 ] == EiData.CONST_COLUMN_FIELD_ID )
      {
        //this.LogDebugValue ( String.Format ( "FieldID: index {0}, Value {1} ", CsvColumnIndex, csvRowArray [ CsvColumnIndex ] ) );

        DataObject.Columns [ dataIndex ].EvadoFieldId = csvRowArray [ CsvColumnIndex ];
      }

      //
      // select the column data type to be updated
      //
      if ( csvRowArray [ 0 ] == EiData.CONST_COLUMN_INDEX )
      {
        //this.LogDebugValue ( String.Format ( "Index: index {0}, Field: {1}, Value: {2} ",
        //  CsvColumnIndex, DataObject.Columns [ dataIndex ].EvadoFieldId, csvRowArray [ CsvColumnIndex ] ) );

        DataObject.Columns [ dataIndex ].Index = Evado.Model.EvStatics.getBool ( csvRowArray [ CsvColumnIndex ] );

      }

      //
      // Update the name parameter value
      //
      if ( csvRowArray [ 0 ] == EiData.CONST_COLUMN_DATA_TYPE )
      {
        //this.LogDebugValue ( String.Format ( "Data Type: index {0}, Field: {1}, Value: {2} ",
        //  CsvColumnIndex, DataObject.Columns [ dataIndex ].EvadoFieldId, csvRowArray [ CsvColumnIndex ] ) );

        EiDataTypes type = EiDataTypes.Null;
        if ( Evado.Model.EvStatics.tryParseEnumValue<EiDataTypes> ( csvRowArray [ CsvColumnIndex ], out type ) == true )
        {
          DataObject.Columns [ dataIndex ].DataType = type;
        }
      }

      //
      // select the column data type to be updated
      //
      if ( csvRowArray [ 0 ] == EiData.CONST_COLUMN_NAME )
      {
        //this.LogDebugValue ( String.Format ( "Name: index {0}, Field: {1}, Value: {2} ",
        //  CsvColumnIndex, DataObject.Columns [ dataIndex ].EvadoFieldId, csvRowArray [ CsvColumnIndex ] ) );

        DataObject.Columns [ dataIndex ].Name = csvRowArray [ CsvColumnIndex ];
      }

    }//END processParameterObject method

    //===================================================================================
    /// <summary>
    /// This  method converts a EiDate into a CSV datafile.
    /// </summary>
    /// <param name="DataObject">EiData object.</param>
    /// <param name="csvRowArray">String Array or CAC.</param>
    /// <param name="CsvColumnIndex">integet index to CSv column.</param>
    //-----------------------------------------------------------------------------------
    private void processDataRow (
      EiData DataObject,
      String [ ] csvRowArray,
      int CsvColumnIndex )
    {
      //this.LogMethod ( "processDataRow method." );
      //
      // Becuase the first column is record identifier the data index points to the column .
      //
      int dataIndex = CsvColumnIndex - 1;

      //
      // Add a column parameter object if data index is greater then the column count.
      //
      if ( DataObject.Columns.Count < dataIndex
        || dataIndex == 0 )
      {
        //this.LogDebugValue ( "ADD DATA ROW OBJECT. " );

        DataObject.AddDataRow ( );

        //this.LogDebugValue ( "dataObject.DataRows.Count: " + DataObject.DataRows.Count );
      }

      int dataRowIndex = DataObject.DataRows.Count - 1;

      //
      // select the column field to be updated
      //
      //this.LogDebugValue ( String.Format ( "Value: index {0}, Value {1} ", CsvColumnIndex, csvRowArray [ CsvColumnIndex ] ) );

      DataObject.DataRows [ dataRowIndex ].Values [ dataIndex ] = csvRowArray [ CsvColumnIndex ];

    }//END processParameterObject method

    //===================================================================================
    /// <summary>
    /// This  method converts a EiDate into a CSV datafile.
    /// </summary>
    /// <param name="ExportData">EiData object.</param>
    /// <returns>List of String object</returns>
    //-----------------------------------------------------------------------------------
    private List<String> getCsvFromDataObject (
        EiData ExportData )
    {
      this.LogMethod ( "GetDataObjectFromCsv method started." );
      this.LogDebug ( "CsvDataList.Count: " + ExportData.DataRows.Count );
      this.LogDebug ( "QueryType: " + ExportData.QueryType );
      this.writeProcessLog ( "Processing in CSV data object." );

      //
      // Initialise the mathods variables and objects.
      //
      List<String> csvOutput = new List<string> ( );

      this.LogDebug ( "Export datea rows: " + ExportData.DataRows.Count );

      //
      // If export is null export with nothing.
      //
      if ( ExportData == null )
      {
        this.LogValue ( "ERROR: Null export data" );
        this.LogMethodEnd ( "GetDataObjectFromCsv" );
        return csvOutput;
      }

      //
      // If export is null export with nothing.
      //
      if ( ExportData.DataRows  == null )
      {
        this.LogValue ( "ERROR: Null data rows" );
        this.LogMethodEnd ( "GetDataObjectFromCsv" );
        return csvOutput;
      }

      //
      // Generate the expor header row.
      //
      String fieldId = EiData.CONST_COLUMN_FIELD_ID;
      String dataType = EiData.CONST_COLUMN_DATA_TYPE;
      String indexData = EiData.CONST_COLUMN_INDEX;

      foreach ( EiColumnParameters parameters in ExportData.Columns )
      {
        fieldId += ",\"" + parameters.EvadoFieldId + "\"";
        dataType += ",\"" + parameters.DataType + "\"";
        indexData += ",\"" + parameters.Index + "\"";
      }//END iteration loop

      csvOutput.Add ( fieldId );
      csvOutput.Add ( dataType );
      csvOutput.Add ( indexData );

      //
      // output the exported data.
      //
      if ( ExportData.DataRows.Count > 0 )
      {
        foreach ( EiDataRow row in ExportData.DataRows )
        {
          //
          // Iterate through each of the rows of data.
          //
          String dataRow = EiData.CONST_DATA_ROW;
          foreach ( String data in row.Values )
          {
            dataRow += ",\"" + data + "\"";
          }
          //
          // Add the data row to the cav output.
          //
          csvOutput.Add ( dataRow );

        }//END iteration loop

      }//END data rows exist


      this.LogMethodEnd ( "GetDataObjectFromCsv" );
      //
      // return the data object.
      //
      return csvOutput;

    }//END GetDataObjectFromCsv method

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region debug methods

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Content">String:  debug text.</param>
    //  ----------------------------------------------------------------------------------
    protected void writeProcessLog ( String Content )
    {
      this._ProcessLog.AppendLine (
           DateTime.Now.ToString ( "dd-MM-yy HH:mm:ss" )
         + ": " + Content );
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

  }//END EiService Class.

}//END namespace Evado.BLL 