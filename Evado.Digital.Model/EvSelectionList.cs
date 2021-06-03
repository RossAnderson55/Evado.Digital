/***************************************************************************************
 * <copyright file="EvFormFieldSelectionList.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvFormFieldSelectionList data object.
 *
 ****************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

namespace Evado.Digital.Model
{
  /// <summary>
  /// This is the data model for the form field selection list class.
  /// </summary>
  [Serializable]
  public class EvSelectionList
  {

    #region CodeItem class

    /// <summary>
    ///  This Xml data class contains the trial objects Xml content.
    /// </summary>
    [Serializable]
    public class Item
    {
      /// <summary>
      ///  Initialise the class object.
      /// </summary>
      public Item ( )
      {
        this.Value = String.Empty;
        this.Description = String.Empty;
        this.Category = String.Empty;
      }

      /// <summary>
      ///  Initialise the class object.
      /// </summary>
      public Item ( int No, String Category, String Value, String Description )
      {
        this.No = No;
        this.Category = Category;
        this.Value = Value;
        this.Description = Description;


      }

      #region class property
      /// <summary>
      /// This property contains a number code item.
      /// </summary>
      public int No { get; set; }

      /// <summary>
      /// This property contains a value of code item.
      /// </summary>
      public string Value { get; set; }

      /// <summary>
      /// This property contains description of code item.
      /// </summary>
      public string Description { get; set; }

      /// <summary>
      /// This property contains a category of code item.
      /// </summary>
      public string Category { get; set; }

      /// <summary>
      /// This property returns the Evado.Model.EvOption object containing the selection options.
      /// </summary>
      public Evado.Model.EvOption GetOption ( bool withCategory )
      {
        if ( withCategory == true
          && Category != String.Empty )
        {
          return new Evado.Model.EvOption ( this.Value, this.Category + " - " + this.Description );
        }

        return new Evado.Model.EvOption ( this.Value, this.Description );
      }

      #endregion

    }//END CodeItem class

    #endregion

    #region Enumerators
    /// <summary>
    /// This enumeration list defines the states of selection list.
    /// </summary>
    public enum SelectionListFieldNames
    {
      /// <summary>
      /// This enumeration defines null value or not selection state
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines list identifier field
      /// </summary>
      ListId,

      /// <summary>
      /// This enumeration defines the list title field
      /// </summary>
      Title,

      /// <summary>
      /// This enumeration defines the list desciption field
      /// </summary>
      Description,

      /// <summary>
      /// This enumeration defines the list title field
      /// </summary>
      Version,

      /// <summary>
      /// This enumeration defines the list title field
      /// </summary>
      State,

      /// <summary>
      /// This enumeration defines the list items field
      /// </summary>
      Items,

      /// <summary>
      /// This enumeration defines the list item No field
      /// </summary>
      Item_No,

      /// <summary>
      /// This enumeration defines the list item value field
      /// </summary>
      Item_Value,

      /// <summary>
      /// This enumeration defines the list item Desciption field
      /// </summary>
      Item_Description,

      /// <summary>
      /// This enumeration defines the list item Category field
      /// </summary>
      Item_Category,

    }

    /// <summary>
    /// This enumerated list defines the selection list states.
    /// </summary>
    public enum SelectionListStates
    {
      /// <summary>
      /// not value is set.
      /// </summary>
      Null,
      /// <summary>
      /// THis enumeration indicates the list is draft
      /// </summary>
      Draft,
      /// <summary>
      /// this enumeration indicates the list is issued.
      /// </summary>
      Issued,
      /// <summary>
      /// This enumeration indicated the lsit is withdrawn
      /// </summary>
      Withdrawn,
    }

    /// <summary>
    /// This enumerated list defines the selection save action.
    /// </summary>
    public enum SaveActions
    {
      /// <summary>
      /// not value is set.
      /// </summary>
      Null = 0,
      /// <summary>
      /// THis enumeration indicates the list is draft
      /// </summary>
      Save = 0,
      /// <summary>
      /// THis enumeration indicates the list is draft
      /// </summary>
      Draft,

      /// <summary>
      /// this enumeration indicates the list is issued.
      /// </summary>
      Issue_List,

      /// <summary>
      /// This enumeration indicated the lsit is withdrawn
      /// </summary>
      Withdraw_List,
      /// <summary>
      /// This enumeration indicates that list to be deleted.
      /// </summary>
      Delete_Object
    }


    #endregion

    public EvSelectionList ( )
    {
      this.State = SelectionListStates.Null;
      this.Version = 0;
      this.ListId = String.Empty;
      this.Title = String.Empty;
    }
    #region class Properties

    /// <summary>
    /// This property contains a global unique identifier of  selection list
    /// </summary>
    public Guid Guid { get; set; }

    /// <summary>
    /// This property contains a list identifier ofselection list
    /// </summary>
    public string ListId { get; set; }

    /// <summary>
    /// This property contains a title of selection list
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// This property contains a reference of selection list
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// This property contains an item list of selection list
    /// </summary>
    public List<Item> Items { get; set; }

    /// <summary>
    /// This method removes the empty items from the list.
    /// </summary>
    public void RemoveEmptyItems ( )
    {
      //
      // remove the empty rows from the list.
      //
      for ( int i = 0; i < this.Items.Count; i++ )
      {
        if ( this.Items [ i ].Value == String.Empty )
        {
          this.Items.RemoveAt ( i );
          i--;
        }
      }
    }

    /// <summary>
    /// This property contains a version of  selection list
    /// </summary>
    public int Version { get; set; }


    /// <summary>
    /// This property contains a state of  selection list
    /// </summary>
    public SelectionListStates State { get; set; }

    /// <summary>
    /// This property contains a save actions
    /// </summary>
    public SaveActions Action { get; set; }

    /// <summary>
    /// This property contains a user who updates a selection list
    /// </summary>
    public string UpdatedBy { get; set; }

    /// <summary>
    /// This property contains a user identifier who updates form field selection list
    /// </summary>
    public string UpdatedByUserId { get; set; }

    /// <summary>
    /// This property contains an updated date of form field selection list
    /// </summary>
    public string UpdatedDate { get; set; }

    /// <summary>
    /// This property returns a list of the selection list options.
    /// </summary>
    public List<Evado.Model.EvOption> CategoryList
    {
      get
      {
        List<Evado.Model.EvOption> optionList = new List<Evado.Model.EvOption> ( );

        if ( this.Items == null )
        {
          this.Items = new List<Item> ( );
        }

        //
        // iterate through the list creating the list categories.
        //
        foreach ( Item item in this.Items )
        {
          Evado.Model.EvOption option = new Evado.Model.EvOption ( item.Category.Trim ( ) );

          if ( optionList.Contains ( option ) == false )
          {
            optionList.Add ( new Evado.Model.EvOption ( item.Category ) );
          }
        }

        //
        // Return category option list.
        //
        return optionList;
      }
    }

    /// <summary>
    /// This property returns the command title content
    /// </summary>
    public string CommandTitle
    {
      get
      {
        return String.Format (
          EdLabels.SelectionLists_LInk_Text,
          this.ListId,
          this.Title,
          this.Version,
          this.State );
      }
    }

    /// <summary>
    /// This property returns the Option list object content
    /// </summary>
    public Evado.Model.EvOption Option
    {
      get
      {
        return new Evado.Model.EvOption ( this.ListId,
          String.Format (
          EdLabels.SelectionLists_LInk_Text,
          this.ListId,
          this.Title,
          this.Version,
          this.State ) );
      }
    }
    #endregion

    #region class methods

    // ==================================================================================
    /// <summary>
    /// This method creates a list of selection options for the passed category.
    /// If category is empty then all options are returned.
    /// </summary>
    /// <param name="Category">String selection option category</param>
    /// <returns>List of Evado.Model.EvOption objects</returns>
    // ----------------------------------------------------------------------------------
    public List<Evado.Model.EvOption> GetOptionList ( String Category, bool ForSelectionList )
    {
      //
      // Initialise the methods variables and objecs.
      //
      List<Evado.Model.EvOption> optionList = new List<Evado.Model.EvOption> ( );

      //
      // Add an empty selection option. 'Null' indicated that nothing is selected.
      //
      if ( ForSelectionList == true )
      {
        optionList.Add ( new Evado.Model.EvOption ( "Null", "" ) );
      }

      //
      // iterate through the list creating the list categories.
      //
      foreach ( Item item in this.Items )
      {
        //
        // skip non matching categories.
        //
        if ( item.Category != Category
          && Category != String.Empty )
        {
          continue;
        }

        //
        // define the option 
        //
        Evado.Model.EvOption option = new Evado.Model.EvOption ( item.Value, item.Description );

        //
        // append the option if it not aready in the list.
        //
        if ( optionList.Contains ( option ) == false )
        {
          optionList.Add ( new Evado.Model.EvOption ( item.Category ) );
        }
      }

      //
      // Return category option list.
      //
      return optionList;
    }

    // ==================================================================================
    /// <summary>
    /// This method sets the field value.
    /// </summary>
    /// <param name="FieldName">EvOrganisation.OrganisationFieldNames: a field name object</param>
    /// <param name="Value">string: a Value for updating</param>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize the internal variables
    /// 
    /// 2. Switch the FieldName and update the Value on the organization field names.
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public void setValue ( EvSelectionList.SelectionListFieldNames FieldName, string Value )
    {
      //
      // Initialize the internal variables
      //
      int intValue = 0;

      //
      // Switch the FieldName and update the Value on the organization field names.
      //
      switch ( FieldName )
      {
        case EvSelectionList.SelectionListFieldNames.ListId:
          {
            this.ListId = Value;
            return;
          }
        case EvSelectionList.SelectionListFieldNames.Title:
          {
            this.Title = Value;
            return;
          }
        case EvSelectionList.SelectionListFieldNames.Description:
          {
            this.Description = Value;
            return;
          }
        default:
          {
            return;
          }
      }//END Switch

    }//END setValue method 
    #endregion

  }//END EdSelectionList class


}//END namespace Evado.Digital.Model 
