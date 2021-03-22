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

namespace Evado.Model.Digital
{
  /// <summary>
  /// This is the data model for the form field selection list class.
  /// </summary>
  [Serializable]
  public class EdPageLayout
  {
    #region Enumerators
    /// <summary>
    /// This enumeration list defines the states of selection list.
    /// </summary>
    public enum FieldNames
    {
      /// <summary>
      /// This enumeration defines null value or not selection state
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines page layout identifier field
      /// </summary>
      PageId,

      /// <summary>
      /// This enumeration defines the page layout title field
      /// </summary>
      Title,

      /// <summary>
      /// This enumeration defines the default home page field
      /// </summary>
      DefaultHomePage,

      /// <summary>
      /// This enumeration defines the page layout User type field
      /// </summary>
      UserType,

      /// <summary>
      /// This enumeration defines the header comment field
      /// </summary>
      HeaderContent,

      /// <summary>
      /// This enumeration defines the header Group list field
      /// </summary>
      HeaderGroupList,

      /// <summary>
      /// This enumeration defines the left column comment field
      /// </summary>
      LeftColumnContent,

      /// <summary>
      /// This enumeration defines the left column Group list field
      /// </summary>
      LeftColumnGroupList,

      /// <summary>
      /// This enumeration defines the left column command lilst field
      /// </summary>
      LeftColumnWidth,

      /// <summary>
      /// This enumeration defines the  center column comment field
      /// </summary>
      CenterColumnContent,

      /// <summary>
      /// This enumeration defines the  center column Group list field
      /// </summary>
      CenterColumnGroupList,

      /// <summary>
      /// This enumeration defines the right column comment field
      /// </summary>
      RightColumnContent,

      /// <summary>
      /// This enumeration defines the right column Group list field
      /// </summary>
      RightColumnGroupList,

      /// <summary>
      /// This enumeration defines the right column command lilst field
      /// </summary>
      RightColumnWidth,

      /// <summary>
      /// This enumeration defines the list title field
      /// </summary>
      Version,

      /// <summary>
      /// This enumeration defines the list title field
      /// </summary>
      State,

    }

    /// <summary>
    /// This enumerated list defines the selection list states.
    /// </summary>
    public enum States
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
      Issue,

      /// <summary>
      /// This enumeration indicated the lsit is withdrawn
      /// </summary>
      Withdraw,
      /// <summary>
      /// This enumeration indicates that list to be deleted.
      /// </summary>
      Delete
    }


    #endregion

    public EdPageLayout ( )
    {
      this.State = States.Null;
      this.Version = 0;
      this.PageId = String.Empty;
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
    public string PageId { get; set; }

    /// <summary>
    /// This property contains a title of selection list
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// This property indicates if this page is the default home page.
    /// </summary>
    public bool DefaultHomePage { get; set; }

    /// <summary>
    /// This property contains an page header content as markdown text.
    /// </summary>
    public string HeaderContent { get; set; }
    /// <summary>
    /// This property contains an page header group list as delimited text.
    /// </summary>
    public string HeaderGroupList { get; set; }

    /// <summary>
    /// This property contains an page LeftColumn content as markdown text.
    /// </summary>
    public string LeftColumnContent { get; set; }
    /// <summary>
    /// This property contains an page header group list as delimited text.
    /// </summary>
    public string LeftColumnGroupList { get; set; }

    private int _LeftColumnWidth = 0;
    /// <summary>
    /// This property contains an page LeftColumn width width as a percentage between 0% - 33%.
    /// </summary>
    public int LeftColumnWidth
    {
      get
      {
        return this._LeftColumnWidth;
      }

      set
      {
        this._LeftColumnWidth = value;

        if ( this._LeftColumnWidth > 33 )
        {
          this._LeftColumnWidth = 33;
        }

        if ( this._LeftColumnWidth < 0 )
        {
          this._LeftColumnWidth = 0;
        }
      }
    }


    /// <summary>
    /// This property contains an page CenterColumn content as markdown text.
    /// </summary>
    public string CenterColumnContent { get; set; }
    /// <summary>
    /// This property contains an page CenterColumn group list as delimited text.
    /// </summary>
    /// 
    public string CenterColumnGroupList { get; set; }

    /// <summary>
    /// This property contains an page RightColumn content as markdown text.
    /// </summary>
    public string RightColumnContent { get; set; }

    /// <summary>
    /// This property contains an page right group list as delimited text.
    /// </summary>
    public string RightColumnGroupList { get; set; }

    private int _RightColumnWidth = 0;
    /// <summary>
    /// This property contains an page RightColumn width as a percentage between 0% - 33%.
    /// </summary>
    public int RightColumnWidth
    {
      get
      {
        return this._RightColumnWidth;
      }

      set
      {
        this._RightColumnWidth = value;

        if ( this._RightColumnWidth > 33 )
        {
          this._RightColumnWidth = 33;
        }

        if ( this._RightColumnWidth < 0 )
        {
          this._RightColumnWidth = 0;
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
    public States State { get; set; }

    /// <summary>
    /// This property contains a reference of selection list
    /// </summary>
    public string UserType { get; set; }

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
    /// This property generates a selection command title text for this page layout.
    /// </summary>
    public string CommandText
    {
      get
      {
        if ( this.DefaultHomePage == true )
        {
          return String.Format (
            EdLabels.PageLayout_Link_Text1,
            this.PageId,
            this.Title,
            this.Version,
            this.State );
        }

        return String.Format (
          EdLabels.PageLayout_Link_Text,
          this.PageId,
          this.Title,
          this.Version,
          this.State );
      }
    }

    /// <summary>
    /// This property generates a selection option object for this page layout.
    /// </summary>
    public EvOption Option
    {
      get
      {
        if ( this.DefaultHomePage == true )
        {
          return new EvOption ( this.PageId,
            String.Format (
            EdLabels.PageLayout_Link_Text1,
            this.PageId,
            this.Title,
            this.Version,
            this.State ) );
        }
        return new EvOption ( this.PageId,
          String.Format (
          EdLabels.PageLayout_Link_Text,
          this.PageId,
          this.Title,
          this.Version,
          this.State ) );
      }
    }
    #endregion

    #region class methods

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
    public void setValue ( EdPageLayout.FieldNames FieldName, string Value )
    {
      //
      // Switch the FieldName and update the Value on the organization field names.
      //
      switch ( FieldName )
      {
        case EdPageLayout.FieldNames.PageId:
          {
            this.PageId = Value;
            return;
          }
        case EdPageLayout.FieldNames.Title:
          {
            this.Title = Value;
            return;
          }
        case EdPageLayout.FieldNames.DefaultHomePage:
          {
            this.DefaultHomePage = EvStatics.getBool ( Value );
            return;
          }
        case EdPageLayout.FieldNames.UserType:
          {
            this.UserType = Value;
            return;
          }
        case EdPageLayout.FieldNames.HeaderContent:
          {
            this.HeaderContent = Value;
            return;
          }
        case EdPageLayout.FieldNames.HeaderGroupList:
          {
            this.HeaderGroupList = Value;
            return;
          }
        case EdPageLayout.FieldNames.LeftColumnContent:
          {
            this.LeftColumnContent = Value;
            return;
          }
        case EdPageLayout.FieldNames.LeftColumnGroupList:
          {
            this.LeftColumnGroupList = Value;
            return;
          }
        case EdPageLayout.FieldNames.LeftColumnWidth:
          {
            this.LeftColumnWidth = EvStatics.getInteger ( Value );
            return;
          }
        case EdPageLayout.FieldNames.CenterColumnContent:
          {
            this.CenterColumnContent = Value;
            return;
          }
        case EdPageLayout.FieldNames.CenterColumnGroupList:
          {
            this.CenterColumnGroupList = Value;
            return;
          }
        case EdPageLayout.FieldNames.RightColumnContent:
          {
            this.RightColumnContent = Value;
            return;
          }
        case EdPageLayout.FieldNames.RightColumnGroupList:
          {
            this.RightColumnGroupList = Value;
            return;
          }
        case EdPageLayout.FieldNames.RightColumnWidth:
          {
            this.RightColumnWidth = EvStatics.getInteger ( Value );
            return;
          }
        default:
          {
            return;
          }
      }//END Switch

    }//END setValue method 
    #endregion

  }//END EdPageLayout class


}//END namespace Evado.Model.Digital 
