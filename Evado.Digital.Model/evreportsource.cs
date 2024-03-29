/***************************************************************************************
 * <copyright file="model\eclinical\EvReportSource.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvReport data object.
 *
 ****************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

namespace Evado.Digital.Model
{
  /// <summary>
  /// data  entity used to model accounts
  /// </summary>
  [Serializable]
  public class EvReportSource
  {
    #region Class Properties

    string _SourceId = string.Empty;
    /// <summary>
    /// This property contains a source  identifier of a report
    /// </summary>
    public String SourceId
    {
      get
      {
        return _SourceId;
      }
      set
      {
        _SourceId = value;
      }
    }


    string _Name = string.Empty;
    /// <summary>
    /// This property contains a source  title of a report
    /// </summary>
    public String Name 
    {
      get
      {
        return _Name;
      }
      set
      {
        _Name = value;
      }
    }
      
    string _Description = string.Empty;
    /// <summary>
    /// This property contains a source  description of a report
    /// </summary>
    public String Description
    {
      get
      {
        return _Description;
      }
      set
      {
        _Description = value;
      }
    }

    EdReport.ReportSourceCode _ReportSource = EdReport.ReportSourceCode.Null;
    /// <summary>
    /// This property contains a report source code
    /// </summary>
    public EdReport.ReportSourceCode ReportSource
    {
      get
      {
        return _ReportSource;
      }
      set
      {
        _ReportSource = value;
      }
    }

    string _SqlQuery = string.Empty;
    /// <summary>
    /// This property contains a source  query
    /// </summary>
    public String SqlQuery
    {
      get
      {
        return _SqlQuery;
      }
      set
      {
        _SqlQuery = value;
      }
    }

    List<EdReportQuery> _QueryList = new List<EdReportQuery> ( );
    /// <summary>
    /// This property contains the list of query options for this data source.
    /// </summary>
    public List<EdReportQuery> QueryList 
    {
      get
      {
        return _QueryList;
      }
      set
      {
        _QueryList = value;
      }
    }

    List<EdReportColumn> _ColumnList = new List<EdReportColumn> ( );
    /// <summary>
    /// This property contains the list of column options for this data source.
    /// </summary>
    public List<EdReportColumn> ColumnList
    {
      get
      {
        return _ColumnList;
      }
      set
      {
        _ColumnList = value;
      }
    }

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class methods

    //==================================================================================
    /// <summary>
    /// This method creates a list of query options.
    /// </summary>
    /// <param name="IsSelectionList">bool: true if a selection list.</param>
    /// <returns>List of Evado.Model.EvOption objects.</returns>
    //-----------------------------------------------------------------------------------
    public List<Evado.Model.EvOption> QueryOptionList ( bool IsSelectionList )
    {
      // 
      // Initialise local variables and objects.
      // 
      List<Evado.Model.EvOption> optionList = new List<Evado.Model.EvOption> ( );
      Evado.Model.EvOption option = new Evado.Model.EvOption ( );

      //
      // exit if query list is null.
      //
      if ( QueryList == null )
      {
        return optionList;
      }

      //
      // Add an empty option for a selection lists.
      //
      if ( IsSelectionList == true )
      {
        optionList.Add ( option );
      }

      //
      // Create the option list.
      //
      foreach ( EdReportQuery query in QueryList )
      {
        option = new Evado.Model.EvOption ( query.QueryId, query.QueryTitle );

        if ( query.DataType != EdReport.DataTypes.Text )
        {
          option.Description += " (" + query.DataType + ")";
        }

        optionList.Add ( option );
      }


      return optionList;

    }//ENdc QueryOptionList method

    //==================================================================================
    /// <summary>
    /// This method creates a list of query options.
    /// </summary>
    /// <param name="IsSelectionList">bool: true if a selection list.</param>
    /// <returns>List of Evado.Model.EvOption objects.</returns>
    //-----------------------------------------------------------------------------------
    public List<Evado.Model.EvOption> ColumnOptionList ( bool IsSelectionList )
    {
      // 
      // Initialise local variables and objects.
      // 
      List<Evado.Model.EvOption> optionList = new List<Evado.Model.EvOption> ( );
      Evado.Model.EvOption option = new Evado.Model.EvOption ( );

      //
      // exit if query list is null.
      //
      if ( ColumnList == null )
      {
        return optionList;
      }

      //
      // Add an empty option for a selection lists.
      //
      if ( IsSelectionList == true )
      {
        optionList.Add ( option );
      }

      //
      // Create the option list.
      //
      foreach ( EdReportColumn column in ColumnList )
      {
        option = new Evado.Model.EvOption ( column.ColumnId, column.HeaderText );

        if ( column.DataType != EdReport.DataTypes.Text )
        {
          option.Description += " (" + column.DataType + ")";
        }

        optionList.Add ( option );
      }


      return optionList;

    }//END ColumnOptionList method

    //==================================================================================
    /// <summary>
    /// This method retrieves the selected query.
    /// </summary>
    /// <param name="QueryId">String: the queries identifier.</param>
    /// <returns>EvReportQuery objects.</returns>
    //-----------------------------------------------------------------------------------
    public EdReportQuery getQuery ( String QueryId )
    {
      //
      // exit if query list is null.
      //
      if ( QueryList == null )
      {
        //return new EvReportQuery ( );
      }

      //
      // search the list for the matching query.
      //
      foreach ( EdReportQuery query in QueryList )
      {
        if ( query.QueryId == QueryId )
        {
          return query;
        }
      }

      //
      // Returm empty objects.
      //
      return new EdReportQuery ( );

    }//END getQuery method

    //==================================================================================
    /// <summary>
    /// This method retrieves the selected query.
    /// </summary>
    /// <param name="SelectionSource">String: the selection source.</param>
    /// <returns>EvReportQuery objects.</returns>
    //-----------------------------------------------------------------------------------
    public EdReportQuery getQueryBySelectionSource ( EdReport.SelectionListTypes SelectionSource )
    {
      //
      // exit if query list is null.
      //
      if ( QueryList == null )
      {
        return new EdReportQuery ( );
      }

      //
      // search the list for the matching query.
      //
      foreach ( EdReportQuery query in QueryList )
      {
        if ( query.SelectionSource == SelectionSource )
        {
          return query;
        }
      }

      //
      // Returm empty objects.
      //
      return new EdReportQuery ( );

    }//END getQuery method

    //==================================================================================
    /// <summary>
    /// This method retrieves the selected query.
    /// </summary>
    /// <param name="ColumnId">String: the column identifier.</param>
    /// <returns>EvReportQuery objects.</returns>
    //-----------------------------------------------------------------------------------
    public EdReportColumn getColumn ( String ColumnId )
    {
      //
      // exit if query list is null.
      //
      if ( ColumnList == null )
      {
        return new EdReportColumn ( );
      }

      //
      // search the list for the matching query.
      //
      foreach ( EdReportColumn column in ColumnList )
      {
        if ( column.ColumnId == ColumnId )
        {
          return column;
        }
      }

      //
      // Returm empty objects.
      //
      return new EdReportColumn ( );

    }//END getColumn method

    //==================================================================================
    /// <summary>
    /// This method retrieves the selected query.
    /// </summary>
    /// <param name="SourceField">String: the column source field.</param>
    /// <returns>EvReportQuery objects.</returns>
    //-----------------------------------------------------------------------------------
    public EdReportColumn getColumnBySourceField ( String SourceField )
    {
      //
      // exit if query list is null.
      //
      if ( QueryList == null )
      {
        return new EdReportColumn ( );
      }

      //
      // search the list for the matching query.
      //
      foreach ( EdReportColumn column in ColumnList )
      {
        if ( column.SourceField == SourceField )
        {
          return column;
        }
      }

      //
      // Returm empty objects.
      //
      return new EdReportColumn ( );

    }//END getColumn method

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

  }//END EvReport class 

}//END Namespace Evado.Model.Digital
