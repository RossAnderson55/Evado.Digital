/***************************************************************************************
 * <copyright file="EvFormFieldSelectionList.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvFormFieldSelectionList data object.
 *
 ****************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

namespace Evado.Model.Digital
{
  /// <summary>
  /// This is the data model for the form field selection list class.
  /// </summary>
  [Serializable]
  public class EvFormFieldSelectionList
  {
    #region CodeItem class

    /// <summary>
    ///  This Xml data class contains the trial objects Xml content.
    /// </summary>
    [Serializable]
    public class CodeItem
    {
      /// <summary>
      ///  Initialise the class object.
      /// </summary>
      public CodeItem()
      {

      }

      #region Private members.

      private int _No = 0;
      private string _Value = string.Empty;
      private string _Description = string.Empty;
      private string _Category = string.Empty;
      /// <summary>
      /// This field dfines the list identifier.
      /// </summary>
      public string ListId = string.Empty;

      #endregion

      #region class property
      /// <summary>
      /// This property contains a number code item.
      /// </summary>
      public int No
      {
        get { return this._No; }
        set { this._No = value; }
      }

      /// <summary>
      /// This property contains a value of code item.
      /// </summary>
      public string Value
      {
        get { return this._Value; }
        set { this._Value = value; }
      }

      /// <summary>
      /// This property contains description of code item.
      /// </summary>
      public string Description
      {
        get { return this._Description; }
        set { this._Description = value; }
      }

      /// <summary>
      /// This property contains a category of code item.
      /// </summary>
      public string Category
      {
        get { return this._Category; }
        set { this._Category = value; }
      }

      #endregion

    }//END CodeItem class

    #endregion

    #region XmlValidationRules class

    /// <summary>
    ///  This Xml data class contains the trial objects Xml content.
    /// </summary>
    [Serializable]
    public class XmlValidationRules
    {
      /// <summary>
      ///  Initialise the class object.
      /// 
      /// </summary>
      public XmlValidationRules()
      {
      }

      #region Private members.

      #endregion

      #region public members

      /// <summary>
      /// This field is number of items
      /// </summary>
      public int NumberOfItems = 1;

      #endregion

    }//END XmlValidationRules class
    #endregion

    #region Enumerators
    /// <summary>
    /// This enumeration list defines the states of selection list.
    /// </summary>
    public enum SelectionListStates
    {
      /// <summary>
      /// This enumeration defines null value or not selection state
      /// </summary>
      Null = 0,

      /// <summary>
      /// This enumeration defines a draft state in selection list
      /// </summary>
      Draft = 1,

      /// <summary>
      /// This enumeration defines a reviewed state in selection list
      /// </summary>
      Reviewed = 2,

      /// <summary>
      /// This enumeration defines an issued state in selection list
      /// </summary>
      Issued = 3,

      /// <summary>
      /// This enumeration defines a withdrawn state in selection list
      /// </summary>
      Withdrawn = 4,
    }

    #endregion

    #region internal variables.
    //
    //Internal member variables
    //
    private int _Uid = 0;
    private Guid _Guid = Guid.Empty;
    private string _ListId = String.Empty;
    private string _Title = String.Empty;
    private string _Reference = String.Empty;
    private string _Instructions = String.Empty;
    private string _Authors = String.Empty;
    private string _ReviewedBy = String.Empty;
    private DateTime _ReviewDate = EvcStatics.CONST_DATE_NULL;
    private string _ApprovedBy = String.Empty;
    private DateTime _ApprovalDate = EvcStatics.CONST_DATE_NULL;
    private string _Version = String.Empty;
    private SelectionListStates _State = SelectionListStates.Null;
    private string _UpdatedBy = String.Empty;
    private string _UpdatedDate = String.Empty;
    private bool _Selected = false;
    private List<CodeItem> _Items = new List<CodeItem>();

    // 
    // Fields used for linking the test to a trial.
    // 
    private string _TrialId = String.Empty;
    private string _TrialInstructions = String.Empty;

    private XmlValidationRules _ValidationRules = new XmlValidationRules();

    // Intermediate business Updatedic variables
    private string _ReviewedByUserId = String.Empty;
    private string _ApprovedByUserId = String.Empty;
    private string _Action = String.Empty;
    private string _UpdatedByUserId = String.Empty;
    private string _UserCommonName = String.Empty;
    private bool _IsAuthenticatedSignature = false;

    #endregion

    #region class Properties
    /// <summary>
    /// This property contains a unique identifier of form field selection list
    /// </summary>
    public int Uid
    {
      get { return this._Uid; }
      set { this._Uid = value; }
    }

    /// <summary>
    /// This property contains a global unique identifier of form field selection list
    /// </summary>
    public Guid Guid
    {
      get { return this._Guid; }
      set { this._Guid = value; }
    }

    /// <summary>
    /// This property contains a list identifier of form field selection list
    /// </summary>
    public string ListId
    {
      get { return this._ListId; }
      set { this._ListId = value; }
    }

    /// <summary>
    /// This property contains a title of form field selection list
    /// </summary>
    public string Title
    {
      get { return this._Title; }
      set { this._Title = value; }
    }

    /// <summary>
    /// This property contains a reference of form field selection list
    /// </summary>
    public string Reference
    {
      get { return this._Reference; }
      set { this._Reference = value; }
    }

    /// <summary>
    /// This property contains instructions of form field selection list
    /// </summary>
    public string Instructions
    {
      get { return this._Instructions; }
      set { this._Instructions = value; }
    }

    /// <summary>
    /// This property contains html instructions of form field selection list
    /// </summary>
    public string htmInstructions
    {
      get { return this._Instructions.Replace("\r\n", "<br/>"); }
    }

    /// <summary>
    /// This property contains authors of form field selection list
    /// </summary>
    public string Authors
    {
      get { return this._Authors; }
      set { this._Authors = value; }
    }

    /// <summary>
    /// This property contains a user who reviews a form field selection list
    /// </summary>
    public string ReviewedBy
    {
      get { return this._ReviewedBy; }
      set { this._ReviewedBy = value; }
    }

    /// <summary>
    /// This property contains a user identifier who reviews a form field selection list
    /// </summary>
    public string ReviewedByUserId
    {
      get { return this._ReviewedByUserId; }
      set { this._ReviewedByUserId = value; }
    }

    /// <summary>
    /// This property contains a reviewed date of form field selection list
    /// </summary>
    public DateTime ReviewDate
    {
      get { return this._ReviewDate; }
      set { this._ReviewDate = value; }
    }

    /// <summary>
    /// This property contains a reviewed date string of form field selection list
    /// </summary>
    public string stReviewDate
    {
      get
      {
        if (this._ReviewDate == EvcStatics.CONST_DATE_NULL)
        {
          return String.Empty;
        }
        return this._ReviewDate.ToString("dd MMM yyyy");
      }
    }

    /// <summary>
    /// This property contains a user who approves a form field selection list
    /// </summary>
    public string ApprovedBy
    {
      get { return this._ApprovedBy; }
      set { this._ApprovedBy = value; }
    }

    /// <summary>
    /// This property contains a user identifier who approves a form field selection list
    /// </summary>
    public string ApprovedByUserId
    {
      get { return this._ApprovedByUserId; }
      set { this._ApprovedByUserId = value; }
    }

    /// <summary>
    /// This property contains an approval date of form field selection list
    /// </summary>
    public DateTime ApprovalDate
    {
      get { return this._ApprovalDate; }
      set { this._ApprovalDate = value; }
    }

    /// <summary>
    /// This property contains an approval date string of a form field selection list
    /// </summary>
    public string stApprovalDate
    {
      get
      {
        if (this._ApprovalDate == EvcStatics.CONST_DATE_NULL)
        {
          return String.Empty;
        }
        return this._ApprovalDate.ToString("dd MMM yyyy");
      }
    }

    /// <summary>
    /// This property contains a version of form field selection list
    /// </summary>
    public string Version
    {
      get { return this._Version; }
      set { this._Version = value; }
    }

    /// <summary>
    /// This property indicates whether a form field selection list is selected.
    /// </summary>
    public bool Selected
    {
      get { return this._Selected; }
      set { this._Selected = value; }
    }

    /// <summary>
    /// This property contains a state object of form field selection list
    /// </summary>
    public SelectionListStates State
    {
      get { return this._State; }
      set { this._State = value; }
    }

    /// <summary>
    /// This property contains a state description of form field selection list
    /// </summary>
    public string StateDesc
    {
      get { return EvcStatics.Enumerations.enumValueToString(this._State); }
      set { string v = value; }
    }

    /// <summary>
    /// This property contains a user who updates a form field selection list
    /// </summary>
    public string UpdatedBy
    {
      get { return this._UpdatedBy; }
      set { this._UpdatedBy = value; }
    }

    /// <summary>
    /// This property contains a user identifier who updates form field selection list
    /// </summary>
    public string UpdatedByUserId
    {
      get { return this._UpdatedByUserId; }
      set { this._UpdatedByUserId = value; }
    }

    /// <summary>
    /// This property contains an updated date of form field selection list
    /// </summary>
    public string UpdatedDate
    {
      get { return this._UpdatedDate; }
      set { this._UpdatedDate = value; }
    }

    /// <summary>
    /// This property contains a user common name of form field selection list
    /// </summary>
    public string UserCommonName
    {
      get { return this._UserCommonName; }
      set { this._UserCommonName = value; }
    }

    /// <summary>
    /// This property contains an action of form field selection list
    /// </summary>
    public string Action
    {
      get { return this._Action; }
      set { this._Action = value; }
    }

    /// <summary>
    /// This property indicates whether a form field selection list contains authenticated signature.
    /// </summary>
    public bool IsAuthenticatedSignature
    {
      get { return this._IsAuthenticatedSignature; }
      set { this._IsAuthenticatedSignature = value; }
    }

    /// <summary>
    /// This property contains a trial identifier of form field selection list
    /// </summary>
    public string TrialId
    {
      get { return this._TrialId; }
      set
      {
        this._TrialId = value;
        if (TrialId.Length > 0)
        {
          this._Selected = true;
        }
      }
    }

    /// <summary>
    /// This property contains trial instructions of form field selection list
    /// </summary>
    public string TrialInstructions
    {
      get { return this._TrialInstructions; }
      set { this._TrialInstructions = value; }
    }

    /// <summary>
    /// This property contains a html Instruction of a form field selection list
    /// </summary>
    public string InstructionsHtml
    {
      get
      {
        string Display = String.Empty;
        if (this._Instructions.Length > 0)
        {
          Display += "<p><b>General Instructions</b><br/>"
            + this._Instructions.Replace("\r", "<BR/>") + "</p>";
        }
        if (this._Instructions.Length > 0)
        {
          Display += "<p><b>Trial Instructions</b><br/>"
            + this._TrialInstructions.Replace("\r", "<BR/>") + "</p>";
        }

        return Display;
      }
    }

    /// <summary>
    /// This property contains validation rule object of form field selection list
    /// </summary>
    public XmlValidationRules ValidationRules
    {
      get
      {
        return this._ValidationRules;
      }
      set
      {
        this._ValidationRules = value;
      }
    }

    /// <summary>
    /// This property contains an item list of form field selection list
    /// </summary>
    public List<CodeItem> Items
    {
      get { return this._Items; }
      set { this._Items = value; }
    }
    #endregion

  }//END EvFormFieldSelectionList class


}//END namespace Evado.Model.Digital 
