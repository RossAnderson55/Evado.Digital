/***************************************************************************************
 * <copyright file="EvFormRecordSummary.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvFormRecordSummary data object.
 *
 ****************************************************************************************/

using System;

namespace Evado.Model.Digital
{

  /// <summary>
  /// Business entity used to model accounts
  /// </summary>
  [Serializable]
  public class EvFormRecordSummary
  {

    #region Internal member variables

    private string _SubjectId = String.Empty;
    private int _DraftRecords = 0;
    private int _SubmittedRecords = 0;
    private int _SourceDataReviewedRecord = 0;
    private int _OpenQueriedRecords = 0;
    private int _LockedRecords = 0;
    private int _CancelledRecords = 0;
    private int _ClosedQueriedRecords = 0;
    private int _TotalRecords = 0;
    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    public EvFormRecordSummary()
    {
    }

    #region Properties
    /// <summary>
    /// This property contains a subject identifier of a record summary.
    /// </summary>
    public string SubjectId
    {
      get { return _SubjectId; }
      set { _SubjectId = value; }
    }

    /// <summary>
    /// This property contains the number of created number of a record summary.
    /// </summary>
    public int DraftRecords
    {
      get { return _DraftRecords; }
      set { _DraftRecords = value; }
    }

    /// <summary>
    /// This property contains the number of submitted records.
    /// </summary>
    public int SubmittedRecords
    {
      get { return _SubmittedRecords; }
      set { _SubmittedRecords = value; }
    }

    /// <summary>
    /// This property contains the number of source data reviewed records.
    /// </summary>
    public int SourceDataReviewedRecords
    {
      get { return _SourceDataReviewedRecord; }
      set { _SourceDataReviewedRecord = value; }
    }

    /// <summary>
    /// This property contains the number of open queries.
    /// </summary>
    public int OpenQueriedRecords
    {
      get { return _OpenQueriedRecords; }
      set { _OpenQueriedRecords = value; }
    }

    /// <summary>
    /// This property contains the number of closed queries.
    /// </summary>
    public int ClosedQueriedRecords
    {
      get { return _ClosedQueriedRecords; }
      set { _ClosedQueriedRecords = value; }
    }

    /// <summary>
    /// This property contains the number of approved records.
    /// </summary>
    public int LockedRecords
    {
      get { return _LockedRecords; }
      set { _LockedRecords = value; }
    }

    /// <summary>
    /// This property contains  number of cancelled records.
    /// </summary>
    public int CancelledRecords
    {
      get { return _CancelledRecords; }
      set { _CancelledRecords = value; }
    }

    /// <summary>
    /// This property contains  number of un source data reviewed records.
    /// </summary>
    public int NotSourceDataReviewedRecords
    {
      get
      {
        int notSourceDataReviewedRecord = this._TotalRecords - this._CancelledRecords - this._SourceDataReviewedRecord;

        return notSourceDataReviewedRecord;
      }
    }

    /// <summary>
    /// This property contains a not approved number of a record summary.
    /// </summary>
    public int NotLockedRecords
    {
      get
      {
        int iNotReviewed = this._TotalRecords - this._CancelledRecords - this._LockedRecords;

        return iNotReviewed;
      }
    }

    /// <summary>
    /// This property contains a total number of a records.
    /// </summary>
    public int TotalRecords
    {
      get { return _TotalRecords; }
      set { _TotalRecords = value; }
    }

    /// <summary>
    /// This property indicates whether all records in summary are signed off
    /// </summary>
    public bool AllRecordsSignedOff
    {
      get
      {
        if (NotLockedRecords == 0)
        {
          return true;
        }
        return false;
      }
    }

    #endregion

  }//END EvFormRecordSummary class

}//END namespace Evado.Model.Digital
