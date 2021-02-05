/***************************************************************************************
 * <copyright file="DAL\EvRecords.cs" company="EVADO HOLDING PTY. LTD.">
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
using System.Collections.Generic;

//Evado. namespace references.
using Evado.Model;
using Evado.Model.Digital;


namespace Evado.Bll.Clinical
{
  /// <summary>
  /// This business object manages the EvRecords in the system.
  /// </summary>
  public class EvFormRecordExport : EvBllBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EvFormRecordExport ( )
    {
      this.ClassNameSpace = "Evado.Bll.Clinical.EvFormRecordExport.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EvFormRecordExport ( EvClassParameters Settings )
    {
      this.ClassParameter = Settings;
      this.ClassNameSpace = "Evado.Bll.Clinical.EvFormRecordExport.";

      if ( this.ClassParameter.LoggingLevel == 0 )
      {
        this.ClassParameter.LoggingLevel = Evado.Dal.EvStaticSetting.LoggingLevel;
      }
    }
    #endregion

    #region Object Initialisation

    private enum StaticHeaderColumns
    {
      SubjectId,
      OrgId,
      MilestoneId,
      VisitId,
      Visit_Date,
      RecordId,
      RecordDate,
      ReferenceId,
      Subject,
      OnsetDate,
      ResolvedDate,
      CommencementDate,
      CompletionDate,
      Comments,
      AuthoredBy,
      AuthoredDate,
      ReviewedBy,
      ReviewedDate,
      MonitoredBy,
      MonitorDate,
      LockedBy,
      LockedDate,

      ParticipantId,
      ScreeningId,
      SponsorId,
      ExternalId,
      RandomisedId,
      ScheduleId,
      ConsentDate,
      DataConsentDate,
      SharingConsentDate,
      ConfirmConsentDate,
      WithdrawConsentDate,
      WithdrawDataConsentDate,
      WithdrawSharingConsentDate,
      WithdrawConfirmConsentDate,
      ConsentStatus,
      DateOfBirth,
      Age,
      Sex,
      Height,
      Weight,
      Bmi,
      Diseases,
      Categories,
      History,
      SubjectState,
      EarlyWithdrawal,
    }

    // 
    // Instantiate the local variables
    // 
    private String [ ] _ExportColumnHeader = null;
    private String [ ] _ExportColumnRow = null;
    int _OutputDataColumnCount = 0;

    private EvEventCodes _EventCode = EvEventCodes.Ok;
    /// <summary>
    /// This property contains the classes event code result.
    /// </summary>
    public EvEventCodes EventCode
    {
      get { return _EventCode; }
      set { _EventCode = value; }
    }

    #endregion

    #region Public methods

    // =====================================================================================
    /// <summary>
    /// This class returns a list of formfield object based on form's Guid
    /// </summary>
    /// <param name="ExportParameters">EvExportParameters object, defining the items to exported.</param>
    /// <param name="UserProfile">EvUserProfile object.</param>
    /// <returns>List of string where each item is row in a CSv output</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the Formfield list based on form's Guid
    /// 
    /// 2. Return the formfield list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public String exportRecords (
      EvExportParameters ExportParameters,
      Evado.Model.Digital.EdUserProfile UserProfile )
    {
      this.LogMethod ( "exportRecords method. " );
      this.LogDebug ( ExportParameters.getRecordExportParameters ( ) );
      this.LogDebug ( "ExportDataSource: " + ExportParameters.ExportDataSource );
      // 
      // Initialise the methods objects and variables.
      // 
      List<EdRecord> recordList = new List<EdRecord> ( );

      //
      // if the ResultData sourc is not project or common record exit.
      //
      switch ( ExportParameters.ExportDataSource )
      {
        case EvExportParameters.ExportDataSources.Project_Record:
          {
            return this.exportProjectRecords (
              ExportParameters,
              UserProfile );
          }
        default:
          {
            return null;
          }
      }

    }//End getRecordList method.

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region export query record methods

    // =====================================================================================
    /// <summary>
    /// this method exports the project records a list of csv string objects.
    /// </summary>
    /// <param name="ExportParameters">EvExportParameters object, defining the items to exported.</param>
    /// <param name="UserProfile">EvUserProfile object.</param>
    /// <returns>List of string where each item is row in a CSv output</returns>
    // -------------------------------------------------------------------------------------
    private String exportProjectRecords (
      EvExportParameters ExportParameters,
      Evado.Model.Digital.EdUserProfile UserProfile )
    {
      this.LogMethod ( "exportProjectRecords method. " );

      // 
      // Initialise the methods objects and variables.
      // 
      List<EdRecord> recordList = new List<EdRecord> ( );
      EdRecords formRecords = new EdRecords ( this.ClassParameter );

      EdQueryParameters queryParameters = new EdQueryParameters ( );

      //
      // Set the start and end date filters.
      //

      queryParameters.States.Add( EdRecordObjectStates.Withdrawn );
       queryParameters.States.Add( EdRecordObjectStates.Draft_Record );

      if ( ExportParameters.IncludeDraftRecords == true )
      {
        queryParameters.States.Add(  EdRecordObjectStates.Withdrawn );
      }

      queryParameters.NotSelectedState = true;
      queryParameters.IncludeRecordValues = true;
      queryParameters.IncludeSummary = false;
      queryParameters.LayoutId = ExportParameters.LayoutId;

      recordList = formRecords.getRecordList ( queryParameters );

      this.LogClass ( formRecords.Log );

      string result = this.createExportFile (
        recordList,
        UserProfile,
        ExportParameters.IncludeFreeTextData,
        ExportParameters.IncludeDraftRecords );

      this.LogMethodEnd ( "exportProjectRecords" );
      return result;

    }//END exportProjectRecords method


    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region export methods

    // =====================================================================================
    /// <summary>
    /// This method creates the content for the common record export file as a CSV file.
    /// </summary>
    /// <param name="FormRecordList">List of EvForm: a list of form objects</param>
    /// <param name="UserProfile">Evado.Model.Digital.EvUserProfile: The user profile.</param>
    /// <param name="IncludeFreeTextData">Boolean: True, if the free text fields are exported.</param>
    /// <param name="IncludeDraftRecords">Boolean: True, if the initialised records are included</param>
    /// <returns>String: an export file string. </returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the form objects list is empty.
    /// 
    /// 2. Loop through the form list and the associated form field list. 
    /// 
    /// 3. Add the formfield list's values to the output string. 
    /// 
    /// 4. Return the output string. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    public String createExportFile (
      System.Collections.Generic.List<EdRecord> FormRecordList,
      Evado.Model.Digital.EdUserProfile UserProfile,
      bool IncludeFreeTextData,
      bool IncludeDraftRecords )
    {
      this.LogMethod ( "createExportFile method " );
      this.LogDebug ( "IncludeFreeTextData:" + IncludeFreeTextData );
      this.LogDebug ( "IncludeDraftRecords:" + IncludeDraftRecords );
      try
      {
        //
        // Initialise the methods variables and objects.
        //
        EdRecord headerForm = new EdRecord ( );
        System.Text.StringBuilder stCsvData = new System.Text.StringBuilder ( );

        //
        // Only process the common records if more than one exists.
        //
        if ( FormRecordList.Count == 0 )
        {
          this.LogDebug ( "FormRecordList count is zero." );
          this.EventCode = EvEventCodes.Data_Export_Empty_Record_List;

          this.LogMethodEnd ( "createExportFile" );
          return stCsvData.ToString ( );
        }

        //
        // Using the last record get a form header i.e. the latest version.
        //
        headerForm = FormRecordList [ ( FormRecordList.Count - 1 ) ];

        // 
        // Create the report header
        // 
        stCsvData.AppendLine ( Evado.Model.Digital.EvcStatics.encodeCsvFirstColumn ( "Form Type: " + EvStatics.getEnumStringValue( headerForm.TypeId ) ) );
        stCsvData.AppendLine ( Evado.Model.Digital.EvcStatics.encodeCsvFirstColumn ( "FormId: " + headerForm.LayoutId ) );
        stCsvData.AppendLine ( Evado.Model.Digital.EvcStatics.encodeCsvFirstColumn ( "Form Title: " + headerForm.Title ) );
        stCsvData.AppendLine ( Evado.Model.Digital.EvcStatics.encodeCsvFirstColumn ( "Exported on: " + DateTime.Now.ToString ( "dd MMM yyyy HH:mm" )
          + " By " + UserProfile.CommonName ) );

        this.LogDebug ( "Form Type: " + headerForm.Design.TypeId );
        this.LogDebug ( "FormId: " + headerForm.LayoutId );
        this.LogDebug ( "Form Title: " + headerForm.Title );
        this.LogDebug ( "Exported on: " + DateTime.Now.ToString ( "dd MMM yyyy HH:mm" )
          + " By " + UserProfile.CommonName );

        //
        // Ouput the ResultData header.
        //
        this._OutputDataColumnCount = this.createExportRecordHeader ( headerForm, IncludeFreeTextData );

        //
        // Append the CSV data header row.
        //
        stCsvData.AppendLine ( this.ConvertToCsv ( this._ExportColumnHeader ) );
        
         
        this.LogDebug ( "Commence export record data." );
        // 
        // Iterate through the datagrid processing each letter in the grid
        // 
        foreach ( EdRecord record in FormRecordList )
        {
          this._ExportColumnRow = new string [ this._OutputDataColumnCount ];
          // 
          // IF the item is a valid record output it.
          //
          if ( record.State == EdRecordObjectStates.Withdrawn )
          {
            this.LogDebug ( "RecordID: " + record.RecordId + " >> Withdrawn or querid record." );
            continue;
          }

          if ( record.State == EdRecordObjectStates.Draft_Record
            && IncludeDraftRecords == false )
          {
            this.LogDebug ( "RecordID: " + record.RecordId + " >> Initialised record." );
            continue;
          }
          this.LogDebug ( "RecordID: " + record.RecordId );
          this.LogDebug ( "Field Count: " + record.Fields.Count );

          //
          // Output the record left hand static fields.
          //
          this._ExportColumnRow [ 0 ] = record.RecordId;
          this._ExportColumnRow [ 1 ] = record.stRecordDate;

          //
          // Output the record fields
          //
          foreach ( EdRecordField recordField in record.Fields )
          {
            this.LogDebug ( "FieldId: " + recordField.FieldId );
            this.LogDebug ( "field Type: " + recordField.TypeId );
            this.LogDebug ( "Field Guid: " + recordField.Guid );
            this.LogDebug ( "ItemValue: " + recordField.ItemValue );
            this.LogDebug ( "ItemText: " + recordField.ItemText );

            if ( recordField.isReadOnly == true )
            {
              continue;
            }

            //
            // Export form field
            //
            if ( recordField.Guid == Guid.Empty )
            {
              continue;
            }

            //
            // select the field type to export.
            //
            switch ( recordField.TypeId )
            {
              case EvDataTypes.Special_Matrix:
              case EvDataTypes.Table:
                {
                  this.ExportTableData ( recordField );
                  break;
                }
              case EvDataTypes.Check_Box_List:
                {
                  this.ExportCheckBoxData ( recordField );
                  break;
                }
              default:
                {
                  this.getExportRecordFieldData (
                    recordField,
                    IncludeFreeTextData );
                  break;
                }
            }//END switch statement


          }//END field iteration loop.


          this.LogDebug ( "Comment.Count: " + record.CommentList.Count );

          String stComments = String.Empty;
          foreach ( EdFormRecordComment comment in record.CommentList )
          {
            stComments += comment.Content
              + " by "
              + comment.UserCommonName
              + " on "
              + comment.CommentDate.ToString ( "dd-MMM-yy HH:mm" ) + " ";
          }

          //
          // record footer information
          //
          this.exportColumnValue ( StaticHeaderColumns.Comments, stComments );

          //
          // Convert the output row into a CSV row.
          //
          stCsvData.AppendLine ( this.ConvertToCsv ( this._ExportColumnRow ) );

        }//END record list  iteration loop

        this.LogDebug ( "ExportFile length: " + stCsvData.Length );

        this.LogMethodEnd ( "createExportFile" );

        // 
        // Return the new record.
        // 
        return stCsvData.ToString ( );

      } //End Try
      catch ( Exception Ex )
      {
        this.EventCode = EvEventCodes.Data_Export_Exception_Event;
        this.LogException ( Ex );

      } // End catch.  

      this.LogMethodEnd ( "createExportFile" );
      return String.Empty;

    }//END createExportFile class

     // =====================================================================================
    /// <summary>
    /// This method iterates through the column header array to find a fields column index.
    /// </summary>
    /// <param name="HeaderFieldId">String: the export header field identifier</param>
    /// <returns>int: export array index value </returns>
    //  ----------------------------------------------------------------------------------
    public int exportColumnIndex ( String HeaderFieldId )
    {
      //
      // Iterate through the export column header array
      //
      for ( int i=0; i < this._ExportColumnHeader.Length; i++ )
      {
        //
        // if the export column header value matches the header field id 
        // return the export colum header index value.
        //
        if ( this._ExportColumnHeader [ i ] == HeaderFieldId )
        {
          return i;
        }
      }

      return -1;
    }

    // =====================================================================================
    /// <summary>
    /// This method iterates through the column header array to update the column value.
    /// </summary>
    /// <param name="HeaderFieldId">String: the export header field identifier</param>
    /// <param name="ColumnValue">String: the export column value</param>
    /// <returns>int: export array index value </returns>
    //  ----------------------------------------------------------------------------------
    public bool exportColumnValue ( object HeaderFieldId, String ColumnValue )
    {
      //
      // Iterate through the export column header array
      //
      for ( int i = 0; i < this._ExportColumnHeader.Length; i++ )
      {
        //
        // if the export column header value matches the header field id 
        // update the export column row cell value wiht the column value.
        //
        if ( this._ExportColumnHeader [ i ] == HeaderFieldId.ToString() )
        {
          this._ExportColumnRow [ i ] = ColumnValue;
          return true;
        }
      }

      return false;
    }

    //  ==================================================================================
    /// <summary>
    /// This method get the header form
    /// </summary>
    /// <param name="FormRecord">EvForm: a form object</param>
    /// <returns>EvForm: a form object</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Execute the method for retrieving the form ResultData object. 
    /// 
    /// 2. Return the form ResultData object. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private EdRecord getHeaderForm ( EdRecord FormRecord )
    {
      this.LogMethod ( "getHeaderForm method " );
      //
      // Initialise the mehtods parameters and variables.
      //
      EdRecord form = new EdRecord ( );

        this.LogDebug ( "Forms " );
        EdRecordLayouts forms = new EdRecordLayouts ( );

        form = forms.GetLayout ( FormRecord.LayoutId );
      return form;

    }//END getHeaderForm method
    #region Header methods

    //  ==================================================================================
    /// <summary>
    /// This method creates the export ResultData header.
    /// </summary>
    /// <param name="HeaderRecord">EvForm: a form object</param>
    /// <param name="ExportFreeText">Boolean: true, if the free text fields are exported</param>
    /// <returns>String: the export record header string</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Loop through the form object items. 
    /// 
    /// 2. Switch the formfield QueryType and append the formfield values to the header. 
    /// 
    /// 3. Return the header string. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private int createExportRecordHeader (
      EdRecord HeaderRecord,
      bool ExportFreeText )
    {
      this.LogMethod ( "createExportRecordHeader method " );

      this._OutputDataColumnCount = 7;

      //
      // Output the header static fields
      //
      System.Text.StringBuilder sbHeader = new System.Text.StringBuilder ( );

      sbHeader.Append ( ";" + StaticHeaderColumns.RecordId );
      sbHeader.Append ( ";" + StaticHeaderColumns.RecordDate );

      //
      // Output the form field header information
      //
      foreach ( EdRecordField formField in HeaderRecord.Fields )
      {
        this.LogDebug ( "FieldId: " + formField.FieldId + ", Type: " + formField.TypeId );

        if ( formField.isReadOnly == true )
        {
          continue;
        }
        //
        // Select the field QueryType to create the correct header.
        //
        switch ( formField.TypeId )
        {
          case EvDataTypes.Table:
          case EvDataTypes.Special_Matrix:
            {
              sbHeader.Append ( this.ExportTableHeader ( formField ) );

              break;
            }

          case EvDataTypes.Check_Box_List:
            {
              sbHeader.Append ( this.ExportCheckBoxHeader ( formField ) );

              break;
            }
          case EvDataTypes.Free_Text:
            {
              if ( ExportFreeText == true )
              {
                sbHeader.Append (  ";" + formField.FieldId  );
              }

              break;
            }

          default:
            {
              sbHeader.Append ( ";" +  formField.FieldId );

              break;
            }
        }//END switch

      }//END iteration loop

      //
      // Output the right hand static fields
      //
      sbHeader.Append ( ";" + StaticHeaderColumns.Comments );
      sbHeader.Append ( ";" + StaticHeaderColumns.AuthoredBy );
      sbHeader.Append ( ";" + StaticHeaderColumns.AuthoredDate );
      sbHeader.Append ( ";" + StaticHeaderColumns.ReviewedBy );
      sbHeader.Append ( ";" + StaticHeaderColumns.ReviewedDate );
      sbHeader.Append ( ";" + StaticHeaderColumns.MonitoredBy );
      sbHeader.Append ( ";" + StaticHeaderColumns.MonitorDate );
      sbHeader.Append ( ";" + StaticHeaderColumns.LockedBy );
      sbHeader.Append ( ";" + StaticHeaderColumns.LockedDate );

      this._ExportColumnHeader = sbHeader.ToString ( ).Split ( ';' );

      this.LogMethodEnd ( "createExportRecordHeader" );

      return this._ExportColumnHeader.Length;

    }//END getExportDataHeader method.

    //  ==================================================================================
    /// <summary>
    /// This class exports the checkbox header. 
    /// </summary>
    /// <param name="FormField">EvFormField: a form field object</param>
    /// <returns>String: a checkbox header string</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Loop through the formfield option list and add the formfield values to the output string
    /// 
    /// 2. Return the output string. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private String ExportCheckBoxHeader ( EdRecordField FormField )
    {
      //
      // Output the header static fields
      //
      System.Text.StringBuilder sOutput = new System.Text.StringBuilder ( );

      //
      // Output the form field header information
      //
      foreach ( EvOption option in FormField.Design.OptionList )
      {
        sOutput.Append ( ";" + FormField.FieldId + "-" + option.Value.Replace ( ",", "" ) );
      }

      return sOutput.ToString ( );

    }//END getExportCheckBoxHeader method.

    //  ==================================================================================
    /// <summary>
    /// This class exports the table header 
    /// </summary>
    /// <param name="FormField">EvFormField: a form field object</param>
    /// <returns>String: a table header string</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Loop through the formfield object table's rows and columns
    /// 
    /// 2. add the formfield values to the table header string. 
    /// 
    /// 3. Return the table header string. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private string ExportTableHeader ( EdRecordField FormField )
    {
      System.Text.StringBuilder sOutput = new System.Text.StringBuilder ( );
      //
      // Output the form field header information
      //
      for ( int row = 0; row < FormField.Table.Rows.Count; row++ )
      {
        for ( int col = 0; col < FormField.Table.Header.Length; col++ )
        {
          if ( FormField.Table.Header [ col ].Text == String.Empty )
          {
            continue;
          }
            sOutput.Append ( ";" + FormField.FieldId + " R:" + row + " C:" + col );

        }//END column iteration loop

      }//END row iteration

      return sOutput.ToString();

    }//END ExportTableHeader method.

    #endregion

    #region DataRows Export methods

    //  ==================================================================================
    /// <summary>
    /// This class exports table spacer
    /// </summary>
    /// <param name="Data">String array: data values </param>
    /// <returns>String: a table spacer string</returns>
    //  ----------------------------------------------------------------------------------
    private String ConvertToCsv ( String [ ] Data )
    {
      this.LogMethod ( "ConvertToCsv method " );
      //
      // Initialise the methods variables and objects.
      //
      System.Text.StringBuilder csvData = new System.Text.StringBuilder ( );
      float fltValue;

      //
      // Iterate through the string array outputting the values as CSV entries.
      //
      for ( int i = 0; i < Data.Length; i++ )
      {
        string cell = Data [ i ];
        //
        // Handle first cell.
        //
        if ( i == 0 )
        {
          if ( cell == null )
          {
            csvData.Append ( "\"\"" );
            continue;
          }
          csvData.Append ( Evado.Model.Digital.EvcStatics.encodeCsvFirstColumn ( cell ) );
          continue;
        }

        //
        // Empty cells will be null ie. not data.
        //
        if ( cell == null )
        {
          csvData.Append ( Evado.Model.Digital.EvcStatics.encodeCsvEmpty );
          continue;
        }

        if ( float.TryParse ( cell, out fltValue ) == true
          || cell.ToLower ( ) == "na" )
        {
          csvData.Append ( Evado.Model.Digital.EvcStatics.encodeCsvNumeric ( cell ) );
        }
        else
        {
          csvData.Append ( Evado.Model.Digital.EvcStatics.encodeCsvText ( cell ) );
        }
      }//END cell interaion loop.

      this.LogMethodEnd ( "ConvertToCsv" );

      return csvData.ToString ( );
    }

    //  ==================================================================================
    /// <summary>
    /// This class exports table spacer
    /// </summary>
    /// <param name="FormField">EvFormField: a formfield object</param>
    /// <returns>String: a table spacer string</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Loop through the formfield object table rows and columns
    /// 
    /// 2. Add the "," character to the table header string. 
    /// 
    /// 3. Return the table header string. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private String ExportTableSpacer ( EdRecordField FormField )
    {
      String stTableHeader = String.Empty;
      //
      // Output the form field header information
      //
      for ( int row = 0; row < FormField.Table.Rows.Count; row++ )
      {
        for ( int col = 0; col < FormField.Table.Header.Length; col++ )
        {
          if ( FormField.Table.Header [ col ].Text != String.Empty )
          {
            stTableHeader += Evado.Model.Digital.EvcStatics.encodeCsvEmpty;
          }

        }//END column iteration loop

      }//END row iteration

      return stTableHeader;

    }//END ExportTableSpacer method.

    //  ==================================================================================
    /// <summary>
    /// This class generates the export ResultData output.
    /// </summary>
    /// <param name="FormField">EvFormField: a formfield object</param>
    /// <param name="ExportFreeText">Boolean: true, if the free text fields are exported</param>
    /// <returns>string: an export record field ResultData string</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Switch the header field object's QueryType and export the associated ResultData. 
    /// 
    /// 2. Return the export record field ResultData string, if the types that have values. 
    /// 
    /// 3. Else, return empty string.
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private bool getExportRecordFieldData ( EdRecordField FormField,
      bool ExportFreeText )
    {
      //
      // Select the field QueryType to export.
      //
      switch ( FormField.TypeId )
      {
        case EvDataTypes.Numeric:
          {
            this.exportColumnValue ( FormField.FieldId, FormField.ItemValue );
            return true;
          }

        case EvDataTypes.Table:
        case EvDataTypes.Special_Matrix:
          {
            this.ExportTableData ( FormField );
            return true;
          }

        case EvDataTypes.Check_Box_List:
          {
            this.ExportCheckBoxData ( FormField );
            return true;
          }

        case EvDataTypes.Boolean:
          {
            string value = EvStatics.encodeCsvBoolean ( FormField.ItemValue );
            this.exportColumnValue ( FormField.FieldId, value );
            return true;
          }
        case EvDataTypes.Free_Text:
          {
            if ( ExportFreeText == true )
            {
            this.exportColumnValue ( FormField.FieldId, FormField.ItemValue );
            }
            return true;
          }

        default:
          {
            this.exportColumnValue ( FormField.FieldId, FormField.ItemValue );
            return true;
          }
      }//END Switch

    }//ENd getExportData

    #endregion
    //  ==================================================================================
    /// <summary>
    /// This method creates the export checkbox ResultData header.
    /// </summary>
    /// <param name="FormField">EvFormField: a formfield object</param>
    /// <returns>String: a checkbox export string</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Loop through the headerfield object optionlist. 
    /// 
    /// 2. Append the compatible ",1" or ",0" character to the output string. 
    /// 
    /// 3. Return the output string. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private void ExportCheckBoxData (
      EdRecordField FormField )
    {
      this.LogMethod ( "ExportCheckBoxData method. " );
      //
      // Define the string builder
      //
      string [ ] values = FormField.ItemValue.Split ( ';' );
      this.LogDebug ( "ItemValue: " + FormField.ItemValue );

      if ( FormField.ItemValue == String.Empty )
      {
        this.LogDebug ( "EXIT empty value" );
        return;
      }

      //
      // Output the form field header information
      //
      foreach ( EvOption option in FormField.Design.OptionList )
      {
        //
        // iterate through the value array.
        //
        foreach ( string value in values )
        {
          if ( option.Value != value )
          {
            continue;
          }

          string exportFieldId = FormField.FieldId + "-" + option.Value.Replace ( ",", "" );

          this.exportColumnValue ( exportFieldId, "1" );
        }
      }//END option list interation loop.

      this.LogMethodEnd ( "ExportCheckBoxData" );
    }//END ExportCheckBoxData method.

    //  ==================================================================================
    /// <summary>
    /// This method creates the export table ResultData header.
    /// </summary>
    /// <param name="FormField">EvFormField: a formfield object</param>
    /// <returns>String: a checkbox export string</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Loop through the headerfield object table's rows. 
    /// 
    /// 2. if the headerfield row is greater than or equal to formfield's row length, 
    /// loop through columns and append the compatible "," character to the output string. 
    /// 
    /// 3. Else, loop through the column and 
    /// 
    /// 4. switch the column QueryType and append the formfield object's column value to the output string.  
    /// 
    /// 5. Return the output string. 
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private void ExportTableData ( 
      EdRecordField FormField )
    {
      this.LogMethod ( "ExportTableData method. " );
      this.LogDebug ( "record FieldId: " + FormField.FieldId );
      this.LogDebug ( "table Rows: " + FormField.Table.Rows.Count );

      //
      // Output the form field header information
      //
      for ( int row = 0; row < FormField.Table.Rows.Count; row++ )
      {
        this.LogDebug ( "Row: " + row );

        //
        // iterate through the columns inserting null values.
        //
        for ( int col = 0; col < FormField.Table.Header.Length; col++ )
        {
        this.LogDebug ( "Col: " + col );
          //
          // Skip empty columns
          //
          if ( FormField.Table.Header [ col ].Text == String.Empty )
          {
            continue;
          }
            string tableFieldId = FormField.FieldId + " R:" + row + " C:" + col;

            string value = FormField.Table.Rows [ row ].Column [ col ];

            this.exportColumnValue ( tableFieldId, value );

        }//END if column iteration

      }//END row iteration

      this.LogMethodEnd ( "ExportTableData " );
    }//END ExportTableData method

    #endregion

  }//END EvFormRecordExport Class.

}//END namespace Evado.Evado.BLL 
