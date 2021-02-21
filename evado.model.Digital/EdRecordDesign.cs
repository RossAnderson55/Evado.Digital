/***************************************************************************************
 * <copyright file="EvRecord.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvRecord data object.
 *
 ****************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

namespace Evado.Model.Digital
{

  /// <summary>
  /// Business entity used to model accounts
  /// </summary>
  [Serializable]
  public class EdRecordDesign
  {
    #region Globals and constants.
    /// <summary>
    /// This field define the number of section in a form.
    /// </summary>
    public const int NoSections = 5;

    #endregion

    #region class property


    private string _Title = String.Empty;
    /// <summary>
    /// This property contains a title of a form design.
    /// </summary>
    public string Title
    {
      get
      {
        return this._Title;
      }
      set
      {
        this._Title = value;
      }
    }

    private string _HttpReference = String.Empty;
    /// <summary>
    /// This property contains a reference of a form design.
    /// </summary>
    public string HttpReference
    {
      get
      {
        return this._HttpReference;
      }
      set
      {
        this._HttpReference = value;
      }
    }
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

    private string _Description = String.Empty;
    /// <summary>
    /// This property contains a description of a form containing update notes. 
    /// </summary>
    public string Description
    {
      get
      {
        return this._Description;
      }
      set
      {
        this._Description = value;
      }
    }


    private EdRecord.UpdateReasonList _UpdateReason = EdRecord.UpdateReasonList.Minor_Update;
    /// <summary>
    /// This property contains an update reason enumerated value 
    /// </summary>
    public EdRecord.UpdateReasonList UpdateReason
    {
      get
      {
        return this._UpdateReason;
      }
      set
      {
        this._UpdateReason = value;
      }
    }

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
      }
    }

    /// <summary>
    /// This property indicates that only the author has edit access to the record.
    /// </summary>
    public EdRecord.ParentTypeList ParentType { get; set; }

    /// <summary>
    /// This property indicates that only the author has access to draft of the record.
    /// </summary>
    public EdRecord.AuthorAccessList AuthorAccess { get; set; }

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
    
    private string _RecordCategory = String.Empty;
    /// <summary>
    /// This property contains a form category of a form design.
    /// </summary>
    public string RecordCategory
    {
      get
      {
        return this._RecordCategory;
      }
      set
      {
        this._RecordCategory = value;
      }
    }
    
    private EdRecordTypes _TypeId = EdRecordTypes.Null;
    /// <summary>
    /// This property contains a type identifier of a form design.
    /// </summary>
    public EdRecordTypes TypeId
    {
      get
      {
        return this._TypeId;
      }
      set
      {
        this._TypeId = value;
      }
    }

    /// <summary>
    /// This property contains the page layout enumerated value 
    /// </summary>
    public object DefaultPageLayout { get; set; }

    private EdRecord.LinkContentSetting _LinkContentSetting = EdRecord.LinkContentSetting.Default;
    /// <summary>
    /// This property indicated whether record summary content is to be displayed in command titles of the page.
    /// </summary>
    public EdRecord.LinkContentSetting LinkContentSetting
    {
      get
      {
        return this._LinkContentSetting;
      }
      set
      {
        this._LinkContentSetting = value;
      }
    }

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

} // Close namespace Evado.Model.Digital
