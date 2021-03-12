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

    #region Public member variables
    //State
    /// <summary>
    /// This field defines the subject's screening identifier.
    /// </summary>
    private List<EdRecordObjectStates> _States = new List<EdRecordObjectStates> ( );

    public List<EdRecordObjectStates> States
    {
      get { return _States; }
      set { _States = value; }
    }

    /// <summary>
    /// This field defines the layout identifier.
    /// </summary>
    public string LayoutId = String.Empty;

    public EdRecordTypes Type = EdRecordTypes.Null;

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
    /// This field defines the result is to inlcude full data set.
    /// </summary>
    public bool FullDataSet = false;

    /// <summary>
    /// This field defines the to not select the subject or record state.
    /// </summary>
    public bool NotSelectedState = false;

    /// <summary>
    /// This field defines the to incude user Guid in output
    /// </summary>
    public bool UseGuid = false;

    /// <summary>
    /// This field defines the output is to be result table count.
    /// </summary>
    public bool CountOnly = false;

    /// <summary>
    /// this field defines the maximum list length of the output.
    /// </summary>
    public int MaxListLength = 1000000;

    /// <summary>
    /// This field defines the result set start range index
    /// </summary>
    public int RecordRangeStart = 0;
    /// <summary>
    /// This field defines the result set finish rand index.
    /// </summary>
    public int RecordRangeFinish = 100000000;
    /// <summary>
    /// This field defines the that the user has edit access to the records.
    /// </summary>
    public bool hasRecordEditAccess = false;

    //public bool SubjectRecordQueryStatus = false;



    /// <summary>
    /// This field defines the if the summarised results are required.
    /// </summary>
    public bool SummaryResult = false;
    #endregion

    #region Class property

    #endregion

  }//END EvQueryParameters class

}//END namespace Evado.Model.Digital
