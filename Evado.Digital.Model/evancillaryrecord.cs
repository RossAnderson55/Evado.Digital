/***************************************************************************************
 * <copyright file="EvSubjectRecord.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvSubjectRecord data object.
 *
 ****************************************************************************************/

using System;
using System.Collections.Generic;

namespace Evado.Digital.Model
{

  /// <summary>
  /// Business entity used to model accounts
  /// </summary>
  [Serializable]
  public class EvAncillaryRecord
  {

    #region Enumeration
    /// <summary>
    /// This enumeration list defines field names of subject record
    /// </summary>
    public enum SubjectRecordFieldNames
    {
      /// <summary>
      /// This enumeration defines null value or not selection state
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines a trial identifier field name of subject record
      /// </summary>
      TrialId,

      /// <summary>
      /// This enumeration defines a subject identifier field name of subject record
      /// </summary>
      SubjectId,

      /// <summary>
      /// This enumeration defines a visit identifier field name of subject record
      /// </summary>
      VisitId,

      /// <summary>
      /// This enumeration defines a record identifier field name of subject record
      /// </summary>
      RecordId,

      /// <summary>
      /// This enumeration defines a record date field name of subject record
      /// </summary>
      RecordDate,

      /// <summary>
      /// This enumeration defines a subject field name of subject record
      /// </summary>
      Subject,

      /// <summary>
      /// This enumeration defines a record field name of subject record
      /// </summary>
      Record,

      /// <summary>
      /// This enumeration defines a status field name of subject record
      /// </summary>
      Status,
    }

    #endregion

    #region Internal member parameters
    private Guid _Guid = Guid.Empty;
    private string _ProjectId = String.Empty;
    private string _SubjectId = String.Empty;
    private string _VisitId = String.Empty;
    private string _RecordId = String.Empty;
    private DateTime _RecordDate =  Evado.Model.EvStatics.CONST_DATE_NULL;
    private string _Subject = String.Empty;
    private string _Record = String.Empty;
    private byte[] _BinaryObject = new byte[1];
    private int _BinaryLength = 0;
    private string _BinaryType = String.Empty;
    private string _BinaryExtension = String.Empty;
    private string _XmlData = String.Empty;
    private string _Researcher = String.Empty;
    private DateTime _ResearcherDate =  Evado.Model.EvStatics.CONST_DATE_NULL;
    private string _Reviewer = String.Empty;
    private DateTime _ReviewDate =  Evado.Model.EvStatics.CONST_DATE_NULL;
    private string _Approver = String.Empty;
    private DateTime _ApprovalDate =  Evado.Model.EvStatics.CONST_DATE_NULL;
    private EdRecordObjectStates _State = EdRecordObjectStates.Null;
    private DateTime _UpdatedDate = EvcStatics.CONST_DATE_NULL;
    private string _BookedOutBy = String.Empty;
    private List<EdUserSignoff> _Signoffs = new List<EdUserSignoff>();
    // 
    // Display fields
    // 
    private string _Action = String.Empty;
    private string _ResearcherUserId = String.Empty;
    private string _ReviewerUserId = String.Empty;
    private string _ApproverUserId = String.Empty;
    private string _UpdatedByUserId = String.Empty;
    private string _UpdatedBy = String.Empty;
    private string _UserCommonName = String.Empty;
    private bool _IsAuthenticatedSignature = false;

    #endregion

    #region Class Properties

    /// <summary>
    /// This property contains a global unique identifier of subject record
    /// </summary>
    public Guid Guid
    {
      get { return this._Guid; }
      set { this._Guid = value; }
    }

    /// <summary>
    /// This property contains a trial identifier of subject record
    /// </summary>
    public string ProjectId
    {
      get { return this._ProjectId; }
      set { this._ProjectId = value; }
    }

    /// <summary>
    /// This property contains a subject identifier of subject record
    /// </summary>
    public string SubjectId
    {
      get { return this._SubjectId; }
      set { this._SubjectId = value; }
    }

    /// <summary>
    /// This property contains a visit identifier of subject record
    /// </summary>
    public string VisitId
    {
      get { return this._VisitId; }
      set { this._VisitId = value; }
    }

    /// <summary>
    /// This property contains a record identifier of subject record
    /// </summary>
    public string RecordId
    {
      get { return this._RecordId; }
      set { this._RecordId = value; }
    }

    /// <summary>
    /// This property contains a record date of subject record
    /// </summary>
    public DateTime RecordDate
    {
      get { return this._RecordDate; }
      set { this._RecordDate = value; }
    }

    /// <summary>
    /// This property contains a record date string of subject record
    /// </summary>
    public string stRecordDate
    {
      get
      {
        if (this.RecordDate >  Evado.Model.EvStatics.CONST_DATE_NULL)
        {
          return this._RecordDate.ToString("dd MMM yyyy");
        }
        return String.Empty;
      }
    }

    /// <summary>
    /// This property contains a subject of subject record
    /// </summary>
    public string Subject
    {
      get { return this._Subject; }
      set { this._Subject = value; }
    }

    /// <summary>
    /// This property contains a record string of subject record
    /// </summary>
    public string Record
    {
      get { return this._Record; }
      set { this._Record = value; }
    }

    /// <summary>
    /// This property contains a binary object of subject record
    /// </summary>
    public byte[] BinaryObject
    {
      get { return this._BinaryObject; }
      set { this._BinaryObject = value; }
    }

    /// <summary>
    /// This property contains a numeric binary length of subject record
    /// </summary>
    public int BinaryLength
    {
      get { return this._BinaryLength; }
      set { this._BinaryLength = value; }
    }

    /// <summary>
    /// This property contains a binary type of subject record
    /// </summary>
    public string BinaryType
    {
      get { return this._BinaryType; }
      set { this._BinaryType = value; }
    }

    /// <summary>
    /// This property contains a binary extension of subject record
    /// </summary>
    public string BinaryExtension
    {
      get { return this._BinaryExtension; }
      set { this._BinaryExtension = value; }
    }

    /// <summary>
    /// This property contains an xml data of subject record
    /// </summary>
    public string XmlData
    {
      get { return this._XmlData; }
      set { this._XmlData = value; }
    }

    /// <summary>
    /// This property contains a researcher of subject record
    /// </summary>
    public string Researcher
    {
      get { return this._Researcher; }
      set { this._Researcher = value; }
    }

    /// <summary>
    /// This property contains a research date of subject record
    /// </summary>
    public DateTime ResearcherDate
    {
      get { return this._ResearcherDate; }
      set { this._ResearcherDate = value; }
    }

    /// <summary>
    /// This property contains a reviewer of subject record
    /// </summary>
    public string Reviewer
    {
      get { return this._Reviewer; }
      set { this._Reviewer = value; }
    }

    /// <summary>
    /// This property contains a review date of subject record
    /// </summary>
    public DateTime ReviewDate
    {
      get { return this._ReviewDate; }
      set { this._ReviewDate = value; }
    }

    /// <summary>
    /// This property contains an approver of subject record
    /// </summary>
    public string Approver
    {
      get { return this._Approver; }
      set { this._Approver = value; }
    }

    /// <summary>
    /// This property contains an approval date of subject record
    /// </summary>
    public DateTime ApprovalDate
    {
      get { return this._ApprovalDate; }
      set { this._ApprovalDate = value; }
    }

    /// <summary>
    /// This property contains a form state object of subject record
    /// </summary>
    public EdRecordObjectStates State
    {
      get { return this._State; }
      set { this._State = value; }
    }

    /// <summary>
    /// This property contains an updated date of subject record
    /// </summary>
    public DateTime UpdatedDate
    {
      get { return this._UpdatedDate; }
      set { this._UpdatedDate = value; }
    }

    /// <summary>
    /// This property contains a user who books out the subject record
    /// </summary>
    public string BookedOutBy
    {
      get { return this._BookedOutBy; }
      set { this._BookedOutBy = value; }
    }

    #region Display Fields

    /// <summary>
    /// This property contains an action string of subject record
    /// </summary>
    public string Action
    {
      get { return this._Action; }
      set { this._Action = value; }
    }

    /// <summary>
    /// This property contains a researcher user identifier of subject record
    /// </summary>
    public string ResearcherUserId
    {
      get { return this._ResearcherUserId; }
      set { this._ResearcherUserId = value; }
    }

    /// <summary>
    /// This property contains a reviewer user identifier of subject record
    /// </summary>
    public string ReviewerUserId
    {
      get { return this._ReviewerUserId; }
      set { this._ReviewerUserId = value; }
    }

    /// <summary>
    /// This property contains an approver user identifier of subject record
    /// </summary>
    public string ApproverUserId
    {
      get { return this._ApproverUserId; }
      set { this._ApproverUserId = value; }
    }

    /// <summary>
    /// This property contains a user identifier of those who updates subject record
    /// </summary>
    public string UpdatedByUserId
    {
      get { return this._UpdatedByUserId; }
      set { this._UpdatedByUserId = value; }
    }

    /// <summary>
    /// This property contains a user who updates the subject record
    /// </summary>
    public string UpdatedBy
    {
      get { return this._UpdatedBy; }
      set { this._UpdatedBy = value; }
    }

    /// <summary>
    /// This property contains a user common name of those who updates subject record
    /// </summary>
    public string UserCommonName
    {
      get { return this._UserCommonName; }
      set { this._UserCommonName = value; }
    }

    /// <summary>
    /// This property indicates whether the subject record is authenticated signature
    /// </summary>
    public bool IsAuthenticatedSignature
    {
      get { return this._IsAuthenticatedSignature; }
      set { this._IsAuthenticatedSignature = value; }
    }

    /// <summary>
    /// This property contains a signoffs list of subject record
    /// </summary>
    public List<EdUserSignoff> Signoffs
    {
      get { return this._Signoffs; }
      set { this._Signoffs = value; }
    }

    /// <summary>
    /// This property contains state description of subject record
    /// </summary>
    public string StateDesc
    {
      get
      {
        return  Evado.Model.EvStatics.enumValueToString(this._State);
      }
      set
      {
        string v = value;
      }
    }
    #endregion

    #endregion

    #region Methods
    // =====================================================================================
    /// <summary>
    /// This class provides a list of trial types.
    /// </summary>
    /// <returns>string: a value of a selected field name</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialise a fieldname object
    /// 
    /// 2. Try parse Fieldname string into an enumerated fieldname
    /// 
    /// 3. Switch fieldname and update value defining by the subject record field names
    /// </remarks>
    //  ------------------------------------------------------------------------------------
    public string getValue(string FieldName)
    {
      // 
      // Initialise a fieldname object
      // 
      SubjectRecordFieldNames fieldname = SubjectRecordFieldNames.Null;

      //
      // Try parse Fieldname string into an enumerated fieldname
      //
      try
      {
        fieldname = (SubjectRecordFieldNames)Enum.Parse(typeof(SubjectRecordFieldNames), FieldName);
      }
      catch
      {
        fieldname = SubjectRecordFieldNames.Null;
      }

      // 
      // Switch fieldname and update value defining by the subject record field names
      // 
      switch (fieldname)
      {
        case SubjectRecordFieldNames.TrialId:
          return this._ProjectId;


        case SubjectRecordFieldNames.SubjectId:
          return this._SubjectId;

        case SubjectRecordFieldNames.VisitId:
          return this._VisitId;

        case SubjectRecordFieldNames.Subject:
          return this._Subject;

        case SubjectRecordFieldNames.Record:
          return this._Record;

        case SubjectRecordFieldNames.RecordDate:
          return this._RecordDate.ToString("d MMM yyyy");

        case SubjectRecordFieldNames.Status:
          return this.StateDesc;

        default:
          {
            return String.Empty;

          }//END Default

      }//END Switch

    }//END getValue method

    // =====================================================================================
    /// <summary>
    /// This method sets the field value.
    /// </summary>
    /// <param name="FieldName">string: a field name</param>
    /// <param name="Value">string: a retrieved value</param>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialise a fieldname object
    /// 
    /// 2. Try parse Fieldname string into an enumerated fieldname
    /// 
    /// 3. Switch fieldname and update value defining by the subject record field names
    /// </remarks>
    //  ------------------------------------------------------------------------------------
    public void setValue(string FieldName, string Value)
    {
      // 
      // Initialise the methods variables and objects.
      //
      DateTime date = EvcStatics.CONST_DATE_NULL;
      //float fltValue = 0;
      //int intValue = 0;
      //bool bValue = false;
      SubjectRecordFieldNames fieldname = SubjectRecordFieldNames.Null;

      //
      // Try parse FieldName string into an enumerated fieldname object
      //
      try
      {
        fieldname = (SubjectRecordFieldNames)Enum.Parse(typeof(SubjectRecordFieldNames), FieldName);
      }
      catch
      {
        fieldname = SubjectRecordFieldNames.Null;
      }

      // 
      // Switch fieldname and update value defining by the subject record field names
      // 

      switch (fieldname)
      {
        case SubjectRecordFieldNames.TrialId:
          this._ProjectId = Value;
          return;

        case SubjectRecordFieldNames.SubjectId:
          this._SubjectId = Value;
          return;

        case SubjectRecordFieldNames.VisitId:
          this._VisitId = Value;
          return;

        case SubjectRecordFieldNames.RecordDate:
          {
            if (DateTime.TryParse(Value, out date) == true)
            {
              this._RecordDate = date;
            }
            return;
          }

        case SubjectRecordFieldNames.Subject:
          {
            this._Subject = Value;
            return;
          }

        case SubjectRecordFieldNames.Record:
          {
            this._Record = Value;
            return;
          }
        default:
          {
            return;
          }

      }//END Switch

    }//END setValue method

    #endregion

  }//END setValue method

}//END namespace Evado.Digital.Model
