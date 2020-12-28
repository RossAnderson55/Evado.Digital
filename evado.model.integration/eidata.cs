using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evado.Model.Integration
{
  /// <summary>
  /// This model class defines the Web Service Query structure.
  /// </summary>
  [Serializable]
  public class EiData
  {
    #region Initialisation methods

    //==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    //-----------------------------------------------------------------------------------
    public EiData ( )
    {
    }

    //==================================================================================
    /// <summary>
    /// This method initialises the clas and sets the project identifier.
    /// </summary>
    /// <param name="ProjectId">String: project identifier</param>
    //-----------------------------------------------------------------------------------
    public EiData ( String ProjectId )
    {
      this.AddQueryParameter ( EiQueryParameterNames.Project_Id, ProjectId );

    }

    //==================================================================================
    /// <summary>
    /// This method initialises the clas and sets the project identifier.
    /// </summary>
    /// <param name="ProjectId">String: project identifier</param>
    /// <param name="QueryType">EiQueryTypes: Query Type</param>
    //-----------------------------------------------------------------------------------
    public EiData ( String ProjectId, EiQueryTypes QueryType )
    {
      this.AddQueryParameter ( EiQueryParameterNames.Project_Id, ProjectId );
      this.QueryType = QueryType;
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class properties and constants

    /// <summary>
    /// This property contains the QueryType of query that is to be executed.
    /// 
    /// The query options are defeins the EvQueryTypes
    /// </summary>
    public EiQueryTypes QueryType { get; set; }

    /// <summary>
    /// This property contains the list of query parameters that are to be query the Evado database.
    /// </summary>
    public List<EiQueryParameter> ParameterList { get; set; }

    /// <summary>
    /// This property contain a list of the data column parameters.
    /// </summary>
    public List<EiColumnParameters> Columns { get; set; }

    /// <summary>
    /// This property contain a list of the rows of data.
    /// </summary>
    public List<EiDataRow> DataRows { get; set; }

    
    /// <summary>
    /// This property contains the user account authorised to execute this query.
    /// </summary>
    public String UserAccount { get; set; }
    

    /// <summary>
    /// This property contains the user account password.
    /// </summary>
    //public String UserPassword { get; set; }

    Evado.Model.Integration.EiEventCodes _EventCode = Model.Integration.EiEventCodes.Ok;
    /// <summary>
    /// This property defines the returned Event code from processing the query or import data.
    /// </summary>
    public Evado.Model.Integration.EiEventCodes EventCode
    {
      set
      {
        this._EventCode = value;
      }
      get
      {
        return this._EventCode;
      }
    }

    /// <summary>
    /// This property contains the error message for data structure
    /// </summary>
    public String ErrorMessage { get; set; }
    /// <summary>
    /// This property contains the process log for data structure
    /// </summary>
    public String ProcessLog { get; set; }

    /// <summary>
    /// This constant defines the string null value. 
    /// </summary>>
    public const String CONST_CUSTOMER_ID = "CUSTOMER_ID:";
    /// <summary>
    /// This constant defines the Query Type CSV row identifier
    /// </summary>
    public const String CONST_QUERY_TYPE = "QueryType:";
    /// <summary>
    /// This constant defines the parameter CSV row identifier
    /// </summary>
    public const String CONST_PARAMETER = "Parameter:";
    /// <summary>
    /// This constant defines the column field CSV row identifier
    /// </summary>
    public const String CONST_COLUMN_FIELD_ID = "ColumnEvdoFieldId:";
    /// <summary>
    /// This constant defines the column date type CSV row identifier
    /// </summary>
    public const String CONST_COLUMN_DATA_TYPE = "ColumnDataType:";
    /// <summary>
    /// This constant defines the column name CSV row identifier
    /// </summary>
    public const String CONST_COLUMN_NAME = "ColumnName:";
    /// <summary>
    /// This constant defines the column index CSV row identifier
    /// </summary>
    public const String CONST_COLUMN_INDEX = "ColumnIndex:";
    /// <summary>
    /// This constant defines the column metadata CSV row identifier
    /// </summary>
    public const String CONST_COLUMN_METADATA = "ColumnMetaData:";
    /// <summary>
    /// This constant defines the data row CSV row identifier
    /// </summary>
    public const String CONST_DATA_ROW = "RowData:";
    /// <summary>
    /// This constant defines the comumn name CSV row identifier
    /// </summary>
    public const String CONST_EVENT_CODE = "EventCode:";

    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class Methods
    //==================================================================================
    /// <summary>
    /// This method test to determine if het parameter is in the parameter list.
    /// </summary>
    /// <param name="Name">EiQueryParameterNames enumeration</param>
    //-----------------------------------------------------------------------------------
    public bool hasQueryParameter (
      EiQueryParameterNames Name )
    {
      //
      // If ParameterList is null initialise it.
      //
      if ( this.ParameterList == null )
      {
        this.ParameterList = new List<EiQueryParameter> ( );

        return false;
      }

      //
      // Iterate through the parameters and update the matching parameter value.
      //
      foreach ( EiQueryParameter parameter in this.ParameterList )
      {
        if ( parameter.Name == Name )
        {
          return true;
        }
      }

      return false;

    }//END hasQueryParameter method

    //==================================================================================
    /// <summary>
    /// This method adds a query parameter value to the query parameter list
    /// </summary>
    /// <param name="Name">EiQueryParameterNames enumeration</param>
    /// <param name="Value">String Value</param>
    //-----------------------------------------------------------------------------------
    public void AddQueryParameter (
      EiQueryParameterNames Name,
      String Value )
    {
      //
      // If ParameterList is null initialise it.
      //
      if ( this.ParameterList == null )
      {
        this.ParameterList = new List<EiQueryParameter> ( );
      }

      //
      // Iterate through the parameters and update the matching parameter value.
      //
      foreach ( EiQueryParameter parameter in this.ParameterList )
      {
        if ( parameter.Name.ToString ( ) == Name.ToString ( ) )
        {
          parameter.Value = Value;

          return;
        }
      }

      //
      // Append the parameter to the parameter list.
      //
      this.ParameterList.Add ( new EiQueryParameter ( Name, Value ) );
    }

    //==================================================================================
    /// <summary>
    /// This method deletes the query parameter from the parameter list.
    /// </summary>
    /// <param name="Name">EiQueryParameterNames enumeration</param>
    //-----------------------------------------------------------------------------------
    public void DeleteQueryParameter (
      EiQueryParameterNames Name )
    {
      //
      // If ParameterList is null initialise it.
      //
      if ( this.ParameterList == null )
      {
        return;
      }

      //
      // Iterate through the parameters and update the matching parameter value.
      //
      for ( int count = 0; count < this.ParameterList.Count; count++ )
      {
        EiQueryParameter parameter = this.ParameterList [ count ];

        if ( parameter.Name.ToString ( ) == Name.ToString ( ) )
        {
          ParameterList.RemoveAt ( count );
          count--;
        }
      }
    }

    //==================================================================================
    /// <summary>
    /// This method adds a query parameter value to the query parameter list
    /// </summary>
    /// <param name="Name">EiQueryParameterNames enumeration</param>
    //-----------------------------------------------------------------------------------
    public String GetQueryParameterValue (
      EiQueryParameterNames Name )
    {
      //
      // If ParameterList is null initialise it.
      //
      if ( this.ParameterList == null )
      {
        this.ParameterList = new List<EiQueryParameter> ( );
      }

      //
      // Iterate through the list looking for the parameter.
      //
      foreach ( EiQueryParameter parameter in this.ParameterList )
      {
        if ( parameter.Name == Name )
        {
          return parameter.Value;
        }
      }//END iteration loop

      return String.Empty;
    }

    //=======================================================
    /// <summary>
    /// This method adds a query parameter value to the query parameter list
    /// </summary>
    /// <returns>EiDataRow</returns>
    //-----------------------------------------------------------------------------------
    public bool hasColumn ( string EvadoFieldId )
    {
      //
      // If DataRows is null initialise it.
      //
      if ( this.Columns == null )
      {
        return false;
      }
      String evadoFieldId = EvadoFieldId;
      //
      // iterate through columns looking for the name value.
      //
      foreach ( EiColumnParameters column in this.Columns )
      {
        if ( column.EvadoFieldId.ToLower ( ) == evadoFieldId.ToLower ( ) )
        {
          return true;
        }
      }

      //
      // return the data row object.
      //
      return false;

    }//END hasColumn method

    //=======================================================
    /// <summary>
    /// This method adds a query parameter value to the query parameter list
    /// </summary>
    /// <returns>EiDataRow</returns>
    //-----------------------------------------------------------------------------------
    public bool hasColumn ( object EvadoFieldId )
    {
      //
      // If DataRows is null initialise it.
      //
      if ( this.Columns == null )
      {
        return false;
      }
      String evadoFieldId = EvadoFieldId.ToString ( );
      //
      // iterate through columns looking for the name value.
      //
      foreach ( EiColumnParameters column in this.Columns )
      {
        if ( column.EvadoFieldId.ToLower ( ) == evadoFieldId.ToLower ( ) )
        {
          return true;
        }
      }

      //
      // return the data row object.
      //
      return false;

    }//END hasColumn method

    //=======================================================
    /// <summary>
    /// This method adds a query parameter value to the query parameter list
    /// </summary>
    /// <returns>EiDataRow</returns>
    //-----------------------------------------------------------------------------------
    public int getColumnNo ( string EvadoFieldId )
    {
      //
      // If DataRows is null initialise it.
      //
      if ( this.Columns == null )
      {
        return -1;
      }

      //
      // iterate through columns looking for the name value.
      //
      for ( int count = 0; count < this.Columns.Count; count++ )
      {
        if ( this.Columns [ count ].EvadoFieldId == EvadoFieldId )
        {
          return count;
        }
      }

      //
      // return the data row object.
      //
      return -1;

    }//END hasColumn method

    //=======================================================
    /// <summary>
    /// This method adds a query parameter value to the query parameter list
    /// </summary>
    /// <returns>EiDataRow</returns>
    //-----------------------------------------------------------------------------------
    public int getColumnNo ( object EvadoFieldId )
    {
      //
      // If DataRows is null initialise it.
      //
      if ( this.Columns == null )
      {
        return -1;
      }
      string fieldId = EvadoFieldId.ToString ( );
      //
      // iterate through columns looking for the name value.
      //
      for ( int count = 0; count < this.Columns.Count; count++ )
      {
        if ( this.Columns [ count ].EvadoFieldId == fieldId )
        {
          return count;
        }
      }

      //
      // return the data row object.
      //
      return -1;

    }//END hasColumn method

    //=======================================================
    /// <summary>
    /// This method adds a query parameter value to the query parameter list
    /// </summary>
    /// <returns>EiDataRow</returns>
    //-----------------------------------------------------------------------------------
    public EiColumnParameters AddColumn ( )
    {
      //
      // If DataRows is null initialise it.
      //
      if ( this.Columns == null )
      {
        this.Columns = new List<EiColumnParameters> ( );
      }

      //
      // Create the data row
      //
      EiColumnParameters column = new EiColumnParameters ( );

      //
      // Append the data row to the DataRows list.
      //
      this.Columns.Add ( column );

      //
      // return the data row object.
      //
      return column;
    }

    //==================================================================================
    /// <summary>
    /// This method adds a query parameter value to the query parameter list
    /// </summary>
    /// <param name="Parameters">EiDataRow object</param>
    /// <returns>EiDataRow</returns>
    //-----------------------------------------------------------------------------------
    public void AddColumn ( EiColumnParameters Parameters )
    {
      this.Columns.Add ( Parameters );
    }

    //==================================================================================
    /// <summary>
    /// This method adds a query parameter value to the query parameter list
    /// </summary>
    /// <param name="Datatype"> Date Type enumerated vlaue</param>
    /// <param name="EvadoFieldId">object Evado Field identifier</param>
    /// <returns>EiDataRow</returns>
    //-----------------------------------------------------------------------------------
    public EiColumnParameters AddColumn (
      EiDataTypes Datatype,
      object EvadoFieldId )
    {
      EiColumnParameters parameters = new EiColumnParameters (
        Datatype,
        EvadoFieldId,
        false );

      this.Columns.Add ( parameters );

      return parameters;
    }

    //==================================================================================
    /// <summary>
    /// This method adds a query parameter value to the query parameter list
    /// </summary>
    /// <param name="Datatype"> Date Type enumerated vlaue</param>
    /// <param name="EvadoFieldId">object Evado Field identifier</param>
    /// <param name="Index">Bool: True column is the index</param>
    /// <returns>EiDataRow</returns>
    //-----------------------------------------------------------------------------------
    public EiColumnParameters AddColumn (
      EiDataTypes Datatype,
      object EvadoFieldId,
      bool Index )
    {
      EiColumnParameters parameters = new EiColumnParameters (
        Datatype,
        EvadoFieldId,
        Index );

      this.Columns.Add ( parameters );

      return parameters;
    }

    //==================================================================================
    /// <summary>
    /// This method adds a query parameter value to the query parameter list
    /// </summary>
    /// <param name="Datatype"> Date Type enumerated vlaue</param>
    /// <param name="EvadoFieldId">object Evado Field identifier</param>
    /// <param name="Index">Bool: True column is the index</param>
    /// <returns>EiDataRow</returns>
    //-----------------------------------------------------------------------------------
    public EiColumnParameters AddColumn (
      EiDataTypes Datatype,
      String EvadoFieldId,
      bool Index )
    {
      EiColumnParameters parameters = new EiColumnParameters (
        Datatype,
        EvadoFieldId,
        Index );

      this.Columns.Add ( parameters );

      return parameters;
    }

    //=======================================================
    /// <summary>
    /// This method adds a query parameter value to the query parameter list
    /// </summary>
    /// <returns>EiDataRow</returns>
    //-----------------------------------------------------------------------------------
    public EiDataRow AddDataRow ( )
    {
      //
      // If columns is null exit.
      //
      if ( this.Columns == null )
      {
        return null;
      }

      //
      // If DataRows is null initialise it.
      //
      if ( this.DataRows == null )
      {
        this.DataRows = new List<EiDataRow> ( );
      }

      //
      // Create the data row
      //
      EiDataRow dataRow = new EiDataRow ( this.Columns.Count );

      //
      // Append the data row to the DataRows list.
      //
      this.DataRows.Add ( dataRow );

      //
      // return the data row object.
      //
      return dataRow;
    }

    //==================================================================================
    /// <summary>
    /// This method adds a query parameter value to the query parameter list
    /// </summary>
    /// <param name="DataRow">EiDataRow object</param>
    /// <returns>EiDataRow</returns>
    //-----------------------------------------------------------------------------------
    public void AddDataRow ( EiDataRow DataRow )
    {
      this.DataRows.Add ( DataRow );
    }

    //===================================================================================
    /// <summary>
    /// Output the Data object as a CSV object.
    /// </summary>
    /// <returns>String: CSV format data file.</returns>
    //-----------------------------------------------------------------------------------
    public String getCsvOutput ( )
    {
      //
      // Initialise the methods variables and objects.
      //
      StringBuilder csvContent = new StringBuilder ( );
      StringBuilder columnField = new StringBuilder ( );
      StringBuilder columnDataType = new StringBuilder ( );
      StringBuilder columnName = new StringBuilder ( );
      StringBuilder columnIndex = new StringBuilder ( );
      StringBuilder columnMetaData = new StringBuilder ( );
      String value = String.Empty;

      //
      // Add the data type as the first row of the output.
      //
      csvContent.AppendLine ( "\"" + CONST_QUERY_TYPE + "\",\"" + this.QueryType + "\"" );

      if ( this.ParameterList != null )
      {
        //
        // Define the header row for the query parameter values.
        //
        csvContent.AppendLine ( "\"" + CONST_PARAMETER + "\",\"Name\",\"Value\"" );

        //
        // output the query parameter values.
        //
        foreach ( Evado.Model.Integration.EiQueryParameter parameter in this.ParameterList )
        {
          csvContent.AppendLine ( "\"" + CONST_PARAMETER + "\",\"" + parameter.Name + "\",\"" + parameter.Value + "\"" );
        }
      }

      //
      // Iterate through the column header outputing the header value for each column.
      //
      if ( this.Columns != null )
      {
        foreach ( Evado.Model.Integration.EiColumnParameters parameter in this.Columns )
        {
          value = String.Empty;
          if ( parameter.EvadoFieldId != null )
          {
            value = parameter.EvadoFieldId;
          }
          columnField.Append ( ",\"" + value + "\"" );

          value = String.Empty;
          if ( parameter.DataType != EiDataTypes.Null )
          {
            value = parameter.DataType.ToString ( );
          }
          columnDataType.Append ( ",\"" + value + "\"" );

          value = String.Empty;
          if ( parameter.Name != null )
          {
            value = parameter.Name;
          }
          columnName.Append ( ",\"" + value + "\"" );

          columnIndex.Append ( ",\"" + parameter.Index.ToString ( ) + "\"" );

          value = String.Empty;
          if ( parameter.MetaData != null )
          {
            value = parameter.MetaData;
          }
          columnMetaData.Append ( ",\"" + value + "\"" );

        }//END columns iteration loop

      }//END Columns exist.

      //
      // Append the column headers in the correct order.
      //
      if ( columnField.Length > 0 )
      {
        csvContent.Append ( "\"" + CONST_COLUMN_FIELD_ID + "\"," );
        csvContent.AppendLine ( columnField.ToString ( ) );
      }
      if ( columnField.Length > 0 )
      {
        csvContent.Append ( "\"" + CONST_COLUMN_DATA_TYPE + "\"" );
        csvContent.AppendLine ( columnDataType.ToString ( ) );
      }
      if ( columnField.Length > 0 )
      {

        csvContent.Append ( "\"" + CONST_COLUMN_NAME + "\"" );
        csvContent.AppendLine ( columnName.ToString ( ) );
      }
      if ( columnField.Length > 0 )
      {
        csvContent.Append ( "\"" + CONST_COLUMN_INDEX + "\"" );
        csvContent.AppendLine ( columnIndex.ToString ( ) );
      }
      if ( columnField.Length > 0 )
      {
        csvContent.Append ( "\"" + CONST_COLUMN_METADATA + "\"" );
        csvContent.AppendLine ( columnMetaData.ToString ( ) );
      }

      //
      // Iterate through each row in the data row list.
      //
      if ( this.DataRows != null )
      {
        foreach ( Evado.Model.Integration.EiDataRow row in this.DataRows )
        {
          //
          // Output the row column 1 value.
          //
          csvContent.Append ( "\"" + CONST_DATA_ROW + "\"" );

          //
          // Iterate through the row string array outputting each value in CSv format.
          //
          foreach ( String value1 in row.Values )
          {
            csvContent.Append ( ",\"" + value1 + "\"" );
          }
          csvContent.AppendLine ( "" );
        }
      }

      //
      // Add the data type as the first row of the output.
      //
      csvContent.AppendLine ( "\"" + EiData.CONST_EVENT_CODE + "\",\"" + this.EventCode + "\"" );

      //
      // Return the csv content.
      //
      return csvContent.ToString ( );
    }

    //===================================================================================
    /// <summary>
    /// Output the Data object as a CSV object.
    /// </summary>
    /// <returns>String: CSV format data file.</returns>
    //-----------------------------------------------------------------------------------
    public String getAsString ( )
    {
      //
      // Initialise the methods variables and objects.
      //
      StringBuilder csvContent = new StringBuilder ( );
      StringBuilder columnField = new StringBuilder ( );
      StringBuilder columnDataType = new StringBuilder ( );
      StringBuilder columnName = new StringBuilder ( );
      StringBuilder columnIndex = new StringBuilder ( );
      StringBuilder columnMetaData = new StringBuilder ( );

      //
      // Add the data type .
      //
      csvContent.AppendLine ( "Query Type: " + this.QueryType );

      //
      // Add the data project.
      //
      csvContent.AppendLine ( "Customer: ," + this.GetQueryParameterValue ( EiQueryParameterNames.Customer_Id ) );

      //
      // Add the data project.
      //
      csvContent.AppendLine ( "ProjectId: " + this.GetQueryParameterValue ( EiQueryParameterNames.Project_Id ) );

      if ( this.ParameterList != null )
      {

        //
        // Define the header row for the query parameter values.
        //
        csvContent.AppendLine ( "Name,Value" );

        //
        // output the query parameter values.
        //
        foreach ( Evado.Model.Integration.EiQueryParameter parameter in this.ParameterList )
        {
          csvContent.AppendLine ( parameter.Name + "," + parameter.Value );
        }
      }

      //
      // Iterate through the column header outputing the header value for each column.
      //
      if ( this.Columns != null )
      {
        foreach ( Evado.Model.Integration.EiColumnParameters parameter in this.Columns )
        {
          if ( parameter.EvadoFieldId != null )
          {
            columnField.Append ( parameter.EvadoFieldId + "," );
          }
          if ( parameter.DataType != EiDataTypes.Null )
          {
            columnDataType.Append ( parameter.DataType + "," );
          }
          if ( parameter.Name != null )
          {
            columnName.Append ( parameter.Name + "," );
          }

          columnIndex.Append ( parameter.Index.ToString ( ) + "," );

          if ( parameter.MetaData != null )
          {
            columnMetaData.Append ( parameter.MetaData + "," );
          }
        }//END columns iteration loop

        csvContent.AppendLine ( columnField.ToString ( ) );
        csvContent.AppendLine ( columnDataType.ToString ( ) );
        csvContent.AppendLine ( columnIndex.ToString ( ) );
        csvContent.AppendLine ( columnMetaData.ToString ( ) );
      }
      //
      // Iterate through the data rows outputing the data values.
      //
      if ( this.DataRows != null )
      {
        foreach ( Evado.Model.Integration.EiDataRow daraRow in this.DataRows )
        {
          for ( int column = 0; column < daraRow.Values.Length; column++ )
          {
            if ( column > 0 )
            {
              csvContent.Append ( "," );
            }
            csvContent.Append ( daraRow.Values [ column ] );
          }
          csvContent.AppendLine ( "" );
        }//END columns iteration loop
      }

      //
      // Return the csv content.
      //
      return csvContent.ToString ( );
    }

    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

  }//END EvWebServiceQuery

}//END Namespace Evado.Model.Integration
