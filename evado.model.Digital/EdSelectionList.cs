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
  public class EdSelectionList
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
      /// This enumeration defines the list items field
      /// </summary>
      Items,
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

    public EdSelectionList ( )
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
    public List<EvOption> CategoryList
    {
      get
      {
        List<EvOption> optionList = new List<EvOption> ( );

        if ( this.Items == null )
        {
          this.Items = new List<Item> ( );
        }

        //
        // iterate through the list creating the list categories.
        //
        foreach ( Item item in this.Items )
        {
          EvOption option = new EvOption ( item.Category.Trim ( ) );

          if ( optionList.Contains ( option ) == false )
          {
            optionList.Add ( new EvOption ( item.Category ) );
          }
        }

        //
        // Return category option list.
        //
        return optionList;
      }
    }

    public string LinkText
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
    #endregion

    #region class methods

    // ==================================================================================
    /// <summary>
    /// This method creates a list of selection options for the passed category.
    /// If category is empty then all options are returned.
    /// </summary>
    /// <param name="Category">String selection option category</param>
    /// <returns>List of EvOption objects</returns>
    // ----------------------------------------------------------------------------------
    public List<EvOption> GetOptionList ( String Category )
    {
      //
      // Initialise the methods variables and objecs.
      //
      List<EvOption> optionList = new List<EvOption> ( );

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
        EvOption option = new EvOption ( item.Value, item.Description );

        //
        // append the option if it not aready in the list.
        //
        if ( optionList.Contains ( option ) == false )
        {
          optionList.Add ( new EvOption ( item.Category ) );
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
    public void setValue ( EdSelectionList.SelectionListFieldNames FieldName, string Value )
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
        case EdSelectionList.SelectionListFieldNames.ListId:
          {
            this.ListId = Value;
            return;
          }
        case EdSelectionList.SelectionListFieldNames.Title:
          {
            this.Title = Value;
            return;
          }
        case EdSelectionList.SelectionListFieldNames.Description:
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


}//END namespace Evado.Model.Digital 
