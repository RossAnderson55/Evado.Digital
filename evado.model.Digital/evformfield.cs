/***************************************************************************************
 * <copyright file="EvFormField.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvFormField data object.
 *
 ****************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

namespace Evado.Model.Digital
{
  /// <summary>
  /// This class defines the form field data object.
  /// </summary>
  [Serializable]
  public partial class EvFormField : EvHasSetValue<EvFormField.FormFieldClassFieldNames>
  {
    #region Class Enumerators

    /// <summary>
    /// This enumeration list defines the form record field states.
    /// </summary>
    public enum FieldStates
    {
      /// <summary>
      /// This enumeration defines null value or no selection state.
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration is for a null value.
      /// </summary>
      Empty,

      /// <summary>
      /// This enumeration defines the the state when a field value has a value.
      /// </summary>
      With_Value,

      /// <summary>
      /// TThis enumeration defines the the state when a field value has been queried.
      /// </summary>
      Queried,

      /// <summary>
      /// This enumeration defines the the state when a field value has been confirmed by a reviewer.
      /// </summary>
      Confirmed,
    }

    /// <summary>
    /// This enumeration list defines the data cleansing states.
    /// </summary>
    public enum DataCleansingStates
    {
      /// <summary>
      /// This enumeration is for a null value.
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines the state as unchanged.
      /// </summary>
      Unchanged,

      /// <summary>
      /// This enumeration defines the state for a value form change.
      /// </summary>
      Format_Changed,

      /// <summary>
      /// This enumeration defines the state for a value change.
      /// </summary>
      Value_Changed,

      /// <summary>
      /// Old values to be enumerated  
      /// </summary>      
      U,
      /// <summary>
      /// Old values to be enumerated  
      /// </summary> 
      F,
      /// <summary>
      /// Old values to be enumerated  
      /// </summary> 
      V,

    }

    /// <summary>
    /// This enumeration list defines the field names of form field class.
    /// </summary>
    public enum FormFieldClassFieldNames
    {
      /// <summary>
      /// This enumeration defines null value or no selection state.
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines field name trail identifier.
      /// </summary>
      TrialId,

      /// <summary>
      /// This enumeration defines field name form identifiers.
      /// </summary>
      FormId,

      /// <summary>
      /// This enumeration defines field name field identifier.
      /// </summary>      
      FieldId,

      /// <summary>
      /// This enumeration defines field name type identifier.
      /// </summary>
      TypeId,

      /// <summary>
      /// This enumeration defines field name order.
      /// </summary>
      Order,

      /// <summary>
      /// This enumeration defines field name subject.
      /// </summary>
      Subject,

      /// <summary>
      /// This enumeration defines field name instruction.
      /// </summary>
      Instructions,

      /// <summary>
      /// This enumeration defines field name reference.
      /// </summary>
      Reference,

      /// <summary>
      /// This enumeration defines field name quiz correct answer value.
      /// </summary>
      Quiz_Value,

      /// <summary>
      /// This enumeration defines field name quiz correct answer description.
      /// </summary>
      Quiz_Answer,

      /// <summary>
      /// This enumeration defines field name quiz correct answer description.
      /// </summary>
      Html_Link,

      /// <summary>
      /// This enumeration defines field name valid for males
      /// </summary>
      NotValidForMale,

      /// <summary>
      /// This enumeration defines field name valid for females
      /// </summary>
      NotValidForFemale,

      /// <summary>
      /// This enumeration defines field name unit.
      /// </summary>
      Unit,

      /// <summary>
      /// This enumeration defines field name default numeric value.
      /// </summary>
      DefaultNumericValue,

      /// <summary>
      /// This enumeration defines field name validation lower limit.
      /// </summary>
      ValidationLowerLimit,

      /// <summary>
      /// This enumeration defines field name validation upper limit
      /// </summary>
      ValidationUpperLimit,

      /// <summary>
      /// This enumeration defines field name alert lower limit.
      /// </summary>
      AlertLowerLimit,

      /// <summary>
      /// This enumeration defines field name alert upper limit.
      /// </summary>
      AlertUpperLimit,

      /// <summary>
      /// This enumeration defines field name normal upper limit.
      /// </summary>
      NormalRangeLowerLimit,

      /// <summary>
      /// This enumeration defines field name alert upper limit.
      /// </summary>
      NormalRangeUpperLimit,

      /// <summary>
      /// This enumeration defines field category .
      /// </summary>
      FieldCategory,

      /// <summary>
      /// This enumeration defines form section .
      /// </summary>
      FormSection,

      /// <summary>
      /// This enumeration defines External Selection ListId .
      /// </summary>
      ExSelectionListId,

      /// <summary>
      /// This enumeration defines External Selection List Category.
      /// </summary>
      ExSelectionListCategory,

      /// <summary>
      /// This enumeration defines Analogue Legend Start field .
      /// </summary>
      AnalogueLegendStart,

      /// <summary>
      /// This enumeration defines Analogue Legend Finish field .
      /// </summary>
      AnalogueLegendFinish,

      /// <summary>
      /// This enumeration defines field selection option.
      /// </summary>
      SelectionOptions,

      /// <summary>
      /// This enumeration defines select by coding value.
      /// </summary>
      SelectByCodingValue,

      /// <summary>
      /// This enumeration defines field summary field .
      /// </summary>
      SummaryField,

      /// <summary>
      /// This enumeration defines field MandatoryField field .
      /// </summary>
      Mandatory,

      /// <summary>
      /// This enumeration defines field SafetyReport field .
      /// </summary>
      SafetyReport,

      /// <summary>
      /// This enumeration defines field DataPoint field .
      /// </summary>
      DataPoint,

      /// <summary>
      /// This enumeration defines field hidden.
      /// </summary>
      HideField,

      /// <summary>
      /// This enumeration defines customised field validation.
      /// </summary>
      Java_Script,

      /// <summary>
      /// This enumeration defines Unit Scaling enumeration
      /// </summary>
      UnitScaling,

    }

    /// <summary>
    /// These enumerations define the analogue scale selection
    /// </summary>
    public enum AnalogueScaleOptions
    {
      /// <summary>
      /// This enumeration defines the null option value.
      /// </summary>
      Null,
      /// <summary>
      /// This enumeration defines the 0 to 10 analogue scale.
      /// </summary>
      Zero_to_Ten,
      /// <summary>
      /// This enumeration defines the 0 to 20 analogue scale.
      /// </summary>
      Zero_to_Twenty,
      /// <summary>
      /// This enumeratino defines the 0 to 30 analogue scale.
      /// </summary>
      Zero_to_Forty,
    }
    #endregion

    #region Constants
    /// <summary>
    /// This constant defienes the data collection consent field identifier.
    /// </summary>
    public const string CONST_DATA_CONSENT_FIELD_ID = "CPTC02";
    /// <summary>
    /// This constant defienes the data sharing consent field identifier.
    /// </summary>
    public const string CONST_SHARING_CONSENT_FIELD_ID = "CPTC03";
    /// <summary>
    /// This constant defienes the confirmed consent field identifier.
    /// </summary>
    public const string CONST_CONFIRM_CONSENT_FIELD_ID = "CPTC04";
    /// <summary>
    /// This constant defines the withsrawal consent field identifier.
    /// </summary>
    public const string CONST_WITHDRAW_CONSENT_FIELD_ID = "WCPTC01";
    /// <summary>
    /// This constant defines the withdrawal consent field identifier.
    /// </summary>
    public const string CONST_WITHDRAW_DATA_CONSENT_FIELD_ID = "WCPTC02";
    /// <summary>
    /// This constant defines the withdrawal of sharing consent field identifier.
    /// </summary>
    public const string CONST_WITHDRAW_SHARING_CONSENT_FIELD_ID = "WCPTC03";
    /// <summary>
    /// This constant defines the withrawal confirmation consent field identifier.
    /// </summary>
    public const string CONST_WITHDRAW_CONFIRM_CONSENT_FIELD_ID = "WCPTC04";

    #endregion

    #region Properties

    /// <summary>
    /// This property contains an order of a form field.
    /// </summary>
    public int Order
    {
      get
      {
        return this.Design.Order;
      }
      set
      {
        this.Design.Order = value;
      }
    }

    private string _FieldId = String.Empty;
    /// <summary>
    /// This property contains a field identifier of a form field.
    /// </summary>
    public string FieldId
    {
      get
      {
        return this._FieldId;
      }
      set
      {
        this._FieldId = value;
      }
    }

    /// <summary>
    /// This property contains a subject of a form field.
    /// </summary>
    public string Title
    {
      get
      {
        return this._Design.Title;
      }
    }

    private Evado.Model.EvDataTypes _TypeId = Evado.Model.EvDataTypes.Null;
    /// <summary>
    /// This property contains a type identifier of a form field.
    /// </summary>
    public Evado.Model.EvDataTypes TypeId
    {
      get
      {
        return this._TypeId;
      }
      set
      {
        this._TypeId = value;
        this.Design.TypeId = this._TypeId;

        //
        // if the data point type false turnoff data point output.
        //
        if ( this.isDataPoint == true )
        {
        }
      }
    }

    private bool _StaticField = false;
    /// <summary>
    /// This property indicates if the field is a static field.
    /// These are fields that are associated with form record member values.
    /// e.g. Subject demographic data.
    /// </summary>
    public bool StaticField
    {
      get { return _StaticField; }
      set { _StaticField = value; }
    }

    /// <summary>
    /// This property contains a type of a form field.
    /// </summary>
    public string Type
    {
      get
      {
        return EvcStatics.Enumerations.enumValueToString ( this._TypeId );
      }
    }

    private string _TrialId = String.Empty;
    /// <summary>
    /// This property contains a trial identifier of a form field.
    /// </summary>
    public string TrialId
    {
      get
      {
        return this._TrialId;
      }
      set
      {
        this._TrialId = value;
      }
    }

    private string _RecordId = String.Empty;
    /// <summary>
    /// This property contains a record identifier of a form field.
    /// </summary>
    public string RecordId
    {
      get
      {
        return this._RecordId;
      }
      set
      {
        this._RecordId = value;
      }
    }

    private string _ItemValue = String.Empty;
    /// <summary>
    /// This property contains an item value of a form field.
    /// </summary>
    public string ItemValue
    {
      get
      {
        return this._ItemValue;
      }
      set
      {
        this._ItemValue = value;
      }
    }

    private string _ItemText = String.Empty;
    /// <summary>
    /// This property contains an item text of a form field.
    /// </summary>
    public string ItemText
    {
      get
      {
        return this._ItemText;
      }
      set
      {
        this._ItemText = value;
      }
    }

    /// <summary>
    /// This property contains an item free text of a form field.
    /// </summary>
    public string [ ] ItemFreeText
    {
      get
      {
        if ( this._TypeId == EvDataTypes.Free_Text )
        {
          return this._ItemText.Split ( '\r' );
        }
        return null;
      }
      set
      {
        if ( this._ItemText == String.Empty )
        {
          this._ItemText = EvcStatics.getArrayAsString ( value, "\r\n" );
        }
      }
    }

    private string _QuizAnswers = String.Empty;
    /// <summary>
    /// This property contains a delimited list of QuizAnswers.
    /// </summary>
    public string QuizAnswers
    {
      get
      {
        return this._QuizAnswers;
      }
      set
      {
        this._QuizAnswers = value;
      }
    }

    private List<EvFormRecordComment> _CommentList = new List<EvFormRecordComment> ( );
    /// <summary>
    /// This property contains a comment list of a form field.
    /// </summary>
    public List<EvFormRecordComment> CommentList
    {
      get { return _CommentList; }
      set { _CommentList = value; }
    }

    private EvFormFieldValidationRules _ValidationRules = new EvFormFieldValidationRules ( );
    /// <summary>
    /// This property contains validation rule object of a form field.
    /// </summary>
    public EvFormFieldValidationRules ValidationRules
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

    private EvFormFieldDesign _Design = new EvFormFieldDesign ( );
    /// <summary>
    /// This property contains a design object of a form field.
    /// </summary>
    public EvFormFieldDesign Design
    {
      get
      {
        return this._Design;
      }
      set
      {
        this._Design = value;

      }
    }

    /// <summary>
    /// This property contains a table object of a form field.
    /// </summary>
    public EvFormFieldTable Table { get; set; }

    private FieldStates _State = FieldStates.Null;
    /// <summary>
    /// This property contains a state object of a form field.
    /// </summary>
    public FieldStates State
    {
      get
      {
        return this._State;
      }
      set
      {
        this._State = value;
      }
    }

    /// <summary>
    /// This property contains a global unique identifier of a form field.
    /// </summary>
    public Guid CustomerGuid { get; set; }

    private Guid _Guid = Guid.Empty;
    /// <summary>
    /// This property contains a global unique identifier of a form field.
    /// </summary>
    public Guid Guid
    {
      get
      {
        return this._Guid;
      }
      set
      {
        this._Guid = value;
      }
    }

    private Guid _FormGuid = Guid.Empty;
    /// <summary>
    /// This property contains a form global unique identifier of a form field.
    /// </summary>
    public Guid FormGuid
    {
      get
      {
        return this._FormGuid;
      }
      set
      {
        this._FormGuid = value;
      }
    }

    private Guid _FormFieldGuid = Guid.Empty;
    /// <summary>
    /// This property contains a form field global unique identifier of a form field.
    /// </summary>
    public Guid FormFieldGuid
    {
      get
      {
        return this._FormFieldGuid;
      }
      set
      {
        this._FormFieldGuid = value;
      }
    }

    private Guid _RecordGuid = Guid.Empty;
    /// <summary>
    /// This property contains a record global unique identifier of a form field.
    /// </summary>
    public Guid RecordGuid
    {
      get
      {
        return this._RecordGuid;
      }
      set
      {
        this._RecordGuid = value;
      }
    }

    private string _AuthoredBy = String.Empty;
    /// <summary>
    /// This property contains a user who authors a form field.
    /// </summary>
    public string AuthoredBy
    {
      get
      {
        return this._AuthoredBy;
      }
      set
      {
        this._AuthoredBy = value;
      }
    }

    private DateTime _AuthoredDate = EvcStatics.CONST_DATE_NULL;
    /// <summary>
    /// This property contains an authored date of a form field.
    /// </summary>
    public DateTime AuthoredDate
    {
      get
      {
        return this._AuthoredDate;
      }
      set
      {
        this._AuthoredDate = value;
      }
    }

    /// <summary>
    /// This property contains an authored date string of a form field.
    /// </summary>
    public string stAuthoredDate
    {
      get
      {
        if ( this._AuthoredDate > EvcStatics.CONST_DATE_NULL )
        {
          return this._AuthoredDate.ToString ( "dd MMM yyy" );
        }
        return String.Empty;
      }
    }

    private string _ReviewedBy = String.Empty;
    /// <summary>
    /// This property contains a user who reviews a form field.
    /// </summary>
    public string ReviewedBy
    {
      get
      {
        return this._ReviewedBy;
      }
      set
      {
        this._ReviewedBy = value;
      }
    }

    private DateTime _ReviewedDate = EvcStatics.CONST_DATE_NULL;
    /// <summary>
    /// This property contains a reviewed date of a form field.
    /// </summary>
    public DateTime ReviewedDate
    {
      get
      {
        return this._ReviewedDate;
      }
      set
      {
        this._ReviewedDate = value;
      }
    }

    /// <summary>
    /// This property contains a reviewed date string of a form field.
    /// </summary>
    public string stReviewedDate
    {
      get
      {
        if ( this.ReviewedDate > EvcStatics.CONST_DATE_NULL )
        {
          return this.ReviewedDate.ToString ( "dd MMM yyy" );
        }
        return String.Empty;
      }
    }

    private string _ApprovedBy = String.Empty;
    /// <summary>
    /// This property contains a user who approves a form field.
    /// </summary>
    public string ApprovedBy
    {
      get
      {
        return this._ApprovedBy;
      }
      set
      {
        this._ApprovedBy = value;
      }
    }

    private DateTime _ApprovalDate = EvcStatics.CONST_DATE_NULL;
    /// <summary>
    /// This property contains an approval date of a form field.
    /// </summary>
    public DateTime ApprovalDate
    {
      get
      {
        return this._ApprovalDate;
      }
      set
      {
        this._ApprovalDate = value;
      }
    }

    /// <summary>
    /// This property contains an approval date string of a form field.
    /// </summary>
    public string stApprovalDate
    {
      get
      {
        if ( this._ApprovalDate > EvcStatics.CONST_DATE_NULL )
        {
          return this._ApprovalDate.ToString ( "dd MMM yyy" );
        }
        return String.Empty;
      }
    }

    private string _BookedOutBy = String.Empty;
    /// <summary>
    /// This property contains a user who books out a form field.
    /// </summary>
    public string BookedOutBy
    {
      get
      {
        return this._BookedOutBy;
      }
      set
      {
        this._BookedOutBy = value;
      }
    }

    private string _InitialVersion = String.Empty;
    /// <summary>
    /// This property contains an initial version of a form field.
    /// </summary>
    public string InitialVersion
    {
      get
      {
        return this._InitialVersion;
      }
      set
      {
        this._InitialVersion = value;
      }
    }

    String _cDashMetadata = String.Empty;
    /// <summary>
    /// This property contains cDash metadata values. 
    /// </summary>
    public string cDashMetadata
    {
      get
      {
        return this._cDashMetadata;
      }
      set
      {
        this._cDashMetadata = value;
      }
    }

    /// <summary>
    /// This property returns the linkt text for the field draft command list.
    /// </summary>
    public String LinkText
    {
      get
      {
        String stTitle = String.Format (
            EvLabels.Form_Field_Draft_List_Command_Title,
            this.FieldId,
            this.Title, 
            this.Type,
            this.Order.ToString ( "###" ) );
        if ( this.Design.SelectByCodingValue == true )
        {
          stTitle += EvLabels.Form_Field_Selection_By_Value_Label;
        }

        if ( this.Design.Mandatory == true )
        {
          stTitle += EvLabels.Form_Field_Is_Mandatory_List_Label;
        }

        if ( this.Design.DataPoint == true )
        {
          stTitle += EvLabels.Form_Field_Is_Data_Point_List_Label;
        }

        if ( this.Design.HideField == true )
        {
          stTitle += EvLabels.Form_Field_Is_Hidden_List_Label;
        }

        return stTitle;
      }
    }

    /// <summary>
    /// This property indicated if the field is readonly and set the mandatory field to false.
    /// </summary>
    public bool isReadOnly
    {
      get
      {
        switch ( this._TypeId )
        {
          case Evado.Model.EvDataTypes.Computed_Field:
          case Evado.Model.EvDataTypes.Read_Only_Text:
          case Evado.Model.EvDataTypes.Sound:
          case Evado.Model.EvDataTypes.Hidden:
          case Evado.Model.EvDataTypes.Html_Link:
          case Evado.Model.EvDataTypes.Video:
          case Evado.Model.EvDataTypes.Html_Content:
          case Evado.Model.EvDataTypes.Bar_Chart:
          case Evado.Model.EvDataTypes.Line_Chart:
          case Evado.Model.EvDataTypes.Pie_Chart:
          case Evado.Model.EvDataTypes.Donut_Chart:
          case Evado.Model.EvDataTypes.Stacked_Bar_Chart:
          case Evado.Model.EvDataTypes.Streamed_Video:
          case Evado.Model.EvDataTypes.External_Image:
          case Evado.Model.EvDataTypes.Special_Medication_Summary:
          case Evado.Model.EvDataTypes.Special_Subject_Demographics:
          case Evado.Model.EvDataTypes.Special_Subsitute_Data:
            {
              this._Design.Mandatory = false;
              return true;
            }
        }

        return false;
      }
    }
    /// <summary>
    /// This property indicated if the field is readonly and set the mandatory field to false.
    /// </summary>
    public bool isDataPoint
    {
      get
      {
        switch ( this._TypeId )
        {
          case Evado.Model.EvDataTypes.Read_Only_Text:
          case Evado.Model.EvDataTypes.Sound:
          case Evado.Model.EvDataTypes.Html_Link:
          case Evado.Model.EvDataTypes.Video:
          case Evado.Model.EvDataTypes.Html_Content:
          case Evado.Model.EvDataTypes.Bar_Chart:
          case Evado.Model.EvDataTypes.Line_Chart:
          case Evado.Model.EvDataTypes.Pie_Chart:
          case Evado.Model.EvDataTypes.Donut_Chart:
          case Evado.Model.EvDataTypes.Stacked_Bar_Chart:
          case Evado.Model.EvDataTypes.Streamed_Video:
          case Evado.Model.EvDataTypes.External_Image:
          case Evado.Model.EvDataTypes.Special_Medication_Summary:
          case Evado.Model.EvDataTypes.Special_Subject_Demographics:
          case Evado.Model.EvDataTypes.Special_Subsitute_Data:
            {
              this._Design.DataPoint = false;
              this._Design.SafetyReport = false;
              return false;
            }
        }
        return true;
      }
    }

    /// <summary>
    /// This property indicates if a field value exists.    
    /// </summary>
    public bool hasValue
    {
      get
      {
        if ( this._ItemText != String.Empty )
        {
          return true;
        }
        if ( this._ItemValue != String.Empty )
        {
          return true;
        }
        if ( this.Table != null )
        {
          foreach ( EvFormFieldTableRow row in this.Table.Rows )
          {
            for ( int i = 0; i < 10; i++ )
            {
              if ( this.Table.Header [ i ].TypeId == EvFormFieldTableColumnHeader.ItemTypeMatrix
                || ( this._TypeId == EvDataTypes.Special_Matrix && i == 0 ) )
              {
                continue;
              }

              if ( row.Column [ i ] != String.Empty )
              {
                return true;
              }

            }//END row column iteration loop.

          }//ENd row iteration loop

        }//EMD field table exists.

        return false;
      }
    }

    #region ValidationError

    // =====================================================================================
    /// <summary>
    /// This property formats the item value for display.    
    /// </summary>
    // -------------------------------------------------------------------------------------
    public string ValidationError
    {
      get
      {
        float Value = float.MinValue;

        if ( this._TypeId == EvDataTypes.Numeric )
        {

          if ( float.TryParse ( this._ItemValue, out Value ) == false )
          {
            Value = float.MinValue;
          }

          if ( Value > float.MinValue )
          {
            // 
            // Test Validation Range.
            // 
            if ( Value < this._ValidationRules.ValidationLowerLimit
              || Value > this._ValidationRules.ValidationUpperLimit )
            {
              return "V";
            }

            // 
            // Test for Normal Range
            // 
            if ( Value < this._ValidationRules.NormalRangeLowerLimit
              || Value > this._ValidationRules.NormalRangeUpperLimit )
            {
              return "N";
            }

            // 
            // Test for Alert Range
            // 
            if ( Value < this._ValidationRules.AlertLowerLimit
              || Value > this._ValidationRules.AlertUpperLimit )
            {
              return "A";
            }

          }//NUmeric value
        }
        return "N";
      }
      set
      {
        string Nul = value;
      }
    }
    #endregion

    #region Other field properties
    /// <summary>
    /// This property contains state description of a form field.
    /// </summary>
    public string StateDesc
    {
      get
      {
        return EvcStatics.Enumerations.enumValueToString ( this._State );
      }
      set
      {
        this._State = EvcStatics.Enumerations.parseEnumValue<EvFormField.FieldStates> ( value );
      }
    }

    private string _UpdatedBy = String.Empty;
    /// <summary>
    /// This property contains an updated string of a form field.
    /// </summary>
    public string Updated
    {
      get
      {
        return this._UpdatedBy;
      }
      set
      {
        this._UpdatedBy = value;
      }
    }

    private string _UpdatedByUserId = String.Empty;
    /// <summary>
    /// This property contains a user identifier who updates a form field.
    /// </summary>
    public string UpdatedByUserId
    {
      get
      {
        return this._UpdatedByUserId;
      }
      set
      {
        this._UpdatedByUserId = value;
      }
    }

    private string _ExternalId = String.Empty;
    /// <summary>
    /// This property contains an external identifier of a form field.
    /// </summary>
    public string ExternalId
    {
      get
      {
        return this._ExternalId;
      }
      set
      {
        this._ExternalId = value;
      }
    }

    private string _SubjectId = String.Empty;
    /// <summary>
    /// This property contains a subject identifier of a form field.
    /// </summary>
    public string SubjectId
    {
      get
      {
        return this._SubjectId;
      }
      set
      {
        this._SubjectId = value;
      }
    }

    private string _VisitId = String.Empty;
    /// <summary>
    /// This property contains a visit identifier of a form field.
    /// </summary>
    public string VisitId
    {
      get
      {
        return this._VisitId;
      }
      set
      {
        this._VisitId = value;
      }
    }

    private int _VisitOrder = 0;
    /// <summary>
    /// This property contains visit order of a form field.
    /// </summary>
    public int VisitOrder
    {
      get
      {
        return this._VisitOrder;
      }
      set
      {
        this._VisitOrder = value;
      }
    }

    private string _FormId = String.Empty;
    /// <summary>
    /// This property contains a form identifier of a form field.
    /// </summary>
    public string FormId
    {
      get
      {
        return this._FormId;
      }
      set
      {
        this._FormId = value;
      }
    }

    private int _FormOrder = 0;
    /// <summary>
    /// This property contains a form order of a form field.
    /// </summary>
    public int FormOrder
    {
      get
      {
        return this._FormOrder;
      }
      set
      {
        this._FormOrder = value;
      }
    }

    private string _FormState = String.Empty;
    /// <summary>
    /// This property contains a form state of a form field.
    /// </summary>
    public string FormState
    {
      get
      {
        return this._FormState;
      }
      set
      {
        this._FormState = value;
      }
    }

    private string _FormTitle = String.Empty;
    /// <summary>
    /// This property contains a form title of a form field.
    /// </summary>
    public string FormTitle
    {
      get
      {
        return this._FormTitle;
      }
      set
      {
        this._FormTitle = value;
      }
    }

    /// <summary>
    /// This property contains a user common name of a form field.
    /// </summary>
    public string UserCommonName = String.Empty;

    /// <summary>
    /// This property contains an action of a form field.
    /// </summary>
    public string Action = String.Empty;

    /// <summary>
    /// This property contains a user identifier who authors a form field.
    /// </summary>
    public string AuthoredByUserId = String.Empty;

    /// <summary>
    /// This property contains user identifier who reviews a form field.
    /// </summary>
    public string ReviewedByUserId = String.Empty;

    /// <summary>
    /// This property contains a user identifier who approves a form field.
    /// </summary>
    public string ApprovedByUserId = String.Empty;

    /// <summary>
    /// This property contains IsSelected of a form field.
    /// </summary>
    public bool IsSelected = true;

    /// <summary>
    /// This property indicates whether a form field is visible.
    /// </summary>
    public bool Visible = true;

    #endregion

    #endregion

    #region Format Value code

    // =====================================================================================
    /// <summary>
    /// This class formats the item value for display.
    /// </summary>
    /// <returns>string: format value codes</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Create an item annotation string.
    /// 
    /// 2. Validate whether the typeId is table or matrix
    /// 
    /// 3. Add html table and row script to the item annotation string.
    /// 
    /// 4. Append the string array elements to the arraylist.
    /// 
    /// 5. Loop through an array to add column. 
    /// 
    /// 6. Close the table
    /// 
    /// 7. Return an item annotation string.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public string FormatValueCodes ( )
    {
      //
      // Create an item annotation string.
      //
      string sItemAnnotation = String.Empty;
      // 
      // If a static comment add the comment as a new table
      // 
      if ( this._TypeId == EvDataTypes.Table
        || this._TypeId == EvDataTypes.Special_Matrix )
      {
        if ( this.Design.Options.Length > 0 )
        {
          //
          // Add html table and row to sItemAnnotation string
          //
          sItemAnnotation += "<table class='RecordItemTable' >"
            + "<tr><td class='TableData' style='Text-Align:Left;vertical-align:top;' >";
          string [ ] OptionArray = this.Design.Options.Split ( (char) ';' );
          // 
          // Append the string array elements to the ArrayList.
          // 
          float NewCol = ( OptionArray.Length + 2 ) / 3;

          //
          // Loop through the array to add column.
          //
          for ( int Count = 0; Count < OptionArray.Length; Count++ )
          {
            if ( Count == NewCol || Count == NewCol + NewCol )
            {
              sItemAnnotation += "</td><td class='TableData' style='width:33%;Text-Align:Left;vertical-align:top;' >";
            }
            sItemAnnotation += OptionArray [ Count ] + "<br/>";
          }
          sItemAnnotation += "</td></tr></table>";
        }
      }
      return sItemAnnotation;

    }//END FormatValueCodes method

    #endregion

    #region GroupCodes methods
    // ==================================================================================
    /// <summary>
    /// This class generates an item code based on the retrieved value.
    /// </summary>
    /// <param name="Value">String: a string value</param>
    /// <returns>string: a string item code</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Extract the code if it exists.
    /// 
    /// 2. Split item into string array using ':' as the delimiter.
    /// 
    /// 3. Return the first value in the array.
    ///     
    /// </remarks>
    // ----------------------------------------------------------------------------------
    private string GetItemCode ( String Value )
    {
      // 
      // Extract the code if it exists.
      //
      if ( Value.Contains ( ":" ) == true )
      {
        // 
        // Split the item into a string array using ':' as the delimiter.
        // 
        string [ ] itemCode = Value.Split ( ':' );

        // 
        // Return the first value in the array.
        // 
        return itemCode [ 0 ];
      }

      // 
      // Null return empty string.
      // 
      return String.Empty;
    }//END GetItemCode method.

    /// <summary>
    /// This method returns item description of the string value
    /// </summary>
    /// <param name="Value">string: a string value</param>
    /// <returns>string: a string item description</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Extract the code if it exists.
    /// 
    /// 2. Split item into string array using ':' as the delimiter.
    /// 
    /// 3. Return the first value in the array.
    ///     
    /// </remarks>
    private string GetItemDescription ( string Value )
    {
      // 
      // Extract the code if it exists.
      //
      if ( Value.Contains ( ":" ) == true )
      {
        // 
        // Split the item into a string array using ':' as the delimiter.
        // 
        string [ ] itemCode = Value.Split ( ':' );

        // 
        // Return the first value in the array.
        // 
        return itemCode [ 1 ];
      }

      // 
      // Null return empty string.
      // 
      return String.Empty;
    }
    #endregion

    #region Option List method

    // =====================================================================================
    /// <summary>
    /// This class returns the Yes No Selection list for the item.
    /// </summary>
    /// <returns>ArrayList: a Yes No list</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize a return list and an option object.
    /// 
    /// 2. Add Yes/No options into a return list.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public static ArrayList getYesNoList ( )
    {
      // 
      // Initialise a return list and an option object.
      // 
      ArrayList list = new ArrayList ( );
      EvOption option = new EvOption ( );
      list.Add ( option );

      //
      // Add Yes/No option into a return array list
      //
      option = new EvOption ( "Yes", "Yes" );
      list.Add ( option );
      option = new EvOption ( "No", "No" );
      list.Add ( option );
      // 
      //Return the completed Array List.
      //
      return list;
    }//END getOptionList method

    #endregion

    #region public class methods

    // =====================================================================================
    /// <summary>
    /// This class sets the value on the object. All of the proper validations should be done here.
    /// </summary>
    /// <param name="FieldName">FormFieldClassFieldNames: field of the object to be updated</param>
    /// <param name="Value">String: a string value to be setted</param>
    /// <returns>EvEventCodes: indicating the successful update of the property value</returns>
    /// <remarks>
    /// This class consists of the following steps:
    /// 
    /// 1. Initialize the internal variables.
    /// 
    /// 2. Switch FieldName and update value for the property defined by form field class field name.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes setValue ( FormFieldClassFieldNames FieldName, String Value )
    {
      // 
      // Initialise the internal variables
      // 
      DateTime date = EvcStatics.CONST_DATE_NULL;
      float fltValue = 0;
      int intValue = 0;
      bool boolValue = false;

      // 
      // The switch determines the item to be updated.
      // 
      switch ( FieldName )
      {
        case FormFieldClassFieldNames.TrialId:
          {
            this.TrialId = Value;
            break;
          }

        case FormFieldClassFieldNames.FormId:
          {
            this.Design.FormIds = Value;
            break;
          }

        case FormFieldClassFieldNames.FieldId:
          {
            this.FieldId = Value;
            break;
          }
        case FormFieldClassFieldNames.TypeId:
          {
            EvDataTypes type = EvDataTypes.Null;
            if ( EvcStatics.Enumerations.tryParseEnumValue<EvDataTypes> ( Value, out type ) == false )
            {
              return EvEventCodes.Data_Enumeration_Casting_Error;
            }
            this.TypeId = type;
            break;
          }

        case FormFieldClassFieldNames.Order:
          {
            if ( int.TryParse ( Value, out intValue ) == false )
            {
              return EvEventCodes.Data_Integer_Casting_Error;
            }
            this.Order = intValue;
            break;
          }

        case FormFieldClassFieldNames.Subject:
          {
            this.Design.Title = Value;
            break;
          }

        case FormFieldClassFieldNames.Instructions:
          {
            this.Design.Instructions = Value;
            break;
          }

        case FormFieldClassFieldNames.Reference:
          {
            this.Design.HttpReference = Value;
            break;
          }

        case FormFieldClassFieldNames.Quiz_Value:
          {
            this.Design.QuizValue = Value;
            break;
          }

        case FormFieldClassFieldNames.Quiz_Answer:
          {
            this.Design.QuizAnswer = Value;
            break;
          }

        case FormFieldClassFieldNames.Html_Link:
          {
            this.Design.HttpReference = Value;
            break;
          }

        case FormFieldClassFieldNames.NotValidForMale:
          {
            if ( bool.TryParse ( Value, out boolValue ) == false )
            {
              return EvEventCodes.Data_General_Value_Casting_Error;
            }

            this.ValidationRules.NotValidForMale = !boolValue;
            break;
          }

        case FormFieldClassFieldNames.NotValidForFemale:
          {
            if ( bool.TryParse ( Value, out boolValue ) == false )
            {
              return EvEventCodes.Data_General_Value_Casting_Error;
            }

            this.ValidationRules.NotValidForFemale = !boolValue;
            break;
          }

        case FormFieldClassFieldNames.Unit:
          {
            this.Design.Unit = Value;
            break;
          }

        case FormFieldClassFieldNames.ValidationLowerLimit:
          {
            if ( float.TryParse ( Value, out fltValue ) == false )
            {
              return EvEventCodes.Data_Float_Casting_Error;
            }

            this.ValidationRules.ValidationLowerLimit = fltValue;
            break;
          }

        case FormFieldClassFieldNames.DefaultNumericValue:
          {
            if ( float.TryParse ( Value, out fltValue ) == false )
            {
              return EvEventCodes.Data_Float_Casting_Error;
            }

            this._Design.DefaultValue = fltValue.ToString ( );
            break;
          }

        case FormFieldClassFieldNames.ValidationUpperLimit:
          {
            if ( float.TryParse ( Value, out fltValue ) == false )
            {
              return EvEventCodes.Data_Float_Casting_Error;
            }

            this.ValidationRules.ValidationUpperLimit = fltValue;
            break;
          }

        case FormFieldClassFieldNames.AlertUpperLimit:
          {
            if ( float.TryParse ( Value, out fltValue ) == false )
            {
              return EvEventCodes.Data_Float_Casting_Error;
            }

            this.ValidationRules.AlertUpperLimit = fltValue;
            break;
          }

        case FormFieldClassFieldNames.AlertLowerLimit:
          {
            if ( float.TryParse ( Value, out fltValue ) == false )
            {
              return EvEventCodes.Data_Float_Casting_Error;
            }

            this.ValidationRules.AlertLowerLimit = fltValue;
            break;
          }

        case FormFieldClassFieldNames.NormalRangeUpperLimit:
          {
            if ( float.TryParse ( Value, out fltValue ) == false )
            {
              return EvEventCodes.Data_Float_Casting_Error;
            }

            this.ValidationRules.NormalRangeUpperLimit = fltValue;
            break;
          }

        case FormFieldClassFieldNames.NormalRangeLowerLimit:
          {
            if ( float.TryParse ( Value, out fltValue ) == false )
            {
              return EvEventCodes.Data_Float_Casting_Error;
            }

            this.ValidationRules.NormalRangeLowerLimit = fltValue;
            break;
          }

        case FormFieldClassFieldNames.SelectionOptions:
          {
            this._Design.Options = Value.Replace ( "\r\n", String.Empty );
            break;
          }

        case FormFieldClassFieldNames.FieldCategory:
          {
            this._Design.FieldCategory = Value;
            break;
          }

        case FormFieldClassFieldNames.FormSection:
          {
            this._Design.Section = Value;
            break;
          }

        case FormFieldClassFieldNames.ExSelectionListId:
          {
            this._Design.ExSelectionListId = Value;
            break;
          }

        case FormFieldClassFieldNames.ExSelectionListCategory:
          {
            this._Design.ExSelectionListCategory = Value;
            break;
          }

        case FormFieldClassFieldNames.AnalogueLegendStart:
          {
            this._Design.AnalogueLegendStart = Value;
            break;
          }

        case FormFieldClassFieldNames.AnalogueLegendFinish:
          {
            this._Design.AnalogueLegendFinish = Value;
            break;
          }

        case FormFieldClassFieldNames.SelectByCodingValue:
          {
            this._Design.SelectByCodingValue = EvcStatics.getBool ( Value );
            break;
          }

        case FormFieldClassFieldNames.SummaryField:
          {
            this._Design.SummaryField = EvcStatics.getBool ( Value );
            break;
          }

        case FormFieldClassFieldNames.Mandatory:
          {
            this._Design.Mandatory = EvcStatics.getBool ( Value );
            break;
          }

        case FormFieldClassFieldNames.SafetyReport:
          {
            this._Design.SafetyReport = EvcStatics.getBool ( Value );
            break;
          }

        case FormFieldClassFieldNames.DataPoint:
          {
            this._Design.DataPoint = EvcStatics.getBool ( Value );
            break;
          }

        case FormFieldClassFieldNames.HideField:
          {
            this._Design.HideField = EvcStatics.getBool ( Value );
            break;
          }

        case FormFieldClassFieldNames.Java_Script:
          {
            this._Design.JavaScript = Value;
            break;
          }

        case FormFieldClassFieldNames.UnitScaling:
          {
            this._Design.UnitScaling = Value;
            break;
          }

      }//End switch

      return 0;

    }// End setvalue method.

    #endregion

    #region Public Static Methods

    //  =================================================================================
    /// <summary>
    /// This method returns a list of field data types.
    /// </summary>
    /// <returns>List of EvOption: a list of data types.</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize a return list and an option object.
    /// 
    /// 2. Add items from option object to the return list.
    /// 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public static List<EvOption> getDataTypes ( EvFormRecordTypes FormType )
    {
      //
      // Initialize a return list and an option object.
      //
      List<EvOption> list = new List<EvOption> ( );

      list.Add ( new EvOption ( EvDataTypes.Null.ToString ( ), String.Empty ) );

      //
      // Add data types for the eConsent form type
      //
      /*
      if ( FormType == EvFormRecordTypes.Informed_Consent
        || FormType == EvFormRecordTypes.Informed_Consent_1
        || FormType == EvFormRecordTypes.Informed_Consent_2
        || FormType == EvFormRecordTypes.Informed_Consent_3
        || FormType == EvFormRecordTypes.Informed_Consent_4 )
       */
      if ( FormType == EvFormRecordTypes.Informed_Consent )
      {
        //list.Add ( EvcStatics.Enumerations.getOption ( EvDataTypes.Boolean ) );

        list.Add ( EvStatics.Enumerations.getOption ( EvDataTypes.Yes_No ) );

        list.Add ( EvcStatics.Enumerations.getOption ( EvDataTypes.Read_Only_Text ) );

        list.Add ( EvcStatics.Enumerations.getOption ( EvDataTypes.Check_Box_List ) );

        list.Add ( EvcStatics.Enumerations.getOption ( EvDataTypes.Radio_Button_List ) );

        list.Add ( EvcStatics.Enumerations.getOption ( EvDataTypes.Special_Quiz_Radio_Buttons ) );

        list.Add ( new EvOption ( EvDataTypes.Special_Query_Checkbox, EvLabels.Special_Query_Checkbox_Field_Type_Label ) );

        list.Add ( new EvOption ( EvDataTypes.Special_Query_YesNo, EvLabels.Special_Query_YesNo_Field_Type_Label ) );

        list.Add ( EvcStatics.Enumerations.getOption ( EvDataTypes.Html_Link ) );

        list.Add ( EvcStatics.Enumerations.getOption ( EvDataTypes.Streamed_Video ) );

        list.Add ( EvcStatics.Enumerations.getOption ( EvDataTypes.External_Image ) );

        list.Add ( EvcStatics.Enumerations.getOption ( EvDataTypes.Special_Subsitute_Data ) );

        list.Add ( EvcStatics.Enumerations.getOption ( EvDataTypes.Signature ) );

        list.Add ( EvcStatics.Enumerations.getOption ( EvDataTypes.User_Endorsement ) );

        return list;
      }

      //
      // Add the data types for normal forms.
      //
      list.Add ( EvcStatics.Enumerations.getOption ( EvDataTypes.Boolean ) );

      list.Add ( EvStatics.Enumerations.getOption ( EvDataTypes.Yes_No ) );

      list.Add ( EvcStatics.Enumerations.getOption ( EvDataTypes.Read_Only_Text ) );

      list.Add ( EvcStatics.Enumerations.getOption ( EvDataTypes.Text ) );

      list.Add ( EvcStatics.Enumerations.getOption ( EvDataTypes.Free_Text ) );

      list.Add ( EvcStatics.Enumerations.getOption ( EvDataTypes.Numeric ) );

      list.Add ( EvcStatics.Enumerations.getOption ( EvDataTypes.Integer_Range ) );

      list.Add ( EvcStatics.Enumerations.getOption ( EvDataTypes.Float_Range ) );

      list.Add ( EvcStatics.Enumerations.getOption ( EvDataTypes.Date ) );

      list.Add ( EvcStatics.Enumerations.getOption ( EvDataTypes.Time ) );

      list.Add ( EvcStatics.Enumerations.getOption ( EvDataTypes.Selection_List ) );

      list.Add ( EvcStatics.Enumerations.getOption ( EvDataTypes.Radio_Button_List ) );

      list.Add ( EvcStatics.Enumerations.getOption ( EvDataTypes.Check_Box_List ) );

      //list.Add(EvStatics.Enumerations.getOption ( EvDataTypes.External_Selection_List ));

      list.Add ( EvcStatics.Enumerations.getOption ( EvDataTypes.Html_Link ) );

      list.Add ( EvcStatics.Enumerations.getOption ( EvDataTypes.Computed_Field ) );

      list.Add ( EvcStatics.Enumerations.getOption ( EvDataTypes.Horizontal_Radio_Buttons ) );

      list.Add ( EvcStatics.Enumerations.getOption ( EvDataTypes.Analogue_Scale ) );

      //list.Add ( EvcStatics.Enumerations.getOption ( EvDataTypes.Signature ) );

      //list.Add ( EvcStatics.Enumerations.getOption ( EvDataTypes.User_Endorsement ) );

      list.Add ( EvcStatics.Enumerations.getOption ( EvDataTypes.Table ) );

      list.Add ( EvcStatics.Enumerations.getOption ( EvDataTypes.Special_Matrix ) );

      list.Add ( EvcStatics.Enumerations.getOption ( EvDataTypes.Special_Subsitute_Data ) );

      if ( FormType == EvFormRecordTypes.Serious_Adverse_Event_Report
        || FormType == EvFormRecordTypes.Adverse_Event_Report )
      {
        list.Add ( EvcStatics.Enumerations.getOption ( EvDataTypes.Special_Subject_Demographics ) );

        list.Add ( EvcStatics.Enumerations.getOption ( EvDataTypes.Special_Medication_Summary ) );
      }

      list.Add ( EvcStatics.Enumerations.getOption ( EvDataTypes.Streamed_Video ) );

      list.Add ( EvcStatics.Enumerations.getOption ( EvDataTypes.External_Image ) );

      return list;

    }//END getDataTypes method

    //  =================================================================================
    /// <summary>
    /// This method returns a list of field data types.
    /// </summary>
    /// <param name="IncludeNullSelection">boolean: true, if the selection list include null value</param>
    /// <returns>List of EvOption: a list of analogue scales</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize a return list and an option object.
    /// 
    /// 2. Add items from option object to the return list.
    /// 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public static List<EvOption> getAnalogueScales (
      bool IncludeNullSelection )
    {
      //
      // Initialize a return list and an option object.
      //
      List<EvOption> list = new List<EvOption> ( );

      EvOption Option = new EvOption ( EvDataTypes.Null.ToString ( ),
        String.Empty );
      if ( IncludeNullSelection == true )
      {
        list.Add ( Option );
      }

      //
      // Add items from option object to the return list.
      //
      Option = new EvOption ( AnalogueScaleOptions.Zero_to_Ten.ToString ( ),
        EvcStatics.Enumerations.enumValueToString ( AnalogueScaleOptions.Zero_to_Ten ) );
      list.Add ( Option );

      Option = new EvOption ( AnalogueScaleOptions.Zero_to_Twenty.ToString ( ),
        EvcStatics.Enumerations.enumValueToString ( AnalogueScaleOptions.Zero_to_Twenty ) );
      list.Add ( Option );

      Option = new EvOption ( AnalogueScaleOptions.Zero_to_Forty.ToString ( ),
        EvcStatics.Enumerations.enumValueToString ( AnalogueScaleOptions.Zero_to_Forty ) );
      list.Add ( Option );

      return list;

    }//END getAnalogueScales method

    /// <summary>
    /// this method returns an field object to empty.
    /// </summary>
    /// <returns>EvFormField object</returns>
    public EvFormField Empty ( )
    {
      return new EvFormField ( );
    }

    #endregion

  }//END EvFormField class

}//END namespace Evado.Model.Digital
