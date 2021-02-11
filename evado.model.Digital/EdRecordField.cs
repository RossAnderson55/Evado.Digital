/***************************************************************************************
 * <copyright file="EdRecordFields.cs" company="EVADO HOLDING PTY. LTD.">
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
  public partial class EdRecordField : EvHasSetValue<EdRecordField.FieldClassFieldNames>
  {
    #region Class Enumerators

    /// <summary>
    /// This enumeration list defines the field names of form field class.
    /// </summary>
    public enum FieldClassFieldNames
    {
      /// <summary>
      /// This enumeration defines null value or no selection state.
      /// </summary>
      Null,

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
      /// This enumeration defines field name quiz correct answer description.
      /// </summary>
      Html_Link,

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
      Selection_Options,

      /// <summary>
      /// This enumeration defines field summary field .
      /// </summary>
      Summary_Field,

      /// <summary>
      /// This enumeration defines field MandatoryField field .
      /// </summary>
      Mandatory,

      /// <summary>
      /// This enumeration defines field DataPoint field .
      /// </summary>
      AI_Data_Point,

      /// <summary>
      /// This enumeration defines field hidden.
      /// </summary>
      Hide_Field,

      /// <summary>
      /// This enumeration defines customised field validation.
      /// </summary>
      Java_Script,

      /// <summary>
      /// This enumeration defines Unit Scaling enumeration
      /// </summary>
      Unit_Scaling,

    }

    #endregion

    #region Properties

    /// <summary>
    /// This property contains a global unique identifier of a form field.
    /// </summary>
    public Guid CustomerGuid { get; set; }

    /// <summary>
    /// This property contains a global unique identifier of a form field.
    /// </summary>
    public Guid Guid { get; set; }

    /// <summary>
    /// This property contains a form global unique identifier of a form field.
    /// </summary>
    public Guid LayoutGuid { get; set; }

    /// <summary>
    /// This property contains a form field global unique identifier of a form field.
    /// </summary>
    public Guid RecordFieldGuid { get; set; }

    /// <summary>
    /// This property contains a record global unique identifier of a form field.
    /// </summary>
    public Guid RecordGuid { get; set; }

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

    /// <summary>
    /// This property contains a field identifier of a form field.
    /// </summary>
    public string LayoutId { get; set; }

    /// <summary>
    /// This property contains a field identifier of a form field.
    /// </summary>
    public string FieldId { get; set; }

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

    /// <summary>
    /// This property contains a type identifier of a form field.
    /// </summary>
    public Evado.Model.EvDataTypes TypeId
    {
      get
      {
        return this.Design.TypeId;
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

    private EdRecordFieldDesign _Design = new EdRecordFieldDesign ( );
    /// <summary>
    /// This property contains a design object of a form field.
    /// </summary>
    public EdRecordFieldDesign Design
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
    public EdRecordTable Table { get; set; }


    /// <summary>
    /// This property returns the linkt text for the field draft command list.
    /// </summary>
    public String LinkText
    {
      get
      {
        String stTitle = String.Format (
            Evado.Model.Digital.EdLabels.Form_Field_Draft_List_Command_Title,
            this.FieldId,
            this.Title, 
            EvStatics.getEnumStringValue( this.TypeId ),
            this.Order.ToString ( "###" ) );

        if ( this.Design.Mandatory == true )
        {
          stTitle += Evado.Model.Digital.EdLabels.Form_Field_Is_Mandatory_List_Label;
        }

        if ( this.Design.AiDataPoint == true )
        {
          stTitle += Evado.Model.Digital.EdLabels.Form_Field_Is_Data_Point_List_Label;
        }

        if ( this.Design.HideField == true )
        {
          stTitle += Evado.Model.Digital.EdLabels.Form_Field_Is_Hidden_List_Label;
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
        switch ( this.TypeId )
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
        switch ( this.TypeId )
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
          case Evado.Model.EvDataTypes.Special_Subsitute_Data:
            {
              this._Design.AiDataPoint = false;
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
          foreach ( EdRecordTableRow row in this.Table.Rows )
          {
            for ( int i = 0; i < 10; i++ )
            {
              if ( this.Table.Header [ i ].TypeId == EvDataTypes.Special_Matrix
                || ( this.TypeId == EvDataTypes.Special_Matrix && i == 0 ) )
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

        if ( this.TypeId == EvDataTypes.Numeric )
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
            if ( Value < this._Design.ValidationLowerLimit
              || Value > this._Design.ValidationUpperLimit )
            {
              return "V";
            }

            // 
            // Test for Normal Range
            // 
            if ( Value < this._Design.NormalRangeLowerLimit
              || Value > this._Design.NormalRangeUpperLimit )
            {
              return "N";
            }

            // 
            // Test for Alert Range
            // 
            if ( Value < this._Design.AlertLowerLimit
              || Value > this._Design.AlertUpperLimit )
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
    public static List<EvOption> getYesNoList ( )
    {
      // 
      // Initialise a return list and an option object.
      // 
      List<EvOption> list = new List<EvOption> ( );
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
    public EvEventCodes setValue ( FieldClassFieldNames FieldName, String Value )
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
        case FieldClassFieldNames.FieldId:
          {
            this.FieldId = Value;
            break;
          }
        case FieldClassFieldNames.TypeId:
          {
            EvDataTypes type = EvDataTypes.Null;
            if ( EvcStatics.Enumerations.tryParseEnumValue<EvDataTypes> ( Value, out type ) == false )
            {
              return EvEventCodes.Data_Enumeration_Casting_Error;
            }
            this.Design.TypeId = type;
            break;
          }

        case FieldClassFieldNames.Order:
          {
            if ( int.TryParse ( Value, out intValue ) == false )
            {
              return EvEventCodes.Data_Integer_Casting_Error;
            }
            this.Order = intValue;
            break;
          }

        case FieldClassFieldNames.Subject:
          {
            this.Design.Title = Value;
            break;
          }

        case FieldClassFieldNames.Instructions:
          {
            this.Design.Instructions = Value;
            break;
          }

        case FieldClassFieldNames.Reference:
          {
            this.Design.HttpReference = Value;
            break;
          }


        case FieldClassFieldNames.Html_Link:
          {
            this.Design.HttpReference = Value;
            break;
          }

        case FieldClassFieldNames.Unit:
          {
            this.Design.Unit = Value;
            break;
          }

        case FieldClassFieldNames.ValidationLowerLimit:
          {
            if ( float.TryParse ( Value, out fltValue ) == false )
            {
              return EvEventCodes.Data_Float_Casting_Error;
            }

            this._Design.ValidationLowerLimit = fltValue;
            break;
          }

        case FieldClassFieldNames.DefaultNumericValue:
          {
            if ( float.TryParse ( Value, out fltValue ) == false )
            {
              return EvEventCodes.Data_Float_Casting_Error;
            }

            this._Design.DefaultValue = fltValue.ToString ( );
            break;
          }

        case FieldClassFieldNames.ValidationUpperLimit:
          {
            if ( float.TryParse ( Value, out fltValue ) == false )
            {
              return EvEventCodes.Data_Float_Casting_Error;
            }

            this._Design.ValidationUpperLimit = fltValue;
            break;
          }

        case FieldClassFieldNames.AlertUpperLimit:
          {
            if ( float.TryParse ( Value, out fltValue ) == false )
            {
              return EvEventCodes.Data_Float_Casting_Error;
            }

            this._Design.AlertUpperLimit = fltValue;
            break;
          }

        case FieldClassFieldNames.AlertLowerLimit:
          {
            if ( float.TryParse ( Value, out fltValue ) == false )
            {
              return EvEventCodes.Data_Float_Casting_Error;
            }

            this._Design.AlertLowerLimit = fltValue;
            break;
          }

        case FieldClassFieldNames.NormalRangeUpperLimit:
          {
            if ( float.TryParse ( Value, out fltValue ) == false )
            {
              return EvEventCodes.Data_Float_Casting_Error;
            }

            this._Design.NormalRangeUpperLimit = fltValue;
            break;
          }

        case FieldClassFieldNames.NormalRangeLowerLimit:
          {
            if ( float.TryParse ( Value, out fltValue ) == false )
            {
              return EvEventCodes.Data_Float_Casting_Error;
            }

            this._Design.NormalRangeLowerLimit = fltValue;
            break;
          }

        case FieldClassFieldNames.Selection_Options:
          {
            this._Design.Options = Value.Replace ( "\r\n", String.Empty );
            break;
          }

        case FieldClassFieldNames.FieldCategory:
          {
            this._Design.FieldCategory = Value;
            break;
          }

        case FieldClassFieldNames.FormSection:
          {
            this._Design.SectionNo = EvStatics.getInteger( Value );
            break;
          }

        case FieldClassFieldNames.ExSelectionListId:
          {
            this._Design.ExSelectionListId = Value;
            break;
          }

        case FieldClassFieldNames.ExSelectionListCategory:
          {
            this._Design.ExSelectionListCategory = Value;
            break;
          }

        case FieldClassFieldNames.AnalogueLegendStart:
          {
            this._Design.AnalogueLegendStart = Value;
            break;
          }

        case FieldClassFieldNames.AnalogueLegendFinish:
          {
            this._Design.AnalogueLegendFinish = Value;
            break;
          }

        case FieldClassFieldNames.Summary_Field:
          {
            this._Design.SummaryField = EvcStatics.getBool ( Value );
            break;
          }

        case FieldClassFieldNames.Mandatory:
          {
            this._Design.Mandatory = EvcStatics.getBool ( Value );
            break;
          }

        case FieldClassFieldNames.AI_Data_Point:
          {
            this._Design.AiDataPoint = EvcStatics.getBool ( Value );
            break;
          }

        case FieldClassFieldNames.Hide_Field:
          {
            this._Design.HideField = EvcStatics.getBool ( Value );
            break;
          }

        case FieldClassFieldNames.Java_Script:
          {
            this._Design.JavaScript = Value;
            break;
          }

        case FieldClassFieldNames.Unit_Scaling:
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
    public static List<EvOption> getDataTypes ( EdRecordTypes FormType )
    {
      //
      // Initialize a return list and an option object.
      //
      List<EvOption> list = new List<EvOption> ( );

      list.Add ( new EvOption ( EvDataTypes.Null.ToString ( ), String.Empty ) );

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

      list.Add ( EvcStatics.Enumerations.getOption ( EvDataTypes.Streamed_Video ) );

      list.Add ( EvcStatics.Enumerations.getOption ( EvDataTypes.External_Image ) );

      return list;

    }//END getDataTypes method

    /// <summary>
    /// this method returns an field object to empty.
    /// </summary>
    /// <returns>EvFormField object</returns>
    public EdRecordField Empty ( )
    {
      return new EdRecordField ( );
    }

    #endregion

  }//END EvFormField class

}//END namespace Evado.Model.Digital
