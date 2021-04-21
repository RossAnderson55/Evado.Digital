/***************************************************************************************
 * <copyright file="EdRecordFields.cs" company="EVADO HOLDING PTY. LTD.">
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
  public partial class EdRecordField : EvHasSetValue<EdRecordField.ClassFieldNames>
  {
    #region Class Enumerators

    /// <summary>
    /// This enumeration list defines the field names of form field class.
    /// </summary>
    public enum ClassFieldNames
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

      /// <summary>
      /// This enumeration defines field layout enumeration
      /// </summary>
      Field_Layout,

      /// <summary>
      /// This enumeration defines the text field width identifier.
      /// </summary>,
      FieldWidth,

      /// <summary>
      /// This enumeration defines the text field height identifier.
      /// </summary>
      FieldHeight,

      /// <summary>
      /// This enumeration defines the media URL identifier.
      /// </summary>
      Media_Url,

      /// <summary>
      /// This enumeration defines the media title identifier.
      /// </summary>
      Media_Title,

      /// <summary>
      /// This enumeration defines the media height identifier.
      /// </summary>
      Media_Height,

      /// <summary>
      /// This enumeration defines the media width identifier.
      /// </summary>
      Media_Width,
    }

    #endregion

    #region Constants

    public const string CONST_CATEGORY_AUTI_FIELD_IDENTIFIER = "AUTO:";

    #endregion

    #region Properties

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
    public Guid FieldGuid { get; set; }

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
    /// This property contains the external media configuration parameters.
    /// </summary>
    public EdRecordMedia RecordMedia { get; set; } 

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
            EvStatics.getEnumStringValue ( this.TypeId ),
            this.Order.ToString ( "###" ) );

        if ( this.Design.Mandatory == true )
        {
          stTitle += Evado.Model.Digital.EdLabels.Form_Field_Is_Mandatory_List_Label;
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
    public bool isSingleValue
    {
      get
      {
        switch ( this.TypeId )
        {
          case Evado.Model.EvDataTypes.Analogue_Scale:
          case Evado.Model.EvDataTypes.Boolean:
          case Evado.Model.EvDataTypes.Computed_Field:
          case Evado.Model.EvDataTypes.Currency:
          case Evado.Model.EvDataTypes.Date:
          case Evado.Model.EvDataTypes.Email_Address:
          case Evado.Model.EvDataTypes.Hidden:
          case Evado.Model.EvDataTypes.Horizontal_Radio_Buttons:
          case Evado.Model.EvDataTypes.Integer:
          case Evado.Model.EvDataTypes.Numeric:
          case Evado.Model.EvDataTypes.Radio_Button_List:
          case Evado.Model.EvDataTypes.Selection_List:
          case Evado.Model.EvDataTypes.External_RadioButton_List:
          case Evado.Model.EvDataTypes.External_Selection_List:
          case Evado.Model.EvDataTypes.Text:
          case Evado.Model.EvDataTypes.Telephone_Number:
          case Evado.Model.EvDataTypes.Time:
          case Evado.Model.EvDataTypes.Yes_No:
            {
              return true;
            }
        }

        return false;
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
          case Evado.Model.EvDataTypes.Binary_File:
          case Evado.Model.EvDataTypes.Sound:
          case Evado.Model.EvDataTypes.Hidden:
          case Evado.Model.EvDataTypes.Video:
          case Evado.Model.EvDataTypes.Html_Content:
          case Evado.Model.EvDataTypes.Bar_Chart:
          case Evado.Model.EvDataTypes.Line_Chart:
          case Evado.Model.EvDataTypes.Pie_Chart:
          case Evado.Model.EvDataTypes.Donut_Chart:
          case Evado.Model.EvDataTypes.Stacked_Bar_Chart:
          case Evado.Model.EvDataTypes.External_Image:
          case Evado.Model.EvDataTypes.Special_Document:
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
        switch ( this.TypeId )
        {
          case Evado.Model.EvDataTypes.Read_Only_Text:
          case Evado.Model.EvDataTypes.Sound:
          case Evado.Model.EvDataTypes.Http_Link:
          case Evado.Model.EvDataTypes.Video:
          case Evado.Model.EvDataTypes.Image:
          case Evado.Model.EvDataTypes.Html_Content:
          case Evado.Model.EvDataTypes.Bar_Chart:
          case Evado.Model.EvDataTypes.Line_Chart:
          case Evado.Model.EvDataTypes.Pie_Chart:
          case Evado.Model.EvDataTypes.Donut_Chart:
          case Evado.Model.EvDataTypes.Stacked_Bar_Chart:
          case Evado.Model.EvDataTypes.Streamed_Video:
          case Evado.Model.EvDataTypes.External_Image:
          case Evado.Model.EvDataTypes.Special_Document:
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
    public EvEventCodes setValue ( ClassFieldNames FieldName, String Value )
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
        case ClassFieldNames.FieldId:
          {
            this.FieldId = Value;
            break;
          }
        case ClassFieldNames.TypeId:
          {
            EvDataTypes type = EvDataTypes.Null;
            if ( EvStatics.tryParseEnumValue<EvDataTypes> ( Value, out type ) == false )
            {
              return EvEventCodes.Data_Enumeration_Casting_Error;
            }
            this.Design.TypeId = type;
            break;
          }

        case ClassFieldNames.Order:
          {
            if ( int.TryParse ( Value, out intValue ) == false )
            {
              return EvEventCodes.Data_Integer_Casting_Error;
            }
            this.Order = intValue;
            break;
          }

        case ClassFieldNames.Subject:
          {
            this.Design.Title = Value;
            break;
          }

        case ClassFieldNames.Instructions:
          {
            this.Design.Instructions = Value;
            break;
          }

        case ClassFieldNames.Reference:
          {
            this.Design.HttpReference = Value;
            break;
          }


        case ClassFieldNames.Html_Link:
          {
            this.Design.HttpReference = Value;
            break;
          }

        case ClassFieldNames.Unit:
          {
            this.Design.Unit = Value;
            break;
          }

        case ClassFieldNames.ValidationLowerLimit:
          {
            if ( float.TryParse ( Value, out fltValue ) == false )
            {
              return EvEventCodes.Data_Float_Casting_Error;
            }

            this._Design.ValidationLowerLimit = fltValue;
            break;
          }

        case ClassFieldNames.DefaultNumericValue:
          {
            if ( float.TryParse ( Value, out fltValue ) == false )
            {
              return EvEventCodes.Data_Float_Casting_Error;
            }

            this._Design.DefaultValue = fltValue.ToString ( );
            break;
          }

        case ClassFieldNames.ValidationUpperLimit:
          {
            if ( float.TryParse ( Value, out fltValue ) == false )
            {
              return EvEventCodes.Data_Float_Casting_Error;
            }

            this._Design.ValidationUpperLimit = fltValue;
            break;
          }

        case ClassFieldNames.AlertUpperLimit:
          {
            if ( float.TryParse ( Value, out fltValue ) == false )
            {
              return EvEventCodes.Data_Float_Casting_Error;
            }

            this._Design.AlertUpperLimit = fltValue;
            break;
          }

        case ClassFieldNames.AlertLowerLimit:
          {
            if ( float.TryParse ( Value, out fltValue ) == false )
            {
              return EvEventCodes.Data_Float_Casting_Error;
            }

            this._Design.AlertLowerLimit = fltValue;
            break;
          }

        case ClassFieldNames.NormalRangeUpperLimit:
          {
            if ( float.TryParse ( Value, out fltValue ) == false )
            {
              return EvEventCodes.Data_Float_Casting_Error;
            }

            this._Design.NormalRangeUpperLimit = fltValue;
            break;
          }

        case ClassFieldNames.NormalRangeLowerLimit:
          {
            if ( float.TryParse ( Value, out fltValue ) == false )
            {
              return EvEventCodes.Data_Float_Casting_Error;
            }

            this._Design.NormalRangeLowerLimit = fltValue;
            break;
          }

        case ClassFieldNames.Selection_Options:
          {
            this._Design.Options = Value.Replace ( "\r\n", String.Empty );
            break;
          }

        case ClassFieldNames.FieldCategory:
          {
            this._Design.FieldCategory = Value;
            break;
          }

        case ClassFieldNames.FormSection:
          {
            this._Design.SectionNo = EvStatics.getInteger ( Value );
            break;
          }

        case ClassFieldNames.ExSelectionListId:
          {
            this._Design.ExSelectionListId = Value;
            break;
          }

        case ClassFieldNames.ExSelectionListCategory:
          {
            this._Design.ExSelectionListCategory = Value;
            break;
          }

        case ClassFieldNames.AnalogueLegendStart:
          {
            this._Design.AnalogueLegendStart = Value;
            break;
          }

        case ClassFieldNames.AnalogueLegendFinish:
          {
            this._Design.AnalogueLegendFinish = Value;
            break;
          }

        case ClassFieldNames.Summary_Field:
          {
            this._Design.IsSummaryField = EvcStatics.getBool ( Value );
            break;
          }

        case ClassFieldNames.Mandatory:
          {
            this._Design.Mandatory = EvcStatics.getBool ( Value );
            break;
          }

        case ClassFieldNames.AI_Data_Point:
          {
            this._Design.AiDataPoint = EvcStatics.getBool ( Value );
            break;
          }

        case ClassFieldNames.Hide_Field:
          {
            this._Design.HideField = EvcStatics.getBool ( Value );
            break;
          }

        case ClassFieldNames.Java_Script:
          {
            this._Design.JavaScript = Value;
            break;
          }

        case ClassFieldNames.Unit_Scaling:
          {
            this._Design.UnitScaling = Value;
            break;
          }

        case ClassFieldNames.Field_Layout:
          {
            this._Design.FieldLayout = Value;
            break;
          }

        case ClassFieldNames.FieldWidth:
          {
            if ( int.TryParse ( Value, out intValue ) == false )
            {
              return EvEventCodes.Data_Integer_Casting_Error;
            }
            this.Design.FieldWidth = intValue;
            break;
          }
        case ClassFieldNames.FieldHeight:
          {
            if ( int.TryParse ( Value, out intValue ) == false )
            {
              return EvEventCodes.Data_Integer_Casting_Error;
            }
            this.Design.FieldHeight = intValue;
            break;
          }

        case ClassFieldNames.Media_Url:
          {
            this.RecordMedia.Url = Value;
            break;
          }


        case ClassFieldNames.Media_Title:
          {
            this.RecordMedia.Title = Value;
            break;
          }
        case ClassFieldNames.Media_Width:
          {
            if ( int.TryParse ( Value, out intValue ) == false )
            {
              return EvEventCodes.Data_Integer_Casting_Error;
            }
            this.RecordMedia.Width = intValue;
            break;
          }
        case ClassFieldNames.Media_Height:
          {
            if ( int.TryParse ( Value, out intValue ) == false )
            {
              return EvEventCodes.Data_Integer_Casting_Error;
            }
            this.RecordMedia.Height = intValue;
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
    public static List<EvOption> getDataTypes ( )
    {
      //
      // Initialize a return list and an option object.
      //
      List<EvOption> list = new List<EvOption> ( );

      list.Add ( new EvOption ( EvDataTypes.Null.ToString ( ), String.Empty ) );

      //
      // Add the data types for normal forms.
      //
      list.Add ( EvStatics.getOption ( EvDataTypes.Boolean ) );

      list.Add ( EvStatics.getOption ( EvDataTypes.Yes_No ) );

      list.Add ( EvStatics.getOption ( EvDataTypes.Read_Only_Text ) );

      list.Add ( EvStatics.getOption ( EvDataTypes.Text ) );

      list.Add ( EvStatics.getOption ( EvDataTypes.Free_Text ) );

      list.Add ( EvStatics.getOption ( EvDataTypes.Numeric ) );

      list.Add ( EvStatics.getOption ( EvDataTypes.Date ) );

      list.Add ( EvStatics.getOption ( EvDataTypes.Time ) );

      list.Add ( EvStatics.getOption ( EvDataTypes.Selection_List ) );

      list.Add ( EvStatics.getOption ( EvDataTypes.Radio_Button_List ) );

      list.Add ( EvStatics.getOption ( EvDataTypes.Check_Box_List ) );

      list.Add ( EvStatics.getOption ( EvDataTypes.External_Selection_List ) );

      list.Add ( EvStatics.getOption ( EvDataTypes.External_RadioButton_List ) );

      list.Add ( EvStatics.getOption ( EvDataTypes.External_CheckBox_List ) );

      list.Add ( EvStatics.getOption ( EvDataTypes.Integer_Range ) );

      list.Add ( EvStatics.getOption ( EvDataTypes.Float_Range ) );

      list.Add ( EvStatics.getOption ( EvDataTypes.Date_Range ) );

      list.Add ( EvStatics.getOption ( EvDataTypes.Image ) );

      list.Add ( EvStatics.getOption ( EvDataTypes.Http_Link ) );

      list.Add ( EvStatics.getOption ( EvDataTypes.Computed_Field ) );

      list.Add ( EvStatics.getOption ( EvDataTypes.Horizontal_Radio_Buttons ) );

      list.Add ( EvStatics.getOption ( EvDataTypes.Analogue_Scale ) );

      list.Add ( EvStatics.getOption ( EvDataTypes.Name ) );

      list.Add ( EvStatics.getOption ( EvDataTypes.Address ) );

      list.Add ( EvStatics.getOption ( EvDataTypes.Telephone_Number ) );

      list.Add ( EvStatics.getOption ( EvDataTypes.Email_Address ) );

      //list.Add ( EvStatics.getOption ( EvDataTypes.Signature ) );

      //list.Add ( EvStatics.getOption ( EvDataTypes.User_Endorsement ) );

      list.Add ( EvStatics.getOption ( EvDataTypes.Table ) );

      list.Add ( EvStatics.getOption ( EvDataTypes.Special_Matrix ) );

      list.Add ( EvStatics.getOption ( EvDataTypes.Special_Subsitute_Data ) );

      list.Add ( EvStatics.getOption ( EvDataTypes.Streamed_Video ) );

      list.Add ( EvStatics.getOption ( EvDataTypes.External_Image ) );

      return list;

    }//END getDataTypes method

    #endregion

  }//END EvFormField class

}//END namespace Evado.Model.Digital
