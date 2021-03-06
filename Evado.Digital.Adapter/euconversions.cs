﻿/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\Conversions.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c)  2002 - 2021  EVADO HOLDING PTY. LTD..  All rights reserved.
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
 *  This class contains the AbstractedPage ResultData object.
 *
 ****************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Evado.Digital.Bll;
// using Evado.Web;

namespace Evado.Digital.Adapter
{
  /// <summary>
  /// This class contains static form field ResultData conversion method.
  /// </summary>
  public class EuConversions
  {

    private static string _Status = String.Empty;

    public static string Status
    {
      get
      {
        return EuConversions._Status;
      }
    }
    // ===============================================================================
    /// <summary>
    /// This static method converts record type enumerations into the correct application
    /// object enumeration value.
    /// </summary>
    /// <param name="RecordType">EvForm.FormRecordTypes enumeration value.</param>
    /// <returns>ApplicationService.ApplicationObjects objects.</returns>
    // ---------------------------------------------------------------------------------
    public static EuAdapterClasses convertRecordType (
       Evado.Digital.Model.EdRecordTypes RecordType )
    {
      return EuAdapterClasses.Records;
    }

    // ===============================================================================
    /// <summary>
    /// This static method convers list of EvOption objects into a List of Option objects.
    /// </summary>
    /// <param name="EvOptionList">List of EvOption objects.</param>
    /// <param name="AddEmptyOption">True: add an empty option object.</param>
    /// <returns>List of Option objects.</returns>
    // ---------------------------------------------------------------------------------
    public static List<Evado.Model.EvOption> convertEvOptionsList ( List<Evado.Model.EvOption> EvOptionList, bool AddEmptyOption )
    {
      // 
      // Initialise the method variables and objects.
      // 
      List<Evado.Model.EvOption> optionList = new List<Evado.Model.EvOption> ( );
      if ( AddEmptyOption == true )
      {
        optionList.Add ( new Evado.Model.EvOption ( ) );
      }

      // 
      // Iterate through the EV option list.
      // 
      foreach ( Evado.Model.EvOption option in EvOptionList )
      {
        if ( option.Value != String.Empty )
        {
          optionList.Add ( new Evado.Model.EvOption ( option.Value, option.Description ) );
        }

      }

      // 
      // return the device option list.
      // 
      return optionList;

    }//END static convertEvOptionsList method

    // =================================================================================
    /// <summary>
    ///  This static method convert a list of EvFormField into a list of ClientClientDataObjectFields
    /// </summary>
    /// <param name="FormRecordFields">The list of EvFormField objects.</param>
    /// <param name="Section">The EvFormSection defining the section to be displayed.</param>
    /// <param name="EditStatus">Evado.UniForm.Model.EditAccess defining the field edit status.</param>
    /// <returns>ClientClientDataObjectGorup object</returns>
    // ---------------------------------------------------------------------------------
    public static Evado.UniForm.Model.Group convertListOfEvFormFields (
      List< Evado.Digital.Model.EdRecordField> FormRecordFields,
        Evado.Digital.Model.EdRecordSection Section,
      Evado.UniForm.Model.EditAccess EditStatus )
    {
      return convertListOfEvFormFields (
        FormRecordFields,
        Section,
        EditStatus,
        Evado.UniForm.Model.GroupLayouts.Full_Width );
    }

    // =================================================================================
    /// <summary>
    ///  This static method convert a list of EvFormField into a list of ClientClientDataObjectFields
    /// </summary>
    /// <param name="FormRecordFields">The list of EvFormField objects.</param>
    /// <param name="Section">The EvFormSection defining the section to be displayed.</param>
    /// <param name="EditStatus">Evado.UniForm.Model.EditAccess defining the field edit status.</param>
    /// <param name="GroupLayout">Evado.UniForm.Model.GroupLayouts defining the pageMenuGroup layout setting.</param>
    /// <returns>ClientClientDataObjectGorup object</returns>
    // ---------------------------------------------------------------------------------
    public static Evado.UniForm.Model.Group convertListOfEvFormFields (
      List< Evado.Digital.Model.EdRecordField> FormRecordFields,
       Evado.Digital.Model.EdRecordSection Section,
      Evado.UniForm.Model.EditAccess EditStatus,
      Evado.UniForm.Model.GroupLayouts GroupLayout )
    {
      EuConversions._Status = "Evado.UniForm.Clinical.Conversions.convertListOfEvFormFields method "
        + " Field Count: " + FormRecordFields.Count
        + " Section: " + Section
        + " EditStatus: " + EditStatus;

      // 
      // Initialise the method variables and objects.
      // 
      List<Evado.UniForm.Model.Field> pageFieldList = new List<Evado.UniForm.Model.Field> ( );
      Evado.UniForm.Model.Group pageGroup = new Evado.UniForm.Model.Group (
        Section.Title,
        String.Empty,
        EditStatus );
      pageGroup.Layout = GroupLayout;

      // 
      // If null create empty object.
      // 
      if ( Section == null )
      {
        Section = new  Evado.Digital.Model.EdRecordSection ( );
      }

      // 
      // if section is empty no sections on the list.
      // 
      if ( Section.Title != String.Empty )
      {
        EuConversions._Status += "\r\nSection No: " + Section.No + ", Section: " + Section.Title;
        pageGroup = new Evado.UniForm.Model.Group (
          Section.Title,
          Section.Instructions,
          Evado.UniForm.Model.EditAccess.Inherited );
        pageGroup.Layout = Evado.UniForm.Model.GroupLayouts.Full_Width;
      }
      // 
      // Iterate through the EV option list.
      // 
      foreach (  Evado.Digital.Model.EdRecordField field in FormRecordFields )
      {
        EuConversions._Status += "\r\nField: " + field.FieldId + ", Sectn: " + field.Design.SectionNo;

        // 
        // IF the section is empty then there are no sections.
        // Process the fields in field order.
        // 
        if ( Section.Title == String.Empty )
        {
          Evado.UniForm.Model.Field pageField = convertEvFormfield ( field );

          if ( pageField != null )
          {
            EuConversions._Status += " >> NO SECTION FIELD CONVERTED";

            pageGroup.FieldList.Add ( pageField );
          }
        }//END no section
        else
        {
          // 
          // generate page fields for the fields in the selected section.
          // 
          if ( field.Design.SectionNo == Section.No )
          {
            EuConversions._Status = " >> SECTION SELECTED";
            Evado.UniForm.Model.Field pageField = convertEvFormfield ( field );

            if ( pageField != null )
            {
              EuConversions._Status = " >> FIELD CONVERTED";
              pageGroup.FieldList.Add ( pageField );
            }
          }//END section exists
        }

      }//END Form field iteration loop.

      // 
      // return the device option list.
      // 
      return pageGroup;

    }//END static convertEvOptionsList method

    // =================================================================================
    /// <summary>
    ///  This static method convert a list of EvFormField into a list of ClientClientDataObjectFields
    /// </summary>
    /// <param name="FormField">The list of EvFormField objects.</param>
    /// <returns>List of ClientClientDataObjectFields</returns>
    // ---------------------------------------------------------------------------------
    public static Evado.UniForm.Model.Field convertEvFormfield (  Evado.Digital.Model.EdRecordField FormField )
    {
      // 
      // Initialise the method variables and objects.
      // 
      Evado.UniForm.Model.Field pageField = new Evado.UniForm.Model.Field (
        FormField.FieldId,
        FormField.Title,
        Evado.Model.EvDataTypes.Null );
      pageField.Layout = Evado.UniForm.Model.FieldLayoutCodes.Left_Justified;

      if ( FormField.Design.Instructions != String.Empty )
      {
        pageField.Description =  FormField.Design.Instructions ;
      }

      switch ( FormField.TypeId )
      {
        case Evado.Model.EvDataTypes.Text:
          {
            pageField.Type = Evado.Model.EvDataTypes.Text;
            pageField.Value = FormField.ItemValue;
            pageField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Width, "50" );
            break;
          }
        case Evado.Model.EvDataTypes.Free_Text:
          {
            pageField.Type = Evado.Model.EvDataTypes.Free_Text;
            pageField.Value = FormField.ItemText;
            pageField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Width, "50" );
            pageField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Height, "5" );
            break;
          }
        case Evado.Model.EvDataTypes.Date:
          {
            pageField.Type = Evado.Model.EvDataTypes.Date;
            pageField.Value = FormField.ItemValue;
            pageField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Width, "12" );
            break;
          }
        case Evado.Model.EvDataTypes.Time:
          {
            pageField.Type = Evado.Model.EvDataTypes.Time;
            pageField.Value = FormField.ItemValue;
            pageField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Width, "6" );
            break;
          }
        case Evado.Model.EvDataTypes.Numeric:
          {
            pageField.Type = Evado.Model.EvDataTypes.Numeric;
            pageField.Value = FormField.ItemValue;

            pageField.AddParameter ( 
              Evado.UniForm.Model.FieldParameterList.Min_Value, 
              FormField.Design.ValidationLowerLimit.ToString ( ) );

            pageField.AddParameter ( 
              Evado.UniForm.Model.FieldParameterList.Max_Value, 
              FormField.Design.ValidationUpperLimit.ToString ( ) );

            pageField.AddParameter ( 
              Evado.UniForm.Model.FieldParameterList.Width, "10" );
            break;
          }
        case Evado.Model.EvDataTypes.Selection_List:
        case Evado.Model.EvDataTypes.External_Selection_List:
          {
            pageField.Type = Evado.Model.EvDataTypes.Selection_List;
            pageField.Value = FormField.ItemValue;
            pageField.OptionList = EuConversions.convertEvOptionsList ( FormField.Design.OptionList, true );
            pageField.AddParameter ( 
              Evado.UniForm.Model.FieldParameterList.Width, "50" );
            break;
          }
        case Evado.Model.EvDataTypes.Radio_Button_List:
          // case Evado.Model.EvDataTypes.Horizontal_Radio_Buttons:
          {
            pageField.Type = Evado.Model.EvDataTypes.Radio_Button_List;
            pageField.Value = FormField.ItemValue;
            pageField.OptionList = EuConversions.convertEvOptionsList ( FormField.Design.OptionList, true );
            pageField.AddParameter ( 
              Evado.UniForm.Model.FieldParameterList.Width, "50" );
            break;
          }
        case Evado.Model.EvDataTypes.Boolean:
          {
            pageField.Type = Evado.Model.EvDataTypes.Boolean;
            pageField.Value = FormField.ItemValue;
            pageField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Width, "50" );
            break;
          }
        case Evado.Model.EvDataTypes.Check_Box_List:
          {
            pageField.Type = Evado.Model.EvDataTypes.Check_Box_List;
            pageField.Value = FormField.ItemValue;
            pageField.OptionList = EuConversions.convertEvOptionsList ( FormField.Design.OptionList, true );
            pageField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Width, "50" );
            break;
          }
        case Evado.Model.EvDataTypes.Analogue_Scale:
        case Evado.Model.EvDataTypes.Horizontal_Radio_Buttons:
          {
            pageField.Type = Evado.Model.EvDataTypes.Read_Only_Text;
            pageField.Value = FormField.ItemValue;
            pageField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Width, "50" );
            break;
          }
        case Evado.Model.EvDataTypes.Table:
        case Evado.Model.EvDataTypes.Special_Matrix:
          {
            pageField = generateTableField ( FormField );
            break;
          }
        default:
          {
            pageField = null;
            break;
          }

      }

      // 
      // return the device option list.
      // 
      return pageField;

    }//END static convertEvOptionsList method

    // =================================================================================
    /// <summary>
    ///  This static method convert a list of EvFormField into a list of ClientClientDataObjectFields
    /// </summary>
    /// <param name="FormField">The list of EvFormField objects.</param>
    /// <returns>List of ClientClientDataObjectFields</returns>
    // ---------------------------------------------------------------------------------
    private static Evado.UniForm.Model.Field generateTableField (  Evado.Digital.Model.EdRecordField FormField )
    {
      // 
      // Initialise the method variables and objects.
      // 
      Evado.UniForm.Model.Field groupField = new Evado.UniForm.Model.Field (
        FormField.FieldId,
        FormField.Title,
       Evado.Model.EvDataTypes.Table );

      // 
      // Initialise the field object.
      // 
      if ( FormField.Design.Instructions != String.Empty )
      {
        groupField.Description =  FormField.Design.Instructions ;
      }
      groupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Width, "100" );
      groupField.EditAccess = Evado.UniForm.Model.EditAccess.Inherited;
      groupField.Type = Evado.Model.EvDataTypes.Table;
      groupField.Table = new Evado.UniForm.Model.Table ( );

      // 
      // Initialise the table header
      // 
      if ( FormField.Table != null )
      {
        for ( int column = 0; column < FormField.Table.Header.Length; column++ )
        {
          groupField.Table.Header [ column ].No = FormField.Table.Header [ column ].No;
          groupField.Table.Header [ column ].Text = FormField.Table.Header [ column ].Text;
          groupField.Table.Header [ column ].TypeId = FormField.Table.Header [ column ].TypeId;
          groupField.Table.Header [ column ].Width = FormField.Table.Header [ column ].Width;

          // 
          // Proces the Options or Unit field value.
          // 
          if ( groupField.Table.Header [ column ].TypeId == Evado.Model.EvDataTypes.Numeric )
          {
            groupField.Table.Header [ column ].OptionsOrUnit = FormField.Table.Header [ column ].OptionsOrUnit;
          }
          if ( groupField.Table.Header [ column ].TypeId == Evado.Model.EvDataTypes.Radio_Button_List
            || groupField.Table.Header [ column ].TypeId == Evado.Model.EvDataTypes.Selection_List )
          {
            groupField.Table.Header [ column ].OptionList = Evado.UniForm.Model.EuStatics.getStringAsOptionList (
              FormField.Table.Header [ column ].OptionsOrUnit );
          }

        }//END Column interation loop

        // 
        // transfer the table values to the pagefield table object.
        // 
        for ( int inRow = 0; inRow < FormField.Table.Rows.Count; inRow++ )
        {
          Evado.UniForm.Model.TableRow row = new Evado.UniForm.Model.TableRow ( );

          for ( int inColumn = 0; inColumn < FormField.Table.ColumnCount; inColumn++ )
          {
            row.Column [ inColumn ] = FormField.Table.Rows [ inRow ].Column [ inColumn ];
          }
          groupField.Table.Rows.Add ( row );
        }
      }

      // 
      // returnt the page field object.
      // 
      return groupField;

    }//END generateTableField method.

  }//END class

}//END Name space
