/***************************************************************************************
 * <copyright file="EvRecord.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvRecord data object.
 *
 ****************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

namespace Evado.Digital.Model
{

  /// <summary>
  /// Business entity used to model accounts
  /// </summary>
  [Serializable]
  public class EdRecordDesign
  {
    public EdRecordDesign ( )
    {
      this.AuthorAccess = EdRecord.AuthorAccessList.Null;
      this.ParentType = EdRecord.ParentTypeList.Null;
      this.ParentEntities = String.Empty;
      this.Description = String.Empty;
      this.Title = String.Empty;
      this.HttpReference = String.Empty;
      this.UpdateReason = EdRecord.UpdateReasonList.Minor_Update;
      this.RecordCategory = String.Empty;
      this.TypeId = EdRecordTypes.Normal_Record;
      this.LinkContentSetting = EdRecord.LinkContentSetting.Default;

    }
    #region Globals and constants.
    /// <summary>
    /// This field define the number of section in a form.
    /// </summary>
    public const int NoSections = 5;

    #endregion

    #region class property

    public bool IsEntity = false;

    /// <summary>
    /// This property contains a title of a form design.
    /// </summary>
    public string Title { get; set; }

    private string _HttpReference = String.Empty;
    /// <summary>
    /// This property contains a reference of a form design.
    /// </summary>
    public string HttpReference { get; set; }

    private string _Instructions = String.Empty;
    /// <summary>
    /// This property contains an instruction of a form design.
    /// </summary>
    public string Instructions
    {
      get
      {
        return this._Instructions;
      }
      set
      {
        this._Instructions = value;

        this._Instructions = this._Instructions.Replace ( "\n", "~" );
        this._Instructions = this._Instructions.Replace ( "\r", "~" );
        this._Instructions = this._Instructions.Replace ( "~~", "~" );
        this._Instructions = this._Instructions.Replace ( "~", " \r\n" );
        this._Instructions = this._Instructions.Replace ( "[b]", "__" );
        this._Instructions = this._Instructions.Replace ( "[/b]", "__" );
        this._Instructions = this._Instructions.Replace ( "[i]", "_" );
        this._Instructions = this._Instructions.Replace ( "[/i]", "_" );
      }
    }

    /// <summary>
    /// This property contains a description of a form containing update notes. 
    /// </summary>
    public string Description { get; set; }


    private EdRecord.UpdateReasonList _UpdateReason = EdRecord.UpdateReasonList.Minor_Update;
    /// <summary>
    /// This property contains an update reason enumerated value 
    /// </summary>
    public EdRecord.UpdateReasonList UpdateReason { get; set; }

    private string _ReadAccessRoles = String.Empty;
    /// <summary>
    /// This property contains a list of read acces roles
    /// </summary>
    public string ReadAccessRoles
    {
      get
      {
        return this._ReadAccessRoles;
      }
      set
      {
        this._ReadAccessRoles = value;

        this.updateReadAccessRole ( );
      }
    }

    private string _EditAccessRoles = String.Empty;
    /// <summary>
    /// This property contains a list of edit acces roles
    /// </summary>
    public string EditAccessRoles
    {
      get
      {
        return this._EditAccessRoles;
      }
      set
      {
        this._EditAccessRoles = value;

        this.updateReadAccessRole ( );
      }
    }

    // ===================================================================================
    /// <summary>
    /// This method updates the read access roles to ensure that all 
    /// edit roles are included.
    /// </summary>
    // ----------------------------------------------------------------------------------
    private void updateReadAccessRole ( )
    {
      //
      // exit if the edit roles are not defined.
      //
      if ( this._EditAccessRoles == String.Empty
        || this._ReadAccessRoles == String.Empty )
      {
        return;
      }

      //
      // create an array of edit roles.
      //
      string [ ] arrEditRole = this._EditAccessRoles.Split ( ';' );

      //
      // Iterate through the edit roles add those that are not in the read access roles.
      //
      for ( int i = 0; i < arrEditRole.Length; i++ )
      {
        if ( this._ReadAccessRoles.Contains ( arrEditRole [ i ] ) == false )
        {
          if ( i > 0 )
          {
            this._ReadAccessRoles += ";";
          }
          this._ReadAccessRoles += arrEditRole [ i ];
        }
      }

    }//END Method.

    /// <summary>
    /// This property indicates that only the author has edit access to the record.
    /// </summary>
    public EdRecord.ParentTypeList ParentType { get; set; }

    /// <summary>
    /// This property indicates that only the author has access to draft of the record.
    /// </summary>
    public EdRecord.AuthorAccessList AuthorAccess { get; set; }

    /// <summary>
    /// This property indicates how the header is to be formatted.
    /// </summary>
    public EdRecord.HeaderFormat HeaderFormat { get; set; }

    /// <summary>
    /// This property indicates how the footer is to be formatted.
    /// </summary>
    public EdRecord.FooterFormat FooterFormat { get; set; }

    private string _ParentEntities = String.Empty;
    /// <summary>
    /// This property contains a list of edit acces roles
    /// </summary>
    public string ParentEntities
    {
      get
      {
        //
        // If the parenttype is not an entity empty the string.
        //
        if ( ParentType != EdRecord.ParentTypeList.Entity )
        {
          _ParentEntities = String.Empty;
        }

        return this._ParentEntities;

      }
      set
      {
        this._ParentEntities = value;
      }
    }

    /// <summary>
    /// This property contains a record id prefix.
    /// </summary>
    public string RecordPrefix { get; set; }

    /// <summary>
    /// This property contains a form approval for display on records.
    /// </summary>
    public string Approval { get; set; }


    /// <summary>
    /// This property contains a form category of a form design.
    /// </summary>
    public string RecordCategory { get; set; }

    /// <summary>
    /// This property contains a type identifier of a form design.
    /// </summary>
    public EdRecordTypes TypeId { get; set; }

    /// <summary>
    /// This property contains the page layout enumerated value 
    /// </summary>
    public string DefaultPageLayout { get; set; }

    /// <summary>
    /// This property indicated whether record summary content is to be displayed in command titles of the page.
    /// </summary>
    public EdRecord.LinkContentSetting LinkContentSetting { get; set; }

    /// <summary>
    /// This property defines how the layout fields are to be displayed when readonly.
    /// </summary>
    public EdRecord.FieldReadonlyDisplayFormats FieldReadonlyDisplayFormat { get; set; }

    /// <summary>
    /// This property indicated whether related entities are to be displayed at the bottom of the page.
    /// </summary>
    public bool DisplayRelatedEntities { get; set; }

    /// <summary>
    /// This property indicated whether the authoer details are to be displayed at the top of the page.
    /// </summary>
    public bool DisplayAuthorDetails { get; set; }

    private List<EdRecordSection> _FormSections = new List<EdRecordSection> ( );
    /// <summary>
    /// This property contains a form section list of a form design.
    /// </summary>
    public List<EdRecordSection> FormSections
    {
      get
      {
        return this._FormSections;
      }
      set
      {
        this._FormSections = value;
      }
    }
    private float _Version = 0.0F;
    /// <summary>
    /// This property contains a version of a form design.
    /// </summary>
    public float Version
    {
      get
      {
        return this._Version;
      }
      set
      {
        this._Version = value;
      }
    }

    /// <summary>
    /// This property contains a version string of a form design.
    /// </summary>
    public string stVersion
    {
      get
      {
        return this._Version.ToString ( "#0.00" );
      }
    }

    private string _JavaScript = String.Empty;
    /// <summary>
    /// This property contains a java validation script of a form design.
    /// </summary>
    public string JavaScript
    {
      get
      {
        return this._JavaScript;
      }
      set
      {
        this._JavaScript = value;
      }
    }


    private bool _hasCScript = false;
    /// <summary>
    /// This property indicates whether a form design has Cs script.
    /// </summary>
    public bool hasCsScript
    {
      get
      {
        return this._hasCScript;
      }
      set
      {
        this._hasCScript = value;
      }
    }


    String _Language = "en";
    /// <summary>
    /// This property contains a form language setting the default is en (English).
    /// </summary>
    public string Language
    {
      get
      {
        return this._Language;
      }
      set
      {
        this._Language = value;
      }
    }



    #endregion

    //===================================================================================
    /// <summary>
    /// This method return the selected section.
    /// </summary>
    /// <param name="SectionTitle">String containing the section title.</param>
    /// <returns>EvFormSection object.</returns>
    //-----------------------------------------------------------------------------------
    public EdRecordSection getSection ( String SectionTitle )
    {
      SectionTitle = SectionTitle.ToLower ( );

      foreach ( EdRecordSection section in this._FormSections )
      {
        String title = section.Title.ToLower ( );

        if ( title == SectionTitle )
        {
          return section;
        }//END selection test

      }//END iteration loop

      return null;

    }//END method

  }//END EvFormDesign class

} // Close namespace Evado.Digital.Model
