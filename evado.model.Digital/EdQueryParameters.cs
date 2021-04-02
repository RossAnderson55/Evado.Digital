/***************************************************************************************
 * <copyright file="QueryParameters.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the QueryParameters data object.
 *
 ****************************************************************************************/

using System;
using System.Collections; 
using System.Collections.Generic;

namespace Evado.Model.Digital
{

  /// <summary>
  /// Business entity used to model Therapy query object.
  /// </summary>
  [Serializable]
  public class EdQueryParameters
  {
    #region Initialization
    /// <summary>
    /// This method initialise object
    /// </summary>
    public EdQueryParameters( )
    { }

    #endregion

    #region Public fields
    //State

    /// <summary>
    /// This field defines the to not select the subject or record state.
    /// </summary>
    public bool NotSelectedState = false;

    /// <summary>
    /// This property defines the record type filter.
    /// </summary>
    public EdRecordTypes Type = EdRecordTypes.Null;

    /// <summary>
    /// This field defines the layout identifier filter.
    /// </summary>
    public string LayoutId = String.Empty;

    /// <summary>
    /// This field enables the organisational filters.
    /// </summary>
    public bool EnableOrganisationFilter = false;

    /// <summary>
    /// This field defines the organisation city filter.
    /// </summary>
    public string Org_City = String.Empty;

    /// <summary>
    /// This field defines the organisation country filter.
    /// </summary>
    public string Org_Country = String.Empty;

    /// <summary>
    /// This field defines the organisation post code filter.
    /// </summary>
    public string Org_PostCode = String.Empty;

    /// <summary>
    /// This field defines the entity record identifier.
    /// </summary>
    public string EntityId = String.Empty;

    /// <summary>
    /// This field defines the record fields are to be included in the result.
    /// </summary>
    public bool IncludeRecordValues = false;

    /// <summary>
    /// This field defines the defines if comments are to be included in the result.
    /// </summary>
    public bool IncludeComments = false;

    /// <summary>
    /// This field defines the include summaries data in the result
    /// </summary>
    public bool IncludeSummary = true;

    /// <summary>
    /// This field defines the to incude user Guid in output
    /// </summary>
    public bool UseGuid = false;

    /// <summary>
    /// This field defines the result set start range index
    /// </summary>
    public int ResultStartRange = 0;

    /// <summary>
    /// This field defines the result set finish rand index.
    /// </summary>
    public int ResultFinishRange = 100000000;

    #endregion

    #region Class property
    /// <summary>
    /// This property contains a list of the entity/record selection states.
    /// </summary>
    private List<EdRecordObjectStates> _States = new List<EdRecordObjectStates> ( );

    public List<EdRecordObjectStates> States
    {
      get { return _States; }
      set { _States = value; }
    }

    /// </summary>
    private String [ ] _SelectionFilters = new String [ EdRecord.FILTER_FIELD_COUNT ];
    /// <summary>
    /// This property contains a list of entity/record selection list filters.
    /// These filters field Ids are created from the listed field filters in the EntityLayout.
    /// The value is the selected value of the selected field value.
    /// </summary>
    public String [ ] SelectionFilters
    {
      get { return _SelectionFilters; }
      set { _SelectionFilters = value; }
    }

    #endregion

  }//END EvQueryParameters class

}//END namespace Evado.Model.Digital
