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
  public class EdPageLayout : Evado.Model.EvParameters
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
      /// This enumeration defines the display main menu field
      /// </summary>
      DisplayMainMenu,

      /// <summary>
      /// This enumeration defines the default home page field
      /// </summary>
      DefaultHomePage,

      /// <summary>
      /// The page components on this layout.
      /// </summary>
      LayoutComponents,

      /// <summary>
      /// This enumeration defines the Menu location field
      /// </summary>
      MenuLocation,

      /// <summary>
      /// This enumeration defines the page command list field
      /// </summary>
      PageCommands,

      /// <summary>
      /// This enumeration defines the page layout User type field
      /// </summary>
      User_Types,

      /// <summary>
      /// This enumeration defines the header comment field
      /// </summary>
      HeaderContent,

      /// <summary>
      /// This enumeration defines the header Group list field
      /// </summary>
      HeaderComponentList,

      /// <summary>
      /// This enumeration defines the left column comment field
      /// </summary>
      LeftColumnContent,

      /// <summary>
      /// This enumeration defines the left column Group list field
      /// </summary>
      LeftColumnComponentList,

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
      RightColumnComponentList,

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
    /// This enumerated list defines the menu location on the page.
    /// </summary>
    public enum LayoutComponentList
    {
      /// <summary>
      /// not value is set, menu not displayed.
      /// </summary>
      Null,

      /// <summary>
      /// THis enumeration the menu is displayed in the page header.
      /// </summary>
      Page_Header,

      /// <summary>
      /// this enumeration the menu is displayed in the left column
      /// </summary>
      Left_Column,

      /// <summary>
      /// This enumeration the menu is displayed in the right column
      /// </summary>
      Right_Column,
    }

    /// <summary>
    /// This enumerated list defines the menu location on the page.
    /// </summary>
    public enum MenuLocations
    {
      /// <summary>
      /// not value is set, menu not displayed.
      /// </summary>
      Null,
      /// <summary>
      /// THis enumeration the menu is displayed in the page header.
      /// </summary>
      Page_Menu,
      /// <summary>
      /// this enumeration the menu is displayed in the left column
      /// </summary>
      Left_Column,
      /// <summary>
      /// this enumeration the menu is displayed in the top in center column.
      /// </summary>
      Top_Center,
      /// <summary>
      /// This enumeration the menu is displayed in the right column
      /// </summary>
      Right_Column,
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
    public bool HomePage { get; set; }

    /// <summary>
    /// This property contains the HTTP reference link for the application.
    /// </summary>
    public bool DisplayMainMenu
    {
      get
      {
        return
         EvStatics.getBool ( this.getParameter ( EdPageLayout.FieldNames.DisplayMainMenu ) );
      }
      set
      {
        this.setParameter ( EdPageLayout.FieldNames.DisplayMainMenu,
          EvDataTypes.Boolean, value.ToString ( ) );
      }
    }

    /// <summary>
    /// This property page components on this page.
    /// </summary>
    public String LayoutComponents
    {
      get
      {
        return
          this.getParameter ( EdPageLayout.FieldNames.LayoutComponents );
      }
      set
      {
        this.setParameter ( EdPageLayout.FieldNames.LayoutComponents,
          EvDataTypes.Text, value.ToString ( ) );
      }
    }

    // ==================================================================================
    /// <summary>
    /// This method returns true if the component is an active page component.
    /// </summary>
    /// <param name="Component">LayoutComponentList enumerated list.</param>
    /// <returns>True: component found</returns>
    // ----------------------------------------------------------------------------------
    public bool hasActiveLayout ( LayoutComponentList Component )
    {
      String component = Component.ToString();

      if ( LayoutComponents.Contains( component ) == true )
      {
        return true;
      }
       return false;
    }


    /// <summary>
    /// This property contains the enumerated value to define where the menu is to be located.
    /// </summary>
    public MenuLocations MenuLocation { get; set; }

    /// <summary>
    /// This property contains an page command enumerated ';' list. 
    /// </summary>
    public string PageCommands { get; set; }

    /// <summary>
    /// This property contains an page header content as markdown text.
    /// </summary>
    public string HeaderContent { get; set; }
    /// <summary>
    /// This property contains an page header group list as delimited text.
    /// </summary>
    public string HeaderComponentList { get; set; }

    /// <summary>
    /// This property contains an page LeftColumn content as markdown text.
    /// </summary>
    public string LeftColumnContent { get; set; }
    /// <summary>
    /// This property contains an page header group list as delimited text.
    /// </summary>
    public string LeftColumnComponentList { get; set; }

    private Int16 _LeftColumnWidth = 0;
    /// <summary>
    /// This property contains an page LeftColumn width width as a percentage between 0% - 33%.
    /// </summary>
    public Int16 LeftColumnWidth
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
    public string CenterColumnComponentList { get; set; }

    /// <summary>
    /// This property contains an page RightColumn content as markdown text.
    /// </summary>
    public string RightColumnContent { get; set; }

    /// <summary>
    /// This property contains an page right group list as delimited text.
    /// </summary>
    public string RightColumnComponentList { get; set; }

    private Int16 _RightColumnWidth = 0;
    /// <summary>
    /// This property contains an page RightColumn width as a percentage between 0% - 33%.
    /// </summary>
    public Int16 RightColumnWidth
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
    public string UserTypes { get; set; }

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
        if ( this.HomePage == true )
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
        if ( this.HomePage == true )
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
            this.HomePage = EvStatics.getBool ( Value );
            return;
          }
        case EdPageLayout.FieldNames.DisplayMainMenu:
          {
            this.setParameter ( EdPageLayout.FieldNames.DisplayMainMenu,
              EvDataTypes.Boolean, Value.ToString ( ) );

            return;
          }


        case EdPageLayout.FieldNames.MenuLocation:
          {
            this.MenuLocation = EvStatics.parseEnumValue<MenuLocations> ( Value );
            return;
          }

        case EdPageLayout.FieldNames.LayoutComponents:
          {
            this.setParameter ( EdPageLayout.FieldNames.LayoutComponents,
              EvDataTypes.Text, Value );
            return;
          }


        case EdPageLayout.FieldNames.PageCommands:
          {
            this.PageCommands = Value;
            return;
          }
        case EdPageLayout.FieldNames.User_Types:
          {
            this.UserTypes = Value;
            return;
          }
        case EdPageLayout.FieldNames.HeaderContent:
          {
            this.HeaderContent = Value;
            return;
          }
        case EdPageLayout.FieldNames.HeaderComponentList:
          {
            this.HeaderComponentList = Value;
            return;
          }
        case EdPageLayout.FieldNames.LeftColumnContent:
          {
            this.LeftColumnContent = Value;
            return;
          }
        case EdPageLayout.FieldNames.LeftColumnComponentList:
          {
            this.LeftColumnComponentList = Value;
            return;
          }
        case EdPageLayout.FieldNames.LeftColumnWidth:
          {
            this.LeftColumnWidth = EvStatics.getShort ( Value );
            return;
          }
        case EdPageLayout.FieldNames.CenterColumnContent:
          {
            this.CenterColumnContent = Value;
            return;
          }
        case EdPageLayout.FieldNames.CenterColumnGroupList:
          {
            this.CenterColumnComponentList = Value;
            return;
          }
        case EdPageLayout.FieldNames.RightColumnContent:
          {
            this.RightColumnContent = Value;
            return;
          }
        case EdPageLayout.FieldNames.RightColumnComponentList:
          {
            this.RightColumnComponentList = Value;
            return;
          }
        case EdPageLayout.FieldNames.RightColumnWidth:
          {
            this.RightColumnWidth = EvStatics.getShort ( Value );
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
