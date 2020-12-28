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
  public class EvFormDesign
  {
    #region Class initialisation
    /// <summary>
    /// This class initialation mathod.
    /// </summary>
    public EvFormDesign()
    {
      this._Categories = new String [ 0 ];
      this._Diseases = new String [ 0 ];
      this._TrialDiseases = new String [ 0 ];
    }

    #endregion

    #region Globals and constants.
    /// <summary>
    /// This field define the number of section in a form.
    /// </summary>
    public const int NoSections = 5;

    #endregion

    #region Private members.

    /* These are to includes in both EvForms and EvCommonForms table as new columns */
    private string _TemplateName = String.Empty;
    private EvFormRecordTypes _TypeId = EvFormRecordTypes.Null;//
    private string _JavaValidationScript = String.Empty;
    private string _Title = String.Empty;
    private string _Approval = String.Empty;
    private string _Reference = String.Empty;//
    private string _Instructions = String.Empty;//
    private string _FormCategory = String.Empty;
    private float _Version = 0.0F;//
    private List<EvFormSection> _FormSections = new List<EvFormSection> ( );
    private bool _hasSectionNavigation = false;
    private bool _hasCScript = false;
    private string _HiddenFields = String.Empty;
    private bool _RegulatoryTemplate = false; // If the form is a regulatory form template.
    private bool _HideAnnotationDuringEditing = false; // To hide the annotation field during editing.

    private string _TrialSites = String.Empty; //added because it was present in the xml column.

    /* Theses columns are to be included in the EVSUBJECTS table object */
    private string _SubjectScreeningIdInstructions = String.Empty;
    private string _SubjectSponsorIdInstructions = String.Empty;
    private string _SubjectRandomisedIdInstructions = String.Empty;
    private string _SubjectExternalIdInstructions = String.Empty;

    /*These columns are to be included in the EvCommonForms table object */

    private string _RecordSubjectInstructions = String.Empty;
    private string _StartDateInstructions = String.Empty;
    private string _FinishDateInstructions = String.Empty;
    private EvForm.SubjectIdentifierFieldFormats _SubjectScreeningIdFormat = EvForm.SubjectIdentifierFieldFormats.Text;
    private EvForm.SubjectIdentifierFieldFormats _SubjectSponsorIdFormat = EvForm.SubjectIdentifierFieldFormats.Text;
    private EvForm.SubjectIdentifierFieldFormats _SubjectRandomisedIdFormat = EvForm.SubjectIdentifierFieldFormats.Text;
    private EvForm.SubjectIdentifierFieldFormats _SubjectExternalIdFormat = EvForm.SubjectIdentifierFieldFormats.Text;

    /* These are to includes in both EvForms and EvCommonForms table as new columns */

    /* These members are loaded from memory. */

    #endregion

    #region class property

    #region These members are to includes in both EvForms and EvCommonForms table as new columns */
    /// <summary>
    /// This property contains a java validation script of a form design.
    /// </summary>
    public string JavaValidationScript
    {
      get
      {
        return this._JavaValidationScript;
      }
      set
      {
        this._JavaValidationScript = value;
      }
    }

    /// <summary>
    /// This property contains a template name of a form design.
    /// </summary>
    public string TemplateName
    {
      get
      {
        return this._TemplateName;
      }
      set
      {
        this._TemplateName = value;
      }
    }

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

    /// <summary>
    /// This property contains a reference of a form design.
    /// </summary>
    public string Reference
    {
      get
      {
        return this._Reference;
      }
      set
      {
        this._Reference = value;
      }
    }

    /// <summary>
    /// This property contains a form category of a form design.
    /// </summary>
    public string FormCategory
    {
      get
      {
        return this._FormCategory;
      }
      set
      {
        this._FormCategory = value;
      }
    }

    String _FormLanguage = "en";
    /// <summary>
    /// This property contains a form language setting the default is en (English).
    /// </summary>
    public string FormLanguage
    {
      get
      {
        return this._FormLanguage;
      }
      set
      {
        this._FormLanguage = value;
      }
    }

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
    /// This property contains an approval of a form design.
    /// </summary>
    public string Approval
    {
      get
      {
        return this._Approval;
      }
      set
      {
        this._Approval = value;
      }
    }

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

    /// <summary>
    /// This property contains a type identifier of a form design.
    /// </summary>
    public EvFormRecordTypes TypeId
    {
      get
      {
        return this._TypeId;
      }
      set
      {
        this._TypeId = value;

        if ( ( int ) value < 0 )
        {
          int typeId = ( int ) value;
          this._TypeId = ( EvFormRecordTypes ) ( -typeId );
        }
      }
    }

    /// <summary>
    /// This property contains a type of a form design.
    /// </summary>
    public string Type
    {
      get
      {
        return EvcStatics.Enumerations.enumValueToString ( this._TypeId );
      }
    }

    /// <summary>
    /// This property contains a form section list of a form design.
    /// </summary>
    public List<EvFormSection> FormSections
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

    /// <summary>
    /// This property indicates wheter a form design has section navigation.
    /// </summary>
    public bool hasSectionNavigation
    {
      get
      {
        return this._hasSectionNavigation;
      }
      set
      {
        this._hasSectionNavigation = value;
      }
    }

    /// <summary>
    /// This property indicates whether a form design hides annotation during eiting.
    /// </summary>
    public bool OnEdit_HideFieldAnnotation
    {
      get
      {
        return this._HideAnnotationDuringEditing;
      }
      set
      {
        this._HideAnnotationDuringEditing = value;
      }
    }

    /// <summary>
    /// This property contains a hidden fields of a form design.
    /// </summary>
    public string HiddenFields
    {
      get
      {
        return this._HiddenFields;
      }
      set
      {
        this._HiddenFields = value;
      }
    }

    /// <summary>
    /// This property indicates whether a form design is on regular template.
    /// </summary>
    public bool RegulatoryTemplate
    {
      get
      {
        return this._RegulatoryTemplate;
      }
      set
      {
        this._RegulatoryTemplate = value;
      }
    }

    
    /// <summary>
    /// This property contains the trial site for the form.
    /// </summary>
    public string TrialSites
    {
      get
      {
        return this._TrialSites;
      }
      set
      {
        this._TrialSites = value;
      }
    }
    
    #endregion

    #region Common Form members.
    /// <summary>
    /// This property contains the Record Subject instructions for the 
    /// common record forms.
    /// </summary>
    public string RecordSubjectInstructions
    {
      get
      {
        return this._RecordSubjectInstructions;
      }
      set
      {
        this._RecordSubjectInstructions = value;
      }
    }

    /// <summary>
    /// This property contains the start date instructions for the 
    /// common record forms.
    /// </summary>
    public string StartDateInstructions
    {
      get
      {
        return this._StartDateInstructions;
      }
      set
      {
        this._StartDateInstructions = value;
      }
    }

    /// <summary>
    /// This property contains the finish date instructions for the 
    /// common record forms.
    /// </summary>
    public string FinishDateInstructions
    {
      get
      {
        return this._FinishDateInstructions;
      }
      set
      {
        this._FinishDateInstructions = value;
      }
    }

    /// <summary>
    /// This property contains the screening id instructions for the 
    /// subject record forms.
    /// </summary>
    public string SubjectScreeningIdInstructions
    {
      get
      {
        return this._SubjectScreeningIdInstructions;
      }
      set
      {
        this._SubjectScreeningIdInstructions = value;
      }
    }

    /// <summary>
    /// This property contains the sponsor id instructions for the 
    /// subject record forms.
    /// </summary>
    public string SubjectSponsorIdInstructions
    {
      get
      {
        return this._SubjectSponsorIdInstructions;
      }
      set
      {
        this._SubjectSponsorIdInstructions = value;
      }
    }

    /// <summary>
    /// This property contains the randomised id instructions for the 
    /// subject record forms.
    /// </summary>
    public string SubjectRandomisedIdInstructions
    {
      get
      {
        return this._SubjectRandomisedIdInstructions;
      }
      set
      {
        this._SubjectRandomisedIdInstructions = value;
      }
    }

    /// <summary>
    /// This property contains the external id instructions for the 
    /// subject record forms.
    /// </summary>
    public string SubjectExternalIdInstructions
    {
      get
      {
        return this._SubjectExternalIdInstructions;
      }
      set
      {
        this._SubjectExternalIdInstructions = value;
      }
    }

    /// <summary>
    /// This property contains the screening id format for the 
    /// subject record forms.
    /// </summary>
    public EvForm.SubjectIdentifierFieldFormats SubjectScreeningIdFormat
    {
      get
      {
        return this._SubjectScreeningIdFormat;
      }
      set
      {
        this._SubjectScreeningIdFormat = value;
      }
    }

    /// <summary>
    /// This property contains the sponsor id format for the 
    /// subject record forms.
    /// </summary>
    public EvForm.SubjectIdentifierFieldFormats SubjectSponsorIdFormat
    {
      get
      {
        return this._SubjectSponsorIdFormat;
      }
      set
      {
        this._SubjectSponsorIdFormat = value;
      }
    }

    /// <summary>
    /// This property contains the randomised id format for the 
    /// subject record forms.
    /// </summary>
    public EvForm.SubjectIdentifierFieldFormats SubjectRandomisedIdFormat
    {
      get
      {
        return this._SubjectRandomisedIdFormat;
      }
      set
      {
        this._SubjectRandomisedIdFormat = value;
      }
    }

    /// <summary>
    /// This property contains the external id format for the 
    /// subject record forms.
    /// </summary>
    public EvForm.SubjectIdentifierFieldFormats SubjectExternalIdFormat
    {
      get
      {
        return this._SubjectExternalIdFormat;
      }
      set
      {
        this._SubjectExternalIdFormat = value;
      }
    }
    #endregion

    #region These members are loaded in memory.

    private String [ ] _Categories = new String [ 0 ];
    /// <summary>
    /// This property contains the categories of a form design.
    /// </summary>
    public String [ ] Categories
    {
      get
      {
        return this._Categories;
      }
      set
      {
        this._Categories = value;
      }
    }

    private String [ ] _Diseases = new String [ 0 ];
    /// <summary>
    /// This property contains discseases of a form design.
    /// </summary>
    public String [] Diseases
    {
      get
      {
        return this._Diseases;
      }
      set
      {
        this._Diseases = value;
      }
    }

    private String [ ] _TrialDiseases = new String [ 0 ];
    /// <summary>
    /// This property contains a trail diseacjer of a form design.
    /// </summary>
    public String [ ]  TrialDiseases
    {
      get
      {
        return this._TrialDiseases;
      }
      set
      {
        this._TrialDiseases = value;
      }
    }

    #endregion

    #endregion

    //===================================================================================
    /// <summary>
    /// This method return the selected section.
    /// </summary>
    /// <param name="SectionTitle">String containing the section title.</param>
    /// <returns>EvFormSection object.</returns>
    //-----------------------------------------------------------------------------------
    public EvFormSection getSection ( String SectionTitle )
    {
      SectionTitle = SectionTitle.ToLower ( );

      foreach ( EvFormSection section in this._FormSections )
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
