/***************************************************************************************
 * <copyright file="model\EvReportQuery.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2021 EVADO HOLDING PTY. LTD..  All rights reserved.
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
 *  This class contains the EvReportQuery data object.
 *
 ****************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

namespace Evado.Model.Digital
{
  /// <summary>
  /// data  entity used to model accounts
  /// </summary>
  [Serializable]
  public class EvReportQuery
  {
    #region Class initialisation method
    /// <summary>
    /// This method initialises the report quety object.
    /// </summary>
    public EvReportQuery ( )
    {
      SelectionList [ 0 ] = new EvOption ( );
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class members

    private String _QueryId = String.Empty;

    private String _QueryTitle = String.Empty;

    /// <summary>
    /// This member defines the selection data source that is used to fill the selection
    /// list.
    /// 
    /// </summary>
    private int _Index = 0;

    /// <summary>
    /// This member defines the selection data source that is used to fill the selection
    /// list.
    /// 
    /// </summary>
    private EvReport.SelectionListTypes _SelectionSource = EvReport.SelectionListTypes.None;

    /// <summary>
    /// This member contains the selection list containing the selection options for the 
    /// query.
    /// 
    /// </summary>
    private EvOption [ ] _SelectionList = new EvOption [ 1 ];

    /// <summary>
    /// This member defines the FieldName of the value to be queried. It could be 
    /// an Sql Column, or a data object member.
    /// 
    /// </summary>
    private string _FieldName = String.Empty;

    /// <summary>
    /// This member contains the value to be used to query the report.
    /// 
    /// </summary>
    private string _Value = String.Empty;

    /// <summary>
    /// This member contains the value name of the query used a selection list. 
    /// As the value may be index.
    /// 
    /// </summary>
    private string _ValueName = String.Empty;

    /// <summary>
    /// This member contains the query prompt that is displayed to the user.
    /// 
    /// </summary>
    private string _Prompt = String.Empty;

    /// <summary>
    /// Operator that will be used by the query to compare.
    /// </summary>
    private EvReport.Operators _Operator = EvReport.Operators.Equals_to;

    /// <summary>
    /// THis member defines whether the quey selection is mandatory,  if mandatory 
    /// then the query option must have a valid value for the report to be executed (generated).
    /// 
    /// </summary>
    private bool _Mandatory = false;

    /// <summary>
    /// This member defines the data type of the query and is used to determine 
    /// some validations on the queries.
    /// 
    /// </summary>
    private EvReport.DataTypes _DataType = EvReport.DataTypes.Text;

    /// <summary>
    /// List of static parameters used to filter the query list.
    /// The parameters are separated by :
    /// </summary>
    private String queryParameters = String.Empty;

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class properties


    /// <summary>
    /// This property contains the source's query identifier.
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public String QueryId
    {
      get { return _QueryId; }
      set { _QueryId = value; }
    }

    /// <summary>
    /// This property contains teh source's query title.
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public String QueryTitle
    {
      get { return _QueryTitle; }
      set { _QueryTitle = value; }
    }

    /// <summary>
    /// This property contains an operator object of the report query
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public EvReport.Operators Operator
    {
      get { return _Operator; }
      set { _Operator = value; }
    }

    /// <summary>
    /// This property contains an index number of the report query
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public int Index
    {
      get
      {
        return this._Index;
      }
      set
      {
        this._Index = value;
      }
    } //end _SelectionSource

    /// <summary>
    /// This property contains a selection source object of the report query
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public EvReport.SelectionListTypes SelectionSource
    {
      get
      {
        return this._SelectionSource;
      }
      set
      {
        this._SelectionSource = value;
      }
    } //end _SelectionSource

    /// <summary>
    /// This property contains a selection list array object of the report query
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public EvOption [ ] SelectionList
    {
      get
      {
        return this._SelectionList;
      }
      set
      {
        this._SelectionList = value;
      }
    } //end _SelectionList

    /// <summary>
    /// This property contains a field name of the report query
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public string FieldName
    {
      get
      {
        return this._FieldName;
      }
      set
      {
        this._FieldName = value;
      }
    } //end _FieldName

    /// <summary>
    /// This property contains value of the report query
    /// </summary>
    public string Value
    {
      get
      {
        return this._Value;
      }
      set
      {
        this._Value = value;
      }
    } //end _Value

    /// <summary>
    /// This property contains value name of the report query
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public string ValueName
    {
      get
      {
        return this._ValueName;
      }
      set
      {
        this._ValueName = value;
      }
    } //end _ValueName

    /// <summary>
    /// This property contains a prompt of the report query
    /// </summary>
    public string Prompt
    {
      get
      {
        return this._Prompt;
      }
      set
      {
        this._Prompt = value;
      }
    } //end _Prompt

    /// <summary>
    /// This property contains of the report query
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public bool Mandatory
    {
      get
      {
        return this._Mandatory;
      }
      set
      {
        this._Mandatory = value;
      }
    } //end _Mandatory

    /// <summary>
    /// This property contains a data type object of the report query
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public EvReport.DataTypes DataType
    {
      get { return _DataType; }
      set { _DataType = value; }
    }


    /// <summary>
    /// This property contains a data type object of the report query
    /// </summary>
    [Newtonsoft.Json.JsonProperty ( "Type" )]
    public String stDataType
    {
      get { return _DataType.ToString(); }
    }

    /// <summary>
    /// This property contains query parameters of the report query
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public String QueryParameters
    {
      get { return queryParameters; }
      set { queryParameters = value; }
    }

    #endregion

    #region publicMethods
    /// <summary>
    /// This method generates a repor a string text
    /// </summary>
    /// <returns>String output from teh report query.</returns>
    public string getAsString ( )
    {
      String output = String.Empty;

      output += "QId: " + this._QueryId;
      output += ", QT: " + this._QueryTitle;
      output += ", P: " + this._Prompt;
      output += ", M: " + this._Mandatory;
      output += ", FN: " + this._FieldName;
      output += ", DT: " + this._DataType;
      output += ", SS: " + this._SelectionSource;
      output += ", VN: " + this._ValueName;
      output += ", V: " + this._Value;
      output += ", O: " + this._Operator;

      return output;
    }

    /// <summary>
    /// This class return true if this query has selection source.
    /// </summary>
    /// <returns>Boolean: true, if the query has selection source</returns>
    public bool hasSelectionSource ( )
    {
      return this.SelectionSource != EvReport.SelectionListTypes.None;
    }

    /// <summary>
    /// This class generates an operator string
    /// </summary>
    /// <returns>String: an operator string</returns>
    /// <remarks>
    /// This method consists of the following step:
    /// 
    /// 1. Switch Operator and return value defining by the operators
    /// </remarks>
    public String getOperatorString ( )
    {
      //
      // Switch Operator and return value defining by the operators
      //
      switch ( Operator )
      {
        case EvReport.Operators.Greater_than:
          return ">";
        case EvReport.Operators.Less_than:
          return "<";
        case EvReport.Operators.Equals_to:
        default:
          return "=";

      }
    }//END getOperatorString class

    #endregion

  }//END EvReportQuery class 

}//END Namespace Evado.Model.Digital
